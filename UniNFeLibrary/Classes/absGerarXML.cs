using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using UniNFeLibrary.Enums;
using System.Xml;

namespace UniNFeLibrary
{
    /// <summary>
    /// Classe abstrata para gerar os XML´s da nota fiscal eletrônica
    /// </summary>
    public abstract class absGerarXML
    {
        #region Atributos
        /// <summary>
        /// Atributo que vai receber a string do XML de lote de NFe´s para que este conteúdo seja gravado após finalizado em arquivo físico no HD
        /// </summary>
        protected string strXMLLoteNfe;
        /// <summary>     
        /// Nome do arquivo para controle da numeração sequencial do lote.
        /// </summary>
        private string NomeArqXmlLote = InfoApp.PastaExecutavel() + "\\UniNfeLote.xml";
        #endregion

        #region Propriedades
        /// <summary>
        /// Nome do arquivo XML que está sendo enviado para os webservices
        /// </summary>
        public string NomeXMLDadosMsg { get; set; }
        /// <summary>
        /// Serviço que está sendo executado (Envio de NFE, Cancelamento, consultas, etc...)
        /// </summary>
        public Servicos Servico { get; set; }
        #endregion

        #region Objetos
        protected Auxiliar oAux = new Auxiliar();
        #endregion

        #region Métodos

        #region Métodos para gerar o Lote de Notas Fiscais Eletrônicas

        #region LoteNfe()
        /// <summary>
        /// Gera o Lote das Notas Fiscais passada por parâmetro na pasta de envio
        /// </summary>
        /// <param name="lstArquivosNFe">Lista dos XML´s de Notas Fiscais a serem gerados os lotes</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        public void LoteNfe(List<string> lstArquivosNFe)
        {
            try
            {
                bool booLiberado = false;
                //Vamos verificar se todos os XML´s estão disponíveis
                for (int i = 0; i < lstArquivosNFe.Count; i++)
                {
                    booLiberado = false;
                    //Verificar se consegue abrir o arquivo em modo exclusivo
                    if (!Auxiliar.FileInUse(lstArquivosNFe[i]))
                    {
                        booLiberado = true;

                        Thread.Sleep(100);
                    }
                }

                if (booLiberado)
                {
                    //Buscar o número do lote a ser utilizado
                    Int32 intNumeroLote = 0;

                    long TamArqLote = 0;
                    bool IniciouLote = false;
                    int ContaNfe = 0;
                    List<string> lstArquivoInseridoLote = new List<string>();

                    for (int i = 0; i < lstArquivosNFe.Count; i++)
                    {
                        //Encerra o lote se o tamanho do arquivo de lote for maior ou igual a 450000 bytes (450 kbytes)
                        if (IniciouLote && TamArqLote >= 450000)
                        {
                            this.EncerrarLoteNfe(intNumeroLote);
                            this.FinalizacaoLote(intNumeroLote, lstArquivoInseridoLote);

                            //Limpar as variáveis, atributos depois de totalmente finalizado o lote, pois o conteúdo
                            //de aglumas variáveis são utilizados na finalização.
                            lstArquivoInseridoLote.Clear();
                            ContaNfe = 0;
                            TamArqLote = 0;
                            IniciouLote = false;
                        }

                        //Iniciar o Lote de NFe
                        if (!IniciouLote)
                        {
                            intNumeroLote = this.ProximoNumeroLote();

                            this.IniciarLoteNfe(intNumeroLote);

                            IniciouLote = true;
                        }

                        //Inserir o arquivo de XML da NFe na string do lote
                        this.InserirNFeLote(lstArquivosNFe[i]);
                        lstArquivoInseridoLote.Add(lstArquivosNFe[i]);
                        ContaNfe ++;
                        FileInfo oArq = new FileInfo(lstArquivosNFe[i]);
                        TamArqLote += oArq.Length;

                        Thread.Sleep(100);

                        //Encerrar o Lote se já passou por todas as notas
                        //Encerrar o lote se já tiver incluido 50 notas (Quantidade máxima permitida pelo SEFAZ)
                        if ((i + 1) == lstArquivosNFe.Count || ContaNfe == 50)
                        {
                            this.EncerrarLoteNfe(intNumeroLote);
                            this.FinalizacaoLote(intNumeroLote, lstArquivoInseridoLote);

                            //Limpar as variáveis, atributos depois de totalmente finalizado o lote, pois o conteúdo
                            //de aglumas variáveis são utilizados na finalização.
                            lstArquivoInseridoLote.Clear();
                            ContaNfe = 0;
                            TamArqLote = 0;
                            IniciouLote = false;                            
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region FinalizacaoLote()
        /// <summary>
        /// Executa alguns procedimentos para finalizar o processo de montagem de 1 lote de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        private void FinalizacaoLote(int intNumeroLote, List<string> lstArquivosNFe)
        {
            try
            {
                //Vou atualizar os lotes das NFE´s no fluxo de envio somente depois de encerrado o lote onde eu 
                //tenho certeza que ele foi gerado e que nenhum erro aconteceu, pois desta forma, se falhar somente na 
                //atualização eu tenho como fazer o UniNFe se recuperar de um erro. Assim sendo não mude de ponto.

                FluxoNfe oFluxoNfe = new FluxoNfe();
                for (int i = 0; i < lstArquivosNFe.Count; i++)
                {
                    //Efetua a leitura do XML, tem que acontecer antes de mover o arquivo
                    absLerXML.DadosNFeClass oDadosNfe = this.LerXMLNFe(lstArquivosNFe[i]);
                    //oLerXml.Nfe(lstArquivosNFe[i]);

                    //Mover o XML da NFE para a pasta de enviados em processamento
                    oAux.MoverArquivo(lstArquivosNFe[i], PastaEnviados.EmProcessamento);

                    //Atualiza o arquivo de controle de fluxo
                    oFluxoNfe.AtualizarTag(oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.idLote, intNumeroLote.ToString("000000000000000"));

                    //Gravar o XML de retorno do número do lote para o ERP
                    this.GravarXMLLoteRetERP(intNumeroLote, lstArquivosNFe[i]);

                    Thread.Sleep(100);
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region LoteNfe() - Sobrecarga
        /// <summary>
        /// Gera lote da nota fiscal eletrônica com somente uma nota fiscal
        /// </summary>
        /// <param name="strArquivoNfe">Nome do arquivo XML da Nota Fiscal</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        public void LoteNfe(string strArquivoNfe)
        {
            List<string> lstArquivo = new List<string>();

            lstArquivo.Add(strArquivoNfe);

            try
            {
                this.LoteNfe(lstArquivo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region IniciarLoteNfe()
        /// <summary>
        /// Inicia a string do XML do Lote de notas fiscais
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected abstract void IniciarLoteNfe(Int32 intNumeroLote);
        #endregion

        #region InserirNFeLote()
        /// <summary>
        /// Insere o XML de Nota Fiscal passado por parâmetro na string do XML de Lote de NFe
        /// </summary>
        /// <param name="strArquivoNfe">Nome do arquivo XML de nota fiscal eletrônica a ser inserido no lote</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected abstract void InserirNFeLote(string strArquivoNfe);
        #endregion

        #region EncerrarLoteNfe()
        /// <summary>
        /// Encerra a string do XML de lote de notas fiscais eletrônicas
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected abstract void EncerrarLoteNfe(Int32 intNumeroLote);
        #endregion

        #region GerarXMLLote()
        /// <summary>
        /// Grava o XML de lote de notas fiscais eletrônicas fisicamente no HD na pasta de envio
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <date>15/04/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        protected void GerarXMLLote(Int32 intNumeroLote)
        {
            //Gravar o XML do lote das notas fiscais
            string vNomeArqLoteNfe = ConfiguracaoApp.vPastaXMLEnvio + "\\" +
                                     intNumeroLote.ToString("000000000000000") +
                                     ExtXml.EnvLot;// "-env-lot.xml";

            StreamWriter SW_2 = null;

            try
            {
                SW_2 = File.CreateText(vNomeArqLoteNfe);
                SW_2.Write(strXMLLoteNfe);
                SW_2.Close();
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                SW_2.Close();
            }
        }
        #endregion

        #region ProximoNumeroLote()
        /// <summary>
        /// Pega o ultimo número de lote utilizado e acrescenta mais 1 para novo envio
        /// </summary>
        /// <returns>Retorna o um novo número de lote a ser utilizado nos envios das notas fiscais</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private Int32 ProximoNumeroLote()
        {
            Int32 intNumeroLote = 1;

            //TODO: Estudar uma forma de colocar um try neste ponto e retornar erro para o ERP caso não consiga ler o XML do numero do lote
            if (File.Exists(NomeArqXmlLote))
            {
                //Carregar os dados do arquivo XML de configurações do UniNfe
                XmlTextReader oLerXml = new XmlTextReader(NomeArqXmlLote);

                while (oLerXml.Read())
                {
                    if (oLerXml.NodeType == XmlNodeType.Element)
                    {
                        if (oLerXml.Name == "UltimoLoteEnviado")
                        {
                            oLerXml.Read(); intNumeroLote = Convert.ToInt32(oLerXml.Value) + 1;
                            break;
                        }
                    }
                }
                oLerXml.Close();
            }

            this.SalvarNumeroLoteUtilizado(intNumeroLote);

            return intNumeroLote;
        }
        #endregion

        #region SalvarNumeroLoteUtilizado()
        /// <summary>
        /// Salva em XML o número do ultimo lote utilizado para envio
        /// </summary>
        /// <param name="intNumeroLote">Numero do lote a ser salvo</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void SalvarNumeroLoteUtilizado(Int32 intNumeroLote)
        {
            //TODO: Estudar uma forma de colocar um try neste ponto para se ocorrer de der algum erro, podermos retornar para o ERP
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            XmlWriter oXmlGravar = XmlWriter.Create(NomeArqXmlLote, oSettings);

            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("DadosLoteNfe");
            oXmlGravar.WriteElementString("UltimoLoteEnviado", intNumeroLote.ToString());
            oXmlGravar.WriteEndElement(); //DadosLoteNfe
            oXmlGravar.WriteEndDocument();
            oXmlGravar.Flush();
            oXmlGravar.Close();
        }
        #endregion

        #region GravarXMLLoteRetERP()
        /// <summary>
        /// Grava um XML com o número de lote utilizado na pasta de retorno para que o ERP possa pegar este número
        /// </summary>
        /// <param name="intNumeroLote">Número do lote a ser gravado no retorno para o ERP</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void GravarXMLLoteRetERP(Int32 intNumeroLote, string NomeArquivoXML)
        {
            //TODO: Estudar uma forma de colocar um try neste ponto para se ocorrer de der algum erro, podermos retornar para o ERP
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;
            oSettings.Encoding = Encoding.UTF8;

            string cArqLoteRetorno = this.NomeArqLoteRetERP(NomeArquivoXML);

            XmlWriter oXmlLoteERP = XmlWriter.Create(cArqLoteRetorno, oSettings);

            oXmlLoteERP.WriteStartDocument();
            oXmlLoteERP.WriteStartElement("DadosLoteNfe");
            oXmlLoteERP.WriteElementString("NumeroLoteGerado", intNumeroLote.ToString());
            oXmlLoteERP.WriteEndElement(); //DadosLoteNfe
            oXmlLoteERP.WriteEndDocument();
            oXmlLoteERP.Flush();
            oXmlLoteERP.Close();
        }
        #endregion

        #endregion

        #region Métodos para gerar o XML´s diversos

        #region StatusServico()
        /// <summary>
        /// Criar um arquivo XML com a estrutura necessária para consultar o status do serviço
        /// </summary>
        /// <returns>Retorna o caminho e nome do arquivo criado</returns>
        /// <example>
        /// string vPastaArq = this.CriaArqXMLStatusServico();
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public abstract string StatusServico(int tpEmis);
        #endregion

        #region StatusServico() - Sobrecarga
        public abstract string StatusServico(int tpEmis, int cUF);
        #endregion

        #region CabecMsg()
        /// <summary>
        /// Gera uma string com o XML do cabeçalho dos dados a serem enviados para os serviços da NFe
        /// </summary>
        /// <param name="pVersaoDados">
        /// Versão do arquivo XML que será enviado para os WebServices. Esta versão varia de serviço para
        /// serviço e deve ser consultada no manual de integração da NFE
        /// </param>
        /// <returns>
        /// Retorna uma string com o XML do cabeçalho dos dados a serem enviados para os serviços da NFe
        /// </returns>
        /// <example>
        /// vCabecMSG = GerarXMLCabecMsg("1.07");
        /// MessageBox.Show( vCabecMSG );
        /// 
        /// //O conteúdo que será demonstrado no MessageBox é:
        /// //
        /// //  <?xml version="1.0" encoding="UTF-8" ?>
        /// //  <cabecMsg xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.02">
        /// //     <versaoDados>1.07</versaoDados>
        /// //  </cabecMsg>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        public abstract string CabecMsg(string VersaoDados);
        #endregion

        #region XmlRetorno()
        /// <summary>
        /// Grava o XML com os dados do retorno dos webservices e deleta o XML de solicitação do serviço.
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <param name="ConteudoXMLRetorno">Conteúdo do XML a ser gerado</param>
        /// <example>
        /// // Arquivo de envio: 20080619T19113320-ped-sta.xml
        /// // Arquivo de retorno que vai ser gravado: 20080619T19113320-sta.xml
        /// this.GravarXmlRetorno("-ped-sta.xml", "-sta.xml");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        public void XmlRetorno(string pFinalArqEnvio, string pFinalArqRetorno, string ConteudoXMLRetorno)
        {
            StreamWriter SW = null;

            try
            {
                //Deletar o arquivo XML da pasta de temporários de XML´s com erros se 
                //o mesmo existir
                oAux.DeletarArqXMLErro(ConfiguracaoApp.vPastaXMLErro + "\\" + oAux.ExtrairNomeArq(this.NomeXMLDadosMsg, ".xml") + ".xml");

                //Gravar o arquivo XML de retorno
                string ArqXMLRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                                      oAux.ExtrairNomeArq(this.NomeXMLDadosMsg, pFinalArqEnvio) +
                                      pFinalArqRetorno;
                SW = File.CreateText(ArqXMLRetorno);
                SW.Write(ConteudoXMLRetorno);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (SW != null)
                {
                    SW.Close();
                }
            }

            //Gravar o XML de retorno também no formato TXT
            if (ConfiguracaoApp.GravarRetornoTXTNFe)
            {
                try
                {
                    this.TXTRetorno(pFinalArqEnvio, pFinalArqRetorno, ConteudoXMLRetorno);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        #endregion

        #region TXTRetorno()
        //TODO: Documentar este método
        protected abstract void TXTRetorno(string pFinalArqEnvio, string pFinalArqRetorno, string ConteudoXMLRetorno);
        #endregion

        #endregion

        #region Métodos para gerar os XML´s de distribuição

        #region this.XMLDistInut()
        /// <summary>
        /// Criar o arquivo XML de distribuição das Inutilizações de Números de NFe´s com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqInut">Nome arquivo XML de Inutilização</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        public abstract void XmlDistInut(string strArqInut, string strRetInutNFe);
        #endregion

        #region this.XMLDistCanc()
        /// <summary>
        /// Criar o arquivo XML de distribuição dos Cancelamentos com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqCanc">Nome arquivo XML de Cancelamento</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        public abstract void XmlDistCanc(string strArqCanc, string strRetCancNFe);
        #endregion

        #region XmlPedRec()
        /// <summary>
        /// Gera o XML de pedido de analise do recibo do lote
        /// </summary>
        /// <param name="strRecibo">Número do recibo a ser consultado o lote</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        public abstract void XmlPedRec(string strRecibo);
        #endregion

        #region XMLDistNFe()
        /// <summary>
        /// Criar o arquivo XML de distribuição das NFE com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqNFe">Nome arquivo XML da NFe</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public abstract void XmlDistNFe(string strArqNFe, string strProtNfe);
        #endregion

        #endregion

        #region Métodos auxiliares

        #region LerXMLNfe()
        protected abstract absLerXML.DadosNFeClass LerXMLNFe(string Arquivo);    
        #endregion

        #region LerXMLRecibo()
        protected abstract absLerXML.DadosRecClass LerXMLRecibo(string Arquivo);
        #endregion

        #region NomeArqLoteRetERP()
        protected string NomeArqLoteRetERP(string NomeArquivoXML)
        {
            return ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                oAux.ExtrairNomeArq(NomeArquivoXML, ExtXml.Nfe/*"-nfe.xml"*/) +
                "-num-lot.xml";
        }
        #endregion

        #endregion

        #endregion
    }
}

﻿using System;
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
        /// Index da empresa selecionada
        /// </summary>
        protected int EmpIndex { get; set; }
        /// <summary>
        /// Atributo que vai receber a string do XML de lote de NFe´s para que este conteúdo seja gravado após finalizado em arquivo físico no HD
        /// </summary>
        protected string strXMLLoteNfe;
        /// <summary>     
        /// Nome do arquivo para controle da numeração sequencial do lote.
        /// </summary>
        protected string NomeArqXmlLote;
        /// <summary>
        /// Nome do arquivo 1 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        protected string Bkp1NomeArqXmlLote;
        /// <summary>
        /// Nome do arquivo 2 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        protected string Bkp2NomeArqXmlLote;
        /// <summary>
        /// Nome do arquivo 3 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        protected string Bkp3NomeArqXmlLote;
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

        #region Construtures
        public absGerarXML(int empIndex)
        {
        }
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
            bool excluirFluxo = true;

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
                        ContaNfe++;
                        FileInfo oArq = new FileInfo(lstArquivosNFe[i]);
                        TamArqLote += oArq.Length;

                        Thread.Sleep(100);

                        //Encerrar o Lote se já passou por todas as notas
                        //Encerrar o lote se já tiver incluido 50 notas (Quantidade máxima permitida pelo SEFAZ)
                        if ((i + 1) == lstArquivosNFe.Count || ContaNfe == 50)
                        {
                            //Encerra o lote
                            this.EncerrarLoteNfe(intNumeroLote);

                            //Se já encerrou o lote não pode mais tirar do fluxo se der erro daqui para baixo
                            excluirFluxo = false;

                            //Finalizar o lote gerando retornos para o ERP.
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
            catch (Exception ex)
            {
                if (excluirFluxo)
                {
                    for (int i = 0; i < lstArquivosNFe.Count; i++)
                    {
                        //Efetua a leitura do XML da NFe
                        absLerXML.DadosNFeClass oDadosNfe = this.LerXMLNFe(lstArquivosNFe[i]);

                        //Excluir a nota fiscal do fluxo pois deu algum erro neste ponto
                        FluxoNfe oFluxoNfe = new FluxoNfe();
                        oFluxoNfe.ExcluirNfeFluxo(oDadosNfe.chavenfe);
                    }
                }

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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Gravar o XML do lote das notas fiscais
            string vNomeArqLoteNfe = Empresa.Configuracoes[emp].PastaEnvio + "\\" +
                                     intNumeroLote.ToString("000000000000000") +
                                     ExtXml.EnvLot;

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

        /// <summary>
        /// Popular a propriedade do nome do arquivo de controle da numeração do lote
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 20/08/2010
        /// </remarks>
        private void PopulateNomeArqLote()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            NomeArqXmlLote = Empresa.Configuracoes[emp].PastaEmpresa + "\\UniNfeLote.xml";
            Bkp1NomeArqXmlLote = Empresa.Configuracoes[emp].PastaEmpresa + "\\Bkp1_UniNfeLote.xml";
            Bkp2NomeArqXmlLote = Empresa.Configuracoes[emp].PastaEmpresa + "\\Bkp2_UniNfeLote.xml";
            Bkp3NomeArqXmlLote = Empresa.Configuracoes[emp].PastaEmpresa + "\\Bkp3_UniNfeLote.xml";
        }

        #region ProximoNumeroLote()
        /// <summary>
        /// Pega o ultimo número de lote utilizado e acrescenta mais 1 para novo envio
        /// </summary>
        /// <returns>Retorna o um novo número de lote a ser utilizado nos envios das notas fiscais</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private Int32 ProximoNumeroLote()
        {
            PopulateNomeArqLote();

            Int32 intNumeroLote = 1;
            bool deuErro = false;

            for (int i = 0; i < 4; i++)
            {
                XmlTextReader oLerXml = null;

                try
                {
                    if (File.Exists(NomeArqXmlLote))
                    {
                        //Carregar os dados do arquivo XML de configurações do UniNfe
                        oLerXml = new XmlTextReader(NomeArqXmlLote);

                        while (oLerXml.Read())
                        {
                            if (oLerXml.NodeType == XmlNodeType.Element)
                            {
                                if (oLerXml.Name == "UltimoLoteEnviado")
                                {
                                    oLerXml.Read(); intNumeroLote = Convert.ToInt32(oLerXml.Value) + 1;

                                    //Vou somar uns 3 números para frente para evitar repetir os números.
                                    if (deuErro)
                                        intNumeroLote += 3;

                                    break;
                                }
                            }
                        }

                        oLerXml.Close();
                    }

                    this.SalvarNumeroLoteUtilizado(intNumeroLote);

                    break;
                }
                catch (Exception ex)
                {
                    deuErro = true;
                    //Fechar o arquivo se o mesmo estiver aberto
                    if (oLerXml != null)
                        if (oLerXml.ReadState != ReadState.Closed)
                            oLerXml.Close();

                    switch (i)
                    {
                        case 0:
                            if (File.Exists(Bkp1NomeArqXmlLote))
                                File.Copy(Bkp1NomeArqXmlLote, NomeArqXmlLote, true);
                            break;

                        case 1:
                            if (File.Exists(Bkp2NomeArqXmlLote))
                                File.Copy(Bkp2NomeArqXmlLote, NomeArqXmlLote, true);
                            break;

                        case 2:
                            if (File.Exists(Bkp3NomeArqXmlLote))
                                File.Copy(Bkp3NomeArqXmlLote, NomeArqXmlLote, true);
                            break;

                        case 3:
                            throw new Exception("Não foi possível efetuar a leitura do arquivo " + NomeArqXmlLote + ". Verifique se o mesmo não está com sua estrutura de XML danificada."); //Se tentou 4 vezes e deu errado, vamos retornar o erro e não tem o que ser feito.

                        default:
                            break;
                    }
                }
                finally
                {
                    //Fechar o arquivo se o mesmo estiver aberto - Wandrey 20/04/2010
                    if (oLerXml != null)
                        if (oLerXml.ReadState != ReadState.Closed)
                            oLerXml.Close();
                }
            }

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
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;
            XmlWriter oXmlGravar = null;

            try
            {
                oXmlGravar = XmlWriter.Create(NomeArqXmlLote, oSettings);
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("DadosLoteNfe");
                oXmlGravar.WriteElementString("UltimoLoteEnviado", intNumeroLote.ToString());
                oXmlGravar.WriteEndElement(); //DadosLoteNfe
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();

                //Criar 3 copias de segurança deste XML para voltar ele caso de algum problema com o mesmo.
                File.Copy(NomeArqXmlLote, Bkp1NomeArqXmlLote, true);
                File.Copy(NomeArqXmlLote, Bkp2NomeArqXmlLote, true);
                File.Copy(NomeArqXmlLote, Bkp3NomeArqXmlLote, true);
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                //Fechar o arquivo se o mesmo ainda estiver aberto - Wandrey 20/04/2010
                if (oXmlGravar != null)
                    if (oXmlGravar.WriteState != WriteState.Closed)
                        oXmlGravar.Close();
            }
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
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;
            oSettings.Encoding = Encoding.UTF8;
            XmlWriter oXmlLoteERP = null;

            string cArqLoteRetorno = this.NomeArqLoteRetERP(NomeArquivoXML);

            try
            {
                oXmlLoteERP = XmlWriter.Create(cArqLoteRetorno, oSettings);

                oXmlLoteERP.WriteStartDocument();
                oXmlLoteERP.WriteStartElement("DadosLoteNfe");
                oXmlLoteERP.WriteElementString("NumeroLoteGerado", intNumeroLote.ToString());
                oXmlLoteERP.WriteEndElement(); //DadosLoteNfe
                oXmlLoteERP.WriteEndDocument();
                oXmlLoteERP.Flush();
                oXmlLoteERP.Close();
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                //Fechar o arquivo se o mesmo ainda estiver aberto - Wandrey 20/04/2010
                if (oXmlLoteERP != null)
                    if (oXmlLoteERP.WriteState != WriteState.Closed)
                        oXmlLoteERP.Close();
            }
        }
        #endregion

        #endregion

        #region Métodos para gerar o XML´s diversos

        #region Cancelamento()
        /// <summary>
        /// Criar um arquivo XML com a estrutura necessária para cancelamento de uma nota
        /// </summary>
        /// <param name="pFinalArqEnvio"></param>
        /// <param name="tpAmb"></param>
        /// <param name="tpEmis"></param>
        /// <param name="chNFe"></param>
        /// <param name="nProt"></param>
        /// <param name="xJust"></param>
        public abstract void Cancelamento(string pFinalArqEnvio, int tpAmb, int tpEmis, string chNFe, string nProt, string xJust);
        #endregion

        #region Consulta()
        public abstract void Consulta(string pFinalArqEnvio, int tpAmb, int tpEmis, string chNFe);
        #endregion

        #region Inutilizacao()
        /// <summary>
        /// Criar um arquivo XML com a estrutura necessária para inutilizacao de numeraca
        /// </summary>
        /// <param name="pFinalArqEnvio"></param>
        /// <param name="tpAmb"></param>
        /// <param name="tpEmis"></param>
        /// <param name="ano"></param>
        /// <param name="CNPJ"></param>
        /// <param name="mod"></param>
        /// <param name="serie"></param>
        /// <param name="nNFIni"></param>
        /// <param name="nNFFin"></param>
        public abstract void Inutilizacao(string pFinalArqEnvio, int tpAmb, int tpEmis, int cUF, int ano, string CNPJ, int mod, int serie, int nNFIni, int nNFFin, string xJust);
        #endregion

        #region StatusServico() - Sobrecarga
        /// <summary>
        /// Cria um arquivo XML com a estrutura necessária para consultar o status do serviço
        /// </summary>
        /// <returns>Retorna o caminho e nome do arquivo criado</returns>
        /// <param name="tpEmis"></param>
        /// <param name="cUF"></param>
        /// <param name="amb"></param>
        /// <returns></returns>
        public abstract string StatusServico(int tpEmis, int cUF, int amb);
        #endregion

        #region StatusServico() - Sobrecarga
        /// <summary>
        /// Cria um arquivo XML com a estrutura necessária para consultar o status do serviço
        /// </summary>
        /// <param name="pFinalArqEnvio"></param>
        /// <param name="tpAmb"></param>
        /// <param name="tpEmis"></param>
        /// <param name="cUF"></param>
        public abstract void StatusServico(string pFinalArqEnvio, int tpAmb, int tpEmis, int cUF);
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
        /// <param name="finalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="finalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <param name="conteudoXMLRetorno">Conteúdo do XML a ser gerado</param>
        /// <example>
        /// // Arquivo de envio: 20080619T19113320-ped-sta.xml
        /// // Arquivo de retorno que vai ser gravado: 20080619T19113320-sta.xml
        /// this.GravarXmlRetorno("-ped-sta.xml", "-sta.xml");
        /// </example>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// </remarks>        
        public void XmlRetorno(string finalArqEnvio, string finalArqRetorno, string conteudoXMLRetorno)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                XmlRetorno(finalArqEnvio, finalArqRetorno, conteudoXMLRetorno, Empresa.Configuracoes[emp].PastaRetorno);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region XmlRetorno()
        /// <summary>
        /// Grava o XML com os dados do retorno dos webservices e deleta o XML de solicitação do serviço.
        /// </summary>
        /// <param name="finalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="finalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <param name="conteudoXMLRetorno">Conteúdo do XML a ser gerado</param>
        /// <param name="pastaGravar">Pasta onde é para ser gravado o XML de Retorno</param>
        /// <example>
        /// // Arquivo de envio: 20080619T19113320-ped-sta.xml
        /// // Arquivo de retorno que vai ser gravado: 20080619T19113320-sta.xml
        /// this.GravarXmlRetorno("-ped-sta.xml", "-sta.xml");
        /// </example>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 25/11/2010
        /// </remarks>        
        public void XmlRetorno(string finalArqEnvio, string finalArqRetorno, string conteudoXMLRetorno, string pastaGravar)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            StreamWriter SW = null;

            try
            {
                //Deletar o arquivo XML da pasta de temporários de XML´s com erros se 
                //o mesmo existir
                oAux.DeletarArqXMLErro(Empresa.Configuracoes[emp].PastaErro + "\\" + oAux.ExtrairNomeArq(this.NomeXMLDadosMsg, ".xml") + ".xml");

                //Gravar o arquivo XML de retorno
                string ArqXMLRetorno = pastaGravar + "\\" +
                                       oAux.ExtrairNomeArq(this.NomeXMLDadosMsg, finalArqEnvio) +
                                       finalArqRetorno;
                SW = File.CreateText(ArqXMLRetorno);
                SW.Write(conteudoXMLRetorno);
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
            if (Empresa.Configuracoes[emp].GravarRetornoTXTNFe)
            {
                try
                {
                    this.TXTRetorno(finalArqEnvio, finalArqRetorno, conteudoXMLRetorno);
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            return Empresa.Configuracoes[emp].PastaRetorno + "\\" +
                oAux.ExtrairNomeArq(NomeArquivoXML, ExtXml.Nfe) +
                "-num-lot.xml";
        }
        #endregion

        #region GravarArquivoParaEnvio
        /// <summary>
        /// grava um arquivo na pasta de envio
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="Conteudo"></param>
        protected void GravarArquivoParaEnvio(string Arquivo, string Conteudo)
        {
            try
            {
                //Gravar o XML
                MemoryStream oMemoryStream = Auxiliar.StringXmlToStream(Conteudo);
                XmlDocument docProc = new XmlDocument();
                docProc.Load(oMemoryStream);
                docProc.Save(Empresa.Configuracoes[EmpIndex].PastaEnvio + "\\" + Path.GetFileName(Arquivo));
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #endregion

        #endregion

        #region ProcessaConsultaCadastro()
        /// <summary>
        /// utilizada pela GerarXML
        /// </summary>
        /// <param name="msXml"></param>
        /// <returns></returns>
        public RetConsCad ProcessaConsultaCadastro(XmlDocument doc)
        {
            if (doc.GetElementsByTagName("infCad") == null)
                return null;

            RetConsCad vRetorno = new RetConsCad();

            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name == "retConsCad")
                {
                    foreach (XmlNode noderetConsCad in node.ChildNodes)
                    {
                        if (noderetConsCad.Name == "infCons")
                        {
                            foreach (XmlNode nodeinfCons in noderetConsCad.ChildNodes)
                            {
                                if (nodeinfCons.Name == "infCad")
                                {
                                    vRetorno.infCad.Add(new infCad());
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].CNPJ = vRetorno.CNPJ;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].CPF = vRetorno.CPF;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].IE = vRetorno.IE;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].UF = vRetorno.UF;

                                    foreach (XmlNode nodeinfCad in nodeinfCons.ChildNodes)
                                    {
                                        switch (nodeinfCad.Name)
                                        {
                                            case "UF":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].UF = nodeinfCad.InnerText;
                                                break;
                                            case "IE":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].IE = nodeinfCad.InnerText;
                                                break;
                                            case "CNPJ":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].CNPJ = nodeinfCad.InnerText;
                                                break;
                                            case "CPF":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].CNPJ = nodeinfCad.InnerText;
                                                break;
                                            case "xNome":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xNome = nodeinfCad.InnerText;
                                                break;
                                            case "xFant":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xFant = nodeinfCad.InnerText;
                                                break;

                                            case "ender":
                                                foreach (XmlNode nodeinfConsEnder in nodeinfCad.ChildNodes)
                                                {
                                                    switch (nodeinfConsEnder.Name)
                                                    {
                                                        case "xLgr":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xLgr = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "nro":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.nro = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "xCpl":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xCpl = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "xBairro":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xBairro = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "xMun":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xMun = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "cMun":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.cMun = Convert.ToInt32("0" + nodeinfConsEnder.InnerText);
                                                            break;
                                                        case "CEP":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.CEP = Convert.ToInt32("0" + nodeinfConsEnder.InnerText);
                                                            break;
                                                    }
                                                }
                                                break;

                                            case "IEAtual":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].IEAtual = nodeinfCad.InnerText;
                                                break;
                                            case "IEUnica":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].IEUnica = nodeinfCad.InnerText;
                                                break;
                                            case "dBaixa":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dBaixa = Auxiliar.getDateTime(nodeinfCad.InnerText);
                                                break;
                                            case "dUltSit":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dUltSit = Auxiliar.getDateTime(nodeinfCad.InnerText);
                                                break;
                                            case "dIniAtiv":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dIniAtiv = Auxiliar.getDateTime(nodeinfCad.InnerText);
                                                break;
                                            case "CNAE":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].CNAE = Convert.ToInt32("0" + nodeinfCad.InnerText);
                                                break;
                                            case "xRegApur":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xRegApur = nodeinfCad.InnerText;
                                                break;
                                            case "cSit":
                                                if (nodeinfCad.InnerText == "0")
                                                    vRetorno.infCad[vRetorno.infCad.Count - 1].cSit = "Contribuinte não habilitado";
                                                else if (nodeinfCad.InnerText == "1")
                                                    vRetorno.infCad[vRetorno.infCad.Count - 1].cSit = "Contribuinte habilitado";
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch (nodeinfCons.Name)
                                    {
                                        case "cStat":
                                            vRetorno.cStat = Convert.ToInt32("0" + nodeinfCons.InnerText);
                                            break;
                                        case "xMotivo":
                                            vRetorno.xMotivo = nodeinfCons.InnerText;
                                            break;
                                        case "UF":
                                            vRetorno.UF = nodeinfCons.InnerText;
                                            break;
                                        case "IE":
                                            vRetorno.IE = nodeinfCons.InnerText;
                                            break;
                                        case "CNPJ":
                                            vRetorno.CNPJ = nodeinfCons.InnerText;
                                            break;
                                        case "CPF":
                                            vRetorno.CPF = nodeinfCons.InnerText;
                                            break;
                                        case "dhCons":
                                            vRetorno.dhCons = Auxiliar.getDateTime(nodeinfCons.InnerText);
                                            break;
                                        case "cUF":
                                            vRetorno.cUF = Convert.ToInt32("0" + nodeinfCons.InnerText);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return vRetorno;
        }
        #endregion

        #region ProcessaConsultaCadastro()
        public RetConsCad ProcessaConsultaCadastro(MemoryStream msXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(msXml);
            return ProcessaConsultaCadastro(doc);
        }
        #endregion

        #region ProcessaConsultaCadastro()
        /// <summary>
        /// Função Callback que analisa a resposta do Status do Servido
        /// </summary>
        /// <param name="elem"></param>
        /// <by>Marcos Diez</by>
        /// <date>30/8/2009</date>
        /// <returns></returns>
        /// <summary>
        /// utilizada internamente pela VerConsultaCadastroClass
        /// </summary>
        /// <param name="cArquivoXML"></param>
        /// <returns></returns>
        public RetConsCad ProcessaConsultaCadastro(string cArquivoXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(cArquivoXML);
            return ProcessaConsultaCadastro(doc);
        }
        #endregion
    }
}

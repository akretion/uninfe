using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Unimake.Business.DFe.Xml.CTe;
using Unimake.Business.DFe.Xml.MDFe;
using Unimake.Business.DFe.Xml.NFe;

namespace NFe.Service
{
    /// <summary>
    /// Classe abstrata para gerar os XML´s da nota fiscal eletrônica
    /// </summary>
    public class GerarXML
    {
        #region Atributos

        /// <summary>
        /// Index da empresa selecionada
        /// </summary>
        protected int EmpIndex { get; set; }

        /// <summary>
        /// Atributo que vai receber a string do XML de lote de NFe´s para que este conteúdo seja gravado após finalizado em arquivo físico no HD
        /// </summary>
        protected string XMLLoteDFe;

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

        #endregion Atributos

        #region Propriedades

        /// <summary>
        /// Nome do arquivo XML que está sendo enviado para os webservices
        /// </summary>
        public string NomeXMLDadosMsg { get; set; }

        /// <summary>
        /// Serviço que está sendo executado (Envio de NFE, Cancelamento, consultas, etc...)
        /// </summary>
        public Servicos Servico { get; set; }

        /// <summary>
        /// Nome do arquivo XML gerado
        /// </summary>
        public string NomeArqGerado { get; private set; }

        #endregion Propriedades

        #region Objetos

        protected Auxiliar oAux = new Auxiliar();

        #endregion Objetos

        #region Construtures

        public GerarXML(int empIndex) => EmpIndex = empIndex;

        public GerarXML(Thread thread)
            : this(Convert.ToInt32(thread.Name))
        {
        }

        #endregion Construtures

        #region Métodos

        #region Métodos para gerar o Lote de Notas Fiscais Eletrônicas

        #region LoteNfe()

        /// <summary>
        /// Gera o Lote das Notas Fiscais passada por parâmetro na pasta de envio
        /// </summary>
        /// <param name="servico">Serviço que está sendo executado</param>
        /// <param name="arquivosXMLDFe">Lista de arquivos que serão inseridos no lote</param>
        /// <param name="versaoXml">Versão do XML do lote</param>
        /// <param name="modeloDFe">Modelo do documento fiscal eletrônico</param>
        public XmlDocument LoteNfe(Servicos servico, List<ArquivoXMLDFe> arquivosXMLDFe, string versaoXml, string modeloDFe)
        {
            Servico = servico;

            var excluirFluxo = true;

            try
            {
                //Buscar o número do lote a ser utilizado
                var numeroLote = 0;

                long TamArqLote = 0;
                var IniciouLote = false;
                var ContaNfe = 0;
                var arquivosInseridosLote = new List<ArquivoXMLDFe>();
                var nfesCount = arquivosXMLDFe.Count;

                for(var i = 0; i < arquivosXMLDFe.Count; i++)
                {
                    //Encerra o lote se o tamanho do arquivo de lote for maior ou igual a 450000 bytes (450 kbytes)
                    if(IniciouLote && TamArqLote >= 450000)
                    {
                        EncerrarLoteNfe(numeroLote, arquivosInseridosLote);

                        //Limpar as variáveis, atributos depois de totalmente finalizado o lote, pois o conteúdo
                        //de aglumas variáveis são utilizados na finalização.
                        arquivosInseridosLote.Clear();
                        ContaNfe = 0;
                        TamArqLote = 0;
                        IniciouLote = false;
                    }

                    //Iniciar o Lote de NFe
                    if(!IniciouLote)
                    {
                        numeroLote = ProximoNumeroLote();
                        IniciarLoteNfe(numeroLote, versaoXml, modeloDFe, nfesCount);
                        IniciouLote = true;
                    }

                    //Inserir o arquivo de XML da NFe na string do lote
                    InserirNFeLote(arquivosXMLDFe[i].ConteudoXML);
                    arquivosInseridosLote.Add(arquivosXMLDFe[i]);
                    ContaNfe++;
                    TamArqLote += new FileInfo(arquivosXMLDFe[i].NomeArquivoXML).Length;

                    //Encerrar o Lote se já passou por todas as notas
                    //Encerrar o lote se já tiver incluido 50 notas (Quantidade máxima permitida pelo SEFAZ)
                    if((i + 1) == arquivosXMLDFe.Count || ContaNfe == 50)
                    {
                        //Encerra o lote
                        EncerrarLoteNfe(numeroLote, arquivosInseridosLote);

                        //Se já encerrou o lote não pode mais tirar do fluxo se der erro daqui para baixo
                        excluirFluxo = false;

                        //Limpar as variáveis, atributos depois de totalmente finalizado o lote, pois o conteúdo
                        //de aglumas variáveis são utilizados na finalização.
                        arquivosInseridosLote.Clear();
                        ContaNfe = 0;
                        TamArqLote = 0;
                        IniciouLote = false;
                    }
                }
            }
            catch
            {
                if(excluirFluxo)
                {
                    for(var i = 0; i < arquivosXMLDFe.Count; i++)
                    {
                        //Efetua a leitura do XML da NFe
                        var oDadosNfe = LerXMLNFe(arquivosXMLDFe[i].ConteudoXML);

                        //Excluir a nota fiscal do fluxo pois deu algum erro neste ponto
                        var oFluxoNfe = new FluxoNfe();
                        if(!oFluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                        {
                            oFluxoNfe.ExcluirNfeFluxo(oDadosNfe.chavenfe);
                        }
                    }
                }

                throw;
            }

            //Converter a string em XML
            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(XMLLoteDFe));

            return doc;
        }

        #endregion LoteNfe()

        #region FinalizacaoLote()

        /// <summary>
        /// Executa alguns procedimentos para finalizar o processo de montagem de 1 lote de notas
        /// </summary>
        private void FinalizacaoLote(int numeroLote, List<ArquivoXMLDFe> arquivosXMLDFe)
        {
            //Vou atualizar os lotes das NFE´s no fluxo de envio somente depois de encerrado o lote onde eu
            //tenho certeza que ele foi gerado e que nenhum erro aconteceu, pois desta forma, se falhar somente na
            //atualização eu tenho como fazer o UniNFe se recuperar de um erro. Assim sendo não mude de ponto.

            var oFluxoNfe = new FluxoNfe();
            for(var i = 0; i < arquivosXMLDFe.Count; i++)
            {
                //Efetua a leitura do XML, tem que acontecer antes de mover o arquivo
                var oDadosNfe = LerXMLNFe(arquivosXMLDFe[i].ConteudoXML);

                Empresas.Configuracoes[EmpIndex].CriarSubPastaEnviado();

                if(oDadosNfe.mod != "58" && oDadosNfe.mod != "57")
                {
                    //Salvar o XML assinado na pasta EmProcessamento
                    var arqEmProcessamento = Empresas.Configuracoes[EmpIndex].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + Path.GetFileName(arquivosXMLDFe[i].NomeArquivoXML);
                    var sw = File.CreateText(arqEmProcessamento);
                    sw.Write(arquivosXMLDFe[i].ConteudoXML.OuterXml);
                    sw.Close();

                    if(File.Exists(arqEmProcessamento))
                    {
                        File.Delete(arquivosXMLDFe[i].NomeArquivoXML);
                    }
                }

                //Atualiza o arquivo de controle de fluxo
                oFluxoNfe.AtualizarTag(oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.idLote, numeroLote.ToString("000000000000000"));

                //Gravar o XML de retorno do número do lote para o ERP
                GravarXMLLoteRetERP(numeroLote, arquivosXMLDFe[i].NomeArquivoXML);
            }
        }

        #endregion FinalizacaoLote()

        #region IniciarLoteNfe()

        /// <summary>
        /// Inicia a string do XML do Lote de notas fiscais
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <param name="modeloDFe">Modelo do Dodumento Fiscal Eletrônico</param>
        /// <param name="nfesCount">Quantidade de notas fiscais no lote</param>
        /// <param name="versaoXml">Versão do XML</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected void IniciarLoteNfe(int intNumeroLote, string versaoXml, string modeloDFe, int nfesCount)
        {
            XMLLoteDFe = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            var indSinc = "";
            switch(Servico)
            {
                case Servicos.MDFeMontarLoteVarios:
                case Servicos.MDFeMontarLoteUm:
                    XMLLoteDFe += "<enviMDFe xmlns=\"" + NFeStrConstants.NAME_SPACE_MDFE + "\" versao=\"" + versaoXml + "\">";
                    break;

                case Servicos.CTeMontarLoteVarios:
                case Servicos.CTeMontarLoteUm:
                    XMLLoteDFe += "<enviCTe xmlns=\"" + NFeStrConstants.NAME_SPACE_CTE + "\" versao=\"" + versaoXml + "\">";
                    break;

                default:
                    // Só vai poder ser sincrono se o lote for com uma nota,
                    // Se for mais de uma o SEFAZ so valida a primeira - Renan 20/05/2015
                    var indsinc = (((modeloDFe == "55" && Empresas.Configuracoes[EmpIndex].IndSinc) || (modeloDFe == "65" && Empresas.Configuracoes[EmpIndex].IndSincNFCe)) && nfesCount == 1 ? "1" : "0");
                    XMLLoteDFe += "<enviNFe xmlns=\"" + NFeStrConstants.NAME_SPACE_NFE + "\" versao=\"" + versaoXml + "\">";
                    indSinc = "<indSinc>" + indsinc + "</indSinc>";
                    break;
            }

            XMLLoteDFe += "<idLote>" + intNumeroLote.ToString("000000000000000") + "</idLote>";
            XMLLoteDFe += indSinc;
        }

        #endregion IniciarLoteNfe()

        #region InserirNFeLote()

        /// <summary>
        /// Insere o XML de Nota Fiscal passado por parâmetro na string do XML de Lote de NFe
        /// </summary>
        /// <param name="strArquivoNfe">Nome do arquivo XML de nota fiscal eletrônica a ser inserido no lote</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected void InserirNFeLote(XmlDocument conteudoXML)
        {
            var vNfeDadosMsg = conteudoXML.OuterXml;

            var tipo = string.Empty;

            switch(Servico)
            {
                case Servicos.MDFeMontarLoteVarios:
                case Servicos.MDFeMontarLoteUm:
                    tipo = "<MDFe";
                    break;

                case Servicos.CTeMontarLoteVarios:
                case Servicos.CTeMontarLoteUm:
                    tipo = "<CTe";
                    break;

                default:
                    tipo = "<NFe";
                    break;
            }

            //Separar somente o conteúdo a partir da tag <NFe> até </NFe>
            var nPosI = vNfeDadosMsg.IndexOf(tipo);
            var nPosF = vNfeDadosMsg.Length - nPosI;
            XMLLoteDFe += vNfeDadosMsg.Substring(nPosI, nPosF);
        }

        #endregion InserirNFeLote()

        #region EncerrarLoteNfe()

        /// <summary>
        /// Encerra a string do XML de lote de notas fiscais eletrônicas
        /// </summary>
        /// <param name="numeroLote">Número do lote que será enviado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected void EncerrarLoteNfe(int numeroLote, List<ArquivoXMLDFe> arquivosXMLDFe)
        {
            switch(Servico)
            {
                case Servicos.MDFeMontarLoteVarios:
                case Servicos.MDFeMontarLoteUm:
                    XMLLoteDFe += "</enviMDFe>";
                    break;

                case Servicos.CTeMontarLoteVarios:
                case Servicos.CTeMontarLoteUm:
                    XMLLoteDFe += "</enviCTe>";
                    break;

                default:
                    XMLLoteDFe += "</enviNFe>";
                    break;
            }

            FinalizacaoLote(numeroLote, arquivosXMLDFe);
        }

        #endregion EncerrarLoteNfe()

        #region PopulateNomeArqLote()

        /// <summary>
        /// Popular a propriedade do nome do arquivo de controle da numeração do lote
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 20/08/2010
        /// </remarks>
        private void PopulateNomeArqLote()
        {
            var emp = Empresas.FindEmpresaByThread();

            NomeArqXmlLote = Empresas.Configuracoes[emp].PastaEmpresa + "\\UniNfeLote.xml";
            Bkp1NomeArqXmlLote = Empresas.Configuracoes[emp].PastaEmpresa + "\\Bkp1_UniNfeLote.xml";
            Bkp2NomeArqXmlLote = Empresas.Configuracoes[emp].PastaEmpresa + "\\Bkp2_UniNfeLote.xml";
            Bkp3NomeArqXmlLote = Empresas.Configuracoes[emp].PastaEmpresa + "\\Bkp3_UniNfeLote.xml";
        }

        #endregion PopulateNomeArqLote()

        /// <summary>
        /// Somente gera o Número do lote no caso de CTeOS para manter compatibilidade com o processo do ERP, facilitando integração.
        /// </summary>
        /// <returns>Número do lote</returns>
        public int GerarLoteCTeOS(string nomeArquivoXML)
        {
            var numeroLote = ProximoNumeroLote();

            GravarXMLLoteRetERP(numeroLote, nomeArquivoXML);

            return numeroLote;
        }

        #region ProximoNumeroLote()

        /// <summary>
        /// Pega o ultimo número de lote utilizado e acrescenta mais 1 para novo envio
        /// </summary>
        /// <returns>Retorna o um novo número de lote a ser utilizado nos envios das notas fiscais</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private int ProximoNumeroLote()
        {
            PopulateNomeArqLote();

            var numeroLote = 1;
            var deuErro = false;

            DateTime startTime;
            DateTime stopTime;
            TimeSpan elapsedTime;

            long elapsedMillieconds;
            startTime = DateTime.Now;

            while(true)
            {
                stopTime = DateTime.Now;
                elapsedTime = stopTime.Subtract(startTime);
                elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                FileStream fsArquivo = null;

                try
                {
                    lock(Smf.NumLote)
                    {
                        //Vou fazer quatro tentativas de leitura do arquivo XML, se falhar, vou tentando restaurar o backup
                        //do arquivo, pois pode estar com a estrutura do XML danificada. Wandrey 04/10/2011
                        for(var i = 0; i < 5; i++)
                        {
                            if(!File.Exists(NomeArqXmlLote))
                            {
                                SalvarNumeroLoteUtilizado(numeroLote, null);
                                break;
                            }
                            else
                            {
                                if(Functions.FileInUse(NomeArqXmlLote))
                                {
                                    continue;
                                }

                                var xmlNumLote = new XmlDocument();
                                fsArquivo = new FileStream(NomeArqXmlLote, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

                                try
                                {
                                    xmlNumLote.Load(fsArquivo);
                                    var list = xmlNumLote.GetElementsByTagName("DadosLoteNfe");
                                    var elem = (XmlElement)list[0];
                                    numeroLote = Convert.ToInt32(elem.GetElementsByTagName("UltimoLoteEnviado")[0].InnerText) + 1;

                                    //Vou somar uns 3 números para frente para evitar repetir os números.
                                    if(deuErro)
                                    {
                                        numeroLote += 3;
                                    }

                                    SalvarNumeroLoteUtilizado(numeroLote, fsArquivo);

                                    break;
                                }
                                catch
                                {
                                    deuErro = true;

                                    fsArquivo.Close();
                                    switch(i)
                                    {
                                        case 0:
                                            if(File.Exists(Bkp1NomeArqXmlLote))
                                            {
                                                File.Copy(Bkp1NomeArqXmlLote, NomeArqXmlLote, true);
                                            }

                                            break;

                                        case 1:
                                            if(File.Exists(Bkp2NomeArqXmlLote))
                                            {
                                                File.Copy(Bkp2NomeArqXmlLote, NomeArqXmlLote, true);
                                            }

                                            break;

                                        case 2:
                                            if(File.Exists(Bkp3NomeArqXmlLote))
                                            {
                                                File.Copy(Bkp3NomeArqXmlLote, NomeArqXmlLote, true);
                                            }

                                            break;

                                        case 3:
                                            if(File.Exists(Bkp1NomeArqXmlLote))
                                            {
                                                File.Delete(Bkp1NomeArqXmlLote);
                                            }

                                            if(File.Exists(Bkp2NomeArqXmlLote))
                                            {
                                                File.Delete(Bkp2NomeArqXmlLote);
                                            }

                                            if(File.Exists(Bkp3NomeArqXmlLote))
                                            {
                                                File.Delete(Bkp3NomeArqXmlLote);
                                            }

                                            if(File.Exists(NomeArqXmlLote))
                                            {
                                                File.Delete(NomeArqXmlLote);
                                            }

                                            break;

                                        case 4:
                                            throw new Exception("Não foi possível efetuar a leitura do arquivo " + NomeArqXmlLote + ". Verifique se o mesmo não está com sua estrutura de XML danificada."); //Se tentou 4 vezes e deu errado, vamos retornar o erro e não tem o que ser feito.
                                    }
                                }
                                finally
                                {
                                }
                            }
                        }

                        break;
                    }
                }
                catch
                {
                    if(fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if(elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }

            return numeroLote;
        }

        #endregion ProximoNumeroLote()

        #region SalvarNumeroLoteUtilizado()

        /// <summary>
        /// Salva em XML o número do ultimo lote utilizado para envio
        /// </summary>
        /// <param name="intNumeroLote">Numero do lote a ser salvo</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void SalvarNumeroLoteUtilizado(int intNumeroLote, FileStream fsArq)
        {
            var oSettings = new XmlWriterSettings();
            var c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;
            XmlWriter oXmlGravar = null;

            try
            {
                if(fsArq != null)
                {
                    fsArq.Close();
                }

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
            finally
            {
                //Fechar o arquivo se o mesmo ainda estiver aberto - Wandrey 20/04/2010
                if(oXmlGravar != null)
                {
                    if(oXmlGravar.WriteState != WriteState.Closed)
                    {
                        oXmlGravar.Close();
                    }
                }
            }
        }

        #endregion SalvarNumeroLoteUtilizado()

        #region GravarXMLLoteRetERP()

        /// <summary>
        /// Grava um XML com o número de lote utilizado na pasta de retorno para que o ERP possa pegar este número
        /// </summary>
        /// <param name="intNumeroLote">Número do lote a ser gravado no retorno para o ERP</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void GravarXMLLoteRetERP(int intNumeroLote, string NomeArquivoXML)
        {
            var oSettings = new XmlWriterSettings();
            var c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;
            XmlWriter oXmlLoteERP = null;

            var cArqLoteRetorno = NomeArqLoteRetERP(NomeArquivoXML);

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

                var emp = Empresas.FindEmpresaByThread();
                if(Empresas.Configuracoes[emp].GravarRetornoTXTNFe)
                {
                    var TXTRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(cArqLoteRetorno, ".xml") + ".txt";

                    File.WriteAllText(TXTRetorno, intNumeroLote.ToString() + ";");
                }
            }
            finally
            {
                //Fechar o arquivo se o mesmo ainda estiver aberto - Wandrey 20/04/2010
                if(oXmlLoteERP != null)
                {
                    if(oXmlLoteERP.WriteState != WriteState.Closed)
                    {
                        oXmlLoteERP.Close();
                    }
                }
            }
        }

        #endregion GravarXMLLoteRetERP()

        #endregion Métodos para gerar o Lote de Notas Fiscais Eletrônicas

        #region Métodos para gerar o XML´s diversos

        #region Consulta()

        /// <summary>
        /// Gera arquivo XML de consulta situação da NFe, CTe ou MDFe
        /// </summary>
        /// <param name="tipoAplicativo">Tipo do aplicativo, se NFe, CTe ou MDFe</param>
        /// <param name="Arquivo">Final do arquivo a ser gerado</param>
        /// <param name="tpAmb">Tipo de ambiente</param>
        /// <param name="tpEmis">Tipo de emissão</param>
        /// <param name="chNFe">Chave da NFe, CTe ou MDFe</param>
        /// <param name="versao">Versão do schema do XML</param>
        public void Consulta(TipoAplicativo tipoAplicativo, string Arquivo, int tpAmb, int tpEmis, string chNFe, string versao)
        {
            var xmlDados = string.Empty;
            switch(tipoAplicativo)
            {
                case TipoAplicativo.Cte:
                    xmlDados = ConsultaCTe(tpAmb, tpEmis, chNFe, versao);
                    break;

                case TipoAplicativo.NFCe:
                case TipoAplicativo.Nfe:
                    xmlDados = ConsultaNFe(tpAmb, tpEmis, chNFe, versao);
                    break;

                case TipoAplicativo.MDFe:
                    xmlDados = ConsultaMDFe(tpAmb, tpEmis, chNFe, versao);
                    break;
            }

            GravarArquivoParaEnvio(Arquivo, xmlDados);
        }

        #region ConsultaNFe()

        /// <summary>
        /// Gera uma string com o XML de consulta (-ped-sit.xml) da NFe
        /// </summary>
        /// <param name="tpAmb">Tipo de ambiente</param>
        /// <param name="tpEmis">Tipo de emissão</param>
        /// <param name="chNFe">Chave da NFe</param>
        /// <param name="versao">Versão do schema do XML</param>
        /// <returns>Retorna uma sting com o XML de consulta situação da NFe (-ped-sit.xml)</returns>
        private string ConsultaNFe(int tpAmb, int tpEmis, string chNFe, string versao)
        {
            var xml = new ConsSitNFe
            {
                ChNFe = chNFe,
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)tpAmb,
                Versao = versao
            };

            var doc = xml.GerarXML();
            var xmlNode = doc.GetElementsByTagName("consSitNFe")[0];
            xmlNode.AppendChild(CriaElemento(doc, TpcnResources.tpEmis.ToString(), tpEmis.ToString(), NFeStrConstants.NAME_SPACE_NFE));
            doc.AppendChild(xmlNode);

            return doc.OuterXml;
        }

        #endregion ConsultaNFe()

        #region ConsultaCTe()

        /// <summary>
        /// Gera uma string com o XML de consulta (-ped-sit.xml) da CTe
        /// </summary>
        /// <param name="tpAmb">Tipo de ambiente</param>
        /// <param name="tpEmis">Tipo de emissão</param>
        /// <param name="chCTe">Chave da CTe</param>
        /// <param name="versao">Versão do schema do XML</param>
        /// <returns>Retorna uma sting com o XML de consulta situação da CTe (-ped-sit.xml)</returns>
        private string ConsultaCTe(int tpAmb, int tpEmis, string chCTe, string versao)
        {

            var xml = new ConsSitCTe
            {
                ChCTe = chCTe,
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)tpAmb,
                Versao = versao
            };

            var doc = xml.GerarXML();
            var xmlNode = doc.GetElementsByTagName("consSitCTe")[0];
            xmlNode.AppendChild(CriaElemento(doc, TpcnResources.tpEmis.ToString(), tpEmis.ToString(), NFeStrConstants.NAME_SPACE_CTE));
            doc.AppendChild(xmlNode);

            return doc.OuterXml;
        }

        #endregion ConsultaCTe()

        #region ConsultaMDFe()

        /// <summary>
        /// Gera uma string com o XML de consulta (-ped-sit.xml) da MDFe
        /// </summary>
        /// <param name="tpAmb">Tipo de ambiente</param>
        /// <param name="tpEmis">Tipo de emissão</param>
        /// <param name="chMDFe">Chave da MDFe</param>
        /// <param name="versao">Versão do schema do XML</param>
        /// <returns>Retorna uma sting com o XML de consulta situação da MDFe (-ped-sit.xml)</returns>
        private string ConsultaMDFe(int tpAmb, int tpEmis, string chMDFe, string versao)
        {
            var xml = new ConsSitMDFe
            {
                ChMDFe = chMDFe,
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)tpAmb,
                Versao = versao
            };

            var doc = xml.GerarXML();
            var xmlNode = doc.GetElementsByTagName("consSitMDFe")[0];
            xmlNode.AppendChild(CriaElemento(doc, TpcnResources.tpEmis.ToString(), tpEmis.ToString(), NFeStrConstants.NAME_SPACE_MDFE));
            doc.AppendChild(xmlNode);

            return doc.OuterXml;
        }

        #endregion ConsultaMDFe()

        #endregion Consulta()

        #region ConsultaCadastro()

        /// <summary>
        /// Cria um arquivo XML com a estrutura necessária para consultar um cadastro
        /// Voce deve preencher o estado e mais um dos tres itens: CPNJ, IE ou CPF
        /// </summary>
        /// <param name="uf">Sigla do UF do cadastro a ser consultado. Tem que ter duas letras. SU para suframa.</param>
        /// <param name="cnpj"></param>
        /// <param name="ie"></param>
        /// <param name="cpf"></param>
        /// <returns>Retorna o caminho e nome do arquivo criado</returns>
        /// <by>Marcos Diez</by>
        /// <date>29/08/2009</date>
        public string ConsultaCadastro(string pArquivo, string uf, string cnpj, string ie, string cpf, string versao)
        {
            var emp = EmpIndex;
            cnpj = OnlyNumbers(cnpj);
            ie = OnlyNumbers(ie);
            cpf = OnlyNumbers(cpf);

            if(string.IsNullOrEmpty(versao))
            {
                versao = NFe.ConvertTxt.versoes.VersaoXMLConsCad;
            }

            var xml = new Unimake.Business.DFe.Xml.NFe.ConsCad
            {
                Versao = versao,
                InfCons = new InfCons()
                {
                    CNPJ = cnpj,
                    IE = ie,
                    CPF = cpf,
                    UF = (Unimake.Business.DFe.Servicos.UFBrasil)Enum.Parse(typeof(Unimake.Business.DFe.Servicos.UFBrasil), uf)
                }
            };

            var _arquivo_saida = (string.IsNullOrEmpty(pArquivo) ? DateTime.Now.ToString("yyyyMMddTHHmmss") + Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML : pArquivo);

            GravarArquivoParaEnvio(_arquivo_saida, xml.GerarXML().OuterXml);

            if(_arquivo_saida.ToLower().IndexOf(Empresas.Configuracoes[emp].PastaValidar.ToLower()) >= 0)
            {
                return _arquivo_saida;
            }

            return Empresas.Configuracoes[emp].PastaXmlEnvio + "\\" + _arquivo_saida;
        }

        /// <summary>
        /// retorna uma string contendo apenas os digitos da entrada
        /// </summary>
        /// <by>Marcos Diez</by>
        /// <date>29/08/2009</date>
        private static string OnlyNumbers(string entrada)
        {
            if(string.IsNullOrEmpty(entrada))
            {
                return null;
            }

            var saida = new StringBuilder(entrada.Length);
            foreach(var c in entrada)
            {
                if(char.IsDigit(c))
                {
                    saida.Append(c);
                }
            }
            return saida.ToString();
        }

        #endregion ConsultaCadastro()

        #region Inutilizacao

        public void Inutilizacao(string pFinalArqEnvio, int tpAmb, int tpEmis, int cUF, int ano, string CNPJ, int mod, int serie, int nNFIni, int nNFFin, string xJust, string versao)
        {
            var xml = new InutNFe
            {
                Versao = versao,
                InfInut = new InutNFeInfInut
                {
                    Ano = ano.ToString("00"),
                    CNPJ = CNPJ.Trim(),
                    CUF = (Unimake.Business.DFe.Servicos.UFBrasil)cUF,
                    Mod = (Unimake.Business.DFe.Servicos.ModeloDFe)mod,
                    NNFIni = nNFIni,
                    NNFFin = nNFFin,
                    Serie = serie,
                    TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)tpAmb,
                    XJust = xJust.TrimStart().TrimEnd()
                }
            };

            var doc = xml.GerarXML();
            var xmlNode = doc.GetElementsByTagName("infInut")[0];
            xmlNode.AppendChild(CriaElemento(doc, TpcnResources.tpEmis.ToString(), tpEmis.ToString(), NFeStrConstants.NAME_SPACE_NFE));

            GravarArquivoParaEnvio(pFinalArqEnvio, doc.OuterXml);
        }

        #endregion

        #region StatusServico()

        /// <summary>
        /// Consulta status do serviço da NFe, CTe e MDFe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="tpEmis">Tipo de emissão</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="amb">Tipo de Ambiente</param>
        /// <returns>Retorna o nome e pasta do arquivo xml gerado</returns>
        public string StatusServico(TipoAplicativo servico, int tpEmis, int cUF, int amb, string versao)
        {
            var arquivoSaida = DateTime.Now.ToString("yyyyMMddTHHmmss") + Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML;

            switch(servico)
            {
                case TipoAplicativo.Cte:
                    StatusServicoCTe(arquivoSaida, amb, tpEmis, cUF, versao);
                    break;

                case TipoAplicativo.NFCe:
                case TipoAplicativo.Nfe:
                    StatusServicoNFe(arquivoSaida, amb, tpEmis, cUF, versao);
                    break;

                case TipoAplicativo.MDFe:
                    StatusServicoMDFe(arquivoSaida, amb, tpEmis, cUF, versao);
                    break;
            }

            return arquivoSaida;
        }

        #region StatusServicoNFe()

        /// <summary>
        /// Gera o XML de consulta status do serviço da NFe
        /// </summary>
        /// <param name="pArquivo">Caminho e nome do arquivo que é para ser gerado</param>
        /// <param name="tpAmb">Ambiente da consulta</param>
        /// <param name="tpEmis">Tipo de emissão da consulta</param>
        /// <param name="cUF">Estado para a consulta</param>
        /// <param name="versao">Versão do schema do XML</param>
        public void StatusServicoNFe(string pArquivo, int tpAmb, int tpEmis, int cUF, string versao)
        {
            var xml = new ConsStatServ
            {
                CUF = (Unimake.Business.DFe.Servicos.UFBrasil)cUF,
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)tpAmb,
                Versao = versao
            };

            var doc = xml.GerarXML();

            if(tpEmis != 1 && pArquivo.ToLower().IndexOf(Empresas.Configuracoes[EmpIndex].PastaValidar.ToLower()) == -1)
            {
                var xmlNode = doc.GetElementsByTagName("consStatServ")[0];
                xmlNode.AppendChild(CriaElemento(doc, TpcnResources.tpEmis.ToString(), tpEmis.ToString(), NFeStrConstants.NAME_SPACE_NFE));
                doc.AppendChild(xmlNode);
            }

            GravarArquivoParaEnvio(pArquivo, doc.OuterXml);
        }

        #endregion StatusServicoNFe()

        #region StatusServicoCTe()

        /// <summary>
        /// Gera o XML de consulta status do serviço do CTe
        /// </summary>
        /// <param name="pArquivo">Caminho e nome do arquivo que é para ser gerado</param>
        /// <param name="tpAmb">Ambiente da consulta</param>
        /// <param name="tpEmis">Tipo de emissão da consulta</param>
        /// <param name="cUF">Estado para a consulta</param>
        /// <param name="versao">Versão do schema do XML</param>
        public void StatusServicoCTe(string pArquivo, int tpAmb, int tpEmis, int cUF, string versao)
        {
            var xml = new ConsStatServCte
            {
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)tpAmb,
                Versao = versao
            };

            var doc = xml.GerarXML();

            var xmlNode = doc.GetElementsByTagName("consStatServCte")[0];
            xmlNode.AppendChild(CriaElemento(doc, TpcnResources.cUF.ToString(), cUF.ToString(), NFeStrConstants.NAME_SPACE_CTE));

            if(tpEmis != 1 && pArquivo.ToLower().IndexOf(Empresas.Configuracoes[EmpIndex].PastaValidar.ToLower()) == -1)
            {
                xmlNode.AppendChild(CriaElemento(doc, TpcnResources.tpEmis.ToString(), tpEmis.ToString(), NFeStrConstants.NAME_SPACE_CTE));
                doc.AppendChild(xmlNode);
            }

            GravarArquivoParaEnvio(pArquivo, doc.OuterXml);
        }

        #endregion StatusServicoCTe()

        #region StatusServicoMDFe()

        /// <summary>
        /// Gera o XML de consulta status do serviço do MDFe
        /// </summary>
        /// <param name="pArquivo">Caminho e nome do arquivo que é para ser gerado</param>
        /// <param name="tpAmb">Ambiente da consulta</param>
        /// <param name="tpEmis">Tipo de emissão da consulta</param>
        /// <param name="cUF">Estado para a consulta</param>
        /// <param name="versao">Versão do schema do XML</param>
        public void StatusServicoMDFe(string pArquivo, int tpAmb, int tpEmis, int cUF, string versao)
        {
            var xml = new ConsStatServMDFe
            {
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)tpAmb,
                Versao = versao
            };

            var doc = xml.GerarXML();

            var xmlNode = doc.GetElementsByTagName("consStatServMDFe")[0];
            xmlNode.AppendChild(CriaElemento(doc, TpcnResources.cUF.ToString(), cUF.ToString(), NFeStrConstants.NAME_SPACE_MDFE));

            if(tpEmis != 1 && pArquivo.ToLower().IndexOf(Empresas.Configuracoes[EmpIndex].PastaValidar.ToLower()) == -1)
            {
                xmlNode.AppendChild(CriaElemento(doc, TpcnResources.tpEmis.ToString(), tpEmis.ToString(), NFeStrConstants.NAME_SPACE_MDFE));
                doc.AppendChild(xmlNode);
            }

            GravarArquivoParaEnvio(pArquivo, doc.OuterXml);
        }

        #endregion StatusServicoMDFe()

        #endregion StatusServico()

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
            var emp = Empresas.FindEmpresaByThread();
            XmlRetorno(finalArqEnvio, finalArqRetorno, conteudoXMLRetorno, Empresas.Configuracoes[emp].PastaXmlRetorno);
        }

        #endregion XmlRetorno()

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
        public void XmlRetorno(string finalArqEnvio, string finalArqRetorno, string conteudoXMLRetorno, string pastaGravar) => XmlRetorno(finalArqEnvio, finalArqRetorno, conteudoXMLRetorno, pastaGravar, NomeXMLDadosMsg);

        #endregion XmlRetorno()

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
        public void XmlRetorno(string finalArqEnvio, string finalArqRetorno, string conteudoXMLRetorno, string pastaGravar, string nomeXMLDadosMsg)
        {
            var emp = Empresas.FindEmpresaByThread();
            try
            {
                //Deletar o arquivo XML da pasta de temporários de XML´s com erros se o mesmo existir
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Functions.ExtrairNomeArq(nomeXMLDadosMsg, ".xml") + ".xml");

                //Gravar o arquivo XML de retorno
                var ArqXMLRetorno = pastaGravar + "\\" +
                                       Functions.ExtrairNomeArq(nomeXMLDadosMsg, finalArqEnvio) +
                                       finalArqRetorno;

                File.WriteAllText(ArqXMLRetorno, conteudoXMLRetorno);

                //gravar o conteudo no FTP
                XmlParaFTP(emp, ArqXMLRetorno);

                //Gravar o XML de retorno também no formato TXT
                if(Empresas.Configuracoes[emp].GravarRetornoTXTNFe)
                {
                    TXTRetorno(finalArqEnvio, finalArqRetorno, conteudoXMLRetorno);
                }
            }
            finally
            {
            }
        }

        #endregion XmlRetorno()

        #region GravarRetornoEmTXT()

        protected void TXTRetorno(string pFinalArqEnvio, string pFinalArqRetorno, string ConteudoXMLRetorno)
        {
            var emp = EmpIndex;
            var temEvento = false;
            var ConteudoRetorno = string.Empty;

            MemoryStream msXml;
            if(Servico == Servicos.NFePedidoConsultaSituacao)
            {
                msXml = Functions.StringXmlToStreamUTF8(ConteudoXMLRetorno);
            }
            else
            {
                msXml = Functions.StringXmlToStream(ConteudoXMLRetorno);
            }

            switch(Servico)
            {
                case Servicos.NFeEnviarLote:
                    {
                        #region Servicos.EnviarLoteNfe

                        var docRec = new XmlDocument();
                        docRec.Load(msXml);

                        var retEnviNFeList = docRec.GetElementsByTagName("retEnviNFe");
                        if(retEnviNFeList != null) //danasa 23-9-2009
                        {
                            if(retEnviNFeList.Count > 0)   //danasa 23-9-2009
                            {
                                var retEnviNFeElemento = (XmlElement)retEnviNFeList.Item(0);
                                if(retEnviNFeElemento != null)   //danasa 23-9-2009
                                {
                                    ConteudoRetorno += (Empresas.Configuracoes[emp].IndSinc || Empresas.Configuracoes[emp].IndSincNFCe ? ";" : "");
                                    ConteudoRetorno += Functions.LerTag(retEnviNFeElemento, NFe.Components.TpcnResources.cStat.ToString());
                                    ConteudoRetorno += Functions.LerTag(retEnviNFeElemento, NFe.Components.TpcnResources.xMotivo.ToString());

                                    #region Processo assíncrono

                                    var infRecList = retEnviNFeElemento.GetElementsByTagName("infRec");
                                    if(infRecList != null)
                                    {
                                        if(infRecList.Count > 0)   //danasa 23-9-2009
                                        {
                                            var infRecElemento = (XmlElement)infRecList.Item(0);
                                            if(infRecElemento != null)   //danasa 23-9-2009
                                            {
                                                ConteudoRetorno += Functions.LerTag(infRecElemento, NFe.Components.TpcnResources.nRec.ToString());
                                                ConteudoRetorno += Functions.LerTag(infRecElemento, NFe.Components.TpcnResources.dhRecbto.ToString());
                                                ConteudoRetorno += Functions.LerTag(infRecElemento, TpcnResources.tMed.ToString());
                                            }
                                        }
                                    }

                                    #endregion

                                    #region Processo síncrono

                                    var infProtList = retEnviNFeElemento.GetElementsByTagName("infProt");
                                    if(infProtList != null)
                                    {
                                        if(infProtList.Count > 0)
                                        {
                                            var infProtElemento = (XmlElement)infProtList.Item(0);
                                            if(infProtElemento != null)   //danasa 23-9-2009
                                            {
                                                var chNFe = Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.chNFe.ToString());

                                                ConteudoRetorno += "\r\n";
                                                ConteudoRetorno += chNFe.Substring(6, 14) + ";";
                                                ConteudoRetorno += chNFe.Substring(25, 9) + ";";
                                                ConteudoRetorno += chNFe;
                                                ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.dhRecbto.ToString());
                                                ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.nProt.ToString());
                                                ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.digVal.ToString());
                                                ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.cStat.ToString());
                                                ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.xMotivo.ToString());
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                        }

                        #endregion Servicos.EnviarLoteNfe
                    }
                    break;

                case Servicos.NFePedidoSituacaoLote:
                    {
                        #region Servicos.PedidoSituacaoLoteNFe

                        var docProRec = new XmlDocument();
                        docProRec.Load(msXml);

                        var retConsReciNFeList = docProRec.GetElementsByTagName("retConsReciNFe");
                        if(retConsReciNFeList != null) //danasa 23-9-2009
                        {
                            if(retConsReciNFeList.Count > 0)   //danasa 23-9-2009
                            {
                                var retConsReciNFeElemento = (XmlElement)retConsReciNFeList.Item(0);
                                if(retConsReciNFeElemento != null)   //danasa 23-9-2009
                                {
                                    ConteudoRetorno += Functions.LerTag(retConsReciNFeElemento, NFe.Components.TpcnResources.nRec.ToString());
                                    ConteudoRetorno += Functions.LerTag(retConsReciNFeElemento, NFe.Components.TpcnResources.cStat.ToString());
                                    ConteudoRetorno += Functions.LerTag(retConsReciNFeElemento, NFe.Components.TpcnResources.xMotivo.ToString());
                                    ConteudoRetorno += "\r\n";

                                    var protNFeList = retConsReciNFeElemento.GetElementsByTagName("protNFe");
                                    if(protNFeList != null)    //danasa 23-9-2009
                                    {
                                        if(protNFeList.Count > 0)   //danasa 23-9-2009
                                        {
                                            var protNFeElemento = (XmlElement)protNFeList.Item(0);
                                            if(protNFeElemento != null)
                                            {
                                                if(protNFeElemento.ChildNodes.Count > 0)
                                                {
                                                    var infProtList = protNFeElemento.GetElementsByTagName("infProt");

                                                    foreach(XmlNode infProtNode in infProtList)
                                                    {
                                                        var infProtElemento = (XmlElement)infProtNode;
                                                        var chNFe = Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.chNFe.ToString());

                                                        ConteudoRetorno += chNFe.Substring(6, 14) + ";";
                                                        ConteudoRetorno += chNFe.Substring(25, 9) + ";";
                                                        ConteudoRetorno += chNFe;
                                                        ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.dhRecbto.ToString());
                                                        ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.nProt.ToString());
                                                        ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.digVal.ToString());
                                                        ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.cStat.ToString());
                                                        ConteudoRetorno += Functions.LerTag(infProtElemento, NFe.Components.TpcnResources.xMotivo.ToString());
                                                        ConteudoRetorno += "\r\n";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Servicos.PedidoSituacaoLoteNFe
                    }
                    break;

                case Servicos.NFeInutilizarNumeros: //danasa 19-9-2009
                    {
                        #region Servicos.InutilizarNumerosNFe

                        var docretInut = new XmlDocument();
                        docretInut.Load(msXml);

                        var retInutList = docretInut.GetElementsByTagName("retInutNFe");
                        if(retInutList != null)
                        {
                            if(retInutList.Count > 0)
                            {
                                var retInutElemento = (XmlElement)retInutList.Item(0);
                                if(retInutElemento != null)
                                {
                                    if(retInutElemento.ChildNodes.Count > 0)
                                    {
                                        var infInutList = retInutElemento.GetElementsByTagName("infInut");
                                        if(infInutList != null)
                                        {
                                            foreach(XmlNode infInutNode in infInutList)
                                            {
                                                var infInutElemento = (XmlElement)infInutNode;

                                                ConteudoRetorno += Functions.LerTag(infInutElemento, NFe.Components.TpcnResources.tpAmb.ToString());
                                                ConteudoRetorno += Functions.LerTag(infInutElemento, NFe.Components.TpcnResources.cStat.ToString());
                                                ConteudoRetorno += Functions.LerTag(infInutElemento, NFe.Components.TpcnResources.xMotivo.ToString());
                                                ConteudoRetorno += Functions.LerTag(infInutElemento, NFe.Components.TpcnResources.cUF.ToString());
                                                ConteudoRetorno += "\r\n";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Servicos.InutilizarNumerosNFe
                    }
                    break;

                case Servicos.NFePedidoConsultaSituacao:   //danasa 19-9-2009
                    {
                        #region Servicos.PedidoConsultaSituacaoNFe

                        var docretConsSit = new XmlDocument();
                        docretConsSit.Load(msXml);

                        var retConsSitList = docretConsSit.GetElementsByTagName("retConsSitNFe");
                        if(retConsSitList != null)
                        {
                            if(retConsSitList.Count > 0)
                            {
                                var retConsSitElemento = (XmlElement)retConsSitList.Item(0);
                                if(retConsSitElemento != null)
                                {
                                    if(retConsSitElemento.ChildNodes.Count > 0)
                                    {
                                        var infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                                        if(infConsSitList != null)
                                        {
                                            foreach(XmlNode infConsSitNode in infConsSitList)
                                            {
                                                var infConsSitElemento = (XmlElement)infConsSitNode;

                                                ConteudoRetorno += Functions.LerTag(infConsSitElemento, NFe.Components.TpcnResources.tpAmb.ToString());
                                                ConteudoRetorno += Functions.LerTag(infConsSitElemento, NFe.Components.TpcnResources.cStat.ToString());
                                                ConteudoRetorno += Functions.LerTag(infConsSitElemento, NFe.Components.TpcnResources.xMotivo.ToString());
                                                ConteudoRetorno += Functions.LerTag(infConsSitElemento, NFe.Components.TpcnResources.cUF.ToString());
                                                ConteudoRetorno += Functions.LerTag(infConsSitElemento, NFe.Components.TpcnResources.dhRecbto.ToString());
                                                ConteudoRetorno += Functions.LerTag(infConsSitElemento, NFe.Components.TpcnResources.nProt.ToString());
                                                ConteudoRetorno += "\r\n";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ///
                        /// grava os eventos
                        ///
                        var retprocEventoNFeList = docretConsSit.GetElementsByTagName("procEventoNFe");
                        if(retprocEventoNFeList != null)
                        {
                            foreach(XmlNode retConsSitNode1 in retprocEventoNFeList)
                            {
                                foreach(XmlNode retConsSitNode2 in retConsSitNode1.ChildNodes)
                                {
                                    if(((XmlElement)retConsSitNode2).Name == "evento" ||
                                        ((XmlElement)retConsSitNode2).Name == "retEvento")
                                    {
                                        foreach(XmlNode retConsSitNode3 in retConsSitNode2.ChildNodes)
                                        {
                                            if(((XmlElement)retConsSitNode3).Name == "infEvento")
                                            {
                                                var cRetorno = "";
                                                foreach(XmlNode retConsSitNode4 in retConsSitNode3.ChildNodes)
                                                {
                                                    switch(((XmlElement)retConsSitNode4).Name)
                                                    {
                                                        case "detEvento":
                                                            foreach(XmlNode retConsSitNode5 in retConsSitNode4.ChildNodes)
                                                            {
                                                                switch(((XmlElement)retConsSitNode5).Name)
                                                                {
                                                                    //case "descEvento":
                                                                    case "xCondUso":
                                                                        break;

                                                                    default:
                                                                        cRetorno += Functions.LerTag((XmlElement)retConsSitNode4, ((XmlElement)retConsSitNode5).Name);
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        default:
                                                            cRetorno += Functions.LerTag((XmlElement)retConsSitNode3, ((XmlElement)retConsSitNode4).Name);
                                                            break;
                                                    }
                                                }
                                                if(cRetorno != "")
                                                {
                                                    ConteudoRetorno += "[" + ((XmlElement)retConsSitNode2).Name + "]\r\n";
                                                    ConteudoRetorno += cRetorno + "\r\n";
                                                    temEvento = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Servicos.PedidoConsultaSituacaoNFe
                    }
                    break;

                case Servicos.NFeConsultaStatusServico:   //danasa 19-9-2009
                    {
                        #region Servicos.PedidoConsultaStatusServicoNFe

                        var docConsStat = new XmlDocument();
                        docConsStat.Load(msXml);

                        var retConsStatServList = docConsStat.GetElementsByTagName("retConsStatServ");
                        if(retConsStatServList != null)
                        {
                            if(retConsStatServList.Count > 0)
                            {
                                var retConsStatServElemento = (XmlElement)retConsStatServList.Item(0);
                                if(retConsStatServElemento != null)
                                {
                                    ConteudoRetorno += Functions.LerTag(retConsStatServElemento, NFe.Components.TpcnResources.tpAmb.ToString());
                                    ConteudoRetorno += Functions.LerTag(retConsStatServElemento, NFe.Components.TpcnResources.cStat.ToString());
                                    ConteudoRetorno += Functions.LerTag(retConsStatServElemento, NFe.Components.TpcnResources.xMotivo.ToString());
                                    ConteudoRetorno += Functions.LerTag(retConsStatServElemento, NFe.Components.TpcnResources.cUF.ToString());
                                    ConteudoRetorno += Functions.LerTag(retConsStatServElemento, NFe.Components.TpcnResources.dhRecbto.ToString());
                                    ConteudoRetorno += Functions.LerTag(retConsStatServElemento, TpcnResources.tMed.ToString());
                                    ConteudoRetorno += "\r\n";
                                }
                            }
                        }

                        #endregion Servicos.PedidoConsultaStatusServicoNFe
                    }
                    break;

                case Servicos.ConsultaCadastroContribuinte: //danasa 19-9-2009
                    {
                        #region Servicos.ConsultaCadastroContribuinte

                        ///
                        /// Retorna o texto conforme o manual do Sefaz versao 3.0
                        ///
                        var rconscad = ProcessaConsultaCadastro(msXml);
                        if(rconscad != null)
                        {
                            ConteudoRetorno = rconscad.cStat.ToString("000") + ";";
                            ConteudoRetorno += rconscad.xMotivo.Replace(";", " ") + ";";
                            ConteudoRetorno += rconscad.UF + ";";
                            ConteudoRetorno += rconscad.IE + ";";
                            ConteudoRetorno += rconscad.CNPJ + ";";
                            ConteudoRetorno += rconscad.CPF + ";";
                            ConteudoRetorno += rconscad.dhCons + ";";
                            ConteudoRetorno += rconscad.cUF.ToString("00") + ";";
                            ConteudoRetorno += "\r\r";
                            foreach(var infCadNode in rconscad.infCad)
                            {
                                ConteudoRetorno += infCadNode.IE + ";";
                                ConteudoRetorno += infCadNode.CNPJ + ";";
                                ConteudoRetorno += infCadNode.CPF + ";";
                                ConteudoRetorno += infCadNode.UF + ";";
                                ConteudoRetorno += infCadNode.cSit + ";";
                                ConteudoRetorno += infCadNode.xNome.Replace(";", " ") + ";";
                                ConteudoRetorno += (string.IsNullOrEmpty(infCadNode.xFant) ? "" : infCadNode.xFant.Replace(";", " ")) + ";";
                                ConteudoRetorno += infCadNode.xRegApur.Replace(";", " ") + ";";
                                ConteudoRetorno += infCadNode.CNAE.ToString() + ";";
                                ConteudoRetorno += infCadNode.dIniAtiv + ";";
                                ConteudoRetorno += infCadNode.dUltSit + ";";
                                ConteudoRetorno += (string.IsNullOrEmpty(infCadNode.IEUnica) ? "" : infCadNode.IEUnica.Replace(";", " ")) + ";";
                                ConteudoRetorno += (string.IsNullOrEmpty(infCadNode.IEAtual) ? "" : infCadNode.IEAtual.Replace(";", " ")) + ";";
                                ConteudoRetorno += infCadNode.ender.xLgr.Replace(";", " ") + ";";
                                ConteudoRetorno += infCadNode.ender.nro.Replace(";", " ") + ";";
                                ConteudoRetorno += (string.IsNullOrEmpty(infCadNode.ender.xCpl) ? "" : infCadNode.ender.xCpl.Replace(";", " ")) + ";";
                                ConteudoRetorno += infCadNode.ender.xBairro.Replace(";", " ") + ";";
                                ConteudoRetorno += infCadNode.ender.cMun.ToString("0000000") + ";";
                                ConteudoRetorno += (string.IsNullOrEmpty(infCadNode.ender.xMun) ? "" : infCadNode.ender.xMun.Replace(";", " ")) + ";";
                                ConteudoRetorno += infCadNode.ender.CEP.ToString("00000000") + ";";
                                ConteudoRetorno += "\r\r";
                            }
                        }

                        #endregion Servicos.ConsultaCadastroContribuinte
                    }
                    break;

                case Servicos.CTeRecepcaoEvento:
                    break;

                case Servicos.UniNFeConsultaInformacoes:
                    break;

                case Servicos.DFeEnviar:
                case Servicos.CTeDistribuicaoDFe:

                    #region Servicos.EnviarDFe

                    var doc = new XmlDocument();
                    doc.Load(msXml);
                    var envEventoList = doc.GetElementsByTagName("retDistDFeInt");
                    foreach(XmlNode ret1Node in envEventoList)
                    {
                        var ret1Elemento = (XmlElement)ret1Node;
                        ConteudoRetorno += Functions.LerTag(ret1Elemento, NFe.Components.TpcnResources.tpAmb.ToString());
                        ConteudoRetorno += Functions.LerTag(ret1Elemento, NFe.Components.TpcnResources.verAplic.ToString());
                        ConteudoRetorno += Functions.LerTag(ret1Elemento, NFe.Components.TpcnResources.cStat.ToString());
                        ConteudoRetorno += Functions.LerTag(ret1Elemento, NFe.Components.TpcnResources.xMotivo.ToString());
                        ConteudoRetorno += Functions.LerTag(ret1Elemento, NFe.Components.TpcnResources.dhResp.ToString());
                        ConteudoRetorno += Functions.LerTag(ret1Elemento, NFe.Components.TpcnResources.ultNSU.ToString());
                        ConteudoRetorno += Functions.LerTag(ret1Elemento, "maxNSU");
                        ConteudoRetorno += "\r\n";

                        var ret1List = ret1Elemento.GetElementsByTagName("loteDistDFeInt");
                        foreach(XmlNode ret in ret1List)
                        {
                            for(var n = 0; n < ret.ChildNodes.Count; ++n)
                            {
                                if(ret.ChildNodes[n].Name.Equals("docZip"))
                                {
                                    var chNFe = "";
                                    var NSU = ret.ChildNodes[n].Attributes[NFe.Components.TpcnResources.NSU.ToString()].Value;

                                    ///
                                    /// descompacta o conteudo
                                    ///
                                    var xmlRes = TFunctions.Decompress(ret.ChildNodes[n].InnerText);

                                    var docres = new XmlDocument();
                                    docres.Load(Functions.StringXmlToStreamUTF8(xmlRes));

                                    if(ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("resEvento"))
                                    {
                                        var envres = docres.GetElementsByTagName("resEvento");
                                        var ret1 = (XmlElement)envres.Item(0);
                                        chNFe = Functions.LerTag(ret1, NFe.Components.TpcnResources.chNFe.ToString(), false);
                                        var nSeqEvento = Convert.ToInt32("0" + Functions.LerTag(ret1, NFe.Components.TpcnResources.nSeqEvento.ToString(), false));
                                        var tpEvento = (ConvertTxt.tpEventos)Convert.ToInt32("0" + Functions.LerTag(ret1, NFe.Components.TpcnResources.tpEvento.ToString(), false));

                                        ConteudoRetorno += "resEvento;" + NSU + ";" + chNFe + ";" + tpEvento.ToString() + ";" + nSeqEvento.ToString("00");
                                    }
                                    else if(ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procEventoNFe"))
                                    {
                                        var envres = docres.GetElementsByTagName("procEventoNFe");
                                        var ret1 = (XmlElement)envres.Item(0);
                                        chNFe = Functions.LerTag(ret1, NFe.Components.TpcnResources.chNFe.ToString(), false);
                                        var nSeqEvento = Convert.ToInt32("0" + Functions.LerTag(ret1, NFe.Components.TpcnResources.nSeqEvento.ToString(), false));
                                        var tpEvento = (ConvertTxt.tpEventos)Convert.ToInt32("0" + Functions.LerTag(ret1, NFe.Components.TpcnResources.tpEvento.ToString(), false));

                                        ConteudoRetorno += "procEventoNFe;" + NSU + ";" + chNFe + ";" + tpEvento.ToString() + ";" + nSeqEvento.ToString("00");
                                    }
                                    else if(ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procNFe"))
                                    {
                                        var envres = docres.GetElementsByTagName("nfeProc");
                                        var ret1 = (XmlElement)envres.Item(0);
                                        chNFe = Functions.LerTag(ret1, NFe.Components.TpcnResources.chNFe.ToString(), false);
                                        ConteudoRetorno += "procNFe;" + NSU + ";" + chNFe + ";";
                                    }
                                    else if(ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("resNFe"))
                                    {
                                        var envres = docres.GetElementsByTagName("resNFe");
                                        var ret1 = (XmlElement)envres.Item(0);
                                        chNFe = Functions.LerTag(ret1, NFe.Components.TpcnResources.chNFe.ToString(), false);
                                        ConteudoRetorno += "resNFe;" + NSU + ";" + chNFe + ";";
                                    }
                                    else if(ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procEventoCTe"))
                                    {
                                        var envres = docres.GetElementsByTagName("procEventoCTe");
                                        var ret1 = (XmlElement)envres.Item(0);
                                        chNFe = Functions.LerTag(ret1, NFe.Components.TpcnResources.chCTe.ToString(), false);
                                        var nSeqEvento = Convert.ToInt32("0" + Functions.LerTag(ret1, NFe.Components.TpcnResources.nSeqEvento.ToString(), false));
                                        var tpEvento = (ConvertTxt.tpEventos)Convert.ToInt32("0" + Functions.LerTag(ret1, NFe.Components.TpcnResources.tpEvento.ToString(), false));

                                        ConteudoRetorno += "procEventoCTe;" + NSU + ";" + chNFe + ";" + tpEvento.ToString() + ";" + nSeqEvento.ToString("00");
                                    }
                                    else if(ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procCTe"))
                                    {
                                        var envres = docres.GetElementsByTagName("cteProc");
                                        var ret1 = (XmlElement)envres.Item(0);
                                        chNFe = Functions.LerTag(ret1, NFe.Components.TpcnResources.chCTe.ToString(), false);
                                        ConteudoRetorno += "procCTe;" + NSU + ";" + chNFe + ";";
                                    }
                                    ConteudoRetorno += "\r\n";
                                }
                            }
                        }
                    }

                    #endregion Servicos.EnviarDFe

                    break;

                case Servicos.EventoRecepcao:
                case Servicos.EventoCancelamento:
                case Servicos.EventoManifestacaoDest:
                case Servicos.EventoCCe:    //danasa 2/7/2011
                    //<retEnvEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                    //  <idLote>000000000038313</idLote>
                    //  <tpAmb>2</tpAmb>
                    //  <verAplic>SP_EVENTOS_PL_100</verAplic>
                    //  <cOrgao>35</cOrgao>
                    //  <cStat>128</cStat>
                    //  <xMotivo>Lote de Evento Processado</xMotivo>
                    //  <retEvento versao="1.00">
                    //      <infEvento>
                    //          <tpAmb>2</tpAmb>
                    //          <verAplic>SP_EVENTOS_PL_100</verAplic>
                    //          <cOrgao>35</cOrgao>
                    //          <cStat>494</cStat>
                    //          <xMotivo>Rejeição: Chave de Acesso inexistente para o tpEvento que exige a existência da NF-e</xMotivo>
                    //          <chNFe>35100610238568000107550010000051260000038315</chNFe>
                    //          <dhRegEvento>2011-07-02T02:44:51-03:00</dhRegEvento>
                    //      </infEvento>
                    //  </retEvento>
                    //</retEnvEvento>
                    {
                        #region Servicos.EnviarCCe

                        var docretEnvCCe = new XmlDocument();
                        docretEnvCCe.Load(msXml);

                        var retCCeList = docretEnvCCe.GetElementsByTagName("retEnvEvento");
                        if(retCCeList != null)
                        {
                            if(retCCeList.Count > 0)
                            {
                                var retCCeElemento = (XmlElement)retCCeList.Item(0);
                                if(retCCeElemento != null)
                                {
                                    if(retCCeElemento.ChildNodes.Count > 0)
                                    {
                                        var infCCeList = retCCeElemento.GetElementsByTagName("retEvento");
                                        if(infCCeList != null)
                                        {
                                            foreach(XmlNode infCCeNode in infCCeList)
                                            {
                                                foreach(XmlNode infCCeNode2 in infCCeNode.ChildNodes)
                                                {
                                                    var infCCeElemento = (XmlElement)infCCeNode2;

                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.tpAmb.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.cOrgao.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.cStat.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.xMotivo.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.chNFe.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.tpEvento.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.xEvento.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.nSeqEvento.ToString());
                                                    var FCNPJCPF = Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.CNPJDest.ToString(), false);
                                                    if(string.IsNullOrEmpty(FCNPJCPF))
                                                    {
                                                        FCNPJCPF = Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.CPFDest.ToString(), false);
                                                    }

                                                    ConteudoRetorno += FCNPJCPF + ";";
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.dhRegEvento.ToString());
                                                    ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.nProt.ToString());

                                                    switch(NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.tpEventos>(Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.tpEvento.ToString(), false)))
                                                    {
                                                        case ConvertTxt.tpEventos.tpEvEPEC:
                                                            {
                                                                ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.cOrgaoAutor.ToString());
                                                                //ConteudoRetorno += Functions.LerTag(infCCeElemento, "chNFePend");
                                                                foreach(XmlNode conschNFePend in infCCeElemento.GetElementsByTagName("chNFePend"))
                                                                {
                                                                    ConteudoRetorno += conschNFePend.InnerText + ";";
                                                                }
                                                            }
                                                            break;

                                                        case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_1:
                                                        case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_2:
                                                        case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_1:
                                                        case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_2:
                                                        case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_1:
                                                        case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_2:
                                                        case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_1:
                                                        case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_2:
                                                            ConteudoRetorno += Functions.LerTag(infCCeElemento, NFe.Components.TpcnResources.emailDest.ToString());
                                                            break;
                                                    }
                                                    ConteudoRetorno += "\r\n";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Servicos.EnviarCCe
                    }
                    break;
            }
            //Gravar o TXT de retorno para o ERP
            if(!string.IsNullOrEmpty(ConteudoRetorno))
            {
                var TXTRetorno = string.Empty;
                TXTRetorno = Functions.ExtrairNomeArq(NomeXMLDadosMsg, pFinalArqEnvio) + pFinalArqRetorno;
                TXTRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(TXTRetorno, ".xml") + ".txt";

                if(Servico == Servicos.NFePedidoConsultaSituacao && temEvento)
                {
                    File.WriteAllText(TXTRetorno, ConteudoRetorno);
                }
                else
                {
                    File.WriteAllText(TXTRetorno, ConteudoRetorno, Encoding.Default);
                }

                //
                //gravar o conteudo no FTP
                XmlParaFTP(emp, TXTRetorno);
            }
        }

        #endregion GravarRetornoEmTXT()

        #endregion Métodos para gerar o XML´s diversos

        #region Métodos para gerar os XML´s de distribuição

        #region XMLDistInut()

        /// <summary>
        /// Criar o arquivo XML de distribuição das Inutilizações de Números de NFe´s com o protocolo de autorização anexado
        /// </summary>
        /// <param name="nomeArqInut">Nome arquivo XML de Inutilização</param>
        /// <param name="strRetInut">Conteúdo retornado pela SEFAZ com o protocolo da inutilização</param>
        /// <param name="conteudoXML">Conteúdo do XML de inutilização já assinado</param>
        public void XmlDistInut(XmlDocument conteudoXML, string strRetInut, string nomeArqInut)
        {
            var emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                var InutNFeList = conteudoXML.GetElementsByTagName("inutNFe");
                var InutNFeNode = InutNFeList[0];
                var strInutNFe = InutNFeNode.OuterXml;

                var versao = ConvertTxt.versoes.VersaoXMLInut;
                if(((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()] != null)
                {
                    versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()].Value;
                }

                //Montar o XML -procInutNFe.xml
                var strXmlProcInutNfe = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                    "<procInutNFe xmlns=\"" + NFeStrConstants.NAME_SPACE_NFE + "\" versao=\"" + versao + "\">" +
                    strInutNFe +
                    strRetInut +
                    "</procInutNFe>";

                //Montar o nome do arquivo -proc-NFe.xml
                var strNomeArqProcInutNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                    PastaEnviados.EmProcessamento.ToString() + "\\" +
                    Functions.ExtrairNomeArq(nomeArqInut, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML) +
                    Propriedade.ExtRetorno.ProcInutNFe;

                //Gravar o XML em uma linha só (sem quebrar as tag's linha a linha) ou dá erro na hora de validar o XML pelos Schemas. Wandrey 13/05/2009
                swProc = File.CreateText(strNomeArqProcInutNFe);
                swProc.Write(strXmlProcInutNfe);

                XmlParaFTP(emp, strNomeArqProcInutNFe);
            }
            finally
            {
                if(swProc != null)
                {
                    swProc.Close();
                }
            }
        }

        /// <summary>
        /// Criar o arquivo XML de distribuição das Inutilizações de Números de NFe´s com o protocolo de autorização anexado
        /// </summary>
        /// <param name="nomeArqInut">Nome arquivo XML de Inutilização</param>
        /// <param name="strRetInut">Conteúdo retornado pela SEFAZ com o protocolo da inutilização</param>
        /// <param name="conteudoXML">Conteúdo do XML de inutilização já assinado</param>
        /// <param name="versao">Versão do schema do XML</param>
        public void XmlDistInutCTe(XmlDocument conteudoXML, string strRetInut, string nomeArqInut, string versao)
        {
            var emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                var InutNFeList = conteudoXML.GetElementsByTagName("inutCTe");
                var InutNFeNode = InutNFeList[0];
                var strInutNFe = InutNFeNode.OuterXml;

                //Montar o XML -procCancCTe.xml
                var strXmlProcInutNfe = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                    "<procInutCTe xmlns=\"" + NFeStrConstants.NAME_SPACE_CTE + "\" versao=\"" + versao + "\">" +
                    strInutNFe +
                    strRetInut +
                    "</procInutCTe>";

                //Montar o nome do arquivo -proc-NFe.xml
                var strNomeArqProcInutNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                    PastaEnviados.EmProcessamento.ToString() + "\\" +
                    Functions.ExtrairNomeArq(nomeArqInut, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML) +
                    Propriedade.ExtRetorno.ProcInutCTe;

                //Gravar o XML em uma linha só (sem quebrar as tag's linha a linha) ou dá erro na hora de validar o XML pelos Schemas. Wandrey 13/05/2009
                swProc = File.CreateText(strNomeArqProcInutNFe);
                swProc.Write(strXmlProcInutNfe);

                XmlParaFTP(emp, strNomeArqProcInutNFe);
            }
            finally
            {
                if(swProc != null)
                {
                    swProc.Close();
                }
            }
        }

        #endregion XMLDistInut()

        #region XmlPedRec()

        /// <summary>
        /// Gera o XML de pedido de consulta do recibo do lote
        /// </summary>
        /// <param name="mod">Modelo do documento fiscal</param>
        /// <param name="recibo">Número do recibo a ser consultado o lote</param>
        public XmlDocument XmlPedRec(string mod, string recibo, string versao)
        {
            var dadosXML = new XmlDocument();

            switch(mod)
            {
                case "65": //NFC-e
                case "55": //NF-e
                    dadosXML = XmlPedRecNFe(recibo, versao, mod, EmpIndex);
                    break;

                case "57": //CT-e
                    dadosXML = XmlPedRecCTe(recibo, versao, EmpIndex);
                    break;

                case "58": //MDF-e
                    dadosXML = XmlPedRecMDFe(recibo, versao, EmpIndex);
                    break;
            }

            #region Gravar arquivo na pasta

            //TODO: WANDREY - De futuro não quero mais gravar o recibo na pasta para melhorar desempenho, mas por conta do código da empresa agora não é possível, tem muita coisa para ajustar.
            var nomeArqPedRec = Empresas.Configuracoes[EmpIndex].PastaXmlEnvio + "\\" + recibo + Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML;
            var nomeArqPedRecTemp = Empresas.Configuracoes[EmpIndex].PastaXmlEnvio + "\\Temp\\" + recibo + Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML;

            var fiTemp = new FileInfo(nomeArqPedRecTemp);

            if(!File.Exists(nomeArqPedRec) && (!File.Exists(nomeArqPedRecTemp) || fiTemp.CreationTime <= DateTime.Now.AddMinutes(-1)))
            {
                GravarArquivoParaEnvio(nomeArqPedRec, dadosXML.OuterXml);
            }

            #endregion Gravar arquivo na pasta

            return dadosXML;
        }

        #endregion XmlPedRec()

        #region XmlPedRecNFe()

        /// <summary>
        /// Gera o XML de pedido de analise do recibo do lote
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        /// <param name="recibo">Número do recibo a ser consultado o lote</param>
        /// <param name="versao">Versão do schema do XML</param>
        /// <param name="mod">Modelo do documento fiscal</param>
        /// <returns>Retorna a string do XML a ser gravado</returns>
        public XmlDocument XmlPedRecNFe(string recibo, string versao, string mod, int emp)
        {
            var xml = new ConsReciNFe
            {
                NRec = recibo,
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                Versao = versao
            };

            var doc = xml.GerarXML();
            var xmlNode = doc.GetElementsByTagName("consReciNFe")[0];
            xmlNode.AppendChild(CriaElemento(doc, TpcnResources.mod.ToString(), mod, NFeStrConstants.NAME_SPACE_NFE));
            doc.AppendChild(xmlNode);

            return doc;
        }

        #endregion XmlPedRecNFe()

        #region XmlPedRecCTe()

        /// <summary>
        /// Gera o XML de pedido de analise do recibo do lote
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        /// <param name="recibo">Número do recibo a ser consultado o lote</param>
        /// <param name="versao">Versão do schema do XML</param>
        /// <returns>Retorna a string do XML a ser gravado</returns>
        public XmlDocument XmlPedRecCTe(string recibo, string versao, int emp)
        {
            var xml = new ConsReciCTe
            {
                NRec = recibo,
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                Versao = versao
            };

            return xml.GerarXML();
        }

        #endregion XmlPedRecCTe()

        #region XmlPedRecMDFe()

        /// <summary>
        /// Gera o XML de pedido de analise do recibo do lote
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        /// <param name="recibo">Número do recibo a ser consultado o lote</param>
        /// <param name="versao">Versão do schema do XML</param>
        /// <returns>Retorna a string do XML a ser gravado</returns>
        public XmlDocument XmlPedRecMDFe(string recibo, string versao, int emp)
        {
            var xml = new ConsReciMDFe
            {
                NRec = recibo,
                TpAmb = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                Versao = versao
            };

            return xml.GerarXML();
        }

        #endregion XmlPedRecMDFe()

        #region XMLDistNFe()

        /// <summary>
        /// Criar o arquivo XML de distribuição das NFE com o protocolo de autorização anexado
        /// </summary>
        /// <param name="arqNFe">Nome arquivo XML da NFe</param>
        /// <param name="protNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <param name="extensao">Extensão para gerar o arquivo de distribuição da NFe</param>
        /// <param name="versao">Versão do XML da NFe</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public string XmlDistNFe(string arqNFe, string protNfe, string extensao, string versao)  //danasa 11-4-2012
        {
            var nomeArqProcNFe = string.Empty;
            var emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                if(File.Exists(arqNFe))
                {
                    var tipo = "nf";

                    //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                    var doc = new XmlDocument();

                    doc.Load(arqNFe);

                    var NFeList = doc.GetElementsByTagName(tipo.ToUpper() + "e");
                    var NFeNode = NFeList[0];
                    var strNFe = NFeNode.OuterXml;

                    //Montar a string contendo o XML -proc-NFe.xml
                    var strXmlProcNfe = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<" + tipo + "eProc xmlns=\"" + NFeStrConstants.NAME_SPACE_NFE + "\" versao=\"" + versao + "\">" +
                        strNFe +
                        protNfe +
                        "</" + tipo + "eProc>";

                    //Montar o nome do arquivo -proc-NFe.xml
                    nomeArqProcNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                     PastaEnviados.EmProcessamento.ToString() + "\\" +
                                     Functions.ExtrairNomeArq(arqNFe, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML) +
                                     extensao;

                    //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de
                    //validar o XML pelos Schemas. Wandrey 13/05/2009
                    swProc = File.CreateText(nomeArqProcNFe);
                    swProc.Write(strXmlProcNfe);
                }
            }
            finally
            {
                if(swProc != null)
                {
                    swProc.Close();
                }
            }

            return nomeArqProcNFe;
        }

        #endregion XMLDistNFe()

        #region XMLDistCTe()

        /// <summary>
        /// Criar o arquivo XML de distribuição dos CTE com o protocolo de autorização anexado
        /// </summary>
        /// <param name="arqCTe">Nome arquivo XML da CTe</param>
        /// <param name="protCTe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <param name="versao">Versão do XML da NFe</param>
        public string XmlDistCTe(string arqCTe, string protCTe, string versao)  //danasa 11-4-2012
        {
            var nomeArqProcCTe = string.Empty;
            var emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                //Montar o nome do arquivo -proc-CTe.xml
                nomeArqProcCTe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                 PastaEnviados.EmProcessamento.ToString() + "\\" +
                                 Functions.ExtrairNomeArq(arqCTe, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML) +
                                 Propriedade.ExtRetorno.ProcCTe;

                if(File.Exists(arqCTe))
                {
                    var tipo = "ct";

                    //Separar as tag´s da CTe que interessa <CTe> até </CTe>
                    var doc = new XmlDocument();

                    doc.Load(arqCTe);

                    var CTeList = doc.GetElementsByTagName(tipo.ToUpper() + "e");
                    var CTeNode = CTeList[0];
                    var conteudoCTe = CTeNode.OuterXml;

                    //Montar a string contendo o XML -proc-CTe.xml
                    var xmlProcCTe = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<" + tipo + "eProc xmlns=\"" + NFeStrConstants.NAME_SPACE_CTE + "\" versao=\"" + versao + "\">" +
                        conteudoCTe +
                        protCTe +
                        "</" + tipo + "eProc>";

                    //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de
                    //validar o XML pelos Schemas. Wandrey 13/05/2009
                    swProc = File.CreateText(nomeArqProcCTe);
                    swProc.Write(xmlProcCTe);
                }
            }
            finally
            {
                if(swProc != null)
                {
                    swProc.Close();
                }
            }

            return nomeArqProcCTe;
        }

        /// <summary>
        /// Criar o arquivo XML de distribuição dos CTE com o protocolo de autorização anexado
        /// </summary>
        /// <param name="arqCTe">Nome arquivo XML da CTe</param>
        /// <param name="protCTe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <param name="versao">Versão do XML da NFe</param>
        public string XmlDistCTeOS(string arqCTe, string protCTe, string versao)  //danasa 11-4-2012
        {
            var nomeArqProcCTe = string.Empty;
            var emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                //Montar o nome do arquivo -proc-CTe.xml
                nomeArqProcCTe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                 PastaEnviados.EmProcessamento.ToString() + "\\" +
                                 Functions.ExtrairNomeArq(arqCTe, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML) +
                                 Propriedade.ExtRetorno.ProcCTe;

                if(File.Exists(arqCTe))
                {
                    var tipo = "ct";

                    //Separar as tag´s da CTe que interessa <CTe> até </CTe>
                    var doc = new XmlDocument();

                    doc.Load(arqCTe);

                    var CTeList = doc.GetElementsByTagName(tipo.ToUpper() + "eOS");
                    var CTeNode = CTeList[0];
                    var conteudoCTe = CTeNode.OuterXml;

                    //Montar a string contendo o XML -proc-CTe.xml
                    var xmlProcCTe = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<" + tipo + "eOSProc xmlns=\"" + NFeStrConstants.NAME_SPACE_CTE + "\" versao=\"" + versao + "\">" +
                        conteudoCTe +
                        protCTe +
                        "</" + tipo + "eOSProc>";

                    //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de
                    //validar o XML pelos Schemas. Wandrey 13/05/2009
                    swProc = File.CreateText(nomeArqProcCTe);
                    swProc.Write(xmlProcCTe);
                }
            }
            finally
            {
                if(swProc != null)
                {
                    swProc.Close();
                }
            }

            return nomeArqProcCTe;
        }

        #endregion XMLDistCTe()

        #region XMLDistMDFe()

        /// <summary>
        /// Criar o arquivo XML de distribuição dos MDFe com o protocolo de autorização anexado
        /// </summary>
        /// <param name="arqMDFe">Nome arquivo XML da MDFe</param>
        /// <param name="protMDFe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <param name="versao">Versão do schema do XML</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public string XmlDistMDFe(string arqMDFe, string protMDFe, string extensao, string versao)  //danasa 11-4-2012
        {
            var nomeArqProcMDFe = string.Empty;
            var emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                if(File.Exists(arqMDFe))
                {
                    var tipo = "mdf";

                    //Separar as tag´s da MDFe que interessa <MDFe> até </MDFe>
                    var doc = new XmlDocument();

                    doc.Load(arqMDFe);

                    var MDFeList = doc.GetElementsByTagName(tipo.ToUpper() + "e");
                    var MDFeNode = MDFeList[0];
                    var conteudoMDFe = MDFeNode.OuterXml;

                    //Montar a string contendo o XML -proc-MDFe.xml
                    var xmlProcMDFe = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<" + tipo + "eProc xmlns=\"" + NFeStrConstants.NAME_SPACE_MDFE + "\" versao=\"" + versao + "\">" +
                        conteudoMDFe +
                        protMDFe +
                        "</" + tipo + "eProc>";

                    //Montar o nome do arquivo -proc-MDFe.xml
                    nomeArqProcMDFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                     PastaEnviados.EmProcessamento.ToString() + "\\" +
                                     Functions.ExtrairNomeArq(arqMDFe, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML) +
                                     extensao;

                    //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de
                    //validar o XML pelos Schemas. Wandrey 13/05/2009
                    swProc = File.CreateText(nomeArqProcMDFe);
                    swProc.Write(xmlProcMDFe);
                }
            }
            finally
            {
                if(swProc != null)
                {
                    swProc.Close();
                }
            }
            return nomeArqProcMDFe;   //danasa 11-4-2012
        }

        #endregion XMLDistMDFe()

        #region XMLDistLMC()

        /// <summary>
        /// Criar o arquivo XML de distribuição dos LMC com o protocolo de autorização anexado
        /// </summary>
        /// <param name="arqLMC">Nome arquivo XML da LMC</param>
        /// <param name="protLMC">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <param name="extensao">String contendo a extensão do arquivo</param>
        public string XmlDistLMC(string arqLMC, string protLMC, string extensao)
        {
            var nomeArqProcLMC = string.Empty;
            var emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                if(File.Exists(arqLMC))
                {
                    var tipo = "livroCombustivel";

                    var doc = new XmlDocument();
                    doc.Load(arqLMC);

                    var LMCList = doc.GetElementsByTagName(tipo);
                    var LMCNode = LMCList[0];
                    var conteudoLMC = LMCNode.OuterXml;

                    //Montar a string contendo o XML -procLMC.xml
                    var xmlProcLMC = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<" + tipo + "Proc xmlns=\"" + NFeStrConstants.NAME_SPACE_LMC + "\" versao=\"1.00\">" +
                        conteudoLMC +
                        protLMC +
                        "</" + tipo + "Proc>";

                    //Montar o nome do arquivo -procLMC.xml
                    nomeArqProcLMC = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                     PastaEnviados.EmProcessamento.ToString() + "\\" +
                                     Functions.ExtrairNomeArq(arqLMC, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML) +
                                     extensao;

                    swProc = File.CreateText(nomeArqProcLMC);
                    swProc.Write(xmlProcLMC);
                }
            }
            finally
            {
                if(swProc != null)
                {
                    swProc.Close();
                }
            }
            return nomeArqProcLMC;
        }

        #endregion XMLDistLMC()        

        #region EnvioConsultaNFeDest

        public void EnvioConsultaNFeDest(string pArquivo, DadosConsultaNFeDest dadosConsulta)
        {
            var doc = new XmlDocument();
            doc.InsertBefore(doc.CreateXmlDeclaration("1.0", "UTF-8", ""), doc.DocumentElement);
            XmlNode envConsulta = doc.CreateElement("consNFeDest");
            envConsulta.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.versao.ToString(), NFe.ConvertTxt.versoes.VersaoXMLEnvConsultaNFeDest));
            envConsulta.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.xmlns.ToString(), NFeStrConstants.NAME_SPACE_NFE));
            envConsulta.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.tpAmb.ToString(), dadosConsulta.tpAmb.ToString()));
            envConsulta.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.xServ.ToString(), string.IsNullOrEmpty(dadosConsulta.xServ) ? "CONSULTAR NFE DEST" : dadosConsulta.xServ));
            envConsulta.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.CNPJ.ToString(), dadosConsulta.CNPJ));
            envConsulta.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.indNFe.ToString(), dadosConsulta.indNFe.ToString()));
            envConsulta.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.indEmi.ToString(), dadosConsulta.indEmi.ToString()));
            envConsulta.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.ultNSU.ToString(), dadosConsulta.ultNSU.ToString()));
            doc.AppendChild(envConsulta);

            GravarArquivoParaEnvio(pArquivo, doc.OuterXml, true);
        }

        #endregion EnvioConsultaNFeDest

        #region -- Evento

        #region EnvioEvento

        /// <summary>
        /// EnvioEvento
        /// </summary>
        /// <param name="pArquivo"></param>
        /// <param name="dadosEnvEvento"></param>
        public void EnvioEvento(string pArquivo, DadosenvEvento dadosEnvEvento)
        {
            if(dadosEnvEvento.eventos.Count == 0)
            {
                throw new Exception("Sem eventos no XML de envio");
            }

            var currentEvento = dadosEnvEvento.eventos[0].tpEvento;
            foreach(var item in dadosEnvEvento.eventos)
            {
                if(!currentEvento.Equals(item.tpEvento))
                {
                    throw new Exception(string.Format("Não é possivel mesclar tipos de eventos dentro de um mesmo xml de eventos. O tipo de evento neste xml é {0}", currentEvento));
                }
            }

            var doc = new XmlDocument();
            doc.InsertBefore(doc.CreateXmlDeclaration("1.0", "UTF-8", ""), doc.DocumentElement);
            XmlNode envEvento = doc.CreateElement("envEvento");
            envEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.versao.ToString(), NFe.ConvertTxt.versoes.VersaoXMLEvento));
            envEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.xmlns.ToString(), NFeStrConstants.NAME_SPACE_NFE));
            envEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.idLote.ToString(), dadosEnvEvento.idLote));
            foreach(var evento in dadosEnvEvento.eventos)
            {
                XmlNode nodeEvento = doc.CreateElement("evento");
                nodeEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.versao.ToString(), dadosEnvEvento.versao));
                nodeEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.xmlns.ToString(), NFeStrConstants.NAME_SPACE_NFE));

                XmlNode infEvento = doc.CreateElement("infEvento");
                infEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.Id.ToString(), evento.Id));
                infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.cOrgao.ToString(), evento.cOrgao.ToString()));
                infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.tpAmb.ToString(), evento.tpAmb.ToString()));
                if(evento.tpEmis != 0)
                {
                    infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.tpEmis.ToString(), evento.tpEmis.ToString()));
                }

                if(!string.IsNullOrEmpty(evento.CNPJ))
                {
                    infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.CNPJ.ToString(), evento.CNPJ));
                }
                else
                {
                    infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.CPF.ToString(), evento.CPF));
                }

                infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.chNFe.ToString(), evento.chNFe));
                // get the UTC offset depending on day light savings
                /*Data e hora do evento no formato AAAA-MM-DDThh:mm:ssTZD (UTC - Universal Coordinated Time,
                onde TZD pode ser -02:00 (Fernando de Noronha), -03:00(Brasília) ou -04:00 (Manaus), no horário de verão serão -
                01:00, -02:00 e -03:00. Ex.: 2010-08-19T13:00:15-03:00.*/
                if(!string.IsNullOrEmpty(evento.dhEvento))
                {
                    if(!(evento.dhEvento.EndsWith("-01:00") ||
                            evento.dhEvento.EndsWith("-02:00") ||
                            evento.dhEvento.EndsWith("-03:00") ||
                            evento.dhEvento.EndsWith("-04:00")))
                    {
                        evento.dhEvento = Convert.ToDateTime(evento.dhEvento).ToString("yyyy-MM-dd\"T\"HH:mm:sszzz");
                    }
                }
                infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.dhEvento.ToString(), evento.dhEvento));
                infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.tpEvento.ToString(), evento.tpEvento));
                infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.nSeqEvento.ToString(), evento.nSeqEvento.ToString()));
                infEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.verEvento.ToString(), evento.verEvento));

                if(string.IsNullOrEmpty(evento.descEvento))
                {
                    evento.descEvento = EnumHelper.GetDescription((NFe.ConvertTxt.tpEventos)Enum.Parse(typeof(NFe.ConvertTxt.tpEventos), evento.tpEvento));
                }

                XmlNode detEvento = doc.CreateElement("detEvento");
                detEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.versao.ToString(), "1.00"));
                detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.descEvento.ToString(), evento.descEvento.Trim()));

                switch(NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.tpEventos>(evento.tpEvento))
                {
                    case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_2:
                        {
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.nProt.ToString(), evento.nProt));
                            foreach(var ppICMS in evento.prorrogacaoICMS)
                            {
                                XmlNode itemPedido = doc.CreateElement(NFe.Components.TpcnResources.itemPedido.ToString());
                                itemPedido.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.numItem.ToString(), ppICMS.numItem));
                                itemPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.qtdeItem.ToString(), ppICMS.qtdeItem.Trim()));
                                detEvento.AppendChild(itemPedido);
                            }
                        }
                        break;

                    case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_2:
                        {
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.idPedidoCancelado.ToString(), evento.idPedidoCancelado));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.nProt.ToString(), evento.nProt));
                        }
                        break;

                    case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_2:
                    case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_2:
                        {
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.idPedido.ToString(), evento.idPedido));
                            switch(NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.tpEventos>(evento.tpEvento))
                            {
                                case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_1:
                                case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_2:
                                    {
                                        XmlNode respCancPedido = doc.CreateElement(NFe.Components.TpcnResources.respCancPedido.ToString());
                                        respCancPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.statCancPedido.ToString(), evento.respCancPedido.statCancPedido.ToString()));
                                        respCancPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.justStatus.ToString(), evento.respCancPedido.justStatus.ToString("00")));
                                        if(!string.IsNullOrEmpty(evento.respCancPedido.justStaOutra))
                                        {
                                            respCancPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.justStaOutra.ToString(), evento.respCancPedido.justStaOutra));
                                        }

                                        detEvento.AppendChild(respCancPedido);
                                    }
                                    break;

                                default:
                                    {
                                        XmlNode respPedido = doc.CreateElement(NFe.Components.TpcnResources.respPedido.ToString());
                                        respPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.statPrazo.ToString(), evento.respPedido.statPrazo));

                                        foreach(var ipt in evento.respPedido.itemPedido)
                                        {
                                            XmlNode itemPedido = doc.CreateElement(NFe.Components.TpcnResources.itemPedido.ToString());
                                            itemPedido.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.numItem.ToString(), ipt.numItem.ToString()));
                                            itemPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.statPedido.ToString(), ipt.statPedido.ToString()));
                                            itemPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.justStatus.ToString(), ipt.justStatus.ToString()));
                                            if(!string.IsNullOrEmpty(ipt.justStaOutra))
                                            {
                                                itemPedido.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.justStaOutra.ToString(), ipt.justStaOutra));
                                            }

                                            respPedido.AppendChild(itemPedido);
                                        }
                                        detEvento.AppendChild(respPedido);
                                    }
                                    break;
                            }
                        }
                        break;

                    case ConvertTxt.tpEventos.tpEvCCe:
                        {
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.xCorrecao.ToString(), evento.xCorrecao.Trim()));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.xCondUso.ToString(), evento.xCondUso.Trim()));
                        }
                        break;

                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                        {
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.nProt.ToString(), evento.nProt));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.xJust.ToString(), evento.xJust.Trim()));
                        }
                        break;

                    case ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe:
                        {
                            detEvento.AppendChild(CriaElemento(doc, TpcnResources.cOrgaoAutor.ToString(), evento.cancelamentoSubstituicao.cOrgaoAutor.ToString()));
                            detEvento.AppendChild(CriaElemento(doc, TpcnResources.tpAutor.ToString(), ((int)evento.cancelamentoSubstituicao.tpAutor).ToString()));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.verAplic.ToString(), evento.cancelamentoSubstituicao.verAplic.Trim()));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.nProt.ToString(), evento.nProt));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.xJust.ToString(), evento.xJust.Trim()));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.chNFeRef.ToString(), evento.cancelamentoSubstituicao.chNFeRef));
                        }
                        break;

                    case ConvertTxt.tpEventos.tpEvOperacaoNaoRealizada:
                        detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.xJust.ToString(), evento.xJust.Trim()));
                        break;

                    case ConvertTxt.tpEventos.tpEvEPEC:
                        {
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.cOrgaoAutor.ToString(), evento.epec.cOrgaoAutor.ToString()));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.tpAutor.ToString(), ((int)evento.epec.tpAutor).ToString()));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.verAplic.ToString(), evento.epec.verAplic.Trim()));
                            if(!string.IsNullOrEmpty(evento.epec.dhEmi))
                            {
                                if(!(evento.epec.dhEmi.EndsWith("-01:00") ||
                                        evento.epec.dhEmi.EndsWith("-02:00") ||
                                        evento.epec.dhEmi.EndsWith("-03:00") ||
                                        evento.epec.dhEmi.EndsWith("-04:00")))
                                {
                                    evento.epec.dhEmi = Convert.ToDateTime(evento.epec.dhEmi).ToString("yyyy-MM-dd\"T\"HH:mm:sszzz");
                                }
                            }
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.dhEmi.ToString(), evento.epec.dhEmi));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.tpNF.ToString(), ((int)evento.epec.tpNF).ToString()));
                            detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.IE.ToString(), evento.epec.IE));

                            XmlNode destEvento = doc.CreateElement("dest");
                            destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.UF.ToString(), evento.epec.dest.UF));
                            if(!string.IsNullOrEmpty(evento.epec.dest.idEstrangeiro) || evento.epec.dest.UF.Equals("EX"))
                            {
                                destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.idEstrangeiro.ToString(), evento.epec.dest.idEstrangeiro));
                            }
                            else
                                if(!string.IsNullOrEmpty(evento.epec.dest.CNPJ))
                            {
                                destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.CNPJ.ToString(), evento.epec.dest.CNPJ));
                            }
                            else
                            {
                                destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.CPF.ToString(), evento.epec.dest.CPF));
                            }

                            if(!string.IsNullOrEmpty(evento.epec.dest.IE) && !evento.epec.dest.IE.Equals("ISENTO"))
                            {
                                destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.IE.ToString(), evento.epec.dest.IE));
                            }

                            switch(evento.chNFe.Substring(20, 2))
                            {
                                case "55":
                                    destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.vNF.ToString(), evento.epec.dest.vNF.ToString("0.00").Replace(",", ".")));
                                    destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.vICMS.ToString(), evento.epec.dest.vICMS.ToString("0.00").Replace(",", ".")));
                                    destEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.vST.ToString(), evento.epec.dest.vST.ToString("0.00").Replace(",", ".")));
                                    detEvento.AppendChild(destEvento);
                                    break;

                                case "65":
                                    detEvento.AppendChild(destEvento);

                                    detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.vNF.ToString(), evento.epec.dest.vNF.ToString("0.00").Replace(",", ".")));
                                    detEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.vICMS.ToString(), evento.epec.dest.vICMS.ToString("0.00").Replace(",", ".")));
                                    break;
                            }
                        }
                        break;
                }
                infEvento.AppendChild(detEvento);
                nodeEvento.AppendChild(infEvento);
                envEvento.AppendChild(nodeEvento);
            }
            doc.AppendChild(envEvento);

            GravarArquivoParaEnvio(pArquivo, doc.OuterXml, true);
        }

        #endregion EnvioEvento

        #region GerarRecepcaoDFe

        public void RecepcaoDFe(string arquivo, distDFeInt value)
        {
            var doc = new XmlDocument();
            doc.InsertBefore(doc.CreateXmlDeclaration("1.0", "UTF-8", ""), doc.DocumentElement);
            XmlNode envEvento = doc.CreateElement("distDFeInt");
            envEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.versao.ToString(), string.IsNullOrEmpty(value.versao) ? NFe.ConvertTxt.versoes.VersaoXMLEnvDFe : value.versao));
            envEvento.Attributes.Append(CriaAttribute(doc, NFe.Components.TpcnResources.xmlns.ToString(), NFeStrConstants.NAME_SPACE_NFE));
            envEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.tpAmb.ToString(), value.tpAmb.ToString()));
            envEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.cUFAutor.ToString(), value.cUFAutor.ToString()));
            if(!string.IsNullOrEmpty(value.CNPJ))
            {
                envEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.CNPJ.ToString(), value.CNPJ.ToString()));
            }
            else
            {
                envEvento.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.CPF.ToString(), value.CPF.ToString()));
            }

            if(!string.IsNullOrEmpty(value.ultNSU))
            {
                XmlNode nodeEvento1 = doc.CreateElement("distNSU");
                nodeEvento1.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.ultNSU.ToString(), value.ultNSU.PadLeft(15, '0')));
                envEvento.AppendChild(nodeEvento1);
            }
            else
            {
                XmlNode nodeEvento2 = doc.CreateElement("consNSU");
                nodeEvento2.AppendChild(CriaElemento(doc, NFe.Components.TpcnResources.NSU.ToString(), value.NSU.PadLeft(15, '0')));
                envEvento.AppendChild(nodeEvento2);
            }
            doc.AppendChild(envEvento);

            GravarArquivoParaEnvio(arquivo, doc.OuterXml, true);
        }

        #endregion GerarRecepcaoDFe

        #region XmlDistEvento

        #region XmlDistEvento()

        /// <summary>
        /// XMLDistEvento
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="vStrXmlRetorno"></param>
        public void XmlDistEvento(int emp, string vStrXmlRetorno)
        {
            var docEventos = new XmlDocument();
            docEventos.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            var retprocEventoNFeList = docEventos.GetElementsByTagName("procEventoNFe");
            if(retprocEventoNFeList != null)
            {
                foreach(XmlNode retConsSitNode1 in retprocEventoNFeList)
                {
                    var cStat = ((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0].InnerText;
                    if(cStat == "135" || cStat == "136" || cStat == "155" || cStat == "124")
                    {
                        var tpEvento = NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.tpEventos>(((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.tpEvento.ToString())[0].InnerText);
                        if(tpEvento != ConvertTxt.tpEventos.tpEvEPEC)
                        {
                            var chNFe = ((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.chNFe.ToString())[0].InnerText;
                            var nSeqEvento = Convert.ToInt32("0" + ((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.nSeqEvento.ToString())[0].InnerText);
                            var dhRegEvento = Functions.GetDateTime(((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.dhRegEvento.ToString())[0].InnerText);

                            XmlDistEvento(emp,
                                chNFe,
                                nSeqEvento,
                                tpEvento,
                                retConsSitNode1.OuterXml,
                                string.Empty,
                                dhRegEvento, false);
                        }
                    }
                }
            }
        }

        #endregion XmlDistEvento()

        #region XmlDistEvento()

        /// <summary>
        /// XMLDistEvento
        /// Criar o arquivo XML de distribuição dos Eventos NFe
        /// </summary>
        public void XmlDistEvento(int emp,
            string ChaveNFe,
            int nSeqEvento,
            NFe.ConvertTxt.tpEventos tpEvento,
            string xmlEventoEnvio,
            string xmlRetornoEnvio,
            DateTime dhRegEvento,
            bool FromTaskEventos)
        {
            /// grava o xml de distribuicao como: chave + "_" +  nSeqEvento
            /// ja que a nSeqEvento deve ser unico para cada chave
            ///
            /// quando o evento for de manifestacao ou cancelamento o nome do arquivo contera o tipo do evento
            ///
            var tempXmlFile =
                    PastaEnviados.Autorizados.ToString() + "\\" +
                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(dhRegEvento) +
                    ChaveNFe + "_" + (tpEvento != ConvertTxt.tpEventos.tpEvCCe ? ((int)tpEvento).ToString() + "_" : "") + nSeqEvento.ToString("00") +
                    Propriedade.ExtRetorno.ProcEventoNFe;

            var sendtodanfemon = true;

            var filenameToWrite = Path.Combine(Empresas.Configuracoes[emp].PastaXmlEnviado, tempXmlFile);
            var filenameBackup = Empresas.Configuracoes[emp].PastaBackup;
            var NFeDeTerceiros = ChaveNFe.Substring(6, 14) != Empresas.Configuracoes[emp].CNPJ ||
                                  ChaveNFe.Substring(0, 2) != Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString();

            ///
            /// eventos de 'Ciencia da operacao', por exemplo, tem CNPJ na chave diferente do CNPJ do cliente Uninfe
            /// entao, neste caso os eventos tem que ser gravados na pasta de autorizados e nao na de terceiros
            ///
            /// Mas e se existir uma nota contra a mesma empresa?
            ///
            /// se for emitido um evento de 'Ciencia da operacao', o xml será gravado na pasta de 'Autoriados', mas se fizer uma consulta
            /// a situacao da mesma nota, os eventos serão gravados na pasta de 'Terceiros'
            ///
            if((tpEvento != ConvertTxt.tpEventos.tpEvCancelamentoNFe &&
                tpEvento != ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe &&
                tpEvento != ConvertTxt.tpEventos.tpEvCCe &&
                tpEvento != ConvertTxt.tpEventos.tpEvEPEC) && !FromTaskEventos && NFeDeTerceiros)
            {
                if(!Empresas.Configuracoes[emp].GravarEventosDeTerceiros ||
                    string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaDownloadNFeDest))
                {
                    return;
                }

                filenameToWrite = Path.Combine(Empresas.Configuracoes[emp].PastaDownloadNFeDest, Path.GetFileName(tempXmlFile));
                ///
                /// xml de terceiros nao grava na pasta de backup
                ///
                filenameBackup = "";
                sendtodanfemon = false;
            }
            else
            {
                if(tpEvento != ConvertTxt.tpEventos.tpEvEPEC && !NFeDeTerceiros)
                {
                    var vePasta = false;
                    if(tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoNFe ||
                        tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe)
                    {
                        vePasta = Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe;
                    }
                    else
                    {
                        vePasta = Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe;
                    }

                    if(vePasta)
                    {
                        var folderNFe = OndeNFeEstaGravada(emp, ChaveNFe, Propriedade.ExtRetorno.ProcNFe);
                        if(!string.IsNullOrEmpty(folderNFe))
                        {
                            filenameToWrite = Path.Combine(folderNFe, Path.GetFileName(filenameToWrite));
                        }
                    }
                }
            }
            string protEnvioEvento;
            if(xmlEventoEnvio.IndexOf("<procEventoNFe") >= 0)
            {
                protEnvioEvento = xmlEventoEnvio;
            }
            else //danasa 4/7/2011
            {
                protEnvioEvento = "<procEventoNFe versao=\"" + NFe.ConvertTxt.versoes.VersaoXMLEvento + "\" xmlns=\"" + NFeStrConstants.NAME_SPACE_NFE + "\">" +
                                  xmlEventoEnvio +
                                  xmlRetornoEnvio +
                                  "</procEventoNFe>";
            }

            //Gravar o arquivo de distribuição na pasta de enviados autorizados
            if(!protEnvioEvento.StartsWith("<?xml"))
            {
                protEnvioEvento = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + protEnvioEvento;
            }

            //Gravar o arquivo de distribuição na pasta de backup
            if(!string.IsNullOrEmpty(filenameBackup))
            {
                var vePasta = false;
                if(tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoNFe ||
                    tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe)
                {
                    vePasta = Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe;
                }
                else
                {
                    vePasta = Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe;
                }

                var subPastaBackup = "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(dhRegEvento) + Path.GetFileName(filenameToWrite);

                if(vePasta)
                {
                    var folderNFe = OndeNFeEstaGravada(emp, ChaveNFe, Propriedade.ExtRetorno.ProcNFe);
                    if(!string.IsNullOrEmpty(folderNFe))
                    {
                        filenameBackup += "\\" + PastaEnviados.Autorizados.ToString() + "\\" + new DirectoryInfo(folderNFe).Name + "\\" + Path.GetFileName(filenameToWrite);
                    }
                    else
                    {
                        filenameBackup += subPastaBackup;
                    }
                }
                else
                {
                    filenameBackup += subPastaBackup;
                }

                if(!Directory.Exists(Path.GetDirectoryName(filenameBackup)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filenameBackup));
                }

                if(!File.Exists(filenameBackup))
                {
                    File.WriteAllText(filenameBackup, protEnvioEvento);
                }
            }
            // cria a pasta se não existir
            if(!Directory.Exists(Path.GetDirectoryName(filenameToWrite)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filenameToWrite));
            }

            if(!File.Exists(filenameToWrite))
            {
                File.WriteAllText(filenameToWrite, protEnvioEvento);
            }

            XmlParaFTP(emp, filenameToWrite);

            if(sendtodanfemon)
            {
                TFunctions.CopiarXMLPastaDanfeMon(filenameToWrite);
            }

            NomeArqGerado = filenameToWrite;
        }

        #endregion XmlDistEvento()

        #region XmlDistEventoCTe()

        /// <summary>
        /// XMLDistEvento CTe
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="vStrXmlRetorno"></param>
        public void XmlDistEventoCTe(int emp, string vStrXmlRetorno)
        {
            //
            //<<<danasa 6-2011
            //<<<UTF8 -> tem acentuacao no retorno
            var docEventos = new XmlDocument();
            docEventos.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));
            var retprocEventoNFeList = docEventos.GetElementsByTagName("procEventoCTe");
            if(retprocEventoNFeList != null)
            {
                foreach(XmlNode retConsSitNode1 in retprocEventoNFeList)
                {
                    var cStat = ((XmlElement)retConsSitNode1).GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                    if(cStat == "134" || cStat == "135" || cStat == "136")
                    {
                        var chNFe = ((XmlElement)retConsSitNode1).GetElementsByTagName(TpcnResources.chCTe.ToString())[0].InnerText;
                        var nSeqEvento = Convert.ToInt32("0" + ((XmlElement)retConsSitNode1).GetElementsByTagName(TpcnResources.nSeqEvento.ToString())[0].InnerText);
                        var tpEvento = Convert.ToInt32("0" + ((XmlElement)retConsSitNode1).GetElementsByTagName(TpcnResources.tpEvento.ToString())[0].InnerText);
                        var dhRegEvento = Functions.GetDateTime(((XmlElement)retConsSitNode1).GetElementsByTagName(TpcnResources.dhRegEvento.ToString())[0].InnerText);
                        var versao = ((XmlElement)retConsSitNode1).Attributes[TpcnResources.versao.ToString()].InnerText;

                        XmlDistEventoCTe(emp, chNFe, nSeqEvento, tpEvento, retConsSitNode1.OuterXml, string.Empty, dhRegEvento, false, versao);
                    }
                }
            }
        }

        #endregion XmlDistEventoCTe()

        #region XmlDistEventoCTe()

        /// <summary>
        /// XMLDistEvento
        /// Criar o arquivo XML de distribuição dos Eventos CTe
        /// </summary>
        public void XmlDistEventoCTe(int emp, string ChaveNFe, int nSeqEvento, int tpEvento, string xmlEventoEnvio, string xmlRetornoEnvio,
            DateTime dhRegEvento, bool FromTaskEventos, string versao)
        {
            var tempXmlFile =
                    PastaEnviados.Autorizados.ToString() + "\\" +
                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(dhRegEvento) +
                    ChaveNFe + "_" + tpEvento.ToString() + "_" + nSeqEvento.ToString("00") + Propriedade.ExtRetorno.ProcEventoCTe;
            var NFeDeTerceiros = ChaveNFe.Substring(6, 14) != Empresas.Configuracoes[emp].CNPJ ||
                      ChaveNFe.Substring(0, 2) != Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString();

            var sendtodanfemon = true;

            var filenameToWrite = Path.Combine(Empresas.Configuracoes[emp].PastaXmlEnviado, tempXmlFile);
            var filenameBackup = Empresas.Configuracoes[emp].PastaBackup;

            if(!FromTaskEventos && NFeDeTerceiros)
            {
                if(!Empresas.Configuracoes[emp].GravarEventosDeTerceiros ||
                    string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaDownloadNFeDest))
                {
                    return;
                }

                filenameToWrite = Path.Combine(Empresas.Configuracoes[emp].PastaDownloadNFeDest, Path.GetFileName(tempXmlFile));
                ///
                /// xml de terceiros nao grava na pasta de backup
                ///
                filenameBackup = "";
                sendtodanfemon = false;
            }
            else
            {
                var vePasta = false;
                if((ConvertTxt.tpEventos)tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoNFe ||
                    (ConvertTxt.tpEventos)tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe)
                {
                    vePasta = Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe;
                }
                else
                {
                    vePasta = Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe;
                }

                if(vePasta)
                {
                    var folderNFe = OndeNFeEstaGravada(emp, ChaveNFe, Propriedade.ExtRetorno.ProcCTe);
                    if(!string.IsNullOrEmpty(folderNFe))
                    {
                        filenameToWrite = Path.Combine(folderNFe, Path.GetFileName(filenameToWrite));
                    }
                }
            }
            string protEnvioEvento;
            if(xmlEventoEnvio.IndexOf("<procEventoCTe") >= 0)
            {
                protEnvioEvento = xmlEventoEnvio;
            }
            else
            {
                protEnvioEvento = "<procEventoCTe versao=\"" + versao + "\" xmlns=\"" + NFeStrConstants.NAME_SPACE_CTE + "\">" +
                                  xmlEventoEnvio +
                                  xmlRetornoEnvio +
                                  "</procEventoCTe>";
            }

            //Gravar o arquivo de distribuição na pasta de enviados autorizados
            if(!protEnvioEvento.StartsWith("<?xml"))
            {
                protEnvioEvento = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + protEnvioEvento;
            }

            // Criar a pasta de backup, caso não exista. Wandrey 25/05/211
            if(!string.IsNullOrEmpty(filenameBackup))
            {
                filenameBackup = Path.Combine(filenameBackup, tempXmlFile);
                if(!Directory.Exists(Path.GetDirectoryName(filenameBackup)))
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filenameBackup));
                }

                //Gravar o arquivo de distribuição na pasta de backup

                if(!File.Exists(filenameBackup))
                {
                    File.WriteAllText(filenameBackup, protEnvioEvento);
                }
            }
            // cria a pasta se não existir
            if(!Directory.Exists(Path.GetDirectoryName(filenameToWrite)))
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filenameToWrite));
            }

            if(!File.Exists(filenameToWrite))
            {
                File.WriteAllText(filenameToWrite, protEnvioEvento);
            }

            XmlParaFTP(emp, filenameToWrite);

            if(sendtodanfemon)
            {
                TFunctions.CopiarXMLPastaDanfeMon(filenameToWrite);
            }

            NomeArqGerado = filenameToWrite;
        }

        #endregion XmlDistEventoCTe()

        #region XmlDistEventoMDFe()

        /// <summary>
        /// XMLDistEvento CTe
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="strXmlRetorno"></param>
        public void XmlDistEventoMDFe(int emp, string strXmlRetorno)
        {
            // <<< UTF8 -> tem acentuação no retorno
            var docEventos = new XmlDocument();
            docEventos.Load(Functions.StringXmlToStreamUTF8(strXmlRetorno));
            var retprocEventoNFeList = docEventos.GetElementsByTagName("procEventoMDFe");
            if(retprocEventoNFeList != null)
            {
                foreach(XmlNode retConsSitNode1 in retprocEventoNFeList)
                {
                    var cStat = ((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0].InnerText;
                    if(cStat == "134" || cStat == "135" || cStat == "136")
                    {
                        var chNFe = ((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.chMDFe.ToString())[0].InnerText;
                        var nSeqEvento = Convert.ToInt32("0" + ((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.nSeqEvento.ToString())[0].InnerText);
                        var tpEvento = Convert.ToInt32("0" + ((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.tpEvento.ToString())[0].InnerText);
                        var dhRegEvento = Functions.GetDateTime/*Convert.ToDateTime*/(((XmlElement)retConsSitNode1).GetElementsByTagName(NFe.Components.TpcnResources.dhRegEvento.ToString())[0].InnerText);
                        var versao = ((XmlElement)retConsSitNode1).Attributes[TpcnResources.versao.ToString()].InnerText;

                        XmlDistEventoMDFe(emp, chNFe, nSeqEvento, tpEvento, retConsSitNode1.OuterXml, string.Empty, dhRegEvento, false, versao);
                    }
                }
            }
        }

        #endregion XmlDistEventoMDFe()

        #region XmlDistEventoMDFe()

        /// <summary>
        /// XMLDistEvento
        /// Criar o arquivo XML de distribuição dos Eventos MDFe
        /// </summary>
        public void XmlDistEventoMDFe(int emp, string ChaveNFe, int nSeqEvento, int tpEvento, string xmlEventoEnvio, string xmlRetornoEnvio, DateTime dhRegEvento, bool FromTaskEventos, string versao)
        {
            // grava o xml de distribuicao como: chave + "_" +  nSeqEvento
            // ja que a nSeqEvento deve ser unico para cada chave
            //
            // quando o evento for de manifestacao ou cancelamento o nome do arquivo contera o tipo do evento
            var tempXmlFile =
                    PastaEnviados.Autorizados.ToString() + "\\" +
                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(dhRegEvento) +
                    ChaveNFe + "_" + tpEvento.ToString() + "_" + nSeqEvento.ToString("00") + Propriedade.ExtRetorno.ProcEventoMDFe;
            var NFeDeTerceiros = ChaveNFe.Substring(6, 14) != Empresas.Configuracoes[emp].CNPJ ||
                      ChaveNFe.Substring(0, 2) != Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString();

            var sendtodanfemon = true;

            var filenameToWrite = Path.Combine(Empresas.Configuracoes[emp].PastaXmlEnviado, tempXmlFile);
            var filenameBackup = Empresas.Configuracoes[emp].PastaBackup;

            if(!FromTaskEventos && NFeDeTerceiros)
            {
                if(!Empresas.Configuracoes[emp].GravarEventosDeTerceiros ||
                    string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaDownloadNFeDest))
                {
                    return;
                }

                filenameToWrite = Path.Combine(Empresas.Configuracoes[emp].PastaDownloadNFeDest, Path.GetFileName(tempXmlFile));
                ///
                /// xml de terceiros nao grava na pasta de backup
                ///
                filenameBackup = "";
                sendtodanfemon = false;
            }
            else
            {
                var vePasta = false;
                if((NFe.ConvertTxt.tpEventos)tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoNFe ||
                    (NFe.ConvertTxt.tpEventos)tpEvento == ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe)
                {
                    vePasta = Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe;
                }
                else
                {
                    vePasta = Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe;
                }

                if(vePasta)
                {
                    var folderNFe = OndeNFeEstaGravada(emp, ChaveNFe, Propriedade.ExtRetorno.ProcMDFe);
                    if(!string.IsNullOrEmpty(folderNFe))
                    {
                        filenameToWrite = Path.Combine(folderNFe, Path.GetFileName(filenameToWrite));
                    }
                }
            }
            string protEnvioEvento;
            if(xmlEventoEnvio.IndexOf("<procEventoMDFe") >= 0)
            {
                protEnvioEvento = xmlEventoEnvio;
            }
            else
            {
                protEnvioEvento = "<procEventoMDFe versao=\"" + versao + "\" xmlns=\"" + NFeStrConstants.NAME_SPACE_MDFE + "\">" +
                                  xmlEventoEnvio +
                                  xmlRetornoEnvio +
                                  "</procEventoMDFe>";
            }

            //Gravar o arquivo de distribuição na pasta de enviados autorizados
            if(!protEnvioEvento.StartsWith("<?xml"))
            {
                protEnvioEvento = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + protEnvioEvento;
            }

            //Gravar o arquivo de distribuição na pasta de backup
            if(!string.IsNullOrEmpty(filenameBackup))
            {
                // Criar a pasta de backup, caso não exista. Wandrey 25/05/211
                filenameBackup = Path.Combine(filenameBackup, tempXmlFile);
                if(!Directory.Exists(Path.GetDirectoryName(filenameBackup)))
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filenameBackup));
                }

                if(!File.Exists(filenameBackup))
                {
                    File.WriteAllText(filenameBackup, protEnvioEvento);
                }
            }
            // cria a pasta se não existir
            if(!Directory.Exists(Path.GetDirectoryName(filenameToWrite)))
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filenameToWrite));
            }

            if(!File.Exists(filenameToWrite))
            {
                File.WriteAllText(filenameToWrite, protEnvioEvento);
            }

            XmlParaFTP(emp, filenameToWrite);

            if(sendtodanfemon)
            {
                TFunctions.CopiarXMLPastaDanfeMon(filenameToWrite);
            }

            NomeArqGerado = filenameToWrite;
        }

        #endregion XmlDistEventoMDFe()

        #endregion XmlDistEvento

        #endregion -- Evento

        #endregion Métodos para gerar os XML´s de distribuição

        #region Métodos auxiliares

        private XmlElement CriaElemento(XmlDocument doc, string elName, string elValue, string ns)
        {
            var node = doc.CreateElement(elName, ns);
            node.InnerText = elValue;
            return node;
        }

        private XmlElement CriaElemento(XmlDocument doc, string elName, string elValue)
        {
            var node = doc.CreateElement(elName);
            node.InnerText = elValue;
            return node;
        }

        private XmlAttribute CriaAttribute(XmlDocument doc, string attrName, string attrValue)
        {
            var attribute = doc.CreateAttribute(attrName);
            attribute.Value = attrValue;
            return attribute;
        }

        #region OndeNFeEstaGravada

        public string OndeNFeEstaGravada(int emp, string ChaveNFe, string extensao)
        {
            var dte = Convert.ToDateTime("20" + ChaveNFe.Substring(2, 2) + "/" + ChaveNFe.Substring(4, 2) + "/1");
            string olddir = null;
            string retorno = null;
            for(var nd = 0; nd < 60; ++nd)
            {
                var dsc = TFunctions.getSubFolder(dte, nd, Empresas.Configuracoes[emp].DiretorioSalvarComo);//.ToString(dte.AddDays(nd));
                if(dsc != "")
                {
                    dsc = "\\" + dsc.TrimEnd('\\');
                }

                if(olddir != null && olddir.Equals(dsc))
                {
                    continue; //evitamos pesquisar por uma pasta que já haviamos pesquisado (AM, MA, ...)
                }

                olddir = dsc;

                // pesquisa onde o arquivo de distribuicao da nota foi gravado
                var files = System.IO.Path.Combine(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString() + dsc,
                                                               ChaveNFe + extensao);

                // Procurar por um arquivo com NFe no ínicio do nome.
                if(!File.Exists(files))
                {
                    files = System.IO.Path.Combine(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString() + dsc,
                                                            "nfe" + ChaveNFe + extensao);

                    //Procurar na pasta denegados
                    if(!File.Exists(files))
                    {
                        files = System.IO.Path.Combine(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString() + dsc,
                                                                ChaveNFe + Propriedade.ExtRetorno.Den);

                        // Procurar por um arquivo com NFe no ínicio do nome.
                        if(!File.Exists(files))
                        {
                            files = System.IO.Path.Combine(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString() + dsc,
                                                                    "nfe" + ChaveNFe + Propriedade.ExtRetorno.Den);
                        }
                    }
                }

                retorno = File.Exists(files) ? Path.GetDirectoryName(files) : "";

                if(!string.IsNullOrEmpty(retorno) || string.IsNullOrEmpty(dsc))
                {
                    break;
                }
            }

            if(string.IsNullOrEmpty(retorno))
            {
                ///
                /// nao a encontrando, pesquisa pela arvore de autorizados/denegados pois pode ser que o ERP pode ter
                /// mudado o tipo DiretorioSalvarComo
                ///
                var files = System.IO.Directory.GetFiles(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString(),
                                                              ChaveNFe + extensao,
                                                              SearchOption.AllDirectories);

                if(files.Length == 0)
                {
                    // Procurar por um arquivo com NFe no ínicio do nome.
                    files = System.IO.Directory.GetFiles(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString(),
                                                                  "nfe" + ChaveNFe + extensao,
                                                                  SearchOption.AllDirectories);

                    //Procurar na pasta denegados
                    if(files.Length == 0)
                    {
                        files = System.IO.Directory.GetFiles(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString(),
                                                             ChaveNFe + Propriedade.ExtRetorno.Den,
                                                             SearchOption.AllDirectories);

                        // Procurar por um arquivo com NFe no ínicio do nome.
                        if(files.Length == 0)
                        {
                            files = System.IO.Directory.GetFiles(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString(),
                                                                 "nfe" + ChaveNFe + Propriedade.ExtRetorno.Den,
                                                                 SearchOption.AllDirectories);
                        }
                    }
                }

                retorno = files.Length > 0 ? Path.GetDirectoryName(files[0]) : "";
            }

            return retorno;
        }

        #endregion OndeNFeEstaGravada

        #region LerXMLNfe()

        /// <summary>
        /// Le o conteudo do XML da NFe
        /// </summary>
        /// <param name="conteudoXML">Conteudo do XML</param>
        /// <returns>Dados do XML da NFe</returns>
        private DadosNFeClass LerXMLNFe(XmlDocument conteudoXML)
        {
            var oLerXML = new LerXML();

            switch(Servico)
            {
                case Servicos.MDFeAssinarValidarEnvioEmLote:
                case Servicos.MDFeMontarLoteVarios:
                case Servicos.MDFeMontarLoteUm:
                    oLerXML.Mdfe(conteudoXML);
                    break;

                case Servicos.CTeAssinarValidarEnvioEmLote:
                case Servicos.CTeMontarLoteVarios:
                case Servicos.CTeMontarLoteUm:
                    oLerXML.Cte(conteudoXML);
                    break;

                default:
                    oLerXML.Nfe(conteudoXML);
                    break;
            }

            return oLerXML.oDadosNfe;
        }

        #endregion LerXMLNfe()

        #region NomeArqLoteRetERP()

        protected string NomeArqLoteRetERP(string NomeArquivoXML)
        {
            var emp = Empresas.FindEmpresaByThread();

            var ext = Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML;

            if(NomeArquivoXML.ToLower().IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML) >= 0)
            {
                ext = Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML;
            }

            if(NomeArquivoXML.ToLower().IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML) >= 0)
            {
                ext = Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML;
            }

            return Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    Functions.ExtrairNomeArq(NomeArquivoXML, ext) + "-num-lot.xml";
        }

        #endregion NomeArqLoteRetERP()

        #region GravarArquivoParaEnvio

        /// <summary>
        /// grava um arquivo na pasta de envio
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="Conteudo"></param>
        protected void GravarArquivoParaEnvio(string Arquivo, string Conteudo) => GravarArquivoParaEnvio(Arquivo, Conteudo, false);

        protected void GravarArquivoParaEnvio(string Arquivo, string Conteudo, bool isUTF8)
        {
            var arqTemp = "";

            try
            {
                //Arquivo na pasta Temp
                arqTemp = Empresas.Configuracoes[EmpIndex].PastaXmlEnvio + "\\Temp\\" + Path.GetFileName(Arquivo);

                if(Arquivo.ToLower().IndexOf(Empresas.Configuracoes[EmpIndex].PastaValidar.ToLower()) >= 0)
                {
                    arqTemp = Arquivo;
                }

                if(string.IsNullOrEmpty(Arquivo))
                {
                    throw new Exception("Nome do arquivo deve ser informado");
                }

                MemoryStream oMemoryStream;
                ///
                ///<<<danasa 6-2011
                ///inclui o "isUTF8" para suportar a gravacao do XML da CCe - caso você queira, acho que pode ser tudo em UTF-8
                if(isUTF8)
                {
                    oMemoryStream = Functions.StringXmlToStreamUTF8(Conteudo);
                }
                else
                {
                    oMemoryStream = Functions.StringXmlToStream(Conteudo);
                }

                var docProc = new XmlDocument();
                docProc.Load(oMemoryStream);

                //Gravar o XML na pasta Temp
                docProc.Save(arqTemp);

                if(Arquivo.ToLower().IndexOf(Empresas.Configuracoes[EmpIndex].PastaValidar.ToLower()) >= 0)
                {
                    return;
                }

                //Mover XML da pasta Temp para Envio
                //Arquivo na pasta de Envio
                var arqEnvio = Empresas.Configuracoes[EmpIndex].PastaXmlEnvio + "\\" + Path.GetFileName(Arquivo);
                Functions.Move(arqTemp, arqEnvio);
            }
            catch(Exception ex)
            {
                Functions.GravarErroMover(arqTemp, Empresas.Configuracoes[Empresas.FindEmpresaByThread()].PastaXmlRetorno, ex.ToString());
            }
        }

        #endregion GravarArquivoParaEnvio

        #endregion Métodos auxiliares

        #endregion Métodos

        #region ProcessaConsultaCadastro()

        /// <summary>
        /// utilizada pela GerarXML
        /// </summary>
        /// <param name="msXml"></param>
        /// <returns></returns>
        ///
        private string ReadInnerText(string value)
        {
            value = value.Replace("&#231;", "ç");
            value = value.Replace("&#227;", "ã");
            value = value.Replace("&amp;", "&");
            value = value.Replace("&lt;", "<");
            value = value.Replace("&gt;", ">");
            value = value.Replace("&quot;", "\"");
            value = value.Replace("&#39;", "'");

            return value;
        }

        public RetConsCad ProcessaConsultaCadastro(XmlDocument doc)
        {
            if(doc.GetElementsByTagName("infCad") == null)
            {
                return null;
            }

            var vRetorno = new RetConsCad();

            foreach(XmlNode node in doc.ChildNodes)
            {
                if(node.Name == "retConsCad")
                {
                    foreach(XmlNode noderetConsCad in node.ChildNodes)
                    {
                        if(noderetConsCad.Name == "infCons")
                        {
                            foreach(XmlNode nodeinfCons in noderetConsCad.ChildNodes)
                            {
                                if(nodeinfCons.Name == "infCad")
                                {
                                    vRetorno.infCad.Add(new infCad());
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].CNPJ = vRetorno.CNPJ;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].CPF = vRetorno.CPF;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].IE = vRetorno.IE;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].UF = vRetorno.UF;

                                    foreach(XmlNode nodeinfCad in nodeinfCons.ChildNodes)
                                    {
                                        switch(nodeinfCad.Name)
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
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xNome = ReadInnerText(nodeinfCad.InnerText);
                                                break;

                                            case "xFant":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xFant = ReadInnerText(nodeinfCad.InnerText);
                                                break;

                                            case "ender":
                                                foreach(XmlNode nodeinfConsEnder in nodeinfCad.ChildNodes)
                                                {
                                                    switch(nodeinfConsEnder.Name)
                                                    {
                                                        case "xLgr":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xLgr = ReadInnerText(nodeinfConsEnder.InnerText);
                                                            break;

                                                        case "nro":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.nro = ReadInnerText(nodeinfConsEnder.InnerText);
                                                            break;

                                                        case "xCpl":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xCpl = ReadInnerText(nodeinfConsEnder.InnerText);
                                                            break;

                                                        case "xBairro":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xBairro = ReadInnerText(nodeinfConsEnder.InnerText);
                                                            break;

                                                        case "xMun":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xMun = ReadInnerText(nodeinfConsEnder.InnerText);
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
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dBaixa = Functions.GetDateTime(nodeinfCad.InnerText);
                                                break;

                                            case "dUltSit":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dUltSit = Functions.GetDateTime(nodeinfCad.InnerText);
                                                break;

                                            case "dIniAtiv":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dIniAtiv = Functions.GetDateTime(nodeinfCad.InnerText);
                                                break;

                                            case "CNAE":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].CNAE = Convert.ToInt32("0" + nodeinfCad.InnerText);
                                                break;

                                            case "xRegApur":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xRegApur = nodeinfCad.InnerText;
                                                break;

                                            case "cSit":
                                                if(nodeinfCad.InnerText == "0")
                                                {
                                                    vRetorno.infCad[vRetorno.infCad.Count - 1].cSit = "Contribuinte não habilitado";
                                                }
                                                else if(nodeinfCad.InnerText == "1")
                                                {
                                                    vRetorno.infCad[vRetorno.infCad.Count - 1].cSit = "Contribuinte habilitado";
                                                }

                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(nodeinfCons.Name)
                                    {
                                        case "cStat":
                                            vRetorno.cStat = Convert.ToInt32("0" + nodeinfCons.InnerText);
                                            break;

                                        case "xMotivo":
                                            vRetorno.xMotivo = ReadInnerText(nodeinfCons.InnerText);
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
                                            vRetorno.dhCons = Functions.GetDateTime(nodeinfCons.InnerText);
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

        #endregion ProcessaConsultaCadastro()

        #region ProcessaConsultaCadastro()

        public RetConsCad ProcessaConsultaCadastro(MemoryStream msXml)
        {
            var doc = new XmlDocument();
            doc.Load(msXml);
            return ProcessaConsultaCadastro(doc);
        }

        #endregion ProcessaConsultaCadastro()

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
            var doc = new XmlDocument();
            doc.Load(cArquivoXML);
            return ProcessaConsultaCadastro(doc);
        }

        #endregion ProcessaConsultaCadastro()

        #region XmlParaFTP

        public void XmlParaFTP(int emp, string vNomeDoArquivo)
        {
            // verifica se o FTP da empresa está ativo
            var vFolder = "";
            ///
            /// exclui o arquivo de erro de FTP
            Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Path.GetFileName(Path.ChangeExtension(vNomeDoArquivo, ".ftp"))));
            ///
            /// o arquivo é Autorizado ou Denegado?
            if(vNomeDoArquivo.Contains(PastaEnviados.Autorizados.ToString()) ||
                vNomeDoArquivo.Contains(PastaEnviados.Denegados.ToString()))
            {
                ///
                /// pega a pasta de enviados do FTP
                vFolder = Empresas.Configuracoes[emp].FTPPastaAutorizados;
                if(!string.IsNullOrEmpty(vFolder))
                {
                    ///
                    /// verifica se é para gravar na pasta especifica ou se é para gravar na mesma
                    /// hierarquia definida para gravar localmente
                    if(!Empresas.Configuracoes[emp].FTPGravaXMLPastaUnica)
                    {
                        var temp = vNomeDoArquivo.Split('\\');
                        ///
                        /// pega a ultima pasta atribuida
                        /// Ex: "c:\nfe\autorizados\201112\3539438493843493-procNFe.xml
                        /// pega a pasta "201112"
                        vFolder += "/" + temp[temp.Length - 2];
                    }
                }
            }
            else
            {
                ///
                /// pega a pasta de retorno no FTP para gravar os retornos dos webservices
                /// se vazia nao grava os retornos
                if(vNomeDoArquivo.ToLower().EndsWith(".xml") ||
                    vNomeDoArquivo.ToLower().EndsWith(".txt") ||
                    vNomeDoArquivo.ToLower().EndsWith(".err"))
                {
                    vFolder = Empresas.Configuracoes[emp].FTPPastaRetornos;
                }
            }

            if(!string.IsNullOrEmpty(vFolder))
            {
                if(!Empresas.Configuracoes[emp].FTPIsAlive)
                {
                    Auxiliar.WriteLog("Tentando enviar o arquivo '" + vNomeDoArquivo + "' para a pasta '" + vFolder + "' no FTP, mas o FTP está inativo.", false);
                }
                else
                {
                    try
                    {
                        Empresas.Configuracoes[emp].SendFileToFTP(vNomeDoArquivo, vFolder);
                    }
                    catch(Exception ex)
                    {
                        ///
                        /// grava um arquivo de erro com extensao "FTP" para diferenciar dos arquivos de erro
                        oAux.GravarArqErroERP(Path.ChangeExtension(vNomeDoArquivo, ".ftp"), ex.Message);
                    }
                }
            }
        }

        #endregion XmlParaFTP

        #region XML para consulta do DFe destinado

        public string XMLDistribuicaoDFe(Servicos servico, int tpAmb, int cUF, string versao, string cnpj, string ultNSU)
        {
            /*
            NFE
            <distDFeInt versao="1.01">
               <tpAmb>1</tpAmb>
               <cUFAutor>35</cUFAutor>
               <CNPJ>06117723000112</CNPJ>
               <distNSU>
                  <ultNSU>123456789012345</ultNSU>
               </distNSU>
            </distDFeInt>

            CTE
            <distDFeInt versao="1.00">
               <tpAmb>1</tpAmb>
               <cUFAutor>35</cUFAutor>
               <CNPJ>11111111111111</CNPJ>
               <distNSU>
                  <ultNSU>000000000000000</ultNSU>
               </distNSU>
            </distDFeInt>
            */

            //StatusServico(pArquivo, tpAmb, tpEmis, cUF, versao, "consStatServ", NFeStrConstants.NAME_SPACE_NFE);

            var nodeStr = "distDFeInt";
            var nsURI = "";
            switch(servico)
            {
                case Servicos.CTeDistribuicaoDFe:
                    nsURI = (servico == Servicos.CTeDistribuicaoDFe ? NFeStrConstants.NAME_SPACE_CTE : NFeStrConstants.NAME_SPACE_NFE);
                    break;

                case Servicos.DFeEnviar:
                    nsURI = (servico == Servicos.CTeDistribuicaoDFe ? NFeStrConstants.NAME_SPACE_CTE : NFeStrConstants.NAME_SPACE_NFE);
                    break;
            }

            var doc = new XmlDocument();
            doc.InsertBefore(doc.CreateXmlDeclaration("1.0", "UTF-8", ""), doc.DocumentElement);
            XmlNode node = doc.CreateElement(nodeStr);
            node.Attributes.Append(CriaAttribute(doc, TpcnResources.versao.ToString(), versao));
            node.Attributes.Append(CriaAttribute(doc, TpcnResources.xmlns.ToString(), nsURI));
            node.AppendChild(CriaElemento(doc, TpcnResources.tpAmb.ToString(), tpAmb.ToString()));
            node.AppendChild(CriaElemento(doc, TpcnResources.cUFAutor.ToString(), cUF.ToString()));
            node.AppendChild(CriaElemento(doc, TpcnResources.CNPJ.ToString(), cnpj));

            XmlNode nodedistNSU = doc.CreateElement("distNSU");
            nodedistNSU.AppendChild(CriaElemento(doc, TpcnResources.ultNSU.ToString(), ultNSU));
            node.AppendChild(nodedistNSU);

            doc.AppendChild(node);

            return doc.OuterXml;
        }

        #endregion XML para consulta do DFe destinado
    }

    public class ArquivoXMLDFe
    {
        public string NomeArquivoXML;
        public XmlDocument ConteudoXML;
    }
}
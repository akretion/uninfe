using NFe.Certificado;
using NFe.Components;
using NFe.Components.EGoverne;
using NFe.Components.EL;
using NFe.Components.FISSLEX;
using NFe.Components.Memory;
using NFe.Components.Metropolis;
using NFe.Components.Pronin;
using NFe.Components.SimplISS;
using NFe.Components.Tinus;
using NFe.Components.Simple;
using NFe.Settings;
using NFSe.Components;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskNFSeConsultaSituacaoLoteRps : TaskAbst
    {
        #region Objeto com os dados do XML de consulta situação do lote rps

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de consulta da situação do lote rps
        /// </summary>
        private DadosPedSitLoteRps oDadosPedSitLoteRps;

        #endregion Objeto com os dados do XML de consulta situação do lote rps

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            ///
            /// extensao permitida: PedSitLoteRps = "-ped-sitloterps.xml";
            ///
            /// Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeConsultarSituacaoLoteRps;

            try
            {
                oDadosPedSitLoteRps = new DadosPedSitLoteRps(emp);
                //Ler o XML para pegar parâmetros de envio
                //LerXML ler = new LerXML();
                PedSitLoteRps(NomeArquivoXML);

                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML) + Propriedade.ExtRetorno.SitLoteRps_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                //Definir o objeto do WebService

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosPedSitLoteRps.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedSitLoteRps = null;

                if (IsUtilizaCompilacaoWs(padraoNFSe, Servico, oDadosPedSitLoteRps.cMunicipio))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitLoteRps.cMunicipio, oDadosPedSitLoteRps.tpAmb, oDadosPedSitLoteRps.tpEmis, padraoNFSe, oDadosPedSitLoteRps.cMunicipio);
                    if (wsProxy != null) pedSitLoteRps = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedSitLoteRps.cMunicipio, oDadosPedSitLoteRps.tpAmb, oDadosPedSitLoteRps.tpEmis, padraoNFSe, Servico);

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        switch (oDadosPedSitLoteRps.cMunicipio)
                        {
                            case 2304400: //Fortaleza - CE
                                cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                                break;

                            case 4125506: //São José dos Pinhais - PR
                                cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://nfe.sjp.pr.gov.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                                break;

                            default:
                                cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                                break;
                        }
                        break;

                    case PadroesNFSe.BETHA:

                        #region Betha

                        ConteudoXML.PreserveWhitespace = false;
                        ConteudoXML.Load(NomeArquivoXML);

                        if (!ConteudoXML.DocumentElement.Name.Contains("e:"))
                        {
                            padraoNFSe = PadroesNFSe.BETHA202;
                            Components.Betha.NewVersion.Betha betha = new Components.Betha.NewVersion.Betha((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                oDadosPedSitLoteRps.cMunicipio,
                                Empresas.Configuracoes[emp].UsuarioWS,
                                Empresas.Configuracoes[emp].SenhaWS,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor);

                            betha.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        }
                        else
                        {
                            wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);
                            wsProxy.Betha = new Betha();
                        }
                        break;

                    #endregion Betha

                    case PadroesNFSe.ABACO:
                    case PadroesNFSe.CANOAS_RS:
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.BHISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.WEBISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.WEBISS_202:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://iss.irati.pr.gov.br/Arquivos/nfseV202.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SIMPLISS:
                        SimplISS simpliss = new SimplISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedSitLoteRps.cMunicipio,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor);

                        simpliss.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.EGOVERNE:

                        #region E-Governe

                        EGoverne egoverne = new EGoverne((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedSitLoteRps.cMunicipio,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor,
                                                        Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assegov = new AssinaturaDigital();
                        assegov.Assinar(NomeArquivoXML, emp, oDadosPedSitLoteRps.cMunicipio);

                        egoverne.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        break;

                    #endregion E-Governe

                    case PadroesNFSe.EL:

                        #region E&L

                        EL el = new EL((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedSitLoteRps.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyUsuario : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxySenha : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyServidor : ""));

                        el.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        break;

                    #endregion E&L

                    case PadroesNFSe.EQUIPLANO:
                        cabecMsg = "1";
                        break;

                    case PadroesNFSe.FISSLEX:
                        FISSLEX fisslex = new FISSLEX((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosPedSitLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        fisslex.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.NATALENSE:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1\" xmlns=\"http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd\"><versaoDados>1</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.CONAM:
                        throw new NFe.Components.Exceptions.ServicoInexistenteException();

                    case PadroesNFSe.PAULISTANA:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosPedSitLoteRps.tpAmb == 1)
                        {
                            pedSitLoteRps = new NFe.Components.PSaoPauloSP.LoteNFe();
                        }
                        else
                        {
                            throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                        }

                        break;

                    case PadroesNFSe.MEMORY:

                        #region Memory

                        Memory memory = new Memory((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosPedSitLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        memory.CancelarNfse(NomeArquivoXML);
                        break;

                    #endregion Memory

                    case PadroesNFSe.METROPOLIS:

                        #region METROPOLIS

                        Metropolis metropolis = new Metropolis((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      oDadosPedSitLoteRps.cMunicipio,
                                                      ConfiguracaoApp.ProxyUsuario,
                                                      ConfiguracaoApp.ProxySenha,
                                                      ConfiguracaoApp.ProxyServidor,
                                                      Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital metropolisdig = new AssinaturaDigital();
                        metropolisdig.Assinar(NomeArquivoXML, emp, oDadosPedSitLoteRps.cMunicipio);

                        metropolis.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        break;

                    #endregion METROPOLIS

                    case PadroesNFSe.PRONIN:
                        if (oDadosPedSitLoteRps.cMunicipio == 4109401 ||
                            oDadosPedSitLoteRps.cMunicipio == 3131703 ||
                            oDadosPedSitLoteRps.cMunicipio == 4303004 ||
                            oDadosPedSitLoteRps.cMunicipio == 4322509 ||
                            oDadosPedSitLoteRps.cMunicipio == 3556602 ||
                            oDadosPedSitLoteRps.cMunicipio == 3512803 || 
                            oDadosPedSitLoteRps.cMunicipio == 4323002 ||
                            oDadosPedSitLoteRps.cMunicipio == 3505807 || 
                            oDadosPedSitLoteRps.cMunicipio == 3530300 ||
                            oDadosPedSitLoteRps.cMunicipio == 4308904 ||
                            oDadosPedSitLoteRps.cMunicipio == 4118501 ||
                            oDadosPedSitLoteRps.cMunicipio == 3554300 ||
                            oDadosPedSitLoteRps.cMunicipio == 3542404 ||
                            oDadosPedSitLoteRps.cMunicipio == 5005707 ||
                            oDadosPedSitLoteRps.cMunicipio == 4314423 ||
                            oDadosPedSitLoteRps.cMunicipio == 3511102 ||
                            oDadosPedSitLoteRps.cMunicipio == 3535804 ||
                            oDadosPedSitLoteRps.cMunicipio == 4306932 ||
                            oDadosPedSitLoteRps.cMunicipio == 4322400 ||
                            oDadosPedSitLoteRps.cMunicipio == 4302808 ||
							oDadosPedSitLoteRps.cMunicipio == 3501301 ||
							oDadosPedSitLoteRps.cMunicipio == 4300109 ||
                            oDadosPedSitLoteRps.cMunicipio == 4124053 ||
                            oDadosPedSitLoteRps.cMunicipio == 4101408)
                        {
                            Pronin pronin = new Pronin((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                oDadosPedSitLoteRps.cMunicipio,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor,
                                Empresas.Configuracoes[emp].X509Certificado);

                            AssinaturaDigital assPronin = new AssinaturaDigital();
                            assPronin.Assinar(NomeArquivoXML, emp, oDadosPedSitLoteRps.cMunicipio);

                            pronin.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        }
                        break;

                    case PadroesNFSe.TINUS:
                        Tinus tinus = new Tinus((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosPedSitLoteRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        tinus.ConsultarSituacaoLoteRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.INTERSOL:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><p:cabecalho versao=\"1\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:p=\"http://ws.speedgov.com.br/cabecalho_v1.xsd\" xmlns:p1=\"http://ws.speedgov.com.br/tipos_v1.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://ws.speedgov.com.br/cabecalho_v1.xsd cabecalho_v1.xsd \"><versaoDados>1</versaoDados></p:cabecalho>";
                        break;

                    case PadroesNFSe.MANAUS_AM:
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.MODERNIZACAO_PUBLICA:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.E_RECEITA:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SIMPLE:

                        Simple simple = new Simple((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedSitLoteRps.cMunicipio,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor,
                                                        Empresas.Configuracoes[emp].X509Certificado);

                        simple.ConsultarSituacaoLoteRps(NomeArquivoXML);

                        break;

                    case PadroesNFSe.SISPMJP:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" ><versaoDados>2.02</versaoDados></cabecalho>";

                        break;

                    case PadroesNFSe.DSF:
                        if (oDadosPedSitLoteRps.cMunicipio == 3549904)
                        {
                            cabecMsg = "<cabecalho versao=\"3\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>3</versaoDados></cabecalho>";
                        }
                        break;
                }

                if (IsInvocar(padraoNFSe, Servico, oDadosPedSitLoteRps.cMunicipio))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, oDadosPedSitLoteRps.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedSitLoteRps, NomeMetodoWS(Servico, oDadosPedSitLoteRps.cMunicipio),
                                            cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML, //"-ped-sitloterps",
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML,  //"-sitloterps",
                                            padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML, Propriedade.ExtRetorno.SitLoteRps_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 31/08/2011
                }
            }
            finally
            {
                try
                {
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 31/08/2011
                }
            }
        }

        #endregion Execute

        #region PedSitLoteRps()

        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta situação do lote rps e disponibilizar conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedSitLoteRps(string arquivoXML)
        {
            //int emp = Empresas.FindEmpresaByThread();

            //XmlDocument doc = new XmlDocument();
            //doc.Load(arquivoXML);

            //XmlNodeList infConsList = doc.GetElementsByTagName("ConsultarSituacaoLoteRpsEnvio");

            //foreach (XmlNode infConsNode in infConsList)
            //{
            //    XmlElement infConsElemento = (XmlElement)infConsNode;
            //}
        }

        #endregion PedSitLoteRps()
    }
}
using NFe.Certificado;
using NFe.Components;
using NFe.Components.Coplan;
using NFe.Components.EGoverne;
using NFe.Components.EL;
using NFe.Components.Fiorilli;
using NFe.Components.Tinus;
using NFe.Components.FISSLEX;
using NFe.Components.GovDigital;
using NFe.Components.Memory;
using NFe.Components.Metropolis;
using NFe.Components.Pronin;
using NFe.Components.SimplISS;
using NFe.Components.Simple;
using NFe.Settings;
using NFSe.Components;
using System;
using System.IO;
using NFe.Components.Elotech;
#if _fw46
using System.ServiceModel;
using static NFe.Components.Security.SOAPSecurity;
#endif

namespace NFe.Service.NFSe
{
    public class TaskNFSeConsultarPorRps : TaskAbst
    {
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            ///
            /// extensao permitida: PedSitNfseRps = "-ped-sitnfserps.xml";
            ///
            /// Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeConsultarPorRps;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML) + Propriedade.ExtRetorno.SitNfseRps_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                //Ler o XML para pegar parâmetros de envio
                LerXML ler = new LerXML();
                ler.PedSitNfseRps(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(ler.oDadosPedSitNfseRps.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedLoteRps = null;
                if (IsUtilizaCompilacaoWs(padraoNFSe, Servico, ler.oDadosPedSitNfseRps.cMunicipio))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitNfseRps.cMunicipio, ler.oDadosPedSitNfseRps.tpAmb, ler.oDadosPedSitNfseRps.tpEmis, padraoNFSe, ler.oDadosPedSitNfseRps.cMunicipio);
                    if (wsProxy != null) pedLoteRps = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(ler.oDadosPedSitNfseRps.cMunicipio, ler.oDadosPedSitNfseRps.tpAmb, ler.oDadosPedSitNfseRps.tpEmis, padraoNFSe, Servico);

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        switch (ler.oDadosPedSitNfseRps.cMunicipio)
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

                    case PadroesNFSe.ABASE:
                        cabecMsg = "<cabecalho xmlns=\"http://nfse.abase.com.br/nfse.xsd\" versao =\"1.00\"><versaoDados>1.00</versaoDados></cabecalho>";
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
                                ler.oDadosPedSitNfseRps.cMunicipio,
                                Empresas.Configuracoes[emp].UsuarioWS,
                                Empresas.Configuracoes[emp].SenhaWS,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor);

                            betha.ConsultarNfsePorRps(NomeArquivoXML);
                        }
                        else
                        {
                            wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);
                            wsProxy.Betha = new Betha();
                        }
                        break;

                    #endregion Betha

                    case PadroesNFSe.IPM:

                        //código da cidade da receita federal, este arquivo pode ser encontrado em ~\uninfe\doc\Codigos_Cidades_Receita_Federal.xls</para>
                        //O código da cidade está hardcoded pois ainda está sendo usado apenas para campo mourão
                        IPM ipm = new IPM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                          Empresas.Configuracoes[emp].PastaXmlRetorno,
                                          Empresas.Configuracoes[emp].UsuarioWS,
                                          Empresas.Configuracoes[emp].SenhaWS,
                                          ler.oDadosPedSitNfseRps.cMunicipio);

                        if (ConfiguracaoApp.Proxy)
                            ipm.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                            
                        ipm.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

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

                    case PadroesNFSe.PORTOVELHENSE:
                        cabecMsg = "<cabecalho versao=\"2.00\" xmlns:ns2=\"http://www.w3.org/2000/09/xmldsig#\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.IIBRASIL:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.04\"><versaoDados>2.04</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FIORILLI:
                        Fiorilli fiorilli = new Fiorilli((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        ler.oDadosPedSitNfseRps.cMunicipio,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor,
                                                        Empresas.Configuracoes[emp].X509Certificado);

                        fiorilli.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SIMPLISS:
                        SimplISS simpliss = new SimplISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        ler.oDadosPedSitNfseRps.cMunicipio,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital adSimpliss = new AssinaturaDigital();
                        adSimpliss.Assinar(NomeArquivoXML, emp, ler.oDadosPedSitNfseRps.cMunicipio);

                        simpliss.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.EGOVERNE:

                        #region E-Governe

                        EGoverne egoverne = new EGoverne((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        ler.oDadosPedSitNfseRps.cMunicipio,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor,
                                                        Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assegov = new AssinaturaDigital();
                        assegov.Assinar(NomeArquivoXML, emp, ler.oDadosPedSitNfseRps.cMunicipio);

                        egoverne.ConsultarNfsePorRps(NomeArquivoXML);

                        #endregion E-Governe

                        break;

                    case PadroesNFSe.EL:

                        #region E&L

                        EL el = new EL((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        ler.oDadosPedSitNfseRps.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyUsuario : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxySenha : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyServidor : ""));

                        el.ConsultarNfsePorRps(NomeArquivoXML);

                        #endregion E&L

                        break;

                    case PadroesNFSe.GOVDIGITAL:
                        GovDigital govdig = new GovDigital((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                            Empresas.Configuracoes[emp].X509Certificado,
                                                            ler.oDadosPedSitNfseRps.cMunicipio,
                                                            ConfiguracaoApp.ProxyUsuario,
                                                            ConfiguracaoApp.ProxySenha,
                                                            ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital adgovdig = new AssinaturaDigital();
                        adgovdig.Assinar(NomeArquivoXML, emp, ler.oDadosPedSitNfseRps.cMunicipio);

                        govdig.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.EQUIPLANO:
                        cabecMsg = "1";
                        break;

                    case PadroesNFSe.RLZ_INFORMATICA_02:
                        if (ler.oDadosPedSitNfseRps.cMunicipio == 5107958)
                            cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.PORTALFACIL_ACTCON_202:
                        if (ler.oDadosPedSitNfseRps.cMunicipio != 3169901)
                            cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.PORTALFACIL_ACTCON:
                    case PadroesNFSe.PRODATA:
                        cabecMsg = "<cabecalho versao=\"2.01\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"<versaoDados>2.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FISSLEX:
                        FISSLEX fisslex = new FISSLEX((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        ler.oDadosPedSitNfseRps.cMunicipio,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor);

                        fisslex.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.NATALENSE:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1\" xmlns=\"http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd\"><versaoDados>1</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.CONAM:
                        throw new NFe.Components.Exceptions.ServicoInexistenteException();

                    case PadroesNFSe.PAULISTANA:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (ler.oDadosPedSitNfseRps.tpAmb == 1)
                        {
                            pedLoteRps = new NFe.Components.PSaoPauloSP.LoteNFe();
                        }
                        else
                        {
                            throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                        }

                        break;

                    case PadroesNFSe.METROPOLIS:

                        #region METROPOLIS

                        Metropolis metropolis = new Metropolis((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      ler.oDadosPedSitNfseRps.cMunicipio,
                                                      ConfiguracaoApp.ProxyUsuario,
                                                      ConfiguracaoApp.ProxySenha,
                                                      ConfiguracaoApp.ProxyServidor,
                                                      Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital metropolisdig = new AssinaturaDigital();
                        metropolisdig.Assinar(NomeArquivoXML, emp, ler.oDadosPedSitNfseRps.cMunicipio);

                        metropolis.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    #endregion METROPOLIS

                    case PadroesNFSe.MEMORY:

                        #region Memory

                        Memory memory = new Memory((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        ler.oDadosPedSitNfseRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        memory.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    #endregion Memory

                    case PadroesNFSe.CAMACARI_BA:
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados><versao>2.01</versao></cabecalho>";
                        break;

                    case PadroesNFSe.NA_INFORMATICA:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        //if (ler.oDadosPedSitNfseRps.tpAmb == 1)
                        //    pedLoteRps = new Components.PCorumbaMS.NfseWSService();
                        //else
                        //    pedLoteRps = new Components.HCorumbaMS.NfseWSService();

                        break;

                    case PadroesNFSe.BSITBR:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (ler.oDadosPedSitNfseRps.tpAmb == 1)
                        {
                            switch (ler.oDadosPedSitNfseRps.cMunicipio)
                            {
                                case 5211800:
                                    pedLoteRps = new Components.PJaraguaGO.nfseWS();
                                    break;

                                case 5220454:
                                    pedLoteRps = new Components.PSenadorCanedoGO.nfseWS();
                                    break;

                                case 3507506:
                                    pedLoteRps = new Components.PBotucatuSP.nfseWS();
                                    break;

                                case 5211909:
                                    pedLoteRps = new Components.PJataiGO.nfseWS();
                                    break;

                                case 5220603:
                                    pedLoteRps = new Components.PSilvaniaGO.nfseWS();
                                    break;
                            }
                        }
                        else
                            throw new Exception("Este município não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                        break;

                    case PadroesNFSe.PRONIN:
                        if (ler.oDadosPedSitNfseRps.cMunicipio == 4109401 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3131703 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4303004 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4322509 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3556602 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3512803 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4323002 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3505807 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3530300 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4308904 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4118501 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3554300 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3542404 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 5005707 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4314423 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3511102 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3535804 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4306932 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4322400 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4302808 ||
							ler.oDadosPedSitNfseRps.cMunicipio == 3501301 ||
							ler.oDadosPedSitNfseRps.cMunicipio == 4300109 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4124053 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4101408 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3550407 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 4310207 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 1502400 ||
                            ler.oDadosPedSitNfseRps.cMunicipio == 3550803)
                        {
                            Pronin pronin = new Pronin((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                ler.oDadosPedSitNfseRps.cMunicipio,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor,
                                Empresas.Configuracoes[emp].X509Certificado);

                            AssinaturaDigital assPronin = new AssinaturaDigital();
                            assPronin.Assinar(NomeArquivoXML, emp, ler.oDadosPedSitNfseRps.cMunicipio);

                            pronin.ConsultarNfsePorRps(NomeArquivoXML);
                        }
                        break;

                    case PadroesNFSe.COPLAN:
                        Coplan coplan = new Coplan((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            ler.oDadosPedSitNfseRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assCoplan = new AssinaturaDigital();
                        assCoplan.Assinar(NomeArquivoXML, emp, ler.oDadosPedSitNfseRps.cMunicipio);

                        coplan.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.TINUS:
                        Tinus tinus = new Tinus((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            ler.oDadosPedSitNfseRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        tinus.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.INTERSOL:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><p:cabecalho versao=\"1\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:p=\"http://ws.speedgov.com.br/cabecalho_v1.xsd\" xmlns:p1=\"http://ws.speedgov.com.br/tipos_v1.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://ws.speedgov.com.br/cabecalho_v1.xsd cabecalho_v1.xsd \"><versaoDados>1</versaoDados></p:cabecalho>";
                        break;

                    case PadroesNFSe.MANAUS_AM:
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.JOINVILLE_SC:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (ler.oDadosPedSitNfseRps.tpAmb == 2)
                            pedLoteRps = new Components.HJoinvilleSC.Servicos();
                        else
                            pedLoteRps = new Components.PJoinvilleSC.Servicos();
                        break;

                    case PadroesNFSe.AVMB_ASTEN:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (ler.oDadosPedSitNfseRps.tpAmb == 2)
                            pedLoteRps = new Components.HPelotasRS.INfseservice();
                        else
                            pedLoteRps = new Components.PPelotasRS.INfseservice();
                        break;

                    case PadroesNFSe.EMBRAS:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.MODERNIZACAO_PUBLICA:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.E_RECEITA:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.TIPLAN_203:
                    case PadroesNFSe.INDAIATUBA_SP:
                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;
#if _fw46
                    case PadroesNFSe.ADM_SISTEMAS:
                        cabecMsg = "<cabecalho versao=\"2.01\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.01</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        pedLoteRps = ler.oDadosPedSitNfseRps.tpAmb == 1 ?
                                        new Components.PAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://demo.saatri.com.br/servicos/nfse.svc")) :
                                        new Components.HAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://homologa-demo.saatri.com.br/servicos/nfse.svc")) as object;

                        SignUsingCredentials(emp, pedLoteRps);
                        break;
#endif

                    case PadroesNFSe.PUBLIC_SOFT:
                        break;

                    case PadroesNFSe.MEGASOFT:
                        cabecMsg = "<cabecalho versao=\"1.00\" xmlns=\"http://megasoftarrecadanet.com.br/xsd/nfse_v01.xsd\"><versaoDados>1.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SIMPLE:

                        Simple simple = new Simple((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        ler.oDadosPedSitNfseRps.cMunicipio,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor,
                                                        Empresas.Configuracoes[emp].X509Certificado);

                        simple.ConsultarNfsePorRps(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SISPMJP:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" ><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SIGCORP_SIGISS_203:
                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SMARAPD_204:
                        cabecMsg = "<cabecalho versao=\"2.04\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.04</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.DSF:
                        if (ler.oDadosPedSitNfseRps.cMunicipio == 3549904)
                        {
                            cabecMsg = "<cabecalho versao=\"3\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>3</versaoDados></cabecalho>";
                        }
                        break;

                    case PadroesNFSe.ELOTECH:
                        Elotech elotech = new Elotech((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            ler.oDadosPedSitNfseRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        elotech.ConsultarNfsePorRps(NomeArquivoXML);
                        break;
                }

                if (IsInvocar(padraoNFSe, Servico, ler.oDadosPedSitNfseRps.cMunicipio))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, ler.oDadosPedSitNfseRps.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, ler.oDadosPedSitNfseRps.cMunicipio), cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,   //"-ped-sitnfserps",
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML, //"-sitnfserps",
                                            padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML, Propriedade.ExtRetorno.SitNfseRps_ERR, ex);
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
    }
}
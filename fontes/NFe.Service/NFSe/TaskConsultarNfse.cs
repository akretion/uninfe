using NFe.Certificado;
using NFe.Components;
using NFe.Components.BAURU_SP;
using NFe.Components.Conam;
using NFe.Components.Consist;
using NFe.Components.Coplan;
using NFe.Components.EGoverne;
using NFe.Components.EL;
using NFe.Components.Elotech;
using NFe.Components.Fiorilli;
using NFe.Components.FISSLEX;
using NFe.Components.GeisWeb;
using NFe.Components.GovDigital;
using NFe.Components.Memory;
using NFe.Components.Metropolis;
using NFe.Components.MGM;
using NFe.Components.Pronin;
using NFe.Components.RLZ_INFORMATICA;
using NFe.Components.SigCorp;
using NFe.Components.Simple;
using NFe.Components.SimplISS;
using NFe.Components.SystemPro;
using NFe.Components.Tinus;
using NFe.Components.WEBFISCO_TECNOLOGIA;
using NFe.Settings;
using NFSe.Components;
using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
using static NFe.Components.Security.SOAPSecurity;

namespace NFe.Service.NFSe
{
    public class TaskNFSeConsultar: TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oDadosPedSitNfse;

        #endregion Objeto com os dados do XML da consulta nfse

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            ///
            /// extensao permitida:  PedSitNfse = "-ped-sitnfse.xml"
            ///
            /// Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeConsultar;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML) + Propriedade.ExtRetorno.SitNfse_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedSitNfse = new DadosPedSitNfse(emp);
                PedSitNfse(NomeArquivoXML);
                var padraoNFSe = Functions.PadraoNFSe(oDadosPedSitNfse.cMunicipio);

                switch(padraoNFSe)
                {
                    case PadroesNFSe.PRODATA:
                    case PadroesNFSe.BETHA:
                        ExecuteDLL(emp, oDadosPedSitNfse.cMunicipio, padraoNFSe);
                        break;

                    default:
                        switch(oDadosPedSitNfse.cMunicipio)
                        {
                            #region Municípios do Padrão SIGCORP
                            case 4105508: //Cianorte-PR
                            case 3303203: //Nilópolis-RJ
                            case 3305109: //São João de Meriti-RJ
                            case 3505500: //Barretos-SP
                                ExecuteDLL(emp, oDadosPedSitNfse.cMunicipio, padraoNFSe);
                                break;
                            #endregion

                            default:
                                WebServiceProxy wsProxy = null;
                                object pedLoteRps = null;

                                if(IsUtilizaCompilacaoWs(padraoNFSe, Servico, oDadosPedSitNfse.cMunicipio))
                                {
                                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, oDadosPedSitNfse.cMunicipio);
                                    if(wsProxy != null)
                                    {
                                        pedLoteRps = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                                    }
                                }

                                var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, Servico);

                                var cabecMsg = "";
                                switch(padraoNFSe)
                                {
                                    case PadroesNFSe.GINFES:
                                        switch(oDadosPedSitNfse.cMunicipio)
                                        {
                                            case 4125506: //São José dos Pinhais - PR
                                                cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://nfe.sjp.pr.gov.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                                                break;

                                            default:
                                                cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                                                break;
                                        }
                                        break;

                                    case PadroesNFSe.IPM:
                                        //código da cidade da receita federal, este arquivo pode ser encontrado em ~\uninfe\doc\Codigos_Cidades_Receita_Federal.xls</para>
                                        //O código da cidade está hardcoded pois ainda está sendo usado apenas para campo mourão
                                        var ipm = new IPM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                          Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                          Empresas.Configuracoes[emp].UsuarioWS,
                                                          Empresas.Configuracoes[emp].SenhaWS,
                                                          oDadosPedSitNfse.cMunicipio);

                                        if(ConfiguracaoApp.Proxy)
                                        {
                                            ipm.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                        }

                                        ipm.ConsultarNfse(NomeArquivoXML);
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

                                    case PadroesNFSe.SYSTEMPRO:
                                        var syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado, oDadosPedSitNfse.cMunicipio);
                                        var ad = new AssinaturaDigital();
                                        ad.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);
                                        syspro.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SIGCORP_SIGISS:
                                        var sigCorp = new SigCorp((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedSitNfse.cMunicipio);
                                        sigCorp.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SIGCORP_SIGISS_203:
                                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.SIMPLISS:
                                        var simpliss = new SimplISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                            oDadosPedSitNfse.cMunicipio,
                                                                            Empresas.Configuracoes[emp].UsuarioWS,
                                                                            Empresas.Configuracoes[emp].SenhaWS,
                                                                            ConfiguracaoApp.ProxyUsuario,
                                                                            ConfiguracaoApp.ProxySenha,
                                                                            ConfiguracaoApp.ProxyServidor);

                                        simpliss.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.CONAM:
                                        var conam = new Conam((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                oDadosPedSitNfse.cMunicipio,
                                                                Empresas.Configuracoes[emp].UsuarioWS,
                                                                Empresas.Configuracoes[emp].SenhaWS);

                                        conam.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.RLZ_INFORMATICA:
                                        var rlz = new Rlz_Informatica((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                                    Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                                    oDadosPedSitNfse.cMunicipio);

                                        rlz.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.EGOVERNE:

                                        #region E-Governe

                                        var egoverne = new EGoverne((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedSitNfse.cMunicipio,
                                        ConfiguracaoApp.ProxyUsuario,
                                        ConfiguracaoApp.ProxySenha,
                                        ConfiguracaoApp.ProxyServidor,
                                        Empresas.Configuracoes[emp].X509Certificado);

                                        var assegov = new AssinaturaDigital();
                                        assegov.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                                        egoverne.ConsultarNfse(NomeArquivoXML);

                                        #endregion E-Governe

                                        break;

                                    case PadroesNFSe.EL:

                                        #region E&L

                                        var el = new EL((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedSitNfse.cMunicipio,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyUsuario : ""),
                                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxySenha : ""),
                                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyServidor : ""));

                                        el.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    #endregion E&L

                                    case PadroesNFSe.GOVDIGITAL:
                                        var govdig = new GovDigital((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                            Empresas.Configuracoes[emp].X509Certificado,
                                                                            oDadosPedSitNfse.cMunicipio,
                                                                            ConfiguracaoApp.ProxyUsuario,
                                                                            ConfiguracaoApp.ProxySenha,
                                                                            ConfiguracaoApp.ProxyServidor);

                                        var adgovdig = new AssinaturaDigital();
                                        adgovdig.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                                        govdig.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.EQUIPLANO:
                                        cabecMsg = "1";
                                        break;

                                    case PadroesNFSe.RLZ_INFORMATICA_02:
                                        if(oDadosPedSitNfse.cMunicipio == 5107958)
                                        {
                                            cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                                        }

                                        break;

                                    case PadroesNFSe.PORTALFACIL_ACTCON_202:
                                        if(oDadosPedSitNfse.cMunicipio != 3169901)
                                        {
                                            cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                                        }

                                        break;

                                    case PadroesNFSe.PORTALFACIL_ACTCON:
                                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.FISSLEX:
                                        var fisslex = new FISSLEX((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        oDadosPedSitNfse.cMunicipio,
                                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                                        ConfiguracaoApp.ProxyUsuario,
                                                                        ConfiguracaoApp.ProxySenha,
                                                                        ConfiguracaoApp.ProxyServidor);

                                        fisslex.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.MGM:

                                        #region MGM

                                        var mgm = new MGM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                           oDadosPedSitNfse.cMunicipio,
                                                           Empresas.Configuracoes[emp].UsuarioWS,
                                                           Empresas.Configuracoes[emp].SenhaWS);
                                        mgm.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    #endregion MGM

                                    case PadroesNFSe.NATALENSE:
                                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1\" xmlns=\"http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd\"><versaoDados>1</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.CONSIST:

                                        #region Consist

                                        var consist = new Consist((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedSitNfse.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        ConfiguracaoApp.ProxyUsuario,
                                        ConfiguracaoApp.ProxySenha,
                                        ConfiguracaoApp.ProxyServidor);

                                        consist.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    #endregion Consist

                                    case PadroesNFSe.METROPOLIS:

                                        #region METROPOLIS

                                        var metropolis = new Metropolis((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                      oDadosPedSitNfse.cMunicipio,
                                                                      ConfiguracaoApp.ProxyUsuario,
                                                                      ConfiguracaoApp.ProxySenha,
                                                                      ConfiguracaoApp.ProxyServidor,
                                                                      Empresas.Configuracoes[emp].X509Certificado);

                                        var metropolisdig = new AssinaturaDigital();
                                        metropolisdig.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                                        metropolis.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    #endregion METROPOLIS

                                    case PadroesNFSe.PAULISTANA:
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        if(oDadosPedSitNfse.tpAmb == 1)
                                        {
                                            pedLoteRps = new NFe.Components.PSaoPauloSP.LoteNFe();
                                        }
                                        else
                                        {
                                            throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                                        }

                                        break;

                                    case PadroesNFSe.MEMORY:

                                        #region Memory

                                        var memory = new Memory((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedSitNfse.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        ConfiguracaoApp.ProxyUsuario,
                                        ConfiguracaoApp.ProxySenha,
                                        ConfiguracaoApp.ProxyServidor);

                                        memory.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    #endregion Memory

                                    case PadroesNFSe.CAMACARI_BA:
                                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados><versao>2.01</versao></cabecalho>";
                                        break;

                                    case PadroesNFSe.NA_INFORMATICA:
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        //if (oDadosPedSitNfse.tpAmb == 1)
                                        //    pedLoteRps = new Components.PCorumbaMS.NfseWSService();
                                        //else
                                        //    pedLoteRps = new Components.HCorumbaMS.NfseWSService();

                                        break;

                                    case PadroesNFSe.FIORILLI:
                                        var fiorilli = new Fiorilli((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        oDadosPedSitNfse.cMunicipio,
                                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                                        ConfiguracaoApp.ProxyUsuario,
                                                                        ConfiguracaoApp.ProxySenha,
                                                                        ConfiguracaoApp.ProxyServidor,
                                                                        Empresas.Configuracoes[emp].X509Certificado);

                                        fiorilli.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.PRONIN:
                                        if(oDadosPedSitNfse.cMunicipio == 3131703 ||
                                            oDadosPedSitNfse.cMunicipio == 4303004 ||
                                            oDadosPedSitNfse.cMunicipio == 4322509 ||
                                            oDadosPedSitNfse.cMunicipio == 3556602 ||
                                            oDadosPedSitNfse.cMunicipio == 3512803 ||
                                            oDadosPedSitNfse.cMunicipio == 4323002 ||
                                            oDadosPedSitNfse.cMunicipio == 3505807 ||
                                            oDadosPedSitNfse.cMunicipio == 3530300 ||
                                            oDadosPedSitNfse.cMunicipio == 4308904 ||
                                            oDadosPedSitNfse.cMunicipio == 4118501 ||
                                            oDadosPedSitNfse.cMunicipio == 3554300 ||
                                            oDadosPedSitNfse.cMunicipio == 3542404 ||
                                            oDadosPedSitNfse.cMunicipio == 5005707 ||
                                            oDadosPedSitNfse.cMunicipio == 4314423 ||
                                            oDadosPedSitNfse.cMunicipio == 3511102 ||
                                            oDadosPedSitNfse.cMunicipio == 3535804 ||
                                            oDadosPedSitNfse.cMunicipio == 4306932 ||
                                            oDadosPedSitNfse.cMunicipio == 4322400 ||
                                            oDadosPedSitNfse.cMunicipio == 4302808 ||
                                            oDadosPedSitNfse.cMunicipio == 3501301 ||
                                            oDadosPedSitNfse.cMunicipio == 4300109 ||
                                            oDadosPedSitNfse.cMunicipio == 4124053 ||
                                            oDadosPedSitNfse.cMunicipio == 4101408 ||
                                            oDadosPedSitNfse.cMunicipio == 3550407 ||
                                            oDadosPedSitNfse.cMunicipio == 4310207 ||
                                            oDadosPedSitNfse.cMunicipio == 1502400 ||
                                            oDadosPedSitNfse.cMunicipio == 3550803)
                                        {
                                            var pronin = new Pronin((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                oDadosPedSitNfse.cMunicipio,
                                                ConfiguracaoApp.ProxyUsuario,
                                                ConfiguracaoApp.ProxySenha,
                                                ConfiguracaoApp.ProxyServidor,
                                                Empresas.Configuracoes[emp].X509Certificado);

                                            var assPronin = new AssinaturaDigital();
                                            assPronin.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                                            pronin.ConsultarNfse(NomeArquivoXML);
                                        }
                                        break;

                                    case PadroesNFSe.BAURU_SP:
                                        var bauru_SP = new Bauru_SP((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedSitNfse.cMunicipio);
                                        bauru_SP.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.COPLAN:
                                        var coplan = new Coplan((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedSitNfse.cMunicipio,
                                            ConfiguracaoApp.ProxyUsuario,
                                            ConfiguracaoApp.ProxySenha,
                                            ConfiguracaoApp.ProxyServidor,
                                            Empresas.Configuracoes[emp].X509Certificado);

                                        var assCoplan = new AssinaturaDigital();
                                        assCoplan.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                                        coplan.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.TINUS:
                                        var tinus = new Tinus((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedSitNfse.cMunicipio,
                                            ConfiguracaoApp.ProxyUsuario,
                                            ConfiguracaoApp.ProxySenha,
                                            ConfiguracaoApp.ProxyServidor,
                                            Empresas.Configuracoes[emp].X509Certificado);

                                        tinus.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.GEISWEB:
                                        var geisWeb = new GeisWeb((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedSitNfse.cMunicipio,
                                            ConfiguracaoApp.ProxyUsuario,
                                            ConfiguracaoApp.ProxySenha,
                                            ConfiguracaoApp.ProxyServidor,
                                            Empresas.Configuracoes[emp].X509Certificado);

                                        geisWeb.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SH3:
                                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                                        break;

                                    case PadroesNFSe.INTERSOL:
                                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><p:cabecalho versao=\"1\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:p=\"http://ws.speedgov.com.br/cabecalho_v1.xsd\" xmlns:p1=\"http://ws.speedgov.com.br/tipos_v1.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://ws.speedgov.com.br/cabecalho_v1.xsd cabecalho_v1.xsd \"><versaoDados>1</versaoDados></p:cabecalho>";
                                        break;

                                    case PadroesNFSe.SOFTPLAN:
                                        var softplan = new Components.SOFTPLAN.SOFTPLAN((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        Empresas.Configuracoes[emp].TokenNFse,
                                                                        Empresas.Configuracoes[emp].TokenNFSeExpire,
                                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                                        Empresas.Configuracoes[emp].ClientID,
                                                                        Empresas.Configuracoes[emp].ClientSecret);

                                        if(ConfiguracaoApp.Proxy)
                                        {
                                            softplan.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                        }

                                        softplan.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    #region CENTI

                                    case PadroesNFSe.CENTI:
                                        var centi = new Components.CENTI.CENTI((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                      Empresas.Configuracoes[emp].UsuarioWS,
                                                                      Empresas.Configuracoes[emp].SenhaWS);

                                        if(ConfiguracaoApp.Proxy)
                                        {
                                            centi.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                        }

                                        centi.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    #endregion CENTI

                                    case PadroesNFSe.MANAUS_AM:
                                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.AVMB_ASTEN:
                                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        if(oDadosPedSitNfse.tpAmb == 2)
                                        {
                                            pedLoteRps = new Components.HPelotasRS.INfseservice();
                                        }
                                        else
                                        {
                                            pedLoteRps = new Components.PPelotasRS.INfseservice();
                                        }

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

                                    case PadroesNFSe.ADM_SISTEMAS:
                                        cabecMsg = "<cabecalho versao=\"2.01\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.01</versaoDados></cabecalho>";
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        pedLoteRps = oDadosPedSitNfse.tpAmb == 1 ?
                                                        new Components.PAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://demo.saatri.com.br/servicos/nfse.svc")) :
                                                        new Components.HAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://homologa-demo.saatri.com.br/servicos/nfse.svc")) as object;

                                        SignUsingCredentials(emp, pedLoteRps);
                                        break;

                                    case PadroesNFSe.PUBLIC_SOFT:
                                        break;

                                    case PadroesNFSe.SIMPLE:

                                        var simple = new Simple((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        oDadosPedSitNfse.cMunicipio,
                                                                        ConfiguracaoApp.ProxyUsuario,
                                                                        ConfiguracaoApp.ProxySenha,
                                                                        ConfiguracaoApp.ProxyServidor,
                                                                        Empresas.Configuracoes[emp].X509Certificado);

                                        simple.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SISPMJP:
                                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" ><versaoDados>2.02</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.SMARAPD_204:
                                        cabecMsg = "<cabecalho versao=\"2.04\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.04</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.DSF:
                                        if(oDadosPedSitNfse.cMunicipio == 3549904)
                                        {
                                            cabecMsg = "<cabecalho versao=\"3\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>3</versaoDados></cabecalho>";
                                        }
                                        break;

                                    case PadroesNFSe.WEBFISCO_TECNOLOGIA:
                                        var webTecnologia = new WEBFISCO_TECNOLOGIA((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                           oDadosPedSitNfse.cMunicipio,
                                                           Empresas.Configuracoes[emp].UsuarioWS,
                                                           Empresas.Configuracoes[emp].SenhaWS);
                                        webTecnologia.ConsultarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.ELOTECH:
                                        var elotech = new Elotech((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                oDadosPedSitNfse.cMunicipio,
                                                ConfiguracaoApp.ProxyUsuario,
                                                ConfiguracaoApp.ProxySenha,
                                                ConfiguracaoApp.ProxyServidor,
                                                Empresas.Configuracoes[emp].X509Certificado);

                                        elotech.ConsultarNfse(NomeArquivoXML);
                                        break;
                                }

                                if(IsInvocar(padraoNFSe, Servico, oDadosPedSitNfse.cMunicipio))
                                {
                                    //Assinar o XML
                                    var ad = new AssinaturaDigital();
                                    ad.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                                    //Invocar o método que envia o XML para o SEFAZ
                                    oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, oDadosPedSitNfse.cMunicipio),
                                                            cabecMsg, this,
                                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,    //"-ped-sitnfse",
                                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML,     //"-sitnfse",
                                                            padraoNFSe, Servico, securityProtocolType);

                                    ///
                                    /// grava o arquivo no FTP
                                    var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
                                    if(File.Exists(filenameFTP))
                                    {
                                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                                    }
                                }

                                break;
                        }

                        break;
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML, Propriedade.ExtRetorno.SitNfse_ERR, ex);
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

        #region PedSitNfse()

        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por numero e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedSitNfse(string arquivoXML)
        {
        }

        #endregion PedSitNfse()

        /// <summary>
        /// Executa o serviço utilizando a DLL do UniNFe.
        /// </summary>
        /// <param name="emp">Empresa que está enviando o XML</param>
        /// <param name="municipio">Código do município para onde será enviado o XML</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        private void ExecuteDLL(int emp, int municipio, PadroesNFSe padraoNFSe)
        {
            var conteudoXML = new XmlDocument();
            conteudoXML.Load(NomeArquivoXML);

            var finalArqEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML;
            var finalArqRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML;
            var versaoXML = DefinirVersaoXML(municipio, conteudoXML, padraoNFSe);
            var servico = DefinirServico(municipio, conteudoXML, padraoNFSe, versaoXML);

            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, finalArqEnvio) + Functions.ExtractExtension(finalArqRetorno) + ".err");

            var configuracao = new Unimake.Business.DFe.Servicos.Configuracao
            {
                TipoDFe = Unimake.Business.DFe.Servicos.TipoDFe.NFSe,
                CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado,
                TipoAmbiente = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                CodigoMunicipio = municipio,
                Servico = servico,
                SchemaVersao = versaoXML
            };

            switch(servico)
            {
                case Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNfse:
                    var consultarNfse = new Unimake.Business.DFe.Servicos.NFSe.ConsultarNfse(conteudoXML, configuracao);
                    consultarNfse.Executar();

                    vStrXmlRetorno = consultarNfse.RetornoWSString;
                    break;

                case Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNfseFaixa:
                    var consultarNfseFaixa = new Unimake.Business.DFe.Servicos.NFSe.ConsultarNfseFaixa(conteudoXML, configuracao);
                    consultarNfseFaixa.Executar();

                    vStrXmlRetorno = consultarNfseFaixa.RetornoWSString;
                    break;

                case Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNotaPrestador:
                    var consultarNotaPrestador = new Unimake.Business.DFe.Servicos.NFSe.ConsultarNotaPrestador(conteudoXML, configuracao);
                    consultarNotaPrestador.Executar();

                    vStrXmlRetorno = consultarNotaPrestador.RetornoWSString;
                    break;
            }


            XmlRetorno(finalArqEnvio, finalArqRetorno);

            /// grava o arquivo no FTP
            var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);

            if(File.Exists(filenameFTP))
            {
                new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
        }

        /// <summary>
        /// Define qual o tipo de serviço de envio de NFSe será utilizado. Envio em lote sincrono, Envio em lote assincrono ou envio de uma única NFSe síncrono.
        /// </summary>
        /// <param name="municipio">Código do município para onde será enviado o XML</param>
        /// <param name="doc">Conteúdo do XML da NFSe</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        /// <param name="versaoXML">Versão do XML</param>
        /// <returns>Retorna o tipo de serviço de envio da NFSe da prefeitura será utilizado</returns>
        private Unimake.Business.DFe.Servicos.Servico DefinirServico(int municipio, XmlDocument doc, PadroesNFSe padraoNFSe, string versaoXML)
        {
            var result = Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNfse;

            switch(padraoNFSe)
            {
                case PadroesNFSe.SIGCORP_SIGISS:
                    result = Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNotaPrestador;
                    break;

                case PadroesNFSe.PRODATA:
                    result = Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNfseFaixa;
                    break;

                case PadroesNFSe.BETHA:
                    if(versaoXML == "2.02")
                    {
                        result = Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNfseFaixa;
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Retorna a versão do XML que está sendo enviado para o município de acordo com o Padrão/Município
        /// </summary>
        /// <param name="codMunicipio">Código do município para onde será enviado o XML</param>
        /// <param name="xmlDoc">Conteúdo do XML da NFSe</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        /// <returns>Retorna a versão do XML que está sendo enviado para o município de acordo com o Padrão/Município</returns>
        private string DefinirVersaoXML(int codMunicipio, XmlDocument xmlDoc, PadroesNFSe padraoNFSe)
        {
            var versaoXML = "0.00";

            switch(padraoNFSe)
            {
                case PadroesNFSe.PRODATA:
                    versaoXML = Functions.GetAttributeXML("LoteRps", "versao", NomeArquivoXML);
                    if(string.IsNullOrWhiteSpace(versaoXML))
                    {
                        versaoXML = "2.01";
                    }

                    break;

                case PadroesNFSe.BETHA:
                    versaoXML = "2.02";

                    if(xmlDoc.DocumentElement.Name.Contains("e:"))
                    {
                        versaoXML = "1.00";
                    }
                    break;

                case PadroesNFSe.SIGCORP_SIGISS:
                    versaoXML = "0.00";
                    break;
            }

            return versaoXML;
        }
    }
}
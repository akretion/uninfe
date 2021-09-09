using NFe.Certificado;
using NFe.Components;
using NFe.Components.BAURU_SP;
using NFe.Components.Conam;
using NFe.Components.Consist;
using NFe.Components.Coplan;
using NFe.Components.EGoverne;
using NFe.Components.EGoverneISS;
using NFe.Components.EL;
using NFe.Components.Elotech;
using NFe.Components.Fiorilli;
using NFe.Components.GeisWeb;
using NFe.Components.GovDigital;
using NFe.Components.Memory;
using NFe.Components.Metropolis;
using NFe.Components.MGM;
using NFe.Components.Pronin;
using NFe.Components.SigCorp;
using NFe.Components.Simple;
using NFe.Components.SimplISS;
using NFe.Components.SystemPro;
using NFe.Components.Tinus;
using NFe.Components.WEBFISCO_TECNOLOGIA;
using NFe.Settings;
using NFe.Validate;
using NFSe.Components;
using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
using static NFe.Components.Security.SOAPSecurity;

namespace NFe.Service.NFSe
{
    public class TaskNFSeCancelar: TaskAbst
    {
        #region Private Fields

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        private DadosPedCanNfse oDadosPedCanNfse;

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        private void EncryptAssinatura()
        {
            ///danasa: 12/2013
            var val = new ValidarXML(NomeArquivoXML, oDadosPedCanNfse.cMunicipio, false);
            val.EncryptAssinatura(NomeArquivoXML);
        }

        /// <summary>
        /// Fazer a leitura do conteúdo do XML de cancelamento de NFS-e e disponibilizar conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedCanNfse(int emp, string arquivoXML)
        {
        }

        #endregion Private Methods

        #region Public Methods

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeCancelar;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML) + Propriedade.ExtRetorno.CanNfse_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedCanNfse = new DadosPedCanNfse(emp);
                PedCanNfse(emp, NomeArquivoXML);
                var padraoNFSe = Functions.PadraoNFSe(oDadosPedCanNfse.cMunicipio);

                switch(padraoNFSe)
                {
                    case PadroesNFSe.PRODATA:
                    case PadroesNFSe.BETHA:
                        ExecuteDLL(emp, oDadosPedCanNfse.cMunicipio, padraoNFSe);
                        break;

                    default:
                        switch(oDadosPedCanNfse.cMunicipio)
                        {
                            #region Municípios do Padrão SIGCORP
                            case 4105508: //Cianorte-PR
                            case 3303203: //Nilópolis-RJ
                            case 3305109: //São João de Meriti-RJ
                            case 3505500: //Barretos-SP
                                ExecuteDLL(emp, oDadosPedCanNfse.cMunicipio, padraoNFSe);
                                break;
                            #endregion

                            default:
                                WebServiceProxy wsProxy = null;
                                object pedCanNfse = null;

                                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                                if(IsUtilizaCompilacaoWs(padraoNFSe))
                                {
                                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis, padraoNFSe, oDadosPedCanNfse.cMunicipio);
                                    if(wsProxy != null)
                                    {
                                        pedCanNfse = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                                    }
                                }
                                var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis, padraoNFSe, Servico);

                                var cabecMsg = "";
                                switch(padraoNFSe)
                                {
                                    case PadroesNFSe.IPM:

                                        //código da cidade da receita federal, este arquivo pode ser encontrado em ~\uninfe\doc\Codigos_Cidades_Receita_Federal.xls</para>
                                        //O código da cidade está hardcoded pois ainda está sendo usado apenas para campo mourão
                                        var ipm = new IPM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                          Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                          Empresas.Configuracoes[emp].UsuarioWS,
                                                          Empresas.Configuracoes[emp].SenhaWS,
                                                          oDadosPedCanNfse.cMunicipio);

                                        if(ConfiguracaoApp.Proxy)
                                        {
                                            ipm.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                        }

                                        if(oDadosPedCanNfse.cMunicipio == 4215000)
                                        {
                                            var adIPM = new AssinaturaDigital();
                                            //adIPM.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);
                                            adIPM.Assinar(NomeArquivoXML, "nfse", "nfse", Empresas.Configuracoes[emp].X509Certificado, emp);
                                        }

                                        ipm.CancelarNfse(NomeArquivoXML);

                                        break;

                                    case PadroesNFSe.ABASE:
                                        cabecMsg = "<cabecalho xmlns=\"http://nfse.abase.com.br/nfse.xsd\" versao =\"1.00\"><versaoDados>1.00</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.GINFES:
                                        cabecMsg = ""; //Cancelamento ainda tá na versão 2.0 então não tem o cabecMsg
                                        break;

                                    case PadroesNFSe.ABACO:
                                    case PadroesNFSe.CANOAS_RS:
                                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.ABACO_204:
                                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"201001\"><versaoDados>2.04</versaoDados></cabecalho>";
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

                                    case PadroesNFSe.PAULISTANA:
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        if(oDadosPedCanNfse.tpAmb == 1)
                                        {
                                            pedCanNfse = new NFe.Components.PSaoPauloSP.LoteNFe();
                                        }
                                        else
                                        {
                                            throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                                        }

                                        EncryptAssinatura();
                                        break;

                                    case PadroesNFSe.DSF:
                                        if(oDadosPedCanNfse.cMunicipio == 3549904)
                                        {
                                            cabecMsg = "<cabecalho versao=\"3\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>3</versaoDados></cabecalho>";
                                        }
                                        else
                                        {
                                            EncryptAssinatura();
                                        }
                                        break;

                                    case PadroesNFSe.TECNOSISTEMAS:
                                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.FINTEL:
                                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.SYSTEMPRO:
                                        var syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado, oDadosPedCanNfse.cMunicipio);
                                        var ad = new AssinaturaDigital();
                                        ad.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        syspro.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SIGCORP_SIGISS:
                                        var sigcorp = new SigCorp((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        oDadosPedCanNfse.cMunicipio);
                                        sigcorp.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SIGCORP_SIGISS_203:
                                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.METROPOLIS:

                                        #region METROPOLIS

                                        var metropolis = new Metropolis((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                      oDadosPedCanNfse.cMunicipio,
                                                                      ConfiguracaoApp.ProxyUsuario,
                                                                      ConfiguracaoApp.ProxySenha,
                                                                      ConfiguracaoApp.ProxyServidor,
                                                                      Empresas.Configuracoes[emp].X509Certificado);

                                        var metropolisdig = new AssinaturaDigital();
                                        metropolisdig.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        metropolis.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion METROPOLIS

                                    case PadroesNFSe.FIORILLI:
                                        var fiorilli = new Fiorilli((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        oDadosPedCanNfse.cMunicipio,
                                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                                        ConfiguracaoApp.ProxyUsuario,
                                                                        ConfiguracaoApp.ProxySenha,
                                                                        ConfiguracaoApp.ProxyServidor,
                                                                        Empresas.Configuracoes[emp].X509Certificado);

                                        var ass = new AssinaturaDigital();
                                        ass.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        fiorilli.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SIMPLISS:
                                        var simpliss = new SimplISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                oDadosPedCanNfse.cMunicipio,
                                                                Empresas.Configuracoes[emp].UsuarioWS,
                                                                Empresas.Configuracoes[emp].SenhaWS,
                                                                ConfiguracaoApp.ProxyUsuario,
                                                                ConfiguracaoApp.ProxySenha,
                                                                ConfiguracaoApp.ProxyServidor);

                                        var sing = new AssinaturaDigital();
                                        sing.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        simpliss.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.CONAM:
                                        var conam = new Conam((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                oDadosPedCanNfse.cMunicipio,
                                                                Empresas.Configuracoes[emp].UsuarioWS,
                                                                Empresas.Configuracoes[emp].SenhaWS);

                                        conam.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.EGOVERNE:

                                        #region E-Governe

                                        var egoverne = new EGoverne((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedCanNfse.cMunicipio,
                                        ConfiguracaoApp.ProxyUsuario,
                                        ConfiguracaoApp.ProxySenha,
                                        ConfiguracaoApp.ProxyServidor,
                                        Empresas.Configuracoes[emp].X509Certificado);

                                        var assegov = new AssinaturaDigital();
                                        assegov.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        egoverne.CancelarNfse(NomeArquivoXML);

                                        #endregion E-Governe

                                        break;

                                    case PadroesNFSe.COPLAN:

                                        #region Coplan

                                        var coplan = new Coplan((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedCanNfse.cMunicipio,
                                            ConfiguracaoApp.ProxyUsuario,
                                            ConfiguracaoApp.ProxySenha,
                                            ConfiguracaoApp.ProxyServidor,
                                            Empresas.Configuracoes[emp].X509Certificado);

                                        var assCoplan = new AssinaturaDigital();
                                        assCoplan.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        coplan.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion Coplan

                                    case PadroesNFSe.EL:

                                        #region E&L

                                        var el = new EL((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedCanNfse.cMunicipio,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyUsuario : ""),
                                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxySenha : ""),
                                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyServidor : ""));

                                        el.CancelarNfse(NomeArquivoXML);

                                        #endregion E&L

                                        break;

                                    case PadroesNFSe.GOVDIGITAL:
                                        var govdig = new GovDigital((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                            Empresas.Configuracoes[emp].X509Certificado,
                                                                            oDadosPedCanNfse.cMunicipio,
                                                                            ConfiguracaoApp.ProxyUsuario,
                                                                            ConfiguracaoApp.ProxySenha,
                                                                            ConfiguracaoApp.ProxyServidor);

                                        var adgovdig = new AssinaturaDigital();
                                        adgovdig.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        govdig.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.BSITBR:
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        if(oDadosPedCanNfse.tpAmb == 1)
                                        {
                                            switch(oDadosPedCanNfse.cMunicipio)
                                            {
                                                case 5211800:
                                                    pedCanNfse = new Components.PJaraguaGO.nfseWS();
                                                    break;

                                                case 5220454:
                                                    pedCanNfse = new Components.PSenadorCanedoGO.nfseWS();
                                                    break;


                                                case 3507506:
                                                    pedCanNfse = new Components.PBotucatuSP.nfseWS();
                                                    break;

                                                case 5211909:
                                                    pedCanNfse = new Components.PJataiGO.nfseWS();
                                                    break;

                                                case 5220603:
                                                    pedCanNfse = new Components.PSilvaniaGO.nfseWS();
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Este município não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                                        }

                                        break;

                                    case PadroesNFSe.EQUIPLANO:
                                        cabecMsg = "1";
                                        break;

                                    case PadroesNFSe.RLZ_INFORMATICA_02:
                                        if(oDadosPedCanNfse.cMunicipio == 5107958)
                                        {
                                            cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                                        }

                                        break;

                                    case PadroesNFSe.PORTALFACIL_ACTCON_202:
                                        if(oDadosPedCanNfse.cMunicipio != 3169901)
                                        {
                                            cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                                        }

                                        break;

                                    case PadroesNFSe.PORTALFACIL_ACTCON:
                                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.MGM:

                                        #region MGM

                                        var mgm = new MGM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                           oDadosPedCanNfse.cMunicipio,
                                                           Empresas.Configuracoes[emp].UsuarioWS,
                                                           Empresas.Configuracoes[emp].SenhaWS);
                                        mgm.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion MGM

                                    case PadroesNFSe.NATALENSE:
                                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1\" xmlns=\"http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd\"><versaoDados>1</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.CONSIST:

                                        #region Consist

                                        var consist = new Consist((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedCanNfse.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        ConfiguracaoApp.ProxyUsuario,
                                        ConfiguracaoApp.ProxySenha,
                                        ConfiguracaoApp.ProxyServidor);

                                        consist.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion Consist

                                    case PadroesNFSe.MEMORY:

                                        #region Memory

                                        var memory = new Memory((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedCanNfse.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        ConfiguracaoApp.ProxyUsuario,
                                        ConfiguracaoApp.ProxySenha,
                                        ConfiguracaoApp.ProxyServidor);

                                        memory.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion Memory

                                    case PadroesNFSe.CAMACARI_BA:
                                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados><versao>2.01</versao></cabecalho>";
                                        break;

                                    case PadroesNFSe.NA_INFORMATICA:
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        //if (oDadosPedCanNfse.tpAmb == 1)
                                        //    pedCanNfse = new Components.PCorumbaMS.NfseWSService();
                                        //else
                                        //    pedCanNfse = new Components.HCorumbaMS.NfseWSService();

                                        break;

                                    case PadroesNFSe.PRONIN:
                                        if(oDadosPedCanNfse.cMunicipio == 3131703 ||
                                            oDadosPedCanNfse.cMunicipio == 4303004 ||
                                            oDadosPedCanNfse.cMunicipio == 3556602 ||
                                            oDadosPedCanNfse.cMunicipio == 3512803 ||
                                            oDadosPedCanNfse.cMunicipio == 4323002 ||
                                            oDadosPedCanNfse.cMunicipio == 3505807 ||
                                            oDadosPedCanNfse.cMunicipio == 3530300 ||
                                            oDadosPedCanNfse.cMunicipio == 4308904 ||
                                            oDadosPedCanNfse.cMunicipio == 4118501 ||
                                            oDadosPedCanNfse.cMunicipio == 3554300 ||
                                            oDadosPedCanNfse.cMunicipio == 3542404 ||
                                            oDadosPedCanNfse.cMunicipio == 5005707 ||
                                            oDadosPedCanNfse.cMunicipio == 4314423 ||
                                            oDadosPedCanNfse.cMunicipio == 3511102 ||
                                            oDadosPedCanNfse.cMunicipio == 3535804 ||
                                            oDadosPedCanNfse.cMunicipio == 4306932 ||
                                            oDadosPedCanNfse.cMunicipio == 4322400 ||
                                            oDadosPedCanNfse.cMunicipio == 4302808 ||
                                            oDadosPedCanNfse.cMunicipio == 3501301 ||
                                            oDadosPedCanNfse.cMunicipio == 4300109 ||
                                            oDadosPedCanNfse.cMunicipio == 4124053 ||
                                            oDadosPedCanNfse.cMunicipio == 4101408 ||
                                            oDadosPedCanNfse.cMunicipio == 3550407 ||
                                            oDadosPedCanNfse.cMunicipio == 4310207 ||
                                            oDadosPedCanNfse.cMunicipio == 1502400 ||
                                            oDadosPedCanNfse.cMunicipio == 3550803)
                                        {
                                            var pronin = new Pronin((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                oDadosPedCanNfse.cMunicipio,
                                                ConfiguracaoApp.ProxyUsuario,
                                                ConfiguracaoApp.ProxySenha,
                                                ConfiguracaoApp.ProxyServidor,
                                                Empresas.Configuracoes[emp].X509Certificado);

                                            var assPronin = new AssinaturaDigital();
                                            assPronin.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                            pronin.CancelarNfse(NomeArquivoXML);
                                        }
                                        break;

                                    case PadroesNFSe.EGOVERNEISS:

                                        #region EGoverne ISS

                                        var egoverneiss = new EGoverneISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedCanNfse.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        ConfiguracaoApp.ProxyUsuario,
                                        ConfiguracaoApp.ProxySenha,
                                        ConfiguracaoApp.ProxyServidor);

                                        egoverneiss.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion EGoverne ISS

                                    case PadroesNFSe.BAURU_SP:
                                        var bauru_SP = new Bauru_SP((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        oDadosPedCanNfse.cMunicipio);
                                        bauru_SP.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.SMARAPD:
                                        if(Empresas.Configuracoes[emp].UnidadeFederativaCodigo == 3201308) //Município de Cariacica-ES
                                        {
                                            throw new Exception("Município de Cariacica-ES não permite cancelamento de NFS-e via webservice.");
                                        }
                                        break;

                                    case PadroesNFSe.SMARAPD_204:
                                        cabecMsg = "<cabecalho versao=\"2.04\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.04</versaoDados></cabecalho>";
                                        break;

                                    #region Tinus

                                    case PadroesNFSe.TINUS:
                                        var tinus = new Tinus((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedCanNfse.cMunicipio,
                                            ConfiguracaoApp.ProxyUsuario,
                                            ConfiguracaoApp.ProxySenha,
                                            ConfiguracaoApp.ProxyServidor,
                                            Empresas.Configuracoes[emp].X509Certificado);

                                        var tinusAss = new AssinaturaDigital();
                                        tinusAss.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        tinus.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion Tinus

                                    case PadroesNFSe.GEISWEB:
                                        var geisWeb = new GeisWeb((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            oDadosPedCanNfse.cMunicipio,
                                            ConfiguracaoApp.ProxyUsuario,
                                            ConfiguracaoApp.ProxySenha,
                                            ConfiguracaoApp.ProxyServidor,
                                            Empresas.Configuracoes[emp].X509Certificado);

                                        geisWeb.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #region SH3

                                    case PadroesNFSe.SH3:
                                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                                        break;

                                    #endregion SH3

                                    #region SOFTPLAN

                                    case PadroesNFSe.SOFTPLAN:
                                        var softplan = new Components.SOFTPLAN.SOFTPLAN((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                          Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                          Empresas.Configuracoes[emp].TokenNFse,
                                                          Empresas.Configuracoes[emp].TokenNFSeExpire,
                                                          Empresas.Configuracoes[emp].UsuarioWS,
                                                          Empresas.Configuracoes[emp].SenhaWS,
                                                          Empresas.Configuracoes[emp].ClientID,
                                                          Empresas.Configuracoes[emp].ClientSecret);

                                        var softplanAssinatura = new AssinaturaDigital();
                                        softplanAssinatura.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        // Validar o Arquivo XML
                                        var softplanValidar = new ValidarXML(NomeArquivoXML, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                                        var validacao = softplanValidar.ValidarArqXML(NomeArquivoXML);
                                        if(validacao != "")
                                        {
                                            throw new Exception(validacao);
                                        }

                                        if(ConfiguracaoApp.Proxy)
                                        {
                                            softplan.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                        }

                                        var softplanAss = new AssinaturaDigital();
                                        softplanAss.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio, AlgorithmType.Sha256);

                                        softplan.CancelarNfse(NomeArquivoXML);

                                        if(Empresas.Configuracoes[emp].TokenNFse != softplan.Token)
                                        {
                                            Empresas.Configuracoes[emp].SalvarConfiguracoesNFSeSoftplan(softplan.Usuario,
                                                                                                        softplan.Senha,
                                                                                                        softplan.ClientID,
                                                                                                        softplan.ClientSecret,
                                                                                                        softplan.Token,
                                                                                                        softplan.TokenExpire,
                                                                                                        Empresas.Configuracoes[emp].CNPJ);
                                        }

                                        break;

                                    #endregion SOFTPLAN

                                    #region CENTI

                                    case PadroesNFSe.CENTI:
                                        var centi = new Components.CENTI.CENTI((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                                        Empresas.Configuracoes[emp].SenhaWS);

                                        var centiAssinatura = new AssinaturaDigital();
                                        centiAssinatura.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        // Validar o Arquivo XML
                                        var centiValidar = new ValidarXML(NomeArquivoXML, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                                        var validacaoCenti = centiValidar.ValidarArqXML(NomeArquivoXML);
                                        if(validacaoCenti != "")
                                        {
                                            throw new Exception(validacaoCenti);
                                        }

                                        if(ConfiguracaoApp.Proxy)
                                        {
                                            centi.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                        }
                                        centi.CancelarNfse(NomeArquivoXML);
                                        break;

                                    #endregion

                                    #region AGILI
                                    case PadroesNFSe.AGILI:
                                        var agili = new Components.AGILI.AGILI((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                          Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                          Empresas.Configuracoes[emp].TokenNFse,
                                                          Empresas.Configuracoes[emp].TokenNFSeExpire,
                                                          Empresas.Configuracoes[emp].UsuarioWS,
                                                          Empresas.Configuracoes[emp].SenhaWS,
                                                          Empresas.Configuracoes[emp].ClientID,
                                                          Empresas.Configuracoes[emp].ClientSecret);

                                        var agiliAssinatura = new AssinaturaDigital();
                                        agiliAssinatura.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                        // Validar o Arquivo XML
                                        var AgiliValidar = new ValidarXML(NomeArquivoXML, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                                        var validacaoAgili = AgiliValidar.ValidarArqXML(NomeArquivoXML);
                                        if(validacaoAgili != "")
                                        {
                                            throw new Exception(validacaoAgili);
                                        }

                                        if(ConfiguracaoApp.Proxy)
                                        {
                                            agili.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                                        }

                                        var AgiliAss = new AssinaturaDigital();
                                        AgiliAss.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio, AlgorithmType.Sha256);

                                        agili.CancelarNfse(NomeArquivoXML);

                                        if(Empresas.Configuracoes[emp].TokenNFse != agili.Token)
                                        {
                                            Empresas.Configuracoes[emp].SalvarConfiguracoesNFSeSoftplan(agili.Usuario,
                                                                                                        agili.Senha,
                                                                                                        agili.ClientID,
                                                                                                        agili.ClientSecret,
                                                                                                        agili.Token,
                                                                                                        agili.TokenExpire,
                                                                                                        Empresas.Configuracoes[emp].CNPJ);
                                        }

                                        break;

                                    #endregion AGILI

                                    case PadroesNFSe.INTERSOL:
                                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><p:cabecalho versao=\"1\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:p=\"http://ws.speedgov.com.br/cabecalho_v1.xsd\" xmlns:p1=\"http://ws.speedgov.com.br/tipos_v1.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://ws.speedgov.com.br/cabecalho_v1.xsd cabecalho_v1.xsd \"><versaoDados>1</versaoDados></p:cabecalho>";
                                        break;

                                    case PadroesNFSe.JOINVILLE_SC:
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        if(oDadosPedCanNfse.tpAmb == 2)
                                        {
                                            pedCanNfse = new Components.HJoinvilleSC.Servicos();
                                        }
                                        else
                                        {
                                            pedCanNfse = new Components.PJoinvilleSC.Servicos();
                                        }

                                        break;

                                    case PadroesNFSe.AVMB_ASTEN:
                                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        if(oDadosPedCanNfse.tpAmb == 2)
                                        {
                                            pedCanNfse = new Components.HPelotasRS.INfseservice();
                                        }
                                        else
                                        {
                                            pedCanNfse = new Components.PPelotasRS.INfseservice();
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

                                        pedCanNfse = oDadosPedCanNfse.tpAmb == 1 ?
                                                        new Components.PAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://demo.saatri.com.br/servicos/nfse.svc")) :
                                                        new Components.HAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://homologa-demo.saatri.com.br/servicos/nfse.svc")) as object;

                                        SignUsingCredentials(emp, pedCanNfse);
                                        break;

                                    case PadroesNFSe.PUBLIC_SOFT:
                                        break;

                                    case PadroesNFSe.SIMPLE:

                                        var simple = new Simple((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                        oDadosPedCanNfse.cMunicipio,
                                                                        ConfiguracaoApp.ProxyUsuario,
                                                                        ConfiguracaoApp.ProxySenha,
                                                                        ConfiguracaoApp.ProxyServidor,
                                                                        Empresas.Configuracoes[emp].X509Certificado);

                                        simple.CancelarNfse(NomeArquivoXML);
                                        break;


                                    case PadroesNFSe.SISPMJP:
                                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" ><versaoDados>2.02</versaoDados></cabecalho>";
                                        break;

                                    case PadroesNFSe.D2TI:
                                        cabecMsg = "<cabecalhoCancelamentoNfseLote xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.ctaconsult.com/nfse\"><versao>1.00</versao><ambiente>" + Empresas.Configuracoes[emp].AmbienteCodigo + "</ambiente></cabecalhoCancelamentoNfseLote>";
                                        break;

                                    case PadroesNFSe.IIBRASIL:
                                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.04\"><versaoDados>2.04</versaoDados></cabecalho>";
                                        //wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                                        //if (oDadosPedCanNfse.tpAmb == 2)
                                        //{
                                        //    pedCanNfse = new Components.HLimeiraSP.NfseWSService();
                                        //}
                                        //else
                                        //{
                                        //    throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                                        //}

                                        break;

                                    case PadroesNFSe.WEBFISCO_TECNOLOGIA:
                                        var webTecnologia = new WEBFISCO_TECNOLOGIA((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                           oDadosPedCanNfse.cMunicipio,
                                                           Empresas.Configuracoes[emp].UsuarioWS,
                                                           Empresas.Configuracoes[emp].SenhaWS);
                                        webTecnologia.CancelarNfse(NomeArquivoXML);
                                        break;

                                    case PadroesNFSe.ELOTECH:
                                        var elotech = new Elotech((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                oDadosPedCanNfse.cMunicipio,
                                                ConfiguracaoApp.ProxyUsuario,
                                                ConfiguracaoApp.ProxySenha,
                                                ConfiguracaoApp.ProxyServidor,
                                                Empresas.Configuracoes[emp].X509Certificado);

                                        elotech.CancelarNfse(NomeArquivoXML);
                                        break;
                                }

                                if(IsInvocar(padraoNFSe, Servico, Empresas.Configuracoes[emp].UnidadeFederativaCodigo))
                                {
                                    //Assinar o XML
                                    var ad = new AssinaturaDigital();

                                    ad.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                                    //Invocar o método que envia o XML para o SEFAZ
                                    oInvocarObj.InvocarNFSe(wsProxy, pedCanNfse, NomeMetodoWS(Servico, oDadosPedCanNfse.cMunicipio), cabecMsg, this,
                                                             Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,   //"-ped-cannfse",
                                                             Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML,   //"-cannfse",
                                                             padraoNFSe, Servico, securityProtocolType);

                                    /// grava o arquivo no FTP
                                    var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                      Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
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
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML, Propriedade.ExtRetorno.CanNfse_ERR, ex);
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

        #endregion Public Methods

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

            var finalArqEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML;
            var finalArqRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML;
            var versaoXML = DefinirVersaoXML(municipio, conteudoXML, padraoNFSe);

            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, finalArqEnvio) + Functions.ExtractExtension(finalArqRetorno) + ".err");

            var configuracao = new Unimake.Business.DFe.Servicos.Configuracao
            {
                TipoDFe = Unimake.Business.DFe.Servicos.TipoDFe.NFSe,
                CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado,
                TipoAmbiente = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                CodigoMunicipio = municipio,
                Servico = Unimake.Business.DFe.Servicos.Servico.NFSeCancelarNfse,
                SchemaVersao = versaoXML
            };

            var cancelarNfse = new Unimake.Business.DFe.Servicos.NFSe.CancelarNfse(conteudoXML, configuracao);
            cancelarNfse.Executar();

            vStrXmlRetorno = cancelarNfse.RetornoWSString;

            XmlRetorno(finalArqEnvio, finalArqRetorno);

            /// grava o arquivo no FTP
            var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);

            if(File.Exists(filenameFTP))
            {
                new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
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
                    versaoXML = "2.01";
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
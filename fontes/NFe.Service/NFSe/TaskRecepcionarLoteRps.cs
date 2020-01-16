using NFe.Certificado;
using NFe.Components;
using NFe.Components.BAURU_SP;
using NFe.Components.Conam;
using NFe.Components.Consist;
using NFe.Components.Coplan;
using NFe.Components.EGoverne;
using NFe.Components.EGoverneISS;
using NFe.Components.EL;
using NFe.Components.Fiorilli;
using NFe.Components.GovDigital;
using NFe.Components.Memory;
using NFe.Components.Metropolis;
using NFe.Components.MGM;
using NFe.Components.Pronin;
using NFe.Components.RLZ_INFORMATICA;
using NFe.Components.SigCorp;
using NFe.Components.SimplISS;
using NFe.Components.Simple;
using NFe.Components.SystemPro;
using NFe.Components.Tinus;
using NFe.Settings;
using NFe.Validate;
using NFSe.Components;
using System;
using System.IO;
using NFe.Components.WEBFISCO_TECNOLOGIA;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Text.RegularExpressions;
using NFe.Components.Elotech;
#if _fw46
using System.ServiceModel;
using static NFe.Components.Security.SOAPSecurity;
#endif

namespace NFe.Service.NFSe
{
    public class TaskNFSeRecepcionarLoteRps : TaskAbst
    {
        #region Objeto com os dados do XML de lote rps

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do lote rps
        /// </summary>
        private DadosEnvLoteRps oDadosEnvLoteRps;

        #endregion Objeto com os dados do XML de lote rps

        public TaskNFSeRecepcionarLoteRps(string arquivo)
        {
            Servico = Servicos.NFSeRecepcionarLoteRps;

            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeRecepcionarLoteRps;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML) + Propriedade.ExtRetorno.RetEnvLoteRps_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosEnvLoteRps = new DadosEnvLoteRps(emp);

                EnvLoteRps(emp, NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosEnvLoteRps.cMunicipio);

                WebServiceProxy wsProxy = null;
                object envLoteRps = null;

                if (IsUtilizaCompilacaoWs(padraoNFSe, Servico, oDadosEnvLoteRps.cMunicipio))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, oDadosEnvLoteRps.cMunicipio);
                    if (wsProxy != null)
                        envLoteRps = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, Servico);

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.IPM:

                        //código da cidade da receita federal, este arquivo pode ser encontrado em ~\uninfe\doc\Codigos_Cidades_Receita_Federal.xls</para>
                        //O código da cidade está hardcoded pois ainda está sendo usado apenas para campo mourão
                        IPM ipm = new IPM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                          Empresas.Configuracoes[emp].PastaXmlRetorno,
                                          Empresas.Configuracoes[emp].UsuarioWS,
                                          Empresas.Configuracoes[emp].SenhaWS,
                                          oDadosEnvLoteRps.cMunicipio);

                        if (ConfiguracaoApp.Proxy)
                            ipm.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);

                        ipm.EmiteNF(NomeArquivoXML);
                        break;

                    case PadroesNFSe.GINFES:
                        switch (oDadosEnvLoteRps.cMunicipio)
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

                        string versao = Functions.GetAttributeXML("LoteRps", "versao", NomeArquivoXML);
                        if (versao.Equals("2.02"))
                        {
                            padraoNFSe = PadroesNFSe.BETHA202;
                            Components.Betha.NewVersion.Betha betha = new Components.Betha.NewVersion.Betha((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                oDadosEnvLoteRps.cMunicipio,
                                Empresas.Configuracoes[emp].UsuarioWS,
                                Empresas.Configuracoes[emp].SenhaWS,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor);

                            AssinaturaDigital signbetha = new AssinaturaDigital();
                            signbetha.Assinar(NomeArquivoXML, emp, 202);

                            if (GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.BETHA202) == Servicos.NFSeRecepcionarLoteRpsSincrono)
                                betha.EmiteNFSincrono(NomeArquivoXML);
                            else
                                betha.EmiteNF(NomeArquivoXML);
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
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.BHISS);
                        break;

                    case PadroesNFSe.SH3:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.SH3);
                        break;

                    case PadroesNFSe.WEBISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.WEBISS_202:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.WEBISS_202);

                        cabecMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.PAULISTANA:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        //if (oDadosEnvLoteRps.tpAmb == 1)
                        envLoteRps = new Components.PSaoPauloSP.LoteNFe();
                        //else
                        //    throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");

                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.NA_INFORMATICA:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.VVISS);

                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        //if (oDadosEnvLoteRps.tpAmb == 1)
                        //    envLoteRps = new Components.PCorumbaMS.NfseWSService();
                        //else
                        //    envLoteRps = new Components.HCorumbaMS.NfseWSService();

                        break;

                    case PadroesNFSe.BSITBR:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.BSITBR);

                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosEnvLoteRps.tpAmb == 1)
                        {
                            switch (oDadosEnvLoteRps.cMunicipio)
                            {
                                case 5211800:
                                    envLoteRps = new Components.PJaraguaGO.nfseWS();
                                    break;

                                case 5220454:
                                    envLoteRps = new Components.PSenadorCanedoGO.nfseWS();
                                    break;

                                case 3507506:
                                    envLoteRps = new Components.PBotucatuSP.nfseWS();
                                    break;
                            }
                        }
                        else
                            throw new Exception("Este município não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                        break;

                    case PadroesNFSe.PORTOVELHENSE:
                        cabecMsg = "<cabecalho versao=\"2.00\" xmlns:ns2=\"http://www.w3.org/2000/09/xmldsig#\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.DSF:
                        if (oDadosEnvLoteRps.cMunicipio == 3549904)
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
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://iss.irati.pr.gov.br/Arquivos/nfseV202.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.FINTEL);
                        break;

                    case PadroesNFSe.PORTALFACIL_ACTCON:
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.PORTALFACIL_ACTCON);
                        break;

                    case PadroesNFSe.PORTALFACIL_ACTCON_202:
                        if (oDadosEnvLoteRps.cMunicipio != 3169901)
                            cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.PORTALFACIL_ACTCON_202);
                        break;

                    case PadroesNFSe.SYSTEMPRO:

                        #region SystemPro

                        SystemPro syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado, oDadosEnvLoteRps.cMunicipio);
                        AssinaturaDigital ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);
                        syspro.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion SystemPro

                    case PadroesNFSe.SIGCORP_SIGISS:
                        SigCorp sigcorp = new SigCorp((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosEnvLoteRps.cMunicipio);
                        sigcorp.EmiteNF(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SIGCORP_SIGISS_203:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.SIGCORP_SIGISS_203);

                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FIORILLI:

                        #region Fiorilli

                        Fiorilli fiorilli = new Fiorilli((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                         Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                         oDadosEnvLoteRps.cMunicipio,
                                                         Empresas.Configuracoes[emp].UsuarioWS,
                                                         Empresas.Configuracoes[emp].SenhaWS,
                                                         ConfiguracaoApp.ProxyUsuario,
                                                         ConfiguracaoApp.ProxySenha,
                                                         ConfiguracaoApp.ProxyServidor,
                                                         Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital ass = new AssinaturaDigital();
                        ass.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        // Validar o Arquivo XML
                        ValidarXML validar = new ValidarXML(NomeArquivoXML, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                        string resValidacao = validar.ValidarArqXML(NomeArquivoXML);
                        if (resValidacao != "")
                        {
                            throw new Exception(resValidacao);
                        }

                        fiorilli.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion Fiorilli

                    case PadroesNFSe.SIMPLISS:

                        #region Simpliss

                        SimplISS simpliss = new SimplISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital sign = new AssinaturaDigital();
                        sign.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        simpliss.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion Simpliss

                    case PadroesNFSe.CONAM:

                        #region Conam

                        Conam conam = new Conam((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS);

                        conam.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion Conam

                    case PadroesNFSe.RLZ_INFORMATICA:

                        #region RLZ

                        Rlz_Informatica rlz = new Rlz_Informatica((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio);

                        rlz.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion RLZ

                    case PadroesNFSe.EGOVERNE:

                        #region E-Governe

                        EGoverne egoverne = new EGoverne((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosEnvLoteRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assEGovoverne = new AssinaturaDigital();
                        assEGovoverne.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        egoverne.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion E-Governe

                    case PadroesNFSe.EL:

                        #region E&L

                        EL el = new EL((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosEnvLoteRps.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyUsuario : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxySenha : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyServidor : ""));

                        el.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion E&L

                    case PadroesNFSe.GOVDIGITAL:

                        #region GOV-Digital

                        GovDigital govdig = new GovDigital((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                            Empresas.Configuracoes[emp].X509Certificado,
                                                            oDadosEnvLoteRps.cMunicipio,
                                                            ConfiguracaoApp.ProxyUsuario,
                                                            ConfiguracaoApp.ProxySenha,
                                                            ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital adgovdig = new AssinaturaDigital();
                        adgovdig.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        govdig.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion GOV-Digital

                    case PadroesNFSe.EQUIPLANO:
                        cabecMsg = "1";
                        break;

                    case PadroesNFSe.NATALENSE:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1\" xmlns=\"http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd\"><versaoDados>1</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.PRODATA:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.PRODATA);
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.VVISS:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.VVISS);
                        break;

                    case PadroesNFSe.METROPOLIS:

                        #region METROPOLIS

                        Metropolis metropolis = new Metropolis((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      oDadosEnvLoteRps.cMunicipio,
                                                      ConfiguracaoApp.ProxyUsuario,
                                                      ConfiguracaoApp.ProxySenha,
                                                      ConfiguracaoApp.ProxyServidor,
                                                      Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital metropolisdig = new AssinaturaDigital();
                        metropolisdig.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        metropolis.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion METROPOLIS

                    case PadroesNFSe.MGM:

                        #region MGM

                        MGM mgm = new MGM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                           oDadosEnvLoteRps.cMunicipio,
                                           Empresas.Configuracoes[emp].UsuarioWS,
                                           Empresas.Configuracoes[emp].SenhaWS);
                        mgm.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion MGM

                    case PadroesNFSe.CONSIST:

                        #region Consist

                        Consist consist = new Consist((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        consist.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion Consist

                    case PadroesNFSe.NOTAINTELIGENTE:

                        #region Nota Inteligente

                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosEnvLoteRps.tpAmb == 1)
                        {
                            //envLoteRps = new NFe.Components.PClaudioMG.api_portClient();
                        }
                        else
                        {
                            throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                        }

                        #endregion Nota Inteligente

                        break;

                    case PadroesNFSe.COPLAN:

                        #region Coplan

                        Coplan coplan = new Coplan((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosEnvLoteRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assCoplan = new AssinaturaDigital();
                        assCoplan.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        coplan.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion Coplan

                    case PadroesNFSe.MEMORY:

                        #region Memory

                        Memory memory = new Memory((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital sigMem = new AssinaturaDigital();
                        sigMem.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        memory.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion Memory

                    case PadroesNFSe.CAMACARI_BA:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.CAMACARI_BA);

                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados><versao>2.01</versao></cabecalho>";
                        break;

                    case PadroesNFSe.CARIOCA:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.CARIOCA);
                        break;

                    case PadroesNFSe.PRONIN:
                        if (oDadosEnvLoteRps.cMunicipio == 4109401 ||
                            oDadosEnvLoteRps.cMunicipio == 3131703 ||
                            oDadosEnvLoteRps.cMunicipio == 4303004 ||
                            oDadosEnvLoteRps.cMunicipio == 3556602 ||
                            oDadosEnvLoteRps.cMunicipio == 3512803 ||
                            oDadosEnvLoteRps.cMunicipio == 4323002 ||
                            oDadosEnvLoteRps.cMunicipio == 3505807 ||
                            oDadosEnvLoteRps.cMunicipio == 3530300 ||
                            oDadosEnvLoteRps.cMunicipio == 4308904 ||
                            oDadosEnvLoteRps.cMunicipio == 4118501 ||
                            oDadosEnvLoteRps.cMunicipio == 3554300 ||
                            oDadosEnvLoteRps.cMunicipio == 3542404 ||
                            oDadosEnvLoteRps.cMunicipio == 5005707 ||
                            oDadosEnvLoteRps.cMunicipio == 4314423 ||
                            oDadosEnvLoteRps.cMunicipio == 3511102 ||
                            oDadosEnvLoteRps.cMunicipio == 3535804 ||
                            oDadosEnvLoteRps.cMunicipio == 4306932 ||
                            oDadosEnvLoteRps.cMunicipio == 4322400 ||
                            oDadosEnvLoteRps.cMunicipio == 4302808 ||
							oDadosEnvLoteRps.cMunicipio == 3501301)
                        {
                            Pronin pronin = new Pronin((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                oDadosEnvLoteRps.cMunicipio,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor,
                                Empresas.Configuracoes[emp].X509Certificado);

                            AssinaturaDigital assPronin = new AssinaturaDigital();
                            assPronin.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                            pronin.EmiteNF(NomeArquivoXML);
                        }
                        break;

                    case PadroesNFSe.EGOVERNEISS:

                        #region EGoverne ISS

                        EGoverneISS egoverneiss = new EGoverneISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        egoverneiss.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion EGoverne ISS

                    case PadroesNFSe.SUPERNOVA:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.SUPERNOVA);
                        break;

                    case PadroesNFSe.MARINGA_PR:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.MARINGA_PR);
                        break;

                    case PadroesNFSe.BAURU_SP:

                        #region BAURU_SP

                        Bauru_SP bauru_SP = new Bauru_SP((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosEnvLoteRps.cMunicipio);
                        bauru_SP.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion BAURU_SP

                    #region Tinus

                    case PadroesNFSe.TINUS:
                        Tinus tinus = new Tinus((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosEnvLoteRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital tinusAss = new AssinaturaDigital();
                        tinusAss.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        tinus.EmiteNF(NomeArquivoXML);
                        break;

                    #endregion Tinus

#if _fw46

                    #region SOFTPLAN

                    case PadroesNFSe.SOFTPLAN:
                        Components.SOFTPLAN.SOFTPLAN softplan = new Components.SOFTPLAN.SOFTPLAN((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        Empresas.Configuracoes[emp].TokenNFse,
                                                        Empresas.Configuracoes[emp].TokenNFSeExpire,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        Empresas.Configuracoes[emp].ClientID,
                                                        Empresas.Configuracoes[emp].ClientSecret);

                        AssinaturaDigital softplanAssinatura = new AssinaturaDigital();
                        softplanAssinatura.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        // Validar o Arquivo XML
                        ValidarXML softplanValidar = new ValidarXML(NomeArquivoXML, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                        string validacao = softplanValidar.ValidarArqXML(NomeArquivoXML);
                        if (validacao != "")
                            throw new Exception(validacao);

                        if (ConfiguracaoApp.Proxy)
                            softplan.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);

                        AssinaturaDigital softplanAss = new AssinaturaDigital();
                        softplanAss.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio, AlgorithmType.Sha256);

                        softplan.EmiteNF(NomeArquivoXML);

                        if (Empresas.Configuracoes[emp].TokenNFse != softplan.Token)
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

#endif

                    case PadroesNFSe.INTERSOL:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><p:cabecalho versao=\"1\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:p=\"http://ws.speedgov.com.br/cabecalho_v1.xsd\" xmlns:p1=\"http://ws.speedgov.com.br/tipos_v1.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://ws.speedgov.com.br/cabecalho_v1.xsd cabecalho_v1.xsd \"><versaoDados>1</versaoDados></p:cabecalho>";
                        break;

                    case PadroesNFSe.MANAUS_AM:
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.JOINVILLE_SC:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosEnvLoteRps.tpAmb == 2)
                            envLoteRps = new Components.HJoinvilleSC.Servicos();
                        else
                            envLoteRps = new Components.PJoinvilleSC.Servicos();
                        break;

                    case PadroesNFSe.AVMB_ASTEN:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.AVMB_ASTEN);

                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosEnvLoteRps.tpAmb == 2)
                            envLoteRps = new Components.HPelotasRS.INfseservice();
                        else
                            envLoteRps = new Components.PPelotasRS.INfseservice();
                        break;

                    case PadroesNFSe.EMBRAS:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.EMBRAS);
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.DESENVOLVECIDADE:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.EMBRAS);
                        break;

                    case PadroesNFSe.MODERNIZACAO_PUBLICA:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.MODERNIZACAO_PUBLICA);
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.E_RECEITA:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.E_RECEITA);
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.TIPLAN_203:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.TIPLAN_203);
                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;
#if _fw46
                    case PadroesNFSe.ADM_SISTEMAS:
                        cabecMsg = "<cabecalho versao=\"2.01\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.01</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        envLoteRps = oDadosEnvLoteRps.tpAmb == 1 ?
                                        new Components.PAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://demo.saatri.com.br/servicos/nfse.svc")) :
                                        new Components.HAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://homologa-demo.saatri.com.br/servicos/nfse.svc")) as object;

                        SignUsingCredentials(emp, envLoteRps);
                        break;
#endif

                    case PadroesNFSe.PUBLIC_SOFT:
                        break;

                    case PadroesNFSe.MEGASOFT:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.MEGASOFT);
                        cabecMsg = "<cabecalho versao=\"1.00\" xmlns=\"http://megasoftarrecadanet.com.br/xsd/nfse_v01.xsd\"><versaoDados>1.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SIMPLE:

                        Simple simple = new Simple((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      oDadosEnvLoteRps.cMunicipio,
                                                      ConfiguracaoApp.ProxyUsuario,
                                                      ConfiguracaoApp.ProxySenha,
                                                      ConfiguracaoApp.ProxyServidor,
                                                      Empresas.Configuracoes[emp].X509Certificado);

                        simple.EmiteNF(NomeArquivoXML);
                        break;

                    case PadroesNFSe.INDAIATUBA_SP:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.INDAIATUBA_SP);
                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SISPMJP:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.SISPMJP);
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" ><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SMARAPD_204:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.SMARAPD_204);
                        cabecMsg = "<cabecalho versao=\"2.04\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.04</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.D2TI:
                        cabecMsg = "<cabecalhoNfseLote xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.ctaconsult.com/nfse\"><versao>1.00</versao><ambiente>2</ambiente></cabecalhoNfseLote>";
                        break;

                    case PadroesNFSe.IIBRASIL:
                        Servico = Servicos.NFSeGerarNfse;
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.04\"><versaoDados>2.04</versaoDados></cabecalho>";
                        GerarTagIntegracao(Empresas.Configuracoes[emp].SenhaWS);
                        break;

                    case PadroesNFSe.WEBFISCO_TECNOLOGIA:
                        WEBFISCO_TECNOLOGIA webTecnologia = new WEBFISCO_TECNOLOGIA((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                           oDadosEnvLoteRps.cMunicipio,
                                           Empresas.Configuracoes[emp].UsuarioWS,
                                           Empresas.Configuracoes[emp].SenhaWS);
                        webTecnologia.EmiteNF(NomeArquivoXML);
                        break;

                    case PadroesNFSe.ELOTECH:
                        Elotech elotech = new Elotech((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosEnvLoteRps.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        elotech.EmiteNF(NomeArquivoXML);
                        break;
                }

                if (IsInvocar(padraoNFSe, Servico, oDadosEnvLoteRps.cMunicipio))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, envLoteRps, NomeMetodoWS(Servico, oDadosEnvLoteRps.cMunicipio), cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML,
                                            padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      Functions.ExtrairNomeArq(NomeArquivoXML,
                                                      Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML) + "\\" + Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, Propriedade.ExtRetorno.RetEnvLoteRps_ERR, ex);
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
        private void GerarTagIntegracao(string token)
       {
            XmlDocument doc = new XmlDocument();
            doc.Load(NomeArquivoXML);
            string conteudoXML, integridade;
            conteudoXML = doc.OuterXml;
            conteudoXML =  Regex.Replace(conteudoXML, "/[^\x20-\x7E]+/", "");
            conteudoXML = Regex.Replace(conteudoXML, "/[ ]+/", "");
            integridade = Criptografia.GerarRSASHA512(conteudoXML + token);

            foreach (object item in ConteudoXML)
            {
                if (typeof(XmlElement) == item.GetType())
                {
                    XmlNode gerarNfseEnvio = (XmlElement)ConteudoXML.GetElementsByTagName("GerarNfseEnvio")[0];
                    XmlNode tagintegridade = ConteudoXML.CreateElement("Integridade", ConteudoXML.DocumentElement.NamespaceURI);

                    tagintegridade.InnerXml = (integridade.Trim()).Trim();

                    gerarNfseEnvio.AppendChild(tagintegridade);

                    conteudoXML = gerarNfseEnvio.OuterXml;

                    break;
                }
            }
            try
            {
                // Atualizar a string do XML já assinada
                string StringXMLAssinado = conteudoXML;

                // Gravar o XML Assinado no HD
                StreamWriter SW_2 = File.CreateText(NomeArquivoXML);
                SW_2.Write(StringXMLAssinado);
                SW_2.Close();
            }
            catch
            {
                throw;
            }
        }

        #region EncryptAssinatura()

        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        private void EncryptAssinatura()
        {
            ///danasa: 12/2013
            Validate.ValidarXML val = new Validate.ValidarXML(NomeArquivoXML, oDadosEnvLoteRps.cMunicipio, false);
            val.EncryptAssinatura(NomeArquivoXML);
        }

        #endregion EncryptAssinatura()

        #region EnvLoteRps()

        /// <summary>
        /// Fazer a leitura do conteúdo do XML de lote rps e disponibiliza o conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void EnvLoteRps(int emp, string arquivoXML)
        {
            //int emp = Empresas.FindEmpresaByThread();

            //XmlDocument doc = new XmlDocument();
            //doc.Load(arquivoXML);

            //XmlNodeList infEnvioList = doc.GetElementsByTagName("EnviarLoteRpsEnvio");

            //foreach (XmlNode infEnvioNode in infEnvioList)
            //{
            //    XmlElement infEnvioElemento = (XmlElement)infEnvioNode;
            //}
        }

        #endregion EnvLoteRps()
    }
}
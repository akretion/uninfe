﻿using NFe.Certificado;
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
using NFe.Components.SigCorp;
using NFe.Components.SimplISS;
using NFe.Components.SystemPro;
using NFe.Components.Tinus;
using NFe.Settings;
using NFe.Validate;
using NFSe.Components;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskNFSeCancelar : TaskAbst
    {
        #region Objeto com os dados do XML de cancelamento de NFS-e

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        private DadosPedCanNfse oDadosPedCanNfse;

        #endregion Objeto com os dados do XML de cancelamento de NFS-e

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeCancelar;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML) + Propriedade.ExtRetorno.CanNfse_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedCanNfse = new DadosPedCanNfse(emp);

                //Ler o XML para pegar parâmetros de envio
                PedCanNfse(emp, NomeArquivoXML);
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosPedCanNfse.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedCanNfse = null;

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                if (IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis, padraoNFSe, oDadosPedCanNfse.cMunicipio);
                    if (wsProxy != null)
                        pedCanNfse = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }
                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis, padraoNFSe, Servico);

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
                                          oDadosPedCanNfse.cMunicipio);

                        if (ConfiguracaoApp.Proxy)
                            ipm.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);

                        ipm.EmiteNF(NomeArquivoXML, true);

                        break;

                    case PadroesNFSe.ABASE:
                        cabecMsg = "<cabecalho xmlns=\"http://nfse.abase.com.br/nfse.xsd\" versao =\"1.00\"><versaoDados>1.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.GINFES:
                        cabecMsg = ""; //Cancelamento ainda tá na versão 2.0 então não tem o cabecMsg
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
                                oDadosPedCanNfse.cMunicipio,
                                Empresas.Configuracoes[emp].UsuarioWS,
                                Empresas.Configuracoes[emp].SenhaWS,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor);

                            AssinaturaDigital signbetha = new AssinaturaDigital();
                            signbetha.Assinar(NomeArquivoXML, emp, 202);

                            betha.CancelarNfse(NomeArquivoXML);
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

                    case PadroesNFSe.BLUMENAU_SC:
                        EncryptAssinatura();
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

                        if (oDadosPedCanNfse.tpAmb == 1)
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
                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://iss.irati.pr.gov.br/Arquivos/nfseV202.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SYSTEMPRO:
                        SystemPro syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado);
                        AssinaturaDigital ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        syspro.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SIGCORP_SIGISS:
                        SigCorp sigcorp = new SigCorp((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedCanNfse.cMunicipio);
                        sigcorp.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.METROPOLIS:

                        #region METROPOLIS

                        Metropolis metropolis = new Metropolis((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      oDadosPedCanNfse.cMunicipio,
                                                      ConfiguracaoApp.ProxyUsuario,
                                                      ConfiguracaoApp.ProxySenha,
                                                      ConfiguracaoApp.ProxyServidor,
                                                      Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital metropolisdig = new AssinaturaDigital();
                        metropolisdig.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        metropolis.CancelarNfse(NomeArquivoXML);
                        break;

                    #endregion METROPOLIS

                    case PadroesNFSe.FIORILLI:
                        Fiorilli fiorilli = new Fiorilli((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedCanNfse.cMunicipio,
                                                        Empresas.Configuracoes[emp].UsuarioWS,
                                                        Empresas.Configuracoes[emp].SenhaWS,
                                                        ConfiguracaoApp.ProxyUsuario,
                                                        ConfiguracaoApp.ProxySenha,
                                                        ConfiguracaoApp.ProxyServidor,
                                                        Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital ass = new AssinaturaDigital();
                        ass.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        fiorilli.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SIMPLISS:
                        SimplISS simpliss = new SimplISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                oDadosPedCanNfse.cMunicipio,
                                                Empresas.Configuracoes[emp].UsuarioWS,
                                                Empresas.Configuracoes[emp].SenhaWS,
                                                ConfiguracaoApp.ProxyUsuario,
                                                ConfiguracaoApp.ProxySenha,
                                                ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital sing = new AssinaturaDigital();
                        sing.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        simpliss.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.CONAM:
                        Conam conam = new Conam((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                oDadosPedCanNfse.cMunicipio,
                                                Empresas.Configuracoes[emp].UsuarioWS,
                                                Empresas.Configuracoes[emp].SenhaWS);

                        conam.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.EGOVERNE:

                        #region E-Governe

                        EGoverne egoverne = new EGoverne((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosPedCanNfse.cMunicipio,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor,
                        Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assegov = new AssinaturaDigital();
                        assegov.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        egoverne.CancelarNfse(NomeArquivoXML);

                        #endregion E-Governe

                        break;

                    case PadroesNFSe.COPLAN:

                        #region Coplan

                        Coplan coplan = new Coplan((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosPedCanNfse.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assCoplan = new AssinaturaDigital();
                        assCoplan.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        coplan.CancelarNfse(NomeArquivoXML);
                        break;

                    #endregion Coplan

                    case PadroesNFSe.EL:

                        #region E&L

                        EL el = new EL((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
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
                        GovDigital govdig = new GovDigital((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                            Empresas.Configuracoes[emp].X509Certificado,
                                                            oDadosPedCanNfse.cMunicipio,
                                                            ConfiguracaoApp.ProxyUsuario,
                                                            ConfiguracaoApp.ProxySenha,
                                                            ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital adgovdig = new AssinaturaDigital();
                        adgovdig.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        govdig.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.BSITBR:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosPedCanNfse.tpAmb == 1)
                            pedCanNfse = new Components.PJaraguaGO.nfseWS();
                        else
                            throw new Exception("Município de Jaraguá-GO não dispõe de ambiente de homologação para envio de NFS-e em teste.");
                        break;

                    case PadroesNFSe.EQUIPLANO:
                        cabecMsg = "1";
                        break;

                    case PadroesNFSe.PORTALFACIL_ACTCON_202:
                        cabecMsg = "<cabecalho><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.PORTALFACIL_ACTCON:
                    case PadroesNFSe.PRODATA:
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.MGM:

                        #region MGM

                        MGM mgm = new MGM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                           oDadosPedCanNfse.cMunicipio,
                                           Empresas.Configuracoes[emp].UsuarioWS,
                                           Empresas.Configuracoes[emp].SenhaWS);
                        mgm.CancelarNfse(NomeArquivoXML);
                        break;

                    #endregion MGM

                    case PadroesNFSe.NATALENSE:
                        cabecMsg = @"
                                    <![CDATA[<?xml version=""1.0""?> <cabecalho xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" versao =""1"" xmlns =""http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"" > <versaoDados>1</versaoDados></cabecalho>
                                    ";
                        break;

                    case PadroesNFSe.CONSIST:

                        #region Consist

                        Consist consist = new Consist((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
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

                    case PadroesNFSe.FREIRE_INFORMATICA:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.MEMORY:

                        #region Memory

                        Memory memory = new Memory((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
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
                        if (oDadosPedCanNfse.cMunicipio == 4109401 ||
                            oDadosPedCanNfse.cMunicipio == 3131703 ||
                            oDadosPedCanNfse.cMunicipio == 4303004)
                        {
                            Pronin pronin = new Pronin((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                oDadosPedCanNfse.cMunicipio,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor,
                                Empresas.Configuracoes[emp].X509Certificado);

                            AssinaturaDigital assPronin = new AssinaturaDigital();
                            assPronin.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                            pronin.CancelarNfse(NomeArquivoXML);
                        }
                        break;

                    case PadroesNFSe.EGOVERNEISS:

                        #region EGoverne ISS

                        EGoverneISS egoverneiss = new EGoverneISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
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
                        Bauru_SP bauru_SP = new Bauru_SP((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        oDadosPedCanNfse.cMunicipio);
                        bauru_SP.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SMARAPD:
                        if (Empresas.Configuracoes[emp].UnidadeFederativaCodigo == 3201308) //Município de Cariacica-ES
                        {
                            throw new Exception("Município de Cariacica-ES não permite cancelamento de NFS-e via webservice.");
                        }
                        break;

                    #region Tinus

                    case PadroesNFSe.TINUS:
                        Tinus tinus = new Tinus((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosPedCanNfse.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital tinusAss = new AssinaturaDigital();
                        tinusAss.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        tinus.CancelarNfse(NomeArquivoXML);
                        break;

                    #endregion Tinus

                    #region SH3

                    case PadroesNFSe.SH3:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    #endregion SH3

#if _fw46

                    #region SOFTPLAN

                    case PadroesNFSe.SOFTPLAN:
                        Components.SOFTPLAN.SOFTPLAN softplan = new Components.SOFTPLAN.SOFTPLAN((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                          Empresas.Configuracoes[emp].PastaXmlRetorno,
                                          Empresas.Configuracoes[emp].UsuarioWS,
                                          Empresas.Configuracoes[emp].SenhaWS,
                                          Empresas.Configuracoes[emp].ClientID,
                                          Empresas.Configuracoes[emp].ClientSecret);

                        AssinaturaDigital softplanAssinatura = new AssinaturaDigital();
                        softplanAssinatura.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                        // Validar o Arquivo XML
                        ValidarXML softplanValidar = new ValidarXML(NomeArquivoXML, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                        string validacao = softplanValidar.ValidarArqXML(NomeArquivoXML);
                        if (validacao != "")
                            throw new Exception(validacao);

                        if (ConfiguracaoApp.Proxy)
                            softplan.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);

                        AssinaturaDigital softplanAss = new AssinaturaDigital();
                        softplanAss.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio, AlgorithmType.Sha256);

                        softplan.CancelarNfse(NomeArquivoXML);
                        break;

                    #endregion SOFTPLAN

#endif

                    case PadroesNFSe.INTERSOL:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><p:cabecalho versao=\"1\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:p=\"http://ws.speedgov.com.br/cabecalho_v1.xsd\" xmlns:p1=\"http://ws.speedgov.com.br/tipos_v1.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://ws.speedgov.com.br/cabecalho_v1.xsd cabecalho_v1.xsd \"><versaoDados>1</versaoDados></p:cabecalho>";
                        break;

                    case PadroesNFSe.JOINVILLE_SC:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosPedCanNfse.tpAmb == 2)
                            pedCanNfse = new Components.HJoinvilleSC.Servicos();
                        else
                            throw new Exception("Ambiente de produção de Joinville-SC ainda não foi implementado no UniNFe.");
                        break;

                    case PadroesNFSe.AVMB_ASTEN:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (oDadosPedCanNfse.tpAmb == 2)
                            pedCanNfse = new Components.HPelotasRS.INfseservice();
                        else
                            pedCanNfse = new Components.PPelotasRS.INfseservice();
                        break;
                }

                if (IsInvocar(padraoNFSe, Servico, Empresas.Configuracoes[emp].UnidadeFederativaCodigo))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, oDadosPedCanNfse.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedCanNfse, NomeMetodoWS(Servico, oDadosPedCanNfse.cMunicipio), cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,   //"-ped-cannfse",
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML,   //"-cannfse",
                                            padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
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

        #endregion Execute

        #region PedCanNfse()

        /// <summary>
        /// Fazer a leitura do conteúdo do XML de cancelamento de NFS-e e disponibilizar conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedCanNfse(int emp, string arquivoXML)
        {
            //int emp = Empresas.FindEmpresaByThread();

            //XmlDocument doc = new XmlDocument();
            //doc.Load(arquivoXML);

            //XmlNodeList infCancList = doc.GetElementsByTagName("CancelarNfseEnvio");

            //foreach (XmlNode infCancNode in infCancList)
            //{
            //    XmlElement infCancElemento = (XmlElement)infCancNode;
            //}
        }

        #endregion PedCanNfse()

        #region EncryptAssinatura()

        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        private void EncryptAssinatura()
        {
            ///danasa: 12/2013
            NFe.Validate.ValidarXML val = new Validate.ValidarXML(NomeArquivoXML, oDadosPedCanNfse.cMunicipio, false);
            val.EncryptAssinatura(NomeArquivoXML);
        }

        #endregion EncryptAssinatura()
    }
}
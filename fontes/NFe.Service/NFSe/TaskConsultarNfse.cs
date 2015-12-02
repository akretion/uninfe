using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFSe.Components;
using NFe.Components.SystemPro;
using NFe.Components.SigCorp;
using NFe.Components.SimplISS;
using NFe.Components.Conam;
using NFe.Components.RLZ_INFORMATICA;
using NFe.Components.EGoverne;
using NFe.Components.EL;
using NFe.Components.GovDigital;
using NFe.Components.FISSLEX;
using NFe.Components.MGM;
using NFe.Components.Consist;

namespace NFe.Service.NFSe
{
    public class TaskNFSeConsultar : TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oDadosPedSitNfse;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

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
                //Ler o XML para pegar parâmetros de envio
                PedSitNfse(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosPedSitNfse.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedLoteRps = null;
                if (IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe);
                    if (wsProxy != null) pedLoteRps = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        if (oDadosPedSitNfse.cMunicipio == 4125506) //São José dos Pinhais - PR  
                            cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://nfe.sjp.pr.gov.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        else
                            cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);
                        wsProxy.Betha = new Betha();
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.BHISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.WEBISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.PORTOVELHENSE:
                        cabecMsg = "<cabecalho versao=\"2.00\" xmlns:ns2=\"http://www.w3.org/2000/09/xmldsig#\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho xmlns=\"http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.SYSTEMPRO:
                        SystemPro syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                        Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado);
                        AssinaturaDigital ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);
                        syspro.ConsultarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SIGCORP_SIGISS:
                        SigCorp sigCorp = new SigCorp((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosPedSitNfse.cMunicipio);
                        sigCorp.ConsultarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SIMPLISS:
                        SimplISS simpliss = new SimplISS((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
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
                        Conam conam = new Conam((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                oDadosPedSitNfse.cMunicipio,
                                                Empresas.Configuracoes[emp].UsuarioWS,
                                                Empresas.Configuracoes[emp].SenhaWS);

                        conam.ConsultarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.RLZ_INFORMATICA:
                        Rlz_Informatica rlz = new Rlz_Informatica((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                                    Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                                    oDadosPedSitNfse.cMunicipio);

                        rlz.ConsultarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.EGOVERNE:
                        #region E-Governe
                        EGoverne egoverne = new EGoverne((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosPedSitNfse.cMunicipio,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor,
                        Empresas.Configuracoes[emp].X509Certificado);

                        AssinaturaDigital assegov = new AssinaturaDigital();
                        assegov.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                        egoverne.ConsultarNfse(NomeArquivoXML);
                        #endregion
                        break;

                    case PadroesNFSe.EL:
                        #region E&L
                        EL el = new EL((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                                        oDadosPedSitNfse.cMunicipio,
                                        Empresas.Configuracoes[emp].UsuarioWS,
                                        Empresas.Configuracoes[emp].SenhaWS,
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyUsuario : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxySenha : ""),
                                        (ConfiguracaoApp.Proxy ? ConfiguracaoApp.ProxyServidor : ""));
                        
                        el.ConsultarNfse(NomeArquivoXML);
                        break;
                        #endregion 

                    case PadroesNFSe.GOVDIGITAL:
                        GovDigital govdig = new GovDigital((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                            Empresas.Configuracoes[emp].PastaXmlRetorno, 
                                                            Empresas.Configuracoes[emp].X509Certificado,
                                                            oDadosPedSitNfse.cMunicipio,
                                                            ConfiguracaoApp.ProxyUsuario,
                                                            ConfiguracaoApp.ProxySenha,
                                                            ConfiguracaoApp.ProxyServidor);

                        AssinaturaDigital adgovdig = new AssinaturaDigital();
                        adgovdig.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                        govdig.ConsultarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.EQUIPLANO:
                        cabecMsg = "1";
                        break;

                    case PadroesNFSe.PRODATA:
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FISSLEX:
                        FISSLEX fisslex = new FISSLEX((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
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
                        MGM mgm = new MGM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                           oDadosPedSitNfse.cMunicipio,
                                           Empresas.Configuracoes[emp].UsuarioWS,
                                           Empresas.Configuracoes[emp].SenhaWS);
                        mgm.ConsultarNfse(NomeArquivoXML);
                        break;
                        #endregion

                    case PadroesNFSe.NATALENSE:
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.CONSIST:
                        #region Consist
                        Consist consist = new Consist((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosPedSitNfse.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);

                        consist.ConsultarNfse(NomeArquivoXML);
                        break;
                        #endregion

                }

                if (IsUtilizaCompilacaoWs(padraoNFSe, Servico))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, oDadosPedSitNfse.cMunicipio), 
                                            cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,    //"-ped-sitnfse",
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML,     //"-sitnfse", 
                                            padraoNFSe, Servico);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
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
        #endregion

        #region PedSitNfse()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por numero e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedSitNfse(string arquivoXML)
        {
            int emp = Empresas.FindEmpresaByThread();
        }
        #endregion
    }
}

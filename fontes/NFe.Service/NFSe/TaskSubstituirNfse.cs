using NFe.Certificado;
using NFe.Components;
using NFe.Components.Coplan;
using NFe.Components.Pronin;
using NFe.Components.SystemPro;
using NFe.Settings;
using System;
using System.IO;
using System.ServiceModel;
using static NFe.Components.Security.SOAPSecurity;

namespace NFe.Service.NFSe
{
    public class TaskSubstituirNfse: TaskAbst
    {
        public TaskSubstituirNfse(string arquivo)
        {
            Servico = Servicos.NFSeSubstituirNfse;
            NomeArquivoXML = arquivo;
        }

        #region Objeto com os dados do XML da consulta nfse

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse dadosXML;

        #endregion Objeto com os dados do XML da consulta nfse

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                dadosXML = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                var padraoNFSe = Functions.PadraoNFSe(dadosXML.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedSubstNfse = null;

                if(IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, dadosXML.cMunicipio);
                    pedSubstNfse = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }
                var cabecMsg = "";

                var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, Servico);

                switch(padraoNFSe)
                {
                    case PadroesNFSe.AVMB_ASTEN:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.AVMB_ASTEN);

                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if(dadosXML.tpAmb == 2)
                        {
                            pedSubstNfse = new Components.HPelotasRS.INfseservice();
                        }
                        else
                        {
                            pedSubstNfse = new Components.PPelotasRS.INfseservice();
                        }

                        break;

                    case PadroesNFSe.EMBRAS:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.E_RECEITA:
                        cabecMsg = "<cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;
                    case PadroesNFSe.ADM_SISTEMAS:
                        cabecMsg = "<cabecalho versao=\"2.01\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.01</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        pedSubstNfse = dadosXML.tpAmb == 1 ?
                                        new Components.PAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://demo.saatri.com.br/servicos/nfse.svc")) :
                                        new Components.HAmargosaBA.InfseClient(GetBinding(), new EndpointAddress("https://homologa-demo.saatri.com.br/servicos/nfse.svc")) as object;

                        SignUsingCredentials(emp, pedSubstNfse);
                        break;

                    case PadroesNFSe.INDAIATUBA_SP:
                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SIGCORP_SIGISS_203:
                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SMARAPD_204:
                        cabecMsg = "<cabecalho versao=\"2.04\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.04</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.IIBRASIL:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.04\"><versaoDados>2.04</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SYSTEMPRO:
                        var syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado, dadosXML.cMunicipio);
                        var ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                        syspro.SubstituirNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.PRONIN:
                        if(dadosXML.cMunicipio == 4323002)
                        {
                            var pronin = new Pronin((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                Empresas.Configuracoes[emp].PastaXmlRetorno,
                                dadosXML.cMunicipio,
                                ConfiguracaoApp.ProxyUsuario,
                                ConfiguracaoApp.ProxySenha,
                                ConfiguracaoApp.ProxyServidor,
                                Empresas.Configuracoes[emp].X509Certificado);

                            var assPronin = new AssinaturaDigital();
                            assPronin.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                            pronin.SubstituirNfse(NomeArquivoXML);
                        }
                        break;

                    case PadroesNFSe.COPLAN:

                        #region Coplan

                        var coplan = new Coplan((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                             dadosXML.cMunicipio,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor,
                            Empresas.Configuracoes[emp].X509Certificado);

                        var assCoplan = new AssinaturaDigital();
                        assCoplan.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                        coplan.SubstituirNfse(NomeArquivoXML);
                        break;

                        #endregion Coplan


                }

                if(IsInvocar(padraoNFSe, Servico, Empresas.Configuracoes[emp].UnidadeFederativaCodigo))
                {

                    //Assinar o XML
                    var ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedSubstNfse, NomeMetodoWS(Servico, dadosXML.cMunicipio), cabecMsg, this,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoXML,
                        padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML) +
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoXML);

                    if(File.Exists(filenameFTP))
                    {
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                    }
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoERR, ex);
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
    }
}
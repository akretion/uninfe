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
using NFe.Components.Fiorilli;
using NFe.Components.SimplISS;
using NFe.Components.Conam;
using NFe.Components.RLZ_INFORMATICA;
using NFe.Components.EGoverne;
using NFe.Components.EL;
using NFe.Components.GovDigital;
using NFe.Components.EloTech;
using NFe.Components.MGM;
using NFe.Components.Consist;

namespace NFe.Service.NFSe
{
    public class TaskNFSeRecepcionarLoteRps : TaskAbst
    {
        #region Objeto com os dados do XML de lote rps
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do lote rps
        /// </summary>
        private DadosEnvLoteRps oDadosEnvLoteRps;
        #endregion

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

                if (IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe);
                    if (wsProxy != null)
                        envLoteRps = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.IPM:
                        //código da cidade da receita federal, este arquivo pode ser encontrado em ~\uninfe\doc\Codigos_Cidades_Receita_Federal.xls</para>
                        //O código da cidade está hardcoded pois ainda está sendo usado apenas para campo mourão
                        IPM ipm = new IPM(Empresas.Configuracoes[emp].UsuarioWS, Empresas.Configuracoes[emp].SenhaWS, oDadosEnvLoteRps.cMunicipio, Empresas.Configuracoes[emp].PastaXmlRetorno);

                        if (ConfiguracaoApp.Proxy)
                            ipm.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);

                        ipm.EmitirNF(NomeArquivoXML, (TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo);
                        break;

                    case PadroesNFSe.GINFES:
                        if (oDadosEnvLoteRps.cMunicipio == 4125506) //São José dos Pinhais - PR  
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

                    case PadroesNFSe.BLUMENAU_SC:
                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.BHISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.BHISS);
                        break;

                    case PadroesNFSe.WEBISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.PAULISTANA:
                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.PORTOVELHENSE:
                        cabecMsg = "<cabecalho versao=\"2.00\" xmlns:ns2=\"http://www.w3.org/2000/09/xmldsig#\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.DSF:
                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho xmlns=\"http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.FINTEL);
                        break;

                    case PadroesNFSe.SYSTEMPRO:
                        #region SystemPro
                        SystemPro syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado);
                        AssinaturaDigital ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);
                        syspro.EmiteNF(NomeArquivoXML);
                        break;
                        #endregion

                    case PadroesNFSe.SIGCORP_SIGISS:
                        #region SigCorp
                        SigCorp sigcorp = new SigCorp((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oDadosEnvLoteRps.cMunicipio);
                        sigcorp.EmiteNF(NomeArquivoXML);
                        break;
                        #endregion

                    case PadroesNFSe.FIORILLI:
                        #region Fiorilli
                        Fiorilli fiorilli = new Fiorilli((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS,
                        ConfiguracaoApp.ProxyUsuario,
                        ConfiguracaoApp.ProxySenha,
                        ConfiguracaoApp.ProxyServidor);


                        AssinaturaDigital ass = new AssinaturaDigital();
                        ass.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                        fiorilli.EmiteNF(NomeArquivoXML);
                        break;
                        #endregion

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
                        #endregion

                    case PadroesNFSe.CONAM:
                        #region Conam
                        Conam conam = new Conam((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio,
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS);

                        conam.EmiteNF(NomeArquivoXML);
                        break;
                        #endregion

                    case PadroesNFSe.RLZ_INFORMATICA:
                        #region RLZ
                        Rlz_Informatica rlz = new Rlz_Informatica((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        oDadosEnvLoteRps.cMunicipio);

                        rlz.EmiteNF(NomeArquivoXML);
                        break;
                        #endregion

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
                        #endregion

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
                        #endregion

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
                        #endregion

                    case PadroesNFSe.EQUIPLANO:
                        cabecMsg = "1";
                        break;

                    case PadroesNFSe.NATALENSE:
                    case PadroesNFSe.PRODATA:
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.VVISS:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.VVISS);
                        break;

                    case PadroesNFSe.ELOTECH:
                        #region EloTech
                        EloTech elotech = new EloTech((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                                      Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      oDadosEnvLoteRps.cMunicipio,
                                                      Empresas.Configuracoes[emp].UsuarioWS,
                                                      Empresas.Configuracoes[emp].SenhaWS,
                                                      ConfiguracaoApp.ProxyUsuario,
                                                      ConfiguracaoApp.ProxySenha,
                                                      ConfiguracaoApp.ProxyServidor,
                                                      Empresas.Configuracoes[emp].X509Certificado);

                        elotech.EmiteNF(NomeArquivoXML);
                        break;
                        #endregion

                    case PadroesNFSe.MGM:
                        #region MGM
                        MGM mgm = new MGM((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                                           Empresas.Configuracoes[emp].PastaXmlRetorno,
                                           oDadosEnvLoteRps.cMunicipio,
                                           Empresas.Configuracoes[emp].UsuarioWS,
                                           Empresas.Configuracoes[emp].SenhaWS);
                        mgm.EmiteNF(NomeArquivoXML);
                        break;
                        #endregion


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
                        #endregion

                }

                if (IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, oDadosEnvLoteRps.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, envLoteRps, NomeMetodoWS(Servico, oDadosEnvLoteRps.cMunicipio), cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,    //"-env-loterps", 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML,  //"-ret-loterps", 
                                            padraoNFSe, Servico);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML) + "\\" + Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
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

        #region EncryptAssinatura()
        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        private void EncryptAssinatura()
        {
            ///danasa: 12/2013
            NFe.Validate.ValidarXML val = new Validate.ValidarXML(NomeArquivoXML, oDadosEnvLoteRps.cMunicipio, false);
            val.EncryptAssinatura(NomeArquivoXML);
        }
        #endregion

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
        #endregion

    }
}

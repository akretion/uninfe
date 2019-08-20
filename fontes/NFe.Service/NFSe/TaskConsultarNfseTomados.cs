using NFe.Certificado;
using NFe.Components;
using NFe.Components.Betha.NewVersion;
using NFe.Settings;
using System;
using System.IO;

#if _fw46


#endif

namespace NFe.Service.NFSe
{
    public class TaskConsultarNfseTomados : TaskAbst
    {
        public TaskConsultarNfseTomados(string arquivo)
        {
            Servico = Servicos.NFSeConsultarNFSeTomados;
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
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                dadosXML = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(dadosXML.cMunicipio);

                //Este serviço, quando padrão BETHA, só tem para a versão do XML 2.02
                if (padraoNFSe == PadroesNFSe.BETHA)
                {
                    padraoNFSe = PadroesNFSe.BETHA202;
                }

                WebServiceProxy wsProxy = null;
                object pedConsNfseTomados = null;

                if (IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, dadosXML.cMunicipio);
                    pedConsNfseTomados = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }
                string cabecMsg = "";

                switch (padraoNFSe)
                {
                    case PadroesNFSe.INDAIATUBA_SP:
                        cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.BETHA202:
                        ConteudoXML.PreserveWhitespace = false;
                        ConteudoXML.Load(NomeArquivoXML);

                        Betha betha = new Betha((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            dadosXML.cMunicipio,
                            Empresas.Configuracoes[emp].UsuarioWS,
                            Empresas.Configuracoes[emp].SenhaWS,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyServidor);

                        betha.ConsultarNfseServicoTomado(NomeArquivoXML);
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
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, Servico);

                if (IsInvocar(padraoNFSe, Servico, dadosXML.cMunicipio))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedConsNfseTomados, NomeMetodoWS(Servico, dadosXML.cMunicipio), cabecMsg, this,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoXML,
                        padraoNFSe, Servico, securityProtocolType);

                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML) +
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoXML);

                    if (File.Exists(filenameFTP))
                    {
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoERR, ex);
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
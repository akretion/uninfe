using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service
{
    public class TaskDownloadEventoseSocial : TaskAbst
    {
        public TaskDownloadEventoseSocial(string arquivo)
        {
            Servico = Servicos.DownloadEventoseSocial;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                WebServiceProxy wsProxy = null;
                object eSocial = null;

                wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, 991, Empresas.Configuracoes[emp].AmbienteCodigo, 0);
                if (wsProxy != null)
                    eSocial = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(991, 1, 0, Servico);

                var nomeMetodo = NomeMetodo();

                new AssinaturaDigital().Assinar(ConteudoXML, emp, 991, AlgorithmType.Sha256);

                oInvocarObj.Invocar(wsProxy,
                                    eSocial,
                                    nomeMetodo,
                                    null,
                                    this,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).EnvioXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).RetornoXML,
                                    true,
                                    securityProtocolType);

                ///
                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                  Functions.ExtrairNomeArq(NomeArquivoXML,
                                                  Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).EnvioXML) + "\\" + Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).RetornoXML);
                if (File.Exists(filenameFTP))
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_downevt).RetornoERR, ex);
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

        private string NomeMetodo()
        {
            var downloadEventosId = ConteudoXML.GetElementsByTagName("solicDownloadEvtsPorId")[0];
            var downloadEventosRecibo = ConteudoXML.GetElementsByTagName("solicDownloadEventosPorNrRecibo")[0];

            if (downloadEventosId != null)
                return "SolicitarDownloadEventosPorId";

            if (downloadEventosRecibo != null)
                return "SolicitarDownloadEventosPorNrRecibo";

            return null;
        }
    }
}
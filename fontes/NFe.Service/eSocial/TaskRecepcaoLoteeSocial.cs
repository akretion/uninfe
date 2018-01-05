using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    public class TaskRecepcaoLoteeSocial : TaskAbst
    {
        public TaskRecepcaoLoteeSocial(string arquivo)
        {
            Servico = Servicos.RecepcaoLoteeSocial;

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
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                WebServiceProxy wsProxy = null;
                object eSocial = null;

                wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, 991, Empresas.Configuracoes[emp].AmbienteCodigo, 0);
                if (wsProxy != null)
                    eSocial = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(991, 1, 0, Servico);

                AssinarXMLLote(emp);

                oInvocarObj.Invocar(wsProxy,
                                    eSocial,
                                    wsProxy.NomeMetodoWS[0],
                                    null,
                                    this,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).EnvioXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).RetornoXML,
                                    true,
                                    securityProtocolType);

                ///
                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                  Functions.ExtrairNomeArq(NomeArquivoXML,
                                                  Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).EnvioXML) + "\\" + Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).RetornoXML);
                if (File.Exists(filenameFTP))
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.eSocial_loteevt).RetornoERR, ex);
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

        private void AssinarXMLLote(int emp)
        {
            AssinaturaDigital ad = new AssinaturaDigital();

            XmlNodeList envioLoteEventosNodeList = ConteudoXML.GetElementsByTagName("envioLoteEventos");

            foreach (XmlNode envioLoteEventosNode in envioLoteEventosNodeList)
            {
                XmlElement envioLoteEventosElement = (XmlElement)envioLoteEventosNode;
                XmlNodeList eventosNodeList = ConteudoXML.GetElementsByTagName("eventos");

                foreach (XmlNode eventosNode in eventosNodeList)
                {
                    XmlElement eventosNodeElement = (XmlElement)eventosNode;
                    XmlNodeList eventoNodeList = ConteudoXML.GetElementsByTagName("evento");

                    foreach (XmlNode eventoNode in eventoNodeList)
                    {
                        XmlElement eventoElement = (XmlElement)eventoNode;
                        XmlNodeList eSocialNodeList = eventoElement.GetElementsByTagName("eSocial");

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(eSocialNodeList[0].OuterXml);

                        ad.Assinar(xmlDoc, emp, 991, AlgorithmType.Sha256, false);

                        XmlNode newNode = xmlDoc.ChildNodes[0];
                        eventoNode.RemoveChild(eSocialNodeList[0]);
                        eventoNode.AppendChild(ConteudoXML.ImportNode(xmlDoc.DocumentElement, true));
                    }
                }
            }
        }
    }
}
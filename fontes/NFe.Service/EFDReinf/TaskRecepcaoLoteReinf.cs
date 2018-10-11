using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    public class TaskRecepcaoLoteReinf : TaskAbst
    {
        public TaskRecepcaoLoteReinf(string arquivo)
        {
            Servico = Servicos.RecepcaoLoteReinf;

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
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                WebServiceProxy wsProxy = null;
                object efdReinf = null;

                wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, 991, Empresas.Configuracoes[emp].AmbienteCodigo, 0);
                if (wsProxy != null)
                    efdReinf = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(991, 1, 0, Servico);

                AssinarXMLLote(emp);

                oInvocarObj.Invocar(wsProxy,
                                    efdReinf,
                                    wsProxy.NomeMetodoWS[0],
                                    null,
                                    this,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).EnvioXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).RetornoXML,
                                    true,
                                    securityProtocolType);

                GerarXMLDistribuicao(ConteudoXML, emp);

                ///
                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                  Functions.ExtrairNomeArq(NomeArquivoXML,
                                                  Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).EnvioXML) + "\\" + Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).RetornoXML);

                if (File.Exists(filenameFTP))
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_loteevt).RetornoERR, ex);
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

        /// <summary>
        /// Gerar XML de distribuição do EFDReinf
        /// </summary>
        private void GerarXMLDistribuicao(XmlDocument conteudoXML, int emp)
        {
            XmlDocument retornoLoteEventosArquivo = new XmlDocument();
            retornoLoteEventosArquivo.LoadXml(vStrXmlRetorno);
            XmlNode evtTotal = null;
            XmlNode eventoAprovado = null;
            StreamWriter swProc = null;

            try
            {
                XmlNode retornoLoteEventos = retornoLoteEventosArquivo.GetElementsByTagName("retornoLoteEventos")[0];
                XmlNode cdStatus = ((XmlElement)retornoLoteEventos).GetElementsByTagName("cdStatus")[0];

                if (cdStatus.InnerText.Equals("0"))
                {
                    XmlNode retonoEventos = retornoLoteEventosArquivo.GetElementsByTagName("retornoEventos")[0];

                    foreach (XmlNode retonoEvento in retonoEventos)
                    {
                        string cdRetorno = ((XmlElement)retonoEvento).GetElementsByTagName("cdRetorno")[0].InnerText;

                        if (cdRetorno.Equals("0"))
                        {
                            evtTotal = ((XmlElement)retonoEvento).GetElementsByTagName("Reinf")[0];

                            string retornoEventoID = ((XmlElement)retonoEvento).Attributes.GetNamedItem("id").Value;

                            XmlNode loteEventos = conteudoXML.GetElementsByTagName("loteEventos")[0];

                            foreach (XmlNode evento in loteEventos)
                            {
                                string eventoID = ((XmlElement)evento).Attributes.GetNamedItem("id").Value;

                                if (retornoEventoID.Equals(eventoID))
                                {
                                    string xmlDistribuicao = "<reinfProc>";
                                    eventoAprovado = ((XmlElement)evento).GetElementsByTagName("Reinf")[0];
                                    xmlDistribuicao += eventoAprovado.OuterXml;
                                    xmlDistribuicao += "<retornoEvento>";
                                    xmlDistribuicao += evtTotal.OuterXml;
                                    xmlDistribuicao += "</retornoEvento>";
                                    xmlDistribuicao += "</reinfProc>";

                                    //Nome do arquivo de distribuição do EFDReinf
                                    string nomeArqDist = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                        eventoID + Propriedade.ExtRetorno.ProcReinf;

                                    //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de
                                    //validar o XML pelos Schemas. Wandrey/André 10/08/2018
                                    swProc = File.CreateText(nomeArqDist);
                                    swProc.Write(xmlDistribuicao);
                                    swProc.Close();
                                    swProc = null;

                                    DateTime dataEvento = Convert.ToDateTime(eventoID.Substring(17, 4) + "-" + eventoID.Substring(21, 2) + "-" + eventoID.Substring(23, 2));

                                    TFunctions.MoverArquivo(nomeArqDist, PastaEnviados.Autorizados, dataEvento);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }
        }

        private void AssinarXMLLote(int emp)
        {
            AssinaturaDigital ad = new AssinaturaDigital();
            ad.AssinarLoteEFDReinf(ConteudoXML, emp);
        }
    }
}
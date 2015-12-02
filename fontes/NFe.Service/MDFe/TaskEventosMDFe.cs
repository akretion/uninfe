using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using NFe.Components;
using NFe.Settings;
using NFe.Certificado;

namespace NFe.Service
{
    public class TaskMDFeEventos : TaskAbst
    {
        public TaskMDFeEventos()
        {
            Servico = Servicos.MDFeRecepcaoEvento;
        }

        #region Classe com os dados do XML do registro de eventos
        private DadosenvEvento dadosEnvEvento;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosEnvEvento = new DadosenvEvento();
                //Ler o XML para pegar parâmetros de envio
                EnvEvento(emp, dadosEnvEvento);//, TpcnResources.chMDFe.ToString());

                ValidaEvento(emp, dadosEnvEvento);

                //vai pegar o ambiente da Chave da Nfe autorizada p/ corrigir tpEmis
                int tpEmis = this.dadosEnvEvento.eventos[0].tpEmis; //Convert.ToInt32(this.dadosEnvEvento.eventos[0].chNFe.Substring(34, 1));

                //Pegar o estado da chave, pois na cOrgao pode vir o estado 91 - Wandreuy 22/08/2012
                int cOrgao = dadosEnvEvento.eventos[0].cOrgao;
                int ufParaWS = cOrgao;

                //Se o cOrgao for igual a 91 tenho que mudar a ufParaWS para que na hora de buscar o WSDL para conectar ao serviço, ele consiga encontrar. Wandrey 23/01/2013
                if (cOrgao == 91)
                    ufParaWS = Convert.ToInt32(dadosEnvEvento.eventos[0].chNFe.Substring(0, 2));

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ufParaWS, dadosEnvEvento.eventos[0].tpAmb, tpEmis);

                object oRecepcaoEvento = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(cOrgao, Servico));

                wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.cUF.ToString(), cOrgao.ToString());
                wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLMDFeEvento);

                //Criar objeto da classe de assinatura digital
                AssinaturaDigital oAD = new AssinaturaDigital();

                //Assinar o XML
                oAD.Assinar(NomeArquivoXML, emp, cOrgao);

                oInvocarObj.Invocar(wsProxy,
                                    oRecepcaoEvento,
                                    wsProxy.NomeMetodoWS[0],//NomeMetodoWS(Servico, ufParaWS), 
                                    oCabecMsg,
                                    this,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).RetornoXML);

                //Ler o retorno
                LerRetornoEvento(emp);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML, Propriedade.ExtRetorno.Eve_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 09/03/2010
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
                    //Se falhou algo na hora de deletar o XML de evento, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }
        #endregion

#if f
        #region EnvEvento()
        /// <summary>
        /// Fazer a leitura dos dados do XML
        /// </summary>
        /// <param name="emp">Código da Empresa</param>
        /// <param name="arquivoXML">Arquivo XML</param>
        private void EnvEvento(int emp, string arquivoXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList envEventoList = doc.GetElementsByTagName("infEvento");

            foreach (XmlNode envEventoNode in envEventoList)
            {
                XmlElement envEventoElemento = (XmlElement)envEventoNode;

                dadosEnvEvento.eventos.Add(new Evento());
                dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].tpEvento = envEventoElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEvento.ToString())[0].InnerText;
                dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].tpAmb = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].cOrgao = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName(NFe.Components.TpcnResources.cOrgao.ToString())[0].InnerText);
                dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].chNFe = envEventoElemento.GetElementsByTagName(TpcnResources.chMDFe.ToString())[0].InnerText;
                dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].nSeqEvento = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName(NFe.Components.TpcnResources.nSeqEvento.ToString())[0].InnerText);
            }
        }
        #endregion
#endif

        #region LerRetornoEvento
        private void LerRetornoEvento(int emp)
        {
            //<<<UTF8 -> tem acentuacao no retorno
            TextReader txt = new StreamReader(NomeArquivoXML, Encoding.Default);
            XmlDocument docEventoOriginal = new XmlDocument();
            docEventoOriginal.Load(Functions.StringXmlToStreamUTF8(txt.ReadToEnd()));
            txt.Close();

            /*
            vStrXmlRetorno = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<retEventoMDFe xmlns=\"http://www.portalfiscal.inf.br/mdfe\" versao=\"2.00\">" +
                "<infEvento Id=\"ID342130000096132\">" +
                "<tpAmb>2</tpAmb>" +
                "<verAplic>RS20130820221405</verAplic>" +
                "<cOrgao>42</cOrgao>" +
                "<cStat>135</cStat>" +
                "<xMotivo>Evento registrado e vinculado a CT-e</xMotivo>" +
                "<chMDFe>42131175892067000187570040000001091211932160</chMDFe>" +
                "<tpEvento>110111</tpEvento>" +
                "<xEvento>Cancelamento</xEvento>" +
                "<nSeqEvento>1</nSeqEvento>" +
                "<dhRegEvento>2013-11-13T15:27:12</dhRegEvento>" +
                "<nProt>342130000096132</nProt>" +
                "</infEvento>" +
                "</retEventoMDFe>";
            */

            MemoryStream msXml = Functions.StringXmlToStreamUTF8(vStrXmlRetorno);
            XmlDocument doc = new XmlDocument();
            doc.Load(msXml);

            XmlNodeList retEnvRetornoList = doc.GetElementsByTagName("retEventoMDFe");

            foreach (XmlNode retConsSitNode in retEnvRetornoList)
            {
                XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                XmlNodeList envEventosList = doc.GetElementsByTagName("infEvento");
                for (int i = 0; i < envEventosList.Count; ++i)
                {
                    XmlElement eleRetorno = envEventosList.Item(i) as XmlElement;

                    string cStatCons = eleRetorno.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;

                    if (cStatCons == "132" || cStatCons == "135" || cStatCons == "136")
                    {
                        string chMDFe = eleRetorno.GetElementsByTagName(TpcnResources.chMDFe.ToString())[0].InnerText;
                        Int32 nSeqEvento = Convert.ToInt32("0" + eleRetorno.GetElementsByTagName(NFe.Components.TpcnResources.nSeqEvento.ToString())[0].InnerText);
                        string tpEvento = eleRetorno.GetElementsByTagName(NFe.Components.TpcnResources.tpEvento.ToString())[0].InnerText;
                        string Id = NFe.Components.TpcnResources.ID.ToString() + tpEvento + chMDFe + nSeqEvento.ToString("00");
                        ///
                        ///procura no Xml de envio pelo Id retornado
                        ///nao sei se a Sefaz retorna na ordem em que foi enviado, então é melhor pesquisar
                        foreach (XmlNode env in docEventoOriginal.GetElementsByTagName("infEvento"))
                        {
                            string Idd = env.Attributes.GetNamedItem(TpcnResources.Id.ToString()).Value;
                            if (Idd == Id)
                            {
                                DateTime dhRegEvento = Functions.GetDateTime(eleRetorno.GetElementsByTagName(NFe.Components.TpcnResources.dhRegEvento.ToString())[0].InnerText);

                                //Gerar o arquivo XML de distribuição do evento, retornando o nome completo do arquivo gravado
                                oGerarXML.XmlDistEventoMDFe(emp, chMDFe, nSeqEvento, Convert.ToInt32(tpEvento), env.ParentNode.OuterXml, eleRetorno.OuterXml, dhRegEvento, true);

                                switch (Convert.ToInt32(tpEvento))
                                {
                                    case 110111: //Cancelamento
                                        try
                                        {
                                            NFe.Service.TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, Empresas.Configuracoes[emp]);
                                        }
                                        catch (Exception ex)
                                        {
                                            Auxiliar.WriteLog("TaskMDFeEventos: " + ex.Message, false);
                                        }
                                        break;
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
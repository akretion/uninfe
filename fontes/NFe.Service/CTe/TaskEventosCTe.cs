using NFe.Components;
using NFe.Settings;
using System;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.CTe;

namespace NFe.Service
{
    public class TaskCTeEventos: TaskAbst
    {
        public TaskCTeEventos(string arquivo)
        {
            Servico = Servicos.CTeRecepcaoEvento;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        #region Classe com os dados do XML do registro de eventos

        private DadosenvEvento dadosEnvEvento;

        #endregion Classe com os dados do XML do registro de eventos

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosEnvEvento = new DadosenvEvento();
                EnvEvento(emp, dadosEnvEvento);
                ValidaEvento(emp, dadosEnvEvento);

                var xml = new EventoCTe();
                xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<EventoCTe>(ConteudoXML);

                //Vai pegar o ambiente da Chave da Nfe autorizada p/ corrigir tpEmis, mas se for carta de correção eletrônica, só aceita envio no ambiente normal.
                var tpEmis = (dadosEnvEvento.eventos[0].tpEvento == ((int)ConvertTxt.tpEventos.tpEvCCe).ToString() ?
                    (int)Components.TipoEmissao.teNormal :
                    dadosEnvEvento.eventos[0].tpEmis);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.CTe,
                    TipoEmissao = (Unimake.Business.DFe.Servicos.TipoEmissao)tpEmis,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                var recepcaoEvento = new Unimake.Business.DFe.Servicos.CTe.RecepcaoEvento(xml, configuracao);
                recepcaoEvento.Executar();

                ConteudoXML = recepcaoEvento.ConteudoXMLAssinado;
                vStrXmlRetorno = recepcaoEvento.RetornoWSString;

                XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).RetornoXML);

                LerRetornoEvento(emp);
            }
            catch(Exception ex)
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

        #endregion Execute

        #region EnvEvento

        protected override void EnvEvento(int emp, DadosenvEvento dadosEnvEvento)
        {
            var eventoCTeList = ConteudoXML.GetElementsByTagName("eventoCTe");
            var eventoCTeElemento = (XmlElement)eventoCTeList[0];
            dadosEnvEvento.versao = eventoCTeElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

            base.EnvEvento(emp, dadosEnvEvento);
        }

        #endregion EnvEvento

        #region LerRetornoEvento

        private void LerRetornoEvento(int emp)
        {
            var docEventoOriginal = ConteudoXML;
            var autorizou = false;

            /*
                        vStrXmlRetorno = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<retEventoCTe xmlns=\"http://www.portalfiscal.inf.br/cte\" versao=\"3.00\">" +
                            "  <infEvento Id=\"ID342130000096132\">" +
                            "    <tpAmb>2</tpAmb>" +
                            "    <verAplic>RS20130820221405</verAplic>" +
                            "    <cOrgao>43</cOrgao>" +
                            "    <cStat>136</cStat>" +
                            "    <xMotivo>Evento registrado e vinculado a CT-e</xMotivo>" +
                            "    <chCTe>43120178408960000182570010000000041000000047</chCTe>" +
                            "    <tpEvento>110140</tpEvento>" +
                            "    <xEvento>Cancelamento</xEvento>" +
                            "    <nSeqEvento>1</nSeqEvento>" +
                            "    <dhRegEvento>2013-11-13T15:27:12</dhRegEvento>" +
                            "    <nProt>342130000096132</nProt>" +
                            "</infEvento>" +
                            "</retEventoCTe>";
            */

            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            var retEnvRetornoList = doc.GetElementsByTagName("retEventoCTe");

            foreach(XmlNode retConsSitNode in retEnvRetornoList)
            {
                var retConsSitElemento = (XmlElement)retConsSitNode;

                var versao = retConsSitElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                var envEventosList = doc.GetElementsByTagName("infEvento");
                for(var i = 0; i < envEventosList.Count; ++i)
                {
                    var eleRetorno = retEnvRetornoList.Item(i) as XmlElement;

                    var cStatCons = eleRetorno.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;

                    if(cStatCons == "134" || cStatCons == "135" || cStatCons == "136")
                    {
                        var chCTe = eleRetorno.GetElementsByTagName(TpcnResources.chCTe.ToString())[0].InnerText;
                        var nSeqEvento = Convert.ToInt32("0" + eleRetorno.GetElementsByTagName(TpcnResources.nSeqEvento.ToString())[0].InnerText);
                        var tpEvento = eleRetorno.GetElementsByTagName(TpcnResources.tpEvento.ToString())[0].InnerText;
                        var Id = TpcnResources.ID.ToString() + tpEvento + chCTe + nSeqEvento.ToString("00");
                        ///
                        ///procura no Xml de envio pelo Id retornado
                        ///nao sei se a Sefaz retorna na ordem em que foi enviado, então é melhor pesquisar
                        foreach(XmlNode env in docEventoOriginal.GetElementsByTagName("infEvento"))
                        {
                            var Idd = env.Attributes.GetNamedItem(TpcnResources.Id.ToString()).Value;
                            if(Idd == Id)
                            {
                                autorizou = true;

                                var dhRegEvento = Functions.GetDateTime(eleRetorno.GetElementsByTagName(TpcnResources.dhRegEvento.ToString())[0].InnerText);

                                //Gerar o arquivo XML de distribuição do evento, retornando o nome completo do arquivo gravado
                                oGerarXML.XmlDistEventoCTe(emp, chCTe, nSeqEvento, Convert.ToInt32(tpEvento), env.ParentNode.OuterXml, eleRetorno.OuterXml, dhRegEvento, true, versao);

                                switch((ConvertTxt.tpEventos)Convert.ToInt32(tpEvento))
                                {
                                    //Verificar se para o evento cancelamento por substituição será necessário disparar o unidanfe
                                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                                    case ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe:
                                    case ConvertTxt.tpEventos.tpEvCCe:
                                    case ConvertTxt.tpEventos.tpEvEPEC:
                                        try
                                        {
                                            TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, Empresas.Configuracoes[emp]);
                                        }
                                        catch(Exception ex)
                                        {
                                            Auxiliar.WriteLog("TaskCTeEventos: " + ex.Message, false);
                                        }
                                        break;
                                }

                                break;
                            }
                        }
                    }
                }
            }

            if(!autorizou)
            {
                oAux.MoveArqErro(NomeArquivoXML);
            }
        }

        #endregion LerRetornoEvento
    }
}
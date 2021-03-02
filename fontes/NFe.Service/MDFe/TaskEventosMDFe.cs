using NFe.Components;
using NFe.Settings;
using System;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.MDFe;

namespace NFe.Service
{
    public class TaskMDFeEventos: TaskAbst
    {
        public TaskMDFeEventos(string arquivo)
        {
            Servico = Servicos.MDFeRecepcaoEvento;
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

                var xml = new EventoMDFe();
                xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<EventoMDFe>(ConteudoXML);

                var tpEmis = dadosEnvEvento.eventos[0].tpEmis;

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.MDFe,
                    TipoEmissao = (Unimake.Business.DFe.Servicos.TipoEmissao)tpEmis,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                var recepcaoEvento = new Unimake.Business.DFe.Servicos.MDFe.RecepcaoEvento(xml, configuracao);
                recepcaoEvento.Executar();

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

        protected override void EnvEvento(int emp, DadosenvEvento dadosEnvEvento)
        {
            var eventoMDFeList = ConteudoXML.GetElementsByTagName("eventoMDFe");
            var eventoCTeElemento = (XmlElement)eventoMDFeList[0];
            dadosEnvEvento.versao = eventoCTeElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

            base.EnvEvento(emp, dadosEnvEvento);
        }

        #region LerRetornoEvento

        private void LerRetornoEvento(int emp)
        {
            var docEventoOriginal = ConteudoXML;
            var autorizou = false;

            /*
            vStrXmlRetorno = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<retEventoMDFe xmlns=\"http://www.portalfiscal.inf.br/mdfe\" versao=\"3.00\">" +
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

            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            var retEnvRetornoList = doc.GetElementsByTagName("retEventoMDFe");

            foreach(XmlNode retConsSitNode in retEnvRetornoList)
            {
                var retConsSitElemento = (XmlElement)retConsSitNode;
                var versao = retConsSitElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                var envEventosList = doc.GetElementsByTagName("infEvento");
                for(var i = 0; i < envEventosList.Count; ++i)
                {
                    var eleRetorno = envEventosList.Item(i) as XmlElement;

                    var cStatCons = eleRetorno.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;

                    if(cStatCons == "132" || cStatCons == "135" || cStatCons == "136")
                    {
                        var chMDFe = eleRetorno.GetElementsByTagName(TpcnResources.chMDFe.ToString())[0].InnerText;
                        var nSeqEvento = Convert.ToInt32("0" + eleRetorno.GetElementsByTagName(TpcnResources.nSeqEvento.ToString())[0].InnerText);
                        var tpEvento = eleRetorno.GetElementsByTagName(TpcnResources.tpEvento.ToString())[0].InnerText;
                        var Id = TpcnResources.ID.ToString() + tpEvento + chMDFe + nSeqEvento.ToString("00");
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
                                oGerarXML.XmlDistEventoMDFe(emp, chMDFe, nSeqEvento, Convert.ToInt32(tpEvento), env.ParentNode.OuterXml, retConsSitElemento.OuterXml, dhRegEvento, true, versao);

                                switch(Convert.ToInt32(tpEvento))
                                {
                                    case 110111: //Cancelamento
                                        try
                                        {
                                            TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, Empresas.Configuracoes[emp]);
                                        }
                                        catch(Exception ex)
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

            if(!autorizou)
            {
                oAux.MoveArqErro(NomeArquivoXML);
            }
        }

        #endregion LerRetornoEvento
    }
}
using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    public class TaskNFeDownload : TaskAbst
    {
        public TaskNFeDownload(string arquivo)
        {
            Servico = Servicos.NFeDownload;
            NomeArquivoXML = arquivo;
            if (vXmlNfeDadosMsgEhXML)
            {
                ConteudoXML.PreserveWhitespace = false;
                ConteudoXML.Load(arquivo);
            }
        }

        #region Classe com os dados do XML do registro de download da nfe

        private DadosenvDownload oDadosenvDownload;

        #endregion Classe com os dados do XML do registro de download da nfe

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();
            try
            {
                oDadosenvDownload = new DadosenvDownload();
                EnvDownloadNFe(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, Convert.ToInt32(oDadosenvDownload.chNFe.Substring(0, 2)), oDadosenvDownload.tpAmb, 0);
                    System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(Convert.ToInt32(oDadosenvDownload.chNFe.Substring(0, 2)), oDadosenvDownload.tpAmb, (int)TipoEmissao.teNormal, Servico);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oDownloadEvento = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(Convert.ToInt32(oDadosenvDownload.chNFe.Substring(0, 2)), Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), oDadosenvDownload.chNFe.Substring(0, 2));
                    wsProxy.SetProp(oCabecMsg, TpcnResources.versaoDados.ToString(), ConvertTxt.versoes.VersaoXMLEnvDownload);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy,
                                        oDownloadEvento,
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg,
                                        this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).RetornoXML,
                                        false,
                                        securityProtocolType);
                    //Ler o retorno
                    LerRetornoDownloadNFe(emp);

                    //Gravar o xml de retorno para o ERP. Não pode gravar antes de extrair os downloads, para que o ERP saiba quando realmente terminou. Wandrey 03/04/2013
                    oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioXML,
                                         Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).RetornoXML, vStrXmlRetorno);
                }
                else
                {
                    // Gerar o XML de eventos a partir do TXT gerado pelo ERP
                    oGerarXML.EnvioDownloadNFe(Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioTXT) +
                                                Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioXML, oDadosenvDownload);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioXML :
                                                        Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioTXT;
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.retDownload_ERR, ex);
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

        #region EnvDownloadNFe

        private void EnvDownloadNFe(int emp, string arquivoXML)
        {
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                /// tpAmb|2
                /// CNPJ|10290739000139
                /// chNFe|35110310290739000139550010000000011051128041
                List<string> cLinhas = Functions.LerArquivo(arquivoXML);
                Functions.PopulateClasse(this.oDadosenvDownload, cLinhas);
            }
            else
            {
                //<?xml version="1.0" encoding="UTF-8"?>
                //<downloadNFe versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                //      <tpAmb>2</tpAmb>
                //      <xServ>DOWNLOAD NFE</xServ>
                //      <CNPJ>10290739000139</CNPJ>
                //      <chNFe>35110310290739000139550010000000011051128041</chNFe>
                //</downloadNFe>

                XmlNodeList envEventoList = ConteudoXML.GetElementsByTagName("downloadNFe");

                foreach (XmlNode envEventoNode in envEventoList)
                {
                    XmlElement envEventoElemento = (XmlElement)envEventoNode;
                    Functions.PopulateClasse(oDadosenvDownload, envEventoElemento);
                }
            }
        }

        #endregion EnvDownloadNFe

        #region LerRetornoDownloadNFe

        private void LerRetornoDownloadNFe(int emp)
        {
            ///<retDownloadNFe versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
            /// <tpAmb>2</tpAmb>
            /// <verAplic>XX_v123</verAplic>
            /// <cStat>139</cStat>
            /// <xMotivo>Pedido de download Processado</xMotivo>
            /// <dhResp>2011-11-24T10:02:46</dhResp>
            /// <retNFe>
            ///     <chNFe>12345678901234567890123456789012345678901234</chNFe>
            ///     <cStat>632</cStat>
            ///     <xMotivo>Rejeição: Solicitação fora de prazo, a NF-e não está mais disponível para download</xMotivo>
            /// </retNFe>
            /// <retNFe>
            ///     <chNFe>12345678901234567890123456789012345678901245</chNFe>
            ///     <cStat>140</cStat>
            ///     <xMotivo>Download disponibilizado</xMotivo>
            ///     <procNFeZip > (xml da procNFe compactado no padrão gZip com representação base64binary) </procNFeZip >
            /// </retNFe>
            /// <retNFe>
            ///     <chNFe>12345678901234567890123456789012345678901256</chNFe>
            ///     <cStat>140</cStat>
            ///     <xMotivo>Download disponibilizado</xMotivo>
            ///     <procNFeZip> (xml da procNFe compactado no padrão gZip com representação base64binary) </procNFeZip >
            /// </retNFe>
            ///</retDownloadNFe>

            string folderDestino = Empresas.Configuracoes[emp].PastaDownloadNFeDest;
            if (string.IsNullOrEmpty(folderDestino))
                return;

            if (!Directory.Exists(folderDestino))
                Directory.CreateDirectory(folderDestino);

            XmlDocument docretDownload = new XmlDocument();
            docretDownload.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            XmlNodeList retDownLoadList = docretDownload.GetElementsByTagName("retDownloadNFe");

            if (retDownLoadList != null)
            {
                if (retDownLoadList.Count > 0)
                {
                    XmlElement retElemento = (XmlElement)retDownLoadList.Item(0);
                    int cStat = Convert.ToInt32(Functions.LerTag(retElemento, TpcnResources.cStat.ToString(), false));
                    if (cStat == 139)
                    {
                        DateTime dhResp = Functions.GetDateTime(Functions.LerTag(retElemento, TpcnResources.dhResp.ToString(), false));

                        XmlNodeList retDownLoadListx = retElemento.GetElementsByTagName("retNFe");
                        if (retDownLoadListx != null)
                        {
                            for (var iitem = 0; iitem < retDownLoadListx.Count; ++iitem)
                            {
                                XmlElement el1 = retDownLoadListx.Item(iitem) as XmlElement;

                                if (Convert.ToInt32(Functions.LerTag(el1, TpcnResources.cStat.ToString(), false)) == 140)
                                {
                                    string chave = Functions.LerTag(el1, NFe.Components.TpcnResources.chNFe.ToString(), false) + Propriedade.ExtRetorno.ProcNFe;

                                    foreach (XmlElement xitem in el1.ChildNodes)
                                    {
                                        switch (xitem.LocalName)
                                        {
                                            case "procNFe":
                                                foreach (XmlElement xitem1 in xitem.ChildNodes)
                                                {
                                                    switch (xitem1.LocalName)
                                                    {
                                                        case "nfeProc":
                                                            bool afound = false;
                                                            for (int a1 = 0; a1 < xitem1.ChildNodes[0].Attributes.Count; ++a1)
                                                                if (xitem1.ChildNodes[0].Attributes[a1].LocalName.Equals("xmlns"))
                                                                    afound = true;

                                                            if (!afound)
                                                            {
                                                                XmlAttribute xmlVersion1 = docretDownload.CreateAttribute("xmlns");
                                                                xmlVersion1.Value = NFeStrConstants.NAME_SPACE_NFE;
                                                                xitem1.ChildNodes[0].Attributes.Append(xmlVersion1);
                                                            }

                                                            afound = false;
                                                            for (int a1 = 0; a1 < xitem1.ChildNodes[1].Attributes.Count; ++a1)
                                                                if (xitem1.ChildNodes[1].Attributes[a1].LocalName.Equals("xmlns"))
                                                                    afound = true;

                                                            if (!afound)
                                                            {
                                                                XmlAttribute xmlVersion1 = docretDownload.CreateAttribute("xmlns");
                                                                xmlVersion1.Value = NFeStrConstants.NAME_SPACE_NFE;
                                                                xitem1.ChildNodes[1].Attributes.Append(xmlVersion1);
                                                            }

                                                            File.WriteAllText(Path.Combine(folderDestino, chave), "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xitem1.OuterXml);
                                                            break;
                                                    }
                                                }
                                                break;

                                            case "procNFeGrupoZip": //<procNFeGrupoZip><NFeZip></NFeZip><protNFeZip>
                                                foreach (XmlElement xitem1 in xitem.ChildNodes)
                                                {
                                                    switch (xitem1.LocalName)
                                                    {
                                                        case "NFeZip":
                                                            //item.GrupoOpcional.procNFeGrupoZip.NFeZip = xitem1.InnerText;
                                                            break;

                                                        case "protNFeZip":
                                                            //item.GrupoOpcional.procNFeGrupoZip.protNFeZip = xitem1.InnerText;
                                                            //Decode(xitem1.InnerText);
                                                            break;
                                                    }
                                                }
                                                break;

                                            case "procNFeZip":
                                                Console.WriteLine("zip");
                                                //item.GrupoOpcional.procNFeZip = "";
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion LerRetornoDownloadNFe
    }
}
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
    public class TaskNFeDownload : TaskAbst
    {
        #region Classe com os dados do XML do registro de download da nfe
        private DadosenvDownload oDadosenvDownload;// = new DadosenvDownload();
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();
            Servico = Servicos.NFeDownload;
            try
            {
                oDadosenvDownload = new DadosenvDownload();
                //Ler o XML para pegar parâmetros de envio
                EnvDownloadNFe(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, Convert.ToInt32(oDadosenvDownload.chNFe.Substring(0, 2)), oDadosenvDownload.tpAmb);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oDownloadEvento = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(Convert.ToInt32(oDadosenvDownload.chNFe.Substring(0, 2)), Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.cUF.ToString(), oDadosenvDownload.chNFe.Substring(0, 2));
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLEnvDownload);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, 
                                        oDownloadEvento, 
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg,
                                        this);
                    //Ler o retorno
                    LerRetornoDownloadNFe(emp);

                    //Gravar o xml de retorno para o ERP. Não pode gravar antes de extrair os downloads, para que o ERP saiba quando realmente terminou. Wandrey 03/04/2013
                    oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioXML, 
                                         Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).RetornoXML, this.vStrXmlRetorno);
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
        #endregion

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

                XmlDocument doc = new XmlDocument();
                doc.Load(arquivoXML);

                XmlNodeList envEventoList = doc.GetElementsByTagName("downloadNFe");

                foreach (XmlNode envEventoNode in envEventoList)
                {
                    XmlElement envEventoElemento = (XmlElement)envEventoNode;
                    Functions.PopulateClasse(this.oDadosenvDownload, envEventoElemento);
                }
            }
        }
        #endregion

        #region LerRetornoDownloadNFe

#if teste
        internal class TretprocNFeGrupoZip
        {
            public string NFeZip { get; set; }             //E JR14 B64 1-1 XML da NF-e compactado no padrão gZip, o tipo do campo é base64Binary.
            public string protNFeZip { get; set; }         //E JR14 B64 1-1 Protocolo de Autorização de Uso compactado no padrão gZip, o tipo do campo é base64Binary.          
        }
        internal class TretprocNFeGrupoOpcional
        {
            //JR12 Grupo opcional G JR08 - 0-1 Grupo de elementos no Schema XML.
            public string procNFeZip { get; set; }                      //CE JR12 B64 0-1 XML da NF-e (procNFe) compactado no padrão gZip, o tipo do campo é base64Binary.
            public TretprocNFeGrupoZip procNFeGrupoZip { get; set; }    //CG JR12 G   0-1 Grupo contendo a NF-e compactada e o Protocolo de Autorização compactado.

            public TretprocNFeGrupoOpcional()
            {
                procNFeGrupoZip = new TretprocNFeGrupoZip();
            }
        }
        internal class TretDownloadNFe_retNFe
        {
            public string chNFe { get; set; }
            public Int32 cStat { get; set; }
            public string xMotivo { get; set; }
            public TretprocNFeGrupoOpcional GrupoOpcional { get; set; }

            public TretDownloadNFe_retNFe()
            {
                GrupoOpcional = new TretprocNFeGrupoOpcional();
            }
        }
#endif

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
            docretDownload.Load(Functions.StringXmlToStreamUTF8(this.vStrXmlRetorno));

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

                        System.Xml.XmlNodeList retDownLoadListx = retElemento.GetElementsByTagName("retNFe");
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
                                                            System.IO.File.WriteAllText(Path.Combine(folderDestino, chave), "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xitem1.OuterXml);
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
                    /*
                    foreach (XmlNode infretNFe in retElemento.ChildNodes)
                    {
                        XmlElement infElemento = (XmlElement)infretNFe;

                        TretDownloadNFe_retNFe item = new TretDownloadNFe_retNFe();

                        item.chNFe = pcnAuxiliar.readValue(infElemento, pcnConstantes.chNFe.ToString());
                        item.cStat = Convert.ToInt32(pcnAuxiliar.readValue(infElemento, pcnConstantes.cStat.ToString()));
                        item.xMotivo = pcnAuxiliar.readValue(infElemento, pcnConstantes.xMotivo.ToString());

                        XmlNode nodeGrupoOpcional = infElemento.FirstChild;
                        if (nodeGrupoOpcional != null)
                        {
                            item.GrupoOpcional.procNFeZip = "";//.procNFeZip = nodeGrupoOpcional.
                            item.GrupoOpcional.procNFeGrupoZip.NFeZip = "";
                            item.GrupoOpcional.procNFeGrupoZip.protNFeZip = "";
                        }
                        this.retNFe.Add(item);
                    }*/
                }
            }
        }
        #endregion

#if false
        #region Decode/Encode
        private string Encode(string str)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
        }

        private string Decode(string str)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }

        private Stream DecodeBase64Gzip(string input)
        {
            char[] enc = input.ToCharArray();
            byte[] dec = Convert.FromBase64CharArray(enc, 0, enc.Length);

            Stream comp = new MemoryStream(dec);
            Stream decomp = new System.IO.Compression.GZipStream(comp, System.IO.Compression.CompressionMode.Decompress, false);

            return decomp;
        }
        #endregion
#endif
    }
}

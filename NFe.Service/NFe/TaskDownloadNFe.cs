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
    public class TaskDownloadNFe : TaskAbst
    {
        #region Classe com os dados do XML do registro de download da nfe
        private DadosenvDownload oDadosenvDownload;// = new DadosenvDownload();
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            Servico = Servicos.DownloadNFe;
            try
            {
                oDadosenvDownload = new DadosenvDownload();
                //Ler o XML para pegar parâmetros de envio
                //LerXML oLer = new LerXML();
                //oLer.
                EnvDownloadNFe(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(
                        Servico,
                        emp,
                        Convert.ToInt32(/*oLer.*/oDadosenvDownload.chNFe.Substring(0, 2)),
                        /*oLer.*/oDadosenvDownload.tpAmb,
                        1);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oDownloadEvento = wsProxy.CriarObjeto("NfeDownloadNF");
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(Convert.ToInt32(/*oLer.*/oDadosenvDownload.chNFe.Substring(0, 2))));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", /*oLer.*/oDadosenvDownload.chNFe.Substring(0, 2));
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvDownload);

                    //Criar objeto da classe de assinatura digital
                    //AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    //oAD.Assinar(NomeArquivoXML, emp);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oDownloadEvento, "nfeDownloadNF", 
                                        oCabecMsg, 
                                        this, 
                                        Propriedade.ExtEnvio.EnvDownload_XML.Replace(".xml", ""), 
                                        Propriedade.ExtRetorno.retDownload_XML.Replace(".xml", ""));

                    //Ler o retorno
                    LerRetornoDownloadNFe(emp);
                }
                else
                {
                    // Gerar o XML de eventos a partir do TXT gerado pelo ERP
                    oGerarXML.EnvioDownloadNFe(Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.EnvDownload_TXT) + Propriedade.ExtEnvio.EnvDownload_XML, /*oLer.*/oDadosenvDownload);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvDownload_XML : Propriedade.ExtEnvio.EnvDownload_TXT;
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
            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Nfe:
                    if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
                    {
                        /// tpAmb|2
                        /// CNPJ|10290739000139 
                        /// chNFe|35110310290739000139550010000000011051128041
                        List<string> cLinhas = Functions.LerArquivo(arquivoXML);
                        foreach (string cTexto in cLinhas)
                        {
                            string[] dados = cTexto.Split('|');
                            if (dados.GetLength(0) <= 1) continue;

                            switch (dados[0].ToLower())
                            {
                                case "tpamb":
                                    this.oDadosenvDownload.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "cnpj":
                                    this.oDadosenvDownload.CNPJ = dados[1].Trim();
                                    break;
                                case "chnfe":
                                    this.oDadosenvDownload.chNFe = dados[1].Trim();
                                    break;
                            }
                        }
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

                            this.oDadosenvDownload.tpAmb = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                            this.oDadosenvDownload.CNPJ = envEventoElemento.GetElementsByTagName("CNPJ")[0].InnerText;
                            this.oDadosenvDownload.chNFe = envEventoElemento.GetElementsByTagName("chNFe")[0].InnerText;
                        }
                    }
                    break;
            }
        }
        #endregion

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

            string folderDestino = Empresa.Configuracoes[emp].PastaDownloadNFeDest;
            if (string.IsNullOrEmpty(folderDestino))
                return;

            if (!Directory.Exists(folderDestino))
                Directory.CreateDirectory(folderDestino);

            XmlDocument docretDownload = new XmlDocument();
            docretDownload.Load(this.vStrXmlRetorno);

            XmlNodeList retDownLoadList = docretDownload.GetElementsByTagName("retDownloadNFe");

            if (retDownLoadList != null)
            {
                if (retDownLoadList.Count > 0)
                {
                    XmlElement retElemento = (XmlElement)retDownLoadList.Item(0);
                    //ConteudoRetorno += Functions.LerTag(retElemento, "tpAmb");
                    //ConteudoRetorno += Functions.LerTag(retElemento, "verAplic");
                    //ConteudoRetorno += Functions.LerTag(retElemento, "cStat");
                    //ConteudoRetorno += Functions.LerTag(retElemento, "xMotivo");
                    //ConteudoRetorno += Functions.LerTag(retElemento, "dhResp");

                    foreach (XmlNode infretNFe in retElemento.ChildNodes)
                    {
                        XmlElement infElemento = (XmlElement)infretNFe;

                        int cStat = Convert.ToInt32(Functions.LerTag(infElemento, "cStat"));
                        if (cStat == 140)
                        {
                            ///
                            /// xml disponivel
                            /// 
                            string chNFe = Functions.LerTag(infElemento, "chNFe");
                            string xml64 = Decode(Functions.LerTag(infElemento, "procNFeZip"));
                        }
                    }
                }
            }
        }
        #endregion

        #region Decode/Encode
        private string Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }
        
        private string Decode(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
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
    }
}

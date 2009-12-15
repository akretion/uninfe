using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using UniNFeLibrary;
using UniNFeLibrary.Enums;

namespace unicte
{
    public class GerarXML : absGerarXML
    {
        #region CabecMsg()
        /// <summary>
        /// Gera uma string com o XML do cabeçalho dos dados a serem enviados para os serviços da NFe
        /// </summary>
        /// <param name="pVersaoDados">
        /// Versão do arquivo XML que será enviado para os WebServices. Esta versão varia de serviço para
        /// serviço e deve ser consultada no manual de integração da NFE
        /// </param>
        /// <returns>
        /// Retorna uma string com o XML do cabeçalho dos dados a serem enviados para os serviços da NFe
        /// </returns>
        /// <example>
        /// vCabecMSG = GerarXMLCabecMsg("1.07");
        /// MessageBox.Show( vCabecMSG );
        /// 
        /// //O conteúdo que será demonstrado no MessageBox é:
        /// //
        /// //  <?xml version="1.0" encoding="UTF-8" ?>
        /// //  <cabecMsg xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.02">
        /// //     <versaoDados>1.07</versaoDados>
        /// //  </cabecMsg>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        public override string CabecMsg(string VersaoDados)
        {
            return string.Empty;
        }
        #endregion

        #region Cancelamento()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFinalArqEnvio"></param>
        /// <param name="tpAmb"></param>
        /// <param name="tpEmis"></param>
        /// <param name="chNFe"></param>
        /// <param name="nProt"></param>
        /// <param name="xJust"></param>
        public override void Cancelamento(string pFinalArqEnvio, int tpAmb, int tpEmis, string chNFe, string nProt, string xJust)
        {
        }
        #endregion

        #region Consulta
        public override void Consulta(string pFinalArqEnvio, int tpAmb, int tpEmis, string chNFe)
        {
            StringBuilder aXML = new StringBuilder();
            aXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            aXML.Append("<consSitCTe xmlns=\"" + ConfiguracaoApp.nsURI + "\" versao=\"" + ConfiguracaoApp.VersaoXMLPedSit + "\">");
            aXML.AppendFormat("<tpAmb>{0}</tpAmb>", tpAmb);
            if (ConfiguracaoApp.tpEmis != tpEmis)
                aXML.AppendFormat("<tpEmis>{0}</tpEmis>", tpEmis);  //<<< opcional >>>
            aXML.Append("<xServ>CONSULTAR</xServ>");
            aXML.AppendFormat("<chCTe>{0}</chCTe>", chNFe);
            aXML.Append("</consSitCTe>");

            oAux.GravarArquivoParaEnvio(pFinalArqEnvio, aXML.ToString());
        }
        #endregion

        #region ConsultaCadastro()
        /// <summary>
        /// Cria um arquivo XML com a estrutura necessária para consultar um cadastro
        /// Voce deve preencher o estado e mais um dos tres itens: CPNJ, IE ou CPF
        /// </summary>
        /// <param name="uf">Sigla do UF do cadastro a ser consultado. Tem que ter duas letras. SU para suframa.</param>
        /// <param name="cnpj"></param>
        /// <param name="ie"></param>
        /// <param name="cpf"></param>
        /// <returns>Retorna o caminho e nome do arquivo criado</returns>
        /// <by>Marcos Diez</by>
        /// <date>29/08/2009</date>
        public string ConsultaCadastro(string pArquivo, string uf, string cnpj, string ie, string cpf)
        {
            string header = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                "<ConsCad xmlns=\"" + ConfiguracaoApp.nsURI +
                "\" versao=\"" + ConfiguracaoApp.VersaoXMLConsCad + "\"><infCons><xServ>CONS-CAD</xServ>";

            cnpj = OnlyNumbers(cnpj);
            ie = OnlyNumbers(ie);
            cpf = OnlyNumbers(cpf);

            StringBuilder saida = new StringBuilder();
            saida.Append(header);
            saida.AppendFormat("<UF>{0}</UF>", uf);
            if (!string.IsNullOrEmpty(cnpj))
            {
                saida.AppendFormat("<CNPJ>{0}</CNPJ>", cnpj);
            }
            else
            if (!string.IsNullOrEmpty(ie))
            {
                saida.AppendFormat("<IE>{0}</IE>", ie);
            }
            else
            if (!string.IsNullOrEmpty(cpf))
            {
                saida.AppendFormat("<CPF>{0}</CPF>", cpf);
            }
            saida.Append("</infCons></ConsCad>");

            string _arquivo_saida = (string.IsNullOrEmpty(pArquivo) ? DateTime.Now.ToString("yyyyMMddThhmmss") + ExtXml.ConsCad : pArquivo);

            oAux.GravarArquivoParaEnvio(_arquivo_saida, saida.ToString());
            
            return ConfiguracaoApp.vPastaXMLEnvio + "\\" + _arquivo_saida;
        }

        /// <summary>
        /// retorna uma string contendo apenas os digitos da entrada
        /// </summary>
        /// <by>Marcos Diez</by>
        /// <date>29/08/2009</date>
        private static string OnlyNumbers(string entrada)
        {
            if (string.IsNullOrEmpty(entrada)) return null;
            StringBuilder saida = new StringBuilder(entrada.Length);
            foreach (char c in entrada)
            {
                if (char.IsDigit(c))
                {
                    saida.Append(c);
                }
            }
            return saida.ToString();
        }
        #endregion  
 
        #region EncerrarLoteNfe()
        /// <summary>
        /// Encerra a string do XML de lote de notas fiscais eletrônicas
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected override void EncerrarLoteNfe(Int32 intNumeroLote)
        {
            strXMLLoteNfe += "</enviCTe>";

            try
            {
                this.GerarXMLLote(intNumeroLote);
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region GravarRetornoEmTXT()
        //TODO: Documentar este método
        protected override void TXTRetorno(string pFinalArqEnvio, string pFinalArqRetorno, string ConteudoXMLRetorno)
        {
        }
        #endregion

        #region IniciarLoteNfe()
        /// <summary>
        /// Inicia a string do XML do Lote de notas fiscais
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected override void IniciarLoteNfe(Int32 intNumeroLote)
        {
            string cVersaoDados = "1.02";

            strXMLLoteNfe = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strXMLLoteNfe += "<enviCTe xmlns=\"http://www.portalfiscal.inf.br/cte\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" versao=\"" + cVersaoDados + "\">";
            strXMLLoteNfe += "<idLote>" + intNumeroLote.ToString("000000000000000") + "</idLote>";
        }

        #endregion

        #region InserirNFeLote()
        /// <summary>
        /// Insere o XML de Nota Fiscal passado por parâmetro na string do XML de Lote de NFe
        /// </summary>
        /// <param name="strArquivoNfe">Nome do arquivo XML de nota fiscal eletrônica a ser inserido no lote</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected override void InserirNFeLote(string strArquivoNfe)
        {
            try
            {
                string vNfeDadosMsg = this.oAux.XmlToString(strArquivoNfe);

                //Separar somente o conteúdo a partir da tag <CTe> até </CTe>
                Int32 nPosI = vNfeDadosMsg.IndexOf("<CTe");
                Int32 nPosF = vNfeDadosMsg.Length - nPosI;
                strXMLLoteNfe += vNfeDadosMsg.Substring(nPosI, nPosF);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Inutilizacao
        public override void Inutilizacao(string pFinalArqEnvio, int tpAmb, int tpEmis, int cUF, int ano, string CNPJ, int mod, int serie, int nNFIni, int nNFFin, string xJust)
        {
        }
        #endregion

        #region LerXMLNFe()
        protected override absLerXML.DadosNFeClass LerXMLNFe(string Arquivo)
        {
            LerXML oLerXML = new LerXML();
            oLerXML.Nfe(Arquivo);

            return oLerXML.oDadosNfe;
        }
        #endregion

        #region LerXMLRecibo()
        protected override absLerXML.DadosRecClass LerXMLRecibo(string Arquivo)
        {
            LerXML oLerXML = new LerXML();
            oLerXML.Recibo(Arquivo);

            return oLerXML.oDadosRec;
        }
        #endregion  

        #region StatusServico()
        /// <summary>
        /// Criar um arquivo XML com a estrutura necessária para consultar o status do serviço
        /// </summary>
        /// <returns>Retorna o caminho e nome do arquivo criado</returns>
        /// <example>
        /// string vPastaArq = this.CriaArqXMLStatusServico();
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public override string StatusServico(int tpEmis)
        {
            return this.StatusServico(tpEmis, ConfiguracaoApp.UFCod);
        }
        #endregion

        #region StatusServico() - Sobrecarga
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tpEmis"></param>
        /// <param name="cUF"></param>
        /// <returns></returns>
        public override string StatusServico(int tpEmis, int cUF)
        {
            //TODO: CONFIG
            string vDadosMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                +"<consStatServCte xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"" + ConfiguracaoApp.VersaoXMLStatusServico + "\" xmlns=\"" + ConfiguracaoApp.nsURI + "\">"
                + "<tpAmb>" + ConfiguracaoApp.tpAmb.ToString() + "</tpAmb>"
                + "<cUF>" + cUF.ToString() + "</cUF>"
                + "<tpEmis>" + tpEmis.ToString() + "</tpEmis>"  //danasa 9-2009
                + "<xServ>STATUS</xServ>"
                + "</consStatServCte>";

            string _arquivo_saida = ConfiguracaoApp.vPastaXMLEnvio + "\\" +
                                    DateTime.Now.ToString("yyyyMMddThhmmss") +
                                    ExtXml.PedSta;// "-ped-sta.xml";

            StreamWriter SW = File.CreateText(_arquivo_saida);
            SW.Write(vDadosMsg);
            SW.Close();

            return _arquivo_saida;
        } 
        #endregion

        #region StatusServico() - Sobrecarga
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pArquivo"></param>
        /// <param name="tpAmb"></param>
        /// <param name="tpEmis"></param>
        /// <param name="cUF"></param>
        public override void StatusServico(string pArquivo, int tpAmb, int tpEmis, int cUF)
        {
        }
        #endregion

        #region XMLDistInut()
        /// <summary>
        /// Criar o arquivo XML de distribuição das Inutilizações de Números de NFe´s com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqInut">Nome arquivo XML de Inutilização</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        public override void XmlDistInut(string strArqInut, string strRetInutNFe)
        {
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqInut);

                XmlNodeList InutNFeList = doc.GetElementsByTagName("inutCTe");
                XmlNode InutNFeNode = InutNFeList[0];
                string strInutNFe = InutNFeNode.OuterXml;

                //Montar o XML -procCancNFe.xml
                string strXmlProcInutNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<procInutCTe xmlns=\"http://www.portalfiscal.inf.br/cte\" versao=\"1.01\">" +
                    strInutNFe +
                    strRetInutNFe +
                    "</procInutCTe>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcInutNFe =  ConfiguracaoApp.vPastaXMLEnviado + "\\" + 
                                                PastaEnviados.EmProcessamento.ToString() + "\\" + 
                                                oAux.ExtrairNomeArq(strArqInut, ExtXml.PedInu/*"-ped-inu.xml"*/) +
                                                ExtXmlRet.ProcInutNFe;// "-procInutNFe.xml";

                //Gravar o XML em uma linha só (sem quebrar as tag's linha a linha) ou dá erro na hora de validar o XML pelos Schemas. Wandrey 13/05/2009
                swProc = File.CreateText(strNomeArqProcInutNFe);
                swProc.Write(strXmlProcInutNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }

        }
        #endregion

        #region XMLDistCanc()
        /// <summary>
        /// Criar o arquivo XML de distribuição dos Cancelamentos com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqCanc">Nome arquivo XML de Cancelamento</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        public override void XmlDistCanc(string strArqCanc, string strRetCancNFe)
        {
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqCanc);

                XmlNodeList CancNFeList = doc.GetElementsByTagName("cancCTe");
                XmlNode CancNFeNode = CancNFeList[0];
                string strCancNFe = CancNFeNode.OuterXml;

                //Montar o XML -procCancNFe.xml
                string strXmlProcCancNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<procCancCTe xmlns=\"http://www.portalfiscal.inf.br/cte\" versao=\"1.01\">" +
                    strCancNFe +
                    strRetCancNFe +
                    "</procCancCTe>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcCancNFe = ConfiguracaoApp.vPastaXMLEnviado + "\\" + 
                                                PastaEnviados.EmProcessamento.ToString() + "\\" + 
                                                oAux.ExtrairNomeArq(strArqCanc, ExtXml.PedCan/*"-ped-can.xml"*/) +
                                                ExtXmlRet.ProcCancNFe;// "-procCancNFe.xml";

                //Gravar o XML em uma linha só (sem quebrar as tag's linha a linha) ou dá erro na hora de validar o XML pelos Schemas. Wandrey 13/05/2009
                swProc = File.CreateText(strNomeArqProcCancNFe);
                swProc.Write(strXmlProcCancNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }
        }
        #endregion

        #region XmlPedRec()
        /// <summary>
        /// Gera o XML de pedido de analise do recibo do lote
        /// </summary>
        /// <param name="strRecibo">Número do recibo a ser consultado o lote</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        public override void XmlPedRec(string strRecibo)
        {
            string strNomeArqPedRec = ConfiguracaoApp.vPastaXMLEnvio + "\\" + strRecibo + ExtXml.PedRec;
            if (!File.Exists(strNomeArqPedRec))
            {
                
                //TODO: CONFIG
                string strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<consReciCTe xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" "+
                    "versao=\"" + ConfiguracaoApp.VersaoXMLPedRec + "\" xmlns=\"" + ConfiguracaoApp.nsURI + "\">" +
                    "<tpAmb>" + ConfiguracaoApp.tpAmb.ToString() + "</tpAmb>" +
                    "<nRec>" + strRecibo + "</nRec>" +
                    "</consReciCTe>";

                //Gravar o XML
                oAux.GravarArquivoParaEnvio(strNomeArqPedRec, strXml);
                //MemoryStream oMemoryStream = Auxiliar.StringXmlToStream(strXml);
                //XmlDocument docProc = new XmlDocument();
                //docProc.Load(oMemoryStream);
                //docProc.Save(strNomeArqPedRec);
            }
        }
        #endregion

        #region XMLDistNFe()
        /// <summary>
        /// Criar o arquivo XML de distribuição das NFE com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqNFe">Nome arquivo XML da NFe</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public override void XmlDistNFe(string strArqNFe, string strProtNfe)
        {
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <CTe> até </CTe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqNFe);

                XmlNodeList NFeList = doc.GetElementsByTagName("CTe");
                XmlNode NFeNode = NFeList[0];
                string strNFe = NFeNode.OuterXml;

                //Montar a string contendo o XML -proc-NFe.xml
                string strXmlProcNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<cteProc xmlns=\"http://www.portalfiscal.inf.br/cte\" versao=\"1.02\">" +
                    strNFe +
                    strProtNfe +
                    "</cteProc>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcNFe = ConfiguracaoApp.vPastaXMLEnviado + "\\" +
                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                            oAux.ExtrairNomeArq(strArqNFe, ExtXml.Nfe) +
                                            ExtXmlRet.ProcNFe;

                //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de 
                //validar o XML pelos Schemas. Wandrey 13/05/2009
                swProc = File.CreateText(strNomeArqProcNFe);
                swProc.Write(strXmlProcNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }
        }
        #endregion
 
    }
}

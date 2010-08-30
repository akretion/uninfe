using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using UniNFeLibrary;
using UniNFeLibrary.Enums;
using System.Threading;

namespace uninfe
{
    public class GerarXML : absGerarXML
    {
        #region Construtores
        public GerarXML(int empIndex)
            : base(empIndex)
        {
            EmpIndex = empIndex;
        }
        #endregion

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
            string vCabecMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><cabecMsg xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"" + ConfiguracaoApp.VersaoXMLCabecMsg + "\"><versaoDados>" + VersaoDados + "</versaoDados></cabecMsg>";

            return vCabecMsg;
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
            StringBuilder aXML = new StringBuilder();
            aXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            aXML.Append("<cancNFe xmlns=\"" + ConfiguracaoApp.nsURI + "\" versao=\"" + ConfiguracaoApp.VersaoXMLCanc + "\">");
            aXML.AppendFormat("<infCanc Id=\"ID{0}\">", chNFe);
            aXML.AppendFormat("<tpAmb>{0}</tpAmb>", tpAmb);
            aXML.Append("<xServ>CANCELAR</xServ>");
            aXML.AppendFormat("<chNFe>{0}</chNFe>", chNFe);
            aXML.AppendFormat("<nProt>{0}</nProt>", nProt);
            aXML.AppendFormat("<xJust>{0}</xJust>", xJust);
            aXML.AppendFormat("<tpEmis>{0}</tpEmis>", tpEmis);
            aXML.Append("</infCanc>");
            aXML.Append("</cancNFe>");

            try
            {
                GravarArquivoParaEnvio(pFinalArqEnvio, aXML.ToString());
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Consulta
        public override void Consulta(string pFinalArqEnvio, int tpAmb, int tpEmis, string chNFe)
        {
            StringBuilder aXML = new StringBuilder();
            aXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            aXML.Append("<consSitNFe xmlns=\"" + ConfiguracaoApp.nsURI + "\" versao=\"" + ConfiguracaoApp.VersaoXMLPedSit + "\">");
            aXML.AppendFormat("<tpAmb>{0}</tpAmb>", tpAmb);
            aXML.AppendFormat("<tpEmis>{0}</tpEmis>", tpEmis);  //<<< opcional >>>
            aXML.Append("<xServ>CONSULTAR</xServ>");
            aXML.AppendFormat("<chNFe>{0}</chNFe>", chNFe);
            aXML.Append("</consSitNFe>");

            GravarArquivoParaEnvio(pFinalArqEnvio, aXML.ToString());
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
            int emp = EmpIndex;

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

            GravarArquivoParaEnvio(_arquivo_saida, saida.ToString());

            return Empresa.Configuracoes[emp].PastaEnvio + "\\" + _arquivo_saida;
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
            strXMLLoteNfe += "</enviNFe>";

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
            int emp = EmpIndex;
            string ConteudoRetorno = string.Empty;

            MemoryStream msXml = Auxiliar.StringXmlToStream(ConteudoXMLRetorno);
            try
            {
                switch (Servico)
                {
                    case Servicos.EnviarLoteNfe:
                        {
                            XmlDocument docRec = new XmlDocument();
                            docRec.Load(msXml);

                            XmlNodeList retEnviNFeList = docRec.GetElementsByTagName("retEnviNFe");
                            if (retEnviNFeList != null) //danasa 23-9-2009
                            {
                                if (retEnviNFeList.Count > 0)   //danasa 23-9-2009
                                {
                                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeList.Item(0);
                                    if (retEnviNFeElemento != null)   //danasa 23-9-2009
                                    {
                                        ConteudoRetorno += oAux.LerTag(retEnviNFeElemento, "cStat");
                                        ConteudoRetorno += oAux.LerTag(retEnviNFeElemento, "xMotivo");

                                        XmlNodeList infRecList = retEnviNFeElemento.GetElementsByTagName("infRec");
                                        if (infRecList != null)
                                        {
                                            if (infRecList.Count > 0)   //danasa 23-9-2009
                                            {
                                                XmlElement infRecElemento = (XmlElement)infRecList.Item(0);
                                                if (infRecElemento != null)   //danasa 23-9-2009
                                                {
                                                    ConteudoRetorno += oAux.LerTag(infRecElemento, "nRec");
                                                    ConteudoRetorno += oAux.LerTag(infRecElemento, "dhRecbto");
                                                    ConteudoRetorno += oAux.LerTag(infRecElemento, "tMed");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case Servicos.PedidoSituacaoLoteNFe:
                        {
                            XmlDocument docProRec = new XmlDocument();
                            docProRec.Load(msXml);

                            XmlNodeList retConsReciNFeList = docProRec.GetElementsByTagName("retConsReciNFe");
                            if (retConsReciNFeList != null) //danasa 23-9-2009
                            {
                                if (retConsReciNFeList.Count > 0)   //danasa 23-9-2009
                                {
                                    XmlElement retConsReciNFeElemento = (XmlElement)retConsReciNFeList.Item(0);
                                    if (retConsReciNFeElemento != null)   //danasa 23-9-2009
                                    {
                                        ConteudoRetorno += oAux.LerTag(retConsReciNFeElemento, "nRec");
                                        ConteudoRetorno += oAux.LerTag(retConsReciNFeElemento, "cStat");
                                        ConteudoRetorno += oAux.LerTag(retConsReciNFeElemento, "xMotivo");
                                        ConteudoRetorno += "\r\n";

                                        XmlNodeList protNFeList = retConsReciNFeElemento.GetElementsByTagName("protNFe");
                                        if (protNFeList != null)    //danasa 23-9-2009
                                        {
                                            if (protNFeList.Count > 0)   //danasa 23-9-2009
                                            {
                                                XmlElement protNFeElemento = (XmlElement)protNFeList.Item(0);
                                                if (protNFeElemento != null)
                                                {
                                                    if (protNFeElemento.ChildNodes.Count > 0)
                                                    {
                                                        XmlNodeList infProtList = protNFeElemento.GetElementsByTagName("infProt");

                                                        foreach (XmlNode infProtNode in infProtList)
                                                        {
                                                            XmlElement infProtElemento = (XmlElement)infProtNode;
                                                            string chNFe = oAux.LerTag(infProtElemento, "chNFe");

                                                            ConteudoRetorno += chNFe.Substring(6, 14) + ";";
                                                            ConteudoRetorno += chNFe.Substring(25, 9) + ";";
                                                            ConteudoRetorno += chNFe;
                                                            ConteudoRetorno += oAux.LerTag(infProtElemento, "dhRecbto");
                                                            ConteudoRetorno += oAux.LerTag(infProtElemento, "nProt");
                                                            ConteudoRetorno += oAux.LerTag(infProtElemento, "digVal");
                                                            ConteudoRetorno += oAux.LerTag(infProtElemento, "cStat");
                                                            ConteudoRetorno += oAux.LerTag(infProtElemento, "xMotivo");
                                                            ConteudoRetorno += "\r\n";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case Servicos.CancelarNFe:  //danasa 19-9-2009
                        {
                            XmlDocument docretCanc = new XmlDocument();
                            docretCanc.Load(msXml);

                            XmlNodeList retCancList = docretCanc.GetElementsByTagName("retCancNFe");
                            if (retCancList != null)
                            {
                                if (retCancList.Count > 0)
                                {
                                    XmlElement retCancElemento = (XmlElement)retCancList.Item(0);
                                    if (retCancElemento != null)
                                    {
                                        if (retCancElemento.ChildNodes.Count > 0)
                                        {
                                            XmlNodeList infCancList = retCancElemento.GetElementsByTagName("infCanc");
                                            if (infCancList != null)
                                            {
                                                foreach (XmlNode infCancNode in infCancList)
                                                {
                                                    XmlElement infCancElemento = (XmlElement)infCancNode;

                                                    ConteudoRetorno += oAux.LerTag(infCancElemento, "tpAmb");
                                                    ConteudoRetorno += oAux.LerTag(infCancElemento, "cStat");
                                                    ConteudoRetorno += oAux.LerTag(infCancElemento, "xMotivo");
                                                    ConteudoRetorno += "\r\n";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case Servicos.InutilizarNumerosNFe: //danasa 19-9-2009
                        {
                            XmlDocument docretInut = new XmlDocument();
                            docretInut.Load(msXml);

                            XmlNodeList retInutList = docretInut.GetElementsByTagName("retInutNFe");
                            if (retInutList != null)
                            {
                                if (retInutList.Count > 0)
                                {
                                    XmlElement retInutElemento = (XmlElement)retInutList.Item(0);
                                    if (retInutElemento != null)
                                    {
                                        if (retInutElemento.ChildNodes.Count > 0)
                                        {
                                            XmlNodeList infInutList = retInutElemento.GetElementsByTagName("infInut");
                                            if (infInutList != null)
                                            {
                                                foreach (XmlNode infInutNode in infInutList)
                                                {
                                                    XmlElement infInutElemento = (XmlElement)infInutNode;

                                                    ConteudoRetorno += oAux.LerTag(infInutElemento, "tpAmb");
                                                    ConteudoRetorno += oAux.LerTag(infInutElemento, "cStat");
                                                    ConteudoRetorno += oAux.LerTag(infInutElemento, "xMotivo");
                                                    ConteudoRetorno += oAux.LerTag(infInutElemento, "cUF");
                                                    ConteudoRetorno += "\r\n";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case Servicos.PedidoConsultaSituacaoNFe:   //danasa 19-9-2009
                        {
                            XmlDocument docretConsSit = new XmlDocument();
                            docretConsSit.Load(msXml);

                            XmlNodeList retConsSitList = docretConsSit.GetElementsByTagName("retConsSitNFe");
                            if (retConsSitList != null)
                            {
                                if (retConsSitList.Count > 0)
                                {
                                    XmlElement retConsSitElemento = (XmlElement)retConsSitList.Item(0);
                                    if (retConsSitElemento != null)
                                    {
                                        if (retConsSitElemento.ChildNodes.Count > 0)
                                        {
                                            XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                                            if (infConsSitList != null)
                                            {
                                                foreach (XmlNode infConsSitNode in infConsSitList)
                                                {
                                                    XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                                    ConteudoRetorno += oAux.LerTag(infConsSitElemento, "tpAmb");
                                                    ConteudoRetorno += oAux.LerTag(infConsSitElemento, "cStat");
                                                    ConteudoRetorno += oAux.LerTag(infConsSitElemento, "xMotivo");
                                                    ConteudoRetorno += oAux.LerTag(infConsSitElemento, "cUF");
                                                    ConteudoRetorno += "\r\n";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case Servicos.PedidoConsultaStatusServicoNFe:   //danasa 19-9-2009
                        {
                            XmlDocument docConsStat = new XmlDocument();
                            docConsStat.Load(msXml);

                            XmlNodeList retConsStatServList = docConsStat.GetElementsByTagName("retConsStatServ");
                            if (retConsStatServList != null)
                            {
                                if (retConsStatServList.Count > 0)
                                {
                                    XmlElement retConsStatServElemento = (XmlElement)retConsStatServList.Item(0);
                                    if (retConsStatServElemento != null)
                                    {
                                        ConteudoRetorno += oAux.LerTag(retConsStatServElemento, "tpAmb");
                                        ConteudoRetorno += oAux.LerTag(retConsStatServElemento, "cStat");
                                        ConteudoRetorno += oAux.LerTag(retConsStatServElemento, "xMotivo");
                                        ConteudoRetorno += oAux.LerTag(retConsStatServElemento, "cUF");
                                        ConteudoRetorno += oAux.LerTag(retConsStatServElemento, "dhRecbto");
                                        ConteudoRetorno += oAux.LerTag(retConsStatServElemento, "tMed");
                                        ConteudoRetorno += "\r\n";
                                    }
                                }
                            }
                        }
                        break;

                    case Servicos.ConsultaCadastroContribuinte: //danasa 19-9-2009
                        {
                            ///
                            /// Retorna o texto conforme o manual do Sefaz versao 3.0
                            /// 
                            RetConsCad rconscad = ProcessaConsultaCadastro(msXml);
                            if (rconscad != null)
                            {
                                ConteudoRetorno = rconscad.cStat.ToString("000") + ";";
                                ConteudoRetorno += rconscad.xMotivo.Replace(";", " ") + ";";
                                ConteudoRetorno += rconscad.UF + ";";
                                ConteudoRetorno += rconscad.IE + ";";
                                ConteudoRetorno += rconscad.CNPJ + ";";
                                ConteudoRetorno += rconscad.CPF + ";";
                                ConteudoRetorno += rconscad.dhCons + ";";
                                ConteudoRetorno += rconscad.cUF.ToString("00") + ";";
                                ConteudoRetorno += "\r\r";
                                foreach (infCad infCadNode in rconscad.infCad)
                                {
                                    ConteudoRetorno += infCadNode.IE + ";";
                                    ConteudoRetorno += infCadNode.CNPJ + ";";
                                    ConteudoRetorno += infCadNode.CPF + ";";
                                    ConteudoRetorno += infCadNode.UF + ";";
                                    ConteudoRetorno += infCadNode.cSit + ";";
                                    ConteudoRetorno += infCadNode.xNome.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.xFant.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.xRegApur.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.CNAE.ToString() + ";";
                                    ConteudoRetorno += infCadNode.dIniAtiv + ";";
                                    ConteudoRetorno += infCadNode.dUltSit + ";";
                                    ConteudoRetorno += infCadNode.IEUnica.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.IEAtual.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.ender.xLgr.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.ender.nro.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.ender.xCpl.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.ender.xBairro.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.ender.cMun.ToString("0000000") + ";";
                                    ConteudoRetorno += infCadNode.ender.xMun.Replace(";", " ") + ";";
                                    ConteudoRetorno += infCadNode.ender.CEP.ToString("00000000") + ";";
                                    ConteudoRetorno += "\r\r";
                                }
                            }
                        }
                        break;

                    case Servicos.ConsultaInformacoesUniNFe:
                        break;
                }
                //Gravar o TXT de retorno para o ERP
                if (!string.IsNullOrEmpty(ConteudoRetorno))
                {
                    string TXTRetorno = string.Empty;
                    TXTRetorno = oAux.ExtrairNomeArq(this.NomeXMLDadosMsg, pFinalArqEnvio) + pFinalArqRetorno;
                    TXTRetorno = Empresa.Configuracoes[emp].PastaRetorno + "\\" + oAux.ExtrairNomeArq(TXTRetorno, ".xml") + ".txt";

                    File.WriteAllText(TXTRetorno, ConteudoRetorno, Encoding.Default);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
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
            string cVersaoDados = "1.10";

            strXMLLoteNfe = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strXMLLoteNfe += "<enviNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" versao=\"" + cVersaoDados + "\">";
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

                //Separar somente o conteúdo a partir da tag <NFe> até </NFe>
                Int32 nPosI = vNfeDadosMsg.IndexOf("<NFe");
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
            StringBuilder aXML = new StringBuilder();
            aXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            aXML.Append("<inutNFe xmlns=\"" + ConfiguracaoApp.nsURI + "\" versao=\"" + ConfiguracaoApp.VersaoXMLInut + "\">");
            aXML.AppendFormat("<infInut Id=\"ID{0}{1}{2}{3}{4}{5}\">", cUF.ToString("00"), CNPJ, mod.ToString("00"), serie.ToString("000"), nNFIni.ToString("000000000"), nNFFin.ToString("000000000"));
            aXML.AppendFormat("<tpAmb>{0}</tpAmb>", tpAmb);
            aXML.AppendFormat("<tpEmis>{0}</tpEmis>", tpEmis);
            aXML.Append("<xServ>INUTILIZAR</xServ>");
            aXML.AppendFormat("<cUF>{0}</cUF>", cUF.ToString("00"));
            aXML.AppendFormat("<ano>{0}</ano>", ano.ToString("00"));
            aXML.AppendFormat("<CNPJ>{0}</CNPJ>", CNPJ);
            aXML.AppendFormat("<mod>{0}</mod>", mod.ToString("00"));
            aXML.AppendFormat("<serie>{0}</serie>", serie);
            aXML.AppendFormat("<nNFIni>{0}</nNFIni>", nNFIni);
            aXML.AppendFormat("<nNFFin>{0}</nNFFin>", nNFFin);
            aXML.AppendFormat("<xJust>{0}</xJust>", xJust);
            aXML.Append("</infInut>");
            aXML.Append("</inutNFe>");

            GravarArquivoParaEnvio(pFinalArqEnvio, aXML.ToString());
        }
        #endregion

        #region LerXMLNFe()
        protected override absLerXML.DadosNFeClass LerXMLNFe(string Arquivo)
        {
            LerXML oLerXML = new LerXML();

            try
            {
                oLerXML.Nfe(Arquivo);
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return oLerXML.oDadosNfe;
        }
        #endregion

        #region LerXMLRecibo()
        protected override absLerXML.DadosRecClass LerXMLRecibo(string Arquivo)
        {
            LerXML oLerXML = new LerXML();

            try
            {
                oLerXML.Recibo(Arquivo);
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return oLerXML.oDadosRec;
        }
        #endregion

        #region StatusServico() - Sobrecarga
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tpEmis"></param>
        /// <param name="cUF"></param>
        /// <returns></returns>
        public override string StatusServico(int tpEmis, int cUF, int amb)
        {
            string _arquivo_saida = DateTime.Now.ToString("yyyyMMddThhmmss") + ExtXml.PedSta;

            this.StatusServico(_arquivo_saida, amb, tpEmis, cUF);

            return _arquivo_saida;
        }
        #endregion

        #region StatusServico() - Sobrecarga
        /// <summary>
        /// Gera o XML de consulta status do serviço da NFe
        /// </summary>
        /// <param name="pArquivo">Caminho e nome do arquivo que é para ser gerado</param>
        /// <param name="tpAmb">Ambiente da consulta</param>
        /// <param name="tpEmis">Tipo de emissão da consulta</param>
        /// <param name="cUF">Estado para a consulta</param>
        public override void StatusServico(string pArquivo, int tpAmb, int tpEmis, int cUF)
        {
            StringBuilder vDadosMsg = new StringBuilder();
            vDadosMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            vDadosMsg.Append("<consStatServ versao=\"" + ConfiguracaoApp.VersaoXMLStatusServico + "\" xmlns=\"" + ConfiguracaoApp.nsURI + "\">");
            vDadosMsg.AppendFormat("<tpAmb>{0}</tpAmb>", tpAmb);
            vDadosMsg.AppendFormat("<cUF>{0}</cUF>", cUF);
            vDadosMsg.AppendFormat("<tpEmis>{0}</tpEmis>", tpEmis);  
            vDadosMsg.Append("<xServ>STATUS</xServ>");
            vDadosMsg.Append("</consStatServ>");

            try
            {
                GravarArquivoParaEnvio(pArquivo, vDadosMsg.ToString());
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
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
            int emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqInut);

                XmlNodeList InutNFeList = doc.GetElementsByTagName("inutNFe");
                XmlNode InutNFeNode = InutNFeList[0];
                string strInutNFe = InutNFeNode.OuterXml;

                //Montar o XML -procCancNFe.xml
                string strXmlProcInutNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<procInutNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"" + ConfiguracaoApp.VersaoXMLInut + "\">" +
                    strInutNFe +
                    strRetInutNFe +
                    "</procInutNFe>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcInutNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
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
            int emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqCanc);

                XmlNodeList CancNFeList = doc.GetElementsByTagName("cancNFe");
                XmlNode CancNFeNode = CancNFeList[0];
                string strCancNFe = CancNFeNode.OuterXml;

                //Montar o XML -procCancNFe.xml
                string strXmlProcCancNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<procCancNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"" + ConfiguracaoApp.VersaoXMLCanc + "\">" +
                    strCancNFe +
                    strRetCancNFe +
                    "</procCancNFe>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcCancNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
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
            int emp = EmpIndex;
            string strNomeArqPedRec = Empresa.Configuracoes[emp].PastaEnvio + "\\" + strRecibo + ExtXml.PedRec;
            if (!File.Exists(strNomeArqPedRec))
            {

                string strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<consReciNFe xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                    "versao=\"" + ConfiguracaoApp.VersaoXMLPedRec + "\" xmlns=\"" + ConfiguracaoApp.nsURI + "\">" +
                    "<tpAmb>" + Empresa.Configuracoes[emp].tpAmb.ToString() + "</tpAmb>" +
                    "<nRec>" + strRecibo + "</nRec>" +
                    "</consReciNFe>";

                //Gravar o XML
                GravarArquivoParaEnvio(strNomeArqPedRec, strXml);
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
            int emp = EmpIndex;
            StreamWriter swProc = null;

            try
            {
                if (File.Exists(strArqNFe))
                {
                    //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                    XmlDocument doc = new XmlDocument();

                    doc.Load(strArqNFe);

                    XmlNodeList NFeList = doc.GetElementsByTagName("NFe");
                    XmlNode NFeNode = NFeList[0];
                    string strNFe = NFeNode.OuterXml;

                    //Montar a string contendo o XML -proc-NFe.xml
                    string strXmlProcNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                        "<nfeProc xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"" + ConfiguracaoApp.VersaoXMLNFe + "\">" +
                        strNFe +
                        strProtNfe +
                        "</nfeProc>";

                    //Montar o nome do arquivo -proc-NFe.xml
                    string strNomeArqProcNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                oAux.ExtrairNomeArq(strArqNFe, ExtXml.Nfe) +
                                                ExtXmlRet.ProcNFe;

                    //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de 
                    //validar o XML pelos Schemas. Wandrey 13/05/2009
                    swProc = File.CreateText(strNomeArqProcNFe);
                    swProc.Write(strXmlProcNfe);
                }
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

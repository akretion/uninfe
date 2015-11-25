using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NFe.Components.QRCode
{
    public class QRCode
    {
        #region Propriedades

        #region Proriedades de resultado do processamento
        public string IdentificadorCSC { get; set; }
        public string TokenCSC { get; set; }
        public string ArquivoXML { get; set; }
        #endregion

        #region Recupera dados do XML da NFCe
        private string CNPJ { get; set; }
        private string CPF { get; set; }
        private string idEstrangeiro { get; set; }
        private string ChaveAcesso { get; set; }
        private string TpAmb { get; set; }
        private string DhEmi { get; set; }
        private string vNF { get; set; }
        private string vICMS { get; set; }
        private string digVal { get; set; }
        #endregion

        #region Valores para montar o HASH e o LINK
        protected string ParametrosQR = null;
        protected string HashQRCode = null;
        protected string ParametrosLinkConsulta = null;
        #endregion

        #endregion

        #region Constrututor
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="identificadorCSC">Codigo de identificação no banco de dados do SEFAZ</param>
        /// <param name="tokenCSC">Codigo de identificação do Contribuinte (antigo Token)</param>
        /// <param name="arquivoXML">Arquivo XML com as informações para calculo</param>
        public QRCode(string identificadorCSC, string tokenCSC, string arquivoXML)
        {
            this.IdentificadorCSC = identificadorCSC.Trim();
            this.TokenCSC = tokenCSC.Trim();
            this.ArquivoXML = arquivoXML;

            this.Populate();
            this.Validate();
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Gerar o Hash do QRCode e montar o Link para inseri-lo no XML
        /// </summary>
        /// <param name="linkUF">Link de consulta da UF</param>
        public void GerarLinkConsulta(string linkUF)
        {
            this.ParametrosQR = "chNFe=" + this.ChaveAcesso +
                "&nVersao=100" +
                "&tpAmb=" + this.TpAmb +
                (String.IsNullOrEmpty(this.CNPJ) ? (String.IsNullOrEmpty(this.CPF) ? (String.IsNullOrEmpty(this.idEstrangeiro) ? "" : "&cDest=" + this.idEstrangeiro) : "&cDest=" + this.CPF) : "&cDest=" + this.CNPJ) +
                "&dhEmi=" + Functions.ComputeHexadecimal(this.DhEmi) +
                "&vNF=" + this.vNF +
                "&vICMS=" + this.vICMS +
                "&digVal=" + Functions.ComputeHexadecimal(this.digVal) +
                "&cIdToken=" + this.TokenCSC;

            this.HashQRCode = Criptografia.GetSHA1HashData(this.ParametrosQR + this.IdentificadorCSC, true);

            this.ParametrosLinkConsulta = linkUF + "?" + this.ParametrosQR.Trim() + "&cHashQRCode=" + this.HashQRCode.Trim();
        }

        /// <summary>
        /// Adicionar link gerado ao XML
        /// </summary>
        public void AddLinkQRCode()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ArquivoXML);
            doc.PreserveWhitespace = false;

            foreach (var item in doc)
            {
                if (typeof(XmlElement) == item.GetType())
                {
                    XmlNode Signature = (XmlElement)doc.GetElementsByTagName("Signature")[0];
                    XmlNode el = item as XmlNode;
                    XmlNode nd = doc.CreateElement("infNFeSupl", doc.DocumentElement.NamespaceURI);
                    XmlNode nd1 = doc.CreateElement("qrCode", doc.DocumentElement.NamespaceURI);
                    nd1.InnerXml = ("<![CDATA[" + this.ParametrosLinkConsulta.Trim() + "]]>").Trim();
                    nd.AppendChild(nd1);
                    el.RemoveChild(Signature);
                    el.AppendChild(nd);
                    el.AppendChild(Signature);

                    break;
                }
            }

            string xmlComQRCode = doc.OuterXml;
            StreamWriter SW_2 = File.CreateText(ArquivoXML);
            SW_2.Write(xmlComQRCode);
            SW_2.Close();
        }

        /// <summary>
        /// Valida se deve adicionar o link ao XML
        /// </summary>
        /// <returns>Bolean</returns>
        public bool CalcularLink()
        {
            bool result = false;

            string mod = GetValueXML("ide", "mod");
            if (mod == "65")
            {
                string value = GetValueXML("ide", "tpImp");
                if (value.Equals("4") || value.Equals("5"))
                {
                    bool naoTemQrCode = string.IsNullOrEmpty(GetValueXML("infNFeSupl", "qrCode").Trim());

                    if (naoTemQrCode)
                        result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Recuperar informações do XML
        /// </summary>
        private void Populate()
        {
            var minhaCultura = new CultureInfo("pt-BR"); //pt-BR usada como base
            minhaCultura.NumberFormat.NumberDecimalSeparator = ".";

            this.CNPJ = GetValueXML("dest", "CNPJ").Trim();
            this.CPF = GetValueXML("dest", "CPF").Trim();
            this.idEstrangeiro = GetValueXML("dest", "idEstrangeiro").Trim();
            this.ChaveAcesso = GetAttributeXML("infNFe", "Id").Substring(3).Trim();
            this.TpAmb = GetValueXML("ide", "tpAmb").Trim();
            this.DhEmi = GetValueXML("ide", "dhEmi").Trim();
            this.vNF = string.Format(minhaCultura, "{0:0.00}", Convert.ToDecimal(GetValueXML("ICMSTot", "vNF").Trim(), minhaCultura));
            this.vICMS = string.Format(minhaCultura, "{0:0.00}", Convert.ToDecimal(GetValueXML("ICMSTot", "vICMS").Trim(), minhaCultura));
            this.digVal = GetValueXML("Reference", "DigestValue").Trim();
        }

        /// <summary>
        /// Validar se os dados foram encontrados no XML
        /// </summary>
        private void Validate()
        {
            if (String.IsNullOrEmpty(this.ChaveAcesso))
                throw new Exceptions.FalhaLinkQRCode(ArquivoXML, "infNFe", "ID");
            if (String.IsNullOrEmpty(this.TpAmb))
                throw new Exceptions.FalhaLinkQRCode(ArquivoXML, "ide", "tpAmb");
            if (String.IsNullOrEmpty(this.DhEmi))
                throw new Exceptions.FalhaLinkQRCode(ArquivoXML, "ide", "dhEmi");
            if (String.IsNullOrEmpty(this.vNF))
                throw new Exceptions.FalhaLinkQRCode(ArquivoXML, "ICMSTot", "vNF");
            if (String.IsNullOrEmpty(this.vICMS))
                throw new Exceptions.FalhaLinkQRCode(ArquivoXML, "ICMSTot", "vICMS");
            if (String.IsNullOrEmpty(this.digVal))
                throw new Exceptions.FalhaLinkQRCode(ArquivoXML, "Reference", "DigestValue");
        }

        /// <summary>
        /// Recupera as informações no XML
        /// </summary>
        /// <param name="elementTag">Grupo de tag</param>
        /// <param name="valueTag">Elmento que deseja retornar o valor</param>
        /// <returns>Valor do elemento</returns>
        private string GetValueXML(string elementTag, string valueTag)
        {
            string value = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(this.ArquivoXML);
            XmlNodeList nodes = doc.GetElementsByTagName(elementTag);

            if (nodes.Count > 0)
            {
                XmlNode node = nodes[0];

                foreach (XmlNode n in node)
                {
                    if (n.NodeType == XmlNodeType.Element)
                    {
                        if (n.Name.Equals(valueTag))
                        {
                            value = n.InnerText;
                            break;
                        }
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Recuperar valor de um atributo do XML
        /// </summary>
        /// <param name="node">Atribute</param>
        /// <param name="attribute">Nome do atributo</param>
        /// <returns></returns>
        private string GetAttributeXML(string node, string attribute)
        {
            string result = "";

            XmlDocument doc = new XmlDocument();
            doc.Load(ArquivoXML);
            XmlElement elementos = (XmlElement)doc.GetElementsByTagName(node)[0];
            result = elementos.GetAttribute(attribute);

            return result;
        }
    }
        #endregion
}


using System;
using System.Globalization;
using System.Xml;

namespace NFe.Components.QRCode
{
    public class QRCode
    {
        #region Propriedades

        #region Proriedades de resultado do processamento

        public XmlDocument ConteudoXML;

        #endregion Proriedades de resultado do processamento

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

        #endregion Recupera dados do XML da NFCe

        #region Valores para montar o HASH e o LINK

        protected string ParametrosQR = null;
        protected string HashQRCode = null;
        protected string ParametrosLinkConsulta = null;

        #endregion Valores para montar o HASH e o LINK

        #endregion Propriedades

        #region Constrututor

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML</param>
        public QRCode(XmlDocument conteudoXML)
        {
            ConteudoXML = conteudoXML;
        }

        #endregion Constrututor

        #region Metodos

        /// <summary>
        /// Gerar o Hash do QRCode e montar o Link para inseri-lo no XML
        /// </summary>
        /// <param name="linkUF">Link de consulta da UF</param>
        /// <param name="identificadorCSC">CSC</param>
        /// <param name="tokenCSC"></param>
        /// <param name="linkUFManual">Link de consulta manual da UF, usado na NFCe</param>
        public void GerarLinkConsulta(string linkUF, string identificadorCSC, string tokenCSC, string linkUFManual)
        {
            if (!CalcularLink())
                return;

            Populate();

            ParametrosQR = "chNFe=" + ChaveAcesso +
                "&nVersao=100" +
                "&tpAmb=" + TpAmb +
                (String.IsNullOrEmpty(CNPJ) ? (String.IsNullOrEmpty(CPF) ? (String.IsNullOrEmpty(idEstrangeiro) ? "" : "&cDest=" + idEstrangeiro) : "&cDest=" + CPF) : "&cDest=" + CNPJ) +
                "&dhEmi=" + Functions.ComputeHexadecimal(DhEmi) +
                "&vNF=" + vNF +
                "&vICMS=" + vICMS +
                "&digVal=" + Functions.ComputeHexadecimal(digVal) +
                "&cIdToken=" + tokenCSC;

            HashQRCode = Criptografia.GetSHA1HashData(ParametrosQR + identificadorCSC, true);

            ParametrosLinkConsulta = linkUF + "?" + ParametrosQR.Trim() + "&cHashQRCode=" + HashQRCode.Trim();

            AddLinkQRCode(linkUFManual);
        }

        /// <summary>
        /// Adicionar link gerado ao XML
        /// </summary>
        private void AddLinkQRCode(string linkUFManual)
        {
            foreach (var item in ConteudoXML)
            {
                if (typeof(XmlElement) == item.GetType())
                {
                    XmlNode Signature = (XmlElement)ConteudoXML.GetElementsByTagName("Signature")[0];
                    XmlNode el = item as XmlNode;
                    XmlNode nd = ConteudoXML.CreateElement("infNFeSupl", ConteudoXML.DocumentElement.NamespaceURI);
                    XmlNode nd1 = ConteudoXML.CreateElement("qrCode", ConteudoXML.DocumentElement.NamespaceURI);
                    nd1.InnerXml = ("<![CDATA[" + this.ParametrosLinkConsulta.Trim() + "]]>").Trim();
                    nd.AppendChild(nd1);

                    if (((XmlElement)ConteudoXML.GetElementsByTagName("infNFe")[0]).GetAttribute("versao").Equals("4.00"))
                    {
                        XmlNode urlChave = ConteudoXML.CreateElement("urlChave", ConteudoXML.DocumentElement.NamespaceURI);
                        urlChave.InnerXml = linkUFManual;
                        nd.AppendChild(urlChave);
                    }

                    el.RemoveChild(Signature);
                    el.AppendChild(nd);
                    el.AppendChild(Signature);

                    break;
                }
            }
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

            CNPJ = GetValueXML("dest", "CNPJ").Trim();
            CPF = GetValueXML("dest", "CPF").Trim();
            idEstrangeiro = GetValueXML("dest", "idEstrangeiro").Trim();
            ChaveAcesso = GetAttributeXML("infNFe", "Id").Substring(3).Trim();
            TpAmb = GetValueXML("ide", "tpAmb").Trim();
            DhEmi = GetValueXML("ide", "dhEmi").Trim();
            vNF = string.Format(minhaCultura, "{0:0.00}", Convert.ToDecimal(GetValueXML("ICMSTot", "vNF").Trim(), minhaCultura));
            vICMS = string.Format(minhaCultura, "{0:0.00}", Convert.ToDecimal(GetValueXML("ICMSTot", "vICMS").Trim(), minhaCultura));
            digVal = GetValueXML("Reference", "DigestValue").Trim();
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
            XmlNodeList nodes = ConteudoXML.GetElementsByTagName(elementTag);

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

            XmlElement elementos = (XmlElement)ConteudoXML.GetElementsByTagName(node)[0];
            result = elementos.GetAttribute(attribute);

            return result;
        }
    }

    #endregion Metodos
}
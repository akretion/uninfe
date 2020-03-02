using System.Security.Cryptography.Xml;
using System.Xml;

namespace NFe.Components.EloTech.WS
{
    internal sealed class SignedXmlWithId: SignedXml
    {
        #region Public Constructors

        public SignedXmlWithId(XmlDocument xml)
            : base(xml)
        {
        }

        public SignedXmlWithId(XmlElement xmlElement)
            : base(xmlElement)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // check to see if it's a standard ID reference
            var idElem = base.GetIdElement(doc, id);

            if(idElem == null)
            {
                var nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

                idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
            }

            return idElem;
        }

        #endregion Public Methods
    }
}
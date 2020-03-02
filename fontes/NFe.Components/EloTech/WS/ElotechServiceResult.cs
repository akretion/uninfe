using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.Components.EloTech.WS
{
    [XmlRoot(ElementName = "MensagemRetorno")]
    public sealed class ElotechServiceResult
    {
        #region Public Properties

        [XmlElement(ElementName = "Codigo")]
        public string Codigo { get; set; }

        [XmlElement(ElementName = "Correcao")]
        public string Correcao { get; set; }

        [XmlElement(ElementName = "Mensagem")]
        public string Mensagem { get; set; }

        [XmlIgnore]
        public string Xml { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator ElotechServiceResult(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var elements = doc.GetElementsByTagName("ListaMensagemRetorno");

            if(elements.Count == 0)
            {
                return new ElotechServiceResult
                {
                    Xml = xml
                };
            }

            using(var stream = new StringReader(elements[0].InnerXml
                                                           .Replace("http://shad.elotech.com.br/schemas/iss/nfse_v2_03.xsd", "")))
            {
                var infoSerializer = new XmlSerializer(typeof(ElotechServiceResult));
                var result = infoSerializer.Deserialize(stream);
                stream.Close();
                return (ElotechServiceResult)result;
            }
        }

        public static implicit operator Exception(ElotechServiceResult result) => new Exception(result.ToString());

        public override string ToString() => $"Codigo..: {Codigo}{Environment.NewLine}" +
                                             $"Mensagem: {Mensagem}{Environment.NewLine}" +
                                             $"Correcao: {Correcao}{Environment.NewLine}";

        #endregion Public Methods
    }
}
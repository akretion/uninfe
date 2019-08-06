using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Xml.NFe
{
    [Serializable()]
    [XmlRoot("procInutNFe", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class ProcInutNFe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("inutNFe")]
        public InutNFe InutNFe { get; set; }

        [XmlElement("retInutNFe")]
        public RetInutNFe RetInutNFe { get; set; }

        /// <summary>
        /// Nome do arquivo de distribuição
        /// </summary>
        [XmlIgnore]
        public string NomeArquivoDistribuicao => InutNFe.InfInut.Id.Substring(2, InutNFe.InfInut.Id.Length - 2) + "-procinutnfe.xml";

        public override XmlDocument GerarXML()
        {
            XmlDocument xmlDocument = base.GerarXML();

            XmlRootAttribute attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
            XmlElement xmlElementNFe = (XmlElement)xmlDocument.GetElementsByTagName("inutNFe")[0];
            xmlElementNFe.SetAttribute("xmlns", attribute.Namespace);
            XmlElement xmlElementProtNFe = (XmlElement)xmlDocument.GetElementsByTagName("retInutNFe")[0];
            xmlElementProtNFe.SetAttribute("xmlns", attribute.Namespace);

            return xmlDocument;
        }
    }
}

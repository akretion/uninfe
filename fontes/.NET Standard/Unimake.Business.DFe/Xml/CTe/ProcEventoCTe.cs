using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Xml.CTe
{
    [Serializable()]
    [XmlRoot("procEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class ProcEventoCTe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("eventoCTe")]
        public EventoCTe EventoCTe { get; set; }

        [XmlElement("retEventoCTe")]
        public RetEventoCTe RetEventoCTe { get; set; }
         
        /// <summary>
        /// Nome do arquivo de distribuição
        /// </summary>
        [XmlIgnore]
        public string NomeArquivoDistribuicao => EventoCTe.InfEvento.ChCTe + "_" + ((int)EventoCTe.InfEvento.TpEvento).ToString("000000") + "_" + EventoCTe.InfEvento.NSeqEvento.ToString("00") + "-proceventocte.xml";

        public override XmlDocument GerarXML()
        {
            var xmlDocument = base.GerarXML();

            XmlRootAttribute attribute = GetType().GetCustomAttribute<XmlRootAttribute>();

            XmlElement xmlElementEvento = (XmlElement)xmlDocument.GetElementsByTagName("eventoCTe")[0];
            xmlElementEvento.SetAttribute("xmlns", attribute.Namespace);

            XmlElement xmlElementRetEvento = (XmlElement)xmlDocument.GetElementsByTagName("retEventoCTe")[0];
            xmlElementRetEvento.SetAttribute("xmlns", attribute.Namespace);

            XmlElement xmlElementRetEventoInfEvento = (XmlElement)xmlElementRetEvento.GetElementsByTagName("infEvento")[0];
            xmlElementRetEventoInfEvento.SetAttribute("xmlns", attribute.Namespace);

            return xmlDocument;
        }
    }
}

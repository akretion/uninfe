#pragma warning disable CS1591

using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Xml.NFe
{
    [Serializable()]
    [XmlRoot("procEventoNFe", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class ProcEventoNFe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("evento")]
        public Evento Evento { get; set; }

        [XmlElement("retEvento")]
        public RetEvento RetEvento { get; set; }

        /// <summary>
        /// Nome do arquivo de distribuição
        /// </summary>
        [XmlIgnore]
        public string NomeArquivoDistribuicao => Evento.InfEvento.ChNFe + "_" + ((int)Evento.InfEvento.TpEvento).ToString("000000") + "_" + Evento.InfEvento.NSeqEvento.ToString("00") + "-proceventonfe.xml";

        public override XmlDocument GerarXML()
        {
            var xmlDocument = base.GerarXML();

            XmlRootAttribute attribute = GetType().GetCustomAttribute<XmlRootAttribute>();

            XmlElement xmlElementEvento = (XmlElement)xmlDocument.GetElementsByTagName("evento")[0];
            xmlElementEvento.SetAttribute("xmlns", attribute.Namespace);

            XmlElement xmlElementRetEvento = (XmlElement)xmlDocument.GetElementsByTagName("retEvento")[0];
            xmlElementRetEvento.SetAttribute("xmlns", attribute.Namespace);

            XmlElement xmlElementRetEventoInfEvento = (XmlElement)xmlElementRetEvento.GetElementsByTagName("infEvento")[0];
            xmlElementRetEventoInfEvento.SetAttribute("xmlns", attribute.Namespace);

            return xmlDocument;
        }
    }
}

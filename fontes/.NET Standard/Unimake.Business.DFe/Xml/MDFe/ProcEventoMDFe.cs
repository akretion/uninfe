﻿#pragma warning disable CS1591

using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Xml.MDFe
{
    [Serializable()]
    [XmlRoot("procEventoMDFe", Namespace = "http://www.portalfiscal.inf.br/mdfe", IsNullable = false)]
    public class ProcEventoMDFe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("eventoMDFe")]
        public EventoMDFe EventoMDFe { get; set; }

        [XmlElement("retEventoMDFe")]
        public RetEventoMDFe RetEventoMDFe { get; set; }
         
        /// <summary>
        /// Nome do arquivo de distribuição
        /// </summary>
        [XmlIgnore]
        public string NomeArquivoDistribuicao => EventoMDFe.InfEvento.ChMDFe + "_" + ((int)EventoMDFe.InfEvento.TpEvento).ToString("000000") + "_" + EventoMDFe.InfEvento.NSeqEvento.ToString("00") + "-proceventomdfe.xml";

        public override XmlDocument GerarXML()
        {
            var xmlDocument = base.GerarXML();

            XmlRootAttribute attribute = GetType().GetCustomAttribute<XmlRootAttribute>();

            XmlElement xmlElementEvento = (XmlElement)xmlDocument.GetElementsByTagName("eventoMDFe")[0];
            xmlElementEvento.SetAttribute("xmlns", attribute.Namespace);

            XmlElement xmlElementRetEvento = (XmlElement)xmlDocument.GetElementsByTagName("retEventoMDFe")[0];
            xmlElementRetEvento.SetAttribute("xmlns", attribute.Namespace);

            XmlElement xmlElementRetEventoInfEvento = (XmlElement)xmlElementRetEvento.GetElementsByTagName("infEvento")[0];
            xmlElementRetEventoInfEvento.SetAttribute("xmlns", attribute.Namespace);

            return xmlDocument;
        }
    }
}

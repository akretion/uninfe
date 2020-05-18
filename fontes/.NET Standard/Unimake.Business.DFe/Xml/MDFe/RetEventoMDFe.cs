﻿using System;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.MDFe
{
    [XmlRoot("retEventoMDFe", Namespace = "http://www.portalfiscal.inf.br/mdfe", IsNullable = false)]
    public class RetEventoMDFe
    {
        [XmlElement("infEvento", Order = 0)]
        public RetEventoMDFeInfEvento InfEvento { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/mdfe")]
    public class RetEventoMDFeInfEvento
    {
        [XmlAttribute(AttributeName = "Id", DataType = "ID")]
        public string Id { get; set; }

        [XmlElement("tpAmb", Order = 0)]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("verAplic", Order = 1)]
        public string VerAplic { get; set; }

        [XmlIgnore]
        public UFBrasil COrgao { get; set; }

        [XmlElement("cOrgao", Order = 2)]
        public int COrgaoField
        {
            get => (int)COrgao;
            set => COrgao = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlElement("cStat", Order = 3)]
        public int CStat { get; set; }

        [XmlElement("xMotivo", Order = 4)]
        public string XMotivo { get; set; }

        [XmlElement("chMDFe", Order = 5)]
        public string ChMDFe { get; set; }

        [XmlElement("tpEvento", Order = 6)]
        public TipoEventoMDFe TpEvento { get; set; }

        [XmlElement("xEvento", Order = 7)]
        public string XEvento { get; set; }

        [XmlElement("nSeqEvento", Order = 8)]
        public int NSeqEvento { get; set; }

        [XmlIgnore]
        public DateTime DhRegEvento { get; set; }

        [XmlElement("dhRegEvento", Order = 12)]
        public string DhRegEventoField
        {
            get => DhRegEvento.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhRegEvento = DateTime.Parse(value);
        }

        [XmlElementAttribute("nProt", Order = 13)]
        public string NProt { get; set; }
    }
}

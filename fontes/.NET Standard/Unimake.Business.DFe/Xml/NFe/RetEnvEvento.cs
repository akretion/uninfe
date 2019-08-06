using System;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [XmlRoot("retEnvEvento", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class RetEnvEvento
    {
        [XmlElement("idLote", Order = 0)]
        public string IdLote { get; set; }

        [XmlElement("tpAmb", Order = 1)]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("verAplic", Order = 2)]
        public string VerAplic { get; set; }

        [XmlIgnore]
        public UFBrasil COrgao { get; set; }

        [XmlElement("cOrgao", Order = 3)]
        public int COrgaoField
        {
            get => (int)COrgao;
            set => COrgao = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlElement("cStat", Order = 4)]
        public int CStat { get; set; }

        [XmlElement("xMotivo", Order = 5)]
        public string XMotivo { get; set; }

        [XmlElement("retEvento", Order = 6)]
        public RetEvento[] RetEvento { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class RetEvento
    {
        [XmlElement("infEvento", Order = 0)]
        public InfEventoRetEvento InfEvento { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfEventoRetEvento
    {
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

        [XmlElement("chNFe", Order = 5)]
        public string ChNFe { get; set; }

        [XmlElement("tpEvento", Order = 6)]
        public TipoEventoNFe TpEvento { get; set; }

        [XmlElement("XEvento", Order = 7)]
        public string XEvento { get; set; }

        [XmlElement("nSeqEvento", Order = 8)]
        public int NSeqEvento { get; set; }

        [XmlElement("CNPJDest", Order = 9)]
        public string CNPJDest { get; set; }

        [XmlElement("CPFDest", Order = 10)]
        public string CPFDest { get; set; }

        [XmlElement("emailDest", Order = 11)]
        public string EmailDest { get; set; }

        [XmlIgnore]
        public DateTime DhRegEvento { get; set; }

        [XmlElement("dhRegEvento", Order = 12)]
        public string DhRegEventoField
        {
            get => DhRegEvento.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhRegEvento = DateTime.Parse(value);
        }

        [XmlElementAttribute("nProt", Order = 13)]
        public string NProt { get; set; }

        [XmlAttribute(DataType = "ID")]
        public string Id { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJDest()
        {
            return !string.IsNullOrWhiteSpace(CNPJDest);
        }

        public bool ShouldSerializeCPFDest()
        {
            return !string.IsNullOrWhiteSpace(CPFDest);
        }

        #endregion
    }
}

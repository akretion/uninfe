using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [XmlRoot("envEvento", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class EnvEvento : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public string IdLote { get; set; }

        [XmlElement("evento")]
        public EnvEventoEvento[] Evento { get; set; }
    }

    //TODO: WANDREY - Não está gerando o Namespace neste ponto do XML, ou seja, na tag <evento>
    [XmlRoot("evento", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class EnvEventoEvento
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("infEvento")]
        public EnvEventoEventoInfEvento InfEvento { get; set; }
    }

    [XmlRoot("infEvento", IsNullable = false)]
    public class EnvEventoEventoInfEvento
    {
        private string IdField;

        [XmlAttribute(DataType = "ID")]
        public string Id
        {
            get => "ID" + ((int)TpEvento).ToString() + ChNFe + NSeqEvento.ToString("00");
            set => IdField = value;
        }

        [XmlElement("cOrgao")]
        public OrgaoIBGE COrgao { get; set; }

        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("chNFe")]
        public string ChNFe { get; set; }

        [XmlElement("dhEvento")]
        public string DhEvento { get; set; }

        [XmlElement("tpEvento")]
        public TipoEventoNFe TpEvento { get; set; }

        [XmlElement("nSeqEvento")]
        public int NSeqEvento { get; set; }

        [XmlElement("verEvento")]
        public string VerEvento { get; set; }

        [XmlElement("detEvento")]
        public EnvEventoEventoInfEventoDetEvento DetEvento { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }
        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrWhiteSpace(CPF);
        }

        #endregion
    }

    [XmlRoot("detEvento", IsNullable = false)]
    public class EnvEventoEventoInfEventoDetEvento
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("descEvento")]
        public string DescEvento { get; set; } = "Cancelamento";

        [XmlElement("nProt")]
        public string NProt { get; set; }

        [XmlElement("xJust")]
        public string XJust { get; set; }
    }
}
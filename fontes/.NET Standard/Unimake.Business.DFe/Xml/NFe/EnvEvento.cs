using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [System.Serializable()]
    [XmlRoot("envEvento", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class EnvEvento : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public string IdLote { get; set; }

        [XmlElement("evento")]
        public Evento[] Evento { get; set; }

        public override XmlDocument GerarXML()
        {
            XmlDocument xmlDocument = base.GerarXML();

            XmlRootAttribute attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
            XmlElement xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("evento")[0];
            xmlElement.SetAttribute("xmlns", attribute.Namespace);

            return xmlDocument;
        }
    }

    [System.Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Evento
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("infEvento")]
        public InfEvento InfEvento { get; set; }
    }

    [System.Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfEvento
    {
        private string IdField;

        [XmlAttribute(DataType = "ID")]
        public string Id
        {
            get => "ID" + ((int)TpEvento).ToString() + ChNFe + NSeqEvento.ToString("00");
            set => IdField = value;
        }

        [XmlIgnore]
        public UFBrasil COrgao { get; set; }

        [XmlElement("cOrgao")]
        public int COrgaoField
        {
            get => (int)COrgao;
            set => COrgao = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }
        
        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("chNFe")]
        public string ChNFe { get; set; }

        [XmlIgnore]
        public DateTime DhEvento { get; set; }

        [XmlElement("dhEvento")]
        public string DhEventoField
        {
            get => DhEvento.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhEvento = DateTime.Parse(value);
        }

        [XmlElement("tpEvento")]
        public TipoEventoNFe TpEvento { get; set; }

        [XmlElement("nSeqEvento")]
        public int NSeqEvento { get; set; }

        [XmlElement("verEvento")]
        public string VerEvento { get; set; }

        [XmlElement("detEvento")]
        public DetEvento DetEvento { get; set; }

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

    [System.Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DetEvento
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
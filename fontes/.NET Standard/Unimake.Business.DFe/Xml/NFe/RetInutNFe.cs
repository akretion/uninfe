using System;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{

    [XmlRoot("ConsCad", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class RetInutNFe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement(ElementName = "infInut")]
        public InfInut infInut = new InfInut();
    }

    public partial class InfInut
    {     
        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("verAplic")]
        public string VerAplic { get; set; }

        [XmlElement("cStat")]
        public int CStat { get; set; }

        [XmlElement("xMotivo")]
        public string XMotivo { get; set; }

        [XmlElement("cUF")]
        public UFBrasil CUF { get; set; }

        [XmlElement("ano")]
        public string Ano { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("TMod")]
        public ModeloDFe TMod { get; set; }

        [XmlElement("serie")]
        public bool Serie { get; set; }

        [XmlElement("nNFIni")]
        public string NNFIni { get; set; }

        [XmlElement("nNFFin")]
        public string NNFFin { get; set; }

        [XmlIgnore]
        public DateTime DhRecbto { get; set; }

        [XmlElement("dhRecbto")]
        public string DhRecbtoField
        {
            get => DhRecbto.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhRecbto = DateTime.Parse(value);
        }

        [XmlElement("nProt")]
        public string NProt { get; set; }

        [XmlElement("Id")]
        public string Id { get; set; }
    }

}

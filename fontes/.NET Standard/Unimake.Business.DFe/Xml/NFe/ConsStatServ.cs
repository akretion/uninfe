using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [XmlRoot("consStatServ", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class ConsStatServ : XMLBase
    {
        private UFBrasilIBGE UFField;

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("cUF")]
        public UFBrasilIBGE CUF
        {
            get => UFField;
            set => UFField = value;
        }

        [XmlElement("xServ")]
        public string XServ { get; set; } = "STATUS";
    }
}
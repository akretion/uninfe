using System;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.CTe
{
    [XmlRoot("retConsSitCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class RetConsSitCTe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("verAplic")]
        public string VerAplic { get; set; }

        [XmlElement("cStat")]
        public int CStat { get; set; }

        [XmlElement("xMotivo")]
        public string XMotivo { get; set; }

        [XmlIgnore]
        public UFBrasil CUF { get; set; }

        [XmlElement("cUF")]
        public int CUFField
        {
            get => (int)CUF;
            set => CUF = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlIgnore]
        public DateTime DhRecbto { get; set; }

        [XmlElement("dhRecbto")]
        public string DhRecbtoField
        {
            get => DhRecbto.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhRecbto = DateTime.Parse(value);
        }

        [XmlElement("chCTe")]
        public string ChCTe { get; set; }
        
        [XmlElement("protCTe")]
        public ProtCTe ProtCTe { get; set; }

        //TODO: Wandrey - Quando montar o ProcEventoCTe tem que voltar aqui e finalizar       
        //[XmlElement("procEventoCTe")]
        //public ProcEventoCTe[] procEventoCTe { get; set; }
    }
}

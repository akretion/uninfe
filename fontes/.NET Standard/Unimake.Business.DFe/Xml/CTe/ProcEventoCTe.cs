#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Xml.CTe
{
    //[Serializable()]
    //[XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/cte")]
    //[XmlRoot("procEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    //public class ProcEventoCTeEPEC: ProcEventoCTe<InfEventoEPEC> { }

    //[Serializable()]
    //[XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/cte")]
    //[XmlRoot("procEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    //public class ProcEventoCTePrestDesacordo: ProcEventoCTe<InfEventoPrestDesacordo> { }

    //[Serializable()]
    //[XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/cte")]
    //[XmlRoot("procEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    //public class ProcEventoCTeCanc: ProcEventoCTe<InfEventoCanc> { }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/cte")]
    [XmlRoot("procEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class ProcEventoCTe<TDetalheEvento>: XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }              

        [XmlElement("eventoCTe")]
        public EventoCTe<TDetalheEvento> EventoCTe { get; set; }

        [XmlElement("retEventoCTe")]
        public RetEventoCTe RetEventoCTe { get; set; }

        [XmlAttribute("ipTransmissor")]
        public string IpTransmissor { get; set; }

        [XmlAttribute("nPortaCon")]
        public int NPortaCon { get; set; }

        [XmlIgnore]
        public DateTime DhConexao { get; set; }

        [XmlAttribute("dhConexao")]
        public string DhConexaoField
        {
            get => DhConexao.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhConexao = DateTime.Parse(value);
        }

        /// <summary>
        /// Nome do arquivo de distribuição
        /// </summary>
        [XmlIgnore]
        //TODO WANDREY: Resolver esta encrenca
        public string NomeArquivoDistribuicao => "";

        //public string NomeArquivoDistribuicao => ((IInfEvento)EventoCTe.InfEvento).ChCTe + "_" + ((int)((IInfEvento)EventoCTe.InfEvento).TpEvento).ToString("000000") + "_" + ((IInfEvento)EventoCTe.InfEvento).NSeqEvento.ToString("00") + "-proceventocte.xml";

        public override XmlDocument GerarXML()
        {
            var doc = base.GerarXML();

            var attribute = GetType().GetCustomAttribute<XmlRootAttribute>();

            var elementProcEventoCTe = (XmlElement)doc.GetElementsByTagName("procEventoCTe")[0];
            elementProcEventoCTe.SetAttribute("xmlns", attribute.Namespace);

            var elementEventoCTe = (XmlElement)doc.GetElementsByTagName("eventoCTe")[0];
            elementEventoCTe.SetAttribute("xmlns", attribute.Namespace);

            var elementRetEvento = (XmlElement)doc.GetElementsByTagName("retEventoCTe")[0];
            elementRetEvento.SetAttribute("xmlns", attribute.Namespace);

            return doc;
        }

        #region ShouldSerialize

        public bool ShouldSerializeNPortaCon() => NPortaCon > 0;

        public bool ShouldSerializeIpTransmissor() => !string.IsNullOrWhiteSpace(IpTransmissor);

        public bool ShouldSerializeDhConexaoField() => DhConexao > DateTime.MinValue;

        #endregion
    }
}

﻿using System;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [XmlRoot("distDFeInt", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class DistDFeInt : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlIgnore]
        public UFBrasil CUFAutor { get; set; }

        [XmlElement("cUFAutor")]
        public int CUFAutorField
        {
            get => (int)CUFAutor;
            set => CUFAutor = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlIgnore]
        public readonly UFBrasil COrgao = UFBrasil.AN;

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("consChNFe")]
        public ConsChNFe ConsChNFe { get; set; }

        [XmlElement("consNSU")]
        public ConsNSU ConsNSU { get; set; }

        [XmlElement("distNSU")]
        public DistNSU DistNSU { get; set; }

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

    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ConsChNFe
    {
        [XmlElement("chNFe", DataType = "token")]
        public string ChNFe { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public partial class ConsNSU
    {
        [XmlElement("NSU", DataType = "token")]
        public string NSU { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public partial class DistNSU
    {
        [XmlElement("ultNSU", DataType = "token")]
        public string UltNSU { get; set; }
    }
}

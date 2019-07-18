﻿using System;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [Serializable()]
    [XmlRoot("retConsCad", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class RetConsCad : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("infCons")]
        public InfConsRetorno InfCons { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfConsRetorno
    {
        [XmlElement("verAplic")]
        public string VerAplic { get; set; }

        [XmlElement("cStat")]
        public int CStat { get; set; }

        [XmlElement("xMotivo")]
        public string XMotivo { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlIgnore]
        public DateTime DhCons { get; set; }

        [XmlElement("dhCons")]
        public string DhConsField
        {
            get => DhCons.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhCons = DateTime.Parse(value);
        }

        [XmlIgnore]
        public UFBrasil CUF { get; set; }

        [XmlElement("cUF")]
        public int CUFField
        {
            get => (int)CUF;
            set => CUF = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlElement("infCad")]
        public InfCad InfCad { get; set; }

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

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public partial class InfCad
    {
        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("cSit")]
        public int CSit { get; set; }

        [XmlElement("indCredNFe")]
        public int IndCredNFe { get; set; }

        [XmlElement("indCredCTe")]
        public int IndCredCTe { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("xFant")]
        public string XFant { get; set; }

        [XmlElement("xRegApur")]
        public string XRegApur { get; set; }

        [XmlElement("CNAE")]
        public string CNAE { get; set; }

        [XmlIgnore]
        public DateTime DIniAtiv { get; set; }

        [XmlElement("dIniAtiv")]
        public string DIniAtivField
        {
            get => DIniAtiv.ToString("yyyy-MM-dd");
            set => DIniAtiv = DateTime.Parse(value);
        }
                     
        [XmlIgnore]
        public DateTime DUltSit { get; set; }

        [XmlElement("dUltSit")]
        public string DUltSitField
        {
            get => DUltSit.ToString("yyyy-MM-dd");
            set => DUltSit = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime DBaixa { get; set; }

        [XmlElement("dBaixa")]
        public string DBaixaField
        {
            get => DBaixa.ToString("yyyy-MM-dd");
            set => DBaixa = DateTime.Parse(value);
        }

        [XmlElement("IEUnica")]
        public string IEUnica { get; set; }

        [XmlElement("IEAtual")]
        public string IEAtual { get; set; }

        [XmlElement("ender")]
        public Ender Ender { get; set; }

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

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public partial class Ender
    {
        [XmlElement("xLgr")]
        public string XLgr { get; set; }

        [XmlElement("nro")]
        public string Nro { get; set; }

        [XmlElement("xCpl")]
        public string XCpl { get; set; }

        [XmlElement("xBairro")]
        public string XBairro { get; set; }

        [XmlElement("cMun")]
        public int CMun { get; set; }

        [XmlElement("xMun")]
        public string XMun { get; set; }

        [XmlElement("CEP")]
        public string CEP { get; set; }
    }

}
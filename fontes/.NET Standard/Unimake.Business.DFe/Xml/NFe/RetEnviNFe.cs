﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [System.Serializable()]
    [XmlRoot("retEnviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class RetEnviNFe : XMLBase
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

        [XmlElement("infRec")]
        public RetEnviNFeInfRec InfRec { get; set; }

        [XmlElement("protNFe")]
        public ProtNFe ProtNFe { get; set; }
    }
    public class RetEnviNFeInfRec
    {
        [XmlElement("nRec")]
        public string NRec { get; set; }

        [XmlElement("tMed")]
        public string TMed { get; set; }
    }
}

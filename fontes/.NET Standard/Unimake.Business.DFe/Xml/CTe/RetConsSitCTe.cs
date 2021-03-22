#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.CTe
{
    [XmlRoot("retConsSitCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class RetConsSitCTe: XMLBase
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

        [XmlElement("protCTe")]
        public ProtCTe ProtCTe { get; set; }

        //TODO WANDREY: Resolver esta encrenca
        //[XmlElement("procEventoCTe")]
        [XmlArrayItem(typeof(ProcEventoCTe<DetEventoCanc>))]
        [XmlArrayItem(typeof(ProcEventoCTe<DetEventoEPEC>))]
        public List<ProcEventoCTe<DetEventoCanc>> ProcEventoCTe { get; set; }

        #region Add (List - Interop)

        //TODO WANDREY: Resolver esta encrenca
        //public void AddProcEventoCTe(ProcEventoCTe procEventoCTe)
        //{
        //    if(ProcEventoCTe == null)
        //    {
        //        ProcEventoCTe = new List<ProcEventoCTe>();
        //    }

        //    ProcEventoCTe.Add(procEventoCTe);
        //}

        #endregion
    }
}

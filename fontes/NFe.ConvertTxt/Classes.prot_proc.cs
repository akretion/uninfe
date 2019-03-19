using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFe.Components;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// procRef
    /// </summary>
    public class procRef
    {
        public string nProc;
        public string indProc;
    }

    public class protNFe
    {
        public TipoAmbiente tpAmb;
        public string verAplic;
        public string chNFe;
        public DateTime dhRecbto;
        public string nProt;
        public string digVal;
        public int cStat;
        public string xMotivo;
        public int cMsg { get; set; }
        public string xMsg { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// Adi
    /// </summary>
    public class Adi
    {
        public int nAdicao;
        public int nSeqAdi;
        public string cFabricante;
        public double vDescDI;
        public string nDraw;
    }

    /// <summary>
    /// DI
    /// </summary>
    public class DI
    {
        public string nDI;
        public DateTime dDI;
        public string xLocDesemb;
        public string UFDesemb;
        public DateTime dDesemb;
        public string cExportador;
        public TpcnTipoViaTransp tpViaTransp;
        public double vAFRMM;
        public TpcnTipoIntermedio tpIntermedio;
        public string CNPJ;
        public string UFTerceiro;
        public List<Adi> adi;

        public DI()
        {
            adi = new List<Adi>();
        }
    }
}

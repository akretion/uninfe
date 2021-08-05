using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// pag - NFC-e
    /// </summary>
    public class pag
    {
        public TpcnIndicadorPagamento indPag = TpcnIndicadorPagamento.ipNone;
        public TpcnFormaPagamento tPag = TpcnFormaPagamento.fpOutro;
        public string xPag;
        public double vPag;
        public string CNPJ;
        public TpcnBandeiraCartao tBand;
        public string cAut;
        public int tpIntegra;

        public pag()
        {
            this.cAut = this.CNPJ = string.Empty;
        }
    }

}

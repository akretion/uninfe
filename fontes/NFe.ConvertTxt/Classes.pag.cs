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
        public TpcnIndicadorPagamento indPag = TpcnIndicadorPagamento.ipOutras;
        public TpcnFormaPagamento tPag = TpcnFormaPagamento.fpOutro;
        public double vPag;
        public string CNPJ;
        public TpcnBandeiraCartao tBand;
        public string cAut;
        public int tpIntegra;
        public double vTroco; //Somente para manter compatibilidade do layout TXT, de futuro, em uma mudança geral da SEFAZ, podemos retirar. Wandrey 26/01/2021

        public pag()
        {
            this.cAut = this.CNPJ = string.Empty;
        }
    }

}

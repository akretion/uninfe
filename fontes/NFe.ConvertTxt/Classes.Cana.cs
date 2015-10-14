using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// Cana
    /// </summary>
    public class Cana
    {
        public string safra;
        public string Ref;
        public double qTotMes;
        public TpcnTipoCampo qTotMes_Tipo;
        public double qTotAnt;
        public TpcnTipoCampo qTotAnt_Tipo;
        public double qTotGer;
        public TpcnTipoCampo qTotGer_Tipo;
        public double vFor;
        public double vTotDed;
        public double vLiqFor;
        public List<fordia> fordia;
        public List<deduc> deduc;

        public Cana()
        {
            qTotMes_Tipo = TpcnTipoCampo.tcDec10;
            qTotAnt_Tipo = TpcnTipoCampo.tcDec10;
            qTotGer_Tipo = TpcnTipoCampo.tcDec10;
            fordia = new List<fordia>();
            deduc = new List<deduc>();
        }
    }
}

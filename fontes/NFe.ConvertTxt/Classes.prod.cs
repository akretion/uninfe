using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// Prod
    /// </summary>
    public class Prod
    {
        public string cProd;
        public int nItem;
        public string cEAN;
        public string cBarra;
        public string xProd;
        public string NCM;
        public string NVE;
        public string EXTIPI;
        public string CFOP;
        public string uCom;
        public double qCom;
        public double vUnCom;
        public TpcnTipoCampo vUnCom_Tipo;
        public double vProd;
        public string cEANTrib;
        public string cBarraTrib;
        public string uTrib;
        public double qTrib;
        public double vUnTrib;
        public TpcnTipoCampo vUnTrib_Tipo;
        public double vFrete;
        public double vSeg;
        public double vDesc;
        public double vOutro;
        public TpcnIndicadorTotal indTot;
        public string xPed;
        public int nItemPed;
        public string nFCI;
        public List<DI> DI;
        public veicProd veicProd;
        public List<Med> med;
        public List<Arma> arma;
        public Comb comb;
        public string nRECOPI;
        public List<detExport> detExport;
        public int CEST;
        public TpcnIndicadorEscala indEscala { get; set; }
        public string CNPJFab { get; set; }
        public string cBenef { get; set; }
        public List<Rastro> rastro { get; set; }

        public Prod()
        {
            vUnCom_Tipo = TpcnTipoCampo.tcDec10;
            vUnTrib_Tipo = TpcnTipoCampo.tcDec10;
            indTot = TpcnIndicadorTotal.itSomaTotalNFe;
            DI = new List<DI>();
            med = new List<Med>();
            arma = new List<Arma>();
            detExport = new List<detExport>();
            rastro = new List<ConvertTxt.Rastro>();
        }
    }
}

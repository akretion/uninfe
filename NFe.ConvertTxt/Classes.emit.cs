using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// Emit
    /// </summary>
    public class Emit
    {
        public string CNPJ;
        public string CPF;
        public string xNome;
        public string xFant;
        public enderEmit enderEmit;
        public string IE;
        public string IEST;
        public string IM;
        public string CNAE;
        public TpcnCRT CRT;

        public Emit()
        {
            this.CNPJ = this.CPF = string.Empty;
            this.CRT = TpcnCRT.crtRegimeNormal;
            this.enderEmit = new enderEmit();
        }
    }

    /// <summary>
    /// enderEmit
    /// </summary>
    public class enderEmit
    {
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public int cMun;
        public string xMun;
        public string UF;
        public int CEP;
        public int cPais;
        public string xPais;
        public string fone;

        public enderEmit()
        {
            this.fone = this.nro = this.UF = this.xBairro = this.xCpl = this.xLgr = this.xMun = this.xPais = "";
        }
    }
}

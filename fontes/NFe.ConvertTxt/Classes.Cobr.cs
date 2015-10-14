using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// Cobr
    /// </summary>
    public class Cobr
    {
        public Fat Fat;
        public List<Dup> Dup;

        public Cobr()
        {
            Dup = new List<Dup>();
        }
    }

    /// <summary>
    /// Dup
    /// </summary>
    public class Dup
    {
        public string nDup;
        public DateTime dVenc;
        public double vDup;
    }

    /// <summary>
    /// Fat
    /// </summary>
    public struct Fat
    {
        public string nFat;
        public double vOrig;
        public double vDesc;
        public double vLiq;
    }

}

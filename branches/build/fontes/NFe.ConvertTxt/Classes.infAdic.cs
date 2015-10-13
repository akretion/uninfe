﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// InfAdic
    /// </summary>
    public class InfAdic
    {
        public string infAdFisco;
        public string infCpl;
        public List<obsCont> obsCont;
        public List<obsFisco> obsFisco;
        public List<procRef> procRef;

        public InfAdic()
        {
            obsCont = new List<obsCont>();
            obsFisco = new List<obsFisco>();
            procRef = new List<procRef>();
        }
    }
}

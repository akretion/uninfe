﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// Retirada
    /// </summary>
    public class Retirada
    {
        public string CNPJ;
        public string CPF;
        public string xNome { get; set; }
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public int cMun;
        public string xMun;
        public string UF;
        public string CEP { get; set; }
        public int cPais { get; set; }
        public string xPais { get; set; }
        public string fone { get; set; }
        public string email { get; set; }
        public string IE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Interface
{
    public static class PrintDANFE
    {
        public static void printDANFE(string NomeArqXMLNFe)
        {
            NFe.Service.TFunctions.ExecutaUniDanfe(NomeArqXMLNFe, DateTime.Today, "");
        }
    }
}

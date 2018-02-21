using NFe.Components;
using System;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// infNFe
    /// </summary>
    public struct infNFe
    {
        public string ID;
        public decimal Versao;
    }

    public class qrCode
    {
        public string chNFe { get; set; }
        public string nVersao { get; set; }
        public TipoAmbiente tpAmb { get; set; }
        public string cDest { get; set; }
        public DateTime dhEmi { get; set; }
        public decimal vNF { get; set; }
        public decimal vICMS { get; set; }
        public string digVal { get; set; }
        public string cIdToken { get; set; }
        public string cHashQRCode { get; set; }
        public string Link { get; set; }
    }
}

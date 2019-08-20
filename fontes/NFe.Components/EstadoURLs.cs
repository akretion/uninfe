namespace NFe.Components
{
    public class EstadoURLConsultaDFe
    {
        public string UF { get; set; }

        #region URL Consulta via QRCode da NFCe

        /// <summary>
        /// URL para consulta via QRCode da NFCe - Produção
        /// </summary>
        public string UrlNFCe { get; set; }

        /// <summary>
        /// URL para consulta via QRCode do CTe- Produção
        /// </summary>
        public string UrlCTeQrCodeP { get; set; }

        /// <summary>
        /// URL para consulta via QRCode do CTe- Homologação
        /// </summary>
        public string UrlCTeQrCodeH { get; set; }

        /// <summary>
        /// URL para consulta via QRCode da NFCe- Homologação
        /// </summary>
        public string UrlNFCeH { get; set; }

        #endregion URL Consulta via QRCode da NFCe

        #region URL Consulta Manual da NFCe

        /// <summary>
        /// URL para consulta manual da NFCe - Produção
        /// </summary>
        public string UrlNFCeM { get; set; }

        /// <summary>
        /// URL para consulta manual da NFCe - Homologação
        /// </summary>
        public string UrlNFCeMH { get; set; }

        #endregion URL Consulta Manual da NFCe

        #region URL Consulta via QRCode da NFCe para versão 4.00

        /// <summary>
        /// URL para consulta via QRCode da NFCe 4.00 - Produção
        /// </summary>
        public string UrlNFCe_400 { get; set; }

        /// <summary>
        /// URL para consulta via QRCode da NFCe 4.00 - Homologação
        /// </summary>
        public string UrlNFCeH_400 { get; set; }

        #endregion URL Consulta via QRCode da NFCe para versão 4.00

    }
}
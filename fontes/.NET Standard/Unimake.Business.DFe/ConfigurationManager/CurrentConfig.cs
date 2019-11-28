using Unimake.Business.DFe.ConfigurationManager.Contract;

namespace Unimake.Business.DFe.ConfigurationManager
{
    public static class CurrentConfig
    {
        #region Private Fields

        private static IConfiguration _configuration;

        #endregion Private Fields

        #region Public Properties

        public static Servicos.DFE DFE { get; set; }

        #endregion

        #region Private Methods

        private static IConfiguration GetConfiguration()
        {
            if (_configuration == null)
                _configuration = ConfigurationService.GetFactory()?.Build() ?? new Configuration();

            return _configuration;
        }

        #endregion Private Methods

        #region Public Properties

        public static string ArquivoConfigGeral => GetConfiguration().ArquivoConfigGeral;
        public static string PastaArqConfig => GetConfiguration().PastaArqConfig;
        public static string PastaArqConfigNFe => GetConfiguration().PastaArqConfigNFe;
        public static string PastaArqConfigNFCe => GetConfiguration().PastaArqConfigNFCe;
        public static string PastaSchema => GetConfiguration().PastaSchema;
        public static string PastaSchemaNFe => GetConfiguration().PastaSchemaNFe;
        public static string PastaSchemaNFCe => GetConfiguration().PastaSchemaNFCe;

        #endregion Public Properties
    }
}
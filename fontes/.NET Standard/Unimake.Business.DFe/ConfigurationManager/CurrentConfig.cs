using Unimake.Business.DFe.ConfigurationManager.Contract;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.ConfigurationManager
{
    public static class CurrentConfig
    {
        #region Private Fields

        private static IConfiguration _configuration;

        #endregion Private Fields

        #region Private Methods

        private static IConfiguration GetConfiguration()
        {
            if(_configuration == null)
            {
                _configuration = ConfigurationService.GetFactory()?.Build() ?? new Configuration();
                new LoadEmbeddedResource().Load();
            }

            return _configuration;
        }

        #endregion Private Methods

        #region Public Properties

        public static string ArquivoConfigPadrao => GetConfiguration().ArquivoConfigPadrao;
        public static string ArquivoConfigGeral => GetConfiguration().ArquivoConfigGeral;
        public static string PastaArqConfig => GetConfiguration().PastaArqConfig;
        public static string PastaArqConfigNFCe => GetConfiguration().PastaArqConfigNFCe;
        public static string PastaArqConfigNFe => GetConfiguration().PastaArqConfigNFe;
        public static string PastaArqConfigCTe => GetConfiguration().PastaArqConfigCTe;
        public static string PastaArqConfigCTeOS => GetConfiguration().PastaArqConfigCTeOS;
        public static string PastaArqConfigMDFe => GetConfiguration().PastaArqConfigMDFe;
        public static string PastaSchema => GetConfiguration().PastaSchema;
        public static string PastaSchemaNFCe => GetConfiguration().PastaSchemaNFCe;
        public static string PastaSchemaNFe => GetConfiguration().PastaSchemaNFe;
        public static string PastaSchemaCTe => GetConfiguration().PastaSchemaCTe;
        public static string PastaSchemaCTeOS => GetConfiguration().PastaSchemaCTeOS;
        public static string PastaSchemaMDFe => GetConfiguration().PastaSchemaMDFe;
        public static Servicos.TipoDFe DFE { get; set; }

        #endregion Public Properties
    }
}
using Unimake.Business.DFe.ConfigurationManager.Contract;

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
                _configuration = ConfigurationService.GetFactory()?.Build() ?? new Configuration();

            return _configuration;
        }

        #endregion Private Methods

        #region Public Properties

        public static string ArquivoConfigGeral => GetConfiguration().ArquivoConfigGeral;
        public static string PastaArqConfig => GetConfiguration().PastaArqConfig;
        public static string SchemaPasta => GetConfiguration().SchemaPasta;

        #endregion Public Properties
    }
}
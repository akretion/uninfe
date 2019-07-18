using System;
using Unimake.Business.DFe.ConfigurationManager.Contract;

namespace Unimake.Business.DFe.ConfigurationManager
{
    public static class ConfigurationService
    {
        #region Private Fields

        private static IConfigurationFactory _configurationFactory;

        #endregion Private Fields

        #region Public Methods

        public static IConfigurationFactory GetFactory() => _configurationFactory;

        public static void RegisterFactory(IConfigurationFactory configurationFactory) =>
            _configurationFactory = configurationFactory ?? throw new ArgumentNullException(nameof(configurationFactory));

        #endregion Public Methods
    }
}
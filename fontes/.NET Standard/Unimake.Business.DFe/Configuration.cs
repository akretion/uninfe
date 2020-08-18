namespace Unimake.Business.DFe
{
    public static class Configuration
    {
        #region Public Properties

        public static string ArquivoConfigGeral => NamespaceConfig + ArquivoConfigPadrao;
        public static string ArquivoConfigPadrao => "Config.xml";
        public static string NamespaceConfig => "Unimake.Business.DFe.Servicos.Config.";
        public static string NamespaceSchema => "Unimake.Business.DFe.Xml.Schemas.";

        #endregion Public Properties
    }
}
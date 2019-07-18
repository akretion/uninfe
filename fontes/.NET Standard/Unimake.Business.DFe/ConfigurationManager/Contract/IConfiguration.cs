namespace Unimake.Business.DFe.ConfigurationManager.Contract
{
    public interface IConfiguration
    {
        #region Public Properties

        string ArquivoConfigGeral { get; }
        string PastaArqConfig { get; }
        string SchemaPasta { get; }

        #endregion Public Properties
    }
}
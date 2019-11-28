namespace Unimake.Business.DFe.ConfigurationManager.Contract
{
    public interface IConfiguration
    {
        #region Public Properties

        string ArquivoConfigGeral { get; }
        string PastaArqConfig { get; }
        string PastaArqConfigNFe { get; }
        string PastaArqConfigNFCe { get; }
        string PastaSchema { get; }
        string PastaSchemaNFe { get; }
        string PastaSchemaNFCe { get; }

        #endregion Public Properties
    }
}
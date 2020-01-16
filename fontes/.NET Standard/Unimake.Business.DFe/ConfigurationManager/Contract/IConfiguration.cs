namespace Unimake.Business.DFe.ConfigurationManager.Contract
{
    public interface IConfiguration
    {
        #region Public Properties

        string ArquivoConfigPadrao { get; }
        string ArquivoConfigGeral { get; }
        string PastaArqConfig { get; }
        string PastaArqConfigNFCe { get; }
        string PastaArqConfigNFe { get; }
        string PastaArqConfigCTe { get; }
        string PastaSchema { get; }
        string PastaSchemaNFCe { get; }
        string PastaSchemaNFe { get; }
        string PastaSchemaCTe { get; }

        #endregion Public Properties
    }
}
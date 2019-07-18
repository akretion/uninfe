namespace Unimake.Business.DFe.ConfigurationManager.Contract
{
    public interface IConfigurationFactory
    {
        #region Public Methods

        IConfiguration Build();

        #endregion Public Methods
    }
}
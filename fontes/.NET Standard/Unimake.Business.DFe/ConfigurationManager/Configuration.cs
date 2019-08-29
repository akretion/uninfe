using Unimake.Business.DFe.ConfigurationManager.Contract;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.ConfigurationManager
{
    public class Configuration: IConfiguration
    {
        #region Public Properties

        public string ArquivoConfigGeral => PastaArqConfig + "Config.xml";
        public virtual string PastaArqConfig => @"Servicos\Nfe\Config\";
        public virtual string SchemaPasta => @"Xml\NFe\Schemas\";

        #endregion Public Properties

        #region Public Constructors

        static Configuration()
        {
            new LoadEmbeddedResource().Load();
        }

        #endregion Public Constructors
    }
}
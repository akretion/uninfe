using Unimake.Business.DFe.ConfigurationManager.Contract;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.ConfigurationManager
{
    public class Configuration: IConfiguration
    {
        #region Public Properties

        public string ArquivoConfigGeral => PastaArqConfig + "Config.xml";
        public virtual string PastaArqConfig => @"Servicos\Config\";
        public virtual string PastaArqConfigNFe => PastaArqConfig + Servicos.DFE.NFe.ToString() + @"\";
        public virtual string PastaArqConfigNFCe => PastaArqConfig + Servicos.DFE.NFCe.ToString() + @"\";
        public virtual string PastaSchema => @"Xml\Schemas\";
        public virtual string PastaSchemaNFe => PastaSchema + Servicos.DFE.NFe.ToString() + @"\";
        public virtual string PastaSchemaNFCe => PastaSchemaNFe; //Mesmo da NFe. Wandrey 29/10/2019

        #endregion Public Properties

        #region Public Constructors

        static Configuration()
        {
            try
            {
                new LoadEmbeddedResource().Load();
            }
            catch
            {
                //Marcelo: 
                //Se o contexto for lambda, não é permitido acessar o file system.
                //Neste caso iremos carregar as configurações no Lambda.
                //ignore
            }
        }

        #endregion Public Constructors
    }
}
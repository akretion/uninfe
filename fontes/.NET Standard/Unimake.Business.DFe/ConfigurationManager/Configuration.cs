using Unimake.Business.DFe.ConfigurationManager.Contract;

namespace Unimake.Business.DFe.ConfigurationManager
{
    public class Configuration : IConfiguration
    {
        #region Public Properties

        public string ArquivoConfigPadrao => "Config.xml";
        public string ArquivoConfigGeral => PastaArqConfig + ArquivoConfigPadrao;
        public virtual string PastaArqConfig => @"Servicos\Config\";
        public virtual string PastaArqConfigNFCe => PastaArqConfig + Servicos.DFE.NFCe.ToString() + @"\";
        public virtual string PastaArqConfigNFe => PastaArqConfig + Servicos.DFE.NFe.ToString() + @"\";
        public virtual string PastaArqConfigCTe => PastaArqConfig + Servicos.DFE.CTe.ToString() + @"\";
        public virtual string PastaArqConfigMDFe => PastaArqConfig + Servicos.DFE.MDFe.ToString() + @"\";
        public virtual string PastaSchema => @"Xml\Schemas\";
        public virtual string PastaSchemaNFe => PastaSchema + Servicos.DFE.NFe.ToString() + @"\";
        public virtual string PastaSchemaNFCe => PastaSchemaNFe;
        public virtual string PastaSchemaCTe => PastaSchema + Servicos.DFE.CTe.ToString() + @"\";
        public virtual string PastaSchemaMDFe => PastaSchema + Servicos.DFE.MDFe.ToString() + @"\";

        #endregion Public Properties
    }
}
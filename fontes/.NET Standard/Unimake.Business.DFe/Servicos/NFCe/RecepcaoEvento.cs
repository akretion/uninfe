using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class RecepcaoEvento: NFe.RecepcaoEvento
    {
        #region Public Constructors

        public RecepcaoEvento(EnvEvento envEvento, Configuracao configuracao)
                    : base(envEvento, configuracao) { }

        public RecepcaoEvento()
        {
        }

        #endregion Public Constructors
    }
}
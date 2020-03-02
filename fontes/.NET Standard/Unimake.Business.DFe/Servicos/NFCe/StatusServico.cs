using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class StatusServico: NFe.StatusServico
    {
        #region Public Constructors

        public StatusServico(ConsStatServ consStatServ, Configuracao configuracao)
            : base(consStatServ, configuracao) { }

        public StatusServico()
        {
        }

        #endregion Public Constructors
    }
}
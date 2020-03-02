using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class ConsultaProtocolo: NFe.ConsultaProtocolo
    {
        #region Public Constructors

        public ConsultaProtocolo(ConsSitNFe consSitNFe, Configuracao configuracao)
            : base(consSitNFe, configuracao) { }

        public ConsultaProtocolo()
        {
        }

        #endregion Public Constructors
    }
}
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class ConsultaCadastro: NFe.ConsultaCadastro
    {
        #region Public Constructors

        public ConsultaCadastro(ConsCad consCad, Configuracao configuracao)
            : base(consCad, configuracao) { }

        public ConsultaCadastro()
        {
        }

        #endregion Public Constructors
    }
}
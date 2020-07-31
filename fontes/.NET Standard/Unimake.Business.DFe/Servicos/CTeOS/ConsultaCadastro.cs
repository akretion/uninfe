using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    public class ConsultaCadastro: NFe.ConsultaCadastro
    {
        #region Public Constructors

        public ConsultaCadastro(ConsCadBase consCad, Configuracao configuracao)
            : base(consCad, configuracao) { }

        public ConsultaCadastro()
        {
        }

        #endregion Public Constructors
    }
}
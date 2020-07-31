using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    public class Inutilizacao: CTe.Inutilizacao
    {
        #region Public Constructors

        public Inutilizacao(InutCTe inutCTe, Configuracao configuracao)
            : base(inutCTe, configuracao)
        {
        }

        public Inutilizacao()
        {
        }

        #endregion Public Constructors
    }
}
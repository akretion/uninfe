using System.Runtime.InteropServices;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    [ComVisible(true)]
    public class StatusServico: CTe.StatusServico
    {
        #region Public Constructors

        public StatusServico(ConsStatServCte consStatServ, Configuracao configuracao)
            : base(consStatServ, configuracao) { }

        public StatusServico()
        {
        }

        #endregion Public Constructors
    }
}
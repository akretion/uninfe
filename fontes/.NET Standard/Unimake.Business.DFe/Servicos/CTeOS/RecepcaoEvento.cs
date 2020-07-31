using System.Runtime.InteropServices;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    [ComVisible(true)]
    public class RecepcaoEvento: CTe.RecepcaoEvento
    {
        #region Public Constructors

        public RecepcaoEvento(EventoCTe envEvento, Configuracao configuracao)
            : base(envEvento, configuracao) { }

        public RecepcaoEvento() { }

        #endregion
    }
}
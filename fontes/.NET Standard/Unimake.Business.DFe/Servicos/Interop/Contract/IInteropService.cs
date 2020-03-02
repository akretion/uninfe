using System.Runtime.InteropServices;
using Unimake.Business.DFe.Xml;

namespace Unimake.Business.DFe.Servicos.Interop
{
    public interface IInteropService<TInteropType>
         where TInteropType : XMLBase
    {
        #region Public Methods

        [ComVisible(false)]
        void Executar();

        [ComVisible(true)]
        void Executar(TInteropType interopType, Configuracao configuracao);

        #endregion Public Methods
    }
}
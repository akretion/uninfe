using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.Memory
{
    public class Memory : MemoryBase
    {
        #region Construtures
        public Memory(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs, proxyuser, proxypass, proxyserver)
        { }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.Fiorilli
{
    public class Fiorilli : FiorilliBase
    {
        #region Construtures
        public Fiorilli(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs)
        { }
        #endregion
    }
}

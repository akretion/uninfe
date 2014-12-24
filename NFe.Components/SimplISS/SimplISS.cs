using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.SimplISS
{
    public class SimplISS : SimplISSBase
    {
        #region Construtures
        public SimplISS(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs)
        { }
        #endregion
    }
}

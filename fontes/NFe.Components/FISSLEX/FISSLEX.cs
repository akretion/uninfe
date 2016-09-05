using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.FISSLEX
{
    public class FISSLEX : FISSLEXBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public FISSLEX(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs, proxyuser, proxypass, proxyserver)
        { }
        #endregion
    }
}

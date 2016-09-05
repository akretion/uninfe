using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.Consist
{
    public class Consist : ConsistBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public Consist(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs, proxyuser, proxypass, proxyserver)
        { }
        #endregion
    }
}

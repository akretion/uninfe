using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.Betha.NewVersion
{
    public class Betha : BethaBase
    {
        #region Propriedades
        public override string NameSpaces
        {
            get
            {
                return "http://www.betha.com.br/e-nota-contribuinte-ws";
            }
        }
        #endregion

        #region Construtores
        public Betha(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs, proxyuser, proxypass, proxyserver)
        { }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.EGoverneISS
{
    public class EGoverneISS : EGoverneISSBase
    {
        #region Propriedades
        public override string NameSpaces
        {
            get
            {
                return "http://schemas.datacontract.org/2004/07/Eissnfe.Negocio.WebServices.Mensagem";
            }
        }
        #endregion

        #region Construtures
        public EGoverneISS(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs, proxyuser, proxypass, proxyserver)
        { }
        #endregion
    }
}

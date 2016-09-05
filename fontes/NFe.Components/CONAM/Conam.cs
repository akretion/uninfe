using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.Conam
{
    public class Conam : ConamBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public Conam(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs)
        { }
        #endregion
    }
}

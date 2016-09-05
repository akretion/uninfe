using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.MGM
{
    public class MGM : MGMBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public MGM(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senha)
            : base(tpAmb, pastaRetorno, codMun, usuario, senha)
        { }
        #endregion
    }
}

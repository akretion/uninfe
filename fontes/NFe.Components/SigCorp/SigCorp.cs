using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.SigCorp
{
    public class SigCorp : SigCorpBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public SigCorp(TipoAmbiente tpAmb, string pastaRetorno, int codMun)
            : base(tpAmb, pastaRetorno, codMun)
        { }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.RLZ_INFORMATICA
{
    public class Rlz_Informatica : Rlz_InformaticaBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public Rlz_Informatica(TipoAmbiente tpAmb, string pastaRetorno, int codMun)
            : base(tpAmb, pastaRetorno, codMun)
        { }
        #endregion
    }
}

using System;

namespace NFe.Components.BAURU_SP
{
    public class Bauru_SP : Bauru_SPBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public Bauru_SP(TipoAmbiente tpAmb, string pastaRetorno, int codMun)
            : base(tpAmb, pastaRetorno, codMun)
        { }
        #endregion
    }
}

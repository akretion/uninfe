using System;

namespace NFe.Components.Memory
{
    public class Memory : MemoryBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures

        public Memory(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs, proxyuser, proxypass, proxyserver)
        { }

        #endregion Construtures
    }
}
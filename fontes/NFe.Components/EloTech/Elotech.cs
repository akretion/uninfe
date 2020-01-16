using System;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Elotech
{
    public class Elotech : ElotechBase
    {
        public override string NameSpaces => throw new NotImplementedException();

        #region Construtures
        public Elotech(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno, codMun, usuarioProxy, senhaProxy, domainProxy, certificado)
        { }
        #endregion
    }
}

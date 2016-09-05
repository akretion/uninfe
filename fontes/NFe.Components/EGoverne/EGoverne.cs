using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NFe.Components.EGoverne
{
    public class EGoverne : EGoverneBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public EGoverne(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno, codMun, usuarioProxy, senhaProxy, domainProxy, certificado)
        { }
        #endregion
    }
}

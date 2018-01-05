using System;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Fiorilli
{
    public class Fiorilli : FiorilliBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures

        public Fiorilli(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno, codMun, usuario, senhaWs, proxyuser, proxypass, proxyserver, certificado)
        { }

        #endregion Construtures
    }
}
using System;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.GeisWeb
{
    public class GeisWeb: GeisWebBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures

        public GeisWeb(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno, codMun, proxyuser, proxypass, proxyserver, certificado)
        { }

        #endregion Construtures
    }
}
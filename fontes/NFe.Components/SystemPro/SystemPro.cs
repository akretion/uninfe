using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.SystemPro
{
    #region classe final
    public class SystemPro: SystemProBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtores
        public SystemPro(TipoAmbiente tpAmb, string pastaRetorno, X509Certificate certificate, int codMun)
            : base(tpAmb, pastaRetorno, certificate, codMun)
        { }
        #endregion
    }
    #endregion
}

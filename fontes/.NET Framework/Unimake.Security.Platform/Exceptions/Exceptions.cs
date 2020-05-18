using System;

namespace Unimake.Security.Platform.Exceptions
{
    public class CarregarCertificadoException: Exception
    {
        #region Public Constructors

        public CarregarCertificadoException(string message)
            : base(message)
        {
        }

        #endregion Public Constructors
    }

    public class CertificadoDigitalException: Exception
    {
        #region Public Constructors

        public CertificadoDigitalException()
            : base("Certificado digital não localizado ou o mesmo está com falha.")
        {
        }

        #endregion Public Constructors
    }
}
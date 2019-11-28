using System;

namespace Unimake.Security.Platform.Exceptions
{
    public class ExceptionCertificadoDigital : Exception
    {
        public ExceptionCertificadoDigital()
            : base("Certificado digital não localizado ou o mesmo está com falha.")
        {
        }
    }
}
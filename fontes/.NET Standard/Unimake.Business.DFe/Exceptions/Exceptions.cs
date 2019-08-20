using System;

namespace Unimake.Security.Exceptions
{
    public class ExceptionCertificadoDigital : Exception
    {
        public ExceptionCertificadoDigital()
            : base("Certificado digital não localizado ou o mesmo está com falha.")
        {
        }
    }
}
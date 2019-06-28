using System;

namespace Unimake.Security.Exceptions
{
    public class ExceptionCertificadoDigital : Exception
    {
        public ExceptionCertificadoDigital()
            : base("Certificado digital não localizado! Ou não foi informado nas configurações do UniNfe ou o mesmo está com falha.")
        {
        }
    }
}
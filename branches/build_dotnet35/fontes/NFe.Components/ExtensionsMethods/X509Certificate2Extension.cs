using System;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace NFe
{
    /// <summary>
    /// Classe para passar o Pin Code por baixo do código 
    /// (a caixa de dialógo pedindo o Pin Code não aparecerá para o usuário).
    /// <athor>Renan Borges</athor>
    /// <date>02/05/2014</date>
    /// </summary>
    public static class clsX509Certificate2Extension
    {
        /// <summary>
        /// Passa PIN Code (Senha/Password) para Certificados
        ///  eToken como o A3 do SERASA do Brasil
        /// </summary>
        /// <param name="_Certificado">O Certificado que está sendo usado 
        /// para a criptografia</param>
        /// <param name="_PinPassword">O Pin Code / Senha / Password</param>
        public static void SetPinPrivateKey(this X509Certificate2 _Certificado, string _PinPassword)
        {
            if(_Certificado == null) throw new ArgumentNullException("_Certificado == null!");
            var key = (RSACryptoServiceProvider)_Certificado.PrivateKey;

            IntPtr ProviderHandle = IntPtr.Zero;
            byte[] PinBuffer = Encoding.ASCII.GetBytes(_PinPassword);

            //Não é necessário descarregar o handle
            SafeNativeMethods.Execute(() => SafeNativeMethods.CryptAcquireContext(
                                                ref ProviderHandle,
                                                key.CspKeyContainerInfo.KeyContainerName,
                                                key.CspKeyContainerInfo.ProviderName,
                                                key.CspKeyContainerInfo.ProviderType,
                                                SafeNativeMethods.CryptContextFlags.Silent)
                                      );
            SafeNativeMethods.Execute(() => SafeNativeMethods.CryptSetProvParam(
                                                ProviderHandle,
                                                SafeNativeMethods.CryptParameter.KeyExchangePin,
                                                PinBuffer,
                                                0)
                                      );
            SafeNativeMethods.Execute(() => SafeNativeMethods.CertSetCertificateContextProperty(
                                           _Certificado.Handle,
                                           SafeNativeMethods.CertificateProperty.CryptoProviderHandle,
                                           0,
                                           ProviderHandle)
                                      );
        }

        /// <summary>
        /// Retorna true se o certificado for do tipo A3.
        /// </summary>
        /// <param name="x509cert">Certificado que deverá ser validado se é A3 ou não.</param>
        /// <returns></returns>
        public static bool IsA3(this X509Certificate2 x509cert)
        {
            if (x509cert == null) 
               return false;

            bool result = false;

            try
            {
                RSACryptoServiceProvider service = x509cert.PrivateKey as RSACryptoServiceProvider;

                if(service != null)
                {
                    if(service.CspKeyContainerInfo.Removable &&
                    service.CspKeyContainerInfo.HardwareDevice)
                        result = true;
                }
            }
            catch
            {
                //assume que é false
                result = false;
            }

            return result;
        }
    }
    /// <summary>
    /// Funções da API do Windows que realmente executam a passagem do PIN
    /// </summary>
    internal static class SafeNativeMethods
    {
        internal enum CryptContextFlags
        {
            None = 0,
            Silent = 0x40
        }

        internal enum CertificateProperty
        {
            None = 0,
            CryptoProviderHandle = 0x1
        }

        internal enum CryptParameter
        {
            None = 0,
            KeyExchangePin = 0x20
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptAcquireContext(
            ref IntPtr hProv,
            string containerName,
            string providerName,
            int providerType,
            CryptContextFlags flags
            );

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptSetProvParam(
            IntPtr hProv,
            CryptParameter dwParam,
            [In] byte[] pbData,
            uint dwFlags);

        [DllImport("CRYPT32.DLL", SetLastError = true)]
        internal static extern bool CertSetCertificateContextProperty(
            IntPtr pCertContext,
            CertificateProperty propertyId,
            uint dwFlags,
            IntPtr pvData
            );

        public static void Execute(Func<bool> action)
        {
            if(!action())
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
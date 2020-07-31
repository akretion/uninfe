using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Unimake.Business.DFe.Security
{
    #region clsX509Certificate2Extension  
    /// <summary>
    /// Classe para passar o Pin Code por baixo do código 
    /// (a caixa de dialógo pedindo o Pin Code não aparecerá para o usuário).
    /// </summary>
    public static class ClsX509Certificate2Extension
    {
        /// <summary>
        /// Passa PIN Code (Senha/Password) para Certificados
        ///  eToken como o A3 do SERASA do Brasil
        /// </summary>
        /// <param name="certificado">O Certificado que está sendo usado 
        /// para a criptografia</param>
        /// <param name="pinPassword">O Pin Code / Senha / Password</param>
        public static void SetPinPrivateKey(this X509Certificate2 certificado, string pinPassword)
        {
            if (certificado == null)
            {
                throw new ArgumentNullException("certificado == null!");
            }

            var key = (RSACryptoServiceProvider)certificado.PrivateKey;

            var ProviderHandle = IntPtr.Zero;
            var PinBuffer = Encoding.ASCII.GetBytes(pinPassword);

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
                                           certificado.Handle,
                                           SafeNativeMethods.CertificateProperty.CryptoProviderHandle,
                                           0,
                                           ProviderHandle)
                                      );
        }

        /// <summary>
        /// Retorna true se o certificado for do tipo A3.
        /// </summary>
        /// <param name="x509cert">Certificado que deverá ser validado se é A3 ou não.</param>
        /// <returns>true = É um certificado A3</returns>
        public static bool IsA3(this X509Certificate2 x509cert)
        {
            if (x509cert == null)
            {
                return false;
            }

            var result = false;

            try
            {
                if (x509cert.PrivateKey is RSACryptoServiceProvider service)
                {
                    if (service.CspKeyContainerInfo.Removable &&
                    service.CspKeyContainerInfo.HardwareDevice)
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
    #endregion

    #region SafeNativeMethods
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
            if (!action())
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
    #endregion
}

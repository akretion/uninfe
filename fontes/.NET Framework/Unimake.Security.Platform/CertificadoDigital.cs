using System;
using System.Security.Cryptography.X509Certificates;
using Unimake.Security.Platform.Exceptions;

namespace Unimake.Security.Platform
{
    public class CertificadoDigital
    {
        /// <summary>
        /// Executa tela com os certificados digitais instalados para seleção do usuário
        /// </summary>
        /// <returns>Retorna o certificado digital (null se nenhum certificado foi selecionado ou se o certificado selecionado está com alguma falha)</returns>
        public X509Certificate2 Selecionar()
        {
            X509Certificate2Collection scollection = AbrirTelaSelecao();

            if (scollection.Count > 0)
            {
                return scollection[0];
            }

            return null;
        }

        /// <summary>
        /// Abre a tela de dialogo do windows para seleção do certificado digital
        /// </summary>
        /// <returns>Retorna a coleção de certificados digitais</returns>
        public X509Certificate2Collection AbrirTelaSelecao()
        {
            X509Certificate2 x509Cert = new X509Certificate2();
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = store.Certificates;
            X509Certificate2Collection collection1 = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2Collection collection2 = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
            X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) digital(is) disponível(is)", "Selecione o certificado digital para uso no aplicativo", X509SelectionFlag.SingleSelection);

            return scollection;
        }

        /// <summary>
        /// Verifica se o certificado digital está vencido
        /// </summary>
        /// <param name="certificado">Certificado digital</param>
        /// <returns>true = Certificado vencido</returns>
        public bool Vencido(X509Certificate2 certificado)
        {
            bool retorna = false;

            if (certificado == null)
            {
                throw new ExceptionCertificadoDigital();
            }

            if (DateTime.Compare(DateTime.Now, certificado.NotAfter) > 0)
            {
                retorna = true;
            }

            return retorna;
        }
    }
}
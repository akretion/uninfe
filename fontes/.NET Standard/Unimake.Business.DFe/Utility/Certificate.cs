using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Unimake.Business.DFe.Security;

namespace Unimake.Business.DFe.Utility
{
    /// <summary>
    /// Utilitários para trabalhar com certificado digital
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// Forçar carregar o PIN do certificado A3 em casos de XML que não tem assinatura
        /// </summary>
        /// <param name="a3PIN">Código PIN do certificado A3</param>
        /// <param name="certificado">Objeto do certificado digital A3 que será utilizado</param>
        public void CarregarPINA3(X509Certificate2 certificado, string a3PIN)
        {
            var docTemp = new XmlDocument();
            docTemp.LoadXml("<pinA3><xServ Id=\"ID1\">PINA3</xServ></pinA3>");

            var tagAssinatura = "pinA3";
            var tagAtributoID = "xServ";

            AssinaturaDigital.Assinar(docTemp, tagAssinatura, tagAtributoID, certificado, AlgorithmType.Sha1, true, a3PIN, "Id");
        }
    }
}

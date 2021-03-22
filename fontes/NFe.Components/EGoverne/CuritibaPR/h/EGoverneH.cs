using NFe.Components.br.gov.pr.curitiba.pilotoisscuritiba.h;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace NFe.Components.EGoverne.CuritibaPR.h
{
    public class EGoverneH : EGoverneSerialization
    {
        private WSNFSeV1001 service = new WSNFSeV1001();

        #region construtores

        public EGoverneH(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno, usuarioProxy, senhaProxy, domainProxy)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
            service.Proxy = WebRequest.DefaultWebProxy;
            service.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            service.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            service.ClientCertificates.Add(certificado);
        }

        /// <summary>
        /// Indentificamos falha no certificado o do servidor, entao temos que ignorar os erros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool MyCertHandler(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("RecepcionarLoteRps", doc.InnerXml);
            GerarRetorno(file, result,
                Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML,
                Encoding.UTF8);
        }

        public override void CancelarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("CancelarNfse", doc.InnerXml);
            GerarRetorno(file, result,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML,
                Encoding.UTF8);
        }

        public override void ConsultarLoteRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarLoteRps", doc.InnerXml);
            GerarRetorno(file, result,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML,
                Encoding.UTF8);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarSituacaoLoteRps", doc.InnerXml);
            GerarRetorno(file, result,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML,
                Encoding.UTF8);
        }

        public override void ConsultarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarNfse", doc.InnerXml);
            GerarRetorno(file, result,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML,
                Encoding.UTF8);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarNfsePorRps", doc.InnerXml);
            GerarRetorno(file, result,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML,
                Encoding.UTF8);
        }

        #endregion Métodos
    }
}
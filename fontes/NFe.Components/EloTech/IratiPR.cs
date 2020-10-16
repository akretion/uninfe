using NFe.Components.Abstract;
using NFe.Components.EloTech.WS;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace NFe.Components.Elotech
{
    public class IratiPR: EmiteNFSeBase
    {
        #region Private Fields

        private readonly ElotechService Servico = new ElotechService("https://irati.iss.elotech.com.br/iss-ws/nfse203.wsdl");                                                                     

        #endregion Private Fields

        #region Private Methods

        private string EnviarLoteRps(string file)
        {
            var enviarLoteRpsResposta = Servico.EnviarLoteRps(file);
            var strResult = SerializarObjeto(enviarLoteRpsResposta);

            return strResult;
        }

        private string EnviarLoteRpsSincrono(string file)
        {
            var enviarLoteRpsSincronoResposta = Servico.EnviarLoteRpsSincrono(file);
            var strResult = SerializarObjeto(enviarLoteRpsSincronoResposta);

            return strResult;
        }

        /// <summary>
        /// Identificamos falha no certificado o do servidor, então temos que ignorar os erros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool MyCertHandler(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) => true;

        #endregion Private Methods

        #region Public Properties

        public override string NameSpaces { get; }

        #endregion Public Properties

        #region Public Constructors

        public IratiPR(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string proxyDomain, X509Certificate certificado)
                                    : base(tpAmb, pastaRetorno)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
            Servico.Proxy = WebRequest.DefaultWebProxy;
            Servico.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            Servico.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            Servico.Certificate = new X509Certificate2(certificado);
            Servico.ProxyDomain = proxyDomain;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void CancelarNfse(string file)
        {
            //var cancelarNfseResposta = Servico.CancelarNfse(file);
            //var strResult = SerializarObjeto(cancelarNfseResposta);

            //GerarRetorno(file, strResult,
            //    Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
            //    Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML,
            //    Encoding.UTF8);
        }

        public override void ConsultarLoteRps(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarLoteRps(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarNfse(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarNfsePorRps(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarSituacaoLoteRps(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
        }

        public override void EmiteNF(string file)
        {
            try
            {
                var strResult = string.Empty;

                var doc = new XmlDocument();
                doc.Load(file);

                switch(doc.DocumentElement.Name)
                {
                    case "EnviarLoteRpsEnvio":
                        strResult = EnviarLoteRps(file);
                        break;

                    case "EnviarLoteRpsSincronoEnvio":
                        strResult = EnviarLoteRpsSincrono(file);
                        break;
                }

                GerarRetorno(file, strResult,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML,
                    Encoding.UTF8);
            }
            catch(WebException ex)
            {
                var sr = new StreamReader(ex.Response.GetResponseStream());
                var message = sr.ReadToEnd();
                throw new System.Exception(message);
            }
        }

        #endregion Public Methods
    }
}
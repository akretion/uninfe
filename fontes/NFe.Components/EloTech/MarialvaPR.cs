using NFe.Components.Abstract;
using NFe.Components.EloTech.WS;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace NFe.Components.Elotech
{
    public class MarialvaPR: EmiteNFSeBase
    {
        #region Private Fields

        private readonly ElotechService Servico = new ElotechService("https://marialva.oxy.elotech.com.br/iss-ws/nfse203.wsdl");

        #endregion Private Fields

        #region Private Methods

        private string EnviarLoteRps(string file)
        {
            var enviarLoteRpsResposta = Servico.EnviarLoteRps(file);

            return enviarLoteRpsResposta.Xml;
        }

        private string EnviarLoteRpsSincrono(string file)
        {
            var enviarLoteRpsSincronoResposta = Servico.EnviarLoteRpsSincrono(file);

            return enviarLoteRpsSincronoResposta.Xml;
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

        public MarialvaPR(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string proxyDomain, X509Certificate certificado)
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
            try
            {
                var docRetorno = new XmlDocument();

                docRetorno.LoadXml(Servico.CancelarNfse(file).Xml);
                var strResult = docRetorno.GetElementsByTagName("ns2:CancelarNfseResposta")[0].OuterXml;

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

        public override void ConsultarLoteRps(string file)
        {
            try
            {
                var docRetorno = new XmlDocument();

                docRetorno.LoadXml(Servico.ConsultarLoteRps(file).Xml);
                var strResult = docRetorno.GetElementsByTagName("ns2:ConsultarLoteRpsResposta")[0].OuterXml;

                GerarRetorno(file, strResult,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML,
                    Encoding.UTF8);
            }
            catch(WebException ex)
            {
                var sr = new StreamReader(ex.Response.GetResponseStream());
                var message = sr.ReadToEnd();
                throw new System.Exception(message);
            }
        }

        public override void ConsultarNfse(string file)
        {
            try
            {
                var docRetorno = new XmlDocument();

                docRetorno.LoadXml(Servico.ConsultarNfse(file).Xml);
                var strResult = docRetorno.GetElementsByTagName("ns2:ConsultarNfseFaixaResposta")[0].OuterXml;

                GerarRetorno(file, strResult,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML,
                    Encoding.UTF8);
            }
            catch(WebException ex)
            {
                var sr = new StreamReader(ex.Response.GetResponseStream());
                var message = sr.ReadToEnd();
                throw new System.Exception(message);
            }
        }

        public override void ConsultarNfsePorRps(string file)
        {
            try
            {
                var docRetorno = new XmlDocument();

                docRetorno.LoadXml(Servico.ConsultarNfsePorRps(file).Xml);
                var strResult = docRetorno.GetElementsByTagName("ns2:ConsultarNfseRpsResposta")[0].OuterXml;

                GerarRetorno(file, strResult,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML,
                    Encoding.UTF8);
            }
            catch(WebException ex)
            {
                var sr = new StreamReader(ex.Response.GetResponseStream());
                var message = sr.ReadToEnd();
                throw new System.Exception(message);
            }
        }

        public override void ConsultarSituacaoLoteRps(string file) => throw new System.Exception("ConsultarSituacaoLoteRps não disponível no webservice das prefeituras padrão Elotech.");

        public override void EmiteNF(string file)
        {
            try
            {
                var strResult = string.Empty;
                var docRetorno = new XmlDocument();

                var doc = new XmlDocument();
                doc.Load(file);

                switch(doc.DocumentElement.Name)
                {
                    case "EnviarLoteRpsEnvio":
                        docRetorno.LoadXml(EnviarLoteRps(file));
                        strResult = docRetorno.GetElementsByTagName("ns2:EnviarLoteRpsResposta")[0].OuterXml;
                        break;

                    case "EnviarLoteRpsSincronoEnvio":
                        docRetorno.LoadXml(EnviarLoteRpsSincrono(file));
                        strResult = docRetorno.GetElementsByTagName("ns2:EnviarLoteRpsSincronoResposta")[0].OuterXml;
                        break;
                }

                GerarRetorno(file, strResult,
                    Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML,
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
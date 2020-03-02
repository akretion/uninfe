using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Unimake.Business.DFe
{
    public class ConsumirWS
    {
        #region Private Fields

        private readonly CookieContainer cookies = new CookieContainer();

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// Criar o envelope (SOAP) para envio ao webservice
        /// </summary>
        /// <param name="soap">Soap</param>
        /// <param name="xmlHeader">string do XML a ser enviado no cabeçalho do soap</param>
        /// <param name="xmlBody">string do XML a ser enviado no corpo do soap</param>
        /// <returns>string do envelope (soap)</returns>
        private static string EnveloparXML(WSSoap soap, string xmlBody)
        {
            var retorna = string.Empty;

            if(xmlBody.IndexOf("?>") >= 0)
            {
                xmlBody = xmlBody.Substring(xmlBody.IndexOf("?>") + 2);
            }

            retorna = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            retorna += soap.SoapString.Replace("{xmlBody}", xmlBody);

            return retorna;
        }

        #endregion Private Methods

        #region Public Properties

        /// <summary>
        /// Conteudo retornado pelo WebService consumido (formato string)
        /// </summary>
        public string RetornoServicoString { get; private set; }

        /// <summary>
        /// Conteudo retornado pelo WebService consumido (formato XmlDocument)
        /// </summary>
        public XmlDocument RetornoServicoXML { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Estabelece conexão com o Webservice e faz o envio do XML e recupera o retorno. Conteúdo retornado pelo webservice pode ser recuperado através das propriedades RetornoServicoXML ou RetornoServicoString.
        /// </summary>
        /// <param name="xml">XML a ser enviado para o webservice</param>
        /// <param name="servico">Parâmetros para execução do serviço (parâmetros do soap)</param>
        /// <param name="certificado">Certificado digital a ser utilizado na conexão com os serviços</param>
        public void ExecutarServico(XmlDocument xml, object servico, X509Certificate2 certificado)
        {
            var soap = (WSSoap)servico;

            try
            {
                var urlpost = new Uri(soap.EnderecoWeb);
                var soapXML = EnveloparXML(soap, xml.OuterXml);
                var buffer2 = Encoding.UTF8.GetBytes(soapXML);

                ServicePointManager.Expect100Continue = false;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RetornoValidacao);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(urlpost);
                httpWebRequest.Headers.Add("SOAPAction: " + soap.ActionWeb);
                httpWebRequest.CookieContainer = cookies;
                httpWebRequest.Timeout = 60000;
                httpWebRequest.ContentType = (string.IsNullOrEmpty(soap.ContentType) ? "application/soap+xml; charset=utf-8;" : soap.ContentType);
                httpWebRequest.Method = "POST";
                httpWebRequest.ClientCertificates.Add(certificado);
                httpWebRequest.ContentLength = buffer2.Length;

                var postData = httpWebRequest.GetRequestStream();
                postData.Write(buffer2, 0, buffer2.Length);
                postData.Close();

                var responsePost = (HttpWebResponse)httpWebRequest.GetResponse();
                var streamPost = responsePost.GetResponseStream();
                var streamReaderResponse = new StreamReader(streamPost, Encoding.UTF8);

                var retornoXml = new XmlDocument();
                retornoXml.LoadXml(streamReaderResponse.ReadToEnd());

                if(retornoXml.GetElementsByTagName(soap.TagRetorno)[0] == null)
                {
                    throw new Exception("Não foi possível localizar a tag <" + soap.TagRetorno + "> no XML retornado pelo webservice.");
                }

                RetornoServicoString = retornoXml.GetElementsByTagName(soap.TagRetorno)[0].ChildNodes[0].OuterXml;
                RetornoServicoXML = new XmlDocument
                {
                    PreserveWhitespace = false
                };
                RetornoServicoXML.LoadXml(RetornoServicoString);
            }
            catch
            {
                throw;
            }
        }

        public bool RetornoValidacao(object sender,
           X509Certificate certificate,
           X509Chain chain,
           SslPolicyErrors sslPolicyErros) => true;

        #endregion Public Methods
    }
}
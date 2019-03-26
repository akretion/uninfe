using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Unimake.DFe
{
    public class ConsumirWS
    {
        private readonly CookieContainer cookies = new CookieContainer();

        /// <summary>
        /// Conteudo retornado pelo WebService consumido (formato string)
        /// </summary>
        public string RetornoServicoString { get; private set; }

        /// <summary>
        /// Conteudo retornado pelo WebService consumido (formato XmlDocument)
        /// </summary>
        public XmlDocument RetornoServicoXML { get; private set; }

        public void ExecutarServico(XmlDocument xml, object servico, X509Certificate2 certificado)
        {
            WSSoap soap = (WSSoap)servico;

            try
            {
                Uri urlpost = new Uri(soap.EnderecoWeb);
                HttpWebRequest httpPostNFe = (HttpWebRequest)HttpWebRequest.Create(urlpost);

                string sNFeDados = EnveloparXML(xml.OuterXml, soap);

                string postConsultaComParametros = sNFeDados;

                byte[] buffer2 = Encoding.UTF8.GetBytes(postConsultaComParametros);

                httpPostNFe.CookieContainer = cookies;
                httpPostNFe.Timeout = 60000;
                httpPostNFe.ContentType = "application/soap+xml; charset=utf-8; action=" + soap.ActionWeb;
                httpPostNFe.Method = "POST";

                ServicePointManager.Expect100Continue = false;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RetornoValidacao);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

                //Certifica o objeto/servico -> adiciona o certificado
                httpPostNFe.ClientCertificates.Add(certificado);

                httpPostNFe.ContentLength = buffer2.Length;

                Stream PostData = httpPostNFe.GetRequestStream();
                PostData.Write(buffer2, 0, buffer2.Length);
                PostData.Close();

                HttpWebResponse responsePost = (HttpWebResponse)httpPostNFe.GetResponse();
                Stream istreamPost = responsePost.GetResponseStream();
                StreamReader strRespotaUrlConsultaNFe = new StreamReader(istreamPost, Encoding.UTF8);

                string x = strRespotaUrlConsultaNFe.ReadToEnd();

                XmlDocument retornoXml = new XmlDocument();
                retornoXml.LoadXml(x);

                RetornoServicoString = retornoXml.GetElementsByTagName(soap.TagRetorno)[0].ChildNodes[0].OuterXml;
                RetornoServicoXML = new XmlDocument();
                RetornoServicoXML.PreserveWhitespace = false;
                RetornoServicoXML.LoadXml(RetornoServicoString);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Envelopar XML
        /// </summary>
        /// <param name="xml">string do XML a ser envelopado</param>
        /// <param name="soap">Soap</param>
        /// <returns>string do XML já envelopado</returns>
        private static string EnveloparXML(string xml, WSSoap soap)
        {
            string retorna = string.Empty;
            if (xml.IndexOf("?>") >= 0)
            {
                xml = xml.Substring(xml.IndexOf("?>") + 2);
            }

            retorna = "<?xml version='1.0' encoding='UTF-8'?>";
            retorna += "<" + soap.VersaoSoap + ":Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:" + soap.VersaoSoap + "='http://www.w3.org/2003/05/soap-envelope'>";
            retorna += "<" + soap.VersaoSoap + ":Body>";
            retorna += "<nfeDadosMsg xmlns= '" + soap.ActionWeb + "'>" + xml + "</nfeDadosMsg>";
            retorna += "</" + soap.VersaoSoap + ":Body>";
            retorna += "</" + soap.VersaoSoap + ":Envelope>";

            return retorna;
        }

        public bool RetornoValidacao(object sender,
           X509Certificate certificate,
           X509Chain chain,
           SslPolicyErrors sslPolicyErros)
        {
            return true;
        }
    }
}

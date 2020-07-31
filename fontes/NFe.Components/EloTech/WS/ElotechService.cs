//Tem como base o código cedido por https://gist.github.com/luizvaz/43ccbd85b16b6802218b50b6d34c26de
//Obrigado Luiz Vaz :)

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace NFe.Components.EloTech.WS
{
    internal sealed class ElotechService
    {
        #region Private Fields

        private const string sSoapEnvelope =
              @"<?xml version=""1.0"" encoding=""utf-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <SOAP-ENV:Header>
                    <wsse:Security
                       xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""
                       xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""
                       SOAP-ENV:mustUnderstand=""1"">
                       <wsse:BinarySecurityToken EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary""
                                                 ValueType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3""
                                                 wsu:Id=""X509-EC95425802FB9F663F15021186132611"">
                       </wsse:BinarySecurityToken>
                    </wsse:Security>
                  </SOAP-ENV:Header>
                  <SOAP-ENV:Body xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" wsu:Id=""id-1"">
                  </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

        private const string STR_SOAPENV = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string STR_WSSE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string STR_WSU = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private string Url;

        #endregion Private Fields

        #region Private Methods

        private HttpWebRequest CreateWebRequest(string url, string action)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Proxy = WebRequest.DefaultWebProxy;

            if(Proxy != null)
            {
                webRequest.Proxy = Proxy;
            }

            if(Credentials != null)
            {
                webRequest.Proxy.Credentials = Credentials;
            }

            return webRequest;
        }

        private XmlDocument AssinaSOAP(string value)
        {
            var doc = new XmlDocument
            {
                PreserveWhitespace = true
            };
            doc.LoadXml(sSoapEnvelope);

            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("SOAP-ENV", STR_SOAPENV);
            ns.AddNamespace("wsse", STR_WSSE);
            ns.AddNamespace("wsu", STR_WSU);
            ns.AddNamespace("ds", STR_WSU);
            ns.AddNamespace("ec", STR_WSU);

            // *** Grab the body element - this is what we create the signature from
            var body = doc.DocumentElement.SelectSingleNode(@"//SOAP-ENV:Body", ns) as XmlElement;
            if(body == null)
            {
                throw new ApplicationException("No body tag found");
            }

            // *** Fill the body
            body.InnerXml = value;

            // *** Signed XML will create Xml Signature - Xml fragment
            var signedXml = new SignedXmlWithId(doc);

            // *** Create a KeyInfo structure
            var keyInfo = new KeyInfo
            {
                Id = "KI-EC95425802FB9F663F15021186132692"
            };

            var keyInfoNode = new KeyInfoNode();
            var wsseSec = doc.CreateElement("wsse", "SecurityTokenReference", STR_WSSE);
            wsseSec.SetAttribute("xmlns:wsu", STR_WSU);
            wsseSec.SetAttribute("Id", STR_WSU, "STR-EC95425802FB9F663F15021186132713");
            var wsseRef = doc.CreateElement("wsse", "Reference", STR_WSSE);
            wsseRef.SetAttribute("URI", "#X509-EC95425802FB9F663F15021186132611");
            wsseRef.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            wsseSec.AppendChild(wsseRef);
            keyInfoNode.Value = wsseSec;
            keyInfo.AddClause(keyInfoNode);

            // *** The actual key for signing - MAKE SURE THIS ISN'T NULL!
            signedXml.SigningKey = Certificate.PrivateKey;

            // *** provide the certficate info that gets embedded - note this is only
            // *** for specific formatting of the message to provide the cert info
            signedXml.KeyInfo = keyInfo;

            // *** Again unusual - meant to make the document match template
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            // Set the InclusiveNamespacesPrefixList property.
            var canMethod = (XmlDsigExcC14NTransform)signedXml.SignedInfo.CanonicalizationMethodObject;
            canMethod.InclusiveNamespacesPrefixList = "SOAP-ENV";

            // *** Now create reference to sign: Point at the Body element
            var reference = new Reference
            {
                Uri = "#id-1"  // reference wsu:Id=id-6 section in same doc
            };

            // Add an enveloped transformation to the reference.
            var env = new XmlDsigExcC14NTransform
            {
                InclusiveNamespacesPrefixList = ""
            };
            reference.AddTransform(env); // required to match doc

            signedXml.AddReference(reference);

            // *** Finally create the signature
            signedXml.ComputeSignature();

            // *** wsse:Security element
            var soapSignature = doc.DocumentElement.SelectSingleNode(@"//wsse:Security", ns) as XmlElement;
            if(soapSignature == null)
            {
                throw new ApplicationException("No wsse:Security tag found");
            }

            // *** Create wsse:BinarySecurityToken element
            var soapToken = doc.DocumentElement.SelectSingleNode(@"//wsse:BinarySecurityToken", ns) as XmlElement;
            if(soapToken == null)
            {
                throw new ApplicationException("No wsse:BinarySecurityToken tag found");
            }

            var export = Certificate.Export(X509ContentType.Cert);
            var base64 = Convert.ToBase64String(export);
            soapToken.InnerText = base64;

            // *** Result is an XML node with the signature detail below it
            // *** Now let's add the sucker into the SOAP-HEADER
            var signedElement = signedXml.GetXml();
            var sId = doc.CreateAttribute("Id");
            sId.Value = "SIG-2";
            signedElement.Attributes.Append(sId);

            // *** And add our signature as content
            soapSignature.AppendChild(signedElement);

            // *** Now add the signature header into the master header
            var soapHeader = doc.DocumentElement.SelectSingleNode("//SOAP-ENV:Header", ns) as XmlElement;
            if(soapHeader == null)
            {
                throw new ApplicationException("No SOAP-ENV:Header tag found");
            }

            return doc;
        }

        #endregion Private Methods

        #region Internal Methods

        internal ElotechServiceResult EnviarLoteRps(string file)
        {
            var request = CreateWebRequest(Url, "");
            var doc = new XmlDocument();
            doc.Load(file);
            var value = doc.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            var soapEnvelopeXml = AssinaSOAP(value);

            using(var stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using(var response = request.GetResponse())
            {
                using(var rd = new StreamReader(response.GetResponseStream()))
                {
                    var soapResult = (ElotechServiceResult)rd.ReadToEnd();

                    //Eu achei esta lista de código de erros da Elotech
                    //http://www.pontagrossa.pr.gov.br/files/elotech_manual_lista_de_erros.pdf
                    //Mas não achei o retorno que diz sucesso, logo assumo o campo "Correcao".

                    if(!string.IsNullOrEmpty(soapResult.Correcao))
                    {
                        throw (Exception)soapResult;
                    }

                    //se chegou aqui, não tem erros e o campo Xml do result terá o conteúdo do XML retornado
                    return soapResult;
                }
            }
        }

        internal ElotechServiceResult EnviarLoteRpsSincrono(string file)
        {
            return EnviarLoteRps(file);
        }


        #endregion Internal Methods

        #region Public Properties

        public X509Certificate2 Certificate { get; set; }
        public NetworkCredential Credentials { get; set; }
        public IWebProxy Proxy { get; set; }
        public string ProxyDomain { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public ElotechService(string url) => Url = url;

        #endregion Public Constructors
    }
}
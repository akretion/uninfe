using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.Components.Tinus.IpojucaPE
{
    [System.Web.Services.WebServiceBinding]
    public abstract class SoapHttpClientProtocol<TSOAPService> : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        #region Private Properties

        private string SOAPAction => GetType().Name;

        #endregion Private Properties

        #region Protected Methods

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var request = base.GetWebRequest(uri);

            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add($@"SOAPAction:http://www.abrasf.org.br/nfse.xsd/WSNFSE.{SOAPAction}.{SOAPAction}");
            request.Method = "POST";

            return request;
        }

        protected new object[] Invoke(string methodName, object[] parameters)
        {
            /*

                ¯\_(ツ)_/¯

                 There is always a solution

                         ,;~;,
                            /\_
                           (  /
                           ((),     ;,;
                           |  \\  ,;;'(
                       __ _(  )'~;;'   \
                     /'  '\'()/~' \ /'\.)
                  ,;(      )||     |
                 ,;' \    /-(.;,   )
                      ) /       ) /
                     //         ||
                    (_\         (_\

                 go horse <3

            */

            var typeName = typeof(TSOAPService).Name;
            var resposta = default(TSOAPService);
            var xmlObject = parameters[0];
            var request = (HttpWebRequest)GetWebRequest(new Uri(Url));
            var xmlDoc = new XmlDocument();
            var serializer = new XmlSerializer(xmlObject.GetType());
            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, xmlObject);
            var body = textWriter.ToString()
                                 .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "")
                                 .Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "")
                                 .Trim();
            var soapEnvXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                               <soap:Envelope
                                    xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
                                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
	                                <soap:Body>
                                    <{SOAPAction} xmlns=""http://www.abrasf.org.br/nfse.xsd"">
                                        $body
                                    </{SOAPAction}>
                                    </soap:Body>
                               </soap:Envelope>".Replace("$body", body);

            xmlDoc.LoadXml(soapEnvXml);

            using (var stream = request.GetRequestStream())
            {
                xmlDoc.Save(stream);
            }

            using (var response = request.GetResponse())
            {
                using (var rd = new StreamReader(response.GetResponseStream()))
                {
                    var xml = rd.ReadToEnd();
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    var node = xmlDoc.GetElementsByTagName(typeName)[0];

                    xml = $@"<?xml version=""1.0"" encoding=""UTF-8"" ?>
                             <{typeName}>
                                {node?.InnerXml}
                             </{typeName}>";

                    serializer = new XmlSerializer(typeof(TSOAPService));

                    using (var reader = new StringReader(xml))
                    {
                        resposta = (TSOAPService)serializer.Deserialize(reader);
                    }
                }
            }

            return new object[] { resposta };
        }

        #endregion Protected Methods
    }
}
using NFe.Components.Abstract;
using NFe.Components.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace NFSe.Components
{
    /// <summary>
    /// Esta classe utiliza métodos POST para fazer requisições
    /// </summary>
    public class POSTRequest : RequestBase, IPostRequest
    {
        public string Post(string usuario, string senha, string URLAPIBase, string file)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient cliente = new HttpClient();

            var body = new
            {
                usuario,
                senha,
                xml = File.ReadAllText(file)
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            result = cliente.PostAsync(URLAPIBase, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Faz o post e retorna uma string  com o resultado
        /// </summary>
        /// <param name="url">url base para utilizar dentro do post</param>
        /// <param name="postData">dados a serem enviados junto com o post</param>
        /// <returns></returns>
        public string PostForm(string url, IDictionary<string, string> postData)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            string file = postData["f1"];

            #region Preparar a requisição

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            //ajustar para permitir o cabeçalho HTTP/1.0
            SetAllowUnsafeHeaderParsing20();

            //evitar o erro "The remote server returned an error: (417) Expectation Failed."
            //para caeçalhos HTTP/1.0
            System.Net.ServicePointManager.Expect100Continue = false;

            if (Proxy != null)
            {
                request.UseDefaultCredentials = false;
                request.Proxy = Proxy;
                request.Proxy.Credentials = Proxy.Credentials;
                request.Credentials = Proxy.Credentials;
            }

            #endregion Preparar a requisição

            #region Crar o stream da solicitação

            Stream memStream = new System.IO.MemoryStream();

            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            foreach (KeyValuePair<string, string> keyValue in postData)
            {
                string formitem = string.Format(formdataTemplate, keyValue.Key, keyValue.Value);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            memStream.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

            string header = string.Format(headerTemplate, "f1", file);

            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

            memStream.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open,
            FileAccess.Read);
            byte[] buffer = new byte[1024];

            int bytesRead = 0;

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            memStream.Write(boundarybytes, 0, boundarybytes.Length);
            fileStream.Close();

            request.ContentLength = memStream.Length;

            #endregion Crar o stream da solicitação

            #region Escrever na requisição

            Stream requestStream = request.GetRequestStream();

            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            #endregion Escrever na requisição

            #region Resposta do servidor

            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();
            StreamReader reader = response.ContentType.IndexOf("charset=") == -1 ?
                new StreamReader(stream, Encoding.UTF8) :
                new StreamReader(stream, Encoding.GetEncoding(response.ContentType.Substring(response.ContentType.IndexOf("charset=") + 8)));

            string result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
            return result;

            #endregion Resposta do servidor
        }

        /// <summary>
        /// Faz o post e retorna uma string  com o resultado
        /// </summary>
        /// <param name="url">url base para utilizar dentro do post</param>
        /// <param name="postData">dados a serem enviados junto com o post</param>
        /// <returns></returns>
        public string PostForm(string url, IDictionary<string, string> postData = null, IList<string> headers = null)
        {
            string result = string.Empty;
            string postParameter = string.Empty;
            string xmlFile = "";

            foreach (KeyValuePair<string, string> keyValue in postData.Where(w => w.Key != "f1"))
                postParameter += $"&{keyValue.Key}={keyValue.Value}";

            if (postParameter.Length > 1)
            {
                postParameter = postParameter?.Substring(1);
                url += $"?{postParameter}";
            }
            string accept = null;
            string contentType = accept;

            if (postData.Keys.Contains("f1"))
            {
                xmlFile = postData["f1"];
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFile);
                xmlFile = doc.InnerXml;
                contentType = "application/xml";
                accept = "application/xml";
            }

            byte[] encode = Encoding.UTF8.GetBytes(xmlFile);
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = encode.Length;
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Accept = accept;

            foreach (string header in headers)
                request.Headers.Add(header);

            if (Proxy != null)
            {
                request.UseDefaultCredentials = false;
                request.Proxy = Proxy;
                request.Proxy.Credentials = Proxy.Credentials;
                request.Credentials = Proxy.Credentials;
            }

            //ajustar para permitir o cabeçalho HTTP/1.0
            SetAllowUnsafeHeaderParsing20();

            //evitar o erro "The remote server returned an error: (417) Expectation Failed."
            //para cabeçalhos HTTP/1.0
            ServicePointManager.Expect100Continue = false;

            if (encode.Length > 0)
            {
                var stream = request.GetRequestStream();
                stream.Write(encode, 0, encode.Length);
                stream.Close();
            }

            WebResponse response = default(WebResponse);
            bool success = true;

            try
            {
                response = request?.GetResponse();
            }
            catch (WebException webEx)
            {
                response = webEx.Response;
                success = false;
            }

            var streamDados = response.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            result = reader.ReadToEnd();
            streamDados.Close();
            response.Close();
            response.Dispose();

            if (!success &&
                result.StartsWith("\n"))
                result = result.Substring(1);

            return result;
        }
    }
}
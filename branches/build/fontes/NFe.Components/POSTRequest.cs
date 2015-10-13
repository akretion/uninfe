using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Reflection;

namespace NFSe.Components
{
    /// <summary>
    /// Esta classe utiliza métodos POST para fazer requisições
    /// </summary>
    public class POSTRequest: IDisposable
    {
        /// <summary>
        /// Proxy para ser utilizado na requisição, pode ser nulo
        /// </summary>
        public IWebProxy Proxy { get; set; }

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

            if(Proxy != null)
            {
                request.UseDefaultCredentials = false;
                request.Proxy = Proxy;
                request.Proxy.Credentials = Proxy.Credentials;
                request.Credentials = Proxy.Credentials;
            }
            #endregion

            #region Crar o stream da solicitação
            Stream memStream = new System.IO.MemoryStream();

            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            foreach(KeyValuePair<string, string> keyValue in postData)
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

            while((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            memStream.Write(boundarybytes, 0, boundarybytes.Length);
            fileStream.Close();

            request.ContentLength = memStream.Length;
            #endregion

            #region Escrever na requisição
            Stream requestStream = request.GetRequestStream();

            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            #endregion

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
            #endregion
        }

        public void Dispose()
        {
            Proxy = null;
        }

        /// <summary>
        /// Evita o erro de servidor cometeu uma violação de protocolo. 
        /// </summary>
        /// <seealso cref="https://msdn.microsoft.com/pt-br/library/system.net.configuration.httpwebrequestelement.useunsafeheaderparsing%28v=vs.110%29.aspx"/>
        void SetAllowUnsafeHeaderParsing20()
        {
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if(aNetAssembly != null)
            {
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if(aSettingsType != null)
                {
                    object anInstance = aSettingsType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if(anInstance != null)
                    {
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if(aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                        }
                    }
                }
            }
        }
    }
}

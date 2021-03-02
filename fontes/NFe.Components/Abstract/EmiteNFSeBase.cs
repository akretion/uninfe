using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.Components.Abstract
{
    public abstract class EmiteNFSeBase: IEmiteNFSe
    {
        public string ProxyUser { get; set; }
        public string ProxyPass { get; set; }
        public string ProxyServer { get; set; }
        public TipoAmbiente tpAmb { get; set; }
        public string PastaRetorno { get; set; }
        public abstract string NameSpaces { get; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Cidade { get; set; }
        public IWebProxy Proxy { get; set; }

        public EmiteNFSeBase(TipoAmbiente tpAmb, string pastaRetorno)
        {
            this.tpAmb = tpAmb;
            this.PastaRetorno = pastaRetorno;
        }

        /// <summary>
        /// Deserializar o objeto para string
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="file">Caminho do arquivo</param>
        /// <returns></returns>
        public T DeserializarObjeto<T>(string file)
            where T : new()
        {
            T envio = new T();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = envio.GetType().Name;
            xRoot.Namespace = NameSpaces;

            XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);
            StreamReader reader = new StreamReader(file);

            try
            {
                envio = (T)serializer.Deserialize(reader);
            }
            catch
            {
                throw;
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }

            return envio;
        }

        /// <summary>
        /// Serializar o objeto para XML
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que será serializado</typeparam>
        /// <param name="retorno">Objeto de retorno que será convertivo</param>
        /// <returns></returns>
        public string SerializarObjeto<T>(T retorno)
            where T : new()
        {
            XmlSerializer serializerResposta = new XmlSerializer(typeof(T));
            StringWriter textWriter = new StringWriter();
            serializerResposta.Serialize(textWriter, retorno);

            return textWriter.ToString();
        }

        public virtual void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            GerarRetorno(file, result, extEnvio, extRetorno, Encoding.Default);
        }

        public virtual void GerarRetorno(string file, string result, string extEnvio, string extRetorno, Encoding encoding)
        {
            string nomearq = Path.Combine(PastaRetorno, Functions.ExtrairNomeArq(file, extEnvio) + extRetorno);

            File.WriteAllText(nomearq, result, encoding);
        }

        public string CreateXML(Object objetoRetorno)
        {
            Object tcErros = null;

            return CreateXML(objetoRetorno, tcErros);
        }

        public string CreateXML(Object objetoRetorno, Object tcErros)
        {
            XmlDocument xmlDoc = new XmlDocument();

            if(objetoRetorno != null && (objetoRetorno.GetType().Name.ToLower() != "string" || objetoRetorno.ToString() != ""))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(objetoRetorno.GetType());
                using(MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, objetoRetorno);
                    xmlStream.Position = 0;
                    xmlDoc.Load(xmlStream);
                }
            }

            if(tcErros != null)
            {
                XmlDocument xmlDoc2 = new XmlDocument();
                XmlSerializer xmlSerializer2 = new XmlSerializer(tcErros.GetType());
                using(MemoryStream xmlStream2 = new MemoryStream())
                {
                    xmlSerializer2.Serialize(xmlStream2, tcErros);
                    xmlStream2.Position = 0;
                    xmlDoc2.Load(xmlStream2);
                }

                if(objetoRetorno != null && (objetoRetorno.GetType().Name.ToLower() != "string" || objetoRetorno.ToString() != ""))
                {
                    XmlNode importedDocument = xmlDoc.ImportNode(xmlDoc2.DocumentElement, true);
                    xmlDoc.DocumentElement.AppendChild(importedDocument);
                }
                else
                {
                    xmlDoc = xmlDoc2;
                }
            }

            return xmlDoc.InnerXml;
        }

        public virtual string EmiteNF(string file, bool cancelamento = false)
        {
            return "";
        }

        public abstract void EmiteNF(string file);

        public abstract void CancelarNfse(string file);

        public abstract void ConsultarLoteRps(string file);

        public abstract void ConsultarSituacaoLoteRps(string file);

        public abstract void ConsultarNfse(string file);

        public abstract void ConsultarNfsePorRps(string file);

        public virtual void SubstituirNfse(string file) { }

        public virtual void ConsultarNfseServicoTomado(string file)
        {
        }

        public virtual void ConsultarXml(string file) { }

        public object WSGeracao
        {
            get;
            protected set;
        }

        public object WSConsultas
        {
            get;
            protected set;
        }

        public void DefinirProxy<T>(SoapHttpClientProtocol request)
        {
            if(!string.IsNullOrEmpty(ProxyUser))
            {
                NetworkCredential credentials = new NetworkCredential(ProxyUser, ProxyPass, ProxyServer);
                WebRequest.DefaultWebProxy.Credentials = credentials;

                request.Proxy = WebRequest.DefaultWebProxy;
                request.Proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
                request.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
            }
        }
    }
}
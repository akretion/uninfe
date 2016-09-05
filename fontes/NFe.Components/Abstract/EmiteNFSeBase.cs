using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.Components.Abstract
{
    public abstract class EmiteNFSeBase : IEmiteNFSe
    {
        public TipoAmbiente tpAmb { get; set; }
        public string PastaRetorno { get; set; }
        public abstract string NameSpaces { get; }

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
            envio = (T)serializer.Deserialize(reader);
            reader.Close();

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

        /// WANDREY:
        ///  nao precisa passar a extensao para excluir, porque podemos varrer a lista de extensoes e exclui-la
        ///  
        public void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            string nomearq = Path.Combine(PastaRetorno, Functions.ExtrairNomeArq(file, extEnvio) + extRetorno);

            StreamWriter write = new StreamWriter(nomearq);
            write.Write(result);
            write.Flush();
            write.Close();
            write.Dispose();
        }

        public string CreateXML(Object objetoRetorno)
        {
            Object tcErros = null;

            return CreateXML(objetoRetorno, tcErros);
        }

        public string CreateXML(Object objetoRetorno, Object tcErros)
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (objetoRetorno != null && (objetoRetorno.GetType().Name.ToLower() != "string" || objetoRetorno.ToString() != ""))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(objetoRetorno.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, objetoRetorno);
                    xmlStream.Position = 0;
                    xmlDoc.Load(xmlStream);
                }
            }

            if (tcErros != null)
            {
                XmlDocument xmlDoc2 = new XmlDocument();
                XmlSerializer xmlSerializer2 = new XmlSerializer(tcErros.GetType());
                using (MemoryStream xmlStream2 = new MemoryStream())
                {
                    xmlSerializer2.Serialize(xmlStream2, tcErros);
                    xmlStream2.Position = 0;
                    xmlDoc2.Load(xmlStream2);
                }

                if (objetoRetorno != null && (objetoRetorno.GetType().Name.ToLower() != "string" || objetoRetorno.ToString() != ""))
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

        public abstract void EmiteNF(string file);

        public abstract void CancelarNfse(string file);

        public abstract void ConsultarLoteRps(string file);

        public abstract void ConsultarSituacaoLoteRps(string file);

        public abstract void ConsultarNfse(string file);

        public abstract void ConsultarNfsePorRps(string file);

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
    }
}

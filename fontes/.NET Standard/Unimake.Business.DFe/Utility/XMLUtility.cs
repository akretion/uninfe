using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Utility
{
    public static class XMLUtility
    {
        /// <summary>
        /// Deserializar XML (Converte o XML para um objeto)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="doc">Conteúdo do XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        public static T Deserializar<T>(XmlDocument doc)
            where T : new()
        {
            T objeto = new T();

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            Stream stream = StringXmlToStream(doc.OuterXml);
            StreamReader reader = new StreamReader(stream);
            objeto = (T)serializer.Deserialize(reader);
            reader.Close();

            return objeto;
        }

        /// <summary>
        /// Serializar o objeto (Converte o objeto para XML)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="objeto">Objeto a ser serializado</param>
        /// <param name="nameSpaces">Namespaces a serem adicionados no XML</param>
        /// <returns>XML</returns>
        public static XmlDocument Serializar<T>(T objeto, List<TNameSpace> nameSpaces = null)
            where T : new()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            if (nameSpaces != null)
            {
                for (int i = 0; i < nameSpaces.Count; i++)
                {
                    ns.Add(nameSpaces[i].Prefix, nameSpaces[i].NS);
                }
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlDocument doc = new XmlDocument();
            using (StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objeto, ns);
                doc.LoadXml(textWriter.ToString());
            }

            return doc;
        }

        /// <summary>
        /// Converte uma string com estrutura de XML para Stream
        /// </summary>
        /// <returns>Conteúdo do XML em Stream</returns>
        /// <param name="conteudoXML">Conteúdo do XML a ser convertido</param>
        public static MemoryStream StringXmlToStream(string conteudoXML)
        {
            byte[] byteArray = new byte[conteudoXML.Length];
            ASCIIEncoding encoding = new ASCIIEncoding();
            byteArray = encoding.GetBytes(conteudoXML);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        public class TNameSpace
        {
            public string Prefix { get; set; }
            public string NS { get; set; }
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}

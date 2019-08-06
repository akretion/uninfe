using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Utility
{
    public static class XMLUtility
    {
        #region Public Classes

        public class TNameSpace
        {
            #region Public Properties

            public string NS { get; set; }
            public string Prefix { get; set; }

            #endregion Public Properties
        }

        public class Utf8StringWriter: StringWriter
        {
            #region Public Properties

            public override Encoding Encoding => Encoding.UTF8;

            #endregion Public Properties
        }

        #endregion Public Classes

        #region Public Methods

        /// <summary>
        /// Gerar o dígito da chave da NFe, CTe, MDFe ou NFCe
        /// </summary>
        /// <param name="chave">Chave (sem o dígito) para efetuar o cálculo do dígito verificador</param>
        /// <returns>Dígito verificador</returns>
        public static int CalcularDVChave(string chave)
        {
            int i, j, Digito;
            const string PESO = "4329876543298765432987654329876543298765432";

            chave = chave.Replace("NFe", "");

            if(chave.Length != 43)
            {
                throw new Exception(string.Format("Erro na composição da chave [{0}] para obter o dígito verificador.", chave) + Environment.NewLine);
            }
            else
            {
                j = 0;
                Digito = -1;
                try
                {
                    for(i = 0; i < 43; ++i)
                    {
                        j += Convert.ToInt32(chave.Substring(i, 1)) * Convert.ToInt32(PESO.Substring(i, 1));
                    }

                    Digito = 11 - (j % 11);
                    if((j % 11) < 2)
                    {
                        Digito = 0;
                    }
                }
                catch
                {
                    Digito = -1;
                }

                if(Digito == -1)
                {
                    throw new Exception(string.Format("Erro no cálculo do dígito verificador da chave [{0}].", chave) + Environment.NewLine);
                }

                return Digito;
            }
        }

        /// <summary>
        /// Deserializar XML (Converte o XML para um objeto)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="doc">Conteúdo do XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        public static T Deserializar<T>(XmlDocument doc)
            where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));
            Stream stream = StringXmlToStream(doc.OuterXml);
            var reader = new StreamReader(stream);
            var objeto = (T)serializer.Deserialize(reader);
            reader.Close();

            return objeto;
        }

        /// <summary>
        /// Gera um número randômico para ser utilizado no Codigo Numérico da NFe, NFCe, CTe, MDFe, etc...
        /// </summary>
        /// <param name="numeroNF">Número da NF, CT ou MDF</param>
        /// <returns>Código numérico</returns>
        public static int GerarCodigoNumerico(int numeroNF)
        {
            var retorno = 0;

            while(retorno == 0)
            {
                var rnd = new Random(numeroNF);

                retorno = Convert.ToInt32(rnd.Next(1, 99999999).ToString("00000000"));
            }

            return retorno;
        }

        /// <summary>
        /// Serializar o objeto (Converte o objeto para XML)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="objeto">Objeto a ser serializado</param>
        /// <param name="nameSpaces">Namespaces a serem adicionados no XML</param>
        /// <returns>XML</returns>
        public static XmlDocument Serializar<T>(T objeto, List<TNameSpace> nameSpaces = null)
            where T : new() => Serializar((object)objeto, nameSpaces);

        /// <summary>
        /// Serializar o objeto (Converte o objeto para XML)
        /// </summary>
        /// <param name="objeto">Objeto a ser serializado</param>
        /// <param name="nameSpaces">Namespaces a serem adicionados no XML</param>
        /// <returns>XML</returns>
        public static XmlDocument Serializar(object obj, List<TNameSpace> nameSpaces = null)
        {
            var ns = new XmlSerializerNamespaces();
            if(nameSpaces != null)
            {
                for(var i = 0; i < nameSpaces.Count; i++)
                {
                    ns.Add(nameSpaces[i].Prefix, nameSpaces[i].NS);
                }
            }

            var xmlSerializer = new XmlSerializer(obj.GetType());
            var doc = new XmlDocument();
            using(StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj, ns);
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
            var byteArray = new byte[conteudoXML.Length];
            var encoding = new ASCIIEncoding();
            byteArray = encoding.GetBytes(conteudoXML);
            var memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        #endregion Public Methods
    }
}
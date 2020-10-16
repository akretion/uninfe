using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Utility
{
    /// <summary>
    /// Utilitários diversos para trabalhar com XML
    /// </summary>
    public static class XMLUtility
    {
        #region Public Classes

        /// <summary>
        /// Tipo Namespace
        /// </summary>
        public class TNameSpace
        {
            #region Public Properties

            /// <summary>
            /// Conteúdo do Namespace
            /// </summary>
            public string NS { get; set; }
            /// <summary>
            /// Prefixo do Namespace
            /// </summary>
            public string Prefix { get; set; }

            #endregion Public Properties
        }

        /// <summary>
        /// Implementa um StringWriter para gravar informações em uma cadeia de caracteres. As informações são armazenadas em um StringBuilder subjacente.
        /// </summary>
        public class Utf8StringWriter: StringWriter
        {
            #region Public Properties

            /// <summary>
            /// Sobrecrever o Encoding para deixar como padrão o UTF8
            /// </summary>
            public override Encoding Encoding => Encoding.UTF8;

            #endregion Public Properties
        }

        #endregion Public Classes

        #region Public Methods

        /// <summary>
        /// Gerar o dígito da chave da NFe, CTe, MDFe ou NFCe
        /// </summary>
        /// <param name="chave">Chave do DFe (sem o dígito) que deve ser calculado o dígito verificador.</param>
        /// <returns>Dígito verificador</returns>
        public static int CalcularDVChave(string chave)
        {
            if(chave is null)
            {
                throw new ArgumentNullException(nameof(chave));
            }

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
        /// <param name="xml">XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        public static T Deserializar<T>(string xml)
            where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream = new StringReader(xml);
            return (T)serializer.Deserialize(stream);
        }

        /// <summary>
        /// Deserializar XML (Converte o XML para um objeto)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="doc">Conteúdo do XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        public static T Deserializar<T>(XmlDocument doc)
            where T : new() => Deserializar<T>(doc.OuterXml);

        /// <summary>
        /// Detectar qual o tipo de documento fiscal eletrônico do XML
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do documento eletrônico</returns>
        public static TipoDFe DetectDFeType(XmlDocument xml) => DetectDFeType(xml.OuterXml);

        /// <summary>
        /// Detectar qual o tipo de documento fiscal eletrônico do XML
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do documento eletrônico</returns>
        public static TipoDFe DetectDFeType(string xml)
        {
            var tipoDFe = TipoDFe.NFe;

            if(xml.Contains("<mod>55</mod>"))
            {
                tipoDFe = TipoDFe.NFe;
            }
            else if(xml.Contains("<mod>65</mod>"))
            {
                tipoDFe = TipoDFe.NFCe;
            }
            else if(xml.Contains("<mod>57</mod>"))
            {
                tipoDFe = TipoDFe.CTe;
            }
            else if(xml.Contains("<mod>67</mod>"))
            {
                tipoDFe = TipoDFe.CTe;
            }
            else if(xml.Contains("infMDFe"))
            {
                tipoDFe = TipoDFe.MDFe;
            }
            else if(xml.Contains("infCFe"))
            {
                tipoDFe = TipoDFe.CFe;
            }

            return tipoDFe;
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
        /// Busca o número da chave do Documento Fiscal Eletrônico no XML do Documento Fiscal Eletrônico
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <returns>Chave do DFe (Documento Fiscal Eletrônico = NFe, NFCe, CTe, etc...)</returns>
        public static string GetChaveDFe(string xml) => GetChaveDFe(xml, DetectDFeType(xml));

        /// <summary>
        /// Busca o número da chave do Documento Fiscal Eletrônico no XML do Documento Fiscal Eletrônico
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <param name="typeDFe">Tipo do DFe</param>
        /// <returns>Chave do DFe (Documento Fiscal Eletrônico = NFe, NFCe, CTe, etc...)</returns>
        public static string GetChaveDFe(string xml, TipoDFe typeDFe)
        {
            var typeString = "";

            switch(typeDFe)
            {
                case TipoDFe.NFe:
                case TipoDFe.NFCe:
                    typeString = "NFe";
                    break;

                case TipoDFe.CTe:
                    typeString = "CTe";
                    break;

                case TipoDFe.MDFe:
                    typeString = "MDFe";
                    break;

                case TipoDFe.CFe:
                    typeString = "CFe";
                    break;
            }

            var pedacinhos = xml.Split(new string[] { $"Id=\"{typeString}" }, StringSplitOptions.None);

            if(pedacinhos.Length < 1)
            {
                return default;
            }

            return pedacinhos[1].Substring(0, 44);
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
        public static XmlDocument Serializar(object objeto, List<TNameSpace> nameSpaces = null)
        {
            if(objeto is null)
            {
                throw new ArgumentNullException(nameof(objeto));
            }

            var ns = new XmlSerializerNamespaces();
            if(nameSpaces != null)
            {
                for(var i = 0; i < nameSpaces.Count; i++)
                {
                    ns.Add(nameSpaces[i].Prefix, nameSpaces[i].NS);
                }
            }

            var xmlSerializer = new XmlSerializer(objeto.GetType());
            var doc = new XmlDocument();
            using(StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objeto, ns);
                doc.LoadXml(textWriter.ToString());
            }

            return doc;
        }

        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo da TAG.
        /// </summary>
        /// <param name="xmlElement">Elemento do XML onde será pesquisado o Nome da TAG</param>
        /// <param name="tagName">Nome da Tag que será pesquisado</param>
        /// <returns>Conteúdo da tag</returns>
        public static bool TagExist(XmlElement xmlElement, string tagName)
        {
            if(xmlElement is null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            return xmlElement.GetElementsByTagName(tagName).Count != 0;
        }

        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo da TAG.
        /// </summary>
        /// <param name="xmlElement">Elemento do XML onde será pesquisado o Nome da TAG</param>
        /// <param name="tagName">Nome da Tag que será pesquisado</param>
        /// <returns>Conteúdo da tag</returns>
        public static string TagRead(XmlElement xmlElement, string tagName)
        {
            if(xmlElement is null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            var content = string.Empty;

            if(xmlElement.GetElementsByTagName(tagName).Count != 0)
            {
                content = xmlElement.GetElementsByTagName(tagName)[0].InnerText;
            }

            return content;
        }

        #endregion Public Methods
    }
}
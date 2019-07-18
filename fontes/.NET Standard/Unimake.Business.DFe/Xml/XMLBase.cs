using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class XMLBase
    {
        protected List<XMLUtility.TNameSpace> NameSpaces;

        public XMLBase()
        {
            XmlRootAttribute attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
            NameSpaces = new List<XMLUtility.TNameSpace>
            {
                new XMLUtility.TNameSpace() { Prefix = "", NS = attribute.Namespace }
            };
        }

        /// <summary>
        /// Deserializar XML (Converte o XML para um objeto)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="doc">Conteúdo do XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        public virtual T LerXML<T>(XmlDocument doc)
            where T : new()
        {
            return XMLUtility.Deserializar<T>(doc);
        }

        /// <summary>
        /// Serializar o objeto (Converte o objeto para XML)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="retorno">Objeto a ser serializado</param>
        /// <returns>String contendo o XML</returns>
        public virtual XmlDocument GerarXML()
        {
            return XMLUtility.Serializar(this, NameSpaces);
        }
    }
}

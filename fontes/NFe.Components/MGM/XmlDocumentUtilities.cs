using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NFe.Components.MGM
{
    /// <summary>
    /// Utilitários para o tipo XMLDocument
    /// </summary>
    public class XmlDocumentUtilities
    {

        /// <summary>
        /// Retorna um nó XML pelo seu nome
        /// </summary>
        /// <param name="doc">Documento XML que contêm o nó. Se não existir irá retornar nulo.</param>
        /// <param name="name">Nome do nó que deverá ser pesquisado.</param>
        /// <returns></returns>
        public static XmlNode GetNode(XmlDocument doc, string name)
        {
            XmlNode result = doc.CreateElement(name);

            for(int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                XmlNode n = doc.DocumentElement.ChildNodes[i];
                if(n.LocalName == name)
                {
                    result = n;
                    break;
                }
            }

            return result;
        }

        public static T GetValue<T>(XmlDocument doc, int index)
        {
            return PrepareValue<T>(doc.DocumentElement.ChildNodes[index].InnerText);
        }
        public static T GetValue<T>(XmlDocument doc, string name)
        {
            XmlNode node = GetNode(doc, name);
            if(node == null) return default(T);
            return PrepareValue<T>(node.InnerText);
        }

        public static T GetValue<T>(XmlNode node, int index)
        {
            return PrepareValue<T>(node.ChildNodes[index].InnerText);
        }
        public static T GetValue<T>(XmlNode node, string name)
        {
            XmlNode localNode = null;

            foreach(XmlNode n in node.ChildNodes)
            {
                if(n.LocalName == name)
                {
                    localNode = n;
                    break;
                }
            }

            if(localNode == null) return default(T);

            return PrepareValue<T>(localNode.InnerText);
        }

        public static T PrepareValue<T>(object value)
        {
            T result = default(T);
            Type t = typeof(T);
            t = Nullable.GetUnderlyingType(t) ?? t;

            if (value != null && value != DBNull.Value)
            {
                if (t.IsEnum)
                {
                    result = ToEnum<T>(value);
                }
                else if (t.FullName.Equals(typeof(System.String).FullName))
                {
                    string s = value.ToString();

                    result = (T)Convert.ChangeType(s, t);

                }
                else
                {
                    try
                    {
                        result = (T)Convert.ChangeType(value, t);
                    }
                    catch (FormatException)
                    {
                        result = default(T);
                    }
                }
            }

            return result;
        }

        public static T ToEnum<T>(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return default(T);
            T result = default(T);
            result = (T)Enum.Parse(typeof(T), value.ToString(), true);
            return result;
        }
    }
}



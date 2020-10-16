using System.Reflection;
using System.Xml;

namespace Unimake.Business.DFe
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlReaderExtensions
    {
        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="name"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static T GetValue<T>(this XmlReader reader, string name, PropertyInfo propertyInfo = null)
        {
            T result = default;

            if(reader.NodeType == XmlNodeType.EndElement)
            {
                reader.Read();
            }

            do
            {
                if(reader.NodeType == XmlNodeType.Element &&
                   !reader.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase) ||
                   reader.NodeType == XmlNodeType.EndElement)
                {
                    return result;
                }

                if(reader.HasValue)
                {
                    if(propertyInfo != null)
                    {
                        result = (T)Utility.Converter.ToAny(propertyInfo.PropertyType, reader.Value);
                    }
                    else
                    {
                        result = Utility.Converter.ToAny<T>(reader.Value);
                    }

                    break;
                }
            } while(reader.Read());

            reader.Read();
            return result;
        }

        #endregion Public Methods
    }
}
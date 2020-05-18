using System.Xml;

namespace Unimake.Business.DFe
{
    public static class XmlReaderExtensions
    {
        #region Public Methods

        public static T GetValue<T>(this XmlReader reader, string name)
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
                    result = Utility.Converter.ToAny<T>(reader.Value);
                    break;
                }
            } while(reader.Read());

            reader.Read();
            return result;
        }

        #endregion Public Methods
    }
}
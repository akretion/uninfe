using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace Unimake.Business.DFe.Xml
{
    internal class DynamicXml: DynamicObject
    {
        #region Private Fields

        private XElement root;

        #endregion Private Fields

        #region Private Constructors

        private DynamicXml(XElement root) => this.root = root;

        #endregion Private Constructors

        #region Private Methods

        private static XElement RemoveNamespaces(XElement xElem)
        {
            var attrs = xElem.Attributes()
                        .Where(a => !a.IsNamespaceDeclaration)
                        .Select(a => new XAttribute(a.Name.LocalName, a.Value))
                        .ToList();

            if(!xElem.HasElements)
            {
                var xElement = new XElement(xElem.Name.LocalName, attrs)
                {
                    Value = xElem.Value
                };
                return xElement;
            }

            var newXElem = new XElement(xElem.Name.LocalName, xElem.Elements().Select(e => RemoveNamespaces(e)));
            newXElem.Add(attrs);
            return newXElem;
        }

        public override string ToString() => root.ToString();

        #endregion Private Methods

        #region Public Methods

        public static dynamic Load(string filename) => 
            new DynamicXml(RemoveNamespaces(XDocument.Load(filename).Root));

        public static dynamic Parse(string xmlString) => 
            new DynamicXml(RemoveNamespaces(XDocument.Parse(xmlString).Root));

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var att = root.Attribute(binder.Name);
            if(att != null)
            {
                result = att.Value;
                return true;
            }

            var nodes = root.Elements(binder.Name);
            if(nodes.Count() > 1)
            {
                result = nodes.Select(n => n.HasElements ? (object)new DynamicXml(n) : n.Value).ToList();
                return true;
            }

            var node = root.Element(binder.Name);
            if(node != null)
            {
                result = node.HasElements || node.HasAttributes ? (object)new DynamicXml(node) : node.Value;
                return true;
            }

            return true;
        }

        #endregion Public Methods
    }
}
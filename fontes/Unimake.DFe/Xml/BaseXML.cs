using System.Xml;

namespace Unimake.DFe.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseXml
    {
        public abstract void Ler(XmlDocument doc);
        public abstract XmlDocument Gerar();
    }
}

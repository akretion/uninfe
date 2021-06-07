using System.IO;
using System.Xml;

namespace Unimake.Business.DFe.Contract.Serialization
{
    /// <summary>
    /// Implementa métodos para serialização e deserialização de XMLs
    /// </summary>
    public interface IXmlSerializable
    {
        #region Public Methods

        /// <summary>
        /// Executa o processamento do XMLReader recebido na deserialização
        /// </summary>
        ///<param name="reader">Reader XML recebido durante o processo de deserialização</param>
        void ReadXml(XmlReader reader);

        /// <summary>
        /// Executa o processamento do XMLReader recebido na serialização
        /// </summary>
        ///<param name="writer">string XML recebido durante o processo de serialização</param>
        void WriteXml(StringWriter writer);

        #endregion Public Methods
    }
}
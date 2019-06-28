using System;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    public class InutNFe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string versao { get; set; }

        [XmlElement(ElementName = "infInut")]
        public InutNFeInfInut infInut = new InutNFeInfInut();
    }

    public class InutNFeInfInut
    {
        private UFBrasilIBGE UFField;

        public string Id { get; set; }
        [XmlElement(ElementName = "tpAmb")]
        public TipoAmbiente tpAmb { get; set; }
        [XmlElement(ElementName = "xServ")]
        public InutNFeInfInutXServ xServ { get; set; }
        [XmlElement(ElementName = "cUF")]
        public UFBrasilIBGE CUF
        {
            get => UFField;
            set => UFField = value;
        }
        public string ano { get; set; }
        public string CNPJ { get; set; }
        public string mod { get; set; }
        public string serie { get; set; }
        public string nNFIni { get; set; }
        public string nNFFin { get; set; }
        public string xJust { get; set; }

        /// <summary>
        /// Conteúdo da tag xServ
        /// </summary>
        public enum InutNFeInfInutXServ
        {
            INUTILIZAR
        }
    }
}

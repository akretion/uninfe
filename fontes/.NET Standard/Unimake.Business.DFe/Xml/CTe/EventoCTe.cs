using System;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.Xml.CTe
{
    [Serializable]
    [XmlRoot(ElementName = "detEvento")]
    public class DetEventoCanc: EventoDetalhe
    {
        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public override string DescEvento { get; set; } = "Cancelamento";

        [XmlElement("nProt", Order = 1)]
        public string NProt { get; set; }

        [XmlElement("xJust", Order = 2)]
        public string XJust { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteRaw($@"
            <evCancCTe>
            <descEvento>{DescEvento}</descEvento>
            <nProt>{NProt}</nProt>
            <xJust>{XJust}</xJust>
            </evCancCTe>");
        }

        #endregion Public Methods
    }

    [Serializable]
    [XmlRoot(ElementName = "detEvento")]
    public class DetEventoCCE: EventoDetalhe
    {
    }

    [Serializable]
    [XmlRoot(ElementName = "detEvento")]
    public class DetEventoManif: EventoDetalhe
    {
    }

    [Serializable()]
    [XmlRoot("eventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class EventoCTe: XMLBase
    {
        #region Private Methods

        private void SignEvent(EventoCTe evento, XmlElement xmlEl)
        {
            var signature = xmlEl.GetElementsByTagName("Signature")[0];
            if(signature != null)
            {
                var signatureEvento = new XmlDocument();

                signatureEvento.LoadXml(signature.OuterXml);
                evento.Signature = XMLUtility.Deserializar<Signature>(signatureEvento);
            }
        }

        #endregion Private Methods

        #region Public Properties

        [XmlElement("infEvento", Order = 0)]
        public InfEvento InfEvento { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#", Order = 1)]
        public Signature Signature { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override XmlDocument GerarXML()
        {
            var xmlDocument = base.GerarXML();

            #region Adicionar o atributo de namespace que falta nas tags "evento"

            var attribute = GetType().GetCustomAttribute<XmlRootAttribute>();

            for(var i = 0; i < xmlDocument.GetElementsByTagName("evento").Count; i++)
            {
                var xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("evento")[i];
                xmlElement.SetAttribute("xmlns", attribute.Namespace);
            }

            #endregion Adicionar o atributo de namespace que falta nas tags "evento"

            return xmlDocument;
        }

        public override T LerXML<T>(XmlDocument doc)
        {
            if(typeof(T) != typeof(EventoCTe))
            {
                throw new InvalidCastException($"Cannot cast type '{typeof(T).Name}' into type '{typeof(EventoCTe).Name}'.");
            }

            var retornar = base.LerXML<T>(doc) as EventoCTe;
            var eventos = doc.GetElementsByTagName("eventoCTe");

            if((eventos?.Count ?? 0) > 0)
            {
                var xmlEl = (XmlElement)eventos[0];

                var xml = new StringBuilder();
                xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xml.Append($"<eventoCTe versao=\"3.00\" xmlns=\"{xmlEl.NamespaceURI}\">");
                xml.Append($"{xmlEl.InnerXml}</eventoCTe>");

                var envEvt = XMLUtility.Deserializar<EventoCTe>(xml.ToString());
                var evt = envEvt;
                SignEvent(evt, xmlEl);
                retornar = evt;
            }

            return (T)(object)retornar;
        }

        #endregion Public Methods
    }

    [XmlInclude(typeof(DetEventoCanc))]
    [XmlInclude(typeof(DetEventoCCE))]
    public class EventoDetalhe: IXmlSerializable
    {
        #region Private Methods

        private void SetPropertyValue(string attributeName)
        {
            PropertyInfo pi;

            if(XmlReader.GetAttribute(attributeName) != "")
            {
                pi = GetType().GetProperty(attributeName, BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.IgnoreCase);
                if(!(pi?.CanWrite ?? false))
                {
                    return;
                }

                pi?.SetValue(this, XmlReader.GetAttribute(attributeName));
            }
        }

        #endregion Private Methods

        #region Internal Properties

        internal XmlReader XmlReader { get; set; }

        #endregion Internal Properties

        #region Internal Methods

        internal virtual void ProcessReader()
        {
            if(XmlReader == null)
            {
                return;
            }

            var pi = default(PropertyInfo);
            var type = GetType();

            SetPropertyValue("versao");
            SetPropertyValue("versaoEvento");

            while(XmlReader.Read())
            {
                if(XmlReader.NodeType == XmlNodeType.Element)
                {
                    pi = type.GetProperty(XmlReader.Name, BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.IgnoreCase);
                }
                else if(XmlReader.NodeType == XmlNodeType.Text)
                {
                    pi?.SetValue(this, XmlReader.Value);
                }
            }
        }

        #endregion Internal Methods

        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public virtual string DescEvento { get; set; }

        [XmlAttribute(AttributeName = "versaoEvento", DataType = "token")]
        public virtual string VersaoEvento { get; set; }

        #endregion Public Properties

        #region Public Methods

        public XmlSchema GetSchema() => default;

        public void ReadXml(XmlReader reader) => XmlReader = reader;

        public virtual void WriteXml(XmlWriter writer) => writer.WriteAttributeString("versaoEvento", VersaoEvento);

        #endregion Public Methods
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEvento
    {
        #region Private Fields

        private EventoDetalhe _detEvento;

        #endregion Private Fields

        #region Public Properties

        [XmlElement("chCTe", Order = 3)]
        public string ChCTe { get; set; }

        [XmlElement("CNPJ", Order = 2)]
        public string CNPJ { get; set; }

        [XmlIgnore]
        public UFBrasil COrgao { get; set; }

        [XmlElement("cOrgao", Order = 0)]
        public int COrgaoField
        {
            get => (int)COrgao;
            set => COrgao = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlElement("detEvento", Order = 9)]
        public EventoDetalhe DetEvento
        {
            get => _detEvento;
            set
            {
                switch(TpEvento)
                {
                    case 0:
                        _detEvento = value;
                        break;

                    //case TipoEventoCTe.CartaCorrecao:
                    //    _detEvento = new DetEventoCCE();
                    //    break;

                    case TipoEventoCTe.Cancelamento:
                        _detEvento = new DetEventoCanc();
                        break;

                    //case TipoEventoNFe.ManifestacaoConfirmacaoOperacao:
                    //case TipoEventoNFe.ManifestacaoCienciaOperacao:
                    //case TipoEventoNFe.ManifestacaoDesconhecimentoOperacao:
                    //case TipoEventoNFe.ManifestacaoOperacaoNaoRealizada:
                    //    _detEvento = new DetEventoManif();
                    //    break;

                    //case TipoEventoNFe.CancelamentoPorSubstituicao:
                    //case TipoEventoNFe.EPEC:
                    //case TipoEventoNFe.PedidoProrrogacao:
                    default:
                        throw new NotImplementedException($"O tipo de evento '{TpEvento}' não está implementado.");
                }

                _detEvento.XmlReader = value.XmlReader;
                _detEvento.ProcessReader();
            }
        }

        [XmlIgnore]
        public DateTime DhEvento { get; set; }

        [XmlElement("dhEvento", Order = 5)]
        public string DhEventoField
        {
            get => DhEvento.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhEvento = DateTime.Parse(value);
        }

        [XmlAttribute(DataType = "ID")]
        public string Id
        {
            get => "ID" + ((int)TpEvento).ToString() + ChCTe + NSeqEvento.ToString("00");
            set => _ = value;
        }

        [XmlElement("nSeqEvento", Order = 7)]
        public int NSeqEvento { get; set; }

        [XmlElement("tpAmb", Order = 1)]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("tpEvento", Order = 6)]
        public TipoEventoCTe TpEvento { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public InfEvento()
        {
        }

        public InfEvento(EventoDetalhe detEvento) => DetEvento = detEvento ?? throw new ArgumentNullException(nameof(detEvento));

        #endregion Public Constructors

        #region Public Methods

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        #endregion Public Methods
    }
}
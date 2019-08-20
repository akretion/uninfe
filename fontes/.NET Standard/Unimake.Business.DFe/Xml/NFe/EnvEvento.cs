using System;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.Xml.NFe
{
    [Serializable()]
    [XmlRoot("envEvento", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class EnvEvento : XMLBase
    {
        #region Public Properties

        [XmlElement("evento", Order = 2)]
        public Evento[] Evento { get; set; }

        [XmlElement("idLote", Order = 1)]
        public string IdLote { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override XmlDocument GerarXML()
        {
            var xmlDocument = base.GerarXML();

            var attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
            var xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("evento")[0];
            xmlElement.SetAttribute("xmlns", attribute.Namespace);

            return xmlDocument;
        }

        public override T LerXML<T>(XmlDocument doc)
        {
            var retornar = base.LerXML<T>(doc);

            if (doc.GetElementsByTagName("Signature")[0] != null)
            {
                var signatureEvento = new XmlDocument();

                signatureEvento.LoadXml(doc.GetElementsByTagName("Signature")[0].OuterXml);
                ((EnvEvento)(object)retornar).Evento[0].Signature = XMLUtility.Deserializar<Signature>(signatureEvento);
            }

            return retornar;
        }

        #endregion Public Methods
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Evento
    {
        #region Public Properties

        [XmlElement("infEvento", Order = 0)]
        public InfEvento InfEvento { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#", Order = 1)]
        public Signature Signature { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        #endregion Public Properties
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfEvento
    {
        #region Private Fields

        private EventoDetalhe _detEvento;

        #endregion Private Fields

        #region Public Properties

        [XmlElement("chNFe", Order = 4)]
        public string ChNFe { get; set; }

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

        [XmlElement("CPF", Order = 3)]
        public string CPF { get; set; }

        [XmlElement("detEvento", Order = 9)]
        public EventoDetalhe DetEvento
        {
            get => _detEvento;
            set
            {
                switch (TpEvento)
                {
                    case 0:
                        _detEvento = value;
                        break;

                    case TipoEventoNFe.CartaCorrecao:
                        _detEvento = new DetEventoCCE();
                        break;

                    case TipoEventoNFe.Cancelamento:
                        _detEvento = new DetEventoCanc();
                        break;

                    case TipoEventoNFe.CancelamentoPorSubstituicao:
                    case TipoEventoNFe.EPEC:
                    case TipoEventoNFe.PedidoProrrogacao:
                    case TipoEventoNFe.ManifestacaoConfirmacaoOperacao:
                    case TipoEventoNFe.ManifestacaoCienciaOperacao:
                    case TipoEventoNFe.ManifestacaoDesconhecimentoOperacao:
                    case TipoEventoNFe.ManifestacaoOperacaoNaoRealizada:
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
            get => DhEvento.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhEvento = DateTime.Parse(value);
        }

        [XmlAttribute(DataType = "ID")]
        public string Id
        {
            get => "ID" + ((int)TpEvento).ToString() + ChNFe + NSeqEvento.ToString("00");
            set => _ = value;
        }

        [XmlElement("nSeqEvento", Order = 7)]
        public int NSeqEvento { get; set; }

        [XmlElement("tpAmb", Order = 1)]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("tpEvento", Order = 6)]
        public TipoEventoNFe TpEvento { get; set; }

        [XmlElement("verEvento", Order = 8)]
        public string VerEvento { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public InfEvento() { }

        public InfEvento(EventoDetalhe detEvento)
        {
            DetEvento = detEvento ?? throw new ArgumentNullException(nameof(detEvento));
        }

        #endregion Public Constructors

        #region Public Methods

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }

        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrWhiteSpace(CPF);
        }

        #endregion Public Methods
    }

    [Serializable]
    [XmlRoot(ElementName = "detEvento")]
    public class DetEventoCanc : EventoDetalhe
    {
        #region Internal Methods

        internal override void ProcessReader()
        {
            base.ProcessReader();
        }

        #endregion Internal Methods

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
            <descEvento>{DescEvento}</descEvento>
            <nProt>{NProt}</nProt>
            <xJust>{XJust}</xJust>");
        }

        #endregion Public Methods
    }

    [Serializable]
    [XmlRoot(ElementName = "detEvento")]
    public class DetEventoCCE : EventoDetalhe
    {
        #region Internal Methods

        internal override void ProcessReader()
        {
            base.ProcessReader();
        }

        #endregion Internal Methods

        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public override string DescEvento { get; set; } = "Carta de Correcao";

        [XmlElement("xCondUso", Order = 2)]
        public string XCondUso { get; set; } = "A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido na emissao de documento fiscal, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da operacao ou da prestacao; II - a correcao de dados cadastrais que implique mudanca do remetente ou do destinatario; III - a data de emissao ou de saida.";

        [XmlElement("xCorrecao", Order = 1)]
        public string XCorrecao { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
        }

        #endregion Public Methods
    }

    [XmlInclude(typeof(DetEventoCanc))]
    [XmlInclude(typeof(DetEventoCCE))]
    public class EventoDetalhe : IXmlSerializable
    {
        #region Internal Properties

        internal XmlReader XmlReader { get; set; }

        #endregion Internal Properties

        #region Internal Methods

        internal virtual void ProcessReader()
        {
            if (XmlReader == null)
            {
                return;
            }

            var pi = default(PropertyInfo);
            var type = GetType();

            if (XmlReader.HasAttributes)
            {
                if (XmlReader.GetAttribute("versao") != "")
                {
                    pi = type.GetProperty("versao", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    pi?.SetValue(this, XmlReader.GetAttribute("versao"));
                }
            }

            while (XmlReader.Read())
            {
                if (XmlReader.NodeType == XmlNodeType.Element)
                {
                    pi = type.GetProperty(XmlReader.Name, BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.IgnoreCase);
                }
                else if (XmlReader.NodeType == XmlNodeType.Text)
                {
                    pi?.SetValue(this, XmlReader.Value);
                }
            }
        }

        #endregion Internal Methods

        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public virtual string DescEvento { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public virtual string Versao { get; set; }

        #endregion Public Properties

        #region Public Methods

        public XmlSchema GetSchema()
        {
            return default;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlReader = reader;
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("versao", Versao);
        }

        #endregion Public Methods
    }
}
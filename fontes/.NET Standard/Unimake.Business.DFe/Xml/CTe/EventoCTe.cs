using System;
using System.Collections.Generic;
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
    public class DetEventoPrestDesacordo : EventoDetalhe
    {
        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public override string DescEvento { get; set; } = "Prestacao do Servico em Desacordo";

        [XmlElement("indDesacordoOper", Order = 1)]
        public string IndDesacordoOper { get; set; }

        [XmlElement("xObs", Order = 2)]
        public string XObs { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteRaw($@"
            <evPrestDesacordo>
            <descEvento>{DescEvento}</descEvento>
            <indDesacordoOper>{IndDesacordoOper}</indDesacordoOper>
            <xObs>{XObs}</xObs>
            </evPrestDesacordo>");
        }

        #endregion Public Methods
    }

    [Serializable]
    [XmlRoot(ElementName = "detEvento")]
    public class DetEventoCancCompEntrega: EventoDetalhe
    {
        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public override string DescEvento { get; set; } = "Cancelamento do Comprovante de Entrega do CT-e";

        [XmlElement("nProt", Order = 1)]
        public string NProt { get; set; }

        [XmlElement("nProtCE", Order = 2)]
        public string NProtCE { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteRaw($@"
            <evCancCECTe>
            <descEvento>{DescEvento}</descEvento>
            <nProt>{NProt}</nProt>
            <nProtCE>{NProtCE}</nProtCE>
            </evCancCECTe>");
        }

        #endregion Public Methods
    }

    [XmlRoot(ElementName = "detEvento")]
    [XmlInclude(typeof(EventoDetalhe))]
    public class DetEventoCCE: EventoDetalhe
    {
        #region Private Fields

        private EventoCCeCTe _eventoCCeCTe;

        #endregion Private Fields

        #region Internal Methods

        internal override void SetValue(PropertyInfo pi)
        {
            if(pi?.Name == nameof(InfCorrecao))
            {
                XmlReader.Read();
                var grupoAlterado = XmlReader.GetValue<string>(nameof(Xml.CTe.InfCorrecao.GrupoAlterado));
                var campoAlterado = XmlReader.GetValue<string>(nameof(Xml.CTe.InfCorrecao.CampoAlterado));
                var valorAlterado = XmlReader.GetValue<string>(nameof(Xml.CTe.InfCorrecao.ValorAlterado));
                var nroItemAlterado = XmlReader.GetValue<string>(nameof(Xml.CTe.InfCorrecao.NroItemAlterado));

                InfCorrecao.Add(new InfCorrecao
                {
                    GrupoAlterado = grupoAlterado,
                    CampoAlterado = campoAlterado,
                    ValorAlterado = valorAlterado,
                    NroItemAlterado = nroItemAlterado
                });

                return;
            }

            base.SetValue(pi);
        }

        #endregion Internal Methods

        #region Public Properties

        [XmlIgnore]
        public override string DescEvento
        {
            get => EventoCCeCTe.DescEvento;
            set => EventoCCeCTe.DescEvento = value;
        }

        [XmlElement(ElementName = "evCCeCTe", Order = 0)]
        public EventoCCeCTe EventoCCeCTe
        {
            get => _eventoCCeCTe ?? (_eventoCCeCTe = new EventoCCeCTe());
            set => _eventoCCeCTe = value;
        }

        [XmlIgnore]
        public List<InfCorrecao> InfCorrecao
        {
            get => EventoCCeCTe.InfCorrecao;
            set => EventoCCeCTe.InfCorrecao = value;
        }

        [XmlIgnore]
        public string XCondUso
        {
            get => EventoCCeCTe.XCondUso;
            set => EventoCCeCTe.XCondUso = value;
        }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            var writeRaw = $@"<evCCeCTe>" +
                $@"<descEvento>{DescEvento}</descEvento>";

            foreach(var infCorrecao in InfCorrecao)
            {
                writeRaw += $@"<infCorrecao>" +
                    $@"<grupoAlterado>{infCorrecao.GrupoAlterado}</grupoAlterado>" +
                    $@"<campoAlterado>{infCorrecao.CampoAlterado}</campoAlterado>" +
                    $@"<valorAlterado>{infCorrecao.ValorAlterado}</valorAlterado>" +
                    $@"</infCorrecao>";
            }

            writeRaw += $@"<xCondUso>{XCondUso}</xCondUso>" +
                $@"</evCCeCTe>";

            writer.WriteRaw(writeRaw);
        }

        #endregion Public Methods
    }

    [XmlInclude(typeof(EventoDetalhe))]
    [XmlRoot(ElementName = "detEvento")]
    public class DetEventoCompEntrega: EventoDetalhe
    {
        #region Private Fields

        private EventoCECTe _eventoCECTe;

        #endregion Private Fields

        #region Internal Methods

        internal override void SetValue(PropertyInfo pi)
        {
            if(pi.Name == nameof(InfEntrega))
            {
                XmlReader.Read();
                InfEntrega.Add(new InfEntrega
                {
                    ChNFe = XmlReader.GetValue<string>(nameof(Xml.CTe.InfEntrega.ChNFe))
                });
                return;
            }

            base.SetValue(pi);
        }

        #endregion Internal Methods

        #region Public Properties

        [XmlIgnore]
        public override string DescEvento
        {
            get => EventoCECTe.DescEvento;
            set => EventoCECTe.DescEvento = value;
        }

        [XmlIgnore]
        public DateTime DhEntrega
        {
            get => EventoCECTe.DhEntrega;
            set => EventoCECTe.DhEntrega = value;
        }

        [XmlIgnore]
        public string DhEntregaField
        {
            get => EventoCECTe.DhEntregaField;
            set => EventoCECTe.DhEntregaField = value;
        }

        [XmlIgnore]
        public DateTime DhHashEntrega
        {
            get => EventoCECTe.DhHashEntrega;
            set => EventoCECTe.DhHashEntrega = value;
        }

        [XmlIgnore]
        public string DhHashEntregaField
        {
            get => EventoCECTe.DhHashEntregaField;
            set => EventoCECTe.DhHashEntregaField = value;
        }

        [XmlElement(ElementName = "evCECTe", Order = 0)]
        public EventoCECTe EventoCECTe
        {
            get => _eventoCECTe ?? (_eventoCECTe = new EventoCECTe());
            set => _eventoCECTe = value;
        }

        [XmlIgnore]
        public string HashEntrega
        {
            get => EventoCECTe.HashEntrega;
            set => EventoCECTe.HashEntrega = value;
        }

        [XmlIgnore]
        public List<InfEntrega> InfEntrega
        {
            get => EventoCECTe.InfEntrega;
            set => EventoCECTe.InfEntrega = value;
        }

        [XmlIgnore]
        public string Latitude
        {
            get => EventoCECTe.Latitude;
            set => EventoCECTe.Latitude = value;
        }

        [XmlIgnore]
        public string Longitude
        {
            get => EventoCECTe.Longitude;
            set => EventoCECTe.Longitude = value;
        }

        [XmlIgnore]
        public string NDoc
        {
            get => EventoCECTe.NDoc;
            set => EventoCECTe.NDoc = value;
        }

        [XmlIgnore]
        public string NProt
        {
            get => EventoCECTe.NProt;
            set => EventoCECTe.NProt = value;
        }

        [XmlIgnore]
        public string XNome
        {
            get => EventoCECTe.XNome;
            set => EventoCECTe.XNome = value;
        }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            var writeRaw = $@"<evCECTe>
                <descEvento>{DescEvento}</descEvento>
                <nProt>{NProt}</nProt>
                <dhEntrega>{DhEntregaField}</dhEntrega>
                <nDoc>{NDoc}</nDoc>
                <xNome>{XNome}</xNome>
                <latitude>{Latitude}</latitude>
                <longitude>{Longitude}</longitude>
                <hashEntrega>{HashEntrega}</hashEntrega>
                <dhHashEntrega>{DhHashEntregaField}</dhHashEntrega>";

            foreach(var infEntrega in InfEntrega)
            {
                writeRaw += $@"<infEntrega><chNFe>{infEntrega.ChNFe}</chNFe></infEntrega>";
            }

            writeRaw += $@"</evCECTe>";

            writer.WriteRaw(writeRaw);
        }

        #endregion Public Methods
    }

    [Serializable]
    [XmlRoot(ElementName = "detEvento")]
    public class EventoCCeCTe: EventoDetalhe
    {
        #region Private Fields

        private string XCondUsoField = "A Carta de Correcao e disciplinada pelo Art. 58-B do CONVENIO/SINIEF 06/89: Fica permitida a utilizacao de carta de correcao, para regularizacao de erro ocorrido na emissao de documentos fiscais relativos a prestacao de servico de transporte, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da prestacao;II - a correcao de dados cadastrais que implique mudanca do emitente, tomador, remetente ou do destinatario;III - a data de emissao ou de saida.";

        #endregion Private Fields

        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public override string DescEvento { get; set; } = "Carta de Correcao";

        [XmlElement("infCorrecao", Order = 1)]
        public List<InfCorrecao> InfCorrecao { get; set; } = new List<InfCorrecao>();

        [XmlElement("xCondUso", Order = 2)]
        public string XCondUso
        {
            get => XCondUsoField;
            set => XCondUsoField = value;
        }

        #endregion Public Properties

        #region Public Methods

        public void AddInfCorrecao(InfCorrecao infcorrecao)
        {
            if(InfCorrecao == null)
            {
                InfCorrecao = new List<InfCorrecao>();
            }

            InfCorrecao.Add(infcorrecao);
        }

        #endregion Public Methods
    }

    [XmlRoot(ElementName = "evCECTe")]
    [XmlInclude(typeof(EventoDetalhe))]
    public class EventoCECTe: EventoDetalhe
    {
        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public override string DescEvento { get; set; } = "Comprovante de Entrega";

        [XmlIgnore]
        public DateTime DhEntrega { get; set; }

        [XmlElement("dhEntrega", Order = 2)]
        public string DhEntregaField
        {
            get => DhEntrega.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhEntrega = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime DhHashEntrega { get; set; }

        [XmlElement("dhHashEntrega", Order = 8)]
        public string DhHashEntregaField
        {
            get => DhHashEntrega.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhHashEntrega = DateTime.Parse(value);
        }

        [XmlElement("hashEntrega", Order = 7)]
        public string HashEntrega { get; set; }

        [XmlElement("infEntrega", Order = 9)]
        public List<InfEntrega> InfEntrega { get; set; } = new List<InfEntrega>();

        [XmlElement("latitude", Order = 5)]
        public string Latitude { get; set; }

        [XmlElement("longitude", Order = 6)]
        public string Longitude { get; set; }

        [XmlElement("nDoc", Order = 3)]
        public string NDoc { get; set; }

        [XmlElement("nProt", Order = 1)]
        public string NProt { get; set; }

        [XmlElement("xNome", Order = 4)]
        public string XNome { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void AddInfEntrega(InfEntrega infentrega)
        {
            if(InfEntrega == null)
            {
                InfEntrega = new List<InfEntrega>();
            }

            InfEntrega.Add(infentrega);
        }

        #endregion Public Methods
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
    [XmlInclude(typeof(DetEventoCancCompEntrega))]
    [XmlInclude(typeof(DetEventoCompEntrega))]
    [XmlInclude(typeof(EventoCECTe))]
    [XmlInclude(typeof(DetEventoPrestDesacordo))]
    public class EventoDetalhe: IXmlSerializable
    {
        #region Private Fields

        private static readonly BindingFlags bindingFlags = BindingFlags.Public |
                                                            BindingFlags.Instance |
                                                            BindingFlags.IgnoreCase;

        private static readonly List<string> hasField = new List<string>
        {
            "DhEntrega",
            "DhHashEntrega"
        };

        #endregion Private Fields

        #region Private Methods

        private bool SetLocalValue(Type type)
        {
            var pi = GetPropertyInfo(type);
            if(pi == null)
            {
                return false;
            }

            SetValue(pi);
            return true;
        }

        private void SetPropertyValue(string attributeName)
        {
            PropertyInfo pi;

            if(XmlReader.GetAttribute(attributeName) != "")
            {
                pi = GetType().GetProperty(attributeName, bindingFlags);
                if(!(pi?.CanWrite ?? false))
                {
                    return;
                }

                pi?.SetValue(this, XmlReader.GetAttribute(attributeName));
            }
        }

        #endregion Private Methods

        #region Protected Internal Methods

        protected internal PropertyInfo GetPropertyInfo(Type type)
        {
            var pi = hasField.Exists(w => w.ToLower() == XmlReader.Name.ToLower()) ?
                                type.GetProperty(XmlReader.Name + "Field", bindingFlags) :
                                type.GetProperty(XmlReader.Name, bindingFlags);
            return pi;
        }

        #endregion Protected Internal Methods

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

            var type = GetType();

            SetPropertyValue("versao");
            SetPropertyValue("versaoEvento");

            while(XmlReader.Read())
            {
                if(XmlReader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                if(SetLocalValue(type) &&
                   XmlReader.NodeType == XmlNodeType.Element)
                {
                    SetLocalValue(type);
                }
            }
        }

        internal virtual void SetValue(PropertyInfo pi) => pi?.SetValue(this, XmlReader.GetValue<object>(XmlReader.Name));

        #endregion Internal Methods

        #region Public Properties

        [XmlElement("descEvento", Order = 0)]
        public virtual string DescEvento { get; set; }

        [XmlAttribute(AttributeName = "versaoEvento")]
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
    public class InfCorrecao
    {
        #region Public Properties

        [XmlElement("campoAlterado", Order = 1)]
        public string CampoAlterado { get; set; }

        [XmlElement("grupoAlterado", Order = 0)]
        public string GrupoAlterado { get; set; }

        [XmlElement("nroItemAlterado", Order = 3)]
        public string NroItemAlterado { get; set; }

        [XmlElement("valorAlterado", Order = 2)]
        public string ValorAlterado { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool ShouldSerializeNroItemAlterado() => !string.IsNullOrWhiteSpace(NroItemAlterado);

        #endregion Public Methods
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEntrega
    {
        #region Private Fields

        private string ChNFeField;

        #endregion Private Fields

        #region Public Properties

        [XmlElement("chNFe", Order = 0)]
        public string ChNFe
        {
            get => ChNFeField;
            set
            {
                if(value.Length != 44)
                {
                    throw new Exception("Conteúdo da tag <chNFe> filha da tag <infEntrega> inválido! O conteúdo da tag deve ter 44 dígitos.");
                }

                ChNFeField = value;
            }
        }

        #endregion Public Properties
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

        [XmlElement("detEvento", Order = 7)]
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

                    case TipoEventoCTe.Cancelamento:
                        _detEvento = new DetEventoCanc();
                        break;

                    case TipoEventoCTe.ComprovanteEntrega:
                        _detEvento = new DetEventoCompEntrega();
                        break;

                    case TipoEventoCTe.CancelamentoComprovanteEntrega:
                        _detEvento = new DetEventoCancCompEntrega();
                        break;

                    case TipoEventoCTe.CartaCorrecao:
                        _detEvento = new DetEventoCCE();
                        break;

                    case TipoEventoCTe.PrestDesacordo:
                        _detEvento = new DetEventoPrestDesacordo();
                        break;

                    default:
                        throw new NotImplementedException($"O tipo de evento '{TpEvento}' não está implementado.");
                }

                _detEvento.XmlReader = value.XmlReader;
                _detEvento.ProcessReader();
            }
        }

        [XmlIgnore]
        public DateTime DhEvento { get; set; }

        [XmlElement("dhEvento", Order = 4)]
        public string DhEventoField
        {
            get => DhEvento.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhEvento = DateTime.Parse(value);
        }

        [XmlAttribute(DataType = "ID", AttributeName = "Id")]
        public string Id
        {
            get => "ID" + ((int)TpEvento).ToString() + ChCTe + NSeqEvento.ToString("00");
            set => _ = value;
        }

        [XmlElement("nSeqEvento", Order = 6)]
        public int NSeqEvento { get; set; }

        [XmlElement("tpAmb", Order = 1)]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("tpEvento", Order = 5)]
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
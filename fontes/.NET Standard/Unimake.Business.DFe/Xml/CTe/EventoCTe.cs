#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.Xml.CTe
{
    [Serializable()]
    [XmlRoot("eventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class EventoCTe<TDetalheEvento>: XMLBase
    {
        [XmlElement("infEvento", Order = 0)]
        public TDetalheEvento InfEvento { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#", Order = 1)]
        public Signature Signature { get; set; }

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }
    }

    public interface IInfEvento
    {
        UFBrasil COrgao { get; set; }
        TipoAmbiente TpAmb { get; set; }
        TipoEventoCTe TpEvento { get; set; }
        string ChCTe { get; set; }
        int NSeqEvento { get; set; }
    }

    public abstract class InfEvento : IInfEvento
    {
        [XmlIgnore]
        public UFBrasil COrgao { get; set; }

        [XmlElement("cOrgao", Order = 0)]
        public int COrgaoField
        {
            get => (int)COrgao;
            set => COrgao = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlElement("tpAmb", Order = 1)]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("CNPJ", Order = 2)]
        public string CNPJ { get; set; }

        [XmlElement("CPF", Order = 3)]
        public string CPF { get; set; }

        [XmlElement("chCTe", Order = 4)]
        public string ChCTe { get; set; }

        [XmlIgnore]
        public DateTime DhEvento { get; set; }

        [XmlElement("dhEvento", Order = 5)]
        public string DhEventoField
        {
            get => DhEvento.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhEvento = DateTime.Parse(value);
        }

        [XmlElement("tpEvento", Order = 6)]
        public TipoEventoCTe TpEvento { get; set; }

        [XmlElement("nSeqEvento", Order = 7)]
        public int NSeqEvento { get; set; }

        [XmlIgnore]
        public abstract InfSolicNFF InfSolicNFF { get; set; }

        [XmlAttribute(DataType = "ID", AttributeName = "Id")]
        public string Id
        {
            get => "ID" + ((int)TpEvento).ToString() + ChCTe + NSeqEvento.ToString("00");
            set => _ = value;
        }

        #region SholdSerializeCNPJ

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEventoCanc: InfEvento
    {
        [XmlElement("detEvento", Order = 8)]
        public DetEventoCanc DetEvento { get; set; }

        [XmlElement("infSolicNFF", Order = 9)]
        public override InfSolicNFF InfSolicNFF { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEventoEPEC: InfEvento
    {
        [XmlElement("detEvento", Order = 8)]
        public DetEventoEPEC DetEvento { get; set; }

        [XmlElement("infSolicNFF", Order = 9)]
        public override InfSolicNFF InfSolicNFF { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEventoPrestDesacordo: InfEvento
    {
        [XmlElement("detEvento", Order = 8)]
        public DetEventoPrestDesacordo DetEvento { get; set; }

        [XmlElement("infSolicNFF", Order = 9)]
        public override InfSolicNFF InfSolicNFF { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEventoCCe: InfEvento
    {
        [XmlElement("detEvento", Order = 8)]
        public DetEventoCCe DetEvento { get; set; }

        [XmlElement("infSolicNFF", Order = 9)]
        public override InfSolicNFF InfSolicNFF { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEventoCompEntrega: InfEvento
    {
        [XmlElement("detEvento", Order = 8)]
        public DetEventoCompEntrega DetEvento { get; set; }

        [XmlElement("infSolicNFF", Order = 9)]
        public override InfSolicNFF InfSolicNFF { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEventoCancCompEntrega: InfEvento
    {
        [XmlElement("detEvento", Order = 8)]
        public DetEventoCancCompEntrega DetEvento { get; set; }

        [XmlElement("infSolicNFF", Order = 9)]
        public override InfSolicNFF InfSolicNFF { get; set; }
    }

    public abstract class DetEvento
    {
        [XmlAttribute(AttributeName = "versaoEvento", DataType = "token")]
        public string VersaoEvento { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetEventoCanc: DetEvento
    {
        [XmlElement("evCancCTe")]
        public EvCancCTe EvCancCTe { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetEventoEPEC: DetEvento
    {
        [XmlElement("evEPECCTe")]
        public EvEPECCTe EvEPECCTe { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetEventoPrestDesacordo: DetEvento
    {
        [XmlElement("evPrestDesacordo")]
        public EvPrestDesacordo EvPrestDesacordo { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetEventoCCe: DetEvento
    {
        [XmlElement("evCCeCTe")]
        public EvCCeCTe EvCCeCTe { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetEventoCompEntrega: DetEvento
    {
        [XmlElement("evCECTe")]
        public EvCECTe EvCECTe { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetEventoCancCompEntrega: DetEvento
    {
        [XmlElement("evCancCECTe")]
        public EvCancCECTe EvCancCECTe { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EvCancCTe
    {
        [XmlElement("descEvento", Order = 0)]
        public string DescEvento { get; set; } = "Cancelamento";

        [XmlElement("nProt", Order = 1)]
        public string NProt { get; set; }

        [XmlElement("xJust", Order = 2)]
        public string XJust { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EvEPECCTe
    {
        [XmlElement("descEvento", Order = 0)]
        public string DescEvento { get; set; } = "EPEC";

        [XmlElement("xJust", Order = 1)]
        public string XJust { get; set; }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS", Order = 2)]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST", Order = 3)]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSST = Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VTPrest { get; set; }

        [XmlElement("vTPrest", Order = 4)]
        public string VTPrestField
        {
            get => VTPrest.ToString("F2", CultureInfo.InvariantCulture);
            set => VTPrest = Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCarga { get; set; }

        [XmlElement("vCarga", Order = 5)]
        public string VCargaField
        {
            get => VCarga.ToString("F2", CultureInfo.InvariantCulture);
            set => VCarga = Converter.ToDouble(value);
        }

        [XmlElement("toma4", Order = 6)]
        public EvEPECCTeToma4 Toma4 { get; set; }

        [XmlElement("modal", Order = 7)]
        public ModalidadeTransporteCTe Modal { get; set; }

        [XmlElement("UFIni", Order = 8)]
        public UFBrasil UFIni { get; set; }

        [XmlElement("UFFim", Order = 9)]
        public UFBrasil UFFim { get; set; }

        [XmlElement("tpCTe", Order = 10)]
        public TipoCTe TpCTe { get; set; }

        [XmlIgnore]
        public DateTime DhEmi { get; set; }

        [XmlElement("dhEmi", Order = 11)]
        public string DhEmiField
        {
            get => DhEmi.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhEmi = DateTime.Parse(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EvEPECCTeToma4
    {
        private TomadorServicoCTe TomaField;

        [XmlElement("toma", Order = 0)]
        public TomadorServicoCTe Toma
        {
            get => TomadorServicoCTe.Outros;
            set => TomaField = value;
        }

        [XmlElement("UF", Order = 1)]
        public UFBrasil UF { get; set; }

        [XmlElement("CNPJ", Order = 2)]
        public string CNPJ { get; set; }

        [XmlElement("CPF", Order = 3)]
        public string CPF { get; set; }

        [XmlElement("IE", Order = 4)]
        public string IE { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EvPrestDesacordo
    {
        [XmlElement("descEvento", Order = 0)]
        public string DescEvento { get; set; } = "Prestacao do Servico em Desacordo";

        [XmlElement("indDesacordoOper", Order = 1)]
        public string IndDesacordoOper { get; set; }

        [XmlElement("xObs", Order = 2)]
        public string XObs { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EvCCeCTe
    {
        private string XCondUsoField = "A Carta de Correcao e disciplinada pelo Art. 58-B do CONVENIO/SINIEF 06/89: Fica permitida a utilizacao de carta de correcao, para regularizacao de erro ocorrido na emissao de documentos fiscais relativos a prestacao de servico de transporte, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da prestacao;II - a correcao de dados cadastrais que implique mudanca do emitente, tomador, remetente ou do destinatario;III - a data de emissao ou de saida.";

        [XmlElement("descEvento", Order = 0)]
        public string DescEvento { get; set; } = "Carta de Correcao";

        [XmlElement("infCorrecao", Order = 1)]
        public List<InfCorrecao> InfCorrecao { get; set; } = new List<InfCorrecao>();

        [XmlElement("xCondUso", Order = 2)]
        public string XCondUso
        {
            get => XCondUsoField;
            set => XCondUsoField = value;
        }

        #region Add List

        public void AddInfCorrecao(InfCorrecao infcorrecao)
        {
            if(InfCorrecao == null)
            {
                InfCorrecao = new List<InfCorrecao>();
            }

            InfCorrecao.Add(infcorrecao);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
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
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EvCECTe
    {
        [XmlElement("descEvento", Order = 0)]
        public string DescEvento { get; set; } = "Comprovante de Entrega";

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

        #region Add List

        public void AddInfEntrega(InfEntrega infentrega)
        {
            if(InfEntrega == null)
            {
                InfEntrega = new List<InfEntrega>();
            }

            InfEntrega.Add(infentrega);
        }

        #endregion 
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfEntrega
    {
        private string ChNFeField;

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
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EvCancCECTe
    {
        [XmlElement("descEvento", Order = 0)]
        public string DescEvento { get; set; } = "Cancelamento do Comprovante de Entrega do CT-e";

        [XmlElement("nProt", Order = 1)]
        public string NProt { get; set; }

        [XmlElement("nProtCE", Order = 2)]
        public string NProtCE { get; set; }
    }
}
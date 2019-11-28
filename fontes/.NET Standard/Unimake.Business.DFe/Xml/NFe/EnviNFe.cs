using System;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
    [Serializable()]
    [XmlRoot("enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class EnviNFe : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public string IdLote { get; set; }

        [XmlElement("indSinc")]
        public SimNao IndSinc { get; set; }

        [XmlElement("NFe")]
        public NFe[] NFe { get; set; }

        public override XmlDocument GerarXML()
        {
            var xmlDoc = base.GerarXML();

            var listEnvNFe = xmlDoc.GetElementsByTagName("enviNFe");
            foreach (var XmlNode in listEnvNFe)
            {
                var xmlElem = (XmlElement)XmlNode;
                if (xmlElem.GetElementsByTagName("NFe")[0] != null)
                {
                    var elemNFe = (XmlElement)xmlElem.GetElementsByTagName("NFe")[0];
                    var attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
                    elemNFe.SetAttribute("xmlns", attribute.Namespace);
                }
            }

            return xmlDoc;
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NFe
    {
        [XmlElement("infNFe")]
        public InfNFe[] InfNFe { get; set; }

        [XmlElement("infNFeSupl")]
        public InfNFeSupl InfNFeSupl { get; set; }

        [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Signature Signature { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfNFe
    {
        private string IdField;

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("ide")]
        public Ide Ide { get; set; }

        [XmlElement("emit")]
        public Emit Emit { get; set; }

        /// <summary>
        /// Esta tag é de uso exclusivo do FISCO, não precisa gerar nada, só temos ela para caso de alguma necessidade de deserialização.
        /// </summary>
        [XmlElement("avulsa")]
        public Avulsa Avulsa { get; set; }

        [XmlElement("dest")]
        public Dest Dest { get; set; }

        [XmlElement("retirada")]
        public Retirada Retirada { get; set; }

        [XmlElement("entrega")]
        public Entrega Entrega { get; set; }

        [XmlElement("autXML")]
        public AutXML[] AutXML { get; set; }

        [XmlElement("det")]
        public Det[] Det { get; set; }

        [XmlElement("total")]
        public Total Total { get; set; }

        [XmlElement("transp")]
        public Transp Transp { get; set; }

        [XmlElement("cobr")]
        public Cobr Cobr { get; set; }

        [XmlElement("pag")]
        public Pag Pag { get; set; }

        [XmlElement("infAdic")]
        public InfAdic InfAdic { get; set; }

        [XmlElement("exporta")]
        public Exporta Exporta { get; set; }

        [XmlElement("compra")]
        public Compra Compra { get; set; }

        [XmlElement("cana")]
        public Cana Cana { get; set; }

        [XmlElement("infRespTec")]
        public InfRespTec InfRespTec { get; set; }

        [XmlAttribute(AttributeName = "Id", DataType = "ID")]
        public string Id
        {
            get
            {
                IdField = "NFe" + Chave;
                return IdField;
            }
            set
            {
                IdField = value;
            }
        }

        private string ChaveField;

        [XmlIgnore]
        public string Chave
        {
            get
            {
                ChaveField = ((int)Ide.CUF).ToString() +
                    Ide.DhEmi.ToString("yyMM") +
                    (string.IsNullOrWhiteSpace(Emit.CNPJ) ? Emit.CPF?.PadLeft(14, '0') : Emit.CNPJ.PadLeft(14, '0')) +
                    ((int)Ide.Mod).ToString().PadLeft(2, '0') +
                    Ide.Serie.ToString().PadLeft(3, '0') +
                    Ide.NNF.ToString().PadLeft(9, '0') +
                    ((int)Ide.TpEmis).ToString() +
                    Ide.CNF.PadLeft(8, '0');

                Ide.CDV = Utility.XMLUtility.CalcularDVChave(ChaveField);

                ChaveField += Ide.CDV.ToString();

                return ChaveField;
            }
            set
            {
                throw new Exception("Não é permitido atribuir valor para a propriedade Chave. Ela é calculada automaticamente.");
            }
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Ide
    {
        private string CNFField;

        [XmlIgnore]
        public UFBrasil CUF { get; set; }

        [XmlElement("cUF")]
        public int CUFField
        {
            get => (int)CUF;
            set => CUF = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlElement("cNF")]
        public string CNF
        {
            get
            {
                var retorno = string.Empty;

                if (string.IsNullOrWhiteSpace(CNFField))
                {
                    if (NNF == 0)
                    {
                        throw new Exception("Defina antes o conteudo da TAG <nNF>, pois o mesmo é utilizado como base para calcular o código numérico.");
                    }

                    retorno = Utility.XMLUtility.GerarCodigoNumerico(NNF).ToString("00000000");
                }
                else
                {
                    retorno = CNFField;
                }

                return retorno;
            }
            set => CNFField = value;
        }

        [XmlElement("natOp")]
        public string NatOp { get; set; }

        [XmlElement("mod")]
        public ModeloDFe Mod { get; set; }

        [XmlElement("serie")]
        public int Serie { get; set; }

        [XmlElement("nNF")]
        public int NNF { get; set; }

        [XmlIgnore]
        public DateTime DhEmi { get; set; }

        [XmlElement("dhEmi")]
        public string DhEmiField
        {
            get => DhEmi.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhEmi = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime DhSaiEnt { get; set; }

        [XmlElement("dhSaiEnt")]
        public string DhSaiEntField
        {
            get => DhSaiEnt.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhSaiEnt = DateTime.Parse(value);
        }

        [XmlElement("tpNF")]
        public TipoOperacao TpNF { get; set; }

        [XmlElement("idDest")]
        public DestinoOperacao IdDest { get; set; }

        [XmlElement("cMunFG")]
        public int CMunFG { get; set; }

        [XmlElement("tpImp")]
        public FormatoImpressaoDANFE TpImp { get; set; }

        [XmlElement("tpEmis")]
        public TipoEmissao TpEmis { get; set; }

        [XmlElement("cDV")]
        public int CDV { get; set; }

        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("finNFe")]
        public FinalidadeNFe FinNFe { get; set; }

        [XmlElement("indFinal")]
        public SimNao IndFinal { get; set; }

        [XmlElement("indPres")]
        public IndicadorPresenca IndPres { get; set; }

        [XmlElement("procEmi")]
        public ProcessoEmissao ProcEmi { get; set; }

        [XmlElement("verProc")]
        public string VerProc { get; set; }

        [XmlIgnore]
        public DateTime DhCont { get; set; }

        [XmlElement("dhCont")]
        public string DhContField
        {
            get => DhCont.ToString("yyyy-MM-ddTHH:mm:ssK");
            set => DhCont = DateTime.Parse(value);
        }

        [XmlElement("xJust")]
        public string XJust { get; set; }

        [XmlElement("NFref")]
        public NFref[] NFref { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeDhContField()
        {
            return DhCont > DateTime.MinValue;
        }

        public bool ShouldSerializeXJust()
        {
            return !string.IsNullOrWhiteSpace(XJust);
        }

        public bool ShouldSerializeDhSaiEntField()
        {
            return DhSaiEnt > DateTime.MinValue;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NFref
    {
        [XmlElement("refCTe")]
        public string RefCTe { get; set; }

        [XmlElement("refECF")]
        public string RefECF { get; set; }

        [XmlElement("refNF")]
        public string RefNF { get; set; }

        [XmlElement("refNFP")]
        public string RefNFP { get; set; }

        [XmlElement("refNFe")]
        public string RefNFe { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeRefCTe()
        {
            return !string.IsNullOrWhiteSpace(RefCTe);
        }

        public bool ShouldSerializeRefECF()
        {
            return !string.IsNullOrWhiteSpace(RefECF);
        }

        public bool ShouldSerializeRefNF()
        {
            return !string.IsNullOrWhiteSpace(RefNF);
        }

        public bool ShouldSerializeRefNFP()
        {
            return !string.IsNullOrWhiteSpace(RefNFP);
        }

        public bool ShouldSerializeRefNFe()
        {
            return !string.IsNullOrWhiteSpace(RefNFe);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Emit
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("xFant")]
        public string XFant { get; set; }

        [XmlElement("enderEmit")]
        public EnderEmit EnderEmit { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("IEST")]
        public string IEST { get; set; }

        [XmlElement("IM")]
        public string IM { get; set; }

        [XmlElement("CNAE")]
        public string CNAE { get; set; }

        [XmlElement("CRT")]
        public CRT CRT { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }

        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrWhiteSpace(CPF);
        }

        public bool ShouldSerializeIEST()
        {
            return !string.IsNullOrWhiteSpace(IEST);
        }

        public bool ShouldSerializeCNAE()
        {
            return !string.IsNullOrWhiteSpace(CNAE);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class EnderEmit
    {
        [XmlElement("xLgr")]
        public string XLgr { get; set; }

        [XmlElement("nro")]
        public string Nro { get; set; }

        [XmlElement("xCpl")]
        public string XCpl { get; set; }

        [XmlElement("xBairro")]
        public string XBairro { get; set; }

        [XmlElement("cMun")]
        public int CMun { get; set; }

        [XmlElement("xMun")]
        public string XMun { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        [XmlElement("fone")]
        public string Fone { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCPais()
        {
            return CPais > 0;
        }

        public bool ShouldSerializeXPais()
        {
            return !string.IsNullOrWhiteSpace(XPais);
        }

        public bool ShouldSerializeXCpl()
        {
            return !string.IsNullOrWhiteSpace(XCpl);
        }

        public bool ShouldSerializeFone()
        {
            return !string.IsNullOrWhiteSpace(Fone);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Avulsa
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("xOrgao")]
        public string XOrgao { get; set; }

        [XmlElement("matr")]
        public string Matr { get; set; }

        [XmlElement("xAgente")]
        public string XAgente { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("nDAR")]
        public string NDAR { get; set; }

        [XmlIgnore]
        public DateTime DEmi { get; set; }

        [XmlElement("dEmi")]
        public string DEmiField
        {
            get => DEmi.ToString("yyyy-MM-dd");
            set => DEmi = DateTime.Parse(value);
        }

        [XmlElement("vDAR")]
        public string VDAR { get; set; }

        [XmlElement("repEmi")]
        public string RepEmi { get; set; }

        [XmlIgnore]
        public DateTime DPag { get; set; }

        [XmlElement("dPag")]
        public string DPagField
        {
            get => DPag.ToString("yyyy-MM-dd");
            set => DPag = DateTime.Parse(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Dest
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlIgnore]
        public string CpfCnpj
        {
            set
            {
                if (value.Length <= 11)
                {
                    CPF = value;
                }
                else
                {
                    CNPJ = value;
                }
            }
        }

        [XmlElement("idEstrangeiro")]
        public string IdEstrangeiro { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("enderDest")]
        public EnderDest EnderDest { get; set; }

        [XmlElement("indIEDest")]
        public IndicadorIEDestinatario IndIEDest { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("ISUF")]
        public string ISUF { get; set; }

        [XmlElement("IM")]
        public string IM { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }

        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrWhiteSpace(CPF);
        }

        public bool ShouldSerializeIdEstrangeiro()
        {
            return !string.IsNullOrWhiteSpace(IdEstrangeiro);
        }

        public bool ShouldSerializeXNome()
        {
            return !string.IsNullOrWhiteSpace(XNome);
        }

        public bool ShouldSerializeIE()
        {
            return !string.IsNullOrWhiteSpace(IE);
        }

        public bool ShouldSerializeISUF()
        {
            return !string.IsNullOrWhiteSpace(ISUF);
        }

        public bool ShouldSerializeIM()
        {
            return !string.IsNullOrWhiteSpace(IM);
        }

        public bool ShouldSerializeEmail()
        {
            return !string.IsNullOrWhiteSpace(Email);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class EnderDest
    {
        [XmlElement("xLgr")]
        public string XLgr { get; set; }

        [XmlElement("nro")]
        public string Nro { get; set; }

        [XmlElement("xCpl")]
        public string XCpl { get; set; }

        [XmlElement("xBairro")]
        public string XBairro { get; set; }

        [XmlElement("cMun")]
        public int CMun { get; set; }

        [XmlElement("xMun")]
        public string XMun { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        [XmlElement("fone")]
        public string Fone { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeXCpl()
        {
            return !string.IsNullOrWhiteSpace(XCpl);
        }

        public bool ShouldSerializeCEP()
        {
            return !string.IsNullOrWhiteSpace(CEP);
        }

        public bool ShouldSerializeCPais()
        {
            return CPais > 0;
        }

        public bool ShouldSerializeXPais()
        {
            return !string.IsNullOrWhiteSpace(XPais);
        }

        public bool ShouldSerializeFone()
        {
            return !string.IsNullOrWhiteSpace(Fone);
        }

        #endregion
    }

    public abstract class LocalBase
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("xLgr")]
        public string XLgr { get; set; }

        [XmlElement("nro")]
        public string Nro { get; set; }

        [XmlElement("xCpl")]
        public string XCpl { get; set; }

        [XmlElement("xBairro")]
        public string XBairro { get; set; }

        [XmlElement("cMun")]
        public int CMun { get; set; }

        [XmlElement("xMun")]
        public string XMun { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeXCpl()
        {
            return !string.IsNullOrWhiteSpace(XCpl);
        }

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }

        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrWhiteSpace(CPF);
        }

        public bool ShouldSerializeXNome()
        {
            return !string.IsNullOrWhiteSpace(XNome);
        }

        public bool ShouldSerializeCEP()
        {
            return !string.IsNullOrWhiteSpace(CEP);
        }

        public bool ShouldSerializeCPais()
        {
            return CPais > 0;
        }

        public bool ShouldSerializeXPais()
        {
            return !string.IsNullOrWhiteSpace(XPais);
        }

        public bool ShouldSerializeFone()
        {
            return !string.IsNullOrWhiteSpace(Fone);
        }

        public bool ShouldSerializeIE()
        {
            return !string.IsNullOrWhiteSpace(IE);
        }

        public bool ShouldSerializeEmail()
        {
            return !string.IsNullOrWhiteSpace(Email);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Retirada : LocalBase { }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Entrega : LocalBase { }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class AutXML
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }

        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrWhiteSpace(CPF);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Det
    {
        #region Private Fields

        private string _infAdProd;

        #endregion

        [XmlAttribute(AttributeName = "nItem")]
        public int NItem { get; set; }

        [XmlElement("prod")]
        public Prod Prod { get; set; }

        [XmlElement("imposto")]
        public Imposto Imposto { get; set; }

        [XmlElement("impostoDevol")]
        public ImpostoDevol ImpostoDevol { get; set; }

        [XmlElement("infAdProd")]
        public string InfAdProd { get => string.IsNullOrWhiteSpace(_infAdProd) ? null : _infAdProd; set => _infAdProd = value; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Prod
    {
        [XmlElement("cProd")]
        public string CProd { get; set; }

        [XmlElement("cEAN")]
        public string CEAN { get; set; } = "";

        [XmlElement("xProd")]
        public string XProd { get; set; }

        [XmlElement("NCM")]
        public string NCM { get; set; }

        [XmlElement("NVE")]
        public string[] NVE { get; set; }

        [XmlElement("CEST")]
        public string CEST { get; set; }

        [XmlElement("indEscala")]
        public IndicadorEscalaRelevante? IndEscala { get; set; }

        [XmlElement("CNPJFab")]
        public string CNPJFab { get; set; }

        [XmlElement("cBenef")]
        public string CBenef { get; set; }

        [XmlElement("EXTIPI")]
        public string EXTIPI { get; set; }

        [XmlElement("CFOP")]
        public string CFOP { get; set; }

        [XmlElement("uCom")]
        public string UCom { get; set; }

        [XmlElement("qCom")]
        public double QCom { get; set; }

        [XmlElement("vUnCom")]
        public double VUnCom { get; set; }

        [XmlIgnore]
        public double VProd { get; set; }
        [XmlElement("vProd")]

        public string VProdField
        {
            get => VProd.ToString("F2", CultureInfo.InvariantCulture);
            set => VProd = Utility.Converter.ToDouble(value);
        }

        [XmlElement("cEANTrib")]
        public string CEANTrib { get; set; } = "";

        [XmlElement("uTrib")]
        public string UTrib { get; set; }

        [XmlElement("qTrib")]
        public double QTrib { get; set; }

        [XmlElement("vUnTrib")]
        public double VUnTrib { get; set; }

        [XmlIgnore]
        public double VFrete { get; set; }

        [XmlElement("vFrete")]
        public string VFreteField
        {
            get => VFrete.ToString("F2", CultureInfo.InvariantCulture);
            set => VFrete = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VSeg { get; set; }

        [XmlElement("vSeg")]
        public string VSegField
        {
            get => VSeg.ToString("F2", CultureInfo.InvariantCulture);
            set => VSeg = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDesc { get; set; }

        [XmlElement("vDesc")]
        public string VDescField
        {
            get => VDesc.ToString("F2", CultureInfo.InvariantCulture);
            set => VDesc = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VOutro { get; set; }

        [XmlElement("vOutro")]
        public string VOutroField
        {
            get => VOutro.ToString("F2", CultureInfo.InvariantCulture);
            set => VOutro = Utility.Converter.ToDouble(value);
        }

        [XmlElement("indTot")]
        public SimNao IndTot { get; set; }

        [XmlElement("DI")]
        public DI[] DI { get; set; }

        [XmlElement("detExport")]
        public DetExport[] DetExport { get; set; }

        [XmlElement("xPed")]
        public string XPed { get; set; }

        [XmlElement("nItemPed")]
        public int NItemPed { get; set; }

        [XmlElement("nFCI")]
        public string NFCI { get; set; }

        /// <remarks/>
        [XmlElement("rastro")]
        public Rastro[] Rastro { get; set; }

        [XmlElement("arma")]
        public Arma[] Arma { get; set; }

        [XmlElement("comb")]
        public Comb[] Comb { get; set; }

        [XmlElement("med")]
        public Med[] Med { get; set; }

        [XmlElement("nRECOPI")]
        public string NRECOPI { get; set; }

        [XmlElement("veicProd")]
        public VeicProd[] VeicProd { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeNVE()
        {
            return NVE != null;
        }

        public bool ShouldSerializeCEST()
        {
            return !string.IsNullOrWhiteSpace(CEST);
        }

        public bool ShouldSerializeCNPJFab()
        {
            return !string.IsNullOrWhiteSpace(CNPJFab);
        }

        public bool ShouldSerializeCBenef()
        {
            return !string.IsNullOrWhiteSpace(CBenef);
        }

        public bool ShouldSerializeEXTIPI()
        {
            return !string.IsNullOrWhiteSpace(EXTIPI);
        }

        public bool ShouldSerializeVFreteField()
        {
            return VFrete > 0;
        }

        public bool ShouldSerializeVSegField()
        {
            return VSeg > 0;
        }

        public bool ShouldSerializeVDescField()
        {
            return VDesc > 0;
        }

        public bool ShouldSerializeVOutroField()
        {
            return VOutro > 0;
        }

        public bool ShouldSerializeXPed()
        {
            return !string.IsNullOrWhiteSpace(XPed);
        }

        public bool ShouldSerializeNItemPed()
        {
            return NItemPed > 0;
        }

        public bool ShouldSerializeNFCI()
        {
            return !string.IsNullOrWhiteSpace(NFCI);
        }

        public bool ShouldSerializeIndEscala()
        {
            return IndEscala != null;
        }

        public bool ShouldSerializeNRECOPI()
        {
            return !string.IsNullOrWhiteSpace(NRECOPI);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DI
    {
        [XmlElement("nDI")]
        public ulong NDI { get; set; }

        [XmlIgnore]
        public DateTime DDI { get; set; }

        [XmlElement("dDI")]
        public string DDIField
        {
            get => DDI.ToString("yyyy-MM-dd");
            set => DDI = DateTime.Parse(value);
        }

        [XmlElement("xLocDesemb")]
        public string XLocDesemb { get; set; }

        [XmlElement("UFDesemb")]
        public UFBrasil UFDesemb { get; set; }

        [XmlIgnore]
        public DateTime DDesemb { get; set; }

        [XmlElement("dDesemb")]
        public string DDesembField
        {
            get => DDesemb.ToString("yyyy-MM-dd");
            set => DDesemb = DateTime.Parse(value);
        }

        [XmlElement("tpViaTransp")]
        public ViaTransporteInternacional TpViaTransp { get; set; }

        [XmlIgnore]
        public double VAFRMM { get; set; }

        [XmlElement("vAFRMM")]
        public string VAFRMMField
        {
            get => VAFRMM.ToString("F2", CultureInfo.InvariantCulture);
            set => VAFRMM = Utility.Converter.ToDouble(value);
        }

        [XmlElement("tpIntermedio")]
        public FormaImportacaoIntermediacao TpIntermedio { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("UFTerceiro")]
        public UFBrasil UFTerceiro { get; set; }

        [XmlElement("cExportador")]
        public string CExportador { get; set; }

        [XmlElement("adi")]
        public Adi[] Adi { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVAFRMM()
        {
            return VAFRMM > 0;
        }

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }

        public bool ShouldSerializeUFTerceiro()
        {
            return Enum.IsDefined(typeof(UFBrasil), UFTerceiro);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Adi
    {
        [XmlElement("nAdicao")]
        public int NAdicao { get; set; }

        [XmlElement("nSeqAdic")]
        public int NSeqAdic { get; set; }

        [XmlElement("cFabricante")]
        public string CFabricante { get; set; }

        [XmlIgnore]
        public double VDescDI { get; set; }

        [XmlElement("vDescDI")]
        public string VDescDIField
        {
            get => VDescDI.ToString("F2", CultureInfo.InvariantCulture);
            set => VDescDI = Utility.Converter.ToDouble(value);
        }

        [XmlElement("nDraw")]
        public ulong NDraw { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeNDraw()
        {
            return NDraw > 0;
        }

        public bool ShouldSerializenVDescDIField()
        {
            return VDescDI > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DetExport
    {
        [XmlElement("nDraw")]
        public ulong NDraw { get; set; }

        [XmlElement("exportInd")]
        public ExportInd ExportInd { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeNDraw()
        {
            return NDraw > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ExportInd
    {
        [XmlElement("nRE")]
        public ulong NRE { get; set; }

        [XmlElement("chNFe")]
        public string ChNFe { get; set; }

        [XmlIgnore]
        public double QExport { get; set; }

        [XmlElement("qExport")]
        public string QExportField
        {
            get => QExport.ToString("F4", CultureInfo.InvariantCulture);
            set => QExport = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Rastro
    {
        [XmlElement("nLote")]
        public string NLote { get; set; }

        [XmlIgnore]
        public double QLote { get; set; }

        [XmlElement("qLote")]
        public string QLoteField
        {
            get => QLote.ToString("F2", CultureInfo.InvariantCulture);
            set => QLote = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public DateTime DFab { get; set; }

        [XmlElement("dFab")]
        public string DFabField
        {
            get => DFab.ToString("yyyy-MM-dd");
            set => DFab = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime DVal { get; set; }

        [XmlElement("dVal")]
        public string DValField
        {
            get => DVal.ToString("yyyy-MM-dd");
            set => DVal = DateTime.Parse(value);
        }

        [XmlElement("cAgreg")]
        public string CAgreg { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCAgreg()
        {
            return !string.IsNullOrWhiteSpace(CAgreg);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public partial class Arma
    {
        [XmlElement("tpArma")]
        public TipoArma TpArma { get; set; }

        [XmlElement("nSerie")]
        public string NSerie { get; set; }

        [XmlElement("nCano")]
        public string NCano { get; set; }

        [XmlElement("descr")]
        public string Descr { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Comb
    {
        [XmlElement("cProdANP")]
        public string CProdANP { get; set; }

        [XmlElement("descANP")]
        public string DescANP { get; set; }

        [XmlIgnore]
        public double PGLP { get; set; }

        [XmlElement("pGLP")]
        public string PGLPField
        {
            get => PGLP.ToString("F4", CultureInfo.InvariantCulture);
            set => PGLP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PGNn { get; set; }

        [XmlElement("pGNn")]
        public string PGNnField
        {
            get => PGNn.ToString("F4", CultureInfo.InvariantCulture);
            set => PGNn = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PGNi { get; set; }

        [XmlElement("pGNi")]
        public string PGNiField
        {
            get => PGNi.ToString("F4", CultureInfo.InvariantCulture);
            set => PGNi = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VPart { get; set; }

        [XmlElement("vPart")]
        public string VPartField
        {
            get => VPart.ToString("F4", CultureInfo.InvariantCulture);
            set => VPart = Utility.Converter.ToDouble(value);
        }

        [XmlElement("CODIF")]
        public string CODIF { get; set; }

        [XmlIgnore]
        public double QTemp { get; set; }

        [XmlElement("qTemp")]
        public string QTempField
        {
            get => QTemp.ToString("F2", CultureInfo.InvariantCulture);
            set => QTemp = Utility.Converter.ToDouble(value);
        }

        [XmlElement("UFCons")]
        public UFBrasil UFCons { get; set; }

        [XmlElement("CIDE")]
        public CIDE CIDE { get; set; }

        [XmlElement("encerrante")]
        public Encerrante Encerrante { get; set; }


        #region ShouldSerialize

        public bool ShouldSerializePGLPField()
        {
            return PGLP > 0;
        }

        public bool ShouldSerializePGNnField()
        {
            return PGNn > 0;
        }

        public bool ShouldSerializePGNiField()
        {
            return PGNi > 0;
        }

        public bool ShouldSerializeVPartField()
        {
            return VPart > 0;
        }

        public bool ShouldSerializeCODIF()
        {
            return !string.IsNullOrWhiteSpace(CODIF);
        }

        public bool ShouldSerializeQTempField()
        {
            return QTemp > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class CIDE
    {
        [XmlElement("qBCProd")]
        public double QBCProd { get; set; }

        [XmlIgnore]
        public double VAliqProd { get; set; }

        [XmlElement("vAliqProd")]
        public string VAliqProdField
        {
            get => VAliqProd.ToString("F4", CultureInfo.InvariantCulture);
            set => VAliqProd = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCIDE { get; set; }

        [XmlElement("vCIDE")]
        public string VCIDEField
        {
            get => VCIDE.ToString("F2", CultureInfo.InvariantCulture);
            set => VCIDE = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Encerrante
    {
        [XmlElement("nBico")]
        public int NBico { get; set; }

        [XmlElement("nBomba")]
        public int NBomba { get; set; }

        [XmlElement("nTanque")]
        public int NTanque { get; set; }

        [XmlIgnore]
        public double VEncIni { get; set; }

        [XmlElement("vEncIni")]
        public string VEncIniField
        {
            get => VEncIni.ToString("F3", CultureInfo.InvariantCulture);
            set => VEncIni = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VEncFin { get; set; }

        [XmlElement("vEncFin")]
        public string VEncFinField
        {
            get => VEncFin.ToString("F3", CultureInfo.InvariantCulture);
            set => VEncFin = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeNBomba()
        {
            return NBomba > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Med
    {
        [XmlElement("cProdANVISA")]
        public string CProdANVISA { get; set; }

        [XmlElement("xMotivoIsencao")]
        public string XMotivoIsencao { get; set; }

        [XmlIgnore]
        public double VPMC { get; set; }

        [XmlElement("vPMC")]
        public string VPMCField
        {
            get => VPMC.ToString("F2", CultureInfo.InvariantCulture);
            set => VPMC = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public partial class VeicProd
    {
        [XmlElement("tpOp")]
        public TipoOperacaoVeicNovo TpOp { get; set; }

        [XmlElement("chassi")]
        public string Chassi { get; set; }

        [XmlElement("cCor")]
        public string CCor { get; set; }

        [XmlElement("xCor")]
        public string XCor { get; set; }

        [XmlElement("pot")]
        public string Pot { get; set; }

        [XmlElement("cilin")]
        public string Cilin { get; set; }

        [XmlElement("pesoL")]
        public string PesoL { get; set; }

        [XmlElement("pesoB")]
        public string PesoB { get; set; }

        [XmlElement("nSerie")]
        public string NSerie { get; set; }

        [XmlElement("tpComb")]
        public string TpComb { get; set; }

        [XmlElement("nMotor")]
        public string NMotor { get; set; }

        [XmlElement("CMT")]
        public string CMT { get; set; }

        [XmlElement("dist")]
        public string Dist { get; set; }

        [XmlElement("anoMod")]
        public int AnoMod { get; set; }

        [XmlElement("anoFab")]
        public int AnoFab { get; set; }

        [XmlElement("tpPint")]
        public string TpPint { get; set; }

        [XmlElement("tpVeic")]
        public string TpVeic { get; set; }

        [XmlElement("espVeic")]
        public string EspVeic { get; set; }

        [XmlElement("VIN")]
        public CondicaoVIN VIN { get; set; }

        [XmlElement("condVeic")]
        public CondicaoVeiculo CondVeic { get; set; }

        [XmlElement("cMod")]
        public int CMod { get; set; }

        [XmlElement("cCorDENATRAN")]
        public string CCorDENATRAN { get; set; }

        [XmlElement("lota")]
        public int Lota { get; set; }

        [XmlElement("tpRest")]
        public TipoRestricaoVeiculo TpRest { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Imposto
    {
        [XmlIgnore]
        public double VTotTrib { get; set; }

        [XmlElement("vTotTrib")]
        public string VTotTribField
        {
            get => VTotTrib.ToString("F2", CultureInfo.InvariantCulture);
            set => VTotTrib = Utility.Converter.ToDouble(value);
        }

        [XmlElement("ICMS")]
        public ICMS[] ICMS { get; set; }

        [XmlElement("II")]
        public II[] II { get; set; }

        [XmlElement("IPI")]
        public IPI IPI { get; set; }

        [XmlElement("ISSQN")]
        public ISSQN[] ISSQN { get; set; }

        [XmlElement("PIS")]
        public PIS PIS { get; set; }

        [XmlElement("PISST")]
        public PISST PISST { get; set; }

        [XmlElement("COFINS")]
        public COFINS COFINS { get; set; }

        [XmlElement("COFINSST")]
        public COFINSST COFINSST { get; set; }

        [XmlElement("ICMSUFDest")]
        public ICMSUFDest ICMSUFDest { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS
    {
        [XmlElement("ICMS00")]
        public ICMS00 ICMS00 { get; set; }

        [XmlElement("ICMS10")]
        public ICMS10 ICMS10 { get; set; }

        [XmlElement("ICMS20")]
        public ICMS20 ICMS20 { get; set; }

        [XmlElement("ICMS30")]
        public ICMS30 ICMS30 { get; set; }

        [XmlElement("ICMS40")]
        public ICMS40 ICMS40 { get; set; }

        [XmlElement("ICMS51")]
        public ICMS51 ICMS51 { get; set; }

        [XmlElement("ICMS60")]
        public ICMS60 ICMS60 { get; set; }

        [XmlElement("ICMS70")]
        public ICMS70 ICMS70 { get; set; }

        [XmlElement("ICMS90")]
        public ICMS90 ICMS90 { get; set; }

        [XmlElement("ICMSPart")]
        public ICMSPart ICMSPart { get; set; }

        [XmlElement("ICMSSN101")]
        public ICMSSN101 ICMSSN101 { get; set; }

        [XmlElement("ICMSSN102")]
        public ICMSSN102 ICMSSN102 { get; set; }

        [XmlElement("ICMSSN201")]
        public ICMSSN201 ICMSSN201 { get; set; }

        [XmlElement("ICMSSN202")]
        public ICMSSN202 ICMSSN202 { get; set; }

        [XmlElement("ICMSSN500")]
        public ICMSSN500 ICMSSN500 { get; set; }

        [XmlElement("ICMSSN900")]
        public ICMSSN900 ICMSSN900 { get; set; }

        [XmlElement("ICMSST")]
        public ICMSST ICMSST { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS00
    {
        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST { get; set; } = "00";

        [XmlElement("modBC")]
        public ModalidadeBaseCalculoICMS ModBC { get; set; }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMS { get; set; }

        [XmlElement("pICMS")]
        public string PICMSField
        {
            get => PICMS.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCP { get; set; }

        [XmlElement("pFCP")]
        public string PFCPField
        {
            get => PFCP.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCP { get; set; }

        [XmlElement("vFCP")]
        public string VFCPField
        {
            get => VFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCP = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializePFCPField()
        {
            return PFCP > 0;
        }

        public bool ShouldSerializeVFCPField()
        {
            return VFCP > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS10
    {
        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST { get; set; } = "10";

        [XmlElement("modBC")]
        public ModalidadeBaseCalculoICMS ModBC { get; set; }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMS { get; set; }

        [XmlElement("pICMS")]
        public string PICMSField
        {
            get => PICMS.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCP { get; set; }

        [XmlElement("vBCFCP")]
        public string VBCFCPField
        {
            get => VBCFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCP { get; set; }

        [XmlElement("pFCP")]
        public string PFCPField
        {
            get => PFCP.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCP { get; set; }

        [XmlElement("vFCP")]
        public string VFCPField
        {
            get => VFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCP = Utility.Converter.ToDouble(value);
        }

        [XmlElement("modBCST")]
        public ModalidadeBaseCalculoICMSST ModBCST { get; set; }

        [XmlIgnore]
        public double PMVAST { get; set; }

        [XmlElement("pMVAST")]
        public string PMVASTField
        {
            get => PMVAST.ToString("F4", CultureInfo.InvariantCulture);
            set => PMVAST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PRedBCSTField
        {
            get => PRedBCST.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }

        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string PICMSSTField
        {
            get => PICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPST { get; set; }

        [XmlElement("vBCFCPST")]
        public string VBCFCPSTField
        {
            get => VBCFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPST { get; set; }

        [XmlElement("pFCPST")]
        public string PFCPSTField
        {
            get => PFCPST.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPST { get; set; }

        [XmlElement("vFCPST")]
        public string VFCPSTField
        {
            get => VFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPST = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVBCFCPField()
        {
            return VBCFCP > 0;
        }

        public bool ShouldSerializePFCPField()
        {
            return PFCP > 0;
        }

        public bool ShouldSerializeVFCPField()
        {
            return VFCP > 0;
        }

        public bool ShouldSerializePMVASTField()
        {
            return PMVAST > 0;
        }

        public bool ShouldSerializePRedBCSTField()
        {
            return PRedBCST > 0;
        }

        public bool ShouldSerializeVBCFCPSTField()
        {
            return VBCFCPST > 0;
        }

        public bool ShouldSerializePFCPSTField()
        {
            return PFCPST > 0;
        }

        public bool ShouldSerializeVFCPSTField()
        {
            return VFCPST > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS20
    {
        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST { get; set; } = "20";

        [XmlElement("modBC")]
        public ModalidadeBaseCalculoICMS ModBC { get; set; }

        [XmlIgnore]
        public double PRedBC { get; set; }

        [XmlElement("pRedBC")]
        public string PRedBCField
        {
            get => PRedBC.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMS { get; set; }

        [XmlElement("pICMS")]
        public string PICMSField
        {
            get => PICMS.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCP { get; set; }

        [XmlElement("vBCFCP")]
        public string VBCFCPField
        {
            get => VBCFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCP { get; set; }

        [XmlElement("pFCP")]
        public string PFCPField
        {
            get => PFCP.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCP { get; set; }

        [XmlElement("vFCP")]
        public string VFCPField
        {
            get => VFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSDeson { get; set; }

        [XmlElement("vICMSDeson")]
        public string VICMSDesonField
        {
            get => VICMSDeson.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSDeson = Utility.Converter.ToDouble(value);
        }

        [XmlElement("motDesICMS")]
        public MotivoDesoneracaoICMS MotDesICMS { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVBCFCPField()
        {
            return VBCFCP > 0;
        }

        public bool ShouldSerializePFCPField()
        {
            return PFCP > 0;
        }

        public bool ShouldSerializeVFCPField()
        {
            return VFCP > 0;
        }

        public bool ShouldSerializeVICMSDesonField()
        {
            return VICMSDeson > 0;
        }

        public bool ShouldSerializeMotDesICMS()
        {
            return VICMSDeson > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS30
    {
        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST { get; set; } = "30";

        [XmlElement("modBCST")]
        public ModalidadeBaseCalculoICMSST ModBCST { get; set; }

        [XmlIgnore]
        public double PMVAST { get; set; }

        [XmlElement("pMVAST")]
        public string PMVASTField
        {
            get => PMVAST.ToString("F4", CultureInfo.InvariantCulture);
            set => PMVAST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PRedBCSTField
        {
            get => PRedBCST.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }

        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string PICMSSTField
        {
            get => PICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => VICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPST { get; set; }

        [XmlElement("vBCFCPST")]
        public string VBCFCPSTField
        {
            get => VBCFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPST { get; set; }

        [XmlElement("pFCPST")]
        public string PFCPSTField
        {
            get => PFCPST.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPST { get; set; }

        [XmlElement("vFCPST")]
        public string VFCPSTField
        {
            get => VFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSDeson { get; set; }

        [XmlElement("vICMSDeson")]
        public string VICMSDesonField
        {
            get => VICMSDeson.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSDeson = Utility.Converter.ToDouble(value);
        }

        [XmlElement("motDesICMS")]
        public MotivoDesoneracaoICMS MotDesICMS { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializePMVASTField()
        {
            return PMVAST > 0;
        }

        public bool ShouldSerializePRedBCSTField()
        {
            return PRedBCST > 0;
        }

        public bool ShouldSerializeVBCFCPSTField()
        {
            return VBCFCPST > 0;
        }

        public bool ShouldSerializePFCPSTField()
        {
            return PFCPST > 0;
        }

        public bool ShouldSerializeVFCPSTField()
        {
            return VFCPST > 0;
        }

        public bool ShouldSerializeVICMSDesonField()
        {
            return VICMSDeson > 0;
        }

        public bool ShouldSerializeMotDesICMS()
        {
            return VICMSDeson > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS40
    {
        private string CSTField;

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("40") || value.Equals("41") || value.Equals("50"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <ICMS40> inválido! Valores aceitos: 40, 41 ou 50.");
                }
            }
        }

        [XmlIgnore]
        public double VICMSDeson { get; set; }

        [XmlElement("vICMSDeson")]
        public string VICMSDesonField
        {
            get => VICMSDeson.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSDeson = Utility.Converter.ToDouble(value);
        }

        [XmlElement("motDesICMS")]
        public MotivoDesoneracaoICMS? MotDesICMS { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVICMSDesonField()
        {
            return VICMSDeson > 0;
        }

        public bool ShouldSerializeMotDesICMS()
        {
            return VICMSDeson > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS51
    {
        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST { get; set; } = "51";

        [XmlElement("modBC")]
        public ModalidadeBaseCalculoICMS ModBC { get; set; }

        [XmlIgnore]
        public double PRedBC { get; set; }

        [XmlElement("pRedBC")]
        public string PRedBCField
        {
            get => PRedBC.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMS { get; set; }

        [XmlElement("pICMS")]
        public string PICMSField
        {
            get => PICMS.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSOp { get; set; }

        [XmlElement("vICMSOp")]
        public string VICMSOpField
        {
            get => VICMSOp.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSOp = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PDif { get; set; }

        [XmlElement("pDif")]
        public string PDifField
        {
            get => PDif.ToString("F4", CultureInfo.InvariantCulture);
            set => PDif = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSDif { get; set; }

        [XmlElement("vICMSDif")]
        public string VICMSDifField
        {
            get => VICMSDif.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSDif = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCP { get; set; }

        [XmlElement("vBCFCP")]
        public string VBCFCPField
        {
            get => VBCFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCP { get; set; }

        [XmlElement("pFCP")]
        public string PFCPField
        {
            get => PFCP.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCP { get; set; }

        [XmlElement("vFCP")]
        public string VFCPField
        {
            get => VFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCP = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVBCFCPField()
        {
            return VBCFCP > 0;
        }

        public bool ShouldSerializePFCPField()
        {
            return PFCP > 0;
        }

        public bool ShouldSerializeVFCPField()
        {
            return VFCP > 0;
        }

        public bool ShouldSerializePRedBCField()
        {
            return PRedBC > 0;
        }

        public bool ShouldSerializeVBCField()
        {
            return VBC > 0;
        }

        public bool ShouldSerializPICMSeField()
        {
            return PICMS > 0;
        }

        public bool ShouldSerializeVICMSOpField()
        {
            return VICMSOp > 0;
        }

        public bool ShouldSerializePDifField()
        {
            return PDif > 0;
        }

        public bool ShouldSerializeVICMSDifField()
        {
            return VICMSDif > 0;
        }

        public bool ShouldSerializeVICMSField()
        {
            return VICMS > 0;
        }

        public bool ShouldSerializeModBCField()
        {
            return Enum.IsDefined(typeof(ModalidadeBaseCalculoICMS), ModBC);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS60
    {
        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST { get; set; } = "60";

        [XmlIgnore]
        public double VBCSTRet { get; set; }

        [XmlElement("vBCSTRet")]
        public string VBCSTRetField
        {
            get => VBCSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PST { get; set; }

        [XmlElement("pST")]
        public string PSTField
        {
            get => PST.ToString("F4", CultureInfo.InvariantCulture);
            set => PST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSSTRet { get; set; }

        [XmlElement("vICMSSTRet")]
        public string VICMSSTRetField
        {
            get => VICMSSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPSTRet { get; set; }

        [XmlElement("vBCFCPSTRet")]
        public string VBCFCPSTRetField
        {
            get => VBCFCPSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPSTRet { get; set; }

        [XmlElement("pFCPSTRet")]
        public string PFCPSTRetField
        {
            get => PFCPSTRet.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPSTRet { get; set; }

        [XmlElement("vFCPSTRet")]
        public string VFCPSTRetField
        {
            get => VFCPSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCEfet { get; set; }

        [XmlElement("pRedBCEfet")]
        public string PRedBCEfetField
        {
            get => PRedBCEfet.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCEfet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCEfet { get; set; }

        [XmlElement("vBCEfet")]
        public string VBCEfetField
        {
            get => VBCEfet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCEfet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSEfet { get; set; }

        [XmlElement("pICMSEfet")]
        public string PICMSEfetField
        {
            get => PICMSEfet.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSEfet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSEfet { get; set; }

        [XmlElement("vICMSEfet")]
        public string VICMSEfetField
        {
            get => VICMSEfet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSEfet = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVBCFCPSTRetField()
        {
            return VBCFCPSTRet > 0;
        }

        public bool ShouldSerializePFCPSTRetField()
        {
            return PFCPSTRet > 0;
        }

        public bool ShouldSerializeVFCPSTRetField()
        {
            return VFCPSTRet > 0;
        }

        public bool ShouldSerializeVBCSTRetField()
        {
            return VBCSTRet > 0;
        }

        public bool ShouldSerializePSTField()
        {
            return PST > 0;
        }

        public bool ShouldSerializeVICMSSTRetField()
        {
            return VICMSSTRet > 0;
        }

        public bool ShouldSerializePRedBCEfetField()
        {
            return PRedBCEfet > 0;
        }

        public bool ShouldSerializeVBCEfetField()
        {
            return VBCEfet > 0;
        }

        public bool ShouldSerializePICMSEfetField()
        {
            return PICMSEfet > 0;
        }

        public bool ShouldSerializeVICMSEfetField()
        {
            return VICMSEfet > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS70
    {
        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public virtual string CST { get; set; } = "70";

        [XmlElement("modBC")]
        public ModalidadeBaseCalculoICMS ModBC { get; set; }

        [XmlIgnore]
        public double PRedBC { get; set; }

        [XmlElement("pRedBC")]
        public string PRedBCField
        {
            get => PRedBC.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMS { get; set; }

        [XmlElement("pICMS")]
        public string PICMSField
        {
            get => PICMS.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCP { get; set; }

        [XmlElement("vBCFCP")]
        public string VBCFCPField
        {
            get => VBCFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCP { get; set; }

        [XmlElement("pFCP")]
        public string PFCPField
        {
            get => PFCP.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCP { get; set; }

        [XmlElement("vFCP")]
        public string VFCPField
        {
            get => VFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public ModalidadeBaseCalculoICMSST ModBCSTField { get; set; }

        [XmlElement("modBCST")]
        public ModalidadeBaseCalculoICMSST ModBCST
        {
            get => ModBCSTField;

            set
            {
                if (value != ModalidadeBaseCalculoICMSST.ValorOperacao)
                {
                    ModBCSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <ModBCST> da <ICMS70> inválido! Valores aceitos: 0 a 5.");
                }
            }
        }

        [XmlIgnore]
        public double PMVAST { get; set; }

        [XmlElement("pMVAST")]
        public string PMVASTField
        {
            get => PMVAST.ToString("F4", CultureInfo.InvariantCulture);
            set => PMVAST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PRedBCSTField
        {
            get => PRedBCST.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }

        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string PICMSSTField
        {
            get => PICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPST { get; set; }

        [XmlElement("vBCFCPST")]
        public string VBCFCPSTField
        {
            get => VBCFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPST { get; set; }

        [XmlElement("pFCPST")]
        public string PFCPSTField
        {
            get => PFCPST.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPST { get; set; }

        [XmlElement("vFCPST")]
        public string VFCPSTField
        {
            get => VFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSDeson { get; set; }

        [XmlElement("vICMSDeson")]
        public string VICMSDesonField
        {
            get => VICMSDeson.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSDeson = Utility.Converter.ToDouble(value);
        }

        [XmlElement("motDesICMS")]
        public MotivoDesoneracaoICMS MotDesICMS { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVBCFCPField()
        {
            return VBCFCP > 0;
        }

        public bool ShouldSerializePFCPField()
        {
            return PFCP > 0;
        }

        public bool ShouldSerializeVFCPField()
        {
            return VFCP > 0;
        }

        public bool ShouldSerializePMVASTField()
        {
            return PMVAST > 0;
        }

        public bool ShouldSerializePRedBCSTField()
        {
            return PRedBCST > 0;
        }

        public bool ShouldSerializeVBCFCPSTField()
        {
            return VBCFCPST > 0;
        }

        public bool ShouldSerializePFCPSTField()
        {
            return PFCPST > 0;
        }

        public bool ShouldSerializeVFCPSTField()
        {
            return VFCPST > 0;
        }

        public bool ShouldSerializeVICMSDesonField()
        {
            return VICMSDeson > 0;
        }

        public bool ShouldSerializeMotDesICMS()
        {
            return VICMSDeson > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMS90 : ICMS70
    {
        [XmlElement("CST")]
        public override string CST { get; set; } = "90";
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSPart
    {
        private string CSTField;

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("10") || value.Equals("90"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <ICMSPart> inválido! Valores aceitos: 10 ou 90.");
                }
            }
        }

        [XmlElement("modBC")]
        public ModalidadeBaseCalculoICMS ModBC { get; set; }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBC { get; set; }

        [XmlElement("pRedBC")]
        public string PRedBCField
        {
            get => PRedBC.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMS { get; set; }

        [XmlElement("pICMS")]
        public string PICMSField
        {
            get => PICMS.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public ModalidadeBaseCalculoICMSST ModBCSTField { get; set; }

        [XmlElement("modBCST")]
        public ModalidadeBaseCalculoICMSST ModBCST
        {
            get => ModBCSTField;

            set
            {
                if (value != ModalidadeBaseCalculoICMSST.ValorOperacao)
                {
                    ModBCSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <ModBCST> da <ICMSPart> inválido! Valores aceitos: 0 a 5.");
                }
            }
        }

        [XmlIgnore]
        public double PMVAST { get; set; }

        [XmlElement("pMVAST")]
        public string PMVASTField
        {
            get => PMVAST.ToString("F4", CultureInfo.InvariantCulture);
            set => PMVAST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PRedBCSTField
        {
            get => PRedBCST.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }

        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string PICMSSTField
        {
            get => PICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PBCOp { get; set; }

        [XmlElement("pBCOp")]
        public string PBCOpField
        {
            get => PBCOp.ToString("F4", CultureInfo.InvariantCulture);
            set => PBCOp = Utility.Converter.ToDouble(value);
        }

        [XmlElement("UFST")]
        public UFBrasil UFST { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializePRedBCField()
        {
            return PRedBC > 0;
        }

        public bool ShouldSerializePRedBCSTField()
        {
            return PRedBCST > 0;
        }

        public bool ShouldSerializePMVASTField()
        {
            return PMVAST > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSSN101
    {
        private string CSOSNField = "101";

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN
        {
            get => CSOSNField;
            set
            {
                if (value.Equals("101"))
                {
                    CSOSNField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CSOSN> da <ICMSSN101> inválido! Valor aceito: 101.");
                }
            }
        }

        [XmlIgnore]
        public double PCredSN { get; set; }

        [XmlElement("pCredSN")]
        public string PCredSNField
        {
            get => PCredSN.ToString("F4", CultureInfo.InvariantCulture);
            set => PCredSN = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCredICMSSN { get; set; }

        [XmlElement("vCredICMSSN")]
        public string VCredICMSSNField
        {
            get => VCredICMSSN.ToString("F2", CultureInfo.InvariantCulture);
            set => VCredICMSSN = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSSN102
    {
        private string CSOSNField;

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN
        {
            get => CSOSNField;
            set
            {
                if (value.Equals("102") || value.Equals("103") || value.Equals("300") || value.Equals("400"))
                {
                    CSOSNField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CSOSN> da <ICMSSN102> inválido! Valores aceitos: 102, 103, 300 ou 400.");
                }
            }
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSSN201
    {
        private string CSOSNField = "201";

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN
        {
            get => CSOSNField;
            set
            {
                if (value.Equals("201"))
                {
                    CSOSNField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CSOSN> da <ICMSSN201> inválido! Valor aceito: 201.");
                }
            }
        }

        [XmlIgnore]
        public ModalidadeBaseCalculoICMSST ModBCSTField { get; set; }

        [XmlElement("modBCST")]
        public ModalidadeBaseCalculoICMSST ModBCST
        {
            get => ModBCSTField;

            set
            {
                if (value != ModalidadeBaseCalculoICMSST.ValorOperacao)
                {
                    ModBCSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <ModBCST> da <ICMSSN201> inválido! Valores aceitos: 0 a 5.");
                }
            }
        }

        [XmlIgnore]
        public double PMVAST { get; set; }

        [XmlElement("pMVAST")]
        public string PMVASTField
        {
            get => PMVAST.ToString("F4", CultureInfo.InvariantCulture);
            set => PMVAST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PRedBCSTField
        {
            get => PRedBCST.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }

        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string PICMSSTField
        {
            get => PICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPST { get; set; }

        [XmlElement("vBCFCPST")]
        public string VBCFCPSTField
        {
            get => VBCFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPST { get; set; }

        [XmlElement("pFCPST")]
        public string PFCPSTField
        {
            get => PFCPST.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPST { get; set; }

        [XmlElement("vFCPST")]
        public string VFCPSTField
        {
            get => VFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PCredSN { get; set; }

        [XmlElement("pCredSN")]
        public string PCredSNField
        {
            get => PCredSN.ToString("F4", CultureInfo.InvariantCulture);
            set => PCredSN = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCredICMSSN { get; set; }

        [XmlElement("vCredICMSSN")]
        public string VCredICMSSNField
        {
            get => VCredICMSSN.ToString("F2", CultureInfo.InvariantCulture);
            set => VCredICMSSN = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializePRedBCSTField()
        {
            return PRedBCST > 0;
        }

        public bool ShouldSerializePMVASTField()
        {
            return PMVAST > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSSN202
    {
        private string CSOSNField;

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN
        {
            get => CSOSNField;
            set
            {
                if (value.Equals("202") || value.Equals("203"))
                {
                    CSOSNField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CSOSN> da <ICMSSN202> inválido! Valores aceitos: 202 ou 203.");
                }
            }
        }

        [XmlIgnore]
        public ModalidadeBaseCalculoICMSST ModBCSTField { get; set; }

        [XmlElement("modBCST")]
        public ModalidadeBaseCalculoICMSST ModBCST
        {
            get => ModBCSTField;

            set
            {
                if (value != ModalidadeBaseCalculoICMSST.ValorOperacao)
                {
                    ModBCSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <ModBCST> da <ICMSSN202> inválido! Valores aceitos: 0 a 5.");
                }
            }
        }

        [XmlIgnore]
        public double PMVAST { get; set; }

        [XmlElement("pMVAST")]
        public string PMVASTField
        {
            get => PMVAST.ToString("F4", CultureInfo.InvariantCulture);
            set => PMVAST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PRedBCSTField
        {
            get => PRedBCST.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }

        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string PICMSSTField
        {
            get => PICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPST { get; set; }

        [XmlElement("vBCFCPST")]
        public string VBCFCPSTField
        {
            get => VBCFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPST { get; set; }

        [XmlElement("pFCPST")]
        public string PFCPSTField
        {
            get => PFCPST.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPST { get; set; }

        [XmlElement("vFCPST")]
        public string VFCPSTField
        {
            get => VFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPST = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializePRedBCSTField()
        {
            return PRedBCST > 0;
        }

        public bool ShouldSerializePMVASTField()
        {
            return PMVAST > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSSN500
    {
        private string CSOSNField = "500";

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN
        {
            get => CSOSNField;
            set
            {
                if (value.Equals("500"))
                {
                    CSOSNField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CSOSN> da <ICMSSN500> inválido! Valor aceito: 500.");
                }
            }
        }

        [XmlIgnore]
        public double VBCSTRet { get; set; }

        [XmlElement("vBCSTRet")]
        public string VBCSTRetField
        {
            get => VBCSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PST { get; set; }

        [XmlElement("pST")]
        public string PSTField
        {
            get => PST.ToString("F4", CultureInfo.InvariantCulture);
            set => PST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSSubstituto { get; set; }

        [XmlElement("vICMSSubstituto")]
        public string VICMSSubstitutoField
        {
            get => VICMSSubstituto.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSSubstituto = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSSTRet { get; set; }

        [XmlElement("vICMSSTRet")]
        public string VICMSSTRetField
        {
            get => VICMSSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPSTRet { get; set; }

        [XmlElement("vBCFCPSTRet")]
        public string VBCFCPSTRetField
        {
            get => VBCFCPSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPSTRet { get; set; }

        [XmlElement("pFCPSTRet")]
        public string PFCPSTRetField
        {
            get => PFCPSTRet.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPSTRet { get; set; }

        [XmlElement("vFCPSTRet")]
        public string VFCPSTRetField
        {
            get => VFCPSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCEfet { get; set; }

        [XmlElement("pRedBCEfet")]
        public string PRedBCEfetField
        {
            get => PRedBCEfet.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCEfet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCEfet { get; set; }

        [XmlElement("vBCEfet")]
        public string VBCEfetField
        {
            get => VBCEfet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCEfet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSEfet { get; set; }

        [XmlElement("pICMSEfet")]
        public string PICMSEfetField
        {
            get => PICMSEfet.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSEfet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSEfet { get; set; }

        [XmlElement("vICMSEfet")]
        public string VICMSEfetField
        {
            get => VICMSEfet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSEfet = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVBCSTRetField()
        {
            return VBCSTRet > 0;
        }

        public bool ShouldSerializePSTField()
        {
            return PST > 0;
        }

        public bool ShouldSerializeVICMSSubstitutoField()
        {
            return VICMSSubstituto > 0;
        }

        public bool ShouldSerializeVICMSSTRetField()
        {
            return VICMSSTRet > 0;
        }

        public bool ShouldSerializeVBCFCPSTRetField()
        {
            return VBCFCPSTRet > 0;
        }

        public bool ShouldSerializePFCPSTRetField()
        {
            return PFCPSTRet > 0;
        }

        public bool ShouldSerializeVFCPSTRetField()
        {
            return VFCPSTRet > 0;
        }

        public bool ShouldSerializePRedBCEfetField()
        {
            return PRedBCEfet > 0;
        }

        public bool ShouldSerializeVBCEfetField()
        {
            return VBCEfet > 0;
        }

        public bool ShouldSerializePICMSEfetField()
        {
            return PICMSEfet > 0;
        }

        public bool ShouldSerializeVICMSEfetField()
        {
            return VICMSEfet > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSSN900
    {
        private string CSOSNField = "900";

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN
        {
            get => CSOSNField;
            set
            {
                if (value.Equals("900"))
                {
                    CSOSNField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CSOSN> da <ICMSSN900> inválido! Valor aceito: 900.");
                }
            }
        }

        [XmlElement("modBC")]
        public ModalidadeBaseCalculoICMS? ModBC { get; set; }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBC { get; set; }

        [XmlElement("pRedBC")]
        public string PRedBCField
        {
            get => PRedBC.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMS { get; set; }

        [XmlElement("pICMS")]
        public string PICMSField
        {
            get => PICMS.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public ModalidadeBaseCalculoICMSST ModBCSTField { get; set; }

        [XmlElement("modBCST")]
        public ModalidadeBaseCalculoICMSST ModBCST
        {
            get => ModBCSTField;

            set
            {
                if (value != ModalidadeBaseCalculoICMSST.ValorOperacao)
                {
                    ModBCSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <ModBCST> da <ICMSSN900> inválido! Valores aceitos: 0 a 5.");
                }
            }
        }

        [XmlIgnore]
        public double PMVAST { get; set; }

        [XmlElement("pMVAST")]
        public string PMVASTField
        {
            get => PMVAST.ToString("F4", CultureInfo.InvariantCulture);
            set => PMVAST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PRedBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PRedBCSTField
        {
            get => PRedBCST.ToString("F4", CultureInfo.InvariantCulture);
            set => PRedBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }

        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string PICMSSTField
        {
            get => PICMSST.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string VICMSSTField
        {
            get => VICMSST.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPST { get; set; }

        [XmlElement("vBCFCPST")]
        public string VBCFCPSTField
        {
            get => VBCFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPST { get; set; }

        [XmlElement("pFCPST")]
        public string PFCPSTField
        {
            get => PFCPST.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPST { get; set; }

        [XmlElement("vFCPST")]
        public string VFCPSTField
        {
            get => VFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PCredSN { get; set; }

        [XmlElement("pCredSN")]
        public string PCredSNField
        {
            get => PCredSN.ToString("F4", CultureInfo.InvariantCulture);
            set => PCredSN = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCredICMSSN { get; set; }

        [XmlElement("vCredICMSSN")]
        public string VCredICMSSNField
        {
            get => VCredICMSSN.ToString("F2", CultureInfo.InvariantCulture);
            set => VCredICMSSN = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeModBC()
        {
            return ModBC != null;
        }

        public bool ShouldSerializeVBCField()
        {
            return VBC > 0;
        }

        public bool ShouldSerializePRedBCField()
        {
            return PRedBC > 0;
        }

        public bool ShouldSerializePICMSField()
        {
            return PICMS > 0;
        }

        public bool ShouldSerializeVICMSField()
        {
            return VICMS > 0;
        }

        public bool ShouldSerializeModBCST()
        {
            return Enum.IsDefined(typeof(ModalidadeBaseCalculoICMSST), ModBCST);
        }

        public bool ShouldSerializePMVASTField()
        {
            return PMVAST > 0;
        }

        public bool ShouldSerializePRedBCSTField()
        {
            return PRedBCST > 0;
        }

        public bool ShouldSerializeVBCSTField()
        {
            return VBCST > 0;
        }

        public bool ShouldSerializePICMSSTField()
        {
            return PICMSST > 0;
        }

        public bool ShouldSerializeVICMSSTField()
        {
            return VICMSST > 0;
        }

        public bool ShouldSerializeVBCFCPSTField()
        {
            return VBCFCPST > 0;
        }

        public bool ShouldSerializePFCPSTField()
        {
            return PFCPST > 0;
        }

        public bool ShouldSerializeVFCPSTField()
        {
            return VFCPST > 0;
        }

        public bool ShouldSerializePCredSNField()
        {
            return PCredSN > 0;
        }

        public bool ShouldSerializeVCredICMSSNField()
        {
            return VCredICMSSN > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSST
    {
        private string CSTField;

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("41") || value.Equals("60"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <ICMSST> inválido! Valores aceitos: 41 ou 60.");
                }
            }
        }

        [XmlIgnore]
        public double VBCSTRet { get; set; }

        [XmlElement("vBCSTRet")]
        public string VBCSTRetField
        {
            get => VBCSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSSTRet { get; set; }

        [XmlElement("vICMSSTRet")]
        public string VICMSSTRetField
        {
            get => VICMSSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCSTDest { get; set; }

        [XmlElement("vBCSTDest")]
        public string VBCSTDestField
        {
            get => VBCSTDest.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCSTDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSSTDest { get; set; }

        [XmlElement("vICMSSTDest")]
        public string VICMSSTDestField
        {
            get => VICMSSTDest.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSSTDest = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class II
    {
        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDespAdu { get; set; }

        [XmlElement("vDespAdu")]
        public string VDespAduField
        {
            get => VDespAdu.ToString("F2", CultureInfo.InvariantCulture);
            set => VDespAdu = Utility.Converter.ToDouble(value);
        }


        [XmlIgnore]
        public double VII { get; set; }

        [XmlElement("vII")]
        public string VIIField
        {
            get => VII.ToString("F2", CultureInfo.InvariantCulture);
            set => VII = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VIOF { get; set; }

        [XmlElement("vIOF")]
        public string VIOFField
        {
            get => VIOF.ToString("F2", CultureInfo.InvariantCulture);
            set => VIOF = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class IPI
    {
        [XmlElement("CNPJProd")]
        public string CNPJProd { get; set; }

        [XmlElement("cSelo")]
        public string CSelo { get; set; }

        [XmlElement("qSelo")]
        public double QSelo { get; set; }

        [XmlElement("cEnq")]
        public string CEnq { get; set; }

        [XmlElement("IPINT")]
        public IPINT IPINT { get; set; }

        [XmlElement("IPITrib")]
        public IPITrib IPITrib { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJProd()
        {
            return !string.IsNullOrWhiteSpace(CNPJProd);
        }

        public bool ShouldSerializeCSelo()
        {
            return !string.IsNullOrWhiteSpace(CSelo);
        }

        public bool ShouldSerializeQSelo()
        {
            return QSelo > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class IPINT
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("01") || value.Equals("02") || value.Equals("03") ||
                    value.Equals("04") || value.Equals("05") || value.Equals("51") ||
                    value.Equals("52") || value.Equals("53") || value.Equals("54") ||
                    value.Equals("55"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <IPINT> inválido! Valores aceitos: 01, 02, 03, 04, 05, 51, 52, 53, 54 ou 55.");
                }
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class IPITrib
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("00") || value.Equals("49") || value.Equals("50") ||
                    value.Equals("99"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <IPITrib> inválido! Valores aceitos: 00, 49, 50 ou 99.");
                }
            }
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PIPI { get; set; }

        [XmlElement("pIPI")]
        public string PIPIField
        {
            get => PIPI.ToString("F4", CultureInfo.InvariantCulture);
            set => PIPI = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double QUnid { get; set; }

        [XmlElement("qUnid")]
        public string QUnidField
        {
            get => QUnid.ToString("F4", CultureInfo.InvariantCulture);
            set => QUnid = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VUnid { get; set; }

        [XmlElement("vUnid")]
        public string VUnidField
        {
            get => VUnid.ToString("F4", CultureInfo.InvariantCulture);
            set => VUnid = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VIPI { get; set; }

        [XmlElement("vIPI")]
        public string VIPIField
        {
            get => VIPI.ToString("F2", CultureInfo.InvariantCulture);
            set => VIPI = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVBCField()
        {
            return VBC > 0;
        }

        public bool ShouldSerializePIPIField()
        {
            return PIPI > 0;
        }

        public bool ShouldSerializeQUnidField()
        {
            return QUnid > 0;
        }

        public bool ShouldSerializeVUnidField()
        {
            return VUnid > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ISSQN
    {
        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VAliq { get; set; }

        [XmlElement("vAliq")]
        public string VAliqField
        {
            get => VAliq.ToString("F4", CultureInfo.InvariantCulture);
            set => VAliq = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VISSQN { get; set; }

        [XmlElement("vISSQN")]
        public string VISSQNField
        {
            get => VISSQN.ToString("F2", CultureInfo.InvariantCulture);
            set => VISSQN = Utility.Converter.ToDouble(value);
        }

        [XmlElement("cMunFG")]
        public int CMunFG { get; set; }

        [XmlElement("cListServ")]
        public ListaServicoISSQN CListServ { get; set; }

        [XmlIgnore]
        public double VDeducao { get; set; }

        [XmlElement("vDeducao")]
        public string VDeducaoField
        {
            get => VDeducao.ToString("F2", CultureInfo.InvariantCulture);
            set => VDeducao = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VOutro { get; set; }

        [XmlElement("vOutro")]
        public string VOutroField
        {
            get => VOutro.ToString("F2", CultureInfo.InvariantCulture);
            set => VOutro = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDescIncond { get; set; }

        [XmlElement("vDescIncond")]
        public string VDescIncondField
        {
            get => VDescIncond.ToString("F2", CultureInfo.InvariantCulture);
            set => VDescIncond = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDescCond { get; set; }

        [XmlElement("vDescCond")]
        public string VDescCondField
        {
            get => VDescCond.ToString("F2", CultureInfo.InvariantCulture);
            set => VDescCond = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VISSRet { get; set; }

        [XmlElement("vISSRet")]
        public string VISSRetField
        {
            get => VISSRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VISSRet = Utility.Converter.ToDouble(value);
        }

        [XmlElement("indISS")]
        public IndicadorExigibilidadeISSQN IndISS { get; set; }

        [XmlElement("cServico")]
        public string CServico { get; set; }

        [XmlElement("cMun")]
        public int CMun { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("nProcesso")]
        public string NProcesso { get; set; }

        [XmlElement("indIncentivo")]
        public SimNao IndIncentivo { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVDeducaoField()
        {
            return VDeducao > 0;
        }

        public bool ShouldSerializeVOutroField()
        {
            return VOutro > 0;
        }

        public bool ShouldSerializeVDescIncondField()
        {
            return VDescIncond > 0;
        }

        public bool ShouldSerializeVDescCondField()
        {
            return VDescCond > 0;
        }

        public bool ShouldSerializeVISSRetField()
        {
            return VISSRet > 0;
        }

        public bool ShouldSerializeCServico()
        {
            return string.IsNullOrWhiteSpace(CServico);
        }

        public bool ShouldSerializeCMun()
        {
            return CMun > 0;
        }

        public bool ShouldSerializeCPais()
        {
            return CPais > 0;
        }

        public bool ShouldSerializeNProcesso()
        {
            return string.IsNullOrWhiteSpace(NProcesso);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class PIS
    {
        [XmlElement("PISAliq")]
        public PISAliq PISAliq { get; set; }

        [XmlElement("PISNT")]
        public PISNT PISNT { get; set; }

        [XmlElement("PISOutr")]
        public PISOutr PISOutr { get; set; }

        [XmlElement("PISQtde")]
        public PISQtde PISQtde { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class PISAliq
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("01") || value.Equals("02"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <PISAliq> inválido! Valores aceitos: 01 ou 02.");
                }
            }
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PPIS { get; set; }

        [XmlElement("pPIS")]
        public string PPISField
        {
            get => PPIS.ToString("F4", CultureInfo.InvariantCulture);
            set => PPIS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VPIS { get; set; }

        [XmlElement("vPIS")]
        public string VPISField
        {
            get => VPIS.ToString("F2", CultureInfo.InvariantCulture);
            set => VPIS = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class PISNT
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("04") || value.Equals("05") || value.Equals("06") || value.Equals("07") || value.Equals("08") || value.Equals("09"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <ICMSNT> inválido! Valores aceitos: 04, 05, 06, 07, 08 ou 09.");
                }
            }
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class PISOutr
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("49") || value.Equals("50") || value.Equals("51") || value.Equals("52") || value.Equals("53") || value.Equals("54") ||
                    value.Equals("55") || value.Equals("56") || value.Equals("60") || value.Equals("61") || value.Equals("62") || value.Equals("63") ||
                    value.Equals("64") || value.Equals("65") || value.Equals("66") || value.Equals("67") || value.Equals("70") || value.Equals("71") ||
                    value.Equals("72") || value.Equals("73") || value.Equals("74") || value.Equals("75") || value.Equals("98") || value.Equals("99"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <PISOutr> inválido! Valores aceitos: 49, 50, 51, 52, 53, 54, 55, 56, 60, 61, 62, 63, 64, 65, 66, 67, 70, 71, 72, 73, 74, 75, 98 ou 99.");
                }
            }
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PPIS { get; set; }

        [XmlElement("pPIS")]
        public string PPISField
        {
            get => PPIS.ToString("F4", CultureInfo.InvariantCulture);
            set => PPIS = Utility.Converter.ToDouble(value);
        }

        [XmlElement("qBCProd")]
        public double QBCProd { get; set; }

        [XmlElement("vAliqProd")]
        public double VAliqProd { get; set; }

        [XmlIgnore]
        public double VPIS { get; set; }

        [XmlElement("vPIS")]
        public string VPISField
        {
            get => VPIS.ToString("F2", CultureInfo.InvariantCulture);
            set => VPIS = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeQBCProd()
        {
            return QBCProd > 0;
        }

        public bool ShouldSerializeVAliqProd()
        {
            return VAliqProd > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class PISQtde
    {
        private string CSTField;

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("03"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <PISQtde> inválido! Valor aceito: 03");
                }
            }
        }

        [XmlElement("qBCProd")]
        public double QBCProd { get; set; }

        [XmlElement("vAliqProd")]
        public double VAliqProd { get; set; }

        [XmlIgnore]
        public double VPIS { get; set; }

        [XmlElement("vPIS")]
        public string VPISField
        {
            get => VPIS.ToString("F2", CultureInfo.InvariantCulture);
            set => VPIS = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class PISST
    {
        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PPIS { get; set; }

        [XmlElement("pPIS")]
        public string PPISField
        {
            get => PPIS.ToString("F4", CultureInfo.InvariantCulture);
            set => PPIS = Utility.Converter.ToDouble(value);
        }


        [XmlElement("qBCProd")]
        public double QBCProd { get; set; }

        [XmlElement("vAliqProd")]
        public double VAliqProd { get; set; }

        [XmlIgnore]
        public double VPIS { get; set; }

        [XmlElement("vPIS")]
        public string VPISField
        {
            get => VPIS.ToString("F2", CultureInfo.InvariantCulture);
            set => VPIS = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializePPISField()
        {
            return PPIS > 0;
        }

        public bool ShouldSerializeQBCProd()
        {
            return QBCProd > 0;
        }

        public bool ShouldSerializeVAliqProd()
        {
            return VAliqProd > 0;
        }

        public bool ShouldSerializeVBCField()
        {
            return VBC > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class COFINS
    {
        [XmlElement("COFINSAliq")]
        public COFINSAliq COFINSAliq { get; set; }

        [XmlElement("COFINSNT")]
        public COFINSNT COFINSNT { get; set; }

        [XmlElement("COFINSOutr")]
        public COFINSOutr COFINSOutr { get; set; }

        [XmlElement("COFINSQtde")]
        public COFINSQtde COFINSQtde { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class COFINSAliq
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("01") || value.Equals("02"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <COFINSAliq> inválido! Valores aceitos: 01 ou 02.");
                }
            }
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PCOFINS { get; set; }

        [XmlElement("pCOFINS")]
        public string PCOFINSField
        {
            get => PCOFINS.ToString("F4", CultureInfo.InvariantCulture);
            set => PCOFINS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCOFINS { get; set; }

        [XmlElement("vCOFINS")]
        public string VCOFINSField
        {
            get => VCOFINS.ToString("F2", CultureInfo.InvariantCulture);
            set => VCOFINS = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class COFINSNT
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("04") || value.Equals("05") || value.Equals("06") || value.Equals("07") || value.Equals("08") || value.Equals("09"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <COFINSNT> inválido! Valores aceitos: 04, 05, 06, 07, 08 ou 09.");
                }
            }
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class COFINSOutr
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("49") || value.Equals("50") || value.Equals("51") || value.Equals("52") || value.Equals("53") || value.Equals("54") ||
                    value.Equals("55") || value.Equals("56") || value.Equals("60") || value.Equals("61") || value.Equals("62") || value.Equals("63") ||
                    value.Equals("64") || value.Equals("65") || value.Equals("66") || value.Equals("67") || value.Equals("70") || value.Equals("71") ||
                    value.Equals("72") || value.Equals("73") || value.Equals("74") || value.Equals("75") || value.Equals("98") || value.Equals("99"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <COFINSOutr> inválido! Valores aceitos: 49, 50, 51, 52, 53, 54, 55, 56, 60, 61, 62, 63, 64, 65, 66, 67, 70, 71, 72, 73, 74, 75, 98 ou 99.");
                }
            }
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PCOFINS { get; set; }

        [XmlElement("pCOFINS")]
        public string PCOFINSField
        {
            get => PCOFINS.ToString("F4", CultureInfo.InvariantCulture);
            set => PCOFINS = Utility.Converter.ToDouble(value);
        }

        [XmlElement("qBCProd")]
        public double QBCProd { get; set; }

        [XmlElement("vAliqProd")]
        public double VAliqProd { get; set; }

        [XmlIgnore]
        public double VCOFINS { get; set; }

        [XmlElement("vCOFINS")]
        public string VCOFINSField
        {
            get => VCOFINS.ToString("F2", CultureInfo.InvariantCulture);
            set => VCOFINS = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeQBCProd()
        {
            return QBCProd > 0;
        }

        public bool ShouldSerializeVAliqProd()
        {
            return VAliqProd > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class COFINSQtde
    {
        private string CSTField;

        [XmlElement("orig")]
        public OrigemMercadoria Orig { get; set; }

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if (value.Equals("03"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <COFINSQtde> inválido! Valor aceito: 03");
                }
            }
        }

        [XmlElement("qBCProd")]
        public double QBCProd { get; set; }

        [XmlElement("vAliqProd")]
        public double VAliqProd { get; set; }

        [XmlIgnore]
        public double VCOFINS { get; set; }

        [XmlElement("vCOFINS")]
        public string VCOFINSField
        {
            get => VCOFINS.ToString("F2", CultureInfo.InvariantCulture);
            set => VCOFINS = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class COFINSST
    {
        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PCOFINS { get; set; }

        [XmlElement("pCOFINS")]
        public string PCOFINSField
        {
            get => PCOFINS.ToString("F4", CultureInfo.InvariantCulture);
            set => PCOFINS = Utility.Converter.ToDouble(value);
        }

        [XmlElement("qBCProd")]
        public double QBCProd { get; set; }

        [XmlElement("vAliqProd")]
        public double VAliqProd { get; set; }

        [XmlIgnore]
        public double VCOFINS { get; set; }

        [XmlElement("vCOFINS")]
        public string VCOFINSField
        {
            get => VCOFINS.ToString("F2", CultureInfo.InvariantCulture);
            set => VCOFINS = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializePCOFINSField()
        {
            return PCOFINS > 0;
        }

        public bool ShouldSerializeQBCProd()
        {
            return QBCProd > 0;
        }

        public bool ShouldSerializeVAliqProd()
        {
            return VAliqProd > 0;
        }

        public bool ShouldSerializeVBCField()
        {
            return VBC > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSUFDest
    {
        [XmlIgnore]
        public double VBCUFDest { get; set; }

        [XmlElement("vBCUFDest")]
        public string VBCUFDestField
        {
            get => VBCUFDest.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCUFDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCFCPUFDest { get; set; }

        [XmlElement("vBCFCPUFDest")]
        public string VBCFCPUFDestField
        {
            get => VBCFCPUFDest.ToString("F4", CultureInfo.InvariantCulture);
            set => VBCFCPUFDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPUFDest { get; set; }

        [XmlElement("pFCPUFDest")]
        public string PFCPUFDestField
        {
            get => PFCPUFDest.ToString("F4", CultureInfo.InvariantCulture);
            set => PFCPUFDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSUFDest { get; set; }

        [XmlElement("pICMSUFDest")]
        public string PICMSUFDestField
        {
            get => PICMSUFDest.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSUFDest = Utility.Converter.ToDouble(value);
        }

        private double PICMSInterField2;

        [XmlIgnore]
        public double PICMSInter
        {
            get => PICMSInterField2;
            set
            {
                if (value == 4 || value == 7 || value == 12)
                {
                    PICMSInterField2 = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <PICMSInter> da tag <ICMSUFDest> inválido! Valores aceitos: 4, 7 ou 12.");
                }
            }
        }

        [XmlElement("pICMSInter")]
        public string PICMSInterField
        {
            get => PICMSInter.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSInter = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSInterPart { get; set; } = 100;

        [XmlElement("pICMSInterPart")]
        public string PICMSInterPartField
        {
            get => PICMSInterPart.ToString("F4", CultureInfo.InvariantCulture);
            set => PICMSInterPart = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPUFDest { get; set; }

        [XmlElement("vFCPUFDest")]
        public string VFCPUFDestField
        {
            get => VFCPUFDest.ToString("F4", CultureInfo.InvariantCulture);
            set => VFCPUFDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSUFDest { get; set; }

        [XmlElement("vICMSUFDest")]
        public string VICMSUFDestField
        {
            get => VICMSUFDest.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSUFDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSUFRemet { get; set; }

        [XmlElement("vICMSUFRemet")]
        public string VICMSUFRemetField
        {
            get => VICMSUFRemet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSUFRemet = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVBCFCPUFDestField()
        {
            return VBCFCPUFDest > 0;
        }

        public bool ShouldSerializePFCPUFDestField()
        {
            return PFCPUFDest > 0;
        }

        public bool ShouldSerializeVFCPUFDestField()
        {
            return VFCPUFDest > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ImpostoDevol
    {
        [XmlIgnore]
        public double PDevol { get; set; }

        [XmlElement("pDevol")]
        public string PDevolField
        {
            get => PDevol.ToString("F2", CultureInfo.InvariantCulture);
            set => PDevol = Utility.Converter.ToDouble(value);
        }

        [XmlElement("IPI")]
        public IPIDevol IPI { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class IPIDevol
    {
        [XmlIgnore]
        public double VIPIDevol { get; set; }

        [XmlElement("vIPIDevol")]
        public string VIPIDevolField
        {
            get => VIPIDevol.ToString("F2", CultureInfo.InvariantCulture);
            set => VIPIDevol = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Total
    {
        [XmlElement("ICMSTot")]
        public ICMSTot ICMSTot { get; set; }

        [XmlElement("ISSQNtot")]
        public ISSQNtot ISSQNtot { get; set; }

        [XmlElement("retTrib")]
        public RetTrib RetTrib { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ICMSTot
    {
        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMS { get; set; }
        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSDeson { get; set; }
        [XmlElement("vICMSDeson")]
        public string VICMSDesonField
        {
            get => VICMSDeson.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSDeson = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPUFDest { get; set; }
        [XmlElement("vFCPUFDest")]
        public string VFCPUFDestField
        {
            get => VFCPUFDest.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPUFDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSUFDest { get; set; }
        [XmlElement("vICMSUFDest")]
        public string VICMSUFDestField
        {
            get => VICMSUFDest.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSUFDest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSUFRemet { get; set; }
        [XmlElement("vICMSUFRemet")]
        public string VICMSUFRemetField
        {
            get => VICMSUFRemet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSUFRemet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCP { get; set; }
        [XmlElement("vFCP")]
        public string VFCPField
        {
            get => VFCP.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCP = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCST { get; set; }
        [XmlElement("vBCST")]
        public string VBCSTField
        {
            get => VBCST.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VST { get; set; }
        [XmlElement("vST")]
        public string VSTField
        {
            get => VST.ToString("F2", CultureInfo.InvariantCulture);
            set => VST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPST { get; set; }
        [XmlElement("vFCPST")]
        public string VFCPSTField
        {
            get => VFCPST.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPST = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPSTRet { get; set; }
        [XmlElement("vFCPSTRet")]
        public string VFCPSTRetField
        {
            get => VFCPSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VProd { get; set; }
        [XmlElement("vProd")]
        public string VProdField
        {
            get => VProd.ToString("F2", CultureInfo.InvariantCulture);
            set => VProd = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFrete { get; set; }
        [XmlElement("vFrete")]
        public string VFreteField
        {
            get => VFrete.ToString("F2", CultureInfo.InvariantCulture);
            set => VFrete = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VSeg { get; set; }
        [XmlElement("vSeg")]
        public string VSegField
        {
            get => VSeg.ToString("F2", CultureInfo.InvariantCulture);
            set => VSeg = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDesc { get; set; }
        [XmlElement("vDesc")]
        public string VDescField
        {
            get => VDesc.ToString("F2", CultureInfo.InvariantCulture);
            set => VDesc = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VII { get; set; }
        [XmlElement("vII")]
        public string VIIField
        {
            get => VII.ToString("F2", CultureInfo.InvariantCulture);
            set => VII = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VIPI { get; set; }
        [XmlElement("vIPI")]
        public string VIPIField
        {
            get => VIPI.ToString("F2", CultureInfo.InvariantCulture);
            set => VIPI = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VIPIDevol { get; set; }
        [XmlElement("vIPIDevol")]
        public string VIPIDevolField
        {
            get => VIPIDevol.ToString("F2", CultureInfo.InvariantCulture);
            set => VIPIDevol = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VPIS { get; set; }
        [XmlElement("vPIS")]
        public string VPISField
        {
            get => VPIS.ToString("F2", CultureInfo.InvariantCulture);
            set => VPIS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCOFINS { get; set; }
        [XmlElement("vCOFINS")]
        public string VCOFINSField
        {
            get => VCOFINS.ToString("F2", CultureInfo.InvariantCulture);
            set => VCOFINS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VOutro { get; set; }
        [XmlElement("vOutro")]
        public string VOutroField
        {
            get => VOutro.ToString("F2", CultureInfo.InvariantCulture);
            set => VOutro = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VNF { get; set; }
        [XmlElement("vNF")]
        public string VNFField
        {
            get => VNF.ToString("F2", CultureInfo.InvariantCulture);
            set => VNF = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VTotTrib { get; set; }
        [XmlElement("vTotTrib")]
        public string VTotTribField
        {
            get => VTotTrib.ToString("F2", CultureInfo.InvariantCulture);
            set => VTotTrib = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVFCPUFDestField()
        {
            return VFCPUFDest > 0;
        }

        public bool ShouldSerializeVICMSUFDestField()
        {
            return VICMSUFDest > 0;
        }

        public bool ShouldSerializeVICMSUFRemetField()
        {
            return VICMSUFRemet > 0;
        }

        public bool ShouldSerializeVTotTribField()
        {
            return VTotTrib > 0;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ISSQNtot
    {
        [XmlIgnore]
        public double VServ { get; set; }

        [XmlElement("vServ")]
        public string VServField
        {
            get => VServ.ToString("F2", CultureInfo.InvariantCulture);
            set => VServ = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBC { get; set; }

        [XmlElement("vBC")]
        public string VBCField
        {
            get => VBC.ToString("F2", CultureInfo.InvariantCulture);
            set => VBC = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VISS { get; set; }

        [XmlElement("vISS")]
        public string VISSField
        {
            get => VISS.ToString("F2", CultureInfo.InvariantCulture);
            set => VISS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VPIS { get; set; }

        [XmlElement("vPIS")]
        public string VPISField
        {
            get => VPIS.ToString("F2", CultureInfo.InvariantCulture);
            set => VPIS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCOFINS { get; set; }

        [XmlElement("vCOFINS")]
        public string VCOFINSField
        {
            get => VCOFINS.ToString("F2", CultureInfo.InvariantCulture);
            set => VCOFINS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public DateTime DCompet { get; set; }

        [XmlElement("dCompet")]
        public string DCompetField
        {
            get => DCompet.ToString("yyyy-MM-dd");
            set => DCompet = DateTime.Parse(value);
        }

        [XmlIgnore]
        public double VDeducao { get; set; }

        [XmlElement("vDeducao")]
        public string VDeducaoField
        {
            get => VDeducao.ToString("F2", CultureInfo.InvariantCulture);
            set => VDeducao = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VOutro { get; set; }

        [XmlElement("vOutro")]
        public string VOutroField
        {
            get => VOutro.ToString("F2", CultureInfo.InvariantCulture);
            set => VOutro = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDescIncond { get; set; }

        [XmlElement("vDescIncond")]
        public string VDescIncondField
        {
            get => VDescIncond.ToString("F2", CultureInfo.InvariantCulture);
            set => VDescIncond = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDescCond { get; set; }

        [XmlElement("vDescCond")]
        public string VDescCondField
        {
            get => VDescCond.ToString("F2", CultureInfo.InvariantCulture);
            set => VDescCond = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VISSRet { get; set; }

        [XmlElement("vISSRet")]
        public string VISSRetField
        {
            get => VISSRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VISSRet = Utility.Converter.ToDouble(value);
        }

        [XmlElement("cRegTrib")]
        public CodigoRegimeEspecialTributacao CRegTrib { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVServField()
        {
            return VServ > 0;
        }

        public bool ShouldSerializeVBCField()
        {
            return VBC > 0;
        }

        public bool ShouldSerializeVISSField()
        {
            return VISS > 0;
        }

        public bool ShouldSerializeVPISField()
        {
            return VPIS > 0;
        }

        public bool ShouldSerializeVCOFINSField()
        {
            return VCOFINS > 0;
        }

        public bool ShouldSerializeVDeducaoField()
        {
            return VDeducao > 0;
        }

        public bool ShouldSerializeVOutroField()
        {
            return VOutro > 0;
        }

        public bool ShouldSerializeVDescIncondField()
        {
            return VDescIncond > 0;
        }

        public bool ShouldSerializeVDescCondField()
        {
            return VDescCond > 0;
        }

        public bool ShouldSerializeVISSRetField()
        {
            return VISSRet > 0;
        }

        public bool ShouldSerializeCRegTrib()
        {
            return Enum.IsDefined(typeof(CodigoRegimeEspecialTributacao), CRegTrib);
        }

        #endregion

    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class RetTrib
    {
        [XmlIgnore]
        public double VRetPIS { get; set; }

        [XmlElement("vRetPIS")]
        public string VRetPISField
        {
            get => VRetPIS.ToString("F2", CultureInfo.InvariantCulture);
            set => VRetPIS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VRetCOFINS { get; set; }

        [XmlElement("vRetCOFINS")]
        public string VRetCOFINSField
        {
            get => VRetCOFINS.ToString("F2", CultureInfo.InvariantCulture);
            set => VRetCOFINS = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VRetCSLL { get; set; }

        [XmlElement("vRetCSLL")]
        public string VRetCSLLField
        {
            get => VRetCSLL.ToString("F2", CultureInfo.InvariantCulture);
            set => VRetCSLL = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCIRRF { get; set; }

        [XmlElement("vBCIRRF")]
        public string VBCIRRFField
        {
            get => VBCIRRF.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCIRRF = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VIRRF { get; set; }

        [XmlElement("vIRRF")]
        public string VIRRFField
        {
            get => VIRRF.ToString("F2", CultureInfo.InvariantCulture);
            set => VIRRF = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCRetPrev { get; set; }

        [XmlElement("vBCRetPrev")]
        public string VBCRetPrevField
        {
            get => VBCRetPrev.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCRetPrev = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VRetPrev { get; set; }

        [XmlElement("vRetPrev")]
        public string VRetPrevField
        {
            get => VRetPrev.ToString("F2", CultureInfo.InvariantCulture);
            set => VRetPrev = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVRetPISField()
        {
            return VRetPIS > 0;
        }

        public bool ShouldSerializeVRetCOFINSField()
        {
            return VRetCOFINS > 0;
        }

        public bool ShouldSerializeVRetCSLLField()
        {
            return VRetCSLL > 0;
        }

        public bool ShouldSerializeVBCIRRFField()
        {
            return VBCIRRF > 0;
        }

        public bool ShouldSerializeVIRRFField()
        {
            return VIRRF > 0;
        }

        public bool ShouldSerializeVBCRetPrevField()
        {
            return VBCRetPrev > 0;
        }

        public bool ShouldSerializeVRetPrevField()
        {
            return VRetPrev > 0;
        }

        #endregion

    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Transp
    {
        [XmlElement("modFrete")]
        public ModalidadeFrete ModFrete { get; set; }

        [XmlElement("transporta")]
        public Transporta Transporta { get; set; }

        [XmlElement("retTransp")]
        public RetTransp RetTransp { get; set; }

        [XmlElement("veicTransp")]
        public VeicTransp VeicTransp { get; set; }

        [XmlElement("reboque")]
        public Reboque Reboque { get; set; }

        [XmlElement("vagao")]
        public string Vagao { get; set; }

        [XmlElement("balsa")]
        public string Balsa { get; set; }

        [XmlElement("vol")]
        public Vol[] Vol { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVagao()
        {
            return !string.IsNullOrWhiteSpace(Vagao);
        }

        public bool ShouldSerializeBalsa()
        {
            return !string.IsNullOrWhiteSpace(Balsa);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Transporta
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("xEnder")]
        public string XEnder { get; set; }

        [XmlElement("xMun")]
        public string XMun { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrWhiteSpace(CNPJ);
        }

        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrWhiteSpace(CPF);
        }

        public bool ShouldSerializeXNome()
        {
            return !string.IsNullOrWhiteSpace(XNome);
        }

        public bool ShouldSerializeIE()
        {
            return !string.IsNullOrWhiteSpace(IE);
        }

        public bool ShouldSerializeXEnder()
        {
            return !string.IsNullOrWhiteSpace(XEnder);
        }

        public bool ShouldSerializeXMun()
        {
            return !string.IsNullOrWhiteSpace(XMun);
        }

        public bool ShouldSerializeUF()
        {
            return Enum.IsDefined(typeof(UFBrasil), UF);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class RetTransp
    {
        [XmlIgnore]
        public double VServ { get; set; }

        [XmlElement("vServ")]
        public string VServField
        {
            get => VServ.ToString("F2", CultureInfo.InvariantCulture);
            set => VServ = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCRet { get; set; }

        [XmlElement("vBCRet")]
        public string VBCRetField
        {
            get => VBCRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSRet { get; set; }

        [XmlElement("pICMSRet")]
        public string PICMSRetField
        {
            get => PICMSRet.ToString("F2", CultureInfo.InvariantCulture);
            set => PICMSRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSRet { get; set; }

        [XmlElement("vICMSRet")]
        public string VICMSRetRetField
        {
            get => VICMSRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSRet = Utility.Converter.ToDouble(value);
        }

        [XmlElement("CFOP")]
        public string CFOP { get; set; }

        [XmlElement("cMunFG")]
        public int CMunFG { get; set; }
    }

    public abstract class VeiculoBase
    {
        [XmlElement("placa")]
        public string Placa { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("RNTC")]
        public string RNTC { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeRNTC()
        {
            return !string.IsNullOrWhiteSpace(RNTC);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class VeicTransp : VeiculoBase { }
    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Reboque : VeiculoBase { }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Vol
    {
        [XmlElement("qVol")]
        public double QVol { get; set; }

        [XmlElement("esp")]
        public string Esp { get; set; }

        [XmlElement("marca")]
        public string Marca { get; set; }

        [XmlElement("nVol")]
        public string NVol { get; set; }

        [XmlIgnore]
        public double PesoL { get; set; }

        [XmlElement("pesoL")]
        public string PesoLField
        {
            get => PesoL.ToString("F3", CultureInfo.InvariantCulture);
            set => PesoL = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PesoB { get; set; }

        [XmlElement("pesoB")]
        public string PesoBField
        {
            get => PesoB.ToString("F3", CultureInfo.InvariantCulture);
            set => PesoB = Utility.Converter.ToDouble(value);
        }

        [XmlElement("lacres")]
        public Lacres[] Lacres { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeEsp()
        {
            return !string.IsNullOrWhiteSpace(Esp);
        }

        public bool ShouldSerializeMarca()
        {
            return !string.IsNullOrWhiteSpace(Marca);
        }

        public bool ShouldSerializeNVol()
        {
            return !string.IsNullOrWhiteSpace(NVol);
        }

        public bool ShouldSerializeQVol()
        {
            return QVol > 0;
        }

        public bool ShouldSerializePesoLField()
        {
            return PesoL > 0;
        }

        public bool ShouldSerializePesoBField()
        {
            return PesoB > 0;
        }

        #endregion

    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Lacres
    {
        [XmlElement("nLacre")]
        public string NLacre { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Cobr
    {
        [XmlElement("fat")]
        public Fat Fat { get; set; }

        [XmlElement("dup")]
        public Dup[] Dup { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Fat
    {
        [XmlElement("nFat")]
        public string NFat { get; set; }

        [XmlIgnore]
        public double VOrig { get; set; }

        [XmlElement("vOrig")]
        public string VOrigField
        {
            get => VOrig.ToString("F2", CultureInfo.InvariantCulture);
            set => VOrig = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VDesc { get; set; }

        [XmlElement("vDesc")]
        public string VDescField
        {
            get => VDesc.ToString("F2", CultureInfo.InvariantCulture);
            set => VDesc = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VLiq { get; set; }

        [XmlElement("vLiq")]
        public string VLiqField
        {
            get => VLiq.ToString("F2", CultureInfo.InvariantCulture);
            set => VLiq = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Dup
    {
        [XmlElement("nDup")]
        public string NDup { get; set; }

        [XmlIgnore]
        public DateTime DVenc { get; set; }

        [XmlElement("dVenc")]
        public string DVencField
        {
            get => DVenc.ToString("yyyy-MM-dd");
            set => DVenc = DateTime.Parse(value);
        }

        [XmlIgnore]
        public double VDup { get; set; }

        [XmlElement("vDup")]
        public string VDupField
        {
            get => VDup.ToString("F2", CultureInfo.InvariantCulture);
            set => VDup = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Pag
    {
        [XmlElement("detPag")]
        public DetPag[] DetPag { get; set; }

        [XmlIgnore]
        public double VTroco { get; set; }

        [XmlElement("vTroco")]
        public string VTrocoField
        {
            get => VTroco.ToString("F2", CultureInfo.InvariantCulture);
            set => VTroco = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DetPag
    {
        [XmlElement("indPag")]
        public IndicadorPagamento? IndPag { get; set; }

        [XmlElement("tPag")]
        public MeioPagamento TPag { get; set; }

        [XmlIgnore]
        public double VPag { get; set; }

        [XmlElement("vPag")]
        public string VPagField
        {
            get => VPag.ToString("F2", CultureInfo.InvariantCulture);
            set => VPag = Utility.Converter.ToDouble(value);
        }

        [XmlElement("card")]
        public Card Card { get; set; }

        public bool ShouldSerializeIndPag()
        {
            return IndPag != null;
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Card
    {
        [XmlElement("tpIntegra")]
        public TipoIntegracaoPagamento TpIntegra { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("tBand")]
        public BandeiraOperadoraCartao TBand { get; set; }

        [XmlElement("cAut")]
        public string CAut { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ()
        {
            return TpIntegra == TipoIntegracaoPagamento.PagamentoIntegrado;
        }

        public bool ShouldSerializeTBand()
        {
            return TpIntegra == TipoIntegracaoPagamento.PagamentoIntegrado;
        }

        public bool ShouldSerializeCAut()
        {
            return TpIntegra == TipoIntegracaoPagamento.PagamentoIntegrado;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfAdic
    {
        [XmlElement("infAdFisco")]
        public string InfAdFisco { get; set; }

        [XmlElement("infCpl")]
        public string InfCpl { get; set; }

        [XmlElement("obsCont")]
        public ObsCont[] ObsCont { get; set; }

        /// <summary>
        /// Uso exclusivo do fisco. Existe somente para deserialização, não utilizar.
        /// </summary>
        [XmlElement("obsFisco")]
        public ObsFisco[] ObsFisco { get; set; }

        [XmlElement("procRef")]
        public ProcRef[] ProcRef { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeInfAdFisco()
        {
            return !string.IsNullOrWhiteSpace(InfAdFisco);
        }

        public bool ShouldSerializeInfCpl()
        {
            return !string.IsNullOrWhiteSpace(InfCpl);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ObsCont
    {
        [XmlElement("xTexto")]
        public string XTexto { get; set; }

        [XmlAttribute(AttributeName = "xCampo")]
        public string XCampo { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ObsFisco
    {
        [XmlElement("xTexto")]
        public string XTexto { get; set; }

        [XmlAttribute(AttributeName = "xCampo")]
        public string XCampo { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ProcRef
    {
        [XmlElement("nProc")]
        public string NProc { get; set; }

        [XmlElement("indProc")]
        public IndicadorOrigemProcesso IndProc { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Exporta
    {
        private UFBrasil UFSaidaPaisField;

        [XmlElement("UFSaidaPais")]
        public UFBrasil UFSaidaPais
        {
            get => UFSaidaPaisField;
            set
            {
                if (value == UFBrasil.EX || value == UFBrasil.AN)
                {
                    throw new Exception("Conteúdo da TAG <UFSaidaPais> inválido. Não pode ser informado EX ou AN.");
                }
                else
                {
                    UFSaidaPaisField = value;
                }
            }
        }

        [XmlElement("xLocExporta")]
        public string XLocExporta { get; set; }

        [XmlElement("xLocDespacho")]
        public string XLocDespacho { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeXLocDespacho()
        {
            return !string.IsNullOrWhiteSpace(XLocDespacho);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Compra
    {
        [XmlElement("xNEmp")]
        public string XNEmp { get; set; }

        [XmlElement("xPed")]
        public string XPed { get; set; }

        [XmlElement("xCont")]
        public string XCont { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeXNEmp()
        {
            return !string.IsNullOrWhiteSpace(XNEmp);
        }

        public bool ShouldSerializeXPed()
        {
            return !string.IsNullOrWhiteSpace(XPed);
        }

        public bool ShouldSerializeXCont()
        {
            return !string.IsNullOrWhiteSpace(XCont);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Cana
    {
        [XmlElement("safra")]
        public string Safra { get; set; }

        [XmlElement("ref")]
        public string Ref { get; set; }

        [XmlElement("forDia")]
        public ForDia[] ForDia { get; set; }

        [XmlIgnore]
        public double QTotMes { get; set; }

        [XmlElement("qTotMes")]
        public string QTotMesField
        {
            get => QTotMes.ToString("F10", CultureInfo.InvariantCulture);
            set => QTotMes = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double QTotAnt { get; set; }

        [XmlElement("qTotAnt")]
        public string QTotAntField
        {
            get => QTotAnt.ToString("F10", CultureInfo.InvariantCulture);
            set => QTotAnt = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double QTotGer { get; set; }

        [XmlElement("qTotGer")]
        public string QTotGerField
        {
            get => QTotGer.ToString("F10", CultureInfo.InvariantCulture);
            set => QTotGer = Utility.Converter.ToDouble(value);
        }

        [XmlElement("deduc")]
        public Deduc[] Deduc { get; set; }

        [XmlIgnore]
        public double VFor { get; set; }

        [XmlElement("vFor")]
        public string VForField
        {
            get => VFor.ToString("F2", CultureInfo.InvariantCulture);
            set => VFor = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VTotDed { get; set; }

        [XmlElement("vTotDed")]
        public string VTotDedField
        {
            get => VTotDed.ToString("F2", CultureInfo.InvariantCulture);
            set => VTotDed = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VLiqFor { get; set; }

        [XmlElement("vLiqFor")]
        public string VLiqForField
        {
            get => VLiqFor.ToString("F2", CultureInfo.InvariantCulture);
            set => VLiqFor = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ForDia
    {
        [XmlIgnore]
        public double Qtde { get; set; }

        [XmlElement("qtde")]
        public string QtdeField
        {
            get => Qtde.ToString("F10", CultureInfo.InvariantCulture);
            set => Qtde = Utility.Converter.ToDouble(value);
        }

        [XmlElement("dia")]
        public int Dia { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class Deduc
    {
        [XmlElement("xDed")]
        public string XDed { get; set; }

        [XmlIgnore]
        public double VDed { get; set; }

        [XmlElement("vDed")]
        public string VDedField
        {
            get => VDed.ToString("F2", CultureInfo.InvariantCulture);
            set => VDed = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfRespTec
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("xContato")]
        public string XContato { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("idCSRT")]
        public string IdCSRT { get; set; }

        [XmlElement("hashCSRT", DataType = "base64Binary")]
        public byte[] HashCSRT { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeIdCSRT()
        {
            return !string.IsNullOrWhiteSpace(IdCSRT);
        }

        public bool ShouldSerializeHashCSRT()
        {
            return HashCSRT != null;
        }

        #endregion
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class InfNFeSupl
    {
        [XmlElement("qrCode")]
        public string QrCode { get; set; }

        [XmlElement("urlChave")]
        public string UrlChave { get; set; }
    }
}
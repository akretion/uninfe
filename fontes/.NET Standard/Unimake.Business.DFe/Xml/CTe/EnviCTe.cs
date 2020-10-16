﻿#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.CTe
{
    [Serializable()]
    [XmlRoot("enviCTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class EnviCTe: XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public string IdLote { get; set; }

        [XmlElement("CTe")]
        public List<CTe> CTe { get; set; }

        public override XmlDocument GerarXML()
        {
            var xmlDoc = base.GerarXML();

            foreach(var nodeEnvCTe in xmlDoc.GetElementsByTagName("enviCTe"))
            {
                var elemEnvCTe = (XmlElement)nodeEnvCTe;

                foreach(var nodeCTe in elemEnvCTe.GetElementsByTagName("CTe"))
                {
                    var elemCTe = (XmlElement)nodeCTe;

                    var attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
                    elemCTe.SetAttribute("xmlns", attribute.Namespace);
                }
            }

            return xmlDoc;
        }

        #region Add (List - Interop)

        public void AddCTe(CTe cte)
        {
            if(CTe == null)
            {
                CTe = new List<CTe>();
            }

            CTe.Add(cte);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    [XmlRoot("CTe", Namespace = "http://www.portalfiscal.inf.br/cte", IsNullable = false)]
    public class CTe
    {
        [XmlElement("infCte")]
        public InfCTe InfCTe { get; set; }

        [XmlElement("infCTeSupl")]
        public InfCTeSupl InfCTeSupl { get; set; }

        [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Signature Signature { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCTe
    {
        private string IdField;

        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("ide")]
        public Ide Ide { get; set; }

        [XmlElement("Compl")]
        public Compl Compl { get; set; }

        [XmlElement("emit")]
        public Emit Emit { get; set; }

        [XmlElement("rem")]
        public Rem Rem { get; set; }

        [XmlElement("exped")]
        public Exped Exped { get; set; }

        [XmlElement("receb")]
        public Receb Receb { get; set; }

        [XmlElement("dest")]
        public Dest Dest { get; set; }

        [XmlElement("vPrest")]
        public VPrest VPrest { get; set; }

        [XmlElement("imp")]
        public Imp Imp { get; set; }

        [XmlElement("infCTeNorm")]
        public InfCTeNorm InfCTeNorm { get; set; }

        [XmlElement("infCteComp")]
        public InfCteComp InfCteComp { get; set; }

        [XmlElement("infCteAnu")]
        public InfCteAnu InfCteAnu { get; set; }

        [XmlElement("autXML")]
        public List<AutXML> AutXML { get; set; }

        [XmlElement("infRespTec")]
        public InfRespTec InfRespTec { get; set; }

        [XmlElement("infSolicNFF")]
        public InfSolicNFF InfSolicNFF { get; set; }

        [XmlAttribute(AttributeName = "Id", DataType = "ID")]
        public string Id
        {
            get
            {
                IdField = "CTe" + Chave;
                return IdField;
            }
            set => IdField = value;
        }

        private string ChaveField;

        [XmlIgnore]
        public string Chave
        {
            get
            {
                ChaveField = ((int)Ide.CUF).ToString() +
                    Ide.DhEmi.ToString("yyMM") +
                    Emit.CNPJ.PadLeft(14, '0') +
                    ((int)Ide.Mod).ToString().PadLeft(2, '0') +
                    Ide.Serie.ToString().PadLeft(3, '0') +
                    Ide.NCT.ToString().PadLeft(9, '0') +
                    ((int)Ide.TpEmis).ToString() +
                    Ide.CCT.PadLeft(8, '0');

                Ide.CDV = Utility.XMLUtility.CalcularDVChave(ChaveField);

                ChaveField += Ide.CDV.ToString();

                return ChaveField;
            }
            set => throw new Exception("Não é permitido atribuir valor para a propriedade Chave. Ela é calculada automaticamente.");
        }

        #region Add (List - Interop)

        public void AddAutXML(AutXML autxml)
        {
            if(AutXML == null)
            {
                AutXML = new List<AutXML>();
            }

            AutXML.Add(autxml);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Ide
    {
        private string CCTField;
        private TipoEmissao TpEmisField;
        private ProcessoEmissao ProcEmiField;

        [XmlIgnore]
        public UFBrasil CUF { get; set; }

        [XmlElement("cUF")]
        public int CUFField
        {
            get => (int)CUF;
            set => CUF = (UFBrasil)Enum.Parse(typeof(UFBrasil), value.ToString());
        }

        [XmlElement("cCT")]
        public string CCT
        {
            get
            {
                string retorno;
                if(string.IsNullOrWhiteSpace(CCTField))
                {
                    if(NCT == 0)
                    {
                        throw new Exception("Defina antes o conteudo da TAG <nCT>, pois o mesmo é utilizado como base para calcular o código numérico.");
                    }

                    retorno = Utility.XMLUtility.GerarCodigoNumerico(NCT).ToString("00000000");
                }
                else
                {
                    retorno = CCTField;
                }

                return retorno;
            }
            set => CCTField = value;
        }

        [XmlElement("CFOP")]
        public string CFOP { get; set; }

        [XmlElement("natOp")]
        public string NatOp { get; set; }

        [XmlElement("mod")]
        public ModeloDFe Mod { get; set; }

        [XmlElement("serie")]
        public int Serie { get; set; }

        [XmlElement("nCT")]
        public int NCT { get; set; }

        [XmlIgnore]
        public DateTime DhEmi { get; set; }

        [XmlElement("dhEmi")]
        public string DhEmiField
        {
            get => DhEmi.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhEmi = DateTime.Parse(value);
        }

        [XmlElement("tpImp")]
        public FormatoImpressaoDACTE TpImp { get; set; }

        [XmlElement("tpEmis")]
        public TipoEmissao TpEmis
        {
            get => TpEmisField;
            set
            {
                if(value == TipoEmissao.ContingenciaFSIA ||
                   value == TipoEmissao.ContingenciaOffLine ||
                   value == TipoEmissao.RegimeEspecialNFF ||
                   value == TipoEmissao.ContingenciaSVCAN)
                {
                    throw new Exception("Conteúdo da tag <tpEmis> inválido! Valores aceitos: 1, 4, 5, 7 ou 8.");
                }

                TpEmisField = value;
            }
        }

        [XmlElement("cDV")]
        public int CDV { get; set; }

        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("tpCTe")]
        public TipoCTe TpCTe { get; set; }

        [XmlElement("procEmi")]
        public ProcessoEmissao ProcEmi
        {
            get => ProcEmiField;
            set
            {
                if(value == ProcessoEmissao.AvulsaPeloContribuinteSiteFisco ||
                    value == ProcessoEmissao.AvulsaPeloFisco)
                {
                    throw new Exception("Conteúdo da tag <procEmi> inválido! Valores aceitos: 0 e 3.");
                }

                ProcEmiField = value;
            }
        }

        [XmlElement("verProc")]
        public string VerProc { get; set; }

        [XmlElement("indGlobalizado")]
        public SimNao IndGlobalizado { get; set; }

        [XmlElement("cMunEnv")]
        public string CMunEnv { get; set; }

        [XmlElement("xMunEnv")]
        public string XMunEnv { get; set; }

        [XmlElement("UFEnv")]
        public UFBrasil UFEnv { get; set; }

        [XmlElement("modal")]
        public ModalidadeTransporteCTe Modal { get; set; }

        [XmlElement("tpServ")]
        public TipoServicoCTe TpServ { get; set; }

        [XmlElement("cMunIni")]
        public string CMunIni { get; set; }

        [XmlElement("xMunIni")]
        public string XMunIni { get; set; }

        [XmlElement("UFIni")]
        public UFBrasil UFIni { get; set; }

        [XmlElement("cMunFim")]
        public string CMunFim { get; set; }

        [XmlElement("xMunFim")]
        public string XMunFim { get; set; }

        [XmlElement("UFFim")]
        public UFBrasil UFFim { get; set; }

        [XmlElement("retira")]
        public string RetiraField { get; set; }

        [XmlIgnore]
        public SimNao Retira
        {
            get => (RetiraField.Equals("0") ? SimNao.Sim : SimNao.Nao);
            set => RetiraField = (value == SimNao.Sim ? "0" : "1");
        }

        [XmlElement("xDetRetira")]
        public string XDetRetira { get; set; }

        [XmlElement("indIEToma")]
        public IndicadorIEDestinatario IndIEToma { get; set; }

        [XmlElement("toma3")]
        public Toma3 Toma3 { get; set; }

        [XmlElement("toma4")]
        public Toma4 Toma4 { get; set; }

        [XmlIgnore]
        public DateTime DhCont { get; set; }

        [XmlElement("dhCont")]
        public string DhContField
        {
            get => DhCont.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => DhCont = DateTime.Parse(value);
        }

        [XmlElement("xJust")]
        public string XJust { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeIndGlobalizado() => IndGlobalizado == SimNao.Sim;

        public bool ShouldSerializeDhContField() => DhCont > DateTime.MinValue;

        public bool ShouldSerializeXJust() => !string.IsNullOrWhiteSpace(XJust);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Toma3
    {
        private TomadorServicoCTe TomaField;

        [XmlElement("toma")]
        public TomadorServicoCTe Toma
        {
            get => TomaField;
            set
            {
                if(value == TomadorServicoCTe.Outros)
                {
                    throw new Exception("Conteúdo da tag <toma> filha da tag <toma3> inválido! Valores aceitos: 0, 1, 2, 3.");
                }

                TomaField = value;
            }
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Toma4
    {
        //private TomadorServicoCTe TomaField;

        [XmlElement("toma")]
        public TomadorServicoCTe Toma
        {
            get => TomadorServicoCTe.Outros;
            set
            {
                if(value != TomadorServicoCTe.Outros)
                {
                    throw new Exception("Conteúdo da tag <toma> filha da tag <toma4> inválido! Valores aceitos: 4.");
                }

                //TomaField = value;
            }
        }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("xFant")]
        public string XFant { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("enderToma")]
        public EnderToma EnderToma { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        public bool ShouldSerializeFone() => !string.IsNullOrWhiteSpace(Fone);

        public bool ShouldSerializeXFant() => !string.IsNullOrWhiteSpace(XFant);

        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        public bool ShouldSerializeEmail() => !string.IsNullOrWhiteSpace(Email);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EnderToma
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

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        #region ShouldSerialize

        public bool ShouldSerializeCPais() => CPais > 0;

        public bool ShouldSerializeXPais() => !string.IsNullOrWhiteSpace(XPais);

        public bool ShouldSerializeXCpl() => !string.IsNullOrWhiteSpace(XCpl);

        public bool ShouldSerializeCEP() => !string.IsNullOrWhiteSpace(CEP);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Compl
    {
        [XmlElement("xCaracAd")]
        public string XCaracAd { get; set; }

        [XmlElement("xCaracSer")]
        public string XCaracSer { get; set; }

        [XmlElement("xEmi")]
        public string XEmi { get; set; }

        [XmlElement("fluxo")]
        public Fluxo Fluxo { get; set; }

        [XmlElement("Entrega")]
        public Entrega Entrega { get; set; }

        [XmlElement("origCalc")]
        public string OrigCalc { get; set; }

        [XmlElement("destCalc")]
        public string DestCalc { get; set; }

        [XmlElement("xObs")]
        public string XObs { get; set; }

        [XmlElement("ObsCont")]
        public List<ObsCont> ObsCont { get; set; }

        [XmlElement("ObsFisco")]
        public List<ObsFisco> ObsFisco { get; set; }

        #region Add (List - Interop)

        public void AddObsCont(ObsCont obsCont)
        {
            if(ObsCont == null)
            {
                ObsCont = new List<ObsCont>();
            }

            ObsCont.Add(obsCont);
        }

        public void AddObsFisco(ObsFisco obsFisco)
        {
            if(ObsFisco == null)
            {
                ObsFisco = new List<ObsFisco>();
            }

            ObsFisco.Add(obsFisco);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeXCaracAd() => !string.IsNullOrWhiteSpace(XCaracAd);

        public bool ShouldSerializeXCaracSer() => !string.IsNullOrWhiteSpace(XCaracSer);

        public bool ShouldSerializeXEmi() => !string.IsNullOrWhiteSpace(XEmi);

        public bool ShouldSerializeOrigCalc() => !string.IsNullOrWhiteSpace(OrigCalc);

        public bool ShouldSerializeDestCalc() => !string.IsNullOrWhiteSpace(DestCalc);

        public bool ShouldSerializeXObs() => !string.IsNullOrWhiteSpace(XObs);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Fluxo
    {
        [XmlElement("xOrig")]
        public string XOrig { get; set; }

        [XmlElement("pass")]
        public List<Pass> Pass { get; set; }

        [XmlElement("xDest")]
        public string XDest { get; set; }

        [XmlElement("xRota")]
        public string XRota { get; set; }

        #region Add (List - Interop)

        public void AddPass(Pass pass)
        {
            if(Pass == null)
            {
                Pass = new List<Pass>();
            }

            Pass.Add(pass);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeXOrig() => !string.IsNullOrWhiteSpace(XOrig);
        public bool ShouldSerializeXDest() => !string.IsNullOrWhiteSpace(XDest);
        public bool ShouldSerializeXRota() => !string.IsNullOrWhiteSpace(XRota);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Pass
    {
        [XmlElement("xPass")]
        public string XPass { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeXPass() => !string.IsNullOrWhiteSpace(XPass);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Entrega
    {
        [XmlElement("semData")]
        public SemData SemData { get; set; }

        [XmlElement("comData")]
        public ComData ComData { get; set; }

        [XmlElement("noPeriodo")]
        public NoPeriodo NoPeriodo { get; set; }

        [XmlElement("semHora")]
        public SemHora SemHora { get; set; }

        [XmlElement("comHora")]
        public ComHora ComHora { get; set; }

        [XmlElement("noInter")]
        public NoInter NoInter { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class SemData
    {
        //private TipoPeriodoEntregaCTe TpPerField;

        [XmlElement("tpPer")]
        public TipoPeriodoEntregaCTe TpPer
        {
            get => TipoPeriodoEntregaCTe.SemDataDefinida;
            set
            {
                if(value != TipoPeriodoEntregaCTe.SemDataDefinida)
                {
                    throw new Exception("Conteúdo da tag <tpPer> filha da tag <semData><Entrega> inválido! Valores aceitos: 0.");
                }

                //TpPerField = value;
            }
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ComData
    {
        private TipoPeriodoEntregaCTe TpPerField;

        [XmlElement("tpPer")]
        public TipoPeriodoEntregaCTe TpPer
        {
            get => TpPerField;
            set
            {
                if(value == TipoPeriodoEntregaCTe.SemDataDefinida || value == TipoPeriodoEntregaCTe.NoPeriodo)
                {
                    throw new Exception("Conteúdo da tag <tpPer> filha da tag <comData><Entrega> inválido! Valores aceitos: 1, 2 ou 3.");
                }

                TpPerField = value;
            }
        }

        [XmlIgnore]
        public DateTime DProg { get; set; }

        [XmlElement("dProg")]
        public string DProgField
        {
            get => DProg.ToString("yyyy-MM-dd");
            set => DProg = DateTime.Parse(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class NoPeriodo
    {
        //private TipoPeriodoEntregaCTe TpPerField;

        [XmlElement("tpPer")]
        public TipoPeriodoEntregaCTe TpPer
        {
            get => TipoPeriodoEntregaCTe.NoPeriodo;
            set
            {
                if(value != TipoPeriodoEntregaCTe.NoPeriodo)
                {
                    throw new Exception("Conteúdo da tag <tpPer> filha da tag <noPeriodo><Entrega> inválido! Valores aceitos: 4.");
                }

                //TpPerField = value;
            }
        }

        [XmlIgnore]
        public DateTime DIni { get; set; }

        [XmlElement("dIni")]
        public string DIniField
        {
            get => DIni.ToString("yyyy-MM-dd");
            set => DIni = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime DFim { get; set; }

        [XmlElement("dFim")]
        public string DFimField
        {
            get => DFim.ToString("yyyy-MM-dd");
            set => DFim = DateTime.Parse(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class SemHora
    {
        //private TipoHoraEntregaCTe TpHorField;

        [XmlElement("tpHor")]
        public TipoHoraEntregaCTe TpHor
        {
            get => TipoHoraEntregaCTe.SemHoraDefinida;
            set
            {
                if(value != TipoHoraEntregaCTe.SemHoraDefinida)
                {
                    throw new Exception("Conteúdo da tag <tpHor> filha da tag <semHora><Entrega> inválido! Valores aceitos: 0.");
                }

                //TpHorField = value;
            }
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ComHora
    {
        private TipoHoraEntregaCTe TpHorField;

        [XmlElement("tpHor")]
        public TipoHoraEntregaCTe TpHor
        {
            get => TpHorField;
            set
            {
                if(value == TipoHoraEntregaCTe.SemHoraDefinida || value == TipoHoraEntregaCTe.NoIntervaloTempo)
                {
                    throw new Exception("Conteúdo da tag <tpHor> filha da tag <comHora><Entrega> inválido! Valores aceitos: 1, 2 ou 3.");
                }

                TpHorField = value;
            }
        }

        [XmlIgnore]
        public DateTime HProg { get; set; }

        [XmlElement("hProg")]
        public string HProgField
        {
            get => HProg.ToString("HH:mm:ss");
            set => HProg = DateTime.Parse(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class NoInter
    {
        //private TipoHoraEntregaCTe TpHorField;

        [XmlElement("tpHor")]
        public TipoHoraEntregaCTe TpHor
        {
            get => TipoHoraEntregaCTe.NoIntervaloTempo;
            set
            {
                if(value != TipoHoraEntregaCTe.NoIntervaloTempo)
                {
                    throw new Exception("Conteúdo da tag <tpHor> filha da tag <noInter><Entrega> inválido! Valores aceitos: 4.");
                }

                //TpHorField = value;
            }
        }

        [XmlIgnore]
        public DateTime HIni { get; set; }

        [XmlElement("hIni")]
        public string HIniField
        {
            get => HIni.ToString("HH:mm:ss");
            set => HIni = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime HFim { get; set; }

        [XmlElement("hFim")]
        public string HFimField
        {
            get => HFim.ToString("HH:mm:ss");
            set => HFim = DateTime.Parse(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ObsCont
    {
        [XmlElement("xTexto")]
        public string XTexto { get; set; }

        [XmlAttribute(AttributeName = "xCampo")]
        public string XCampo { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ObsFisco
    {
        [XmlElement("xTexto")]
        public string XTexto { get; set; }

        [XmlAttribute(AttributeName = "xCampo")]
        public string XCampo { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Emit
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("IEST")]
        public string IEST { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("xFant")]
        public string XFant { get; set; }

        [XmlElement("enderEmit")]
        public EnderEmit EnderEmit { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeIEST() => !string.IsNullOrWhiteSpace(IEST);

        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        public bool ShouldSerializeXFant() => !string.IsNullOrWhiteSpace(XFant);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
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

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        #region ShouldSerialize               

        public bool ShouldSerializeXCpl() => !string.IsNullOrWhiteSpace(XCpl);

        public bool ShouldSerializeCEP() => !string.IsNullOrWhiteSpace(CEP);

        public bool ShouldSerializeFone() => !string.IsNullOrWhiteSpace(Fone);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Rem
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("xFant")]
        public string XFant { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("enderReme")]
        public EnderReme EnderReme { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        public bool ShouldSerializeXFant() => !string.IsNullOrWhiteSpace(XFant);

        public bool ShouldSerializeFone() => !string.IsNullOrWhiteSpace(Fone);

        public bool ShouldSerializeEmail() => !string.IsNullOrWhiteSpace(Email);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EnderReme
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

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        #region ShouldSerialize 

        public bool ShouldSerializeCPais() => CPais > 0;

        public bool ShouldSerializeXPais() => !string.IsNullOrWhiteSpace(XPais);

        public bool ShouldSerializeXCpl() => !string.IsNullOrWhiteSpace(XCpl);

        public bool ShouldSerializeCEP() => !string.IsNullOrWhiteSpace(CEP);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Exped
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("enderExped")]
        public EnderExped EnderExped { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        public bool ShouldSerializeFone() => !string.IsNullOrWhiteSpace(Fone);

        public bool ShouldSerializeEmail() => !string.IsNullOrWhiteSpace(Email);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EnderExped
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

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        #region ShouldSerialize 

        public bool ShouldSerializeCPais() => CPais > 0;

        public bool ShouldSerializeXPais() => !string.IsNullOrWhiteSpace(XPais);

        public bool ShouldSerializeXCpl() => !string.IsNullOrWhiteSpace(XCpl);

        public bool ShouldSerializeCEP() => !string.IsNullOrWhiteSpace(CEP);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Receb
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("enderReceb")]
        public EnderReceb EnderReceb { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        public bool ShouldSerializeFone() => !string.IsNullOrWhiteSpace(Fone);

        public bool ShouldSerializeEmail() => !string.IsNullOrWhiteSpace(Email);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EnderReceb
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

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        #region ShouldSerialize 

        public bool ShouldSerializeCPais() => CPais > 0;

        public bool ShouldSerializeXPais() => !string.IsNullOrWhiteSpace(XPais);

        public bool ShouldSerializeXCpl() => !string.IsNullOrWhiteSpace(XCpl);

        public bool ShouldSerializeCEP() => !string.IsNullOrWhiteSpace(CEP);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Dest
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        [XmlElement("ISUF")]
        public string ISUF { get; set; }

        [XmlElement("enderDest")]
        public EnderDest EnderDest { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        public bool ShouldSerializeFone() => !string.IsNullOrWhiteSpace(Fone);

        public bool ShouldSerializeEmail() => !string.IsNullOrWhiteSpace(Email);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
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

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;

        [XmlElement("xPais")]
        public string XPais { get; set; } = "BRASIL";

        #region ShouldSerialize 

        public bool ShouldSerializeCPais() => CPais > 0;

        public bool ShouldSerializeXPais() => !string.IsNullOrWhiteSpace(XPais);

        public bool ShouldSerializeXCpl() => !string.IsNullOrWhiteSpace(XCpl);

        public bool ShouldSerializeCEP() => !string.IsNullOrWhiteSpace(CEP);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class VPrest
    {
        [XmlIgnore]
        public double VTPrest { get; set; }

        [XmlElement("vTPrest")]
        public string VTPrestField
        {
            get => VTPrest.ToString("F2", CultureInfo.InvariantCulture);
            set => VTPrest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VRec { get; set; }

        [XmlElement("vRec")]
        public string VRecField
        {
            get => VRec.ToString("F2", CultureInfo.InvariantCulture);
            set => VRec = Utility.Converter.ToDouble(value);
        }

        [XmlElement("Comp")]
        public List<Comp> Comp { get; set; }

        #region Add (List - Interop)

        public void AddComp(Comp comp)
        {
            if(Comp == null)
            {
                Comp = new List<Comp>();
            }

            Comp.Add(comp);
        }

        #endregion

    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Comp
    {
        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlIgnore]
        public double VComp { get; set; }

        [XmlElement("vComp")]
        public string VCompField
        {
            get => VComp.ToString("F2", CultureInfo.InvariantCulture);
            set => VComp = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Imp
    {
        [XmlElement("ICMS")]
        public ICMS ICMS { get; set; }

        [XmlIgnore]
        public double VTotTrib { get; set; }

        [XmlElement("vTotTrib")]
        public string VTotTribField
        {
            get => VTotTrib.ToString("F2", CultureInfo.InvariantCulture);
            set => VTotTrib = Utility.Converter.ToDouble(value);
        }

        [XmlElement("infAdFisco")]
        public string InfAdFisco { get; set; }

        [XmlElement("ICMSUFFim")]
        public ICMSUFFim ICMSUFFim { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeVTotTribField() => VTotTrib > 0;

        public bool ShouldSerializeInfAdFisco() => !string.IsNullOrWhiteSpace(InfAdFisco);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMS
    {
        [XmlElement("ICMS00")]
        public ICMS00 ICMS00 { get; set; }

        [XmlElement("ICMS20")]
        public ICMS20 ICMS20 { get; set; }

        [XmlElement("ICMS45")]
        public ICMS45 ICMS45 { get; set; }

        [XmlElement("ICMS60")]
        public ICMS60 ICMS60 { get; set; }

        [XmlElement("ICMS90")]
        public ICMS90 ICMS90 { get; set; }

        [XmlElement("ICMSOutraUF")]
        public ICMSOutraUF ICMSOutraUF { get; set; }

        [XmlElement("ICMSSN")]
        public ICMSSN ICMSSN { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMS00
    {
        [XmlElement("CST")]
        public string CST { get; set; } = "00";

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
            get => PICMS.ToString("F2", CultureInfo.InvariantCulture);
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
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMS20
    {
        [XmlElement("CST")]
        public string CST { get; set; } = "20";

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
            get => PICMS.ToString("F2", CultureInfo.InvariantCulture);
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
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMS45
    {
        private string CSTField;

        [XmlElement("CST")]
        public string CST
        {
            get => CSTField;
            set
            {
                if(value.Equals("40") || value.Equals("41") || value.Equals("51"))
                {
                    CSTField = value;
                }
                else
                {
                    throw new Exception("Conteúdo da TAG <CST> da <ICMS45> inválido! Valores aceitos: 40, 41 ou 51.");
                }
            }
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMS60
    {
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
        public double VICMSSTRet { get; set; }

        [XmlElement("vICMSSTRet")]
        public string VICMSSTRetField
        {
            get => VICMSSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSSTRet { get; set; }

        [XmlElement("pICMSSTRet")]
        public string PICMSSTRetField
        {
            get => PICMSSTRet.ToString("F2", CultureInfo.InvariantCulture);
            set => PICMSSTRet = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VCred { get; set; }

        [XmlElement("vCred")]
        public string VCredField
        {
            get => VCred.ToString("F2", CultureInfo.InvariantCulture);
            set => VCred = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVCredField() => VCred > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMS90
    {
        [XmlElement("CST")]
        public string CST { get; set; } = "90";

        [XmlIgnore]
        public double PRedBC { get; set; }

        [XmlElement("pRedBC")]
        public string PRedBCField
        {
            get => PRedBC.ToString("F2", CultureInfo.InvariantCulture);
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
            get => PICMS.ToString("F2", CultureInfo.InvariantCulture);
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
        public double VCred { get; set; }

        [XmlElement("vCred")]
        public string VCredField
        {
            get => VCred.ToString("F2", CultureInfo.InvariantCulture);
            set => VCred = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializePRedBCField() => PRedBC > 0;

        public bool ShouldSerializeVCredField() => VCred > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMSOutraUF
    {
        [XmlElement("CST")]
        public string CST { get; set; } = "90";

        [XmlIgnore]
        public double PRedBCOutraUF { get; set; }

        [XmlElement("pRedBCOutraUF")]
        public string PRedBCOutraUFField
        {
            get => PRedBCOutraUF.ToString("F2", CultureInfo.InvariantCulture);
            set => PRedBCOutraUF = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VBCOutraUF { get; set; }

        [XmlElement("vBCOutraUF")]
        public string VBCOutraUFField
        {
            get => VBCOutraUF.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCOutraUF = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSOutraUF { get; set; }

        [XmlElement("pICMSOutraUF")]
        public string PICMSOutraUFField
        {
            get => PICMSOutraUF.ToString("F2", CultureInfo.InvariantCulture);
            set => PICMSOutraUF = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSOutraUF { get; set; }

        [XmlElement("vICMSOutraUF")]
        public string VICMSOutraUFField
        {
            get => VICMSOutraUF.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSOutraUF = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializePRedBCOutraUFField() => PRedBCOutraUF > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMSSN
    {
        [XmlElement("CST")]
        public string CST { get; set; } = "90";

        [XmlElement("indSN")]
        public SimNao IndSN { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class ICMSUFFim
    {
        [XmlIgnore]
        public double VBCUFFim { get; set; }

        [XmlElement("vBCUFFim")]
        public string VBCUFFimField
        {
            get => VBCUFFim.ToString("F2", CultureInfo.InvariantCulture);
            set => VBCUFFim = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PFCPUFFim { get; set; }

        [XmlElement("pFCPUFFim")]
        public string PFCPUFFimField
        {
            get => PFCPUFFim.ToString("F2", CultureInfo.InvariantCulture);
            set => PFCPUFFim = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSUFFim { get; set; }

        [XmlElement("pICMSUFFim")]
        public string PICMSUFFimField
        {
            get => PICMSUFFim.ToString("F2", CultureInfo.InvariantCulture);
            set => PICMSUFFim = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double PICMSInter { get; set; }

        [XmlElement("pICMSInter")]
        public string PICMSInterField
        {
            get => PICMSInter.ToString("F2", CultureInfo.InvariantCulture);
            set => PICMSInter = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFCPUFFim { get; set; }

        [XmlElement("vFCPUFFim")]
        public string VFCPUFFimField
        {
            get => VFCPUFFim.ToString("F2", CultureInfo.InvariantCulture);
            set => VFCPUFFim = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSUFFim { get; set; }

        [XmlElement("vICMSUFFim")]
        public string VICMSUFFimField
        {
            get => VICMSUFFim.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSUFFim = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VICMSUFIni { get; set; }

        [XmlElement("vICMSUFIni")]
        public string VICMSUFIniField
        {
            get => VICMSUFIni.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMSUFIni = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCTeNorm
    {
        [XmlElement("infCarga")]
        public InfCarga InfCarga { get; set; }

        [XmlElement("infDoc")]
        public InfDoc InfDoc { get; set; }

        [XmlElement("docAnt")]
        public DocAnt DocAnt { get; set; }

        [XmlElement("infModal")]
        public InfModal InfModal { get; set; }

        [XmlElement("veicNovos")]
        public List<VeicNovos> VeicNovos { get; set; }

        [XmlElement("cobr")]
        public Cobr Cobr { get; set; }

        [XmlElement("infCteSub")]
        public InfCteSub InfCteSub { get; set; }

        [XmlElement("infGlobalizado")]
        public InfGlobalizado InfGlobalizado { get; set; }

        [XmlElement("infServVinc")]
        public InfServVinc InfServVinc { get; set; }

        #region Add (List - Interop)

        public void AddVeicNovos(VeicNovos veicNovos)
        {
            if(VeicNovos == null)
            {
                VeicNovos = new List<VeicNovos>();
            }

            VeicNovos.Add(veicNovos);
        }

        #endregion


        #region ShouldSerialize

        //public bool ShouldSerialize() => !string.IsNullOrWhiteSpace();

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCarga
    {
        [XmlIgnore]
        public double VCarga { get; set; }

        [XmlElement("vCarga")]
        public string VCargaField
        {
            get => VCarga.ToString("F2", CultureInfo.InvariantCulture);
            set => VCarga = Utility.Converter.ToDouble(value);
        }

        [XmlElement("proPred")]
        public string ProPred { get; set; }

        [XmlElement("xOutCat")]
        public string XOutCat { get; set; }

        [XmlElement("infQ")]
        public List<InfQ> InfQ { get; set; }

        [XmlIgnore]
        public double VCargaAverb { get; set; }

        [XmlElement("vCargaAverb")]
        public string VCargaAverbField
        {
            get => VCargaAverb.ToString("F2", CultureInfo.InvariantCulture);
            set => VCargaAverb = Utility.Converter.ToDouble(value);
        }

        #region Add (List - Interop)

        public void AddInfQ(InfQ infq)
        {
            if(InfQ == null)
            {
                InfQ = new List<InfQ>();
            }

            InfQ.Add(infq);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeVCargaField() => VCarga > 0;
        public bool ShouldSerializeXOutCat() => !string.IsNullOrWhiteSpace(XOutCat);
        public bool ShouldSerializeVCargaAverbField() => VCargaAverb > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfQ
    {
        [XmlElement("cUnid")]
        public CodigoUnidadeMedidaCTe CUnid { get; set; }

        [XmlElement("tpMed")]
        public string TpMed { get; set; }

        [XmlIgnore]
        public double QCarga { get; set; }

        [XmlElement("qCarga")]
        public string QCargaField
        {
            get => QCarga.ToString("F4", CultureInfo.InvariantCulture);
            set => QCarga = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfDoc
    {
        [XmlElement("infNF")]
        public List<InfNF> InfNF { get; set; }

        [XmlElement("infNFe")]
        public List<InfNFe> InfNFe { get; set; }

        [XmlElement("infOutros")]
        public List<InfOutros> InfOutros { get; set; }

        #region Add (List - Interop)

        public void AddInfNF(InfNF infnf)
        {
            if(InfNF == null)
            {
                InfNF = new List<InfNF>();
            }

            InfNF.Add(infnf);
        }

        public void AddInfNFe(InfNFe infnfe)
        {
            if(InfNFe == null)
            {
                InfNFe = new List<InfNFe>();
            }

            InfNFe.Add(infnfe);
        }

        public void AddInfOutros(InfOutros infoutros)
        {
            if(InfOutros == null)
            {
                InfOutros = new List<InfOutros>();
            }

            InfOutros.Add(infoutros);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfNF
    {
        [XmlElement("nRoma")]
        public string NRoma { get; set; }

        [XmlElement("nPed")]
        public string NPed { get; set; }

        [XmlElement("mod")]
        public ModeloNF Mod { get; set; }

        [XmlElement("serie")]
        public string Serie { get; set; }

        [XmlElement("nDoc")]
        public string NDoc { get; set; }

        [XmlIgnore]
        public DateTime DEmi { get; set; }

        [XmlElement("dEmi")]
        public string DEmiField
        {
            get => DEmi.ToString("yyyy-MM-dd");
            set => DEmi = DateTime.Parse(value);
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
        public double VICMS { get; set; }

        [XmlElement("vICMS")]
        public string VICMSField
        {
            get => VICMS.ToString("F2", CultureInfo.InvariantCulture);
            set => VICMS = Utility.Converter.ToDouble(value);
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
        public double VProd { get; set; }

        [XmlElement("vProd")]
        public string VProdField
        {
            get => VProd.ToString("F2", CultureInfo.InvariantCulture);
            set => VProd = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VNF { get; set; }

        [XmlElement("vNF")]
        public string VNFField
        {
            get => VNF.ToString("F2", CultureInfo.InvariantCulture);
            set => VNF = Utility.Converter.ToDouble(value);
        }

        [XmlElement("nCFOP")]
        public string NCFOP { get; set; }

        [XmlIgnore]
        public double NPeso { get; set; }

        [XmlElement("nPeso")]
        public string NPesoField
        {
            get => NPeso.ToString("F3", CultureInfo.InvariantCulture);
            set => NPeso = Utility.Converter.ToDouble(value);
        }

        [XmlElement("PIN")]
        public string PIN { get; set; }

        [XmlIgnore]
        public DateTime DPrev { get; set; }

        [XmlElement("dPrev")]
        public string DPrevField
        {
            get => DPrev.ToString("yyyy-MM-dd");
            set => DPrev = DateTime.Parse(value);
        }

        [XmlElement("infUnidCarga")]
        public List<InfUnidCarga> InfUnidCarga { get; set; }

        [XmlElement("infUnidTransp")]
        public List<InfUnidTransp> InfUnidTransp { get; set; }

        #region Add (List - Interop)

        public void AddInfUnidCarga(InfUnidCarga infUnidCarga)
        {
            if(InfUnidCarga == null)
            {
                InfUnidCarga = new List<InfUnidCarga>();
            }

            InfUnidCarga.Add(infUnidCarga);
        }

        public void AddInfUnidTransp(InfUnidTransp infUnidTransp)
        {
            if(InfUnidTransp == null)
            {
                InfUnidTransp = new List<InfUnidTransp>();
            }

            InfUnidTransp.Add(infUnidTransp);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeNRoma() => !string.IsNullOrWhiteSpace(NRoma);
        public bool ShouldSerializeNPed() => !string.IsNullOrWhiteSpace(NPed);
        public bool ShouldSerializeNPesoField() => NPeso > 0;
        public bool ShouldSerializePIN() => !string.IsNullOrWhiteSpace(PIN);
        public bool ShouldSerializeDPrevField() => DPrev > DateTime.MinValue;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfUnidCarga
    {
        [XmlElement("tpUnidCarga")]
        public virtual TipoUnidadeCarga TpUnidCarga { get; set; }

        [XmlElement("idUnidCarga")]
        public string IdUnidCarga { get; set; }

        [XmlElement("lacUnidCarga")]
        public List<LacUnidCarga> LacUnidCarga { get; set; }

        [XmlIgnore]
        public double QtdRat { get; set; }

        [XmlElement("qtdRat")]
        public string QtdRatField
        {
            get => QtdRat.ToString("F2", CultureInfo.InvariantCulture);
            set => QtdRat = Utility.Converter.ToDouble(value);
        }

        #region Add (List - Interop)

        public void AddLacUnidCarga(LacUnidCarga lacUnidCarga)
        {
            if(LacUnidCarga == null)
            {
                LacUnidCarga = new List<LacUnidCarga>();
            }

            LacUnidCarga.Add(lacUnidCarga);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeQtdRatField() => QtdRat > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class LacUnidCarga
    {
        [XmlElement("nLacre")]
        public string NLacre { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfUnidTransp
    {
        [XmlElement("tpUnidTransp")]
        public virtual TipoUnidadeTransporte TpUnidTransp { get; set; }

        [XmlElement("idUnidTransp")]
        public string IdUnidTransp { get; set; }

        [XmlElement("lacUnidTransp")]
        public List<LacUnidTransp> LacUnidTransp { get; set; }

        [XmlElement("infUnidCarga")]
        public List<InfUnidCarga> InfUnidCarga { get; set; }

        [XmlIgnore]
        public double QtdRat { get; set; }

        [XmlElement("qtdRat")]
        public string QtdRatField
        {
            get => QtdRat.ToString("F2", CultureInfo.InvariantCulture);
            set => QtdRat = Utility.Converter.ToDouble(value);
        }

        #region Add (List - Interop)

        public void AddLacUnidTransp(LacUnidTransp lacUnidTransp)
        {
            if(LacUnidTransp == null)
            {
                LacUnidTransp = new List<LacUnidTransp>();
            }

            LacUnidTransp.Add(lacUnidTransp);
        }

        public void AddInfUnidCarga(InfUnidCarga infUnidCarga)
        {
            if(InfUnidCarga == null)
            {
                InfUnidCarga = new List<InfUnidCarga>();
            }

            InfUnidCarga.Add(infUnidCarga);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeQtdRatField() => QtdRat > 0;

        #endregion

    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class LacUnidTransp: LacUnidCarga { }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfNFe
    {
        [XmlElement("chave")]
        public string Chave { get; set; }

        [XmlElement("PIN")]
        public string PIN { get; set; }

        [XmlIgnore]
        public DateTime DPrev { get; set; }

        [XmlElement("dPrev")]
        public string DPrevField
        {
            get => DPrev.ToString("yyyy-MM-dd");
            set => DPrev = DateTime.Parse(value);
        }

        [XmlElement("infUnidCarga")]
        public List<InfUnidCarga> InfUnidCarga { get; set; }

        [XmlElement("infUnidTransp")]
        public List<InfUnidTransp> InfUnidTransp { get; set; }

        #region Add (List - Interop)

        public void AddInfUnidCarga(InfUnidCarga infUnidCarga)
        {
            if(InfUnidCarga == null)
            {
                InfUnidCarga = new List<InfUnidCarga>();
            }

            InfUnidCarga.Add(infUnidCarga);
        }

        public void AddInfUnidTransp(InfUnidTransp infUnidTransp)
        {
            if(InfUnidTransp == null)
            {
                InfUnidTransp = new List<InfUnidTransp>();
            }

            InfUnidTransp.Add(infUnidTransp);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializePIN() => !string.IsNullOrWhiteSpace(PIN);
        public bool ShouldSerializeDPrevField() => DPrev > DateTime.MinValue;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfOutros
    {
        [XmlElement("tpDoc")]
        public TipoDocumentoOriginarioCTe TpDoc { get; set; }

        [XmlElement("descOutros")]
        public string DescOutros { get; set; }

        [XmlElement("nDoc")]
        public string NDoc { get; set; }

        [XmlIgnore]
        public DateTime DEmi { get; set; }

        [XmlElement("dEmi")]
        public string DEmiField
        {
            get => DEmi.ToString("yyyy-MM-dd");
            set => DEmi = DateTime.Parse(value);
        }

        [XmlIgnore]
        public double VDocFisc { get; set; }

        [XmlElement("vDocFisc")]
        public string VDocFiscField
        {
            get => VDocFisc.ToString("F2", CultureInfo.InvariantCulture);
            set => VDocFisc = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public DateTime DPrev { get; set; }

        [XmlElement("dPrev")]
        public string DPrevField
        {
            get => DPrev.ToString("yyyy-MM-dd");
            set => DPrev = DateTime.Parse(value);
        }

        [XmlElement("infUnidCarga")]
        public List<InfUnidCarga> InfUnidCarga { get; set; }

        [XmlElement("infUnidTransp")]
        public List<InfUnidTransp> InfUnidTransp { get; set; }

        #region Add (List - Interop)

        public void AddInfUnidCarga(InfUnidCarga infUnidCarga)
        {
            if(InfUnidCarga == null)
            {
                InfUnidCarga = new List<InfUnidCarga>();
            }

            InfUnidCarga.Add(infUnidCarga);
        }

        public void AddInfUnidTransp(InfUnidTransp infUnidTransp)
        {
            if(InfUnidTransp == null)
            {
                InfUnidTransp = new List<InfUnidTransp>();
            }

            InfUnidTransp.Add(infUnidTransp);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeDescOutros() => !string.IsNullOrWhiteSpace(DescOutros);
        public bool ShouldSerializeNDoc() => !string.IsNullOrWhiteSpace(NDoc);
        public bool ShouldSerializeDEmiField() => DEmi > DateTime.MinValue;
        public bool ShouldSerializeVDocFiscField() => VDocFisc > 0;
        public bool ShouldSerializeDPrevField() => DPrev > DateTime.MinValue;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DocAnt
    {
        [XmlElement("emiDocAnt")]
        public List<EmiDocAnt> EmiDocAnt { get; set; }

        #region Add (List - Interop)

        public void AddEmiDocAnt(EmiDocAnt emiDocAnt)
        {
            if(EmiDocAnt == null)
            {
                EmiDocAnt = new List<EmiDocAnt>();
            }

            EmiDocAnt.Add(emiDocAnt);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EmiDocAnt
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("idDocAnt")]
        public List<IdDocAnt> IdDocAnt { get; set; }

        #region Add (List - Interop)

        public void AddIdDocAnt(IdDocAnt idDocAnt)
        {
            if(IdDocAnt == null)
            {
                IdDocAnt = new List<IdDocAnt>();
            }

            IdDocAnt.Add(idDocAnt);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);

        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class IdDocAnt
    {
        [XmlElement("idDocAntEle")]
        public List<IdDocAntEle> IdDocAntEle { get; set; }

        [XmlElement("idDocAntPap")]
        public List<IdDocAntPap> IdDocAntPap { get; set; }

        #region Add (List - Interop)

        public void AddIdDocAntEle(IdDocAntEle idDocAntEle)
        {
            if(IdDocAntEle == null)
            {
                IdDocAntEle = new List<IdDocAntEle>();
            }

            IdDocAntEle.Add(idDocAntEle);
        }

        public void AddIdDocAntPap(IdDocAntPap idDocAntPap)
        {
            if(IdDocAntPap == null)
            {
                IdDocAntPap = new List<IdDocAntPap>();
            }

            IdDocAntPap.Add(idDocAntPap);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class IdDocAntPap
    {
        [XmlElement("tpDoc")]
        public TipoDocumentoTransporteAnteriorCTe TpDoc { get; set; }

        [XmlElement("serie")]
        public string Serie { get; set; }

        [XmlElement("subser")]
        public string Subser { get; set; }

        [XmlElement("nDoc")]
        public string NDoc { get; set; }

        [XmlIgnore]
        public DateTime DEmi { get; set; }

        [XmlElement("dEmi")]
        public string DEmiField
        {
            get => DEmi.ToString("yyyy-MM-dd");
            set => DEmi = DateTime.Parse(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeSubser() => !string.IsNullOrWhiteSpace(Subser);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class IdDocAntEle
    {
        [XmlElement("chCTe")]
        public string ChCTe { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfModal
    {
        [XmlAttribute(AttributeName = "versaoModal", DataType = "token")]
        public string VersaoModal { get; set; }

        [XmlElement("rodo")]
        public Rodo Rodo { get; set; }

        [XmlElement("multimodal")]
        public MultiModal MultiModal { get; set; }

        [XmlElement("duto")]
        public Duto Duto { get; set; }

        [XmlElement("aereo")]
        public Aereo Aereo { get; set; }

        [XmlElement("aquav")]
        public Aquav Aquav { get; set; }

        [XmlElement("ferrov")]
        public Ferrov Ferrov { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Rodo
    {
        [XmlElement("RNTRC")]
        public string RNTRC { get; set; }

        [XmlElement("occ")]
        public List<Occ> Occ { get; set; }

        #region Add (List - Interop)

        public void AddOcc(Occ occ)
        {
            if(Occ == null)
            {
                Occ = new List<Occ>();
            }

            Occ.Add(occ);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Occ
    {
        [XmlElement("serie")]
        public string Serie { get; set; }

        [XmlElement("nOcc")]
        public int NOcc { get; set; }

        [XmlIgnore]
        public DateTime DEmi { get; set; }

        [XmlElement("dEmi")]
        public string DEmiField
        {
            get => DEmi.ToString("yyyy-MM-dd");
            set => DEmi = DateTime.Parse(value);
        }

        [XmlElement("emiOcc")]
        public EmiOcc EmiOcc { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeSerie() => !string.IsNullOrWhiteSpace(Serie);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EmiOcc
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("cInt")]
        public string CInt { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        [XmlElement("fone")]
        public string Fone { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCInt() => !string.IsNullOrWhiteSpace(CInt);
        public bool ShouldSerializeFone() => !string.IsNullOrWhiteSpace(Fone);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class MultiModal
    {
        [XmlElement("COTM")]
        public string COTM { get; set; }

        [XmlElement("indNegociavel")]
        public IndicadorNegociavelCTe IndNegociavel { get; set; }

        [XmlElement("seg")]
        public Seg Seg { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Seg
    {
        [XmlElement("infSeg")]
        public InfSeg InfSeg { get; set; }

        [XmlElement("nApol")]
        public string NApol { get; set; }

        [XmlElement("nAver")]
        public string NAver { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfSeg
    {
        [XmlElement("xSeg")]
        public string XSeg { get; set; }

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Duto
    {
        [XmlIgnore]
        public double VTar { get; set; }

        [XmlElement("vTar")]
        public string VTarField
        {
            get => VTar.ToString("F6", CultureInfo.InvariantCulture);
            set => VTar = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public DateTime DIni { get; set; }

        [XmlElement("dIni")]
        public string DIniField
        {
            get => DIni.ToString("yyyy-MM-dd");
            set => DIni = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime DFim { get; set; }

        [XmlElement("dFim")]
        public string DFimField
        {
            get => DFim.ToString("yyyy-MM-dd");
            set => DFim = DateTime.Parse(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeVTarField() => VTar > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Aereo
    {
        [XmlElement("nMinu")]
        public string NMinu { get; set; }

        [XmlElement("nOCA")]
        public string NOCA { get; set; }

        [XmlIgnore]
        public DateTime DPrevAereo { get; set; }

        [XmlElement("dPrevAereo")]
        public string DPrevAereoField
        {
            get => DPrevAereo.ToString("yyyy-MM-dd");
            set => DPrevAereo = DateTime.Parse(value);
        }

        [XmlElement("natCarga")]
        public NatCarga NatCarga { get; set; }

        [XmlElement("tarifa")]
        public Tarifa Tarifa { get; set; }

        [XmlElement("peri")]
        public List<Peri> Peri { get; set; }

        #region Add (List - Interop)

        public void AddPeri(Peri peri)
        {
            if(Peri == null)
            {
                Peri = new List<Peri>();
            }

            Peri.Add(peri);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeNMinu() => !string.IsNullOrWhiteSpace(NMinu);
        public bool ShouldSerializeNOCA() => !string.IsNullOrWhiteSpace(NOCA);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class NatCarga
    {
        [XmlElement("xDime")]
        public string XDime { get; set; }

        [XmlElement("cInfManu")]
        public List<InformacaoManuseioCTe> CInfManu { get; set; }

        #region Add (List - Interop)

        public void AddCInfManu(InformacaoManuseioCTe cInfManu)
        {
            if(CInfManu == null)
            {
                CInfManu = new List<InformacaoManuseioCTe>();
            }

            CInfManu.Add(cInfManu);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeXDime() => !string.IsNullOrWhiteSpace(XDime);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Tarifa
    {
        [XmlElement("CL")]
        public string CL { get; set; }

        [XmlElement("cTar")]
        public string CTar { get; set; }

        [XmlIgnore]
        public double VTar { get; set; }

        [XmlElement("vTar")]
        public string VTarField
        {
            get => VTar.ToString("F2", CultureInfo.InvariantCulture);
            set => VTar = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeCTar() => !string.IsNullOrWhiteSpace(CTar);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Peri
    {
        [XmlElement("nONU")]
        public string NONU { get; set; }

        [XmlElement("qTotEmb")]
        public string QTotEmb { get; set; }

        [XmlElement("infTotAP")]
        public InfTotAP InfTotAP { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfTotAP
    {
        [XmlIgnore]
        public double QTotProd { get; set; }

        [XmlElement("qTotProd")]
        public string QTotProdField
        {
            get => QTotProd.ToString("F4", CultureInfo.InvariantCulture);
            set => QTotProd = Utility.Converter.ToDouble(value);
        }

        [XmlElement("UnidadeMedidaArtigoPerigoso")]
        public UnidadeMedidaArtigoPerigoso UniAP { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Aquav
    {
        [XmlIgnore]
        public double VPrest { get; set; }

        [XmlElement("vPrest")]
        public string VPrestField
        {
            get => VPrest.ToString("F2", CultureInfo.InvariantCulture);
            set => VPrest = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VAFRMM { get; set; }

        [XmlElement("vAFRMM")]
        public string VAFRMMField
        {
            get => VAFRMM.ToString("F2", CultureInfo.InvariantCulture);
            set => VAFRMM = Utility.Converter.ToDouble(value);
        }

        [XmlElement("xNavio")]
        public string XNavio { get; set; }

        [XmlElement("balsa")]
        public List<Balsa> Balsa { get; set; }

        [XmlElement("nViag")]
        public string NViag { get; set; }

        [XmlElement("direc")]
        public DirecaoCTe Direc { get; set; }

        [XmlElement("irin")]
        public string Irin { get; set; }

        [XmlElement("detCont")]
        public List<DetCont> DetCont { get; set; }

        [XmlElement("tpNav")]
        public TipoNavegacao? TpNav { get; set; }

        #region Add (List - Interop)

        public void AddBalsa(Balsa balsa)
        {
            if(Balsa == null)
            {
                Balsa = new List<Balsa>();
            }

            Balsa.Add(balsa);
        }

        public void AddDetCont(DetCont detCont)
        {
            if(DetCont == null)
            {
                DetCont = new List<DetCont>();
            }

            DetCont.Add(detCont);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeNViag() => !string.IsNullOrWhiteSpace(NViag);
        public bool ShouldSerializeTpNav() => TpNav != null && TpNav != TipoNavegacao.NaoDefinido;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Balsa
    {
        [XmlElement("xBalsa")]
        public string XBalsa { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetCont
    {
        [XmlElement("nCont")]
        public string NCont { get; set; }

        [XmlElement("lacre")]
        public List<Lacre> Lacre { get; set; }

        [XmlElement("infDoc")]
        public DetContInfDoc InfDoc { get; set; }

        #region Add (List - Interop)

        public void AddLacre(Lacre lacre)
        {
            if(Lacre == null)
            {
                Lacre = new List<Lacre>();
            }

            Lacre.Add(lacre);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Lacre
    {
        [XmlElement("nLacre")]
        public string NLacre { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetContInfDoc
    {
        [XmlElement("infNF")]
        public List<DetContInfDocInfNF> InfNF { get; set; }

        [XmlElement("infNFe")]
        public List<DetContInfDocInfNFe> InfNFe { get; set; }

        #region Add (List - Interop)

        public void AddInfNF(DetContInfDocInfNF infNF)
        {
            if(InfNF == null)
            {
                InfNF = new List<DetContInfDocInfNF>();
            }

            InfNF.Add(infNF);
        }

        public void AddInfNFe(DetContInfDocInfNFe infNFe)
        {
            if(InfNFe == null)
            {
                InfNFe = new List<DetContInfDocInfNFe>();
            }

            InfNFe.Add(infNFe);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetContInfDocInfNF
    {
        [XmlElement("serie")]
        public string Serie { get; set; }

        [XmlElement("nDoc")]
        public string NDoc { get; set; }

        [XmlIgnore]
        public double UnidRat { get; set; }

        [XmlElement("unidRat")]
        public string UnidRatField
        {
            get => UnidRat.ToString("F2", CultureInfo.InvariantCulture);
            set => UnidRat = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeUnidRatField() => UnidRat > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class DetContInfDocInfNFe
    {
        [XmlElement("chave")]
        public string Chave { get; set; }

        [XmlIgnore]
        public double UnidRat { get; set; }

        [XmlElement("unidRat")]
        public string UnidRatField
        {
            get => UnidRat.ToString("F2", CultureInfo.InvariantCulture);
            set => UnidRat = Utility.Converter.ToDouble(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeUnidRatField() => UnidRat > 0;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Ferrov
    {
        [XmlElement("tpTraf")]
        public TipoTrafegoCTe TpTraf { get; set; }

        [XmlElement("trafMut")]
        public TrafMut TrafMut { get; set; }

        [XmlElement("fluxo")]
        public string Fluxo { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class TrafMut
    {
        [XmlElement("respFat")]
        public FerroviaCTe RespFat { get; set; }

        [XmlElement("ferrEmi")]
        public FerroviaCTe FerrEmi { get; set; }

        [XmlIgnore]
        public double VFrete { get; set; }

        [XmlElement("vFrete")]
        public string VFreteField
        {
            get => VFrete.ToString("F2", CultureInfo.InvariantCulture);
            set => VFrete = Utility.Converter.ToDouble(value);
        }

        [XmlElement("chCTeFerroOrigem")]
        public string ChCTeFerroOrigem { get; set; }

        [XmlElement("ferroEnv")]
        public List<FerroEnv> FerroEnv { get; set; }

        #region Add (List - Interop)

        public void AddFerroEnv(FerroEnv ferroEnv)
        {
            if(FerroEnv == null)
            {
                FerroEnv = new List<FerroEnv>();
            }

            FerroEnv.Add(ferroEnv);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeChCTeFerroOrigem() => !string.IsNullOrWhiteSpace(ChCTeFerroOrigem);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class FerroEnv
    {
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("cInt")]
        public string CInt { get; set; }

        [XmlElement("IE")]
        public string IE { get; set; }

        [XmlElement("xNome")]
        public string XNome { get; set; }

        [XmlElement("enderFerro")]
        public EnderFerro EnderFerro { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeCInt() => !string.IsNullOrWhiteSpace(CInt);
        public bool ShouldSerializeIE() => !string.IsNullOrWhiteSpace(IE);

        #endregion

    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class EnderFerro
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

        [XmlElement("CEP")]
        public string CEP { get; set; }

        [XmlElement("UF")]
        public UFBrasil UF { get; set; }

        #region ShouldSerialize 

        public bool ShouldSerializeNro() => !string.IsNullOrWhiteSpace(Nro);

        public bool ShouldSerializeXCpl() => !string.IsNullOrWhiteSpace(XCpl);

        public bool ShouldSerializeXBairro() => !string.IsNullOrWhiteSpace(XBairro);

        #endregion

    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class VeicNovos
    {
        [XmlElement("chassi")]
        public string Chassi { get; set; }

        [XmlElement("cCor")]
        public string CCor { get; set; }

        [XmlElement("xCor")]
        public string XCor { get; set; }

        [XmlElement("cMod")]
        public string CMod { get; set; }

        [XmlIgnore]
        public double VUnit { get; set; }

        [XmlElement("vUnit")]
        public string VUnitField
        {
            get => VUnit.ToString("F2", CultureInfo.InvariantCulture);
            set => VUnit = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public double VFrete { get; set; }

        [XmlElement("vFrete")]
        public string VFreteField
        {
            get => VFrete.ToString("F2", CultureInfo.InvariantCulture);
            set => VFrete = Utility.Converter.ToDouble(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class Cobr
    {
        [XmlElement("fat")]
        public Fat Fat { get; set; }

        [XmlElement("dup")]
        public List<Dup> Dup { get; set; }

        #region Add (List - Interop)

        public void AddDup(Dup dup)
        {
            if(Dup == null)
            {
                Dup = new List<Dup>();
            }

            Dup.Add(dup);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
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
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
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
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCteSub
    {

        [XmlElement("chCte")]
        public string ChCte { get; set; }

        [XmlElement("refCteAnu")]
        public string RefCteAnu { get; set; }

        [XmlElement("tomaICMS")]
        public TomaICMS TomaICMS { get; set; }

        [XmlIgnore]
        public SimNao IndAlteraToma { get; set; }

        [XmlElement("indAlteraToma")]
        public string IndAlteraTomaField
        {
            get => (IndAlteraToma == SimNao.Sim ? "1" : "");
            set => IndAlteraToma = (SimNao)Enum.Parse(typeof(SimNao), value.ToString());
        }

        #region ShouldSerialize

        public bool ShouldSerializeRefCteAnu() => !string.IsNullOrWhiteSpace(RefCteAnu);
        public bool ShouldSerializeIndAlteraTomaField() => IndAlteraToma == SimNao.Sim;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class TomaICMS
    {
        [XmlElement("refNFe")]
        public string RefNFe { get; set; }

        [XmlElement("refNF")]
        public RefNF RefNF { get; set; }

        [XmlElement("refCte")]
        public string RefCte { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeRefNFe() => !string.IsNullOrWhiteSpace(RefNFe);
        public bool ShouldSerializeRefCte() => !string.IsNullOrWhiteSpace(RefCte);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class RefNF
    {
        private string ModField;

        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("CPF")]
        public string CPF { get; set; }

        [XmlElement("mod")]
        public string Mod
        {
            get => ModField;
            set
            {
                var permitido = "01,1B,02,2D,2E,04,06,07,08,8B,09,10,11,13,14,15,16,17,18,20,21,22,23,24,25,26,27,28 e 55";
                if(!permitido.Contains(value))
                {
                    throw new Exception("Conteúdo da tag <mod> filha da tag <infCteSub><tomaICMS><RefNF> inválido! Valores aceitos: " + permitido + ".");
                }

                ModField = value;
            }
        }

        [XmlElement("serie")]
        public string Serie { get; set; }

        [XmlElement("subserie")]
        public string Subserie { get; set; }

        [XmlElement("nro")]
        public int Nro { get; set; }

        [XmlIgnore]
        public double Valor { get; set; }

        [XmlElement("valor")]
        public string ValorField
        {
            get => Valor.ToString("F2", CultureInfo.InvariantCulture);
            set => Valor = Utility.Converter.ToDouble(value);
        }

        [XmlIgnore]
        public DateTime DEmi { get; set; }

        [XmlElement("dEmi")]
        public string DEmiField
        {
            get => DEmi.ToString("yyyy-MM-dd");
            set => DEmi = DateTime.Parse(value);
        }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);
        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);
        public bool ShouldSerializeSubserie() => !string.IsNullOrWhiteSpace(Subserie);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfGlobalizado
    {
        private string XObsField;

        [XmlElement("xObs")]
        public string XObs
        {
            get => XObsField;
            set
            {
                if(value.Length < 15)
                {
                    throw new Exception("Conteúdo da tag <xObs> filha da tag <infGlobalizado> inválido! O conteúdo deve ter no mínimo 15 caracteres.");
                }
                else if(value.Length > 256)
                {
                    throw new Exception("Conteúdo da tag <xObs> filha da tag <infGlobalizado> inválido! O conteúdo deve ter no máximo 256 caracteres.");
                }

                XObsField = value;
            }
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfServVinc
    {
        [XmlElement("infCTeMultimodal")]
        public List<InfCTeMultimodal> InfCTeMultimodal { get; set; }

        #region Add (List - Interop)

        public void AddInfCTeMultimodal(InfCTeMultimodal infCTeMultimodal)
        {
            if(InfCTeMultimodal == null)
            {
                InfCTeMultimodal = new List<InfCTeMultimodal>();
            }

            InfCTeMultimodal.Add(infCTeMultimodal);
        }

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCTeMultimodal
    {
        [XmlElement("chCTeMultimodal")]
        public string ChCTeMultimodal { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCteComp
    {
        [XmlElement("chCTe")]
        public string ChCTe { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCteAnu
    {
        [XmlElement("chCte")]
        public string ChCte { get; set; }

        [XmlIgnore]
        public DateTime DEmi { get; set; }

        [XmlElement("dEmi")]
        public string DEmiField
        {
            get => DEmi.ToString("yyyy-MM-dd");
            set => DEmi = DateTime.Parse(value);
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class AutXML
    {
        private string CNPJField;
        private string CPFField;

        [XmlElement("CNPJ")]
        public string CNPJ
        {
            get => CNPJField;
            set
            {
                if(!string.IsNullOrWhiteSpace(CPFField))
                {
                    throw new Exception("Não é permitido informar conteúdo na propriedade CPF e CNPJ (da classe AuxXML) ao mesmo tempo no mesmo objeto, somente uma delas pode ter conteúdo.");
                }

                CNPJField = value;
            }
        }

        [XmlElement("CPF")]
        public string CPF
        {
            get => CPFField;
            set
            {
                if(!string.IsNullOrWhiteSpace(CNPJField))
                {
                    throw new Exception("Não é permitido informar conteúdo na propriedade CPF e CNPJ (da classe AuxXML) ao mesmo tempo no mesmo objeto, somente uma delas pode ter conteúdo.");
                }

                CPFField = value;
            }
        }

        #region ShouldSerialize

        public bool ShouldSerializeCNPJ() => !string.IsNullOrWhiteSpace(CNPJ);
        public bool ShouldSerializeCPF() => !string.IsNullOrWhiteSpace(CPF);

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
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

        public bool ShouldSerializeIdCSRT() => !string.IsNullOrWhiteSpace(IdCSRT);

        public bool ShouldSerializeHashCSRT() => HashCSRT != null;

        #endregion
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfSolicNFF
    {
        [XmlElement("xSolic", Order = 0)]
        public string XSolic { get; set; }
    }

    [Serializable()]
    [XmlType(Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class InfCTeSupl
    {
        [XmlElement("qrCodCTe")]
        public string QrCodCTe { get; set; }
    }
}

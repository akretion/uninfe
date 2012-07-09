using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.ConvertTxt
{
    public enum TpcnTipoCampo { tcStr, tcInt, tcDat, tcHor, tcDatHor, tcDec2, tcDec3, tcDec4, tcDec10 }
    public enum TpcnTipoAmbiente 
    { 
        taProducao = 1, 
        taHomologacao = 2 
    }
    public enum TpcnIndicadorPagamento 
    { 
        ipVista = 0, 
        ipPrazo = 1, 
        ipOutras = 2 
    }
    public enum TpcnTipoNFe 
    { 
        tnEntrada = 0, 
        tnSaida = 1
    }
    public enum TpcnTipoImpressao 
    { 
        tiRetrato = 1, 
        tiPaisagem = 2
    }
    public enum TpcnFinalidadeNFe
    {
        fnNormal = 1,
        fnComplementar = 2,
        fnAjuste = 3
    }
    public enum TpcnTipoEmissao
    {
        teNormal = 1,
        teContingencia = 2,
        teSCAN = 3,
        teDPEC = 4,
        teFSDA = 5,
        teSVCRS = 6,
        teSVCSP = 7
    }

    public enum TpcnCSTIcms
    {
        cst00,
        cst10,
        cst20,
        cst30,
        cst40,
        cst41,
        cst45,
        cst50,
        cst51,
        cst60,
        cst70,
        cst80,
        cst81,
        cst90,
        cstPart10,
        cstPart90,
        cstRep41,
        cstVazio,
        cstICMSOutraUF,
        cstICMSSN
    } //80 e 81 apenas para CTe

    internal enum ObOp
    {
        Obrigatorio,
        Opcional
    }

    /// <summary>
    /// Avulsa
    /// </summary>
    public class Avulsa
    {
        public string CNPJ;
        public string xOrgao;
        public string matr;
        public string xAgente;
        public string fone;
        public string UF;
        public string nDAR;
        public DateTime dEmi;
        public double vDAR;
        public string repEmi;
        public DateTime dPag;
    }

    /// <summary>
    /// Adi
    /// </summary>
    public struct Adi
    {
        public int nAdicao;
        public int nSeqAdi;
        public string cFabricante;
        public double vDescDI;
    }

    /// <summary>
    /// Arma
    /// </summary>
    public class Arma
    {
        public int tpArma;
        public int nSerie;
        public int nCano;
        public string descr;
    }

    /// <summary>
    /// Cana
    /// </summary>
    public class Cana
    {
        public string safra;
        public string Ref;
        public double qTotMes;
        public TpcnTipoCampo qTotMes_Tipo;
        public double qTotAnt;
        public TpcnTipoCampo qTotAnt_Tipo;
        public double qTotGer;
        public TpcnTipoCampo qTotGer_Tipo;
        public double vFor;
        public double vTotDed;
        public double vLiqFor;
        public List<fordia> fordia;
        public List<deduc> deduc;

        public Cana()
        {
            qTotMes_Tipo = TpcnTipoCampo.tcDec10;
            qTotAnt_Tipo = TpcnTipoCampo.tcDec10;
            qTotGer_Tipo = TpcnTipoCampo.tcDec10;
            fordia = new List<fordia>();
            deduc = new List<deduc>();
        }
    }

    /// <summary>
    /// CIDE
    /// </summary>
    public struct CIDE
    {
        public double qBCprod;
        public double vAliqProd;
        public double vCIDE;
    }

    /// <summary>
    /// Cobr
    /// </summary>
    public class Cobr
    {
        public Fat Fat;
        public List<Dup> Dup;

        public Cobr()
        {
            Dup = new List<Dup>();
        }
    }

    /// <summary>
    /// COFINS
    /// </summary>
    public struct COFINS
    {
        public string CST;
        public double vBC;
        public double pCOFINS;
        public double vCOFINS;
        public double vBCProd;
        public double vAliqProd;
        public double qBCProd;
    }

    /// <summary>
    /// COFINSST
    /// </summary>
    public struct COFINSST
    {
        public double vBC;
        public double pCOFINS;
        public double qBCProd;
        public double vAliqProd;
        public double vCOFINS;
    }

    /// <summary>
    /// Comb
    /// </summary>
    public struct Comb
    {
        public int cProdANP;
        public string CODIF;
        public double qTemp;
        public string UFCons;
        public CIDE CIDE;
    }

    /// <summary>
    /// Compra
    /// </summary>
    public struct Compra
    {
        public string xNEmp;
        public string xPed;
        public string xCont;
    }

    /// <summary>
    /// deduc
    /// </summary>
    public class deduc
    {
        public string xDed;
        public double vDed;
    }

    /// <summary>
    /// Dest
    /// </summary>
    public class Dest
    {
        public string CNPJ;
        public string CPF;
        public string xNome;
        public enderDest enderDest;
        public string IE;
        public string ISUF;
        public string email;

        public Dest()
        {
            this.CNPJ = this.CPF = string.Empty;
        }
    }

    /// <summary>
    /// Det
    /// </summary>
    public class Det
    {
        public Prod Prod;
        public Imposto Imposto;
        public string infAdProd;

        public Det()
        {
            Prod = new Prod();
            Imposto = new Imposto();
        }
    }

    /// <summary>
    /// DI
    /// </summary>
    public class DI
    {
        public string nDI;
        public DateTime dDI;
        public string xLocDesemb;
        public string UFDesemb;
        public DateTime dDesemb;
        public string cExportador;
        public List<Adi> adi;

        public DI()
        {
            adi = new List<Adi>();
        }
    }

    /// <summary>
    /// Dup
    /// </summary>
    public class Dup
    {
        public string nDup;
        public DateTime dVenc;
        public double vDup;
    }

    /// <summary>
    /// Emit
    /// </summary>
    public class Emit
    {
        public string CNPJ;
        public string CPF;
        public string xNome;
        public string xFant;
        public enderEmit enderEmit;
        public string IE;
        public string IEST;
        public string IM;
        public string CNAE;
        public int CRT;

        public Emit()
        {
            this.CNPJ = this.CPF = string.Empty;
        }
    }

    /// <summary>
    /// enderDest
    /// </summary>
    public struct enderDest
    {
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public int cMun;
        public string xMun;
        public string UF;
        public int CEP;
        public int cPais;
        public string xPais;
        public string fone;
    }

    /// <summary>
    /// enderEmit
    /// </summary>
    public struct enderEmit
    {
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public int cMun;
        public string xMun;
        public string UF;
        public int CEP;
        public int cPais;
        public string xPais;
        public string fone;
    }

    /// <summary>
    /// Entrega
    /// </summary>
    public class Entrega
    {
        public string CNPJ;
        public string CPF;
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public int cMun;
        public string xMun;
        public string UF;
    }

    /// <summary>
    /// Exporta
    /// </summary>
    public struct Exporta
    {
        public string UFEmbarq;
        public string xLocEmbarq;
    }

    /// <summary>
    /// Fat
    /// </summary>
    public struct Fat
    {
        public string nFat;
        public double vOrig;
        public double vDesc;
        public double vLiq;
    }

    /// <summary>
    /// fordia
    /// </summary>
    public class fordia
    {
        public int dia;
        public double qtde;
        public TpcnTipoCampo qtde_Tipo;
    }

    /// <summary>
    /// ICMS
    /// </summary>
    public struct ICMS
    {
        public int orig;
        public string CST;
        public int ICMSPart10;
        public int ICMSPart90;
        public int ICMSst;
        public string modBC;
        public double pRedBC;
        public double vBC;
        public double pICMS;
        public double vICMS;
        public string modBCST;
        public double pMVAST;
        public double pRedBCST;
        public double vBCST;
        public double pICMSST;
        public double vICMSST;
        public int motDesICMS;
        public double vBCOp;
        public string UFST;
        public double vBCSTRet;
        public double vICMSSTRet;
        public double vBCSTDest;
        public double vICMSSTDest;

        //-- CSON
        public int CSOSN;
        public double pCredSN;
        public double vCredICMSSN;
    }

    /// <summary>
    /// ICMSTot
    /// </summary>
    public struct ICMSTot
    {
        public double vBC;
        public double vICMS;
        public double vBCST;
        public double vST;
        public double vProd;
        public double vFrete;
        public double vSeg;
        public double vDesc;
        public double vII;
        public double vIPI;
        public double vPIS;
        public double vCOFINS;
        public double vOutro;
        public double vNF;
    }

    /// <summary>
    /// Ide
    /// </summary>
    public class Ide
    {
        public int cUF { get; set; }
        public int cNF { get; set; }
        public string natOp { get; set; }
        public TpcnIndicadorPagamento indPag { get; set; }
        public int mod { get; set; }
        public int serie { get; set; }
        public int nNF { get; set; }
        public DateTime dEmi { get; set; }
        public DateTime dSaiEnt { get; set; }
        public DateTime hSaiEnt { get; set; }
        public TpcnTipoNFe tpNF { get; set; }
        public int cMunFG { get; set; }
        public List<NFref> NFref { get; set; }
        public TpcnTipoImpressao tpImp { get; set; }
        public TpcnTipoEmissao tpEmis { get; set; }
        public int cDV { get; set; }
        public TpcnTipoAmbiente tpAmb { get; set; }
        public TpcnFinalidadeNFe finNFe { get; set; }
        public string procEmi { get; set; }
        public string verProc { get; set; }
        public DateTime dhCont { get; set; }
        public string xJust { get; set; }

        public Ide()
        {
            NFref = new List<NFref>();
        }
    }

    /// <summary>
    /// II
    /// </summary>
    public struct II
    {
        public double vBC;
        public double vDespAdu;
        public double vII;
        public double vIOF;
    }

    /// <summary>
    /// Imposto
    /// </summary>
    public class Imposto
    {
        public IPI IPI;
        public ICMS ICMS;
        public II II;
        public PIS PIS;
        public PISST PISST;
        public COFINS COFINS;
        public COFINSST COFINSST;
        public ISSQN ISSQN;
        public ICMSTot ICMSTot;
        public ISSQNtot ISSQNtot;
        public retTrib retTrib;
    }

    /// <summary>
    /// InfAdic
    /// </summary>
    public class InfAdic
    {
        public string infAdFisco;
        public string infCpl;
        public List<obsCont> obsCont;
        public List<obsFisco> obsFisco;
        public List<procRef> procRef;

        public InfAdic()
        {
            obsCont = new List<obsCont>();
            obsFisco = new List<obsFisco>();
            procRef = new List<procRef>();
        }
    }

    /// <summary>
    /// infNFe
    /// </summary>
    public struct infNFe
    {
        public string ID;
    }

    /// <summary>
    /// IPI
    /// </summary>
    public struct IPI
    {
        public string clEnq;
        public string CNPJProd;
        public string cSelo;
        public int qSelo;
        public string cEnq;
        public string CST;
        public double vBC;
        public double qUnid;
        public double vUnid;
        public double pIPI;
        public double vIPI;
    }

    /// <summary>
    /// ISSQN
    /// </summary>
    public struct ISSQN
    {
        public double vBC;
        public double vAliq;
        public double vISSQN;
        public int cMunFG;
        public int cListServ;
        public string cSitTrib;
    }

    /// <summary>
    /// ISSQNtot
    /// </summary>
    public struct ISSQNtot
    {
        public double vServ;
        public double vBC;
        public double vISS;
        public double vPIS;
        public double vCOFINS;
    }

    /// <summary>
    /// Lacres
    /// </summary>
    public class Lacres
    {
        public string nLacre;
    }

    /// <summary>
    /// Med
    /// </summary>
    public class Med
    {
        public string nLote;
        public double qLote;
        public DateTime dFab;
        public DateTime dVal;
        public double vPMC;
    }

    /// <summary>
    /// NFe
    /// </summary>
    public class NFe
    {
        public Ide ide { get; private set; }
        public Emit emit { get; private set; }
        public Dest dest { get; private set; }
        public Avulsa avulsa { get; private set; }
        public Entrega entrega { get; private set; }

        public infNFe infNFe;
        public Retirada retirada { get; private set; }
        public List<Det> det { get; private set; }
        public Total Total;
        public Transp Transp { get; private set; }
        public Cobr Cobr { get; private set; }
        public InfAdic InfAdic { get; private set; }
        public Exporta exporta;
        public Compra compra;
        public Cana cana;
        public protNFe protNFe { get; private set; }

        public NFe()
        {
            ide = new Ide();
            emit = new Emit();
            dest = new Dest();
            avulsa = new Avulsa();
            entrega = new Entrega();
            retirada = new Retirada();
            det = new List<Det>();
            Transp = new Transp();
            Cobr = new Cobr();
            InfAdic = new InfAdic();
            cana = new Cana();
            protNFe = new protNFe();
        }
    }

    /// <summary>
    /// NFref
    /// </summary>
    public class NFref
    {
        public string refNFe { get; set; }
        public string refCTe { get; set; }
        public refNF refNF { get; set; }
        public refNFP refNFP { get; set; }
        public refECF refECF { get; set; }

        public NFref()
        {
            refNF = null;
            refNFP = null;
            refECF = null;
            this.refCTe = this.refNFe = string.Empty;
        }
        public NFref(string refNFe, string refCTe)
        {
            this.refNFe = refNFe;
            this.refCTe = refCTe;
        }
    }

    /// <summary>
    /// obsCont
    /// </summary>
    public class obsCont
    {
        public string xCampo;
        public string xTexto;
    }

    /// <summary>
    /// obsFisco
    /// </summary>
    public class obsFisco
    {
        public string xCampo;
        public string xTexto;
    }

    /// <summary>
    /// PIS
    /// </summary>
    public struct PIS
    {
        public string CST;
        public double vBC;
        public double pPIS;
        public double vPIS;
        public double qBCProd;
        public double vAliqProd;
    }

    /// <summary>
    /// PISST
    /// </summary>
    public struct PISST
    {
        public double vBC;
        public double pPis;
        public double qBCProd;
        public double vAliqProd;
        public double vPIS;
    }

    /// <summary>
    /// procRef
    /// </summary>
    public class procRef
    {
        public string nProc;
        public string indProc;
    }

    /// <summary>
    /// Prod
    /// </summary>
    public class Prod
    {
        public string cProd;
        public int nItem;
        public string cEAN;
        public string xProd;
        public string NCM;
        public string EXTIPI;
        public string CFOP;
        public string uCom;
        public double qCom;
        public double vUnCom;
        public TpcnTipoCampo vUnCom_Tipo;
        public double vProd;
        public string cEANTrib;
        public string uTrib;
        public double qTrib;
        public double vUnTrib;
        public TpcnTipoCampo vUnTrib_Tipo;
        public double vFrete;
        public double vSeg;
        public double vDesc;
        public double vOutro;
        public int indTot;
        public string xPed;
        public int nItemPed;
        public List<DI> DI;
        public veicProd veicProd;
        public List<Med> med;
        public List<Arma> arma;
        public Comb comb;

        public Prod()
        {
            vUnCom_Tipo = TpcnTipoCampo.tcDec10;
            vUnTrib_Tipo = TpcnTipoCampo.tcDec10;
            DI = new List<DI>();
            med = new List<Med>();
            arma = new List<Arma>();
        }
    }

    public class protNFe
    {
        public TpcnTipoAmbiente tpAmb;
        public string verAplic;
        public string chNFe;
        public DateTime dhRecbto;
        public string nProt;
        public string digVal;
        public int cStat;
        public string xMotivo;
    }

    /// <summary>
    /// Reboque
    /// </summary>
    public class Reboque
    {
        public string placa;
        public string UF;
        public string RNTC;
        public string vagao;
        public string balsa;
    }

    /// <summary>
    /// RefECF
    /// </summary>
    public class refECF
    {
        public string mod { get; set; }
        public int nECF { get; set; }
        public int nCOO { get; set; }
    }

    /// <summary>
    /// RefNF
    /// </summary>
    public class refNF
    {
        public int cUF { get; set; }
        public string AAMM { get; set; }
        public string CNPJ { get; set; }
        public string mod { get; set; }
        public int serie { get; set; }
        public int nNF { get; set; }
    }

    /// <summary>
    /// RefNFP
    /// </summary>
    public class refNFP
    {
        public int cUF { get; set; }
        public string AAMM { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string IE { get; set; }
        public string mod { get; set; }
        public int serie { get; set; }
        public int nNF { get; set; }
        public refNFP()
        {
            this.CNPJ = this.CPF = string.Empty;
        }
    }

    /// <summary>
    /// Retirada
    /// </summary>
    public class Retirada
    {
        public string CNPJ;
        public string CPF;
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public int cMun;
        public string xMun;
        public string UF;
    }

    /// <summary>
    /// retTransp
    /// </summary>
    public struct retTransp
    {
        public double vServ;
        public double vBCRet;
        public double pICMSRet;
        public double vICMSRet;
        public string CFOP;
        public int cMunFG;
    }

    /// <summary>
    /// retTrib
    /// </summary>
    public struct retTrib
    {
        public double vRetPIS;
        public double vRetCOFINS;
        public double vRetCSLL;
        public double vBCIRRF;
        public double vIRRF;
        public double vBCRetPrev;
        public double vRetPrev;
    }

    /// <summary>
    /// Total
    /// </summary>
    public struct Total
    {
        public ICMSTot ICMSTot;
        public ISSQNtot ISSQNtot;
        public retTrib retTrib;
    }

    /// <summary>
    /// Transp
    /// </summary>
    public class Transp
    {
        public int modFrete;
        public Transporta Transporta;
        public retTransp retTransp;
        public veicTransp veicTransp;
        public List<Vol> Vol;
        public List<Reboque> Reboque;

        public Transp()
        {
            Vol = new List<Vol>();
            Reboque = new List<Reboque>();
        }
    }

    /// <summary>
    /// Transporta
    /// </summary>
    public struct Transporta
    {
        public string CNPJ;
        public string CPF;
        public string xNome;
        public string IE;
        public string xEnder;
        public string xMun;
        public string UF;
    }

    /// <summary>
    /// veicProd
    /// </summary>
    public struct veicProd
    {
        public string tpOp;
        public string chassi;
        public string cCor;
        public string xCor;
        public string pot;
        public string cilin;
        public string pesoL;
        public string pesoB;
        public string nSerie;
        public string tpComb;
        public string nMotor;
        public string CMT;
        public string dist;
        public int anoMod;
        public int anoFab;
        public string tpPint;
        public int tpVeic;
        public int espVeic;
        public string VIN;
        public string condVeic;
        public string cMod;
        public int cCorDENATRAN;
        public int lota;
        public int tpRest;
    }

    /// <summary>
    /// veicTransp
    /// </summary>
    public struct veicTransp
    {
        public string placa;
        public string UF;
        public string RNTC;
    }

    /// <summary>
    /// Vol
    /// </summary>
    public class Vol
    {
        public int qVol;
        public string esp;
        public string marca;
        public string nVol;
        public double pesoL;
        public double pesoB;
        public List<Lacres> Lacres;

        public Vol()
        {
            this.Lacres = new List<Lacres>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
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
        public string indSomaCOFINSST;
    }

    /// <summary>
    /// ICMS
    /// </summary>
    public struct ICMS
    {
        public TpcnOrigemMercadoria orig;
        public string CST;
        public int ICMSPart10;
        public int ICMSPart90;
        public int ICMSst;
        public TpcnDeterminacaoBaseIcms modBC;
        public double pRedBC;
        public double vBC;
        public double pICMS;
        public double vICMS;
        public TpcnDeterminacaoBaseIcmsST modBCST;
        public double pMVAST;
        public double pRedBCST;
        public double vBCST;
        public double pICMSST;
        public double vICMSST;
        public int motDesICMS;
        public double pBCOp;
        public string UFST;
        public double vBCSTRet;
        public double vICMSSTRet;
        public double vBCSTDest;
        public double vICMSSTDest;
        public double vICMSDeson;
        public double vICMSDif;
        public double pDif;
        public double vICMSOp;
        public double pFCP;
        public double vFCP;
        public double pFCPDif;
        public double vFCPDif;
        public double vFCPEfet;
        public double vBCFCP;
        public double vBCFCPST;
        public double pFCPST;
        public double vFCPST;
        public double vICMSSTDeson;
        public int motDesICMSST;
        public double pST;
        public double vBCFCPSTRet;
        public double pFCPSTRet;
        public double vFCPSTRet;
        public double pRedBCEfet;
        public double vBCEfet;
        public double pICMSEfet;
        public double vICMSEfet;
        public double vICMSSubstituto;

        //-- CSON
        public int CSOSN;
        public double pCredSN;
        public double vCredICMSSN;

        public ICMSUFDest ICMSUFDest;
    }

    /// <summary>
    /// ICMSTot
    /// </summary>
    public struct ICMSTot
    {
        public double vBC;
        public double vICMS;
        public double vICMSDeson;
        public double vICMSUFDest;
        public double vFCPUFDest;
        public double vICMSUFRemet;
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
        public double vTotTrib;
        public double vFCP;
        public double vFCPST;
        public double vFCPSTRet;
        public double vIPIDevol;
    }

    public struct ICMSUFDest
    {
        public double vBCUFDest;
        public double pFCPUFDest;
        public double pICMSUFDest;
        public double pICMSInter;
        public double pICMSInterPart; 
        public double vFCPUFDest;
        public double vICMSUFDest;
        public double vICMSUFRemet;
        public double vBCFCPUFDest;
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
        public double vTotTrib;
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

        public Imposto()
        {
            ISSQN.cListServ = string.Empty;
        }
    }

    public class impostoDevol
    {
        public double pDevol;
        public double vIPIDevol;
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
        public string cListServ;
        public string cSitTrib;
        // 3.10
        public double vDeducao;
        public double vOutro;
        public double vDescIncond;
        public double vDescCond;
        public double vISSRet;
        public TpcnindISS indISS;
        public string cServico;
        public int cMun;
        public int cPais;
        public string nProcesso;
        public bool indIncentivo;
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

        public DateTime dCompet;
        public double vDeducao;
        public double vOutro;
        public double vDescIncond;
        public double vDescCond;
        public double vISSRet;
        public TpcnRegimeTributario cRegTrib;
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
        public string indSomaPISST;
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

}

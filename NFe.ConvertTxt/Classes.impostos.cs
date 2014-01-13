﻿using System;
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
        public double vICMSDeson;
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
        public double vINSS;
        public double vIR;
        public double vCSLL;
        public double vOutro;
        public double vDescIncond;
        public double vDescCond;
        public bool indISSRet;
        public TpcnindISS indISS;
        public string cServico;
        public string cMun;
        public int cPais;
        public string nProcesso;
        public TpcnRegimeTributario cRegTrib;
        public bool indIncentivo;
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components
{
    public enum TpcnResources
    {
        AAMM,
        ano,
        anoFab,
        anoMod,
        balsa,

        cAerDes,
        cAerEmb,
        cAut,
        cap,
        capKG,
        capM3,
        cCor,
        cCorDENATRAN,
        cCT,
        cDV,
        cEAN,
        cEANTrib,
        cEmbar,
        cEmbComb,
        cEnq,
        CEP,
        cExportador,
        cFabricante,
        CFOP,
        chassi,
        chave,
        chCTe,
        chCte,
        chMDFe,
        chNFe,
        cilin,
        cIMP,
        cInt,
        CIOT,
        clEnq,
        cInfManu,
        CL,
        cListServ,
        cMDF,
        cMod,
        CMT,
        cMun,
        cMunFG,
        cMunEnv,
        cMunIni,
        cMunFim,
        CNAE,
        cNF,
        CNPJ,
        CNPJAgeNav,
        CNPJDest,
        CNPJForn,
        CNPJPg,
        CNPJProd,
        CODIF,
        cOrgao,
        cOrgaoAutor,
        condVeic,
        COTM,
        cMunDescarga,
        cMunCarrega,
        cPais,
        CPF,
        CPFDest,
        cProd,
        cProdANP,
        cPrtDest,
        cPrtEmb,
        cRegTrib,
        CRT,
        cSelo,
        cServico,
        cSit,
        cSitTrib,
        CSOSN,
        CST,
        cStat,
        cTar,
        cTermCarreg,
        cTermDescarreg,
        cUF,
        cUFAutor,
        cUnid,

        dBaixa,
        dCompet,
        dDesemb,
        dDI,
        dEmi,
        descr,
        descEvento,
        descOutros,
        destCalc,
        detEvento,
        dFab,
        dFim,
        dhCons,
        dhCont,
        dhEmi,
        dhEvento,
        dhRecbto,
        dhRegDPEC,
        dhRegEvento,
        dhResp,
        dhSaiEnt,
        dhTrem,
        dIni,
        dia,
        digVal,
        dIniAtiv,
        direc,
        dist,
        dPag,
        dPrev,
        dPrevAereo,
        dProg,
        dSaiEnt,
        dUltSit,
        dVal,
        dVenc,
        dVoo,

        email,
        esp,
        espVeic,
        EXTIPI,

        ferrEmi,

        finNFe,
        fluxo,
        fone,
        forPag,

        grEmb,

        hFim,
        hIni,
        hProg,

        hSaiEnt,

        ICMS,
        ID,
        Id,
        idDest,
        idEstrangeiro,
        idLote,
        IdT,
        idTrem,
        idUnidCarga,
        idUnidTransp,
        IE,
        IEAtual,
        IEUnica,
        IEST,
        IM,
        indCredCTe,
        indCredNFe,
        indEmi,
        indFinal,
        indIEDest,
        indIncentivo,
        indISS,
        indISSRet,
        indNegociavel,
        indNFe,
        indPag,
        indPres,
        indProc,
        indSN,
        indTot,
        infAdFisco,
        infAdProd,
        infCpl,
        irin,
        ISUF,
        lota,
        marca,
        matr,
        mod,
        modal,
        modBC,
        modBCST,
        motDesICMS,
        modFrete,
        nAdicao,
        nApol,
        nac,
        natOp,
        nAver,
        nBooking,
        nCano,
        NCM,
        nCOO,
        nCompra,
        nCont,
        nCT,
        nCtrl,
        nDAR,
        nMDF,
        nDI,
        nDoc,
        nDup,
        nDraw,
        nECF,
        nFat,
        nFCI,
        nCFOP,
        NFref,
        NItem,
        nItemPed,
        nLacre,
        nLote,
        nMinu,
        nMotor,
        nNF,
        nNFFin,
        nNFIni,
        nOCA,
        nOcc,
        nONU,
        nPed,
        nPeso,
        nProc,
        nProcesso,
        nProt,
        nro,
        nRE,
        nRECOPI,
        nRec,
        nRegDPEC,
        nRoma,
        nSeq,
        nSeqAdic,
        nSeqEvento,
        nSerie,
        nVag,
        nViag,
        nVol,
        nVoo,
        NSU,
        NVE,

        orig,
        origCalc,

        pBCOp,
        pDif,
        pDevol,
        pMixGN,
        pCOFINS,
        pCredSN,
        pesoB,
        pesoBC,
        pesoL,
        pesoR,
        pICMS,
        pICMSRet,
        pICMSST,
        pICMSOutraUF,
        pICMSSTRet,
        PIN,
        pIPI,
        PISOutr,
        placa,
        pMVAST,
        pontoFulgor,
        pot,
        pPIS,
        procEmi,
        proPred,
        pRedBC,
        pRedBCST,

        pRedBCOutraUF,
        prtDest,
        prtEmb,
        prtTrans,

        qBCProd,
        qCarga,
        qCom,
        qCT,
        qCTe,
        qExport,
        qLote,
        qMDFe,
        qNF,
        qNFe,
        qSelo,
        qtde,
        qtdRat,
        qTemp,
        qTotAnt,
        qTotGer,
        qTotMes,
        qTotProd,
        qTrib,
        qUnid,
        qVag,
        qVol,
        qVolTipo,

        Ref,
        refCTe,
        refCte,
        refCteAnu,
        refCTE,
        refNFe,
        RENAVAM,
        repEmi,
        respFat,
        respSeg,
        retira,
        RNTC,
        RNTRC,

        safra,
        SegCodBarra,
        serie,
        subser,
        subserie,

        tara,
        tBand,
        tMed,
        toma,
        tpAmb,
        tpArma,
        tpAutor,
        tpCar,
        tpComb,
        tpCTe,
        tpDoc,
        tpEmb,
        tpEmis,
        tpEmit,
        tpEvento,
        tpHor,
        tpImp,
        tpIntermedio,
        tpMed,
        tpNF,
        tpNav,
        tpOp,
        tPag,
        tpPer,
        tpPint,
        tpProp,
        tpRest,
        tpRod,
        tpServ,
        tpTraf,
        tpUnidCarga,
        tpUnidTransp,
        tpVag,
        tpVeic,
        tpViaTransp,
        TU,

        UFEnv,
        UFFim,
        UFIni,
        UFPer,
        UFSaidaPais,
        UFTerceiro,
        uCom,
        UF,
        UFCons,
        UFDesemb,
        UFEmbarq,
        UFST,
        ultNSU,
        unidRat,
        uTrib,

        vAFRMM,
        valor,
        vCarga,
        vCSLL,
        vDescCond,
        vDescIncond,
        vDeducao,
        vDocFisc,
        vICMSDif,
        vICMSOp,
        vINSS,
        vIPIDevol,
        vIR,
        xLocDespacho,
        xLocExporta,
        vagao,
        vAliq,
        vAliqProd,
        vBC,
        vBCICMS,
        vBCICMSST,
        vBCICMSSTCons,
        vBCICMSSTDest,
        vBCOutraUF,
        vBCIRRF,
        vBCOp,
        vBCRet,
        vBCRetPrev,
        vBCST,
        vBCSTDest,
        vBCSTRet,
        vCIDE,
        vCOFINS,
        vComp,
        vCred,
        vCredICMSSN,
        vDAR,
        vDed,
        vDesc,
        vDescDI,
        vDespAdu,
        vDup,
        verAplic,
        verEvento,
        verProc,
        versao,
        versaoDados,
        versaoModal,
        vFor,
        vFrete,
        vICMS,
        vICMSDeson,
        vICMSRet,
        vICMSST,
        vICMSSTCons,
        vICMSSTDest,
        vICMSSTRet,
        vICMSOutraUF,
        vII,
        VIN,
        vIOF,
        vIPI,
        vIRRF,
        vISS,
        vISSQN,
        vISSRet,
        vLiq,
        vLiqFor,
        vNF,
        vOrig,
        vOutro,
        vPag,
        vPIS,
        vPMC,
        vPrest,
        vProd,
        vRec,
        vRetCOFINS,
        vRetCSLL,
        vRetPIS,
        vRetPrev,
        vSeg,
        vServ,
        vST,
        vTar,
        vTotDed,
        vTotTrib,
        vTPrest,
        vUnCom,
        vUnid,
        vUnit,
        vUnTrib,
        vValePed,

        xAgente,
        xBairro,
        xBalsa,
        xCampo,
        xCaracAd,
        xCaracSer,
        xClaRisco,
        xCondUso,
        xCont,
        xCor,
        xCorrecao,
        xCpl,
        xDed,
        xDest,
        xDetRetira,
        xDime,
        xEmi,
        xEnder,
        xEvento,
        xFant,
        xJust,
        xLAgEmi,
        xLgr,
        xLocDesemb,
        xLocEmbarq,
        xMotivo,
        xmlns,
        xMun,
        xMunCarrega,
        xMunDescarga,
        xMunEnv,
        xMunFim,
        xMunIni,
        xNavio,
        xNEmp,
        xNome,
        xNomeAE,
        xObs,
        xOrgao,
        xOri,
        xOrig,
        xOutCat,
        xPais,
        xPass,
        xPed,
        xPref,
        xProd,
        xRegApur,
        xRota,
        xSeg,
        xServ,
        xTexto
    }

    public class NFeStrConstants
    {
        public static string NAME_SPACE_MDFE = "http://www.portalfiscal.inf.br/mdfe";
        public static string NAME_SPACE_CTE = "http://www.portalfiscal.inf.br/cte";
        public static string NAME_SPACE_NFE = "http://www.portalfiscal.inf.br/nfe";

        public static string proxyError = "Especifique o nome do servidor/usuário/senha e porta para conectar do servidor proxy";
        public static string versaoError = "Defina a versão";
        public static string Nome = "Nome";
        public static string Padrao = "Padrao";
        public static string Estado = "Estado";
        public static string Registro = "Registro";
        public static string Servico = "Servico";
        public static string SVC = "SVC";
        public static string LocalHomologacao = "LocalHomologacao";
        public static string LocalProducao = "LocalProducao";


        public static string nfe_configuracoes = "nfe_configuracoes";
        public static string PastaXmlAssinado = "PastaXmlAssinado";
        public static string PastaXmlEnvio = "PastaXmlEnvio";
        public static string PastaXmlEmLote = "PastaXmlEmLote";
        public static string PastaXmlRetorno = "PastaXmlRetorno";
        public static string PastaXmlEnviado = "PastaXmlEnviado";
        public static string PastaXmlErro = "PastaXmlErro";
        public static string PastaBackup = "PastaBackup";
        public static string PastaValidar = "PastaValidar";
        public static string PastaDownloadNFeDest = "PastaDownloadNFeDest";
        public static string ConfiguracaoDanfe = "ConfiguracaoDanfe";
        public static string ConfiguracaoCCe = "ConfiguracaoCCe";
        public static string PastaExeUniDanfe = "PastaExeUniDanfe";
        public static string PastaConfigUniDanfe = "PastaConfigUniDanfe";
        public static string PastaDanfeMon = "PastaDanfeMon";
        public static string XMLDanfeMonNFe = "XMLDanfeMonNFe";
        public static string XMLDanfeMonProcNFe = "XMLDanfeMonProcNFe";
        public static string XMLDanfeMonDenegadaNFe = "XMLDanfeMonDenegadaNFe";

        public static string UsuarioWS = "UsuarioWS";
        public static string SenhaWS = "SenhaWS";

        public static string FTPAtivo = "FTPAtivo";
        public static string FTPGravaXMLPastaUnica = "FTPGravaXMLPastaUnica";
        public static string FTPSenha = "FTPSenha";
        public static string FTPPastaAutorizados = "FTPPastaAutorizados";
        public static string FTPPastaRetornos = "FTPPastaRetornos";
        public static string FTPNomeDoUsuario = "FTPNomeDoUsuario";
        public static string FTPNomeDoServidor = "FTPNomeDoServidor";
        public static string FTPPorta = "FTPPorta";

        public static string CertificadoDigital = "CertificadoDigital";
        public static string CertificadoDigitalThumbPrint = "CertificadoDigitalThumbPrint";
        public static string CertificadoInstalado = "CertificadoInstalado";
        public static string CertificadoArquivo = "CertificadoArquivo";
        public static string CertificadoSenha = "CertificadoSenha";
        public static string CertificadoPIN = "CertificadoPIN";
        public static string ProviderCertificado = "ProviderCertificado";
        public static string ProviderTypeCertificado = "ProviderTypeCertificado";

        public static string AmbienteCodigo = "AmbienteCodigo";
        public static string DiasLimpeza = "DiasLimpeza";
        public static string DiretorioSalvarComo = "DiretorioSalvarComo";
        public static string GravarRetornoTXTNFe = "GravarRetornoTXTNFe";
        public static string GravarEventosNaPastaEnviadosNFe = "GravarEventosNaPastaEnviadosNFe";
        public static string GravarEventosCancelamentoNaPastaEnviadosNFe = "GravarEventosCancelamentoNaPastaEnviadosNFe";
        public static string GravarEventosDeTerceiros = "GravarEventosDeTerceiros";
        public static string CompactarNfe = "CompactarNfe";
        public static string TempoConsulta = "TempoConsulta";
        public static string UnidadeFederativaCodigo = "UnidadeFederativaCodigo";
        public static string IndSinc = "IndSinc";
    }
}
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
        chMDFe,
        chNFe,
        cilin,
        cInt,
        CIOT,
        clEnq,
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
        detEvento,
        dFab,
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
        dia,
        digVal,
        dIniAtiv,
        dist,
        dPag,
        dPrev,
        dSaiEnt,
        dUltSit,
        dVal,
        dVenc,
        dVoo,

        email,
        esp,
        espVeic,
        EXTIPI,

        finNFe,
        fone,
        forPag,

        hSaiEnt,

        ICMS,
        ID,
        Id,
        idDest,
        idEstrangeiro,
        idLote,
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
        indNFe,
        indPag,
        indPres,
        indProc,
        indTot,
        infAdFisco,
        infAdProd,
        infCpl,
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
        nac,
        natOp,
        nCano,
        NCM,
        nCOO,
        nCompra,
        nCont,
        nCT,
        nDAR,
        nMDF,
        nDI,
        nDoc,
        nDup,
        nDraw,
        nECF,
        nFat,
        nFCI,
        NFref,
        NItem,
        nItemPed,
        nLacre,
        nLote,
        nMotor,
        nNF,
        nNFFin,
        nNFIni,
        nProc,
        nProcesso,
        nProt,
        nro,
        nRE,
        nRECOPI,
        nRec,
        nRegDPEC,
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

        pBCOp,
        pDif,
        pDevol,
        pMixGN,
        pCOFINS,
        pCredSN,
        pesoB,
        pesoL,
        pICMS,
        pICMSRet,
        pICMSST,
        PIN,
        pIPI,
        PISOutr,
        placa,
        pMVAST,
        pot,
        pPIS,
        pRedBC,
        pRedBCST,

        procEmi,
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
        qTrib,
        qUnid,
        qVag,
        qVol,

        Ref,
        refCTe,
        refNFe,
        RENAVAM,
        repEmi,
        retira,
        RNTC,
        RNTRC,

        safra,
        SegCodBarra,
        serie,
        subser,

        tara,
        tBand,
        tMed,
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
        tpImp,
        tpIntermedio,
        tpMed,
        tpNF,
        tpOp,
        tPag,
        tpPer,
        tpPint,
        tpProp,
        tpRest,
        tpRod,
        tpServ,
        tpUnidCarga,
        tpUnidTransp,
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
        uTrib,

        vAFRMM,
        vCarga,
        vCSLL,
        vDescCond,
        vDescIncond,
        vDeducao,
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
        vFor,
        vFrete,
        vICMS,
        vICMSDeson,
        vICMSRet,
        vICMSST,
        vICMSSTCons,
        vICMSSTDest,
        vICMSSTRet,
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
        vProd,
        vRetCOFINS,
        vRetCSLL,
        vRetPIS,
        vRetPrev,
        vSeg,
        vServ,
        vST,
        vTotDed,
        vTotTrib,
        vUnCom,
        vUnid,
        vUnit,
        vUnTrib,
        xAgente,
        xBairro,
        xCampo,
        xCondUso,
        xCont,
        xCor,
        xCorrecao,
        xCpl,
        xDed,
        xDest,
        xDetRetira,
        xEnder,
        xEvento,
        xFant,
        xJust,
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
        xNEmp,
        xNome,
        xOrgao,
        xOri,
        xPais,
        xPed,
        xPref,
        xProd,
        xRegApur,
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

using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.ConvertTxt
{
    ///
    /// NFC-e
    /// 
    public enum TpcnDestinoOperacao {
        doInterna = 1, 
        doInterestadual = 2, 
        doExterior = 3
    }
    public enum TpcnConsumidorFinal {
        cfNao = 0, 
        cfConsumidorFinal = 1
    }
    public enum TpcnPresencaComprador {
        pcNao=0, 
        pcPresencial=1, 
        pcInternet=2, 
        pcTeleatendimento=3, 
        pcOutros=9
    }
    public enum TpcnFormaPagamento {
        fpDinheiro=1, 
        fpCheque=2, 
        fpCartaoCredito=3, 
        fpCartaoDebito=4, 
        fpCreditoLoja=5,
        fpValeAlimentacao=10, 
        fpValeRefeicao=11, 
        fpValePresente=12, 
        fpValeCombustivel=13,
        fpOutro=99
    }
    public enum TpcnBandeiraCartao
    {
        bcVisa=1, 
        bcMasterCard=2, 
        bcAmericanExpress=3, 
        bcSorocred=4, 
        bcOutros=99
    }

    public enum TpcnProcessoEmissao
    {
        peAplicativoContribuinte = 0,
        peAvulsaFisco = 1,
        peAvulsaContribuinte = 2,
        peContribuinteAplicativoFisco = 3
    }
    public enum TpcnModalidadeFrete 
    { 
        mfContaEmitente = 0, 
        mfContaDestinatario = 1, 
        mfContaTerceiros = 2, 
        mfSemFrete = 9
    }
    public enum TpcnDeterminacaoBaseIcms 
    { 
        dbiMargemValorAgregado, 
        dbiPauta, 
        dbiPrecoTabelado, 
        dbiValorOperacao 
    }
    public enum TpcnDeterminacaoBaseIcmsST 
    { 
        dbisPrecoTabelado = 0, 
        dbisListaNegativa = 1, 
        dbisListaPositiva = 2, 
        dbisListaNeutra = 3, 
        dbisMargemValorAgregado = 4, 
        dbisPauta = 5
    }
    public enum TpcnOrigemMercadoria
    {
        oeNacional = 0,
        oeEstrangeiraImportacaoDireta = 1,
        oeEstrangeiraAdquiridaBrasil = 2,
		oeNacional_Mercadoria_ou_bem_com_Conteúdo_de_Importação_superior_a_40 = 3,
        oeNacional_Cuja_produção_tenha_sido_feita_em_conformidade_com_o_PPB = 4,
        oeNacional_Mercadoria_com_bem_ou_conteúdo_de_importação_inferior_a_40 = 5,
        oeEstrangeira_Importação_direta_sem_similar_nacional = 6,
        oeEstrangeira_Adquirida_no_mercado_interno_com_similar_nacional=7
    }
    public enum TpcnTipoArma 
    { 
        taUsoPermitido = 0, 
        taUsoRestrito = 1
    }
    public enum TpcnCondicaoVeiculo 
    { 
        cvAcabado = 1, 
        cvInacabado = 2, 
        cvSemiAcabado = 3
    }
    public enum TpcnTipoOperacao 
    { 
        toVendaConcessionaria = 1, 
        toFaturamentoDireto = 2, 
        toVendaDireta = 3, 
        toOutros = 0
    }
    public enum TpcnIndicadorTotal 
    { 
        itNaoSomaTotalNFe = 0, 
        itSomaTotalNFe = 1 
    }
    public enum TpcnCRT 
    { 
        crtSimplesNacional = 1, 
        crtSimplesExcessoReceita = 2,
        crtRegimeNormal = 3
    }
    public enum TpcnTipoCampo 
    { 
        tcStr, tcInt, tcDat, tcDat2, tcHor, tcDatHor, tcDec2, tcDec3, tcDec4, tcDec10 
    }

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
        tiPaisagem = 2,
        tiDANFESimplificado = 3,
        tiDANFENFCe = 4,
        //tiDANFENFCe_resumido = 5,
        tiDANFENFCe_em_mensagem_eletrônica = 5
    }
    public enum TpcnFinalidadeNFe
    {
        fnNormal = 1,
        fnComplementar = 2,
        fnAjuste = 3,
        fnNFe_de_Resumo_da_operação_em_contingência_da_NFCe = 4
    }
    public enum TpcnTipoEmissao
    {
        teNormal = 1,
        teContingencia = 2,     //Contingência FS-IA, com impressão do DANFE em formulário de segurança;
        teSCAN = 3,             //Contingência SCAN (Sistema de Contingência do Ambiente Nacional);
        teDPEC = 4,             //Contingência DPEC (Declaração Prévia da Emissão em Contingência);
        teFSDA = 5,             //Contingência FS-DA, com impressão do DANFE em formulário de segurança;
        teSVCAN = 6,            //Contingência SVC-AN (SEFAZ Virtual de Contingência do AN);
        teSVCRS = 7,            //Contingência SVC-RS (SEFAZ Virtual de Contingência do RS);
        teSVCSP = 8, 
        teOffLine = 9           //Contingência off-line da NFC-e;
    }
    public enum TpcnECFModRef 
    {
        ECFModRefVazio, 
        ECFModRef2B,
        ECFModRef2C,
        ECFModRef2D /*'', '2B', '2C','2D'*/
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

    public enum TpcnindIEDest {
        inContribuinte=1, 
        inIsento=2, 
        inNaoContribuinte=9
    }

    public enum TpcnTipoViaTransp
    {
        tvMaritima = 1,
        tvFluvial = 2,
        tvLacustre = 3,
        tvAerea = 4,
        tvPostal = 5,
        tvFerroviaria = 6,
        tvRodoviaria = 7,
        tvConduto = 8,
        tvMeiosProprios = 9,
        tvEntradaSaidaFicta = 10
    }

    public enum TpcnTipoIntermedio
    {
        tiContaPropria = 1,
        tiContaOrdem = 2,
        tiEncomenda = 3
    }

    public enum TpcnindISS
    {
        iiExigivel = 1, 
        iiNaoIncidencia = 2, 
        iiIsencao = 3, 
        iiExportacao = 4,
        iiImunidade = 5, 
        iiExigSuspDecisaoJudicial = 6, 
        iiExigSuspProcessoAdm = 7
    }

    public enum TpcnRegimeTributario
    {
        Microempresa_Municipal = 1,
        Estimativa = 2,
        Sociedade_de_Profissionais = 3,
        Cooperativa = 4,
        Microempresário_Individual__MEI = 5,
        Microempresário_e_Empresa_de_Pequeno_Porte__ME_EPP = 6
    }
}

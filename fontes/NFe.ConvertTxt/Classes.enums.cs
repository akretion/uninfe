using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace NFe.ConvertTxt
{
    public static class versoes
    {
        #region NFe
        public static string VersaoXMLStatusServico = "4.00";
        public static string VersaoXMLNFe = "4.00";
        public static string VersaoXMLPedSit = "4.00";
        public static string VersaoXMLInut = "4.00";
        public static string VersaoXMLConsCad = "2.00";
        public static string VersaoXMLEvento = "1.00";
        public static string VersaoXMLEnvConsultaNFeDest = "1.01";
        public static string VersaoXMLEnvDFe = "1.00";
        #endregion

        #region MDF-e
        public static string VersaoXMLMDFeCabecMsg = "1.00";
        public static string VersaoXMLMDFeStatusServico = "1.00";
        public static string VersaoXMLMDFe = "1.00";
        public static string VersaoXMLMDFePedRec = "1.00";
        public static string VersaoXMLMDFeEvento = "1.00";
        public static string VersaoXMLMDFeConsNaoEnc = "1.00";
        #endregion

        #region CT-e
        public static string VersaoXMLCTeCabecMsg = "2.00";
        public static string VersaoXMLCTeStatusServico = "2.00";
        public static string VersaoXMLCTe = "2.00";
        public static string VersaoXMLCTePedRec = "2.00";
        public static string VersaoXMLCTeInut = "2.00";
        public static string VersaoXMLCTeEvento = "2.00";
        #endregion
    }
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
        [Description("0=Não se aplica (por exemplo, Nota Fiscal complementar ou de ajuste);")]
        pcNao = 0, 
        [Description("Operação presencial")]
        pcPresencial = 1, 
        [Description("Operação não presencial, pela Internet")]
        pcInternet = 2, 
        [Description("Operação não presencial, Teleatendimento")]
        pcTeleatendimento = 3, 
        [Description("NFC-e em operação com entrega a domicílio")]
        pcEntregaDomicilio = 4,
        [Description("Operação presencial, fora do estabelecimento")]
        pcPresencialForaEstabelecimento = 5,
        [Description("Operação não presencial, outros")]
        pcOutros = 9
    }
    public enum TpcnFormaPagamento {
        [Description("01=Dinheiro")]
        fpDinheiro=1, 
        [Description("02=Cheque")]
        fpCheque=2, 
        [Description("03=Cartão de Crédito")]
        fpCartaoCredito=3, 
        [Description("04=Cartão de Débito")]
        fpCartaoDebito=4, 
        [Description("05=Crédito Loja")]
        fpCreditoLoja=5,
        [Description("10=Vale Alimentação")]
        fpValeAlimentacao=10, 
        [Description("11=Vale Refeição")]
        fpValeRefeicao=11, 
        [Description("12=Vale Presente")]
        fpValePresente=12, 
        [Description("13=Vale Combustível")]
        fpValeCombustivel=13,
        //[Description("14=Duplicata Mercantil")]
        //fpDulicataMercantil = 14,
        [Description("15=Boleto Bancário")]
        fpBoletoBancario = 15,
        [Description("90=Sem pagamento")]
        fpSemPagamento=90,
        [Description("99 =Outros")]        fpOutro=99
    }
    public enum TpcnBandeiraCartao
    {
        [Description("01 - VISA")]
        bcVisa=1, 
        [Description("02 - MasterCard")]
        bcMasterCard=2, 
        [Description("03 - American Express")]
        bcAmericanExpress=3, 
        [Description("04 - Sorocred")]
        bcSorocred=4, 
        [Description("05 - Diners Club")]
        bcDinersClub = 5,
        [Description("06 - Elo")]
        bcElo = 6,
        [Description("07 - Hipercard")]
        bcHipercard = 7,
        [Description("08 - Aura")]
        bcAura = 8,
        [Description("09 - Cabal")]
        bcCabal = 9,
        [Description("09 - Outros")]
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
        [Description("0=Contratação do Frete por conta do Remetente (CIF)")]
        mfContaEmitente = 0, 
        [Description("1=Contratação do Frete por conta do Destinatário (FOB)")]
        mfContaDestinatario = 1, 
        [Description("2=Contratação do Frete por conta de Terceiros")]
        mfContaTerceiros = 2, 
        [Description("3=Transporte Próprio por conta do Remetente")]
        mfTranspProprioContaRemetente = 3,
        [Description("4=Transporte Próprio por conta do Destinatário")]
        mfTranspProprioContaDestinatario = 4,
        [Description("9 = Sem Ocorrência de Transporte")]
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
        dbisPauta = 5,
        dbisValorOperacao = 6,
        NaoInserirTagNoXML = 100 //Quando a tag não é obrigatórioa, vamos retornar este valor para termos controle sobre a situação.
    }
    public enum TpcnOrigemMercadoria
    {
        [Description("0 - Nacional")]
        oeNacional = 0,
        [Description("1 - Estrangeira - Importação direta")]
        oeEstrangeiraImportacaoDireta = 1,
        [Description("2 - Estrangeira - Adquirida no mercado interno")]
        oeEstrangeiraAdquiridaBrasil = 2,
        [Description("3 - Nacional - Mercadoria ou bem com Conteúdo de Importação > 40 % e < 70 % ")]
		oeNacional_Mercadoria_ou_bem_com_Conteúdo_de_Importação_superior_a_40 = 3,
        [Description("4 - Nacional - Cuja produção tenha sido feita em conformidade com o PPB")]
        oeNacional_Cuja_produção_tenha_sido_feita_em_conformidade_com_o_PPB = 4,
        [Description("5 - Nacional - Mercadoria com bem ou conteúdo de importação inferior a 40%")]
        oeNacional_Mercadoria_com_bem_ou_conteúdo_de_importação_inferior_a_40 = 5,
        [Description("6 - Estrangeira - Importação direta, sem similar nacional")]
        oeEstrangeira_Importação_direta_sem_similar_nacional = 6,
        [Description("7 - Estrangeira - Adquirida no mercado interno com similar nacional.")]
        oeEstrangeira_Adquirida_no_mercado_interno_com_similar_nacional = 7,
        [Description("8 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 70%")]
        oeEstrangeira_Nacional_Conteúdo_de_Importação_superior_a_70 = 8
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
    public enum TpcnIndicadorEscala 
    { 
        [Description("Nenhum")]
        ieNenhum = ' ',
        [Description("S - Produzido em Escala Relevante")]
        ieNaoSomaTotalNFe = 'S', 
        [Description("N – Produzido em Escala NÃO Relevante")]
        ieSomaTotalNFe = 'N' 
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
        tcDec2 = 2, tcDec3 = 3, tcDec4 = 4, tcDec5 = 5, tcDec6 = 6, tcDec7 = 7, tcDec8 = 8, tcDec9 = 9, tcDec10 = 10,
        tcStr, tcInt, tcDatYYYY_MM_DD, tcDatYYYYMMDD, tcHor, tcDatHor
    }

    public enum TpcnIndicadorPagamento 
    { 
        ipNone = -1,
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
        tiNao = 0,
        tiRetrato = 1, 
        tiPaisagem = 2,
        tiDANFESimplificado = 3,
        tiDANFENFCe = 4,
        tiDANFENFCe_em_mensagem_eletrônica = 5
    }

    public enum TpcnFinalidadeNFe
    {
        fnNormal = 1,
        fnComplementar = 2,
        fnAjuste = 3,
        fnDevolucao = 4
    }

    public enum TpcnECFModRef 
    {
        ECFModRefVazio, 
        ECFModRef2B,
        ECFModRef2C,
        ECFModRef2D /*'', '2B', '2C','2D'*/
    }

    public enum tpEventos
    {
        [Description("Carta de Correcao")]
        tpEvCCe = 110110,
        [Description("Cancelamento")]
        tpEvCancelamentoNFe = 110111,
        [Description("Cancelamento por substituicao")]
        tpEvCancelamentoSubstituicaoNFCe = 110112,
        [Description("Confirmacao da Operacao")]
        tpEvConfirmacaoOperacao = 210200,
        [Description("Ciencia da Operacao")]
        tpEvCienciaOperacao = 210210,
        [Description("Desconhecimento da Operacao")]
        tpEvDesconhecimentoOperacao = 210220,
        [Description("EPEC")]
        tpEvEPEC = 110140,
        [Description("EPEC - CTe")]
        tpEvEPECCTe = 110113,
        [Description("Operação nao Realizada")]
        tpEvOperacaoNaoRealizada = 210240,
        [Description("Encerramento MDFe")]
        tpEvEncerramentoMDFe = 1101120, //O código correto do evento é 110112, mas como tem o evento de cancelamento por substituição da nfce que tem o mesmo número acrescentamos um zero no final para não gerar conflito. Wandrey/André
        [Description("Inclusao de condutor")]
        tpEvInclusaoCondutor = 110114,
        [Description("Registro de passagem")]
        tpEvRegistroPassagem = 310620,
        [Description("Registro de passagem-BRid")]
        tpEvRegistroPassagemBRid = 510620,
        [Description("Registro Multimodal")]
        tpevRegMultimodal = 110160,
        [Description("Pedido de prorrogação 1º. prazo")]
        tpEvPedProrrogacao_ICMS_1 = 111500,
        [Description("Pedido de prorrogação 2º. prazo")]
        tpEvPedProrrogacao_ICMS_2 = 111501,
        [Description("Cancelamento de Pedido de Prorrogação 1º. Prazo")]
        tpEvCancPedProrrogacao_ICMS_1 = 111502,
        [Description("Cancelamento de Pedido de Prorrogação 2º. Prazo")]
        tpEvCancPedProrrogacao_ICMS_2 = 111503,
        [Description("Fisco Resposta ao Pedido de Prorrogação 1º prazo")]
        tpEvFiscoRespPedProrrogacao_ICMS_1 = 411500,
        [Description("Fisco Resposta ao Pedido de Prorrogação 2º prazo")]
        tpEvFiscoRespPedProrrogacao_ICMS_2 = 411501,
        [Description("Fisco Resposta ao Cancelamento de Prorrogação 1º prazo")]
        tpEvFiscoRespCancPedProrrogacao_ICMS_1 = 411502,
        [Description("Fisco Resposta ao Cancelamento de Prorrogação 2º prazo")]
        tpEvFiscoRespCancPedProrrogacao_ICMS_2 = 411503
    }

    public enum TpcnTipoAutor
    {
        taEmpresaEmitente = 1,
        taEmpresaDestinataria = 2,
        taEmpresa = 3,
        taFisco = 5,
        taRFB = 6,
        taOutros = 9
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
        Opcional, 
        None
    }

    public enum TpcnindIEDest {
        inContribuinte=1, 
        inIsento=2, 
        inNaoContribuinte=9
    }

    public enum TpcnMod
    {
        modNFe = 55,
        modNFCe = 65,
        modCTe = 57,
        modMDFe = 58,
        modIntefinido = 99
    }

    public enum TpcnTipoViaTransp
    {
        [Description("Marítima")]
        tvMaritima = 1,
        [Description("Fluvial")]
        tvFluvial = 2,
        [Description("Lacustre")]
        tvLacustre = 3,
        [Description("Aerea")]
        tvAerea = 4,
        [Description("Postal")]
        tvPostal = 5,
        [Description("Ferroviária")]
        tvFerroviaria = 6,
        [Description("Rodoviária")]
        tvRodoviaria = 7,
        [Description("Conduto/Rede Transmissão")]
        tvConduto = 8,
        [Description("Meios próprios")]
        tvMeiosProprios = 9,
        [Description("Entrada/Saida ficta")]
        tvEntradaSaidaFicta = 10
    }

    public enum TpcnTipoIntermedio
    {
        [Description("Importação por conta própria")]
        tiContaPropria = 1,
        [Description("Importação por conta e ordem")]
        tiContaOrdem = 2,
        [Description("Importação por encomenda")]
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
        Nenhum = 0,
        Microempresa_Municipal = 1,
        Estimativa = 2,
        Sociedade_de_Profissionais = 3,
        Cooperativa = 4,
        Microempresário_Individual__MEI = 5,
        Microempresário_e_Empresa_de_Pequeno_Porte__ME_EPP = 6
    }
}

using System.Xml.Serialization;

namespace Unimake.Business.DFe.Servicos
{
    #region Servico
    /// <summary>
    /// Serviços disponíveis
    /// </summary>
    public enum Servico
    {
        #region NFe

        /// <summary>
        /// Consulta status serviço NFe/NFCe
        /// </summary>
        NFeStatusServico,

        /// <summary>
        /// Consulta protocolo da NFe/NFCe
        /// </summary>
        NFeConsultaProtocolo,

        /// <summary>
        /// Consulta recibo NFe/NFCe
        /// </summary>
        NFeConsultaRecibo,

        /// <summary>
        /// Inutilização de números da nota fiscal eletrônica
        /// </summary>
        NFeInutilizacao,

        /// <summary>
        /// Consulta cadastro do contribuinte
        /// </summary>
        NFeConsultaCadastro,

        /// <summary>
        /// Envio de Eventos (Cancelamento, CCe, EPEC, etc...)
        /// </summary>
        NFeRecepcaoEvento,

        /// <summary>
        /// Envio do XML de lote de NFe/NFCe
        /// </summary>
        NFeAutorizacao,

        /// <summary>
        /// Envio do XML de consulta dos documentos fiscais eletrônicos destinados
        /// </summary>
        NFeDistribuicaoDFe,

        /// <summary>
        /// Serviço não definido
        /// </summary>
        Nulo

        #endregion
    }
    #endregion

    #region DFE
    /// <summary>
    /// Tipos de DFe´s existentes
    /// </summary>
    public enum DFE
    {
        /// <summary>
        /// NF-e - Nota Fiscal Eletrônica
        /// </summary>
        NFe,
        /// <summary>
        /// NFC-e - Nota Fiscal de Venda a Consumidor Eletrônica
        /// </summary>
        NFCe,
        /// <summary>
        /// CT-e - Conhecimento de Transporte Eletrônico
        /// </summary>
        CTe,
        /// <summary>
        /// MDF-e - Manifesto Eletrônico de Documentos Fiscais
        /// </summary>
        MDFe,
        /// <summary>
        /// Nota Fiscal de Serviço Eletrônica
        /// </summary>
        NFSe,
        /// <summary>
        /// CFe-SAT - Sistema Autenticador e Transmissor de Cupons Fiscais Eletrônicos
        /// </summary>
        SAT
    }
    #endregion       

    #region UF

    /// <summary>
    /// Unidades Federativas do Brasil (Tem como XmlEnum o nome abreviado da UF)
    /// </summary>
    public enum UFBrasil
    {
        /// <summary>
        /// Acre - AC (12)
        /// </summary>
        AC = 12,

        /// <summary>
        /// Alagoas - AL (27)
        /// </summary>
        AL = 27,

        /// <summary>
        /// Amapá - AP (16)
        /// </summary>
        AP = 16,

        /// <summary>
        /// Amazonas - AM (13)
        /// </summary>
        AM = 13,

        /// <summary>
        /// Bahia - BA (29)
        /// </summary>
        BA = 29,

        /// <summary>
        /// Ceará - CE (23)
        /// </summary>
        CE = 23,

        /// <summary>
        /// Distrito Federal - DF (53)
        /// </summary>
        DF = 53,

        /// <summary>
        /// Espírito Santo - ES (32)
        /// </summary>
        ES = 32,

        /// <summary>
        /// Exportação
        /// </summary>
        EX = 99,

        /// <summary>
        /// Goiás - GO (52)
        /// </summary>
        GO = 52,

        /// <summary>
        /// Maranhão - MA (21)
        /// </summary>
        MA = 21,

        /// <summary>
        /// Mato Grosso - MT (51)
        /// </summary>
        MT = 51,

        /// <summary>
        /// Mato Grosso do Sul - MS (50)
        /// </summary>
        MS = 50,

        /// <summary>
        /// Minas Gerais - MG (31)
        /// </summary>
        MG = 31,

        /// <summary>
        /// Pará - PA (15)
        /// </summary>
        PA = 15,

        /// <summary>
        /// Paraíba - PB (25)
        /// </summary>
        PB = 25,

        /// <summary>
        /// Paraná - PR (41)
        /// </summary>
        PR = 41,

        /// <summary>
        /// Pernambuco - PE (26)
        /// </summary>
        PE = 26,

        /// <summary>
        /// Piauí - PI (22)
        /// </summary>
        PI = 22,

        /// <summary>
        /// Rio de Janeiro - RJ (33)
        /// </summary>
        RJ = 33,

        /// <summary>
        /// Rio Grande do Norte - RN (24)
        /// </summary>
        RN = 24,

        /// <summary>
        /// Rio Grande do Sul - RS (43)
        /// </summary>
        RS = 43,

        /// <summary>
        /// Rondônia - RO (11)
        /// </summary>
        RO = 11,

        /// <summary>
        /// Roraima - RR (14)
        /// </summary>
        RR = 14,

        /// <summary>
        /// Santa Catarina - SC (42)
        /// </summary>
        SC = 42,

        /// <summary>
        /// São Paulo - SP (35)
        /// </summary>
        SP = 35,

        /// <summary>
        /// Sergipe - SE (28)
        /// </summary>
        SE = 28,

        /// <summary>
        /// Tocantins - TO (17)
        /// </summary>
        TO = 17,

        /// <summary>
        /// Ambiente Nacional - AN (91)
        /// </summary>
        AN = 91
    }

    #endregion

    #region TipoAmbiente

    /// <summary>
    /// Tipo ambiente DFe (NFe, CTe, MDFe, NFCe, etc...)
    /// </summary>
    public enum TipoAmbiente
    {
        [XmlEnum("1")]
        Producao = 1,
        [XmlEnum("2")]
        Homologacao = 2
    }

    #endregion

    #region ModeloDFe

    /// <summary>
    /// Modelos dos DFes (NFe, CTe, MDFe, NFCe, etc...)
    /// </summary>
    public enum ModeloDFe
    {
        /// <summary>
        /// NF-e (Modelo: 55)
        /// </summary>
        [XmlEnum("55")]
        NFe = 55,
        /// <summary>
        /// NFC-e (Modelo: 65)
        /// </summary>
        [XmlEnum("65")]
        NFCe = 65,
        /// <summary>
        /// CT-e (Modelo: 57)
        /// </summary>
        [XmlEnum("57")]
        CTe = 57,
        /// <summary>
        /// MDF-e (Modelo: 58)
        /// </summary>
        [XmlEnum("58")]
        MDFe = 58,
        /// <summary>
        /// CTeOS (Modelo: 67)
        /// </summary>
        [XmlEnum("67")]
        CTeOS = 67
    }

    #endregion

    #region TipoEventoNFe

    /// <summary>
    /// Tipos de eventos da NFe e NFCe
    /// </summary>
    public enum TipoEventoNFe
    {
        /// <summary>
        /// Carta de correção eletrônica (110110)
        /// </summary>
        [XmlEnum("110110")]
        CartaCorrecao = 110110,
        /// <summary>
        /// Cancelamento NFe (110111)
        /// </summary>
        [XmlEnum("110111")]
        Cancelamento = 110111,
        /// <summary>
        /// Cancelamento da NFCe sendo substituida por outra NFCe (110112)
        /// </summary>
        [XmlEnum("110112")]
        CancelamentoPorSubstituicao = 110112,
        /// <summary>
        /// EPEC - Evento Prévio de Emissão em Contingência (110140)
        /// </summary>
        [XmlEnum("110140")]
        EPEC = 110140,
        /// <summary>
        /// Pedido de prorrogação do prazo de ICMS no caso de remessa para industrialização (111500)
        /// </summary>
        [XmlEnum("111500")]
        PedidoProrrogacao = 111500,
        /// <summary>
        /// Manifestação do Destinatário - Confirmação da Operação (210200)
        /// </summary>
        [XmlEnum("210200")]
        ManifestacaoConfirmacaoOperacao = 210200,
        /// <summary>
        /// Manifestação do Destinatário - Ciência da Operação (210210)
        /// </summary>
        [XmlEnum("210210")]
        ManifestacaoCienciaOperacao = 210210,
        /// <summary>
        /// Manifestação do Destinatário - Desconhecimento da Operação (210220)
        /// </summary>
        [XmlEnum("210220")]
        ManifestacaoDesconhecimentoOperacao = 210220,
        /// <summary>
        /// Manifestação do Destinatário - Operação não realizada (210240)
        /// </summary>
        [XmlEnum("210240")]
        ManifestacaoOperacaoNaoRealizada = 210240
    }

    #endregion

    #region SimNao

    /// <summary>
    /// Sim ou Não (1 ou 0)
    /// </summary>
    public enum SimNao
    {
        /// <summary>
        /// Não (9)
        /// </summary>
        [XmlEnum("0")]
        Não = 0,

        /// <summary>
        /// Sim (1)
        /// </summary>
        [XmlEnum("1")]
        Sim = 1
    }

    #endregion

    #region TipoOperacao

    /// <summary>
    /// Tipo de operação (Entrada ou Saída)
    /// </summary>
    public enum TipoOperacao
    {
        /// <summary>
        /// Operação de entrada
        /// </summary>
        [XmlEnum("0")]
        Entrada = 0,
        /// <summary>
        /// Operação de saída
        /// </summary>
        [XmlEnum("1")]
        Saida = 1
    }

    #endregion

    #region DestinoOperacao

    /// <summary>
    /// Identificador do Destino da Operação
    /// </summary>
    public enum DestinoOperacao
    {
        /// <summary>
        /// Operação interna, ou seja, dentro do estado de origem
        /// </summary>
        [XmlEnum("1")]
        OperacaoInterna = 1,

        /// <summary>
        /// Operação interestadual, ou seja, estado diferente do de origem
        /// </summary>
        [XmlEnum("2")]
        OperacaoInterestadual = 2,

        /// <summary>
        /// Operação com o exterior, ou seja, fora do país de origem
        /// </summary>
        [XmlEnum("3")]
        OperacaoExterior = 3
    }

    #endregion

    #region FormatoImpressaoDANFE

    /// <summary>
    /// Formato de impressão do DANFE
    /// </summary>
    public enum FormatoImpressaoDANFE
    {
        /// <summary>
        /// 0=Sem geração de DANFE
        /// </summary>
        [XmlEnum("0")]
        SemGeracao = 0,

        /// <summary>
        /// 1=DANFE normal, Retrato
        /// </summary>
        [XmlEnum("1")]
        NormalRetrato = 1,

        /// <summary>
        /// 2=DANFE normal, Paisagem
        /// </summary>
        [XmlEnum("2")]
        NormalPaisagem = 2,

        /// <summary>
        /// 3=DANFE Simplificado
        /// </summary>
        [XmlEnum("3")]
        Simplificado = 3,

        /// <summary>
        /// 4=DANFE NFC-e
        /// </summary>
        [XmlEnum("4")]
        NFCe = 4,

        /// <summary>
        /// 5=DANFE NFC-e em mensagem eletrônica
        /// </summary>
        [XmlEnum("5")]
        NFCeMensagemEletronica = 5
    }

    #endregion

    #region TipoEmissao

    /// <summary>
    /// Tipo de emissão do DF-e (NFe, NFCe, CTe, MDFe, etc...)
    /// </summary>
    public enum TipoEmissao
    {
        /// <summary>
        /// 1=Emissão normal (não em contingência)
        /// </summary>
        [XmlEnum("1")]
        Normal = 1,

        /// <summary>
        /// 2=Contingência FS-IA, com impressão do DANFE em formulário de segurança
        /// </summary>
        [XmlEnum("2")]
        ContingenciaFSIA = 2,

        /// <summary>
        /// 4=Contingência DPEC (Declaração Prévia da Emissão em Contingência);
        /// </summary>
        [XmlEnum("4")]
        ContingenciaDPEC = 4,

        /// <summary>
        /// 5=Contingência FS-DA, com impressão do DANFE em formulário de segurança;
        /// </summary>
        [XmlEnum("5")]
        ContingenciaFSDA = 5,

        /// <summary>
        /// 6=Contingência SVC-AN (SEFAZ Virtual de Contingência do AN);
        /// </summary>
        [XmlEnum("6")]
        ContingenciaSVCAN = 6,

        /// <summary>
        /// 7=Contingência SVC-RS (SEFAZ Virtual de Contingência do RS);
        /// </summary>
        [XmlEnum("7")]
        ContingenciaSVCRS = 7,

        /// <summary>
        /// 9=Contingência off-line da NFC-e
        /// </summary>
        [XmlEnum("9")]
        ContingenciaOffLine = 9
    }

    #endregion

    #region FinalidadeNFe

    /// <summary>
    /// Finalidades da NFe/NFCe
    /// </summary>
    public enum FinalidadeNFe
    {
        /// <summary>
        /// 1=NF-e normal
        /// </summary>
        [XmlEnum("1")]
        Normal = 1,

        /// <summary>
        /// 2=NF-e complementar
        /// </summary>
        [XmlEnum("2")]
        Complementar = 2,

        /// <summary>
        /// 3=NF-e de ajuste
        /// </summary>
        [XmlEnum("3")]
        Auste = 3,

        /// <summary>
        /// 4=Devolução de mercadoria
        /// </summary>
        [XmlEnum("4")]
        Devolucao = 4
    }

    #endregion

    #region IndicadorPresenca

    /// <summary>
    /// Indicador de presença do comprador no estabelecimento comercial no momento da operação
    /// </summary>
    public enum IndicadorPresenca
    {
        /// <summary>
        /// 0=Não se aplica (por exemplo, Nota Fiscal complementar ou de ajuste)
        /// </summary>
        [XmlEnum("0")]
        NaoSeAplica = 0,

        /// <summary>
        /// 1=Operação presencial
        /// </summary>
        [XmlEnum("1")]
        OperacaoPresencial = 1,

        /// <summary>
        /// 2=Operação não presencial, pela Internet
        /// </summary>
        [XmlEnum("2")]
        OperacaoInternet = 2,

        /// <summary>
        /// 3=Operação não presencial, Teleatendimento
        /// </summary>
        [XmlEnum("3")]
        OperacaoTeleAtendimento = 3,

        /// <summary>
        /// 4=NFC-e em operação com entrega a domicílio
        /// </summary>
        [XmlEnum("4")]
        NFCeEntregaDomicilio = 4,

        /// <summary>
        /// 9=Operação não presencial, outros
        /// </summary>
        [XmlEnum("9")]
        OperacaoOutros = 9
    }

    #endregion

    #region ProcessoEmissao

    /// <summary>
    /// Processo de emissão do DFe (NFe, NFCe, etc...)
    /// </summary>
    public enum ProcessoEmissao
    {
        /// <summary>
        /// 0=Emissão de NF-e com aplicativo do contribuinte
        /// </summary>
        [XmlEnum("0")]
        AplicativoContribuinte = 0,

        /// <summary>
        /// 1=Emissão de NF-e avulsa pelo Fisco;
        /// </summary>
        [XmlEnum("1")]
        AvulsaPeloFisco = 1,

        /// <summary>
        /// 2=Emissão de NF-e avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco;
        /// </summary>
        [XmlEnum("2")]
        AvulsaPeloContribuinteSiteFisco = 2,

        /// <summary>
        /// 3=Emissão NF-e pelo contribuinte com aplicativo fornecido pelo Fisco.
        /// </summary>
        [XmlEnum("3")]
        AplicativoFisco = 3
    }

    #endregion

    #region CRT

    /// <summary>
    /// Códigos de regimes tributários
    /// </summary>
    public enum CRT
    {
        /// <summary>
        /// 1=Simples Nacional
        /// </summary>
        [XmlEnum("1")]
        SimplesNacional = 1,

        /// <summary>
        /// 2=Simples Nacional, excesso sublimite de receita bruta
        /// </summary>
        [XmlEnum("2")]
        SimplesNacionalExcessoSublimite = 2,

        /// <summary>
        /// 3=Regime Normal
        /// </summary>
        [XmlEnum("3")]
        RegimeNormal = 3
    }

    #endregion

    #region IndicadorIEDestinatario

    /// <summary>
    /// Indicador da IE do Destinatário
    /// </summary>
    public enum IndicadorIEDestinatario
    {
        /// <summary>
        /// 1=Contribuinte ICMS (informar a IE do destinatário)
        /// </summary>
        [XmlEnum("1")]
        ContribuinteICMS = 1,

        /// <summary>
        /// 2=Contribuinte isento de Inscrição no cadastro de Contribuintes do ICMS
        /// </summary>
        [XmlEnum("2")]
        ContribuinteIsento = 2,

        /// <summary>
        /// 9=Não Contribuinte, que pode ou não possuir Inscrição Estadual no Cadastro de Contribuintes do ICMS
        /// </summary>
        [XmlEnum("9")]
        NaoContribuinte = 9
    }

    #endregion

    #region IndicadorEscalaRelevante

    /// <summary>
    /// Indicador de Escala de Relevante
    /// </summary>
    public enum IndicadorEscalaRelevante
    {
        /// <summary>
        /// S - Produzido em Escala Relevante
        /// </summary>
        [XmlEnum("S")]
        Sim,

        /// <summary>
        /// N – Produzido em Escala NÃO Relevante
        /// </summary>
        [XmlEnum("N")]
        Nao
    }

    #endregion

    #region ViaTransporteInternacional

    /// <summary>
    /// Via Transporte Internacional Informada na Declaração de Importação
    /// </summary>
    public enum ViaTransporteInternacional
    {
        /// <summary>
        /// 1=Marítima
        /// </summary>
        [XmlEnum("1")]
        Maritima = 1,

        /// <summary>
        /// 2=Fluvial
        /// </summary>
        [XmlEnum("2")]
        Fluvial = 2,

        /// <summary>
        /// 3=Lacustre
        /// </summary>
        [XmlEnum("3")]
        Lacustre = 3,

        /// <summary>
        /// 4=Aérea
        /// </summary>
        [XmlEnum("4")]
        Aerea = 4,

        /// <summary>
        /// 5=Postal
        /// </summary>
        [XmlEnum("5")]
        Postal = 5,

        /// <summary>
        /// 6=Ferroviária
        /// </summary>
        [XmlEnum("6")]
        Ferroviaria = 6,

        /// <summary>
        /// 7=Rodoviária
        /// </summary>
        [XmlEnum("7")]
        Rodoviaria = 7,

        /// <summary>
        /// 8=Conduto / Rede Transmissão
        /// </summary>
        [XmlEnum("8")]
        CondutoRedeTransmissao = 8,

        /// <summary>
        /// 9=Meios Próprios
        /// </summary>
        [XmlEnum("9")]
        MeiosProprios = 9,

        /// <summary>
        /// 10=Entrada / Saída ficta
        /// </summary>
        [XmlEnum("10")]
        EntradaSaidaFicta = 10,

        /// <summary>
        /// 11=Courier
        /// </summary>
        [XmlEnum("11")]
        Courier = 11,

        /// <summary>
        /// 12=Handcarry
        /// </summary>
        [XmlEnum("12")]
        Handcarry = 12
    }

    #endregion

    #region FormaImportacaoIntermediacao

    /// <summary>
    /// Forma de importação quanto a intermediação
    /// </summary>
    public enum FormaImportacaoIntermediacao
    {
        /// <summary>
        /// 1=Importação por conta própria
        /// </summary>
        [XmlEnum("1")]
        ImportacaoPorContaPropria = 1,

        /// <summary>
        /// 2=Importação por conta e ordem
        /// </summary>
        [XmlEnum("2")]
        ImportacaoPorContaOrdem = 2,

        /// <summary>
        /// 3=Importação por encomenda
        /// </summary>
        [XmlEnum("3")]
        ImportacaoPorEncomenda = 3
    }

    #endregion

    #region OrigemMercadoria

    /// <summary>
    /// Origens das mercadorias
    /// </summary>
    public enum OrigemMercadoria
    {
        /// <summary>
        /// 0 - Nacional, exceto as indicadas nos códigos 3, 4, 5 e 8;
        /// </summary>
        [XmlEnum("0")]
        Nacional = 0,

        /// <summary>
        /// 1 - Estrangeira - Importação direta, exceto a indicada no código 6;
        /// </summary>
        [XmlEnum("1")]
        Estrangeira = 1,

        /// <summary>
        /// 2 - Estrangeira - Adquirida no mercado interno, exceto a indicada no código 7;
        /// </summary>
        [XmlEnum("2")]
        Estrangeira2 = 2,

        /// <summary>
        /// 3 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 40% e inferior ou igual a 70%;
        /// </summary>
        [XmlEnum("3")]
        Nacional3 = 3,

        /// <summary>
        /// 4 - Nacional, cuja produção tenha sido feita em conformidade com os processos produtivos básicos de que tratam as legislações citadas nos Ajustes;
        /// </summary>
        [XmlEnum("4")]
        Nacional4 = 4,

        /// <summary>
        /// 5 - Nacional, mercadoria ou bem com Conteúdo de Importação inferior ou igual a 40%;
        /// </summary>
        [XmlEnum("5")]
        Nacional5 = 5,

        /// <summary>
        /// 6 - Estrangeira - Importação direta, sem similar nacional, constante em lista da CAMEX e gás natural;
        /// </summary>
        [XmlEnum("6")]
        Estrangeira6 = 6,

        /// <summary>
        /// 7 - Estrangeira - Adquirida no mercado interno, sem similar nacional, constante lista CAMEX e gás natural.
        /// </summary>
        [XmlEnum("7")]
        Estrangeira7 = 7,

        /// <summary>
        /// 8 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 70%;
        /// </summary>
        [XmlEnum("8")]
        Nacional8 = 8
    }

    #endregion

    #region ModalidadeBaseCalculoICMS

    /// <summary>
    /// Modalidades de determinação da Base de Cálculo do ICMS
    /// </summary>
    public enum ModalidadeBaseCalculoICMS
    {
        /// <summary>
        /// 0=Margem Valor Agregado (%)
        /// </summary>
        [XmlEnum("0")]
        MargemValorAgregado = 0,

        /// <summary>
        /// 1=Pauta (Valor)
        /// </summary>
        [XmlEnum("1")]
        Pauta = 1,

        /// <summary>
        /// 2=Preço Tabelado Máx. (valor)
        /// </summary>
        [XmlEnum("2")]
        PrecoTabeladoMaximo = 2,

        /// <summary>
        /// 3=Valor da operação
        /// </summary>
        [XmlEnum("3")]
        ValorOperacao = 3
    }

    #endregion

    #region ModalidadeBaseCalculoICMSST

    /// <summary>
    /// Modalidade de determinação da Basse de Cálculo do ICMS ST
    /// </summary>
    public enum ModalidadeBaseCalculoICMSST
    {
        /// <summary>
        /// 0=Preço tabelado ou máximo sugerido
        /// </summary>
        [XmlEnum("0")]
        PrecoTabeladoMaximoSugerido = 0,

        /// <summary>
        /// 1=Lista Negativa (valor)
        /// </summary>
        [XmlEnum("1")]
        ListaNegativa = 1,

        /// <summary>
        /// 2=Lista Positiva (valor)
        /// </summary>
        [XmlEnum("2")]
        ListaPositiva = 2,

        /// <summary>
        /// 3=Lista Neutra (valor)
        /// </summary>
        [XmlEnum("3")]
        ListaNeutra = 3,

        /// <summary>
        /// 4=Margem Valor Agregado (%)
        /// </summary>
        [XmlEnum("4")]
        MargemValorAgregado = 4,

        /// <summary>
        /// 5=Pauta (valor)
        /// </summary>
        [XmlEnum("5")]
        Pauta = 5,

        /// <summary>
        /// 6=Valor da Operação
        /// </summary>
        [XmlEnum("6")]
        ValorOperacao = 6
    }

    #endregion

    #region IndicadorOrigemProcesso

    /// <summary>
    /// Indicador da Origem do Processo
    /// </summary>
    public enum IndicadorOrigemProcesso
    {
        /// <summary>
        /// 0=SEFAZ
        /// </summary>
        [XmlEnum("0")]
        SEFAZ = 0,

        /// <summary>
        /// 1=Justiça Federal
        /// </summary>
        [XmlEnum("1")]
        JusticaFederal = 1,

        /// <summary>
        /// 2=Justiça Estadual
        /// </summary>
        [XmlEnum("2")]
        JusticaEstadual = 2,

        /// <summary>
        /// 3=Secex/RFB
        /// </summary>
        [XmlEnum("3")]
        SecexRFB = 3,

        /// <summary>
        /// 9=Outros
        /// </summary>
        [XmlEnum("9")]
        Outros = 9
    }

    #endregion

    #region MotivoDesoneracaoICMS

    public enum MotivoDesoneracaoICMS
    {
        /// <summary>
        /// 3=Uso na agropecuária
        /// </summary>
        [XmlEnum("3")]
        UsoAgropecuaria = 3,

        /// <summary>
        /// 9=Outros
        /// </summary>
        [XmlEnum("9")]
        Outro = 9,

        /// <summary>
        /// 12=Órgão de fomento e desenvolvimento agropecuário
        /// </summary>
        [XmlEnum("12")]
        OrgaoFomentoDesenvolvimentoAgropecuario = 12
    }

    #endregion

    #region ModalidadeFrete

    public enum ModalidadeFrete
    {
        /// <summary>
        /// 0=Contratação do Frete por conta do Remetente (CIF); 
        /// </summary>
        [XmlEnum("0")]
        ContratacaoFretePorContaRemetente_CIF = 0,

        /// <summary>
        /// 1=Contratação do Frete por conta do Destinatário (FOB)
        /// </summary>
        [XmlEnum("1")]
        ContratacaoFretePorContaDestinatário_FOB = 1,

        /// <summary>
        /// 2=Contratação do Frete por conta de Terceiros
        /// </summary>
        [XmlEnum("2")]
        ContratacaoFretePorContaTerceiros = 2,

        /// <summary>
        /// 3=Transporte Próprio por conta do Remetente
        /// </summary>
        [XmlEnum("3")]
        TransporteProprioPorContaRemetente = 3,

        /// <summary>
        /// 4=Transporte Próprio por conta do Destinatário
        /// </summary>
        [XmlEnum("4")]
        TransporteProprioPorContaDestinatário = 4,

        /// <summary>
        /// 9=Sem Ocorrência de Transporte
        /// </summary>
        [XmlEnum("9")]
        SemOcorrenciaTransporte = 9
    }

    #endregion

    #region TipoIntegracaoPagamento

    /// <summary>
    /// Tipo de Integração do processo de pagamento com o sistema de automação da empresa
    /// </summary>
    public enum TipoIntegracaoPagamento
    {
        /// <summary>
        /// 1=Pagamento integrado com o sistema de automação da empresa (Ex.: equipamento TEF, Comércio Eletrônico)
        /// </summary>
        [XmlEnum("1")]
        PagamentoIntegrado = 1,
        /// <summary>
        /// 2= Pagamento não integrado com o sistema de automação da empresa (Ex.: equipamento POS)
        /// </summary>
        [XmlEnum("2")]
        PagamentoNaoIntegrado = 2
    }

    #endregion

    #region BandeiraOperadoraCartao

    /// <summary>
    /// Bandeira da operadora de cartão de crédito e/ou débito
    /// </summary>    
    public enum BandeiraOperadoraCartao
    {
        /// <summary>
        /// 01=Visa
        /// </summary>
        [XmlEnum("01")]
        Visa = 1,

        /// <summary>
        /// 02=Mastercard
        /// </summary>
        [XmlEnum("02")]
        Mastercard = 2,

        /// <summary>
        /// 03=American Express
        /// </summary>
        [XmlEnum("03")]
        AmericanExpress = 3,

        /// <summary>
        /// 04=Sorocred
        /// </summary>
        [XmlEnum("04")]
        Sorocred = 4,

        /// <summary>
        /// 05=Diners Club
        /// </summary>
        [XmlEnum("05")]
        DinersClub = 5,

        /// <summary>
        /// 06=Elo
        /// </summary>
        [XmlEnum("06")]
        Elo = 6,

        /// <summary>
        /// 07=Hipercard
        /// </summary>
        [XmlEnum("07")]
        Hipercard = 7,

        /// <summary>
        /// 08=Aura
        /// </summary>
        [XmlEnum("08")]
        Aura = 8,

        /// <summary>
        /// 09=Cabal
        /// </summary>
        [XmlEnum("09")]
        Cabal = 9,

        /// <summary>
        /// 99=Outros
        /// </summary>
        [XmlEnum("99")]
        Outros = 99
    }

    #endregion

    #region MeioPagamento

    public enum MeioPagamento
    {
        /// <summary>
        /// 01=Dinheiro
        /// </summary>
        [XmlEnum("01")]
        Dinheiro = 1,

        /// <summary>
        /// 02=Cheque
        /// </summary>
        [XmlEnum("02")]
        Cheque = 2,

        /// <summary>
        /// 03=Cartão de Crédito
        /// </summary>
        [XmlEnum("03")]
        CartaoCredito = 3,

        /// <summary>
        /// 04=Cartão de Débito
        /// </summary>
        [XmlEnum("04")]
        CartaoDebito = 4,

        /// <summary>
        /// 05=Crédito Loja
        /// </summary>
        [XmlEnum("05")]
        CreditoLoja = 5,

        /// <summary>
        /// 10=Vale Alimentação
        /// </summary>
        [XmlEnum("10")]
        ValeAlimentacao = 10,

        /// <summary>
        /// 11=Vale Refeição
        /// </summary>
        [XmlEnum("11")]
        ValeRefeicao = 11,

        /// <summary>
        /// 12=Vale Presente
        /// </summary>
        [XmlEnum("12")]
        ValePresente = 12,

        /// <summary>
        /// 13=Vale Combustível
        /// </summary>
        [XmlEnum("13")]
        ValeCombustivel = 13,

        /// <summary>
        /// 15=Boleto Bancário
        /// </summary> 
        [XmlEnum("15")]
        BoletoBancario = 15,

        /// <summary>
        /// 90=Sem pagamento
        /// </summary>
        [XmlEnum("90")]
        Sempagamento = 90,

        /// <summary>
        /// 99=Outros
        /// </summary>
        [XmlEnum("99")]
        Outros = 99
    }

    #endregion

    #region IndicadorPagamento

    public enum IndicadorPagamento
    {
        /// <summary>
        /// Pagamento à Vista
        /// </summary>
        [XmlEnum("0")]
        PagamentoVista = 0,

        /// <summary>
        /// Pagamento à Prazo
        /// </summary>
        [XmlEnum("1")]
        PagamentoPrazo = 1
    }

    #endregion

    #region Lista de serviços

    /// <summary>
    /// Lista de Serviços do ISSQN
    /// </summary>
    public enum ListaServicoISSQN
    {
        /// <summary>
        /// Serviço 01.01
        /// </summary>
        [XmlEnum("01.01")]
        Servico0101 = 0101,

        /// <summary>
        /// Serviço 01.02
        /// </summary>
        [XmlEnum("01.02")]
        Servico0102 = 0102,

        /// <summary>
        /// Serviço 01.03
        /// </summary>
        [XmlEnum("01.03")]
        Servico0103 = 0103,

        /// <summary>
        /// Serviço 01.04
        /// </summary>
        [XmlEnum("01.04")]
        Servico0104 = 0104,

        /// <summary>
        /// Serviço 01.05
        /// </summary>
        [XmlEnum("01.05")]
        Servico0105 = 0105,

        /// <summary>
        /// Serviço 01.06
        /// </summary>
        [XmlEnum("01.06")]
        Servico0106 = 0106,

        /// <summary>
        /// Serviço 01.07
        /// </summary>
        [XmlEnum("01.07")]
        Servico0107 = 0107,

        /// <summary>
        /// Serviço 01.08
        /// </summary>
        [XmlEnum("01.08")]
        Servico0108 = 0108,

        /// <summary>
        /// Serviço 01.09
        /// </summary>
        [XmlEnum("01.09")]
        Servico0109 = 0109,

        /// <summary>
        /// Serviço 02.01
        /// </summary>
        [XmlEnum("02.01")]
        Servico0201 = 0201,

        /// <summary>
        /// Serviço 03.02
        /// </summary>
        [XmlEnum("03.02")]
        Servico0302 = 0302,

        /// <summary>
        /// Serviço 03.03
        /// </summary>
        [XmlEnum("03.03")]
        Servico0303 = 0303,

        /// <summary>
        /// Serviço 03.04
        /// </summary>
        [XmlEnum("03.04")]
        Servico0304 = 0304,

        /// <summary>
        /// Serviço 03.05
        /// </summary>
        [XmlEnum("03.05")]
        Servico0305 = 0305,

        /// <summary>
        /// Serviço 04.01
        /// </summary>
        [XmlEnum("04.01")]
        Servico0401 = 0401,

        /// <summary>
        /// Serviço 04.02
        /// </summary>
        [XmlEnum("04.02")]
        Servico0402 = 0402,

        /// <summary>
        /// Serviço 04.03
        /// </summary>
        [XmlEnum("04.03")]
        Servico0403 = 0403,

        /// <summary>
        /// Serviço 04.04
        /// </summary>
        [XmlEnum("04.04")]
        Servico0404 = 0404,

        /// <summary>
        /// Serviço 04.05
        /// </summary>
        [XmlEnum("04.05")]
        Servico0405 = 0405,

        /// <summary>
        /// Serviço 04.06
        /// </summary>
        [XmlEnum("04.06")]
        Servico0406 = 0406,

        /// <summary>
        /// Serviço 04.07
        /// </summary>
        [XmlEnum("04.07")]
        Servico0407 = 0407,

        /// <summary>
        /// Serviço 04.08
        /// </summary>
        [XmlEnum("04.08")]
        Servico0408 = 0408,

        /// <summary>
        /// Serviço 04.09
        /// </summary>
        [XmlEnum("04.09")]
        Servico0409 = 0409,

        /// <summary>
        /// Serviço 04.10
        /// </summary>
        [XmlEnum("04.10")]
        Servico0410 = 0410,

        /// <summary>
        /// Serviço 04.11
        /// </summary>
        [XmlEnum("04.11")]
        Servico0411 = 0411,

        /// <summary>
        /// Serviço 04.12
        /// </summary>
        [XmlEnum("04.12")]
        Servico0412 = 0412,

        /// <summary>
        /// Serviço 04.13
        /// </summary>
        [XmlEnum("04.13")]
        Servico0413 = 0413,

        /// <summary>
        /// Serviço 04.14
        /// </summary>
        [XmlEnum("04.14")]
        Servico0414 = 0414,

        /// <summary>
        /// Serviço 04.15
        /// </summary>
        [XmlEnum("04.15")]
        Servico0415 = 0415,

        /// <summary>
        /// Serviço 04.16
        /// </summary>
        [XmlEnum("04.16")]
        Servico0416 = 0416,

        /// <summary>
        /// Serviço 04.17
        /// </summary>
        [XmlEnum("04.17")]
        Servico0417 = 0417,

        /// <summary>
        /// Serviço 04.18
        /// </summary>
        [XmlEnum("04.18")]
        Servico0418 = 0418,

        /// <summary>
        /// Serviço 04.19
        /// </summary>
        [XmlEnum("04.19")]
        Servico0419 = 0419,

        /// <summary>
        /// Serviço 04.20
        /// </summary>
        [XmlEnum("04.20")]
        Servico0420 = 0420,

        /// <summary>
        /// Serviço 04.21
        /// </summary>
        [XmlEnum("04.21")]
        Servico0421 = 0421,

        /// <summary>
        /// Serviço 04.22
        /// </summary>
        [XmlEnum("04.22")]
        Servico0422 = 0422,

        /// <summary>
        /// Serviço 04.23
        /// </summary>
        [XmlEnum("04.23")]
        Servico0423 = 0423,

        /// <summary>
        /// Serviço 05.01
        /// </summary>
        [XmlEnum("05.01")]
        Servico0501 = 0501,

        /// <summary>
        /// Serviço 05.02
        /// </summary>
        [XmlEnum("05.02")]
        Servico0502 = 0502,

        /// <summary>
        /// Serviço 05.03
        /// </summary>
        [XmlEnum("05.03")]
        Servico0503 = 0503,

        /// <summary>
        /// Serviço 05.04
        /// </summary>
        [XmlEnum("05.04")]
        Servico0504 = 0504,

        /// <summary>
        /// Serviço 05.05
        /// </summary>
        [XmlEnum("05.05")]
        Servico0505 = 0505,

        /// <summary>
        /// Serviço 05.06
        /// </summary>
        [XmlEnum("05.06")]
        Servico0506 = 0506,

        /// <summary>
        /// Serviço 05.07
        /// </summary>
        [XmlEnum("05.07")]
        Servico0507 = 0507,

        /// <summary>
        /// Serviço 05.08
        /// </summary>
        [XmlEnum("05.08")]
        Servico0508 = 0508,

        /// <summary>
        /// Serviço 05.09
        /// </summary>
        [XmlEnum("05.09")]
        Servico0509 = 0509,

        /// <summary>
        /// Serviço 06.01
        /// </summary>
        [XmlEnum("06.01")]
        Servico0601 = 0601,

        /// <summary>
        /// Serviço 06.02
        /// </summary>
        [XmlEnum("06.02")]
        Servico0602 = 0602,

        /// <summary>
        /// Serviço 06.03
        /// </summary>
        [XmlEnum("06.03")]
        Servico0603 = 0603,

        /// <summary>
        /// Serviço 06.04
        /// </summary>
        [XmlEnum("06.04")]
        Servico0604 = 0604,

        /// <summary>
        /// Serviço 06.05
        /// </summary>
        [XmlEnum("06.05")]
        Servico0605 = 0605,

        /// <summary>
        /// Serviço 06.06
        /// </summary>
        [XmlEnum("06.06")]
        Servico0606 = 0606,

        /// <summary>
        /// Serviço 07.01
        /// </summary>
        [XmlEnum("07.01")]
        Servico0701 = 0701,

        /// <summary>
        /// Serviço 07.02
        /// </summary>
        [XmlEnum("07.02")]
        Servico0702 = 0702,

        /// <summary>
        /// Serviço 07.03
        /// </summary>
        [XmlEnum("07.03")]
        Servico0703 = 0703,

        /// <summary>
        /// Serviço 07.04
        /// </summary>
        [XmlEnum("07.04")]
        Servico0704 = 0704,

        /// <summary>
        /// Serviço 07.05
        /// </summary>
        [XmlEnum("07.05")]
        Servico0705 = 0705,

        /// <summary>
        /// Serviço 07.06
        /// </summary>
        [XmlEnum("07.06")]
        Servico0706 = 0706,

        /// <summary>
        /// Serviço 07.07
        /// </summary>
        [XmlEnum("07.07")]
        Servico0707 = 0707,

        /// <summary>
        /// Serviço 07.08
        /// </summary>
        [XmlEnum("07.08")]
        Servico0708 = 0708,

        /// <summary>
        /// Serviço 07.09
        /// </summary>
        [XmlEnum("07.09")]
        Servico0709 = 0709,

        /// <summary>
        /// Serviço 07.10
        /// </summary>
        [XmlEnum("07.10")]
        Servico0710 = 0710,

        /// <summary>
        /// Serviço 07.11
        /// </summary>
        [XmlEnum("07.11")]
        Servico0711 = 0711,

        /// <summary>
        /// Serviço 07.12
        /// </summary>
        [XmlEnum("07.12")]
        Servico0712 = 0712,

        /// <summary>
        /// Serviço 07.13
        /// </summary>
        [XmlEnum("07.13")]
        Servico0713 = 0713,

        /// <summary>
        /// Serviço 07.16
        /// </summary>
        [XmlEnum("07.16")]
        Servico0716 = 0716,

        /// <summary>
        /// Serviço 07.17
        /// </summary>
        [XmlEnum("07.17")]
        Servico0717 = 0717,

        /// <summary>
        /// Serviço 07.18
        /// </summary>
        [XmlEnum("07.18")]
        Servico0718 = 0718,

        /// <summary>
        /// Serviço 07.19
        /// </summary>
        [XmlEnum("07.19")]
        Servico0719 = 0719,

        /// <summary>
        /// Serviço 07.20
        /// </summary>
        [XmlEnum("07.20")]
        Servico0720 = 0720,

        /// <summary>
        /// Serviço 07.21
        /// </summary>
        [XmlEnum("07.21")]
        Servico0721 = 0721,

        /// <summary>
        /// Serviço 07.22
        /// </summary>
        [XmlEnum("07.22")]
        Servico0722 = 0722,

        /// <summary>
        /// Serviço 08.01
        /// </summary>
        [XmlEnum("08.01")]
        Servico0801 = 0801,

        /// <summary>
        /// Serviço 08.02
        /// </summary>
        [XmlEnum("08.02")]
        Servico0802 = 0802,

        /// <summary>
        /// Serviço 09.01
        /// </summary>
        [XmlEnum("09.01")]
        Servico0901 = 0901,

        /// <summary>
        /// Serviço 09.02
        /// </summary>
        [XmlEnum("09.02")]
        Servico0902 = 0902,

        /// <summary>
        /// Serviço 09.03
        /// </summary>
        [XmlEnum("09.03")]
        Servico0903 = 0903,

        /// <summary>
        /// Serviço 10.01
        /// </summary>
        [XmlEnum("10.01")]
        Servico1001 = 1001,

        /// <summary>
        /// Serviço 10.02
        /// </summary>
        [XmlEnum("10.02")]
        Servico1002 = 1002,

        /// <summary>
        /// Serviço 10.03
        /// </summary>
        [XmlEnum("10.03")]
        Servico1003 = 1003,

        /// <summary>
        /// Serviço 10.04
        /// </summary>
        [XmlEnum("10.04")]
        Servico1004 = 1004,

        /// <summary>
        /// Serviço 10.05
        /// </summary>
        [XmlEnum("10.05")]
        Servico1005 = 1005,

        /// <summary>
        /// Serviço 10.06
        /// </summary>
        [XmlEnum("10.06")]
        Servico1006 = 1006,

        /// <summary>
        /// Serviço 10.07
        /// </summary>
        [XmlEnum("10.07")]
        Servico1007 = 1007,

        /// <summary>
        /// Serviço 10.08
        /// </summary>
        [XmlEnum("10.08")]
        Servico1008 = 1008,

        /// <summary>
        /// Serviço 10.09
        /// </summary>
        [XmlEnum("10.09")]
        Servico1009 = 1009,

        /// <summary>
        /// Serviço 10.10
        /// </summary>
        [XmlEnum("10.10")]
        Servico1010 = 1010,

        /// <summary>
        /// Serviço 11.01
        /// </summary>
        [XmlEnum("11.01")]
        Servico1101 = 1101,

        /// <summary>
        /// Serviço 11.02
        /// </summary>
        [XmlEnum("11.02")]
        Servico1102 = 1102,

        /// <summary>
        /// Serviço 11.03
        /// </summary>
        [XmlEnum("11.03")]
        Servico1103 = 1103,

        /// <summary>
        /// Serviço 11.04
        /// </summary>
        [XmlEnum("11.04")]
        Servico1104 = 1104,

        /// <summary>
        /// Serviço 12.01
        /// </summary>
        [XmlEnum("12.01")]
        Servico1201 = 1201,

        /// <summary>
        /// Serviço 12.02
        /// </summary>
        [XmlEnum("12.02")]
        Servico1202 = 1202,

        /// <summary>
        /// Serviço 12.03
        /// </summary>
        [XmlEnum("12.03")]
        Servico1203 = 1203,

        /// <summary>
        /// Serviço 12.04
        /// </summary>
        [XmlEnum("12.04")]
        Servico1204 = 1204,

        /// <summary>
        /// Serviço 12.05
        /// </summary>
        [XmlEnum("12.05")]
        Servico1205 = 1205,

        /// <summary>
        /// Serviço 12.06
        /// </summary>
        [XmlEnum("12.06")]
        Servico1206 = 1206,

        /// <summary>
        /// Serviço 12.07
        /// </summary>
        [XmlEnum("12.07")]
        Servico1207 = 1207,

        /// <summary>
        /// Serviço 12.08
        /// </summary>
        [XmlEnum("12.08")]
        Servico1208 = 1208,

        /// <summary>
        /// Serviço 12.09
        /// </summary>
        [XmlEnum("12.09")]
        Servico1209 = 1209,

        /// <summary>
        /// Serviço 12.10
        /// </summary>
        [XmlEnum("12.10")]
        Servico1210 = 1210,

        /// <summary>
        /// Serviço 12.11
        /// </summary>
        [XmlEnum("12.11")]
        Servico1211 = 1211,

        /// <summary>
        /// Serviço 12.12
        /// </summary>
        [XmlEnum("12.12")]
        Servico1212 = 1212,

        /// <summary>
        /// Serviço 12.13
        /// </summary>
        [XmlEnum("12.13")]
        Servico1213 = 1213,

        /// <summary>
        /// Serviço 12.14
        /// </summary>
        [XmlEnum("12.14")]
        Servico1214 = 1214,

        /// <summary>
        /// Serviço 12.15
        /// </summary>
        [XmlEnum("12.15")]
        Servico1215 = 1215,

        /// <summary>
        /// Serviço 12.16
        /// </summary>
        [XmlEnum("12.16")]
        Servico1216 = 1216,

        /// <summary>
        /// Serviço 12.17
        /// </summary>
        [XmlEnum("12.17")]
        Servico1217 = 1217,

        /// <summary>
        /// Serviço 13.02
        /// </summary>
        [XmlEnum("13.02")]
        Servico1302 = 1302,

        /// <summary>
        /// Serviço 13.03
        /// </summary>
        [XmlEnum("13.03")]
        Servico1303 = 1303,

        /// <summary>
        /// Serviço 13.04
        /// </summary>
        [XmlEnum("13.04")]
        Servico1304 = 1304,

        /// <summary>
        /// Serviço 13.05
        /// </summary>
        [XmlEnum("13.05")]
        Servico1305 = 1305,

        /// <summary>
        /// Serviço 14.01
        /// </summary>
        [XmlEnum("14.01")]
        Servico1401 = 1401,

        /// <summary>
        /// Serviço 14.02
        /// </summary>
        [XmlEnum("14.02")]
        Servico1402 = 1402,

        /// <summary>
        /// Serviço 14.03
        /// </summary>
        [XmlEnum("14.03")]
        Servico1403 = 1403,

        /// <summary>
        /// Serviço 14.04
        /// </summary>
        [XmlEnum("14.04")]
        Servico1404 = 1404,

        /// <summary>
        /// Serviço 14.05
        /// </summary>
        [XmlEnum("14.05")]
        Servico1405 = 1405,

        /// <summary>
        /// Serviço 14.06
        /// </summary>
        [XmlEnum("14.06")]
        Servico1406 = 1406,

        /// <summary>
        /// Serviço 14.07
        /// </summary>
        [XmlEnum("14.07")]
        Servico1407 = 1407,

        /// <summary>
        /// Serviço 14.08
        /// </summary>
        [XmlEnum("14.08")]
        Servico1408 = 1408,

        /// <summary>
        /// Serviço 14.09
        /// </summary>
        [XmlEnum("14.09")]
        Servico1409 = 1409,

        /// <summary>
        /// Serviço 14.10
        /// </summary>
        [XmlEnum("14.10")]
        Servico1410 = 1410,

        /// <summary>
        /// Serviço 14.11
        /// </summary>
        [XmlEnum("14.11")]
        Servico1411 = 1411,

        /// <summary>
        /// Serviço 14.12
        /// </summary>
        [XmlEnum("14.12")]
        Servico1412 = 1412,

        /// <summary>
        /// Serviço 14.13
        /// </summary>
        [XmlEnum("14.13")]
        Servico1413 = 1413,

        /// <summary>
        /// Serviço 14.14
        /// </summary>
        [XmlEnum("14.14")]
        Servico1414 = 1414,

        /// <summary>
        /// Serviço 15.01
        /// </summary>
        [XmlEnum("15.01")]
        Servico1501 = 1501,

        /// <summary>
        /// Serviço 15.02
        /// </summary>
        [XmlEnum("15.02")]
        Servico1502 = 1502,

        /// <summary>
        /// Serviço 15.03
        /// </summary>
        [XmlEnum("15.03")]
        Servico1503 = 1503,

        /// <summary>
        /// Serviço 15.04
        /// </summary>
        [XmlEnum("15.04")]
        Servico1504 = 1504,

        /// <summary>
        /// Serviço 15.05
        /// </summary>
        [XmlEnum("15.05")]
        Servico1505 = 1505,

        /// <summary>
        /// Serviço 15.06
        /// </summary>
        [XmlEnum("15.06")]
        Servico1506 = 1506,

        /// <summary>
        /// Serviço 15.07
        /// </summary>
        [XmlEnum("15.07")]
        Servico1507 = 1507,

        /// <summary>
        /// Serviço 15.08
        /// </summary>
        [XmlEnum("15.08")]
        Servico1508 = 1508,

        /// <summary>
        /// Serviço 15.09
        /// </summary>
        [XmlEnum("15.09")]
        Servico1509 = 1509,

        /// <summary>
        /// Serviço 15.10
        /// </summary>
        [XmlEnum("15.10")]
        Servico1510 = 1510,

        /// <summary>
        /// Serviço 15.11
        /// </summary>
        [XmlEnum("15.11")]
        Servico1511 = 1511,

        /// <summary>
        /// Serviço 15.12
        /// </summary>
        [XmlEnum("15.12")]
        Servico1512 = 1512,

        /// <summary>
        /// Serviço 15.13
        /// </summary>
        [XmlEnum("15.13")]
        Servico1513 = 1513,

        /// <summary>
        /// Serviço 15.14
        /// </summary>
        [XmlEnum("15.14")]
        Servico1514 = 1514,

        /// <summary>
        /// Serviço 15.15
        /// </summary>
        [XmlEnum("15.15")]
        Servico1515 = 1515,

        /// <summary>
        /// Serviço 15.16
        /// </summary>
        [XmlEnum("15.16")]
        Servico1516 = 1516,

        /// <summary>
        /// Serviço 15.17
        /// </summary>
        [XmlEnum("15.17")]
        Servico1517 = 1517,

        /// <summary>
        /// Serviço 15.18
        /// </summary>
        [XmlEnum("15.18")]
        Servico1518 = 1518,

        /// <summary>
        /// Serviço 16.01
        /// </summary>
        [XmlEnum("16.01")]
        Servico1601 = 1601,

        /// <summary>
        /// Serviço 16.02
        /// </summary>
        [XmlEnum("16.02")]
        Servico1602 = 1602,

        /// <summary>
        /// Serviço 17.01
        /// </summary>
        [XmlEnum("17.01")]
        Servico1701 = 1701,

        /// <summary>
        /// Serviço 17.02
        /// </summary>
        [XmlEnum("17.02")]
        Servico1702 = 1702,

        /// <summary>
        /// Serviço 17.03
        /// </summary>
        [XmlEnum("17.03")]
        Servico1703 = 1703,

        /// <summary>
        /// Serviço 17.04
        /// </summary>
        [XmlEnum("17.04")]
        Servico1704 = 1704,

        /// <summary>
        /// Serviço 17.05
        /// </summary>
        [XmlEnum("17.05")]
        Servico1705 = 1705,

        /// <summary>
        /// Serviço 17.06
        /// </summary>
        [XmlEnum("17.06")]
        Servico1706 = 1706,

        /// <summary>
        /// Serviço 17.08
        /// </summary>
        [XmlEnum("17.08")]
        Servico1708 = 1708,

        /// <summary>
        /// Serviço 17.09
        /// </summary>
        [XmlEnum("17.09")]
        Servico1709 = 1709,

        /// <summary>
        /// Serviço 17.10
        /// </summary>
        [XmlEnum("17.10")]
        Servico1710 = 1710,

        /// <summary>
        /// Serviço 17.11
        /// </summary>
        [XmlEnum("17.11")]
        Servico1711 = 1711,

        /// <summary>
        /// Serviço 17.12
        /// </summary>
        [XmlEnum("17.12")]
        Servico1712 = 1712,

        /// <summary>
        /// Serviço 17.13
        /// </summary>
        [XmlEnum("17.13")]
        Servico1713 = 1713,

        /// <summary>
        /// Serviço 17.14
        /// </summary>
        [XmlEnum("17.14")]
        Servico1714 = 1714,

        /// <summary>
        /// Serviço 17.15
        /// </summary>
        [XmlEnum("17.15")]
        Servico1715 = 1715,

        /// <summary>
        /// Serviço 17.16
        /// </summary>
        [XmlEnum("17.16")]
        Servico1716 = 1716,

        /// <summary>
        /// Serviço 17.17
        /// </summary>
        [XmlEnum("17.17")]
        Servico1717 = 1717,

        /// <summary>
        /// Serviço 17.18
        /// </summary>
        [XmlEnum("17.18")]
        Servico1718 = 1718,

        /// <summary>
        /// Serviço 17.19
        /// </summary>
        [XmlEnum("17.19")]
        Servico1719 = 1719,

        /// <summary>
        /// Serviço 17.20
        /// </summary>
        [XmlEnum("17.20")]
        Servico1720 = 1720,

        /// <summary>
        /// Serviço 17.21
        /// </summary>
        [XmlEnum("17.21")]
        Servico1721 = 1721,

        /// <summary>
        /// Serviço 17.22
        /// </summary>
        [XmlEnum("17.22")]
        Servico1722 = 1722,

        /// <summary>
        /// Serviço 17.23
        /// </summary>
        [XmlEnum("17.23")]
        Servico1723 = 1723,

        /// <summary>
        /// Serviço 17.24
        /// </summary>
        [XmlEnum("17.24")]
        Servico1724 = 1724,

        /// <summary>
        /// Serviço 17.25
        /// </summary>
        [XmlEnum("17.25")]
        Servico1725 = 1725,

        /// <summary>
        /// Serviço 18.01
        /// </summary>
        [XmlEnum("18.01")]
        Servico1801 = 1801,

        /// <summary>
        /// Serviço 19.01
        /// </summary>
        [XmlEnum("19.01")]
        Servico1901 = 1901,

        /// <summary>
        /// Serviço 20.01
        /// </summary>
        [XmlEnum("20.01")]
        Servico2001 = 2001,

        /// <summary>
        /// Serviço 20.02
        /// </summary>
        [XmlEnum("20.02")]
        Servico2002 = 2002,

        /// <summary>
        /// Serviço 20.03
        /// </summary>
        [XmlEnum("20.03")]
        Servico2003 = 2003,

        /// <summary>
        /// Serviço 21.01
        /// </summary>
        [XmlEnum("21.01")]
        Servico2101 = 2101,

        /// <summary>
        /// Serviço 22.01
        /// </summary>
        [XmlEnum("22.01")]
        Servico2201 = 2201,

        /// <summary>
        /// Serviço 23.01
        /// </summary>
        [XmlEnum("23.01")]
        Servico2301 = 2301,

        /// <summary>
        /// Serviço 24.01
        /// </summary>
        [XmlEnum("24.01")]
        Servico2401 = 2401,

        /// <summary>
        /// Serviço 25.01
        /// </summary>
        [XmlEnum("25.01")]
        Servico2501 = 2501,

        /// <summary>
        /// Serviço 25.02
        /// </summary>
        [XmlEnum("25.02")]
        Servico2502 = 2502,

        /// <summary>
        /// Serviço 25.03
        /// </summary>
        [XmlEnum("25.03")]
        Servico2503 = 2503,

        /// <summary>
        /// Serviço 25.04
        /// </summary>
        [XmlEnum("25.04")]
        Servico2504 = 2504,

        /// <summary>
        /// Serviço 25.05
        /// </summary>
        [XmlEnum("25.05")]
        Servico2505 = 2505,

        /// <summary>
        /// Serviço 26.01
        /// </summary>
        [XmlEnum("26.01")]
        Servico2601 = 2601,

        /// <summary>
        /// Serviço 27.01
        /// </summary>
        [XmlEnum("27.01")]
        Servico2701 = 2701,

        /// <summary>
        /// Serviço 28.01
        /// </summary>
        [XmlEnum("28.01")]
        Servico2801 = 2801,

        /// <summary>
        /// Serviço 29.01
        /// </summary>
        [XmlEnum("29.01")]
        Servico2901 = 2901,

        /// <summary>
        /// Serviço 30.01
        /// </summary>
        [XmlEnum("30.01")]
        Servico3001 = 3001,

        /// <summary>
        /// Serviço 31.01
        /// </summary>
        [XmlEnum("31.01")]
        Servico3101 = 3101,

        /// <summary>
        /// Serviço 32.01
        /// </summary>
        [XmlEnum("32.01")]
        Servico3201 = 3201,

        /// <summary>
        /// Serviço 33.01
        /// </summary>
        [XmlEnum("33.01")]
        Servico3301 = 3301,

        /// <summary>
        /// Serviço 34.01
        /// </summary>
        [XmlEnum("34.01")]
        Servico3401 = 3401,

        /// <summary>
        /// Serviço 35.01
        /// </summary>
        [XmlEnum("35.01")]
        Servico3501 = 3501,

        /// <summary>
        /// Serviço 36.01
        /// </summary>
        [XmlEnum("36.01")]
        Servico3601 = 3601,

        /// <summary>
        /// Serviço 37.01
        /// </summary>
        [XmlEnum("37.01")]
        Servico3701 = 3701,

        /// <summary>
        /// Serviço 38.01
        /// </summary>
        [XmlEnum("38.01")]
        Servico3801 = 3801,

        /// <summary>
        /// Serviço 39.01
        /// </summary>
        [XmlEnum("39.01")]
        Servico3901 = 3901,

        /// <summary>
        /// Serviço 40.01
        /// </summary>
        [XmlEnum("40.01")]
        Servico4001 = 4001
    }

    #endregion

    #region Indicador da exigibilidade do ISS 

    /// <summary>
    /// Indicador de Exigibilidade do ISSQN
    /// </summary>
    public enum IndicadorExigibilidadeISSQN
    {
        /// <summary>
        /// 1=Exigível
        /// </summary>
        [XmlEnum("1")]
        Exigivel = 1,

        /// <summary>
        /// 2=Não incidência
        /// </summary>
        [XmlEnum("2")]
        NaoIncidencia = 2,

        /// <summary>
        /// 3=Isenção
        /// </summary>
        [XmlEnum("3")]
        Isencao = 3,

        /// <summary>
        /// 4=Exportação
        /// </summary>
        [XmlEnum("4")]
        Exportacao = 4,

        /// <summary>
        /// 5=Imunidade
        /// </summary>
        [XmlEnum("5")]
        Imunidade = 5,

        /// <summary>
        /// 6=Exigibilidade Suspensa por Decisão Judicial
        /// </summary>
        [XmlEnum("6")]
        SuspensaDecisaoJudicial = 6,

        /// <summary>
        /// 7=Exigibilidade Suspensa por Processo Administrativo
        /// </summary>
        [XmlEnum("7")]
        SuspensaProcessoAdministrativo = 7
    }

    #endregion

    #region Código do Regime Especial de Tributação

    /// <summary>
    /// Códigos de Regime Especial de Tributação - Utilizado na prestação de serviços (ISSQN)
    /// </summary>
    public enum CodigoRegimeEspecialTributacao
    {
        /// <summary>
        /// 1=Microempresa Municipal,
        /// </summary>
        [XmlEnum("1")]
        MicroempresaMunicipal = 1,

        /// <summary>
        /// 2=Estimativa,
        /// </summary>
        [XmlEnum("2")]
        Estimativa = 2,

        /// <summary>
        /// 3=Sociedade de Profissionais,
        /// </summary>
        [XmlEnum("3")]
        SociedadeProfissionais = 3,

        /// <summary>
        /// 4=Cooperativa,
        /// </summary>
        [XmlEnum("4")]
        Cooperativa = 4,

        /// <summary>
        /// 5=Microempresário Individual (MEI),
        /// </summary>
        [XmlEnum("5")]
        MicroEmpresarioIndividual = 5,

        /// <summary>
        /// 6=Microempresário e Empresa de Pequeno Porte (ME/EPP)
        /// </summary>
        [XmlEnum("6")]
        MicroEmpresarioEmpresaPequenoPorte = 6
    }

    #endregion

    #region TipoOperacaoVeicNovo

    /// <summary>
    /// Tipo de operação - Detalhamento de veículos novos
    /// </summary>
    public enum TipoOperacaoVeicNovo
    {
        /// <sumary>
        /// 1=Venda concessionária,
        /// </sumary>
        [XmlEnum("1")]
        VendaConcessionaria = 1,

        /// <sumary>
        /// 2=Faturamento direto para consumidor final
        /// </sumary>
        [XmlEnum("2")]
        FaturamentoDiretoConsumidorfinal = 2,

        /// <sumary>
        /// 3=Venda direta para grandes consumidores (frotista, governo, etc...)
        /// </sumary>
        [XmlEnum("3")]
        VendaDiretaGrandesConsumidores = 3,

        /// <sumary>
        /// 0=Outros
        /// </sumary>
        [XmlEnum("0")]
        Outros = 0
    }

    #endregion

    #region CondicaoVIN

    /// <summary>
    /// Condição do VIN, chassi do veículo, remarcado ou normal
    /// </summary>
    public enum CondicaoVIN
    {
        /// <summary>
        /// R = Veículo com VIN (chassi) remarcado
        /// </summary>
        [XmlEnum("R")]
        Remarcado = 0,

        /// <summary>
        /// R = Veículo com VIN (chassi) remarcado
        /// </summary>
        [XmlEnum("N")]
        Normal = 1
    }

    #endregion


    #region CondicaoVeiculo

    /// <summary>
    /// Condição do veículo
    /// </summary>
    public enum CondicaoVeiculo
    {
        /// <summary>
        /// 1=Acabado
        /// </summary>
        [XmlEnum("1")]
        Acabado = 1,

        /// <summary>
        /// 2=Inacabado
        /// </summary>
        [XmlEnum("2")]
        Inacabado = 2,

        /// <summary>
        /// 3=Semiacabado
        /// </summary>
        [XmlEnum("3")]
        Semiacabado = 3
    }

    #endregion


    #region TipoRestricaoVeiculo

    /// <summary>
    /// Tipo de Restrição do Veículo
    /// </summary>
    public enum TipoRestricaoVeiculo
    {
        /// <sumary>
        /// 0=Não há
        /// </sumary>
        [XmlEnum("0")]
        NaoHa = 0,

        /// <sumary>
        /// 1=Alienação Fiduciária
        /// </sumary>
        [XmlEnum("1")]
        AlienacaoFiduciaria = 1,

        /// <sumary>
        /// 2=Arrendamento Mercantil
        /// </sumary>
        [XmlEnum("2")]
        ArrendamentoMercantil = 2,

        /// <sumary>
        /// 3=Reserva de Domínio
        /// </sumary>
        [XmlEnum("3")]
        ReservaDominio = 3,

        /// <sumary>
        /// 4=Penhor de Veículos
        /// </sumary>
        [XmlEnum("4")]
        PenhorVeiculos = 4,

        /// <sumary>
        /// 9=Outras
        /// </sumary>
        [XmlEnum("9")]
        Outras = 9
    }

    #endregion


    #region TipoArma

    /// <summary>
    /// Indicador do tipo de arma de fogo
    /// </summary>
    public enum TipoArma
    {
        /// <summary>
        /// 0 = Uso Permitido
        /// </summary>
        [XmlEnum("0")]
        UsoPermitido = 0,

        /// <summary>
        /// Uso Restrito
        /// </summary>
        [XmlEnum("1")]
        UsoRestrito = 1
    }

    #endregion   
}

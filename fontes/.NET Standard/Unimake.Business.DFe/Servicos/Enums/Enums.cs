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
    /// Unidades Federativas do Brasil (Tem como XmlEnumAttribute o nome abreviado da UF)
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Producao = 1,
        [System.Xml.Serialization.XmlEnumAttribute("2")]
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
        [System.Xml.Serialization.XmlEnumAttribute("55")]
        NFe = 55,
        /// <summary>
        /// NFC-e (Modelo: 65)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("65")]
        NFCe = 65,
        /// <summary>
        /// CT-e (Modelo: 57)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("57")]
        CTe = 57,
        /// <summary>
        /// MDF-e (Modelo: 58)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("58")]
        MDFe = 58,
        /// <summary>
        /// CTeOS (Modelo: 67)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("67")]
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
        [System.Xml.Serialization.XmlEnumAttribute("110110")]
        CartaCorrecao = 110110,
        /// <summary>
        /// Cancelamento NFe (110111)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("110111")]
        Cancelamento = 110111,
        /// <summary>
        /// Cancelamento da NFCe sendo substituida por outra NFCe (110112)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("110112")]
        CancelamentoPorSubstituicao = 110112,
        /// <summary>
        /// EPEC - Evento Prévio de Emissão em Contingência (110140)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("110140")]
        EPEC = 110140,
        /// <summary>
        /// Pedido de prorrogação do prazo de ICMS no caso de remessa para industrialização (111500)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("111500")]
        PedidoProrrogacao = 111500,
        /// <summary>
        /// Manifestação do Destinatário - Confirmação da Operação (210200)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("210200")]
        ManifestacaoConfirmacaoOperacao = 210200,
        /// <summary>
        /// Manifestação do Destinatário - Ciência da Operação (210210)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("210210")]
        ManifestacaoCienciaOperacao = 210210,
        /// <summary>
        /// Manifestação do Destinatário - Desconhecimento da Operação (210220)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("210220")]
        ManifestacaoDesconhecimentoOperacao = 210220,
        /// <summary>
        /// Manifestação do Destinatário - Operação não realizada (210240)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("210240")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Não = 0,

        /// <summary>
        /// Sim (1)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Entrada = 0,
        /// <summary>
        /// Operação de saída
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        OperacaoInterna = 1,

        /// <summary>
        /// Operação interestadual, ou seja, estado diferente do de origem
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        OperacaoInterestadual = 2,

        /// <summary>
        /// Operação com o exterior, ou seja, fora do país de origem
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        SemGeracao = 0,

        /// <summary>
        /// 1=DANFE normal, Retrato
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        NormalRetrato = 1,

        /// <summary>
        /// 2=DANFE normal, Paisagem
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        NormalPaisagem = 2,

        /// <summary>
        /// 3=DANFE Simplificado
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Simplificado = 3,

        /// <summary>
        /// 4=DANFE NFC-e
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        NFCe = 4,

        /// <summary>
        /// 5=DANFE NFC-e em mensagem eletrônica
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Normal = 1,

        /// <summary>
        /// 2=Contingência FS-IA, com impressão do DANFE em formulário de segurança
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        ContingenciaFSIA = 2,

        /// <summary>
        /// 4=Contingência DPEC (Declaração Prévia da Emissão em Contingência);
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        ContingenciaDPEC = 4,

        /// <summary>
        /// 5=Contingência FS-DA, com impressão do DANFE em formulário de segurança;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        ContingenciaFSDA = 5,

        /// <summary>
        /// 6=Contingência SVC-AN (SEFAZ Virtual de Contingência do AN);
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        ContingenciaSVCAN = 6,

        /// <summary>
        /// 7=Contingência SVC-RS (SEFAZ Virtual de Contingência do RS);
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        ContingenciaSVCRS = 7,

        /// <summary>
        /// 9=Contingência off-line da NFC-e
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Normal = 1,

        /// <summary>
        /// 2=NF-e complementar
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Complementar = 2,

        /// <summary>
        /// 3=NF-e de ajuste
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Auste = 3,

        /// <summary>
        /// 4=Devolução de mercadoria
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        NaoSeAplica = 0,

        /// <summary>
        /// 1=Operação presencial
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        OperacaoPresencial = 1,

        /// <summary>
        /// 2=Operação não presencial, pela Internet
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        OperacaoInternet = 2,

        /// <summary>
        /// 3=Operação não presencial, Teleatendimento
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        OperacaoTeleAtendimento = 3,

        /// <summary>
        /// 4=NFC-e em operação com entrega a domicílio
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        NFCeEntregaDomicilio = 4,

        /// <summary>
        /// 9=Operação não presencial, outros
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        AplicativoContribuinte = 0,

        /// <summary>
        /// 1=Emissão de NF-e avulsa pelo Fisco;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        AvulsaPeloFisco = 1,

        /// <summary>
        /// 2=Emissão de NF-e avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        AvulsaPeloContribuinteSiteFisco = 2,

        /// <summary>
        /// 3=Emissão NF-e pelo contribuinte com aplicativo fornecido pelo Fisco.
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        SimplesNacional = 1,

        /// <summary>
        /// 2=Simples Nacional, excesso sublimite de receita bruta
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        SimplesNacionalExcessoSublimite = 2,

        /// <summary>
        /// 3=Regime Normal
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        ContribuinteICMS = 1,

        /// <summary>
        /// 2=Contribuinte isento de Inscrição no cadastro de Contribuintes do ICMS
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        ContribuinteIsento = 2,

        /// <summary>
        /// 9=Não Contribuinte, que pode ou não possuir Inscrição Estadual no Cadastro de Contribuintes do ICMS
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
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
        [System.Xml.Serialization.XmlEnumAttribute("S")]
        Sim,

        /// <summary>
        /// N – Produzido em Escala NÃO Relevante
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("N")]
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Maritima = 1,

        /// <summary>
        /// 2=Fluvial
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Fluvial = 2,

        /// <summary>
        /// 3=Lacustre
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Lacustre = 3,

        /// <summary>
        /// 4=Aérea
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Aerea = 4,

        /// <summary>
        /// 5=Postal
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Postal = 5,

        /// <summary>
        /// 6=Ferroviária
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Ferroviaria = 6,

        /// <summary>
        /// 7=Rodoviária
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Rodoviaria = 7,

        /// <summary>
        /// 8=Conduto / Rede Transmissão
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        CondutoRedeTransmissao = 8,

        /// <summary>
        /// 9=Meios Próprios
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        MeiosProprios = 9,

        /// <summary>
        /// 10=Entrada / Saída ficta
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        EntradaSaidaFicta = 10,

        /// <summary>
        /// 11=Courier
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Courier = 11,

        /// <summary>
        /// 12=Handcarry
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("12")]
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
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        ImportacaoPorContaPropria = 1,

        /// <summary>
        /// 2=Importação por conta e ordem
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        ImportacaoPorContaOrdem = 2,

        /// <summary>
        /// 3=Importação por encomenda
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Nacional = 0,

        /// <summary>
        /// 1 - Estrangeira - Importação direta, exceto a indicada no código 6;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Estrangeira = 1,

        /// <summary>
        /// 2 - Estrangeira - Adquirida no mercado interno, exceto a indicada no código 7;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Estrangeira2 = 2,

        /// <summary>
        /// 3 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 40% e inferior ou igual a 70%;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Nacional3 = 3,

        /// <summary>
        /// 4 - Nacional, cuja produção tenha sido feita em conformidade com os processos produtivos básicos de que tratam as legislações citadas nos Ajustes;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Nacional4 = 4,

        /// <summary>
        /// 5 - Nacional, mercadoria ou bem com Conteúdo de Importação inferior ou igual a 40%;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Nacional5 = 5,

        /// <summary>
        /// 6 - Estrangeira - Importação direta, sem similar nacional, constante em lista da CAMEX e gás natural;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Estrangeira6 = 6,

        /// <summary>
        /// 7 - Estrangeira - Adquirida no mercado interno, sem similar nacional, constante lista CAMEX e gás natural.
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Estrangeira7 = 7,

        /// <summary>
        /// 8 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 70%;
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        MargemValorAgregado = 0,

        /// <summary>
        /// 1=Pauta (Valor)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Pauta = 1,

        /// <summary>
        /// 2=Preço Tabelado Máx. (valor)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        PrecoTabeladoMaximo = 2,

        /// <summary>
        /// 3=Valor da operação
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        PrecoTabeladoMaximoSugerido = 0,

        /// <summary>
        /// 1=Lista Negativa (valor)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        ListaNegativa = 1,

        /// <summary>
        /// 2=Lista Positiva (valor)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        ListaPositiva = 2,

        /// <summary>
        /// 3=Lista Neutra (valor)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        ListaNeutra = 3,

        /// <summary>
        /// 4=Margem Valor Agregado (%)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        MargemValorAgregado = 4,

        /// <summary>
        /// 5=Pauta (valor)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Pauta = 5
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
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        SEFAZ = 0,

        /// <summary>
        /// 1=Justiça Federal
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        JusticaFederal = 1,

        /// <summary>
        /// 2=Justiça Estadual
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        JusticaEstadual = 2,

        /// <summary>
        /// 3=Secex/RFB
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        SecexRFB = 3,

        /// <summary>
        /// 9=Outros
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Outros = 9
    }

    #endregion

    #region MotivoDesoneracaoICMS

    public enum MotivoDesoneracaoICMS
    {
        /// <summary>
        /// 3=Uso na agropecuária
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        UsoAgropecuaria = 3,

        /// <summary>
        /// 9=Outros
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Outro = 9,

        /// <summary>
        /// 12=Órgão de fomento e desenvolvimento agropecuário
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        OrgaoFomentoDesenvolvimentoAgropecuario = 12
    }

    #endregion

    #region ModalidadeFrete

    public enum ModalidadeFrete
    {
        /// <summary>
        /// 0=Contratação do Frete por conta do Remetente (CIF); 
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        ContratacaoFretePorContaRemetente_CIF = 0,

        /// <summary>
        /// 1=Contratação do Frete por conta do Destinatário (FOB)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        ContratacaoFretePorContaDestinatário_FOB = 1,

        /// <summary>
        /// 2=Contratação do Frete por conta de Terceiros
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        ContratacaoFretePorContaTerceiros = 2,

        /// <summary>
        /// 3=Transporte Próprio por conta do Remetente
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        TransporteProprioPorContaRemetente = 3,

        /// <summary>
        /// 4=Transporte Próprio por conta do Destinatário
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        TransporteProprioPorContaDestinatário = 4,

        /// <summary>
        /// 9=Sem Ocorrência de Transporte
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
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
        [System.Xml.Serialization.XmlEnum("1")]
        PagamentoIntegrado = 1,
        /// <summary>
        /// 2= Pagamento não integrado com o sistema de automação da empresa (Ex.: equipamento POS)
        /// </summary>
        [System.Xml.Serialization.XmlEnum("2")]
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
        [System.Xml.Serialization.XmlEnum("01")]
        Visa = 1,

        /// <summary>
        /// 02=Mastercard
        /// </summary>
        [System.Xml.Serialization.XmlEnum("02")]
        Mastercard = 2,

        /// <summary>
        /// 03=American Express
        /// </summary>
        [System.Xml.Serialization.XmlEnum("03")]
        AmericanExpress = 3,

        /// <summary>
        /// 04=Sorocred
        /// </summary>
        [System.Xml.Serialization.XmlEnum("04")]
        Sorocred = 4,

        /// <summary>
        /// 05=Diners Club
        /// </summary>
        [System.Xml.Serialization.XmlEnum("05")]
        DinersClub = 5,

        /// <summary>
        /// 06=Elo
        /// </summary>
        [System.Xml.Serialization.XmlEnum("06")]
        Elo = 6,

        /// <summary>
        /// 07=Hipercard
        /// </summary>
        [System.Xml.Serialization.XmlEnum("07")]
        Hipercard = 7,

        /// <summary>
        /// 08=Aura
        /// </summary>
        [System.Xml.Serialization.XmlEnum("08")]
        Aura = 8,

        /// <summary>
        /// 09=Cabal
        /// </summary>
        [System.Xml.Serialization.XmlEnum("09")]
        Cabal = 9,

        /// <summary>
        /// 99=Outros
        /// </summary>
        [System.Xml.Serialization.XmlEnum("99")]
        Outros = 99
    }

    #endregion

    #region MeioPagamento

    public enum MeioPagamento
    {
        /// <summary>
        /// 01=Dinheiro
        /// </summary>
        [System.Xml.Serialization.XmlEnum("01")]
        Dinheiro = 1,

        /// <summary>
        /// 02=Cheque
        /// </summary>
        [System.Xml.Serialization.XmlEnum("02")]
        Cheque = 2,

        /// <summary>
        /// 03=Cartão de Crédito
        /// </summary>
        [System.Xml.Serialization.XmlEnum("03")]
        CartaoCredito = 3,

        /// <summary>
        /// 04=Cartão de Débito
        /// </summary>
        [System.Xml.Serialization.XmlEnum("04")]
        CartaoDebito = 4,

        /// <summary>
        /// 05=Crédito Loja
        /// </summary>
        [System.Xml.Serialization.XmlEnum("05")]
        CreditoLoja = 5,

        /// <summary>
        /// 10=Vale Alimentação
        /// </summary>
        [System.Xml.Serialization.XmlEnum("10")]
        ValeAlimentacao = 10,

        /// <summary>
        /// 11=Vale Refeição
        /// </summary>
        [System.Xml.Serialization.XmlEnum("11")]
        ValeRefeicao = 11,

        /// <summary>
        /// 12=Vale Presente
        /// </summary>
        [System.Xml.Serialization.XmlEnum("12")]
        ValePresente = 12,

        /// <summary>
        /// 13=Vale Combustível
        /// </summary>
        [System.Xml.Serialization.XmlEnum("13")]
        ValeCombustivel = 13,

        /// <summary>
        /// 15=Boleto Bancário
        /// </summary> 
        [System.Xml.Serialization.XmlEnum("15")]
        BoletoBancario = 15,

        /// <summary>
        /// 90=Sem pagamento
        /// </summary>
        [System.Xml.Serialization.XmlEnum("90")]
        Sempagamento = 90,

        /// <summary>
        /// 99=Outros
        /// </summary>
        [System.Xml.Serialization.XmlEnum("99")]
        Outros = 99
    }

    #endregion

    #region IndicadorPagamento

    public enum IndicadorPagamento
    {
        /// <summary>
        /// Pagamento à Vista
        /// </summary>
        [System.Xml.Serialization.XmlEnum("0")]
        PagamentoVista = 0,

        /// <summary>
        /// Pagamento à Prazo
        /// </summary>
        [System.Xml.Serialization.XmlEnum("1")]
        PagamentoPrazo = 1
    }

    #endregion
}
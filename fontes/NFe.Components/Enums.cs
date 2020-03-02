using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NFe.Components
{
    #region SubPastas da pasta de enviados

    /// <summary>
    /// SubPastas da pasta de XML´s enviados para os webservices
    /// </summary>
    public enum PastaEnviados
    {
        EmProcessamento,
        Autorizados,
        Denegados,
        Originais
    }

    #endregion SubPastas da pasta de enviados

    #region Servicos

    /// <summary>
    /// Serviços executados pelo Aplicativo
    /// </summary>
    public enum Servicos
    {
        #region NFe

        /// <summary>
        /// Consulta status serviço NFe
        /// </summary>
        NFeConsultaStatusServico,

        /// <summary>
        /// Somente converter TXT da NFe para XML de NFe
        /// </summary>
        NFeConverterTXTparaXML,

        /// <summary>
        /// Envia os lotes de NFe para os webservices
        /// </summary>
        NFeEnviarLote,

        /// <summary>
        /// Envia XML de Inutilização da NFe
        /// </summary>
        NFeInutilizarNumeros,

        /// <summary>
        /// Assinar e montar lote de uma NFe
        /// </summary>
        NFeMontarLoteUma,

        /// <summary>
        /// Consulta situação da NFe
        /// </summary>
        NFePedidoConsultaSituacao,

        /// <summary>
        /// Consulta recibo do lote nfe
        /// </summary>
        NFePedidoSituacaoLote,

        #region Eventos NFe

        /// <summary>
        /// Enviar XML Evento - Cancelamento
        /// </summary>
        EventoCancelamento,

        /// <summary>
        /// Enviar XML Evento - Carta de Correção
        /// </summary>
        EventoCCe,

        /// <summary>
        /// Enviar um evento de EPEC
        /// </summary>
        EventoEPEC,

        /// <summary>
        /// Enviar um evento de manifestacao
        /// </summary>
        EventoManifestacaoDest,

        /// <summary>
        /// Enviar XML de Evento NFe
        /// </summary>
        EventoRecepcao,

        #endregion Eventos NFe

        /// <summary>
        /// Assinar e validar um XML de NFe no envio em Lote
        /// </summary>
        NFeAssinarValidarEnvioEmLote,

        /// <summary>
        /// Monta chave de acesso
        /// </summary>
        NFeGerarChave,

        /// <summary>
        /// Assinar e montar lote de várias NFe
        /// </summary>
        NFeMontarLoteVarias,

        #endregion NFe

        #region CTe

        /// <summary>
        /// Assinar e validar um XML de CTe no envio em Lote
        /// </summary>
        CTeAssinarValidarEnvioEmLote,

        /// <summary>
        /// Consulta Status Serviço CTe
        /// </summary>
        CTeConsultaStatusServico,

        /// <summary>
        /// Envia os lotes de CTe para os webservices
        /// </summary>
        CTeEnviarLote,

        /// <summary>
        /// Envia XML de Inutilização da CTe
        /// </summary>
        CTeInutilizarNumeros,

        /// <summary>
        /// Montar lote de um CTe
        /// </summary>
        CTeMontarLoteUm,

        /// <summary>
        /// Assinar e montar lote de várias CTe
        /// </summary>
        CTeMontarLoteVarios,

        /// <summary>
        /// Consulta situação da CTe
        /// </summary>
        CTePedidoConsultaSituacao,

        /// <summary>
        /// Consulta recibo do lote CTe
        /// </summary>
        CTePedidoSituacaoLote,

        /// <summary>
        /// Enviar XML Evento CTe
        /// </summary>
        CTeRecepcaoEvento,

        /// <summary>
        /// Enviar XML de distribuição de DFe de interesses de autores (CTe)
        /// </summary>
        CTeDistribuicaoDFe,

        /// <summary>
        /// Enviar XML de CTe modelo 67
        /// </summary>
        CteRecepcaoOS,

        #endregion CTe

        #region NFSe

        /// <summary>
        /// Cancelar NFS-e
        /// </summary>
        [Description("Cancelar NFS-e")]
        NFSeCancelar,

        /// <summary>
        /// Consultar NFS-e por Data
        /// </summary>
        [Description("Consultar NFS-e por Data")]
        NFSeConsultar,

        /// <summary>
        /// Consultar lote RPS
        /// </summary>
        [Description("ConsultarLoteRPS")]
        NFSeConsultarLoteRps,

        /// <summary>
        /// Consultar NFS-e por RPS
        /// </summary>
        [Description("Consultar NFS-e por RPS")]
        NFSeConsultarPorRps,

        /// <summary>
        /// Consultar Situação do lote RPS NFS-e
        /// </summary>
        [Description("Consultar Situação do lote RPS NFS-e")]
        NFSeConsultarSituacaoLoteRps,

        /// <summary>
        /// Consultar a URL de visualização da NFSe
        /// </summary>
        [Description("Consultar a URL de Visualização da NFS-e")]
        NFSeConsultarURL,

        /// <summary>
        /// Consultar a URL de visualização da NFSe
        /// </summary>
        [Description("Consultar a URL de Visualização da NFS-e com a Série")]
        NFSeConsultarURLSerie,

        /// <summary>
        /// Enviar Lote RPS NFS-e
        /// </summary>
        [Description("Enviar Lote RPS NFS-e ")]
        NFSeRecepcionarLoteRps,

        /// <summary>
        /// Enviar Lote RPS NFS-e de forma sincrona
        /// Criado inicialmente para ser utilizado para o padrão BHIss, pois é necessario utilizar a recepção de lote das duas formas.
        /// </summary>
        [Description("Enviar Lote RPS NFS-e Sincrono")]
        NFSeRecepcionarLoteRpsSincrono,

        /// <summary>
        /// Enviar Lote RPS NFS-e de forma sincrona
        /// Criado inicialmente para ser utilizado para o padrão BHIss, pois é necessario utilizar a recepção de lote das duas formas.
        /// </summary>
        [Description("Enviar Lote RPS NFS-e Sincrono")]
        NFSeGerarNfse,

        /// <summary>
        /// Consulta da imagem de uma NFS-e em formato PNG
        /// Criado inicialmente para ser utilizado para o padrão INFISC para a Prefeitura de Caxias do Sul - RS
        /// </summary>
        [Description("Consulta da imagem de uma NFS-e em formato PNG")]
        NFSeConsultarNFSePNG,

        /// <summary>
        /// Consulta da imagem de uma NFS-e em formato
        /// Criado inicialmente para ser utilizado para o padrão INFISC para a Prefeitura de Caxias do Sul - RS
        /// </summary>
        [Description("Inutilização de uma NFS-e")]
        NFSeInutilizarNFSe,

        /// <summary>
        /// Consulta da imagem de uma NFS-e em formato PDF
        /// Criado inicialmente para ser utilizado para o padrão INFISC para a Prefeitura de Caxias do Sul - RS
        /// </summary>
        [Description("Consulta da imagem de uma NFS-e em formato PDF")]
        NFSeConsultarNFSePDF,

        /// <summary>
        /// Baixar o XML da NFSe
        /// </summary>
        [Description("Obter o XML da NFS-e")]
        NFSeObterNotaFiscal,

        /// <summary>
        /// Consulta Sequencia do Lote da Nota RPS
        /// </summary>
        [Description("Consulta Sequencia do Lote da Nota RPS")]
        NFSeConsultaSequenciaLoteNotaRPS,

        /// <summary>
        /// Substituir NFS-e
        /// </summary>
        [Description("Substituir NFS-e")]
        NFSeSubstituirNfse,

        /// <summary>
        /// Consultar Status NFS-e
        /// </summary>
        [Description("Consultar Status da NFS-e")]
        NFSeConsultarStatusNota,

		// <summary>
        /// Consultar as notas fiscais de serviço recebidas
        /// </summary>
        [Description("Consultar NFS-e recebidas")]
        NFSeConsultarNFSeRecebidas,

        /// <summary>
        /// Consultar as notas fiscais de serviço tomados
        /// </summary>
        [Description("Consultar NFS-e tomados")]
        NFSeConsultarNFSeTomados,

        #endregion NFSe

        #region CFSe

        /// <summary>
        /// Enviar Lote CFS-e
        /// </summary>
        [Description("Enviar Lote CFS-e")]
        RecepcionarLoteCfse,

        /// <summary>
        /// Enviar Lote CFS-e Sincrono
        /// </summary>
        [Description("Enviar Lote CFS-e")]
        RecepcionarLoteCfseSincrono,

        /// <summary>
        /// Cancelar CFS-e
        /// </summary>
        [Description("Enviar Cancelamento CFS-e")]
        CancelarCfse,

        /// <summary>
        /// Consultar Lote CFS-e
        /// </summary>
        [Description("Enviar consulta do lote CFS-e")]
        ConsultarLoteCfse,

        /// <summary>
        /// Consultar CFS-e
        /// </summary>
        [Description("Enviar consulta do CFS-e")]
        ConsultarCfse,

        /// <summary>
        /// Configurar/Ativar Terminal CFS-e
        /// </summary>
        [Description("Enviar XML de configuração/ativação de terminal CFS-e")]
        ConfigurarTerminalCfse,

        /// <summary>
        /// Informar terminal CFS-e em manutenção
        /// </summary>
        [Description("Enviar XML para informar que o terminal de CFS-e está em manutenção")]
        EnviarInformeManutencaoCfse,

        /// <summary>
        /// Informar data sem movimento de CFS-e
        /// </summary>
        [Description("Enviar XML para informar que não teve movimento de CFS-e no dia")]
        InformeTrasmissaoSemMovimentoCfse,

        /// <summary>
        /// Consulta dados cadastro terminal CFS-e
        /// </summary>
        [Description("Enviar XML para consultar dados cadastros terminal CFS-e")]
        ConsultarDadosCadastroCfse,

        #endregion CFSe

        #region MDFe

        /// <summary>
        /// Assinar e validar um XML de MDFe no envio em Lote
        /// </summary>
        MDFeAssinarValidarEnvioEmLote,

        /// <summary>
        /// Consulta MDFe nao encerrados
        /// </summary>
        MDFeConsultaNaoEncerrado,

        /// <summary>
        /// Consulta Status Serviço MDFe
        /// </summary>
        MDFeConsultaStatusServico,

        /// <summary>
        /// Envia os lotes de MDFe para os webservices Assincrono
        /// </summary>
        MDFeEnviarLote,

        /// <summary>
        /// Envia os lotes de MDFe para os webservices Sincrono
        /// </summary>
        MDFeEnviarLoteSinc,

        /// <summary>
        /// Montar lote de um MDFe
        /// </summary>
        MDFeMontarLoteUm,

        /// <summary>
        /// Assinar e montar lote de várias MDFe
        /// </summary>
        MDFeMontarLoteVarios,

        /// <summary>
        /// Consulta situação da MDFe
        /// </summary>
        MDFePedidoConsultaSituacao,

        /// <summary>
        /// Consulta recibo do lote MDFe
        /// </summary>
        MDFePedidoSituacaoLote,

        /// <summary>
        /// Enviar XML Evento MDFe
        /// </summary>
        MDFeRecepcaoEvento,

        #endregion MDFe

        #region SAT/CFe

        /// <summary>
        /// Consultar SAT
        /// </summary>
        SATConsultar,

        SATExtrairLogs,
        SATConsultarStatusOperacional,
        SATTesteFimAFim,
        SATTrocarCodigoDeAtivacao,
        SATEnviarDadosVenda,
        SATConverterNFCe,
        SATCancelarUltimaVenda,
        SATConfigurarInterfaceDeRede,
        SATAssociarAssinatura,
        SATAtivar,
        SATBloquear,
        SATDesbloquear,
        SATConsultarNumeroSessao,

        #endregion SAT/CFe

        #region Serviços em comum NFe, CTe, MDFe e NFSe

        /// <summary>
        /// Valida e envia o XML de pedido de Consulta do Cadastro do Contribuinte para o webservice
        /// </summary>
        ConsultaCadastroContribuinte,

        /// <summary>
        /// Efetua verificações nas notas em processamento para evitar algumas falhas e perder retornos de autorização de notas
        /// </summary>
        EmProcessamento,

        /// <summary>
        /// Somente assinar e validar o XML
        /// </summary>
        AssinarValidar,

        #endregion Serviços em comum NFe, CTe, MDFe e NFSe

        #region Serviços gerais

        /// <summary>
        /// Consultar Informações Gerais do UniNFe
        /// </summary>
        UniNFeConsultaInformacoes,

        /// <summary>
        /// Solicitar ao UniNFe que altere suas configurações
        /// </summary>
        UniNFeAlterarConfiguracoes,

        /// <summary>
        /// Efetua uma limpeza das pastas que recebem arquivos temporários
        /// </summary>
        UniNFeLimpezaTemporario,

        /// <summary>
        /// Consultas efetuadas pela pasta GERAL.
        /// </summary>
        UniNFeConsultaGeral,

        /// <summary>
        /// Consulta Certificados Instalados na estação do UniNFe.
        /// </summary>
        UniNFeConsultaCertificados,

        /// <summary>
        /// Força atualizar o aplicativo UniNFe
        /// </summary>
        UniNFeUpdate,

        #endregion Serviços gerais

        #region Não sei para que serve - Wandrey

        /// <summary>
        /// WSExiste
        /// </summary>
        WSExiste,

        #endregion Não sei para que serve - Wandrey

        #region Impressao do DANFE

        DANFEImpressao,
        DANFEImpressao_Contingencia,

        #endregion Impressao do DANFE

        #region Impressao do relatorio de e-mails do DANFE

        DANFERelatorio,

        #endregion Impressao do relatorio de e-mails do DANFE

        #region LMC

        /// <summary>
        /// Envio do XML de LMC
        /// </summary>
        LMCAutorizacao,

        #endregion LMC

        DFeEnviar,

        #region EFDReinf

        RecepcaoLoteReinf,
        ConsultarLoteReinf,
        ConsultasReinf,

        #endregion EFDReinf

        #region eSocial

        RecepcaoLoteeSocial,
        ConsultarLoteeSocial,
        ConsultarIdentificadoresEventoseSocial,
        DownloadEventoseSocial,

        #endregion eSocial

        /// <summary>
        /// Nulo / Nenhum serviço em execução
        /// </summary>
        Nulo
    }

    #endregion Servicos

    #region TipoAplicativo

    public enum TipoAplicativo
    {
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs da NF-e e NFC-e
        /// </summary>
        ///
        [Description("NF-e e NFC-e")]
        Nfe = 0,

        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do CT-e
        /// </summary>
        [Description("CT-e")]
        Cte = 1,

        /// <summary>
        /// Aplicativo ou servicos para processamento dos XMLs da NFS-e
        /// </summary>
        [Description("NFS-e")]
        Nfse = 2,

        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do MDF-e
        /// </summary>
        [Description("MDF-e")]
        MDFe = 3,

        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs da NFC-e
        /// </summary>
        [Description("NFC-e")]
        NFCe = 4,

        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do SAT
        /// </summary>
        [Description("SAT")]
        SAT = 5,

        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do EFD Reinf
        /// </summary>
        [Description("EFD Reinf")]
        EFDReinf = 6,

        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do eSocial
        /// </summary>
        [Description("eSocial")]
        eSocial = 7,

        /// <summary>
        /// Aplicativo ou seviços para processamento dos XMLs de EFD Reinf e eSocial
        /// </summary>
        [Description("EFD Reinf e eSocial")]
        EFDReinfeSocial = 8,

#if _fw46

        [Description("NF-e, NFC-e, CT-e, MDF-e, EFD Reinf e eSocial")]
        Todos = 10,

#else
        [Description("NF-e, NFC-e, CT-e, MDF-e")]
        Todos = 10,
#endif

        [Description("")]
        Nulo = 100
    }

    #endregion TipoAplicativo

    #region Padrão NFSe

    public enum PadroesNFSe
    {
        /// <summary>
        /// Não Identificado
        /// </summary>
        [Description("Não identificado")]
        NaoIdentificado,

        /// <summary>
        /// Padrão GINFES
        /// </summary>
        [Description("GINFES")]
        GINFES,

        /// <summary>
        /// Padrão da BETHA Sistemas
        /// </summary>
        [Description("BETHA")]
        BETHA,

        /// <summary>
        /// Padrão da BETHA versão 2.02
        /// </summary>
        [Description("BETHA 2.02")]
        BETHA202,

        /// <summary>
        /// Padrão da THEMA Informática
        /// </summary>
        [Description("THEMA")]
        THEMA,

        /// <summary>
        /// Padrão da prefeitura de Salvador-BA
        /// </summary>
        [Description("Salvador-BA")]
        SALVADOR_BA,

        /// <summary>
        /// Padrão da prefeitura de Canoas-RS
        /// </summary>
        [Description("Canoas-RS")]
        CANOAS_RS,

        /// <summary>
        /// Padrão da ISS Net / Nota Control Tecnologia
        /// </summary>
        [Description("ISS Net")]
        ISSNET,

        /// <summary>
        /// Padrão ISS On-Line/Assessor Público
        /// Padrão da prefeitura de Barra Mansa-RJ
        /// </summary>
        [Description("ISS On-line/Assessor Público")]
        ISSONLINE_ASSESSORPUBLICO,
             
        /// <summary>
        /// Padrão da prefeitura de Juiz de Fora-MG
        /// </summary>
        [Description("BHISS")]
        BHISS,

        /// <summary>
        /// Padrao GIF
        /// Prefeitura de Campo Bom-RS
        /// </summary>
        [Description("GIF/Infisc")]
        GIF,

        /// <summary>
        /// Padrão IPM
        /// <para>Prefeitura de Campo Mourão.</para>
        /// </summary>
        [Description("IPM")]
        IPM,

        /// <summary>
        /// Padrão DUETO
        /// Prefeitura de Nova Santa Rita - RS
        /// </summary>
        [Description("Dueto")]
        DUETO,

        /// <summary>
        /// Padrão WEB ISS
        /// Prefeitura de Feira de Santana - BA
        /// </summary>
        [Description("Web ISS")]
        WEBISS,

        /// <summary>
        /// Padrão WEB ISS
        /// Versão do XML 2.02
        /// </summary>
        [Description("Web ISS")]
        WEBISS_202,

        /// <summary>
        /// Padrão Nota Fiscal Eletrônica Paulistana -
        /// Prefeitura São Paulo - SP
        /// </summary>
        [Description("Paulistana")]
        PAULISTANA,

        /// <summary>
        /// Padrão Nota Fiscal Eletrônica Porto Velhense
        /// Prefeitura de Porto Velho - RO
        /// </summary>
        [Description("Portovelhense")]
        PORTOVELHENSE,

        /// <summary>
        /// Padrão Nota Fiscal Eletrônica da PRONIN (GovBR)
        /// Prefeitura de Mirassol - SP
        /// </summary>
        [Description("Pronin")]
        PRONIN,

        /// <summary>
        /// Padrão Nota Fiscal Eletrônica ISS-ONline da 4R Sistemas
        /// Prefeitura de Governador Valadares - SP
        /// </summary>
        [Description("ISS On-Line/4R")]
        ISSONLINE4R,

        /// <summary>
        /// Padrão Nota Fiscal eletrônica DSF
        /// Prefeitura de Campinas - SP
        /// Prefeitura de Campo Grande - MS
        /// </summary>
        [Description("DSF")]
        DSF,

        /// <summary>
        /// Padrão Tecno Sistemas
        /// Prefeitura de Portão - RS
        /// </summary>
        [Description("Tecno Sistemas")]
        TECNOSISTEMAS,

        /// <summary>
        /// Padrão System-PRO
        /// Prefeitura de Erechim - RS
        /// </summary>
        [Description("System-Pro")]
        SYSTEMPRO,

        /// <summary>
        /// Preifetura de Macaé - RJ
        /// </summary>
        [Description("Tiplan")]
        TIPLAN,

        /// <summary>
        /// Preifetura de Niterói - RJ
        /// </summary>
        [Description("Tiplan")]
        TIPLAN_203,

        /// <summary>
        /// Prefeitura do Rio de Janeiro - RJ
        /// </summary>
        [Description("Carioca")]
        CARIOCA,

        /// <summary>
        /// Prefeitura de Bauru - SP
        /// </summary>
        [Description("SigCorp/SigISS")]
        SIGCORP_SIGISS,

        /// <summary>
        /// SigCorp_SigISS versao 2.03
        /// </summary>
        [Description("SigCorp/SigISS")]
        SIGCORP_SIGISS_203,

        /// <summary>
        /// Padrão SmaraPD
        /// Prefeitura de Sertãozinho - SP
        /// </summary>
        [Description("SmaraPD")]
        SMARAPD,

        /// <summary>
        /// SMARAPD na versão 2.04
        /// </summary>
        [Description("SmaraPD")]
        SMARAPD_204,

        /// <summary>
        /// Padrão Fiorilli
        /// Prefeitura de Taquara - SP
        /// </summary>
        [Description("Fiorilli")]
        FIORILLI,

        /// <summary>
        /// Padrão Fintel
        /// Prefeitura de Ponta Grossa - PR
        /// </summary>
        [Description("Fintel")]
        FINTEL,

        /// <summary>
        /// Padrão ISSWEB
        /// Prefeitura de Mairipora - SP
        /// </summary>
        [Description("ISSWeb")]
        ISSWEB,

        /// <summary>
        /// Padrão SimplIss
        /// Prefeitura de Piracicaba - SP
        /// </summary>
        [Description("SimplIss")]
        SIMPLISS,

        /// <summary>
        /// Padrão Conam
        /// Prefeitura de Varginha - MG
        /// </summary>
        [Description("CONAM")]
        CONAM,

        /// <summary>
        /// Padrão Rlz Informatica
        /// Prefeitura de Santa Fé do Sul - PR
        /// </summary>
        [Description("Rlz Informática")]
        RLZ_INFORMATICA,

        /// <summary>
        /// Padrão E-Governe
        /// Prefeitura de Curitiba - PR
        /// </summary>
        [Description("E-Governe")]
        EGOVERNE,

        /// <summary>
        /// Padrão E-Governe
        /// Prefeitura de Osasco - SP
        /// </summary>
        [Description("EGoverne ISS")]
        EGOVERNEISS,

        /// <summary>
        /// Padrão E&L
        /// Prefeitura de Simões Filho - BA
        /// </summary>
        [Description("E&L")]
        EL,

        /// <summary>
        /// Padrao GOV-Digital
        /// Prefeitura de Divinopolis-MG
        /// </summary>
        [Description("Gov-Digital")]
        GOVDIGITAL,

        /// <summary>
        /// Padrão Equiplano
        /// Prefeitura de Toledo - PR
        /// </summary>
        [Description("Equiplano")]
        EQUIPLANO,

        /// <summary>
        /// Padrão Prodata
        /// Prefeitura de Itumbiara - GO
        /// </summary>
        [Description("Prodata")]
        PRODATA,

        /// <summary>
        /// Padrão VVISS
        /// Prefeitura de Vila Velha - ES
        /// </summary>
        [Description("VVISS")]
        VVISS,

        /// <summary>
        /// Padrão FISSLEX
        /// Prefeitura Sinop - MT
        /// </summary>
        [Description("FISSLEX")]
        FISSLEX,

        /// <summary>
        /// Padrão EloTech
        /// </summary>
        [Description("EloTech")]
        ELOTECH,

        /// <summary>
        /// Padrão MGM
        /// Prefeitura de Penapolis - SP
        /// </summary>
        [Description("MGM")]
        MGM,

        /// <summary>
        /// Padrão Natalense
        /// Prefeitura de Natal - RN
        /// </summary>
        [Description("Natalense")]
        NATALENSE,

        /// <summary>
        /// Padrão Consist
        /// Prefeitura de Patos de Minas - MG
        /// </summary>
        [Description("Consist")]
        CONSIST,

        /// <summary>
        /// Padrão de Goiania
        /// </summary>
        [Description("Goiania")]
        GOIANIA,

        /// <summary>
        /// Padrão Nota Inteligente
        /// Prefeitura de Claudio - MG
        /// </summary>
        [Description("Nota Inteligente")]
        NOTAINTELIGENTE,

        /// <summary>
        /// Padrão Memory
        /// Prefeitura de Ponte Nova - MG
        /// </summary>
        [Description("Memory")]
        MEMORY,

        /// <summary>
        /// Prefeitura de Camaçari - BA
        /// </summary>
        [Description("Camaçari-BA")]
        CAMACARI_BA,

        /// <summary>
        /// Padrão N&A Informática
        /// </summary>
        [Description("N&A Informática")]
        NA_INFORMATICA,

        /// <summary>
        /// PAdrão ABACO
        /// Prefeitura de Rondonópolis - MT
        /// </summary>
        [Description("ABACO")]
        ABACO,

        /// <summary>
        /// Padrão Metrópolis
        /// </summary>
        [Description("Metrópolis")]
        METROPOLIS,

        /// <summary>
        /// Padrão Actcon
        /// </summary>
        [Description("Portal Fácil/Actcon")]
        PORTALFACIL_ACTCON,

        /// <summary>
        /// Padrão Actcon
        /// </summary>
        [Description("Portal Fácil/Actcon")]
        PORTALFACIL_ACTCON_202,

        /// <summary>
        /// Padrão Pública
        /// </summary>
        [Description("Pública")]
        PUBLICA,

        /// <summary>
        /// Padrão BSIT-BR / SIGEP
        /// </summary>
        [Description("BSIT-BR / SIGEP")]
        BSITBR,

        /// <summary>
        /// Padrão ABASE Sistemas
        /// </summary>
        [Description("ABASE")]
        ABASE,

        /// <summary>
        /// Lexsom
        /// </summary>
        [Description("LEXSOM")]
        LEXSOM,

        /// <summary>
        /// SH3
        /// </summary>
        [Description("SH3")]
        SH3,

        /// <summary>
        /// Coplan
        /// </summary>
        [Description("Coplan")]
        COPLAN,

        /// <summary>
        /// Super Nova
        /// </summary>
        [Description("Super Nova")]
        SUPERNOVA,

        /// <summary>
        /// DBSELLER
        /// </summary>
        [Description("DBSELLER")]
        DBSELLER,

        /// <summary>
        /// Prefeitura de Maringá-PR
        /// </summary>
        [Description("MARINGA_PR")]
        MARINGA_PR,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Bauru-SP
        /// </summary>
        [Description("BAURU_SP")]
        BAURU_SP,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Jaboatão dos Guararapes-PE
        /// </summary>
        [Description("TINUS")]
        TINUS,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Eusébio-CE
        /// </summary>
        [Description("INTERSOL")]
        INTERSOL,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Florianópolis-SC
        /// </summary>
        [Description("SOFTPLAN")]
        SOFTPLAN,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Manaus-AM
        /// </summary>
        [Description("MANAUS_AM")]
        MANAUS_AM,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Joinville-SC
        /// </summary>
        JOINVILLE_SC,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Pelotas-RS
        /// </summary>
        [Description("AVMB/ASTEN")]
        AVMB_ASTEN,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Lorena-SP
        /// </summary>
        [Description("EMBRAS")]
        EMBRAS,

        /// <summary>
        /// Padrão utilizado pela prefeitura Paragominas-PA
        /// </summary>
        [Description("Desenvolve Cidade")]
        DESENVOLVECIDADE,

        /// <summary>
        /// Padrão utilizada pela prefeitura de Vitória-ES
        /// </summary>
        [Description("VITORIA_ES")]
        VITORIA_ES,

        /// <summary>
        /// Padrão utilizada pela prefeitura de Nova Friburgo-RJ
        /// </summary>
        [Description("MODERNIZACAO_PUBLICA")]
        MODERNIZACAO_PUBLICA,
        /// <summary>
        /// Padrão utilizado pelo município de Montes Claros-MG
        /// </summary>
        [Description("e-Receita")]
        E_RECEITA,

        /// <summary>
        /// Padrão utilizado pelo município de Amargosa-BA
        /// </summary>
        [Description("ADM_SISTEMAS")]
        ADM_SISTEMAS,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Paulista-PE
        /// </summary>
        [Description("PUBLIC_SOFT")]
        PUBLIC_SOFT,

        /// <summary>
        /// Padrão utilizado pelo município de Cromínia-GO
        /// </summary>
        [Description("MEGASOFT")]
        MEGASOFT,

        /// <summary>s
        /// Padrão CECAM
        /// </summary>
        [Description("CECAM")]
        CECAM,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Camboriú-SC
        /// </summary>
        [Description("SIMPLE")]
        SIMPLE,

        /// <summary>
        /// Padrão utilizado pela prefeitura de Indaiatuba-SP
        /// </summary>
        [Description("INDAIATUBA_SP")]
        INDAIATUBA_SP,

        /// <summary>
        /// Padrão utilizado pela prefeitura de João Pessoa-PB
        /// </summary>
        [Description("SISPMJP")]
        SISPMJP,

         /// <summary>s
         /// Padrão D2TI
         /// </summary>
        [Description("D2TI")]
        D2TI,

        /// <summary>s
        /// Padrão IIBRASIL
        /// </summary>
        [Description("IIBRASIL")]
        IIBRASIL,

        /// <summary>
        /// Padrão WEBFISCO_TECNOLOGIA
        /// </summary>
        [Description("WEBFISCO_TECNOLOGIA")]
        WEBFISCO_TECNOLOGIA
        ///***ATENÇÃO***
        ///o nome deste enum tem que coincidir com o nome da url, pq faço um "IndexOf" deste enum para pegar o padrao
    }

    #endregion Padrão NFSe

    #region Classe dos tipos de ambiente da NFe

    /// <summary>
    /// Tipo de ambiente
    /// </summary>
    public enum TipoAmbiente
    {
        [Description("Produção")]
        taProducao = 1,

        [Description("Homologação")]
        taHomologacao = 2
    }

    #endregion Classe dos tipos de ambiente da NFe

    /// <summary>
    /// Regime tributação ISSQN
    /// </summary>
    public enum RegTribISSQN
    {
        [Description("1 - Micro Empresa Municipal")]
        MicroEmpresaMunicipal = 1,

        [Description("2 - Estimativa")]
        Estimativa = 2,

        [Description("3 - Sociedade de Profissionais")]
        SociedadeDeProfissionais = 3,

        [Description("4 - Cooperativa")]
        Cooperativa = 4,

        [Description("5 - Micro Empresário Individual (MEI)")]
        MicroEmpresarioIndividual = 5
    }

    /// <summary>
    /// Informa se o Desconto sobre
    /// subtotal deve ser rateado entre
    /// os itens sujeitos à tributação pelo ISSQN.
    /// </summary>
    public enum IndRatISSQN
    {
        [Description("Sim")]
        S,

        [Description("Não")]
        N
    }

    #region TipoEmissao

    /// <summary>
    /// TipoEmissao
    /// </summary>
    public enum TipoEmissao
    {
        [Description("")]
        teNone = 0,

        [Description("Normal")]
        teNormal = 1,

        [Description("Contingência com FS")]
        teFS = 2,

        [Description("Contingência com EPEC")]
        teEPEC = 4,

        [Description("Contingência com FS-DA")]
        teFSDA = 5,

        [Description("Contingência com SVC-AN")]
        teSVCAN = 6,

        [Description("Contingência com SVC-RS")]
        teSVCRS = 7,

        [Description("Contingência com SVC-SP")]
        teSVCSP = 8,

        [Description("Contingência Off-Line (NFC-e)")]
        teOffLine = 9
    }

    #endregion TipoEmissao

    #region Erros Padrões

    public enum ErroPadrao
    {
        ErroNaoDetectado = 0,
        FalhaInternet = 1,
        FalhaEnvioXmlWS = 2,
        CertificadoVencido = 3,
        FalhaEnvioXmlNFeWS = 5,
        CertificadoNaoEncontrado = 6
    }

    #endregion Erros Padrões

    #region EnumHelper

    /*
ComboBox combo = new ComboBox();
combo.DataSource = EnumHelper.ToList(typeof(SimpleEnum));
combo.DisplayMember = "Value";
combo.ValueMember = "Key";

        foreach (string value in Enum.GetNames(typeof(Model.TipoCampanhaSituacao)))
        {
            Model.TipoCampanhaSituacao stausEnum = (Model.TipoCampanhaSituacao)Enum.Parse(typeof(Model.TipoCampanhaSituacao), value);
            Console.WriteLine(" Description: " + value+"  "+ Model.EnumHelper.GetDescription(stausEnum));
        }

 */

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class AttributeTipoAplicacao : Attribute
    {
        private TipoAplicativo aplicacao;

        public TipoAplicativo Aplicacao
        {
            get
            {
                return this.aplicacao;
            }
        }

        public AttributeTipoAplicacao(TipoAplicativo aplicacao)
            : base()
        {
            this.aplicacao = aplicacao;
        }
    }

    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }

    /// <summary>
    /// Classe com metodos para serem utilizadas nos Enuns
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Retorna a description do enum
        /// </summary>
        /// <param name="value">Enum para buscar a description</param>
        /// <returns>Retorna a description do enun</returns>
        /*public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }*/

        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }

        /// <summary>
        /// Gets the <see cref="DescriptionAttribute"/> of an <see cref="Enum"/> type value.
        /// </summary>
        /// <param name="value">The <see cref="Enum"/> type value.</param>
        /// <returns>A string containing the text of the <see cref="DescriptionAttribute"/>.</returns>
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            EnumDescriptionAttribute[] attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            else
            {
                return GetEnumItemDescription(value);
                //DescriptionAttribute[] dattributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                //if (dattributes != null && dattributes.Length > 0)
                //description = dattributes[0].Description;
            }
            return description;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetEnumItemDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        ///  Converts the <see cref="Enum"/> type to an <see cref="IList"/> compatible object.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> type.</param>
        /// <returns>An <see cref="IList"/> containing the enumerated type value and description.</returns>
        public static IList ToList(Type type, bool returnInt, bool excluibrancos)
        {
            return ToList(type, returnInt, excluibrancos, "");
        }

        public static IList ToList(Type type, bool returnInt, bool excluibrancos, string eliminar)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                string _descr = GetDescription(value);
                if (excluibrancos && string.IsNullOrEmpty(_descr)) continue;

                if (eliminar.IndexOf(Convert.ToInt32(value).ToString()) != -1) continue;

                if (returnInt)
                    list.Add(new KeyValuePair<int, string>(Convert.ToInt32(value), _descr));
                else
                    list.Add(new KeyValuePair<Enum, string>(value, _descr));
            }

            return list;
        }

        public static IList ToStrings(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(GetDescription(value));
            }

            return list;
        }
    }

    #endregion EnumHelper
}
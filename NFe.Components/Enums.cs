using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

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
        Denegados
    }
    #endregion

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
        ConsultaStatusServicoNFe,
        /// <summary>
        /// Assinar e montar lote de uma NFe
        /// </summary>
        MontarLoteUmaNFe,
        /// <summary>
        /// Envia os lotes de NFe para os webservices (NfeRecepcao)
        /// </summary>
        EnviarLoteNfe,
        /// <summary>
        /// Envia os lotes de NFe para os webservices (NFeAutorizacao)
        /// </summary>
        EnviarLoteNfe2,
        /// <summary>
        /// Consulta recibo do lote nfe (NFeRetRecepcao)
        /// </summary>
        PedidoSituacaoLoteNFe,
        /// <summary>
        /// Consulta recibo do lote nfe (NFeRetAutorizacao)
        /// </summary>
        PedidoSituacaoLoteNFe2,        
        /// <summary>
        /// Consulta situação da NFe
        /// </summary>
        PedidoConsultaSituacaoNFe,
        /// <summary>
        /// Envia XML de Inutilização da NFe
        /// </summary>
        InutilizarNumerosNFe,
        /// <summary>
        /// Somente converter TXT da NFe para XML de NFe
        /// </summary>
        ConverterTXTparaXML,

        #region Eventos NFe
        /// <summary>
        /// Enviar XML de Evento NFe
        /// </summary>
        RecepcaoEvento,
        /// <summary>
        /// Enviar XML Evento - Carta de Correção
        /// </summary>
        EnviarCCe,
        /// <summary>
        /// Enviar XML Evento - Cancelamento
        /// </summary>
        EnviarEventoCancelamento,
        /// <summary>
        /// Enviar um evento de manifestacao
        /// </summary>
        EnviarManifDest,
        #endregion

        /// <summary>
        /// Assinar e validar um XML de NFe no envio em Lote
        /// </summary>
        AssinarValidarNFeEnvioEmLote,
        /// <summary>
        /// Assinar e montar lote de várias NFe
        /// </summary>
        MontarLoteVariasNFe,
        /// <summary>
        /// Monta chave de acesso
        /// </summary>
        GerarChaveNFe,
        /// <summary>
        /// Enviar o XML do DPEC para o SCE - Sistema de Contingência Eletrônica
        /// </summary>
        EnviarDPEC,
        /// <summary>
        /// Consultar o registro do DPEC no SCE - Sistema de Contingência Eletrônica        
        /// </summary>
        ConsultarDPEC,
        /// <summary>
        /// Enviar solicitacao de download de nfe
        /// </summary>
        DownloadNFe,
        /// <summary>
        /// Enviar uma consulta de nfe de destinatario
        /// </summary>
        ConsultaNFDest,
        /// <summary>
        /// Registro de saida
        /// </summary>
        RegistroDeSaida,
        /// <summary>
        /// Registro de saida
        /// </summary>
        RegistroDeSaidaCancelamento,
        #endregion

        #region CTe
        /// <summary>
        /// Consulta Status Serviço CTe
        /// </summary>
        ConsultaStatusServicoCTe,
        /// <summary>
        /// Montar lote de um CTe
        /// </summary>
        MontarLoteUmCTe,
        /// <summary>
        /// Envia os lotes de CTe para os webservices
        /// </summary>
        EnviarLoteCTe,
        /// <summary>
        /// Consulta recibo do lote CTe
        /// </summary>
        PedidoSituacaoLoteCTe,
        /// <summary>
        /// Consulta situação da CTe
        /// </summary>
        PedidoConsultaSituacaoCTe,
        /// <summary>
        /// Envia XML de Inutilização da CTe
        /// </summary>
        InutilizarNumerosCTe,
        /// <summary>
        /// Enviar XML Evento CTe
        /// </summary>
        RecepcaoEventoCTe,
        /// <summary>
        /// Assinar e validar um XML de CTe no envio em Lote
        /// </summary>
        AssinarValidarCTeEnvioEmLote,
        /// <summary>
        /// Assinar e montar lote de várias CTe
        /// </summary>
        MontarLoteVariosCTe,
        #endregion

        #region NFSe
        /// <summary>
        /// Enviar Lote RPS NFS-e 
        /// </summary>
        RecepcionarLoteRps,
        /// <summary>
        /// Consultar Situação do lote RPS NFS-e
        /// </summary>
        ConsultarSituacaoLoteRps,
        /// <summary>
        /// Consultar NFS-e por RPS
        /// </summary>
        ConsultarNfsePorRps,
        /// <summary>
        /// Consultar NFS-e por NFS-e
        /// </summary>
        ConsultarNfse,
        /// <summary>
        /// Consultar lote RPS
        /// </summary>
        ConsultarLoteRps,
        /// <summary>
        /// Cancelar NFS-e
        /// </summary>
        CancelarNfse,
        /// <summary>
        /// Consultar a URL de visualização da NFSe
        /// </summary>
        ConsultarURLNfse,
        #endregion

        #region MDFe
        /// <summary>
        /// Consulta Status Serviço MDFe
        /// </summary>
        ConsultaStatusServicoMDFe,
        /// <summary>
        /// Montar lote de um MDFe
        /// </summary>
        MontarLoteUmMDFe,
        /// <summary>
        /// Envia os lotes de MDFe para os webservices
        /// </summary>
        EnviarLoteMDFe,
        /// <summary>
        /// Consulta recibo do lote MDFe
        /// </summary>
        PedidoSituacaoLoteMDFe,
        /// <summary>
        /// Consulta situação da MDFe
        /// </summary>
        PedidoConsultaSituacaoMDFe,
        /// <summary>
        /// Assinar e validar um XML de MDFe no envio em Lote
        /// </summary>
        AssinarValidarMDFeEnvioEmLote,
        /// <summary>
        /// Assinar e montar lote de várias MDFe
        /// </summary>
        MontarLoteVariosMDFe,
        /// <summary>
        /// Enviar XML Evento MDFe
        /// </summary>
        RecepcaoEventoMDFe,
        #endregion

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
        #endregion

        #region Serviços gerais
        /// <summary>
        /// Consultar Informações Gerais do UniNFe
        /// </summary>
        ConsultaInformacoesUniNFe,
        /// <summary>
        /// Solicitar ao UniNFe que altere suas configurações
        /// </summary>
        AlterarConfiguracoesUniNFe,
        /// <summary>
        /// Efetua uma limpeza das pastas que recebem arquivos temporários
        /// </summary>
        LimpezaTemporario,
        /// <summary>
        /// Consultas efetuadas pela pasta GERAL.
        /// </summary>
        ConsultaGeral,
        /// <summary>
        /// Consulta Certificados Instalados na estação do UniNFe.
        /// </summary>
        ConsultaCertificados,
        #endregion

        #region Não sei para que serve - Wandrey
        /// <summary>
        /// WSExiste
        /// </summary>
        WSExiste,
        #endregion

        /// <summary>
        /// Nulo / Nenhum serviço em execução
        /// </summary>        
        Nulo
    }
    #endregion

    #region TipoAplicativo
    public enum TipoAplicativo
    {
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs da NF-e
        /// </summary>
        Nfe = 0,
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do CT-e
        /// </summary>
        Cte = 1,
        /// <summary>
        /// Aplicativo ou servicos para processamento dos XMLs da NFS-e
        /// </summary>
        Nfse = 2,
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do MDF-e
        /// </summary>
        MDFe = 3,
        Nulo = 100
    }
    #endregion

    #region Padrão NFSe
    public enum PadroesNFSe
    {
        /// <summary>
        /// Não Identificado
        /// </summary>
        NaoIdentificado,
        /// <summary>
        /// Padrão GINFES
        /// </summary>
        GINFES,
        /// <summary>
        /// Padrão da BETHA Sistemas
        /// </summary>
        BETHA,
        /// <summary>
        /// Padrão da THEMA Informática
        /// </summary>
        THEMA,
        /// <summary>
        /// Padrão da prefeitura de Salvador-BA
        /// </summary>
        SALVADOR_BA,
        /// <summary>
        /// Padrão da prefeitura de Canoas-RS
        /// </summary>
        CANOAS_RS,
        /// <summary>
        /// Padrão da ISS Net
        /// </summary>    
        ISSNET,
        /// <summary>
        /// Padrão da prefeitura de Apucarana-PR
        /// Padrão da prefeitura de Aracatuba-SP
        /// </summary>
        ISSONLINE,
        /// <summary>
        /// Padrão da prefeitura de Blumenau-SC
        /// </summary>
        BLUMENAU_SC,
        /// <summary>
        /// Padrão da prefeitura de Juiz de Fora-MG
        /// </summary>
        BHISS,
        /// <summary>
        /// Padrao GIF
        /// Prefeitura de Campo Bom-RS
        /// </summary>
        GIF,
        /// <summary>
        /// Padrão IPM
        /// <para>Prefeitura de Campo Mourão.</para>
        /// </summary>
        IPM,
        /// <summary>
        /// Padrão DUETO
        /// Prefeitura de Nova Santa Rita - RS
        /// </summary>
        DUETO,
        /// <summary>
        /// Padrão WEB ISS
        /// Prefeitura de Feira de Santana - BA
        /// </summary>
        WEBISS,
        /// <summary>
        /// Padrão Nota Fiscal Eletrônica Paulistana -
        /// Prefeitura São Paulo - SP
        /// </summary>
        PAULISTANA,
        /// <summary>
        /// Padrão Nota Fiscal Eletrônica Porto Velhense
        /// Prefeitura de Porto Velho - RO
        /// </summary>
        PORTOVELHENSE,
        /// <summary>
        /// Padrão Nota Fiscal Eletrônica da PRONIN (GovBR)
        /// Prefeitura de Mirassol - SP
        /// </summary>
        PRONIN

        ///Atencao Wandrey.
        ///o nome deste enum tem que coincidir com o nome da url, pq faço um "IndexOf" deste enum para pegar o padrao
    }
    #endregion

    #region NF
    /// <summary>
    /// Tipo de ambiente
    /// </summary>
    public enum TpAmb
    {
        /// <summary>
        /// Ambiente de produção
        /// </summary>
        [Description("Produção")]
        Producao = 1,

        /// <summary>
        /// Ambiente de homologação
        /// </summary>
        [Description("Homologação")]
        Homologacao = 2
    }
    #endregion

    #region Erros Padrões
    public enum ErroPadrao
    {
        ErroNaoDetectado = 0,
        FalhaInternet = 1,
        FalhaEnvioXmlWS = 2,
        CertificadoVencido = 3,
        FalhaEnvioXmlWSDPEC = 4, //danasa 21/10/2010
        FalhaEnvioXmlNFeWS = 5
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Text;

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
        /// <summary>
        /// Assina, valida e envia o XML de cancelamento de NFe para o webservice
        /// </summary>
        CancelarNFe,
        /// <summary>
        /// Assina, valida e envia o XML de Inutilização de números de NFe para o webservice
        /// </summary>
        InutilizarNumerosNFe,
        /// <summary>
        /// Valida e envia o XML de pedido de Consulta da Situação da NFe para o webservice
        /// </summary>
        PedidoConsultaSituacaoNFe,
        /// <summary>
        /// Valida e envia o XML de pedido de Consulta Status dos Serviços da NFe para o webservice
        /// </summary>
        PedidoConsultaStatusServicoNFe,
        /// <summary>
        /// Valida e envia o XML de pedido de Consulta da Situação do Lote da NFe para o webservice
        /// </summary>
        PedidoSituacaoLoteNFe,
        /// <summary>
        /// Valida e envia o XML de pedido de Consulta do Cadastro do Contribuinte para o webservice
        /// </summary>
        ConsultaCadastroContribuinte,
        /// <summary>
        /// Consultar Informações Gerais do UniNFe
        /// </summary>
        ConsultaInformacoesUniNFe,
        /// <summary>
        /// Solicitar ao UniNFe que altere suas configurações
        /// </summary>
        AlterarConfiguracoesUniNFe,
        /// <summary>
        /// Assinar e montar lote de uma NFe
        /// </summary>
        MontarLoteUmaNFe,
        /// <summary>
        /// Assinar e montar lote de várias NFe
        /// </summary>
        MontarLoteVariasNFe,
        /// <summary>
        /// Envia os lotes de notas fiscais eletrônicas para os webservices
        /// </summary>
        EnviarLoteNfe,
        /// <summary>
        /// Somente assinar e validar o XML
        /// </summary>
        AssinarValidar,
        /// <summary>
        /// Somente converter TXT da NFe para XML de NFe
        /// </summary>
        ConverterTXTparaXML,
        /// <summary>
        /// Monta chave de acesso
        /// </summary>
        GerarChaveNFe,
        /// <summary>
        /// Efetua verificações nas notas em processamento para evitar algumas falhas e perder retornos de autorização de notas
        /// </summary>
        EmProcessamento,
        /// <summary>
        /// Efetua uma limpeza das pastas que recebem arquivos temporários
        /// </summary>
        LimpezaTemporario,
        /// <summary>
        /// Enviar o XML do DPEC para o SCE - Sistema de Contingência Eletrônica
        /// </summary>
        EnviarDPEC,
        /// <summary>
        /// Consultar o registro do DPEC no SCE - Sistema de Contingência Eletrônica        
        /// </summary>
        ConsultarDPEC,
        /// <summary>
        /// Assinar e validar um XML de NFe
        /// </summary>
        AssinarValidarNFe,
        /// <summary>
        /// Enviar uma carta de correcao
        /// </summary>
        EnviarCCe,
        /// <summary>
        /// Enviar um evento de cancelamento
        /// </summary>
        EnviarEventoCancelamento,
        /// <summary>
        /// Enviar solicitacao de download de nfe
        /// </summary>
        DownloadNFe,
        /// <summary>
        /// Enviar uma consulta de nfe de destinatario
        /// </summary>
        ConsultaNFeDest,
        /// <summary>
        /// Enviar um evento de manifestacao
        /// </summary>
        EnviarManifestacao,
        /// <summary>
        /// Nulo / Nenhum serviço em execução
        /// </summary>        
        Nulo,
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
        CancelarNfse
    }
    #endregion

    #region TipoAplicativo
    public enum TipoAplicativo
    {
        /// <summary>
        /// Aplicativo de conhecimento de transporte eletrônico
        /// </summary>
        Cte,
        /// <summary>
        /// Aplicativo de nota fiscal eletrônica
        /// </summary>
        Nfe,
        /// <summary>
        /// Aplicativo de nota fiscal de serviços eletrônica
        /// </summary>
        Nfse
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
        CANOAS_RS

        ///Atencao Wandrey.
        ///o nome deste enum tem que coincidir com o nome da url, pq faço um "IndexOf" deste enum para pegar o padrao
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

using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Components
{
    /// <summary>
    /// Classe responsável por definir uma lista dos arquivos de SCHEMAS para validação dos XMLs
    /// </summary>
    public class SchemaXML
    {
        /// <summary>
        /// Informações dos schemas para validação dos XML
        /// </summary>
        public static Dictionary<string, InfSchema> InfSchemas = new Dictionary<string, InfSchema>();
        /// <summary>
        /// O Maior ID que tem na lista
        /// </summary>
        //public static int MaxID { get; set; }
        public static int MaxID { get { return InfSchemas.Count; } }

        /// <summary>
        /// Cria várias listas com as TAG´s de identificação dos XML´s e seus Schemas
        /// </summary>
        /// <date>31/07/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void CriarListaIDXML()
        {
            InfSchemas.Clear();

            int id = 0;

            #region NFe

            #region NFe versão 2.0

            #region XML Distribuição Cancelamento
            InfSchemas.Add("NFE-procCancNFe", new InfSchema()
            {
                Tag = "procCancNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\procCancNFe_v{0}.xsd",
                Descricao = "XML de distribuição do Cancelamento da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Cadastro Contribuinte
            InfSchemas.Add("NFE-ConsCad", new InfSchema()
            {
                Tag = "ConsCad",
                ID = ++id,
                ArquivoXSD = "NFe\\consCad_v{0}.xsd",
                Descricao = "XML de Consulta do Cadastro do Contribuinte",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Recibo Lote
            InfSchemas.Add("NFE-consReciNFe", new InfSchema()
            {
                Tag = "consReciNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\consReciNfe_v{0}.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Situação NFe
            InfSchemas.Add("NFE-consSitNFe", new InfSchema()
            {
                Tag = "consSitNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\consSitNFe_v{0}.xsd",
                Descricao = "XML de Consulta da Situação da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Status Serviço NFe
            InfSchemas.Add("NFE-consStatServ", new InfSchema()
            {
                Tag = "consStatServ",
                ID = ++id,
                ArquivoXSD = "NFe\\consStatServ_v{0}.xsd",
                Descricao = "XML de Consulta da Situação do Serviço da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio Lote
            InfSchemas.Add("NFE-enviNFe", new InfSchema()
            {
                Tag = "enviNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\enviNFe_v{0}.xsd",
                Descricao = "XML de Envio de Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Inutilização
            InfSchemas.Add("NFE-inutNFe", new InfSchema()
            {
                Tag = "inutNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\inutNFe_v{0}.xsd",
                Descricao = "XML de Inutilização de Numerações de Notas Fiscais Eletrônicas",
                TagAssinatura = "inutNFe",
                TagAtributoId = "infInut",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML NFe
            InfSchemas.Add("NFE-NFe", new InfSchema()
            {
                Tag = "NFe",
                ID = ++id,
                ArquivoXSD = "NFe\\nfe_v{0}.xsd",
                Descricao = "XML da Nota Fiscal Eletrônica",
                TagAssinatura = "NFe",
                TagAtributoId = "infNFe",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Inutilização
            InfSchemas.Add("NFE-procInutNFe", new InfSchema()
            {
                Tag = "procInutNFe",
                ID = ++id,
                //ArquivoXSD = "NFe\\procInutNFe_v2.00.xsd",
                ArquivoXSD = "NFe\\procInutNFe_v{0}.xsd",
                Descricao = "XML de distribuição de Inutilização de Números de NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição NFe
            InfSchemas.Add("NFE-nfeProc", new InfSchema()
            {
                Tag = "nfeProc",
                ID = ++id,
                ArquivoXSD = "NFe\\procNFe_v{0}.xsd",
                Descricao = "XML de distribuição da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #endregion

            #region NFe versão 3.1

            #region XML Consulta Recibo Lote
            /*InfSchemas.Add("NFE-3.10-consReciNFe", new InfSchema()
            {
                Tag = "consReciNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\consReciNfe_v3.10.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Consulta Situação NFe
            /*InfSchemas.Add("NFE-3.10-consSitNFe", new InfSchema()
            {
                Tag = "consSitNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\consSitNFe_v3.10.xsd",
                Descricao = "XML de Consulta da Situação da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Consulta Status Serviço NFe
            /*InfSchemas.Add("NFE-3.10-consStatServ", new InfSchema()
            {
                Tag = "consStatServ",
                ID = ++id,
                ArquivoXSD = "NFe\\consStatServ_v3.10.xsd",
                Descricao = "XML de Consulta da Situação do Serviço da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Envio Lote
            /*InfSchemas.Add("NFE-3.10-enviNFe", new InfSchema()
            {
                Tag = "enviNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\enviNFe_v3.10.xsd",
                Descricao = "XML de Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Inutilização
            /*InfSchemas.Add("NFE-3.10-inutNFe", new InfSchema()
            {
                Tag = "inutNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\inutNFe_v3.10.xsd",
                Descricao = "XML de Inutilização de Numerações de Notas Fiscais Eletrônicas",
                TagAssinatura = "inutNFe",
                TagAtributoId = "infInut",
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML NFe
            /*InfSchemas.Add("NFE-3.10-NFe", new InfSchema()
            {
                Tag = "NFe",
                ID = ++id,
                ArquivoXSD = "NFe\\nfe_v3.10.xsd",
                Descricao = "XML da Nota Fiscal Eletrônica",
                TagAssinatura = "NFe",
                TagAtributoId = "infNFe",
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Distribuição Inutilização
            /*InfSchemas.Add("NFE-3.10-procInutNFe", new InfSchema()
            {
                Tag = "procInutNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\procInutNFe_v3.10.xsd",
                Descricao = "XML de distribuição de Inutilização de Números de NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Distribuição NFe
            /*InfSchemas.Add("NFE-3.10-nfeProc", new InfSchema()
            {
                Tag = "nfeProc",
                ID = ++id,
                ArquivoXSD = "NFe\\procNFe_v3.10.xsd",
                Descricao = "XML de distribuição da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Consulta Cadastro Contribuinte
            /*InfSchemas.Add("NFE-3.10-ConsCad", new InfSchema()
            {
                Tag = "ConsCad",
                ID = ++id,
                ArquivoXSD = "NFe\\consCad_v3.10.xsd",
                Descricao = "XML de Consulta do Cadastro do Contribuinte",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #endregion

            #region XML Gerais da NFe

            #region XML Recepção EPEC
            InfSchemas.Add("NFE-envEvento110140", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EPEC\\envEPEC_v1.00.xsd",
                //ArquivoXSD = "EPEC\\eventoEPEC_v0.01.xsd",
                Descricao = "XML de registro do EPEC (Sistema de Contingência Eletrônica) NF-e",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Recepção EPEC
            InfSchemas.Add("NFE-envEvento-65-110140", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EPEC\\eventoEPEC_v0.01.xsd",
                Descricao = "XML de registro do EPEC (Sistema de Contingência Eletrônica) NFC-e",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio CCe
            InfSchemas.Add("NFE-envEvento110110", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "CCe\\envCCe_v1.00.xsd",
                Descricao = "XML de registro de envio da CCe da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de Eventos de cancelamento
            InfSchemas.Add("NFE-envEvento110111", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EventoCanc\\envEventoCancNFe_v1.00.xsd",
                Descricao = "XML de evento de cancelamento da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210200
            InfSchemas.Add("NFE-envEvento210200", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EventoManifestaDestinat\\e210200_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210210
            InfSchemas.Add("NFE-envEvento210210", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EventoManifestaDestinat\\e210210_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210220
            InfSchemas.Add("NFE-envEvento210220", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EventoManifestaDestinat\\e210220_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210240
            InfSchemas.Add("NFE-envEvento210240", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EventoManifestaDestinat\\e210240_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento pedido de prorrogação 1º. prazo - 111500
            InfSchemas.Add("NFE-envEvento111500", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "", //"NFe\\envPProrrogNFe_v1.0.xsd",
                Descricao = "Evento pedido de prorrogação 1º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento pedido de prorrogação 2º. prazo - 111501
            InfSchemas.Add("NFE-envEvento111501", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "", //"NFe\\envPProrrogNFe_v1.0.xsd",
                Descricao = "Evento pedido de prorrogação 2º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Cancelamento de Pedido de Prorrogação 1º. Prazo - 111502
            InfSchemas.Add("NFE-envEvento111502", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "", //"NFe\\envCancelPProrrogNFe_v1.0.xsd",
                Descricao = "Evento Cancelamento de pedido de prorrogação 1º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Cancelamento de Pedido de Prorrogação 2º. Prazo - 111503
            InfSchemas.Add("NFE-envEvento111503", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "", //"NFe\\envCancelPProrrogNFe_v1.0.xsd",
                Descricao = "Evento Cancelamento de pedido de prorrogação 2º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            /*
            #region Evento Fisco Resposta ao Pedido de Prorrogação 1º prazo - 411500
            InfSchemas.Add("NFE-envEvento411500", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Pedido de Prorrogação 1º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Fisco Resposta ao Pedido de Prorrogação 2º prazo - 411501
            InfSchemas.Add("NFE-envEvento411501", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Pedido de Prorrogação 2º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Fisco Resposta ao Pedido de Prorrogação 1º prazo - 411502
            InfSchemas.Add("NFE-envEvento411502", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Cancelamento de Prorrogação 1º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Fisco Resposta ao Pedido de Prorrogação 2º prazo - 411503
            InfSchemas.Add("NFE-envEvento411503", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Cancelamento de Prorrogação 2º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion
            */

            #region XML Envio de consulta de nfe
            InfSchemas.Add("NFE-consNFeDest", new InfSchema()
            {
                Tag = "nfeConsultaNFDest",
                ID = ++id,
                ArquivoXSD = "ConsultaNFDest\\consNFeDest_v1.01.xsd",
                Descricao = "XML de consulta de NFe do destinatário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de download de nfe
            InfSchemas.Add("NFE-downloadNFe", new InfSchema()
            {
                Tag = "downloadNFe",
                ID = ++id,
                ArquivoXSD = "DownloadNFe\\downloadNFe_v1.00.xsd",
                Descricao = "XML de download de nfe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de confirmacao de recebimento de manifestacoes
            InfSchemas.Add("NFE-envConfRecebto", new InfSchema()
            {
                Tag = "envEvento",
                ID = ++id,
                ArquivoXSD = "EventoManifestaDestinat\\envConfRecebto_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de registro de saida
#if nao
            InfSchemas.Add("NFE-envRegistroSaida", new InfSchema()
            {
                Tag = "envRegistro",
                ID = ++id,
                ArquivoXSD = "envRegistro_v1.00.xsd",
                Descricao = "XML de registro de envio de registro de saida",
                TagAssinatura = "evento",
                TagAtributoId = "infRegistro",
                TargetNameSpace = string.Empty
            });
#endif
            #endregion

            #region XML Envio de cancelamento registro de saida
#if nao
            InfSchemas.Add("NFE-envCancRegistroSaida", new InfSchema()
            {
                Tag = "envCancRegistro",
                ID = ++id,
                ArquivoXSD = "envCancRegistro_v1.00.xsd",
                Descricao = "XML de registro de envio de cancelamento de registro de saida",
                TagAssinatura = "evento",
                TagAtributoId = "infCancRegistro",
                TargetNameSpace = string.Empty
            });
#endif
            #endregion

            #endregion

            #endregion

            #region MDF-e

            #region XML Envio do MDFe
            InfSchemas.Add("NFE-MDFe", new InfSchema()
            {
                Tag = "MDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\mdfe_v1.00.xsd",
                Descricao = "XML do Manifesto Eletrônico de Documentos Fiscais",
                TagAssinatura = "MDFe",
                TagAtributoId = "infMDFe",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML Envio Lote MDFe
            InfSchemas.Add("NFE-enviMDFe", new InfSchema()
            {
                Tag = "enviMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\enviMDFe_v1.00.xsd",
                Descricao = "XML de Envio de Lote de Manifesto Eletrônico de Docimentos Fiscais",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de consulta recibo do MDFe
            InfSchemas.Add("NFE-consReciMDFe", new InfSchema()
            {
                Tag = "consReciMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\consReciMDFe_v1.00.xsd",
                Descricao = "XML de consulta recibo MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de consulta situação do MDFe
            InfSchemas.Add("NFE-consSitMDFe", new InfSchema()
            {
                Tag = "consSitMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\consSitMDFe_v1.00.xsd",
                Descricao = "XML de consulta situação do MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de consulta status dos serviços do MDFe
            InfSchemas.Add("NFE-consStatServMDFe", new InfSchema()
            {
                Tag = "consStatServMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\consStatServMDFe_v1.00.xsd",
                Descricao = "XML de consulta status do serviço do MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Evento de Cancelamento do MDFe
            InfSchemas.Add("NFE-eventoMDFe110111", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de cancelamento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Evento de Encerramento do MDFe
            InfSchemas.Add("NFE-eventoMDFe110112", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de encerramento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Evento de Inclusão de Condutor
            InfSchemas.Add("NFE-eventoMDFe110114", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de encerramento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Eventos Gerais do MDFe
            InfSchemas.Add("NFE-eventoMDFe310620", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = ++id,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de registro de passagem do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Eventos Gerais do MDFe
            InfSchemas.Add("NFE-consMDFeNaoEnc", new InfSchema()
            {
                Tag = "consMDFeNaoEnc",
                ID = ++id,
                ArquivoXSD = "MDFe\\consMDFeNaoEnc_v1.00.xsd",
                Descricao = "Pedido de Consulta MDF-e não encerrados",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #endregion

            #region CTe

            #region XML Distribuição NFe
            SchemaXML.InfSchemas.Add("NFE-cteProc", new InfSchema()
            {
                Tag = "cteProc",
                ID = ++id,
                ArquivoXSD = "CTe\\procCTe_v2.00.xsd",
                Descricao = "XML de distribuição do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Distribuição Cancelamento
            SchemaXML.InfSchemas.Add("NFE-procCancCTe", new InfSchema()
            {
                Tag = "procCancCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\procCancCTe_v1.04.xsd",
                Descricao = "XML de distribuição do Cancelamento do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Distribuição Inutilização
            SchemaXML.InfSchemas.Add("NFE-procInutCTe", new InfSchema()
            {
                Tag = "procInutCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\procInutCTe_v2.00.xsd",
                Descricao = "XML de distribuição de Inutilização de Números do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML NFe
            SchemaXML.InfSchemas.Add("NFE-CTe", new InfSchema()
            {
                Tag = "CTe",
                ID = ++id,
                ArquivoXSD = "CTe\\cte_v2.00.xsd",
                Descricao = "XML de Conhecimento de Transporte Eletrônico",
                TagAssinatura = "CTe",
                TagAtributoId = "infCte",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Envio Lote
            SchemaXML.InfSchemas.Add("NFE-enviCTe", new InfSchema()
            {
                Tag = "enviCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\enviCTe_v2.00.xsd",
                Descricao = "XML de Envio de Lote dos Conhecimentos de Transportes Eletrônicos",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Cancelamento
            SchemaXML.InfSchemas.Add("NFE-eventoCTe110111", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\eventoCTe_v2.00.xsd",
                Descricao = "XML de Cancelamento do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Inutilização
            SchemaXML.InfSchemas.Add("NFE-inutCTe", new InfSchema()
            {
                Tag = "inutCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\inutCTe_v2.00.xsd",
                Descricao = "XML de Inutilização de Numerações do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "inutCTe",
                TagAtributoId = "infInut",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Consulta Situação NFe
            SchemaXML.InfSchemas.Add("NFE-consSitCTe", new InfSchema()
            {
                Tag = "consSitCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\consSitCTe_v2.00.xsd",
                Descricao = "XML de Consulta da Situação do Conhecimento de Transporte Eletrônico",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Consulta Recibo Lote
            SchemaXML.InfSchemas.Add("NFE-consReciCTe", new InfSchema()
            {
                Tag = "consReciCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\consReciCTe_v2.00.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Conhecimentos de Transportes Eletrônicos",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Consulta Situação Serviço NFe
            SchemaXML.InfSchemas.Add("NFE-consStatServCte", new InfSchema()
            {
                Tag = "consStatServCte",
                ID = ++id,
                ArquivoXSD = "CTe\\consStatServCTe_v2.00.xsd",
                Descricao = "XML de Consulta da Situação do Serviço do Conhecimento de Transporte Eletrônico",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML Envio CCe por envento
            SchemaXML.InfSchemas.Add("NFE-eventoCTe110110", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\eventoCTe_v2.00.xsd",
                Descricao = "XML de registro de envio da CCe da CTe",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE

            });
            #endregion

            #region XML Envio Registro Multimodal
            SchemaXML.InfSchemas.Add("NFE-envEvento110160", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\eventoCTe_v2.00.xsd",
                Descricao = "XML de registro de envio Registro Multimodal do CTe",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE

            });
            #endregion

            #region XML de Envio do evento de contingencia EPEC
            SchemaXML.InfSchemas.Add("NFE-eventoCTe110140"/*"NFE-eventoCTe110113"*/, new InfSchema()
            {
                Tag = "eventoCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\eventoCTe_v2.00.xsd",
                Descricao = "XML de Envio do evento de contingencia EPEC",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #region XML de Envio do evento de Registro Multimodal
            SchemaXML.InfSchemas.Add("NFE-eventoCTe110160", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\eventoCTe_v2.00.xsd",
                Descricao = "XML de Envio do evento de Registro Multimodal",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #endregion

            #region Distribuição de DFe´s
            InfSchemas.Add("NFE-distDFeInt", new InfSchema()
            {
                Tag = "distDFeInt",
                ID = ++id,
                ArquivoXSD = "DFe\\distDFeInt_v1.00.xsd",
                Descricao = "XML de consulta de documentos fiscais eletrônicos",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region LMC
            InfSchemas.Add("NFE-autorizacao", new InfSchema()
            {
                Tag = "autorizacao",
                ID = ++id,
                ArquivoXSD = "LMC\\autorizacao_v1.00.xsd",
                Descricao = "XML do Livro de Movimentação de Combustíveis (LMC)",
                TagAssinatura = "livroCombustivel",
                TagAtributoId = "infLivroCombustivel",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_LMC
            });
            #endregion
        }
    }

    public class InfSchema
    {
        /// <summary>
        /// TAG do XML que identifica qual XML é
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Identificador único numérico do XML 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Breve descrição do arquivo XML
        /// </summary>
        public string Descricao { get; set; }
        /// <summary>
        /// Nome do arquivo de schema para validar o XML
        /// </summary>
        public string ArquivoXSD { get; set; }
        /// <summary>
        /// Nome da tag do XML que será assinada
        /// </summary>
        public string TagAssinatura { get; set; }
        /// <summary>
        /// Nome da tag que tem o atributo ID
        /// </summary>
        public string TagAtributoId { get; set; }
        /// <summary>
        /// Nome da tag de lote do XML que será assinada
        /// </summary>
        public string TagLoteAssinatura { get; set; }
        /// <summary>
        /// Nome da tag de lote que tem o atributo ID
        /// </summary>
        public string TagLoteAtributoId { get; set; }
        /// <summary>
        /// URL do schema de cada XML
        /// </summary>
        public string TargetNameSpace { get; set; }
    }
}

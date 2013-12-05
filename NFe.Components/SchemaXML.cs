﻿using System;
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
        public static int MaxID { get; set; }

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

            #region XML Distribuição NFe
            InfSchemas.Add("NFE-nfeProc", new InfSchema()
            {
                Tag = "nfeProc",
                ID = ++id,
                ArquivoXSD = "NFe\\procNFe_v2.00.xsd",
                Descricao = "XML de distribuição da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Cancelamento
            InfSchemas.Add("NFE-procCancNFe", new InfSchema()
            {
                Tag = "procCancNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\procCancNFe_v2.00.xsd",
                Descricao = "XML de distribuição do Cancelamento da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Inutilização
            InfSchemas.Add("NFE-procInutNFe", new InfSchema()
            {
                Tag = "procInutNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\procInutNFe_v2.00.xsd",
                Descricao = "XML de distribuição de Inutilização de Números de NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML NFe
            InfSchemas.Add("NFE-NFe", new InfSchema()
            {
                Tag = "NFe",
                ID = ++id,
                ArquivoXSD = "NFe\\nfe_v2.00.xsd",
                Descricao = "XML da Nota Fiscal Eletrônica",
                TagAssinatura = "NFe",
                TagAtributoId = "infNFe",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio Lote
            InfSchemas.Add("NFE-enviNFe", new InfSchema()
            {
                Tag = "enviNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\enviNFe_v2.00.xsd",
                Descricao = "XML de Envio de Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Cancelamento
            InfSchemas.Add("NFE-cancNFe", new InfSchema()
            {
                Tag = "cancNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\cancNFe_v2.00.xsd",
                Descricao = "XML de Cancelamento de Nota Fiscal Eletrônica",
                TagAssinatura = "cancNFe",
                TagAtributoId = "infCanc",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Inutilização
            InfSchemas.Add("NFE-inutNFe", new InfSchema()
            {
                Tag = "inutNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\inutNFe_v2.00.xsd",
                Descricao = "XML de Inutilização de Numerações de Notas Fiscais Eletrônicas",
                TagAssinatura = "inutNFe",
                TagAtributoId = "infInut",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Situação NFe
            InfSchemas.Add("NFE-consSitNFe", new InfSchema()
            {
                Tag = "consSitNFe",
                ID = ++id,
                ArquivoXSD = "NFe\\consSitNFe_v2.01.xsd",
                Descricao = "XML de Consulta da Situação da Nota Fiscal Eletrônica",
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
                ArquivoXSD = "NFe\\consReciNfe_v2.00.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Situação Serviço NFe
            InfSchemas.Add("NFE-consStatServ", new InfSchema()
            {
                Tag = "consStatServ",
                ID = ++id,
                ArquivoXSD = "NFe\\consStatServ_v2.00.xsd",
                Descricao = "XML de Consulta da Situação do Serviço da Nota Fiscal Eletrônica",
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
                ArquivoXSD = "NFe\\consCad_v2.00.xsd",
                Descricao = "XML de Consulta do Cadastro do Contribuinte",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta DPEC
            InfSchemas.Add("NFE-consDPEC", new InfSchema()
            {
                Tag = "consDPEC",
                ID = ++id,
                ArquivoXSD = "DPEC\\consDPEC_v1.01.xsd",
                Descricao = "XML de consulta do DPEC no SCE (Sistema de Contingência Eletrônica)",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Recepção DPEC
            InfSchemas.Add("NFE-envDPEC", new InfSchema()
            {
                Tag = "envDPEC",
                ID = ++id,
                ArquivoXSD = "DPEC\\envDPEC_v1.01.xsd",
                Descricao = "XML de registro do DPEC no SCE (Sistema de Contingência Eletrônica)",
                TagAssinatura = "envDPEC",
                TagAtributoId = "infDPEC",
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
            #endregion

            #region XML Envio de cancelamento registro de saida
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/mdfe"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"

            });
            #endregion

            #region XML de Envio do evento de contingencia EPEC
            SchemaXML.InfSchemas.Add("NFE-eventoCTe110113", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = ++id,
                ArquivoXSD = "CTe\\eventoCTe_v2.00.xsd",
                Descricao = "XML de Envio do evento de contingencia EPEC",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
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
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
            });
            #endregion

            #endregion

            #region Determinar a propriedade MaxID
            MaxID = 0;
            foreach (InfSchema item in InfSchemas.Values)
            {
                if (item.ID > MaxID)
                    MaxID = item.ID;
            }
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

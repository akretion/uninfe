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
        public static int MaxID { get; set; }

        /// <summary>
        /// Cria várias listas com as TAG´s de identificação dos XML´s e seus Schemas
        /// </summary>
        /// <date>31/07/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void CriarListaIDXML()
        {
            InfSchemas.Clear();

            #region XML Distribuição NFe
            InfSchemas.Add("NFE-nfeProc", new InfSchema()
            {
                Tag = "nfeProc",
                ID = 9,
                ArquivoXSD = "NFe\\procNFe_v2.00.xsd",
                Descricao = "XML de distribuição da NFe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Cancelamento
            InfSchemas.Add("NFE-procCancNFe", new InfSchema()
            {
                Tag = "procCancNFe",
                ID = 10,
                ArquivoXSD = "NFe\\procCancNFe_v2.00.xsd",
                Descricao = "XML de distribuição do Cancelamento da NFe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Inutilização
            InfSchemas.Add("NFE-procInutNFe", new InfSchema()
            {
                Tag = "procInutNFe",
                ID = 11,
                ArquivoXSD = "NFe\\procInutNFe_v2.00.xsd",
                Descricao = "XML de distribuição de Inutilização de Números de NFe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML NFe
            InfSchemas.Add("NFE-NFe", new InfSchema()
            {
                Tag = "NFe",
                ID = 1,
                ArquivoXSD = "NFe\\nfe_v2.00.xsd",
                Descricao = "XML de Nota Fiscal Eletrônica",
                TagAssinatura = "NFe",
                TagAtributoId = "infNFe",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio Lote
            InfSchemas.Add("NFE-enviNFe", new InfSchema()
            {
                Tag = "enviNFe",
                ID = 2,
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
                ID = 3,
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
                ID = 4,
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
                ID = 5,
                ArquivoXSD = "NFe\\consSitNFe_v2.00.xsd",
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
                ID = 6,
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
                ID = 7,
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
                ID = 8,
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
                ID = 12,
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
                ID = 13,
                ArquivoXSD = "DPEC\\envDPEC_v1.01.xsd",
                Descricao = "XML de registro do DPEC no SCE (Sistema de Contingência Eletrônica)",
                TagAssinatura = "envDPEC",                
                TagAtributoId = "infDPEC",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio CCe
            InfSchemas.Add("NFE-envEvento", new InfSchema()
            {
                Tag = "envEvento",
                ID = 14,
                ArquivoXSD = "CCe\\envCCe_v1.00.xsd",
                Descricao = "XML de registro de envio da CCe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de Eventos de cancelamento
            InfSchemas.Add("NFE-envEvento110111", new InfSchema()
            {
                Tag = "envEvento",
                ID = 15,
                ArquivoXSD = "EventoCanc\\envEventoCancNFe_v1.00.xsd",
                Descricao = "XML de registro de envio de evento de cancelamento",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210200
            InfSchemas.Add("NFE-envEvento210200", new InfSchema()
            {
                Tag = "envEvento",
                ID = 16,
                ArquivoXSD = "EventoManifestaDestinat\\e210200_v1.00.xsd",
                Descricao = "XML de registro de envio de evento de manifestacao",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210210
            InfSchemas.Add("NFE-envEvento210210", new InfSchema()
            {
                Tag = "envEvento",
                ID = 17,
                ArquivoXSD = "EventoManifestaDestinat\\e210210_v1.00.xsd",
                Descricao = "XML de registro de envio de evento de manifestacao",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210220
            InfSchemas.Add("NFE-envEvento210220", new InfSchema()
            {
                Tag = "envEvento",
                ID = 19,
                ArquivoXSD = "EventoManifestaDestinat\\e210220_v1.00.xsd",
                Descricao = "XML de registro de envio de evento de manifestacao",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210240
            InfSchemas.Add("NFE-envEvento210240", new InfSchema()
            {
                Tag = "envEvento",
                ID = 19,
                ArquivoXSD = "EventoManifestaDestinat\\e210240_v1.00.xsd",
                Descricao = "XML de registro de envio de evento de manifestacao",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de consulta de nfe
            InfSchemas.Add("NFE-consNFeDest", new InfSchema()
            {
                Tag = "nfeConsultaNFDest",
                ID = 20,
                ArquivoXSD = "ConsultaNFDest\\consNFeDest_v1.01.xsd",
                Descricao = "XML de registro de envio de consulta a nfe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de download de nfe
            InfSchemas.Add("NFE-downloadNFe", new InfSchema()
            {
                Tag = "downloadNFe",
                ID = 21,
                ArquivoXSD = "DownloadNFe\\downloadNFe_v1.00.xsd",
                Descricao = "XML de registro de download de nfe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de confirmacao de recebimento de manifestacoes
            InfSchemas.Add("NFE-envConfRecebto", new InfSchema()
            {
                Tag = "envEvento",
                ID = 22,
                ArquivoXSD = "EventoManifestaDestinat\\envConfRecebto_v1.00.xsd",
                Descricao = "XML de registro de envio de evento de manifestacao",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de registro de saida
            InfSchemas.Add("NFE-envRegistroSaida", new InfSchema()
            {
                Tag = "envRegistro",
                ID = 23,
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
                ID = 24,
                ArquivoXSD = "envCancRegistro_v1.00.xsd",
                Descricao = "XML de registro de envio de cancelamento de registro de saida",
                TagAssinatura = "evento",
                TagAtributoId = "infCancRegistro",
                TargetNameSpace = string.Empty
            });
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

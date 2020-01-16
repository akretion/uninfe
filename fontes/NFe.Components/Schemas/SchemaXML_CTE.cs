namespace NFe.Components
{
    public class SchemaXML_CTE
    {
        public static void CriarListaIDXML()
        {
            #region CTe

            #region XML Distribuição CTe

            SchemaXML.InfSchemas.Add("NFE-cteProc", new InfSchema()
            {
                Tag = "cteProc",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\procCTe_v{0}.xsd",
                Descricao = "XML de distribuição do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Distribuição CTe

            #region XML Distribuição CTe

            SchemaXML.InfSchemas.Add("NFE-cteOSProc", new InfSchema()
            {
                Tag = "cteOSProc",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\procCTeOS_v{0}.xsd",
                Descricao = "XML de distribuição do CTeOS com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Distribuição CTe

            #region XML Distribuição Cancelamento

            SchemaXML.InfSchemas.Add("NFE-procCancCTe", new InfSchema()
            {
                Tag = "procCancCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\procCancCTe_v{0}.xsd",
                Descricao = "XML de distribuição do Cancelamento do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Distribuição Cancelamento

            #region XML Distribuição Inutilização

            SchemaXML.InfSchemas.Add("NFE-procInutCTe", new InfSchema()
            {
                Tag = "procInutCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\procInutCTe_v{0}.xsd",
                Descricao = "XML de distribuição de Inutilização de Números do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Distribuição Inutilização

            #region XML CTe

            SchemaXML.InfSchemas.Add("NFE-CTe", new InfSchema()
            {
                Tag = "CTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cte_v{0}.xsd",
                Descricao = "XML de Conhecimento de Transporte Eletrônico",
                TagAssinatura = "CTe",
                TagAtributoId = "infCte",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML CTe

            #region XML Envio Lote

            SchemaXML.InfSchemas.Add("NFE-enviCTe", new InfSchema()
            {
                Tag = "enviCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\enviCTe_v{0}.xsd",
                Descricao = "XML de Envio de Lote dos Conhecimentos de Transportes Eletrônicos",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Envio Lote

            #region XML Cancelamento

            SchemaXML.InfSchemas.Add("NFE-eventoCTe110111", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de Cancelamento do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Cancelamento

            #region XML Comprovante de entrega 

            SchemaXML.InfSchemas.Add("NFE-eventoCTe110180", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de Comprovante de entrega do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Comprovante de entrega

            #region XML Cancelamento Comprovante de entrega

            SchemaXML.InfSchemas.Add("NFE-eventoCTe110181", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de Cancelamento Comprovante de entrega do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Cancelamento Comprovante de entrega

            #region XML Inutilização

            SchemaXML.InfSchemas.Add("NFE-inutCTe", new InfSchema()
            {
                Tag = "inutCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\inutCTe_v{0}.xsd",
                Descricao = "XML de Inutilização de Numerações do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "inutCTe",
                TagAtributoId = "infInut",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Inutilização

            #region XML Consulta Situação CTe

            SchemaXML.InfSchemas.Add("NFE-consSitCTe", new InfSchema()
            {
                Tag = "consSitCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\consSitCTe_v{0}.xsd",
                Descricao = "XML de Consulta da Situação do Conhecimento de Transporte Eletrônico",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Consulta Situação CTe

            #region XML Consulta Recibo Lote

            SchemaXML.InfSchemas.Add("NFE-consReciCTe", new InfSchema()
            {
                Tag = "consReciCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\consReciCTe_v{0}.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Conhecimentos de Transportes Eletrônicos",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Consulta Recibo Lote

            #region XML Consulta Situação Serviço NFe

            SchemaXML.InfSchemas.Add("NFE-consStatServCte", new InfSchema()
            {
                Tag = "consStatServCte",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\consStatServCTe_v{0}.xsd",
                Descricao = "XML de Consulta da Situação do Serviço do Conhecimento de Transporte Eletrônico",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Consulta Situação Serviço NFe

            #region XML Envio CCe por envento

            SchemaXML.InfSchemas.Add("NFE-eventoCTe110110", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de registro de envio da CCe da CTe",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Envio CCe por envento

            #region XML Envio Registro Multimodal

            SchemaXML.InfSchemas.Add("NFE-envEvento110160", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de registro de envio Registro Multimodal do CTe",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML Envio Registro Multimodal

            #region XML de Envio do evento de contingencia EPEC

            SchemaXML.InfSchemas.Add("NFE-eventoCTe110113", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de Envio do evento de contingencia EPEC",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML de Envio do evento de contingencia EPEC

            #region XML de Envio do evento de Registro Multimodal

            SchemaXML.InfSchemas.Add("NFE-eventoCTe110160", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de Envio do evento de Registro Multimodal",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML de Envio do evento de Registro Multimodal

            #region XML de Envio do evento de Prestação do Serviço em Desacordo

            SchemaXML.InfSchemas.Add("NFE-eventoCTe610110", new InfSchema()
            {
                Tag = "eventoCTe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v{0}.xsd",
                Descricao = "XML de Envio do evento de Registro Multimodal",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML de Envio do evento de Prestação do Serviço em Desacordo

            #region XML de CT-e OS, modelo 67

            SchemaXML.InfSchemas.Add("NFE-CTeOS", new InfSchema()
            {
                Tag = "CTeOS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cteOS_v{0}.xsd",
                Descricao = "XML de CT-e OS, modelo 67",
                TagAssinatura = "CTeOS",
                TagAtributoId = "infCte",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion XML de CT-e OS, modelo 67

            #region Schemas Modal

            #region Rodoviário

            SchemaXML.InfSchemas.Add("NFE-rodo-CTe", new InfSchema()
            {
                Tag = "rodo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cteModalRodoviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Rodoviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion Rodoviário

            #region Ferroviário

            SchemaXML.InfSchemas.Add("NFE-ferrov-CTe", new InfSchema()
            {
                Tag = "ferrov",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cteModalFerroviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Ferroviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion Ferroviário

            #region Dutoviário

            SchemaXML.InfSchemas.Add("NFE-duto-CTe", new InfSchema()
            {
                Tag = "duto",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cteModalDutoviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Dutoviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion Dutoviário

            #region Aeroviário

            SchemaXML.InfSchemas.Add("NFE-aereo-CTe", new InfSchema()
            {
                Tag = "aereo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cteModalAereo_v{0}.xsd",
                Descricao = "XML de CTe - Modal Aeroviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion Aeroviário

            #region Aquaviário

            SchemaXML.InfSchemas.Add("NFE-aquav-CTe", new InfSchema()
            {
                Tag = "aquav",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cteModalAquaviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Aquaviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion Aquaviário

            #region Multimodal

            SchemaXML.InfSchemas.Add("NFE-multimodal-CTe", new InfSchema()
            {
                Tag = "aquav",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\cteMultiModal_v{0}.xsd",
                Descricao = "XML de CTe - Multi Modal",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });

            #endregion Multimodal

            #endregion Schemas Modal

            #endregion CTe
        }
    }
}
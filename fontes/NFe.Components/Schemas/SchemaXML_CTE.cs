using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NFe.Components
{
    public class SchemaXML_CTE
    {
        public static void CriarListaIDXML()
        {
            #region CTe

            #region XML Distribuição NFe
            SchemaXML.InfSchemas.Add("NFE-cteProc", new InfSchema()
            {
                Tag = "cteProc",
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
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
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CTe\\eventoCTe_v2.00.xsd",
                Descricao = "XML de Envio do evento de Registro Multimodal",
                TagAssinatura = "eventoCTe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NFe.Components;

namespace unicte
{
    class SchemaXMLCte
    {
        public static void CriarListaIDXML()
        {
            SchemaXML.InfSchemas.Clear();

            #region XML Distribuição NFe
            SchemaXML.InfSchemas.Add("CTE-cteProc", new InfSchema()
            {
                Tag = "cteProc",
                ID = 9,
                ArquivoXSD = "CTe\\procCTe_v1.04.xsd",
                Descricao = "XML de distribuição do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Cancelamento
            SchemaXML.InfSchemas.Add("CTE-procCancCTe", new InfSchema()
            {
                Tag = "procCancCTe",
                ID = 10,
                ArquivoXSD = "CTe\\procCancCTe_v1.04.xsd",
                Descricao = "XML de distribuição do Cancelamento do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Inutilização
            SchemaXML.InfSchemas.Add("CTE-procInutCTe", new InfSchema()
            {
                Tag = "procInutCTe",
                ID = 11,
                ArquivoXSD = "CTe\\procInutCTe_v1.04.xsd",
                Descricao = "XML de distribuição de Inutilização de Números do CTe com protocolo de autorização anexado",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML NFe
            SchemaXML.InfSchemas.Add("CTE-CTe", new InfSchema()
            {
                Tag = "CTe",
                ID = 1,
                ArquivoXSD = "CTe\\cte_v1.04.xsd",
                Descricao = "XML de Conhecimento de Transporte Eletrônico",
                TagAssinatura = "CTe",
                TagAtributoId = "infCte",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio Lote
            SchemaXML.InfSchemas.Add("CTE-enviCTe", new InfSchema()
            {
                Tag = "enviCTe",
                ID = 2,
                ArquivoXSD = "CTe\\enviCte_v1.04.xsd",
                Descricao = "XML de Envio de Lote dos Conhecimentos de Transportes Eletrônicos",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Cancelamento
            SchemaXML.InfSchemas.Add("CTE-cancCTe", new InfSchema()
            {
                Tag = "cancCTe",
                ID = 3,
                ArquivoXSD = "CTe\\cancCte_v1.04.xsd",
                Descricao = "XML de Cancelamento do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "cancCTe",
                TagAtributoId = "infCanc",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Inutilização
            SchemaXML.InfSchemas.Add("CTE-inutCTe", new InfSchema()
            {
                Tag = "inutCTe",
                ID = 4,
                ArquivoXSD = "CTe\\inutCte_v1.04.xsd",
                Descricao = "XML de Inutilização de Numerações do Conhecimento de Transporte Eletrônico",
                TagAssinatura = "inutCTe",
                TagAtributoId = "infInut",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Situação NFe
            SchemaXML.InfSchemas.Add("CTE-consSitCTe", new InfSchema()
            {
                Tag = "consSitCTe",
                ID = 5,
                ArquivoXSD = "CTe\\consSitCte_v1.04.xsd",
                Descricao = "XML de Consulta da Situação do Conhecimento de Transporte Eletrônico",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Recibo Lote
            SchemaXML.InfSchemas.Add("CTE-consReciCTe", new InfSchema()
            {
                Tag = "consReciCTe",
                ID = 6,
                ArquivoXSD = "CTe\\consReciCte_v1.04.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Conhecimentos de Transportes Eletrônicos",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Situação Serviço NFe
            SchemaXML.InfSchemas.Add("CTE-consStatServCte", new InfSchema()
            {
                Tag = "consStatServCte",
                ID = 7,
                ArquivoXSD = "CTe\\consStatServCte_v1.04.xsd",
                Descricao = "XML de Consulta da Situação do Serviço do Conhecimento de Transporte Eletrônico",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Cadastro Contribuinte
            SchemaXML.InfSchemas.Add("CTE-ConsCad", new InfSchema()
            {
                Tag = "ConsCad",
                ID = 8,
                ArquivoXSD = "CTe\\consCad_v2.00.xsd",
                Descricao = "XML de Consulta do Cadastro do Contribuinte",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = "http://www.portalfiscal.inf.br/nfe"
            });
            #endregion

            #region Determinar a propriedade MaxID
            SchemaXML.MaxID = 0;
            foreach (InfSchema item in SchemaXML.InfSchemas.Values)
            {
                if (item.ID > SchemaXML.MaxID)
                    SchemaXML.MaxID = item.ID;
            }
            #endregion
        }
    }
}

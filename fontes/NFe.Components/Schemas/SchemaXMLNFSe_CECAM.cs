using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_CECAM
    {
        public static void CriarListaIDXML()
        {
            #region Pedido cancelamento

            SchemaXML.InfSchemas.Add("NFSE-CECAM-ISSECancelaNFe", new InfSchema()
            {
                Tag = "ISSECancelaNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CECAM\\XSDISSECancelaNFe.xsd",
                Descricao = "Pedido de cancelamento",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            #endregion Pedido cancelamento

            #region XML para Gerar NFse

            SchemaXML.InfSchemas.Add("NFSE-CECAM-NFEEletronica", new InfSchema()
            {
                Tag = "NFEEletronica",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CECAM\\XSDNFEletronica.xsd",
                Descricao = "XML de Lote RPS - Sincrono",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            #endregion XML para Gerar NFse

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-CECAM-ISSEConsultaNota", new InfSchema()
            {
                Tag = "ISSEConsultaNota",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CECAM\\XSDISSEConsultaNota.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            #endregion XML de Consulta de Lote RPS

        }
    }
}
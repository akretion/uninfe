using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_SOFTPLAN
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-SOFTPLAN-xmlProcessamentoNfpse", new InfSchema()
            {
                Tag = "xmlProcessamentoNfpse",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "xmlProcessamentoNfpse",
                TagAtributoId = "valorTotalServicos",
                TargetNameSpace = ""
            });

            #endregion XML de lote RPS
        }
    }
}
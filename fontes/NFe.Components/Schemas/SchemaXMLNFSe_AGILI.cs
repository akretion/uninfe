using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_AGILI
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-AGILI-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de cancelamento",
                TagAssinatura = "CancelarNfseEnvio",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            //SchemaXML.InfSchemas.Add("NFSE-SOFTPLAN-xmlProcessamentoNfpse", new InfSchema()
            //{
            //    Tag = "xmlProcessamentoNfpse",
            //    ID = SchemaXML.InfSchemas.Count + 1,
            //    ArquivoXSD = "",
            //    Descricao = "XML de Lote RPS",
            //    TagAssinatura = "xmlProcessamentoNfpse",
            //    TagAtributoId = "valorTotalServicos",
            //    TargetNameSpace = ""
            //});

            //SchemaXML.InfSchemas.Add("NFSE-SOFTPLAN-ConsultarNfseEnvio", new InfSchema()
            //{
            //    Tag = "ConsultarNfseEnvio",
            //    ID = SchemaXML.InfSchemas.Count + 1,
            //    ArquivoXSD = "",
            //    Descricao = "Consulta de NFSe por Código de Verificação e CMC(Inscrição Estadual)",
            //    TagAssinatura = "",
            //    TagAtributoId = "",
            //    TargetNameSpace = ""
            //});

            #endregion XML de lote RPS
        }
    }
}
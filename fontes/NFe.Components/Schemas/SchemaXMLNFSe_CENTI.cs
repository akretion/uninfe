using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_CENTI
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-CENTI-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS - Sincrono",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = ""
            });

            //SchemaXML.InfSchemas.Add("NFSE-SOFTPLAN-xmlCancelamentoNfpse", new InfSchema()
            //{
            //    Tag = "xmlCancelamentoNfpse",
            //    ID = SchemaXML.InfSchemas.Count + 1,
            //    ArquivoXSD = "",
            //    Descricao = "XML de cancelamento",
            //    TagAssinatura = "xmlCancelamentoNfpse",
            //    TagAtributoId = "codigoVerificacao",
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
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


            #endregion XML de lote RPS

            #region  XML de lote cancelamento

            SchemaXML.InfSchemas.Add("NFSE-CENTI-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = ""
            });

            #endregion XML de cancelamento

            #region  Consulta NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-SOFTPLAN-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            #endregion  Consulta NFSe por Rps

        }
    }
}
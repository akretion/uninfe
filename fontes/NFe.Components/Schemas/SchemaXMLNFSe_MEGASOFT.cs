using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_MEGASOFT
    {
        public static void CriarListaIDXML()
        {
            #region Consulta NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-MEGASOFT-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\MEGASOFT\\nfse_v01.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://megasoftarrecadanet.com.br/xsd/nfse_v01.xsd"
            });

            #endregion Consulta NFSe por Rps

            #region XML para Gerar NFse

            SchemaXML.InfSchemas.Add("NFSE-MEGASOFT-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\MEGASOFT\\nfse_v01.xsd",
                Descricao = "XML de Lote RPS - Sincrono",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://megasoftarrecadanet.com.br/xsd/nfse_v01.xsd"
            });

            #endregion XML para Gerar NFse
        }
    }
}
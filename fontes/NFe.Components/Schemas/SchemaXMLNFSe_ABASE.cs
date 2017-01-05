using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_ABASE
    {
        public static void CriarListaIDXML()
        {
            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-ABASE-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABASE\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfse.abase.com.br/nfse.xsd"
            });

            #endregion XML de Consulta de Lote RPS

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-ABASE-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABASE\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://nfse.abase.com.br/nfse.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-ABASE-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABASE\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://nfse.abase.com.br/nfse.xsd"
            });

            #endregion XML de Consulta Situação do Lote RPS

            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-ABASE-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABASE\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://nfse.abase.com.br/nfse.xsd"
            });

            #endregion XML de lote RPS

            #region Consulta NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-ABASE-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABASE\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfse.abase.com.br/nfse.xsd"
            });

            #endregion Consulta NFSe por Rps

            #region Consulta NFSe por período

            SchemaXML.InfSchemas.Add("NFSE-ABASE-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABASE\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfse.abase.com.br/nfse.xsd"
            });

            #endregion Consulta NFSe por período
        }
    }
}
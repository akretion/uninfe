using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_INTERSOL
    {
        public static void CriarListaIDXML()
        {
            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-INTERSOL-p:CancelarNfseEnvio", new InfSchema()
            {
                Tag = "p:CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\INTERSOL\\cancelar_nfse_envio_v1.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "p1:InfPedidoCancelamento",
                TargetNameSpace = "http://ws.speedgov.com.br/cancelar_nfse_envio_v1.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

            #region XML de Consultar Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-INTERSOL-p:ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "p:ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\INTERSOL\\consultar_lote_rps_envio_v1.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "p:ConsultarLoteRpsEnvio",
                TagAtributoId = "p:Protocolo",
                TargetNameSpace = "http://ws.speedgov.com.br/consultar_lote_rps_envio_v1.xsd"
            });

            #endregion XML de Consultar Lote RPS

            #region XML de Consultar NFse

            SchemaXML.InfSchemas.Add("NFSE-INTERSOL-p:ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "p:ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\INTERSOL\\consultar_nfse_envio_v1.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://ws.speedgov.com.br/consultar_nfse_envio_v1.xsd"
            });

            #endregion XML de Consultar NFse

            #region XML de Consulta de NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-INTERSOL-p:ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "p:ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\INTERSOL\\consultar_nfse_rps_envio_v1.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://ws.speedgov.com.br/consultar_nfse_rps_envio_v1.xsd"
            });

            #endregion XML de Consulta de NFSe por Rps

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-INTERSOL-p:ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "p:ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\INTERSOL\\consultar_situacao_lote_rps_envio_v1.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "p:ConsultarSituacaoLoteRpsEnvio",
                TagAtributoId = "p:Protocolo",
                TargetNameSpace = "http://ws.speedgov.com.br/consultar_situacao_lote_rps_envio_v1.xsd"
            });

            #endregion XML de Consulta Situação do Lote RPS

            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-INTERSOL-p:EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "p:EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\INTERSOL\\enviar_lote_rps_envio_v1.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "p1:Rps",
                TagAtributoId = "p1:InfRps",
                TagLoteAssinatura = "p:EnviarLoteRpsEnvio",
                TagLoteAtributoId = "p:LoteRps",
                TargetNameSpace = "http://ws.speedgov.com.br/enviar_lote_rps_envio_v1.xsd"
            });

            #endregion XML de lote RPS
        }
    }
}
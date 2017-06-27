using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_DBSELLER
    {
        public static void CriarListaIDXML()
        {
            #region DBSELLER

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DBSELLER-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DBSELLER-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DBSELLER-ConsultarSituacaoLoteRps", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion
            
            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-DBSELLER-ConsultarNfsePorRps", new InfSchema()
            {
                Tag = "ConsultarNfsePorRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion
            
            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-DBSELLER-CancelarNfse", new InfSchema()
            {
                Tag = "CancelarNfse",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion
            
            #endregion
        }
    }
}

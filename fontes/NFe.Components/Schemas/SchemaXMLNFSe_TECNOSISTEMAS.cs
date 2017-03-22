using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_TECNOSISTEMAS
    {
        public static void CriarListaIDXML()
        {
            #region Consulta NFSe

            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarNfseServicoPrestadoEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseServicoPrestadoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Consulta NFSe

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de Lote RPS

            #region Consulta NFSe por Faixa

            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Consulta NFSe por Faixa

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta Situação do Lote RPS

            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagLoteAtributoId = "LoteRps",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de lote RPS

            #region Consulta Sequencia Lote Nota RPS

            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarSequenciaLoteNotaRPSEnvio", new InfSchema()
            {
                Tag = "ConsultarSequenciaLoteNotaRPSEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta Sequencia do Lote da Nota RPS",
                TagLoteAtributoId = "",
                TagLoteAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Consulta Sequencia Lote Nota RPS
        }
    }
}
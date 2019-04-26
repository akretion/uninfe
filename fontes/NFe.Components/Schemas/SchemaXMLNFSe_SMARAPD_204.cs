using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_SMARAPD_204
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de lote RPS

            #region XML de lote RPS Síncrono

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de lote RPS Síncrono

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de Cancelamento de NFS-e

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de Consulta de Lote RPS

            #region XML de Consulta de NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de Consulta de NFSe por Rps

            #region XML de Consulta de NFSe por Faixa

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por Faixa

            #region Gerar NFSe Envio

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-GerarNfseEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion Gerar NFSe Envio

            #region Substituir Nfse

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-SubstituirNfseEnvio", new InfSchema()
            {
                Tag = "SubstituirNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Substituição de NFSe",
                TagAssinatura0 = "Pedido",
                TagAtributoId0 = "InfPedidoCancelamento",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "SubstituirNfseEnvio",
                TagLoteAtributoId = "SubstituicaoNfse",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Substituir Nfse

            #region Consulta NFSe Servico Tomado

            SchemaXML.InfSchemas.Add("NFSE-SMARAPD_204-ConsultarNfseServicoPrestadoEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseServicoPrestadoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\nfse_v2-04.xsd",
                Descricao = "XML de Consulta da NFSe Servicos Tomados",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Consulta NFSe Servico Tomado
        }
    }
}
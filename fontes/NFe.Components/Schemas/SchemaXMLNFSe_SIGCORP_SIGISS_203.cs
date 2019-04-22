﻿using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_SIGCORP_SIGISS_203
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de lote RPS

            #region XML de lote RPS Síncrono

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de lote RPS Síncrono

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de Cancelamento de NFS-e

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de Consulta de Lote RPS

            #region XML de Consulta de NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de Consulta de NFSe por Rps

            #region XML de Consulta de NFSe por Faixa

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por Faixa

            #region Gerar NFSe Envio

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-GerarNfseEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion Gerar NFSe Envio

            #region Substituir Nfse

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-SubstituirNfseEnvio", new InfSchema()
            {
                Tag = "SubstituirNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
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

            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS_203-ConsultarNfseServicoTomadoEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseServicoTomadoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\nfse_v2-03.xsd",
                Descricao = "XML de Consulta da NFSe Servicos Tomados",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Consulta NFSe Servico Tomado
        }
    }
}
﻿using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_AVMB_ASTEN
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de lote RPS

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de Lote RPS

            #region Consulta NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Consulta NFSe por Rps

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

            #region XML de Lote RPS (Síncrono)

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de Lote RPS - Síncrono",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Lote RPS (Síncrono)

            #region XML de Consulta de NFSe por Faixa

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por Faixa

            #region XML de Lote RPS (Síncrono)

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de RPS - Síncrono",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                // TagLoteAssinatura = "GerarNfseEnvio",
                // TagLoteAtributoId = "Rps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Lote RPS (Síncrono)

            #region Substituir Nfse

            SchemaXML.InfSchemas.Add("NFSE-AVMB_ASTEN-SubstituirNfseEnvio", new InfSchema()
            {
                Tag = "SubstituirNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\AVMB_ASTEN\\nfse.xsd",
                Descricao = "XML de RPS - Síncrono",
                TagAssinatura0 = "Pedido",
                TagAtributoId0 = "InfPedidoCancelamento",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "SubstituirNfseEnvio",
                TagLoteAtributoId = "SubstituicaoNfse",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Lote RPS (Síncrono)

        }
    }
}
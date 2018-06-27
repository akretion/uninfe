﻿using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_E_RECEITA
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de lote RPS

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de Lote RPS

            #region Consulta NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion Consulta NFSe por Rps

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

            #region XML de Consulta de NFSe por Faixa

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por Faixa

            #region XML de Lote RPS (Síncrono)

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion XML de Lote RPS (Síncrono)

            #region XML para Gerar NFse

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
                Descricao = "XML de Lote RPS - Sincrono",
                TagAssinatura = "GerarNfseEnvio",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML para Gerar NFse

            #region Substituir Nfse

            SchemaXML.InfSchemas.Add("NFSE-E_RECEITA-SubstituirNfseEnvio", new InfSchema()
            {
                Tag = "SubstituirNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\E_RECEITA\\nfse v2 02.xsd",
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
        }
    }
}
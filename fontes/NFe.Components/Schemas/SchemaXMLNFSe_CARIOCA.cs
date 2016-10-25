using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_CARIOCA
    {
        public static void CriarListaIDXML()
        {
            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-CARIOCA-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CARIOCA\\nfse_pcrj_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://notacarioca.rio.gov.br/WSNacional/XSD/1/nfse_pcrj_v01.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-CARIOCA-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CARIOCA\\nfse_pcrj_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://notacarioca.rio.gov.br/WSNacional/XSD/1/nfse_pcrj_v01.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-CARIOCA-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CARIOCA\\nfse_pcrj_v01.xsd",
                Descricao = "XML de Consulta de Situação do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://notacarioca.rio.gov.br/WSNacional/XSD/1/nfse_pcrj_v01.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-CARIOCA-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CARIOCA\\nfse_pcrj_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://notacarioca.rio.gov.br/WSNacional/XSD/1/nfse_pcrj_v01.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-CARIOCA-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CARIOCA\\nfse_pcrj_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://notacarioca.rio.gov.br/WSNacional/XSD/1/nfse_pcrj_v01.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-CARIOCA-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CARIOCA\\nfse_pcrj_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://notacarioca.rio.gov.br/WSNacional/XSD/1/nfse_pcrj_v01.xsd"
            });
            #endregion

            #region XML de lote RPS (Sincrono)
            SchemaXML.InfSchemas.Add("NFSE-CARIOCA-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\CARIOCA\\nfse_pcrj_v01.xsd",
                Descricao = "XML de Lote RPS - Sincrono",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TargetNameSpace = "http://notacarioca.rio.gov.br/WSNacional/XSD/1/nfse_pcrj_v01.xsd"
            });
            #endregion
        }
    }
}

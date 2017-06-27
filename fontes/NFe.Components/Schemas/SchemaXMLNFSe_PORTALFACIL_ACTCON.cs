using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_PORTALFACIL_ACTCON
    {
        public static void CriarListaIDXML()
        {
            #region 2.01
            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Consulta de Situação do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });
            #endregion

            #region XML de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "GerarNfseEnvio",
                TagLoteAtributoId = "Rps",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v201.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfevaladares.portalfacil.com.br/homologacao/schema/nfse_v201.xsd"
            });

            #endregion
            #endregion            
        }
    }
}

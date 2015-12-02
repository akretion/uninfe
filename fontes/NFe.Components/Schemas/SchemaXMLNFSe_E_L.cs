using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_E_L
    {
        public static void CriarListaIDXML()
        {
            #region E&L
            /*
            #region Consulta NFSe

            SchemaXML.InfSchemas.Add("NFSE-EGOVERNE-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://isscuritiba.curitiba.pr.gov.br/iss/nfse.xsd"
            });

            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-EGOVERNE-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://isscuritiba.curitiba.pr.gov.br/iss/nfse.xsd"
            });

            #endregion

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-EGOVERNE-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://isscuritiba.curitiba.pr.gov.br/iss/nfse.xsd"
            });

            #endregion

            #region Consulta NFSe por RPS

            SchemaXML.InfSchemas.Add("NFSE-EGOVERNE-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://isscuritiba.curitiba.pr.gov.br/iss/nfse.xsd"
            });

            #endregion

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-EGOVERNE-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://isscuritiba.curitiba.pr.gov.br/iss/nfse.xsd"
            });

            #endregion
            */

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-EL-LoteRps", new InfSchema()
            {
                Tag = "LoteRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EL\\el-nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TagLoteAssinatura = "",
                TagLoteAtributoId = "",
                TargetNameSpace = "http://www.el.com.br/nfse/xsd/el-nfse.xsd"
            });
            #endregion

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_SMARAPD
    {
        public static void CriarListaIDXML()
        {
            /*
            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-SMARAPD-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_consultar_nfse_envio.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_consultar_nfse_envio.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_consultar_nfse_rps_envio.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_consultar_nfse_rps_envio.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_consultar_lote_rps_envio.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_consultar_lote_rps_envio.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-p1:CancelarNfseEnvio", new InfSchema()
            {
                Tag = "p1:CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_cancelar_nfse_envio.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "tc:InfPedidoCancelamento",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_cancelar_nfse_envio.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_consultar_situacao_lote_rps_envio.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_consultar_situacao_lote_rps_envio.xsd"
            });
            #endregion
            */
            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SMARAPD-tbnfd", new InfSchema()
            {
                Tag = "tbnfd",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SMARAPD\\WSEntradaNfd.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "tbnfd",
                TagAtributoId = "nfd",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SMARAPD-nfd", new InfSchema()
            {
                Tag = "nfd",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "nfd",
                TagAtributoId = "inscricaomunicipalemissor",
                TargetNameSpace = ""
            });
            #endregion
            /*
            #region XML de consulta da URL para impressão da NFSe
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-ConsultarUrlVisualizacaoNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarUrlVisualizacaoNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_consultar_url_visualizacao_nfse_envio.xsd",
                Descricao = "XML de Consulta da URL de Visualização da NFSe",
                TagAssinatura = "",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_consultar_url_visualizacao_nfse_envio.xsd"
            });
            #endregion

            #region XML de consulta da URL para impressão da NFSe com Série
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-ConsultarUrlVisualizacaoNfseSerieEnvio", new InfSchema()
            {
                Tag = "ConsultarUrlVisualizacaoNfseSerieEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_consultar_url_visualizacao_nfse_serie_envio.xsd",
                Descricao = "XML de Consulta da URL de Visualização da NFSe com série",
                TagAssinatura = "",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_consultar_url_visualizacao_nfse_serie_envio.xsd"
            });
            #endregion
            */
        }
    }
}

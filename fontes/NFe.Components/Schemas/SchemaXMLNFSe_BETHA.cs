using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_BETHA
    {
        public static void CriarListaIDXML()
        {
            #region 1.0
            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-BETHA-e:ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "e:ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\servico_consultar_nfse_envio_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-BETHA-e:ConsultarNfsePorRpsEnvio", new InfSchema()
            {
                Tag = "e:ConsultarNfsePorRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\servico_consultar_nfse_rps_envio_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BETHA-e:ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "e:ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\servico_consultar_lote_rps_envio_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BETHA-e:CancelarNfseEnvio", new InfSchema()
            {
                Tag = "e:CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\servico_cancelar_nfse_envio_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BETHA-e:ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "e:ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\servico_consultar_situacao_lote_rps_envio_v01.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BETHA-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\servico_enviar_lote_rps_envio_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion
            #endregion 1.0

            #region 2.02
            #region XML de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-2.02-BETHA-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\nfse_v202.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Lote RPS (Síncrono)
            SchemaXML.InfSchemas.Add("NFSE-2.02-BETHA-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\nfse_v202.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BETHA-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\nfse_v202.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BETHA-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\nfse_v202.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-BETHA-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\nfse_v202.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-BETHA-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BETHA\\nfse_v202.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #region XML de Consulta de NFSe por Periodo
            SchemaXML.InfSchemas.Add("NFSE-BETHA-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de NFSe por Periodo",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.betha.com.br/e-nota-contribuinte-ws"
            });
            #endregion

            #endregion 2.02
        }
    }
}

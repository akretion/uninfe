using System;
using System.Collections.Generic;
using System.Text;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe
    {
        public static void CriarListaIDXML()
        {
            #region Schemas padrão ABRASF

            #region ABRASF - Recife - PE (2611606)

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-ABRASF-2611606-EnviarLoteRpsEnvio", new InfSchema() {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_recife_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://nfse.recife.pe.gov.br/wsnacional/xsd/1/nfse_recife_v01.xsd"
            });
            #endregion

            #endregion

            #endregion

            #region Schemas padrão GINFES

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-GINFES-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES\\servico_consultar_nfse_envio_v03.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "ConsultarNfseEnvio",
                TagAtributoId = "NumeroNfse",
                TargetNameSpace = "http://www.ginfes.com.br/servico_consultar_nfse_envio_v03.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-GINFES-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfsePorRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES\\servico_consultar_nfse_rps_envio_v03.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "ConsultarNfseRpsEnvio",
                TagAtributoId = "Prestador",
                TargetNameSpace = "http://www.ginfes.com.br/servico_consultar_nfse_rps_envio_v03.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GINFES-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES\\servico_consultar_lote_rps_envio_v03.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "ConsultarLoteRpsEnvio",
                TagAtributoId = "Protocolo",
                TargetNameSpace = "http://www.ginfes.com.br/servico_consultar_lote_rps_envio_v03.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-GINFES-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES\\servico_cancelar_nfse_envio_v02.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "CancelarNfseEnvio",
                TagAtributoId = "NumeroNfse",
                TargetNameSpace = "http://www.ginfes.com.br/servico_cancelar_nfse_envio"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GINFES-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES\\servico_consultar_situacao_lote_rps_envio_v03.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "ConsultarSituacaoLoteRpsEnvio",
                TagAtributoId = "Protocolo",
                TargetNameSpace = "http://www.ginfes.com.br/servico_consultar_situacao_lote_rps_envio_v03.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GINFES-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES\\servico_enviar_lote_rps_envio_v03.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.ginfes.com.br/servico_enviar_lote_rps_envio_v03.xsd"
            });
            #endregion

            #region GINFES - São José dos Pinhais - PR (4125506)

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-GINFES-4125506-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES_SJP\\servico_consultar_nfse_envio_v03.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfe.sjp.pr.gov.br/servico_consultar_nfse_envio_v03.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-GINFES-4125506-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfsePorRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES_SJP\\servico_consultar_nfse_rps_envio_v03.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfe.sjp.pr.gov.br/servico_consultar_nfse_rps_envio_v03.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GINFES-4125506-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES_SJP\\servico_consultar_lote_rps_envio_v03.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfe.sjp.pr.gov.br/servico_consultar_lote_rps_envio_v03.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-GINFES-4125506-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES_SJP\\servico_cancelar_nfse_envio_v03.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "CancelarNfseEnvio",
                TagAtributoId = "Pedido",
                TargetNameSpace = "http://nfe.sjp.pr.gov.br/servico_cancelar_nfse_envio_v03.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GINFES-4125506-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES_SJP\\servico_consultar_situacao_lote_rps_envio_v03.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfe.sjp.pr.gov.br/servico_consultar_situacao_lote_rps_envio_v03.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GINFES-4125506-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GINFES_SJP\\servico_enviar_lote_rps_envio_v03.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://nfe.sjp.pr.gov.br/servico_enviar_lote_rps_envio_v03.xsd"
            });
            #endregion

            #endregion

            #endregion


            #region Schemas padrão SIGCORP_SIGISS

            #region SIGCORP_SIGISS - Londrina - PR (4113700)

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS-4113700-tcDescricaoRps", new InfSchema() {
                Tag = "tcDescricaoRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\tcDescricaoRps.xsd",
                Descricao = "XML de Lote RPS",
                TargetNameSpace = "http://iss.londrina.pr.gov.br/ws/v1_03"
            });
            #endregion
            
            #endregion

            #endregion


            #region Schemas padrão BETHA

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

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BETHA-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
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

            #endregion

            #region Schemas padrão THEMA

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-THEMA-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-THEMA-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "CancelarNfseEnvio",
                TagAtributoId = "Pedido",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-THEMA-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-THEMA-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-THEMA-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-THEMA-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas padrão ABACO

            #region CANOAS_RS

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-CANOAS_RS-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABACO\\nfse_v2010.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-CANOAS_RS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABACO\\nfse_v2010.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-CANOAS_RS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABACO\\nfse_v2010.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-CANOAS_RS-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABACO\\nfse_v2010.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-CANOAS_RS-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABACO\\nfse_v2010.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-CANOAS_RS-ConsultarNfsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABACO\\nfse_v2010.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #endregion

            #endregion

            #region Schemas padrão ISSNet

            #region NOVOHAMBURGO_RS/SANTAMARIA_RS

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-ConsultarNfseEnvio", new InfSchema()
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

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-ISSNET-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSNET\\servico_enviar_lote_rps_envio.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "tc:Rps",
                TagAtributoId = "tc:InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.issnetonline.com.br/webserviceabrasf/vsd/servico_enviar_lote_rps_envio.xsd"
            });
            #endregion

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

            #endregion

            #endregion

            #region Schemas padrão ISSOnLine

            #region APUCARANA_PR/ARACATUBA_SP/PENAPOLIS_SP

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-ISSONLINE-NFSE", new InfSchema()
            {
                Tag = "NFSE",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de NFSe padrão ISSOnLine",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #endregion

            #endregion

            #region Schemas BLUMENAU_SC

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoConsultaNFePeriodo", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFePeriodo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoConsultaNFePeriodo_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "p1:PedidoConsultaNFePeriodo",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoConsultaNFe", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoConsultaNFe_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "p1:PedidoConsultaNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoConsultaLote", new InfSchema()
            {
                Tag = "p1:PedidoConsultaLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoConsultaLote_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "p1:PedidoConsultaLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-PedidoCancelamentoNFe", new InfSchema()
            {
                Tag = "PedidoCancelamentoNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoCancelamentoNFe_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "PedidoCancelamentoNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoInformacoesLote", new InfSchema()
            {
                Tag = "p1:PedidoInformacoesLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoInformacoesLote_v01.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "p1:PedidoInformacoesLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-PedidoEnvioLoteRPS", new InfSchema()
            {
                Tag = "PedidoEnvioLoteRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoEnvioLoteRPS_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "PedidoEnvioLoteRPS",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #endregion

            #region Schemas BHISS

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BHISS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS (Assincrono)
            SchemaXML.InfSchemas.Add("NFSE-BHISS-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Lote RPS (Assincrono)",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS (Sincrono)
            SchemaXML.InfSchemas.Add("NFSE-BHISS-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "GerarNfseEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas GIF

            #region Schemas GIF - Específico município de Parobé

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-NFSE", new InfSchema()
            {
                Tag = "NFSE",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\nfse-parobe-v1-1.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "pedidoLoteNFSe",
                TagAtributoId = "pedidoLoteNFSe",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-pedidoLoteNFSe", new InfSchema()
            {
                Tag = "pedidoLoteNFSe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\nfse-parobe-v1-1.xsd",
                Descricao = "XML de NFSe padrão GIF",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-pedConsultaTrans", new InfSchema()
            {
                Tag = "pedConsultaTrans",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\nfse-parobe-v1-1.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "pedConsultaTrans",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-pedidoNFSe", new InfSchema()
            {
                Tag = "pedidoNFSe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\nfse-parobe-v1-1.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "pedidoNFSe",
                TagAtributoId = "pedidoNFSe",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-pedidoCancelamentoLote", new InfSchema()
            {
                Tag = "pedidoCancelamentoLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\nfse-parobe-v1-1.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "pedidoCancelamentoLote",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-pedidoStatusLote", new InfSchema()
            {
                Tag = "pedidoStatusLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\nfse-parobe-v1-1.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "pedidoStatusLote",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-envioLote", new InfSchema()
            {
                Tag = "envioLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "envioLote",
                TagAtributoId = "NFS-e",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de consulta URL NFSe
            SchemaXML.InfSchemas.Add("NFSE-GIF-4314050-pedidoNFSePNG", new InfSchema()
            {
                Tag = "pedidoNFSePNG",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\nfse-parobe-v1-1.xsd",
                Descricao = "XML de consulta URL NFe padrão GIF",
                TagAssinatura = "pedidoLoteNFSePNG",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #endregion

            #region Schemas GIF - Demais municípios

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-GIF-NFSE", new InfSchema()
            {
                Tag = "NFSE",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "pedidoLoteNFSe",
                TagAtributoId = "pedidoLoteNFSe",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-GIF-pedidoLoteNFSe", new InfSchema()
            {
                Tag = "pedidoLoteNFSe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de NFSe padrão GIF",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-GIF-pedConsultaTrans", new InfSchema()
            {
                Tag = "pedConsultaTrans",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "pedConsultaTrans",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GIF-pedidoNFSe", new InfSchema()
            {
                Tag = "pedidoNFSe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "pedidoNFSe",
                TagAtributoId = "pedidoNFSe",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-GIF-pedAnulaNFSe", new InfSchema()
            {
                Tag = "pedAnulaNFSe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "pedAnulaNFSe",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GIF-pedidoStatusLote", new InfSchema()
            {
                Tag = "pedidoStatusLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "pedidoStatusLote",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GIF-envioLote", new InfSchema()
            {
                Tag = "envioLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "envioLote",
                TagAtributoId = "NFS-e",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de consulta NFSe em PNG
            SchemaXML.InfSchemas.Add("NFSE-GIF-pedidoNFSePNG", new InfSchema()
            {
                Tag = "pedidoNFSePNG",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de consulta da NFSe em PNG padrão GIF",
                TagAssinatura = "pedidoNFSePNG",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de Inutilização da NFSe
            SchemaXML.InfSchemas.Add("NFSE-GIF-solicitacaoInutilizacao", new InfSchema()
            {
                Tag = "solicitacaoInutilizacao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de Inutilização da NFSe com padrão GIF/Infisc.",
                TagAssinatura = "solicitacaoInutilizacao",
                TagAtributoId = "numerosInutilizados",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #region XML de consulta NFSe em PNG
            SchemaXML.InfSchemas.Add("NFSE-GIF-pedidoNFSePDF", new InfSchema()
            {
                Tag = "pedidoNFSePDF",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GIF\\NFSe-Infisc-v1.xsd",
                Descricao = "XML de consulta da NFSe em PDF padrão GIF",
                TagAssinatura = "pedidoNFSePDF",
                TagAtributoId = "CNPJ",
                TargetNameSpace = "http://ws.pc.gif.com.br/"
            });
            #endregion

            #endregion

            #endregion

            #region Schemas padrão DUETO
            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DUETO-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DUETO\\servico_enviar_lote_rps_envio.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://tempuri.org/servico_enviar_lote_rps_envio.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-DUETO-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DUETO\\servico_cancelar_nfse_envio.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_enviar_lote_rps_envio.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DUETO-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DUETO\\servico_consultar_lote_rps_envio.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_lote_rps_envio.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DUETO-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DUETO\\servico_consultar_situacao_lote_rps_envio.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_situacao_lote_rps_envio.xsd"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-DUETO-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DUETO\\servico_consultar_nfse_rps_envio.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_nfse_rps_envio.xsd"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-DUETO-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DUETO\\servico_consultar_nfse_envio.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_nfse_envio.xsd"
            });
            #endregion
            #endregion

            #region Schemas padrão WebISS

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-WEBISS-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\WEBISS\\servico_enviar_lote_rps_envio.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-WEBISS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\WEBISS\\servico_cancelar_nfse_envio.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse"

            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-WEBISS-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\WEBISS\\servico_consultar_lote_rps_envio.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-WEBISS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\WEBISS\\servico_consultar_situacao_lote_rps_envio.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-WEBISS-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\WEBISS\\servico_consultar_nfse_rps_envio.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-WEBISS-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\WEBISS\\servico_consultar_nfse_envio.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse"
            });
            #endregion

            #endregion

            #region Schemas padrão Paulistana

            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-PedidoEnvioLoteRPS", new InfSchema()
            {
                Tag = "PedidoEnvioLoteRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoEnvioLoteRPS_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "PedidoEnvioLoteRPS",
                TagLoteAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });

            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoCancelamentoNFe", new InfSchema()
            {
                Tag = "p1:PedidoCancelamentoNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoCancelamentoNFe_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "p1:PedidoCancelamentoNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"

            });

            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-PedidoCancelamentoNFe", new InfSchema()
            {
                Tag = "PedidoCancelamentoNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoCancelamentoNFe_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "PedidoCancelamentoNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"

            });

            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoConsultaLote", new InfSchema()
            {
                Tag = "p1:PedidoConsultaLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoConsultaLote_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "p1:PedidoConsultaLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoInformacoesLote", new InfSchema()
            {
                Tag = "p1:PedidoInformacoesLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoInformacoesLote_v01.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "p1:PedidoInformacoesLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoConsultaNFePeriodo", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFePeriodo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoConsultaNFePeriodo_v01.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "p1:PedidoConsultaNFePeriodo",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoConsultaNFe", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoConsultaNFe_v01.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "p1:PedidoConsultaNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });

            #endregion

            #endregion

            #region Schemas SALVADOR_BA

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-SALVADOR_BA-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SALVADOR_BA\\nfse_salvador.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-SALVADOR_BA-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SALVADOR_BA\\nfse_salvador.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SALVADOR_BA-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SALVADOR_BA\\nfse_salvador.xsd",
                Descricao = "XML de Consulta de Situação do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-SALVADOR_BA-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SALVADOR_BA\\nfse_salvador.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SALVADOR_BA-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SALVADOR_BA\\nfse_salvador.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SALVADOR_BA-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SALVADOR_BA\\nfse_salvador.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas padrão PORTOVELHENSE

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTOVELHENSE-p:ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "p:ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PORTOVELHENSE\\nfse_v2.0.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-PORTOVELHENSE-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PORTOVELHENSE\\nfse_v2.0.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTOVELHENSE-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PORTOVELHENSE\\nfse_v2.0.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTOVELHENSE-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PORTOVELHENSE\\nfse_v2.0.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-PORTOVELHENSE-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PORTOVELHENSE\\nfse_v2.0.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por Faixa
            SchemaXML.InfSchemas.Add("NFSE-PORTOVELHENSE-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PORTOVELHENSE\\nfse_v2.0.xsd",
                Descricao = "XML de Consulta da NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas PRONIN

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-PRONIN-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-PRONIN-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PRONIN-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-PRONIN-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PRONIN-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PRONIN-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas ISSONLINE4R (4R Sistemas)

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-ISSONLINE4R-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento de NFSe",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-ISSONLINE4R-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-ISSONLINE4R-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TagAssinatura = "Rps",
                TagAtributoId = "Rps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas DSF

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqCancelamentoNFSe", new InfSchema()
            {
                Tag = "ns1:ReqCancelamentoNFSe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqCancelamentoNFSe.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "ns1:ReqCancelamentoNFSe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqConsultaNFSeRPS", new InfSchema()
            {
                Tag = "ns1:ReqConsultaNFSeRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqConsultaNFSeRPS.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqConsultaLote", new InfSchema()
            {
                Tag = "ns1:ReqConsultaLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqConsultaLote.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqConsultaNotas", new InfSchema()
            {
                Tag = "ns1:ReqConsultaNotas",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqConsultaNotas.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "ns1:ReqConsultaNotas",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ConsultaSeqRps", new InfSchema()
            {
                Tag = "ns1:ConsultaSeqRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ConsultaSeqRps.xsd",
                Descricao = "XML de Consulta da Situação do Lote RPS (Retorna número do Ultimo RPS)",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqEnvioLoteRPS", new InfSchema()
            {
                Tag = "ns1:ReqEnvioLoteRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqEnvioLoteRPS.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "ns1:ReqEnvioLoteRPS",
                TagAtributoId = "Lote",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #endregion

            #region TECNOSISTEMAS

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
            #endregion

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
            #endregion

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
            #endregion

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
            #endregion

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
            #endregion

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
            #endregion

            #endregion

            #region Schemas padrão SYSTEMPRO

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-SYSTEMPRO-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SYSTEMPRO-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-SYSTEMPRO-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas TIPLAN
            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-TIPLAN-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\TIPLAN\\nfse_municipal_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.nfe.com.br/WSNacional/XSD/1/nfse_municipal_v01.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-TIPLAN-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\TIPLAN\\nfse_municipal_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.nfe.com.br/WSNacional/XSD/1/nfse_municipal_v01.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-TIPLAN-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\TIPLAN\\nfse_municipal_v01.xsd",
                Descricao = "XML de Consulta de Situação do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.nfe.com.br/WSNacional/XSD/1/nfse_municipal_v01.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-TIPLAN-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\TIPLAN\\nfse_municipal_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.nfe.com.br/WSNacional/XSD/1/nfse_municipal_v01.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-TIPLAN-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\TIPLAN\\nfse_municipal_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.nfe.com.br/WSNacional/XSD/1/nfse_municipal_v01.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-TIPLAN-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\TIPLAN\\nfse_municipal_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.nfe.com.br/WSNacional/XSD/1/nfse_municipal_v01.xsd"
            });
            #endregion

            #endregion

            #region Schemas CARIOCA
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

            #endregion

            #region SMARAPD
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
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "tbnfd",
                TagAtributoId = "nfd",
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
            #endregion

            #region FIORILLI

            #region Consulta NFSe
            /*
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
            */
            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-FIORILLI-cancelarNfse", new InfSchema()
            {
                Tag = "cancelarNfse",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            
            #endregion

            #region XML de Consulta de Lote RPS
            /*
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
            */
            #endregion

            #region Consulta NFSe por Faixa
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region XML de Consulta Situação do Lote RPS
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FIORILLI-gerarNfse", new InfSchema()
            {
                Tag = "gerarNfse",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagLoteAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "Rps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #endregion

            #region FINTEL
            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                //TagLoteAssinatura = "GerarNfseEnvio",
                //TagLoteAtributoId = "Rps",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Faixa
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            
            #endregion

            #region XML de Consulta de NFSe por RPS
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion                       
            #endregion

            #region ISSWEB

            #region Consulta NFSe
            SchemaXML.InfSchemas.Add("NFSE-ISSWEB-ISSEConsultaNota", new InfSchema()
            {
                Tag = "ISSEConsultaNota",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSWEB\\XSDISSEConsultaNota.xsd",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://186.201.198.98/issweb/webservices/XSDISSEConsultaNota.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-ISSWEB-ISSECancelaNFe", new InfSchema()
            {
                Tag = "ISSECancelaNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSWEB\\XSDISSECancelaNFe.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://186.201.198.98/issweb/webservices/XSDISSECancelaNFe.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-ISSWEB-NFEEletronica", new InfSchema()
            {
                Tag = "NFEEletronica",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSWEB\\XSDNFEletronica.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAtributoId = "",
                TagLoteAssinatura = "",
                TargetNameSpace = "http://186.201.198.98/issweb/webservices/XSDNFEletronica.xsd"
            });
            #endregion

            #endregion

            #region SIMPLISS

            #region Consulta NFSe
            /*
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
            */
            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-SIMPLISS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.sistema.com.br/Nfse/arquivos/nfse_3.xsd"
            });

            #endregion

            #region XML de Consulta de Lote RPS
            /*
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
            */
            #endregion

            #region Consulta NFSe por Faixa
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region XML de Consulta Situação do Lote RPS
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SIMPLISS-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagLoteAtributoId = "InfRps",
                TagLoteAssinatura = "Rps",
                TargetNameSpace = "http://www.sistema.com.br/Nfse/arquivos/nfse_3.xsd"
            });
            #endregion

            #endregion

            #region E-GOVERNE

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

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-EGOVERNE-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EGOVERNE\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://isscuritiba.curitiba.pr.gov.br/iss/nfse.xsd"
            });
            #endregion

            #endregion

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

            #region GOVDIGITAL

            #region Consulta NFSe

            SchemaXML.InfSchemas.Add("NFSE-GOVDIGITAL-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOVDIGITAL\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-GOVDIGITAL-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOVDIGITAL\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-GOVDIGITAL-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOVDIGITAL\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion

            #region Consulta NFSe por RPS

            SchemaXML.InfSchemas.Add("NFSE-GOVDIGITAL-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOVDIGITAL\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-GOVDIGITAL-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOVDIGITAL\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GOVDIGITAL-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOVDIGITAL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                //TagLoteAssinatura = "EnviarLoteRpsEnvio",
                //TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #endregion

            #region EQUIPLANO

            #region Consulta NFSe

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarNfseEnvio", new InfSchema()
            {
                Tag = "es:esConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarNfseEnvio_v01.xsd",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "es:esConsultarNfseEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esCancelarNfseEnvio", new InfSchema()
            {
                Tag = "es:esCancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esCancelarNfseEnvio_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "es:esCancelarNfseEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "es:esConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarLoteRpsEnvio_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "es:esConsultarLoteRpsEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region Consulta NFSe por RPS

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarNfsePorRpsEnvio", new InfSchema()
            {
                Tag = "es:esConsultarNfsePorRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarNfsePorRpsEnvio_v01.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "es:esConsultarNfsePorRpsEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "es:esConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarSituacaoLoteRpsEnvio_v01.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "es:esConsultarSituacaoLoteRpsEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:enviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "es:enviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esRecepcionarLoteRpsEnvio_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "es:enviarLoteRpsEnvio",
                TagAtributoId = "lote",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });
            #endregion

            #endregion

            #region PRODATA

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-PRODATA-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PRODATA-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-PRODATA-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PRODATA-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion
            #endregion
            
            #region VVISS
            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-VVISS-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                //TagLoteAssinatura = "GerarNfseEnvio",
                //TagLoteAtributoId = "Rps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-VVISS-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-VVISS-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-VVISS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-VVISS-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Faixa
            SchemaXML.InfSchemas.Add("NFSE-VVISS-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion

            #region XML de Consulta de NFSe por RPS
            SchemaXML.InfSchemas.Add("NFSE-VVISS-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse_v2_01.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion
            #endregion

            #region Schemas padrão FISSLEX

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FISSLEX-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-FISSLEX-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "CancelarNfseEnvio",
                TagAtributoId = "Pedido",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FISSLEX-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-FISSLEX-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-FISSLEX-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FISSLEX-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #endregion

            #region Schemas padrão NATALENSE

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-NATALENSE-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\NATALENSE\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-NATALENSE-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\NATALENSE\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "CancelarNfseEnvio",
                TagAtributoId = "Pedido",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-NATALENSE-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\NATALENSE\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-NATALENSE-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\NATALENSE\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-NATALENSE-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\NATALENSE\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-NATALENSE-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\NATALENSE\\nfse.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #endregion
        }
    }
}

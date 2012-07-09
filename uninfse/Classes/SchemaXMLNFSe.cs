using System;
using System.Collections.Generic;
using System.Text;
using NFe.Components;

namespace uninfse
{
    class SchemaXMLNFSe
    {
        public static void CriarListaIDXML()
        {
            SchemaXML.InfSchemas.Clear();

            #region Schemas padrão GINFES

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GINFES-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = 1,
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
                ID = 2,
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
                ID = 3,
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
                ID = 4,
                ArquivoXSD = "NFSe\\GINFES\\servico_enviar_lote_rps_envio_v03.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.ginfes.com.br/servico_enviar_lote_rps_envio_v03.xsd"
            });
            #endregion

            #endregion

            #region Schemas padrão BETHA

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BETHA-e:ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "e:ConsultarLoteRpsEnvio",
                ID = 5,
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
                ID = 6,
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
                ID = 7,
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
                ID = 8,
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
                ID = 9,
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
                ID = 10,
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
                ID = 11,
                ArquivoXSD = "NFSe\\ABRASF\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-THEMA-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = 12,
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
                ID = 13,
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
                ID = 14,
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
                ID = 15,
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
                ID = 16,
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
                ID = 17,
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
                ID = 18,
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
                ID = 19,
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
                ID = 20,
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
                ID = 21,
                ArquivoXSD = "NFSe\\ABACO\\nfse_v2010.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion

            #endregion

            #endregion

            #region Determinar a propriedade MaxID
            SchemaXML.MaxID = 0;
            foreach (InfSchema item in SchemaXML.InfSchemas.Values)
            {
                if (item.ID > SchemaXML.MaxID)
                    SchemaXML.MaxID = item.ID;
            }
            #endregion
        }
    }
}

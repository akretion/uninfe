using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NFe.Components
{
    public class SchemaXML_NFe
    {
        public static void CriarListaIDXML()
        {
            #region NFe

            #region NFe versão 2.0

            #region XML Distribuição Cancelamento
            SchemaXML.InfSchemas.Add("NFE-procCancNFe", new InfSchema()
            {
                Tag = "procCancNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\procCancNFe_v{0}.xsd",
                Descricao = "XML de distribuição do Cancelamento da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Cadastro Contribuinte
            SchemaXML.InfSchemas.Add("NFE-ConsCad", new InfSchema()
            {
                Tag = "ConsCad",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consCad_v{0}.xsd",
                Descricao = "XML de Consulta do Cadastro do Contribuinte",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Recibo Lote
            SchemaXML.InfSchemas.Add("NFE-consReciNFe", new InfSchema()
            {
                Tag = "consReciNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consReciNfe_v{0}.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Situação NFe
            SchemaXML.InfSchemas.Add("NFE-consSitNFe", new InfSchema()
            {
                Tag = "consSitNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consSitNFe_v{0}.xsd",
                Descricao = "XML de Consulta da Situação da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Consulta Status Serviço NFe
            SchemaXML.InfSchemas.Add("NFE-consStatServ", new InfSchema()
            {
                Tag = "consStatServ",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consStatServ_v{0}.xsd",
                Descricao = "XML de Consulta da Situação do Serviço da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio Lote
            SchemaXML.InfSchemas.Add("NFE-enviNFe", new InfSchema()
            {
                Tag = "enviNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\enviNFe_v{0}.xsd",
                Descricao = "XML de Envio de Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Inutilização
            SchemaXML.InfSchemas.Add("NFE-inutNFe", new InfSchema()
            {
                Tag = "inutNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\inutNFe_v{0}.xsd",
                Descricao = "XML de Inutilização de Numerações de Notas Fiscais Eletrônicas",
                TagAssinatura = "inutNFe",
                TagAtributoId = "infInut",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML NFe
            SchemaXML.InfSchemas.Add("NFE-NFe", new InfSchema()
            {
                Tag = "NFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\nfe_v{0}.xsd",
                Descricao = "XML da Nota Fiscal Eletrônica",
                TagAssinatura = "NFe",
                TagAtributoId = "infNFe",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição Inutilização
            SchemaXML.InfSchemas.Add("NFE-procInutNFe", new InfSchema()
            {
                Tag = "procInutNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "NFe\\procInutNFe_v2.00.xsd",
                ArquivoXSD = "NFe\\procInutNFe_v{0}.xsd",
                Descricao = "XML de distribuição de Inutilização de Números de NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Distribuição NFe
            SchemaXML.InfSchemas.Add("NFE-nfeProc", new InfSchema()
            {
                Tag = "nfeProc",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\procNFe_v{0}.xsd",
                Descricao = "XML de distribuição da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });
            #endregion

            #endregion

            #region NFe versão 3.1

            #region XML Consulta Recibo Lote
            /*SchemaXML.InfSchemas.Add("NFE-3.10-consReciNFe", new InfSchema()
            {
                Tag = "consReciNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consReciNfe_v3.10.xsd",
                Descricao = "XML de Consulta do Recibo do Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Consulta Situação NFe
            /*SchemaXML.InfSchemas.Add("NFE-3.10-consSitNFe", new InfSchema()
            {
                Tag = "consSitNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consSitNFe_v3.10.xsd",
                Descricao = "XML de Consulta da Situação da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Consulta Status Serviço NFe
            /*SchemaXML.InfSchemas.Add("NFE-3.10-consStatServ", new InfSchema()
            {
                Tag = "consStatServ",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consStatServ_v3.10.xsd",
                Descricao = "XML de Consulta da Situação do Serviço da Nota Fiscal Eletrônica",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Envio Lote
            /*SchemaXML.InfSchemas.Add("NFE-3.10-enviNFe", new InfSchema()
            {
                Tag = "enviNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\enviNFe_v3.10.xsd",
                Descricao = "XML de Lote de Notas Fiscais Eletrônicas",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Inutilização
            /*SchemaXML.InfSchemas.Add("NFE-3.10-inutNFe", new InfSchema()
            {
                Tag = "inutNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\inutNFe_v3.10.xsd",
                Descricao = "XML de Inutilização de Numerações de Notas Fiscais Eletrônicas",
                TagAssinatura = "inutNFe",
                TagAtributoId = "infInut",
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML NFe
            /*SchemaXML.InfSchemas.Add("NFE-3.10-NFe", new InfSchema()
            {
                Tag = "NFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\nfe_v3.10.xsd",
                Descricao = "XML da Nota Fiscal Eletrônica",
                TagAssinatura = "NFe",
                TagAtributoId = "infNFe",
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Distribuição Inutilização
            /*SchemaXML.InfSchemas.Add("NFE-3.10-procInutNFe", new InfSchema()
            {
                Tag = "procInutNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\procInutNFe_v3.10.xsd",
                Descricao = "XML de distribuição de Inutilização de Números de NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Distribuição NFe
            /*SchemaXML.InfSchemas.Add("NFE-3.10-nfeProc", new InfSchema()
            {
                Tag = "nfeProc",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\procNFe_v3.10.xsd",
                Descricao = "XML de distribuição da NFe",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #region XML Consulta Cadastro Contribuinte
            /*SchemaXML.InfSchemas.Add("NFE-3.10-ConsCad", new InfSchema()
            {
                Tag = "ConsCad",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\consCad_v3.10.xsd",
                Descricao = "XML de Consulta do Cadastro do Contribuinte",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = string.Empty
            });*/
            #endregion

            #endregion

            #region XML Gerais da NFe

            #region XML Recepção EPEC
            SchemaXML.InfSchemas.Add("NFE-envEvento110140", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EPEC\\envEPEC_v1.00.xsd",
                //ArquivoXSD = "EPEC\\eventoEPEC_v0.01.xsd",
                Descricao = "XML de registro do EPEC (Sistema de Contingência Eletrônica) NF-e",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Recepção EPEC
            SchemaXML.InfSchemas.Add("NFE-envEvento-65-110140", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EPEC\\eventoEPEC_v0.01.xsd",
                Descricao = "XML de registro do EPEC (Sistema de Contingência Eletrônica) NFC-e",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio CCe
            SchemaXML.InfSchemas.Add("NFE-envEvento110110", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "CCe\\envCCe_v1.00.xsd",
                Descricao = "XML de registro de envio da CCe da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de Eventos de cancelamento
            SchemaXML.InfSchemas.Add("NFE-envEvento110111", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EventoCanc\\envEventoCancNFe_v1.00.xsd",
                Descricao = "XML de evento de cancelamento da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210200
            SchemaXML.InfSchemas.Add("NFE-envEvento210200", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EventoManifestaDestinat\\e210200_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210210
            SchemaXML.InfSchemas.Add("NFE-envEvento210210", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EventoManifestaDestinat\\e210210_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210220
            SchemaXML.InfSchemas.Add("NFE-envEvento210220", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EventoManifestaDestinat\\e210220_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de manifestacoes-e210240
            SchemaXML.InfSchemas.Add("NFE-envEvento210240", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EventoManifestaDestinat\\e210240_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento pedido de prorrogação 1º. prazo - 111500
            SchemaXML.InfSchemas.Add("NFE-envEvento111500", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "", //"NFe\\envPProrrogNFe_v1.0.xsd",
                Descricao = "Evento pedido de prorrogação 1º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento pedido de prorrogação 2º. prazo - 111501
            SchemaXML.InfSchemas.Add("NFE-envEvento111501", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "", //"NFe\\envPProrrogNFe_v1.0.xsd",
                Descricao = "Evento pedido de prorrogação 2º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Cancelamento de Pedido de Prorrogação 1º. Prazo - 111502
            SchemaXML.InfSchemas.Add("NFE-envEvento111502", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "", //"NFe\\envCancelPProrrogNFe_v1.0.xsd",
                Descricao = "Evento Cancelamento de pedido de prorrogação 1º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Cancelamento de Pedido de Prorrogação 2º. Prazo - 111503
            SchemaXML.InfSchemas.Add("NFE-envEvento111503", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "", //"NFe\\envCancelPProrrogNFe_v1.0.xsd",
                Descricao = "Evento Cancelamento de pedido de prorrogação 2º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            /*
            #region Evento Fisco Resposta ao Pedido de Prorrogação 1º prazo - 411500
            SchemaXML.InfSchemas.Add("NFE-envEvento411500", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Pedido de Prorrogação 1º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Fisco Resposta ao Pedido de Prorrogação 2º prazo - 411501
            SchemaXML.InfSchemas.Add("NFE-envEvento411501", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Pedido de Prorrogação 2º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Fisco Resposta ao Pedido de Prorrogação 1º prazo - 411502
            SchemaXML.InfSchemas.Add("NFE-envEvento411502", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Cancelamento de Prorrogação 1º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Evento Fisco Resposta ao Pedido de Prorrogação 2º prazo - 411503
            SchemaXML.InfSchemas.Add("NFE-envEvento411503", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envFiscoNfe_v1.0.xsd",
                Descricao = "Evento Fisco Resposta ao Cancelamento de Prorrogação 2º prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion
            */

            #region XML Envio de consulta de nfe
            SchemaXML.InfSchemas.Add("NFE-consNFeDest", new InfSchema()
            {
                Tag = "nfeConsultaNFDest",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "ConsultaNFDest\\consNFeDest_v1.01.xsd",
                Descricao = "XML de consulta de NFe do destinatário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de download de nfe
            SchemaXML.InfSchemas.Add("NFE-downloadNFe", new InfSchema()
            {
                Tag = "downloadNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "DownloadNFe\\downloadNFe_v1.00.xsd",
                Descricao = "XML de download de nfe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de confirmacao de recebimento de manifestacoes
            SchemaXML.InfSchemas.Add("NFE-envConfRecebto", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EventoManifestaDestinat\\envConfRecebto_v1.00.xsd",
                Descricao = "XML de evento de manifestação do destinatário da NFe",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region XML Envio de registro de saida
#if nao
            SchemaXML.InfSchemas.Add("NFE-envRegistroSaida", new InfSchema()
            {
                Tag = "envRegistro",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "envRegistro_v1.00.xsd",
                Descricao = "XML de registro de envio de registro de saida",
                TagAssinatura = "evento",
                TagAtributoId = "infRegistro",
                TargetNameSpace = string.Empty
            });
#endif
            #endregion

            #region XML Envio de cancelamento registro de saida
#if nao
            SchemaXML.InfSchemas.Add("NFE-envCancRegistroSaida", new InfSchema()
            {
                Tag = "envCancRegistro",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "envCancRegistro_v1.00.xsd",
                Descricao = "XML de registro de envio de cancelamento de registro de saida",
                TagAssinatura = "evento",
                TagAtributoId = "infCancRegistro",
                TargetNameSpace = string.Empty
            });
#endif
            #endregion

            #endregion

            #endregion
        }
    }
}

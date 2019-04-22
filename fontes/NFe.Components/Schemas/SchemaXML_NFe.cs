namespace NFe.Components
{
    public class SchemaXML_NFe
    {
        public static void CriarListaIDXML()
        {
            #region NFe

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

            #endregion XML Distribuição Cancelamento

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

            #endregion XML Consulta Cadastro Contribuinte

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

            #endregion XML Consulta Recibo Lote

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

            #endregion XML Consulta Situação NFe

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

            #endregion XML Consulta Status Serviço NFe

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

            #endregion XML Envio Lote

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

            #endregion XML Inutilização

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

            #endregion XML NFe

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

            #endregion XML Distribuição Inutilização

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

            #endregion XML Distribuição NFe

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

            #endregion XML Recepção EPEC

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

            #endregion XML Recepção EPEC

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

            #endregion XML Envio CCe

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

            #endregion XML Envio de Eventos de cancelamento

            #region XML Envio de Eventos de cancelamento por substituição

            SchemaXML.InfSchemas.Add("NFE-envEvento110112", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EventoCanc\\envEventoCancSubst_v1.00.xsd",
                Descricao = "XML de evento de cancelamento da NFe por substituição",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });

            #endregion XML Envio de Eventos de cancelamento por substituição

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

            #endregion XML Envio de manifestacoes-e210200

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

            #endregion XML Envio de manifestacoes-e210210

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

            #endregion XML Envio de manifestacoes-e210220

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

            #endregion XML Envio de manifestacoes-e210240

            #region Eventos pedido de prorrogação e cancelamento da prorrogação

            #region Evento pedido de prorrogação 1º. prazo - 111500

            SchemaXML.InfSchemas.Add("NFE-envEvento111500", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envRemIndus_v1.00.xsd",
                Descricao = "Evento pedido de prorrogação 1º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });

            #endregion Evento pedido de prorrogação 1º. prazo - 111500

            #region Evento pedido de prorrogação 2º. prazo - 111501

            SchemaXML.InfSchemas.Add("NFE-envEvento111501", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envRemIndus_v1.00.xsd",
                Descricao = "Evento pedido de prorrogação 2º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });

            #endregion Evento pedido de prorrogação 2º. prazo - 111501

            #region Evento Cancelamento de Pedido de Prorrogação 1º. Prazo - 111502

            SchemaXML.InfSchemas.Add("NFE-envEvento111502", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envRemIndus_v1.00.xsd",
                Descricao = "Evento Cancelamento de pedido de prorrogação 1º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });

            #endregion Evento Cancelamento de Pedido de Prorrogação 1º. Prazo - 111502

            #region Evento Cancelamento de Pedido de Prorrogação 2º. Prazo - 111503

            SchemaXML.InfSchemas.Add("NFE-envEvento111503", new InfSchema()
            {
                Tag = "envEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFe\\envRemIndus_v1.00.xsd",
                Descricao = "Evento Cancelamento de pedido de prorrogação 2º. prazo",
                TagAssinatura = "evento",
                TagAtributoId = "infEvento",
                TargetNameSpace = string.Empty
            });

            #endregion Evento Cancelamento de Pedido de Prorrogação 2º. Prazo - 111503

            #endregion Eventos pedido de prorrogação e cancelamento da prorrogação

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

            #endregion XML Envio de consulta de nfe
                        
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

            #endregion XML Envio de confirmacao de recebimento de manifestacoes

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

            #endregion XML Envio de registro de saida

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

            #endregion XML Envio de cancelamento registro de saida

            #endregion XML Gerais da NFe

            #endregion NFe
        }
    }
}
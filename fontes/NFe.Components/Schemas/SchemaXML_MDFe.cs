namespace NFe.Components
{
    public class SchemaXML_MDFe
    {
        public static void CriarListaIDXML()
        {
            #region MDF-e

            #region XML Envio do MDFe

            SchemaXML.InfSchemas.Add("NFE-MDFe", new InfSchema()
            {
                Tag = "MDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\mdfe_v{0}.xsd",
                Descricao = "XML do Manifesto Eletrônico de Documentos Fiscais",
                TagAssinatura = "MDFe",
                TagAtributoId = "infMDFe",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML Envio do MDFe

            #region XML Envio Lote MDFe

            SchemaXML.InfSchemas.Add("NFE-enviMDFe", new InfSchema()
            {
                Tag = "enviMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\enviMDFe_v{0}.xsd",
                Descricao = "XML de Envio de Lote de Manifesto Eletrônico de Docimentos Fiscais",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML Envio Lote MDFe

            #region XML de consulta recibo do MDFe

            SchemaXML.InfSchemas.Add("NFE-consReciMDFe", new InfSchema()
            {
                Tag = "consReciMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consReciMDFe_v{0}.xsd",
                Descricao = "XML de consulta recibo MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de consulta recibo do MDFe

            #region XML de consulta situação do MDFe

            SchemaXML.InfSchemas.Add("NFE-consSitMDFe", new InfSchema()
            {
                Tag = "consSitMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consSitMDFe_v{0}.xsd",
                Descricao = "XML de consulta situação do MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de consulta situação do MDFe

            #region XML de consulta status dos serviços do MDFe

            SchemaXML.InfSchemas.Add("NFE-consStatServMDFe", new InfSchema()
            {
                Tag = "consStatServMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consStatServMDFe_v{0}.xsd",
                Descricao = "XML de consulta status do serviço do MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de consulta status dos serviços do MDFe

            #region XML de Evento de Cancelamento do MDFe

            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110111", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v{0}.xsd",
                Descricao = "XML de evento de cancelamento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de Evento de Cancelamento do MDFe

            #region XML de Evento de Encerramento do MDFe

            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110112", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v{0}.xsd",
                Descricao = "XML de evento de encerramento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de Evento de Encerramento do MDFe

            #region XML de Evento de Inclusão de Condutor

            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110114", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v{0}.xsd",
                Descricao = "XML de evento de encerramento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de Evento de Inclusão de Condutor

            #region XML de Evento de Pagamento Operação MDF-e

            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110116", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v{0}.xsd",
                Descricao = "XML de evento de Pagamento Operação MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de Evento de Pagamento Operação MDF-e

            #region XML de Evento de Inclusão de Condutor

            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110115", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v{0}.xsd",
                Descricao = "XML de evento de encerramento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de Evento de Inclusão de Condutor

            #region XML de Eventos Gerais do MDFe

            SchemaXML.InfSchemas.Add("NFE-eventoMDFe310620", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v{0}.xsd",
                Descricao = "XML de evento de registro de passagem do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de Eventos Gerais do MDFe

            #region XML de Eventos Gerais do MDFe

            SchemaXML.InfSchemas.Add("NFE-consMDFeNaoEnc", new InfSchema()
            {
                Tag = "consMDFeNaoEnc",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consMDFeNaoEnc_v{0}.xsd",
                Descricao = "Pedido de Consulta MDF-e não encerrados",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });

            #endregion XML de Eventos Gerais do MDFe

            #region Schemas Modal

            #region Rodoviário
            SchemaXML.InfSchemas.Add("NFE-rodo-MDFe", new InfSchema()
            {
                Tag = "rodo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\mdfeModalRodoviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Rodoviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region Ferroviário
            SchemaXML.InfSchemas.Add("NFE-ferrov-MDFe", new InfSchema()
            {
                Tag = "ferrov",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\mdfeModalFerroviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Ferroviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region Dutoviário
            SchemaXML.InfSchemas.Add("NFE-duto-MDFe", new InfSchema()
            {
                Tag = "duto",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\mdfeModalDutoviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Dutoviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region Aeroviário
            SchemaXML.InfSchemas.Add("NFE-aereo-MDFe", new InfSchema()
            {
                Tag = "aereo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\mdfeModalAereo_v{0}.xsd",
                Descricao = "XML de CTe - Modal Aeroviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region Aquaviário
            SchemaXML.InfSchemas.Add("NFE-aquav-MDFe", new InfSchema()
            {
                Tag = "aquav",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\mdfeModalAquaviario_v{0}.xsd",
                Descricao = "XML de CTe - Modal Aquaviário",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #endregion Schemas Modal

            #endregion MDF-e
        }
    }
}
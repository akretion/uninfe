namespace NFe.Components
{
    public class SchemaXML_EFDReinf
    {
        public static void CriarListaIDXML()
        {
            #region R-1000 - Informações do Contribuinte

            SchemaXML.InfSchemas.Add("Reinf-evtInfoContri", new InfSchema()
            {
                Tag = "evtInfoContri",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtInfoContribuinte-v{0}.xsd",
                Descricao = "XML EFDReinf - 1000 - Informações do Contribuinte",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtInfoContri",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtInfoContribuinte/v{0}"
            });

            #endregion R-1000 - Informações do Contribuinte

            #region R-1070 - Tabela de Processos Administrativos/Judiciais

            SchemaXML.InfSchemas.Add("Reinf-evtTabProcesso", new InfSchema()
            {
                Tag = "evtTabProcesso",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtTabProcesso-v{0}.xsd",
                Descricao = "XML EFDReinf - 1070 - Tabela de Processos Administrativos/Judiciais",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtTabProcesso",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtTabProcesso/v{0}"
            });

            #endregion R-1070 - Tabela de Processos Administrativos/Judiciais

            #region R-2010 - Retenção Contribuição Previdenciária - Serviços Tomados

            SchemaXML.InfSchemas.Add("Reinf-evtServTom", new InfSchema()
            {
                Tag = "evtServTom",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtTomadorServicos-v{0}.xsd",
                Descricao = "XML EFDReinf - 2010 - Retenção Contribuição Previdenciária - Serviços Tomados",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtServTom",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtTomadorServicos/v{0}"
            });

            #endregion R-2010 - Retenção Contribuição Previdenciária - Serviços Tomados

            #region R-2020 - Retenção Contribuição Previdenciária - Serviços Prestados

            SchemaXML.InfSchemas.Add("Reinf-evtServPrest", new InfSchema()
            {
                Tag = "evtServPrest",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtPrestadorServicos-v{0}.xsd",
                Descricao = "XML EFDReinf - 2020 - Retenção Contribuição Previdenciária - Serviços Prestados",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtServPrest",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtPrestadorServicos/v{0}"
            });

            #endregion R-2020 - Retenção Contribuição Previdenciária - Serviços Prestados

            #region R-2030 - Recursos Recebidos por Associação Desportiva

            SchemaXML.InfSchemas.Add("Reinf-evtAssocDespRec", new InfSchema()
            {
                Tag = "evtAssocDespRec",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtRecursoRecebidoAssociacao-v{0}.xsd",
                Descricao = "XML EFDReinf - 2030 - Recursos Recebidos por Associação Desportiva",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtAssocDespRec",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtRecursoRecebidoAssociacao/v{0}"
            });

            #endregion R-2030 - Recursos Recebidos por Associação Desportiva

            #region R-2040 - Recursos Repassados para Associação Desportiva

            SchemaXML.InfSchemas.Add("Reinf-evtAssocDespRep", new InfSchema()
            {
                Tag = "evtAssocDespRep",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtRecursoRepassadoAssociacao-v{0}.xsd",
                Descricao = "XML EFDReinf - 2040 - Recursos Repassados para Associação Desportiva",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtAssocDespRep",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtRecursoRepassadoAssociacao/v{0}"
            });

            #endregion R-2040 - Recursos Repassados para Associação Desportiva

            #region R-2050 - Comercialização da Produção por Produtor Rural PJ/Agroindústria

            SchemaXML.InfSchemas.Add("Reinf-evtComProd", new InfSchema()
            {
                Tag = "evtComProd",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtInfoProdRural-v{0}.xsd",
                Descricao = "XML EFDReinf - 2050 - Comercialização da Produção por Produtor Rural PJ/Agroindústria",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtComProd",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtInfoProdRural/v{0}"
            });

            #endregion R-2050 - Comercialização da Produção por Produtor Rural PJ/Agroindústria

            #region R-2055 - Aquisição de produção rural

            SchemaXML.InfSchemas.Add("Reinf-evtAqProd", new InfSchema()
            {
                Tag = "evtAqProd",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtAquisicaoProdRural-v{0}.xsd",
                Descricao = "XML EFDReinf - 2055 - Aquisição de produção rural",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtAqProd",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evt2055AquisicaoProdRural/v{0}"
            });

            #endregion 

            #region R-2060 - Contribuição Previdenciária sobre a Receita Bruta - CPRB

            SchemaXML.InfSchemas.Add("Reinf-evtCPRB", new InfSchema()
            {
                Tag = "evtCPRB",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtInfoCPRB-v{0}.xsd",
                Descricao = "XML EFDReinf - 2060 - Contribuição Previdenciária sobre a Receita Bruta - CPRB",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtCPRB",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtInfoCPRB/v{0}"
            });

            #endregion R-2060 - Contribuição Previdenciária sobre a Receita Bruta - CPRB

            #region R-2098 - Reabertura dos Eventos Periódicos

            SchemaXML.InfSchemas.Add("Reinf-evtReabreEvPer", new InfSchema()
            {
                Tag = "evtReabreEvPer",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtReabreEvPer-v{0}.xsd",
                Descricao = "XML EFDReinf - 2098 - Reabertura dos Eventos Periódicos",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtReabreEvPer",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtReabreEvPer/v{0}"
            });

            #endregion R-2098 - Reabertura dos Eventos Periódicos

            #region R-2099 - Fechamento dos Eventos Periódicos

            SchemaXML.InfSchemas.Add("Reinf-evtFechaEvPer", new InfSchema()
            {
                Tag = "evtFechaEvPer",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtFechamento-v{0}.xsd",
                Descricao = "XML EFDReinf - 2099 - Fechamento dos Eventos Periódicos",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtFechaEvPer",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtFechamento/v{0}"
            });

            #endregion R-2099 - Fechamento dos Eventos Periódicos

            #region R-3010 - Receita de Espetáculo Desportivo

            SchemaXML.InfSchemas.Add("Reinf-evtEspDesportivo", new InfSchema()
            {
                Tag = "evtEspDesportivo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtEspDesportivo-v{0}.xsd",
                Descricao = "XML EFDReinf - 3010 - Receita de Espetáculo Desportivo",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtEspDesportivo",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtEspDesportivo/v{0}"
            });

            #endregion R-3010 - Receita de Espetáculo Desportivo

            #region R-5001 - Informações das bases e dos tributos consolidados por contribuinte - evtTotal (versão 1.02.00 ou inferior)

            SchemaXML.InfSchemas.Add("Reinf-evtTotal", new InfSchema()
            {
                Tag = "evtTotal",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\retornoTotalizadorContribuinte-v{0}.xsd",
                Descricao = "XML EFDReinf - 5001 - Informações das bases e dos tributos consolidados por contribuinte",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtTotal",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/retornoTotalizadorContribuinte/v{0}"
            });

            #endregion R-5001 - Informações das bases e dos tributos consolidados por contribuinte - evtTotal (versão 1.02.00 ou inferior)

            #region R-5001 - Informações das bases e dos tributos consolidados por contribuinte - evtTotalContrib - versão 1.03.00

            SchemaXML.InfSchemas.Add("Reinf-evtTotalContrib", new InfSchema()
            {
                Tag = "evtTotalContrib",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\retornoTotalizadorContribuinte-v{0}.xsd",
                Descricao = "XML EFDReinf - 5001 - Informações das bases e dos tributos consolidados por contribuinte",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtTotalContrib",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtTotalContrib/v{0}"
            });

            #endregion R-5001 - Informações das bases e dos tributos consolidados por contribuinte - evtTotalContrib - versão 1.03.00

            #region R-9000 - Exclusão de Eventos

            SchemaXML.InfSchemas.Add("Reinf-evtExclusao", new InfSchema()
            {
                Tag = "evtExclusao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\evtExclusao-v{0}.xsd",
                Descricao = "XML EFDReinf - 9000 - Exclusão de Eventos",
                TagAssinatura = "Reinf",
                TagAtributoId = "evtExclusao",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/evtExclusao/v{0}"
            });

            #endregion R-9000 - Exclusão de Eventos

            #region Lote de eventos

            SchemaXML.InfSchemas.Add("Reinf-loteEventos", new InfSchema()
            {
                Tag = "loteEventos",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "EFDReinf\\envioLoteEventos-v{0}.xsd",
                Descricao = "EFDReinf - XML de Envio de Lote de Eventos",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.reinf.esocial.gov.br/schemas/envioLoteEventos/v{0}"
            });

            #endregion Lote de eventos

            #region Consulta Lote de eventos

            SchemaXML.InfSchemas.Add("Reinf-ConsultaInformacoesConsolidadas", new InfSchema()
            {
                Tag = "ConsultaInformacoesConsolidadas",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "EFDReinf - XML de Consulta de Lote de Eventos",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            #endregion Consulta Lote de eventos

            #region Consultas do EFDReinf

            SchemaXML.InfSchemas.Add("Reinf-ConsultaTotalizadores", new InfSchema()
            {
                Tag = "ConsultaTotalizadores",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "EFDReinf - XML de Consultas do EFDReinf",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            SchemaXML.InfSchemas.Add("Reinf-ConsultaReciboEvento", new InfSchema()
            {
                Tag = "ConsultaReciboEvento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "EFDReinf - XML de Consultas do EFDReinf",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            #endregion Consultas do EFDReinf
        }
    }
}
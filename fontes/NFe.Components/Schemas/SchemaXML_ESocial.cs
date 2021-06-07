namespace NFe.Components
{
    public class SchemaXML_ESocial
    {
        public static void CriarListaIDXML()
        {
            #region S-1000 - Informações do Empregador/Contribuinte/Órgão Público

            SchemaXML.InfSchemas.Add("eSocial-evtInfoEmpregador", new InfSchema()
            {
                Tag = "evtInfoEmpregador",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtInfoEmpregador.xsd",
                Descricao = "XML eSocial - 1000 - Informações do Empregador/Contribuinte/Órgão Público",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtInfoEmpregador",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtInfoEmpregador/v_S_01_00_00"
            });

            #endregion S-1000 - Informações do Empregador/Contribuinte/Órgão Público

            #region S-1005 - Tabela de Estabelecimentos, Obras ou Unidades de Órgãos Públicos

            SchemaXML.InfSchemas.Add("eSocial-evtTabEstab", new InfSchema()
            {
                Tag = "evtTabEstab",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtTabEstab.xsd",
                Descricao = "XML eSocial - 1005 - Tabela de Estabelecimentos, Obras ou Unidades de Órgãos Públicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtTabEstab",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtTabEstab/v_S_01_00_00"
            });

            #endregion S-1005 - Tabela de Estabelecimentos, Obras ou Unidades de Órgãos Públicos

            #region S-1010 - Tabela de Rubricas

            SchemaXML.InfSchemas.Add("eSocial-evtTabRubrica", new InfSchema()
            {
                Tag = "evtTabRubrica",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtTabRubrica.xsd",
                Descricao = "XML eSocial - 1295 - Solicitação de Totalização para Pagamento em Contingência",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtTabRubrica",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtTabRubrica/v_S_01_00_00"
            });

            #endregion S-1010 - Tabela de Rubricas

            #region S-1020 - Tabela de Lotações Tributárias

            SchemaXML.InfSchemas.Add("eSocial-evtTabLotacao", new InfSchema()
            {
                Tag = "evtTabLotacao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtTabLotacao.xsd",
                Descricao = "XML eSocial - 1020 - Tabela de Lotações Tributárias",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtTabLotacao",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtTabLotacao/v_S_01_00_00"
            });

            #endregion S-1020 - Tabela de Lotações Tributárias

            #region S-1070 - Tabela de Processos Administrativos/Judiciais

            SchemaXML.InfSchemas.Add("eSocial-evtTabProcesso", new InfSchema()
            {
                Tag = "evtTabProcesso",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtTabProcesso.xsd",
                Descricao = "XML eSocial - 1070 - Tabela de Processos Administrativos/Judiciais",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtTabProcesso",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtTabProcesso/v_S_01_00_00"
            });

            #endregion S-1070 - Tabela de Processos Administrativos/Judiciais

            #region S-1200 - Remuneração de trabalhador vinculado ao Regime Geral de Previd. Social

            SchemaXML.InfSchemas.Add("eSocial-evtRemun", new InfSchema()
            {
                Tag = "evtRemun",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtRemun.xsd",
                Descricao = "XML eSocial - 1200 - Remuneração de trabalhador vinculado ao Regime Geral de Previd. Social",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtRemun",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtRemun/v_S_01_00_00"
            });

            #endregion S-1200 - Remuneração de trabalhador vinculado ao Regime Geral de Previd. Social

            #region S-1202 - Remuneração de servidor vinculado a Regime Próprio de Previd. Social

            SchemaXML.InfSchemas.Add("eSocial-evtRmnRPPS", new InfSchema()
            {
                Tag = "evtRmnRPPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtRmnRPPS.xsd",
                Descricao = "XML eSocial - 1202 - Remuneração de servidor vinculado a Regime Próprio de Previd. Social",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtRmnRPPS",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtRmnRPPS/v_S_01_00_00"
            });

            #endregion S-1202 - Remuneração de servidor vinculado a Regime Próprio de Previd. Social

            #region S-1207 - Benefícios previdenciários - RPPS

            SchemaXML.InfSchemas.Add("eSocial-evtBenPrRP", new InfSchema()
            {
                Tag = "evtBenPrRP",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtBenPrRP.xsd",
                Descricao = "XML eSocial - 1207 - Benefícios previdenciários - RPPS",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtBenPrRP",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtBenPrRP/v_S_01_00_00"
            });

            #endregion S-1207 - Benefícios previdenciários - RPPS

            #region S-1210 - Pagamentos de Rendimentos do Trabalho

            SchemaXML.InfSchemas.Add("eSocial-evtPgtos", new InfSchema()
            {
                Tag = "evtPgtos",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtPgtos.xsd",
                Descricao = "XML eSocial - 1210 - Pagamentos de Rendimentos do Trabalho",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtPgtos",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtPgtos/v_S_01_00_00"
            });

            #endregion S-1210 - Pagamentos de Rendimentos do Trabalho

            #region S-1260 - Comercialização da Produção Rural Pessoa Física

            SchemaXML.InfSchemas.Add("eSocial-evtComProd", new InfSchema()
            {
                Tag = "evtComProd",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtComProd.xsd",
                Descricao = "XML eSocial - 1260 - Comercialização da Produção Rural Pessoa Física",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtComProd",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtComProd/v_S_01_00_00"
            });

            #endregion S-1260 - Comercialização da Produção Rural Pessoa Física

            #region S-1280 - Informações Complementares aos Eventos Periódicos

            SchemaXML.InfSchemas.Add("eSocial-evtInfoComplPer", new InfSchema()
            {
                Tag = "evtInfoComplPer",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtInfoComplPer.xsd",
                Descricao = "XML eSocial - 1280 - Informações Complementares aos Eventos Periódicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtInfoComplPer",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtInfoComplPer/v_S_01_00_00"
            });

            #endregion S-1280 - Informações Complementares aos Eventos Periódicos

            #region S-1298 - Reabertura dos Eventos Periódicos

            SchemaXML.InfSchemas.Add("eSocial-evtReabreEvPer", new InfSchema()
            {
                Tag = "evtReabreEvPer",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtReabreEvPer.xsd",
                Descricao = "XML eSocial - 1298 - Reabertura dos Eventos Periódicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtReabreEvPer",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtReabreEvPer/v_S_01_00_00"
            });

            #endregion S-1298 - Reabertura dos Eventos Periódicos

            #region S-1299 - Fechamento dos Eventos Periódicos

            SchemaXML.InfSchemas.Add("eSocial-evtFechaEvPer", new InfSchema()
            {
                Tag = "evtFechaEvPer",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtFechaEvPer.xsd",
                Descricao = "XML eSocial - 1299 - Fechamento dos Eventos Periódicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtFechaEvPer",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtFechaEvPer/v_S_01_00_00"
            });

            #endregion S-1299 - Fechamento dos Eventos Periódicos

            #region S-2190 - Admissão de Trabalhador - Registro Preliminar

            SchemaXML.InfSchemas.Add("eSocial-evtAdmPrelim", new InfSchema()
            {
                Tag = "evtAdmPrelim",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtAdmPrelim.xsd",
                Descricao = "XML eSocial - 2190 - Admissão de Trabalhador - Registro Preliminar",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtAdmPrelim",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtAdmPrelim/v_S_01_00_00"
            });

            #endregion S-2190 - Admissão de Trabalhador - Registro Preliminar

            #region S-2200 - Cadastramento Inicial do Vínculo e Admissão/Ingresso de Trabalhador

            SchemaXML.InfSchemas.Add("eSocial-evtAdmissao", new InfSchema()
            {
                Tag = "evtAdmissao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtAdmissao.xsd",
                Descricao = "XML eSocial - 2200 - Cadastramento Inicial do Vínculo e Admissão/Ingresso de Trabalhador",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtAdmissao",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtAdmissao/v_S_01_00_00"
            });

            #endregion S-2200 - Cadastramento Inicial do Vínculo e Admissão/Ingresso de Trabalhador

            #region S-2205 - Alteração de Dados Cadastrais do Trabalhador

            SchemaXML.InfSchemas.Add("eSocial-evtAltCadastral", new InfSchema()
            {
                Tag = "evtAltCadastral",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtAltCadastral.xsd",
                Descricao = "XML eSocial - 2205 - Alteração de Dados Cadastrais do Trabalhador",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtAltCadastral",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtAltCadastral/v_S_01_00_00"
            });

            #endregion S-2205 - Alteração de Dados Cadastrais do Trabalhador

            #region S-2206 - Alteração de Contrato de Trabalho

            SchemaXML.InfSchemas.Add("eSocial-evtAltContratual", new InfSchema()
            {
                Tag = "evtAltContratual",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtAltContratual.xsd",
                Descricao = "XML eSocial - 2206 - Alteração de Contrato de Trabalho",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtAltContratual",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtAltContratual/v_S_01_00_00"
            });

            #endregion S-2206 - Alteração de Contrato de Trabalho

            #region S-2210 - Comunicação de Acidente de Trabalho

            SchemaXML.InfSchemas.Add("eSocial-evtCAT", new InfSchema()
            {
                Tag = "evtCAT",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCAT.xsd",
                Descricao = "XML eSocial - 2210 - Comunicação de Acidente de Trabalho",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCAT",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCAT/v_S_01_00_00"
            });

            #endregion S-2210 - Comunicação de Acidente de Trabalho

            #region S-2220 - Monitoramento da Saúde do Trabalhador

            SchemaXML.InfSchemas.Add("eSocial-evtMonit", new InfSchema()
            {
                Tag = "evtMonit",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtMonit.xsd",
                Descricao = "XML eSocial - 2220 - Monitoramento da Saúde do Trabalhador",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtMonit",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtMonit/v_S_01_00_00"
            });

            #endregion S-2220 - Monitoramento da Saúde do Trabalhador

            #region S-2230 - Afastamento Temporário

            SchemaXML.InfSchemas.Add("eSocial-evtAfastTemp", new InfSchema()
            {
                Tag = "evtAfastTemp",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtAfastTemp.xsd",
                Descricao = "XML eSocial - 2230 - Afastamento Temporário",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtAfastTemp",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtAfastTemp/v_S_01_00_00"
            });

            #endregion S-2230 - Afastamento Temporário

            #region S-2231 - Cessão/Exercício em outro Órgão

            SchemaXML.InfSchemas.Add("eSocial-evtCessao", new InfSchema()
            {
                Tag = "evtCessao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCessao.xsd",
                Descricao = "XML eSocial - 2231 - Cessão/Exercício em outro Órgão",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCessao",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCessao/v_S_01_00_00"
            });

            #endregion

            #region S-2240 - Condições Ambientais do Trabalho - Fatores de Risco

            SchemaXML.InfSchemas.Add("eSocial-evtExpRisco", new InfSchema()
            {
                Tag = "evtExpRisco",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtExpRisco.xsd",
                Descricao = "XML eSocial - 2240 - Condições Ambientais do Trabalho - Fatores de Risco",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtExpRisco",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtExpRisco/v_S_01_00_00"
            });

            #endregion S-2240 - Condições Ambientais do Trabalho - Fatores de Risco

            #region S-1270 - Contratação de Trabalhadores Avulsos Não Portuários

            SchemaXML.InfSchemas.Add("eSocial-evtContratAvNP", new InfSchema()
            {
                Tag = "evtContratAvNP",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtContratAvNP.xsd",
                Descricao = "XML eSocial - 1270 - Contratação de Trabalhadores Avulsos Não Portuários",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtContratAvNP",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtContratAvNP/v_S_01_00_00"
            });

            #endregion S-1270 - Contratação de Trabalhadores Avulsos Não Portuários

            #region S-2298 - Reintegração

            SchemaXML.InfSchemas.Add("eSocial-evtReintegr", new InfSchema()
            {
                Tag = "evtReintegr",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtReintegr.xsd",
                Descricao = "XML eSocial - 2298 - Reintegração",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtReintegr",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtReintegr/v_S_01_00_00"
            });

            #endregion S-2298 - Reintegração

            #region S-2299 - Desligamento

            SchemaXML.InfSchemas.Add("eSocial-evtDeslig", new InfSchema()
            {
                Tag = "evtDeslig",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtDeslig.xsd",
                Descricao = "XML eSocial - 2299 - Desligamento",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtDeslig",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtDeslig/v_S_01_00_00"
            });

            #endregion S-2299 - Desligamento

            #region S-2300 - Trabalhador Sem Vínculo de Emprego/Estatutário - Início

            SchemaXML.InfSchemas.Add("eSocial-evtTSVInicio", new InfSchema()
            {
                Tag = "evtTSVInicio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtTSVInicio.xsd",
                Descricao = "XML eSocial - 2300 - Trabalhador Sem Vínculo de Emprego/Estatutário - Início",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtTSVInicio",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtTSVInicio/v_S_01_00_00"
            });

            #endregion S-2300 - Trabalhador Sem Vínculo de Emprego/Estatutário - Início

            #region S-2306 - Trabalhador Sem Vínculo de Emprego/Estatutário - Alteração Contratual

            SchemaXML.InfSchemas.Add("eSocial-evtTSVAltContr", new InfSchema()
            {
                Tag = "evtTSVAltContr",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtTSVAltContr.xsd",
                Descricao = "XML eSocial - 2306 - Trabalhador Sem Vínculo de Emprego/Estatutário - Alteração Contratual",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtTSVAltContr",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtTSVAltContr/v_S_01_00_00"
            });

            #endregion S-2306 - Trabalhador Sem Vínculo de Emprego/Estatutário - Alteração Contratual

            #region S-2399 - Trabalhador Sem Vínculo de Emprego/Estatutário - Término

            SchemaXML.InfSchemas.Add("eSocial-evtTSVTermino", new InfSchema()
            {
                Tag = "evtTSVTermino",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtTSVTermino.xsd",
                Descricao = "XML eSocial - 2399 - Trabalhador Sem Vínculo de Emprego/Estatutário - Término",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtTSVTermino",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtTSVTermino/v_S_01_00_00"
            });

            #endregion S-2399 - Trabalhador Sem Vínculo de Emprego/Estatutário - Término

            #region S-2400 - Cadastro de Beneficiários – Entes Públicos

            SchemaXML.InfSchemas.Add("eSocial-evtCdBenefIn", new InfSchema()
            {
                Tag = "evtCdBenefIn",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCdBenefIn.xsd",
                Descricao = "XML eSocial - 2400 - Cadastro de Beneficiário - Entes Públicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCdBenefIn",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCdBenefIn/v_S_01_00_00"
            });

            #endregion

            #region S-2405 - Alteração de Dados Cadastrais do Beneficiário - Entes Públicos

            SchemaXML.InfSchemas.Add("eSocial-evtCdBenefAlt", new InfSchema()
            {
                Tag = "evtCdBenefAlt",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCdBenefAlt.xsd",
                Descricao = "XML eSocial - 2405 - Alteração de Dados Cadastrais do Beneficiário - Entes Públicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCdBenefAlt",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCdBenefAlt/v_S_01_00_00"
            });

            #endregion

            #region S-2410 - Cadastro de Benefícios Ente Público

            SchemaXML.InfSchemas.Add("eSocial-evtCdBenIn", new InfSchema()
            {
                Tag = "evtCdBenIn",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCdBenIn.xsd",
                Descricao = "XML eSocial - 2410 - Cadastro de Benefícios Ente Público",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCdBenIn",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCdBenIn/v_S_01_00_00"
            });

            #endregion

            #region S-2416 - Alteração do Cadastro de Benefícios - Entes Públicos

            SchemaXML.InfSchemas.Add("eSocial-evtCdBenAlt", new InfSchema()
            {
                Tag = "evtCdBenAlt",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCdBenAlt.xsd",
                Descricao = "XML eSocial - 2416 - Alteração do Cadastro de Benefícios - Entes Públicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCdBenAlt",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCdBenAlt/v_S_01_00_00"
            });

            #endregion

            #region S-2418 - Reativação de Benefício - Entes Públicos

            SchemaXML.InfSchemas.Add("eSocial-evtReativBen", new InfSchema()
            {
                Tag = "evtReativBen",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtReativBen.xsd",
                Descricao = "XML eSocial - 2418 - Reativação de Benefício - Entes Públicos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtReativBen",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtReativBen/v_S_01_00_00"
            });

            #endregion

            #region S-2420 - Cadastro de Benefício - Entes Públicos - Término

            SchemaXML.InfSchemas.Add("eSocial-evtCdBenTerm", new InfSchema()
            {
                Tag = "evtCdBenTerm",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCdBenTerm.xsd",
                Descricao = "XML eSocial - 2420 - Cadastro de Benefício - Entes Públicos - Término",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCdBenTerm",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCdBenTerm/v_S_01_00_00"
            });

            #endregion

            #region S-3000 - Exclusão de eventos

            SchemaXML.InfSchemas.Add("eSocial-evtExclusao", new InfSchema()
            {
                Tag = "evtExclusao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtExclusao.xsd",
                Descricao = "XML eSocial - 3000 - Exclusão de eventos",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtExclusao",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtExclusao/v_S_01_00_00"
            });

            #endregion S-3000 - Exclusão de eventos

            #region S-5001 - Informações das contribuições sociais por trabalhador

            SchemaXML.InfSchemas.Add("eSocial-evtBasesTrab", new InfSchema()
            {
                Tag = "evtBasesTrab",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtBasesTrab.xsd",
                Descricao = "XML eSocial - 5001 - Informações das contribuições sociais por trabalhador",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtBasesTrab",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtBasesTrab/v_S_01_00_00"
            });

            #endregion S-5001 - Informações das contribuições sociais por trabalhador

            #region S-5002 - Imposto de Renda Retido na Fonte

            SchemaXML.InfSchemas.Add("eSocial-evtIrrfBenef", new InfSchema()
            {
                Tag = "evtIrrfBenef",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtIrrfBenef.xsd",
                Descricao = "XML eSocial - 5002 - Imposto de Renda Retido na Fonte",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtIrrfBenef",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtIrrfBenef/v_S_01_00_00"
            });

            #endregion

            #region S-5003 - Informações do FGTS por Trabalhador

            SchemaXML.InfSchemas.Add("eSocial-evtBasesFGTS", new InfSchema()
            {
                Tag = "evtBasesFGTS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtBasesFGTS.xsd",
                Descricao = "XML eSocial - 5003 - Informações do FGTS por Trabalhador",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtBasesFGTS",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtBasesFGTS/v_S_01_00_00"
            });

            #endregion

            #region S-5013 - Informações do FGTS Consolidadas por Contribuinte

            SchemaXML.InfSchemas.Add("eSocial-evtFGTS", new InfSchema()
            {
                Tag = "evtFGTS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtFGTS.xsd",
                Descricao = "XML eSocial - 5013 - Informações do FGTS Consolidadas por Contribuinte",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtFGTS",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtFGTS/v_S_01_00_00"
            });

            #endregion

            #region S-5011 - Informações das contribuições sociais consolidadas por contribuinte

            SchemaXML.InfSchemas.Add("eSocial-evtCS", new InfSchema()
            {
                Tag = "evtCS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\evtCS.xsd",
                Descricao = "XML eSocial - 5011 - Informações das contribuições sociais consolidadas por contribuinte",
                TagAssinatura = "eSocial",
                TagAtributoId = "evtCS",
                TargetNameSpace = "http://www.esocial.gov.br/schema/evt/evtCS/v_S_01_00_00"
            });

            #endregion S-5011 - Informações das contribuições sociais consolidadas por contribuinte

            #region Lote de eventos

            SchemaXML.InfSchemas.Add("eSocial-envioLoteEventos", new InfSchema()
            {
                Tag = "envioLoteEventos",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\EnvioLoteEventos-v1_1_1.xsd",
                Descricao = "eSocial - XML de Envio de Lote de Eventos",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.esocial.gov.br/schema/lote/eventos/envio/v1_1_1"
            });

            #endregion Lote de eventos

            #region Consulta Lote de eventos

            SchemaXML.InfSchemas.Add("eSocial-consultaLoteEventos", new InfSchema()
            {
                Tag = "consultaLoteEventos",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\ConsultaLoteEventos-v1_0_0.xsd",
                Descricao = "eSocial - XML de Consulta de Lote de Eventos",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.esocial.gov.br/schema/lote/eventos/envio/consulta/retornoProcessamento/v1_0_0"
            });

            #endregion Consulta Lote de eventos

            #region Consulta Identificadores dos Eventos dos Empregadores

            SchemaXML.InfSchemas.Add("eSocial-consultaIdentificadoresEvts-consultaEvtsEmpregador", new InfSchema()
            {
                Tag = "consultaIdentificadoresEvts",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\ConsultaIdentificadoresEventosEmpregador-v1_0_0.xsd",
                Descricao = "eSocial - XML de Consulta Identificadores dos Eventos dos Empregadores",
                TagAssinatura = "eSocial",
                TagAtributoId = "consultaIdentificadoresEvts",
                TargetNameSpace = "http://www.esocial.gov.br/schema/consulta/identificadores-eventos/empregador/v1_0_0"
            });

            #endregion Consulta Identificadores dos Eventos dos Empregadores

            #region Consulta Identificadores dos Eventos das Tabelas

            SchemaXML.InfSchemas.Add("eSocial-consultaIdentificadoresEvts-consultaEvtsTabela", new InfSchema()
            {
                Tag = "consultaIdentificadoresEvts",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\ConsultaIdentificadoresEventosTabela-v1_0_0.xsd",
                Descricao = "eSocial - XML deConsulta Identificadores dos Eventos das Tabelas",
                TagAssinatura = "eSocial",
                TagAtributoId = "consultaIdentificadoresEvts",
                TargetNameSpace = "http://www.esocial.gov.br/schema/consulta/identificadores-eventos/tabela/v1_0_0"
            });

            #endregion Consulta Identificadores dos Eventos das Tabelas

            #region Consulta Identificadores dos Eventos dos Trabalhadores

            SchemaXML.InfSchemas.Add("eSocial-consultaIdentificadoresEvts-consultaEvtsTrabalhador", new InfSchema()
            {
                Tag = "consultaIdentificadoresEvts",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\ConsultaIdentificadoresEventosTrabalhador-v1_0_0.xsd",
                Descricao = "eSocial - XML de Consulta Identificadores dos Eventos dos Trabalhadores",
                TagAssinatura = "eSocial",
                TagAtributoId = "consultaIdentificadoresEvts",
                TargetNameSpace = "http://www.esocial.gov.br/schema/consulta/identificadores-eventos/trabalhador/v1_0_0"
            });

            #endregion Consulta Identificadores dos Eventos dos Trabalhadores

            #region Download dos Eventos por Id

            SchemaXML.InfSchemas.Add("eSocial-download-solicDownloadEvtsPorId", new InfSchema()
            {
                Tag = "download",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\SolicitacaoDownloadEventosPorId-v1_0_0.xsd",
                Descricao = "eSocial - XML de Download dos Eventos por Id",
                TagAssinatura = "eSocial",
                TagAtributoId = "download",
                TargetNameSpace = "http://www.esocial.gov.br/schema/download/solicitacao/id/v1_0_0"
            });

            #endregion Download dos Eventos por Id

            #region Download dos Eventos por Número do Recibo

            SchemaXML.InfSchemas.Add("eSocial-download-solicDownloadEventosPorNrRecibo", new InfSchema()
            {
                Tag = "download",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "eSocial\\SolicitacaoDownloadEventosPorNrRecibo-v1_0_0.xsd",
                Descricao = "eSocial - XML de Download dos Eventos por Número do Recibo",
                TagAssinatura = "eSocial",
                TagAtributoId = "download",
                TargetNameSpace = "http://www.esocial.gov.br/schema/download/solicitacao/nrRecibo/v1_0_0"
            });

            #endregion Download dos Eventos por Número do Recibo
        }
    }
}
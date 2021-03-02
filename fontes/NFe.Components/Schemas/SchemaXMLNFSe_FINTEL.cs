using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_FINTEL
    {
        public static void CriarListaIDXML()
        {

            #region NFSe

            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de lote RPS

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de Lote RPS

            #region XML de Consulta de NFSe por Faixa

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por Faixa

            #region XML de Consulta de NFSe por RPS

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por RPS

            #region XML de Consulta da Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta Situação do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta da Situação do Lote RPS

            #endregion NFSe

            #region CFSe

            #region XML de lote do CFSe

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteCupomSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteCupomSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML de Lote CFSe",
                TagLoteAssinatura = "EnviarLoteCupomSincronoEnvio",
                TagLoteAtributoId = "LoteCupom",
                TagAssinatura = "InfDeclaracaoPrestacaoServico",
                TagAtributoId = "Cupom",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteCupomEnvio", new InfSchema()
            {
                Tag = "EnviarLoteCupomEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML de Lote CFSe",
                TagLoteAssinatura = "EnviarLoteCupomEnvio",
                TagLoteAtributoId = "LoteCupom",
                TagAssinatura = "InfDeclaracaoPrestacaoServico",
                TagAtributoId = "Cupom",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML de lote do CFSe

            #region XML de Cancelamento do CFSe

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-CancelarCupomEnvio", new InfSchema()
            {
                Tag = "CancelarCupomEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML de Cancelamento do CFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML de Cancelamento do CFSe

            #region XML de Consulta de Lote CFSe

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarLoteCupomEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteCupomEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML de Consulta de Lote CFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML de Consulta de Lote CFSe

            #region XML de Consulta CFSe

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarCfseEnvio", new InfSchema()
            {
                Tag = "ConsultarCfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML de Consulta do CFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML de Consulta CFSe

            #region XML para configurar/ativar terminal de CFSe

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConfigurarAtivarTerminal", new InfSchema()
            {
                Tag = "ConfigurarAtivarTerminal",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML para configurar/ativar terminal de CFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML para configurar/ativar terminal de CFSe

            #region XML para informar que terminal CFSe está em manutenção

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarInformeManutencao", new InfSchema()
            {
                Tag = "EnviarInformeManutencao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML para informar que o terminal de CFSe está em manutenção",
                TagAssinatura = "EnviarInformeManutencao",
                TagAtributoId = "Pedido",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML para informar que terminal CFSe está em manutenção

            #region XML para informar dia sem movimento de CFSe

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-InformeTrasmissaoSemMovimento", new InfSchema()
            {
                Tag = "InformeTrasmissaoSemMovimento",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML para informar dia sem movimento de CFSe",
                TagAssinatura = "InformeTrasmissaoSemMovimento",
                TagAtributoId = "Pedido",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML para informar dia sem movimento de CFSe

            #region XML para consultar dados cadastro terminal CFSe

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarDadosCadastro", new InfSchema()
            {
                Tag = "ConsultarDadosCadastro",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\cfse.xsd",
                Descricao = "XML para consultar dados cadastro terminal CFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.paracambi.rj.gov.br/Arquivos/cfse.xsd"
            });

            #endregion XML para consultar dados cadastro terminal CFSe

            #endregion CFSe

        }
    }
}
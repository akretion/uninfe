 using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_PRONIN
    {
        public static void CriarListaIDXML()
        {
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
            #endregion XML de Consulta de NFSe por Data

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
            #endregion XML de Consulta de NFSe por Rps

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
            #endregion XML de Consulta de Lote RPS

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
            #endregion XML de Cancelamento de NFS-e

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
            #endregion XML de Consulta Situação do Lote RPS

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
            #endregion XML de lote RPS

            #region Gerar NFSe Envio

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-GerarNfseEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion Gerar NFSe Envio

            #region XML de lote RPS - Guarapuava-PR

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4109401-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Guarapuava-PR

            #region XML de lote RPS - Cachoeira do Sul-RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4303004-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Cachoeira do Sul-RS

            #region XML de lote RPS - Vacari-RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4322509-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Vacari-RS

            #region XML de lote RPS - Vera Cruz-SC

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-3556602-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Vera Cruz-SC

            #region XML de lote RPS - Cosmópolis-SP

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-3512803-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Cosmópolis-SP

            #region XML de lote RPS - Viamão-RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4323002-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Viamão-RS

            #region XML de lote RPS - Bastos-SP

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-3505807-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Bastos-SP

            #region XML de lote RPS - Mirassol-SP

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-3530300-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Mirassol-SP

            #region XML de lote RPS - Pator Branco-PR

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4118501-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Pator Branco-PR

            #region XML de lote RPS - Teodoro Sampaio-SP

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-3554300-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Teodoro Sampaio-SP

            #region XML de lote RPS - Naviraí - MS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-5005707-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Naviraí - MS

            #region XML de lote RPS - Picada Café-RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4314423-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Picada Café-RS

            #region XML de lote RPS - Catanduva-SP

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-3511102-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Catanduva-SP

            #region XML de lote RPS - Paranapanema-SP

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-3535804-EnviarLoteRpsEnvio", new InfSchema()
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
            #endregion XML de lote RPS - Paranapanema-SP

            #region XML de lote RPS - Entre-Ijuís - RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4306932-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Entre-Ijuís - RS

            #region XML de lote RPS - Uruguaiana-RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4322400-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"

                //Tag = "EnviarLoteRpsEnvio",
                //ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                //Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfDeclaracaoPrestacaoServico",
                //TagLoteAssinatura = "EnviarLoteRpsEnvio",
                //TagLoteAtributoId = "LoteRps",
                //TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Uruguaiana-RS

            #region XML de lote RPS - Caçapava do Sul-RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4302808-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Caçapava do Sul-RS


            #region XML de lote RPS - Agudo - RS

            SchemaXML.InfSchemas.Add("NFSE-PRONIN-4300109-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PRONIN\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de lote RPS - Agudo - RS
        }
    }
}
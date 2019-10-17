using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_IIBRASIL
    {
        public static void CriarListaIDXML()
        {

            #region XML para Gerar NFse
            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\IIBRASIL\\servico_enviar_lote_rps_envio.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_enviar_lote_rps_envio.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\IIBRASIL\\servico_cancelar_nfse_envio.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_enviar_lote_rps_envio.xsd"
            });
            #endregion -

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\IIBRASIL\\servico_consultar_lote_rps_envio.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_lote_rps_envio.xsd"
            });

            #endregion -
            #region XML de Consultar Nfse Servico Prestado

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-ConsultarNfseServicoPrestado", new InfSchema()
            {
                Tag = "ConsultarNfseServicoPrestado",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });

            #endregion
            

            #region Consulta NFSe Servico Tomado

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-ConsultarNfseServicoTomado", new InfSchema()
            {
                Tag = "ConsultarNfseServicoTomado",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe Servicos Tomados",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion Consulta NFSe Servico Tomado

            #region XML de Consulta de NFSe por Faixa

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion XML de Consulta de NFSe por Faixa

            #region Consulta NFSe por RPS

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\IIBRASIL\\servico_consultar_nfse_rps_envio.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_nfse_rps_envio.xsd"
            });
            #endregion

          

            #region Consulta NFSe 
          
            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\IIBRASIL\\servico_consultar_nfse_envio.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_nfse_envio.xsd"
            });
            #endregion Consulta NFSe 

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\IIBRASIL\\servico_consultar_situacao_lote_rps_envio.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://tempuri.org/servico_consultar_situacao_lote_rps_envio.xsd"
            });
            #endregion

            #region XML de lote RPS Síncrono

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\IIBRASIL\\",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TagLoteAssinatura = "",
                TagLoteAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion XML de lote RPS Síncrono
                  
            #region Substituir Nfse

            SchemaXML.InfSchemas.Add("NFSE-IIBRASIL-SubstituirNfseEnvio", new InfSchema()
            {
                Tag = "SubstituirNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Substituição de NFSe",
                TagAssinatura0 = "",
                TagAtributoId0 = "",
                TagAssinatura = "",
                TagAtributoId = "",
                TagLoteAssinatura = "",
                TagLoteAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion Substituir Nfse

          

        }
    }
}


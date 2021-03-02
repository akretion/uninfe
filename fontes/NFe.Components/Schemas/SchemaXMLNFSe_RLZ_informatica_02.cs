using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_RLZ_INFORMATICA_02
    {
        public static void CriarListaIDXML()
        {
            #region 2.02

            #region XML de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-RLZ_INFORMATICA_02-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\RLZ_INFORMATICA_02\\XSDRLZ_Informatica_02.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Lote RPS

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-RLZ_INFORMATICA_02-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\RLZ_INFORMATICA_02\\XSDRLZ_Informatica_02.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-RLZ_INFORMATICA_02-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\RLZ_INFORMATICA_02\\XSDRLZ_Informatica_02.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de Lote RPS

            #region XML de Consulta de NFSe por Data

            SchemaXML.InfSchemas.Add("NFSE-RLZ_INFORMATICA_02-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\RLZ_INFORMATICA_02\\XSDRLZ_Informatica_02.xsd",  //NFSe\\RLZ_INFORMATICA_02\\XSDRLZ_Informatica_02.xsd
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por Data

            #region XML de Consulta de NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-RLZ_INFORMATICA_02-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\RLZ_INFORMATICA_02\\XSDRLZ_Informatica_02.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion XML de Consulta de NFSe por Rps

            #endregion 2.02
        }
    }
}
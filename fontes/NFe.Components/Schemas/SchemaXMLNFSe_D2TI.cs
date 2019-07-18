using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_D2TI
    {
        public static void CriarListaIDXML()
        {
            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-D2TI-cancelamentoNfseLote", new InfSchema()
            {
                Tag = "cancelamentoNfseLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\D2TI\\CancelamentoNFSe_v1.00.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.ctaconsult.com/nfse"
            });
            #endregion


            #region XML para Gerar NFse
            SchemaXML.InfSchemas.Add("NFSE-D2TI-nfseLote", new InfSchema()
            {
                Tag = "nfseLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\D2TI\\RecepcaoNFSe_v1.00.xsd",
                Descricao = "XML de Lote RPS - Sincrono",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.ctaconsult.com/nfse"
            });
            #endregion
        }
    }
}

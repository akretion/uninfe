using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_BAURU_SP
    {
        public static void CriarListaIDXML()
        {
            #region BAURU_SP

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BAURU_SP-GerarNota", new InfSchema() {
                Tag = "GerarNota",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de cancelamento
            SchemaXML.InfSchemas.Add("NFSE-BAURU_SP-CancelarNota", new InfSchema()
            {
                Tag = "CancelarNota",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta situação de NFSe
            SchemaXML.InfSchemas.Add("NFSE-BAURU_SP-ConsultarNotaPrestador", new InfSchema()
            {
                Tag = "ConsultarNotaPrestador",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta situação de NFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BAURU_SP-ConsultarNotaValida", new InfSchema()
            {
                Tag = "ConsultarNotaValida",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #endregion BAURU_SP
        }
    }
}

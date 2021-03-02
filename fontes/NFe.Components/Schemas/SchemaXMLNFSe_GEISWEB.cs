using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_GEISWEB
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-GEISWEB-EnviaLoteRps", new InfSchema()
            {
                Tag = "EnviarLoteRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GEISWEB\\envio_lote_rps.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "Rps",
                TagLoteAssinatura = "",
                TagLoteAtributoId = "",
                TargetNameSpace = "http://www.geisweb.com.br/xsd/envio_lote_rps.xsd"
            });

            #endregion XML de lote RPS

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-GEISWEB-ConsultaLoteRps", new InfSchema()
            {
                Tag = "ConsultarLoteRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GEISWEB\\consulta_lote_rps.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.geisweb.com.br/xsd/envio_lote_rps.xsd"
            });

            #endregion XML de Consulta de Lote RPS

            #region Consulta NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-GEISWEB-ConsultaNfse", new InfSchema()
            {
                Tag = "ConsultarNfseRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GEISWEB\\consulta_nfse.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.geisweb.com.br/xsd/envio_lote_rps.xsd"
            });

            #endregion Consulta NFSe por Rps

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-GEISWEB-CancelaNfse", new InfSchema()
            {
                Tag = "CancelaNfse",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GEISWEB\\cancela_nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "CancelaNfse",
                TagAtributoId = "CnpjCpf",
                TargetNameSpace = "http://www.geisweb.com.br/xsd/envio_lote_rps.xsd"
            });

            #endregion XML de Cancelamento de NFS-e

        }
    }
}
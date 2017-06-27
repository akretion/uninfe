using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_GOIANIA
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-GOIANIA-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOIANIA\\nfse_gyn_v02.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://nfse.goiania.go.gov.br/xsd/nfse_gyn_v02.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-GOIANIA-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\GOIANIA\\nfse_gyn_v02.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfse.goiania.go.gov.br/xsd/nfse_gyn_v02.xsd"
            });
            #endregion
        }
    }
}

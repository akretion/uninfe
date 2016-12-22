using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_BHISS
    {
        public static void CriarListaIDXML()
        {
            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarNfseEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BHISS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BHISS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS (Assincrono)
            SchemaXML.InfSchemas.Add("NFSE-BHISS-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                //                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS (Assincrono)",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS (Sincrono)
            SchemaXML.InfSchemas.Add("NFSE-BHISS-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                //                ArquivoXSD = "NFSe\\BHISS\\nfse.xsd",
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfRps",
                TagLoteAssinatura = "GerarNfseEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion
        }
    }
}

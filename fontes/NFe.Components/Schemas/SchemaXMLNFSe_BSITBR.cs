using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_BSITBR
    {
        public static void CriarListaIDXML()
        {
            #region XML de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BSITBR-p:GerarNfseEnvio", new InfSchema()
            {
                Tag = "p:GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "NFSe\\BSITBR\\nfse-v2.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "p:Rps",
                TagAtributoId = "p:InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-BSITBR-p:EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "p:EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "NFSe\\BSITBR\\nfse-v2.xsd",
                Descricao = "XML de Lote RPS - Síncrono",
                TagAssinatura = "p:Rps",
                TagAtributoId = "p:InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "p:EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "p:LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BSITBR-p:CancelarNfseEnvio", new InfSchema()
            {
                Tag = "p:CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "NFSe\\BSITBR\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "p:Pedido",
                TagAtributoId = "p:InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BSITBR-p:ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "p:ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "NFSe\\BSITBR\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion            

            #region XML de Consulta de NFSe por RPS
            SchemaXML.InfSchemas.Add("NFSE-BSITBR-p:ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "p:ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "NFSe\\BSITBR\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion                       
        }
    }
}

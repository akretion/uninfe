using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_FINTEL
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                //TagLoteAssinatura = "GerarNfseEnvio",
                //TagLoteAtributoId = "Rps",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });

            SchemaXML.InfSchemas.Add("NFSE-FINTEL-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Lote RPS",
                //TagAssinatura = "Rps",
                //TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Faixa
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            
            #endregion

            #region XML de Consulta de NFSe por RPS
            SchemaXML.InfSchemas.Add("NFSE-FINTEL-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\FINTEL\\nfse.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd"
            });
            #endregion                       
        }
    }
}

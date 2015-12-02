using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_DSF
    {
        public static void CriarListaIDXML()
        {
            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqCancelamentoNFSe", new InfSchema()
            {
                Tag = "ns1:ReqCancelamentoNFSe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqCancelamentoNFSe.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "ns1:ReqCancelamentoNFSe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqConsultaNFSeRPS", new InfSchema()
            {
                Tag = "ns1:ReqConsultaNFSeRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqConsultaNFSeRPS.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqConsultaLote", new InfSchema()
            {
                Tag = "ns1:ReqConsultaLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqConsultaLote.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqConsultaNotas", new InfSchema()
            {
                Tag = "ns1:ReqConsultaNotas",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqConsultaNotas.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "ns1:ReqConsultaNotas",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ConsultaSeqRps", new InfSchema()
            {
                Tag = "ns1:ConsultaSeqRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ConsultaSeqRps.xsd",
                Descricao = "XML de Consulta da Situação do Lote RPS (Retorna número do Ultimo RPS)",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-DSF-ns1:ReqEnvioLoteRPS", new InfSchema()
            {
                Tag = "ns1:ReqEnvioLoteRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\DSF\\ReqEnvioLoteRPS.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "ns1:ReqEnvioLoteRPS",
                TagAtributoId = "Lote",
                TargetNameSpace = "http://localhost:8080/WsNFe2/lote"
            });
            #endregion
        }
    }
}

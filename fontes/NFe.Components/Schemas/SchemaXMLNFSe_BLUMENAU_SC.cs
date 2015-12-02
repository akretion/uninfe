using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_BLUMENAU_SC
    {
        public static void CriarListaIDXML()
        {
            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoConsultaNFePeriodo", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFePeriodo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoConsultaNFePeriodo_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "p1:PedidoConsultaNFePeriodo",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoConsultaNFe", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoConsultaNFe_v01.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "p1:PedidoConsultaNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoConsultaLote", new InfSchema()
            {
                Tag = "p1:PedidoConsultaLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoConsultaLote_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "p1:PedidoConsultaLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-PedidoCancelamentoNFe", new InfSchema()
            {
                Tag = "PedidoCancelamentoNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoCancelamentoNFe_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "PedidoCancelamentoNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-p1:PedidoInformacoesLote", new InfSchema()
            {
                Tag = "p1:PedidoInformacoesLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoInformacoesLote_v01.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "p1:PedidoInformacoesLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-BLUMENAU_SC-PedidoEnvioLoteRPS", new InfSchema()
            {
                Tag = "PedidoEnvioLoteRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\BLUMENAU_SC\\PedidoEnvioLoteRPS_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "PedidoEnvioLoteRPS",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://nfse.blumenau.sc.gov.br"
            });
            #endregion
        }
    }
}

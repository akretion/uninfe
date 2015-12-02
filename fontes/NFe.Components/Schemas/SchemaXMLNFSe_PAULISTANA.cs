using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_PAULISTANA
    {
        public static void CriarListaIDXML()
        {
            #region XML de lote RPS

            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-PedidoEnvioLoteRPS", new InfSchema()
            {
                Tag = "PedidoEnvioLoteRPS",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoEnvioLoteRPS_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAssinatura = "PedidoEnvioLoteRPS",
                TagLoteAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });

            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoCancelamentoNFe", new InfSchema()
            {
                Tag = "p1:PedidoCancelamentoNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoCancelamentoNFe_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "p1:PedidoCancelamentoNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"

            });

            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-PedidoCancelamentoNFe", new InfSchema()
            {
                Tag = "PedidoCancelamentoNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoCancelamentoNFe_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "PedidoCancelamentoNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"

            });

            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoConsultaLote", new InfSchema()
            {
                Tag = "p1:PedidoConsultaLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoConsultaLote_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "p1:PedidoConsultaLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });
            #endregion

            #region XML de Consulta Situação do Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoInformacoesLote", new InfSchema()
            {
                Tag = "p1:PedidoInformacoesLote",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoInformacoesLote_v01.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "p1:PedidoInformacoesLote",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });
            #endregion

            #region Consulta NFSe por período
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoConsultaNFePeriodo", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFePeriodo",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoConsultaNFePeriodo_v01.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "p1:PedidoConsultaNFePeriodo",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });
            #endregion

            #region Consulta NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-PAULISTANA-p1:PedidoConsultaNFe", new InfSchema()
            {
                Tag = "p1:PedidoConsultaNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\PAULISTANA\\PedidoConsultaNFe_v01.xsd",
                Descricao = "XML de Consulta da NFSe por período",
                TagAssinatura = "p1:PedidoConsultaNFe",
                TagAtributoId = "Cabecalho",
                TargetNameSpace = "http://www.prefeitura.sp.gov.br/nfe"
            });

            #endregion
        }
    }
}

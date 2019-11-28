using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_SIMPLISS
    {
        public static void CriarListaIDXML()
        {
            #region Consulta NFSe
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarNfseServicoPrestadoEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseServicoPrestadoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-SIMPLISS-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.sistema.com.br/Nfse/arquivos/nfse_3.xsd"
            });

            #endregion

            #region XML de Consulta de Lote RPS
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region Consulta NFSe por Faixa
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da NFSe por Faixa",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region XML de Consulta Situação do Lote RPS
            /*
            SchemaXML.InfSchemas.Add("NFSE-TECNOSISTEMAS-ConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            */
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SIMPLISS-GerarNovaNfseEnvio", new InfSchema()
            {
                Tag = "GerarNovaNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagLoteAtributoId = "InformacaoNfse",
                TagLoteAssinatura = "GerarNovaNfseEnvio",
                TargetNameSpace = "http://www.sistema.com.br/Nfse/arquivos/nfse_3.xsd"
            });
            #endregion

            #region Blumenau - SC
                      
            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-SIMPLISS-4202404-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });

            #endregion
        
            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SIMPLISS-4202404-GerarNfseEnvio", new InfSchema()
            {
                Tag = "GerarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps

            SchemaXML.InfSchemas.Add("NFSE-SIMPLISS-4202404-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "ConsultarNfseRpsEnvio",
                TagAtributoId = "IdentificacaoRps",
                TargetNameSpace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd"
            });
            #endregion XML de Consulta de NFSe por Rps

            #endregion

        }
    }
}

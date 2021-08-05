using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_PORTALFACIL_ACTCON_202
    {
        public static void CriarListaIDXML()
        {            
            #region 2.02
            #region XML de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v202.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://nfejf.portalfacil.com.br/nfseserv/schema/nfse_v202.xsd"
            });
            #endregion

            #region XML de Lote RPS (Síncrono)
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsSincronoEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v202.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://nfejf.portalfacil.com.br/nfseserv/schema/nfse_v202.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v202.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://nfejf.portalfacil.com.br/nfseserv/schema/nfse_v202.xsd"
            });
            #endregion

            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v202.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfejf.portalfacil.com.br/nfseserv/schema/nfse_v202.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Data
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-ConsultarNfseFaixaEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseFaixaEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v202.xsd",
                Descricao = "XML de Consulta de NFSe por Data",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfejf.portalfacil.com.br/nfseserv/schema/nfse_v202.xsd"
            });
            #endregion

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ACTCON\\nfse_v202.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://nfejf.portalfacil.com.br/nfseserv/schema/nfse_v202.xsd"
            });
            #endregion

            #endregion 2.02

            #region 2.04
            #region XML de Lote RPS Ipatinga - MG 2.04
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3131307-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "", //NFSe\\ACTCON\\nfse_v204.xsd
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "" //https://nfeipatinga.portalfacil.com.br/webservices/2.04/nfse_v204.xsd
            });
            #endregion

            #region XML de Cancelamento de NFS-e Ipatinga - MG 2.04
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3131307-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta de Lote RPS Ipatinga - MG 2.04 
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3131307-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta de NFSe por Rps Ipatinga - MG 2.04
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3131307-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            //---

            #region XML de Lote RPS Além Paraíba - MG 2.04
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3101508-EnviarLoteRpsSincronoEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "", //NFSe\\ACTCON\\nfse_v204.xsd
                Descricao = "XML de Lote RPS",
                TagAssinatura = "Rps",
                TagAtributoId = "InfDeclaracaoPrestacaoServico",
                TagLoteAssinatura = "EnviarLoteRpsSincronoEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "" //https://nfeipatinga.portalfacil.com.br/webservices/2.04/nfse_v204.xsd
            });
            #endregion

            #region XML de Cancelamento de NFS-e Além Paraíba - MG 2.04
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3101508-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta de Lote RPS Além Paraíba - MG 2.04 
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3101508-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #region XML de Consulta de NFSe por Rps Além Paraíba - MG 2.04
            SchemaXML.InfSchemas.Add("NFSE-PORTALFACIL_ACTCON_202-3101508-ConsultarNfseRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarNfseRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion


            #endregion 2.04


        }
    }
}

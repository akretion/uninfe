using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_EQUIPLANO
    {
        public static void CriarListaIDXML()
        {
            #region Consulta NFSe

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarNfseEnvio", new InfSchema()
            {
                Tag = "es:esConsultarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarNfseEnvio_v01.xsd",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "es:esConsultarNfseEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de Cancelamento de NFS-e

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esCancelarNfseEnvio", new InfSchema()
            {
                Tag = "es:esCancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esCancelarNfseEnvio_v01.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "es:esCancelarNfseEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de Consulta de Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "es:esConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarLoteRpsEnvio_v01.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "es:esConsultarLoteRpsEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region Consulta NFSe por RPS

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarNfsePorRpsEnvio", new InfSchema()
            {
                Tag = "es:esConsultarNfsePorRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarNfsePorRpsEnvio_v01.xsd",
                Descricao = "XML de Consulta da NFSe por RPS",
                TagAssinatura = "es:esConsultarNfsePorRpsEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de Consulta Situação do Lote RPS

            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:esConsultarSituacaoLoteRpsEnvio", new InfSchema()
            {
                Tag = "es:esConsultarSituacaoLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esConsultarSituacaoLoteRpsEnvio_v01.xsd",
                Descricao = "XML de Consulta da Situacao do Lote RPS",
                TagAssinatura = "es:esConsultarSituacaoLoteRpsEnvio",
                TagAtributoId = "prestador",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });

            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-EQUIPLANO-es:enviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "es:enviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\EQUIPLANO\\esRecepcionarLoteRpsEnvio_v01.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "es:enviarLoteRpsEnvio",
                TagAtributoId = "lote",
                TargetNameSpace = "http://www.equiplano.com.br/esnfs"
            });
            #endregion
        }
    }
}

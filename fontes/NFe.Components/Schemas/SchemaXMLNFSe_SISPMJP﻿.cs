using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.Schemas
{
    public class SchemaXMLNFSe_SISPMJP﻿
    {
        public static void CriarListaIDXML()
        {
            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-SISPMJP﻿﻿-ConsultarNfsePorRps", new InfSchema()
            {
                Tag = "ConsultarNfsePorRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SISPMJP\\nfse v2 02.xsd",
                Descricao = "XML de Consulta de NFSe por Rps",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion
                       
            #region XML de Consulta de Lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SISPMJP-ConsultarLoteRpsEnvio", new InfSchema()
            {
                Tag = "ConsultarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SISPMJP\\nfse v2 02.xsd",
                Descricao = "XML de Consulta de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SISPMJP-EnviarLoteRpsEnvio", new InfSchema()
            {
                Tag = "EnviarLoteRpsEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SISPMJP\\nfse v2 02.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TagLoteAssinatura = "EnviarLoteRpsEnvio",
                TagLoteAtributoId = "LoteRps",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-SISPMJP-CancelarNfseEnvio", new InfSchema()
            {
                Tag = "CancelarNfseEnvio",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SISPMJP\\nfse v2 02.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "Pedido",
                TagAtributoId = "InfPedidoCancelamento",
                TargetNameSpace = "http://www.abrasf.org.br/nfse.xsd"
            });
            #endregion

        }

    }
}

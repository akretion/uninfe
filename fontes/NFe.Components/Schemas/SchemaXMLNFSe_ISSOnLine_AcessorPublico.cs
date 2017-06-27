using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_ISSOnLine_AcessorPublico
    {
        public static void CriarListaIDXML()
        {
            #region APUCARANA_PR/ARACATUBA_SP/PENAPOLIS_SP

            #region XML de Consulta de NFSe por Rps
            SchemaXML.InfSchemas.Add("NFSE-ISSONLINE_ASSESSORPUBLICO-NFSE", new InfSchema()
            {
                Tag = "NFSE",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "",
                Descricao = "XML de NFSe padrão ISSOnLine",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = ""
            });
            #endregion

            #endregion
        }
    }
}

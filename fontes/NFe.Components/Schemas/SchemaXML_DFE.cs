using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NFe.Components
{
    public class SchemaXML_DFE
    {
        public static void CriarListaIDXML()
        {
            #region Distribuição de DFe´s
            SchemaXML.InfSchemas.Add("NFE-distDFeInt", new InfSchema()
            {
                Tag = "distDFeInt",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "DFe\\distDFeInt_v1.00.xsd",
                Descricao = "XML de consulta de documentos fiscais eletrônicos",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion
        }
    }
}

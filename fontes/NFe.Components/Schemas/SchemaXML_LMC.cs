using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NFe.Components
{
    public class SchemaXML_LMC
    {
        public static void CriarListaIDXML()
        {
            #region LMC
            SchemaXML.InfSchemas.Add("NFE-autorizacao", new InfSchema()
            {
                Tag = "autorizacao",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "LMC\\autorizacao_v1.00.xsd",
                Descricao = "XML do Livro de Movimentação de Combustíveis (LMC)",
                TagAssinatura = "livroCombustivel",
                TagAtributoId = "infLivroCombustivel",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_LMC
            });
            #endregion
        }
    }
}

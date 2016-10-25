using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NFe.Components
{
    public class SchemaXML_CFE
    {
        public static void CriarListaIDXML()
        {
            #region CFe
            #region XML CFe
            SchemaXML.InfSchemas.Add("NFE-CFe", new InfSchema()
            {
                Tag = "CFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                //ArquivoXSD = "CFe\\20150203_CfeRecepcaoValidador_0004.xsd",
                Descricao = "XML de Recepção do SAT",
                //TagAssinatura = "CFe",
                //TagAtributoId = "infCFe",
                //TargetNameSpace = NFeStrConstants.NAME_SPACE_CTE
            });
            #endregion
            #endregion
        }
    }
}

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
                Descricao = "XML de consulta de documentos fiscais eletrônicos (NFe)",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Distribuição de DFe´s
            SchemaXML.InfSchemas.Add("NFE-distDFeIntCTe", new InfSchema()
            {
                Tag = "distDFeInt",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "DFeCTe\\distDFeInt_v1.00.xsd",
                Descricao = "XML de consulta de documentos fiscais eletrônicos (CTe)",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://www.portalfiscal.inf.br/cte"
            });
            #endregion

            #region Distribuição de DFe´s
            SchemaXML.InfSchemas.Add("NFE-1.01-distDFeInt", new InfSchema()
            {
                Tag = "distDFeInt",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "DFe\\distDFeInt_v1.01.xsd",
                Descricao = "XML de consulta de documentos fiscais eletrônicos (NFe)",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion

            #region Distribuição de DFe´s
            SchemaXML.InfSchemas.Add("NFE-1.35-distDFeInt", new InfSchema()
            {
                Tag = "distDFeInt",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "DFe\\distDFeInt_v1.35.xsd",
                Descricao = "XML de consulta de documentos fiscais eletrônicos (NFe)",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = string.Empty
            });
            #endregion
        }
    }
}

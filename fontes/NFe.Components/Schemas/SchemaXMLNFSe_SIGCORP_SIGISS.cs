using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_SIGCORP_SIGISS
    {
        public static void CriarListaIDXML()
        {
            #region SIGCORP_SIGISS - Londrina - PR (4113700)

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-SIGCORP_SIGISS-4113700-tcDescricaoRps", new InfSchema() {
                Tag = "tcDescricaoRps",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\SIGCORP_SIGISS\\tcDescricaoRps.xsd",
                Descricao = "XML de Lote RPS",
                TargetNameSpace = "http://iss.londrina.pr.gov.br/ws/v1_03"
            });
            #endregion
            
            #endregion
        }
    }
}

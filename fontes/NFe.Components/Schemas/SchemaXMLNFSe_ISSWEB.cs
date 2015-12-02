using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NFe.Components;

namespace NFSe.Components
{
    public class SchemaXMLNFSe_ISSWEB
    {
        public static void CriarListaIDXML()
        {
            #region Consulta NFSe
            SchemaXML.InfSchemas.Add("NFSE-ISSWEB-ISSEConsultaNota", new InfSchema()
            {
                Tag = "ISSEConsultaNota",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSWEB\\XSDISSEConsultaNota.xsd",
                Descricao = "XML de Consulta da NFSe",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://186.201.198.98/issweb/webservices/XSDISSEConsultaNota.xsd"
            });
            #endregion

            #region XML de Cancelamento de NFS-e
            SchemaXML.InfSchemas.Add("NFSE-ISSWEB-ISSECancelaNFe", new InfSchema()
            {
                Tag = "ISSECancelaNFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSWEB\\XSDISSECancelaNFe.xsd",
                Descricao = "XML de Cancelamento da NFS-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = "http://186.201.198.98/issweb/webservices/XSDISSECancelaNFe.xsd"
            });
            #endregion

            #region XML de lote RPS
            SchemaXML.InfSchemas.Add("NFSE-ISSWEB-NFEEletronica", new InfSchema()
            {
                Tag = "NFEEletronica",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "NFSe\\ISSWEB\\XSDNFEletronica.xsd",
                Descricao = "XML de Lote RPS",
                TagLoteAtributoId = "",
                TagLoteAssinatura = "",
                TargetNameSpace = "http://186.201.198.98/issweb/webservices/XSDNFEletronica.xsd"
            });
            #endregion
        }
    }
}

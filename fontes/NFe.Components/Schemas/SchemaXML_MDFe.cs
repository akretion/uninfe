using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NFe.Components
{
    public class SchemaXML_MDFe
    {
        public static void CriarListaIDXML()
        {
            #region MDF-e

            #region XML Envio do MDFe
            SchemaXML.InfSchemas.Add("NFE-MDFe", new InfSchema()
            {
                Tag = "MDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\mdfe_v1.00.xsd",
                Descricao = "XML do Manifesto Eletrônico de Documentos Fiscais",
                TagAssinatura = "MDFe",
                TagAtributoId = "infMDFe",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML Envio Lote MDFe
            SchemaXML.InfSchemas.Add("NFE-enviMDFe", new InfSchema()
            {
                Tag = "enviMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\enviMDFe_v1.00.xsd",
                Descricao = "XML de Envio de Lote de Manifesto Eletrônico de Docimentos Fiscais",
                TagAssinatura = string.Empty,
                TagAtributoId = string.Empty,
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de consulta recibo do MDFe
            SchemaXML.InfSchemas.Add("NFE-consReciMDFe", new InfSchema()
            {
                Tag = "consReciMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consReciMDFe_v1.00.xsd",
                Descricao = "XML de consulta recibo MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de consulta situação do MDFe
            SchemaXML.InfSchemas.Add("NFE-consSitMDFe", new InfSchema()
            {
                Tag = "consSitMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consSitMDFe_v1.00.xsd",
                Descricao = "XML de consulta situação do MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de consulta status dos serviços do MDFe
            SchemaXML.InfSchemas.Add("NFE-consStatServMDFe", new InfSchema()
            {
                Tag = "consStatServMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consStatServMDFe_v1.00.xsd",
                Descricao = "XML de consulta status do serviço do MDF-e",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Evento de Cancelamento do MDFe
            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110111", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de cancelamento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Evento de Encerramento do MDFe
            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110112", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de encerramento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Evento de Inclusão de Condutor
            SchemaXML.InfSchemas.Add("NFE-eventoMDFe110114", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de encerramento do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Eventos Gerais do MDFe
            SchemaXML.InfSchemas.Add("NFE-eventoMDFe310620", new InfSchema()
            {
                Tag = "eventoMDFe",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\eventoMDFe_v1.00.xsd",
                Descricao = "XML de evento de registro de passagem do MDF-e",
                TagAssinatura = "eventoMDFe",
                TagAtributoId = "infEvento",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #region XML de Eventos Gerais do MDFe
            SchemaXML.InfSchemas.Add("NFE-consMDFeNaoEnc", new InfSchema()
            {
                Tag = "consMDFeNaoEnc",
                ID = SchemaXML.InfSchemas.Count + 1,
                ArquivoXSD = "MDFe\\consMDFeNaoEnc_v1.00.xsd",
                Descricao = "Pedido de Consulta MDF-e não encerrados",
                TagAssinatura = "",
                TagAtributoId = "",
                TargetNameSpace = NFeStrConstants.NAME_SPACE_MDFE
            });
            #endregion

            #endregion
        }
    }
}

using System;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Security;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class RecepcaoEvento : ServicoBase
    {
        private RecepcaoEvento(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            EnvEvento xml = new EnvEvento();
            xml = xml.LerXML<EnvEvento>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.CodigoUF = (int)xml.Evento[0].InfEvento.COrgao;
                Configuracoes.TipoAmbiente = xml.Evento[0].InfEvento.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        //public RetEnviNFe Result
        //{
        //    get
        //    {
        //        if (!string.IsNullOrWhiteSpace(RetornoWSString))
        //        {
        //            return XMLUtility.Deserializar<RetEnviNFe>(RetornoWSXML);
        //        }

        //        return new RetEnviNFe
        //        {
        //            CStat = 0,
        //            XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
        //        };
        //    }
        //}

        public RecepcaoEvento(EnvEvento envEvento, Configuracao configuracao)
                            : this(envEvento.GerarXML(), configuracao)
        {
        }

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            new AssinaturaDigital().Assinar(ConteudoXML, Configuracoes.TagAssinatura, Configuracoes.TagAtributoID, Configuracoes.CertificadoDigital, AlgorithmType.Sha1, true, "", "Id");

            base.Executar();
        }

        protected override void XmlValidar()
        {
            EnvEvento xml = new EnvEvento();
            xml = xml.LerXML<EnvEvento>(ConteudoXML);

            string schemaArquivo = string.Empty;
            string schemaArquivoEspecifico = string.Empty;

            foreach (SchemaEspecifico item in Configuracoes.SchemasEspecificos)
            {
                if (((int)xml.Evento[0].InfEvento.TpEvento).ToString() == item.Id)
                {
                    schemaArquivo = item.SchemaArquivo;
                    schemaArquivoEspecifico = item.SchemaArquivoEspecifico;
                    break;
                }
            }

            XmlDocument xmlEspecifico = new XmlDocument();
            xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEvento>(xml.Evento[0].InfEvento.DetEvento).OuterXml);
            
            ValidarXMLEvento(ConteudoXML, schemaArquivo, Configuracoes.TargetNS);
            ValidarXMLEvento(xmlEspecifico, schemaArquivoEspecifico, Configuracoes.TargetNS);
        }

        private void ValidarXMLEvento(XmlDocument xml, string schemaArquivo, string targetNS)
        {
            ValidarSchema validar = new ValidarSchema();
            validar.Validar(xml, Path.Combine(Configuracoes.SchemaPasta, schemaArquivo), targetNS);

            if (!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }
    }
}
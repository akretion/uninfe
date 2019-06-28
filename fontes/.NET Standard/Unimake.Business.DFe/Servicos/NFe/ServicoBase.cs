using System;
using System.IO;
using System.Xml;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public abstract class ServicoBase : Servicos.ServicoBase
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML que será enviado para o WebService</param>
        /// <param name="configuracao">Objeto "Configuracoes" com as propriedade necessária para a execução do serviço</param>
        public ServicoBase(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            XmlValidar();

            WSSoap soap = new WSSoap
            {
                EnderecoWeb = (Configuracoes.TipoAmbiente == 1 ? Configuracoes.WebEnderecoProducao : Configuracoes.WebEnderecoHomologacao),
                ActionWeb = (Configuracoes.TipoAmbiente == 1 ? Configuracoes.WebActionProducao : Configuracoes.WebActionHomologacao),
                TagRetorno = Configuracoes.WebTagRetorno,
                VersaoSoap = Configuracoes.WebSoapVersion,
                SoapString = Configuracoes.WebSoapString,
                ContentType = Configuracoes.WebContentType
            };

            ConsumirWS consumirWS = new ConsumirWS();
            consumirWS.ExecutarServico(ConteudoXML, soap, Configuracoes.CertificadoDigital);

            RetornoWSString = consumirWS.RetornoServicoString;
            RetornoWSXML = consumirWS.RetornoServicoXML;
        }

        /// <summary>
        /// Validar o XML
        /// </summary>
        protected override void XmlValidar()
        {
            ValidarSchema validar = new ValidarSchema();
            validar.Validar(ConteudoXML, Path.Combine(Configuracoes.SchemaPasta, Configuracoes.SchemaArquivo), Configuracoes.TargetNS);

            if (!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }

        /// <summary>
        /// Definir configurações
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            //Definir a pasta onde fica o schema do XML
            Configuracoes.SchemaPasta = @"Xml\NFe\Schemas\";
        }
    }
}
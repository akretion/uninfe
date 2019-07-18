using System;
using System.IO;
using System.Xml;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public abstract class ServicoBase : Servicos.ServicoBase
    {
        #region Protected Methods

        /// <summary>
        /// Definir configurações
        /// </summary>
        protected override void DefinirConfiguracao() =>
            //Definir a pasta onde fica o schema do XML
            Configuracoes.SchemaPasta = ConfigurationManager.CurrentConfig.SchemaPasta;

        /// <summary>
        /// Validar o XML
        /// </summary>
        protected override void XmlValidar()
        {
            var validar = new ValidarSchema();
            validar.Validar(ConteudoXML, Path.Combine(Configuracoes.SchemaPasta, Configuracoes.SchemaArquivo), Configuracoes.TargetNS);

            if (!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }

        #endregion Protected Methods

        #region Public Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML que será enviado para o WebService</param>
        /// <param name="configuracao">Objeto "Configuracoes" com as propriedade necessária para a execução do serviço</param>
        public ServicoBase(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            XmlValidar();

            var soap = new WSSoap
            {
                EnderecoWeb = (Configuracoes.TipoAmbiente == TipoAmbiente.Producao ? Configuracoes.WebEnderecoProducao : Configuracoes.WebEnderecoHomologacao),
                ActionWeb = (Configuracoes.TipoAmbiente == TipoAmbiente.Producao ? Configuracoes.WebActionProducao : Configuracoes.WebActionHomologacao),
                TagRetorno = Configuracoes.WebTagRetorno,
                VersaoSoap = Configuracoes.WebSoapVersion,
                SoapString = Configuracoes.WebSoapString,
                ContentType = Configuracoes.WebContentType
            };

            var consumirWS = new ConsumirWS();
            consumirWS.ExecutarServico(ConteudoXML, soap, Configuracoes.CertificadoDigital);

            RetornoWSString = consumirWS.RetornoServicoString;
            RetornoWSXML = consumirWS.RetornoServicoXML;
        }

        #endregion Public Methods
    }
}
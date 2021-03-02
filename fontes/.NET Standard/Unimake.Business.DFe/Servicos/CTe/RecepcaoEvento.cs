using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    /// <summary>
    /// Envio do XML de eventos do CTe para o WebService
    /// </summary>
    [ComVisible(true)]
    public class RecepcaoEvento<TDetalheEvento>: ServicoBase
    {
        #region Private Fields
        private EventoCTe<TDetalheEvento> EventoCTe => new EventoCTe<TDetalheEvento>().LerXML<EventoCTe<TDetalheEvento>>(ConteudoXML);

        #endregion Private Fields

        #region Private Constructors

        private RecepcaoEvento(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        #endregion Private Constructors

        #region Private Methods

        private void ValidarXMLEvento(XmlDocument xml, string schemaArquivo, string targetNS)
        {
            var validar = new ValidarSchema();
            validar.Validar(xml, Configuracoes.TipoDFe.ToString() + "." + schemaArquivo, targetNS);

            if(!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new EventoCTe<TDetalheEvento>();
            xml = xml.LerXML<EventoCTe<TDetalheEvento>>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.CodigoUF = (int)((IInfEvento)xml.InfEvento).COrgao;
                Configuracoes.TipoAmbiente = ((IInfEvento)xml.InfEvento).TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        /// <summary>
        /// Validar o XML
        /// </summary>
        protected override void XmlValidar()
        {
            var xml = EventoCTe;

            var schemaArquivo = string.Empty;
            var schemaArquivoEspecifico = string.Empty;

            if(Configuracoes.SchemasEspecificos.Count > 0)
            {
                var tpEvento = ((int)((IInfEvento)xml.InfEvento).TpEvento);

                try
                {
                    schemaArquivo = Configuracoes.SchemasEspecificos[tpEvento.ToString()].SchemaArquivo;
                    schemaArquivoEspecifico = Configuracoes.SchemasEspecificos[tpEvento.ToString()].SchemaArquivoEspecifico;
                }
                catch
                {
                    throw new Exception("Não foi possível localizar no arquivo de configuração a definição do schema do modal específico do evento " + tpEvento.ToString() + ".");
                }
            }

            #region Validar o XML geral

            ValidarXMLEvento(ConteudoXML, schemaArquivo, Configuracoes.TargetNS);

            #endregion Validar o XML geral

            #region Validar a parte específica de cada evento

            if(ConteudoXML.GetElementsByTagName("detEvento")[0] != null)
            {
                var xmlEspecifico = new XmlDocument();
                xmlEspecifico.LoadXml(ConteudoXML.GetElementsByTagName(ConteudoXML.GetElementsByTagName("detEvento")[0].FirstChild.Name)[0].OuterXml);
                ValidarXMLEvento(xmlEspecifico, schemaArquivoEspecifico, Configuracoes.TargetNS);
            }
            else
            {
                throw new Exception("Não foi possível localizar o detalhamento do evento. Tag (detEvento).");
            }

            #endregion Validar a parte específica de cada evento
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Propriedade contendo o XML do evento com o protocolo de autorização anexado
        /// </summary>
        public ProcEventoCTe<TDetalheEvento> ProcEventoCTeResult => new ProcEventoCTe<TDetalheEvento>
        {
            Versao = EventoCTe.Versao,
            EventoCTe = EventoCTe,
            RetEventoCTe = Result
        };

        /// <summary>
        /// Conteúdo retornado pelo webservice depois do envio do XML
        /// </summary>
        public RetEventoCTe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetEventoCTe>(RetornoWSXML);
                }

                return new RetEventoCTe
                {
                    InfEvento = new RetEventoCTeInfEvento
                    {
                        CStat = 0,
                        XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                    }
                };

            }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="envEvento">Objeto contendo o XML a ser enviado</param>
        /// <param name="configuracao">Configurações para conexão e envio do XML para o webservice</param>
        public RecepcaoEvento(EventoCTe<TDetalheEvento> envEvento, Configuracao configuracao)
            : this(envEvento?.GerarXML() ?? throw new ArgumentNullException(nameof(envEvento)), configuracao) { }

        /// <summary>
        /// Construtor
        /// </summary>
        public RecepcaoEvento()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executar o serviço
        /// </summary>
        [ComVisible(false)]
        public override void Executar() => base.Executar();

        /// <summary>
        /// Executa o serviço: Assina o XML, valida e envia para o webservice
        /// </summary>
        /// <param name="envEvento">Objeto contendo o XML a ser enviado</param>
        /// <param name="configuracao">Configurações a serem utilizadas na conexão e envio do XML para o webservice</param>
        [ComVisible(true)]
        public void Executar(EventoCTe<TDetalheEvento> envEvento, Configuracao configuracao)
        {
            if(envEvento == null)
            {
                throw new ArgumentNullException(nameof(envEvento));
            }

            PrepararServico(envEvento.GerarXML(), configuracao);

            Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta) => GravarXmlDistribuicao(pasta, ProcEventoCTeResult.NomeArquivoDistribuicao, ProcEventoCTeResult.GerarXML().OuterXml);

        /// <summary>
        /// Grava o XML de dsitribuição no stream
        /// </summary>
        /// <param name="stream">Stream que vai receber o XML de distribuição</param>
        public void GravarXmlDistribuicao(Stream stream) => GravarXmlDistribuicao(stream, ProcEventoCTeResult.GerarXML().OuterXml);

        #endregion Public Methods
    }
}
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    [ComVisible(true)]
    public class RecepcaoEvento : ServicoBase
    {
        #region Private Fields
        private EventoCTe EventoCTe
        {
            get
            {
                return new EventoCTe().LerXML<EventoCTe>(ConteudoXML);
            }
        }

        #endregion Private Fields

        #region Private Constructors

        private RecepcaoEvento(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        #endregion Private Constructors

        #region Private Methods

        private void ValidarXMLEvento(XmlDocument xml, string schemaArquivo, string targetNS)
        {
            var validar = new ValidarSchema();
            validar.Validar(xml, Path.Combine(Configuracoes.SchemaPasta, schemaArquivo), targetNS);

            if (!validar.Success)
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
            var xml = new EventoCTe();
            xml = xml.LerXML<EventoCTe>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.CodigoUF = (int)xml.InfEvento.COrgao;
                Configuracoes.TipoAmbiente = xml.InfEvento.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        protected override void XmlValidar()
        {
            var xml = EventoCTe;

            var schemaArquivo = string.Empty;
            var schemaArquivoEspecifico = string.Empty;

            if (Configuracoes.SchemasEspecificos.Count > 0)
            {
                var tpEvento = ((int)xml.InfEvento.TpEvento);

                schemaArquivo = Configuracoes.SchemasEspecificos[tpEvento.ToString()].SchemaArquivo;
                schemaArquivoEspecifico = Configuracoes.SchemasEspecificos[tpEvento.ToString()].SchemaArquivoEspecifico;
            }

            #region Validar o XML geral

            ValidarXMLEvento(ConteudoXML, schemaArquivo, Configuracoes.TargetNS);

            #endregion Validar o XML geral

            #region Validar a parte específica de cada evento

            var listEvento = ConteudoXML.GetElementsByTagName("eventoCTe");
            for (var i = 0; i < listEvento.Count; i++)
            {
                var elementEvento = (XmlElement)listEvento[i];

                if (elementEvento.GetElementsByTagName("infEvento")[0] != null)
                {
                    var elementInfEvento = (XmlElement)elementEvento.GetElementsByTagName("infEvento")[0];
                    if (elementInfEvento.GetElementsByTagName("tpEvento")[0] != null)
                    {
                        var tpEvento = elementInfEvento.GetElementsByTagName("tpEvento")[0].InnerText;

                        var tipoEventoCTe = (TipoEventoCTe)Enum.Parse(typeof(TipoEventoCTe), tpEvento);

                        var xmlEspecifico = new XmlDocument();
                        switch (tipoEventoCTe)
                        {
                            //case TipoEventoCTe.CartaCorrecao:
                            //    xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoCCE>((DetEventoCCE)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                            //    break;

                            case TipoEventoCTe.Cancelamento:
                                xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoCanc>((DetEventoCanc)xml.InfEvento.DetEvento).OuterXml);
                                break;

                            //case TipoEventoCTe.CancelamentoPorSubstituicao:
                            //    break;

                            //case TipoEventoCTe.EPEC:
                            //    break;

                            //case TipoEventoCTe.PedidoProrrogacao:
                            //    break;

                            //case TipoEventoCTe.ManifestacaoConfirmacaoOperacao:
                            //case TipoEventoCTe.ManifestacaoCienciaOperacao:
                            //case TipoEventoCTe.ManifestacaoDesconhecimentoOperacao:
                            //case TipoEventoCTe.ManifestacaoOperacaoNaoRealizada:
                            //    xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoManif>((DetEventoManif)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                            //    break;

                            default:
                                throw new Exception("Não foi possível identificar o tipo de evento.");
                        }

                        ValidarXMLEvento(xmlEspecifico, schemaArquivoEspecifico, Configuracoes.TargetNS);
                    }
                }
            }

            #endregion Validar a parte específica de cada evento
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Propriedade contendo o XML do evento com o protocolo de autorização anexado
        /// </summary>
        public ProcEventoCTe ProcEventoCTeResult => new ProcEventoCTe
        {
            Versao = EventoCTe.Versao,
            EventoCTe = EventoCTe,
            RetEventoCTe = Result
        };

        public RetEventoCTe Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
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

        public RecepcaoEvento(EventoCTe envEvento, Configuracao configuracao)
            : this(envEvento?.GerarXML() ?? throw new ArgumentNullException(nameof(envEvento)), configuracao) { }

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

        [ComVisible(true)]
        public void Executar(EventoCTe envEvento, Configuracao configuracao)
        {
            if (envEvento == null)
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
        public void GravarXmlDistribuicao(string pasta)
        {
            GravarXmlDistribuicao(pasta, ProcEventoCTeResult.NomeArquivoDistribuicao, ProcEventoCTeResult.GerarXML().OuterXml);
        }

        /// <summary>
        /// Grava o XML de dsitribuição no stream
        /// </summary>
        /// <param name="stream">Stream que vai receber o XML de distribuição</param>
        public void GravarXmlDistribuicao(Stream stream)
        {
            GravarXmlDistribuicao(stream, ProcEventoCTeResult.GerarXML().OuterXml);
        }

        #endregion Public Methods
    }
}
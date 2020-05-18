using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class RecepcaoEvento: ServicoBase, IInteropService<EnvEvento>
    {
        #region Private Properties

        private EnvEvento EnvEvento => new EnvEvento().LerXML<EnvEvento>(ConteudoXML);

        #endregion Private Properties

        #region Private Methods

        private void ValidarXMLEvento(XmlDocument xml, string schemaArquivo, string targetNS)
        {
            var validar = new ValidarSchema();
            validar.Validar(xml, Path.Combine(Configuracoes.SchemaPasta, schemaArquivo), targetNS);

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
            var xml = new EnvEvento();
            xml = xml.LerXML<EnvEvento>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.CodigoUF = (int)xml.Evento[0].InfEvento.COrgao;
                Configuracoes.TipoAmbiente = xml.Evento[0].InfEvento.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        protected override void XmlValidar()
        {
            var xml = EnvEvento;

            var schemaArquivo = string.Empty;
            var schemaArquivoEspecifico = string.Empty;

            if(Configuracoes.SchemasEspecificos.Count > 0)
            {
                var tpEvento = ((int)xml.Evento[0].InfEvento.TpEvento);

                schemaArquivo = Configuracoes.SchemasEspecificos[tpEvento.ToString()].SchemaArquivo;
                schemaArquivoEspecifico = Configuracoes.SchemasEspecificos[tpEvento.ToString()].SchemaArquivoEspecifico;
            }

            #region Validar o XML geral

            ValidarXMLEvento(ConteudoXML, schemaArquivo, Configuracoes.TargetNS);

            #endregion Validar o XML geral

            #region Validar a parte específica de cada evento

            var listEvento = ConteudoXML.GetElementsByTagName("evento");
            for(var i = 0; i < listEvento.Count; i++)
            {
                var elementEvento = (XmlElement)listEvento[i];

                if(elementEvento.GetElementsByTagName("infEvento")[0] != null)
                {
                    var elementInfEvento = (XmlElement)elementEvento.GetElementsByTagName("infEvento")[0];
                    if(elementInfEvento.GetElementsByTagName("tpEvento")[0] != null)
                    {
                        var tpEvento = elementInfEvento.GetElementsByTagName("tpEvento")[0].InnerText;

                        var tipoEventoNFe = (TipoEventoNFe)Enum.Parse(typeof(TipoEventoNFe), tpEvento);

                        var xmlEspecifico = new XmlDocument();
                        switch(tipoEventoNFe)
                        {
                            case TipoEventoNFe.CartaCorrecao:
                                xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoCCE>((DetEventoCCE)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                                break;

                            case TipoEventoNFe.Cancelamento:
                                xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoCanc>((DetEventoCanc)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                                break;

                            case TipoEventoNFe.CancelamentoPorSubstituicao:
                                xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoCancSubst>((DetEventoCancSubst)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                                break;

                            case TipoEventoNFe.EPEC:
                                break;

                            case TipoEventoNFe.PedidoProrrogacao:
                                break;

                            case TipoEventoNFe.ManifestacaoConfirmacaoOperacao:
                            case TipoEventoNFe.ManifestacaoCienciaOperacao:
                            case TipoEventoNFe.ManifestacaoDesconhecimentoOperacao:
                            case TipoEventoNFe.ManifestacaoOperacaoNaoRealizada:
                                xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoManif>((DetEventoManif)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                                break;

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
        public ProcEventoNFe[] ProcEventoNFeResult
        {
            get
            {
                var retorno = new ProcEventoNFe[EnvEvento.Evento.Count];

                for(var i = 0; i < EnvEvento.Evento.Count; i++)
                {
                    retorno[i] = new ProcEventoNFe
                    {
                        Versao = EnvEvento.Versao,
                        Evento = EnvEvento.Evento[i],
                        RetEvento = Result.RetEvento[i]
                    };
                };

                return retorno;
            }
        }

        public RetEnvEvento Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetEnvEvento>(RetornoWSXML);
                }

                return new RetEnvEvento
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public RecepcaoEvento(EnvEvento envEvento, Configuracao configuracao)
            : base(envEvento?.GerarXML() ?? throw new ArgumentNullException(nameof(envEvento)), configuracao) { }

        public RecepcaoEvento()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(true)]
        public void Executar(EnvEvento envEvento, Configuracao configuracao)
        {
            PrepararServico(envEvento?.GerarXML() ?? throw new ArgumentNullException(nameof(envEvento)), configuracao);
            Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            for(var i = 0; i < Result.RetEvento.Count; i++)
            {
                GravarXmlDistribuicao(pasta, ProcEventoNFeResult[i].NomeArquivoDistribuicao, ProcEventoNFeResult[i].GerarXML().OuterXml);
            }
        }

        /// <summary>
        /// Grava o XML de dsitribuição no stream
        /// </summary>
        /// <param name="stream">Stream que vai receber o XML de distribuição</param>
        public void GravarXmlDistribuicao(Stream stream)
        {
            for(var i = 0; i < Result.RetEvento.Count; i++)
            {
                GravarXmlDistribuicao(stream, ProcEventoNFeResult[i].GerarXML().OuterXml);
            }
        }

        #endregion Public Methods
    }
}
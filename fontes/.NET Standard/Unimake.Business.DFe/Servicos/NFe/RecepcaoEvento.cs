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
        private EnvEvento EnvEvento;

        private RecepcaoEvento(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new EnvEvento();
            xml = xml.LerXML<EnvEvento>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.CodigoUF = (int)xml.Evento[0].InfEvento.COrgao;
                Configuracoes.TipoAmbiente = xml.Evento[0].InfEvento.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        public RetEnvEvento Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
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

        /// <summary>
        /// Propriedade contendo o XML do evento com o protocolo de autorização anexado
        /// </summary>
        public ProcEventoNFe ProcEventoNFeResult => new ProcEventoNFe
        {
            //TODO: WANDREY - Tem que gerar o XML de distribuição de todos os eventos enviados, pois pode ter mais de um, por enquanto, estou entendendo que está enviando somente 1 por XML
            Versao = EnvEvento.Versao,
            Evento = EnvEvento.Evento[0],
            RetEvento = Result.RetEvento[0]
        };

        public RecepcaoEvento(EnvEvento envEvento, Configuracao configuracao)
                            : this(envEvento.GerarXML(), configuracao)
        {
            EnvEvento = envEvento;
        }

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            new AssinaturaDigital().Assinar(ConteudoXML, Configuracoes.TagAssinatura, Configuracoes.TagAtributoID, Configuracoes.CertificadoDigital, AlgorithmType.Sha1, true, "", "Id");
            EnvEvento = EnvEvento.LerXML<EnvEvento>(ConteudoXML);

            base.Executar();
        }

        protected override void XmlValidar()
        {
            var xml = EnvEvento;

            var schemaArquivo = string.Empty;
            var schemaArquivoEspecifico = string.Empty;

            foreach (var item in Configuracoes.SchemasEspecificos)
            {
                if (((int)xml.Evento[0].InfEvento.TpEvento).ToString() == item.Id)
                {
                    schemaArquivo = item.SchemaArquivo;
                    schemaArquivoEspecifico = item.SchemaArquivoEspecifico;
                    break;
                }
            }

            #region Validar o XML geral

            ValidarXMLEvento(ConteudoXML, schemaArquivo, Configuracoes.TargetNS);

            #endregion

            #region Validar a parte específica de cada evento

            var listEvento = ConteudoXML.GetElementsByTagName("evento");
            for (var i = 0; i < listEvento.Count; i++)
            {
                var elementEvento = (XmlElement)listEvento[i];

                if (elementEvento.GetElementsByTagName("infEvento")[0] != null)
                {
                    var elementInfEvento = (XmlElement)elementEvento.GetElementsByTagName("infEvento")[0];
                    if (elementInfEvento.GetElementsByTagName("tpEvento")[0] != null)
                    {
                        var tpEvento = elementInfEvento.GetElementsByTagName("tpEvento")[0].InnerText;

                        var tipoEventoNFe = (TipoEventoNFe)Enum.Parse(typeof(TipoEventoNFe), tpEvento);

                        var xmlEspecifico = new XmlDocument();
                        switch (tipoEventoNFe)
                        {
                            case TipoEventoNFe.CartaCorrecao:
                                xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoCCE>((DetEventoCCE)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                                break;

                            case TipoEventoNFe.Cancelamento:
                                xmlEspecifico.LoadXml(XMLUtility.Serializar<DetEventoCanc>((DetEventoCanc)xml.Evento[i].InfEvento.DetEvento).OuterXml);
                                break;

                            case TipoEventoNFe.CancelamentoPorSubstituicao:
                                break;

                            case TipoEventoNFe.EPEC:
                                break;

                            case TipoEventoNFe.PedidoProrrogacao:
                                break;

                            case TipoEventoNFe.ManifestacaoConfirmacaoOperacao:
                                break;

                            case TipoEventoNFe.ManifestacaoCienciaOperacao:
                                break;

                            case TipoEventoNFe.ManifestacaoDesconhecimentoOperacao:
                                break;

                            case TipoEventoNFe.ManifestacaoOperacaoNaoRealizada:
                                break;

                            default:
                                throw new Exception("Não foi possível identificar o tipo de evento.");
                        }

                        ValidarXMLEvento(xmlEspecifico, schemaArquivoEspecifico, Configuracoes.TargetNS);
                    }
                }
            }

            #endregion
        }

        private void ValidarXMLEvento(XmlDocument xml, string schemaArquivo, string targetNS)
        {
            var validar = new ValidarSchema();
            validar.Validar(xml, Path.Combine(Configuracoes.SchemaPasta, schemaArquivo), targetNS);

            if (!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            GravarXmlDistribuicao(pasta, ProcEventoNFeResult.NomeArquivoDistribuicao, ProcEventoNFeResult.GerarXML().OuterXml);
        }
    }
}
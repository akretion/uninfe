using System;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.GNRE;

namespace Unimake.Business.DFe.Servicos.GNRE
{
    /// <summary>
    /// Envia XML de consulta Processamento de Lote de GNRE para WebService
    /// </summary>
    public class ConsultaResultadoLote: ServicoBase, IInteropService<TConsLoteGNRE>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new TConsLoteGNRE();
            xml = xml.LerXML<TConsLoteGNRE>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.GNREConsultaResultadoLote;
                Configuracoes.TipoAmbiente = xml.Ambiente;
                Configuracoes.SchemaVersao = "1.00";

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Contem o resultado da consulta do lote
        /// </summary>
        public TResultLoteGNRE Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<TResultLoteGNRE>(RetornoWSXML);
                }

                return new TResultLoteGNRE
                {
                    SituacaoProcess = new SituacaoProcess
                    {
                        Codigo = "0",
                        Descricao = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado do WebService."
                    }
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        public ConsultaResultadoLote()
            : base()
        {
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="tConsLoteGNRE">Objeto contendo o XML da consulta do lote da GNRE</param>
        /// <param name="configuracao">Objeto contendo as configurações a serem utilizadas na consulta do lote da GNRE</param>
        public ConsultaResultadoLote(TConsLoteGNRE tConsLoteGNRE, Configuracao configuracao)
                    : base(tConsLoteGNRE?.GerarXML() ?? throw new ArgumentNullException(nameof(tConsLoteGNRE)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executa o envio da consulta do lote da GNRE
        /// </summary>
        /// <param name="tConsLoteGNRE">Objeto contendo o XML da consulta do lote da GNRE</param>
        /// <param name="configuracao">Objeto contendo as configurações a serem utilizadas na consulta do lote da GNRE</param>
        [ComVisible(true)]
        public void Executar(TConsLoteGNRE tConsLoteGNRE, Configuracao configuracao)
        {
            PrepararServico(tConsLoteGNRE?.GerarXML() ?? throw new ArgumentNullException(nameof(tConsLoteGNRE)), configuracao);
            Executar();
        }

        /// <summary>
        /// Gravar o XML de distrubição da GNRE
        /// </summary>
        /// <param name="pasta">Pasta onde será gravado o XML de distribuição</param>
        /// <param name="nomeArquivo">Nome do arquivo de distribuição que será gravado</param>
        /// <param name="conteudoXML">Conteúdo do XML de distribuição a ser gravado</param>
        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta status do serviço.");

        #endregion Public Methods
    }
}
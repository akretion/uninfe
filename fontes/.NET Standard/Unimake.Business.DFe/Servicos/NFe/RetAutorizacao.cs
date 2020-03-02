using System;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class RetAutorizacao: ServicoBase, IInteropService<ConsReciNFe>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsReciNFe();
            xml = xml.LerXML<ConsReciNFe>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeConsultaRecibo;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;
                Configuracoes.CodigoUF = Convert.ToInt32(xml.NRec.Substring(0, 2));
                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsReciNFe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsReciNFe>(RetornoWSXML);
                }

                return new RetConsReciNFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public RetAutorizacao()
        {
        }

        public RetAutorizacao(ConsReciNFe consReciNFe, Configuracao configuracao)
            : base(consReciNFe?.GerarXML() ?? throw new ArgumentNullException(nameof(consReciNFe)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(true)]
        public void Executar(ConsReciNFe consReciNFe, Configuracao configuracao)
        {
            PrepararServico(consReciNFe?.GerarXML() ?? throw new ArgumentNullException(nameof(consReciNFe)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) =>
            throw new Exception("Não existe XML de distribuição para consulta do recibo de lote.");

        #endregion Public Methods
    }
}
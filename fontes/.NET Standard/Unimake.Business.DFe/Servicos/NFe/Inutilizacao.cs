using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class Inutilizacao: ServicoBase, IInteropService<InutNFe>
    {
        #region Private Properties

        private InutNFe InutNFe => new InutNFe().LerXML<InutNFe>(ConteudoXML);

        #endregion Private Properties

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new InutNFe();
            xml = xml.LerXML<InutNFe>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeInutilizacao;
                Configuracoes.CodigoUF = (int)xml.InfInut.CUF;
                Configuracoes.TipoAmbiente = xml.InfInut.TpAmb;
                Configuracoes.Modelo = xml.InfInut.Mod;
                Configuracoes.TipoEmissao = TipoEmissao.Normal; //Inutilização só funciona no tipo de emissão Normal, ou seja, não tem inutilização em SVC-AN ou SVC-RS
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Propriedade contendo o XML da inutilização com o protocolo de autorização anexado
        /// </summary>
        public ProcInutNFe ProcInutNFeResult => new ProcInutNFe
        {
            Versao = InutNFe.Versao,
            InutNFe = InutNFe,
            RetInutNFe = Result
        };

        public RetInutNFe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetInutNFe>(RetornoWSXML);
                }

                return new RetInutNFe
                {
                    InfInut = new InfInut
                    {
                        CStat = 0,
                        XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                    }
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public Inutilizacao(InutNFe inutNFe, Configuracao configuracao)
                    : base(inutNFe?.GerarXML() ?? throw new System.ArgumentNullException(nameof(inutNFe)), configuracao) { }

        public Inutilizacao()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(true)]
        public void Executar(InutNFe inutNFe, Configuracao configuracao)
        {
            PrepararServico(inutNFe?.GerarXML() ?? throw new System.ArgumentNullException(nameof(inutNFe)), configuracao);
            Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta) => GravarXmlDistribuicao(pasta, ProcInutNFeResult.NomeArquivoDistribuicao, ProcInutNFeResult.GerarXML().OuterXml);

        /// <summary>
        /// Grava o XML de dsitribuição no stream
        /// </summary>
        /// <param name="stream">Stream que vai receber o XML de distribuição</param>
        public void GravarXmlDistribuicao(System.IO.Stream stream) => GravarXmlDistribuicao(stream, ProcInutNFeResult.GerarXML().OuterXml);

        #endregion Public Methods
    }
}
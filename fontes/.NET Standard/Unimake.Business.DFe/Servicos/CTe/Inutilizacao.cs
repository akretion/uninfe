using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    public class Inutilizacao: ServicoBase, IInteropService<InutCTe>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new InutCTe();
            xml = xml.LerXML<InutCTe>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.CTeInutilizacao;
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
        public ProcInutCTe ProcInutCTeResult
        {
            get
            {
                var InutCTe = new InutCTe().LerXML<InutCTe>(ConteudoXML);

                var result = new ProcInutCTe
                {
                    Versao = InutCTe.Versao,
                    InutCTe = InutCTe,
                    RetInutCTe = Result
                };

                return result;
            }
        }

        public RetInutCTe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetInutCTe>(RetornoWSXML);
                }

                return new RetInutCTe
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

        public Inutilizacao()
        {
        }

        public Inutilizacao(InutCTe inutCTe, Configuracao configuracao)
                    : base(inutCTe?.GerarXML() ?? throw new System.ArgumentNullException(nameof(inutCTe)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        public void Executar(InutCTe inutCTe, Configuracao configuracao)
        {
            PrepararServico(inutCTe?.GerarXML() ?? throw new System.ArgumentNullException(nameof(inutCTe)), configuracao);
            Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta) => GravarXmlDistribuicao(pasta, ProcInutCTeResult.NomeArquivoDistribuicao, ProcInutCTeResult.GerarXML().OuterXml);

        #endregion Public Methods
    }
}
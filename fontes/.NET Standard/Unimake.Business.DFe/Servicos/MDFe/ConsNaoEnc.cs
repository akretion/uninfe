using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.MDFe;

namespace Unimake.Business.DFe.Servicos.MDFe
{
    [ComVisible(true)]
    public class ConsNaoEnc : ServicoBase, IInteropService<ConsMDFeNaoEnc>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsMDFeNaoEnc();
            xml = xml.LerXML<ConsMDFeNaoEnc>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.MDFeConsultaNaoEnc;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsMDFeNaoEnc Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsMDFeNaoEnc>(RetornoWSXML);
                }

                return new RetConsMDFeNaoEnc
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public ConsNaoEnc()
            : base() { }

        public ConsNaoEnc(ConsMDFeNaoEnc consMDFeNaoEnc, Configuracao configuracao)
            : base(consMDFeNaoEnc?.GerarXML() ?? throw new System.ArgumentNullException(nameof(consMDFeNaoEnc)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        public void Executar(ConsMDFeNaoEnc consMDFeNaoEnc, Configuracao configuracao)
        {
            PrepararServico(consMDFeNaoEnc?.GerarXML() ?? throw new System.ArgumentNullException(nameof(consMDFeNaoEnc)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta status do serviço.");

        #endregion Public Methods
    }
}
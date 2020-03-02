using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.MDFe;

namespace Unimake.Business.DFe.Servicos.MDFe
{
    [ComVisible(true)]
    public class StatusServico: ServicoBase, IInteropService<ConsStatServMDFe>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsStatServMDFe();
            xml = xml.LerXML<ConsStatServMDFe>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.MDFeStatusServico;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsStatServMDFe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsStatServMDFe>(RetornoWSXML);
                }

                return new RetConsStatServMDFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public StatusServico()
            : base() { }

        public StatusServico(ConsStatServMDFe consStatServMDFe, Configuracao configuracao)
            : base(consStatServMDFe?.GerarXML() ?? throw new System.ArgumentNullException(nameof(consStatServMDFe)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        public void Executar(ConsStatServMDFe consStatServMDFe, Configuracao configuracao)
        {
            PrepararServico(consStatServMDFe?.GerarXML() ?? throw new System.ArgumentNullException(nameof(consStatServMDFe)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta status do serviço.");

        #endregion Public Methods
    }
}
using System;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    [ComVisible(true)]
    public class StatusServico: ServicoBase, IInteropService<ConsStatServCte>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsStatServCte();
            xml = xml.LerXML<ConsStatServCte>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.CTeStatusServico;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsStatServCte Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsStatServCte>(RetornoWSXML);
                }

                return new RetConsStatServCte
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

        public StatusServico(ConsStatServCte consStatServCte, Configuracao configuracao)
                    : base(consStatServCte?.GerarXML() ?? throw new ArgumentNullException(nameof(consStatServCte)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        public void Executar(ConsStatServCte consStatServCte, Configuracao configuracao)
        {
            PrepararServico(consStatServCte?.GerarXML() ?? throw new ArgumentNullException(nameof(consStatServCte)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta status do serviço.");

        #endregion Public Methods
    }
}
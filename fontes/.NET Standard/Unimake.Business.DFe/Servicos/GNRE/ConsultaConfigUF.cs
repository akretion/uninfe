using System;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.GNRE;

namespace Unimake.Business.DFe.Servicos.GNRE
{
    public class ConsultaConfigUF: ServicoBase, IInteropService<TConsultaConfigUf>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new TConsultaConfigUf();
            xml = xml.LerXML<TConsultaConfigUf>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.GNREConsultaConfigUF;
                Configuracoes.CodigoUF = (int)xml.UF;
                Configuracoes.TipoAmbiente = xml.Ambiente;
                Configuracoes.SchemaVersao = "1.00";

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public TConfigUf Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<TConfigUf>(RetornoWSXML);
                }

                return new TConfigUf
                {
                    SituacaoConsulta = new SituacaoConsulta
                    {
                        Codigo = "0",
                        Descricao = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado do WebService."
                    }
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public ConsultaConfigUF()
            : base()
        {
        }

        public ConsultaConfigUF(TConsultaConfigUf tConsultaConfigUf, Configuracao configuracao)
                    : base(tConsultaConfigUf?.GerarXML() ?? throw new ArgumentNullException(nameof(tConsultaConfigUf)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(true)]
        public void Executar(TConsultaConfigUf tConsultaConfigUf, Configuracao configuracao)
        {
            PrepararServico(tConsultaConfigUf?.GerarXML() ?? throw new ArgumentNullException(nameof(tConsultaConfigUf)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta status do serviço.");

        #endregion Public Methods
    }
}
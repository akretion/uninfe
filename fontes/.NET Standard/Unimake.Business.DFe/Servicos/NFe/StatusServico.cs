using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    [ComVisible(true)]
    public class StatusServico: ServicoBase
    {
        #region Private Constructors

        private StatusServico(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        #endregion Private Constructors

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsStatServ();
            xml = xml.LerXML<ConsStatServ>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeStatusServico;
                Configuracoes.CodigoUF = (int)xml.CUF;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsStatServ Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsStatServ>(RetornoWSXML);
                }

                return new RetConsStatServ
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public StatusServico()
            : base()
        {
        }

        public StatusServico(ConsStatServ consStatServ, Configuracao configuracao)
                    : this(consStatServ?.GerarXML(), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta status do serviço.");

        #endregion Public Methods
    }
}
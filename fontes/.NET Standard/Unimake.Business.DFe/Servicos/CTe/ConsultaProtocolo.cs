using System;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    public class ConsultaProtocolo: ServicoBase, IInteropService<ConsSitCTe>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsSitCTe();
            xml = xml.LerXML<ConsSitCTe>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.CTeConsultaProtocolo;
                Configuracoes.CodigoUF = Convert.ToInt32(xml.ChCTe.Substring(0, 2));
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.Modelo = (ModeloDFe)int.Parse(xml.ChCTe.Substring(20, 2));
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsSitCTe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsSitCTe>(RetornoWSXML);
                }

                return new RetConsSitCTe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public ConsultaProtocolo(ConsSitCTe consSitCTe, Configuracao configuracao)
            : base(consSitCTe?.GerarXML() ?? throw new ArgumentNullException(nameof(consSitCTe)), configuracao) { }

        public ConsultaProtocolo()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Executar(ConsSitCTe consSitCTe, Configuracao configuracao)
        {
            PrepararServico(consSitCTe?.GerarXML() ?? throw new ArgumentNullException(nameof(consSitCTe)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta de protocolo.");

        #endregion Public Methods
    }
}
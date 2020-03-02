using System;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.MDFe;

namespace Unimake.Business.DFe.Servicos.MDFe
{
    public class ConsultaProtocolo: ServicoBase, IInteropService<ConsSitMDFe>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsSitMDFe();
            xml = xml.LerXML<ConsSitMDFe>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.MDFeConsultaProtocolo;
                Configuracoes.CodigoUF = Convert.ToInt32(xml.ChMDFe.Substring(0, 2));
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.Modelo = (ModeloDFe)int.Parse(xml.ChMDFe.Substring(20, 2));
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsSitMDFe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsSitMDFe>(RetornoWSXML);
                }

                return new RetConsSitMDFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public ConsultaProtocolo(ConsSitMDFe consSitMDFe, Configuracao configuracao)
            : base(consSitMDFe.GerarXML(), configuracao) { }

        public ConsultaProtocolo()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(true)]
        public void Executar(ConsSitMDFe consSitMDFe, Configuracao configuracao)
        {
            PrepararServico(consSitMDFe?.GerarXML() ?? throw new ArgumentNullException(nameof(consSitMDFe)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new Exception("Não existe XML de distribuição para consulta de protocolo.");

        #endregion Public Methods
    }
}
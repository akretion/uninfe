using System;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    public class RetAutorizacao: ServicoBase
    {
        #region Private Constructors

        private RetAutorizacao(XmlDocument conteudoXML, Configuracao configuracao)
                            : base(conteudoXML, configuracao) { }

        #endregion Private Constructors

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsReciCTe();
            xml = xml.LerXML<ConsReciCTe>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.CTeConsultaRecibo;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;
                Configuracoes.CodigoUF = Convert.ToInt32(xml.NRec.Substring(0, 2));
                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsReciCTe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsReciCTe>(RetornoWSXML);
                }

                return new RetConsReciCTe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public RetAutorizacao(ConsReciCTe consReciCTe, Configuracao configuracao)
            : this(consReciCTe?.GerarXML() ?? throw new ArgumentNullException(nameof(consReciCTe)), configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(true)]
        public void Executar(ConsReciCTe consReciCTe, Configuracao configuracao)
        {
            if(configuracao is null)
            {
                throw new ArgumentNullException(nameof(configuracao));
            }

            PrepararServico(consReciCTe?.GerarXML() ?? throw new ArgumentNullException(nameof(consReciCTe)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) =>
            throw new Exception("Não existe XML de distribuição para consulta do recibo de lote.");

        #endregion Public Methods
    }
}
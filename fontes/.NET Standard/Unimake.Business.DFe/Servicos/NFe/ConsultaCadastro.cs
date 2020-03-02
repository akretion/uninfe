using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class ConsultaCadastro: ServicoBase, IInteropService<ConsCadBase>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new ConsCad();
            xml = xml.LerXML<ConsCad>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeConsultaCadastro;
                Configuracoes.CodigoUF = (int)xml.InfCons.UF;
                Configuracoes.Modelo = ModeloDFe.NFe;
                Configuracoes.SchemaVersao = xml.Versao;
                Configuracoes.TipoAmbiente = TipoAmbiente.Producao;
                Configuracoes.TipoEmissao = TipoEmissao.Normal;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetConsCad Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsCad>(RetornoWSXML);
                }

                return new RetConsCad
                {
                    InfCons = new InfConsRetorno
                    {
                        CStat = 0,
                        XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                    }
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public ConsultaCadastro(ConsCadBase consCad, Configuracao configuracao)
                    : base(consCad.GerarXML(), configuracao) { }

        public ConsultaCadastro()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(false)]
        public override void Executar()
        {
            base.Executar();

            //Mato Grosso do Sul está retornando o XML da consulta cadastro fora do padrão, vou ter que intervir neste ponto para fazer a correção
            if(Configuracoes.CodigoUF == (int)UFBrasil.MT)
            {
                if(RetornoWSXML.GetElementsByTagName("retConsCad")[0] != null)
                {
                    RetornoWSString = RetornoWSXML.GetElementsByTagName("retConsCad")[0].OuterXml;
                    RetornoWSXML.LoadXml(RetornoWSString);
                }
            }
        }

        [ComVisible(true)]
        public void Executar(ConsCadBase consCad, Configuracao configuracao)
        {
            PrepararServico(consCad.GerarXML(), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new System.Exception("Não existe XML de distribuição para consulta cadastro do contribuinte.");

        #endregion Public Methods
    }
}
using System.Xml;
using Unimake.Business.DFe.Security;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class Autorizacao: ServicoBase
    {
        #region Private Fields

        protected EnviNFe EnviNFe;

        #endregion Private Fields

        #region Private Constructors

        private Autorizacao(XmlDocument conteudoXML, Configuracao configuracao)
                    : base(conteudoXML, configuracao) { }

        #endregion Private Constructors

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            if(EnviNFe == null)
            {
                Configuracoes.Definida = false;
                return;
            }

            var xml = EnviNFe;

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeAutorizacao;
                Configuracoes.CodigoUF = (int)xml.NFe[0].InfNFe[0].Ide.CUF;
                Configuracoes.TipoAmbiente = xml.NFe[0].InfNFe[0].Ide.TpAmb;
                Configuracoes.Modelo = xml.NFe[0].InfNFe[0].Ide.Mod;
                Configuracoes.TipoEmissao = xml.NFe[0].InfNFe[0].Ide.TpEmis;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Propriedade contendo o XML da NFe com o protocolo de autorização anexado
        /// </summary>
        public NfeProc NfeProcResult
        {
            get
            {
                if(EnviNFe.IndSinc == SimNao.Sim) //Envio síncrono
                {
                    return new NfeProc
                    {
                        Versao = EnviNFe.Versao,
                        NFe = EnviNFe.NFe[0],
                        ProtNFe = Result.ProtNFe
                    };
                }
                else //TODO: Ainda tenho que ver como vai ficar o envio assincrono, pois tem a consulta recibo para fazer.
                {
                    return null;
                    //if (Result.CStat == 103)
                    //{
                    //}
                }
            }
        }

        public RetEnviNFe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetEnviNFe>(RetornoWSXML);
                }

                return new RetEnviNFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public Autorizacao(EnviNFe enviNFe, Configuracao configuracao)
                            : this(enviNFe.GerarXML(), configuracao)
        {
            EnviNFe = enviNFe;
            Inicializar();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            if (Configuracoes.TipoDFe == DFE.NFe)
            {
                new AssinaturaDigital().Assinar(ConteudoXML, Configuracoes.TagAssinatura, Configuracoes.TagAtributoID, Configuracoes.CertificadoDigital, AlgorithmType.Sha1, true, "", "Id");
                EnviNFe = EnviNFe.LerXML<EnviNFe>(ConteudoXML);
            }


            base.Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta) => GravarXmlDistribuicao(pasta, NfeProcResult.NomeArquivoDistribuicao, NfeProcResult.GerarXML().OuterXml);

        /// <summary>
        /// Grava o XML de dsitribuição no stream
        /// </summary>
        /// <param name="stream">Stream que vai receber o XML de distribuição</param>
        public void GravarXmlDistribuicao(System.IO.Stream stream) => GravarXmlDistribuicao(stream, NfeProcResult.GerarXML().OuterXml);

        #endregion Public Methods
    }
}
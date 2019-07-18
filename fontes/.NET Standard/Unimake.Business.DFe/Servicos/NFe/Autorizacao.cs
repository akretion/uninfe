using System.Xml;
using Unimake.Business.DFe.Security;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class Autorizacao: ServicoBase
    {
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
            var xml = new EnviNFe();
            xml = xml.LerXML<EnviNFe>(ConteudoXML);

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
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            new AssinaturaDigital().Assinar(ConteudoXML, Configuracoes.TagAssinatura, Configuracoes.TagAtributoID, Configuracoes.CertificadoDigital, AlgorithmType.Sha1, true, "", "Id");
            base.Executar();
        }

        #endregion Public Methods
    }
}
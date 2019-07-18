using System;
using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class ConsultaProtocolo : ServicoBase
    {
        #region Private Constructors

        private ConsultaProtocolo(XmlDocument conteudoXML, Configuracao configuracao)
                            : base(conteudoXML, configuracao) { }

        #endregion Public Constructors

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsSitNFe xml = new ConsSitNFe();
            xml = xml.LerXML<ConsSitNFe>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeConsultaProtocolo;
                Configuracoes.CodigoUF = Convert.ToInt32(xml.ChNFe.Substring(0, 2));
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.Modelo = (ModeloDFe)int.Parse(xml.ChNFe.Substring(20, 2));
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        public RetConsSitNFe Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsSitNFe>(RetornoWSXML);
                }

                return new RetConsSitNFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        public ConsultaProtocolo(ConsSitNFe consSitNFe, Configuracao configuracao)
            : this(consSitNFe.GerarXML(), configuracao) { }
    }
}
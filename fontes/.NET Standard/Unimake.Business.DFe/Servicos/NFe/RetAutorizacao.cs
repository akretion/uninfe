using System;
using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class RetAutorizacao : ServicoBase
    {
        #region Private Constructors

        private RetAutorizacao(XmlDocument conteudoXML, Configuracao configuracao)
                            : base(conteudoXML, configuracao) { }

        #endregion Public Constructors

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsReciNFe xml = new ConsReciNFe();
            xml = xml.LerXML<ConsReciNFe>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeConsultaRecibo;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;
                Configuracoes.CodigoUF = Convert.ToInt32(xml.NRec.Substring(0, 2));
                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        public RetConsReciNFe Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsReciNFe>(RetornoWSXML);
                }

                return new RetConsReciNFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        public RetAutorizacao(ConsReciNFe consReciNFe, Configuracao configuracao)
            : this(consReciNFe.GerarXML(), configuracao) { }
    }
}
using System;
using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
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
            ConsSitCTe xml = new ConsSitCTe();
            xml = xml.LerXML<ConsSitCTe>(ConteudoXML);

            if (!Configuracoes.Definida)
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

        public RetConsSitCTe Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
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

        public ConsultaProtocolo(ConsSitCTe consSitCTe, Configuracao configuracao)
            : this(consSitCTe.GerarXML(), configuracao) { }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML)
        {
            throw new System.Exception("Não existe XML de distribuição para consulta de protocolo.");
        }

    }
}
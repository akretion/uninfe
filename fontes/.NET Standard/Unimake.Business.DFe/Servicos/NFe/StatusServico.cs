using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class StatusServico : ServicoBase
    {
        private StatusServico(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsStatServ xml = new ConsStatServ();
            xml = xml.LerXML<ConsStatServ>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeStatusServico;
                Configuracoes.CodigoUF = (int)xml.CUF;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        public RetConsStatServ Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
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

        public StatusServico(ConsStatServ consStatServ, Configuracao configuracao)
                    : this(consStatServ.GerarXML(), configuracao) { }
    }
}

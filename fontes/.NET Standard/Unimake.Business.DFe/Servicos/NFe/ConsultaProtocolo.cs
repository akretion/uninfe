using System;
using System.Xml;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class ConsultaProtocolo : ServicoBase
    {
        public ConsultaProtocolo(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsSitNFe xml = new ConsSitNFe();
            xml = xml.LerXML<ConsSitNFe>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.CodigoUF = Convert.ToInt32(xml.ChNFe.Substring(0, 2));
                Configuracoes.TipoAmbiente = (int)xml.TpAmb;
                Configuracoes.Modelo = xml.ChNFe.Substring(20, 2);
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }
    }
}

using System;
using System.Xml;
using Unimake.DFe.Xml.NFe;

namespace Unimake.DFe.Servicos.NFe
{
    public class ConsultaProtocolo : NFeBase
    {
        public ConsultaProtocolo(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsSitNFe xml = new ConsSitNFe();
            xml.Ler(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.cUF = Convert.ToInt32(xml.chNFe.Substring(0, 2));
                Configuracoes.tpAmb = xml.tpAmb;
                Configuracoes.mod = xml.chNFe.Substring(20, 2);
                Configuracoes.tpEmis = xml.tpEmis;
                Configuracoes.SchemaVersao = xml.versao;

                base.DefinirConfiguracao();
            }

            //Remover a tag tpEmis que não é padrão do XML
            ConteudoXML.DocumentElement.RemoveChild(ConteudoXML.GetElementsByTagName("tpEmis")[0]);
        }
    }
}

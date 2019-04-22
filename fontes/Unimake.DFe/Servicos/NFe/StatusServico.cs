using System.Xml;
using Unimake.DFe.Xml.NFe;

namespace Unimake.DFe.Servicos.NFe
{
    public class StatusServico : NFeBase
    {
        public StatusServico(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsStatServ xml = new ConsStatServ();
            xml.Ler(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.cUF = xml.cUF;
                Configuracoes.tpAmb = xml.tpAmb;
                Configuracoes.mod = "55";
                Configuracoes.tpEmis = xml.tpEmis;
                Configuracoes.SchemaVersao = xml.versao;

                base.DefinirConfiguracao();
            }

            //Remover a tag tpEmis que não é padrão do XML
            ConteudoXML.DocumentElement.RemoveChild(ConteudoXML.GetElementsByTagName("tpEmis")[0]);
        }
    }
}

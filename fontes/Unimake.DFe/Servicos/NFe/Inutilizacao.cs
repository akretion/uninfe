using System.Xml;
using Unimake.DFe.Xml.NFe;

namespace Unimake.DFe.Servicos.NFe
{
    public class Inutilizacao : NFeBase
    {
        public Inutilizacao(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            InutNFe xml = new InutNFe();
            xml.Ler(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.cUF = xml.infInut.cUF;
                Configuracoes.tpAmb = xml.infInut.tpAmb;
                Configuracoes.mod = xml.infInut.mod;
                Configuracoes.tpEmis = 1; //Inutilização só funciona no tipo de emissão Normal, ou seja, não tem inutilização em SVC-AN ou SVC-RS
                Configuracoes.SchemaVersao = xml.versao;

                base.DefinirConfiguracao();
            }
        }

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            new Security.AssinaturaDigital().Assinar(ConteudoXML, Configuracoes.TagAssinatura, Configuracoes.TagAtributoID, Configuracoes.CertificadoDigital);

            base.Executar();
        }
    }
}

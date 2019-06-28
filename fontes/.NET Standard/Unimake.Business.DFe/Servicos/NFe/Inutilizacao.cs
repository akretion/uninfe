using System.Xml;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class Inutilizacao : ServicoBase
    {
        public Inutilizacao(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            InutNFe xml = new InutNFe();
            xml = xml.LerXML<InutNFe>(ConteudoXML);
            
            if (!Configuracoes.Definida)
            {
//                Configuracoes.CodigoUF = (int)xml.infInut.cUF;
                Configuracoes.TipoAmbiente = (int)xml.infInut.tpAmb;
                Configuracoes.Modelo = xml.infInut.mod;
                Configuracoes.TipoEmissao = 1; //Inutilização só funciona no tipo de emissão Normal, ou seja, não tem inutilização em SVC-AN ou SVC-RS
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

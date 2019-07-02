using System.Xml;
using Unimake.Business.DFe.Security;
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
                Configuracoes.CodigoUF = (int)xml.InfInut.CUF;
                Configuracoes.TipoAmbiente = (int)xml.InfInut.TpAmb;
                Configuracoes.Modelo = ((int)xml.InfInut.Mod).ToString();
                Configuracoes.TipoEmissao = 1; //Inutilização só funciona no tipo de emissão Normal, ou seja, não tem inutilização em SVC-AN ou SVC-RS
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            new AssinaturaDigital().Assinar(ConteudoXML, Configuracoes.TagAssinatura, Configuracoes.TagAtributoID, Configuracoes.CertificadoDigital, AlgorithmType.Sha1, true, "", "Id");

            base.Executar();
        }
    }
}

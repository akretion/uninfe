using System.Xml;
using Unimake.DFe.Xml.NFe;

namespace Unimake.DFe.Servicos.NFe
{
    public class ConsultaCadastro : NFeBase
    {
        public ConsultaCadastro(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsCad xml = new ConsCad();
            xml.Ler(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.cUF = xml.infCons.cUF;
                Configuracoes.tpAmb = 1; //Consulta será sempre em produção
                Configuracoes.mod = "";
                Configuracoes.tpEmis = 1; //Consulta cadastro só funciona no tipo de emissão Normal, ou seja, não tem inutilização em SVC-AN ou SVC-RS
                Configuracoes.SchemaVersao = xml.versao;

                base.DefinirConfiguracao();
            }
        }
    }
}

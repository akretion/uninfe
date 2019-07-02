using System.Xml;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class RecepcaoEvento : ServicoBase
    {
        public RecepcaoEvento(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            EnvEvento xml = new EnvEvento();
            xml = xml.LerXML<EnvEvento>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.CodigoUF = (int)xml.Evento[0].InfEvento.COrgao;
                Configuracoes.TipoAmbiente = (int)xml.Evento[0].InfEvento.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        protected override void XmlValidar()
        {
            //TODO: WANDREY - tenho que encontrar uma forma de validar os XMLs de eventos, pois tem vários arquivos de schemas e o XML de configuração SP.XML ou PR.XML não preve vários.
        }
    }
}
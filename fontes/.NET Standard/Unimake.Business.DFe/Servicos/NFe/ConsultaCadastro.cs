using System.Xml;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class ConsultaCadastro : ServicoBase
    {
        private ConsultaCadastro(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            ConsCad xml = new ConsCad();
            xml = xml.LerXML<ConsCad>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeConsultaCadastro;
                Configuracoes.CodigoUF = (int)xml.InfCons.UF;
                Configuracoes.Modelo = ModeloDFe.NFe;
                Configuracoes.SchemaVersao = xml.Versao;
                Configuracoes.TipoAmbiente = TipoAmbiente.Producao;
                Configuracoes.TipoEmissao = TipoEmissao.Normal;

                base.DefinirConfiguracao();
            }
        }

        public RetConsCad Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetConsCad>(RetornoWSXML);
                }

                return new RetConsCad
                {
                    InfCons = new InfConsRetorno
                    {
                        CStat = 0,
                        XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                    }
                };
            }
        }

        public ConsultaCadastro(ConsCad consCad, Configuracao configuracao)
                    : this(consCad.GerarXML(), configuracao) { }

        public override void Executar()
        {
            base.Executar();

            //Mato Grosso do Sul está retornando o XML da consulta cadastro fora do padrão, vou ter que intervir neste ponto para fazer a correção
            if (Configuracoes.CodigoUF == (int)UFBrasil.MT)
            {
                if (RetornoWSXML.GetElementsByTagName("retConsCad")[0] != null)
                {
                    RetornoWSString = RetornoWSXML.GetElementsByTagName("retConsCad")[0].OuterXml;
                    RetornoWSXML.LoadXml(RetornoWSString);
                }
            }
        }
    }
}

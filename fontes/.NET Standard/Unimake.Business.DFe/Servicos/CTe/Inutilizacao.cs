using System.Xml;
using Unimake.Business.DFe.Security;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    public class Inutilizacao : ServicoBase
    {
        private Inutilizacao(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new InutCTe();
            xml = xml.LerXML<InutCTe>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.CTeInutilizacao;
                Configuracoes.CodigoUF = (int)xml.InfInut.CUF;
                Configuracoes.TipoAmbiente = xml.InfInut.TpAmb;
                Configuracoes.Modelo = xml.InfInut.Mod;
                Configuracoes.TipoEmissao = TipoEmissao.Normal; //Inutilização só funciona no tipo de emissão Normal, ou seja, não tem inutilização em SVC-AN ou SVC-RS
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
            InutCTe = InutCTe.LerXML<InutCTe>(ConteudoXML);

            base.Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            GravarXmlDistribuicao(pasta, ProcInutCTeResult.NomeArquivoDistribuicao, ProcInutCTeResult.GerarXML().OuterXml);
        }

        public RetInutCTe Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetInutCTe>(RetornoWSXML);
                }

                return new RetInutCTe
                {
                    InfInut = new InfInut
                    {
                        CStat = 0,
                        XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                    }
                };
            }
        }

        /// <summary>
        /// Propriedade contendo o XML da inutilização com o protocolo de autorização anexado
        /// </summary>
        public ProcInutCTe ProcInutCTeResult => new ProcInutCTe
        {
            Versao = InutCTe.Versao,
            InutCTe = InutCTe,
            RetInutCTe = Result
        };

        private InutCTe InutCTe;

        public Inutilizacao(InutCTe inutCTe, Configuracao configuracao)
            : this(inutCTe.GerarXML(), configuracao)
        {
            InutCTe = inutCTe;
        }
    }
}

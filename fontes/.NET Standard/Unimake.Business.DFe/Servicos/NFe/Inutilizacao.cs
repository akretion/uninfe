using System.Xml;
using Unimake.Business.DFe.Security;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
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
            var xml = new InutNFe();
            xml = xml.LerXML<InutNFe>(ConteudoXML);

            if (!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeInutilizacao;
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
            InutNFe = InutNFe.LerXML<InutNFe>(ConteudoXML);

            base.Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            GravarXmlDistribuicao(pasta, ProcInutNFeResult.NomeArquivoDistribuicao, ProcInutNFeResult.GerarXML().OuterXml);
        }

        public RetInutNFe Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetInutNFe>(RetornoWSXML);
                }

                return new RetInutNFe
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
        public ProcInutNFe ProcInutNFeResult => new ProcInutNFe
        {
            Versao = InutNFe.Versao,
            InutNFe = InutNFe,
            RetInutNFe = Result
        };

        private InutNFe InutNFe;

        public Inutilizacao(InutNFe inutNFe, Configuracao configuracao)
            : this(inutNFe.GerarXML(), configuracao) => InutNFe = inutNFe;
    }
}

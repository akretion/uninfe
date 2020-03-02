using System;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class DistribuicaoDFe: ServicoBase, IInteropService<DistDFeInt>
    {
        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            var xml = new DistDFeInt();
            xml = xml.LerXML<DistDFeInt>(ConteudoXML);

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeDistribuicaoDFe;
                Configuracoes.CodigoUF = (int)xml.COrgao;
                Configuracoes.TipoAmbiente = xml.TpAmb;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public RetDistDFeInt Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetDistDFeInt>(RetornoWSXML);
                }

                return new RetDistDFeInt
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public DistribuicaoDFe(DistDFeInt distDFeInt, Configuracao configuracao)
                    : base(distDFeInt.GerarXML(), configuracao) { }

        public DistribuicaoDFe()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [ComVisible(true)]
        public void Executar(DistDFeInt distDFeInt, Configuracao configuracao)
        {
            PrepararServico(distDFeInt?.GerarXML() ?? throw new ArgumentNullException(nameof(distDFeInt)), configuracao);
            Executar();
        }

        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML) => throw new Exception("Não existe XML de distribuição para consulta de documentos fiscais eletrônicos destinados.");

        /// <summary>
        /// Gravar os XML contidos no DocZIP da consulta em uma pasta no HD
        /// </summary>
        /// <param name="folder">Nome da pasta onde é para salvar os XML</param>
        /// <param name="saveXMLResumo">Salvar os arquivos de resumo da NFe e Eventos da NFe?</param>
        public void GravarXMLDocZIP(string folder, bool saveXMLResumo)
        {
            foreach(var item in Result.LoteDistDFeInt.DocZip)
            {
                var save = true;
                var conteudoXML = Compress.GZIPDecompress(Convert.ToBase64String(item.Value));
                var nomeArquivo = string.Empty;

                var docXML = new XmlDocument();
                docXML.Load(Converter.StringToStreamUTF8(conteudoXML));

                if(item.Schema.StartsWith("resEvento"))
                {
                    nomeArquivo = item.NSU + "-resEvento.xml";
                    save = saveXMLResumo;
                }
                else if(item.Schema.StartsWith("procEventoNFe"))
                {
                    var chNFe = XMLUtility.TagRead(((XmlElement)((XmlElement)docXML.GetElementsByTagName("evento")[0]).GetElementsByTagName("infEvento")[0]), "chNFe");
                    var tpEvento = XMLUtility.TagRead(((XmlElement)((XmlElement)docXML.GetElementsByTagName("evento")[0]).GetElementsByTagName("infEvento")[0]), "tpEvento");
                    var nSeqEvento = XMLUtility.TagRead(((XmlElement)((XmlElement)docXML.GetElementsByTagName("evento")[0]).GetElementsByTagName("infEvento")[0]), "nSeqEvento");
                    nomeArquivo = chNFe + "_" + tpEvento + "_" + nSeqEvento.PadLeft(2, '0') + "-procEventoNFe.xml";
                }
                else if(item.Schema.StartsWith("procNFe"))
                {
                    var chave = ((XmlElement)docXML.GetElementsByTagName("infNFe")[0]).GetAttribute("Id").Substring(3, 44);
                    nomeArquivo = chave + "-procNFe.xml";
                }
                else if(item.Schema.StartsWith("resNFe"))
                {
                    nomeArquivo = item.NSU + "-resNFe.xml";
                    save = saveXMLResumo;
                }

                if(save && !string.IsNullOrEmpty(nomeArquivo))
                {
                    base.GravarXmlDistribuicao(folder, nomeArquivo, conteudoXML);
                }
            }
        }

        #endregion Public Methods
    }
}
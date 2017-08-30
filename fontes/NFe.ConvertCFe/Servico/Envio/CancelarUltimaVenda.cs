using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System.IO;
using System.Xml;
using Unimake.SAT.Utility;
using EnunsSAT = Unimake.SAT.Enuns;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{
    /// <summary>
    /// Classe responsável pelo envio do cancelamento do SAT
    /// </summary>
    public class CancelarUltimaVenda : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        private Empresa DadosEmpresa = null;

        /// <summary>
        /// Dados do envio do XML
        /// </summary>
        private string CancelarUltimaVendaEnvio = null;

        /// <summary>
        /// Chave de acesso da venda que esta sendo cancelada
        /// </summary>
        private string ChaveAcessoVenda = null;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        private Servicos.Retorno.CancelarUltimaVendaResponse CancelarUltimaVendaRetorno = new Servicos.Retorno.CancelarUltimaVendaResponse();

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public CancelarUltimaVenda(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            XmlNodeList elemList = doc.GetElementsByTagName("infCFe");
            ChaveAcessoVenda = elemList[0].Attributes["chCanc"].Value;

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
            CancelarUltimaVendaEnvio = doc.InnerXml;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override void Enviar()
        {
            string resposta = Sat.CancelarUltimaVenda(ChaveAcessoVenda, CancelarUltimaVendaEnvio);
            CancelarUltimaVendaRetorno = new Servicos.Retorno.CancelarUltimaVendaResponse(resposta);
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string xml = string.Empty;
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.CancelarUltimaVendaSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.CancelarUltimaVendaSAT).RetornoXML);

            File.WriteAllText(result, CancelarUltimaVendaRetorno.ToXML());

            if (CancelarUltimaVendaRetorno.CodigoMensagem.Equals("07000"))
            {
                xml = Path.Combine(DadosEmpresa.PastaXmlRetorno, Path.GetFileName(ArquivoXML));
                File.WriteAllText(xml, CancelarUltimaVendaRetorno.ArquivoCFe);
            }

            File.Delete(ArquivoXML);

            return xml;
        }
    }
}
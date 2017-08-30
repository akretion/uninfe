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
    /// Classe responsável pelo envio da venda pelo SAT
    /// </summary>
    public class EnviarDadosVenda : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        private Empresa DadosEmpresa = null;

        /// <summary>
        /// Dados do envio do XML
        /// </summary>
        private string DadosVendaEnvio = null;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        private Servicos.Retorno.EnviarDadosVendaResponse DadosVendaRetorno = new Servicos.Retorno.EnviarDadosVendaResponse();

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public EnviarDadosVenda(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
            DadosVendaEnvio = doc.InnerXml;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override void Enviar()
        {
            string resposta = Sat.EnviarDadosVenda(DadosVendaEnvio);
            DadosVendaRetorno = new Servicos.Retorno.EnviarDadosVendaResponse(resposta);
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string xml = string.Empty;
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).RetornoXML);

            File.WriteAllText(result, DadosVendaRetorno.ToXML());

            if (DadosVendaRetorno.CodigoMensagem.Equals("06000"))
            {
                xml = Path.Combine(DadosEmpresa.PastaXmlRetorno, DadosVendaRetorno.ChaveConsulta.Substring(3) + Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).EnvioXML);
                File.WriteAllText(xml, DadosVendaRetorno.ArquivoCFe);
            }

            File.Delete(ArquivoXML);

            return xml;
        }
    }
}
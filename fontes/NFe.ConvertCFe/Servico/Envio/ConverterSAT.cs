using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.SAT.Conversao;
using NFe.Settings;
using System.IO;
using System.Xml;

namespace NFe.SAT.Servico.Envio
{
    /// <summary>
    /// Classe responsável pela conversão da NFCe para CFe para envio pelo SAT
    /// </summary>
    public class ConverterSAT : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        Empresa DadosEmpresa = null;

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Caminho do arquivo XML convertido
        /// </summary>
        private string ArquivoConvertido { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ConverterSAT(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override void Enviar()
        {
            ArquivoConvertido = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                                             Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConverterSAT).EnvioXML) +
                                                                                  Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).EnvioXML);

            ConverterNFCe conversao = new ConverterNFCe(ArquivoXML, DadosEmpresa, ArquivoConvertido);
            conversao.ConverterSAT();
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlEnvio, Path.GetFileName(ArquivoConvertido));
            File.Move(ArquivoConvertido, result);
            File.Delete(ArquivoXML);

            return result;
        }
    }
}
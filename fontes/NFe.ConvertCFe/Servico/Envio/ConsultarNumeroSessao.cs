using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using Unimake.SAT.Utility;
using EnunsSAT = Unimake.SAT.Enuns;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{
    /// <summary>
    /// Classe responsável pela comunicação com o SAT
    /// </summary>
    public class ConsultarNumeroSessao : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        private Empresa DadosEmpresa = null;

        /// <summary>
        /// Dados do envio do XML
        /// </summary>
        private string ConsultarNumeroSessaoEnvio = null;

        /// <summary>
        /// Numero da sessão que será consultada
        /// </summary>
        private int NumeroSessao = 0;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        private Servicos.Retorno.ConsultarNumeroSessaoResponse ConsultarRetorno = null;

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ConsultarNumeroSessao(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            ConsultarNumeroSessaoEnvio = doc.InnerXml;
            NumeroSessao = Convert.ToInt32(GetValueXML(doc, "ConsultarNumeroSessao", "NumeroSessao"));
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override void Enviar()
        {
            string resposta = Sat.ConsultarNumeroSessao(NumeroSessao);
            ConsultarRetorno = new Servicos.Retorno.ConsultarNumeroSessaoResponse(resposta);
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarNumeroSessaoSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarNumeroSessaoSAT).RetornoXML);

            File.WriteAllText(result, ConsultarRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}
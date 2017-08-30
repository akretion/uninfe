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
    /// Classe responsável pela associação da assinatura no SAT
    /// </summary>
    public class AssociarAssinatura : ServicoBase
    {
        /// <summary>
        /// Dados do XML
        /// </summary>
        private XmlDocument Document = new XmlDocument();

        /// <summary>
        /// Dados da empresa
        /// </summary>
        private Empresa DadosEmpresa = null;

        /// <summary>
        /// Assinatura digital conjunto "CNPJ Software House" + "CNPJ do estabelecimento comercial".
        /// </summary>
        private string CNPJValue = null;

        /// <summary>
        /// Assinatura digital conjunto "CNPJ Software House" + "CNPJ do estabelecimento comercial".
        /// </summary>
        private string AssinaturaCNPJs = null;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        private Servicos.Retorno.AssociarAssinaturaResponse AssociarAssinaturaRetorno = new Servicos.Retorno.AssociarAssinaturaResponse();

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public AssociarAssinatura(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            Document.Load(fs);
            fs.Close();
            fs.Dispose();

            CNPJValue = GetValueXML(Document, "AssociarAssinatura", "CNPJvalue");
            AssinaturaCNPJs = GetValueXML(Document, "AssociarAssinatura", "assinaturaCNPJs");

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override void Enviar()
        {
            string resposta = Sat.CancelarUltimaVenda(CNPJValue, AssinaturaCNPJs);
            AssociarAssinaturaRetorno = new Servicos.Retorno.AssociarAssinaturaResponse(resposta);
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.AssociarAssinaturaSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.AssociarAssinaturaSAT).RetornoXML);

            File.WriteAllText(result, AssociarAssinaturaRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}
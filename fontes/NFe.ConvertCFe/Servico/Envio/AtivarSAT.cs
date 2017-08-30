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
    /// Classe responsável pela ativação do SAT
    /// </summary>
    public class AtivarSAT : ServicoBase
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
        /// Identificador do tipo de Certificado, descritos na tabela 15
        /// </summary>
        private int SubComando = 0;

        /// <summary>
        /// CNPJ do contribuinte, somente números
        /// </summary>
        private string CNPJ = null;

        /// <summary>
        /// Código do Estado da Federação, segundo tabela do IBGE, onde o SAT será ativado
        /// </summary>
        private int CodigoUF = 0;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        private Servicos.Retorno.AtivarSATResponse AtivarSATRetorno = new Servicos.Retorno.AtivarSATResponse();

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public AtivarSAT(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            Document.Load(fs);
            fs.Close();
            fs.Dispose();

            SubComando = Convert.ToInt32(GetValueXML(Document, "AtivarSAT", "SubComando"));
            CNPJ = GetValueXML(Document, "AtivarSAT", "CNPJ");
            CodigoUF = Convert.ToInt32(GetValueXML(Document, "AtivarSAT", "CodigoUF"));

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
            string resposta = Sat.AtivarSAT(SubComando, CNPJ, CodigoUF);
            AtivarSATRetorno = new Servicos.Retorno.AtivarSATResponse(resposta);
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.AtivarSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.AtivarSAT).RetornoXML);

            File.WriteAllText(result, AtivarSATRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}
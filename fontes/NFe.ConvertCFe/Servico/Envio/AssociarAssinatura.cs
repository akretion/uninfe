using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Servicos = Unimake.SAT.Servico;
using EnunsSAT = Unimake.SAT.Enuns;
using NFe.Components;
using NFe.SAT.Abstract.Servico;
using Unimake.SAT;
using NFe.Settings;

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
        XmlDocument Document = new XmlDocument();

        /// <summary>
        /// Dados da empresa
        /// </summary>
        Empresa DadosEmpresa = null;

        /// <summary>
        /// Assinatura digital conjunto "CNPJ Software House" + "CNPJ do estabelecimento comercial". 
        /// </summary>
        string CNPJValue = null;

        /// <summary>
        /// Assinatura digital conjunto "CNPJ Software House" + "CNPJ do estabelecimento comercial". 
        /// </summary>
        string AssinaturaCNPJs = null;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        Servicos.Retorno.AssociarAssinaturaResponse AssociarAssinaturaRetorno = new Servicos.Retorno.AssociarAssinaturaResponse();

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

            CNPJValue = GetValueXML(Document,"AssociarAssinatura", "CNPJvalue");
            AssinaturaCNPJs = GetValueXML(Document, "AssociarAssinatura", "assinaturaCNPJs");

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            Marca = Utils.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override string Enviar()
        {
            string resposta = Sat.CancelarUltimaVenda(CNPJValue, AssinaturaCNPJs);
            AssociarAssinaturaRetorno = new Servicos.Retorno.AssociarAssinaturaResponse(resposta);

            return AssociarAssinaturaRetorno.ToXML();
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string xml = "";
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                                         Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.AssociarAssinaturaSAT).EnvioXML) +
                                                                              Propriedade.Extensao(Propriedade.TipoEnvio.AssociarAssinaturaSAT).RetornoXML);
            using (StreamWriter writer = new StreamWriter(result))
                writer.Write(AssociarAssinaturaRetorno.ToXML());

            File.Delete(ArquivoXML);

            return xml;
        }
    }
}

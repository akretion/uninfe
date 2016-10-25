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
    /// Classe responsável pelo envio da venda pelo SAT
    /// </summary>
    public class EnviarDadosVenda : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        Empresa DadosEmpresa = null;

        /// <summary>
        /// Dados do envio do XML
        /// </summary>
        string DadosVendaEnvio = null;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        Servicos.Retorno.EnviarDadosVendaResponse DadosVendaRetorno = new Servicos.Retorno.EnviarDadosVendaResponse();

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
            Marca = Utils.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
            DadosVendaEnvio = doc.InnerXml;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override string Enviar()
        {
            string resposta = Sat.EnviarDadosVenda(DadosVendaEnvio);
            DadosVendaRetorno = new Servicos.Retorno.EnviarDadosVendaResponse(resposta);

            return DadosVendaRetorno.ToXML();
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string xml = "";
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                                         Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).EnvioXML) +
                                                                              Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).RetornoXML);
            using (StreamWriter writer = new StreamWriter(result))
                writer.Write(DadosVendaRetorno.ToXML());

            if (DadosVendaRetorno.CodigoMensagem.Equals("06000"))
            {
                xml = Path.Combine(DadosEmpresa.PastaXmlRetorno, Path.GetFileName(ArquivoXML));
                using (StreamWriter writer = new StreamWriter(xml))
                    writer.Write(DadosVendaRetorno.ArquivoCFe);
            }

            File.Delete(ArquivoXML);

            return xml;
        }
    }
}

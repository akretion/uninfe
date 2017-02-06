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
    /// Classe responsável pelo envio do cancelamento do SAT
    /// </summary>
    public class CancelarUltimaVenda : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        Empresa DadosEmpresa = null;

        /// <summary>
        /// Dados do envio do XML
        /// </summary>
        string CancelarUltimaVendaEnvio = null;

        /// <summary>
        /// Chave de acesso da venda que esta sendo cancelada
        /// </summary>
        string ChaveAcessoVenda = null;

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        Servicos.Retorno.CancelarUltimaVendaResponse CancelarUltimaVendaRetorno = new Servicos.Retorno.CancelarUltimaVendaResponse();

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
            Marca = Utils.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
            CancelarUltimaVendaEnvio = doc.InnerXml;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override string Enviar()
        {
            string resposta = Sat.CancelarUltimaVenda(ChaveAcessoVenda, CancelarUltimaVendaEnvio);
            CancelarUltimaVendaRetorno = new Servicos.Retorno.CancelarUltimaVendaResponse(resposta);

            return CancelarUltimaVendaRetorno.ToXML();
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string xml = "";
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                                         Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.CancelarUltimaVendaSAT).EnvioXML) +
                                                                              Propriedade.Extensao(Propriedade.TipoEnvio.CancelarUltimaVendaSAT).RetornoXML);
            using (StreamWriter writer = new StreamWriter(result))
                writer.Write(CancelarUltimaVendaRetorno.ToXML());

            if (CancelarUltimaVendaRetorno.CodigoMensagem.Equals("07000"))
            {
                xml = Path.Combine(DadosEmpresa.PastaXmlRetorno, Path.GetFileName(ArquivoXML));
                using (StreamWriter writer = new StreamWriter(xml))
                    writer.Write(CancelarUltimaVendaRetorno.ArquivoCFe);
            }

            File.Delete(ArquivoXML);

            return xml;
        }
    }
}

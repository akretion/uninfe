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
    /// Classe responsável pela comunicação com o SAT
    /// </summary>
    public class TrocarCodigoDeAtivacao : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        Empresa DadosEmpresa = null;

        /// <summary>
        /// Dados do envio do XML
        /// </summary>
        Servicos.Envio.TrocarCodigoDeAtivacao TrocarCodigoDeAtivacaoEnvio = new Servicos.Envio.TrocarCodigoDeAtivacao();

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        Servicos.Retorno.TrocarCodigoDeAtivacaoResponse TrocarCodigoDeAtivacaoRetorno = null;

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public TrocarCodigoDeAtivacao(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            TrocarCodigoDeAtivacaoEnvio = DeserializarObjeto<Servicos.Envio.TrocarCodigoDeAtivacao>();            
            Marca = TrocarCodigoDeAtivacaoEnvio.Marca;
            CodigoAtivacao = TrocarCodigoDeAtivacaoEnvio.CodigoAtivacaoAtual;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override void Enviar()
        {
            string resposta = Sat.TrocarCodigoDeAtivacao(TrocarCodigoDeAtivacaoEnvio.Opcao ,TrocarCodigoDeAtivacaoEnvio.CodigoAtivacaoNovo, TrocarCodigoDeAtivacaoEnvio.CodigoAtivacaoNovo);
            TrocarCodigoDeAtivacaoRetorno = new Servicos.Retorno.TrocarCodigoDeAtivacaoResponse(resposta);
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno, 
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.TrocarCodigoDeAtivacaoSAT).EnvioXML) + 
                Propriedade.Extensao(Propriedade.TipoEnvio.TrocarCodigoDeAtivacaoSAT).RetornoXML);                        

            File.WriteAllText(result, TrocarCodigoDeAtivacaoRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }       
    }
}

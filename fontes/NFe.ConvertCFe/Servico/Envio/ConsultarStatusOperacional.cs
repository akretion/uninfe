﻿using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System.IO;
using System.Xml;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{
    /// <summary>
    /// Classe responsável pela comunicação com o SAT
    /// </summary>
    public class ConsultarStatusOperacional : ServicoBase
    {
        /// <summary>
        /// Dados da empresa
        /// </summary>
        private Empresa DadosEmpresa = null;

        /// <summary>
        /// Dados do envio do XML
        /// </summary>
        private Servicos.Envio.ConsultarStatusOperacional ConsultarStatusOperacionalEnvio = new Servicos.Envio.ConsultarStatusOperacional();

        /// <summary>
        /// Resposta do equipamento sat
        /// </summary>
        private Servicos.Retorno.ConsultarStatusOperacionalResponse ConsultarStatusOperacionalRetorno = null;

        /// <summary>
        /// Nome do arquivo XML que esta sendo manipulado
        /// </summary>
        public override string ArquivoXML { get; set; }

        /// <summary>
        /// Construtor com serialização
        /// </summary>
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ConsultarStatusOperacional(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            ConsultarStatusOperacionalEnvio = DeserializarObjeto<Servicos.Envio.ConsultarStatusOperacional>();
            Marca = ConsultarStatusOperacionalEnvio.Marca;
            CodigoAtivacao = ConsultarStatusOperacionalEnvio.CodigoAtivacao;
        }

        /// <summary>
        /// Comunicar com o equipamento SAT
        /// </summary>
        public override void Enviar()
        {
            string resposta = Sat.ConsultarStatusOperacional();
            ConsultarStatusOperacionalRetorno = new Servicos.Retorno.ConsultarStatusOperacionalResponse(resposta);
        }

        /// <summary>
        /// Salva o XML de Retorno
        /// </summary>
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarStatusOperacionalSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarStatusOperacionalSAT).RetornoXML);

            File.WriteAllText(result, ConsultarStatusOperacionalRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}
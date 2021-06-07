using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskConsultaSequenciaLoteNotaRPS : TaskAbst
    {
        public TaskConsultaSequenciaLoteNotaRPS(string arquivo)
        {
            Servico = Servicos.NFSeConsultaSequenciaLoteNotaRPS;
            NomeArquivoXML = arquivo;
        }

        #region Objeto com os dados do XML da consulta nfse

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSeqLoteNotaRPS dadosPedSeqLoteNotaRPS;

        #endregion Objeto com os dados do XML da consulta nfse

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSeqLoteNotaRPS).EnvioXML) + Propriedade.ExtRetorno.SeqLoteNotaRPS_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                dadosPedSeqLoteNotaRPS = new DadosPedSeqLoteNotaRPS(emp);
                //Ler o XML para pegar parâmetros de envio
                PedSeqLoteNotaRPS(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(dadosPedSeqLoteNotaRPS.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedSeqLoteNotaRPS = null;

                if (IsUtilizaCompilacaoWs(padraoNFSe, Servico, dadosPedSeqLoteNotaRPS.cMunicipio))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedSeqLoteNotaRPS.cMunicipio, dadosPedSeqLoteNotaRPS.tpAmb, dadosPedSeqLoteNotaRPS.tpEmis, padraoNFSe, dadosPedSeqLoteNotaRPS.cMunicipio);
                    if (wsProxy != null) pedSeqLoteNotaRPS = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedSeqLoteNotaRPS.cMunicipio, dadosPedSeqLoteNotaRPS.tpAmb, dadosPedSeqLoteNotaRPS.tpEmis, padraoNFSe, Servico);

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.TECNOSISTEMAS:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;
                }

                if (IsInvocar(padraoNFSe, Servico, dadosPedSeqLoteNotaRPS.cMunicipio))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, dadosPedSeqLoteNotaRPS.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedSeqLoteNotaRPS, NomeMetodoWS(Servico, dadosPedSeqLoteNotaRPS.cMunicipio),
                        cabecMsg, this,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSeqLoteNotaRPS).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSeqLoteNotaRPS).RetornoXML,
                        padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSeqLoteNotaRPS).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSeqLoteNotaRPS).RetornoXML);

                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSeqLoteNotaRPS).EnvioXML, Propriedade.ExtRetorno.SeqLoteNotaRPS_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 31/08/2011
                }
            }
            finally
            {
                try
                {
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 31/08/2011
                }
            }
        }

        #endregion Execute

        #region PedSeqLoteNotaRPS()

        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por numero e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedSeqLoteNotaRPS(string arquivoXML)
        {
            int emp = Empresas.FindEmpresaByThread();
        }

        #endregion PedSeqLoteNotaRPS()
    }
}
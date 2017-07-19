using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskConsultarCfse : TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oDadosPedSitNfse;

        #endregion Objeto com os dados do XML da consulta nfse

        public TaskConsultarCfse(string arquivo)
        {
            Servico = Servicos.ConsultarCfse;

            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedSitNfse = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosPedSitNfse.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedLoteRps = null;
                if (IsUtilizaCompilacaoWs(padraoNFSe, Servico, oDadosPedSitNfse.cMunicipio))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, oDadosPedSitNfse.cMunicipio);
                    if (wsProxy != null) pedLoteRps = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, Servico);

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://iss.irati.pr.gov.br/Arquivos/nfseV202.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;
                }

                if (IsInvocar(padraoNFSe, Servico, oDadosPedSitNfse.cMunicipio))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, oDadosPedSitNfse.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, oDadosPedSitNfse.cMunicipio),
                                            cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).RetornoXML,
                                            padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).RetornoXML);
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitCFSe).RetornoERR, ex);
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
    }
}
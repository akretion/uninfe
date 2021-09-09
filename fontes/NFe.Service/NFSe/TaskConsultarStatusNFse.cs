using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskConsultarStatusNFse: TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedStaNfse oDadosPedStaNfse;

        public TaskConsultarStatusNFse(string arquivo)
        {
            Servico = Servicos.NFSeConsultarStatusNota;
            NomeArquivoXML = arquivo;
        }

        #endregion Objeto com os dados do XML da consulta nfse

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            ///
            /// extensao permitida:  PedStaNfse = "-ped-stanfse.xml"
            ///
            /// Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeConsultarStatusNota;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedStaNFse).EnvioXML) + Propriedade.ExtRetorno.Sta_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedStaNfse = new DadosPedStaNfse(emp);

                //Ler o XML para pegar parâmetros de envio
                PedStaNfse(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                var padraoNFSe = Functions.PadraoNFSe(oDadosPedStaNfse.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedStaNota = null;
                if(IsUtilizaCompilacaoWs(padraoNFSe, Servico, oDadosPedStaNfse.cMunicipio))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedStaNfse.cMunicipio,
                            oDadosPedStaNfse.tpAmb, oDadosPedStaNfse.tpEmis, padraoNFSe, oDadosPedStaNfse.cMunicipio);
                    if(wsProxy != null)
                    {
                        pedStaNota = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    }
                }

                var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedStaNfse.cMunicipio, oDadosPedStaNfse.tpAmb, oDadosPedStaNfse.tpEmis, padraoNFSe, Servico);

                var cabecMsg = "";

                if(IsInvocar(padraoNFSe, Servico, oDadosPedStaNfse.cMunicipio))
                {
                    //Assinar o XML
                    var ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, oDadosPedStaNfse.cMunicipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedStaNota, NomeMetodoWS(Servico, oDadosPedStaNfse.cMunicipio),
                                            cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedStaNFse).EnvioXML,    //"-ped-stanfse",
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedStaNFse).RetornoXML,     //"-stanfse",
                                            padraoNFSe, Servico, securityProtocolType);

                    /// grava o arquivo no FTP
                    var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
                    if(File.Exists(filenameFTP))
                    {
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                    }
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML, Propriedade.ExtRetorno.SitNfse_ERR, ex);
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

        #region PedStaNfse()

        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por numero e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedStaNfse(string arquivoXML)
        {
            var emp = Empresas.FindEmpresaByThread();
        }

        #endregion PedStaNfse()

    }
}

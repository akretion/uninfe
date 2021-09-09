using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service.NFSe
{
    public class TaskConsultarNfseTomados: TaskAbst
    {
        public TaskConsultarNfseTomados(string arquivo)
        {
            Servico = Servicos.NFSeConsultarNFSeTomados;
            NomeArquivoXML = arquivo;
        }

        #region Objeto com os dados do XML da consulta nfse

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse dadosXML;

        #endregion Objeto com os dados do XML da consulta nfse

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                dadosXML = new DadosPedSitNfse(emp);
                var padraoNFSe = Functions.PadraoNFSe(dadosXML.cMunicipio);

                switch(padraoNFSe)
                {
                    case PadroesNFSe.BETHA:
                        ExecuteDLL(emp, dadosXML.cMunicipio, padraoNFSe);
                        break;

                    default:
                        WebServiceProxy wsProxy = null;
                        object pedConsNfseTomados = null;

                        if(IsUtilizaCompilacaoWs(padraoNFSe))
                        {
                            wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, dadosXML.cMunicipio);
                            pedConsNfseTomados = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                        }
                        var cabecMsg = "";

                        switch(padraoNFSe)
                        {
                            case PadroesNFSe.INDAIATUBA_SP:
                                cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                                break;

                            case PadroesNFSe.SIGCORP_SIGISS_203:
                                cabecMsg = "<cabecalho versao=\"2.03\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.03</versaoDados></cabecalho>";
                                break;

                            case PadroesNFSe.SMARAPD_204:
                                cabecMsg = "<cabecalho versao=\"2.04\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.04</versaoDados></cabecalho>";
                                break;
                            case PadroesNFSe.IIBRASIL:
                                cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.04\"><versaoDados>2.04</versaoDados></cabecalho>";
                                break;
                        }

                        var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, Servico);

                        if(IsInvocar(padraoNFSe, Servico, dadosXML.cMunicipio))
                        {
                            //Assinar o XML
                            var ad = new AssinaturaDigital();
                            ad.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                            //Invocar o método que envia o XML para o SEFAZ
                            oInvocarObj.InvocarNFSe(wsProxy, pedConsNfseTomados, NomeMetodoWS(Servico, dadosXML.cMunicipio), cabecMsg, this,
                                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML,
                                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoXML,
                                padraoNFSe, Servico, securityProtocolType);

                            /// grava o arquivo no FTP
                            var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML) +
                                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoXML);

                            if(File.Exists(filenameFTP))
                            {
                                new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                            }
                        }

                        break;
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoERR, ex);
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

        /// <summary>
        /// Retorna a versão do XML que está sendo enviado para o município de acordo com o Padrão/Município
        /// </summary>
        /// <param name="codMunicipio">Código do município para onde será enviado o XML</param>
        /// <param name="xmlDoc">Conteúdo do XML da NFSe</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        /// <returns>Retorna a versão do XML que está sendo enviado para o município de acordo com o Padrão/Município</returns>
        private void ExecuteDLL(int emp, int municipio, PadroesNFSe padraoNFSe)
        {
            var conteudoXML = new XmlDocument();
            conteudoXML.Load(NomeArquivoXML);

            var finalArqEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML;
            var finalArqRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoXML;
            var versaoXML = DefinirVersaoXML(municipio, conteudoXML, padraoNFSe);
            var servico = Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNfseServicoTomado;

            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, finalArqEnvio) + Functions.ExtractExtension(finalArqRetorno) + ".err");

            var configuracao = new Unimake.Business.DFe.Servicos.Configuracao
            {
                TipoDFe = Unimake.Business.DFe.Servicos.TipoDFe.NFSe,
                CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado,
                TipoAmbiente = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                CodigoMunicipio = municipio,
                Servico = servico,
                SchemaVersao = versaoXML
            };

            var consultarNfseServicoTomado = new Unimake.Business.DFe.Servicos.NFSe.ConsultarNfseServicoTomado(conteudoXML, configuracao);
            consultarNfseServicoTomado.Executar();

            vStrXmlRetorno = consultarNfseServicoTomado.RetornoWSString;

            XmlRetorno(finalArqEnvio, finalArqRetorno);

            /// grava o arquivo no FTP
            var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeTom).RetornoXML);

            if(File.Exists(filenameFTP))
            {
                new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
        }

        /// <summary>
        /// Executa o serviço utilizando a DLL do UniNFe.
        /// </summary>
        /// <param name="emp">Empresa que está enviando o XML</param>
        /// <param name="municipio">Código do município para onde será enviado o XML</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        private string DefinirVersaoXML(int codMunicipio, XmlDocument xmlDoc, PadroesNFSe padraoNFSe)
        {
            var versaoXML = "0.00";

            switch(padraoNFSe)
            {
                case PadroesNFSe.BETHA:
                    versaoXML = "2.02";
                    break;
            }

            return versaoXML;
        }
    }
}
using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskSubstituirNfse : TaskAbst
    {
        public TaskSubstituirNfse(string arquivo)
        {
            Servico = Servicos.NFSeSubstituirNfse;
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
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                dadosXML = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(dadosXML.cMunicipio);
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, dadosXML.cMunicipio);
                object pedSubstNfse = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                string cabecMsg = "";

                switch (padraoNFSe)
                {
                    case PadroesNFSe.AVMB_ASTEN:
                        Servico = GetTipoServicoSincrono(Servico, NomeArquivoXML, PadroesNFSe.AVMB_ASTEN);

                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (dadosXML.tpAmb == 2)
                            pedSubstNfse = new Components.HPelotasRS.INfseservice();
                        else
                            pedSubstNfse = new Components.PPelotasRS.INfseservice();
                        break;
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, Servico);

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedSubstNfse, NomeMetodoWS(Servico, dadosXML.cMunicipio), cabecMsg, this,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoXML,
                    padraoNFSe, Servico, securityProtocolType);

                ///
                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML) +
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoXML);

                if (File.Exists(filenameFTP))
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoERR, ex);
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
using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskConfigurarTerminalCfse : TaskAbst
    {
        public TaskConfigurarTerminalCfse(string arquivo)
        {
            Servico = Servicos.ConfigurarTerminalCfse;

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
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                int tpEmis = Empresas.Configuracoes[emp].tpEmis;
                int tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
                int municipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;

                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(municipio);
                WebServiceProxy wsProxy = null;
                object envConfigTermCfse = null;

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                if (IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, municipio, tpAmb, tpEmis, padraoNFSe, municipio);
                    if (wsProxy != null)
                        envConfigTermCfse = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }
                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(municipio, tpAmb, tpEmis, padraoNFSe, Servico);

                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho versao=\"2.02\" xmlns=\"http://iss.irati.pr.gov.br/Arquivos/nfseV202.xsd\"><versaoDados>2.02</versaoDados></cabecalho>";
                        break;
                }

                if (IsInvocar(padraoNFSe))
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, municipio);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, envConfigTermCfse, NomeMetodoWS(Servico, municipio), cabecMsg, this,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).EnvioXML,   
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).RetornoXML, 
                                            padraoNFSe, Servico, securityProtocolType);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                      Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).RetornoXML);
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvConfigTermCFSe).RetornoERR, ex);
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
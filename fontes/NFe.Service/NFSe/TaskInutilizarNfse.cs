using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskInutilizarNfse: TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oDadosInuNfse;
        #endregion

        #region Execute
        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeInutilizarNFSe;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInuNFSe).EnvioXML) + Propriedade.ExtRetorno.InuNfse_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosInuNfse = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                var padraoNFSe = Functions.PadraoNFSe(oDadosInuNfse.cMunicipio);
                var wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosInuNfse.cMunicipio, oDadosInuNfse.tpAmb, oDadosInuNfse.tpEmis, padraoNFSe, oDadosInuNfse.cMunicipio);
                var pedInuNfse = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                var cabecMsg = "";

                var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosInuNfse.cMunicipio, oDadosInuNfse.tpAmb, oDadosInuNfse.tpEmis, padraoNFSe, Servico);

                //Assinar o XML
                var ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosInuNfse.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedInuNfse, NomeMetodoWS(Servico, oDadosInuNfse.cMunicipio), cabecMsg, this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedInuNFSe).EnvioXML,    //"-ped-inunfse", 
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedInuNFSe).RetornoXML,  //"-inunfse", 
                                        padraoNFSe, Servico, securityProtocolType);

                ///
                /// grava o arquivo no FTP
                var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInuNFSe).EnvioXML) +
                                                Propriedade.Extensao(Propriedade.TipoEnvio.PedInuNFSe).RetornoXML);
                if(File.Exists(filenameFTP))
                {
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInuNFSe).EnvioXML, Propriedade.ExtRetorno.InuNfse_ERR, ex);
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
        #endregion
    }
}

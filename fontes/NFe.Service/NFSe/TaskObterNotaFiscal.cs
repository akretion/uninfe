using NFe.Certificado;
using NFe.Components;
using NFe.Components.WEBFISCO_TECNOLOGIA;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service.NFSe
{
    public class TaskObterNotaFiscal : TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oObterNotaFiscal;
        #endregion

        #region Execute
        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeObterNotaFiscal;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).EnvioXML) +
                                         Propriedade.ExtRetorno.NFSeXML_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oObterNotaFiscal = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do municipio
                var padraoNFSe = Functions.PadraoNFSe(oObterNotaFiscal.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedNfseXML = null;

                if (IsUtilizaCompilacaoWs(padraoNFSe, Servico, oObterNotaFiscal.cMunicipio))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oObterNotaFiscal.cMunicipio, oObterNotaFiscal.tpAmb, oObterNotaFiscal.tpEmis, padraoNFSe, oObterNotaFiscal.cMunicipio);

                    if (wsProxy != null)
                    {
                        pedNfseXML = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    }
                }

                var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oObterNotaFiscal.cMunicipio, oObterNotaFiscal.tpAmb, oObterNotaFiscal.tpEmis, padraoNFSe, Servico);
                var cabecMsg = "";

                switch (padraoNFSe)
                {
                    case PadroesNFSe.WEBFISCO_TECNOLOGIA:
                        var webTecnologia = new WEBFISCO_TECNOLOGIA((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            oObterNotaFiscal.cMunicipio,
                            Empresas.Configuracoes[emp].UsuarioWS,
                            Empresas.Configuracoes[emp].SenhaWS);

                        webTecnologia.ConsultarXml(NomeArquivoXML);
                        break;
                }

                if (IsInvocar(padraoNFSe, Servico, oObterNotaFiscal.cMunicipio))
                {

                    //Assinar o XML
                    var ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oObterNotaFiscal.cMunicipio));

                    //Invocar o método que envia o XML para o municipio
                    oInvocarObj.InvocarNFSe(wsProxy, pedNfseXML, NomeMetodoWS(Servico, oObterNotaFiscal.cMunicipio), cabecMsg, this,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).RetornoXML,
                        padraoNFSe, Servico, securityProtocolType);

                    /// grava o arquivo no FTP
                    var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).EnvioXML) +
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).RetornoXML);

                    if (File.Exists(filenameFTP))
                    {
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).EnvioXML,
                        Propriedade.ExtRetorno.NFSeXML_ERR, ex);
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

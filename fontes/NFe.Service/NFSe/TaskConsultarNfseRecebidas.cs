using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

#if _fw46

using System.ServiceModel;
using static NFe.Components.Security.SOAPSecurity;

#endif

namespace NFe.Service.NFSe
{
    public class TaskConsultarNfseRecebidas : TaskAbst
    {
        public TaskConsultarNfseRecebidas(string arquivo)
        {
            Servico = Servicos.NFSeConsultarNFSeRecebidas;
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
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                dadosXML = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(dadosXML.cMunicipio);
                WebServiceProxy wsProxy = null;
                object pedConsNfseRecebidas = null;
                if (!String.IsNullOrEmpty(Empresas.Configuracoes[emp].CertificadoPIN))
                {
                    new Unimake.Business.DFe.Utility.Certificate().CarregarPINA3(Empresas.Configuracoes[emp].X509Certificado, Empresas.Configuracoes[emp].CertificadoPIN);
                }



                if (IsUtilizaCompilacaoWs(padraoNFSe))
                {
                    wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, dadosXML.cMunicipio);
                    pedConsNfseRecebidas = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                }
                string cabecMsg = "";

                switch (padraoNFSe)
                {
                    case PadroesNFSe.PAULISTANA:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);

                        if (dadosXML.tpAmb == 1)
                            pedConsNfseRecebidas = new Components.PSaoPauloSP.LoteNFe();
                        else
                            throw new Exception("Município de São Paulo-SP não dispõe de ambiente de homologação para envio de NFS-e em teste.");

                        break;
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosXML.cMunicipio, dadosXML.tpAmb, dadosXML.tpEmis, padraoNFSe, Servico);

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, dadosXML.cMunicipio);

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedConsNfseRecebidas, NomeMetodoWS(Servico, dadosXML.cMunicipio), cabecMsg, this,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).RetornoXML,
                    padraoNFSe, Servico, securityProtocolType);

                ///
                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).EnvioXML) +
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).RetornoXML);

                if (File.Exists(filenameFTP))
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).EnvioXML,
                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRec).RetornoERR, ex);
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
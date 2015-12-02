using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFSe.Components;

namespace NFe.Service.NFSe
{
    public class TaskConsultarNfsePDF : TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oDadosPedNfsePDF;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeConsultarNFSePDF;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) + 
                                         Propriedade.ExtRetorno.NFSePDF_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedNfsePDF = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do municipio
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosPedNfsePDF.cMunicipio);
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedNfsePDF.cMunicipio, oDadosPedNfsePDF.tpAmb, oDadosPedNfsePDF.tpEmis, padraoNFSe);
                object pedNfsePNG = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                string cabecMsg = "";

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosPedNfsePDF.cMunicipio));

                //Invocar o método que envia o XML para o municipio
                oInvocarObj.InvocarNFSe(wsProxy, pedNfsePNG, NomeMetodoWS(Servico, oDadosPedNfsePDF.cMunicipio), cabecMsg, this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML,   //"-ped-nfsepdf", 
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML,   //"-nfsepdf", 
                                        padraoNFSe, Servico);

                ///
                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) + 
                                                Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML);
                if (File.Exists(filenameFTP))
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, 
                                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML,
                                                    Propriedade.ExtRetorno.NFSePDF_ERR, ex);
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

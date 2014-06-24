﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFSe.Components;
using NFe.Components.SystemPro;

namespace NFe.Service.NFSe
{
    public class TaskConsultarNfse : TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oDadosPedSitNfse;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Functions.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultarNfse;

            try
            {
                oDadosPedSitNfse = new DadosPedSitNfse(emp);
                //Ler o XML para pegar parâmetros de envio
                //LerXML ler = new LerXML();
                /*ler.*/
                PedSitNfse(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object pedLoteRps = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(/*ler.*/oDadosPedSitNfse.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosPedSitNfse.cMunicipio, /*ler.*/oDadosPedSitNfse.tpAmb, /*ler.*/oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        wsProxy.Betha = new Betha();
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosPedSitNfse.cMunicipio, /*ler.*/oDadosPedSitNfse.tpAmb, /*ler.*/oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosPedSitNfse.cMunicipio, /*ler.*/oDadosPedSitNfse.tpAmb, /*ler.*/oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.ISSNET:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.BLUMENAU_SC:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.BHISS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.GIF:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.DUETO:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.WEBISS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.PAULISTANA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.SALVADOR_BA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.PORTOVELHENSE:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"2.00\" xmlns:ns2=\"http://www.w3.org/2000/09/xmldsig#\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.PRONIN:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.DSF:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedSitNfse.cMunicipio, oDadosPedSitNfse.tpAmb, oDadosPedSitNfse.tpEmis, padraoNFSe, string.Empty);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SYSTEMPRO:
                        SystemPro syspro = new SystemPro((TpAmb)Empresa.Configuracoes[emp].tpAmb,
                            Empresa.Configuracoes[emp].PastaRetorno, Empresa.Configuracoes[emp].X509Certificado);
                        AssinaturaDigital ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosPedSitNfse.cMunicipio));
                        syspro.ConsultarNfse(NomeArquivoXML);
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }
                if (padraoNFSe != PadroesNFSe.IPM && padraoNFSe != PadroesNFSe.SYSTEMPRO)
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(/*ler.*/oDadosPedSitNfse.cMunicipio));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, /*ler.*/oDadosPedSitNfse.cMunicipio), cabecMsg, this, "-ped-sitnfse", "-sitnfse", padraoNFSe, Servico);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresa.Configuracoes[emp].PastaRetorno,
                        Path.GetFileName(NomeArquivoXML.Replace(Propriedade.ExtEnvio.PedSitNfse, Propriedade.ExtRetorno.SitNfse)));
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedSitNfse, Propriedade.ExtRetorno.SitNfse_ERR, ex);
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

        #region PedSitNfse()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por numero e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedSitNfse(string arquivoXML)
        {
            int emp = Functions.FindEmpresaByThread();
        }
        #endregion
    }
}

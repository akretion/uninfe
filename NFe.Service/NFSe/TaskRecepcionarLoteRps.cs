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
    public class TaskRecepcionarLoteRps : TaskAbst
    {
        #region Objeto com os dados do XML de lote rps
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do lote rps
        /// </summary>
        private DadosEnvLoteRps oDadosEnvLoteRps;
        #endregion

        public override void Execute()
        {
            int emp = Functions.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.RecepcionarLoteRps;

            try
            {
                oDadosEnvLoteRps = new DadosEnvLoteRps(emp);

                EnvLoteRps(emp, NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object envLoteRps = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosEnvLoteRps.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.IPM:
                        //código da cidade da receita federal, este arquivo pode ser encontrado em ~\uninfe\doc\Codigos_Cidades_Receita_Federal.xls</para>
                        //O código da cidade está hardcoded pois ainda está sendo usado apenas para campo mourão
                        IPM ipm = new IPM(Empresa.Configuracoes[emp].UsuarioWS, Empresa.Configuracoes[emp].SenhaWS, 7483, Empresa.Configuracoes[emp].PastaXmlRetorno);
                        ipm.EmitirNF(NomeArquivoXML, (TpAmb)Empresa.Configuracoes[emp].AmbienteCodigo);
                        break;

                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        wsProxy.Betha = new Betha();
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.ISSNET:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.ISSONLINE:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.BLUMENAU_SC:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        #region Encriptar tag <Assinatura>
                        EncryptAssinatura();
                        #endregion

                        break;

                    case PadroesNFSe.BHISS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.GIF:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.DUETO:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.WEBISS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.PAULISTANA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        #region Encriptar tag <Assinatura>
                        EncryptAssinatura();
                        #endregion
                        break;

                    case PadroesNFSe.SALVADOR_BA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.PORTOVELHENSE:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"2.00\" xmlns:ns2=\"http://www.w3.org/2000/09/xmldsig#\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.00</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.PRONIN:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.ISSONLINE4R:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, padraoNFSe, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.DSF:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        #region Encriptar tag <Assinatura>
                        EncryptAssinatura();
                        #endregion
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis, string.Empty);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.SYSTEMPRO:
                        SystemPro syspro = new SystemPro((TpAmb)Empresa.Configuracoes[emp].AmbienteCodigo,
                            Empresa.Configuracoes[emp].PastaXmlRetorno, Empresa.Configuracoes[emp].X509Certificado);
                        AssinaturaDigital ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosEnvLoteRps.cMunicipio));
                        syspro.EmiteNF(NomeArquivoXML);
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }

                if (padraoNFSe != PadroesNFSe.IPM && padraoNFSe != PadroesNFSe.SYSTEMPRO)
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosEnvLoteRps.cMunicipio));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, envLoteRps, NomeMetodoWS(Servico, oDadosEnvLoteRps.cMunicipio), cabecMsg, this, "-env-loterps", "-ret-loterps", padraoNFSe, Servico);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresa.Configuracoes[emp].PastaXmlRetorno,
                        Path.GetFileName(NomeArquivoXML.Replace(Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.RetLoteRps)));
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);

                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.RetLoteRps_ERR, ex);
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

        #region EncryptAssinatura()
        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        private void EncryptAssinatura()
        {
            ///danasa: 12/2013
            NFe.Validate.ValidarXML val = new Validate.ValidarXML(NomeArquivoXML, oDadosEnvLoteRps.cMunicipio);
            val.EncryptAssinatura(NomeArquivoXML);
            /*
            string arquivoXML = NomeArquivoXML;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList pedidoEnvioLoteRPSList = doc.GetElementsByTagName("PedidoEnvioLoteRPS");

            foreach (XmlNode pedidoEnvioLoteRPSNode in pedidoEnvioLoteRPSList)
            {
                XmlElement pedidoEnvioLoteRPSElemento = (XmlElement)pedidoEnvioLoteRPSNode;

                XmlNodeList rpsList = doc.GetElementsByTagName("RPS");

                foreach (XmlNode rpsNode in rpsList)
                {
                    XmlElement rpsElement = (XmlElement)rpsNode;


                    if (rpsElement.GetElementsByTagName("Assinatura").Count != 0)
                    {
                        //Encryptar a tag Assinatura
                        rpsElement.GetElementsByTagName("Assinatura")[0].InnerText = Criptografia.SignWithRSASHA1(Empresa.Configuracoes[Functions.FindEmpresaByThread()].X509Certificado,
                            rpsElement.GetElementsByTagName("Assinatura")[0].InnerText);
                    }
                }
            }

            //Salvar o XML com as alterações efetuadas
            doc.Save(arquivoXML);
*/
        }
        #endregion

        #region EnvLoteRps()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de lote rps e disponibiliza o conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void EnvLoteRps(int emp, string arquivoXML)
        {
            //int emp = Functions.FindEmpresaByThread();

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infEnvioList = doc.GetElementsByTagName("EnviarLoteRpsEnvio");

            foreach (XmlNode infEnvioNode in infEnvioList)
            {
                XmlElement infEnvioElemento = (XmlElement)infEnvioNode;
            }
        }
        #endregion

    }
}

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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.RecepcionarLoteRps;

            try
            {
                oDadosEnvLoteRps = new DadosEnvLoteRps(emp);
                //Ler o XML para pegar parâmetros de envio
                //LerXML ler = new LerXML();
                /*ler.*/EnvLoteRps(emp, NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object envLoteRps = null;
                string cabecMsg = "";
                //PadroesNFSe padraoNFSe = Functions.PadraoNFSe(/*ler.*/oDadosPedSitLoteRps.cMunicipio);
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(/*ler.*/oDadosEnvLoteRps.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosEnvLoteRps.cMunicipio, /*ler.*/oDadosEnvLoteRps.tpAmb, /*ler.*/oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        wsProxy.Betha = new Betha();
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosEnvLoteRps.cMunicipio, /*ler.*/oDadosEnvLoteRps.tpAmb, /*ler.*/oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.SALVADOR_BA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosEnvLoteRps.cMunicipio, /*ler.*/oDadosEnvLoteRps.tpAmb, /*ler.*/oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosEnvLoteRps.cMunicipio, /*ler.*/oDadosEnvLoteRps.tpAmb, /*ler.*/oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.ISSNET:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.ISSONLINE:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.BLUMENAU_SC:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosEnvLoteRps.cMunicipio, oDadosEnvLoteRps.tpAmb, oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosEnvLoteRps.cMunicipio));
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }


                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(/*ler.*/oDadosEnvLoteRps.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, envLoteRps, NomeMetodoWS(Servico, /*ler.*/oDadosEnvLoteRps.cMunicipio), cabecMsg, this, "-env-loterps", "-ret-loterps", padraoNFSe, Servico);
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

        #region EnvLoteRps()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de lote rps e disponibiliza o conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void EnvLoteRps(int emp, string arquivoXML)
        {
            //int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

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

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
    public class TaskCancelarNfse : TaskAbst
    {
        #region Objeto com os dados do XML de cancelamento de NFS-e
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        private DadosPedCanNfse oDadosPedCanNfse;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.CancelarNfse;

            try
            {
                oDadosPedCanNfse = new DadosPedCanNfse(emp);
                //Ler o XML para pegar parâmetros de envio
                //LerXML ler = new LerXML();
                /*ler.*/
                PedCanNfse(emp, NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object pedCanNfse = null;
                string cabecMsg = "";
                //PadroesNFSe padraoNFSe = Functions.PadraoNFSe(/*ler.*/oDadosPedSitLoteRps.cMunicipio);
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(/*ler.*/oDadosPedCanNfse.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosPedCanNfse.cMunicipio, /*ler.*/oDadosPedCanNfse.tpAmb, /*ler.*/oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosPedCanNfse.cMunicipio));
                        cabecMsg = ""; //Cancelamento ainda tá na versão 2.0 então não tem o cabecMsg
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        wsProxy.Betha = new Betha();
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosPedCanNfse.cMunicipio, /*ler.*/oDadosPedCanNfse.tpAmb, /*ler.*/oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosPedCanNfse.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, /*ler.*/oDadosPedCanNfse.cMunicipio, /*ler.*/oDadosPedCanNfse.tpAmb, /*ler.*/oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*ler.*/oDadosPedCanNfse.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.ISSNET:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedCanNfse.cMunicipio));
                        break;

                    case PadroesNFSe.ISSONLINE:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedCanNfse.cMunicipio));
                        break;

                    case PadroesNFSe.BLUMENAU_SC:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, oDadosPedCanNfse.cMunicipio));
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(/*ler.*/oDadosPedCanNfse.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedCanNfse, NomeMetodoWS(Servico, /*ler.*/oDadosPedCanNfse.cMunicipio), cabecMsg, this, "-ped-cannfse", "-cannfse", padraoNFSe, Servico);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.CanNfse_ERR, ex);
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

        #region PedCanNfse()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de cancelamento de NFS-e e disponibilizar conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        private void PedCanNfse(int emp, string arquivoXML)
        {
            //int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infCancList = doc.GetElementsByTagName("CancelarNfseEnvio");

            foreach (XmlNode infCancNode in infCancList)
            {
                XmlElement infCancElemento = (XmlElement)infCancNode;
            }
        }
        #endregion

    }
}

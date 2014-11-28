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
using NFe.Components.SystemPro;
using NFe.Components.SigCorp;
using NFe.Components.Fiorilli;

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
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.CancelarNfse;

            try
            {
                oDadosPedCanNfse = new DadosPedCanNfse(emp);
                //Ler o XML para pegar parâmetros de envio
                PedCanNfse(emp, NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosPedCanNfse.cMunicipio);
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedCanNfse.cMunicipio, oDadosPedCanNfse.tpAmb, oDadosPedCanNfse.tpEmis, padraoNFSe);
                object pedCanNfse = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                string cabecMsg = "";
                switch (padraoNFSe)
                {
                    case PadroesNFSe.IPM:
                        //código da cidade da receita federal, este arquivo pode ser encontrado em ~\uninfe\doc\Codigos_Cidades_Receita_Federal.xls</para>
                        //O código da cidade está hardcoded pois ainda está sendo usado apenas para campo mourão
                        IPM ipm = new IPM(Empresas.Configuracoes[emp].UsuarioWS, Empresas.Configuracoes[emp].SenhaWS, oDadosPedCanNfse.cMunicipio, Empresas.Configuracoes[emp].PastaXmlRetorno);
                        ipm.EmitirNF(NomeArquivoXML, (TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo, true);
                        break;

                    case PadroesNFSe.GINFES:
                        cabecMsg = ""; //Cancelamento ainda tá na versão 2.0 então não tem o cabecMsg
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresas.Configuracoes[emp].X509Certificado);
                        wsProxy.Betha = new Betha();
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.BLUMENAU_SC:
                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.BHISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.WEBISS:
                        cabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.PAULISTANA:
                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.DSF:
                        EncryptAssinatura();
                        break;

                    case PadroesNFSe.TECNOSISTEMAS:
                        cabecMsg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><cabecalho xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"20.01\" xmlns=\"http://www.nfse-tecnos.com.br/nfse.xsd\"><versaoDados>20.01</versaoDados></cabecalho>";
                        break;

                    case PadroesNFSe.FINTEL:
                        cabecMsg = "<cabecalho xmlns=\"http://iss.pontagrossa.pr.gov.br/Arquivos/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";
                        break;

                    case PadroesNFSe.SYSTEMPRO:
                        SystemPro syspro = new SystemPro((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno, Empresas.Configuracoes[emp].X509Certificado);
                        AssinaturaDigital ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosPedCanNfse.cMunicipio));
                        syspro.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SIGCORP_SIGISS:
                        SigCorp sigcorp = new SigCorp((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                            Empresas.Configuracoes[emp].PastaXmlRetorno,
                            Convert.ToInt32(oDadosPedCanNfse.cMunicipio));
                        sigcorp.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.FIORILLI:
                        Fiorilli fiorilli = new Fiorilli((TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                        Empresas.Configuracoes[emp].PastaXmlRetorno,
                        Convert.ToInt32(oDadosPedCanNfse.cMunicipio),
                        Empresas.Configuracoes[emp].UsuarioWS,
                        Empresas.Configuracoes[emp].SenhaWS);

                        AssinaturaDigital ass = new AssinaturaDigital();
                        ass.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosPedCanNfse.cMunicipio));

                        fiorilli.CancelarNfse(NomeArquivoXML);
                        break;

                    case PadroesNFSe.SMARAPD:
                        cabecMsg = "";
                        break;

                }

                if (padraoNFSe != PadroesNFSe.IPM && padraoNFSe != PadroesNFSe.SYSTEMPRO && padraoNFSe != PadroesNFSe.SIGCORP_SIGISS && padraoNFSe != PadroesNFSe.FIORILLI)
                {
                    //Assinar o XML
                    AssinaturaDigital ad = new AssinaturaDigital();
                    ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosPedCanNfse.cMunicipio));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.InvocarNFSe(wsProxy, pedCanNfse, NomeMetodoWS(Servico, oDadosPedCanNfse.cMunicipio), cabecMsg, this, "-ped-cannfse", "-cannfse", padraoNFSe, Servico);

                    ///
                    /// grava o arquivo no FTP
                    string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                        Path.GetFileName(NomeArquivoXML.Replace(Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.CanNfse)));
                    if (File.Exists(filenameFTP))
                        new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
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
            //int emp = Empresas.FindEmpresaByThread();

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infCancList = doc.GetElementsByTagName("CancelarNfseEnvio");

            foreach (XmlNode infCancNode in infCancList)
            {
                XmlElement infCancElemento = (XmlElement)infCancNode;
            }
        }
        #endregion

        #region EncryptAssinatura()
        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        private void EncryptAssinatura()
        {
            ///danasa: 12/2013
            NFe.Validate.ValidarXML val = new Validate.ValidarXML(NomeArquivoXML, oDadosPedCanNfse.cMunicipio);
            val.EncryptAssinatura(NomeArquivoXML);

            /*
            string arquivoXML = NomeArquivoXML;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList pedidoCancelamentoNFeList = doc.GetElementsByTagName("PedidoCancelamentoNFe");

            foreach (XmlNode pedidoCancelamentoNFeNode in pedidoCancelamentoNFeList)
            {
                XmlElement pedidoCancelamentoNFeElemento = (XmlElement)pedidoCancelamentoNFeNode;

                XmlNodeList detalheList = doc.GetElementsByTagName("Detalhe");

                foreach (XmlNode detalheNode in detalheList)
                {
                    XmlElement detalheElement = (XmlElement)detalheNode;


                    if (detalheElement.GetElementsByTagName("AssinaturaCancelamento").Count != 0)
                    {
                        //Encryptar a tag Assinatura
                        detalheElement.GetElementsByTagName("AssinaturaCancelamento")[0].InnerText = Criptografia.SignWithRSASHA1(Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado,
                            detalheElement.GetElementsByTagName("AssinaturaCancelamento")[0].InnerText);
                    }
                }
            }

            //Salvar o XML com as alterações efetuadas
            doc.Save(arquivoXML);*/
        }
        #endregion
    }
}

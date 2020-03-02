using NFe.Components;
using NFe.Exceptions;
using NFe.Settings;
using System;
using System.Threading;
using System.Xml;

namespace NFe.Service
{
    public class TaskNFeRecepcao : TaskAbst
    {
        public TaskNFeRecepcao(string arquivo)
        {
            Servico = Servicos.NFeEnviarLote;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public TaskNFeRecepcao(XmlDocument conteudoXML)
        {
            Servico = Servicos.NFeEnviarLote;

            ConteudoXML = conteudoXML;
            ConteudoXML.PreserveWhitespace = false;
            NomeArquivoXML = Empresas.Configuracoes[Empresas.FindEmpresaByThread()].PastaXmlEnvio + "\\temp\\" +
                conteudoXML.GetElementsByTagName(TpcnResources.idLote.ToString())[0].InnerText + Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML;
        }

        #region Classe com os dados do XML do retorno do envio do Lote de NFe

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        private DadosRecClass dadosRec;

        #endregion Classe com os dados do XML do retorno do envio do Lote de NFe

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            FluxoNfe oFluxoNfe = new FluxoNfe();
            LerXML oLer = new LerXML();

            try
            {
                dadosRec = new DadosRecClass();

                //Ler o XML de Lote para pegar o número do lote que está sendo enviado
                oLer.Nfe(ConteudoXML);

                var idLote = oLer.oDadosNfe.idLote;

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp,
                    Convert.ToInt32(oLer.oDadosNfe.cUF),
                    Convert.ToInt32(oLer.oDadosNfe.tpAmb),
                    Convert.ToInt32(oLer.oDadosNfe.tpEmis),
                    oLer.oDadosNfe.versao,
                    oLer.oDadosNfe.mod,
                    0);

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(Convert.ToInt32(oLer.oDadosNfe.cUF), Convert.ToInt32(oLer.oDadosNfe.tpAmb), Convert.ToInt32(oLer.oDadosNfe.tpEmis), Servico);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                object oRecepcao = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                object oCabecMsg = null;
                if (oLer.oDadosNfe.versao != "4.00")
                {
                    oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(Convert.ToInt32(oLer.oDadosNfe.cUF), Servico));
                    wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), oLer.oDadosNfe.cUF);
                    wsProxy.SetProp(oCabecMsg, TpcnResources.versaoDados.ToString(), oLer.oDadosNfe.versao);
                }

                //Invocar o método que envia o XML para o SEFAZ
                if (Empresas.Configuracoes[emp].IndSinc && oLer.oDadosNfe.indSinc)
                {
                    //Não posso gerar o arquivo na pasta de retorno através do método Invocar, por isso não estou colocando os dois ultimos parâmetros com a definição dos prefixos dos arquivos. O arquivo de retorno no processo síncrono deve acontecer somente depois de finalizado o processo da nota, ou gera problemas. Wandrey 11/06/2015
                    oInvocarObj.Invocar(wsProxy,
                                        oRecepcao,
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg,
                                        this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).RetornoXML,
                                        false,
                                        securityProtocolType);

                    Protocolo(vStrXmlRetorno);
                }
                else
                {
                    oInvocarObj.Invocar(wsProxy,
                                        oRecepcao,
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg,
                                        this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML,
                                        Propriedade.ExtRetorno.Rec,
                                        true,
                                        securityProtocolType);

                    Recibo(vStrXmlRetorno, emp);
                }

                if (dadosRec.cStat == "104") //Lote processado - Processo da NFe Síncrono - Wandrey 13/03/2014
                {
                    FinalizarNFeSincrono(vStrXmlRetorno, emp, oLer.oDadosNfe.chavenfe);

                    oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).RetornoXML, vStrXmlRetorno);
                }
                else if (dadosRec.cStat == "103") //Lote recebido com sucesso - Processo da NFe Assíncrono
                {
                    if (dadosRec.tMed > 0)
                        Thread.Sleep(dadosRec.tMed * 1000);

                    //Atualizar o número do recibo no XML de controle do fluxo de notas enviadas
                    oFluxoNfe.AtualizarTag(oLer.oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.tMed, dadosRec.tMed.ToString());
                    oFluxoNfe.AtualizarTagRec(idLote, dadosRec.nRec);

                    XmlDocument xmlPedRec = oGerarXML.XmlPedRecNFe(dadosRec.nRec, oLer.oDadosNfe.versao, oLer.oDadosNfe.mod, emp);
                    TaskNFeRetRecepcao nfeRetRecepcao = new TaskNFeRetRecepcao(xmlPedRec);
                    nfeRetRecepcao.chNFe = oLer.oDadosNfe.chavenfe;
                    nfeRetRecepcao.Execute();
                }
                else if (Convert.ToInt32(dadosRec.cStat) > 200 ||
                         Convert.ToInt32(dadosRec.cStat) == 108 || //Verifica se o servidor de processamento está paralisado momentaneamente. Wandrey 13/04/2012
                         Convert.ToInt32(dadosRec.cStat) == 109) //Verifica se o servidor de processamento está paralisado sem previsão. Wandrey 13/04/2012
                {
                    if (Empresas.Configuracoes[emp].IndSinc && oLer.oDadosNfe.indSinc)
                    {
                        // OPS!!! Processo sincrono rejeição da SEFAZ, temos que gravar o XML para o ERP, pois no processo síncrono isso não pode ser feito dentro do método Invocar
                        oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).RetornoXML/*.ExtRetorno.ProRec_XML*/, vStrXmlRetorno);
                    }
                    //Se o status do retorno do lote for maior que 200 ou for igual a 108 ou 109,
                    //vamos ter que excluir a nota do fluxo, porque ela foi rejeitada pelo SEFAZ
                    //Primeiro vamos mover o xml da nota da pasta EmProcessamento para pasta de XML´s com erro e depois a tira do fluxo
                    //Wandrey 30/04/2009
                    oAux.MoveArqErro(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + oFluxoNfe.LerTag(oLer.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe));
                    oFluxoNfe.ExcluirNfeFluxo(oLer.oDadosNfe.chavenfe);
                }

                //Deleta o arquivo de lote
                Functions.DeletarArquivo(NomeArquivoXML);
            }
            catch (ExceptionEnvioXML ex)
            {
                TrataException(emp, ex, oLer.oDadosNfe.chavenfe);
            }
            catch (ExceptionSemInternet ex)
            {
                TrataException(emp, ex, oLer.oDadosNfe.chavenfe);
            }
            catch (Exception ex)
            {
                TrataException(emp, ex, oLer.oDadosNfe.chavenfe);
            }
        }

        #endregion Execute

        /// <summary>
        /// Tratar exceção
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        /// <param name="ex">Objeto com a exception</param>
        private void TrataException(int emp, Exception ex, string chavenfe)
        {
            try
            {
                new FluxoNfe().ExcluirNfeFluxo(chavenfe);

                //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                switch (Empresas.Configuracoes[emp].IndSinc)
                {
                    case true:
                        LerXML oLer = new LerXML();
                        oLer.Nfe(ConteudoXML);
                        if (oLer.oDadosNfe.indSinc)
                            TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLot, Propriedade.ExtRetorno.ProRec_ERR, ex);
                        else
                            goto default;

                        break;

                    default:
                        TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.ExtRetorno.Rec_ERR, ex);
                        break;
                }
            }
            catch
            {
                //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                //Wandrey 16/03/2010
            }
        }

        #region Recibo

        /// <summary>
        /// Faz a leitura do XML do Recibo do lote enviado e disponibiliza os valores
        /// de algumas tag´s
        /// </summary>
        /// <param name="strXml">String contendo o XML</param>
        private void Recibo(string strXml, int emp)
        {
            dadosRec.cStat =
                dadosRec.nRec = string.Empty;
            dadosRec.tMed = 0;

            XmlDocument xml = new XmlDocument();
            xml.Load(Functions.StringXmlToStream(strXml));

            XmlNodeList retEnviNFeList = xml.GetElementsByTagName("retEnviNFe");

            foreach (XmlNode retEnviNFeNode in retEnviNFeList)
            {
                XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                dadosRec.cStat = retEnviNFeElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;

                XmlNodeList infRecList = xml.GetElementsByTagName("infRec");

                foreach (XmlNode infRecNode in infRecList)
                {
                    XmlElement infRecElemento = (XmlElement)infRecNode;

                    dadosRec.nRec = infRecElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText;
                    dadosRec.tMed = Convert.ToInt32(infRecElemento.GetElementsByTagName(TpcnResources.tMed.ToString())[0].InnerText);

                    if (dadosRec.tMed > 15)
                        dadosRec.tMed = 15;

                    if (dadosRec.tMed <= 0)
                        dadosRec.tMed = Empresas.Configuracoes[emp].TempoConsulta;
                }
            }
        }

        #endregion Recibo

        #region Protocolo()

        /// <summary>
        /// Faz leitura do protocolo quando configurado para processo Síncrono
        /// </summary>
        /// <param name="strXml">String contendo o XML</param>
        private void Protocolo(string strXml)
        {
            dadosRec.cStat =
                dadosRec.nRec = string.Empty;
            dadosRec.tMed = 0;

            XmlDocument xml = new XmlDocument();
            xml.Load(Functions.StringXmlToStream(strXml));

            XmlNodeList retEnviNFeList = xml.GetElementsByTagName(xml.FirstChild.Name);

            foreach (XmlNode retEnviNFeNode in retEnviNFeList)
            {
                XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                dadosRec.cStat = retEnviNFeElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;

                if (retEnviNFeElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0] != null)
                    dadosRec.nRec = retEnviNFeElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText;
            }
        }

        #endregion Protocolo()

        #region FinalizarNFeSincrono()

        /// <summary>
        /// Finalizar a NFe no processo Síncrono
        /// </summary>
        /// <param name="xmlRetorno">Conteúdo do XML retornado da SEFAZ</param>
        /// <param name="emp">Código da empresa para buscar as configurações</param>
        private void FinalizarNFeSincrono(string xmlRetorno, int emp, string chNFe)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(Functions.StringXmlToStream(xmlRetorno));

            XmlNodeList protNFe = xml.GetElementsByTagName("protNFe");

            FluxoNfe fluxoNFe = new FluxoNfe();

            TaskNFeRetRecepcao retRecepcao = new TaskNFeRetRecepcao();
            retRecepcao.chNFe = chNFe;
            retRecepcao.FinalizarNFe(protNFe, fluxoNFe, emp, ConteudoXML);
        }

        #endregion FinalizarNFeSincrono()
    }
}
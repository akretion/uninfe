using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;

namespace NFe.Service
{
    public class TaskCancelamento : TaskAbst
    {
        #region Classe com os dados do XML da consulta do pedido de cancelamento
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        private DadosPedCanc oDadosPedCanc;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.CancelarNFe;

            try
            {
                oDadosPedCanc = new DadosPedCanc(emp);
                //Ler o XML para pegar parâmetros de envio
                //LerXML oLer = new LerXML();
                ///*oLer.*/
                PedCanc(emp, NomeArquivoXML);

                if (this.vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.CancelarNFe, emp, /*oLer.*/oDadosPedCanc.cUF, /*oLer.*/oDadosPedCanc.tpAmb, /*oLer.*/oDadosPedCanc.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oCancelamento = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*oLer.*/oDadosPedCanc.cUF));
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(oDadosPedCanc.cUF));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", /*oLer.*/oDadosPedCanc.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLCanc);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(/*oLer.*/oDadosPedCanc.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oCancelamento, NomeMetodoWS(Servico, /*oLer.*/oDadosPedCanc.cUF), oCabecMsg, this, "-ped-can", "-can");

                    //Ler o retorno do webservice
                    LerRetornoCanc(NomeArquivoXML, this.vStrXmlRetorno, this.oGerarXML);
                }
                else
                {
                    //Gerar o XML de solicitação de cancelamento de uma NFe a partir do TXT Gerado pelo ERP
                    oGerarXML.Cancelamento(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                        /*oLer.*/oDadosPedCanc.tpAmb,
                        /*oLer.*/oDadosPedCanc.tpEmis,
                        /*oLer.*/oDadosPedCanc.chNFe,
                        /*oLer.*/oDadosPedCanc.nProt,
                        /*oLer.*/oDadosPedCanc.xJust);
                }
            }
            catch (Exception ex)
            {
                string ExtRet = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtRet = Propriedade.ExtEnvio.PedCan_XML;
                else //Se for TXT
                    ExtRet = Propriedade.ExtEnvio.PedCan_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.Can_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 09/03/2010
                }
            }
            finally
            {
                try
                {
                    if (!this.vXmlNfeDadosMsgEhXML) //Se for o TXT para ser transformado em XML, vamos excluir o TXT depois de gerado o XML
                        Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region PedCanc()
        /// <summary>
        /// PedCan(string cArquivoXML)
        /// </summary>
        /// <param name="cArquivoXML"></param>
        private void PedCanc(int emp, string cArquivoXML)
        {
            //int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            this.oDadosPedCanc.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.oDadosPedCanc.tpEmis = Empresa.Configuracoes[emp].tpEmis;

            if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
            {
                //      tpAmb|2
                //      chNFe|35080699999090910270550000000000011234567890
                //      nProt|135080000000001
                //      xJust|Teste do WS de Cancelamento
                //      tpEmis|1                                    <<< opcional >>>
                List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
                foreach (string cTexto in cLinhas)
                {
                    string[] dados = cTexto.Split('|');
                    switch (dados[0].ToLower())
                    {
                        case "tpamb":
                            this.oDadosPedCanc.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "chnfe":
                            this.oDadosPedCanc.chNFe = dados[1].Trim();
                            break;
                        case "nprot":
                            this.oDadosPedCanc.nProt = dados[1].Trim();
                            break;
                        case "xjust":
                            this.oDadosPedCanc.xJust = dados[1].Trim();
                            break;
                        case "tpemis":
                            this.oDadosPedCanc.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                    }
                }
            }
            else
            {
                //<?xml version="1.0" encoding="UTF-8"?>
                //<cancNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
                //  <infCanc Id="ID35080699999090910270550000000000011234567890">
                //      <tpAmb>2</tpAmb>
                //      <xServ>CANCELAR</xServ>
                //      <chNFe>35080699999090910270550000000000011234567890</chNFe>
                //      <nProt>135080000000001</nProt>
                //      <xJust>Teste do WS de Cancelamento</xJust>
                //      <tpEmis>1</tpEmis>                                      <<< opcional >>>
                //  </infCanc>}
                //</cancNFe>
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList infCancList = doc.GetElementsByTagName("infCanc");

                foreach (XmlNode infCancNode in infCancList)
                {
                    XmlElement infCancElemento = (XmlElement)infCancNode;

                    this.oDadosPedCanc.tpAmb = Convert.ToInt32("0" + infCancElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            if (infCancElemento.GetElementsByTagName("chCTe").Count != 0)
                                this.oDadosPedCanc.chNFe = infCancElemento.GetElementsByTagName("chCTe")[0].InnerText;
                            break;

                        case TipoAplicativo.Nfe:
                            if (infCancElemento.GetElementsByTagName("chNFe").Count != 0)
                                this.oDadosPedCanc.chNFe = infCancElemento.GetElementsByTagName("chNFe")[0].InnerText;
                            break;

                        default:
                            break;
                    }

                    ///
                    /// danasa 12-9-2009
                    /// 
                    if (infCancElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        this.oDadosPedCanc.tpEmis = Convert.ToInt16(infCancElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement["infCanc"].RemoveChild(infCancElemento.GetElementsByTagName("tpEmis")[0]);

                        /// salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
                }
            }
        }
        #endregion

        #region LerRetornoCanc()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do cancelamento
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void LerRetornoCanc(string NomeArquivoXML, string vStrXmlRetorno, GerarXML oGerarXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();

            try
            {
                MemoryStream msXml = Functions.StringXmlToStream(vStrXmlRetorno);
                doc.Load(msXml);

                XmlNodeList retCancNFeList = null;

                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        retCancNFeList = doc.GetElementsByTagName("retCancCTe");
                        break;
                    case TipoAplicativo.Nfe:
                        retCancNFeList = doc.GetElementsByTagName("retCancNFe");
                        break;
                    default:
                        break;
                }

                foreach (XmlNode retCancNFeNode in retCancNFeList)
                {
                    XmlElement retCancNFeElemento = (XmlElement)retCancNFeNode;

                    XmlNodeList infCancList = retCancNFeElemento.GetElementsByTagName("infCanc");

                    foreach (XmlNode infCancNode in infCancList)
                    {
                        XmlElement infCancElemento = (XmlElement)infCancNode;

                        if (infCancElemento.GetElementsByTagName("cStat")[0].InnerText == "101" ||  //Cancelamento Homologado
                            infCancElemento.GetElementsByTagName("cStat")[0].InnerText == "151")    //Cancelamento fora do prazo
                        {
                            string retCancNFe = retCancNFeNode.OuterXml;

                            oGerarXML.XmlDistCanc(NomeArquivoXML, retCancNFe);
                            ///
                            /// danasa 9-2009
                            /// pega a data da emissão da nota para mover os XML's para a pasta de origem da NFe
                            /// 
                            string cChaveNFe = string.Empty;
                            switch (Propriedade.TipoAplicativo)
                            {
                                case TipoAplicativo.Cte:
                                    cChaveNFe = infCancElemento.GetElementsByTagName("chCTe")[0].InnerText;
                                    break;
                                case TipoAplicativo.Nfe:
                                    cChaveNFe = infCancElemento.GetElementsByTagName("chNFe")[0].InnerText;
                                    break;
                                default:
                                    break;
                            }

                            //Move o arquivo de Distribuição da pasta EmProcessamento para a pasta de enviados autorizados
                            string strNomeArqProcCancNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                            Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedCan_XML) + Propriedade.ExtRetorno.ProcCancNFe;

                            if (Empresa.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe)
                            {
                                string folderNFe = oGerarXML.OndeNFeEstaGravada(emp, cChaveNFe);
                                if (!string.IsNullOrEmpty(folderNFe))
                                {
                                    Functions.Move(strNomeArqProcCancNFe, Path.Combine(folderNFe, Path.GetFileName(strNomeArqProcCancNFe)));
                                    Functions.Move(NomeArquivoXML, Path.Combine(folderNFe, Path.GetFileName(NomeArquivoXML)));

                                    strNomeArqProcCancNFe = "";
                                }
                            }
                            if (strNomeArqProcCancNFe != "")
                            {
                                //
                                //TODO: Cancelamento - Se for pasta por dia, tem que pegar a data de dentro do XML da NFe
                                DateTime dtEmissaoNFe = new DateTime(Convert.ToInt16("20" + cChaveNFe.Substring(2, 2)), Convert.ToInt16(cChaveNFe.Substring(4, 2)), 1);
                                if (Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString() != "AM")
                                {
                                    dtEmissaoNFe = Functions.GetDateTime/*Convert.ToDateTime*/(infCancElemento.GetElementsByTagName("dhRecbto")[0].InnerText);
                                }

                                TFunctions.MoverArquivo(strNomeArqProcCancNFe, PastaEnviados.Autorizados, dtEmissaoNFe);

                                //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                                TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, dtEmissaoNFe);
                            }
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            Functions.DeletarArquivo(NomeArquivoXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region GerarXmlDistCanc()
        /// <summary>
        /// Gera o XML de distribuição do cancelamento dos arquivos -ped-can.xml que estão parados na pasta EmProcessamento
        /// </summary>
        /// <param name="chaveNFe">Chave da nfe/cte que está sendo consultada a situação</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 12/01/2012
        /// </remarks>
        public void GerarXmlDistCanc(string chaveNFe, string vStrXmlRetorno, GerarXML oGerarXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                string[] files = Directory.GetFiles(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString(),
                                 "*" + Propriedade.ExtEnvio.PedCan_XML,
                                 SearchOption.TopDirectoryOnly);

                foreach (string file in files)
                {
                    if (!Functions.FileInUse(file))
                    {
                        System.Xml.XmlDocument xmlCanc = new System.Xml.XmlDocument();
                        xmlCanc.Load(file);

                        string chaveNFeCanc = string.Empty;
                        if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                        {
                            chaveNFeCanc = xmlCanc.GetElementsByTagName("chCTe")[0].InnerText;
                        }
                        else
                        {
                            chaveNFeCanc = xmlCanc.GetElementsByTagName("chNFe")[0].InnerText;
                        }

                        if (chaveNFeCanc == chaveNFe)
                        {
                            LerRetornoCanc(file, vStrXmlRetorno, oGerarXML);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

    }
}

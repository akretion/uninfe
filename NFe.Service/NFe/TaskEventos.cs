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
    public class TaskEventos : TaskAbst
    {
        #region Classe com os dados do XML do registro de eventos
        private DadosenvEvento oDadosEnvEvento;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            Servico = Servicos.Nulo;

            try
            {
                oDadosEnvEvento = new DadosenvEvento();
                //Ler o XML para pegar parâmetros de envio
                EnvEvento(emp, NomeArquivoXML);

                int currentEvento = Convert.ToInt32(oDadosEnvEvento.eventos[0].tpEvento);
                ///
                /// mudei para aqui cajo haja erro e qdo for gravar o arquivo de erro precisamos saber qual o servico
                switch (currentEvento)
                {
                    case 110111:
                        Servico = Servicos.EnviarEventoCancelamento;
                        break;
                    case 110110:
                        Servico = Servicos.EnviarCCe;
                        break;
                    default:
                        Servico = Servicos.EnviarManifestacao;
                        break;
                }
                foreach (Evento item in oDadosEnvEvento.eventos)
                    if (currentEvento != Convert.ToInt32(item.tpEvento))
                        throw new Exception(string.Format("Não é possivel mesclar tipos de eventos dentro de um mesmo xml/txt de eventos. O tipo de evento neste xml/txt é {0}", currentEvento));

                //Pegar o estado da chave, pois na cOrgao pode vir o estado 91 - Wandreuy 22/08/2012
                //int cOrgao = Convert.ToInt32(oDadosEnvEvento.eventos[0].chNFe.Substring(0, 2));
                int cOrgao = oDadosEnvEvento.eventos[0].cOrgao;

                //if (cOrgao == 90 || cOrgao == 91)   //Amb.Nacional
                //{
                //cOrgao = Convert.ToInt32(oDadosEnvEvento.eventos[0].chNFe.Substring(0, 2));//<<< 7/2012

                ///Estados que utilizam a SVAN: ES, MA, PA, PI, RN
                ///Devem utilizar 91
                ///
                ///para testar
                /*switch (cOrgao)
                {
                    case 32:
                    case 21:
                    case 15:
                    case 22:
                    case 24:
                        cOrgao = 91;
                        break;
                }*/
                //}
                //Definir o serviço que será executado para a classe

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(
                        Servico,
                        emp,
                        cOrgao,
                        oDadosEnvEvento.eventos[0].tpAmb,
                        Empresa.Configuracoes[emp].tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoEvento;
                    if (/*oLer.*/oDadosEnvEvento.eventos[0].cOrgao == 52)
                    {
                        oRecepcaoEvento = wsProxy.CriarObjeto("NfeRecepcaoEvento");
                    }
                    else
                    {
                        oRecepcaoEvento = wsProxy.CriarObjeto("RecepcaoEvento");
                    }
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(cOrgao, Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", cOrgao.ToString());
                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvCCe);
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvCancelamento);
                            break;
                        default:
                            wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvManifestacao);
                            break;
                    }

                    //Criar objeto da classe de assinatura digital
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, cOrgao);

                    //Invocar o método que envia o XML para o SEFAZ
                    string xmlExtEnvio = string.Empty;
                    string xmlExtRetorno = string.Empty;
                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            xmlExtEnvio = Propriedade.ExtEnvio.EnvCCe_XML.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.retEnvCCe_XML.Replace(".xml", "");
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            xmlExtEnvio = Propriedade.ExtEnvio.EnvCancelamento_XML.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.retCancelamento_XML.Replace(".xml", "");
                            break;
                        default:
                            xmlExtEnvio = Propriedade.ExtEnvio.EnvManifestacao_XML.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.retManifestacao_XML.Replace(".xml", "");
                            break;
                    }
                    oInvocarObj.Invocar(wsProxy, oRecepcaoEvento, "nfeRecepcaoEvento", oCabecMsg, this, xmlExtEnvio, xmlExtRetorno);

                    //Ler o retorno
                    LerRetornoEvento(emp);
                }
                else
                {
                    // Gerar o XML de eventos a partir do TXT gerado pelo ERP
                    string xmlFileExt = string.Empty;
                    string xmlFileExtTXT = string.Empty;
                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            xmlFileExt = Propriedade.ExtEnvio.EnvCCe_XML;
                            xmlFileExtTXT = Propriedade.ExtEnvio.EnvCCe_TXT;
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            xmlFileExt = Propriedade.ExtEnvio.EnvCancelamento_XML;
                            xmlFileExtTXT = Propriedade.ExtEnvio.EnvCancelamento_TXT;
                            break;
                        default:
                            xmlFileExt = Propriedade.ExtEnvio.EnvManifestacao_XML;
                            xmlFileExtTXT = Propriedade.ExtEnvio.EnvManifestacao_TXT;
                            break;
                    }
                    oGerarXML.EnvioEvento(Functions.ExtrairNomeArq(NomeArquivoXML, xmlFileExtTXT) + xmlFileExt, oDadosEnvEvento);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (Servico == Servicos.Nulo)
                    {
                        ///
                        /// pode ter vindo de um txt e houve erro
                        if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCCe_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCCe_TXT))
                            Servico = Servicos.EnviarCCe;
                        else
                            if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvManifestacao_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvManifestacao_TXT))
                                Servico = Servicos.EnviarManifestacao;
                            else
                                if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCancelamento_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCancelamento_TXT))
                                    Servico = Servicos.EnviarEventoCancelamento;
                    }
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    string ExtRet = string.Empty;
                    string ExtRetorno = string.Empty;

                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvCCe_XML : Propriedade.ExtEnvio.EnvCCe_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.retEnvCCe_ERR;
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvCancelamento_XML : Propriedade.ExtEnvio.EnvCancelamento_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.retCancelamento_ERR;
                            break;
                        case Servicos.EnviarManifestacao:
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvManifestacao_XML : Propriedade.ExtEnvio.EnvManifestacao_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.retManifestacao_ERR;
                            break;
                        default:
                            throw new Exception("Nao pode identificar o tipo de serviço para o arquivo: " + NomeArquivoXML);
                    }
                    if (ExtRetorno != string.Empty)
                    {
                        TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtRetorno, ex);
                    }
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
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de evento, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region EnvEvento()
        private void EnvEvento(int emp, string arquivoXML)
        {
            ///
            /// danasa 6/2011
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        break;

                    case TipoAplicativo.Nfe:
                        ///<<<<EVENTO DE CARTA DE CORRECAO>>>>
                        ///idLote|000000000015255
                        ///evento|1
                        ///Id|ID1101103511031029073900013955001000000001105112804101                    <<opcional
                        ///cOrgao|35
                        ///tpAmb|2
                        ///CNPJ|10290739000139 
                        ///    ou
                        ///CPF|80531385800
                        ///chNFe|35110310290739000139550010000000011051128041
                        ///dhEvento|2011-03-03T08:06:00
                        ///tpEvento|110110
                        ///nSeqEvento|1
                        ///verEvento|1.00
                        ///descEvento|Carta de Correção                                                 <<opcional
                        ///xCorrecao|Texto de teste para Carta de Correção. Conteúdo do campo xCorrecao.
                        ///xCondUso|A Carta de Correção é disciplinada pelo § 1º-A do art. ..........   <<opcional
                        ///evento|2
                        ///Id|ID1101103511031029073900013955001000000001105112804102
                        ///...
                        ///evento|20    <<MAXIMO
                        ///Id|ID1101103511031029073900013955001000000001105112804103
                        ///...

                        ///<<<<EVENTO DE CANCELAMENTO>>>>
                        /// idLote|000000000015255
                        /// evento|1
                        /// Id|ID1101113511031029073900013955001000000001105112804102
                        /// cOrgao|35
                        /// tpAmb|2
                        /// CNPJ|10290739000139 
                        ///    ou
                        /// CPF|80531385800
                        /// chNFe|35110310290739000139550010000000011051128041
                        /// dhEvento|2011-03-03T08:06:00-03:00
                        /// tpEvento|110111
                        /// nSeqEvento|1
                        /// verEvento|1.00
                        /// descEvento|Cancelamento                                                      <<opcional
                        /// xJust|Justificativa do cancelamento
                        /// nProt|010101010101010

                        ///<<<<EVENTO DE CONFIRMACAO DA OPERACAO>>>>
                        ///idLote|000000000015255
                        ///evento|1
                        ///Id|ID2102003511031029073900013955001000000001105112804102
                        ///cOrgao|35
                        ///tpAmb|2
                        ///CNPJ|10290739000139 
                        ///    ou
                        ///CPF|80531385800
                        ///chNFe|35110310290739000139550010000000011051128041
                        ///dhEvento|2011-03-03T08:06:00-03:00
                        ///tpEvento|210200
                        ///nSeqEvento|1
                        ///verEvento|1.00
                        ///descEvento|Confirmacao da Operacao                                           <<opcional
                        ///xJust|Justificativa.....

                        /// ------------------------------------
                        ///<<<<EVENTO DE CIENCIA DA OPERACAO>>>>
                        ///idLote|000000000015255
                        ///evento|1
                        ///Id|ID2102103511031029073900013955001000000001105112804102
                        ///cOrgao|35
                        ///tpAmb|2
                        ///CNPJ|10290739000139 
                        ///    ou
                        ///CPF|80531385800
                        ///chNFe|35110310290739000139550010000000011051128041
                        ///dhEvento|2011-03-03T08:06:00-03:00
                        ///tpEvento|210210
                        ///nSeqEvento|1
                        ///verEvento|1.00
                        ///descEvento|Ciencia da Operacao                                               <<opcional

                        /// --------------------------------------------
                        ///<<<<EVENTO DE DESCONHECIMENTO DA OPERACAO>>>>
                        ///idLote|000000000015255
                        ///evento|1
                        ///Id|ID2102203511031029073900013955001000000001105112804102
                        ///cOrgao|35
                        ///tpAmb|2
                        ///CNPJ|10290739000139 
                        ///    ou
                        ///CPF|80531385800
                        ///chNFe|35110310290739000139550010000000011051128041
                        ///dhEvento|2011-03-03T08:06:00-03:00
                        ///tpEvento|210220
                        ///nSeqEvento|1
                        ///verEvento|1.00
                        ///descEvento|Desconhecimento da Operacao                                        <<opcional
                        ///xJust|Justificativa.....

                        /// --------------------------------------------
                        ///<<<<EVENTO DE OPERACAO NAO REALIZADA>>>>
                        ///idLote|000000000015255
                        ///evento|1
                        ///Id|ID2102403511031029073900013955001000000001105112804102
                        ///cOrgao|35
                        ///tpAmb|2
                        ///CNPJ|10290739000139 
                        ///    ou
                        ///CPF|80531385800
                        ///chNFe|35110310290739000139550010000000011051128041
                        ///dhEvento|2011-03-03T08:06:00-03:00
                        ///tpEvento|210240
                        ///nSeqEvento|1
                        ///verEvento|1.00
                        ///descEvento|Operacao nao realizada                                            <<opcional

                        List<string> cLinhas = Functions.LerArquivo(arquivoXML);
                        foreach (string cTexto in cLinhas)
                        {
                            string[] dados = cTexto.Split('|');
                            if (dados.GetLength(0) == 1) continue;

                            switch (dados[0].ToLower())
                            {
                                case "idlote":
                                    this.oDadosEnvEvento.idLote = dados[1].Trim();
                                    break;
                                case "evento":
                                    this.oDadosEnvEvento.eventos.Add(new Evento());
                                    break;
                                case "id":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].Id = dados[1].Trim();
                                    break;
                                case "corgao":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].cOrgao = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "tpamb":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "cnpj":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].CNPJ = dados[1].Trim();
                                    break;
                                case "cpf":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].CPF = dados[1].Trim();
                                    break;
                                case "chnfe":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].chNFe = dados[1].Trim();
                                    break;
                                case "dhevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].dhEvento = dados[1].Trim();
                                    break;
                                case "tpevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].tpEvento = dados[1].Trim();
                                    break;
                                case "nseqevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].nSeqEvento = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "verevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].verEvento = dados[1].Trim();
                                    break;
                                case "descevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].descEvento = dados[1].Trim();
                                    break;
                                case "xcorrecao":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].xCorrecao = dados[1].Trim();
                                    break;
                                case "xconduso":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].xCondUso = dados[1].Trim();
                                    break;
                                case "xjust":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].xJust = dados[1].Trim();
                                    break;
                                case "nprot":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].nProt = dados[1].Trim();
                                    break;
                            }
                        }
                        foreach (Evento evento in this.oDadosEnvEvento.eventos)
                        {
                            switch (evento.tpEvento)
                            {
                                case "110110":
                                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = "Carta de Correcao";
                                    break;
                                case "110111":
                                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = "Cancelamento";
                                    evento.nSeqEvento = 1;
                                    break;
                                case "210200":
                                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = "Confirmacao da Operacao";
                                    evento.nSeqEvento = 1;
                                    break;
                                case "210210":
                                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = "Ciencia da Operacao";
                                    evento.nSeqEvento = 1;
                                    break;
                                case "210220":
                                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = "Desconhecimento da Operacao";
                                    evento.nSeqEvento = 1;
                                    break;
                                case "210240":
                                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = "Operacao nao Realizada";
                                    evento.nSeqEvento = 1;
                                    break;
                            }
                            if (string.IsNullOrEmpty(evento.verEvento))
                                evento.verEvento = "1.00";

                            if (evento.tpAmb == 0)
                                evento.tpAmb = Empresa.Configuracoes[emp].tpAmb;

                            if (evento.cOrgao == 0)
                                evento.cOrgao = Convert.ToInt32(evento.chNFe.Substring(0, 2));

                            if (string.IsNullOrEmpty(evento.Id))
                                evento.Id = "ID" + evento.tpEvento + evento.chNFe + evento.nSeqEvento.ToString("00");

                            if (string.IsNullOrEmpty(evento.xCondUso))
                                if (evento.descEvento == "Carta de Correcao")
                                    evento.xCondUso =
                                        "A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, " +
                                        "de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido na emissao de " +
                                        "documento fiscal, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o " +
                                        "valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da " +
                                        "operacao ou da prestacao; II - a correcao de dados cadastrais que implique mudanca do remetente " +
                                        "ou do destinatario; III - a data de emissao ou de saida.";
                                else
                                    evento.xCondUso =
                                        "A Carta de Correção é disciplinada pelo § 1º-A do art. 7º do Convênio S/N, de 15 de dezembro de 1970 e pode ser " +
                                        "utilizada para regularização de erro ocorrido na emissão de documento fiscal, desde que o erro não esteja relacionado " +
                                        "com: I - as variáveis que determinam o valor do imposto tais como: base de cálculo, alíquota, diferença de preço, " +
                                        "quantidade, valor da operação ou da prestação; II - a correção de dados cadastrais que implique mudança do " +
                                        "remetente ou do destinatário; III - a data de emissão ou de saída.";
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                //<?xml version="1.0" encoding="UTF-8"?>
                //<envEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                //  <idLote>000000000015255</idLote>
                //  <evento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                //    <infEvento Id="ID1101103511031029073900013955001000000001105112804108">
                //      <cOrgao>35</cOrgao>
                //      <tpAmb>2</tpAmb>
                //      <CNPJ>10290739000139</CNPJ>
                //      <chNFe>35110310290739000139550010000000011051128041</chNFe>
                //      <dhEvento>2011-03-03T08:06:00-03:00</dhEvento>
                //      <tpEvento>110110</tpEvento>
                //      <nSeqEvento>8</nSeqEvento>
                //      <verEvento>1.00</verEvento>
                //      <detEvento versao="1.00">
                //          <descEvento>Carta de Correção</descEvento>
                //          <xCorrecao>Texto de teste para Carta de Correção. Conteúdo do campo xCorrecao.</xCorrecao>
                //          <xCondUso>A Carta de Correção é disciplinada pelo § 1º-A do art. 7º do Convênio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularização de erro ocorrido na emissão de documento fiscal, desde que o erro não esteja relacionado com: I - as variáveis que determinam o valor do imposto tais como: base de cálculo, alíquota, diferença de preço, quantidade, valor da operação ou da prestação; II - a correção de dados cadastrais que implique mudança do remetente ou do destinatário; III - a data de emissão ou de saída.</xCondUso>
                //      </detEvento>
                //    </infEvento>
                //  </evento>
                //</envEvento>

                XmlDocument doc = new XmlDocument();
                doc.Load(arquivoXML);

                XmlNodeList envEventoList = doc.GetElementsByTagName("infEvento");

                foreach (XmlNode envEventoNode in envEventoList)
                {
                    XmlElement envEventoElemento = (XmlElement)envEventoNode;

                    this.oDadosEnvEvento.eventos.Add(new Evento());
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].tpEvento = envEventoElemento.GetElementsByTagName("tpEvento")[0].InnerText;
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].tpAmb = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].cOrgao = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("cOrgao")[0].InnerText);
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].chNFe = envEventoElemento.GetElementsByTagName("chNFe")[0].InnerText;
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].nSeqEvento = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("nSeqEvento")[0].InnerText);
                }
            }
        }
        #endregion

        #region LerRetornoEvento
        private void LerRetornoEvento(int emp)
        {
            try
            {
                //
                //<<<danasa 6-2011
                //<<<UTF8 -> tem acentuacao no retorno
                TextReader txt = new StreamReader(NomeArquivoXML, Encoding.Default);
                XmlDocument docEventoOriginal = new XmlDocument();
                docEventoOriginal.Load(Functions.StringXmlToStreamUTF8(txt.ReadToEnd()));
                txt.Close();

                MemoryStream msXml = Functions.StringXmlToStreamUTF8(this.vStrXmlRetorno);
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                XmlNodeList retEnvRetornoList = doc.GetElementsByTagName("retEnvEvento");

                foreach (XmlNode retConsSitNode in retEnvRetornoList)
                {
                    XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                    //Pegar o status de retorno da NFe que está sendo consultada a situação
                    var cStatCons = string.Empty;
                    if (retConsSitElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatCons = retConsSitElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }
                    switch (cStatCons)
                    {
                        case "128": //Lote de Evento Processado
                            {
#if structRetorno
                                <retEnvEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                                    <idLote>000000000015256</idLote>
                                    <tpAmb>2</tpAmb>
                                    <verAplic>SP_EVENTOS_PL_100</verAplic><
                                    cOrgao>35</cOrgao>
                                    <cStat>128</cStat>
                                    <xMotivo>Lote de Evento Processado</xMotivo>
                                    <retEvento versao="1.00">
                                        <infEvento>
                                            <tpAmb>2</tpAmb>
                                            <verAplic>SP_EVENTOS_PL_100</verAplic>
                                            <cOrgao>35</cOrgao>
                                            <cStat>135</cStat>
                                            <xMotivo>Evento registrado e vinculado a NF-e</xMotivo>
                                            <chNFe>35110610238568000107550010000121751000137350</chNFe>
                                            <tpEvento>110110</tpEvento>
                                            <xEvento>Carta de Correção registrada</xEvento>
                                            <nSeqEvento>3</nSeqEvento>
                                            <CNPJDest>99999999000191</CNPJDest>
                                            <dhRegEvento>2011-06-09T04:00:16-03:00</dhRegEvento>
                                            <nProt>135110004500940</nProt>
                                        </infEvento>
                                    </retEvento>
                                    <retEvento versao="1.00">
                                        <infEvento>
                                            <tpAmb>2</tpAmb>
                                            <verAplic>SP_EVENTOS_PL_100</verAplic>
                                            <cOrgao>35</cOrgao>
                                            <cStat>135</cStat>
                                            <xMotivo>Evento registrado e vinculado a NF-e</xMotivo>
                                            <chNFe>35110610238568000107550010000121751000137350</chNFe>
                                            <tpEvento>110110</tpEvento>
                                            <xEvento>Carta de Correção registrada</xEvento>
                                            <nSeqEvento>4</nSeqEvento>
                                            <CNPJDest>99999999000191</CNPJDest>
                                            <dhRegEvento>2011-06-09T04:00:16-03:00</dhRegEvento>
                                            <nProt>135110004500941</nProt>
                                        </infEvento>
                                    </retEvento>
                                </retEnvEvento>
#endif
                                XmlNodeList envEventosList = doc.GetElementsByTagName("retEvento");
                                for (int i = 0; i < envEventosList.Count; ++i)
                                {
                                    XmlElement eleRetorno = envEventosList.Item(i) as XmlElement;
                                    cStatCons = eleRetorno.GetElementsByTagName("cStat")[0].InnerText;
                                    if (cStatCons == "135" || cStatCons == "136" || cStatCons == "155")
                                    {
                                        string chNFe = eleRetorno.GetElementsByTagName("chNFe")[0].InnerText;
                                        Int32 nSeqEvento = Convert.ToInt32("0" + eleRetorno.GetElementsByTagName("nSeqEvento")[0].InnerText);
                                        string tpEvento = eleRetorno.GetElementsByTagName("tpEvento")[0].InnerText;
                                        string Id = "ID" + tpEvento + chNFe + nSeqEvento.ToString("00");
                                        ///
                                        ///procura no Xml de envio pelo Id retornado
                                        ///nao sei se a Sefaz retorna na ordem em que foi enviado, então é melhor pesquisar
                                        foreach (XmlNode env in docEventoOriginal.GetElementsByTagName("infEvento"))
                                        {
                                            string Idd = env.Attributes.GetNamedItem("Id").Value;
                                            if (Idd == Id)
                                            {
                                                DateTime dhRegEvento = Functions.GetDateTime/* Convert.ToDateTime*/(eleRetorno.GetElementsByTagName("dhRegEvento")[0].InnerText);
                                                //if (Empresa.Configuracoes[emp].DiretorioSalvarComo == "AM")
                                                //    dhRegEvento = new DateTime(Convert.ToInt16("20" + chNFe.Substring(2, 2)), Convert.ToInt16(chNFe.Substring(4, 2)), 1);

                                                //Gerar o arquivo XML de distribuição do evento, retornando o nome completo do arquivo gravado
                                                oGerarXML.XmlDistEvento(emp, chNFe, nSeqEvento, Convert.ToInt32(tpEvento), env.ParentNode.OuterXml, eleRetorno.OuterXml, dhRegEvento);

                                                switch (Convert.ToInt32(tpEvento))
                                                {
                                                    case 110111: //Cancelamento
                                                        NFe.Service.TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, "");
                                                        break;

                                                    case 110110: //CCe
                                                        NFe.Service.TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, "");
                                                        break;
                                                }

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }
}

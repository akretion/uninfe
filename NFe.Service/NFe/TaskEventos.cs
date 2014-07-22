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
        public TaskEventos()
        {
            Servico = Servicos.Nulo;
            novaNomenclatura = false;
        }

        #region Classe com os dados do XML do registro de eventos
        private DadosenvEvento dadosEnvEvento;
        private int tpEmis = 0;
        private bool novaNomenclatura = false;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            novaNomenclatura = NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.PedEve) || 
                NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.PedEve_TXT) ||
                NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.PedEPEC) || 
                NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.PedEPEC_TXT);

            try
            {
                dadosEnvEvento = new DadosenvEvento();
                //Ler o XML para pegar parâmetros de envio
                EnvEvento(emp, NomeArquivoXML);

                string currentEvento = dadosEnvEvento.eventos[0].tpEvento;
                // mudei para aqui cajo haja erro e qdo for gravar o arquivo de erro precisamos saber qual o servico
                switch ((NFe.ConvertTxt.tpEventos)Enum.Parse(typeof(NFe.ConvertTxt.tpEventos), currentEvento))
                {
                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                        Servico = Servicos.EnviarEventoCancelamento;
                        break;
                    case ConvertTxt.tpEventos.tpEvCCe:
                        Servico = Servicos.EnviarCCe;
                        break;
                    case ConvertTxt.tpEventos.tpEvEPEC:
                        Servico = Servicos.EnviarEPEC;
                        break;
                    default:
                        Servico = Servicos.EnviarManifDest;
                        break;
                }

                foreach (Evento item in dadosEnvEvento.eventos)
                {
                    tpEmis = Convert.ToInt32(item.chNFe.Substring(34, 1)); //vai pegar o ambiente da Chave da Nfe autorizada p/ corrigir caso emitida em modo SCAN - Renan
                    if (!currentEvento.Equals(item.tpEvento))
                        throw new Exception(string.Format("Não é possivel mesclar tipos de eventos dentro de um mesmo xml/txt de eventos. O tipo de evento neste xml/txt é {0}", currentEvento));
                }

                //Pegar o estado da chave, pois na cOrgao pode vir o estado 91 - Wandreuy 22/08/2012
                int cOrgao = dadosEnvEvento.eventos[0].cOrgao;
                int ufParaWS = cOrgao;

                //Se o cOrgao for igual a 91 tenho que mudar a ufParaWS para que na hora de buscar o WSDL para conectar ao serviço, ele consiga encontrar. Wandrey 23/01/2013
                if (cOrgao == 91)
                    ufParaWS = Convert.ToInt32(dadosEnvEvento.eventos[0].chNFe.Substring(0, 2));

                // Se for evento de cancelamento
                switch (Servico)
                {
                    case Servicos.EnviarEventoCancelamento:
                        switch ((NFe.Components.TipoEmissao)tpEmis)
                        {
                            case NFe.Components.TipoEmissao.teSVCAN:
                            case NFe.Components.TipoEmissao.teSVCRS:
                            case NFe.Components.TipoEmissao.teSVCSP:
                            case NFe.Components.TipoEmissao.teSCAN:
                                //Se a nota fiscal foi emitida em ambiente SCAN o cancelamento tem que ir para SCAN ou gera uma rejeição. Wandrey 15/02/2013
                                break;

                            case NFe.Components.TipoEmissao.teNormal:
                                //Se a nota fiscal foi emitida em ambiente NORMAL, o cancelamento tem que ir para o ambiente normal ou gera uma rejeição. Wandrey 15/02/2013
                                break;

                            default:
                                //Os demais tipos de emissão tem que sempre ir para o ambiente NORMAL. Wandrey 22/02/2013
                                tpEmis = (int)NFe.Components.TipoEmissao.teNormal;
                                break;
                        }
                        break;

                    case Servicos.EnviarCCe:
                        //CCe só existe no ambiente Normal. Wandrey 22/04/2013
                        tpEmis = (int)NFe.Components.TipoEmissao.teNormal;
                        break;

                    case Servicos.EnviarEPEC:
                        //tpEmis = Propriedade.TipoEmissao.teEPEC;
                        break;

                    default:
                        break;
                }

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(
                        Servico,
                        emp,
                        ufParaWS,
                        dadosEnvEvento.eventos[0].tpAmb,
                        tpEmis, string.Empty);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoEvento;
                    oRecepcaoEvento = wsProxy.CriarObjeto("RecepcaoEvento");

                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(cOrgao, Servico));
                    string xmlExtEnvio = string.Empty;
                    string xmlExtRetorno = string.Empty;

                    wsProxy.SetProp(oCabecMsg, "cUF", cOrgao.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", NFe.ConvertTxt.versoes.VersaoXMLEvento);

                    if (novaNomenclatura)
                    {
                        if (Servico == Servicos.EnviarEPEC)
                        {
                            xmlExtEnvio = Propriedade.ExtEnvio.PedEPEC.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.retEPEC_XML.Replace(".xml", "");
                        }
                        else
                        {
                            xmlExtEnvio = Propriedade.ExtEnvio.PedEve.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.Eve.Replace(".xml", "");
                        }
                    }
                    else
                    {
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
                    }

                    //Criar objeto da classe de assinatura digital
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, cOrgao);

                    oInvocarObj.Invocar(wsProxy, oRecepcaoEvento, NomeMetodoWS(Servico, ufParaWS), oCabecMsg, this, xmlExtEnvio, xmlExtRetorno);

                    //Ler o retorno
                    LerRetornoEvento(emp);
                }
                else
                {
                    // Gerar o XML de eventos a partir do TXT gerado pelo ERP
                    string xmlFileExt = string.Empty;
                    string xmlFileExtTXT = string.Empty;
                    if (novaNomenclatura)
                    {
                        if (Servico == Servicos.EnviarEPEC)
                        {
                            xmlFileExt = Propriedade.ExtEnvio.PedEPEC;
                            xmlFileExtTXT = Propriedade.ExtEnvio.PedEPEC_TXT;
                        }
                        else
                        {
                            xmlFileExt = Propriedade.ExtEnvio.PedEve;
                            xmlFileExtTXT = Propriedade.ExtEnvio.PedEve_TXT;
                        }
                    }
                    else
                    {
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
                    }
                    oGerarXML.EnvioEvento(Functions.ExtrairNomeArq(NomeArquivoXML, xmlFileExtTXT) + xmlFileExt, dadosEnvEvento);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    string ExtRet = string.Empty;
                    string ExtRetorno = string.Empty;

                    if (novaNomenclatura)
                    {
                        if (Servico == Servicos.EnviarEPEC)
                        {
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.PedEPEC : Propriedade.ExtEnvio.PedEPEC_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.retEPEC_ERR;
                        }
                        else
                        {
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.PedEve : Propriedade.ExtEnvio.PedEve_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.Eve_ERR;
                        }
                    }
                    else
                    {
                        if (Servico == Servicos.Nulo)
                        {
                            // pode ter vindo de um txt e houve erro
                            if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCCe_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCCe_TXT))
                                Servico = Servicos.EnviarCCe;
                            else
                                if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvManifestacao_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvManifestacao_TXT))
                                    Servico = Servicos.EnviarManifDest;
                                else
                                    if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCancelamento_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCancelamento_TXT))
                                        Servico = Servicos.EnviarEventoCancelamento;
                        }

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

                            case Servicos.EnviarManifDest:
                                ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvManifestacao_XML : Propriedade.ExtEnvio.EnvManifestacao_TXT;
                                ExtRetorno = Propriedade.ExtRetorno.retManifestacao_ERR;
                                break;

                            default:
                                throw new Exception("Nao pode identificar o tipo de serviço para o arquivo: " + NomeArquivoXML);
                        }
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
                ///<<<<EVENTO DE CARTA DE CORRECAO>>>>
                ///idLote|000000000015255
                ///evento|1
                ///Id|ID1101103511031029073900013955001000000001105112804101                    <<opcional
                ///cOrgao|35
                ///tpAmb|2
                ///CNPJ|10290739000139 
                ///    ou
                ///CPF|12345678901
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
                /// CPF|12345678901
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
                ///CPF|12345678901
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
                ///CPF|12345678901
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
                ///CPF|12345678901
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
                ///CPF|12345678901
                ///chNFe|35110310290739000139550010000000011051128041
                ///dhEvento|2011-03-03T08:06:00-03:00
                ///tpEvento|210240
                ///nSeqEvento|1
                ///verEvento|1.00
                ///descEvento|Operacao nao realizada                                             <<opcional


                /// --------------------------------------------
                ///<<<<EVENTO EPEC>>>>
                ///idLote|000000000015255
                ///evento|1
                ///Id|ID2102403511031029073900013955001000000001105112804102
                ///cOrgao|35
                ///tpAmb|2
                ///CNPJ|10290739000139 
                ///    ou
                ///CPF|12345678901
                ///chNFe|35110310290739000139550010000000011051128041
                ///dhEvento|2011-03-03T08:06:00-03:00
                ///tpEvento|110140
                ///nSeqEvento|1
                ///verEvento|1.00
                ///descEvento|EPEC                                                               <<opcional
                ///epec.cOrgaoAutor|35
                ///epec.tpAutor|1
                ///epec.verAplic|1.1.1.0
                ///epec.dhEmi|2011-03-03T08:06:00-03:00
                ///epec.tpNF|1
                ///epec.IE|ISENTO
                ///epec.dest.UF|SP
                ///epec.dest.CNPJ|10290739000139
                ///     ou
                ///epec.dest.CPF|12345678901
                ///     ou
                ///epec.dest.idEstrangeiro|9999999
                ///epec.dest.IE|nao pode conter o texto 'ISENTO'
                ///epec.vNF|1234.00
                ///epec.vICMS|1.00
                ///epec.vST|2.00

                List<string> cLinhas = Functions.LerArquivo(arquivoXML);

                foreach (string cTexto in cLinhas)
                {
                    string[] dados = cTexto.Split(new char[] {'|'} );
                    if (dados.Length == 1) continue;

                    switch (dados[0].ToLower())
                    {
                        case "idlote":
                            this.dadosEnvEvento.idLote = dados[1].Trim();
                            break;
                        case "evento":
                            this.dadosEnvEvento.eventos.Add(new Evento());
                            break;
                        case "id":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].Id = dados[1].Trim();
                            break;
                        case "corgao":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].cOrgao = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "tpamb":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "cnpj":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].CNPJ = dados[1].Trim();
                            break;
                        case "cpf":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].CPF = dados[1].Trim();
                            break;
                        case "chnfe":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].chNFe = dados[1].Trim();
                            break;
                        case "dhevento":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].dhEvento = dados[1].Trim();
                            break;
                        case "tpevento":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].tpEvento = dados[1].Trim();
                            break;
                        case "nseqevento":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].nSeqEvento = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "verevento":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].verEvento = dados[1].Trim();
                            break;
                        case "descevento":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].descEvento = dados[1].Trim();
                            break;
                        case "xcorrecao":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].xCorrecao = dados[1].Trim();
                            break;
                        case "xconduso":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].xCondUso = dados[1].Trim();
                            break;
                        case "xjust":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].xJust = dados[1].Trim();
                            break;
                        case "nprot":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].nProt = dados[1].Trim();
                            break;
                        ///
                        /// EPEC
                        /// 
                        case "epec.corgaoautor":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.cOrgaoAutor = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "epec.dhemi":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dhEmi = dados[1].Trim();
                            break;
                        case "epec.ie":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.IE = dados[1].Trim();
                            break;
                        case "epec.veraplic":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.verAplic = dados[1].Trim();
                            break;
                        case "epec.tpautor":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.tpAutor = (NFe.ConvertTxt.TpcnTipoAutor)Enum.Parse(typeof(NFe.ConvertTxt.TpcnTipoAutor), dados[1].Trim());
                            break;
                        case "epec.tpnf":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.tpNF = (NFe.ConvertTxt.TpcnTipoNFe)Enum.Parse(typeof(NFe.ConvertTxt.TpcnTipoNFe), dados[1].Trim());
                            break;
                        case "epec.dest.idestrangeiro":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.idEstrangeiro = dados[1].Trim();
                            break;
                        case "epec.dest.cnpj":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.CNPJ = dados[1].Trim();
                            break;
                        case "epec.dest.cpf":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.CPF = dados[1].Trim();
                            break;
                        case "epec.dest.ie":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.IE = dados[1].Trim();
                            break;
                        case "epec.dest.uf":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.UF = dados[1].Trim();
                            break;
                        case "epec.vnf":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.vNF = Convert.ToDouble("0" + dados[1].Trim().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                            break;
                        case "epec.vicms":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.vICMS = Convert.ToDouble("0" + dados[1].Trim().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                            break;
                        case "epec.vst":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.vST = Convert.ToDouble("0" + dados[1].Trim().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                            break;
                    }
                }
                foreach (Evento evento in this.dadosEnvEvento.eventos)
                {
                    NFe.ConvertTxt.tpEventos tpe = (NFe.ConvertTxt.tpEventos)Enum.Parse(typeof(ConvertTxt.tpEventos), evento.tpEvento);
                    switch (tpe)
                    {
                        case ConvertTxt.tpEventos.tpEvEPEC:
                        case ConvertTxt.tpEventos.tpEvCCe:
                        case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                        case ConvertTxt.tpEventos.tpEvCienciaOperacao:
                        case ConvertTxt.tpEventos.tpEvConfirmacaoOperacao:
                        case ConvertTxt.tpEventos.tpEvDesconhecimentoOperacao:
                        case ConvertTxt.tpEventos.tpEvOperacaoNaoRealizada:
                            evento.nSeqEvento = 1;
                            break;
                        case ConvertTxt.tpEventos.tpEvEncerramentoMDFe:
                        case ConvertTxt.tpEventos.tpEvInclusaoCondutor:
                        case ConvertTxt.tpEventos.tpEvRegistroPassagem:
                        case ConvertTxt.tpEventos.tpEvRegistroPassagemBRid:
                            break;
                    }
                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = EnumHelper.GetDescription(tpe);

                    if (string.IsNullOrEmpty(evento.verEvento))
                        evento.verEvento = "1.00";

                    if (evento.tpAmb == 0)
                        evento.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;

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

                    dadosEnvEvento.eventos.Add(new Evento());
                    dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].tpEvento = envEventoElemento.GetElementsByTagName("tpEvento")[0].InnerText;
                    dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].tpAmb = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                    dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].cOrgao = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("cOrgao")[0].InnerText);
                    dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].chNFe = envEventoElemento.GetElementsByTagName("chNFe")[0].InnerText;
                    dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].nSeqEvento = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("nSeqEvento")[0].InnerText);
                }
            }
        }
        #endregion

        #region LerRetornoEvento
        private void LerRetornoEvento(int emp)
        {
            // <<<UTF8 -> tem acentuacao no retorno
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
                            XmlNodeList envEventosList = doc.GetElementsByTagName("retEvento");
                            for (int i = 0; i < envEventosList.Count; ++i)
                            {
                                XmlElement eleRetorno = envEventosList.Item(i) as XmlElement;
                                cStatCons = eleRetorno.GetElementsByTagName("cStat")[0].InnerText;
                                if (cStatCons == "135" || cStatCons == "136" || cStatCons == "155")
                                {
                                    string chNFe = eleRetorno.GetElementsByTagName("chNFe")[0].InnerText;
                                    Int32 nSeqEvento = Convert.ToInt32("0" + eleRetorno.GetElementsByTagName("nSeqEvento")[0].InnerText);
                                    NFe.ConvertTxt.tpEventos tpEvento = (NFe.ConvertTxt.tpEventos)Enum.Parse(typeof(NFe.ConvertTxt.tpEventos), eleRetorno.GetElementsByTagName("tpEvento")[0].InnerText);
                                    string Id = "ID" + ((Int32)tpEvento).ToString("000000") + chNFe + nSeqEvento.ToString("00");
                                    ///
                                    ///procura no Xml de envio pelo Id retornado
                                    ///nao sei se a Sefaz retorna na ordem em que foi enviado, então é melhor pesquisar
                                    foreach (XmlNode env in docEventoOriginal.GetElementsByTagName("infEvento"))
                                    {
                                        string Idd = env.Attributes.GetNamedItem("Id").Value;
                                        if (Idd == Id)
                                        {
                                            if (tpEvento != ConvertTxt.tpEventos.tpEvEPEC)
                                            {
                                                DateTime dhRegEvento = Functions.GetDateTime(eleRetorno.GetElementsByTagName("dhRegEvento")[0].InnerText);

                                                //Gerar o arquivo XML de distribuição do evento, retornando o nome completo do arquivo gravado
                                                oGerarXML.XmlDistEvento(emp, chNFe, nSeqEvento, tpEvento, env.ParentNode.OuterXml, eleRetorno.OuterXml, dhRegEvento);
                                            }
                                            switch (tpEvento)
                                            {
                                                case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                                                case ConvertTxt.tpEventos.tpEvCCe:
                                                    try
                                                    {
                                                        NFe.Service.TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, Empresas.Configuracoes[emp]);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Auxiliar.WriteLog("TaskEventos: " + ex.Message);
                                                    }
                                                    break;

                                                case ConvertTxt.tpEventos.tpEvEPEC:
                                                    if (cStatCons == "136")
                                                        //Evento autorizado sem vinculação do evento à respectiva NF-e
                                                        try
                                                        {
                                                            NFe.Service.TFunctions.ExecutaUniDanfe(NomeArquivoXML, DateTime.Today, Empresas.Configuracoes[emp]);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Auxiliar.WriteLog("TaskEventos: " + ex.Message);
                                                        }
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
        #endregion
    }
}

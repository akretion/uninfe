using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    public class TaskNFeEventos : TaskAbst
    {
        public TaskNFeEventos(string arquivo)
        {
            Servico = Servicos.Nulo;
            novaNomenclatura = false;
            dadosEnvEvento = new DadosenvEvento();

            NomeArquivoXML = arquivo;
            if (vXmlNfeDadosMsgEhXML)
            {
                ConteudoXML.PreserveWhitespace = false;
                ConteudoXML.Load(arquivo);
            }
        }

        #region Classe com os dados do XML do registro de eventos

        private DadosenvEvento dadosEnvEvento;
        private bool novaNomenclatura = false;

        #endregion Classe com os dados do XML do registro de eventos

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                //Ler o XML para pegar parâmetros de envio
                EnvEvento(emp, dadosEnvEvento);

                string currentEvento = dadosEnvEvento.eventos[0].tpEvento;
                // mudei para aqui caso haja erro e qdo for gravar o arquivo de erro precisamos saber qual o servico
                switch (EnumHelper.StringToEnum<ConvertTxt.tpEventos>(currentEvento))
                {
                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                    case ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe:
                        Servico = Servicos.EventoCancelamento;
                        break;

                    case ConvertTxt.tpEventos.tpEvCCe:
                        Servico = Servicos.EventoCCe;
                        break;

                    case ConvertTxt.tpEventos.tpEvEPEC:
                        Servico = Servicos.EventoEPEC;
                        break;

                    case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_2:
                    case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_2:
                    case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_2:
                    case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_1:
                    case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_2:
                        Servico = Servicos.EventoRecepcao;
                        break;

                    default:
                        Servico = Servicos.EventoManifestacaoDest;
                        break;
                }
                ValidaEvento(emp, dadosEnvEvento);

                //Pegar o estado da chave, pois na cOrgao pode vir o estado 91 - Wandreuy 22/08/2012
                int cOrgao = dadosEnvEvento.eventos[0].cOrgao;
                //vai pegar o ambiente da Chave da Nfe autorizada p/ corrigir tpEmis
                int tpEmis = this.dadosEnvEvento.eventos[0].tpEmis; //Convert.ToInt32(this.dadosEnvEvento.eventos[0].chNFe.Substring(34, 1));
                int ufParaWS = cOrgao;

                //Se o cOrgao for igual a 91 tenho que mudar a ufParaWS para que na hora de buscar o WSDL para conectar ao serviço, ele consiga encontrar. Wandrey 23/01/2013
                if (cOrgao == 91)
                    ufParaWS = Convert.ToInt32(dadosEnvEvento.eventos[0].chNFe.Substring(0, 2));

                switch (Servico)
                {
                    case Servicos.EventoCancelamento:
                        switch ((TipoEmissao)tpEmis)
                        {
                            case TipoEmissao.teSVCAN:
                            case TipoEmissao.teSVCRS:
                            case TipoEmissao.teSVCSP:
                            case TipoEmissao.teNormal:
                                //Se a nota fiscal foi emitida em ambiente NORMAL, o cancelamento tem que ir para o ambiente normal ou gera uma rejeição. Wandrey 15/02/2013
                                break;

                            default:
                                //Os demais tipos de emissão tem que sempre ir para o ambiente NORMAL. Wandrey 22/02/2013
                                tpEmis = (int)TipoEmissao.teNormal;
                                break;
                        }
                        break;

                    case Servicos.EventoCCe:
                        //CCe só existe no ambiente Normal. Wandrey 22/04/2013
                        tpEmis = (int)TipoEmissao.teNormal;
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
                        tpEmis,
                        string.Empty,
                        dadosEnvEvento.eventos[0].mod,
                        0);

                    System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(ufParaWS, dadosEnvEvento.eventos[0].tpAmb, tpEmis, Servico);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoEvento = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    string xmlExtEnvio = string.Empty;
                    string xmlExtRetorno = string.Empty;

                    object oCabecMsg = null;
                    //Criei o try para deixar compativel a versão 3.10 com a 4.0
                    try
                    {
                        oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(cOrgao, Servico));
                        wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), cOrgao.ToString());
                        wsProxy.SetProp(oCabecMsg, TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLEvento);
                    }
                    catch { }

                    if (novaNomenclatura)
                    {
                        xmlExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML.Replace(".xml", "");
                        xmlExtRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).RetornoXML.Replace(".xml", "");
                    }
                    else
                    {
                        switch (Servico)
                        {
                            case Servicos.EventoCCe:
                                xmlExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML.Replace(".xml", "");
                                xmlExtRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).RetornoXML.Replace(".xml", "");
                                break;

                            case Servicos.EventoCancelamento:
                                xmlExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML.Replace(".xml", "");
                                xmlExtRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).RetornoXML.Replace(".xml", "");
                                break;

                            default:
                                xmlExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML.Replace(".xml", "");
                                xmlExtRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).RetornoXML.Replace(".xml", "");
                                break;
                        }
                    }

                    //Criar objeto da classe de assinatura digital
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(ConteudoXML, emp, cOrgao);

                    oInvocarObj.Invocar(wsProxy, oRecepcaoEvento, wsProxy.NomeMetodoWS[0], oCabecMsg, this, xmlExtEnvio, xmlExtRetorno, true, securityProtocolType);

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
                        xmlFileExt = Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML;
                        xmlFileExtTXT = Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioTXT;
                    }
                    else
                    {
                        switch (Servico)
                        {
                            case Servicos.EventoCCe:
                                xmlFileExt = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML;
                                xmlFileExtTXT = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioTXT;
                                break;

                            case Servicos.EventoCancelamento:
                                xmlFileExt = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML;
                                xmlFileExtTXT = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioTXT;
                                break;

                            default:
                                xmlFileExt = Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML;
                                xmlFileExtTXT = Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioTXT;
                                break;
                        }
                    }
                    string f = Functions.ExtrairNomeArq(NomeArquivoXML, xmlFileExtTXT) + xmlFileExt;

                    if (NomeArquivoXML.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        f = Path.Combine(Empresas.Configuracoes[emp].PastaValidar, f);
                    }
                    oGerarXML.EnvioEvento(f, dadosEnvEvento);
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
                        ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML :
                                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioTXT;
                        ExtRetorno = Propriedade.ExtRetorno.Eve_ERR;
                    }
                    else
                    {
                        if (Servico == Servicos.Nulo)
                        {
                            // pode ter vindo de um txt e houve erro
                            if (NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML) ||
                                NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioTXT))
                                Servico = Servicos.EventoCCe;
                            else
                                if (NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML) ||
                                    NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioTXT))
                                Servico = Servicos.EventoManifestacaoDest;
                            else
                                    if (NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML) ||
                                        NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioTXT))
                                Servico = Servicos.EventoCancelamento;
                        }

                        switch (Servico)
                        {
                            case Servicos.EventoCCe:
                                ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML :
                                                                Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioTXT;
                                ExtRetorno = Propriedade.ExtRetorno.retEnvCCe_ERR;
                                break;

                            case Servicos.EventoCancelamento:
                                ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML :
                                                                Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioTXT;
                                ExtRetorno = Propriedade.ExtRetorno.retCancelamento_ERR;
                                break;

                            case Servicos.EventoManifestacaoDest:
                                ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML :
                                                                Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioTXT;
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

        #endregion Execute

        #region EnvEvento()

        protected override void EnvEvento(int emp, DadosenvEvento dadosEnvEvento)
        {
            novaNomenclatura =
                NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML) ||
                NomeArquivoXML.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioTXT);

            ///
            /// danasa 6/2011
            ///
            if (Path.GetExtension(NomeArquivoXML).ToLower() == ".txt")
            {
                #region --txt

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
                ///epec.dest.vNF|1234.00
                ///epec.dest.vICMS|1.00
                ///epec.dest.vST|2.00

                /// --------------------------------------------
                ///<<<<Evento pedido de prorrogação de ICMS | Evento cancelamento de pedido de prorrogação de ICMS>>>>
                ///idLote|000000000015255
                ///evento|1
                ///Id|ID2102403511031029073900013955001000000001105112804102
                ///cOrgao|35
                ///tpAmb|2
                ///CNPJ|10290739000139
                ///chNFe|35110310290739000139550010000000011051128041
                ///dhEvento|2011-03-03T08:06:00-03:00
                ///tpEvento|111500 ou 111501 ou 111502 ou 111503
                ///nSeqEvento|1
                ///verEvento|1.00
                ///descEvento|Pedido de Prorrogacao                                             <<opcional
                ///nProt|0000000000001
                ///itemPedido.numItem|1
                ///itemPedido.qtdeItem|1
                ///
                ///descEvento|Cancelamento de Pedido de Prorrogacao                             <<opcional
                ///nProt|0000000000001
                ///idPedidoCancelado|ID999999CHAVE-NFE-9

                /// --------------------------------------------
                ///<<<<Evento Fisco Resposta ao Pedido de Prorrogação>>>>
                ///idLote|000000000015255
                ///evento|1
                ///Id|ID2102403511031029073900013955001000000001105112804102
                ///cOrgao|35
                ///tpAmb|2
                ///CNPJ|10290739000139
                ///chNFe|35110310290739000139550010000000011051128041
                ///dhEvento|2011-03-03T08:06:00-03:00
                ///tpEvento|411500 ou 411501 ou 411502 ou 411503
                ///nSeqEvento|1
                ///verEvento|1.00
                ///descEvento|Fisco – Prorrogação ICMS remessa para industrialização            <<opcional
                ///idPedido|
                ///respPedido.statPrazo|
                ///respPedido.itemPedido.numItem|
                ///respPedido.itemPedido.statPedido|
                ///respPedido.itemPedido.justStatus|
                ///respPedido.itemPedido.justStaOutra|
                ///respCancPedido.statCancPedido|
                ///respCancPedido.justStatus|
                ///respCancPedido.justStaOutra|

                List<string> cLinhas = Functions.LerArquivo(NomeArquivoXML);
                ProrrogacaoICMS lpcICMS = null;
                ItemPedido itemPedido = null;
                const string err0 = "Informe a linha \"respPedido.itemPedido.numItem\"";

                foreach (string cTexto in cLinhas)
                {
                    string[] dados = cTexto.Split(new char[] { '|' });
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

                        case "tpemis":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].tpEmis = Convert.ToInt32("0" + dados[1].Trim());
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
                        /// Prorrogacao/Cancelamento de ICMS
                        ///
                        case "itempedido.numitem":
                            lpcICMS = new ProrrogacaoICMS() { numItem = dados[1].Trim() };
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].prorrogacaoICMS.Add(lpcICMS);
                            break;

                        case "itempedido.qtdeitem":
                            if (lpcICMS == null) throw new Exception("Informe a linha \"itemPedido.numItem\"");
                            lpcICMS.qtdeItem = dados[1].Trim();
                            lpcICMS = null;
                            break;

                        case "idpedidocancelado":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].idPedidoCancelado = dados[1].Trim();
                            break;
                        ///
                        /// Fisco – Prorrogação ICMS remessa para industrialização
                        ///
                        case "idpedido":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].idPedido = dados[1].Trim();
                            break;

                        case "resppedido.statprazo":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].respPedido.statPrazo = dados[1].Trim();
                            break;

                        case "resppedido.itempedido.numitem":
                            itemPedido = new ItemPedido() { numItem = Convert.ToInt32("0" + dados[1].Trim()) };
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].respPedido.itemPedido.Add(itemPedido);
                            break;

                        case "resppedido.itempedido.statpedido":
                            if (itemPedido == null) throw new Exception(err0);
                            itemPedido.statPedido = Convert.ToInt32("0" + dados[1].Trim());
                            break;

                        case "resppedido.itempedido.juststatus":
                            if (itemPedido == null) throw new Exception(err0);
                            itemPedido.justStatus = Convert.ToInt32("0" + dados[1].Trim());
                            break;

                        case "resppedido.itempedido.juststaoutra":
                            if (!string.IsNullOrEmpty(dados[1].Trim()))
                            {
                                if (itemPedido == null) throw new Exception(err0);
                                itemPedido.justStaOutra = dados[1].Trim();
                            }
                            break;

                        case "respcancpedido.statcancpedido":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].respCancPedido.statCancPedido = Convert.ToInt32("0" + dados[1].Trim());
                            break;

                        case "respcancpedido.juststatus":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].respCancPedido.justStatus = Convert.ToInt32("0" + dados[1].Trim());
                            break;

                        case "respcancpedido.juststaoutra":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].respCancPedido.justStaOutra = dados[1].Trim();
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
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.tpAutor = NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.TpcnTipoAutor>(dados[1].Trim());
                            break;

                        case "epec.tpnf":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.tpNF = NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.TpcnTipoNFe>(dados[1].Trim());
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

                        case "epec.dest.vnf":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.vNF = Convert.ToDouble("0" + dados[1].Trim().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                            break;

                        case "epec.dest.vicms":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.vICMS = Convert.ToDouble("0" + dados[1].Trim().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                            break;

                        case "epec.dest.vst":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].epec.dest.vST = Convert.ToDouble("0" + dados[1].Trim().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                            break;

                        ///Cancelamento por substituição da NFCe
                        case "cancelamentosubstituicao.corgaoautor":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].cancelamentoSubstituicao.cOrgaoAutor = Convert.ToInt32("0" + dados[1].Trim());
                            break;

                        case "cancelamentosubstituicao.chnferef":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].cancelamentoSubstituicao.chNFeRef = dados[1].Trim();
                            break;

                        case "cancelamentosubstituicao.veraplic":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].cancelamentoSubstituicao.verAplic = dados[1].Trim();
                            break;

                        case "cancelamentosubstituicao.tpautor":
                            this.dadosEnvEvento.eventos[this.dadosEnvEvento.eventos.Count - 1].cancelamentoSubstituicao.tpAutor = NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.TpcnTipoAutor>(dados[1].Trim());
                            break;
                    }
                }
                foreach (Evento evento in this.dadosEnvEvento.eventos)
                {
                    ConvertTxt.tpEventos tpe = EnumHelper.StringToEnum<ConvertTxt.tpEventos>(evento.tpEvento);
                    switch (tpe)
                    {
                        case ConvertTxt.tpEventos.tpEvEPEC:
                        case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                        case ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe:
                        case ConvertTxt.tpEventos.tpEvCienciaOperacao:
                        case ConvertTxt.tpEventos.tpEvConfirmacaoOperacao:
                        case ConvertTxt.tpEventos.tpEvDesconhecimentoOperacao:
                        case ConvertTxt.tpEventos.tpEvOperacaoNaoRealizada:
                            evento.nSeqEvento = 1;
                            break;

                        case ConvertTxt.tpEventos.tpEvEncerramentoMDFe:
                        case ConvertTxt.tpEventos.tpEvPagamentoOperacaoMDFe:
                        case ConvertTxt.tpEventos.tpEvInclusaoCondutor:
                        case ConvertTxt.tpEventos.tpEvRegistroPassagem:
                        case ConvertTxt.tpEventos.tpEvRegistroPassagemBRid:
                            break;

                        case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_1: //pedido de prorrogacao 1
                        case ConvertTxt.tpEventos.tpEvPedProrrogacao_ICMS_2: //pedido de prorrogacao 2
                            if (string.IsNullOrEmpty(evento.descEvento))
                                evento.descEvento = "Pedido de Prorrogacao";
                            break;

                        case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_1: //pedido de cancelamento 1
                        case ConvertTxt.tpEventos.tpEvCancPedProrrogacao_ICMS_2: //pedido de cancelamento 2
                            if (string.IsNullOrEmpty(evento.descEvento))
                                evento.descEvento = "Cancelamento de Pedido de Prorrogacao";
                            break;

                        case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_1:
                        case ConvertTxt.tpEventos.tpEvFiscoRespPedProrrogacao_ICMS_2:
                        case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_1:
                        case ConvertTxt.tpEventos.tpEvFiscoRespCancPedProrrogacao_ICMS_2:
                            if (string.IsNullOrEmpty(evento.descEvento))
                                evento.descEvento = "Fisco – Prorrogacao ICMS remessa para industrializacao";
                            break;
                    }
                    if (string.IsNullOrEmpty(evento.descEvento)) evento.descEvento = EnumHelper.GetDescription(tpe);

                    if (string.IsNullOrEmpty(evento.verEvento))
                        evento.verEvento = "1.00";

                    if (evento.nSeqEvento == 0)
                        evento.nSeqEvento = 1;

                    if (evento.tpAmb == 0)
                        evento.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;

                    if (evento.cOrgao == 0)
                        evento.cOrgao = Convert.ToInt32(evento.chNFe.Substring(0, 2));

                    if (evento.tpEmis == 0)
                        evento.tpEmis = Convert.ToInt32(evento.chNFe.Substring(34, 1));

                    if (string.IsNullOrEmpty(evento.Id))
                        evento.Id = TpcnResources.ID.ToString() + evento.tpEvento + evento.chNFe + evento.nSeqEvento.ToString("00");

                    if (string.IsNullOrEmpty(evento.xCondUso))
                        evento.xCondUso =
                            "A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, " +
                            "de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido na emissao de " +
                            "documento fiscal, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o " +
                            "valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da " +
                            "operacao ou da prestacao; II - a correcao de dados cadastrais que implique mudanca do remetente " +
                            "ou do destinatario; III - a data de emissao ou de saida.";
                }

                #endregion --txt
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

                base.EnvEvento(emp, dadosEnvEvento);
            }
        }

        #endregion EnvEvento()

        #region LerRetornoEvento

        private void LerRetornoEvento(int emp)
        {
            XmlDocument docEventoOriginal = ConteudoXML;
            bool autorizou = false;

            // <<<UTF8 -> tem acentuacao no retorno
            XmlDocument doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));
            //doc.Load(@"C:\Users\wandrey\Downloads\51160417625687000153550010001224661001224662-ret-env-canc (1).xml");

            XmlNodeList retEnvRetornoList = doc.GetElementsByTagName("retEnvEvento");

            foreach (XmlNode retConsSitNode in retEnvRetornoList)
            {
                XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                //Pegar o status de retorno da NFe que está sendo consultada a situação
                var cStatCons = string.Empty;
                if (retConsSitElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0] != null)
                {
                    cStatCons = retConsSitElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                }
                switch (cStatCons)
                {
                    case "128": //Lote de Evento Processado
                        {
                            XmlNodeList envEventosList = doc.GetElementsByTagName("retEvento");
                            for (int i = 0; i < envEventosList.Count; ++i)
                            {
                                XmlElement eleRetorno = envEventosList.Item(i) as XmlElement;
                                cStatCons = eleRetorno.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                                if (cStatCons == "135" || cStatCons == "136" || cStatCons == "155")
                                {
                                    string chNFe = eleRetorno.GetElementsByTagName(TpcnResources.chNFe.ToString())[0].InnerText;
                                    Int32 nSeqEvento = Convert.ToInt32("0" + eleRetorno.GetElementsByTagName(TpcnResources.nSeqEvento.ToString())[0].InnerText);
                                    ConvertTxt.tpEventos tpEvento = EnumHelper.StringToEnum<ConvertTxt.tpEventos>(eleRetorno.GetElementsByTagName(TpcnResources.tpEvento.ToString())[0].InnerText);
                                    string Id = TpcnResources.ID.ToString() + ((Int32)tpEvento).ToString("000000") + chNFe + nSeqEvento.ToString("00");
                                    ///
                                    ///procura no Xml de envio pelo Id retornado
                                    ///nao sei se a Sefaz retorna na ordem em que foi enviado, então é melhor pesquisar
                                    foreach (XmlNode env in docEventoOriginal.GetElementsByTagName("infEvento"))
                                    {
                                        string Idd = env.Attributes.GetNamedItem(TpcnResources.Id.ToString()).Value;
                                        if (Idd == Id)
                                        {
                                            autorizou = true;

                                            DateTime dhRegEvento = Functions.GetDateTime(eleRetorno.GetElementsByTagName(TpcnResources.dhRegEvento.ToString())[0].InnerText);

                                            ///
                                            /// Gerar o arquivo XML de distribuição do evento
                                            ///
                                            oGerarXML.XmlDistEvento(emp, chNFe, nSeqEvento, tpEvento,
                                                                    env.ParentNode.OuterXml,
                                                                    eleRetorno.OuterXml,
                                                                    dhRegEvento,
                                                                    true);

                                            switch (tpEvento)
                                            {   
                                                case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                                                case ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe:
                                                case ConvertTxt.tpEventos.tpEvCCe:
                                                    try
                                                    {
                                                        TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, Empresas.Configuracoes[emp]);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Auxiliar.WriteLog("TaskNFeEventos: " + ex.Message, false);
                                                    }
                                                    break;

                                                case ConvertTxt.tpEventos.tpEvEPEC:
                                                    if (cStatCons == "136")
                                                        //Evento autorizado sem vinculação do evento à respectiva NF-e
                                                        try
                                                        {
                                                            TFunctions.ExecutaUniDanfe(oGerarXML.NomeArqGerado, DateTime.Today, Empresas.Configuracoes[emp]);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Auxiliar.WriteLog("TaskNFeEventos: " + ex.Message, false);
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

            if (!autorizou)
                oAux.MoveArqErro(NomeArquivoXML);
        }

        #endregion LerRetornoEvento
    }
}
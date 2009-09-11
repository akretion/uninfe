﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UniNFeLibrary;
using UniNFeLibrary.Enums;

namespace UniNFeLibrary
{
    #region Classe ServicoNFe
    public abstract class absServicoNFe
    {
        #region Objetos
        protected Auxiliar oAux = new Auxiliar();
        protected InvocarObjeto oInvocarObj = new InvocarObjeto();
        #endregion

        #region Propriedades

        /// <summary>
        /// Conteúdo do XML de retorno do serviço, ou seja, para cada serviço invocado a classe seta neste atributo a string do XML Retornado pelo serviço
        /// </summary>
        public string vStrXmlRetorno { get; set; }

        protected string mXmlNfeDadosMsg;
        protected Servicos mServico { get; set; }

        /// <summary>
        /// Arquivo XML contendo os dados a serem enviados (Nota Fiscal, Pedido de Status, Cancelamento, etc...)
        /// </summary>
        public abstract string vXmlNfeDadosMsg { get; set; }

        /// <summary>
        /// Serviço que está sendo executado (Envio de Nota, Cancelamento, Consulta, etc...)
        /// </summary>
        protected abstract Servicos Servico { get; set; }

        #endregion

        #region Métodos de execução dos serviços da NFE

        #region StatusServico()
        public abstract void StatusServico();
        #endregion

        #region Recepcao()
        public abstract void Recepcao();
        #endregion

        #region RetRecepcao()
        public abstract void RetRecepcao();
        #endregion

        #region Consulta()
        public abstract void Consulta();
        #endregion

        #region Cancelamento()
        public abstract void Cancelamento();
        #endregion

        #region Inutilizacao()
        public abstract void Inutilizacao();
        #endregion

        #region ConsultaCadastro()
        public abstract void ConsultaCadastro();
        #endregion

        #endregion

        #region Métodos auxiliares

        #region CabecMsg()
        /// <summary>
        /// Auxiliar na geração do cabecalho da mensagem quando estivermos utilizando o InvokeMember para chamar o método
        /// </summary>
        /// <param name="cVersaoDados">Versão dos dados do XML</param>
        /// <returns>Conteúdo do XML</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>07/08/2009</date>
        public abstract string CabecMsg(string cVersaoDados);
        #endregion

        #region XmlRetorno()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de retorno para o ERP quando estivermos utilizando o InvokeMember para chamar o método
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public abstract void XmlRetorno(string pFinalArqEnvio, string pFinalArqRetorno);
        #endregion

        #region LoteNfe()
        public abstract void LoteNfe(string strArquivoNfe);
        #endregion

        #region LoteNfe() - Sobrecarga
        public abstract void LoteNfe(List<string> lstArquivoNfe);
        #endregion

        #region XmlPedRec()
        public abstract void XmlPedRec(string nRec);
        #endregion

        #region VerStatusServico()
        public abstract string VerStatusServico(int tpEmis, int cUF);
        #endregion

        #region AssinarValidarXMLNFe()
        /// <summary>
        /// Assinar e validar o XML da Nota Fiscal Eletrônica e move para a pasta de assinados
        /// </summary>
        /// <param name="bMoverXML">true = Mover XML assinado da pasta de Lote para a subpasta Assinado</param>
        /// <param name="strPasta">Nome da pasta onde está o XML a ser validado e assinado</param>
        /// <returns>true = Conseguiu assinar e validar</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        public Boolean AssinarValidarXMLNFe(string strPasta)
        {
            Boolean bRetorna = false;
            Boolean bAssinado = this.Assinado(this.vXmlNfeDadosMsg);
            Boolean bValidadoSchema = false;
            Boolean bValidacaoGeral = false;

            //Criar Pasta dos XML´s a ser enviado em Lote já assinados
            string strPastaLoteAssinado = strPasta + ConfiguracaoApp.NomePastaXMLAssinado;

            //Se o arquivo XML já existir na pasta de assinados, vou avisar o ERP que já tem um em andamento
            string strArqDestino = strPastaLoteAssinado + "\\" + oAux.ExtrairNomeArq(this.vXmlNfeDadosMsg, ".xml") + ".xml";

            try
            {
                //Fazer uma leitura de algumas tags do XML
                absLerXML.DadosNFeClass oDadosNFe = this.LerXMLNFe(this.vXmlNfeDadosMsg);
                string ChaveNfe = oDadosNFe.chavenfe;
                string TpEmis = oDadosNFe.tpEmis;

                //Inserir NFe no XML de controle do fluxo
                try
                {
                    FluxoNfe oFluxoNfe = new FluxoNfe();
                    if (oFluxoNfe.NfeExiste(ChaveNfe))
                    {
                        //Deletar o arquivo da pasta em processamento
                        oAux.DeletarArquivo(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + oAux.ExtrairNomeArq(this.vXmlNfeDadosMsg, ".xml") + ".xml");

                        //Deletar a NFE do arquivo de controle de fluxo
                        oFluxoNfe.ExcluirNfeFluxo(ChaveNfe);
                    }

                    //Deletar o arquivo XML da pasta de temporários de XML´s com erros se o mesmo existir
                    oAux.DeletarArqXMLErro(ConfiguracaoApp.vPastaXMLErro + "\\" + oAux.ExtrairNomeArq(this.vXmlNfeDadosMsg, ".xml") + ".xml");

                    //Validações gerais
                    if (this.ValidacoesGeraisXMLNFe(this.vXmlNfeDadosMsg, oDadosNFe))
                    {
                        bValidacaoGeral = true;
                    }

                    //Assinar o arquivo XML
                    if (bValidacaoGeral && !bAssinado)
                    {
                        AssinaturaDigital oAD = new AssinaturaDigital();

                        ValidarXMLs oValidador = new ValidarXMLs();
                        oValidador.TipoArquivoXML(this.vXmlNfeDadosMsg);

                        oAD.Assinar(this.vXmlNfeDadosMsg, oValidador.TagAssinar, ConfiguracaoApp.oCertificado);

                        if (oAD.intResultado == 0)
                        {
                            bAssinado = true;
                        }
                    }

                    // Validar o Arquivo XML da NFe com os Schemas se estiver assinado
                    if (bValidacaoGeral && bAssinado)
                    {
                        string cResultadoValidacao = oAux.ValidarArqXML(this.vXmlNfeDadosMsg);
                        if (cResultadoValidacao == "")
                        {
                            bValidadoSchema = true;
                        }
                        else
                        {
                            //Registrar o erro da validação do schema para o sistema ERP
                            throw new Exception(cResultadoValidacao);
                        }
                    }

                    //Mover o arquivo XML da pasta de lote para a pasta de XML´s assinados
                    if (bValidadoSchema)
                    {
                        try
                        {
                            //Se a pasta de assinados não existir, vamos criar
                            if (!Directory.Exists(strPastaLoteAssinado))
                            {
                                Directory.CreateDirectory(strPastaLoteAssinado);
                            }

                            if (!File.Exists(strArqDestino))
                            {
                                //Mover o arquivo para a pasta de XML´s assinados
                                FileInfo oArquivo = new FileInfo(this.vXmlNfeDadosMsg);
                                oArquivo.MoveTo(strArqDestino);

                                bRetorna = true;
                            }
                            else
                            {
                                oFluxoNfe.InserirNfeFluxo(ChaveNfe, oAux.ExtrairNomeArq(strArqDestino, ".xml") + ".xml");

                                throw new IOException("Esta nota fiscal já está na pasta de Notas Fiscais assinadas e em processo de envio, desta forma não é possível enviar a mesma novamente.\r\n" +
                                    this.vXmlNfeDadosMsg);
                            }
                        }
                        catch (IOException ex)
                        {
                            throw (ex);
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }

                    if (bRetorna)
                    {
                        try
                        {
                            oFluxoNfe.InserirNfeFluxo(ChaveNfe, oAux.ExtrairNomeArq(strArqDestino, ".xml") + ".xml");
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            catch (Exception ex)
            {
                oAux.GravarArqErroServico(this.vXmlNfeDadosMsg, ExtXml.Nfe, "-nfe.err", ex.Message);

                //Se já foi movido o XML da Nota Fiscal para a pasta em Processamento, vou ter que 
                //forçar mover para a pasta de XML com erro neste ponto.
                oAux.MoveArqErro(strArqDestino);
            }

            return bRetorna;
        }
        #endregion

        #region LerXMLNFe()
        protected abstract absLerXML.DadosNFeClass LerXMLNFe(string Arquivo);
        #endregion

        #region Assinado()
        /// <summary>
        /// Verifica se o XML já está assinado digitalmente ou não
        /// </summary>
        /// <param name="cArquivoXML">Arquivo a ser verificado</param>
        /// <returns>true = Arquivo XML já assinado</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        protected Boolean Assinado(string cArquivoXML)
        {
            Boolean bAssinado = false;

            //TODO: Tem que criar ainda o código que verifica se já está assinado ou não

            return bAssinado;
        }
        #endregion

        #region ValidacoesGerais()
        /// <summary>
        /// Efetua uma leitura do XML da nota fiscal eletrônica e faz diversas conferências do seu conteúdo e bloqueia se não 
        /// estiver de acordo com as configurações do UNINFE
        /// </summary>
        /// <param name="strArquivoNFe">Arquivo XML da NFe</param>
        /// <returns>true = Validado com sucesso</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>16/04/2009</date>
        protected bool ValidacoesGeraisXMLNFe(string strArquivoNFe, absLerXML.DadosNFeClass oDadosNFe)
        {
            bool booValido = false;
            string cTextoErro = "";

            //TODO: CONFIG
            try
            {
                //Verificar o tipo de emissão se bate com o configurado, se não bater vai retornar um erro 
                //para o ERP
                // danasa 8-2009
                if ((ConfiguracaoApp.tpEmis == TipoEmissao.teNormal && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5")) ||
                    (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN && (oDadosNFe.tpEmis == "3")))
                {
                    booValido = true;
                }
                // danasa 8-2009
                else if (ConfiguracaoApp.tpEmis == TipoEmissao.teContingencia && (oDadosNFe.tpEmis == "2"))
                {
                    booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 09/06/2009
                }
                // danasa 8-2009
                else if (ConfiguracaoApp.tpEmis == TipoEmissao.teFSDA && (oDadosNFe.tpEmis == "5"))
                {
                    booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                }
                else
                {
                    booValido = false;

                    // danasa 8-2009
                    if (ConfiguracaoApp.tpEmis == TipoEmissao.teNormal && oDadosNFe.tpEmis == "3")
                    {
                        cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SEFAZ " +
                            "(Secretaria Estadual da Fazenda) e o XML está configurado para enviar " +
                            "para o SCAN do Ambiente Nacional.\r\n\r\n";

                    }
                    // danasa 8-2009
                    else if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5"))
                    {
                        cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao SCAN do Ambiente Nacional " +
                            "e o XML está configurado para enviar para o Ambiente da SEFAZ (Secretaria Estadual da Fazenda)\r\n\r\n";
                    }

                    cTextoErro += "O XML não será enviado e será movido para a pasta de XML com erro para análise.";

                    throw new Exception(cTextoErro);
                }

                #region Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
                //Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
                if (booValido)
                {
                    cTextoErro = string.Empty;

                    #region Tag <cUF>
                    if (oDadosNFe.cUF != oDadosNFe.chavenfe.Substring(3, 2))
                    {
                        cTextoErro += "O código da UF informado na tag <cUF> está diferente do informado na chave da NF-e.\r\n" +
                            "Código da UF informado na tag <cUF>: " + oDadosNFe.cUF + "\r\n" +
                            "Código da UF informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(3, 2) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <cNF>
                    if (oDadosNFe.cNF != oDadosNFe.chavenfe.Substring(37, 9))
                    {
                        cTextoErro += "O código numérico informado na tag <cNF> está diferente do informado na chave da NF-e.\r\n" +
                            "Código numérico informado na tag <cNF>: " + oDadosNFe.cNF + "\r\n" +
                            "Código numérico informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(37, 9) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <mod>
                    if (oDadosNFe.mod != oDadosNFe.chavenfe.Substring(23, 2))
                    {
                        cTextoErro += "O modelo informado na tag <mod> está diferente do informado na chave da NF-e.\r\n" +
                            "Modelo informado na tag <mod>: " + oDadosNFe.mod + "\r\n" +
                            "Modelo informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(23, 2) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <nNF>
                    if (Convert.ToInt32(oDadosNFe.nNF) != Convert.ToInt32(oDadosNFe.chavenfe.Substring(28, 9)))
                    {
                        cTextoErro += "O número da NF-e informado na tag <nNF> está diferente do informado na chave da NF-e.\r\n" +
                            "Número da NFe informado na tag <nNF>: " + Convert.ToInt32(oDadosNFe.nNF).ToString() + "\r\n" +
                            "Número da NFe informado na chave da NF-e: " + Convert.ToInt32(oDadosNFe.chavenfe.Substring(28, 9)).ToString() + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <cDV>
                    if (oDadosNFe.cDV != oDadosNFe.chavenfe.Substring(46, 1))
                    {
                        cTextoErro += "O número do dígito verificador informado na tag <cDV> está diferente do informado na chave da NF-e.\r\n" +
                            "Número do dígito verificador informado na tag <cDV>: " + oDadosNFe.cDV + "\r\n" +
                            "Número do dígito verificador informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(46, 1) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <CNPJ> da tag <emit>
                    if (oDadosNFe.CNPJ != oDadosNFe.chavenfe.Substring(9, 14))
                    {
                        cTextoErro += "O CNPJ do emitente informado na tag <emit><CNPJ> está diferente do informado na chave da NF-e.\r\n" +
                            "CNPJ do emitente informado na tag <emit><CNPJ>: " + oDadosNFe.CNPJ + "\r\n" +
                            "CNPJ do emitente informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(9, 14) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <serie>
                    if (Convert.ToInt32(oDadosNFe.serie) != Convert.ToInt32(oDadosNFe.chavenfe.Substring(25, 3)))
                    {
                        cTextoErro += "A série informada na tag <serie> está diferente da informada na chave da NF-e.\r\n" +
                            "Série informada na tag <cDV>: " + Convert.ToInt32(oDadosNFe.serie).ToString() + "\r\n" +
                            "Série informada na chave da NF-e: " + Convert.ToInt32(oDadosNFe.chavenfe.Substring(25, 3)).ToString() + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <dEmi>
                    if (oDadosNFe.dEmi.Month.ToString("00") != oDadosNFe.chavenfe.Substring(7, 2) ||
                        oDadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) != oDadosNFe.chavenfe.Substring(5, 2))
                    {
                        cTextoErro += "A ano e mês da emissão informada na tag <dEmi> está diferente da informada na chave da NF-e.\r\n" +
                            "Mês/Ano da data de emissão informada na tag <dEmi>: " + oDadosNFe.dEmi.Month.ToString("00") + "/" + oDadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) + "\r\n" +
                            "Mês/Ano informados na chave da NF-e: " + oDadosNFe.chavenfe.Substring(5, 2) + "/" + oDadosNFe.chavenfe.Substring(7, 2) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    if (!booValido)
                    {
                        throw new Exception(cTextoErro);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                booValido = false;

                throw (ex);
            }

            return booValido;
        }
        #endregion

        #region LerRetornoLote()
        protected abstract void LerRetornoLote();
        #endregion

        #region LerRetornoCanc()
        protected abstract void LerRetornoCanc();
        #endregion

        #region LerRetornoInut()
        protected abstract void LerRetornoInut();
        #endregion

        #endregion
    }
    #endregion
}
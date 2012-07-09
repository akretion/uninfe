using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Validate;

namespace NFe.Service
{
    public abstract class TaskAbs
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

        protected string mNomeArquivoXML;
        protected Servicos mServico { get; set; }

        /// <summary>
        /// Pasta/Nome do arquivo XML contendo os dados a serem enviados (Nota Fiscal, Pedido de Status, Cancelamento, etc...)
        /// </summary>
        public abstract string NomeArquivoXML { get; set; }

        /// <summary>
        /// Serviço que está sendo executado (Envio de Nota, Cancelamento, Consulta, etc...)
        /// </summary>
        protected abstract Servicos Servico { get; set; }

        /// <summary>
        /// Se o vXmlNFeDadosMsg é um XML
        /// </summary>
        public bool vXmlNfeDadosMsgEhXML    //danasa 12-9-2009
        {
            get { return Path.GetExtension(NomeArquivoXML).ToLower() == ".xml"; }
        }
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

        #region RecepcaoDPEC()
        public abstract void RecepcaoDPEC();
        #endregion

        #region ConsultaDPEC()
        public abstract void ConsultaDPEC();
        #endregion

        #region RecepcaoEvento()
        public abstract void RecepcaoEvento();
        #endregion

        #endregion

        #region Métodos de execução dos serviços da NFSe
        public abstract void RecepcionarLoteRps();
        public abstract void ConsultarSituacaoLoteRps();
        public abstract void ConsultarNfsePorRps();
        public abstract void ConsultarNfse();
        public abstract void ConsultarLoteRps();
        public abstract void CancelarNfse();
        #endregion

        #region Métodos auxiliares

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
        public abstract void XmlPedRec(int empresa, string nRec);
        #endregion

        #region AssinarValidarXMLNFe()
        /// <summary>
        /// Assinar e validar o XML da Nota Fiscal Eletrônica e move para a pasta de assinados
        /// </summary>
        /// <param name="pasta">Nome da pasta onde está o XML a ser validado e assinado</param>
        /// <returns>true = Conseguiu assinar e validar</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 03/04/2009
        /// </remarks>
        public void AssinarValidarXMLNFe(string pasta)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            Boolean tudoOK = false;
            Boolean bAssinado = this.Assinado(NomeArquivoXML);
            Boolean bValidadoSchema = false;
            Boolean bValidacaoGeral = false;

            //Criar Pasta dos XML´s a ser enviado em Lote já assinados
            string pastaLoteAssinado = pasta + Propriedade.NomePastaXMLAssinado;

            //Se o arquivo XML já existir na pasta de assinados, vou avisar o ERP que já tem um em andamento
            string arqDestino = pastaLoteAssinado + "\\" + oAux.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml";

            try
            {
                //Fazer uma leitura de algumas tags do XML
                DadosNFeClass oDadosNFe = this.LerXMLNFe(NomeArquivoXML);
                string ChaveNfe = oDadosNFe.chavenfe;
                string TpEmis = oDadosNFe.tpEmis;

                //Inserir NFe no XML de controle do fluxo
                try
                {
                    FluxoNfe oFluxoNfe = new FluxoNfe();
                    if (oFluxoNfe.NfeExiste(ChaveNfe))
                    {
                        //Mover o arquivo da pasta em processamento para a pasta de XML´s com erro
                        oAux.MoveArqErro(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + oAux.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml");

                        //Deletar a NFE do arquivo de controle de fluxo
                        oFluxoNfe.ExcluirNfeFluxo(ChaveNfe);

                        //Vou forçar uma exceção, e o ERP através do inicio da mensagem de erro pode tratar e já gerar uma consulta
                        //situação para finalizar o processo. Assim envito perder os XML´s que estão na pasta EmProcessamento
                        //tendo assim a possibilidade de gerar o -procNfe.XML através da consulta situação.
                        //Wandrey 08/10/2009
                        //throw new Exception("NFE NO FLUXO: Esta nota fiscal já está na pasta de Notas Fiscais em processo de envio, desta forma não é possível envia-la novamente. Se a nota fiscal estiver presa no fluxo de envio sem conseguir finalizar o processo, gere um consulta situação da NFe para forçar a finalização.\r\n" + NomeArquivoXML);
                    }
                    else
                    {
                        //Deletar o arquivo XML da pasta de temporários de XML´s com erros se o mesmo existir
                        Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaErro + "\\" + oAux.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml");
                    }

                    //Validações gerais
                    if (ValidacoesGeraisXMLNFe(NomeArquivoXML, oDadosNFe))
                    {
                        bValidacaoGeral = true;
                    }

                    //Assinar o arquivo XML
                    if (bValidacaoGeral && !bAssinado)
                    {
                        AssinaturaDigital assDig = new AssinaturaDigital();
                        
                        assDig.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosNFe.cUF));

                        bAssinado = true;
                    }

                    // Validar o Arquivo XML da NFe com os Schemas se estiver assinado
                    if (bValidacaoGeral && bAssinado)
                    {
                        //TODO UNINFSE: Analisar namespace do metodo abaixo onde passei string.Empty. Vai ficar assim mesmo?
                        ValidarXML validar = new ValidarXML(NomeArquivoXML, Convert.ToInt32(oDadosNFe.cUF));

                        string cResultadoValidacao = validar.ValidarArqXML(NomeArquivoXML);
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
                            if (!Directory.Exists(pastaLoteAssinado))
                            {
                                Directory.CreateDirectory(pastaLoteAssinado);
                            }

                            FileInfo fiDestino = new FileInfo(arqDestino);

                            if (!File.Exists(arqDestino) || (long)DateTime.Now.Subtract(fiDestino.LastWriteTime).TotalMilliseconds >= 60000) //60.000 ms que corresponde á 60 segundos que corresponde a 1 minuto
                            {
                                //Mover o arquivo para a pasta de XML´s assinados
                                //FileInfo oArquivo = new FileInfo(NomeArquivoXML);
                                //oArquivo.MoveTo(arqDestino);
                                Functions.Move(NomeArquivoXML, arqDestino);

                                tudoOK = true;
                            }
                            else
                            {
                                oFluxoNfe.InserirNfeFluxo(ChaveNfe, oAux.ExtrairNomeArq(arqDestino, ".xml") + ".xml");

                                throw new IOException("Esta nota fiscal já está na pasta de Notas Fiscais assinadas e em processo de envio, desta forma não é possível enviar a mesma novamente.\r\n" +
                                    NomeArquivoXML);
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

                    if (tudoOK)
                    {
                        try
                        {
                            oFluxoNfe.InserirNfeFluxo(ChaveNfe, oAux.ExtrairNomeArq(arqDestino, ".xml") + ".xml");
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
                try
                {
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.Nfe, Propriedade.ExtRetorno.Nfe_ERR, ex);

                    //Se já foi movido o XML da Nota Fiscal para a pasta em Processamento, vou ter que 
                    //forçar mover para a pasta de XML com erro neste ponto.
                    oAux.MoveArqErro(arqDestino);
                }
                catch
                {
                    //Se ocorrer algum erro na hora de tentar gravar o XML de erro para o ERP ou mover o arquivo XML para a pasta de XML com erro, não 
                    //vou poder fazer nada, pq foi algum erro de rede, permissão de acesso a pasta ou arquivo, etc.
                    //Wandey 13/03/2010
                }

                throw (ex);
            }
        }
        #endregion

        #region LerXMLNFe()
        /// <summary>
        /// Le o conteúdo do XML da NFe
        /// </summary>
        /// <param name="Arquivo">Arquivo XML da NFe</param>
        /// <returns>Retorna o conteúdo do XML da NFe</returns>
        private DadosNFeClass LerXMLNFe(string Arquivo)
        {
            LerXML oLerXML = new LerXML();

            try
            {
                oLerXML.Nfe(Arquivo);
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return oLerXML.oDadosNfe;
        }

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
        protected bool ValidacoesGeraisXMLNFe(string strArquivoNFe, DadosNFeClass oDadosNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            bool booValido = false;
            string cTextoErro = "";

            try
            {
                //Verificar o tipo de emissão se bate com o configurado, se não bater vai retornar um erro 
                //para o ERP
                // danasa 8-2009
                if ((Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "4")) ||
                    (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSCAN && (oDadosNFe.tpEmis == "3")))
                {
                    booValido = true;
                }
                // danasa 8-2009
                else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teContingencia && (oDadosNFe.tpEmis == "2"))
                {
                    booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 09/06/2009
                }
                else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teDPEC && (oDadosNFe.tpEmis == "4"))
                {
                    booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                }
                else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teFSDA && (oDadosNFe.tpEmis == "5"))
                {
                    booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                }
                else
                {
                    booValido = false;

                    // danasa 8-2009
                    if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && oDadosNFe.tpEmis == "3")
                    {
                        cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SEFAZ " +
                            "(Secretaria Estadual da Fazenda) e o XML está configurado para enviar " +
                            "para o SCAN do Ambiente Nacional.\r\n\r\n";

                    }
                    // danasa 8-2009
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSCAN && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5"))
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

                    #region Tag <tpEmis>
                    if (oDadosNFe.tpEmis != oDadosNFe.chavenfe.Substring(37, 1))
                    {
                        cTextoErro += "O código numérico informado na tag <tpEmis> está diferente do informado na chave da NF-e.\r\n" +
                            "Código numérico informado na tag <tpEmis>: " + oDadosNFe.tpEmis + "\r\n" +
                            "Código numérico informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(37, 1) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <cNF>
                    if (oDadosNFe.cNF != oDadosNFe.chavenfe.Substring(38, 8))
                    {
                        cTextoErro += "O código numérico informado na tag <cNF> está diferente do informado na chave da NF-e.\r\n" +
                            "Código numérico informado na tag <cNF>: " + oDadosNFe.cNF + "\r\n" +
                            "Código numérico informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(38, 8) + "\r\n\r\n";
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

        #region LerRetornoLoteNFe()
        protected abstract void LerRetornoLoteNFe();
        #endregion

        #region LerRetornoLoteCTe()
        protected abstract void LerRetornoLoteCTe();
        #endregion

        #region LerRetornoSitNFe()
        protected abstract void LerRetornoSitNFe(string ChaveNFe);
        #endregion

        #region LerRetornoSitCTe()
        protected abstract void LerRetornoSitCTe(string ChaveNFe);
        #endregion

        #region LerRetornoCanc()
        protected abstract void LerRetornoCanc(string xmlCanc);
        #endregion

        #region LerRetDPEC()
        protected abstract void LerRetDPEC();
        #endregion

        #region LerRetornoInut()
        protected abstract void LerRetornoInut();
        #endregion

        #region LerRetEvento()
        protected abstract void LerRetornoEvento(int emp);
        #endregion

        #endregion

        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erros ocorridos durante as operações para que o ERP possa tratá-los        
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de Erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="errorCode">Código do erro</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 02/06/2011
        /// </remarks>
        public void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception)
        {
            try
            {
                GravarArqErroServico(arquivo, finalArqEnvio, finalArqErro, exception, ErroPadrao.ErroNaoDetectado, true);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erros ocorridos durante as operações para que o ERP possa tratá-los        
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de Erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="errorCode">Código do erro</param>
        /// <param name="moveArqErro">Move o arquivo informado no parametro "arquivo" para a pasta de XML com ERRO</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 02/06/2011
        /// </remarks>
        public void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception, bool moveArqErro)
        {
            try
            {
                GravarArqErroServico(arquivo, finalArqEnvio, finalArqErro, exception, ErroPadrao.ErroNaoDetectado, moveArqErro);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erros ocorridos durante as operações para que o ERP possa tratá-los        
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de Erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="errorCode">Código do erro</param>
        /// <param name="moveArqErro">Move o arquivo informado no parametro "arquivo" para a pasta de XML com ERRO</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 02/06/2011
        /// </remarks>
        public void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception, ErroPadrao erroPadrao, bool moveArqErro)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                //Qualquer erro ocorrido o aplicativo vai mover o XML com falha da pasta de envio
                //para a pasta de XML´s com erros. Futuramente ele é excluido quando outro igual
                //for gerado corretamente.
                if (moveArqErro)
                    this.MoveArqErro(arquivo);

                //Grava arquivo de ERRO para o ERP
                string arqErro = Empresa.Configuracoes[emp].PastaRetorno + "\\" +
                                  Functions.ExtrairNomeArq(arquivo, finalArqEnvio) +
                                  finalArqErro;

                string erroMessage = string.Empty;

                erroMessage += "ErrorCode|" + ((int)erroPadrao).ToString("0000000000");
                erroMessage += "\r\n";
                erroMessage += "Message|" + exception.Message;
                erroMessage += "\r\n";
                erroMessage += "StackTrace|" + exception.StackTrace;
                erroMessage += "\r\n";
                erroMessage += "Source|" + exception.Source;
                erroMessage += "\r\n";
                erroMessage += "Type|" + exception.GetType();
                erroMessage += "\r\n";
                erroMessage += "TargetSite|" + exception.TargetSite;
                erroMessage += "\r\n";
                erroMessage += "HashCode|" + exception.GetHashCode().ToString();

                if (exception.InnerException != null)
                {
                    erroMessage += "\r\n";
                    erroMessage += "\r\n";
                    erroMessage += "InnerException 1";
                    erroMessage += "\r\n";
                    erroMessage += "Message|" + exception.InnerException.Message;
                    erroMessage += "\r\n";
                    erroMessage += "StackTrace|" + exception.InnerException.StackTrace;
                    erroMessage += "\r\n";
                    erroMessage += "TargetSite|" + exception.InnerException.TargetSite;
                    erroMessage += "\r\n";
                    erroMessage += "Source|" + exception.InnerException.Source;
                    erroMessage += "\r\n";
                    erroMessage += "HashCode|" + exception.InnerException.GetHashCode().ToString();

                    if (exception.InnerException.InnerException != null)
                    {
                        erroMessage += "\r\n";
                        erroMessage += "\r\n";
                        erroMessage += "InnerException 2";
                        erroMessage += "\r\n";
                        erroMessage += "Message|" + exception.InnerException.InnerException.Message;
                        erroMessage += "\r\n";
                        erroMessage += "StackTrace|" + exception.InnerException.InnerException.StackTrace;
                        erroMessage += "\r\n";
                        erroMessage += "TargetSite|" + exception.InnerException.InnerException.TargetSite;
                        erroMessage += "\r\n";
                        erroMessage += "Source|" + exception.InnerException.InnerException.Source;
                        erroMessage += "\r\n";
                        erroMessage += "HashCode|" + exception.InnerException.InnerException.GetHashCode().ToString();

                        if (exception.InnerException.InnerException.InnerException != null)
                        {
                            erroMessage += "\r\n";
                            erroMessage += "\r\n";
                            erroMessage += "InnerException 3";
                            erroMessage += "\r\n";
                            erroMessage += "Message|" + exception.InnerException.InnerException.InnerException.Message;
                            erroMessage += "\r\n";
                            erroMessage += "StackTrace|" + exception.InnerException.InnerException.InnerException.StackTrace;
                            erroMessage += "\r\n";
                            erroMessage += "TargetSite|" + exception.InnerException.InnerException.InnerException.TargetSite;
                            erroMessage += "\r\n";
                            erroMessage += "Source|" + exception.InnerException.InnerException.InnerException.Source;
                            erroMessage += "\r\n";
                            erroMessage += "HashCode|" + exception.InnerException.InnerException.InnerException.GetHashCode().ToString();
                        }
                    }
                }

                try
                {
                    // Gerar log do erro
                    Auxiliar.WriteLog(erroMessage, true);
                }
                catch
                {
                }

                File.WriteAllText(arqErro, erroMessage, Encoding.Default);

                ///
                /// grava o arquivo de erro no FTP
                new GerarXML(emp).XmlParaFTP(emp, arqErro);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region MoveArqErro
        /// <summary>
        /// Move arquivos XML com erro para uma pasta de xml´s com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg)</example>
        private void MoveArqErro(string Arquivo)
        {
            try
            {
                MoveArqErro(Arquivo, Path.GetExtension(Arquivo));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region MoveArqErro()
        /// <summary>
        /// Move arquivos com a extensão informada e que está com erro para uma pasta de xml´s/arquivos com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <param name="ExtensaoArq">Extensão do arquivo que vai ser movido. Ex: .xml</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg, ".xml")</example>
        private void MoveArqErro(string Arquivo, string ExtensaoArq)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                if (File.Exists(Arquivo))
                {
                    FileInfo oArquivo = new FileInfo(Arquivo);

                    if (Directory.Exists(Empresa.Configuracoes[emp].PastaErro))
                    {
                        string vNomeArquivo = Empresa.Configuracoes[emp].PastaErro + "\\" + Functions.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;

                        Functions.Move(Arquivo, vNomeArquivo);

                        Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para a pasta de XML com problemas.", true);

                        /*
                        //Deletar o arquivo da pasta de XML com erro se o mesmo existir lá para evitar erros na hora de mover. Wandrey
                        if (File.Exists(vNomeArquivo))
                            this.DeletarArquivo(vNomeArquivo);

                        //Mover o arquivo da nota fiscal para a pasta do XML com erro
                        oArquivo.MoveTo(vNomeArquivo);
                        */
                    }
                    else
                    {
                        //Antes estava deletando o arquivo, agora vou retornar uma mensagem de erro
                        //pois não podemos excluir, pode ser coisa importante. Wandrey 25/02/2011
                        throw new Exception("A pasta de XML´s com erro informada nas configurações não existe, por favor verifique.");
                        //oArquivo.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region MoverArquivo()
        /// <summary>
        /// Move arquivos da nota fiscal eletrônica para suas respectivas pastas
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser movido</param>
        /// <param name="PastaXMLEnviado">Pasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="SubPastaXMLEnviado">SubPasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="PastaBackup">Pasta para Backup dos XML´s enviados</param>
        /// <param name="Emissao">Data de emissão da Nota Fiscal ou Data Atual do envio do XML para separação dos XML´s em subpastas por Ano e Mês</param>
        /// <date>16/07/2008</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void MoverArquivo(string arquivo, PastaEnviados subPastaXMLEnviado, DateTime emissao)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                #region Criar pastas que receberão os arquivos
                //Criar subpastas da pasta dos XML´s enviados
                Empresa.CriarSubPastaEnviado(emp);

                //Criar Pasta do Mês para gravar arquivos enviados autorizados ou denegados
                string nomePastaEnviado = string.Empty;
                string destinoArquivo = string.Empty;
                switch (subPastaXMLEnviado)
                {
                    case PastaEnviados.EmProcessamento:
                        nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString();
                        destinoArquivo = nomePastaEnviado + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                        break;

                    case PastaEnviados.Autorizados:
                        nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                        destinoArquivo = nomePastaEnviado + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                        goto default;

                    case PastaEnviados.Denegados:
                        nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                        if (arquivo.ToLower().EndsWith("-den.xml"))//danasa 11-4-2012
                            destinoArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(arquivo));
                        else
                            destinoArquivo = Path.Combine(nomePastaEnviado, Functions.ExtrairNomeArq(arquivo, "-nfe.xml") + "-den.xml");
                        goto default;

                    default:
                        if (!Directory.Exists(nomePastaEnviado))
                        {
                            System.IO.Directory.CreateDirectory(nomePastaEnviado);
                        }
                        break;
                }
                #endregion

                //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                if (Directory.Exists(nomePastaEnviado))
                {
                    #region Mover o XML para a pasta de XML´s enviados
                    //Se for para mover para a Pasta EmProcessamento
                    if (subPastaXMLEnviado == PastaEnviados.EmProcessamento)
                    {
                        //Se já existir o arquivo na pasta EmProcessamento vamos mover 
                        //ele para a pasta com erro antes para evitar exceção. Wandrey 05/07/2011
                        if (File.Exists(destinoArquivo))
                        {
                            string destinoErro = Empresa.Configuracoes[emp].PastaErro + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                            File.Move(destinoArquivo, destinoErro);

                            //danasa 11-4-2012
                            Auxiliar.WriteLog("Arquivo \"" + destinoArquivo + "\" movido para a pasta \"" + Empresa.Configuracoes[emp].PastaErro + "\".", true);
                        }
                        File.Move(arquivo, destinoArquivo);
                    }
                    else
                    {
                        //Se já existir o arquivo na pasta autorizados ou denegado, não vou mover o novo arquivo para lá, pois posso estar sobrepondo algum arquivo importante
                        //Sendo assim se o usuário quiser forçar mover, tem que deletar da pasta autorizados ou denegados manualmente, com isso evitamos perder um XML importante.
                        //Wandrey 05/07/2011
                        if (!File.Exists(destinoArquivo))
                        {
                            File.Move(arquivo, destinoArquivo);
                        }
                        else
                        {
                            string destinoErro = Empresa.Configuracoes[emp].PastaErro + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                            File.Move(arquivo, destinoErro);

                            //danasa 11-4-2012
                            Auxiliar.WriteLog("Arquivo \"" + arquivo + "\" movido para a pasta \"" + Empresa.Configuracoes[emp].PastaErro + "\".", true);
                        }
                    }
                    #endregion

                    if (subPastaXMLEnviado == PastaEnviados.Autorizados || subPastaXMLEnviado == PastaEnviados.Denegados)
                    {
                        #region Copiar XML para a pasta de BACKUP
                        //Fazer um backup do XML que foi copiado para a pasta de enviados
                        //para uma outra pasta para termos uma maior segurança no arquivamento
                        //Normalmente esta pasta é em um outro computador ou HD
                        if (Empresa.Configuracoes[emp].PastaBackup.Trim() != "")
                        {
                            //Criar Pasta do Mês para gravar arquivos enviados
                            string nomePastaBackup = string.Empty;
                            switch (subPastaXMLEnviado)
                            {
                                case PastaEnviados.Autorizados:
                                    nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" + PastaEnviados.Autorizados + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                    goto default;

                                case PastaEnviados.Denegados:
                                    nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" + PastaEnviados.Denegados + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                    goto default;

                                default:
                                    if (!Directory.Exists(nomePastaBackup))
                                    {
                                        System.IO.Directory.CreateDirectory(nomePastaBackup);
                                    }
                                    break;
                            }

                            //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                            if (Directory.Exists(nomePastaBackup))
                            {
                                //Mover o arquivo da nota fiscal para a pasta de backup
                                string destinoBackup = nomePastaBackup + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                                if (File.Exists(destinoBackup))
                                {
                                    File.Delete(destinoBackup);
                                }
                                File.Copy(destinoArquivo, destinoBackup);
                            }
                            else
                            {
                                throw new Exception("Pasta de backup informada nas configurações não existe. (Pasta: " + nomePastaBackup + ")");
                            }
                        }
                        #endregion

                        #region Copiar o XML para a pasta do DanfeMon, se configurado para isso
                        CopiarXMLPastaDanfeMon(destinoArquivo);
                        #endregion

                        #region Copiar o XML para o FTP
                        GerarXML oGerarXML = new GerarXML(emp);
                        oGerarXML.XmlParaFTP(emp, destinoArquivo);
                        #endregion
                    }
                }
                else
                {
                    throw new Exception("Pasta para arquivamento dos XML´s enviados não existe. (Pasta: " + nomePastaEnviado + ")");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region MoverArquivo()
        /// <summary>
        /// Move arquivos da nota fiscal eletrônica para suas respectivas pastas
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser movido</param>
        /// <param name="PastaXMLEnviado">Pasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="SubPastaXMLEnviado">SubPasta de XML´s enviados para onde será movido o arquivo</param>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void MoverArquivo(string Arquivo, PastaEnviados SubPastaXMLEnviado)
        {
            try
            {
                this.MoverArquivo(Arquivo, SubPastaXMLEnviado, DateTime.Now);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region CopiarXMLPastaDanfeMon()
        /// <summary>
        /// Copia o XML da NFe para a pasta monitorada pelo DANFEMon para que o mesmo imprima o DANFe.
        /// A copia só é efetuada de o UniNFe estiver configurado para isso.
        /// </summary>
        /// <param name="arquivoCopiar">Nome do arquivo com as pastas e subpastas a ser copiado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 20/04/2010
        /// </remarks>
        protected void CopiarXMLPastaDanfeMon(string arquivoCopiar)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                if (!string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaDanfeMon))
                {
                    if (Directory.Exists(Empresa.Configuracoes[emp].PastaDanfeMon))
                    {
                        if ((arquivoCopiar.ToLower().Contains("-nfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonNFe) ||
                            (arquivoCopiar.ToLower().Contains("-procnfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                            (arquivoCopiar.ToLower().Contains("-den.xml") && Empresa.Configuracoes[emp].XMLDanfeMonDenegadaNFe))
                        {
                            //Montar o nome do arquivo de destino
                            string arqDestino = Empresa.Configuracoes[emp].PastaDanfeMon + "\\" + Functions.ExtrairNomeArq(arquivoCopiar, ".xml") + ".xml";

                            //Copiar o arquivo para o destino
                            FileInfo oArquivo = new FileInfo(arquivoCopiar);
                            oArquivo.CopyTo(arqDestino, true);
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

        #region ExecutaUniDanfe()
        /// <summary>
        /// Executa o aplicativo UniDanfe para gerar/imprimir o DANFE
        /// </summary>
        /// <param name="NomeArqXMLNFe">Nome do arquivo XML da NFe (final -nfe.xml)</param>
        /// <param name="DataEmissaoNFe">Data de emissão da NFe</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 03/02/2010
        /// </remarks>
        protected void ExecutaUniDanfe(string NomeArqXMLNFe, DateTime DataEmissaoNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
            if (Empresa.Configuracoes[emp].PastaExeUniDanfe != string.Empty &&
                File.Exists(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe"))
            {
                string strNomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(DataEmissaoNFe);
                string strArqProcNFe = strNomePastaEnviado + "\\" + Functions.ExtrairNomeArq(Functions.ExtrairNomeArq(NomeArqXMLNFe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe, ".xml") + ".xml";

                if (File.Exists(strArqProcNFe))
                {
                    string Args = "A=\"" + strArqProcNFe + "\"";
                    if (Empresa.Configuracoes[emp].PastaConfigUniDanfe != string.Empty)
                    {
                        Args += " PC=\"" + Empresa.Configuracoes[emp].PastaConfigUniDanfe + "\"";
                    }

                    System.Diagnostics.Process.Start(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe", Args);
                }
            }
        }
        #endregion
    }
}
 
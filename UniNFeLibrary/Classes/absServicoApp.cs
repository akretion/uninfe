﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Xml;
using UniNFeLibrary.Enums;

namespace UniNFeLibrary
{
    #region Classe ServicoApp
    /// <summary>
    /// Classe responsável pela execução dos serviços do UniNFe
    /// </summary>
    public abstract class absServicoApp
    {
        #region Métodos gerais

        #region BuscaXML()
        /// <summary>
        /// Procurar os arquivos XML´s a serem enviados aos web-services ou para ser executado alguma rotina
        /// </summary>
        /// <param name="pTipoArq">Mascara dos arquivos as serem pesquisados. Ex: *.xml   *-nfe.xml</param>
        public abstract void BuscaXML(Object srvServico);
        #endregion

        #region ProcessaXML()
        /// <summary>
        /// Processa/envia os XML´s gravados na pasta de envio
        /// </summary>
        /// <param name="oNfe">Objeto referente a UniNfeClass</param>
        /// <param name="strPasta">Pasta de envio</param>
        /// <param name="strMascara">Mascara dos arquivos a serem pesquisados e processados</param>
        /// <param name="strAtividade">Atividade a ser executada com o(s) XML(s) encontrados</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        protected void ProcessaXML(Object oNfe, Servicos srvServico)
        {
            string strPasta = string.Empty;
            List<string> lstArquivos = new List<string>();
            lstArquivos.Clear();

            string strMascaraArq = string.Empty;
            string strMetodo = string.Empty;

            switch (srvServico)
            {
                case Servicos.EmProcessamento:
                    // danasa 10-2009
                    this.EmProcessamento();
                    break;

                case Servicos.CancelarNFe:
                    strMetodo = "Cancelamento";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedCan);
                    // danasa 12-9-2009
                    lstArquivos.AddRange(this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedCan_TXT));
                    goto default;

                case Servicos.InutilizarNumerosNFe:
                    strMetodo = "Inutilizacao";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedInu);
                    // danasa 12-9-2009
                    lstArquivos.AddRange(this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedInu_TXT));
                    goto default;

                case Servicos.PedidoConsultaSituacaoNFe:
                    strMetodo = "Consulta";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedSit);
                    // danasa 12-9-2009
                    lstArquivos.AddRange(this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedSit_TXT));
                    goto default;

                case Servicos.PedidoConsultaStatusServicoNFe:
                    strMetodo = "StatusServico";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedSta);
                    // danasa 12-9-2009
                    lstArquivos.AddRange(this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedSta_TXT));
                    goto default;

                case Servicos.PedidoSituacaoLoteNFe:
                    strMetodo = "RetRecepcao";
                    this.GerarXMLPedRec(oNfe);
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.PedRec); //Tem que ficar depois de ter gerado os XML de consulta do recibo ou dá falha. Wandrey 22/05/2009
                    goto default;

                case Servicos.ConsultaCadastroContribuinte:
                    strMetodo = "ConsultaCadastro";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.ConsCad);
                    // danasa 12-9-2009
                    lstArquivos.AddRange(this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.ConsCad_TXT));
                    goto default;

                case Servicos.ConsultaInformacoesUniNFe:
                    strMetodo = "GravarXMLDadosCertificado";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.ConsInf);
                    //danasa 12-9-2009
                    lstArquivos.AddRange(this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.ConsInf_TXT));
                    goto default;

                case Servicos.AlterarConfiguracoesUniNFe:
                    strMetodo = "ReconfigurarUniNfe";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.AltCon);
                    //danasa 12-9-2009
                    lstArquivos.AddRange(this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.AltCon_TXT));
                    goto default;

                case Servicos.AssinarNFePastaEnvio:
                    this.AssinarValidarNFe(oNfe, ConfiguracaoApp.vPastaXMLEnvio);
                    break;

                case Servicos.AssinarNFePastaEnvioEmLote:
                    if (ConfiguracaoApp.cPastaXMLEmLote != string.Empty)
                    {
                        this.AssinarValidarNFe(oNfe, ConfiguracaoApp.cPastaXMLEmLote);
                    }
                    break;

                case Servicos.MontarLoteUmaNFe:
                    this.MontarLoteUmaNfe(oNfe);
                    break;

                case Servicos.MontarLoteVariasNFe:
                    if (ConfiguracaoApp.cPastaXMLEmLote != string.Empty)
                    {
                        this.MontarLoteVariasNfe(oNfe);
                    }
                    break;

                case Servicos.EnviarLoteNfe:
                    strMetodo = "Recepcao";
                    lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.EnvLot);
                    goto default;

                case Servicos.ValidarAssinar:
                    this.AssinarValidarXML(); //Somente validar e assinar os diversos XML´s da NFe
                    break;

                case Servicos.ConverterTXTparaXML:
                    this.ConvTXT(ConfiguracaoApp.vPastaXMLEnvio);
                    break;

                ///
                /// danasa 9-2009
                /// 
                case Servicos.GerarChaveNFe:
                    this.GerarChaveNFe();
                    break;

                default:  //Assinar, validar, enviar ou somente processar os arquivos XML encontrados na pasta de envio
                    for (int i = 0; i < lstArquivos.Count; i++)
                    {
                        //Se o arquivo estiver ainda em uso eu pulo ele para tentar mais tarde
                        if (Auxiliar.FileInUse(lstArquivos[i]))
                            continue;

                        string cError = "";
                        try
                        {
                            //Processa ou envia o XML
                            this.EnviarArquivo(lstArquivos[i], oNfe, strMetodo);
                        }
                        catch (IOException ex)
                        {
                            ///
                            /// danasa 9-2009
                            /// 
                            cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                        }
                        catch (Exception ex)
                        {
                            ///
                            /// danasa 9-2009
                            /// 
                            cError = (ex.InnerException != null ? ex.InnerException.Message: ex.Message);
                        }
                        ///
                        /// danasa 9-2009
                        /// 
                        if (!string.IsNullOrEmpty(cError))
                        {
                            Auxiliar oAux = new Auxiliar();
                            ///
                            /// grava o arquivo de erro
                            /// 
                            oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(lstArquivos[i]) + ".err", cError);
                            ///
                            /// move o arquivo para a pasta de erro
                            /// 
                            oAux.MoveArqErro(lstArquivos[i]);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region AssinarValidarArquivo()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="oNfe">Objeto do UniNFeClass a ser utilizado</param>
        /// <param name="strPasta">Pasta onde está os XML´s</param>
        private void AssinarValidarNFe(Object oNfe, string strPasta)
        {
            //Definir o tipo do serviço
            Type tipoServico = oNfe.GetType();

            //Monta a lista de XML´s encontrados na pasta
            List<string> lstArquivos = this.ArquivosPasta(strPasta, "*" + ExtXml.Nfe);

            //Assinar, Validar, Enviar ou somente processar os arquivos XML encontrados na pasta de envio
            for (int i = 0; i < lstArquivos.Count; i++)
            {
                //Se o arquivo estiver em uso eu pulo ele para tentar mais tarde
                if (Auxiliar.FileInUse(lstArquivos[i]))
                    continue;

                string cError = "";
                try
                {
                    //Definir o arquivo XML 
                    tipoServico.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.SetProperty, null, oNfe, new object[] { lstArquivos[i] });

                    //Assinar e Validar o XML de nota fiscal eletrônica e coloca na pasta de Assinados
                    tipoServico.InvokeMember("AssinarValidarXMLNFe", System.Reflection.BindingFlags.InvokeMethod, null, oNfe, new Object[] { strPasta });
                }
                catch (IOException ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                ///
                /// danasa 9-2009
                /// 
                if (!string.IsNullOrEmpty(cError))
                {
                    Auxiliar oAux = new Auxiliar();
                    ///
                    /// grava o arquivo de erro
                    /// 
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(lstArquivos[i]) + ".err", cError);
                    ///
                    /// move o arquivo para a pasta de erro
                    /// 
                    oAux.MoveArqErro(lstArquivos[i]);
                }
            }
        }
        #endregion

        #region EmProcessamento
        protected abstract void EmProcessamento();
        #endregion

        #region EnviarArquivo()
        /// <summary>
        /// Analisa o tipo do XML que está na pasta de envio e executa a operação necessária. Exemplo: Envia ao SEFAZ, reconfigura o UniNFE, etc... 
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML a ser enviado ou analisado</param>
        /// <param name="oNfe">Objeto da classe UniNfeClass a ser utilizado nas operações</param>
        private void EnviarArquivo(string cArquivo, Object oNfe, string strMetodo)
        {
            #region Código retirado
            /*
             * Não pode ser verificado neste ponto, visto que os retornos esperados pelo ERP podem se modificar, assim
             * sendo movi esta parte do código para dentro dos métodos da classe InvocarObjeto, lá ele vai dar exatamente
             * o retorno que o ERP espera e já trata todos os serviços que precisam da internet e os que não precisam
             * continuam funcionando normalmente.
             * Wandrey 16/11/2009
            if (!InternetCS.IsConnectedToInternet())
            {
                //Registrar o erro da validação para o sistema ERP
                throw new Exception("Sem conexão com a internet.\r\nMétodo: " + strMetodo + "\r\nArquivo: " + cArquivo);
            }
            */
            #endregion

            //Definir o tipo do serviço
            Type tipoServico = oNfe.GetType();

            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.SetProperty, null, oNfe, new object[] { cArquivo });

            try
            {
                //TODO: CONFIG
                if (ConfiguracaoApp.tpEmis != TipoEmissao.teContingencia/*2*/) //2-Confingência em Formulário de segurança não envia na hora, tem que aguardar voltar para normal.
                {
                    if (strMetodo == "ReconfigurarUniNfe")
                    {
                        this.ReconfigurarUniNFe(cArquivo);
                    }
                    else if (strMetodo == "GravarXMLDadosCertificado")
                    {
                        this.GravarXMLDadosCertificado(cArquivo);
                    }
                    else
                    {
                        tipoServico.InvokeMember(strMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oNfe, null);
                    }
                }
                else
                {
                    if (strMetodo == "ReconfigurarUniNfe")
                    {
                        this.ReconfigurarUniNFe(cArquivo);
                    }
                    else if (strMetodo == "RetRecepcao" || strMetodo == "Consulta" || strMetodo == "StatusServico")
                    {
                        tipoServico.InvokeMember(strMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oNfe, null);
                    }
                    else if (strMetodo == "GravarXMLDadosCertificado")
                    {
                        this.GravarXMLDadosCertificado(cArquivo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region ArquivosPasta()
        /// <summary>
        /// Monta uma lista dos arquivos existentes em uma determinada pasta
        /// </summary>
        /// <param name="strPasta">Pasta a ser verificada a existencia de arquivos</param>
        /// <param name="strMascara">Mascara dos arquivos a serem procurados</param>
        /// <returns>Retorna a lista dos arquivos da pasta</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        protected List<string> ArquivosPasta(string strPasta, string strMascara)
        {
            //Criar uma Lista dos arquivos existentes na pasta
            List<string> lstArquivos = new List<string>();

            if (strPasta.Trim() != "" && Directory.Exists(strPasta))
            {
                string cError = "";
                try
                {
                    string[] filesInFolder = Directory.GetFiles(strPasta, strMascara);
                    foreach (string item in filesInFolder)
                    {
                        lstArquivos.Add(item);
                    }
                }
                catch (IOException ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                catch (Exception ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                if (!string.IsNullOrEmpty(cError))
                {
                    new Auxiliar().GravarArqErroERP(string.Format(InfoApp.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddThhmmss")), cError);
                    lstArquivos.Clear();
                }
            }
            return lstArquivos;
        }
        #endregion

        #region MontarLoteUmaNfe()
        /// <summary>
        /// Monta o um lote para cada NFe
        /// </summary>
        /// <param name="oNfe">Objeto referente a instância da classe UniNfeClass</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/04/2009</date>
        private void MontarLoteUmaNfe(Object oNfe)
        {
            //Definir o tipo do serviço
            Type tipoServico = oNfe.GetType();

            List<string> lstArquivos = new List<string>();
            FluxoNfe oFluxoNfe = new FluxoNfe();

            lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio + ConfiguracaoApp.NomePastaXMLAssinado, "*" + ExtXml.Nfe);
            for (int i = 0; i < lstArquivos.Count; i++)
            {
                string cError = "";
                try
                {
                    absLerXML.DadosNFeClass oDadosNfe = this.LerXMLNFe(lstArquivos[i]);
                    if (!oFluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                    {
                        //Gerar lote
                        tipoServico.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.SetProperty, null, oNfe, new object[] { lstArquivos[i] });
                        tipoServico.InvokeMember("LoteNfe", System.Reflection.BindingFlags.InvokeMethod, null, oNfe, new object[] { lstArquivos[i] });
                    }
                }
                catch (IOException ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                catch (Exception ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                ///
                /// danasa 9-2009
                /// 
                if (!string.IsNullOrEmpty(cError))
                {
                    Auxiliar oAux = new Auxiliar();
                    ///
                    /// grava o arquivo de erro
                    /// 
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(lstArquivos[i]) + ".err", cError);
                    ///
                    /// move o arquivo para a pasta de erro
                    /// 
                    oAux.MoveArqErro(lstArquivos[i]);
                }
            }
        }
        #endregion

        #region MontarLoteVariasNfe()
        /// <summary>
        /// Monta o um lote com várias NFe´s
        /// </summary>
        /// <param name="oNfe">Objeto referente a instância da classe absServicoNFe</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>28/04/2009</date>
        private void MontarLoteVariasNfe(Object oNfe)
        {
            List<string> lstArqMontarLote = new List<string>();

            //Aguardar a assinatura de todos os arquivos da pasta de lotes
            lstArqMontarLote = this.ArquivosPasta(ConfiguracaoApp.cPastaXMLEmLote, "*" + ExtXml.Nfe);
            if (lstArqMontarLote.Count > 0) return;

            //Verificar se existe o arquivo que solicita a montagem do lote
            lstArqMontarLote = this.ArquivosPasta(ConfiguracaoApp.cPastaXMLEmLote, "*" + ExtXml.MontarLote);

            for (int b = 0; b < lstArqMontarLote.Count; b++)
            {
                string NomeArquivo = lstArqMontarLote[b];

                //O arquivo existe mas pode estar em uso
                if (Auxiliar.FileInUse(NomeArquivo) == true)
                    return;

                Auxiliar oAux = new Auxiliar();
                List<string> lstNfe = new List<string>();
                FileStream fsArquivo = null;
                FluxoNfe oFluxoNfe = new FluxoNfe();

                string MensagemErro = string.Empty;
                bool lTeveErro = false;

                try
                {
                    XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                    fsArquivo = new FileStream(NomeArquivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); //Abrir um arquivo XML usando FileStream
                    doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                    XmlNodeList documentoList = doc.GetElementsByTagName("MontarLoteNFe"); //Pesquisar o elemento Documento no arquivo XML
                    foreach (XmlNode documentoNode in documentoList)
                    {
                        XmlElement documentoElemento = (XmlElement)documentoNode;

                        int QtdeArquivo = documentoElemento.GetElementsByTagName("ArquivoNFe").Count;

                        for (int d = 0; d < QtdeArquivo; d++)
                        {
                            string ArquivoNFe = ConfiguracaoApp.cPastaXMLEmLote + ConfiguracaoApp.NomePastaXMLAssinado + "\\" + documentoElemento.GetElementsByTagName("ArquivoNFe")[d].InnerText;

                            if (File.Exists(ArquivoNFe))
                            {

                                try
                                {
                                    absLerXML.DadosNFeClass oDadosNfe = this.LerXMLNFe(ArquivoNFe);
                                    if (!oFluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                                    {
                                        lstNfe.Add(ArquivoNFe);
                                    }
                                    else
                                    {
                                        MensagemErro += "Arquivo: " + ArquivoNFe + " já está no fluxo de envio e não será incluido em novo lote.\r\n";
                                        lTeveErro = true;

                                        FileInfo oArq = new FileInfo(ArquivoNFe);
                                        oArq.Delete();
                                    }
                                }
                                catch (IOException ex)
                                {
                                    MensagemErro += (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + "\r\n";
                                    lTeveErro = true;
                                }
                                catch (Exception ex)
                                {
                                    MensagemErro += (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + "\r\n";
                                    lTeveErro = true;
                                }

                            }
                            else
                            {
                                lTeveErro = true;
                                MensagemErro += "Arquivo: " + ArquivoNFe + " não existe e não será incluido no lote!\r\n";
                            }
                        }
                    }

                    fsArquivo.Close(); //Fecha o arquivo XML

                    //Definir o tipo do serviço
                    Type tipoServico = oNfe.GetType();

                    try
                    {
                        //Gerar lote
                        tipoServico.InvokeMember("LoteNfe", System.Reflection.BindingFlags.InvokeMethod, null, oNfe, new object[] { lstNfe });
                    }
                    catch (IOException ex)
                    {
                        MensagemErro += (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + "\r\n";
                        lTeveErro = true;
                    }
                    catch (Exception ex)
                    {
                        MensagemErro += (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + "\r\n";
                        lTeveErro = true;
                    }
                }
                catch (Exception ex)
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    lTeveErro = true;
                    MensagemErro += (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + "\r\n";
                }

                //Deletar o arquivo de solicitão de montagem do lote de NFe
                try
                {
                    FileInfo oArquivo = new FileInfo(NomeArquivo);
                    oArquivo.Delete();
                }
                catch (IOException ex)
                {
                    lTeveErro = true;
                    MensagemErro += (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + "\r\n";
                }
                catch (Exception ex)
                {
                    lTeveErro = true;
                    MensagemErro += (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + "\r\n";
                }

                if (lTeveErro)
                {
                    oAux.GravarArqErroServico(NomeArquivo, ExtXml.MontarLote/*"-montar-lote.xml"*/, "-montar-lote.err", MensagemErro);
                }
            }
        }
        #endregion

        #region GravarXMLDadosCertificado()
        /// <summary>
        /// Gravar o XML de retorno com as informações do UniNFe para o aplicativo de ERP
        /// </summary>
        /// <param name="oNfe">Objeto da classe UniNfeClass para conseguir pegar algumas informações para gravar o XML</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        private void GravarXMLDadosCertificado(string ArquivoXml)
        {
            InfoApp oInfUniNfe = new InfoApp();

            //Deletar o arquivo de solicitação do serviço
            FileInfo oArquivo = new FileInfo(ArquivoXml);
            oArquivo.Delete();

            Auxiliar oAux = new Auxiliar();

            if (Path.GetExtension(ArquivoXml).ToLower() == ".txt")
                oInfUniNfe.GravarXMLInformacoes(ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                                                oAux.ExtrairNomeArq(ArquivoXml, ExtXml.ConsInf) + "-ret-cons-inf.txt");
            else
                oInfUniNfe.GravarXMLInformacoes(ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                                                oAux.ExtrairNomeArq(ArquivoXml, ExtXml.ConsInf) + "-ret-cons-inf.xml");
        }
        #endregion

        #region ReconfigurarUniNFe()
        /// <summary>
        /// Reconfigura o UniNFe, gravando as novas informações na tela de configuração
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML contendo as novas configurações</param>
        protected void ReconfigurarUniNFe(string cArquivo)
        {
            ConfiguracaoApp oConfig = new ConfiguracaoApp();
            oConfig.ReconfigurarUniNFe(cArquivo);
        }
        #endregion

        #region GerarXMLPedRec()
        /// <summary>
        /// Gera o XML de consulta do recibo do lote de notas enviadas
        /// </summary>
        /// <param name="oNfe">Objeto referente a classe UniNFeClass a ser utilizado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        private void GerarXMLPedRec(object oNfe)
        {
            Type tipoServico = oNfe.GetType();

            //Criar a lista dos recibos a serem consultados no SEFAZ
            List<ReciboCons> lstRecibo = new List<ReciboCons>();

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                lstRecibo = oFluxoNfe.CriarListaRec();
            }
            catch
            {
                //Não precisa fazer nada se não conseguiu criar a lista, somente con
            }

            ReciboCons oReciboCons;

            for (int i = 0; i < lstRecibo.Count; i++)
            {
                oReciboCons = lstRecibo[i];
                int intTempoConsulta = 0;
                if (oReciboCons.tMed > 10)
                {
                    intTempoConsulta = oReciboCons.tMed / 2;
                }

                if (DateTime.Now.Subtract(oReciboCons.dPedRec).Seconds >= intTempoConsulta)
                {
                    //Atualizar a tag da data e hora da ultima consulta do recibo
                    oFluxoNfe.AtualizarDPedRec(oReciboCons.nRec, DateTime.Now);
                    tipoServico.InvokeMember("XmlPedRec", System.Reflection.BindingFlags.InvokeMethod, null, oNfe, new object[] { oReciboCons.nRec });
                }
            }
        }
        #endregion

        #region AssinarValidarXML()
        private void AssinarValidarXML()
        {
            ///
            /// danasa 21-9-2009
            /// 
            this.ConvTXT(ConfiguracaoApp.PastaValidar);

            List<string> lstMascaras = new List<string>();
            lstMascaras.Add(ExtXml.Nfe);
            lstMascaras.Add(ExtXml.EnvLot);
            lstMascaras.Add(ExtXml.PedRec);
            lstMascaras.Add(ExtXml.PedSit);
            lstMascaras.Add(ExtXml.PedSta);
            lstMascaras.Add(ExtXml.PedCan);
            lstMascaras.Add(ExtXml.PedInu);
            lstMascaras.Add(ExtXml.PedSta);
            lstMascaras.Add(ExtXml.ConsCad);
            lstMascaras.Add(ExtXmlRet.ProcCancNFe);
            lstMascaras.Add(ExtXmlRet.ProcInutNFe);
            lstMascaras.Add(ExtXmlRet.ProcNFe);

            Auxiliar oAux = new Auxiliar();

            List<string> lstArquivos = new List<string>();

            for (int i = 0; i < lstMascaras.Count; i++)
            {
                lstArquivos = this.ArquivosPasta(ConfiguracaoApp.PastaValidar, "*" + lstMascaras[i]);

                for (int b = 0; b < lstArquivos.Count; b++)
                {
                    if (Auxiliar.FileInUse(lstArquivos[b]))
                        continue;

                    oAux.ValidarAssinarXML(lstArquivos[b]);
                }
                lstArquivos.Clear();
            }
            Thread.Sleep(2000);
        }
        #endregion

        #region ConvTXT()
        /// <summary>
        /// Converter arquivos de NFe no formato TXT para XML
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/069/2009</date>
        protected abstract void ConvTXT(string vPasta);
        #endregion

        #region LerXMLNfe()
        protected abstract absLerXML.DadosNFeClass LerXMLNFe(string Arquivo);
        #endregion

        #region LerXMLRecibo()
        protected abstract absLerXML.DadosRecClass LerXMLRecibo(string Arquivo);
        #endregion

        #region GerarChaveNFe()
        /// <summary>
        /// Monta a chave da NFe
        /// </summary>
        /// <param name="lstArquivos"></param>
        /// <returns></returns>
        protected abstract void GerarChaveNFe();
        #endregion

        #endregion
    }
    #endregion
}
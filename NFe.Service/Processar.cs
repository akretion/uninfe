using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using NFe.Components;
using NFe.Settings;
using NFe.Exceptions;
using NFe.ConvertTxt;
using NFe.Validate;
using NFe.Certificado;
using NFe.Components.Info;

namespace NFe.Service
{
    public class Processar
    {
        #region Métodos gerais

        #region ProcessaArquivo()
        public void ProcessaArquivo(string arquivo, Servicos servico)
        {
            try
            {
                int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

                try
                {
                    if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                    {
                        #region Executar o serviço da NFS-e
                        switch (servico)
                        {
                            case Servicos.ConsultarLoteRps:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                PedidoConsultaLoteRps(new Task(), arquivo);
                                break;

                            case Servicos.CancelarNfse:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                CancelarNfse(new Task(), arquivo);
                                break;

                            case Servicos.ConsultarSituacaoLoteRps:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                PedidoConsultaSituacaoLoteRps(new Task(), arquivo);
                                break;

                            case Servicos.ConsultarNfse:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                PedidoConsultaNfse(new Task(), arquivo);
                                break;

                            case Servicos.ConsultarNfsePorRps:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                PedidoConsultaNfsePorRps(new Task(), arquivo);
                                break;

                            case Servicos.RecepcionarLoteRps:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                RecepcionarLoteRps(new Task(), arquivo);
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Executar servico da NF-e e CT-e
                        switch (servico)
                        {
                            case Servicos.PedidoConsultaSituacaoNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                PedidoConsultaSituacaoNFe(new Task(), arquivo);
                                break;

                            case Servicos.PedidoConsultaStatusServicoNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                PedidoConsultaStatusServicoNFe(new Task(), arquivo);
                                break;

                            case Servicos.ConsultaCadastroContribuinte:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                ConsultaCadastroContribuinte(new Task(), arquivo);
                                break;

                            case Servicos.CancelarNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                CancelarNFe(new Task(), arquivo);
                                break;

                            case Servicos.InutilizarNumerosNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                InutilizarNumerosNFe(new Task(), arquivo);
                                break;

                            case Servicos.PedidoSituacaoLoteNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                PedidoSituacaoLoteNFe(new Task(), arquivo);
                                break;

                            case Servicos.MontarLoteUmaNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                MontarLoteUmaNFe(new Task(), arquivo);
                                break;

                            case Servicos.EnviarLoteNfe:
                                EnviarLoteNfe(new Task(), arquivo);
                                break;

                            case Servicos.GerarChaveNFe:
                                GerarChaveNFe(arquivo);
                                break;

                            case Servicos.EnviarDPEC:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                EnviarDPEC(new Task(), arquivo);
                                break;

                            case Servicos.ConsultarDPEC:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                ConsultarDPEC(new Task(), arquivo);
                                break;

                            case Servicos.ConverterTXTparaXML:
                                ConverterTXTparaXML(arquivo);
                                break;

                            case Servicos.AssinarValidarNFe:
                                CertVencido(emp);

                                AssinarValidarNFe(new Task(), arquivo, Empresa.Configuracoes[emp].PastaEnvioEmLote);
                                break;

                            case Servicos.MontarLoteVariasNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();

                                MontarLoteVariasNFe(new Task(), arquivo);
                                break;

                            case Servicos.EnviarCCe:
                            case Servicos.EnviarManifestacao:
                            case Servicos.EnviarEventoCancelamento:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                EnviarEvento(new Task(), arquivo);
                                break;

                            case Servicos.ConsultaNFeDest:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                EnviarConsultarNFeDest(new Task(), arquivo);
                                break;

                            case Servicos.DownloadNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                EnviarDownloadNFe(new Task(), arquivo);
                                break;
                        }
                        #endregion
                    }

                    #region Serviços em comum
                    switch (servico)
                    {
                        case Servicos.AssinarValidar:
                            CertVencido(emp);

                            AssinarValidar(arquivo);
                            break;

                        case Servicos.ConsultaInformacoesUniNFe:
                            ConsultaInformacoesUniNFe(arquivo);
                            break;

                        case Servicos.AlterarConfiguracoesUniNFe:
                            AlterarConfiguracoesUniNFe(arquivo);
                            break;
                    }
                    #endregion
                }
                catch (ExceptionSemInternet ex)
                {
                    GravaErroERP(arquivo, servico, ex, ex.ErrorCode);
                }
                catch (ExceptionCerticicadoDigital ex)
                {
                    GravaErroERP(arquivo, servico, ex, ex.ErrorCode);
                }
                catch
                {
                }
                finally
                {
                }
            }
            catch { }
        }
        #endregion

        #region Métodos para execução dos servicos

        #region PedidoConsultaSituacaoNFe()
        /// <summary>
        /// Executar as tarefas pertinentes a consulta da situação da NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="Arquivo">Arquivo a ser tratado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 18/04/2011
        /// </remarks>
        protected void PedidoConsultaSituacaoNFe(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "Consulta");
        }
        #endregion

        #region PedidoConsultaStatusServicoNFe()
        /// <summary>
        /// Executa as tarefas pertinentes a consulta status dos serviços da NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void PedidoConsultaStatusServicoNFe(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "StatusServico");
        }
        #endregion

        #region ConsultaCadastroContribuinte()
        /// <summary>
        /// Executa as tarefas pertinentes a consulta do cadastro de contribuintes
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void ConsultaCadastroContribuinte(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "ConsultaCadastro");
        }
        #endregion

        #region CancelarNFe()
        /// <summary>
        /// Executa as tarefas pertinentes ao Cancelamento de NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void CancelarNFe(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "Cancelamento");
        }
        #endregion

        #region InutilizarNumerosNFe()
        /// <summary>
        /// Executa as tarefas pertinentes a Inutilização de Números da NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void InutilizarNumerosNFe(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "Inutilizacao");
        }
        #endregion

        #region PedidoSituacaoLoteNFe()
        /// <summary>
        /// Executa as tarefas pertinentes a consulta situação do lote da NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void PedidoSituacaoLoteNFe(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "RetRecepcao");
        }
        #endregion

        #region MontarLoteUmaNFe()
        /// <summary>
        /// Executar as tarefas pertinentes a assinatura e montagem do lote de uma única nota fiscal eletrônica
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void MontarLoteUmaNFe(Object nfe, string arquivo)
        {
            try
            {
                int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
                AssinarValidarNFe(nfe, arquivo, Empresa.Configuracoes[emp].PastaEnvio);

                Auxiliar oAux = new Auxiliar();

                //Definir o tipo do serviço
                Type tipoServico = nfe.GetType();

                //Definir o nome do arquivo assinado
                FileInfo arq = new FileInfo(arquivo);
                string arquivoAssinado = arq.DirectoryName.Substring(0, arq.DirectoryName.Length - 5) + Propriedade.NomePastaXMLAssinado + "\\" + arq.Name;

                //Montar lote de nfe
                FluxoNfe oFluxoNfe = new FluxoNfe();

                string cError = "";
                try
                {
                    DadosNFeClass oDadosNfe = this.LerXMLNFe(arquivoAssinado);
                    if (!oFluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                    {
                        //Gerar lote
                        tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivoAssinado });
                        tipoServico.InvokeMember("LoteNfe", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { arquivoAssinado });
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
                    try
                    {
                        ///
                        /// grava o arquivo de erro
                        /// 
                        oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(arquivoAssinado) + ".err", cError);
                        ///
                        /// move o arquivo para a pasta de erro
                        /// 
                        oAux.MoveArqErro(arquivoAssinado);
                    }
                    catch
                    {
                        //A principio não vou fazer nada Wandrey 24/04/2011
                    }
                }
            }
            catch { }
        }
        #endregion

        #region AssinarValidarNFe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarNFe(Object nfe, string arquivo, string pasta)
        {
            //Se o arquivo estiver em uso eu pulo ele para tentar mais tarde
            try
            {
                //                if (!Auxiliar.FileInUse(arquivo))
                //                {
                //Definir o tipo do serviço
                Type tipoServico = nfe.GetType();

                //Definir o arquivo XML 
                tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivo });

                //Assinar e Validar o XML de nota fiscal eletrônica e coloca na pasta de Assinados
                tipoServico.InvokeMember("AssinarValidarXMLNFe", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new Object[] { pasta });
                //                }
            }
            catch (Exception ex)
            {
                try
                {
                    #region Código retirado, tem uma observação na linha abaixo explicando. Wandrey 24/08/2011
                    //Não precisamos gravar nenhum erro aqui, pois já é gravado dentro do método "AssinarValidarXMLNFe" chamado acima
                    /*
                    Auxiliar oAux = new Auxiliar();
                    ///
                    /// grava o arquivo de erro
                    /// 
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(arquivo) + ".err", ex.Message);
                    ///
                    /// move o arquivo para a pasta de erro
                    /// 
                    oAux.MoveArqErro(arquivo);
                    */
                    #endregion
                }
                catch
                {
                    //Se der algum erro não faço nada neste ponto, pq não tenho muito o que fazer
                }

                throw (ex);
            }
        }
        #endregion

        #region MontarLoteVariasNFe()
        /// <summary>
        /// Executar as tarefas pertinentes a assinatura e montagem do lote de várias notas fiscais eletrônicas
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void MontarLoteVariasNFe(Object nfe, string arquivo)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            Auxiliar oAux = new Auxiliar();
            List<string> arquivosNFe = new List<string>();

            //Aguardar a assinatura de todos os arquivos da pasta de lotes
            arquivosNFe = oAux.ArquivosPasta(Empresa.Configuracoes[emp].PastaEnvioEmLote, "*" + Propriedade.ExtEnvio.Nfe);
            if (arquivosNFe.Count == 0) // && !Auxiliar.FileInUse(arquivo))
            {
                List<string> notas = new List<string>();
                FileStream fsArquivo = null;
                FluxoNfe fluxoNfe = new FluxoNfe();

                try
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); //Abrir um arquivo XML usando FileStream
                        doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        XmlNodeList documentoList = doc.GetElementsByTagName("MontarLoteNFe"); //Pesquisar o elemento Documento no arquivo XML
                        foreach (XmlNode documentoNode in documentoList)
                        {
                            XmlElement documentoElemento = (XmlElement)documentoNode;

                            int QtdeArquivo = documentoElemento.GetElementsByTagName("ArquivoNFe").Count;

                            for (int d = 0; d < QtdeArquivo; d++)
                            {
                                string arquivoNFe = Empresa.Configuracoes[emp].PastaEnvioEmLote + Propriedade.NomePastaXMLAssinado + "\\" + documentoElemento.GetElementsByTagName("ArquivoNFe")[d].InnerText;

                                if (File.Exists(arquivoNFe))
                                {

                                    try
                                    {
                                        DadosNFeClass oDadosNfe = this.LerXMLNFe(arquivoNFe);
                                        if (!fluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                                        {
                                            notas.Add(arquivoNFe);
                                        }
                                        else
                                        {
                                            throw new Exception("Arquivo: " + arquivoNFe + " já está no fluxo de envio e não será incluido em novo lote.");

                                            //File.Delete(arquivoNFe);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw (ex);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Arquivo: " + arquivoNFe + " não existe e não será incluido no lote!");
                                }
                            }
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML

                        //Definir o tipo do serviço
                        Type tipoServico = nfe.GetType();

                        try
                        {
                            //Gerar lote
                            tipoServico.InvokeMember("LoteNfe", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { notas });
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (fsArquivo != null)
                        {
                            fsArquivo.Close();
                        }

                        throw (ex);
                    }

                    //Deletar o arquivo de solicitão de montagem do lote de NFe
                    try
                    {
                        FileInfo oArquivo = new FileInfo(arquivo);
                        oArquivo.Delete();
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
                        new Task().GravarArqErroServico(arquivo, Propriedade.ExtEnvio.MontarLote, "-montar-lote.err", ex);
                    }
                    catch
                    {
                        //Se deu algum erro na hora de gravar o arquivo de erro de retorno para o ERP, infelizmente não poderemos fazer nada
                        //pois deve estar ocorrendo alguma falha de rede, hd, permissão de acesso a pasta ou arquivos, etc. Wandrey 22/03/2010
                    }
                }
            }
        }
        #endregion

        #region EnviarLoteNfe()
        /// <summary>
        /// Executar tarefas pertinentes ao envio do lote de NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void EnviarLoteNfe(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "Recepcao");
        }
        #endregion

        #region GerarChaveNFe()
        /// <summary>
        /// Executa tarefas pertinentes a geração da Chave da NFe solicitado pelo ERP
        /// </summary>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void GerarChaveNFe(string arquivo)
        {
            Auxiliar oAux = new Auxiliar();

            FileInfo fi = new FileInfo(arquivo);
            // processa arquivos XML
            if (fi.Extension.ToLower() == ".xml")
            {
                new NFeW().GerarChaveNFe(arquivo, true);
            }
            // processa arquivos TXT
            else
            {
                new NFeW().GerarChaveNFe(arquivo, false);
            }
        }
        #endregion

        #region EnviarDPEC()
        /// <summary>
        /// Executa as tarefas pertinentes ao envio do xml do DPEC
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void EnviarDPEC(object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "RecepcaoDPEC");
        }
        #endregion

        #region ConsultarDPEC()
        /// <summary>
        /// Executa as tarefas pertinentes ao envio do xml da consulta do DPEC
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void ConsultarDPEC(object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "ConsultaDPEC");
        }
        #endregion

        #region AssinarValidar()
        /// <summary>
        /// Executa as tarefas pertinentes ao processo de somente assinar e validar os arquivos
        /// </summary>
        /// <param name="arquivo">Arquivo a ser assinado e validado</param>
        protected void AssinarValidar(string arquivo)
        {
            try
            {
                if (arquivo.ToLower().IndexOf(Propriedade.ExtEnvio.Nfe_TXT) > 0)
                    ConvTXT(arquivo);
                else
                {
                    int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
                    ValidarXML validar = new ValidarXML(arquivo, Empresa.Configuracoes[emp].UFCod);
                    validar.ValidarAssinarXML(arquivo);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region ConverterTXTparaXML()
        /// <summary>
        /// Executa as tarefas pertinentes da conversão de NF-e em TXT para XML 
        /// </summary>
        /// <param name="arquivo">Nome do arquivo a ser convertido</param>
        protected void ConverterTXTparaXML(string arquivo)
        {
            try
            {
                ConvTXT(arquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region ConsultaInformacoesUniNFe()
        /// <summary>
        /// Executa as tarefas pertinentes a consulta das informações do UniNFe
        /// </summary>
        /// <param name="arquivo">Arquivo a ser tratado/param>
        protected void ConsultaInformacoesUniNFe(string arquivo)
        {
            try
            {
                GravarXMLDadosCertificado(arquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region AlterarConfiguracoesUniNFe()
        /// <summary>
        /// Executa as tarefas pertinentes a consulta das informações do UniNFe
        /// </summary>
        /// <param name="arquivo">Arquivo a ser tratado/param>
        protected void AlterarConfiguracoesUniNFe(string arquivo)
        {
            try
            {
                ReconfigurarUniNFe(arquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region LimpezaTemporario()
        /// <summary>
        /// Executar as tarefas pertinentes a limpeza de arquivos temporários
        /// </summary>
        public void LimpezaTemporario()
        {
            while (true)
            {
                ExecutaLimpeza();

                Thread.Sleep(new TimeSpan(1, 0, 0, 0));
            }
        }
        #endregion

        #region EmProcessamento()
        /// <summary>
        /// Executar as tarefas pertinentes a analise das notas em processamento a mais de 5 minutos
        /// </summary>
        public void EmProcessamento()
        {
            //NFeEmProcessamento nfe = new NFeEmProcessamento();

            while (true)
            {
                for (int i = 0; i < Empresa.Configuracoes.Count; i++)
                {
                    BackgroundWorker worker = new BackgroundWorker();

                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());
                    worker.DoWork += new DoWorkEventHandler(ExecutarEmProcessamento);
                    worker.RunWorkerAsync(i);

                    //Thread t = new Thread(nfe.Analisar);
                    //t.Name = i.ToString();
                    //t.Start();
                    //t.Join();
                }

                Thread.Sleep(300000); //Dorme por 5 minutos, e executa novamente
            }
        }

        public void ExecutarEmProcessamento(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.Name = e.Argument.ToString();
            NFeEmProcessamento nfe = new NFeEmProcessamento();
            nfe.Analisar();
        }
        #endregion

        #region GerarXMLPedRec()
        /// <summary>
        /// Executa as tarefas pertinentes a geração dos pedidos de consulta situação de lote da NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe servico NFe</param>
        public void GerarXMLPedRec(object nfe)
        {
            while (true)
            {
                for (int i = 0; i < Empresa.Configuracoes.Count; i++)
                {
                    GerarXMLPedRec(i, nfe);
                }

                Thread.Sleep(2000);
            }
        }
        #endregion

        #region EnviarEvento
        /// <summary>
        /// Executa as tarefas pertinentes ao envio do xml da CCe
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="arquivo"></param>
        protected void EnviarEvento(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "RecepcaoEvento");
        }
        #endregion

        #region EnviarDownloadNFe
        /// <summary>
        /// Executa as tarefas pertinentes ao envio do xml de download das nfe recebidas
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="arquivo"></param>
        protected void EnviarDownloadNFe(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "RecepcaoDownloadNFe");
        }
        #endregion

        #region EnviarConsultarNFeDest
        /// <summary>
        /// Executa as tarefas pertinentes ao envio do xml de consulta das nfe recebidas
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="arquivo"></param>
        protected void EnviarConsultarNFeDest(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "RecepcaoConsultaNFeDest");
        }
        #endregion

        #endregion

        #region DirecionarArquivo()
        /// <summary>
        /// Direcionar os arquivos encontrados na pasta de envico corretamente
        /// </summary>
        /// <param name="arquivos">Lista de arquivos</param>
        /// <param name="metodo">Método a ser executado do serviço da NFe</param>
        /// <param name="nfe">Objeto do serviço da NFe a ser executado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 18/04/2011
        /// </remarks>
        private void DirecionarArquivo(List<string> arquivos, object nfe, string metodo)
        {
            for (int i = 0; i < arquivos.Count; i++)
            {
                DirecionarArquivo(arquivos[i], nfe, metodo);
            }
        }
        #endregion

        #region DirecionarArquivo()
        /// <summary>
        /// Direcionar o arquivo
        /// </summary>
        /// <param name="arquivos">Arquivo</param>
        /// <param name="metodo">Método a ser executado do serviço da NFe</param>
        /// <param name="nfe">Objeto do serviço da NFe a ser executado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 18/04/2011
        /// </remarks>
        private void DirecionarArquivo(string arquivo, object nfe, string metodo)
        {
            try
            {
                //Se o arquivo estiver ainda em uso eu pulo ele para tentar mais tarde
                //                if (!Auxiliar.FileInUse(arquivo))
                //                {
                //Processa ou envia o XML
                EnviarArquivo(arquivo, nfe, metodo);
                //                }
            }
            catch
            {
                //Não pode ser tratado nenhum erro aqui, visto que já estão sendo tratados e devidamente retornados
                //para o ERP no ponto da execução dos serviços. Foi muito bem testado e analisado. Wandrey 09/03/2010
            }
        }
        #endregion

        #region EnviarArquivo()
        /// <summary>
        /// Analisa o tipo do XML que está na pasta de envio e executa a operação necessária. Exemplo: Envia ao SEFAZ, reconfigura o UniNFE, etc... 
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML a ser enviado ou analisado</param>
        /// <param name="oNfe">Objeto da classe UniNfeClass a ser utilizado nas operações</param>
        private void EnviarArquivo(string arquivo, Object nfe, string metodo)
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

            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o tipo do serviço
            Type tipoServico = nfe.GetType();

            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivo });

            try
            {
                if (Empresa.Configuracoes[emp].tpEmis != Propriedade.TipoEmissao.teContingencia &&
                    Empresa.Configuracoes[emp].tpEmis != Propriedade.TipoEmissao.teFSDA &&
                    Empresa.Configuracoes[emp].tpEmis != Propriedade.TipoEmissao.teDPEC) //Confingência em formulário de segurança e DPEC não envia na hora, tem que aguardar voltar para normal.
                {
                    tipoServico.InvokeMember(metodo, System.Reflection.BindingFlags.InvokeMethod, null, nfe, null);
                }
                else
                {
                    if (metodo == "RetRecepcao" ||
                        metodo == "StatusServico" ||
                        metodo == "RecepcaoDPEC" ||
                        metodo == "Consulta" ||
                        metodo == "ConsultaDPEC" ||
                        metodo == "ConsultaCadastro")
                    {
                        tipoServico.InvokeMember(metodo, System.Reflection.BindingFlags.InvokeMethod, null, nfe, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            string sArqRetorno = string.Empty;

            Auxiliar oAux = new Auxiliar();

            if (Path.GetExtension(ArquivoXml).ToLower() == ".txt")
                sArqRetorno = Empresa.Configuracoes[emp].PastaRetorno + "\\" +
                              Functions/*oAux*/.ExtrairNomeArq(ArquivoXml, Propriedade.ExtEnvio.ConsInf_XML) + "-ret-cons-inf.txt";
            else
                sArqRetorno = Empresa.Configuracoes[emp].PastaRetorno + "\\" +
                              Functions/*oAux*/.ExtrairNomeArq(ArquivoXml, Propriedade.ExtEnvio.ConsInf_XML) + "-ret-cons-inf.xml";

            try
            {
                Aplicacao app = new Aplicacao();

                //Deletar o arquivo de solicitação do serviço
                FileInfo oArquivo = new FileInfo(ArquivoXml);
                oArquivo.Delete();

                app.GravarXMLInformacoes(sArqRetorno);
            }
            catch (Exception ex)
            {
                try
                {
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(sArqRetorno) + ".err", ex.Message);
                }
                catch
                {
                    //Se também falhou gravar o arquivo de retorno para o ERP, infelizmente não posso fazer mais nada. Deve estar com algum problema na rede, HD, permissão de acesso as pastas, etc... Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region ReconfigurarUniNFe()
        /// <summary>
        /// Reconfigura o UniNFe, gravando as novas informações na tela de configuração
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML contendo as novas configurações</param>
        protected void ReconfigurarUniNFe(string cArquivo)
        {
            try
            {
                ConfiguracaoApp oConfig = new ConfiguracaoApp();
                oConfig.ReconfigurarUniNFe(cArquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region GerarXMLPedRec()
        /// <summary>
        /// Gera o XML de consulta do recibo do lote de notas enviadas
        /// </summary>
        /// <param name="empresa">Index da empresa que é para gerar os pedidos de consulta do recibo do lote da nfe</param>
        /// <param name="nfe">Objeto da classe ServicoNfe</param>
        /// <by>Wandrey Mundin Ferreira</by>
        private void GerarXMLPedRec(int empresa, object nfe)
        {
            //Criar a lista dos recibos a serem consultados no SEFAZ
            List<ReciboCons> recibos = new List<ReciboCons>();

            FluxoNfe fluxoNfe = new FluxoNfe(empresa);

            try
            {
                recibos = fluxoNfe.CriarListaRec();
            }
            catch
            {
                //Não precisa fazer nada se não conseguiu criar a lista, somente con
            }

            Type tipoServico = nfe.GetType();

            for (int i = 0; i < recibos.Count; i++)
            {
                ReciboCons reciboCons = recibos[i];
                var tempoConsulta = reciboCons.tMed;

                //Vou dar no mínimo 2 segundos para efetuar a consulta do recibo. Wandrey 20/07/2010
                if (tempoConsulta < Empresa.Configuracoes[empresa].TempoConsulta)
                    tempoConsulta = Empresa.Configuracoes[empresa].TempoConsulta;

                if (tempoConsulta < 2)
                    tempoConsulta = 2;

                if (DateTime.Now.Subtract(reciboCons.dPedRec).Seconds >= tempoConsulta)
                {
                    //Atualizar a tag da data e hora da ultima consulta do recibo aumentando 10 segundos
                    fluxoNfe.AtualizarDPedRec(reciboCons.nRec, DateTime.Now.AddSeconds(10));
                    tipoServico.InvokeMember("XmlPedRec", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { empresa, reciboCons.nRec });
                }
            }
        }
        #endregion

        #region ConvTXT()
        /// <summary>
        /// Converter o arquivo de NFe do formato TXT para XML
        /// </summary>
        /// <param name="arquivo">Nome completo do arquivo a ser convertido (Pasta e arquivo)</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// </remarks>
        protected void ConvTXT(String arquivo)
        {
            //            if (Auxiliar.FileInUse(arquivo))
            //                return;


            Auxiliar oAux = new Auxiliar();

            NFe.ConvertTxt.ConversaoTXT oUniTxtToXml = new NFe.ConvertTxt.ConversaoTXT();

            string pasta = new FileInfo(arquivo).DirectoryName;
            pasta = pasta.Substring(0, pasta.Length - 5); //Retirar a pasta \Temp do final - Wandrey 03/08/2011

            string ccMessage = string.Empty;
            string ccExtension = "-nfe.err";

            try
            {
                int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

                ///
                /// exclui o arquivo de erro
                /// 
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileName(Functions/*oAux*/.ExtrairNomeArq(arquivo, "-nfe.txt") + ccExtension));
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileName(Functions/*oAux*/.ExtrairNomeArq(arquivo, "-nfe.txt") + "-nfe-ret.xml"));
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaErro + "\\" + Path.GetFileName(arquivo));
                ///
                /// exclui o arquivo TXT original
                /// 
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileNameWithoutExtension(arquivo) + "-orig.txt");

                ///
                /// processa a conversão
                /// 
                oUniTxtToXml.Converter(arquivo, pasta, Empresa.Configuracoes[emp].PastaRetorno);

                //Deu tudo certo com a conversão?
                if (string.IsNullOrEmpty(oUniTxtToXml.cMensagemErro))
                {
                    ///
                    /// danasa 8-2009
                    /// 
                    if (oUniTxtToXml.cRetorno.Count == 0)
                    {
                        ccMessage = "cStat=02\r\n" +
                            "xMotivo=Falha na conversão. Sem informações para converter o arquivo texto";

                        oAux.MoveArqErro(arquivo, ".txt");
                    }
                    else
                    {
                        ///
                        /// salva o arquivo texto original
                        ///
                        //FileInfo otxtArquivo = new FileInfo(arquivo);
                        if (pasta.ToLower().Equals(Empresa.Configuracoes[emp].PastaEnvio.ToLower()))
                        {
                            FileInfo ArqOrig = new FileInfo(arquivo);

                            string vvNomeArquivoDestino = Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileNameWithoutExtension(arquivo) + "-orig.txt";
                            ArqOrig.MoveTo(vvNomeArquivoDestino);
                        }
                        ccExtension = "-nfe.txt";
                        ccMessage = "cStat=01\r\n" +
                            "xMotivo=Convertido com sucesso. Foi(ram) convertida(s) " + oUniTxtToXml.cRetorno.Count.ToString() + " nota(s) fiscal(is)";

                        foreach (NFe.ConvertTxt.txtTOxmlClassRetorno txtClass in oUniTxtToXml.cRetorno)
                        {
                            ///
                            /// monta o texto que será gravado no arquivo de aviso ao ERP
                            /// 
                            ccMessage += Environment.NewLine +
                                    "Nota fiscal: " + txtClass.NotaFiscal.ToString("000000000") +
                                    " Série: " + txtClass.Serie.ToString("000") +
                                    " - ChaveNFe: " + txtClass.ChaveNFe;
                            ///
                            /// move o arquivo XML criado na pasta Envio\Convertidos para a pasta Envio
                            /// ou
                            /// move o arquivo XML criado na pasta Validar\Convertidos para a pasta Validar
                            ///
                            FileInfo oArquivo = new FileInfo(txtClass.XMLFileName);
                            string vNomeArquivoDestino = Path.Combine(pasta, Path.GetFileName(txtClass.XMLFileName));

                            ///
                            /// excluo o XML se já existe
                            /// 
                            Functions.DeletarArquivo(vNomeArquivoDestino);

                            ///
                            /// move o arquivo da pasta "Envio\Convertidos" para a pasta "Envio"
                            /// ou
                            /// move o arquivo da pasta "Validar\Convertidos" para a pasta "Validar"
                            /// 
                            oArquivo.MoveTo(vNomeArquivoDestino);
                        }
                    }
                }
                else
                {
                    ///
                    /// danasa 8-2009
                    /// 
                    ccMessage = "cStat=99\r\n" +
                        "xMotivo=Falha na conversão\r\n" +
                        "MensagemErro=" + oUniTxtToXml.cMensagemErro;
                }
            }
            catch (Exception ex)
            {
                ccMessage = ex.Message;
                ccExtension = "-nfe.err";
            }

            if (!string.IsNullOrEmpty(ccMessage))
            {
                oAux.MoveArqErro(arquivo, ".txt");
                ///
                /// exclui todos os XML gerados na pasta Envio\convertidos
                /// 
                foreach (NFe.ConvertTxt.txtTOxmlClassRetorno txtClass in oUniTxtToXml.cRetorno)
                {
                    Functions.DeletarArquivo(pasta + "\\convertidos\\" + txtClass.XMLFileName);
                }
                ///
                /// danasa 8-2009
                /// 
                /// Gravar o retorno para o ERP em formato TXT com o erro ocorrido
                /// 
                oAux.GravarArqErroERP(Functions/*oAux*/.ExtrairNomeArq(arquivo, "-nfe.txt") + ccExtension, ccMessage);
            }
        }
        #endregion

        #region LerXMLNFe
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

        #region Executa Limpeza
        /// <summary>
        /// executa a limpeza das pastas temp e retorno
        /// </summary>
        /// <by>http://desenvolvedores.net/marcelo</by>
        private void ExecutaLimpeza()
        {
            lock (this)
            {
                //Limpar conteúdo da pasta de LOG, mas manter 60 dias de informação
                Limpar(Propriedade.PastaLog, 60);

                for (int i = 0; i < Empresa.Configuracoes.Count; i++)
                {
                    //Limpar conteúdo da pasta temp que fica dentro da pasta de envio de cada empresa a cada 10 dias
                    Limpar(Empresa.Configuracoes[i].PastaEnvio + "\\temp", 10);
                    Limpar(Empresa.Configuracoes[i].PastaValidar + "\\temp", 10);   //danasa 12/8/2011
                    Limpar(Empresa.Configuracoes[i].PastaEnvioEmLote + "\\temp", 10);   //Wandrey 05/10/2011

                    if (Empresa.Configuracoes[i].DiasLimpeza == 0)
                        continue;

                    #region temporario
                    Limpar(Empresa.Configuracoes[i].PastaErro, Empresa.Configuracoes[i].DiasLimpeza);
                    #endregion

                    #region retorno
                    Limpar(Empresa.Configuracoes[i].PastaRetorno, Empresa.Configuracoes[i].DiasLimpeza);
                    #endregion
                }
            }
        }

        private void Limpar(string diretorio, int diasLimpeza)
        {
            // danasa 27-2-2011
            if (diasLimpeza == 0) return;

            if (!Directory.Exists(diretorio)) return;   //danasa 12/8/2011

            //recupera os arquivos da pasta temporario
            string[] files = Directory.GetFiles(diretorio, "*.*", SearchOption.AllDirectories);
            DateTime UltimaData = DateTime.Today.AddDays(-diasLimpeza);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                //usar a última data de acesso, e não a data de criação
                if (fi.LastWriteTime <= UltimaData)
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch
                    {
                        //td bem... nao deu para excluir. fica pra próxima
                    }
                }

                Application.DoEvents();
            }
        }
        #endregion

        #region CertVencido
        /// <summary>
        /// Verificar se o certificado digital está vencido
        /// </summary>
        /// <param name="emp">Empresa que é para ser verificado o certificado digital</param>
        /// <remarks>
        /// Retorna uma exceção ExceptionCertificadoDigital caso o certificado esteja vencido
        /// </remarks>
        protected void CertVencido(int emp)
        {
            CertificadoDigital CertDig = new CertificadoDigital();
            if (CertDig.Vencido(emp))
            {
                throw new ExceptionCerticicadoDigital(ErroPadrao.CertificadoVencido, "(" + CertDig.dValidadeInicial.ToString() + " a " + CertDig.dValidadeFinal.ToString() + ")");
            }
        }
        #endregion

        #region IsConnectedToInternet()
        /// <summary>
        /// Verifica se a conexão com a internet está OK
        /// </summary>
        /// <remarks>
        /// Retorna uma exceção ExceptionSemInternet caso a internet não esteja OK
        /// </remarks>
        protected void IsConnectedToInternet()
        {
            //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
            if (ConfiguracaoApp.ChecarConexaoInternet)
                if (!Functions.IsConnectedToInternet())
                {
                    throw new ExceptionSemInternet(ErroPadrao.FalhaInternet);
                }
        }
        #endregion

        #region GravaErroERP()
        /// <summary>
        /// Gravar o erro ocorrido para o ERP
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que seria processado</param>
        /// <param name="extRet">Extensão do arquivo de erro a ser gravado</param>
        /// <param name="servico">Serviço que está sendo executado</param>
        /// <param name="ex">Exception gerada</param>
        protected void GravaErroERP(string arquivo, Servicos servico, Exception ex, ErroPadrao erroPadrao)
        {
            string extRetERR = string.Empty;
            string extRet = string.Empty;

            switch (servico)
            {
                case Servicos.CancelarNFe:
                    extRet = Propriedade.ExtEnvio.PedCan_XML;
                    extRetERR = Propriedade.ExtRetorno.Can_ERR;
                    goto default;

                case Servicos.InutilizarNumerosNFe:
                    extRet = Propriedade.ExtEnvio.PedInu_XML;
                    extRetERR = Propriedade.ExtRetorno.Inu_ERR;
                    goto default;

                case Servicos.PedidoConsultaSituacaoNFe:
                    extRet = Propriedade.ExtEnvio.PedSit_XML;
                    extRetERR = Propriedade.ExtRetorno.Sit_ERR;
                    goto default;

                case Servicos.PedidoConsultaStatusServicoNFe:
                    extRet = Propriedade.ExtEnvio.PedSta_XML;
                    extRetERR = Propriedade.ExtRetorno.Sta_ERR;
                    goto default;

                case Servicos.PedidoSituacaoLoteNFe:
                    extRet = Propriedade.ExtEnvio.PedRec_XML;
                    extRetERR = Propriedade.ExtRetorno.ProRec_ERR;
                    goto default;

                case Servicos.ConsultaCadastroContribuinte:
                    extRet = Propriedade.ExtEnvio.ConsCad_XML;
                    extRetERR = Propriedade.ExtRetorno.ConsCad_ERR;
                    goto default;

                case Servicos.ConsultaInformacoesUniNFe:
                    //Não tem definição pois não gera arquivo .ERR
                    break;

                case Servicos.AlterarConfiguracoesUniNFe:
                    //Não tem definição pois não gera arquivo .ERR
                    break;

                case Servicos.MontarLoteUmaNFe:
                    extRet = Propriedade.ExtEnvio.Nfe;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    goto default;

                case Servicos.MontarLoteVariasNFe:
                    extRet = Propriedade.ExtEnvio.MontarLote;
                    extRetERR = Propriedade.ExtRetorno.MontarLote_ERR;
                    goto default;

                case Servicos.EnviarLoteNfe:
                    extRet = Propriedade.ExtEnvio.EnvLot;
                    extRetERR = Propriedade.ExtRetorno.Rec_ERR;
                    break;

                case Servicos.AssinarValidar:
                    //Não tem definição pois não gera arquivo .ERR
                    break;

                case Servicos.ConverterTXTparaXML:
                    //Não tem definição pois não gera arquivo .ERR
                    break;

                case Servicos.GerarChaveNFe:
                    //Não tem definição pois não gera arquivo .ERR
                    break;

                case Servicos.EmProcessamento:
                    //Não tem definição pois não gera arquivo .ERR
                    break;

                case Servicos.LimpezaTemporario:
                    //Não tem definição pois não gera arquivo .ERR
                    break;

                case Servicos.EnviarDPEC:
                    extRet = Propriedade.ExtEnvio.EnvDPEC_XML;
                    extRetERR = Propriedade.ExtRetorno.retDPEC_ERR;
                    goto default;

                case Servicos.ConsultarDPEC:
                    extRet = Propriedade.ExtEnvio.ConsDPEC_XML;
                    extRetERR = Propriedade.ExtRetorno.retConsDPEC_ERR;
                    goto default;

                case Servicos.AssinarValidarNFe:
                    extRet = Propriedade.ExtEnvio.Nfe;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    goto default;

                case Servicos.EnviarCCe:
                    extRet = Propriedade.ExtEnvio.EnvCCe_XML;
                    extRetERR = Propriedade.ExtRetorno.retEnvCCe_ERR;
                    goto default;

                case Servicos.EnviarManifestacao:
                    extRet = Propriedade.ExtEnvio.EnvManifestacao_XML;
                    extRetERR = Propriedade.ExtRetorno.retManifestacao_ERR;
                    goto default;

                case Servicos.EnviarEventoCancelamento:
                    extRet = Propriedade.ExtEnvio.EnvCancelamento_XML;
                    extRetERR = Propriedade.ExtRetorno.retCancelamento_ERR;
                    goto default;

                case Servicos.DownloadNFe:
                    extRet = Propriedade.ExtEnvio.EnvDownload_XML;
                    extRetERR = Propriedade.ExtRetorno.retDownload_ERR;
                    goto default;

                case Servicos.ConsultaNFeDest:
                    extRet = Propriedade.ExtEnvio.ConsNFeDest_XML;
                    extRetERR = Propriedade.ExtRetorno.retConsNFeDest_ERR;
                    goto default;

                case Servicos.RecepcionarLoteRps:
                    extRet = Propriedade.ExtEnvio.EnvLoteRps;
                    extRetERR = Propriedade.ExtRetorno.RetLoteRps_ERR;
                    goto default;

                case Servicos.ConsultarSituacaoLoteRps:
                    extRet = Propriedade.ExtEnvio.PedSitLoteRps;
                    extRetERR = Propriedade.ExtRetorno.SitLoteRps_ERR;
                    goto default;

                case Servicos.ConsultarNfsePorRps:
                    extRet = Propriedade.ExtEnvio.PedSitNfseRps;
                    extRetERR = Propriedade.ExtRetorno.SitNfseRps_ERR;
                    goto default;

                case Servicos.ConsultarNfse:
                    extRet = Propriedade.ExtEnvio.PedSitNfse;
                    extRetERR = Propriedade.ExtRetorno.SitNfse_ERR;
                    goto default;

                case Servicos.ConsultarLoteRps:
                    extRet = Propriedade.ExtEnvio.PedLoteRps;
                    extRetERR = Propriedade.ExtRetorno.LoteRps_ERR;
                    goto default;

                case Servicos.CancelarNfse:
                    extRet = Propriedade.ExtEnvio.PedCanNfse;
                    extRetERR = Propriedade.ExtRetorno.CanNfse_ERR;
                    goto default;

                default:
                    try
                    {
                        //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                        new Task().GravarArqErroServico(arquivo, extRet, extRetERR, ex, erroPadrao, true);
                    }
                    catch
                    {
                        //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                        //Wandrey 02/06/2011
                    }
                    break;
            }
        }
        #endregion

        #endregion

        #region Métodos de controle da NFS-e

        #region PedidoConsultaLoteRps()
        /// <summary>
        /// Executar as tarefas pertinentes a consulta da situação do lote RPS
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="Arquivo">Arquivo a ser tratado</param>
        protected void PedidoConsultaLoteRps(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "ConsultarLoteRps");
        }
        #endregion

        #region CancelarNfse()
        /// <summary>
        /// Executar as tarefas pertinentes ao cancelamento de NFS-e
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="Arquivo">Arquivo a ser tratado</param>
        protected void CancelarNfse(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "CancelarNfse");
        }
        #endregion

        #region PedidoConsultaSituacaoLoteRps()
        /// <summary>
        /// Executar as tarefas pertinentes ao pedido de consulta situação do lote de RPS
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="Arquivo">Arquivo a ser tratado</param>
        protected void PedidoConsultaSituacaoLoteRps(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "ConsultarSituacaoLoteRps");
        }
        #endregion

        #region PedidoConsultaNfse()
        /// <summary>
        /// Executar as tarefas pertinentes ao pedido de consulta da situação da Nfse
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="Arquivo">Arquivo a ser tratado</param>
        protected void PedidoConsultaNfse(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "ConsultarNfse");
        }
        #endregion

        #region PedidoConsultaNfsePorRps()
        /// <summary>
        /// Executar as tarefas pertinentes ao pedido de consulta da situação da Nfse por Rps
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="Arquivo">Arquivo a ser tratado</param>
        protected void PedidoConsultaNfsePorRps(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "ConsultarNfsePorRps");
        }
        #endregion

        #region RecepcionarLoteRps()
        /// <summary>
        /// Executar as tarefas pertinentes a recepção do lote RPS
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="Arquivo">Arquivo a ser tratado</param>
        protected void RecepcionarLoteRps(Object nfe, string arquivo)
        {
            DirecionarArquivo(arquivo, nfe, "RecepcionarLoteRps");
        }
        #endregion

        #endregion
    }
}
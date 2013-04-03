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
        public void ProcessaArquivo(int emp, string arquivo)//, Servicos servico)
        {
            try
            {
                Servicos servico = this.DefinirTipoServico(emp, arquivo);
                try
                {
                    if (servico == Servicos.Nulo)
                        throw new Exception("Não pode identificar o tipo de serviço baseado no arquivo " + arquivo);

                    if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                    {
                        #region Executar o serviço da NFS-e
                        switch (servico)
                        {
                            case Servicos.ConsultarLoteRps:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                this.DirecionarArquivo(arquivo, new NFSe.TaskConsultarLoteRps());
                                break;

                            case Servicos.CancelarNfse:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                this.DirecionarArquivo(arquivo, new NFSe.TaskCancelarNfse());
                                break;

                            case Servicos.ConsultarSituacaoLoteRps:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                this.DirecionarArquivo(arquivo, new NFSe.TaskConsultaSituacaoLoteRps());
                                break;

                            case Servicos.ConsultarNfse:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                this.DirecionarArquivo(arquivo, new NFSe.TaskConsultarNfse());
                                break;

                            case Servicos.ConsultarNfsePorRps:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                this.DirecionarArquivo(arquivo, new NFSe.TaskConsultarNfsePorRps());
                                break;

                            case Servicos.RecepcionarLoteRps:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                this.DirecionarArquivo(arquivo, new NFSe.TaskRecepcionarLoteRps());
                                break;

                            case Servicos.ConsultarURLNfse:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                this.DirecionarArquivo(arquivo, new NFSe.TaskConsultarURLNfse());
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
                                DirecionarArquivo(arquivo, new TaskPedidoConsultaSituacaoNFe());
                                break;

                            case Servicos.PedidoConsultaStatusServicoNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskConsultaStatus());
                                break;

                            case Servicos.ConsultaCadastroContribuinte:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskCadastroContribuinte());
                                break;

                            case Servicos.CancelarNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskCancelamento());
                                break;

                            case Servicos.InutilizarNumerosNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskInutilizacao());
                                break;

                            case Servicos.PedidoSituacaoLoteNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskRetRecepcao());
                                break;

                            case Servicos.MontarLoteUmaNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskMontarLoteUmaNFe());
                                break;

                            case Servicos.MontarLoteVariasNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskMontarLoteVariasNFe());
                                break;

                            case Servicos.EnviarLoteNfe:
                                DirecionarArquivo(arquivo, new TaskNFeRecepcao());
                                break;

                            case Servicos.GerarChaveNFe:
                                GerarChaveNFe(arquivo);
                                break;

                            case Servicos.EnviarDPEC:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskRecepcaoDPEC());
                                break;

                            case Servicos.ConsultarDPEC:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskConsultaDPEC());
                                break;

                            case Servicos.ConverterTXTparaXML:
                                ConverterTXTparaXML(arquivo);
                                break;

                            case Servicos.AssinarValidarNFe:
                                CertVencido(emp);
                                AssinarValidarNFe(arquivo, Empresa.Configuracoes[emp].PastaEnvioEmLote);
                                break;

                            case Servicos.EnviarCCe:
                            case Servicos.EnviarManifDest:
                            case Servicos.EnviarEventoCancelamento:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskEventos());
                                break;

                            case Servicos.ConsultaNFDest:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskConsultaNFDest());
                                break;

                            case Servicos.DownloadNFe:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskDownloadNFe());
                                break;

                            case Servicos.RegistroDeSaida:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskRegistroDeSaida());
                                break;

                            case Servicos.RegistroDeSaidaCancelamento:
                                CertVencido(emp);
                                IsConnectedToInternet();
                                DirecionarArquivo(arquivo, new TaskRegistroDeSaidaCancelamento());
                                break;

                            case Servicos.WSExiste:
                                DirecionarArquivo(arquivo, new TaskWSExiste());
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

                        case Servicos.ConsultaGeral:
                            ConsultarGeral(arquivo);
                            break;
                    }
                    #endregion
                }
                catch (ExceptionSemInternet ex)
                {
                    GravaErroERP(arquivo, servico, ex, ex.ErrorCode);
                }
                catch (ExceptionCertificadoDigital ex)
                {
                    GravaErroERP(arquivo, servico, ex, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    if (servico == Servicos.Nulo || servico == Servicos.PedidoConsultaStatusServicoNFe)
                    {
                        /// 7/2012 <<< danasa
                        ///o erp nao precisa esperar pelo tempo excedido, então retornamos um arquivo .err
                        ///
                        GravaErroERP(arquivo, servico, ex, ErroPadrao.ErroNaoDetectado);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region DefinirTipoServico()
        /// <summary>
        /// Definir o tipo do servico a ser executado a partir da extensão do arquivo
        /// </summary>
        /// <param name="fullPath">Nome do arquivo completo do qual é para definir o tipo de serviço a ser executado</param>
        /// <returns>Retorna o tipo do serviço que deve ser executado</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 27/04/2011
        /// </remarks>
        public Servicos DefinirTipoServico(int empresa, string fullPath)
        {
            Servicos tipoServico = Servicos.Nulo;

            try
            {
                string arq = fullPath.ToLower().Trim();

                #region Serviços que funcionam tanto na pasta Geral como na pasta da Empresa
                if (arq.IndexOf(Propriedade.ExtEnvio.ConfigEmp) >= 0)
                {
                    tipoServico = Servicos.CadastrarEmpresa;
                }
                else if (arq.IndexOf(Propriedade.ExtEnvio.ConsCertificado) >= 0)
                {
                    tipoServico = Servicos.ConsultaGeral;
                }
                else if (arq.IndexOf(Propriedade.ExtEnvio.AltCon_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.AltCon_TXT) >= 0)
                {
                    tipoServico = Servicos.AlterarConfiguracoesUniNFe;
                }
                #endregion
                else
                {
                    if (arq.IndexOf(Empresa.Configuracoes[empresa].PastaValidar.ToLower()) >= 0)
                    {
                        tipoServico = Servicos.AssinarValidar;
                    }
                    else
                    {
                        if (arq.IndexOf(Propriedade.ExtEnvio.PedSit_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedSit_TXT) >= 0)
                        {
                            tipoServico = Servicos.PedidoConsultaSituacaoNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedSta_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedSta_TXT) >= 0)
                        {
                            tipoServico = Servicos.PedidoConsultaStatusServicoNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsCad_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsCad_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaCadastroContribuinte;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedCan_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedCan_TXT) >= 0)
                        {
                            tipoServico = Servicos.CancelarNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedInu_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedInu_TXT) >= 0)
                        {
                            tipoServico = Servicos.InutilizarNumerosNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedRec_XML) >= 0)
                        {
                            tipoServico = Servicos.PedidoSituacaoLoteNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.Nfe) >= 0)
                        {
                            FileInfo infArq = new FileInfo(arq);
                            string pastaArq = ConfiguracaoApp.RemoveEndSlash(infArq.DirectoryName).ToLower().Trim();
                            string pastaLote = ConfiguracaoApp.RemoveEndSlash(Empresa.Configuracoes[empresa].PastaEnvioEmLote).ToLower().Trim();
                            string pastaEnvio = ConfiguracaoApp.RemoveEndSlash(Empresa.Configuracoes[empresa].PastaEnvio).ToLower().Trim();

                            //Remover a subpasta temp
                            if (pastaArq.EndsWith("\\temp"))
                                pastaArq = Path.GetDirectoryName(pastaArq);

                            //Definir o serviço
                            if (pastaArq == pastaLote)
                                tipoServico = Servicos.AssinarValidarNFe;
                            else if (pastaArq == pastaEnvio)
                                tipoServico = Servicos.MontarLoteUmaNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.Nfe_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConverterTXTparaXML;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvLot) >= 0)
                        {
                            tipoServico = Servicos.EnviarLoteNfe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.GerarChaveNFe_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.GerarChaveNFe_TXT) >= 0)
                        {
                            tipoServico = Servicos.GerarChaveNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_TXT) >= 0)
                        {
                            tipoServico = Servicos.WSExiste;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDPEC_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvDPEC_TXT) >= 0)
                        {
                            tipoServico = Servicos.EnviarDPEC;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsDPEC_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsDPEC_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultarDPEC;
                        }
                        /*                        else if (arq.IndexOf(Propriedade.ExtEnvio.AltCon_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.AltCon_TXT) >= 0)
                                                {
                                                    tipoServico = Servicos.AlterarConfiguracoesUniNFe;
                                                }*/
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsInf_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsInf_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaInformacoesUniNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCCe_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvCCe_TXT) >= 0)
                        {
                            tipoServico = Servicos.EnviarCCe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCancelamento_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvCancelamento_TXT) >= 0)
                        {
                            tipoServico = Servicos.EnviarEventoCancelamento;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvManifestacao_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvManifestacao_TXT) >= 0)
                        {
                            tipoServico = Servicos.EnviarManifDest;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsNFeDest_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsNFeDest_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaNFDest;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDownload_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvDownload_TXT) >= 0)
                        {
                            tipoServico = Servicos.DownloadNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvRegistroDeSaida_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvRegistroDeSaida_TXT) >= 0)
                        {
                            tipoServico = Servicos.RegistroDeSaida;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCancRegistroDeSaida_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvCancRegistroDeSaida_TXT) >= 0)
                        {
                            tipoServico = Servicos.RegistroDeSaidaCancelamento;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.MontarLote) >= 0)
                        {
                            if (arq.IndexOf(Empresa.Configuracoes[empresa].PastaEnvioEmLote.ToLower().Trim()) >= 0)
                            {
                                tipoServico = Servicos.MontarLoteVariasNFe;
                            }
                        }
                        #region NFS-e
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedLoteRps) >= 0)
                        {
                            tipoServico = Servicos.ConsultarLoteRps;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedCanNfse) >= 0)
                        {
                            tipoServico = Servicos.CancelarNfse;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitLoteRps) >= 0)
                        {
                            tipoServico = Servicos.ConsultarSituacaoLoteRps;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvLoteRps) >= 0)
                        {
                            tipoServico = Servicos.RecepcionarLoteRps;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitNfse) >= 0)
                        {
                            tipoServico = Servicos.ConsultarNfse;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitNfseRps) >= 0)
                        {
                            tipoServico = Servicos.ConsultarNfsePorRps;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedURLNfse) >= 0)
                        {
                            tipoServico = Servicos.ConsultarURLNfse;
                        }
                        #endregion
                    }
                }
            }
            catch
            {
            }

            return tipoServico;
        }
        #endregion

        #region AssinarValidarNFe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarNFe(string arquivo, string pasta)
        {
            try
            {
                TaskAssinarValidarNFe nfe = new TaskAssinarValidarNFe();
                nfe.NomeArquivoXML = arquivo;
                nfe.AssinarValidarXMLNFe(pasta);

                /*
                //Definir o tipo do serviço
                Type tipoServico = nfe.GetType();

                //Definir o arquivo XML 
                tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivo });
                
                //Assinar e Validar o XML de nota fiscal eletrônica e coloca na pasta de Assinados
                tipoServico.InvokeMember("AssinarValidarXMLNFe", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new Object[] { pasta });
                 */
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
                    new ConverterTXT(arquivo);
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

        #region ConverterTXTparaXML()
        /// <summary>
        /// Executa as tarefas pertinentes da conversão de NF-e em TXT para XML 
        /// </summary>
        /// <param name="arquivo">Nome do arquivo a ser convertido</param>
        protected void ConverterTXTparaXML(string arquivo)
        {
            try
            {
                new ConverterTXT(arquivo);
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
                }

                Thread.Sleep(720000); //Dorme por 12 minutos, para atender o problema do consumo indevido da SEFAZ
            }
        }

        public void ExecutarEmProcessamento(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.Name = e.Argument.ToString();
            NFeEmProcessamento nfe = new NFeEmProcessamento();
            nfe.Analisar();
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
                    Empresa empresa = Empresa.Configuracoes[i];

                    if (!string.IsNullOrEmpty(empresa.PastaEmpresa))
                        GerarXMLPedRec(i, nfe);
                }

                Thread.Sleep(2000);
            }
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
        private void DirecionarArquivo(string arquivo, object taskClass)
        {
            try
            {
                //Processa ou envia o XML
                EnviarArquivo(arquivo, taskClass, "Execute");
            }
            catch (Exception ex)
            {
                //Não pode ser tratado nenhum erro aqui, visto que já estão sendo tratados e devidamente retornados
                //para o ERP no ponto da execução dos serviços. Foi muito bem testado e analisado. Wandrey 09/03/2010
                if (taskClass is TaskConsultaStatus)
                    throw (ex);
            }
        }

        private void DirecionarArquivo(string arquivo, object nfe, string metodo)
        {
            try
            {
                //Processa ou envia o XML
                EnviarArquivo(arquivo, nfe, metodo);
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

            try
            {
                int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

                //Definir o tipo do serviço
                Type tipoServico = nfe.GetType();

                //Definir o arquivo XML para a classe UniNfeClass
                tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivo });

                if (Empresa.Configuracoes[emp].tpEmis != Propriedade.TipoEmissao.teContingencia &&
                    Empresa.Configuracoes[emp].tpEmis != Propriedade.TipoEmissao.teFSDA &&
                    Empresa.Configuracoes[emp].tpEmis != Propriedade.TipoEmissao.teDPEC) //Confingência em formulário de segurança e DPEC não envia na hora, tem que aguardar voltar para normal.
                {
                    tipoServico.InvokeMember(metodo, System.Reflection.BindingFlags.InvokeMethod, null, nfe, null);
                }
                else
                {
                    if (//metodo == "Execute" ||
                        nfe is TaskRetRecepcao ||               //metodo == "RetRecepcao" ||
                        nfe is TaskConsultaStatus ||            //metodo == "StatusServico" ||
                        nfe is TaskRecepcaoDPEC ||              //metodo == "RecepcaoDPEC" ||
                        nfe is TaskPedidoConsultaSituacaoNFe || //metodo == "Consulta" ||
                        nfe is TaskConsultaDPEC ||              //metodo == "ConsultaDPEC" ||
                        nfe is TaskCadastroContribuinte)        // || metodo == "ConsultaCadastro")
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
                              Functions.ExtrairNomeArq(ArquivoXml, Propriedade.ExtEnvio.ConsInf_TXT) + "-ret-cons-inf.txt";
            else
                sArqRetorno = Empresa.Configuracoes[emp].PastaRetorno + "\\" +
                              Functions.ExtrairNomeArq(ArquivoXml, Propriedade.ExtEnvio.ConsInf_XML) + "-ret-cons-inf.xml";

            try
            {
                Aplicacao app = new Aplicacao();

                //Deletar o arquivo de solicitação do serviço
                FileInfo oArquivo = new FileInfo(ArquivoXml);
                oArquivo.Delete();

                oArquivo = new FileInfo(sArqRetorno);
                if (oArquivo.Exists)
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
                throw new ExceptionCertificadoDigital(ErroPadrao.CertificadoVencido, "(" + CertDig.dValidadeInicial.ToString() + " a " + CertDig.dValidadeFinal.ToString() + ")");
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

                case Servicos.EnviarManifDest:
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

                case Servicos.ConsultaNFDest:
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
                        TFunctions.GravarArqErroServico(arquivo, extRet, extRetERR, ex, erroPadrao, true);

                        //new Task().GravarArqErroServico(arquivo, extRet, extRetERR, ex, erroPadrao, true);
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

        #region ConsultaGeral()
        protected void ConsultarGeral(string arquivo)
        {
            string arq = arquivo.ToLower().Trim();

            if (arq.EndsWith(Propriedade.ExtEnvio.ConsCertificado))
            {
                ConsultaCertificados(arquivo);
            }


        }

        protected void ConsultaCertificados(string arquivo)
        {
            ConfiguracaoApp oConfig = new ConfiguracaoApp();
            oConfig.CertificadosInstalados(arquivo);
        }

        #endregion
    }
}
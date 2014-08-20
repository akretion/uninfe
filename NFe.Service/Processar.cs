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
        public void ProcessaArquivo(int emp, string arquivo)
        {
            try
            {
                Servicos servico = Servicos.Nulo;
                try
                {
                    servico = DefinirTipoServico(emp, arquivo);

                    if (servico == Servicos.Nulo)
                        throw new Exception("Não pode identificar o tipo de serviço baseado no arquivo " + arquivo);

                    switch (servico)
                    {
                        #region NFS-e
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

                        case Servicos.ConsultarURLNfseSerie:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            this.DirecionarArquivo(arquivo, new NFSe.TaskConsultarURLNfseSerie());
                            break;
                    #endregion

                        #region NFe
                        case Servicos.PedidoConsultaSituacaoNFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskConsultaSituacaoNFe());
                            break;

                        case Servicos.ConsultaStatusServicoNFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskConsultaStatus());
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

                        case Servicos.EnviarEPEC:
                        case Servicos.RecepcaoEvento:
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

                        case Servicos.AssinarValidarNFeEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarNFe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.ConsultaCadastroContribuinte:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskCadastroContribuinte());
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
                        #endregion

                        #region MDFe
                        case Servicos.ConsultaStatusServicoMDFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskConsultaStatusMDFe());
                            break;

                        case Servicos.MontarLoteUmMDFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskMontarLoteUmMDFe());
                            break;

                        case Servicos.EnviarLoteMDFe:
                            DirecionarArquivo(arquivo, new TaskMDFeRecepcao());
                            break;

                        case Servicos.PedidoSituacaoLoteMDFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskRetRecepcaoMDFe());
                            break;

                        case Servicos.PedidoConsultaSituacaoMDFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskConsultaSituacaoMDFe());
                            break;

                        case Servicos.AssinarValidarMDFeEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarMDFe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.MontarLoteVariosMDFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskMontarLoteVariasMDFe());
                            break;

                        case Servicos.RecepcaoEventoMDFe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskEventosMDFe());
                            break;
                        #endregion

                        #region CTe
                        case Servicos.ConsultaStatusServicoCTe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskConsultaStatusCTe());
                            break;

                        case Servicos.MontarLoteUmCTe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskMontarLoteUmCTe());
                            break;

                        case Servicos.EnviarLoteCTe:
                            DirecionarArquivo(arquivo, new TaskCTeRecepcao());
                            break;

                        case Servicos.PedidoSituacaoLoteCTe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskRetRecepcaoCTe());
                            break;

                        case Servicos.PedidoConsultaSituacaoCTe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskConsultaSituacaoCTe());
                            break;

                        case Servicos.InutilizarNumerosCTe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskInutilizacaoCTe());
                            break;

                        case Servicos.RecepcaoEventoCTe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskEventosCTe());
                            break;

                        case Servicos.AssinarValidarCTeEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarCTe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.MontarLoteVariosCTe:
                            CertVencido(emp);
                            IsConnectedToInternet();
                            DirecionarArquivo(arquivo, new TaskMontarLoteVariasCTe());
                            break;
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

                        case Servicos.WSExiste:
                            DirecionarArquivo(arquivo, new TaskWSExiste());
                            break;

                        case Servicos.ImpressaoNFe:
                            DirecionarArquivo(arquivo, new TaskDanfe());
                            break;

                        case Servicos.DanfeRelatorio:
                            DirecionarArquivo(arquivo, new TaskDanfeReport());
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
                    if (servico == Servicos.Nulo || servico == Servicos.ConsultaStatusServicoNFe)
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

        private Servicos DefinirTipoServico(int empresa, string fullPath)
        {
            Servicos tipoServico = Servicos.Nulo;

            string arq = fullPath.ToLower().Trim();

            #region Serviços que funcionam tanto na pasta Geral como na pasta da Empresa
            if (arq.IndexOf(Propriedade.ExtEnvio.ConsCertificado) >= 0)
            {
                tipoServico = Servicos.ConsultaGeral;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.AltCon_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.AltCon_TXT) >= 0)
            {
                tipoServico = Servicos.AlterarConfiguracoesUniNFe;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_TXT) >= 0)
            {
                tipoServico = Servicos.WSExiste;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvImpressaoDanfe_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvImpressaoDanfe_TXT) >= 0)
            {
                tipoServico = Servicos.ImpressaoNFe;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDanfeReport_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvDanfeReport_TXT) >= 0)
            {
                tipoServico = Servicos.DanfeRelatorio;
            }
            #endregion
            else
            {
                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaValidar.ToLower()) >= 0)
                {
                    tipoServico = Servicos.AssinarValidar;
                }
                else
                {
                    FileInfo infArq = new FileInfo(arq);
                    string pastaArq = ConfiguracaoApp.RemoveEndSlash(infArq.DirectoryName).ToLower().Trim();
                    string pastaLote = ConfiguracaoApp.RemoveEndSlash(Empresas.Configuracoes[empresa].PastaXmlEmLote).ToLower().Trim();
                    string pastaEnvio = ConfiguracaoApp.RemoveEndSlash(Empresas.Configuracoes[empresa].PastaXmlEnvio).ToLower().Trim();
                    if (pastaArq.EndsWith("\\temp"))
                        pastaArq = Path.GetDirectoryName(pastaArq);

                    #region Arquivos com extensão txt (Somente NFe tem TXT)
                    if (fullPath.ToLower().EndsWith(".txt"))
                    {
                        if (arq.IndexOf(Propriedade.ExtEnvio.PedSit_TXT) >= 0)
                        {
                            tipoServico = Servicos.PedidoConsultaSituacaoNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedSta_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaStatusServicoNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsCad_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaCadastroContribuinte;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedInu_TXT) >= 0)
                        {
                            tipoServico = Servicos.InutilizarNumerosNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.Nfe_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConverterTXTparaXML;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.GerarChaveNFe_TXT) >= 0)
                        {
                            tipoServico = Servicos.GerarChaveNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_TXT) >= 0)
                        {
                            tipoServico = Servicos.WSExiste;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDPEC_TXT) >= 0)
                        {
                            tipoServico = Servicos.EnviarDPEC;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsDPEC_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultarDPEC;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.PedEPEC_TXT) >= 0)
                        {
                            tipoServico = Servicos.EnviarEPEC;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsInf_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaInformacoesUniNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCCe_TXT) >= 0)
                        {
                            tipoServico = Servicos.RecepcaoEvento;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCancelamento_TXT) >= 0)
                        {
                            tipoServico = Servicos.RecepcaoEvento;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvManifestacao_TXT) >= 0)
                        {
                            tipoServico = Servicos.RecepcaoEvento;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.ConsNFeDest_TXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaNFDest;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDownload_TXT) >= 0)
                        {
                            tipoServico = Servicos.DownloadNFe;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvRegistroDeSaida_TXT) >= 0)
                        {
                            tipoServico = Servicos.RegistroDeSaida;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCancRegistroDeSaida_TXT) >= 0)
                        {
                            tipoServico = Servicos.RegistroDeSaidaCancelamento;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.MontarLote_TXT) >= 0)
                        {
                            if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                            {
                                tipoServico = Servicos.MontarLoteVariasNFe;
                            }
                        }
                    }
                    #endregion
                    else
                    #region Arquivos com extensão XML
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(fullPath);

                        switch (doc.DocumentElement.Name)
                        {
                            #region MDFe
                            case "consStatServMDFe":
                                tipoServico = Servicos.ConsultaStatusServicoMDFe;
                                break;

                            case "MDFe":
                                if (pastaArq == pastaLote)
                                    tipoServico = Servicos.AssinarValidarMDFeEnvioEmLote;
                                else if (pastaArq == pastaEnvio)
                                    tipoServico = Servicos.MontarLoteUmMDFe;
                                break;

                            case "enviMDFe":
                                tipoServico = Servicos.EnviarLoteMDFe;
                                break;

                            case "consReciMDFe":
                                tipoServico = Servicos.PedidoSituacaoLoteMDFe;
                                break;

                            case "consSitMDFe":
                                tipoServico = Servicos.PedidoConsultaSituacaoMDFe;
                                break;

                            case "MontarLoteMDFe":
                                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                {
                                    tipoServico = Servicos.MontarLoteVariosMDFe;
                                }
                                break;

                            case "eventoMDFe":
                                tipoServico = Servicos.RecepcaoEventoMDFe;
                                break;
                            #endregion

                            #region CTe
                            case "consStatServCte":
                                tipoServico = Servicos.ConsultaStatusServicoCTe;
                                break;

                            case "CTe":
                                if (pastaArq == pastaLote)
                                    tipoServico = Servicos.AssinarValidarCTeEnvioEmLote;
                                else if (pastaArq == pastaEnvio)
                                    tipoServico = Servicos.MontarLoteUmCTe;
                                break;

                            case "enviCTe":
                                tipoServico = Servicos.EnviarLoteCTe;
                                break;

                            case "consReciCTe":
                                tipoServico = Servicos.PedidoSituacaoLoteCTe;
                                break;

                            case "consSitCTe":
                                tipoServico = Servicos.PedidoConsultaSituacaoCTe;
                                break;

                            case "inutCTe":
                                tipoServico = Servicos.InutilizarNumerosCTe;
                                break;

                            case "eventoCTe":
                                tipoServico = Servicos.RecepcaoEventoCTe;
                                break;

                            case "MontarLoteCTe":
                                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                {
                                    tipoServico = Servicos.MontarLoteVariosCTe;
                                }
                                break;
                            #endregion

                            #region NFe
                            case "consStatServ":
                                tipoServico = Servicos.ConsultaStatusServicoNFe;
                                break;

                            case "NFe":
                                if (pastaArq == pastaLote)
                                    tipoServico = Servicos.AssinarValidarNFeEnvioEmLote;
                                else if (pastaArq == pastaEnvio)
                                    tipoServico = Servicos.MontarLoteUmaNFe;
                                break;

                            case "enviNFe":
                                tipoServico = Servicos.EnviarLoteNfe;
                                break;

                            case "consReciNFe":
                                tipoServico = Servicos.PedidoSituacaoLoteNFe;
                                break;

                            case "consSitNFe":
                                tipoServico = Servicos.PedidoConsultaSituacaoNFe;
                                break;

                            case "inutNFe":
                                tipoServico = Servicos.InutilizarNumerosNFe;
                                break;

                            case "envEvento":
                                if (arq.IndexOf(Propriedade.ExtEnvio.PedEPEC) >= 0)
                                    tipoServico = Servicos.EnviarEPEC;
                                else
                                    tipoServico = Servicos.RecepcaoEvento;
                                break;

                            case "ConsCad":
                                tipoServico = Servicos.ConsultaCadastroContribuinte;
                                break;

                            case "MontarLoteNFe":
                                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                {
                                    tipoServico = Servicos.MontarLoteVariasNFe;
                                }
                                break;

                            case "envDPEC":
                                tipoServico = Servicos.EnviarDPEC;
                                break;

                            case "consDPEC":
                                tipoServico = Servicos.ConsultarDPEC;
                                break;

                            case "gerarChave":
                                tipoServico = Servicos.GerarChaveNFe;
                                break;

                            case "consNFeDest":
                                tipoServico = Servicos.ConsultaNFDest;
                                break;

                            case "downloadNFe":
                                tipoServico = Servicos.DownloadNFe;
                                break;
                            #endregion

                            #region Geral
                            case "ConsInf":
                                tipoServico = Servicos.ConsultaInformacoesUniNFe;
                                break;
                            #endregion

                            default:
                                #region NFS-e
                                if (arq.IndexOf(Propriedade.ExtEnvio.PedLoteRps) >= 0)
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
                                else if (arq.IndexOf(Propriedade.ExtEnvio.PedURLNfseSerie) >= 0)
                                {
                                    tipoServico = Servicos.ConsultarURLNfseSerie;
                                }
                                #endregion

                                break;
                        }
                    }
                    #endregion
                }
            }

            return tipoServico;
        }

        #region AssinarValidarNFe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarNFe(string arquivo, string pasta)
        {
            TaskAssinarValidarNFe nfe = new TaskAssinarValidarNFe();
            nfe.NomeArquivoXML = arquivo;
            nfe.AssinarValidarXMLNFe(pasta);
        }
        #endregion

        #region AssinarValidarCTe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarCTe(string arquivo, string pasta)
        {
            TaskAssinarValidarCTe nfe = new TaskAssinarValidarCTe();
            nfe.NomeArquivoXML = arquivo;
            nfe.AssinarValidarXMLNFe(pasta);
        }
        #endregion

        #region AssinarValidarMDFe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarMDFe(string arquivo, string pasta)
        {
            TaskAssinarValidarMDFe nfe = new TaskAssinarValidarMDFe();
            nfe.NomeArquivoXML = arquivo;
            nfe.AssinarValidarXMLNFe(pasta);
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
                    int emp = Empresas.FindEmpresaByThread();
                    ValidarXML validar = new ValidarXML(arquivo, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);
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
            bool hasAll = false;

            while (true)
            {
                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    if (Empresas.Configuracoes[i].Servico == TipoAplicativo.Nfse)
                        continue;

                    BackgroundWorker worker = new BackgroundWorker();

                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());
                    worker.DoWork += new DoWorkEventHandler(ExecutarEmProcessamento);
                    worker.RunWorkerAsync(i);

                    hasAll = true;
                }
                if (hasAll)
                    Thread.Sleep(720000); //Dorme por 12 minutos, para atender o problema do consumo indevido da SEFAZ
                else
                    break;
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
                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    Empresa empresa = Empresas.Configuracoes[i];

                    if (!string.IsNullOrEmpty(empresa.PastaEmpresa) && empresa.Servico != TipoAplicativo.Nfse)
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
            catch
            {
                //Não pode ser tratado nenhum erro aqui, visto que já estão sendo tratados e devidamente retornados
                //para o ERP no ponto da execução dos serviços. Foi muito bem testado e analisado. Wandrey 09/03/2010
                if (taskClass is TaskConsultaStatus)
                    throw;
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
            int emp = Empresas.FindEmpresaByThread();

            //Definir o tipo do serviço
            Type tipoServico = nfe.GetType();

            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivo });

            if (Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teFS &&
                Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teFSDA &&
                Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teOffLine &&
                Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teDPEC) //Confingência em formulário de segurança e DPEC não envia na hora, tem que aguardar voltar para normal.
            {
                tipoServico.InvokeMember(metodo, System.Reflection.BindingFlags.InvokeMethod, null, nfe, null);
            }
            else
            {
                if (nfe is TaskRetRecepcao ||
                    nfe is TaskConsultaStatus ||
                    nfe is TaskRecepcaoDPEC ||
                    nfe is TaskConsultaSituacaoNFe ||
                    nfe is TaskConsultaDPEC ||
                    nfe is TaskCadastroContribuinte)
                {
                    tipoServico.InvokeMember(metodo, System.Reflection.BindingFlags.InvokeMethod, null, nfe, null);
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
            int emp = Empresas.FindEmpresaByThread();
            string sArqRetorno = string.Empty;

            Auxiliar oAux = new Auxiliar();

            if (Path.GetExtension(ArquivoXml).ToLower() == ".txt")
                sArqRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                              Functions.ExtrairNomeArq(ArquivoXml, Propriedade.ExtEnvio.ConsInf_TXT) + "-ret-cons-inf.txt";
            else
                sArqRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
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
                if (tempoConsulta < Empresas.Configuracoes[empresa].TempoConsulta)
                    tempoConsulta = Empresas.Configuracoes[empresa].TempoConsulta;

                if (tempoConsulta < 2)
                    tempoConsulta = 2;

                if (DateTime.Now.Subtract(reciboCons.dPedRec).Seconds >= tempoConsulta)
                {
                    //Atualizar a tag da data e hora da ultima consulta do recibo aumentando 10 segundos
                    fluxoNfe.AtualizarDPedRec(reciboCons.nRec, DateTime.Now.AddSeconds(10));

                    switch (reciboCons.Servico)
                    {
                        case TipoAplicativo.Cte:
                            tipoServico.InvokeMember("XmlPedRecCTe", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { empresa, reciboCons.nRec, reciboCons.versao });
                            break;

                        case TipoAplicativo.Nfe:
                            tipoServico.InvokeMember("XmlPedRec", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { empresa, reciboCons.nRec, reciboCons.versao });
                            break;

                        case TipoAplicativo.MDFe:
                            tipoServico.InvokeMember("XmlPedRecMDFe", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { empresa, reciboCons.nRec, reciboCons.versao });
                            break;

                        default:
                            break;
                    }
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

                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    //Limpar conteúdo da pasta temp que fica dentro da pasta de envio de cada empresa a cada 10 dias
                    Limpar(Empresas.Configuracoes[i].PastaXmlEnvio + "\\temp", 10);
                    Limpar(Empresas.Configuracoes[i].PastaValidar + "\\temp", 10);   //danasa 12/8/2011
                    Limpar(Empresas.Configuracoes[i].PastaXmlEmLote + "\\temp", 10);   //Wandrey 05/10/2011

                    if (Empresas.Configuracoes[i].DiasLimpeza == 0)
                        continue;

                    #region temporario
                    Limpar(Empresas.Configuracoes[i].PastaXmlErro, Empresas.Configuracoes[i].DiasLimpeza);
                    #endregion

                    #region retorno
                    Limpar(Empresas.Configuracoes[i].PastaXmlRetorno, Empresas.Configuracoes[i].DiasLimpeza);
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
#if !DEBUG
            CertificadoDigital CertDig = new CertificadoDigital();
            if (CertDig.Vencido(emp))
            {
                throw new ExceptionCertificadoDigital(ErroPadrao.CertificadoVencido, "(" + CertDig.dValidadeInicial.ToString() + " a " + CertDig.dValidadeFinal.ToString() + ")");
            }
#endif
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
                #region NFe / CTe / MDFe
                case Servicos.InutilizarNumerosCTe:
                case Servicos.InutilizarNumerosNFe:
                    extRet = Propriedade.ExtEnvio.PedInu_XML;
                    extRetERR = Propriedade.ExtRetorno.Inu_ERR;
                    break;

                case Servicos.PedidoConsultaSituacaoCTe:
                case Servicos.PedidoConsultaSituacaoNFe:
                case Servicos.PedidoConsultaSituacaoMDFe:
                    extRet = Propriedade.ExtEnvio.PedSit_XML;
                    extRetERR = Propriedade.ExtRetorno.Sit_ERR;
                    break;

                case Servicos.ConsultaStatusServicoCTe:
                case Servicos.ConsultaStatusServicoNFe:
                case Servicos.ConsultaStatusServicoMDFe:
                    extRet = Propriedade.ExtEnvio.PedSta_XML;
                    extRetERR = Propriedade.ExtRetorno.Sta_ERR;
                    break;

                case Servicos.PedidoSituacaoLoteCTe:
                case Servicos.PedidoSituacaoLoteMDFe:
                case Servicos.PedidoSituacaoLoteNFe:
                    extRet = Propriedade.ExtEnvio.PedRec_XML;
                    extRetERR = Propriedade.ExtRetorno.ProRec_ERR;
                    break;

                case Servicos.ConsultaCadastroContribuinte:
                    extRet = Propriedade.ExtEnvio.ConsCad_XML;
                    extRetERR = Propriedade.ExtRetorno.ConsCad_ERR;
                    break;

                case Servicos.MontarLoteUmCTe:
                    extRet = Propriedade.ExtEnvio.Cte;
                    extRetERR = Propriedade.ExtRetorno.Cte_ERR;
                    break;

                case Servicos.MontarLoteUmaNFe:
                    extRet = Propriedade.ExtEnvio.Nfe;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    break;

                case Servicos.MontarLoteUmMDFe:
                    extRet = Propriedade.ExtEnvio.MDFe;
                    extRetERR = Propriedade.ExtRetorno.MDFe_ERR;
                    break;

                case Servicos.MontarLoteVariosCTe:
                case Servicos.MontarLoteVariasNFe:
                case Servicos.MontarLoteVariosMDFe:
                    extRet = Propriedade.ExtEnvio.MontarLote;
                    extRetERR = Propriedade.ExtRetorno.MontarLote_ERR;
                    break;

                case Servicos.EnviarLoteCTe:
                case Servicos.EnviarLoteNfe:
                case Servicos.EnviarLoteNfe2:
                case Servicos.EnviarLoteMDFe:
                    extRet = Propriedade.ExtEnvio.EnvLot;
                    extRetERR = Propriedade.ExtRetorno.Rec_ERR;
                    break;

                case Servicos.EnviarDPEC:
                    extRet = Propriedade.ExtEnvio.EnvDPEC_XML;
                    extRetERR = Propriedade.ExtRetorno.retDPEC_ERR;
                    break;

                case Servicos.ConsultarDPEC:
                    extRet = Propriedade.ExtEnvio.ConsDPEC_XML;
                    extRetERR = Propriedade.ExtRetorno.retConsDPEC_ERR;
                    break;

                case Servicos.AssinarValidarCTeEnvioEmLote:
                    extRet = Propriedade.ExtEnvio.Cte;
                    extRetERR = Propriedade.ExtRetorno.Cte_ERR;
                    break;

                case Servicos.AssinarValidarMDFeEnvioEmLote:
                    extRet = Propriedade.ExtEnvio.MDFe;
                    extRetERR = Propriedade.ExtRetorno.MDFe_ERR;
                    break;

                case Servicos.AssinarValidarNFeEnvioEmLote:
                    extRet = Propriedade.ExtEnvio.Nfe;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    break;

                case Servicos.RecepcaoEvento:
                case Servicos.RecepcaoEventoCTe:
                case Servicos.RecepcaoEventoMDFe:
                case Servicos.EnviarEPEC:
                    extRet = Propriedade.ExtEnvio.PedEve;
                    extRetERR = Propriedade.ExtRetorno.Eve_ERR;
                    break;

                case Servicos.EnviarCCe:
                    extRet = Propriedade.ExtEnvio.EnvCCe_XML;
                    extRetERR = Propriedade.ExtRetorno.retEnvCCe_ERR;
                    break;

                case Servicos.EnviarManifDest:
                    extRet = Propriedade.ExtEnvio.EnvManifestacao_XML;
                    extRetERR = Propriedade.ExtRetorno.retManifestacao_ERR;
                    break;

                case Servicos.EnviarEventoCancelamento:
                    extRet = Propriedade.ExtEnvio.EnvCancelamento_XML;
                    extRetERR = Propriedade.ExtRetorno.retCancelamento_ERR;
                    break;

                case Servicos.DownloadNFe:
                    extRet = Propriedade.ExtEnvio.EnvDownload_XML;
                    extRetERR = Propriedade.ExtRetorno.retDownload_ERR;
                    break;

                case Servicos.ConsultaNFDest:
                    extRet = Propriedade.ExtEnvio.ConsNFeDest_XML;
                    extRetERR = Propriedade.ExtRetorno.retConsNFeDest_ERR;
                    break;

                #endregion

                #region NFSe
                case Servicos.RecepcionarLoteRps:
                    extRet = Propriedade.ExtEnvio.EnvLoteRps;
                    extRetERR = Propriedade.ExtRetorno.RetLoteRps_ERR;
                    break;

                case Servicos.ConsultarSituacaoLoteRps:
                    extRet = Propriedade.ExtEnvio.PedSitLoteRps;
                    extRetERR = Propriedade.ExtRetorno.SitLoteRps_ERR;
                    break;

                case Servicos.ConsultarNfsePorRps:
                    extRet = Propriedade.ExtEnvio.PedSitNfseRps;
                    extRetERR = Propriedade.ExtRetorno.SitNfseRps_ERR;
                    break;

                case Servicos.ConsultarNfse:
                    extRet = Propriedade.ExtEnvio.PedSitNfse;
                    extRetERR = Propriedade.ExtRetorno.SitNfse_ERR;
                    break;

                case Servicos.ConsultarLoteRps:
                    extRet = Propriedade.ExtEnvio.PedLoteRps;
                    extRetERR = Propriedade.ExtRetorno.LoteRps_ERR;
                    break;

                case Servicos.CancelarNfse:
                    extRet = Propriedade.ExtEnvio.PedCanNfse;
                    extRetERR = Propriedade.ExtRetorno.CanNfse_ERR;
                    break;

                #endregion

                #region Diversos
                case Servicos.AlterarConfiguracoesUniNFe:
                case Servicos.AssinarValidar:
                case Servicos.ConsultaInformacoesUniNFe:
                case Servicos.ConverterTXTparaXML:
                case Servicos.EmProcessamento:
                case Servicos.GerarChaveNFe:
                case Servicos.LimpezaTemporario:
                    //Não tem definição pois não gera arquivo .ERR
                    break;
                #endregion

                default:
                    //Como não foi possível identificar o tipo do servico vou mudar somente a extensão para .err pois isso pode acontecer caso exista erro na estrutura do XML.
                    //Renan - 05/03/2014 
                    extRet = ".xml";
                    extRetERR = ".err";
                    break;
            }
            if (!string.IsNullOrEmpty(extRet))
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(arquivo, extRet, extRetERR, ex, erroPadrao, true);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 02/06/2011
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
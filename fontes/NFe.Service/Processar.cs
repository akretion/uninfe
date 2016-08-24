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
                    ValidarExtensao(arquivo);

                    servico = DefinirTipoServico(emp, arquivo);

                    if (servico == Servicos.Nulo)
                        throw new Exception("Não pode identificar o tipo de serviço baseado no arquivo " + arquivo);

                    switch (servico)
                    {
                        #region NFS-e
                        case Servicos.NFSeCancelar:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeCancelar());
                            break;

                        case Servicos.NFSeConsultar:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultar());
                            break;

                        case Servicos.NFSeConsultarLoteRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarLoteRps());
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarPorRps());
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultaSituacaoLoteRps());
                            break;

                        case Servicos.NFSeConsultarURL:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarURL());
                            break;

                        case Servicos.NFSeConsultarURLSerie:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarURLSerie());
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeRecepcionarLoteRps());
                            break;

                        case Servicos.NFSeConsultarNFSePNG:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskConsultarNfsePNG());
                            break;

                        case Servicos.NFSeInutilizarNFSe:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskInutilizarNfse());
                            break;

                        case Servicos.NFSeConsultarNFSePDF:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskConsultarNfsePDF());
                            break;

                        case Servicos.NFSeObterNotaFiscal:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskObterNotaFiscal());
                            break;

                        #endregion

                        #region NFe
                        case Servicos.NFeAssinarValidarEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarNFe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.ConsultaCadastroContribuinte:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCadastroContribuinte());
                            break;

                        case Servicos.EventoRecepcao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeEventos());
                            break;

                        case Servicos.NFeConsultaNFDest:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeConsultaNFDest());
                            break;

                        case Servicos.NFeConsultaStatusServico:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeConsultaStatus());
                            break;

                        case Servicos.NFeConverterTXTparaXML:
                            ConverterTXTparaXML(arquivo);
                            break;

                        case Servicos.NFeDownload:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeDownload());
                            break;

                        case Servicos.NFeEnviarLote:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskNFeRecepcao());
                            break;

                        case Servicos.NFeGerarChave:
                            GerarChaveNFe(arquivo);
                            break;

                        case Servicos.NFeInutilizarNumeros:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeInutilizacao());
                            break;

                        case Servicos.NFePedidoSituacaoLote:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeRetRecepcao());
                            break;

                        case Servicos.NFeMontarLoteUma:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeMontarLoteUmaNFe());
                            break;

                        case Servicos.NFeMontarLoteVarias:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeMontarLoteVarias());
                            break;

                        case Servicos.NFePedidoConsultaSituacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeConsultaSituacao());
                            break;

#if nao
                        case Servicos.RegistroDeSaida:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskRegistroDeSaida());
                            break;

                        case Servicos.RegistroDeSaidaCancelamento:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskRegistroDeSaidaCancelamento());
                            break;
#endif
                        #endregion

                        #region MDFe
                        case Servicos.MDFeAssinarValidarEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarMDFe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.MDFeConsultaNaoEncerrado:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeConsNaoEncerrado());
                            break;

                        case Servicos.MDFeConsultaStatusServico:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeConsultaStatus());
                            break;

                        case Servicos.MDFeEnviarLote:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskMDFeRecepcao());
                            break;

                        case Servicos.MDFeMontarLoteUm:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeMontarLoteUm());
                            break;

                        case Servicos.MDFeMontarLoteVarios:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeMontarLoteVarias());
                            break;

                        case Servicos.MDFePedidoSituacaoLote:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeRetRecepcao());
                            break;

                        case Servicos.MDFePedidoConsultaSituacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeConsultaSituacao());
                            break;

                        case Servicos.MDFeRecepcaoEvento:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeEventos());
                            break;
                        #endregion

                        #region CTe
                        case Servicos.CTeAssinarValidarEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarCTe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.CTeConsultaStatusServico:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeConsultaStatus());
                            break;

                        case Servicos.CTeEnviarLote:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskCTeRecepcao());
                            break;

                        case Servicos.CTeInutilizarNumeros:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeInutilizacao());
                            break;

                        case Servicos.CTeMontarLoteUm:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeMontarLoteUm());
                            break;

                        case Servicos.CTeMontarLoteVarios:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeMontarLoteVarias());
                            break;

                        case Servicos.CTePedidoConsultaSituacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeConsultaSituacao());
                            break;

                        case Servicos.CTePedidoSituacaoLote:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeRetRecepcao());
                            break;

                        case Servicos.CTeRecepcaoEvento:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeEventos());
                            break;
                        #endregion

                        #region DFe
                        case Servicos.DFeEnviar:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskDFeRecepcao());
                            break;
                        #endregion

                        #region LMC
                        case Servicos.LMCAutorizacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskLMCAutorizacao());
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

                        case Servicos.UniNFeAlterarConfiguracoes:
                            AlterarConfiguracoesUniNFe(arquivo);
                            break;

                        case Servicos.UniNFeConsultaGeral:
                            ConsultarGeral(arquivo);
                            break;

                        case Servicos.UniNFeConsultaInformacoes:
                            ConsultaInformacoesUniNFe(arquivo);
                            break;

                        case Servicos.WSExiste:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskWSExiste());
                            break;

                        case Servicos.DANFEImpressao:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskDanfe());
                            break;

                        case Servicos.DANFEImpressao_Contingencia:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskDanfeContingencia());
                            break;

                        case Servicos.DANFERelatorio:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskDanfeReport());
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
                    if (servico == Servicos.Nulo || servico == Servicos.NFeConsultaStatusServico)
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

        private void ValidarExtensao(string arquivo)
        {
            var extOk = false;
            string extensoes = "";
            foreach (Propriedade.TipoEnvio item in Enum.GetValues(typeof(Propriedade.TipoEnvio)))
            {
                var EXT = Propriedade.Extensao(item);

                if (extensoes != "") extensoes += ", ";
                extensoes += EXT.EnvioXML;

                if (arquivo.EndsWith(EXT.EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                {
                    extOk = true;
                    break;
                }
                if (!string.IsNullOrEmpty(EXT.EnvioTXT))
                {
                    extensoes += (EXT.descricao != "" ? " ou " : ", ") + EXT.EnvioTXT;
                    if (arquivo.EndsWith(EXT.EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        extOk = true;
                        break;
                    }
                }
                if (EXT.descricao != "")
                    extensoes += ": \"" + EXT.descricao + "\"\r\n";
            }
            if (!extOk)
            {
                throw new Exception("Não pode identificar o tipo de arquivo baseado no arquivo '" + arquivo + "'\r\nExtensões permitidas: " + extensoes);
            }
        }
        #endregion

        private Servicos DefinirTipoServico(int empresa, string fullPath)
        {
            Servicos tipoServico = Servicos.Nulo;

            string arq = fullPath.ToLower().Trim();

            #region Serviços que funcionam tanto na pasta Geral como na pasta da Empresa
            if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.ConsCertificado).EnvioXML) >= 0)
            {
                tipoServico = Servicos.UniNFeConsultaGeral;
            }
            else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.AltCon).EnvioXML) >= 0 ||
                     arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.AltCon).EnvioTXT) >= 0)
            {
                tipoServico = Servicos.UniNFeAlterarConfiguracoes;
            }
            else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioXML) >= 0 ||
                     arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioTXT) >= 0)
            {
                tipoServico = Servicos.WSExiste;
            }
            else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).EnvioXML) >= 0 ||
                     arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).EnvioTXT) >= 0)
            {
                tipoServico = Servicos.DANFEImpressao;
            }
            else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioXML) >= 0 ||
                     arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioTXT) >= 0)
            {
                tipoServico = Servicos.DANFERelatorio;
            }
            #endregion
            else
            {
                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaContingencia.ToLower()) >= 0)
                {
                    tipoServico = Servicos.DANFEImpressao_Contingencia;
                }
                else
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
                        if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.NFePedidoConsultaSituacao;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.NFeConsultaStatusServico;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.ConsultaCadastroContribuinte;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.NFeInutilizarNumeros;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.NFeConverterTXTparaXML;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.GerarChaveNFe).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.NFeGerarChave;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.WSExiste;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.ConsInf).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.UniNFeConsultaInformacoes;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioTXT) >= 0 ||
                                arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioTXT) >= 0 ||
                                arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioTXT) >= 0 ||
                                arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.EventoRecepcao;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.ConsNFeDest).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.NFeConsultaNFDest;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.NFeDownload;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioTXT) >= 0)
                        {
                            tipoServico = Servicos.DFeEnviar;
                        }
                        else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.MontarLote).EnvioTXT) >= 0)
                        {
                            if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                            {
                                tipoServico = Servicos.NFeMontarLoteVarias;
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
                            #region DFe
                            case "distDFeInt":
                                tipoServico = Servicos.DFeEnviar;
                                break;
                            #endregion

                            #region MDFe
                            case "consMDFeNaoEnc":
                                tipoServico = Servicos.MDFeConsultaNaoEncerrado;
                                break;

                            case "consStatServMDFe":
                                tipoServico = Servicos.MDFeConsultaStatusServico;
                                break;

                            case "MDFe":
                                if (pastaArq == pastaLote)
                                    tipoServico = Servicos.MDFeAssinarValidarEnvioEmLote;
                                else if (pastaArq == pastaEnvio)
                                    tipoServico = Servicos.MDFeMontarLoteUm;
                                break;

                            case "enviMDFe":
                                tipoServico = Servicos.MDFeEnviarLote;
                                break;

                            case "consReciMDFe":
                                tipoServico = Servicos.MDFePedidoSituacaoLote;
                                break;

                            case "consSitMDFe":
                                tipoServico = Servicos.MDFePedidoConsultaSituacao;
                                break;

                            case "MontarLoteMDFe":
                                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                {
                                    tipoServico = Servicos.MDFeMontarLoteVarios;
                                }
                                break;

                            case "eventoMDFe":
                                tipoServico = Servicos.MDFeRecepcaoEvento;
                                break;
                            #endregion

                            #region CTe
                            case "consStatServCte":
                                tipoServico = Servicos.CTeConsultaStatusServico;
                                break;

                            case "CTe":
                                if (pastaArq == pastaLote)
                                    tipoServico = Servicos.CTeAssinarValidarEnvioEmLote;
                                else if (pastaArq == pastaEnvio)
                                    tipoServico = Servicos.CTeMontarLoteUm;
                                break;

                            case "enviCTe":
                                tipoServico = Servicos.CTeEnviarLote;
                                break;

                            case "consReciCTe":
                                tipoServico = Servicos.CTePedidoSituacaoLote;
                                break;

                            case "consSitCTe":
                                tipoServico = Servicos.CTePedidoConsultaSituacao;
                                break;

                            case "inutCTe":
                                tipoServico = Servicos.CTeInutilizarNumeros;
                                break;

                            case "eventoCTe":
                                tipoServico = Servicos.CTeRecepcaoEvento;
                                break;

                            case "MontarLoteCTe":
                                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                {
                                    tipoServico = Servicos.CTeMontarLoteVarios;
                                }
                                break;
                            #endregion

                            #region NFe
                            case "consStatServ":
                                tipoServico = Servicos.NFeConsultaStatusServico;
                                break;

                            case "NFe":
                                if (pastaArq == pastaLote)
                                    tipoServico = Servicos.NFeAssinarValidarEnvioEmLote;
                                else if (pastaArq == pastaEnvio)
                                    tipoServico = Servicos.NFeMontarLoteUma;
                                break;

                            case "enviNFe":
                                tipoServico = Servicos.NFeEnviarLote;
                                break;

                            case "consReciNFe":
                                tipoServico = Servicos.NFePedidoSituacaoLote;
                                break;

                            case "consSitNFe":
                                tipoServico = Servicos.NFePedidoConsultaSituacao;
                                break;

                            case "inutNFe":
                                tipoServico = Servicos.NFeInutilizarNumeros;
                                break;

                            case "envEvento":
                                tipoServico = Servicos.EventoRecepcao;
                                break;

                            case "ConsCad":
                                tipoServico = Servicos.ConsultaCadastroContribuinte;
                                break;

                            case "MontarLoteNFe":
                                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                {
                                    tipoServico = Servicos.NFeMontarLoteVarias;
                                }
                                break;

                            case "gerarChave":
                                tipoServico = Servicos.NFeGerarChave;
                                break;

                            case "consNFeDest":
                                tipoServico = Servicos.NFeConsultaNFDest;
                                break;

                            case "downloadNFe":
                                tipoServico = Servicos.NFeDownload;
                                break;
                            #endregion

                            #region LMC
                            case "autorizacao":
                                tipoServico = Servicos.LMCAutorizacao;
                                break;
                            #endregion

                            #region Geral
                            case "ConsInf":
                                tipoServico = Servicos.UniNFeConsultaInformacoes;
                                break;
                            #endregion

                            default:
                                #region NFS-e
                                if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultarLoteRps;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeCancelar;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultarSituacaoLoteRps;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeRecepcionarLoteRps;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultar;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultarPorRps;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedURLNFSe).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultarURL;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedURLNFSeSerie).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultarURLSerie;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePNG).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultarNFSePNG;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedInuNFSe).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeInutilizarNFSe;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeConsultarNFSePDF;
                                }
                                else if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).EnvioXML) >= 0)
                                {
                                    tipoServico = Servicos.NFSeObterNotaFiscal;
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
            TaskNFeAssinarValidar nfe = new TaskNFeAssinarValidar();
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
            TaskCTeAssinarValidar nfe = new TaskCTeAssinarValidar();
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
            TaskMDFeAssinarValidar nfe = new TaskMDFeAssinarValidar();
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
                int emp = Empresas.FindEmpresaByThread();

                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaValidado, Path.GetFileName(Path.ChangeExtension(arquivo, ".xml"))));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlErro, Path.GetFileName(Path.ChangeExtension(arquivo, ".xml"))));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlErro, Path.GetFileName(arquivo)));

                if (arquivo.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region DFe
                        var temp = Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioTXT);
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, temp + Propriedade.ExtRetorno.retEnvDFe_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, temp + Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioXML));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskDFeRecepcao());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region Consulta ao cadastro de contribuinte
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioTXT) + Propriedade.ExtRetorno.ConsCad_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioTXT) + Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).RetornoXML));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskCadastroContribuinte());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        new ConverterTXT(arquivo);
                    }

                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioTXT, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioTXT, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioTXT, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region Eventos
                        if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                            Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioTXT) + Propriedade.ExtRetorno.retEnvCCe_ERR));

                        if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                            Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioTXT) + Propriedade.ExtRetorno.retCancelamento_ERR));

                        if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                            Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioTXT) + Propriedade.ExtRetorno.retManifestacao_ERR));

                        if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                            Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioTXT) + Propriedade.ExtRetorno.Eve_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeEventos());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region Inutilizacao
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioTXT) + Propriedade.ExtRetorno.Inu_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioTXT) + Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).RetornoXML));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeInutilizacao());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioTXT) + Propriedade.ExtRetorno.Sta_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeConsultaStatus());
                    }

                    if (arquivo.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.ConsNFeDest).EnvioTXT) >= 0)
                    {
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.ConsNFeDest).EnvioTXT) + Propriedade.ExtRetorno.retConsNFeDest_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeConsultaNFDest());
                    }

                    if (arquivo.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioTXT) >= 0)
                    {
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioTXT) + Propriedade.ExtRetorno.Sit_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeConsultaSituacao());
                    }
                }
                else
                {
                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DirecionarArquivo(arquivo);
                    }

                    ValidarXML validar = new ValidarXML(arquivo, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, true);
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

                    NFeEmProcessamento nfe = new NFeEmProcessamento();
                    nfe.Analisar(i);

                    hasAll = true;
                }
                if (hasAll)
                    Thread.Sleep(720000); //Dorme por 12 minutos, para atender o problema do consumo indevido da SEFAZ
                else
                    break;
            }
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
        /// Direcionar os arquivos encontrados na pasta de envio corretamente
        /// </summary>
        /// <param name="arquivos">Lista de arquivos</param>
        /// <param name="metodo">Método a ser executado do serviço da NFe</param>
        /// <param name="nfe">Objeto do serviço da NFe a ser executado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 18/04/2011
        /// </remarks>
        /*private void DirecionarArquivo(List<string> arquivos, object nfe, string metodo)
        {
            for (int i = 0; i < arquivos.Count; i++)
            {
                DirecionarArquivo(arquivos[i], nfe, metodo);
            }
        }*/
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
        private void DirecionarArquivo(int emp, bool veCertificado, bool veConexao, string arquivo, object taskClass)
        {
            try
            {
                if (veCertificado)
                    CertVencido(emp);
                if (veConexao)
                    IsConnectedToInternet();

                //Processa ou envia o XML
                EnviarArquivo(emp, arquivo, taskClass, "Execute");
            }
            catch (ExceptionCertificadoDigital ex)
            {
                throw ex;
            }
            catch (ExceptionSemInternet ex)
            {
                throw ex;
            }
            catch
            {
                //Não pode ser tratado nenhum erro aqui, visto que já estão sendo tratados e devidamente retornados
                //para o ERP no ponto da execução dos serviços. Foi muito bem testado e analisado. Wandrey 09/03/2010
            }
        }

        private void DirecionarArquivo(string arquivo)
        {
            //Processa ou envia o XML
            var valclass = new TaskValidar();
            //Definir o tipo do serviço
            Type tipoServico = valclass.GetType();
            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, valclass, new object[] { arquivo });
            tipoServico.InvokeMember("Execute", System.Reflection.BindingFlags.InvokeMethod, null, valclass, null);
        }
        #endregion

        #region EnviarArquivo()
        /// <summary>
        /// Analisa o tipo do XML que está na pasta de envio e executa a operação necessária. Exemplo: Envia ao SEFAZ, reconfigura o UniNFE, etc... 
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML a ser enviado ou analisado</param>
        /// <param name="oNfe">Objeto da classe UniNfeClass a ser utilizado nas operações</param>
        private void EnviarArquivo(int emp, string arquivo, Object nfe, string metodo)
        {
            //int emp = Empresas.FindEmpresaByThread();

            //Definir o tipo do serviço
            Type tipoServico = nfe.GetType();

            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivo });

            bool doExecute = arquivo.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0;
            if (!doExecute)
            {
                if (Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teFS &&
                    Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teFSDA &&
                    Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teOffLine &&
                    Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teEPEC) //Confingência em formulário de segurança e EPEC não envia na hora, tem que aguardar voltar para normal.
                {
                    doExecute = true;
                }
                else
                {
                    if (nfe is TaskDFeRecepcao ||
                        nfe is TaskNFeRetRecepcao ||
                        nfe is TaskNFeConsultaStatus ||
                        nfe is TaskNFeConsultaSituacao ||
                        nfe is TaskDanfeContingencia ||
                        nfe is TaskDanfe ||
                        nfe is TaskCadastroContribuinte ||
                        nfe is TaskCTeRetRecepcao ||
                        nfe is TaskCTeConsultaStatus ||
                        nfe is TaskCTeConsultaSituacao ||
                        nfe is TaskMDFeRetRecepcao ||
                        nfe is TaskMDFeConsultaStatus ||
                        nfe is TaskMDFeConsultaSituacao ||
                        nfe is TaskMDFeConsNaoEncerrado ||
                        nfe is TaskLMCAutorizacao ||
                        (nfe is TaskNFeEventos && Empresas.Configuracoes[emp].tpEmis == (int)NFe.Components.TipoEmissao.teEPEC) ||
                        (nfe is TaskCTeEventos && Empresas.Configuracoes[emp].tpEmis == (int)NFe.Components.TipoEmissao.teEPEC))
                    {
                        doExecute = true;
                    }
                }
            }
            if (doExecute)
                tipoServico.InvokeMember(metodo, System.Reflection.BindingFlags.InvokeMethod, null, nfe, null);
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
                              Functions.ExtrairNomeArq(ArquivoXml, Propriedade.Extensao(Propriedade.TipoEnvio.ConsInf).EnvioTXT) +
                              Propriedade.Extensao(Propriedade.TipoEnvio.ConsInf).RetornoTXT;
            else
                sArqRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                              Functions.ExtrairNomeArq(ArquivoXml, Propriedade.Extensao(Propriedade.TipoEnvio.ConsInf).EnvioXML) +
                              Propriedade.Extensao(Propriedade.TipoEnvio.ConsInf).RetornoXML;

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

                if (tempoConsulta > 15)
                    tempoConsulta = 15; //Tempo previsto no manual da SEFAZ, isso foi feito pq o ambiente SVAN está retornando na consulta recibo, tempo superior a 160, mas não está com problema, é erro no calculo deste tempo. Wandrey

                if (tempoConsulta < Empresas.Configuracoes[empresa].TempoConsulta)
                    tempoConsulta = Empresas.Configuracoes[empresa].TempoConsulta;

                //Vou dar no mínimo 3 segundos para efetuar a consulta do recibo. Wandrey 21/11/2014
                if (tempoConsulta < 3)
                    tempoConsulta = 3;

                if (DateTime.Now.Subtract(reciboCons.dPedRec).Seconds >= tempoConsulta)
                {
                    //Atualizar a tag da data e hora da ultima consulta do recibo aumentando 10 segundos
                    fluxoNfe.AtualizarDPedRec(reciboCons.nRec, DateTime.Now.AddSeconds(10));
                    tipoServico.InvokeMember("XmlPedRec", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { empresa, reciboCons.nRec, reciboCons.versao, reciboCons.mod });
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
                Limpar(-1, Propriedade.PastaLog, "", 60);

                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    //Limpar conteúdo da pasta temp que fica dentro da pasta de envio de cada empresa a cada 10 dias
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlEnvio, "temp", 10);
                    Limpar(i, Empresas.Configuracoes[i].PastaValidar, "temp", 10);   //danasa 12/8/2011
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlEmLote, "temp", 10);   //Wandrey 05/10/2011

                    if (Empresas.Configuracoes[i].DiasLimpeza == 0)
                        continue;

                    #region temporario
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlErro, "", Empresas.Configuracoes[i].DiasLimpeza);
                    #endregion

                    #region retorno
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlRetorno, "", Empresas.Configuracoes[i].DiasLimpeza);
                    #endregion
                }
            }
        }

        private void Limpar(int empresa, string diretorio, string subdir, int diasLimpeza)
        {
            // danasa 27-2-2011
            if (diasLimpeza == 0 || string.IsNullOrEmpty(diretorio)) return;

            if (!Directory.Exists(diretorio)) return;   //danasa 12/8/2011

            if (!string.IsNullOrEmpty(subdir))
                diretorio += "\\" + subdir;

            if (!Directory.Exists(diretorio)) return;   //danasa 12/8/2011

            try
            {
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
            catch (Exception ex)
            {
                if (empresa >= 0 && Empresas.Configuracoes.Count > 0)
                    Functions.WriteLog(Empresas.Configuracoes[empresa].Nome + "\r\n" + ex.Message, false, true, Empresas.Configuracoes[empresa].CNPJ);
                else
                    Functions.WriteLog("Geral:\r\n" + ex.Message, false, true, "");
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
            //#if !DEBUG
            CertificadoDigital CertDig = new CertificadoDigital();
            if (CertDig.Vencido(emp))
            {
                throw new ExceptionCertificadoDigital(ErroPadrao.CertificadoVencido, "(" + CertDig.dValidadeInicial.ToString() + " a " + CertDig.dValidadeFinal.ToString() + ")");
            }
            //#endif
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
                case Servicos.CTeInutilizarNumeros:
                case Servicos.NFeInutilizarNumeros:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Inu_ERR;
                    break;

                case Servicos.CTePedidoConsultaSituacao:
                case Servicos.NFePedidoConsultaSituacao:
                case Servicos.MDFePedidoConsultaSituacao:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Sit_ERR;
                    break;

                case Servicos.CTeConsultaStatusServico:
                case Servicos.NFeConsultaStatusServico:
                case Servicos.MDFeConsultaStatusServico:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Sta_ERR;
                    break;

                case Servicos.CTePedidoSituacaoLote:
                case Servicos.MDFePedidoSituacaoLote:
                case Servicos.NFePedidoSituacaoLote:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.ProRec_ERR;
                    break;

                case Servicos.ConsultaCadastroContribuinte:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.ConsCad_ERR;
                    break;

                case Servicos.CTeMontarLoteUm:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Cte_ERR;
                    break;

                case Servicos.NFeMontarLoteUma:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    break;

                case Servicos.MDFeMontarLoteUm:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.MDFe_ERR;
                    break;

                case Servicos.CTeMontarLoteVarios:
                case Servicos.NFeMontarLoteVarias:
                case Servicos.MDFeMontarLoteVarios:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.MontarLote).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.MontarLote_ERR;
                    break;

                case Servicos.CTeEnviarLote:
                case Servicos.NFeEnviarLote:
                case Servicos.NFeEnviarLote2:
                case Servicos.MDFeEnviarLote:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Rec_ERR;
                    break;

                case Servicos.CTeAssinarValidarEnvioEmLote:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Cte_ERR;
                    break;

                case Servicos.MDFeAssinarValidarEnvioEmLote:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.MDFe_ERR;
                    break;

                case Servicos.NFeAssinarValidarEnvioEmLote:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    break;

                case Servicos.EventoRecepcao:
                case Servicos.CTeRecepcaoEvento:
                case Servicos.MDFeRecepcaoEvento:
                case Servicos.EventoEPEC:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.Eve_ERR;
                    break;

                case Servicos.EventoCCe:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.retEnvCCe_ERR;
                    break;

                case Servicos.EventoManifestacaoDest:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.retManifestacao_ERR;
                    break;

                case Servicos.EventoCancelamento:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.retCancelamento_ERR;
                    break;

                case Servicos.NFeDownload:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.EnvDownload).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.retDownload_ERR;
                    break;

                case Servicos.NFeConsultaNFDest:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.ConsNFeDest).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.retConsNFeDest_ERR;
                    break;

                #endregion

                #region NFSe
                case Servicos.NFSeRecepcionarLoteRps:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.RetEnvLoteRps_ERR;
                    break;

                case Servicos.NFSeConsultarSituacaoLoteRps:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.SitLoteRps_ERR;
                    break;

                case Servicos.NFSeConsultarPorRps:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.SitNfseRps_ERR;
                    break;

                case Servicos.NFSeConsultar:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.SitNfse_ERR;
                    break;

                case Servicos.NFSeConsultarLoteRps:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.LoteRps_ERR;
                    break;

                case Servicos.NFSeCancelar:
                    extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML;
                    extRetERR = Propriedade.ExtRetorno.CanNfse_ERR;
                    break;

                #endregion

                #region Diversos
                case Servicos.UniNFeAlterarConfiguracoes:
                case Servicos.AssinarValidar:
                case Servicos.UniNFeConsultaInformacoes:
                case Servicos.NFeConverterTXTparaXML:
                case Servicos.EmProcessamento:
                case Servicos.NFeGerarChave:
                case Servicos.UniNFeLimpezaTemporario:
                    //Não tem definição pois não gera arquivo .ERR
                    break;
                #endregion

                default:
                    if (arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML))
                    {
                        extRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML;
                        extRetERR = Propriedade.ExtRetorno.Sit_ERR;
                    }
                    else
                    {
                        //Como não foi possível identificar o tipo do servico vou mudar somente a extensão para .err pois isso pode acontecer caso exista erro na estrutura do XML.
                        //Renan - 05/03/2014 
                        extRet = ".xml";
                        extRetERR = ".err";
                    }
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

            if (arq.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.ConsCertificado).EnvioXML) >= 0)
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
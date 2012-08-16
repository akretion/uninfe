using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
//using UniNFeLibrary;
//using UniNFeLibrary.Enums;
using System.Xml;

#if x
namespace UniNFeServico
{
    #region Classe ServicoUniNFe
    /// <summary>
    /// Classe responsável pela execução dos serviços do UniNFe
    /// </summary>
    public class ServicoUniNFe : AServicoApp
    {
        #region ProcessaArquivo()
        public override void ProcessaArquivo(object paramThread)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            ParametroThread param = (ParametroThread)paramThread;
            Servicos servico = param.Servico;

            try
            {
                string arquivo = param.Arquivo;

                #region Executar o serviço
                switch (servico)
                {
                    case Servicos.PedidoConsultaSituacaoNFe:
                        Empresa.Configuracoes[emp].PoolPedidoConsultaSituacaoNFe.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        PedidoConsultaSituacaoNFe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.PedidoConsultaStatusServicoNFe:
                        Empresa.Configuracoes[emp].PoolDiversos.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        PedidoConsultaStatusServicoNFe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.ConsultaCadastroContribuinte:
                        Empresa.Configuracoes[emp].PoolDiversos.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        ConsultaCadastroContribuinte(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.CancelarNFe:
                        Empresa.Configuracoes[emp].PoolDiversos.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        CancelarNFe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.InutilizarNumerosNFe:
                        Empresa.Configuracoes[emp].PoolDiversos.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        InutilizarNumerosNFe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.PedidoSituacaoLoteNFe:
                        Empresa.Configuracoes[emp].PoolPedidoSituacaoLoteNFe.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        PedidoSituacaoLoteNFe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.MontarLoteUmaNFe:
                        CertVencido(emp);
                        IsConnectedToInternet();

                        MontarLoteUmaNFe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.EnviarLoteNfe:
                        Empresa.Configuracoes[emp].PoolEnviarLoteNfe.WaitOne();
                        EnviarLoteNfe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.GerarChaveNFe:
                        GerarChaveNFe(arquivo);
                        break;

                    case Servicos.EnviarDPEC:
                        Empresa.Configuracoes[emp].PoolEnviarDPEC.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        EnviarDPEC(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.ConsultarDPEC:
                        Empresa.Configuracoes[emp].PoolDiversos.WaitOne();

                        CertVencido(emp);
                        IsConnectedToInternet();

                        ConsultarDPEC(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.AssinarValidar:
                        CertVencido(emp);

                        AssinarValidar(arquivo);
                        break;

                    case Servicos.ConverterTXTparaXML:
                        ConverterTXTparaXML(arquivo);
                        break;

                    case Servicos.ConsultaInformacoesUniNFe:
                        ConsultaInformacoesUniNFe(arquivo);
                        break;

                    case Servicos.AlterarConfiguracoesUniNFe:
                        AlterarConfiguracoesUniNFe(arquivo);
                        break;

                    case Servicos.AssinarValidarNFe:
                        CertVencido(emp);

                        AssinarValidarNFe(new ServicoNFe(), arquivo, true);
                        break;

                    case Servicos.MontarLoteVariasNFe:
                        CertVencido(emp);
                        IsConnectedToInternet();

                        MontarLoteVariasNFe(new ServicoNFe(), arquivo);
                        break;

                    case Servicos.EnviarCCe:
                        Empresa.Configuracoes[emp].PoolCCe.WaitOne();
                        CertVencido(emp);
                        IsConnectedToInternet();

                        //EnviarEvento(new ServicoNFe(), arquivo);
                        break;

                    default:
                        break;
                }
                #endregion
            }
            catch (ExceptionSemInternet ex)
            {
                GravaErroERP(param.Arquivo, servico, ex, ex.ErrorCode);               
            }
            catch (ExceptionCerticicadoDigital ex)
            {
                GravaErroERP(param.Arquivo, servico, ex, ex.ErrorCode);
            }
            catch { }
            finally
            {
                #region Atualizar Pool de Threads
                switch (servico)
                {
                    case Servicos.EnviarLoteNfe:
                        Empresa.Configuracoes[emp].PoolEnviarLoteNfe.Release();
                        break;

                    case Servicos.PedidoConsultaSituacaoNFe:
                        Empresa.Configuracoes[emp].PoolPedidoConsultaSituacaoNFe.Release();
                        break;

                    case Servicos.PedidoConsultaStatusServicoNFe:
                        Empresa.Configuracoes[emp].PoolDiversos.Release();
                        break;

                    case Servicos.CancelarNFe:
                        Empresa.Configuracoes[emp].PoolDiversos.Release();
                        break;

                    case Servicos.ConsultaCadastroContribuinte:
                        Empresa.Configuracoes[emp].PoolDiversos.Release();
                        break;

                    case Servicos.InutilizarNumerosNFe:
                        Empresa.Configuracoes[emp].PoolDiversos.Release();
                        break;

                    case Servicos.PedidoSituacaoLoteNFe:
                        Empresa.Configuracoes[emp].PoolPedidoSituacaoLoteNFe.Release();
                        break;

                    case Servicos.EnviarDPEC:
                        Empresa.Configuracoes[emp].PoolEnviarDPEC.Release();
                        break;

                    case Servicos.ConsultarDPEC:
                        Empresa.Configuracoes[emp].PoolDiversos.Release();
                        break;

                    case Servicos.EnviarCCe:
                        Empresa.Configuracoes[emp].PoolCCe.Release();
                        break;

                    default:
                        break;
                }
                #endregion

                Thread.CurrentThread.Interrupt();
            }
        }
        #endregion
    }
    #endregion
}
#endif
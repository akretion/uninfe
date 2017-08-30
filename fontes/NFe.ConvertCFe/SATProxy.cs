using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.SAT.Servico.Envio;
using NFe.Settings;
using System;
using Unimake.SAT.Abstract.Servico;
using Unimake.SAT.Servico.Retorno;

namespace NFe.SAT
{
    /// <summary>
    /// Classe responsável pelo consumo dos serviços SAT
    /// </summary>
    public class SATProxy
    {
        #region Propriedades

        /// <summary>
        /// Dados da empresa que esta sendo realizado o envio
        /// </summary>
        private Empresa DadosEmpresa;

        /// <summary>
        /// Serviço que esta sendo executado
        /// </summary>
        private Servicos Servicos;

        /// <summary>
        /// Arquivo XML
        /// </summary>
        private string Arquivo;

        /// <summary>
        /// Objeto de envio do SAT
        /// </summary>
        private ServicoBase _Dispatch = null;

        /// <summary>
        /// Define o objeto de envio
        /// </summary>
        protected ServicoBase Dispatch
        {
            get
            {
                if (_Dispatch == null)
                {
                    switch (Servicos)
                    {
                        case Servicos.SATTrocarCodigoDeAtivacao:
                            _Dispatch = new TrocarCodigoDeAtivacao(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATTesteFimAFim:
                            _Dispatch = new TesteFimAFim(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATConsultarStatusOperacional:
                            _Dispatch = new ConsultarStatusOperacional(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATExtrairLogs:
                            _Dispatch = new ExtrairLogs(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATConsultar:
                            _Dispatch = new ConsultarSAT(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATEnviarDadosVenda:
                            _Dispatch = new EnviarDadosVenda(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATConverterNFCe:
                            _Dispatch = new ConverterSAT(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATCancelarUltimaVenda:
                            _Dispatch = new CancelarUltimaVenda(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATConfigurarInterfaceDeRede:
                            _Dispatch = new ConfigurarInterfaceDeRede(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATAssociarAssinatura:
                            _Dispatch = new AssociarAssinatura(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATAtivar:
                            _Dispatch = new AtivarSAT(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATBloquear:
                            _Dispatch = new BloquearSAT(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATDesbloquear:
                            _Dispatch = new DesbloquearSAT(Arquivo, DadosEmpresa);
                            break;

                        case Servicos.SATConsultarNumeroSessao:
                            _Dispatch = new ConsultarNumeroSessao(Arquivo, DadosEmpresa);
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
                return _Dispatch;
            }
        }

        /// <summary>
        /// Objeto de resposta do SAT
        /// </summary>
        private ResponseBase _Response = null;

        /// <summary>
        /// Define o objeto de resposta
        /// </summary>
        public ResponseBase Response
        {
            get
            {
                if (_Response == null)
                {
                    switch (Servicos)
                    {
                        case Servicos.SATTrocarCodigoDeAtivacao:
                            _Response = new TrocarCodigoDeAtivacaoResponse();
                            break;

                        case Servicos.SATTesteFimAFim:
                            _Response = new TesteFimAFimResponse();
                            break;

                        case Servicos.SATConsultarStatusOperacional:
                            _Response = new ConsultarStatusOperacionalResponse();
                            break;

                        case Servicos.SATExtrairLogs:
                            _Response = new ExtrairLogsResponse();
                            break;

                        case Servicos.SATConsultar:
                            _Response = new ConsultarSATResponse();
                            break;

                        case Servicos.SATEnviarDadosVenda:
                            _Response = new EnviarDadosVendaResponse();
                            break;

                        case Servicos.SATCancelarUltimaVenda:
                            _Response = new CancelarUltimaVendaResponse();
                            break;

                        case Servicos.SATConfigurarInterfaceDeRede:
                            _Response = new ConfigurarInterfaceDeRedeResponse();
                            break;

                        case Servicos.SATAssociarAssinatura:
                            _Response = new AssociarAssinaturaResponse();
                            break;

                        case Servicos.SATAtivar:
                            _Response = new AtivarSATResponse();
                            break;

                        case Servicos.SATBloquear:
                            _Response = new BloquearSATResponse();
                            break;

                        case Servicos.SATDesbloquear:
                            _Response = new DesbloquearSATResponse();
                            break;

                        case Servicos.SATConsultarNumeroSessao:
                            _Response = new ConsultarNumeroSessaoResponse();
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
                return _Response;
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="servicos">serviço que esta sendo usado</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        /// <param name="arquivo">arquivo XML</param>
        public SATProxy(Servicos servicos, Empresa dadosEmpresa, string arquivo)
        {
            Servicos = servicos;
            DadosEmpresa = dadosEmpresa;
            Arquivo = arquivo;
        }

        #endregion Construtores

        /// <summary>
        /// Método de troca de código de ativação
        /// </summary>
        /// <returns></returns>
        public void Enviar()
        {
            try
            {
                Dispatch.Enviar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para salvar resposta do SAT
        /// </summary>
        public string SaveResponse()
        {
            try
            {
                return Dispatch.SaveResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
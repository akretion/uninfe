using NFe.Components;
using NFe.Settings;
using System;

namespace NFe.Service
{
    /// <summary>
    /// Classe para consultar status do serviço do CTe
    /// </summary>
    public class TaskCTeConsultaStatus : TaskAbst
    {
        public TaskCTeConsultaStatus(string arquivo)
        {
            Servico = Servicos.CTeConsultaStatusServico;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        #region Classe com os dados do XML da consulta do status do serviço da NFe

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do status do serviço
        /// </summary>
        private DadosPedSta dadosPedSta;

        #endregion Classe com os dados do XML da consulta do status do serviço da NFe

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedSta = new DadosPedSta();
                //Ler o XML para pegar parâmetros de envio
                PedSta(emp, dadosPedSta);

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedSta.cUF, dadosPedSta.tpAmb, dadosPedSta.tpEmis, 0);
                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedSta.cUF, dadosPedSta.tpAmb, dadosPedSta.tpEmis, Servico);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                var oStatusServico = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                var oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedSta.cUF, Servico, dadosPedSta.tpEmis));

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), dadosPedSta.cUF.ToString());
                wsProxy.SetProp(oCabecMsg, TpcnResources.versaoDados.ToString(), dadosPedSta.versao);

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy, oStatusServico, wsProxy.NomeMetodoWS[0], oCabecMsg, this,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).RetornoXML,
                                    true,
                                    securityProtocolType);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML,
                                    Propriedade.ExtRetorno.Sta_ERR, ex);
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
                    //Deletar o arquivo de solicitação do serviço
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de solicitação do serviço,
                    //infelizmente não posso fazer mais nada, o UniNFe vai tentar mandar
                    //o arquivo novamente para o webservice
                    //Wandrey 09/03/2010
                }
            }
        }

        #endregion Execute
    }
}
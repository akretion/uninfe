using NFe.Components;
using NFe.Settings;
using System;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.MDFe;

namespace NFe.Service
{
    /// <summary>
    /// Classe para consultar status do serviço do MDFe
    /// </summary>
    public class TaskMDFeConsultaStatus: TaskAbst
    {
        public TaskMDFeConsultaStatus(string arquivo)
        {
            Servico = Servicos.MDFeConsultaStatusServico;
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

        /// <summary>
        /// Executa o serviço solicitado
        /// </summary>
        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedSta = new DadosPedSta();
                PedSta(emp, dadosPedSta);

                var xml = new ConsStatServMDFe();
                xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<ConsStatServMDFe>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.MDFe,
                    CodigoUF = dadosPedSta.cUF,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                if(ConfiguracaoApp.Proxy)
                {
                    configuracao.HasProxy = true;
                    configuracao.ProxyAutoDetect = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
                    configuracao.ProxyUser = ConfiguracaoApp.ProxyUsuario;
                    configuracao.ProxyPassword = ConfiguracaoApp.ProxySenha;
                }

                var statusServico = new Unimake.Business.DFe.Servicos.MDFe.StatusServico(xml, configuracao);
                statusServico.Executar();

                vStrXmlRetorno = statusServico.RetornoWSString;
                XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).RetornoXML);
            }
            catch(Exception ex)
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
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.NFe;

namespace NFe.Service
{
    public class TaskNFeConsultaStatus: TaskAbst
    {
        public TaskNFeConsultaStatus(string arquivo)
        {
            Servico = Servicos.NFeConsultaStatusServico;
            NomeArquivoXML = arquivo;
            if(vXmlNfeDadosMsgEhXML)
            {
                ConteudoXML.PreserveWhitespace = false;
                ConteudoXML.Load(arquivo);
            }
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
            var emp = Empresas.FindEmpresaByThread();
            try
            {
                dadosPedSta = new DadosPedSta();
                PedSta(emp, dadosPedSta);

                if(vXmlNfeDadosMsgEhXML)
                {
                    var xml = new ConsStatServ();
                    xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<ConsStatServ>(ConteudoXML);

                    var configuracao = new Configuracao
                    {
                        TipoDFe = (dadosPedSta.mod == "65" ? TipoDFe.NFCe : TipoDFe.NFe),
                        TipoEmissao = (Unimake.Business.DFe.Servicos.TipoEmissao)dadosPedSta.tpEmis,
                        CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                    };

                    if (ConfiguracaoApp.Proxy)
                    {
                        configuracao.HasProxy = true;
                        configuracao.ProxyAutoDetect = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
                        configuracao.ProxyUser = ConfiguracaoApp.ProxyUsuario;
                        configuracao.ProxyPassword = ConfiguracaoApp.ProxySenha;                            
                    }

                    if(dadosPedSta.mod == "65")
                    {
                        var statusServico = new Unimake.Business.DFe.Servicos.NFCe.StatusServico(xml, configuracao);
                        statusServico.Executar();

                        vStrXmlRetorno = statusServico.RetornoWSString;
                        XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).RetornoXML);
                    }
                    else
                    {
                        var statusServico = new Unimake.Business.DFe.Servicos.NFe.StatusServico(xml, configuracao);
                        statusServico.Executar();

                        vStrXmlRetorno = statusServico.RetornoWSString;
                        XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).RetornoXML);
                    }
                }
                else
                {
                    var f = Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml";

                    if(NomeArquivoXML.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        f = Path.Combine(Empresas.Configuracoes[emp].PastaValidar, f);
                    }
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.StatusServicoNFe(f, dadosPedSta.tpAmb, dadosPedSta.tpEmis, dadosPedSta.cUF, dadosPedSta.versao);
                }
            }
            catch(Exception ex)
            {
                var extRet = vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML :
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioTXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, extRet, Propriedade.ExtRetorno.Sta_ERR, ex);
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

        #region PedSta()

        /// <summary>
        /// Faz a leitura do XML de pedido do status de serviço
        /// </summary>
        /// <param name="cArquivoXml">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        ///
        protected override void PedSta(int emp, DadosPedSta dadosPedSta)
        {
            base.PedSta(emp, dadosPedSta);

            if(string.IsNullOrEmpty(dadosPedSta.versao))
            {
                throw new Exception(NFeStrConstants.versaoError);
            }
        }

        #endregion PedSta()
    }
}
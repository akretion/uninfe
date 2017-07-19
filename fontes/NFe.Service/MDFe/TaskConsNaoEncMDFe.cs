using NFe.Components;
using NFe.Settings;
using System;
using System.Xml;

namespace NFe.Service
{
    public class TaskMDFeConsNaoEncerrado : TaskAbst
    {
        public TaskMDFeConsNaoEncerrado(string arquivo)
        {
            Servico = Servicos.MDFeConsultaNaoEncerrado;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                Int32 tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
                Int32 cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
                string versao = string.Empty;

                foreach (XmlNode consSitNFeNode in ConteudoXML.GetElementsByTagName("consMDFeNaoEnc"))
                {
                    XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;
                    versao = consSitNFeElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                    tpAmb = Convert.ToInt32("0" + Functions.LerTag(consSitNFeElemento, TpcnResources.tpAmb.ToString(), false));
                }

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, cUF, tpAmb, 0);
                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(cUF, tpAmb, 1, Servico);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                var oServico = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                var cabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(cUF, Servico));

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(cabecMsg, TpcnResources.cUF.ToString(), cUF.ToString());
                wsProxy.SetProp(cabecMsg, TpcnResources.versaoDados.ToString(), versao);

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy,
                    oServico,
                    wsProxy.NomeMetodoWS[0],
                    cabecMsg,
                    this,
                    Propriedade.Extensao(Propriedade.TipoEnvio.MDFeConsNaoEncerrados).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.MDFeConsNaoEncerrados).RetornoXML,
                    true,
                    securityProtocolType);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                            Propriedade.Extensao(Propriedade.TipoEnvio.MDFeConsNaoEncerrados).EnvioXML,
                            Propriedade.ExtRetorno.MDFeConsNaoEnc_ERR, ex);
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
    }
}
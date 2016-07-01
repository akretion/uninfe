using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    /// <summary>
    /// Classe para consultar status do serviço do CTe
    /// </summary>
    public class TaskCTeConsultaStatus : TaskAbst
    {
        #region Classe com os dados do XML da consulta do status do serviço da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do status do serviço
        /// </summary>
        private DadosPedSta dadosPedSta;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.CTeConsultaStatusServico;

            try
            {
                dadosPedSta = new DadosPedSta();
                //Ler o XML para pegar parâmetros de envio
                PedSta(emp, dadosPedSta);//NomeArquivoXML);

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedSta.cUF, dadosPedSta.tpAmb, dadosPedSta.tpEmis);
                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedSta.cUF, dadosPedSta.tpAmb, dadosPedSta.tpEmis, PadroesNFSe.NaoIdentificado, Servico);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                var oStatusServico = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                var oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedSta.cUF, Servico, dadosPedSta.tpEmis));

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), dadosPedSta.cUF.ToString());
                wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLCTeStatusServico);

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
        #endregion

        #region PedSta()
        /// <summary>
        /// Faz a leitura do XML de pedido do status de serviço
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        /// <param name="arquivoXML">Arquivo xml a ser lido</param>
        /// 
#if f
        private void PedSta(int emp, string arquivoXML)
        {
            dadosPedSta.tpAmb = 0;
            dadosPedSta.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            dadosPedSta.tpEmis = Empresas.Configuracoes[emp].tpEmis;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList consStatServList = null;

            consStatServList = doc.GetElementsByTagName("consStatServCte");

            foreach (XmlNode consStatServNode in consStatServList)
            {
                bool regravar = false;

                XmlElement consStatServElemento = (XmlElement)consStatServNode;

                this.dadosPedSta.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);

                if (consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString()).Count != 0)
                {
                    this.dadosPedSta.cUF = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0].InnerText);

                    // para que o validador não rejeite, excluo a tag <cUF>
                    doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0]);

                    regravar = true;
                }

                if (consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString()).Count != 0)
                {
                    this.dadosPedSta.tpEmis = Convert.ToInt16(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0].InnerText);

                    // para que o validador não rejeite, excluo a tag <tpEmis>
                    doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0]);

                    regravar = true;
                }

                // Salvar o arquivo modificado
                if (regravar)
                    doc.Save(arquivoXML);
            }
        }
#endif
        #endregion
    }
}

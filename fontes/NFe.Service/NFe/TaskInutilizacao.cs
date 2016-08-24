using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;
using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    /// <summary>
    /// Classe para envio de XMLs de inutilização do NFe
    /// </summary>
    public class TaskNFeInutilizacao : TaskAbst
    {
        public TaskNFeInutilizacao()
        {
            Servico = Servicos.NFeInutilizarNumeros;
        }

        #region Classe com os dados do XML do pedido de inutilização de números de NF
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de inutilizacao
        /// </summary>
        private DadosPedInut dadosPedInut;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedInut = new DadosPedInut(emp);
                PedInut(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedInut.cUF, dadosPedInut.tpAmb, dadosPedInut.tpEmis, dadosPedInut.versao, dadosPedInut.mod.ToString(), 0);
                    System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedInut.cUF, dadosPedInut.tpAmb, dadosPedInut.tpEmis, PadroesNFSe.NaoIdentificado, Servico);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oInutilizacao = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedInut.cUF, Servico));

                    //Atribuir conteúdo p ara duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.cUF.ToString(), dadosPedInut.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), dadosPedInut.versao);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(dadosPedInut.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy,
                                        oInutilizacao,
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg, this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).RetornoXML,
                                        true,
                                        securityProtocolType);

                    //Ler o retorno do webservice
                    LerRetornoInut();
                }
                else
                {
                    string f = Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml";

                    if (NomeArquivoXML.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        f = Path.Combine(Empresas.Configuracoes[emp].PastaValidar, f);
                    }
                    oGerarXML.Inutilizacao(f,
                        dadosPedInut.tpAmb,
                        dadosPedInut.tpEmis,
                        dadosPedInut.cUF,
                        dadosPedInut.ano,
                        dadosPedInut.CNPJ,
                        dadosPedInut.mod,
                        dadosPedInut.serie,
                        dadosPedInut.nNFIni,
                        dadosPedInut.nNFFin,
                        dadosPedInut.xJust,
                        dadosPedInut.versao);
                }
            }
            catch (Exception ex)
            {
                string ExtRet = string.Empty;

                if (vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML;
                else //Se for TXT
                    ExtRet = Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioTXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.Inu_ERR, ex);
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
                    if (!vXmlNfeDadosMsgEhXML) //Se for o TXT para ser transformado em XML, vamos excluir o TXT depois de gerado o XML
                        Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de inutilização, infelizmente não posso 
                    //fazer mais nada. Com certeza o uninfe sendo restabelecido novamente vai tentar enviar o mesmo 
                    //xml de inutilização para o SEFAZ. Este erro pode ocorrer por falha no HD, rede, Permissão de pastas, etc. Wandrey 23/03/2010
                }
            }
        }
        #endregion

        #region PedInut()
        /// <summary>
        /// PedInut(string cArquivoXML)
        /// </summary>
        /// <param name="cArquivoXML"></param>
        private void PedInut(int emp, string cArquivoXML)
        {
            dadosPedInut.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            dadosPedInut.tpEmis = Empresas.Configuracoes[emp].tpEmis;
            dadosPedInut.versao = "";

            if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
            {
                //      tpAmb|2
                //      tpEmis|1                <<< opcional >>>
                //      cUF|35
                //      ano|08
                //      CNPJ|99999090910270
                //      mod|55
                //      serie|0
                //      nNFIni|1
                //      nNFFin|1
                //      xJust|Teste do WS de Inutilizacao
                //      versao|3.10
                List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
                Functions.PopulateClasse(dadosPedInut, cLinhas);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList InutNFeList = doc.GetElementsByTagName("inutNFe");

                foreach (XmlNode InutNFeNode in InutNFeList)
                {
                    XmlElement InutNFeElemento = (XmlElement)InutNFeNode;
                    dadosPedInut.versao = InutNFeElemento.Attributes[NFe.Components.TpcnResources.versao.ToString()].InnerText;

                    XmlNodeList infInutList = InutNFeElemento.GetElementsByTagName("infInut");

                    foreach (XmlNode infInutNode in infInutList)
                    {
                        XmlElement infInutElemento = (XmlElement)infInutNode;
                        Functions.PopulateClasse(dadosPedInut, infInutElemento);

                        if (infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString()).Count != 0)
                        {
                            dadosPedInut.tpEmis = Convert.ToInt16(infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0].InnerText);
                            /// para que o validador não rejeite, excluo a tag <tpEmis>
                            doc.DocumentElement["infInut"].RemoveChild(infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0]);
                            /// salvo o arquivo modificado
                            doc.Save(cArquivoXML);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(dadosPedInut.versao))
                throw new Exception("Inutilização: Versão deve ser informada");
        }
        #endregion

        #region LerRetornoInut()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento da Inutilização
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void LerRetornoInut()
        {
            int emp = Empresas.FindEmpresaByThread();

            XmlDocument doc = new XmlDocument();

            MemoryStream msXml = Functions.StringXmlToStream(vStrXmlRetorno);
            doc.Load(msXml);

            XmlNodeList retInutNFeList = doc.GetElementsByTagName("retInutNFe");

            foreach (XmlNode retInutNFeNode in retInutNFeList)
            {
                XmlElement retInutNFeElemento = (XmlElement)retInutNFeNode;

                XmlNodeList infInutList = retInutNFeElemento.GetElementsByTagName("infInut");

                foreach (XmlNode infInutNode in infInutList)
                {
                    XmlElement infInutElemento = (XmlElement)infInutNode;

                    if (infInutElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText == "102") //Inutilização de Número Homologado
                    {
                        string strRetInutNFe = retInutNFeNode.OuterXml;

                        oGerarXML.XmlDistInut(NomeArquivoXML, strRetInutNFe);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, DateTime.Now);

                        //Move o arquivo de Distribuição para a pasta de enviados autorizados
                        string strNomeArqProcInutNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML) + Propriedade.ExtRetorno.ProcInutNFe;
                        TFunctions.MoverArquivo(strNomeArqProcInutNFe, PastaEnviados.Autorizados, DateTime.Now);
                    }
                    else
                    {
                        //Deletar o arquivo de solicitação do serviço da pasta de envio
                        Functions.DeletarArquivo(NomeArquivoXML);
                    }
                }
            }
        }
        #endregion
    }
}

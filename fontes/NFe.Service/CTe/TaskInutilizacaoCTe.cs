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
    /// Classe para envio de XMLs de inutilização do CTe
    /// </summary>
    public class TaskCTeInutilizacao : TaskAbst
    {
        public TaskCTeInutilizacao()
        {
            Servico = Servicos.CTeInutilizarNumeros;
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

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedInut.cUF, dadosPedInut.tpAmb, dadosPedInut.tpEmis);
                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedInut.cUF, dadosPedInut.tpAmb, dadosPedInut.tpEmis, PadroesNFSe.NaoIdentificado);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                object oInutilizacao = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);//NomeClasseWS(Servico, dadosPedInut.cUF));
                object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedInut.cUF, Servico));

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.cUF.ToString(), dadosPedInut.cUF.ToString());
                wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLCTeInut);

                //Criar objeto da classe de assinatura digita
                AssinaturaDigital oAD = new AssinaturaDigital();

                //Assinar o XML
                oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(dadosPedInut.cUF));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy,
                                    oInutilizacao,
                                    wsProxy.NomeMetodoWS[0],//NomeMetodoWS(Servico, dadosPedInut.cUF), 
                                    oCabecMsg,
                                    this,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).RetornoXML,
                                    true,
                                    securityProtocolType);

                //Ler o retorno do webservice
                this.LerRetornoInut();
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML, 
                                                    Propriedade.ExtRetorno.Inu_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 09/03/2010
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
            this.dadosPedInut.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            this.dadosPedInut.tpEmis = Empresas.Configuracoes[emp].tpEmis;

            XmlDocument doc = new XmlDocument();
            doc.Load(cArquivoXML);

            XmlNodeList InutNFeList = doc.GetElementsByTagName("inutCTe");

            foreach (XmlNode InutNFeNode in InutNFeList)
            {
                XmlElement InutNFeElemento = (XmlElement)InutNFeNode;

                XmlNodeList infInutList = InutNFeElemento.GetElementsByTagName("infInut");

                foreach (XmlNode infInutNode in infInutList)
                {
                    XmlElement infInutElemento = (XmlElement)infInutNode;
                    Functions.PopulateClasse(dadosPedInut, infInutElemento);
#if false
                    if (infInutElemento.GetElementsByTagName("tpAmb")[0] != null)
                        this.dadosPedInut.tpAmb = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                    if (infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.cUF.ToString())[0] != null)
                        this.dadosPedInut.cUF = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.cUF.ToString())[0].InnerText);

                    if (infInutElemento.GetElementsByTagName("ano")[0] != null)
                        this.dadosPedInut.ano = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("ano")[0].InnerText);

                    if (infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.CNPJ.ToString())[0] != null)
                        this.dadosPedInut.CNPJ = infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.CNPJ.ToString())[0].InnerText;

                    if (infInutElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0] != null)
                        this.dadosPedInut.mod = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0].InnerText);

                    if (infInutElemento.GetElementsByTagName(TpcnResources.serie.ToString())[0] != null)
                        this.dadosPedInut.serie = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName(TpcnResources.serie.ToString())[0].InnerText);

                    if (infInutElemento.GetElementsByTagName("nCTIni")[0] != null)
                        this.dadosPedInut.nNFIni = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nCTIni")[0].InnerText);

                    if (infInutElemento.GetElementsByTagName("nCTFin")[0] != null)
                        this.dadosPedInut.nNFFin = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nCTFin")[0].InnerText);
#endif
                    if (infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString()).Count != 0)
                    {
                        this.dadosPedInut.tpEmis = Convert.ToInt16(infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement["infInut"].RemoveChild(infInutElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0]);
                        /// salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
                }
            }
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

            /*
             this.vStrXmlRetorno = "<retInutCTe versao=\"2.00\" xmlns=\"http://www.portalfiscal.inf.br/cte\"><infInut><tpAmb>2</tpAmb><verAplic>SP-CTe-05-12-2013</verAplic><cStat>102</cStat><xMotivo>Inutilização de número homologado</xMotivo><cUF>35</cUF><ano>13</ano><CNPJ>11319532000102</CNPJ><mod>57</mod><serie>1</serie><nCTIni>2017</nCTIni><nCTFin>2017</nCTFin><dhRecbto>2013-12-13T17:43:06</dhRecbto><nProt>135130006325186</nProt></infInut></retInutCTe>";
            */

            MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
            doc.Load(msXml);

            XmlNodeList retInutNFeList = doc.GetElementsByTagName("retInutCTe");

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

                        oGerarXML.XmlDistInutCTe(NomeArquivoXML, strRetInutNFe);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, DateTime.Now);

                        //Move o arquivo de Distribuição para a pasta de enviados autorizados
                        string strNomeArqProcInutNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML) + 
                                                        Propriedade.ExtRetorno.ProcInutCTe;
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
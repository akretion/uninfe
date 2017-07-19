using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    /// <summary>
    /// Classe para envio de XMLs de inutilização do CTe
    /// </summary>
    public class TaskCTeInutilizacao : TaskAbst
    {
        public TaskCTeInutilizacao(string arquivo)
        {
            Servico = Servicos.CTeInutilizarNumeros;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        #region Classe com os dados do XML do pedido de inutilização de números de NF

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de inutilizacao
        /// </summary>
        private DadosPedInut dadosPedInut;

        #endregion Classe com os dados do XML do pedido de inutilização de números de NF

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedInut = new DadosPedInut(emp);
                PedInut(emp);

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedInut.cUF, dadosPedInut.tpAmb, dadosPedInut.tpEmis, 0);
                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedInut.cUF, dadosPedInut.tpAmb, dadosPedInut.tpEmis, Servico);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                object oInutilizacao = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);//NomeClasseWS(Servico, dadosPedInut.cUF));
                object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedInut.cUF, Servico));

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), dadosPedInut.cUF.ToString());
                wsProxy.SetProp(oCabecMsg, TpcnResources.versaoDados.ToString(), dadosPedInut.versao);

                //Criar objeto da classe de assinatura digita
                AssinaturaDigital oAD = new AssinaturaDigital();

                //Assinar o XML
                oAD.Assinar(ConteudoXML, emp, Convert.ToInt32(dadosPedInut.cUF));

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

        #endregion Execute

        #region PedInut()

        /// <summary>
        /// PedInut(string cArquivoXML)
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        private void PedInut(int emp)
        {
            dadosPedInut.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            dadosPedInut.tpEmis = Empresas.Configuracoes[emp].tpEmis;

            XmlNodeList InutNFeList = ConteudoXML.GetElementsByTagName("inutCTe");

            foreach (XmlNode InutNFeNode in InutNFeList)
            {
                XmlElement InutNFeElemento = (XmlElement)InutNFeNode;

                XmlNodeList infInutList = InutNFeElemento.GetElementsByTagName("infInut");
                dadosPedInut.versao = InutNFeElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                foreach (XmlNode infInutNode in infInutList)
                {
                    XmlElement infInutElemento = (XmlElement)infInutNode;
                    Functions.PopulateClasse(dadosPedInut, infInutElemento);

                    if (infInutElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                    {
                        this.dadosPedInut.tpEmis = Convert.ToInt16(infInutElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        ConteudoXML.DocumentElement["infInut"].RemoveChild(infInutElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0]);
                        /// salvo o arquivo modificado
                        ConteudoXML.Save(NomeArquivoXML);
                    }
                }
            }
        }

        #endregion PedInut()

        #region LerRetornoInut()

        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento da Inutilização
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void LerRetornoInut()
        {
            int emp = Empresas.FindEmpresaByThread();

            //            vStrXmlRetorno = "<retInutCTe versao=\"2.00\" xmlns=\"http://www.portalfiscal.inf.br/cte\"><infInut><tpAmb>2</tpAmb><verAplic>SP-CTe-05-12-2013</verAplic><cStat>102</cStat><xMotivo>Inutilização de número homologado</xMotivo><cUF>35</cUF><ano>13</ano><CNPJ>11319532000102</CNPJ><mod>57</mod><serie>1</serie><nCTIni>2017</nCTIni><nCTFin>2017</nCTFin><dhRecbto>2013-12-13T17:43:06</dhRecbto><nProt>135130006325186</nProt></infInut></retInutCTe>";

            XmlDocument doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStream(vStrXmlRetorno));

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

                        dadosPedInut = new DadosPedInut(emp);
                        PedInut(emp);

                        oGerarXML.XmlDistInutCTe(ConteudoXML, strRetInutNFe, NomeArquivoXML, dadosPedInut.versao);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        StreamWriter sw = File.CreateText(NomeArquivoXML);
                        sw.Write(ConteudoXML.OuterXml);
                        sw.Close();
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

        #endregion LerRetornoInut()
    }
}
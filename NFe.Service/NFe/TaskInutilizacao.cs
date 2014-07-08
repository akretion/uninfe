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
    public class TaskInutilizacao : TaskAbst
    {
        public TaskInutilizacao()
        {
            Servico = Servicos.InutilizarNumerosNFe;
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
            int emp = Functions.FindEmpresaByThread();

            try
            {
                dadosPedInut = new DadosPedInut(emp);
                PedInut(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedInut.cUF, dadosPedInut.tpAmb, dadosPedInut.tpEmis, dadosPedInut.versao);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oInutilizacao = wsProxy.CriarObjeto(NomeClasseWS(Servico, dadosPedInut.cUF));
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedInut.cUF, Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", dadosPedInut.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", dadosPedInut.versao);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(dadosPedInut.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oInutilizacao, NomeMetodoWS(Servico, dadosPedInut.cUF), oCabecMsg, this, "-ped-inu", "-inu");

                    //Ler o retorno do webservice
                    LerRetornoInut();
                }
                else
                {
                    oGerarXML.Inutilizacao(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
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
                    ExtRet = Propriedade.ExtEnvio.PedInu_XML;
                else //Se for TXT
                    ExtRet = Propriedade.ExtEnvio.PedInu_TXT;

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
            //int emp = Functions.FindEmpresaByThread();

            dadosPedInut.tpAmb = Empresa.Configuracoes[emp].AmbienteCodigo;
            dadosPedInut.tpEmis = Empresa.Configuracoes[emp].tpEmis;
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
#if false
                foreach (string cTexto in cLinhas)
                {
                    string[] dados = cTexto.Split('|');
                    switch (dados[0].ToLower())
                    {
                        case "tpamb":
                            dadosPedInut.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "tpemis":
                            dadosPedInut.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "cuf":
                            dadosPedInut.cUF = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "ano":
                            dadosPedInut.ano = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "cnpj":
                            dadosPedInut.CNPJ = dados[1].Trim();
                            break;
                        case "mod":
                            dadosPedInut.mod = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "serie":
                            dadosPedInut.serie = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "nnfini":
                            dadosPedInut.nNFIni = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "nnffin":
                            dadosPedInut.nNFFin = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "xjust":
                            dadosPedInut.xJust = dados[1].Trim();
                            break;
                        case "versao":
                            dadosPedInut.versao = dados[1].Trim();
                            break;
                    }
                }
#endif
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList InutNFeList = doc.GetElementsByTagName("inutNFe");

                foreach (XmlNode InutNFeNode in InutNFeList)
                {
                    XmlElement InutNFeElemento = (XmlElement)InutNFeNode;
                    dadosPedInut.versao = InutNFeElemento.Attributes["versao"].InnerText;

                    XmlNodeList infInutList = InutNFeElemento.GetElementsByTagName("infInut");
                    
                    foreach (XmlNode infInutNode in infInutList)
                    {
                        XmlElement infInutElemento = (XmlElement)infInutNode;
                        Functions.PopulateClasse(dadosPedInut, infInutElemento);                   
#if false
                        if (infInutElemento.GetElementsByTagName("tpAmb")[0] != null)
                            dadosPedInut.tpAmb = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                        if (infInutElemento.GetElementsByTagName("cUF")[0] != null)
                            dadosPedInut.cUF = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("cUF")[0].InnerText);

                        if (infInutElemento.GetElementsByTagName("ano")[0] != null)
                            dadosPedInut.ano = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("ano")[0].InnerText);

                        if (infInutElemento.GetElementsByTagName("CNPJ")[0] != null)
                            dadosPedInut.CNPJ = infInutElemento.GetElementsByTagName("CNPJ")[0].InnerText;

                        if (infInutElemento.GetElementsByTagName("mod")[0] != null)
                            dadosPedInut.mod = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("mod")[0].InnerText);

                        if (infInutElemento.GetElementsByTagName("serie")[0] != null)
                            dadosPedInut.serie = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("serie")[0].InnerText);

                        if (infInutElemento.GetElementsByTagName("nNFIni")[0] != null)
                            dadosPedInut.nNFIni = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFIni")[0].InnerText);

                        if (infInutElemento.GetElementsByTagName("nNFFin")[0] != null)
                            dadosPedInut.nNFFin = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFFin")[0].InnerText);
#endif               
                        if (infInutElemento.GetElementsByTagName("tpEmis").Count != 0)
                        {
                            dadosPedInut.tpEmis = Convert.ToInt16(infInutElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                            /// para que o validador não rejeite, excluo a tag <tpEmis>
                            doc.DocumentElement["infInut"].RemoveChild(infInutElemento.GetElementsByTagName("tpEmis")[0]);
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
            int emp = Functions.FindEmpresaByThread();

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

                    if (infInutElemento.GetElementsByTagName("cStat")[0].InnerText == "102") //Inutilização de Número Homologado
                    {
                        string strRetInutNFe = retInutNFeNode.OuterXml;

                        oGerarXML.XmlDistInut(NomeArquivoXML, strRetInutNFe);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, DateTime.Now);

                        //Move o arquivo de Distribuição para a pasta de enviados autorizados
                        string strNomeArqProcInutNFe = Empresa.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedInu_XML) + Propriedade.ExtRetorno.ProcInutNFe;
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

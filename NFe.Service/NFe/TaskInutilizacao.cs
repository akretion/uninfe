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
    public class TaskInutilizacao: TaskAbst
    {
        /// <summary>
        /// Envia o XML de inutilização de numeração de notas fiscais
        /// </summary>
        /// <remarks>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// No caso da Inutilização se tudo estiver correto retorna um XML
        /// dizendo se foi inutilizado corretamente ou não.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </remarks>
        /// <example>
        /// oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
        /// oUniNfe.Inutilizacao();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<?xml version="1.0" encoding="UTF-8" ?> 
        /// //<retInutNFe xmlns="http://www.portalfiscal.inf.br/nfe" xmlns:ds="http://www.w3.org/2000/09/xmldsig#" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.portalfiscal.inf.br/nfe retInutNFe_v1.07.xsd" versao="1.07">
        /// //   <infInut>
        /// //      <tpAmb>2</tpAmb> 
        /// //      <verAplic>1.10</verAplic> 
        /// //      <cStat>102</cStat> 
        /// //      <xMotivo>Inutilizacao de numero homologado</xMotivo> 
        /// //      <cUF>51</cUF> 
        /// //      <ano>08</ano> 
        /// //      <CNPJ>12345678901234</CNPJ> 
        /// //      <mod>55</mod> 
        /// //      <serie>1</serie> 
        /// //      <nNFIni>101</nNFIni> 
        /// //      <nNFFin>101</nNFFin> 
        /// //      <dhRecbto>2008-07-01T16:47:11</dhRecbto> 
        /// //      <nProt>151080000197712</nProt> 
        /// //   </infInut>
        /// //</retInutNFe>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>

        #region Classe com os dados do XML do pedido de inutilização de números de NF
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de inutilizacao
        /// </summary>
        private DadosPedInut oDadosPedInut;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Functions.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.InutilizarNumerosNFe;

            try
            {
                oDadosPedInut = new DadosPedInut(emp);
                //Ler o XML para pegar parâmetros de envio
                //LerXML oLer = new LerXML();
                ///*oLer.*/
                PedInut(emp, NomeArquivoXML);

                if(this.vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.InutilizarNumerosNFe, emp, /*oLer.*/oDadosPedInut.cUF, /*oLer.*/oDadosPedInut.tpAmb, /*oLer.*/oDadosPedInut.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oInutilizacao = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*oLer.*/oDadosPedInut.cUF));
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(oDadosPedInut.cUF, Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", /*oLer.*/oDadosPedInut.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLInut);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(/*oLer.*/oDadosPedInut.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oInutilizacao, NomeMetodoWS(Servico, /*oLer.*/oDadosPedInut.cUF), oCabecMsg, this, "-ped-inu", "-inu");

                    //Ler o retorno do webservice
                    this.LerRetornoInut();
                }
                else
                {
                    oGerarXML.Inutilizacao(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                        /*oLer.*/oDadosPedInut.tpAmb,
                        /*oLer.*/oDadosPedInut.tpEmis,
                        /*oLer.*/oDadosPedInut.cUF,
                        /*oLer.*/oDadosPedInut.ano,
                        /*oLer.*/oDadosPedInut.CNPJ,
                        /*oLer.*/oDadosPedInut.mod,
                        /*oLer.*/oDadosPedInut.serie,
                        /*oLer.*/oDadosPedInut.nNFIni,
                        /*oLer.*/oDadosPedInut.nNFFin,
                        /*oLer.*/oDadosPedInut.xJust);
                }

            }
            catch(Exception ex)
            {
                string ExtRet = string.Empty;

                if(this.vXmlNfeDadosMsgEhXML) //Se for XML
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
                    if(!this.vXmlNfeDadosMsgEhXML) //Se for o TXT para ser transformado em XML, vamos excluir o TXT depois de gerado o XML
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

            this.oDadosPedInut.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.oDadosPedInut.tpEmis = Empresa.Configuracoes[emp].tpEmis;

            if(Path.GetExtension(cArquivoXML).ToLower() == ".txt")
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
                List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
                foreach(string cTexto in cLinhas)
                {
                    string[] dados = cTexto.Split('|');
                    switch(dados[0].ToLower())
                    {
                        case "tpamb":
                            this.oDadosPedInut.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "tpemis":
                            this.oDadosPedInut.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "cuf":
                            this.oDadosPedInut.cUF = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "ano":
                            this.oDadosPedInut.ano = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "cnpj":
                            this.oDadosPedInut.CNPJ = dados[1].Trim();
                            break;
                        case "mod":
                            this.oDadosPedInut.mod = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "serie":
                            this.oDadosPedInut.serie = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "nnfini":
                            this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "nnffin":
                            this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "xjust":
                            this.oDadosPedInut.xJust = dados[1].Trim();
                            break;
                    }
                }
            }
            else
            {
                //<?xml version="1.0" encoding="UTF-8"?>
                //<inutNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
                //  <infInut Id="ID359999909091027055000000000001000000001">
                //      <tpAmb>2</tpAmb>
                //      <tpEmis>1</tpEmis>                  <<< opcional >>>
                //      <xServ>INUTILIZAR</xServ>
                //      <cUF>35</cUF>
                //      <ano>08</ano>
                //      <CNPJ>99999090910270</CNPJ>
                //      <mod>55</mod>
                //      <serie>0</serie>
                //      <nNFIni>1</nNFIni>
                //      <nNFFin>1</nNFFin>
                //      <xJust>Teste do WS de InutilizaÃ§Ã£o</xJust>
                //  </infInut>
                //</inutNFe>
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList InutNFeList = null;

                switch(Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        InutNFeList = doc.GetElementsByTagName("inutCTe");
                        break;

                    case TipoAplicativo.Nfe:
                        InutNFeList = doc.GetElementsByTagName("inutNFe");
                        break;

                    default:
                        break;
                }

                foreach(XmlNode InutNFeNode in InutNFeList)
                {
                    XmlElement InutNFeElemento = (XmlElement)InutNFeNode;

                    XmlNodeList infInutList = InutNFeElemento.GetElementsByTagName("infInut");

                    foreach(XmlNode infInutNode in infInutList)
                    {
                        XmlElement infInutElemento = (XmlElement)infInutNode;

                        if(infInutElemento.GetElementsByTagName("tpAmb")[0] != null)
                            this.oDadosPedInut.tpAmb = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                        if(infInutElemento.GetElementsByTagName("cUF")[0] != null)
                            this.oDadosPedInut.cUF = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("cUF")[0].InnerText);

                        if(infInutElemento.GetElementsByTagName("ano")[0] != null)
                            this.oDadosPedInut.ano = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("ano")[0].InnerText);

                        if(infInutElemento.GetElementsByTagName("CNPJ")[0] != null)
                            this.oDadosPedInut.CNPJ = infInutElemento.GetElementsByTagName("CNPJ")[0].InnerText;

                        if(infInutElemento.GetElementsByTagName("mod")[0] != null)
                            this.oDadosPedInut.mod = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("mod")[0].InnerText);

                        if(infInutElemento.GetElementsByTagName("serie")[0] != null)
                            this.oDadosPedInut.serie = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("serie")[0].InnerText);

                        switch(Propriedade.TipoAplicativo)
                        {
                            case TipoAplicativo.Cte:
                                if(infInutElemento.GetElementsByTagName("nCTIni")[0] != null)
                                    this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nCTIni")[0].InnerText);

                                if(infInutElemento.GetElementsByTagName("nCTFin")[0] != null)
                                    this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nCTFin")[0].InnerText);
                                break;

                            case TipoAplicativo.Nfe:
                                if(infInutElemento.GetElementsByTagName("nNFIni")[0] != null)
                                    this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFIni")[0].InnerText);

                                if(infInutElemento.GetElementsByTagName("nNFFin")[0] != null)
                                    this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFFin")[0].InnerText);
                                break;

                            default:
                                break;
                        }

                        ///
                        /// danasa 12-9-2009
                        /// 
                        if(infInutElemento.GetElementsByTagName("tpEmis").Count != 0)
                        {
                            this.oDadosPedInut.tpEmis = Convert.ToInt16(infInutElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                            /// para que o validador não rejeite, excluo a tag <tpEmis>
                            doc.DocumentElement["infInut"].RemoveChild(infInutElemento.GetElementsByTagName("tpEmis")[0]);
                            /// salvo o arquivo modificado
                            doc.Save(cArquivoXML);
                        }
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
            int emp = Functions.FindEmpresaByThread();

            XmlDocument doc = new XmlDocument();

            MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
            doc.Load(msXml);

            XmlNodeList retInutNFeList = null;

            switch(Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Cte:
                    retInutNFeList = doc.GetElementsByTagName("retInutCTe");
                    break;
                case TipoAplicativo.Nfe:
                    retInutNFeList = doc.GetElementsByTagName("retInutNFe");
                    break;
                default:
                    break;
            }

            foreach(XmlNode retInutNFeNode in retInutNFeList)
            {
                XmlElement retInutNFeElemento = (XmlElement)retInutNFeNode;

                XmlNodeList infInutList = retInutNFeElemento.GetElementsByTagName("infInut");

                foreach(XmlNode infInutNode in infInutList)
                {
                    XmlElement infInutElemento = (XmlElement)infInutNode;

                    if(infInutElemento.GetElementsByTagName("cStat")[0].InnerText == "102") //Inutilização de Número Homologado
                    {
                        string strRetInutNFe = retInutNFeNode.OuterXml;

                        oGerarXML.XmlDistInut(NomeArquivoXML, strRetInutNFe);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, DateTime.Now);

                        //Move o arquivo de Distribuição para a pasta de enviados autorizados
                        string strNomeArqProcInutNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
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

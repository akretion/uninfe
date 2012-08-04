using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    public class TaskConsultaNFDest : TaskAbst
    {
        #region Classe com os dados do XML do registro de consulta da nfe do destinatario
        private DadosConsultaNFeDest oDadosConsultaNFeDest = new DadosConsultaNFeDest();
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            Servico = Servicos.ConsultaNFDest;

            try
            {
                oDadosConsultaNFeDest = new DadosConsultaNFeDest();
                //Ler o XML para pegar parâmetros de envio
                //LerXML oLer = new LerXML();
                /*oLer.*/EnvConsultaNFeDest(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    int cUF = Empresa.Configuracoes[emp].UFCod;

                    //cUF = 43;

                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(
                        Servico,
                        emp,
                        cUF,
                        /*oLer.*/oDadosConsultaNFeDest.tpAmb,
                        1);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oConsNFDestEvento = wsProxy.CriarObjeto("NfeConsultaDest");
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(cUF));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", cUF.ToString());
                    //if (cUF == 35)
                    //{
                        //wsProxy.SetProp(oCabecMsg, "indComp", "0");
                    //}
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvConsultaNFeDest);

                    //Criar objeto da classe de assinatura digital
                    //AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    //oAD.Assinar(NomeArquivoXML, emp);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, 
                                        oConsNFDestEvento, 
                                        "nfeConsultaNFDest", 
                                        oCabecMsg, 
                                        this, 
                                        Propriedade.ExtEnvio.ConsNFeDest_XML.Replace(".xml", ""), 
                                        Propriedade.ExtRetorno.retConsNFeDest_XML.Replace(".xml", ""));

                    //Ler o retorno
                    this.LerRetornoConsultaNFeDest(emp);
                }
                else
                {
                    // Gerar o XML de eventos a partir do TXT gerado pelo ERP
                    oGerarXML.EnvioConsultaNFeDest(Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.ConsNFeDest_TXT) + Propriedade.ExtEnvio.ConsNFeDest_XML, /*oLer.*/oDadosConsultaNFeDest);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.ConsNFeDest_XML : Propriedade.ExtEnvio.ConsNFeDest_TXT;
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.retConsNFeDest_ERR, ex);
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
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de evento, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region EnvConsultaNFeDest
        private void EnvConsultaNFeDest(int emp, string arquivoXML)
        {
            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Nfe:
                    if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
                    {
                        /// tpAmb|2
                        /// CNPJ|10290739000139 
                        /// indNFe|0
                        /// indEmi|0
                        /// ultNSU|00000001
                        List<string> cLinhas = Functions.LerArquivo(arquivoXML);
                        foreach (string cTexto in cLinhas)
                        {
                            string[] dados = cTexto.Split('|');
                            if (dados.GetLength(0) <= 1) continue;

                            switch (dados[0].ToLower())
                            {
                                case "tpamb":
                                    this.oDadosConsultaNFeDest.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "cnpj":
                                    this.oDadosConsultaNFeDest.CNPJ = dados[1].Trim();
                                    break;
                                case "indnfe":
                                    this.oDadosConsultaNFeDest.indNFe = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "indemi":
                                    this.oDadosConsultaNFeDest.indEmi = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "ultnsu":
                                    this.oDadosConsultaNFeDest.ultNSU = dados[1].Trim();
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //<?xml version="1.0" encoding="UTF-8"?>
                        //<consNFeDest versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                        //      <tpAmb>2</tpAmb>
                        //      <xServ>CONSULTAR NFE DEST</xServ>
                        //      <CNPJ>10290739000139</CNPJ>
                        //      <indNFe>0</indNFe>
                        //      <indEmi>0</indEmi>
                        //      <ultNSU>000000000000000</ultNSU>
                        //</consNFeDest>

                        XmlDocument doc = new XmlDocument();
                        doc.Load(arquivoXML);

                        XmlNodeList envEventoList = doc.GetElementsByTagName("consNFeDest");

                        foreach (XmlNode envEventoNode in envEventoList)
                        {
                            XmlElement envEventoElemento = (XmlElement)envEventoNode;

                            this.oDadosConsultaNFeDest.tpAmb = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                            this.oDadosConsultaNFeDest.CNPJ = envEventoElemento.GetElementsByTagName("CNPJ")[0].InnerText;
                            this.oDadosConsultaNFeDest.indNFe = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("indNFe")[0].InnerText);
                            this.oDadosConsultaNFeDest.indEmi = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("indEmi")[0].InnerText);
                            this.oDadosConsultaNFeDest.ultNSU = envEventoElemento.GetElementsByTagName("ultNSU")[0].InnerText;
                        }
                    }
                    break;
            }
        }
        #endregion

        #region LerRetornoConsultaNFeDest
        private void LerRetornoConsultaNFeDest(int emp)
        {
            /*
            <retConsNFeDest versao="1.01">
                <tpAmb>2</tpAmb>
                <verAplic>1.0.0</verAplic>
                <cStat>137</cStat>
                <xMotivo>Nenhum documento localizado para o destinatario</xMotivo>
                <dhResp>2012-07-10T09:59:24</dhResp>
                <indCont>1</indCont>
                <ultNSU>102668467</ultNSU>
                <ret>   0..50
                    <resNFe>
                    </resNFe>
                    <resCanc>
                    </resCanc>
                    <resCCe>
                    </resCCe>
                </ret>
            </retConsNFeDest>              
             */
        }
        #endregion
    }
}

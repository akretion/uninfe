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
    public class TaskConsultaStatus: TaskAbst
    {
        /// <summary>
        /// Verificar o status do Serviço da NFe do SEFAZ em questão
        /// </summary>
        /// <remark>
        /// Como retorno, o método atualiza a propriedade this.vNfeRetorno da classe 
        /// com o conteúdo do retorno do WebService.
        /// No caso do StatusServico se tudo estiver correto retorna um XML
        /// dizendo que o serviço está em operação
        /// Se der algum erro ele grava um arquivo txt com a extensão .ERR com o conteúdo do erro
        /// </remark>
        /// <example>
        /// oUniNfe.vUF = 51; //Setar o Estado que é para ser verificado o status do serviço
        /// oUniNfe.vXmlNfeDadosMsg = "c:\pedstatus.xml";
        /// oUniNfe.StatusServico();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// // <?xml version="1.0" encoding="UTF-8"?>
        /// //   <retConsStatServ xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
        /// //      <tpAmb>2</tpAmb>
        /// //      <verAplic>1.10</verAplic>
        /// //      <cStat>107</cStat>
        /// //      <xMotivo>Servico em Operacao</xMotivo>
        /// //      <cUF>51</cUF>
        /// //      <dhRecbto>2008-06-12T11:16:55</dhRecbto>
        /// //      <tMed>2</tMed>
        /// //   </retConsStatServ>
        /// </example>
        /// <by>
        /// Wandrey Mundin Ferreira
        /// </by>
        /// <date>
        /// 01/04/2008
        /// </date>
        /// 

        #region Classe com os dados do XML da consulta do status do serviço da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do status do serviço
        /// </summary>
        private DadosPedSta oDadosPedSta;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Functions.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoConsultaStatusServicoNFe;

            try
            {
                oDadosPedSta = new DadosPedSta();
                //Ler o XML para pegar parâmetros de envio
                //var oLer = new LerXML();
                ///*oLer.*/
                PedSta(emp, NomeArquivoXML);

                if(vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoConsultaStatusServicoNFe, emp, /*oLer.*/oDadosPedSta.cUF, /*oLer.*/oDadosPedSta.tpAmb, /*oLer.*/oDadosPedSta.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    var oStatusServico = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*oLer.*/oDadosPedSta.cUF));
                    var oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(oDadosPedSta.cUF, Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", /*oLer.*/oDadosPedSta.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLStatusServico);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oStatusServico, NomeMetodoWS(Servico, /*oLer.*/oDadosPedSta.cUF), oCabecMsg, this, "-ped-sta", "-sta");
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.StatusServico(System.IO.Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                        /*oLer.*/oDadosPedSta.tpAmb,
                        /*oLer.*/oDadosPedSta.tpEmis,
                        /*oLer.*/oDadosPedSta.cUF);
                }
            }
            catch(Exception ex)
            {
                var extRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.PedSta_XML : Propriedade.ExtEnvio.PedSta_TXT;

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
        #endregion

        #region PedSta()
        /// <summary>
        /// Faz a leitura do XML de pedido do status de serviço
        /// </summary>
        /// <param name="cArquivoXml">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        private void PedSta(int emp, string cArquivoXML)
        {
            //int emp = Functions.FindEmpresaByThread();

            this.oDadosPedSta.tpAmb = 0;
            this.oDadosPedSta.cUF = Empresa.Configuracoes[emp].UFCod;
            ///
            /// danasa 9-2009
            /// Assume o que está na configuracao
            /// 
            this.oDadosPedSta.tpEmis = Empresa.Configuracoes[emp].tpEmis;

            ///
            /// danasa 12-9-2009
            /// 
            if(Path.GetExtension(cArquivoXML).ToLower() == ".txt")
            {
                switch(Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        break;

                    case TipoAplicativo.Nfe:
                        // tpEmis|1						<<< opcional >>>
                        // tpAmb|1
                        // cUF|35
                        List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
                        foreach(string cTexto in cLinhas)
                        {
                            string[] dados = cTexto.Split('|');
                            switch(dados[0].ToLower())
                            {
                                case "tpamb":
                                    this.oDadosPedSta.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "cuf":
                                    this.oDadosPedSta.cUF = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "tpemis":
                                    this.oDadosPedSta.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                //<?xml version="1.0" encoding="UTF-8"?>
                //<consStatServ xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
                //  <tpAmb>2</tpAmb>
                //  <cUF>35</cUF>
                //  <xServ>STATUS</xServ>
                //</consStatServ>                    

                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consStatServList = null;

                switch(Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        consStatServList = doc.GetElementsByTagName("consStatServCte");
                        break;

                    case TipoAplicativo.Nfe:
                        consStatServList = doc.GetElementsByTagName("consStatServ");
                        break;

                    default:
                        break;
                }

                foreach(XmlNode consStatServNode in consStatServList)
                {
                    XmlElement consStatServElemento = (XmlElement)consStatServNode;

                    this.oDadosPedSta.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                    if(consStatServElemento.GetElementsByTagName("cUF").Count != 0)
                    {
                        this.oDadosPedSta.cUF = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName("cUF")[0].InnerText);

                        //Se for o UniCTe tem que remover a tag da UF
                        if(Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                        {
                            doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName("cUF")[0]);
                        }
                    }

                    if(consStatServElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        this.oDadosPedSta.tpEmis = Convert.ToInt16(consStatServElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName("tpEmis")[0]);
                        /// salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
                }
            }
        }
        #endregion
    }
}

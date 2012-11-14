using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;

namespace NFe.Service
{
    public class TaskPedidoConsultaSituacaoNFe : TaskAbst
    {
        /// <summary>
        /// Envia o XML de consulta da situação da nota fiscal
        /// </summary>
        /// <remarks>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// No caso da Consulta se tudo estiver correto retorna um XML
        /// com a situação da nota fiscal (Se autorizada ou não).
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </remarks>
        /// <example>
        /// oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
        /// oUniNfe.Consulta();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<?xml version="1.0" encoding="UTF-8" ?>
        /// //   <retConsSitNFe versao="1.07" xmlns="http://www.portalfiscal.inf.br/nfe">
        /// //      <infProt>
        /// //         <tpAmb>2</tpAmb>
        /// //         <verAplic>1.10</verAplic>
        /// //         <cStat>100</cStat>
        /// //         <xMotivo>Autorizado o uso da NF-e</xMotivo>
        /// //         <cUF>51</cUF>
        /// //         <chNFe>51080612345678901234550010000001041671821888</chNFe>
        /// //         <dhRecbto>2008-06-27T15:01:48</dhRecbto>
        /// //         <nProt>151080000194296</nProt>
        /// //         <digVal>WHM/TzTvF+LrdUwtwvk26qgsko0=</digVal>
        /// //      </infProt>
        /// //   </retConsSitNFe>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>

        #region Classe com os dados do XML da pedido de consulta da situação da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta da situação da nota
        /// </summary>
        private DadosPedSit oDadosPedSit;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoConsultaSituacaoNFe;

            try
            {
                oDadosPedSit = new DadosPedSit();
                //Ler o XML para pegar parâmetros de envio
                //LerXML oLer = new LerXML();
                ///*oLer.*/
                PedSit(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = null;
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoConsultaSituacaoNFe, emp, /*oLer.*/oDadosPedSit.cUF, /*oLer.*/oDadosPedSit.tpAmb, /*oLer.*/oDadosPedSit.tpEmis);
                            break;
                        case TipoAplicativo.Nfe:
                            wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoConsultaSituacaoNFe, emp, /*oLer.*/oDadosPedSit.cUF, /*oLer.*/oDadosPedSit.tpAmb, /*oLer.*/oDadosPedSit.tpEmis, /*oLer.*/oDadosPedSit.versaoNFe);
                            break;
                        default:
                            break;
                    }

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    if (/*oLer.*/oDadosPedSit.versaoNFe == 1 && Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
                    {
                        object oConsulta = null;
                        if (/*oLer.*/oDadosPedSit.cUF == 41)
                            oConsulta = wsProxy.CriarObjeto("NfeConsultaService");
                        else
                            oConsulta = wsProxy.CriarObjeto("NfeConsulta");

                        //Invocar o método que envia o XML para o SEFAZ
                        oInvocarObj.Invocar(wsProxy, oConsulta, "nfeConsultaNF", this);
                    }
                    else
                    {
                        object oConsulta = wsProxy.CriarObjeto(NomeClasseWS(Servico, /*oLer.*/oDadosPedSit.cUF));
                        object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(oDadosPedSit.cUF, Servico));

                        //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                        wsProxy.SetProp(oCabecMsg, "cUF", /*oLer.*/oDadosPedSit.cUF.ToString());
                        switch (Propriedade.TipoAplicativo)
                        {
                            case TipoAplicativo.Cte:
                                wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLPedSit);
                                break;
                            case TipoAplicativo.Nfe:
                                wsProxy.SetProp(oCabecMsg, "versaoDados", /*oLer.*/oDadosPedSit.versaoNFe == 201 ? "2.01" : ConfiguracaoApp.VersaoXMLPedSit);
                                break;
                            default:
                                break;
                        }

                        //Invocar o método que envia o XML para o SEFAZ
                        oInvocarObj.Invocar(wsProxy, oConsulta, NomeMetodoWS(Servico, /*oLer.*/oDadosPedSit.cUF), oCabecMsg, this);
                    }

                    //Efetuar a leitura do retorno da situação para ver se foi autorizada ou não
                    //Na versão 1 não posso gerar o -procNfe, ou vou ter que tratar a estrutura do XML de acordo com a versão, a consulta na versão 1 é somente para obter o resultado mesmo.
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            this.LerRetornoSitCTe(/*oLer.*/oDadosPedSit.chNFe);
                            break;
                        case TipoAplicativo.Nfe:
                            if (/*oLer.*/oDadosPedSit.versaoNFe != 1)
                                this.LerRetornoSitNFe(/*oLer.*/oDadosPedSit.chNFe);
                            break;
                        default:
                            break;
                    }

                    //Gerar o retorno para o ERP
                    oGerarXML.XmlRetorno(Propriedade.ExtEnvio.PedSit_XML, Propriedade.ExtRetorno.Sit_XML, this.vStrXmlRetorno);
                }
                else
                {
                    oGerarXML.Consulta(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                        /*oLer.*/oDadosPedSit.tpAmb,
                        /*oLer.*/oDadosPedSit.tpEmis,
                        /*oLer.*/oDadosPedSit.chNFe);
                }
            }
            catch (Exception ex)
            {
                string ExtRet = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtRet = Propriedade.ExtEnvio.PedSit_XML;
                else //Se for TXT
                    ExtRet = Propriedade.ExtEnvio.PedSit_TXT;

                try
                {
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.Sit_ERR, ex);
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
                    //Se falhou algo na hora de deletar o XML de pedido da consulta da situação da NFe, infelizmente
                    //não posso fazser mais nada, o UniNFe vai tentar mantar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 22/03/2010
                }
            }
        }
        #endregion

        #region PedSit()
        /// <summary>
        /// Faz a leitura do XML de pedido de consulta da situação da NFe
        /// </summary>
        /// <param name="cArquivoXML">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        private void PedSit(int emp, string cArquivoXML)
        {
            //int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            this.oDadosPedSit.tpAmb = Empresa.Configuracoes[emp].tpAmb;// string.Empty;
            this.oDadosPedSit.chNFe = string.Empty;

            try
            {
                if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
                {
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            break;

                        case TipoAplicativo.Nfe:
                            //      tpAmb|2
                            //      tpEmis|1                <<< opcional >>>
                            //      chNFe|35080600000000000000550000000000010000000000
                            List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
                            foreach (string cTexto in cLinhas)
                            {
                                string[] dados = cTexto.Split('|');
                                switch (dados[0].ToLower())
                                {
                                    case "tpamb":
                                        this.oDadosPedSit.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                        break;
                                    case "tpemis":
                                        this.oDadosPedSit.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                                        break;
                                    case "chnfe":
                                        this.oDadosPedSit.chNFe = dados[1].Trim();
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
                    XmlDocument doc = new XmlDocument();
                    doc.Load(cArquivoXML);

                    XmlNodeList consSitNFeList = null;

                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            consSitNFeList = doc.GetElementsByTagName("consSitCTe");
                            break;

                        case TipoAplicativo.Nfe:
                            consSitNFeList = doc.GetElementsByTagName("consSitNFe");
                            break;

                        default:
                            break;
                    }

                    foreach (XmlNode consSitNFeNode in consSitNFeList)
                    {
                        XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                        this.oDadosPedSit.tpAmb = Convert.ToInt32("0" + consSitNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                        switch (Propriedade.TipoAplicativo)
                        {
                            case TipoAplicativo.Cte:
                                this.oDadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName("chCTe")[0].InnerText;
                                break;

                            case TipoAplicativo.Nfe:
                                this.oDadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName("chNFe")[0].InnerText;

                                //Definir a versão do XML para resolver o problema do Estado do Paraná e Goiás que não migrou o banco de dados
                                //da versão 1.0 para a 2.0, sendo assim a consulta situação de notas fiscais tem que ser feita cada uma em seu 
                                //ambiente. Wandrey 23/03/2011
                                if (consSitNFeElemento.GetAttribute("versao") == "1.07" && (this.oDadosPedSit.cUF == 41 || this.oDadosPedSit.cUF == 52))
                                    this.oDadosPedSit.versaoNFe = 1;
                                else
                                {
                                    this.oDadosPedSit.versaoNFe = 2;

                                    bool _temCCe = ConfiguracaoApp.TemCCe(this.oDadosPedSit.chNFe.Substring(0, 2), this.oDadosPedSit.tpAmb, this.oDadosPedSit.tpEmis);

                                    if ((consSitNFeElemento.GetAttribute("versao") == "2.00" && _temCCe) ||
                                        (consSitNFeElemento.GetAttribute("versao") == "2.01" && !_temCCe))
                                    {
                                        //if (_temCCe && oDadosPedSit.cUF != 31)
                                        if (_temCCe)
                                            this.oDadosPedSit.versaoNFe = 201;

                                        //consSitNFeElemento.Attributes["versao"].InnerText = _temCCe && oDadosPedSit.cUF != 31 ? "2.01" : ConfiguracaoApp.VersaoXMLPedSit;
                                        consSitNFeElemento.Attributes["versao"].InnerText = _temCCe ? "2.01" : ConfiguracaoApp.VersaoXMLPedSit;
                                        doc.Save(cArquivoXML);
                                    }
                                    else
                                        if (consSitNFeElemento.GetAttribute("versao") == "2.01")
                                            this.oDadosPedSit.versaoNFe = 201;
                                }
                                break;

                            default:
                                break;
                        }

                        ///
                        /// danasa 12-9-2009
                        /// 
                        if (consSitNFeElemento.GetElementsByTagName("tpEmis").Count != 0)
                        {
                            this.oDadosPedSit.tpEmis = Convert.ToInt16(consSitNFeElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                            /// para que o validador não rejeite, excluo a tag <tpEmis>
                            doc.DocumentElement.RemoveChild(consSitNFeElemento.GetElementsByTagName("tpEmis")[0]);
                            /// salvo o arquivo modificado
                            doc.Save(cArquivoXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region LerRetornoSitCTe()
        /// <summary>
        /// Ler o retorno da consulta situação da nota fiscal e de acordo com o status ele trata as notas enviadas se ainda não foram tratadas
        /// </summary>
        /// <param name="ChaveNFe">Chave da NFe que está sendo consultada</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/06/2010
        /// </remarks>
        private void LerRetornoSitCTe(string ChaveNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            LerXML oLerXml = new LerXML();
            MemoryStream msXml = Functions.StringXmlToStream(vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                XmlNodeList retConsSitList = doc.GetElementsByTagName("retConsSitCTe");

                foreach (XmlNode retConsSitNode in retConsSitList)
                {
                    XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                    //Definir a chave da NFe a ser pesquisada
                    string strChaveNFe = "CTe" + ChaveNFe;

                    //Definir o nome do arquivo da NFe e seu caminho
                    string strNomeArqNfe = oFluxoNfe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                    if (string.IsNullOrEmpty(strNomeArqNfe))
                    {
                        if (string.IsNullOrEmpty(strChaveNFe))
                            throw new Exception("LerRetornoSitCTe(): Não pode obter o nome do arquivo");

                        strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.ExtEnvio.Nfe;
                    }

                    string strArquivoNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                    //Pegar o status de retorno da NFe que está sendo consultada a situação
                    var cStatCons = string.Empty;
                    if (retConsSitElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatCons = retConsSitElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }

                    switch (cStatCons)
                    {
                        #region Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)
                        case "280": //A-Certificado transmissor inválido
                        case "281": //A-Validade do certificado
                        case "283": //A-Verifica a cadeia de Certificação
                        case "286": //A-LCR do certificado de Transmissor
                        case "284": //A-Certificado do Transmissor revogado
                        case "285": //A-Certificado Raiz difere da "IPC-Brasil"
                        case "282": //A-Falta a extensão de CNPJ no Certificado
                        case "214": //B-Tamanho do XML de dados superior a 500 Kbytes
                        case "243": //B-XML de dados mal formatado
                        case "108": //B-Verifica se o Serviço está paralisado momentaneamente
                        case "109": //B-Verifica se o serviço está paralisado sem previsão
                        case "242": //C-Elemento nfeCabecMsg inexistente no SOAP Header
                        case "409": //C-Campo cUF inexistente no elemento nfeCabecMsg do SOAP Header
                        case "410": //C-Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP
                        case "411": //C-Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP
                        case "238": //C-Versão dos Dados informada é superior à versão vigente
                        case "239": //C-Versão dos Dados não suportada
                        case "215": //D-Verifica schema XML da área de dados
                        case "404": //D-Verifica o uso de prefixo no namespace
                        case "402": //D-XML utiliza codificação diferente de UTF-8
                        case "252": //E-Tipo do ambiente da NF-e difere do ambiente do web service
                        case "226": //E-UF da Chave de Acesso difere da UF do Web Service
                        case "236": //E-Valida DV da Chave de Acesso
                        case "216": //E-Verificar se campo "Codigo Numerico"
                            break;
                        #endregion

                        #region Nota fiscal rejeitada
                        case "217": //J-NFe não existe na base de dados do SEFAZ
                            goto case "TirarFluxo";
                        #endregion

                        #region Nota fiscal autorizada
                        case "100": //Autorizado o uso da NFe
                        case "150":
                            XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                            if (infConsSitList != null)
                            {
                                foreach (XmlNode infConsSitNode in infConsSitList)
                                {
                                    XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                    //Pegar o Status do Retorno da consulta situação
                                    string strStat = Functions.LerTag(infConsSitElemento, "cStat").Replace(";", "");

                                    switch (strStat)
                                    {
                                        case "100":
                                        case "150":
                                            //O retorno da consulta situação a posição das tag´s é diferente do que vem 
                                            //na consulta do recibo, assim sendo tenho que montar esta parte do XML manualmente
                                            //para que fique um XML de distribuição válido. Wandrey 07/10/2009
                                            string atributoId = string.Empty;
                                            if (infConsSitElemento.GetAttribute("Id").Length != 0)
                                            {
                                                atributoId = " Id=\"" + infConsSitElemento.GetAttribute("Id") + "\"";
                                            }

                                            string strProtNfe = "<protCTe versao=\"" + ConfiguracaoApp.VersaoXMLNFe + "\">" +
                                                "<infProt" + atributoId + ">" +
                                                "<tpAmb>" + Functions.LerTag(infConsSitElemento, "tpAmb", false) + "</tpAmb>" +
                                                "<verAplic>" + Functions.LerTag(infConsSitElemento, "verAplic", false) + "</verAplic>" +
                                                "<chCTe>" + Functions.LerTag(infConsSitElemento, "chCTe", false) + "</chCTe>" +
                                                "<dhRecbto>" + Functions.LerTag(infConsSitElemento, "dhRecbto", false) + "</dhRecbto>" +
                                                "<nProt>" + Functions.LerTag(infConsSitElemento, "nProt", false) + "</nProt>" +
                                                "<digVal>" + Functions.LerTag(infConsSitElemento, "digVal", false) + "</digVal>" +
                                                "<cStat>" + Functions.LerTag(infConsSitElemento, "cStat", false) + "</cStat>" +
                                                "<xMotivo>" + Functions.LerTag(infConsSitElemento, "xMotivo", false) + "</xMotivo>" +
                                                "</infProt>" +
                                                "</protCTe>";

                                            //Definir o nome do arquivo -procNfe.xml                                               
                                            string strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        Functions/*oAux*/.ExtrairNomeArq(strArquivoNFe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe;

                                            //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                            //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Verificar se a -nfe.xml existe na pasta de autorizados
                                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtEnvio.Nfe);

                                                //Verificar se o -procNfe.xml existe na past de autorizados
                                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);

                                                //Se o XML de distribuição não estiver na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    if (!File.Exists(strArquivoNFeProc))
                                                    {
                                                        oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcNFe);
                                                    }
                                                }

                                                //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                    TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }

                                                //Se a NFe não existir ainda na pasta de autorizados
                                                if (!NFeJaNaAutorizada)
                                                {
                                                    //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                    TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }
                                                else
                                                {
                                                    //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                    Functions.DeletarArquivo(strArquivoNFe);
                                                }

                                                //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
                                                //ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi);
                                                if (procNFeJaNaAutorizada)
                                                    TFunctions.ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi, "dacte");
                                            }

                                            if (File.Exists(strArquivoNFeProc))
                                            {
                                                //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                Functions.DeletarArquivo(strArquivoNFeProc);
                                            }

                                            break;

                                        case "301":
                                            //Ler o XML para pegar a data de emissão para criar a psta dos XML´s Denegados
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                                                if (!oAux.EstaDenegada(strArquivoNFe, oLerXml.oDadosNfe.dEmi))
                                                {
                                                    TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                                }
                                            }

                                            break;

                                        case "302":
                                            goto case "301";

                                        case "303":
                                            goto case "301";

                                        case "304":
                                            goto case "301";

                                        case "305":
                                            goto case "301";

                                        case "306":
                                            goto case "301";

                                        case "110": //Uso Denegado
                                            goto case "301";

                                        default:
                                            //Mover o XML da NFE a pasta de XML´s com erro
                                            oAux.MoveArqErro(strArquivoNFe);
                                            break;
                                    }

                                    //Deletar a NFE do arquivo de controle de fluxo
                                    oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                                }
                            }
                            break;
                        #endregion

                        #region Nota fiscal cancelada
                        case "101": //Cancelamento Homologado ou Nfe Cancelada
                            goto case "100";
                        #endregion

                        #region Nota fiscal Denegada
                        case "110": //NFe Denegada
                            goto case "100";

                        case "301": //NFe Denegada
                            goto case "100";

                        case "302": //NFe Denegada
                            goto case "100";

                        case "303": //NFe Denegada
                            goto case "100";

                        case "304": //NFe Denegada
                            goto case "100";

                        case "305": //NFe Denegada
                            goto case "100";

                        case "306": //NFe Denegada
                            goto case "100";
                        #endregion

                        #region Conteúdo para retirar a nota fiscal do fluxo
                        case "TirarFluxo":
                            //Mover o XML da NFE a pasta de XML´s com erro
                            oAux.MoveArqErro(strArquivoNFe);

                            //Deletar a NFE do arquivo de controle de fluxo
                            oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region LerRetornoSitNFe()
        /// <summary>
        /// Ler o retorno da consulta situação da nota fiscal e de acordo com o status ele trata as notas enviadas se ainda não foram tratadas
        /// </summary>
        /// <param name="ChaveNFe">Chave da NFe que está sendo consultada</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/06/2010
        /// </remarks>
        private void LerRetornoSitNFe(string ChaveNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            LerXML oLerXml = new LerXML();
            MemoryStream msXml = Functions.StringXmlToStreamUTF8(this.vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                #region Distribuicao de Eventos

                oGerarXML.XmlDistEvento(emp, this.vStrXmlRetorno);  //<<<danasa 6-2011

                #endregion

                #region Cancelamento NFe
                new TaskCancelamento().GerarXmlDistCanc(ChaveNFe, this.vStrXmlRetorno, this.oGerarXML);  //Wandrey 12/01/2012
                #endregion

                XmlNodeList retConsSitList = doc.GetElementsByTagName("retConsSitNFe");

                foreach (XmlNode retConsSitNode in retConsSitList)
                {
                    XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                    //Definir a chave da NFe a ser pesquisada
                    string strChaveNFe = "NFe" + ChaveNFe;

                    //Definir o nome do arquivo da NFe e seu caminho
                    string strNomeArqNfe = oFluxoNfe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                    if (string.IsNullOrEmpty(strNomeArqNfe))
                    {
                        if (string.IsNullOrEmpty(strChaveNFe))
                            throw new Exception("LerRetornoSitNFe(): Não pode obter o nome do arquivo");

                        strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.ExtEnvio.Nfe;
                    }

                    string strArquivoNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                    //Pegar o status de retorno da NFe que está sendo consultada a situação
                    var cStatCons = string.Empty;
                    if (retConsSitElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatCons = retConsSitElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }

                    switch (cStatCons)
                    {
                        #region Rejeições do XML de consulta da situação da NFe (Não é a nfe que foi rejeitada e sim o XML de consulta da situação da nfe)

                        #region Validação do Certificado de Transmissão
                        case "280":
                        case "281":
                        case "283":
                        case "286":
                        case "284":
                        case "285":
                        case "282":
                        #endregion

                        #region Validação Inicial da Mensagem no WebService
                        case "214":
                        case "243":
                        case "108":
                        case "109":
                        #endregion

                        #region Validação das informações de controle da chamada ao WebService
                        case "242":
                        case "409":
                        case "410":
                        case "411":
                        case "238":
                        case "239":
                        #endregion

                        #region Validação da forma da área de dados
                        case "215":
                        case "516":
                        case "517":
                        case "545":
                        case "587":
                        case "588":
                        case "404":
                        case "402":
                        #endregion

                        #region Validação das regras de negócios da consulta a NF-e
                        case "252":
                        case "226":
                        case "236":
                        case "614":
                        case "615":
                        case "616":
                        case "617":
                        case "618":
                        case "619":
                        case "620":
                            break;
                        #endregion

                        #region Nota fiscal rejeitada
                        case "217": //J-NFe não existe na base de dados do SEFAZ
                            goto case "TirarFluxo";

                        case "562": //J-Verificar se o campo "Código Numérico" informado na chave de acesso é diferente do existente no BD
                            goto case "TirarFluxo";

                        case "561": //J-Verificar se campo MM (mês) informado na Chave de Acesso é diferente do existente no BD
                            goto case "TirarFluxo";
                        #endregion

                        #endregion

                        #region Nota fiscal autorizada
                        case "100": //Autorizado o uso da NFe
                            XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                            if (infConsSitList != null)
                            {
                                foreach (XmlNode infConsSitNode in infConsSitList)
                                {
                                    XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                    //Pegar o Status do Retorno da consulta situação
                                    string strStat = Functions.LerTag(infConsSitElemento, "cStat").Replace(";", "");

                                    switch (strStat)
                                    {
                                        case "100":
                                            //O retorno da consulta situação a posição das tag´s é diferente do que vem 
                                            //na consulta do recibo, assim sendo tenho que montar esta parte do XML manualmente
                                            //para que fique um XML de distribuição válido. Wandrey 07/10/2009
                                            string strProtNfe = GeraStrProtNFe(infConsSitElemento);//danasa 11-4-2012

                                            //Definir o nome do arquivo -procNfe.xml                                               
                                            string strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        Functions.ExtrairNomeArq(strArquivoNFe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe;

                                            //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                            //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Verificar se a -nfe.xml existe na pasta de autorizados
                                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtEnvio.Nfe);

                                                //Verificar se o -procNfe.xml existe na past de autorizados
                                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);

                                                //Se o XML de distribuição não estiver na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    if (!File.Exists(strArquivoNFeProc))
                                                    {
                                                        oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcNFe);
                                                    }
                                                }

                                                //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                    TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                    //Atualizar a situação para que eu só mova o arquivo com final -NFe.xml para a pasta autorizado se 
                                                    //a procnfe já estiver lá, ou vai ficar na pasta emProcessamento para tentar gerar novamente.
                                                    //Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                    procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);
                                                }

                                                //Se a NFe não existir ainda na pasta de autorizados
                                                if (!NFeJaNaAutorizada)
                                                {
                                                    //1-Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                    //2-Só vou mover o -nfe.xml para a pasta autorizados se já existir a -procnfe.xml, caso contrário vou manter na pasta EmProcessamento
                                                    //  para tentar gerar novamente o -procnfe.xml
                                                    //  Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                    if (procNFeJaNaAutorizada)
                                                        TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }
                                                else
                                                {
                                                    //1-Se já estiver na pasta de autorizados, vou somente mover ela da pasta de XML´s em processamento
                                                    //2-Só vou mover o -nfe.xml da pasta EmProcessamento se também existir a -procnfe.xml na pasta autorizados, caso contrário vou manter na pasta EmProcessamento
                                                    //  para tentar gerar novamente o -procnfe.xml
                                                    //  Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                    if (procNFeJaNaAutorizada)
                                                        oAux.MoveArqErro(strArquivoNFe);
                                                    //oAux.DeletarArquivo(strArquivoNFe);
                                                }

                                                //Disparar a geração/impressão do UniDanfe. 03/02/2010 - Wandrey
                                                if (procNFeJaNaAutorizada)
                                                    TFunctions.ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi, "danfe");
                                            }

                                            if (File.Exists(strArquivoNFeProc))
                                            {
                                                //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                Functions.DeletarArquivo(strArquivoNFeProc);
                                            }

                                            break;

                                        //danasa 11-4-2012
                                        case "110": //Uso Denegado
                                        case "301":
                                        case "302":
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                            //
                                            // NFe existe na pasta EmProcessamento?
                                            ProcessaNotaDenegada(emp, oLerXml, strArquivoNFe, infConsSitElemento);
                                            break;

                                        default:
                                            //Mover o XML da NFE a pasta de XML´s com erro
                                            oAux.MoveArqErro(strArquivoNFe);
                                            break;
                                    }

                                    //Deletar a NFE do arquivo de controle de fluxo
                                    oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                                }
                            }
                            break;
                        #endregion

                        #region Nota fiscal cancelada
                        case "101": //Cancelamento Homologado ou Nfe Cancelada
                            goto case "100";
                        #endregion

                        #region Nota fiscal Denegada
                        case "110": //NFe Denegada
                            goto case "100";

                        case "301": //NFe Denegada
                            goto case "100";

                        case "302": //NFe Denegada
                            goto case "100";

                        case "205": //Nfe já está denegada na base do SEFAZ
                            goto case "100";    ///<<<<<<<<<< ??????????????????? >>>>>>>>>>>>
                        ///
                        //Ler o XML para pegar a data de emissão para criar a psta dos XML´s Denegados
                        /*
                        if (File.Exists(strArquivoNFe))
                        {
                            oLerXml.Nfe(strArquivoNFe);

                            //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                            if (!oAux.EstaDenegada(strArquivoNFe, oLerXml.oDadosNfe.dEmi))
                            {
                                oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                            }
                        }
                        break;*/

                        #endregion

                        #region Conteúdo para retirar a nota fiscal do fluxo
                        case "TirarFluxo":
                            //Mover o XML da NFE a pasta de XML´s com erro
                            oAux.MoveArqErro(strArquivoNFe);

                            //Deletar a NFE do arquivo de controle de fluxo
                            oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }
}

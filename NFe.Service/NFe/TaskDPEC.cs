using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    public class TaskDPECRecepcao : TaskAbst
    {
        #region Classe com os dados do XML de registro do DPEC
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do registro do DPEC
        /// </summary>
        private DadosEnvDPEC dadosEnvDPEC;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.DPECEnviar;

            try
            {
                dadosEnvDPEC = new DadosEnvDPEC();
                //Ler o XML para pegar parâmetros de envio
                EnvDPEC(emp, NomeArquivoXML);    //danasa 21/10/2010

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.DPECEnviar, emp, dadosEnvDPEC.cUF, dadosEnvDPEC.tpAmb, dadosEnvDPEC.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoDPEC = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);// "SCERecepcaoRFB");
                    object oCabecMsg = wsProxy.CriarObjeto("sceCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    //oWSProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.cUF.ToString(), /*oLer.*/dadosEnvDPEC.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLEnvDPEC);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(dadosEnvDPEC.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oRecepcaoDPEC, wsProxy.NomeMetodoWS[0] /*"sceRecepcaoDPEC"*/, oCabecMsg, this);

                    //Ler o retorno
                    LerRetDPEC();

                    //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                    //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                    oGerarXML.XmlRetorno(Propriedade.ExtEnvio.EnvDPEC_XML, Propriedade.ExtRetorno.retDPEC_XML, vStrXmlRetorno);
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.EnvioDPEC(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml", /*oLer.*/dadosEnvDPEC);
                }
            }
            catch (Exception ex)
            {
                var ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvDPEC_XML : Propriedade.ExtEnvio.EnvDPEC_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.retDPEC_ERR, ex);
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
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region EnvDPEC()
        /// <summary>
        /// Efetua a leitura do XML de registro do DPEC
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML de registro do DPEC</param>
        private void EnvDPEC(int emp, string arquivoXML)
        {
            this.dadosEnvDPEC.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            this.dadosEnvDPEC.tpEmis = (int)NFe.Components.TipoEmissao.teEPECeDPEC;
            this.dadosEnvDPEC.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;

            ///
            /// danasa 21/10/2010
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                ///cUF|31                   |
                ///tpAmb|2                  | opcional
                ///verProc|1.0.0
                ///CNPJ|10238568000360
                ///IE|148230665114
                ///------
                ///chNFe|31101010238568000360550010000001011000001011
                ///CNPJCPF|05481336000137   | se UF=EX->Branco
                ///UF|SP
                ///vNF|123456.00
                ///vICMS|18.00
                ///vST|121.99
                List<string> cLinhas = Functions.LerArquivo(arquivoXML);
                Functions.PopulateClasse(this.dadosEnvDPEC, cLinhas);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arquivoXML);

                XmlNodeList infDPECList = doc.GetElementsByTagName("infDPEC");

                foreach (XmlNode infDPECNode in infDPECList)
                {
                    XmlElement infDPECElemento = (XmlElement)infDPECNode;

                    this.dadosEnvDPEC.tpAmb = Convert.ToInt32("0" + infDPECElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                    this.dadosEnvDPEC.cUF = Convert.ToInt32("0" + infDPECElemento.GetElementsByTagName(NFe.Components.TpcnResources.cUF.ToString())[0].InnerText);
                }
            }
        }
        #endregion

        #region LerRetDPEC()
        private void LerRetDPEC()
        {
            int emp = Empresas.FindEmpresaByThread();

            XmlDocument doc = new XmlDocument();

            MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
            doc.Load(msXml);

            XmlNodeList retDPECList = doc.GetElementsByTagName("retDPEC");

            foreach (XmlNode retDPECNode in retDPECList)
            {
                XmlElement retDPECElemento = (XmlElement)retDPECNode;

                XmlNodeList infDPECRegList = retDPECElemento.GetElementsByTagName("infDPECReg");

                foreach (XmlNode infDPECRegNode in infDPECRegList)
                {
                    XmlElement infDPECRegElemento = (XmlElement)infDPECRegNode;

                    if (infDPECRegElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText == "124" ||
                        infDPECRegElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText == "125") //DPEC Homologado
                    {
                        string cChaveNFe = infDPECRegElemento.GetElementsByTagName(NFe.Components.TpcnResources.chNFe.ToString())[0].InnerText;
                        string dhRegDPEC = infDPECRegElemento.GetElementsByTagName("dhRegDPEC")[0].InnerText;
                        DateTime dtEmissaoDPEC = new DateTime(Convert.ToInt16(dhRegDPEC.Substring(0, 4)), Convert.ToInt16(dhRegDPEC.Substring(5, 2)), Convert.ToInt16(dhRegDPEC.Substring(8, 2)));

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, dtEmissaoDPEC);

                        //Gravar o XML retornado pelo WebService do SEFAZ na pasta de autorizados. Wandrey 25/11/2010
                        string nomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + 
                                                  PastaEnviados.Autorizados.ToString() + "\\" + 
                                                  Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(dtEmissaoDPEC);
                        oGerarXML.XmlRetorno(Propriedade.ExtEnvio.EnvDPEC_XML, Propriedade.ExtRetorno.retDPEC_XML, vStrXmlRetorno, nomePastaEnviado);
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

    public class TaskDPECConsulta : TaskAbst
    {
        #region Classe com os dados do XML de consulta do registro do DPEC
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do registro do DPEC
        /// </summary>
        private DadosConsDPEC dadosConsDPEC;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.DPECConsultar;

            try
            {
                dadosConsDPEC = new DadosConsDPEC();
                //Ler o XML para pegar parâmetros de envio
                ConsDPEC(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.DPECConsultar, emp, 0, dadosConsDPEC.tpAmb, dadosConsDPEC.tpEmis, string.Empty);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoDPEC = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);//"SCEConsultaRFB");
                    object oCabecMsg = wsProxy.CriarObjeto("sceCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLConsDPEC);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oRecepcaoDPEC, wsProxy.NomeMetodoWS[0]/* "sceConsultaDPEC"*/, oCabecMsg, this);

                    //Ler o retorno
                    LerRetConsDPEC(emp);

                    //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                    //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                    oGerarXML.XmlRetorno(Propriedade.ExtEnvio.ConsDPEC_XML, Propriedade.ExtRetorno.retConsDPEC_XML, vStrXmlRetorno);
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.ConsultaDPEC(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml", dadosConsDPEC);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.ConsDPEC_XML : Propriedade.ExtEnvio.ConsDPEC_TXT;
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.retConsDPEC_ERR, ex);
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
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region LerRetConsDPEC
        private void LerRetConsDPEC(int emp)
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
            doc.Load(msXml);

            XmlNodeList retDPECList = doc.GetElementsByTagName("retConsDPEC");

            foreach (XmlNode retDPECNode in retDPECList)
            {
                XmlElement retDPECElemento = (XmlElement)retDPECNode;

                XmlNodeList infDPECRegList = retDPECElemento.GetElementsByTagName("infDPECReg");

                foreach (XmlNode infDPECRegNode in infDPECRegList)
                {
                    XmlElement infDPECRegElemento = (XmlElement)infDPECRegNode;

                    if (infDPECRegElemento.GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0].InnerText == "124" ||
                        infDPECRegElemento.GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0].InnerText == "125") //DPEC Homologado
                    {
                        //string cChaveNFe = infDPECRegElemento.GetElementsByTagName(NFe.Components.TpcnResources.chNFe.ToString())[0].InnerText;
                        string dhRegDPEC = infDPECRegElemento.GetElementsByTagName("dhRegDPEC")[0].InnerText;
                        DateTime dtEmissaoDPEC = new DateTime(Convert.ToInt16(dhRegDPEC.Substring(0, 4)), Convert.ToInt16(dhRegDPEC.Substring(5, 2)), Convert.ToInt16(dhRegDPEC.Substring(8, 2)));

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        string arqEnvDpec = Empresas.Configuracoes[emp].PastaXmlEnvio + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.ConsDPEC_XML) + Propriedade.ExtEnvio.EnvDPEC_XML;
                        if (File.Exists(arqEnvDpec))
                        {
                            TFunctions.MoverArquivo(arqEnvDpec, PastaEnviados.Autorizados, dtEmissaoDPEC);
                        }

                        //Gravar o XML retornado pelo WebService do SEFAZ na pasta de autorizados. Wandrey 25/11/2010
                        string nomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + 
                                                  PastaEnviados.Autorizados.ToString() + "\\" + 
                                                  Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(dtEmissaoDPEC);
                        oGerarXML.XmlRetorno(Propriedade.ExtEnvio.ConsDPEC_XML, Propriedade.ExtRetorno.retConsDPEC_XML, vStrXmlRetorno, nomePastaEnviado);
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

        #region ConsDPEC()
        private void ConsDPEC(int emp, string arquivoXML)
        {
            this.dadosConsDPEC.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            this.dadosConsDPEC.tpEmis = (int)NFe.Components.TipoEmissao.teEPECeDPEC;

            ///
            /// danasa 21/10/2010
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                ///cUF|31                   |
                ///tpAmb|2                  | opcional
                ///verProc|1.0.0
                ///CNPJ|10238568000360
                ///IE|148230665114
                ///------
                ///chNFe|31101010238568000360550010000001011000001011
                ///CNPJCPF|05481336000137   | se UF=EX->Branco
                ///UF|SP
                ///vNF|123456.00
                ///vICMS|18.00
                ///vST|121.99
                List<string> cLinhas = Functions.LerArquivo(arquivoXML);
                Functions.PopulateClasse(this.dadosConsDPEC, cLinhas);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arquivoXML);

                XmlNodeList consDPECList = doc.GetElementsByTagName("consDPEC");

                foreach (XmlNode consDPECNode in consDPECList)
                {
                    XmlElement consDPECElemento = (XmlElement)consDPECNode;

                    this.dadosConsDPEC.tpAmb = Convert.ToInt32("0" + consDPECElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpAmb.ToString())[0].InnerText);
                }
            }
        }
        #endregion
    }
}

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
    /// <summary>
    /// Consultar situação do MDFe
    /// </summary>
    public class TaskMDFeConsultaSituacao : TaskAbst
    {
        public TaskMDFeConsultaSituacao()
        {
            Servico = Servicos.MDFePedidoConsultaSituacao;
        }

        #region Classe com os dados do XML da pedido de consulta da situação da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta da situação da nota
        /// </summary>
        private DadosPedSit dadosPedSit;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedSit = new DadosPedSit();
                //Ler o XML para pegar parâmetros de envio
                PedSit(emp, dadosPedSit);// NomeArquivoXML);

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedSit.cUF, dadosPedSit.tpAmb, dadosPedSit.tpEmis);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                object oConsulta = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);//NomeClasseWS(Servico, dadosPedSit.cUF));
                object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedSit.cUF, Servico));

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.cUF.ToString(), dadosPedSit.cUF.ToString());
                wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), NFe.ConvertTxt.versoes.VersaoXMLMDFePedSit);

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy,
                                    oConsulta,
                                    wsProxy.NomeMetodoWS[0],//NomeMetodoWS(Servico, dadosPedSit.cUF), 
                                    oCabecMsg, this);

                //Efetuar a leitura do retorno da situação para ver se foi autorizada ou não
                //Na versão 1 não posso gerar o -procNfe, ou vou ter que tratar a estrutura do XML de acordo com a versão, a consulta na versão 1 é somente para obter o resultado mesmo.
                LerRetornoSitMDFe(dadosPedSit.chNFe);

                //Gerar o retorno para o ERP
                oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML, 
                                     Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).RetornoXML, this.vStrXmlRetorno);
            }
            catch (Exception ex)
            {
                try
                {
                    TFunctions.GravarArqErroServico(NomeArquivoXML, 
                                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML, 
                                                    Propriedade.ExtRetorno.Sit_ERR, ex);
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
        /// <param name="arquivoXML">Nome do XML a ser lido</param>
        /// <param name="emp">Código da empresa</param>
        /*
        private void PedSit(int emp, string arquivoXML)
        {
            this.dadosPedSit.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            this.dadosPedSit.chNFe = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList consSitNFeList = doc.GetElementsByTagName("consSitMDFe");

            foreach (XmlNode consSitNFeNode in consSitNFeList)
            {
                XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                dadosPedSit.tpAmb = Convert.ToInt32("0" + consSitNFeElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpAmb.ToString())[0].InnerText);
                dadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName(TpcnResources.chMDFe.ToString())[0].InnerText;

                if (consSitNFeElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString()).Count != 0)
                {
                    this.dadosPedSit.tpEmis = Convert.ToInt16(consSitNFeElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0].InnerText);
                    /// para que o validador não rejeite, excluo a tag <tpEmis>
                    doc.DocumentElement.RemoveChild(consSitNFeElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0]);
                    /// salvo o arquivo modificado
                    doc.Save(arquivoXML);
                }
            }
        }*/
        #endregion

        #region LerRetornoSitMDFe()
        /// <summary>
        /// Ler o retorno da consulta situação da nota fiscal e de acordo com o status ele trata as notas enviadas se ainda não foram tratadas
        /// </summary>
        /// <param name="ChaveMDFe">Chave da NFe que está sendo consultada</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/06/2010
        /// </remarks>
        private void LerRetornoSitMDFe(string ChaveMDFe)
        {
            int emp = Empresas.FindEmpresaByThread();

            oGerarXML.XmlDistEventoMDFe(emp, this.vStrXmlRetorno);


            LerXML oLerXml = new LerXML();
            MemoryStream msXml = Functions.StringXmlToStreamUTF8(this.vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            XmlDocument doc = new XmlDocument();
            doc.Load(msXml);

            XmlNodeList retConsSitList = doc.GetElementsByTagName("retConsSitMDFe");

            foreach (XmlNode retConsSitNode in retConsSitList)
            {
                XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                //Definir a chave da NFe a ser pesquisada
                string strChaveNFe = "MDFe" + ChaveMDFe;

                //Definir o nome do arquivo da NFe e seu caminho
                string strNomeArqNfe = oFluxoNfe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                if (string.IsNullOrEmpty(strNomeArqNfe))
                {

                    strNomeArqNfe = strChaveNFe.Substring(4) + Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML;
                }

                string strArquivoNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                #region CNPJ da chave não é de uma empresa Uninfe
                bool notDaEmpresa = (ChaveMDFe.Substring(6, 14) != Empresas.Configuracoes[emp].CNPJ ||
                                    ChaveMDFe.Substring(0, 2) != Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString());

                if (!File.Exists(strArquivoNFe) && notDaEmpresa)
                    return;
                #endregion

                //Pegar o status de retorno da NFe que está sendo consultada a situação
                var cStatCons = string.Empty;
                if (retConsSitElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0] != null)
                {
                    cStatCons = retConsSitElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
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
                    case "598":
                    case "599":
                    case "404":
                    case "402":
                    #endregion

                    #region Validação das regras de negócios da consulta a NF-e
                    case "252":
                    case "226":
                    case "494":
                    case "227":
                    case "253":
                        break;
                    #endregion

                    #region Nota fiscal rejeitada
                    case "217": //J-NFe não existe na base de dados do SEFAZ
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
                                string strStat = Functions.LerTag(infConsSitElemento, NFe.Components.TpcnResources.cStat.ToString()).Replace(";", "");

                                switch (strStat)
                                {
                                    case "100":
                                        var strProtNfe = retConsSitElemento.GetElementsByTagName("protMDFe")[0].OuterXml;

                                        //Definir o nome do arquivo -procNfe.xml                                               
                                        string strArquivoNFeProc = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                                    PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                    Functions.ExtrairNomeArq(strArquivoNFe, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML) + Propriedade.ExtRetorno.ProcMDFe;

                                        //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                        //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                        if (File.Exists(strArquivoNFe))
                                        {
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                            oLerXml.Mdfe(strArquivoNFe);

                                            //Verificar se a -nfe.xml existe na pasta de autorizados
                                            bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML);

                                            //Verificar se o -procNfe.xml existe na past de autorizados
                                            bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.ExtRetorno.ProcMDFe);

                                            //Se o XML de distribuição não estiver na pasta de autorizados
                                            if (!procNFeJaNaAutorizada)
                                            {
                                                if (!File.Exists(strArquivoNFeProc))
                                                {
                                                    oGerarXML.XmlDistMDFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcMDFe);
                                                }
                                            }

                                            //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                            if (!(procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.ExtRetorno.ProcMDFe)))
                                            {
                                                //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                //Atualizar a situação para que eu só mova o arquivo com final -NFe.xml para a pasta autorizado se 
                                                //a procnfe já estiver lá, ou vai ficar na pasta emProcessamento para tentar gerar novamente.
                                                //Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.ExtRetorno.ProcMDFe);
                                            }

                                            //Se a NFe não existir ainda na pasta de autorizados
                                            if (!(NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML)))
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
                                            {
                                                string strArquivoDist = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                                            PastaEnviados.Autorizados.ToString() + "\\" +
                                                                            Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi) +
                                                                            Functions.ExtrairNomeArq(strArquivoNFe, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML) + 
                                                                            Propriedade.ExtRetorno.ProcMDFe;
                                                try
                                                {
                                                    TFunctions.ExecutaUniDanfe(strArquivoDist, oLerXml.oDadosNfe.dEmi, Empresas.Configuracoes[emp]);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Auxiliar.WriteLog("TaskMDFeConsultaSituacao: " + ex.Message, false);
                                                }
                                            }
                                        }

                                        if (File.Exists(strArquivoNFeProc))
                                        {
                                            //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                            Functions.DeletarArquivo(strArquivoNFeProc);
                                        }

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
        #endregion
    }
}
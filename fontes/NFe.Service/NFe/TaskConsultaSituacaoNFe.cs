using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    /// <summary>
    /// Consultar situação da NFe
    /// </summary>
    public class TaskNFeConsultaSituacao : TaskAbst
    {
        public TaskNFeConsultaSituacao(string arquivo)
        {
            Servico = Servicos.NFePedidoConsultaSituacao;
            NomeArquivoXML = arquivo;
            if (vXmlNfeDadosMsgEhXML)
            {
                ConteudoXML.PreserveWhitespace = false;
                ConteudoXML.Load(arquivo);
            }
        }

        #region Classe com os dados do XML da pedido de consulta da situação da NFe

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta da situação da nota
        /// </summary>
        private DadosPedSit dadosPedSit;

        #endregion Classe com os dados do XML da pedido de consulta da situação da NFe

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedSit = new DadosPedSit();
                PedSit(emp, dadosPedSit);

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosPedSit.cUF, dadosPedSit.tpAmb, dadosPedSit.tpEmis, dadosPedSit.versao, dadosPedSit.mod, 0);
                    System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedSit.cUF, dadosPedSit.tpAmb, dadosPedSit.tpEmis, Servico);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oConsulta = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                    object oCabecMsg = null;
                    if (dadosPedSit.versao != "4.00")
                    {
                        oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedSit.cUF, Servico));
                        wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), dadosPedSit.cUF.ToString());
                        wsProxy.SetProp(oCabecMsg, TpcnResources.versaoDados.ToString(), dadosPedSit.versao);
                    }

                    new AssinaturaDigital().CarregarPIN(emp, NomeArquivoXML, Servico);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oConsulta, wsProxy.NomeMetodoWS[0], oCabecMsg, this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).RetornoXML,
                                        false,
                                        securityProtocolType);

                    //Efetuar a leitura do retorno da situação para ver se foi autorizada ou não
                    //Na versão 1 não posso gerar o -procNfe, ou vou ter que tratar a estrutura do XML de acordo com a versão, a consulta na versão 1 é somente para obter o resultado mesmo.
                    LerRetornoSitNFe(dadosPedSit.chNFe);

                    //Gerar o retorno para o ERP
                    oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML,
                                         Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).RetornoXML, this.vStrXmlRetorno);
                }
                else
                {
                    string f = Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml";

                    if (NomeArquivoXML.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        f = Path.Combine(Empresas.Configuracoes[emp].PastaValidar, f);
                    }
                    oGerarXML.Consulta(TipoAplicativo.Nfe, f,
                        dadosPedSit.tpAmb,
                        dadosPedSit.tpEmis,
                        dadosPedSit.chNFe,
                        dadosPedSit.versao);
                }
            }
            catch (Exception ex)
            {
                string ExtEnvio = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML;
                else //Se for TXT
                    ExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioTXT;

                try
                {
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtEnvio, Propriedade.ExtRetorno.Sit_ERR, ex);
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

        #endregion Execute

        #region PedSit()

        /// <summary>
        /// Faz a leitura do XML de pedido de consulta da situação da NFe
        /// </summary>
        /// <param name="cArquivoXML">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        ///
        protected override void PedSit(int emp, DadosPedSit dadosPedSit)
        {
            dadosPedSit.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            dadosPedSit.chNFe = string.Empty;
            dadosPedSit.tpEmis = Empresas.Configuracoes[emp].tpEmis;
            dadosPedSit.versao = "";

            if (Path.GetExtension(this.NomeArquivoXML).ToLower() == ".txt")
            {
                //      tpAmb|2
                //      tpEmis|1                <<< opcional >>>
                //      chNFe|35080600000000000000550000000000010000000000
                //      versao|3.10
                List<string> cLinhas = Functions.LerArquivo(this.NomeArquivoXML);
                Functions.PopulateClasse(dadosPedSit, cLinhas);
                if (dadosPedSit.versao == "2.00")
                    dadosPedSit.versao = "2.01";
            }
            else
            {
                XmlNodeList consSitNFeList = ConteudoXML.GetElementsByTagName("consSitNFe");

                foreach (XmlNode consSitNFeNode in consSitNFeList)
                {
                    XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                    dadosPedSit.tpAmb = Convert.ToInt32("0" + consSitNFeElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                    dadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName(TpcnResources.chNFe.ToString())[0].InnerText;
                    dadosPedSit.versao = consSitNFeElemento.Attributes[TpcnResources.versao.ToString()].Value;
                    dadosPedSit.mod = dadosPedSit.chNFe.Substring(20, 2);

                    //Se alguém ainda gerar com a versão 2.00 vou mudar para a "2.01" para facilitar para o usuário do UniNFe
                    if (dadosPedSit.versao == "2.00")
                    {
                        consSitNFeElemento.Attributes[TpcnResources.versao.ToString()].InnerText = dadosPedSit.versao = "2.01";
                    }

                    if (consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                    {
                        this.dadosPedSit.tpEmis = Convert.ToInt16(consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        ConteudoXML.DocumentElement.RemoveChild(consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0]);
                        /// salvo o arquivo modificado
                    }
                }
            }

            if (this.dadosPedSit.versao == "")
                throw new Exception(NFeStrConstants.versaoError);
        }

        #endregion PedSit()

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
            int emp = Empresas.FindEmpresaByThread();

            oGerarXML.XmlDistEvento(emp, vStrXmlRetorno);

            LerXML oLerXml = new LerXML();
            FluxoNfe oFluxoNfe = new FluxoNfe();

            XmlDocument doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

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
                    strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML;
                }
                string strArquivoNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                #region CNPJ da chave não é de uma empresa Uninfe

                bool notDaEmpresa = (ChaveNFe.Substring(6, 14) != Empresas.Configuracoes[emp].CNPJ ||
                                    ChaveNFe.Substring(0, 2) != Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString());

                if (!File.Exists(strArquivoNFe))
                {
                    if (notDaEmpresa)
                        return;
                }

                #endregion CNPJ da chave não é de uma empresa Uninfe

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

                    #endregion Validação do Certificado de Transmissão

                    #region Validação Inicial da Mensagem no WebService

                    case "214":
                    case "243":
                    case "108":
                    case "109":

                    #endregion Validação Inicial da Mensagem no WebService

                    #region Validação das informações de controle da chamada ao WebService

                    case "242":
                    case "409":
                    case "410":
                    case "411":
                    case "238":
                    case "239":

                    #endregion Validação das informações de controle da chamada ao WebService

                    #region Validação da forma da área de dados

                    case "215":
                    case "516":
                    case "517":
                    case "545":
                    case "587":
                    case "588":
                    case "404":
                    case "402":

                    #endregion Validação da forma da área de dados

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

                    #endregion Validação das regras de negócios da consulta a NF-e

                    #region Nota fiscal rejeitada

                    case "217": //J-NFe não existe na base de dados do SEFAZ
                        goto case "TirarFluxo";

                    case "562": //J-Verificar se o campo "Código Numérico" informado na chave de acesso é diferente do existente no BD
                        goto case "TirarFluxo";

                    case "561": //J-Verificar se campo MM (mês) informado na Chave de Acesso é diferente do existente no BD
                        goto case "TirarFluxo";

                    #endregion Nota fiscal rejeitada

                    #endregion Rejeições do XML de consulta da situação da NFe (Não é a nfe que foi rejeitada e sim o XML de consulta da situação da nfe)

                    #region Nota fiscal autorizada

                    case "100": //Autorizado o uso da NFe
                    case "150": //Autorizado o uso da NFe fora do prazo
                        XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                        if (infConsSitList != null)
                        {
                            foreach (XmlNode infConsSitNode in infConsSitList)
                            {
                                XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                //Pegar o Status do Retorno da consulta situação
                                string strStat = Functions.LerTag(infConsSitElemento, TpcnResources.cStat.ToString(), false);

                                //Pegar a versão do XML
                                var protNFeElemento = (XmlElement)retConsSitElemento.GetElementsByTagName("protNFe")[0];
                                string versao = protNFeElemento.GetAttribute(TpcnResources.versao.ToString());

                                switch (strStat)
                                {
                                    case "100": //NFe Autorizada
                                    case "150": //NFe Autorizada fora do prazo
                                        string strProtNfe = retConsSitElemento.GetElementsByTagName("protNFe")[0].OuterXml;

                                        //Definir o nome do arquivo -procNfe.xml
                                        string strArquivoNFeProc = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                            Functions.ExtrairNomeArq(strArquivoNFe, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML) + Propriedade.ExtRetorno.ProcNFe;

                                        //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                        //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                        if (File.Exists(strArquivoNFe))
                                        {
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                            XmlDocument conteudoXML = new XmlDocument();
                                            conteudoXML.Load(strArquivoNFe);
                                            oLerXml.Nfe(conteudoXML);

                                            //Verificar se a -nfe.xml existe na pasta de autorizados
                                            bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML);

                                            //Verificar se o -procNfe.xml existe na past de autorizados
                                            bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, Propriedade.ExtRetorno.ProcNFe);

                                            //Se o XML de distribuição não estiver na pasta de autorizados
                                            if (!procNFeJaNaAutorizada)
                                            {
                                                if (!File.Exists(strArquivoNFeProc))
                                                {
                                                    Auxiliar.WriteLog("TaskNFeConsultaSituacao: Gerou o arquivo de distribuição através da consulta situação da NFe.", false);
                                                    oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcNFe, versao);
                                                }
                                            }

                                            //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                            if (!(procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, Propriedade.ExtRetorno.ProcNFe)))
                                            {
                                                //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                //Atualizar a situação para que eu só mova o arquivo com final -NFe.xml para a pasta autorizado se
                                                //a procnfe já estiver lá, ou vai ficar na pasta emProcessamento para tentar gerar novamente.
                                                //Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, Propriedade.ExtRetorno.ProcNFe);
                                            }

                                            //Se a NFe não existir ainda na pasta de autorizados
                                            if (!(NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML)))
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
                                                try
                                                {
                                                    string strArquivoDist = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                                                PastaEnviados.Autorizados.ToString() + "\\" +
                                                                                Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi) +
                                                                                Path.GetFileName(strArquivoNFe);

                                                    TFunctions.ExecutaUniDanfe(strArquivoDist, oLerXml.oDadosNfe.dEmi, Empresas.Configuracoes[emp]);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Auxiliar.WriteLog("TaskNFeConsultaSituacao:  (Falha na execução do UniDANFe) " + ex.Message, false);
                                                }
                                            }
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
                                    case "303":
                                        if (File.Exists(strArquivoNFe))
                                        {
                                            ///
                                            /// se o ERP copiou o arquivo da NFe para a pasta em Processamento, o Uninfe irá montar o XML de distribuicao, caso nao exista,
                                            /// e imprimir o DANFE
                                            ///
                                            ProcessaNFeDenegada(emp, oLerXml, strArquivoNFe, null, protNFeElemento.OuterXml, versao);
                                        }
                                        //ProcessaNFeDenegada(emp, oLerXml, strArquivoNFe, retConsSitElemento.GetElementsByTagName("protNFe")[0].OuterXml, versao);
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

                    #endregion Nota fiscal autorizada

                    #region Nota fiscal cancelada

                    case "101": //Cancelamento Homologado ou Nfe Cancelada
                        goto case "100";

                    #endregion Nota fiscal cancelada

                    #region Nota fiscal Denegada

                    case "110": //NFe Denegada
                        goto case "100";

                    case "301": //NFe Denegada
                        goto case "100";

                    case "302": //NFe Denegada
                        goto case "100";

                    case "205": //Nfe já está denegada na base do SEFAZ
                        goto case "100";

                    #endregion Nota fiscal Denegada

                    #region Conteúdo para retirar a nota fiscal do fluxo

                    case "TirarFluxo":
                        //Mover o XML da NFE a pasta de XML´s com erro
                        oAux.MoveArqErro(strArquivoNFe);

                        //Deletar a NFE do arquivo de controle de fluxo
                        oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                        break;

                    #endregion Conteúdo para retirar a nota fiscal do fluxo

                    default:
                        goto case "TirarFluxo";
                }
            }
        }

        #endregion LerRetornoSitNFe()
    }
}
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.CTe;

namespace NFe.Service
{
    /// <summary>
    /// Consultar situação do CTe
    /// </summary>
    public class TaskCTeConsultaSituacao: TaskAbst
    {
        public TaskCTeConsultaSituacao(string arquivo)
        {
            Servico = Servicos.CTePedidoConsultaSituacao;

            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
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
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedSit = new DadosPedSit();
                PedSit(emp, dadosPedSit);

                var xml = new ConsSitCTe();
                xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<ConsSitCTe>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = (dadosPedSit.chNFe.Substring(20, 2) == "67" ? TipoDFe.CTeOS : TipoDFe.CTe),
                    TipoEmissao = (Unimake.Business.DFe.Servicos.TipoEmissao)dadosPedSit.tpEmis,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                if(ConfiguracaoApp.Proxy)
                {
                    configuracao.HasProxy = true;
                    configuracao.ProxyAutoDetect = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
                    configuracao.ProxyUser = ConfiguracaoApp.ProxyUsuario;
                    configuracao.ProxyPassword = ConfiguracaoApp.ProxySenha;
                }

                if(dadosPedSit.chNFe.Substring(20, 2) == "67")
                {
                    var consultaProtocolo = new Unimake.Business.DFe.Servicos.CTeOS.ConsultaProtocolo(xml, configuracao);
                    consultaProtocolo.Executar();

                    vStrXmlRetorno = consultaProtocolo.RetornoWSString;
                }
                else
                {
                    var consultaProtocolo = new Unimake.Business.DFe.Servicos.CTe.ConsultaProtocolo(xml, configuracao);
                    consultaProtocolo.Executar();

                    vStrXmlRetorno = consultaProtocolo.RetornoWSString;
                }

                LerRetornoSitCTe(dadosPedSit.chNFe);

                XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).RetornoXML);
            }
            catch(Exception ex)
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

        #endregion Execute

        #region LerRetornoSitCTe()

        /// <summary>
        /// Ler o retorno da consulta situação da nota fiscal e de acordo com o status ele trata as notas enviadas se ainda não foram tratadas
        /// </summary>
        /// <param name="ChaveCTe">Chave da NFe que está sendo consultada</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/06/2010
        /// </remarks>
        private void LerRetornoSitCTe(string ChaveCTe)
        {
            var emp = Empresas.FindEmpresaByThread();

            oGerarXML.XmlDistEventoCTe(emp, vStrXmlRetorno);

            var oLerXml = new LerXML();
            var oFluxoNfe = new FluxoNfe();

            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            var retConsSitList = doc.GetElementsByTagName("retConsSitCTe");

            foreach(XmlNode retConsSitNode in retConsSitList)
            {
                var retConsSitElemento = (XmlElement)retConsSitNode;

                //Definir a chave da NFe a ser pesquisada
                var strChaveCTe = "CTe" + ChaveCTe;

                //Definir o nome do arquivo da NFe e seu caminho
                var strNomeArqCTe = oFluxoNfe.LerTag(strChaveCTe, FluxoNfe.ElementoFixo.ArqNFe);

                if(string.IsNullOrEmpty(strNomeArqCTe))
                {
                    strNomeArqCTe = strChaveCTe.Substring(3) + Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML;
                }
                var strArquivoCTe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqCTe;

                #region CNPJ da chave não é de uma empresa Uninfe

                var notDaEmpresa = (ChaveCTe.Substring(6, 14) != Empresas.Configuracoes[emp].CNPJ ||
                                    ChaveCTe.Substring(0, 2) != Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString());

                if(!File.Exists(strArquivoCTe))
                {
                    if(notDaEmpresa)
                    {
                        return;
                    }

                    var arquivos = Directory.GetFiles(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString(), "*-cte.*");

                    foreach(var arquivo in arquivos)
                    {
                        var arqXML = new XmlDocument();
                        arqXML.Load(arquivo);

                        var chave = ((XmlElement)arqXML.GetElementsByTagName("infCte")[0]).GetAttribute("Id").Substring(3);

                        if(chave.Equals(ChaveCTe))
                        {
                            strNomeArqCTe = Path.GetFileName(arquivo);
                            strArquivoCTe = arquivo;
                            break;
                        }
                    }
                }

                #endregion CNPJ da chave não é de uma empresa Uninfe

                //Pegar o status de retorno da NFe que está sendo consultada a situação
                var cStatCons = string.Empty;
                if(retConsSitElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0] != null)
                {
                    cStatCons = retConsSitElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                }

                switch(cStatCons)
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

                    #endregion Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)

                    #region Nota fiscal rejeitada

                    case "217": //J-NFe não existe na base de dados do SEFAZ
                        goto case "TirarFluxo";

                    #endregion Nota fiscal rejeitada

                    #region Nota fiscal autorizada

                    case "100": //Autorizado o uso da NFe
                    case "150":
                        var infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                        if(infConsSitList != null)
                        {
                            foreach(XmlNode infConsSitNode in infConsSitList)
                            {
                                var infConsSitElemento = (XmlElement)infConsSitNode;

                                //Pegar o Status do Retorno da consulta situação
                                var strStat = Functions.LerTag(infConsSitElemento, TpcnResources.cStat.ToString()).Replace(";", "");

                                switch(strStat)
                                {
                                    case "100":
                                    case "150":
                                        var strProtNfe = retConsSitElemento.GetElementsByTagName("protCTe")[0].OuterXml;

                                        //Definir o nome do arquivo -procNfe.xml
                                        var strArquivoNFeProc = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                            Functions.ExtrairNomeArq(strArquivoCTe, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML) +
                                            Propriedade.ExtRetorno.ProcCTe;

                                        //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                        //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                        if(File.Exists(strArquivoCTe))
                                        {
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                            var conteudoXML = new XmlDocument();
                                            conteudoXML.Load(strArquivoCTe);
                                            oLerXml.Cte(conteudoXML);

                                            if(Empresas.Configuracoes[emp].CompararDigestValueDFeRetornadoSEFAZ)
                                            {
                                                var digestValueConsultaSituacao = infConsSitElemento.GetElementsByTagName("digVal")[0].InnerText;
                                                var digestValueCTe = conteudoXML.GetElementsByTagName("DigestValue")[0].InnerText;

                                                if(!string.IsNullOrEmpty(digestValueConsultaSituacao) && !string.IsNullOrEmpty(digestValueCTe))
                                                {
                                                    if(!digestValueConsultaSituacao.Equals(digestValueCTe))
                                                    {
                                                        oAux.MoveArqErro(strArquivoCTe);
                                                        throw new Exception("O valor do DigestValue da consulta situação é diferente do DigestValue do CTe.");
                                                    }
                                                }
                                            }

                                            //Verificar se a -nfe.xml existe na pasta de autorizados
                                            var NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoCTe, oLerXml.oDadosNfe.dEmi,
                                                Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML,
                                                Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML);

                                            //Verificar se o -procNfe.xml existe na past de autorizados
                                            var procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoCTe, oLerXml.oDadosNfe.dEmi,
                                                Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML,
                                                Propriedade.ExtRetorno.ProcCTe);

                                            //Se o XML de distribuição não estiver na pasta em processamento
                                            if(!procNFeJaNaAutorizada && !File.Exists(strArquivoNFeProc))
                                            {
                                                if(ChaveCTe.Substring(20, 2) == "67")
                                                {
                                                    oGerarXML.XmlDistCTeOS(strArquivoCTe, strProtNfe, oLerXml.oDadosNfe.versao);
                                                }
                                                else
                                                {
                                                    oGerarXML.XmlDistCTe(strArquivoCTe, strProtNfe, oLerXml.oDadosNfe.versao);
                                                }
                                            }

                                            //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                            if(!(procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoCTe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML, Propriedade.ExtRetorno.ProcCTe)))
                                            {
                                                //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                            }

                                            //Se a NFe não existir ainda na pasta de autorizados
                                            if(!(NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoCTe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML)))
                                            {
                                                //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                if(!Empresas.Configuracoes[emp].SalvarSomenteXMLDistribuicao)
                                                {
                                                    TFunctions.MoverArquivo(strArquivoCTe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }
                                                else
                                                {
                                                    TFunctions.MoverArquivo(strArquivoCTe, PastaEnviados.Originais, oLerXml.oDadosNfe.dEmi);
                                                }
                                            }
                                            else
                                            {
                                                //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                Functions.DeletarArquivo(strArquivoCTe);
                                            }

                                            //Disparar a geração/impressao do UniDanfe. 03/02/2010 - Wandrey
                                            if(procNFeJaNaAutorizada)
                                            {
                                                try
                                                {
                                                    var strArquivoDist = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                        PastaEnviados.Autorizados.ToString() + "\\" +
                                                        Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi) +
                                                        Path.GetFileName(strArquivoNFeProc);
                                                    TFunctions.ExecutaUniDanfe(strArquivoDist, oLerXml.oDadosNfe.dEmi, Empresas.Configuracoes[emp]);
                                                }
                                                catch(Exception ex)
                                                {
                                                    Auxiliar.WriteLog("TaskCTeConsultaSituacao: " + ex.Message, false);
                                                }
                                            }
                                        }

                                        if(File.Exists(strArquivoNFeProc))
                                        {
                                            //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                            Functions.DeletarArquivo(strArquivoNFeProc);
                                        }

                                        break;

                                    case "301":
                                        //Ler o XML para pegar a data de emissão para criar a psta dos XML´s Denegados
                                        if(File.Exists(strArquivoCTe))
                                        {
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                            var conteudoXML = new XmlDocument();
                                            conteudoXML.Load(strArquivoCTe);
                                            oLerXml.Cte(conteudoXML);

                                            //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                                            if(!oAux.EstaDenegada(strArquivoCTe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML, Propriedade.ExtRetorno.Den))
                                            {
                                                TFunctions.MoverArquivo(strArquivoCTe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                                ///
                                                /// existe DACTE de CTe denegado???
                                                ///
                                                try
                                                {
                                                    var strArquivoDist = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                        PastaEnviados.Denegados.ToString() + "\\" +
                                                        Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi) +
                                                        Functions.ExtrairNomeArq(strArquivoCTe, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML) + Propriedade.ExtRetorno.Den;

                                                    TFunctions.ExecutaUniDanfe(strArquivoDist, oLerXml.oDadosNfe.dEmi, Empresas.Configuracoes[emp]);
                                                }
                                                catch(Exception ex)
                                                {
                                                    Auxiliar.WriteLog("TaskCTeConsultaSituacao: " + ex.Message, false);
                                                }
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
                                        oAux.MoveArqErro(strArquivoCTe);
                                        break;
                                }

                                //Deletar a NFE do arquivo de controle de fluxo
                                oFluxoNfe.ExcluirNfeFluxo(strChaveCTe);
                            }
                        }
                        break;

                    #endregion Nota fiscal autorizada

                    #region Nota fiscal cancelada

                    case "101": //Cancelamento Homologado ou Nfe Cancelada
                        oFluxoNfe.ExcluirNfeFluxo(strChaveCTe);
                        oAux.MoveArqErro(strArquivoCTe);
                        break;

                    #endregion Nota fiscal cancelada

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

                    #endregion Nota fiscal Denegada

                    #region Conteúdo para retirar a nota fiscal do fluxo

                    case "TirarFluxo":
                        //Mover o XML da NFE a pasta de XML´s com erro
                        oAux.MoveArqErro(strArquivoCTe);

                        //Deletar a NFE do arquivo de controle de fluxo
                        oFluxoNfe.ExcluirNfeFluxo(strChaveCTe);
                        break;

                    #endregion Conteúdo para retirar a nota fiscal do fluxo

                    default:
                        break;
                }
            }
        }

        #endregion LerRetornoSitCTe()
    }
}
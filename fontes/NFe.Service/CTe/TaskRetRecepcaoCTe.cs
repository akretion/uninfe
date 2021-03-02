using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.CTe;

namespace NFe.Service
{
    public class TaskCTeRetRecepcao: TaskAbst
    {
        public TaskCTeRetRecepcao(string arquivo)
        {
            Servico = Servicos.CTePedidoSituacaoLote;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public TaskCTeRetRecepcao(XmlDocument conteudoXML)
        {
            Servico = Servicos.CTePedidoSituacaoLote;
            ConteudoXML = conteudoXML;
            ConteudoXML.PreserveWhitespace = false;
            NomeArquivoXML = Empresas.Configuracoes[Empresas.FindEmpresaByThread()].PastaXmlEnvio + "\\temp\\" +
                conteudoXML.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText + Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML;
        }

        public TaskCTeRetRecepcao(XmlDocument conteudoXML, int emp)
        {
            Servico = Servicos.CTePedidoSituacaoLote;
            ConteudoXML = conteudoXML;
            ConteudoXML.PreserveWhitespace = false;
            NomeArquivoXML = Empresas.Configuracoes[emp].PastaXmlEnvio + "\\temp\\" +
                conteudoXML.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText + Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML;
        }

        #region Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado

        /// <summary>
        /// Esta Herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de consulta do recibo do lote de NFe enviado
        /// </summary>
        private DadosPedRecClass dadosPedRec;

        #endregion Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            Execute(emp);
        }

        public void Execute(int emp)
        {
            try
            {
                dadosPedRec = new DadosPedRecClass();
                PedRec(emp);

                var xml = new ConsReciCTe();
                xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<ConsReciCTe>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.CTe,
                    TipoEmissao = (Unimake.Business.DFe.Servicos.TipoEmissao)dadosPedRec.tpEmis,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                if(ConfiguracaoApp.Proxy)
                {
                    configuracao.HasProxy = true;
                    configuracao.ProxyAutoDetect = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
                    configuracao.ProxyUser = ConfiguracaoApp.ProxyUsuario;
                    configuracao.ProxyPassword = ConfiguracaoApp.ProxySenha;
                }

                var retAutorizacao = new Unimake.Business.DFe.Servicos.CTe.RetAutorizacao(xml, configuracao);
                retAutorizacao.Executar();

                vStrXmlRetorno = retAutorizacao.RetornoWSString;

                LerRetornoLoteCTe(emp);

                XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).RetornoXML);
            }
            catch(Exception ex)
            {
                try
                {
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML, Propriedade.ExtRetorno.ProRec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Pois ocorreu algum erro de rede, hd, permissão das pastas, etc. Wandrey 22/03/2010
                }
            }
            finally
            {
                //Deletar o arquivo de solicitação do serviço
                Functions.DeletarArquivo(NomeArquivoXML);
            }
        }

        #endregion Execute

        #region PedRec()

        /// <summary>
        /// Faz a leitura do XML de pedido da consulta do recibo do lote de notas enviadas
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        private void PedRec(int emp)
        {
            dadosPedRec.tpAmb = 0;
            dadosPedRec.tpEmis = Empresas.Configuracoes[emp].tpEmis;
            dadosPedRec.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            dadosPedRec.nRec = string.Empty;

            var consReciNFeList = ConteudoXML.GetElementsByTagName("consReciCTe");

            foreach(XmlNode consReciNFeNode in consReciNFeList)
            {
                var consReciNFeElemento = (XmlElement)consReciNFeNode;

                dadosPedRec.tpAmb = Convert.ToInt32("0" + consReciNFeElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                dadosPedRec.nRec = consReciNFeElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText;
                dadosPedRec.cUF = Convert.ToInt32(dadosPedRec.nRec.Substring(0, 2));
                dadosPedRec.versao = consReciNFeElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                if(consReciNFeElemento.GetElementsByTagName(TpcnResources.cUF.ToString()).Count != 0)
                {
                    dadosPedRec.cUF = Convert.ToInt32("0" + consReciNFeElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0].InnerText);
                    /// Para que o validador não rejeite, excluo a tag <cUF>
                    ConteudoXML.DocumentElement.RemoveChild(consReciNFeElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0]);
                }
                if(consReciNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                {
                    dadosPedRec.tpEmis = Convert.ToInt16(consReciNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0].InnerText);
                    /// Para que o validador não rejeite, excluo a tag <tpEmis>
                    ConteudoXML.DocumentElement.RemoveChild(consReciNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0]);
                }
            }
        }

        #endregion PedRec()

        #region LerRetornoLoteCTe()

        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do lote de notas fiscais e
        /// atualiza o arquivo de fluxo e envio de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private void LerRetornoLoteCTe(int emp)
        {
            var oLerXml = new LerXML();
            var fluxoNFe = new FluxoNfe();

            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            var retConsReciNFeList = doc.GetElementsByTagName("retConsReciCTe");

            foreach(XmlNode retConsReciNFeNode in retConsReciNFeList)
            {
                var retConsReciNFeElemento = (XmlElement)retConsReciNFeNode;

                //Pegar o número do recibo do lote enviado
                var nRec = string.Empty;
                if(retConsReciNFeElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0] != null)
                {
                    nRec = retConsReciNFeElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText;
                }

                //Pegar o status de retorno do lote enviado
                var cStatLote = string.Empty;
                if(retConsReciNFeElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0] != null)
                {
                    cStatLote = retConsReciNFeElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                }

                switch(cStatLote)
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
                    case "217": //E-Acesso BD CTE
                    case "216": //E-Verificar se campo "Codigo Numerico"
                        break;

                    #region Serviço paralisado

                    case "108":
                    case "109":
                        //Se o serviço estiver paralisado momentaneamente ou sem previsão de retorno, vamos tentar consultar somente a cada 3 minutos pra evitar consumo indevido.
                        if(nRec != string.Empty)
                        {
                            fluxoNFe.AtualizarDPedRec(nRec, DateTime.Now.AddSeconds(180));
                        }
                        break;

                    #endregion Serviço paralisado

                    #endregion Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)

                    #region Lote ainda está sendo processado

                    case "105": //E-Verifica se o lote não está na fila de resposta, mas está na fila de entrada (Lote em processamento)
                        //Ok vou aguardar o ERP gerar uma nova consulta para encerrar o fluxo da nota
                        break;

                    #endregion Lote ainda está sendo processado

                    #region Lote não foi localizado pelo recibo que está sendo consultado

                    case "106": //E-Verifica se o lote não está na fila de saída, nem na fila de entrada (Lote não encontrado)
                        //No caso do lote não encontrado através do recibo, o ERP vai ter que consultar a situação da NFe para encerrar ela
                        //Vou somente excluir ela do fluxo para não ficar consultando o recibo que não existe
                        if(nRec != string.Empty)
                        {
                            fluxoNFe.ExcluirNfeFluxoRec(nRec.Trim());
                        }
                        break;

                    #endregion Lote não foi localizado pelo recibo que está sendo consultado

                    #region Lote foi processado, agora tenho que tratar as notas fiscais dele

                    case "104": //Lote processado
                        var protNFeList = retConsReciNFeElemento.GetElementsByTagName("protCTe");

                        foreach(XmlNode protNFeNode in protNFeList)
                        {
                            var protNFeElemento = (XmlElement)protNFeNode;

                            var strProtNfe = protNFeElemento.OuterXml;

                            var infProtList = protNFeElemento.GetElementsByTagName("infProt");

                            foreach(XmlNode infProtNode in infProtList)
                            {
                                var infProtElemento = (XmlElement)infProtNode;

                                var strChaveNFe = string.Empty;
                                var strStat = string.Empty;

                                if(infProtElemento.GetElementsByTagName(TpcnResources.chCTe.ToString())[0] != null)
                                {
                                    strChaveNFe = "CTe" + infProtElemento.GetElementsByTagName(TpcnResources.chCTe.ToString())[0].InnerText;
                                }

                                if(infProtElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0] != null)
                                {
                                    strStat = infProtElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                                }

                                //Definir o nome do arquivo da NFe e seu caminho
                                var strNomeArqNfe = fluxoNFe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                                // se por algum motivo o XML não existir no "Fluxo", então o arquivo tem que existir
                                // na pasta "EmProcessamento" assinada.
                                if(string.IsNullOrEmpty(strNomeArqNfe))
                                {
                                    if(string.IsNullOrEmpty(strChaveNFe))
                                    {
                                        oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).RetornoXML, vStrXmlRetorno);
                                        throw new Exception("LerRetornoLoteCTe(): Não pode obter o nome do arquivo");
                                    }

                                    strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML;
                                }
                                var strArquivoNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                                //Atualizar a Tag de status da NFe no fluxo para que se ocorrer alguma falha na exclusão eu tenha esta campo para ter uma referencia em futuras consultas
                                fluxoNFe.AtualizarTag(strChaveNFe, FluxoNfe.ElementoEditavel.cStat, strStat);

                                //Atualizar a tag da data e hora da ultima consulta do recibo
                                fluxoNFe.AtualizarDPedRec(nRec, DateTime.Now);

                                switch(strStat)
                                {
                                    case "100": //NFe Autorizada
                                        if(File.Exists(strArquivoNFe))
                                        {
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                            var conteudoXMLCTe = new XmlDocument();
                                            conteudoXMLCTe.Load(strArquivoNFe);
                                            oLerXml.Cte(conteudoXMLCTe);

                                            //Juntar o protocolo com a NFE já copiando para a pasta em processamento
                                            var strArquivoNFeProc = oGerarXML.XmlDistCTe(strArquivoNFe, strProtNfe, oLerXml.oDadosNfe.versao);

                                            //Mover a cteProc da pasta de CTe em processamento para a NFe Autorizada
                                            //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procCTe.xml) para
                                            //depois mover o da nfe (-cte.xml), pois se ocorrer algum erro, tenho como reconstruir o senário,
                                            //assim sendo não inverta as posições. Wandrey 08/10/2009
                                            TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                            //Mover a CTe da pasta de CTe em processamento para CTe Autorizada
                                            //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procCTe.xml) para
                                            //depois mover o da nfe (-cte.xml), pois se ocorrer algum erro, tenho como reconstruir o cenário.
                                            //assim sendo não inverta as posições. Wandrey 08/10/2009
                                            if(!Empresas.Configuracoes[emp].SalvarSomenteXMLDistribuicao)
                                            {
                                                TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                            }
                                            else
                                            {
                                                TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Originais, oLerXml.oDadosNfe.dEmi);
                                            }

                                            //Disparar a geração/impressao do UniDanfe. 03/02/2010 - Wandrey
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
                                                Auxiliar.WriteLog("TaskCTeRetRecepcao: " + ex.Message, false);
                                            }
                                        }
                                        break;

                                    case "301": //NFe Denegada - Irregularidade fiscal do emitente
                                        if(File.Exists(strArquivoNFe))
                                        {
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                            var conteudoXMLCTe = new XmlDocument();
                                            conteudoXMLCTe.Load(strArquivoNFe);
                                            oLerXml.Cte(conteudoXMLCTe);

                                            //Mover a NFE da pasta de NFE em processamento para NFe Denegadas
                                            TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                            ///
                                            /// existe DACTE de CTe denegado???
                                            ///
                                            try
                                            {
                                                var strArquivoDist = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                                    PastaEnviados.Denegados.ToString() + "\\" +
                                                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi) +
                                                    Functions.ExtrairNomeArq(strArquivoNFe, Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML) + Propriedade.ExtRetorno.Den;

                                                TFunctions.ExecutaUniDanfe(strArquivoDist, oLerXml.oDadosNfe.dEmi, Empresas.Configuracoes[emp]);
                                            }
                                            catch(Exception ex)
                                            {
                                                Auxiliar.WriteLog("TaskCTeRetRecepcao: " + ex.Message, false);
                                            }
                                        }
                                        break;

                                    case "302": //NFe Denegada - Irregularidade fiscal do remetente
                                        goto case "301";

                                    case "303": //NFe Denegada - Irregularidade fiscal do destinatário
                                        goto case "301";

                                    case "304": //NFe Denegada - Irregularidade fiscal do expedidor
                                        goto case "301";

                                    case "305": //NFe Denegada - Irregularidade fiscal do recebedor
                                        goto case "301";

                                    case "306": //NFe Denegada - Irregularidade fiscal do tomador
                                        goto case "301";

                                    case "110": //NFe Denegada - Não sei quando ocorre este, mas descobrir ele no manual então estou incluindo. Wandrey 20/10/2009
                                        goto case "301";

                                    default: //NFe foi rejeitada
                                        //Mover o XML da NFE a pasta de XML´s com erro
                                        oAux.MoveArqErro(strArquivoNFe);
                                        break;
                                }

                                //Deletar a NFE do arquivo de controle de fluxo
                                fluxoNFe.ExcluirNfeFluxo(strChaveNFe);
                                break;
                            }
                        }
                        break;

                    #endregion Lote foi processado, agora tenho que tratar as notas fiscais dele

                    #region Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.

                    default:
                        //Qualquer outro tipo de rejeião vou tirar todas as notas do lote do fluxo, pois se o lote foi rejeitado, todas as notas fiscais também foram
                        //De acordo com o manual de integração se o status do lote não for 104, tudo foi rejeitado. Wandrey 20/07/2010

                        //Vou retirar as notas do fluxo pelo recibo
                        if(nRec != string.Empty)
                        {
                            fluxoNFe.ExcluirNfeFluxoRec(nRec.Trim());
                        }

                        break;

                        #endregion Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.
                }
            }
        }

        #endregion LerRetornoLoteCTe()
    }
}
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.MDFe;

namespace NFe.Service
{
    public class TaskMDFeRetRecepcao: TaskAbst
    {
        public string chMDFe { private get; set; } = null;

        public TaskMDFeRetRecepcao() => Servico = Servicos.MDFePedidoSituacaoLote;

        public TaskMDFeRetRecepcao(string arquivo)
        {
            Servico = Servicos.MDFePedidoSituacaoLote;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public TaskMDFeRetRecepcao(XmlDocument conteudoXML)
        {
            Servico = Servicos.MDFePedidoSituacaoLote;
            ConteudoXML = conteudoXML;
            ConteudoXML.PreserveWhitespace = false;
            NomeArquivoXML = Empresas.Configuracoes[Empresas.FindEmpresaByThread()].PastaXmlEnvio + "\\temp\\" +
                conteudoXML.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText + Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).EnvioXML;
        }

        public TaskMDFeRetRecepcao(XmlDocument conteudoXML, int emp)
        {
            Servico = Servicos.MDFePedidoSituacaoLote;
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

                var xml = new ConsReciMDFe();
                xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<ConsReciMDFe>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.MDFe,
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

                var retAutorizacao = new Unimake.Business.DFe.Servicos.MDFe.RetAutorizacao(xml, configuracao);
                retAutorizacao.Executar();

                vStrXmlRetorno = retAutorizacao.RetornoWSString;

                LerRetornoLoteMDFe(emp);

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
            dadosPedRec.nRec =
                dadosPedRec.versao = string.Empty; ;

            var consReciNFeList = ConteudoXML.GetElementsByTagName("consReciMDFe");

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

        #region LerRetornoLoteMDFe()

        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do lote de notas fiscais e
        /// atualiza o arquivo de fluxo e envio de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private void LerRetornoLoteMDFe(int emp)
        {
            var oLerXml = new LerXML();
            var fluxoNFe = new FluxoNfe();

            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            var retConsReciNFeList = doc.GetElementsByTagName("retConsReciMDFe");

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

                    #region Validação do certificado de transmissão

                    case "280":
                    case "281":
                    case "282":
                    case "283":
                    case "284":
                    case "285":
                    case "286":

                    #endregion Validação do certificado de transmissão

                    #region Validação inicial da mensagem no webservice

                    case "214":
                    case "243":

                    #endregion Validação inicial da mensagem no webservice

                    #region Validação das informações de controle da chamada ao webservice

                    case "242":
                    case "409":
                    case "410":
                    case "411":
                    case "238":
                    case "239":

                    #endregion Validação das informações de controle da chamada ao webservice

                    #region Validação da forma da área de dados

                    case "215":
                    case "598":
                    case "599":
                    case "404":
                    case "402":

                    #endregion Validação da forma da área de dados

                    #region Validação das regras de negócio da consulta recibo

                    case "252":
                    case "226":
                    case "247":
                    case "494":
                    case "227":
                    case "253":

                        #endregion Validação das regras de negócio da consulta recibo

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

                    #endregion Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)

                    #region Lote foi processado, agora tenho que tratar as notas fiscais dele

                    case "104": //Lote processado
                        // Atualizar a tag da data e hora da ultima consulta do recibo
                        fluxoNFe.AtualizarDPedRec(nRec, DateTime.Now);

                        FinalizarMDFe(retConsReciNFeElemento.GetElementsByTagName("protMDFe"), fluxoNFe, emp, null);
                        break;

                    #endregion Lote foi processado, agora tenho que tratar as notas fiscais dele

                    #region Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.

                    default:
                        //Qualquer outro tipo de rejeião vou tirar todas as notas do lote do fluxo, pois se o lote foi rejeitado, todas as notas fiscais também foram
                        //De acordo com o manual de integração se o status do lote não for 104, tudo foi rejeitado. Wandrey 20/07/2010
                        if(Convert.ToInt32(cStatLote) >= 1)
                        {
                            //Vou retirar as notas do fluxo pelo recibo
                            if(nRec != string.Empty)
                            {
                                fluxoNFe.ExcluirNfeFluxoRec(nRec.Trim());
                            }
                        }

                        break;

                        #endregion Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.
                }
            }
        }

        #endregion LerRetornoLoteMDFe()

        #region FinalizarMDFe()
        public void FinalizarMDFe(XmlNodeList protNFeList, FluxoNfe fluxoNFe, int emp, XmlDocument conteudoXMLLote)
        {
            var oLerXml = new LerXML();

            foreach(XmlNode protNFeNode in protNFeList)
            {
                var protNFeElemento = (XmlElement)protNFeNode;

                var strProtNfe = protNFeElemento.OuterXml;

                var infProtList = protNFeElemento.GetElementsByTagName("infProt");

                foreach(XmlNode infProtNode in infProtList)
                {
                    var tirarFluxo = true;
                    var infProtElemento = (XmlElement)infProtNode;

                    var strChaveNFe = string.Empty;
                    var strStat = string.Empty;

                    if(infProtElemento.GetElementsByTagName(TpcnResources.chMDFe.ToString())[0] != null)
                    {
                        strChaveNFe = "MDFe" + infProtElemento.GetElementsByTagName(TpcnResources.chMDFe.ToString())[0].InnerText;
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
                            throw new Exception("LerRetornoLoteMDFe(): Não pode obter o nome do arquivo");
                        }

                        strNomeArqNfe = strChaveNFe.Substring(4) + Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML;
                    }
                    var strArquivoNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                    //Atualizar a Tag de status da NFe no fluxo para que se ocorrer alguma falha na exclusão eu tenha esta campo para ter uma referencia em futuras consultas
                    fluxoNFe.AtualizarTag(strChaveNFe, FluxoNfe.ElementoEditavel.cStat, strStat);

                    switch(strStat)
                    {
                        case "100": //MDFe Autorizado
                            if(File.Exists(strArquivoNFe))
                            {
                                //Juntar o protocolo com a NFE já copiando para a pasta de autorizadas
                                var strArquivoNFeProc = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                    PastaEnviados.EmProcessamento.ToString() + "\\" +
                                    Functions.ExtrairNomeArq(strNomeArqNfe, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML) + Propriedade.ExtRetorno.ProcMDFe;

                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                if(conteudoXMLLote == null)
                                {
                                    conteudoXMLLote = new XmlDocument();
                                    conteudoXMLLote.Load(strArquivoNFe);
                                }
                                oLerXml.Mdfe(conteudoXMLLote);

                                //Verificar se a -nfe.xml existe na pasta de autorizados
                                var NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML);

                                //Verificar se o -procNfe.xml existe na past de autorizados
                                var procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.ExtRetorno.ProcMDFe);

                                //Se o XML de distribuição não estiver na pasta de autorizados
                                if(!procNFeJaNaAutorizada)
                                {
                                    if(!File.Exists(strArquivoNFeProc))
                                    {
                                        oGerarXML.XmlDistMDFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcMDFe, oLerXml.oDadosNfe.versao);
                                    }
                                }

                                if(!(procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.ExtRetorno.ProcMDFe)))
                                {
                                    //Mover a nfePRoc da pasta de NFE em processamento para a NFe Autorizada
                                    //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para
                                    //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário,
                                    //assim sendo não inverta as posições. Wandrey 08/10/2009
                                    TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                    //Atualizar a situação para que eu só mova o arquivo com final -NFe.xml para a pasta autorizado se
                                    //a procnfe já estiver lá, ou vai ficar na pasta emProcessamento para tentar gerar novamente.
                                    //Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                    procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.ExtRetorno.ProcMDFe);
                                }

                                if(!NFeJaNaAutorizada && procNFeJaNaAutorizada)
                                {
                                    //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                    //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para
                                    //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário.
                                    //assim sendo não inverta as posições. Wandrey 08/10/2009
                                    if(!Empresas.Configuracoes[emp].SalvarSomenteXMLDistribuicao)
                                    {
                                        TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                    }
                                    else
                                    {
                                        TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Originais, oLerXml.oDadosNfe.dEmi);
                                    }
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
                                        Auxiliar.WriteLog("TaskMDFeRetRecepcao: " + ex.Message, false);
                                    }
                                }
                                //Vou verificar se estão os dois arquivos na pasta Autorizados, se tiver eu tiro do fluxo caso contrário não. Wandrey 13/02/2012
                                NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML);
                                procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML, Propriedade.ExtRetorno.ProcMDFe);
                                if(!procNFeJaNaAutorizada || !NFeJaNaAutorizada)
                                {
                                    tirarFluxo = false;
                                }
                            }
                            break;

                        default: //NFe foi rejeitada
                                 //O Status da NFe tem que ser maior que 1 ou deu algum erro na hora de ler o XML de retorno da consulta do recibo, sendo assim, vou mantar a nota no fluxo para consultar novamente.
                            if(Convert.ToInt32(strStat) >= 1)
                            {
                                //Mover o XML da NFE a pasta de XML´s com erro
                                oAux.MoveArqErro(strArquivoNFe);
                            }
                            else
                            {
                                tirarFluxo = false;
                            }

                            break;
                    }

                    //Deletar a NFE do arquivo de controle de fluxo
                    if(tirarFluxo)
                    {
                        fluxoNFe.ExcluirNfeFluxo(strChaveNFe);
                    }

                    break;
                }
            }

        }
        #endregion
    }
}
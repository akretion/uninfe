using NFe.Components;
using NFe.Exceptions;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.CTeOS;

namespace NFe.Service
{
    public class TaskCTeRecepcaoOS: TaskAbst
    {
        private int NumeroLote;

        public TaskCTeRecepcaoOS(string arquivo)
        {
            Servico = Servicos.CteRecepcaoOS;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        #region Classe com os dados do XML do retorno do envio do Lote de NFe

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        private DadosRecClass dadosRec;

        #endregion Classe com os dados do XML do retorno do envio do Lote de NFe

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosRec = new DadosRecClass();
                var fluxoNfe = new FluxoNfe();

                var lerXml = new LerXML();
                lerXml.Cte(ConteudoXML);

                var idLote = lerXml.oDadosNfe.idLote;

                var xmlCTeOS = new CTeOS();
                xmlCTeOS = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<CTeOS>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.CTeOS,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                if(ConfiguracaoApp.Proxy)
                {
                    configuracao.HasProxy = true;
                    configuracao.ProxyAutoDetect = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
                    configuracao.ProxyUser = ConfiguracaoApp.ProxyUsuario;
                    configuracao.ProxyPassword = ConfiguracaoApp.ProxySenha;
                }

                var autorizacao = new Unimake.Business.DFe.Servicos.CTeOS.Autorizacao(xmlCTeOS, configuracao);

                NumeroLote = oGerarXML.GerarLoteCTeOS(NomeArquivoXML);

                autorizacao.Executar();

                ConteudoXML = autorizacao.ConteudoXMLAssinado;

                SalvarArquivoEmProcessamento(emp, lerXml.oDadosNfe.chavenfe);

                vStrXmlRetorno = autorizacao.RetornoWSString;

                #region Parte que trata o retorno do lote, ou seja, o número do recibo

                LerRetorno(emp);

                //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).RetornoXML,
                    vStrXmlRetorno,
                    Empresas.Configuracoes[emp].PastaXmlRetorno,
                    NumeroLote.ToString("000000000000000") + Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML);

                #endregion
            }
            catch(ExceptionEnvioXML ex)
            {
                //Ocorreu algum erro no exato momento em que tentou enviar o XML para o SEFAZ, vou ter que tratar
                //para ver se o XML chegou lá ou não, se eu consegui pegar o número do recibo de volta ou não, etc.
                //E ver se vamos tirar o XML do Fluxo ou finalizar ele com a consulta situação da NFe
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML, Propriedade.ExtRetorno.ProRec_ERR, ex, NumeroLote.ToString("000000000000000") + Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch(ExceptionSemInternet ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML, Propriedade.ExtRetorno.ProRec_ERR, ex, NumeroLote.ToString("000000000000000") + Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML, Propriedade.ExtRetorno.ProRec_ERR, ex, NumeroLote.ToString("000000000000000") + Propriedade.Extensao(Propriedade.TipoEnvio.CTeOS).EnvioXML);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
        }

        #endregion Execute

        /// <summary>
        /// Salvar o arquivo do CTeOS assinado na pasta EmProcessamento
        /// </summary>
        /// <param name="emp">Codigo da empresa</param>
        /// <param name="chaveCTeOS">Chave do CTeOS</param>
        private void SalvarArquivoEmProcessamento(int emp, string chaveCTeOS)
        {
            Empresas.Configuracoes[emp].CriarSubPastaEnviado();

            var nomeArqCTeOS = Path.GetFileName(NomeArquivoXML);
            var arqEmProcessamento = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + nomeArqCTeOS;

            var sw = File.CreateText(arqEmProcessamento);
            sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + ConteudoXML.GetElementsByTagName("CTeOS")[0].OuterXml);
            sw.Close();

            if(File.Exists(arqEmProcessamento))
            {
                File.Delete(NomeArquivoXML);

                NomeArquivoXML = arqEmProcessamento;
            }
        }

        private void LerRetorno(int emp)
        {
            /*
            vStrXmlRetorno = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<retCTeOS versao=\"3.00\" xmlns=\"http://www.portalfiscal.inf.br/cte\"><tpAmb>2</tpAmb><cUF>35</cUF><verAplic>SP-CTe-23-06-2017</verAplic><cStat>100</cStat><xMotivo>Autorizado o uso do CT-e</xMotivo><protCTe versao=\"3.00\"><infProt><tpAmb>2</tpAmb><verAplic>SP-CTe-23-06-2017</verAplic><chCTe>35170746014122000138670000000000061860795141</chCTe><dhRecbto>2017-07-21T09:52:19-03:00</dhRecbto><nProt>135170008578938</nProt><digVal>iYpkun2Ovm+sp+eMkzDtX6gtkzI=</digVal><cStat>100</cStat><xMotivo>Autorizado o uso do CT-e</xMotivo></infProt></protCTe></retCTeOS>";
            */

            /*
            vStrXmlRetorno = "<retCTeOS versao=\"3.00\" xmlns=\"http://www.portalfiscal.inf.br/cte\"><tpAmb>2</tpAmb><cUF>35</cUF><verAplic>SP-CTe-23-06-2017</verAplic><cStat>100</cStat><xMotivo>Autorizado o uso do CT-e</xMotivo><protCTe versao=\"3.00\"><infProt><tpAmb>2</tpAmb><verAplic>SP-CTe-23-06-2017</verAplic><chCTe>35170746014122000138670000000000261309301440</chCTe><dhRecbto>2017-07-26T11:47:48-03:00</dhRecbto><nProt>135170008595733</nProt><digVal>XTkEEwjNnoYasDYz/VJ7HuZVUEo=</digVal><cStat>100</cStat><xMotivo>Autorizado o uso do CT-e</xMotivo></infProt></protCTe></retCTeOS>";
            */

            var oLerXml = new LerXML();

            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            var retConsReciNFeList = doc.GetElementsByTagName("retCTeOS");

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
                    case "217": //E-Acesso BD CTE
                    case "216": //E-Verificar se campo "Codigo Numerico"
                        break;

                    #endregion Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)

                    #region Lote ainda está sendo processado

                    case "105": //E-Verifica se o lote não está na fila de resposta, mas está na fila de entrada (Lote em processamento)
                        //Ok vou aguardar o ERP gerar uma nova consulta para encerrar o fluxo da nota
                        break;

                    #endregion Lote ainda está sendo processado

                    #region Lote foi processado, agora tenho que tratar as notas fiscais dele

                    case "104": //Lote processado
                    case "100": //Processo sincrono já retorna como 100
                        var protNFeList = retConsReciNFeElemento.GetElementsByTagName("protCTe");

                        foreach(XmlNode protNFeNode in protNFeList)
                        {
                            var protNFeElemento = (XmlElement)protNFeNode;

                            var strProtNfe = protNFeElemento.OuterXml;

                            var infProtList = protNFeElemento.GetElementsByTagName("infProt");

                            foreach(XmlNode infProtNode in infProtList)
                            {
                                var infProtElemento = (XmlElement)infProtNode;

                                var strStat = string.Empty;

                                if(infProtElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0] != null)
                                {
                                    strStat = infProtElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                                }

                                //Definir o nome do arquivo da NFe e seu caminho
                                var strArquivoNFe = NomeArquivoXML;

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
                                            var strArquivoNFeProc = oGerarXML.XmlDistCTeOS(strArquivoNFe, strProtNfe, oLerXml.oDadosNfe.versao);

                                            //Mover a cteProc da pasta de CTe em processamento para a NFe Autorizada
                                            //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procCTe.xml) para
                                            //depois mover o da nfe (-cte.xml), pois se ocorrer algum erro, tenho como reconstruir o senário,
                                            //assim sendo não inverta as posições. Wandrey 08/10/2009
                                            TFunctions.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                            //Mover a CTe da pasta de CTe em processamento para CTe Autorizada
                                            //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procCTe.xml) para
                                            //depois mover o da nfe (-cte.xml), pois se ocorrer algum erro, tenho como reconstruir o cenário.
                                            //assim sendo não inverta as posições. Wandrey 08/10/2009
                                            TFunctions.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

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
                                                Auxiliar.WriteLog("TaskCTeRecepcaoOS: " + ex.Message, false);
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

                                break;
                            }
                        }
                        break;

                    #endregion Lote foi processado, agora tenho que tratar as notas fiscais dele

                    #region Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.

                    default:
                        //Qualquer outro tipo de rejeião vou tirar todas as notas do lote do fluxo, pois se o lote foi rejeitado, todas as notas fiscais também foram
                        //De acordo com o manual de integração se o status do lote não for 104, tudo foi rejeitado. Wandrey 20/07/2010
                        TFunctions.MoveArqErro(NomeArquivoXML);
                        break;

                        #endregion Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.
                }
            }
        }
    }
}
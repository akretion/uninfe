using NFe.Components;
using NFe.Settings;
using NFe.Validate;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.Service
{
    /// <summary>
    /// Classe para invocar os métodos e propriedades das classes dos webservices da NFE
    /// </summary>
    public class InvocarObjeto
    {
        #region Objetos

        private Auxiliar oAux = new Auxiliar();

        #endregion Objetos

        #region Métodos
        #region Invocar()

        /// <summary>
        /// Metodo responsável por invocar o serviço do WebService do SEFAZ
        /// </summary>
        /// <param name="wsProxy">Objeto da classe construida do WSDL</param>
        /// <param name="servicoWS">Objeto da classe de envio do XML</param>
        /// <param name="metodo">Método da classe de envio do XML que faz o envio</param>
        /// <param name="cabecMsg">Objeto da classe de cabecalho do serviço</param>
        /// <param name="servicoNFe">Objeto do Serviço de envio da NFE do UniNFe</param>
        /// <param name="finalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="finalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <param name="gravaRetorno">Grava o arquivo de retorno para o ERP na execução deste método?</param>
        public void Invocar(WebServiceProxy wsProxy,
                            object servicoWS,
                            string metodo,
                            object cabecMsg,
                            object servicoNFe,
                            string finalArqEnvio,
                            string finalArqRetorno,
                            bool gravaRetorno,
                            SecurityProtocolType securityProtocolType)
        {
            int emp = Empresas.FindEmpresaByThread();

            finalArqEnvio = Functions.ExtractExtension(finalArqEnvio);
            finalArqRetorno = Functions.ExtractExtension(finalArqRetorno);

            XmlDocument docXML = new XmlDocument();

            // Definir o tipo de serviço da NFe
            Type typeServicoNFe = servicoNFe.GetType();

            Servicos servico = (Servicos)wsProxy.GetProp(servicoNFe, NFeStrConstants.Servico);

            docXML = (XmlDocument)(typeServicoNFe.InvokeMember("ConteudoXML", System.Reflection.BindingFlags.GetField, null, servicoNFe, null));

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(typeServicoNFe.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.GetProperty, null, servicoNFe, null));

            // Exclui o Arquivo de Erro
            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(XmlNfeDadosMsg, finalArqEnvio + ".xml") + finalArqRetorno + ".err");

            if (docXML == null)
                docXML.Load(XmlNfeDadosMsg);

            // Validar o Arquivo XML
            switch (servico)
            {
                case Servicos.NFeEnviarLote:
                case Servicos.CTeEnviarLote:
                case Servicos.MDFeEnviarLote:
                case Servicos.MDFeEnviarLoteSinc:
                    //XML de NFe, CTe e MDFe, na montagem do lote eu valido o XML antes, como o lote quem monta é o XML entendo que não está montando errado, sendo assim, não vou validar novamente o XML para ganhar desempenho. Wandrey 18/09/2016
                    break;

                default:
                    ValidarXML validar = new ValidarXML(docXML, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                    string cResultadoValidacao = validar.ValidarArqXML(docXML, XmlNfeDadosMsg);
                    if (cResultadoValidacao != "")
                    {
                        throw new Exception(cResultadoValidacao);
                    }
                    break;
            }

            // Definir Proxy
            if (ConfiguracaoApp.Proxy)
            {
                wsProxy.SetProp(servicoWS, "Proxy", Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta, ConfiguracaoApp.DetectarConfiguracaoProxyAuto));
            }

            // Limpa a variável de retorno
            XmlNode XmlRetorno = null;
            string strRetorno = string.Empty;

            //Vou mudar o timeout para evitar que demore a resposta e o uninfe aborte antes de recebe-la. Wandrey 17/09/2009
            //Isso talvez evite de não conseguir o número do recibo se o serviço do SEFAZ estiver lento.
            wsProxy.SetProp(servicoWS, "Timeout", 60000);

            if (cabecMsg != null)
            {
                switch (servico)
                {
                    case Servicos.MDFePedidoConsultaSituacao:
                    case Servicos.MDFePedidoSituacaoLote:
                    case Servicos.MDFeEnviarLote:
                    case Servicos.MDFeEnviarLoteSinc:
                    case Servicos.MDFeConsultaStatusServico:
                    case Servicos.MDFeRecepcaoEvento:
                    case Servicos.MDFeConsultaNaoEncerrado:
                        wsProxy.SetProp(servicoWS, "mdfeCabecMsgValue", cabecMsg);
                        break;

                    case Servicos.CTeInutilizarNumeros:
                    case Servicos.CTePedidoConsultaSituacao:
                    case Servicos.CTePedidoSituacaoLote:
                    case Servicos.CTeEnviarLote:
                    case Servicos.CTeRecepcaoEvento:
                    case Servicos.CTeConsultaStatusServico:
                        if (wsProxy.GetProp(cabecMsg, TpcnResources.cUF.ToString()).ToString() == "50") //Mato Grosso do Sul fugiu o padrão nacional
                        {
                            try
                            {
                                wsProxy.SetProp2(servicoWS, "cteCabecMsg", cabecMsg);
                            }
                            catch //Se der erro é pq não está no ambiente normal então tem que ser o nome padrão pois Mato Grosso do Sul fugiu o padrão nacional.
                            {
                                wsProxy.SetProp(servicoWS, "cteCabecMsgValue", cabecMsg);
                            }
                        }
                        else
                        {
                            wsProxy.SetProp(servicoWS, "cteCabecMsgValue", cabecMsg);
                        }
                        break;

                    case Servicos.CteRecepcaoOS:
                        wsProxy.SetProp(servicoWS, "cteCabecMsgValue", cabecMsg);
                        break;

                    case Servicos.CTeDistribuicaoDFe:
                    case Servicos.DFeEnviar:
                        break;

                    case Servicos.LMCAutorizacao:
                        break;

                    default:
                        wsProxy.SetProp(servicoWS, "nfeCabecMsgValue", cabecMsg);
                        break;
                }
            }

            //Definir novamente o protocolo de segurança, pois é uma propriedade estática e o seu valor pode ser alterado antes do envio. Wandrey 03/05/2016
            ServicePointManager.SecurityProtocol = securityProtocolType;

            switch (servico)
            {
                case Servicos.ConsultarLoteReinf:
                    string reciboEFD = string.Empty;

                    if (docXML.GetElementsByTagName("numeroReciboFechamento")[0] != null)
                    {
                        reciboEFD = docXML.GetElementsByTagName("numeroReciboFechamento")[0].InnerText;
                    }
                    else
                    {
                        reciboEFD = docXML.GetElementsByTagName("numeroProtocoloFechamento")[0].InnerText;
                    }

                    XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                    {
                        Convert.ToByte(docXML.GetElementsByTagName("tipoInscricaoContribuinte")[0].InnerText),
                        docXML.GetElementsByTagName("numeroInscricaoContribuinte")[0].InnerText,
                        reciboEFD
                    });
                    break;

                case Servicos.ConsultasReinf:
                    if (docXML.GetElementsByTagName("tipoEvento")[0] == null)
                    {
                        reciboEFD = string.Empty;

                        if (docXML.GetElementsByTagName("numeroReciboFechamento")[0] != null)
                        {
                            reciboEFD = docXML.GetElementsByTagName("numeroReciboFechamento")[0].InnerText;
                        }
                        else
                        {
                            reciboEFD = docXML.GetElementsByTagName("numeroProtocoloFechamento")[0].InnerText;
                        }

                        XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                        {
                            Convert.ToByte(docXML.GetElementsByTagName("tipoInscricaoContribuinte")[0].InnerText),
                            docXML.GetElementsByTagName("numeroInscricaoContribuinte")[0].InnerText,
                            reciboEFD
                        });
                    }
                    else
                    {
                        switch (docXML.GetElementsByTagName("tipoEvento")[0].InnerText)
                        {
                            case "1000":
                            case "1070":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText
                                });
                                break;
                            case "2010":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscEstab")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscEstab")[0].InnerText,
                                    docXML.GetElementsByTagName("cnpjPrestador")[0].InnerText
                                });
                                break;
                            case "2020":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    docXML.GetElementsByTagName("nrInscEstabPrest")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscTomador")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscTomador")[0].InnerText
                                });
                                break;
                            case "2030":
                            case "2040":
                            case "2050":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    docXML.GetElementsByTagName("nrInscEstab")[0].InnerText
                                });
                                break;
                            case "2055":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscAdq")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscAdq")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscProd")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscProd")[0].InnerText
                                });
                                break;
                            case "2060":
                            case "4040":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscEstab")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscEstab")[0].InnerText
                                });
                                break;
                            case "4080":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscEstab")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscEstab")[0].InnerText,
                                    docXML.GetElementsByTagName("cnpjFontePagadora")[0].InnerText
                                });
                                break;
                            case "4010":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscEstab")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscEstab")[0].InnerText,
                                    docXML.GetElementsByTagName("cpfBeneficiario")[0].InnerText
                                });
                                break;
                            case "4020":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText,
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInscEstab")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInscEstab")[0].InnerText,
                                    docXML.GetElementsByTagName("cnpjBeneficiario")[0].InnerText
                                });
                                break;
                            case "2098":
                            case "2099":
                            case "4004":
                            case "4098":
                            case "4099":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("perApur")[0].InnerText
                                   });
                                break;
                            case "3010":
                                XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[]
                                {
                                    Convert.ToInt32(docXML.GetElementsByTagName("tipoEvento")[0].InnerText),
                                    Convert.ToByte(docXML.GetElementsByTagName("tpInsc")[0].InnerText),
                                    docXML.GetElementsByTagName("nrInsc")[0].InnerText,
                                    docXML.GetElementsByTagName("dtApur")[0].InnerText,
                                    docXML.GetElementsByTagName("nrInscEstabelecimento")[0].InnerText
                                   });
                                break;
                        }
                    }
                    break;

                case Servicos.ConsultarIdentificadoresEventoseSocial:
                case Servicos.DownloadEventoseSocial:
                    XmlRetorno = wsProxy.InvokeElement(servicoWS, metodo, new object[] { docXML.DocumentElement });
                    break;

                case Servicos.MDFeEnviarLoteSinc:
                    XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[] { TFunctions.CompressXML(docXML) });
                    break;

                default:
                    XmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[] { docXML });
                    break;
            }

            if (XmlRetorno == null)
                throw new Exception("Erro de envio da solicitação do serviço: " + servico.ToString());

            typeServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, servicoNFe, new object[] { XmlRetorno.OuterXml });

            // Registra o retorno de acordo com o status obtido
            if (gravaRetorno)
            {
                typeServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, servicoNFe, new Object[] { finalArqEnvio + ".xml", finalArqRetorno + ".xml" });
            }
        }

        #endregion Invocar()


        #region InvocarNFSe()

        /// <summary>
        /// Metodo responsável por invocar o serviço do WebService do SEFAZ
        /// </summary>
        /// <param name="wsProxy">Objeto da classe construida do WSDL</param>
        /// <param name="servicoWS">Objeto da classe de envio do XML</param>
        /// <param name="metodo">Método da classe de envio do XML que faz o envio</param>
        /// <param name="cabecMsg">Objeto da classe de cabecalho do serviço</param>
        /// <param name="servicoNFe">Objeto do Serviço de envio da NFE do UniNFe</param>
        /// <param name="finalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="finalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 17/03/2010
        /// </remarks>
        public void InvocarNFSe(WebServiceProxy wsProxy,
                            object servicoWS,
                            string metodo,
                            string cabecMsg,
                            object servicoNFe,
                            string finalArqEnvio,
                            string finalArqRetorno,
                            PadroesNFSe padraoNFSe,
                            Servicos servicoNFSe,
                            SecurityProtocolType securityProtocolType)
        {
            int emp = Empresas.FindEmpresaByThread();

            finalArqEnvio = Functions.ExtractExtension(finalArqEnvio);
            finalArqRetorno = Functions.ExtractExtension(finalArqRetorno);

            XmlDocument docXML = new XmlDocument();

            // Definir o tipo de serviço da NFe
            Type typeServicoNFe = servicoNFe.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(typeServicoNFe.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.GetProperty, null, servicoNFe, null));

            // Exclui o Arquivo de Erro
            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(XmlNfeDadosMsg, finalArqEnvio + ".xml") + finalArqRetorno + ".err");

            // Validar o Arquivo XML
            ValidarXML validar = new ValidarXML(XmlNfeDadosMsg, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
            string cResultadoValidacao = validar.ValidarArqXML(XmlNfeDadosMsg);
            if (cResultadoValidacao != "")
            {
                switch (padraoNFSe)
                {
                    case PadroesNFSe.SMARAPD:
                        break;

                    default:
                        throw new Exception(cResultadoValidacao);
                }
            }

            //Definir novamente o protocolo de segurança, pois é uma propriedade estática e o seu valor pode ser alterado antes do envio. Wandrey 03/05/2016
            ServicePointManager.SecurityProtocol = securityProtocolType;

            // Montar o XML de Lote de envio de Notas fiscais
            docXML.Load(XmlNfeDadosMsg);

            // Definir Proxy
            if (ConfiguracaoApp.Proxy && wsProxy != null)
            {
                switch (padraoNFSe)
                {
                    case PadroesNFSe.BETHA:
                        wsProxy.Betha.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta, ConfiguracaoApp.DetectarConfiguracaoProxyAuto);
                        break;

                    default:
                        wsProxy.SetProp(servicoWS, "Proxy", Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta, ConfiguracaoApp.DetectarConfiguracaoProxyAuto));
                        break;
                }
            }

            // Limpa a variável de retorno
            string strRetorno = string.Empty;

            //Vou mudar o timeout para evitar que demore a resposta e o uninfe aborte antes de recebe-la. Wandrey 17/09/2009
            //Isso talvez evite de não conseguir o número do recibo se o serviço do SEFAZ estiver lento.
            if (wsProxy != null)
            {
                switch (padraoNFSe)
                {
                    case PadroesNFSe.NOTAINTELIGENTE:
                    case PadroesNFSe.BETHA:
                        break;

                    default:
                        wsProxy.SetProp(servicoWS, "Timeout", 120000);
                        break;
                }
            }

            //Invocar o membro
            switch (padraoNFSe)
            {
                #region Padrão BETHA

                case PadroesNFSe.BETHA:
                    switch (metodo)
                    {
                        case "ConsultarSituacaoLoteRps":
                            strRetorno = wsProxy.Betha.ConsultarSituacaoLoteRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "ConsultarLoteRps":
                            strRetorno = wsProxy.Betha.ConsultarLoteRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "CancelarNfse":
                            strRetorno = wsProxy.Betha.CancelarNfse(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "ConsultarNfse":
                            strRetorno = wsProxy.Betha.ConsultarNfse(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "ConsultarNfsePorRps":
                            strRetorno = wsProxy.Betha.ConsultarNfsePorRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "RecepcionarLoteRps":
                            strRetorno = wsProxy.Betha.RecepcionarLoteRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;
                    }
                    break;

                #endregion Padrão BETHA

                #region NOTAINTELIGENTE

                case PadroesNFSe.NOTAINTELIGENTE:
                    //NFe.Components.PClaudioMG.api_portClient wsClaudio = (NFe.Components.PClaudioMG.api_portClient)servicoWS;

                    switch (servicoNFSe)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            //strRetorno = wsClaudio.RecepcionarLoteRps(docXML.OuterXml.ToString());
                            break;
                    }
                    //strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { docXML.OuterXml.ToString() });
                    break;

                #endregion NOTAINTELIGENTE

                #region Padrão ISSONLINE

                case PadroesNFSe.ISSONLINE_ASSESSORPUBLICO:
                    int operacao;
                    string senhaWs = Functions.GetMD5Hash(Empresas.Configuracoes[emp].SenhaWS);

                    switch (servicoNFSe)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            operacao = 1;
                            break;

                        case Servicos.NFSeCancelar:
                            operacao = 2;
                            break;

                        default:
                            operacao = 3;
                            break;
                    }

                    strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { Convert.ToSByte(operacao), Empresas.Configuracoes[emp].UsuarioWS, senhaWs, docXML.OuterXml });
                    break;

                #endregion Padrão ISSONLINE

                #region Padrão Paulistana

                case PadroesNFSe.PAULISTANA:
                    strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { 1, docXML.OuterXml });
                    break;

                #endregion Padrão Paulistana

                #region TECNOSISTEMAS

                case PadroesNFSe.TECNOSISTEMAS:
                    strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { docXML.OuterXml, cabecMsg.ToString() });
                    break;

                #endregion TECNOSISTEMAS

                #region SMARAPD

                case PadroesNFSe.SMARAPD:
                    if (metodo == "nfdEntradaCancelar")
                    {
                        strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { Empresas.Configuracoes[emp].UsuarioWS,
                            TFunctions.EncryptSHA1(Empresas.Configuracoes[emp].SenhaWS),
                            docXML.OuterXml });
                    }
                    else if (metodo == "nfdSaida")
                    {
                        strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { Empresas.Configuracoes[emp].UsuarioWS,
                            TFunctions.EncryptSHA1(Empresas.Configuracoes[emp].SenhaWS),
                            Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString(),
                            docXML.OuterXml });
                    }
                    else if (metodo == "urlNfd")
                    {
                        strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { Convert.ToInt32(docXML.GetElementsByTagName("codigoMunicipio")[0].InnerText),
                            Convert.ToInt32(docXML.GetElementsByTagName("numeroNfd")[0].InnerText),
                            Convert.ToInt32(docXML.GetElementsByTagName("serieNfd")[0].InnerText),
                            docXML.GetElementsByTagName("inscricaoMunicipal")[0].InnerText });
                    }
                    else
                    {
                        strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { Empresas.Configuracoes[emp].UsuarioWS,
                            TFunctions.EncryptSHA1(Empresas.Configuracoes[emp].SenhaWS),
                            Empresas.Configuracoes[emp].UnidadeFederativaCodigo,
                            docXML.OuterXml });
                    }
                    break;

                #endregion SMARAPD

                #region ISSWEB

                case PadroesNFSe.ISSWEB:
                    string versao = docXML.DocumentElement.GetElementsByTagName("Versao")[0].InnerText;
                    string cnpj = docXML.DocumentElement.GetElementsByTagName("CNPJCPFPrestador")[0].InnerText;
                    strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { cnpj, docXML.OuterXml, versao });
                    break;

                #endregion ISSWEB

                #region NA_INFORMATICA

                case PadroesNFSe.NA_INFORMATICA:
                    switch (servicoNFSe)
                    {
                        #region Recepcionar Lote RPS - Assíncrono

                        case Servicos.NFSeRecepcionarLoteRps:
                            //if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taProducao)
                            //{
                            //    ((Components.PCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.PCorumbaMS.RecepcionarLoteRps dadosEnvio = new Components.PCorumbaMS.RecepcionarLoteRps();
                            //    Components.PCorumbaMS.RecepcionarLoteRpsResponse dadosRetorno = new Components.PCorumbaMS.RecepcionarLoteRpsResponse();
                            //    dadosEnvio.EnviarLoteRpsEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.PCorumbaMS.NfseWSService)servicoWS).RecepcionarLoteRps(dadosEnvio);
                            //    strRetorno = dadosRetorno.EnviarLoteRpsResposta;
                            //}
                            //else
                            //{
                            //    ((Components.HCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.HCorumbaMS.RecepcionarLoteRps dadosEnvio = new Components.HCorumbaMS.RecepcionarLoteRps();
                            //    Components.HCorumbaMS.RecepcionarLoteRpsResponse dadosRetorno = new Components.HCorumbaMS.RecepcionarLoteRpsResponse();
                            //    dadosEnvio.EnviarLoteRpsEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.HCorumbaMS.NfseWSService)servicoWS).RecepcionarLoteRps(dadosEnvio);
                            //    strRetorno = dadosRetorno.EnviarLoteRpsResposta;
                            //}
                            break;

                        #endregion Recepcionar Lote RPS - Assíncrono

                        #region Recepcionar Lote RPS - Síncrono

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            //if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taProducao)
                            //{
                            //    ((Components.PCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.PCorumbaMS.RecepcionarLoteRpsSincrono dadosEnvio = new Components.PCorumbaMS.RecepcionarLoteRpsSincrono();
                            //    Components.PCorumbaMS.RecepcionarLoteRpsSincronoResponse dadosRetorno = new Components.PCorumbaMS.RecepcionarLoteRpsSincronoResponse();
                            //    dadosEnvio.EnviarLoteRpsSincronoEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.PCorumbaMS.NfseWSService)servicoWS).RecepcionarLoteRpsSincrono(dadosEnvio);
                            //    strRetorno = dadosRetorno.EnviarLoteRpsSincronoResposta;
                            //}
                            //else
                            //{
                            //    ((Components.HCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.HCorumbaMS.RecepcionarLoteRpsSincrono dadosEnvio = new Components.HCorumbaMS.RecepcionarLoteRpsSincrono();
                            //    Components.HCorumbaMS.RecepcionarLoteRpsSincronoResponse dadosRetorno = new Components.HCorumbaMS.RecepcionarLoteRpsSincronoResponse();
                            //    dadosEnvio.EnviarLoteRpsSincronoEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.HCorumbaMS.NfseWSService)servicoWS).RecepcionarLoteRpsSincrono(dadosEnvio);
                            //    strRetorno = dadosRetorno.EnviarLoteRpsSincronoResposta;
                            //}
                            break;

                        #endregion Recepcionar Lote RPS - Síncrono

                        #region Cancelar RPS

                        case Servicos.NFSeCancelar:
                            //if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taProducao)
                            //{
                            //    ((Components.PCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.PCorumbaMS.CancelarNfse dadosEnvio = new Components.PCorumbaMS.CancelarNfse();
                            //    Components.PCorumbaMS.CancelarNfseResponse dadosRetorno = new Components.PCorumbaMS.CancelarNfseResponse();
                            //    dadosEnvio.CancelarNfseEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.PCorumbaMS.NfseWSService)servicoWS).CancelarNfse(dadosEnvio);
                            //    strRetorno = dadosRetorno.CancelarNfseResposta;
                            //}
                            //else
                            //{
                            //    ((Components.HCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.HCorumbaMS.CancelarNfse dadosEnvio = new Components.HCorumbaMS.CancelarNfse();
                            //    Components.HCorumbaMS.CancelarNfseResponse dadosRetorno = new Components.HCorumbaMS.CancelarNfseResponse();
                            //    dadosEnvio.CancelarNfseEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.HCorumbaMS.NfseWSService)servicoWS).CancelarNfse(dadosEnvio);
                            //    strRetorno = dadosRetorno.CancelarNfseResposta;
                            //}
                            break;

                        #endregion Cancelar RPS

                        #region Consultar Lote RPS

                        case Servicos.NFSeConsultarLoteRps:
                            //if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taProducao)
                            //{
                            //    ((Components.PCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.PCorumbaMS.ConsultarLoteRps dadosEnvio = new Components.PCorumbaMS.ConsultarLoteRps();
                            //    Components.PCorumbaMS.ConsultarLoteRpsResponse dadosRetorno = new Components.PCorumbaMS.ConsultarLoteRpsResponse();
                            //    dadosEnvio.ConsultarLoteRpsEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.PCorumbaMS.NfseWSService)servicoWS).ConsultarLoteRps(dadosEnvio);
                            //    strRetorno = dadosRetorno.ConsultarLoteRpsResposta;
                            //}
                            //else
                            //{
                            //    ((Components.HCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.HCorumbaMS.ConsultarLoteRps dadosEnvio = new Components.HCorumbaMS.ConsultarLoteRps();
                            //    Components.HCorumbaMS.ConsultarLoteRpsResponse dadosRetorno = new Components.HCorumbaMS.ConsultarLoteRpsResponse();
                            //    dadosEnvio.ConsultarLoteRpsEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.HCorumbaMS.NfseWSService)servicoWS).ConsultarLoteRps(dadosEnvio);
                            //    strRetorno = dadosRetorno.ConsultarLoteRpsResposta;
                            //}
                            break;

                        #endregion Consultar Lote RPS

                        #region Consulta Situação Nfse

                        case Servicos.NFSeConsultar:
                            //if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taProducao)
                            //{
                            //    ((Components.PCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.PCorumbaMS.ConsultarNfsePorFaixa dadosEnvio = new Components.PCorumbaMS.ConsultarNfsePorFaixa();
                            //    Components.PCorumbaMS.ConsultarNfsePorFaixaResponse dadosRetorno = new Components.PCorumbaMS.ConsultarNfsePorFaixaResponse();
                            //    dadosEnvio.ConsultarNfsePorFaixaEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.PCorumbaMS.NfseWSService)servicoWS).ConsultarNfsePorFaixa(dadosEnvio);
                            //    strRetorno = dadosRetorno.ConsultarNfsePorFaixaResposta;
                            //}
                            //else
                            //{
                            //    ((Components.HCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.HCorumbaMS.ConsultarNfsePorFaixa dadosEnvio = new Components.HCorumbaMS.ConsultarNfsePorFaixa();
                            //    Components.HCorumbaMS.ConsultarNfsePorFaixaResponse dadosRetorno = new Components.HCorumbaMS.ConsultarNfsePorFaixaResponse();
                            //    dadosEnvio.ConsultarNfsePorFaixaEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.HCorumbaMS.NfseWSService)servicoWS).ConsultarNfsePorFaixa(dadosEnvio);
                            //    strRetorno = dadosRetorno.ConsultarNfsePorFaixaResposta;
                            //}
                            break;

                        #endregion Consulta Situação Nfse

                        #region Consulta Situação Nfse por RPS

                        case Servicos.NFSeConsultarPorRps:
                            //if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taProducao)
                            //{
                            //    ((Components.PCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.PCorumbaMS.ConsultarNfsePorRps dadosEnvio = new Components.PCorumbaMS.ConsultarNfsePorRps();
                            //    Components.PCorumbaMS.ConsultarNfsePorRpsResponse dadosRetorno = new Components.PCorumbaMS.ConsultarNfsePorRpsResponse();
                            //    dadosEnvio.ConsultarNfsePorRpsEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.PCorumbaMS.NfseWSService)servicoWS).ConsultarNfsePorRps(dadosEnvio);
                            //    strRetorno = dadosRetorno.ConsultarNfsePorRpsResposta;
                            //}
                            //else
                            //{
                            //    ((Components.HCorumbaMS.NfseWSService)servicoWS).ClientCertificates.Add(Empresas.Configuracoes[emp].X509Certificado);
                            //    Components.HCorumbaMS.ConsultarNfsePorRps dadosEnvio = new Components.HCorumbaMS.ConsultarNfsePorRps();
                            //    Components.HCorumbaMS.ConsultarNfsePorRpsResponse dadosRetorno = new Components.HCorumbaMS.ConsultarNfsePorRpsResponse();
                            //    dadosEnvio.ConsultarNfsePorRpsEnvio = docXML.OuterXml.ToString();
                            //    dadosRetorno = ((Components.HCorumbaMS.NfseWSService)servicoWS).ConsultarNfsePorRps(dadosEnvio);
                            //    strRetorno = dadosRetorno.ConsultarNfsePorRpsResposta;
                            //}
                            break;

                            #endregion Consulta Situação Nfse por RPS
                    }
                    break;

                #endregion NA_INFORMATICA

                #region ABASE

                case PadroesNFSe.ABASE:
                    if (servicoNFSe == Servicos.NFSeConsultarPorRps)
                        goto default;
                    else
                    {
                        XmlNode xmlRetorno = wsProxy.InvokeXML(servicoWS, metodo, new object[] { cabecMsg.ToString(), docXML.OuterXml });
                        strRetorno = xmlRetorno.OuterXml;
                    }
                    break;

                #endregion ABASE

                case PadroesNFSe.LEXSOM:
                    XmlNode result = wsProxy.InvokeXML(servicoWS, metodo, new object[] { docXML });
                    strRetorno = result.OuterXml;
                    break;

                #region Padrão Joinville_SC

                case PadroesNFSe.JOINVILLE_SC:
                    if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao)
                    {
                        switch (metodo)
                        {
                            case "RecepcionarLoteRps":
                                XmlNode joXmlAssinatura = docXML.GetElementsByTagName("Signature")[docXML.GetElementsByTagName("Signature").Count - 1];
                                strRetorno = SerializarObjeto((Components.HJoinvilleSC.EnviarLoteRpsResposta)wsProxy.Invoke(servicoWS, metodo, new object[] { docXML, joXmlAssinatura }));
                                break;

                            case "CancelarNfse":
                                strRetorno = SerializarObjeto((Components.HJoinvilleSC.CancelarNfseResposta)wsProxy.Invoke(servicoWS, metodo, new object[] { docXML }));
                                break;

                            case "ConsultarLoteRps":
                                strRetorno = SerializarObjeto((Components.HJoinvilleSC.ConsultarLoteRpsResposta)wsProxy.Invoke(
                                    servicoWS,
                                    metodo,
                                    new object[] {
                                        new Components.HJoinvilleSC.Prestador
                                        {
                                            CpfCnpj = new Components.HJoinvilleSC.CpfCnpj
                                            {
                                                Cnpj = (docXML.GetElementsByTagName("Cnpj")[0] != null ? docXML.GetElementsByTagName("Cnpj")[0].InnerText : ""),
                                                Cpf = (docXML.GetElementsByTagName("Cpf")[0] != null ? docXML.GetElementsByTagName("Cpf")[0].InnerText : "")
                                            },
                                            InscricaoMunicipal = (docXML.GetElementsByTagName("InscricaoMunicipal")[0] != null ? docXML.GetElementsByTagName("InscricaoMunicipal")[0].InnerText : "")
                                        },
                                        docXML.GetElementsByTagName("Protocolo")[0].InnerText }));
                                break;

                            case "ConsultarNfseRps":
                                strRetorno = SerializarObjeto((Components.HJoinvilleSC.ConsultarNfseRpsResposta)wsProxy.Invoke(
                                    servicoWS,
                                    metodo,
                                    new object[] {
                                        new Components.HJoinvilleSC.IdentificacaoRps
                                        {
                                            Numero = Convert.ToInt32((docXML.GetElementsByTagName("Numero")[0] != null ? docXML.GetElementsByTagName("Numero")[0].InnerText : "0")),
                                            Serie = (docXML.GetElementsByTagName("Serie")[0] != null ? docXML.GetElementsByTagName("Serie")[0].InnerText : ""),
                                            Tipo = Convert.ToInt32((docXML.GetElementsByTagName("Tipo")[0] != null ? docXML.GetElementsByTagName("Tipo")[0].InnerText : "0"))
                                        },
                                        new Components.HJoinvilleSC.Prestador
                                        {
                                            CpfCnpj = new Components.HJoinvilleSC.CpfCnpj
                                            {
                                                Cnpj = (docXML.GetElementsByTagName("Cnpj")[0] != null ? docXML.GetElementsByTagName("Cnpj")[0].InnerText : ""),
                                                Cpf = (docXML.GetElementsByTagName("Cpf")[0] != null ? docXML.GetElementsByTagName("Cpf")[0].InnerText : "")
                                            },
                                            InscricaoMunicipal = (docXML.GetElementsByTagName("InscricaoMunicipal")[0] != null ? docXML.GetElementsByTagName("InscricaoMunicipal")[0].InnerText : "")
                                        }
                                    }));
                                break;
                        }
                    }
                    else
                        switch (metodo)
                        {
                            case "RecepcionarLoteRps":
                                XmlNode joXmlAssinatura = docXML.GetElementsByTagName("Signature")[docXML.GetElementsByTagName("Signature").Count - 1];

                                strRetorno = SerializarObjeto((Components.PJoinvilleSC.EnviarLoteRpsResposta)wsProxy.Invoke(servicoWS, metodo, new object[] { docXML, joXmlAssinatura }));
                                break;

                            case "CancelarNfse":
                                strRetorno = SerializarObjeto((Components.PJoinvilleSC.CancelarNfseResposta)wsProxy.Invoke(servicoWS, metodo, new object[] { docXML }));
                                break;

                            case "ConsultarLoteRps":
                                strRetorno = SerializarObjeto((Components.PJoinvilleSC.ConsultarLoteRpsResposta)wsProxy.Invoke(
                                    servicoWS,
                                    metodo,
                                    new object[] {
                                        new Components.PJoinvilleSC.Prestador
                                        {
                                            CpfCnpj = new Components.PJoinvilleSC.CpfCnpj
                                            {
                                                Cnpj = (docXML.GetElementsByTagName("Cnpj")[0] != null ? docXML.GetElementsByTagName("Cnpj")[0].InnerText : ""),
                                                Cpf = (docXML.GetElementsByTagName("Cpf")[0] != null ? docXML.GetElementsByTagName("Cpf")[0].InnerText : "")
                                            },
                                            InscricaoMunicipal = (docXML.GetElementsByTagName("InscricaoMunicipal")[0] != null ? docXML.GetElementsByTagName("InscricaoMunicipal")[0].InnerText : "")
                                        },
                                        docXML.GetElementsByTagName("Protocolo")[0].InnerText }));
                                break;

                            case "ConsultarNfseRps":
                                strRetorno = SerializarObjeto((Components.PJoinvilleSC.ConsultarNfseRpsResposta)wsProxy.Invoke(
                                    servicoWS,
                                    metodo,
                                    new object[] {
                                        new Components.PJoinvilleSC.IdentificacaoRps
                                        {
                                            Numero = Convert.ToInt32((docXML.GetElementsByTagName("Numero")[0] != null ? docXML.GetElementsByTagName("Numero")[0].InnerText : "0")),
                                            Serie = (docXML.GetElementsByTagName("Serie")[0] != null ? docXML.GetElementsByTagName("Serie")[0].InnerText : ""),
                                            Tipo = Convert.ToInt32((docXML.GetElementsByTagName("Tipo")[0] != null ? docXML.GetElementsByTagName("Tipo")[0].InnerText : "0"))
                                        },
                                        new Components.PJoinvilleSC.Prestador
                                        {
                                            CpfCnpj = new Components.PJoinvilleSC.CpfCnpj
                                            {
                                                Cnpj = (docXML.GetElementsByTagName("Cnpj")[0] != null ? docXML.GetElementsByTagName("Cnpj")[0].InnerText : ""),
                                                Cpf = (docXML.GetElementsByTagName("Cpf")[0] != null ? docXML.GetElementsByTagName("Cpf")[0].InnerText : "")
                                            },
                                            InscricaoMunicipal = (docXML.GetElementsByTagName("InscricaoMunicipal")[0] != null ? docXML.GetElementsByTagName("InscricaoMunicipal")[0].InnerText : "")
                                        }
                                    }));
                                break;
                        }

                    break;

                #endregion Padrão Joinville_SC

                #region AVMB_ASTEN
                case PadroesNFSe.AVMB_ASTEN:
                    if (Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao)
                    {
                        Components.HPelotasRS.output pelotasOutput = new Components.HPelotasRS.output();
                        Components.HPelotasRS.input pelotasInput = new Components.HPelotasRS.input();
                        pelotasInput.nfseCabecMsg = cabecMsg;
                        pelotasInput.nfseDadosMsg = docXML.OuterXml;

                        pelotasOutput = (Components.HPelotasRS.output)wsProxy.Invoke(servicoWS, metodo, new object[] { pelotasInput });
                        strRetorno = pelotasOutput.outputXML;
                    }
                    else
                    {
                        Components.PPelotasRS.output pelotasOutput = new Components.PPelotasRS.output();
                        Components.PPelotasRS.input pelotasInput = new Components.PPelotasRS.input();
                        pelotasInput.nfseCabecMsg = cabecMsg;
                        pelotasInput.nfseDadosMsg = docXML.OuterXml;

                        pelotasOutput = (Components.PPelotasRS.output)wsProxy.Invoke(servicoWS, metodo, new object[] { pelotasInput });
                        strRetorno = pelotasOutput.outputXML;
                    }
                    break;
                #endregion

                case PadroesNFSe.PUBLIC_SOFT:
                    strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { docXML.OuterXml, cabecMsg.ToString() });
                    break;

                case PadroesNFSe.CECAM:
                    string cnpjcpfprestador = docXML.GetElementsByTagName("CNPJCPFPrestador")[0].InnerText;

                    string versaoXml = docXML.GetElementsByTagName("Versao")[0].InnerText;

                    strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { cnpjcpfprestador, docXML.OuterXml, versaoXml });
                    break;

                case PadroesNFSe.SMARAPD_204:
#if _fw46
                    dynamic smarapdOutput = Activator.CreateInstance(servicoWS.GetType().GetMethod(metodo).ReturnType);
                    dynamic smarapdInput = Activator.CreateInstance(servicoWS.GetType().GetMethod(metodo).GetParameters()[0].ParameterType);
                    smarapdInput.nfseCabecMsg = cabecMsg;
                    smarapdInput.nfseDadosMsg = docXML.OuterXml;

                    smarapdOutput = wsProxy.Invoke(servicoWS, metodo, new object[] { smarapdInput });
                    strRetorno = smarapdOutput.outputXML;
#endif
                    break;

                case PadroesNFSe.GEISWEB:
                    strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { docXML.OuterXml });
                    break;

                default:

                    #region Demais padrões

                    if (string.IsNullOrEmpty(cabecMsg))
                        strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { docXML.OuterXml });
                    else
                        strRetorno = wsProxy.InvokeStr(servicoWS, metodo, new object[] { cabecMsg.ToString(), docXML.OuterXml });
                    break;

                    #endregion Demais padrões
            }

            #region gerar arquivos assinados(somente debug)

#if DEBUG
            string path = Application.StartupPath + "\\teste_assintura\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            StreamWriter sw = new StreamWriter(path + "nfseMsg_assinado.xml", true);
            sw.Write(docXML.OuterXml);
            sw.Close();

            StreamWriter sw2 = new StreamWriter(path + "cabecMsg_assinado.xml", true);
            sw2.Write(cabecMsg.ToString());
            sw2.Close();
#endif

            #endregion gerar arquivos assinados(somente debug)

            //Atualizar o atributo do serviço da Nfe com o conteúdo retornado do webservice do sefaz
            typeServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, servicoNFe, new object[] { strRetorno });

            // Registra o retorno de acordo com o status obtido
            if (finalArqEnvio != string.Empty && finalArqRetorno != string.Empty)
            {
                typeServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, servicoNFe, new Object[] { finalArqEnvio + ".xml", finalArqRetorno + ".xml" });
            }
        }

        #endregion InvocarNFSe()

        /// <summary>
        /// Serializar o objeto para XML
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que será serializado</typeparam>
        /// <param name="retorno">Objeto de retorno que será convertivo</param>
        /// <returns></returns>
        private string SerializarObjeto<T>(T retorno)
            where T : new()
        {
            XmlSerializer serializerResposta = new XmlSerializer(typeof(T));
            StringWriter textWriter = new StringWriter();
            serializerResposta.Serialize(textWriter, retorno);

            return textWriter.ToString();
        }

        #endregion Métodos
    }
}

namespace NFe.Exceptions
{
    /// <summary>
    /// Classe para tratamento de exceções da classe Invocar Objeto
    /// </summary>
    public class ExceptionSemInternet : Exception
    {
        public ErroPadrao ErrorCode { get; private set; }

        /// <summary>
        /// Construtor que já define uma mensagem pré-definida de exceção
        /// </summary>
        /// <param name="CodigoErro">Código da mensagem de erro (Classe MsgErro)</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>24/11/2009</date>
        public ExceptionSemInternet(ErroPadrao Erro)
            : base(MsgErro.ErroPreDefinido(Erro))
        {
            this.ErrorCode = Erro;
        }

        /// <summary>
        /// Construtor que já define uma mensagem pré-definida de exceção com possibilidade de complemento da mensagem
        /// </summary>
        /// <param name="CodigoErro">Código da mensagem de erro (Classe MsgErro)</param>
        /// <param name="ComplementoMensagem">Complemento da mensagem de exceção</param>
        public ExceptionSemInternet(ErroPadrao Erro, string ComplementoMensagem)
            : base(MsgErro.ErroPreDefinido(Erro, ComplementoMensagem))
        {
            this.ErrorCode = Erro;
        }
    }

    /// <summary>
    /// Classe para tratamento de exceções da classe Invocar Objeto, mas exatamente no ponto em que vai enviar o XML para o SEFAZ
    /// </summary>
    public class ExceptionEnvioXML : Exception
    {
        public ErroPadrao ErrorCode { get; private set; }

        /// <summary>
        /// Construtor que já define uma mensagem pré-definida de exceção
        /// </summary>
        /// <param name="CodigoErro">Código da mensagem de erro (Classe MsgErro)</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/03/2010
        /// </remarks>
        public ExceptionEnvioXML(ErroPadrao Erro)
            : base(MsgErro.ErroPreDefinido(Erro))
        {
            this.ErrorCode = Erro;
        }

        /// <summary>
        /// Construtor que ´já define uma mensagem pré-definida de exceção com possibilidade de complemento da mensagem
        /// </summary>
        /// <param name="CodigoErro">Código da mensagem de erro (Classe MsgErro)</param>
        /// <param name="ComplementoMensagem">Complemento da mensagem de exceção</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/03/2010
        /// </remarks>
        public ExceptionEnvioXML(ErroPadrao Erro, string ComplementoMensagem)
            : base(MsgErro.ErroPreDefinido(Erro, ComplementoMensagem))
        {
            this.ErrorCode = Erro;
        }
    }
}
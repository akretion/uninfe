using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading;
using NFe.Exceptions;
using NFe.Settings;
using NFe.Components;
using NFe.Validate;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace NFe.Service
{
    /// <summary>
    /// Classe para invocar os métodos e propriedades das classes dos webservices da NFE
    /// </summary>
    public class InvocarObjeto
    {
        #region Objetos
        private Auxiliar oAux = new Auxiliar();
        #endregion

        #region Métodos

        #region Invocar()
        /// <summary>
        /// Metodo responsável por invocar o serviço do WebService do SEFAZ
        /// </summary>
        /// <param name="oWSProxy">Objeto da classe construida do WSDL</param>
        /// <param name="oServicoWS">Objeto da classe de envio do XML</param>
        /// <param name="cMetodo">Método da classe de envio do XML que faz o envio</param>
        /// <param name="cabecMsg">Objeto da classe de cabecalho do serviço</param>
        /// <param name="oServicoNFe">Objeto do Serviço de envio da NFE do UniNFe</param>
        /// <param name="cFinalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="cFinalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 17/03/2010
        /// </remarks>
        public void Invocar(WebServiceProxy oWSProxy,
                            object oServicoWS,
                            string cMetodo,
                            object cabecMsg,
                            object oServicoNFe,
                            string cFinalArqEnvio,
                            string cFinalArqRetorno)
        {
            int emp = Empresas.FindEmpresaByThread();

            XmlDocument docXML = new XmlDocument();

            // Definir o tipo de serviço da NFe
            Type typeServicoNFe = oServicoNFe.GetType();

            Servicos servico = (Servicos)oWSProxy.GetProp(oServicoNFe, NFe.Components.NFeStrConstants.Servico);

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(typeServicoNFe.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.GetProperty, null, oServicoNFe, null));

            // Exclui o Arquivo de Erro
            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(XmlNfeDadosMsg, cFinalArqEnvio + ".xml") + cFinalArqRetorno + ".err");

            // Validar o Arquivo XML
            ValidarXML validar = new ValidarXML(XmlNfeDadosMsg, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);

            string cResultadoValidacao = validar.ValidarArqXML(XmlNfeDadosMsg);
            if (cResultadoValidacao != "")
            {
                throw new Exception(cResultadoValidacao);
            }

            // Montar o XML de Lote de envio de Notas fiscais
            docXML.Load(XmlNfeDadosMsg);

            // Definir Proxy
            if (ConfiguracaoApp.Proxy)
            {
                oWSProxy.SetProp(oServicoWS, "Proxy", Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta));
            }

            // Limpa a variável de retorno
            XmlNode XmlRetorno = null;
            string strRetorno = string.Empty;

            //Vou mudar o timeout para evitar que demore a resposta e o uninfe aborte antes de recebe-la. Wandrey 17/09/2009
            //Isso talvez evite de não conseguir o número do recibo se o serviço do SEFAZ estiver lento.
            oWSProxy.SetProp(oServicoWS, "Timeout", 60000);

            //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
            if (ConfiguracaoApp.ChecarConexaoInternet)
                if (!Functions.IsConnectedToInternet())
                {
                    //Registrar o erro da validação para o sistema ERP
                    throw new ExceptionSemInternet(ErroPadrao.FalhaInternet, "\r\nArquivo: " + XmlNfeDadosMsg);
                }

            //Atribuir conteúdo para uma propriedade da classe NfeStatusServico2
            if (cMetodo.Substring(0, 3).ToLower() == "sce") // DPEC
            {
                oWSProxy.SetProp(oServicoWS, "sceCabecMsgValue", cabecMsg);
            }
            else
            {
                switch (servico)
                {
                    case Servicos.PedidoConsultaSituacaoMDFe:
                    case Servicos.PedidoSituacaoLoteMDFe:
                    case Servicos.EnviarLoteMDFe:
                    case Servicos.ConsultaStatusServicoMDFe:
                    case Servicos.RecepcaoEventoMDFe:
                    case Servicos.ConsultaNaoEncerradoMDFe:
                        oWSProxy.SetProp(oServicoWS, "mdfeCabecMsgValue", cabecMsg);
                        break;

                    case Servicos.InutilizarNumerosCTe:
                    case Servicos.PedidoConsultaSituacaoCTe:
                    case Servicos.PedidoSituacaoLoteCTe:
                    case Servicos.EnviarLoteCTe:
                    case Servicos.RecepcaoEventoCTe:
                    case Servicos.ConsultaStatusServicoCTe:
                        if (oWSProxy.GetProp(cabecMsg, "cUF").ToString() == "50") //Mato Grosso do Sul fugiu o padrão nacional
                        {
                            try
                            {
                                oWSProxy.SetProp(oServicoWS, "cteCabecMsg", cabecMsg);
                            }
                            catch //Se der erro é pq não está no ambiente normal então tem que ser o nome padrão pois Mato Grosso do Sul fugiu o padrão nacional.
                            {
                                oWSProxy.SetProp(oServicoWS, "cteCabecMsgValue", cabecMsg);
                            }
                        }
                        else
                        {
                            oWSProxy.SetProp(oServicoWS, "cteCabecMsgValue", cabecMsg);
                        }
                        break;

                    case Servicos.EnviarDFe:
                        break;

                    default:
                        oWSProxy.SetProp(oServicoWS, "nfeCabecMsgValue", cabecMsg);
                        break;
                }
            }

            // Envio da NFe Compactada - Renan
            if (servico == Servicos.EnviarLoteNfeZip2)
            {
                XmlNfeDadosMsg = XmlNfeDadosMsg + ".gz";
                FileInfo XMLNfeZip = new FileInfo(XmlNfeDadosMsg);
                string encodedData = StreamExtensions.ToBase64(XMLNfeZip);
                XmlRetorno = (XmlNode)oWSProxy.InvokeXML(oServicoWS, cMetodo, new object[] { encodedData });
            }
            else
                XmlRetorno = (XmlNode)oWSProxy.InvokeXML(oServicoWS, cMetodo, new object[] { docXML });

            if (XmlRetorno == null)
                throw new Exception("Erro de envio da solicitação do serviço: " + servico.ToString());

            typeServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, oServicoNFe, new object[] { XmlRetorno.OuterXml });

            // Registra o retorno de acordo com o status obtido
            if (cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
            {
                typeServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, oServicoNFe, new Object[] { cFinalArqEnvio + ".xml", cFinalArqRetorno + ".xml" });
            }
        }
        #endregion

        #region Invocar()
        /// <summary>
        /// Método responsável por invocar o serviço do WebService do SEFAZ
        /// </summary>
        /// <param name="oWSProxy">Objeto da classe construída do WSDL</param>
        /// <param name="oServicoWS">Objeto da classe de envio do XML</param>
        /// <param name="cMetodo">Método da classe de envio do XML que faz o envio</param>
        /// <param name="oCabecMsg">Objeto da classe de cabeçalho do serviço</param>
        /// <param name="oServicoNFe">Objeto do Serviço de envio da NFE do UniNFe</param>
        /// <remarks>
        /// Observações: Como esta sobrecarga não tem os parâmetros "cFinalArqEnvio e cFinalArqRetorno", 
        /// não será gerado o arquivo de retorno do webservice, 
        /// sendo assim no ponto onde este foi chamado deve-se manualmente fazer a gravação do retorno se for do interesse
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 17/03/2010
        /// </remarks>
        public void Invocar(WebServiceProxy oWSProxy,
                            object oServicoWS,
                            string cMetodo,
                            object oCabecMsg,
                            object oServicoNFe)
        {
            this.Invocar(oWSProxy, oServicoWS, cMetodo, oCabecMsg, oServicoNFe, string.Empty, string.Empty);
        }
        #endregion

        #region InvocarNFSe()
        /// <summary>
        /// Metodo responsável por invocar o serviço do WebService do SEFAZ
        /// </summary>
        /// <param name="oWSProxy">Objeto da classe construida do WSDL</param>
        /// <param name="oServicoWS">Objeto da classe de envio do XML</param>
        /// <param name="cMetodo">Método da classe de envio do XML que faz o envio</param>
        /// <param name="cabecMsg">Objeto da classe de cabecalho do serviço</param>
        /// <param name="oServicoNFe">Objeto do Serviço de envio da NFE do UniNFe</param>
        /// <param name="cFinalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="cFinalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 17/03/2010
        /// </remarks>
        public void InvocarNFSe(WebServiceProxy oWSProxy,
                            object oServicoWS,
                            string cMetodo,
                            string cabecMsg,
                            object oServicoNFe,
                            string cFinalArqEnvio,
                            string cFinalArqRetorno,
                            PadroesNFSe padraoNFSe,
                            Servicos servicoNFSe)
        {
            int emp = Empresas.FindEmpresaByThread();

            XmlDocument docXML = new XmlDocument();

            // Definir o tipo de serviço da NFe
            Type typeServicoNFe = oServicoNFe.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(typeServicoNFe.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.GetProperty, null, oServicoNFe, null));

            // Exclui o Arquivo de Erro
            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(XmlNfeDadosMsg, cFinalArqEnvio + ".xml") + cFinalArqRetorno + ".err");

            // Validar o Arquivo XML
            ValidarXML validar = new ValidarXML(XmlNfeDadosMsg, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);
            string cResultadoValidacao = validar.ValidarArqXML(XmlNfeDadosMsg);
            if (cResultadoValidacao != "")
            {
                switch (padraoNFSe)
                {
                    case PadroesNFSe.ISSONLINE4R:
                        break;
                    case PadroesNFSe.SMARAPD:
                        break;
                    default:
                        throw new Exception(cResultadoValidacao);
                }

            }

            // Montar o XML de Lote de envio de Notas fiscais
            docXML.Load(XmlNfeDadosMsg);

            // Definir Proxy
            if (ConfiguracaoApp.Proxy)
                if (padraoNFSe != PadroesNFSe.BETHA)
                {
                    oWSProxy.SetProp(oServicoWS, "Proxy", Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta));
                }
                else
                {
                    oWSProxy.Betha.Proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta);
                }

            // Limpa a variável de retorno
            string strRetorno = string.Empty;

            //Vou mudar o timeout para evitar que demore a resposta e o uninfe aborte antes de recebe-la. Wandrey 17/09/2009
            //Isso talvez evite de não conseguir o número do recibo se o serviço do SEFAZ estiver lento.
            if (padraoNFSe != PadroesNFSe.BETHA)
                oWSProxy.SetProp(oServicoWS, "Timeout", 60000);

            //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
            if (ConfiguracaoApp.ChecarConexaoInternet)  //danasa: 12/2013
                if (!Functions.IsConnectedToInternet())
                {
                    //Registrar o erro da validação para o sistema ERP
                    throw new ExceptionSemInternet(ErroPadrao.FalhaInternet, "\r\nArquivo: " + XmlNfeDadosMsg);
                }

            //Invocar o membro
            switch (padraoNFSe)
            {
                #region Padrão BETHA
                case PadroesNFSe.BETHA:
                    switch (cMetodo)
                    {
                        case "ConsultarSituacaoLoteRps":
                            strRetorno = oWSProxy.Betha.ConsultarSituacaoLoteRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "ConsultarLoteRps":
                            strRetorno = oWSProxy.Betha.ConsultarLoteRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "CancelarNfse":
                            strRetorno = oWSProxy.Betha.CancelarNfse(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "ConsultarNfse":
                            strRetorno = oWSProxy.Betha.ConsultarNfse(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "ConsultarNfsePorRps":
                            strRetorno = oWSProxy.Betha.ConsultarNfsePorRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;

                        case "RecepcionarLoteRps":
                            strRetorno = oWSProxy.Betha.RecepcionarLoteRps(docXML, Empresas.Configuracoes[emp].AmbienteCodigo);
                            break;
                    }
                    break;
                #endregion

                #region Padrão ISSONLINE
                case PadroesNFSe.ISSONLINE:
                    int operacao;
                    string senhaWs = Functions.GetMD5Hash(Empresas.Configuracoes[emp].SenhaWS);

                    switch (servicoNFSe)
                    {
                        case Servicos.RecepcionarLoteRps:
                            operacao = 1;
                            break;
                        case Servicos.CancelarNfse:
                            operacao = 2;
                            break;
                        default:
                            operacao = 3;
                            break;
                    }

                    strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { Convert.ToSByte(operacao), Empresas.Configuracoes[emp].UsuarioWS, senhaWs, docXML.OuterXml });
                    break;
                #endregion

                #region Padrão Blumenau-SC
                case PadroesNFSe.BLUMENAU_SC:
                    strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { 1, docXML.OuterXml });
                    break;
                #endregion

                #region Padrão Paulistana
                case PadroesNFSe.PAULISTANA:
                    strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { 1, docXML.OuterXml });
                    break;
                #endregion

                #region Demais padrões
                case PadroesNFSe.TECNOSISTEMAS:
                    strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { docXML.OuterXml, cabecMsg.ToString() });
                    break;

                case PadroesNFSe.SMARAPD:
                    if (cMetodo == "nfdEntradaCancelar")
                        strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { Empresas.Configuracoes[emp].UsuarioWS,
                        TFunctions.EncryptSHA1(Empresas.Configuracoes[emp].SenhaWS),
                        docXML.OuterXml });
                    else
                        strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { Empresas.Configuracoes[emp].UsuarioWS,
                        TFunctions.EncryptSHA1(Empresas.Configuracoes[emp].SenhaWS),
                        Empresas.Configuracoes[emp].UnidadeFederativaCodigo, 
                        docXML.OuterXml });
                    break;

                case PadroesNFSe.ISSWEB:
                    string versao = docXML.DocumentElement.GetElementsByTagName("Versao")[0].InnerText;
                    string cnpj = docXML.DocumentElement.GetElementsByTagName("CNPJCPFPrestador")[0].InnerText;
                    strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { cnpj, docXML.OuterXml, versao });
                    break;

                case PadroesNFSe.GINFES:
                case PadroesNFSe.THEMA:
                case PadroesNFSe.SALVADOR_BA:
                case PadroesNFSe.CANOAS_RS:
                case PadroesNFSe.ISSNET:
                default:
                    if (string.IsNullOrEmpty(cabecMsg))
                        strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { docXML.OuterXml });
                    else
                        strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { cabecMsg.ToString(), docXML.OuterXml });
                    break;
                #endregion
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
            #endregion

            //Atualizar o atributo do serviço da Nfe com o conteúdo retornado do webservice do sefaz                  
            typeServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, oServicoNFe, new object[] { strRetorno });

            // Registra o retorno de acordo com o status obtido
            if (cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
            {
                typeServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, oServicoNFe, new Object[] { cFinalArqEnvio + ".xml", cFinalArqRetorno + ".xml" });
            }
        }
        #endregion

        #endregion
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
        /// Construtor que ´já define uma mensagem pré-definida de exceção com possibilidade de complemento da mensagem
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

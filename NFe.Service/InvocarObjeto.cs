using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading;
using NFe.Exceptions;
using NFe.Settings;
using NFe.Components;
using NFe.Validate;

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
            int emp = Functions.FindEmpresaByThread();

            XmlDocument docXML = new XmlDocument();

            // Definir o tipo de serviço da NFe
            Type typeServicoNFe = oServicoNFe.GetType();

            Servicos servico = (Servicos)oWSProxy.GetProp(oServicoNFe, "Servico");

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(typeServicoNFe.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.GetProperty, null, oServicoNFe, null));

            // Exclui o Arquivo de Erro
            Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Functions.ExtrairNomeArq(XmlNfeDadosMsg, cFinalArqEnvio + ".xml") + cFinalArqRetorno + ".err");

            // Validar o Arquivo XML
            ValidarXML validar = new ValidarXML(XmlNfeDadosMsg, Empresa.Configuracoes[emp].UFCod);

            string cResultadoValidacao = validar.ValidarArqXML(XmlNfeDadosMsg);
            if(cResultadoValidacao != "")
            {
                throw new Exception(cResultadoValidacao);
            }

            // Montar o XML de Lote de envio de Notas fiscais
            docXML.Load(XmlNfeDadosMsg);

            // Definir Proxy
            if(ConfiguracaoApp.Proxy)
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
            if(ConfiguracaoApp.ChecarConexaoInternet)
                if(!Functions.IsConnectedToInternet())
                {
                    //Registrar o erro da validação para o sistema ERP
                    throw new ExceptionSemInternet(ErroPadrao.FalhaInternet, "\r\nArquivo: " + XmlNfeDadosMsg);
                }

            //Atribuir conteúdo para uma propriedade da classe NfeStatusServico2
            if(cMetodo.Substring(0, 3).ToLower() == "sce") // DPEC
            {
                oWSProxy.SetProp(oServicoWS, "sceCabecMsgValue", cabecMsg);
            }
            else
            {
                switch(Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        if(servico == Servicos.ConsultaCadastroContribuinte)
                        {
                            oWSProxy.SetProp(oServicoWS, "nfeCabecMsgValue", cabecMsg);
                        }
                        else
                        {
                            if(oWSProxy.GetProp(cabecMsg, "cUF").ToString() == "50")
                            {
                                oWSProxy.SetProp(oServicoWS, "cteCabecMsg", cabecMsg);
                            }
                            else
                            {
                                oWSProxy.SetProp(oServicoWS, "cteCabecMsgValue", cabecMsg);
                            }
                        }
                        break;

                    case TipoAplicativo.Nfe:
                        oWSProxy.SetProp(oServicoWS, "nfeCabecMsgValue", cabecMsg);
                        break;
                }
            }

            XmlRetorno = (XmlNode)oWSProxy.InvokeXML(oServicoWS, cMetodo, new object[] { docXML });

            typeServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, oServicoNFe, new object[] { XmlRetorno.OuterXml });

            // Registra o retorno de acordo com o status obtido
            if(cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
            {
                typeServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, oServicoNFe, new Object[] { cFinalArqEnvio + ".xml", cFinalArqRetorno + ".xml" });
            }
        }
        #endregion

        #region Invocar()
        /// <summary>
        /// Metodo responsável por invocar o serviço do WebService do SEFAZ
        /// </summary>
        /// <param name="oWSProxy">Objeto da classe construida do WSDL</param>
        /// <param name="oServicoWS">Objeto da classe de envio do XML</param>
        /// <param name="cMetodo">Método da classe de envio do XML que faz o envio</param>
        /// <param name="oCabecMsg">Objeto da classe de cabecalho do serviço</param>
        /// <param name="oServicoNFe">Objeto do Serviço de envio da NFE do UniNFe</param>
        /// <remarks>
        /// Observaçoes: Como esta sobrecarga não tem os parâmetros "cFinalArqEnvio e cFinalArqRetorno", 
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
            int emp = Functions.FindEmpresaByThread();

            XmlDocument docXML = new XmlDocument();

            // Definir o tipo de serviço da NFe
            Type typeServicoNFe = oServicoNFe.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(typeServicoNFe.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.GetProperty, null, oServicoNFe, null));

            // Exclui o Arquivo de Erro
            Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Functions/*oAux*/.ExtrairNomeArq(XmlNfeDadosMsg, cFinalArqEnvio + ".xml") + cFinalArqRetorno + ".err");

            // Validar o Arquivo XML
            ValidarXML validar = new ValidarXML(XmlNfeDadosMsg, Empresa.Configuracoes[emp].UFCod);
            string cResultadoValidacao = validar.ValidarArqXML(XmlNfeDadosMsg);
            if(cResultadoValidacao != "")
            {
                throw new Exception(cResultadoValidacao);
            }

            // Montar o XML de Lote de envio de Notas fiscais
            docXML.Load(XmlNfeDadosMsg);

            // Definir Proxy
            if(ConfiguracaoApp.Proxy)
                if(padraoNFSe != PadroesNFSe.BETHA)
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
            if(padraoNFSe != PadroesNFSe.BETHA)
                oWSProxy.SetProp(oServicoWS, "Timeout", 60000);

            //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
            if(!Functions.IsConnectedToInternet())
            {
                //Registrar o erro da validação para o sistema ERP
                throw new ExceptionSemInternet(ErroPadrao.FalhaInternet, "\r\nArquivo: " + XmlNfeDadosMsg);
            }

            //Invocar o membro
            switch(padraoNFSe)
            {
                #region Padrão BETHA
                case PadroesNFSe.BETHA:
                    switch(cMetodo)
                    {
                        case "ConsultarSituacaoLoteRps":
                            strRetorno = oWSProxy.Betha.ConsultarSituacaoLoteRps(docXML, Empresa.Configuracoes[emp].tpAmb);
                            break;

                        case "ConsultarLoteRps":
                            strRetorno = oWSProxy.Betha.ConsultarLoteRps(docXML, Empresa.Configuracoes[emp].tpAmb);
                            break;

                        case "CancelarNfse":
                            strRetorno = oWSProxy.Betha.CancelarNfse(docXML, Empresa.Configuracoes[emp].tpAmb);
                            break;

                        case "ConsultarNfse":
                            strRetorno = oWSProxy.Betha.ConsultarNfse(docXML, Empresa.Configuracoes[emp].tpAmb);
                            break;

                        case "ConsultarNfsePorRps":
                            strRetorno = oWSProxy.Betha.ConsultarNfsePorRps(docXML, Empresa.Configuracoes[emp].tpAmb);
                            break;

                        case "RecepcionarLoteRps":
                            strRetorno = oWSProxy.Betha.RecepcionarLoteRps(docXML, Empresa.Configuracoes[emp].tpAmb);
                            break;
                    }
                    break;
                #endregion

                #region Padrão ISSONLINE
                case PadroesNFSe.ISSONLINE:
                    int operacao;
                    string senhaWs = Functions.GetMD5Hash(Empresa.Configuracoes[emp].SenhaWS);

                    switch(servicoNFSe)
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

                    strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { Convert.ToSByte(operacao), Empresa.Configuracoes[emp].UsuarioWS, senhaWs, docXML.OuterXml });
                    break;
                #endregion

                #region Padrão Blumenau-SC
                case PadroesNFSe.BLUMENAU_SC:
                    strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { 1, docXML.OuterXml });
                    break;
                #endregion

                #region Demais padrões
                case PadroesNFSe.GINFES:
                case PadroesNFSe.THEMA:
                case PadroesNFSe.SALVADOR_BA:
                case PadroesNFSe.CANOAS_RS:
                case PadroesNFSe.ISSNET:
                case PadroesNFSe.DUETO:
                default:
                    if(string.IsNullOrEmpty(cabecMsg))
                        strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { docXML.OuterXml });
                    else
                        strRetorno = oWSProxy.InvokeStr(oServicoWS, cMetodo, new object[] { cabecMsg.ToString(), docXML.OuterXml });

                    break;
                #endregion
            }

            //Atualizar o atributo do serviço da Nfe com o conteúdo retornado do webservice do sefaz                  
            typeServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, oServicoNFe, new object[] { strRetorno });

            // Registra o retorno de acordo com o status obtido
            if(cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
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
    public class ExceptionSemInternet: Exception
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
    public class ExceptionEnvioXML: Exception
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

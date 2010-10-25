using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading;

namespace UniNFeLibrary
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

        #region RelacionarCertObj()
        /// <summary>
        /// Relaciona o certificdo digital a ser utilizado na autenticação com o objeto do serviço.
        /// </summary>
        /// <param name="pObjeto">Objeto que é para ser relacionado o certificado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>19/06/2008</date>
        private void RelacionarCertObj(object pObjeto)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);
            
            //Detectar o tipo do objeto
            Type tipoServico = pObjeto.GetType();

            //Relacionar o certificado ao objeto
            object oClientCertificates;
            Type tipoClientCertificates;
            oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, pObjeto, new Object[] { });
            tipoClientCertificates = oClientCertificates.GetType();
            tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { Empresa.Configuracoes[emp].X509Certificado });
        }
        #endregion

        #region Invocar() - Sobrecarga
        /// <summary>
        /// Metodo responsável por invocar o serviço do WebService do SEFAZ
        /// </summary>
        /// <param name="oWSProxy">Objeto da classe construida do WSDL</param>
        /// <param name="oServicoWS">Objeto da classe de envio do XML</param>
        /// <param name="cMetodo">Método da classe de envio do XML que faz o envio</param>
        /// <param name="oCabecMsg">Objeto da classe de cabecalho do serviço</param>
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
                            object oCabecMsg,
                            object oServicoNFe,
                            string cFinalArqEnvio,
                            string cFinalArqRetorno)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);
            XmlDocument docXML = new XmlDocument();

            // Definir o tipo de serviço da NFe
            Type typeServicoNFe = oServicoNFe.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(typeServicoNFe.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.GetProperty, null, oServicoNFe, null));

            try
            {
                //Verificar se o certificado digital está vencido, se tiver vai forçar uma exceção
                CertificadoDigital CertDig = new CertificadoDigital();
                CertDig.PrepInfCertificado(Empresa.Configuracoes[emp].X509Certificado);

                if (CertDig.lLocalizouCertificado == true)
                {
                    if (DateTime.Compare(DateTime.Now, CertDig.dValidadeFinal) > 0)
                    {
                        throw new ExceptionInvocarObjeto(ErroPadrao.CertificadoVencido, "(" + CertDig.dValidadeInicial.ToString() + " a " + CertDig.dValidadeFinal.ToString() + ")");
                    }
                }

                // Exclui o Arquivo de Erro
                oAux.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + oAux.ExtrairNomeArq(XmlNfeDadosMsg, cFinalArqEnvio + ".xml") + cFinalArqRetorno + ".err");

                // Validar o Arquivo XML
                string cResultadoValidacao = oAux.ValidarArqXML(XmlNfeDadosMsg);
                if (cResultadoValidacao != "")
                {
                    throw new Exception(cResultadoValidacao);
                }

                // Montar o XML de Lote de envio de Notas fiscais
                docXML.Load(XmlNfeDadosMsg);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            // Definir Proxy
            if (ConfiguracaoApp.Proxy)
            {
                oWSProxy.SetProp(oServicoWS, "Proxy", this.DefinirProxy());
            }

            // Limpa a variável de retorno
            XmlNode XmlRetorno;

            //Vou mudar o timeout para evitar que demore a resposta e o uninfe aborte antes de recebe-la. Wandrey 17/09/2009
            //Isso talvez evite de não conseguir o número do recibo se o serviço do SEFAZ estiver lento.
            oWSProxy.SetProp(oServicoWS, "Timeout", 20000);

            try
            {
                //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
                if (!InternetCS.IsConnectedToInternet())
                {
                    //Registrar o erro da validação para o sistema ERP
                    throw new ExceptionInvocarObjeto(ErroPadrao.FalhaInternet, "\r\nArquivo: " + XmlNfeDadosMsg);
                }

                //Atribuir conteúdo para uma propriedade da classe NfeStatusServico2
                if (cMetodo.Substring(0, 3).ToLower() == "sce") // DPEC
                {
                    oWSProxy.SetProp(oServicoWS, "sceCabecMsgValue", oCabecMsg);
                }
                else
                {
                    switch (ConfiguracaoApp.TipoAplicativo)
                    {
                        case UniNFeLibrary.Enums.TipoAplicativo.Cte:
                            oWSProxy.SetProp(oServicoWS, "cteCabecMsgValue", oCabecMsg);
                            break;

                        case UniNFeLibrary.Enums.TipoAplicativo.Nfe:
                            oWSProxy.SetProp(oServicoWS, "nfeCabecMsgValue", oCabecMsg);
                            break;

                        default:
                            break;
                    }
                }


                try
                {
                    //Invocar o membro
                    XmlRetorno = (XmlNode)oWSProxy.InvokeXML(oServicoWS, cMetodo, new object[] { docXML });
                }
                catch (Exception ex)
                {
                    if (cMetodo.Substring(0, 3).ToLower() == "sce") //danasa 21/10/2010
                        throw new ExceptionEnvioXML(ErroPadrao.FalhaEnvioXmlWSDPEC, "\r\nArquivo " + XmlNfeDadosMsg + "\r\nMessage Exception: " + ex.Message);
                    throw new ExceptionEnvioXML(ErroPadrao.FalhaEnvioXmlWS, "\r\nArquivo " + XmlNfeDadosMsg + "\r\nMessage Exception: " + ex.Message);
                }

                //Atualizar o atributo do serviço da Nfe com o conteúdo retornado do webservice do sefaz                  
                typeServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, oServicoNFe, new object[] { XmlRetorno.OuterXml });

                // Registra o retorno de acordo com o status obtido
                if (cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
                {
                    typeServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, oServicoNFe, new Object[] { cFinalArqEnvio + ".xml", cFinalArqRetorno + ".xml" });
                }
            }
            catch (ExceptionEnvioXML ex)
            {
                throw (ex);
            }
            catch (ExceptionInvocarObjeto ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Invocar() - Sobrecarga
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
            try
            {
                this.Invocar(oWSProxy, oServicoWS, cMetodo, oCabecMsg, oServicoNFe, string.Empty, string.Empty);
            }
            catch (ExceptionEnvioXML ex)
            {
                throw (ex);
            }
            catch (ExceptionInvocarObjeto ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Invocar()
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e Grava o XML retornado
        /// </summary>
        /// <param name="ServicoNFe">Objeto da classe ser serviço da NFe</param>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="ServicoWS">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <param name="cFinalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="cFinalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF", "-ped-can", "-can");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public bool Invocar(object ServicoNFe, string cVersaoDados, object ServicoWS, string cMetodo, string cFinalArqEnvio, string cFinalArqRetorno)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            bool lRetorna = false;

            // Definir o tipo de serviço dos WebServices do SEFAZ
            Type TipoServicoWS = ServicoWS.GetType();

            // Definir o tipo de serviço da NFe
            Type TipoServicoNFe = ServicoNFe.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(TipoServicoNFe.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.GetProperty, null, ServicoNFe, null));

            // exclui o arquivo de erro
            // danasa 19-9-2009
            oAux.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + oAux.ExtrairNomeArq(XmlNfeDadosMsg, cFinalArqEnvio + ".xml") + cFinalArqRetorno + ".err");

            // Validar o Arquivo XML
            string cResultadoValidacao = oAux.ValidarArqXML(XmlNfeDadosMsg);
            if (cResultadoValidacao != "")
            {
                //Registrar o erro da validação para o sistema ERP
                oAux.GravarArqErroServico(XmlNfeDadosMsg, cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", cResultadoValidacao);

                lRetorna = false;
                return lRetorna;
            }

            // Declara variável (tipo string) com o conteúdo do Cabecalho da mensagem a ser enviada para o webservice
            //            string vNFeCabecMsg = oGerarXML.CabecMsg(cVersaoDados);
            string vNFeCabecMsg = (string)(TipoServicoNFe.InvokeMember("CabecMsg", System.Reflection.BindingFlags.InvokeMethod, null, ServicoNFe, new Object[] { cVersaoDados }));

            // Montar o XML de Lote de envio de Notas fiscais
            string vNFeDadosMsg = oAux.XmlToString(XmlNfeDadosMsg);

            // Passar para o Objeto qual vai ser o certificado digital que ele deve utilizar             
            this.RelacionarCertObj(ServicoWS);

            // Definir Proxy
            if (ConfiguracaoApp.Proxy)
            {
                TipoServicoWS.InvokeMember("Proxy", System.Reflection.BindingFlags.SetProperty, null, ServicoWS, new object[] { this.DefinirProxy() });
            }

            // Limpa a variável de retorno
            string XmlRetorno = string.Empty;

            //Vou mudar o timeout para evitar que demore a resposta e o uninfe aborte antes de recebe-la. Wandrey 17/09/2009
            //Isso talvez evite de não conseguir o número do recibo se o serviço do SEFAZ estiver lento.
            TipoServicoWS.InvokeMember("Timeout", System.Reflection.BindingFlags.SetProperty, null, ServicoWS, new object[] { 100000 });

            //Vou fazer 5 tentativas de envio, se na terceira falhar eu gravo o erro de Retorno para o ERP
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
                    if (!InternetCS.IsConnectedToInternet())
                    {
                        //Registrar o erro da validação para o sistema ERP
                        throw new ExceptionInvocarObjeto(ErroPadrao.FalhaInternet, "\r\nArquivo: " + XmlNfeDadosMsg);
                    }

                    //Invocar o membro, ou seja, mandar o XML para o SEFAZ
                    XmlRetorno = (string)(TipoServicoWS.InvokeMember(cMetodo, System.Reflection.BindingFlags.InvokeMethod, null, ServicoWS, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));

                    //Atualizar o atributo do serviço da Nfe com o conteúdo retornado do webservice do sefaz
                    TipoServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, ServicoNFe, new object[] { XmlRetorno });

                    // Registra o retorno de acordo com o status obtido e Exclui o XML de solicitação do serviço
                    if (cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
                    {
                        TipoServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, ServicoNFe, new Object[] { cFinalArqEnvio + ".xml", cFinalArqRetorno + ".xml" });
                    }

                    lRetorna = true;
                }

                catch (ExceptionInvocarObjeto ex)
                {
                    // Passo alternativo: Registra o retorno no sistema interno, de acordo com a exceção
                    if (i == 5)
                    {
                        oAux.GravarArqErroServico(XmlNfeDadosMsg, cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", ex.Message + "\r\n\r\n" + ex.ToString());
                        lRetorna = false;

                        throw (ex);
                    }
                }

                if (lRetorna == true)
                {
                    break;
                }
            }

            return lRetorna;
        }
        #endregion

        #region Invocar() - Sobrecarga()
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e não grava o XML retornado
        /// </summary>
        /// <param name="ServicoNFe">Objeto da classe ser serviço da NFe</param>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="oServico">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public bool Invocar(object ServicoNFe, string cVersaoDados, object oServico, string cMetodo)
        {
            return Invocar(ServicoNFe, cVersaoDados, oServico, cMetodo, string.Empty, string.Empty);
        }
        #endregion

        #region Invocar() - Sobrecarga
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e Grava o XML retornado
        /// </summary>
        /// <param name="ServicoNFe">Objeto da classe ser serviço da NFe</param>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="CabecMsg">Objeto da classe de CabecMsg</param>
        /// <param name="ServicoWS">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="oParam">Parametros para execução dos serviços</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <param name="cFinalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="cFinalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF", "-ped-can", "-can");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public bool Invocar(object ServicoNFe, string cVersaoDados, object CabecMsg, object ServicoWS, ParametroEnvioXML oParam, string cMetodo, string cFinalArqEnvio, string cFinalArqRetorno)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            bool lRetorna = false;

            // Definir o tipo de serviço dos WebServices do SEFAZ
            Type TipoServicoWS = ServicoWS.GetType();

            // Definir o tipo de serviço da NFe
            Type TipoServicoNFe = ServicoNFe.GetType();

            // Definir o tipo do objeto CabecMsg
            Type TipoCabecMsg = CabecMsg.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(TipoServicoNFe.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.GetProperty, null, ServicoNFe, null));

            // exclui o arquivo de erro
            // danasa 19-9-2009
            oAux.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + oAux.ExtrairNomeArq(XmlNfeDadosMsg, cFinalArqEnvio + ".xml") + cFinalArqRetorno + ".err");

            // Validar o Arquivo XML
            string cResultadoValidacao = oAux.ValidarArqXML(XmlNfeDadosMsg);
            if (cResultadoValidacao != "")
            {
                //Registrar o erro da validação para o sistema ERP
                oAux.GravarArqErroServico(XmlNfeDadosMsg, cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", cResultadoValidacao);

                lRetorna = false;
                return lRetorna;
            }

            // Definir algumas propriedades do objeto do cabeçalho da mensagem
            string cUF = Empresa.Configuracoes[emp].UFCod.ToString();
            if (oParam != null)
            {
                cUF = oParam.UFCod.ToString();
            }

            TipoCabecMsg.InvokeMember("cUF", System.Reflection.BindingFlags.SetProperty, null, CabecMsg, new object[] { cUF });
            TipoCabecMsg.InvokeMember("versaoDados", System.Reflection.BindingFlags.SetProperty, null, CabecMsg, new object[] { cVersaoDados });

            // Montar o XML de Lote de envio de Notas fiscais
            XmlDocument docXML = new XmlDocument();
            docXML.Load(XmlNfeDadosMsg);

            // Passar para o Objeto qual vai ser o certificado digital que ele deve utilizar             
            this.RelacionarCertObj(ServicoWS);

            // Definir Proxy
            if (ConfiguracaoApp.Proxy)
            {
                TipoServicoWS.InvokeMember("Proxy", System.Reflection.BindingFlags.SetProperty, null, ServicoWS, new object[] { this.DefinirProxy() });
            }

            // Limpa a variável de retorno
            XmlNode XmlRetorno;

            //Vou mudar o timeout para evitar que demore a resposta e o uninfe aborte antes de recebe-la. Wandrey 17/09/2009
            //Isso talvez evite de não conseguir o número do recibo se o serviço do SEFAZ estiver lento.
            TipoServicoWS.InvokeMember("Timeout", System.Reflection.BindingFlags.SetProperty, null, ServicoWS, new object[] { 100000 });

            //Vou fazer 3 tentativas de envio, se na terceira falhar eu gravo o erro de Retorno para o ERP
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
                    if (!InternetCS.IsConnectedToInternet())
                    {
                        //Registrar o erro da validação para o sistema ERP
                        throw new ExceptionInvocarObjeto(ErroPadrao.FalhaInternet, "\r\nArquivo: " + XmlNfeDadosMsg);
                    }

                    //Atualizar a propriedade do objeto do cabecalho da mensagem
                    TipoServicoWS.InvokeMember("cteCabecMsgValue", System.Reflection.BindingFlags.SetProperty, null, ServicoWS, new object[] { CabecMsg });

                    //Invocar o membro
                    XmlRetorno = (XmlNode)(TipoServicoWS.InvokeMember(cMetodo, System.Reflection.BindingFlags.InvokeMethod, null, ServicoWS, new Object[] { docXML }));

                    //Atualizar o atributo do serviço da Nfe com o conteúdo retornado do webservice do sefaz                  
                    TipoServicoNFe.InvokeMember("vStrXmlRetorno", System.Reflection.BindingFlags.SetProperty, null, ServicoNFe, new object[] { XmlRetorno.OuterXml });

                    // Registra o retorno de acordo com o status obtido e Exclui o XML de solicitaÃ§Ã£o do serviÃ§o
                    if (cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
                    {
                        TipoServicoNFe.InvokeMember("XmlRetorno", System.Reflection.BindingFlags.InvokeMethod, null, ServicoNFe, new Object[] { cFinalArqEnvio + ".xml", cFinalArqRetorno + ".xml" });
                    }

                    lRetorna = true;
                }
                catch (ExceptionInvocarObjeto ex)
                {
                    // Passo alternativo: Registra o retorno no sistema interno, de acordo com a exceção
                    if (i == 5)
                    {
                        oAux.GravarArqErroServico(XmlNfeDadosMsg, cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", ex.Message + "\r\n\r\n" + ex.ToString());
                        lRetorna = false;

                        throw (ex);
                    }
                }

                if (lRetorna == true)
                {
                    break;
                }
            }
            return lRetorna;
        }
        #endregion

        #region Invocar() - Sobrecarga()
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e não grava o XML retornado
        /// </summary>
        /// <param name="ServicoNFe">Objeto da classe ser serviço da NFe</param>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="CabecMsg">Objeto da classe de CabecMsg</param>
        /// <param name="oServico">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public bool Invocar(object ServicoNFe, string cVersaoDados, object CabecMsg, object oServico, string cMetodo)
        {
            return Invocar(ServicoNFe, cVersaoDados, CabecMsg, oServico, null, cMetodo, string.Empty, string.Empty);
        }
        #endregion

        #region Invocar() - Sobrecarga()
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e não grava o XML retornado
        /// </summary>
        /// <param name="ServicoNFe">Objeto da classe ser serviço da NFe</param>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="CabecMsg">Objeto da classe de CabecMsg</param>
        /// <param name="oServico">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="oParam">Parametros para execução dos serviços</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public bool Invocar(object ServicoNFe, string cVersaoDados, object CabecMsg, object oServico, ParametroEnvioXML oParam, string cMetodo)
        {
            return Invocar(ServicoNFe, cVersaoDados, CabecMsg, oServico, oParam, cMetodo, string.Empty, string.Empty);
        }
        #endregion

        #region Invocar() - Sobrecarga
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e Grava o XML retornado
        /// </summary>
        /// <param name="ServicoNFe">Objeto da classe ser serviço da NFe</param>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="CabecMsg">Objeto da classe de CabecMsg</param>
        /// <param name="ServicoWS">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <param name="cFinalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="cFinalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF", "-ped-can", "-can");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public bool Invocar(object ServicoNFe, string cVersaoDados, object CabecMsg, object ServicoWS, string cMetodo, string cFinalArqEnvio, string cFinalArqRetorno)
        {
            return Invocar(ServicoNFe, cVersaoDados, CabecMsg, ServicoWS, null, cMetodo, cFinalArqEnvio, cFinalArqRetorno);
        }
        #endregion

        #region DefinirProxy()
        /// <summary>
        /// Efetua as definições do proxy
        /// </summary>
        /// <returns>Retorna as definições do Proxy</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/09/2009</date>
        private System.Net.IWebProxy DefinirProxy()
        {
            System.Net.IWebProxy Proxy = new System.Net.WebProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyPorta);
            Proxy.Credentials = new System.Net.NetworkCredential(ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha);

            //Analisar o codigo abaixo ver a diferença e ver se vai ter alguma necessidade no futuro
            //System.Net.IWebProxy Proxy = new System.Net.WebProxy(string Address, true);
            //Proxy.Credentials = new System.Net.NetworkCredential(ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, string domain);

            return Proxy;
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// Classe para tratamento de exceções da classe Invocar Objeto
    /// </summary>
    public class ExceptionInvocarObjeto : Exception
    {
        public ErroPadrao ErrorCode { get; private set; }

        /// <summary>
        /// Construtor que já define uma mensagem pré-definida de exceção
        /// </summary>
        /// <param name="CodigoErro">Código da mensagem de erro (Classe MsgErro)</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>24/11/2009</date>
        public ExceptionInvocarObjeto(ErroPadrao Erro)
            : base(MsgErro.ErroPreDefinido(Erro))
        {
            this.ErrorCode = Erro;
        }

        /// <summary>
        /// Construtor que ´já define uma mensagem pré-definida de exceção com possibilidade de complemento da mensagem
        /// </summary>
        /// <param name="CodigoErro">Código da mensagem de erro (Classe MsgErro)</param>
        /// <param name="ComplementoMensagem">Complemento da mensagem de exceção</param>
        public ExceptionInvocarObjeto(ErroPadrao Erro, string ComplementoMensagem)
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

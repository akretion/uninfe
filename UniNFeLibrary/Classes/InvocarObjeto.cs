using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

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
            //Detectar o tipo do objeto
            Type tipoServico = pObjeto.GetType();

            //Relacionar o certificado ao objeto
            object oClientCertificates;
            Type tipoClientCertificates;
            oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, pObjeto, new Object[] { });
            tipoClientCertificates = oClientCertificates.GetType();
            tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { ConfiguracaoApp.oCertificado });            
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
            bool lRetorna = false;

            // Definir o tipo de serviço dos WebServices do SEFAZ
            Type TipoServicoWS = ServicoWS.GetType();

            // Definir o tipo de serviço da NFe
            Type TipoServicoNFe = ServicoNFe.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(TipoServicoNFe.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.GetProperty, null, ServicoNFe, null));

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

            // Limpa a variável de retorno
            string XmlRetorno = string.Empty;

            //Vou fazer 3 tentativas de envio, se na terceira falhar eu gravo o erro de Retorno para o ERP
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    //Deu erro na primeira tentativa, sendo assim, vou aumentar o timeout para ver se não resolve a questão na segunda e terceira tentativa
                    if (i == 2)
                    {
                        TipoServicoWS.InvokeMember("Timeout", System.Reflection.BindingFlags.SetProperty, null, ServicoWS, new object[] { 300000 });
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

                catch (Exception ex)
                {
                    // Passo alternativo: Registra o retorno no sistema interno, de acordo com a exceção
                    if (i == 5)
                    {
                        oAux.GravarArqErroServico(XmlNfeDadosMsg, cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", ex.ToString());
                        lRetorna = false;
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
            bool lRetorna = false;

            // Definir o tipo de serviço dos WebServices do SEFAZ
            Type TipoServicoWS = ServicoWS.GetType();

            // Definir o tipo de serviço da NFe
            Type TipoServicoNFe = ServicoNFe.GetType();

            // Definir o tipo do objeto CabecMsg
            Type TipoCabecMsg = CabecMsg.GetType();

            // Resgatar o nome do arquivo XML a ser enviado para o webservice
            string XmlNfeDadosMsg = (string)(TipoServicoNFe.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.GetProperty, null, ServicoNFe, null));

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
            string cUF = ConfiguracaoApp.UFCod.ToString();
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

            // Limpa a variável de retorno
            XmlNode XmlRetorno;

            //Vou fazer 3 tentativas de envio, se na terceira falhar eu gravo o erro de Retorno para o ERP
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    //Deu erro na primeira tentativa, sendo assim, vou aumentar o timeout para ver se não resolve a questão na segunda e terceira tentativa
                    if (i == 2)
                    {
                        TipoServicoWS.InvokeMember("Timeout", System.Reflection.BindingFlags.SetProperty, null, ServicoWS, new object[] { 300000 });
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
                catch (Exception ex)
                {
                    // Passo alternativo: Registra o retorno no sistema interno, de acordo com a exceção
                    if (i == 5)
                    {
                        oAux.GravarArqErroServico(XmlNfeDadosMsg, cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", ex.ToString());
                        lRetorna = false;
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

        #endregion
    }
}

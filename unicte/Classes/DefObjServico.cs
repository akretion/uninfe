using System;
using System.Collections.Generic;
using System.Text;
using UniNFeLibrary;

namespace unicte
{
    public class DefObjServico : UniNFeLibrary.IDefObjServico
    {
        #region Métodos

        #region Cancelamento()
        /// <summary>
        /// Definir o Objeto que vai ser utilizado para cancelar notas fiscais de acordo com o 
        /// Estado e Ambiente informado para a classe
        /// </summary>
        /// <example>
        /// object oServico = null;
        /// this.Cancelamento(ref oServico);                       
        /// 
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        /// 
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeCancelamentoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <param name="pServico">Objeto da classe do webservice de envio dos XML que é para ser instanciada</param>
        /// <param name="pCabecMsg">Objeto da classe de cabecalho da mensagem que é para ser instanciada</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public void Cancelamento(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam)
        {
            //TODO: CONFIG
            if (oParam/*ConfiguracaoApp*/.tpAmb == TipoAmbiente.taProducao)
            {
                if (oParam/*ConfiguracaoApp*/.tpEmis == TipoEmissao.teSCAN)
                {
                }
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: break;
                        case 43: pServico = new wsRSPCancelamento.CteCancelamento(); pCabecMsg = new wsRSPCancelamento.cteCabecMsg(); break;
                        case 35: pServico = new wsSPPCancelamento.CteCancelamento(); pCabecMsg = new wsSPPCancelamento.cteCabecMsg(); break;
                    }
            }
            else if (oParam/*ConfiguracaoApp*/.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam/*ConfiguracaoApp*/.tpEmis == TipoEmissao.teSCAN)
                {
                }   //Contingência SCAN Ambiente Nacional
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: pServico = new wsMTHCancelamento.CteCancelamento(); pCabecMsg = new wsMTHCancelamento.cteCabecMsg(); break;
                        case 43: pServico = new wsRSHCancelamento.CteCancelamento(); pCabecMsg = new wsRSHCancelamento.cteCabecMsg(); break;
                        case 35: pServico = new wsSPHCancelamento.CteCancelamento(); pCabecMsg = new wsSPHCancelamento.cteCabecMsg(); break;
                    }
            }
        }
        #endregion

        #region Consulta
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para consultar   
        /// a situação da nota fiscal de acordo com o Estado e Ambiente
        /// informado para a classe
        /// </summary>
        /// <param name="pServico">Objeto da classe do webservice de envio dos XML que é para ser instanciada</param>
        /// <param name="pCabecMsg">Objeto da classe de cabecalho da mensagem que é para ser instanciada</param>
        /// <example>
        /// object oServico = null;
        /// this.Consulta(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeConsultaNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public void Consulta(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam)
        {
            if (oParam.tpAmb == TipoAmbiente.taProducao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) { } //Contingência SCAN Ambiente Nascional
                else if (oParam.UFCod == 51) { }
                else if (oParam.UFCod == 43) { pServico = new wsRSPConsulta.CteConsulta(); pCabecMsg = new wsRSPConsulta.cteCabecMsg(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPPConsulta.CteConsulta(); pCabecMsg = new wsSPPConsulta.cteCabecMsg(); }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) { } //Contingência SCAN Ambiente Nascional
                else if (oParam.UFCod == 51) { pServico = new wsMTHConsulta.CteConsulta(); pCabecMsg = new wsMTHConsulta.cteCabecMsg(); }
                else if (oParam.UFCod == 43) { pServico = new wsRSHConsulta.CteConsulta(); pCabecMsg = new wsRSHConsulta.cteCabecMsg(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPHConsulta.CteConsulta(); pCabecMsg = new wsSPHConsulta.cteCabecMsg(); }
            }
        }
        #endregion

        #region ConsultaCadastro()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para consultar o cadastro do contribuinte de acordo com o Estado e Ambiente informado para a classe
        /// </summary>
        /// <param name="pServico">Objeto da classe do webservice de envio dos XML que é para ser instanciada</param>
        /// <param name="pCabecMsg">Objeto da classe de cabecalho da mensagem que é para ser instanciada</param>
        /// <example>
        /// object oServico = null;
        /// this.ConsultaCadastro(ref oServico);
        /// if (this.InvocarObjeto("1.01", oServico, "consultaCadastro", "-cons-cad", "-ret-cons-cad") == true)
        /// {
        ///    //Deletar o arquivo de solicitação do serviço
        ///    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
        /// }
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/01/2009</date>
        public void ConsultaCadastro(ref object pServico, ParametroEnvioXML oParam)
        {
            if (oParam.tpAmb == TipoAmbiente.taProducao)
            {
                switch (oParam.UFCod)
                {
                    case 43: pServico = new wsRSPConsultaCadastro.CadConsultaCadastro(); break;
                    case 35: pServico = new wsSPPConsultaCadastro.CadConsultaCadastro(); break;
                }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                switch (oParam.UFCod)
                {
                    case 35: pServico = new wsSPHConsultaCadastro.CadConsultaCadastro(); break;
                }
            }
        }
        #endregion

        #region Inutilizacao()
        /// <summary>
        /// Definir o Objeto que vai ser utilizado para inutizar núemros de notas fiscais de acordo com o Estado e 
        /// Ambiente informado para a classe
        /// </summary>
        /// <param name="pServico">Objeto da classe do webservice de envio dos XML que é para ser instanciada</param>
        /// <param name="pCabecMsg">Objeto da classe de cabecalho da mensagem que é para ser instanciada</param>
        /// <example>
        /// object oServico = null;
        /// this.Inutilizacao(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        ///
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeInutilizacaoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public void Inutilizacao(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam)
        {
            //TODO: CONFIG
            if (oParam/*ConfiguracaoApp*/.tpAmb == TipoAmbiente.taProducao)
            {
                if (oParam/*ConfiguracaoApp*/.tpEmis == TipoEmissao.teSCAN)
                {
                }
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: break;
                        case 43: pServico = new wsRSPInutilizacao.CteInutilizacao(); pCabecMsg = new wsRSPInutilizacao.cteCabecMsg(); break;
                        case 35: pServico = new wsSPPInutilizacao.CteInutilizacao(); pCabecMsg = new wsSPPInutilizacao.cteCabecMsg(); break;
                    }
            }
            else if (oParam/*ConfiguracaoApp*/.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam/*ConfiguracaoApp*/.tpEmis == TipoEmissao.teSCAN)
                {
                }   //Contingência SCAN Ambiente Nacional
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: pServico = new wsMTHInutilizacao.CteInutilizacao(); pCabecMsg = new wsMTHInutilizacao.cteCabecMsg(); break;
                        case 43: pServico = new wsRSHInutilizacao.CteInutilizacao(); pCabecMsg = new wsRSHInutilizacao.cteCabecMsg(); break;
                        case 35: pServico = new wsSPHInutilizacao.CteInutilizacao(); pCabecMsg = new wsSPHInutilizacao.cteCabecMsg(); break;
                    }
            }
        }
        #endregion

        #region Recepcao()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para enviar a nota fiscal
        /// de acordo com o Estado e Ambiente informado para a classe
        /// </summary>
        /// <param name="pServico">Objeto da classe do webservice de envio dos XML que é para ser instanciada</param>
        /// <param name="pCabecMsg">Objeto da classe de cabecalho da mensagem que é para ser instanciada</param>
        /// <example>
        /// object oServico = null;
        /// this.Recepcao(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeRecepcao", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public void Recepcao(ref object pServico, ref object pCabecMsg)
        {
            //TODO: CONFIG
            if (ConfiguracaoApp.tpAmb == TipoAmbiente.taProducao/*1*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN) { } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSPRecepcao.CteRecepcao(); pCabecMsg = new wsRSPRecepcao.cteCabecMsg(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPPRecepcao.CteRecepcao(); pCabecMsg = new wsSPPRecepcao.cteCabecMsg(); }
            }
            else if (ConfiguracaoApp.tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN) { } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTHRecepcao.CteRecepcao(); pCabecMsg = new wsMTHRecepcao.cteCabecMsg(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSHRecepcao.CteRecepcao(); pCabecMsg = new wsRSHRecepcao.cteCabecMsg(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPHRecepcao.CteRecepcao(); pCabecMsg = new wsSPHRecepcao.cteCabecMsg(); }
            }
        }
        #endregion

        #region RetRecepcao()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para receber o retorno
        /// da situação da nota fiscal de acordo com o Estado e Ambiente
        /// informado para a classe
        /// </summary>
        /// <param name="pServico">Objeto da classe do webservice de envio dos XML que é para ser instanciada</param>
        /// <param name="pCabecMsg">Objeto da classe de cabecalho da mensagem que é para ser instanciada</param>
        /// <example>
        /// object oServico = null;
        /// this.RetRecepcao(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeRetRecepcao", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public void RetRecepcao(ref object pServico, ref object pCabecMsg)
        {
            //TODO: CONFIG
            if (ConfiguracaoApp.tpAmb == TipoAmbiente.taProducao/*1*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN) { } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSPRetRecepcao.CteRetRecepcao(); pCabecMsg = new wsRSPRetRecepcao.cteCabecMsg(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPPRetRecepcao.CteRetRecepcao(); pCabecMsg = new wsSPPRetRecepcao.cteCabecMsg(); }
            }
            else if (ConfiguracaoApp.tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN) { } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTHRetRecepcao.CteRetRecepcao(); pCabecMsg = new wsMTHRetRecepcao.cteCabecMsg(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSHRetRecepcao.CteRetRecepcao(); pCabecMsg = new wsRSHRetRecepcao.cteCabecMsg(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPHRetRecepcao.CteRetRecepcao(); pCabecMsg = new wsSPHRetRecepcao.cteCabecMsg(); }
            }
        }
        #endregion

        #region StatusServico()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para consultar o status do 
        /// serviço de acordo com o Estado e Ambiente informado para a classe
        /// </summary>
        /// <param name="pServico">Objeto da classe do webservice de envio dos XML que é para ser instanciada</param>
        /// <param name="pCabecMsg">Objeto da classe de cabecalho da mensagem que é para ser instanciada</param>
        /// <example>
        /// object oServico = null;
        /// this.StatusServico(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeStatusServicoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public void StatusServico(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam)
        {
            if (oParam.tpAmb == TipoAmbiente.taProducao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) {  } //Contingência SCAN Ambiente Nascional
                else if (oParam.UFCod == 51) {  }
                else if (oParam.UFCod == 43) { pServico = new wsRSPStatusServico.CteStatusServico(); pCabecMsg = new wsRSPStatusServico.cteCabecMsg(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPPStatusServico.CteStatusServico(); pCabecMsg = new wsSPPStatusServico.cteCabecMsg(); } 
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) {  } //Contingência SCAN Ambiente Nascional
                else if (oParam.UFCod == 51) { pServico = new wsMTHStatusServico.CteStatusServico(); pCabecMsg = new wsMTHStatusServico.cteCabecMsg(); }
                else if (oParam.UFCod == 43) { pServico = new wsRSHStatusServico.CteStatusServico(); pCabecMsg = new wsRSHStatusServico.cteCabecMsg(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPHStatusServico.CteStatusServico(); pCabecMsg = new wsSPHStatusServico.cteCabecMsg(); }
            }
        }
        #endregion

        #endregion
    }
}

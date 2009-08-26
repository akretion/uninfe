using System;
using System.Collections.Generic;
using System.Text;
using UniNFeLibrary;

namespace uninfe
{
    public class DefObjServico : UniNFeLibrary.IDefObjServico
    {
        #region Métodos

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
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANPStatusServico.NfeStatusServico(); } //Contingência SCAN Ambiente Nascional
                else if (oParam.UFCod == 51) { pServico = new wsMTPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 43) { pServico = new wsRSPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 31) { pServico = new wsMGPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 50) { pServico = new wsMSPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 52) { pServico = new wsGOPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 41) { pServico = new wsPRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 23) { pServico = new wsCEPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 29) { pServico = new wsBAPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 53) { pServico = new wsDFPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 26) { pServico = new wsPEPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 11) { pServico = new wsROPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 13) { pServico = new wsAMPStatusServico.NfeStatusServico(); }

                else if (oParam.UFCod == 15) { pServico = new wsVNPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 21) { pServico = new wsVNPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 22) { pServico = new wsVNPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 24) { pServico = new wsVNPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 32) { pServico = new wsVNPStatusServico.NfeStatusServico(); }

                else if (oParam.UFCod == 42) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 17) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 28) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 33) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 12) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 27) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 16) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 25) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 14) { pServico = new wsVRPStatusServico.NfeStatusServico(); }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANHStatusServico.NfeStatusServico(); } //Contingência SCAN Ambiente Nascional
                else if (oParam.UFCod == 51) { pServico = new wsMTHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 43) { pServico = new wsRSHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 31) { pServico = new wsMGHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 50) { pServico = new wsMSHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 52) { pServico = new wsGOHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 41) { pServico = new wsPRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 23) { pServico = new wsCEHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 29) { pServico = new wsBAHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 53) { pServico = new wsDFHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 26) { pServico = new wsPEHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 11) { pServico = new wsROHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 13) { pServico = new wsAMHStatusServico.NfeStatusServico(); }

                else if (oParam.UFCod == 15) { pServico = new wsVNHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 21) { pServico = new wsVNHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 22) { pServico = new wsVNHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 24) { pServico = new wsVNHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 32) { pServico = new wsVNHStatusServico.NfeStatusServico(); }

                else if (oParam.UFCod == 42) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 17) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 28) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 33) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 12) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 27) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 16) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 25) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
                else if (oParam.UFCod == 14) { pServico = new wsVRHStatusServico.NfeStatusServico(); }
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
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANPRecepcao.NfeRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMPRecepcao.NfeRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNPRecepcao.NfeRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRPRecepcao.NfeRecepcao(); }
            }
            else if (ConfiguracaoApp.tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANHRecepcao.NfeRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMHRecepcao.NfeRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNHRecepcao.NfeRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRHRecepcao.NfeRecepcao(); }
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
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANPRetRecepcao.NfeRetRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRPRetRecepcao.NfeRetRecepcaoService(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMPRetRecepcao.NfeRetRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); }
            }
            else if (ConfiguracaoApp.tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANHRetRecepcao.NfeRetRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRHRetRecepcao.NfeRetRecepcaoService(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMHRetRecepcao.NfeRetRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); }
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
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANPConsulta.NfeConsulta(); } //Contingência SCAN Ambiente Nascional
                else if (oParam.UFCod == 51) { pServico = new wsMTPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 43) { pServico = new wsRSPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 31) { pServico = new wsMGPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 50) { pServico = new wsMSPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 52) { pServico = new wsGOPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 41) { pServico = new wsPRPConsulta.NfeConsultaService(); }
                else if (oParam.UFCod == 23) { pServico = new wsCEPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 29) { pServico = new wsBAPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 53) { pServico = new wsDFPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 26) { pServico = new wsPEPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 11) { pServico = new wsROPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 13) { pServico = new wsAMPConsulta.NfeConsulta(); }

                else if (oParam.UFCod == 15) { pServico = new wsVNPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 21) { pServico = new wsVNPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 22) { pServico = new wsVNPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 24) { pServico = new wsVNPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 32) { pServico = new wsVNPConsulta.NfeConsulta(); }

                else if (oParam.UFCod == 42) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 17) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 28) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 33) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 12) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 27) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 16) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 25) { pServico = new wsVRPConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 14) { pServico = new wsVRPConsulta.NfeConsulta(); }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANHConsulta.NfeConsulta(); } //Contingência SCAN Ambiente Nacional
                else if (oParam.UFCod == 51) { pServico = new wsMTHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 43) { pServico = new wsRSHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 31) { pServico = new wsMGHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 50) { pServico = new wsMSHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 52) { pServico = new wsGOHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 41) { pServico = new wsPRHConsulta.NfeConsultaService(); }
                else if (oParam.UFCod == 23) { pServico = new wsCEHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 29) { pServico = new wsBAHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 53) { pServico = new wsDFHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 26) { pServico = new wsPEHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 11) { pServico = new wsROHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 13) { pServico = new wsAMHConsulta.NfeConsulta(); }

                else if (oParam.UFCod == 15) { pServico = new wsVNHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 21) { pServico = new wsVNHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 22) { pServico = new wsVNHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 24) { pServico = new wsVNHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 32) { pServico = new wsVNHConsulta.NfeConsulta(); }

                else if (oParam.UFCod == 42) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 17) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 28) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 33) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 12) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 27) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 16) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 25) { pServico = new wsVRHConsulta.NfeConsulta(); }
                else if (oParam.UFCod == 14) { pServico = new wsVRHConsulta.NfeConsulta(); }
            }
        }
        #endregion

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
        public void Cancelamento(ref object pServico, ref object pCabecMsg)
        {
            //TODO: CONFIG
            if (ConfiguracaoApp.tpAmb == TipoAmbiente.taProducao/*1*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANPCancelamento.NfeCancelamento(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRPCancelamento.NfeCancelamentoService(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMPCancelamento.NfeCancelamento(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNPCancelamento.NfeCancelamento(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRPCancelamento.NfeCancelamento(); }
            }
            else if (ConfiguracaoApp.tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANHCancelamento.NfeCancelamento(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRHCancelamento.NfeCancelamentoService(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMHCancelamento.NfeCancelamento(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNHCancelamento.NfeCancelamento(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRHCancelamento.NfeCancelamento(); }
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
        public void Inutilizacao(ref object pServico, ref object pCabecMsg)
        {
            //TODO: CONFIG
            if (ConfiguracaoApp.tpAmb == TipoAmbiente.taProducao/*1*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANPInutilizacao.NfeInutilizacao(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRPInutilizacao.NfeInutilizacaoService(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMPInutilizacao.NfeInutilizacao(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNPInutilizacao.NfeInutilizacao(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRPInutilizacao.NfeInutilizacao(); }
            }
            else if (ConfiguracaoApp.tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (ConfiguracaoApp.tpEmis == TipoEmissao.teSCAN/*3*/) { pServico = new wsSCANHInutilizacao.NfeInutilizacao(); } //Contingência SCAN Ambiente Nascional
                else if (ConfiguracaoApp.UFCod == 51) { pServico = new wsMTHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 43) { pServico = new wsRSHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 31) { pServico = new wsMGHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 35) { pServico = new wsSPHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 50) { pServico = new wsMSHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 52) { pServico = new wsGOHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 41) { pServico = new wsPRHInutilizacao.NfeInutilizacaoService(); }
                else if (ConfiguracaoApp.UFCod == 23) { pServico = new wsCEHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 29) { pServico = new wsBAHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 53) { pServico = new wsDFHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 26) { pServico = new wsPEHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 11) { pServico = new wsROHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 13) { pServico = new wsAMHInutilizacao.NfeInutilizacao(); }

                else if (ConfiguracaoApp.UFCod == 15) { pServico = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 21) { pServico = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 22) { pServico = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 24) { pServico = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 32) { pServico = new wsVNHInutilizacao.NfeInutilizacao(); }

                else if (ConfiguracaoApp.UFCod == 42) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 17) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 28) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 33) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 12) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 27) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 16) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 25) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (ConfiguracaoApp.UFCod == 14) { pServico = new wsVRHInutilizacao.NfeInutilizacao(); }
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
                if (oParam.UFCod == 29) { pServico = new wsBAPConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 23) { pServico = new wsCEPConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 53) { pServico = new wsDFPConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 52) { pServico = new wsGOPConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 26) { pServico = new wsPEPConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 11) { pServico = new wsROPConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 43) { pServico = new wsRSPConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPPConsultaCadastro.CadConsultaCadastro(); }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (oParam.UFCod == 23) { pServico = new wsCEHConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 53) { pServico = new wsDFHConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 52) { pServico = new wsGOHConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 26) { pServico = new wsPEHConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 11) { pServico = new wsROHConsultaCadastro.CadConsultaCadastro(); }
                else if (oParam.UFCod == 35) { pServico = new wsSPHConsultaCadastro.CadConsultaCadastro(); }
            }
        }
        #endregion

        #endregion
    }
}

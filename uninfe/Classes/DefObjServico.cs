using System;
using System.Collections.Generic;
using System.Text;
using UniNFeLibrary;
using System.Threading;

namespace uninfe
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
            if (oParam/*ConfiguracaoApp*/.tpAmb == TipoAmbiente.taProducao)
            {
                if (oParam/*ConfiguracaoApp*/.tpEmis == TipoEmissao.teSCAN)
                {
                    pServico = new wsSCANPCancelamento.NfeCancelamento();
                }   //Contingência SCAN Ambiente Nacional
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: pServico = new wsMTPCancelamento.NfeCancelamento(); break;
                        case 43: pServico = new wsRSPCancelamento.NfeCancelamento(); break;
                        case 31: pServico = new wsMGPCancelamento.NfeCancelamento(); break;
                        case 35: pServico = new wsSPPCancelamento.NfeCancelamento(); break;
                        case 50: pServico = new wsMSPCancelamento.NfeCancelamento(); break;
                        case 52: pServico = new wsGOPCancelamento.NfeCancelamento(); break;
                        case 41: pServico = new wsPRPCancelamento.NfeCancelamentoService(); break;
                        case 23: pServico = new wsCEPCancelamento.NfeCancelamento(); break;
                        case 29: pServico = new wsBAPCancelamento.NfeCancelamento(); break;
                        case 53: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 26: pServico = new wsPEPCancelamento.NfeCancelamento(); break;
                        case 11: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 13: pServico = new wsAMPCancelamento.NfeCancelamento(); break;

                        case 15: pServico = new wsVNPCancelamento.NfeCancelamento(); break;
                        case 21: pServico = new wsVNPCancelamento.NfeCancelamento(); break;
                        case 22: pServico = new wsVNPCancelamento.NfeCancelamento(); break;
                        case 24: pServico = new wsVNPCancelamento.NfeCancelamento(); break;
                        case 32: pServico = new wsVNPCancelamento.NfeCancelamento(); break;

                        case 42: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 17: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 28: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 33: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 12: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 27: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 16: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 25: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                        case 14: pServico = new wsVRPCancelamento.NfeCancelamento(); break;
                    }
            }
            else if (oParam/*ConfiguracaoApp*/.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam/*ConfiguracaoApp*/.tpEmis == TipoEmissao.teSCAN)
                {
                    pServico = new wsSCANHCancelamento.NfeCancelamento();
                }   //Contingência SCAN Ambiente Nacional
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: pServico = new wsMTHCancelamento.NfeCancelamento(); break;
                        case 43: pServico = new wsRSHCancelamento.NfeCancelamento(); break;
                        case 31: pServico = new wsMGHCancelamento.NfeCancelamento(); break;
                        case 35: pServico = new wsSPHCancelamento.NfeCancelamento(); break;
                        case 50: pServico = new wsMSHCancelamento.NfeCancelamento(); break;
                        case 52: pServico = new wsGOHCancelamento.NfeCancelamento(); break;
                        case 41: pServico = new wsPRHCancelamento.NfeCancelamentoService(); break;
                        case 23: pServico = new wsCEHCancelamento.NfeCancelamento(); break;
                        case 29: pServico = new wsBAHCancelamento.NfeCancelamento(); break;
                        case 53: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 26: pServico = new wsPEHCancelamento.NfeCancelamento(); break;
                        case 11: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 13: pServico = new wsAMHCancelamento.NfeCancelamento(); break;

                        case 15: pServico = new wsVNHCancelamento.NfeCancelamento(); break;
                        case 21: pServico = new wsVNHCancelamento.NfeCancelamento(); break;
                        case 22: pServico = new wsVNHCancelamento.NfeCancelamento(); break;
                        case 24: pServico = new wsVNHCancelamento.NfeCancelamento(); break;
                        case 32: pServico = new wsVNHCancelamento.NfeCancelamento(); break;

                        case 42: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 17: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 28: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 33: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 12: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 27: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 16: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 25: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
                        case 14: pServico = new wsVRHCancelamento.NfeCancelamento(); break;
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
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANPConsulta.NfeConsulta(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (oParam.UFCod)
                    {
                        case 51: pServico = new wsMTPConsulta.NfeConsulta(); break;
                        case 43: pServico = new wsRSPConsulta.NfeConsulta(); break;
                        case 31: pServico = new wsMGPConsulta.NfeConsulta(); break;
                        case 35: pServico = new wsSPPConsulta.NfeConsulta(); break;
                        case 50: pServico = new wsMSPConsulta.NfeConsulta(); break;
                        case 52: pServico = new wsGOPConsulta.NfeConsulta(); break;
                        case 41: pServico = new wsPRPConsulta.NfeConsultaService(); break;
                        case 23: pServico = new wsCEPConsulta.NfeConsulta(); break;
                        case 29: pServico = new wsBAPConsulta.NfeConsulta(); break;
                        case 53: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 26: pServico = new wsPEPConsulta.NfeConsulta(); break;
                        case 11: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 13: pServico = new wsAMPConsulta.NfeConsulta(); break;

                        case 15: pServico = new wsVNPConsulta.NfeConsulta(); break;
                        case 21: pServico = new wsVNPConsulta.NfeConsulta(); break;
                        case 22: pServico = new wsVNPConsulta.NfeConsulta(); break;
                        case 24: pServico = new wsVNPConsulta.NfeConsulta(); break;
                        case 32: pServico = new wsVNPConsulta.NfeConsulta(); break;

                        case 42: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 17: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 28: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 33: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 12: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 27: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 16: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 25: pServico = new wsVRPConsulta.NfeConsulta(); break;
                        case 14: pServico = new wsVRPConsulta.NfeConsulta(); break;
                    }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANHConsulta.NfeConsulta(); } //Contingência SCAN Ambiente Nacional
                else
                    switch (oParam.UFCod)
                    {
                        case 51: pServico = new wsMTHConsulta.NfeConsulta(); break;
                        case 43: pServico = new wsRSHConsulta.NfeConsulta(); break;
                        case 31: pServico = new wsMGHConsulta.NfeConsulta(); break;
                        case 35: pServico = new wsSPHConsulta.NfeConsulta(); break;
                        case 50: pServico = new wsMSHConsulta.NfeConsulta(); break;
                        case 52: pServico = new wsGOHConsulta.NfeConsulta(); break;
                        case 41: pServico = new wsPRHConsulta.NfeConsultaService(); break;
                        case 23: pServico = new wsCEHConsulta.NfeConsulta(); break;
                        case 29: pServico = new wsBAHConsulta.NfeConsulta(); break;
                        case 53: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 26: pServico = new wsPEHConsulta.NfeConsulta(); break;
                        case 11: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 13: pServico = new wsAMHConsulta.NfeConsulta(); break;

                        case 15: pServico = new wsVNHConsulta.NfeConsulta(); break;
                        case 21: pServico = new wsVNHConsulta.NfeConsulta(); break;
                        case 22: pServico = new wsVNHConsulta.NfeConsulta(); break;
                        case 24: pServico = new wsVNHConsulta.NfeConsulta(); break;
                        case 32: pServico = new wsVNHConsulta.NfeConsulta(); break;

                        case 42: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 17: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 28: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 33: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 12: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 27: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 16: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 25: pServico = new wsVRHConsulta.NfeConsulta(); break;
                        case 14: pServico = new wsVRHConsulta.NfeConsulta(); break;
                    }
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
                    case 29: pServico = new wsBAPConsultaCadastro.CadConsultaCadastro(); break;
                    case 23: pServico = new wsCEPConsultaCadastro.CadConsultaCadastro(); break;
                    case 52: pServico = new wsGOPConsultaCadastro.CadConsultaCadastro(); break;
                    case 26: pServico = new wsPEPConsultaCadastro.CadConsultaCadastro(); break;
                    case 43: pServico = new wsRSPConsultaCadastro.CadConsultaCadastro(); break;
                    case 35: pServico = new wsSPPConsultaCadastro.CadConsultaCadastro(); break;
                    case 31: pServico = new wsMGPConsultaCadastro.CadConsultaCadastro(); break;
                }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                switch (oParam.UFCod)
                {
                    case 23: pServico = new wsCEHConsultaCadastro.CadConsultaCadastro(); break;
                    case 52: pServico = new wsGOHConsultaCadastro.CadConsultaCadastro(); break;
                    case 26: pServico = new wsPEHConsultaCadastro.CadConsultaCadastro(); break;
                    case 35: pServico = new wsSPHConsultaCadastro.CadConsultaCadastro(); break;
                    case 31: pServico = new wsMGHConsultaCadastro.CadConsultaCadastro(); break;
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
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            if (oParam/*ConfiguracaoApp*/.tpAmb == TipoAmbiente.taProducao)
            {
                if (oParam/*ConfiguracaoApp*/.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANPInutilizacao.NfeInutilizacao(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: pServico = new wsMTPInutilizacao.NfeInutilizacao(); break;
                        case 43: pServico = new wsRSPInutilizacao.NfeInutilizacao(); break;
                        case 31: pServico = new wsMGPInutilizacao.NfeInutilizacao(); break;
                        case 35: pServico = new wsSPPInutilizacao.NfeInutilizacao(); break;
                        case 50: pServico = new wsMSPInutilizacao.NfeInutilizacao(); break;
                        case 52: pServico = new wsGOPInutilizacao.NfeInutilizacao(); break;
                        case 41: pServico = new wsPRPInutilizacao.NfeInutilizacaoService(); break;
                        case 23: pServico = new wsCEPInutilizacao.NfeInutilizacao(); break;
                        case 29: pServico = new wsBAPInutilizacao.NfeInutilizacao(); break;
                        case 53: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 26: pServico = new wsPEPInutilizacao.NfeInutilizacao(); break;
                        case 11: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 13: pServico = new wsAMPInutilizacao.NfeInutilizacao(); break;

                        case 15: pServico = new wsVNPInutilizacao.NfeInutilizacao(); break;
                        case 21: pServico = new wsVNPInutilizacao.NfeInutilizacao(); break;
                        case 22: pServico = new wsVNPInutilizacao.NfeInutilizacao(); break;
                        case 24: pServico = new wsVNPInutilizacao.NfeInutilizacao(); break;
                        case 32: pServico = new wsVNPInutilizacao.NfeInutilizacao(); break;

                        case 42: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 17: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 28: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 33: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 12: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 27: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 16: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 25: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                        case 14: pServico = new wsVRPInutilizacao.NfeInutilizacao(); break;
                    }
            }
            else if (Empresa.Configuracoes[emp].tpAmb == TipoAmbiente.taHomologacao)
            {
                if (Empresa.Configuracoes[emp].tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANHInutilizacao.NfeInutilizacao(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (oParam/*ConfiguracaoApp*/.UFCod)
                    {
                        case 51: pServico = new wsMTHInutilizacao.NfeInutilizacao(); break;
                        case 43: pServico = new wsRSHInutilizacao.NfeInutilizacao(); break;
                        case 31: pServico = new wsMGHInutilizacao.NfeInutilizacao(); break;
                        case 35: pServico = new wsSPHInutilizacao.NfeInutilizacao(); break;
                        case 50: pServico = new wsMSHInutilizacao.NfeInutilizacao(); break;
                        case 52: pServico = new wsGOHInutilizacao.NfeInutilizacao(); break;
                        case 41: pServico = new wsPRHInutilizacao.NfeInutilizacaoService(); break;
                        case 23: pServico = new wsCEHInutilizacao.NfeInutilizacao(); break;
                        case 29: pServico = new wsBAHInutilizacao.NfeInutilizacao(); break;
                        case 53: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 26: pServico = new wsPEHInutilizacao.NfeInutilizacao(); break;
                        case 11: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 13: pServico = new wsAMHInutilizacao.NfeInutilizacao(); break;

                        case 15: pServico = new wsVNHInutilizacao.NfeInutilizacao(); break;
                        case 21: pServico = new wsVNHInutilizacao.NfeInutilizacao(); break;
                        case 22: pServico = new wsVNHInutilizacao.NfeInutilizacao(); break;
                        case 24: pServico = new wsVNHInutilizacao.NfeInutilizacao(); break;
                        case 32: pServico = new wsVNHInutilizacao.NfeInutilizacao(); break;

                        case 42: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 17: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 28: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 33: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 12: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 27: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 16: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 25: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
                        case 14: pServico = new wsVRHInutilizacao.NfeInutilizacao(); break;
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
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            if (Empresa.Configuracoes[emp].tpAmb == TipoAmbiente.taProducao/*1*/)
            {
                if (Empresa.Configuracoes[emp].tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANPRecepcao.NfeRecepcao(); } //Contingência SCAN Ambiente Nascional
                else switch (Empresa.Configuracoes[emp].UFCod)
                    {
                        case 51: pServico = new wsMTPRecepcao.NfeRecepcao(); break;
                        case 43: pServico = new wsRSPRecepcao.NfeRecepcao(); break;
                        case 31: pServico = new wsMGPRecepcao.NfeRecepcao(); break;
                        case 35: pServico = new wsSPPRecepcao.NfeRecepcao(); break;
                        case 50: pServico = new wsMSPRecepcao.NfeRecepcao(); break;
                        case 52: pServico = new wsGOPRecepcao.NfeRecepcao(); break;
                        case 41: pServico = new wsPRPRecepcao.NfeRecepcao(); break;
                        case 23: pServico = new wsCEPRecepcao.NfeRecepcao(); break;
                        case 29: pServico = new wsBAPRecepcao.NfeRecepcao(); break;
                        case 53: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 26: pServico = new wsPEPRecepcao.NfeRecepcao(); break;
                        case 11: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 13: pServico = new wsAMPRecepcao.NfeRecepcao(); break;

                        case 15: pServico = new wsVNPRecepcao.NfeRecepcao(); break;
                        case 21: pServico = new wsVNPRecepcao.NfeRecepcao(); break;
                        case 22: pServico = new wsVNPRecepcao.NfeRecepcao(); break;
                        case 24: pServico = new wsVNPRecepcao.NfeRecepcao(); break;
                        case 32: pServico = new wsVNPRecepcao.NfeRecepcao(); break;

                        case 42: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 17: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 28: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 33: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 12: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 27: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 16: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 25: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                        case 14: pServico = new wsVRPRecepcao.NfeRecepcao(); break;
                    }
            }
            else if (Empresa.Configuracoes[emp].tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (Empresa.Configuracoes[emp].tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANHRecepcao.NfeRecepcao(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (Empresa.Configuracoes[emp].UFCod)
                    {
                        case 51: pServico = new wsMTHRecepcao.NfeRecepcao(); break;
                        case 43: pServico = new wsRSHRecepcao.NfeRecepcao(); break;
                        case 31: pServico = new wsMGHRecepcao.NfeRecepcao(); break;
                        case 35: pServico = new wsSPHRecepcao.NfeRecepcao(); break;
                        case 50: pServico = new wsMSHRecepcao.NfeRecepcao(); break;
                        case 52: pServico = new wsGOHRecepcao.NfeRecepcao(); break;
                        case 41: pServico = new wsPRHRecepcao.NfeRecepcao(); break;
                        case 23: pServico = new wsCEHRecepcao.NfeRecepcao(); break;
                        case 29: pServico = new wsBAHRecepcao.NfeRecepcao(); break;
                        case 53: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 26: pServico = new wsPEHRecepcao.NfeRecepcao(); break;
                        case 11: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 13: pServico = new wsAMHRecepcao.NfeRecepcao(); break;

                        case 15: pServico = new wsVNHRecepcao.NfeRecepcao(); break;
                        case 21: pServico = new wsVNHRecepcao.NfeRecepcao(); break;
                        case 22: pServico = new wsVNHRecepcao.NfeRecepcao(); break;
                        case 24: pServico = new wsVNHRecepcao.NfeRecepcao(); break;
                        case 32: pServico = new wsVNHRecepcao.NfeRecepcao(); break;

                        case 42: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 17: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 28: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 33: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 12: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 27: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 16: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 25: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                        case 14: pServico = new wsVRHRecepcao.NfeRecepcao(); break;
                    }
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
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            if (Empresa.Configuracoes[emp].tpAmb == TipoAmbiente.taProducao/*1*/)
            {
                if (Empresa.Configuracoes[emp].tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANPRetRecepcao.NfeRetRecepcao(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (Empresa.Configuracoes[emp].UFCod)
                    {
                        case 51: pServico = new wsMTPRetRecepcao.NfeRetRecepcao(); break;
                        case 43: pServico = new wsRSPRetRecepcao.NfeRetRecepcao(); break;
                        case 31: pServico = new wsMGPRetRecepcao.NfeRetRecepcao(); break;
                        case 35: pServico = new wsSPPRetRecepcao.NfeRetRecepcao(); break;
                        case 50: pServico = new wsMSPRetRecepcao.NfeRetRecepcao(); break;
                        case 52: pServico = new wsGOPRetRecepcao.NfeRetRecepcao(); break;
                        case 41: pServico = new wsPRPRetRecepcao.NfeRetRecepcaoService(); break;
                        case 23: pServico = new wsCEPRetRecepcao.NfeRetRecepcao(); break;
                        case 29: pServico = new wsBAPRetRecepcao.NfeRetRecepcao(); break;
                        case 53: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 26: pServico = new wsPEPRetRecepcao.NfeRetRecepcao(); break;
                        case 11: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 13: pServico = new wsAMPRetRecepcao.NfeRetRecepcao(); break;

                        case 15: pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); break;
                        case 21: pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); break;
                        case 22: pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); break;
                        case 24: pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); break;
                        case 32: pServico = new wsVNPRetRecepcao.NfeRetRecepcao(); break;

                        case 42: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 17: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 28: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 33: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 12: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 27: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 16: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 25: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                        case 14: pServico = new wsVRPRetRecepcao.NfeRetRecepcao(); break;
                    }
            }
            else if (Empresa.Configuracoes[emp].tpAmb == TipoAmbiente.taHomologacao/*2*/)
            {
                if (Empresa.Configuracoes[emp].tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANHRetRecepcao.NfeRetRecepcao(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (Empresa.Configuracoes[emp].UFCod)
                    {
                        case 51: pServico = new wsMTHRetRecepcao.NfeRetRecepcao(); break;
                        case 43: pServico = new wsRSHRetRecepcao.NfeRetRecepcao(); break;
                        case 31: pServico = new wsMGHRetRecepcao.NfeRetRecepcao(); break;
                        case 35: pServico = new wsSPHRetRecepcao.NfeRetRecepcao(); break;
                        case 50: pServico = new wsMSHRetRecepcao.NfeRetRecepcao(); break;
                        case 52: pServico = new wsGOHRetRecepcao.NfeRetRecepcao(); break;
                        case 41: pServico = new wsPRHRetRecepcao.NfeRetRecepcaoService(); break;
                        case 23: pServico = new wsCEHRetRecepcao.NfeRetRecepcao(); break;
                        case 29: pServico = new wsBAHRetRecepcao.NfeRetRecepcao(); break;
                        case 53: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 26: pServico = new wsPEHRetRecepcao.NfeRetRecepcao(); break;
                        case 11: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 13: pServico = new wsAMHRetRecepcao.NfeRetRecepcao(); break;

                        case 15: pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); break;
                        case 21: pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); break;
                        case 22: pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); break;
                        case 24: pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); break;
                        case 32: pServico = new wsVNHRetRecepcao.NfeRetRecepcao(); break;

                        case 42: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 17: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 28: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 33: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 12: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 27: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 16: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 25: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                        case 14: pServico = new wsVRHRetRecepcao.NfeRetRecepcao(); break;
                    }
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
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANPStatusServico.NfeStatusServico(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (oParam.UFCod)
                    {
                        case 51: pServico = new wsMTPStatusServico.NfeStatusServico(); break;
                        case 43: pServico = new wsRSPStatusServico.NfeStatusServico(); break;
                        case 31: pServico = new wsMGPStatusServico.NfeStatusServico(); break;
                        case 35: pServico = new wsSPPStatusServico.NfeStatusServico(); break;
                        case 50: pServico = new wsMSPStatusServico.NfeStatusServico(); break;
                        case 52: pServico = new wsGOPStatusServico.NfeStatusServico(); break;
                        case 41: pServico = new wsPRPStatusServico.NfeStatusServico(); break;
                        case 23: pServico = new wsCEPStatusServico.NfeStatusServico(); break;
                        case 29: pServico = new wsBAPStatusServico.NfeStatusServico(); break;
                        case 53: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 26: pServico = new wsPEPStatusServico.NfeStatusServico(); break;
                        case 11: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 13: pServico = new wsAMPStatusServico.NfeStatusServico(); break;

                        case 15: pServico = new wsVNPStatusServico.NfeStatusServico(); break;
                        case 21: pServico = new wsVNPStatusServico.NfeStatusServico(); break;
                        case 22: pServico = new wsVNPStatusServico.NfeStatusServico(); break;
                        case 24: pServico = new wsVNPStatusServico.NfeStatusServico(); break;
                        case 32: pServico = new wsVNPStatusServico.NfeStatusServico(); break;

                        case 42: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 17: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 28: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 33: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 12: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 27: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 16: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 25: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                        case 14: pServico = new wsVRPStatusServico.NfeStatusServico(); break;
                    }
            }
            else if (oParam.tpAmb == TipoAmbiente.taHomologacao)
            {
                if (oParam.tpEmis == TipoEmissao.teSCAN) { pServico = new wsSCANHStatusServico.NfeStatusServico(); } //Contingência SCAN Ambiente Nascional
                else
                    switch (oParam.UFCod)
                    {
                        case 51: pServico = new wsMTHStatusServico.NfeStatusServico(); break;
                        case 43: pServico = new wsRSHStatusServico.NfeStatusServico(); break;
                        case 31: pServico = new wsMGHStatusServico.NfeStatusServico(); break;
                        case 35: pServico = new wsSPHStatusServico.NfeStatusServico(); break;
                        case 50: pServico = new wsMSHStatusServico.NfeStatusServico(); break;
                        case 52: pServico = new wsGOHStatusServico.NfeStatusServico(); break;
                        case 41: pServico = new wsPRHStatusServico.NfeStatusServico(); break;
                        case 23: pServico = new wsCEHStatusServico.NfeStatusServico(); break;
                        case 29: pServico = new wsBAHStatusServico.NfeStatusServico(); break;
                        case 53: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 26: pServico = new wsPEHStatusServico.NfeStatusServico(); break;
                        case 11: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 13: pServico = new wsAMHStatusServico.NfeStatusServico(); break;

                        case 15: pServico = new wsVNHStatusServico.NfeStatusServico(); break;
                        case 21: pServico = new wsVNHStatusServico.NfeStatusServico(); break;
                        case 22: pServico = new wsVNHStatusServico.NfeStatusServico(); break;
                        case 24: pServico = new wsVNHStatusServico.NfeStatusServico(); break;
                        case 32: pServico = new wsVNHStatusServico.NfeStatusServico(); break;

                        case 42: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 17: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 28: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 33: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 12: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 27: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 16: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 25: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                        case 14: pServico = new wsVRHStatusServico.NfeStatusServico(); break;
                    }
            }
        }
        #endregion

        #endregion
    }
}

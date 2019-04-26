﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace NFe.Components.PSmaraPD_204 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="nfseSOAPSoapBinding", Namespace="http://nfse.abrasf.org.br")]
    public partial class NfseWSService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CancelarNfseOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarLoteRpsOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarNfseServicoPrestadoOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarNfseServicoTomadoOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarNfsePorFaixaOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarNfsePorRpsOperationCompleted;
        
        private System.Threading.SendOrPostCallback RecepcionarLoteRpsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GerarNfseOperationCompleted;
        
        private System.Threading.SendOrPostCallback SubstituirNfseOperationCompleted;
        
        private System.Threading.SendOrPostCallback RecepcionarLoteRpsSincronoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public NfseWSService() {
            this.Url = global::NFe.Components.Properties.Settings.Default.NFe_Components_PSertaozinhoSP_NfseWSService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event CancelarNfseCompletedEventHandler CancelarNfseCompleted;
        
        /// <remarks/>
        public event ConsultarLoteRpsCompletedEventHandler ConsultarLoteRpsCompleted;
        
        /// <remarks/>
        public event ConsultarNfseServicoPrestadoCompletedEventHandler ConsultarNfseServicoPrestadoCompleted;
        
        /// <remarks/>
        public event ConsultarNfseServicoTomadoCompletedEventHandler ConsultarNfseServicoTomadoCompleted;
        
        /// <remarks/>
        public event ConsultarNfsePorFaixaCompletedEventHandler ConsultarNfsePorFaixaCompleted;
        
        /// <remarks/>
        public event ConsultarNfsePorRpsCompletedEventHandler ConsultarNfsePorRpsCompleted;
        
        /// <remarks/>
        public event RecepcionarLoteRpsCompletedEventHandler RecepcionarLoteRpsCompleted;
        
        /// <remarks/>
        public event GerarNfseCompletedEventHandler GerarNfseCompleted;
        
        /// <remarks/>
        public event SubstituirNfseCompletedEventHandler SubstituirNfseCompleted;
        
        /// <remarks/>
        public event RecepcionarLoteRpsSincronoCompletedEventHandler RecepcionarLoteRpsSincronoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/CancelarNfse", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CancelarNfseResponse", Namespace="http://nfse.abrasf.org.br")]
        public output CancelarNfse([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input CancelarNfseRequest) {
            object[] results = this.Invoke("CancelarNfse", new object[] {
                        CancelarNfseRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void CancelarNfseAsync(input CancelarNfseRequest) {
            this.CancelarNfseAsync(CancelarNfseRequest, null);
        }
        
        /// <remarks/>
        public void CancelarNfseAsync(input CancelarNfseRequest, object userState) {
            if ((this.CancelarNfseOperationCompleted == null)) {
                this.CancelarNfseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCancelarNfseOperationCompleted);
            }
            this.InvokeAsync("CancelarNfse", new object[] {
                        CancelarNfseRequest}, this.CancelarNfseOperationCompleted, userState);
        }
        
        private void OnCancelarNfseOperationCompleted(object arg) {
            if ((this.CancelarNfseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CancelarNfseCompleted(this, new CancelarNfseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/ConsultarLoteRps", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarLoteRpsResponse", Namespace="http://nfse.abrasf.org.br")]
        public output ConsultarLoteRps([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input ConsultarLoteRpsRequest) {
            object[] results = this.Invoke("ConsultarLoteRps", new object[] {
                        ConsultarLoteRpsRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarLoteRpsAsync(input ConsultarLoteRpsRequest) {
            this.ConsultarLoteRpsAsync(ConsultarLoteRpsRequest, null);
        }
        
        /// <remarks/>
        public void ConsultarLoteRpsAsync(input ConsultarLoteRpsRequest, object userState) {
            if ((this.ConsultarLoteRpsOperationCompleted == null)) {
                this.ConsultarLoteRpsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarLoteRpsOperationCompleted);
            }
            this.InvokeAsync("ConsultarLoteRps", new object[] {
                        ConsultarLoteRpsRequest}, this.ConsultarLoteRpsOperationCompleted, userState);
        }
        
        private void OnConsultarLoteRpsOperationCompleted(object arg) {
            if ((this.ConsultarLoteRpsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarLoteRpsCompleted(this, new ConsultarLoteRpsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/ConsultarNfseServicoPrestado", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarNfseServicoPrestadoResponse", Namespace="http://nfse.abrasf.org.br")]
        public output ConsultarNfseServicoPrestado([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input ConsultarNfseServicoPrestadoRequest) {
            object[] results = this.Invoke("ConsultarNfseServicoPrestado", new object[] {
                        ConsultarNfseServicoPrestadoRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarNfseServicoPrestadoAsync(input ConsultarNfseServicoPrestadoRequest) {
            this.ConsultarNfseServicoPrestadoAsync(ConsultarNfseServicoPrestadoRequest, null);
        }
        
        /// <remarks/>
        public void ConsultarNfseServicoPrestadoAsync(input ConsultarNfseServicoPrestadoRequest, object userState) {
            if ((this.ConsultarNfseServicoPrestadoOperationCompleted == null)) {
                this.ConsultarNfseServicoPrestadoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarNfseServicoPrestadoOperationCompleted);
            }
            this.InvokeAsync("ConsultarNfseServicoPrestado", new object[] {
                        ConsultarNfseServicoPrestadoRequest}, this.ConsultarNfseServicoPrestadoOperationCompleted, userState);
        }
        
        private void OnConsultarNfseServicoPrestadoOperationCompleted(object arg) {
            if ((this.ConsultarNfseServicoPrestadoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarNfseServicoPrestadoCompleted(this, new ConsultarNfseServicoPrestadoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/ConsultarNfseServicoTomado", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarNfseServicoTomadoResponse", Namespace="http://nfse.abrasf.org.br")]
        public output ConsultarNfseServicoTomado([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input ConsultarNfseServicoTomadoRequest) {
            object[] results = this.Invoke("ConsultarNfseServicoTomado", new object[] {
                        ConsultarNfseServicoTomadoRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarNfseServicoTomadoAsync(input ConsultarNfseServicoTomadoRequest) {
            this.ConsultarNfseServicoTomadoAsync(ConsultarNfseServicoTomadoRequest, null);
        }
        
        /// <remarks/>
        public void ConsultarNfseServicoTomadoAsync(input ConsultarNfseServicoTomadoRequest, object userState) {
            if ((this.ConsultarNfseServicoTomadoOperationCompleted == null)) {
                this.ConsultarNfseServicoTomadoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarNfseServicoTomadoOperationCompleted);
            }
            this.InvokeAsync("ConsultarNfseServicoTomado", new object[] {
                        ConsultarNfseServicoTomadoRequest}, this.ConsultarNfseServicoTomadoOperationCompleted, userState);
        }
        
        private void OnConsultarNfseServicoTomadoOperationCompleted(object arg) {
            if ((this.ConsultarNfseServicoTomadoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarNfseServicoTomadoCompleted(this, new ConsultarNfseServicoTomadoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/ConsultarNfsePorFaixa", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarNfsePorFaixaResponse", Namespace="http://nfse.abrasf.org.br")]
        public output ConsultarNfsePorFaixa([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input ConsultarNfsePorFaixaRequest) {
            object[] results = this.Invoke("ConsultarNfsePorFaixa", new object[] {
                        ConsultarNfsePorFaixaRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarNfsePorFaixaAsync(input ConsultarNfsePorFaixaRequest) {
            this.ConsultarNfsePorFaixaAsync(ConsultarNfsePorFaixaRequest, null);
        }
        
        /// <remarks/>
        public void ConsultarNfsePorFaixaAsync(input ConsultarNfsePorFaixaRequest, object userState) {
            if ((this.ConsultarNfsePorFaixaOperationCompleted == null)) {
                this.ConsultarNfsePorFaixaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarNfsePorFaixaOperationCompleted);
            }
            this.InvokeAsync("ConsultarNfsePorFaixa", new object[] {
                        ConsultarNfsePorFaixaRequest}, this.ConsultarNfsePorFaixaOperationCompleted, userState);
        }
        
        private void OnConsultarNfsePorFaixaOperationCompleted(object arg) {
            if ((this.ConsultarNfsePorFaixaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarNfsePorFaixaCompleted(this, new ConsultarNfsePorFaixaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/ConsultarNfsePorRps", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarNfsePorRpsResponse", Namespace="http://nfse.abrasf.org.br")]
        public output ConsultarNfsePorRps([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input ConsultarNfsePorRpsRequest) {
            object[] results = this.Invoke("ConsultarNfsePorRps", new object[] {
                        ConsultarNfsePorRpsRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarNfsePorRpsAsync(input ConsultarNfsePorRpsRequest) {
            this.ConsultarNfsePorRpsAsync(ConsultarNfsePorRpsRequest, null);
        }
        
        /// <remarks/>
        public void ConsultarNfsePorRpsAsync(input ConsultarNfsePorRpsRequest, object userState) {
            if ((this.ConsultarNfsePorRpsOperationCompleted == null)) {
                this.ConsultarNfsePorRpsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarNfsePorRpsOperationCompleted);
            }
            this.InvokeAsync("ConsultarNfsePorRps", new object[] {
                        ConsultarNfsePorRpsRequest}, this.ConsultarNfsePorRpsOperationCompleted, userState);
        }
        
        private void OnConsultarNfsePorRpsOperationCompleted(object arg) {
            if ((this.ConsultarNfsePorRpsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarNfsePorRpsCompleted(this, new ConsultarNfsePorRpsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/RecepcionarLoteRps", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecepcionarLoteRpsResponse", Namespace="http://nfse.abrasf.org.br")]
        public output RecepcionarLoteRps([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input RecepcionarLoteRpsRequest) {
            object[] results = this.Invoke("RecepcionarLoteRps", new object[] {
                        RecepcionarLoteRpsRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void RecepcionarLoteRpsAsync(input RecepcionarLoteRpsRequest) {
            this.RecepcionarLoteRpsAsync(RecepcionarLoteRpsRequest, null);
        }
        
        /// <remarks/>
        public void RecepcionarLoteRpsAsync(input RecepcionarLoteRpsRequest, object userState) {
            if ((this.RecepcionarLoteRpsOperationCompleted == null)) {
                this.RecepcionarLoteRpsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecepcionarLoteRpsOperationCompleted);
            }
            this.InvokeAsync("RecepcionarLoteRps", new object[] {
                        RecepcionarLoteRpsRequest}, this.RecepcionarLoteRpsOperationCompleted, userState);
        }
        
        private void OnRecepcionarLoteRpsOperationCompleted(object arg) {
            if ((this.RecepcionarLoteRpsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecepcionarLoteRpsCompleted(this, new RecepcionarLoteRpsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/GerarNfse", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GerarNfseResponse", Namespace="http://nfse.abrasf.org.br")]
        public output GerarNfse([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input GerarNfseRequest) {
            object[] results = this.Invoke("GerarNfse", new object[] {
                        GerarNfseRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void GerarNfseAsync(input GerarNfseRequest) {
            this.GerarNfseAsync(GerarNfseRequest, null);
        }
        
        /// <remarks/>
        public void GerarNfseAsync(input GerarNfseRequest, object userState) {
            if ((this.GerarNfseOperationCompleted == null)) {
                this.GerarNfseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGerarNfseOperationCompleted);
            }
            this.InvokeAsync("GerarNfse", new object[] {
                        GerarNfseRequest}, this.GerarNfseOperationCompleted, userState);
        }
        
        private void OnGerarNfseOperationCompleted(object arg) {
            if ((this.GerarNfseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GerarNfseCompleted(this, new GerarNfseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/SubstituirNfse", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SubstituirNfseResponse", Namespace="http://nfse.abrasf.org.br")]
        public output SubstituirNfse([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input SubstituirNfseRequest) {
            object[] results = this.Invoke("SubstituirNfse", new object[] {
                        SubstituirNfseRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void SubstituirNfseAsync(input SubstituirNfseRequest) {
            this.SubstituirNfseAsync(SubstituirNfseRequest, null);
        }
        
        /// <remarks/>
        public void SubstituirNfseAsync(input SubstituirNfseRequest, object userState) {
            if ((this.SubstituirNfseOperationCompleted == null)) {
                this.SubstituirNfseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubstituirNfseOperationCompleted);
            }
            this.InvokeAsync("SubstituirNfse", new object[] {
                        SubstituirNfseRequest}, this.SubstituirNfseOperationCompleted, userState);
        }
        
        private void OnSubstituirNfseOperationCompleted(object arg) {
            if ((this.SubstituirNfseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubstituirNfseCompleted(this, new SubstituirNfseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nfse.abrasf.org.br/RecepcionarLoteRpsSincrono", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecepcionarLoteRpsSincronoResponse", Namespace="http://nfse.abrasf.org.br")]
        public output RecepcionarLoteRpsSincrono([System.Xml.Serialization.XmlElementAttribute(Namespace="http://nfse.abrasf.org.br")] input RecepcionarLoteRpsSincronoRequest) {
            object[] results = this.Invoke("RecepcionarLoteRpsSincrono", new object[] {
                        RecepcionarLoteRpsSincronoRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void RecepcionarLoteRpsSincronoAsync(input RecepcionarLoteRpsSincronoRequest) {
            this.RecepcionarLoteRpsSincronoAsync(RecepcionarLoteRpsSincronoRequest, null);
        }
        
        /// <remarks/>
        public void RecepcionarLoteRpsSincronoAsync(input RecepcionarLoteRpsSincronoRequest, object userState) {
            if ((this.RecepcionarLoteRpsSincronoOperationCompleted == null)) {
                this.RecepcionarLoteRpsSincronoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecepcionarLoteRpsSincronoOperationCompleted);
            }
            this.InvokeAsync("RecepcionarLoteRpsSincrono", new object[] {
                        RecepcionarLoteRpsSincronoRequest}, this.RecepcionarLoteRpsSincronoOperationCompleted, userState);
        }
        
        private void OnRecepcionarLoteRpsSincronoOperationCompleted(object arg) {
            if ((this.RecepcionarLoteRpsSincronoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecepcionarLoteRpsSincronoCompleted(this, new RecepcionarLoteRpsSincronoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://nfse.abrasf.org.br")]
    public partial class input {
        
        private string nfseCabecMsgField;
        
        private string nfseDadosMsgField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nfseCabecMsg {
            get {
                return this.nfseCabecMsgField;
            }
            set {
                this.nfseCabecMsgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nfseDadosMsg {
            get {
                return this.nfseDadosMsgField;
            }
            set {
                this.nfseDadosMsgField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://nfse.abrasf.org.br")]
    public partial class output {
        
        private string outputXMLField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string outputXML {
            get {
                return this.outputXMLField;
            }
            set {
                this.outputXMLField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void CancelarNfseCompletedEventHandler(object sender, CancelarNfseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CancelarNfseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CancelarNfseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void ConsultarLoteRpsCompletedEventHandler(object sender, ConsultarLoteRpsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarLoteRpsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarLoteRpsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void ConsultarNfseServicoPrestadoCompletedEventHandler(object sender, ConsultarNfseServicoPrestadoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarNfseServicoPrestadoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarNfseServicoPrestadoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void ConsultarNfseServicoTomadoCompletedEventHandler(object sender, ConsultarNfseServicoTomadoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarNfseServicoTomadoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarNfseServicoTomadoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void ConsultarNfsePorFaixaCompletedEventHandler(object sender, ConsultarNfsePorFaixaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarNfsePorFaixaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarNfsePorFaixaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void ConsultarNfsePorRpsCompletedEventHandler(object sender, ConsultarNfsePorRpsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarNfsePorRpsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarNfsePorRpsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void RecepcionarLoteRpsCompletedEventHandler(object sender, RecepcionarLoteRpsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecepcionarLoteRpsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecepcionarLoteRpsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void GerarNfseCompletedEventHandler(object sender, GerarNfseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GerarNfseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GerarNfseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void SubstituirNfseCompletedEventHandler(object sender, SubstituirNfseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubstituirNfseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SubstituirNfseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void RecepcionarLoteRpsSincronoCompletedEventHandler(object sender, RecepcionarLoteRpsSincronoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecepcionarLoteRpsSincronoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecepcionarLoteRpsSincronoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((output)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
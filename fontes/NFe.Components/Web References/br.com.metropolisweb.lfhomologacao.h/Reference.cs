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

namespace NFe.Components.br.com.metropolisweb.lfhomologacao.h {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="NfseSoapPortBinding", Namespace="http://impl.endpoint.nfse.ws.webservicenfse.edza.com.br/")]
    public partial class Nfse : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ConsultarSituacaoLoteRpsOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarLoteRpsOperationCompleted;
        
        private System.Threading.SendOrPostCallback CancelarNfseOperationCompleted;
        
        private System.Threading.SendOrPostCallback RecepcionarLoteRpsOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarNfseOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarNfsePorRpsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Nfse() {
            this.Url = global::NFe.Components.Properties.Settings.Default.NFe_Components_br_com_metropolisweb_lfhomologacao_h_Nfse;
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
        public event ConsultarSituacaoLoteRpsCompletedEventHandler ConsultarSituacaoLoteRpsCompleted;
        
        /// <remarks/>
        public event ConsultarLoteRpsCompletedEventHandler ConsultarLoteRpsCompleted;
        
        /// <remarks/>
        public event CancelarNfseCompletedEventHandler CancelarNfseCompleted;
        
        /// <remarks/>
        public event RecepcionarLoteRpsCompletedEventHandler RecepcionarLoteRpsCompleted;
        
        /// <remarks/>
        public event ConsultarNfseCompletedEventHandler ConsultarNfseCompleted;
        
        /// <remarks/>
        public event ConsultarNfsePorRpsCompletedEventHandler ConsultarNfsePorRpsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", ResponseNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarSituacaoLoteRpsResponse", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public output ConsultarSituacaoLoteRps([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] input ConsultarSituacaoLoteRpsRequest) {
            object[] results = this.Invoke("ConsultarSituacaoLoteRps", new object[] {
                        ConsultarSituacaoLoteRpsRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarSituacaoLoteRpsAsync(input ConsultarSituacaoLoteRpsRequest) {
            this.ConsultarSituacaoLoteRpsAsync(ConsultarSituacaoLoteRpsRequest, null);
        }
        
        /// <remarks/>
        public void ConsultarSituacaoLoteRpsAsync(input ConsultarSituacaoLoteRpsRequest, object userState) {
            if ((this.ConsultarSituacaoLoteRpsOperationCompleted == null)) {
                this.ConsultarSituacaoLoteRpsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarSituacaoLoteRpsOperationCompleted);
            }
            this.InvokeAsync("ConsultarSituacaoLoteRps", new object[] {
                        ConsultarSituacaoLoteRpsRequest}, this.ConsultarSituacaoLoteRpsOperationCompleted, userState);
        }
        
        private void OnConsultarSituacaoLoteRpsOperationCompleted(object arg) {
            if ((this.ConsultarSituacaoLoteRpsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarSituacaoLoteRpsCompleted(this, new ConsultarSituacaoLoteRpsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", ResponseNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarLoteRpsResponse", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public output ConsultarLoteRps([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] input ConsultarLoteRpsRequest) {
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", ResponseNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CancelarNfseResponse", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public output CancelarNfse([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] input CancelarNfseRequest) {
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", ResponseNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecepcionarLoteRpsResponse", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public output RecepcionarLoteRps([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] input RecepcionarLoteRpsRequest) {
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", ResponseNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarNfseResponse", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public output ConsultarNfse([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] input ConsultarNfseRequest) {
            object[] results = this.Invoke("ConsultarNfse", new object[] {
                        ConsultarNfseRequest});
            return ((output)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarNfseAsync(input ConsultarNfseRequest) {
            this.ConsultarNfseAsync(ConsultarNfseRequest, null);
        }
        
        /// <remarks/>
        public void ConsultarNfseAsync(input ConsultarNfseRequest, object userState) {
            if ((this.ConsultarNfseOperationCompleted == null)) {
                this.ConsultarNfseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarNfseOperationCompleted);
            }
            this.InvokeAsync("ConsultarNfse", new object[] {
                        ConsultarNfseRequest}, this.ConsultarNfseOperationCompleted, userState);
        }
        
        private void OnConsultarNfseOperationCompleted(object arg) {
            if ((this.ConsultarNfseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarNfseCompleted(this, new ConsultarNfseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", ResponseNamespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConsultarNfsePorRpsResponse", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public output ConsultarNfsePorRps([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] input ConsultarNfsePorRpsRequest) {
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://endpoint.nfse.ws.webservicenfse.edza.com.br/")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ConsultarSituacaoLoteRpsCompletedEventHandler(object sender, ConsultarSituacaoLoteRpsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarSituacaoLoteRpsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarSituacaoLoteRpsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ConsultarLoteRpsCompletedEventHandler(object sender, ConsultarLoteRpsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void CancelarNfseCompletedEventHandler(object sender, CancelarNfseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void RecepcionarLoteRpsCompletedEventHandler(object sender, RecepcionarLoteRpsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ConsultarNfseCompletedEventHandler(object sender, ConsultarNfseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarNfseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarNfseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ConsultarNfsePorRpsCompletedEventHandler(object sender, ConsultarNfsePorRpsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
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
}

#pragma warning restore 1591
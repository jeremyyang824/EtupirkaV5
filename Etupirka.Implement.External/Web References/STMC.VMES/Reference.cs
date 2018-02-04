﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace Etupirka.Implement.External.STMC.VMES {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PickChangeSoap", Namespace="http://stmc.mes.pickchange.com")]
    public partial class PickChange : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback PickChangeInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback PickMultipleChangeOperationCompleted;
        
        private System.Threading.SendOrPostCallback PickInsertInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback PickDeleteInfoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PickChange() {
            this.Url = global::Etupirka.Implement.External.Properties.Settings.Default.Etupirka_Implement_External_STMC_VMES_PickChange;
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
        public event PickChangeInfoCompletedEventHandler PickChangeInfoCompleted;
        
        /// <remarks/>
        public event PickMultipleChangeCompletedEventHandler PickMultipleChangeCompleted;
        
        /// <remarks/>
        public event PickInsertInfoCompletedEventHandler PickInsertInfoCompleted;
        
        /// <remarks/>
        public event PickDeleteInfoCompletedEventHandler PickDeleteInfoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://stmc.mes.pickchange.com/PickChangeInfo", RequestNamespace="http://stmc.mes.pickchange.com", ResponseNamespace="http://stmc.mes.pickchange.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PickChangeInfo(string monumber, string molinenumber, string osnumber, string pointofuse, string workcenter, string workcenterDes, string worktime, string preparetime) {
            object[] results = this.Invoke("PickChangeInfo", new object[] {
                        monumber,
                        molinenumber,
                        osnumber,
                        pointofuse,
                        workcenter,
                        workcenterDes,
                        worktime,
                        preparetime});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PickChangeInfoAsync(string monumber, string molinenumber, string osnumber, string pointofuse, string workcenter, string workcenterDes, string worktime, string preparetime) {
            this.PickChangeInfoAsync(monumber, molinenumber, osnumber, pointofuse, workcenter, workcenterDes, worktime, preparetime, null);
        }
        
        /// <remarks/>
        public void PickChangeInfoAsync(string monumber, string molinenumber, string osnumber, string pointofuse, string workcenter, string workcenterDes, string worktime, string preparetime, object userState) {
            if ((this.PickChangeInfoOperationCompleted == null)) {
                this.PickChangeInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPickChangeInfoOperationCompleted);
            }
            this.InvokeAsync("PickChangeInfo", new object[] {
                        monumber,
                        molinenumber,
                        osnumber,
                        pointofuse,
                        workcenter,
                        workcenterDes,
                        worktime,
                        preparetime}, this.PickChangeInfoOperationCompleted, userState);
        }
        
        private void OnPickChangeInfoOperationCompleted(object arg) {
            if ((this.PickChangeInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PickChangeInfoCompleted(this, new PickChangeInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://stmc.mes.pickchange.com/PickMultipleChange", RequestNamespace="http://stmc.mes.pickchange.com", ResponseNamespace="http://stmc.mes.pickchange.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PickMultipleChange(string monumber, string molinenumber) {
            object[] results = this.Invoke("PickMultipleChange", new object[] {
                        monumber,
                        molinenumber});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PickMultipleChangeAsync(string monumber, string molinenumber) {
            this.PickMultipleChangeAsync(monumber, molinenumber, null);
        }
        
        /// <remarks/>
        public void PickMultipleChangeAsync(string monumber, string molinenumber, object userState) {
            if ((this.PickMultipleChangeOperationCompleted == null)) {
                this.PickMultipleChangeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPickMultipleChangeOperationCompleted);
            }
            this.InvokeAsync("PickMultipleChange", new object[] {
                        monumber,
                        molinenumber}, this.PickMultipleChangeOperationCompleted, userState);
        }
        
        private void OnPickMultipleChangeOperationCompleted(object arg) {
            if ((this.PickMultipleChangeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PickMultipleChangeCompleted(this, new PickMultipleChangeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://stmc.mes.pickchange.com/PickInsertInfo", RequestNamespace="http://stmc.mes.pickchange.com", ResponseNamespace="http://stmc.mes.pickchange.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PickInsertInfo(string monumber, string molinenumber, string osnumber, string pointofuse, string workcenter, string workcenterDes, string worktime, string preparetime) {
            object[] results = this.Invoke("PickInsertInfo", new object[] {
                        monumber,
                        molinenumber,
                        osnumber,
                        pointofuse,
                        workcenter,
                        workcenterDes,
                        worktime,
                        preparetime});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PickInsertInfoAsync(string monumber, string molinenumber, string osnumber, string pointofuse, string workcenter, string workcenterDes, string worktime, string preparetime) {
            this.PickInsertInfoAsync(monumber, molinenumber, osnumber, pointofuse, workcenter, workcenterDes, worktime, preparetime, null);
        }
        
        /// <remarks/>
        public void PickInsertInfoAsync(string monumber, string molinenumber, string osnumber, string pointofuse, string workcenter, string workcenterDes, string worktime, string preparetime, object userState) {
            if ((this.PickInsertInfoOperationCompleted == null)) {
                this.PickInsertInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPickInsertInfoOperationCompleted);
            }
            this.InvokeAsync("PickInsertInfo", new object[] {
                        monumber,
                        molinenumber,
                        osnumber,
                        pointofuse,
                        workcenter,
                        workcenterDes,
                        worktime,
                        preparetime}, this.PickInsertInfoOperationCompleted, userState);
        }
        
        private void OnPickInsertInfoOperationCompleted(object arg) {
            if ((this.PickInsertInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PickInsertInfoCompleted(this, new PickInsertInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://stmc.mes.pickchange.com/PickDeleteInfo", RequestNamespace="http://stmc.mes.pickchange.com", ResponseNamespace="http://stmc.mes.pickchange.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PickDeleteInfo(string monumber, string molinenumber, string osnumber) {
            object[] results = this.Invoke("PickDeleteInfo", new object[] {
                        monumber,
                        molinenumber,
                        osnumber});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PickDeleteInfoAsync(string monumber, string molinenumber, string osnumber) {
            this.PickDeleteInfoAsync(monumber, molinenumber, osnumber, null);
        }
        
        /// <remarks/>
        public void PickDeleteInfoAsync(string monumber, string molinenumber, string osnumber, object userState) {
            if ((this.PickDeleteInfoOperationCompleted == null)) {
                this.PickDeleteInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPickDeleteInfoOperationCompleted);
            }
            this.InvokeAsync("PickDeleteInfo", new object[] {
                        monumber,
                        molinenumber,
                        osnumber}, this.PickDeleteInfoOperationCompleted, userState);
        }
        
        private void OnPickDeleteInfoOperationCompleted(object arg) {
            if ((this.PickDeleteInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PickDeleteInfoCompleted(this, new PickDeleteInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void PickChangeInfoCompletedEventHandler(object sender, PickChangeInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PickChangeInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PickChangeInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void PickMultipleChangeCompletedEventHandler(object sender, PickMultipleChangeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PickMultipleChangeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PickMultipleChangeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void PickInsertInfoCompletedEventHandler(object sender, PickInsertInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PickInsertInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PickInsertInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void PickDeleteInfoCompletedEventHandler(object sender, PickDeleteInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PickDeleteInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PickDeleteInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
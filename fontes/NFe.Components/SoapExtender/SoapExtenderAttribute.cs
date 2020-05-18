using System;
using System.Web.Services.Protocols;

namespace NFe.Components.SoapExtender
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SoapExtenderAttribute: SoapExtensionAttribute
    {
        #region Private Fields

        private int priority;

        #endregion Private Fields

        #region Public Properties

        public override Type ExtensionType => typeof(SoapExtenderExtension);

        public override int Priority
        {
            get => priority;
            set => priority = value;
        }

        #endregion Public Properties
    }
}
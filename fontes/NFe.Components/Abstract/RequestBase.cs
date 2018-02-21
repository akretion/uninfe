using NFe.Components.Interface;
using System;
using System.Net;
using System.Reflection;

namespace NFe.Components.Abstract
{
    public abstract class RequestBase : IRequest
    {
        #region Public Properties

        /// <summary>
        /// Proxy para ser utilizado na requisição, pode ser nulo
        /// </summary>
        public IWebProxy Proxy { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            Proxy = null;
        }

        /// <summary>
        /// Evita o erro de servidor cometeu uma violação de protocolo.
        /// </summary>
        /// <seealso cref="https://msdn.microsoft.com/pt-br/library/system.net.configuration.httpwebrequestelement.useunsafeheaderparsing%28v=vs.110%29.aspx"/>
        public void SetAllowUnsafeHeaderParsing20()
        {
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    object anInstance = aSettingsType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                        }
                    }
                }
            }
        }

        #endregion Public Methods
    }
}
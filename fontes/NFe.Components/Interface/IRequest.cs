using System;
using System.Net;

namespace NFe.Components.Interface
{
    public interface IRequest : IDisposable
    {
        #region Properties

        /// <summary>
        /// Proxy para ser utilizado na requisição, pode ser nulo
        /// </summary>
        IWebProxy Proxy { get; set; }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Evita o erro de servidor cometeu uma violação de protocolo.
        /// </summary>
        /// <seealso cref="https://msdn.microsoft.com/pt-br/library/system.net.configuration.httpwebrequestelement.useunsafeheaderparsing%28v=vs.110%29.aspx"/>
        void SetAllowUnsafeHeaderParsing20();
        
        #endregion Methods
    }
}
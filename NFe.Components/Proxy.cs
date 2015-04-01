using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components
{
    public class Proxy
    {
        #region DefinirProxy()
        /// <summary>
        /// Efetua as definições do proxy
        /// </summary>
        /// <returns>Retorna as definições do Proxy</returns>
        /// <param name="servidor">Endereço do servidor de proxy</param>
        /// <param name="usuario">Usuário para autenticação no servidor de proxy</param>
        /// <param name="senha">Senha do usuário para autenticação no servidor de proxy</param>
        /// <param name="porta">Porta de comunicação do servidor proxy</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 29/09/2009
        /// </remarks>
        public static System.Net.IWebProxy DefinirProxy(string servidor, string usuario, string senha, int porta, bool detectarAutomaticamente = false)
        {
            System.Net.IWebProxy proxy = detectarAutomaticamente ? 
                System.Net.WebRequest.GetSystemWebProxy() 
                : System.Net.WebRequest.DefaultWebProxy;

            if (proxy != null)
            {
                if (!String.IsNullOrEmpty(usuario) && !String.IsNullOrEmpty(senha))
                    proxy.Credentials = new System.Net.NetworkCredential(usuario, senha);
            }

            return proxy;
        }
        #endregion
    }
}

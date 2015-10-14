using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Settings
{
#if DEBUG
    /// <summary>
    /// Classe para mensagem em modo debug, que serão exibidas no output
    /// </summary>
    public sealed class Debug
    {
        #region Constantes
        /// <summary>
        /// Categoria para o modo debug
        /// </summary>
        public const string Category = "UniNFe";
        #endregion

        /// <summary>
        /// Escreve um log no Output
        /// </summary>
        /// <param name="message">mensagem a ser escrita</param>
        public static void WriteLine(string message)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0:HH:mm:ss.fff}=> {1}", DateTime.Now, message), Category);
        }
    }

#endif
}

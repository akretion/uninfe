using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converte uma lista de string em um string separada pelo char definido em separator
        /// </summary>
        /// <param name="strings">array de string que deverá ser concatenado</param>
        /// <param name="separator">Separador de cada índice do array que será concatenado</param>
        /// <param name="offSet">Define até que posição será concatenada dentro do array.</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> strings, char separator, int offSet = 0)
        {
            if(strings.Count() == 0) return "";
            if(offSet <= 0) offSet = strings.Count();

            int i = 0;
            string result = "";

            foreach(var item in strings)
            {
                result += string.Format("{0}{1}", item, separator);
                if(++i >= offSet) break;
            }

            result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
}

using System;
using System.Globalization;

namespace Unimake.Business.DFe.Utility
{
    internal static class Converter
    {
        /// <summary>
        /// Converte um valor vindo do XML em double 
        /// </summary>
        /// <param name="value">valor para conversão</param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            if (value == null)
            {
                //TODO: Marcelo >>> Vai retornar zero por padrão mesmo?
                return 0;
            }

            double.TryParse(value.ToString(),
                            NumberStyles.Number,
                            CultureInfo.InvariantCulture,
                            out var result);
            return result;
        }

        /// <summary>
        /// Converter STRING para ENUM
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="value">String a ser convertida</param>
        /// <returns>Retorna o Enum da string passada como parâmetro</returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

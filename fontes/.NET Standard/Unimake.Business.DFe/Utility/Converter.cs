using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
            if(value == null)
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
    }
}

using System;
using System.Globalization;
using System.IO;

namespace Unimake.Business.DFe.Utility
{
    public static class Converter
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

        /// <summary>
        /// Converter string para MemoryStream com  UTF8 Encoding
        /// </summary>
        /// <param name="contentConvert">Conteúdo a ser convertido</param>
        /// <returns>Conteúdo convertido para MemoryStrem com UTF8 Encoding</returns>
        public static MemoryStream StringToStreamUTF8(string contentConvert)
        {
            var byteArray = new byte[contentConvert.Length];
            var encoding = new System.Text.UTF8Encoding();
            byteArray = encoding.GetBytes(contentConvert);
            var memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

    }
}

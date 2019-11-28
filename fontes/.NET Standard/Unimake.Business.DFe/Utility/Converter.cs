using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Calcular o valor hexadecimal de uma string
        /// </summary>
        /// <param name="input">Valor a ser convertido</param>
        /// <returns>Valor convertido em hexadecimal</returns>
        public static string ToHexadecimal(string input)
        {
            var hexOutput = "";
            var values = input.ToCharArray();
            foreach (var letter in values)
            {
                // Get the integral value of the character.
                var value = Convert.ToInt32(letter);

                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += string.Format("{0:x}", value);
            }

            return hexOutput;
        }

        /// <summary>
        /// Converte conteúdo para HSA1HashData
        /// </summary>
        /// <param name="data">Conteudo a ser convertido</param>
        /// <returns>Conteúdo convertido para SH1HashData</returns>
        public static string ToSHA1HashData(string data)
        {
            return ToSHA1HashData(data, false);
        }

        /// <summary>
        /// Converte conteúdo para HSA1HashData
        /// </summary>
        /// <param name="data">Conteudo a ser convertido</param>
        /// <param name="toUpper">Resultado todo em maiúsculo?</param>
        /// <returns>Conteúdo convertido para SH1HashData</returns>
        public static string ToSHA1HashData(string data, bool toUpper)
        {
            using (HashAlgorithm algorithm = new SHA1CryptoServiceProvider())
            {
                var buffer = algorithm.ComputeHash(Encoding.ASCII.GetBytes(data));
                var builder = new StringBuilder(buffer.Length);
                foreach (var num in buffer)
                {
                    if (toUpper)
                    {
                        builder.Append(num.ToString("X2"));
                    }
                    else
                    {
                        builder.Append(num.ToString("x2"));
                    }
                }

                return builder.ToString();
            }
        }

    }
}

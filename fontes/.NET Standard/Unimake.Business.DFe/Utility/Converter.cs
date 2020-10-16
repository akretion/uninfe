using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Unimake.Business.DFe.Utility
{
    /// <summary>
    /// Classe para conversão de objetos
    /// </summary>
    public static class Converter
    {
        #region Private Methods

        /// <summary>
        /// Converter tipo de um objeto
        /// </summary>
        /// <param name="value">Para qual tipo converter o conteúdo do objeto</param>
        /// <param name="conversionType">Para qual tipo converter o conteúdo do objeto</param>
        /// <returns>Conteúdo do objeto convertido para o tipo informado</returns>
        private static object ChangeType(object value, Type conversionType)
        {
            if(value == null)
            {
                return null;
            }

            object result = null;

            try
            {
                if(conversionType.IsEnum)
                {
                    var i = Convert.ToInt32(value);

                    result = (from enun in Enum.GetValues(conversionType).Cast<int>()
                              where enun == i
                              select enun).First();
                }
                else if(conversionType == typeof(TimeSpan) ||
                        conversionType == typeof(Nullable<TimeSpan>))
                {
                    var timeDate = new DateTime();

                    if(DateTime.TryParse(value.ToString(), out timeDate))
                    {
                        result = new TimeSpan(timeDate.Ticks);
                    }
                }
                else
                {
                    var conv = TypeDescriptor.GetConverter(conversionType);
                    if(conv?.CanConvertFrom(value.GetType()) ?? false)
                    {
                        try
                        {
                            result = conv.ConvertFrom(value);
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                    else
                    {
                        result = System.Convert.ChangeType(value, conversionType);
                    }
                }
            }
            catch
            {
                var conv = TypeDescriptor.GetConverter(conversionType);
                if(conv?.CanConvertFrom(value.GetType()) ?? false)
                {
                    try
                    {
                        result = conv.ConvertFrom(value);
                    }
                    catch
                    {
                        //do nothing
                    }
                };
            }

            return result;
        }

        #endregion Private Methods

        #region Public Methods

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
        /// Tenta converter qualquer objeto passado em value para o tipo esperado em T
        /// </summary>
        /// <typeparam name="T">Tipo esperado para conversão</typeparam>
        /// <param name="value">Valor que deverá ser convertido</param>
        /// <returns>Value convertido em T</returns>
        public static T ToAny<T>(object value) => (T)ToAny(typeof(T), value);

        /// <summary>
        /// Converter tipo de um objeto
        /// </summary>
        /// <param name="t">Para qual tipo converter o conteúdo do objeto</param>
        /// <param name="value">Conteúdo do objeto a ser convertido</param>
        /// <returns>Conteúdo do objeto convertido para o tipo informado</returns>
        public static object ToAny(Type t, object value)
        {
            var result = default(object);

            t = Nullable.GetUnderlyingType(t) ?? t;

            if(value != null && value != DBNull.Value)
            {
                try
                {
                    if(t.IsEnum)
                    {
                        result = Enum.Parse(t, value.ToString());
                    }
                    else if(t.FullName.Equals(typeof(string).FullName))
                    {
                        var s = value.ToString();

                        result = ChangeType(s, t);
                    }
                    else
                    {
                        result = ChangeType(value, t);
                    }
                }
                catch
                {
                    result = default;
                }
            }

            return result;
        }

        /// <summary>
        /// Converte um valor do objeto em double
        /// </summary>
        /// <param name="value">valor a ser convertido</param>
        /// <returns>Valor convertido para double</returns>
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

        /// <summary>
        /// Converter STRING para ENUM
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="value">String a ser convertida</param>
        /// <returns>Retorna o Enum da string passada como parâmetro</returns>
        public static T ToEnum<T>(this string value) => (T)Enum.Parse(typeof(T), value, true);

        /// <summary>
        /// Calcular o valor hexadecimal de uma string
        /// </summary>
        /// <param name="input">Valor a ser convertido</param>
        /// <returns>Valor convertido em hexadecimal</returns>
        public static string ToHexadecimal(string input)
        {
            var hexOutput = "";
            var values = input.ToCharArray();
            foreach(var letter in values)
            {
                // Get the integral value of the character.
                var value = Convert.ToInt32(letter);

                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += string.Format("{0:x}", value);
            }

            return hexOutput;
        }

        /// <summary>
        /// Criptografa uma string com RSA-SHA1 e retorna o conteúdo convertido para Base64String
        /// </summary>
        /// <param name="certificado">certificado utilizado na criptografia</param>
        /// <param name="value">Conteúdo a ser criptografado</param>
        /// <returns>Retorna a string assinada com RSA SHA1 e convertida para Base64String</returns>
        public static string ToRSASHA1(X509Certificate2 certificado, string value)
        {
            // Converter a cadeia de caracteres ASCII para bytes.
            var asciiEncoding = new ASCIIEncoding();
            var asciiBytes = asciiEncoding.GetBytes(value);

            // Gerar o HASH (array de bytes) utilizando SHA1
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1Hash = sha1.ComputeHash(asciiBytes);

            // Assinar o HASH (array de bytes) utilizando RSA-SHA1.
            var rsa = new RSACryptoServiceProvider();
            rsa = certificado.PrivateKey as RSACryptoServiceProvider;
            asciiBytes = rsa.SignHash(sha1Hash, "SHA1");
            var result = Convert.ToBase64String(asciiBytes);

            return result;
        }

        /// <summary>
        /// Converte conteúdo para HSA1HashData
        /// </summary>
        /// <param name="data">Conteudo a ser convertido</param>
        /// <returns>Conteúdo convertido para SH1HashData</returns>
        public static string ToSHA1HashData(string data) => ToSHA1HashData(data, false);

        /// <summary>
        /// Converte conteúdo para HSA1HashData
        /// </summary>
        /// <param name="data">Conteudo a ser convertido</param>
        /// <param name="toUpper">Resultado todo em maiúsculo?</param>
        /// <returns>Conteúdo convertido para SH1HashData</returns>
        public static string ToSHA1HashData(string data, bool toUpper)
        {
            using(HashAlgorithm algorithm = new SHA1CryptoServiceProvider())
            {
                var buffer = algorithm.ComputeHash(Encoding.ASCII.GetBytes(data));
                var builder = new StringBuilder(buffer.Length);
                foreach(var num in buffer)
                {
                    if(toUpper)
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

        #endregion Public Methods
    }
}
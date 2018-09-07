using NFe.Exceptions;
using System;
using System.Linq;

namespace NFe.Components
{
    public class CPF
    {
        #region Private Fields

        private string _value = "";

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// Construtor do objeto CPF
        /// </summary>
        /// <param name="value">CPF informado</param>
        private CPF(string value)
        {
            if (value.Length == 0)
                return;

            value = Functions.OnlyNumbers(value, ".-").ToString();

            if (!Validate(value))
                throw new ExceptionCPFInvalido(value);

            _value = value;
        }

        #endregion Private Constructors

        #region Public Operator

        public static implicit operator CPF(string value)
        {
            return new CPF(value);
        }

        #endregion Public Operator

        #region Public Methods

        /// <summary>
        /// Analisa o CPF informado e verifica se é válido
        /// </summary>
        /// <param name="cpf">CPF informado</param>
        /// <param name="allowNullOrEmpty">Se true, permite nulo e branco</param>
        /// <returns></returns>
        public static bool Validate(string cpf, bool allowNullOrEmpty = true)
        {
            cpf = Functions.OnlyNumbers(cpf, "/.-").ToString();
            if (String.IsNullOrEmpty(cpf) && allowNullOrEmpty) return true;
            if (string.IsNullOrEmpty(cpf) || cpf.Length < 11) return false;

            //Se todos os números forem iguais, isso indica que o CPF é inválido
            int result = CountChars(cpf, Convert.ToChar(cpf.Substring(0, 1)));
            if (result == 11) return allowNullOrEmpty;

            try
            {
                #region valida

                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;

                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length != 11)
                    return false;

                tempCpf = cpf.Substring(0, 9);
                soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cpf.EndsWith(digito);

                #endregion valida
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// gravação de dados
        /// </summary>
        /// <param name="provider">CurrentCulture</param>
        /// <returns>somente os números</returns>
        public string ToString(IFormatProvider provider)
        {
            return Functions.OnlyNumbers(_value, ".-").ToString();
        }

        /// <summary>
        /// Converte para o formato string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Format(_value);
        }

        /// <summary>
        /// Formatar o CPF
        /// </summary>
        /// <param name="cpf">CPF a ser formatado</param>
        /// <returns></returns>
        public string Format(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return "___.___.___-__";

            cpf = Functions.OnlyNumbers(cpf, "-.").ToString();

            return Convert.ToInt64(cpf).ToString(@"000\.000\.000-00");
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Conta a quantidade de vezes que um carácter aparece em uma string
        /// </summary>
        /// <param name="value">string com os caracteres a ser analisado</param>
        /// <param name="charToCount">carácter a ser contado</param>
        /// <returns>A quantidade de vezes que o carácter (charToCount) foi encontrado na string (value)</returns>
        private static int CountChars(string value, char charToCount)
        {
            if (String.IsNullOrEmpty(value)) value = "";
            int result = value.Count(c => c == charToCount);
            return result;
        }

        #endregion Private Methods
    }
}

namespace NFe.Exceptions
{
    /// <summary>
    /// CPF não é válido.
    /// </summary>
    public class ExceptionCPFInvalido : Exception
    {
        private string _cpf = "";

        public ExceptionCPFInvalido(string cpf)
        {
            _cpf = cpf;
        }

        public override string Message
        {
            get
            {
                return "O CPF informado não é válido\nCPF: " + _cpf;
            }
        }
    }
}
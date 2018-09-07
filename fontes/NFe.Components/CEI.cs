using NFe.Exceptions;
using System;

namespace NFe.Components
{
    public class CEI
    {
        #region Locais

        private string _cei;

        #endregion Locais

        #region Private Constructors

        private CEI(string cei)
        {
            if (cei.Length == 0)
                return;

            if (!Validate(cei))
                throw new ExceptionCEIInvalido(cei);

            _cei = cei;
        }

        #endregion Private Constructors

        #region Public Operator

        public static implicit operator CEI(string cei)
        {
            return new CEI(cei);
        }

        #endregion Public Operator

        #region Public Methods

        /// <summary>
        /// Validar o Cadastro Específico do INSS
        /// </summary>
        /// <param name="cei">Número do cadastro específico do INSS a ser validado</param>
        /// <returns></returns>
        public static bool Validate(string cei)
        {
            cei = Functions.OnlyNumbers(cei, "./").ToString();

            if (String.IsNullOrEmpty(cei) || cei.Length < 12)
                return false;

            var peso = "74185216374";
            var soma = 0;

            //Faz um for para multiplicar os números do CEI digitado pelos números do peso.
            //E somar o total de cada número multiplicado.
            for (int i = 1; i < 12; i++)
            {
                int fator = Convert.ToInt32(peso.Substring(i - 1, 1));
                int valor = Convert.ToInt32(cei.Substring(i - 1, 1));
                soma += (fator * valor);
            }

            //Pega o length do resultado da soma e desconta 2 para pegar somente a dezena.
            int definirDezena = soma.ToString().Length - 2;

            //pega a dezena
            int dezena = Convert.ToInt32(soma.ToString().Substring(definirDezena));

            //pega o algarismo da dezena
            int algarismoDezena = Convert.ToInt32(dezena.ToString().Substring(0, 1));

            //pega o algarismo da unidade
            int algarismoUnidade = Convert.ToInt32(soma) - (Convert.ToInt32((soma / 10)) * 10);

            //soma o algarismo da dezena com o algarismo da unidade.
            soma = Convert.ToInt32(algarismoDezena) + algarismoUnidade;

            //unidade da soma
            int unidadeSoma = Convert.ToInt32(soma.ToString().Substring(soma.ToString().Length - 1));

            int digitoEncontrado = 10 - unidadeSoma;

            int definirDigitoEncontrado = Convert.ToInt32(digitoEncontrado.ToString().Length - 1);

            int unidadeDigitoEncontrado = Convert.ToInt32(digitoEncontrado.ToString().Substring(definirDigitoEncontrado));

            //pega o dígito (último número) do cei digitado.
            int digitoCEI = Convert.ToInt32(cei.Substring(11));

            if (digitoCEI != unidadeDigitoEncontrado)
                return false;
            else
                return true;
        }

        public override string ToString()
        {
            return Format(_cei);
        }

        public string ToString(IFormatProvider provider)
        {
            return Functions.OnlyNumbers(_cei, "./").ToString();
        }

        #endregion Public Methods

        #region Private Methods

        private string Format(string cei)
        {
            if (String.IsNullOrEmpty(cei)) return "__.___.____/__";

            cei = Functions.OnlyNumbers(cei, "./").ToString();

            return Convert.ToInt64(cei).ToString(@"00\.000\.0000\/00");
        }

        #endregion Private Methods
    }
}

namespace NFe.Exceptions
{
    /// <summary>
    /// Cadastro Específico do INSS não é válido.
    /// </summary>
    public class ExceptionCEIInvalido : Exception
    {
        private string _cei = "";

        public ExceptionCEIInvalido(string cei)
        {
            _cei = cei;
        }

        public override string Message
        {
            get
            {
                return "O CEI informado não é válido\nCEI: " + _cei;
            }
        }
    }
}
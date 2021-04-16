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
            if(cei.Length == 0)
            {
                return;
            }

            if(!Validate(cei))
            {
                throw new ExceptionCEIInvalido(cei);
            }

            _cei = cei;
        }

        #endregion Private Constructors

        #region Public Operator

        public static implicit operator CEI(string cei) => new CEI(cei);

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

            if(string.IsNullOrEmpty(cei) || cei.Length < 12)
            {
                return false;
            }

            var peso = "74185216374";
            var soma = 0;

            //Faz um for para multiplicar os números do CEI digitado pelos números do peso.
            //E somar o total de cada número multiplicado.
            for(var i = 0; i < 11; i++)
            {
                var fator = Convert.ToInt32(peso.Substring(i, 1));
                var valor = Convert.ToInt32(cei.Substring(i, 1));
                soma += (fator * valor);
            }

            //Pegar a Dezena
            var dezena = Convert.ToInt32(soma.ToString().Substring(soma.ToString().Length - 2, 1));

            //Pegar a Unidade
            var unidade = Convert.ToInt32(soma.ToString().Substring(soma.ToString().Length - 1, 1));

            //Total Dezena + Unidade
            var total = (dezena + unidade);

            //Calcular o resultado
            var ultimoDigitoTotal = Convert.ToInt32(total.ToString().Substring(total.ToString().Length - 1, 1));
            var resultado = 10 - ultimoDigitoTotal;

            //Calcular o dígito verificador
            var digitoVerificador = Convert.ToInt32(resultado.ToString().Substring(resultado.ToString().Length - 1, 1));

            //pega o dígito (último número) do cei digitado.
            var digitoCEI = Convert.ToInt32(cei.Substring(11));

            if(digitoCEI != digitoVerificador)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override string ToString() => Format(_cei);

        public string ToString(IFormatProvider provider) => Functions.OnlyNumbers(_cei, "./").ToString();

        #endregion Public Methods

        #region Private Methods

        private string Format(string cei)
        {
            if(string.IsNullOrEmpty(cei))
            {
                return "__.___.____/__";
            }

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
    public class ExceptionCEIInvalido: Exception
    {
        private string _cei = "";

        public ExceptionCEIInvalido(string cei) => _cei = cei;

        public override string Message => "O CEI informado não é válido\nCEI: " + _cei;
    }
}
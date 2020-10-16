﻿namespace System
{
    /// <summary>
    /// Extensão da classe String
    /// </summary>
    public static class StringExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retorna uma string truncada até o máximo definido no parâmetro see cref="maxLength" />
        /// </summary>
        /// <param name="value">String que será truncada</param>
        /// <param name="maxLength">Tamanho máximo que a string poderá ter</param>
        /// <returns>Retorna uma string truncada até o máximo definido no parâmetro see cfref="maxLength"</returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        #endregion Public Methods
    }
}
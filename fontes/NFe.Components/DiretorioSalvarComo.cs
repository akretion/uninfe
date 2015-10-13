﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Components
{
    /// <summary>
    /// Definição de como os diretórios devem ser salvos
    /// </summary>
    /// <by>http://desenvolvedores.net/marcelo</by>
    public class DiretorioSalvarComo
    {
        #region Locais
        private string mSalvarComo = "";
        #endregion

        #region Operadores
        public static implicit operator DiretorioSalvarComo(string rhs)
        {
            return new DiretorioSalvarComo(rhs);
        }

        public static implicit operator string(DiretorioSalvarComo rhs)
        {
            if (rhs == null)
                return "AM"; //padrão

            return rhs.ToString();
        }
        #endregion

        #region Construtores
        private DiretorioSalvarComo(string s)
        {
            mSalvarComo = s;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return mSalvarComo;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// converte o diretório do formato AMD para a estrutura esperada
        /// </summary>
        /// <param name="emissao">date de emissão</param>
        /// <returns>estrutura de diretórios</returns>
        public string ToString(DateTime emissao)
        {
            string ret = this.ToString();
            if (string.IsNullOrEmpty(ret)) return "";
            if (ret.Equals("Raiz"))
                return "";

            //extrai os períodos
            int dia = emissao.Day;
            int mes = emissao.Month;
            int ano = emissao.Year;

            //apenas faz um replace dos valores
            ret = ret.Replace("D", dia.ToString("00"));
            ret = ret.Replace("M", mes.ToString("00"));
            ret = ret.Replace("A", ano.ToString("0000"));

            return ret + "\\";
        }
        #endregion
    }
}

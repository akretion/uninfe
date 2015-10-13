using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace NFe.Components
{
    public class ComboElem
    {
        public ComboElem() { }
        public ComboElem(string valor, int codigo, string nome)
        {
            Valor = valor;
            Codigo = codigo;
            Nome = nome;
        }

        public ComboElem(string valor, int codigo)
        {
            Valor = valor;
            Codigo = codigo;
        }

        string _valor;
        int _Codigo;
        string _nome;
        public string Servico { get; set; }
        public string Key
        {
            get
            {
                return this.Valor + this.Servico.ToString();
            }
        }

        public string Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

        public int Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
    }

    public class OrdenacaoPorNome : IComparer 
    {
        int IComparer.Compare( Object x, Object y)
        {
            ComboElem objetoA = (ComboElem)x;
            ComboElem objetoB = (ComboElem)y;

            return objetoA.Nome.CompareTo(objetoB.Nome);
        }

    }
}

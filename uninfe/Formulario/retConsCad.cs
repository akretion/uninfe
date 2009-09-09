using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace uninfe.Formulario
{
    public partial class retConsCad : UserControl
    {
        public retConsCad(UniNFeLibrary.infCad infCad)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(infCad.CNPJ))
            {
                this.label1.Text = "CNPJ";
                this.txtCNPJ_CPF.Text = FmtCgcCpf(infCad.CNPJ, true);
            }
            else
            {
                this.label1.Text = "CPF";
                this.txtCNPJ_CPF.Text = FmtCgcCpf(infCad.CPF, false);
            }
            this.txtxIE.Text = infCad.IE;
            this.txtxNome.Text = infCad.xNome;
            this.txtxFant.Text = infCad.xFant;
            this.txtxEnder.Text = infCad.xLgr;
            this.txtxBairro.Text = infCad.xBairro;
            this.txtnro.Text = infCad.nro;
            this.txtxCpl.Text = infCad.xCpl;
            this.txtUF.Text = infCad.UF;
            this.txtxMun.Text = infCad.xMun;
            this.txtCEP.Text = infCad.CEP.ToString("00000000");
            this.txtdBaixa.Text = infCad.dBaixa.ToShortDateString();
            this.txtdUltSit.Text = infCad.dUltSit.ToShortDateString();
            this.txtdIniAtiv.Text = infCad.dIniAtiv.ToShortDateString();
            this.txtxRegApur.Text = infCad.xRegApur;
            this.txtCNAE.Text = infCad.CNAE.ToString();
            this.txtIEAtual.Text = infCad.IEAtual;
            this.txtIEUnica.Text = infCad.IEUnica;
            this.txtcSit.Text = infCad.cSit;
        }

        private string Align(string val, int size)
        {
            string Result = string.Empty;
            for (int i = 0; i < size; ++i)
                Result += '0';
            Result += val;
            Result = Result.Remove(0, Result.Length - size);
            return Result;
        }

        private string FmtCgcCpf(string Value, bool juridica)
        {
            string Result = Align(Value,14);
            if (juridica)
                Result = Result.Substring(0, 2) + "." +
                        Result.Substring(2, 3) + "." +
                        Result.Substring(5, 3) + "/" +
                        Result.Substring(8, 4) + "-" +
                        Result.Substring(12, 2);
            else
                Result = Result.Substring(3, 3) + "." +
                            Result.Substring(6, 3) + "." +
                            Result.Substring(9, 3) + "-" +
                            Result.Substring(12, 2);
            return Result;
        }
    }
}

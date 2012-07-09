using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NFe.Components;
using NFe.Settings;

namespace NFe.Interface
{
    public partial class FormSenha : Form
    {
        public bool AcessoAutorizado { get; private set; }
        public bool AcessoCancelado { get; private set; }

        public FormSenha()
        {
            InitializeComponent();
            AcessoAutorizado = false;
            AcessoCancelado = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VerificaSenha();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AcessoAutorizado = false;
            AcessoCancelado = true;
        }

        private void FormSenha_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!AcessoAutorizado)
                AcessoCancelado = true;
        }

        private void VerificaSenha()
        {
            string senhaCrip = Functions.GerarMD5(tbSenha.Text.Trim());

            if (string.IsNullOrEmpty(tbSenha.Text.Trim()) || senhaCrip != ConfiguracaoApp.SenhaConfig)
                MessageBox.Show("Senha incorreta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                AcessoAutorizado = true;
                this.Close();
            }
        }
    }
}

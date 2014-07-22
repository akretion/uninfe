using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class FormSenha : MetroFramework.Forms.MetroForm
    {
        public FormSenha()
        {
            InitializeComponent();
        }

        private void FormSenha_Load(object sender, EventArgs e)
        {
            this.Theme = uninfeDummy.mainForm.uTheme;
            this.Style = uninfeDummy.mainForm.uStyle;
            this.Text = NFe.Components.Propriedade.NomeAplicacao;
            this.tbSenha.Clear();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, _e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                this.tbSenha.Focus();
            };
            t.Start();
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.tbSenha.Focus();

            string senhaCrip = NFe.Components.Functions.GerarMD5(tbSenha.Text.Trim());

            if (string.IsNullOrEmpty(tbSenha.Text) || senhaCrip != NFe.Settings.ConfiguracaoApp.SenhaConfig)
            {
                MetroFramework.MetroMessageBox.Show(this, "Senha inválida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void tbSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.metroButton1_Click(sender, null);
        }

        public static bool SolicitaSenha()
        {
            using (FormSenha f = new FormSenha())
            {
                return f.ShowDialog() == DialogResult.OK;
            }
        }
    }
}

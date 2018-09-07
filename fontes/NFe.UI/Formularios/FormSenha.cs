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
        bool frommain;
        public FormSenha(bool fromMain)
        {
            InitializeComponent();
            frommain = fromMain;
        }

        private void FormSenha_Load(object sender, EventArgs e)
        {
            uninfeDummy.ClearControls(this, true, frommain);
            this.Text = NFe.Components.Propriedade.NomeAplicacao;
            this.tbSenha.Clear();
            if (frommain)
            {
                this.Size = new Size(uninfeDummy.mainForm.Size.Width, this.Height);
                this.Location = new Point(uninfeDummy.mainForm.Location.X, uninfeDummy.mainForm.Location.Y + (uninfeDummy.mainForm.Height - this.Height) / 2);
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
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
                       

            if (string.IsNullOrEmpty(tbSenha.Text) || (senhaCrip != NFe.Settings.ConfiguracaoApp.SenhaConfig && senhaCrip != NFe.Components.Propriedade.SenhaAdm))
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

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static bool SolicitaSenha(bool fromMain)
        {
            using (FormSenha f = new FormSenha(fromMain))
            {
                return f.ShowDialog() == DialogResult.OK;
            }
        }
    }
}

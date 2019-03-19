using NFe.Components;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class userConfiguracao_resptecnico : MetroFramework.Controls.MetroUserControl
    {
        public event EventHandler changeEvent;

        private Settings.Empresa empresa;

        public userConfiguracao_resptecnico()
        {
            InitializeComponent();
        }

        public void Populate(Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            txtCnpj.Text = string.IsNullOrEmpty(empresa.RespTecCNPJ) ? "" : CNPJ.FormatCNPJ(empresa.RespTecCNPJ);
            txtContato.Text = empresa.RespTecXContato;
            txtEmail.Text = empresa.RespTecEmail;
            txtTelefone.Text = empresa.RespTecTelefone;
            txtIdCSRT.Text = empresa.RespTecIdCSRT;
            txtCSRT.Text = empresa.RespTecCSRT;
        }

        public void Validar(bool salvando = true)
        {
            empresa.RespTecCNPJ = Functions.OnlyNumbers(txtCnpj.Text, ".-/").ToString();
            empresa.RespTecXContato = txtContato.Text;
            empresa.RespTecEmail = txtEmail.Text;
            empresa.RespTecTelefone = Functions.OnlyNumbers(txtTelefone.Text, "()-").ToString().Trim();
            empresa.RespTecIdCSRT = String.IsNullOrEmpty(txtIdCSRT.Text) ? txtIdCSRT.Text : txtIdCSRT.Text.PadLeft(2, '0');
            empresa.RespTecCSRT = txtCSRT.Text;
        }

        public void FocusFirstControl()
        {
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();
            };
            t.Start();
        }

        private void txtCnpj_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtContato_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtIdCSRT_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtCSRT_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEmail.Text))
            {
                if (!ValidaEmail())
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "E-mail inválido", "UniNFe");
                    txtEmail.Focus();
                }
            }
        }

        private bool ValidaEmail()
        {
            return Regex.IsMatch(txtEmail.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        private void txtCnpj_Leave(object sender, EventArgs e)
        {
            try
            {
                string cnpj = Functions.OnlyNumbers(txtCnpj.Text, ".,-/").ToString();

                if (string.IsNullOrEmpty(cnpj))
                    return;

                if (Convert.ToInt64(cnpj).Equals(0))
                {
                    txtCnpj.Focus();
                    return;
                }

                cnpj = cnpj.PadLeft(14, '0');
                txtCnpj.Text = ((CNPJ)cnpj).ToString();
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "UniNFe");
                txtCnpj.Focus();
            }
        }

        private void txtIdCSRT_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtIdCSRT.Text))
            {
                if (Convert.ToInt32(txtIdCSRT.Text) <= 0)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "ID do CSRT inválido", "UniNFe");
                    txtIdCSRT.Focus();
                }
            }
        }
    }
}
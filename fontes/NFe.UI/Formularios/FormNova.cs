using NFe.Components;
using NFe.Settings;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormNova : MetroFramework.Forms.MetroForm
    {
        Form _owner;

        public FormNova(Form parente)
        {
            InitializeComponent();
            _owner = parente;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_owner != null)
            {
                this.Size = new Size(_owner.Size.Width, this.Height);
                this.Location = new Point(_owner.Location.X, _owner.Location.Y + (_owner.Height - this.Height) / 2);
            }
            uninfeDummy.ClearControls(this, true, true);

            this.Text = NFe.Components.Propriedade.NomeAplicacao + " - Nova empresa";

            this.cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo(false);
            this.cbServico.DisplayMember = "Value";
            this.cbServico.ValueMember = "Key";
            this.cbServico.Enabled = true;
            this.cbServico.SelectedIndex = 0;
            this.edtCNPJ.Text = this.edtNome.Text = "";
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

                this.edtCNPJ.Focus();
            };
            t.Start();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            FormatarCPFCNPJ();

            if (edtCNPJ.Text.Length == 14)
            {
                if (!CPF.Validate(edtCNPJ.Text, false))
                {
                    MetroFramework.MetroMessageBox.Show(this, "CPF inválido", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.edtCNPJ.Focus();
                    return;
                }
            }
            else
            {
                if (!CNPJ.Validate(edtCNPJ.Text))
                {
                    MetroFramework.MetroMessageBox.Show(this, "CNPJ inválido", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.edtCNPJ.Focus();
                    return;
                }
            }

            string cnpj = Functions.OnlyNumbers(edtCNPJ.Text, ".-/").ToString();

            if (this.edtNome.Text.Trim().Length == 0)
            {
                this.edtNome.Focus();
                MetroFramework.MetroMessageBox.Show(this, "Nome deve ser informado", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedValue;

            Empresa empresa = null;
            switch (servico)
            {
                case TipoAplicativo.Todos:
                case TipoAplicativo.Nfe:

                    //Serviço todos e NFe utilizam a mesma pasta de configurações, então não posso permitir configurar o mesmo CNPJ para os dois serviços. Wandrey
                    if ((empresa = Empresas.FindConfEmpresa(cnpj, TipoAplicativo.Todos)) == null)
                        empresa = Empresas.FindConfEmpresa(cnpj, TipoAplicativo.Nfe);
                    break;

                default:
                    empresa = Empresas.FindConfEmpresa(cnpj, servico);
                    break;
            }

            if (empresa != null)
            {
                string msgErro = "Já existe uma Empresa/CNPJ configurada para atender este serviço, conforme dados abaixo: " +
                                 "\r\n\r\nEmpresa configurada: " + empresa.Nome +
                                 "\r\nServiço configurado: " + NFe.Components.EnumHelper.GetDescription(empresa.Servico);

                MetroFramework.MetroMessageBox.Show(this, msgErro, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void edtCNPJ_TextChanged(object sender, EventArgs e)
        {
            this.metroButton1.Enabled = this.edtCNPJ.Text.Length > 0 && this.edtNome.Text.Trim().Length > 0;
        }

        private void edtCNPJ_Enter(object sender, EventArgs e)
        {
            this.edtCNPJ.Text = (string)NFe.Components.Functions.OnlyNumbers(this.edtCNPJ.Text, ".-/");
        }

        private void edtCNPJ_Leave(object sender, EventArgs e)
        {
            FormatarCPFCNPJ();
        }

        private void FormatarCPFCNPJ()
        {
            string cnpj = Functions.OnlyNumbers(this.edtCNPJ.Text, ".,-/").ToString();

            if (string.IsNullOrEmpty(cnpj))
                return;

            if (Convert.ToInt64(cnpj).Equals(0))
            {
                this.edtCNPJ.Clear();
                this.edtCNPJ.Focus();
                return;
            }

            if (cnpj.Length < 13)
                this.edtCNPJ.Text = uninfeDummy.FmtCnpjCpf(cnpj, false);
            else
                this.edtCNPJ.Text = uninfeDummy.FmtCnpjCpf(cnpj, true);
        }
    }
}
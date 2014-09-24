using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

            this.cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo(true);
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
            this.edtCNPJ.Focus();

            string cnpj = NFe.Components.Functions.OnlyNumbers(this.edtCNPJ.Text, ".,-/").ToString().PadLeft(14, '0');
            this.edtCNPJ.Text = uninfeDummy.FmtCgcCpf(cnpj, true);

            if (!NFe.Components.CNPJ.Validate(cnpj) || cnpj.Equals("00000000000000"))
            {
                if (cnpj.Equals("00000000000000"))
                    this.edtCNPJ.Clear();

                MetroFramework.MetroMessageBox.Show(this, "CNPJ inválido", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.edtNome.Text.Trim().Length == 0)
            {
                this.edtNome.Focus();
                MetroFramework.MetroMessageBox.Show(this, "Nome deve ser informado", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            NFe.Components.TipoAplicativo servico = (NFe.Components.TipoAplicativo)cbServico.SelectedValue;
            int cnt;

            if (servico == NFe.Components.TipoAplicativo.Nfse)
            {
                cnt = NFe.Settings.Empresas.FindConfEmpresaIndex(cnpj, servico);
            }
            else
            {
                cnt =   NFe.Settings.Empresas.FindConfEmpresaIndex(cnpj, NFe.Components.TipoAplicativo.Todos) >= 0 ||
                        NFe.Settings.Empresas.FindConfEmpresaIndex(cnpj, NFe.Components.TipoAplicativo.NFCe) >= 0 ||
                        NFe.Settings.Empresas.FindConfEmpresaIndex(cnpj, NFe.Components.TipoAplicativo.Nfe) >= 0 ||
                        NFe.Settings.Empresas.FindConfEmpresaIndex(cnpj, NFe.Components.TipoAplicativo.Cte) >= 0 ||
                        NFe.Settings.Empresas.FindConfEmpresaIndex(cnpj, NFe.Components.TipoAplicativo.MDFe) >= 0 ? 1 : 0;
            }
            if (cnt > 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Empresa/CNPJ para atender o serviço de " + servico.ToString() + " já existe", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            string cnpj = NFe.Components.Functions.OnlyNumbers(this.edtCNPJ.Text, ".,-/").ToString().PadLeft(14, '0');
            this.edtCNPJ.Text = uninfeDummy.FmtCgcCpf(cnpj, true);
        }
    }
}

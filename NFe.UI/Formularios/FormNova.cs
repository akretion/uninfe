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

            this.Size = new Size(_owner/*uninfeDummy.mainForm*/.Size.Width, this.Height);
            this.Location = new Point(_owner/*uninfeDummy.mainForm*/.Location.X, _owner/*uninfeDummy.mainForm*/.Location.Y + (_owner/*uninfeDummy.mainForm*/.Height - this.Height) / 2);

            uninfeDummy.ClearControls(this, true, true);

            this.Text = NFe.Components.Propriedade.NomeAplicacao + " - Nova empresa";

            #region Montar array DropList dos tipos de serviços
            this.cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo();
            this.cbServico.DisplayMember = "Value";
            this.cbServico.ValueMember = "Key";
            #endregion

            this.cbServico.Enabled = (NFe.Components.Propriedade.TipoAplicativo == Components.TipoAplicativo.Nfe);
            this.cbServico.SelectedIndex = 0;
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
            if (NFe.Settings.Empresas.FindConfEmpresa(cnpj, servico) != null)
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

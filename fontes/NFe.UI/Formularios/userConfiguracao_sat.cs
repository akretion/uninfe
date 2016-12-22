using System;
using System.ComponentModel;
using System.Windows.Forms;
using NFe.Components;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_sat : MetroFramework.Controls.MetroUserControl
    {
        public userConfiguracao_sat()
        {
            InitializeComponent();

            cbRegTribISSQN.DataSource = EnumHelper.ToList(typeof(RegTribISSQN), true, true);
            cbRegTribISSQN.DisplayMember = "Value";
            cbRegTribISSQN.ValueMember = "Key";

            cbindRatISSQN.DataSource = EnumHelper.ToList(typeof(IndRatISSQN), true, true);
            cbindRatISSQN.DisplayMember = "Value";
            cbindRatISSQN.ValueMember = "Key";
        }

        public event EventHandler changeEvent;
        private Settings.Empresa empresa;

        public void Populate(Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            cbMacarSAT.SelectedItem = empresa.MarcaSAT;
            txtCodigoAtivacaoSAT.Text = empresa.CodigoAtivacaoSAT;
            txtSignAC.Text = empresa.SignACSAT;
            txtCNPJSw.Text = empresa.CNPJSoftwareHouse;            
            txtNumeroCaixa.Text = empresa.NumeroCaixa;
            ckConversaoNFCe.Checked = empresa.UtilizaConversaoCFe;
            cbRegTribISSQN.SelectedValue = (int)empresa.RegTribISSQNSAT;
            cbindRatISSQN.SelectedValue = (int)empresa.IndRatISSQNSAT;

            lblCNPJSw.Visible =
                txtCNPJSw.Visible =
                lblSignAC.Visible =
                txtSignAC.Visible =
                lblIndRatISSQN.Visible =
                lblRegTribISSQN.Visible =
                cbindRatISSQN.Visible =
                cbRegTribISSQN.Visible =
                lblNumeroCaixa.Visible =
                txtNumeroCaixa.Visible = ckConversaoNFCe.Checked;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void Validar()
        {
            empresa.MarcaSAT = cbMacarSAT.SelectedItem.ToString();
            empresa.CodigoAtivacaoSAT = txtCodigoAtivacaoSAT.Text;
            empresa.UtilizaConversaoCFe = ckConversaoNFCe.Checked;
            empresa.CNPJSoftwareHouse = txtCNPJSw.Text;
            empresa.SignACSAT = txtSignAC.Text;
            empresa.NumeroCaixa = txtNumeroCaixa.Text;
            empresa.RegTribISSQNSAT = (RegTribISSQN)cbRegTribISSQN.SelectedValue;
            empresa.IndRatISSQNSAT = (IndRatISSQN)cbindRatISSQN.SelectedValue;
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

        private void cbMacarSAT_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtCodigoAtivacaoSAT_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void ckConversaoNFCe_CheckedChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
            lblCNPJSw.Visible =
                txtCNPJSw.Visible =
                lblSignAC.Visible =
                txtSignAC.Visible =
                lblIndRatISSQN.Visible =
                lblRegTribISSQN.Visible =
                cbindRatISSQN.Visible =
                cbRegTribISSQN.Visible =
                lblNumeroCaixa.Visible =
                txtNumeroCaixa.Visible = ckConversaoNFCe.Checked;
        }

        private void txtSignAC_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtCNPJSw_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void cbRegTribISSQN_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void cbindRatISSQN_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void txtNumeroCaixa_TextChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }
    }
}

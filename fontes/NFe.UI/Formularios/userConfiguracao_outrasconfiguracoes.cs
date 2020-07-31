using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NFe.Components;
using NFe.Settings;

namespace NFe.UI.Formularios
{
    public partial class userConfiguracao_outrasconfiguracoes : MetroFramework.Controls.MetroUserControl
    {
        public event EventHandler changeEvent;


        private Settings.Empresa empresa;
        public userConfiguracao_outrasconfiguracoes()
        {
            InitializeComponent();
        }

        public void Populate(Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);
            chkSalvarXMLDistribuicao.Checked = empresa.SalvarSomenteXMLDistribuicao;
            cbIndSincMDFe.Checked = empresa.IndSincMDFe;
            Configurar(empresa);
        }

        public void Validar(bool salvando = true)
        {
            empresa.SalvarSomenteXMLDistribuicao = chkSalvarXMLDistribuicao.Checked;
            empresa.IndSincMDFe = cbIndSincMDFe.Checked;
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

        private void ChkSalvarXMLDistribuicao_CheckedChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void cbIndSincMDFe_CheckedChanged(object sender, EventArgs e)
        {
            changeEvent?.Invoke(sender, e);
        }

        private void Configurar(Empresa empresa)
        {
            switch (empresa.Servico)
            {
                case TipoAplicativo.Nfe:
                case TipoAplicativo.Nfse:
                case TipoAplicativo.Cte:
                case TipoAplicativo.NFCe:
                case TipoAplicativo.SAT:
                case TipoAplicativo.EFDReinf:
                case TipoAplicativo.eSocial:
                case TipoAplicativo.EFDReinfeSocial:
                case TipoAplicativo.Nulo:
                    cbIndSincMDFe.Visible = false;
                    break;
                case TipoAplicativo.Todos:
                case TipoAplicativo.MDFe:
                    cbIndSincMDFe.Visible = false;
                    break;
            }
        }
    }
}
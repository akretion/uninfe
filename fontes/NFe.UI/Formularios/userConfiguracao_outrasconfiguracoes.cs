using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        public void Validar(bool salvando = true)
        {
            empresa.SalvarSomenteXMLDistribuicao = chkSalvarXMLDistribuicao.Checked;
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
    }
}

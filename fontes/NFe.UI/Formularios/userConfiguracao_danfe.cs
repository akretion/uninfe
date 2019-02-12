using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_danfe : MetroFramework.Controls.MetroUserControl
    {
        public userConfiguracao_danfe()
        {
            InitializeComponent();
        }

        public event EventHandler changeEvent;
        NFe.Settings.Empresa empresa;

        public void Populate(NFe.Settings.Empresa empresa)
        {
            uninfeDummy.ClearControls(this, true, false);

            #region Definir um texto explicativo sobre a impressão do DANFE. Wandrey 02/02/2010
            this.tbTextoDANFE.Text = 
                "Você pode automatizar o processo de geração/impressão do documento fiscal eletrônico através do UniDANFE ou do DANFE Mon, bastando preencher os campos abaixo." +
                "\r\n\r\n" +
                "As configurações adicionais devem ser definidas no UniDANFE ou no arquivo XML auxiliar. Para maiores detalhes, consulte a documentação do UniDANFE.";
            #endregion

            this.empresa = empresa;

            tbConfiguracaoDanfe.Text = this.empresa.ConfiguracaoDanfe;
            tbConfiguracaoCCe.Text = this.empresa.ConfiguracaoCCe;
            tbPastaConfigUniDanfe.Text = this.empresa.PastaConfigUniDanfe;
            tbPastaExeUniDanfe.Text = this.empresa.PastaExeUniDanfe;
            tbPastaXmlParaDanfeMon.Text = this.empresa.PastaDanfeMon;
            cbDanfeMonNfe.Checked = this.empresa.XMLDanfeMonNFe;
            cbDanfeMonProcNfe.Checked = this.empresa.XMLDanfeMonProcNFe;
            edtEmailDANFE.Text = this.empresa.EmailDanfe;
            chkAddEmailDANFE.Checked = this.empresa.AdicionaEmailDanfe;
        }

        public void Validar()
        {
            empresa.ConfiguracaoDanfe = this.tbConfiguracaoDanfe.Text;
            empresa.ConfiguracaoCCe = this.tbConfiguracaoCCe.Text;
            empresa.PastaConfigUniDanfe = this.tbPastaConfigUniDanfe.Text;
            empresa.PastaExeUniDanfe = this.tbPastaExeUniDanfe.Text;
            empresa.PastaDanfeMon = this.tbPastaXmlParaDanfeMon.Text;
            empresa.XMLDanfeMonNFe = this.cbDanfeMonNfe.Checked;
            empresa.XMLDanfeMonProcNFe = this.cbDanfeMonProcNfe.Checked;
            empresa.EmailDanfe = edtEmailDANFE.Text;
            empresa.AdicionaEmailDanfe = chkAddEmailDANFE.Checked;
        }

        public void FocusFirstControl()
        {
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                this.tbPastaExeUniDanfe.Focus();
            };
            t.Start();
        }

        private void selectxmlenvio(object sender, MouseEventArgs e)
        {
            MetroFramework.Controls.MetroTextBox control = (MetroFramework.Controls.MetroTextBox)sender;
            int x = control.ClientRectangle.Width - control.Icon.Size.Width;
            if (e.Location.X >= x)  // a imagem foi pressionada?
            {
                if (!string.IsNullOrEmpty(control.Text))
                    this.folderBrowserDialog1.SelectedPath = control.Text;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    control.Text = this.folderBrowserDialog1.SelectedPath;
                }
                control.Focus();
                control.SelectAll();
            }
        }

        private void tbPastaExeUniDanfe_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                selectxmlenvio(tbPastaExeUniDanfe, e);
        }

        private void tbPastaConfigUniDanfe_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                selectxmlenvio(tbPastaConfigUniDanfe, e);
        }

        private void tbPastaXmlParaDanfeMon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                selectxmlenvio(tbPastaXmlParaDanfeMon, e);
        }

        private void tbPastaExeUniDanfe_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null) changeEvent(sender, e);
        }

        private void tbPastaExeUniDanfe_KeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}

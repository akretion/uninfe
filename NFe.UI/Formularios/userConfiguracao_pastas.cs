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
    [ToolboxItem(false)]
    public partial class userConfiguracao_pastas : MetroFramework.Controls.MetroUserControl
    {
        public userConfiguracao_pastas()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lbl_textBox_PastaDownload.Visible =
                textBox_PastaXmlEmLote.Visible = lbl_textBox_PastaLote.Visible = 
                textBox_PastaBackup.Visible = lbl_textBox_PastaBackup.Visible =
                textBox_PastaXmlEnviado.Visible = lbl_textBox_PastaEnviados.Visible =
                textBox_PastaDownloadNFeDest.Visible = lbl_textBox_PastaDownload.Visible = (Propriedade.TipoAplicativo == TipoAplicativo.Nfe);

            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
            {
                lbl_textBox_PastaRetornoXML.Location = new Point(lbl_textBox_PastaBackup.Location.X, 67);
                textBox_PastaXmlRetorno.Location = new Point(lbl_textBox_PastaBackup.Location.X, 86);

                lbl_textBox_PastaXmlErro.Location = new Point(lbl_textBox_PastaBackup.Location.X, 110);
                textBox_PastaXmlErro.Location = new Point(lbl_textBox_PastaBackup.Location.X, 129);

                lbl_textBox_PastaValidar.Location = new Point(lbl_textBox_PastaBackup.Location.X, 153);
                textBox_PastaValidar.Location = new Point(lbl_textBox_PastaBackup.Location.X, 172);
            }
        }

        public event EventHandler changeEvent;
        private NFe.Settings.Empresa empresa;
        private Dictionary<object, string> __oldvalues = new Dictionary<object, string>();

        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            cbCriaPastas.Checked = empresa.CriaPastasAutomaticamente;
            textBox_PastaXmlEnvio.Text = empresa.PastaXmlEnvio;
            textBox_PastaXmlRetorno.Text = empresa.PastaXmlRetorno;
            textBox_PastaXmlEnviado.Text = empresa.PastaXmlEnviado;
            textBox_PastaXmlErro.Text = empresa.PastaXmlErro;
            textBox_PastaXmlEmLote.Text = empresa.PastaXmlEmLote;
            textBox_PastaValidar.Text = empresa.PastaValidar;
            textBox_PastaDownloadNFeDest.Text = empresa.PastaDownloadNFeDest;
            textBox_PastaBackup.Text = empresa.PastaBackup;

            var Components = this.Controls.Cast<object>()
                                       .OfType<MetroFramework.Controls.MetroTextBox>();
            foreach (var c in Components)
            {
                if (!__oldvalues.ContainsKey(c))
                    __oldvalues.Add(c, c.Text);
                else
                    __oldvalues[c] = c.Text;
            }
        }

        public void Validar()
        {
            empresa.CriaPastasAutomaticamente = cbCriaPastas.Checked;
            empresa.PastaXmlEnvio = textBox_PastaXmlEnvio.Text;
            empresa.PastaXmlRetorno = textBox_PastaXmlRetorno.Text;
            empresa.PastaXmlEnviado = textBox_PastaXmlEnviado.Text;
            empresa.PastaXmlErro = textBox_PastaXmlErro.Text;
            empresa.PastaXmlEmLote = textBox_PastaXmlEmLote.Text;
            empresa.PastaValidar = textBox_PastaValidar.Text;
            empresa.PastaDownloadNFeDest = textBox_PastaDownloadNFeDest.Text;
            empresa.PastaBackup = textBox_PastaBackup.Text;
        }

        public void FocusFirstControl()
        {
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                this.textBox_PastaXmlEnvio.Focus();
            };
            t.Start();
        }

        private void textBox_PastaEnvioXML_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MetroFramework.Controls.MetroTextBox control = (MetroFramework.Controls.MetroTextBox)sender;
                int x = control.ClientRectangle.Width - control.Icon.Size.Width;
                if (e.Location.X >= x)  // a imagem foi pressionada?
                    selectxmlenvio(sender);
            }
        }

        private void textBox_PastaXmlEnvio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                selectxmlenvio(sender);
                e.Handled = true;
            }
        }

        private void selectxmlenvio(object sender)
        {
            MetroFramework.Controls.MetroTextBox control = (MetroFramework.Controls.MetroTextBox)sender;

            if (!string.IsNullOrEmpty(control.Text))
                this.folderBrowserDialog1.SelectedPath = control.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                control.Text = this.folderBrowserDialog1.SelectedPath;
            }
            control.SelectAll();
            control.Focus();
        }

        private bool dirNOTexiste(string pasta)
        {
            if (string.IsNullOrEmpty(pasta)) return false;
            return !System.IO.Directory.Exists(pasta);
        }


        private void textBox_PastaXmlEnvio_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void textBox_PastaEnvioXML_Leave(object sender, EventArgs e)
        {
            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                cbCriaPastas.Checked = dirNOTexiste(this.textBox_PastaXmlEnvio.Text) ||
                                       dirNOTexiste(this.textBox_PastaXmlRetorno.Text) ||
                                       dirNOTexiste(this.textBox_PastaXmlErro.Text) ||
                                       dirNOTexiste(this.textBox_PastaValidar.Text);
            else
                cbCriaPastas.Checked = dirNOTexiste(this.textBox_PastaBackup.Text) ||
                                       dirNOTexiste(this.textBox_PastaXmlEnviado.Text) ||
                                       dirNOTexiste(this.textBox_PastaXmlEnvio.Text) ||
                                       dirNOTexiste(this.textBox_PastaXmlRetorno.Text) ||
                                       dirNOTexiste(this.textBox_PastaXmlErro.Text) ||
                                       dirNOTexiste(this.textBox_PastaValidar.Text) ||
                                       dirNOTexiste(this.textBox_PastaXmlEmLote.Text) ||
                                       dirNOTexiste(this.textBox_PastaDownloadNFeDest.Text);

            if ((sender == this.textBox_PastaValidar && Propriedade.TipoAplicativo == TipoAplicativo.Nfse) ||
                (sender == this.textBox_PastaDownloadNFeDest && Propriedade.TipoAplicativo == TipoAplicativo.Nfe))
            {
                this.textBox_PastaXmlEnvio.Focus();
            }
        }

        private void textBox_PastaXmlEnvio_Validating(object sender, CancelEventArgs e)
        {
            MetroFramework.Controls.MetroTextBox _control = (MetroFramework.Controls.MetroTextBox)sender;

            _control.Text = _control.Text.TrimEnd('\\');

            if (__oldvalues[_control].ToString().Equals(_control.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (!string.IsNullOrEmpty(_control.Text))
            {
                foreach (var _Empresa in NFe.Settings.Empresas.Configuracoes)
                {
                    string pasta = "";
                    if (sender == this.textBox_PastaBackup)
                        pasta = _Empresa.PastaBackup;
                    else if (sender == this.textBox_PastaDownloadNFeDest)
                        pasta = _Empresa.PastaDownloadNFeDest;
                    else if (sender == this.textBox_PastaXmlEnviado)
                        pasta = _Empresa.PastaXmlEnviado;
                    else if (sender == this.textBox_PastaXmlEnvio)
                        pasta = _Empresa.PastaXmlEnvio;
                    else if (sender == this.textBox_PastaXmlEmLote)
                        pasta = _Empresa.PastaXmlEmLote;
                    else if (sender == this.textBox_PastaXmlRetorno)
                        pasta = _Empresa.PastaXmlRetorno;
                    else if (sender == this.textBox_PastaValidar)
                        pasta = _Empresa.PastaValidar;
                    else if (sender == this.textBox_PastaXmlErro)
                        pasta = _Empresa.PastaXmlErro;

                    if (!this.empresa.CNPJ.Equals(_Empresa.CNPJ) &&
                        !this.empresa.Servico.Equals(_Empresa.Servico) &&
                        pasta.Equals(_control.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "A pasta informada já existe na empresa '" + _Empresa.Nome + "'\r\n\r\nRevertendo para a pasta anterior.", "");

                        _control.Text = __oldvalues[_control].ToString();
                        _control.SelectAll();
                        _control.Focus();

                        e.Cancel = true;
                        return;
                    }
                }

                if (sender == textBox_PastaXmlEnvio)
                {
                    string[] dirs = textBox_PastaXmlEnvio.Text.Split(new char[] { '\\' });
                    string baseDir = dirs.Join('\\', dirs.Length - 1);

                    dirs = __oldvalues[_control].ToString().Split(new char[] { '\\' });
                    string baseDirOld = "";
                    if (dirs.Length > 0)
                        baseDirOld = dirs.Join('\\', dirs.Length - 1);

                    if (!baseDirOld.Equals(baseDir, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Deseja redefinir os outros diretórios para que tenham a mesma estrutura do diretório de envio?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //stopChangedEvent = true;
                            SetNewDir(textBox_PastaBackup, baseDir, "Backup");
                            SetNewDir(textBox_PastaXmlEnviado, baseDir, "Enviados");
                            SetNewDir(textBox_PastaXmlRetorno, baseDir, "Retorno");
                            SetNewDir(textBox_PastaXmlErro, baseDir, "Erro");
                            SetNewDir(textBox_PastaValidar, baseDir, "Validar");
                            SetNewDir(textBox_PastaXmlEmLote, baseDir,"EnvioEmLote");
                            SetNewDir(textBox_PastaDownloadNFeDest, baseDir, "DownloadNFe");
                            //stopChangedEvent = false;
                        }
                    }
                }
            }
            __oldvalues[_control] = _control.Text;
        }

        /// <summary>
        /// Ajusta o diretório no componente informado para o diretório base 
        /// respeitando o último nome de diretório informado
        /// </summary>
        /// <param name="textBox">Componente do tipo TextBox que será modificado</param>
        /// <param name="baseDir">diretório base para substituir no componente</param>
        private void SetNewDir(MetroFramework.Controls.MetroTextBox textBox, string baseDir, string subfolder)
        {
            string[] dirs = textBox.Text.Split(new char[] { '\\' });
            if (dirs.Length > 0 && !String.IsNullOrEmpty(textBox.Text))
            {
                string dir = dirs[dirs.Length - 1];
                textBox.Text = String.Format("{0}\\{1}", baseDir, dir);
            }
            else
                textBox.Text = String.Format("{0}\\{1}", baseDir, subfolder);
            __oldvalues[textBox] = textBox.Text;
        }
    }
}

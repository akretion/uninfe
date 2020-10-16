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
using System.IO;

/*
    • Renomeei os métodos para seguir o padrão UpperCamelCase. Isso é C# não Java
    • Corrigi a nomenclatura de métodos se o mesmo começar com o nome em inglês, se seja todo em inglês.
    Marcelo 18/05/2016
*/

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class UserConfiguracaoPastas : MetroFramework.Controls.MetroUserControl
    {
        #region Private Fields

        private Dictionary<object, string> __oldvalues = new Dictionary<object, string>();

        private NFe.Settings.Empresa empresa;

        #endregion Private Fields

        #region Public Events
        //Renomeei para seguir o padrão UpperCamelCase ... isso aqui não é java
        public event EventHandler ChangeEvent;

        #endregion Public Events

        #region Public Constructors

        public UserConfiguracaoPastas()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

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

        public void Populate(Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            ConfigurarPastas(empresa);

            textBox_PastaXmlEnvio.Text = empresa.PastaXmlEnvio;
            textBox_PastaXmlRetorno.Text = empresa.PastaXmlRetorno;
            textBox_PastaXmlEnviado.Text = empresa.PastaXmlEnviado;
            textBox_PastaXmlErro.Text = empresa.PastaXmlErro;
            textBox_PastaXmlEmLote.Text = empresa.PastaXmlEmLote;
            textBox_PastaValidar.Text = empresa.PastaValidar;
            textBox_PastaDownloadNFeDest.Text = empresa.PastaDownloadNFeDest;
            textBox_PastaBackup.Text = empresa.PastaBackup;
            cbCriaPastas.Checked = empresa.CriaPastasAutomaticamente;

            var Components = this.Controls.Cast<object>()
                                       .OfType<MetroFramework.Controls.MetroTextBox>();
            foreach (var c in Components)
            {
                if (!__oldvalues.ContainsKey(c))
                    __oldvalues.Add(c, c.Text);
                else
                    __oldvalues[c] = c.Text;
            }

            AlinharControles();
        }

        public void Validar()
        {
            empresa.CriaPastasAutomaticamente = cbCriaPastas.Checked;
            empresa.PastaXmlEnvio = textBox_PastaXmlEnvio.Text;
            empresa.PastaXmlRetorno = textBox_PastaXmlRetorno.Text;
            empresa.PastaXmlErro = textBox_PastaXmlErro.Text;
            empresa.PastaValidar = textBox_PastaValidar.Text;
            empresa.PastaDownloadNFeDest = textBox_PastaDownloadNFeDest.Text;
            empresa.PastaXmlEnviado = textBox_PastaXmlEnviado.Text;
            empresa.PastaBackup = textBox_PastaBackup.Text;
            empresa.PastaXmlEmLote = textBox_PastaXmlEmLote.Text;

            if (empresa.Servico.Equals(TipoAplicativo.Nfse))
            {
                empresa.PastaDownloadNFeDest =
                empresa.PastaXmlEnviado =
                empresa.PastaBackup =
                empresa.PastaXmlEmLote = string.Empty;
            }
            else if (empresa.Servico.Equals(TipoAplicativo.EFDReinfeSocial) ||
                empresa.Servico.Equals(TipoAplicativo.EFDReinf) ||
                empresa.Servico.Equals(TipoAplicativo.eSocial))
            {
                empresa.PastaXmlEmLote =
                empresa.PastaDownloadNFeDest = string.Empty;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Realinha os controles com base no tabindex
        /// </summary>
        private void AlinharControles()
        {
            int top = Top - VerticalScroll.Value;
            foreach (Control control in Controls.Cast<Control>()
                                               .Where(control => control.Visible)
                                               .OrderBy(control => control.TabIndex))
            {
                control.Top = top;
                top = control.Bottom + 2;
            }
        }

        private bool DirNotExists(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return !System.IO.Directory.Exists(path);
        }

        private void SelectSenderXML(object sender)
        {
            MetroFramework.Controls.MetroTextBox control = (MetroFramework.Controls.MetroTextBox)sender;

            if (!string.IsNullOrEmpty(control.Text) && Directory.Exists(control.Text))
                this.folderBrowserDialog1.SelectedPath = control.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                control.Text = this.folderBrowserDialog1.SelectedPath;
            }
            control.SelectAll();
            control.Focus();
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

        private void textBox_PastaEnvioXML_Leave(object sender, EventArgs e)
        {
            if (this.empresa.Servico == TipoAplicativo.Nfse)
                cbCriaPastas.Checked = DirNotExists(this.textBox_PastaXmlEnvio.Text) ||
                                       DirNotExists(this.textBox_PastaXmlRetorno.Text) ||
                                       DirNotExists(this.textBox_PastaXmlErro.Text) ||
                                       DirNotExists(this.textBox_PastaValidar.Text);
            else
                cbCriaPastas.Checked = DirNotExists(this.textBox_PastaBackup.Text) ||
                                       DirNotExists(this.textBox_PastaXmlEnviado.Text) ||
                                       DirNotExists(this.textBox_PastaXmlEnvio.Text) ||
                                       DirNotExists(this.textBox_PastaXmlRetorno.Text) ||
                                       DirNotExists(this.textBox_PastaXmlErro.Text) ||
                                       DirNotExists(this.textBox_PastaValidar.Text) ||
                                       DirNotExists(this.textBox_PastaXmlEmLote.Text) ||
                                       DirNotExists(this.textBox_PastaDownloadNFeDest.Text);

            if ((sender == this.textBox_PastaValidar && this.empresa.Servico == TipoAplicativo.Nfse) ||
                (sender == this.textBox_PastaDownloadNFeDest && !(empresa.Servico == TipoAplicativo.Nfse)))
            {
                this.textBox_PastaXmlEnvio.Focus();
            }
        }

        private void textBox_PastaEnvioXML_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MetroFramework.Controls.MetroTextBox control = (MetroFramework.Controls.MetroTextBox)sender;
                int x = control.ClientRectangle.Width - control.Icon.Size.Width;
                if (e.Location.X >= x)  // a imagem foi pressionada?
                    SelectSenderXML(sender);
            }
        }

        private void textBox_PastaXmlEnvio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                SelectSenderXML(sender);
                e.Handled = true;
            }
        }

        private void textBox_PastaXmlEnvio_TextChanged(object sender, EventArgs e)
        {
            if (ChangeEvent != null)
                ChangeEvent(sender, e);
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
                            SetNewDir(textBox_PastaXmlRetorno, baseDir, "Retorno");
                            SetNewDir(textBox_PastaXmlErro, baseDir, "Erro");
                            SetNewDir(textBox_PastaValidar, baseDir, "Validar");
                            if (this.empresa.Servico != TipoAplicativo.Nfse)
                            {
                                SetNewDir(textBox_PastaXmlEnviado, baseDir, "Enviados");
                                SetNewDir(textBox_PastaBackup, baseDir, "Backup");
                                SetNewDir(textBox_PastaXmlEmLote, baseDir, "EnvioEmLote");
                                SetNewDir(textBox_PastaDownloadNFeDest, baseDir, "DownloadNFe");
                            }
                        }
                    }
                }
            }
            __oldvalues[_control] = _control.Text;
        }

        private void ConfigurarPastas(Empresa empresa)
        {
            switch (empresa.Servico)
            {
                case TipoAplicativo.Nfse:
                    textBox_PastaXmlEmLote.Visible = false;
                    lbl_textBox_PastaLote.Visible = false;
                    textBox_PastaDownloadNFeDest.Visible = false;
                    lbl_textBox_PastaDownload.Visible = false;
                    textBox_PastaBackup.Visible = false;
                    lbl_textBox_PastaBackup.Visible = false;
                    textBox_PastaXmlEnviado.Visible = false;
                    lbl_textBox_PastaEnviados.Visible = false;
                    textBox_PastaXmlEnvio.Visible = true;
                    textBox_PastaXmlRetorno.Visible = true;
                    textBox_PastaXmlErro.Visible = true;
                    textBox_PastaValidar.Visible = true;
                    lbl_textBox_PastaEnvioXML.Visible = true;
                    lbl_textBox_PastaXmlErro.Visible = true;
                    lbl_textBox_PastaRetornoXML.Visible = true;
                    lbl_textBox_PastaValidar.Visible = true;
                    break;
                case TipoAplicativo.EFDReinf:
                case TipoAplicativo.eSocial:
                case TipoAplicativo.EFDReinfeSocial:
                case TipoAplicativo.GNRE:
                    textBox_PastaXmlEmLote.Visible = false;
                    lbl_textBox_PastaLote.Visible = false;
                    textBox_PastaDownloadNFeDest.Visible = false;
                    lbl_textBox_PastaDownload.Visible = false;
                    textBox_PastaBackup.Visible = true;
                    lbl_textBox_PastaBackup.Visible = true;
                    textBox_PastaXmlEnviado.Visible = true;
                    lbl_textBox_PastaEnviados.Visible = true;
                    textBox_PastaXmlEnvio.Visible = true;
                    textBox_PastaXmlRetorno.Visible = true;
                    textBox_PastaXmlErro.Visible = true;
                    textBox_PastaValidar.Visible = true;
                    lbl_textBox_PastaEnvioXML.Visible = true;
                    lbl_textBox_PastaXmlErro.Visible = true;
                    lbl_textBox_PastaRetornoXML.Visible = true;
                    lbl_textBox_PastaValidar.Visible = true;

                    break;
                case TipoAplicativo.Nfe:
                case TipoAplicativo.Cte:
                case TipoAplicativo.MDFe:
                case TipoAplicativo.NFCe:
                case TipoAplicativo.SAT:
                case TipoAplicativo.Todos:
                case TipoAplicativo.Nulo:
                default:
                    textBox_PastaXmlEmLote.Visible = true;
                    lbl_textBox_PastaLote.Visible = true;
                    textBox_PastaDownloadNFeDest.Visible = true;
                    lbl_textBox_PastaDownload.Visible = true;
                    textBox_PastaBackup.Visible = true;
                    lbl_textBox_PastaBackup.Visible = true;
                    textBox_PastaXmlEnviado.Visible = true;
                    lbl_textBox_PastaEnviados.Visible = true;
                    textBox_PastaXmlEnvio.Visible = true;
                    textBox_PastaXmlRetorno.Visible = true;
                    textBox_PastaXmlErro.Visible = true;
                    textBox_PastaValidar.Visible = true;
                    lbl_textBox_PastaEnvioXML.Visible = true;
                    lbl_textBox_PastaXmlErro.Visible = true;
                    lbl_textBox_PastaRetornoXML.Visible = true;
                    lbl_textBox_PastaValidar.Visible = true;
                    break;
            }
        }

        #endregion Private Methods
    }
}
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
    public partial class userConfiguracao_ftp : MetroFramework.Controls.MetroUserControl
    {
        public userConfiguracao_ftp()
        {
            InitializeComponent();
        }

        public event EventHandler changeEvent;
        NFe.Settings.Empresa empresa;
        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            edtFTP_Ativo.Checked = empresa.FTPAtivo;
            edtFTP_Passivo.Checked = empresa.FTPPassivo;
            edtFTP_GravaXMLPastaUnica.Checked = empresa.FTPGravaXMLPastaUnica;
            edtFTP_Password.Text = empresa.FTPSenha;
            edtFTP_PastaDestino.Text = empresa.FTPPastaAutorizados;
            edtFTP_PastaRetornos.Text = empresa.FTPPastaRetornos;
            edtFTP_Porta.Text = empresa.FTPPorta.ToString();
            edtFTP_Server.Text = empresa.FTPNomeDoServidor;
            edtFTP_UserName.Text = empresa.FTPNomeDoUsuario;

            edtFTP_PastaDestino.Visible =
                lbl_edtFTP_PastaDestino.Visible =
                edtFTP_GravaXMLPastaUnica.Visible = (empresa.Servico == TipoAplicativo.Nfe || empresa.Servico == TipoAplicativo.Todos);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void Validar()
        {
            this.empresa.FTPAtivo = this.edtFTP_Ativo.Checked;
            this.empresa.FTPPassivo = this.edtFTP_Passivo.Checked;
            this.empresa.FTPGravaXMLPastaUnica = this.edtFTP_GravaXMLPastaUnica.Checked;
            this.empresa.FTPSenha = this.edtFTP_Password.Text;
            this.empresa.FTPPastaAutorizados = this.edtFTP_PastaDestino.Text;
            this.empresa.FTPPastaRetornos = this.edtFTP_PastaRetornos.Text;
            this.empresa.FTPPorta = Convert.ToInt32(this.edtFTP_Porta.Text);
            this.empresa.FTPNomeDoServidor = this.edtFTP_Server.Text;
            this.empresa.FTPNomeDoUsuario = this.edtFTP_UserName.Text;            
        }

        public void FocusFirstControl()
        {
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                edtFTP_Server.Focus();
            };
            t.Start();
        }

        private void btnTestarFTP_Click(object sender, EventArgs e)
        {
            FTP ftp = null;
            try
            {
                ftp = new FTP(this.edtFTP_Server.Text, Convert.ToInt32("0" + this.edtFTP_Porta.Text), this.edtFTP_UserName.Text, this.edtFTP_Password.Text);
                ftp.Connect();
                if (ftp.IsConnected)
                {
                    string vCurrente = ftp.GetWorkingDirectory();

                    if (this.empresa.Servico == TipoAplicativo.Nfe || this.empresa.Servico == TipoAplicativo.Todos)
                        if (!ftp.changeDir(this.edtFTP_PastaDestino.Text))
                        {
                            string error = "Pasta '" + this.edtFTP_PastaDestino.Text + "' não existe no FTP.\r\nDesejá criá-la agora?";
                            if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, error, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                ftp.makeDir(this.edtFTP_PastaDestino.Text);
                            }
                        }

                    ftp.ChangeDir(vCurrente);

                    if (!string.IsNullOrEmpty(this.edtFTP_PastaRetornos.Text))
                        if (!ftp.changeDir(this.edtFTP_PastaRetornos.Text))
                        {
                            string error = "Pasta '" + this.edtFTP_PastaRetornos.Text + "' não existe no FTP.";
                            if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, error, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                ftp.makeDir(this.edtFTP_PastaRetornos.Text);
                            }
                        }

                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "FTP conectado com sucesso!", "");
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ftp != null)
                    if (ftp.IsConnected)
                        ftp.Disconnect();
            }
        }

        private void edtFTP_Server_TextChanged(object sender, EventArgs e)
        {
            if (this.changeEvent != null)
                this.changeEvent(sender, e);
        }

        private void edtFTP_Ativo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.changeEvent != null)
                this.changeEvent(sender, e);
        }
    }
}

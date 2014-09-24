using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

using NFe.Certificado;
using NFe.Components;
using NFe.Settings;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_certificado : MetroFramework.Controls.MetroUserControl
    {
        public userConfiguracao_certificado()
        {
            InitializeComponent();
        }

        public event EventHandler changeEvent;
        private NFe.Settings.Empresa empresa;
        private X509Certificate2 oMeuCert;

        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            textBox_dadoscertificado.BackColor = txtArquivoCertificado.BackColor;
            textBox_dadoscertificado.Height = 210;

            if (!string.IsNullOrEmpty(empresa.CNPJ))
            {
                try
                {
                    empresa.X509Certificado = empresa.BuscaConfiguracaoCertificado();
                }
                catch
                {
                    //Se der algum erro na hora de buscar o certificado, o sistema tem que permitir o usuário continuar com a configuração para que ele acerte o erro. Wandrey 19/09/2014
                }

                oMeuCert = empresa.X509Certificado;
            }
            else
                oMeuCert = null;

            ckbCertificadoInstalado.Checked = empresa.CertificadoInstalado;
            if (empresa.CertificadoInstalado)
            {
                DemonstraDadosCertificado();
                txtPinCertificado.Text = empresa.CertificadoPIN;
            }
            else
            {
                txtArquivoCertificado.Text = empresa.CertificadoArquivo;
                txtSenhaCertificado.Text = empresa.CertificadoSenha;
            }
            ckbCertificadoInstalado_CheckedChanged(null, null);
        }

        public void Validar()
        {
            empresa.CertificadoInstalado = ckbCertificadoInstalado.Checked;
            empresa.CertificadoArquivo = txtArquivoCertificado.Text;
            empresa.CertificadoSenha = txtSenhaCertificado.Text;
            empresa.Certificado = (this.oMeuCert == null ? empresa.Certificado : oMeuCert.Subject.ToString());
            empresa.CertificadoDigitalThumbPrint = (this.oMeuCert == null ? empresa.CertificadoDigitalThumbPrint : oMeuCert.Thumbprint);
            empresa.CertificadoPIN = txtPinCertificado.Text;
        }

        public void FocusFirstControl()
        {
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                var Components = this.Controls.Cast<object>().OfType<MetroFramework.Controls.MetroTextBox>();
                foreach (var c in Components)
                {
                    if (c.Enabled && !c.ReadOnly)
                    {
                        c.Focus();
                        break;
                    }
                }
            };
            t.Start();
        }

        private void button_selecionar_certificado_Click(object sender, EventArgs e)
        {
            if (this.ckbCertificadoInstalado.Checked)
            {
                CertificadoDigital oCertDig = new CertificadoDigital();

                if (oCertDig.SelecionarCertificado() == true)
                {
                    oMeuCert = oCertDig.oCertificado;
                    this.empresa.Certificado = oMeuCert.Subject;
                    this.empresa.CertificadoDigitalThumbPrint = oMeuCert.Thumbprint;
                    this.empresa.X509Certificado = oMeuCert;
                    DemonstraDadosCertificado();
                    if (changeEvent != null)
                        changeEvent(sender, e);
                }
            }
            else
            {
                if (File.Exists(txtArquivoCertificado.Text))
                {
                    FileInfo arq = new FileInfo(txtArquivoCertificado.Text);
                    this.openFileDialog1.InitialDirectory = arq.DirectoryName;
                    this.openFileDialog1.FileName = txtArquivoCertificado.Text;
                }
                else
                {
                    this.openFileDialog1.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
                    this.openFileDialog1.FileName = null;
                }

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtArquivoCertificado.Text = this.openFileDialog1.FileName;
                }
            }
        }

        #region DemonstraDadosCertificado()
        private void DemonstraDadosCertificado()
        {
            if (oMeuCert != null)
            {
                DateTime hoje = DateTime.Now;
                TimeSpan dif = oMeuCert.NotAfter.Subtract(hoje);
                string mensagemRestante;

                if (dif.Days > 0)
                {
                    mensagemRestante = "Faltam " + dif.Days + " dias para vencer o certificado.";
                }
                else
                {
                    mensagemRestante = "Certificado vencido a " + (dif.Days) * -1 + " dias.";
                    if (this.textBox_dadoscertificado.BackColor != Color.Red)
                        this.textBox_dadoscertificado.BackColor = Color.Red;
                }
                this.textBox_dadoscertificado.Text =
                    "[Sujeito]\r\n" + oMeuCert.Subject + "\r\n\r\n" +
                    "[Validade]\r\n" + oMeuCert.NotBefore + " à " + oMeuCert.NotAfter + "\r\n" + mensagemRestante + "\r\n\r\n" +
                    "[ThumbPrint]\r\n" + oMeuCert.Thumbprint;
            }
            else
            {
                // Comparação feita para demonstrar possiveis certificados A3 que podem não estar presentes ou detectados. Renan - 18/06/2013
                if (string.IsNullOrEmpty(this.empresa.Certificado))
                {
                    textBox_dadoscertificado.Clear();
                }
                else
                {
                    this.textBox_dadoscertificado.Text =
                        "[Sujeito]\r\n" + this.empresa.Certificado + "\r\n\r\n" +
                        "[ThumbPrint]\r\n" + this.empresa.CertificadoDigitalThumbPrint + "\r\n\r\n" +
                        "[Alerta]\r\n" + "Certificado não foi Detectado na Estação! Podem ocorrer erros na emissão de documentos.";
                }
            }
        }
        #endregion

        private void ckbCertificadoInstalado_CheckedChanged(object sender, EventArgs e)
        {
            //button_selecionar_certificado.Enabled = ckbCertificadoInstalado.Checked;
            lblSenhaCertificado.Visible =
                txtSenhaCertificado.Visible =
                lblArquivoCertificado.Visible =
                txtArquivoCertificado.Visible = !ckbCertificadoInstalado.Checked;

            lblCerificadoInstalado.Visible =
                textBox_dadoscertificado.Visible = 
                lblPinCertificado.Visible =
                txtPinCertificado.Visible = ckbCertificadoInstalado.Checked;

            if (!ckbCertificadoInstalado.Checked)
            {
                textBox_dadoscertificado.Text = "";
                oMeuCert = null;
                empresa.Certificado = "";
                empresa.X509Certificado = oMeuCert;

                lblArquivoCertificado.Location = new Point(lblCerificadoInstalado.Location.X, lblCerificadoInstalado.Location.Y);
                lblArquivoCertificado.Refresh();
                txtArquivoCertificado.Location = new Point(textBox_dadoscertificado.Location.X, textBox_dadoscertificado.Location.Y);
                txtArquivoCertificado.Refresh();

                lblSenhaCertificado.Location = new Point(lblCerificadoInstalado.Location.X, 72);
                lblSenhaCertificado.Refresh();
                txtSenhaCertificado.Location = new Point(lblCerificadoInstalado.Location.X, 90);
                txtSenhaCertificado.Refresh();
            }
            else
            {
                lblPinCertificado.Location = new Point(textBox_dadoscertificado.Location.X, textBox_dadoscertificado.Location.Y + textBox_dadoscertificado.Size.Height + 10);
                txtPinCertificado.Location = new Point(lblPinCertificado.Location.X, lblPinCertificado.Location.Y + lblPinCertificado.Size.Height + 7);
            }
            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void txtArquivoCertificado_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);
        }
    }
}

using NFe.Certificado;
using NFe.Components;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Unimake.Business.DFe.Security;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_certificado: MetroFramework.Controls.MetroUserControl
    {
        public userConfiguracao_certificado() => InitializeComponent();

        public event EventHandler changeEvent;
        private NFe.Settings.Empresa empresa;
        private X509Certificate2 oMeuCert;
        public Formularios.UserConfiguracaoPastas ucPastas { private get; set; }

        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            var tltValidarProvider = new System.Windows.Forms.ToolTip();
            tltValidarProvider.SetToolTip(btnValidarProvider, "Testar o PIN informado.");

            textBox_dadoscertificado.BackColor = txtArquivoCertificado.BackColor;
            textBox_dadoscertificado.Height = 160;
            ckbUsaCertificado.Checked = empresa.UsaCertificado;
            ckbUsaCertificado.Enabled = (empresa.Servico == TipoAplicativo.Nfse);

            oMeuCert = null;

            if(empresa.UsaCertificado)
            {
                if(!string.IsNullOrEmpty(empresa.CNPJ))
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

                ckbUsarCertificadoInstalado.Checked = empresa.CertificadoInstalado;
                if(empresa.CertificadoInstalado)
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

                HabilitaComponentesPINA3();
            }
            else
            {
                oMeuCert = null;
            }
        }

        public void Validar(bool salvando = true)
        {
            empresa.CertificadoInstalado = ckbUsarCertificadoInstalado.Checked && ckbUsaCertificado.Checked;
            empresa.CertificadoArquivo = ckbUsaCertificado.Checked ? txtArquivoCertificado.Text : "";
            empresa.CertificadoSenha = ckbUsaCertificado.Checked ? txtSenhaCertificado.Text : "";
            empresa.Certificado = (ckbUsaCertificado.Checked ? (oMeuCert == null ? empresa.Certificado : oMeuCert.Subject.ToString()) : "");
            empresa.CertificadoDigitalThumbPrint = (ckbUsaCertificado.Checked ? (oMeuCert == null ? empresa.CertificadoDigitalThumbPrint : oMeuCert.Thumbprint) : "");
            empresa.CertificadoPIN = ckbUsaCertificado.Checked ? txtPinCertificado.Text : "";
            empresa.UsaCertificado = ckbUsaCertificado.Checked;

            if(ckbUsaCertificado.Checked)
            {
                if(!string.IsNullOrEmpty(empresa.CertificadoPIN))
                {
                    if(salvando)
                    {
                        ValidarCertificadoA3(true);
                    }
                }
            }
        }

        public void FocusFirstControl()
        {
            var t = new Timer
            {
                Interval = 50
            };
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                var Components = Controls.Cast<object>().OfType<MetroFramework.Controls.MetroTextBox>();
                foreach(var c in Components)
                {
                    if(c.Enabled && !c.ReadOnly)
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
            try
            {
                if(ckbUsarCertificadoInstalado.Checked)
                {
                    var oCertDig = new CertificadoDigital();

                    if(oCertDig.SelecionarCertificado() == true)
                    {
                        oMeuCert = oCertDig.oCertificado;
                        empresa.Certificado = oMeuCert.Subject;
                        empresa.CertificadoDigitalThumbPrint = oMeuCert.Thumbprint;
                        empresa.X509Certificado = oMeuCert;
                        DemonstraDadosCertificado();

                        if(changeEvent != null)
                        {
                            changeEvent(sender, e);
                        }
                    }
                }
                else
                {
                    if(File.Exists(txtArquivoCertificado.Text))
                    {
                        var arq = new FileInfo(txtArquivoCertificado.Text);
                        openFileDialog1.InitialDirectory = arq.DirectoryName;
                        openFileDialog1.FileName = txtArquivoCertificado.Text;
                    }
                    else
                    {
                        openFileDialog1.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
                        openFileDialog1.FileName = null;
                    }
                    if(openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        txtArquivoCertificado.Text = openFileDialog1.FileName;
                    }
                }

                HabilitaComponentesPINA3();
            }
            catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                      ex.Message,
                                      "Seleção de certificado digital.",
                                      MessageBoxButtons.OK);
            }
        }

        #region DemonstraDadosCertificado()
        private void DemonstraDadosCertificado()
        {
            if(ckbUsaCertificado.Checked)
            {
                if(oMeuCert != null)
                {
                    var hoje = DateTime.Now;
                    var dif = oMeuCert.NotAfter.Subtract(hoje);
                    string mensagemRestante;

                    if(dif.Days > 0)
                    {
                        mensagemRestante = "Faltam " + dif.Days + " dias para vencer o certificado.";
                    }
                    else
                    {
                        mensagemRestante = "Certificado vencido a " + (dif.Days) * -1 + " dias.";
                        if(textBox_dadoscertificado.BackColor != Color.Red)
                        {
                            textBox_dadoscertificado.BackColor = Color.Red;
                        }
                    }
                    textBox_dadoscertificado.Text =
                        "[Sujeito]\r\n" + oMeuCert.Subject + "\r\n\r\n" +
                        "[Validade]\r\n" + oMeuCert.NotBefore + " à " + oMeuCert.NotAfter + "\r\n" + mensagemRestante + "\r\n\r\n" +
                        "[ThumbPrint]\r\n" + oMeuCert.Thumbprint;
                }
                else
                {
                    // Comparação feita para demonstrar possiveis certificados A3 que podem não estar presentes ou detectados. Renan - 18/06/2013
                    if(string.IsNullOrEmpty(empresa.Certificado))
                    {
                        textBox_dadoscertificado.Clear();
                    }
                    else
                    {
                        textBox_dadoscertificado.Text =
                            "[Sujeito]\r\n" + empresa.Certificado + "\r\n\r\n" +
                            "[ThumbPrint]\r\n" + empresa.CertificadoDigitalThumbPrint + "\r\n\r\n" +
                            "[Alerta]\r\n" + "Certificado não foi Detectado na Estação! Podem ocorrer erros na emissão de documentos.";
                    }
                }
            }
            else
            {
                textBox_dadoscertificado.Clear();
                txtArquivoCertificado.Clear();
                txtSenhaCertificado.Clear();
            }
        }
        #endregion

        private void ckbCertificadoInstalado_CheckedChanged(object sender, EventArgs e)
        {
            //button_selecionar_certificado.Enabled = ckbCertificadoInstalado.Checked;
            lblSenhaCertificado.Visible =
                txtSenhaCertificado.Visible =
                lblArquivoCertificado.Visible =
                txtArquivoCertificado.Visible = !ckbUsarCertificadoInstalado.Checked;

            lblCerificadoInstalado.Visible =
                textBox_dadoscertificado.Visible = ckbUsarCertificadoInstalado.Checked;

            if(!ckbUsarCertificadoInstalado.Checked)
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

            HabilitaComponentesPINA3();

            if(changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void txtArquivoCertificado_TextChanged(object sender, EventArgs e)
        {
            if(changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        /// <summary>
        /// Habilita os componentes na tela para digitação do PIN do certificado A3
        /// </summary>
        private void HabilitaComponentesPINA3()
        {
            var isA3 = false;

            if(ckbUsarCertificadoInstalado.Checked && ckbUsaCertificado.Checked)
            {
                isA3 = empresa.X509Certificado.IsA3();
            }

            if(isA3)
            {
                btnValidarProvider.Visible = true;
                txtPinCertificado.Visible = true;
                lblPinCertificado.Visible = true;
                btnValidarProvider.Enabled = true;
                txtPinCertificado.Enabled = true;
                lblPinCertificado.Enabled = true;
            }
            else
            {
                btnValidarProvider.Visible = false;
                txtPinCertificado.Visible = false;
                lblPinCertificado.Visible = false;
                btnValidarProvider.Enabled = false;
                txtPinCertificado.Enabled = false;
                lblPinCertificado.Enabled = false;
            }
        }

        private void ckbTemCertificadoInstalado_CheckedChanged(object sender, EventArgs e)
        {
            ckbUsarCertificadoInstalado.Enabled =
                button_selecionar_certificado.Enabled =
                textBox_dadoscertificado.Enabled =
                txtArquivoCertificado.Enabled =
                txtSenhaCertificado.Enabled = ckbUsaCertificado.Checked;

            HabilitaComponentesPINA3();

            if(changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void cboProviders_TextChanged(object sender, EventArgs e)
        {
            if(changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private const string provError = "Validação do PIN A3:";

        private void btnValidarProvider_Click(object sender, EventArgs e)
        {
            try
            {
                Validar(false);
                empresa.PastaXmlEnvio = ucPastas.textBox_PastaXmlEnvio.Text;
                ValidarCertificadoA3(false);
            }
            catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                      ex.Message,
                                      provError,
                                      MessageBoxButtons.OK);
            }
        }

        private void ValidarCertificadoA3(bool salvando)
        {
            if(string.IsNullOrEmpty(empresa.CertificadoPIN))
            {
                throw new Exception("Informe o PIN do certificado");
            }

            Wait.Show("Validando PIN...");

            try
            {
                if(!empresa.CertificadoPINCarregado)
                {
                    empresa.X509Certificado.SetPinPrivateKey(empresa.CertificadoPIN);
                    empresa.CertificadoPINCarregado = true;
                }

                Wait.Close();

                if(!salvando)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                        "PIN do certificado A3 validado com sucesso.",
                        provError,
                        MessageBoxButtons.OK);
                }
            }
            catch(Exception ex)
            {
                Wait.Close();

                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                    "PIN do certificado A3 é inválido.",
                    provError,
                    MessageBoxButtons.OK);

                throw ex;
            }
        }
    }
}

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
        public Formularios.userConfiguracao_pastas ucPastas { private get; set; }

        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            System.Windows.Forms.ToolTip tltBuscarProvider = new System.Windows.Forms.ToolTip();
            tltBuscarProvider.SetToolTip(btnBuscarProvider, "Pesquisar provider válido automaticamente");

            System.Windows.Forms.ToolTip tltValidarProvider = new System.Windows.Forms.ToolTip();
            tltValidarProvider.SetToolTip(btnValidarProvider, "Validar provider selecionado");

            textBox_dadoscertificado.BackColor = txtArquivoCertificado.BackColor;
            textBox_dadoscertificado.Height = 160;
            ckbUsaCertificado.Checked = empresa.UsaCertificado;
            ckbUsaCertificado.Enabled = (empresa.Servico == TipoAplicativo.Nfse);

            oMeuCert = null;

            if (empresa.UsaCertificado)
            {
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

                ckbUsarCertificadoInstalado.Checked = empresa.CertificadoInstalado;
                if (empresa.CertificadoInstalado)
                {
                    DemonstraDadosCertificado();
                    txtPinCertificado.Text = empresa.CertificadoPIN;
                    ProvidersCertificado();
                }
                else
                {
                    txtArquivoCertificado.Text = empresa.CertificadoArquivo;
                    txtSenhaCertificado.Text = empresa.CertificadoSenha;
                }
                ckbCertificadoInstalado_CheckedChanged(null, null);
            }
            else
                oMeuCert = null;
        }

        private void ProvidersCertificado()
        {
            CertificadoDigital oCertificado = new CertificadoDigital();
            List<CertProviders> providers = new List<CertProviders>();
            cboProviders.Items.Clear();

            providers = oCertificado.GetListProviders();

            foreach (CertProviders certinfo in providers)
            {
                cboProviders.Items.Add(certinfo.NameKey);
            }

            if (!String.IsNullOrEmpty(empresa.ProviderCertificado))
                cboProviders.SelectedItem = empresa.ProviderCertificado;
            else
                IdentificarProviderValido();
        }

        public void Validar(bool salvando = true)
        {
            empresa.CertificadoInstalado = ckbUsarCertificadoInstalado.Checked && ckbUsaCertificado.Checked;
            empresa.CertificadoArquivo = ckbUsaCertificado.Checked ? txtArquivoCertificado.Text : "";
            empresa.CertificadoSenha = ckbUsaCertificado.Checked ? txtSenhaCertificado.Text : "";
            empresa.Certificado = (ckbUsaCertificado.Checked ? (this.oMeuCert == null ? empresa.Certificado : oMeuCert.Subject.ToString()) : "");
            empresa.CertificadoDigitalThumbPrint = (ckbUsaCertificado.Checked ? (this.oMeuCert == null ? empresa.CertificadoDigitalThumbPrint : oMeuCert.Thumbprint) : "");
            empresa.CertificadoPIN = ckbUsaCertificado.Checked ? txtPinCertificado.Text : "";
            empresa.UsaCertificado = ckbUsaCertificado.Checked;
            empresa.ProviderCertificado = 
                empresa.ProviderTypeCertificado = "";

            if (ckbUsaCertificado.Checked)
            {
                if (!String.IsNullOrEmpty(empresa.CertificadoPIN))
                {
                    CertificadoDigital oCertificado = new CertificadoDigital();
                    CertProviders providerInfo = new CertProviders();
                    providerInfo = oCertificado.GetInfoProvider(cboProviders.SelectedItem.ToString());
                    empresa.ProviderCertificado = providerInfo.NameKey;
                    empresa.ProviderTypeCertificado = providerInfo.Type;

                    if (salvando)
                        ValidarCertificadoA3(true);
                }
            }
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
            if (this.ckbUsarCertificadoInstalado.Checked)
            {
                CertificadoDigital oCertDig = new CertificadoDigital();

                if (oCertDig.SelecionarCertificado() == true)
                {
                    oMeuCert = oCertDig.oCertificado;
                    this.empresa.Certificado = oMeuCert.Subject;
                    this.empresa.CertificadoDigitalThumbPrint = oMeuCert.Thumbprint;
                    this.empresa.X509Certificado = oMeuCert;
                    DemonstraDadosCertificado();
                    IdentificarProviderValido();
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
            if (this.ckbUsaCertificado.Checked)
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
            else
            {
                this.textBox_dadoscertificado.Clear();
                this.txtArquivoCertificado.Clear();
                this.txtPinCertificado.Clear();
                this.txtSenhaCertificado.Clear();
            }
        }
        #endregion

        private void ckbCertificadoInstalado_CheckedChanged(object sender, EventArgs e)
        {
            //button_selecionar_certificado.Enabled = ckbCertificadoInstalado.Checked;
            lblSenhaCertificado.Visible =
                txtSenhaCertificado.Visible =
                lblArquivoCertificado.Visible =
                txtArquivoCertificado.Visible =
                btnBuscarProvider.Visible =
                btnValidarProvider.Visible = !ckbUsarCertificadoInstalado.Checked;

            lblCerificadoInstalado.Visible =
                textBox_dadoscertificado.Visible =
                lblPinCertificado.Visible =
                txtPinCertificado.Visible =
                btnBuscarProvider.Visible =
                btnValidarProvider.Visible = ckbUsarCertificadoInstalado.Checked;

            lblPinCertificado.Visible =
                txtPinCertificado.Visible = true;

            lblProvider.Visible =
                   cboProviders.Visible = true;

            if (!ckbUsarCertificadoInstalado.Checked)
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

                lblPinCertificado.Visible =
                    txtPinCertificado.Visible = false;
                lblProvider.Visible =
                    cboProviders.Visible = false;
            }
            else
            {
                lblPinCertificado.Location = new Point(textBox_dadoscertificado.Location.X, textBox_dadoscertificado.Location.Y + textBox_dadoscertificado.Size.Height + 10);
                txtPinCertificado.Location = new Point(lblPinCertificado.Location.X, lblPinCertificado.Location.Y + lblPinCertificado.Size.Height + 7);

                lblProvider.Location = new Point(txtPinCertificado.Location.X, txtPinCertificado.Location.Y + txtPinCertificado.Size.Height + 10);
                cboProviders.Location = new Point(lblProvider.Location.X, lblProvider.Location.Y + lblProvider.Size.Height + 7);
            }
            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void txtArquivoCertificado_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);

            if (clsX509Certificate2Extension.IsA3(empresa.X509Certificado))
            {
                if (String.IsNullOrEmpty(txtPinCertificado.Text))
                    cboProviders.Enabled = false;
                else
                    cboProviders.Enabled = true;

                txtPinCertificado.Enabled = true;

                //IdentificarProviderValido();
            }
            else
            {
                txtPinCertificado.Enabled = false;
                cboProviders.Enabled = false;
            }
        }

        private void ckbTemCertificadoInstalado_CheckedChanged(object sender, EventArgs e)
        {
            this.ckbUsarCertificadoInstalado.Enabled =
                this.button_selecionar_certificado.Enabled =
                this.textBox_dadoscertificado.Enabled =
                this.txtArquivoCertificado.Enabled =
                this.txtPinCertificado.Enabled =
                this.txtSenhaCertificado.Enabled = this.ckbUsaCertificado.Checked;
            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void cboProviders_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void btnBuscarProvider_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Validar(false);
                this.empresa.PastaXmlEnvio = this.ucPastas.textBox_PastaXmlEnvio.Text;
                this.IdentificarProviderValido();
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                      ex.Message,
                                      "Buscar provider - Resultado:",
                                      MessageBoxButtons.OK);
            }
        }

        private const string provError = "Validação do Provider - Resultado:";

        private void btnValidarProvider_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validar(false);
                this.empresa.PastaXmlEnvio = this.ucPastas.textBox_PastaXmlEnvio.Text;
                this.ValidarCertificadoA3(false);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                      ex.Message,
                                      provError,
                                      MessageBoxButtons.OK);
            }
        }

        private void ValidarCertificadoA3(bool salvando)
        {
            if (String.IsNullOrEmpty(empresa.CertificadoPIN))
                throw new Exception("Informe o PIN do certificado");

            if (cboProviders.SelectedItem.ToString() == "")
                throw new Exception("Informe o provedor do certificado");

            Wait.Show("Validando provider...");
            try
            {
                CertificadoProviders certificadoProviders = new CertificadoProviders(empresa.X509Certificado,
                                                                                     empresa.PastaXmlEnvio,
                                                                                     Empresas.FindEmpresaByThread(),
                                                                                     empresa.CertificadoPIN);
                CertProviders xCertProviders = new CertProviders();
                xCertProviders.NameKey = cboProviders.SelectedItem.ToString();
                xCertProviders.Type = certificadoProviders.GetProviderType(xCertProviders.NameKey);

                if (certificadoProviders.TestarProvider(xCertProviders))
                {
                    Wait.Close();
                    if (!salvando)
                        MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                                              "Provider válido, XML assinado com sucesso.",
                                                              provError,
                                                              MessageBoxButtons.OK);
                }

                else
                {
                    Wait.Close();
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                          "Provider inválido, não foi possível assinar um XML com este provider.",
                                          provError,
                                          MessageBoxButtons.OK);

                    if (salvando)
                    {
                        throw new Exception("Não foi possível salvar a configuração.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Wait.Close();
            }
        }

        private void IdentificarProviderValido()
        {
            Wait.Show("Procurando por um provider válido...");
            CertificadoProviders certificadoProviders = new CertificadoProviders(empresa.X509Certificado,
                                                                                 empresa.PastaXmlEnvio,
                                                                                 Empresas.FindEmpresaByThread(),
                                                                                 txtPinCertificado.Text);
            if (certificadoProviders.Run())
            {
                Wait.Close();
                if (certificadoProviders.ProviderIdentificado)
                {
                    DialogResult result = MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                                          "Foi identificado um provider para o Certificado: " + certificadoProviders.ProviderValido.NameKey + "\n" +
                                                          "Deseja defini-lo com provider para este certificado?", "Identificação automatica de Provider",
                                                          MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cboProviders.SelectedItem = certificadoProviders.ProviderValido.NameKey;
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                                          "Não foi identificado um provider para o funcionamento adequado do Certificado selecionado \n" +
                                                          "Tente reiniciar o Certificado e a Senha do PIN e tente novamente.", "Identificação automatica de Provider",
                                                          MessageBoxButtons.OK);
                }
            }
            else
                Wait.Close();
        }
    }
}

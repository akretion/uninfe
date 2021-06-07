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
using Unimake.Business.DFe.Security;

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
        public Formularios.UserConfiguracaoPastas ucPastas { private get; set; }

        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.empresa = empresa;
            uninfeDummy.ClearControls(this, true, false);

            System.Windows.Forms.ToolTip tltValidarProvider = new System.Windows.Forms.ToolTip();
            tltValidarProvider.SetToolTip(btnValidarProvider, "Testar o PIN informado.");

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
                oMeuCert = null;
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

            if (ckbUsaCertificado.Checked)
            {
                if (!String.IsNullOrEmpty(empresa.CertificadoPIN))
                {
                    CertificadoDigital oCertificado = new CertificadoDigital();
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
            try
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
                txtArquivoCertificado.Visible = !ckbUsarCertificadoInstalado.Checked;

            lblCerificadoInstalado.Visible =
                textBox_dadoscertificado.Visible = ckbUsarCertificadoInstalado.Checked;

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
            }
            else
            {
                lblPinCertificado.Location = new Point(textBox_dadoscertificado.Location.X, textBox_dadoscertificado.Location.Y + textBox_dadoscertificado.Size.Height + 10);
                txtPinCertificado.Location = new Point(lblPinCertificado.Location.X, lblPinCertificado.Location.Y + lblPinCertificado.Size.Height + 7);
            }

            HabilitaComponentesPINA3();

            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void txtArquivoCertificado_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);
        }

        /// <summary>
        /// Habilita os componentes na tela para digitação do PIN do certificado A3
        /// </summary>
        private void HabilitaComponentesPINA3()
        {
            bool isA3 = false;

#if _fw46
            if (this.ckbUsarCertificadoInstalado.Checked && ckbUsaCertificado.Checked)
               isA3 = empresa.X509Certificado.IsA3();
#endif

            if (isA3)
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
            this.ckbUsarCertificadoInstalado.Enabled =
                this.button_selecionar_certificado.Enabled =
                this.textBox_dadoscertificado.Enabled =
                this.txtArquivoCertificado.Enabled =
                this.txtSenhaCertificado.Enabled = this.ckbUsaCertificado.Checked;

            HabilitaComponentesPINA3();

            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void cboProviders_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);
        }
        
        private const string provError = "Validação do PIN - Resultado:";

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

            Wait.Show("Validando PIN...");
            try
            {
                if (Empresas.FindConfEmpresaIndex(empresa.CNPJ, empresa.Servico) == -1)
                    Empresas.Configuracoes.Add(empresa);

                CertificadoProviders certificadoProviders = new CertificadoProviders(empresa.X509Certificado,
                                                                                     empresa.PastaXmlEnvio,
                                                                                     Empresas.FindConfEmpresaIndex(empresa.CNPJ, empresa.Servico),
                                                                                     empresa.CertificadoPIN);
                CertProviders xCertProviders = new CertProviders();

                if (certificadoProviders.TestarProvider(xCertProviders))
                {
                    Wait.Close();
                    if (!salvando)
                        MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                                              "Configuração do PIN validada, XML assinado com sucesso.",
                                                              provError,
                                                              MessageBoxButtons.OK);
                }

                else
                {
                    Wait.Close();
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                          "PIN inválido, não foi possível assinar um XML com esta chave.",
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
    }
}

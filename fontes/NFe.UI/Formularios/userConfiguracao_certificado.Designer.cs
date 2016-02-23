namespace NFe.UI.Formularios
{
    partial class userConfiguracao_certificado
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPinCertificado = new MetroFramework.Controls.MetroTextBox();
            this.lblPinCertificado = new MetroFramework.Controls.MetroLabel();
            this.txtSenhaCertificado = new MetroFramework.Controls.MetroTextBox();
            this.lblSenhaCertificado = new MetroFramework.Controls.MetroLabel();
            this.txtArquivoCertificado = new MetroFramework.Controls.MetroTextBox();
            this.lblArquivoCertificado = new MetroFramework.Controls.MetroLabel();
            this.textBox_dadoscertificado = new MetroFramework.Controls.MetroTextBox();
            this.lblCerificadoInstalado = new MetroFramework.Controls.MetroLabel();
            this.ckbUsarCertificadoInstalado = new MetroFramework.Controls.MetroCheckBox();
            this.button_selecionar_certificado = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ckbUsaCertificado = new MetroFramework.Controls.MetroCheckBox();
            this.btnValidarProvider = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.button_selecionar_certificado)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPinCertificado
            // 
            this.txtPinCertificado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPinCertificado.Enabled = false;
            this.txtPinCertificado.Lines = new string[] {
        "Styled Textbox"};
            this.txtPinCertificado.Location = new System.Drawing.Point(3, 292);
            this.txtPinCertificado.MaxLength = 32767;
            this.txtPinCertificado.Name = "txtPinCertificado";
            this.txtPinCertificado.PasswordChar = '#';
            this.txtPinCertificado.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPinCertificado.SelectedText = "";
            this.txtPinCertificado.Size = new System.Drawing.Size(605, 22);
            this.txtPinCertificado.TabIndex = 9;
            this.txtPinCertificado.Text = "Styled Textbox";
            this.txtPinCertificado.UseSelectable = true;
            this.txtPinCertificado.UseStyleColors = true;
            this.txtPinCertificado.Visible = false;
            this.txtPinCertificado.TextChanged += new System.EventHandler(this.txtArquivoCertificado_TextChanged);
            // 
            // lblPinCertificado
            // 
            this.lblPinCertificado.AutoSize = true;
            this.lblPinCertificado.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblPinCertificado.Location = new System.Drawing.Point(3, 273);
            this.lblPinCertificado.Margin = new System.Windows.Forms.Padding(3);
            this.lblPinCertificado.Name = "lblPinCertificado";
            this.lblPinCertificado.Size = new System.Drawing.Size(151, 15);
            this.lblPinCertificado.TabIndex = 8;
            this.lblPinCertificado.Text = "PIN Certificado A3 (opcional)";
            // 
            // txtSenhaCertificado
            // 
            this.txtSenhaCertificado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSenhaCertificado.Lines = new string[] {
        "Styled Textbox"};
            this.txtSenhaCertificado.Location = new System.Drawing.Point(3, 247);
            this.txtSenhaCertificado.MaxLength = 32767;
            this.txtSenhaCertificado.Name = "txtSenhaCertificado";
            this.txtSenhaCertificado.PasswordChar = '#';
            this.txtSenhaCertificado.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSenhaCertificado.SelectedText = "";
            this.txtSenhaCertificado.Size = new System.Drawing.Size(605, 22);
            this.txtSenhaCertificado.TabIndex = 7;
            this.txtSenhaCertificado.Text = "Styled Textbox";
            this.txtSenhaCertificado.UseSelectable = true;
            this.txtSenhaCertificado.UseStyleColors = true;
            this.txtSenhaCertificado.TextChanged += new System.EventHandler(this.txtArquivoCertificado_TextChanged);
            // 
            // lblSenhaCertificado
            // 
            this.lblSenhaCertificado.AutoSize = true;
            this.lblSenhaCertificado.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSenhaCertificado.Location = new System.Drawing.Point(3, 228);
            this.lblSenhaCertificado.Margin = new System.Windows.Forms.Padding(3);
            this.lblSenhaCertificado.Name = "lblSenhaCertificado";
            this.lblSenhaCertificado.Size = new System.Drawing.Size(111, 15);
            this.lblSenhaCertificado.TabIndex = 6;
            this.lblSenhaCertificado.Text = "Senha do Certificado";
            // 
            // txtArquivoCertificado
            // 
            this.txtArquivoCertificado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArquivoCertificado.IconRight = true;
            this.txtArquivoCertificado.Lines = new string[] {
        "Styled Textbox"};
            this.txtArquivoCertificado.Location = new System.Drawing.Point(3, 203);
            this.txtArquivoCertificado.MaxLength = 32767;
            this.txtArquivoCertificado.Name = "txtArquivoCertificado";
            this.txtArquivoCertificado.PasswordChar = '\0';
            this.txtArquivoCertificado.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtArquivoCertificado.SelectedText = "";
            this.txtArquivoCertificado.Size = new System.Drawing.Size(605, 22);
            this.txtArquivoCertificado.TabIndex = 5;
            this.txtArquivoCertificado.Text = "Styled Textbox";
            this.txtArquivoCertificado.UseSelectable = true;
            this.txtArquivoCertificado.UseStyleColors = true;
            this.txtArquivoCertificado.TextChanged += new System.EventHandler(this.txtArquivoCertificado_TextChanged);
            // 
            // lblArquivoCertificado
            // 
            this.lblArquivoCertificado.AutoSize = true;
            this.lblArquivoCertificado.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblArquivoCertificado.Location = new System.Drawing.Point(3, 184);
            this.lblArquivoCertificado.Margin = new System.Windows.Forms.Padding(3);
            this.lblArquivoCertificado.Name = "lblArquivoCertificado";
            this.lblArquivoCertificado.Size = new System.Drawing.Size(242, 15);
            this.lblArquivoCertificado.TabIndex = 4;
            this.lblArquivoCertificado.Text = "Local de armazenamento do Certificado Digital";
            // 
            // textBox_dadoscertificado
            // 
            this.textBox_dadoscertificado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dadoscertificado.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_dadoscertificado.Location = new System.Drawing.Point(3, 43);
            this.textBox_dadoscertificado.MaxLength = 32767;
            this.textBox_dadoscertificado.Multiline = true;
            this.textBox_dadoscertificado.Name = "textBox_dadoscertificado";
            this.textBox_dadoscertificado.PasswordChar = '\0';
            this.textBox_dadoscertificado.ReadOnly = true;
            this.textBox_dadoscertificado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_dadoscertificado.SelectedText = "";
            this.textBox_dadoscertificado.Size = new System.Drawing.Size(605, 135);
            this.textBox_dadoscertificado.TabIndex = 3;
            this.textBox_dadoscertificado.Text = "Styled Textbox";
            this.textBox_dadoscertificado.UseSelectable = true;
            this.textBox_dadoscertificado.UseStyleColors = true;
            this.textBox_dadoscertificado.TextChanged += new System.EventHandler(this.txtArquivoCertificado_TextChanged);
            // 
            // lblCerificadoInstalado
            // 
            this.lblCerificadoInstalado.AutoSize = true;
            this.lblCerificadoInstalado.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblCerificadoInstalado.Location = new System.Drawing.Point(3, 24);
            this.lblCerificadoInstalado.Margin = new System.Windows.Forms.Padding(3);
            this.lblCerificadoInstalado.Name = "lblCerificadoInstalado";
            this.lblCerificadoInstalado.Size = new System.Drawing.Size(239, 15);
            this.lblCerificadoInstalado.TabIndex = 2;
            this.lblCerificadoInstalado.Text = "Informações do certificado digital selecionado:";
            // 
            // ckbUsarCertificadoInstalado
            // 
            this.ckbUsarCertificadoInstalado.AutoSize = true;
            this.ckbUsarCertificadoInstalado.Location = new System.Drawing.Point(140, 3);
            this.ckbUsarCertificadoInstalado.Name = "ckbUsarCertificadoInstalado";
            this.ckbUsarCertificadoInstalado.Size = new System.Drawing.Size(246, 15);
            this.ckbUsarCertificadoInstalado.TabIndex = 1;
            this.ckbUsarCertificadoInstalado.Text = "Utilizar certificado instalado no Windows ?";
            this.ckbUsarCertificadoInstalado.UseSelectable = true;
            this.ckbUsarCertificadoInstalado.CheckedChanged += new System.EventHandler(this.ckbCertificadoInstalado_CheckedChanged);
            // 
            // button_selecionar_certificado
            // 
            this.button_selecionar_certificado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selecionar_certificado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_selecionar_certificado.Image = global::NFe.UI.Properties.Resources.identity1;
            this.button_selecionar_certificado.Location = new System.Drawing.Point(614, 43);
            this.button_selecionar_certificado.Name = "button_selecionar_certificado";
            this.button_selecionar_certificado.Size = new System.Drawing.Size(55, 63);
            this.button_selecionar_certificado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.button_selecionar_certificado.TabIndex = 52;
            this.button_selecionar_certificado.TabStop = false;
            this.button_selecionar_certificado.Click += new System.EventHandler(this.button_selecionar_certificado_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // ckbUsaCertificado
            // 
            this.ckbUsaCertificado.AutoSize = true;
            this.ckbUsaCertificado.Location = new System.Drawing.Point(3, 3);
            this.ckbUsaCertificado.Name = "ckbUsaCertificado";
            this.ckbUsaCertificado.Size = new System.Drawing.Size(118, 15);
            this.ckbUsaCertificado.TabIndex = 0;
            this.ckbUsaCertificado.Text = "Utilizar certificado";
            this.ckbUsaCertificado.UseSelectable = true;
            this.ckbUsaCertificado.CheckedChanged += new System.EventHandler(this.ckbTemCertificadoInstalado_CheckedChanged);
            // 
            // btnValidarProvider
            // 
            this.btnValidarProvider.BackgroundImage = global::NFe.UI.Properties.Resources.e10b_Accept_48;
            this.btnValidarProvider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnValidarProvider.Enabled = false;
            this.btnValidarProvider.Location = new System.Drawing.Point(644, 233);
            this.btnValidarProvider.Name = "btnValidarProvider";
            this.btnValidarProvider.Size = new System.Drawing.Size(25, 25);
            this.btnValidarProvider.TabIndex = 62;
            this.btnValidarProvider.UseSelectable = true;
            this.btnValidarProvider.Visible = false;
            this.btnValidarProvider.Click += new System.EventHandler(this.btnValidarProvider_Click);
            // 
            // userConfiguracao_certificado
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.btnValidarProvider);
            this.Controls.Add(this.ckbUsaCertificado);
            this.Controls.Add(this.txtPinCertificado);
            this.Controls.Add(this.lblPinCertificado);
            this.Controls.Add(this.txtSenhaCertificado);
            this.Controls.Add(this.lblSenhaCertificado);
            this.Controls.Add(this.txtArquivoCertificado);
            this.Controls.Add(this.lblArquivoCertificado);
            this.Controls.Add(this.button_selecionar_certificado);
            this.Controls.Add(this.textBox_dadoscertificado);
            this.Controls.Add(this.lblCerificadoInstalado);
            this.Controls.Add(this.ckbUsarCertificadoInstalado);
            this.Name = "userConfiguracao_certificado";
            this.Size = new System.Drawing.Size(715, 359);
            ((System.ComponentModel.ISupportInitialize)(this.button_selecionar_certificado)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox txtPinCertificado;
        private MetroFramework.Controls.MetroLabel lblPinCertificado;
        private MetroFramework.Controls.MetroTextBox txtSenhaCertificado;
        private MetroFramework.Controls.MetroLabel lblSenhaCertificado;
        private MetroFramework.Controls.MetroTextBox txtArquivoCertificado;
        private MetroFramework.Controls.MetroLabel lblArquivoCertificado;
        private System.Windows.Forms.PictureBox button_selecionar_certificado;
        private MetroFramework.Controls.MetroTextBox textBox_dadoscertificado;
        private MetroFramework.Controls.MetroLabel lblCerificadoInstalado;
        private MetroFramework.Controls.MetroCheckBox ckbUsarCertificadoInstalado;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private MetroFramework.Controls.MetroCheckBox ckbUsaCertificado;
        private MetroFramework.Controls.MetroButton btnValidarProvider;
    }
}

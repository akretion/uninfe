namespace NFe.UI.Formularios
{
    partial class userConfiguracao_geral
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
            this.lblSenha = new MetroFramework.Controls.MetroLabel();
            this.metroLabel16 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.tbSenhaConfig2 = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.tbSenhaConfig = new MetroFramework.Controls.MetroTextBox();
            this.chkGravarLogOperacao = new MetroFramework.Controls.MetroCheckBox();
            this.cbChecaConexaoInternet = new MetroFramework.Controls.MetroCheckBox();
            this.lblServidor = new MetroFramework.Controls.MetroLabel();
            this.tbServidor = new MetroFramework.Controls.MetroTextBox();
            this.lblPorta = new MetroFramework.Controls.MetroLabel();
            this.nudPorta = new MetroFramework.Controls.MetroTextBox();
            this.tbSenha = new MetroFramework.Controls.MetroTextBox();
            this.lblUsuario = new MetroFramework.Controls.MetroLabel();
            this.tbUsuario = new MetroFramework.Controls.MetroTextBox();
            this.cbProxy = new MetroFramework.Controls.MetroCheckBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.chkConfProxyAuto = new MetroFramework.Controls.MetroCheckBox();
            this.chkConfirmaSaida = new MetroFramework.Controls.MetroCheckBox();
            this.metroPanel2.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSenha.Location = new System.Drawing.Point(281, 34);
            this.lblSenha.Margin = new System.Windows.Forms.Padding(3);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(37, 15);
            this.lblSenha.TabIndex = 3;
            this.lblSenha.Text = "Senha";
            // 
            // metroLabel16
            // 
            this.metroLabel16.AutoSize = true;
            this.metroLabel16.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel16.Location = new System.Drawing.Point(6, 199);
            this.metroLabel16.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel16.Name = "metroLabel16";
            this.metroLabel16.Size = new System.Drawing.Size(208, 15);
            this.metroLabel16.TabIndex = 3;
            this.metroLabel16.Text = "Senha de acesso a tela de configurações";
            // 
            // metroPanel2
            // 
            this.metroPanel2.BackColor = System.Drawing.Color.Transparent;
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.metroLabel9);
            this.metroPanel2.Controls.Add(this.tbSenhaConfig2);
            this.metroPanel2.Controls.Add(this.metroLabel7);
            this.metroPanel2.Controls.Add(this.tbSenhaConfig);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(6, 217);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(429, 130);
            this.metroPanel2.TabIndex = 4;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // metroLabel9
            // 
            this.metroLabel9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel9.AutoSize = true;
            this.metroLabel9.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel9.Location = new System.Drawing.Point(11, 61);
            this.metroLabel9.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(87, 15);
            this.metroLabel9.TabIndex = 2;
            this.metroLabel9.Text = "Repetir a senha:";
            // 
            // tbSenhaConfig2
            // 
            this.tbSenhaConfig2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSenhaConfig2.Lines = new string[] {
        "Styled Textbox"};
            this.tbSenhaConfig2.Location = new System.Drawing.Point(11, 81);
            this.tbSenhaConfig2.MaxLength = 32767;
            this.tbSenhaConfig2.Name = "tbSenhaConfig2";
            this.tbSenhaConfig2.PasswordChar = '*';
            this.tbSenhaConfig2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSenhaConfig2.SelectedText = "";
            this.tbSenhaConfig2.Size = new System.Drawing.Size(403, 22);
            this.tbSenhaConfig2.TabIndex = 3;
            this.tbSenhaConfig2.Text = "Styled Textbox";
            this.tbSenhaConfig2.UseSelectable = true;
            this.tbSenhaConfig2.UseStyleColors = true;
            this.tbSenhaConfig2.TextChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.tbSenhaConfig2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // metroLabel7
            // 
            this.metroLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel7.Location = new System.Drawing.Point(11, 7);
            this.metroLabel7.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(37, 15);
            this.metroLabel7.TabIndex = 0;
            this.metroLabel7.Text = "Senha";
            // 
            // tbSenhaConfig
            // 
            this.tbSenhaConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSenhaConfig.Lines = new string[] {
        "Styled Textbox"};
            this.tbSenhaConfig.Location = new System.Drawing.Point(11, 27);
            this.tbSenhaConfig.MaxLength = 32767;
            this.tbSenhaConfig.Name = "tbSenhaConfig";
            this.tbSenhaConfig.PasswordChar = '*';
            this.tbSenhaConfig.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSenhaConfig.SelectedText = "";
            this.tbSenhaConfig.Size = new System.Drawing.Size(403, 22);
            this.tbSenhaConfig.TabIndex = 1;
            this.tbSenhaConfig.Text = "Styled Textbox";
            this.tbSenhaConfig.UseSelectable = true;
            this.tbSenhaConfig.UseStyleColors = true;
            this.tbSenhaConfig.TextChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.tbSenhaConfig.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // chkGravarLogOperacao
            // 
            this.chkGravarLogOperacao.AutoSize = true;
            this.chkGravarLogOperacao.Location = new System.Drawing.Point(6, 162);
            this.chkGravarLogOperacao.Name = "chkGravarLogOperacao";
            this.chkGravarLogOperacao.Size = new System.Drawing.Size(209, 15);
            this.chkGravarLogOperacao.TabIndex = 2;
            this.chkGravarLogOperacao.Text = "Gravar log das operações realizadas";
            this.chkGravarLogOperacao.UseSelectable = true;
            this.chkGravarLogOperacao.CheckedChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.chkGravarLogOperacao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // cbChecaConexaoInternet
            // 
            this.cbChecaConexaoInternet.AutoSize = true;
            this.cbChecaConexaoInternet.Location = new System.Drawing.Point(6, 144);
            this.cbChecaConexaoInternet.Name = "cbChecaConexaoInternet";
            this.cbChecaConexaoInternet.Size = new System.Drawing.Size(284, 15);
            this.cbChecaConexaoInternet.TabIndex = 1;
            this.cbChecaConexaoInternet.Text = "Checar a conexão com a internet ao enviar o XML";
            this.cbChecaConexaoInternet.UseSelectable = true;
            this.cbChecaConexaoInternet.CheckedChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.cbChecaConexaoInternet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // lblServidor
            // 
            this.lblServidor.AutoSize = true;
            this.lblServidor.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblServidor.Location = new System.Drawing.Point(281, 85);
            this.lblServidor.Margin = new System.Windows.Forms.Padding(3);
            this.lblServidor.Name = "lblServidor";
            this.lblServidor.Size = new System.Drawing.Size(49, 15);
            this.lblServidor.TabIndex = 7;
            this.lblServidor.Text = "Servidor";
            // 
            // tbServidor
            // 
            this.tbServidor.Lines = new string[] {
        "Styled Textbox"};
            this.tbServidor.Location = new System.Drawing.Point(281, 104);
            this.tbServidor.MaxLength = 32767;
            this.tbServidor.Name = "tbServidor";
            this.tbServidor.PasswordChar = '\0';
            this.tbServidor.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbServidor.SelectedText = "";
            this.tbServidor.Size = new System.Drawing.Size(267, 22);
            this.tbServidor.TabIndex = 8;
            this.tbServidor.Text = "Styled Textbox";
            this.tbServidor.UseSelectable = true;
            this.tbServidor.UseStyleColors = true;
            this.tbServidor.TextChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.tbServidor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblPorta.Location = new System.Drawing.Point(7, 85);
            this.lblPorta.Margin = new System.Windows.Forms.Padding(3);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(35, 15);
            this.lblPorta.TabIndex = 5;
            this.lblPorta.Text = "Porta";
            // 
            // nudPorta
            // 
            this.nudPorta.Lines = new string[] {
        "Styled Textbox"};
            this.nudPorta.Location = new System.Drawing.Point(7, 104);
            this.nudPorta.MaxLength = 32767;
            this.nudPorta.Name = "nudPorta";
            this.nudPorta.PasswordChar = '\0';
            this.nudPorta.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.nudPorta.SelectedText = "";
            this.nudPorta.Size = new System.Drawing.Size(68, 22);
            this.nudPorta.TabIndex = 6;
            this.nudPorta.Text = "Styled Textbox";
            this.nudPorta.UseSelectable = true;
            this.nudPorta.UseStyleColors = true;
            this.nudPorta.TextChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.nudPorta.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            this.nudPorta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nudPorta_KeyPress);
            // 
            // tbSenha
            // 
            this.tbSenha.Lines = new string[] {
        "Styled Textbox"};
            this.tbSenha.Location = new System.Drawing.Point(281, 53);
            this.tbSenha.MaxLength = 32767;
            this.tbSenha.Name = "tbSenha";
            this.tbSenha.PasswordChar = '*';
            this.tbSenha.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSenha.SelectedText = "";
            this.tbSenha.Size = new System.Drawing.Size(267, 22);
            this.tbSenha.TabIndex = 4;
            this.tbSenha.Text = "Styled Textbox";
            this.tbSenha.UseSelectable = true;
            this.tbSenha.UseStyleColors = true;
            this.tbSenha.TextChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.tbSenha.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblUsuario.Location = new System.Drawing.Point(7, 34);
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(3);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(45, 15);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Text = "Usuário";
            // 
            // tbUsuario
            // 
            this.tbUsuario.Lines = new string[] {
        "Styled Textbox"};
            this.tbUsuario.Location = new System.Drawing.Point(7, 53);
            this.tbUsuario.MaxLength = 32767;
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.PasswordChar = '\0';
            this.tbUsuario.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbUsuario.SelectedText = "";
            this.tbUsuario.Size = new System.Drawing.Size(267, 22);
            this.tbUsuario.TabIndex = 2;
            this.tbUsuario.Text = "Styled Textbox";
            this.tbUsuario.UseSelectable = true;
            this.tbUsuario.UseStyleColors = true;
            this.tbUsuario.TextChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.tbUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // cbProxy
            // 
            this.cbProxy.AutoSize = true;
            this.cbProxy.Location = new System.Drawing.Point(3, 3);
            this.cbProxy.Name = "cbProxy";
            this.cbProxy.Size = new System.Drawing.Size(144, 15);
            this.cbProxy.TabIndex = 0;
            this.cbProxy.Text = "Usar um servidor proxy";
            this.cbProxy.UseSelectable = true;
            this.cbProxy.CheckedChanged += new System.EventHandler(this.cbProxy_CheckedChanged);
            this.cbProxy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(443, 319);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(111, 28);
            this.metroButton1.TabIndex = 5;
            this.metroButton1.Text = "Salvar";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackColor = System.Drawing.Color.Transparent;
            this.metroPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel1.Controls.Add(this.chkConfProxyAuto);
            this.metroPanel1.Controls.Add(this.lblSenha);
            this.metroPanel1.Controls.Add(this.tbUsuario);
            this.metroPanel1.Controls.Add(this.lblUsuario);
            this.metroPanel1.Controls.Add(this.cbProxy);
            this.metroPanel1.Controls.Add(this.tbSenha);
            this.metroPanel1.Controls.Add(this.nudPorta);
            this.metroPanel1.Controls.Add(this.lblServidor);
            this.metroPanel1.Controls.Add(this.lblPorta);
            this.metroPanel1.Controls.Add(this.tbServidor);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(6, 3);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(562, 133);
            this.metroPanel1.TabIndex = 0;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // chkConfProxyAuto
            // 
            this.chkConfProxyAuto.AutoSize = true;
            this.chkConfProxyAuto.Enabled = false;
            this.chkConfProxyAuto.Location = new System.Drawing.Point(192, 4);
            this.chkConfProxyAuto.Name = "chkConfProxyAuto";
            this.chkConfProxyAuto.Size = new System.Drawing.Size(285, 15);
            this.chkConfProxyAuto.TabIndex = 9;
            this.chkConfProxyAuto.Text = "Detectar configuração de proxy automaticamente";
            this.chkConfProxyAuto.UseSelectable = true;
            this.chkConfProxyAuto.CheckedChanged += new System.EventHandler(this.chkConfProxyAuto_CheckedChanged);
            // 
            // chkConfirmaSaida
            // 
            this.chkConfirmaSaida.AutoSize = true;
            this.chkConfirmaSaida.Location = new System.Drawing.Point(6, 181);
            this.chkConfirmaSaida.Name = "chkConfirmaSaida";
            this.chkConfirmaSaida.Size = new System.Drawing.Size(340, 15);
            this.chkConfirmaSaida.TabIndex = 6;
            this.chkConfirmaSaida.Text = "Exibir tela de confirmação ao fechar manualmente o UniNFe";
            this.chkConfirmaSaida.UseSelectable = true;
            this.chkConfirmaSaida.CheckedChanged += new System.EventHandler(this.tbUsuario_TextChanged);
            this.chkConfirmaSaida.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsuario_KeyDown);
            // 
            // userConfiguracao_geral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkConfirmaSaida);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.metroLabel16);
            this.Controls.Add(this.metroPanel2);
            this.Controls.Add(this.chkGravarLogOperacao);
            this.Controls.Add(this.cbChecaConexaoInternet);
            this.Controls.Add(this.metroButton1);
            this.Name = "userConfiguracao_geral";
            this.Size = new System.Drawing.Size(576, 364);
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel lblSenha;
        private MetroFramework.Controls.MetroLabel metroLabel16;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroLabel metroLabel9;
        private MetroFramework.Controls.MetroTextBox tbSenhaConfig2;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroTextBox tbSenhaConfig;
        private MetroFramework.Controls.MetroCheckBox chkGravarLogOperacao;
        private MetroFramework.Controls.MetroCheckBox cbChecaConexaoInternet;
        private MetroFramework.Controls.MetroLabel lblServidor;
        private MetroFramework.Controls.MetroTextBox tbServidor;
        private MetroFramework.Controls.MetroLabel lblPorta;
        private MetroFramework.Controls.MetroTextBox nudPorta;
        private MetroFramework.Controls.MetroTextBox tbSenha;
        private MetroFramework.Controls.MetroLabel lblUsuario;
        private MetroFramework.Controls.MetroTextBox tbUsuario;
        private MetroFramework.Controls.MetroCheckBox cbProxy;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroCheckBox chkConfProxyAuto;
        private MetroFramework.Controls.MetroCheckBox chkConfirmaSaida;
    }
}

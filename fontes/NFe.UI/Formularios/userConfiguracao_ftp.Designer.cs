namespace NFe.UI.Formularios
{
    partial class userConfiguracao_ftp
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
            this.edtFTP_GravaXMLPastaUnica = new MetroFramework.Controls.MetroCheckBox();
            this.edtFTP_Ativo = new MetroFramework.Controls.MetroCheckBox();
            this.lblFTP_PastaRetornos = new MetroFramework.Controls.MetroLabel();
            this.edtFTP_PastaRetornos = new MetroFramework.Controls.MetroTextBox();
            this.lbl_edtFTP_PastaDestino = new MetroFramework.Controls.MetroLabel();
            this.edtFTP_PastaDestino = new MetroFramework.Controls.MetroTextBox();
            this.edtFTP_Porta = new MetroFramework.Controls.MetroTextBox();
            this.edtFTP_Password = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel39 = new MetroFramework.Controls.MetroLabel();
            this.edtFTP_UserName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel38 = new MetroFramework.Controls.MetroLabel();
            this.edtFTP_Server = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel41 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel40 = new MetroFramework.Controls.MetroLabel();
            this.btnTestarFTP = new MetroFramework.Controls.MetroButton();
            this.edtFTP_Passivo = new MetroFramework.Controls.MetroCheckBox();
            this.SuspendLayout();
            // 
            // edtFTP_GravaXMLPastaUnica
            // 
            this.edtFTP_GravaXMLPastaUnica.AutoSize = true;
            this.edtFTP_GravaXMLPastaUnica.Location = new System.Drawing.Point(0, 257);
            this.edtFTP_GravaXMLPastaUnica.Name = "edtFTP_GravaXMLPastaUnica";
            this.edtFTP_GravaXMLPastaUnica.Size = new System.Drawing.Size(547, 15);
            this.edtFTP_GravaXMLPastaUnica.TabIndex = 52;
            this.edtFTP_GravaXMLPastaUnica.Text = "Gravar os XML\'s autorizados em uma única pasta, se não, serão criadas pastas conf" +
    "orme a definição";
            this.edtFTP_GravaXMLPastaUnica.UseSelectable = true;
            this.edtFTP_GravaXMLPastaUnica.CheckedChanged += new System.EventHandler(this.edtFTP_Ativo_CheckedChanged);
            // 
            // edtFTP_Ativo
            // 
            this.edtFTP_Ativo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFTP_Ativo.AutoSize = true;
            this.edtFTP_Ativo.Location = new System.Drawing.Point(568, 19);
            this.edtFTP_Ativo.Name = "edtFTP_Ativo";
            this.edtFTP_Ativo.Size = new System.Drawing.Size(56, 15);
            this.edtFTP_Ativo.TabIndex = 51;
            this.edtFTP_Ativo.Text = "Ativo?";
            this.edtFTP_Ativo.UseSelectable = true;
            this.edtFTP_Ativo.CheckedChanged += new System.EventHandler(this.edtFTP_Ativo_CheckedChanged);
            // 
            // lblFTP_PastaRetornos
            // 
            this.lblFTP_PastaRetornos.AutoSize = true;
            this.lblFTP_PastaRetornos.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblFTP_PastaRetornos.Location = new System.Drawing.Point(0, 212);
            this.lblFTP_PastaRetornos.Margin = new System.Windows.Forms.Padding(3);
            this.lblFTP_PastaRetornos.Name = "lblFTP_PastaRetornos";
            this.lblFTP_PastaRetornos.Size = new System.Drawing.Size(383, 15);
            this.lblFTP_PastaRetornos.TabIndex = 49;
            this.lblFTP_PastaRetornos.Text = "Pasta onde serão gravados os arquivos XML´s de retorno dos WebServices";
            // 
            // edtFTP_PastaRetornos
            // 
            this.edtFTP_PastaRetornos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFTP_PastaRetornos.Lines = new string[] {
        "Styled Textbox"};
            this.edtFTP_PastaRetornos.Location = new System.Drawing.Point(0, 229);
            this.edtFTP_PastaRetornos.MaxLength = 32767;
            this.edtFTP_PastaRetornos.Name = "edtFTP_PastaRetornos";
            this.edtFTP_PastaRetornos.PasswordChar = '\0';
            this.edtFTP_PastaRetornos.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtFTP_PastaRetornos.SelectedText = "";
            this.edtFTP_PastaRetornos.Size = new System.Drawing.Size(562, 22);
            this.edtFTP_PastaRetornos.TabIndex = 50;
            this.edtFTP_PastaRetornos.Text = "Styled Textbox";
            this.edtFTP_PastaRetornos.UseSelectable = true;
            this.edtFTP_PastaRetornos.UseStyleColors = true;
            this.edtFTP_PastaRetornos.TextChanged += new System.EventHandler(this.edtFTP_Server_TextChanged);
            // 
            // lbl_edtFTP_PastaDestino
            // 
            this.lbl_edtFTP_PastaDestino.AutoSize = true;
            this.lbl_edtFTP_PastaDestino.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_edtFTP_PastaDestino.Location = new System.Drawing.Point(0, 169);
            this.lbl_edtFTP_PastaDestino.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_edtFTP_PastaDestino.Name = "lbl_edtFTP_PastaDestino";
            this.lbl_edtFTP_PastaDestino.Size = new System.Drawing.Size(284, 15);
            this.lbl_edtFTP_PastaDestino.TabIndex = 47;
            this.lbl_edtFTP_PastaDestino.Text = "Pasta onde serão gravados os arquivos XML´s enviados";
            // 
            // edtFTP_PastaDestino
            // 
            this.edtFTP_PastaDestino.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFTP_PastaDestino.Lines = new string[] {
        "Styled Textbox"};
            this.edtFTP_PastaDestino.Location = new System.Drawing.Point(0, 187);
            this.edtFTP_PastaDestino.MaxLength = 32767;
            this.edtFTP_PastaDestino.Name = "edtFTP_PastaDestino";
            this.edtFTP_PastaDestino.PasswordChar = '\0';
            this.edtFTP_PastaDestino.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtFTP_PastaDestino.SelectedText = "";
            this.edtFTP_PastaDestino.Size = new System.Drawing.Size(562, 22);
            this.edtFTP_PastaDestino.TabIndex = 48;
            this.edtFTP_PastaDestino.Text = "Styled Textbox";
            this.edtFTP_PastaDestino.UseSelectable = true;
            this.edtFTP_PastaDestino.UseStyleColors = true;
            this.edtFTP_PastaDestino.TextChanged += new System.EventHandler(this.edtFTP_Server_TextChanged);
            // 
            // edtFTP_Porta
            // 
            this.edtFTP_Porta.Lines = new string[] {
        "Styled Textbox"};
            this.edtFTP_Porta.Location = new System.Drawing.Point(0, 145);
            this.edtFTP_Porta.MaxLength = 32767;
            this.edtFTP_Porta.Name = "edtFTP_Porta";
            this.edtFTP_Porta.PasswordChar = '\0';
            this.edtFTP_Porta.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtFTP_Porta.SelectedText = "";
            this.edtFTP_Porta.Size = new System.Drawing.Size(80, 22);
            this.edtFTP_Porta.TabIndex = 46;
            this.edtFTP_Porta.Text = "Styled Textbox";
            this.edtFTP_Porta.UseSelectable = true;
            this.edtFTP_Porta.UseStyleColors = true;
            this.edtFTP_Porta.TextChanged += new System.EventHandler(this.edtFTP_Server_TextChanged);
            // 
            // edtFTP_Password
            // 
            this.edtFTP_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFTP_Password.Lines = new string[] {
        "Styled Textbox"};
            this.edtFTP_Password.Location = new System.Drawing.Point(0, 103);
            this.edtFTP_Password.MaxLength = 32767;
            this.edtFTP_Password.Name = "edtFTP_Password";
            this.edtFTP_Password.PasswordChar = '*';
            this.edtFTP_Password.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtFTP_Password.SelectedText = "";
            this.edtFTP_Password.Size = new System.Drawing.Size(562, 22);
            this.edtFTP_Password.TabIndex = 45;
            this.edtFTP_Password.Text = "Styled Textbox";
            this.edtFTP_Password.UseSelectable = true;
            this.edtFTP_Password.UseStyleColors = true;
            this.edtFTP_Password.TextChanged += new System.EventHandler(this.edtFTP_Server_TextChanged);
            // 
            // metroLabel39
            // 
            this.metroLabel39.AutoSize = true;
            this.metroLabel39.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel39.Location = new System.Drawing.Point(0, 43);
            this.metroLabel39.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel39.Name = "metroLabel39";
            this.metroLabel39.Size = new System.Drawing.Size(95, 15);
            this.metroLabel39.TabIndex = 43;
            this.metroLabel39.Text = "Nome do usuário";
            // 
            // edtFTP_UserName
            // 
            this.edtFTP_UserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFTP_UserName.Lines = new string[] {
        "Styled Textbox"};
            this.edtFTP_UserName.Location = new System.Drawing.Point(0, 61);
            this.edtFTP_UserName.MaxLength = 32767;
            this.edtFTP_UserName.Name = "edtFTP_UserName";
            this.edtFTP_UserName.PasswordChar = '\0';
            this.edtFTP_UserName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtFTP_UserName.SelectedText = "";
            this.edtFTP_UserName.Size = new System.Drawing.Size(562, 22);
            this.edtFTP_UserName.TabIndex = 44;
            this.edtFTP_UserName.Text = "Styled Textbox";
            this.edtFTP_UserName.UseSelectable = true;
            this.edtFTP_UserName.UseStyleColors = true;
            this.edtFTP_UserName.TextChanged += new System.EventHandler(this.edtFTP_Server_TextChanged);
            // 
            // metroLabel38
            // 
            this.metroLabel38.AutoSize = true;
            this.metroLabel38.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel38.Location = new System.Drawing.Point(0, 1);
            this.metroLabel38.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel38.Name = "metroLabel38";
            this.metroLabel38.Size = new System.Drawing.Size(99, 15);
            this.metroLabel38.TabIndex = 41;
            this.metroLabel38.Text = "Nome da servidor";
            // 
            // edtFTP_Server
            // 
            this.edtFTP_Server.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFTP_Server.Lines = new string[] {
        "Styled Textbox"};
            this.edtFTP_Server.Location = new System.Drawing.Point(0, 19);
            this.edtFTP_Server.MaxLength = 32767;
            this.edtFTP_Server.Name = "edtFTP_Server";
            this.edtFTP_Server.PasswordChar = '\0';
            this.edtFTP_Server.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtFTP_Server.SelectedText = "";
            this.edtFTP_Server.Size = new System.Drawing.Size(562, 22);
            this.edtFTP_Server.TabIndex = 42;
            this.edtFTP_Server.Text = "Styled Textbox";
            this.edtFTP_Server.UseSelectable = true;
            this.edtFTP_Server.UseStyleColors = true;
            this.edtFTP_Server.TextChanged += new System.EventHandler(this.edtFTP_Server_TextChanged);
            // 
            // metroLabel41
            // 
            this.metroLabel41.AutoSize = true;
            this.metroLabel41.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel41.Location = new System.Drawing.Point(0, 127);
            this.metroLabel41.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel41.Name = "metroLabel41";
            this.metroLabel41.Size = new System.Drawing.Size(35, 15);
            this.metroLabel41.TabIndex = 54;
            this.metroLabel41.Text = "Porta";
            // 
            // metroLabel40
            // 
            this.metroLabel40.AutoSize = true;
            this.metroLabel40.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel40.Location = new System.Drawing.Point(0, 85);
            this.metroLabel40.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel40.Name = "metroLabel40";
            this.metroLabel40.Size = new System.Drawing.Size(37, 15);
            this.metroLabel40.TabIndex = 53;
            this.metroLabel40.Text = "Senha";
            // 
            // btnTestarFTP
            // 
            this.btnTestarFTP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestarFTP.Location = new System.Drawing.Point(491, 145);
            this.btnTestarFTP.Name = "btnTestarFTP";
            this.btnTestarFTP.Size = new System.Drawing.Size(71, 22);
            this.btnTestarFTP.TabIndex = 75;
            this.btnTestarFTP.Text = "Testar";
            this.btnTestarFTP.UseSelectable = true;
            this.btnTestarFTP.Click += new System.EventHandler(this.btnTestarFTP_Click);
            // 
            // edtFTP_Passivo
            // 
            this.edtFTP_Passivo.AutoSize = true;
            this.edtFTP_Passivo.Location = new System.Drawing.Point(86, 148);
            this.edtFTP_Passivo.Name = "edtFTP_Passivo";
            this.edtFTP_Passivo.Size = new System.Drawing.Size(102, 15);
            this.edtFTP_Passivo.TabIndex = 76;
            this.edtFTP_Passivo.Text = "Modo passivo?";
            this.edtFTP_Passivo.UseSelectable = true;
            this.edtFTP_Passivo.CheckedChanged += new System.EventHandler(this.edtFTP_Passivo_CheckedChanged);
            // 
            // userConfiguracao_ftp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.edtFTP_Passivo);
            this.Controls.Add(this.btnTestarFTP);
            this.Controls.Add(this.metroLabel41);
            this.Controls.Add(this.metroLabel40);
            this.Controls.Add(this.edtFTP_GravaXMLPastaUnica);
            this.Controls.Add(this.edtFTP_Ativo);
            this.Controls.Add(this.lblFTP_PastaRetornos);
            this.Controls.Add(this.edtFTP_PastaRetornos);
            this.Controls.Add(this.lbl_edtFTP_PastaDestino);
            this.Controls.Add(this.edtFTP_PastaDestino);
            this.Controls.Add(this.edtFTP_Porta);
            this.Controls.Add(this.edtFTP_Password);
            this.Controls.Add(this.metroLabel39);
            this.Controls.Add(this.edtFTP_UserName);
            this.Controls.Add(this.metroLabel38);
            this.Controls.Add(this.edtFTP_Server);
            this.Name = "userConfiguracao_ftp";
            this.Size = new System.Drawing.Size(638, 284);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroCheckBox edtFTP_GravaXMLPastaUnica;
        private MetroFramework.Controls.MetroCheckBox edtFTP_Ativo;
        private MetroFramework.Controls.MetroLabel lblFTP_PastaRetornos;
        private MetroFramework.Controls.MetroTextBox edtFTP_PastaRetornos;
        private MetroFramework.Controls.MetroLabel lbl_edtFTP_PastaDestino;
        private MetroFramework.Controls.MetroTextBox edtFTP_PastaDestino;
        private MetroFramework.Controls.MetroTextBox edtFTP_Porta;
        private MetroFramework.Controls.MetroTextBox edtFTP_Password;
        private MetroFramework.Controls.MetroLabel metroLabel39;
        private MetroFramework.Controls.MetroTextBox edtFTP_UserName;
        private MetroFramework.Controls.MetroLabel metroLabel38;
        private MetroFramework.Controls.MetroTextBox edtFTP_Server;
        private MetroFramework.Controls.MetroLabel metroLabel41;
        private MetroFramework.Controls.MetroLabel metroLabel40;
        private MetroFramework.Controls.MetroButton btnTestarFTP;
        private MetroFramework.Controls.MetroCheckBox edtFTP_Passivo;
    }
}

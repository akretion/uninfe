namespace NFe.UI.Formularios
{
    partial class userConfiguracao_sat
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
            this.metroLabel39 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel38 = new MetroFramework.Controls.MetroLabel();
            this.cbMacarSAT = new MetroFramework.Controls.MetroComboBox();
            this.txtCodigoAtivacaoSAT = new MetroFramework.Controls.MetroTextBox();
            this.ckConversaoNFCe = new MetroFramework.Controls.MetroCheckBox();
            this.txtSignAC = new MetroFramework.Controls.MetroTextBox();
            this.txtCNPJSw = new MetroFramework.Controls.MetroTextBox();
            this.lblSignAC = new MetroFramework.Controls.MetroLabel();
            this.lblCNPJSw = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLabel39
            // 
            this.metroLabel39.AutoSize = true;
            this.metroLabel39.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel39.Location = new System.Drawing.Point(0, 53);
            this.metroLabel39.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel39.Name = "metroLabel39";
            this.metroLabel39.Size = new System.Drawing.Size(106, 15);
            this.metroLabel39.TabIndex = 43;
            this.metroLabel39.Text = "Código de Ativação";
            // 
            // metroLabel38
            // 
            this.metroLabel38.AutoSize = true;
            this.metroLabel38.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel38.Location = new System.Drawing.Point(0, 1);
            this.metroLabel38.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel38.Name = "metroLabel38";
            this.metroLabel38.Size = new System.Drawing.Size(38, 15);
            this.metroLabel38.TabIndex = 41;
            this.metroLabel38.Text = "Marca";
            // 
            // cbMacarSAT
            // 
            this.cbMacarSAT.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbMacarSAT.FormattingEnabled = true;
            this.cbMacarSAT.ItemHeight = 19;
            this.cbMacarSAT.Items.AddRange(new object[] {
            "TANCA",
            "BEMATECH",
            "DARUMA"});
            this.cbMacarSAT.Location = new System.Drawing.Point(0, 19);
            this.cbMacarSAT.Name = "cbMacarSAT";
            this.cbMacarSAT.Size = new System.Drawing.Size(310, 25);
            this.cbMacarSAT.TabIndex = 52;
            this.cbMacarSAT.UseSelectable = true;
            this.cbMacarSAT.SelectedIndexChanged += new System.EventHandler(this.cbMacarSAT_SelectedIndexChanged);
            // 
            // txtCodigoAtivacaoSAT
            // 
            this.txtCodigoAtivacaoSAT.Lines = new string[0];
            this.txtCodigoAtivacaoSAT.Location = new System.Drawing.Point(0, 74);
            this.txtCodigoAtivacaoSAT.MaxLength = 32767;
            this.txtCodigoAtivacaoSAT.Name = "txtCodigoAtivacaoSAT";
            this.txtCodigoAtivacaoSAT.PasswordChar = '\0';
            this.txtCodigoAtivacaoSAT.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCodigoAtivacaoSAT.SelectedText = "";
            this.txtCodigoAtivacaoSAT.Size = new System.Drawing.Size(310, 23);
            this.txtCodigoAtivacaoSAT.TabIndex = 53;
            this.txtCodigoAtivacaoSAT.UseSelectable = true;
            this.txtCodigoAtivacaoSAT.TextChanged += new System.EventHandler(this.txtCodigoAtivacaoSAT_TextChanged);
            // 
            // ckConversaoNFCe
            // 
            this.ckConversaoNFCe.AutoSize = true;
            this.ckConversaoNFCe.Location = new System.Drawing.Point(0, 108);
            this.ckConversaoNFCe.Name = "ckConversaoNFCe";
            this.ckConversaoNFCe.Size = new System.Drawing.Size(209, 15);
            this.ckConversaoNFCe.TabIndex = 54;
            this.ckConversaoNFCe.Text = "Utiliza conversão de NFCe para CFe";
            this.ckConversaoNFCe.UseSelectable = true;
            this.ckConversaoNFCe.CheckedChanged += new System.EventHandler(this.ckConversaoNFCe_CheckedChanged);
            // 
            // txtSignAC
            // 
            this.txtSignAC.Lines = new string[0];
            this.txtSignAC.Location = new System.Drawing.Point(0, 157);
            this.txtSignAC.MaxLength = 32767;
            this.txtSignAC.Name = "txtSignAC";
            this.txtSignAC.PasswordChar = '\0';
            this.txtSignAC.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSignAC.SelectedText = "";
            this.txtSignAC.Size = new System.Drawing.Size(310, 23);
            this.txtSignAC.TabIndex = 55;
            this.txtSignAC.UseSelectable = true;
            // 
            // txtCNPJSw
            // 
            this.txtCNPJSw.Lines = new string[0];
            this.txtCNPJSw.Location = new System.Drawing.Point(0, 205);
            this.txtCNPJSw.MaxLength = 32767;
            this.txtCNPJSw.Name = "txtCNPJSw";
            this.txtCNPJSw.PasswordChar = '\0';
            this.txtCNPJSw.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCNPJSw.SelectedText = "";
            this.txtCNPJSw.Size = new System.Drawing.Size(310, 23);
            this.txtCNPJSw.TabIndex = 56;
            this.txtCNPJSw.UseSelectable = true;
            // 
            // lblSignAC
            // 
            this.lblSignAC.AutoSize = true;
            this.lblSignAC.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSignAC.Location = new System.Drawing.Point(0, 135);
            this.lblSignAC.Name = "lblSignAC";
            this.lblSignAC.Size = new System.Drawing.Size(43, 15);
            this.lblSignAC.TabIndex = 57;
            this.lblSignAC.Text = "SignAC";
            // 
            // lblCNPJSw
            // 
            this.lblCNPJSw.AutoSize = true;
            this.lblCNPJSw.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblCNPJSw.Location = new System.Drawing.Point(0, 187);
            this.lblCNPJSw.Name = "lblCNPJSw";
            this.lblCNPJSw.Size = new System.Drawing.Size(131, 15);
            this.lblCNPJSw.TabIndex = 58;
            this.lblCNPJSw.Text = "CNPJ da Software House";
            // 
            // userConfiguracao_sat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lblCNPJSw);
            this.Controls.Add(this.lblSignAC);
            this.Controls.Add(this.txtCNPJSw);
            this.Controls.Add(this.txtSignAC);
            this.Controls.Add(this.ckConversaoNFCe);
            this.Controls.Add(this.txtCodigoAtivacaoSAT);
            this.Controls.Add(this.cbMacarSAT);
            this.Controls.Add(this.metroLabel39);
            this.Controls.Add(this.metroLabel38);
            this.Name = "userConfiguracao_sat";
            this.Size = new System.Drawing.Size(638, 284);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel39;
        private MetroFramework.Controls.MetroLabel metroLabel38;
        private MetroFramework.Controls.MetroComboBox cbMacarSAT;
        private MetroFramework.Controls.MetroTextBox txtCodigoAtivacaoSAT;
        private MetroFramework.Controls.MetroCheckBox ckConversaoNFCe;
        private MetroFramework.Controls.MetroTextBox txtSignAC;
        private MetroFramework.Controls.MetroTextBox txtCNPJSw;
        private MetroFramework.Controls.MetroLabel lblSignAC;
        private MetroFramework.Controls.MetroLabel lblCNPJSw;
    }
}

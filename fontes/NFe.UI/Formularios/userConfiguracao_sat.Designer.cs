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
            this.cbRegTribISSQN = new MetroFramework.Controls.MetroComboBox();
            this.cbindRatISSQN = new MetroFramework.Controls.MetroComboBox();
            this.lblRegTribISSQN = new MetroFramework.Controls.MetroLabel();
            this.lblIndRatISSQN = new MetroFramework.Controls.MetroLabel();
            this.lblNumeroCaixa = new MetroFramework.Controls.MetroLabel();
            this.txtNumeroCaixa = new MetroFramework.Controls.MetroTextBox();
            this.lblVersaoLayout = new MetroFramework.Controls.MetroLabel();
            this.comboVersaoLayout = new MetroFramework.Controls.MetroComboBox();
            this.lblTipoConversao = new MetroFramework.Controls.MetroLabel();
            this.comboTipoConversao = new MetroFramework.Controls.MetroComboBox();
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
            this.metroLabel38.Size = new System.Drawing.Size(87, 15);
            this.metroLabel38.TabIndex = 41;
            this.metroLabel38.Text = "Marca / Modelo";
            // 
            // cbMacarSAT
            // 
            this.cbMacarSAT.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbMacarSAT.FormattingEnabled = true;
            this.cbMacarSAT.ItemHeight = 19;
            this.cbMacarSAT.Items.AddRange(new object[] {
            "BEMATECH",
            "CONTROL_ID",
            "DARUMA",
            "DIMEP",
            "ELGIN",
            "ELGIN_II",
            "ELGIN_SMART",
            "EPSON",
            "EMULADOR",
            "NITERE",
            "SWEDA",
            "TANCA",
            "KRYPTUS"});
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
            this.txtSignAC.TextChanged += new System.EventHandler(this.txtSignAC_TextChanged);
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
            this.txtCNPJSw.TextChanged += new System.EventHandler(this.txtCNPJSw_TextChanged);
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
            // cbRegTribISSQN
            // 
            this.cbRegTribISSQN.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbRegTribISSQN.FormattingEnabled = true;
            this.cbRegTribISSQN.ItemHeight = 19;
            this.cbRegTribISSQN.Location = new System.Drawing.Point(329, 157);
            this.cbRegTribISSQN.Name = "cbRegTribISSQN";
            this.cbRegTribISSQN.Size = new System.Drawing.Size(288, 25);
            this.cbRegTribISSQN.TabIndex = 59;
            this.cbRegTribISSQN.UseSelectable = true;
            this.cbRegTribISSQN.SelectedIndexChanged += new System.EventHandler(this.cbRegTribISSQN_SelectedIndexChanged);
            // 
            // cbindRatISSQN
            // 
            this.cbindRatISSQN.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbindRatISSQN.FormattingEnabled = true;
            this.cbindRatISSQN.ItemHeight = 19;
            this.cbindRatISSQN.Location = new System.Drawing.Point(329, 205);
            this.cbindRatISSQN.Name = "cbindRatISSQN";
            this.cbindRatISSQN.Size = new System.Drawing.Size(288, 25);
            this.cbindRatISSQN.TabIndex = 60;
            this.cbindRatISSQN.UseSelectable = true;
            this.cbindRatISSQN.SelectedIndexChanged += new System.EventHandler(this.cbindRatISSQN_SelectedIndexChanged);
            // 
            // lblRegTribISSQN
            // 
            this.lblRegTribISSQN.AutoSize = true;
            this.lblRegTribISSQN.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblRegTribISSQN.Location = new System.Drawing.Point(329, 135);
            this.lblRegTribISSQN.Name = "lblRegTribISSQN";
            this.lblRegTribISSQN.Size = new System.Drawing.Size(135, 15);
            this.lblRegTribISSQN.TabIndex = 61;
            this.lblRegTribISSQN.Text = "Regime tributação ISSQN";
            // 
            // lblIndRatISSQN
            // 
            this.lblIndRatISSQN.AutoSize = true;
            this.lblIndRatISSQN.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblIndRatISSQN.Location = new System.Drawing.Point(329, 185);
            this.lblIndRatISSQN.Name = "lblIndRatISSQN";
            this.lblIndRatISSQN.Size = new System.Drawing.Size(261, 15);
            this.lblIndRatISSQN.TabIndex = 62;
            this.lblIndRatISSQN.Text = "Desconto subtotal rateado entre itens com ISSQN:";
            // 
            // lblNumeroCaixa
            // 
            this.lblNumeroCaixa.AutoSize = true;
            this.lblNumeroCaixa.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblNumeroCaixa.Location = new System.Drawing.Point(0, 240);
            this.lblNumeroCaixa.Name = "lblNumeroCaixa";
            this.lblNumeroCaixa.Size = new System.Drawing.Size(98, 15);
            this.lblNumeroCaixa.TabIndex = 64;
            this.lblNumeroCaixa.Text = "Numero do Caixa:";
            // 
            // txtNumeroCaixa
            // 
            this.txtNumeroCaixa.Lines = new string[0];
            this.txtNumeroCaixa.Location = new System.Drawing.Point(0, 258);
            this.txtNumeroCaixa.MaxLength = 3;
            this.txtNumeroCaixa.Name = "txtNumeroCaixa";
            this.txtNumeroCaixa.PasswordChar = '\0';
            this.txtNumeroCaixa.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNumeroCaixa.SelectedText = "";
            this.txtNumeroCaixa.Size = new System.Drawing.Size(310, 23);
            this.txtNumeroCaixa.TabIndex = 63;
            this.txtNumeroCaixa.UseSelectable = true;
            this.txtNumeroCaixa.TextChanged += new System.EventHandler(this.txtNumeroCaixa_TextChanged);
            // 
            // lblVersaoLayout
            // 
            this.lblVersaoLayout.AutoSize = true;
            this.lblVersaoLayout.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblVersaoLayout.Location = new System.Drawing.Point(329, 237);
            this.lblVersaoLayout.Name = "lblVersaoLayout";
            this.lblVersaoLayout.Size = new System.Drawing.Size(133, 15);
            this.lblVersaoLayout.TabIndex = 66;
            this.lblVersaoLayout.Text = "Versão do layout do SAT:";
            // 
            // comboVersaoLayout
            // 
            this.comboVersaoLayout.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboVersaoLayout.FormattingEnabled = true;
            this.comboVersaoLayout.ItemHeight = 19;
            this.comboVersaoLayout.Items.AddRange(new object[] {
            "0.07",
            "0.08"});
            this.comboVersaoLayout.Location = new System.Drawing.Point(329, 255);
            this.comboVersaoLayout.Name = "comboVersaoLayout";
            this.comboVersaoLayout.Size = new System.Drawing.Size(288, 25);
            this.comboVersaoLayout.TabIndex = 65;
            this.comboVersaoLayout.UseSelectable = true;
            this.comboVersaoLayout.SelectedIndexChanged += new System.EventHandler(this.ComboVersaoLayout_SelectedIndexChanged);
            // 
            // lblTipoConversao
            // 
            this.lblTipoConversao.AutoSize = true;
            this.lblTipoConversao.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblTipoConversao.Location = new System.Drawing.Point(329, 283);
            this.lblTipoConversao.Name = "lblTipoConversao";
            this.lblTipoConversao.Size = new System.Drawing.Size(104, 15);
            this.lblTipoConversao.TabIndex = 68;
            this.lblTipoConversao.Text = "Tipo de Conversão:";
            // 
            // comboTipoConversao
            // 
            this.comboTipoConversao.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboTipoConversao.FormattingEnabled = true;
            this.comboTipoConversao.ItemHeight = 19;
            this.comboTipoConversao.Items.AddRange(new object[] {
            "Truncamento",
            "Arredondamento"});
            this.comboTipoConversao.Location = new System.Drawing.Point(329, 301);
            this.comboTipoConversao.Name = "comboTipoConversao";
            this.comboTipoConversao.Size = new System.Drawing.Size(288, 25);
            this.comboTipoConversao.TabIndex = 67;
            this.comboTipoConversao.UseSelectable = true;
            this.comboTipoConversao.SelectedIndexChanged += new System.EventHandler(this.comboTipoConversao_SelectedIndexChanged);
            // 
            // userConfiguracao_sat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lblTipoConversao);
            this.Controls.Add(this.comboTipoConversao);
            this.Controls.Add(this.lblVersaoLayout);
            this.Controls.Add(this.comboVersaoLayout);
            this.Controls.Add(this.lblNumeroCaixa);
            this.Controls.Add(this.txtNumeroCaixa);
            this.Controls.Add(this.lblIndRatISSQN);
            this.Controls.Add(this.lblRegTribISSQN);
            this.Controls.Add(this.cbindRatISSQN);
            this.Controls.Add(this.cbRegTribISSQN);
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
            this.Size = new System.Drawing.Size(638, 333);
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
        private MetroFramework.Controls.MetroComboBox cbRegTribISSQN;
        private MetroFramework.Controls.MetroComboBox cbindRatISSQN;
        private MetroFramework.Controls.MetroLabel lblRegTribISSQN;
        private MetroFramework.Controls.MetroLabel lblIndRatISSQN;
        private MetroFramework.Controls.MetroLabel lblNumeroCaixa;
        private MetroFramework.Controls.MetroTextBox txtNumeroCaixa;
        private MetroFramework.Controls.MetroLabel lblVersaoLayout;
        private MetroFramework.Controls.MetroComboBox comboVersaoLayout;
        private MetroFramework.Controls.MetroLabel lblTipoConversao;
        private MetroFramework.Controls.MetroComboBox comboTipoConversao;
    }
}

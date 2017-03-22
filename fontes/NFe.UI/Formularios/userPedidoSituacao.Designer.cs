namespace NFe.UI
{
    partial class userPedidoSituacao
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
            this.metroLabel17 = new MetroFramework.Controls.MetroLabel();
            this.cbEmpresa = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.comboUf = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cbVersao = new MetroFramework.Controls.MetroComboBox();
            this.lblAmbiente = new MetroFramework.Controls.MetroLabel();
            this.cbAmbiente = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.cbEmissao = new MetroFramework.Controls.MetroComboBox();
            this.lblServico = new MetroFramework.Controls.MetroLabel();
            this.cbServico = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.textResultado = new MetroFramework.Controls.MetroTextBox();
            this.buttonPesquisa = new MetroFramework.Controls.MetroButton();
            this.lblCNPJ = new MetroFramework.Controls.MetroLabel();
            this.txtCNPJ = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Size = new System.Drawing.Size(244, 25);
            this.labelTitle.Text = "Pedido de situação de serviços";
            // 
            // metroLabel17
            // 
            this.metroLabel17.AutoSize = true;
            this.metroLabel17.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel17.Location = new System.Drawing.Point(0, 58);
            this.metroLabel17.Name = "metroLabel17";
            this.metroLabel17.Size = new System.Drawing.Size(239, 15);
            this.metroLabel17.TabIndex = 16;
            this.metroLabel17.Text = "Utilizar para consulta o certificado da empresa";
            // 
            // cbEmpresa
            // 
            this.cbEmpresa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEmpresa.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbEmpresa.FormattingEnabled = true;
            this.cbEmpresa.IntegralHeight = false;
            this.cbEmpresa.ItemHeight = 19;
            this.cbEmpresa.Items.AddRange(new object[] {
            "Item 4"});
            this.cbEmpresa.Location = new System.Drawing.Point(0, 77);
            this.cbEmpresa.MaxDropDownItems = 15;
            this.cbEmpresa.Name = "cbEmpresa";
            this.cbEmpresa.Size = new System.Drawing.Size(778, 25);
            this.cbEmpresa.TabIndex = 15;
            this.cbEmpresa.UseSelectable = true;
            this.cbEmpresa.SelectedIndexChanged += new System.EventHandler(this.cbEmpresa_SelectedIndexChanged);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.Location = new System.Drawing.Point(0, 109);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(146, 15);
            this.metroLabel1.TabIndex = 18;
            this.metroLabel1.Text = "UF para o envio da consulta";
            // 
            // comboUf
            // 
            this.comboUf.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboUf.FormattingEnabled = true;
            this.comboUf.IntegralHeight = false;
            this.comboUf.ItemHeight = 19;
            this.comboUf.Items.AddRange(new object[] {
            "Item 4"});
            this.comboUf.Location = new System.Drawing.Point(0, 128);
            this.comboUf.MaxDropDownItems = 15;
            this.comboUf.Name = "comboUf";
            this.comboUf.Size = new System.Drawing.Size(181, 25);
            this.comboUf.TabIndex = 17;
            this.comboUf.UseSelectable = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel2.Location = new System.Drawing.Point(189, 109);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(41, 15);
            this.metroLabel2.TabIndex = 20;
            this.metroLabel2.Text = "Versão";
            // 
            // cbVersao
            // 
            this.cbVersao.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbVersao.FormattingEnabled = true;
            this.cbVersao.ItemHeight = 19;
            this.cbVersao.Items.AddRange(new object[] {
            "3.10",
            "3.00",
            "2.00"});
            this.cbVersao.Location = new System.Drawing.Point(185, 128);
            this.cbVersao.MaxDropDownItems = 15;
            this.cbVersao.Name = "cbVersao";
            this.cbVersao.Size = new System.Drawing.Size(185, 25);
            this.cbVersao.TabIndex = 19;
            this.cbVersao.UseSelectable = true;
            // 
            // lblAmbiente
            // 
            this.lblAmbiente.AutoSize = true;
            this.lblAmbiente.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblAmbiente.Location = new System.Drawing.Point(374, 109);
            this.lblAmbiente.Name = "lblAmbiente";
            this.lblAmbiente.Size = new System.Drawing.Size(56, 15);
            this.lblAmbiente.TabIndex = 22;
            this.lblAmbiente.Text = "Ambiente";
            // 
            // cbAmbiente
            // 
            this.cbAmbiente.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbAmbiente.FormattingEnabled = true;
            this.cbAmbiente.ItemHeight = 19;
            this.cbAmbiente.Items.AddRange(new object[] {
            "A"});
            this.cbAmbiente.Location = new System.Drawing.Point(374, 128);
            this.cbAmbiente.MaxDropDownItems = 15;
            this.cbAmbiente.Name = "cbAmbiente";
            this.cbAmbiente.Size = new System.Drawing.Size(181, 25);
            this.cbAmbiente.TabIndex = 21;
            this.cbAmbiente.UseSelectable = true;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel4.Location = new System.Drawing.Point(0, 161);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(89, 15);
            this.metroLabel4.TabIndex = 24;
            this.metroLabel4.Text = "Tipo de emissão";
            // 
            // cbEmissao
            // 
            this.cbEmissao.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbEmissao.FormattingEnabled = true;
            this.cbEmissao.ItemHeight = 19;
            this.cbEmissao.Items.AddRange(new object[] {
            "a"});
            this.cbEmissao.Location = new System.Drawing.Point(0, 181);
            this.cbEmissao.MaxDropDownItems = 15;
            this.cbEmissao.Name = "cbEmissao";
            this.cbEmissao.Size = new System.Drawing.Size(370, 25);
            this.cbEmissao.TabIndex = 23;
            this.cbEmissao.UseSelectable = true;
            // 
            // lblServico
            // 
            this.lblServico.AutoSize = true;
            this.lblServico.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblServico.Location = new System.Drawing.Point(374, 161);
            this.lblServico.Name = "lblServico";
            this.lblServico.Size = new System.Drawing.Size(43, 15);
            this.lblServico.TabIndex = 26;
            this.lblServico.Text = "Serviço";
            // 
            // cbServico
            // 
            this.cbServico.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbServico.FormattingEnabled = true;
            this.cbServico.ItemHeight = 19;
            this.cbServico.Items.AddRange(new object[] {
            "A"});
            this.cbServico.Location = new System.Drawing.Point(374, 181);
            this.cbServico.MaxDropDownItems = 15;
            this.cbServico.Name = "cbServico";
            this.cbServico.Size = new System.Drawing.Size(181, 25);
            this.cbServico.TabIndex = 25;
            this.cbServico.UseSelectable = true;
            this.cbServico.SelectedIndexChanged += new System.EventHandler(this.cbServico_SelectedIndexChanged);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel3.Location = new System.Drawing.Point(0, 225);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(49, 15);
            this.metroLabel3.TabIndex = 27;
            this.metroLabel3.Text = "Situação";
            // 
            // textResultado
            // 
            this.textResultado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textResultado.Lines = new string[0];
            this.textResultado.Location = new System.Drawing.Point(0, 245);
            this.textResultado.MaxLength = 32767;
            this.textResultado.Multiline = true;
            this.textResultado.Name = "textResultado";
            this.textResultado.PasswordChar = '\0';
            this.textResultado.ReadOnly = true;
            this.textResultado.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textResultado.SelectedText = "";
            this.textResultado.Size = new System.Drawing.Size(780, 236);
            this.textResultado.TabIndex = 28;
            this.textResultado.UseSelectable = true;
            this.textResultado.UseStyleColors = true;
            // 
            // buttonPesquisa
            // 
            this.buttonPesquisa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPesquisa.Location = new System.Drawing.Point(698, 181);
            this.buttonPesquisa.Name = "buttonPesquisa";
            this.buttonPesquisa.Size = new System.Drawing.Size(80, 25);
            this.buttonPesquisa.TabIndex = 29;
            this.buttonPesquisa.Text = "Consultar";
            this.buttonPesquisa.UseSelectable = true;
            this.buttonPesquisa.Click += new System.EventHandler(this.buttonPesquisa_Click);
            // 
            // lblCNPJ
            // 
            this.lblCNPJ.AutoSize = true;
            this.lblCNPJ.Enabled = false;
            this.lblCNPJ.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblCNPJ.Location = new System.Drawing.Point(561, 109);
            this.lblCNPJ.Name = "lblCNPJ";
            this.lblCNPJ.Size = new System.Drawing.Size(36, 15);
            this.lblCNPJ.TabIndex = 30;
            this.lblCNPJ.Text = "CNPJ:";
            // 
            // txtCNPJ
            // 
            this.txtCNPJ.Enabled = false;
            this.txtCNPJ.Lines = new string[0];
            this.txtCNPJ.Location = new System.Drawing.Point(561, 128);
            this.txtCNPJ.MaxLength = 32767;
            this.txtCNPJ.Name = "txtCNPJ";
            this.txtCNPJ.PasswordChar = '\0';
            this.txtCNPJ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCNPJ.SelectedText = "";
            this.txtCNPJ.Size = new System.Drawing.Size(131, 25);
            this.txtCNPJ.TabIndex = 31;
            this.txtCNPJ.UseSelectable = true;
            // 
            // userPedidoSituacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtCNPJ);
            this.Controls.Add(this.lblCNPJ);
            this.Controls.Add(this.buttonPesquisa);
            this.Controls.Add(this.textResultado);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.lblServico);
            this.Controls.Add(this.cbServico);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.cbEmissao);
            this.Controls.Add(this.lblAmbiente);
            this.Controls.Add(this.cbAmbiente);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.cbVersao);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.comboUf);
            this.Controls.Add(this.metroLabel17);
            this.Controls.Add(this.cbEmpresa);
            this.Name = "userPedidoSituacao";
            this.Size = new System.Drawing.Size(783, 484);
            this.Controls.SetChildIndex(this.labelTitle, 0);
            this.Controls.SetChildIndex(this.cbEmpresa, 0);
            this.Controls.SetChildIndex(this.metroLabel17, 0);
            this.Controls.SetChildIndex(this.comboUf, 0);
            this.Controls.SetChildIndex(this.metroLabel1, 0);
            this.Controls.SetChildIndex(this.cbVersao, 0);
            this.Controls.SetChildIndex(this.metroLabel2, 0);
            this.Controls.SetChildIndex(this.cbAmbiente, 0);
            this.Controls.SetChildIndex(this.lblAmbiente, 0);
            this.Controls.SetChildIndex(this.cbEmissao, 0);
            this.Controls.SetChildIndex(this.metroLabel4, 0);
            this.Controls.SetChildIndex(this.cbServico, 0);
            this.Controls.SetChildIndex(this.lblServico, 0);
            this.Controls.SetChildIndex(this.metroLabel3, 0);
            this.Controls.SetChildIndex(this.textResultado, 0);
            this.Controls.SetChildIndex(this.buttonPesquisa, 0);
            this.Controls.SetChildIndex(this.lblCNPJ, 0);
            this.Controls.SetChildIndex(this.txtCNPJ, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel17;
        private MetroFramework.Controls.MetroComboBox cbEmpresa;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroComboBox comboUf;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroComboBox cbVersao;
        private MetroFramework.Controls.MetroLabel lblAmbiente;
        private MetroFramework.Controls.MetroComboBox cbAmbiente;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroComboBox cbEmissao;
        private MetroFramework.Controls.MetroLabel lblServico;
        private MetroFramework.Controls.MetroComboBox cbServico;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox textResultado;
        private MetroFramework.Controls.MetroButton buttonPesquisa;
        private MetroFramework.Controls.MetroLabel lblCNPJ;
        private MetroFramework.Controls.MetroTextBox txtCNPJ;
    }
}

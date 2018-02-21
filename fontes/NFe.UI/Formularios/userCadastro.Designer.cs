namespace NFe.UI
{
    partial class userCadastro
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
            this.buttonPesquisa = new MetroFramework.Controls.MetroButton();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cbVersao = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.comboUf = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel17 = new MetroFramework.Controls.MetroLabel();
            this.cbEmpresa = new MetroFramework.Controls.MetroComboBox();
            this.rbCPF = new MetroFramework.Controls.MetroRadioButton();
            this.metroLabel18 = new MetroFramework.Controls.MetroLabel();
            this.rbCNPJ = new MetroFramework.Controls.MetroRadioButton();
            this.rbIE = new MetroFramework.Controls.MetroRadioButton();
            this.textConteudo = new MetroFramework.Controls.MetroTextBox();
            this.textResultado = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Size = new System.Drawing.Size(291, 25);
            this.labelTitle.Text = "Consulta ao cadastro de contribuinte";
            // 
            // buttonPesquisa
            // 
            this.buttonPesquisa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPesquisa.Location = new System.Drawing.Point(685, 201);
            this.buttonPesquisa.Name = "buttonPesquisa";
            this.buttonPesquisa.Size = new System.Drawing.Size(80, 25);
            this.buttonPesquisa.TabIndex = 38;
            this.buttonPesquisa.Text = "Consultar";
            this.buttonPesquisa.UseSelectable = true;
            this.buttonPesquisa.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel2.Location = new System.Drawing.Point(230, 109);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(41, 15);
            this.metroLabel2.TabIndex = 35;
            this.metroLabel2.Text = "Versão";
            // 
            // cbVersao
            // 
            this.cbVersao.Enabled = false;
            this.cbVersao.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbVersao.FormattingEnabled = true;
            this.cbVersao.ItemHeight = 19;
            this.cbVersao.Items.AddRange(new object[] {
            "2.00",
            "3.10"});
            this.cbVersao.Location = new System.Drawing.Point(232, 127);
            this.cbVersao.MaxDropDownItems = 15;
            this.cbVersao.Name = "cbVersao";
            this.cbVersao.PromptText = "Here we go again...";
            this.cbVersao.Size = new System.Drawing.Size(181, 25);
            this.cbVersao.TabIndex = 34;
            this.cbVersao.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.Location = new System.Drawing.Point(0, 109);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(146, 15);
            this.metroLabel1.TabIndex = 33;
            this.metroLabel1.Text = "UF para o envio da consulta";
            // 
            // comboUf
            // 
            this.comboUf.BackColor = System.Drawing.SystemColors.Highlight;
            this.comboUf.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboUf.FormattingEnabled = true;
            this.comboUf.IntegralHeight = false;
            this.comboUf.ItemHeight = 19;
            this.comboUf.Items.AddRange(new object[] {
            "AC Acre\",",
            "AL Alagoas\",",
            "AM Amazonas\",",
            "AP Amapá\",",
            "BA Bahia\",",
            "CE Ceará\",",
            "DF Distrito Federal\",",
            "ES Espírito Santo\",",
            "GO Goiás\",",
            "MA Maranhão\",",
            "MG Minas Gerais\",",
            "MS Mato Grosso do Sul\"",
            "MT Mato Grosso\",",
            "PA Pará\",",
            "PB Paraiba\",",
            "PE Pernambuco\",",
            "PI Piauí\",",
            "PR Paraná\",",
            "RJ Rio de Janeiro\",",
            "RN Rio Grande do Norte",
            "RO Rondonia\",",
            "RR Roraima\",",
            "RS Rio Grande do Sul\",",
            "SC Santa Catarina\",",
            "SE Sergipe\",",
            "SP São Paulo\",",
            "TO Tocantins\","});
            this.comboUf.Location = new System.Drawing.Point(0, 127);
            this.comboUf.MaxDropDownItems = 15;
            this.comboUf.Name = "comboUf";
            this.comboUf.PromptText = "Here we go again...";
            this.comboUf.Size = new System.Drawing.Size(226, 25);
            this.comboUf.TabIndex = 32;
            this.comboUf.UseSelectable = true;
            // 
            // metroLabel17
            // 
            this.metroLabel17.AutoSize = true;
            this.metroLabel17.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel17.Location = new System.Drawing.Point(0, 58);
            this.metroLabel17.Name = "metroLabel17";
            this.metroLabel17.Size = new System.Drawing.Size(242, 15);
            this.metroLabel17.TabIndex = 31;
            this.metroLabel17.Text = "Utilizar para consulta o certificado da empresa:";
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
            "Normal Combobox",
            "Item 2",
            "Item 3",
            "Item 4"});
            this.cbEmpresa.Location = new System.Drawing.Point(0, 77);
            this.cbEmpresa.MaxDropDownItems = 15;
            this.cbEmpresa.Name = "cbEmpresa";
            this.cbEmpresa.PromptText = "Here we go again...";
            this.cbEmpresa.Size = new System.Drawing.Size(765, 25);
            this.cbEmpresa.TabIndex = 30;
            this.cbEmpresa.UseSelectable = true;
            this.cbEmpresa.SelectedIndexChanged += new System.EventHandler(this.cbEmpresa_SelectedIndexChanged);
            // 
            // rbCPF
            // 
            this.rbCPF.AutoSize = true;
            this.rbCPF.Location = new System.Drawing.Point(67, 180);
            this.rbCPF.Name = "rbCPF";
            this.rbCPF.Size = new System.Drawing.Size(44, 15);
            this.rbCPF.TabIndex = 41;
            this.rbCPF.Text = "CPF";
            this.rbCPF.UseSelectable = true;
            this.rbCPF.CheckedChanged += new System.EventHandler(this.rbCPF_CheckedChanged);
            // 
            // metroLabel18
            // 
            this.metroLabel18.AutoSize = true;
            this.metroLabel18.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel18.Location = new System.Drawing.Point(0, 160);
            this.metroLabel18.Name = "metroLabel18";
            this.metroLabel18.Size = new System.Drawing.Size(130, 15);
            this.metroLabel18.TabIndex = 40;
            this.metroLabel18.Text = "Conteudo para pesquisa";
            // 
            // rbCNPJ
            // 
            this.rbCNPJ.AutoSize = true;
            this.rbCNPJ.Checked = true;
            this.rbCNPJ.Location = new System.Drawing.Point(0, 180);
            this.rbCNPJ.Name = "rbCNPJ";
            this.rbCNPJ.Size = new System.Drawing.Size(50, 15);
            this.rbCNPJ.TabIndex = 39;
            this.rbCNPJ.TabStop = true;
            this.rbCNPJ.Text = "CNPJ";
            this.rbCNPJ.UseSelectable = true;
            this.rbCNPJ.CheckedChanged += new System.EventHandler(this.rbCNPJ_CheckedChanged);
            // 
            // rbIE
            // 
            this.rbIE.AutoSize = true;
            this.rbIE.Location = new System.Drawing.Point(128, 180);
            this.rbIE.Name = "rbIE";
            this.rbIE.Size = new System.Drawing.Size(32, 15);
            this.rbIE.TabIndex = 42;
            this.rbIE.TabStop = true;
            this.rbIE.Text = "IE";
            this.rbIE.UseSelectable = true;
            this.rbIE.CheckedChanged += new System.EventHandler(this.rbIE_CheckedChanged);
            // 
            // textConteudo
            // 
            this.textConteudo.Lines = new string[0];
            this.textConteudo.Location = new System.Drawing.Point(0, 201);
            this.textConteudo.MaxLength = 32767;
            this.textConteudo.Name = "textConteudo";
            this.textConteudo.PasswordChar = '\0';
            this.textConteudo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textConteudo.SelectedText = "";
            this.textConteudo.Size = new System.Drawing.Size(181, 22);
            this.textConteudo.TabIndex = 43;
            this.textConteudo.UseSelectable = true;
            // 
            // textResultado
            // 
            this.textResultado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textResultado.Lines = new string[0];
            this.textResultado.Location = new System.Drawing.Point(0, 249);
            this.textResultado.MaxLength = 32767;
            this.textResultado.Multiline = true;
            this.textResultado.Name = "textResultado";
            this.textResultado.PasswordChar = '\0';
            this.textResultado.ReadOnly = true;
            this.textResultado.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textResultado.SelectedText = "";
            this.textResultado.Size = new System.Drawing.Size(766, 141);
            this.textResultado.TabIndex = 44;
            this.textResultado.UseSelectable = true;
            this.textResultado.UseStyleColors = true;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel3.Location = new System.Drawing.Point(0, 230);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(38, 15);
            this.metroLabel3.TabIndex = 45;
            this.metroLabel3.Text = "Status";
            // 
            // userCadastro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.textResultado);
            this.Controls.Add(this.textConteudo);
            this.Controls.Add(this.rbIE);
            this.Controls.Add(this.rbCPF);
            this.Controls.Add(this.metroLabel18);
            this.Controls.Add(this.rbCNPJ);
            this.Controls.Add(this.buttonPesquisa);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.cbVersao);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.comboUf);
            this.Controls.Add(this.metroLabel17);
            this.Controls.Add(this.cbEmpresa);
            this.Name = "userCadastro";
            this.Size = new System.Drawing.Size(768, 393);
            this.Controls.SetChildIndex(this.labelTitle, 0);
            this.Controls.SetChildIndex(this.cbEmpresa, 0);
            this.Controls.SetChildIndex(this.metroLabel17, 0);
            this.Controls.SetChildIndex(this.comboUf, 0);
            this.Controls.SetChildIndex(this.metroLabel1, 0);
            this.Controls.SetChildIndex(this.cbVersao, 0);
            this.Controls.SetChildIndex(this.metroLabel2, 0);
            this.Controls.SetChildIndex(this.buttonPesquisa, 0);
            this.Controls.SetChildIndex(this.rbCNPJ, 0);
            this.Controls.SetChildIndex(this.metroLabel18, 0);
            this.Controls.SetChildIndex(this.rbCPF, 0);
            this.Controls.SetChildIndex(this.rbIE, 0);
            this.Controls.SetChildIndex(this.textConteudo, 0);
            this.Controls.SetChildIndex(this.textResultado, 0);
            this.Controls.SetChildIndex(this.metroLabel3, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton buttonPesquisa;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroComboBox cbVersao;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroComboBox comboUf;
        private MetroFramework.Controls.MetroLabel metroLabel17;
        private MetroFramework.Controls.MetroComboBox cbEmpresa;
        private MetroFramework.Controls.MetroRadioButton rbCPF;
        private MetroFramework.Controls.MetroLabel metroLabel18;
        private MetroFramework.Controls.MetroRadioButton rbCNPJ;
        private MetroFramework.Controls.MetroRadioButton rbIE;
        private MetroFramework.Controls.MetroTextBox textConteudo;
        private MetroFramework.Controls.MetroTextBox textResultado;
        private MetroFramework.Controls.MetroLabel metroLabel3;

    }
}

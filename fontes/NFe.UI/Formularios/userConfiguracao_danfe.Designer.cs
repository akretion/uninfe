namespace NFe.UI.Formularios
{
    partial class userConfiguracao_danfe
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
            this.cbDanfeMonProcNfe = new MetroFramework.Controls.MetroCheckBox();
            this.cbDanfeMonNfe = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabel37 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel36 = new MetroFramework.Controls.MetroLabel();
            this.tbConfiguracaoCCe = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel35 = new MetroFramework.Controls.MetroLabel();
            this.tbConfiguracaoDanfe = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel34 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel33 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel32 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel31 = new MetroFramework.Controls.MetroLabel();
            this.tbTextoDANFE = new MetroFramework.Controls.MetroTextBox();
            this.tbPastaXmlParaDanfeMon = new MetroFramework.Controls.MetroTextBox();
            this.tbPastaConfigUniDanfe = new MetroFramework.Controls.MetroTextBox();
            this.tbPastaExeUniDanfe = new MetroFramework.Controls.MetroTextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.edtEmailDANFE = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.chkAddEmailDANFE = new MetroFramework.Controls.MetroCheckBox();
            this.SuspendLayout();
            // 
            // cbDanfeMonProcNfe
            // 
            this.cbDanfeMonProcNfe.AutoSize = true;
            this.cbDanfeMonProcNfe.Location = new System.Drawing.Point(27, 340);
            this.cbDanfeMonProcNfe.Name = "cbDanfeMonProcNfe";
            this.cbDanfeMonProcNfe.Size = new System.Drawing.Size(296, 15);
            this.cbDanfeMonProcNfe.TabIndex = 14;
            this.cbDanfeMonProcNfe.Text = "XML de distribuição do documento fiscal eletrônico";
            this.cbDanfeMonProcNfe.UseSelectable = true;
            this.cbDanfeMonProcNfe.CheckStateChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            // 
            // cbDanfeMonNfe
            // 
            this.cbDanfeMonNfe.AutoSize = true;
            this.cbDanfeMonNfe.Location = new System.Drawing.Point(27, 319);
            this.cbDanfeMonNfe.Name = "cbDanfeMonNfe";
            this.cbDanfeMonNfe.Size = new System.Drawing.Size(215, 15);
            this.cbDanfeMonNfe.TabIndex = 13;
            this.cbDanfeMonNfe.Text = "XML do documento fiscal eletrônico";
            this.cbDanfeMonNfe.UseSelectable = true;
            this.cbDanfeMonNfe.CheckStateChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            // 
            // metroLabel37
            // 
            this.metroLabel37.AutoSize = true;
            this.metroLabel37.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel37.Location = new System.Drawing.Point(3, 298);
            this.metroLabel37.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel37.Name = "metroLabel37";
            this.metroLabel37.Size = new System.Drawing.Size(436, 15);
            this.metroLabel37.TabIndex = 12;
            this.metroLabel37.Text = "XML´s a serem copiados na pasta para impressão do DANFE a partir do DANFE Mon";
            // 
            // metroLabel36
            // 
            this.metroLabel36.AutoSize = true;
            this.metroLabel36.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel36.Location = new System.Drawing.Point(3, 182);
            this.metroLabel36.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel36.Name = "metroLabel36";
            this.metroLabel36.Size = new System.Drawing.Size(137, 15);
            this.metroLabel36.TabIndex = 5;
            this.metroLabel36.Text = "Nomes das configurações";
            // 
            // tbConfiguracaoCCe
            // 
            this.tbConfiguracaoCCe.Lines = new string[] {
        "Styled Textbox"};
            this.tbConfiguracaoCCe.Location = new System.Drawing.Point(305, 222);
            this.tbConfiguracaoCCe.MaxLength = 32767;
            this.tbConfiguracaoCCe.Name = "tbConfiguracaoCCe";
            this.tbConfiguracaoCCe.PasswordChar = '\0';
            this.tbConfiguracaoCCe.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbConfiguracaoCCe.SelectedText = "";
            this.tbConfiguracaoCCe.Size = new System.Drawing.Size(272, 22);
            this.tbConfiguracaoCCe.TabIndex = 9;
            this.tbConfiguracaoCCe.Text = "Styled Textbox";
            this.tbConfiguracaoCCe.UseSelectable = true;
            this.tbConfiguracaoCCe.UseStyleColors = true;
            this.tbConfiguracaoCCe.TextChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            // 
            // metroLabel35
            // 
            this.metroLabel35.AutoSize = true;
            this.metroLabel35.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel35.Location = new System.Drawing.Point(305, 203);
            this.metroLabel35.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel35.Name = "metroLabel35";
            this.metroLabel35.Size = new System.Drawing.Size(27, 15);
            this.metroLabel35.TabIndex = 8;
            this.metroLabel35.Text = "CCe";
            // 
            // tbConfiguracaoDanfe
            // 
            this.tbConfiguracaoDanfe.Lines = new string[] {
        "Styled Textbox"};
            this.tbConfiguracaoDanfe.Location = new System.Drawing.Point(27, 222);
            this.tbConfiguracaoDanfe.MaxLength = 32767;
            this.tbConfiguracaoDanfe.Name = "tbConfiguracaoDanfe";
            this.tbConfiguracaoDanfe.PasswordChar = '\0';
            this.tbConfiguracaoDanfe.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbConfiguracaoDanfe.SelectedText = "";
            this.tbConfiguracaoDanfe.Size = new System.Drawing.Size(272, 22);
            this.tbConfiguracaoDanfe.TabIndex = 7;
            this.tbConfiguracaoDanfe.Text = "Styled Textbox";
            this.tbConfiguracaoDanfe.UseSelectable = true;
            this.tbConfiguracaoDanfe.UseStyleColors = true;
            this.tbConfiguracaoDanfe.TextChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            // 
            // metroLabel34
            // 
            this.metroLabel34.AutoSize = true;
            this.metroLabel34.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel34.Location = new System.Drawing.Point(27, 203);
            this.metroLabel34.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel34.Name = "metroLabel34";
            this.metroLabel34.Size = new System.Drawing.Size(115, 15);
            this.metroLabel34.TabIndex = 6;
            this.metroLabel34.Text = "NFe/MDFe/CTe/NFCe";
            // 
            // metroLabel33
            // 
            this.metroLabel33.AutoSize = true;
            this.metroLabel33.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel33.Location = new System.Drawing.Point(3, 250);
            this.metroLabel33.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel33.Name = "metroLabel33";
            this.metroLabel33.Size = new System.Drawing.Size(505, 15);
            this.metroLabel33.TabIndex = 10;
            this.metroLabel33.Text = "Pasta onde deve ser gravado o XML para a impressão do documento fiscal a partir d" +
    "o DANFE Mon";
            // 
            // metroLabel32
            // 
            this.metroLabel32.AutoSize = true;
            this.metroLabel32.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel32.Location = new System.Drawing.Point(3, 139);
            this.metroLabel32.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel32.Name = "metroLabel32";
            this.metroLabel32.Size = new System.Drawing.Size(252, 15);
            this.metroLabel32.TabIndex = 3;
            this.metroLabel32.Text = "Pasta do arquivo de configurações do UniDANFE";
            // 
            // metroLabel31
            // 
            this.metroLabel31.AutoSize = true;
            this.metroLabel31.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel31.Location = new System.Drawing.Point(3, 98);
            this.metroLabel31.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel31.Name = "metroLabel31";
            this.metroLabel31.Size = new System.Drawing.Size(178, 15);
            this.metroLabel31.TabIndex = 1;
            this.metroLabel31.Text = "Pasta do executável do UniDANFE";
            // 
            // tbTextoDANFE
            // 
            this.tbTextoDANFE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTextoDANFE.Lines = new string[0];
            this.tbTextoDANFE.Location = new System.Drawing.Point(3, 3);
            this.tbTextoDANFE.MaxLength = 32767;
            this.tbTextoDANFE.MinimumSize = new System.Drawing.Size(651, 89);
            this.tbTextoDANFE.Multiline = true;
            this.tbTextoDANFE.Name = "tbTextoDANFE";
            this.tbTextoDANFE.PasswordChar = '\0';
            this.tbTextoDANFE.ReadOnly = true;
            this.tbTextoDANFE.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbTextoDANFE.SelectedText = "";
            this.tbTextoDANFE.Size = new System.Drawing.Size(651, 89);
            this.tbTextoDANFE.TabIndex = 0;
            this.tbTextoDANFE.UseSelectable = true;
            // 
            // tbPastaXmlParaDanfeMon
            // 
            this.tbPastaXmlParaDanfeMon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPastaXmlParaDanfeMon.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbPastaXmlParaDanfeMon.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.tbPastaXmlParaDanfeMon.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.tbPastaXmlParaDanfeMon.IconRight = true;
            this.tbPastaXmlParaDanfeMon.Lines = new string[] {
        "Styled Textbox"};
            this.tbPastaXmlParaDanfeMon.Location = new System.Drawing.Point(3, 268);
            this.tbPastaXmlParaDanfeMon.MaxLength = 32767;
            this.tbPastaXmlParaDanfeMon.Name = "tbPastaXmlParaDanfeMon";
            this.tbPastaXmlParaDanfeMon.PasswordChar = '\0';
            this.tbPastaXmlParaDanfeMon.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbPastaXmlParaDanfeMon.SelectedText = "";
            this.tbPastaXmlParaDanfeMon.Size = new System.Drawing.Size(651, 22);
            this.tbPastaXmlParaDanfeMon.TabIndex = 11;
            this.tbPastaXmlParaDanfeMon.Text = "Styled Textbox";
            this.tbPastaXmlParaDanfeMon.UseSelectable = true;
            this.tbPastaXmlParaDanfeMon.UseStyleColors = true;
            this.tbPastaXmlParaDanfeMon.TextChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            this.tbPastaXmlParaDanfeMon.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPastaExeUniDanfe_KeyDown);
            this.tbPastaXmlParaDanfeMon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPastaXmlParaDanfeMon_MouseDown);
            // 
            // tbPastaConfigUniDanfe
            // 
            this.tbPastaConfigUniDanfe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPastaConfigUniDanfe.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbPastaConfigUniDanfe.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.tbPastaConfigUniDanfe.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.tbPastaConfigUniDanfe.IconRight = true;
            this.tbPastaConfigUniDanfe.Lines = new string[] {
        "Styled Textbox"};
            this.tbPastaConfigUniDanfe.Location = new System.Drawing.Point(3, 157);
            this.tbPastaConfigUniDanfe.MaxLength = 32767;
            this.tbPastaConfigUniDanfe.Name = "tbPastaConfigUniDanfe";
            this.tbPastaConfigUniDanfe.PasswordChar = '\0';
            this.tbPastaConfigUniDanfe.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbPastaConfigUniDanfe.SelectedText = "";
            this.tbPastaConfigUniDanfe.Size = new System.Drawing.Size(651, 22);
            this.tbPastaConfigUniDanfe.TabIndex = 4;
            this.tbPastaConfigUniDanfe.Text = "Styled Textbox";
            this.tbPastaConfigUniDanfe.UseSelectable = true;
            this.tbPastaConfigUniDanfe.UseStyleColors = true;
            this.tbPastaConfigUniDanfe.TextChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            this.tbPastaConfigUniDanfe.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPastaExeUniDanfe_KeyDown);
            this.tbPastaConfigUniDanfe.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPastaConfigUniDanfe_MouseDown);
            // 
            // tbPastaExeUniDanfe
            // 
            this.tbPastaExeUniDanfe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPastaExeUniDanfe.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbPastaExeUniDanfe.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.tbPastaExeUniDanfe.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.tbPastaExeUniDanfe.IconRight = true;
            this.tbPastaExeUniDanfe.Lines = new string[] {
        "Styled Textbox"};
            this.tbPastaExeUniDanfe.Location = new System.Drawing.Point(3, 115);
            this.tbPastaExeUniDanfe.MaxLength = 32767;
            this.tbPastaExeUniDanfe.Name = "tbPastaExeUniDanfe";
            this.tbPastaExeUniDanfe.PasswordChar = '\0';
            this.tbPastaExeUniDanfe.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbPastaExeUniDanfe.SelectedText = "";
            this.tbPastaExeUniDanfe.Size = new System.Drawing.Size(651, 22);
            this.tbPastaExeUniDanfe.TabIndex = 2;
            this.tbPastaExeUniDanfe.Text = "Styled Textbox";
            this.tbPastaExeUniDanfe.UseSelectable = true;
            this.tbPastaExeUniDanfe.UseStyleColors = true;
            this.tbPastaExeUniDanfe.TextChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            this.tbPastaExeUniDanfe.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPastaExeUniDanfe_KeyDown);
            this.tbPastaExeUniDanfe.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPastaExeUniDanfe_MouseDown);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // edtEmailDANFE
            // 
            this.edtEmailDANFE.Lines = new string[] {
        "Styled Textbox"};
            this.edtEmailDANFE.Location = new System.Drawing.Point(3, 380);
            this.edtEmailDANFE.MaxLength = 32767;
            this.edtEmailDANFE.Name = "edtEmailDANFE";
            this.edtEmailDANFE.PasswordChar = '\0';
            this.edtEmailDANFE.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtEmailDANFE.SelectedText = "";
            this.edtEmailDANFE.Size = new System.Drawing.Size(296, 22);
            this.edtEmailDANFE.TabIndex = 16;
            this.edtEmailDANFE.Text = "Styled Textbox";
            this.edtEmailDANFE.UseSelectable = true;
            this.edtEmailDANFE.UseStyleColors = true;
            this.edtEmailDANFE.TextChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.Location = new System.Drawing.Point(3, 361);
            this.metroLabel1.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(38, 15);
            this.metroLabel1.TabIndex = 15;
            this.metroLabel1.Text = "E-mail";
            // 
            // chkAddEmailDANFE
            // 
            this.chkAddEmailDANFE.AutoSize = true;
            this.chkAddEmailDANFE.Location = new System.Drawing.Point(305, 380);
            this.chkAddEmailDANFE.Name = "chkAddEmailDANFE";
            this.chkAddEmailDANFE.Size = new System.Drawing.Size(256, 15);
            this.chkAddEmailDANFE.TabIndex = 17;
            this.chkAddEmailDANFE.Text = "Adiciona este e-mail ao que constar no XML";
            this.chkAddEmailDANFE.UseSelectable = true;
            this.chkAddEmailDANFE.CheckedChanged += new System.EventHandler(this.tbPastaExeUniDanfe_TextChanged);
            // 
            // userConfiguracao_danfe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.chkAddEmailDANFE);
            this.Controls.Add(this.edtEmailDANFE);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.cbDanfeMonProcNfe);
            this.Controls.Add(this.cbDanfeMonNfe);
            this.Controls.Add(this.metroLabel37);
            this.Controls.Add(this.metroLabel36);
            this.Controls.Add(this.tbConfiguracaoCCe);
            this.Controls.Add(this.metroLabel35);
            this.Controls.Add(this.tbConfiguracaoDanfe);
            this.Controls.Add(this.metroLabel34);
            this.Controls.Add(this.metroLabel33);
            this.Controls.Add(this.tbPastaXmlParaDanfeMon);
            this.Controls.Add(this.metroLabel32);
            this.Controls.Add(this.tbPastaConfigUniDanfe);
            this.Controls.Add(this.metroLabel31);
            this.Controls.Add(this.tbPastaExeUniDanfe);
            this.Controls.Add(this.tbTextoDANFE);
            this.Name = "userConfiguracao_danfe";
            this.Size = new System.Drawing.Size(675, 411);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel37;
        private MetroFramework.Controls.MetroLabel metroLabel36;
        private MetroFramework.Controls.MetroLabel metroLabel35;
        private MetroFramework.Controls.MetroLabel metroLabel34;
        private MetroFramework.Controls.MetroLabel metroLabel33;
        private MetroFramework.Controls.MetroLabel metroLabel32;
        private MetroFramework.Controls.MetroLabel metroLabel31;
        private MetroFramework.Controls.MetroTextBox tbTextoDANFE;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public MetroFramework.Controls.MetroCheckBox cbDanfeMonProcNfe;
        public MetroFramework.Controls.MetroCheckBox cbDanfeMonNfe;
        public MetroFramework.Controls.MetroTextBox tbConfiguracaoCCe;
        public MetroFramework.Controls.MetroTextBox tbConfiguracaoDanfe;
        public MetroFramework.Controls.MetroTextBox tbPastaXmlParaDanfeMon;
        public MetroFramework.Controls.MetroTextBox tbPastaConfigUniDanfe;
        public MetroFramework.Controls.MetroTextBox tbPastaExeUniDanfe;
        public MetroFramework.Controls.MetroTextBox edtEmailDANFE;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        public MetroFramework.Controls.MetroCheckBox chkAddEmailDANFE;
    }
}

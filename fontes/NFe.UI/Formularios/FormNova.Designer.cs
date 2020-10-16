namespace NFe.UI.Formularios
{
    partial class FormNova
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCNPJ = new MetroFramework.Controls.MetroLabel();
            this.edtCNPJ = new MetroFramework.Controls.MetroTextBox();
            this.lblNome = new MetroFramework.Controls.MetroLabel();
            this.edtNome = new MetroFramework.Controls.MetroTextBox();
            this.cbServico = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel19 = new MetroFramework.Controls.MetroLabel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.lblNumero = new MetroFramework.Controls.MetroLabel();
            this.comboDocumento = new MetroFramework.Controls.MetroComboBox();
            this.metroPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCNPJ
            // 
            this.lblCNPJ.AutoSize = true;
            this.lblCNPJ.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblCNPJ.Location = new System.Drawing.Point(11, 15);
            this.lblCNPJ.Margin = new System.Windows.Forms.Padding(3);
            this.lblCNPJ.Name = "lblCNPJ";
            this.lblCNPJ.Size = new System.Drawing.Size(66, 15);
            this.lblCNPJ.TabIndex = 0;
            this.lblCNPJ.Text = "Documento";
            // 
            // edtCNPJ
            // 
            this.edtCNPJ.Lines = new string[] {
        "Normal Textbox"};
            this.edtCNPJ.Location = new System.Drawing.Point(150, 33);
            this.edtCNPJ.MaxLength = 18;
            this.edtCNPJ.Name = "edtCNPJ";
            this.edtCNPJ.PasswordChar = '\0';
            this.edtCNPJ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtCNPJ.SelectedText = "";
            this.edtCNPJ.Size = new System.Drawing.Size(238, 22);
            this.edtCNPJ.TabIndex = 3;
            this.edtCNPJ.Text = "Normal Textbox";
            this.edtCNPJ.UseSelectable = true;
            this.edtCNPJ.TextChanged += new System.EventHandler(this.edtCNPJ_TextChanged);
            this.edtCNPJ.Enter += new System.EventHandler(this.edtCNPJ_Enter);
            this.edtCNPJ.Leave += new System.EventHandler(this.edtCNPJ_Leave);
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblNome.Location = new System.Drawing.Point(11, 65);
            this.lblNome.Margin = new System.Windows.Forms.Padding(3);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(39, 15);
            this.lblNome.TabIndex = 4;
            this.lblNome.Text = "Nome";
            // 
            // edtNome
            // 
            this.edtNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtNome.Lines = new string[] {
        "Normal Textbox"};
            this.edtNome.Location = new System.Drawing.Point(11, 84);
            this.edtNome.MaxLength = 100;
            this.edtNome.Name = "edtNome";
            this.edtNome.PasswordChar = '\0';
            this.edtNome.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtNome.SelectedText = "";
            this.edtNome.Size = new System.Drawing.Size(514, 22);
            this.edtNome.TabIndex = 5;
            this.edtNome.Text = "Normal Textbox";
            this.edtNome.UseSelectable = true;
            this.edtNome.TextChanged += new System.EventHandler(this.edtCNPJ_TextChanged);
            // 
            // cbServico
            // 
            this.cbServico.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbServico.FormattingEnabled = true;
            this.cbServico.IntegralHeight = false;
            this.cbServico.ItemHeight = 19;
            this.cbServico.Location = new System.Drawing.Point(11, 130);
            this.cbServico.MaxDropDownItems = 15;
            this.cbServico.Name = "cbServico";
            this.cbServico.Size = new System.Drawing.Size(514, 25);
            this.cbServico.TabIndex = 7;
            this.cbServico.UseSelectable = true;
            // 
            // metroLabel19
            // 
            this.metroLabel19.AutoSize = true;
            this.metroLabel19.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel19.Location = new System.Drawing.Point(11, 112);
            this.metroLabel19.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel19.Name = "metroLabel19";
            this.metroLabel19.Size = new System.Drawing.Size(43, 15);
            this.metroLabel19.TabIndex = 6;
            this.metroLabel19.Text = "Serviço";
            // 
            // metroButton1
            // 
            this.metroButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton1.Location = new System.Drawing.Point(398, 246);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(80, 25);
            this.metroButton1.TabIndex = 1;
            this.metroButton1.Text = "&Ok";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroButton2
            // 
            this.metroButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.metroButton2.Location = new System.Drawing.Point(484, 246);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(80, 25);
            this.metroButton2.TabIndex = 2;
            this.metroButton2.Text = "&Cancelar";
            this.metroButton2.UseSelectable = true;
            // 
            // metroPanel2
            // 
            this.metroPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel2.BackColor = System.Drawing.Color.Transparent;
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.lblNumero);
            this.metroPanel2.Controls.Add(this.comboDocumento);
            this.metroPanel2.Controls.Add(this.lblCNPJ);
            this.metroPanel2.Controls.Add(this.cbServico);
            this.metroPanel2.Controls.Add(this.edtCNPJ);
            this.metroPanel2.Controls.Add(this.metroLabel19);
            this.metroPanel2.Controls.Add(this.edtNome);
            this.metroPanel2.Controls.Add(this.lblNome);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(23, 59);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(541, 181);
            this.metroPanel2.TabIndex = 0;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // lblNumero
            // 
            this.lblNumero.AutoSize = true;
            this.lblNumero.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblNumero.Location = new System.Drawing.Point(151, 17);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(49, 15);
            this.lblNumero.TabIndex = 2;
            this.lblNumero.Text = "Número";
            // 
            // comboDocumento
            // 
            this.comboDocumento.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboDocumento.FormattingEnabled = true;
            this.comboDocumento.ItemHeight = 19;
            this.comboDocumento.Items.AddRange(new object[] {
            "CPF",
            "CEI",
            "CAEPF",
            "CNPJ"});
            this.comboDocumento.Location = new System.Drawing.Point(11, 31);
            this.comboDocumento.Name = "comboDocumento";
            this.comboDocumento.Size = new System.Drawing.Size(121, 25);
            this.comboDocumento.TabIndex = 1;
            this.comboDocumento.UseSelectable = true;
            this.comboDocumento.SelectedValueChanged += new System.EventHandler(this.comboDocumento_SelectedValueChanged);
            // 
            // FormNova
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.metroButton2;
            this.ClientSize = new System.Drawing.Size(587, 285);
            this.ControlBox = false;
            this.Controls.Add(this.metroPanel2);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNova";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "FormNova";
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLabel lblCNPJ;
        private MetroFramework.Controls.MetroLabel lblNome;
        private MetroFramework.Controls.MetroLabel metroLabel19;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        public MetroFramework.Controls.MetroTextBox edtCNPJ;
        public MetroFramework.Controls.MetroTextBox edtNome;
        public MetroFramework.Controls.MetroComboBox cbServico;
        public MetroFramework.Controls.MetroComboBox comboDocumento;
        private MetroFramework.Controls.MetroLabel lblNumero;
    }
}
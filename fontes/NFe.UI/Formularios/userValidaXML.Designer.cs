namespace NFe.UI
{
    partial class userValidaXML
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.edtFilename = new MetroFramework.Controls.MetroTextBox();
            this.btn_Validar = new MetroFramework.Controls.MetroButton();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.edtTipoarquivo = new MetroFramework.Controls.MetroTextBox();
            this.textBox_resultado = new MetroFramework.Controls.MetroTextBox();
            this.cbEmpresas = new MetroFramework.Controls.MetroComboBox();
            this.metroTabControl = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabControl.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Size = new System.Drawing.Size(101, 25);
            this.labelTitle.Text = "Validar XML";
            // 
            // metroLabel17
            // 
            this.metroLabel17.AutoSize = true;
            this.metroLabel17.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel17.Location = new System.Drawing.Point(0, 58);
            this.metroLabel17.Name = "metroLabel17";
            this.metroLabel17.Size = new System.Drawing.Size(328, 15);
            this.metroLabel17.TabIndex = 0;
            this.metroLabel17.Text = "Selecione a empresa do certificado a ser utilizado para validação";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.Location = new System.Drawing.Point(0, 113);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(143, 15);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Arquivo XML a ser validado";
            // 
            // edtFilename
            // 
            this.edtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFilename.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.edtFilename.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.edtFilename.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.edtFilename.IconRight = true;
            this.edtFilename.Lines = new string[] {
        "Styled Textbox"};
            this.edtFilename.Location = new System.Drawing.Point(0, 135);
            this.edtFilename.MaxLength = 32767;
            this.edtFilename.Name = "edtFilename";
            this.edtFilename.PasswordChar = '\0';
            this.edtFilename.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtFilename.SelectedText = "";
            this.edtFilename.Size = new System.Drawing.Size(527, 22);
            this.edtFilename.TabIndex = 3;
            this.edtFilename.Text = "Styled Textbox";
            this.edtFilename.UseSelectable = true;
            this.edtFilename.TextChanged += new System.EventHandler(this.textBox_arqxml_TextChanged);
            this.edtFilename.MouseDown += new System.Windows.Forms.MouseEventHandler(this.edtFilename_MouseDown);
            // 
            // btn_Validar
            // 
            this.btn_Validar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Validar.Location = new System.Drawing.Point(533, 135);
            this.btn_Validar.Name = "btn_Validar";
            this.btn_Validar.Size = new System.Drawing.Size(62, 22);
            this.btn_Validar.TabIndex = 4;
            this.btn_Validar.Text = "Validar";
            this.btn_Validar.UseSelectable = true;
            this.btn_Validar.Click += new System.EventHandler(this.btn_Validar_Click);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel2.Location = new System.Drawing.Point(0, 160);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(85, 15);
            this.metroLabel2.TabIndex = 5;
            this.metroLabel2.Text = "Tipo de arquivo";
            // 
            // edtTipoarquivo
            // 
            this.edtTipoarquivo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtTipoarquivo.Lines = new string[] {
        "Disabled Textbox"};
            this.edtTipoarquivo.Location = new System.Drawing.Point(0, 182);
            this.edtTipoarquivo.MaxLength = 32767;
            this.edtTipoarquivo.Name = "edtTipoarquivo";
            this.edtTipoarquivo.PasswordChar = '\0';
            this.edtTipoarquivo.ReadOnly = true;
            this.edtTipoarquivo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtTipoarquivo.SelectedText = "";
            this.edtTipoarquivo.Size = new System.Drawing.Size(527, 22);
            this.edtTipoarquivo.TabIndex = 6;
            this.edtTipoarquivo.Text = "Disabled Textbox";
            this.edtTipoarquivo.UseSelectable = true;
            // 
            // textBox_resultado
            // 
            this.textBox_resultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_resultado.Lines = new string[] {
        "Multiline Textbox"};
            this.textBox_resultado.Location = new System.Drawing.Point(5, 5);
            this.textBox_resultado.MaxLength = 32767;
            this.textBox_resultado.Multiline = true;
            this.textBox_resultado.Name = "textBox_resultado";
            this.textBox_resultado.PasswordChar = '\0';
            this.textBox_resultado.ReadOnly = true;
            this.textBox_resultado.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_resultado.SelectedText = "";
            this.textBox_resultado.Size = new System.Drawing.Size(509, 145);
            this.textBox_resultado.TabIndex = 8;
            this.textBox_resultado.Text = "Multiline Textbox";
            this.textBox_resultado.UseSelectable = true;
            // 
            // cbEmpresas
            // 
            this.cbEmpresas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEmpresas.DisplayMember = "Nome";
            this.cbEmpresas.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbEmpresas.FormattingEnabled = true;
            this.cbEmpresas.IntegralHeight = false;
            this.cbEmpresas.ItemHeight = 19;
            this.cbEmpresas.Items.AddRange(new object[] {
            "Normal Combobox",
            "Item 2",
            "Item 3",
            "Item 4"});
            this.cbEmpresas.Location = new System.Drawing.Point(0, 80);
            this.cbEmpresas.MaxDropDownItems = 15;
            this.cbEmpresas.Name = "cbEmpresas";
            this.cbEmpresas.Size = new System.Drawing.Size(527, 25);
            this.cbEmpresas.TabIndex = 1;
            this.cbEmpresas.UseSelectable = true;
            this.cbEmpresas.ValueMember = "CNPJ";
            this.cbEmpresas.SelectedIndexChanged += new System.EventHandler(this.cbEmpresas_SelectedIndexChanged);
            // 
            // metroTabControl
            // 
            this.metroTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTabControl.Controls.Add(this.metroTabPage1);
            this.metroTabControl.Controls.Add(this.metroTabPage2);
            this.metroTabControl.Location = new System.Drawing.Point(0, 212);
            this.metroTabControl.Name = "metroTabControl";
            this.metroTabControl.SelectedIndex = 0;
            this.metroTabControl.Size = new System.Drawing.Size(527, 197);
            this.metroTabControl.TabIndex = 13;
            this.metroTabControl.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.AutoScroll = true;
            this.metroTabPage1.Controls.Add(this.textBox_resultado);
            this.metroTabPage1.HorizontalScrollbar = true;
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Padding = new System.Windows.Forms.Padding(5);
            this.metroTabPage1.Size = new System.Drawing.Size(519, 155);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Resultado da validação";
            this.metroTabPage1.VerticalScrollbar = true;
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 10;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Padding = new System.Windows.Forms.Padding(5);
            this.metroTabPage2.Size = new System.Drawing.Size(519, 155);
            this.metroTabPage2.TabIndex = 6;
            this.metroTabPage2.Text = "XML";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 10;
            // 
            // userValidaXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.metroTabControl);
            this.Controls.Add(this.cbEmpresas);
            this.Controls.Add(this.edtTipoarquivo);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.btn_Validar);
            this.Controls.Add(this.edtFilename);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.metroLabel17);
            this.Name = "userValidaXML";
            this.Size = new System.Drawing.Size(602, 411);
            this.Controls.SetChildIndex(this.labelTitle, 0);
            this.Controls.SetChildIndex(this.metroLabel17, 0);
            this.Controls.SetChildIndex(this.metroLabel1, 0);
            this.Controls.SetChildIndex(this.edtFilename, 0);
            this.Controls.SetChildIndex(this.btn_Validar, 0);
            this.Controls.SetChildIndex(this.metroLabel2, 0);
            this.Controls.SetChildIndex(this.edtTipoarquivo, 0);
            this.Controls.SetChildIndex(this.cbEmpresas, 0);
            this.Controls.SetChildIndex(this.metroTabControl, 0);
            this.metroTabControl.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel17;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox edtFilename;
        private MetroFramework.Controls.MetroButton btn_Validar;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox edtTipoarquivo;
        private MetroFramework.Controls.MetroTextBox textBox_resultado;
        private MetroFramework.Controls.MetroComboBox cbEmpresas;
        private MetroFramework.Controls.MetroTabControl metroTabControl;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
    }
}

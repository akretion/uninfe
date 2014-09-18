namespace NFe.UI.Formularios.NFSe
{
    partial class FormMunicipio
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
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.edtPadrao = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.lblCNPJ = new MetroFramework.Controls.MetroLabel();
            this.edtUF = new MetroFramework.Controls.MetroComboBox();
            this.edtCodMun = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel19 = new MetroFramework.Controls.MetroLabel();
            this.edtMunicipio = new MetroFramework.Controls.MetroTextBox();
            this.lblNome = new MetroFramework.Controls.MetroLabel();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroPanel2
            // 
            this.metroPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel2.BackColor = System.Drawing.Color.Transparent;
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.edtPadrao);
            this.metroPanel2.Controls.Add(this.metroLabel1);
            this.metroPanel2.Controls.Add(this.lblCNPJ);
            this.metroPanel2.Controls.Add(this.edtUF);
            this.metroPanel2.Controls.Add(this.edtCodMun);
            this.metroPanel2.Controls.Add(this.metroLabel19);
            this.metroPanel2.Controls.Add(this.edtMunicipio);
            this.metroPanel2.Controls.Add(this.lblNome);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(23, 63);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(614, 224);
            this.metroPanel2.TabIndex = 0;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // edtPadrao
            // 
            this.edtPadrao.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.edtPadrao.FormattingEnabled = true;
            this.edtPadrao.IntegralHeight = false;
            this.edtPadrao.ItemHeight = 23;
            this.edtPadrao.Location = new System.Drawing.Point(11, 183);
            this.edtPadrao.MaxDropDownItems = 15;
            this.edtPadrao.Name = "edtPadrao";
            this.edtPadrao.Size = new System.Drawing.Size(174, 29);
            this.edtPadrao.TabIndex = 7;
            this.edtPadrao.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.Location = new System.Drawing.Point(11, 165);
            this.metroLabel1.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(44, 15);
            this.metroLabel1.TabIndex = 6;
            this.metroLabel1.Text = "Padrão";
            // 
            // lblCNPJ
            // 
            this.lblCNPJ.AutoSize = true;
            this.lblCNPJ.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblCNPJ.Location = new System.Drawing.Point(11, 15);
            this.lblCNPJ.Margin = new System.Windows.Forms.Padding(3);
            this.lblCNPJ.Name = "lblCNPJ";
            this.lblCNPJ.Size = new System.Drawing.Size(71, 15);
            this.lblCNPJ.TabIndex = 0;
            this.lblCNPJ.Text = "Código IBGE";
            // 
            // edtUF
            // 
            this.edtUF.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.edtUF.FormattingEnabled = true;
            this.edtUF.IntegralHeight = false;
            this.edtUF.ItemHeight = 19;
            this.edtUF.Location = new System.Drawing.Point(11, 130);
            this.edtUF.MaxDropDownItems = 15;
            this.edtUF.Name = "edtUF";
            this.edtUF.Size = new System.Drawing.Size(102, 25);
            this.edtUF.TabIndex = 5;
            this.edtUF.UseSelectable = true;
            // 
            // edtCodMun
            // 
            this.edtCodMun.Lines = new string[] {
        "Normal Textbox"};
            this.edtCodMun.Location = new System.Drawing.Point(11, 33);
            this.edtCodMun.MaxLength = 7;
            this.edtCodMun.Name = "edtCodMun";
            this.edtCodMun.PasswordChar = '\0';
            this.edtCodMun.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtCodMun.SelectedText = "";
            this.edtCodMun.Size = new System.Drawing.Size(102, 22);
            this.edtCodMun.TabIndex = 1;
            this.edtCodMun.Text = "Normal Textbox";
            this.edtCodMun.UseSelectable = true;
            this.edtCodMun.TextChanged += new System.EventHandler(this.edtMunicipio_TextChanged);
            this.edtCodMun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edtCodMun_KeyPress);
            this.edtCodMun.Leave += new System.EventHandler(this.edtCodMun_Leave);
            // 
            // metroLabel19
            // 
            this.metroLabel19.AutoSize = true;
            this.metroLabel19.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel19.Location = new System.Drawing.Point(11, 112);
            this.metroLabel19.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel19.Name = "metroLabel19";
            this.metroLabel19.Size = new System.Drawing.Size(21, 15);
            this.metroLabel19.TabIndex = 4;
            this.metroLabel19.Text = "UF";
            // 
            // edtMunicipio
            // 
            this.edtMunicipio.Lines = new string[] {
        "Normal Textbox"};
            this.edtMunicipio.Location = new System.Drawing.Point(11, 84);
            this.edtMunicipio.MaxLength = 32767;
            this.edtMunicipio.Name = "edtMunicipio";
            this.edtMunicipio.PasswordChar = '\0';
            this.edtMunicipio.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtMunicipio.SelectedText = "";
            this.edtMunicipio.Size = new System.Drawing.Size(587, 22);
            this.edtMunicipio.TabIndex = 3;
            this.edtMunicipio.Text = "Normal Textbox";
            this.edtMunicipio.UseSelectable = true;
            this.edtMunicipio.TextChanged += new System.EventHandler(this.edtMunicipio_TextChanged);
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblNome.Location = new System.Drawing.Point(11, 65);
            this.lblNome.Margin = new System.Windows.Forms.Padding(3);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(93, 15);
            this.lblNome.TabIndex = 2;
            this.lblNome.Text = "Município/Cidade";
            // 
            // metroButton2
            // 
            this.metroButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.metroButton2.Location = new System.Drawing.Point(557, 293);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(80, 25);
            this.metroButton2.TabIndex = 2;
            this.metroButton2.Text = "&Cancelar";
            this.metroButton2.UseSelectable = true;
            // 
            // metroButton1
            // 
            this.metroButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton1.Location = new System.Drawing.Point(472, 293);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(80, 25);
            this.metroButton1.TabIndex = 1;
            this.metroButton1.Text = "&Ok";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // FormMunicipio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 330);
            this.Controls.Add(this.metroPanel2);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMunicipio";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Novo municipio";
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroLabel lblCNPJ;
        public MetroFramework.Controls.MetroComboBox edtUF;
        private MetroFramework.Controls.MetroLabel metroLabel19;
        private MetroFramework.Controls.MetroLabel lblNome;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroButton metroButton1;
        public MetroFramework.Controls.MetroComboBox edtPadrao;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox edtCodMun;
        private MetroFramework.Controls.MetroTextBox edtMunicipio;
    }
}
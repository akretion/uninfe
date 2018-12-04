namespace NFe.UI
{
    partial class userConfiguracoes
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
            this.tc_main = new MetroFramework.Controls.MetroTabControl();
            this.tpGeral = new MetroFramework.Controls.MetroTabPage();
            this.tpEmpresas = new MetroFramework.Controls.MetroTabPage();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cbEmpresas = new MetroFramework.Controls.MetroComboBox();
            this.btnExcluir = new MetroFramework.Controls.MetroButton();
            this.tc_empresa = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.btnNova = new MetroFramework.Controls.MetroButton();
            this.tc_main.SuspendLayout();
            this.tpEmpresas.SuspendLayout();
            this.tc_empresa.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTitle.Size = new System.Drawing.Size(121, 25);
            this.labelTitle.Text = "Configurações";
            // 
            // tc_main
            // 
            this.tc_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_main.Controls.Add(this.tpGeral);
            this.tc_main.Controls.Add(this.tpEmpresas);
            this.tc_main.Location = new System.Drawing.Point(0, 47);
            this.tc_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tc_main.Name = "tc_main";
            this.tc_main.SelectedIndex = 1;
            this.tc_main.Size = new System.Drawing.Size(566, 340);
            this.tc_main.TabIndex = 13;
            this.tc_main.UseSelectable = true;
            // 
            // tpGeral
            // 
            this.tpGeral.HorizontalScrollbar = true;
            this.tpGeral.HorizontalScrollbarBarColor = true;
            this.tpGeral.HorizontalScrollbarHighlightOnWheel = false;
            this.tpGeral.HorizontalScrollbarSize = 8;
            this.tpGeral.Location = new System.Drawing.Point(4, 38);
            this.tpGeral.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tpGeral.Name = "tpGeral";
            this.tpGeral.Size = new System.Drawing.Size(558, 298);
            this.tpGeral.TabIndex = 0;
            this.tpGeral.Text = "Geral";
            this.tpGeral.VerticalScrollbar = true;
            this.tpGeral.VerticalScrollbarBarColor = true;
            this.tpGeral.VerticalScrollbarHighlightOnWheel = false;
            this.tpGeral.VerticalScrollbarSize = 8;
            // 
            // tpEmpresas
            // 
            this.tpEmpresas.BackColor = System.Drawing.Color.Transparent;
            this.tpEmpresas.Controls.Add(this.metroLabel2);
            this.tpEmpresas.Controls.Add(this.cbEmpresas);
            this.tpEmpresas.Controls.Add(this.btnExcluir);
            this.tpEmpresas.Controls.Add(this.tc_empresa);
            this.tpEmpresas.Controls.Add(this.btnNova);
            this.tpEmpresas.HorizontalScrollbar = true;
            this.tpEmpresas.HorizontalScrollbarBarColor = true;
            this.tpEmpresas.HorizontalScrollbarHighlightOnWheel = false;
            this.tpEmpresas.HorizontalScrollbarSize = 8;
            this.tpEmpresas.Location = new System.Drawing.Point(4, 38);
            this.tpEmpresas.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tpEmpresas.Name = "tpEmpresas";
            this.tpEmpresas.Size = new System.Drawing.Size(558, 298);
            this.tpEmpresas.TabIndex = 3;
            this.tpEmpresas.Text = "Empresas";
            this.tpEmpresas.VerticalScrollbar = true;
            this.tpEmpresas.VerticalScrollbarBarColor = true;
            this.tpEmpresas.VerticalScrollbarHighlightOnWheel = false;
            this.tpEmpresas.VerticalScrollbarSize = 8;
            this.tpEmpresas.Visible = false;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(0, 2);
            this.metroLabel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(65, 19);
            this.metroLabel2.TabIndex = 2;
            this.metroLabel2.Text = "Empresas";
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
            this.cbEmpresas.Location = new System.Drawing.Point(2, 23);
            this.cbEmpresas.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbEmpresas.MaxDropDownItems = 15;
            this.cbEmpresas.Name = "cbEmpresas";
            this.cbEmpresas.Size = new System.Drawing.Size(406, 25);
            this.cbEmpresas.TabIndex = 15;
            this.cbEmpresas.UseSelectable = true;
            this.cbEmpresas.ValueMember = "CNPJ";
            this.cbEmpresas.SelectedIndexChanged += new System.EventHandler(this.cbEmpresas_SelectedIndexChanged);
            // 
            // btnExcluir
            // 
            this.btnExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcluir.Location = new System.Drawing.Point(485, 23);
            this.btnExcluir.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(70, 23);
            this.btnExcluir.TabIndex = 16;
            this.btnExcluir.Text = "Excluir";
            this.btnExcluir.UseSelectable = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // tc_empresa
            // 
            this.tc_empresa.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_empresa.Controls.Add(this.metroTabPage1);
            this.tc_empresa.Location = new System.Drawing.Point(2, 52);
            this.tc_empresa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tc_empresa.Name = "tc_empresa";
            this.tc_empresa.SelectedIndex = 0;
            this.tc_empresa.ShowToolTips = true;
            this.tc_empresa.Size = new System.Drawing.Size(560, 258);
            this.tc_empresa.TabIndex = 14;
            this.tc_empresa.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 8;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(552, 216);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "metroTabPage1";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 8;
            // 
            // btnNova
            // 
            this.btnNova.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNova.Location = new System.Drawing.Point(412, 23);
            this.btnNova.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNova.Name = "btnNova";
            this.btnNova.Size = new System.Drawing.Size(70, 23);
            this.btnNova.TabIndex = 5;
            this.btnNova.Text = "Nova";
            this.btnNova.UseSelectable = true;
            this.btnNova.Click += new System.EventHandler(this.btnNova_Click);
            // 
            // userConfiguracoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.tc_main);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "userConfiguracoes";
            this.Size = new System.Drawing.Size(568, 387);
            this.Resize += new System.EventHandler(this.userConfiguracoes_Resize);
            this.Controls.SetChildIndex(this.labelTitle, 0);
            this.Controls.SetChildIndex(this.tc_main, 0);
            this.tc_main.ResumeLayout(false);
            this.tpEmpresas.ResumeLayout(false);
            this.tpEmpresas.PerformLayout();
            this.tc_empresa.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl tc_main;
        private MetroFramework.Controls.MetroTabPage tpGeral;
        private MetroFramework.Controls.MetroTabPage tpEmpresas;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTabControl tc_empresa;
        private MetroFramework.Controls.MetroButton btnNova;
        private MetroFramework.Controls.MetroComboBox cbEmpresas;
        private MetroFramework.Controls.MetroButton btnExcluir;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;

    }
}

namespace NFe.Interface
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblServico = new System.Windows.Forms.Label();
            this.cbServico = new System.Windows.Forms.ComboBox();
            this.edtCNPJ = new System.Windows.Forms.MaskedTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblServico);
            this.panel1.Controls.Add(this.cbServico);
            this.panel1.Controls.Add(this.edtCNPJ);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(477, 71);
            this.panel1.TabIndex = 0;
            // 
            // lblServico
            // 
            this.lblServico.AutoSize = true;
            this.lblServico.Location = new System.Drawing.Point(231, 15);
            this.lblServico.Name = "lblServico";
            this.lblServico.Size = new System.Drawing.Size(43, 13);
            this.lblServico.TabIndex = 33;
            this.lblServico.Text = "Serviço\r\n";
            // 
            // cbServico
            // 
            this.cbServico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServico.FormattingEnabled = true;
            this.cbServico.ItemHeight = 13;
            this.cbServico.Location = new System.Drawing.Point(234, 31);
            this.cbServico.Name = "cbServico";
            this.cbServico.Size = new System.Drawing.Size(173, 21);
            this.cbServico.TabIndex = 32;
            // 
            // edtCNPJ
            // 
            this.edtCNPJ.Location = new System.Drawing.Point(54, 32);
            this.edtCNPJ.Mask = "00,000,000/0000-00";
            this.edtCNPJ.Name = "edtCNPJ";
            this.edtCNPJ.Size = new System.Drawing.Size(128, 20);
            this.edtCNPJ.TabIndex = 31;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(54, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 13);
            this.label19.TabIndex = 30;
            this.label19.Text = "CNPJ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(333, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(414, 89);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "&Fechar";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // FormNova
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(501, 121);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNova";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nova empresa";
            this.Load += new System.EventHandler(this.FormNova_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblServico;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.ComboBox cbServico;
        public System.Windows.Forms.MaskedTextBox edtCNPJ;
    }
}
namespace uninfe.Formulario
{
    partial class FormConsultaCadastro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConsultaCadastro));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboUf = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonPesquisaIE = new System.Windows.Forms.Button();
            this.buttonPesquisaCPF = new System.Windows.Forms.Button();
            this.buttonPesquisaCNPJ = new System.Windows.Forms.Button();
            this.textIE = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textCpf = new System.Windows.Forms.TextBox();
            this.textCnpj = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStatusServidor = new System.Windows.Forms.Button();
            this.textResultado = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboUf);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.buttonPesquisaIE);
            this.groupBox1.Controls.Add(this.buttonPesquisaCPF);
            this.groupBox1.Controls.Add(this.buttonPesquisaCNPJ);
            this.groupBox1.Controls.Add(this.textIE);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textCpf);
            this.groupBox1.Controls.Add(this.textCnpj);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consulta de Cadastro de Contribuínte";
            // 
            // comboUf
            // 
            this.comboUf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUf.FormattingEnabled = true;
            this.comboUf.Location = new System.Drawing.Point(48, 16);
            this.comboUf.Name = "comboUf";
            this.comboUf.Size = new System.Drawing.Size(121, 21);
            this.comboUf.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 954;
            this.label4.Text = "UF";
            // 
            // buttonPesquisaIE
            // 
            this.buttonPesquisaIE.Location = new System.Drawing.Point(242, 101);
            this.buttonPesquisaIE.Name = "buttonPesquisaIE";
            this.buttonPesquisaIE.Size = new System.Drawing.Size(75, 23);
            this.buttonPesquisaIE.TabIndex = 7;
            this.buttonPesquisaIE.Text = "Pesquisar";
            this.buttonPesquisaIE.UseVisualStyleBackColor = true;
            this.buttonPesquisaIE.Click += new System.EventHandler(this.buttonPesquisaIE_Click);
            // 
            // buttonPesquisaCPF
            // 
            this.buttonPesquisaCPF.Location = new System.Drawing.Point(242, 72);
            this.buttonPesquisaCPF.Name = "buttonPesquisaCPF";
            this.buttonPesquisaCPF.Size = new System.Drawing.Size(75, 23);
            this.buttonPesquisaCPF.TabIndex = 5;
            this.buttonPesquisaCPF.Text = "Pesquisar";
            this.buttonPesquisaCPF.UseVisualStyleBackColor = true;
            this.buttonPesquisaCPF.Click += new System.EventHandler(this.buttonPesquisaCPF_Click);
            // 
            // buttonPesquisaCNPJ
            // 
            this.buttonPesquisaCNPJ.Location = new System.Drawing.Point(242, 43);
            this.buttonPesquisaCNPJ.Name = "buttonPesquisaCNPJ";
            this.buttonPesquisaCNPJ.Size = new System.Drawing.Size(75, 23);
            this.buttonPesquisaCNPJ.TabIndex = 3;
            this.buttonPesquisaCNPJ.Text = "Pesquisar";
            this.buttonPesquisaCNPJ.UseVisualStyleBackColor = true;
            this.buttonPesquisaCNPJ.Click += new System.EventHandler(this.buttonPesquisaCNPJ_Click);
            // 
            // textIE
            // 
            this.textIE.Location = new System.Drawing.Point(48, 102);
            this.textIE.Name = "textIE";
            this.textIE.Size = new System.Drawing.Size(188, 20);
            this.textIE.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 88;
            this.label3.Text = "IE";
            // 
            // textCpf
            // 
            this.textCpf.Location = new System.Drawing.Point(48, 73);
            this.textCpf.Name = "textCpf";
            this.textCpf.Size = new System.Drawing.Size(188, 20);
            this.textCpf.TabIndex = 4;
            // 
            // textCnpj
            // 
            this.textCnpj.Location = new System.Drawing.Point(48, 44);
            this.textCnpj.Name = "textCnpj";
            this.textCnpj.Size = new System.Drawing.Size(188, 20);
            this.textCnpj.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1654;
            this.label2.Text = "CPF";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 65;
            this.label1.Text = "CNPJ";
            // 
            // buttonStatusServidor
            // 
            this.buttonStatusServidor.Location = new System.Drawing.Point(79, 158);
            this.buttonStatusServidor.Name = "buttonStatusServidor";
            this.buttonStatusServidor.Size = new System.Drawing.Size(195, 23);
            this.buttonStatusServidor.TabIndex = 8;
            this.buttonStatusServidor.Text = "Consutar Status do Servidor SEFAZ";
            this.buttonStatusServidor.UseVisualStyleBackColor = true;
            this.buttonStatusServidor.Click += new System.EventHandler(this.buttonStatusServidor_Click);
            // 
            // textResultado
            // 
            this.textResultado.Location = new System.Drawing.Point(12, 187);
            this.textResultado.Multiline = true;
            this.textResultado.Name = "textResultado";
            this.textResultado.ReadOnly = true;
            this.textResultado.Size = new System.Drawing.Size(335, 133);
            this.textResultado.TabIndex = 9;
            // 
            // FormConsultaCadastro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 327);
            this.Controls.Add(this.textResultado);
            this.Controls.Add(this.buttonStatusServidor);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormConsultaCadastro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta ao Servidor";
            this.Load += new System.EventHandler(this.FormConsultaCadastro_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonPesquisaCPF;
        private System.Windows.Forms.Button buttonPesquisaCNPJ;
        private System.Windows.Forms.TextBox textIE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textCpf;
        private System.Windows.Forms.TextBox textCnpj;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonPesquisaIE;
        private System.Windows.Forms.Button buttonStatusServidor;
        private System.Windows.Forms.TextBox textResultado;
        private System.Windows.Forms.ComboBox comboUf;
        private System.Windows.Forms.Label label4;

    }
}
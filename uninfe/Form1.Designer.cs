namespace uninfe
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_status_servico = new System.Windows.Forms.Button();
            this.button_selecionar_certificado = new System.Windows.Forms.Button();
            this.button_exibir_certificado_selecionado = new System.Windows.Forms.Button();
            this.comboBoxAmbiente = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_assinarxml = new System.Windows.Forms.Button();
            this.button_recepcao_nfe = new System.Windows.Forms.Button();
            this.textBox_xmldados = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_seleciona_xmldados = new System.Windows.Forms.Button();
            this.textBox_xmlretorno = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_RetRecepcao = new System.Windows.Forms.Button();
            this.comboBoxUF = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_status_servico
            // 
            resources.ApplyResources(this.button_status_servico, "button_status_servico");
            this.button_status_servico.Name = "button_status_servico";
            this.button_status_servico.UseVisualStyleBackColor = true;
            this.button_status_servico.Click += new System.EventHandler(this.button_status_servico_Click);
            // 
            // button_selecionar_certificado
            // 
            resources.ApplyResources(this.button_selecionar_certificado, "button_selecionar_certificado");
            this.button_selecionar_certificado.Name = "button_selecionar_certificado";
            this.button_selecionar_certificado.UseVisualStyleBackColor = true;
            this.button_selecionar_certificado.Click += new System.EventHandler(this.button_selecionar_certificado_Click);
            // 
            // button_exibir_certificado_selecionado
            // 
            resources.ApplyResources(this.button_exibir_certificado_selecionado, "button_exibir_certificado_selecionado");
            this.button_exibir_certificado_selecionado.Name = "button_exibir_certificado_selecionado";
            this.button_exibir_certificado_selecionado.UseVisualStyleBackColor = true;
            this.button_exibir_certificado_selecionado.Click += new System.EventHandler(this.button_exibir_certificado_selecionado_Click);
            // 
            // comboBoxAmbiente
            // 
            this.comboBoxAmbiente.DisplayMember = "Homologação";
            this.comboBoxAmbiente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAmbiente.FormattingEnabled = true;
            this.comboBoxAmbiente.Items.AddRange(new object[] {
            resources.GetString("comboBoxAmbiente.Items"),
            resources.GetString("comboBoxAmbiente.Items1")});
            resources.ApplyResources(this.comboBoxAmbiente, "comboBoxAmbiente");
            this.comboBoxAmbiente.Name = "comboBoxAmbiente";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // button_assinarxml
            // 
            resources.ApplyResources(this.button_assinarxml, "button_assinarxml");
            this.button_assinarxml.Name = "button_assinarxml";
            this.button_assinarxml.UseVisualStyleBackColor = true;
            this.button_assinarxml.Click += new System.EventHandler(this.button_assinarxml_Click);
            // 
            // button_recepcao_nfe
            // 
            resources.ApplyResources(this.button_recepcao_nfe, "button_recepcao_nfe");
            this.button_recepcao_nfe.Name = "button_recepcao_nfe";
            this.button_recepcao_nfe.UseVisualStyleBackColor = true;
            this.button_recepcao_nfe.Click += new System.EventHandler(this.button_recepcao_nfe_Click);
            // 
            // textBox_xmldados
            // 
            resources.ApplyResources(this.textBox_xmldados, "textBox_xmldados");
            this.textBox_xmldados.Name = "textBox_xmldados";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // button_seleciona_xmldados
            // 
            resources.ApplyResources(this.button_seleciona_xmldados, "button_seleciona_xmldados");
            this.button_seleciona_xmldados.Name = "button_seleciona_xmldados";
            this.button_seleciona_xmldados.UseVisualStyleBackColor = true;
            this.button_seleciona_xmldados.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_xmlretorno
            // 
            resources.ApplyResources(this.textBox_xmlretorno, "textBox_xmlretorno");
            this.textBox_xmlretorno.Name = "textBox_xmlretorno";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // button_RetRecepcao
            // 
            resources.ApplyResources(this.button_RetRecepcao, "button_RetRecepcao");
            this.button_RetRecepcao.Name = "button_RetRecepcao";
            this.button_RetRecepcao.UseVisualStyleBackColor = true;
            this.button_RetRecepcao.Click += new System.EventHandler(this.button_RetRecepcao_Click);
            // 
            // comboBoxUF
            // 
            this.comboBoxUF.DisplayMember = "PR";
            this.comboBoxUF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUF.FormattingEnabled = true;
            this.comboBoxUF.Items.AddRange(new object[] {
            resources.GetString("comboBoxUF.Items"),
            resources.GetString("comboBoxUF.Items1"),
            resources.GetString("comboBoxUF.Items2")});
            resources.ApplyResources(this.comboBoxUF, "comboBoxUF");
            this.comboBoxUF.Name = "comboBoxUF";
            this.comboBoxUF.SelectedIndexChanged += new System.EventHandler(this.comboBoxUF_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // FormTeste
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_RetRecepcao);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_xmlretorno);
            this.Controls.Add(this.button_seleciona_xmldados);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_xmldados);
            this.Controls.Add(this.button_recepcao_nfe);
            this.Controls.Add(this.button_assinarxml);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxAmbiente);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxUF);
            this.Controls.Add(this.button_exibir_certificado_selecionado);
            this.Controls.Add(this.button_selecionar_certificado);
            this.Controls.Add(this.button_status_servico);
            this.Name = "FormTeste";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_status_servico;
        private System.Windows.Forms.Button button_selecionar_certificado;
        private System.Windows.Forms.Button button_exibir_certificado_selecionado;
        private System.Windows.Forms.ComboBox comboBoxAmbiente;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_assinarxml;
        private System.Windows.Forms.Button button_recepcao_nfe;
        private System.Windows.Forms.TextBox textBox_xmldados;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_seleciona_xmldados;
        private System.Windows.Forms.TextBox textBox_xmlretorno;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_RetRecepcao;
        private System.Windows.Forms.ComboBox comboBoxUF;
        private System.Windows.Forms.Label label1;
    }
}


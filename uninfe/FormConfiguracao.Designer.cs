namespace uninfe
{
    partial class FormConfiguracao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguracao));
            this.textBox_PastaEnvioXML = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_PastaRetornoXML = new System.Windows.Forms.TextBox();
            this.comboBox_UF = new System.Windows.Forms.ComboBox();
            this.comboBox_Ambiente = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_salvar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_fechar = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog_xmlenvio = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog_xmlretorno = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox_dadoscertificado = new System.Windows.Forms.TextBox();
            this.button_selecionar_certificado = new System.Windows.Forms.Button();
            this.button_SelectPastaXmlRetorno = new System.Windows.Forms.Button();
            this.button_selectxmlenvio = new System.Windows.Forms.Button();
            this.textBox_PastaEnviados = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_SelectPastaXmlEnviado = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox_Empresa = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_tpEmis = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_SelectPastaBackup = new System.Windows.Forms.Button();
            this.textBox_PastaBackup = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_SelectPastaXmlErro = new System.Windows.Forms.Button();
            this.textBox_PastaXmlErro = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.folderBrowserDialog_xmlenviado = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog_xmlerro = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog_backup = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_PastaEnvioXML
            // 
            this.textBox_PastaEnvioXML.AccessibleDescription = "";
            this.textBox_PastaEnvioXML.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaEnvioXML.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaEnvioXML.Location = new System.Drawing.Point(6, 25);
            this.textBox_PastaEnvioXML.Name = "textBox_PastaEnvioXML";
            this.textBox_PastaEnvioXML.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaEnvioXML.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pasta de envio dos arquivos XML:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pasta de retorno dos arquivos XML:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Unidade Federativa (UF-Estado):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Ambiente:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(225, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Informações do certificado digital selecionado:";
            // 
            // textBox_PastaRetornoXML
            // 
            this.textBox_PastaRetornoXML.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaRetornoXML.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaRetornoXML.Location = new System.Drawing.Point(6, 72);
            this.textBox_PastaRetornoXML.Name = "textBox_PastaRetornoXML";
            this.textBox_PastaRetornoXML.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaRetornoXML.TabIndex = 10;
            // 
            // comboBox_UF
            // 
            this.comboBox_UF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_UF.FormattingEnabled = true;
            this.comboBox_UF.Location = new System.Drawing.Point(6, 69);
            this.comboBox_UF.Name = "comboBox_UF";
            this.comboBox_UF.Size = new System.Drawing.Size(121, 21);
            this.comboBox_UF.TabIndex = 8;
            // 
            // comboBox_Ambiente
            // 
            this.comboBox_Ambiente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Ambiente.FormattingEnabled = true;
            this.comboBox_Ambiente.Location = new System.Drawing.Point(6, 116);
            this.comboBox_Ambiente.Name = "comboBox_Ambiente";
            this.comboBox_Ambiente.Size = new System.Drawing.Size(121, 21);
            this.comboBox_Ambiente.TabIndex = 10;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_salvar,
            this.toolStripButton_fechar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(612, 39);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_salvar
            // 
            this.toolStripButton_salvar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_salvar.Image = global::uninfe.Properties.Resources.filesave;
            this.toolStripButton_salvar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_salvar.Name = "toolStripButton_salvar";
            this.toolStripButton_salvar.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton_salvar.Text = "toolStripButton1";
            this.toolStripButton_salvar.ToolTipText = "Salvar as alterações e fecha a tela";
            this.toolStripButton_salvar.Click += new System.EventHandler(this.toolStripButton_salvar_Click);
            // 
            // toolStripButton_fechar
            // 
            this.toolStripButton_fechar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_fechar.Image = global::uninfe.Properties.Resources.fileclose;
            this.toolStripButton_fechar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_fechar.Name = "toolStripButton_fechar";
            this.toolStripButton_fechar.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton_fechar.Text = "toolStripButton2";
            this.toolStripButton_fechar.ToolTipText = "Fechar a tela sem salvar as alterações";
            this.toolStripButton_fechar.Click += new System.EventHandler(this.toolStripButton_fechar_Click);
            // 
            // folderBrowserDialog_xmlenvio
            // 
            this.folderBrowserDialog_xmlenvio.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // folderBrowserDialog_xmlretorno
            // 
            this.folderBrowserDialog_xmlretorno.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // textBox_dadoscertificado
            // 
            this.textBox_dadoscertificado.Location = new System.Drawing.Point(3, 22);
            this.textBox_dadoscertificado.Multiline = true;
            this.textBox_dadoscertificado.Name = "textBox_dadoscertificado";
            this.textBox_dadoscertificado.ReadOnly = true;
            this.textBox_dadoscertificado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_dadoscertificado.Size = new System.Drawing.Size(516, 237);
            this.textBox_dadoscertificado.TabIndex = 12;
            // 
            // button_selecionar_certificado
            // 
            this.button_selecionar_certificado.Image = global::uninfe.Properties.Resources.identity1;
            this.button_selecionar_certificado.Location = new System.Drawing.Point(525, 22);
            this.button_selecionar_certificado.Name = "button_selecionar_certificado";
            this.button_selecionar_certificado.Size = new System.Drawing.Size(60, 50);
            this.button_selecionar_certificado.TabIndex = 13;
            this.button_selecionar_certificado.UseVisualStyleBackColor = true;
            this.button_selecionar_certificado.Click += new System.EventHandler(this.button_selecionar_certificado_Click);
            // 
            // button_SelectPastaXmlRetorno
            // 
            this.button_SelectPastaXmlRetorno.Image = global::uninfe.Properties.Resources.folder_orange_open;
            this.button_SelectPastaXmlRetorno.Location = new System.Drawing.Point(554, 70);
            this.button_SelectPastaXmlRetorno.Name = "button_SelectPastaXmlRetorno";
            this.button_SelectPastaXmlRetorno.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaXmlRetorno.TabIndex = 11;
            this.button_SelectPastaXmlRetorno.UseVisualStyleBackColor = true;
            this.button_SelectPastaXmlRetorno.Click += new System.EventHandler(this.button_SelectPastaXmlRetorno_Click);
            // 
            // button_selectxmlenvio
            // 
            this.button_selectxmlenvio.Image = global::uninfe.Properties.Resources.folder_orange_open;
            this.button_selectxmlenvio.Location = new System.Drawing.Point(554, 23);
            this.button_selectxmlenvio.Name = "button_selectxmlenvio";
            this.button_selectxmlenvio.Size = new System.Drawing.Size(27, 23);
            this.button_selectxmlenvio.TabIndex = 6;
            this.button_selectxmlenvio.UseVisualStyleBackColor = true;
            this.button_selectxmlenvio.Click += new System.EventHandler(this.button_selectxmlenvio_Click);
            // 
            // textBox_PastaEnviados
            // 
            this.textBox_PastaEnviados.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaEnviados.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaEnviados.Location = new System.Drawing.Point(6, 118);
            this.textBox_PastaEnviados.Name = "textBox_PastaEnviados";
            this.textBox_PastaEnviados.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaEnviados.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(262, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Pasta para arquivamento dos arquivos XML enviados:";
            // 
            // button_SelectPastaXmlEnviado
            // 
            this.button_SelectPastaXmlEnviado.Image = global::uninfe.Properties.Resources.folder_orange_open;
            this.button_SelectPastaXmlEnviado.Location = new System.Drawing.Point(554, 116);
            this.button_SelectPastaXmlEnviado.Name = "button_SelectPastaXmlEnviado";
            this.button_SelectPastaXmlEnviado.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaXmlEnviado.TabIndex = 18;
            this.button_SelectPastaXmlEnviado.UseVisualStyleBackColor = true;
            this.button_SelectPastaXmlEnviado.Click += new System.EventHandler(this.button_SelectPastaXmlEnviado_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(10, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(596, 288);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_Empresa);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.comboBox_tpEmis);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.comboBox_UF);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.comboBox_Ambiente);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(588, 262);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Geral";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox_Empresa
            // 
            this.textBox_Empresa.Location = new System.Drawing.Point(6, 23);
            this.textBox_Empresa.MaxLength = 50;
            this.textBox_Empresa.Name = "textBox_Empresa";
            this.textBox_Empresa.Size = new System.Drawing.Size(557, 20);
            this.textBox_Empresa.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Empresa:";
            // 
            // comboBox_tpEmis
            // 
            this.comboBox_tpEmis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_tpEmis.FormattingEnabled = true;
            this.comboBox_tpEmis.Location = new System.Drawing.Point(6, 164);
            this.comboBox_tpEmis.Name = "comboBox_tpEmis";
            this.comboBox_tpEmis.Size = new System.Drawing.Size(284, 21);
            this.comboBox_tpEmis.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Tipo de Emissão NF-e:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_SelectPastaBackup);
            this.tabPage2.Controls.Add(this.textBox_PastaBackup);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.button_SelectPastaXmlErro);
            this.tabPage2.Controls.Add(this.textBox_PastaXmlErro);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.textBox_PastaEnvioXML);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.button_SelectPastaXmlEnviado);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.textBox_PastaRetornoXML);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.button_selectxmlenvio);
            this.tabPage2.Controls.Add(this.textBox_PastaEnviados);
            this.tabPage2.Controls.Add(this.button_SelectPastaXmlRetorno);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(588, 262);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pastas";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_SelectPastaBackup
            // 
            this.button_SelectPastaBackup.Image = global::uninfe.Properties.Resources.folder_orange_open;
            this.button_SelectPastaBackup.Location = new System.Drawing.Point(554, 213);
            this.button_SelectPastaBackup.Name = "button_SelectPastaBackup";
            this.button_SelectPastaBackup.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaBackup.TabIndex = 24;
            this.button_SelectPastaBackup.UseVisualStyleBackColor = true;
            this.button_SelectPastaBackup.Click += new System.EventHandler(this.button_SelectPastaBackup_Click);
            // 
            // textBox_PastaBackup
            // 
            this.textBox_PastaBackup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaBackup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaBackup.Location = new System.Drawing.Point(6, 213);
            this.textBox_PastaBackup.Name = "textBox_PastaBackup";
            this.textBox_PastaBackup.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaBackup.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 197);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(235, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Pasta opcional para Backup dos XML enviados:";
            // 
            // button_SelectPastaXmlErro
            // 
            this.button_SelectPastaXmlErro.Image = global::uninfe.Properties.Resources.folder_orange_open;
            this.button_SelectPastaXmlErro.Location = new System.Drawing.Point(554, 162);
            this.button_SelectPastaXmlErro.Name = "button_SelectPastaXmlErro";
            this.button_SelectPastaXmlErro.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaXmlErro.TabIndex = 21;
            this.button_SelectPastaXmlErro.UseVisualStyleBackColor = true;
            this.button_SelectPastaXmlErro.Click += new System.EventHandler(this.button_SelectPastaXmlErro_Click);
            // 
            // textBox_PastaXmlErro
            // 
            this.textBox_PastaXmlErro.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaXmlErro.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaXmlErro.Location = new System.Drawing.Point(6, 164);
            this.textBox_PastaXmlErro.Name = "textBox_PastaXmlErro";
            this.textBox_PastaXmlErro.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaXmlErro.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(437, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Pasta para arquivamento temporário dos XML que apresentaram erro na tentativa do " +
                "envio:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox_dadoscertificado);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.button_selecionar_certificado);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(588, 262);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Certificado Digital";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // FormConfiguracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 342);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConfiguracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurações";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_PastaEnvioXML;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_PastaRetornoXML;
        private System.Windows.Forms.ComboBox comboBox_UF;
        private System.Windows.Forms.ComboBox comboBox_Ambiente;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Button button_selectxmlenvio;
        private System.Windows.Forms.Button button_SelectPastaXmlRetorno;
        private System.Windows.Forms.ToolStripButton toolStripButton_salvar;
        private System.Windows.Forms.ToolStripButton toolStripButton_fechar;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_xmlenvio;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_xmlretorno;
        private System.Windows.Forms.TextBox textBox_dadoscertificado;
        private System.Windows.Forms.Button button_selecionar_certificado;
        private System.Windows.Forms.TextBox textBox_PastaEnviados;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_SelectPastaXmlEnviado;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_xmlenviado;
        private System.Windows.Forms.Button button_SelectPastaXmlErro;
        private System.Windows.Forms.TextBox textBox_PastaXmlErro;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_xmlerro;
        private System.Windows.Forms.ComboBox comboBox_tpEmis;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_Empresa;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_SelectPastaBackup;
        private System.Windows.Forms.TextBox textBox_PastaBackup;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_backup;
    }
}
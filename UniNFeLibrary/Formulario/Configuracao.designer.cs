namespace UniNFeLibrary.Formulario
{
    partial class Configuracao
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuracao));
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
            this.checkBoxRetornoNFETxt = new System.Windows.Forms.CheckBox();
            this.textBox_Empresa = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_tpEmis = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.udDiasLimpeza = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.cboDiretorioSalvarComo = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button_SelectPastaValidar = new System.Windows.Forms.Button();
            this.tbPastaValidar = new System.Windows.Forms.TextBox();
            this.btnSelectPastaLote = new System.Windows.Forms.Button();
            this.tbPastaLote = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button_SelectPastaBackup = new System.Windows.Forms.Button();
            this.textBox_PastaBackup = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_SelectPastaXmlErro = new System.Windows.Forms.Button();
            this.textBox_PastaXmlErro = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.nudPorta = new System.Windows.Forms.NumericUpDown();
            this.tbSenha = new System.Windows.Forms.TextBox();
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.cbProxy = new System.Windows.Forms.CheckBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.lblPorta = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.folderBrowserDialog_xmlenviado = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog_xmlerro = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog_backup = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog_xmlenviolote = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog_Validar = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDiasLimpeza)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorta)).BeginInit();
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
            this.textBox_PastaEnvioXML.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(486, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pasta onde será gravado os arquivos XML´s a serem enviados individualmente para o" +
                "s WebServices:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(357, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pasta onde será gravado os arquivos XML´s de retorno dos WebServices:";
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
            this.textBox_PastaRetornoXML.Location = new System.Drawing.Point(6, 115);
            this.textBox_PastaRetornoXML.Name = "textBox_PastaRetornoXML";
            this.textBox_PastaRetornoXML.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaRetornoXML.TabIndex = 10;
            // 
            // comboBox_UF
            // 
            this.comboBox_UF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_UF.FormattingEnabled = true;
            this.comboBox_UF.ItemHeight = 13;
            this.comboBox_UF.Location = new System.Drawing.Point(6, 69);
            this.comboBox_UF.Name = "comboBox_UF";
            this.comboBox_UF.Size = new System.Drawing.Size(121, 21);
            this.comboBox_UF.TabIndex = 8;
            // 
            // comboBox_Ambiente
            // 
            this.comboBox_Ambiente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Ambiente.FormattingEnabled = true;
            this.comboBox_Ambiente.ItemHeight = 13;
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
            this.toolStrip1.Size = new System.Drawing.Size(607, 39);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_salvar
            // 
            this.toolStripButton_salvar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_salvar.Image = global::UniNFeLibrary.Properties.Resources.filesave;
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
            this.toolStripButton_fechar.Image = global::UniNFeLibrary.Properties.Resources.fileclose;
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
            this.textBox_dadoscertificado.Size = new System.Drawing.Size(516, 326);
            this.textBox_dadoscertificado.TabIndex = 18;
            // 
            // button_selecionar_certificado
            // 
            this.button_selecionar_certificado.Image = global::UniNFeLibrary.Properties.Resources.identity1;
            this.button_selecionar_certificado.Location = new System.Drawing.Point(525, 22);
            this.button_selecionar_certificado.Name = "button_selecionar_certificado";
            this.button_selecionar_certificado.Size = new System.Drawing.Size(60, 50);
            this.button_selecionar_certificado.TabIndex = 19;
            this.button_selecionar_certificado.UseVisualStyleBackColor = true;
            this.button_selecionar_certificado.Click += new System.EventHandler(this.button_selecionar_certificado_Click);
            // 
            // button_SelectPastaXmlRetorno
            // 
            this.button_SelectPastaXmlRetorno.Image = global::UniNFeLibrary.Properties.Resources.folder_orange_open;
            this.button_SelectPastaXmlRetorno.Location = new System.Drawing.Point(554, 113);
            this.button_SelectPastaXmlRetorno.Name = "button_SelectPastaXmlRetorno";
            this.button_SelectPastaXmlRetorno.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaXmlRetorno.TabIndex = 11;
            this.button_SelectPastaXmlRetorno.UseVisualStyleBackColor = true;
            this.button_SelectPastaXmlRetorno.Click += new System.EventHandler(this.button_SelectPastaXmlRetorno_Click);
            // 
            // button_selectxmlenvio
            // 
            this.button_selectxmlenvio.Image = global::UniNFeLibrary.Properties.Resources.folder_orange_open;
            this.button_selectxmlenvio.Location = new System.Drawing.Point(554, 23);
            this.button_selectxmlenvio.Name = "button_selectxmlenvio";
            this.button_selectxmlenvio.Size = new System.Drawing.Size(27, 23);
            this.button_selectxmlenvio.TabIndex = 7;
            this.button_selectxmlenvio.UseVisualStyleBackColor = true;
            this.button_selectxmlenvio.Click += new System.EventHandler(this.button_selectxmlenvio_Click);
            // 
            // textBox_PastaEnviados
            // 
            this.textBox_PastaEnviados.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaEnviados.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaEnviados.Location = new System.Drawing.Point(6, 161);
            this.textBox_PastaEnviados.Name = "textBox_PastaEnviados";
            this.textBox_PastaEnviados.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaEnviados.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(265, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Pasta onde será gravado os arquivos XML´s enviados:";
            // 
            // button_SelectPastaXmlEnviado
            // 
            this.button_SelectPastaXmlEnviado.Image = global::UniNFeLibrary.Properties.Resources.folder_orange_open;
            this.button_SelectPastaXmlEnviado.Location = new System.Drawing.Point(554, 159);
            this.button_SelectPastaXmlEnviado.Name = "button_SelectPastaXmlEnviado";
            this.button_SelectPastaXmlEnviado.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaXmlEnviado.TabIndex = 13;
            this.button_SelectPastaXmlEnviado.UseVisualStyleBackColor = true;
            this.button_SelectPastaXmlEnviado.Click += new System.EventHandler(this.button_SelectPastaXmlEnviado_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(6, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(596, 377);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxRetornoNFETxt);
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
            this.tabPage1.Size = new System.Drawing.Size(588, 351);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Geral";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxRetornoNFETxt
            // 
            this.checkBoxRetornoNFETxt.AutoSize = true;
            this.checkBoxRetornoNFETxt.Location = new System.Drawing.Point(6, 329);
            this.checkBoxRetornoNFETxt.Name = "checkBoxRetornoNFETxt";
            this.checkBoxRetornoNFETxt.Size = new System.Drawing.Size(334, 17);
            this.checkBoxRetornoNFETxt.TabIndex = 13;
            this.checkBoxRetornoNFETxt.Text = "Gravar o retorno dos webservices também no formato texto (TXT)";
            this.checkBoxRetornoNFETxt.UseVisualStyleBackColor = true;
            // 
            // textBox_Empresa
            // 
            this.textBox_Empresa.Location = new System.Drawing.Point(6, 23);
            this.textBox_Empresa.MaxLength = 50;
            this.textBox_Empresa.Name = "textBox_Empresa";
            this.textBox_Empresa.Size = new System.Drawing.Size(557, 20);
            this.textBox_Empresa.TabIndex = 2;
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
            this.comboBox_tpEmis.ItemHeight = 13;
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
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Tipo de Emissão:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.udDiasLimpeza);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.cboDiretorioSalvarComo);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.button_SelectPastaValidar);
            this.tabPage2.Controls.Add(this.tbPastaValidar);
            this.tabPage2.Controls.Add(this.btnSelectPastaLote);
            this.tabPage2.Controls.Add(this.tbPastaLote);
            this.tabPage2.Controls.Add(this.label11);
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
            this.tabPage2.Size = new System.Drawing.Size(588, 351);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pastas";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // udDiasLimpeza
            // 
            this.udDiasLimpeza.Location = new System.Drawing.Point(529, 322);
            this.udDiasLimpeza.Maximum = new decimal(new int[] {
            7305,
            0,
            0,
            0});
            this.udDiasLimpeza.Name = "udDiasLimpeza";
            this.udDiasLimpeza.Size = new System.Drawing.Size(52, 20);
            this.udDiasLimpeza.TabIndex = 32;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(278, 318);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(250, 32);
            this.label14.TabIndex = 31;
            this.label14.Text = "Quantos dias devem ser mantidos os arquivos na pasta temporário e retorno? Deixe " +
                "0 para infinito.";
            // 
            // cboDiretorioSalvarComo
            // 
            this.cboDiretorioSalvarComo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiretorioSalvarComo.FormattingEnabled = true;
            this.cboDiretorioSalvarComo.Items.AddRange(new object[] {
            "AMD",
            "AM",
            "AD",
            "MDA",
            "MD",
            "MA",
            "DMA",
            "DM",
            "DA",
            "A\\M\\D",
            "A\\M",
            "A\\D",
            "M\\D\\A",
            "M\\D",
            "M\\A",
            "D\\M\\A",
            "D\\M",
            "D\\A"});
            this.cboDiretorioSalvarComo.Location = new System.Drawing.Point(190, 321);
            this.cboDiretorioSalvarComo.Name = "cboDiretorioSalvarComo";
            this.cboDiretorioSalvarComo.Size = new System.Drawing.Size(65, 21);
            this.cboDiretorioSalvarComo.TabIndex = 30;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 318);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(185, 35);
            this.label13.TabIndex = 29;
            this.label13.Text = "Como devem ser criados os diretórios baseados na data de emissão?";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 275);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(350, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Pasta onde será gravado os arquivos XML´s a serem somente validados:";
            // 
            // button_SelectPastaValidar
            // 
            this.button_SelectPastaValidar.Image = global::UniNFeLibrary.Properties.Resources.folder_orange_open;
            this.button_SelectPastaValidar.Location = new System.Drawing.Point(555, 290);
            this.button_SelectPastaValidar.Name = "button_SelectPastaValidar";
            this.button_SelectPastaValidar.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaValidar.TabIndex = 27;
            this.button_SelectPastaValidar.UseVisualStyleBackColor = true;
            this.button_SelectPastaValidar.Click += new System.EventHandler(this.button_SelectPastaValidar_Click);
            // 
            // tbPastaValidar
            // 
            this.tbPastaValidar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbPastaValidar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.tbPastaValidar.Location = new System.Drawing.Point(7, 290);
            this.tbPastaValidar.Name = "tbPastaValidar";
            this.tbPastaValidar.Size = new System.Drawing.Size(542, 20);
            this.tbPastaValidar.TabIndex = 26;
            // 
            // btnSelectPastaLote
            // 
            this.btnSelectPastaLote.Image = global::UniNFeLibrary.Properties.Resources.folder_orange_open;
            this.btnSelectPastaLote.Location = new System.Drawing.Point(554, 69);
            this.btnSelectPastaLote.Name = "btnSelectPastaLote";
            this.btnSelectPastaLote.Size = new System.Drawing.Size(27, 23);
            this.btnSelectPastaLote.TabIndex = 9;
            this.btnSelectPastaLote.UseVisualStyleBackColor = true;
            this.btnSelectPastaLote.Click += new System.EventHandler(this.btnSelectPastaLote_Click);
            // 
            // tbPastaLote
            // 
            this.tbPastaLote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbPastaLote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.tbPastaLote.Location = new System.Drawing.Point(6, 69);
            this.tbPastaLote.Name = "tbPastaLote";
            this.tbPastaLote.Size = new System.Drawing.Size(542, 20);
            this.tbPastaLote.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(488, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Pasta onde será gravado os arquivos XML´s de NF-e a serem enviadas em lote para o" +
                "s WebServices:";
            // 
            // button_SelectPastaBackup
            // 
            this.button_SelectPastaBackup.Image = global::UniNFeLibrary.Properties.Resources.folder_orange_open;
            this.button_SelectPastaBackup.Location = new System.Drawing.Point(554, 250);
            this.button_SelectPastaBackup.Name = "button_SelectPastaBackup";
            this.button_SelectPastaBackup.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaBackup.TabIndex = 17;
            this.button_SelectPastaBackup.UseVisualStyleBackColor = true;
            this.button_SelectPastaBackup.Click += new System.EventHandler(this.button_SelectPastaBackup_Click);
            // 
            // textBox_PastaBackup
            // 
            this.textBox_PastaBackup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaBackup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaBackup.Location = new System.Drawing.Point(6, 250);
            this.textBox_PastaBackup.Name = "textBox_PastaBackup";
            this.textBox_PastaBackup.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaBackup.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 234);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(200, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Pasta para Backup dos XML´s enviados:";
            // 
            // button_SelectPastaXmlErro
            // 
            this.button_SelectPastaXmlErro.Image = global::UniNFeLibrary.Properties.Resources.folder_orange_open;
            this.button_SelectPastaXmlErro.Location = new System.Drawing.Point(554, 205);
            this.button_SelectPastaXmlErro.Name = "button_SelectPastaXmlErro";
            this.button_SelectPastaXmlErro.Size = new System.Drawing.Size(27, 23);
            this.button_SelectPastaXmlErro.TabIndex = 15;
            this.button_SelectPastaXmlErro.UseVisualStyleBackColor = true;
            this.button_SelectPastaXmlErro.Click += new System.EventHandler(this.button_SelectPastaXmlErro_Click);
            // 
            // textBox_PastaXmlErro
            // 
            this.textBox_PastaXmlErro.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaXmlErro.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaXmlErro.Location = new System.Drawing.Point(6, 207);
            this.textBox_PastaXmlErro.Name = "textBox_PastaXmlErro";
            this.textBox_PastaXmlErro.Size = new System.Drawing.Size(542, 20);
            this.textBox_PastaXmlErro.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 190);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(445, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Pasta para arquivamento temporário dos XML´s que apresentaram erro na tentativa d" +
                "o envio:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox_dadoscertificado);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.button_selecionar_certificado);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(588, 351);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Certificado Digital";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.nudPorta);
            this.tabPage4.Controls.Add(this.tbSenha);
            this.tabPage4.Controls.Add(this.tbUsuario);
            this.tabPage4.Controls.Add(this.cbProxy);
            this.tabPage4.Controls.Add(this.lblSenha);
            this.tabPage4.Controls.Add(this.lblPorta);
            this.tabPage4.Controls.Add(this.lblUsuario);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(588, 315);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Proxy";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // nudPorta
            // 
            this.nudPorta.Enabled = false;
            this.nudPorta.Location = new System.Drawing.Point(59, 89);
            this.nudPorta.Name = "nudPorta";
            this.nudPorta.Size = new System.Drawing.Size(71, 20);
            this.nudPorta.TabIndex = 6;
            this.nudPorta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSenha
            // 
            this.tbSenha.Enabled = false;
            this.tbSenha.Location = new System.Drawing.Point(59, 60);
            this.tbSenha.Name = "tbSenha";
            this.tbSenha.Size = new System.Drawing.Size(216, 20);
            this.tbSenha.TabIndex = 5;
            // 
            // tbUsuario
            // 
            this.tbUsuario.Enabled = false;
            this.tbUsuario.Location = new System.Drawing.Point(59, 34);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.Size = new System.Drawing.Size(327, 20);
            this.tbUsuario.TabIndex = 4;
            // 
            // cbProxy
            // 
            this.cbProxy.AutoSize = true;
            this.cbProxy.Location = new System.Drawing.Point(9, 11);
            this.cbProxy.Name = "cbProxy";
            this.cbProxy.Size = new System.Drawing.Size(133, 17);
            this.cbProxy.TabIndex = 3;
            this.cbProxy.Text = "Usar um servidor proxy";
            this.cbProxy.UseVisualStyleBackColor = true;
            this.cbProxy.CheckedChanged += new System.EventHandler(this.cbProxy_CheckedChanged);
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Enabled = false;
            this.lblSenha.Location = new System.Drawing.Point(6, 62);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(41, 13);
            this.lblSenha.TabIndex = 2;
            this.lblSenha.Text = "Senha:";
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Enabled = false;
            this.lblPorta.Location = new System.Drawing.Point(6, 92);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(35, 13);
            this.lblPorta.TabIndex = 1;
            this.lblPorta.Text = "Porta:";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Enabled = false;
            this.lblUsuario.Location = new System.Drawing.Point(6, 35);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuário:";
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipTitle = "As pastas informadas não podem se repetir";
            // 
            // Configuracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 427);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Configuracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurações";
            this.Load += new System.EventHandler(this.Configuracao_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Configuracao_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDiasLimpeza)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorta)).EndInit();
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
        private System.Windows.Forms.Button btnSelectPastaLote;
        private System.Windows.Forms.TextBox tbPastaLote;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_xmlenviolote;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button_SelectPastaValidar;
        private System.Windows.Forms.TextBox tbPastaValidar;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_Validar;
        private System.Windows.Forms.CheckBox checkBoxRetornoNFETxt;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.CheckBox cbProxy;
        private System.Windows.Forms.TextBox tbSenha;
        private System.Windows.Forms.TextBox tbUsuario;
        private System.Windows.Forms.NumericUpDown nudPorta;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboDiretorioSalvarComo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown udDiasLimpeza;
    }
}
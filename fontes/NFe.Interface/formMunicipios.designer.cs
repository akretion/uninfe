namespace NFe.Interface
{
    partial class formMunicipios
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
            this.comboUf = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colIBGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMunicipio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPadrao = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.edtUF = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.edtPadrao = new System.Windows.Forms.ComboBox();
            this.edtMunicipio = new System.Windows.Forms.TextBox();
            this.edtCodMun = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvDireto = new System.Windows.Forms.DataGridView();
            this.colIBGE_D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCidade_D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPadrao_D = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.municipioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDireto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.municipioBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboUf
            // 
            this.comboUf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUf.FormattingEnabled = true;
            this.comboUf.Items.AddRange(new object[] {
            "AC",
            "AL",
            "AP",
            "AM",
            "BA",
            "CE",
            "DF",
            "ES",
            "GO",
            "MA",
            "MT",
            "MS.MG.PA.PB.PR.PE.PI.RJ.RN.RS.RO.RR.SC.SP.SE.TO"});
            this.comboUf.Location = new System.Drawing.Point(59, 15);
            this.comboUf.Name = "comboUf";
            this.comboUf.Size = new System.Drawing.Size(188, 21);
            this.comboUf.TabIndex = 0;
            this.comboUf.SelectedIndexChanged += new System.EventHandler(this.comboUf_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIBGE,
            this.colMunicipio,
            this.colPadrao});
            this.dataGridView1.Location = new System.Drawing.Point(10, 50);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(669, 338);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // colIBGE
            // 
            this.colIBGE.DataPropertyName = "ibge";
            this.colIBGE.FillWeight = 119.797F;
            this.colIBGE.HeaderText = "Código IBGE";
            this.colIBGE.Name = "colIBGE";
            this.colIBGE.ReadOnly = true;
            // 
            // colMunicipio
            // 
            this.colMunicipio.DataPropertyName = "municipio";
            this.colMunicipio.FillWeight = 119.797F;
            this.colMunicipio.HeaderText = "Municipio";
            this.colMunicipio.Name = "colMunicipio";
            this.colMunicipio.ReadOnly = true;
            // 
            // colPadrao
            // 
            this.colPadrao.DataPropertyName = "padrao";
            this.colPadrao.FillWeight = 119.797F;
            this.colPadrao.HeaderText = "Padrão";
            this.colPadrao.Name = "colPadrao";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Estados";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(696, 424);
            this.tabControl1.TabIndex = 7;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboUf);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(688, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Municípios a definir/manter";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.btnAdd);
            this.tabPage2.Controls.Add(this.dgvDireto);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(688, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Municípios definidos";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.edtUF);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.edtPadrao);
            this.panel1.Controls.Add(this.edtMunicipio);
            this.panel1.Controls.Add(this.edtCodMun);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(117, 85);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 172);
            this.panel1.TabIndex = 8;
            this.panel1.Visible = false;
            // 
            // edtUF
            // 
            this.edtUF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.edtUF.FormattingEnabled = true;
            this.edtUF.Location = new System.Drawing.Point(119, 72);
            this.edtUF.Name = "edtUF";
            this.edtUF.Size = new System.Drawing.Size(67, 21);
            this.edtUF.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Location = new System.Drawing.Point(15, 126);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(394, 5);
            this.panel2.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Padrão";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "UF";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Município/Cidade";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Código IBGE";
            // 
            // edtPadrao
            // 
            this.edtPadrao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.edtPadrao.FormattingEnabled = true;
            this.edtPadrao.Location = new System.Drawing.Point(119, 98);
            this.edtPadrao.Name = "edtPadrao";
            this.edtPadrao.Size = new System.Drawing.Size(147, 21);
            this.edtPadrao.TabIndex = 3;
            // 
            // edtMunicipio
            // 
            this.edtMunicipio.Location = new System.Drawing.Point(119, 46);
            this.edtMunicipio.Name = "edtMunicipio";
            this.edtMunicipio.Size = new System.Drawing.Size(287, 20);
            this.edtMunicipio.TabIndex = 1;
            // 
            // edtCodMun
            // 
            this.edtCodMun.Location = new System.Drawing.Point(119, 20);
            this.edtCodMun.MaxLength = 7;
            this.edtCodMun.Name = "edtCodMun";
            this.edtCodMun.Size = new System.Drawing.Size(100, 20);
            this.edtCodMun.TabIndex = 0;
            this.edtCodMun.Leave += new System.EventHandler(this.edtCodMun_Leave);
            this.edtCodMun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edtCodMun_KeyPress);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(331, 137);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "&Abandonar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(8, 366);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "&Novo";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvDireto
            // 
            this.dgvDireto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDireto.AutoGenerateColumns = false;
            this.dgvDireto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDireto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDireto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIBGE_D,
            this.colCidade_D,
            this.colPadrao_D});
            this.dgvDireto.DataSource = this.municipioBindingSource;
            this.dgvDireto.Location = new System.Drawing.Point(10, 10);
            this.dgvDireto.MultiSelect = false;
            this.dgvDireto.Name = "dgvDireto";
            this.dgvDireto.RowHeadersWidth = 20;
            this.dgvDireto.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDireto.Size = new System.Drawing.Size(669, 351);
            this.dgvDireto.TabIndex = 6;
            this.dgvDireto.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dgvDireto.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // colIBGE_D
            // 
            this.colIBGE_D.DataPropertyName = "CodigoMunicipio";
            this.colIBGE_D.HeaderText = "Código IBGE";
            this.colIBGE_D.Name = "colIBGE_D";
            this.colIBGE_D.ReadOnly = true;
            // 
            // colCidade_D
            // 
            this.colCidade_D.DataPropertyName = "Nome";
            this.colCidade_D.HeaderText = "Município";
            this.colCidade_D.Name = "colCidade_D";
            this.colCidade_D.ReadOnly = true;
            // 
            // colPadrao_D
            // 
            this.colPadrao_D.DataPropertyName = "PadraoStr";
            this.colPadrao_D.HeaderText = "Padrão";
            this.colPadrao_D.Name = "colPadrao_D";
            this.colPadrao_D.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPadrao_D.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // municipioBindingSource
            // 
            this.municipioBindingSource.DataSource = typeof(NFe.Components.Municipio);
            // 
            // formMunicipios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 448);
            this.Controls.Add(this.tabControl1);
            this.Name = "formMunicipios";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Municipios";
            this.Load += new System.EventHandler(this.formMunicipios_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formMunicipios_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDireto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.municipioBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboUf;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvDireto;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.BindingSource municipioBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIBGE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMunicipio;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPadrao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIBGE_D;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCidade_D;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPadrao_D;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox edtCodMun;
        private System.Windows.Forms.TextBox edtMunicipio;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox edtPadrao;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox edtUF;
    }
}
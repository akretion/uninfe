namespace NFe.UI.Formularios.NFSe
{
    partial class userGrid1
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.municipioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.comboUf = new MetroFramework.Controls.MetroComboBox();
            this.codigoMunicipioDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPadrao = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.municipioBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigoMunicipioDataGridViewTextBoxColumn,
            this.nomeDataGridViewTextBoxColumn,
            this.colPadrao});
            this.dataGridView1.DataSource = this.municipioBindingSource;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.Location = new System.Drawing.Point(3, 38);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(691, 389);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // municipioBindingSource
            // 
            this.municipioBindingSource.AllowNew = false;
            this.municipioBindingSource.DataSource = typeof(NFe.Components.Municipio);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(3, 7);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(53, 19);
            this.metroLabel1.TabIndex = 8;
            this.metroLabel1.Text = "Estados";
            // 
            // comboUf
            // 
            this.comboUf.FormattingEnabled = true;
            this.comboUf.ItemHeight = 19;
            this.comboUf.Location = new System.Drawing.Point(62, 7);
            this.comboUf.MaxDropDownItems = 15;
            this.comboUf.Name = "comboUf";
            this.comboUf.Size = new System.Drawing.Size(201, 25);
            this.comboUf.TabIndex = 7;
            this.comboUf.UseSelectable = true;
            this.comboUf.SelectedValueChanged += new System.EventHandler(this.comboUf_SelectedValueChanged);
            // 
            // codigoMunicipioDataGridViewTextBoxColumn
            // 
            this.codigoMunicipioDataGridViewTextBoxColumn.DataPropertyName = "CodigoMunicipio";
            this.codigoMunicipioDataGridViewTextBoxColumn.HeaderText = "Código IBGE";
            this.codigoMunicipioDataGridViewTextBoxColumn.Name = "codigoMunicipioDataGridViewTextBoxColumn";
            this.codigoMunicipioDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nomeDataGridViewTextBoxColumn
            // 
            this.nomeDataGridViewTextBoxColumn.DataPropertyName = "Nome";
            this.nomeDataGridViewTextBoxColumn.HeaderText = "Nome";
            this.nomeDataGridViewTextBoxColumn.Name = "nomeDataGridViewTextBoxColumn";
            this.nomeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // colPadrao
            // 
            this.colPadrao.DataPropertyName = "PadraoStr";
            this.colPadrao.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colPadrao.HeaderText = "Padrão";
            this.colPadrao.Name = "colPadrao";
            this.colPadrao.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPadrao.Sorted = true;
            this.colPadrao.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // userGrid1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.comboUf);
            this.Name = "userGrid1";
            this.Size = new System.Drawing.Size(697, 430);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.municipioBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroComboBox comboUf;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource municipioBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoMunicipioDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPadrao;
    }
}

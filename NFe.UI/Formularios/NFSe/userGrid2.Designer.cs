namespace NFe.UI.Formularios.NFSe
{
    partial class userGrid2
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
            this.dgvDireto = new System.Windows.Forms.DataGridView();
            this.municipioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnAdd = new MetroFramework.Controls.MetroButton();
            this.colIBGE_D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCidade_D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPadrao_D = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDireto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.municipioBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDireto
            // 
            this.dgvDireto.AllowUserToDeleteRows = false;
            this.dgvDireto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDireto.AutoGenerateColumns = false;
            this.dgvDireto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDireto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDireto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDireto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIBGE_D,
            this.colCidade_D,
            this.colPadrao_D});
            this.dgvDireto.DataSource = this.municipioBindingSource;
            this.dgvDireto.Location = new System.Drawing.Point(3, 38);
            this.dgvDireto.MultiSelect = false;
            this.dgvDireto.Name = "dgvDireto";
            this.dgvDireto.RowHeadersWidth = 20;
            this.dgvDireto.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDireto.Size = new System.Drawing.Size(685, 315);
            this.dgvDireto.TabIndex = 10;
            this.dgvDireto.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDireto_CellEndEdit);
            this.dgvDireto.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDireto_DataError);
            // 
            // municipioBindingSource
            // 
            this.municipioBindingSource.AllowNew = false;
            this.municipioBindingSource.DataSource = typeof(NFe.Components.Municipio);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 7);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Novo";
            this.btnAdd.UseSelectable = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            this.colPadrao_D.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colPadrao_D.HeaderText = "Padrão";
            this.colPadrao_D.Name = "colPadrao_D";
            this.colPadrao_D.Sorted = true;
            // 
            // userGrid2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvDireto);
            this.Name = "userGrid2";
            this.Size = new System.Drawing.Size(691, 350);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDireto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.municipioBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnAdd;
        private System.Windows.Forms.BindingSource municipioBindingSource;
        public System.Windows.Forms.DataGridView dgvDireto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIBGE_D;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCidade_D;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPadrao_D;
    }
}

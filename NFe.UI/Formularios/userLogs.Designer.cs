namespace NFe.UI
{
    partial class userLogs
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
            this.cbArquivos = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox3 = new MetroFramework.Controls.MetroTextBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Size = new System.Drawing.Size(40, 25);
            this.labelTitle.Text = "Log";
            // 
            // cbArquivos
            // 
            this.cbArquivos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbArquivos.BackColor = System.Drawing.Color.Transparent;
            this.cbArquivos.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbArquivos.FormattingEnabled = true;
            this.cbArquivos.IntegralHeight = false;
            this.cbArquivos.ItemHeight = 19;
            this.cbArquivos.Items.AddRange(new object[] {
            "Eins",
            "Polizei",
            "Zwei"});
            this.cbArquivos.Location = new System.Drawing.Point(3, 89);
            this.cbArquivos.MaxDropDownItems = 15;
            this.cbArquivos.Name = "cbArquivos";
            this.cbArquivos.Size = new System.Drawing.Size(517, 25);
            this.cbArquivos.Sorted = true;
            this.cbArquivos.TabIndex = 32;
            this.cbArquivos.UseSelectable = true;
            this.cbArquivos.SelectedIndexChanged += new System.EventHandler(this.metroComboBox3_SelectedIndexChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(3, 64);
            this.metroLabel2.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(60, 19);
            this.metroLabel2.TabIndex = 33;
            this.metroLabel2.Text = "Arquivos";
            // 
            // metroTextBox3
            // 
            this.metroTextBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTextBox3.Lines = new string[] {
        "Multiline Textbox"};
            this.metroTextBox3.Location = new System.Drawing.Point(3, 124);
            this.metroTextBox3.MaxLength = 32767;
            this.metroTextBox3.Multiline = true;
            this.metroTextBox3.Name = "metroTextBox3";
            this.metroTextBox3.PasswordChar = '\0';
            this.metroTextBox3.ReadOnly = true;
            this.metroTextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.metroTextBox3.SelectedText = "";
            this.metroTextBox3.Size = new System.Drawing.Size(593, 223);
            this.metroTextBox3.TabIndex = 34;
            this.metroTextBox3.Text = "Multiline Textbox";
            this.metroTextBox3.UseSelectable = true;
            this.metroTextBox3.UseStyleColors = true;
            // 
            // metroButton1
            // 
            this.metroButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton1.Location = new System.Drawing.Point(532, 89);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(64, 25);
            this.metroButton1.TabIndex = 35;
            this.metroButton1.Text = "Excluir";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // userLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroTextBox3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.cbArquivos);
            this.Name = "userLogs";
            this.Size = new System.Drawing.Size(603, 350);
            this.Controls.SetChildIndex(this.labelTitle, 0);
            this.Controls.SetChildIndex(this.cbArquivos, 0);
            this.Controls.SetChildIndex(this.metroLabel2, 0);
            this.Controls.SetChildIndex(this.metroTextBox3, 0);
            this.Controls.SetChildIndex(this.metroButton1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroComboBox cbArquivos;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox metroTextBox3;
        private MetroFramework.Controls.MetroButton metroButton1;
    }
}

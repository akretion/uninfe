namespace NFe.UI.Formularios
{
    partial class FormUpdate
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
            this.prgDownload = new MetroFramework.Controls.MetroProgressBar();
            this.lblProgresso = new MetroFramework.Controls.MetroLabel();
            this.btnAtualizar = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // prgDownload
            // 
            this.prgDownload.Location = new System.Drawing.Point(23, 63);
            this.prgDownload.Name = "prgDownload";
            this.prgDownload.Size = new System.Drawing.Size(462, 23);
            this.prgDownload.Style = MetroFramework.MetroColorStyle.Green;
            this.prgDownload.TabIndex = 27;
            this.prgDownload.Theme = MetroFramework.MetroThemeStyle.Light;
            this.prgDownload.Value = 25;
            // 
            // lblProgresso
            // 
            this.lblProgresso.AutoSize = true;
            this.lblProgresso.Location = new System.Drawing.Point(23, 92);
            this.lblProgresso.Margin = new System.Windows.Forms.Padding(3);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(76, 19);
            this.lblProgresso.TabIndex = 28;
            this.lblProgresso.Text = "MetroLabel";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Location = new System.Drawing.Point(225, 131);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(127, 27);
            this.btnAtualizar.TabIndex = 29;
            this.btnAtualizar.Text = "Iniciar a atualização";
            this.btnAtualizar.UseSelectable = true;
            this.btnAtualizar.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroButton1
            // 
            this.metroButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.metroButton1.Location = new System.Drawing.Point(358, 131);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(127, 27);
            this.metroButton1.TabIndex = 30;
            this.metroButton1.Text = "&Fechar";
            this.metroButton1.UseSelectable = true;
            // 
            // FormUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.metroButton1;
            this.ClientSize = new System.Drawing.Size(508, 175);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.lblProgresso);
            this.Controls.Add(this.prgDownload);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdate";
            this.Resizable = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Atualização do ...";
            this.Load += new System.EventHandler(this.FormUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroProgressBar prgDownload;
        private MetroFramework.Controls.MetroLabel lblProgresso;
        private MetroFramework.Controls.MetroButton btnAtualizar;
        private MetroFramework.Controls.MetroButton metroButton1;
    }
}
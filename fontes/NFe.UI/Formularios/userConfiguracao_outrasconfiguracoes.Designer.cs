namespace NFe.UI.Formularios
{
    partial class userConfiguracao_outrasconfiguracoes
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
            this.chkSalvarXMLDistribuicao = new MetroFramework.Controls.MetroCheckBox();
            this.cbIndSincMDFe = new MetroFramework.Controls.MetroCheckBox();
            this.SuspendLayout();
            // 
            // chkSalvarXMLDistribuicao
            // 
            this.chkSalvarXMLDistribuicao.AutoSize = true;
            this.chkSalvarXMLDistribuicao.Location = new System.Drawing.Point(9, 7);
            this.chkSalvarXMLDistribuicao.Name = "chkSalvarXMLDistribuicao";
            this.chkSalvarXMLDistribuicao.Size = new System.Drawing.Size(337, 15);
            this.chkSalvarXMLDistribuicao.TabIndex = 0;
            this.chkSalvarXMLDistribuicao.Text = "Salvar na pasta autorizados somente o XML de distribuição?";
            this.chkSalvarXMLDistribuicao.UseSelectable = true;
            this.chkSalvarXMLDistribuicao.CheckedChanged += new System.EventHandler(this.ChkSalvarXMLDistribuicao_CheckedChanged);
            // 
            // cbIndSincMDFe
            // 
            this.cbIndSincMDFe.AutoSize = true;
            this.cbIndSincMDFe.Location = new System.Drawing.Point(9, 28);
            this.cbIndSincMDFe.Name = "cbIndSincMDFe";
            this.cbIndSincMDFe.Size = new System.Drawing.Size(268, 15);
            this.cbIndSincMDFe.TabIndex = 1;
            this.cbIndSincMDFe.Text = "Enviar o MDF-e utilizando o processo síncrono";
            this.cbIndSincMDFe.UseSelectable = true;
            this.cbIndSincMDFe.CheckedChanged += new System.EventHandler(this.cbIndSincMDFe_CheckedChanged);
            // 
            // userConfiguracao_outrasconfiguracoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbIndSincMDFe);
            this.Controls.Add(this.chkSalvarXMLDistribuicao);
            this.Name = "userConfiguracao_outrasconfiguracoes";
            this.Size = new System.Drawing.Size(640, 374);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroCheckBox chkSalvarXMLDistribuicao;
        private MetroFramework.Controls.MetroCheckBox cbIndSincMDFe;
    }
}

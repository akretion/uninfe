namespace NFe.Interface
{
    partial class FormSobre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSobre));
            this.lblNomeAplicacao = new System.Windows.Forms.Label();
            this.lblDescricaoAplicacao = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_licenca = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_DataUltimaModificacao = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_versao = new System.Windows.Forms.TextBox();
            this.btnManualUniNFe = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabelSiteProduto = new System.Windows.Forms.LinkLabel();
            this.linkLabelEmail = new System.Windows.Forms.LinkLabel();
            this.linkLabelSite = new System.Windows.Forms.LinkLabel();
            this.lblEmpresa = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNomeAplicacao
            // 
            this.lblNomeAplicacao.AutoSize = true;
            this.lblNomeAplicacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeAplicacao.Location = new System.Drawing.Point(165, 0);
            this.lblNomeAplicacao.Name = "lblNomeAplicacao";
            this.lblNomeAplicacao.Size = new System.Drawing.Size(113, 31);
            this.lblNomeAplicacao.TabIndex = 1;
            this.lblNomeAplicacao.Text = "UniNFe";
            // 
            // lblDescricaoAplicacao
            // 
            this.lblDescricaoAplicacao.AutoSize = true;
            this.lblDescricaoAplicacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricaoAplicacao.Location = new System.Drawing.Point(170, 31);
            this.lblDescricaoAplicacao.Name = "lblDescricaoAplicacao";
            this.lblDescricaoAplicacao.Size = new System.Drawing.Size(216, 17);
            this.lblDescricaoAplicacao.TabIndex = 2;
            this.lblDescricaoAplicacao.Text = "Monitor da Nota Fiscal Eletrônica";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(222, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Desenvolvido por:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Autorização de utilização:";
            // 
            // textBox_licenca
            // 
            this.textBox_licenca.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_licenca.Location = new System.Drawing.Point(13, 223);
            this.textBox_licenca.Multiline = true;
            this.textBox_licenca.Name = "textBox_licenca";
            this.textBox_licenca.ReadOnly = true;
            this.textBox_licenca.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_licenca.Size = new System.Drawing.Size(534, 138);
            this.textBox_licenca.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Data da última modificação:";
            // 
            // textBox_DataUltimaModificacao
            // 
            this.textBox_DataUltimaModificacao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_DataUltimaModificacao.Location = new System.Drawing.Point(13, 179);
            this.textBox_DataUltimaModificacao.Name = "textBox_DataUltimaModificacao";
            this.textBox_DataUltimaModificacao.ReadOnly = true;
            this.textBox_DataUltimaModificacao.Size = new System.Drawing.Size(169, 20);
            this.textBox_DataUltimaModificacao.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Versão:";
            // 
            // textBox_versao
            // 
            this.textBox_versao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_versao.Location = new System.Drawing.Point(225, 178);
            this.textBox_versao.Name = "textBox_versao";
            this.textBox_versao.ReadOnly = true;
            this.textBox_versao.Size = new System.Drawing.Size(221, 20);
            this.textBox_versao.TabIndex = 14;
            // 
            // btnManualUniNFe
            // 
            this.btnManualUniNFe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualUniNFe.Image = global::NFe.Interface.Properties.Resources.pdf3;
            this.btnManualUniNFe.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnManualUniNFe.Location = new System.Drawing.Point(472, 142);
            this.btnManualUniNFe.Name = "btnManualUniNFe";
            this.btnManualUniNFe.Size = new System.Drawing.Size(75, 75);
            this.btnManualUniNFe.TabIndex = 15;
            this.btnManualUniNFe.Text = "Manual";
            this.btnManualUniNFe.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManualUniNFe.UseVisualStyleBackColor = true;
            this.btnManualUniNFe.Click += new System.EventHandler(this.btnManualUniNFe_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NFe.Interface.Properties.Resources.uninfe128;
            this.pictureBox1.Location = new System.Drawing.Point(3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // linkLabelSiteProduto
            // 
            this.linkLabelSiteProduto.AutoSize = true;
            this.linkLabelSiteProduto.Location = new System.Drawing.Point(222, 122);
            this.linkLabelSiteProduto.Name = "linkLabelSiteProduto";
            this.linkLabelSiteProduto.Size = new System.Drawing.Size(143, 13);
            this.linkLabelSiteProduto.TabIndex = 19;
            this.linkLabelSiteProduto.TabStop = true;
            this.linkLabelSiteProduto.Text = "www.unimake.com.br/uninfe";
            this.linkLabelSiteProduto.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSiteProduto_LinkClicked);
            // 
            // linkLabelEmail
            // 
            this.linkLabelEmail.AutoSize = true;
            this.linkLabelEmail.Location = new System.Drawing.Point(222, 135);
            this.linkLabelEmail.Name = "linkLabelEmail";
            this.linkLabelEmail.Size = new System.Drawing.Size(108, 13);
            this.linkLabelEmail.TabIndex = 18;
            this.linkLabelEmail.TabStop = true;
            this.linkLabelEmail.Text = "nfe@unimake.com.br";
            // 
            // linkLabelSite
            // 
            this.linkLabelSite.AutoSize = true;
            this.linkLabelSite.Location = new System.Drawing.Point(222, 97);
            this.linkLabelSite.Name = "linkLabelSite";
            this.linkLabelSite.Size = new System.Drawing.Size(109, 13);
            this.linkLabelSite.TabIndex = 17;
            this.linkLabelSite.TabStop = true;
            this.linkLabelSite.Text = "www.unimake.com.br";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.Location = new System.Drawing.Point(222, 84);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(99, 13);
            this.lblEmpresa.TabIndex = 16;
            this.lblEmpresa.Text = "Unimake Softwares";
            // 
            // FormSobre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 373);
            this.Controls.Add(this.linkLabelSiteProduto);
            this.Controls.Add(this.linkLabelEmail);
            this.Controls.Add(this.linkLabelSite);
            this.Controls.Add(this.lblEmpresa);
            this.Controls.Add(this.btnManualUniNFe);
            this.Controls.Add(this.textBox_versao);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_DataUltimaModificacao);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_licenca);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDescricaoAplicacao);
            this.Controls.Add(this.lblNomeAplicacao);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormSobre";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sobre";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblNomeAplicacao;
        private System.Windows.Forms.Label lblDescricaoAplicacao;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_licenca;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_DataUltimaModificacao;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_versao;
        private System.Windows.Forms.Button btnManualUniNFe;
        private System.Windows.Forms.LinkLabel linkLabelSiteProduto;
        private System.Windows.Forms.LinkLabel linkLabelEmail;
        private System.Windows.Forms.LinkLabel linkLabelSite;
        private System.Windows.Forms.Label lblEmpresa;

    }
}
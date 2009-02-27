namespace uninfe
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_config = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_StatusServicoNfe = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_sobre = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_teste = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_validarxml = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel_NomeEmpresa = new System.Windows.Forms.ToolStripLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_config,
            this.toolStripButton_StatusServicoNfe,
            this.toolStripButton_sobre,
            this.toolStripButton_teste,
            this.toolStripButton_validarxml,
            this.toolStripLabel_NomeEmpresa});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 55);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            // 
            // toolStripButton_config
            // 
            this.toolStripButton_config.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_config.Image = global::uninfe.Properties.Resources.gear;
            this.toolStripButton_config.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_config.Name = "toolStripButton_config";
            this.toolStripButton_config.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_config.Text = "toolStripButton1";
            this.toolStripButton_config.ToolTipText = "Configurações do aplicativo";
            this.toolStripButton_config.Click += new System.EventHandler(this.toolStripButton_config_Click);
            // 
            // toolStripButton_StatusServicoNfe
            // 
            this.toolStripButton_StatusServicoNfe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_StatusServicoNfe.Image = global::uninfe.Properties.Resources.kontact_todo;
            this.toolStripButton_StatusServicoNfe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StatusServicoNfe.Name = "toolStripButton_StatusServicoNfe";
            this.toolStripButton_StatusServicoNfe.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_StatusServicoNfe.Text = "toolStripButton1";
            this.toolStripButton_StatusServicoNfe.ToolTipText = "Verifica a situação do serviço da Nota Fiscal Eletrônica";
            this.toolStripButton_StatusServicoNfe.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton_sobre
            // 
            this.toolStripButton_sobre.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_sobre.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_sobre.Image = global::uninfe.Properties.Resources.uninfe;
            this.toolStripButton_sobre.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_sobre.Name = "toolStripButton_sobre";
            this.toolStripButton_sobre.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_sobre.Text = "toolStripButton1";
            this.toolStripButton_sobre.ToolTipText = "Sobre o UniNFe";
            this.toolStripButton_sobre.Click += new System.EventHandler(this.toolStripButton_sobre_Click);
            // 
            // toolStripButton_teste
            // 
            this.toolStripButton_teste.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_teste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_teste.Image = global::uninfe.Properties.Resources.package_utilities;
            this.toolStripButton_teste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_teste.Name = "toolStripButton_teste";
            this.toolStripButton_teste.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_teste.Text = "toolStripButton2";
            this.toolStripButton_teste.ToolTipText = "Rotina para testes diversos com a NFe";
            this.toolStripButton_teste.Visible = false;
            this.toolStripButton_teste.Click += new System.EventHandler(this.toolStripButton_teste_Click);
            // 
            // toolStripButton_validarxml
            // 
            this.toolStripButton_validarxml.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_validarxml.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_validarxml.Image")));
            this.toolStripButton_validarxml.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_validarxml.Name = "toolStripButton_validarxml";
            this.toolStripButton_validarxml.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_validarxml.Text = "toolStripButton_validarxml";
            this.toolStripButton_validarxml.ToolTipText = "Validar os arquivos XML";
            this.toolStripButton_validarxml.Click += new System.EventHandler(this.toolStripButton_validarxml_Click);
            // 
            // toolStripLabel_NomeEmpresa
            // 
            this.toolStripLabel_NomeEmpresa.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel_NomeEmpresa.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripLabel_NomeEmpresa.Name = "toolStripLabel_NomeEmpresa";
            this.toolStripLabel_NomeEmpresa.Size = new System.Drawing.Size(86, 52);
            this.toolStripLabel_NomeEmpresa.Text = "toolStripLabel1";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Para abrir novamente o UniNFE, de um duplo clique sobre o ícone.";
            this.notifyIcon1.BalloonTipTitle = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 564);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_config;
        private System.Windows.Forms.ToolStripButton toolStripButton_teste;
        private System.Windows.Forms.ToolStripButton toolStripButton_StatusServicoNfe;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripButton toolStripButton_sobre;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_NomeEmpresa;
        private System.Windows.Forms.ToolStripButton toolStripButton_validarxml;
    }
}




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
            this.toolStripButton_validarxml = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_sobre = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnUpdate = new System.Windows.Forms.ToolStripButton();
            this.tbLogs = new System.Windows.Forms.ToolStripButton();
            this.tsPrintDanfe = new System.Windows.Forms.ToolStripButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tbPararServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRestartServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmConsultaCadastroServico = new System.Windows.Forms.ToolStripMenuItem();
            this.configuraçõesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.vaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreOUniNFeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_config,
            this.toolStripButton_StatusServicoNfe,
            this.toolStripButton_validarxml,
            this.toolStripButton_sobre,
            this.toolStripBtnUpdate,
            this.tbLogs,
            this.tsPrintDanfe});
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
            this.toolStripButton_StatusServicoNfe.ToolTipText = "Consultar Situação do Servidor e Cadastro de Contribuinte";
            this.toolStripButton_StatusServicoNfe.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton_validarxml
            // 
            this.toolStripButton_validarxml.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_validarxml.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_validarxml.Image")));
            this.toolStripButton_validarxml.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_validarxml.Name = "toolStripButton_validarxml";
            this.toolStripButton_validarxml.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_validarxml.Text = "toolStripButton_validarxml";
            this.toolStripButton_validarxml.ToolTipText = "Validar arquivos XML";
            this.toolStripButton_validarxml.Click += new System.EventHandler(this.toolStripButton_validarxml_Click);
            // 
            // toolStripButton_sobre
            // 
            this.toolStripButton_sobre.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_sobre.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_sobre.Image = global::uninfe.Properties.Resources.uninfe128;
            this.toolStripButton_sobre.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_sobre.Name = "toolStripButton_sobre";
            this.toolStripButton_sobre.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_sobre.Text = "toolStripButton1";
            this.toolStripButton_sobre.ToolTipText = "Sobre o UniNFe";
            this.toolStripButton_sobre.Click += new System.EventHandler(this.toolStripButton_sobre_Click);
            // 
            // toolStripBtnUpdate
            // 
            this.toolStripBtnUpdate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripBtnUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnUpdate.Image = global::uninfe.Properties.Resources.update48x48;
            this.toolStripBtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnUpdate.Name = "toolStripBtnUpdate";
            this.toolStripBtnUpdate.Size = new System.Drawing.Size(52, 52);
            this.toolStripBtnUpdate.Text = "toolStripButton1";
            this.toolStripBtnUpdate.ToolTipText = "Atualizar o aplicativo";
            this.toolStripBtnUpdate.Click += new System.EventHandler(this.toolStripBtnUpdate_Click);
            // 
            // tbLogs
            // 
            this.tbLogs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbLogs.Image = ((System.Drawing.Image)(resources.GetObject("tbLogs.Image")));
            this.tbLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbLogs.Name = "tbLogs";
            this.tbLogs.Size = new System.Drawing.Size(52, 52);
            this.tbLogs.Text = "Logs";
            this.tbLogs.ToolTipText = "Visualiza os Logs";
            this.tbLogs.Click += new System.EventHandler(this.tbLogs_Click);
            // 
            // tsPrintDanfe
            // 
            this.tsPrintDanfe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPrintDanfe.Image = ((System.Drawing.Image)(resources.GetObject("tsPrintDanfe.Image")));
            this.tsPrintDanfe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPrintDanfe.Name = "tsPrintDanfe";
            this.tsPrintDanfe.Size = new System.Drawing.Size(52, 52);
            this.tsPrintDanfe.Text = "Impressão do DANFE/CCe";
            this.tsPrintDanfe.Click += new System.EventHandler(this.tsPrintDanfe_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Para abrir novamente o UniNFE, de um duplo clique ou pressione o botão direito do" +
    " mouse sobre o ícone.";
            this.notifyIcon1.BalloonTipTitle = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPararServico,
            this.tbRestartServico,
            this.tbSeparator1,
            this.toolStripMenuItem1,
            this.toolStripSeparator2,
            this.cmConsultaCadastroServico,
            this.configuraçõesToolStripMenuItem1,
            this.vaToolStripMenuItem,
            this.toolStripSeparator3,
            this.toolStripMenuItem3,
            this.toolStripMenuItem2,
            this.sobreOUniNFeToolStripMenuItem,
            this.toolStripSeparator1,
            this.sairToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(381, 248);
            // 
            // tbPararServico
            // 
            this.tbPararServico.Name = "tbPararServico";
            this.tbPararServico.Size = new System.Drawing.Size(380, 22);
            this.tbPararServico.Text = "Parar o serviço";
            this.tbPararServico.Click += new System.EventHandler(this.tbPararServico_Click);
            // 
            // tbRestartServico
            // 
            this.tbRestartServico.Name = "tbRestartServico";
            this.tbRestartServico.Size = new System.Drawing.Size(380, 22);
            this.tbRestartServico.Text = "Reiniciar o serviço";
            this.tbRestartServico.Click += new System.EventHandler(this.tbRestartServico_Click);
            // 
            // tbSeparator1
            // 
            this.tbSeparator1.Name = "tbSeparator1";
            this.tbSeparator1.Size = new System.Drawing.Size(377, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(380, 22);
            this.toolStripMenuItem1.Text = "Abrir UniNFe";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(377, 6);
            // 
            // cmConsultaCadastroServico
            // 
            this.cmConsultaCadastroServico.Name = "cmConsultaCadastroServico";
            this.cmConsultaCadastroServico.Size = new System.Drawing.Size(380, 22);
            this.cmConsultaCadastroServico.Text = "Consultar situação dos serviços e cadastro de contribuinte";
            this.cmConsultaCadastroServico.Click += new System.EventHandler(this.cmConsultaCadastroServico_Click);
            // 
            // configuraçõesToolStripMenuItem1
            // 
            this.configuraçõesToolStripMenuItem1.Name = "configuraçõesToolStripMenuItem1";
            this.configuraçõesToolStripMenuItem1.Size = new System.Drawing.Size(380, 22);
            this.configuraçõesToolStripMenuItem1.Text = "Configurações";
            this.configuraçõesToolStripMenuItem1.Click += new System.EventHandler(this.configuraçõesToolStripMenuItem1_Click);
            // 
            // vaToolStripMenuItem
            // 
            this.vaToolStripMenuItem.Name = "vaToolStripMenuItem";
            this.vaToolStripMenuItem.Size = new System.Drawing.Size(380, 22);
            this.vaToolStripMenuItem.Text = "Validar arquivos XML";
            this.vaToolStripMenuItem.Click += new System.EventHandler(this.vaToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(377, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(380, 22);
            this.toolStripMenuItem3.Text = "Logs";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(380, 22);
            this.toolStripMenuItem2.Text = "Manual do UniNFe";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // sobreOUniNFeToolStripMenuItem
            // 
            this.sobreOUniNFeToolStripMenuItem.Name = "sobreOUniNFeToolStripMenuItem";
            this.sobreOUniNFeToolStripMenuItem.Size = new System.Drawing.Size(380, 22);
            this.sobreOUniNFeToolStripMenuItem.Text = "Sobre o UniNFe";
            this.sobreOUniNFeToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_sobre_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(377, 6);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(380, 22);
            this.sairToolStripMenuItem.Text = "Fechar o UniNFe";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_config;
        private System.Windows.Forms.ToolStripButton toolStripButton_StatusServicoNfe;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripButton toolStripButton_sobre;
        private System.Windows.Forms.ToolStripButton toolStripButton_validarxml;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmConsultaCadastroServico;
        private System.Windows.Forms.ToolStripMenuItem vaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configuraçõesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sobreOUniNFeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripButton toolStripBtnUpdate;
        private System.Windows.Forms.ToolStripMenuItem tbPararServico;
        private System.Windows.Forms.ToolStripMenuItem tbRestartServico;
        private System.Windows.Forms.ToolStripSeparator tbSeparator1;
        private System.Windows.Forms.ToolStripButton tbLogs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripButton tsPrintDanfe;
    }
}




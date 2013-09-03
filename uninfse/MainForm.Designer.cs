namespace uninfse
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
            this.toolStripButton_validarxml = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_sobre = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMunicipios = new System.Windows.Forms.ToolStripButton();
            this.tbLogs = new System.Windows.Forms.ToolStripButton();
            this.tbClearLockFiles = new System.Windows.Forms.ToolStripButton();
            this.tbForceUpdateWSDL = new System.Windows.Forms.ToolStripButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tbPararServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRestartServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
            this.toolStripButton_validarxml,
            this.toolStripButton_sobre,
            this.toolStripBtnUpdate,
            this.toolStripSeparator4,
            this.btnMunicipios,
            this.tbLogs,
            this.tbClearLockFiles,
            this.tbForceUpdateWSDL});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 55);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            // 
            // toolStripButton_config
            // 
            this.toolStripButton_config.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_config.Image = global::uninfse.Properties.Resources.gear;
            this.toolStripButton_config.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_config.Name = "toolStripButton_config";
            this.toolStripButton_config.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_config.Text = "toolStripButton1";
            this.toolStripButton_config.ToolTipText = "Configurações do aplicativo";
            this.toolStripButton_config.Click += new System.EventHandler(this.toolStripButton_config_Click);
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
            this.toolStripButton_sobre.Image = global::uninfse.Properties.Resources.uninfse128;
            this.toolStripButton_sobre.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_sobre.Name = "toolStripButton_sobre";
            this.toolStripButton_sobre.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton_sobre.Text = "toolStripButton1";
            this.toolStripButton_sobre.ToolTipText = "Sobre o UniNFSe";
            this.toolStripButton_sobre.Click += new System.EventHandler(this.toolStripButton_sobre_Click);
            // 
            // toolStripBtnUpdate
            // 
            this.toolStripBtnUpdate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripBtnUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnUpdate.Image = global::uninfse.Properties.Resources.update48x48;
            this.toolStripBtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnUpdate.Name = "toolStripBtnUpdate";
            this.toolStripBtnUpdate.Size = new System.Drawing.Size(52, 52);
            this.toolStripBtnUpdate.Text = "toolStripButton1";
            this.toolStripBtnUpdate.ToolTipText = "Atualizar o aplicativo";
            this.toolStripBtnUpdate.Click += new System.EventHandler(this.toolStripBtnUpdate_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 55);
            // 
            // btnMunicipios
            // 
            this.btnMunicipios.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMunicipios.Image = global::uninfse.Properties.Resources.city48x48;
            this.btnMunicipios.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMunicipios.Name = "btnMunicipios";
            this.btnMunicipios.Size = new System.Drawing.Size(52, 52);
            this.btnMunicipios.Text = "Municípios";
            this.btnMunicipios.Click += new System.EventHandler(this.btnMunicipios_Click);
            // 
            // tbLogs
            // 
            this.tbLogs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbLogs.Image = ((System.Drawing.Image)(resources.GetObject("tbLogs.Image")));
            this.tbLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbLogs.Name = "tbLogs";
            this.tbLogs.Size = new System.Drawing.Size(52, 52);
            this.tbLogs.Text = "toolStripButton1";
            this.tbLogs.ToolTipText = "Visualiza os logs";
            this.tbLogs.Click += new System.EventHandler(this.tbLogs_Click);
            // 
            // tbClearLockFiles
            // 
            this.tbClearLockFiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbClearLockFiles.Image = ((System.Drawing.Image)(resources.GetObject("tbClearLockFiles.Image")));
            this.tbClearLockFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbClearLockFiles.Name = "tbClearLockFiles";
            this.tbClearLockFiles.Size = new System.Drawing.Size(52, 52);
            this.tbClearLockFiles.Text = "Excluir arquivos do tipo \".lock\"";
            this.tbClearLockFiles.Click += new System.EventHandler(this.tbClearLockFiles_Click);
            // 
            // tbForceUpdateWSDL
            // 
            this.tbForceUpdateWSDL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbForceUpdateWSDL.Image = ((System.Drawing.Image)(resources.GetObject("tbForceUpdateWSDL.Image")));
            this.tbForceUpdateWSDL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbForceUpdateWSDL.Name = "tbForceUpdateWSDL";
            this.tbForceUpdateWSDL.Size = new System.Drawing.Size(52, 52);
            this.tbForceUpdateWSDL.Text = "Forçar atualização de WSDLs e Schemas";
            this.tbForceUpdateWSDL.Click += new System.EventHandler(this.tbForceUpdateWSDL_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Para abrir novamente o UniNFSe, de um duplo clique ou pressione o botão direito d" +
                "o mouse sobre o ícone.";
            this.notifyIcon1.BalloonTipTitle = "UniNFSe - Monitor de Notas Fiscais de Serviços Eletrônicas";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "UniNFSe - Monitor de Notas Fiscais de Serviços Eletrônicas";
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
            this.configuraçõesToolStripMenuItem1,
            this.vaToolStripMenuItem,
            this.toolStripSeparator3,
            this.toolStripMenuItem3,
            this.toolStripMenuItem2,
            this.sobreOUniNFeToolStripMenuItem,
            this.toolStripSeparator1,
            this.sairToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(186, 226);
            // 
            // tbPararServico
            // 
            this.tbPararServico.Name = "tbPararServico";
            this.tbPararServico.Size = new System.Drawing.Size(185, 22);
            this.tbPararServico.Text = "Parar o serviço";
            this.tbPararServico.Click += new System.EventHandler(this.tbPararServico_Click);
            // 
            // tbRestartServico
            // 
            this.tbRestartServico.Name = "tbRestartServico";
            this.tbRestartServico.Size = new System.Drawing.Size(185, 22);
            this.tbRestartServico.Text = "Reiniciar o serviço";
            this.tbRestartServico.Click += new System.EventHandler(this.tbRestartServico_Click);
            // 
            // tbSeparator1
            // 
            this.tbSeparator1.Name = "tbSeparator1";
            this.tbSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.toolStripMenuItem1.Text = "Abrir UniNFSe";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // configuraçõesToolStripMenuItem1
            // 
            this.configuraçõesToolStripMenuItem1.Name = "configuraçõesToolStripMenuItem1";
            this.configuraçõesToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.configuraçõesToolStripMenuItem1.Text = "Configurações";
            this.configuraçõesToolStripMenuItem1.Click += new System.EventHandler(this.configuraçõesToolStripMenuItem1_Click);
            // 
            // vaToolStripMenuItem
            // 
            this.vaToolStripMenuItem.Name = "vaToolStripMenuItem";
            this.vaToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.vaToolStripMenuItem.Text = "Validar arquivos XML";
            this.vaToolStripMenuItem.Click += new System.EventHandler(this.vaToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(185, 22);
            this.toolStripMenuItem3.Text = "Logs";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(185, 22);
            this.toolStripMenuItem2.Text = "Manual do UniNFSe";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // sobreOUniNFeToolStripMenuItem
            // 
            this.sobreOUniNFeToolStripMenuItem.Name = "sobreOUniNFeToolStripMenuItem";
            this.sobreOUniNFeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.sobreOUniNFeToolStripMenuItem.Text = "Sobre o UniNFSe";
            this.sobreOUniNFeToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_sobre_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.sairToolStripMenuItem.Text = "Fechar o UniNFSe";
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
            this.Text = "UniNFSe - Monitor de Notas Fiscais de Serviços Eletrônicas";
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
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripButton toolStripButton_sobre;
        private System.Windows.Forms.ToolStripButton toolStripButton_validarxml;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
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

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnMunicipios;
        private System.Windows.Forms.ToolStripButton tbLogs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripButton tbClearLockFiles;
        private System.Windows.Forms.ToolStripButton tbForceUpdateWSDL;
    }
}




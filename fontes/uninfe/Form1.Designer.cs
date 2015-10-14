namespace uninfe2
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tbPararServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRestartServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmAbrir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmConfiguracoes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmSituacaoServicos = new System.Windows.Forms.ToolStripMenuItem();
            this.cmConsultaCadastro = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDANFE = new System.Windows.Forms.ToolStripMenuItem();
            this.cmValidarXML = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.cmManual = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSobre = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmFechar = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            this.metroStyleManager1.Style = "Orange";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPararServico,
            this.tbRestartServico,
            this.tbSeparator1,
            this.cmAbrir,
            this.toolStripSeparator2,
            this.cmConfiguracoes,
            this.toolStripSeparator4,
            this.cmSituacaoServicos,
            this.cmConsultaCadastro,
            this.cmDANFE,
            this.cmValidarXML,
            this.toolStripSeparator3,
            this.cmLogs,
            this.cmManual,
            this.cmSobre,
            this.toolStripSeparator1,
            this.cmFechar});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(381, 320);
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
            // cmAbrir
            // 
            this.cmAbrir.Name = "cmAbrir";
            this.cmAbrir.Size = new System.Drawing.Size(380, 22);
            this.cmAbrir.Text = "Abrir UniNFe";
            this.cmAbrir.Click += new System.EventHandler(this.cmAbrir_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(377, 6);
            // 
            // cmConfiguracoes
            // 
            this.cmConfiguracoes.Name = "cmConfiguracoes";
            this.cmConfiguracoes.Size = new System.Drawing.Size(380, 22);
            this.cmConfiguracoes.Text = "Configurações";
            this.cmConfiguracoes.Click += new System.EventHandler(this.cmConfiguracoes_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(377, 6);
            // 
            // cmSituacaoServicos
            // 
            this.cmSituacaoServicos.Name = "cmSituacaoServicos";
            this.cmSituacaoServicos.Size = new System.Drawing.Size(380, 22);
            this.cmSituacaoServicos.Text = "Consultar situação dos serviços";
            this.cmSituacaoServicos.Click += new System.EventHandler(this.cmSituacaoServicos_Click);
            // 
            // cmConsultaCadastro
            // 
            this.cmConsultaCadastro.Name = "cmConsultaCadastro";
            this.cmConsultaCadastro.Size = new System.Drawing.Size(380, 22);
            this.cmConsultaCadastro.Text = "Consultar situação dos serviços e cadastro de contribuinte";
            this.cmConsultaCadastro.Click += new System.EventHandler(this.cmConsultaCadastro_Click);
            // 
            // cmDANFE
            // 
            this.cmDANFE.Name = "cmDANFE";
            this.cmDANFE.Size = new System.Drawing.Size(380, 22);
            this.cmDANFE.Text = "Imprimir DANFE/DAMFe/DACTe/CCe";
            this.cmDANFE.Click += new System.EventHandler(this.cmDANFE_Click);
            // 
            // cmValidarXML
            // 
            this.cmValidarXML.Name = "cmValidarXML";
            this.cmValidarXML.Size = new System.Drawing.Size(380, 22);
            this.cmValidarXML.Text = "Validar arquivos XML";
            this.cmValidarXML.Click += new System.EventHandler(this.cmValidarXML_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(377, 6);
            // 
            // cmLogs
            // 
            this.cmLogs.Name = "cmLogs";
            this.cmLogs.Size = new System.Drawing.Size(380, 22);
            this.cmLogs.Text = "Logs";
            this.cmLogs.Click += new System.EventHandler(this.cmLogs_Click);
            // 
            // cmManual
            // 
            this.cmManual.Name = "cmManual";
            this.cmManual.Size = new System.Drawing.Size(380, 22);
            this.cmManual.Text = "Manual do UniNFe";
            this.cmManual.Click += new System.EventHandler(this.cmManual_Click);
            // 
            // cmSobre
            // 
            this.cmSobre.Name = "cmSobre";
            this.cmSobre.Size = new System.Drawing.Size(380, 22);
            this.cmSobre.Text = "Sobre o UniNFe";
            this.cmSobre.Click += new System.EventHandler(this.cmSobre_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(377, 6);
            // 
            // cmFechar
            // 
            this.cmFechar.Name = "cmFechar";
            this.cmFechar.Size = new System.Drawing.Size(380, 22);
            this.cmFechar.Text = "Fechar o UniNFe";
            this.cmFechar.Click += new System.EventHandler(this.cmFechar_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Para abrir novamente o {0}, de um duplo clique ou pressione o botão direito do mo" +
    "use sobre o ícone.";
            this.notifyIcon1.BalloonTipTitle = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "UniNFE - Monitor da Nota Fiscal Eletrônica";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "UniNFe";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tbPararServico;
        private System.Windows.Forms.ToolStripMenuItem tbRestartServico;
        private System.Windows.Forms.ToolStripSeparator tbSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmAbrir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmConsultaCadastro;
        private System.Windows.Forms.ToolStripMenuItem cmConfiguracoes;
        private System.Windows.Forms.ToolStripMenuItem cmValidarXML;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmLogs;
        private System.Windows.Forms.ToolStripMenuItem cmManual;
        private System.Windows.Forms.ToolStripMenuItem cmSobre;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmFechar;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem cmDANFE;
        private System.Windows.Forms.ToolStripMenuItem cmSituacaoServicos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}


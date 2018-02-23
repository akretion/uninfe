namespace NFe.UI
{
    partial class Form_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.tbPararServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRestartServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmAbrir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmConfiguracoes = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMunicipios = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmSituacaoServicos = new System.Windows.Forms.ToolStripMenuItem();
            this.cmConsultaCadastro = new System.Windows.Forms.ToolStripMenuItem();
            this.cmValidarXML = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.cmManual = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSobre = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmFechar = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // tbPararServico
            // 
            this.tbPararServico.Name = "tbPararServico";
            this.tbPararServico.Size = new System.Drawing.Size(257, 22);
            this.tbPararServico.Text = "Parar o serviço";
            this.tbPararServico.Click += new System.EventHandler(this.tbPararServico_Click);
            // 
            // tbRestartServico
            // 
            this.tbRestartServico.Name = "tbRestartServico";
            this.tbRestartServico.Size = new System.Drawing.Size(257, 22);
            this.tbRestartServico.Text = "Reiniciar o serviço";
            this.tbRestartServico.Click += new System.EventHandler(this.tbRestartServico_Click);
            // 
            // tbSeparator1
            // 
            this.tbSeparator1.Name = "tbSeparator1";
            this.tbSeparator1.Size = new System.Drawing.Size(254, 6);
            // 
            // cmAbrir
            // 
            this.cmAbrir.Name = "cmAbrir";
            this.cmAbrir.Size = new System.Drawing.Size(257, 22);
            this.cmAbrir.Text = "Abrir UniNFe";
            this.cmAbrir.Click += new System.EventHandler(this.cmAbrir_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(254, 6);
            // 
            // cmConfiguracoes
            // 
            this.cmConfiguracoes.Name = "cmConfiguracoes";
            this.cmConfiguracoes.Size = new System.Drawing.Size(257, 22);
            this.cmConfiguracoes.Text = "Configurações";
            this.cmConfiguracoes.Click += new System.EventHandler(this.cmConfiguracoes_Click);
            // 
            // cmMunicipios
            // 
            this.cmMunicipios.Name = "cmMunicipios";
            this.cmMunicipios.Size = new System.Drawing.Size(257, 22);
            this.cmMunicipios.Text = "Municipios";
            this.cmMunicipios.Click += new System.EventHandler(this.cmMunicipios_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(254, 6);
            // 
            // cmSituacaoServicos
            // 
            this.cmSituacaoServicos.Name = "cmSituacaoServicos";
            this.cmSituacaoServicos.Size = new System.Drawing.Size(257, 22);
            this.cmSituacaoServicos.Text = "Consultar situação dos serviços";
            this.cmSituacaoServicos.Click += new System.EventHandler(this.cmSituacaoServicos_Click);
            // 
            // cmConsultaCadastro
            // 
            this.cmConsultaCadastro.Name = "cmConsultaCadastro";
            this.cmConsultaCadastro.Size = new System.Drawing.Size(257, 22);
            this.cmConsultaCadastro.Text = "Consultar cadastro de contribuinte";
            this.cmConsultaCadastro.Click += new System.EventHandler(this.cmConsultaCadastro_Click);
            // 
            // cmValidarXML
            // 
            this.cmValidarXML.Name = "cmValidarXML";
            this.cmValidarXML.Size = new System.Drawing.Size(257, 22);
            this.cmValidarXML.Text = "Validar arquivos XML";
            this.cmValidarXML.Click += new System.EventHandler(this.cmValidarXML_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(254, 6);
            // 
            // cmLogs
            // 
            this.cmLogs.Name = "cmLogs";
            this.cmLogs.Size = new System.Drawing.Size(257, 22);
            this.cmLogs.Text = "Logs";
            this.cmLogs.Click += new System.EventHandler(this.cmLogs_Click);
            // 
            // cmManual
            // 
            this.cmManual.Name = "cmManual";
            this.cmManual.Size = new System.Drawing.Size(257, 22);
            this.cmManual.Text = "Manual do UniNFe";
            this.cmManual.Click += new System.EventHandler(this.cmManual_Click);
            // 
            // cmSobre
            // 
            this.cmSobre.Name = "cmSobre";
            this.cmSobre.Size = new System.Drawing.Size(257, 22);
            this.cmSobre.Text = "Sobre o UniNFe";
            this.cmSobre.Click += new System.EventHandler(this.cmSobre_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(254, 6);
            // 
            // cmFechar
            // 
            this.cmFechar.Name = "cmFechar";
            this.cmFechar.Size = new System.Drawing.Size(257, 22);
            this.cmFechar.Text = "Fechar o UniNFe";
            this.cmFechar.Click += new System.EventHandler(this.cmFechar_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
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
            this.cmMunicipios,
            this.toolStripSeparator4,
            this.cmSituacaoServicos,
            this.cmConsultaCadastro,
            this.cmValidarXML,
            this.toolStripSeparator3,
            this.cmLogs,
            this.cmManual,
            this.cmSobre,
            this.toolStripSeparator1,
            this.cmFechar});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(258, 298);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.metroContextMenu1_Opening);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::NFe.UI.Properties.Resources.uninfe;
            this.pictureBox1.Location = new System.Drawing.Point(18, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 50);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImagePadding = new System.Windows.Forms.Padding(210, 10, 0, 0);
            this.BackMaxSize = 50;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form_Main";
            this.Padding = new System.Windows.Forms.Padding(15, 60, 15, 15);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "UniNFe";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.Form_Main_ControlRemoved);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroToolTip metroToolTip1;
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
//        private System.Windows.Forms.ToolStripMenuItem cmDANFE;
        private System.Windows.Forms.ToolStripMenuItem cmSituacaoServicos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmMunicipios;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        public MetroFramework.Components.MetroStyleManager metroStyleManager1;
    }
}


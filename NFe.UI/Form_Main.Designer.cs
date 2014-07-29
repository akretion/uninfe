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
            this.cmDANFE = new System.Windows.Forms.ToolStripMenuItem();
            this.cmValidarXML = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.cmManual = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSobre = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmFechar = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.metroContextMenu1 = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.pnlOptions = new MetroFramework.Controls.MetroPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroTile_back = new MetroFramework.Controls.MetroTile();
            this.mtBlue = new MetroFramework.Controls.MetroTile();
            this.mtBlack = new MetroFramework.Controls.MetroTile();
            this.mtMarrom = new MetroFramework.Controls.MetroTile();
            this.mtPink = new MetroFramework.Controls.MetroTile();
            this.mtSilver = new MetroFramework.Controls.MetroTile();
            this.mtMagenta = new MetroFramework.Controls.MetroTile();
            this.mtGreen = new MetroFramework.Controls.MetroTile();
            this.mtRed = new MetroFramework.Controls.MetroTile();
            this.mtPurple = new MetroFramework.Controls.MetroTile();
            this.mtOrange = new MetroFramework.Controls.MetroTile();
            this.mtLime = new MetroFramework.Controls.MetroTile();
            this.mtWhite = new MetroFramework.Controls.MetroTile();
            this.mtTeal = new MetroFramework.Controls.MetroTile();
            this.mtYellow = new MetroFramework.Controls.MetroTile();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.metroContextMenu1.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Magenta;
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
            this.tbPararServico.Size = new System.Drawing.Size(248, 22);
            this.tbPararServico.Text = "Parar o serviço";
            this.tbPararServico.Click += new System.EventHandler(this.tbPararServico_Click);
            // 
            // tbRestartServico
            // 
            this.tbRestartServico.Name = "tbRestartServico";
            this.tbRestartServico.Size = new System.Drawing.Size(248, 22);
            this.tbRestartServico.Text = "Reiniciar o serviço";
            this.tbRestartServico.Click += new System.EventHandler(this.tbRestartServico_Click);
            // 
            // tbSeparator1
            // 
            this.tbSeparator1.Name = "tbSeparator1";
            this.tbSeparator1.Size = new System.Drawing.Size(245, 6);
            // 
            // cmAbrir
            // 
            this.cmAbrir.Name = "cmAbrir";
            this.cmAbrir.Size = new System.Drawing.Size(248, 22);
            this.cmAbrir.Text = "Abrir UniNFe";
            this.cmAbrir.Click += new System.EventHandler(this.cmAbrir_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(245, 6);
            // 
            // cmConfiguracoes
            // 
            this.cmConfiguracoes.Name = "cmConfiguracoes";
            this.cmConfiguracoes.Size = new System.Drawing.Size(248, 22);
            this.cmConfiguracoes.Text = "Configurações";
            this.cmConfiguracoes.Click += new System.EventHandler(this.cmConfiguracoes_Click);
            // 
            // cmMunicipios
            // 
            this.cmMunicipios.Name = "cmMunicipios";
            this.cmMunicipios.Size = new System.Drawing.Size(248, 22);
            this.cmMunicipios.Text = "Municipios";
            this.cmMunicipios.Click += new System.EventHandler(this.cmMunicipios_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(245, 6);
            // 
            // cmSituacaoServicos
            // 
            this.cmSituacaoServicos.Name = "cmSituacaoServicos";
            this.cmSituacaoServicos.Size = new System.Drawing.Size(248, 22);
            this.cmSituacaoServicos.Text = "Consultar situação dos serviços";
            this.cmSituacaoServicos.Click += new System.EventHandler(this.cmSituacaoServicos_Click);
            // 
            // cmConsultaCadastro
            // 
            this.cmConsultaCadastro.Name = "cmConsultaCadastro";
            this.cmConsultaCadastro.Size = new System.Drawing.Size(248, 22);
            this.cmConsultaCadastro.Text = "Consultar cadastro de contribuinte";
            this.cmConsultaCadastro.Click += new System.EventHandler(this.cmConsultaCadastro_Click);
            // 
            // cmDANFE
            // 
            this.cmDANFE.Name = "cmDANFE";
            this.cmDANFE.Size = new System.Drawing.Size(248, 22);
            this.cmDANFE.Text = "Imprimir DANFE/DAMFe/DACTe/CCe";
            this.cmDANFE.Click += new System.EventHandler(this.cmDANFE_Click);
            // 
            // cmValidarXML
            // 
            this.cmValidarXML.Name = "cmValidarXML";
            this.cmValidarXML.Size = new System.Drawing.Size(248, 22);
            this.cmValidarXML.Text = "Validar arquivos XML";
            this.cmValidarXML.Click += new System.EventHandler(this.cmValidarXML_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(245, 6);
            // 
            // cmLogs
            // 
            this.cmLogs.Name = "cmLogs";
            this.cmLogs.Size = new System.Drawing.Size(248, 22);
            this.cmLogs.Text = "Logs";
            this.cmLogs.Click += new System.EventHandler(this.cmLogs_Click);
            // 
            // cmManual
            // 
            this.cmManual.Name = "cmManual";
            this.cmManual.Size = new System.Drawing.Size(248, 22);
            this.cmManual.Text = "Manual do UniNFe";
            this.cmManual.Click += new System.EventHandler(this.cmManual_Click);
            // 
            // cmSobre
            // 
            this.cmSobre.Name = "cmSobre";
            this.cmSobre.Size = new System.Drawing.Size(248, 22);
            this.cmSobre.Text = "Sobre o UniNFe";
            this.cmSobre.Click += new System.EventHandler(this.cmSobre_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(245, 6);
            // 
            // cmFechar
            // 
            this.cmFechar.Name = "cmFechar";
            this.cmFechar.Size = new System.Drawing.Size(248, 22);
            this.cmFechar.Text = "Fechar o UniNFe";
            this.cmFechar.Click += new System.EventHandler(this.cmFechar_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.metroContextMenu1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // metroContextMenu1
            // 
            this.metroContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.cmDANFE,
            this.cmValidarXML,
            this.toolStripSeparator3,
            this.cmLogs,
            this.cmManual,
            this.cmSobre,
            this.toolStripSeparator1,
            this.cmFechar});
            this.metroContextMenu1.Name = "metroContextMenu1";
            this.metroContextMenu1.ShowImageMargin = false;
            this.metroContextMenu1.Size = new System.Drawing.Size(249, 320);
            this.metroContextMenu1.Opening += new System.ComponentModel.CancelEventHandler(this.metroContextMenu1_Opening);
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.Color.Transparent;
            this.pnlOptions.Controls.Add(this.pictureBox1);
            this.pnlOptions.Controls.Add(this.metroLabel2);
            this.pnlOptions.Controls.Add(this.metroTile_back);
            this.pnlOptions.Controls.Add(this.mtBlue);
            this.pnlOptions.Controls.Add(this.mtBlack);
            this.pnlOptions.Controls.Add(this.mtMarrom);
            this.pnlOptions.Controls.Add(this.mtPink);
            this.pnlOptions.Controls.Add(this.mtSilver);
            this.pnlOptions.Controls.Add(this.mtMagenta);
            this.pnlOptions.Controls.Add(this.mtGreen);
            this.pnlOptions.Controls.Add(this.mtRed);
            this.pnlOptions.Controls.Add(this.mtPurple);
            this.pnlOptions.Controls.Add(this.mtOrange);
            this.pnlOptions.Controls.Add(this.mtLime);
            this.pnlOptions.Controls.Add(this.mtWhite);
            this.pnlOptions.Controls.Add(this.mtTeal);
            this.pnlOptions.Controls.Add(this.mtYellow);
            this.pnlOptions.HorizontalScrollbarBarColor = true;
            this.pnlOptions.HorizontalScrollbarHighlightOnWheel = false;
            this.pnlOptions.HorizontalScrollbarSize = 10;
            this.pnlOptions.Location = new System.Drawing.Point(48, 393);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(716, 83);
            this.pnlOptions.TabIndex = 41;
            this.pnlOptions.VerticalScrollbarBarColor = true;
            this.pnlOptions.VerticalScrollbarHighlightOnWheel = false;
            this.pnlOptions.VerticalScrollbarSize = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::NFe.UI.Properties.Resources.BluePixel;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(716, 1);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 45;
            this.pictureBox1.TabStop = false;
            // 
            // metroLabel2
            // 
            this.metroLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel2.Location = new System.Drawing.Point(643, 28);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(35, 15);
            this.metroLabel2.TabIndex = 44;
            this.metroLabel2.Text = "Tema";
            // 
            // metroTile_back
            // 
            this.metroTile_back.ActiveControl = null;
            this.metroTile_back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTile_back.Location = new System.Drawing.Point(643, 48);
            this.metroTile_back.Name = "metroTile_back";
            this.metroTile_back.Size = new System.Drawing.Size(65, 30);
            this.metroTile_back.Style = MetroFramework.MetroColorStyle.Black;
            this.metroTile_back.TabIndex = 42;
            this.metroTile_back.Text = "Preto";
            this.metroTile_back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTile_back.UseSelectable = true;
            this.metroTile_back.Click += new System.EventHandler(this.metroTile_back_Click);
            // 
            // mtBlue
            // 
            this.mtBlue.ActiveControl = null;
            this.mtBlue.Location = new System.Drawing.Point(39, 46);
            this.mtBlue.Name = "mtBlue";
            this.mtBlue.Size = new System.Drawing.Size(30, 30);
            this.mtBlue.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtBlue.TabIndex = 23;
            this.mtBlue.UseSelectable = true;
            this.mtBlue.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtBlack
            // 
            this.mtBlack.ActiveControl = null;
            this.mtBlack.Location = new System.Drawing.Point(3, 46);
            this.mtBlack.Name = "mtBlack";
            this.mtBlack.Size = new System.Drawing.Size(30, 30);
            this.mtBlack.Style = MetroFramework.MetroColorStyle.Black;
            this.mtBlack.TabIndex = 26;
            this.mtBlack.UseSelectable = true;
            this.mtBlack.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtMarrom
            // 
            this.mtMarrom.ActiveControl = null;
            this.mtMarrom.Location = new System.Drawing.Point(183, 46);
            this.mtMarrom.Name = "mtMarrom";
            this.mtMarrom.Size = new System.Drawing.Size(30, 30);
            this.mtMarrom.Style = MetroFramework.MetroColorStyle.Brown;
            this.mtMarrom.TabIndex = 32;
            this.mtMarrom.UseSelectable = true;
            this.mtMarrom.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtPink
            // 
            this.mtPink.ActiveControl = null;
            this.mtPink.Location = new System.Drawing.Point(39, 10);
            this.mtPink.Name = "mtPink";
            this.mtPink.Size = new System.Drawing.Size(30, 30);
            this.mtPink.Style = MetroFramework.MetroColorStyle.Pink;
            this.mtPink.TabIndex = 25;
            this.mtPink.UseSelectable = true;
            this.mtPink.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtSilver
            // 
            this.mtSilver.ActiveControl = null;
            this.mtSilver.Location = new System.Drawing.Point(219, 46);
            this.mtSilver.Name = "mtSilver";
            this.mtSilver.Size = new System.Drawing.Size(30, 30);
            this.mtSilver.Style = MetroFramework.MetroColorStyle.Silver;
            this.mtSilver.TabIndex = 31;
            this.mtSilver.UseSelectable = true;
            this.mtSilver.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtMagenta
            // 
            this.mtMagenta.ActiveControl = null;
            this.mtMagenta.Location = new System.Drawing.Point(219, 10);
            this.mtMagenta.Name = "mtMagenta";
            this.mtMagenta.Size = new System.Drawing.Size(30, 30);
            this.mtMagenta.Style = MetroFramework.MetroColorStyle.Magenta;
            this.mtMagenta.TabIndex = 30;
            this.mtMagenta.UseSelectable = true;
            this.mtMagenta.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtGreen
            // 
            this.mtGreen.ActiveControl = null;
            this.mtGreen.Location = new System.Drawing.Point(111, 46);
            this.mtGreen.Name = "mtGreen";
            this.mtGreen.Size = new System.Drawing.Size(30, 30);
            this.mtGreen.Style = MetroFramework.MetroColorStyle.Green;
            this.mtGreen.TabIndex = 29;
            this.mtGreen.UseSelectable = true;
            this.mtGreen.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtRed
            // 
            this.mtRed.ActiveControl = null;
            this.mtRed.Location = new System.Drawing.Point(147, 46);
            this.mtRed.Name = "mtRed";
            this.mtRed.Size = new System.Drawing.Size(30, 30);
            this.mtRed.Style = MetroFramework.MetroColorStyle.Red;
            this.mtRed.TabIndex = 28;
            this.mtRed.UseSelectable = true;
            this.mtRed.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtPurple
            // 
            this.mtPurple.ActiveControl = null;
            this.mtPurple.Location = new System.Drawing.Point(75, 46);
            this.mtPurple.Name = "mtPurple";
            this.mtPurple.Size = new System.Drawing.Size(30, 30);
            this.mtPurple.Style = MetroFramework.MetroColorStyle.Purple;
            this.mtPurple.TabIndex = 27;
            this.mtPurple.UseSelectable = true;
            this.mtPurple.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtOrange
            // 
            this.mtOrange.ActiveControl = null;
            this.mtOrange.Location = new System.Drawing.Point(147, 10);
            this.mtOrange.Name = "mtOrange";
            this.mtOrange.Size = new System.Drawing.Size(30, 30);
            this.mtOrange.Style = MetroFramework.MetroColorStyle.Orange;
            this.mtOrange.TabIndex = 26;
            this.mtOrange.UseSelectable = true;
            this.mtOrange.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtLime
            // 
            this.mtLime.ActiveControl = null;
            this.mtLime.Location = new System.Drawing.Point(183, 10);
            this.mtLime.Name = "mtLime";
            this.mtLime.Size = new System.Drawing.Size(30, 30);
            this.mtLime.Style = MetroFramework.MetroColorStyle.Lime;
            this.mtLime.TabIndex = 25;
            this.mtLime.UseSelectable = true;
            this.mtLime.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtWhite
            // 
            this.mtWhite.ActiveControl = null;
            this.mtWhite.Location = new System.Drawing.Point(111, 10);
            this.mtWhite.Name = "mtWhite";
            this.mtWhite.Size = new System.Drawing.Size(30, 30);
            this.mtWhite.Style = MetroFramework.MetroColorStyle.White;
            this.mtWhite.TabIndex = 24;
            this.mtWhite.UseSelectable = true;
            this.mtWhite.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtTeal
            // 
            this.mtTeal.ActiveControl = null;
            this.mtTeal.Location = new System.Drawing.Point(75, 10);
            this.mtTeal.Name = "mtTeal";
            this.mtTeal.Size = new System.Drawing.Size(30, 30);
            this.mtTeal.Style = MetroFramework.MetroColorStyle.Teal;
            this.mtTeal.TabIndex = 22;
            this.mtTeal.UseSelectable = true;
            this.mtTeal.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // mtYellow
            // 
            this.mtYellow.ActiveControl = null;
            this.mtYellow.Location = new System.Drawing.Point(3, 10);
            this.mtYellow.Name = "mtYellow";
            this.mtYellow.Size = new System.Drawing.Size(30, 30);
            this.mtYellow.Style = MetroFramework.MetroColorStyle.Yellow;
            this.mtYellow.TabIndex = 21;
            this.mtYellow.UseSelectable = true;
            this.mtYellow.Click += new System.EventHandler(this.metroTileRed_Click);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.abrirToolStripMenuItem.Text = "Abrir...";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImagePadding = new System.Windows.Forms.Padding(210, 10, 0, 0);
            this.BackMaxSize = 50;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pnlOptions);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form_Main";
            this.Padding = new System.Windows.Forms.Padding(15, 60, 15, 15);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "UniNFe";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.Form_Main_ControlAdded);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.Form_Main_ControlRemoved);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_Main_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.metroContextMenu1.ResumeLayout(false);
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem cmDANFE;
        private System.Windows.Forms.ToolStripMenuItem cmSituacaoServicos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private MetroFramework.Controls.MetroPanel pnlOptions;
        private MetroFramework.Controls.MetroTile mtBlue;
        private MetroFramework.Controls.MetroTile mtTeal;
        private MetroFramework.Controls.MetroTile mtYellow;
        private MetroFramework.Controls.MetroTile mtGreen;
        private MetroFramework.Controls.MetroTile mtPink;
        private MetroFramework.Controls.MetroTile mtRed;
        private MetroFramework.Controls.MetroTile mtPurple;
        private MetroFramework.Controls.MetroTile mtOrange;
        private MetroFramework.Controls.MetroTile mtBlack;
        private MetroFramework.Controls.MetroTile mtLime;
        private MetroFramework.Controls.MetroTile mtWhite;
        private MetroFramework.Controls.MetroTile mtMarrom;
        private MetroFramework.Controls.MetroTile mtSilver;
        private MetroFramework.Controls.MetroTile mtMagenta;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTile metroTile_back;
        public MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem cmMunicipios;
        private MetroFramework.Controls.MetroContextMenu metroContextMenu1;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
    }
}


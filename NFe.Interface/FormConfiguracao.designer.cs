namespace NFe.Interface
{
    partial class FormConfiguracao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguracao));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.tbCancel = new System.Windows.Forms.ToolStripButton();
            this.tbDelete = new System.Windows.Forms.ToolStripButton();
            this.tbAdd = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.chkGravarLogOperacao = new System.Windows.Forms.CheckBox();
            this.cbChecaConexaoInternet = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSenhaConfig2 = new System.Windows.Forms.TextBox();
            this.tbSenhaConfig = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbServidor = new System.Windows.Forms.TextBox();
            this.lblServidor = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.nudPorta = new System.Windows.Forms.NumericUpDown();
            this.lblPorta = new System.Windows.Forms.Label();
            this.tbSenha = new System.Windows.Forms.TextBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.cbProxy = new System.Windows.Forms.CheckBox();
            this.chkUseProxyAutomaticamente = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorta)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSave,
            this.tbCancel,
            this.tbDelete,
            this.tbAdd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(611, 39);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbSave
            // 
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSave.Image = global::NFe.Interface.Properties.Resources.filesave;
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(36, 36);
            this.tbSave.Text = "toolStripButton1";
            this.tbSave.ToolTipText = "Salvar as alterações e fechar a tela";
            this.tbSave.Click += new System.EventHandler(this.tbSave_Click);
            // 
            // tbCancel
            // 
            this.tbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbCancel.Image = global::NFe.Interface.Properties.Resources.fileclose;
            this.tbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbCancel.Name = "tbCancel";
            this.tbCancel.Size = new System.Drawing.Size(36, 36);
            this.tbCancel.Text = "toolStripButton2";
            this.tbCancel.ToolTipText = "Fechar a tela sem salvar as alterações";
            this.tbCancel.Click += new System.EventHandler(this.tbCancel_Click);
            // 
            // tbDelete
            // 
            this.tbDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbDelete.Image = global::NFe.Interface.Properties.Resources.excluir;
            this.tbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbDelete.Name = "tbDelete";
            this.tbDelete.Size = new System.Drawing.Size(36, 36);
            this.tbDelete.Text = "Exclui";
            this.tbDelete.ToolTipText = "Exclui a empresa";
            this.tbDelete.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // tbAdd
            // 
            this.tbAdd.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbAdd.Image = global::NFe.Interface.Properties.Resources.incluir;
            this.tbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAdd.Name = "tbAdd";
            this.tbAdd.Size = new System.Drawing.Size(36, 36);
            this.tbAdd.Text = "Nova";
            this.tbAdd.ToolTipText = "Nova empresa";
            this.tbAdd.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipTitle = "As pastas informadas não podem se repetir";
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage9);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(0, 39);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(611, 469);
            this.tabControl4.TabIndex = 3;
            this.tabControl4.SelectedIndexChanged += new System.EventHandler(this.tabControl4_SelectedIndexChanged);
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.chkUseProxyAutomaticamente);
            this.tabPage9.Controls.Add(this.chkGravarLogOperacao);
            this.tabPage9.Controls.Add(this.cbChecaConexaoInternet);
            this.tabPage9.Controls.Add(this.groupBox1);
            this.tabPage9.Controls.Add(this.tbServidor);
            this.tabPage9.Controls.Add(this.lblServidor);
            this.tabPage9.Controls.Add(this.lblUsuario);
            this.tabPage9.Controls.Add(this.nudPorta);
            this.tabPage9.Controls.Add(this.lblPorta);
            this.tabPage9.Controls.Add(this.tbSenha);
            this.tabPage9.Controls.Add(this.lblSenha);
            this.tabPage9.Controls.Add(this.tbUsuario);
            this.tabPage9.Controls.Add(this.cbProxy);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(603, 443);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "Geral";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // chkGravarLogOperacao
            // 
            this.chkGravarLogOperacao.AutoSize = true;
            this.chkGravarLogOperacao.Location = new System.Drawing.Point(9, 149);
            this.chkGravarLogOperacao.Name = "chkGravarLogOperacao";
            this.chkGravarLogOperacao.Size = new System.Drawing.Size(198, 17);
            this.chkGravarLogOperacao.TabIndex = 15;
            this.chkGravarLogOperacao.Text = "Gravar log das operações realizadas";
            this.chkGravarLogOperacao.UseVisualStyleBackColor = true;
            // 
            // cbChecaConexaoInternet
            // 
            this.cbChecaConexaoInternet.AutoSize = true;
            this.cbChecaConexaoInternet.Location = new System.Drawing.Point(9, 126);
            this.cbChecaConexaoInternet.Name = "cbChecaConexaoInternet";
            this.cbChecaConexaoInternet.Size = new System.Drawing.Size(267, 17);
            this.cbChecaConexaoInternet.TabIndex = 14;
            this.cbChecaConexaoInternet.Text = "Checar a Conexão com a Internet ao Enviar o XML";
            this.cbChecaConexaoInternet.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbSenhaConfig2);
            this.groupBox1.Controls.Add(this.tbSenhaConfig);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(9, 203);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 123);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Senha de acesso a tela de configurações";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Senha:";
            // 
            // tbSenhaConfig2
            // 
            this.tbSenhaConfig2.Location = new System.Drawing.Point(9, 87);
            this.tbSenhaConfig2.Name = "tbSenhaConfig2";
            this.tbSenhaConfig2.PasswordChar = '*';
            this.tbSenhaConfig2.Size = new System.Drawing.Size(327, 20);
            this.tbSenhaConfig2.TabIndex = 12;
            // 
            // tbSenhaConfig
            // 
            this.tbSenhaConfig.Location = new System.Drawing.Point(9, 43);
            this.tbSenhaConfig.Name = "tbSenhaConfig";
            this.tbSenhaConfig.PasswordChar = '*';
            this.tbSenhaConfig.Size = new System.Drawing.Size(327, 20);
            this.tbSenhaConfig.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Repetir a senha:";
            // 
            // tbServidor
            // 
            this.tbServidor.Enabled = false;
            this.tbServidor.Location = new System.Drawing.Point(96, 89);
            this.tbServidor.Name = "tbServidor";
            this.tbServidor.Size = new System.Drawing.Size(463, 20);
            this.tbServidor.TabIndex = 8;
            this.tbServidor.TextChanged += new System.EventHandler(this.change_Modificado);
            // 
            // lblServidor
            // 
            this.lblServidor.AutoSize = true;
            this.lblServidor.Enabled = false;
            this.lblServidor.Location = new System.Drawing.Point(93, 73);
            this.lblServidor.Name = "lblServidor";
            this.lblServidor.Size = new System.Drawing.Size(49, 13);
            this.lblServidor.TabIndex = 7;
            this.lblServidor.Text = "Servidor:";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Enabled = false;
            this.lblUsuario.Location = new System.Drawing.Point(6, 31);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuário:";
            // 
            // nudPorta
            // 
            this.nudPorta.Enabled = false;
            this.nudPorta.Location = new System.Drawing.Point(9, 89);
            this.nudPorta.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.nudPorta.Name = "nudPorta";
            this.nudPorta.Size = new System.Drawing.Size(71, 20);
            this.nudPorta.TabIndex = 6;
            this.nudPorta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPorta.ValueChanged += new System.EventHandler(this.change_Modificado);
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Enabled = false;
            this.lblPorta.Location = new System.Drawing.Point(6, 73);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(35, 13);
            this.lblPorta.TabIndex = 1;
            this.lblPorta.Text = "Porta:";
            // 
            // tbSenha
            // 
            this.tbSenha.Enabled = false;
            this.tbSenha.Location = new System.Drawing.Point(343, 47);
            this.tbSenha.Name = "tbSenha";
            this.tbSenha.PasswordChar = '●';
            this.tbSenha.Size = new System.Drawing.Size(216, 20);
            this.tbSenha.TabIndex = 5;
            this.tbSenha.TextChanged += new System.EventHandler(this.change_Modificado);
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Enabled = false;
            this.lblSenha.Location = new System.Drawing.Point(340, 31);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(41, 13);
            this.lblSenha.TabIndex = 2;
            this.lblSenha.Text = "Senha:";
            // 
            // tbUsuario
            // 
            this.tbUsuario.Enabled = false;
            this.tbUsuario.Location = new System.Drawing.Point(9, 47);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.Size = new System.Drawing.Size(327, 20);
            this.tbUsuario.TabIndex = 4;
            this.tbUsuario.TextChanged += new System.EventHandler(this.change_Modificado);
            // 
            // cbProxy
            // 
            this.cbProxy.AutoSize = true;
            this.cbProxy.Location = new System.Drawing.Point(9, 7);
            this.cbProxy.Name = "cbProxy";
            this.cbProxy.Size = new System.Drawing.Size(133, 17);
            this.cbProxy.TabIndex = 3;
            this.cbProxy.Text = "Usar um servidor proxy";
            this.cbProxy.UseVisualStyleBackColor = true;
            this.cbProxy.CheckedChanged += new System.EventHandler(this.cbProxy_CheckedChanged);
            // 
            // chkUseProxyAutomaticamente
            // 
            this.chkUseProxyAutomaticamente.AutoSize = true;
            this.chkUseProxyAutomaticamente.Location = new System.Drawing.Point(9, 172);
            this.chkUseProxyAutomaticamente.Name = "chkUseProxyAutomaticamente";
            this.chkUseProxyAutomaticamente.Size = new System.Drawing.Size(240, 17);
            this.chkUseProxyAutomaticamente.TabIndex = 16;
            this.chkUseProxyAutomaticamente.Text = "Usar configuração de proxy automaticamente";
            this.chkUseProxyAutomaticamente.UseVisualStyleBackColor = true;
            // 
            // FormConfiguracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 508);
            this.Controls.Add(this.tabControl4);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConfiguracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuração";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfiguracao_FormClosing);
            this.Load += new System.EventHandler(this.FormConfiguracao_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl4.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.ToolStripButton tbCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton tbDelete;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TextBox tbServidor;
        private System.Windows.Forms.Label lblServidor;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.NumericUpDown nudPorta;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.TextBox tbSenha;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.TextBox tbUsuario;
        private System.Windows.Forms.CheckBox cbProxy;
        private System.Windows.Forms.ToolStripButton tbAdd;
        private System.Windows.Forms.TextBox tbSenhaConfig;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSenhaConfig2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbChecaConexaoInternet;
        private System.Windows.Forms.CheckBox chkGravarLogOperacao;
        private System.Windows.Forms.CheckBox chkUseProxyAutomaticamente;
    }
}
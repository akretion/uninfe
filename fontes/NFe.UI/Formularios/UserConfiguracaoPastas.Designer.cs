namespace NFe.UI.Formularios
{
    partial class UserConfiguracaoPastas
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
            this.cbCriaPastas = new MetroFramework.Controls.MetroCheckBox();
            this.lbl_textBox_PastaDownload = new MetroFramework.Controls.MetroLabel();
            this.lbl_textBox_PastaValidar = new MetroFramework.Controls.MetroLabel();
            this.lbl_textBox_PastaBackup = new MetroFramework.Controls.MetroLabel();
            this.lbl_textBox_PastaXmlErro = new MetroFramework.Controls.MetroLabel();
            this.lbl_textBox_PastaEnviados = new MetroFramework.Controls.MetroLabel();
            this.lbl_textBox_PastaRetornoXML = new MetroFramework.Controls.MetroLabel();
            this.lbl_textBox_PastaLote = new MetroFramework.Controls.MetroLabel();
            this.lbl_textBox_PastaEnvioXML = new MetroFramework.Controls.MetroLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox_PastaDownloadNFeDest = new MetroFramework.Controls.MetroTextBox();
            this.textBox_PastaValidar = new MetroFramework.Controls.MetroTextBox();
            this.textBox_PastaBackup = new MetroFramework.Controls.MetroTextBox();
            this.textBox_PastaXmlErro = new MetroFramework.Controls.MetroTextBox();
            this.textBox_PastaXmlEnviado = new MetroFramework.Controls.MetroTextBox();
            this.textBox_PastaXmlRetorno = new MetroFramework.Controls.MetroTextBox();
            this.textBox_PastaXmlEmLote = new MetroFramework.Controls.MetroTextBox();
            this.textBox_PastaXmlEnvio = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // cbCriaPastas
            // 
            this.cbCriaPastas.AutoSize = true;
            this.cbCriaPastas.Location = new System.Drawing.Point(3, 3);
            this.cbCriaPastas.Name = "cbCriaPastas";
            this.cbCriaPastas.Size = new System.Drawing.Size(195, 15);
            this.cbCriaPastas.TabIndex = 0;
            this.cbCriaPastas.Text = "Criar as pastas automaticamente";
            this.cbCriaPastas.UseSelectable = true;
            this.cbCriaPastas.CheckedChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            // 
            // lbl_textBox_PastaDownload
            // 
            this.lbl_textBox_PastaDownload.AutoSize = true;
            this.lbl_textBox_PastaDownload.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaDownload.Location = new System.Drawing.Point(3, 325);
            this.lbl_textBox_PastaDownload.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaDownload.Name = "lbl_textBox_PastaDownload";
            this.lbl_textBox_PastaDownload.Size = new System.Drawing.Size(610, 15);
            this.lbl_textBox_PastaDownload.TabIndex = 15;
            this.lbl_textBox_PastaDownload.Text = "Pasta onde serão gravados os arquivos XML´s  de NFe baixados da Sefaz e retornos " +
    "da consulta de eventos de terceiros";
            // 
            // lbl_textBox_PastaValidar
            // 
            this.lbl_textBox_PastaValidar.AutoSize = true;
            this.lbl_textBox_PastaValidar.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaValidar.Location = new System.Drawing.Point(3, 282);
            this.lbl_textBox_PastaValidar.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaValidar.Name = "lbl_textBox_PastaValidar";
            this.lbl_textBox_PastaValidar.Size = new System.Drawing.Size(378, 15);
            this.lbl_textBox_PastaValidar.TabIndex = 13;
            this.lbl_textBox_PastaValidar.Text = "Pasta onde serão gravados os arquivos XML´s a serem somente validados";
            // 
            // lbl_textBox_PastaBackup
            // 
            this.lbl_textBox_PastaBackup.AutoSize = true;
            this.lbl_textBox_PastaBackup.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaBackup.Location = new System.Drawing.Point(3, 239);
            this.lbl_textBox_PastaBackup.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaBackup.Name = "lbl_textBox_PastaBackup";
            this.lbl_textBox_PastaBackup.Size = new System.Drawing.Size(202, 15);
            this.lbl_textBox_PastaBackup.TabIndex = 11;
            this.lbl_textBox_PastaBackup.Text = "Pasta para Backup dos XML´s enviados";
            // 
            // lbl_textBox_PastaXmlErro
            // 
            this.lbl_textBox_PastaXmlErro.AutoSize = true;
            this.lbl_textBox_PastaXmlErro.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaXmlErro.Location = new System.Drawing.Point(3, 196);
            this.lbl_textBox_PastaXmlErro.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaXmlErro.Name = "lbl_textBox_PastaXmlErro";
            this.lbl_textBox_PastaXmlErro.Size = new System.Drawing.Size(474, 15);
            this.lbl_textBox_PastaXmlErro.TabIndex = 9;
            this.lbl_textBox_PastaXmlErro.Text = "Pasta para arquivamento temporário dos XML´s que apresentaram erro na tentativa d" +
    "o envio";
            // 
            // lbl_textBox_PastaEnviados
            // 
            this.lbl_textBox_PastaEnviados.AutoSize = true;
            this.lbl_textBox_PastaEnviados.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaEnviados.Location = new System.Drawing.Point(3, 153);
            this.lbl_textBox_PastaEnviados.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaEnviados.Name = "lbl_textBox_PastaEnviados";
            this.lbl_textBox_PastaEnviados.Size = new System.Drawing.Size(285, 15);
            this.lbl_textBox_PastaEnviados.TabIndex = 7;
            this.lbl_textBox_PastaEnviados.Text = "Pasta onde serão gravados os arquivos XML´s enviados";
            // 
            // lbl_textBox_PastaRetornoXML
            // 
            this.lbl_textBox_PastaRetornoXML.AutoSize = true;
            this.lbl_textBox_PastaRetornoXML.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaRetornoXML.Location = new System.Drawing.Point(3, 110);
            this.lbl_textBox_PastaRetornoXML.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaRetornoXML.Name = "lbl_textBox_PastaRetornoXML";
            this.lbl_textBox_PastaRetornoXML.Size = new System.Drawing.Size(383, 15);
            this.lbl_textBox_PastaRetornoXML.TabIndex = 5;
            this.lbl_textBox_PastaRetornoXML.Text = "Pasta onde serão gravados os arquivos XML´s de retorno dos WebServices";
            // 
            // lbl_textBox_PastaLote
            // 
            this.lbl_textBox_PastaLote.AutoSize = true;
            this.lbl_textBox_PastaLote.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaLote.Location = new System.Drawing.Point(3, 67);
            this.lbl_textBox_PastaLote.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaLote.Name = "lbl_textBox_PastaLote";
            this.lbl_textBox_PastaLote.Size = new System.Drawing.Size(520, 15);
            this.lbl_textBox_PastaLote.TabIndex = 3;
            this.lbl_textBox_PastaLote.Text = "Pasta onde serão gravados os arquivos XML´s de NF-e a serem enviadas em lote para" +
    " os WebServices";
            // 
            // lbl_textBox_PastaEnvioXML
            // 
            this.lbl_textBox_PastaEnvioXML.AutoSize = true;
            this.lbl_textBox_PastaEnvioXML.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_textBox_PastaEnvioXML.Location = new System.Drawing.Point(3, 24);
            this.lbl_textBox_PastaEnvioXML.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_textBox_PastaEnvioXML.Name = "lbl_textBox_PastaEnvioXML";
            this.lbl_textBox_PastaEnvioXML.Size = new System.Drawing.Size(515, 15);
            this.lbl_textBox_PastaEnvioXML.TabIndex = 1;
            this.lbl_textBox_PastaEnvioXML.Text = "Pasta onde serão gravados os arquivos XML´s a serem enviados individualmente para" +
    " os WebServices";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // textBox_PastaDownloadNFeDest
            // 
            this.textBox_PastaDownloadNFeDest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaDownloadNFeDest.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaDownloadNFeDest.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaDownloadNFeDest.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaDownloadNFeDest.IconRight = true;
            this.textBox_PastaDownloadNFeDest.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaDownloadNFeDest.Location = new System.Drawing.Point(3, 344);
            this.textBox_PastaDownloadNFeDest.MaxLength = 32767;
            this.textBox_PastaDownloadNFeDest.Name = "textBox_PastaDownloadNFeDest";
            this.textBox_PastaDownloadNFeDest.PasswordChar = '\0';
            this.textBox_PastaDownloadNFeDest.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaDownloadNFeDest.SelectedText = "";
            this.textBox_PastaDownloadNFeDest.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaDownloadNFeDest.TabIndex = 16;
            this.textBox_PastaDownloadNFeDest.Text = "Styled Textbox";
            this.textBox_PastaDownloadNFeDest.UseSelectable = true;
            this.textBox_PastaDownloadNFeDest.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaDownloadNFeDest.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaDownloadNFeDest.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaDownloadNFeDest.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaDownloadNFeDest.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // textBox_PastaValidar
            // 
            this.textBox_PastaValidar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaValidar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaValidar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaValidar.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaValidar.IconRight = true;
            this.textBox_PastaValidar.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaValidar.Location = new System.Drawing.Point(3, 301);
            this.textBox_PastaValidar.MaxLength = 32767;
            this.textBox_PastaValidar.Name = "textBox_PastaValidar";
            this.textBox_PastaValidar.PasswordChar = '\0';
            this.textBox_PastaValidar.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaValidar.SelectedText = "";
            this.textBox_PastaValidar.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaValidar.TabIndex = 14;
            this.textBox_PastaValidar.Text = "Styled Textbox";
            this.textBox_PastaValidar.UseSelectable = true;
            this.textBox_PastaValidar.UseStyleColors = true;
            this.textBox_PastaValidar.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaValidar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaValidar.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaValidar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaValidar.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // textBox_PastaBackup
            // 
            this.textBox_PastaBackup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaBackup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaBackup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaBackup.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaBackup.IconRight = true;
            this.textBox_PastaBackup.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaBackup.Location = new System.Drawing.Point(3, 258);
            this.textBox_PastaBackup.MaxLength = 32767;
            this.textBox_PastaBackup.Name = "textBox_PastaBackup";
            this.textBox_PastaBackup.PasswordChar = '\0';
            this.textBox_PastaBackup.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaBackup.SelectedText = "";
            this.textBox_PastaBackup.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaBackup.TabIndex = 12;
            this.textBox_PastaBackup.Text = "Styled Textbox";
            this.textBox_PastaBackup.UseSelectable = true;
            this.textBox_PastaBackup.UseStyleColors = true;
            this.textBox_PastaBackup.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaBackup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaBackup.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaBackup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaBackup.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // textBox_PastaXmlErro
            // 
            this.textBox_PastaXmlErro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaXmlErro.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaXmlErro.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaXmlErro.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaXmlErro.IconRight = true;
            this.textBox_PastaXmlErro.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaXmlErro.Location = new System.Drawing.Point(3, 215);
            this.textBox_PastaXmlErro.MaxLength = 32767;
            this.textBox_PastaXmlErro.Name = "textBox_PastaXmlErro";
            this.textBox_PastaXmlErro.PasswordChar = '\0';
            this.textBox_PastaXmlErro.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaXmlErro.SelectedText = "";
            this.textBox_PastaXmlErro.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaXmlErro.TabIndex = 10;
            this.textBox_PastaXmlErro.Text = "Styled Textbox";
            this.textBox_PastaXmlErro.UseSelectable = true;
            this.textBox_PastaXmlErro.UseStyleColors = true;
            this.textBox_PastaXmlErro.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaXmlErro.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaXmlErro.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaXmlErro.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaXmlErro.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // textBox_PastaXmlEnviado
            // 
            this.textBox_PastaXmlEnviado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaXmlEnviado.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaXmlEnviado.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaXmlEnviado.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaXmlEnviado.IconRight = true;
            this.textBox_PastaXmlEnviado.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaXmlEnviado.Location = new System.Drawing.Point(3, 172);
            this.textBox_PastaXmlEnviado.MaxLength = 32767;
            this.textBox_PastaXmlEnviado.Name = "textBox_PastaXmlEnviado";
            this.textBox_PastaXmlEnviado.PasswordChar = '\0';
            this.textBox_PastaXmlEnviado.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaXmlEnviado.SelectedText = "";
            this.textBox_PastaXmlEnviado.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaXmlEnviado.TabIndex = 8;
            this.textBox_PastaXmlEnviado.Text = "Styled Textbox";
            this.textBox_PastaXmlEnviado.UseSelectable = true;
            this.textBox_PastaXmlEnviado.UseStyleColors = true;
            this.textBox_PastaXmlEnviado.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaXmlEnviado.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaXmlEnviado.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaXmlEnviado.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaXmlEnviado.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // textBox_PastaXmlRetorno
            // 
            this.textBox_PastaXmlRetorno.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaXmlRetorno.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaXmlRetorno.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaXmlRetorno.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaXmlRetorno.IconRight = true;
            this.textBox_PastaXmlRetorno.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaXmlRetorno.Location = new System.Drawing.Point(3, 129);
            this.textBox_PastaXmlRetorno.MaxLength = 32767;
            this.textBox_PastaXmlRetorno.Name = "textBox_PastaXmlRetorno";
            this.textBox_PastaXmlRetorno.PasswordChar = '\0';
            this.textBox_PastaXmlRetorno.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaXmlRetorno.SelectedText = "";
            this.textBox_PastaXmlRetorno.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaXmlRetorno.TabIndex = 6;
            this.textBox_PastaXmlRetorno.Text = "Styled Textbox";
            this.textBox_PastaXmlRetorno.UseSelectable = true;
            this.textBox_PastaXmlRetorno.UseStyleColors = true;
            this.textBox_PastaXmlRetorno.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaXmlRetorno.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaXmlRetorno.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaXmlRetorno.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaXmlRetorno.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // textBox_PastaXmlEmLote
            // 
            this.textBox_PastaXmlEmLote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaXmlEmLote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaXmlEmLote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaXmlEmLote.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaXmlEmLote.IconRight = true;
            this.textBox_PastaXmlEmLote.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaXmlEmLote.Location = new System.Drawing.Point(3, 86);
            this.textBox_PastaXmlEmLote.MaxLength = 32767;
            this.textBox_PastaXmlEmLote.Name = "textBox_PastaXmlEmLote";
            this.textBox_PastaXmlEmLote.PasswordChar = '\0';
            this.textBox_PastaXmlEmLote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaXmlEmLote.SelectedText = "";
            this.textBox_PastaXmlEmLote.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaXmlEmLote.TabIndex = 4;
            this.textBox_PastaXmlEmLote.Text = "Styled Textbox";
            this.textBox_PastaXmlEmLote.UseSelectable = true;
            this.textBox_PastaXmlEmLote.UseStyleColors = true;
            this.textBox_PastaXmlEmLote.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaXmlEmLote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaXmlEmLote.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaXmlEmLote.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaXmlEmLote.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // textBox_PastaXmlEnvio
            // 
            this.textBox_PastaXmlEnvio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PastaXmlEnvio.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_PastaXmlEnvio.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.textBox_PastaXmlEnvio.Icon = global::NFe.UI.Properties.Resources.folder_orange_open;
            this.textBox_PastaXmlEnvio.IconRight = true;
            this.textBox_PastaXmlEnvio.Lines = new string[] {
        "Styled Textbox"};
            this.textBox_PastaXmlEnvio.Location = new System.Drawing.Point(3, 43);
            this.textBox_PastaXmlEnvio.MaxLength = 32767;
            this.textBox_PastaXmlEnvio.Name = "textBox_PastaXmlEnvio";
            this.textBox_PastaXmlEnvio.PasswordChar = '\0';
            this.textBox_PastaXmlEnvio.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_PastaXmlEnvio.SelectedText = "";
            this.textBox_PastaXmlEnvio.Size = new System.Drawing.Size(623, 22);
            this.textBox_PastaXmlEnvio.TabIndex = 2;
            this.textBox_PastaXmlEnvio.Text = "Styled Textbox";
            this.textBox_PastaXmlEnvio.UseSelectable = true;
            this.textBox_PastaXmlEnvio.UseStyleColors = true;
            this.textBox_PastaXmlEnvio.TextChanged += new System.EventHandler(this.textBox_PastaXmlEnvio_TextChanged);
            this.textBox_PastaXmlEnvio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PastaXmlEnvio_KeyDown);
            this.textBox_PastaXmlEnvio.Leave += new System.EventHandler(this.textBox_PastaEnvioXML_Leave);
            this.textBox_PastaXmlEnvio.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_PastaEnvioXML_MouseDown);
            this.textBox_PastaXmlEnvio.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PastaXmlEnvio_Validating);
            // 
            // userConfiguracao_pastas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.cbCriaPastas);
            this.Controls.Add(this.lbl_textBox_PastaDownload);
            this.Controls.Add(this.textBox_PastaDownloadNFeDest);
            this.Controls.Add(this.lbl_textBox_PastaValidar);
            this.Controls.Add(this.textBox_PastaValidar);
            this.Controls.Add(this.lbl_textBox_PastaBackup);
            this.Controls.Add(this.textBox_PastaBackup);
            this.Controls.Add(this.lbl_textBox_PastaXmlErro);
            this.Controls.Add(this.textBox_PastaXmlErro);
            this.Controls.Add(this.lbl_textBox_PastaEnviados);
            this.Controls.Add(this.textBox_PastaXmlEnviado);
            this.Controls.Add(this.lbl_textBox_PastaRetornoXML);
            this.Controls.Add(this.textBox_PastaXmlRetorno);
            this.Controls.Add(this.lbl_textBox_PastaLote);
            this.Controls.Add(this.textBox_PastaXmlEmLote);
            this.Controls.Add(this.lbl_textBox_PastaEnvioXML);
            this.Controls.Add(this.textBox_PastaXmlEnvio);
            this.Name = "userConfiguracao_pastas";
            this.Size = new System.Drawing.Size(640, 374);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaDownload;
        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaValidar;
        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaBackup;
        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaXmlErro;
        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaEnviados;
        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaRetornoXML;
        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaLote;
        private MetroFramework.Controls.MetroLabel lbl_textBox_PastaEnvioXML;
        public MetroFramework.Controls.MetroCheckBox cbCriaPastas;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private MetroFramework.Controls.MetroTextBox textBox_PastaXmlErro;
        private MetroFramework.Controls.MetroTextBox textBox_PastaDownloadNFeDest;
        private MetroFramework.Controls.MetroTextBox textBox_PastaValidar;
        private MetroFramework.Controls.MetroTextBox textBox_PastaBackup;
        private MetroFramework.Controls.MetroTextBox textBox_PastaXmlEnviado;
        private MetroFramework.Controls.MetroTextBox textBox_PastaXmlRetorno;
        private MetroFramework.Controls.MetroTextBox textBox_PastaXmlEmLote;
        public MetroFramework.Controls.MetroTextBox textBox_PastaXmlEnvio;
    }
}

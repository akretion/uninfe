namespace NFe.UI.Formularios
{
    partial class userConfiguracao_diversos
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
            this.checkBoxRetornoNFETxt = new MetroFramework.Controls.MetroCheckBox();
            this.cbIndSinc = new MetroFramework.Controls.MetroCheckBox();
            this.lbl_udTempoConsulta = new MetroFramework.Controls.MetroLabel();
            this.checkBoxCompactaNFe = new MetroFramework.Controls.MetroCheckBox();
            this.udTempoConsulta = new MetroFramework.Controls.MetroTextBox();
            this.lbl_udDiasLimpeza = new MetroFramework.Controls.MetroLabel();
            this.udDiasLimpeza = new MetroFramework.Controls.MetroTextBox();
            this.cboDiretorioSalvarComo = new MetroFramework.Controls.MetroComboBox();
            this.comboBox_tpEmis = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
            this.comboBox_Ambiente = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.comboBox_UF = new MetroFramework.Controls.MetroComboBox();
            this.labelUF = new MetroFramework.Controls.MetroLabel();
            this.cbServico = new MetroFramework.Controls.MetroComboBox();
            this.checkBoxGravarEventosNaPastaEnviadosNFe = new MetroFramework.Controls.MetroCheckBox();
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabel19 = new MetroFramework.Controls.MetroLabel();
            this.lbl_DiretorioSalvarComo = new MetroFramework.Controls.MetroLabel();
            this.metroLabel21 = new MetroFramework.Controls.MetroLabel();
            this.edtNome = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel22 = new MetroFramework.Controls.MetroLabel();
            this.edtCNPJ = new MetroFramework.Controls.MetroTextBox();
            this.checkBoxGravarEventosDeTerceiros = new MetroFramework.Controls.MetroCheckBox();
            this.lbl_UsuarioWS = new MetroFramework.Controls.MetroLabel();
            this.txtUsuarioWS = new MetroFramework.Controls.MetroTextBox();
            this.lbl_SenhaWS = new MetroFramework.Controls.MetroLabel();
            this.txtSenhaWS = new MetroFramework.Controls.MetroTextBox();
            this.lbl_Padrao = new MetroFramework.Controls.MetroLabel();
            this.edtPadrao = new MetroFramework.Controls.MetroTextBox();
            this.lbl_CodMun = new MetroFramework.Controls.MetroLabel();
            this.edtCodMun = new MetroFramework.Controls.MetroTextBox();
            this.grpQRCode = new System.Windows.Forms.GroupBox();
            this.edtTokenCSC = new MetroFramework.Controls.MetroTextBox();
            this.edtIdentificadorCSC = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.grpQRCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxRetornoNFETxt
            // 
            this.checkBoxRetornoNFETxt.AutoSize = true;
            this.checkBoxRetornoNFETxt.Location = new System.Drawing.Point(3, 299);
            this.checkBoxRetornoNFETxt.Name = "checkBoxRetornoNFETxt";
            this.checkBoxRetornoNFETxt.Size = new System.Drawing.Size(379, 15);
            this.checkBoxRetornoNFETxt.TabIndex = 29;
            this.checkBoxRetornoNFETxt.Text = "Gravar os retornos dos webservices também no formato texto (TXT)";
            this.checkBoxRetornoNFETxt.UseSelectable = true;
            this.checkBoxRetornoNFETxt.CheckedChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // cbIndSinc
            // 
            this.cbIndSinc.AutoSize = true;
            this.cbIndSinc.Location = new System.Drawing.Point(3, 320);
            this.cbIndSinc.Name = "cbIndSinc";
            this.cbIndSinc.Size = new System.Drawing.Size(252, 15);
            this.cbIndSinc.TabIndex = 30;
            this.cbIndSinc.Text = "Enviar a NFe utilizando o processo síncrono";
            this.cbIndSinc.UseSelectable = true;
            this.cbIndSinc.CheckedChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // lbl_udTempoConsulta
            // 
            this.lbl_udTempoConsulta.AutoSize = true;
            this.lbl_udTempoConsulta.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_udTempoConsulta.Location = new System.Drawing.Point(3, 189);
            this.lbl_udTempoConsulta.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_udTempoConsulta.Name = "lbl_udTempoConsulta";
            this.lbl_udTempoConsulta.Size = new System.Drawing.Size(427, 15);
            this.lbl_udTempoConsulta.TabIndex = 24;
            this.lbl_udTempoConsulta.Text = "Quantidade em segundos p/efetuar a consultar da autorização da NFe/NFCe/MDFe";
            // 
            // checkBoxCompactaNFe
            // 
            this.checkBoxCompactaNFe.AutoSize = true;
            this.checkBoxCompactaNFe.Location = new System.Drawing.Point(3, 341);
            this.checkBoxCompactaNFe.Name = "checkBoxCompactaNFe";
            this.checkBoxCompactaNFe.Size = new System.Drawing.Size(211, 15);
            this.checkBoxCompactaNFe.TabIndex = 31;
            this.checkBoxCompactaNFe.Text = "Compactar NFe para enviar a SEFAZ";
            this.checkBoxCompactaNFe.UseSelectable = true;
            this.checkBoxCompactaNFe.CheckedChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // udTempoConsulta
            // 
            this.udTempoConsulta.Lines = new string[] {
        "Styled Textbox"};
            this.udTempoConsulta.Location = new System.Drawing.Point(3, 208);
            this.udTempoConsulta.MaxLength = 4;
            this.udTempoConsulta.Name = "udTempoConsulta";
            this.udTempoConsulta.PasswordChar = '\0';
            this.udTempoConsulta.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.udTempoConsulta.SelectedText = "";
            this.udTempoConsulta.Size = new System.Drawing.Size(74, 22);
            this.udTempoConsulta.TabIndex = 25;
            this.udTempoConsulta.Text = "Styled Textbox";
            this.udTempoConsulta.UseSelectable = true;
            this.udTempoConsulta.UseStyleColors = true;
            this.udTempoConsulta.TextChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            this.udTempoConsulta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.udDiasLimpeza_KeyPress);
            // 
            // lbl_udDiasLimpeza
            // 
            this.lbl_udDiasLimpeza.AutoSize = true;
            this.lbl_udDiasLimpeza.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_udDiasLimpeza.Location = new System.Drawing.Point(3, 147);
            this.lbl_udDiasLimpeza.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_udDiasLimpeza.Name = "lbl_udDiasLimpeza";
            this.lbl_udDiasLimpeza.Size = new System.Drawing.Size(502, 15);
            this.lbl_udDiasLimpeza.TabIndex = 18;
            this.lbl_udDiasLimpeza.Text = "Quantos dias devem ser mantidos os arquivos na pasta temporário e retorno? Deixe " +
    "0 para infinito.";
            // 
            // udDiasLimpeza
            // 
            this.udDiasLimpeza.Lines = new string[] {
        "Styled Textbox"};
            this.udDiasLimpeza.Location = new System.Drawing.Point(3, 166);
            this.udDiasLimpeza.MaxLength = 2;
            this.udDiasLimpeza.Name = "udDiasLimpeza";
            this.udDiasLimpeza.PasswordChar = '\0';
            this.udDiasLimpeza.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.udDiasLimpeza.SelectedText = "";
            this.udDiasLimpeza.Size = new System.Drawing.Size(74, 22);
            this.udDiasLimpeza.TabIndex = 19;
            this.udDiasLimpeza.Text = "Styled Textbox";
            this.udDiasLimpeza.UseSelectable = true;
            this.udDiasLimpeza.UseStyleColors = true;
            this.udDiasLimpeza.TextChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            this.udDiasLimpeza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.udDiasLimpeza_KeyPress);
            // 
            // cboDiretorioSalvarComo
            // 
            this.cboDiretorioSalvarComo.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cboDiretorioSalvarComo.FormattingEnabled = true;
            this.cboDiretorioSalvarComo.ItemHeight = 19;
            this.cboDiretorioSalvarComo.Items.AddRange(new object[] {
            "Raiz",
            "AMD",
            "AM",
            "AD",
            "MDA",
            "MD",
            "MA",
            "DMA",
            "DM",
            "DA",
            "A\\M\\D",
            "A\\M",
            "A\\D",
            "M\\D\\A",
            "M\\D",
            "M\\A",
            "D\\M\\A",
            "D\\M",
            "D\\A"});
            this.cboDiretorioSalvarComo.Location = new System.Drawing.Point(3, 115);
            this.cboDiretorioSalvarComo.MaxDropDownItems = 15;
            this.cboDiretorioSalvarComo.Name = "cboDiretorioSalvarComo";
            this.cboDiretorioSalvarComo.Size = new System.Drawing.Size(174, 25);
            this.cboDiretorioSalvarComo.TabIndex = 13;
            this.cboDiretorioSalvarComo.UseSelectable = true;
            this.cboDiretorioSalvarComo.SelectedIndexChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // comboBox_tpEmis
            // 
            this.comboBox_tpEmis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_tpEmis.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboBox_tpEmis.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.comboBox_tpEmis.FormattingEnabled = true;
            this.comboBox_tpEmis.ItemHeight = 19;
            this.comboBox_tpEmis.Location = new System.Drawing.Point(448, 64);
            this.comboBox_tpEmis.MaxDropDownItems = 15;
            this.comboBox_tpEmis.Name = "comboBox_tpEmis";
            this.comboBox_tpEmis.Size = new System.Drawing.Size(250, 25);
            this.comboBox_tpEmis.TabIndex = 11;
            this.comboBox_tpEmis.UseSelectable = true;
            this.comboBox_tpEmis.SelectedIndexChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // metroLabel11
            // 
            this.metroLabel11.AutoSize = true;
            this.metroLabel11.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel11.Location = new System.Drawing.Point(448, 46);
            this.metroLabel11.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel11.Name = "metroLabel11";
            this.metroLabel11.Size = new System.Drawing.Size(89, 15);
            this.metroLabel11.TabIndex = 10;
            this.metroLabel11.Text = "Tipo de emissão";
            // 
            // comboBox_Ambiente
            // 
            this.comboBox_Ambiente.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboBox_Ambiente.FormattingEnabled = true;
            this.comboBox_Ambiente.ItemHeight = 19;
            this.comboBox_Ambiente.Location = new System.Drawing.Point(337, 64);
            this.comboBox_Ambiente.MaxDropDownItems = 15;
            this.comboBox_Ambiente.Name = "comboBox_Ambiente";
            this.comboBox_Ambiente.Size = new System.Drawing.Size(105, 25);
            this.comboBox_Ambiente.TabIndex = 9;
            this.comboBox_Ambiente.UseSelectable = true;
            this.comboBox_Ambiente.SelectedIndexChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // metroLabel10
            // 
            this.metroLabel10.AutoSize = true;
            this.metroLabel10.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel10.Location = new System.Drawing.Point(337, 46);
            this.metroLabel10.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(56, 15);
            this.metroLabel10.TabIndex = 8;
            this.metroLabel10.Text = "Ambiente";
            // 
            // comboBox_UF
            // 
            this.comboBox_UF.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.comboBox_UF.FormattingEnabled = true;
            this.comboBox_UF.IntegralHeight = false;
            this.comboBox_UF.ItemHeight = 19;
            this.comboBox_UF.Location = new System.Drawing.Point(183, 64);
            this.comboBox_UF.MaxDropDownItems = 15;
            this.comboBox_UF.Name = "comboBox_UF";
            this.comboBox_UF.Size = new System.Drawing.Size(148, 25);
            this.comboBox_UF.TabIndex = 7;
            this.comboBox_UF.UseSelectable = true;
            this.comboBox_UF.DropDown += new System.EventHandler(this.comboBox_UF_DropDown);
            this.comboBox_UF.SelectedIndexChanged += new System.EventHandler(this.comboBox_UF_SelectedIndexChanged);
            this.comboBox_UF.DropDownClosed += new System.EventHandler(this.comboBox_UF_DropDownClosed);
            // 
            // labelUF
            // 
            this.labelUF.AutoSize = true;
            this.labelUF.FontSize = MetroFramework.MetroLabelSize.Small;
            this.labelUF.Location = new System.Drawing.Point(183, 46);
            this.labelUF.Margin = new System.Windows.Forms.Padding(3);
            this.labelUF.Name = "labelUF";
            this.labelUF.Size = new System.Drawing.Size(127, 15);
            this.labelUF.TabIndex = 6;
            this.labelUF.Text = "Unidade Federativa (UF)";
            // 
            // cbServico
            // 
            this.cbServico.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbServico.FormattingEnabled = true;
            this.cbServico.IntegralHeight = false;
            this.cbServico.ItemHeight = 19;
            this.cbServico.Location = new System.Drawing.Point(3, 64);
            this.cbServico.MaxDropDownItems = 15;
            this.cbServico.Name = "cbServico";
            this.cbServico.Size = new System.Drawing.Size(174, 25);
            this.cbServico.TabIndex = 5;
            this.cbServico.UseSelectable = true;
            this.cbServico.SelectedIndexChanged += new System.EventHandler(this.cbServico_SelectedIndexChanged);
            // 
            // checkBoxGravarEventosNaPastaEnviadosNFe
            // 
            this.checkBoxGravarEventosNaPastaEnviadosNFe.AutoSize = true;
            this.checkBoxGravarEventosNaPastaEnviadosNFe.Location = new System.Drawing.Point(3, 236);
            this.checkBoxGravarEventosNaPastaEnviadosNFe.Name = "checkBoxGravarEventosNaPastaEnviadosNFe";
            this.checkBoxGravarEventosNaPastaEnviadosNFe.Size = new System.Drawing.Size(542, 15);
            this.checkBoxGravarEventosNaPastaEnviadosNFe.TabIndex = 26;
            this.checkBoxGravarEventosNaPastaEnviadosNFe.Text = "Gravar os eventos na mesma pasta dos arquivos de NFe/NFCe/MDFe/CTe autorizados/de" +
    "negados?";
            this.checkBoxGravarEventosNaPastaEnviadosNFe.UseSelectable = true;
            this.checkBoxGravarEventosNaPastaEnviadosNFe.CheckedChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe
            // 
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.AutoSize = true;
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Location = new System.Drawing.Point(3, 257);
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Name = "checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe";
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Size = new System.Drawing.Size(636, 15);
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.TabIndex = 27;
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Text = "Gravar os eventos de cancelamento na mesma pasta dos arquivos da NFe/NFCe/MDFe/CT" +
    "e autorizados/denegados?";
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.UseSelectable = true;
            this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.CheckedChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // metroLabel19
            // 
            this.metroLabel19.AutoSize = true;
            this.metroLabel19.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel19.Location = new System.Drawing.Point(3, 46);
            this.metroLabel19.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel19.Name = "metroLabel19";
            this.metroLabel19.Size = new System.Drawing.Size(43, 15);
            this.metroLabel19.TabIndex = 4;
            this.metroLabel19.Text = "Serviço";
            // 
            // lbl_DiretorioSalvarComo
            // 
            this.lbl_DiretorioSalvarComo.AutoSize = true;
            this.lbl_DiretorioSalvarComo.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_DiretorioSalvarComo.Location = new System.Drawing.Point(3, 96);
            this.lbl_DiretorioSalvarComo.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_DiretorioSalvarComo.Name = "lbl_DiretorioSalvarComo";
            this.lbl_DiretorioSalvarComo.Size = new System.Drawing.Size(356, 15);
            this.lbl_DiretorioSalvarComo.TabIndex = 12;
            this.lbl_DiretorioSalvarComo.Text = "Como devem ser criados os diretórios baseados na data de emissão?";
            // 
            // metroLabel21
            // 
            this.metroLabel21.AutoSize = true;
            this.metroLabel21.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel21.Location = new System.Drawing.Point(183, 3);
            this.metroLabel21.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel21.Name = "metroLabel21";
            this.metroLabel21.Size = new System.Drawing.Size(39, 15);
            this.metroLabel21.TabIndex = 2;
            this.metroLabel21.Text = "Nome";
            // 
            // edtNome
            // 
            this.edtNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtNome.Lines = new string[] {
        "Styled Textbox"};
            this.edtNome.Location = new System.Drawing.Point(183, 21);
            this.edtNome.MaxLength = 32767;
            this.edtNome.Name = "edtNome";
            this.edtNome.PasswordChar = '\0';
            this.edtNome.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtNome.SelectedText = "";
            this.edtNome.Size = new System.Drawing.Size(515, 22);
            this.edtNome.TabIndex = 3;
            this.edtNome.Text = "Styled Textbox";
            this.edtNome.UseSelectable = true;
            this.edtNome.TextChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // metroLabel22
            // 
            this.metroLabel22.AutoSize = true;
            this.metroLabel22.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel22.Location = new System.Drawing.Point(3, 3);
            this.metroLabel22.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel22.Name = "metroLabel22";
            this.metroLabel22.Size = new System.Drawing.Size(33, 15);
            this.metroLabel22.TabIndex = 0;
            this.metroLabel22.Text = "CNPJ";
            // 
            // edtCNPJ
            // 
            this.edtCNPJ.Lines = new string[] {
        "Styled Textbox"};
            this.edtCNPJ.Location = new System.Drawing.Point(3, 21);
            this.edtCNPJ.MaxLength = 14;
            this.edtCNPJ.Name = "edtCNPJ";
            this.edtCNPJ.PasswordChar = '\0';
            this.edtCNPJ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtCNPJ.SelectedText = "";
            this.edtCNPJ.Size = new System.Drawing.Size(174, 22);
            this.edtCNPJ.TabIndex = 1;
            this.edtCNPJ.Text = "Styled Textbox";
            this.edtCNPJ.UseSelectable = true;
            this.edtCNPJ.TextChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            this.edtCNPJ.Enter += new System.EventHandler(this.edtCNPJ_Enter);
            this.edtCNPJ.Leave += new System.EventHandler(this.edtCNPJ_Leave);
            // 
            // checkBoxGravarEventosDeTerceiros
            // 
            this.checkBoxGravarEventosDeTerceiros.AutoSize = true;
            this.checkBoxGravarEventosDeTerceiros.Location = new System.Drawing.Point(3, 278);
            this.checkBoxGravarEventosDeTerceiros.Name = "checkBoxGravarEventosDeTerceiros";
            this.checkBoxGravarEventosDeTerceiros.Size = new System.Drawing.Size(384, 15);
            this.checkBoxGravarEventosDeTerceiros.TabIndex = 28;
            this.checkBoxGravarEventosDeTerceiros.Text = "Gravar os eventos na consulta de NFe/NFCe/MDFe/CTe de terceiros?";
            this.checkBoxGravarEventosDeTerceiros.UseSelectable = true;
            this.checkBoxGravarEventosDeTerceiros.CheckedChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // lbl_UsuarioWS
            // 
            this.lbl_UsuarioWS.AutoSize = true;
            this.lbl_UsuarioWS.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_UsuarioWS.Location = new System.Drawing.Point(363, 143);
            this.lbl_UsuarioWS.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_UsuarioWS.Name = "lbl_UsuarioWS";
            this.lbl_UsuarioWS.Size = new System.Drawing.Size(67, 15);
            this.lbl_UsuarioWS.TabIndex = 20;
            this.lbl_UsuarioWS.Text = "WS-Usuário";
            // 
            // txtUsuarioWS
            // 
            this.txtUsuarioWS.Lines = new string[] {
        "Styled Textbox"};
            this.txtUsuarioWS.Location = new System.Drawing.Point(363, 161);
            this.txtUsuarioWS.MaxLength = 32767;
            this.txtUsuarioWS.Name = "txtUsuarioWS";
            this.txtUsuarioWS.PasswordChar = '\0';
            this.txtUsuarioWS.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUsuarioWS.SelectedText = "";
            this.txtUsuarioWS.Size = new System.Drawing.Size(134, 22);
            this.txtUsuarioWS.TabIndex = 21;
            this.txtUsuarioWS.Text = "Styled Textbox";
            this.txtUsuarioWS.UseSelectable = true;
            this.txtUsuarioWS.UseStyleColors = true;
            this.txtUsuarioWS.TextChanged += new System.EventHandler(this.txtUsuarioWS_TextChanged);
            // 
            // lbl_SenhaWS
            // 
            this.lbl_SenhaWS.AutoSize = true;
            this.lbl_SenhaWS.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_SenhaWS.Location = new System.Drawing.Point(503, 143);
            this.lbl_SenhaWS.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_SenhaWS.Name = "lbl_SenhaWS";
            this.lbl_SenhaWS.Size = new System.Drawing.Size(59, 15);
            this.lbl_SenhaWS.TabIndex = 22;
            this.lbl_SenhaWS.Text = "WS-Senha";
            // 
            // txtSenhaWS
            // 
            this.txtSenhaWS.Lines = new string[] {
        "Styled Textbox"};
            this.txtSenhaWS.Location = new System.Drawing.Point(503, 161);
            this.txtSenhaWS.MaxLength = 32767;
            this.txtSenhaWS.Name = "txtSenhaWS";
            this.txtSenhaWS.PasswordChar = '*';
            this.txtSenhaWS.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSenhaWS.SelectedText = "";
            this.txtSenhaWS.Size = new System.Drawing.Size(134, 22);
            this.txtSenhaWS.TabIndex = 23;
            this.txtSenhaWS.Text = "Styled Textbox";
            this.txtSenhaWS.UseSelectable = true;
            this.txtSenhaWS.UseStyleColors = true;
            this.txtSenhaWS.TextChanged += new System.EventHandler(this.txtSenhaWS_TextChanged);
            // 
            // lbl_Padrao
            // 
            this.lbl_Padrao.AutoSize = true;
            this.lbl_Padrao.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_Padrao.Location = new System.Drawing.Point(503, 97);
            this.lbl_Padrao.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_Padrao.Name = "lbl_Padrao";
            this.lbl_Padrao.Size = new System.Drawing.Size(43, 15);
            this.lbl_Padrao.TabIndex = 16;
            this.lbl_Padrao.Text = "Padrão";
            // 
            // edtPadrao
            // 
            this.edtPadrao.Lines = new string[] {
        "Styled Textbox"};
            this.edtPadrao.Location = new System.Drawing.Point(503, 115);
            this.edtPadrao.MaxLength = 32767;
            this.edtPadrao.Name = "edtPadrao";
            this.edtPadrao.PasswordChar = '\0';
            this.edtPadrao.ReadOnly = true;
            this.edtPadrao.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtPadrao.SelectedText = "";
            this.edtPadrao.Size = new System.Drawing.Size(134, 22);
            this.edtPadrao.TabIndex = 17;
            this.edtPadrao.TabStop = false;
            this.edtPadrao.Text = "Styled Textbox";
            this.edtPadrao.UseSelectable = true;
            this.edtPadrao.UseStyleColors = true;
            // 
            // lbl_CodMun
            // 
            this.lbl_CodMun.AutoSize = true;
            this.lbl_CodMun.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_CodMun.Location = new System.Drawing.Point(363, 97);
            this.lbl_CodMun.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_CodMun.Name = "lbl_CodMun";
            this.lbl_CodMun.Size = new System.Drawing.Size(111, 15);
            this.lbl_CodMun.TabIndex = 14;
            this.lbl_CodMun.Text = "Código do municipio";
            // 
            // edtCodMun
            // 
            this.edtCodMun.Lines = new string[] {
        "Styled Textbox"};
            this.edtCodMun.Location = new System.Drawing.Point(363, 115);
            this.edtCodMun.MaxLength = 32767;
            this.edtCodMun.Name = "edtCodMun";
            this.edtCodMun.PasswordChar = '\0';
            this.edtCodMun.ReadOnly = true;
            this.edtCodMun.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtCodMun.SelectedText = "";
            this.edtCodMun.Size = new System.Drawing.Size(134, 22);
            this.edtCodMun.TabIndex = 15;
            this.edtCodMun.TabStop = false;
            this.edtCodMun.Text = "Styled Textbox";
            this.edtCodMun.UseSelectable = true;
            this.edtCodMun.UseStyleColors = true;
            // 
            // grpQRCode
            // 
            this.grpQRCode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.grpQRCode.Controls.Add(this.edtTokenCSC);
            this.grpQRCode.Controls.Add(this.edtIdentificadorCSC);
            this.grpQRCode.Controls.Add(this.metroLabel2);
            this.grpQRCode.Controls.Add(this.metroLabel1);
            this.grpQRCode.Location = new System.Drawing.Point(3, 362);
            this.grpQRCode.Name = "grpQRCode";
            this.grpQRCode.Size = new System.Drawing.Size(681, 67);
            this.grpQRCode.TabIndex = 32;
            this.grpQRCode.TabStop = false;
            this.grpQRCode.Text = "Dados QR-Code";
            this.grpQRCode.Visible = false;
            // 
            // edtTokenCSC
            // 
            this.edtTokenCSC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtTokenCSC.Lines = new string[] {
        "Styled Textbox"};
            this.edtTokenCSC.Location = new System.Drawing.Point(360, 37);
            this.edtTokenCSC.MaxLength = 6;
            this.edtTokenCSC.Name = "edtTokenCSC";
            this.edtTokenCSC.PasswordChar = '\0';
            this.edtTokenCSC.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtTokenCSC.SelectedText = "";
            this.edtTokenCSC.Size = new System.Drawing.Size(315, 22);
            this.edtTokenCSC.TabIndex = 21;
            this.edtTokenCSC.Text = "Styled Textbox";
            this.edtTokenCSC.UseSelectable = true;
            this.edtTokenCSC.TextChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // edtIdentificadorCSC
            // 
            this.edtIdentificadorCSC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtIdentificadorCSC.Lines = new string[] {
        "Styled Textbox"};
            this.edtIdentificadorCSC.Location = new System.Drawing.Point(20, 37);
            this.edtIdentificadorCSC.MaxLength = 36;
            this.edtIdentificadorCSC.Name = "edtIdentificadorCSC";
            this.edtIdentificadorCSC.PasswordChar = '\0';
            this.edtIdentificadorCSC.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtIdentificadorCSC.SelectedText = "";
            this.edtIdentificadorCSC.Size = new System.Drawing.Size(327, 22);
            this.edtIdentificadorCSC.TabIndex = 20;
            this.edtIdentificadorCSC.Text = "Styled Textbox";
            this.edtIdentificadorCSC.UseSelectable = true;
            this.edtIdentificadorCSC.TextChanged += new System.EventHandler(this.comboBox_Ambiente_SelectedIndexChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel2.Location = new System.Drawing.Point(360, 19);
            this.metroLabel2.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(52, 15);
            this.metroLabel2.TabIndex = 18;
            this.metroLabel2.Text = "ID Token:";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.Location = new System.Drawing.Point(20, 19);
            this.metroLabel1.Margin = new System.Windows.Forms.Padding(3);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(30, 15);
            this.metroLabel1.TabIndex = 16;
            this.metroLabel1.Text = "CSC:";
            // 
            // userConfiguracao_diversos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.grpQRCode);
            this.Controls.Add(this.lbl_Padrao);
            this.Controls.Add(this.edtPadrao);
            this.Controls.Add(this.lbl_CodMun);
            this.Controls.Add(this.edtCodMun);
            this.Controls.Add(this.lbl_SenhaWS);
            this.Controls.Add(this.txtSenhaWS);
            this.Controls.Add(this.lbl_UsuarioWS);
            this.Controls.Add(this.txtUsuarioWS);
            this.Controls.Add(this.checkBoxRetornoNFETxt);
            this.Controls.Add(this.cbIndSinc);
            this.Controls.Add(this.lbl_udTempoConsulta);
            this.Controls.Add(this.checkBoxCompactaNFe);
            this.Controls.Add(this.udTempoConsulta);
            this.Controls.Add(this.lbl_udDiasLimpeza);
            this.Controls.Add(this.udDiasLimpeza);
            this.Controls.Add(this.cboDiretorioSalvarComo);
            this.Controls.Add(this.comboBox_tpEmis);
            this.Controls.Add(this.metroLabel11);
            this.Controls.Add(this.comboBox_Ambiente);
            this.Controls.Add(this.metroLabel10);
            this.Controls.Add(this.comboBox_UF);
            this.Controls.Add(this.labelUF);
            this.Controls.Add(this.cbServico);
            this.Controls.Add(this.checkBoxGravarEventosNaPastaEnviadosNFe);
            this.Controls.Add(this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe);
            this.Controls.Add(this.metroLabel19);
            this.Controls.Add(this.lbl_DiretorioSalvarComo);
            this.Controls.Add(this.metroLabel21);
            this.Controls.Add(this.edtNome);
            this.Controls.Add(this.metroLabel22);
            this.Controls.Add(this.edtCNPJ);
            this.Controls.Add(this.checkBoxGravarEventosDeTerceiros);
            this.Name = "userConfiguracao_diversos";
            this.Size = new System.Drawing.Size(701, 433);
            this.grpQRCode.ResumeLayout(false);
            this.grpQRCode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroCheckBox checkBoxRetornoNFETxt;
        private MetroFramework.Controls.MetroCheckBox cbIndSinc;
        private MetroFramework.Controls.MetroLabel lbl_udTempoConsulta;
        private MetroFramework.Controls.MetroCheckBox checkBoxCompactaNFe;
        private MetroFramework.Controls.MetroTextBox udTempoConsulta;
        private MetroFramework.Controls.MetroLabel lbl_udDiasLimpeza;
        private MetroFramework.Controls.MetroTextBox udDiasLimpeza;
        private MetroFramework.Controls.MetroComboBox cboDiretorioSalvarComo;
        private MetroFramework.Controls.MetroComboBox comboBox_tpEmis;
        private MetroFramework.Controls.MetroLabel metroLabel11;
        private MetroFramework.Controls.MetroComboBox comboBox_Ambiente;
        private MetroFramework.Controls.MetroLabel metroLabel10;
        private MetroFramework.Controls.MetroComboBox comboBox_UF;
        private MetroFramework.Controls.MetroLabel labelUF;
        private MetroFramework.Controls.MetroComboBox cbServico;
        private MetroFramework.Controls.MetroCheckBox checkBoxGravarEventosNaPastaEnviadosNFe;
        private MetroFramework.Controls.MetroCheckBox checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe;
        private MetroFramework.Controls.MetroLabel metroLabel19;
        private MetroFramework.Controls.MetroLabel lbl_DiretorioSalvarComo;
        private MetroFramework.Controls.MetroLabel metroLabel21;
        private MetroFramework.Controls.MetroTextBox edtNome;
        private MetroFramework.Controls.MetroLabel metroLabel22;
        private MetroFramework.Controls.MetroTextBox edtCNPJ;
        private MetroFramework.Controls.MetroCheckBox checkBoxGravarEventosDeTerceiros;
        private MetroFramework.Controls.MetroLabel lbl_UsuarioWS;
        private MetroFramework.Controls.MetroTextBox txtUsuarioWS;
        private MetroFramework.Controls.MetroLabel lbl_SenhaWS;
        private MetroFramework.Controls.MetroTextBox txtSenhaWS;
        private MetroFramework.Controls.MetroLabel lbl_Padrao;
        private MetroFramework.Controls.MetroTextBox edtPadrao;
        private MetroFramework.Controls.MetroLabel lbl_CodMun;
        private MetroFramework.Controls.MetroTextBox edtCodMun;
        private System.Windows.Forms.GroupBox grpQRCode;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox edtTokenCSC;
        private MetroFramework.Controls.MetroTextBox edtIdentificadorCSC;
    }
}

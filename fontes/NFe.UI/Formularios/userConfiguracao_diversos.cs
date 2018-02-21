using NFe.Components;
using NFe.Settings;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_diversos : MetroFramework.Controls.MetroUserControl
    {
        private ArrayList arrServico = new ArrayList();
        private Empresa empresa;
        private TipoAplicativo servicoCurrent;
        private bool loading;
        private string cnpjCurrent = "";

        public event EventHandler changeEvent;

        public userConfiguracoes uConfiguracoes;
        private ArrayList arrUF, arrMunicipios;

        public userConfiguracao_diversos()
        {
            InitializeComponent();

            this.loading = true;

            if (!DesignMode)
            {
                this.cbServico.SelectedIndexChanged -= cbServico_SelectedIndexChanged;
                servicoCurrent = TipoAplicativo.Nulo;

                #region Montar Array DropList da UF

                try
                {
                    arrUF = Functions.CarregaEstados();
                    arrMunicipios = Functions.CarregaMunicipios();
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                }

                #endregion Montar Array DropList da UF

                #region Montar Array DropList do Ambiente
                comboBox_Ambiente.DataSource = EnumHelper.ToList(typeof(TipoAmbiente), true, true);
                comboBox_Ambiente.DisplayMember = "Value";
                comboBox_Ambiente.ValueMember = "Key";
                #endregion Montar Array DropList do Ambiente

                #region Montar array DropList dos tipos de serviços
                this.cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo(false);
                this.cbServico.DisplayMember = "Value";
                this.cbServico.ValueMember = "Key";
                #endregion Montar array DropList dos tipos de serviços

                #region Montar Array DropList do Tipo de Emissão da NF-e
                comboBox_tpEmis.DataSource = EnumHelper.ToList(typeof(TipoEmissao), true, true);
                comboBox_tpEmis.DisplayMember = "Value";
                comboBox_tpEmis.ValueMember = "Key";
                #endregion Montar Array DropList do Tipo de Emissão da NF-e

                this.cbServico.SelectedIndexChanged += cbServico_SelectedIndexChanged;

                loc_1 = lbl_udDiasLimpeza.Location;
                loc_2 = udDiasLimpeza.Location;
            }
        }

        private Point loc_1, loc_2;

        public void Populate(Empresa empresa, bool novaempresa)
        {
            this.loading = true;
            try
            {
                uninfeDummy.ClearControls(this, true, false);

                this.empresa = empresa;

                Configurar(empresa, novaempresa);

                if (empresa.Servico == TipoAplicativo.Nfse)
                    comboBox_UF.DataSource = arrMunicipios;
                else
                    comboBox_UF.DataSource = arrUF;

                comboBox_UF.DisplayMember = NFeStrConstants.Nome;
                comboBox_UF.ValueMember = "Codigo";

                cnpjCurrent = this.edtCNPJ.Text = empresa.CNPJ;
                this.edtNome.Text = empresa.Nome;

                if (!string.IsNullOrEmpty(empresa.CNPJ))
                    this.edtCNPJ.Text = uninfeDummy.FmtCnpjCpf(this.edtCNPJ.Text, true);

                comboBox_tpEmis.SelectedValue = this.empresa.tpEmis;
                comboBox_Ambiente.SelectedValue = this.empresa.AmbienteCodigo;
                comboBox_UF.SelectedValue = this.empresa.UnidadeFederativaCodigo;
                cbServico.SelectedValue = (int)this.empresa.Servico;

                if (empresa.Servico == TipoAplicativo.Nfse && this.empresa.UnidadeFederativaCodigo == 0)
                    comboBox_UF.SelectedIndex = 0;

                checkBoxRetornoNFETxt.Checked = this.empresa.GravarRetornoTXTNFe;
                checkBoxGravarEventosDeTerceiros.Checked = this.empresa.GravarEventosDeTerceiros;
                checkBoxGravarEventosNaPastaEnviadosNFe.Checked = this.empresa.GravarEventosNaPastaEnviadosNFe;
                checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked = this.empresa.GravarEventosCancelamentoNaPastaEnviadosNFe;
                checkBoxCompactaNFe.Checked = this.empresa.CompactarNfe;
                checkBoxArqNSU.Checked = this.empresa.ArqNSU;

                // São Paulo não possui processo síncrono
                if (this.empresa.UnidadeFederativaCodigo == 35)
                    cbIndSinc.Checked =
                    cbIndSinc.Enabled = false;
                else
                {
                    cbIndSinc.Enabled = true;
                    cbIndSinc.Checked = this.empresa.IndSinc;
                }

                edtIdentificadorCSC.Text = this.empresa.IdentificadorCSC;
                edtTokenCSC.Text = this.empresa.TokenCSC;

                cboDiretorioSalvarComo.Text = this.empresa.DiretorioSalvarComo;
                udDiasLimpeza.Text = this.empresa.DiasLimpeza.ToString();
                udTempoConsulta.Text = this.empresa.TempoConsulta.ToString();

                this.txtSenhaWS.Text = this.empresa.SenhaWS;
                this.txtUsuarioWS.Text = this.empresa.UsuarioWS;
                this.txtClienteID.Text = this.empresa.ClientID;
                this.txtClientSecret.Text = this.empresa.ClientSecret;

                HabilitaUsuarioSenhaWS(this.empresa.UnidadeFederativaCodigo);
                servicoCurrent = this.empresa.Servico;

                HabilitaOpcaoCompactar(this.empresa.Servico == TipoAplicativo.Nfe);

                this.edtCNPJ.ReadOnly = !string.IsNullOrEmpty(empresa.CNPJ);
                this.cbServico.Enabled = !this.edtCNPJ.ReadOnly;

                if (this.empresa.Servico != TipoAplicativo.Nfse && !novaempresa)
                    this.cbServico.Enabled = true;
            }
            finally
            {
                this.loading = false;
                cbServico_SelectedIndexChanged(null, null);
                comboBox_UF_SelectedIndexChanged(null, null);
            }
        }

        public bool Validar(bool exibeerro, bool novaempresa)
        {
            string cnpj = (string)Functions.OnlyNumbers(this.edtCNPJ.Text, ".-/");

            if (Convert.ToInt32("0" + udTempoConsulta.Text) < 2 || Convert.ToInt32("0" + udTempoConsulta.Text) > 15)
                throw new Exception(lbl_udTempoConsulta.Text + " inválido");

            if (this.comboBox_UF.SelectedValue == null)
                throw new Exception(labelUF.Text + " deve ser informado");

            ValidadeCNPJ(true);

            if (string.IsNullOrEmpty(edtNome.Text))
                throw new Exception("Nome da empresa deve ser informado");

            if (servicoCurrent != (TipoAplicativo)this.cbServico.SelectedValue && !novaempresa && exibeerro)
            {
                if ((TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.Nfse)
                {
                    throw new Exception("Não pode mudar para esse tipo de serviço (NFSe)");
                }

                if ((TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.SAT)
                {
                    throw new Exception("Não pode mudar para esse tipo de serviço (SAT)");
                }

                var e = Empresas.FindConfEmpresa(cnpj, (TipoAplicativo)this.cbServico.SelectedValue);
                if (e != null)
                    throw new Exception("A empresa '" + e.Nome + "' já está monitorando esse tipo de serviço");

                if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Confirma a alteração do tipo de serviço?", "",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return false;
                }
            }

            switch ((TipoAplicativo)this.cbServico.SelectedValue)
            {
                case TipoAplicativo.NFCe:
                    if (!string.IsNullOrEmpty(this.edtIdentificadorCSC.Text) && string.IsNullOrEmpty(this.edtTokenCSC.Text))
                    {
                        throw new Exception("É obrigatório informar o IDToken quando informado o CSC.");
                    }
                    else if (string.IsNullOrEmpty(this.edtIdentificadorCSC.Text) && !string.IsNullOrEmpty(this.edtTokenCSC.Text))
                    {
                        throw new Exception("É obrigatório informar o CSC quando informado o IDToken.");
                    }

                    break;
            }

            this.empresa.AmbienteCodigo = (int)comboBox_Ambiente.SelectedValue;
            this.empresa.CNPJ = cnpj;
            this.empresa.CompactarNfe = checkBoxCompactaNFe.Checked;
            this.empresa.ArqNSU = checkBoxArqNSU.Checked;
            this.empresa.DiasLimpeza = Math.Abs(Convert.ToInt32("0" + this.udDiasLimpeza.Text));
            this.empresa.DiretorioSalvarComo = cboDiretorioSalvarComo.Text;
            this.empresa.GravarRetornoTXTNFe = checkBoxRetornoNFETxt.Checked;
            this.empresa.GravarEventosDeTerceiros = checkBoxGravarEventosDeTerceiros.Checked;
            this.empresa.GravarEventosNaPastaEnviadosNFe = checkBoxGravarEventosNaPastaEnviadosNFe.Checked;
            this.empresa.GravarEventosCancelamentoNaPastaEnviadosNFe = checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked;
            this.empresa.Nome = this.edtNome.Text;
            this.empresa.IndSinc = this.cbIndSinc.Checked;
            this.empresa.TempoConsulta = Math.Abs(Convert.ToInt32("0" + this.udTempoConsulta.Text));
            this.empresa.tpEmis = (int)comboBox_tpEmis.SelectedValue;
            this.empresa.UnidadeFederativaCodigo = (int)comboBox_UF.SelectedValue;
            this.empresa.Servico = (TipoAplicativo)this.cbServico.SelectedValue;
            this.empresa.SenhaWS = this.txtSenhaWS.Text;
            this.empresa.UsuarioWS = this.txtUsuarioWS.Text;
            this.empresa.IdentificadorCSC = this.edtIdentificadorCSC.Text;
            this.empresa.TokenCSC = this.edtTokenCSC.Text;
            this.empresa.ClientID = this.txtClienteID.Text;
            this.empresa.ClientSecret = this.txtClientSecret.Text;

            return true;
        }

        private void HabilitaOpcaoCompactar(bool ativar)
        {
            if (this.empresa.Servico == TipoAplicativo.Nfse) ativar = false;
            checkBoxCompactaNFe.Visible = ativar;
        }

        private void udDiasLimpeza_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar);
        }

        private void cbServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.loading)
                return;

            this.grpQRCode.Visible = (TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.NFCe ||
                                     (TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.Nfe ||
                                     (TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.Todos;

            if ((TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.Nfe ||
                (TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.NFCe ||
                (TipoAplicativo)this.cbServico.SelectedValue == TipoAplicativo.Todos)
            {
                HabilitaOpcaoCompactar(true);
            }
            else
            {
                HabilitaOpcaoCompactar(false);
                checkBoxCompactaNFe.Checked = false;
            }
            if (this.changeEvent != null)
                this.changeEvent(sender, e);
        }

        private void HabilitaUsuarioSenhaWS(int ufCod)
        {
            bool visible = ufCod == 4101408 /*Apucarana*/ ||
                           ufCod == 3502804 /*Araçatuba*/||
                           ufCod == 4104303 /*Campo Mourão*/||
                           ufCod == 4104204 /*Campo Largo*/||
                           ufCod == 3537305 /*Penapolis*/||
                           ufCod == 4309209 /*Gravataí*/||
                           ufCod == 3551702 /*Sertãozinho*/||
                           ufCod == 3201308 /*Cariacica*/||
                           ufCod == 3538709 /*Piracicaba*/||
                           ufCod == 2930709 /*Simões Filho*/||
                           ufCod == 3553807 /*Taquarituba*/||
                           ufCod == 3512902 /*Cosmorama*/||
                           ufCod == 3515004 /*Embu das Artes*/||
                           ufCod == 3148004 /*Patos de Minas*/||
                           ufCod == 3506508 /*Birigui*/||
                           ufCod == 3201506 /*Colatina*/||
                           ufCod == 4118204 /*Paranaguá*/||
                           ufCod == 3130309 /*Iguatama*/||
                           ufCod == 3504503 /*Avaré*/||
                           ufCod == 3541406 /*Presidente Prudente-SP*/||
                           ufCod == 4217808 /*Taió-SC*/||
                           ufCod == 4101101 /*Andirá-PR*/||
                           ufCod == 3306305 /*Volta Redonda-RJ*/||
                           ufCod == 3522802 /*Itaporanga-SP*/||
                           ufCod == 4201307 /*Araquari-SC*/||
                           ufCod == 3205002 /*Serra-ES*/||
                           ufCod == 3504008 /*Assis-SP*/||
                           ufCod == 4202008 /*Balneário Camboriú-SC*/||
                           ufCod == 3148103 /*Patrocínio-MG*/||
                           ufCod == 3524501 /*Jaci-SP*/||
                           ufCod == 3300407 /*Barra Mansa-RJ*/ ||
                           ufCod == 3530409 /*Mirassolândia-SP*/ ||
                           ufCod == 3528809 /*Macaraí-SP*/ ||
                           ufCod == 5003207 /*Corumba-MS*/ ||
                           ufCod == 1600303 /*Macapá-AP*/ ||
                           ufCod == 3202603 /*Iconha-ES*/ ||
                           ufCod == 4205407 /*Florianópolis-SC*/ ||
                           ufCod == 4215802 /*São Bento do Sul-SC*/ ||
                           ufCod == 3540804 /*Potirendaba-SP*/ ||
                           ufCod == 4320404 /*Serafina Corrêa-RS*/ ||
                           ufCod == 4307807 /*Estrela-RS*/ ||
                           ufCod == 4211900 /*Palhoça-SC*/;

            bool visiblepass = ufCod == 3152105 || visible; /*Ponte nova*/

            lbl_UsuarioWS.Visible =
                txtUsuarioWS.Visible = visible;

            lbl_SenhaWS.Visible =
                txtSenhaWS.Visible = visiblepass;
        }

        private void comboBox_UF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.loading)
                return;

            // danasa 1-2012
            try
            {
                object xuf = this.comboBox_UF.SelectedValue;

                edtCodMun.Text = xuf.ToString();
                //edtPadrao.Text = Functions.PadraoNFSe(Convert.ToInt32(xuf)).ToString();

                edtPadrao.Text = EnumHelper.GetEnumItemDescription(Functions.PadraoNFSe(Convert.ToInt32(xuf)));
                HabilitaUsuarioSenhaWS(Convert.ToInt32(edtCodMun.Text));

                //Se o município for Florianópolis, temos que demonstrar os campos: ClientID e Client Secret
                if (edtCodMun.Text.Equals("4205407"))
                {
                    lblClienteID.Visible = true;
                    lblClientSecret.Visible = true;
                    txtClienteID.Visible = true;
                    txtClientSecret.Visible = true;
                }
                else
                {
                    lblClienteID.Visible = false;
                    lblClientSecret.Visible = false;
                    txtClienteID.Visible = false;
                    txtClientSecret.Visible = false;
                }
            }
            catch
            {
                HabilitaUsuarioSenhaWS(-1);
                edtCodMun.Text = edtPadrao.Text = "Indefinido";
            }
            if (this.changeEvent != null)
                this.changeEvent(sender, e);
        }

        private void comboBox_Ambiente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.changeEvent != null)
                this.changeEvent(sender, e);
        }

        public bool ValidadeCNPJ(bool istrow = false)
        {
            return true;
        }

        private void edtCNPJ_Leave(object sender, EventArgs e)
        {
            this.ValidadeCNPJ();
        }

        private void edtCNPJ_Enter(object sender, EventArgs e)
        {
        }

        private void comboBox_UF_DropDownClosed(object sender, EventArgs e)
        {
            if (this.empresa.Servico == TipoAplicativo.Nfse)
                comboBox_UF.DropDownWidth = comboBox_UF.Width;
        }

        private void comboBox_UF_DropDown(object sender, EventArgs e)
        {
            if (this.empresa.Servico == TipoAplicativo.Nfse)
                comboBox_UF.DropDownWidth = 300;
        }

        private void txtUsuarioWS_TextChanged(object sender, EventArgs e)
        {
            if (this.changeEvent != null)
                this.changeEvent(sender, e);
        }

        private void txtSenhaWS_TextChanged(object sender, EventArgs e)
        {
            if (this.changeEvent != null)
                this.changeEvent(sender, e);
        }

        private void Configurar(Empresa empresa, bool novaempresa)
        {
            switch (empresa.Servico)
            {
                case TipoAplicativo.Nfse:
                    labelUF.Visible = true;
                    labelUF.Text = "Município";
                    comboBox_UF.Visible = true;
                    lbl_udDiasLimpeza.Location = new Point(this.lbl_udTempoConsulta.Location.X, this.lbl_udTempoConsulta.Location.Y);
                    udDiasLimpeza.Location = new Point(this.udTempoConsulta.Location.X, this.udTempoConsulta.Location.Y);
                    lbl_CodMun.Visible = true;
                    edtCodMun.Visible = true;
                    edtPadrao.Visible = true;
                    lbl_Padrao.Visible = true;
                    cboDiretorioSalvarComo.Visible = false;
                    lbl_DiretorioSalvarComo.Visible = false;
                    comboBox_tpEmis.Visible = false;
                    metroLabel11.Visible = false;
                    checkBoxGravarEventosNaPastaEnviadosNFe.Visible = false;
                    checkBoxRetornoNFETxt.Visible = false;
                    checkBoxGravarEventosDeTerceiros.Visible = false;
                    checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible = false;
                    checkBoxCompactaNFe.Visible = false;
                    udTempoConsulta.Visible = lbl_udTempoConsulta.Visible = false;
                    cbIndSinc.Visible = false;
                    comboBox_Ambiente.Visible = true;
                    checkBoxArqNSU.Visible = false;
                    break;

                case TipoAplicativo.SAT:
                    labelUF.Visible = true;
                    labelUF.Text = "Unidade Federativa (UF)";
                    comboBox_UF.Visible = true;
                    cboDiretorioSalvarComo.Visible = false;
                    lbl_DiretorioSalvarComo.Visible = false;
                    comboBox_tpEmis.Visible = false;
                    metroLabel11.Visible = false;
                    checkBoxGravarEventosNaPastaEnviadosNFe.Visible = false;
                    checkBoxRetornoNFETxt.Visible = false;
                    checkBoxGravarEventosDeTerceiros.Visible = false;
                    checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible = false;
                    checkBoxCompactaNFe.Visible = false;
                    udTempoConsulta.Visible = lbl_udTempoConsulta.Visible = false;
                    cbIndSinc.Visible = false;
                    metroLabel10.Visible = false;
                    comboBox_Ambiente.Visible = false;
                    lbl_CodMun.Visible = false;
                    edtCodMun.Visible = false;
                    edtPadrao.Visible = false;
                    lbl_Padrao.Visible = false;
                    lblClienteID.Visible = false;
                    lblClientSecret.Visible = false;
                    txtClienteID.Visible = false;
                    txtClientSecret.Visible = false;
                    checkBoxArqNSU.Visible = false;
                    break;

                case TipoAplicativo.EFDReinf:
                case TipoAplicativo.eSocial:
                case TipoAplicativo.EFDReinfeSocial:
                    comboBox_UF.Visible = false;
                    comboBox_tpEmis.Visible = false;
                    udTempoConsulta.Visible = false;
                    checkBoxCompactaNFe.Visible = false;
                    checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible = false;
                    checkBoxGravarEventosDeTerceiros.Visible = false;
                    checkBoxGravarEventosNaPastaEnviadosNFe.Visible = false;
                    checkBoxRetornoNFETxt.Visible = false;
                    cbIndSinc.Visible = false;
                    grpQRCode.Visible = false;
                    metroLabel11.Visible = false;
                    lbl_udTempoConsulta.Visible = false;
                    labelUF.Visible = false;
                    lbl_CodMun.Visible = false;
                    edtCodMun.Visible = false;
                    edtPadrao.Visible = false;
                    lbl_Padrao.Visible = false;
                    lblClienteID.Visible = false;
                    lblClientSecret.Visible = false;
                    txtClienteID.Visible = false;
                    txtClientSecret.Visible = false;
                    checkBoxArqNSU.Visible = false;
                    break;

                default:
                    labelUF.Visible = true;
                    labelUF.Text = "Unidade Federativa (UF)";
                    comboBox_UF.Visible = true;
                    lbl_udDiasLimpeza.Location = loc_1;
                    udDiasLimpeza.Location = loc_2;
                    cboDiretorioSalvarComo.Visible = true;
                    lbl_DiretorioSalvarComo.Visible = true;
                    comboBox_tpEmis.Visible = true;
                    metroLabel11.Visible = true;
                    checkBoxGravarEventosNaPastaEnviadosNFe.Visible = true;
                    checkBoxRetornoNFETxt.Visible = true;
                    checkBoxGravarEventosDeTerceiros.Visible = true;
                    checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible = true;
                    checkBoxCompactaNFe.Visible = true;
                    udTempoConsulta.Visible = lbl_udTempoConsulta.Visible = true;
                    cbIndSinc.Visible = true;
                    metroLabel10.Visible = true;
                    comboBox_Ambiente.Visible = true;
                    lbl_CodMun.Visible = false;
                    edtCodMun.Visible = false;
                    edtPadrao.Visible = false;
                    lbl_Padrao.Visible = false;
                    grpQRCode.Visible = true;
                    edtTokenCSC.Visible = true;
                    edtIdentificadorCSC.Visible = true;
                    metroLabel2.Visible = true;
                    metroLabel1.Visible = true;
                    lblClienteID.Visible = false;
                    lblClientSecret.Visible = false;
                    txtClienteID.Visible = false;
                    txtClientSecret.Visible = false;
                    checkBoxArqNSU.Visible = true;
                    break;
            }
        }
    }
}
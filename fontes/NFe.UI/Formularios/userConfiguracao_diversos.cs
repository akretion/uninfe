using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using NFe.Components;
using NFe.Settings;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_diversos : MetroFramework.Controls.MetroUserControl
    {
        private ArrayList arrServico = new ArrayList();
        private NFe.Settings.Empresa empresa;
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

                #endregion

                #region Montar Array DropList do Ambiente
                comboBox_Ambiente.DataSource = EnumHelper.ToList(typeof(TipoAmbiente), true, true);
                comboBox_Ambiente.DisplayMember = "Value";
                comboBox_Ambiente.ValueMember = "Key";
                #endregion

                #region Montar array DropList dos tipos de serviços
                this.cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo(true);
                this.cbServico.DisplayMember = "Value";
                this.cbServico.ValueMember = "Key";
                #endregion

                #region Montar Array DropList do Tipo de Emissão da NF-e
                if (Propriedade.TipoAplicativo == TipoAplicativo.Todos)//.Nfe)
                {
                    comboBox_tpEmis.DataSource = EnumHelper.ToList(typeof(TipoEmissao), true, true);
                }
                else
                {
                    ArrayList arrTpEmis = new ArrayList();
                    arrTpEmis.Add(new KeyValuePair<int, string>((int)NFe.Components.TipoEmissao.teNormal, EnumHelper.GetDescription(NFe.Components.TipoEmissao.teNormal)));
                    comboBox_tpEmis.DataSource = arrTpEmis;
                }
                comboBox_tpEmis.DisplayMember = "Value";
                comboBox_tpEmis.ValueMember = "Key";
                #endregion

                this.cbServico.SelectedIndexChanged += cbServico_SelectedIndexChanged;

                loc_1 = lbl_udDiasLimpeza.Location;
                loc_2 = udDiasLimpeza.Location;
            }
        }

        private Point loc_1, loc_2;

        public void Populate(NFe.Settings.Empresa empresa, bool novaempresa)
        {
            this.loading = true;
            try
            {
                uninfeDummy.ClearControls(this, true, false);

                this.empresa = empresa;

                if (empresa.Servico == TipoAplicativo.Nfse)
                {
                    labelUF.Text = "Município";
                    lbl_udDiasLimpeza.Location = new Point(this.lbl_udTempoConsulta.Location.X, this.lbl_udTempoConsulta.Location.Y);
                    udDiasLimpeza.Location = new Point(this.udTempoConsulta.Location.X, this.udTempoConsulta.Location.Y);
                }
                else
                {
                    labelUF.Text = "Unidade Federativa (UF)";
                    lbl_udDiasLimpeza.Location = loc_1;
                    udDiasLimpeza.Location = loc_2;
                }
                this.lbl_CodMun.Visible =
                    this.edtCodMun.Visible =
                    this.edtPadrao.Visible =
                    this.lbl_Padrao.Visible = (empresa.Servico == TipoAplicativo.Nfse);

                cboDiretorioSalvarComo.Visible =
                    lbl_DiretorioSalvarComo.Visible =
                    comboBox_tpEmis.Visible =
                    metroLabel11.Visible =
                    checkBoxGravarEventosNaPastaEnviadosNFe.Visible =
                    checkBoxRetornoNFETxt.Visible =
                    checkBoxGravarEventosDeTerceiros.Visible =
                    checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible =
                    checkBoxCompactaNFe.Visible =
                    udTempoConsulta.Visible = lbl_udTempoConsulta.Visible =
                    cbIndSinc.Visible = !(empresa.Servico == TipoAplicativo.Nfse);

                /*

                if (empresa.Servico == TipoAplicativo.Nfe || empresa.Servico == TipoAplicativo.NFCe || empresa.Servico == TipoAplicativo.Todos)
                {
                    grpQRCode.Visible =
                    edtIdentificadorCSC.Visible =
                    edtTokenCSC.Visible = true;
                }
                else
                {
                    grpQRCode.Visible =
                    edtIdentificadorCSC.Visible =
                    edtTokenCSC.Visible = false;
                }
                */

                if (empresa.Servico == TipoAplicativo.Nfse)
                    comboBox_UF.DataSource = arrMunicipios;
                else
                    comboBox_UF.DataSource = arrUF;

                comboBox_UF.DisplayMember = NFe.Components.NFeStrConstants.Nome;
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
                cbIndSinc.Checked = this.empresa.IndSinc;
                edtIdentificadorCSC.Text = this.empresa.IdentificadorCSC;
                edtTokenCSC.Text = this.empresa.TokenCSC;

                cboDiretorioSalvarComo.Text = this.empresa.DiretorioSalvarComo;
                udDiasLimpeza.Text = this.empresa.DiasLimpeza.ToString();
                udTempoConsulta.Text = this.empresa.TempoConsulta.ToString();

                this.txtSenhaWS.Text = this.empresa.SenhaWS;
                this.txtUsuarioWS.Text = this.empresa.UsuarioWS;

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
                           ufCod == 4309209 /*Gravatai*/||
                           ufCod == 3551702 /*Sertaozinho*/||
                           ufCod == 3201308 /*Cariacica*/||
                           ufCod == 3538709 /*Piracicaba*/||
                           ufCod == 2930709 /*Simoes Filho*/||
                           ufCod == 3553807 /*Taquaritua*/||
                           ufCod == 3512902 /*Cosmorama*/||
                           ufCod == 3515004 /*Embu das Artes*/||
                           ufCod == 3148004 /*Patos de Minas*/||
                           ufCod == 3506508 /*Birigui*/||
                           ufCod == 3201506 /*Colatina*/||
                           ufCod == 4118204 /*Paranagua*/||
                           ufCod == 3130309 /*Iguatama*/;

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
    }
}

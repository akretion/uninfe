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

//using NFe.Certificado;
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

        public userConfiguracao_diversos()
        {
            InitializeComponent();

            this.loading = true;

            if (!DesignMode)
            {
                this.cbServico.SelectedIndexChanged -= cbServico_SelectedIndexChanged;
                servicoCurrent = TipoAplicativo.Nulo;
                if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                {
                    labelUF.Text = "Município";
                    lbl_udDiasLimpeza.Location = new Point(this.lbl_udTempoConsulta.Location.X, this.lbl_udTempoConsulta.Location.Y);
                    udDiasLimpeza.Location = new Point(this.udTempoConsulta.Location.X, this.udTempoConsulta.Location.Y);
                }
                this.lbl_CodMun.Visible =
                    this.edtCodMun.Visible =
                    this.edtPadrao.Visible =
                    this.lbl_Padrao.Visible = Propriedade.TipoAplicativo == TipoAplicativo.Nfse;

                cboDiretorioSalvarComo.Visible = 
                    lbl_DiretorioSalvarComo.Visible =
                    checkBoxGravarEventosNaPastaEnviadosNFe.Visible =
                    checkBoxRetornoNFETxt.Visible =
                    checkBoxGravarEventosDeTerceiros.Visible =
                    checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible =
                    checkBoxCompactaNFe.Visible =
                    udTempoConsulta.Visible = lbl_udTempoConsulta.Visible =
                    cbIndSinc.Visible = (Propriedade.TipoAplicativo == TipoAplicativo.Nfe);

                if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                    this.Size = new Size(640, 300);

                #region Montar Array DropList da UF
                ArrayList arrUF = new ArrayList();

                try
                {
                    arrUF = Functions.CarregaUF();
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                }

                comboBox_UF.DataSource = arrUF;
                comboBox_UF.DisplayMember = NFe.Components.NFeStrConstants.Nome;
                comboBox_UF.ValueMember = "Codigo";
                #endregion

                #region Montar Array DropList do Ambiente
                comboBox_Ambiente.DataSource = EnumHelper.ToList(typeof(TipoAmbiente), true, true);
                comboBox_Ambiente.DisplayMember = "Value";
                comboBox_Ambiente.ValueMember = "Key";
                #endregion

                #region Montar array DropList dos tipos de serviços
                this.cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo();
                this.cbServico.DisplayMember = "Value";
                this.cbServico.ValueMember = "Key";
                #endregion

                #region Montar Array DropList do Tipo de Emissão da NF-e
                if (Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
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
            }
        }

        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.loading = true;
            try
            {
                uninfeDummy.ClearControls(this, true, false);

                this.empresa = empresa;

                cnpjCurrent = this.edtCNPJ.Text = empresa.CNPJ;
                this.edtNome.Text = empresa.Nome;

                if (!string.IsNullOrEmpty(empresa.CNPJ))
                    this.edtCNPJ.Text = uninfeDummy.FmtCgcCpf(this.edtCNPJ.Text, true);

                comboBox_tpEmis.SelectedValue = this.empresa.tpEmis;
                comboBox_Ambiente.SelectedValue = this.empresa.AmbienteCodigo;
                comboBox_UF.SelectedValue = this.empresa.UnidadeFederativaCodigo;
                cbServico.SelectedValue = (int)this.empresa.Servico;

                checkBoxRetornoNFETxt.Checked = this.empresa.GravarRetornoTXTNFe;
                checkBoxGravarEventosDeTerceiros.Checked = this.empresa.GravarEventosDeTerceiros;
                checkBoxGravarEventosNaPastaEnviadosNFe.Checked = this.empresa.GravarEventosNaPastaEnviadosNFe;
                checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked = this.empresa.GravarEventosCancelamentoNaPastaEnviadosNFe;
                checkBoxCompactaNFe.Checked = this.empresa.CompactarNfe;
                cbIndSinc.Checked = this.empresa.IndSinc;

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
            }
            finally
            {
                this.loading = false;
                cbServico_SelectedIndexChanged(null, null);
                comboBox_UF_SelectedIndexChanged(null, null);
            }
        }

        public void Validar()
        {
            if (Convert.ToInt32("0" + udTempoConsulta.Text) < 2 || Convert.ToInt32("0" + udTempoConsulta.Text) > 15)
                throw new Exception(lbl_udTempoConsulta.Text + " inválido");

            if (this.comboBox_UF.SelectedValue == null)
                throw new Exception(labelUF.Text + " deve ser informado");

            ValidadeCNPJ(true);

            if (string.IsNullOrEmpty(edtNome.Text))
                throw new Exception("Nome da empresa deve ser informado");

            string cnpj = (string)Functions.OnlyNumbers(this.edtCNPJ.Text, ".-/");
            /*
            if (!this.edtCNPJ.ReadOnly &&
                Empresa.FindConfEmpresa(cnpj, (TipoAplicativo)this.cbServico.SelectedValue) != null)
            {
                throw new Exception("Empresa/CNPJ já existe");
            }
            */

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

            if (cbServico.SelectedIndex == 0) // Somente NFe habilita este campo
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

#if false
            if (cbServico.SelectedValue != null && cbServico.Enabled)
            {
                TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedValue;

                if (servico != servicoCurrent)
                {
                    if (Empresa.FindConfEmpresa(cnpjCurrent, servico) != null)
                    {
                        this.cbServico.Focus();
                        Dialogs.ShowMessage("Empresa/CNPJ para atender o serviço de " + servico.ToString() + " já existe", 0, 0, MessageBoxIcon.Error);
                        this.cbServico.SelectedValue = servicoCurrent;
                        return;
                    }

                    bool mudaPastas = true;
                    if (!string.IsNullOrEmpty(this.uConfiguracoes.uce_pastas.textBox_PastaEnvioXML.Text))
                    {
                        mudaPastas = Dialogs.YesNo("Serviço foi alterado e você já tem as pastas definidas.\r\nDeseja mudá-las para o novo Serviço?", 0, 0);
                    }

                    if (mudaPastas)
                        MudarPastas(cnpjCurrent, servico);

                    servicoCurrent = servico;

                    if (this.changeEvent != null)
                        this.changeEvent(sender, e);
                }
                this.uConfiguracoes.uce_pastas.empresa.Servico = servicoCurrent;
            }
#endif
        }

        private void HabilitaUsuarioSenhaWS(int ufCod)
        {
            bool visible = ufCod == 4101408 /*Apucarana*/ ||
                           ufCod == 3502804 /*Araçatuba*/||
                           ufCod == 4104303 /*Campo Mourão*/||
                           ufCod == 3537305 /*Penapolis*/;

            lbl_UsuarioWS.Visible = 
                lbl_SenhaWS.Visible = 
                txtUsuarioWS.Visible =
                txtSenhaWS.Visible = visible;
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
                edtPadrao.Text = Functions.PadraoNFSe(Convert.ToInt32(xuf)).ToString();
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
#if false
            string cnpj = Functions.OnlyNumbers(this.edtCNPJ.Text, ".,-/").ToString().PadLeft(14,'0');
            this.edtCNPJ.Text = uninfeDummy.FmtCgcCpf(cnpj, true);

            if (!this.edtCNPJ.ReadOnly)
            {
                TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedValue;
                string nome = edtNome.Text;

                if (cnpjCurrent != cnpj || string.IsNullOrEmpty(cnpj))
                {
                    if (!CNPJ.Validate(cnpj) || cnpj.Equals("00000000000000"))
                    {
                        if (cnpj.Equals("00000000000000"))
                            this.edtCNPJ.Clear();

                        this.edtCNPJ.Focus();
                        if (istrow)
                            throw new Exception("CNPJ inválido");
                        Dialogs.ShowMessage("CNPJ inválido", 0, 0, MessageBoxIcon.Error);
                        return false;
                    }

                    bool mudaPastas = true;
                    if (Empresa.FindConfEmpresa(cnpj, servico) != null)
                    {
                        Dialogs.ShowMessage("Empresa/CNPJ para atender o serviço de " + servico.ToString() + " já existe", 0, 0, MessageBoxIcon.Information);

                        this.cbServico.SelectedIndexChanged -= cbServico_SelectedIndexChanged;

                        if (Empresa.FindConfEmpresa(cnpj, TipoAplicativo.Nfe) == null)
                        {
                            cbServico.SelectedValue = servicoCurrent = servico = TipoAplicativo.Nfe;
                            MudarPastas(cnpj, servicoCurrent);
                            mudaPastas = false;
                        }
                        else if (Empresa.FindConfEmpresa(cnpj, TipoAplicativo.Cte) == null)
                        {
                            cbServico.SelectedValue = servicoCurrent = servico = TipoAplicativo.Cte;
                            MudarPastas(cnpj, servicoCurrent);
                            mudaPastas = false;
                        }
                        else if (Empresa.FindConfEmpresa(cnpj, TipoAplicativo.Nfse) == null)
                        {
                            cbServico.SelectedValue = servicoCurrent = servico = TipoAplicativo.Nfse;
                            MudarPastas(cnpj, servicoCurrent);
                            mudaPastas = false;
                        }
                        else if (Empresa.FindConfEmpresa(cnpj, TipoAplicativo.MDFe) == null)
                        {
                            cbServico.SelectedValue = servicoCurrent = servico = TipoAplicativo.MDFe;
                            MudarPastas(cnpj, servicoCurrent);
                            mudaPastas = false;
                        }
                        else if (Empresa.FindConfEmpresa(cnpj, TipoAplicativo.NFCe) == null)
                        {
                            cbServico.SelectedValue = servicoCurrent = servico = TipoAplicativo.NFCe;
                            MudarPastas(cnpj, servicoCurrent);
                            mudaPastas = false;
                        }
                        else
                        {
                            this.cbServico.SelectedIndexChanged += cbServico_SelectedIndexChanged;
                            this.edtCNPJ.Focus();
                            return false;
                        }
                        this.cbServico.SelectedIndexChanged += cbServico_SelectedIndexChanged;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(this.uConfiguracoes.uce_pastas.textBox_PastaEnvioXML.Text))
                        {
                            ///
                            /// tenta achar uma configuracao valida
                            /// 
                            foreach (Empresa empresa in Empresas.Configuracoes)
                            {
                                if (empresa.CNPJ.Trim() != cnpj && !string.IsNullOrEmpty(empresa.PastaXmlEnvio))
                                {
                                    this.uConfiguracoes.uce_pastas.textBox_PastaEnvioXML.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlEnvio, cnpj);
                                    this.uConfiguracoes.uce_pastas.textBox_PastaRetornoXML.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlRetorno, cnpj);
                                    this.uConfiguracoes.uce_pastas.textBox_PastaXmlErro.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlErro, cnpj);
                                    this.uConfiguracoes.uce_pastas.textBox_PastaValidar.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaValidar, cnpj);
                                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                                    {
                                        this.uConfiguracoes.uce_pastas.textBox_PastaLote.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlEmLote, cnpj);
                                        this.uConfiguracoes.uce_pastas.textBox_PastaEnviados.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlEnviado, cnpj);
                                        this.uConfiguracoes.uce_pastas.textBox_PastaBackup.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaBackup, cnpj);
                                        this.uConfiguracoes.uce_pastas.textBox_PastaDownload.Text = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaDownloadNFeDest, cnpj);
                                    }
                                    this.uConfiguracoes.uce_danfe.tbConfiguracaoDanfe.Text = empresa.ConfiguracaoDanfe;
                                    this.uConfiguracoes.uce_danfe.tbConfiguracaoCCe.Text = empresa.ConfiguracaoCCe;
                                    this.uConfiguracoes.uce_danfe.tbPastaConfigUniDanfe.Text = empresa.PastaConfigUniDanfe;
                                    this.uConfiguracoes.uce_danfe.tbPastaExeUniDanfe.Text = empresa.PastaExeUniDanfe;
                                    this.uConfiguracoes.uce_danfe.tbPastaXmlParaDanfeMon.Text = empresa.PastaDanfeMon;
                                    this.uConfiguracoes.uce_danfe.cbDanfeMonNfe.Checked = empresa.XMLDanfeMonNFe;
                                    this.uConfiguracoes.uce_danfe.cbDanfeMonProcNfe.Checked = empresa.XMLDanfeMonProcNFe;

                                    this.checkBoxRetornoNFETxt.Checked = empresa.GravarRetornoTXTNFe;
                                    this.checkBoxGravarEventosNaPastaEnviadosNFe.Checked = empresa.GravarEventosNaPastaEnviadosNFe;
                                    this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked = empresa.GravarEventosCancelamentoNaPastaEnviadosNFe;
                                    this.checkBoxGravarEventosDeTerceiros.Checked = empresa.GravarEventosDeTerceiros;
                                    this.checkBoxCompactaNFe.Checked = empresa.CompactarNfe;
                                    this.cbIndSinc.Checked = empresa.IndSinc;

                                    this.uConfiguracoes.uce_pastas.cbCriaPastas.Checked = true;
                                    mudaPastas = false;
                                    break;
                                }
                            }
                            ///
                            /// se ainda assim nao foi encontrada nenhuma configuracao válida assume a pasta de instalacao do uninfe
                            /// 
                            if (string.IsNullOrEmpty(this.uConfiguracoes.uce_pastas.textBox_PastaEnvioXML.Text))
                            {
                                this.uConfiguracoes.uce_pastas.cbCriaPastas.Checked = true;
                                mudaPastas = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.uConfiguracoes.uce_pastas.textBox_PastaEnvioXML.Text) && mudaPastas)
                    {
                        mudaPastas = Dialogs.YesNo("CNPJ foi alterado e você já tem as pastas definidas.\r\nDeseja mudá-las para o novo CNPJ?", 0, 0);
                    }

                    if (mudaPastas)
                        MudarPastas(cnpj, servico);
                }
                this.uConfiguracoes.uce_pastas.empresa.CNPJ = cnpj;
                cnpjCurrent = cnpj;
            }
#endif
            return true;
        }

        private void edtCNPJ_Leave(object sender, EventArgs e)
        {
            this.ValidadeCNPJ();
        }

        private void edtCNPJ_Enter(object sender, EventArgs e)
        {
#if false
            this.edtCNPJ.Text = (string)Functions.OnlyNumbers(this.edtCNPJ.Text, ".-/");
#endif
        }

        private void comboBox_UF_DropDownClosed(object sender, EventArgs e)
        {
            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                comboBox_UF.DropDownWidth = comboBox_UF.Width;
        }

        private void comboBox_UF_DropDown(object sender, EventArgs e)
        {
            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                comboBox_UF.DropDownWidth = 300;
        }

#if false
        /// <summary>
        /// CopiaPastaDaEmpresa
        /// </summary>
        /// <param name="origemCNPJ"></param>
        /// <param name="origemPasta"></param>
        /// <param name="oEmpresa"></param>
        /// <returns></returns>
        private string CopiaPastaDeEmpresa(string origemCNPJ, string origemPasta, string destinoCNPJ)
        {
            if (string.IsNullOrEmpty(origemPasta))
                return "";

            ///
            ///o usuario pode ter colocado o CNPJ como parte do nome da pasta
            ///
            string newPasta = origemPasta.Replace(origemCNPJ.Trim(), destinoCNPJ.Trim());

            if (origemPasta.ToLower() == newPasta.ToLower())
            {
                int lastBackSlash = ConfiguracaoApp.RemoveEndSlash(origemPasta).LastIndexOf("\\");
                newPasta = origemPasta.Insert(lastBackSlash, "\\" + destinoCNPJ);
            }
            return newPasta;
        }

        private void MudarPastas(string cnpj, TipoAplicativo servico)
        {
            if (this.changeEvent != null)
                this.changeEvent(null, null);

            string subpasta = (Propriedade.TipoAplicativo == TipoAplicativo.Nfse ? "\\" + servico.ToString().ToLower() : "");

            this.uConfiguracoes.uce_pastas.textBox_PastaEnvioXML.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\Envio");
            this.uConfiguracoes.uce_pastas.textBox_PastaRetornoXML.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\Retorno");
            this.uConfiguracoes.uce_pastas.textBox_PastaXmlErro.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\Erro");
            this.uConfiguracoes.uce_pastas.textBox_PastaValidar.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\Validar");

            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
            {
                this.uConfiguracoes.uce_pastas.textBox_PastaLote.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\EnvioEmLote");
                this.uConfiguracoes.uce_pastas.textBox_PastaEnviados.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\Enviado");

                if (!string.IsNullOrEmpty(this.uConfiguracoes.uce_pastas.textBox_PastaDownload.Text))
                    this.uConfiguracoes.uce_pastas.textBox_PastaDownload.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\DownloadNFe");

                if (!string.IsNullOrEmpty(this.uConfiguracoes.uce_pastas.textBox_PastaBackup.Text))
                    this.uConfiguracoes.uce_pastas.textBox_PastaBackup.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + subpasta + "\\Backup");
            }
        }
#endif
    }
}

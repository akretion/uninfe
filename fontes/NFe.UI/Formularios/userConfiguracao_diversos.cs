using NFe.Components;

#if _fw46
using NFe.Components.SOFTPLAN;
#endif

using NFe.Settings;
using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
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

            loading = true;

            if (!DesignMode)
            {
                cbServico.SelectedIndexChanged -= cbServico_SelectedIndexChanged;
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

                cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo(false);
                cbServico.DisplayMember = "Value";
                cbServico.ValueMember = "Key";
                #endregion Montar array DropList dos tipos de serviços

                #region Montar Array DropList do Tipo de Emissão da NF-e

                comboBox_tpEmis.DataSource = EnumHelper.ToList(typeof(TipoEmissao), true, true);
                comboBox_tpEmis.DisplayMember = "Value";
                comboBox_tpEmis.ValueMember = "Key";
                #endregion Montar Array DropList do Tipo de Emissão da NF-e

                cbServico.SelectedIndexChanged += cbServico_SelectedIndexChanged;
            }
        }

        public void Populate(Empresa empresa, bool novaempresa)
        {
            loading = true;
            try
            {
                uninfeDummy.ClearControls(this, true, false);

                this.empresa = empresa;

                Configurar(empresa, novaempresa);

                if (empresa.Servico == TipoAplicativo.Nfse)
                {
                    comboBox_UF.DataSource = arrMunicipios;
                }
                else
                {
                    comboBox_UF.DataSource = arrUF;
                }

                comboBox_UF.DisplayMember = NFeStrConstants.Nome;
                comboBox_UF.ValueMember = "Codigo";

                cnpjCurrent = edtCNPJ.Text = empresa.CNPJ;
                edtNome.Text = empresa.Nome;

                if (!string.IsNullOrEmpty(empresa.Documento))
                {
                    if (empresa.Documento.Equals("CPF"))
                    {
                        edtCNPJ.Text = ((CPF)edtCNPJ.Text).ToString();
                    }
                    else if (empresa.Documento.Equals("CEI"))
                    {
                        edtCNPJ.Text = ((CEI)edtCNPJ.Text).ToString();
                    }
                    else if (empresa.Documento.Equals("CAEPF"))
                    {
                        edtCNPJ.Text = Convert.ToInt64(edtCNPJ.Text).ToString(@"000\.000\.000\/000\-00");
                    }
                    else
                    {
                        edtCNPJ.Text = ((CNPJ)edtCNPJ.Text).ToString();
                    }
                }
                else
                {
                    if (empresa?.CNPJ?.Length == 11)
                    {
                        edtCNPJ.Text = ((CPF)edtCNPJ.Text).ToString();
                    }
                    else if (empresa?.CNPJ?.Length == 12)
                    {
                        edtCNPJ.Text = ((CEI)edtCNPJ.Text).ToString();
                    }
                    else
                    {
                        edtCNPJ.Text = ((CNPJ)edtCNPJ.Text).ToString();
                    }
                }

                comboBox_tpEmis.SelectedValue = this.empresa.tpEmis;
                comboBox_Ambiente.SelectedValue = this.empresa.AmbienteCodigo;
                comboBox_UF.SelectedValue = this.empresa.UnidadeFederativaCodigo;
                cbServico.SelectedValue = (int)this.empresa.Servico;

                if (empresa.Servico == TipoAplicativo.Nfse && this.empresa.UnidadeFederativaCodigo == 0)
                {
                    comboBox_UF.SelectedIndex = 0;
                }

                checkBoxRetornoNFETxt.Checked = this.empresa.GravarRetornoTXTNFe;
                checkBoxGravarEventosDeTerceiros.Checked = this.empresa.GravarEventosDeTerceiros;
                checkBoxGravarEventosNaPastaEnviadosNFe.Checked = this.empresa.GravarEventosNaPastaEnviadosNFe;
                checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked = this.empresa.GravarEventosCancelamentoNaPastaEnviadosNFe;
                checkBoxArqNSU.Checked = this.empresa.ArqNSU;

                //// São Paulo não possui processo síncrono
                //if (this.empresa.UnidadeFederativaCodigo == 35)
                //{
                //    cbIndSinc.Checked =
                //    cbIndSinc.Enabled = false;
                //}
                //else
                //{
                cbIndSinc.Enabled = true;
                cbIndSinc.Checked = this.empresa.IndSinc;
                //}

                edtIdentificadorCSC.Text = this.empresa.IdentificadorCSC;
                edtTokenCSC.Text = this.empresa.TokenCSC;

                cboDiretorioSalvarComo.Text = this.empresa.DiretorioSalvarComo;
                udDiasLimpeza.Text = this.empresa.DiasLimpeza.ToString();
                udTempoConsulta.Text = this.empresa.TempoConsulta.ToString();

                txtSenhaWS.Text = this.empresa.SenhaWS;
                txtUsuarioWS.Text = this.empresa.UsuarioWS;

#if _fw46
                if (empresa.UnidadeFederativaCodigo.Equals(4205407))
                {
                    Empresa result = empresa.RecuperarConfiguracaoNFSeSoftplan(empresa.CNPJ);

                    txtClienteID.Text = result.ClientID;
                    txtClientSecret.Text = result.ClientSecret;
                    empresa.ClientID = result.ClientID;
                    empresa.ClientSecret = result.ClientSecret;
                    empresa.TokenNFse = result.TokenNFse;
                    empresa.TokenNFSeExpire = result.TokenNFSeExpire;
                }

                if (empresa.UnidadeFederativaCodigo.Equals(5107925))
                {
                    Empresa result = empresa.RecuperarConfiguracaoNFSeSoftplan(empresa.CNPJ);

                    txtClienteID.Text = result.ClientID;
                    txtClientSecret.Text = result.ClientSecret;
                    empresa.ClientID = result.ClientID;
                    empresa.ClientSecret = result.ClientSecret;
                    empresa.TokenNFse = result.TokenNFse;
                    empresa.TokenNFSeExpire = result.TokenNFSeExpire;
                }

#endif

                HabilitaUsuarioSenhaWS(this.empresa.UnidadeFederativaCodigo);
                servicoCurrent = this.empresa.Servico;

                HabilitaOpcaoCompactar(this.empresa.Servico == TipoAplicativo.Nfe);

                edtCNPJ.ReadOnly = !string.IsNullOrEmpty(empresa.CNPJ);
                cbServico.Enabled = !edtCNPJ.ReadOnly;

                if (this.empresa.Servico != TipoAplicativo.Nfse && !novaempresa)
                {
                    cbServico.Enabled = true;
                }

                if (this.empresa.Servico.Equals(TipoAplicativo.Nfe) ||
                    this.empresa.Servico.Equals(TipoAplicativo.NFCe) ||
                    this.empresa.Servico.Equals(TipoAplicativo.MDFe) ||
                    this.empresa.Servico.Equals(TipoAplicativo.Cte) ||
                    this.empresa.Servico.Equals(TipoAplicativo.Todos))
                {
                    checkBoxValidarDigestValue.Checked = this.empresa.CompararDigestValueDFeRetornadoSEFAZ;
                }
            }
            finally
            {
                loading = false;
                cbServico_SelectedIndexChanged(null, null);
                comboBox_UF_SelectedIndexChanged(null, null);
            }
        }

        public bool Validar(bool exibeerro, bool novaempresa)
        {
            string cnpj = (string)Functions.OnlyNumbers(edtCNPJ.Text, ".-/");

            if (Convert.ToInt32("0" + udTempoConsulta.Text) < 2 || Convert.ToInt32("0" + udTempoConsulta.Text) > 15)
            {
                throw new Exception(lbl_udTempoConsulta.Text + " inválido");
            }

            if (comboBox_UF.SelectedValue == null)
            {
                throw new Exception(labelUF.Text + " deve ser informado");
            }

            ValidadeCNPJ(true);

            if (string.IsNullOrEmpty(edtNome.Text))
            {
                throw new Exception("Nome da empresa deve ser informado");
            }

            if (servicoCurrent != (TipoAplicativo)cbServico.SelectedValue && !novaempresa && exibeerro)
            {
                if ((TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Nfse)
                {
                    throw new Exception("Não pode mudar para esse tipo de serviço (NFSe)");
                }

                if ((TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.SAT)
                {
                    throw new Exception("Não pode mudar para esse tipo de serviço (SAT)");
                }

                Empresa e = Empresas.FindConfEmpresa(cnpj, (TipoAplicativo)cbServico.SelectedValue);
                if (e != null)
                {
                    throw new Exception("A empresa '" + e.Nome + "' já está monitorando esse tipo de serviço");
                }

                if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Confirma a alteração do tipo de serviço?", "",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return false;
                }
            }

            switch ((TipoAplicativo)cbServico.SelectedValue)
            {
                case TipoAplicativo.NFCe:
                    if (!string.IsNullOrEmpty(edtIdentificadorCSC.Text) && string.IsNullOrEmpty(edtTokenCSC.Text))
                    {
                        throw new Exception("É obrigatório informar o IDToken quando informado o CSC.");
                    }
                    else if (string.IsNullOrEmpty(edtIdentificadorCSC.Text) && !string.IsNullOrEmpty(edtTokenCSC.Text))
                    {
                        throw new Exception("É obrigatório informar o CSC quando informado o IDToken.");
                    }

                    break;
            }

            empresa.AmbienteCodigo = (int)comboBox_Ambiente.SelectedValue;
            empresa.CNPJ = cnpj;
            empresa.ArqNSU = checkBoxArqNSU.Checked;
            empresa.DiasLimpeza = Math.Abs(Convert.ToInt32("0" + udDiasLimpeza.Text));
            empresa.DiretorioSalvarComo = cboDiretorioSalvarComo.Text;
            empresa.GravarRetornoTXTNFe = checkBoxRetornoNFETxt.Checked;
            empresa.GravarEventosDeTerceiros = checkBoxGravarEventosDeTerceiros.Checked;
            empresa.GravarEventosNaPastaEnviadosNFe = checkBoxGravarEventosNaPastaEnviadosNFe.Checked;
            empresa.GravarEventosCancelamentoNaPastaEnviadosNFe = checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked;
            empresa.Nome = edtNome.Text;
            empresa.IndSinc = cbIndSinc.Checked;
            empresa.TempoConsulta = Math.Abs(Convert.ToInt32("0" + udTempoConsulta.Text));
            empresa.tpEmis = (int)comboBox_tpEmis.SelectedValue;
            empresa.UnidadeFederativaCodigo = (int)comboBox_UF.SelectedValue;
            empresa.Servico = (TipoAplicativo)cbServico.SelectedValue;
            empresa.SenhaWS = txtSenhaWS.Text;
            empresa.UsuarioWS = txtUsuarioWS.Text;
            empresa.IdentificadorCSC = edtIdentificadorCSC.Text;
            empresa.TokenCSC = edtTokenCSC.Text;
            empresa.CompararDigestValueDFeRetornadoSEFAZ = checkBoxValidarDigestValue.Checked;

            //Configurações para o município de Florianópolis-SC
#if _fw46
            if (edtCodMun.Text.Equals("4205407"))
            {
                if (string.IsNullOrEmpty(txtUsuarioWS.Text) ||
                string.IsNullOrEmpty(txtSenhaWS.Text) ||
                string.IsNullOrEmpty(txtClienteID.Text) ||
                string.IsNullOrEmpty(txtClientSecret.Text))
                {
                    throw new Exception("As seguintes informações tem que estarem todas informadas: Usuário, Senha, ClientID e ClientSecret");
                }

                IWebProxy proxy = null;

                if (ConfiguracaoApp.Proxy)
                {
                    if (ConfiguracaoApp.Proxy)
                    {
                        proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyPorta,
                            ConfiguracaoApp.DetectarConfiguracaoProxyAuto);
                    }
                }

                string url = "";

                if ((TipoAmbiente)comboBox_Ambiente.SelectedValue == TipoAmbiente.taHomologacao)
                {
                    url = @"https://nfps-e-hml.pmf.sc.gov.br/api/v1/";
                }
                else
                {
                    url = @"https://nfps-e.pmf.sc.gov.br/api/v1/";
                }

                Token token = Token.GerarToken(proxy,
                                             txtUsuarioWS.Text,
                                             txtSenhaWS.Text,
                                             txtClienteID.Text,
                                             txtClientSecret.Text,
                                             url);

                DateTime tokenNFSeExpire = DateTime.Now.AddSeconds(token.ExpiresIn);

                empresa.SalvarConfiguracoesNFSeSoftplan(txtUsuarioWS.Text,
                                                        txtSenhaWS.Text,
                                                        txtClienteID.Text,
                                                        txtClientSecret.Text,
                                                        token.AccessToken,
                                                        tokenNFSeExpire,
                                                        edtCNPJ.Text);
            }
            if (edtCodMun.Text.Equals("5107925"))
            {
                if (string.IsNullOrEmpty(txtUsuarioWS.Text) ||
                string.IsNullOrEmpty(txtSenhaWS.Text) ||
                string.IsNullOrEmpty(txtClienteID.Text) ||
                string.IsNullOrEmpty(txtClientSecret.Text))
                {
                    throw new Exception("As seguintes informações tem que estarem todas informadas: Usuário, Senha, ClientID e ClientSecret");
                }

                IWebProxy proxy = null;

                if (ConfiguracaoApp.Proxy)
                {
                    if (ConfiguracaoApp.Proxy)
                    {
                        proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor,
                            ConfiguracaoApp.ProxyUsuario,
                            ConfiguracaoApp.ProxySenha,
                            ConfiguracaoApp.ProxyPorta,
                            ConfiguracaoApp.DetectarConfiguracaoProxyAuto);
                    }
                }

                string url = "";

                url = @"http://agiliblue.agilicloud.com.br/api/";

                Token token = Token.GerarToken(proxy,
                                             txtUsuarioWS.Text,
                                             txtSenhaWS.Text,
                                             txtClienteID.Text,
                                             txtClientSecret.Text,
                                             url);

                DateTime tokenNFSeExpire = DateTime.Now.AddSeconds(token.ExpiresIn);

                empresa.SalvarConfiguracoesNFSeSoftplan(txtUsuarioWS.Text,
                                                        txtSenhaWS.Text,
                                                        txtClienteID.Text,
                                                        txtClientSecret.Text,
                                                        token.AccessToken,
                                                        tokenNFSeExpire,
                                                        edtCNPJ.Text);
            }
#endif

            return true;
        }

        private void HabilitaOpcaoCompactar(bool ativar)
        {
            if (empresa.Servico == TipoAplicativo.Nfse)
            {
                ativar = false;
            }
        }

        private void udDiasLimpeza_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar);
        }

        private void cbServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                return;
            }

            grpQRCode.Visible = (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.NFCe ||
                                     (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Nfe ||
                                     (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Todos;

            checkBoxValidarDigestValue.Visible = (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.NFCe ||
                                     (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Nfe ||
                                     (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Cte ||
                                     (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.MDFe ||
                                     (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Todos;

            if ((TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Nfe ||
                (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.NFCe ||
                (TipoAplicativo)cbServico.SelectedValue == TipoAplicativo.Todos)
            {
                HabilitaOpcaoCompactar(true);
            }
            else
            {
                HabilitaOpcaoCompactar(false);
            }
            if (changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void HabilitaUsuarioSenhaWS(int ufCod)
        {
            bool visible = ufCod == 3502804 /*Araçatuba*/||
                           ufCod == 4104303 /*Campo Mourão*/||
                           ufCod == 4104204 /*Campo Largo*/||
                           ufCod == 3537305 /*Penapolis*/||
                           ufCod == 4309209 /*Gravataí*/||
                           ufCod == 3201308 /*Cariacica*/||
                           ufCod == 3538709 /*Piracicaba*/||
                           ufCod == 3553807 /*Taquarituba*/||
                           ufCod == 3512902 /*Cosmorama*/||
                           ufCod == 3515004 /*Embu das Artes*/||
                           ufCod == 3148004 /*Patos de Minas*/||
                           ufCod == 3506508 /*Birigui*/||
                           ufCod == 3201506 /*Colatina*/||
                           ufCod == 4118204 /*Paranaguá*/||
                           ufCod == 3130309 /*Iguatama*/||
                           ufCod == 3541406 /*Presidente Prudente-SP*/||
                           ufCod == 4217808 /*Taió-SC*/||
                           ufCod == 4101101 /*Andirá-PR*/||
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
                           ufCod == 4211900 /*Palhoça-SC*/ ||
                           ufCod == 4317202 /*Santa Rosa-RS*/ ||
                           ufCod == 4202909 /*Brusque-SC*/ ||
                           ufCod == 3535507 /*Paraguaçu Paulista-SP*/ ||
                           ufCod == 1503606 /*Itaituba-PA*/ ||
                           ufCod == 3200904 /*Barra de São Francisco-ES*/ ||
                           ufCod == 2901007 /*Amargosa-BA*/ ||
                           ufCod == 3152105 /*Ponte Nova-MG*/ ||
                           ufCod == 3536703 /*Pederneiras-SP*/ ||
                           ufCod == 3120904 /*Curvelo-MG*/ ||
                           ufCod == 3162708 /*São João do Paraíso-MG*/ ||
                           ufCod == 3168002 /*Taiobeiras-MG*/ ||
                           ufCod == 3530607 /*Mogi das Cruzes-SP*/ ||
                           ufCod == 3515509 /*Fernandópolis-SP*/ ||
                           ufCod == 3527108 /*Lins-SP*/ ||
                           ufCod == 3514403 /*Dracena-SP*/ ||
                           ufCod == 3544004 /*Rio das Pedras-SP*/ ||
                           ufCod == 4302105 /*Bento Gonçalves-RS*/ ||
                           ufCod == 4207502 /*Indaial-SC*/ ||
                           ufCod == 4211801 /*Ouro-SC*/ ||
                           ufCod == 3500501 /*Aguas de Lindoia-SP*/ ||
                           ufCod == 3523107 /*Itaquaquecetuba-SP*/ ||
                           ufCod == 3143104 /*Monte Carnmelo-MG*/||
                           ufCod == 2931350 /*Teixeira de Freitas-BA*/||
                           ufCod == 3205101 /*Viana-ES*/ ||
                           ufCod == 3202405 /*Guarapari-ES*/ ||
                           ufCod == 3157005 /*Salinas-MG*/ ||
                           ufCod == 3141108 /*Matozinhos-MG*/||
                           ufCod == 4119152 /*Pinhais-PR*/||
                           ufCod == 4127205 /*Terra Boa-PR*/||
                           ufCod == 4313508 /*Osório-RS */ ||
                           ufCod == 4118006 /*Paraíso do Norte-PR*/ ||
                           ufCod == 4300604 /*Alvorada-RS*/ ||
                           ufCod == 4104907 /*Castro-PR*/ ||
                           ufCod == 3505302 /*Barra Bonita-SP/*/ ||
                           ufCod == 4202404 /*Blumenau-SC*/ ||
                           ufCod == 3520004 /*Igaraçu do Tietê-SP*/ ||
                           ufCod == 3539400 /*Piratininga-SP*/ ||
                           ufCod == 3516705 /*Garça-SP*/ ||
                           ufCod == 3514502 /*Duartina-SP*/ ||
                           ufCod == 3526902 /*Limeira-SP*/||
                           ufCod == 3146008 /*Ouro Fino-MG*/||
                           ufCod == 4323804 /*Xangri-la-RS*/||
                           ufCod == 5107925 /*Sorriso-MT*/ ||
                           ufCod == 3549102 /*São João da Boa Vista-SP*/ ||
                           ufCod == 3556404 /*Vargem Grande do Sul-SP*/ ||
                           ufCod == 4104808 /*Cascavel-PR*/ ||
                           ufCod == 4303103 /*Cachoeirinha-RS*/ ||
                           ufCod == 3204203 /*Piuma-ES*/ ||
                           ufCod == 1502152 /*Canaã dos Carajás-PA*/ ||
                           ufCod == 4114609 /*Marechal Cândido Rondon-PR*/ ||
                           ufCod == 5207402 /*Edéia-GO*/ ||
                           ufCod == 4115200 /*Maringá-PR*/ ||
                           ufCod == 1100122 /*Ji-Paraná-RO*/||
                           ufCod == 4213203 /*Pomerode-SC*/||
                           ufCod == 4213500 /*Porto Belo-SC*/||
                           ufCod == 4215000 /*Rio Negrinho-SC*/;



            lbl_UsuarioWS.Visible = txtUsuarioWS.Visible = lbl_SenhaWS.Visible = txtSenhaWS.Visible = visible;
        }

        private void comboBox_UF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                return;
            }

            // danasa 1-2012
            try
            {
                object xuf = comboBox_UF.SelectedValue;

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
                //Se o município for Sorriso, temos que demonstrar os campos: ClientID e Client Secret
               else if (edtCodMun.Text.Equals("5107925"))
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

#if _fw35
                if (edtCodMun.Text.Equals("2901007"))
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, @"Este município não funciona com a versão do UniNFe com .NET Framework 3.5. Dessa forma, instale a versão do UniNFe com .NET Framework 4.6.2 que consta no site da Unimake.", "Atenção");
#endif
            }
            catch
            {
                HabilitaUsuarioSenhaWS(-1);
                edtCodMun.Text = edtPadrao.Text = "Indefinido";
            }
            if (changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void comboBox_Ambiente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        public bool ValidadeCNPJ(bool istrow = false)
        {
            return true;
        }

        private void edtCNPJ_Leave(object sender, EventArgs e)
        {
            ValidadeCNPJ();
        }

        private void edtCNPJ_Enter(object sender, EventArgs e)
        {
        }

        private void comboBox_UF_DropDownClosed(object sender, EventArgs e)
        {
            if (empresa.Servico == TipoAplicativo.Nfse)
            {
                comboBox_UF.DropDownWidth = comboBox_UF.Width;
            }
        }

        private void comboBox_UF_DropDown(object sender, EventArgs e)
        {
            if (empresa.Servico == TipoAplicativo.Nfse)
            {
                comboBox_UF.DropDownWidth = 300;
            }
        }

        private void txtUsuarioWS_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void txtSenhaWS_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void txtClienteID_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void txtClientSecret_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
            {
                changeEvent(sender, e);
            }
        }

        private void Configurar(Empresa empresa, bool novaempresa)
        {
            switch (empresa.Servico)
            {
                case TipoAplicativo.Nfse:
                    labelUF.Visible = true;
                    labelUF.Text = "Município";
                    comboBox_UF.Visible = true;
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
                    udTempoConsulta.Visible = lbl_udTempoConsulta.Visible = false;
                    cbIndSinc.Visible = false;
                    comboBox_Ambiente.Visible = true;
                    checkBoxArqNSU.Visible = false;
                    checkBoxValidarDigestValue.Visible = false;
                    lbl_udDiasLimpeza.Location = new System.Drawing.Point(3, 247);
                    udDiasLimpeza.Location = new System.Drawing.Point(3, 266);
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
                    checkBoxValidarDigestValue.Visible = false;
                    lbl_udDiasLimpeza.Location = new System.Drawing.Point(3, 147);
                    udDiasLimpeza.Location = new System.Drawing.Point(3, 166);
                    break;

                case TipoAplicativo.EFDReinf:
                case TipoAplicativo.eSocial:
                case TipoAplicativo.EFDReinfeSocial:
                    comboBox_UF.Visible = false;
                    comboBox_tpEmis.Visible = false;
                    udTempoConsulta.Visible = false;
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
                    checkBoxValidarDigestValue.Visible = false;
                    lbl_udDiasLimpeza.Location = new System.Drawing.Point(3, 147);
                    udDiasLimpeza.Location = new System.Drawing.Point(3, 166);
                    break;

                case TipoAplicativo.GNRE:
                    labelUF.Visible = true;
                    labelUF.Text = "Unidade Federativa (UF)";
                    comboBox_UF.Visible = true;
                    cboDiretorioSalvarComo.Visible = true;
                    lbl_DiretorioSalvarComo.Visible = true;

                    comboBox_tpEmis.Visible = false;
                    udTempoConsulta.Visible = false;
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
                    checkBoxValidarDigestValue.Visible = false;
                    lbl_udDiasLimpeza.Location = new System.Drawing.Point(3, 147);
                    udDiasLimpeza.Location = new System.Drawing.Point(3, 166);
                    break;

                default:
                    labelUF.Visible = true;
                    labelUF.Text = "Unidade Federativa (UF)";
                    comboBox_UF.Visible = true;
                    cboDiretorioSalvarComo.Visible = true;
                    lbl_DiretorioSalvarComo.Visible = true;
                    comboBox_tpEmis.Visible = true;
                    metroLabel11.Visible = true;
                    checkBoxGravarEventosNaPastaEnviadosNFe.Visible = true;
                    checkBoxRetornoNFETxt.Visible = true;
                    checkBoxGravarEventosDeTerceiros.Visible = true;
                    checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible = true;
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
                    checkBoxValidarDigestValue.Visible = true;
                    lbl_udDiasLimpeza.Location = new System.Drawing.Point(3, 147);
                    udDiasLimpeza.Location = new System.Drawing.Point(3, 166);
                    break;
            }
        }
    }
}
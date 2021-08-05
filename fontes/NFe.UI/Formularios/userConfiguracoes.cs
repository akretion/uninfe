using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class userConfiguracoes : UserControl1
    {
        private bool first = true;

        public userConfiguracoes()
        {
            InitializeComponent();
        }

        #region local

        /// <summary>
        /// Controla se o evento changed_Modificado deve ser executado, se true, não executa o evento
        /// </summary>
        private bool stopChangedEvent = false;

        #endregion local

        private MetroFramework.Controls.MetroTabPage _tpEmpresa_divs,
            _tpEmpresa_danfe,
            _tpEmpresa_pastas,
            _tpEmpresa_cert,
            _tpEmpresa_ftp,
            _tpEmpresa_sat,
            _tpEmpresa_resptecnico,
            _tpEmpresa_outrasconfiguracoes;

        private string empcnpj = "";
        private TipoAplicativo servico;

        private Formularios.userConfiguracao_certificado uce_cert;
        public Formularios.userConfiguracao_danfe uce_danfe;
        private Formularios.userConfiguracao_diversos uce_divs;
        private Formularios.userConfiguracao_ftp uce_ftp;
        private Formularios.userConfiguracao_geral uc_geral = null;
        public Formularios.UserConfiguracaoPastas uce_pastas;
        private Formularios.userConfiguracao_sat uce_sat;
        private Formularios.userConfiguracao_resptecnico uce_resptecnico;
        private Formularios.userConfiguracao_outrasconfiguracoes uce_outrasconfiguracoes;
        private Empresa currentEmpresa;
        private TipoAplicativo servicoCurrent;

        private MetroFramework.Controls.MetroTabPage createtpage(int id)
        {
            #region --createtpage

            var tpage = new MetroFramework.Controls.MetroTabPage();
            tpage.Padding = new Padding(2);
            //tpage.Theme = uninfeDummy.mainForm.uTheme;
            //tpage.Style = uninfeDummy.mainForm.uStyle;

            switch (id)
            {
                case 0:
                    tpage.Text = "Principal";
                    //tpage.AutoScroll = (Propriedade.TipoAplicativo == TipoAplicativo.Nfe || Propriedade.TipoExecucao == TipoExecucao.teAll);
                    uce_divs = new Formularios.userConfiguracao_diversos();
                    uce_divs.uConfiguracoes = this;
                    uce_divs.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_divs);
                    break;

                case 1:
                    tpage.Text = "Pastas";
                    //tpage.AutoScroll = (Propriedade.TipoAplicativo == TipoAplicativo.Nfe || Propriedade.TipoExecucao == TipoExecucao.teAll);
                    uce_pastas = new Formularios.UserConfiguracaoPastas();
                    uce_pastas.ChangeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_pastas);
                    break;

                case 2:
                    tpage.Text = "Certificado digital";
                    uce_cert = new Formularios.userConfiguracao_certificado();
                    uce_cert.changeEvent += changed_Modificado;
                    uce_cert.ucPastas = uce_pastas;
                    tpage.Controls.Add(uce_cert);
                    break;

                case 3:
                    tpage.Text = "FTP";
                    uce_ftp = new Formularios.userConfiguracao_ftp();
                    uce_ftp.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_ftp);
                    break;

                case 4:
                    tpage.Text = "DANFE";
                    tpage.AutoScroll = true;
                    uce_danfe = new Formularios.userConfiguracao_danfe();
                    uce_danfe.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_danfe);
                    break;

                case 5:
                    tpage.Text = "SAT";
                    uce_sat = new Formularios.userConfiguracao_sat();
                    uce_sat.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_sat);
                    break;

                case 6:
                    tpage.Text = "Responsável Técnico";
                    uce_resptecnico = new Formularios.userConfiguracao_resptecnico();
                    uce_resptecnico.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_resptecnico);
                    break;

                case 7:
                    tpage.Text = "Outras Configurações";
                    uce_outrasconfiguracoes = new Formularios.userConfiguracao_outrasconfiguracoes();
                    uce_outrasconfiguracoes.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_outrasconfiguracoes);
                    break;
            }
            tpage.Controls[tpage.Controls.Count - 1].Dock = DockStyle.Fill;

            return tpage;

            #endregion --createtpage
        }

        public override void UpdateControles()
        {
            #region --UpdateControles

            base.UpdateControles();

            if (first)
            {
                tc_empresa.TabPages.Clear();

                CreateControles();

                back_button.Tag = 1;   //previne que seja pressionado pelo UserControl1

                back_button.Click += delegate
                {
                    if (VerificaSeAbandona())
                    {
                        BackFuncao();
                    }
                    ConfiguracaoApp.CarregarDados();
                };

                tc_main.Selected += (_sender, _e) =>
                {
                    if (_e.TabPage == tpGeral)
                        uc_geral.FocusFirstControl();
                };
                tc_empresa.Selected += (_sender, _e) =>
                {
                    if (_e.TabPage == _tpEmpresa_pastas) uce_pastas.FocusFirstControl();
                    if (_e.TabPage == _tpEmpresa_cert) uce_cert.FocusFirstControl();
                    if (_e.TabPage == _tpEmpresa_ftp) uce_ftp.FocusFirstControl();
                    if (_e.TabPage == _tpEmpresa_danfe) uce_danfe.FocusFirstControl();
                    if (_e.TabPage == _tpEmpresa_sat) uce_sat.FocusFirstControl();
                    if (_e.TabPage == _tpEmpresa_resptecnico) uce_resptecnico.FocusFirstControl();
                    if (_e.TabPage == _tpEmpresa_outrasconfiguracoes) uce_outrasconfiguracoes.FocusFirstControl();
                };

                tc_main.SelectedIndex = 1;
                tc_empresa.SelectedIndex = 0;

                if (cbEmpresas.Items.Count == 0)
                    btnNova_Click(null, null);
            }
            first = false;

            #endregion --UpdateControles
        }

        private void CreateControles()
        {
            #region --CreateControles

            cbEmpresas.Visible = metroLabel2.Visible = true;

            if (uc_geral == null)
            {
                tc_empresa.TabPages.Add(_tpEmpresa_divs = createtpage(0));
                tc_empresa.TabPages.Add(_tpEmpresa_pastas = createtpage(1));
                tc_empresa.TabPages.Add(_tpEmpresa_cert = createtpage(2));
                tc_empresa.TabPages.Add(_tpEmpresa_ftp = createtpage(3));
                tc_empresa.TabPages.Add(_tpEmpresa_danfe = createtpage(4));
                tc_empresa.TabPages.Add(_tpEmpresa_sat = createtpage(5));
                tc_empresa.TabPages.Add(_tpEmpresa_resptecnico = createtpage(6));
                tc_empresa.TabPages.Add(_tpEmpresa_outrasconfiguracoes = createtpage(7));
                uc_geral = new Formularios.userConfiguracao_geral();
                tpGeral.Controls.Add(uc_geral);
            }

            tc_main.SelectedTab = tpGeral;
            tc_empresa.SelectedTab = _tpEmpresa_divs;

            cbEmpresas.SelectedIndexChanged -= cbEmpresas_SelectedIndexChanged;
            cbEmpresas.DataSource = null;
            cbEmpresas.DisplayMember = NFeStrConstants.Nome;
            cbEmpresas.ValueMember = "Key";
            cbEmpresas.DataSource = Auxiliar.CarregaEmpresa(false);
            btnExcluir.Visible = cbEmpresas.Items.Count > 0;

            ConfiguracaoApp.CarregarDados();
            uc_geral.PopulateConfGeral();

            userConfiguracoes_Resize(null, null);

            if (cbEmpresas.Items.Count > 0)
            {
                tc_empresa.Enabled =
                    btnExcluir.Enabled =
                    cbEmpresas.Enabled = true;
                cbEmpresas.SelectedIndex = 0;
                cbEmpresas.SelectedIndexChanged += cbEmpresas_SelectedIndexChanged;
                cbEmpresas_SelectedIndexChanged(null, null);
            }
            else
                CopiaDadosDaEmpresaParaControls(new Empresa(), true);

            #endregion --CreateControles
        }

        private void userConfiguracoes_Resize(object sender, EventArgs e)
        {
            if (uc_geral != null)
            {
                ///
                /// centraliza o usercontrol das configuracoes gerais
                int w = uc_geral.Size.Width;
                //int h = uc_geral.Size.Height;
                int left = (Size.Width - w) / 2;
                uc_geral.Location = new Point(left, 10);
            }
        }

        /// <summary>
        /// CopiaPastaDaEmpresa
        /// </summary>
        /// <param name="origemCNPJ"></param>
        /// <param name="origemPasta"></param>
        /// <param name="oEmpresa"></param>
        /// <returns></returns>
        private string CopiaPastaDeEmpresa(string origemCNPJ, string origemPasta, Empresa destino)
        {
            if (string.IsNullOrEmpty(origemPasta))
                return "";

            ///
            /// o usuario pode ter colocado o CNPJ como parte do nome da pasta
            ///
            /// origemPasta: C:\uninfe\CNPJ_antigo\envio
            /// origemCNPJ:  CNPJ_novo
            /// newPasta:    C:\uninfe\CNPJ_novo\envio
            ///
            string newPasta = origemPasta.Replace(origemCNPJ.Trim(), destino.CNPJ.Trim());

            if (origemPasta.Equals(newPasta, StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (string sp in new string[] { "\\nfse\\", "\\nfce\\", "\\cte\\", "\\mdfe\\", "\\" })
                    origemPasta = origemPasta.Replace(destino.CNPJ + sp, "");

                int lastBackSlash = ConfiguracaoApp.RemoveEndSlash(origemPasta).LastIndexOf("\\");

                string subpasta = destino.CNPJ;
                switch (destino.Servico)
                {
                    case TipoAplicativo.Todos:
                    case TipoAplicativo.Nfe:
                        break;

                    default:
                        subpasta += "\\" + destino.Servico.ToString().ToLower();
                        break;
                }
                newPasta = origemPasta.Insert(lastBackSlash, "\\" + subpasta);
            }
            return newPasta;
        }

        /// <summary>
        /// CopiaPastaDeEmpresa
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="oempresa"></param>
        private void CopiaPastaDeEmpresa(Empresa empresa, Empresa oempresa)
        {
            oempresa.PastaXmlEnvio = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlEnvio, oempresa);
            oempresa.PastaXmlRetorno = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlRetorno, oempresa);
            oempresa.PastaXmlErro = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlErro, oempresa);
            oempresa.PastaValidar = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaValidar, oempresa);
            if (oempresa.Servico != TipoAplicativo.Nfse)
            {
                if (empresa.Servico != TipoAplicativo.Nfse)
                {
                    oempresa.PastaXmlEmLote = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlEmLote, oempresa);
                    oempresa.PastaXmlEnviado = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaXmlEnviado, oempresa);
                    oempresa.PastaBackup = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaBackup, oempresa);
                    oempresa.PastaDownloadNFeDest = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaDownloadNFeDest, oempresa);
                }
                else
                {
                    string folder = Path.GetDirectoryName(ConfiguracaoApp.RemoveEndSlash(oempresa.PastaXmlEnvio));

                    oempresa.PastaXmlEnviado = Path.Combine(folder, "Enviado");
                    oempresa.PastaXmlEmLote = Path.Combine(folder, "EnvioEmLote");
                    oempresa.PastaDownloadNFeDest = Path.Combine(folder, "DownloadNFe");
                    oempresa.PastaBackup = Path.Combine(folder, "Backup");
                }
            }
            oempresa.ConfiguracaoDanfe = empresa.ConfiguracaoDanfe;
            oempresa.ConfiguracaoCCe = empresa.ConfiguracaoCCe;
            oempresa.PastaConfigUniDanfe = empresa.PastaConfigUniDanfe;
            oempresa.PastaExeUniDanfe = empresa.PastaExeUniDanfe;
            oempresa.PastaDanfeMon = empresa.PastaDanfeMon;
            oempresa.XMLDanfeMonNFe = empresa.XMLDanfeMonNFe;
            oempresa.XMLDanfeMonProcNFe = empresa.XMLDanfeMonProcNFe;

            oempresa.GravarRetornoTXTNFe = empresa.GravarRetornoTXTNFe;
            oempresa.GravarEventosNaPastaEnviadosNFe = empresa.GravarEventosNaPastaEnviadosNFe;
            oempresa.GravarEventosCancelamentoNaPastaEnviadosNFe = empresa.GravarEventosCancelamentoNaPastaEnviadosNFe;
            oempresa.GravarEventosDeTerceiros = empresa.GravarEventosDeTerceiros;
            oempresa.IndSinc = empresa.IndSinc;
            oempresa.IndSincNFCe = empresa.IndSincNFCe;

            oempresa.CodigoAtivacaoSAT = empresa.CodigoAtivacaoSAT;
            oempresa.MarcaSAT = empresa.MarcaSAT;
            oempresa.UtilizaConversaoCFe = empresa.UtilizaConversaoCFe;
            oempresa.CNPJSoftwareHouse = empresa.CNPJSoftwareHouse;
            oempresa.SignACSAT = empresa.SignACSAT;
            oempresa.NumeroCaixa = empresa.NumeroCaixa;
            oempresa.IndRatISSQNSAT = empresa.IndRatISSQNSAT;
            oempresa.RegTribISSQNSAT = empresa.RegTribISSQNSAT;

            oempresa.CriaPastasAutomaticamente = true;
        }

        private void CopiaDadosDaEmpresaParaControls(Empresa oempresa, bool empty)
        {
            bool _modificado = false;
            bool _nova = string.IsNullOrEmpty(oempresa.PastaXmlEnvio);

            stopChangedEvent = true;
            try
            {
                servicoCurrent = oempresa.Servico;
                oempresa.CriaPastasAutomaticamente = false;

                if (string.IsNullOrEmpty(oempresa.PastaXmlEnvio) && !empty)
                {
                    oempresa.CriaPastasAutomaticamente = true;
                    _modificado = true;

                    ///
                    /// tenta definir as pastas na mesma arvore do CNPJ
                    ///
                    foreach (Empresa rr in (from x in Empresas.Configuracoes
                                            where x.CNPJ.Equals(oempresa.CNPJ)
                                            select x))
                    {
                        string temp = CopiaPastaDeEmpresa(rr.CNPJ, rr.PastaXmlEnvio, oempresa);
                        if (!Directory.Exists(temp))
                        {
                            CopiaPastaDeEmpresa(rr, oempresa);
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(oempresa.PastaXmlEnvio))
                    {
                        ///
                        /// acha uma configuracao valida
                        ///
                        foreach (Empresa rr in (from x in Empresas.Configuracoes
                                                where !x.CNPJ.Equals(oempresa.CNPJ)
                                                select x))
                        {
                            string temp = CopiaPastaDeEmpresa(rr.CNPJ, rr.PastaXmlEnvio, oempresa);

                            if (!Directory.Exists(temp))
                            {
                                CopiaPastaDeEmpresa(rr, oempresa);
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(oempresa.PastaXmlEnvio))
                    {
                        ///
                        /// se mesmo assim não encontrou nenhuma configuracao valida, assume a pasta do UniNFe
                        ///
                        string subpasta = oempresa.CNPJ;
                        switch (oempresa.Servico)
                        {
                            case TipoAplicativo.Todos:
                            case TipoAplicativo.Nfe:
                                break;

                            default:
                                subpasta += "\\" + oempresa.Servico.ToString().ToLower();
                                break;
                        }
                        oempresa.PastaXmlEnvio = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Envio");
                        oempresa.PastaXmlRetorno = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Retorno");
                        oempresa.PastaXmlErro = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Erro");
                        oempresa.PastaValidar = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Validar");
                        if (oempresa.Servico != TipoAplicativo.Nfse)
                        {
                            oempresa.PastaXmlEnviado = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Enviado");
                            oempresa.PastaXmlEmLote = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\EnvioEmLote");
                            oempresa.PastaDownloadNFeDest = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\DownloadNFe");
                        }
                    }
                }

                ConfigurarAbas(oempresa, _nova);
            }
            finally
            {
                tc_empresa.Enabled = !empty;

                stopChangedEvent = false;
                Modificado = _modificado;
            }
        }

        private bool _Modificado;

        private bool Modificado
        {
            get
            {
                return _Modificado;
            }
            set
            {
                if (value)
                {
                    btnNova.Text = "Salvar";
                    btnExcluir.Text = "Cancelar";
                }
                else
                {
                    btnNova.Text = "Nova";
                    btnExcluir.Text = "Excluir";
                }
                cbEmpresas.Enabled = !value;
                btnExcluir.Enabled = true;
                _Modificado = value;
            }
        }

        private void changed_Modificado(object sender, EventArgs e)
        {
            if (!stopChangedEvent) Modificado = true;
        }

        private void cbEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region

            currentEmpresa = null;

            var list = (cbEmpresas.DataSource as System.Collections.ArrayList)[cbEmpresas.SelectedIndex] as NFe.Components.ComboElem;
            var empresa = Empresas.FindConfEmpresa(list.Valor, NFe.Components.EnumHelper.StringToEnum<TipoAplicativo>(list.Servico));
            if (empresa == null)
            {
                CopiaDadosDaEmpresaParaControls(new Empresa(), true);
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Não pode acessar os dados da empresa selecionada", "");
            }
            else
            {
                currentEmpresa = new Empresa();
                empresa.CopyObjectTo(currentEmpresa);
                empcnpj = currentEmpresa.CNPJ;
                servico = currentEmpresa.Servico;

                CopiaDadosDaEmpresaParaControls(currentEmpresa, false);

                btnNova.Tag = 0;
            }
            #endregion
        }

        private const string constAbandono = "Dados da configuração foram alterados, abandona mesmo assim?";

        public bool VerificaSeAbandona()
        {
            #region

            if (tc_empresa.Enabled)
            {
                if (DadosMudaramDaEmpresa(false) || uc_geral.Modificado || !EmpresaValidada)
                    return MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                        constAbandono, "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            }
            BackFuncao();
            return true;

            #endregion
        }

        private bool EmpresaValidada = false;

        private bool DadosMudaramDaEmpresa(bool exibeerro)
        {
            #region --DadosMudaramDaEmpresa

            EmpresaValidada = true;

            if (currentEmpresa == null || !Modificado)
                return false;

            try
            {
                uc_geral.Validar();
                if (!uce_divs.Validar(exibeerro, Convert.ToInt16(btnNova.Tag) == 1))
                {
                    EmpresaValidada = false;
                    return false;
                }

                uce_pastas.Validar();

                switch (currentEmpresa.Servico)
                {
                    case TipoAplicativo.SAT:
                        uce_ftp.Validar();
                        uce_danfe.Validar();
                        uce_sat.Validar();
                        break;
                    case TipoAplicativo.Nfse:
                    case TipoAplicativo.EFDReinf:
                    case TipoAplicativo.eSocial:
                    case TipoAplicativo.EFDReinfeSocial:
                        uce_cert.Validar();
                        break;

                    case TipoAplicativo.GNRE:
                        goto default;

                    case TipoAplicativo.Nfe:
                    case TipoAplicativo.NFCe:
                    case TipoAplicativo.Cte:
                    case TipoAplicativo.MDFe:
                    case TipoAplicativo.Todos:
                        uce_cert.Validar();
                        uce_ftp.Validar();
                        uce_danfe.Validar();
                        uce_resptecnico.Validar();
                        uce_outrasconfiguracoes.Validar();
                        break;

                    default:
                        uce_cert.Validar();
                        uce_ftp.Validar();
                        uce_danfe.Validar();
                        break;
                }
            }
            catch (Exception ex)
            {
                if (exibeerro)
                    throw ex;
            }

            if (Convert.ToInt16(btnNova.Tag) == 1) //se inclusao nao precisa verificar se mudou algo, já que nao tem classe existente
                return Modificado;

            var _Empresa = Empresas.FindConfEmpresa(empcnpj, servico);
            if (_Empresa == null)
                return Modificado;

            PropertyInfo[] allClassToProperties = _Empresa.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var pi in allClassToProperties)
            {
                if (pi.CanWrite)
                {
                    var pii = currentEmpresa.GetType().GetProperty(pi.Name, BindingFlags.Instance | BindingFlags.Public);
                    if (pii != null)
                    {
                        object ob1 = pi.GetValue(_Empresa, null);
                        object ob2 = pii.GetValue(currentEmpresa, null);
                        if (ob1 == null || ob2 == null)
                            continue;

                        if (!ob1.Equals(ob2))
                        {
                            Console.WriteLine("{0}: old:{1} x new:{2}", pi.Name, ob1.ToString(), ob2.ToString());
                            return true;
                        }
                    }
                }
            }
            return false;

            #endregion
        }

        private void btnNova_Click(object sender, EventArgs e)
        {
            if (btnNova.Text.Equals("Nova"))
            {
                bool ok = false;
                using (Formularios.FormNova f = new Formularios.FormNova(FindForm()))
                {
                    ok = f.ShowDialog() == DialogResult.OK;

                    if (ok)
                    {
                        currentEmpresa = new Empresa();
                        currentEmpresa.CNPJ = Functions.OnlyNumbers(f.edtCNPJ.Text, ".,-/").ToString();
                        currentEmpresa.Nome = f.edtNome.Text;
                        currentEmpresa.Servico = (TipoAplicativo)f.cbServico.SelectedValue;
                        currentEmpresa.Documento = f.comboDocumento.SelectedItem.ToString();

                        if (currentEmpresa.Servico == TipoAplicativo.Nfse)
                            currentEmpresa.UnidadeFederativaCodigo = 0;
                        else
                            currentEmpresa.UnidadeFederativaCodigo = 41;
                    }
                }
                if (ok)
                {
                    cbEmpresas.Visible = metroLabel2.Visible = false;
                    CopiaDadosDaEmpresaParaControls(currentEmpresa, false);

                    tc_main.SelectedIndex = 1;
                    tc_empresa.SelectedIndex = 0;

                    btnNova.Tag = 1;
                    Modificado = true;
                }
            }
            else
            {
                ///
                /// salva a lista de empresas
                List<Empresa> temp = new List<Empresa>(Empresas.Configuracoes);
                try
                {
                    ///
                    /// compara o que foi mudado
                    ///
                    bool grava = DadosMudaramDaEmpresa(true);

                    if (!EmpresaValidada)
                        return;

                    if (grava)
                    {
                        currentEmpresa.RemoveEndSlash();

                        if (servicoCurrent != currentEmpresa.Servico)
                        {
                            var oe = Empresas.FindConfEmpresa(currentEmpresa.CNPJ, servicoCurrent);
                            if (oe != null)
                                Empresas.Configuracoes.Remove(oe);
                        }

                        string _key = currentEmpresa.CNPJ + currentEmpresa.Servico.ToString();
                        ///
                        /// salva a configuracao da empresa
                        ///

                        currentEmpresa.SalvarConfiguracao((currentEmpresa.Servico == TipoAplicativo.SAT ? false : true), true);

                        ValidarPastaBackup();

                        var app = new ConfiguracaoApp();
                        ///
                        /// salva o arquivo da lista de empresas
                        ///
                        app.GravarArqEmpresas();

                        if (uc_geral.Modificado)
                        {
                            ///
                            /// salva as configuracoes gerais
                            app.GravarConfigGeral();
                        }
                        ///
                        /// reload a lista de empresas
                        Empresas.CarregaConfiguracao();
                        ///
                        /// reload o ambiente p/ manutencao
                        CreateControles();
                        ///
                        /// reposiciona a empresa no combobox
                        ///
                        for (int item = 0; item < cbEmpresas.Items.Count; ++item)
                        {
                            ComboElem empr = cbEmpresas.Items[item] as ComboElem;
                            if (empr.Key.Equals(_key))
                            {
                                cbEmpresas.SelectedIndex = item;
                                break;
                            }
                        }
                    }
                    else
                    {
                        ///
                        /// a empresa nao mudou mas as propriedades gerais mudaram?
                        if (uc_geral.Modificado)
                        {
                            new ConfiguracaoApp().GravarConfigGeral();
                            uc_geral.PopulateConfGeral();
                        }
                    }
                    Modificado = false;
                    cbEmpresas.Visible = metroLabel2.Visible = true;
                }
                catch (Exception ex)
                {
                    ///
                    /// restaura a lista de empresas
                    Empresas.Configuracoes = new List<Empresa>(temp);

                    if (Convert.ToInt16(btnNova.Tag) == 1)//inclusao
                    {
                        ///
                        /// exclui as pastas criadas na inclusao
                        ///
                        try
                        {
                            currentEmpresa.ExcluiPastas();
                        }
                        catch { }
                    }
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "UniNFe");
                }
            }
        }

        #region ValidarPastaBackup()

        /// <summary>
        /// Verifica se tem alguma empresa sem informar a pasta de backup.
        /// Esta verificação só ocorre quando configurado manualmente no uninfe, quando é configurado pelo ERP, a pasta é opcional.
        /// </summary>
        private void ValidarPastaBackup()
        {
            for (int i = 0; i < Empresas.Configuracoes.Count; i++)
            {
                if (Empresas.Configuracoes[i].Servico != TipoAplicativo.Nfse)
                {
                    if (string.IsNullOrEmpty(Empresas.Configuracoes[i].PastaBackup))
                    {
                        throw new Exception("Não foi informado a pasta de backup dos XML autorizados da empresa " + Empresas.Configuracoes[i].Nome.Trim() + ".");
                    }
                }
            }
        }

        #endregion

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (btnExcluir.Text.Equals("Excluir"))
            {
                if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Deseja realmente excluir esta empresa?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        var list = (cbEmpresas.DataSource as System.Collections.ArrayList)[cbEmpresas.SelectedIndex] as ComboElem;
                        var _Empresa = Empresas.FindConfEmpresa(list.Valor, EnumHelper.StringToEnum<TipoAplicativo>(list.Servico));
                        if (_Empresa != null)
                        {
                            Empresas.Configuracoes.Remove(_Empresa);
                            new ConfiguracaoApp().GravarArqEmpresas();
                            CreateControles();

                            Auxiliar.WriteLog("Empresa '" + _Empresa.CNPJ + "' - Serviço: '" + _Empresa.Servico.ToString() + "' excluída", false);
                            /*
                                                        if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Deseja excluir as pastas desta empresa?\r\n\r\nExcluindo-as, serão eliminadas todos os XML's autorizados/denegados/eventos", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                                        {
                                                            ///
                                                            /// exclui as pastas criadas
                                                            ///
                                                            try
                                                            {
                                                                _Empresa.ExcluiPastas();
                                                            }
                                                            catch { }
                                                        }
                            */
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Não foi possível acessar a empresa para excluí-la");
                        }
                    }
                    catch (Exception ex)
                    {
                        MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                    }
                }
            }
            else
            {
                ///
                /// compara o que foi mudado
                ///
                try
                {
                    bool pergunta = DadosMudaramDaEmpresa(false);

                    if (EmpresaValidada)
                    {
                        if (pergunta)
                        {
                            pergunta = !(MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                                constAbandono, "",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                        }
                        if (!pergunta)
                        {
                            Modificado = false;
                            empcnpj = "";

                            if (Empresas.Configuracoes.Count > 0)
                            {
                                cbEmpresas_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                ///
                                /// as propriedades gerais mudou?
                                if (uc_geral.Modificado)
                                {
                                    new ConfiguracaoApp().GravarConfigGeral();
                                    uc_geral.PopulateConfGeral();
                                }
                                ///
                                /// como nao tem nenhuma empresa, fecha este processo voltando ao menu principal
                                BackFuncao();
                            }
                            cbEmpresas.Visible = metroLabel2.Visible = true;
                            btnExcluir.Visible = cbEmpresas.Items.Count > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                }
            }
        }

        private void ConfigurarAbas(Empresa empresa, bool novaempresa)
        {
            switch (empresa.Servico)
            {
                case TipoAplicativo.Nfse:
                    uce_divs.Populate(empresa, novaempresa);
                    uce_pastas.Populate(empresa);
                    uce_cert.Populate(empresa);
                    uce_ftp.Populate(empresa);
                    _tpEmpresa_cert.Parent = tc_empresa;
                    _tpEmpresa_ftp.Parent = tc_empresa;
                    _tpEmpresa_danfe.Parent = null;
                    _tpEmpresa_sat.Parent = null;
                    _tpEmpresa_resptecnico.Parent = null;
                    _tpEmpresa_outrasconfiguracoes.Parent = null;
                    break;
                case TipoAplicativo.SAT:
                    uce_divs.Populate(empresa, novaempresa);
                    uce_pastas.Populate(empresa);
                    uce_ftp.Populate(empresa);
                    uce_danfe.Populate(empresa);
                    uce_sat.Populate(empresa);
                    _tpEmpresa_sat.Parent = tc_empresa;
                    _tpEmpresa_cert.Parent = null;
                    _tpEmpresa_resptecnico.Parent = null;
                    _tpEmpresa_outrasconfiguracoes.Parent = null;
                    break;
                case TipoAplicativo.EFDReinf:
                case TipoAplicativo.eSocial:
                case TipoAplicativo.EFDReinfeSocial:
                    uce_divs.Populate(empresa, novaempresa);
                    uce_pastas.Populate(empresa);
                    uce_cert.Populate(empresa);
                    _tpEmpresa_cert.Parent = tc_empresa;
                    _tpEmpresa_ftp.Parent = null;
                    _tpEmpresa_danfe.Parent = null;
                    _tpEmpresa_sat.Parent = null;
                    _tpEmpresa_resptecnico.Parent = null;
                    _tpEmpresa_outrasconfiguracoes.Parent = null;
                    break;
                case TipoAplicativo.Nfe:
                case TipoAplicativo.NFCe:
                case TipoAplicativo.Cte:
                case TipoAplicativo.MDFe:
                case TipoAplicativo.Todos:
                    uce_divs.Populate(empresa, novaempresa);
                    uce_pastas.Populate(empresa);
                    uce_cert.Populate(empresa);
                    uce_ftp.Populate(empresa);
                    uce_danfe.Populate(empresa);
                    uce_resptecnico.Populate(empresa);
                    uce_outrasconfiguracoes.Populate(empresa);
                    _tpEmpresa_cert.Parent = tc_empresa;
                    _tpEmpresa_ftp.Parent = tc_empresa;
                    _tpEmpresa_danfe.Parent = tc_empresa;
                    _tpEmpresa_resptecnico.Parent = tc_empresa;
                    _tpEmpresa_outrasconfiguracoes.Parent = null;
                    _tpEmpresa_outrasconfiguracoes.Parent = tc_empresa;
                    _tpEmpresa_sat.Parent = null;
                    break;
                default:
                    uce_divs.Populate(empresa, novaempresa);
                    uce_pastas.Populate(empresa);
                    uce_cert.Populate(empresa);
                    uce_ftp.Populate(empresa);
                    uce_danfe.Populate(empresa);
                    _tpEmpresa_cert.Parent = tc_empresa;
                    _tpEmpresa_ftp.Parent = tc_empresa;
                    _tpEmpresa_danfe.Parent = tc_empresa;
                    _tpEmpresa_sat.Parent = null;
                    _tpEmpresa_resptecnico.Parent = null;
                    _tpEmpresa_outrasconfiguracoes.Parent = null;
                    break;
            }
        }
    }
}
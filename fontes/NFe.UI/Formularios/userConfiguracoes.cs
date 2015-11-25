using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

using NFe.Settings;
using NFe.Components;

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
        bool stopChangedEvent = false;
        #endregion

        MetroFramework.Controls.MetroTabPage _tpEmpresa_divs,
            _tpEmpresa_danfe,
            _tpEmpresa_pastas,
            _tpEmpresa_cert,
            _tpEmpresa_ftp;

        private string empcnpj = "";
        private TipoAplicativo servico;

        private Formularios.userConfiguracao_certificado uce_cert;
        public Formularios.userConfiguracao_danfe uce_danfe;
        private Formularios.userConfiguracao_diversos uce_divs;
        private Formularios.userConfiguracao_ftp uce_ftp;
        private Formularios.userConfiguracao_geral uc_geral = null;
        public Formularios.userConfiguracao_pastas uce_pastas;
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
                    uce_pastas = new Formularios.userConfiguracao_pastas();
                    uce_pastas.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_pastas);
                    break;
                case 2:
                    tpage.Text = "Certificado digital";
                    uce_cert = new Formularios.userConfiguracao_certificado();
                    uce_cert.changeEvent += changed_Modificado;
                    uce_cert.ucPastas = this.uce_pastas;
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
            }
            tpage.Controls[tpage.Controls.Count - 1].Dock = DockStyle.Fill;

            return tpage;

            #endregion
        }

        public override void UpdateControles()
        {
            #region --UpdateControles

            base.UpdateControles();

            if (first)
            {
                tc_empresa.TabPages.Clear();

                CreateControles();

                this.back_button.Tag = 1;   //previne que seja pressionado pelo UserControl1

                this.back_button.Click += delegate
                {
                    if (this.VerificaSeAbandona())
                    {
                        BackFuncao();
                    }
                    ConfiguracaoApp.CarregarDados();
                };

                this.tc_main.Selected += (_sender, _e) =>
                {
                    if (_e.TabPage == this.tpGeral)
                        this.uc_geral.FocusFirstControl();
                };
                this.tc_empresa.Selected += (_sender, _e) =>
                {
                    if (_e.TabPage == this._tpEmpresa_ftp) this.uce_ftp.FocusFirstControl();
                    if (_e.TabPage == this._tpEmpresa_pastas) this.uce_pastas.FocusFirstControl();
                    if (_e.TabPage == this._tpEmpresa_danfe) this.uce_danfe.FocusFirstControl();
                    if (_e.TabPage == this._tpEmpresa_cert) this.uce_cert.FocusFirstControl();
                };

                this.tc_main.SelectedIndex = 1;
                this.tc_empresa.SelectedIndex = 0;

                if (this.cbEmpresas.Items.Count == 0)
                    btnNova_Click(null, null);
            }
            first = false;

            #endregion
        }

        private void CreateControles()
        {
            #region --CreateControles

            this.cbEmpresas.Visible = this.metroLabel2.Visible = true;

            if (uc_geral == null)
            {
                this.tc_empresa.TabPages.Add(this._tpEmpresa_divs = this.createtpage(0));
                this.tc_empresa.TabPages.Add(this._tpEmpresa_pastas = this.createtpage(1));
                this.tc_empresa.TabPages.Add(this._tpEmpresa_cert = this.createtpage(2));
                this.tc_empresa.TabPages.Add(this._tpEmpresa_ftp = this.createtpage(3));
                this.tc_empresa.TabPages.Add(this._tpEmpresa_danfe = this.createtpage(4));
                uc_geral = new Formularios.userConfiguracao_geral();
                this.tpGeral.Controls.Add(uc_geral);
            }

            this.tc_main.SelectedTab = this.tpGeral;
            this.tc_empresa.SelectedTab = this._tpEmpresa_divs;

            this.cbEmpresas.SelectedIndexChanged -= cbEmpresas_SelectedIndexChanged;
            this.cbEmpresas.DataSource = null;
            this.cbEmpresas.DisplayMember = NFe.Components.NFeStrConstants.Nome;
            this.cbEmpresas.ValueMember = "Key";
            this.cbEmpresas.DataSource = Auxiliar.CarregaEmpresa(false);
            this.btnExcluir.Visible = this.cbEmpresas.Items.Count > 0;

            ConfiguracaoApp.CarregarDados();
            uc_geral.PopulateConfGeral();

            userConfiguracoes_Resize(null, null);

            if (this.cbEmpresas.Items.Count > 0)
            {
                this.tc_empresa.Enabled =
                    this.btnExcluir.Enabled =
                    this.cbEmpresas.Enabled = true;
                this.cbEmpresas.SelectedIndex = 0;
                this.cbEmpresas.SelectedIndexChanged += cbEmpresas_SelectedIndexChanged;
                cbEmpresas_SelectedIndexChanged(null, null);
            }
            else
                this.CopiaDadosDaEmpresaParaControls(new Empresa(), true);

            #endregion
        }

        private void userConfiguracoes_Resize(object sender, EventArgs e)
        {
            if (uc_geral != null)
            {
                ///
                /// centraliza o usercontrol das configuracoes gerais
                int w = uc_geral.Size.Width;
                //int h = uc_geral.Size.Height;
                int left = (this.Size.Width - w) / 2;
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
            oempresa.CompactarNfe = empresa.CompactarNfe;
            oempresa.IndSinc = empresa.IndSinc;

            oempresa.CriaPastasAutomaticamente = true;
        }

        void CopiaDadosDaEmpresaParaControls(Empresa oempresa, bool empty)
        {
            bool _modificado = false;
            bool _nova = string.IsNullOrEmpty(oempresa.PastaXmlEnvio);
            
            stopChangedEvent = true;
            try
            {
                this.servicoCurrent = oempresa.Servico;
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
                uce_divs.Populate(oempresa, _nova);
                uce_pastas.Populate(oempresa);
                uce_ftp.Populate(oempresa);
                uce_cert.Populate(oempresa);

                if (oempresa.Servico != TipoAplicativo.Nfse)
                {
                    _tpEmpresa_danfe.Parent = tc_empresa;
                    uce_danfe.Populate(oempresa);
                }
                else
                {
                    if (_tpEmpresa_danfe != null)
                        _tpEmpresa_danfe.Parent = null;
                }
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
                return this._Modificado;
            }
            set
            {
                if (value)
                {
                    this.btnNova.Text = "Salvar";
                    this.btnExcluir.Text = "Cancelar";
                }
                else
                {
                    this.btnNova.Text = "Nova";
                    this.btnExcluir.Text = "Excluir";
                }
                this.cbEmpresas.Enabled = !value;
                this.btnExcluir.Enabled = true;
                this._Modificado = value;
            }
        }

        private void changed_Modificado(object sender, EventArgs e)
        {
            if (!stopChangedEvent) this.Modificado = true;
        }

        private void cbEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region

            this.currentEmpresa = null;

            var list = (this.cbEmpresas.DataSource as System.Collections.ArrayList)[this.cbEmpresas.SelectedIndex] as NFe.Components.ComboElem;
            var empresa = Empresas.FindConfEmpresa(list.Valor, NFe.Components.EnumHelper.StringToEnum<TipoAplicativo>(list.Servico));
            if (empresa == null)
            {
                CopiaDadosDaEmpresaParaControls(new Empresa(), true);
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Não pode acessar os dados da empresa selecionada", "");
            }
            else
            {
                this.currentEmpresa = new Empresa();
                empresa.CopyObjectTo(currentEmpresa);
                empcnpj = this.currentEmpresa.CNPJ;
                servico = this.currentEmpresa.Servico;

                CopiaDadosDaEmpresaParaControls(this.currentEmpresa, false);

                this.btnNova.Tag = 0;
            }
            #endregion
        }

        const string constAbandono = "Dados da configuração foram alterados, abandona mesmo assim?";

        public bool VerificaSeAbandona()
        {
            #region

            if (this.tc_empresa.Enabled)
            {
                if (DadosMudaramDaEmpresa(false) || this.uc_geral.Modificado || !this.EmpresaValidada)
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

            this.EmpresaValidada = true;

            if (this.currentEmpresa == null || !this.Modificado)
                return false;

            try
            {
                this.uc_geral.Validar();
                if (!this.uce_divs.Validar(exibeerro, Convert.ToInt16(this.btnNova.Tag) == 1))
                {
                    this.EmpresaValidada = false;
                    return false;
                }
                this.uce_pastas.Validar();
                this.uce_cert.Validar();
                if (this.currentEmpresa.Servico != TipoAplicativo.Nfse)
                    this.uce_danfe.Validar();
                this.uce_ftp.Validar();
            }
            catch (Exception ex)
            {
                if (exibeerro)
                    throw ex;
            }

            if (Convert.ToInt16(this.btnNova.Tag) == 1) //se inclusao nao precisa verificar se mudou algo, já que nao tem classe existente
                return this.Modificado;

            var _Empresa = Empresas.FindConfEmpresa(this.empcnpj, this.servico);
            if (_Empresa == null)
                return this.Modificado;

            PropertyInfo[] allClassToProperties = _Empresa.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var pi in allClassToProperties)
            {
                if (pi.CanWrite)
                {
                    var pii = this.currentEmpresa.GetType().GetProperty(pi.Name, BindingFlags.Instance | BindingFlags.Public);
                    if (pii != null)
                    {
                        object ob1 = pi.GetValue(_Empresa, null);
                        object ob2 = pii.GetValue(this.currentEmpresa, null);
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
            if (this.btnNova.Text.Equals("Nova"))
            {
                bool ok = false;
                using (Formularios.FormNova f = new Formularios.FormNova(this.FindForm()))
                {
                    ok = f.ShowDialog() == DialogResult.OK;

                    if (ok)
                    {
                        this.currentEmpresa = new Empresa();
                        this.currentEmpresa.CNPJ = NFe.Components.Functions.OnlyNumbers(f.edtCNPJ.Text, ".,-/").ToString().PadLeft(14, '0');
                        this.currentEmpresa.Nome = f.edtNome.Text;
                        this.currentEmpresa.Servico = (TipoAplicativo)f.cbServico.SelectedValue;
                        if (this.currentEmpresa.Servico == TipoAplicativo.Nfse)
                            this.currentEmpresa.UnidadeFederativaCodigo = 0;
                        else
                            this.currentEmpresa.UnidadeFederativaCodigo = 41;
                    }
                }
                if (ok)
                {
                    this.cbEmpresas.Visible = this.metroLabel2.Visible = false;
                    this.CopiaDadosDaEmpresaParaControls(currentEmpresa, false);

                    this.tc_main.SelectedIndex = 1;
                    this.tc_empresa.SelectedIndex = 0;

                    this.btnNova.Tag = 1;
                    this.Modificado = true;
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

                    if (!this.EmpresaValidada)
                        return;

                    if (grava)
                    {
                        currentEmpresa.RemoveEndSlash();

                        if (this.servicoCurrent != this.currentEmpresa.Servico)
                        {
                            var oe = Empresas.FindConfEmpresa(this.currentEmpresa.CNPJ, this.servicoCurrent);
                            if (oe != null)
                                Empresas.Configuracoes.Remove(oe);
                        }

                        string _key = this.currentEmpresa.CNPJ + this.currentEmpresa.Servico.ToString();
                        ///
                        /// salva a configuracao da empresa
                        /// 

                        this.currentEmpresa.SalvarConfiguracao(true, true);


                        var app = new ConfiguracaoApp();
                        ///
                        /// salva o arquivo da lista de empresas
                        ///                         
                        app.GravarArqEmpresas();

                        if (this.uc_geral.Modificado)
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
                        this.CreateControles();
                        ///
                        /// reposiciona a empresa no combobox
                        /// 
                        for (int item = 0; item < this.cbEmpresas.Items.Count; ++item)
                        {
                            NFe.Components.ComboElem empr = this.cbEmpresas.Items[item] as NFe.Components.ComboElem;
                            if (empr.Key.Equals(_key))
                            {
                                this.cbEmpresas.SelectedIndex = item;
                                break;
                            }
                        }
                    }
                    else
                    {
                        ///
                        /// a empresa nao mudou mas as propriedades gerais mudaram?
                        if (this.uc_geral.Modificado)
                        {
                            new ConfiguracaoApp().GravarConfigGeral();
                            this.uc_geral.PopulateConfGeral();
                        }
                    }
                    this.Modificado = false;
                    this.cbEmpresas.Visible = this.metroLabel2.Visible = true;
                }
                catch (Exception ex)
                {
                    ///
                    /// restaura a lista de empresas
                    Empresas.Configuracoes = new List<Empresa>(temp);

                    if (Convert.ToInt16(this.btnNova.Tag) == 1)//inclusao
                    {
                        ///
                        /// exclui as pastas criadas na inclusao
                        /// 
                        try
                        {
                            this.currentEmpresa.ExcluiPastas();
                        }
                        catch { }
                    }
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (this.btnExcluir.Text.Equals("Excluir"))
            {
                if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Deseja realmente excluir esta empresa?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        var list = (this.cbEmpresas.DataSource as System.Collections.ArrayList)[this.cbEmpresas.SelectedIndex] as NFe.Components.ComboElem;
                        var _Empresa = Empresas.FindConfEmpresa(list.Valor, NFe.Components.EnumHelper.StringToEnum<TipoAplicativo>(list.Servico));
                        if (_Empresa != null)
                        {
                            Empresas.Configuracoes.Remove(_Empresa);
                            new ConfiguracaoApp().GravarArqEmpresas();
                            this.CreateControles();

                            Auxiliar.WriteLog("Empresa '" + _Empresa.CNPJ + "' - Serviço: '" + _Empresa.Servico.ToString() + "' excluída", false);

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

                    if (this.EmpresaValidada)
                    {
                        if (pergunta)
                        {
                            pergunta = !(MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                                constAbandono, "", 
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                        }
                        if (!pergunta)
                        {
                            this.Modificado = false;
                            this.empcnpj = "";

                            if (Empresas.Configuracoes.Count > 0)
                            {
                                cbEmpresas_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                ///
                                /// as propriedades gerais mudou?
                                if (this.uc_geral.Modificado)
                                {
                                    new ConfiguracaoApp().GravarConfigGeral();
                                    this.uc_geral.PopulateConfGeral();
                                }
                                ///
                                /// como nao tem nenhuma empresa, fecha este processo voltando ao menu principal
                                this.BackFuncao();
                            }
                            this.cbEmpresas.Visible = this.metroLabel2.Visible = true;
                            this.btnExcluir.Visible = this.cbEmpresas.Items.Count > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                }
            }
        }
    }
}

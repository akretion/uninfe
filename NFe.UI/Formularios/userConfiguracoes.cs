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

        private MetroFramework.Controls.MetroTabPage createtpage(int id)
        {
            var tpage = new MetroFramework.Controls.MetroTabPage();
            tpage.Padding = new Padding(2);
            tpage.Theme = uninfeDummy.mainForm.uTheme;
            tpage.Style = uninfeDummy.mainForm.uStyle;

            switch (id)
            {
                case 0:
                    tpage.Text = "Principal";
                    tpage.AutoScroll = (Propriedade.TipoAplicativo == TipoAplicativo.Nfe || Propriedade.TipoExecucao == TipoExecucao.teAll);
                    uce_divs = new Formularios.userConfiguracao_diversos();
                    uce_divs.uConfiguracoes = this;
                    uce_divs.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_divs); 
                    break;
                case 1:
                    tpage.Text = "Pastas";
                    tpage.AutoScroll = (Propriedade.TipoAplicativo == TipoAplicativo.Nfe || Propriedade.TipoExecucao == TipoExecucao.teAll);
                    uce_pastas = new Formularios.userConfiguracao_pastas();
                    uce_pastas.changeEvent += changed_Modificado;
                    tpage.Controls.Add(uce_pastas); 
                    break;
                case 2:
                    tpage.Text = "Certificado digital";
                    uce_cert = new Formularios.userConfiguracao_certificado();
                    uce_cert.changeEvent += changed_Modificado;
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
        }

        public override void UpdateControles()
        {
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

                if (this.cbEmpresas.Items.Count == 0)
                    btnNova_Click(null, null);
            }
            first = false;
        }

        private void CreateControles()
        {
            if (uc_geral == null)
            {
                this.tc_empresa.TabPages.Add(this._tpEmpresa_divs = this.createtpage(0));
                this.tc_empresa.TabPages.Add(this._tpEmpresa_pastas = this.createtpage(1));
                this.tc_empresa.TabPages.Add(this._tpEmpresa_cert = this.createtpage(2));
                this.tc_empresa.TabPages.Add(this._tpEmpresa_ftp = this.createtpage(3));
                if (Propriedade.TipoAplicativo == TipoAplicativo.Nfe || Propriedade.TipoExecucao == TipoExecucao.teAll)
                {
                    this.tc_empresa.TabPages.Add(this._tpEmpresa_danfe = this.createtpage(4));
                }

                uc_geral = new Formularios.userConfiguracao_geral();
                this.tpGeral.Controls.Add(uc_geral);
            }

            this.tc_main.SelectedTab = this.tpGeral;
            this.tc_empresa.SelectedTab = this._tpEmpresa_divs;

            this.cbEmpresas.SelectedIndexChanged -= cbEmpresas_SelectedIndexChanged;
            this.cbEmpresas.DataSource = null;
            this.cbEmpresas.DisplayMember = NFe.Components.NFeStrConstants.Nome;
            this.cbEmpresas.ValueMember = "Valor";
            this.cbEmpresas.DataSource = Auxiliar.CarregaEmpresa(false);

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

            string ctemp = destino.CNPJ + (destino.Servico == TipoAplicativo.Nfe || destino.Servico == TipoAplicativo.Todos ? "" : "\\" + destino.Servico.ToString().ToLower());
            string newPasta = origemPasta.Replace(origemCNPJ, ctemp);

            if (origemPasta.ToLower() == newPasta.ToLower())
            {
                int lastBackSlash = ConfiguracaoApp.RemoveEndSlash(origemPasta).LastIndexOf("\\");
                newPasta = origemPasta.Insert(lastBackSlash, "\\" + ctemp);
            }
            return newPasta;
        }

        void CopiaDadosDaEmpresaParaControls(Empresa oempresa, bool empty)
        {
            bool _modificado = false;
            stopChangedEvent = true;
            try
            {
                oempresa.CriaPastasAutomaticamente = false;

                this.tc_main.SelectedIndex = 1;
                this.tc_empresa.SelectedIndex = 0;

                if (string.IsNullOrEmpty(oempresa.PastaXmlEnvio) && !empty)
                {
                    ///
                    /// tenta achar uma configuracao valida
                    /// 
                    foreach (Empresa empresa in Empresas.Configuracoes)
                    {
                        if (empresa.CNPJ.Trim() != oempresa.CNPJ && 
                            empresa.Servico != oempresa.Servico && 
                            !string.IsNullOrEmpty(empresa.PastaXmlEnvio))
                        {
                            string cpath = empresa.CNPJ + (empresa.Servico == TipoAplicativo.Nfse ? "\\nfse" : "");

                            oempresa.PastaXmlEnvio = CopiaPastaDeEmpresa(cpath, empresa.PastaXmlEnvio, oempresa);
                            oempresa.PastaXmlRetorno = CopiaPastaDeEmpresa(cpath, empresa.PastaXmlRetorno, oempresa);
                            oempresa.PastaXmlErro = CopiaPastaDeEmpresa(cpath, empresa.PastaXmlErro, oempresa);
                            oempresa.PastaValidar = CopiaPastaDeEmpresa(cpath, empresa.PastaValidar, oempresa);
                            if (empresa.Servico != TipoAplicativo.Nfse)//Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                            {
                                oempresa.PastaXmlEmLote = CopiaPastaDeEmpresa(cpath, empresa.PastaXmlEmLote, oempresa);
                                oempresa.PastaXmlEnviado = CopiaPastaDeEmpresa(cpath, empresa.PastaXmlEnviado, oempresa);
                                oempresa.PastaBackup = CopiaPastaDeEmpresa(cpath, empresa.PastaBackup, oempresa);
                                oempresa.PastaDownloadNFeDest = CopiaPastaDeEmpresa(cpath, empresa.PastaDownloadNFeDest, oempresa);
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
                            _modificado = true;
                            break;
                        }
                    }
                    ///
                    /// se ainda assim nao foi encontrada nenhuma configuracao válida assume a pasta de instalacao do uninfe
                    /// 
                    if (string.IsNullOrEmpty(oempresa.PastaXmlEnvio))
                    {
                        string subpasta = oempresa.CNPJ + (oempresa.Servico == TipoAplicativo.Nfse ? "\\nfse" : "");

                        oempresa.PastaXmlEnvio = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Envio");
                        oempresa.PastaXmlRetorno = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Retorno");
                        oempresa.PastaXmlErro = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Erro");
                        oempresa.PastaValidar = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Validar");
                        if (oempresa.Servico != TipoAplicativo.Nfse)//Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                        {
                            oempresa.PastaXmlEnviado = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Enviado");
                            //oempresa.PastaBackup = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\Backup");
                            oempresa.PastaXmlEmLote = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\EnvioEmLote");
                            oempresa.PastaDownloadNFeDest = Path.Combine(Propriedade.PastaExecutavel, subpasta + "\\DownloadNFe");
                        }
                        oempresa.CriaPastasAutomaticamente = true;
                        _modificado = true;
                    }
                }
                uce_divs.Populate(oempresa);
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
        private bool Modificado {
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
            this.currentEmpresa = null;

            var list = (this.cbEmpresas.DataSource as System.Collections.ArrayList)[this.cbEmpresas.SelectedIndex] as NFe.Components.ComboElem;
            var empresa = Empresas.FindConfEmpresa(this.cbEmpresas.SelectedValue.ToString(), NFe.Components.EnumHelper.StringToEnum<TipoAplicativo>(list.Servico));
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
        }

        public bool VerificaSeAbandona()
        {
            if (this.tc_empresa.Enabled)
            {
                if (DadosMudaramDaEmpresa(false) || this.uc_geral.Modificado)
                    return MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Dados da configuração foram alterados, abandona mesmo assim?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            }
            BackFuncao();
            return true;
        }

        private bool DadosMudaramDaEmpresa(bool exibeerro)
        {
            if (this.currentEmpresa == null || !this.Modificado)
                return false;

            try
            {
                this.uc_geral.Validar();
                this.uce_divs.Validar();
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
                    this.CopiaDadosDaEmpresaParaControls(currentEmpresa, false);

                    this.btnNova.Tag = 1;
                    this.Modificado = true;
                }
            }
            else
            {
                try
                {
                    ///
                    /// compara o que foi mudado
                    /// 
                    bool grava = DadosMudaramDaEmpresa(true);
                    if (grava)
                    {
                        //throw new Exception(currentEmpresa.PastaXmlEnvio);
                        ///
                        /// salva a posicao no combo da empresa sendo editada
                        int i = this.cbEmpresas.SelectedIndex;
                        ///
                        /// salva a configuracao da empresa
                        this.currentEmpresa.SalvarConfiguracao(true);

                        var app = new ConfiguracaoApp();
                        ///
                        /// salva o arquivo da lista de empresas
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
                        if (i >= 0)
                            this.cbEmpresas.SelectedIndex = i;
                    }
                    else
                    {
                        ///
                        /// a empresa nao mudou mas as propriedades gerais mudou?
                        if (this.uc_geral.Modificado)
                        {
                            new ConfiguracaoApp().GravarConfigGeral();
                            this.uc_geral.PopulateConfGeral();
                        }
                    }
                    this.Modificado = false;
                }
                catch (Exception ex)
                {
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
                if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Confirma a exclusão desta empresa?","", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Deseja realmente excluir esta empresa?","", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

                                if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Deseja excluir as pastas desta empresa?","", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                        }
                        catch (Exception ex)
                        {
                            MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                        }
                    }
                }
            }
            else
            {
                ///
                /// compara o que foi mudado
                /// 
                bool pergunta = DadosMudaramDaEmpresa(false);
                if (pergunta)
                {
                    pergunta = !(MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Confirma o abandono da edição desta empresa?","", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
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
                }
            }
        }
    }
}

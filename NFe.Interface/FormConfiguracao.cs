using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using NFe.Settings;
using NFe.Components;

namespace NFe.Interface
{
    public delegate void UpdateText(string nome);

    public partial class FormConfiguracao: Form
    {
        ///
        /// danasa 9-2010
        private EventHandler OnMyClose;
        private bool Salvos;
        private bool Modificado;
        private List<Empresa> tempEmpresas = new List<Empresa>();
        /// <summary>
        /// Se a senha de acesso a tela de configurações foi digitada corretamente
        /// </summary>
        public bool AcessoAutorizado { get; private set; }

        ///
        /// danasa 9-2010
        public FormConfiguracao(EventHandler _OnClose, Form mdiParent)
        {
            InitializeComponent();
            //Wandrey 24/04/2011
            MdiParent = mdiParent;

            /// danasa 9-2010
            OnMyClose = _OnClose;
            Salvos = false;
            Modificado = false;
            tabControl4.SelectedIndex = 0;

            if(!SolicitaSenha())
            {
                AcessoAutorizado = false;
                this.Close();
            }
            else
            {
                AcessoAutorizado = true;
                PopulateCbEmpresa();
                PopulateConfGeral();
            }
        }

        private void FormConfiguracao_Load(object sender, EventArgs e)
        {
            ///
            /// danasa 9-2009
            /// 
            XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
            iniFile.LoadForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"), true);

            tabControl4_SelectedIndexChanged(sender, e);
        }

        #region Métodos gerais

        private bool SolicitaSenha()
        {
            bool retorna = true;

            if(!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig))
            {
                FormSenha senha = new FormSenha();

                if(this.MdiParent == null)
                {
                    senha.StartPosition = FormStartPosition.CenterScreen;
                    senha.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    senha.ShowInTaskbar = true;
                }
                else
                {
                    senha.StartPosition = FormStartPosition.CenterParent;
                    senha.FormBorderStyle = FormBorderStyle.None;
                    senha.ShowInTaskbar = false;
                }

                senha.ShowDialog();

                if(!senha.AcessoAutorizado || senha.AcessoCancelado)
                    retorna = false;
            }

            return retorna;
        }

        #region PopulateConfGeral()
        /// <summary>
        /// Popular os campos de configurações gerais
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        private void PopulateConfGeral()
        {
            //ConfiguracaoApp oCarrega = new ConfiguracaoApp();
            ConfiguracaoApp.CarregarDados();
            //ConfiguracaoApp.CarregarDadosSobre();

            cbProxy.Checked = ConfiguracaoApp.Proxy;
            tbServidor.Text = ConfiguracaoApp.ProxyServidor;
            tbUsuario.Text = ConfiguracaoApp.ProxyUsuario;
            tbSenha.Text = ConfiguracaoApp.ProxySenha;
            nudPorta.Value = ConfiguracaoApp.ProxyPorta;
            tbSenhaConfig.Text = ConfiguracaoApp.SenhaConfig;
            tbSenhaConfig2.Text = ConfiguracaoApp.SenhaConfig;
            cbChecaConexaoInternet.Checked = ConfiguracaoApp.ChecarConexaoInternet;
            chkGravarLogOperacao.Checked = ConfiguracaoApp.GravarLogOperacoesRealizadas;
        }
        #endregion

        #region PopulateCbEmpresa()
        /// <summary>
        /// Popular a ComboBox das empresas
        /// </summary>
        /// <remarks>
        /// Observações: Tem que popular separadamente do Método Populate() para evitar ficar recarregando na hora que selecionamos outra empresa
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        private void PopulateCbEmpresa()
        {
            try
            {
                foreach(Empresa elemen in Empresas.Configuracoes)
                {
                    string strNome;
                    if(elemen.Nome.Length > 20)
                        strNome = elemen.Nome.Substring(0, 20);
                    else
                        strNome = elemen.Nome;
                    tempEmpresas.Add(elemen);
                    TabPage page = new TabPage(strNome);
                    ucConfiguracao dados = new ucConfiguracao(null);
                    dados.PopulateConfEmpresa(elemen.CNPJ, elemen.Servico);
                    dados.Tag = elemen.CNPJ;
                    page.Controls.Add(dados);
                    dados.Dock = DockStyle.Fill;
                    this.tabControl4.TabPages.Add(page);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        public void UpdateText(string nome)
        {
            if(!string.IsNullOrEmpty(nome))
                this.tabControl4.TabPages[this.tabControl4.SelectedIndex].Text = nome;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            TabPage page = new TabPage("-- NOVA --");
            ucConfiguracao dados = new ucConfiguracao(UpdateText);
            dados.Tag = "new"; //para indicar que é uma nova empresa
            page.Controls.Add(dados);
            dados.Dock = DockStyle.Fill;
            this.tabControl4.TabPages.Add(page);
            this.tabControl4.SelectedIndex = this.tabControl4.TabPages.Count - 1;
            dados.PopulateConfEmpresa("", TipoAplicativo.Nfe);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int ativa = this.tabControl4.SelectedIndex;
            if(ativa == 0)
                return;

            ucConfiguracao control = (ucConfiguracao)this.tabControl4.TabPages[this.tabControl4.SelectedIndex].Controls[0];
            if(control != null)
            {
                if(MessageBox.Show("Exclui esta empresa?", "Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if(Empresas.FindConfEmpresa(control.oEmpresa.CNPJ, control.oEmpresa.Servico) != null)
                        Empresas.Configuracoes.Remove(control.oEmpresa);

                    control.Dispose();

                    //  this.tabControl4.TabPages.RemoveAt(ativa);
                    if(this.tabControl4.TabPages[ativa] is TabPage)
                        this.tabControl4.TabPages.Remove(this.tabControl4.TabPages[ativa]);
                    this.Modificado = true;

                    if(ativa >= this.tabControl4.TabPages.Count)
                        this.tabControl4.SelectedIndex = this.tabControl4.TabPages.Count - 1;
                    else
                        this.tabControl4.SelectedIndex = ativa;
                }
            }
        }

        private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.tabControl4.SelectedIndex > 0)
            {
                ucConfiguracao control = (ucConfiguracao)this.tabControl4.TabPages[this.tabControl4.SelectedIndex].Controls[0];
                control.focusNome();
            }
            tbDelete.Enabled = this.tabControl4.SelectedIndex > 0;
        }

        private void FormConfiguracao_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                if(!this.Salvos && this.TeveModificacao())
                {
                    switch(MessageBox.Show("Dados foram alterados, deseja salvá-los?", "Advertência...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Cancel:
                            //continua a editar
                            e.Cancel = true;
                            break;
                        case DialogResult.Yes:
                            e.Cancel = !this.Salvar();
                            break;
                        case DialogResult.No:
                            //byebye
                            this.Salvos = false;
                            Empresas.Configuracoes.Clear();
                            Empresas.Configuracoes.AddRange(tempEmpresas);
                            break;
                    }
                }
            }
            if(!e.Cancel)
            {
                ///
                /// danasa 9-2009
                /// 
                XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
                iniFile.SaveForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"));
                iniFile.Save();
                ///
                /// danasa 9-2010
                /// 
                if(OnMyClose != null)
                {
                    if(this.Salvos)    //danasa 20-9-2010
                    {
                        OnMyClose(sender, null);
                    }
                }
            }
        }


        #endregion

        private void tbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool Salvar()
        {
            if (cbProxy.Checked &&
                (nudPorta.Value == 0 ||
                string.IsNullOrEmpty(tbServidor.Text) ||
                string.IsNullOrEmpty(tbUsuario.Text) ||
                string.IsNullOrEmpty(tbSenha.Text)))
            {
                MessageBox.Show(NFeStrConstants.proxyError, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (this.tabControl4.TabPages.Count == 1)
            {
                MessageBox.Show("É necessário cadastrar e configurar a(s) empresa(s) que será(ão) gerenciada(s) pelo aplicativo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Verificar se as senhas são idênticas
            if(tbSenhaConfig.Text.Trim() != tbSenhaConfig2.Text.Trim())
            {
                MessageBox.Show("As senhas de acesso a tela de configurações devem ser idênticas.", "Senhas diferentes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //Atualizar as propriedades das configurações da empresa
            foreach(TabPage page in this.tabControl4.TabPages)
            {
                if(page.Controls[0] is ucConfiguracao)
                {
                    ucConfiguracao dados = (ucConfiguracao)page.Controls[0];
                    dados.AtualizarPropriedadeEmpresa();

                    if(string.IsNullOrEmpty(dados.oEmpresa.CNPJ))
                    {
                        this.tabControl4.SelectedTab = page;
                        MessageBox.Show("CNPJ deve ser informado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if(string.IsNullOrEmpty(dados.oEmpresa.Nome))
                    {
                        this.tabControl4.SelectedTab = page;
                        MessageBox.Show("Nome da empresa deve ser informado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if(dados.Tag.ToString() == "new" && Empresas.FindConfEmpresa(dados.oEmpresa.CNPJ, dados.oEmpresa.Servico) != null)
                    {
                        this.tabControl4.SelectedTab = page;
                        MessageBox.Show("Empresa/CNPJ já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            //Salvar as configurações
            ConfiguracaoApp oConfig = new ConfiguracaoApp();
            try
            {
                //inclui a(s) empresa(s) incluida(s) na lista de empresas
                foreach(TabPage page in this.tabControl4.TabPages)
                {
                    if(page.Controls[0] is ucConfiguracao)
                    {
                        ucConfiguracao dados = (ucConfiguracao)page.Controls[0];
                        if(Empresas.FindConfEmpresa(dados.oEmpresa.CNPJ, dados.oEmpresa.Servico) == null)
                        {
                            page.Controls[0].Tag = dados.oEmpresa.CNPJ;
                            Empresas.Configuracoes.Add(dados.oEmpresa);
                        }
                    }
                }
                //Atualizar as propriedades das configurações gerais
                ConfiguracaoApp.Proxy = this.cbProxy.Checked;
                ConfiguracaoApp.ProxyPorta = (int)this.nudPorta.Value;
                ConfiguracaoApp.ProxySenha = this.tbSenha.Text;
                ConfiguracaoApp.ProxyServidor = tbServidor.Text;
                ConfiguracaoApp.ProxyUsuario = tbUsuario.Text;
                ConfiguracaoApp.ChecarConexaoInternet = cbChecaConexaoInternet.Checked;
                ConfiguracaoApp.GravarLogOperacoesRealizadas = chkGravarLogOperacao.Checked;

                if(string.IsNullOrEmpty(tbSenhaConfig.Text))
                {
                    ConfiguracaoApp.SenhaConfig = string.Empty;
                }
                else
                {
                    //a geracao do MD5 ficará no metodo que grava a configuracao
                    ConfiguracaoApp.SenhaConfig = tbSenhaConfig.Text;
                }

                oConfig.GravarConfig(true, true); ///<<<<<danasa 1-5-2011

                this.Salvos = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return this.Salvos;
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            ValidateConfiguracao();

            this.Salvar();
            if(this.Salvos)
                this.Close();   //danasa 20-9-2010 - força a entrada em "Configuracao_FormClosed"
        }

        /// <summary>
        /// Validar o componente de configuração antes de continuar.
        /// </summary>
        /// <remarks>Isso foi feito devido a necessidade de se perguntar se os diretórios 
        /// deveriam ter a mesma estrutura do diretório de envio</remarks>
        private void ValidateConfiguracao()
        {
            foreach(TabPage page in this.tabControl4.TabPages)
            {
                if(page.Controls[0] is ucConfiguracao)
                {
                    ucConfiguracao control = (ucConfiguracao)page.Controls[0];
                    control.Validate();
                }
            }
        }

        private void cbProxy_CheckedChanged(object sender, EventArgs e)
        {
            lblPorta.Enabled =
                lblSenha.Enabled =
                lblUsuario.Enabled =
                lblServidor.Enabled =
                tbUsuario.Enabled =
                tbSenha.Enabled =
                nudPorta.Enabled =
                tbServidor.Enabled = cbProxy.Checked;

            this.Modificado = true;
        }

        private void change_Modificado(object sender, EventArgs e)
        {
            this.Modificado = true;
        }

        private bool TeveModificacao()
        {
            if(this.Modificado) return true;

            foreach(TabPage page in this.tabControl4.TabPages)
            {
                if(page.Controls[0] is ucConfiguracao)
                {
                    ucConfiguracao control = (ucConfiguracao)page.Controls[0];
                    if(control.Modificado) return true;
                }
            }
            return false;
        }
    }
}

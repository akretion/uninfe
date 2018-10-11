using MetroFramework.Forms;
using NFe.Components;
using NFe.Settings;
using NFe.Threadings;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.Design.AxImporter;

namespace NFe.UI
{
    public partial class Form_Main : MetroForm
    {
        private bool first = false;
        private bool servicoInstaladoErodando = false;
        private string srvName = Propriedade.ServiceName;
        private menu _menu;
        private bool _maximized;
        private bool _formloaded = false;

        public Form_Main()
        {
            InitializeComponent();

            uninfeDummy.mainForm = this;
            uninfeDummy.UltimoAcessoConfiguracao = DateTime.MinValue;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool error = false;
            try
            {
                //
                //SERVICO: danasa 7/2011
                //servico está instalado e rodando?
                this.servicoInstaladoErodando = Propriedade.ServicoRodando;

                this.tbSeparator1.Visible =
                    this.tbRestartServico.Visible =
                    this.tbPararServico.Visible = this.servicoInstaladoErodando;

                ///
                /// danasa 9-2009
                ///
                this.MinimumSize = new Size(800, 630);
                this.MaximumSize = new Size(800, 630);
                //Trazer minimizado e no systray
                this.notifyIcon1.Text = NFe.Components.Propriedade.NomeAplicacao + " - " + NFe.Components.Propriedade.DescricaoAplicacao;
                this.notifyIcon1.Visible = true;
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.ShowInTaskbar = false;

                ConfiguracaoApp.StartVersoes();

                _menu = new menu();
                Controls.Add(_menu);
                _menu.Dock = DockStyle.Fill;

                notifyIcon1.Icon = Icon = Properties.Resources.uninfe_icon;

                cmAbrir.Text = "Abrir " + Propriedade.NomeAplicacao;
                cmFechar.Text = "Fechar " + Propriedade.NomeAplicacao;
                cmSobre.Text = "Sobre o " + Propriedade.NomeAplicacao;
                cmManual.Text = "Manual do " + Propriedade.NomeAplicacao;
                cmManual.Enabled = File.Exists(Path.Combine(Propriedade.PastaExecutavel, Propriedade.NomeAplicacao + ".pdf"));

                string msg = "";
                Propriedade.VerificaArquivos(out error, out msg);
                if (error)
                {
                    MetroFramework.MetroMessageBox.Show(this, msg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }

                if (!this.servicoInstaladoErodando)     //danasa 12/8/2011
                    //Definir eventos de controles de execução das thread´s de serviços do UniNFe. Wandrey 26/07/2011
                    new ThreadControlEvents();  //danasa 12/8/2011

                //Executar os serviços do UniNFe em novas threads
                //Tem que ser carregado depois que o formulário da MainForm estiver totalmente carregado para evitar Erros. Wandrey 19/10/2010
                this.ExecutaServicos();
            }
            finally
            {
                if (!error)
                    this.updateControleDoServico();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (first)
            {
                //Faz a aplicação sumir da barra de tarefas
                //danasa
                //  Se usuario mudar o tamanho da janela, não pode desaparece-la da taskbar
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.ShowInTaskbar = false;
                }
            }
            first = true;
            base.OnResize(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            PararServicos(false);

            /*
                * Excluir os arquivos de ".lock"
                *
                * 05/06/2013
                * Marcelo
                */
            Empresas.ClearLockFiles(false);

            foreach (var uc in this.Controls)
            {
                if (uc is MetroFramework.Controls.MetroUserControl)
                    (uc as MetroFramework.Controls.MetroUserControl).Dispose();
            }
            base.OnClosed(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this._formloaded)
                this.SaveForm();

            //
            // TODO: Aqui, deveriamos verificar se ainda existe alguma Thread pendente antes de fechar
            //
            if (e.CloseReason == CloseReason.UserClosing && !Propriedade.EncerrarApp)
            {
                // se o botão de fechar for pressionado pelo usuário, o mainform não será fechado em sim minimizado.
                e.Cancel = true;
                ///
                /// verifica se o processo de manutencao de empresas está ativo
                ///
                foreach (var uc in this.Controls)
                {
                    if (uc.GetType().Equals(typeof(userConfiguracoes)))
                    {
                        if (!((userConfiguracoes)uc).VerificaSeAbandona())
                            return;

                        break;
                    }
                }
                // hide this and metro owner
                Form form = this;
                do
                {
                    form.Hide();
                } while ((form = form.Owner) != null);

                this.notifyIcon1.Visible = true;
            }
            else
            {
                e.Cancel = false;  //se o PC for desligado o windows o fecha automaticamente.
            }
            base.OnFormClosing(e);
        }

        private void Form_Main_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control != null && !Propriedade.EncerrarApp)
            {
                if (e.Control.GetType().Equals(typeof(userConfiguracoes)))
                {
                    RestartServicos();
                }

                if (e.Control.GetType().Equals(typeof(Formularios.userMunicipios)))
                {
                    Refresh();
                    WebServiceProxy.reloadWebServicesList();
                }
            }
        }

        #region Métodos gerais

        private void SaveForm()
        {
            uninfeDummy.xmlParams.SaveForm(this, "\\main");
            uninfeDummy.xmlParams.Save();
        }

        public void updateControleDoServico()
        {
            if (servicoInstaladoErodando)
            {
                this.tbPararServico.Enabled = ServiceProcess.StatusService(srvName) == System.ServiceProcess.ServiceControllerStatus.Running;
                this.tbRestartServico.Enabled = ServiceProcess.StatusService(srvName) == System.ServiceProcess.ServiceControllerStatus.Stopped;
            }
        }

        #region ExecutaServicos()

        /// <summary>
        /// Metodo responsável por iniciar os serviços do UniNFe em threads diferentes
        /// </summary>
        public void ExecutaServicos(bool updateOptions = true)
        {
            if (servicoInstaladoErodando)
            {
                Empresas.CarregaConfiguracao();

                switch (ServiceProcess.StatusService(srvName))
                {
                    case System.ServiceProcess.ServiceControllerStatus.Stopped:
                        ServiceProcess.StartService(srvName, 40000);
                        break;

                    case System.ServiceProcess.ServiceControllerStatus.Paused:
                        ServiceProcess.RestartService(srvName, 40000);
                        break;
                }
                if (updateOptions)
                    this.updateControleDoServico();
            }
            else
            {
                ThreadService.Start();
            }
        }

        #endregion ExecutaServicos()

        #region PararServicos()

        public void PararServicos(bool fechaServico)
        {
            if (servicoInstaladoErodando)
            {
                if (fechaServico)
                {
                    ServiceProcess.StopService(srvName, 40000);
                }
            }
            else
            {
                ThreadService.Stop();
            }
        }

        #endregion PararServicos()

        #endregion Métodos gerais

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this._formloaded)
            {
                this._maximized = false;
                try
                {
                    if (uninfeDummy.xmlParams.ValueExists(this.Name, "WindowState"))
                    {
                        switch (uninfeDummy.xmlParams.ReadValue(this.Name + "\\main", "WindowState", 0))
                        {
                            case 2:
                                this._maximized = true;
                                break;
                        }
                    }
                }
                catch { }
            }

            BringToFront();
            Show();

            //Voltar a janela em seu estado normal
            if (this._maximized)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                if (!this._formloaded)
                    try
                    {
                        uninfeDummy.xmlParams.LoadForm(this, "\\main", true);
                    }
                    catch { }
            }
            // Faz a aplicação aparecer na barra de tarefas.
            this.ShowInTaskbar = true;
            this._formloaded = true;
            this._maximized = false;

            Activate();
        }

        private void cmAbrir_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, null);
        }

        private void tbPararServico_Click(object sender, EventArgs e)
        {
            uninfeDummy.opServicos = uninfeOpcoes2.opStopServico;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
        }

        private void tbRestartServico_Click(object sender, EventArgs e)
        {
            uninfeDummy.opServicos = uninfeOpcoes2.opRestartServico;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
        }

        private bool MainVisible
        {
            get
            {
                return this.WindowState != FormWindowState.Minimized && this.Visible;
            }
        }

        private void cmConfiguracoes_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
            {
                if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig) && uninfeDummy.TempoExpirou())
                {
                    if (!FormSenha.SolicitaSenha(false))
                        return;

                    uninfeDummy.UltimoAcessoConfiguracao = DateTime.Now;
                }
                FormDummy.ShowModulo(uninfeOpcoes.opConfiguracoes);
            }
            else
                this._menu.Show(uninfeOpcoes.opConfiguracoes);
        }

        private void cmSituacaoServicos_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
                FormDummy.ShowModulo(uninfeOpcoes.opServicos);
            else
                this._menu.Show(uninfeOpcoes.opServicos);
        }

        private void cmConsultaCadastro_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
                FormDummy.ShowModulo(uninfeOpcoes.opCadastro);
            else
                this._menu.Show(uninfeOpcoes.opCadastro);
        }

        private void cmDANFE_Click(object sender, EventArgs e)
        {
            if (MainVisible)
                this._menu.Show(uninfeOpcoes.opDanfe);
        }

        private void cmLogs_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
                FormDummy.ShowModulo(uninfeOpcoes.opLogs);
            else
                this._menu.Show(uninfeOpcoes.opLogs);
        }

        private void cmMunicipios_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
                FormDummy.ShowModulo(uninfeOpcoes.opMunicipios);
            else
                this._menu.Show(uninfeOpcoes.opMunicipios);
        }

        private void cmValidarXML_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
                FormDummy.ShowModulo(uninfeOpcoes.opValidarXML);
            else
                this._menu.Show(uninfeOpcoes.opValidarXML);
        }

        private void cmManual_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Propriedade.UrlManualUniNFe);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmSobre_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
                FormDummy.ShowModulo(uninfeOpcoes.opSobre);
            else
                this._menu.Show(uninfeOpcoes.opSobre);
        }

        private void cmFechar_Click(object sender, EventArgs e)
        {
            if (ConfiguracaoApp.ConfirmaSaida)
            {
                if (MetroFramework.MetroMessageBox.Show(this,
                            "Confirma o encerramento do " + NFe.Components.Propriedade.NomeAplicacao + "?",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Propriedade.EncerrarApp = true;
                    this.notifyIcon1.Visible = false;
                    this.Close();
                }
            }
            else
            {
                Propriedade.EncerrarApp = true;
                this.notifyIcon1.Visible = false;
                this.Close();
            }
        }

        public void updateVisual()
        {
            var Components = this.Controls.Cast<object>()
                .Where(obj => !ReferenceEquals(obj, this._menu))
                .OfType<MetroFramework.Controls.MetroUserControl>();
            foreach (var c in Components)
            {
                uninfeDummy.ClearControls(c, false, false);
                ((UserControl1)c).UpdateControles();
            }

            if (this._menu != null)
            {
                _menu.UpdateControles();
            }
            this.Refresh();
        }

        public void _RestartServicos()
        {
            this.PararServicos(true);
            this.ExecutaServicos();
        }

        public void RestartServicos()
        {
            if (Empresas.Configuracoes.Count == 0)
            {
                MetroFramework.MetroMessageBox.Show(this,
                    "É necessário cadastrar e configurar a(s) empresa(s) que será(ão) gerenciada(s) pelo aplicativo.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            uninfeDummy.opServicos = uninfeOpcoes2.opRestartTasks;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
        }

        private void metroContextMenu1_Opening(object sender, CancelEventArgs e)
        {
            this.cmValidarXML.Enabled = (NFe.Settings.Empresas.Configuracoes.Count > 0);

            //            this.cmDANFE.Visible =
            this.cmSituacaoServicos.Visible =
            this.cmConsultaCadastro.Visible = (NFe.Settings.Empresas.CountEmpresasNFe > 0);

            this.cmMunicipios.Visible = Empresas.CountEmpresasNFse > 0;
        }
    }
}
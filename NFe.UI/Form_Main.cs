using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;

using NFe.Settings;
using NFe.Components;
using NFe.Threadings;

namespace NFe.UI
{
    public partial class Form_Main : MetroForm
    {
        //For animated panels direction
        string optionsDirection = "down";

        //For animated panels timeout
        int optionsTimeOut = 0;

        //For animated panels position
        int optionsX;
        int optionsY;

        Timer tm = new Timer();
        
        private bool first = false;
        private bool restartServico = false;
        private bool servicoInstaladoErodando = false;
        private string srvName = Propriedade.ServiceName[Propriedade.TipoAplicativo == NFe.Components.TipoAplicativo.Nfe ? 0 : 1];
        private menu _menu;
        private bool _maximized;

        public Form_Main()
        {
            InitializeComponent();

            uninfeDummy.mainForm = this;
            uninfeDummy.UltimoAcessoConfiguracao = DateTime.MinValue;
#if false
            try
            {
                // Executar as conversões de atualizações de versão quando tiver
                string nomeEmpresa = Auxiliar.ConversaoNovaVersao(string.Empty);
                if (!string.IsNullOrEmpty(nomeEmpresa))
                {
                    /// danasa 20-9-2010
                    /// exibe a mensagem de erro
                    Dialogs.ShowMessage("Não foi possível localizar o CNPJ da empresa no certificado configurado", 0, 0, MessageBoxIcon.Error);

                    /// e pede o CNPJ
                    FormCNPJ fcnpj = new FormCNPJ(nomeEmpresa);
                    if (fcnpj.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        /// tenta processar já com o CNPJ definido
                        Auxiliar.ConversaoNovaVersao(fcnpj.Cnpj);
                        restartServico = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Dialogs.ShowMessage(ex.Message, 600, 200, MessageBoxIcon.Error);
            }
#endif
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    uninfeDummy.xmlParams.LoadForm(this, "\\main");
                    this._maximized = this.WindowState == FormWindowState.Maximized;
                }
                catch { }

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
                this.MinimumSize = new Size(800, 600);
                this.MaximumSize = new Size(1000, 800);
                //Trazer minimizado e no systray
                this.notifyIcon1.BalloonTipText = string.Format("Para abrir novamente o {0}, de um duplo clique ou pressione o botão direito do mouse sobre o ícone.", NFe.Components.Propriedade.NomeAplicacao);
                this.notifyIcon1.BalloonTipTitle =
                    this.notifyIcon1.Text = NFe.Components.Propriedade.NomeAplicacao + " - " + NFe.Components.Propriedade.DescricaoAplicacao;
                this.notifyIcon1.Visible = true;
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.ShowInTaskbar = false;
                this.notifyIcon1.ShowBalloonTip(6000);

                this.uTheme = (MetroFramework.MetroThemeStyle)Enum.Parse(typeof(MetroFramework.MetroThemeStyle), uninfeDummy.xmlParams.ReadValue(this.Name, "Theme", this.metroStyleManager1.Theme.ToString()), false);
                this.uStyle = (MetroFramework.MetroColorStyle)Enum.Parse(typeof(MetroFramework.MetroColorStyle), uninfeDummy.xmlParams.ReadValue(this.Name, "Style", this.metroStyleManager1.Style.ToString()), false);

                _menu = new menu();
                this.Controls.Add(_menu);
                _menu.Dock = DockStyle.Fill;

                switch (NFe.Components.Propriedade.TipoAplicativo)
                {
                    case NFe.Components.TipoAplicativo.Nfe:
                        this.notifyIcon1.Icon = this.Icon = NFe.UI.Properties.Resources.uninfe_icon;
                        break;

                    case NFe.Components.TipoAplicativo.Nfse:
                        this.notifyIcon1.Icon = this.Icon = NFe.UI.Properties.Resources.uninfse_icon;
                        break;
                }

                this.cmAbrir.Text = "Abrir " + NFe.Components.Propriedade.NomeAplicacao;
                this.cmFechar.Text = "Fechar " + NFe.Components.Propriedade.NomeAplicacao;
                this.cmSobre.Text = "Sobre o " + NFe.Components.Propriedade.NomeAplicacao;
                this.cmManual.Text = "Manual do " + NFe.Components.Propriedade.NomeAplicacao;
                this.cmManual.Enabled = File.Exists(Path.Combine(NFe.Components.Propriedade.PastaExecutavel, NFe.Components.Propriedade.NomeAplicacao + ".pdf"));

                ConfiguracaoApp.StartVersoes();

                if (TipoAplicativo.Nfse == NFe.Components.Propriedade.TipoAplicativo)
                {
                    if (!System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios) || 
                        !System.IO.File.Exists(Propriedade.NomeArqXMLWebService))
                    {
                        MetroFramework.MetroMessageBox.Show(this, 
                            "Arquivos '" + Propriedade.NomeArqXMLMunicipios + "' e/ou '" + Propriedade.NomeArqXMLWebService + "' não encontrados", "", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
                if (!this.servicoInstaladoErodando)     //danasa 12/8/2011
                    //Definir eventos de controles de execução das thread´s de serviços do UniNFe. Wandrey 26/07/2011
                    new ThreadControlEvents();  //danasa 12/8/2011

                //Executar os serviços do UniNFe em novas threads
                //Tem que ser carregado depois que o formulário da MainForm estiver totalmente carregado para evitar Erros. Wandrey 19/10/2010
                this.ExecutaServicos();

                tm.Tick += delegate
                {
                    if (optionsTimeOut < 1000)
                    {
                        optionsTimeOut++;
                    }
                    if (optionsTimeOut == 1000)
                    {
                        if (optionsDirection == "up")
                        {
                            optionsDirection = "down";
                        }
                    }
                    if (optionsDirection == "up")
                    {
                        if (optionsY > Height - pnlOptions.Height + 3)
                        {
                            optionsY -= 3;
                            pnlOptions.Location = new Point(optionsX, optionsY);
                        }
                    }
                    else
                    {
                        if (optionsY < Height)
                        {
                            optionsY += 3;
                        }
                        pnlOptions.Location = new Point(optionsX, optionsY);
                    }
                };
                tm.Interval = 1;
            }
            finally
            {
                this.updateControleDoServico();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            int x = Width - 50;
            optionsX = 0;
            optionsY = Height + pnlOptions.Height - 25;
            pnlOptions.Size = new Size(x, pnlOptions.Height);
            pnlOptions.Location = new Point(optionsX, optionsY);
            
            if (first)
            {
                //Faz a aplicação sumir da barra de tarefas
                //danasa
                //  Se usuario mudar o tamanho da janela, não pode desaparece-la da taskbar
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.ShowInTaskbar = false;

                    //Mostrar o balão com as informações que selecionamos
                    //O parâmetro passado refere-se ao tempo (ms)
                    // em que ficará aparecendo. Coloque "0" se quiser
                    // que ele feche somente quando o usuário clicar

                    notifyIcon1.ShowBalloonTip(6000);

                    tm.Stop();
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

            this.SaveForm();

            foreach (var uc in this.Controls)
            {
                if (uc is MetroFramework.Controls.MetroUserControl)
                    (uc as MetroFramework.Controls.MetroUserControl).Dispose();
            }
            base.OnClosed(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
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

                        //this.RestartServicos();
                        break;
                    }
                }
                // hide this and metro owner
                Form form = this;
                do
                {
                    form.Hide();
                } while ((form = form.Owner) != null);
                //this.Visible = false;
                //this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                this.notifyIcon1.ShowBalloonTip(6000);
            }
            else
            {
                e.Cancel = false;  //se o PC for desligado o windows o fecha automaticamente.
            }

            tm.Stop();

            base.OnFormClosing(e);
        }

        private void Form_Main_ControlAdded(object sender, ControlEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                tm.Start();

            ///
            /// fecha o painel de preferencias
            optionsTimeOut = 1000;
            optionsDirection = "up";
        }

        private void Form_Main_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control != null && !Propriedade.EncerrarApp)
            {
                if (e.Control.GetType().Equals(typeof(userConfiguracoes)))
                {
                    this.RestartServicos();
                }

                if (e.Control.GetType().Equals(typeof(Formularios.userMunicipios)))
                {
                    ///
                    /// reloadWebServicesList carrega as URL's com base no XML de municipios.
                    if (WebServiceProxy.reloadWebServicesList())
                        ConfiguracaoApp.loadResouces();
                }
            }
        }

        #region Métodos gerais

        public MetroFramework.MetroColorStyle uStyle
        {
            get { return this.metroStyleManager1.Style; }
            set
            {
                if (!value.Equals(this.metroStyleManager1.Style))
                {
                    this.metroStyleManager1.Style = value;
                    updateSettings();
                }
                this.Style = value;
            }
        }

        public MetroFramework.MetroThemeStyle uTheme
        {
            get { return this.metroStyleManager1.Theme; }
            set
            {
                if (!value.Equals(this.metroStyleManager1.Theme))
                {
                    this.metroStyleManager1.Theme = value;
                    updateSettings();
                }
                metroTile_back.Text = value == MetroThemeStyle.Dark ? "Branco" : "Preto";
                this.Theme = value;
            }
        }

        void updateSettings()
        {
            uninfeDummy.xmlParams.WriteValue(this.Name, "Theme", this.metroStyleManager1.Theme.ToString());
            uninfeDummy.xmlParams.WriteValue(this.Name, "Style", this.metroStyleManager1.Style.ToString());
            uninfeDummy.xmlParams.Save();
        }

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
        public void ExecutaServicos()
        {
            if (servicoInstaladoErodando)
            {
                Empresas.CarregaConfiguracao();

                if (restartServico)
                    ServiceProcess.StopService(srvName, 40000);

                restartServico = false;

                switch (ServiceProcess.StatusService(srvName))
                {
                    case System.ServiceProcess.ServiceControllerStatus.Stopped:
                        ServiceProcess.StartService(srvName, 40000);
                        break;
                    case System.ServiceProcess.ServiceControllerStatus.Paused:
                        ServiceProcess.RestartService(srvName, 40000);
                        break;
                }
                this.updateControleDoServico();
            }
            else
            {
                ThreadService.Start();
            }
        }
        #endregion

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
        #endregion

        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BringToFront();
            Show();
            //Voltar a janela em seu estado normal
            if (this._maximized)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;

            this._maximized = false;

            // Faz a aplicação aparecer na barra de tarefas.            
            this.ShowInTaskbar = true;

            Activate();
            tm.Start();
            return;







            //Voltar a janela em seu estado normal
            if (this._maximized)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;

            this._maximized = false;

            // Faz a aplicação aparecer na barra de tarefas.            
            this.ShowInTaskbar = true;

            // Levando o Form de volta para a tela.
            this.Visible = true;

            tm.Start();
        }

        private void cmAbrir_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, null);
        }

        private void tbPararServico_Click(object sender, EventArgs e)
        {
            uninfeDummy.opServicos = uninfeOpcoes2.opStopServico;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
            /*
            using (var ff = new NFe.UI.Formularios.FormWait())
            {
                ff.servicename = this.srvName;
                ff.stopservice = true;
                ff.mensagem = "Parando o serviço do " + NFe.Components.Propriedade.NomeAplicacao;
                if (ff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    MetroFramework.MetroMessageBox.Show(this, 
                            "Serviço do " + NFe.Components.Propriedade.NomeAplicacao + " parado com sucesso!", "", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
            //this.updateControleDoServico();
        }

        private void tbRestartServico_Click(object sender, EventArgs e)
        {
            uninfeDummy.opServicos = uninfeOpcoes2.opRestartTasks;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
            /*
            using (var ff = new NFe.UI.Formularios.FormWait())
            {
                ff.servicename = this.srvName;
                ff.stopservice = false;
                ff.mensagem = "Reiniciando o serviço do " + NFe.Components.Propriedade.NomeAplicacao;
                if (ff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    MetroFramework.MetroMessageBox.Show(this, 
                        "Serviço do " + NFe.Components.Propriedade.NomeAplicacao + " reiniciado com sucesso!", "", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
            //this.updateControleDoServico();
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
                    if (!FormSenha.SolicitaSenha())
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
            if (!MainVisible)
                uninfeDummy.printDanfe();
            else
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
                NFe.Components.Functions.ExibeDocumentacao();
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (MetroFramework.MetroMessageBox.Show(this,
                        "Confirma o encerramento do " + NFe.Components.Propriedade.NomeAplicacao + "?", 
                        "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Propriedade.EncerrarApp = true;
                this.notifyIcon1.Visible = false;
                this.Close();
            }
        }

        private void Form_Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y >= Height - 15)// && e.X < (Width - pnlRightOptions.Width))
            {
                optionsDirection = "up";
                optionsTimeOut = 0;
            }
            if (e.X >= Width - 15)
            {
                optionsDirection = "down";
            }
            //if (e.X < (Width - pnlRightOptions.Width))
            {
                //rightDirection = "Left";
            }
        }

        void updateVisual()
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

        private void metroTile_back_Click(object sender, EventArgs e)
        {
            this.uTheme = (this.uTheme == MetroThemeStyle.Light) ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
            updateVisual();
        }

        private void metroTileRed_Click(object sender, EventArgs e)
        {
            if (!uStyle.Equals(((MetroFramework.Controls.MetroTile)sender).Style))
            {
                uStyle = ((MetroFramework.Controls.MetroTile)sender).Style;
                //Console.WriteLine(".............................." + uStyle.ToString() + "..." + ((MetroFramework.Controls.MetroTile)sender).Name);
                updateVisual();
            }
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
            /*
            using (var ff = new NFe.UI.Formularios.FormWait())
            {
                ff.acao1 = PararServicos;
                ff.acao2 = ExecutaServicos;
                ff.mensagem = "Parando os serviços";
//                ff.ShowDialog();
            }
            */

            if (!MainVisible)
                notifyIcon1.ShowBalloonTip(6000);
        }

        private void metroContextMenu1_Opening(object sender, CancelEventArgs e)
        {
            switch (NFe.Components.Propriedade.TipoAplicativo)
            {
                case NFe.Components.TipoAplicativo.Nfe:
                    this.cmMunicipios.Visible = false;
                    break;

                case NFe.Components.TipoAplicativo.Nfse:
                    this.cmConsultaCadastro.Visible =
                        this.cmSituacaoServicos.Visible =
                        this.cmDANFE.Visible = false;
                    break;
            }
            this.cmDANFE.Enabled =
                this.cmSituacaoServicos.Enabled =
                this.cmValidarXML.Enabled =
                this.cmConsultaCadastro.Enabled = (NFe.Settings.Empresas.Configuracoes != null && NFe.Settings.Empresas.Configuracoes.Count > 0);
        }
    }
}

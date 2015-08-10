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

using MetroFramework.Components;
using MetroFramework.Forms;

using NFe.Settings;
using NFe.Components;
using NFe.Interface;
using NFe.Threadings;

namespace uninfe2
{
    public partial class Form1 : MetroForm
    {
        private bool first = false;
        private bool restartServico = false;
        private bool servicoInstaladoErodando = false;
        private string srvName = Propriedade.ServiceName[Propriedade.TipoAplicativo == NFe.Components.TipoAplicativo.Nfe ? 0 : 1];

        public Form1()
        {
            InitializeComponent();

            uninfeDummy.mainForm = this;
            uninfeDummy.UltimoAcessoConfiguracao = DateTime.MinValue;

            try
            {
                // Executar as conversões de atualizações de versão quando tiver
                string nomeEmpresa = Auxiliar.ConversaoNovaVersao(string.Empty);
                if (!string.IsNullOrEmpty(nomeEmpresa))
                {
                    /// danasa 20-9-2010
                    /// exibe a mensagem de erro
                    Dialogs.ShowMessage("Não foi possível localizar o CNPJ da empresa no certificado configurado");

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
                Dialogs.ShowMessage(ex.Message);
            }
            try
            {
                XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
                iniFile.LoadForm(this, "main");
            }
            catch { }   // para evitar que para alguns que derrubam o uninfe quando atualizam

        }

        private menu _menu;

        public string uStyle
        {
            get { return this.metroStyleManager1.Style; }
            set {
                var old = this.metroStyleManager1.Style;
                this.metroStyleManager1.Style = value;
                if (!old.Equals(value))
                    updateSettings();
            }
        }
        public string uTheme
        {
            get { return this.metroStyleManager1.Theme; }
            set {
                var old = this.metroStyleManager1.Theme;
                this.metroStyleManager1.Theme = value;
                if (!old.Equals(value))
                    updateSettings();
            }
        }

        void updateSettings()
        {
            NFe.Components.XMLIniFile xml = new NFe.Components.XMLIniFile(NFe.Components.Propriedade.NomeArqXMLParams);
            xml.WriteValue(this.Name, "Theme", this.metroStyleManager1.Theme);
            xml.WriteValue(this.Name, "Style", this.metroStyleManager1.Style);
            xml.Save();
        }

        private void SaveForm()
        {
            XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
            iniFile.SaveForm(this, "main");
            iniFile.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
            //SERVICO: danasa 7/2011
            //servico está instalado e rodando?
            this.servicoInstaladoErodando = Propriedade.ServicoRodando;

            this.tbSeparator1.Visible =
                this.tbRestartServico.Visible =
                this.tbPararServico.Visible = this.servicoInstaladoErodando;

            this.updateControleDoServico();

            ///
            /// danasa 9-2009
            ///
            this.MinimumSize = new Size(800, 600);
            //Trazer minimizado e no systray
            this.notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.notifyIcon1.ShowBalloonTip(6000);

            //Propriedade.TipoAplicativo = TipoAplicativo.Nfe;
            //ConfiguracaoApp.AtualizaWSDL = !System.IO.File.Exists(Propriedade.NomeArqXMLWebService); //danasa: 12/2013
            ConfiguracaoApp.StartVersoes();

            if (!this.servicoInstaladoErodando)     //danasa 12/8/2011
                //Definir eventos de controles de execução das thread´s de serviços do UniNFe. Wandrey 26/07/2011
                new ThreadControlEvents();  //danasa 12/8/2011

            //Executar os serviços do UniNFe em novas threads
            //Tem que ser carregado depois que o formulário da MainForm estiver totalmente carregado para evitar Erros. Wandrey 19/10/2010
            this.ExecutaServicos();


            NFe.Components.XMLIniFile xml = new NFe.Components.XMLIniFile(NFe.Components.Propriedade.NomeArqXMLParams);
            this.metroStyleManager1.Theme = xml.ReadValue(this.Name, "Theme", this.metroStyleManager1.Theme);
            this.metroStyleManager1.Style = xml.ReadValue(this.Name, "Style", this.metroStyleManager1.Style);

            switch (NFe.Components.Propriedade.TipoAplicativo)
            {
                case NFe.Components.TipoAplicativo.Nfe:
                    this.notifyIcon1.Icon =
                        this.Icon = Properties.Resources.uninfe1;
                    break;

                case NFe.Components.TipoAplicativo.Nfse:
                    this.notifyIcon1.Icon =
                        this.Icon = Properties.Resources.uninfse;
                    break;
            }
            this.notifyIcon1.BalloonTipText = string.Format(this.notifyIcon1.BalloonTipText, NFe.Components.Propriedade.NomeAplicacao);
            this.notifyIcon1.BalloonTipTitle =
                this.notifyIcon1.Text = NFe.Components.Propriedade.NomeAplicacao + " - " + NFe.Components.Propriedade.DescricaoAplicacao;

            EnableControlsCM();

            this.cmConsultaCadastro.Visible =
                this.cmDANFE.Visible = NFe.Components.Propriedade.TipoAplicativo == TipoAplicativo.Nfe;

            this.cmAbrir.Text = "Abrir " + NFe.Components.Propriedade.NomeAplicacao;
            this.cmFechar.Text = "Fechar " + NFe.Components.Propriedade.NomeAplicacao;
            this.cmSobre.Text = "Sobre o " + NFe.Components.Propriedade.NomeAplicacao;
            this.cmManual.Text = "Manual do " + NFe.Components.Propriedade.NomeAplicacao;

            _menu = new menu();
            this.Controls.Add(_menu);
            _menu.Dock = DockStyle.Fill;


            /*
            foreach (var item in MetroStyleManager.Styles.Themes)
            {
                Console.WriteLine(item.Key.ToString());
            }*/
        }

        public void EnableControlsCM()
        {
            this.cmDANFE.Enabled =
                this.cmSituacaoServicos.Enabled =
                this.cmValidarXML.Enabled =
                this.cmConsultaCadastro.Enabled = (NFe.Settings.Empresa.Configuracoes != null && NFe.Settings.Empresa.Configuracoes.Count > 0);
        }

        protected override void OnResize(EventArgs e)
        {
            if (first)
            {
                ///
                /// danasa 9-2009
                /// 
                if (this.WindowState != FormWindowState.Minimized)
                {
                    this.SaveForm();
                }
                //Faz a aplicação sumir da barra de tarefas
                //danasa
                //  Se usuario mudar o tamanho da janela, não pode desaparece-la da taskbar
                if (this.WindowState == FormWindowState.Minimized)
                    this.ShowInTaskbar = false;

                //Mostrar o balão com as informações que selecionamos
                //O parâmetro passado refere-se ao tempo (ms)
                // em que ficará aparecendo. Coloque "0" se quiser
                // que ele feche somente quando o usuário clicar

                if (this.WindowState == FormWindowState.Minimized)
                {
                    notifyIcon1.ShowBalloonTip(6000);
                }
            }
            first = true;
            base.OnResize(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_menu != null)
            {
                PararServicos(false);

                #region Ticket #110
                /* 
                 * Excluir os arquivos de ".lock"
                 * 
                 * 05/06/2013
                 * Marcelo
                 */
                Empresa.ClearLockFiles(false);
                #endregion

                this.SaveForm();

                foreach (var uc in this.Controls)
                {
                    if (uc is MetroFramework.Controls.MetroUserControl)
                        (uc as MetroFramework.Controls.MetroUserControl).Dispose();
                }
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
                ///
                /// danasa 9-2009
                /// 
                if (this.WindowState != FormWindowState.Minimized)
                {
                    SaveForm();
                }
                // se o botão de fechar for pressionado pelo usuário, o mainform não será fechado em sim minimizado.
                e.Cancel = true;
                this.Visible = false;
                //this.OnResize(e);
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                this.notifyIcon1.ShowBalloonTip(6000);
            }
            else
            {
                e.Cancel = false;  //se o PC for desligado o windows o fecha automaticamente.
            } 
            base.OnFormClosing(e);
        }

        #region Métodos gerais

        private void updateControleDoServico()
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
        private void ExecutaServicos()
        {
            Empresa.CarregaConfiguracao();

            if (servicoInstaladoErodando)
            {
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
        private void PararServicos(bool fechaServico)
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

        private void cmFechar_Click(object sender, EventArgs e)
        {
            Propriedade.EncerrarApp = true;

            this.notifyIcon1.Visible = false;
            this.Close();
        }

        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Voltar a janela em seu estado normal
            this.WindowState = FormWindowState.Normal;

            // Faz a aplicação aparecer na barra de tarefas.            
            this.ShowInTaskbar = true;

            // Levando o Form de volta para a tela.
            this.Visible = true;
        }

        private void cmAbrir_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, null);
        }

        private void tbPararServico_Click(object sender, EventArgs e)
        {
            FormWait fw = new FormWait();
            fw.Show();
            try
            {
                fw.DisplayMessage("Parando o serviço do UniNFe");
                ServiceProcess.StopService(srvName, 40000);
                this.updateControleDoServico();
                fw.StopMarquee();
                MessageBox.Show("Serviço do UniNFe parado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fw.Dispose();
            }            
        }

        private void tbRestartServico_Click(object sender, EventArgs e)
        {
            FormWait fw = new FormWait();
            fw.Show();
            try
            {
                fw.DisplayMessage("Reiniciando o serviço do UniNFe");
                ServiceProcess.RestartService(srvName, 40000);
                this.updateControleDoServico();
                fw.StopMarquee();
                MessageBox.Show("Serviço do UniNFe reiniciado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fw.Dispose();
            }
        }

        private void cmConfiguracoes_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig) && uninfeDummy.TempoExpirou())
                {
                    if (!uninfe2.Formularios.FormSenha.SolicitaSenha())
                        return;

                    uninfeDummy.UltimoAcessoConfiguracao = DateTime.Now;
                }
                using (uninfe2.Formularios.FormDummy f = new Formularios.FormDummy())
                {
                    f.opcao = uninfeOpcoes.opConfiguracoes;
                    f.ShowDialog();
                }
            }
            else
                this._menu.Show(uninfeOpcoes.opConfiguracoes);
        }

        private void cmSituacaoServicos_Click(object sender, EventArgs e)
        {

        }

        private void cmConsultaCadastro_Click(object sender, EventArgs e)
        {

        }

        private void cmDANFE_Click(object sender, EventArgs e)
        {

        }

        private void cmLogs_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                using (uninfe2.Formularios.FormDummy f = new Formularios.FormDummy())
                {
                    f.opcao = uninfeOpcoes.opLogs;
                    f.ShowDialog();
                }
            }
            else
                this._menu.Show(uninfeOpcoes.opLogs);        
        }

        private void cmValidarXML_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                using (uninfe2.Formularios.FormDummy f = new Formularios.FormDummy())
                {
                    f.opcao = uninfeOpcoes.opValidarXML;
                    f.ShowDialog();
                }
            }
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
                Dialogs.ShowMessage(ex.Message);
            }
        }

        private void cmSobre_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                using (uninfe2.Formularios.FormDummy f = new Formularios.FormDummy())
                {
                    f.opcao = uninfeOpcoes.opSobre;
                    f.ShowDialog();
                }
            }
            else
                this._menu.Show(uninfeOpcoes.opSobre);
        }

    }
}

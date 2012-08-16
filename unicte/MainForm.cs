using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NFe.Settings;
using NFe.Components;
using NFe.Interface;
using NFe.Threadings;

namespace unicte
{
    #region Classe MainForm
    public partial class MainForm : Form
    {
        private bool restartServico = false;
        private bool servicoInstaladoErodando = false;

        #region MainForm()
        public MainForm()
        {
            InitializeComponent();

            try
            {
                // Executar as conversões de atualizações de versão quando tiver
                string nomeEmpresa = Auxiliar.ConversaoNovaVersao(string.Empty);
                if (!string.IsNullOrEmpty(nomeEmpresa))
                {
                    /// danasa 20-9-2010
                    /// exibe a mensagem de erro
                    MessageBox.Show("Não foi possível localizar o CNPJ da empresa no certificado configurado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            try
            {
                XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
                iniFile.LoadForm(this, "");
            }
            catch { }   // para evitar que para alguns que derrubam o uninfe quando atualizam

            //Trazer minimizado e no systray
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notifyIcon1.ShowBalloonTip(6000);

            this.MinimumSize = new Size(750, 600);

            #region Definir valores propriedades de configuração
            Propriedade.TipoAplicativo = TipoAplicativo.Cte;
            ConfiguracaoApp.StartVersoes();
            SchemaXMLCte.CriarListaIDXML();
            #endregion

            if (!this.servicoInstaladoErodando)     //danasa 12/8/2011
                //Definir eventos de controles de execução das thread´s de serviços do UniNFe. Wandrey 26/07/2011
                new ThreadControlEvents();  //danasa 12/8/2011
        }
        #endregion

        #region Métodos gerais

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
                    ServiceProcess.StopService(Propriedade.ServiceName, 40000);

                restartServico = false;

                switch (ServiceProcess.StatusService(Propriedade.ServiceName))
                {
                    case System.ServiceProcess.ServiceControllerStatus.Stopped:
                        ServiceProcess.StartService(Propriedade.ServiceName, 40000);
                        break;
                    case System.ServiceProcess.ServiceControllerStatus.Paused:
                        ServiceProcess.RestartService(Propriedade.ServiceName, 40000);
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
                    ServiceProcess.StopService(Propriedade.ServiceName, 40000);
                }
            }
            else
            {
                ThreadService.Stop();
            }
        }
        #endregion

        #endregion

        #region Métodos de eventos

        #region MainForm_Resize()

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ///
            /// danasa 9-2009
            /// 
            if (this.WindowState != FormWindowState.Minimized)
            {
                XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
                iniFile.SaveForm(this, "");
                iniFile.Save();
            }
            //Faz a aplicação sumir da barra de tarefas
            //danasa
            //  Se usuario mudar o tamanho da janela, não pode desaparece-la da tasknar
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
            //Ativar o ícone na área de notificação
            //para isso a propriedade Visible deveria ser setada
            //como false, mas prefiro deixar o ícone lá.
            //notifyIcon1.Visible = true;
        }
        #endregion

        #region -- Show desktop
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.notifyIcon1_MouseDoubleClick(sender, null);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Voltar a janela em seu estado normal
            this.WindowState = FormWindowState.Normal;

            // Faz a aplicação aparecer na barra de tarefas.            
            this.ShowInTaskbar = true;

            // Levando o Form de volta para a tela.
            this.Visible = true;

            // Faz desaparecer o ícone na área de notificação,
            // para isso a propriedade Visible deveria ser setada 
            // como true no evento Resize do Form.

            // notifyIcon1.Visible=false;
        }
        #endregion

        #region MainForm_FormClosed()

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PararServicos(false);
        }
        #endregion

        #region MainForm_FormClosing

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
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
                    XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
                    iniFile.SaveForm(this, "");
                    iniFile.Save();
                }
                // se o botão de fechar for pressionado pelo usuário, o mainform não será fechado em sim minimizado.
                e.Cancel = true;
                this.Visible = false;
                this.OnResize(e);
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.ShowBalloonTip(6000);
            }
            else
            {
                e.Cancel = false;  //se o PC for desligado o windows o fecha automaticamente.
            }
        }
        #endregion

        #region -- Sobre o UniNFe
        private void toolStripButton_sobre_Click(object sender, EventArgs e)
        {
            this.toolStripButton_sobre.Enabled =
                this.sobreOUniNFeToolStripMenuItem.Enabled = false;
            using (FormSobre oSobre = new FormSobre())
            {
                oSobre.MinimizeBox =
                    oSobre.ShowInTaskbar = !(sender is ToolStripButton);
                oSobre.ShowDialog();
            }
            this.sobreOUniNFeToolStripMenuItem.Enabled =
                this.toolStripButton_sobre.Enabled = true;
        }
        #endregion

        #region -- Consulta servico e cadastro
        private int CadastroAtivo()
        {
            FormConsultaCadastro oCadastro = null;
            //danasa 
            foreach (Form fg in this.MdiChildren)
            {
                if (fg is FormConsultaCadastro)
                {
                    ///
                    /// configuracão já está ativa como MDI
                    /// 
                    this.notifyIcon1_MouseDoubleClick(null, null);
                    oCadastro = fg as FormConsultaCadastro;
                    oCadastro.WindowState = FormWindowState.Normal;
                    return 1;
                }
            }
            foreach (Form fg in Application.OpenForms)
            {
                if (fg is FormConsultaCadastro)
                {
                    oCadastro = fg as FormConsultaCadastro;
                    oCadastro.WindowState = FormWindowState.Normal;
                    return 0;
                }
            }
            return -1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Empresa.Configuracoes.Count <= 0)
            {
                MessageBox.Show("É necessário cadastrar e configurar as empresas que serão gerenciadas pelo aplicativo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (CadastroAtivo())
            {
                case 0:
                    ///
                    /// configuracao ja existe como Modal
                    /// minimiza o MainForm para que a tela de configuracao esteja visivel
                    /// 
                    this.WindowState = FormWindowState.Minimized;
                    break;

                case -1:
                    {
                        FormConsultaCadastro consultaCadastro = new FormConsultaCadastro();
                        consultaCadastro.MdiParent = this;
                        consultaCadastro.MinimizeBox = false;
                        consultaCadastro.Show();
                    }
                    break;
            }
        }

        private void cmConsultaCadastroServico_Click(object sender, EventArgs e)
        {
            if (Empresa.Configuracoes.Count <= 0)
            {
                MessageBox.Show("É necessário cadastrar e configurar as empresas que serão gerenciadas pelo aplicativo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (CadastroAtivo())
            {
                case -1:
                    ///
                    /// tela principal está visivel?
                    /// 
                    if (this.WindowState != FormWindowState.Minimized)
                        ///
                        /// então abre o cadastro como MDI
                        /// 
                        this.toolStripButton1_Click(sender, e);
                    else
                        using (FormConsultaCadastro consultaCadastro = new FormConsultaCadastro())
                        {
                            consultaCadastro.MinimizeBox = true;
                            consultaCadastro.ShowInTaskbar = true;
                            consultaCadastro.ShowDialog();
                        }
                    break;
            }
            //this.DemonstrarStatusServico();
        }
        #endregion

        #region -- Validar
        private void toolStripButton_validarxml_Click(object sender, EventArgs e)
        {
            if (Empresa.Configuracoes.Count <= 0)
            {
                MessageBox.Show("É necessário cadastrar e configurar as empresas que serão gerenciadas pelo aplicativo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FormValidarXML oValidarXML = new FormValidarXML();
            oValidarXML.MdiParent = this;
            oValidarXML.MinimizeBox = false;
            oValidarXML.Show();
        }

        private void vaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Empresa.Configuracoes.Count <= 0)
            {
                MessageBox.Show("É necessário cadastrar e configurar as empresas que serão gerenciadas pelo aplicativo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FormValidarXML oValidarXML = new FormValidarXML())
            {
                oValidarXML.ShowInTaskbar = true;
                oValidarXML.MinimizeBox = true;
                oValidarXML.ShowDialog();
            }
        }
        #endregion

        #region -- Configuracao
        private int ConfiguracaoAtiva()
        {
            FormConfiguracao oConfig = null;
            //danasa 
            foreach (Form fg in this.MdiChildren)
            {
                if (fg is FormConfiguracao)
                {
                    ///
                    /// configuracão já está ativa como MDI
                    /// 
                    this.notifyIcon1_MouseDoubleClick(null, null);
                    oConfig = fg as FormConfiguracao;
                    oConfig.WindowState = FormWindowState.Normal;
                    return 1;
                }
            }
            foreach (Form fg in Application.OpenForms)
            {
                if (fg is FormConfiguracao)
                {
                    oConfig = fg as FormConfiguracao;
                    oConfig.WindowState = FormWindowState.Normal;
                    return 0;
                }
            }
            return -1;
        }


        ///
        /// danasa 9-2010
        private void onCloseConfiguracao(object sender, EventArgs e)
        {
            /// danasa 20-9-2010
            FormWait fw = new FormWait();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                fw.Show();
                fw.DisplayMessage("Parando os serviços");
                this.PararServicos(true);
                fw.DisplayMessage("Iniciando os serviços");
                this.ExecutaServicos();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                fw.Dispose();
            }
        }

        private void toolStripButton_config_Click(object sender, EventArgs e)
        {
            switch (ConfiguracaoAtiva())
            {
                case 0:
                    ///
                    /// configuracao ja existe como Modal
                    /// minimiza o MainForm para que a tela de configuracao esteja visivel
                    /// 
                    this.WindowState = FormWindowState.Minimized;
                    break;

                case -1:
                    {
                        try
                        {
                            FormConfiguracao oConfig = new FormConfiguracao(onCloseConfiguracao, this);
                            oConfig.MinimizeBox = false;
                            if (oConfig.AcessoAutorizado)
                            {
                                oConfig.Show();
                            }
                        }
                        catch
                        {
                        }
                    }
                    break;
            }
        }

        private void configuraçõesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            switch (ConfiguracaoAtiva())
            {
                case -1:
                    ///
                    /// tela principal está visivel?
                    /// 
                    if (this.WindowState != FormWindowState.Minimized)
                        ///
                        /// então abre a configuração como MDI
                        /// 
                        toolStripButton_config_Click(sender, e);
                    else
                        using (FormConfiguracao oConfig = new FormConfiguracao(onCloseConfiguracao, null))
                        {
                            oConfig.MinimizeBox = true;
                            if (oConfig.AcessoAutorizado)
                            {
                                oConfig.ShowDialog();
                            }
                        }
                    break;
            }
        }
        #endregion

        #region sairToolStripMenuItem_Click

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Propriedade.EncerrarApp = true;

            this.notifyIcon1.Visible = false;

            this.Close();
        }
        #endregion

        #region toolStripMenuItem2_Click

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\" + Propriedade.NomeAplicacao + ".pdf"))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + Propriedade.NomeAplicacao + ".pdf");
                }
                else
                {
                    MessageBox.Show("Não foi possível localizar o arquivo de manual do sistema.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region toolStripBtnUpdate_Click

        private void toolStripBtnUpdate_Click(object sender, EventArgs e)
        {
            FormUpdate FormUp = new FormUpdate("i" + Propriedade.NomeAplicacao.ToLower() + ".exe");
            FormUp.MdiParent = this;
            FormUp.MinimizeBox = false;
            FormUp.Show();
        }
        #endregion

        ///
        /// danasa 9-2010
        /*private void onCloseEmpresas(object sender, EventArgs e)
        {
            /// danasa 20-9-2010
            FormWait fw = new FormWait();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                fw.Show();
                fw.DisplayMessage("Parando os serviços");
                this.PararServicos(true);
                fw.DisplayMessage("Lendo as empresas");
                Empresa.CarregaConfiguracao();
                fw.DisplayMessage("Iniciando os serviços");
                this.ExecutaServicos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fw.Dispose();
                this.Cursor = Cursors.Default;
            }
        }*/

        #region MainForm_Load

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Executar os serviços do UniNFe em novas threads
            //Tem que ser carregado depois que o formulário da MainForm estiver totalmente carregado para evitar Erros. Wandrey 19/10/2010
            this.ExecutaServicos();
        }
        #endregion

        #region SERVICO: danasa 7/2011
        private void tbPararServico_Click(object sender, EventArgs e)
        {
            FormWait fw = new FormWait();
            fw.Show();
            try
            {
                fw.DisplayMessage("Parando o serviço do UniNFe");
                ServiceProcess.StopService(Propriedade.ServiceName, 40000);
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
                ServiceProcess.RestartService(Propriedade.ServiceName, 40000);
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

        private void updateControleDoServico()
        {
            if (servicoInstaladoErodando)
            {
                this.tbPararServico.Enabled = ServiceProcess.StatusService(Propriedade.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Running;
                this.tbRestartServico.Enabled = ServiceProcess.StatusService(Propriedade.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Stopped;
            }
        }
        #endregion

        #region logs
        private int LogAtivo()
        {
            FormLogs oLog = null;
            //danasa 
            foreach (Form fg in this.MdiChildren)
            {
                if (fg is FormLogs)
                {
                    ///
                    /// configuracão já está ativa como MDI
                    /// 
                    this.notifyIcon1_MouseDoubleClick(null, null);
                    oLog = fg as FormLogs;
                    oLog.WindowState = FormWindowState.Normal;
                    return 1;
                }
            }
            foreach (Form fg in Application.OpenForms)
            {
                if (fg is FormLogs)
                {
                    oLog = fg as FormLogs;
                    oLog.WindowState = FormWindowState.Normal;
                    return 0;
                }
            }
            return -1;
        }
        private void tbLogs_Click(object sender, EventArgs e)
        {
            switch (LogAtivo())
            {
                case 0:
                    ///
                    /// configuracao ja existe como Modal
                    /// minimiza o MainForm para que a tela de configuracao esteja visivel
                    /// 
                    this.WindowState = FormWindowState.Minimized;
                    break;

                case -1:
                    {
                        FormLogs consultaCadastro = new FormLogs();
                        consultaCadastro.MdiParent = this;
                        consultaCadastro.MinimizeBox = false;
                        consultaCadastro.Show();
                    }
                    break;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            switch (LogAtivo())
            {
                case -1:
                    ///
                    /// tela principal está visivel?
                    /// 
                    if (this.WindowState != FormWindowState.Minimized)
                        ///
                        /// então abre a configuração como MDI
                        /// 
                        tbLogs.PerformClick();
                    else
                        using (FormLogs oConfig = new FormLogs())
                        {
                            oConfig.MinimizeBox = true;
                            oConfig.ShowDialog();
                        }
                    break;
            }
        }
        #endregion
    }
    #endregion
}
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormUpdate : MetroFramework.Forms.MetroForm
    {
        #region Public Constructors

        public FormUpdate()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Eventos

        private void FormUpdate_Load(object sender, EventArgs e)
        {
            ShowTitle();

            prgDownload.Value = 0;
            prgDownload.Visible = false;
            this.lblProgresso.Text = "";
            uninfeDummy.ClearControls(this, true, true);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MetroFramework.MetroMessageBox.Show(null,
                "Após o download, a aplicação será encerrada para a execução do instalador do aplicativo.\r\n\r\nDeseja continuar com a atualização?", "",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Auxiliar.WriteLog("Iniciado o processo de Atualização do UniNFe pelo Usuário.", false);

                    //Travar botao apos iniciar download
                    this.metroButton1.Enabled =
                        this.btnAtualizar.Enabled = false;

                    // Habilitar alguns controles
                    prgDownload.Visible = true;

                    //Executar o download
                    this.Text = "Baixando a atualização do " + Propriedade.NomeAplicacao;
                    Application.DoEvents();

                    this.metroButton1.Enabled = true;

                    //Executar o instalador do uninfe
                    uninfeDummy.mainForm.PararServicos(true);

                    IWebProxy proxy = null;

                    if (ConfiguracaoApp.Proxy)
                        proxy = Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, 
                            ConfiguracaoApp.ProxyUsuario, 
                            ConfiguracaoApp.ProxySenha, 
                            ConfiguracaoApp.ProxyPorta, 
                            ConfiguracaoApp.DetectarConfiguracaoProxyAuto);

                    new UniNFeUpdate(proxy).Instalar((updateProgressArgs) =>
                    {
                        // Atualizar a barra de progresso
                        prgDownload.Value = updateProgressArgs.ProgressPercentage;
                    });

                    Auxiliar.WriteLog("Processo de download da atualização do UniNFe pelo foi concluído.", false);

                    //Forçar o encerramento da aplicação
                    Application.Exit();
                }
            }
            catch (IOException ex)
            {
                MetroFramework.MetroMessageBox.Show(null, "Ocorreu um erro ao tentar fazer o download do instalador do aplicativo.\r\n\r\nErro: " + ex.Message, "");
            }
            catch (WebException ex)
            {
                MetroFramework.MetroMessageBox.Show(null, "Ocorreu um erro ao tentar fazer o download do instalador do aplicativo.\r\n\r\nErro: " + ex.Message, "");
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(null, "Ocorreu um erro ao tentar fazer o download do instalador do aplicativo.\r\n\r\nErro: " + ex.Message, "");
            }
        }

        #endregion Eventos

        #region Private Methods

        private void ShowTitle()
        {
            this.Text = "Atualização do " + Propriedade.NomeAplicacao;
            Application.DoEvents();
        }

        #endregion Private Methods
    }
}
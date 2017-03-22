using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;
using NFe.Components;
using NFe.Settings;

namespace NFe.UI.Formularios
{
    public partial class FormUpdate : MetroFramework.Forms.MetroForm
    {
        #region Campos
        // The stream of data retrieved from the web server
        /// <summary>
        /// Fluxo de dados obtidos a partir do servidor web
        /// </summary>
        private Stream strResponse;
        /// <summary>
        /// Fluxo de dados que será gravado em seguida no HardDisk
        /// </summary>
        private Stream strLocal;
        /// <summary>
        /// A solicitação para o servidor web das informações sobre o arquivo
        /// </summary>
        private HttpWebRequest webRequest;
        /// <summary>
        /// A resposta do servidor web que contém as informações sobre o arquivo
        /// </summary>
        private HttpWebResponse webResponse;
        /// <summary>
        /// Progresso do download em percentual
        /// </summary>
        private static int PercProgress;
        /// <summary>
        /// Delegate que chamaremos a partir da thread para atualizar o progresso
        /// </summary>
        /// <param name="BytesRead">Bytes a serem lidos</param>
        /// <param name="TotalBytes">Total de bytes (tamanho) do arquivo que está sendo efetuado o download</param>
        private delegate void UpdateProgessCallback(Int64 BytesRead, Int64 TotalBytes);
        /// <summary>
        /// URL de onde é para ser efetuado o download do arquivo
        /// </summary>
        private string URL = "http://www.unimake.com.br/downloads/iuninfe.exe";
        /// <summary>
        /// Caminho local onde é para ser gravado o arquivo que está sendo efetuado o download
        /// </summary>
        private string LocalArq;
        /// <summary>
        /// Caminho do sistema o qual o UniNFe sera atualizado
        /// </summary>
        private string PastaInstalar;
        /// <summary>
        /// Atualização cancelada
        /// </summary>
        private bool Cancelado = true;
        #endregion

        public FormUpdate()
        {
            InitializeComponent();
        }

        private void FormUpdate_Load(object sender, EventArgs e)
        {
            ShowTitle();

            string NomeInstalador = "i" + NFe.Components.Propriedade.NomeAplicacao.ToLower() + "5.exe";
#if _fw35
            NomeInstalador = "i" + NFe.Components.Propriedade.NomeAplicacao.ToLower() + "5_fw35.exe";
#endif                        

            this.PastaInstalar = Application.StartupPath;
            this.LocalArq = Application.StartupPath + "\\" + NomeInstalador;
            this.URL = "http://www.unimake.com.br/downloads/" + NomeInstalador;

            prgDownload.Value = 0;
            prgDownload.Visible = false;
            this.lblProgresso.Text = "";
            uninfeDummy.ClearControls(this, true, true);
        }

        private void ShowTitle()
        {
            this.Text = "Atualização do " + NFe.Components.Propriedade.NomeAplicacao;
            Application.DoEvents();
        }

        private void metroButton1_Click(object sender, EventArgs e)
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
                this.Text = "Baixando a atualização do " + NFe.Components.Propriedade.NomeAplicacao;
                Application.DoEvents();
                this.Download();

                this.metroButton1.Enabled = true;

                //Executar o instalador do uninfe
                if (File.Exists(LocalArq) && !Cancelado)
                {
                    NFe.UI.uninfeDummy.mainForm.PararServicos(true);

                    System.Diagnostics.Process.Start(this.LocalArq, "/SILENT /DIR=" + this.PastaInstalar);

                    Auxiliar.WriteLog("Processo de download da atualização do UniNFe pelo foi concluído.", false);
                    //Forçar o encerramento da aplicação                    
                    Application.Exit();

                    this.Close();
                }
                else if (!Cancelado)
                {
                    ShowTitle();

                    string msg = "Não foi possível localizar o instalador da atualização";
                    MetroFramework.MetroMessageBox.Show(null, msg, "");
                    this.btnAtualizar.Enabled = true;
                    Auxiliar.WriteLog(msg, false);
                }
            }
        }

        private void FormUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Cancelado)
            {
                strResponse.Close();
                strLocal.Close();
            }
            Cancelado = true;
        }

        #region Métodos gerais

        #region Download()
        /// <summary>
        /// Efetua o download do instalador do aplicativo
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Date: 20/05/2010
        /// </remarks>
        private void Download()
        {
            using (WebClient Client = new WebClient())
            {
                try
                {
                    // Desmarcar o flag
                    Cancelado = false;

                    // Criar um pedido do arquivo que será baixado
                    webRequest = (HttpWebRequest)WebRequest.Create(URL);

                    // Definir dados da conexao do proxy
                    if (ConfiguracaoApp.Proxy)
                    {
                        webRequest.Proxy = NFe.Components.Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta, ConfiguracaoApp.DetectarConfiguracaoProxyAuto);
                    }

                    // Atribuir autenticação padrão para a recuperação do arquivo
                    webRequest.Credentials = CredentialCache.DefaultCredentials;

                    // Obter a resposta do servidor
                    webResponse = (HttpWebResponse)webRequest.GetResponse();

                    // Perguntar ao servidor o tamanho do arquivo que será baixado
                    Int64 fileSize = webResponse.ContentLength;

                    // Abrir a URL para download                    
                    strResponse = Client.OpenRead(URL);

                    // Criar um novo arquivo a partir do fluxo de dados que será salvo na local disk
                    strLocal = new FileStream(LocalArq, FileMode.Create, FileAccess.Write, FileShare.None);

                    // Ele irá armazenar o número atual de bytes recuperados do servidor
                    int bytesSize = 0;

                    // Um buffer para armazenar e gravar os dados recuperados do servidor
                    byte[] downBuffer = new byte[2048];

                    // Loop através do buffer - Até que o buffer esteja vazio
                    while (Cancelado == false && (bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        // Gravar os dados do buffer no disco rigido
                        strLocal.Write(downBuffer, 0, bytesSize);

                        // Invocar um método para atualizar a barra de progresso
                        this.Invoke(new UpdateProgessCallback(this.UpdateProgress), new object[] { strLocal.Length, fileSize });
                    }

                    if (Cancelado)
                        MetroFramework.MetroMessageBox.Show(null, "Atualização foi cancelada!", "");
                    else
                        this.Invoke(new UpdateProgessCallback(this.UpdateProgress), new object[] { fileSize, fileSize });

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
                finally
                {
                    // Encerrar as streams
                    if (strResponse != null)
                        strResponse.Close();

                    if (strLocal != null)
                        strLocal.Close();

                    webRequest.Abort();
                    webResponse.Close();
                }
            }
        }
        #endregion

        #region UpdateProgress()
        /// <summary>
        /// Atualizar a barra de progresso do download
        /// </summary>
        /// <param name="BytesRead">Quantidade de bytes lidos</param>
        /// <param name="TotalBytes">Quantidade total de bytes do arquivo</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 20/05/2010
        /// </remarks>
        private void UpdateProgress(Int64 BytesRead, Int64 TotalBytes)
        {
            // Calcular o percentual do download em progresso
            PercProgress = Convert.ToInt32((BytesRead * 100) / TotalBytes);

            // Atualizar a barra de progresso
            prgDownload.Value = PercProgress;

            // Demonstrar o progresso atual do download
            prgDownload.Text = "Baixado " + BytesRead + " de " + TotalBytes + " (" + PercProgress + "%)";

            Application.DoEvents();
        }
        #endregion

        #endregion
    }
}

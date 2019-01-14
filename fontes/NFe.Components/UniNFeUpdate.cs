using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace NFe.Components
{
    public class UniNFeUpdate
    {
        #region Private Fields

        private Stream strLocal;
        private Stream strResponse;
        private HttpWebRequest webRequest;
        private HttpWebResponse webResponse;
        private string nomeInstalador;
        private string pastaInstalar;
        private string localArq;
        private string url;
        
        #endregion Private Fields

        #region Public Properties

        public IWebProxy Proxy { get; set; }

        #endregion Public Properties

        #region Public Constructor

        public UniNFeUpdate()
        {
            nomeInstalador = "i" + Propriedade.NomeAplicacao.ToLower() + "5.exe";

#if _fw35
            nomeInstalador = "i" + Propriedade.NomeAplicacao.ToLower() + "5_fw35.exe";
#elif x64
            nomeInstalador = "i" + Propriedade.NomeAplicacao.ToLower() + "5_fw46_x64.exe";
#elif x86
            nomeInstalador = "i" + Propriedade.NomeAplicacao.ToLower() + "5_fw46_x86.exe";
#endif
            pastaInstalar = Application.StartupPath;
            localArq = Path.Combine(Application.StartupPath, nomeInstalador);
            url = $"http://www.unimake.com.br/downloads/{nomeInstalador}";
        }

        public UniNFeUpdate(IWebProxy proxy)
            : this()
        {
            Proxy = proxy;
        }

        #endregion Public Constructor

        #region Private Methods

        private void Download(Action<UpdateProgessEventArgs> updateProgressAction = null)
        {
            using (WebClient Client = new WebClient())
            {
                try
                {
                    // Criar um pedido do arquivo que será baixado
                    webRequest = (HttpWebRequest)WebRequest.Create(url);

                    // Definir dados da conexao do proxy
                    if (Proxy != null)
                        webRequest.Proxy = Proxy;

                    // Atribuir autenticação padrão para a recuperação do arquivo
                    webRequest.Credentials = CredentialCache.DefaultCredentials;

                    // Obter a resposta do servidor
                    webResponse = (HttpWebResponse)webRequest.GetResponse();

                    // Perguntar ao servidor o tamanho do arquivo que será baixado
                    Int64 fileSize = webResponse.ContentLength;

                    // Abrir a URL para download
                    strResponse = Client.OpenRead(url);

                    if (!File.Exists(localArq))
                        File.Create(localArq).Close();

                    // Criar um novo arquivo a partir do fluxo de dados que será salvo na local disk
                    strLocal = new FileStream(localArq, FileMode.Create, FileAccess.Write, FileShare.None);

                    // Ele irá armazenar o número atual de bytes recuperados do servidor
                    int bytesSize = 0;

                    // Um buffer para armazenar e gravar os dados recuperados do servidor
                    byte[] downBuffer = new byte[2048];

                    UpdateProgessEventArgs updateProgressArgs = new UpdateProgessEventArgs
                    {
                        BytesRead = bytesSize,
                        TotalBytes = fileSize
                    };

                    // Loop através do buffer - Até que o buffer esteja vazio
                    while ((bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        // Gravar os dados do buffer no disco rigido
                        strLocal.Write(downBuffer, 0, bytesSize);
                        updateProgressArgs.BytesRead = strLocal.Length;

                        // Invocar um método para atualizar a barra de progresso
                        if (updateProgressAction != null)
                            updateProgressAction.Invoke(updateProgressArgs);
                    }
                }
                catch (IOException)
                {
                    throw;
                }
                catch (WebException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
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

        #endregion Private Methods

        #region Public Methods

        public void Instalar(Action<UpdateProgessEventArgs> updateProgressAction = null)
        {
            try
            {
                Download(updateProgressAction);
                System.Diagnostics.Process.Start(localArq, "/SILENT /DIR=" + pastaInstalar);
            }
            catch (IOException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Public Methods
    }

    #region Argumentos

    public class UpdateProgessEventArgs : EventArgs
    {
        /// <summary>
        /// Bytes a serem lidos
        /// </summary>
        public Int64 BytesRead;

        /// <summary>
        /// Total de bytes (tamanho) do arquivo que está sendo efetuado o download
        /// </summary>
        public Int64 TotalBytes;

        /// <summary>
        /// Porcentagem de progresso do download do arquivo
        /// </summary>
        public int ProgressPercentage
        {
            get
            {
                int result = TotalBytes == 0 ? 0 : Convert.ToInt32((BytesRead * 100) / TotalBytes);
                return result;
            }
        }
    }

    #endregion Argumentos
}
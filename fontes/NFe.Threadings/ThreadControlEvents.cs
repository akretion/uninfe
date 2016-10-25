using System;
using NFe.Settings;
using NFe.Service;
using NFe.Components;
using System.Threading;

namespace NFe.Threadings
{
    public class ThreadControlEvents : IDisposable
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public ThreadControlEvents()
        {
            ThreadItem.OnEnded += new ThreadItem.ThreadEndedHandler(ThreadItem_OnEnded);
            ThreadItem.OnReleased += new ThreadItem.ThreadReleasedHandler(ThreadItem_OnReleased);
            ThreadItem.OnStarted += new ThreadItem.ThreadStartHandler(ThreadItem_OnStarted);
        }

        #region ThreadItem_OnStarted()
        /// <summary>
        /// Evento executado quando a thread vai iniciar
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnStarted(ThreadItem item)
        {
#if DEBUG
            Debug.WriteLine(String.Format("Contagem em processamento: '{0}'.", FileSystemWatcher._pool.GetLifetimeService()));
#endif
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " iniciou o processamento (Data criação: " + item.FileInfo.LastWriteTime + ")", false);
            Processar(item);
        }

        /// <summary>
        /// Executa os passos do processamento do arquivo informado em item
        /// </summary>
        /// <param name="item">Item que deverá ser processado</param>
        private static void Processar(ThreadItem item)
        {
            FileSystemWatcher._pool.WaitOne();

            // A padding interval to make the output more orderly.
            int padding = Interlocked.Add(ref FileSystemWatcher._padding, 100);
            new Processar().ProcessaArquivo(item.Empresa, item.FileInfo.FullName);            
        }
        #endregion

        #region ThreadItem_OnReleased()
        /// <summary>
        /// Evento executado quando o item é removido da fila de execução
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnReleased(ThreadItem item)
        {
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " foi descarregado da lista de processamento (Data criação: " + item.FileInfo.LastWriteTime + ")", false);

            //Se estiver reconfigurando o UniNFe, tem que reiniciar as threads
            if (item.FileInfo.FullName.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.AltCon).EnvioXML) >= 0 ||
                item.FileInfo.FullName.IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.AltCon).EnvioTXT) >= 0)
            {
                ThreadService.Stop();
                ThreadService.Start();
            }
        }
        #endregion

        #region ThreadItem_OnEnded()
        /// <summary>
        /// Evento é executado quando a thread vai finalizar
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnEnded(ThreadItem item)
        {
            int listCount = FileSystemWatcher._pool.Release();
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " finalizou o processamento. Itens disponiveis no Semaforo (" + listCount.ToString() + "). (Data criação: " + item.FileInfo.LastWriteTime + ")", false);            
        }
        #endregion

        #region IDisposable members
        /// <summary>
        /// Retorna true se este objeto foi descarregado
        /// </summary>	
        public bool Disposed { get; private set; }

        ~ThreadControlEvents()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            Disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}

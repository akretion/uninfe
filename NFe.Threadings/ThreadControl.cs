using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using NFe.Settings;
using NFe.Components;
using NFe.Service;

namespace NFe.Threadings
{
    #region Semaphore
    /// <summary>
    /// controla a posição em que thread está no momento
    /// </summary>
    public enum Semaphore
    {
        /// <summary>
        /// adicionada. Podemos executar
        /// </summary>
        Added,

        /// <summary>
        /// Está sendo processada
        /// </summary>
        Started,

        /// <summary>
        /// finalizou a execução
        /// </summary>
        Finished,

        /// <summary>
        /// Terminou, pode ser removida da lista
        /// </summary>
        Released
    }
    #endregion

    #region ThreadItem
    /// <summary>
    /// classe de item da thread
    /// </summary>
    public class ThreadItem: IDisposable
    {
        #region delegates
        public delegate void ThreadStartHandler(ThreadItem item);
        public delegate void ThreadEndedHandler(ThreadItem item);
        public delegate void ThreadReleasedHandler(ThreadItem item);
        #endregion

        #region Eventos
        /// <summary>
        /// acontece quando a thread começou a leitura do arquivo
        /// </summary>
        public static event ThreadStartHandler OnStarted;
        /// <summary>
        /// acontece quando a thread finalizou a leitura do arquivo 
        /// </summary>
        public static event ThreadEndedHandler OnEnded;

        /// <summary>
        /// acontece quando a thread removeu o arquivo do buffer
        /// </summary>
        public static event ThreadReleasedHandler OnReleased;

        #endregion

        public ThreadItem(System.IO.FileInfo fi, int empresa)
        {
            FileInfo = fi;
            Empresa = empresa;
        }

        public System.IO.FileInfo FileInfo { get; private set; }
        public int Empresa { get; private set; }

        /*<#8084>
         * Com a morte da classe ThreadControl, este método passou a ser responsável pela execução dos eventos que antes eram feitos pela ThreadControl
         * 
         */
        /// <summary>
        /// Método responsável por executar os eventos de forma síncrona em uma thread separada
        /// </summary>
        public void Run()
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;
            bgw.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());

            bgw.DoWork += new DoWorkEventHandler((sender, e) =>
            {
                Thread.CurrentThread.Name = Empresa.ToString();

                try
                {
                    //avisa que vai iniciar
                    if(OnStarted != null) OnStarted(this);

                    //avisa que vai finalizar
                    if(OnEnded != null) OnEnded(this);
                }
                catch(Exception ex)
                {
                    Auxiliar.WriteLog("Ocorreu um erro na execução da thread que está sendo executada.\r\nThreadControl.Cs (1)\r\n" + ex.Message, true);
                }
                finally
                {
                    try
                    {
                        //remove o item                   
                        //avisa que removeu o item
                        if(OnReleased != null) OnReleased(this);
                    }
                    catch(Exception ex)
                    {
                        Auxiliar.WriteLog("Ocorreu um erro ao tentar remover o item da Thread que está sendo executada.\r\nThreadControl.Cs (2)\r\n" + ex.Message, true);
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            });

            bgw.RunWorkerAsync();
        }

        #region IDisposable members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                //need to do something?
            }

#if DEBUG
            Debug.WriteLine(String.Format("ThreadItem Dipose(disposing: {0});", disposing));
#endif
        }

        ~ThreadItem()
        {
#if DEBUG
            Debug.WriteLine("ThreadItem ~Destructor");
#endif

            Dispose(false);
        }
        #endregion
        //</#8084>
    }
    #endregion

    /*<#8084>
     * A classe ThreadControl deixou de existir por não ser mais utilizada dentro do aplicação, foi subusituída pelo método ThreadItem.Run()_
     *</#8084>
     */

    #region  ThreadService
    /// <summary>
    /// Classe responsavel por executar as thread´s base de verificação dos serviços a serem executados
    /// </summary>
    public static class ThreadService
    {
        public static List<Thread> Threads = new List<Thread>();

        public static void Stop()
        {
            for(int i = 0; i < Threads.Count; i++)
            {
                Thread t = Threads[i];
                t.Abort();
            }
            Threads.Clear();

            for(int i = 0; i < MonitoraPasta.fsw.Count; i++)
            {
                MonitoraPasta.fsw[i].StopWatch = true;
            }
        }

        public static void Start()
        {
            Empresa.CarregaConfiguracao();

            #region Ticket #110
            Empresa.CreateLockFile(true);
            #endregion

            //Executar o monitoramento de pastas das empresas cadastradas
            MonitoraPasta e = new MonitoraPasta();

            Threads.Clear();

            //Executa a thread que faz a limpeza dos arquivos temporários
            Thread t = new Thread(new Processar().LimpezaTemporario);
            t.IsBackground = true;
            t.Start();
            Threads.Add(t);

            //Executa a thread que faz a verificação das notas em processamento
            Thread t2 = new Thread(new Processar().EmProcessamento);
            t2.IsBackground = true;
            t2.Start();
            Threads.Add(t2);

            //Executar a thread que faz a consulta do recibo das notas fiscais enviadas
            Processar srv = new Processar();
            Thread t3 = new Thread(srv.GerarXMLPedRec);
            t3.IsBackground = true;
            t3.Start(new NFe.Service.TaskGerarXMLPedRec());
            Threads.Add(t3);
        }
    }
    #endregion
}

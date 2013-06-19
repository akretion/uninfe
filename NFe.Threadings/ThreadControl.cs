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
    public class ThreadItem
    {
        public ThreadItem(System.IO.FileInfo fi, int empresa)
        {
            FileInfo = fi;
            Status = Semaphore.Added;
            Empresa = empresa;
        }

        public System.IO.FileInfo FileInfo { get; private set; }
        public Semaphore Status { get; internal set; }
        public int Empresa { get; private set; }
    }
    #endregion

    #region ThreadControl
    /// <summary>
    /// controla a execução das threads dentro da aplicação
    /// </summary>
    public static class ThreadControl
    {
        #region Extension Method
        /// <summary>
        /// reetorna o primeiro item da lista
        /// </summary>
        /// <param name="item">lista de items</param>
        /// <returns>primeiro item que encontrar</returns>
        public static ThreadItem GetItem(this List<ThreadItem> item)
        {
            ThreadItem result = null;

            if (Buffer.Count > 0)
            {
                try
                {
                    result = item.FirstOrDefault(i =>
                      {
                          if (i.Status == Semaphore.Added)
                              return true;

                          return false;
                      });
                }
                catch (Exception ex)
                {
                    /*Provavelmente nunca irá cair aqui... Mas...*/
                    Auxiliar.WriteLog(ex.Message, true);
                }
            }

            return result;
        }
        #endregion

        #region Construtores
        static ThreadControl()
        {
        }
        #endregion

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

        #region Atributos
        private static List<ThreadItem> Buffer = new List<ThreadItem>();
        private static ManualResetEvent ResetThread = null;
        #endregion

        #region Métodos
        public static void Start()
        {
            //inicia a thread que ficará executando o buffer
            ThreadControl.ResetThread = new ManualResetEvent(false);

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());
            worker.DoWork += new DoWorkEventHandler(ExecuteBuffer);
            worker.RunWorkerAsync();

            /*Thread tBuffer = new Thread(new ThreadStart(ExecuteBuffer));
            tBuffer.Start();*/
        }

        public static void Stop()
        {
            ResetThread.Set();
        }

        /// <summary>
        /// adiciona um item na lista de threads
        /// </summary>
        /// <param name="item"></param>
        public static void Add(ThreadItem item)
        {
            Buffer.Add(item);
        }

        /// <summary>
        /// executa o buffer (loop infinito)
        /// </summary>
        static void ExecuteBuffer(object sender2, DoWorkEventArgs e2)
        {
            bool signal = false;
            while (!signal)
            {
                while (Buffer.Count > 0)
                {
                    try
                    {
                        ThreadItem item = Buffer.GetItem();

                        if (item != null)
                        {
                            item.Status = Semaphore.Started;

                            BackgroundWorker worker = new BackgroundWorker();
                            
                            worker.WorkerSupportsCancellation = true;           
                            worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());
                            worker.DoWork += new DoWorkEventHandler(ExecuteItem);
                            worker.RunWorkerAsync(item);                            

                            /*Thread t = new Thread(new ParameterizedThreadStart(ExecuteItem));
                            t.Name = item.Empresa.ToString();
                            t.Start(item);*/
                        }
                    }
                    catch (Exception ex)
                    {
                        Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace, true);
                    }

                    Thread.Sleep(200);
                }

                try
                {
                    signal = ResetThread.WaitOne(1000);
                }
                catch
                {
                    signal = true;
                }
            }
        }

        //static void ExecuteItem(object o)
        static void ExecuteItem(object sender, DoWorkEventArgs e)
        {
            ThreadItem item = e.Argument as ThreadItem;
            Thread.CurrentThread.Name = item.Empresa.ToString();

            try
            {
                //avisa que vai iniciar
                if (OnStarted != null) OnStarted(item);

                //avisa que vai finalizar
                item.Status = Semaphore.Finished;
                if (OnEnded != null) OnEnded(item);
            }
            catch (Exception ex)
            {
                Auxiliar.WriteLog("Ocorreu um erro na execução da thread que está sendo executada.\r\nThreadControl.Cs (1)\r\n" + ex.Message, true);
            }                        
            finally
            {
                try
                {
                    //remove o item
                    item.Status = Semaphore.Released;
                    Buffer.Remove(item);

                    //avisa que removeu o item
                    if (OnReleased != null) OnReleased(item);                    
                }
                catch (Exception ex)
                {
                    Auxiliar.WriteLog("Ocorreu um erro ao tentar remover o item da Thread que está sendo executada.\r\nThreadControl.Cs (2)\r\n" + ex.Message, true);
                }
            }
        }
        #endregion
    }
    #endregion

    #region  ThreadService
    /// <summary>
    /// Classe responsavel por executar as thread´s base de verificação dos serviços a serem executados
    /// </summary>
    public static class ThreadService
    {
        public static List<Thread> Threads = new List<Thread>();

        public static void Stop()
        {
            for (int i = 0; i < Threads.Count; i++)
            {
                Thread t = Threads[i];
                t.Abort();
            }
            Threads.Clear();

            ThreadControl.Stop();

            for (int i = 0; i < MonitoraPasta.fsw.Count; i++)
            {
                MonitoraPasta.fsw[i].StopWatch = true;
            }            
        }

        public static void Start()
        {
            Empresa.CarregaConfiguracao();

            ThreadControl.Start();

            //Executar o monitoramento de pastas das empresas cadastradas
            MonitoraPasta e = new MonitoraPasta();

            Threads.Clear();

            //Executa a thread que faz a limpeza dos arquivos temporários
            Thread t = new Thread(new Processar().LimpezaTemporario);
            t.Start();
            Threads.Add(t);

            //Executa a thread que faz a verificação das notas em processamento
            Thread t2 = new Thread(new Processar().EmProcessamento);
            t2.Start();
            Threads.Add(t2);

            //Executar a thread que faz a consulta do recibo das notas fiscais enviadas
            Processar srv = new Processar();
            Thread t3 = new Thread(srv.GerarXMLPedRec);
            t3.Start(new NFe.Service.TaskGerarXMLPedRec());
            Threads.Add(t3);
        }
    }
    #endregion
}

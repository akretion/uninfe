using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFe.Settings;
using NFe.Service;
using NFe.Components;
using System.Threading;

namespace NFe.Threadings
{
    #region #21040

    /// <summary>
    /// Método que será executado ao processar o timer
    /// </summary>
    /// <param name="item"></param>
    delegate void ProcessarHandler(ThreadItem item);

    class BufferItem: IDisposable
    {
        #region propriedades
        /// <summary>
        /// Quando uma empresa usa um certificado do tipo A3, é executado por este Buffer
        /// </summary>
        public Queue<ThreadItem> Buffer { get; private set; }

        /// <summary>
        /// Este timer é executado sempre que o buffer possuir itens que deverão ser enviados
        /// </summary>
        System.Timers.Timer timer = null;

        #endregion

        #region construtores
        /// <summary>
        /// Inicia uma nova instancia e define o método de processamento
        /// </summary>
        /// <param name="processar">método que será chamado para processar o item</param>
        public BufferItem(ProcessarHandler processar)
        {
            Buffer = new Queue<ThreadItem>();

            if(timer == null)
            {
                //-------------------------------------------------------------------------
                // Inicia o timer com 500 milissegundos. Desta forma a execução será rápida
                // Como faz um "Stop" logo ao iniciar o método "Elapsed", não tem problema
                // de chamar com este tempo.
                //-------------------------------------------------------------------------
                timer = new System.Timers.Timer(500);

                timer.Elapsed += (s, e) =>
                {
                    timer.Stop();

                    try
                    {
                        //-------------------------------------------------------------------------
                        // Recupera o threaditem e executa o mesmo
                        //-------------------------------------------------------------------------
                        while(Buffer.Count > 0)
                        {
                            ThreadItem item = Buffer.Dequeue();
                            if(item == null) continue;
                            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " iniciou o processamento pelo Buffer");
                            processar.Invoke(item);
                            Thread.Sleep(0);
                            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " finalizou o processamento pelo Buffer");
                            if(Disposed) return;//cai fora ... foi descarregada
                        }
                    }
                    catch(Exception ex)
                    {
                        Auxiliar.WriteLog("ExceptionBuffer: " + ex.ToString());
                    }
                    finally
                    {
                        //-------------------------------------------------------------------------
                        // Sair ... mas continuar executando de tempos em tempos
                        //-------------------------------------------------------------------------
                        timer.Start();
                    }
                };
            }

            timer.Start();
        }
        #endregion

        #region IDisposable members
        /// <summary>
        /// Retorna true se este objeto foi descarregado
        /// </summary>	
        public bool Disposed { get; private set; }

        ~BufferItem()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                //faça algo
                GC.SuppressFinalize(this);
            }

            Disposed = true;
            timer.Stop();
            Buffer.Clear();
        }

        public virtual void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
    #endregion

    public class ThreadControlEvents: IDisposable
    {
        #region #21040
        /// <summary>
        /// Quando uma empresa usa um certificado do tipo A3, é executado por este Buffer
        /// É utilizado apenas os primeiros 8 dígitos do CNPJ como chave.
        /// </summary>
        IDictionary<int, BufferItem> Buffer = new Dictionary<int, BufferItem>();
        #endregion

        /// <summary>
        /// Construtor
        /// </summary>
        public ThreadControlEvents()
        {
            ThreadItem.OnEnded += new ThreadItem.ThreadEndedHandler(ThreadItem_OnEnded);
            ThreadItem.OnReleased += new ThreadItem.ThreadReleasedHandler(ThreadItem_OnReleased);
            ThreadItem.OnStarted += new ThreadItem.ThreadStartHandler(ThreadItem_OnStarted);

            #region #21040
            //criar um buffer para cada empresa que o certificado é A3
            foreach(Empresa emp in Empresa.Configuracoes)
            {
                if(emp.X509Certificado.IsA3())
                {
                    //-------------------------------------------------------------------------
                    // Usar o subject como chave, pois pode se configurar o mesmo certificado
                    // para empresas diferentes.
                    //-------------------------------------------------------------------------
                    int key = emp.X509Certificado.Subject.GetHashCode();
                    if(!Buffer.ContainsKey(key)) Buffer[key] = new BufferItem(Processar);
                }
            }
            #endregion
        }

        #region ThreadItem_OnStarted()
        /// <summary>
        /// Evento executado quando a thread vai iniciar
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnStarted(ThreadItem item)
        {
            Empresa empresa = Empresa.Configuracoes[item.Empresa];

            #region #21040
            /*
             * Se o certificado for A3, então vai para o Buffer controlado, pois deverá ser executado um de cada vez
             * 
             */
            if(empresa.X509Certificado.IsA3())
            {
                //-------------------------------------------------------------------------
                // Usar o subject como chave, pois pode se configurar o mesmo certificado
                // para empresas diferentes.
                //-------------------------------------------------------------------------
                int key = empresa.X509Certificado.Subject.GetHashCode();
                BufferItem bItem = Buffer[key];
                bItem.Buffer.Enqueue(item);
                return;
            }
            #endregion

            //danasa 12/8/2011
            //mudei de posição e inclui o FullName
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " iniciou o processamento");
            Processar(item);
        }

        /// <summary>
        /// Executa os passos do processamento do arquivo informado em item
        /// </summary>
        /// <param name="item">Item que deverá ser processado</param>
        private static void Processar(ThreadItem item)
        {
            //Serviços tipoServico = Auxiliar.DefinirTipoServico(item.Empresa, item.FileInfo.FullName);
            new Processar().ProcessaArquivo(item.Empresa, item.FileInfo.FullName);//, tipoServico);
        }
        #endregion

        #region ThreadItem_OnReleased()
        /// <summary>
        /// Evento executado quando o item é removido da fila de execução
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnReleased(ThreadItem item)
        {
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " foi descarregado da lista de processamento");

            //Se estiver reconfigurando o UniNFe, tem que reiniciar as threads
            if(item.FileInfo.FullName.IndexOf(Propriedade.ExtEnvio.AltCon_XML) >= 0 || item.FileInfo.FullName.IndexOf(Propriedade.ExtEnvio.AltCon_TXT) >= 0)
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
            Empresa empresa = Empresa.Configuracoes[item.Empresa];

            #region #21040
            /*
             * Se o certificado for A3, então vai para o Buffer controlado, pois deverá ser executado um de cada vez
             * 
             */
            if(empresa.X509Certificado.IsA3())
                Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " será processado pelo buffer");
            else
                Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " finalizou o processamento");
            #endregion
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
            if(disposing)
            {
                GC.SuppressFinalize(this);
            }

            Buffer.Clear();
            Disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}

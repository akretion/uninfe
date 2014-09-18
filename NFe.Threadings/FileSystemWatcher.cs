using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections;

using NFe.Components;
using NFe.Settings;

namespace NFe.Threadings
{
    public class FileSystemWatcher : IDisposable    //<<<danasa 1-5-2011
    {
        public delegate void FileChangedHandler(FileInfo fi);
        public event FileChangedHandler OnFileChanged;
        public string Directory { get; set; }
        public string Filter { get; set; }

        private bool CancelProcess = false;
        private List<Thread> workers = new List<Thread>();

        private bool _disposed = false;
        ~FileSystemWatcher()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && worker != null)
                    worker.Dispose();

                workers.Clear();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public FileSystemWatcher(string directory, string filter)
        {
            Directory = directory;
            Filter = filter;
        }

        private BackgroundWorker worker = null;

        public void StartWatch()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Hashtable OldFiles = new Hashtable();

            CancelProcess = false;
            if (String.IsNullOrEmpty(Directory) || (!String.IsNullOrEmpty(Directory) && !System.IO.Directory.Exists(Directory)))
                CancelProcess = true;

            while (!CancelProcess)
            {
                try
                {
                    if (!String.IsNullOrEmpty(Directory))
                    {
                        bool clear = true;
                        foreach (Thread worker in this.workers)
                        {
                            if (worker.IsAlive)
                                clear = false;
                        }
                        if (clear)
                            workers.Clear();

                        var Files = System.IO.Directory.GetFiles(Directory, "*.*", System.IO.SearchOption.TopDirectoryOnly).
                                        Where(s => Filter.Contains(System.IO.Path.GetExtension(s).ToLower()));

                        if (Files != null)
                        {
                            Hashtable NewFiles = new Hashtable();

                            //cria todos os fileinfos
                            foreach (string s in Files)
                            {
                                FileInfo fi = new FileInfo(s);

                                if (File.Exists(fi.FullName))
                                    if (!Functions.FileInUse(fi.FullName))
                                    {
                                        //Definir o nome do arquivo na pasta temp
                                        string arqTemp = fi.DirectoryName + "\\Temp\\" + fi.Name;

                                        //Remove atributo somente Leitura para evitar erros de permissão com o Arquivo - Renan Borges
                                        NFe.Service.TFunctions.RemoveSomenteLeitura(fi.FullName);

                                        //Mover o arquivo para pasta temp e disparar o serviço a ser executado
                                        Functions.Move(fi.FullName, arqTemp);

                                        FileInfo fi2 = new FileInfo(arqTemp);
                                        NewFiles.Add(fi2.Name, fi2);
                                    }
                            }

                            foreach (FileInfo fi in NewFiles.Values)
                            {
                                if (CancelProcess || ((BackgroundWorker)sender).CancellationPending)
                                {
                                    break;
                                }

                                if (OldFiles.Contains(fi.Name))
                                {
                                    FileInfo oldFi = OldFiles[fi.Name] as FileInfo;

#if DEBUG
                                    Debug.WriteLine(String.Format("FileSystem: Lendo arquivo '{0}'.", fi.FullName));
#endif

                                    if (oldFi.CreationTime != fi.CreationTime || oldFi.Length != fi.Length)
                                    {
                                        RaiseFileChanged(fi);
                                    }
                                    else
                                    {
                                        #region Bug Fix #14522
                                        if (!fi.FullName.ToLower().Contains("\\contingencia"))
                                        {
                                            //-------------------------------------------------------------------------
                                            // Se caiu aqui, é o mesmo arquivo, logo iremos mover para a pasta erro
                                            //-------------------------------------------------------------------------
                                            int index = Empresas.FindEmpresaByThread();
                                            string errorPath = String.Format("{0}\\{1}",
                                                Settings.Empresas.Configuracoes[index].PastaXmlErro,
                                                oldFi.Name);

#if DEBUG
                                            Debug.WriteLine(String.Format("FileSystem: Fim lendo arquivo '{0}'.", fi.FullName));
#endif

                                            Functions.Move(oldFi.FullName, errorPath);
                                        }
                                        #endregion
                                    }

#if DEBUG
                                    Debug.WriteLine(String.Format("FileSystem: Fim lendo arquivo '{0}'.", fi.FullName));
#endif
                                }
                                else
                                {
#if DEBUG
                                    Debug.WriteLine(String.Format("FileSystem: Lendo arquivo '{0}'.", fi.FullName));
#endif

                                    RaiseFileChanged(fi);
                                    OldFiles.Add(fi.Name, fi);
#if DEBUG
                                    Debug.WriteLine(String.Format("FileSystem: Fim lendo arquivo '{0}'.", fi.FullName));
#endif
                                }
                            }
                            OldFiles = NewFiles.Clone() as Hashtable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                }
                finally
                {
                    Thread.Sleep(2000);
                }
            }
        }

        private void RaiseFileChanged(FileInfo fi)
        {
            if (File.Exists(fi.FullName))
            {
                ///
                /// TODO!!!
                /// entre este processo e o RaiseEvent está tendo uma demora considerável
                /// 
                if (fi.Length > 0)
                {
                    Thread tworker = new Thread(
                        new ThreadStart(
                            delegate
                            {
                                if (OnFileChanged != null)
                                {
                                    OnFileChanged(fi);
                                }
                            }
                        ));
                    this.workers.Add(tworker);
                    tworker.IsBackground = true;
                    tworker.Start();
                    tworker.Join();

/*
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());
                    worker.DoWork += new DoWorkEventHandler(RaiseEvent);
                    worker.RunWorkerAsync(fi);*/
                }
                else
                {
                    Auxiliar.WriteLog(fi.FullName + " - Arquivo com tamanho zerado.", true);
                }
            }
            else
            {
                Auxiliar.WriteLog(fi.FullName + " - Arquivo não existe.", true);
            }
        }

        private void RaiseEvent(object sender, DoWorkEventArgs e)
        {
            FileInfo fi = e.Argument as FileInfo;

#if DEBUG
            Debug.WriteLine(String.Format("A leitura do arquivo '{0}' foi iniciada.", fi.FullName));
#endif

            if (OnFileChanged != null)
                OnFileChanged(fi);

#if DEBUG
            Debug.WriteLine(String.Format("A leitura do arquivo '{0}' foi finalizada", fi.FullName));
#endif
        }

        /// <summary>
        /// StopWatch
        /// </summary>
        public bool StopWatch       //modifiquei para que no mainform se verifique se foi cancelado ou nao
        {
            get { return CancelProcess; }
            set
            {
                if (value && this.worker != null && this.worker.IsBusy)//<<<<danasa 1-5-2011
                {
                    CancelProcess = true;
                    foreach (Thread worker in this.workers)
                        if (worker.IsAlive)
                            worker.Abort();
 
                    this.worker.CancelAsync();
                }
            }
        }
    }
}

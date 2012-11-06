using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.IO;
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

        private bool _disposed = false;
        ~FileSystemWatcher()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing && worker != null)
                    worker.Dispose();
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

        private BackgroundWorker worker = null;         //<<<<danasa 1-5-2011

        public void StartWatch()
        {
            /*BackgroundWorker*/
            worker = new BackgroundWorker();   //<<<<danasa 1-5-2011
            worker.WorkerSupportsCancellation = true;               //<<<<danasa 1-5-2011
            worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());//<<<<danasa 1-5-2011
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            Hashtable OldFiles = new Hashtable();

            CancelProcess = false;//<<<<danasa 1-5-2011

            while (!CancelProcess)
            {
                try
                {
                    string[] Files = System.IO.Directory.GetFiles(Directory, Filter, System.IO.SearchOption.TopDirectoryOnly);

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

                            if (oldFi.CreationTime != fi.CreationTime)
                                RaiseFileChanged(fi);
                            else if (oldFi.Length != fi.Length)
                                RaiseFileChanged(fi);
                        }
                        else
                        {
                            RaiseFileChanged(fi);
                            OldFiles.Add(fi.Name, fi);
                        }
                    }

                    OldFiles = NewFiles.Clone() as Hashtable;
                }
                catch (Exception ex)
                {
                    Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                }

                Thread.Sleep(1000);
            }
        }

        private void RaiseFileChanged(FileInfo fi)
        {
            if (File.Exists(fi.FullName))
            {
                if (fi.Length > 0)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ((sender, e) => ((BackgroundWorker)sender).Dispose());
                    worker.DoWork += new DoWorkEventHandler(RaiseEvent);
                    worker.RunWorkerAsync(fi);

                    /*Thread t = new Thread(new ThreadStart(delegate()
                    {
                        if (OnFileChanged != null)
                            OnFileChanged(fi);
                    }));
                    t.Start();*/
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

            if (OnFileChanged != null)
                OnFileChanged(fi);
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
                    this.worker.CancelAsync();
                }
            }
        }
    }
}

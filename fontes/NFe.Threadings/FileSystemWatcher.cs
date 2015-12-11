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
using NFe.Service;

namespace NFe.Threadings
{
    public class FileSystemWatcher : IDisposable    //<<<danasa 1-5-2011
    {
        #region Propriedades
        public delegate void FileChangedHandler(FileInfo fi);
        public event FileChangedHandler OnFileChanged;
        private List<string> Directorys = new List<string>();
        public string Directory { get; set; }
        public string Filter { get; set; }

        private bool CancelProcess = false;

        private bool _disposed = false;

        public static System.Threading.Semaphore _pool;
        public static int _padding;
        #endregion

        #region Destrutores
        ~FileSystemWatcher()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                CancelProcess = true;
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Construtores
        public FileSystemWatcher(string directory, string filter)
        {
            Directorys.Clear();
            Directorys.Add(directory);
            Filter = filter;
        }

        public FileSystemWatcher(List<string> directorys, string filter)
        {
            Directorys.Clear();
            foreach (var item in directorys)
            {
                Directorys.Add(item);
            }

            Filter = filter;
        }
        #endregion

        public void StartWatch()
        {
            Thread t = new Thread(
                      new ThreadStart(ProcessFiles));
            t.IsBackground = true;
            t.Start();
        }

        #region Metodos
        void ProcessFiles()
        {
            int emp = Empresas.FindEmpresaByThread();
            Hashtable OldFiles = new Hashtable();
            string arqTemp = "";

            CancelProcess = false;
/*            if (String.IsNullOrEmpty(Directory) || (!String.IsNullOrEmpty(Directory) && !System.IO.Directory.Exists(Directory)))
                CancelProcess = true;*/

            // Create a semaphore that can satisfy up to three
            // concurrent requests. Use an initial count of zero,
            // so that the entire semaphore count is initially
            // owned by the main program thread.
            //
            _pool = new System.Threading.Semaphore(10, 10);

            while (!CancelProcess)
            {
                try
                {
                    foreach (var item in Directorys)
                    {
                        Directory = item;

                        if (!String.IsNullOrEmpty(Directory) && System.IO.Directory.Exists(Directory))
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(Directory);                            
                            FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                                                                .Where(s => Filter.Contains(s.Extension.ToLower()))
                                                                .OrderBy(o => o.CreationTime)
                                                                .ToArray(); //Retorna o conteúdo já ordenado por data de modificação, do menor para o maior. Wandrey 26/03/2015

                            if (files != null)
                            {
                                Hashtable NewFiles = new Hashtable();

                                //cria todos os fileinfos
                                foreach (FileInfo fi in files)
                                {
                                    if (File.Exists(fi.FullName))
                                        if (!Functions.FileInUse(fi.FullName))
                                        {
                                            //Definir o nome do arquivo na pasta temp
                                            arqTemp = fi.DirectoryName + "\\Temp\\" + fi.Name;

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
                                    if (CancelProcess)
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
                }
                catch (Exception ex)
                {
                    Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace, false);
                    if (emp >= 0)
                        Functions.GravarErroMover(arqTemp, Empresas.Configuracoes[emp].PastaXmlRetorno, ex.ToString());
                }
                finally
                {
                    if (!CancelProcess)
                        Thread.Sleep(2000);
                }
            }
        }

        private void RaiseFileChanged(FileInfo fi)
        {
            string msgError = null;

            if (File.Exists(fi.FullName))
            {
                ///
                /// TODO!!!
                /// entre este processo e o RaiseEvent está tendo uma demora considerável
                /// 
                if (fi.Length > 0 || (  fi.Name.ToLower().EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.sair_XML).EnvioTXT) ||
                                        fi.Name.ToLower().EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.pedUpdatewsdl).EnvioTXT) ||
                                        fi.Name.ToLower().EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.pedLayouts).EnvioTXT) ||
                                        fi.Name.ToLower().EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.pedRestart).EnvioTXT)))
                {
                    Thread tworker = new Thread(
                        new ThreadStart(
                            delegate
                            {
                                if (OnFileChanged != null)
                                {
                                    try
                                    {
                                        OnFileChanged(fi);
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                                    }
                                }
                            }
                        ));
                    tworker.IsBackground = true;
                    tworker.Start();
                    tworker.Join();
                }
                else
                {
                    msgError = fi.FullName + " - Arquivo com tamanho zerado.";
                }
            }
            else
            {
                msgError = fi.FullName + " - Arquivo não existe.";
            }
            if (msgError != null)
                if (fi.Directory.Name.ToLower().EndsWith("geral\\temp"))
                    NFe.Components.Functions.WriteLog(msgError, false, true, "");
                else
                    Auxiliar.WriteLog(msgError, true);
        }

        /// <summary>
        /// StopWatch
        /// </summary>
        public bool StopWatch
        {
            get { return CancelProcess; }
            set { CancelProcess = value; }
        }
        #endregion
    }
}
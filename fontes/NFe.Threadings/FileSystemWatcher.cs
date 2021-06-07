using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unimake.Business.DFe.Security;

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

        #endregion Propriedades

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

        #endregion Destrutores

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

        #endregion Construtores

        public void StartWatch()
        {
            Thread t = new Thread(new ThreadStart(ProcessFiles));
            t.IsBackground = true;
            t.Start();
        }

        #region Metodos

        private void ProcessFiles()
        {
            bool pastaGeral = false;
            foreach (var item in Directorys)
            {
                string directory = item;
                if (directory.ToLower().EndsWith("\\geral"))
                    pastaGeral = true;
            }
            int emp = (pastaGeral ? -1 : Empresas.FindEmpresaByThread());

            string arqTemp = "";

            CancelProcess = false;

            // Create a semaphore that can satisfy up to three
            // concurrent requests. Use an initial count of zero,
            // so that the entire semaphore count is initially
            // owned by the main program thread.
            //
            _pool = new System.Threading.Semaphore(10, 20);

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
                                if (files.Length > 0)
                                {
                                    //cria todos os fileinfos
                                    foreach (FileInfo fi in files)
                                    {
                                        if (CancelProcess)
                                        {
                                            break;
                                        }

                                        if (File.Exists(fi.FullName))
                                        {
                                            if (!Functions.FileInUse(fi.FullName))
                                            {
                                                arqTemp = fi.DirectoryName + "\\Temp\\" + fi.Name;
                                                Functions.Move(fi.FullName, arqTemp);
                                                RaiseFileChanged(new FileInfo(arqTemp), emp);
                                            }
                                        }
                                    }
                                }
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
                        Thread.Sleep(1000);
                }
            }
        }

        private void RaiseFileChanged(FileInfo fi, int emp)
        {
            string msgError = null;

            if (File.Exists(fi.FullName))
            {
                ///
                /// TODO!!!
                /// entre este processo e o RaiseEvent está tendo uma demora considerável
                ///
                if (fi.Length > 0 || (fi.Name.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.sair_XML).EnvioTXT) ||
                                      fi.Name.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.pedUpdatewsdl).EnvioTXT) ||
                                      fi.Name.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.pedLayouts).EnvioTXT) ||
                                      fi.Name.ToLower().EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.pedRestart).EnvioTXT)))
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

                    if (emp >= 0)
                    {
                        if (fi.Name.ToLower().IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.AltCon).EnvioXML) >= 0 ||
                            fi.Name.ToLower().IndexOf(Propriedade.Extensao(Propriedade.TipoEnvio.AltCon).EnvioTXT) >= 0)
                            Empresas.Configuracoes[emp].CriarFilaProcesamento = true;

                        if (Empresas.Configuracoes[emp].X509Certificado.IsA3() || Empresas.Configuracoes[emp].CriarFilaProcesamento)
                        {
                            tworker.Join();
                        }
                    }
                    else
                    {
                        tworker.Join();
                    }
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
                    Functions.WriteLog(msgError, false, true, "");
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

        #endregion Metodos
    }
}
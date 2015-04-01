using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

using NFe.Components;
using NFe.Components.Info;
using NFe.Settings;

namespace uninfe
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler((sender, e) =>
            {
                Auxiliar.WriteLog(e.Exception.Message + "\r\n" + e.Exception.StackTrace, false);
                if (e.Exception.InnerException != null)
                {
                    Auxiliar.WriteLog(e.Exception.InnerException.Message + "\r\n" + e.Exception.InnerException.StackTrace, false);

                    if (e.Exception.InnerException.InnerException != null)
                    {
                        Auxiliar.WriteLog(e.Exception.InnerException.InnerException.Message + "\r\n" + e.Exception.InnerException.InnerException.StackTrace, false);
                    }

                    if (e.Exception.InnerException.InnerException.InnerException != null)
                    {
                        Auxiliar.WriteLog(e.Exception.InnerException.InnerException.InnerException.Message + "\r\n" + e.Exception.InnerException.InnerException.InnerException.StackTrace, false);
                    }
                }
            });

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, e) =>
            {
                Auxiliar.WriteLog(e.ExceptionObject.ToString(), false);
            });
            //Esta deve ser a primeira linha do Main, não coloque nada antes dela. Wandrey 31/07/2009
            Propriedade.AssemblyEXE = Assembly.GetExecutingAssembly();

            bool silencioso = false;
            ConfiguracaoApp.AtualizaWSDL = false;

            if (args.Length >= 1)
                foreach (string param in args)
                {
                    if (param.ToLower().Equals("/silent"))
                    {
                        silencioso = true;
                        continue;
                    }
                    if (param.ToLower().Equals("/updatewsdl"))
                    {
                        ConfiguracaoApp.AtualizaWSDL = true;
                        continue;
                    }
                    if (param.ToLower().Equals("/quit") || param.ToLower().Equals("/restart"))
                    {
                        string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                        int Id = System.Diagnostics.Process.GetCurrentProcess().Id;

                        foreach (System.Diagnostics.Process clsProcess in System.Diagnostics.Process.GetProcesses())
                        {
                            if (clsProcess.ProcessName.Equals(procName))
                            {
                                try
                                {
                                    if (param.ToLower().Equals("/quit") ||
                                        (param.ToLower().Equals("/restart") && clsProcess.Id != Id))
                                    {
                                        Empresas.ClearLockFiles(false);
                                        clsProcess.Kill();
                                    }
                                }
                                catch
                                {
                                }
                                if (param.ToLower().Equals("/quit"))
                                    return;
                            }
                        }
                    }
                }

            Propriedade.TipoAplicativo = TipoAplicativo.Nfe;

#if DEBUG
            NFe.Components.NativeMethods.AllocConsole();
            Console.WriteLine("start....." + Propriedade.NomeAplicacao);
#endif

            bool executando = Aplicacao.AppExecutando();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (executando)
            {
                if (!silencioso)
                    MessageBox.Show("Somente uma instância do " + Propriedade.NomeAplicacao + " pode ser executada." + (Empresas.ExisteErroDiretorio ? "\r\n\r\nPossíveis erros:\r\n\r\n" + Empresas.ErroCaminhoDiretorio : ""), "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (Empresas.ExisteErroDiretorio)
                    if (!silencioso)
                        MessageBox.Show("Ocorreu um erro ao efetuar a leitura das configurações da empresa. " +
                                "Por favor entre na tela de configurações da(s) empresa(s) listada(s), acesse a aba \"Pastas\" e reconfigure-as.\r\n\r\n" + Empresas.ErroCaminhoDiretorio, "Atenção",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Application.Run(new MainForm());
            }
#if DEBUG
            NFe.Components.NativeMethods.FreeConsole();
#endif
        }
    }
}

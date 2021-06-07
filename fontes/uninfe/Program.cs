using NFe.Components;
using NFe.Components.Info;
using NFe.Settings;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace uninfe
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Unimake.Business.DFe.Xml.AssemblyResolver.AssemblyResolve;

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

            //Começar a contar o tempo de execução do aplicativo - Renan 24/06/2015
            ConfiguracaoApp.ExecutionTime = new System.Diagnostics.Stopwatch();
            ConfiguracaoApp.ExecutionTime.Start();

            if (args.Length >= 1)
            {
                foreach (string param in args)
                {
                    if (param.ToLower().Equals("/silent"))
                    {
                        silencioso = true;
                        continue;
                    }
                    if (param.ToLower().Equals("/updatewsdl"))
                    {
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
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            Propriedade.TipoAplicativo = TipoAplicativo.Todos;

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
                {
                    MetroFramework.MetroMessageBox.Show(null,
                        "Somente uma instância do " + Propriedade.NomeAplicacao + " pode ser executada." + (Empresas.ExisteErroDiretorio ? "\r\nPossíveis erros:\r\n" + Empresas.ErroCaminhoDiretorio : ""), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (!silencioso)
                {
                    if (Empresas.ExisteErroDiretorio)
                    {
                        MetroFramework.MetroMessageBox.Show(null,
                                "Ocorreu um erro ao efetuar a leitura das configurações da empresa. " +
                                "Por favor entre na tela de configurações da(s) empresa(s) listada(s), acesse a aba \"Pastas\" e reconfigure-as.\r\n\r\n" + Empresas.ErroCaminhoDiretorio, "",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                //Vamos forçar a carregar o dicionário com as extensões padrões do UniNFe antes de carregar as threads dos serviços para evitar erro com a chave principal do dicionário,
                //caso duas threads tentem carregar ao mesmo tempo.
                Propriedade.Extensao(NFe.Components.Propriedade.TipoEnvio.AltCon);
                
                Application.Run(new NFe.UI.Form_Main());
            }
#if DEBUG
            NFe.Components.NativeMethods.FreeConsole();
#endif
        }

    }
}

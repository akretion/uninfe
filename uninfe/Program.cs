using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using NFe.Components;
using NFe.Components.Info;

namespace uninfe
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            //Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727", false);
            Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP", false);
            string[] v = Key.GetSubKeyNames();
            foreach (string s in v)
                Console.WriteLine(s);
            */

            //Esta deve ser a primeira linha do Main, não coloque nada antes dela. Wandrey 31/07/2009
            Propriedade.AssemblyEXE = Assembly.GetExecutingAssembly();

            if (Aplicacao.AppExecutando())
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

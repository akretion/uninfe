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
            //Esta deve ser a primeira linha do Main, não coloque nada antes dela. Wandrey 31/07/2009
            Propriedade.AssemblyEXE = Assembly.GetExecutingAssembly();

            bool silencioso = false;
            ConfiguracaoApp.AtualizaWSDL = false;

            if(args.Length >= 1)
                foreach(string param in args)
                {
                    if(param.ToLower().Equals("/silent"))
                    {
                        silencioso = true;
                        continue;
                    }
                    if(param.ToLower().Equals("/updatewsdl"))
                    {
                        ConfiguracaoApp.AtualizaWSDL = true;
                        continue;
                    }
                }


            if(Aplicacao.AppExecutando(silencioso))
            {
                return;
            }

            //#if DEBUG
            //            NFe.Components.NativeMethods.AllocConsole();
            //            Console.WriteLine("UNINFE: Debug Console");
            //#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //#if DEBUG
            //            NFe.Components.NativeMethods.FreeConsole();
            //#endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;

using NFe.Components;
using NFe.Components.Info;
using NFe.Settings;
using NFe.Threadings;

namespace UniNFeServico
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Unimake.Business.DFe.Xml.AssemblyResolver.AssemblyResolve;

            //Esta deve ser a primeira linha do Main, não coloque nada antes dela. Wandrey 31/07/2009
            Propriedade.AssemblyEXE = Assembly.GetExecutingAssembly();

            System.Threading.Mutex oneMutex = null;

            if (Aplicacao.UniNFeSevicoAppExecutando())
            {
                return;
            }

            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]{ new UniNFeService() };
                ServiceBase.Run(ServicesToRun);
            }
            finally
            {
                if (oneMutex != null)
                    oneMutex.Close();
            }
        }
    }
}

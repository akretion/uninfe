using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

using NFe.Components;
using NFe.Components.Info;
using NFe.Settings;

namespace uninfe2
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
                Auxiliar.WriteLog(e.Exception.Message + "\r\n" + e.Exception.StackTrace);
                if (e.Exception.InnerException != null)
                {
                    Auxiliar.WriteLog(e.Exception.InnerException.Message + "\r\n" + e.Exception.InnerException.StackTrace);

                    if (e.Exception.InnerException.InnerException != null)
                    {
                        Auxiliar.WriteLog(e.Exception.InnerException.InnerException.Message + "\r\n" + e.Exception.InnerException.InnerException.StackTrace);
                    }

                    if (e.Exception.InnerException.InnerException.InnerException != null)
                    {
                        Auxiliar.WriteLog(e.Exception.InnerException.InnerException.InnerException.Message + "\r\n" + e.Exception.InnerException.InnerException.InnerException.StackTrace);
                    }
                }
#if false
                ///
                /// executando por servico?
                if (Propriedade.ServicoRodando)
                    return;

                if (NFe.UI.uninfeDummy.mainForm == null)
                    return;

                if (NFe.UI.uninfeDummy.mainForm.WindowState != FormWindowState.Minimized &&
                    NFe.UI.uninfeDummy.showError)
                {
                    MetroFramework.MetroMessageBox.Show(NFe.UI.uninfeDummy.mainForm, e.Exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
#endif
            });

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, e) =>
            {
                Auxiliar.WriteLog(e.ExceptionObject.ToString());
            });

            //NFe.UI.uninfeDummy.showError = true;
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

            Propriedade.TipoAplicativo = (Propriedade.NomeAplicacao.ToLower().Equals("uninfe") ? TipoAplicativo.Nfe : TipoAplicativo.Nfse);

#if DEBUG
            NFe.Components.NativeMethods.AllocConsole();
            Console.WriteLine("start....." + Propriedade.NomeAplicacao);

            try
            {
                XmlDocument wsdlDoc = new XmlDocument();
                wsdlDoc.Load(@"E:\Usr\NFe\uninfe\b_uninfe\uninfe\bin\Release\WSDL\Homologacao\GO\HGORecepcaoEvento.wsdl");
                wsdlDoc.Load(@"E:\Usr\NFe\uninfe\b_uninfe\uninfe\bin\Release\WSDL\Homologacao\SP\HSPNFeRecepcaoEvento_200.wsdl");
                //Console.WriteLine(wsdlDoc.InnerXml);
                foreach (var n in wsdlDoc.ChildNodes)
                {
                    if (n is XmlElement)
                    {
                        foreach (var m in ((System.Xml.XmlElement)n).ChildNodes)
                        {
                            Console.WriteLine(m.ToString());
                            if (m is XmlElement)
                                if (((System.Xml.XmlElement)m).Attributes.Count > 0)
                                    Console.WriteLine(((System.Xml.XmlElement)m).Name + "..." + ((System.Xml.XmlElement)m).Attributes[0].Value);
                        }
                    }
                }
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(wsdlDoc.NameTable);
                nsMgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

                XmlNode node = wsdlDoc.SelectSingleNode("//xmlns:s/wsdl:definitions/wsdl:service", nsMgr);
                if (node != null)
                {
                    string description = node.InnerText;

                    Console.WriteLine("found: " + description);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("-----------------------------------------------");


#endif


            bool executando = Aplicacao.AppExecutando(!silencioso);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (executando)
            {
                MetroFramework.MetroMessageBox.Show(null, 
                    "Somente uma instância do " + Propriedade.NomeAplicacao + " pode ser executada.", "", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
                if (Empresas.ExisteErroDiretorio)
                    MetroFramework.MetroMessageBox.Show(null,
                            "Ocorreu um erro ao efetuar a leitura das configurações da empresa. " +
                            "Por favor entre na tela de configurações da(s) empresa(s) listada(s) abaixo na aba \"Pastas\" e reconfigure-as.\r\n\r\n" + Empresas.ErroCaminhoDiretorio, "", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

            Application.Run(new NFe.UI.Form_Main());

#if DEBUG
            NFe.Components.NativeMethods.FreeConsole();
#endif
        }

    }
}

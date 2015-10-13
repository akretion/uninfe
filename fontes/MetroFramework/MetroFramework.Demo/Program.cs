using System;
using System.Windows.Forms;

namespace MetroFramework.Demo
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            NFe.Components.NativeMethods.AllocConsole();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
#if DEBUG
            NFe.Components.NativeMethods.FreeConsole();
#endif
        }
    }
}

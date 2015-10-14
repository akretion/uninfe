using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormSpinner : Form
    {
        private string Mensagem = "Carregando, aguarde...";
        public FormSpinner(string mensagem)
        {
            InitializeComponent();
           
            if (!String.IsNullOrEmpty(mensagem))
            {
                Mensagem = mensagem;
            }

        }

        private void FormSpinner_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = Mensagem;
        }
    }

    /// <summary>
    /// Classe que exibe o ícone de espera do componente
    /// </summary>
    public static class Wait
    {
        static FormSpinner wait = null;
        static Thread t = null;
        /// <summary>
        /// Exibe o form de espera
        /// </summary>
        /// <param name="owner">Fomulário que será o pai deste componente</param>
        public static void Show(string mensagem)
        {
            if (wait != null && wait.Visible) return;

            //-------------------------------------------------------------------------
            // Garantir que existirá sempre apenas uma chamada
            //-------------------------------------------------------------------------
            //MessageBox.OnBeforeShowMessageBox -= new MessageBox.ShowMessageHandler(MessageBox_OnBeforeShowMessageBox);

            //assim evita-se o congelamento do componente
            t = new Thread(new ThreadStart(delegate()
            {
                //MessageBox.OnBeforeShowMessageBox += new MessageBox.ShowMessageHandler(MessageBox_OnBeforeShowMessageBox);
                wait = new FormSpinner(mensagem);
                Application.Run(wait);
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            Thread.Sleep(100);
        }

        static void MessageBox_OnBeforeShowMessageBox(ref bool cancel)
        {
            Close();
        }

        /// <summary>
        /// Fecha a espera
        /// </summary>
        public static void Close()
        {
            if (wait == null || wait.IsDisposed) return;

            wait.Invoke((MethodInvoker)delegate
            {
                wait.Close();
                wait.Dispose();
                Application.DoEvents();
            });

            t.Abort();
            GC.Collect();
        }
    }
}

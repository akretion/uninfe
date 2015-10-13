using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormWait : MetroFramework.Forms.MetroForm
    {
        public FormWait()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uninfeDummy.ClearControls(this, true, true);
            this.metroLabel6.Text = mensagem;

            NFe.Settings.Auxiliar.WriteLog(this.metroLabel6.Text, false);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Timer t = new Timer();
            t.Interval = 500;
            t.Tick += delegate { 
                t.Stop();
                try
                {
                    if (acao1 != null)
                    {
                        acao1();
                        this.metroLabel6.Text = "Iniciando os serviços";
                        this.metroLabel6.Update();
                        acao2();
                    }
                    else
                    {
                        if (stopservice)
                            NFe.Components.ServiceProcess.StopService(servicename, 40000);
                        else
                            NFe.Components.ServiceProcess.RestartService(servicename, 40000);
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(null, ex.Message, "");
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            };
            t.Start();
        }

        public Action acao1 { private get; set; }
        public Action acao2 { private get; set; }
        public bool stopservice { private get; set; }
        public string servicename { private get; set; }
        public string mensagem { private get; set; }
    }
}

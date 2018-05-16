using MetroFramework.Forms;
using System;
using System.Windows.Forms;
using uninfe_ws;

namespace NFe.UI
{
    public partial class Form_Main : MetroForm
    {
        public Form_Main()
        {
            InitializeComponent();
        }

        uninfe_ws.UserControl3 uc1;
        uninfe_ws.UserControl3 uc2;
        uninfe_ws.UserControl3 uc3;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bool ask1 = true;
            bool ask2 = true;
            string fn = System.IO.Path.Combine(Application.StartupPath, "uninfe_ws.xml");
            NFe.Components.XMLIniFile xml = new Components.XMLIniFile(fn);
            
            ask1 = !System.IO.File.Exists(xml.ReadValue("webservice", "uninfe", ""));
            ask2 = !System.IO.File.Exists(xml.ReadValue("webservice", "uninfse", ""));

            if (ask1)
                using (OpenFileDialog fo = new OpenFileDialog())
                {
                    fo.InitialDirectory = Application.StartupPath;
                    fo.Title = "Selecione a configuração do UniNF-e";
                    fo.Filter = "Arquivo de configuração do UniNF-e|webservice.xml";
                    if (fo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ask1 = false;
                        xml.WriteValue("webservice", "uninfe", fo.FileName);
                        xml.Save();
                    }
                    else
                        ask2 = false;
                }

            if (ask2)
                using (OpenFileDialog fo = new OpenFileDialog())
                {
                    fo.InitialDirectory = Application.StartupPath;
                    fo.Title = "Selecione a configuração do UniNFS-e";
                    fo.Filter = "Arquivo de configuração do UniNFS-e|webservice.xml";
                    if (fo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        xml.WriteValue("webservice", "uninfse", fo.FileName);
                        xml.Save();
                        ask2 = false;
                    }
                }

            if (ask1 || ask2)
            {
                MetroFramework.MetroMessageBox.Show(null, "Você precisa definir quais os arquivos webservice.xml","Erro");
                Application.Exit();
                return;
            }

            uc1 = new UserControl3();
            uc1.Parent = metroTabPage1;
            uc1.loadData(NFe.Components.TipoAplicativo.Nfe, null);
            uc1.Dock = DockStyle.Fill;

            uc2 = new UserControl3();
            uc2.Parent = metroTabPage2;
            uc2.loadData(NFe.Components.TipoAplicativo.Nfse, null);
            uc2.Dock = DockStyle.Fill;

            uc3 = new UserControl3();
            uc3.Parent = metroTabPage3;
            uc3.loadData(NFe.Components.TipoAplicativo.Nfse, "XX");
            uc3.Dock = DockStyle.Fill;
        }
    }


}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class UserControl2 : MetroFramework.Controls.MetroUserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            uninfeDummy.ClearControls(this, true, false);

            switch (uninfeDummy.opServicos)
            {
                case uninfeOpcoes2.opStopServico:
                    this.metroLabel1.Text = "Parando o serviço do " + NFe.Components.Propriedade.NomeAplicacao;
                    break;

                case uninfeOpcoes2.opRestartServico:
                    this.metroLabel1.Text = "Reiniciando o serviço do " + NFe.Components.Propriedade.NomeAplicacao;
                    break;

                case uninfeOpcoes2.opRestartTasks:
                    this.metroLabel1.Text = "Parando/Executando os serviços";
                    break;
            }
            
            this.Invalidate();

            BackgroundWorker bm = new BackgroundWorker();
            bm.DoWork += (_sender, _e) => 
                {
                    switch (uninfeDummy.opServicos)
                    {
                        case uninfeOpcoes2.opStopServico:
                            uninfeDummy.mainForm.PararServicos(true);
                            break;

                        case uninfeOpcoes2.opRestartServico:
                            uninfeDummy.mainForm.ExecutaServicos(false);
                            break;

                        case uninfeOpcoes2.opRestartTasks:
                            uninfeDummy.mainForm._RestartServicos();
                            break;

                        default:
                            throw new Exception("Opcao invalida");
                    }
                    System.Threading.Thread.Sleep(2000);
                    _e.Result = true;
                };
            bm.RunWorkerCompleted += (_sender, _e) =>
                {
                    ((BackgroundWorker)_sender).Dispose();
                    
                    MetroFramework.Forms.MetroTaskWindow.ForceClose();

                    if (_e.Error != null)
                    {
                        MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, _e.Error.Message, "");
                    }
                    else
                    {
                        switch (uninfeDummy.opServicos)
                        {
                            case uninfeOpcoes2.opStopServico:
                                uninfeDummy.mainForm.updateControleDoServico();
                                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                    "Serviço do " + NFe.Components.Propriedade.NomeAplicacao + " parado com sucesso!", "",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case uninfeOpcoes2.opRestartServico:
                                uninfeDummy.mainForm.updateControleDoServico();
                                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, 
                                    "Serviço do " + NFe.Components.Propriedade.NomeAplicacao + " reiniciado com sucesso!", "", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                    }
                };

            bm.RunWorkerAsync();
        }
    }
}

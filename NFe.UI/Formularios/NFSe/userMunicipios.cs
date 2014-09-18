using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NFe.Components;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userMunicipios : UserControl1
    {
        private NFSe.userGrid1 grid1;
        private NFSe.userGrid2 grid2;
        private Timer timerRefresh;

        public userMunicipios()
        {
            InitializeComponent();
        }

        void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid1.metroGrid1.Tag = metroTabControl1.SelectedIndex;

            if (metroTabControl1.SelectedIndex == 1)
                this.grid2.RefreshMunicipiosDefinidos();
            else
                this.grid1.RefreshMunicipiosADefinir();
        }

        public override void UpdateControles()
        {
            base.UpdateControles();

            if (grid1 == null)
            {
                this.metroTabControl1.SelectedIndex = 0;

                grid1 = new NFSe.userGrid1();
                grid1.Parent = this.metroTabPage1;
                grid1.Location = new Point(1, 1);
                grid1.Dock = DockStyle.Fill;
                grid1.umunicipio = this;

                grid2 = new NFSe.userGrid2();
                grid2.Parent = this.metroTabPage2;
                grid2.Location = new Point(1, 1);
                grid2.Dock = DockStyle.Fill;

                this.metroTabControl1.SelectedIndexChanged += metroTabControl1_SelectedIndexChanged;

                //grid1.metroGrid1.BackgroundColor =
                    //grid2.dgvDireto.BackgroundColor = MetroFramework.Drawing.MetroPaint.BackColor.Form(uninfeDummy.mainForm.uTheme);
            }
        }

        public void RefreshGrid2()
        {
            ///
            /// a criacao de um timer foi um arremedo que fiz para que o municipio seja excluido da grade
            /// tentei excluir diretamente ou dar um rebuild, mas este componente dá um erro de recursividade.
            /// 
            timerRefresh = new Timer();
            timerRefresh.Tick += delegate
            {
                timerRefresh.Stop();
                timerRefresh.Dispose();

                this.grid2.RefreshMunicipiosDefinidos();
            };
            timerRefresh.Start();
        }
    }
}

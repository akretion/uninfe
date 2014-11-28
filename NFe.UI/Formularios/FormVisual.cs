using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormVisual : MetroFramework.Forms.MetroForm
    {
        public FormVisual()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControls();
        }

        private void UpdateControls()
        {
            /*
            this.metroLabel2.Style = 
                this.metroButton2.Style =
                this.pnlOptions.Style = 
                this.Style = uninfeDummy.mainForm.uStyle;
            this.metroLabel2.Theme =
                this.metroButton2.Theme =
                this.pnlOptions.Theme = 
                this.Theme = uninfeDummy.mainForm.uTheme;
            */
            this.metroTile_back.Text = "Branco";
            this.Refresh();
        }

        private void mtYellow_Click(object sender, EventArgs e)
        {
            //if (!uninfeDummy.mainForm.uStyle.Equals(((MetroFramework.Controls.MetroTile)sender).Style))
            {
                //uninfeDummy.mainForm.uStyle = ((MetroFramework.Controls.MetroTile)sender).Style;
                //uninfeDummy.mainForm.updateVisual();
                UpdateControls();
            }
        }

        private void metroTile_back_Click(object sender, EventArgs e)
        {
            //uninfeDummy.mainForm.uTheme = MetroThemeStyle.Light;
            //uninfeDummy.mainForm.updateVisual();
            UpdateControls();
        }
    }
}

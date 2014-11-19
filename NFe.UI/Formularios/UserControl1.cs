using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class UserControl1 : MetroFramework.Controls.MetroUserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private bool clearAll = true;
        public virtual void UpdateControles()
        {
            Dock = DockStyle.Fill;
            try
            {
                uninfeDummy.ClearControls(this, clearAll, false);
                clearAll = false;
                try
                {
                    this.StyleManager = ((MetroFramework.Interfaces.IMetroForm)uninfeDummy.mainForm).StyleManager.Clone(this) as MetroFramework.Components.MetroStyleManager;
                }
                catch
                {
                    this.StyleManager = uninfeDummy.mainForm.StyleManager;
                }

                this.pictureBox1.Image = NFe.UI.Properties.Resources.e112_Back_48;
            }
            catch { }
        }

        public PictureBox back_button { get { return this.pictureBox1; } }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (uninfeDummy.mainForm.WindowState == FormWindowState.Minimized)
                return;

            if (this.pictureBox1.Tag == null)
            {
                BackFuncao();
            }
        }

        public void BackFuncao()
        {
            foreach (var uc in uninfeDummy.mainForm.Controls)
            {
                if (uc.GetType().Equals(typeof(menu)))
                {
                    ((menu)uc).Visible = true;
                    continue;
                }
                if (uc is MetroFramework.Controls.MetroUserControl)
                    (uc as MetroFramework.Controls.MetroUserControl).Visible = false;
            }
        }
    }
}

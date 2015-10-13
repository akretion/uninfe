using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class Dialogs : MetroFramework.Forms.MetroForm
    {
        public Dialogs()
        {
            InitializeComponent();
            if (!DesignMode && uninfeDummy.mainForm != null)
            {
                uninfeDummy.ClearControls(this, true, true);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Text = NFe.Components.Propriedade.NomeAplicacao;
            base.OnLoad(e);
        }

        public static void ShowMessage(string msg, int w, int h, System.Windows.Forms.MessageBoxIcon iconStyle, ScrollBars scrollBars = ScrollBars.None)
        {
            using (Dialogs form = new Dialogs())
            {
                form.btnYes.Visible = false;

                if (iconStyle == MessageBoxIcon.Information)
                    form.pictureBox1.Image = Properties.Resources.information;
                else if (iconStyle == MessageBoxIcon.Warning)
                    form.pictureBox1.Image = Properties.Resources.warning;
                else if (iconStyle == MessageBoxIcon.Error)
                    form.pictureBox1.Image = Properties.Resources.error;
                else
                    form.pictureBox1.Visible = false;

                form.metroTextBox3.Text = msg;
                form.metroTextBox3.ScrollBars = scrollBars;
                if (w > 0)
                    form.Width = w;
                if (h > 0)
                    form.Height = Math.Max(150, h);
                form.ShowDialog();
            }
        }

        public static bool YesNo(string msg, int w=0, int h=0)
        {
            using (Dialogs form = new Dialogs())
            {
                form.btnNo.Text = "Não";
                form.pictureBox1.Image = Properties.Resources.about;
                form.metroTextBox3.Text = msg;
                if (w > 0)
                    form.Width = w;
                if (h > 0)
                    form.Height = Math.Max(150, h);
                return form.ShowDialog() == DialogResult.Yes;
            }
        }
    }
}

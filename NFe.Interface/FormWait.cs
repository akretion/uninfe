using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NFe.Interface
{
    public partial class FormWait : Form
    {
        public FormWait()
        {
            InitializeComponent();

            this.label1.Text = "Espere...";
            this.progressBar1.Style = ProgressBarStyle.Marquee;
        }

        public void StopMarquee()
        {
            this.progressBar1.MarqueeAnimationSpeed = 0;
        }

        public void DisplayMessage(string msg)
        {
            this.label1.Text = msg;
            this.label1.Update();
            Application.DoEvents();
        }
    }
}

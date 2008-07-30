using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace uninfe
{
    public partial class FormValidarXML : Form
    {
        public FormValidarXML()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog_arqxml.FileName = "";
            this.openFileDialog_arqxml.Filter = "Arquivos XML|*.xml";
            if (this.openFileDialog_arqxml.ShowDialog() == DialogResult.OK)
            {
                this.textBox_arqxml.Text = this.openFileDialog_arqxml.FileName.ToString();
            }
        }
    }
}

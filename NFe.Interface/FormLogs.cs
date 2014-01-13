using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.Interface
{
    public partial class FormLogs : Form
    {
        public FormLogs()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }

        private void FormLogs_Load(object sender, EventArgs e)
        {
            this.textBox1.Dock = DockStyle.Fill;
            this.textBox1.Text = "";

            try
            {
                string[] files = System.IO.Directory.GetFiles(NFe.Components.Propriedade.PastaLog, "*.log");
                foreach (string filename in files)
                {
                    this.cbArquivos.Items.Add(System.IO.Path.GetFileName(filename));
                }
                if (cbArquivos.Items.Count > 0)
                {
                    cbArquivos.Enabled = true;
                    cbArquivos.SelectedIndex = 0;
                }
                else
                    cbArquivos.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbArquivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Text = System.IO.File.ReadAllText(System.IO.Path.Combine(NFe.Components.Propriedade.PastaLog, cbArquivos.Text));
                this.textBox1.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                this.textBox1.BackColor = Color.Red;
                this.textBox1.Text = ex.Message;
            }
        }
    }
}

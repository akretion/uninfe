using System;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class userLogs : UserControl1
    {
        public userLogs()
        {
            InitializeComponent();
        }

        public override void UpdateControles()
        {
            base.UpdateControles();

            this.metroTextBox3.Text = "";
            this.cbArquivos.Items.Clear();

            string[] files = System.IO.Directory.GetFiles(NFe.Components.Propriedade.PastaLog, "*.log");
            foreach (string filename in files)
            {
                this.cbArquivos.Items.Add(System.IO.Path.GetFileName(filename));
            }
            if (cbArquivos.Items.Count > 0)
            {
                cbArquivos.Enabled = true;
                string fileName = "uninfe_" + DateTime.Now.ToString("yyyy-MMM-dd") + ".log";
                int pos;
                if ((pos = this.cbArquivos.Items.IndexOf(fileName)) >= 0)
                    cbArquivos.SelectedIndex = pos;
                else
                    cbArquivos.SelectedIndex = 0;
            }
            else
                cbArquivos.Enabled = false;
        }

        private void metroComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.metroTextBox3.Text = System.IO.File.ReadAllText(System.IO.Path.Combine(NFe.Components.Propriedade.PastaLog, cbArquivos.Text), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                this.metroTextBox3.Text = ex.Message;
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Confirma a exclusão deste arquivo de log?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var fn = System.IO.Path.Combine(NFe.Components.Propriedade.PastaLog, cbArquivos.Text);
                if (System.IO.File.Exists(fn))
                    System.IO.File.Delete(fn);

                this.UpdateControles();
            }
        }
    }
}
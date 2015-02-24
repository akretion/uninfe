using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace uninfe_ws
{
    public partial class UserControl2 : MetroFramework.Controls.MetroUserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        private string _folder;
        public string folder {
            get { return _folder; }
            set { _folder = value; }
        }

        private void metroTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MetroFramework.Controls.MetroTextBox control = (MetroFramework.Controls.MetroTextBox)sender;
                int x = control.ClientRectangle.Width - control.Icon.Size.Width;
                if (e.Location.X >= x)  // a imagem foi pressionada?
                {
                    using (OpenFileDialog folderBrowserDialog1 = new OpenFileDialog())
                    {
                        folderBrowserDialog1.Filter = "Arquivo wsdl (*.wsdl)|*.wsdl";
                        if (!string.IsNullOrEmpty(control.Text))
                        {
                            string[] s = control.Text.Split(new char[] { '\\' });
                            if (s.Length>2)
                                // tira a pasta 'wsdl'
                                folderBrowserDialog1.InitialDirectory = folder + "\\" + s[s.Length - 2];
                        }
                        else
                            folderBrowserDialog1.InitialDirectory = folder;

                        folderBrowserDialog1.FileName = System.IO.Path.GetFileName(control.Text);

                        folderBrowserDialog1.Title = folderBrowserDialog1.InitialDirectory;

                        if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                        {
                            control.Text = getfile(System.IO.Path.GetFullPath(folderBrowserDialog1.FileName));
                        }
                        control.Focus();
                        control.SelectAll();
                    }
                }
            }
        }

        private void metroTextBox1_Leave(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroTextBox control = (MetroFramework.Controls.MetroTextBox)sender;
            control.Text = getfile(control.Text);
        }

        private string getfile(string value)
        {
            if (value.ToUpper().StartsWith("NFE\\")) value = value.Substring(4);
            if (value.ToUpper().StartsWith("NFSE\\")) value = value.Substring(5);

            string[] s = value.Split(new char[] { '\\' });
            if (s.Length > 3)
            {
                value = "";
                for (int n = s.Length - 4; n < s.Length; ++n)
                {
                    value += "\\" + s[n];
                }
                return value.Substring(1);
            }
            return value;
        }
    }
}

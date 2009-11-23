using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UniNFeLibrary;
using System.Reflection;

namespace unicte
{
    public partial class FormSobre : Form
    {
        public FormSobre()
        {
            InitializeComponent();

            this.textBox_versao.Text = InfoApp.Versao();

            //Atualizar o texto da licença de uso

            this.textBox_licenca.Text  = "UniCTe – Monitor de Conhecimentos de Transportes Eletrônicos\r\n\r\n";
            this.textBox_licenca.Text += "Todos os direitos reservados para Unimake Soluções Corporativas LTDA.\r\n\r\n";
            this.textBox_licenca.Text += "Este software pode ser utilizado gratuitamente no \"ambiente de homologação\" da Secretaria da Fazenda, todavia sua licença expira se for utilizado no \"ambiente de produção\" do orgão citado.\r\n\r\n";
            this.textBox_licenca.Text += "Para adquirir a licença para uso no ambiente de produção acesse o site www.unimake.com.br/unicte";

            textBox_DataUltimaModificacao.Text = File.GetLastWriteTimeUtc("unicte.exe").ToString("dd/MM/yyyy - hh:mm:ss");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.unimake.com.br");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.unimake.com.br/unicte");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:nfe@unimake.com.br");
        }

        private void btnManualUniCTe_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\UniCTe.pdf"))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\UniCTe.pdf");
                }
                else
                {
                    MessageBox.Show("Não foi possível localizar o arquivo de manual do UniCTe.","Erro",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Erro",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

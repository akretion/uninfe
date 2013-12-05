using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NFe.Components;
using NFe.Settings;

namespace NFe.Interface
{
    public partial class FormSobre : Form
    {
        public FormSobre()
        {
            InitializeComponent();

            switch (NFe.Components.Propriedade.TipoAplicativo)
            {
                case NFe.Components.TipoAplicativo.Nfe:
                    this.pictureBox1.Image = global::NFe.Interface.Properties.Resources.uninfe128;
                    this.Icon = global::NFe.Interface.Properties.Resources.uninfe;
                    break;

                case NFe.Components.TipoAplicativo.Nfse:
                    this.pictureBox1.Image = global::NFe.Interface.Properties.Resources.uninfse128;
                    this.Icon = global::NFe.Interface.Properties.Resources.uninfse;
                    break;
            }
            

            this.textBox_versao.Text = Propriedade.Versao;
            lblDescricaoAplicacao.Text = Propriedade.DescricaoAplicacao;
            lblNomeAplicacao.Text = Propriedade.NomeAplicacao;

            //Atualizar o texto da licença de uso
            this.textBox_licenca.Text = "GNU General Public License\r\n\r\n";
            this.textBox_licenca.Text += Propriedade.NomeAplicacao + " - " + Propriedade.DescricaoAplicacao + "\r\n";
            this.textBox_licenca.Text += "Copyright (C) 2008 " + ConfiguracaoApp.NomeEmpresa + "\r\n\r\n";
            this.textBox_licenca.Text += "Este programa é software livre; você pode redistribuí-lo e/ou modificá-lo sob os termos da Licença Pública Geral GNU, conforme publicada pela Free Software Foundation; tanto a versão 2 da Licença como (a seu critério) qualquer versão mais nova.\r\n\r\n";
            this.textBox_licenca.Text += "Este programa é distribuído na expectativa de ser útil, mas SEM QUALQUER GARANTIA; sem mesmo a garantia implícita de COMERCIALIZAÇÃO ou de ADEQUAÇÃO A QUALQUER PROPÓSITO EM PARTICULAR. Consulte a Licença Pública Geral GNU para obter mais detalhes.\r\n\r\n";
            this.textBox_licenca.Text += "Você deve ter recebido uma cópia da Licença Pública Geral GNU junto com este programa; se não, escreva para a Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA     02111-1307, USA ou consulte a licença oficial em http://www.gnu.org/licenses/.";

            textBox_DataUltimaModificacao.Text = File.GetLastWriteTime(Propriedade.NomeAplicacao+".exe").ToString("dd/MM/yyyy - HH:mm:ss");

            lblEmpresa.Text = ConfiguracaoApp.NomeEmpresa;
            linkLabelSite.Text = ConfiguracaoApp.Site;
            linkLabelSiteProduto.Text = ConfiguracaoApp.SiteProduto;
            linkLabelEmail.Text = ConfiguracaoApp.Email;
        }

        private void linkLabelSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + ConfiguracaoApp.Site);
        }

        private void linkLabelSiteProduto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + ConfiguracaoApp.SiteProduto);
        }

        private void linkLabelEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + ConfiguracaoApp.Email);
        }

        private void btnManualUniNFe_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\UniNFe.pdf"))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\UniNFe.pdf");
                }
                else
                {
                    MessageBox.Show("Não foi possível localizar o arquivo de manual do UniNFe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

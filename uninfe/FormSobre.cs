using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace uninfe
{
    public partial class FormSobre : Form
    {
        public FormSobre()
        {
            InitializeComponent();

            //Montar a versão do programa
            Assembly objAssembly = Assembly.GetExecutingAssembly();

            string versao;
        
            foreach (Attribute attr in Attribute.GetCustomAttributes(objAssembly)) 
            { 
                if (attr.GetType() == typeof(AssemblyVersionAttribute)) 
                { 
                    versao = ((AssemblyVersionAttribute)attr).Version; 
                } 
            }
            string delimStr = ",=";
            char[] delimiter = delimStr.ToCharArray();
            string[] strAssembly = objAssembly.ToString().Split(delimiter);
            versao = strAssembly[2]; 

            this.textBox_versao.Text = versao.ToString();

            //Atualizar o texto da licença de uso
            this.textBox_licenca.Text  = "GNU General Public License\r\n\r\n";
            this.textBox_licenca.Text += "UniNFe – Monitor de Notas Fiscais Eletrônicas\r\n";
            this.textBox_licenca.Text += "Copyright (C) 2008 Unimake Soluções Corporativas LTDA\r\n\r\n";
            this.textBox_licenca.Text += "Este programa é software livre; você pode redistribuí-lo e/ou modificá-lo sob os termos da Licença Pública Geral GNU, conforme publicada pela Free Software Foundation; tanto a versão 2 da Licença como (a seu critério) qualquer versão mais nova.\r\n\r\n";
            this.textBox_licenca.Text += "Este programa é distribuído na expectativa de ser útil, mas SEM QUALQUER GARANTIA; sem mesmo a garantia implícita de COMERCIALIZAÇÃO ou de ADEQUAÇÃO A QUALQUER PROPÓSITO EM PARTICULAR. Consulte a Licença Pública Geral GNU para obter mais detalhes.\r\n\r\n";
            this.textBox_licenca.Text += "Você deve ter recebido uma cópia da Licença Pública Geral GNU junto com este programa; se não, escreva para a Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA     02111-1307, USA ou consulte a licença oficial em http://www.gnu.org/licenses/.";

            textBox_DataUltimaModificacao.Text = File.GetLastWriteTimeUtc("uninfe.exe").ToString("dd/MM/yyyy - hh:mm:ss");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.unimake.com.br");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.unimake.com.br/uninfe");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:nfe@unimake.com.br");
        }
/*
        private static void AssemblyPropriedades( string product, ref string version) 
        { 
            try 
            { 
                Assembly objAssembly = Assembly.GetExecutingAssembly(); 

                foreach (Attribute attr in Attribute.GetCustomAttributes(objAssembly)) 
                { 
                    if (attr.GetType() == typeof(AssemblyProductAttribute)) 
                    { 
                        product = ((AssemblyProductAttribute)attr).Product; 
                    } 
                    if (attr.GetType() == typeof(AssemblyVersionAttribute)) 
                    { 
                        version = ((AssemblyVersionAttribute)attr).Version; 
                    } 
                } 
                string delimStr = ",="; 
                char[] delimiter = delimStr.ToCharArray(); 
                string[] strAssembly = objAssembly.ToString().Split(delimiter); 
                version = strAssembly[2]; 
            }
            catch (Exception ex) 
            { 
                Log(ex); 
            } 
        }
 */ 
    }
}

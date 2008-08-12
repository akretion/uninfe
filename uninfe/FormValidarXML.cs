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
        ValidadorXMLClass oValidarXML = new ValidadorXMLClass();

        public FormValidarXML()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog_arqxml.FileName = "";
            this.openFileDialog_arqxml.RestoreDirectory = true; //Para não mudar o diretório corrente da aplicação. Wandrey 06/08/2008
            this.openFileDialog_arqxml.Filter = "Arquivos XML|*.xml";
            if (this.openFileDialog_arqxml.ShowDialog() == DialogResult.OK)
            {
                this.textBox_arqxml.Text = this.openFileDialog_arqxml.FileName.ToString();

                oValidarXML.TipoArquivoXML(this.textBox_arqxml.Text);
                this.textBox_tipoarq.Text = oValidarXML.cRetornoTipoArq;
            }
            
            this.textBox_resultado.Text = "";
        }

        private void toolStripButton_validar_Click(object sender, EventArgs e)
        {
            this.textBox_resultado.Text = "";

            oValidarXML.TipoArquivoXML(this.textBox_arqxml.Text);

            if (oValidarXML.nRetornoTipoArq >= 1 && oValidarXML.nRetornoTipoArq <= 7)
            {
                oValidarXML.ValidarXML(this.textBox_arqxml.Text, oValidarXML.cArquivoSchema);
                if (oValidarXML.Retorno == 0)
                {
                    this.textBox_resultado.Text = "Arquivo validado com sucesso;";
                }
                else
                {
                    this.textBox_resultado.Text = "XML INCONSISTENTE!\r\n\r\n" + oValidarXML.RetornoString;
                }
            }
            else
            { 
                this.textBox_tipoarq.Text   = oValidarXML.cRetornoTipoArq;
                this.textBox_resultado.Text = "XML INCONSISTENTE!\r\n\r\n" + oValidarXML.cRetornoTipoArq;
            }
        }
    }
}

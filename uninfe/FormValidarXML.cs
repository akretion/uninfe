using System;
using System.IO;
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

            //Copiar o arquivo XML para temporários para assinar e depois vou validar o que está nos temporários
            UniNfeClass oNfe = new UniNfeClass();
            FileInfo oArquivo = new FileInfo(this.textBox_arqxml.Text);
            string cArquivo = System.IO.Path.GetTempPath() + oNfe.ExtrairNomeArq(this.textBox_arqxml.Text, ".xml");

            FileInfo oArqDestino = new FileInfo(cArquivo);

            if (File.Exists(cArquivo)) //Deletar o arquivo antes de copiar se existir na pasta de temporários
            {
                oArqDestino.Delete();
            }

            oArquivo.CopyTo(cArquivo);

            //Detectar o tipo do arquivo
            oValidarXML.TipoArquivoXML(cArquivo);

            //Assinar o arquivo XML copiado para a pasta TEMP
            bool lAssinar = false;
            string cTagAssinar = "";
            switch (oValidarXML.nRetornoTipoArq)
            {
                case 1:
                    lAssinar = true;
                    cTagAssinar = "infNFe";
                    break;

                case 3:
                    lAssinar = true;
                    cTagAssinar = "infCanc";
                    break;

                case 4:
                    lAssinar = true;
                    cTagAssinar = "infInut";
                    break;                    
            }

            bool lValidar = false;
            if (lAssinar == true)
            {
                ConfigUniNFe oConfig = new ConfigUniNFe();
                oConfig.CarregarDados();

                UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();
                oAD.Assinar(cArquivo, cTagAssinar, oConfig.oCertificado);
                if (oAD.vResultado == 0)
                {
                    lValidar = true;
                }
                else
                {
                    lValidar = false;
                    this.textBox_tipoarq.Text = oValidarXML.cRetornoTipoArq;
                    this.textBox_resultado.Text = "Ocorreu um erro ao tentar assinar o XML: \r\n\r\n" + oAD.vResultado.ToString() + " " + oAD.vResultadoString;
                }
            }
            else
            {
                lValidar = true;
            }

            if (lValidar == true)
            {
                //Validar o arquivo
                if (oValidarXML.nRetornoTipoArq >= 1 && oValidarXML.nRetornoTipoArq <= 7)
                {
                    oValidarXML.ValidarXML(cArquivo, oValidarXML.cArquivoSchema);
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
                    this.textBox_tipoarq.Text = oValidarXML.cRetornoTipoArq;
                    this.textBox_resultado.Text = "XML INCONSISTENTE!\r\n\r\n" + oValidarXML.cRetornoTipoArq;
                }
            }

            oArqDestino.Delete();
        }
    }
}

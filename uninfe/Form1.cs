using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using uninfe;

namespace uninfe
{
    public partial class Form1 : Form
    {
        UniNfeClass oUniNfe = new UniNfeClass();        

        public Form1()
        {
            oUniNfe.SelecionarCertificado();
            InitializeComponent();
        }

        private void button_status_servico_Click(object sender, EventArgs e)
        {
            oUniNfe.vXmlNfeDadosMsg = this.textBox_xmldados.Text;
            oUniNfe.vAmbiente = 2;
            if (this.comboBoxUF.Text == "MT")
            {
                oUniNfe.vUF = 51;
            }
            else if (this.comboBoxUF.Text == "RS")
            {
                oUniNfe.vUF = 43;
            }

            oUniNfe.StatusServico();
            this.textBox_xmlretorno.Text = oUniNfe.vStrXmlRetorno;
        }

        private void button_selecionar_certificado_Click(object sender, EventArgs e)
        {
            oUniNfe.SelecionarCertificado();
        }

        private void button_exibir_certificado_selecionado_Click(object sender, EventArgs e)
        {
            oUniNfe.ExibirCertSel();
        }

        private void comboBoxUF_SelectedIndexChanged(object sender, EventArgs e)
        {
            //oUniNfe.vUF = comboBoxUF.Text;
        }

        private void button_assinarxml_Click(object sender, EventArgs e)
        {
            UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();
            oAD.Assinar(this.textBox_xmldados.Text, "infNFe", oUniNfe.oCertificado);

            if (oAD.vResultado == 0) //Assinado corretamente
            {
                string pDadosNfe = oAD.vXMLStringAssinado;
                string vStringNfe = pDadosNfe.Substring(pDadosNfe.IndexOf("<NFe"), pDadosNfe.Length - pDadosNfe.IndexOf("<NFe"));
                MessageBox.Show(vStringNfe);

                Clipboard.SetText(vStringNfe, TextDataFormat.Text);
                MessageBox.Show(oAD.vXMLStringAssinado);
            }
            else //Ocorreu algum erro na assinatura
            {
                MessageBox.Show(oAD.vResultadoString, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_recepcao_nfe_Click(object sender, EventArgs e)
        {
            oUniNfe.vXmlNfeDadosMsg = this.textBox_xmldados.Text;
            oUniNfe.vAmbiente = 2;
            oUniNfe.vUF = 51;
            oUniNfe.Recepcao();
            this.textBox_xmlretorno.Text = oUniNfe.vStrXmlRetorno;
            MessageBox.Show(oUniNfe.vStrXmlRetorno);
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.textBox_xmldados.Text = this.openFileDialog1.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void button_RetRecepcao_Click(object sender, EventArgs e)
        {
            oUniNfe.vXmlNfeDadosMsg = this.textBox_xmldados.Text;
            oUniNfe.RetRecepcao();
            this.textBox_xmlretorno.Text = oUniNfe.vStrXmlRetorno;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FormConfiguracao oConfig = new FormConfiguracao();
            oConfig.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            UniLerXMLClass oLerNFe = new UniLerXMLClass();
          
            if (oLerNFe.Nfe(this.textBox_xmldados.Text) == true)
            {
                string chavenfe = oLerNFe.oDadosNfe.chavenfe;
                DateTime dEmi = oLerNFe.oDadosNfe.dEmi;
                string tpEmis = oLerNFe.oDadosNfe.tpEmis;
                string tpAmb = oLerNFe.oDadosNfe.tpAmb;

                this.textBox_xmlretorno.Text = "Chave NFe: " + chavenfe + "\r\n" +
                                               "Emissao: " + dEmi + "\r\n" +
                                               "Tipo Emissao: " + tpEmis + "\r\n" +
                                               "Ambiente: " + tpAmb + "\r\n";
            }
            else
            {
                MessageBox.Show("Ocorreu um erro ao tentar ler o XML:\r\n\r\n" + oLerNFe.cMensagemErro);
            }
        }
    }
}


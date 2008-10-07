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
            
            XmlDocument doc=new XmlDocument();
            doc.LoadXml(this.textBox_xmldados.Text);
            
            //Definimos a propriedade No como XmlNode a informarmos o XPath para ela que será cadastro/funcionário
            string nome = doc.SelectSingleNode("//NFe//infNFe//ide").InnerText;

            // Mostramos no navegador o resultado da busca deste path informando o índice deles diretamente. 
            //MessageBox.Show(No.ChildNodes.Item(0).InnerText);
            //MessageBox.Show(No.ChildNodes.Item(1).InnerText);

  
//            System.Xml.XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("BookMark[@name='Mozilla1']");   
//xmlNode points to the required node  

/*
            //Carregar os dados do arquivo XML de configurações do UniNfe
            XmlTextReader oLerXml = null;

            try
            {
                oLerXml = new XmlTextReader(vArquivoConfig);

                while (oLerXml.Read())
                {
                    if (oLerXml.NodeType == XmlNodeType.Element)
                    {
                        if (oLerXml.Name == "NFe")

                        if (oLerXml.Name == "ide")
                        {
                            while (oLerXml.Read())
                            {
                                if (oLerXml.NodeType == XmlNodeType.Element)
                                {
                                    if (oLerXml.Name == "PastaXmlEnvio") { oLerXml.Read(); this.vPastaXMLEnvio = oLerXml.Value; }
                                    else if (oLerXml.Name == "PastaXmlRetorno") { oLerXml.Read(); this.vPastaXMLRetorno = oLerXml.Value; }
                                    else if (oLerXml.Name == "PastaXmlEnviado") { oLerXml.Read(); this.vPastaXMLEnviado = oLerXml.Value; }
                                    else if (oLerXml.Name == "PastaXmlErro") { oLerXml.Read(); this.vPastaXMLErro = oLerXml.Value; }
                                    else if (oLerXml.Name == "UnidadeFederativa") { oLerXml.Read(); this.vUnidadeFederativa = oLerXml.Value; }
                                    else if (oLerXml.Name == "UnidadeFederativaCodigo") { oLerXml.Read(); this.vUnidadeFederativaCodigo = Convert.ToInt32(oLerXml.Value); }
                                    else if (oLerXml.Name == "Ambiente") { oLerXml.Read(); this.vAmbiente = oLerXml.Value; }
                                    else if (oLerXml.Name == "AmbienteCodigo") { oLerXml.Read(); this.vAmbienteCodigo = Convert.ToInt32(oLerXml.Value); }
                                    else if (oLerXml.Name == "CertificadoDigital") { oLerXml.Read(); this.vCertificado = oLerXml.Value; }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (oLerXml != null)
                    oLerXml.Close();
            }         

*/

/*
            XmlTextReader reader = null;

            try
            {
                //Create the XML fragment to be parsed.
//                string xmlFrag = "<book genre='novel' misc='sale-item &h; 1987'></book>";

                StreamReader SR;
                SR = File.OpenText(this.textBox_xmldados.Text);
                string xmlFrag = SR.ReadToEnd();
                SR.Close();

                //Create the XmlParserContext.
                XmlParserContext context;
                string subset = "";
                context = new XmlParserContext(null, null, "NFe", null, null, subset, "", "", XmlSpace.None);

                //Create the reader.
                reader = new XmlTextReader(xmlFrag, XmlNodeType.Element, context);

                //Read the misc attribute. The attribute is parsed
                //into multiple text and entity reference nodes.
                reader.MoveToContent();
                reader.MoveToAttribute("CNPJ");
                while (reader.ReadAttributeValue())
                {
                    if (reader.NodeType == XmlNodeType.EntityReference)
                    {
                        MessageBox.Show("Oi1" + reader.Value);
                    }
                    else
                    {
                        MessageBox.Show("Oi2" + reader.Value);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
 */
        }
    }
}


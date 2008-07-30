using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Security.Cryptography.X509Certificates;

namespace uninfe
{
    public partial class FormConfiguracao : Form
    {
        private X509Certificate2 oMeuCert;

        public FormConfiguracao()
        {

            InitializeComponent();

            //Montar os itens do DropList da UF
            ArrayList arrUF = new ArrayList();
            arrUF.Add(new DropDownListClass("AM", 13));
            arrUF.Add(new DropDownListClass("GO", 52));
            arrUF.Add(new DropDownListClass("MG", 31));
            arrUF.Add(new DropDownListClass("MS", 50));
            arrUF.Add(new DropDownListClass("MT", 51));
            arrUF.Add(new DropDownListClass("PR", 41));
            arrUF.Add(new DropDownListClass("RS", 43));
            arrUF.Add(new DropDownListClass("SC", 42));
            arrUF.Add(new DropDownListClass("SP", 35));
            arrUF.Add(new DropDownListClass("TO", 17));

            comboBox_UF.DataSource = arrUF;
            comboBox_UF.DisplayMember = "nome";
            comboBox_UF.ValueMember = "valor";

            //Montar os itens do DropList do Ambiente
            ArrayList arrAmb = new ArrayList();
            arrAmb.Add(new DropDownListClass("Produção", 1));
            arrAmb.Add(new DropDownListClass("Homologação", 2));

            comboBox_Ambiente.DataSource = arrAmb;
            comboBox_Ambiente.DisplayMember = "nome";
            comboBox_Ambiente.ValueMember = "valor";

            //Carregar os dados gravados no XML das configurações
            CarregarConfiguracoes oCarrega = new CarregarConfiguracoes();

            oCarrega.CarregarDados();

            this.textBox_PastaEnvioXML.Text = oCarrega.vPastaXMLEnvio;
            this.textBox_PastaRetornoXML.Text = oCarrega.vPastaXMLRetorno;
            this.textBox_PastaEnviados.Text = oCarrega.vPastaXMLEnviado;
            this.comboBox_UF.Text = oCarrega.vUnidadeFederativa;            
            this.comboBox_Ambiente.Text = oCarrega.vAmbiente;
            this.oMeuCert = oCarrega.oCertificado;

            this.DemonstraDadosCertificado();
        }

        private void button_selectxmlenvio_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlenvio.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.textBox_PastaEnvioXML.Text = this.folderBrowserDialog_xmlenvio.SelectedPath;
            }
        }

        private void button_SelectPastaXmlRetorno_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlretorno.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.textBox_PastaRetornoXML.Text = this.folderBrowserDialog_xmlretorno.SelectedPath;
            }
        }

        private void button_selecionar_certificado_Click(object sender, EventArgs e)
        {
            UniNfeClass oUniNfe = new UniNfeClass();

            if (oUniNfe.SelecionarCertificado() == true)
            {
                this.oMeuCert = oUniNfe.oCertificado;
                this.DemonstraDadosCertificado();
            }
        }

        private void DemonstraDadosCertificado()
        {
            if (oMeuCert != null)
            {
                this.textBox_dadoscertificado.Text =
                    "[Sujeito]\r\n" + oMeuCert.Subject + "\r\n\r\n" +
                    "[Validade]\r\n" + oMeuCert.NotBefore + " à " + oMeuCert.NotAfter;
            }
        }

        private void toolStripButton_fechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton_salvar_Click(object sender, EventArgs e)
        {
            DirectoryInfo oDirEnvio = new DirectoryInfo(this.textBox_PastaEnvioXML.Text);
            DirectoryInfo oDirRetorno = new DirectoryInfo(this.textBox_PastaRetornoXML.Text);
            DirectoryInfo oDirEnviado = new DirectoryInfo(this.textBox_PastaEnviados.Text);

            if (this.oMeuCert == null)
            {
                MessageBox.Show("Selecione o certificado digital a ser utilizado na autenticação dos serviços da nota fiscal eletrônica.", "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!oDirEnvio.Exists)
            {
                MessageBox.Show("A pasta de envio dos arquivos XML informada não existe.", "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!oDirRetorno.Exists)
            {
                MessageBox.Show("A pasta de retorno dos arquivos XML informada não existe.", "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!oDirEnviado.Exists)
            {
                MessageBox.Show("A pasta dos arquivos XML enviados informada não existe.", "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                XmlWriterSettings oSettings = new XmlWriterSettings();

                //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
                oSettings.Indent = true;
                oSettings.IndentChars = "";
                oSettings.NewLineOnAttributes = false;
                oSettings.OmitXmlDeclaration = false;

                //Agora vamos criar um XML Writer
                XmlWriter oXmlGravar = XmlWriter.Create("UniNfeConfig.xml", oSettings);

                //Agora vamos gravar os dados
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("nfe_configuracoes");
                oXmlGravar.WriteElementString("PastaXmlEnvio", this.textBox_PastaEnvioXML.Text);
                oXmlGravar.WriteElementString("PastaXmlRetorno", this.textBox_PastaRetornoXML.Text);
                oXmlGravar.WriteElementString("PastaXmlEnviado", this.textBox_PastaEnviados.Text);
                oXmlGravar.WriteElementString("UnidadeFederativa", this.comboBox_UF.Text);
                oXmlGravar.WriteElementString("UnidadeFederativaCodigo", this.comboBox_UF.SelectedValue.ToString());
                oXmlGravar.WriteElementString("Ambiente", this.comboBox_Ambiente.Text);
                oXmlGravar.WriteElementString("AmbienteCodigo", this.comboBox_Ambiente.SelectedValue.ToString());
                oXmlGravar.WriteElementString("CertificadoDigital", oMeuCert.Subject.ToString());
                oXmlGravar.WriteEndElement(); //nfe_configuracoes
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();

                this.Dispose();
            }
        }

        private void button_SelectPastaXmlEnviado_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlenviado.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.textBox_PastaEnviados.Text = this.folderBrowserDialog_xmlenviado.SelectedPath;
            }
        }
    }

    public class CarregarConfiguracoes
    {
        public string vPastaXMLEnvio { get; private set; }
        public string vPastaXMLRetorno { get; private set; }
        public string vPastaXMLEnviado { get; private set; }
        public string vUnidadeFederativa { get; private set; }
        public int vUnidadeFederativaCodigo { get; private set; }
        public string vAmbiente { get; private set; }
        public int vAmbienteCodigo { get; private set; }
        public string vCertificado { get; private set; }
        public X509Certificate2 oCertificado { get; private set; }

        public void CarregarDados()
        {
            string vArquivoConfig = "UniNfeConfig.xml";
            this.vPastaXMLEnvio = string.Empty;
            this.vPastaXMLRetorno = string.Empty;
            this.vPastaXMLEnviado = string.Empty;
            this.vUnidadeFederativa = string.Empty;
            this.vUnidadeFederativaCodigo = 0;
            this.vAmbiente = string.Empty;
            this.vAmbienteCodigo = 0;
            this.vCertificado = string.Empty;

            if (File.Exists(vArquivoConfig))
            {
                //Carregar os dados do arquivo XML de configurações do UniNfe
                XmlTextReader oLerXml = new XmlTextReader(vArquivoConfig);

                while (oLerXml.Read())
                {
                    if (oLerXml.NodeType == XmlNodeType.Element)
                    {
                        if (oLerXml.Name == "nfe_configuracoes")
                        {
                            while (oLerXml.Read())
                            {
                                if (oLerXml.NodeType == XmlNodeType.Element)
                                {
                                    if (oLerXml.Name == "PastaXmlEnvio") { oLerXml.Read(); this.vPastaXMLEnvio = oLerXml.Value; }
                                    else if (oLerXml.Name == "PastaXmlRetorno") { oLerXml.Read(); this.vPastaXMLRetorno = oLerXml.Value; }
                                    else if (oLerXml.Name == "PastaXmlEnviado") { oLerXml.Read(); this.vPastaXMLEnviado = oLerXml.Value; }
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
                oLerXml.Close();

                //Ajustar o certificado digital de String para o tipo X509Certificate2
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, this.vCertificado, false);

                if (collection1.Count == 0)
                {
                }
                else
                {
                    this.oCertificado = collection1[0];
                }
            }
        }
    }
}

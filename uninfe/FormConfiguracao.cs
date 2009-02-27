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
            arrUF.Add(new DropDownListClass("AC", 12));
            arrUF.Add(new DropDownListClass("AL", 27));
            arrUF.Add(new DropDownListClass("AP", 16));
            arrUF.Add(new DropDownListClass("AM", 13));
            arrUF.Add(new DropDownListClass("BA", 29));
            arrUF.Add(new DropDownListClass("CE", 23));
            arrUF.Add(new DropDownListClass("DF", 53));
//          arrUF.Add(new DropDownListClass("ES", 32));
            arrUF.Add(new DropDownListClass("GO", 52));
            arrUF.Add(new DropDownListClass("MA", 21));
            arrUF.Add(new DropDownListClass("MG", 31));
            arrUF.Add(new DropDownListClass("MS", 50));
            arrUF.Add(new DropDownListClass("MT", 51));
            arrUF.Add(new DropDownListClass("PA", 15));
            arrUF.Add(new DropDownListClass("PB", 25));
            arrUF.Add(new DropDownListClass("PE", 26));
            arrUF.Add(new DropDownListClass("PI", 22));
            arrUF.Add(new DropDownListClass("PR", 41));
            arrUF.Add(new DropDownListClass("RJ", 33));
            arrUF.Add(new DropDownListClass("RN", 24));
            arrUF.Add(new DropDownListClass("RO", 11));
            arrUF.Add(new DropDownListClass("RR", 14));
            arrUF.Add(new DropDownListClass("RS", 43));
            arrUF.Add(new DropDownListClass("SC", 42));
            arrUF.Add(new DropDownListClass("SE", 28));
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

            //Montar os itens do DropList do Tipo de Emissão da NF-e
            ArrayList arrTpEmis = new ArrayList();
            ArrayList.Synchronized(arrTpEmis);

            arrTpEmis.Add(new DropDownListClass("Normal", 1));
            arrTpEmis.Add(new DropDownListClass("Contingência com formulário de segurança", 2));
            arrTpEmis.Add(new DropDownListClass("Contingência com SCAN do Ambiente Nacional", 3));

            comboBox_tpEmis.DataSource = arrTpEmis;
            comboBox_tpEmis.DisplayMember = "nome";
            comboBox_tpEmis.ValueMember = "valor";

            //Carregar os dados gravados no XML das configurações
            ConfigUniNFe oCarrega = new ConfigUniNFe();

            oCarrega.CarregarDados();

            this.textBox_PastaEnvioXML.Text = oCarrega.vPastaXMLEnvio;
            this.textBox_PastaRetornoXML.Text = oCarrega.vPastaXMLRetorno;
            this.textBox_PastaEnviados.Text = oCarrega.vPastaXMLEnviado;
            this.textBox_PastaXmlErro.Text = oCarrega.vPastaXMLErro;
            this.textBox_Empresa.Text = oCarrega.cNomeEmpresa;
            this.oMeuCert = oCarrega.oCertificado;
            this.DemonstraDadosCertificado();

            if (oCarrega.cPastaBackup == "")
            {
                this.textBox_PastaBackup.Text = "";
            }
            else
            {
                this.textBox_PastaBackup.Text = oCarrega.cPastaBackup;
            }
            //Carregar o conteúdo do droplist do tipo de emissão para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrTpEmis.Count; i++)
			{
                if (((uninfe.DropDownListClass)(new System.Collections.ArrayList(arrTpEmis))[i]).Valor == oCarrega.vTpEmis)
                {
                    this.comboBox_tpEmis.Text = ((uninfe.DropDownListClass)(new System.Collections.ArrayList(arrTpEmis))[i]).Nome;
                    break;
                }
			}

            //Carregar o conteúdo do droplist da Unidade Federativa (UF) para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrUF.Count; i++)
            {
                if (((uninfe.DropDownListClass)(new System.Collections.ArrayList(arrUF))[i]).Valor == oCarrega.vUnidadeFederativaCodigo)
                {
                    this.comboBox_UF.Text = ((uninfe.DropDownListClass)(new System.Collections.ArrayList(arrUF))[i]).Nome;
                    break;
                }
            }

            //Carregar o conteúdo do droplist do Ambiente para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrAmb.Count; i++)
            {
                if (((uninfe.DropDownListClass)(new System.Collections.ArrayList(arrAmb))[i]).Valor == oCarrega.vAmbienteCodigo)
                {
                    this.comboBox_Ambiente.Text = ((uninfe.DropDownListClass)(new System.Collections.ArrayList(arrAmb))[i]).Nome;
                    break;
                }
            }
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
            CertificadoDigitalClass oCertDig = new CertificadoDigitalClass();

            if (oCertDig.SelecionarCertificado() == true)
            {
                this.oMeuCert = oCertDig.oCertificado;
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
            ConfigUniNFe oConfig = new ConfigUniNFe();
            
            oConfig.vPastaXMLEnvio = this.textBox_PastaEnvioXML.Text;
            oConfig.vPastaXMLRetorno = this.textBox_PastaRetornoXML.Text;
            oConfig.vPastaXMLEnviado = this.textBox_PastaEnviados.Text;
            oConfig.vPastaXMLErro = this.textBox_PastaXmlErro.Text;
            oConfig.vUnidadeFederativaCodigo = Convert.ToInt32(this.comboBox_UF.SelectedValue);
            oConfig.vAmbienteCodigo = Convert.ToInt32(this.comboBox_Ambiente.SelectedValue);
            oConfig.vTpEmis = Convert.ToInt32(this.comboBox_tpEmis.SelectedValue);
            oConfig.cNomeEmpresa = this.textBox_Empresa.Text;
            oConfig.cPastaBackup = this.textBox_PastaBackup.Text;
            if (this.oMeuCert == null)
            {
                oConfig.vCertificado = "";
            }
            else
            {
                oConfig.vCertificado = oMeuCert.Subject.ToString();
            }

            if (oConfig.GravarConfig() == false)
            {
                MessageBox.Show(oConfig.cErroGravarConfig, "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //Avisar os serviços do UniNFE que uma configuração nova foi feita e que eles deve recarregar
                //as configurações na memória novamente.
                ServicoUniNFe.CarregarConfiguracoes();

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

        private void button_SelectPastaXmlErro_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlerro.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.textBox_PastaXmlErro.Text = this.folderBrowserDialog_xmlerro.SelectedPath;
            }
        }

        private void button_SelectPastaBackup_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_backup.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.textBox_PastaBackup.Text = this.folderBrowserDialog_backup.SelectedPath;
            }
        }
    }

    /// <summary>
    /// Classe responsável por relalizar algumas tarefas na parte de configurações do uninfe.
    /// Arquivo de configurações: UniNfeConfig.xml
    /// </summary>
    public class ConfigUniNFe
    {
        public string vPastaXMLEnvio { get; set; }
        public string vPastaXMLRetorno { get; set; }
        public string vPastaXMLEnviado { get; set; }
        public string vPastaXMLErro { get; set; }
        public int vUnidadeFederativaCodigo { get; set; }
        public int vAmbienteCodigo { get; set; }
        public int vTpEmis { get; set; }
        public string vCertificado { get; set; }
        public X509Certificate2 oCertificado { get; set; }
        public string cNomeEmpresa { get; set; }
        public string cPastaBackup { get; set; }

        /// <summary>
        /// Recebe uma mensagem de erro caso venha a ocorrer na execução do método "GravarConfig()"
        /// </summary>
        public string cErroGravarConfig { get; private set; }

        /// <summary>
        /// Carrega as configurações realizadas no UniNFe gravadas no XML UniNfeConfig.xml para
        /// propriedades, para facilitar a leitura das informações necessárias para as transações da NF-e.
        /// </summary>
        /// <example>
        /// ConfigUniNFe oConfig = new ConfigUniNFe();
        /// oConfig.CarregarDados();
        /// oNfe.oCertificado = oConfig.oCertificado;
        /// oNfe.vUF = oConfig.vUnidadeFederativaCodigo;
        /// oNfe.vAmbiente = oConfig.vAmbienteCodigo;
        /// oNfe.vPastaXMLEnvio = oConfig.vPastaXMLEnvio;
        /// oNfe.vPastaXMLRetorno = oConfig.vPastaXMLRetorno;
        /// oNfe.vPastaXMLEnviado = oConfig.vPastaXMLEnviado;
        /// oNfe.vPastaXMLErro = oConfig.vPastaXMLErro;
        /// </example>
        public void CarregarDados()
        {
            string vArquivoConfig = "UniNfeConfig.xml";
            this.vPastaXMLEnvio = string.Empty;
            this.vPastaXMLRetorno = string.Empty;
            this.vPastaXMLEnviado = string.Empty;
            this.vPastaXMLErro = string.Empty;
            this.vUnidadeFederativaCodigo = 0;
            this.vAmbienteCodigo = 2;
            this.vTpEmis = 1;
            this.vCertificado = string.Empty;
            this.cNomeEmpresa = string.Empty;
            this.cPastaBackup = string.Empty;

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
                                    else if (oLerXml.Name == "PastaXmlErro") { oLerXml.Read(); this.vPastaXMLErro = oLerXml.Value; }
                                    else if (oLerXml.Name == "UnidadeFederativaCodigo") { oLerXml.Read(); this.vUnidadeFederativaCodigo = Convert.ToInt32(oLerXml.Value); }
                                    else if (oLerXml.Name == "AmbienteCodigo") { oLerXml.Read(); this.vAmbienteCodigo = Convert.ToInt32(oLerXml.Value); }
                                    else if (oLerXml.Name == "CertificadoDigital") { oLerXml.Read(); this.vCertificado = oLerXml.Value; }
                                    else if (oLerXml.Name == "tpEmis") { oLerXml.Read(); this.vTpEmis = Convert.ToInt32(oLerXml.Value); }
                                    else if (oLerXml.Name == "NomeEmpresa") { oLerXml.Read(); this.cNomeEmpresa = oLerXml.Value; }
                                    else if (oLerXml.Name == "PastaBackup") { oLerXml.Read(); this.cPastaBackup = oLerXml.Value; }
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

        /// <summary>
        /// Método responsável por gravar as configurações do UniNFe no arquivo "UniNfeConfig.xml"
        /// </summary>
        /// <returns>Retorna true se conseguiu gravar corretamente as configurações ou false se não conseguiu</returns>
        public bool GravarConfig()
        {
            bool lValidou = this.ValidarConfig();
            if (lValidou == true)
            {
                try
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
                    oXmlGravar.WriteElementString("PastaXmlEnvio", this.vPastaXMLEnvio);
                    oXmlGravar.WriteElementString("PastaXmlRetorno", this.vPastaXMLRetorno);
                    oXmlGravar.WriteElementString("PastaXmlEnviado", this.vPastaXMLEnviado);
                    oXmlGravar.WriteElementString("PastaXmlErro", this.vPastaXMLErro);
                    oXmlGravar.WriteElementString("UnidadeFederativaCodigo", this.vUnidadeFederativaCodigo.ToString());
                    oXmlGravar.WriteElementString("AmbienteCodigo", this.vAmbienteCodigo.ToString());
                    oXmlGravar.WriteElementString("CertificadoDigital", this.vCertificado);
                    oXmlGravar.WriteElementString("tpEmis", this.vTpEmis.ToString());
                    oXmlGravar.WriteElementString("NomeEmpresa", this.cNomeEmpresa);
                    oXmlGravar.WriteElementString("PastaBackup", this.cPastaBackup);
                    oXmlGravar.WriteEndElement(); //nfe_configuracoes
                    oXmlGravar.WriteEndDocument();
                    oXmlGravar.Flush();
                    oXmlGravar.Close();
                }
                catch (Exception ex)
                {
                    this.cErroGravarConfig = ex.Message;
                }
            }

            return (lValidou);
        }

        /// <summary>
        /// Verifica se algumas das informações das configurações tem algum problema ou falha
        /// </summary>
        /// <returns>
        /// true - nenhum problema/falha
        /// false - encontrou algum problema
        /// </returns>
        private bool ValidarConfig()
        {
            bool lValidou = true;

            //Verifica se o nome da empresa ou alguma pasta de configuração está em branco
            if (this.cNomeEmpresa == "")
            {
                this.cErroGravarConfig = "Informe o nome da empresa.";
                lValidou = false;
            }
            else if (this.vPastaXMLEnviado == "")
            {
                this.cErroGravarConfig = "Informe a pasta para arquivamento dos arquivos XML enviados.";
                lValidou = false;
            }
            else if (this.vPastaXMLEnvio == "")
            {
                this.cErroGravarConfig = "Informe a pasta de envio dos arquivos XML.";
                lValidou = false;
            }
            else if (this.vPastaXMLRetorno == "")
            {
                this.cErroGravarConfig = "Informe a pasta de retorno dos arquivos XML.";
                lValidou = false;
            }
            else if (this.vPastaXMLErro == "")
            {
                this.cErroGravarConfig = "Informe a pasta para arquivamento temporário dos arquivos XML que apresentaram erros.";
                lValidou = false;
            }

            //Verificar se as pastas existem
            if (lValidou == true)
            {
                DirectoryInfo oDirEnvio = new DirectoryInfo(this.vPastaXMLEnvio.Trim());
                DirectoryInfo oDirRetorno = new DirectoryInfo(this.vPastaXMLRetorno.Trim());
                DirectoryInfo oDirEnviado = new DirectoryInfo(this.vPastaXMLEnviado.Trim());
                DirectoryInfo oDirErro = new DirectoryInfo(this.vPastaXMLErro.Trim());

                if (this.vCertificado == "")
                {
                    this.cErroGravarConfig = "Selecione o certificado digital a ser utilizado na autenticação dos serviços da nota fiscal eletrônica.";
                    lValidou = false;
                }
                else if (!oDirEnvio.Exists)
                {
                    this.cErroGravarConfig = "A pasta de envio dos arquivos XML informada não existe.";
                    lValidou = false;
                }
                else if (!oDirRetorno.Exists)
                {
                    this.cErroGravarConfig = "A pasta de retorno dos arquivos XML informada não existe.";
                    lValidou = false;
                }
                else if (!oDirEnviado.Exists)
                {
                    this.cErroGravarConfig = "A pasta para arquivamento dos arquivos XML enviados informada não existe.";
                    lValidou = false;
                }
                else if (!oDirErro.Exists)
                {
                    this.cErroGravarConfig = "A pasta para arquivamento temporário dos arquivos XML com erro informada não existe.";
                    lValidou = false;
                }
                else if (this.cPastaBackup.Trim() != "")
                {
                    DirectoryInfo oDirBackup = new DirectoryInfo(this.cPastaBackup.Trim());

                    if (!oDirBackup.Exists)
                    {
                        this.cErroGravarConfig = "A pasta opcional para backup dos XML enviados informada não existe.";
                        lValidou = false;
                    }
                }
            }

            //TODO: Verificar se as pastas informadas são todas diferentes uma das outras, pois não pode ser igual

            return lValidou;
        }

        /// <summary>
        /// Método responsável por reconfigurar automaticamente o UniNFe a partir de um XML com as 
        /// informações necessárias.
        /// O Método grava um arquivo na pasta de retorno do UniNFe com a informação se foi bem 
        /// sucedida a reconfiguração ou não.
        /// </summary>
        /// <param name="cArquivoXml">Nome e pasta do arquivo de configurações gerado pelo ERP para atualização
        /// das configurações do uninfe</param>        /// 
        public void ReconfigurarUniNFe( string cArquivoXml )
        {
            string cStat = "";
            string xMotivo = "";
            bool lErro = false;
            bool lEncontrouTag = false;

            //Recarrega as configurações atuais
            this.CarregarDados();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXml);

                XmlNodeList ConfUniNfeList = doc.GetElementsByTagName("altConfUniNFe");

                foreach (XmlNode ConfUniNfeNode in ConfUniNfeList)
                {
                    XmlElement ConfUniNfeElemento = (XmlElement)ConfUniNfeNode;

                    //Se a tag <PastaXmlEnvio> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnvio").Count != 0)
                    {
                        this.vPastaXMLEnvio = ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnvio")[0].InnerText;
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaXmlRetorno> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlRetorno").Count != 0)
                    {
                        this.vPastaXMLRetorno = ConfUniNfeElemento.GetElementsByTagName("PastaXmlRetorno")[0].InnerText;
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaXmlEnviado> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnviado").Count != 0)
                    {
                        this.vPastaXMLEnviado = ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnviado")[0].InnerText;
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaXmlErro> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlErro").Count != 0)
                    {
                        this.vPastaXMLErro = ConfUniNfeElemento.GetElementsByTagName("PastaXmlErro")[0].InnerText;
                        lEncontrouTag = true;
                    }
                    //Se a tag <UnidadeFederativaCodigo> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("UnidadeFederativaCodigo").Count != 0)
                    {
                        this.vUnidadeFederativaCodigo = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName("UnidadeFederativaCodigo")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <AmbienteCodigo> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("AmbienteCodigo").Count != 0)
                    {
                        this.vAmbienteCodigo = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName("AmbienteCodigo")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <tpEmis> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        this.vTpEmis = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <NomeEmpresa> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("NomeEmpresa").Count != 0)
                    {
                        this.cNomeEmpresa = ConfUniNfeElemento.GetElementsByTagName("NomeEmpresa")[0].InnerText;
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaBackup> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaBackup").Count != 0)
                    {
                        this.cPastaBackup = ConfUniNfeElemento.GetElementsByTagName("PastaBackup")[0].InnerText;
                        lEncontrouTag = true;
                    }
                }                
            }
            catch (Exception ex)
            {
                cStat = "2";
                xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do UniNFe: " + ex.Message;
                lErro = true;
            }
            
            if (lEncontrouTag == true)
            {
                if (lErro == false)
                {
                    if (this.ValidarConfig() == false || this.GravarConfig() == false)
                    {
                        cStat = "2";
                        xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do UniNFe: " + this.cErroGravarConfig;
                        lErro = true;
                    }
                    else
                    {
                        cStat = "1";
                        xMotivo = "Configuracao do UniNFe alterada com sucesso";
                        lErro = false;
                    }
                }
            }
            else
            {
                cStat = "2";
                xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do UniNFe: Nenhuma tag padrão de configuração foi localizada no XML";
                lErro = true;
            }

            //Se deu algum erro tenho que voltar as configurações como eram antes, ou seja
            //exatamente como estavam gravadas no XML de configuração
            if (lErro == true)
            {
                this.CarregarDados();
            }

            //Gravar o XML de retorno com a informação do sucesso ou não na reconfiguração
            try
            {
                string cArqRetorno = this.vPastaXMLRetorno + "\\uninfe-ret-alt-con.xml";

                FileInfo oArqRetorno = new FileInfo(cArqRetorno);
                if (oArqRetorno.Exists == true)
                {
                    oArqRetorno.Delete();
                }

                XmlWriterSettings oSettings = new XmlWriterSettings();

                oSettings.Indent = true;
                oSettings.IndentChars = "";
                oSettings.NewLineOnAttributes = false;
                oSettings.OmitXmlDeclaration = false;

                XmlWriter oXmlGravar = XmlWriter.Create(cArqRetorno, oSettings);

                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("retAltConfUniNFe");
                oXmlGravar.WriteElementString("cStat", cStat);
                oXmlGravar.WriteElementString("xMotivo", xMotivo);
                oXmlGravar.WriteEndElement(); //retAltConfUniNFe
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();
            }
            catch 
            {
                //Ocorreu erro na hora de gerar o arquivo de erro para o ERP
            }

            //Deletar o arquivo de configurações automáticas gerado pelo ERP
            FileInfo oArqReconf = new FileInfo(cArquivoXml);
            oArqReconf.Delete();
        }
    }
}

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
using UniNFeLibrary;

namespace UniNFeLibrary.Formulario
{
    #region Classe Configuracao
    public partial class Configuracao : Form
    {
        private X509Certificate2 oMeuCert;

        public Configuracao()
        {
            InitializeComponent();

            #region Montar Array DropList da UF
            ArrayList arrUF = new ArrayList();
            if (InfoApp.NomeAplicacao().ToUpper() == "UNINFE")
            {
                arrUF.Add(new DropDownLista("AC", 12));
                arrUF.Add(new DropDownLista("AL", 27));
                arrUF.Add(new DropDownLista("AP", 16));
                arrUF.Add(new DropDownLista("AM", 13));
                arrUF.Add(new DropDownLista("BA", 29));
                arrUF.Add(new DropDownLista("CE", 23));
                arrUF.Add(new DropDownLista("DF", 53));
                arrUF.Add(new DropDownLista("ES", 32));
                arrUF.Add(new DropDownLista("GO", 52));
                arrUF.Add(new DropDownLista("MA", 21));
                arrUF.Add(new DropDownLista("MG", 31));
                arrUF.Add(new DropDownLista("MS", 50));
            }
            arrUF.Add(new DropDownLista("MT", 51));
            if (InfoApp.NomeAplicacao().ToUpper() == "UNINFE")
            {
                arrUF.Add(new DropDownLista("PA", 15));
                arrUF.Add(new DropDownLista("PB", 25));
                arrUF.Add(new DropDownLista("PE", 26));
                arrUF.Add(new DropDownLista("PI", 22));
                arrUF.Add(new DropDownLista("PR", 41));
                arrUF.Add(new DropDownLista("RJ", 33));
                arrUF.Add(new DropDownLista("RN", 24));
                arrUF.Add(new DropDownLista("RO", 11));
                arrUF.Add(new DropDownLista("RR", 14));
            }
            arrUF.Add(new DropDownLista("RS", 43));
            if (InfoApp.NomeAplicacao().ToUpper() == "UNINFE")
            {
                arrUF.Add(new DropDownLista("SC", 42));
                arrUF.Add(new DropDownLista("SE", 28));
            }
            arrUF.Add(new DropDownLista("SP", 35));
            if (InfoApp.NomeAplicacao().ToUpper() == "UNINFE")
            {
                arrUF.Add(new DropDownLista("TO", 17));
            }

            comboBox_UF.DataSource = arrUF;
            comboBox_UF.DisplayMember = "Nome";
            comboBox_UF.ValueMember = "Valor";
            #endregion

            #region Montar Array DropList do Ambiente
            //
            // danasa 8-2009
            // atribuido "TipoEmbiente"
            //
            ArrayList arrAmb = new ArrayList();
            arrAmb.Add(new DropDownLista("Produção", TipoAmbiente.taProducao/*1*/));
            arrAmb.Add(new DropDownLista("Homologação", TipoAmbiente.taHomologacao/*2*/));

            comboBox_Ambiente.DataSource = arrAmb;
            comboBox_Ambiente.DisplayMember = "nome";
            comboBox_Ambiente.ValueMember = "valor";
            #endregion

            #region Montar Array DropList do Tipo de Emissão da NF-e
            ArrayList arrTpEmis = new ArrayList();
            ArrayList.Synchronized(arrTpEmis);
            //
            // danasa 8-2009
            // danasa 9-2009
            // atribuido "TipoEmissao.
            //
            arrTpEmis.Add(new DropDownLista(UniNFeConsts.tpEmissao[TipoEmissao.teNormal], TipoEmissao.teNormal));
            arrTpEmis.Add(new DropDownLista(UniNFeConsts.tpEmissao[TipoEmissao.teContingencia], TipoEmissao.teContingencia));
            arrTpEmis.Add(new DropDownLista(UniNFeConsts.tpEmissao[TipoEmissao.teSCAN], TipoEmissao.teSCAN));
            //arrTpEmis.Add(new DropDownLista(UniNFeConsts.tpEmissao[TipoEmissao.teDPEC], TipoEmissao.teDPEC));
            arrTpEmis.Add(new DropDownLista(UniNFeConsts.tpEmissao[TipoEmissao.teFSDA], TipoEmissao.teFSDA));

            comboBox_tpEmis.DataSource = arrTpEmis;
            comboBox_tpEmis.DisplayMember = "nome";
            comboBox_tpEmis.ValueMember = "valor";
            #endregion

            #region Carregar os dados gravados no XML das configurações
            ConfiguracaoApp oCarrega = new ConfiguracaoApp();

            ConfiguracaoApp.CarregarDados();

            this.textBox_PastaEnvioXML.Text = ConfiguracaoApp.vPastaXMLEnvio;
            this.textBox_PastaRetornoXML.Text = ConfiguracaoApp.vPastaXMLRetorno;
            this.textBox_PastaEnviados.Text = ConfiguracaoApp.vPastaXMLEnviado;
            this.textBox_PastaXmlErro.Text = ConfiguracaoApp.vPastaXMLErro;
            this.tbPastaLote.Text = ConfiguracaoApp.cPastaXMLEmLote;
            this.tbPastaValidar.Text = ConfiguracaoApp.PastaValidar;
            this.checkBoxRetornoNFETxt.Checked = ConfiguracaoApp.GravarRetornoTXTNFe;
            this.textBox_Empresa.Text = ConfiguracaoApp.cNomeEmpresa;
            this.oMeuCert = ConfiguracaoApp.oCertificado;
            this.DemonstraDadosCertificado();

            if (ConfiguracaoApp.cPastaBackup == "")
            {
                this.textBox_PastaBackup.Text = "";
            }
            else
            {
                this.textBox_PastaBackup.Text = ConfiguracaoApp.cPastaBackup;
            }
            //Carregar o conteúdo do droplist do tipo de emissão para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrTpEmis.Count; i++)
            {
                if (((DropDownLista)(new System.Collections.ArrayList(arrTpEmis))[i]).Valor == ConfiguracaoApp.tpEmis)
                {
                    this.comboBox_tpEmis.Text = ((DropDownLista)(new System.Collections.ArrayList(arrTpEmis))[i]).Nome;
                    break;
                }
            }

            //Carregar o conteúdo do droplist da Unidade Federativa (UF) para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrUF.Count; i++)
            {
                if (((DropDownLista)(new System.Collections.ArrayList(arrUF))[i]).Valor == ConfiguracaoApp.UFCod)
                {
                    this.comboBox_UF.Text = ((DropDownLista)(new System.Collections.ArrayList(arrUF))[i]).Nome;
                    break;
                }
            }

            //Carregar o conteúdo do droplist do Ambiente para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrAmb.Count; i++)
            {
                if (((DropDownLista)(new System.Collections.ArrayList(arrAmb))[i]).Valor == ConfiguracaoApp.tpAmb)
                {
                    this.comboBox_Ambiente.Text = ((DropDownLista)(new System.Collections.ArrayList(arrAmb))[i]).Nome;
                    break;
                }
            }
            #endregion
        }

        #region Métodos gerais
        #region DemonstraDadosCertificado()
        private void DemonstraDadosCertificado()
        {
            if (oMeuCert != null)
            {
                this.textBox_dadoscertificado.Text =
                    "[Sujeito]\r\n" + oMeuCert.Subject + "\r\n\r\n" +
                    "[Validade]\r\n" + oMeuCert.NotBefore + " à " + oMeuCert.NotAfter;
            }
        }
        #endregion
        #endregion

        #region Métodos de evento
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
            CertificadoDigital oCertDig = new CertificadoDigital();

            if (oCertDig.SelecionarCertificado() == true)
            {
                this.oMeuCert = oCertDig.oCertificado;
                this.DemonstraDadosCertificado();
            }
        }

        private void toolStripButton_fechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton_salvar_Click(object sender, EventArgs e)
        {
            //Criar o XML de controle do fluxo de NFe se ainda não existir
            FluxoNfe oFluxo = new FluxoNfe();
            oFluxo.CriarXml();

            //Salvar as configurações
            ConfiguracaoApp.vPastaXMLEnvio = this.textBox_PastaEnvioXML.Text.Trim();
            ConfiguracaoApp.vPastaXMLRetorno = this.textBox_PastaRetornoXML.Text.Trim();
            ConfiguracaoApp.vPastaXMLEnviado = this.textBox_PastaEnviados.Text.Trim();
            ConfiguracaoApp.vPastaXMLErro = this.textBox_PastaXmlErro.Text.Trim();
            ConfiguracaoApp.UFCod = Convert.ToInt32(this.comboBox_UF.SelectedValue);
            ConfiguracaoApp.tpAmb = Convert.ToInt32(this.comboBox_Ambiente.SelectedValue);
            ConfiguracaoApp.tpEmis = Convert.ToInt32(this.comboBox_tpEmis.SelectedValue);
            ConfiguracaoApp.cNomeEmpresa = this.textBox_Empresa.Text;
            ConfiguracaoApp.cPastaBackup = this.textBox_PastaBackup.Text.Trim();
            ConfiguracaoApp.cPastaXMLEmLote = this.tbPastaLote.Text.Trim();
            ConfiguracaoApp.PastaValidar = this.tbPastaValidar.Text.Trim();
            ConfiguracaoApp.GravarRetornoTXTNFe = this.checkBoxRetornoNFETxt.Checked;
            if (this.oMeuCert == null)
            {
                ConfiguracaoApp.vCertificado = "";
            }
            else
            {
                ConfiguracaoApp.vCertificado = oMeuCert.Subject.ToString();
            }

            ConfiguracaoApp oConfig = new ConfiguracaoApp();
            if (oConfig.GravarConfig() == false)
            {
                MessageBox.Show(ConfiguracaoApp.cErroGravarConfig, "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
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

        private void btnSelectPastaLote_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlenviolote.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.tbPastaLote.Text = this.folderBrowserDialog_xmlenviolote.SelectedPath;
            }
        }

        private void button_SelectPastaValidar_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_Validar.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.tbPastaValidar.Text = this.folderBrowserDialog_Validar.SelectedPath;
            }

        }
        #endregion

        private void cbProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProxy.Checked)
            {
                lblPorta.Enabled = true;
                lblSenha.Enabled = true;
                lblUsuario.Enabled = true;
                tbUsuario.Enabled = true;
                tbSenha.Enabled = true;
                nudPorta.Enabled = true;
            }
            else
            {
                lblPorta.Enabled = false;
                lblSenha.Enabled = false;
                lblUsuario.Enabled = false;
                tbUsuario.Enabled = false;
                tbSenha.Enabled = false;
                nudPorta.Enabled = false;
            }
        }

        private void Configuracao_Load(object sender, EventArgs e)
        {
            ///
            /// danasa 9-2009
            /// 
            XMLIniFile iniFile = new XMLIniFile(InfoApp.NomeArqXMLParams());
            iniFile.LoadForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"));
        }

        private void Configuracao_FormClosed(object sender, FormClosedEventArgs e)
        {
            ///
            /// danasa 9-2009
            /// 
            XMLIniFile iniFile = new XMLIniFile(InfoApp.NomeArqXMLParams());
            iniFile.SaveForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"));
            iniFile.Save();
        }
    }
    #endregion
}

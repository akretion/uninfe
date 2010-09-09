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
        ///
        /// danasa 9-2010
        private EventHandler OnMyClose;
        private bool Salvos;

        ///
        /// danasa 9-2010
        public Configuracao(EventHandler _OnClose)
        {
            InitializeComponent();

            PopulateCbEmpresa();
            PopulateConfGeral();
            PopulateConfEmpresa();

            ///
            /// danasa 9-2010
            this.OnMyClose = _OnClose;
            this.Salvos = false;
        }

        #region Métodos gerais

        #region PopulateCbEmpresa()
        /// <summary>
        /// Popular a ComboBox das empresas
        /// </summary>
        /// <remarks>
        /// Observações: Tem que popular separadamente do Método Populate() para evitar ficar recarregando na hora que selecionamos outra empresa
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        private void PopulateCbEmpresa()
        {
            ArrayList empresa = new ArrayList();
            try
            {
                empresa = Auxiliar.CarregaEmpresa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (empresa.Count > 0)
            {
                cbEmpresa.DataSource = empresa;
                cbEmpresa.DisplayMember = "Nome";
                cbEmpresa.ValueMember = "Valor";
            }
        }
        #endregion

        #region PopulateConfEmpresa()
        /// <summary>
        /// Popular campos das configurações por empresa
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 29/07/2010
        /// </remarks>
        private void PopulateConfEmpresa()
        {
            #region Definir um texto explicativo sobre a impressão do DANFE. Wandrey 02/02/2010
            tbTextoDANFE.Text = "Você pode automatizar o processo de geração/impressão do DANFE através do UniDANFe ou do DANFeMon, bastando preencher os campos abaixo." +
                "\r\n\r\n" +
                "As configurações adicionais devem ser definidas no UniDANFe ou no arquivo XML auxiliar. Para maiores detalhes, consulte a documentação do UniDANFe.";
            #endregion

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

            #region Carregar as configurações da empresa selecionada
            if (Empresa.Configuracoes.Count > 0)
            {
                Empresa oEmpresa = Empresa.FindConfEmpresa(cbEmpresa.SelectedValue.ToString().Trim());

                textBox_PastaEnvioXML.Text = oEmpresa.PastaEnvio;
                textBox_PastaRetornoXML.Text = oEmpresa.PastaRetorno;
                textBox_PastaEnviados.Text = oEmpresa.PastaEnviado;
                textBox_PastaXmlErro.Text = oEmpresa.PastaErro;
                tbPastaLote.Text = oEmpresa.PastaEnvioEmLote;
                tbPastaValidar.Text = oEmpresa.PastaValidar;

                textBox_PastaBackup.Text = (oEmpresa.PastaBackup == string.Empty ? string.Empty : oEmpresa.PastaBackup);
                tbPastaConfigUniDanfe.Text = (oEmpresa.PastaConfigUniDanfe == string.Empty ? string.Empty : oEmpresa.PastaConfigUniDanfe);
                tbPastaExeUniDanfe.Text = (oEmpresa.PastaExeUniDanfe == string.Empty ? string.Empty : oEmpresa.PastaExeUniDanfe);
                tbPastaXmlParaDanfeMon.Text = (oEmpresa.PastaDanfeMon == string.Empty ? string.Empty : oEmpresa.PastaDanfeMon);
                cbDanfeMonNfe.Checked = oEmpresa.XMLDanfeMonNFe;
                cbDanfeMonProcNfe.Checked = oEmpresa.XMLDanfeMonProcNFe;
                checkBoxRetornoNFETxt.Checked = oEmpresa.GravarRetornoTXTNFe;

                oMeuCert = oEmpresa.X509Certificado;
                DemonstraDadosCertificado();

                cboDiretorioSalvarComo.Text = oEmpresa.DiretorioSalvarComo;
                udDiasLimpeza.Value = oEmpresa.DiasLimpeza;

                //Carregar o conteúdo do droplist do tipo de emissão para forçar demonstrar
                //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
                for (int i = 0; i < arrTpEmis.Count; i++)
                {
                    if (((DropDownLista)(new System.Collections.ArrayList(arrTpEmis))[i]).Valor == oEmpresa.tpEmis)
                    {
                        this.comboBox_tpEmis.Text = ((DropDownLista)(new System.Collections.ArrayList(arrTpEmis))[i]).Nome;
                        break;
                    }
                }

                //Carregar o conteúdo do droplist da Unidade Federativa (UF) para forçar demonstrar
                //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
                for (int i = 0; i < arrUF.Count; i++)
                {
                    if (((DropDownLista)(new System.Collections.ArrayList(arrUF))[i]).Valor == oEmpresa.UFCod)
                    {
                        this.comboBox_UF.Text = ((DropDownLista)(new System.Collections.ArrayList(arrUF))[i]).Nome;
                        break;
                    }
                }

                //Carregar o conteúdo do droplist do Ambiente para forçar demonstrar
                //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
                for (int i = 0; i < arrAmb.Count; i++)
                {
                    if (((DropDownLista)(new System.Collections.ArrayList(arrAmb))[i]).Valor == oEmpresa.tpAmb)
                    {
                        this.comboBox_Ambiente.Text = ((DropDownLista)(new System.Collections.ArrayList(arrAmb))[i]).Nome;
                        break;
                    }
                }
            }
            #endregion
        }
        #endregion

        #region PopulateConfGeral()
        /// <summary>
        /// Popular os campos de configurações gerais
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        private void PopulateConfGeral()
        {
            ConfiguracaoApp oCarrega = new ConfiguracaoApp();
            ConfiguracaoApp.CarregarDados();

            cbProxy.Checked = ConfiguracaoApp.Proxy;
            tbServidor.Text = ConfiguracaoApp.ProxyServidor;
            tbUsuario.Text = ConfiguracaoApp.ProxyUsuario;
            tbSenha.Text = ConfiguracaoApp.ProxySenha;
            nudPorta.Value = ConfiguracaoApp.ProxyPorta;
        }
        #endregion

        #region DemonstraDadosCertificado()
        private void DemonstraDadosCertificado()
        {
            if (oMeuCert != null)
            {
                this.textBox_dadoscertificado.Text =
                    "[Sujeito]\r\n" + oMeuCert.Subject + "\r\n\r\n" +
                    "[Validade]\r\n" + oMeuCert.NotBefore + " à " + oMeuCert.NotAfter;
            }
            else
            {
                textBox_dadoscertificado.Clear();
            }
        }
        #endregion

        #region AtualizarPropriedadeEmpresa()
        /// <summary>
        /// Atualizar as propriedades da coleção das empresas
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        public void AtualizarPropriedadeEmpresa()
        {
            //Localizar a empresa na coleção
            int indexEmpresa = Empresa.FindConfEmpresaIndex(cbEmpresa.SelectedValue.ToString().Trim());

            //Atualizar as propriedades das configurações da empresa
            if (Empresa.Valid(indexEmpresa))
            {
                Empresa.Configuracoes[indexEmpresa].PastaEnvio = this.textBox_PastaEnvioXML.Text.Trim();
                Empresa.Configuracoes[indexEmpresa].PastaRetorno = this.textBox_PastaRetornoXML.Text.Trim();
                Empresa.Configuracoes[indexEmpresa].PastaEnviado = this.textBox_PastaEnviados.Text.Trim();
                Empresa.Configuracoes[indexEmpresa].PastaErro = this.textBox_PastaXmlErro.Text.Trim();
                Empresa.Configuracoes[indexEmpresa].UFCod = Convert.ToInt32(this.comboBox_UF.SelectedValue);
                Empresa.Configuracoes[indexEmpresa].tpAmb = Convert.ToInt32(this.comboBox_Ambiente.SelectedValue);
                Empresa.Configuracoes[indexEmpresa].tpEmis = Convert.ToInt32(this.comboBox_tpEmis.SelectedValue);
                Empresa.Configuracoes[indexEmpresa].PastaBackup = this.textBox_PastaBackup.Text.Trim();
                Empresa.Configuracoes[indexEmpresa].PastaEnvioEmLote = this.tbPastaLote.Text.Trim();
                Empresa.Configuracoes[indexEmpresa].PastaValidar = this.tbPastaValidar.Text.Trim();
                Empresa.Configuracoes[indexEmpresa].GravarRetornoTXTNFe = this.checkBoxRetornoNFETxt.Checked;
                Empresa.Configuracoes[indexEmpresa].DiretorioSalvarComo = this.cboDiretorioSalvarComo.Text;
                Empresa.Configuracoes[indexEmpresa].PastaConfigUniDanfe = tbPastaConfigUniDanfe.Text;
                Empresa.Configuracoes[indexEmpresa].PastaExeUniDanfe = tbPastaExeUniDanfe.Text;
                Empresa.Configuracoes[indexEmpresa].PastaDanfeMon = tbPastaXmlParaDanfeMon.Text;
                Empresa.Configuracoes[indexEmpresa].XMLDanfeMonNFe = this.cbDanfeMonNfe.Checked;
                Empresa.Configuracoes[indexEmpresa].XMLDanfeMonProcNFe = this.cbDanfeMonProcNfe.Checked;
                Empresa.Configuracoes[indexEmpresa].DiasLimpeza = (int)udDiasLimpeza.Value;
                Empresa.Configuracoes[indexEmpresa].Certificado = (this.oMeuCert == null ? string.Empty : oMeuCert.Subject.ToString());
            }
        }
        #endregion

        #region Salvar()
        /// <summary>
        /// Salvar as configurações realizadas no XML
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        private void Salvar()
        {
            //Atualizar as propriedades das configurações da empresa
            AtualizarPropriedadeEmpresa();

            //Atualizar as propriedades das configurações gerais
            ConfiguracaoApp.Proxy = this.cbProxy.Checked;
            ConfiguracaoApp.ProxyPorta = (int)this.nudPorta.Value;
            ConfiguracaoApp.ProxySenha = this.tbSenha.Text;
            ConfiguracaoApp.ProxyServidor = tbServidor.Text;
            ConfiguracaoApp.ProxyUsuario = tbUsuario.Text;

            //Salvar as configurações
            ConfiguracaoApp oConfig = new ConfiguracaoApp();
            try
            {
                oConfig.GravarConfig();

                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                oMeuCert = oCertDig.oCertificado;
                Empresa.Configuracoes[Empresa.FindConfEmpresaIndex(cbEmpresa.SelectedValue.ToString().Trim())].Certificado = oMeuCert.Subject;
                Empresa.Configuracoes[Empresa.FindConfEmpresaIndex(cbEmpresa.SelectedValue.ToString().Trim())].X509Certificado = oMeuCert;
                DemonstraDadosCertificado();
            }
        }

        private void toolStripButton_fechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton_salvar_Click(object sender, EventArgs e)
        {
            this.Salvar();
            this.Salvos = true;
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

        private void cbProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProxy.Checked)
            {
                lblPorta.Enabled = true;
                lblSenha.Enabled = true;
                lblUsuario.Enabled = true;
                lblServidor.Enabled = true;
                tbUsuario.Enabled = true;
                tbSenha.Enabled = true;
                nudPorta.Enabled = true;
                tbServidor.Enabled = true;
            }
            else
            {
                lblPorta.Enabled = false;
                lblSenha.Enabled = false;
                lblUsuario.Enabled = false;
                lblServidor.Enabled = false;
                tbUsuario.Enabled = false;
                tbSenha.Enabled = false;
                nudPorta.Enabled = false;
                tbServidor.Enabled = false;
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
            ///
            /// danasa 9-2010
            if (OnMyClose != null)
                if (e.CloseReason == CloseReason.UserClosing)
                    if (this.Salvos)
                        OnMyClose(sender, null);
        }

        private void button_SelectPastaExeUniDanfe_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlenvio.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.tbPastaExeUniDanfe.Text = this.folderBrowserDialog_xmlenvio.SelectedPath;
            }

        }

        private void button_SelectPastaConfUniDanfe_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlenvio.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.tbPastaConfigUniDanfe.Text = this.folderBrowserDialog_xmlenvio.SelectedPath;
            }
        }

        private void btnSelectPastaParaXmlDanfeMon_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog_xmlenvio.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.tbPastaXmlParaDanfeMon.Text = this.folderBrowserDialog_xmlenvio.SelectedPath;
            }
        }


        private void cbEmpresa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Popular com novas informações da empresa selecionada
            PopulateConfEmpresa();
        }

        #endregion

        private void cbEmpresa_DropDown(object sender, EventArgs e)
        {
            //Atualizar as propriedades das configurações da empresa com o conteúdo da tela
            //antes de mudar a empresa para não perder os dados.
            AtualizarPropriedadeEmpresa();
        }
    }
    #endregion
}

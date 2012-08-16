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
using NFe.Settings;
using NFe.Components;
using NFe.Certificado;

namespace NFe.Interface
{
    public partial class ucConfiguracao : UserControl
    {
        private X509Certificate2 oMeuCert;
        private UpdateText updateText;
        private string cnpjCurrent;

        public ucConfiguracao(UpdateText updateText)
        {
            InitializeComponent();

            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
            {
                ///danasa 1-2012
                this.tabControl3.TabPages.Remove(this.tabPageDanfe);
                labelUF.Text = "Município/Cidade:";
            }
            else
            {
                ///danasa 1-2012
                labelCodMun.Visible =
                    labelPadrao.Visible =
                    edtCodMun.Visible =
                    edtPadrao.Visible = false;
            }
            checkBoxGravarEventosNaPastaEnviadosNFe.Visible =
                checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Visible =
                textBox_PastaDownload.Visible =
                button_SelecionarPastaDownload.Visible = Propriedade.TipoAplicativo == TipoAplicativo.Nfe;
            this.updateText = updateText;
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.tabControl3.Dock = DockStyle.Fill;
            this.tabControl3.SelectedIndex = 0;
        }

        public bool Modificado { get; set; }
        public Empresa oEmpresa { get; set; }

        public void focusNome()
        {
            if (this.edtNome.Enabled)
                if (string.IsNullOrEmpty(edtNome.Text))
                {
                    this.tabControl3.SelectedIndex = 0;
                    this.edtNome.Focus();
                }
        }

        /// <summary>
        /// CopiaPastaDaEmpresa
        /// </summary>
        /// <param name="origemCNPJ"></param>
        /// <param name="origemPasta"></param>
        /// <param name="oEmpresa"></param>
        /// <returns></returns>
        private string CopiaPastaDeEmpresa(string origemCNPJ, string origemPasta, Empresa oEmpresa)
        {
            if (string.IsNullOrEmpty(origemPasta))
                return "";

            ///
            ///o usuario pode ter colocado o CNPJ como parte do nome da pasta
            ///
            string newPasta = origemPasta.Replace(origemCNPJ.Trim(), oEmpresa.CNPJ.Trim());

            if (origemPasta.ToLower() == newPasta.ToLower())
            {
                int lastBackSlash = ConfiguracaoApp.RemoveEndSlash(origemPasta).LastIndexOf("\\");
                newPasta = origemPasta.Insert(lastBackSlash, "\\" + oEmpresa.CNPJ);
            }
            return newPasta;
        }

        #region PopulateConfEmpresa()
        /// <summary>
        /// Popular campos das configurações por empresa
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 29/07/2010
        /// </remarks>
        public void PopulateConfEmpresa(string cnpj)
        {
            #region Definir um texto explicativo sobre a impressão do DANFE. Wandrey 02/02/2010
            tbTextoDANFE.Text = "Você pode automatizar o processo de geração/impressão do DANFE através do UniDANFe ou do DANFeMon, bastando preencher os campos abaixo." +
                "\r\n\r\n" +
                "As configurações adicionais devem ser definidas no UniDANFe ou no arquivo XML auxiliar. Para maiores detalhes, consulte a documentação do UniDANFe.";
            #endregion

            #region Montar Array DropList da UF
            ArrayList arrUF = new ArrayList();

            try
            {
                arrUF = Functions.CarregaUF();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            comboBox_UF.DataSource = arrUF;
            comboBox_UF.DisplayMember = "Nome";
            comboBox_UF.ValueMember = "Codigo";
            #endregion

            #region Montar Array DropList do Ambiente
            //
            // danasa 8-2009
            // atribuido "TipoEmbiente"
            //
            ArrayList arrAmb = new ArrayList();
            arrAmb.Add(new ComboElem("Produção", Propriedade.TipoAmbiente.taProducao));
            arrAmb.Add(new ComboElem("Homologação", Propriedade.TipoAmbiente.taHomologacao));

            comboBox_Ambiente.DataSource = arrAmb;
            comboBox_Ambiente.DisplayMember = "valor";
            comboBox_Ambiente.ValueMember = "codigo";
            #endregion

            #region Montar Array DropList do Tipo de Emissão da NF-e
            ArrayList arrTpEmis = new ArrayList();
            ArrayList.Synchronized(arrTpEmis);
            //
            // danasa 8-2009
            // danasa 9-2009
            // atribuido "TipoEmissao.
            //
            arrTpEmis.Add(new ComboElem(Propriedade.tpEmissao[Propriedade.TipoEmissao.teNormal], Propriedade.TipoEmissao.teNormal));

            if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
            {                
                if (Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
                {
                    arrTpEmis.Add(new ComboElem(Propriedade.tpEmissao[Propriedade.TipoEmissao.teContingencia], Propriedade.TipoEmissao.teContingencia)); 
                    arrTpEmis.Add(new ComboElem(Propriedade.tpEmissao[Propriedade.TipoEmissao.teSCAN], Propriedade.TipoEmissao.teSCAN));
                    arrTpEmis.Add(new ComboElem(Propriedade.tpEmissao[Propriedade.TipoEmissao.teDPEC], Propriedade.TipoEmissao.teDPEC));
                }

                arrTpEmis.Add(new ComboElem(Propriedade.tpEmissao[Propriedade.TipoEmissao.teFSDA], Propriedade.TipoEmissao.teFSDA));

                if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                {
                    arrTpEmis.Add(new ComboElem(Propriedade.tpEmissao[Propriedade.TipoEmissao.teSVCRS], Propriedade.TipoEmissao.teSVCRS));
                    arrTpEmis.Add(new ComboElem(Propriedade.tpEmissao[Propriedade.TipoEmissao.teSVCSP], Propriedade.TipoEmissao.teSVCSP));
                }
            }

            comboBox_tpEmis.DataSource = arrTpEmis;
            comboBox_tpEmis.DisplayMember = "Valor";
            comboBox_tpEmis.ValueMember = "Codigo";
            #endregion

            #region Carregar as configurações da empresa selecionada
            oEmpresa = null;
            textBox_dadoscertificado.Height = 287;
            textBox_dadoscertificado.Refresh();

            if (Empresa.Configuracoes.Count > 0)
            {
                oEmpresa = Empresa.FindConfEmpresa(cnpj.Trim());
                if (oEmpresa != null)
                {
                    ///
                    /// danasa 20-9-2010
                    /// tirado daqui pois se entrado + de 1 vez na configuracao da empresa, a propriedade CriaPastasAutomaticamente será definida como false 
                    /// já que na segunda vez os nomes das pastas já estão atribuidas
                    //oEmpresa.CriaPastasAutomaticamente = false;

                    if (string.IsNullOrEmpty(oEmpresa.PastaEnvio))
                    {
                        ///
                        /// tenta achar uma configuracao valida
                        /// 
                        foreach (Empresa empresa in Empresa.Configuracoes)
                        {
                            if (empresa.CNPJ.Trim() != oEmpresa.CNPJ.Trim() && !string.IsNullOrEmpty(empresa.PastaEnvio))
                            {
                                oEmpresa.PastaEnvio = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaEnvio, oEmpresa);
                                oEmpresa.PastaRetorno = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaRetorno, oEmpresa);
                                oEmpresa.PastaEnviado = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaEnviado, oEmpresa);
                                oEmpresa.PastaErro = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaErro, oEmpresa);
                                oEmpresa.PastaEnvioEmLote = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaEnvioEmLote, oEmpresa);
                                oEmpresa.PastaValidar = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaValidar, oEmpresa);
                                oEmpresa.PastaBackup = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaBackup, oEmpresa);
                                oEmpresa.PastaDownloadNFeDest = CopiaPastaDeEmpresa(empresa.CNPJ, empresa.PastaDownloadNFeDest, oEmpresa);

                                oEmpresa.PastaConfigUniDanfe = empresa.PastaConfigUniDanfe;
                                oEmpresa.PastaExeUniDanfe = empresa.PastaExeUniDanfe;
                                oEmpresa.PastaDanfeMon = empresa.PastaDanfeMon;
                                oEmpresa.XMLDanfeMonNFe = empresa.XMLDanfeMonNFe;
                                oEmpresa.XMLDanfeMonProcNFe = empresa.XMLDanfeMonProcNFe;
                                oEmpresa.GravarRetornoTXTNFe = empresa.GravarRetornoTXTNFe;
                                oEmpresa.GravarEventosNaPastaEnviadosNFe = empresa.GravarEventosNaPastaEnviadosNFe;
                                oEmpresa.GravarEventosCancelamentoNaPastaEnviadosNFe = empresa.GravarEventosCancelamentoNaPastaEnviadosNFe;
                                oEmpresa.GravarEventosDeTerceiros = empresa.GravarEventosDeTerceiros;

                                oEmpresa.CriaPastasAutomaticamente = true;
                                break;
                            }
                        }
                        ///
                        /// se ainda assim nao foi encontrada nenhuma configuracao válida assume a pasta de instalacao do uninfe
                        /// 
                        if (string.IsNullOrEmpty(oEmpresa.PastaEnvio))
                        {
                            oEmpresa.PastaEnvio = Path.Combine(Propriedade.PastaExecutavel, oEmpresa.CNPJ + "\\Envio");
                            oEmpresa.PastaEnviado = Path.Combine(Propriedade.PastaExecutavel, oEmpresa.CNPJ + "\\Enviado");
                            oEmpresa.PastaRetorno = Path.Combine(Propriedade.PastaExecutavel, oEmpresa.CNPJ + "\\Retorno");
                            oEmpresa.PastaErro = Path.Combine(Propriedade.PastaExecutavel, oEmpresa.CNPJ + "\\Erro");
                            oEmpresa.PastaEnvioEmLote = Path.Combine(Propriedade.PastaExecutavel, oEmpresa.CNPJ + "\\EnvioEmLote");
                            oEmpresa.PastaValidar = Path.Combine(Propriedade.PastaExecutavel, oEmpresa.CNPJ + "\\Validar");
                            oEmpresa.PastaDownloadNFeDest = Path.Combine(Propriedade.PastaExecutavel, oEmpresa.CNPJ + "\\DownloadNFe");

                            oEmpresa.CriaPastasAutomaticamente = true;
                        }
                    }

                    edtNome.Text = oEmpresa.Nome;
                    edtCNPJ.Text = oEmpresa.CNPJ;
                    oMeuCert = oEmpresa.X509Certificado;

                    ckbCertificadoInstalado.Checked = oEmpresa.CertificadoInstalado;
                    if (oEmpresa.CertificadoInstalado)
                    {
                        DemonstraDadosCertificado();
                    }
                    else
                    {
                        txtArquivoCertificado.Text = oEmpresa.CertificadoArquivo;
                        txtSenhaCertificado.Text = oEmpresa.CertificadoSenha;
                    }

                    edtNome.Enabled = edtCNPJ.Enabled = false;
                }
            }
            if (oEmpresa == null)
            {
                oEmpresa = new Empresa();
                oEmpresa.tpEmis = (int)Propriedade.TipoEmissao.teNormal;
                oEmpresa.tpAmb = (int)Propriedade.TipoAmbiente.taHomologacao;
                oEmpresa.UFCod = 41;
                oEmpresa.DiretorioSalvarComo = "AM";
                oEmpresa.CNPJ = "";
            }
            textBox_PastaEnvioXML.Text = oEmpresa.PastaEnvio;
            textBox_PastaRetornoXML.Text = oEmpresa.PastaRetorno;
            textBox_PastaEnviados.Text = oEmpresa.PastaEnviado;
            textBox_PastaXmlErro.Text = oEmpresa.PastaErro;
            textBox_PastaLote.Text = oEmpresa.PastaEnvioEmLote;
            textBox_PastaValidar.Text = oEmpresa.PastaValidar;
            textBox_PastaDownload.Text = oEmpresa.PastaDownloadNFeDest;
            textBox_PastaBackup.Text = (oEmpresa.PastaBackup == string.Empty ? string.Empty : oEmpresa.PastaBackup);

            tbPastaConfigUniDanfe.Text = (oEmpresa.PastaConfigUniDanfe == string.Empty ? string.Empty : oEmpresa.PastaConfigUniDanfe);
            tbPastaExeUniDanfe.Text = (oEmpresa.PastaExeUniDanfe == string.Empty ? string.Empty : oEmpresa.PastaExeUniDanfe);
            tbPastaXmlParaDanfeMon.Text = (oEmpresa.PastaDanfeMon == string.Empty ? string.Empty : oEmpresa.PastaDanfeMon);
            cbDanfeMonNfe.Checked = oEmpresa.XMLDanfeMonNFe;
            cbDanfeMonProcNfe.Checked = oEmpresa.XMLDanfeMonProcNFe;
            checkBoxRetornoNFETxt.Checked = oEmpresa.GravarRetornoTXTNFe;
            checkBoxGravarEventosDeTerceiros.Checked = oEmpresa.GravarEventosDeTerceiros;
            checkBoxGravarEventosNaPastaEnviadosNFe.Checked = oEmpresa.GravarEventosNaPastaEnviadosNFe;
            checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked = oEmpresa.GravarEventosCancelamentoNaPastaEnviadosNFe;
            cbCriaPastas.Checked = oEmpresa.CriaPastasAutomaticamente;

            cboDiretorioSalvarComo.Text = oEmpresa.DiretorioSalvarComo;
            udDiasLimpeza.Value = oEmpresa.DiasLimpeza;
            udTempoConsulta.Value = (oEmpresa.TempoConsulta >= udTempoConsulta.Minimum && oEmpresa.TempoConsulta <= udTempoConsulta.Maximum ? oEmpresa.TempoConsulta : udTempoConsulta.Minimum);

            edtFTP_Ativo.Checked = oEmpresa.FTPAtivo;
            edtFTP_GravaXMLPastaUnica.Checked = oEmpresa.FTPGravaXMLPastaUnica;
            edtFTP_Password.Text = oEmpresa.FTPSenha;
            edtFTP_PastaDestino.Text = oEmpresa.FTPPastaAutorizados;
            edtFTP_PastaRetornos.Text = oEmpresa.FTPPastaRetornos;
            edtFTP_Porta.Value = oEmpresa.FTPPorta;
            edtFTP_Server.Text = oEmpresa.FTPNomeDoServidor;
            edtFTP_UserName.Text = oEmpresa.FTPNomeDoUsuario;

            cnpjCurrent = oEmpresa.CNPJ;

            //Carregar o conteúdo do droplist do tipo de emissão para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrTpEmis.Count; i++)
            {
                if (((ComboElem)(new System.Collections.ArrayList(arrTpEmis))[i]).Codigo == oEmpresa.tpEmis)
                {
                    this.comboBox_tpEmis.Text = ((ComboElem)(new System.Collections.ArrayList(arrTpEmis))[i]).Valor;
                    break;
                }
            }

            //Carregar o conteúdo do droplist da Unidade Federativa (UF) para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrUF.Count; i++)
            {
                if (((ComboElem)(new System.Collections.ArrayList(arrUF))[i]).Codigo == oEmpresa.UFCod)
                {
                    this.comboBox_UF.Text = ((ComboElem)(new System.Collections.ArrayList(arrUF))[i]).Nome;
                    break;
                }
            }

            //Carregar o conteúdo do droplist do Ambiente para forçar demonstrar
            //o conteúdo já informado pelo usuário. Wandrey 30/10/2008
            for (int i = 0; i < arrAmb.Count; i++)
            {
                if (((ComboElem)(new System.Collections.ArrayList(arrAmb))[i]).Codigo == oEmpresa.tpAmb)
                {
                    this.comboBox_Ambiente.Text = ((ComboElem)(new System.Collections.ArrayList(arrAmb))[i]).Valor;
                    break;
                }
            }
            #endregion

            this.Modificado = false;
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
            //Atualizar as propriedades das configurações da empresa
            oEmpresa.PastaEnvio = this.textBox_PastaEnvioXML.Text.Trim();
            oEmpresa.PastaRetorno = this.textBox_PastaRetornoXML.Text.Trim();
            oEmpresa.PastaEnviado = this.textBox_PastaEnviados.Text.Trim();
            oEmpresa.PastaErro = this.textBox_PastaXmlErro.Text.Trim();
            oEmpresa.PastaBackup = this.textBox_PastaBackup.Text.Trim();
            oEmpresa.PastaEnvioEmLote = this.textBox_PastaLote.Text.Trim();
            oEmpresa.PastaValidar = this.textBox_PastaValidar.Text.Trim();
            oEmpresa.PastaDownloadNFeDest = this.textBox_PastaDownload.Text.Trim();

            oEmpresa.UFCod = Convert.ToInt32(this.comboBox_UF.SelectedValue);
            oEmpresa.tpAmb = Convert.ToInt32(this.comboBox_Ambiente.SelectedValue);
            oEmpresa.tpEmis = Convert.ToInt32(this.comboBox_tpEmis.SelectedValue);
            oEmpresa.GravarRetornoTXTNFe = this.checkBoxRetornoNFETxt.Checked;
            oEmpresa.GravarEventosDeTerceiros = this.checkBoxGravarEventosDeTerceiros.Checked;
            oEmpresa.GravarEventosNaPastaEnviadosNFe = this.checkBoxGravarEventosNaPastaEnviadosNFe.Checked;
            oEmpresa.GravarEventosCancelamentoNaPastaEnviadosNFe = this.checkBoxGravarEventosCancelamentoNaPastaEnviadosNFe.Checked;
            oEmpresa.DiretorioSalvarComo = this.cboDiretorioSalvarComo.Text;
            oEmpresa.PastaConfigUniDanfe = tbPastaConfigUniDanfe.Text;
            oEmpresa.PastaExeUniDanfe = tbPastaExeUniDanfe.Text;
            oEmpresa.PastaDanfeMon = tbPastaXmlParaDanfeMon.Text;
            oEmpresa.XMLDanfeMonNFe = this.cbDanfeMonNfe.Checked;
            oEmpresa.XMLDanfeMonProcNFe = this.cbDanfeMonProcNfe.Checked;
            oEmpresa.DiasLimpeza = (int)udDiasLimpeza.Value;
            oEmpresa.TempoConsulta = (int)udTempoConsulta.Value;
            oEmpresa.Certificado = (this.oMeuCert == null ? string.Empty : oMeuCert.Subject.ToString());
            oEmpresa.CertificadoThumbPrint = (this.oMeuCert == null ? string.Empty : oMeuCert.Thumbprint);
            oEmpresa.Nome = edtNome.Text;
            oEmpresa.CNPJ = Functions.OnlyNumbers(this.edtCNPJ.Text, ".,-/").ToString();
            oEmpresa.CriaPastasAutomaticamente = cbCriaPastas.Checked;

            oEmpresa.FTPAtivo = this.edtFTP_Ativo.Checked;
            oEmpresa.FTPGravaXMLPastaUnica = this.edtFTP_GravaXMLPastaUnica.Checked;
            oEmpresa.FTPSenha = this.edtFTP_Password.Text;
            oEmpresa.FTPPastaAutorizados = this.edtFTP_PastaDestino.Text;
            oEmpresa.FTPPastaRetornos = this.edtFTP_PastaRetornos.Text;
            oEmpresa.FTPPorta = Convert.ToInt32(this.edtFTP_Porta.Value);
            oEmpresa.FTPNomeDoServidor = this.edtFTP_Server.Text;
            oEmpresa.FTPNomeDoUsuario = this.edtFTP_UserName.Text;

            oEmpresa.CertificadoInstalado = ckbCertificadoInstalado.Checked;
            oEmpresa.CertificadoArquivo = txtArquivoCertificado.Text;
            oEmpresa.CertificadoSenha = txtSenhaCertificado.Text;
        }
        #endregion

        #region DemonstraDadosCertificado()
        private void DemonstraDadosCertificado()
        {
            if (oMeuCert != null)
            {
                DateTime hoje = DateTime.Now;
                TimeSpan dif = oMeuCert.NotAfter.Subtract(hoje);
                string mensagemRestante;

                if (dif.Days > 0)
                {
                    mensagemRestante = "Faltam " + dif.Days + " dias para vencer o certificado.";
                }
                else
                {
                    mensagemRestante = "Certificado vencido a " + (dif.Days) * -1 + " dias.";
                }
                this.textBox_dadoscertificado.Text =
                    "[Sujeito]\r\n" + oMeuCert.Subject + "\r\n\r\n" +
                    "[Validade]\r\n" + oMeuCert.NotBefore + " à " + oMeuCert.NotAfter + "\r\n" + mensagemRestante + "\r\n\r\n" +
                    "[ThumbPrint]\r\n" + oMeuCert.Thumbprint;
            }
            else
            {
                textBox_dadoscertificado.Clear();
            }
        }
        #endregion

        private void edtCNPJ_Leave(object sender, EventArgs e)
        {
            string cnpj = Functions.OnlyNumbers(this.edtCNPJ.Text, ".,-/").ToString();
            string nome = edtNome.Text.ToString();

            if (edtNome.Text.ToString().Length > 20)
                nome = edtNome.Text.Substring(0, 20);

            if (string.IsNullOrEmpty(cnpj))
            {
                if (this.updateText != null)
                    this.updateText("-- NOVA --");
            }
            else
            {
                if (cnpjCurrent != cnpj)
                {
                    if (!CNPJ.Validate(cnpj))
                    {
                        this.tabControl3.SelectedIndex = 0;
                        this.edtCNPJ.Focus();
                        MessageBox.Show("CNPJ inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (Empresa.FindConfEmpresa(cnpj) != null)
                    {
                        this.tabControl3.SelectedIndex = 0;
                        this.edtCNPJ.Focus();
                        MessageBox.Show("Empresa/CNPJ já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.Modificado = true;
                    bool mudaPastas = true;
                    if (!string.IsNullOrEmpty(textBox_PastaEnvioXML.Text))
                    {
                        mudaPastas = MessageBox.Show("CNPJ foi alterado e você já tem as pastas definidas. Deseja mudá-las para o novo CNPJ?", "CNPJ alterado", MessageBoxButtons.YesNo) == DialogResult.Yes;
                    }
                    if (mudaPastas)
                    {
                        this.textBox_PastaEnvioXML.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\Envio");
                        this.textBox_PastaEnviados.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\Enviado");
                        this.textBox_PastaRetornoXML.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\Retorno");
                        this.textBox_PastaXmlErro.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\Erro");
                        this.textBox_PastaLote.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\EnvioEmLote");
                        this.textBox_PastaValidar.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\Validar");
                        if (!string.IsNullOrEmpty(textBox_PastaDownload.Text))
                            this.textBox_PastaDownload.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\DownloadNFe");

                        if (!string.IsNullOrEmpty(textBox_PastaBackup.Text))
                            textBox_PastaBackup.Text = Path.Combine(Propriedade.PastaExecutavel, cnpj + "\\Backup");
                    }
                }
                cnpjCurrent = cnpj;

                if (this.updateText != null)
                    this.updateText(nome);
            }
        }

        private void button_selectxmlenvio_Click(object sender, EventArgs e)
        {
            TextBox control = null;

            switch (Convert.ToInt16(((Button)sender).Tag))
            {
                case 0: control = textBox_PastaEnvioXML; break;
                case 1: control = textBox_PastaLote; break;
                case 2: control = textBox_PastaRetornoXML; break;
                case 3: control = textBox_PastaEnviados; break;
                case 4: control = textBox_PastaXmlErro; break;
                case 5: control = textBox_PastaBackup; break;
                case 6: control = textBox_PastaValidar; break;
                case 7: control = tbPastaExeUniDanfe; break;
                case 8: control = tbPastaConfigUniDanfe; break;
                case 9: control = tbPastaXmlParaDanfeMon; break;
                case 10: control = textBox_PastaDownload; break;
            }
            this.folderBrowserDialog1.SelectedPath = control.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                control.Text = this.folderBrowserDialog1.SelectedPath;
            }
            control.Focus();
            control.SelectAll();
        }

        private void button_selecionar_certificado_Click(object sender, EventArgs e)
        {
            if (ckbCertificadoInstalado.Checked)
            {
                CertificadoDigital oCertDig = new CertificadoDigital();

                if (oCertDig.SelecionarCertificado() == true)
                {
                    oMeuCert = oCertDig.oCertificado;
                    oEmpresa.Certificado = oMeuCert.Subject;
                    oEmpresa.CertificadoThumbPrint = oMeuCert.Thumbprint;
                    oEmpresa.X509Certificado = oMeuCert;
                    DemonstraDadosCertificado();
                }
            }
            else
            {
                if (File.Exists(txtArquivoCertificado.Text))
                {
                    FileInfo arq = new FileInfo(txtArquivoCertificado.Text);
                    this.openFileDialog1.InitialDirectory = arq.DirectoryName;
                    this.openFileDialog1.FileName = txtArquivoCertificado.Text;
                }
                else
                {
                    this.openFileDialog1.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
                    this.openFileDialog1.FileName = null;
                }

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtArquivoCertificado.Text = this.openFileDialog1.FileName;
                }
            }
        }

        private bool dirNOTexiste(string pasta)
        {
            if (string.IsNullOrEmpty(pasta)) return false;
            return !Directory.Exists(pasta);
        }

        private void changed_Modificado(object sender, EventArgs e)
        {
            // danasa 1-2012
            try
            {
                object xuf = comboBox_UF.SelectedValue;
                edtCodMun.Text = xuf.ToString();
                edtPadrao.Text = Functions.PadraoNFSe(Convert.ToInt32(xuf)).ToString();
            }
            catch
            {
                edtCodMun.Text = edtPadrao.Text = "Indefinido";
            }
            // danasa 1-2012

            this.Modificado = true;

            cbCriaPastas.Checked = dirNOTexiste(this.textBox_PastaBackup.Text) ||
                                   dirNOTexiste(this.textBox_PastaEnviados.Text) ||
                                   dirNOTexiste(this.textBox_PastaEnvioXML.Text) ||
                                   dirNOTexiste(this.textBox_PastaRetornoXML.Text) ||
                                   dirNOTexiste(this.textBox_PastaXmlErro.Text) ||
                                   dirNOTexiste(this.textBox_PastaValidar.Text) ||
                                   dirNOTexiste(this.textBox_PastaLote.Text) ||
                                   dirNOTexiste(this.textBox_PastaDownload.Text);
        }

        private void btnFTPTestar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            FTP ftp = new FTP(this.edtFTP_Server.Text, Convert.ToInt32(this.edtFTP_Porta.Value), this.edtFTP_UserName.Text, this.edtFTP_Password.Text);
            try
            {
                ftp.Connect();
                if (ftp.IsConnected)
                {
                    string vCurrente = ftp.GetWorkingDirectory();

                    //ftp.UploadFile("c:\\usr\\usr.rar", this.edtFTP_PastaDestino.Text + "/testeA.rar", false);

                    if (!ftp.changeDir(this.edtFTP_PastaDestino.Text))
                    {
                        string error = "Pasta '" + this.edtFTP_PastaDestino.Text + "' não existe no FTP.\r\nDesejá criá-la agora?";
                        if (MessageBox.Show(error, "Informação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            ftp.makeDir(this.edtFTP_PastaDestino.Text);
                        }
                    }
                    //ftp.UploadFile("c:\\usr\\usr.rar", "testeX.rar", false);

                    ftp.ChangeDir(vCurrente);

                    if (!string.IsNullOrEmpty(this.edtFTP_PastaRetornos.Text))
                        if (!ftp.changeDir(this.edtFTP_PastaRetornos.Text))
                        {
                            string error = "Pasta '" + this.edtFTP_PastaRetornos.Text + "' não existe no FTP.";
                            if (MessageBox.Show(error, "Informação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                ftp.makeDir(this.edtFTP_PastaRetornos.Text);
                            }
                        }

                    //ftp.UploadFile("c:\\usr\\usr.rar", "teste.rar", false);

                    MessageBox.Show("FTP conectado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ftp.IsConnected)
                    ftp.Disconnect();

                Cursor = Cursors.Default;
            }
        }

        private void ckbCertificadoInstalado_CheckedChanged(object sender, EventArgs e)
        {
            //button_selecionar_certificado.Enabled = ckbCertificadoInstalado.Checked;
            lblSenhaCertificado.Visible =
            txtSenhaCertificado.Visible =
            lblCertificadoArquivo.Visible =
            txtArquivoCertificado.Visible =
            !ckbCertificadoInstalado.Checked;

            lblCerificadoInstalado.Visible =
            textBox_dadoscertificado.Visible =
            ckbCertificadoInstalado.Checked;

            if (!ckbCertificadoInstalado.Checked)
            {
                textBox_dadoscertificado.Text = "";
                oMeuCert = null;
                oEmpresa.Certificado = "";
                oEmpresa.X509Certificado = oMeuCert;

                lblCertificadoArquivo.Location = new Point(3, 30);
                lblCertificadoArquivo.Refresh();
                txtArquivoCertificado.Location = new Point(6, 46);
                txtArquivoCertificado.Refresh();
                lblSenhaCertificado.Location = new Point(3, 72);
                lblSenhaCertificado.Refresh();
                txtSenhaCertificado.Location = new Point(6, 88);
                txtSenhaCertificado.Refresh();
            }
        }
    }
}

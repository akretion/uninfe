using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Service;

namespace NFe.Interface
{
    public partial class FormConsultaCadastro : Form
    {
        ArrayList arrUF = new ArrayList();
        ArrayList empresa = new ArrayList();
        ArrayList arrAmb = new ArrayList();
        int Emp;

        public FormConsultaCadastro()
        {
            InitializeComponent();
        }

        private void FormConsultaCadastro_Load(object sender, EventArgs e)
        {
            PreencheEstados();
            PopulateCbEmpresa();
            XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
            iniFile.LoadForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"), true);
        }

        private void FormConsultaCadastro_FormClosed(object sender, FormClosedEventArgs e)
        {
            XMLIniFile iniFile = new XMLIniFile(Propriedade.NomeArqXMLParams);
            iniFile.SaveForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"));
            iniFile.Save();
        }

        private void PreencheEstados()
        {
            this.cbVersao.SelectedIndex = 0;
            this.cbVersaoConsCad.SelectedIndex = 0;

            try
            {
                arrUF = Functions.CarregaUF();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            comboUf.DataSource = arrUF;
            comboUf.DisplayMember = "nome";
            comboUf.ValueMember = "valor";

            this.textConteudo.Text =
               this.toolStripStatusLabel1.Text = "";

            this.buttonPesquisa.Enabled = false;

            this.cbEmissao.Items.Clear();
            this.cbEmissao.Items.Add(Propriedade.tpEmissao[(int)NFe.Components.TipoEmissao.teNormal]);
            this.cbEmissao.Items.Add(Propriedade.tpEmissao[(int)NFe.Components.TipoEmissao.teSCAN]);
            this.cbEmissao.Items.Add(Propriedade.tpEmissao[(int)NFe.Components.TipoEmissao.teSVCRS]);
            this.cbEmissao.Items.Add(Propriedade.tpEmissao[(int)NFe.Components.TipoEmissao.teSVCSP]);
            this.cbEmissao.Items.Add(Propriedade.tpEmissao[(int)NFe.Components.TipoEmissao.teSVCAN]);

            #region Montar Array DropList do Ambiente
            arrAmb.Add(new ComboElem("Produção", (int)NFe.Components.TipoAmbiente.taProducao));
            arrAmb.Add(new ComboElem("Homologação", (int)NFe.Components.TipoAmbiente.taHomologacao));

            cbAmbiente.DataSource = arrAmb;
            cbAmbiente.DisplayMember = "valor";
            cbAmbiente.ValueMember = "codigo";
            #endregion

            #region cbServico
            cbServico.Items.Clear();
            cbServico.Items.Add("NF-e"); //0
            cbServico.Items.Add("CT-e"); //1
            cbServico.Items.Add("NFS-e"); //2
            cbServico.Items.Add("MDF-e"); //3
            cbServico.Items.Add("NFC-e"); //4
            #endregion
        }

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
                cbEmpresa.DisplayMember = NFe.Components.NFeStrConstants.Nome;
                cbEmpresa.ValueMember = "Valor";
            }
        }
        #endregion

        private const string _wait = "Consultando o servidor. Aguarde....";

        private void buttonStatusServidor_Click(object sender, EventArgs e)
        {
            this.textResultado.Text = "";
            this.Refresh();
            
            TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedIndex;

            if (servico == TipoAplicativo.Cte)
            {
                if (this.cbEmissao.SelectedIndex == 1)
                {
                    MessageBox.Show("CT-e não dispõe do tipo de contingência SCAN.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else if (this.cbEmissao.SelectedIndex == 4)
                {
                    MessageBox.Show("CT-e não dispõe do tipo de contingência SVCAN.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (servico == TipoAplicativo.Nfe)
            {
                if (this.cbEmissao.SelectedIndex == 3)
                {
                    MessageBox.Show("NF-e não dispõe do tipo de contingência SCVSP.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (servico == TipoAplicativo.Nfse)
            {
                MessageBox.Show("NFS-e não dispõe do serviço de consulta status.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (servico == TipoAplicativo.MDFe)
            {
                if (this.cbEmissao.SelectedIndex != 0)
                {
                    MessageBox.Show("MDF-e só dispõe do tipo de emissão Normal.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (servico == TipoAplicativo.NFCe)
            {
                if (this.cbEmissao.SelectedIndex != 0)
                {
                    MessageBox.Show("MDF-e só dispõe do tipo de emissão Normal.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            this.toolStripStatusLabel1.Text = _wait;
            this.Refresh();

            this.Cursor = Cursors.WaitCursor;
            try
            {
                GerarXML oGerar = new GerarXML(Emp);

                int cUF = ((ComboElem)(new System.Collections.ArrayList(arrUF))[comboUf.SelectedIndex]).Codigo;
                int amb = ((ComboElem)(new System.Collections.ArrayList(arrAmb))[cbAmbiente.SelectedIndex]).Codigo;
                int tpEmis = (int)NFe.Components.TipoEmissao.teNormal;

                switch (this.cbEmissao.SelectedIndex)
                {
                    case 0:
                        tpEmis = (int)NFe.Components.TipoEmissao.teNormal;
                        break;
                    case 1:
                        tpEmis = (int)NFe.Components.TipoEmissao.teSCAN;
                        break;
                    case 2:
                        tpEmis = (int)NFe.Components.TipoEmissao.teSVCRS;
                        break;
                    case 3:
                        tpEmis = (int)NFe.Components.TipoEmissao.teSVCSP;
                        break;
                    case 4:
                        tpEmis = (int)NFe.Components.TipoEmissao.teSVCAN;
                        break;
                }

                string XmlNfeDadosMsg = Empresas.Configuracoes[Emp].PastaXmlEnvio + "\\" + oGerar.StatusServico(servico, tpEmis, cUF, amb, this.cbVersao.SelectedItem.ToString());

                //Demonstrar o status do serviço
                this.textResultado.Text = VerStatusServico(XmlNfeDadosMsg);
            }
            catch (Exception ex)
            {
                this.textResultado.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.toolStripStatusLabel1.Text = "";
            }
        }

        private void buttonPesquisaCNPJ_Click(object sender, EventArgs e)
        {
            this.textConteudo.Focus();
            this.toolStripStatusLabel1.Text = _wait;
            this.textResultado.Text = "";
            this.Refresh();

            this.Cursor = Cursors.WaitCursor;

            RetConsCad vConsCad = null;
            try
            {
                object vvConsCad = null;

                if (rbCNPJ.Checked)
                    vvConsCad = ConsultaCadastro((string)this.comboUf.SelectedValue, this.textConteudo.Text, string.Empty, string.Empty);
                else
                    if (rbCPF.Checked)
                        vvConsCad = ConsultaCadastro((string)this.comboUf.SelectedValue, string.Empty, string.Empty, this.textConteudo.Text);
                    else
                        vvConsCad = ConsultaCadastro((string)this.comboUf.SelectedValue, string.Empty, this.textConteudo.Text, string.Empty);

                if (vvConsCad is RetConsCad)
                {
                    vConsCad = (vvConsCad as RetConsCad);
                    if (vConsCad == null)
                        this.textResultado.Text = "Não pode obter a resposta do Sefaz";
                }
                else
                {
                    throw new Exception((string)vvConsCad);
                }
            }
            catch (Exception ex)
            {
                this.textResultado.Text = ex.Message;
                vConsCad = null;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.toolStripStatusLabel1.Text = "";
                if (vConsCad != null)
                {
                    if (vConsCad.infCad.Count > 0)
                    {
                        using (FormConsultaCadastroResult fResult = new FormConsultaCadastroResult(vConsCad))
                        {
                            fResult.ShowDialog();
                        }
                    }
                    else
                        this.textResultado.Text = vConsCad.xMotivo;
                }
            }
        }

        private void rbCNPJ_CheckedChanged(object sender, EventArgs e)
        {
            this.textConteudo.Mask = "00,000,000/0000-00";
            this.textConteudo.SelectionStart = 0;
        }

        private void rbCPF_CheckedChanged(object sender, EventArgs e)
        {
            this.textConteudo.Mask = "000,000,000-00";
            this.textConteudo.SelectionStart = 0;
        }

        private void rbIE_CheckedChanged(object sender, EventArgs e)
        {
            this.textConteudo.Mask = "";
            this.textConteudo.SelectionStart = 0;
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.buttonPesquisa.Enabled = this.textConteudo.Text.Replace(",", "").Replace(".", "").Replace("-", "").Replace("/", "").Trim() != "";
        }

        private void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDetalheForm();
        }

        /// <summary>
        /// Popular detalhes do form de acordo com a empresa selecionada
        /// </summary>
        private void PopulateDetalheForm()
        {
            Emp = ((ComboElem)(new System.Collections.ArrayList(empresa))[cbEmpresa.SelectedIndex]).Codigo;

            if (Empresas.Configuracoes[Emp].Servico == TipoAplicativo.Nfse)
            {
                MessageBox.Show("NFS-e não dispõe do serviço de consulta status.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //Posicionar o elemento da combo UF
            foreach (ComboElem elem in arrUF)
            {
                if (elem.Codigo == Empresas.Configuracoes[Emp].UnidadeFederativaCodigo)
                {
                    comboUf.SelectedValue = elem.Valor;
                    break;
                }
            }
            //Posicionar o elemento da combo Ambiente
            cbAmbiente.SelectedValue = Empresas.Configuracoes[Emp].AmbienteCodigo;

            //Posicionar o elemento da combo tipo de emissão
            switch ((NFe.Components.TipoEmissao)Empresas.Configuracoes[Emp].tpEmis)
            {
                case NFe.Components.TipoEmissao.teNormal:
                    this.cbEmissao.SelectedIndex = 0;
                    break;
                case NFe.Components.TipoEmissao.teSCAN:
                    this.cbEmissao.SelectedIndex = 1;
                    break;
                case NFe.Components.TipoEmissao.teSVCRS:
                    this.cbEmissao.SelectedIndex = 2;
                    break;
                case NFe.Components.TipoEmissao.teSVCSP:
                    this.cbEmissao.SelectedIndex = 3;
                    break;
                case NFe.Components.TipoEmissao.teSVCAN:
                    this.cbEmissao.SelectedIndex = 4;
                    break;
            }

            if (Empresas.Configuracoes[Emp].Servico != TipoAplicativo.Nfse)
            {
                cbServico.SelectedIndex = (int)Empresas.Configuracoes[Emp].Servico;
            }
        }

        #region VerStatusServico() e ConsultaCadastro()

        /// <summary>
        /// Verifica e retorna o Status do Servido da NFE. Para isso este método gera o arquivo XML necessário
        /// para obter o status do serviõ e faz a leitura do XML de retorno, disponibilizando uma string com a mensagem
        /// obtida.
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public string VerStatusServico(string XmlNfeDadosMsg)
        {
            string ArqXMLRetorno = Empresas.Configuracoes[Emp].PastaXmlRetorno + "\\" +
                      Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.ExtEnvio.PedSta_XML) +
                      "-sta.xml";

            string ArqERRRetorno = Empresas.Configuracoes[Emp].PastaXmlRetorno + "\\" +
                      Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.ExtEnvio.PedSta_XML) +
                      "-sta.err";

            string result = string.Empty;

            try
            {
                result = (string)EnviaArquivoERecebeResposta(1, ArqXMLRetorno, ArqERRRetorno);
            }
            finally
            {
                Functions.DeletarArquivo(ArqERRRetorno);
                Functions.DeletarArquivo(ArqXMLRetorno);
            }

            return result;
        }

        /// <summary>
        /// Função Callback que analisa a resposta do Status do Servido
        /// </summary>
        /// <param name="elem"></param>
        /// <by>Marcos Diez</by>
        /// <date>30/8/2009</date>
        /// <returns></returns>
        private static string ProcessaStatusServico(string cArquivoXML)//XmlTextReader elem)
        {
            string rst = "Erro na leitura do XML " + cArquivoXML;
            XmlTextReader elem = new XmlTextReader(cArquivoXML);
            try
            {
                while (elem.Read())
                {
                    if (elem.NodeType == XmlNodeType.Element)
                    {
                        if (elem.Name == "xMotivo")
                        {
                            elem.Read();
                            rst = elem.Value;
                            break;
                        }
                    }
                }
            }
            finally
            {
                elem.Close();
            }

            return rst;
        }

        /// <summary>
        /// VerConsultaCadastroClass
        /// </summary>
        /// <param name="XmlNfeDadosMsg"></param>
        /// <returns></returns>
        public object VerConsultaCadastro(string XmlNfeDadosMsg)
        {
            string ArqXMLRetorno = Empresas.Configuracoes[Emp].PastaXmlRetorno + "\\" +
                       Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.ExtEnvio.ConsCad_XML) +
                       "-ret-cons-cad.xml";

            string ArqERRRetorno = Empresas.Configuracoes[Emp].PastaXmlRetorno + "\\" +
                      Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.ExtEnvio.ConsCad_XML) +
                      "-ret-cons-cad.err";

            object vRetorno = null;
            try
            {
                vRetorno = EnviaArquivoERecebeResposta(2, ArqXMLRetorno, ArqERRRetorno);
            }
            finally
            {
                Functions.DeletarArquivo(ArqERRRetorno);
                Functions.DeletarArquivo(ArqXMLRetorno);
            }
            return vRetorno;
        }

        #region ConsultaCadastro()
        /// <summary>
        /// Verifica um cadastro no site da receita.
        /// Voce deve preencher o estado e mais um dos tres itens: CPNJ, IE ou CPF.
        /// </summary>
        /// <param name="uf">Sigla do UF do cadastro a ser consultado. Tem que ter dois dígitos. SU para suframa.</param>
        /// <param name="cnpj"></param>
        /// <param name="ie"></param>
        /// <param name="cpf"></param>
        /// <returns>String com o resultado da consuta do cadastro</returns>
        /// <by>Marcos Diez</by>
        /// <date>29/08/2009</date>
        public object ConsultaCadastro(string uf, string cnpj, string ie, string cpf)
        {
            GerarXML oGerar = new GerarXML(Emp);

            //Criar XML para obter o cadastro do contribuinte
            string XmlNfeConsultaCadastro = oGerar.ConsultaCadastro(string.Empty, uf, cnpj, ie, cpf, this.cbVersaoConsCad.SelectedItem.ToString());

            return VerConsultaCadastro(XmlNfeConsultaCadastro);
        }
        #endregion

        /// <summary>
        /// Envia um arquivo para o webservice da NFE e recebe a resposta. 
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2009</date>
        private object EnviaArquivoERecebeResposta(int tipo, string arqXMLRetorno, string arqERRRetorno)
        {
            object vStatus = "Ocorreu uma falha ao tentar obter a situação do serviço junto ao SEFAZ.\r\n\r\n" +
                "O problema pode ter ocorrido por causa dos seguintes fatores:\r\n\r\n" +
                "- Problema com o certificado digital\r\n" +
                "- Necessidade de atualização da cadeia de certificados digitais\r\n" +
                "- Falha de conexão com a internet\r\n" +
                "- Falha nos servidores do SEFAZ\r\n\r\n" +
                "Afirmamos que a produtora do software não se responsabiliza por decisões tomadas e/ou execuções realizadas com base nas informações acima.\r\n\r\n";

            DateTime startTime;
            DateTime stopTime;
            TimeSpan elapsedTime;

            long elapsedMillieconds;
            startTime = DateTime.Now;

            while (true)
            {
                stopTime = DateTime.Now;
                elapsedTime = stopTime.Subtract(startTime);
                elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                if (elapsedMillieconds >= 60000)
                {
                    break;
                }

                if (File.Exists(arqXMLRetorno))
                {
                    if (!Functions.FileInUse(arqXMLRetorno))
                    {
                        try
                        {
                            //Ler o status do serviço no XML retornado pelo WebService
                            //XmlTextReader oLerXml = new XmlTextReader(ArqXMLRetorno);

                            try
                            {
                                //GerarXML oGerar = new GerarXML(Emp);
                                if (tipo == 1)
                                    vStatus = ProcessaStatusServico(arqXMLRetorno);
                                else
                                    vStatus = new GerarXML(Emp).ProcessaConsultaCadastro(arqXMLRetorno);
                            }
                            catch (Exception ex)
                            {
                                vStatus = ex.Message;
                                break;
                                //Se não conseguir ler o arquivo vai somente retornar ao loop para tentar novamente, pois 
                                //pode ser que o arquivo esteja em uso ainda.
                            }

                            //Detetar o arquivo de retorno
                            try
                            {
                                FileInfo oArquivoDel = new FileInfo(arqXMLRetorno);
                                oArquivoDel.Delete();
                                break;
                            }
                            catch
                            {
                                //Somente deixa fazer o loop novamente e tentar deletar
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (File.Exists(arqERRRetorno))
                {
                    //Retornou um arquivo com a extensão .ERR, ou seja, deu um erro,
                    //futuramente tem que retornar esta mensagem para a MessageBox do usuário.

                    //Detetar o arquivo de retorno
                    try
                    {
                        vStatus += System.IO.File.ReadAllText(arqERRRetorno, Encoding.Default);
                        System.IO.File.Delete(arqERRRetorno);
                        break;
                    }
                    catch
                    {
                        //Somente deixa fazer o loop novamente e tentar deletar
                    }
                }

                Thread.Sleep(3000);
            }

            //Retornar o status do serviço
            return vStatus;
        }
        #endregion

        private void cbServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedIndex;
            this.cbVersao.Enabled = servico == TipoAplicativo.Nfe;
        }
    }
}

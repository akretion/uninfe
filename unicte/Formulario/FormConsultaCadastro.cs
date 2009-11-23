using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using unicte;
using UniNFeLibrary;

namespace uninfe.Formulario
{
    public partial class FormConsultaCadastro : Form
    {
        ArrayList arrUF = new ArrayList();

        public FormConsultaCadastro()
        {
            InitializeComponent();
        }

        private void FormConsultaCadastro_Load(object sender, EventArgs e)
        {
            PreencheEstados();
            XMLIniFile iniFile = new XMLIniFile(InfoApp.NomeArqXMLParams());
            iniFile.LoadForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"));
        }

        private void FormConsultaCadastro_FormClosed(object sender, FormClosedEventArgs e)
        {
            XMLIniFile iniFile = new XMLIniFile(InfoApp.NomeArqXMLParams());
            iniFile.SaveForm(this, (this.MdiParent == null ? "\\Normal" : "\\MDI"));
            iniFile.Save();
        }

        private void PreencheEstados()
        {
            arrUF.Add(new ComboElem("AC", 12, "Acre")); 
            arrUF.Add(new ComboElem("AL", 27, "Alagoas"));
            arrUF.Add(new ComboElem("AP", 16, "Amapá"));
            arrUF.Add(new ComboElem("AM", 13, "Amazonas"));
            arrUF.Add(new ComboElem("BA", 29, "Bahia"));
            arrUF.Add(new ComboElem("CE", 23, "Ceará"));
            arrUF.Add(new ComboElem("DF", 53, "Distrito Federal"));
            arrUF.Add(new ComboElem("ES", 32, "Espirito Santo"));
            arrUF.Add(new ComboElem("GO", 52, "Goias"));
            arrUF.Add(new ComboElem("MA", 21, "Maranhão"));
            arrUF.Add(new ComboElem("MT", 51, "Mato Grosso"));
            arrUF.Add(new ComboElem("MS", 50, "Mato Grosso do Sul"));
            arrUF.Add(new ComboElem("MG", 31, "Minas Gerais"));
            arrUF.Add(new ComboElem("PA", 15, "Pará"));                  
            arrUF.Add(new ComboElem("PB", 25, "Paraíba"));               
            arrUF.Add(new ComboElem("PR", 41, "Paraná"));                
            arrUF.Add(new ComboElem("PE", 26, "Pernambuco"));            
            arrUF.Add(new ComboElem("PI", 22, "Piauí"));                 
            arrUF.Add(new ComboElem("RJ", 33, "Rio de Janeiro"));        
            arrUF.Add(new ComboElem("RN", 24, "Rio Grande do Norte"));   
            arrUF.Add(new ComboElem("RS", 43, "Rio Grande do Sul"));     
            arrUF.Add(new ComboElem("RO", 11, "Rondônia"));              
            arrUF.Add(new ComboElem("RR", 14, "Roraima"));
            arrUF.Add(new ComboElem("SC", 42, "Santa Catarina"));
            arrUF.Add(new ComboElem("SP", 35, "São Paulo"));
            arrUF.Add(new ComboElem("SE", 28, "Sergipe"));
            arrUF.Add(new ComboElem("SU", 0,  "Suframa"));
            arrUF.Add(new ComboElem("TO", 17, "Tocantins"));

            comboUf.DataSource = arrUF;
            comboUf.DisplayMember = "nome";
            comboUf.ValueMember = "valor";

            foreach (ComboElem elem in arrUF)
            {
                if (elem.UFCod == ConfiguracaoApp.UFCod)
                {
                    comboUf.SelectedValue = elem.Valor;
                    break;
                }
            }

            this.textConteudo.Text = 
               this.toolStripStatusLabel1.Text = "";

            this.buttonPesquisa.Enabled = false;

            this.cbEmissao.Items.Clear();
            this.cbEmissao.Items.Add(UniNFeConsts.tpEmissao[TipoEmissao.teNormal]);
            this.cbEmissao.Items.Add(UniNFeConsts.tpEmissao[TipoEmissao.teContingencia]);
            this.cbEmissao.Items.Add(UniNFeConsts.tpEmissao[TipoEmissao.teSCAN]);
            this.cbEmissao.Items.Add(UniNFeConsts.tpEmissao[TipoEmissao.teDPEC]);
            this.cbEmissao.Items.Add(UniNFeConsts.tpEmissao[TipoEmissao.teFSDA]);
            this.cbEmissao.SelectedIndex = ConfiguracaoApp.tpEmis - 1;
        }

        private const string _wait = "Consultando o servidor. Aguarde....";

        private void buttonStatusServidor_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = _wait;
            this.textResultado.Text = "";
            this.Refresh();

            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Carregar configurações
                ServicoCTe oNfe = new ServicoCTe();

                int cUF = ((ComboElem)(new System.Collections.ArrayList(arrUF))[comboUf.SelectedIndex]).UFCod;

                //Demonstrar o status do serviço
                this.textResultado.Text = oNfe.StatusServico(this.cbEmissao.SelectedIndex + 1, cUF);
            }
            catch (Exception ex)
            {
                this.textResultado.Text = (ex.InnerException!=null?ex.InnerException.Message:ex.Message);
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

            UniNFeLibrary.RetConsCad vConsCad = null;
            try
            {
                ServicoCTe oNfe = new ServicoCTe();
                object vvConsCad = null;

                if (rbCNPJ.Checked)
                    vvConsCad = oNfe.ConsultaCadastroClass((string)this.comboUf.SelectedValue, this.textConteudo.Text, string.Empty, string.Empty);
                else
                    if (rbCPF.Checked)
                        vvConsCad = oNfe.ConsultaCadastroClass((string)this.comboUf.SelectedValue, string.Empty, string.Empty, this.textConteudo.Text);
                    else
                        vvConsCad = oNfe.ConsultaCadastroClass((string)this.comboUf.SelectedValue, string.Empty, this.textConteudo.Text, string.Empty);

                if (vvConsCad is UniNFeLibrary.RetConsCad)
                {
                    vConsCad = (vvConsCad as UniNFeLibrary.RetConsCad);
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
                this.textResultado.Text = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
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

        internal class ComboElem
        {
            private string _nome;
            private string _valor;
            private int _UFCod;

            public ComboElem(string valor, int ufcod, string nome)
            {
                this._nome = nome;
                this._valor = valor;
                this._UFCod = ufcod;
            }
            public string Nome { get { return _nome; } }
            public string Valor { get { return _valor; } }
            public int UFCod { get { return _UFCod; } }
        }
    }
}

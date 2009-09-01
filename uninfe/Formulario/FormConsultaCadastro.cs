using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace uninfe.Formulario
{

  

    public partial class FormConsultaCadastro : Form
    {
        public FormConsultaCadastro()
        {
            InitializeComponent();
        }

        private void FormConsultaCadastro_Load(object sender, EventArgs e)
        {
            PreencheEstados();
        }

        void PreencheEstados()
        {
               ArrayList arrUF = new ArrayList();
               arrUF.Add(new ComboElem("AC", "Acre"));
               arrUF.Add(new ComboElem("AL", "Alagoas"));
               arrUF.Add(new ComboElem("AP", "Amapá"));
               arrUF.Add(new ComboElem("AM", "Amazonas"));
               arrUF.Add(new ComboElem("BA", "Bahia"));
               arrUF.Add(new ComboElem("CE", "Ceará"));
               arrUF.Add(new ComboElem("DF", "Distrito Federal"));
               arrUF.Add(new ComboElem("ES", "Espirito Santo"));
               arrUF.Add(new ComboElem("GO", "Goias"));
               arrUF.Add(new ComboElem("MA", "Maranhão"));
               arrUF.Add(new ComboElem("MT", "Mato Grosso"));
               arrUF.Add(new ComboElem("MS", "Mato Grosso do Sul"));
               arrUF.Add(new ComboElem("MG", "Minas Gerais"));
               arrUF.Add(new ComboElem("PA", "Pará"));
               arrUF.Add(new ComboElem("PB", "Paraíba"));
               arrUF.Add(new ComboElem("PR", "Paraná"));
               arrUF.Add(new ComboElem("PE", "Pernambuco"));
               arrUF.Add(new ComboElem("PI", "Piauí"));
               arrUF.Add(new ComboElem("RJ", "Rio de Janeiro"));
               arrUF.Add(new ComboElem("RN", "Rio Grande do Norte"));
               arrUF.Add(new ComboElem("RS", "Rio Grande do Sul"));
               arrUF.Add(new ComboElem("RO", "Rondônia"));
               arrUF.Add(new ComboElem("RR", "Roraima"));
               arrUF.Add(new ComboElem("SC", "Santa Catarina"));
               arrUF.Add(new ComboElem("SP", "São Paulo"));
               arrUF.Add(new ComboElem("SE", "Sergipe"));
               arrUF.Add(new ComboElem("SU", "Suframa"));
               arrUF.Add(new ComboElem("TO", "Tocantins"));

               comboUf.DataSource = arrUF;
               comboUf.DisplayMember = "nome";
               comboUf.ValueMember = "valor";
            
        }

        private void buttonStatusServidor_Click(object sender, EventArgs e)
        {
            textResultado.Text = "Consultando o Servidor. Aguarde....";
            this.Refresh();
            //Carregar configurações
            ServicoNFe oNfe = new ServicoNFe();

            //Demonstrar o status do serviço

            textResultado.Text = "Status do servidor: " + oNfe.VerStatusServico();
        }

        private void buttonPesquisaCNPJ_Click(object sender, EventArgs e)
        {
            textResultado.Text = "Consultando o Servidor. Aguarde....";
            this.Refresh();
            ServicoNFe oNfe = new ServicoNFe();
            var saida = oNfe.ConsultaCadastro( (string) comboUf.SelectedValue , textCnpj.Text , "", "");
            textResultado.Text = saida;       
        }

        private void buttonPesquisaCPF_Click(object sender, EventArgs e)
        {
            textResultado.Text = "Consultando o Servidor. Aguarde....";
            this.Refresh();
            ServicoNFe oNfe = new ServicoNFe();
            var saida = oNfe.ConsultaCadastro((string)comboUf.SelectedValue, "", "" , textCpf.Text );
            textResultado.Text = saida;       
        }

        private void buttonPesquisaIE_Click(object sender, EventArgs e)
        {
            textResultado.Text = "Consultando o Servidor. Aguarde....";
            this.Refresh();
            ServicoNFe oNfe = new ServicoNFe();
            var saida = oNfe.ConsultaCadastro((string)comboUf.SelectedValue, "", textIE.Text, "");
            textResultado.Text = saida;     
        }



       

    }

    class ComboElem
    {
        public string _nome; public string Nome { get { return _nome; } }
        public string _valor; public string Valor { get { return _valor; } }

        public ComboElem(string valor, string nome)
        {
            this._nome = nome;
            this._valor = valor;
        }
    }
}

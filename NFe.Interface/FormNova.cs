using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NFe.Components;
using NFe.Settings;

namespace NFe.Interface
{
    public partial class FormNova : Form
    {
        private ArrayList arrServico = new ArrayList();

        public FormNova()
        {
            InitializeComponent();
        }

        private void FormNova_Load(object sender, EventArgs e)
        {
            switch (Propriedade.TipoExecucao)
            {
                case TipoExecucao.teAll:
                case TipoExecucao.teNFe:
                    arrServico.Add(new ComboElem("Todos", (int)TipoAplicativo.Todos));
                    arrServico.Add(new ComboElem("NF-e", (int)TipoAplicativo.Nfe));
                    arrServico.Add(new ComboElem("CT-e", (int)TipoAplicativo.Cte));
                    arrServico.Add(new ComboElem("MDF-e", (int)TipoAplicativo.MDFe));
                    arrServico.Add(new ComboElem("NFC-e", (int)TipoAplicativo.NFCe));
                    if (Propriedade.TipoExecucao == TipoExecucao.teAll)
                        arrServico.Add(new ComboElem("NFS-e", (int)TipoAplicativo.Nfse));
                    break;
                case TipoExecucao.teNFSe:
                    arrServico.Add(new ComboElem("NFS-e", (int)TipoAplicativo.Nfse));
                    break;
            }
            cbServico.DataSource = arrServico;
            cbServico.DisplayMember = "valor";
            cbServico.ValueMember = "codigo";

            cbServico.SelectedIndex = 0;
            edtCNPJ.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cnpj = Functions.OnlyNumbers(this.edtCNPJ.Text, ".,-/").ToString();
            TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedValue;

            if (!CNPJ.Validate(cnpj))
            {
                this.edtCNPJ.Focus();
                MessageBox.Show("CNPJ inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Empresas.FindConfEmpresa(cnpj, servico) != null)
            {
                MessageBox.Show("Empresa/CNPJ para atender o serviço de " + servico.ToString() + " já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}

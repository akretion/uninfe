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
            arrServico.Add(new ComboElem("NF-e, NFC-e, CT-e e MDF-e", (int)TipoAplicativo.Todos));
            arrServico.Add(new ComboElem("NF-e e NFC-e", (int)TipoAplicativo.Nfe));
            arrServico.Add(new ComboElem("CT-e", (int)TipoAplicativo.Cte));
            arrServico.Add(new ComboElem("MDF-e", (int)TipoAplicativo.MDFe));
            arrServico.Add(new ComboElem("NFC-e", (int)TipoAplicativo.NFCe));
            arrServico.Add(new ComboElem("NFS-e", (int)TipoAplicativo.Nfse));

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

            Empresa empresa = null;
            switch (servico)
            {
                case TipoAplicativo.Todos:
                case TipoAplicativo.Nfe:
                    //Serviço todos e NFe utilizam a mesma pasta de configurações, então não posso permitir configurar o mesmo CNPJ para os dois serviços. Wandrey
                    if ((empresa = Empresas.FindConfEmpresa(cnpj, TipoAplicativo.Todos)) == null)
                        empresa = Empresas.FindConfEmpresa(cnpj, TipoAplicativo.Nfe);
                    break;

                default:
                    empresa = Empresas.FindConfEmpresa(cnpj, servico);
                    break;
            }

            if (empresa != null)
            {
                string msgErro = "Já existe uma Empresa/CNPJ configurada para atender este serviço, conforme dados abaixo: " +
                                 "\r\n\r\nEmpresa configurada: " + empresa.Nome +
                                 "\r\nServiço configurado: " + NFe.Components.EnumHelper.GetDescription(empresa.Servico);

                MessageBox.Show(msgErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}

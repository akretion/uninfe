using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;

using NFe.Components;


namespace NFe.UI.Formularios.NFSe
{
    public partial class FormMunicipio : MetroFramework.Forms.MetroForm
    {
        public FormMunicipio()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uninfeDummy.ClearControls(this, true, true);

            foreach (var estado in Propriedade.Estados)
            {
                this.edtUF.Items.Add(estado.UF);
            }
            this.edtUF.SelectedIndex = 0;

            var _PadroesDataSource = new List<PadroesDataSource>();
            XElement axml = XElement.Load(Propriedade.NomeArqXMLWebService_NFSe);
            var xs = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                      where p.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value == "XX" &&
                             p.Attribute(NFe.Components.TpcnResources.ID.ToString()).Value == p.Attribute(NFe.Components.NFeStrConstants.Padrao).Value
                      orderby p.Attribute(NFe.Components.NFeStrConstants.Padrao).Value
                      select p);
            foreach (var item in xs)
            {
                PadroesNFSe type = WebServiceNFSe.GetPadraoFromString(item.Attribute(NFe.Components.NFeStrConstants.Padrao).Value);
                _PadroesDataSource.Add(new PadroesDataSource { fromType = type.ToString(), fromDescription = EnumHelper.GetEnumItemDescription(type) });
            }

            this.edtPadrao.Sorted = false;
            this.edtPadrao.DataSource = _PadroesDataSource;// WebServiceNFSe.PadroesNFSeListDataSource.Where(p => p.fromType != PadroesNFSe.NaoIdentificado.ToString()).ToList();
            this.edtPadrao.ValueMember = "fromType";
            this.edtPadrao.DisplayMember = "fromDescription";
            
            this.edtPadrao.SelectedIndex = 0;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.edtCodMun.Focus();
        }

        private void edtCodMun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void edtCodMun_Leave(object sender, EventArgs e)
        {
            this.edtCodMun.Text = this.edtCodMun.Text.PadLeft(7, '0');
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.edtCodMun.Focus();
            if (Functions.CodigoParaUF(Convert.ToInt32(this.edtCodMun.Text.Substring(0, 2))) != this.edtUF.SelectedItem.ToString())
            {
                MetroFramework.MetroMessageBox.Show(this, "Código do IBGE diverge da UF", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.edtMunicipio.Text == "")
            {
                MetroFramework.MetroMessageBox.Show(this, "Nome do município deve ser informado", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.edtMunicipio.Focus();
                return;
            }

            foreach (Municipio mun in Propriedade.Municipios)
                if (mun.CodigoMunicipio.ToString() == this.edtCodMun.Text)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Código IBGE já definido no municipio \"" + mun.Nome + "\"", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            try
            {
                WebServiceNFSe.SalvarXMLMunicipios(this.edtUF.SelectedItem.ToString(), 
                    this.edtMunicipio.Text, 
                    Convert.ToInt32(this.edtCodMun.Text),
                    (this.edtPadrao.SelectedItem as PadroesDataSource).fromType, 
                    false);
                
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(null, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void edtMunicipio_TextChanged(object sender, EventArgs e)
        {
            this.metroButton1.Enabled = !this.edtCodMun.Text.PadLeft(7, '0').Equals("0000000") &&
                this.edtMunicipio.Text.Trim().Length > 0;
        }
    }
}

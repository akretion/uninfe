using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NFe.Components;

namespace NFe.UI.Formularios.NFSe
{
    [ToolboxItem(false)]
    public partial class userGrid2 : MetroFramework.Controls.MetroUserControl
    {
        public userGrid2()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uninfeDummy.ClearControls(this, true, false);

            //this.colPadrao_D.Items.AddRange(WebServiceNFSe.PadroesNFSeList);
            colPadrao_D.Sorted = false;
            colPadrao_D.DataSource = WebServiceNFSe.PadroesNFSeListDataSource;
            colPadrao_D.ValueMember = "fromType";
            colPadrao_D.DisplayMember = "fromDescription";
        }

        private void dgvDireto_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int codmun = Convert.ToInt32((sender as DataGridView).Rows[e.RowIndex].Cells[this.colIBGE_D.Index].Value);
                string cidade = (sender as DataGridView).Rows[e.RowIndex].Cells[this.colNome_D.Index].Value.ToString().Trim();
                string padrao = (sender as DataGridView).Rows[e.RowIndex].Cells[this.colPadrao_D.Index].Value.ToString();
                string uf = Functions.CodigoParaUF(Convert.ToInt32(codmun.ToString().Substring(0, 2)));

                WebServiceNFSe.SalvarXMLMunicipios(uf, cidade, codmun, padrao, true);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
            }
        }

        private void dgvDireto_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        public void RefreshMunicipiosDefinidos()
        {
            var final = from p in Propriedade.Municipios orderby p.Nome select p;
            this.municipioBindingSource.DataSource = final.ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (FormMunicipio f = new FormMunicipio())
            {
                if (f.ShowDialog() == DialogResult.OK)
                    this.RefreshMunicipiosDefinidos();
            }
        }
    }
}

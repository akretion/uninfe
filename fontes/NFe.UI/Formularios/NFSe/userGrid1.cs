using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using NFe.Components;

namespace NFe.UI.Formularios.NFSe
{
    [ToolboxItem(false)]
    public partial class userGrid1 : MetroFramework.Controls.MetroUserControl
    {
        private string currpadrao;
        private ArrayList arrUF = new ArrayList();
        public userMunicipios umunicipio { private get; set; }

        public userGrid1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            colPadrao.Sorted = false;
            colPadrao.DataSource = WebServiceNFSe.PadroesNFSeUnicoWSDLDataSource;
            colPadrao.ValueMember = "fromType";
            colPadrao.DisplayMember = "fromDescription";

            foreach (var estado in Propriedade.Estados)
            {
                arrUF.Add(new ComboElem(estado.UF, estado.CodigoMunicipio, estado.Nome));
            }
            
            this.comboUf.SelectedValueChanged -= comboUf_SelectedValueChanged;
            this.comboUf.DataSource = arrUF;
            this.comboUf.DisplayMember = "nome";
            this.comboUf.ValueMember = "codigo";
            this.comboUf.MaxDropDownItems = this.comboUf.Items.Count;
            this.comboUf.SelectedIndex = 0;
            this.comboUf.SelectedValueChanged += comboUf_SelectedValueChanged;

            comboUf_SelectedValueChanged(null, null);

            uninfeDummy.ClearControls(this, true, false);
        }

        private void comboUf_SelectedValueChanged(object sender, EventArgs e)
        {
            RefreshMunicipiosADefinir();
        }

        public void RefreshMunicipiosADefinir()
        {
            if (this.comboUf.SelectedValue == null) return;

            try
            {
                int cUF = ((ComboElem)(new System.Collections.ArrayList(arrUF))[comboUf.SelectedIndex]).Codigo;

                List<NFe.Components.Municipio> cidades = new List<NFe.Components.Municipio>();

                ///
                /// extrai os municipios do resource
                /// 
                Stream str = System.Reflection.Assembly.GetExecutingAssembly().
                                GetManifestResourceStream("NFe.UI.Formularios.NFSe.Municipios.MunIBGE-UF" + cUF.ToString("00") + ".txt");
                if (str != null)
                {
                    if (str.Length > 0)
                    {
                        byte[] b = new byte[str.Length];
                        str.Read(b, 0, (int)str.Length);
                        string text = System.Text.Encoding.UTF7.GetString(b);
                        if (text != "")
                        {
                            foreach (var linha in text.Split(new char[] { '\n' }))
                            {
                                if (string.IsNullOrEmpty(linha)) continue;
                                var cm = Convert.ToInt32(linha.Substring(0, 7));
                                ///
                                /// municipio já tem um padrão definido?
                                if (Propriedade.Municipios.FirstOrDefault(w => w.CodigoMunicipio == cm) == null)
                                {
                                    cidades.Add(new NFe.Components.Municipio
                                    {
                                        CodigoMunicipio = cm,
                                        Nome = linha.Substring(7).Trim(),
                                        PadraoStr = Functions.PadraoNFSe(cm).ToString()
                                    });
                                }
                            }
                        }
                    }
#if false
                    ///
                    /// pega um nome de arquivo na pasta temporaria
                    string fileTemp = Path.GetTempFileName();
                    /// cria/abre o arquivo temporario para gravar os municipios
                    FileStream fs = new FileStream(fileTemp, System.IO.FileMode.OpenOrCreate);
                    byte[] b = new byte[str.Length];
                    str.Read(b, 0, (int)str.Length);
                    fs.Write(b, 0, (int)str.Length);
                    fs.Flush();
                    fs.Close();
                    ///
                    /// popula a lista de municipios
                    string[] lines = System.IO.File.ReadAllLines(fileTemp, Encoding.UTF7);
                    foreach (string linha in lines)
                    {
                        var cm = Convert.ToInt32(linha.Substring(0, 7));
                        if (Propriedade.Municipios.FirstOrDefault(w => w.CodigoMunicipio == cm) == null)
                        {
                            cidades.Add(new NFe.Components.Municipio
                            {
                                CodigoMunicipio = cm,//Convert.ToInt32(linha.Substring(0, 7)),
                                Nome = linha.Substring(7).Trim(),
                                PadraoStr = Functions.PadraoNFSe(cm).ToString()//Convert.ToInt32(linha.Substring(0, 7).Trim())).ToString()
                            });
                        }
                    }
                    ///
                    /// exclui o arquivo temporario
                    FileInfo fi = new FileInfo(fileTemp);
                    fi.Delete();
#endif
                }
                else
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Não foi possivel ler os municipios", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                var final = from p in cidades orderby p.Nome select p;
                this.municipioBindingSource.DataSource = final.ToList();

            }
            catch (Exception ex)
            {
                this.metroGrid1.Enabled = false;
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int codmun = Convert.ToInt32((sender as DataGridView).Rows[e.RowIndex].Cells[this.colIBGE.Index].Value);
                string cidade = (sender as DataGridView).Rows[e.RowIndex].Cells[this.colNome.Index].Value.ToString().Trim();
                string padrao = (sender as DataGridView).Rows[e.RowIndex].Cells[this.colPadrao.Index].Value.ToString();
                string uf = Functions.CodigoParaUF(Convert.ToInt32(codmun.ToString().Substring(0, 2)));

                if (padrao.Equals(currpadrao)) return;

                WebServiceNFSe.SalvarXMLMunicipios(uf, cidade, codmun, padrao, false);

                if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                    ///
                    /// remove o municipio da grade
                    this.metroGrid1.Rows.RemoveAt(e.RowIndex);

                /*

                WANDREY: nao sei pq desse codigo abaixo...

                string cUF = ((ComboElem)(new System.Collections.ArrayList(this.arrUF))[this.comboUf.SelectedIndex]).Valor;
                if (cUF == uf)
                {
                    for (int n = 0; n < this.metroGrid1.RowCount; ++n)
                        if (this.metroGrid1.Rows[n].Cells[0].Value.ToString() == codmun.ToString())
                        {
                            this.metroGrid1.Rows[n].Cells[this.colPadrao.Index].Value = padrao;
                            break;
                        }
                }
                if (padrao == PadroesNFSe.NaoIdentificado.ToString())
                {
                    umunicipio.RefreshGrid2(); //nao precisa pq o refresh será efetuando quando se muda a tabsheet
                }
                */
            }
            catch (Exception ex)
            {
                (sender as DataGridView).Rows[e.RowIndex].Cells[this.colPadrao.Index].Value = currpadrao;
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void metroGrid1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            currpadrao = (sender as DataGridView).Rows[e.RowIndex].Cells[this.colPadrao.Index].Value.ToString();
        }
    }
}

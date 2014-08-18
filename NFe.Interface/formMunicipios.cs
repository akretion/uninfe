using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NFe.Components;

namespace NFe.Interface
{
    public partial class formMunicipios : Form
    {
        private EventHandler OnMyClose;
        private ArrayList arrUF = new ArrayList();

        public formMunicipios(EventHandler _OnClose)
        {
            InitializeComponent();
            this.OnMyClose = _OnClose;
        }

        private void formMunicipios_Load(object sender, EventArgs e)
        {
            this.colPadrao.Items.Clear();
            this.colPadrao.Items.AddRange(WebServiceNFSe.PadroesNFSeList);

            this.colPadrao_D.Items.Clear();
            this.colPadrao_D.Items.AddRange(WebServiceNFSe.PadroesNFSeList);
            
            for (int v = 0; v < Propriedade.CodigosEstados.Length / 2; ++v)
            {
                arrUF.Add(new ComboElem(Propriedade.CodigosEstados[v, 1].Substring(0, 2), Convert.ToInt32(Propriedade.CodigosEstados[v, 0]), Propriedade.CodigosEstados[v, 1]));
                this.edtUF.Items.Add(Propriedade.CodigosEstados[v, 1].Substring(0, 2));
            }
            this.comboUf.DataSource = arrUF;
            this.comboUf.DisplayMember = "nome";
            this.comboUf.ValueMember = "codigo";
            this.comboUf.MaxDropDownItems = this.comboUf.Items.Count;
            this.comboUf.SelectedIndex = 0;

            this.edtUF.SelectedIndex = 0;
            this.edtPadrao.Items.AddRange(WebServiceNFSe.PadroesNFSeList);
            this.edtPadrao.Items.RemoveAt(0);
            this.edtPadrao.SelectedIndex = 0;
        }

        private void formMunicipios_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.OnMyClose(sender, EventArgs.Empty);
        }

        private Timer timerRefresh = null;

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int codmun = Convert.ToInt32((sender as DataGridView).Rows[e.RowIndex].Cells[0].Value);
                string cidade = (sender as DataGridView).Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
                string padrao = (sender as DataGridView).Rows[e.RowIndex].Cells[2].Value.ToString();
                string uf = Functions.CodigoParaUF(Convert.ToInt32(codmun.ToString().Substring(0, 2)));

                WebServiceNFSe.SalvarXMLMunicipios(uf, cidade, codmun, padrao, this.tabControl1.SelectedIndex==1);

                if (tabControl1.SelectedIndex == 1)
                {
                    string cUF = ((ComboElem)(new System.Collections.ArrayList(arrUF))[comboUf.SelectedIndex]).Valor;
                    if (cUF == uf)
                    {
                        for(int n = 0; n < dataGridView1.RowCount; ++n)
                            if (dataGridView1.Rows[n].Cells[0].Value.ToString() == codmun.ToString())
                            {
                                dataGridView1.Rows[n].Cells[2].Value = padrao;
                                break;
                            }
                    }
                    if (padrao == PadroesNFSe.NaoIdentificado.ToString())
                    {
                        ///
                        /// a criacao de um timer foi um arremedo que fiz para que o municipio seja excluido da grade
                        /// tentei excluir diretamente ou dar um rebuild, mas este componente dá um erro de recursividade.
                        timerRefresh = new Timer();
                        timerRefresh.Tick += new EventHandler(timerRefresh_Tick);
                        timerRefresh.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void timerRefresh_Tick(object sender, EventArgs e)
        {
            timerRefresh.Stop();
            timerRefresh.Dispose();

            RefreshMunicipios();
        }

        internal class cidade
        {
            public string ibge { get; set; }
            public string municipio { get; set; }
            public string padrao { get; set; }

            public cidade(string _ibge, string _mun, string _padrao)
            {
                this.ibge = _ibge;
                this.municipio = _mun;
                this.padrao = _padrao;
            }
        }

        private void comboUf_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int cUF = ((ComboElem)(new System.Collections.ArrayList(arrUF))[comboUf.SelectedIndex]).Codigo;

                List<cidade> cidades = new List<cidade>();
                ///
                /// extrai os municipios do resource
                Stream str = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("NFe.Interface.Municipios.MunIBGE-UF" + cUF.ToString("00") + ".txt");
                if (str != null)
                {
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
                        cidades.Add(new cidade(linha.Substring(0, 7), linha.Substring(7), Functions.PadraoNFSe(Convert.ToInt32(linha.Substring(0, 7).Trim())).ToString()));
                    }
                    ///
                    /// esclui o arquivo temporario
                    FileInfo fi = new FileInfo(fileTemp);
                    fi.Delete();
                }
                else
                    MessageBox.Show("Não foi possivel ler os municipios");

                var final = from p in cidades orderby p.municipio select p;
                this.dataGridView1.DataSource = final.ToList();
                this.dataGridView1.Enabled = this.dataGridView1.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                this.dataGridView1.Enabled = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                RefreshMunicipios();
            }
        }

        private void RefreshMunicipios()
        {
            var final = from p in Propriedade.Municipios orderby p.Nome select p;
            this.dgvDireto.DataSource = null;
            this.dgvDireto.DataSource = final.ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int aLeft = this.dgvDireto.Location.X + (this.dgvDireto.Size.Width - this.panel1.Size.Width) / 2;
            int aTop = this.dgvDireto.Location.Y + (this.dgvDireto.Size.Height - this.panel1.Size.Height) / 2;

            this.panel1.Location = new Point(aLeft, aTop);
            this.dgvDireto.Enabled = false;
            this.panel1.Visible = true;
            this.btnAdd.Enabled = false;
            this.edtMunicipio.Text =
                this.edtCodMun.Text = "";

            this.edtCodMun.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.btnAdd.Enabled = true;
            this.dgvDireto.Enabled = true;
            this.panel1.Visible = false;
        }

        private void edtCodMun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsPunctuation(e.KeyChar))
                e.Handled = true;
        }

        private void edtCodMun_Leave(object sender, EventArgs e)
        {
            this.edtCodMun.Text = this.edtCodMun.Text.PadLeft(7, '0');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.edtCodMun.Focus();
            if (Functions.CodigoParaUF(Convert.ToInt32(this.edtCodMun.Text.Substring(0, 2))) != this.edtUF.SelectedItem.ToString())
            {
                MessageBox.Show("Código do IBGE diverge da UF");
                return;
            }
            if (this.edtMunicipio.Text == "")
            {
                MessageBox.Show("Nome do município deve ser informado");
                this.edtMunicipio.Focus();
                return;
            }

            foreach (Municipio mun in Propriedade.Municipios)
                if (mun.CodigoMunicipio.ToString() == this.edtCodMun.Text)
                {
                    MessageBox.Show("Código IBGE já definido no municipio \"" + mun.Nome + "\"");
                    return;
                }

            try
            {
                WebServiceNFSe.SalvarXMLMunicipios(this.edtUF.SelectedItem.ToString(), this.edtMunicipio.Text, Convert.ToInt32(this.edtCodMun.Text), this.edtPadrao.SelectedItem.ToString(), false);
                RefreshMunicipios();
                this.button2.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace uninfe_ws
{
    public partial class UserControl3 : MetroFramework.Controls.MetroUserControl
    {
        public UserControl3()
        {
            InitializeComponent();
        }

        private string[] labels_nfe = new string[]{
                "NFeRecepcao",
                "NFeRetRecepcao",
                "NFeInutilizacao",
                "NFeConsulta",
                "NFeStatusServico",
                "NFeConsultaCadastro",
                "NFeRecepcaoEvento",
                "NFeConsultaNFeDest",
                "NFeDownload",
                "NFeManifDest",
                "NFeAutorizacao",
                "NFeRetAutorizacao",
                "MDFeRecepcao",
                "MDFeRetRecepcao",
                "MDFeConsulta",
                "MDFeStatusServico",
                "MDFeRecepcaoEvento",
                "CTeRecepcaoEvento",
                "CTeRecepcao",
                "CTeRetRecepcao",
                "CTeInutilizacao",
                "CTeConsulta",
                "CTeStatusServico",
                "CTeConsultaCadastro"};

        private string[] labels_nfse = new string[]{
                "RecepcionarLoteRps",
                "ConsultarSituacaoLoteRps",
                "ConsultarNfsePorRps",
                "ConsultarNfse",
                "ConsultarLoteRps",
                "ConsultarURLNfse",
                "CancelarNfse"};
        private string[] xlabels;
        private string configname;
        private XmlDocument doc = new XmlDocument();
        private List<Estado> listaEstados = new List<Estado>();
        private NFe.Components.TipoAplicativo _tipo;
        private string filebackup;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.edtPadrao.Items.AddRange(NFe.Components.WebServiceNFSe.PadroesNFSeList);
            this.metroTabControl1.SelectedIndex = 0;
        }

        public void loadData(NFe.Components.TipoAplicativo opcao)
        {
            this._tipo = opcao;

            edtEstados.SelectedIndexChanged -= metroComboBox1_SelectedIndexChanged;
            try
            {
                xlabels = this._tipo == NFe.Components.TipoAplicativo.Nfse ? labels_nfse : labels_nfe;
                int X = 2;
                foreach (var label in xlabels)
                {
                    UserControl2 uc1 = new UserControl2();
                    this.tpProd.Controls.Add(uc1);
                    uc1.folder = System.IO.Path.GetDirectoryName(configname) + "\\producao";
                    uc1.metroLabel1.Text = label;
                    uc1.metroTextBox1.Text = label;
                    uc1.Name = "P_" + label;
                    uc1.Location = new Point(2, X);
                    uc1.Size = new System.Drawing.Size(this.tpProd.Size.Width - 20, uc1.Size.Height);

                    UserControl2 uc2 = new UserControl2();
                    this.tpHomo.Controls.Add(uc2);
                    uc2.folder = System.IO.Path.GetDirectoryName(configname) + "\\homologacao";
                    uc2.metroLabel1.Text = label;
                    uc2.metroTextBox1.Text = label;
                    uc2.Name = "H_" + label;
                    uc2.Location = new Point(2, X);
                    uc2.Size = new System.Drawing.Size(this.tpHomo.Size.Width - 20, uc2.Size.Height);

                    X += uc2.Size.Height + 4;
                }
                string padraobase = NFe.Components.PadroesNFSe.NaoIdentificado.ToString();
                string fn = System.IO.Path.Combine(Application.StartupPath, "uninfe_ws.xml");
                NFe.Components.XMLIniFile xml = new NFe.Components.XMLIniFile(fn);
                switch (this._tipo)
                {
                    case NFe.Components.TipoAplicativo.Nfse:
                        configname = xml.ReadValue("webservice", "uninfse", "");
                        break;

                    default:
                        padraobase = "";
                        configname = xml.ReadValue("webservice", "uninfe", "");
                        this.metroLabel5.Visible =
                            this.edtPadrao.Visible = false;
                        break;
                }
                filebackup = this.configname + ".xml.bck";

/*                if (!System.IO.File.Exists(configname + ".xml"))
                    System.IO.File.Copy(configname, configname + ".xml");
                configname += ".xml"; */

                XElement axml = XElement.Load(configname);
                var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                         where (string)p.Attribute(NFe.Components.NFeStrConstants.UF) != "XX"
                         orderby p.Attribute(NFe.Components.NFeStrConstants.Nome).Value
                         select p);
                foreach (var item in s)
                {
                    listaEstados.Add(new Estado
                    {
                        Nome = item.Attribute(NFe.Components.NFeStrConstants.Nome).Value,
                        ID = item.Attribute(NFe.Components.NFeStrConstants.ID).Value,
                        UF = item.Attribute(NFe.Components.NFeStrConstants.UF).Value,
                        Padrao = item.Attribute(NFe.Components.NFeStrConstants.Padrao) == null ? padraobase : item.Attribute(NFe.Components.NFeStrConstants.Padrao).Value
                    });
                    dummy.listageral.Remove(listaEstados[listaEstados.Count - 1].key);
                    dummy.listageral.Add(listaEstados[listaEstados.Count - 1].key, (int)this._tipo);
                }
                edtEstados.DisplayMember = "text";
                edtEstados.ValueMember = NFe.Components.NFeStrConstants.ID;
                edtEstados.DataSource = listaEstados;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(null, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                metroComboBox1_SelectedIndexChanged(null, null);
                edtEstados.SelectedIndexChanged += metroComboBox1_SelectedIndexChanged;
            }
        }

        void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                XmlDocument docx = new XmlDocument();

                var item = listaEstados[edtEstados.SelectedIndex];
                edtNome.Text = item.Nome;
                edtUF.Text = item.UF;
                edtID.Text = item.ID;
                edtPadrao.Text = item.Padrao;

                ///
                /// limpa todos os enderecos
                for (int vs = 0; vs < xlabels.Length; ++vs)
                {
                    (this.tpHomo.Controls["H_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = "";
                    (this.tpProd.Controls["P_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = "";
                }
                ///
                /// seleciona os enderecos do xml
                XElement axml = XElement.Load(configname);
                var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                         where (string)p.Attribute(NFe.Components.NFeStrConstants.UF) == item.UF &&
                                (string)p.Attribute(NFe.Components.NFeStrConstants.ID) == item.ID
                         select p);
                foreach (var itemx in s)
                {
                    if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                    {
                        if (!itemx.Attribute(NFe.Components.NFeStrConstants.Padrao).Value.Equals(item.Padrao))
                            continue;
                    }
                    docx.LoadXml(itemx.FirstNode.ToString());
                    foreach (var node in docx.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalHomologacao))
                    {
                        for (int vs = 0; vs < xlabels.Length; ++vs)
                        {
                            string c = NFe.Components.Functions.LerTag((XmlElement)node, xlabels[vs], false);
                            (this.tpHomo.Controls["H_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = c;
                        }
                    }

                    docx.LoadXml(itemx.LastNode.ToString());
                    foreach (var node in docx.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalProducao))
                    {
                        for (int vs = 0; vs < xlabels.Length; ++vs)
                        {
                            string c = NFe.Components.Functions.LerTag((XmlElement)node, xlabels[vs], false);
                            (this.tpProd.Controls["P_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = c;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(null, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.edtID.Text = this.edtID.Text.PadLeft(this._tipo == NFe.Components.TipoAplicativo.Nfse ? 7 : 2, '0');
                this.edtUF.Text = this.edtUF.Text.ToUpper();

                if (string.IsNullOrEmpty(this.edtNome.Text))
                    throw new Exception("Nome deve ser informado");

                if (Convert.ToInt32(this.edtID.Text) < 900 && this._tipo == NFe.Components.TipoAplicativo.Nfe)
                {
                    if (!NFe.Components.Functions.UFParaCodigo(this.edtUF.Text).Equals(Convert.ToInt32(this.edtID.Text)))
                    {
                        throw new Exception("Divergência entre 'UF' e 'ID'");
                    }
                }
                if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                {
                    int cUF = NFe.Components.Functions.UFParaCodigo(this.edtUF.Text);
                    if (cUF != Convert.ToInt32(this.edtID.Text.Substring(0, 2)) || cUF == 0)
                    {
                        throw new Exception("Divergência entre 'UF' e 'ID'");
                    }
                }

                string key = this.edtUF.Text + " - " +
                            this.edtID.Text +
                            (this._tipo == NFe.Components.TipoAplicativo.Nfse ? (" - " + this.edtPadrao.SelectedItem.ToString()) : "");

                Estado item = null;
                try
                {
                    item = listaEstados[edtEstados.SelectedIndex];
                }
                catch
                {
                    item = new Estado();
                }
                StringBuilder erro = new StringBuilder();
                for (int vs = 0; vs < xlabels.Length; ++vs)
                {
                    string c = (this.tpProd.Controls["P_" + xlabels[vs]] as UserControl2).metroTextBox1.Text;
                    dummy.ArquivoExiste(erro, configname, c);

                    c = (this.tpHomo.Controls["H_" + xlabels[vs]] as UserControl2).metroTextBox1.Text;
                    dummy.ArquivoExiste(erro, configname, c);
                }
                if (erro.Length > 0)
                {
                    if (!(MetroFramework.MetroMessageBox.Show(null, erro.ToString() + "\r\n\r\nConfirma mesmo assim?", "Erro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                        return;
                }

                if (this.edtUF.Text == "XX")
                {
                    throw new Exception("Não atualize nada nesta UF");
                }

                if (!key.Equals(item.key))  //é diferente da selecionada?
                {
                    if (dummy.listageral.ContainsKey(key))   //a nova chave existe?
                    {
                        throw new Exception("Já existe uma configuração com os parâmetros informados");
                    }
                }
                if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                    if (!this.edtNome.Text.Trim().EndsWith(this.edtUF.Text))
                        this.edtNome.Text += " - " + this.edtUF.Text;

                if (dummy.listageral.ContainsKey(key))   //a nova chave existe?
                {
                    item.Nome = this.edtNome.Text;
                    item.ID = this.edtID.Text;
                    item.Padrao = this.edtPadrao.SelectedItem.ToString();
                    item.UF = this.edtUF.Text;
                }
                else
                {
                    Estado estado = new Estado();
                    estado.Nome = this.edtNome.Text;
                    estado.ID = this.edtID.Text;
                    estado.Padrao = this.edtPadrao.SelectedItem.ToString();
                    estado.UF = this.edtUF.Text;

                    dummy.listageral.Add(key, (int)this._tipo);

                    listaEstados.Add(estado);
                }

                XElement[] eleProducao = new XElement[this.xlabels.Length];
                XElement[] eleHomologa = new XElement[this.xlabels.Length];
                for (int vs = 0; vs < xlabels.Length; ++vs)
                {
                    eleProducao[vs] = new XElement(xlabels[vs], (this.tpProd.Controls["P_" + xlabels[vs]] as UserControl2).metroTextBox1.Text);
                    eleHomologa[vs] = new XElement(xlabels[vs], (this.tpHomo.Controls["H_" + xlabels[vs]] as UserControl2).metroTextBox1.Text);
                }

                this.makebackup();

                bool atualizou = false;
                XElement users = XElement.Load(this.configname);
                IEnumerable<XElement> elements = users.Elements();

                if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                    elements.AncestorsAndSelf(NFe.Components.NFeStrConstants.Estado).
                        Where(x1 => x1.Attribute(NFe.Components.NFeStrConstants.ID).Value == item.ID &&
                                    x1.Attribute(NFe.Components.NFeStrConstants.Padrao).Value == item.Padrao && 
                                    x1.Attribute(NFe.Components.NFeStrConstants.UF).Value == item.UF).Remove();
                else
                    elements.AncestorsAndSelf(NFe.Components.NFeStrConstants.Estado).
                        Where(x1 => x1.Attribute(NFe.Components.NFeStrConstants.ID).Value == item.ID &&
                                    x1.Attribute(NFe.Components.NFeStrConstants.UF).Value == item.UF).Remove();
                /*
                var query =
                    from b in users.Elements()
                    where ((string)b.Attribute(NFe.Components.NFeStrConstants.UF)).Equals(this.edtUF.Text) &&
                          ((string)b.Attribute(NFe.Components.NFeStrConstants.ID)).Equals(this.edtID.Text)
                    select b;
                foreach (var cconv in query)
                {
                    if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                    {
                        if (!cconv.Attribute(NFe.Components.NFeStrConstants.Padrao).Value.Equals(item.Padrao))
                            continue;
                    }
                    cconv.SetAttributeValue(NFe.Components.NFeStrConstants.Nome, this.edtNome.Text);
                    cconv.SetAttributeValue(NFe.Components.NFeStrConstants.ID, this.edtID.Text);
                    cconv.SetAttributeValue(NFe.Components.NFeStrConstants.UF, this.edtUF.Text);
                    cconv.SetAttributeValue(NFe.Components.NFeStrConstants.Padrao, this.edtPadrao.SelectedItem.ToString());
                    cconv.SetElementValue(NFe.Components.NFeStrConstants.LocalHomologacao, eleHomologacao);
                    cconv.SetElementValue(NFe.Components.NFeStrConstants.LocalProducao, eleProducao);

                    atualizou = true;
                    break;
                }*/
                if (!atualizou)
                {
                    if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                    {
                        XElement ele = new XElement(NFe.Components.NFeStrConstants.Estado,
                           new XAttribute(NFe.Components.NFeStrConstants.ID, this.edtID.Text),
                           new XAttribute(NFe.Components.NFeStrConstants.Nome, this.edtNome.Text),
                           new XAttribute(NFe.Components.NFeStrConstants.UF, this.edtUF.Text),
                           new XAttribute(NFe.Components.NFeStrConstants.Padrao, this.edtPadrao.SelectedItem.ToString()),
                           new XElement(NFe.Components.NFeStrConstants.LocalHomologacao, eleHomologa),
                           new XElement(NFe.Components.NFeStrConstants.LocalProducao, eleProducao)
                        );
                        users.Add(ele);
                    }
                    else
                    {
                        XElement ele = new XElement(NFe.Components.NFeStrConstants.Estado,
                           new XAttribute(NFe.Components.NFeStrConstants.ID, this.edtID.Text),
                           new XAttribute(NFe.Components.NFeStrConstants.Nome, this.edtNome.Text),
                           new XAttribute(NFe.Components.NFeStrConstants.UF, this.edtUF.Text),
                           new XElement(NFe.Components.NFeStrConstants.LocalHomologacao, eleHomologa),
                           new XElement(NFe.Components.NFeStrConstants.LocalProducao, eleProducao)
                        );
                        users.Add(ele);
                    }
                }
                users.Save(this.configname);

                RefreshEstados();

                MetroFramework.MetroMessageBox.Show(null, "OK Atualizado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.restorebackup();
                MetroFramework.MetroMessageBox.Show(null, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(null, "Confirma a exclusão desta configuração?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    this.makebackup();

                    var item = listaEstados[edtEstados.SelectedIndex];

                    XElement users = XElement.Load(this.configname);
                    IEnumerable<XElement> elements = users.Elements();

                    if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                        elements.AncestorsAndSelf(NFe.Components.NFeStrConstants.Estado).
                            Where(x1 => x1.Attribute(NFe.Components.NFeStrConstants.ID).Value == item.ID &&
                                        x1.Attribute(NFe.Components.NFeStrConstants.Padrao).Value == item.Padrao &&
                                        x1.Attribute(NFe.Components.NFeStrConstants.UF).Value == item.UF).Remove();
                    else
                        elements.AncestorsAndSelf(NFe.Components.NFeStrConstants.Estado).
                            Where(x1 => x1.Attribute(NFe.Components.NFeStrConstants.ID).Value == item.ID &&
                                        x1.Attribute(NFe.Components.NFeStrConstants.UF).Value == item.UF).Remove();
                    users.Save(this.configname);

                    dummy.listageral.Remove(item.key);

                    listaEstados.RemoveAt(edtEstados.SelectedIndex);

                    RefreshEstados();
                }
                catch (Exception ex)
                {
                    this.restorebackup();
                    MetroFramework.MetroMessageBox.Show(null, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void makebackup()
        {
            System.IO.File.Copy(this.configname, this.filebackup, true);
        }

        private void restorebackup()
        {
            System.IO.File.Copy(this.filebackup, this.configname, true);
        }

        private void RefreshEstados()
        {
            edtEstados.SelectedIndexChanged -= metroComboBox1_SelectedIndexChanged;
            edtEstados.DataSource = null;
            edtEstados.DisplayMember = "text";
            edtEstados.ValueMember = NFe.Components.NFeStrConstants.ID;
            edtEstados.DataSource = listaEstados;
            edtEstados.SelectedIndex = 0;
            metroComboBox1_SelectedIndexChanged(null, null);
            edtEstados.SelectedIndexChanged += metroComboBox1_SelectedIndexChanged;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

        private List<string> xlabels = null;
        private string configname = null;
        private string uf = null;
        private XmlDocument doc = new XmlDocument();
        private List<Estado> listaEstados = new List<Estado>();
        private List<string> listageral = new List<string>();
        private NFe.Components.TipoAplicativo _tipo;
        private string filebackup;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.edtPadrao.Items.AddRange(NFe.Components.WebServiceNFSe.PadroesNFSeList);
            this.metroTabControl1.SelectedIndex = 0;
        }

        public void loadData(NFe.Components.TipoAplicativo opcao, string uf)
        {
            this._tipo = opcao;
            this.uf = uf;

            listageral.Clear();
            listaEstados.Clear();

            edtPadrao.SelectedIndexChanged -= edtPadrao_SelectedIndexChanged;
            edtEstados.SelectedIndexChanged -= metroComboBox1_SelectedIndexChanged;
            int oIndex = 0;
            try
            {
                string padraobase = NFe.Components.PadroesNFSe.NaoIdentificado.ToString();
                if (this.configname == null)
                {
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
                            this.metroLabel5.Text = "SVC";
                            this.edtPadrao.Items.Clear();
                            this.edtPadrao.Items.Add(NFe.Components.TipoEmissao.teNone.ToString());
                            this.edtPadrao.Items.Add(NFe.Components.TipoEmissao.teSVCAN.ToString());
                            this.edtPadrao.Items.Add(NFe.Components.TipoEmissao.teSVCRS.ToString());
                            break;
                    }
                    filebackup = this.configname + ".xml.bck";

                    xlabels = new List<string>();
                    ///
                    /// varre os nomes das propriedades da classe, eliminando o esquecimento quando da criacao de novas propriedades
                    /// 
                    NFe.Components.URLws temp = new NFe.Components.URLws();
                    foreach (var se in temp.GetType().GetProperties())
                    {
                        if (se.Name.StartsWith("NFe") || 
                            se.Name.StartsWith("CTe") ||
                            se.Name.StartsWith("Cte") ||
                            se.Name.StartsWith("DFe") || 
                            se.Name.StartsWith("MDFe") ||
                            se.Name.StartsWith("LMC") ||
                            se.Name.EndsWith("Cfse") ||
                            se.Name.EndsWith("eSocial") ||
                            se.Name.EndsWith("Reinf"))
                        {
                            if (this._tipo == NFe.Components.TipoAplicativo.Nfe)
                                this.xlabels.Add(se.Name);
                        }
                        else
                            if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                                this.xlabels.Add(se.Name);
                    }

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
                }
                else
                    oIndex = edtEstados.SelectedIndex;

                if (this._tipo == NFe.Components.TipoAplicativo.Nfe)
                {
                    NFe.Components.Propriedade.Estados = null;
                    NFe.Components.Propriedade.NomeArqXMLWebService_NFe = configname;
                }
                XElement axml = XElement.Load(configname);
                var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                         where  (uf == null && p.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value != "XX") ||
                                (uf != null && p.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value == "XX")
                         orderby p.Attribute(NFe.Components.NFeStrConstants.Nome).Value
                         select p);
                foreach (var item in s)
                {
                    string pdr = "", Svc = "";

                    if (item.Attribute(NFe.Components.NFeStrConstants.Padrao) == null)
                        pdr = (this._tipo == NFe.Components.TipoAplicativo.Nfse ? padraobase : "");
                    else
                        pdr = item.Attribute(NFe.Components.NFeStrConstants.Padrao).Value;

                    if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                        Svc = "";
                    else
                        if (item.Attribute(NFe.Components.NFeStrConstants.SVC) == null)
                            Svc = NFe.Components.TipoEmissao.teNone.ToString();
                        else
                            Svc = item.Attribute(NFe.Components.NFeStrConstants.SVC).Value;

                    listaEstados.Add(new Estado
                    {
                        Nome = item.Attribute(NFe.Components.NFeStrConstants.Nome).Value,
                        ID = item.Attribute(NFe.Components.TpcnResources.ID.ToString()).Value,
                        UF = item.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value,
                        svc = Svc,
                        Padrao = pdr
                    });
                    listageral.Add(listaEstados[listaEstados.Count - 1].key);
                }
                edtEstados.DataSource = null;
                edtEstados.Items.Clear();
                edtEstados.DisplayMember = "text";
                edtEstados.ValueMember = NFe.Components.TpcnResources.ID.ToString();
                edtEstados.DataSource = listaEstados;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(null, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                edtEstados.SelectedIndex = oIndex;
                metroComboBox1_SelectedIndexChanged(null, null);
                edtEstados.SelectedIndexChanged += metroComboBox1_SelectedIndexChanged;
                edtPadrao.SelectedIndexChanged += edtPadrao_SelectedIndexChanged;
            }
        }

        void edtPadrao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._tipo == NFe.Components.TipoAplicativo.Nfse && this.uf != "XX")
            {
                XmlDocument docx = new XmlDocument();
                XElement axml = XElement.Load(configname);
                var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                         where  (p.Attribute(NFe.Components.NFeStrConstants.Padrao).Value.Equals(this.edtPadrao.Text)) &&
                                (p.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value == "XX")
                         select p);
                foreach (var item in s)
                {
                    fillEnderecos(docx, item);
                }
            }
        }

        void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            edtPadrao.SelectedIndexChanged -= edtPadrao_SelectedIndexChanged;
            try
            {
                XmlDocument docx = new XmlDocument();

                var item = listaEstados[edtEstados.SelectedIndex];
                edtNome.Text = item.Nome;
                edtUF.Text = item.UF;
                edtID.Text = item.ID;
                if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                    edtPadrao.Text = item.Padrao;
                else
                    edtPadrao.Text = item.svc;

                ///
                /// limpa todos os enderecos
                for (int vs = 0; vs < xlabels.Count; ++vs)
                {
                    (this.tpHomo.Controls["H_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = "";
                    (this.tpProd.Controls["P_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = "";
                }
                ///
                /// seleciona os enderecos do xml
                XElement axml = XElement.Load(configname);
                var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                         where (string)p.Attribute(NFe.Components.TpcnResources.UF.ToString()) == item.UF &&
                                (string)p.Attribute(NFe.Components.TpcnResources.ID.ToString()) == item.ID
                         select p);
                foreach (var itemx in s)
                {
                    if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                    {
                        if (!itemx.Attribute(NFe.Components.NFeStrConstants.Padrao).Value.Equals(item.Padrao))
                            continue;
                    }
                    fillEnderecos(docx, itemx);
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(null, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                edtPadrao.SelectedIndexChanged += edtPadrao_SelectedIndexChanged;
            }
        }

        private void fillEnderecos(XmlDocument docx, XElement itemx)
        {
            docx.LoadXml(itemx.FirstNode.ToString());
            foreach (var node in docx.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalHomologacao))
            {
                for (int vs = 0; vs < xlabels.Count; ++vs)
                {
                    string c = NFe.Components.Functions.LerTag((XmlElement)node, xlabels[vs], false);
                    (this.tpHomo.Controls["H_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = c;
                }
            }

            docx.LoadXml(itemx.LastNode.ToString());
            foreach (var node in docx.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalProducao))
            {
                for (int vs = 0; vs < xlabels.Count; ++vs)
                {
                    string c = NFe.Components.Functions.LerTag((XmlElement)node, xlabels[vs], false);
                    (this.tpProd.Controls["P_" + xlabels[vs]] as UserControl2).metroTextBox1.Text = c;
                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.makebackup();

            try
            {
                this.edtUF.Text = this.edtUF.Text.ToUpper();

                if (string.IsNullOrEmpty(this.edtNome.Text))
                    throw new Exception("Nome deve ser informado");

                if (string.IsNullOrEmpty(this.edtUF.Text))
                    throw new Exception("UF deve ser informada");

                if (this.uf == null)
                {
                    this.edtID.Text = this.edtID.Text.PadLeft(this._tipo == NFe.Components.TipoAplicativo.Nfse ? 7 : 2, '0');

                    if (this.edtUF.Text == "XX")
                        throw new Exception("Não atualize nada nesta UF");

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
                }
                string key = this.edtUF.Text + " - " +
                            this.edtID.Text +
                            (this._tipo == NFe.Components.TipoAplicativo.Nfse ? (" - " + this.edtPadrao.SelectedItem.ToString()) : "");

                Estado item = null;
                bool novo = false;
                try
                {
                    item = listaEstados[edtEstados.SelectedIndex];

                    if (!item.ID.Equals(this.edtID.Text) ||
                        !item.Nome.Equals(this.edtNome.Text) ||
                        !item.Padrao.Equals(this.edtPadrao.Text))
                    {
                        switch (MetroFramework.MetroMessageBox.Show(null, "Informações alteradas\r\n\r\nSim->Altera as informações\r\nNão->Nova informação\r\nCancelar->Abandonar", 
                                        "Aviso", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                        {
                            case DialogResult.Yes:
                                break;
                            case DialogResult.No:
                                novo = true;
                                break;
                            default:
                                return;
                        }
                    }
                }
                catch
                {
                    item = new Estado();
                    novo = true;
                }

                if (item != null)
                {
                    if (!key.Equals(item.key) || novo)  //é diferente da selecionada?
                    {
                        if (listageral.Contains(key))   //a nova chave existe?
                            throw new Exception("Já existe uma configuração com os parâmetros informados");
                    }
                }
                StringBuilder erro = new StringBuilder();
                for (int vs = 0; vs < xlabels.Count; ++vs)
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

                if (this._tipo == NFe.Components.TipoAplicativo.Nfse && this.uf == null)
                    if (!this.edtNome.Text.Trim().EndsWith(this.edtUF.Text))
                        this.edtNome.Text += " - " + this.edtUF.Text;

                XElement[] eleProducao = new XElement[this.xlabels.Count];
                XElement[] eleHomologa = new XElement[this.xlabels.Count];
                for (int vs = 0; vs < xlabels.Count; ++vs)
                {
                    eleProducao[vs] = new XElement(xlabels[vs], (this.tpProd.Controls["P_" + xlabels[vs]] as UserControl2).metroTextBox1.Text);
                    eleHomologa[vs] = new XElement(xlabels[vs], (this.tpHomo.Controls["H_" + xlabels[vs]] as UserControl2).metroTextBox1.Text);
                }

                XElement users = XElement.Load(this.configname);
                IEnumerable<XElement> elements = users.Elements();

                if (item != null && !novo)
                {
                    if (!string.IsNullOrEmpty(item.ID))
                    {
                        elements.AncestorsAndSelf(NFe.Components.NFeStrConstants.Estado).
                            Where(x1 => x1.Attribute(NFe.Components.TpcnResources.ID.ToString()).Value == item.ID &&
                                        x1.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value == item.UF).Remove();
                    }
                }
                XElement ele = new XElement(NFe.Components.NFeStrConstants.Estado,
                    new XAttribute(NFe.Components.TpcnResources.ID.ToString(), this.edtID.Text),
                    new XAttribute(NFe.Components.NFeStrConstants.Nome, this.edtNome.Text),
                    new XAttribute(NFe.Components.TpcnResources.UF.ToString(), this.edtUF.Text),
                    new XAttribute((this._tipo == NFe.Components.TipoAplicativo.Nfse ? NFe.Components.NFeStrConstants.Padrao : NFe.Components.NFeStrConstants.SVC), this.edtPadrao.SelectedItem.ToString()),
                    new XElement(NFe.Components.NFeStrConstants.LocalHomologacao, eleHomologa),
                    new XElement(NFe.Components.NFeStrConstants.LocalProducao, eleProducao)
                );
                users.Add(ele);

                users.Save(this.configname);

                RefreshDados();

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

                    /*if (this._tipo == NFe.Components.TipoAplicativo.Nfse)
                        elements.AncestorsAndSelf(NFe.Components.NFeStrConstants.Estado).
                            Where(x1 => x1.Attribute(NFe.Components.TpcnResources.ID.ToString()).Value == item.ID &&
                                        x1.Attribute(NFe.Components.NFeStrConstants.Padrao).Value == item.Padrao &&
                                        x1.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value == item.UF).Remove();
                    else*/
                        elements.AncestorsAndSelf(NFe.Components.NFeStrConstants.Estado).
                            Where(x1 => x1.Attribute(NFe.Components.TpcnResources.ID.ToString()).Value == item.ID &&
                                        x1.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value == item.UF).Remove();
                    users.Save(this.configname);

                    //dummy.listageral.Remove(item.key);
                    //listaEstados.RemoveAt(edtEstados.SelectedIndex);

                    RefreshDados();
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
            if (System.IO.File.Exists(this.filebackup))
                System.IO.File.Copy(this.filebackup, this.configname, true);
        }

        private void RefreshDados()
        {
            this.loadData(this._tipo, this.uf);
        }
    }
}

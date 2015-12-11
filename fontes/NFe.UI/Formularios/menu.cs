using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MetroFramework.Forms;

using NFe.Components;
using NFe.Settings;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class menu : MetroFramework.Controls.MetroUserControl
    {
        public menu()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ConfiguracaoApp.CarregarDadosSobre();

            metroLink_unimake.Visible = !string.IsNullOrEmpty(NFe.Settings.ConfiguracaoApp.Site);
            metroLink_unimake.Text = NFe.Settings.ConfiguracaoApp.Site;
            uninfeDummy.mainForm.Text = NFe.Components.Propriedade.NomeAplicacao + " - Monitor DF-e";
            metroTile_update.Text = "Atualizar o " + NFe.Components.Propriedade.NomeAplicacao;
            metroTile_sefaz_200.Visible = DateTime.Today <= new DateTime(2015, 4, 1);

            this.metroTile_doc.Enabled =
                System.IO.File.Exists(System.IO.Path.Combine(NFe.Components.Propriedade.PastaExecutavel, NFe.Components.Propriedade.NomeAplicacao + ".pdf"));

            var Components = this.Controls.Cast<object>()
                                                   .Where(obj => !ReferenceEquals(obj, this))
                                                   .OfType<MetroFramework.Controls.MetroTile>();
            foreach (var c in Components)
            {
                c.Style = (MetroFramework.MetroColorStyle)uninfeDummy.xmlParams.ReadValue(this.Name + "\\" + c.Name, "color", Convert.ToInt16(c.Style));
            }

            //string _style = uninfeDummy.uStyle;
            //this.cbStyles.DataSource = estilos;
            //this.cbStyles.DisplayMember = "display_style";
            //this.cbStyles.ValueMember = "internal_style";
            //this.cbStyles.SelectedValue = _style;

            UpdateControles();
        }

        public void UpdateControles()
        {
            //this.Theme = uninfeDummy.mainForm.uTheme;
            //this.Style = uninfeDummy.mainForm.uStyle;

            this.metroTile_Configuracoes.TileCount = NFe.Settings.Empresas.Configuracoes.Count;
            this.metroTile_excluirLock.Enabled =
                this.metroTile_ValidaXml.Enabled =
                    (NFe.Settings.Empresas.Configuracoes != null && NFe.Settings.Empresas.Configuracoes.Count > 0);

            this.metroTile_municipios.Visible = Empresas.CountEmpresasNFse > 0;
            this.metroTile_Servicos.Visible =
                this.metroTile_Danfe.Visible =
                this.metroTile_CadastroContrib.Visible = Empresas.CountEmpresasNFe > 0;
        }

        public void Show(uninfeOpcoes opcao)
        {
            DisposeAllControls();

            switch (opcao)
            {
                case uninfeOpcoes.opCadastro:
                    this.metroTile_CAD_Click(null, null);
                    break;

                case uninfeOpcoes.opConfiguracoes:
                    this.metroTile_CFG_Click(null, null);
                    break;

                case uninfeOpcoes.opServicos:
                    metroTile_STA_Click(null, null);
                    break;

                case uninfeOpcoes.opSobre:
                    this.metroTile_sobre_Click(null, null);
                    break;

                case uninfeOpcoes.opValidarXML:
                    this.metroTile_VAL_Click(null, null);
                    break;

                case uninfeOpcoes.opLogs:
                    this.metroTile_log_Click(null, null);
                    break;

                case uninfeOpcoes.opMunicipios:
                    this.metroTile_municipios_Click(null, null);
                    break;
            }
        }

        private void menu_VisibleChanged(object sender, EventArgs e)
        {
            if (!Propriedade.EncerrarApp)
            {
                UpdateControles();

                DisposeAllControls();
            }
        }

        private void DisposeAllControls()
        {
            var Components = uninfeDummy.mainForm.Controls.Cast<object>()
                                                   .Where(obj => !ReferenceEquals(obj, this))
                                                   .OfType<MetroFramework.Controls.MetroUserControl>();
            foreach (var c in Components)
            {
                c.Dispose();
            }
        }

        private bool createControl(Type user)
        {
            bool _novo = false;

            //NFe.UI.uninfeDummy.showError = false;
            this.VisibleChanged -= menu_VisibleChanged;
            try
            {
                UserControl1 _uc = null;
                ///
                /// processo já existe?
                /// 
                foreach (Control __uc in uninfeDummy.mainForm.Controls)
                {
                    if (__uc.GetType().Equals(user))
                    {
                        _uc = __uc as UserControl1;
                        break;
                    }
                }

                _novo = (_uc == null);
                if (_uc == null)
                {
                    ///
                    /// cria o processo
                    _uc = Activator.CreateInstance(user) as UserControl1;
                }
                _uc.UpdateControles();
                if (_novo)
                {
                    uninfeDummy.mainForm.Controls.Add(_uc);
                }
                var Components = uninfeDummy.mainForm.Controls.Cast<object>()
                                                       .Where(obj => !ReferenceEquals(obj, _uc))
                                                       .OfType<MetroFramework.Controls.MetroUserControl>();
                foreach (Control ctrl in Components)
                {
                    ctrl.Visible = false;
                }
                _uc.Visible = true;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.VisibleChanged += menu_VisibleChanged;
                //NFe.UI.uninfeDummy.showError = true;
            }
            return _novo;
        }

        private void metroTile_CFG_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig) && uninfeDummy.TempoExpirou())
            {
                if (!FormSenha.SolicitaSenha(true))
                    return;

                uninfeDummy.UltimoAcessoConfiguracao = DateTime.Now;
            }
            createControl(typeof(userConfiguracoes));
        }

        private void metroTile_log_Click(object sender, EventArgs e)
        {
            createControl(typeof(userLogs));
        }

        private void metroTile_sobre_Click(object sender, EventArgs e)
        {
            createControl(typeof(userSobre));
        }

        private void metroTile_VAL_Click(object sender, EventArgs e)
        {
            createControl(typeof(userValidaXML));
        }

        private void metroTile_danfe_Click(object sender, EventArgs e)
        {
            uninfeDummy.printDanfe();
        }

        private void metroTile_excluir_Click(object sender, EventArgs e)
        {
            if (Empresas.Configuracoes == null || Empresas.Configuracoes.Count == 0) return;
            if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm,
                                "Exclui os arquivos de \".lock\" configurados para esta instância?\r\n" +
                                "Tem certeza que deseja continuar? ", "",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Empresas.CreateLockFile(true);

                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "Arquivos de \".lock\" excluídos com sucesso.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void metroTile_wsdl_Click(object sender, EventArgs e)
        {
            string msg = string.Format("Após confirmada esta função o {0} irá sobrepor todos os WSDLs e Schemas com as versões originais da versão do {0}" +
                    ", sobrepondo assim possíveis arquivos que tenham sido atualizados manualmente.\r\n\r\nTem certeza que deseja continuar?",
                                Propriedade.NomeAplicacao);

            if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Functions.DeletarArquivo(ConfiguracaoApp.XMLVersoesWSDL);

                    string cerros = "";
                    ConfiguracaoApp.ForceUpdateWSDL(false, ref cerros);

                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, "WSDLs e Schemas atualizados com sucesso.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void metroTile_update_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig) && uninfeDummy.TempoExpirou())
            {
                if (!FormSenha.SolicitaSenha(true))
                    return;

                uninfeDummy.UltimoAcessoConfiguracao = DateTime.Now;
            }

            Formularios.FormUpdate FormUp = new Formularios.FormUpdate();
            FormUp.ShowDialog();
            FormUp.Dispose();
        }

        private void metroTile_STA_Click(object sender, EventArgs e)
        {
            createControl(typeof(userPedidoSituacao));
        }

        private void metroTile_CAD_Click(object sender, EventArgs e)
        {
            createControl(typeof(userCadastro));
        }

        private void metroTile_municipios_Click(object sender, EventArgs e)
        {
            createControl(typeof(Formularios.userMunicipios));
        }

        private void metroLink3_Click(object sender, EventArgs e)
        {
            this.StartPage("http://" + NFe.Settings.ConfiguracaoApp.Site);
        }

        private void metroTile_sefaz_Click(object sender, EventArgs e)
        {
            this.StartPage("http://www.nfe.fazenda.gov.br/portal/disponibilidade.aspx?versao=2.00");
        }

        private void metroTile_sefaz_310_Click(object sender, EventArgs e)
        {
            this.StartPage("http://www.nfe.fazenda.gov.br/portal/disponibilidade.aspx?versao=3.100");
        }

        /*
        private void metroTile_visual_Click(object sender, EventArgs e)
        {
            using (NFe.UI.Formularios.FormVisual v = new Formularios.FormVisual())
            {
                v.ShowDialog();
            }
        }
        */

        private void metroTile_doc_Click(object sender, EventArgs e)
        {
            try
            {
                NFe.Components.Functions.ExibeDocumentacao();
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
            }
        }

        void StartPage(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.CurrentTile.Style = (MetroFramework.MetroColorStyle)Convert.ToInt16(e.ClickedItem.Tag);
            this.CurrentTile.Refresh();

            uninfeDummy.xmlParams.WriteValue(this.Name + "\\" + this.CurrentTile.Name, "color", (int)this.CurrentTile.Style);

            this.CurrentTile = null;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (this.CurrentTile != null)
            {
                foreach (var item in this.contextMenuStrip1.Items)
                {
                    try
                    {
                        if (item.ToString().Equals("Restaurar Padrão"))
                            continue;

                        ((ToolStripMenuItem)item).Checked = Convert.ToInt16(((ToolStripMenuItem)item).Tag) == (int)this.CurrentTile.Style;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private MetroFramework.Controls.MetroTile CurrentTile = null;

        private void metroTile_Configuracoes_MouseDown(object sender, MouseEventArgs e)
        {
            this.CurrentTile = (MetroFramework.Controls.MetroTile)sender;
        }
    }
}

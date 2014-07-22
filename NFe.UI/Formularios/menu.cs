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
/*
        private List<_styles> estilos;

        internal class _styles
        {
            public string internal_style { get; set; }
            public string display_style { get; set; }
        }
        */

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            /*
            estilos = new List<_styles>(){
                new _styles{ display_style="Amarelo", internal_style="Yellow" },
                new _styles{ display_style="Azul(1)", internal_style="Blue" },
                new _styles{ display_style="Azul(2)", internal_style="Teal" },
                new _styles{ display_style="Branco", internal_style="White" },
                new _styles{ display_style="Laranja", internal_style="Orange" },
                new _styles{ display_style="Limão", internal_style="Lime" },
                new _styles{ display_style="Magenta", internal_style="Magenta" },
                new _styles{ display_style="Marrom", internal_style="Brown" },
                new _styles{ display_style="Prata", internal_style="Silver" },
                new _styles{ display_style="Preto", internal_style="Black" },
                new _styles{ display_style="Rosa", internal_style="Pink" },
                new _styles{ display_style="Roxo", internal_style="Purple" },
                new _styles{ display_style="Verde", internal_style="Green" },
                new _styles{ display_style="Vermelho", internal_style="Red" }
            };
            */

            metroLink_unimake.Visible = !string.IsNullOrEmpty(NFe.Settings.ConfiguracaoApp.Site);
            metroLink_unimake.Text = NFe.Settings.ConfiguracaoApp.Site;
            uninfeDummy.mainForm.Text = NFe.Components.Propriedade.NomeAplicacao + "  (" + NFe.Components.Propriedade.Versao + ")";
            metroTile_sobre.Text = "Sobre o " + NFe.Components.Propriedade.NomeAplicacao;
            metroTile_update.Text = "Atualizar o " + NFe.Components.Propriedade.NomeAplicacao;

            this.metroTile_doc.Enabled =
                System.IO.File.Exists(System.IO.Path.Combine(NFe.Components.Propriedade.PastaExecutavel, NFe.Components.Propriedade.NomeAplicacao + ".pdf"));

            switch (NFe.Components.Propriedade.TipoAplicativo)
            {
                case NFe.Components.TipoAplicativo.Nfe:
                    this.metroTile_municipios.Visible = false;
                    break;

                case NFe.Components.TipoAplicativo.Nfse:
                    metroTile_sobre.TileImage = NFe.UI.Properties.Resources.uninfse;
                    this.metroTile_Danfe.Visible =
                        this.metroTile_CadastroContrib.Visible =
                        this.metroTile_Servicos.Visible =
                        //this.metroTile_doc.Visible = 
                        this.metroTile_sefaz_200.Visible = 
                        this.metroTile_sefaz_310.Visible = false;
                    metroTile_municipios.Location = new Point(metroTile_Danfe.Location.X, metroTile_Danfe.Location.Y);
                    metroTile_municipios.Size = new Size(metroTile_Danfe.Size.Width, metroTile_Danfe.Size.Height);
                    break;
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
            this.Theme = uninfeDummy.mainForm.uTheme;
            this.Style = uninfeDummy.mainForm.uStyle;

            this.metroTile_Configuracoes.TileCount = NFe.Settings.Empresas.Configuracoes.Count;
            this.metroTile_CadastroContrib.Enabled =
                this.metroTile_Servicos.Enabled =
                this.metroTile_ValidaXml.Enabled =
                this.metroTile_excluirLock.Enabled =
                this.metroTile_Danfe.Enabled = (NFe.Settings.Empresas.Configuracoes != null && NFe.Settings.Empresas.Configuracoes.Count > 0);
        }

        public void Show(uninfeOpcoes opcao)
        {
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
            UpdateControles();

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
                foreach(Control __uc in uninfeDummy.mainForm.Controls)
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
                    //Dialogs.ShowMessage("Arquivos de \".lock\" excluídos com sucesso.", 0, 0, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Dialogs.ShowMessage(ex.Message, 500, 0, MessageBoxIcon.Error, ScrollBars.Both);
                }
            }
        }

        private void metroTile_wsdl_Click(object sender, EventArgs e)
        {
            string XMLVersoesWSDL = Propriedade.PastaExecutavel + "\\VersoesWSDLs.xml";
            string msg = string.Format("Após confirmada esta função o {0} irá sobrepor todos os WSDLs e Schemas com as versões originais da versão do {0}" +
                    ", sobrepondo assim possíveis arquivos que tenham sido atualizados manualmente.\r\n\r\nTem certeza que deseja continuar?",
                                Propriedade.NomeAplicacao);

            if (MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            if (Dialogs.YesNo("ATENÇÂO! - Atualização dos WSDLs e SCHEMAS\r\n\r\n" + msg, 500, 250))
            {
                try
                {
                    if (File.Exists(XMLVersoesWSDL))
                    {
                        File.Delete(XMLVersoesWSDL);
                    }
                    ConfiguracaoApp.ForceUpdateWSDL(false);

                    throw new Exception("WSDLs e Schemas atualizados com sucesso.");
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                    //Dialogs.ShowMessage(ex.Message, 500, 0, MessageBoxIcon.Error, ScrollBars.Both);
                }
            }
        }

        private void metroTile_update_Click(object sender, EventArgs e)
        {
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
            System.Diagnostics.Process.Start("http://" + NFe.Settings.ConfiguracaoApp.Site);
        }

        private void metroTile_sefaz_Click(object sender, EventArgs e)
        {
            const string address_disp = "http://www.nfe.fazenda.gov.br/portal/disponibilidade.aspx?versao=2.00";
            System.Diagnostics.Process.Start(address_disp);
        }

        private void metroTile_sefaz_310_Click(object sender, EventArgs e)
        {
            const string address_disp = "http://www.nfe.fazenda.gov.br/portal/disponibilidade.aspx?versao=3.100";
            System.Diagnostics.Process.Start(address_disp);
        }

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
    }
}

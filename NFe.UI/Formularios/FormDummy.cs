using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class FormDummy : MetroFramework.Forms.MetroForm
    {
        public FormDummy()
        {
            InitializeComponent();
        }

        public uninfeOpcoes opcao { get; set; }

        public static void ShowModulo(uninfeOpcoes opcao)
        {
            using (FormDummy f = new FormDummy())
            {
                f.opcao = opcao;
                if (f.LoadProcesso())
                    f.ShowDialog();
                else
                    f.Tag = 1;  //ops nao precisa salvar a posicao da tela
            }
        }

        bool LoadProcesso()
        {
            //NFe.UI.uninfeDummy.showError = false;
            try
            {
                UserControl1 control = null;

                switch (opcao)
                {
                    case uninfeOpcoes.opCadastro:
                        this.Controls.Add(control = new userCadastro());
                        break;

                    case uninfeOpcoes.opConfiguracoes:
                        this.Controls.Add(control = new userConfiguracoes());
                        break;

                    case uninfeOpcoes.opLogs:
                        this.Controls.Add(control = new userLogs());
                        break;

                    case uninfeOpcoes.opServicos:
                        this.Controls.Add(control = new userPedidoSituacao());
                        break;

                    case uninfeOpcoes.opSobre:
                        this.Size = new Size(800, 600);
                        this.Controls.Add(control = new userSobre());
                        break;

                    case uninfeOpcoes.opValidarXML:
                        this.Controls.Add(control = new userValidaXML());
                        break;

                    case uninfeOpcoes.opMunicipios:
                        this.Controls.Add(control = new Formularios.userMunicipios());
                        break;
                }
                if (control != null)
                {
                    control.Dock = DockStyle.Fill;
                    control.back_button.Click += delegate { this.Close(); };
                    control.UpdateControles();
                    this.Invalidate();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Text = NFe.Components.Propriedade.NomeAplicacao + "  (" + NFe.Components.Propriedade.Versao + ")";
            this.MinimumSize = new Size(800, 600);

            this.Theme = uninfeDummy.mainForm.uTheme;
            this.Style = uninfeDummy.mainForm.uStyle;
            this.StyleManager = uninfeDummy.mainForm.StyleManager;

            uninfeDummy.xmlParams.LoadForm(this, "\\" + opcao.ToString());

            switch (NFe.Components.Propriedade.TipoAplicativo)
            {
                case NFe.Components.TipoAplicativo.Nfe:
                    this.Icon = NFe.UI.Properties.Resources.uninfe_icon;
                    break;

                case NFe.Components.TipoAplicativo.Nfse:
                    this.Icon = NFe.UI.Properties.Resources.uninfse_icon;
                    break;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (this.Tag == null)
            {
                if (opcao == uninfeOpcoes.opConfiguracoes)
                {
                    uninfeDummy.mainForm.RestartServicos();
                }
                uninfeDummy.xmlParams.SaveForm(this, "\\" + opcao.ToString());
                uninfeDummy.xmlParams.Save();
            }
            base.OnFormClosed(e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class userCadastroResultDados : MetroFramework.Controls.MetroUserControl
    {
        public userCadastroResultDados()
        {
            InitializeComponent();
        }

        public void Populate(NFe.Service.infCad infCad)
        {
            uninfeDummy.ClearControls(this, true, false);
            if (!string.IsNullOrEmpty(infCad.CNPJ))
            {
                this.label1.Text = NFe.Components.TpcnResources.CNPJ.ToString();
                this.txtCNPJ_CPF.Text = uninfeDummy.FmtCnpjCpf(infCad.CNPJ, true);
            }
            else
            {
                this.label1.Text = "CPF";
                this.txtCNPJ_CPF.Text = uninfeDummy.FmtCnpjCpf(infCad.CPF, false);
            }
            this.txtxIE.Text = infCad.IE;
            this.txtxNome.Text = infCad.xNome;
            this.txtxFant.Text = infCad.xFant;
            this.txtxEnder.Text = infCad.ender.xLgr;
            this.txtxBairro.Text = infCad.ender.xBairro;
            this.txtnro.Text = infCad.ender.nro;
            this.txtxCpl.Text = infCad.ender.xCpl;
            this.txtUF.Text = infCad.UF;
            this.txtxMun.Text = infCad.ender.xMun;
            this.txtCEP.Text = infCad.ender.CEP.ToString("00000000");
            this.txtdBaixa.Text = infCad.dBaixa.ToShortDateString();
            this.txtdUltSit.Text = infCad.dUltSit.ToShortDateString();
            this.txtdIniAtiv.Text = infCad.dIniAtiv.ToShortDateString();
            this.txtxRegApur.Text = infCad.xRegApur;
            this.txtCNAE.Text = infCad.CNAE.ToString();
            this.txtIEAtual.Text = infCad.IEAtual;
            this.txtIEUnica.Text = infCad.IEUnica;
            this.txtcSit.Text = infCad.cSit;
        }

    }
}

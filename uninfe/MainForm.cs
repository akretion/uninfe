using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace uninfe
{
    public partial class MainForm : Form
    {
        UniNfeClass oNfe = new UniNfeClass();

        public MainForm()
        {
            InitializeComponent();
            CarregarConfiguracoes();

            //Trazer minimizado e no systray
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notifyIcon1.ShowBalloonTip(6000);
        }

        private void toolStripButton_config_Click(object sender, EventArgs e)
        {
            FormConfiguracao oConfig = new FormConfiguracao();
            oConfig.MdiParent = this;
            oConfig.Show();
        }

        private void toolStripButton_teste_Click(object sender, EventArgs e)
        {
            Form1 oFormTeste = new Form1();
            oFormTeste.MdiParent = this;
            oFormTeste.Show();
        }

        private void ReconfigurarUniNFe(string cArquivo)
        {
            ConfigUniNFe oConfig = new ConfigUniNFe();
            oConfig.ReconfigurarUniNFe(cArquivo);

            CarregarConfiguracoes();
        }

        private void CarregarConfiguracoes()
        {
            ConfigUniNFe oConfig = new ConfigUniNFe();
            oConfig.CarregarDados();
            oNfe.oCertificado = oConfig.oCertificado;
            oNfe.vUF = oConfig.vUnidadeFederativaCodigo;
            oNfe.vAmbiente = oConfig.vAmbienteCodigo;
            oNfe.vPastaXMLEnvio = oConfig.vPastaXMLEnvio;
            oNfe.vPastaXMLRetorno = oConfig.vPastaXMLRetorno;
            oNfe.vPastaXMLEnviado = oConfig.vPastaXMLEnviado;
            oNfe.vPastaXMLErro = oConfig.vPastaXMLErro;
            oNfe.vTpEmis = oConfig.vTpEmis;

            //Ativar o timer
            this.timer_connect_webservice.Enabled = true; 
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.timer_connect_webservice.Enabled = false;

            MessageBox.Show(oNfe.VerStatusServico(), "Situação do serviço da NFe é:", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.timer_connect_webservice.Enabled = true;
        }

        private void timer_connect_webservice_Tick(object sender, EventArgs e)
        {
            this.timer_connect_webservice.Enabled = false; //Desativar o timer

            //a linha comentada abaixo retorna a localização do executável. Wandrey 11/11/2008
            //string pathFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //a linha comentada abaixo acredito eu que muda o path, diretório, tem que testar. Wandrey 11/11/2008
            //System.IO.Directory.SetCurrentDirectory()

            //Procurar arquivos XML para ser enviados aos web-services
            if (oNfe.vPastaXMLEnvio != string.Empty)
            {
                string[] aArquivos = Directory.GetFiles(oNfe.vPastaXMLEnvio,"*.xml");

                for (int i = 0; i < aArquivos.Length; i++)
                {
                    try
                    {
                        using (FileStream fs = File.Open(aArquivos[i], FileMode.Open, FileAccess.ReadWrite, FileShare.Write))
                        {
                            //Conseguiu abrir o arquivo, significa que está perfeitamente gerado
                            //assim vou iniciar o processo de envio do XML
                            fs.Close();
                            this.EnviarArquivo(aArquivos[i]);
                        }
                    }
                    catch (IOException)
                    {       
                        //Não conseguiu abrir o arquivo e vai aguardar, pois pode ser que ainda esteja sendo gerado.
                    }
                }
            }

            this.timer_connect_webservice.Enabled = true; //Ativar o timer
        }

        private void EnviarArquivo( string cArquivo )
        {
            //-nfe.xml = Arquivo XML de Nota Fiscal Eletronica a ser assinada e incluida em um lote de notas
            bool lNfe = (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 4, 4).ToLower() == "-nfe");
            //-ped-rec.xml = Arquivo XML de Pedido do Resultado do Processamento do Lote de NF-e
            bool lPedRec = (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-rec");
            //-ped-sit.xml = Arquivo XML de Pedido da Consulta Atual da NF-e
            bool lPedSit = (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-sit");
            //-ped-sta.xml = Arquivo XML de Pedido de Consulta do Status do Serviço
            bool lPedSta = (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-sta");
            //-ped-can.xml = Arquivo XML de Cancelamento de Notas Fiscais
            bool lPedCan = (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-can");
            //-ped-inu.xml = Arquivo XML de Inutilização de Numeração de Notas Fiscais
            bool lPedInu = (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-inu");
            //-alt-con.xml = Arquivo XML de Configuração Automática do UniNFe
            bool lAltCon = (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-alt-con");

            //Invocar o serviço solicitado
            oNfe.vXmlNfeDadosMsg = cArquivo;
            if (oNfe.vTpEmis != 2) //2-Confingência em Formulário decimal segurança não envia na hora, tem que aguardar voltar para normal.
            {
                if (lNfe == true)
                {
                    oNfe.Recepcao();
                }
                else if (lPedRec == true)
                {
                    oNfe.RetRecepcao();
                }
                else if (lPedSit == true)
                {
                    oNfe.Consulta();
                }
                else if (lPedSta == true)
                {
                    oNfe.StatusServico();
                }
                else if (lPedCan == true)
                {
                    oNfe.Cancelamento();
                }
                else if (lPedInu == true)
                {
                    oNfe.Inutilizacao();
                }
                if (lAltCon == true)
                {
                    this.ReconfigurarUniNFe(cArquivo);
                }
            }
            else
            {
                if (lAltCon == true)
                {
                    this.ReconfigurarUniNFe(cArquivo);
                }
                else if (lPedRec == true)
                {
                    oNfe.RetRecepcao();
                }
                else if (lPedSit == true)
                {
                    oNfe.Consulta();
                }
                else if (lPedSta == true)
                {
                    oNfe.StatusServico();
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Faz a aplicação sumir da barra de tarefas
            this.ShowInTaskbar = false;

            //Mostrar o balão com as informações que selecionamos
            //O parâmetro passado refere-se ao tempo (ms)
            // em que ficará aparecendo. Coloque "0" se quiser
            // que ele feche somente quando o usuário clicar

            notifyIcon1.ShowBalloonTip(6000);

            //Ativar o ícone na área de notificação
            //para isso a propriedade Visible deveria ser setada
            //como false, mas prefiro deixar o ícone lá.
            //notifyIcon1.Visible = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Voltar a janela em seu estado normal
            this.WindowState = FormWindowState.Normal;

            // Faz a aplicação aparecer na barra de tarefas.            
            this.ShowInTaskbar = true;

            // Levando o Form de volta para a tela.

            this.WindowState = FormWindowState.Normal;
            this.Visible = true;

            // Faz desaparecer o ícone na área de notificação,
            // para isso a propriedade Visible deveria ser setada 
            // como true no evento Resize do Form.

            // notifyIcon1.Visible=false;
        }

        private void toolStripButton_recarregar_config_Click(object sender, EventArgs e)
        {
            CarregarConfiguracoes();
            MessageBox.Show("Configurações recarregadas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void toolStripButton_sobre_Click(object sender, EventArgs e)
        {
            FormSobre oSobre = new FormSobre();
            oSobre.MdiParent = this;
            oSobre.Show();       
        }

        private void toolStripButton_validarxml_Click(object sender, EventArgs e)
        {
            FormValidarXML oValidarXML = new FormValidarXML();
            oValidarXML.MdiParent = this;
            oValidarXML.Show();
        }
    }
}

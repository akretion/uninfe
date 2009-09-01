using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UniNFeLibrary;
using UniNFeLibrary.Formulario;
using UniNFeLibrary.Enums;
using uninfe.Formulario;

namespace uninfe
{
    #region Classe MainForm
    public partial class MainForm : Form
    {
        #region Definir os objetos dos serviços executados pelo UniNFe
        ServicoUniNFe oServicoAssinaNfeEnvio = new ServicoUniNFe();
        ServicoUniNFe oServicoAssinaNfeEnvioEmLote = new ServicoUniNFe();
        ServicoUniNFe oServicoMontaLoteUmaNfe = new ServicoUniNFe();
        ServicoUniNFe oServicoMontaLoteVariasNfe = new ServicoUniNFe();
        ServicoUniNFe oServicoEnviarLoteNfe = new ServicoUniNFe();
        ServicoUniNFe oServicoPedCan = new ServicoUniNFe();
        ServicoUniNFe oServicoPedInu = new ServicoUniNFe();
        ServicoUniNFe oServicoPedRec = new ServicoUniNFe();
        ServicoUniNFe oServicoPedSit = new ServicoUniNFe();
        ServicoUniNFe oServicoPedSta = new ServicoUniNFe();
        ServicoUniNFe oServicoConsCad = new ServicoUniNFe();
        ServicoUniNFe oServicoConsInf = new ServicoUniNFe();
        ServicoUniNFe oServicoAltCon = new ServicoUniNFe();
        ServicoUniNFe oServicoValidarAssinar = new ServicoUniNFe();
        ServicoUniNFe oServicoConvTXT = new ServicoUniNFe();
        #endregion

        #region Definir os objetos de threads para executar os serviços do UniNFe
        ParameterizedThreadStart oOperacaoAssinaNfeEnvio;
        ParameterizedThreadStart oOperacaoAssinaNfeEnvioEmLote;
        ParameterizedThreadStart oOperacaoMontaLoteUmaNfe;
        ParameterizedThreadStart oOperacaoMontaLoteVariasNfe;
        ParameterizedThreadStart oOperacaoEnviarLoteNfe;
        ParameterizedThreadStart oOperacaoPedCan;
        ParameterizedThreadStart oOperacaoPedInu;
        ParameterizedThreadStart oOperacaoPedRec;
        ParameterizedThreadStart oOperacaoPedSit;
        ParameterizedThreadStart oOperacaoPedSta;
        ParameterizedThreadStart oOperacaoConsCad;
        ParameterizedThreadStart oOperacaoConsInf;
        ParameterizedThreadStart oOperacaoAltCon;
        ParameterizedThreadStart oOperacaoValidarAssinar;
        ParameterizedThreadStart oOperacaoConvTXT;

        Thread oThreadAssinaNfeEnvio;
        Thread oThreadAssinaNfeEnvioEmLote;
        Thread oThreadMontaLoteUmaNfe;
        Thread oThreadMontaLoteVariasNfe;
        Thread oThreadEnviarLoteNfe;
        Thread oThreadPedCan;
        Thread oThreadPedInu;
        Thread oThreadPedRec;
        Thread oThreadPedSit;
        Thread oThreadPedSta;
        Thread oThreadConsCad;
        Thread oThreadConsInf;
        Thread oThreadAltCon;
        Thread oThreadValidarAssinar;
        Thread oThreadConvTXT;
        #endregion

        #region Atributos
        private bool booPodeFechar = false;
        #endregion

        #region MainForm()
        public MainForm()
        {
            InitializeComponent();
            AtualizarDadosToolBar();

            //Criar XML de controle de fluxo de envios de Notas Fiscais
            FluxoNfe oFluxoNfe = new FluxoNfe();
            oFluxoNfe.CriarXml();           

            //Trazer minimizado e no systray
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notifyIcon1.ShowBalloonTip(6000);

            this.MinimumSize = new Size(750,600);   

            #region Executar os serviços em novas threads
            //Carregar as configurações antes de executar os serviços do UNINFE
            ConfiguracaoApp.CarregarDados();
            ConfiguracaoApp.VersaoXMLCanc = "1.07";
            ConfiguracaoApp.VersaoXMLConsCad = "1.01";
            ConfiguracaoApp.VersaoXMLInut = "1.07";
            ConfiguracaoApp.VersaoXMLNFe = "1.10";
            ConfiguracaoApp.VersaoXMLPedRec = "1.10";
            ConfiguracaoApp.VersaoXMLPedSit = "1.07";
            ConfiguracaoApp.VersaoXMLStatusServico = "1.07";
            ConfiguracaoApp.VersaoXMLCabecMsg = "1.02";
            ConfiguracaoApp.nsURI = "http://www.portalfiscal.inf.br/nfe";
            SchemaXML.CriarListaIDXML();

            //Executar os serviços do UniNFe em novas threads
            this.ExecutaServicos();
            #endregion
        }
        #endregion

        #region Métodos gerais

        #region AtualizarDadosToolBar()
        /// <summary>
        /// Atualiza o nome da empresa na ToolBar do UniNFe
        /// </summary>
        private void AtualizarDadosToolBar()
        {
            //Atualizar o nome da empresa na toolstrip
            //Wandrey 28/11/2008
            this.toolStripLabel_NomeEmpresa.Text = ConfiguracaoApp.cNomeEmpresa;
            if (ConfiguracaoApp.cNomeEmpresa.Trim() != "")
            {
                ///
                /// danasa 8-2009
                /// notifyIcon1.Text tem o limite de 64 bytes
                /// 
                string cTemp = "UniNFe\r\n" + ConfiguracaoApp.cNomeEmpresa;
                this.notifyIcon1.Text = cTemp.Substring(0, Math.Min(63, cTemp.Length));
            }
        }
        #endregion

        #region ExecutaServicos()
        /// <summary>
        /// Metodo responsável por iniciar os serviços do UniNFe em threads diferentes
        /// </summary>
        private void ExecutaServicos()
        {
            #region Executar a Thread que verifica e processa os XML´s de cancelamentos de notas fiscais
            oOperacaoPedCan = new ParameterizedThreadStart(oServicoPedCan.BuscaXML); 
            oThreadPedCan = new Thread(oOperacaoPedCan);
            oThreadPedCan.Name = Servicos.CancelarNFe.ToString();
            oThreadPedCan.Start(Servicos.CancelarNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s de Inutilização de números de notas fiscais
            oOperacaoPedInu = new ParameterizedThreadStart(oServicoPedInu.BuscaXML);
            oThreadPedInu = new Thread(oOperacaoPedInu); 
            oThreadPedInu.Name = Servicos.InutilizarNumerosNFe.ToString();
            oThreadPedInu.Start(Servicos.InutilizarNumerosNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido da Situação da Nota Fiscal Eletrônica
            oOperacaoPedSit = new ParameterizedThreadStart(oServicoPedSit.BuscaXML);
            oThreadPedSit = new Thread(oOperacaoPedSit); 
            oThreadPedSit.Name = Servicos.PedidoConsultaSituacaoNFe.ToString();
            oThreadPedSit.Start(Servicos.PedidoConsultaSituacaoNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido do Status do Serviço da Nota Fiscal Eletrônica
            oOperacaoPedSta = new ParameterizedThreadStart(oServicoPedSta.BuscaXML);
            oThreadPedSta = new Thread(oOperacaoPedSta); 
            oThreadPedSta.Name = Servicos.PedidoConsultaStatusServicoNFe.ToString();
            oThreadPedSta.Start(Servicos.PedidoConsultaStatusServicoNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido da Situação do Recibo do Lote de Notas Fiscais Eletrônica
            oOperacaoPedRec = new ParameterizedThreadStart(oServicoPedRec.BuscaXML);
            oThreadPedRec = new Thread(oOperacaoPedRec);
            oThreadPedRec.Name = Servicos.PedidoSituacaoLoteNFe.ToString();
            oThreadPedRec.Start(Servicos.PedidoSituacaoLoteNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido de Consulta do Cadastro de Contribuintes
            oOperacaoConsCad = new ParameterizedThreadStart(oServicoConsCad.BuscaXML);
            oThreadConsCad = new Thread(oOperacaoConsCad);
            oThreadConsCad.Name = Servicos.ConsultaCadastroContribuinte.ToString();
            oThreadConsCad.Start(Servicos.ConsultaCadastroContribuinte);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido de Consulta das Informações do UniNfe
            oOperacaoConsInf = new ParameterizedThreadStart(oServicoConsInf.BuscaXML);
            oThreadConsInf = new Thread(oOperacaoConsInf);
            oThreadConsInf.Name = Servicos.ConsultaInformacoesUniNFe.ToString();
            oThreadConsInf.Start(Servicos.ConsultaInformacoesUniNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido de Alteração das Configurações do UniNfe
            oOperacaoAltCon = new ParameterizedThreadStart(oServicoAltCon.BuscaXML);
            oThreadAltCon = new Thread(oOperacaoAltCon);
            oThreadAltCon.Name = Servicos.AlterarConfiguracoesUniNFe.ToString();
            oThreadAltCon.Start(Servicos.AlterarConfiguracoesUniNFe);
            #endregion

            #region Executar a Thread que verifica e assina os XML´s de notas fiscais eletrônicas da pasta Envio
            oOperacaoAssinaNfeEnvio = new ParameterizedThreadStart(oServicoAssinaNfeEnvio.BuscaXML);
            oThreadAssinaNfeEnvio = new Thread(oOperacaoAssinaNfeEnvio);
            oThreadAssinaNfeEnvio.Name = Servicos.AssinarNFePastaEnvio.ToString();
            oThreadAssinaNfeEnvio.Start(Servicos.AssinarNFePastaEnvio);
            #endregion  
 
            #region Executar a Thread que verifica e monta o lote com uma única nota fiscal eletrônica
            oOperacaoMontaLoteUmaNfe = new ParameterizedThreadStart(oServicoMontaLoteUmaNfe.BuscaXML);
            oThreadMontaLoteUmaNfe = new Thread(oOperacaoMontaLoteUmaNfe);
            oThreadMontaLoteUmaNfe.Name = Servicos.MontarLoteUmaNFe.ToString();
            oThreadMontaLoteUmaNfe.Start(Servicos.MontarLoteUmaNFe);
            #endregion

            #region Executar a Thread que envia os lotes de notas fiscais eletrônicas
            oOperacaoEnviarLoteNfe = new ParameterizedThreadStart(oServicoEnviarLoteNfe.BuscaXML);  
            oThreadEnviarLoteNfe = new Thread(oOperacaoEnviarLoteNfe);
            oThreadEnviarLoteNfe.Name = Servicos.EnviarLoteNfe.ToString();
            oThreadEnviarLoteNfe.Start(Servicos.EnviarLoteNfe);
            #endregion
  
            #region Executar a Thread que somente Valida e Assina XML´s e dá o retorno para o ERP
            oOperacaoValidarAssinar = new ParameterizedThreadStart(oServicoValidarAssinar.BuscaXML);
            oThreadValidarAssinar = new Thread(oOperacaoValidarAssinar);
            oThreadValidarAssinar.Name = Servicos.ValidarAssinar.ToString();
            oThreadValidarAssinar.Start(Servicos.ValidarAssinar);
            #endregion

            #region Executar a Thread que somente Converte o TXT da NFe para XML
            oOperacaoConvTXT = new ParameterizedThreadStart(oServicoConvTXT.BuscaXML);
            oThreadConvTXT = new Thread(oOperacaoConvTXT);
            oThreadConvTXT.Name = Servicos.ConverterTXTparaXML.ToString();
            oThreadConvTXT.Start(Servicos.ConverterTXTparaXML);
            #endregion

  
            #region Executar a Thread que verifica e assina os XML´s de notas fiscais eletrônicas da pasta de Envio em Lote
            oOperacaoAssinaNfeEnvioEmLote = new ParameterizedThreadStart(oServicoAssinaNfeEnvioEmLote.BuscaXML);
            oThreadAssinaNfeEnvioEmLote = new Thread(oOperacaoAssinaNfeEnvioEmLote);
            oThreadAssinaNfeEnvioEmLote.Name = Servicos.AssinarNFePastaEnvioEmLote.ToString();
            oThreadAssinaNfeEnvioEmLote.Start(Servicos.AssinarNFePastaEnvioEmLote);
            #endregion
  
            #region Executar a Thread que verifica e monta o lote com várias notas fiscais eletrônicas
            oOperacaoMontaLoteVariasNfe = new ParameterizedThreadStart(oServicoMontaLoteVariasNfe.BuscaXML);
            oThreadMontaLoteVariasNfe = new Thread(oOperacaoMontaLoteVariasNfe);
            oThreadMontaLoteVariasNfe.Name = Servicos.MontarLoteVariasNFe.ToString();
            oThreadMontaLoteVariasNfe.Start(Servicos.MontarLoteVariasNFe);
            #endregion               
        }
        #endregion

        #region DemonstrarStatusServico()
        private void DemonstrarStatusServico()
        {
            //Carregar configurações
            ServicoNFe oNfe = new ServicoNFe();

            //Demonstrar o status do serviço
            MessageBox.Show(oNfe.VerStatusServico(), "Situação do serviço da NFe é:", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #endregion

        #region Métodos de eventos
        private void toolStripButton_config_Click(object sender, EventArgs e)
        {
            //danasa 
            Configuracao oConfig = null;
            foreach (Form fg in this.MdiChildren)
            {
                if (fg is Configuracao)
                {
                    oConfig = fg as Configuracao;
                    oConfig.WindowState = FormWindowState.Normal;
                    break;
                }
            }
            //danasa 
            if (oConfig == null)
                oConfig = new Configuracao();
            oConfig.MdiParent = this;
            oConfig.MinimizeBox = false;
            oConfig.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var consultaCadastro = new FormConsultaCadastro();
            consultaCadastro.MdiParent = this;
            consultaCadastro.MinimizeBox = false;
            consultaCadastro.Show();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Faz a aplicação sumir da barra de tarefas
            //danasa
            //  Se usuario mudar o tamanho da janela, não pode desaparece-la da tasknar
            if (this.WindowState == FormWindowState.Minimized)
                this.ShowInTaskbar = false;   

            //Mostrar o balão com as informações que selecionamos
            //O parâmetro passado refere-se ao tempo (ms)
            // em que ficará aparecendo. Coloque "0" se quiser
            // que ele feche somente quando o usuário clicar

            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.ShowBalloonTip(6000);
            }

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

        private void toolStripButton_sobre_Click(object sender, EventArgs e)
        {
            FormSobre oSobre = new FormSobre();
            oSobre.MinimizeBox = false;
            oSobre.ShowInTaskbar = false;            
            oSobre.ShowDialog();
        }

        private void toolStripButton_validarxml_Click(object sender, EventArgs e)
        {
            ValidarXML oValidarXML = new ValidarXML();
            oValidarXML.MdiParent = this;
            oValidarXML.MinimizeBox = false;
            oValidarXML.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Abortar todas as thread´s em execução
            oThreadAltCon.Abort();
            oThreadConsCad.Abort();
            oThreadConsInf.Abort();
            oThreadPedCan.Abort();
            oThreadPedInu.Abort();
            oThreadPedRec.Abort();
            oThreadPedSit.Abort();
            oThreadPedSta.Abort();
            oThreadMontaLoteUmaNfe.Abort();
            oThreadMontaLoteVariasNfe.Abort();
            oThreadAssinaNfeEnvio.Abort();
            oThreadAssinaNfeEnvioEmLote.Abort();
            oThreadEnviarLoteNfe.Abort();
            oThreadValidarAssinar.Abort();
            oThreadConvTXT.Abort();                
        }

        private void sobreOUniNFeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSobre oSobre = new FormSobre();
            oSobre.MinimizeBox = true;
            oSobre.ShowInTaskbar = true;
            oSobre.ShowDialog();
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var consultaCadastro = new FormConsultaCadastro();
            consultaCadastro.MinimizeBox = true;
            consultaCadastro.ShowInTaskbar = true;
            consultaCadastro.ShowDialog();

            //this.DemonstrarStatusServico();
        }

        private void vaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ValidarXML oValidarXML = new ValidarXML();
            oValidarXML.ShowInTaskbar = true;
            oValidarXML.MinimizeBox = true;
            oValidarXML.Show();
        }

        private void configuraçõesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Configuracao oConfig = new Configuracao();
            oConfig.MinimizeBox = true;
            oConfig.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.booPodeFechar = true;
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //
            // TODO: Aqui, deveriamos verificar se ainda existe alguma Thread pendente antes de fechar
            //
            if (e.CloseReason == CloseReason.UserClosing && ! this.booPodeFechar)
            {
                // se o botão de fechar for pressionado pelo usuário, o mainform não será fechado em sim minimizado.
                e.Cancel = true;
                this.Visible = false;
                this.OnResize(e);
                notifyIcon1.ShowBalloonTip(6000);
            }
            else
            {
                e.Cancel = false;  //se o PC for desligado o windows o fecha automaticamente.
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\UniNFe.pdf"))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\UniNFe.pdf");
                }
                else
                {
                    MessageBox.Show("Não foi possível localizar o arquivo de manual do UniNFe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
    #endregion
}

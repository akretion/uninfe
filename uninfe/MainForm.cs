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

            #region Executar os serviços em novas threads
            //Executar o método para forçar os serviços a carregarem as configurações assim 
            // que forem executados$
            ServicoUniNFe.CarregarConfiguracoes();

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
            ConfigUniNFe oConfig = new ConfigUniNFe();
            oConfig.CarregarDados();

            //Atualizar o nome da empresa na toolstrip
            //Wandrey 28/11/2008
            this.toolStripLabel_NomeEmpresa.Text = oConfig.cNomeEmpresa;
            if (oConfig.cNomeEmpresa.Trim() != "")
            {
                this.notifyIcon1.Text = "UniNFe\r\n" + oConfig.cNomeEmpresa;
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
            oThreadPedCan.Name = ServicoUniNFe.Servicos.CancelarNFe.ToString();
            oThreadPedCan.Start(ServicoUniNFe.Servicos.CancelarNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s de Inutilização de números de notas fiscais
            oOperacaoPedInu = new ParameterizedThreadStart(oServicoPedInu.BuscaXML);
            oThreadPedInu = new Thread(oOperacaoPedInu); 
            oThreadPedInu.Name = ServicoUniNFe.Servicos.InutilizarNumerosNFe.ToString();
            oThreadPedInu.Start(ServicoUniNFe.Servicos.InutilizarNumerosNFe);
            #endregion
                        
            #region Executar a Thread que verifica e processa os XML´s Pedido da Situação da Nota Fiscal Eletrônica
            oOperacaoPedSit = new ParameterizedThreadStart(oServicoPedSit.BuscaXML);
            oThreadPedSit = new Thread(oOperacaoPedSit); 
            oThreadPedSit.Name = ServicoUniNFe.Servicos.PedidoConsultaSituacaoNFe.ToString();
            oThreadPedSit.Start(ServicoUniNFe.Servicos.PedidoConsultaSituacaoNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido do Status do Serviço da Nota Fiscal Eletrônica
            oOperacaoPedSta = new ParameterizedThreadStart(oServicoPedSta.BuscaXML);
            oThreadPedSta = new Thread(oOperacaoPedSta); 
            oThreadPedSta.Name = ServicoUniNFe.Servicos.PedidoConsultaStatusServicoNFe.ToString();
            oThreadPedSta.Start(ServicoUniNFe.Servicos.PedidoConsultaStatusServicoNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido da Situação do Recibo do Lote de Notas Fiscais Eletrônica
            oOperacaoPedRec = new ParameterizedThreadStart(oServicoPedRec.BuscaXML);
            oThreadPedRec = new Thread(oOperacaoPedRec);
            oThreadPedRec.Name = ServicoUniNFe.Servicos.PedidoSituacaoLoteNFe.ToString();
            oThreadPedRec.Start(ServicoUniNFe.Servicos.PedidoSituacaoLoteNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido de Consulta do Cadastro de Contribuintes
            oOperacaoConsCad = new ParameterizedThreadStart(oServicoConsCad.BuscaXML);
            oThreadConsCad = new Thread(oOperacaoConsCad);
            oThreadConsCad.Name = ServicoUniNFe.Servicos.ConsultaCadastroContribuinte.ToString();
            oThreadConsCad.Start(ServicoUniNFe.Servicos.ConsultaCadastroContribuinte);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido de Consulta das Informações do UniNfe
            oOperacaoConsInf = new ParameterizedThreadStart(oServicoConsInf.BuscaXML);
            oThreadConsInf = new Thread(oOperacaoConsInf);
            oThreadConsInf.Name = ServicoUniNFe.Servicos.ConsultaInformacoesUniNFe.ToString();
            oThreadConsInf.Start(ServicoUniNFe.Servicos.ConsultaInformacoesUniNFe);
            #endregion

            #region Executar a Thread que verifica e processa os XML´s Pedido de Alteração das Configurações do UniNfe
            oOperacaoAltCon = new ParameterizedThreadStart(oServicoAltCon.BuscaXML);
            oThreadAltCon = new Thread(oOperacaoAltCon);
            oThreadAltCon.Name = ServicoUniNFe.Servicos.AlterarConfiguracoesUniNFe.ToString();
            oThreadAltCon.Start(ServicoUniNFe.Servicos.AlterarConfiguracoesUniNFe);
            #endregion
  
            #region Executar a Thread que verifica e assina os XML´s de notas fiscais eletrônicas da pasta Envio
            oOperacaoAssinaNfeEnvio = new ParameterizedThreadStart(oServicoAssinaNfeEnvio.BuscaXML);
            oThreadAssinaNfeEnvio = new Thread(oOperacaoAssinaNfeEnvio);
            oThreadAssinaNfeEnvio.Name = ServicoUniNFe.Servicos.AssinarNFePastaEnvio.ToString();
            oThreadAssinaNfeEnvio.Start(ServicoUniNFe.Servicos.AssinarNFePastaEnvio);
            #endregion  
 
            #region Executar a Thread que verifica e monta o lote com uma única nota fiscal eletrônica
            oOperacaoMontaLoteUmaNfe = new ParameterizedThreadStart(oServicoMontaLoteUmaNfe.BuscaXML);
            oThreadMontaLoteUmaNfe = new Thread(oOperacaoMontaLoteUmaNfe);
            oThreadMontaLoteUmaNfe.Name = ServicoUniNFe.Servicos.MontarLoteUmaNFe.ToString();
            oThreadMontaLoteUmaNfe.Start(ServicoUniNFe.Servicos.MontarLoteUmaNFe);
            #endregion
    
            #region Executar a Thread que envia os lotes de notas fiscais eletrônicas
            oOperacaoEnviarLoteNfe = new ParameterizedThreadStart(oServicoEnviarLoteNfe.BuscaXML);  
            oThreadEnviarLoteNfe = new Thread(oOperacaoEnviarLoteNfe);
            oThreadEnviarLoteNfe.Name = ServicoUniNFe.Servicos.EnviarLoteNfe.ToString();
            oThreadEnviarLoteNfe.Start(ServicoUniNFe.Servicos.EnviarLoteNfe);
            #endregion

            #region Executar a Thread que somente Valida e Assina XML´s e dá o retorno para o ERP
            oOperacaoValidarAssinar = new ParameterizedThreadStart(oServicoValidarAssinar.BuscaXML);
            oThreadValidarAssinar = new Thread(oOperacaoValidarAssinar);
            oThreadValidarAssinar.Name = ServicoUniNFe.Servicos.ValidarAssinar.ToString();
            oThreadValidarAssinar.Start(ServicoUniNFe.Servicos.ValidarAssinar);
            #endregion

            #region Executar a Thread que somente Converte o TXT da NFe para XML
            oOperacaoConvTXT = new ParameterizedThreadStart(oServicoConvTXT.BuscaXML);
            oThreadConvTXT = new Thread(oOperacaoConvTXT);
            oThreadConvTXT.Name = ServicoUniNFe.Servicos.ConverterTXTparaXML.ToString();
            oThreadConvTXT.Start(ServicoUniNFe.Servicos.ConverterTXTparaXML);
            #endregion

            #region Executar a Thread que verifica e assina os XML´s de notas fiscais eletrônicas da pasta de Envio em Lote
//          oOperacaoAssinaNfeEnvioEmLote = new ParameterizedThreadStart(oServicoAssinaNfeEnvioEmLote.BuscaXML);
//          oThreadAssinaNfeEnvioEmLote = new Thread(oOperacaoAssinaNfeEnvioEmLote);
//          oThreadAssinaNfeEnvioEmLote.Name = ServicoUniNFe.Servicos.AssinarNFePastaEnvioEmLote.ToString();
//          oThreadAssinaNfeEnvioEmLote.Start(ServicoUniNFe.Servicos.AssinarNFePastaEnvioEmLote);
            #endregion
  
            #region Executar a Thread que verifica e monta o lote com várias notas fiscais eletrônicas
//          oOperacaoMontaLoteVariasNfe = new ParameterizedThreadStart(oServicoMontaLoteVariasNfe.BuscaXML);
//          oThreadMontaLoteVariasNfe = new Thread(oOperacaoMontaLoteVariasNfe);
//          oThreadMontaLoteUmaNfe.Name = ServicoUniNFe.Servicos.MontarLoteVariasNFe.ToString();
//          oThreadMontaLoteUmaNfe.Start(ServicoUniNFe.Servicos.MontarLoteVariasNFe);
            #endregion   
        }
        #endregion

        #region DemonstrarStatusServico()
        private void DemonstrarStatusServico()
        {
            //Carregar configurações
            ConfigUniNFe oConfig = new ConfigUniNFe();
            UniNfeClass oNfe = new UniNfeClass();

            oConfig.CarregarDados();

            oNfe.oCertificado = oConfig.oCertificado;
            oNfe.vUF = oConfig.vUnidadeFederativaCodigo;
            oNfe.vAmbiente = oConfig.vAmbienteCodigo;
            oNfe.vPastaXMLEnvio = oConfig.vPastaXMLEnvio;
            oNfe.vPastaXMLRetorno = oConfig.vPastaXMLRetorno;
            oNfe.vPastaXMLEnviado = oConfig.vPastaXMLEnviado;
            oNfe.vPastaXMLErro = oConfig.vPastaXMLErro;
            oNfe.vPastaXMLEmLote = oConfig.cPastaXMLEmLote;
            oNfe.cPastaBackup = oConfig.cPastaBackup;
            oNfe.vTpEmis = oConfig.vTpEmis;

            //Demonstrar o status do serviço
            MessageBox.Show(oNfe.VerStatusServico(), "Situação do serviço da NFe é:", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #endregion

        #region Métodos de eventos
        private void toolStripButton_config_Click(object sender, EventArgs e)
        {
            FormConfiguracao oConfig = new FormConfiguracao();
            oConfig.MdiParent = this;
            oConfig.MinimizeBox = false;
            oConfig.Show();
        }

        private void toolStripButton_teste_Click(object sender, EventArgs e)
        {
            Form1 oFormTeste = new Form1();
            oFormTeste.MdiParent = this;
            oFormTeste.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.DemonstrarStatusServico();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Faz a aplicação sumir da barra de tarefas
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
            FormValidarXML oValidarXML = new FormValidarXML();
            oValidarXML.MinimizeBox = false;
            oValidarXML.ShowInTaskbar = false;
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
//          oThreadMontaLoteVariasNfe.Abort();
            oThreadAssinaNfeEnvio.Abort();
//          oThreadAssinaNfeEnvioEmLote.Abort();
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
            this.DemonstrarStatusServico();
        }

        private void vaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormValidarXML oValidarXML = new FormValidarXML();
            oValidarXML.ShowInTaskbar = true;
            oValidarXML.MinimizeBox = true;
            oValidarXML.Show();
        }

        private void configuraçõesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormConfiguracao oConfig = new FormConfiguracao();
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
        #endregion

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
    }
    #endregion
}

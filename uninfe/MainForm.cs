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
using System.Reflection;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace uninfe
{
    public partial class MainForm : Form
    {
        #region Definir os objetos dos serviços executados pelo UniNFe
        ServicoUniNFe oServicoNfe = new ServicoUniNFe();
        ServicoUniNFe oServicoPedCan = new ServicoUniNFe();
        ServicoUniNFe oServicoPedInu = new ServicoUniNFe();
        ServicoUniNFe oServicoPedRec = new ServicoUniNFe();
        ServicoUniNFe oServicoPedSit = new ServicoUniNFe();
        ServicoUniNFe oServicoPedSta = new ServicoUniNFe();
        ServicoUniNFe oServicoConsCad = new ServicoUniNFe();
        ServicoUniNFe oServicoConsInf = new ServicoUniNFe();
        ServicoUniNFe oServicoAltCon = new ServicoUniNFe();
        #endregion

        #region Definir os objetos de threads para executar os serviços do UniNFe
        ParameterizedThreadStart oOperacaoNfe;
        Thread oThreadNfe;

        ParameterizedThreadStart oOperacaoPedCan;
        Thread oThreadPedCan;

        ParameterizedThreadStart oOperacaoPedInu;
        Thread oThreadPedInu;

        ParameterizedThreadStart oOperacaoPedRec;
        Thread oThreadPedRec;

        ParameterizedThreadStart oOperacaoPedSit;
        Thread oThreadPedSit;

        ParameterizedThreadStart oOperacaoPedSta;
        Thread oThreadPedSta;

        ParameterizedThreadStart oOperacaoConsCad;
        Thread oThreadConsCad;

        ParameterizedThreadStart oOperacaoConsInf;
        Thread oThreadConsInf;

        ParameterizedThreadStart oOperacaoAltCon;
        Thread oThreadAltCon;
        #endregion

        public MainForm()
        {
            InitializeComponent();
            AtualizarDadosToolBar();

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

        /// <summary>
        /// Metodo responsável por iniciar os serviços do UniNFe em threads diferentes
        /// </summary>
        private void ExecutaServicos()
        {
            oOperacaoNfe = new ParameterizedThreadStart(oServicoNfe.BuscaXML); oThreadNfe = new Thread(oOperacaoNfe);
            oOperacaoPedCan = new ParameterizedThreadStart(oServicoPedCan.BuscaXML); oThreadPedCan = new Thread(oOperacaoPedCan);
            oOperacaoPedInu = new ParameterizedThreadStart(oServicoPedInu.BuscaXML); oThreadPedInu = new Thread(oOperacaoPedInu);
            oOperacaoPedRec = new ParameterizedThreadStart(oServicoPedRec.BuscaXML); oThreadPedRec = new Thread(oOperacaoPedRec);
            oOperacaoPedSit = new ParameterizedThreadStart(oServicoPedSit.BuscaXML); oThreadPedSit = new Thread(oOperacaoPedSit);
            oOperacaoPedSta = new ParameterizedThreadStart(oServicoPedSta.BuscaXML); oThreadPedSta = new Thread(oOperacaoPedSta);
            oOperacaoConsCad = new ParameterizedThreadStart(oServicoConsCad.BuscaXML); oThreadConsCad = new Thread(oOperacaoConsCad);
            oOperacaoConsInf = new ParameterizedThreadStart(oServicoConsInf.BuscaXML); oThreadConsInf = new Thread(oOperacaoConsInf);
            oOperacaoAltCon = new ParameterizedThreadStart(oServicoAltCon.BuscaXML); oThreadAltCon = new Thread(oOperacaoAltCon);

            oThreadNfe.Name = "ServicoNFe"; oThreadNfe.Start("*-nfe.xml");
            oThreadPedCan.Name = "ServicoPedCan"; oThreadPedCan.Start("*-ped-can.xml");
            oThreadPedInu.Name = "ServicoPedInu"; oThreadPedInu.Start("*-ped-inu.xml");
            oThreadPedRec.Name = "ServicoPedRec"; oThreadPedRec.Start("*-ped-rec.xml");
            oThreadPedSit.Name = "ServicoPedSit"; oThreadPedSit.Start("*-ped-sit.xml");
            oThreadPedSta.Name = "ServicoPedSta"; oThreadPedSta.Start("*-ped-sta.xml");
            oThreadConsCad.Name = "ServicoConsCad"; oThreadConsCad.Start("*-cons-cad.xml");
            oThreadConsInf.Name = "ServicoConsInf"; oThreadConsInf.Start("*-cons-inf.xml");
            oThreadAltCon.Name = "ServicoAltCon"; oThreadAltCon.Start("*-alt-con.xml");
        }

        #region Métodos de eventos
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

        private void toolStripButton1_Click(object sender, EventArgs e)
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
            oNfe.cPastaBackup = oConfig.cPastaBackup;
            oNfe.vTpEmis = oConfig.vTpEmis;

            //Demonstrar o status do serviço
            MessageBox.Show(oNfe.VerStatusServico(), "Situação do serviço da NFe é:", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            oThreadNfe.Abort();
            oThreadAltCon.Abort();
            oThreadConsCad.Abort();
            oThreadConsInf.Abort();
            oThreadPedCan.Abort();
            oThreadPedInu.Abort();
            oThreadPedRec.Abort();
            oThreadPedSit.Abort();
            oThreadPedSta.Abort();
        }
        #endregion
    }

    /// <summary>
    /// Classe responsável pela execução dos serviços do UniNFe
    /// </summary>
    public class ServicoUniNFe
    {
        /// <summary>
        /// Lista que vai receber um string de identificação dos
        /// serviços que devem ser recarregar as configurações.
        /// </summary>
        static List<string> lstContrConfig = new List<string>(9);

        /// <summary>
        /// Atualiza a lista de serviços que devem recarregar as configurações. Este
        /// método marca todos os serviços de uma única vez para recarregar.
        /// </summary>
        /// <date>10/02/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>        
        public static void CarregarConfiguracoes()
        {
            lstContrConfig.Clear();

            lstContrConfig.Add("*-nfe.xml");
            lstContrConfig.Add("*-ped-can.xml");
            lstContrConfig.Add("*-ped-inu.xml");
            lstContrConfig.Add("*-ped-rec.xml");
            lstContrConfig.Add("*-ped-sit.xml");
            lstContrConfig.Add("*-ped-sta.xml");
            lstContrConfig.Add("*-cons-cad.xml");
            lstContrConfig.Add("*-cons-inf.xml");
            lstContrConfig.Add("*-alt-con.xml");
        }

        /// <summary>
        /// Procurar os arquivos XML´s a serem enviados aos web-services ou para ser executado alguma rotina
        /// </summary>
        /// <param name="pTipoArq">Mascara dos arquivos as serem pesquisados. Ex: *.xml   *-nfe.xml</param>
        public void BuscaXML(Object oMascaraArq)
        {
            ConfigUniNFe oConfig = new ConfigUniNFe();
            UniNfeClass oNfe = new UniNfeClass();

            string sMascaraArq = oMascaraArq.ToString();

            while (true)
            {
                if (lstContrConfig.FindIndex(delegate(string s)
                {
                    return s.Equals(sMascaraArq);
                }) > -1)
                {
                    //Encontrou o sMascaraArq na lista, então tenho que recarregar as configurações pq algo foi modificado pelo usuário na tela de configurações.
                    oConfig.CarregarDados();

                    oNfe.oCertificado = oConfig.oCertificado;
                    oNfe.vUF = oConfig.vUnidadeFederativaCodigo;
                    oNfe.vAmbiente = oConfig.vAmbienteCodigo;
                    oNfe.vPastaXMLEnvio = oConfig.vPastaXMLEnvio;
                    oNfe.vPastaXMLRetorno = oConfig.vPastaXMLRetorno;
                    oNfe.vPastaXMLEnviado = oConfig.vPastaXMLEnviado;
                    oNfe.vPastaXMLErro = oConfig.vPastaXMLErro;
                    oNfe.cPastaBackup = oConfig.cPastaBackup;
                    oNfe.vTpEmis = oConfig.vTpEmis;

                    //Remover o item da lista para não recarregar mais a configuração
                    lstContrConfig.Remove(sMascaraArq);
                }
                else if (oConfig.vPastaXMLEnvio != string.Empty)
                {
                    List<string> lstArquivos = new List<string>();

                    try
                    {
                        foreach (string item in Directory.GetFiles(oConfig.vPastaXMLEnvio, sMascaraArq))
                        {
                            lstArquivos.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso a pasta.
                    }

                    for (int i = 0; i < lstArquivos.Count; i++)
                    {
                        try
                        {
                            using (FileStream fs = File.Open(lstArquivos[i], FileMode.Open, FileAccess.ReadWrite, FileShare.Write))
                            {
                                //Conseguiu abrir o arquivo, significa que está perfeitamente gerado
                                //assim vou iniciar o processo de envio do XML
                                fs.Close();
                                this.EnviarArquivo(lstArquivos[i], oNfe);
                            }
                        }
                        catch (IOException ex)
                        {
                            //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                        }
                        catch (Exception ex)
                        {
                            //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                        }                       
                    }                     
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Analisa o tipo do XML que está na pasta de envio e executa a operação necessária. Exemplo: Envia ao SEFAZ, reconfigura o UniNFE, etc... 
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML a ser enviado ou analisado</param>
        /// <param name="oNfe">Objeto da classe UniNfeClass a ser utilizado nas operações</param>
        private void EnviarArquivo(string cArquivo, Object oNfe)
        {
            string cMetodo = "";

            #region Definir o método que vai ser executado

            if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 4, 4).ToLower() == "-nfe") //-nfe.xml = Arquivo XML de Nota Fiscal Eletronica a ser assinada e incluida em um lote de notas
            {
                cMetodo = "Recepcao";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-rec") //-ped-rec.xml = Arquivo XML de Pedido do Resultado do Processamento do Lote de NF-e
            {
                cMetodo = "RetRecepcao";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-sit") //-ped-sit.xml = Arquivo XML de Pedido da Consulta Atual da NF-e
            {
                cMetodo = "Consulta";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-sta") //-ped-sta.xml = Arquivo XML de Pedido de Consulta do Status do Serviço
            {
                cMetodo = "StatusServico";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-can") //-ped-can.xml = Arquivo XML de Cancelamento de Notas Fiscais
            {
                cMetodo = "Cancelamento";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-ped-inu") //-ped-inu.xml = Arquivo XML de Inutilização de Numeração de Notas Fiscais
            {
                cMetodo = "Inutilizacao";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 9, 9).ToLower() == "-cons-cad") //-cons-cad.xml = Arquivo XML de Consulta do Cadastro dos Contribuintes
            {
                cMetodo = "ConsultaCadastro";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 8, 8).ToLower() == "-alt-con") //-alt-con.xml = Arquivo XML de Configuração Automática do UniNFe
            {
                cMetodo = "ReconfigurarUniNfe";
            }
            else if (cArquivo.Substring(cArquivo.ToUpper().IndexOf(".XML") - 9, 9).ToLower() == "-cons-inf") //-cons-inf = Arquivo XML de Consulta das informações do UniNfe
            {
                cMetodo = "GravarXMLDadosCertificado";
            }

            #endregion

            //Definir o tipo do serviço
            Type tipoServico = oNfe.GetType();

            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.SetProperty, null, oNfe, new object[] { cArquivo });

            //Buscar o tipo de emissão configurado
            int vTpEmis = (int)tipoServico.InvokeMember("vTpEmis", BindingFlags.GetProperty, null, oNfe, null);

            if (vTpEmis != 2) //2-Confingência em Formulário decimal segurança não envia na hora, tem que aguardar voltar para normal.
            {
                if (cMetodo == "ReconfigurarUniNfe")
                {
                    this.ReconfigurarUniNFe(cArquivo);
                }
                else if (cMetodo == "GravarXMLDadosCertificado")
                {
                    this.GravarXMLDadosCertificado(oNfe);
                }
                else
                {
                    tipoServico.InvokeMember(cMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oNfe, null);
                }
            }
            else
            {
                if (cMetodo == "ReconfigurarUniNfe")
                {
                    this.ReconfigurarUniNFe(cArquivo);
                }
                else if (cMetodo == "RetRecepcao" || cMetodo == "Consulta" || cMetodo == "StatusServico")
                {
                    tipoServico.InvokeMember(cMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oNfe, null);
                }
                else if (cMetodo == "GravarXMLDadosCertificado")
                {
                    this.GravarXMLDadosCertificado(oNfe);
                }
            }
        }

        /// <summary>
        /// Gravar o XML de retorno com as informações do UniNFe para o aplicativo de ERP
        /// </summary>
        /// <param name="oNfe">Objeto da classe UniNfeClass para conseguir pegar algumas informações para gravar o XML</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        private void GravarXMLDadosCertificado(Object oNfe)
        {
            //Definir o tipo do serviço
            Type tipoServico = oNfe.GetType();

            string cArquivoEnvio = (string)tipoServico.InvokeMember("vXmlNfeDadosMsg", BindingFlags.GetProperty, null, oNfe, null);
            string cPastaRetorno = (string)tipoServico.InvokeMember("vPastaXMLRetorno", BindingFlags.GetProperty, null, oNfe, null);

            UniNfeInfClass oInfUniNfe = new UniNfeInfClass();

            //Deletar o arquivo de solicitação do serviço
            FileInfo oArquivo = new FileInfo(cArquivoEnvio);
            oArquivo.Delete();

            oInfUniNfe.GravarXMLInformacoes(cPastaRetorno + "\\uninfe-ret-cons-inf.xml");
        }

        /// <summary>
        /// Reconfigura o UniNFe, gravando as novas informações na tela de configuração
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML contendo as novas configurações</param>
        private void ReconfigurarUniNFe(string cArquivo)
        {
            ConfigUniNFe oConfig = new ConfigUniNFe();
            oConfig.ReconfigurarUniNFe(cArquivo);

            CarregarConfiguracoes();
        }

    }
}

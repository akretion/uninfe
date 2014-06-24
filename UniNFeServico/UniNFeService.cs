using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;
using NFe.Components;
using NFe.Settings;
using NFe.Threadings;

namespace UniNFeServico
{
    public partial class UniNFeService : ServiceBase
    {
        private List<Thread> threads = new List<Thread>();
        //private string StartupPath;

        public UniNFeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            //StartupPath = NFe.Components.Propriedade.PastaExecutavel;
            WriteLog("Serviço iniciado na pasta: " + NFe.Components.Propriedade.PastaExecutavel);
            this.iniciarServicosUniNFe();
        }

        protected override void OnStop()
        {
            base.OnStop();

            WriteLog("Serviço parado");
            this.pararServicosUniNFe();
        }

        /*
        protected override void OnPause()
        {
            base.OnPause();
            WriteLog("Serviço pausado");
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            WriteLog("Serviço continuado");
        }
        */

        protected override void OnShutdown()
        {
            this.pararServicosUniNFe();
            WriteLog("Serviço terminado");
            base.OnShutdown();
        }

        private void iniciarServicosUniNFe()
        {
            #region Definir valores propriedades de configuração
            //Carregar as configurações antes de executar os serviços do UNINFE
            Propriedade.TipoAplicativo = TipoAplicativo.Nfe;
            ConfiguracaoApp.StartVersoes();
            #endregion

            // Executar as conversões de atualizações de versão quando tiver
            Auxiliar.ConversaoNovaVersao(string.Empty);
            Empresa.CarregaConfiguracao();

            //Executar o monitoramento de pastas das empresas cadastradas
            MonitoraPasta e = new MonitoraPasta();

            threads.Clear();

            ThreadService.Start();

#if nao
            //Executa a thread que faz a limpeza dos arquivos temporários
            Thread t = new Thread(new ServicoUniNFe().LimpezaTemporario);
            t.Start();
            threads.Add(t);

            //Executa a thread que faz a verificação das notas em processamento
            Thread t2 = new Thread(new ServicoUniNFe().EmProcessamento);
            t2.Start();
            threads.Add(t2);

            //Executar a thread que faz a consulta do recibo das notas fiscais enviadas
            ServicoUniNFe srv = new ServicoUniNFe();
            Thread t3 = new Thread(srv.GerarXMLPedRec);
            t3.Start(new ServicoNFe());
            threads.Add(t3);
#endif

            new ThreadControlEvents();
        }

        private void pararServicosUniNFe()
        {
            for (int i = 0; i < threads.Count; i++)
            {
                Thread t = threads[i];
                t.Abort();
            }
            for (int i = 0; i < MonitoraPasta.fsw.Count; i++)
            {
                MonitoraPasta.fsw[i].StopWatch = true;
            }
        }

        protected void WriteEventEntry(
            String Message,
            EventLogEntryType EventType,
            int ID,
            short Category)
        {
            // Select the log.
            EventLog.Log = "Application";

            // Define the source.
            EventLog.Source = ServiceName;

            // Write the log entry.
            EventLog.WriteEntry(Message, EventType, ID, Category);
        }

        protected override void OnCustomCommand(int command)
        {
            // Perform the custom command.
            if (command == 130)
            {
                WriteEventEntry("Executed Custom Command", EventLogEntryType.Information, 2000, 2);
            }
            // Perform the default action.
            base.OnCustomCommand(command);
        }

        // OBTENDO O ENDEREÇO FÍSICO DE UM WINDOWS SERVICE
        // Obtêm o endereço físico (diretório) do serviço
        // defaultDir representa um diretório padrão caso não seja possível obter a chave
        // this.ServiceName é o nome do serviço
        
        private string GetPhysicalPath(string defaultDir)
        {
            string diretorio;

            //' Obtêm a chave de registro.
            Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + this.ServiceName, false);

            diretorio = (string)Key.GetValue("ImagePath", defaultDir);

            //StreamWriter arquivoWS = new StreamWriter("c:\\demoWSEventos.log");
            //arquivoWS.WriteLine(diretorio);
            //arquivoWS.WriteLine(Path.GetDirectoryName(diretorio.Replace("\"", "")));
            //arquivoWS.Flush();
            //arquivoWS.Close();

            return Path.GetDirectoryName(diretorio.Replace("\"", ""));

            //' Remove diretórios de executáveis e debug, no caso de querer o path padrão
            //diretorio = diretorio.Replace("\\bin", "").Replace("\\Debug", "");

            //' Retorna o diretório sem o arquivo executável
            //' Obtêm a última barra do diretório e seleciona a string até este ponto.
            //int UltBarra = diretorio.LastIndexOf("\\");
            //return diretorio.Substring(0, UltBarra + 1);

            //' Assim, caso seu Windows Service esteja em:
            //'c:\Projetos\WinService\bin\debug\WinService.exe, esse método obterá o path:
            //'c:\Projetos\WinService
        }
        void WriteLog(string msg)
        {
            Auxiliar.WriteLog(msg);
        }
    }
}

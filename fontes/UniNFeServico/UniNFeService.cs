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
    [ToolboxItem(false)]
    public partial class UniNFeService : ServiceBase
    {
        public UniNFeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            base.OnStart(args);           
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
            Propriedade.TipoAplicativo = TipoAplicativo.Todos;
            ConfiguracaoApp.StartVersoes();

            Empresas.CarregaConfiguracao();

            string filenameWS1 = Propriedade.NomeArqXMLMunicipios;
            string filenameWS2 = Propriedade.NomeArqXMLWebService_NFSe;
            string filenameWS3 = Propriedade.NomeArqXMLWebService_NFe;
            string msg = "";
            bool error = false;
            Propriedade.VerificaArquivos(out error, out msg);
            if (error)
            {
                this.WriteLog(msg);
            }
            else
                foreach (Empresa empresa in Empresas.Configuracoes)
                {
                    if (empresa.X509Certificado == null && empresa.UsaCertificado)
                    {
                        msg = "Não pode ler o certificado da empresa: " + empresa.CNPJ + "=>" + empresa.Nome + "=>" + empresa.Servico.ToString();

                        string f = Path.Combine(empresa.PastaXmlRetorno,
                                                "uninfeServico_" + DateTime.Now.ToString("yyyy-MMM-dd_hh-mm-ss") + ".txt");
                        System.IO.File.WriteAllText(f, msg);
                       
                        WriteLog(msg);
                    }
                }

            if (!error)
            {
                // Executar as conversões de atualizações de versão quando tiver
                Auxiliar.ConversaoNovaVersao(string.Empty);

                ThreadService.Start();

                new ThreadControlEvents();
            }
            else
            {
                WriteLog("Servico do UniNFe não está sendo executado ");
            }
        }

        private void pararServicosUniNFe()
        {
            ThreadService.Stop();
            Empresas.ClearLockFiles(false);
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
            bool o = ConfiguracaoApp.GravarLogOperacoesRealizadas;
            ConfiguracaoApp.GravarLogOperacoesRealizadas = true;
            Auxiliar.WriteLog(msg, false);
            ConfiguracaoApp.GravarLogOperacoesRealizadas = o;
        }
    }
}

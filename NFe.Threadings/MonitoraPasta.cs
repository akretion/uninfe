using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using NFe.Settings;

namespace NFe.Threadings
{
    /// <summary>
    /// Executar as thread´s dos serviços do UniNFe e NFe conforme necessário
    /// </summary>
    /// <remarks>
    /// Autor: Wandrey Mundin Ferreira
    /// Data: 04/04/2011
    /// </remarks>
    public class MonitoraPasta
    {
        #region Construtores
        /// <summary>
        /// Construtor
        /// </summary>
        public MonitoraPasta()
        {
            MonitorarPasta();
        }
        #endregion

        #region Propriedades
        /// <summary>
        /// Tipos de arquivos a serem monitorados pelo UniNFe
        /// </summary>
        public static List<FileSystemWatcher> fsw = new List<FileSystemWatcher>();
        #endregion

        #region MonitorarPastas()
        /// <summary>
        /// Executa as thread´s de monitoração de pastas
        /// </summary>
        private void MonitorarPasta()
        {
            fsw.Clear();

            for(int i = 0; i < Empresa.Configuracoes.Count; i++)
            {
                #region Pasta de envio
                fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaXmlEnvio, "*.xml"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();

                fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaXmlEnvio, "*.txt"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();
                #endregion

                #region Pasta de envio em Lote
                if(!string.IsNullOrEmpty(Empresa.Configuracoes[i].PastaXmlEmLote))
                {
                    fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaXmlEmLote, "*.xml"));
                    fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                    fsw[fsw.Count - 1].StartWatch();

                    fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaXmlEmLote, "*.txt"));
                    fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                    fsw[fsw.Count - 1].StartWatch();
                }
                #endregion

                #region Pasta Validar
                fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaValidar, "*.xml"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();

                fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaValidar, "*.txt"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();
                #endregion
            }

            #region Pasta Geral
            fsw.Add(new FileSystemWatcher(Path.Combine(System.Windows.Forms.Application.StartupPath, "Geral"), "*.xml"));
            fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
            fsw[fsw.Count - 1].StartWatch();
            #endregion
        }
        #endregion

        #region LocalizaEmpresa()
        /// <summary>
        /// Localiza a empresa da qual o arquivo faz parte para processar com as configurações corretas
        /// </summary>
        /// <param name="fullPath">Nome do arquivo completo (com pasta e tudo)</param>
        /// <returns>Index da empresa dentro da lista de configurações</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 26/04/2011
        /// </remarks>
        private int LocalizaEmpresa(FileInfo fi)
        {
            int empresa = -1;

            try
            {
                ///
                ///danasa 3/11/2011
                string fullName = ConfiguracaoApp.RemoveEndSlash(fi.Directory.FullName.ToLower());
                ///
                /// "EndsWith" é para pegar apenas se terminar com, já que nas empresas pode ter um nome 'temp' no meio das definicoes das pastas
                if(fullName.EndsWith("\\temp"))
                    /// exclui o 'arquivo' temp.
                    fullName = Path.GetDirectoryName(fullName);

                for(int i = 0; i < Empresa.Configuracoes.Count; i++)
                {
                    if(fullName == Empresa.Configuracoes[i].PastaXmlEnvio.ToLower() ||
                        fullName == Empresa.Configuracoes[i].PastaXmlEmLote.ToLower() ||
                        fullName == Empresa.Configuracoes[i].PastaValidar.ToLower())
                    {
                        empresa = i;
                        break;
                    }
                }
            }
            catch
            {
            }

            return empresa;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento que executa thread´s para processar os arquivos que são colocados nas pastas que estão sendo monitoradas pela FileSystemWatcher
        /// </summary>
        /// <param name="fi">Arquivo detectado pela FileSystemWatcher</param>
        private void fsw_OnFileChanged(FileInfo fi)
        {
            try
            {
                int empresa;
                string arq = fi.FullName.ToLower();

                if(fi.Directory.FullName.ToLower().EndsWith("geral\\temp"))
                {
                    //encerra o UniNFe no arquivo -sair.xmls
                    if(arq.EndsWith("-sair.xml"))
                    {
                        File.Delete(fi.FullName);
                        Environment.Exit(0);
                    }

                    empresa = 1; //Vou criar fixo como 1 quando for na pasta geral, pois na pasta geral não tem como detectar qual é a empresa. Wandrey 20/03/2013
                }
                else
                {
                    empresa = LocalizaEmpresa(fi);
                }


                if(empresa >= 0)
                {
                    /*<#8084>
                     * Aqui foi modificado porque a ThreadControl deixou de existir.
                     * E todo o processamento que antes existia na thread control foi deixado apenas no método Run(), que é chamado abaixo
                     * 
                     * Marcelo
                     */
                    new ThreadItem(fi, empresa).Run();
                    //</#8084>
                }
                else
                {
                    Auxiliar.WriteLog(fi.FullName + " - Não localizou a empresa.", true);
                }
            }
            catch(Exception ex)
            {
                Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        #endregion
    }
}

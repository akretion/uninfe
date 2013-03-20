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
            for (int i = 0; i < Empresa.Configuracoes.Count; i++)
            {
                #region Pasta de envio
                fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaEnvio, "*.xml"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();

                fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaEnvio, "*.txt"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();
                #endregion

                #region Pasta de envio em Lote
                if (!string.IsNullOrEmpty(Empresa.Configuracoes[i].PastaEnvioEmLote))
                {
                    fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaEnvioEmLote, "*.xml"));
                    fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                    fsw[fsw.Count - 1].StartWatch();

                    fsw.Add(new FileSystemWatcher(Empresa.Configuracoes[i].PastaEnvioEmLote, "*.txt"));
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
        private int LocalizaEmpresa(FileInfo fi, bool lCadastrandoEmpresa)
        {
            int  empresa = -1;
            int  lQtRodou = -1;
            bool lEncontrouEmp = false;

            try
            {
                ///
                ///danasa 3/11/2011
                string fullName = ConfiguracaoApp.RemoveEndSlash(fi.Directory.FullName.ToLower());
                ///
                /// "EndsWith" é para pegar apenas se terminar com, já que nas empresas pode ter um nome 'temp' no meio das definicoes das pastas
                if (fullName.EndsWith("\\temp"))
                    /// exclui o 'arquivo' temp.
                    fullName = Path.GetDirectoryName(fullName);

                for (int i = 0; i < Empresa.Configuracoes.Count; i++)
                {
                    if (fullName == Empresa.Configuracoes[i].PastaEnvio.ToLower() ||
                        fullName == Empresa.Configuracoes[i].PastaEnvioEmLote.ToLower() ||
                        fullName == Empresa.Configuracoes[i].PastaValidar.ToLower())
                    {
                        empresa = i;
                        lEncontrouEmp = true;
                        break;
                    }

                    lQtRodou += 1;

                }

                // caso ele nao axe a empresa ele vai posicionar no ultimo mais somente se estiver cadastrando pela pasta geral.
                if (lCadastrandoEmpresa == true && lEncontrouEmp == false)
                    empresa = lQtRodou + 1;
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
                bool lCadastrandoEmp = false;
                string arq = fi.FullName.ToLower();


                /// verifica se esta cadastrando uma nova empresa - Renan
                if (fi.Directory.FullName.ToLower().EndsWith("geral\\temp"))
                {
                    //encerra o UniNFe no arquivo -sair.xmls
                    if (arq.EndsWith("-sair.xml"))
                    {
                        File.Delete(fi.FullName);
                        Environment.Exit(0);
                    }
                    else
                        lCadastrandoEmp = true;
                }

                empresa = LocalizaEmpresa(fi, lCadastrandoEmp);

                if (empresa >= 0)
                {
                    ThreadControl.Add(new ThreadItem(fi, empresa));
                }
                else
                {
                    Auxiliar.WriteLog(fi.FullName + " - Não localizou a empresa.", true);
                }
            }
            catch (Exception ex)
            {
                Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        #endregion
    }
}

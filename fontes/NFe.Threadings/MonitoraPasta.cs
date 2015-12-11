using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;

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

            for (int i = 0; i < Empresas.Configuracoes.Count; i++)
            {
                List<string> pastas = new List<string>();

                pastas.Add(Empresas.Configuracoes[i].PastaXmlEnvio);

                if (!string.IsNullOrEmpty(Empresas.Configuracoes[i].PastaXmlEmLote) &&
                    Empresas.Configuracoes[i].Servico != Components.TipoAplicativo.Nfse)
                {
                    pastas.Add(Empresas.Configuracoes[i].PastaXmlEmLote);
                }

                pastas.Add(Empresas.Configuracoes[i].PastaValidar);

                #region Pasta impressão dfe em contingência
                if (Directory.Exists(Empresas.Configuracoes[i].PastaContingencia) &&
                    Empresas.Configuracoes[i].tpEmis != (int)NFe.Components.TipoEmissao.teNormal)
                {
                    pastas.Add(Empresas.Configuracoes[i].PastaContingencia);
                }
                #endregion

                fsw.Add(new FileSystemWatcher(pastas, "*.xml,*.txt"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();
            }

            #region Pasta Geral
            fsw.Add(new FileSystemWatcher(Path.Combine(System.Windows.Forms.Application.StartupPath, "Geral"), "*.xml,*.txt"));
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
                if (fullName.EndsWith("\\temp"))
                    /// exclui o 'arquivo' temp.
                    fullName = Path.GetDirectoryName(fullName);

                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    if (fullName == Empresas.Configuracoes[i].PastaXmlEnvio.ToLower() ||
                        fullName == Empresas.Configuracoes[i].PastaXmlEmLote.ToLower() ||
                        fullName == Empresas.Configuracoes[i].PastaValidar.ToLower() ||
                        fullName == Empresas.Configuracoes[i].PastaContingencia.ToLower())
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

                if (fi.Directory.FullName.ToLower().EndsWith("geral\\temp"))
                {
                    ///
                    /// encerra o UniNFe no arquivo -sair.xml
                    /// 
                    var sext = NFe.Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.sair_XML);
                    if (arq.EndsWith(sext.EnvioTXT) || arq.EndsWith(sext.EnvioXML))
                    {
                        File.Delete(fi.FullName);
                        Empresas.ClearLockFiles(false);
                        if (!NFe.Components.Propriedade.ExecutandoPeloUniNFe)
                        {
                            if (NFe.Components.ServiceProcess.StatusService(NFe.Components.Propriedade.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Running)
                                NFe.Components.ServiceProcess.StopService(NFe.Components.Propriedade.ServiceName, 40000);
                        }
                        else
                            ThreadService.Stop();
                        Environment.Exit(0);
                        return;
                    }

                    string ExtRetorno = null;
                    string finalArqErro = null;
                    Exception exx = null;

                    ///
                    /// Atualiza WSDL / Schemas
                    /// 
                    var ext = NFe.Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedUpdatewsdl);
                    if (arq.EndsWith(ext.EnvioTXT) || arq.EndsWith(ext.EnvioXML))
                    {
                        #region ---Atualiza WSDL e Schemas
                        File.Delete(fi.FullName);

                        NFe.Components.Functions.DeletarArquivo(NFe.Settings.ConfiguracaoApp.XMLVersoesWSDL);

                        string cerros = "";
                        try
                        {
                            ConfiguracaoApp.ForceUpdateWSDL(false, ref cerros);

                            if (!string.IsNullOrEmpty(cerros)) throw new Exception(cerros);

                            string ExtRet = (arq.EndsWith(".xml") ? ext.RetornoXML : ext.RetornoTXT);
                            string arqRetorno = NFe.Components.Propriedade.PastaGeralRetorno + "\\" + NFe.Components.Functions.ExtrairNomeArq(fi.FullName, null) + ExtRet;
                            const string rst = "Schemas atualizados com sucesso!!!";

                            if (arq.EndsWith(".xml"))
                            {
                                var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                                        new XElement("UPDT",
                                                            new XElement("Result", rst)));
                                xml.Save(arqRetorno);
                            }
                            else
                                System.IO.File.WriteAllText(arqRetorno, rst);
                            return;
                        }
                        catch (Exception ex)
                        {
                            ExtRetorno = (arq.EndsWith(".xml") ? ext.EnvioXML : ext.EnvioTXT);
                            finalArqErro = ext.EnvioXML.Replace(".xml", ".err");
                            exx = ex;
                        }
                        #endregion
                    }

                    ///
                    /// restart o UniNFe
                    /// 
                    var uext = NFe.Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedRestart);
                    if (arq.EndsWith(uext.EnvioTXT) || arq.EndsWith(uext.EnvioXML))
                    {
                        #region ---Reinicia o UniNFe
                        File.Delete(fi.FullName);
                        try
                        {
                            if (NFe.Components.Propriedade.ExecutandoPeloUniNFe)
                            {
                                System.Diagnostics.Process.Start(NFe.Components.Propriedade.PastaExecutavel + "\\uninfe.exe", "/restart");
                            }
                            else
                            {
                                NFe.Components.ServiceProcess.StopService(NFe.Components.Propriedade.ServiceName, 40000);
                                for (int i = 0; i < 10; ++i)
                                {
                                    System.Threading.Thread.Sleep(500);

                                    if (NFe.Components.ServiceProcess.StatusService(NFe.Components.Propriedade.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Stopped)
                                    {
                                        NFe.Components.ServiceProcess.RestartService(NFe.Components.Propriedade.ServiceName, 40000);
                                        break;
                                    }
                                }
                            }
                            return;
                        }
                        catch(Exception ex)
                        {
                            ExtRetorno = (arq.EndsWith(".xml") ? uext.EnvioXML : uext.EnvioTXT);
                            finalArqErro = uext.EnvioXML.Replace(".xml", ".err");
                            exx = ex;
                        }
                        #endregion
                    }

                    if (ExtRetorno != null)
                    {
                        try
                        {
                            NFe.Service.TFunctions.GravarArqErroServico(fi.FullName, ExtRetorno, finalArqErro, exx);
                        }
                        catch { }
                        return;
                    }

                    ///
                    /// solicitacao de layouts
                    /// 
                    var lext = NFe.Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedLayouts);
                    if (arq.EndsWith(lext.EnvioTXT) || arq.EndsWith(lext.EnvioXML))
                    {
                        NFe.Service.TaskLayouts l = new NFe.Service.TaskLayouts();
                        l.NomeArquivoXML = fi.FullName;
                        l.Execute();
                        return;
                    }
                    empresa = 0; //Vou criar fixo como 0 quando for na pasta geral, pois na pasta geral não tem como detectar qual é a empresa. Wandrey 20/03/2013
                }
                else
                {
                    empresa = LocalizaEmpresa(fi);
                }


                if (empresa >= 0)
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
            catch (Exception ex)
            {
                if (fi.Directory.Name.ToLower().EndsWith("geral\\temp"))
                    NFe.Components.Functions.WriteLog(ex.Message + "\r\n" + ex.StackTrace, false, true, "");
                else
                    Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace, false);
            }
        }

        #endregion
    }
}

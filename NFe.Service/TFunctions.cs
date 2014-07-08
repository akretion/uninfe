using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;
using System.IO.Compression;

namespace NFe.Service
{
    public class TFunctions
    {
        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erros ocorridos durante as operações para que o ERP possa tratá-los        
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de Erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="errorCode">Código do erro</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 02/06/2011
        /// </remarks>
        public static void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception)
        {
            GravarArqErroServico(arquivo, finalArqEnvio, finalArqErro, exception, ErroPadrao.ErroNaoDetectado, true);
        }
        #endregion

        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erros ocorridos durante as operações para que o ERP possa tratá-los        
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de Erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="errorCode">Código do erro</param>
        /// <param name="moveArqErro">Move o arquivo informado no parametro "arquivo" para a pasta de XML com ERRO</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 02/06/2011
        /// </remarks>
        public static void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception, bool moveArqErro)
        {
            GravarArqErroServico(arquivo, finalArqEnvio, finalArqErro, exception, ErroPadrao.ErroNaoDetectado, moveArqErro);
        }
        #endregion

        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erros ocorridos durante as operações para que o ERP possa tratá-los        
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de Erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="errorCode">Código do erro</param>
        /// <param name="moveArqErro">Move o arquivo informado no parametro "arquivo" para a pasta de XML com ERRO</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 02/06/2011
        /// </remarks>
        public static void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception, ErroPadrao erroPadrao, bool moveArqErro)
        {
            int emp = Functions.FindEmpresaByThread();

            //Qualquer erro ocorrido o aplicativo vai mover o XML com falha da pasta de envio
            //para a pasta de XML´s com erros. Futuramente ele é excluido quando outro igual
            //for gerado corretamente.
            if (moveArqErro)
                MoveArqErro(arquivo);

            //Grava arquivo de ERRO para o ERP
            string arqErro = Empresa.Configuracoes[emp].PastaXmlRetorno + "\\" +
                              Functions.ExtrairNomeArq(arquivo, finalArqEnvio) +
                              finalArqErro;

            string erroMessage = string.Empty;

            erroMessage += "ErrorCode|" + ((int)erroPadrao).ToString("0000000000");
            erroMessage += "\r\n";
            erroMessage += "Message|" + exception.Message;
            erroMessage += "\r\n";
            erroMessage += "StackTrace|" + exception.StackTrace;
            erroMessage += "\r\n";
            erroMessage += "Source|" + exception.Source;
            erroMessage += "\r\n";
            erroMessage += "Type|" + exception.GetType();
            erroMessage += "\r\n";
            erroMessage += "TargetSite|" + exception.TargetSite;
            erroMessage += "\r\n";
            erroMessage += "HashCode|" + exception.GetHashCode().ToString();

            if (exception.InnerException != null)
            {
                erroMessage += "\r\n";
                erroMessage += "\r\n";
                erroMessage += "InnerException 1";
                erroMessage += "\r\n";
                erroMessage += "Message|" + exception.InnerException.Message;
                erroMessage += "\r\n";
                erroMessage += "StackTrace|" + exception.InnerException.StackTrace;
                erroMessage += "\r\n";
                erroMessage += "TargetSite|" + exception.InnerException.TargetSite;
                erroMessage += "\r\n";
                erroMessage += "Source|" + exception.InnerException.Source;
                erroMessage += "\r\n";
                erroMessage += "HashCode|" + exception.InnerException.GetHashCode().ToString();

                if (exception.InnerException.InnerException != null)
                {
                    erroMessage += "\r\n";
                    erroMessage += "\r\n";
                    erroMessage += "InnerException 2";
                    erroMessage += "\r\n";
                    erroMessage += "Message|" + exception.InnerException.InnerException.Message;
                    erroMessage += "\r\n";
                    erroMessage += "StackTrace|" + exception.InnerException.InnerException.StackTrace;
                    erroMessage += "\r\n";
                    erroMessage += "TargetSite|" + exception.InnerException.InnerException.TargetSite;
                    erroMessage += "\r\n";
                    erroMessage += "Source|" + exception.InnerException.InnerException.Source;
                    erroMessage += "\r\n";
                    erroMessage += "HashCode|" + exception.InnerException.InnerException.GetHashCode().ToString();

                    if (exception.InnerException.InnerException.InnerException != null)
                    {
                        erroMessage += "\r\n";
                        erroMessage += "\r\n";
                        erroMessage += "InnerException 3";
                        erroMessage += "\r\n";
                        erroMessage += "Message|" + exception.InnerException.InnerException.InnerException.Message;
                        erroMessage += "\r\n";
                        erroMessage += "StackTrace|" + exception.InnerException.InnerException.InnerException.StackTrace;
                        erroMessage += "\r\n";
                        erroMessage += "TargetSite|" + exception.InnerException.InnerException.InnerException.TargetSite;
                        erroMessage += "\r\n";
                        erroMessage += "Source|" + exception.InnerException.InnerException.InnerException.Source;
                        erroMessage += "\r\n";
                        erroMessage += "HashCode|" + exception.InnerException.InnerException.InnerException.GetHashCode().ToString();
                    }
                }
            }

            try
            {
                // Gerar log do erro
                Auxiliar.WriteLog(erroMessage, true);
                //TODO: (Marcelo) Este tratamento de erro não poderia ser feito diretamente no método?
            }
            catch
            {
            }

            File.WriteAllText(arqErro, erroMessage, Encoding.UTF8);//.Default);

            ///
            /// grava o arquivo de erro no FTP
            new GerarXML(emp).XmlParaFTP(emp, arqErro);
        }
        #endregion

        #region MoveArqErro
        /// <summary>
        /// Move arquivos XML com erro para uma pasta de xml´s com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg)</example>
        public static void MoveArqErro(string Arquivo)
        {
            MoveArqErro(Arquivo, Path.GetExtension(Arquivo));
        }
        #endregion

        #region MoveArqErro()
        /// <summary>
        /// Move arquivos com a extensão informada e que está com erro para uma pasta de xml´s/arquivos com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <param name="ExtensaoArq">Extensão do arquivo que vai ser movido. Ex: .xml</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg, ".xml")</example>
        private static void MoveArqErro(string Arquivo, string ExtensaoArq)
        {
            int emp = Functions.FindEmpresaByThread();

            if (File.Exists(Arquivo))
            {
                FileInfo oArquivo = new FileInfo(Arquivo);

                if (Directory.Exists(Empresa.Configuracoes[emp].PastaXmlErro))
                {
                    string vNomeArquivo = Empresa.Configuracoes[emp].PastaXmlErro + "\\" + Functions.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;

                    Functions.Move(Arquivo, vNomeArquivo);

                    Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para "+vNomeArquivo, true);
                    //Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para a pasta de XML com problemas.", true);

                    /*
                    //Deletar o arquivo da pasta de XML com erro se o mesmo existir lá para evitar erros na hora de mover. Wandrey
                    if (File.Exists(vNomeArquivo))
                        this.DeletarArquivo(vNomeArquivo);

                    //Mover o arquivo da nota fiscal para a pasta do XML com erro
                    oArquivo.MoveTo(vNomeArquivo);
                    */
                }
                else
                {
                    //Antes estava deletando o arquivo, agora vou retornar uma mensagem de erro
                    //pois não podemos excluir, pode ser coisa importante. Wandrey 25/02/2011
                    throw new Exception("A pasta de XML´s com erro informada nas configurações não existe, por favor verifique.");
                    //oArquivo.Delete();
                }
            }
        }
        #endregion

        #region MoverArquivo()
        /// <summary>
        /// Move arquivos da nota fiscal eletrônica para suas respectivas pastas
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser movido</param>
        /// <param name="PastaXMLEnviado">Pasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="SubPastaXMLEnviado">SubPasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="PastaBackup">Pasta para Backup dos XML´s enviados</param>
        /// <param name="Emissao">Data de emissão da Nota Fiscal ou Data Atual do envio do XML para separação dos XML´s em subpastas por Ano e Mês</param>
        /// <date>16/07/2008</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void MoverArquivo(string arquivo, PastaEnviados subPastaXMLEnviado, DateTime emissao)
        {
            int emp = Functions.FindEmpresaByThread();

            #region Criar pastas que receberão os arquivos
            //Criar subpastas da pasta dos XML´s enviados
            Empresa.CriarSubPastaEnviado(emp);

            //Criar Pasta do Mês para gravar arquivos enviados autorizados ou denegados
            string nomePastaEnviado = string.Empty;
            string destinoArquivo = string.Empty;
            switch (subPastaXMLEnviado)
            {
                case PastaEnviados.EmProcessamento:
                    nomePastaEnviado = Empresa.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString();
                    destinoArquivo = nomePastaEnviado + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                    break;

                case PastaEnviados.Autorizados:
                    nomePastaEnviado = Empresa.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                    destinoArquivo = nomePastaEnviado + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                    goto default;

                case PastaEnviados.Denegados:
                    nomePastaEnviado = Empresa.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                    if (arquivo.ToLower().EndsWith(Propriedade.ExtRetorno.Den))//danasa 11-4-2012
                        destinoArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(arquivo));
                    else
                        destinoArquivo = Path.Combine(nomePastaEnviado, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.Den);
                    goto default;

                default:
                    if (!Directory.Exists(nomePastaEnviado))
                    {
                        System.IO.Directory.CreateDirectory(nomePastaEnviado);
                    }
                    break;
            }
            #endregion

            //Se conseguiu criar a pasta ele move o arquivo, caso contrário
            if (Directory.Exists(nomePastaEnviado))
            {
                #region Mover o XML para a pasta de XML´s enviados
                //Se for para mover para a Pasta EmProcessamento
                if (subPastaXMLEnviado == PastaEnviados.EmProcessamento)
                {
                    //Se já existir o arquivo na pasta EmProcessamento vamos mover 
                    //ele para a pasta com erro antes para evitar exceção. Wandrey 05/07/2011
                    if (File.Exists(destinoArquivo))
                    {
                        string destinoErro = Empresa.Configuracoes[emp].PastaXmlErro + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                        File.Move(destinoArquivo, destinoErro);

                        //danasa 11-4-2012
                        Auxiliar.WriteLog("Arquivo \"" + destinoArquivo + "\" movido para a pasta \"" + Empresa.Configuracoes[emp].PastaXmlErro + "\".", true);
                    }
                    File.Move(arquivo, destinoArquivo);
                }
                else
                {
                    //Se já existir o arquivo na pasta autorizados ou denegado, não vou mover o novo arquivo para lá, pois posso estar sobrepondo algum arquivo importante
                    //Sendo assim se o usuário quiser forçar mover, tem que deletar da pasta autorizados ou denegados manualmente, com isso evitamos perder um XML importante.
                    //Wandrey 05/07/2011
                    if (!File.Exists(destinoArquivo))
                    {
                        File.Move(arquivo, destinoArquivo);
                    }
                    else
                    {
                        string destinoErro = Empresa.Configuracoes[emp].PastaXmlErro + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                        File.Move(arquivo, destinoErro);

                        //danasa 11-4-2012
                        Auxiliar.WriteLog("Arquivo \"" + arquivo + "\" movido para a pasta \"" + Empresa.Configuracoes[emp].PastaXmlErro + "\".", true);
                    }
                }
                #endregion

                if (subPastaXMLEnviado == PastaEnviados.Autorizados || subPastaXMLEnviado == PastaEnviados.Denegados)
                {
                    #region Copiar XML para a pasta de BACKUP
                    //Fazer um backup do XML que foi copiado para a pasta de enviados
                    //para uma outra pasta para termos uma maior segurança no arquivamento
                    //Normalmente esta pasta é em um outro computador ou HD
                    if (Empresa.Configuracoes[emp].PastaBackup.Trim() != "")
                    {
                        //Criar Pasta do Mês para gravar arquivos enviados
                        string nomePastaBackup = string.Empty;
                        switch (subPastaXMLEnviado)
                        {
                            case PastaEnviados.Autorizados:
                                nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" + PastaEnviados.Autorizados + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                goto default;

                            case PastaEnviados.Denegados:
                                nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" + PastaEnviados.Denegados + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                goto default;

                            default:
                                if (!Directory.Exists(nomePastaBackup))
                                {
                                    System.IO.Directory.CreateDirectory(nomePastaBackup);
                                }
                                break;
                        }

                        //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                        if (Directory.Exists(nomePastaBackup))
                        {
                            //Mover o arquivo da nota fiscal para a pasta de backup
                            string destinoBackup = nomePastaBackup + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                            if (File.Exists(destinoBackup))
                            {
                                File.Delete(destinoBackup);
                            }
                            File.Copy(destinoArquivo, destinoBackup);
                        }
                        else
                        {
                            throw new Exception("Pasta de backup informada nas configurações não existe. (Pasta: " + nomePastaBackup + ")");
                        }
                    }
                    #endregion

                    #region Copiar o XML para a pasta do DanfeMon, se configurado para isso
                    CopiarXMLPastaDanfeMon(destinoArquivo);
                    #endregion

                    #region Copiar o XML para o FTP
                    GerarXML oGerarXML = new GerarXML(emp);
                    oGerarXML.XmlParaFTP(emp, destinoArquivo);
                    #endregion
                }
            }
            else
            {
                throw new Exception("Pasta para arquivamento dos XML´s enviados não existe. (Pasta: " + nomePastaEnviado + ")");
            }
        }
        #endregion

        #region MoverArquivo()
        /// <summary>
        /// Move arquivos da nota fiscal eletrônica para suas respectivas pastas
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser movido</param>
        /// <param name="PastaXMLEnviado">Pasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="SubPastaXMLEnviado">SubPasta de XML´s enviados para onde será movido o arquivo</param>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void MoverArquivo(string Arquivo, PastaEnviados SubPastaXMLEnviado)
        {
            MoverArquivo(Arquivo, SubPastaXMLEnviado, DateTime.Now);
        }
        #endregion

        #region CopiarXMLPastaDanfeMon()
        /// <summary>
        /// Copia o XML da NFe para a pasta monitorada pelo DANFEMon para que o mesmo imprima o DANFe.
        /// A copia só é efetuada de o UniNFe estiver configurado para isso.
        /// </summary>
        /// <param name="arquivoCopiar">Nome do arquivo com as pastas e subpastas a ser copiado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 20/04/2010
        /// </remarks>
        public static void CopiarXMLPastaDanfeMon(string arquivoCopiar)
        {
            int emp = Functions.FindEmpresaByThread();

            if (!string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaDanfeMon))
            {
                if (Directory.Exists(Empresa.Configuracoes[emp].PastaDanfeMon))
                {
                    if ((arquivoCopiar.ToLower().Contains("-nfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonNFe) ||
                        (arquivoCopiar.ToLower().Contains("-procnfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains("-den.xml") && Empresa.Configuracoes[emp].XMLDanfeMonDenegadaNFe) ||
                        (arquivoCopiar.ToLower().Contains("-cte.xml") && Empresa.Configuracoes[emp].XMLDanfeMonNFe) ||
                        (arquivoCopiar.ToLower().Contains("-proccte.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains("-mdfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonNFe) ||
                        (arquivoCopiar.ToLower().Contains("-procmdfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains("-proceventonfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains("-proceventocte.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains("-proceventomdfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe))
                    {
                        //Montar o nome do arquivo de destino
                        string arqDestino = Empresa.Configuracoes[emp].PastaDanfeMon + "\\" + Functions.ExtrairNomeArq(arquivoCopiar, ".xml") + ".xml";

                        //Copiar o arquivo para o destino
                        FileInfo oArquivo = new FileInfo(arquivoCopiar);
                        oArquivo.CopyTo(arqDestino, true);
                    }
                }
            }
        }
        #endregion

        #region ExecutaUniDanfe()

        #region RetornarConteudoEntre()
        /// <summary>
        /// Executa o aplicativo UniDanfe para gerar/imprimir o DANFE
        /// </summary>
        /// <param name="NomeArqXMLNFe">Nome do arquivo XML da NFe (final -nfe.xml)</param>
        /// <param name="DataEmissaoNFe">Data de emissão da NFe</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 03/02/2010
        /// </remarks>
        private static string RetornarConteudoEntre(string Conteudo, string Inicio, string Fim)
        {
            int i;
            i = Conteudo.IndexOf(Inicio);
            if (i == -1)
                return "";

            string s = Conteudo.Substring(i + Inicio.Length);
            i = s.IndexOf(Fim);
            if (i == -1)
                return "";
            return s.Substring(0, i);
        }
        #endregion

        #region ExcluirArqAuxiliar()
        private static void ExcluirArqAuxiliar(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;

            int passo = 0;
            System.Threading.Thread.Sleep(1000);
            while (!(sender as System.ComponentModel.BackgroundWorker).CancellationPending)
            {
                if (File.Exists((string)e.Argument))
                {
                    if (!Functions.FileInUse((string)e.Argument))
                    {
                        File.Delete((string)e.Argument);
                        e.Cancel = true;
                        break;
                    }
                }
                if (!e.Cancel)
                {
                    e.Cancel = (++passo > 10);
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
        #endregion

        public static void ExecutaUniDanfe_ForcaEmail(int emp)
        {
            if (Empresa.Configuracoes[emp].PastaExeUniDanfe != string.Empty &&
                File.Exists(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe"))
            {
                System.Diagnostics.Process.Start(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe", "envia_email=1");
            }
        }

        #region RenomearXmlReport()
        private static void RenomearXmlReport(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;

            string sx = (string)e.Argument;
            int emp = Convert.ToInt32(sx.Split('|')[0]);
            string fm = sx.Split('|')[1];

            string[] relname = new string[]{ 
                    Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\rel_email_enviar.xml",
                    Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\rel_email_enviados.xml", 
                    Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\rel_email_erros.xml" 
                };

            System.Threading.Thread.Sleep(1000);
            int passo = 0;            
            while (!(sender as System.ComponentModel.BackgroundWorker).CancellationPending)
            {
                foreach (var s in relname)
                {
                    if (File.Exists(s))
                    {
                        if (!Functions.FileInUse(s))
                        {
                            string _out = Path.Combine(Empresa.Configuracoes[emp].PastaXmlRetorno, fm.Replace(".txt",".xml"));
                            if (File.Exists(_out))
                                File.Delete(_out);
                            File.Move(s, _out);
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                e.Cancel = (++passo > 10);
                System.Threading.Thread.Sleep(100);
            }
        }
        #endregion

        #region ExecutaUniDanfe_ReportEmail
        public static void ExecutaUniDanfe_ReportEmail(int emp, DateTime datai, DateTime dataf, bool imprimir = false, string ExportarPasta = "Enviados", string filename="")
        {
            if (Empresa.Configuracoes[emp].PastaExeUniDanfe != string.Empty &&
                File.Exists(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe"))
            {
                System.Diagnostics.Process.Start(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe",
                    string.Format("rel_email=1 datai=\"{0:yyyy-MM-dd}\" dataf=\"{1:yyyy-MM-dd}\" imprimir={2} pasta=\"{3}\"", 
                                    datai, dataf, imprimir ? 1 : 0, ExportarPasta));

                if (!imprimir)
                {
                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ((sender, e) => ((System.ComponentModel.BackgroundWorker)sender).Dispose());
                    worker.DoWork += new System.ComponentModel.DoWorkEventHandler(RenomearXmlReport);
                    worker.RunWorkerAsync(emp + "|" + filename);
                }
            }
        }
        #endregion

        #region ExecutaUniDanfe()
        public static void ExecutaUniDanfe(string nomeArqXMLNFe, 
            DateTime dataEmissaoNFe, 
            NFe.Settings.Empresa emp, 
            string anexos = "", 
            string printer = "", 
            Int32 copias = 0, 
            string email = "")
        {
            //Disparar a geração/impressão do UniDanfe. 03/02/2010 - Wandrey
            if (!string.IsNullOrEmpty(emp.PastaExeUniDanfe) &&
                File.Exists(Path.Combine(emp.PastaExeUniDanfe, "unidanfe.exe")))
            {
                string nomePastaEnviado = string.Empty;
                string arqProcNFe = string.Empty;
                //string strArqNFe = string.Empty;
                string fExtensao = string.Empty;
                string fEmail = string.Empty;
                string fProtocolo = "";
                string tipo = "";
                bool denegada = false;
                bool temCancelamento = false;
                bool isEPEC = false;
                bool isDPEC = false;
                string tempFile = "";
                string fAuxiliar = "";

                if (!File.Exists(nomeArqXMLNFe))
                {
                    throw new Exception("Arquivo " + nomeArqXMLNFe + " não encontrado para impressão do DANFE/DACTE/CCe");
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(nomeArqXMLNFe);

                switch (doc.DocumentElement.Name)
                {
                    case "cteProc":
                        tipo = "cte";
                        ///
                        /// le o protocolo de autorizacao
                        /// 
                        foreach (var el3 in doc.GetElementsByTagName("protCTe"))
                        {
                            if (((XmlElement)el3).GetElementsByTagName(NFe.ConvertTxt.TpcnResources.cStat.ToString())[0] != null)
                            {
                                string cStat = ((XmlElement)el3).GetElementsByTagName(NFe.ConvertTxt.TpcnResources.cStat.ToString())[0].InnerText;
                                switch (cStat)
                                {
                                    //denegada
                                    case "110":
                                    case "301":
                                    case "302":
                                    case "303":
                                    case "304":
                                    case "305":
                                    case "306":
                                        denegada = true;
                                        break;
                                }
                                break;
                            }
                        }
                        break;

                    case "mdfeProc":
                        tipo = "mdfe";
                        break;

                    case "nfeProc":
                        arqProcNFe = nomeArqXMLNFe;
                        break;

                    case "procCancNFe": //cancelamento antigo
                        {
                            temCancelamento = true;
                            tipo = "nfe";
                            XmlElement cl = (XmlElement)doc.GetElementsByTagName("chNFe")[0];
                            if (cl != null)
                            {
                                tempFile = cl.InnerText;
                                arqProcNFe = cl.InnerText + Propriedade.ExtRetorno.ProcNFe;
                            }
                        }
                        break;

                    case "procEventoNFe":
                    case "procEventoCTe":
                    case "procEventoMDFe":
                        {
                            XmlElement cl = (XmlElement)doc.GetElementsByTagName("tpEvento")[0];
                            if (cl != null)
                            {
                                switch ((NFe.ConvertTxt.tpEventos)Convert.ToInt32(cl.InnerText))
                                {
                                    case ConvertTxt.tpEventos.tpEvCCe:
                                        switch (doc.DocumentElement.Name)
                                        {
                                            case "procEventoCTe":
                                                tipo = "ccte";
                                                cl = (XmlElement)doc.GetElementsByTagName("chCTe")[0];
                                                break;
                                            case "procEventoMDFe":
                                                ///
                                                /// nao existe CCe de MDFe, mas fica aqui por enquanto
                                                tipo = "ccemdfe";
                                                cl = null;
                                                break;
                                            default:
                                                tipo = "cce";
                                                cl = (XmlElement)doc.GetElementsByTagName("chNFe")[0];
                                                break;
                                        }
                                        break;

                                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                                        temCancelamento = true;
                                        switch (doc.DocumentElement.Name)
                                        {
                                            case "procEventoCTe":
                                                tipo = "cte";
                                                cl = (XmlElement)doc.GetElementsByTagName("chCTe")[0];
                                                break;
                                            case "procEventoMDFe":
                                                tipo = "canmdfe";
                                                cl = (XmlElement)doc.GetElementsByTagName("chMDFe")[0];
                                                break;
                                            default:
                                                tipo = "nfe";
                                                cl = (XmlElement)doc.GetElementsByTagName("chNFe")[0];
                                                break;
                                        }
                                        break;

                                    case ConvertTxt.tpEventos.tpEvEPEC:
                                        cl = null;
                                        isEPEC = true;
                                        break;

                                    default:
                                        ///
                                        /// tipo de evento desconhecido
                                        /// 
                                        throw new Exception("Arquivo de evento " + nomeArqXMLNFe + " desconhecido para impressão do DANFE/DACTE/CCe");
                                }

                                if (cl != null)
                                {
                                    ///
                                    /// le o nome do arquivo de distribuicao da NFe/CTe
                                    /// 
                                    switch (tipo)
                                    {
                                        case "nfe":
                                        case "cce":
                                            arqProcNFe = cl.InnerText + Propriedade.ExtRetorno.ProcNFe;
                                            break;
                                        case "cte":
                                        case "ccte":
                                            arqProcNFe = cl.InnerText + Propriedade.ExtRetorno.ProcCTe;
                                            break;
                                        case "canmdfe":
                                            arqProcNFe = cl.InnerText + Propriedade.ExtRetorno.ProcMDFe;
                                            break;
                                    }
                                }
                            }
                        }
                        break;

                    case "retDPEC":
                        isDPEC = true;
                        break;

                    default:
                        if (!nomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.Den) &&
                            !nomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.retDPEC_XML) &&
                            !nomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.retEPEC_XML))
                        {
                            ///
                            /// tipo de arquivo desconhecido
                            /// 
                            throw new Exception("Arquivo " + nomeArqXMLNFe + " desconhecido para impressão do DANFE/DACTE/CCe");
                        }
                        break;
                }
                if (!isDPEC)
                    isDPEC = nomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.retDPEC_XML);
                if (isDPEC)
                {
                    ///
                    /// pesquisa na arvore de enviados pelo arquivo da NFe/NFCe
                    /// 
                    string[] fTemp = Directory.GetFiles(emp.PastaXmlEnvio,
                                                        Path.GetFileName(Functions.ExtrairNomeArq(nomeArqXMLNFe, Propriedade.ExtRetorno.retDPEC_XML) + Propriedade.ExtEnvio.Nfe),
                                                        SearchOption.AllDirectories);
                    if (fTemp.Length == 0)
                        fTemp = Directory.GetFiles(emp.PastaXmlEnviado,
                                                        Path.GetFileName(Functions.ExtrairNomeArq(nomeArqXMLNFe, Propriedade.ExtRetorno.retDPEC_XML) + Propriedade.ExtEnvio.Nfe),
                                                        SearchOption.AllDirectories);
                    if (fTemp.Length > 0)
                    {
                        arqProcNFe = fTemp[0];
                    }
                    if (!File.Exists(arqProcNFe))
                    {
                        throw new Exception("Arquivo a NFe/NFCe" + Path.GetFileName(Functions.ExtrairNomeArq(nomeArqXMLNFe, Propriedade.ExtRetorno.retDPEC_XML) + Propriedade.ExtEnvio.Nfe) + " não encontrado para impressão do DANFE/DACTE");
                    }
                }

                if (nomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.Den))
                {
                    nomePastaEnviado = Path.GetDirectoryName(nomeArqXMLNFe);
                    arqProcNFe = nomeArqXMLNFe;
                }
                else
                {
                    nomePastaEnviado = emp.PastaXmlEnviado + "\\" +
                        PastaEnviados.Autorizados.ToString() + "\\" +
                        emp.DiretorioSalvarComo.ToString(dataEmissaoNFe);
                }

                if (arqProcNFe != string.Empty)
                {
                    if (Path.GetDirectoryName(arqProcNFe) == "")
                        ///
                        /// o nome pode ter sido atribuido pela leitura do evento, então não tem 'path'
                        /// 
                        arqProcNFe = Path.Combine(nomePastaEnviado, arqProcNFe);

                    if (!File.Exists(arqProcNFe))
                    {
                        ///
                        /// arquivo da NFe/NFce não encontrada no 'path' especifico, então pesquisamos na arvore de enviados
                        /// 
                        string[] fTemp = Directory.GetFiles(emp.PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString(), 
                                                            Path.GetFileName(arqProcNFe),
                                                            SearchOption.AllDirectories);
                        if (fTemp.Length == 0)
                            fTemp = Directory.GetFiles(emp.PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString(),
                                                        Path.GetFileName(arqProcNFe),
                                                        SearchOption.AllDirectories);
                        if (fTemp.Length > 0)
                        {
                            arqProcNFe = fTemp[0];
                        }
                    }

                    if (!File.Exists(arqProcNFe))
                    {
                        throw new Exception("Arquivo " + Path.GetFileName(arqProcNFe) + " não encontrado para impressão do DANFE: (" + tipo + ")");
                    }

                    if (tipo.Equals("nfe") || tipo.Equals("nfce") || tipo.Equals("cce") || tipo == "")
                    {
                        ///
                        /// le o xml da NFe/NFCe
                        /// 
                        var nfer = new NFe.ConvertTxt.nfeRead();
                        nfer.ReadFromXml(arqProcNFe);
                        fEmail = nfer.nfe.dest.email;
                        if (tipo == "")
                        {
                            tipo = (nfer.nfe.ide.mod == ConvertTxt.TpcnMod.modNFCe ? "nfce" : "nfe");
                        }
                        switch (nfer.nfe.protNFe.cStat)
                        {
                            case 110:
                            case 205:
                            case 301:
                            case 302:
                                denegada = true;
                                break;

                            default:
                                if (arqProcNFe.Equals(nomeArqXMLNFe))
                                    tempFile = nfer.nfe.infNFe.ID.Replace("NFe", "").Replace("NFCe", "");
                                break;
                        }
                    }

                    if (!temCancelamento && !denegada && tempFile != "")
                    {
                        ///
                        /// mandou imprimir pelo -procNFe, -procMDFe ou -procCTe, verifica se tem o xml de cancelamento
                        /// 
                        switch (tipo)
                        {
                            case "nfe":
                            case "nfce":
                                fExtensao = Propriedade.ExtRetorno.ProcEventoNFe;
                                break;
                            case "cte":
                                fExtensao = Propriedade.ExtRetorno.ProcEventoCTe;
                                break;
                            case "mdfe":
                                fExtensao = Propriedade.ExtRetorno.ProcEventoMDFe;
                                break;
                            default:
                                fExtensao = "";
                                break;
                        }
                        if (!string.IsNullOrEmpty(fExtensao))
                        {
                            fExtensao = tempFile + string.Format("_{0}_01{1}",
                                                        ((int)NFe.ConvertTxt.tpEventos.tpEvCancelamentoNFe).ToString(),
                                                        fExtensao);

                            string[] fTemp = Directory.GetFiles(emp.PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString(),
                                                                Path.GetFileName(fExtensao),
                                                                SearchOption.AllDirectories);
                            if (fTemp.Length == 0 && tipo.Equals("nfe"))
                            {
                                ///
                                /// ops, por evento não foi encontrado, procuramos pelo cancelamento antigo
                                /// 
                                fExtensao = tempFile + "-procCancNFe.xml";
                            }

                            if (!string.IsNullOrEmpty(fExtensao))
                            {
                                ///
                                /// pesquisa pelo xml de cancelamento
                                /// 
                                string[] fxTemp = Directory.GetFiles(emp.PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString(),
                                                                    Path.GetFileName(fExtensao),
                                                                    SearchOption.AllDirectories);
                                if (fxTemp.Length > 0)
                                {
                                    doc.Load(nomeArqXMLNFe = fxTemp[0]);
                                    temCancelamento = true;
                                }
                            }
                        }
                    }
                }
                ///
                /// DPEC, EPEC, CCe e Cancelamento por evento
                /// 
                string ctemp = doc.OuterXml;// File.ReadAllText(nomeArqXMLNFe);
                string dhReg = RetornarConteudoEntre(ctemp, "<dhRegDPEC>", "</dhRegDPEC>");
                if (dhReg == "")
                    dhReg = RetornarConteudoEntre(ctemp, "<dhRegEvento>", "</dhRegEvento>");
                DateTime dhRegEvento = Functions.GetDateTime(dhReg);

                if (dhRegEvento.Year > 1)
                {
                    if ((fProtocolo = RetornarConteudoEntre(ctemp, "<nRegDPEC>", "</nRegDPEC>")) == "")
                    {
                        if ((fProtocolo = RetornarConteudoEntre(ctemp, "</dhRegEvento><nProt>", "</nProt>")) == "")
                            fProtocolo = RetornarConteudoEntre(ctemp, "<nProt>", "</nProt>");
                    }
                    fProtocolo += "  " + dhRegEvento.ToString("dd/MM/yyyy HH:mm:ss");
                    if (dhReg.EndsWith("-01:00") ||
                        dhReg.EndsWith("-02:00") ||
                        dhReg.EndsWith("-03:00") ||
                        dhReg.EndsWith("-04:00"))
                    {
                        fProtocolo += dhReg.Substring(dhReg.Length - 6);
                    }
                }

                if (File.Exists(arqProcNFe) || File.Exists(nomeArqXMLNFe))
                {
                    string Args = "";

                    if (string.IsNullOrEmpty(fEmail))
                        fEmail = email;

                    if (!string.IsNullOrEmpty(fEmail))
                    {
                        Args += " EE=1";    //EnviarEmail
                        Args += " E=\"" + fEmail + "\"";
                        Args += " IEX=123";   //IgnorarEmailXML
                    }

                    if (!string.IsNullOrEmpty(emp.PastaConfigUniDanfe))
                    {
                        Args += " PC=\"" + emp.PastaConfigUniDanfe + "\"";
                    }

                    if (isEPEC)
                        Args += " P=2"; //numero de cópias
                    else
                        if (copias > 0)
                            Args += " P=" + copias.ToString();

                    if (!string.IsNullOrEmpty(printer))
                        Args += " I=\"" + printer + "\"";

                    string configDanfe = "";
                    if (isDPEC || isEPEC)
                    {
                        Args += " A=\"" + arqProcNFe + "\"";
                        Args += " AD=\"" + nomeArqXMLNFe + "\"";
                        Args += " T=danfe";
                        configDanfe = emp.ConfiguracaoDanfe;
                    }
                    else
                    {
                        switch (tipo)
                        {
                            case "nfe":
                            case "nfce":
                                Args += " A=\"" + arqProcNFe + "\"";
                                Args += " T=danfe";
                                configDanfe = emp.ConfiguracaoDanfe;
                                break;

                            case "mdfe":
                                Args += " A=\"" + nomeArqXMLNFe + "\"";
                                Args += " T=damdfe";
                                configDanfe = emp.ConfiguracaoDanfe;
                                break;

                            case "cte":
                                Args += " A=\"" + nomeArqXMLNFe + "\"";
                                Args += " T=dacte";
                                configDanfe = emp.ConfiguracaoDanfe;
                                break;

                            default:
                                if (File.Exists(arqProcNFe))
                                {
                                    switch (tipo)
                                    {
                                        case "cce":
                                        case "ccte":
                                            Args += " A=\"" + nomeArqXMLNFe + "\"";
                                            Args += " N=\"" + arqProcNFe + "\"";
                                            configDanfe = emp.ConfiguracaoCCe;
                                            break;
                                        case "canmdfe":
                                            Args += " A=\"" + nomeArqXMLNFe + "\"";
                                            Args += " N=\"" + arqProcNFe + "\"";
                                            tipo = "";
                                            break;
                                        default:
                                            Args += " A=\"" + arqProcNFe + "\"";
                                            break;
                                    }
                                }
                                else
                                {
                                    Args += " A=\"" + nomeArqXMLNFe + "\"";
                                }
                                if (!string.IsNullOrEmpty(tipo))
                                    Args += " T=" + tipo;
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(configDanfe))
                        Args += " C=\"" + configDanfe + "\"";

                    Args += " M=1"; //Imprimir
                    
                    if (temCancelamento)
                        Args += " CC=1"; //Cancelamento

                    if (!string.IsNullOrEmpty(anexos))
                    {
                        var an = 1;
                        foreach (var af in anexos.Split(new char[] { ';' }))
                        {
                            Args += " anexo" + an.ToString() + "=\"" + af.Replace("\"","") + "\"";
                            ++an;
                            if (an > 6) break;
                        }
                    }

                    ///
                    /// define o arquivo de saida de erros
                    /// 
                    if (File.Exists(arqProcNFe))
                        fAuxiliar = Path.GetFileName(arqProcNFe).Replace(".xml","");
                    else
                        fAuxiliar = Path.GetFileName(nomeArqXMLNFe).Replace(".xml", "");
                    fAuxiliar = (string)NFe.Components.Functions.OnlyNumbers(fAuxiliar, ".-");
                    fAuxiliar += "-danfe-erros.txt";
                    //saida erros para arquivo e nome do arquivo de erro
                    Args += " A=A AE=\"" + Path.Combine(emp.PastaXmlRetorno, fAuxiliar) + "\"";
                    fAuxiliar = "";

                    if (fProtocolo != "")
                    {
                        if (File.Exists(arqProcNFe))
                            fAuxiliar = Path.GetFileName(arqProcNFe);
                        else
                            fAuxiliar = Path.GetFileName(nomeArqXMLNFe);
                        fAuxiliar = Path.Combine(Path.GetTempPath(), "aux-" + fAuxiliar);

                        StringBuilder xmlAux = new StringBuilder();
                        xmlAux.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        xmlAux.Append("<outrasInfDANFe>");
                        xmlAux.AppendFormat("<protocolonfe>{0}</protocolonfe>", fProtocolo);
                        xmlAux.Append("</outrasInfDANFe>");

                        File.WriteAllText(fAuxiliar, xmlAux.ToString(), Encoding.UTF8);
                        Args += " AU=\"" + fAuxiliar + "\"";
                        ///
                        ///OBS: deveria existir um argumento para excluir o arquivo auxiliar, já que ele é temporario
                    }
                    System.Diagnostics.Process.Start(Path.Combine(emp.PastaExeUniDanfe, "unidanfe.exe"), Args);

                    if (fAuxiliar != "")
                    {
                        System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                        worker.WorkerSupportsCancellation = true;
                        worker.RunWorkerCompleted += ((sender, e) => ((System.ComponentModel.BackgroundWorker)sender).Dispose());
                        worker.DoWork += new System.ComponentModel.DoWorkEventHandler(ExcluirArqAuxiliar);
                        worker.RunWorkerAsync(fAuxiliar);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region RemoveSomenteLeitura()
        /// <summary>
        /// Metodo que remove atributo de Somente Leitura do Arquivo caso o mesmo estiver marcado, evitando problemas no acesso do arquivo.
        /// Renan - 26/11/13
        /// </summary>
        /// <param name="file">Arquivo a remover o atributo</param>
        public static void RemoveSomenteLeitura(string file)
        {
            FileAttributes attributes = File.GetAttributes(file);

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                // Show the file.
                attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
                File.SetAttributes(file, attributes);
            }
        }

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }
        #endregion

        #region Compacta XML
        public static void CompressXML(FileInfo fileToCompress)
        {
            using (FileStream fromZipFile = fileToCompress.OpenRead())
            {
                using (FileStream toZipFile = File.Create(fileToCompress.FullName + ".gz"))
                {
                    using (GZipStream zip = new GZipStream(toZipFile, CompressionMode.Compress))
                    {
                        fromZipFile.CopyTo(zip);

                        Console.WriteLine("Zipado {0} de {1} para {2} bytes.",
                            fileToCompress.Name, fileToCompress.Length.ToString(), toZipFile.Length.ToString());
                    }
                }
            }
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;
using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

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
            if(moveArqErro)
                MoveArqErro(arquivo);

            //Grava arquivo de ERRO para o ERP
            string arqErro = Empresa.Configuracoes[emp].PastaRetorno + "\\" +
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

            if(exception.InnerException != null)
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

                if(exception.InnerException.InnerException != null)
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

                    if(exception.InnerException.InnerException.InnerException != null)
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

            File.WriteAllText(arqErro, erroMessage, Encoding.Default);

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

            if(File.Exists(Arquivo))
            {
                FileInfo oArquivo = new FileInfo(Arquivo);

                if(Directory.Exists(Empresa.Configuracoes[emp].PastaErro))
                {
                    string vNomeArquivo = Empresa.Configuracoes[emp].PastaErro + "\\" + Functions.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;

                    Functions.Move(Arquivo, vNomeArquivo);

                    Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para a pasta de XML com problemas.", true);

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
            switch(subPastaXMLEnviado)
            {
                case PastaEnviados.EmProcessamento:
                    nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString();
                    destinoArquivo = nomePastaEnviado + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                    break;

                case PastaEnviados.Autorizados:
                    nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                    destinoArquivo = nomePastaEnviado + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                    goto default;

                case PastaEnviados.Denegados:
                    nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                    if(arquivo.ToLower().EndsWith("-den.xml"))//danasa 11-4-2012
                        destinoArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(arquivo));
                    else
                        destinoArquivo = Path.Combine(nomePastaEnviado, Functions.ExtrairNomeArq(arquivo, "-nfe.xml") + "-den.xml");
                    goto default;

                default:
                    if(!Directory.Exists(nomePastaEnviado))
                    {
                        System.IO.Directory.CreateDirectory(nomePastaEnviado);
                    }
                    break;
            }
            #endregion

            //Se conseguiu criar a pasta ele move o arquivo, caso contrário
            if(Directory.Exists(nomePastaEnviado))
            {
                #region Mover o XML para a pasta de XML´s enviados
                //Se for para mover para a Pasta EmProcessamento
                if(subPastaXMLEnviado == PastaEnviados.EmProcessamento)
                {
                    //Se já existir o arquivo na pasta EmProcessamento vamos mover 
                    //ele para a pasta com erro antes para evitar exceção. Wandrey 05/07/2011
                    if(File.Exists(destinoArquivo))
                    {
                        string destinoErro = Empresa.Configuracoes[emp].PastaErro + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                        File.Move(destinoArquivo, destinoErro);

                        //danasa 11-4-2012
                        Auxiliar.WriteLog("Arquivo \"" + destinoArquivo + "\" movido para a pasta \"" + Empresa.Configuracoes[emp].PastaErro + "\".", true);
                    }
                    File.Move(arquivo, destinoArquivo);
                }
                else
                {
                    //Se já existir o arquivo na pasta autorizados ou denegado, não vou mover o novo arquivo para lá, pois posso estar sobrepondo algum arquivo importante
                    //Sendo assim se o usuário quiser forçar mover, tem que deletar da pasta autorizados ou denegados manualmente, com isso evitamos perder um XML importante.
                    //Wandrey 05/07/2011
                    if(!File.Exists(destinoArquivo))
                    {
                        File.Move(arquivo, destinoArquivo);
                    }
                    else
                    {
                        string destinoErro = Empresa.Configuracoes[emp].PastaErro + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                        File.Move(arquivo, destinoErro);

                        //danasa 11-4-2012
                        Auxiliar.WriteLog("Arquivo \"" + arquivo + "\" movido para a pasta \"" + Empresa.Configuracoes[emp].PastaErro + "\".", true);
                    }
                }
                #endregion

                if(subPastaXMLEnviado == PastaEnviados.Autorizados || subPastaXMLEnviado == PastaEnviados.Denegados)
                {
                    #region Copiar XML para a pasta de BACKUP
                    //Fazer um backup do XML que foi copiado para a pasta de enviados
                    //para uma outra pasta para termos uma maior segurança no arquivamento
                    //Normalmente esta pasta é em um outro computador ou HD
                    if(Empresa.Configuracoes[emp].PastaBackup.Trim() != "")
                    {
                        //Criar Pasta do Mês para gravar arquivos enviados
                        string nomePastaBackup = string.Empty;
                        switch(subPastaXMLEnviado)
                        {
                            case PastaEnviados.Autorizados:
                                nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" + PastaEnviados.Autorizados + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                goto default;

                            case PastaEnviados.Denegados:
                                nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" + PastaEnviados.Denegados + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                goto default;

                            default:
                                if(!Directory.Exists(nomePastaBackup))
                                {
                                    System.IO.Directory.CreateDirectory(nomePastaBackup);
                                }
                                break;
                        }

                        //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                        if(Directory.Exists(nomePastaBackup))
                        {
                            //Mover o arquivo da nota fiscal para a pasta de backup
                            string destinoBackup = nomePastaBackup + "\\" + Functions.ExtrairNomeArq(arquivo, ".xml") + ".xml";
                            if(File.Exists(destinoBackup))
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

            if(!string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaDanfeMon))
            {
                if(Directory.Exists(Empresa.Configuracoes[emp].PastaDanfeMon))
                {
                    if((arquivoCopiar.ToLower().Contains("-nfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonNFe) ||
                        (arquivoCopiar.ToLower().Contains("-procnfe.xml") && Empresa.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains("-den.xml") && Empresa.Configuracoes[emp].XMLDanfeMonDenegadaNFe))
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
            if(i == -1)
                return "";

            string s = Conteudo.Substring(i + Inicio.Length);
            i = s.IndexOf(Fim);
            if(i == -1)
                return "";
            return s.Substring(0, i);
        }
        #endregion

        #region ExcluirArqAuxiliar()
        private static void ExcluirArqAuxiliar(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if(e.Cancel)
                return;

            System.Threading.Thread.Sleep(1000);
            while(!(sender as System.ComponentModel.BackgroundWorker).CancellationPending)
            {
                if(File.Exists((string)e.Argument))
                {
                    if(!Functions.FileInUse((string)e.Argument))
                    {
                        File.Delete((string)e.Argument);
                        e.Cancel = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region ExecutaUniDanfe()
        public static void ExecutaUniDanfe(string NomeArqXMLNFe, DateTime DataEmissaoNFe, string tipo)
        {
            int emp = Functions.FindEmpresaByThread();
            if(tipo == "")
            {
                for(int i = 0; i < Empresa.Configuracoes.Count; i++)
                {
                    Empresa empresa = Empresa.Configuracoes[i];
                    if(Path.GetDirectoryName(NomeArqXMLNFe).ToLower().StartsWith((empresa.PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString()).ToLower()))
                    {
                        emp = i;
                        break;
                    }
                }
            }

            //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
            if(Empresa.Configuracoes[emp].PastaExeUniDanfe != string.Empty &&
                File.Exists(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe"))
            {
                string strNomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(DataEmissaoNFe);
                string strArqProcNFe = Path.Combine(strNomePastaEnviado, Functions.ExtrairNomeArq(Functions.ExtrairNomeArq(NomeArqXMLNFe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe, ".xml") + ".xml");
                string strArqNFe = string.Empty;
                string fExtensao = string.Empty;
                string fEmail = string.Empty;
                string fProtocolo = "";

                if(tipo == "" && !File.Exists(strArqProcNFe) && File.Exists(NomeArqXMLNFe))
                {
                    tipo = "danfe";
                    strArqProcNFe = NomeArqXMLNFe;
                }

                if(NomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.retDPEC_XML))
                {
                    fExtensao = Propriedade.ExtRetorno.retDPEC_XML;
                    tipo = "danfe";
                    ///
                    ///le o protocolo do DPEC
                    string ctemp = File.ReadAllText(NomeArqXMLNFe);
                    DateTime dhRegEvento = Functions.GetDateTime(RetornarConteudoEntre(ctemp, "<dhRegDPEC>", "</dhRegDPEC>"));
                    fProtocolo = RetornarConteudoEntre(ctemp, "<nRegDPEC>", "</nRegDPEC>");
                    fProtocolo += "  " + dhRegEvento.ToString("dd/MM/yyyy HH:mm:ss");
                }
                else
                    if(NomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.ProcCancNFe))
                    {
                        fExtensao = Propriedade.ExtRetorno.ProcCancNFe;
                        tipo = "danfe";
                        ///
                        ///le o protocolo de cancelamento
                        string ctemp = File.ReadAllText(NomeArqXMLNFe);
                        DateTime dhRegEvento = Functions.GetDateTime(RetornarConteudoEntre(ctemp, "<dhRecbto>", "</dhRecbto>"));
                        fProtocolo = RetornarConteudoEntre(ctemp, "</dhRecbto><nProt>", "</nProt>");
                        fProtocolo += "  " + dhRegEvento.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                        if(NomeArqXMLNFe.EndsWith("_110111_01" + Propriedade.ExtRetorno.ProcEventoNFe)) //cancelamento por evento
                        {
                            fExtensao = "_110111_01" + Propriedade.ExtRetorno.ProcEventoNFe;
                            tipo = "danfe";
                            ///
                            ///le o protocolo de cancelamento
                            string ctemp = File.ReadAllText(NomeArqXMLNFe);
                            DateTime dhRegEvento = Functions.GetDateTime(RetornarConteudoEntre(ctemp, "<dhRegEvento>", "</dhRegEvento>"));
                            fProtocolo = RetornarConteudoEntre(ctemp, "</dhRegEvento><nProt>", "</nProt>");
                            fProtocolo += "  " + dhRegEvento.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        else
                            if(tipo.Equals("cce") || NomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.ProcEventoNFe))
                            {
                                tipo = "cce";
                                bool foundCCe = false;
                                for(int nSeq = 1; nSeq < 21; ++nSeq)
                                {
                                    fExtensao = "_" + nSeq.ToString("00") + Propriedade.ExtRetorno.ProcEventoNFe;
                                    if(NomeArqXMLNFe.EndsWith(fExtensao))
                                    {
                                        foundCCe = true;
                                        break;
                                    }
                                }
                                if(!foundCCe)
                                    fExtensao = string.Empty;
                            }
                            else
                                if(NomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.retEnvCCe_XML))
                                {
                                    fExtensao = Propriedade.ExtRetorno.retEnvCCe_XML;
                                    tipo = "cce";
                                }

                string fArgs = "";
                if(fExtensao != string.Empty)
                {
                    if(NomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.retDPEC_XML))
                        strArqNFe = Functions.ExtrairNomeArq(Functions.ExtrairNomeArq(NomeArqXMLNFe, fExtensao) + Propriedade.ExtEnvio.Nfe, ".xml") + ".xml";
                    else
                        strArqNFe = Functions.ExtrairNomeArq(Functions.ExtrairNomeArq(NomeArqXMLNFe, fExtensao) + Propriedade.ExtRetorno.ProcNFe, ".xml") + ".xml";

                    if(!string.IsNullOrEmpty(strArqNFe))
                    {
                        string strArqProc = Path.Combine(strNomePastaEnviado, strArqNFe);
                        if(!File.Exists(strArqProc))
                        {
                            string[] fTemp = Directory.GetFiles(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString(), strArqNFe, SearchOption.AllDirectories);
                            if(fTemp.Length > 0)
                                strArqProc = fTemp[0];
                        }
                        if(File.Exists(strArqProc))
                        {
                            NFe.ConvertTxt.nfeRead fread = new ConvertTxt.nfeRead();
                            fread.ReadFromXml(strArqProc);
                            fEmail = fread.nfe.dest.email;

                            if(NomeArqXMLNFe.EndsWith(Propriedade.ExtRetorno.retDPEC_XML))
                            {
                                fArgs = " AD=\"" + NomeArqXMLNFe + "\"";
                                fArgs += " A=\"" + strArqProc + "\"";
                            }
                            else
                                if(tipo.Equals("cce"))
                                {
                                    fArgs = " A=\"" + NomeArqXMLNFe + "\"";
                                    fArgs += " N=\"" + strArqProc + "\"";
                                }
                                else
                                {
                                    fArgs = " A=\"" + strArqProc + "\"";
                                    fArgs += " X1=\"" + NomeArqXMLNFe + "\"";
                                    fArgs += " CC=\"1\"";
                                }
                            if(!string.IsNullOrEmpty(fEmail))
                            {
                                fArgs += " EE=\"1\"";
                                fArgs += " E=\"" + fEmail + "\"";
                                fArgs += " IEX=\"1\"";
                            }
                            if(Empresa.Configuracoes[emp].PastaConfigUniDanfe != string.Empty)
                            {
                                fArgs += " PC=\"" + Empresa.Configuracoes[emp].PastaConfigUniDanfe + "\"";
                            }
                            fArgs += " T=\"" + tipo + "\"";
                            fArgs += " M=\"1\"";

                            string fAuxiliar = "";
                            if(fProtocolo != "")
                            {
                                fAuxiliar = Path.Combine(Path.GetTempPath(), Path.GetFileName(strArqProc.Replace("-procNFe", "-procNFedanfe")));

                                StringBuilder xmlAux = new StringBuilder();
                                xmlAux.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                xmlAux.Append("<outrasInfDANFe>");
                                xmlAux.AppendFormat("<protocolonfe>{0}</protocolonfe>", fProtocolo);
                                xmlAux.Append("</outrasInfDANFe>");

                                File.WriteAllText(fAuxiliar, xmlAux.ToString(), Encoding.UTF8);
                                fArgs += " AU=\"" + fAuxiliar + "\"";
                                ///
                                ///OBS: deveria existir um argumento para excluir o arquivo auxiliar, já que ele é temporario
                            }
                            System.Diagnostics.Process.Start(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe", fArgs);

                            if(fAuxiliar != "")
                            {
                                System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                                worker.WorkerSupportsCancellation = true;
                                worker.RunWorkerCompleted += ((sender, e) => ((System.ComponentModel.BackgroundWorker)sender).Dispose());
                                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(ExcluirArqAuxiliar);
                                worker.RunWorkerAsync(fAuxiliar);
                            }
                        }
                    }
                    return;
                }

                if(File.Exists(strArqProcNFe))
                {
                    string Args = "A=\"" + strArqProcNFe + "\"";
                    if(Empresa.Configuracoes[emp].PastaConfigUniDanfe != string.Empty)
                    {
                        Args += " PC=\"" + Empresa.Configuracoes[emp].PastaConfigUniDanfe + "\"";
                        //Args += " T=\"" + tipo + "\"";
                    }
                    Args += " T=\"" + tipo + "\"";

                    System.Diagnostics.Process.Start(Empresa.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe", Args);
                }
            }
        }
        #endregion

        #endregion
    }
}

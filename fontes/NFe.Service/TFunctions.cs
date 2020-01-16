using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NFe.Service
{
    public class TFunctions
    {
        #region GravarArqErroServico()

        /// <summary>
        /// Grava um arquivo texto com os erros ocorridos durante as operações para que o ERP possa tratá-los
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        public static void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception)
        {
            GravarArqErroServico(arquivo, finalArqEnvio, finalArqErro, exception, ErroPadrao.ErroNaoDetectado, true);
        }

        /// <summary>
        /// Grava um arquivo texto com os erros ocorridos durante as operações para que o ERP possa tratá-los
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="nomeArqRetorno">Nome do arquivo de retorno, caso não queira gravar um nome diferente do informado no parametro "arquivo"</param>
        public static void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception, string nomeArqRetorno)
        {
            GravarArqErroServico(arquivo, finalArqEnvio, finalArqErro, exception, ErroPadrao.ErroNaoDetectado, true, nomeArqRetorno);
        }

        #endregion GravarArqErroServico()

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

        #endregion GravarArqErroServico()

        #region GravarArqErroServico()

        /// <summary>
        /// Grava um arquivo texto com um erros ocorridos durante as operações para que o ERP possa tratá-los
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que está sendo processado</param>
        /// <param name="finalArqEnvio">string final do nome do arquivo que é para ser substituida na gravação do arquivo de Erro</param>
        /// <param name="finalArqErro">string final do nome do arquivo que é para ser utilizado no nome do arquivo de erro</param>
        /// <param name="exception">Exception gerada</param>
        /// <param name="moveArqErro">Move o arquivo informado no parametro "arquivo" para a pasta de XML com ERRO</param>
        /// <param name="nomeArqRetorno">Nome do arquivo de retorno, caso não queira gravar um nome diferente do informado no parametro "arquivo"</param>
        /// <param name="erroPadrao">Informe o erro padrão do UniNFe</param>
        public static void GravarArqErroServico(string arquivo, string finalArqEnvio, string finalArqErro, Exception exception, ErroPadrao erroPadrao, bool moveArqErro, string nomeArqRetorno = "")
        {
            int emp = Empresas.FindEmpresaByThread();

            //Qualquer erro ocorrido o aplicativo vai mover o XML com falha da pasta de envio
            //para a pasta de XML´s com erros. Futuramente ele é excluido quando outro igual
            //for gerado corretamente.
            if (moveArqErro)
                MoveArqErro(arquivo);

            //Grava arquivo de ERRO para o ERP
            string pastaRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno;
            FileInfo fi = new FileInfo(arquivo);
            if (fi.Directory.FullName.ToLower().EndsWith("geral\\temp"))
                pastaRetorno = Propriedade.PastaGeralRetorno;

            string arqErro = pastaRetorno + "\\" + Functions.ExtrairNomeArq((string.IsNullOrEmpty(nomeArqRetorno) ? arquivo : nomeArqRetorno), finalArqEnvio) + finalArqErro;

            string erroMessage = MontaStringErro(exception, erroPadrao);

            try
            {
                // Gerar log do erro
                Auxiliar.WriteLog(erroMessage, true);
            }
            catch
            {
            }

            File.WriteAllText(arqErro, erroMessage);

            // grava o arquivo de erro no FTP
            new GerarXML(emp).XmlParaFTP(emp, arqErro);
        }

        #region MontaStringErro()

        /// <summary>
        /// Montar a string do erro da exception
        /// </summary>
        /// <param name="exception">Objeto da exception</param>
        /// <param name="erroPadrao">ErroPadrao</param>
        /// <returns>Retorna uma string com o erro ocorrido.</returns>
        private static string MontaStringErro(Exception exception, ErroPadrao erroPadrao)
        {
            string erroMessage = string.Empty;

            erroMessage += "Versão|" + Propriedade.Versao + "\r\n" +
                "ErrorCode|" + ((int)erroPadrao).ToString("0000000000") +
                "\r\n" +
                "Message|" + exception.Message +
                "\r\n" +
                "StackTrace|" + exception.StackTrace +
                "\r\n" +
                "Source|" + exception.Source +
                "\r\n" +
                "Type|" + exception.GetType() +
                "\r\n" +
                "TargetSite|" + exception.TargetSite +
                "\r\n" +
                "HashCode|" + exception.GetHashCode().ToString();

            if (exception.InnerException != null)
                erroMessage += "\r\n\r\n" + MontaStringErro(exception.InnerException, erroPadrao);

            return erroMessage;
        }

        #endregion MontaStringErro()

        #endregion GravarArqErroServico()

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

        #endregion MoveArqErro

        #region MoveArqErro()

        /// <summary>
        /// Move arquivos com a extensão informada e que está com erro para uma pasta de xml´s/arquivos com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <param name="ExtensaoArq">Extensão do arquivo que vai ser movido. Ex: .xml</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg, ".xml")</example>
        private static void MoveArqErro(string Arquivo, string ExtensaoArq)
        {
            int emp = Empresas.FindEmpresaByThread();

            if (File.Exists(Arquivo))
            {
                FileInfo oArquivo = new FileInfo(Arquivo);

                if (!string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlErro) && Directory.Exists(Empresas.Configuracoes[emp].PastaXmlErro))
                {
                    string vNomeArquivo = Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Functions.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;

                    Functions.Move(Arquivo, vNomeArquivo);

                    Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para " + vNomeArquivo, true);
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

        #endregion MoveArqErro()

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
        public static void MoverArquivo(string arquivo, PastaEnviados subPastaXMLEnviado, DateTime emissao, string nomeArquivoDestino)
        {
            int emp = Empresas.FindEmpresaByThread();

            #region Criar pastas que receberão os arquivos

            Empresas.Configuracoes[emp].CriarSubPastaEnviado();

            //Criar Pasta do Mês para gravar arquivos enviados autorizados ou denegados
            string nomePastaEnviado = string.Empty;
            string destinoArquivo = string.Empty;
            switch (subPastaXMLEnviado)
            {
                case PastaEnviados.EmProcessamento:
                    nomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString();
                    destinoArquivo = nomePastaEnviado + "\\" + (String.IsNullOrEmpty(nomeArquivoDestino) ? Path.GetFileName(arquivo) : nomeArquivoDestino);
                    break;

                case PastaEnviados.Autorizados:
                    nomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                       PastaEnviados.Autorizados.ToString() + "\\" +
                                       Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                    destinoArquivo = nomePastaEnviado + Path.GetFileName(arquivo);
                    goto default;

                case PastaEnviados.Denegados:
                    nomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                       PastaEnviados.Denegados.ToString() + "\\" +
                                       Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);

                    if (arquivo.ToLower().EndsWith(Propriedade.ExtRetorno.Den))
                        destinoArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(arquivo));
                    else
                        destinoArquivo = Path.Combine(nomePastaEnviado, Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML) + Propriedade.ExtRetorno.Den);
                    goto default;

                case PastaEnviados.Originais:
                    nomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                       PastaEnviados.Originais.ToString() + "\\" +
                                       Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);

                    destinoArquivo = nomePastaEnviado + Path.GetFileName(arquivo);
                    goto default;

                default:
                    if (!Directory.Exists(nomePastaEnviado))
                    {
                        Directory.CreateDirectory(nomePastaEnviado);
                    }
                    break;
            }

            #endregion Criar pastas que receberão os arquivos

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
                        string destinoErro = Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Path.GetFileName(arquivo);
                        File.Move(destinoArquivo, destinoErro);

                        //danasa 11-4-2012
                        Auxiliar.WriteLog("Arquivo \"" + destinoArquivo + "\" movido para a pasta \"" + Empresas.Configuracoes[emp].PastaXmlErro + "\".", true);
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
                        string destinoErro = Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Path.GetFileName(arquivo);
                        File.Move(arquivo, destinoErro);

                        //danasa 11-4-2012
                        Auxiliar.WriteLog("Arquivo \"" + arquivo + "\" movido para a pasta \"" + Empresas.Configuracoes[emp].PastaXmlErro + "\".", true);
                    }
                }

                #endregion Mover o XML para a pasta de XML´s enviados

                if (subPastaXMLEnviado == PastaEnviados.Autorizados || subPastaXMLEnviado == PastaEnviados.Denegados)
                {
                    #region Copiar XML para a pasta de BACKUP

                    //Fazer um backup do XML que foi copiado para a pasta de enviados
                    //para uma outra pasta para termos uma maior segurança no arquivamento
                    //Normalmente esta pasta é em um outro computador ou HD
                    if (Empresas.Configuracoes[emp].PastaBackup.Trim() != "")
                    {
                        //Criar Pasta do Mês para gravar arquivos enviados
                        string nomePastaBackup = string.Empty;
                        switch (subPastaXMLEnviado)
                        {
                            case PastaEnviados.Autorizados:
                                nomePastaBackup = Empresas.Configuracoes[emp].PastaBackup + "\\" +
                                    PastaEnviados.Autorizados.ToString() + "\\" +
                                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                goto default;

                            case PastaEnviados.Denegados:
                                nomePastaBackup = Empresas.Configuracoes[emp].PastaBackup + "\\" +
                                    PastaEnviados.Denegados.ToString() + "\\" +
                                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                goto default;

                            case PastaEnviados.Originais:
                                nomePastaBackup = Empresas.Configuracoes[emp].PastaBackup + "\\" +
                                    PastaEnviados.Originais.ToString() + "\\" +
                                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
                                goto default;

                            default:
                                if (!Directory.Exists(nomePastaBackup))
                                {
                                    Directory.CreateDirectory(nomePastaBackup);
                                }
                                break;
                        }

                        //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                        if (Directory.Exists(nomePastaBackup))
                        {
                            //Mover o arquivo da nota fiscal para a pasta de backup
                            string destinoBackup = nomePastaBackup + Path.GetFileName(arquivo);
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

                    #endregion Copiar XML para a pasta de BACKUP

                    #region Copiar o XML para a pasta do DanfeMon, se configurado para isso

                    CopiarXMLPastaDanfeMon(destinoArquivo);

                    #endregion Copiar o XML para a pasta do DanfeMon, se configurado para isso

                    #region Copiar o XML para o FTP

                    new GerarXML(emp).XmlParaFTP(emp, destinoArquivo);

                    #endregion Copiar o XML para o FTP
                }
            }
            else
            {
                throw new Exception("Pasta para arquivamento dos XML´s enviados não existe. (Pasta: " + nomePastaEnviado + ")");
            }
        }

        #endregion MoverArquivo()

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
            MoverArquivo(Arquivo, SubPastaXMLEnviado, DateTime.Now, "");
        }

        /// <summary>
        /// Move arquivos da nota fiscal eletrônica para suas respectivas pastas
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser movido</param>
        /// <param name="PastaXMLEnviado">Pasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="SubPastaXMLEnviado">SubPasta de XML´s enviados para onde será movido o arquivo</param>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void MoverArquivo(string Arquivo, PastaEnviados SubPastaXMLEnviado, string nomeArquivoDestino)
        {
            MoverArquivo(Arquivo, SubPastaXMLEnviado, DateTime.Now, nomeArquivoDestino);
        }

        /// <summary>
        /// Move arquivos da nota fiscal eletrônica para suas respectivas pastas
        /// </summary>
        /// <param name="arquivo">Nome do arquivo a ser movido</param>
        /// <param name="subPastaXMLEnviado">Pasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="emissao">Data de emissão da Nota Fiscal ou Data Atual do envio do XML para separação dos XML´s em subpastas por Ano e Mês</param>
        public static void MoverArquivo(string arquivo, PastaEnviados subPastaXMLEnviado, DateTime emissao)
        {
            MoverArquivo(arquivo, subPastaXMLEnviado, emissao, "");
        }

        #endregion MoverArquivo()

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
            int emp = Empresas.FindEmpresaByThread();

            if (!string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaDanfeMon))
            {
                if (Directory.Exists(Empresas.Configuracoes[emp].PastaDanfeMon))
                {
                    if ((arquivoCopiar.ToLower().Contains(Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.ExtRetorno.ProcNFe.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.ExtRetorno.Den.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonDenegadaNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.ExtRetorno.ProcCTe.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.ExtRetorno.ProcMDFe.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.ExtRetorno.ProcEventoNFe.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.ExtRetorno.ProcEventoCTe.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonProcNFe) ||
                        (arquivoCopiar.ToLower().Contains(Propriedade.ExtRetorno.ProcEventoMDFe.ToLower()) && Empresas.Configuracoes[emp].XMLDanfeMonProcNFe))
                    {
                        //Montar o nome do arquivo de destino
                        string arqDestino = Empresas.Configuracoes[emp].PastaDanfeMon + "\\" + Path.GetFileName(arquivoCopiar);

                        //Copiar o arquivo para o destino
                        File.Copy(arquivoCopiar, arqDestino, true);
                    }
                }
            }
        }

        #endregion CopiarXMLPastaDanfeMon()

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

        #endregion RetornarConteudoEntre()

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

        #endregion ExcluirArqAuxiliar()

        #region ExecutaUniDanfe_ForcaEmail

        public static void ExecutaUniDanfe_ForcaEmail(int emp)
        {
            if (Empresas.Configuracoes[emp].PastaExeUniDanfe != string.Empty &&
                File.Exists(Empresas.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe"))
            {
                System.Diagnostics.Process.Start(Empresas.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe", "envia_email=1");
            }
        }

        #endregion ExecutaUniDanfe_ForcaEmail

        #region RenomearXmlReport()

        private static void RenomearXmlReport(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;

            string sx = (string)e.Argument;
            int emp = Convert.ToInt32(sx.Split('|')[0]);
            string fm = sx.Split('|')[1];

            string[] relname = new string[]{
                    Empresas.Configuracoes[emp].PastaExeUniDanfe + "\\rel_email_enviar.xml",
                    Empresas.Configuracoes[emp].PastaExeUniDanfe + "\\rel_email_enviados.xml",
                    Empresas.Configuracoes[emp].PastaExeUniDanfe + "\\rel_email_erros.xml"
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
                            string _out = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, fm.Replace(".txt", ".xml"));
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

        #endregion RenomearXmlReport()

        #region ExecutaUniDanfe_ReportEmail

        public static void ExecutaUniDanfe_ReportEmail(int emp, DateTime datai, DateTime dataf, bool imprimir = false, string ExportarPasta = "Enviados", string filename = "")
        {
            if (Empresas.Configuracoes[emp].PastaExeUniDanfe != string.Empty &&
                File.Exists(Empresas.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe"))
            {
                System.Diagnostics.Process.Start(Empresas.Configuracoes[emp].PastaExeUniDanfe + "\\unidanfe.exe",
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

        #endregion ExecutaUniDanfe_ReportEmail

        #region ExecutaUniDanfe()

        public static string getSubFolder(DateTime value, int ndias, DiretorioSalvarComo salvarComo)
        {
            if (salvarComo.ToString().Contains("D"))
                return salvarComo.ToString(value.AddDays(ndias * -1));

            if (salvarComo.ToString().Contains("M"))
                return salvarComo.ToString(value.AddMonths(ndias * -1));

            return "";
        }

        public static void ExecutaUniDanfe(string nomeArquivoRecebido, DateTime dataEmissaoNFe, NFe.Settings.Empresa emp, Dictionary<string, string> args = null)
        {
#if DEBUG
            Auxiliar.WriteLog("ExecutaUniDanfe: Preparando a execução do UniDANFe p/ o arquivo: \"" + nomeArquivoRecebido + "\"", false);
#endif
            const string erroMsg = "Arquivo {0} não encontrado para impressão do DANFE/DACTE/CCe/DAMDFe {1}";

            //Disparar a geração/impressão do UniDanfe. 03/02/2010 - Wandrey
            if (!string.IsNullOrEmpty(emp.PastaExeUniDanfe) &&
                File.Exists(Path.Combine(emp.PastaExeUniDanfe, "unidanfe.exe")))
            {
                Auxiliar.WriteLog("ExecutaUniDanfe: Preparando a execução do UniDANFe.", false);

                string nomePastaEnviado = string.Empty;
                string arqProcNFe = string.Empty;
                string fExtensao = string.Empty;
                string fEmail = "";
                string fProtocolo = "";
                string tipo = "";
                string tempFile = "";
                string fAuxiliar = "";
                string epecTipo = "";
                bool denegada = false;
                bool temCancelamento = false;
                bool isEPEC = false;
                bool cancelamentoNfe = false;

                if (!File.Exists(nomeArquivoRecebido))
                    throw new Exception(string.Format(erroMsg, nomeArquivoRecebido, ""));

                XmlDocument doc = new XmlDocument();
                doc.Load(nomeArquivoRecebido);

                switch (doc.DocumentElement.Name)
                {
                    case "nfeProc":
                        arqProcNFe = nomeArquivoRecebido;
                        break;

                    case "NFe":
                        foreach (var el3 in doc.GetElementsByTagName("ide"))
                        {
                            if (((XmlElement)el3).GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0] != null)
                            {
                                tipo = ((XmlElement)el3).GetElementsByTagName(NFe.Components.TpcnResources.mod.ToString())[0].InnerText.Equals("55") ? "nfe" : "nfce";
                            }
                        }
                        arqProcNFe = nomeArquivoRecebido;
                        break;

                    case "cteOSProc":
                    case "CTeOS":
                        tipo = "cteos";
                        arqProcNFe = nomeArquivoRecebido;

                        ///
                        /// le o protocolo de autorizacao
                        ///
                        if (doc.DocumentElement.Name.Equals("cteOSProc"))
                        {
                            foreach (var el3 in doc.GetElementsByTagName("protCTe"))
                            {
                                if (((XmlElement)el3).GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0] != null)
                                {
                                    string cStat = ((XmlElement)el3).GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0].InnerText;
                                    switch (cStat)
                                    {
                                        //denegada
                                        case "110":
                                        case "301":
                                            denegada = true;
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                        break;

                    case "cteProc":
                    case "CTe":
                        tipo = "cte";
                        arqProcNFe = nomeArquivoRecebido;

                        ///
                        /// le o protocolo de autorizacao
                        ///
                        if (doc.DocumentElement.Name.Equals("cteProc"))
                        {
                            foreach (var el3 in doc.GetElementsByTagName("protCTe"))
                            {
                                if (((XmlElement)el3).GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0] != null)
                                {
                                    string cStat = ((XmlElement)el3).GetElementsByTagName(NFe.Components.TpcnResources.cStat.ToString())[0].InnerText;
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
                        }
                        break;

                    case "mdfeProc":
                    case "MDFe":
                        tipo = "mdfe";
                        arqProcNFe = nomeArquivoRecebido;
                        break;

                    case "procCancNFe": //cancelamento antigo
                        {
                            temCancelamento = true;
                            tipo = "nfe";
                            XmlElement cl = (XmlElement)doc.GetElementsByTagName(NFe.Components.TpcnResources.chNFe.ToString())[0];
                            if (cl != null)
                            {
                                tempFile = cl.InnerText;
                                arqProcNFe = cl.InnerText + Propriedade.ExtRetorno.ProcNFe;
                            }
                        }
                        break;

                    case "CFe":
                    case "CFeCanc":
                        tipo = "cfe";
                        arqProcNFe = nomeArquivoRecebido;
                        break;

                    case "procEventoNFe":
                    case "procEventoCTe":
                    case "procEventoMDFe":
                        {
                            XmlElement cl = (XmlElement)doc.GetElementsByTagName(NFe.Components.TpcnResources.tpEvento.ToString())[0];
                            if (cl != null)
                            {
                                switch ((NFe.ConvertTxt.tpEventos)Convert.ToInt32(cl.InnerText))
                                {
                                    case ConvertTxt.tpEventos.tpEvCCe:
                                        switch (doc.DocumentElement.Name)
                                        {
                                            case "procEventoCTe":
                                                tipo = "ccte";
                                                cl = (XmlElement)doc.GetElementsByTagName(TpcnResources.chCTe.ToString())[0];
                                                break;

                                            case "procEventoMDFe":

                                                ///
                                                /// nao existe CCe de MDFe, mas fica aqui por enquanto
                                                tipo = "ccemdfe";
                                                cl = null;
                                                break;

                                            default:
                                                tipo = "cce";
                                                cl = (XmlElement)doc.GetElementsByTagName(NFe.Components.TpcnResources.chNFe.ToString())[0];
                                                break;
                                        }
                                        break;

                                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                                    case ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe:
                                        temCancelamento = true;
                                        switch (doc.DocumentElement.Name)
                                        {
                                            case "procEventoCTe":
                                                tipo = "cte";
                                                cl = (XmlElement)doc.GetElementsByTagName(TpcnResources.chCTe.ToString())[0];
                                                break;

                                            case "procEventoMDFe":
                                                tipo = "canmdfe";
                                                cl = (XmlElement)doc.GetElementsByTagName(TpcnResources.chMDFe.ToString())[0];
                                                break;

                                            default:
                                                tipo = "nfe";
                                                cancelamentoNfe = true;
                                                cl = (XmlElement)doc.GetElementsByTagName(TpcnResources.chNFe.ToString())[0];
                                                break;
                                        }
                                        break;

                                    case ConvertTxt.tpEventos.tpEvEPEC:
                                        cl = null;
                                        isEPEC = true;
                                        epecTipo = doc.DocumentElement.Name;
                                        break;

                                    default:

                                        ///
                                        /// tipo de evento desconhecido
                                        ///
                                        throw new Exception("Arquivo de evento " + nomeArquivoRecebido + " desconhecido para impressão do DANFE/DACTE/CCe/DAMDFe");
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
                                        case "cteos":
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

                    default:
                        if (!nomeArquivoRecebido.EndsWith(Propriedade.ExtRetorno.Den))
                        {
                            ///
                            /// tipo de arquivo desconhecido
                            ///
                            throw new Exception("Arquivo " + nomeArquivoRecebido + " desconhecido para impressão do DANFE/DACTE/CCe/DAMDFe");
                        }
                        break;
                }

                if (isEPEC)
                {
                    switch (epecTipo)
                    {
                        case "procEventoCTe":
                            fExtensao = Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML;
                            break;

                        case "procEventoMDFe":
                            fExtensao = Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML;
                            break;

                        default:    //pode ser NFe
                            fExtensao = Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML;
                            break;
                    }
                    string xTemp = Path.GetFileName(Functions.ExtrairNomeArq(nomeArquivoRecebido, Propriedade.ExtRetorno.ProcEventoNFe)) + fExtensao;

                    xTemp = xTemp.Replace("_" + ((int)ConvertTxt.tpEventos.tpEvEPEC).ToString() + "_01", "");

                    ///
                    /// pesquisa pelo arquivo da NFe/NFCe/MDFe/CTe
                    ///
                    if (File.Exists(Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), xTemp)))
                        arqProcNFe = Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), xTemp);
                    else
                    {
                        string[] fTemp = Directory.GetFiles(emp.PastaXmlEnvio, xTemp, SearchOption.AllDirectories);
                        if (fTemp.Length == 0)
                        {
                            fTemp = Directory.GetFiles(emp.PastaXmlEnviado, xTemp, SearchOption.AllDirectories);
                            if (fTemp.Length == 0)
                            {
                                if (emp.tpEmis != (int)NFe.Components.TipoEmissao.teNormal)
                                {
                                    fTemp = Directory.GetFiles(emp.PastaContingencia,
                                                               Path.GetFileName(Functions.ExtrairNomeArq(nomeArquivoRecebido, Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML) + fExtensao),
                                                               SearchOption.AllDirectories);
                                    if (fTemp.Length == 0)
                                    {
                                        fTemp = Directory.GetFiles(emp.PastaValidado, xTemp, SearchOption.TopDirectoryOnly);
                                        if (fTemp.Length == 0)
                                            fTemp = Directory.GetFiles(emp.PastaContingencia, xTemp, SearchOption.TopDirectoryOnly);
                                    }
                                }
                                if (fTemp.Length == 0)
                                {
                                    ///
                                    /// OPS!!! EPEC <-> denegado?
                                    ///
                                    xTemp = Functions.ExtrairNomeArq(xTemp, fExtensao) + Propriedade.ExtRetorno.Den;
                                    if (File.Exists(Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), xTemp)))
                                        arqProcNFe = Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), xTemp);
                                    else
                                        fTemp = Directory.GetFiles(emp.PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString(), xTemp, SearchOption.AllDirectories);
                                }
                            }
                        }

                        if (fTemp.Length > 0)
                            arqProcNFe = fTemp[0];
                    }
                    if (string.IsNullOrEmpty(arqProcNFe) || !File.Exists(arqProcNFe))
                        throw new Exception(string.Format(erroMsg, xTemp, ""));
                }

                if (nomeArquivoRecebido.EndsWith(Propriedade.ExtRetorno.Den))
                {
                    nomePastaEnviado = Path.GetDirectoryName(nomeArquivoRecebido);
                    arqProcNFe = nomeArquivoRecebido;
                }
                else
                {
                    nomePastaEnviado = emp.PastaXmlEnviado + "\\" +
                        PastaEnviados.Autorizados.ToString() + "\\" +
                        emp.DiretorioSalvarComo.ToString(dataEmissaoNFe);
                }

                if (arqProcNFe != string.Empty)
                {
                    if (File.Exists(Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), Path.GetFileName(arqProcNFe))))

                        ///
                        /// em sistemas que o XML é gravado no DB, ele pode nao precisar deixar gravado o XML nas pastas de autorizados/denegados
                        /// então eles podem extrair os conteudos do DB e gravá-los em uma pasta qualquer
                        ///
                        arqProcNFe = Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), Path.GetFileName(arqProcNFe));
                    else

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
                        int ndias = 0;
                        while (ndias < 60)
                        {
                            ///
                            /// usamos o 'DiretorioSalvarComo' para pesquisar pelo arquivo numa pasta baseando-se pela
                            /// dataEmissaoNFe.AddDays(-ndias)
                            ///
                            string fTemp = Path.Combine(emp.PastaXmlEnviado +
                                                            "\\" + PastaEnviados.Autorizados.ToString() +
                                                            "\\" + getSubFolder(dataEmissaoNFe, ndias, emp.DiretorioSalvarComo), //.ToString(dataEmissaoNFe.AddDays(ndias * -1)),
                                                        Path.GetFileName(arqProcNFe));
                            if (!File.Exists(fTemp))
                                fTemp = Path.Combine(emp.PastaXmlEnviado +
                                                        "\\" + PastaEnviados.Denegados.ToString() +
                                                        "\\" + getSubFolder(dataEmissaoNFe, ndias, emp.DiretorioSalvarComo), //emp.DiretorioSalvarComo.ToString(dataEmissaoNFe.AddDays(ndias * -1)),
                                                     Path.GetFileName(arqProcNFe));
                            ++ndias;
                            if (File.Exists(fTemp))
                            {
                                arqProcNFe = fTemp;
                                break;
                            }
                            if (emp.DiretorioSalvarComo.ToString().Equals("Raiz") || emp.DiretorioSalvarComo.ToString().Equals(""))

                                ///
                                /// ops!
                                /// Se definido como 'Raiz' e nao encontrou, nao precisamos mais pesquisar pelo arquivo em
                                /// pastas baseado na data de emissao da nota
                                ///
                                break;
                        }
                    }

                    if (!File.Exists(arqProcNFe))
                        throw new Exception(string.Format(erroMsg, Path.GetFileName(arqProcNFe), ": (" + tipo + ")"));

                    if (tipo.Equals("nfe") || tipo.Equals("nfce") || tipo.Equals("cce") || tipo == "")
                    {
                        ///
                        /// le o xml da NFe/NFCe
                        ///
                        var nfer = new NFe.ConvertTxt.nfeRead();
                        nfer.ReadFromXml(arqProcNFe);
                        fEmail = nfer.nfe.dest.email;

                        if (tipo == "" || cancelamentoNfe)
                        {
                            if (nfer.nfe.ide.tpImp == ConvertTxt.TpcnTipoImpressao.tiDANFESimplificado)
                            {
                                //DANFE simplificado
                                tipo = "ds";
                            }
                            else
                            {
                                tipo = (nfer.nfe.ide.mod == ConvertTxt.TpcnMod.modNFCe ? "nfce" : "nfe");
                            }
                        }
                        switch (nfer.nfe.protNFe.cStat)
                        {
                            case 110:
                            case 205:
                            case 301:
                            case 302:
                            case 303:
                                denegada = true;
                                break;

                            default:
                                if (arqProcNFe.Equals(nomeArquivoRecebido))
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
                            case "ds":
                                fExtensao = Propriedade.ExtRetorno.ProcEventoNFe;
                                break;

                            case "cte":
                            case "cteos":
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
                            int ndias = 0;
                            while (ndias < 60)
                            {
                                //TODO André/Wandrey: Tem que fazer o tratamento do cancelamento por substituição da nfce, por enquanto, não vai funcionar a impressão quando cancelamento for por substuituição
                                string filenameCancelamento = tempFile +
                                                                string.Format("_{0}_01{1}",
                                                                    ((int)NFe.ConvertTxt.tpEventos.tpEvCancelamentoNFe).ToString(),
                                                                    fExtensao);

                                ///
                                /// usamos o 'DiretorioSalvarComo' para pesquisar pelo arquivo numa pasta baseando-se pela
                                /// dataEmissaoNFe.AddDays(-ndias)
                                ///
                                string fTemp = Path.Combine(emp.PastaXmlEnviado +
                                                                "\\" + PastaEnviados.Autorizados.ToString() +
                                                                "\\" + getSubFolder(dataEmissaoNFe, ndias, emp.DiretorioSalvarComo), //emp.DiretorioSalvarComo.ToString(dataEmissaoNFe.AddDays(ndias * -1)),
                                                            Path.GetFileName(filenameCancelamento));
                                if (!File.Exists(fTemp) && (tipo.Equals("nfe") || tipo.Equals("ds")))
                                {
                                    ///
                                    /// ops, por evento não foi encontrado, procuramos pelo cancelamento antigo
                                    /// só entramos aqui se o xml de cancelamento nao foi encontrado e se a nota for 'nfe'
                                    /// já que para outros tipos não existia o cancelamento por '-procCancNFe.xml'.
                                    ///
                                    filenameCancelamento = tempFile + "-procCancNFe.xml";
                                    fTemp = Path.Combine(emp.PastaXmlEnviado +
                                                            "\\" + PastaEnviados.Autorizados.ToString() +
                                                            "\\" + getSubFolder(dataEmissaoNFe, ndias, emp.DiretorioSalvarComo), //emp.DiretorioSalvarComo.ToString(dataEmissaoNFe.AddDays(ndias * -1)),
                                                         Path.GetFileName(filenameCancelamento));
                                }
                                if (!File.Exists(fTemp))

                                    ///
                                    /// em sistemas que o XML é gravado no DB, ele pode nao precisar deixar gravado o XML nas pastas de autorizados/denegados
                                    /// então eles podem extrair os conteudos do DB e gravá-los em uma pasta qualquer
                                    ///
                                    if (File.Exists(Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), Path.GetFileName(filenameCancelamento))))
                                        fTemp = Path.Combine(Path.GetDirectoryName(nomeArquivoRecebido), Path.GetFileName(filenameCancelamento));

                                ++ndias;
                                if (File.Exists(fTemp))
                                {
                                    doc.Load(nomeArquivoRecebido = fTemp);
                                    temCancelamento = true;
                                    break;
                                }
                                else
                                {
                                    if ((!tipo.Equals("nfe") && !tipo.Equals("ds")) || emp.DiretorioSalvarComo.ToString().Equals("Raiz") || emp.DiretorioSalvarComo.ToString().Equals(""))

                                        ///
                                        /// ops!
                                        /// Se definido como 'Raiz' e nao encontrou, nao precisamos mais pesquisar pelo arquivo em
                                        /// pastas baseado na data de emissao da nota e se o tipo de nota nao é 'nfe'
                                        ///
                                        break;
                                }
                            }
                        }
                    }
                }

                ///
                /// EPEC, CCe e Cancelamento por evento
                ///
                string ctemp = doc.OuterXml;// File.ReadAllText(nomeArqXMLNFe);
                string dhReg = RetornarConteudoEntre(ctemp, "<dhRegEvento>", "</dhRegEvento>");
                DateTime dhRegEvento = Functions.GetDateTime(dhReg);

                if (dhRegEvento.Year > 1)
                {
                    if ((fProtocolo = RetornarConteudoEntre(ctemp, "</dhRegEvento><nProt>", "</nProt>")) == "")
                        fProtocolo = RetornarConteudoEntre(ctemp, "<nProt>", "</nProt>");

                    fProtocolo += "  " + dhRegEvento.ToString("dd/MM/yyyy HH:mm:ss");
                    if (dhReg.EndsWith("-01:00") ||
                        dhReg.EndsWith("-02:00") ||
                        dhReg.EndsWith("-03:00") ||
                        dhReg.EndsWith("-04:00"))
                    {
                        fProtocolo += dhReg.Substring(dhReg.Length - 6);
                    }
                }

                if (File.Exists(arqProcNFe) || File.Exists(nomeArquivoRecebido))
                {
                    string Args = "";

                    if (tipo.Equals("cte") || tipo.Equals("cteos"))
                    {
                        Args += " EE=1";    //EnviarEmail
                        if (!string.IsNullOrEmpty(emp.EmailDanfe) && !emp.AdicionaEmailDanfe)
                            Args += " E=\"" + emp.EmailDanfe + "\"";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(fEmail))
                        {
                            if (args != null)
                                args.TryGetValue("email", out fEmail);
                        }

                        ///
                        /// se tem um e-mail definido nos parametros da empresa
                        ///

                        if (!string.IsNullOrEmpty(emp.EmailDanfe))
                        {
                            if (!emp.AdicionaEmailDanfe)
                                fEmail = emp.EmailDanfe;
                            else
                                fEmail += ";" + emp.EmailDanfe;
                        }

                        if (!string.IsNullOrEmpty(fEmail))
                        {
                            fEmail = fEmail.Replace(";", ",").TrimStart(new char[] { ',', ' ' }).TrimEnd(new char[] { ',' });

                            if (!string.IsNullOrEmpty(fEmail))
                            {
                                Args += " EE=1";    //EnviarEmail
                                Args += " E=\"" + fEmail + "\"";
                                Args += " IEX=1";   //IgnorarEmail principal
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(emp.PastaConfigUniDanfe))
                    {
                        Args += " PC=\"" + emp.PastaConfigUniDanfe + "\"";
                    }

                    if (isEPEC)
                        Args += " P=2"; //numero de cópias
                    else
                    {
                        if (args != null)
                        {
                            string copias = "";
                            if (args.TryGetValue("copias", out copias))
                                if (!copias.Equals("-1") && Convert.ToInt32("0" + copias) > 0)
                                    Args += " P=" + copias;
                        }
                    }

                    string configDanfe = "";
                    if (isEPEC)
                    {
                        ///
                        /// define como arquivo principal o XML da NFe/NFCe/MDFe/CTe
                        ///
                        Args += " A=\"" + arqProcNFe + "\"";

                        ///
                        /// define como arquivo adicional o enviado a esta chamada
                        ///
                        Args += " AD=\"" + nomeArquivoRecebido + "\"";
                        if (epecTipo.Equals("procEventoMDFe"))
                            Args += " T=damdfe";
                        else
                            if (epecTipo.Equals("procEventoCTe"))
                            Args += " T=dacte";
                        else
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
                            
                            case "ds":
                                Args += " A=\"" + arqProcNFe + "\"";
                                Args += " T=ds";
                                configDanfe = emp.ConfiguracaoDanfe;
                                break;

                            case "mdfe":
                                Args += " A=\"" + arqProcNFe + "\"";
                                Args += " T=damdfe";
                                configDanfe = emp.ConfiguracaoDanfe;
                                break;

                            case "cteos":
                                Args += " A=\"" + arqProcNFe + "\"";
                                Args += " T=dacteos";
                                configDanfe = emp.ConfiguracaoDanfe;
                                break;

                            case "cte":
                                Args += " A=\"" + arqProcNFe + "\"";
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
                                            Args += " A=\"" + nomeArquivoRecebido + "\"";
                                            Args += " N=\"" + arqProcNFe + "\"";
                                            configDanfe = emp.ConfiguracaoCCe;
                                            break;

                                        case "canmdfe":
                                            Args += " A=\"" + nomeArquivoRecebido + "\"";
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
                                    Args += " A=\"" + nomeArquivoRecebido + "\"";
                                }
                                if (!string.IsNullOrEmpty(tipo))
                                    Args += " T=" + tipo;
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(configDanfe))
                        Args += " C=\"" + configDanfe + "\"";

                    if (temCancelamento)
                        Args += " CC=1"; //Cancelamento

                    string temps = "";

                    if (args != null)
                    {
                        if (args.TryGetValue("impressora", out temps))
                            if (!string.IsNullOrEmpty(temps))
                                Args += " I=\"" + temps + "\"";

                        if (args.TryGetValue("anexos", out temps))
                            if (!string.IsNullOrEmpty(temps))
                            {
                                var an = 1;
                                foreach (var af in temps.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    Args += " anexo" + an.ToString() + "=\"" + af.Replace("\"", "") + "\"";
                                    ++an;
                                    if (an > 6) break;
                                }
                            }

                        if (args.TryGetValue("opcoes", out temps))
                            if (!string.IsNullOrEmpty(temps))
                                Args += " " + temps + " ";   //opcoes do UniDANFE

                        if (args.TryGetValue("np", out temps))
                            if (!string.IsNullOrEmpty(temps))
                            {
                                Args += " NP=\"" + temps + "\"";   //NomePDF
                                Args += " M=0"; //NAO Imprimir
                                Args += " V=0"; //NAO Visualizar
                            }

                        if (args.TryGetValue("pp", out temps))
                            if (!string.IsNullOrEmpty(temps))
                                Args += " PP=\"" + temps + "\"";   //PastaPDF

                        if (args.TryGetValue("plq", out temps))
                            if (!string.IsNullOrEmpty(temps))
                                Args += " plq=\"" + temps + "\"";   //pasta local ou da rede para onde a imagem do QR

                        ///
                        /// define o arquivo de saida de erros
                        ///
                        args.TryGetValue("auxiliar", out fAuxiliar);
                    }

                    temps = Path.GetFileName(nomeArquivoRecebido).Replace(".xml", "");

                    if (string.IsNullOrEmpty(fAuxiliar))
                    {
                        ///
                        /// formata o arquivo auxiliar com base no arquivo enviado para impressao
                        ///
                        /// 999999-procNFe.xml -> aux-99999-procNFe-danfe-erros.txt
                        /// 999999-procCTe.xml -> aux-99999-procCTe-danfe-erros.txt
                        /// 999999-procMDFe.xml -> aux-99999-procMDFe-danfe-erros.txt
                        /// 999999-procEventoNFe.xml -> aux-99999-procEventoNFe-danfe-erros.txt
                        ///
                        fAuxiliar = temps + "-danfe-erros.txt";
                    }

                    //saida erros para arquivo e nome do arquivo de erro
                    Args += " S=A AE=\"" + Path.Combine(emp.PastaXmlRetorno, Path.GetFileName(fAuxiliar)) + "\"";
                    fAuxiliar = "";

                    if (fProtocolo != "")
                    {
                        ///
                        /// formata o arquivo de saida de erros com base no arquivo enviado para impressao
                        /// 999999-procNFe.xml -> aux-99999-procNFe.xml
                        /// 999999-procCTe.xml -> aux-99999-procCTe.xml
                        /// 999999-procMDFe.xml -> aux-99999-procMDFe.xml
                        /// 999999-procEventoNFe.xml -> aux-99999-procEventoNFe.xml
                        ///
                        fAuxiliar = Path.Combine(Path.GetTempPath(), "aux-" + Path.GetFileName(nomeArquivoRecebido));

                        StringBuilder xmlAux = new StringBuilder();
                        xmlAux.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        xmlAux.Append("<outrasInfDANFe>");
                        xmlAux.AppendFormat("<protocolonfe>{0}</protocolonfe>", fProtocolo);
                        xmlAux.Append("</outrasInfDANFe>");

                        File.WriteAllText(fAuxiliar, xmlAux.ToString());
                        Args += " AU=\"" + fAuxiliar + "\"";

                        ///
                        ///OBS: deveria existir um argumento para excluir o arquivo auxiliar, já que ele é temporario
                    }

                    Auxiliar.WriteLog("ExecutaUniDanfe: Iniciou a execução do UniDANFe.", false);
                    System.Diagnostics.Process.Start(Path.Combine(emp.PastaExeUniDanfe, "unidanfe.exe"), Args);
                    Auxiliar.WriteLog("ExecutaUniDanfe: Encerrou a execução do UniDANFe.", false);

                    if (args != null)
                    {
                        string fFileNameRetornoOk = temps + NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).RetornoXML;

                        ///
                        /// formata o arquivo de retorno ao ERP com base no arquivo enviado para impressao
                        /// 999999-procNFe.xml -> 99999-procNFe-ret-danfe.xml
                        /// 999999-procCTe.xml -> 99999-procCTe-ret-danfe.xml
                        /// 999999-procMDFe.xml -> 99999-procMDFe-ret-danfe.xml
                        /// 999999-procEventoNFe.xml -> 99999-procEventoNFe-ret-danfe.xml
                        tipo = "";
                        if (args.TryGetValue("xml", out tipo))
                            if (tipo == "0")    //é TXT?
                                fFileNameRetornoOk = NFe.Components.Functions.ExtrairNomeArq(fFileNameRetornoOk, NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).RetornoXML) +
                                                     NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).RetornoTXT;

                        if (fFileNameRetornoOk.EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).RetornoXML))
                        {
                            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                                    new XElement("DANFE",
                                                        new XElement(NFe.Components.TpcnResources.cStat.ToString(), "1"),
                                                        new XElement("Argumentos", Args)));
                            xml.Save(Path.Combine(emp.PastaXmlRetorno, fFileNameRetornoOk));
                        }
                        else
                        {
                            File.WriteAllText(Path.Combine(emp.PastaXmlRetorno, fFileNameRetornoOk), "cStat|1\n\rArgumentos|" + Args + "\n\r");
                        }
                    }

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

        #endregion ExecutaUniDanfe()

        #endregion ExecutaUniDanfe()

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

        #endregion RemoveSomenteLeitura()

        #region CompressXML()

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

        public static string CompressXML(XmlDocument conteudoXML)
        {
            string value = conteudoXML.InnerXml;

            byte[] buffer = Encoding.UTF8.GetBytes(value);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            return Convert.ToBase64String(ms.GetBuffer());
        }

        #endregion CompressXML()

        #region Decompress

        public static string Decompress(string input)
        {
            char[] enc = input.ToCharArray();
            byte[] dec = Convert.FromBase64CharArray(enc, 0, enc.Length);

            byte[] encodedDataAsBytes = Convert.FromBase64String(input);
            using (System.IO.Stream comp = new System.IO.MemoryStream(encodedDataAsBytes))
            {
                using (System.IO.Stream decomp = new System.IO.Compression.GZipStream(comp, System.IO.Compression.CompressionMode.Decompress, false))
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(decomp))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        /*
                public static string Decompress(string compressedValue)
                {
                    byte[] gZipBuffer = Convert.FromBase64String(compressedValue);
                    using (var memoryStream = new MemoryStream())
                    {
                        int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                        memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                        var buffer = new byte[dataLength];

                        memoryStream.Position = 0;
                        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                        {
                            gZipStream.Read(buffer, 0, buffer.Length);
                        }

                        return Encoding.UTF8.GetString(buffer);
                    }
                }
        */

        #endregion Decompress

        /// <summary>
        /// Lucas A. Araujo
        /// Criptografa uma string.
        /// </summary>
        /// <param name="messageString">String</param>
        /// <returns>Texto criptografado</returns>
        public static string EncryptSHA1(string messageString)
        {
            SHA1 mySHA1 = SHA1Managed.Create();
            byte[] hashValue = mySHA1.ComputeHash(UnicodeEncoding.UTF8.GetBytes(messageString));

            return Convert.ToBase64String(hashValue);
        }

        public static void CriarArquivosParaServico()
        {
            string arqServicoIniciar = Path.Combine(Propriedade.PastaExecutavel, "servico_iniciar.bat");
            if (!File.Exists(arqServicoIniciar))
            {
                try
                {
                    string binPath = "\"" + Propriedade.PastaExecutavel + "\\uninfeservico.exe\"";

                    File.WriteAllText(Path.Combine(Propriedade.PastaExecutavel, "servico_iniciar.bat"),
                        "net start UniNFeServico" +
                        "\r\npause");

                    File.WriteAllText(Path.Combine(Propriedade.PastaExecutavel, "servico_parar.bat"),
                        "net stop UniNFeServico" +
                        "\r\npause");

                    File.WriteAllText(Path.Combine(Propriedade.PastaExecutavel, "servico_reiniciar.bat"),
                        "net stop UniNFeServico" +
                        "\r\n" +
                        "net start UniNFeServico" +
                        "\r\npause");

                    File.WriteAllText(Path.Combine(Propriedade.PastaExecutavel, "servico_remover.bat"),
                        "sc delete UniNFeServico" +
                        "\r\npause");

                    File.WriteAllText(Path.Combine(Propriedade.PastaExecutavel, "servico_instalar.bat"),
                        "sc create UniNFeServico binPath= " + binPath + " DisplayName= " + "\"UniNfe Servico\" start= auto" +
                        "\r\npause");

                    File.WriteAllText(Path.Combine(Propriedade.PastaExecutavel, "servico_testar.bat"),
                        "call servico_instalar\r\n" +
                        "call servico_iniciar\r\n" +
                        "call servico_parar\r\n" +
                        "call servico_remover\r\n");
                }
                catch { }
            }
        }

        public static T ToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
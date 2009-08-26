using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UniNFeLibrary.Enums;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Threading;

namespace UniNFeLibrary
{
    public class Auxiliar
    {
        #region ExtrairNomeArq()
        /// <summary>
        /// Extrai somente o nome do arquivo de uma string; para ser utilizado na situação desejada. Veja os exemplos na documentação do código.
        /// </summary>
        /// <param name="pPastaArq">String contendo o caminho e nome do arquivo que é para ser extraido o nome.</param>
        /// <param name="pFinalArq">String contendo o final do nome do arquivo até onde é para ser extraído.</param>
        /// <returns>Retorna somente o nome do arquivo de acordo com os parâmetros passados - veja exemplos.</returns>
        /// <example>
        /// MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", "-ped-sta.xml"));
        /// //Será demonstrado no message a string "ArqSituacao"
        /// 
        /// MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", ".xml"));
        /// //Será demonstrado no message a string "ArqSituacao-ped-sta"
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>19/06/2008</date>
        public string ExtrairNomeArq(string pPastaArq, string pFinalArq)
        {
            //Achar o posição inicial do nome do arquivo
            //procura por pastas, tira elas para ficar somente o nome do arquivo
            Int32 nAchou = 0;
            Int32 nPosI = 0;
            for (Int32 nCont = 0; nCont < pPastaArq.Length; nCont++)
            {
                nAchou = pPastaArq.IndexOf("\\", nCont);
                if (nAchou >= 0)
                {
                    nCont = nAchou;
                    nPosI = nAchou + 1;
                }
                else
                {
                    break;
                }
            }

            //Achar a posição final do nome do arquivo
            Int32 nPosF = pPastaArq.ToUpper().IndexOf(pFinalArq.ToUpper());

            //Extrair o nome do arquivo
            string cRetorna = pPastaArq.Substring(nPosI, nPosF - nPosI);

            return cRetorna;
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
        public void MoverArquivo(string Arquivo, PastaEnviados SubPastaXMLEnviado, DateTime Emissao)
        {
            //TODO: Criar vários try/catch neste método para evitar erros

            //Definir o arquivo que vai ser deletado ou movido para outra pasta
            FileInfo oArquivo = new FileInfo(Arquivo);

            //Criar a pasta EmProcessamento
            if (!Directory.Exists(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString()))
            {
                System.IO.Directory.CreateDirectory(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString());
            }

            //Criar a Pasta Autorizado
            if (!Directory.Exists(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString()))
            {
                System.IO.Directory.CreateDirectory(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString());
            }

            //Criar a Pasta Denegado
            if (!Directory.Exists(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString()))
            {
                System.IO.Directory.CreateDirectory(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString());
            }

            //Criar Pasta do Mês para gravar arquivos enviados autorizados ou denegados
            string strNomePastaEnviado = string.Empty;
            string strDestinoArquivo = string.Empty;
            switch (SubPastaXMLEnviado)
            {
                case PastaEnviados.EmProcessamento:
                    strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString();
                    strDestinoArquivo = strNomePastaEnviado + "\\" + this.ExtrairNomeArq(Arquivo, ".xml") + ".xml";
                    break;

                case PastaEnviados.Autorizados:
                    strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Emissao.ToString("yyyyMM");
                    strDestinoArquivo = strNomePastaEnviado + "\\" + this.ExtrairNomeArq(Arquivo, ".xml") + ".xml";
                    goto default;

                case PastaEnviados.Denegados:
                    strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + Emissao.ToString("yyyyMM");
                    strDestinoArquivo = strNomePastaEnviado + "\\" + this.ExtrairNomeArq(Arquivo, "-nfe.xml") + "-den.xml";
                    goto default;

                default:
                    if (!Directory.Exists(strNomePastaEnviado))
                    {
                        System.IO.Directory.CreateDirectory(strNomePastaEnviado);
                    }
                    break;
            }

            //Se conseguiu criar a pasta ele move o arquivo, caso contrário
            if (Directory.Exists(strNomePastaEnviado) == true)
            {
                //Mover o arquivo da nota fiscal para a pasta dos enviados
                if (File.Exists(strDestinoArquivo))
                {
                    FileInfo oArqDestino = new FileInfo(strDestinoArquivo);
                    oArqDestino.Delete();
                }
                oArquivo.MoveTo(strDestinoArquivo);

                if (SubPastaXMLEnviado == PastaEnviados.Autorizados || SubPastaXMLEnviado == PastaEnviados.Denegados)
                {
                    //Fazer um backup do XML que foi copiado para a pasta de enviados
                    //para uma outra pasta para termos uma maior segurança no arquivamento
                    //Normalmente esta pasta é em um outro computador ou HD
                    if (ConfiguracaoApp.cPastaBackup.Trim() != "")
                    {
                        //Criar Pasta do Mês para gravar arquivos enviados
                        string strNomePastaBackup = string.Empty;
                        switch (SubPastaXMLEnviado)
                        {
                            case PastaEnviados.Autorizados:
                                strNomePastaBackup = ConfiguracaoApp.cPastaBackup + "\\" + PastaEnviados.Autorizados + "\\" + Emissao.ToString("yyyyMM");
                                goto default;

                            case PastaEnviados.Denegados:
                                strNomePastaBackup = ConfiguracaoApp.cPastaBackup + "\\" + PastaEnviados.Denegados + "\\" + Emissao.ToString("yyyyMM");
                                goto default;

                            default:
                                if (Directory.Exists(strNomePastaBackup) == false)
                                {
                                    System.IO.Directory.CreateDirectory(strNomePastaBackup);
                                }
                                break;
                        }

                        //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                        if (Directory.Exists(strNomePastaBackup) == true)
                        {
                            //Mover o arquivo da nota fiscal para a pasta de backup
                            string strNomeArquivoBkp = strNomePastaBackup + "\\" + this.ExtrairNomeArq(Arquivo, ".xml") + ".xml";
                            if (File.Exists(strNomeArquivoBkp))
                            {
                                FileInfo oArqDestinoBkp = new FileInfo(strNomeArquivoBkp);
                                oArqDestinoBkp.Delete();
                            }
                            FileInfo oArquivoBkp = new FileInfo(strDestinoArquivo);

                            oArquivoBkp.CopyTo(strNomeArquivoBkp, true);
                        }
                        else
                        {
                            //TODO: Tenho que tratar este Erro e retornar algo para o ERP urgente, pois pode falhar
                        }
                    }
                }
            }
            else
            {
                //TODO: Tenho que tratar este Erro e retornar algo para o ERP urgente, pois pode falhar                
            }
        }
        #endregion

        #region MoverArquivo() - Sobrecarga
        /// <summary>
        /// Move arquivos da nota fiscal eletrônica para suas respectivas pastas
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser movido</param>
        /// <param name="PastaXMLEnviado">Pasta de XML´s enviados para onde será movido o arquivo</param>
        /// <param name="SubPastaXMLEnviado">SubPasta de XML´s enviados para onde será movido o arquivo</param>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void MoverArquivo(string Arquivo, PastaEnviados SubPastaXMLEnviado)
        {
            this.MoverArquivo(Arquivo, SubPastaXMLEnviado, DateTime.Now);
        }
        #endregion

        #region DeletarArquivo()
        /// <summary>
        /// Excluir arquivos do HD
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser excluido.</param>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void DeletarArquivo(string Arquivo)
        {
            //TODO: Criar vários try/catch neste método para evitar erros

            //Definir o arquivo que vai ser deletado ou movido para outra pasta
            FileInfo oArquivo = new FileInfo(Arquivo);

            if (File.Exists(Arquivo))
            {
                oArquivo.Delete();
            }
        }
        #endregion

        #region XmlToString()
        /// <summary>
        /// Método responsável por ler o conteúdo de um XML e retornar em uma string
        /// </summary>
        /// <param name="parNomeArquivo">Caminho e nome do arquivo XML que é para pegar o conteúdo e retornar na string.</param>
        /// <returns>Retorna uma string com o conteúdo do arquivo XML</returns>
        /// <example>
        /// string ConteudoXML;
        /// ConteudoXML = THIS.XmltoString( @"c:\arquivo.xml" );
        /// MessageBox.Show( ConteudoXML );
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        public string XmlToString(string parNomeArquivo)
        {
            string conteudo_xml = string.Empty;

            StreamReader SR = null;
            try
            {
                SR = File.OpenText(parNomeArquivo);
                conteudo_xml = SR.ReadToEnd();
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                SR.Close();
            }

            return conteudo_xml;
        }
        #endregion

        #region DeletarArqXMLErro()
        /// <summary>
        /// Deleta o XML da pata temporária dos arquivos com erro se o mesmo existir.
        /// </summary>
        public void DeletarArqXMLErro(string Arquivo)
        {
            try
            {
                this.DeletarArquivo(Arquivo);
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region MoveArqErro()
        /// <summary>
        /// Move arquivos com a extensão informada e que está com erro para uma pasta de xml´s/arquivos com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <param name="ExtensaoArq">Extensão do arquivo que vai ser movido. Ex: .xml</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg, ".xml")</example>
        public void MoveArqErro(string Arquivo, string ExtensaoArq)
        {
            if (File.Exists(Arquivo))
            {
                FileInfo oArquivo = new FileInfo(Arquivo);

                if (Directory.Exists(ConfiguracaoApp.vPastaXMLErro) == true)
                {
                    //Mover o arquivo da nota fiscal para a pasta do XML com erro
                    string vNomeArquivo = ConfiguracaoApp.vPastaXMLErro + "\\" + this.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;
                    if (File.Exists(vNomeArquivo))
                    {
                        FileInfo oArqDestino = new FileInfo(vNomeArquivo);
                        oArqDestino.Delete();
                    }

                    oArquivo.MoveTo(vNomeArquivo);
                }
                else
                {
                    oArquivo.Delete();
                }
            }
        }
        #endregion

        #region MoveArqErro
        /// <summary>
        /// Move arquivos XML com erro para uma pasta de xml´s com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg)</example>
        public void MoveArqErro(string Arquivo)
        {
            this.MoveArqErro(Arquivo, ".xml");
        }
        #endregion

        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erro ocorrido na invocação dos WebServices ou na execusão de alguma
        /// rotina de validação, etc. Este arquivo é gravado para que o sistema ERP tenha condições de interagir
        /// com o usuário.
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo XML que foi enviado para os WebServices</param>
        /// <param name="PastaXMLRetorno">Pasta de retorno para gravar o XML de erro</param>
        /// <param name="FinalArqEnvio">Final do nome do arquivo de solicitação do serviço</param>
        /// <param name="FinalArqErro">Final do nome do arquivo que é para ser gravado o erro</param>
        /// <param name="Erro">Texto do erro ocorrido a ser gravado no arquivo</param>
        /// <param name="PastaXMLErro">Pasta para mover o XML com erro</param>
        /// <by>Wandrey Mundin Ferreira</by>
        public void GravarArqErroServico(string Arquivo, string FinalArqEnvio, string FinalArqErro, string Erro)
        {
            //Qualquer erro ocorrido o aplicativo vai mover o XML com falha da pasta de envio
            //para a pasta de XML´s com erros. Futuramente ele é excluido quando outro igual
            //for gerado corretamente.
            this.MoveArqErro(Arquivo);

            //Grava arquivo de ERRO para o ERP
            string cArqErro = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                              this.ExtrairNomeArq(Arquivo, FinalArqEnvio) +
                              FinalArqErro;

            File.WriteAllText(cArqErro, Erro, Encoding.Default);
        }
        #endregion

        #region GravarArqErroERP
        /// <summary>
        /// grava um arquivo de erro ao ERP
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="Erro"></param>
        public void GravarArqErroERP(string Arquivo, string Erro)
        {
            //Grava arquivo de ERRO para o ERP
            string cArqErro = ConfiguracaoApp.vPastaXMLRetorno + "\\" + Arquivo;
            File.WriteAllText(cArqErro, Erro, Encoding.Default);
        }
        #endregion

        #region ValidarArqXML()
        /// <summary>
        /// Valida o arquivo XML 
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo XML a ser validado</param>
        /// <returns>
        /// Se retornar uma string em branco, significa que o XML foi 
        /// validado com sucesso, ou seja, não tem nenhum erro. Se o retorno
        /// tiver algo, algum erro ocorreu na validação.
        /// </returns>
        /// <example>
        /// string cResultadoValidacao = this.ValidarArqXML();
        /// 
        /// if (cResultadoValidacao == "")
        /// {
        ///     MessageBox.Show( "Arquivo validado com sucesso" );
        /// }
        /// else
        /// {
        ///     MessageBox.Show( cResultadoValidacao );
        /// }
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>31/07/2008</date>         
        public string ValidarArqXML(string Arquivo)
        {
            string cRetorna = "";

            // Validar Arquivo XML
            ValidarXMLs oValidador = new ValidarXMLs();
            oValidador.TipoArquivoXML(Arquivo);

            if (oValidador.nRetornoTipoArq >= 1 && oValidador.nRetornoTipoArq <= 11)
            {
                oValidador.Validar(Arquivo, oValidador.cArquivoSchema);
                if (oValidador.Retorno != 0)
                {
                    cRetorna = "XML INCONSISTENTE!\r\n\r\n" + oValidador.RetornoString;
                }
            }
            else
            {
                cRetorna = "XML INCONSISTENTE!\r\n\r\n" + oValidador.cRetornoTipoArq;
            }

            return cRetorna;
        }
        #endregion

        #region ValidarAssinarXML()
        /// <summary>
        /// Efetua a validação de qualquer XML, NFE, Cancelamento, Inutilização, etc..., e retorna se está ok ou não
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo XML a ser validado e assinado</param>
        /// <param name="PastaValidar">Nome da pasta onde fica os arquivos a serem validados</param>
        /// <param name="PastaXMLErro">Nome da pasta onde é para gravar os XML´s validados que apresentaram erro.</param>
        /// <param name="PastaXMLRetorno">Nome da pasta de retorno onde será gravado o XML com o status da validação</param>
        /// <param name="Certificado">Certificado digital a ser utilizado na validação</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>28/05/2009</date>
        public void ValidarAssinarXML(string Arquivo)
        {
            Boolean Assinou = true;
            ValidarXMLs oValidador = new ValidarXMLs();
            oValidador.TipoArquivoXML(Arquivo);

            //Assinar o XML se tiver tag para assinar
            if (oValidador.TagAssinar != string.Empty)
            {
                AssinaturaDigital oAD = new AssinaturaDigital();

                try
                {
                    oAD.Assinar(Arquivo, oValidador.TagAssinar, ConfiguracaoApp.oCertificado);

                    if (oAD.intResultado != 0)
                    {
                        Assinou = false;
                    }
                }
                catch (Exception ex)
                {
                    Assinou = false;
                    this.GravarXMLRetornoValidacao(Arquivo, "2", "Ocorreu um erro ao assinar o XML: " + ex.Message);
                    this.MoveArqErro(Arquivo);
                }
            }


            if (Assinou)
            {
                // Validar o Arquivo XML
                if (oValidador.nRetornoTipoArq >= 1 && oValidador.nRetornoTipoArq <= 11)
                {
                    oValidador.Validar(Arquivo, oValidador.cArquivoSchema);
                    if (oValidador.Retorno != 0)
                    {
                        this.GravarXMLRetornoValidacao(Arquivo, "3", "Ocorreu um erro ao validar o XML: " + oValidador.RetornoString);
                        this.MoveArqErro(Arquivo);
                    }
                    else
                    {
                        if (!Directory.Exists(ConfiguracaoApp.PastaValidar + "\\Validado"))
                        {
                            Directory.CreateDirectory(ConfiguracaoApp.PastaValidar + "\\Validado");
                        }

                        string ArquivoNovo = ConfiguracaoApp.PastaValidar + "\\Validado\\" + this.ExtrairNomeArq(Arquivo, ".xml") + ".xml";

                        if (File.Exists(ArquivoNovo))
                        {
                            FileInfo oArqNovo = new FileInfo(ArquivoNovo);
                            oArqNovo.Delete();
                        }

                        FileInfo oArquivo = new FileInfo(Arquivo);
                        oArquivo.MoveTo(ArquivoNovo);

                        this.GravarXMLRetornoValidacao(Arquivo, "1", "XML assinado e validado com sucesso.");
                    }
                }
                else
                {
                    this.GravarXMLRetornoValidacao(Arquivo, "4", "Ocorreu um erro ao validar o XML: " + oValidador.cRetornoTipoArq);
                    this.MoveArqErro(Arquivo);
                }
            }
        }
        #endregion

        #region GravarXMLRetornoValidacao()
        /// <summary>
        /// Na tentativa de somente validar ou assinar o XML se encontrar um erro vai ser retornado um XML para o ERP
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo XML validado</param>
        /// <param name="PastaXMLRetorno">Pasta de retorno para ser gravado o XML</param>
        /// <param name="cStat">Status da validação</param>
        /// <param name="xMotivo">Status descritivo da validação</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>28/05/2009</date>
        private void GravarXMLRetornoValidacao(string Arquivo, string cStat, string xMotivo)
        {
            //Definir o nome do arquivo de retorno
            string ArquivoRetorno = this.ExtrairNomeArq(Arquivo, ".xml") + "-ret.xml";

            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            //Agora vamos criar um XML Writer
            XmlWriter oXmlGravar = XmlWriter.Create(ConfiguracaoApp.vPastaXMLRetorno + "\\" + ArquivoRetorno);

            //Agora vamos gravar os dados
            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("Validacao");
            oXmlGravar.WriteElementString("cStat", cStat);
            oXmlGravar.WriteElementString("xMotivo", xMotivo);
            oXmlGravar.WriteEndElement(); //nfe_configuracoes
            oXmlGravar.WriteEndDocument();
            oXmlGravar.Flush();
            oXmlGravar.Close();
        }
        #endregion

        #region LerTag()
        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo.
        /// </summary>
        /// <param name="Elemento">Elemento a ser pesquisado o Nome da TAG</param>
        /// <param name="NomeTag">Nome da Tag</param>
        /// <returns>Conteúdo da tag</returns>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public string LerTag(XmlElement Elemento, string NomeTag)
        {
            string Retorno = string.Empty;

            if (Elemento.GetElementsByTagName(NomeTag).Count != 0)
            {
                Retorno = Elemento.GetElementsByTagName(NomeTag)[0].InnerText;
                Retorno += ";";
            }

            return Retorno;
        }
        #endregion

        #region VerStatusServico()
        /// <summary>
        /// Verifica e retorna o Status do Servido da NFE. Para isso este método gera o arquivo XML necessário
        /// para obter o status do serviõ e faz a leitura do XML de retorno, disponibilizando uma string com a mensagem
        /// obtida.
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public string VerStatusServico(string XmlNfeDadosMsg)
        {
            string vStatus = "Ocorreu uma falha ao tentar obter a situação do serviço. Aguarde um momento e tente novamente.";

            string ArqXMLRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                      this.ExtrairNomeArq(XmlNfeDadosMsg, "-ped-sta.xml") +
                      "-sta.xml";

            string ArqERRRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                      this.ExtrairNomeArq(XmlNfeDadosMsg, "-ped-sta.xml") +
                      "-sta.err";

            DateTime startTime;
            DateTime stopTime;
            TimeSpan elapsedTime;

            long elapsedMillieconds;
            startTime = DateTime.Now;

            while (true)
            {
                stopTime = DateTime.Now;
                elapsedTime = stopTime.Subtract(startTime);
                elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minutos
                {
                    break;
                }

                if (File.Exists(ArqXMLRetorno))
                {
                    try
                    {
                        //Verificar se consegue abrir o arquivo em modo exclusivo, se conseguir ele dá sequencia
                        using (FileStream fs = File.Open(ArqXMLRetorno, FileMode.Open, FileAccess.ReadWrite, FileShare.Write))
                        {
                            //Conseguiu abrir o arquivo, significa que está perfeitamente gerado
                            //assim vou iniciar o processo de envio do XML
                            fs.Close();

                            //Ler o status do serviço no XML retornado pelo WebService
                            XmlTextReader oLerXml = new XmlTextReader(ArqXMLRetorno);

                            try
                            {
                                while (oLerXml.Read())
                                {
                                    if (oLerXml.NodeType == XmlNodeType.Element)
                                    {
                                        if (oLerXml.Name == "xMotivo")
                                        {
                                            oLerXml.Read();
                                            vStatus = oLerXml.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                //Se não conseguir ler o arquivo vai somente retornar ao loop para tentar novamente, pois 
                                //pode ser que o arquivo esteja em uso ainda.
                            }

                            oLerXml.Close();

                            //Detetar o arquivo de retorno
                            try
                            {
                                FileInfo oArquivoDel = new FileInfo(ArqXMLRetorno);
                                oArquivoDel.Delete();

                                break;
                            }
                            catch
                            {
                                //Somente deixa fazer o loop novamente e tentar deletar
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else if (File.Exists(ArqERRRetorno))
                {
                    //Retornou um arquivo com a extensão .ERR, ou seja, deu um erro,
                    //futuramente tem que retornar esta mensagem para a MessageBox do usuário.

                    //Detetar o arquivo de retorno
                    try
                    {
                        FileInfo oArquivoDel = new FileInfo(ArqERRRetorno);
                        oArquivoDel.Delete();
                        break;
                    }
                    catch
                    {
                        //Somente deixa fazer o loop novamente e tentar deletar
                    }
                }

                Thread.Sleep(5000);
            }

            //Retornar o status do serviço
            return vStatus;
        }
        #endregion

        #region MemoryStream
        /// <summary>
        /// Método responsável por converter uma String contendo a estrutura de um XML em uma Stream para
        /// ser lida pela XMLDocument
        /// </summary>
        /// <returns>String convertida em Stream</returns>
        /// <remarks>Conteúdo do método foi fornecido pelo Marcelo da desenvolvedores.net</remarks>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public static MemoryStream StringXmlToStream(string strXml)
        {
            byte[] byteArray = new byte[strXml.Length];
            System.Text.ASCIIEncoding encoding = new
            System.Text.ASCIIEncoding();
            byteArray = encoding.GetBytes(strXml);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
        #endregion
    }
}

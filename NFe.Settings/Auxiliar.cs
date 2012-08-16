using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using NFe.Components;
using System.Xml;
using System.Windows.Forms;

namespace NFe.Settings
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
        /// 
#if tirada
        public string xExtrairNomeArq(string pPastaArq, string pFinalArq)
        {
            FileInfo fi = new FileInfo(pPastaArq);
            string ret = fi.Name;
            ret = ret.Substring(0, ret.Length - pFinalArq.Length);
            return ret;
        }
#endif
        #endregion

        #region GravarArqErroERP
        /// <summary>
        /// grava um arquivo de erro ao ERP
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="Erro"></param>
        public void GravarArqErroERP(string Arquivo, string Erro)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            if (!string.IsNullOrEmpty(Arquivo))
            {
                try
                {
                    if (Empresa.Configuracoes[emp].PastaRetorno != string.Empty)
                    {
                        //Grava arquivo de ERRO para o ERP
                        string cArqErro = Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileName(Arquivo);
                        File.WriteAllText(cArqErro, Erro, Encoding.Default);
                    }
                }
                catch
                {
                    //TODO: V3.0 - Não deveriamos retornar a exeção com throw?
                }
            }
        }
        #endregion

        #region WriteLog()
        public static void WriteLog(string msg)
        {
            bool geraLog = ConfiguracaoApp.GravarLogOperacoesRealizadas;
            if (geraLog)
                Auxiliar.WriteLog(msg, false);
        }
        #endregion

        #region WriteLog()
        public static void WriteLog(string msg, bool gravarStackTrace)
        {
            bool geraLog = ConfiguracaoApp.GravarLogOperacoesRealizadas;

            if (geraLog)
            {
                string fileName = Propriedade.PastaLog + "\\uninfe_" + DateTime.Now.ToString("yyyy-MMM-dd") + ".log";

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

                    StreamWriter arquivoWS = null;
                    try
                    {
                        //Se for para gravar ot race
                        if (gravarStackTrace)
                        {
                            msg += "\r\nSTACK TRACE:";
                            msg += "\r\n" + Environment.StackTrace;

                            /*
                            StackTrace stackTrace = new StackTrace();
                            StackFrame[] stackFrames = stackTrace.GetFrames();
                            foreach (StackFrame s in stackFrames)
                            {
                                msg += "\r\nModule: " + s.GetMethod().ReflectedType.Module.Name + " Class: " + s.GetMethod().ReflectedType.FullName + " Method: " + s.GetMethod().Name;
                                msg += " line: " + s.GetFileLineNumber();
                                
                            }*/
                        }

                        arquivoWS = new StreamWriter(fileName, true);
                        arquivoWS.WriteLine(DateTime.Now.ToLongTimeString() + "  " + msg);
                        arquivoWS.Flush();
                        arquivoWS.Close();
                        break;
                    }
                    catch
                    {
                        if (arquivoWS != null)
                        {
                            arquivoWS.Close();
                        }

                        if (elapsedMillieconds >= 60000) //60.000 ms que corresponde á 60 segundos que corresponde a 1 minuto
                        {
                            break;
                        }
                    }

                    Thread.Sleep(2);
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
            try
            {
                this.MoveArqErro(Arquivo, Path.GetExtension(Arquivo));
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                if (File.Exists(Arquivo))
                {
                    FileInfo oArquivo = new FileInfo(Arquivo);

                    if (Directory.Exists(Empresa.Configuracoes[emp].PastaErro))
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
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion       

        #region EstaAutorizada()
        /// <summary>
        /// Verifica se o XML de Distribuição da Nota Fiscal (-procNFe) já está na pasta de Notas Autorizadas
        /// </summary>
        /// <param name="Arquivo">Arquivo XML a ser verificado</param>
        /// <param name="Emissao">Data de emissão da NFe</param>
        /// <param name="Extensao">Extensão a ser verificada (ExtXml.Nfe ou Propriedade.ExtRetorno.ProcNFe)</param>
        /// <returns>Se está na pasta de XML´s autorizados</returns>
        public bool EstaAutorizada(string Arquivo, DateTime Emissao, string Extensao)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            string strNomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + "\\" + Functions.ExtrairNomeArq(Arquivo, Propriedade.ExtEnvio.Nfe) + Extensao);
        }
        #endregion

        #region EstaDenegada()
        /// <summary>
        /// Verifica se o XML da nota fiscal já está na pasta de Notas Denegadas
        /// </summary>
        /// <param name="Arquivo">Arquivo XML a ser verificado</param>
        /// <param name="Emissao">Data de emissão da NFe</param>
        /// <returns>Se está na pasta de XML´s denegados</returns>
        public bool EstaDenegada(string Arquivo, DateTime Emissao)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            string strNomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                            PastaEnviados.Denegados.ToString() + "\\" +
                                            Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + "\\" + Functions.ExtrairNomeArq(Arquivo, Propriedade.ExtEnvio.Nfe) + "-den.xml");
        }
        #endregion

        #region ArquivosPasta()
        /// <summary>
        /// Monta uma lista dos arquivos existentes em uma determinada pasta
        /// </summary>
        /// <param name="strPasta">Pasta a ser verificada a existencia de arquivos</param>
        /// <param name="strMascara">Mascara dos arquivos a serem procurados</param>
        /// <returns>Retorna a lista dos arquivos da pasta</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        public List<string> ArquivosPasta(string strPasta, string strMascara)
        {
            //Criar uma Lista dos arquivos existentes na pasta
            List<string> lstArquivos = new List<string>();

            if (strPasta.Trim() != "" && Directory.Exists(strPasta))
            {
                string cError = "";
                try
                {
                    string[] filesInFolder = Directory.GetFiles(strPasta, strMascara);
                    foreach (string item in filesInFolder)
                    {
                        lstArquivos.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    cError = ex.Message;
                }
                if (!string.IsNullOrEmpty(cError))
                {
                    new Auxiliar().GravarArqErroERP(string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddTHHmmss")), cError);
                    lstArquivos.Clear();
                }
            }

            return lstArquivos;
        }
        #endregion

        #region ConversaoNovaVersao()
        /// <summary>
        /// Conversões que são executadas quando atualizado o aplicativo.
        /// Alguns ajustes que são necessários serem executados automaticamente
        /// para evitar falhas no aplicativo
        /// </summary>
        public static string ConversaoNovaVersao(string cnpjEmpresa)    //danasa 20-9-2010
        {
            #region Conversão referente a parte de Multi-Empresas
            try
            {
                if (!File.Exists(Propriedade.NomeArqEmpresa) && File.Exists(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig))
                {
                    #region Localizar o CNPJ da empresa no certificado
                    string certificado = string.Empty;
                    string nomeEmpresa = string.Empty;  //danasa 20-9-2010

                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig);
                    var configList = xmlDoc.GetElementsByTagName(NFeStrConstants.nfe_configuracoes);
                    foreach (XmlNode configNode in configList)
                    {
                        var configElemento = (XmlElement)configNode;

                        if (configElemento.GetElementsByTagName(NFeStrConstants.CertificadoDigital)[0] != null)
                            certificado = configElemento.GetElementsByTagName(NFeStrConstants.CertificadoDigital)[0].InnerText;
                    }

                    string[] dados = certificado.Split(new char[] { ',', ':' });
                    foreach (string dado in dados)
                    {
                        if (cnpjEmpresa == string.Empty)  //danasa 20-9-2010
                            if (CNPJ.Validate((string)Functions.OnlyNumbers(dado.TrimStart())))
                            {
                                cnpjEmpresa = (string)Functions.OnlyNumbers(dado.TrimStart());
                            }

                        /// danasa 20-9-2010
                        /// use o TrimStart() pois em "dado" está retornando branco no inicio
                        if (dado.TrimStart().StartsWith("CN="))
                        {
                            nomeEmpresa = dado.TrimStart().Substring(3, dado.TrimStart().Length - 3);
                        }
                    }
                    if (cnpjEmpresa == string.Empty || nomeEmpresa == string.Empty) //danasa 20-9-2010
                    {
                        if (nomeEmpresa == string.Empty)
                            throw new Exception("Não foi possível localizar o CNPJ da empresa no certificado configurado, sendo assim as configurações do aplicativo deverão ser realizadas novamente.");

                        /// danasa 20-9-2010
                        /// retorna o nome da empresa ao MainForm para exibir na tela de solicitacao do CNPJ
                        return nomeEmpresa;
                    }
                    #endregion

                    #region Criar o diretório das configurações da empresa
                    string dirEmpresa = Propriedade.PastaExecutavel.Trim() + "\\" + cnpjEmpresa;
                    if (!Directory.Exists(dirEmpresa))
                        Directory.CreateDirectory(dirEmpresa);
                    #endregion

                    #region Copiar o arquivo de configurações para a pasta da empresa
                    string arqConfigOrigem = Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig;
                    string arqConfigDestino = dirEmpresa + "\\" + Propriedade.NomeArqConfig;
                    if (!File.Exists(arqConfigDestino))
                    {
                        File.Copy(arqConfigOrigem, arqConfigDestino);

                        if (File.Exists(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLote))
                        {
                            File.Copy(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLote, dirEmpresa + "\\" + Propriedade.NomeArqXmlLote, true);
                            File.Delete(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLote);
                        }

                        if (File.Exists(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp1))
                        {
                            File.Copy(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp1, dirEmpresa + "\\" + Propriedade.NomeArqXmlLoteBkp1, true);
                            File.Delete(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp1);
                        }

                        if (File.Exists(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp2))
                        {
                            File.Copy(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp2, dirEmpresa + "\\" + Propriedade.NomeArqXmlLoteBkp2, true);
                            File.Delete(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp2);
                        }

                        if (File.Exists(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp3))
                        {
                            File.Copy(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp3, dirEmpresa + "\\" + Propriedade.NomeArqXmlLoteBkp3, true);
                            File.Delete(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlLoteBkp3);
                        }

                        if (File.Exists(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlFluxoNfe))
                        {
                            File.Copy(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlFluxoNfe, dirEmpresa + "\\" + Propriedade.NomeArqXmlFluxoNfe, true);
                            File.Delete(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqXmlFluxoNfe);
                        }
                    }

                    #endregion

                    #region Criar o XML do cadastro de empresas
                    XmlWriterSettings oSettings = new XmlWriterSettings();
                    UTF8Encoding c = new UTF8Encoding(false);

                    //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
                    oSettings.Encoding = c;
                    oSettings.Indent = true;
                    oSettings.IndentChars = "";
                    oSettings.NewLineOnAttributes = false;
                    oSettings.OmitXmlDeclaration = false;

                    try
                    {
                        //Agora vamos criar um XML Writer
                        XmlWriter oXmlGravar = XmlWriter.Create(Propriedade.NomeArqEmpresa, oSettings);

                        //Agora vamos gravar os dados
                        oXmlGravar.WriteStartDocument();
                        oXmlGravar.WriteStartElement("Empresa");

                        try
                        {
                            //Abrir a tag <Registro>
                            oXmlGravar.WriteStartElement("Registro");

                            //Criar o atributo CNPJ dentro da tag Registro
                            oXmlGravar.WriteStartAttribute("CNPJ");

                            //Setar o conteúdo do atributo CNPJ
                            oXmlGravar.WriteString(cnpjEmpresa.Trim());

                            //Encerrar o atributo CNPJ
                            oXmlGravar.WriteEndAttribute(); // Encerrar o atributo CNPJ

                            //Criar a tag <Nome> com seu conteúdo </Nome>
                            oXmlGravar.WriteElementString("Nome", nomeEmpresa.Trim());

                            //Encerrar a tag </Registro>
                            oXmlGravar.WriteEndElement();
                        }
                        catch (Exception ex)
                        {
                            string error = "Ocorreu um erro ao tentar gravar as empresas cadastradas.\r\n\r\nErro: " + ex.Message;
                            if (!Propriedade.ServicoRodando || Propriedade.ExecutandoPeloUniNFe)
                                MessageBox.Show(error, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                WriteLog(error, true);
                        }

                        oXmlGravar.WriteEndElement(); //Encerrar o elemento Empresa
                        oXmlGravar.WriteEndDocument();
                        oXmlGravar.Flush();
                        oXmlGravar.Close();
                    }
                    catch (Exception ex)
                    {
                        string error = "Ocorreu um erro ao tentar gravar as empresas cadastradas.\r\n\r\nErro: " + ex.Message;
                        if (!Propriedade.ServicoRodando || Propriedade.ExecutandoPeloUniNFe)
                            MessageBox.Show(error, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            WriteLog(error, true);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                string error = "Ocorreu um erro na hora de converter o aplicativo para multiempresas.\r\n\r\nErro: " + ex.Message;
                if (!Propriedade.ServicoRodando || Propriedade.ExecutandoPeloUniNFe)
                    MessageBox.Show(error, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    WriteLog(error, true);
            }
            return "";
            #endregion
        }
        #endregion

        #region DefinirTipoServico()
        /// <summary>
        /// Definir o tipo do servico a ser executado a partir da extensão do arquivo
        /// </summary>
        /// <param name="fullPath">Nome do arquivo completo do qual é para definir o tipo de serviço a ser executado</param>
        /// <returns>Retorna o tipo do serviço que deve ser executado</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 27/04/2011
        /// </remarks>
#if movido_para_processar_cs
        public static Servicos xDefinirTipoServico(int empresa, string fullPath)
        {
            Servicos tipoServico = Servicos.Nulo;

            try
            {
                string arq = fullPath.ToLower().Trim();

                if (arq.IndexOf(Empresa.Configuracoes[empresa].PastaValidar.ToLower()) >= 0)
                {
                    tipoServico = Servicos.AssinarValidar;
                }
                else
                {
                    if (arq.IndexOf(Propriedade.ExtEnvio.PedSit_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedSit_TXT) >= 0)
                    {
                        tipoServico = Servicos.PedidoConsultaSituacaoNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedSta_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedSta_TXT) >= 0)
                    {
                        tipoServico = Servicos.PedidoConsultaStatusServicoNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.ConsCad_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsCad_TXT) >= 0)
                    {
                        tipoServico = Servicos.ConsultaCadastroContribuinte;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedCan_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedCan_TXT) >= 0)
                    {
                        tipoServico = Servicos.CancelarNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedInu_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.PedInu_TXT) >= 0)
                    {
                        tipoServico = Servicos.InutilizarNumerosNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedRec_XML) >= 0)
                    {
                        tipoServico = Servicos.PedidoSituacaoLoteNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.Nfe) >= 0)
                    {
                        FileInfo infArq = new FileInfo(arq);
                        string pastaArq = ConfiguracaoApp.RemoveEndSlash(infArq.DirectoryName).ToLower().Trim();
                        string pastaLote = ConfiguracaoApp.RemoveEndSlash(Empresa.Configuracoes[empresa].PastaEnvioEmLote).ToLower().Trim();
                        string pastaEnvio = ConfiguracaoApp.RemoveEndSlash(Empresa.Configuracoes[empresa].PastaEnvio).ToLower().Trim();

                        //Remover a subpasta temp
                        if (pastaArq.EndsWith("\\temp"))
                            pastaArq = Path.GetDirectoryName(pastaArq);

                        //Definir o serviço
                        if (pastaArq == pastaLote)
                            tipoServico = Servicos.AssinarValidarNFe;
                        else if (pastaArq == pastaEnvio)
                            tipoServico = Servicos.MontarLoteUmaNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.Nfe_TXT) >= 0)
                    {
                        tipoServico = Servicos.ConverterTXTparaXML;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvLot) >= 0)
                    {
                        tipoServico = Servicos.EnviarLoteNfe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.GerarChaveNFe_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.GerarChaveNFe_TXT) >= 0)
                    {
                        tipoServico = Servicos.GerarChaveNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_TXT) >= 0)
                    {
                        tipoServico = Servicos.WSExiste;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDPEC_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvDPEC_TXT) >= 0)
                    {
                        tipoServico = Servicos.EnviarDPEC;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.ConsDPEC_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsDPEC_TXT) >= 0)
                    {
                        tipoServico = Servicos.ConsultarDPEC;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.AltCon_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.AltCon_TXT) >= 0)
                    {
                        tipoServico = Servicos.AlterarConfiguracoesUniNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.ConsInf_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsInf_TXT) >= 0)
                    {
                        tipoServico = Servicos.ConsultaInformacoesUniNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCCe_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvCCe_TXT) >= 0)
                    {
                        tipoServico = Servicos.EnviarCCe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCancelamento_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvCancelamento_TXT) >= 0)
                    {
                        tipoServico = Servicos.EnviarEventoCancelamento;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvManifestacao_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvManifestacao_TXT) >= 0)
                    {
                        tipoServico = Servicos.EnviarManifestacao;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.ConsNFeDest_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.ConsNFeDest_TXT) >= 0)
                    {
                        tipoServico = Servicos.ConsultaNFDest;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDownload_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvDownload_TXT) >= 0)
                    {
                        tipoServico = Servicos.DownloadNFe;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvRegistroDeSaida_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvRegistroDeSaida_TXT) >= 0)
                    {
                        tipoServico = Servicos.RegistroDeSaida;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCancRegistroDeSaida_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvCancRegistroDeSaida_TXT) >= 0)
                    {
                        tipoServico = Servicos.RegistroDeSaidaCancelamento;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.MontarLote) >= 0)
                    {
                        if (arq.IndexOf(Empresa.Configuracoes[empresa].PastaEnvioEmLote.ToLower().Trim()) >= 0)
                        {
                            tipoServico = Servicos.MontarLoteVariasNFe;
                        }
                    }
                    #region NFS-e
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedLoteRps) >= 0)
                    {
                        tipoServico = Servicos.ConsultarLoteRps;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedCanNfse) >= 0)
                    {
                        tipoServico = Servicos.CancelarNfse;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitLoteRps) >= 0)
                    {
                        tipoServico = Servicos.ConsultarSituacaoLoteRps;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvLoteRps) >= 0)
                    {
                        tipoServico = Servicos.RecepcionarLoteRps;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitNfse) >= 0)
                    {
                        tipoServico = Servicos.ConsultarNfse;
                    }
                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitNfseRps) >= 0)
                    {
                        tipoServico = Servicos.ConsultarNfsePorRps;
                    }
                    #endregion
                }
            }
            catch
            {
            }

            return tipoServico;
        }
#endif
        #endregion

        #region CarregaEmpresa()
        /// <summary>
        /// Carrega as Emoresas que foram cadastradas e estão gravadas no XML
        /// </summary>
        /// <returns>Retorna uma ArrayList das empresas cadastradas</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 28/07/2010
        /// </remarks>
        public static ArrayList CarregaEmpresa()
        {
            ArrayList empresa = new ArrayList();

            string arqXML = Propriedade.NomeArqEmpresa;

            if (File.Exists(arqXML))
            {
                XmlTextReader oLerXml = null;
                try
                {
                    //Carregar os dados do arquivo XML de configurações da Aplicação
                    oLerXml = new XmlTextReader(arqXML);

                    while (oLerXml.Read())
                    {
                        if (oLerXml.NodeType == XmlNodeType.Element)
                        {
                            if (oLerXml.Name.Equals("Registro"))
                            {
                                string cnpj = oLerXml.GetAttribute("CNPJ");

                                while (oLerXml.Read())
                                {
                                    if (oLerXml.NodeType == XmlNodeType.Element && oLerXml.Name.Equals("Nome"))
                                    {
                                        oLerXml.Read();
                                        string nome = oLerXml.Value;
                                        empresa.Add(new ComboElem(cnpj, 1, nome));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (oLerXml != null)
                        oLerXml.Close();
                }
            }

            empresa.Sort(new OrdenacaoPorNome());

            return empresa;
        }
        #endregion
    }
}

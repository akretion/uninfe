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
            //FileInfo oArquivo = new FileInfo(Arquivo);

            if (File.Exists(Arquivo))
            {
                FileInfo oArquivo = new FileInfo(Arquivo);  // << movido para cá >>
                oArquivo.Delete();
            }
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
            FileInfo fi = new FileInfo(pPastaArq);
            string ret = fi.Name;
            ret = ret.Substring(0, ret.Length - pFinalArq.Length);
            return ret;

            #region bkp Código Antigo
            /*
             **** DEPOIS DE AVALIADO O CÓDIGO ACIMA. ESTE AQUI PODE SER APAGADO ****
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
             */
            #endregion
        }
        #endregion

        #region FileInUse()
        /// <summary>
        /// detectar se o arquivo está em uso
        /// </summary>
        /// <param name="file">caminho do arquivo</param>
        /// <returns>true se estiver em uso</returns>
        /// <by>http://desenvolvedores.net/marcelo</by>
        public static bool FileInUse(string file)
        {
            bool ret = false;

            try
            {
                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                {
                    fs.Close();//fechar o arquivo para nao dar erro em outras aplicações
                }
                return ret;
            }
            catch (IOException ex)
            {
                ret = true;
            }
            return ret;
        }
        #endregion

        #region GerarChaveNFe
        /// <summary>
        /// MontaChave
        /// Cria a chave de acesso da NFe
        /// </summary>
        /// <param name="ArqXMLPedido"></param>
        public void GerarChaveNFe(string ArqPedido, Boolean xml)
        {
            // XML - pedido
            // Filename: XXXXXXXX-gerar-chave.xml
            // -------------------------------------------------------------------
            // <?xml version="1.0" encoding="UTF-8"?>
            // <gerarChave>
            //      <UF>35</UF>                 //se não informado, assume a da configuracao
            //      <nNF>1000</nNF>
            //      <cNF>0</cNF>                //se não informado, eu gero
            //      <serie>1</serie>
            //      <AAMM>0912</AAMM>
            //      <CNPJ>55801377000131</CNPJ>
            // </gerarChave>
            //
            // XML - resposta
            // Filename: XXXXXXXX-ret-gerar-chave.xml
            // -------------------------------------------------------------------
            // <?xml version="1.0" encoding="UTF-8"?>
            // <retGerarChave>
            //      <chaveNFe>350912</chaveNFe>
            // </retGerarChave>
            //

            // TXT - pedido
            // Filename: XXXXXXXX-gerar-chave.txt
            // -------------------------------------------------------------------
            // UF|35
            // nNF|1000
            // cNF|0
            // serie|1
            // AAMM|0912
            // CNPJ|55801377000131
            //
            // TXT - resposta
            // Filename: XXXXXXXX-ret-gerar-chave.txt
            // -------------------------------------------------------------------
            // 35091255801377000131550010000000010000176506
            //
            // -------------------------------------------------------------------
            // ERR - resposta
            // Filename: XXXXXXXX-gerar-chave.err

            string ArqXMLRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" + (xml ? this.ExtrairNomeArq(ArqPedido, ExtXml.GerarChaveNFe_XML) + "-ret-gerar-chave.xml" : this.ExtrairNomeArq(ArqPedido, ExtXml.GerarChaveNFe_TXT) + "-ret-gerar-chave.txt");
            string ArqERRRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" + (xml ? this.ExtrairNomeArq(ArqPedido, ExtXml.GerarChaveNFe_XML) + "-gerar-chave.err" : this.ExtrairNomeArq(ArqPedido, ExtXml.GerarChaveNFe_TXT) + "-gerar-chave.err");

            this.DeletarArquivo(ArqXMLRetorno);
            this.DeletarArquivo(ArqERRRetorno);
            this.DeletarArquivo(ConfiguracaoApp.vPastaXMLErro + "\\" + ArqPedido);

            try
            {
                if (!File.Exists(ArqPedido))
                {
                    throw new Exception("Arquivo " + ArqPedido + " não encontrado");
                }
                UnitxtTOxmlClass oUniTxtToXml = new UnitxtTOxmlClass();

                if (!Auxiliar.FileInUse(ArqPedido))
                {
                    int serie = 0;
                    int nNF = 0;
                    int cNF = 0;
                    int cUF = ConfiguracaoApp.UFCod;
                    string cAAMM = "0000";
                    string cChave = "";
                    string cCNPJ = "";
                    string cError = "";

                    if (xml)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(ArqPedido);

                        XmlNodeList mChaveList = doc.GetElementsByTagName("gerarChave");

                        foreach (XmlNode mChaveNode in mChaveList)
                        {
                            XmlElement mChaveElemento = (XmlElement)mChaveNode;

                            if (mChaveElemento.GetElementsByTagName("UF").Count != 0)
                                cUF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("UF")[0].InnerText);

                            if (mChaveElemento.GetElementsByTagName("nNF").Count != 0)
                                nNF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("nNF")[0].InnerText);

                            if (mChaveElemento.GetElementsByTagName("cNF").Count != 0)
                                cNF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("cNF")[0].InnerText);

                            if (mChaveElemento.GetElementsByTagName("serie").Count != 0)
                                serie = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("serie")[0].InnerText);

                            if (mChaveElemento.GetElementsByTagName("AAMM").Count != 0)
                                cAAMM = mChaveElemento.GetElementsByTagName("AAMM")[0].InnerText;

                            if (mChaveElemento.GetElementsByTagName("CNPJ").Count != 0)
                                cCNPJ = mChaveElemento.GetElementsByTagName("CNPJ")[0].InnerText;
                        }
                    }
                    else
                    {
                        List<string> cLinhas = this.LerArquivo(ArqPedido);
                        string[] dados;
                        foreach (string cLinha in cLinhas)
                        {
                            dados = cLinha.Split('|');
                            dados[0] = dados[0].ToUpper();
                            if (dados.GetLength(0) == 1)
                                cError += "Segmento [" + dados[0] + "] inválido" + Environment.NewLine;
                            else
                                switch (dados[0].ToLower())
                                {
                                    case "uf":
                                        cUF = Convert.ToInt32("0" + dados[1]);
                                        break;
                                    case "nnf":
                                        nNF = Convert.ToInt32("0" + dados[1]);
                                        break;
                                    case "cnf":
                                        cNF = Convert.ToInt32("0" + dados[1]);
                                        break;
                                    case "serie":
                                        serie = Convert.ToInt32("0" + dados[1]);
                                        break;
                                    case "aamm":
                                        cAAMM = dados[1];
                                        break;
                                    case "cnpj":
                                        cCNPJ = dados[1];
                                        break;
                                }
                        }
                    }
                    if (nNF == 0)
                        cError = "Número da nota fiscal deve ser informado" + Environment.NewLine;

                    if (string.IsNullOrEmpty(cAAMM))
                        cError += "Ano e mês da emissão deve ser informado" + Environment.NewLine;

                    if (string.IsNullOrEmpty(cCNPJ))
                        cError += "CNPJ deve ser informado" + Environment.NewLine;

                    //System.Windows.Forms.MessageBox.Show(cAAMM);

                    if (cAAMM.Substring(0, 2) == "00")
                        cError += "Ano da emissão inválido" + Environment.NewLine;

                    //System.Windows.Forms.MessageBox.Show(cChave + "\n\r" + cUF.ToString() + "\n\r" + nNF.ToString() + "\n\r" + cNF.ToString() + "\n\r" + serie.ToString() + "\n\r" + cCNPJ + "\n\r" + cAAMM);

                    if (Convert.ToInt32(cAAMM.Substring(2, 2)) <= 0 || Convert.ToInt32(cAAMM.Substring(2, 2)) > 12)
                        cError += "Mês da emissão inválido" + Environment.NewLine;

                    if (cError != "")
                        throw new Exception(cError);

                    //System.Windows.Forms.MessageBox.Show(cChave + "\n\r" + cUF.ToString() + "\n\r" + nNF.ToString() + "\n\r" + cNF.ToString() + "\n\r" + serie.ToString() + "\n\r" + cCNPJ + "\n\r" + cAAMM);

                    Int64 iTmp = Convert.ToInt64("0" + cCNPJ);
                    cChave = cUF.ToString("00") + cAAMM + iTmp.ToString("00000000000000") + "55";

                    //System.Windows.Forms.MessageBox.Show(cChave);

                    if (cNF == 0)
                    {
                        ///
                        /// gera codigo aleatorio
                        /// 
                        //System.Windows.Forms.MessageBox.Show("go cNF");
                        cNF = oUniTxtToXml.GerarCodigoNumerico(nNF);

                        //System.Windows.Forms.MessageBox.Show(cNF.ToString());
                    }
                    ///
                    /// calcula do digito verificador
                    /// 
                    string ccChave = cChave + serie.ToString("000") + nNF.ToString("000000000") + cNF.ToString("000000000");
                    int cDV = oUniTxtToXml.GerarDigito(ccChave);
                    ///
                    /// monta a chave da NFe
                    /// 
                    cChave += serie.ToString("000") + nNF.ToString("000000000") + cNF.ToString("000000000") + cDV.ToString("0");

                    //System.Windows.Forms.MessageBox.Show(cChave + "\n\r" + cUF.ToString() + "\n\r" + nNF.ToString() + "\n\r" + cNF.ToString() + "\n\r" + serie.ToString() + "\n\r" + cCNPJ + "\n\r" + cAAMM);

                    ///
                    /// grava o XML/TXT de resposta
                    /// 
                    string vMsgRetorno = (xml ? "<?xml version=\"1.0\" encoding=\"UTF-8\"?><retGerarChave><chaveNFe>" + cChave + "</chaveNFe></retGerarChave>" : cChave);
                    File.WriteAllText(ArqXMLRetorno, vMsgRetorno, Encoding.Default);
                    ///
                    /// exclui o XML/TXT de pedido
                    /// 
                    this.DeletarArquivo(ArqPedido);
                }
            }
            catch (Exception ex)
            {
                this.MoveArqErro(ArqPedido);
                File.WriteAllText(ArqERRRetorno, "Arquivo " + ArqERRRetorno + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : ex.Message), Encoding.Default);
            }
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
            if (!string.IsNullOrEmpty(Arquivo))
            {
                try
                {
                    if (ConfiguracaoApp.vPastaXMLRetorno != string.Empty)
                    {
                        //Grava arquivo de ERRO para o ERP
                        string cArqErro = ConfiguracaoApp.vPastaXMLRetorno + "\\" + Path.GetFileName(Arquivo);
                        File.WriteAllText(cArqErro, Erro, Encoding.Default);
                    }
                }
                catch
                {
                    //TODO: V3.0 - Não deveriamos retornar a exeção
                }
            }
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

        #region GravarArquivoParaEnvio
        /// <summary>
        /// grava um arquivo na pasta de envio
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="Conteudo"></param>
        public void GravarArquivoParaEnvio(string Arquivo, string Conteudo)
        {
            //Gravar o XML
            MemoryStream oMemoryStream = Auxiliar.StringXmlToStream(Conteudo);
            XmlDocument docProc = new XmlDocument();
            docProc.Load(oMemoryStream);
            docProc.Save(ConfiguracaoApp.vPastaXMLEnvio + "\\" + Path.GetFileName(Arquivo));
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

        #region LerArquivo()
        /// <summary>
        /// Le arquivos no formato TXT
        /// Retorna uma lista do conteudo do arquivo
        /// </summary>
        /// <param name="cArquivo"></param>
        /// <returns></returns>
        public List<string> LerArquivo(string cArquivo)
        {
            List<string> lstRetorno = new List<string>();
            if (File.Exists(cArquivo))
            {
                TextReader txt = new StreamReader(cArquivo);
                try
                {
                    string cLinhaTXT = txt.ReadLine();
                    while (cLinhaTXT != null)
                    {
                        string[] dados = cLinhaTXT.Split('|');
                        if (dados.GetLength(0) == 2)
                        {
                            lstRetorno.Add(cLinhaTXT);
                        }
                        cLinhaTXT = txt.ReadLine();
                    }
                }
                finally
                {
                    txt.Close();
                }
                if (lstRetorno.Count == 0)
                    throw new Exception("Arquivo: " + cArquivo + " vazio");
            }
            return lstRetorno;
        }
        #endregion

        #region LerTag()
        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo com um ponto e vírgula no final do conteúdo.
        /// </summary>
        /// <param name="Elemento">Elemento a ser pesquisado o Nome da TAG</param>
        /// <param name="NomeTag">Nome da Tag</param>
        /// <returns>Conteúdo da tag</returns>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public string LerTag(XmlElement Elemento, string NomeTag)
        {
            return this.LerTag(Elemento, NomeTag, true);
        }
        #endregion

        #region LerTag()
        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo, com ou sem um ponto e vírgula no final do conteúdo.
        /// </summary>
        /// <param name="Elemento">Elemento a ser pesquisado o Nome da TAG</param>
        /// <param name="NomeTag">Nome da Tag</param>
        /// <param name="RetornaPontoVirgula">Retorna com ponto e vírgula no final do conteúdo da tag</param>
        /// <returns>Conteúdo da tag</returns>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public string LerTag(XmlElement Elemento, string NomeTag, bool RetornaPontoVirgula)
        {
            string Retorno = string.Empty;

            if (Elemento.GetElementsByTagName(NomeTag).Count != 0)
            {
                if (RetornaPontoVirgula)
                {
                    Retorno = Elemento.GetElementsByTagName(NomeTag)[0].InnerText.Replace(";", " ");  //danasa 19-9-2009
                    Retorno += ";";
                }
                else
                {
                    Retorno = Elemento.GetElementsByTagName(NomeTag)[0].InnerText;  //Wandrey 07/10/2009
                }
            }
            return Retorno;
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
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byteArray = encoding.GetBytes(strXml);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
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
                    string vNomeArquivo = ConfiguracaoApp.vPastaXMLErro + "\\" + this.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;

                    //Deletar o arquivo da pasta de XML com erro se o mesmo existir lá para evitar erros na hora de mover. Wandrey
                    if (File.Exists(vNomeArquivo))
                        this.DeletarArquivo(vNomeArquivo);

                    //Mover o arquivo da nota fiscal para a pasta do XML com erro
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
            this.MoveArqErro(Arquivo, Path.GetExtension(Arquivo));
        }
        #endregion

        #region MoverArquivo()
        public void MoverArquivo(string Arquivo, string strDestinoArquivo)
        {
            if (File.Exists(Arquivo))   //danasa 10-2009
            {
                //Mover o arquivo original para a pasta de destino
                this.DeletarArquivo(strDestinoArquivo);

                //Definir o arquivo que vai ser deletado ou movido para outra pasta
                FileInfo oArquivo = new FileInfo(Arquivo);
                oArquivo.MoveTo(strDestinoArquivo);
            }
        }
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
            try
            {
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
                        strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
                        strDestinoArquivo = strNomePastaEnviado + "\\" + this.ExtrairNomeArq(Arquivo, ".xml") + ".xml";
                        goto default;

                    case PastaEnviados.Denegados:
                        strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
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
                                    strNomePastaBackup = ConfiguracaoApp.cPastaBackup + "\\" + PastaEnviados.Autorizados + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
                                    goto default;

                                case PastaEnviados.Denegados:
                                    strNomePastaBackup = ConfiguracaoApp.cPastaBackup + "\\" + PastaEnviados.Denegados + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
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
                                throw new Exception("Pasta de backup informada nas configurações não existe. (Pasta: " + strNomePastaBackup + ")");
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Pasta para arquivamento dos XML´s enviados não existe. (Pasta: " + strNomePastaEnviado + ")");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
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
            try
            {
                this.MoverArquivo(Arquivo, SubPastaXMLEnviado, DateTime.Now);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region RenomearArquivo
        // danasa 10-2009
        public void RenomearArquivo(string oldFileName, string newFileName)
        {
            this.DeletarArquivo(newFileName);
            if (File.Exists(oldFileName))
            {
                FileInfo oArquivo = new FileInfo(oldFileName);
                oArquivo.MoveTo(newFileName);
            }
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
                    this.GravarXMLRetornoValidacao(Arquivo, "2", "Ocorreu um erro ao assinar o XML: " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
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

        #region VerStatusServico() e ConsultaCadastro()

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
            string ArqXMLRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                      this.ExtrairNomeArq(XmlNfeDadosMsg, ExtXml.PedSta) +
                      "-sta.xml";

            string ArqERRRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                      this.ExtrairNomeArq(XmlNfeDadosMsg, ExtXml.PedSta) +
                      "-sta.err";

            string result = string.Empty;

            try
            {
                result = (string)EnviaArquivoERecebeResposta(ArqXMLRetorno, ArqERRRetorno, ProcessaStatusServico);
            }
            finally
            {
                this.DeletarArquivo(ArqERRRetorno);
                this.DeletarArquivo(ArqXMLRetorno);
            }

            return result;
        }

        /// <summary>
        /// Função Callback que analisa a resposta do Status do Servido
        /// </summary>
        /// <param name="elem"></param>
        /// <by>Marcos Diez</by>
        /// <date>30/8/2009</date>
        /// <returns></returns>
        private static string ProcessaStatusServico(string cArquivoXML)//XmlTextReader elem)
        {
            string rst = "Erro na leitura do XML " + cArquivoXML;
            XmlTextReader elem = new XmlTextReader(cArquivoXML);
            try
            {
                while (elem.Read())
                {
                    if (elem.NodeType == XmlNodeType.Element)
                    {
                        if (elem.Name == "xMotivo")
                        {
                            elem.Read();
                            rst = elem.Value;
                            break;
                        }
                    }
                }
            }
            finally
            {
                elem.Close();
            }

            return rst;
        }

        /// <summary>
        /// VerConsultaCadastroClass
        /// </summary>
        /// <param name="XmlNfeDadosMsg"></param>
        /// <returns></returns>
        public object VerConsultaCadastroClass(string XmlNfeDadosMsg)
        {
            string ArqXMLRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                       this.ExtrairNomeArq(XmlNfeDadosMsg, ExtXml.ConsCad) +
                       "-ret-cons-cad.xml";

            string ArqERRRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                      this.ExtrairNomeArq(XmlNfeDadosMsg, ExtXml.ConsCad) +
                      "-ret-cons-cad.err";

            object vRetorno = null;
            try
            {
                vRetorno = EnviaArquivoERecebeResposta(ArqXMLRetorno, ArqERRRetorno, ProcessaConsultaCadastroClass);
                //vRetorno = ProcessaConsultaCadastroClass(@"c:\usr\nfe\uninfe\modelos\retorno-cons-cad.txt");
            }
            finally
            {
                this.DeletarArquivo(ArqERRRetorno);
                this.DeletarArquivo(ArqXMLRetorno);
            }
            return vRetorno;
        }

        private static DateTime getDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;

            int _ano = Convert.ToInt16(value.Substring(0, 4));
            int _mes = Convert.ToInt16(value.Substring(5, 2));
            int _dia = Convert.ToInt16(value.Substring(8, 2));
            if (value.Contains("T") && value.Contains(":"))
            {
                int _hora = Convert.ToInt16(value.Substring(11, 2));
                int _min = Convert.ToInt16(value.Substring(14, 2));
                int _seg = Convert.ToInt16(value.Substring(17, 2));
                return new DateTime(_ano, _mes, _dia, _hora, _min, _seg);
            }
            return new DateTime(_ano, _mes, _dia);
        }

        /// <summary>
        /// utilizada pela GerarXML
        /// </summary>
        /// <param name="msXml"></param>
        /// <returns></returns>
        private static RetConsCad ProcessaConsultaCadastro(XmlDocument doc)
        {
            if (doc.GetElementsByTagName("infCad") == null)
                return null;

            RetConsCad vRetorno = new RetConsCad();

            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name == "retConsCad")
                {
                    foreach (XmlNode noderetConsCad in node.ChildNodes)
                    {
                        if (noderetConsCad.Name == "infCons")
                        {
                            foreach (XmlNode nodeinfCons in noderetConsCad.ChildNodes)
                            {
                                if (nodeinfCons.Name == "infCad")
                                {
                                    vRetorno.infCad.Add(new infCad());
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].CNPJ = vRetorno.CNPJ;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].CPF = vRetorno.CPF;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].IE = vRetorno.IE;
                                    vRetorno.infCad[vRetorno.infCad.Count - 1].UF = vRetorno.UF;

                                    foreach (XmlNode nodeinfCad in nodeinfCons.ChildNodes)
                                    {
                                        switch (nodeinfCad.Name)
                                        {
                                            case "UF":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].UF = nodeinfCad.InnerText;
                                                break;
                                            case "IE":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].IE = nodeinfCad.InnerText;
                                                break;
                                            case "CNPJ":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].CNPJ = nodeinfCad.InnerText;
                                                break;
                                            case "CPF":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].CNPJ = nodeinfCad.InnerText;
                                                break;
                                            case "xNome":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xNome = nodeinfCad.InnerText;
                                                break;
                                            case "xFant":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xFant = nodeinfCad.InnerText;
                                                break;

                                            case "ender":
                                                foreach (XmlNode nodeinfConsEnder in nodeinfCad.ChildNodes)
                                                {
                                                    switch (nodeinfConsEnder.Name)
                                                    {
                                                        case "xLgr":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xLgr = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "nro":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.nro = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "xCpl":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xCpl = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "xBairro":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xBairro = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "xMun":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.xMun = nodeinfConsEnder.InnerText;
                                                            break;
                                                        case "cMun":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.cMun = Convert.ToInt32("0" + nodeinfConsEnder.InnerText);
                                                            break;
                                                        case "CEP":
                                                            vRetorno.infCad[vRetorno.infCad.Count - 1].ender.CEP = Convert.ToInt32("0" + nodeinfConsEnder.InnerText);
                                                            break;
                                                    }
                                                }
                                                break;

                                            case "IEAtual":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].IEAtual = nodeinfCad.InnerText;
                                                break;
                                            case "IEUnica":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].IEUnica = nodeinfCad.InnerText;
                                                break;
                                            case "dBaixa":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dBaixa = getDateTime(nodeinfCad.InnerText);
                                                break;
                                            case "dUltSit":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dUltSit = getDateTime(nodeinfCad.InnerText);
                                                break;
                                            case "dIniAtiv":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].dIniAtiv = getDateTime(nodeinfCad.InnerText);
                                                break;
                                            case "CNAE":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].CNAE = Convert.ToInt32("0" + nodeinfCad.InnerText);
                                                break;
                                            case "xRegApur":
                                                vRetorno.infCad[vRetorno.infCad.Count - 1].xRegApur = nodeinfCad.InnerText;
                                                break;
                                            case "cSit":
                                                if (nodeinfCad.InnerText == "0")
                                                    vRetorno.infCad[vRetorno.infCad.Count - 1].cSit = "Contribuinte não habilitado";
                                                else if (nodeinfCad.InnerText == "1")
                                                    vRetorno.infCad[vRetorno.infCad.Count - 1].cSit = "Contribuinte habilitado";
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch (nodeinfCons.Name)
                                    {
                                        case "cStat":
                                            vRetorno.cStat = Convert.ToInt32("0" + nodeinfCons.InnerText);
                                            break;
                                        case "xMotivo":
                                            vRetorno.xMotivo = nodeinfCons.InnerText;
                                            break;
                                        case "UF":
                                            vRetorno.UF = nodeinfCons.InnerText;
                                            break;
                                        case "IE":
                                            vRetorno.IE = nodeinfCons.InnerText;
                                            break;
                                        case "CNPJ":
                                            vRetorno.CNPJ = nodeinfCons.InnerText;
                                            break;
                                        case "CPF":
                                            vRetorno.CPF = nodeinfCons.InnerText;
                                            break;
                                        case "dhCons":
                                            vRetorno.dhCons = getDateTime(nodeinfCons.InnerText);
                                            break;
                                        case "cUF":
                                            vRetorno.cUF = Convert.ToInt32("0" + nodeinfCons.InnerText);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return vRetorno;
        }
        public RetConsCad ProcessaConsultaCadastroClass(MemoryStream msXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(msXml);
            return ProcessaConsultaCadastro(doc);
        }
        /// <summary>
        /// Função Callback que analisa a resposta do Status do Servido
        /// </summary>
        /// <param name="elem"></param>
        /// <by>Marcos Diez</by>
        /// <date>30/8/2009</date>
        /// <returns></returns>
        /// <summary>
        /// utilizada internamente pela VerConsultaCadastroClass
        /// </summary>
        /// <param name="cArquivoXML"></param>
        /// <returns></returns>
        private static RetConsCad ProcessaConsultaCadastroClass(string cArquivoXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(cArquivoXML);
            return ProcessaConsultaCadastro(doc);
        }

        /// <summary>
        /// Escopo do CalBack de analise de resposta da EnviarArquivoEReceberResposta
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        delegate object Processa(string cArquivoXML);

        /// <summary>
        /// Envia um arquivo para o webservice da NFE e recebe a resposta. 
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2009</date>
        private object EnviaArquivoERecebeResposta(string ArqXMLRetorno, string ArqERRRetorno, Processa processa)
        {
            object vStatus = "Ocorreu uma falha ao tentar obter a situação do serviço junto ao SEFAZ.\r\n\r\n"+
                "O problema pode ter ocorrido por causa dos seguintes fatores:\r\n\r\n"+
                "- Problema com o certificado digital\r\n"+
                "- Necessidade de atualização da cadeia de certificados digitais\r\n"+
                "- Falha de conexão com a internet\r\n"+
                "- Falha nos servidores do SEFAZ\r\n\r\n"+
                "Afirmamos que a produtora do software não se responsabiliza por decisões tomadas e/ou execuções realizadas com base nas informações acima.\r\n\r\n";
            
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
                    if (!Auxiliar.FileInUse(ArqXMLRetorno))
                    {
                        try
                        {
                            //Ler o status do serviço no XML retornado pelo WebService
                            //XmlTextReader oLerXml = new XmlTextReader(ArqXMLRetorno);

                            try
                            {
                                vStatus = processa(ArqXMLRetorno);// oLerXml);
                            }
                            catch (Exception ex)
                            {
                                vStatus = ex.Message;
                                break;
                                //Se não conseguir ler o arquivo vai somente retornar ao loop para tentar novamente, pois 
                                //pode ser que o arquivo esteja em uso ainda.
                            }

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
                        catch
                        {
                        }
                    }
                }
                else if (File.Exists(ArqERRRetorno))
                {
                    //Retornou um arquivo com a extensão .ERR, ou seja, deu um erro,
                    //futuramente tem que retornar esta mensagem para a MessageBox do usuário.

                    //Detetar o arquivo de retorno
                    try
                    {
                        vStatus += System.IO.File.ReadAllText(ArqERRRetorno, Encoding.Default);
                        System.IO.File.Delete(ArqERRRetorno);
                        break;
                    }
                    catch
                    {
                        //Somente deixa fazer o loop novamente e tentar deletar
                    }
                }
                Thread.Sleep(3000);
            }
            //Retornar o status do serviço
            return vStatus;
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

        #region EstaAutorizada()
        /// <summary>
        /// Verifica se o XML de Distribuição da Nota Fiscal (-procNFe) já está na pasta de Notas Autorizadas
        /// </summary>
        /// <param name="Arquivo">Arquivo XML a ser verificado</param>
        /// <param name="Emissao">Data de emissão da NFe</param>
        /// <param name="Extensao">Extensão a ser verificada (ExtXml.Nfe ou ExtXmlRet.ProcNFe)</param>
        /// <returns>Se está na pasta de XML´s autorizados</returns>
        public bool EstaAutorizada(string Arquivo, DateTime Emissao, string Extensao)
        {
            string strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + "\\" + this.ExtrairNomeArq(Arquivo, ExtXml.Nfe) + Extensao);
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
            string strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + "\\" + this.ExtrairNomeArq(Arquivo, ExtXml.Nfe) + "-den.xml");
        }
        #endregion

        #region ExecutaUniDanfe()
        /// <summary>
        /// Executa o aplicativo UniDanfe para gerar/imprimir o DANFE
        /// </summary>
        /// <param name="NomeArqXMLNFe">Nome do arquivo XML da NFe (final -nfe.xml)</param>
        /// <param name="DataEmissaoNFe">Data de emissão da NFe</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/02/2010</date>
        public void ExecutaUniDanfe(string NomeArqXMLNFe, DateTime DataEmissaoNFe)
        {
            //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
            if (ConfiguracaoApp.PastaExeUniDanfe != string.Empty && 
                File.Exists(ConfiguracaoApp.PastaExeUniDanfe + "\\unidanfe.exe") &&
                File.Exists(NomeArqXMLNFe))
            {
                string strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(DataEmissaoNFe);
                string strArqProcNFe = strNomePastaEnviado + "\\" + this.ExtrairNomeArq(this.ExtrairNomeArq(NomeArqXMLNFe, ExtXml.Nfe) + ExtXmlRet.ProcNFe, ".xml") + ".xml";

                string Args = "A=\"" + strArqProcNFe + "\"";
                if (ConfiguracaoApp.PastaConfigUniDanfe != string.Empty)
                {
                    Args += " PC=\"" + ConfiguracaoApp.PastaConfigUniDanfe + "\"";
                }

                System.Diagnostics.Process.Start(ConfiguracaoApp.PastaExeUniDanfe + "\\unidanfe.exe", Args);
            }
        }
        #endregion
    }

    #region infCad & RetConsCad
    public class enderConsCadInf
    {
        public string xLgr { get; set; }
        public string nro { get; set; }
        public string xCpl { get; set; }
        public string xBairro { get; set; }
        public int cMun { get; set; }
        public string xMun { get; set; }
        public int CEP { get; set; }
    }
    public class infCad
    {
        public string IE { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string xNome { get; set; }
        public string xFant { get; set; }
        public string IEAtual { get; set; }
        public string IEUnica { get; set; }
        public DateTime dBaixa { get; set; }
        public DateTime dUltSit { get; set; }
        public DateTime dIniAtiv { get; set; }
        public int CNAE { get; set; }
        public string xRegApur { get; set; }
        public string cSit { get; set; }
        public enderConsCadInf ender { get; set; }

        public infCad()
        {
            ender = new enderConsCadInf();
        }
    }

    public class RetConsCad
    {
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public string UF { get; set; }
        public string IE { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public DateTime dhCons { get; set; }
        public Int32 cUF { get; set; }
        public List<infCad> infCad { get; set; }

        public RetConsCad()
        {
            infCad = new List<infCad>();
        }
    }
    #endregion
}

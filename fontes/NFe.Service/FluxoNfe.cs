using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using NFe.Settings;
using NFe.Components;

namespace NFe.Service
{
    #region Classe FluxoNfe
    /// <summary>
    /// Classe de controle do fluxo das notas fiscais eletrônicas que estão em processo de envio
    /// </summary>
    public class FluxoNfe
    {
        /// <summary>
        /// Código da empresa
        /// </summary>
        private int empresa { get; set; }

        #region Construtores
        public FluxoNfe()
        {
            int emp = Empresas.FindEmpresaByThread();
            empresa = emp;

            NomeXmlControleFluxo = Empresas.Configuracoes[emp].PastaEmpresa + "\\fluxonfe.xml";
        }

        public FluxoNfe(int emp)
        {
            empresa = emp;
            NomeXmlControleFluxo = Empresas.Configuracoes[emp].PastaEmpresa + "\\fluxonfe.xml";
        }
        #endregion

        #region Enumeradores

        #region ElementoEditavel
        /// <summary>
        /// Enumerador das tag´s editáveis do XML de controle do fluxo
        /// </summary>
        public enum ElementoEditavel
        {
            /// <summary>
            /// Tag que contém o número do lote
            /// </summary>
            idLote,
            /// <summary>
            /// Tag que contém o número do Recibo
            /// </summary>
            nRec,
            /// <summary>
            /// Tag que contém o status da NFe
            /// </summary>
            cStat,
            /// <summary>
            /// Data e hora da ultima consulta do recibo do lote de nfe enviado
            /// </summary>
            dPedRec,
            /// <summary>
            /// Tempo médio de resposta da SEFAZ
            /// </summary>
            tMed,
            /// <summary>
            /// Versão de schema do XML
            /// </summary>
            versao,
            /// <summary>
            /// Modelo do documento fiscal
            /// </summary>
            mod
        }
        #endregion

        #region ElementoFixo
        /// <summary>
        /// Enumerador das tag´s e atributos fixos do XML de controle do fluxo
        /// </summary>
        public enum ElementoFixo
        {
            /// <summary>
            /// Tag principal Documentos NFe
            /// </summary>
            DocumentosNFe,
            /// <summary>
            /// Tag Documento - Uma para cada NFe em processamento
            /// </summary>
            Documento,
            /// <summary>
            /// Tag com a ChaveNFe
            /// </summary>
            ChaveNFe,
            /// <summary>
            /// Tag com o nome do arquivo NFe
            /// </summary>
            ArqNFe
        }
        #endregion

        #endregion

        #region Propriedades
        /// <summary>
        /// Nome do arquivo XML onde é gravado o controle do fluxo
        /// </summary>
        private string NomeXmlControleFluxo { get; set; }
        #endregion

        #region métodos gerais

        #region CriarXml()
        public void CriarXml()
        {
            CriarXml(false);
        }
        #endregion

        #region CriarXml()
        /// <summary>
        /// Cria o arquivo XML para o controle do fluxo
        /// </summary>
        /// <param name="forcar">Força criar o arquivo mesmo que já exista</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/04/2009</date>
        private void CriarXml(bool VerificaEstruturaXml)
        {
            while (true)
            {
                lock (Smf.Fluxo)
                {
                    XmlWriter xtw = null; // criar instância para xmltextwriter. 
                    try
                    {
                        #region Testar para ver se o XML não tá danificado, ou seja, sem as tag´s iniciais, se tiver força recriar ele
                        bool ForcarCriar = false;
                        if (VerificaEstruturaXml)
                        {
                            XmlDocument doc = null;
                            FileStream fsArquivo = null;
                            try
                            {
                                fsArquivo = OpenFileFluxo(true); //Abrir um arquivo XML usando FileStream

                                if (File.Exists(NomeXmlControleFluxo))
                                {
                                    doc = new XmlDocument();
                                    doc.Load(NomeXmlControleFluxo);
                                }
                            }
                            catch
                            {
                                if (doc != null)
                                {
                                    if (doc.DocumentElement == null)
                                    {
                                        ForcarCriar = true;
                                    }
                                }
                            }
                            finally
                            {
                                if (fsArquivo != null)
                                {
                                    fsArquivo.Close();
                                }
                            }
                        }
                        #endregion

                        #region Criar arquivo
                        if (!File.Exists(NomeXmlControleFluxo) || ForcarCriar)
                        {
                            ///
                            /// danasa 20-9-2010
                            /// 
                            bool goCriaArquivoDeFluxo = true;
                            if (File.Exists(NomeXmlControleFluxo))
                                if (Functions.FileInUse(NomeXmlControleFluxo))
                                    ///
                                    /// O metodo "BuscarXML" acessa o metodo para criar o xml de fluxo, só que como ele é acessado várias vezes
                                    /// e como o arquivo está sendo criado, é exibida várias mensagens de erro de acesso ao arquivo de fluxo
                                    goCriaArquivoDeFluxo = false;

                            if (goCriaArquivoDeFluxo)
                            {
                                XmlWriterSettings oSettings = new XmlWriterSettings();
                                UTF8Encoding c = new UTF8Encoding(false);

                                oSettings.Encoding = c;
                                oSettings.Indent = true;
                                oSettings.IndentChars = "";
                                oSettings.NewLineOnAttributes = false;
                                oSettings.OmitXmlDeclaration = false;

                                xtw = XmlWriter.Create(NomeXmlControleFluxo, oSettings); //atribuir arquivo, caminho e codificação 
                                xtw.WriteStartDocument(); //comaçar a escrever o documento 
                                xtw.WriteStartElement(ElementoFixo.DocumentosNFe.ToString()); //Criar elemento raiz
                                xtw.WriteEndElement(); //encerrar tag DocumentosNFe
                                xtw.Flush();
                            }
                        }
                        #endregion
                    }
                    finally
                    {
                        if (xtw != null)
                        {
                            if (xtw.WriteState != WriteState.Closed)
                            {
                                xtw.Close(); //Fechar o arquivo e salvar
                            }
                        }
                    }

                    break;
                }
            }
        }
        #endregion

        #region InserirNfeFluxo()
        /// <summary>
        /// Insere a NFe no fluxo em processo
        /// </summary>
        /// <param name="strChaveNFe">Chave da NFe</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/04/2009</date>
        public void InserirNfeFluxo(string strChaveNFe, string mod, string fullPathNFe)
        {
            string nomeArqNFe = Functions.ExtrairNomeArq(fullPathNFe, ".xml") + ".xml";

            CriarXml();

            if (!this.NfeExiste(strChaveNFe))
            {
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

                    FileStream lfile = null;
                    try
                    {
                        lock (Smf.Fluxo)
                        {
                            XmlDocument xd = new XmlDocument(); //Criar instância do XmlDocument Class

                            lfile = OpenFileFluxo(false); //Abrir um arquivo XML usando FileStream

                            #region Pegar a versão do schema do XML
                            string versaoXmlNFe = string.Empty;
                            XmlDocument xmlNFe = new XmlDocument();
                            xmlNFe.Load(fullPathNFe);

                            try
                            {
                                if (((XmlElement)(XmlNode)xmlNFe.GetElementsByTagName(xmlNFe.DocumentElement.Name)[0]).Attributes[NFe.Components.TpcnResources.versao.ToString()] != null)
                                    versaoXmlNFe = ((XmlElement)(XmlNode)xmlNFe.GetElementsByTagName(xmlNFe.DocumentElement.Name)[0]).Attributes[NFe.Components.TpcnResources.versao.ToString()].Value;
                                else if (((XmlElement)(XmlNode)xmlNFe.GetElementsByTagName(xmlNFe.DocumentElement.FirstChild.Name)[0]).Attributes[NFe.Components.TpcnResources.versao.ToString()] != null)
                                    versaoXmlNFe = ((XmlElement)(XmlNode)xmlNFe.GetElementsByTagName(xmlNFe.DocumentElement.FirstChild.Name)[0]).Attributes[NFe.Components.TpcnResources.versao.ToString()].Value;
                            }
                            catch
                            {
                                throw new Exception("Ocorreu uma falha na leitura da versão do arquivo XML na hora de inseri-lo no fluxo de envio: " + fullPathNFe);
                            }
                            #endregion

                            xd.Load(lfile); //Carregar o arquivo aberto no XmlDocument
                            XmlElement cl = xd.CreateElement(ElementoFixo.Documento.ToString()); //Criar um Elemento chamado Documento
                            cl.SetAttribute(ElementoFixo.ChaveNFe.ToString(), strChaveNFe); // Setar atributo para o Elemento Documento

                            //Tag ArqNFeAssinado
                            this.CriarTag(xd, cl, ElementoFixo.ArqNFe.ToString(), nomeArqNFe);

                            //Tag idLote
                            this.CriarTag(xd, cl, ElementoEditavel.idLote.ToString(), string.Empty);

                            //Tag nRec
                            this.CriarTag(xd, cl, ElementoEditavel.nRec.ToString(), string.Empty);

                            //Tag cStat
                            this.CriarTag(xd, cl, ElementoEditavel.cStat.ToString(), string.Empty);

                            //Tag tMed
                            this.CriarTag(xd, cl, ElementoEditavel.tMed.ToString(), string.Empty);

                            //Tag dPedRec
                            this.CriarTag(xd, cl, ElementoEditavel.dPedRec.ToString(), DateTime.Now.ToString());

                            //tag versao
                            this.CriarTag(xd, cl, ElementoEditavel.versao.ToString(), versaoXmlNFe);

                            //tag mod
                            this.CriarTag(xd, cl, ElementoEditavel.mod.ToString(), mod);

                            //Fechar o arquovo e gravar o conteúdo no HD
                            lfile.Close(); //Fechar o FileStream
                            xd.Save(NomeXmlControleFluxo); //Salvar o conteudo do XmlDocument para o arquivo  
                            break;
                        }
                    }
                    catch
                    {
                        if (lfile != null)
                        {
                            lfile.Close();
                        }

                        if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minuto
                        {
                            throw;
                        }
                    }

                    Thread.Sleep(100);
                }
            }
        }
        #endregion

        #region CriarTag()
        /// <summary>
        /// Criar Tag no XML de fluxo
        /// </summary>
        /// <param name="xd">Objeto XmlDocument</param>
        /// <param name="cl">Objeto XmlElement</param>
        /// <param name="strTag">Nome da Tag</param>
        /// <param name="strConteudo">Conteúdo da Tag</param>
        private void CriarTag(XmlDocument xd, XmlElement cl, string strTag, string strConteudo)
        {
            XmlElement na = xd.CreateElement(strTag);
            XmlText natext = xd.CreateTextNode(strConteudo);
            na.AppendChild(natext); //Gravar o texto da unidade para o nó Unidade
            cl.AppendChild(na); //Gravar nó Unidade para o elemento Produto
            xd.DocumentElement.AppendChild(cl); //Gravar o elemento raiz para o XmlDocument
        }
        #endregion

        #region ExcluirNfeFluxo()
        /// <summary>
        /// Excluir a NFe do fluxo em processamento através da chave da NFe
        /// </summary>
        /// <param name="strChaveNFe">Chave da NFe</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/04/2009</date>
        public void ExcluirNfeFluxo(string strChaveNFe)
        {
            CriarXml();

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
                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument xdXml = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false); //Abrir um arquivo XML usando FileStream
                        xdXml.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        bool removeu = false;
                        XmlNodeList list = xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        for (int i = 0; i < list.Count; i++) //Navegar em todos os elementos do nó Documento
                        {
                            XmlElement cl = (XmlElement)xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString())[i]; //Recuperar o conteúdo da tag Documento
                            if (cl.GetAttribute(ElementoFixo.ChaveNFe.ToString()) == strChaveNFe)
                            {
                                xdXml.DocumentElement.RemoveChild(cl); //Remove o elemento do documento
                                removeu = true;
                            }
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML

                        //Só gravo se removeu algo ou vou manter o arquivo como estava
                        if (removeu)
                            xdXml.Save(NomeXmlControleFluxo); //Grava o arquivo XML                    

                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }
        }
        #endregion

        #region ExcluirNfeFluxoRec()
        /// <summary>
        /// Excluir as NFe´s no fluxo através do recibo. Ótimo para retirar todas as notas de um único lote de uma única vez.
        /// </summary>
        /// <param name=TpcnResources.nRec.ToString()>Número do recibo do lote enviado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Date: 20/07/2010
        /// </remarks>
        public void ExcluirNfeFluxoRec(string nRec)
        {
            CriarXml();

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
                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument xdXml = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false); //Abrir um arquivo XML usando FileStream
                        xdXml.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        XmlNodeList documentosList = xdXml.GetElementsByTagName(ElementoFixo.DocumentosNFe.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        foreach (XmlNode documentosNode in documentosList)
                        {
                            XmlElement documentosElemento = (XmlElement)documentosNode;

                            List<XmlNode> nodeExcluir = new List<XmlNode>();

                            XmlNodeList documentoList = documentosElemento.GetElementsByTagName(ElementoFixo.Documento.ToString());
                            for (int i = 0; i < documentoList.Count; i++)
                            {
                                var documentoNode = documentoList[i];
                                var documentoElemento = (XmlElement)documentoNode;
                                var tagRec = documentoElemento.GetElementsByTagName(ElementoEditavel.nRec.ToString())[0].InnerText.Trim(); //Recupera o conteúdo da tag de nRec
                                if (tagRec == nRec)
                                {
                                    nodeExcluir.Add(documentoNode);
                                }
                            }

                            for (int i = 0; i < nodeExcluir.Count; i++)
                            {
                                xdXml.DocumentElement.RemoveChild(nodeExcluir[i]);
                            }
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML
                        xdXml.Save(NomeXmlControleFluxo); //Grava o arquivo XML                
                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }
        }
        #endregion

        #region NfeExiste()
        /// <summary>
        /// Verifica se a NFe já existe no arquivo XML de controle do fluxo.
        /// </summary>
        /// <returns>true = Existe</returns>
        public bool NfeExiste(string strChaveNFe)
        {
            CriarXml();

            bool booExiste = false;

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
                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument xdXml = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false);
                        xdXml.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        XmlNodeList list = xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        for (int i = 0; i < list.Count; i++) //Navegar em todos os elementos do nó Documento
                        {
                            XmlElement cl = (XmlElement)xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString())[i]; //Recuperar o conteúdo da tag Documento
                            if (cl.GetAttribute(ElementoFixo.ChaveNFe.ToString()) == strChaveNFe)
                                booExiste = true;
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML

                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                        fsArquivo.Close();

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde Ã¡ 120 segundos que corresponde a 2 minuto
                        throw;
                }

                Thread.Sleep(100);
            }

            return booExiste;
        }
        #endregion

        #region NFeComLote()
        /// <summary>
        /// Verifica se a NFE já foi incluida em um lote de NFe
        /// </summary>
        /// <param name="strChaveNFe">Chave da Nota Fiscal Eletrônica a ser Verificado</param>
        /// <returns>true = Já está em um lote</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public bool NFeComLote(string strChaveNFe)
        {
            bool booComLote = false;

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
                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument xdXml = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false);
                        xdXml.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        XmlNodeList list = xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        for (int i = 0; i < list.Count; i++) //Navegar em todos os elementos do nó Documento
                        {
                            XmlElement cl = (XmlElement)xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString())[i]; //Recuperar o conteúdo da tag Documento
                            XmlElement xeTagLote = (XmlElement)xdXml.GetElementsByTagName(ElementoEditavel.idLote.ToString())[i]; //Recupera o conteúdo da tag de Lote
                            if (cl.GetAttribute(ElementoFixo.ChaveNFe.ToString()) == strChaveNFe)
                            {
                                if (xeTagLote.InnerText != string.Empty)
                                {
                                    booComLote = true;
                                }
                            }
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML
                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde Ã¡ 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }

            return booComLote;
        }
        #endregion

        #region AtualizarTag()
        /// <summary>
        /// Atualizar o conteúdo das Tag´s do XML de controle do Fluxo
        /// </summary>
        /// <param name="chaveNFe">Chave da NFe</param>
        /// <param name="tag">Tag a ser atualizada</param>
        /// <param name="novoConteudo">Novo conteúdo para a tag</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/04/2009</date>
        public void AtualizarTag(string chaveNFe, ElementoEditavel tag, string novoConteudo)
        {
            CriarXml();

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

                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument xdXml = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false); //Abrir um arquivo XML usando FileStream
                        xdXml.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        XmlNodeList list = xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        for (int i = 0; i < list.Count; i++) //Navegar em todos os elementos do nó Documento
                        {
                            XmlElement xeDoc = (XmlElement)xdXml.GetElementsByTagName(ElementoFixo.Documento.ToString())[i]; //Recuperar o conteúdo da tag Produto
                            XmlElement xeTag = (XmlElement)xdXml.GetElementsByTagName(tag.ToString())[i]; //Recuperar o conteúdo da tag
                            if (xeDoc.GetAttribute(ElementoFixo.ChaveNFe.ToString()) == chaveNFe)
                            {
                                if (xeTag != null)
                                {
                                    xeTag.InnerText = novoConteudo; //Setar o novo conteúdo para a tag
                                }
                                break;
                            }
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML
                        xdXml.Save(NomeXmlControleFluxo); //Grava o arquivo xml

                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }
        }
        #endregion

        #region AtualizarTagRec()
        /// <summary>
        /// Atualiza a tag nRec de todas as NFe´s do lote passado por parâmetro
        /// </summary>
        /// <param name="strLote">Lote que é para atualziar o número do recibo</param>
        /// <param name="strNovoConteudo">Número do recibo</param>
        public void AtualizarTagRec(string strLote, string strRecibo)
        {
            CriarXml();

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

                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false);
                        doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument
                        fsArquivo.Close();

                        XmlNodeList documentosList = doc.GetElementsByTagName(ElementoFixo.DocumentosNFe.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        foreach (XmlNode documentosNode in documentosList)
                        {
                            XmlElement documentosElemento = (XmlElement)documentosNode;

                            XmlNodeList documentoList = documentosElemento.GetElementsByTagName(ElementoFixo.Documento.ToString());
                            foreach (XmlNode documentoNode in documentoList)
                            {
                                XmlElement documentoElemento = (XmlElement)documentoNode;

                                string strChaveNFe = string.Empty;
                                if (documentoElemento.HasAttributes)
                                {
                                    strChaveNFe = documentoElemento.Attributes["ChaveNFe"].InnerText;
                                }

                                if (documentoElemento.GetElementsByTagName(TpcnResources.idLote.ToString())[0].InnerText == strLote)
                                {
                                    AtualizarTag(strChaveNFe, ElementoEditavel.dPedRec, DateTime.Now.AddSeconds(20).ToString());
                                    AtualizarTag(strChaveNFe, ElementoEditavel.nRec, strRecibo);
                                }
                            }
                        }

                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }
        }
        #endregion

        #region LerTag()
        /// <summary>
        /// Ler conteúdo da Tag de uma determinada NFe que já está no controle de fluxo de notas sendo enviadas
        /// </summary>
        /// <param name="strChaveNFe">Chave da NFe que é para ler a tag</param>
        /// <param name="Tag">Nome da tag a ser lida</param>
        /// <returns>Retorna o conteúdo da TAG</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private string LerTag(string strChaveNFe, string Tag)
        {
            CriarXml();

            string strConteudo = string.Empty;

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
                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false);

                        doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        XmlNodeList documentoList = doc.GetElementsByTagName(ElementoFixo.Documento.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        foreach (XmlNode documentoNode in documentoList)
                        {
                            XmlElement documentoElemento = (XmlElement)documentoNode;

                            if (documentoElemento.GetAttribute(ElementoFixo.ChaveNFe.ToString()) == strChaveNFe)
                            {
                                if (documentoElemento.GetElementsByTagName(Tag)[0] != null) //null significa que não encontrou a TAG, comparo para evitar erros
                                {
                                    strConteudo = documentoElemento.GetElementsByTagName(Tag)[0].InnerText;
                                }

                                break;
                            }
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML

                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde Ã¡ 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }

            return strConteudo;
        }
        #endregion

        #region LerTag() - Sobrecarga
        /// <summary>
        /// Ler conteúdo da Tag de uma determinada NFe que já está no controle de fluxo de notas sendo enviadas
        /// </summary>
        /// <param name="strChaveNFe">Chave da NFe que é para ler a tag</param>
        /// <param name="Tag">Nome da tag a ser lida</param>
        /// <returns>Retorna o conteúdo da TAG</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public string LerTag(string strChaveNFe, ElementoEditavel Tag)
        {
            string strConteudo = string.Empty;
            strConteudo = LerTag(strChaveNFe, Tag.ToString());
            return strConteudo;
        }
        #endregion

        #region LerTag() - Sobrecarga
        /// <summary>
        /// Ler conteúdo da Tag de uma determinada NFe que já está no controle de fluxo de notas sendo enviadas
        /// </summary>
        /// <param name="strChaveNFe">Chave da NFe que é para ler a tag</param>
        /// <param name="Tag">Nome da tag a ser lida</param>
        /// <returns>Retorna o conteúdo da TAG</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public string LerTag(string strChaveNFe, ElementoFixo Tag)
        {
            string strConteudo = string.Empty;
            strConteudo = LerTag(strChaveNFe, Tag.ToString());
            return strConteudo;
        }
        #endregion

        #region CriarListaRec()
        /// <summary>
        /// Criar uma lista com os recibos a serem consultados no servidor do SEFAZ
        /// </summary>
        /// <returns>Lista dos recibos</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        public List<ReciboCons> CriarListaRec()
        {
            CriarXml(true);

            List<ReciboCons> lstRecibo = new List<ReciboCons>();
            List<string> lstNumRec = new List<string>();

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
                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false);
                        doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument
                        fsArquivo.Close();

                        XmlNodeList documentoList = doc.GetElementsByTagName(ElementoFixo.Documento.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        foreach (XmlNode documentoNode in documentoList)
                        {
                            XmlElement documentoElemento = (XmlElement)documentoNode;

                            string nRec = Functions.LerTag(documentoElemento, ElementoEditavel.nRec.ToString(), "");
                            string versao = Functions.LerTag(documentoElemento, ElementoEditavel.versao.ToString(), "");
                            string ChaveNFe = documentoElemento.GetAttribute(ElementoFixo.ChaveNFe.ToString());
                            string NomeArquivo = Functions.LerTag(documentoElemento, ElementoFixo.ArqNFe.ToString(), "");
                            string NomeArquivoEmProcessamento = Empresas.Configuracoes[empresa].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + NomeArquivo;
                            string NomeArquivoAssinado = Empresas.Configuracoes[empresa].PastaXmlEnvio + "\\temp\\" + NomeArquivo;
                            string NomeArquivoAssinadoLote = Empresas.Configuracoes[empresa].PastaXmlEmLote + "\\temp\\" + NomeArquivo;
                            string mod = Functions.LerTag(documentoElemento, ElementoEditavel.mod.ToString(), "");
                            bool excluiNota = false;
                            if (File.Exists(NomeArquivoEmProcessamento))
                            {
                                int tMed = 3; //3 segundos
                                DateTime dPedRec = DateTime.Now.AddMinutes(-60);

                                tMed = Convert.ToInt32(Functions.LerTag(documentoElemento, ElementoEditavel.tMed.ToString(), tMed.ToString()));
                                dPedRec = Convert.ToDateTime(Functions.LerTag(documentoElemento, ElementoEditavel.dPedRec.ToString(), dPedRec.ToString("yyyy-MM-dd HH:mm:ss")));

                                //Se tiver mais de 2 dias no fluxo, vou excluir a nota dele.
                                //Não faz sentido uma nota ficar no fluxo todo este tempo, então vou fazer uma limpeza
                                //Wandrey 11/09/2009
                                if (DateTime.Now.Subtract(dPedRec).Days >= 2)
                                {
                                    excluiNota = true;
                                }
                                else
                                {
                                    if (nRec != string.Empty && !lstNumRec.Contains(nRec))
                                    {
                                        lstNumRec.Add(nRec);

                                        ReciboCons oReciboCons = new ReciboCons();
                                        oReciboCons.dPedRec = dPedRec;
                                        oReciboCons.nRec = nRec;
                                        oReciboCons.tMed = tMed;
                                        oReciboCons.versao = versao;
                                        oReciboCons.mod = mod;
                                        lstRecibo.Add(oReciboCons);
                                    }
                                }
                            }
                            else if (!File.Exists(NomeArquivoAssinado) && !File.Exists(NomeArquivoAssinadoLote))
                                excluiNota = true;

                            if (excluiNota)
                            {
                                //Deletar o arquivo da pasta em processamento
                                Auxiliar oAux = new Auxiliar();
                                oAux.MoveArqErro(NomeArquivoEmProcessamento);

                                //Deletar a NFE do arquivo de controle de fluxo
                                ExcluirNfeFluxo(ChaveNFe);
                            }
                        }
                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde Ã¡ 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }

            return lstRecibo;
        }
        #endregion

        #region AtualizarTagDPedRec()
        /// <summary>
        /// Atualiza a tag nRec de todas as NFe´s do lote passado por parâmetro
        /// </summary>
        /// <param name="strLote">Lote que é para atualziar o número do recibo</param>
        /// <param name="strNovoConteudo">Número do recibo</param>
        /// <by>Wandrey Mundin Ferreira</by>
        public void AtualizarDPedRec(string strRec, DateTime dtData)
        {
            CriarXml();

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

                FileStream fsArquivo = null;

                try
                {
                    lock (Smf.Fluxo)
                    {
                        XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = OpenFileFluxo(false);
                        doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument
                        fsArquivo.Close();

                        XmlNodeList documentosList = doc.GetElementsByTagName(ElementoFixo.DocumentosNFe.ToString()); //Pesquisar o elemento Documento no arquivo XML
                        foreach (XmlNode documentosNode in documentosList)
                        {
                            XmlElement documentosElemento = (XmlElement)documentosNode;

                            XmlNodeList documentoList = documentosElemento.GetElementsByTagName(ElementoFixo.Documento.ToString());
                            foreach (XmlNode documentoNode in documentoList)
                            {
                                XmlElement documentoElemento = (XmlElement)documentoNode;

                                string strChaveNFe = string.Empty;
                                if (documentoElemento.HasAttributes)
                                {
                                    strChaveNFe = documentoElemento.Attributes["ChaveNFe"].InnerText;
                                }

                                if (documentoElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText == strRec)
                                {
                                    AtualizarTag(strChaveNFe, ElementoEditavel.dPedRec, dtData.ToString());
                                }
                            }
                        }

                        break;
                    }
                }
                catch
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minuto
                    {
                        throw;
                    }
                }

                Thread.Sleep(100);
            }
        }
        #endregion

        #region OpenFileFluxo()
        /// <summary>
        /// Abre o arquivo FluxoNFe.XML com permissão de leitura e gravação, mas só compartilha para leitura
        /// </summary>
        /// <returns>FileStream do arquivo FluxoNFe.XML</returns>
        private FileStream OpenFileFluxo(bool somenteLeitura)
        {
            if (somenteLeitura)
                return new FileStream(NomeXmlControleFluxo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            else
                return new FileStream(NomeXmlControleFluxo, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        }
        #endregion

        #endregion
    }
    #endregion

    #region ReciboCons
    /// <summary>
    /// Classe para auxiliar retornos dos recibos a serem consultados no SEFAZ
    /// </summary>
    public class ReciboCons
    {
        /// <summary>
        /// Número do recibo
        /// </summary>
        public string nRec = string.Empty;
        /// <summary>
        /// Tempo médio de resposta do SEFAZ
        /// </summary>
        public int tMed = 0;
        /// <summary>
        /// Data e hora da ultima consulta do recibo efetuada
        /// </summary>
        public DateTime dPedRec;
        /// <summary>
        /// Versão do Schema do XML da NFe
        /// </summary>
        public string versao;
        /// <summary>
        /// Modelo do documento fiscal
        /// </summary>
        public string mod;
    }
    #endregion
}

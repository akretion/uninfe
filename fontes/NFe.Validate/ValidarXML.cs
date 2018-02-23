using NFe.Certificado;
using NFe.Components;
using NFe.Components.QRCode;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NFe.Validate
{
    /// <summary>
    /// Classe de validação dos XML´s
    /// </summary>
    public class ValidarXML
    {
        #region Construtores

        public ValidarXML(string arquivoXML, int UFCod, bool soValidar)
        {
            TipoArqXml = new TipoArquivoXML(arquivoXML, UFCod, soValidar);
        }

        public ValidarXML(XmlDocument conteudoXML, int UFCod, bool soValidar)
        {
            TipoArqXml = new TipoArquivoXML("", conteudoXML, UFCod, soValidar);
        }

        #endregion Construtores

        public TipoArquivoXML TipoArqXml = null;

        public int Retorno { get; private set; }
        public string RetornoString { get; private set; }

        /// <summary>
        /// Pasta dos schemas para validação do XML
        /// </summary>
        private string PastaSchema = Propriedade.PastaSchemas;

        private string cErro;

        #region EncryptAssinatura()

        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        public void EncryptAssinatura(string arquivoXML)
        {
            if (TipoArqXml.cArquivoSchema.Contains("PAULISTANA") ||
                TipoArqXml.cArquivoSchema.Contains("BLUMENAU") ||
                TipoArqXml.cArquivoSchema.Contains("DSF"))
            {
                if (arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                    arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                {
                    bool found = false;
                    bool bSave = false;
                    string sh1 = "";
                    XmlDocument doc = new XmlDocument();
                    doc.Load(arquivoXML);

                    if (arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                    {
                        const string Assinatura = "Assinatura";

                        XmlNodeList rpsList = doc.GetElementsByTagName("RPS");
                        foreach (XmlNode rpsNode in rpsList)
                        {
                            XmlElement rpsElement = (XmlElement)rpsNode;

                            if (rpsElement.GetElementsByTagName(Assinatura).Count != 0)
                            {
                                found = true;
                                //Encryptar a tag Assinatura
                                int len = TipoArqXml.cArquivoSchema.Contains("DSF") ? 94 : 86;
                                if (rpsElement.GetElementsByTagName(Assinatura)[0].InnerText.Length == len)    //jah assinado?
                                {
                                    bSave = true;
                                    if (TipoArqXml.cArquivoSchema.Contains("DSF"))
                                    {
                                        sh1 = Criptografia.GetSHA1HashData(rpsElement.GetElementsByTagName(Assinatura)[0].InnerText);
                                    }
                                    else
                                    {
                                        if (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado == null)
                                            throw new Exceptions.ExceptionCertificadoDigital(ErroPadrao.CertificadoNaoEncontrado);

                                        sh1 = Criptografia.SignWithRSASHA1(Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado,
                                                rpsElement.GetElementsByTagName(Assinatura)[0].InnerText);
                                    }
                                    rpsElement.GetElementsByTagName(Assinatura)[0].InnerText = sh1;
                                }
                            }
                        }
                        if (!found)
                            throw new Exception("Não foi possivel encontrar a tag <RPS><" + Assinatura + ">");
                    }
                    else if (arquivoXML.EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML, StringComparison.InvariantCultureIgnoreCase) &&
                            !TipoArqXml.cArquivoSchema.Contains("DSF"))
                    {
                        const string AssinaturaCancelamento = "AssinaturaCancelamento";

                        XmlNodeList detalheList = doc.GetElementsByTagName("Detalhe");
                        foreach (XmlNode detalheNode in detalheList)
                        {
                            XmlElement detalheElement = (XmlElement)detalheNode;

                            if (detalheElement.GetElementsByTagName(AssinaturaCancelamento).Count != 0)
                            {
                                found = true;
                                if (detalheElement.GetElementsByTagName(AssinaturaCancelamento)[0].InnerText.Length == 20)
                                {
                                    if (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado == null)
                                        throw new Exceptions.ExceptionCertificadoDigital(ErroPadrao.CertificadoNaoEncontrado);

                                    bSave = true;
                                    //Encryptar a tag Assinatura
                                    sh1 = Criptografia.SignWithRSASHA1(Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado,
                                                    detalheElement.GetElementsByTagName(AssinaturaCancelamento)[0].InnerText);

                                    detalheElement.GetElementsByTagName(AssinaturaCancelamento)[0].InnerText = sh1;
                                }
                            }
                        }
                        if (!found)
                            throw new Exception("Não foi possivel encontrar a tag <Detalhe><" + AssinaturaCancelamento + ">");
                    }
                    //Salvar o XML com as alterações efetuadas
                    if (bSave)
                        doc.Save(arquivoXML);
                }
            }
        }

        #endregion EncryptAssinatura()

        /// <summary>
        /// Método responsável por validar a estrutura do XML de acordo com o schema passado por parâmetro
        /// </summary>
        /// <param name="rotaArqXML">XML a ser validado</param>
        /// <param name="cRotaArqSchema">Schema a ser utilizado na validação</param>
        /// <param name="nsSchema">Namespace contendo a URL do schema</param>
        private void Validar(string rotaArqXML)
        {
            bool lArqXML = File.Exists(rotaArqXML);

            if (File.Exists(rotaArqXML))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(rotaArqXML);
                Validar(doc, rotaArqXML);
            }
            else
            {
                Retorno = 2;
                RetornoString = "Arquivo XML não foi encontrato";
            }
        }

        /// <summary>
        /// Método responsável por validar a estrutura do XML de acordo com o schema passado por parâmetro
        /// </summary>
        /// <param name="rotaArqXML">Nome do arquivo XML</param>
        /// <param name="conteudoXML">Conteúdo do XML a ser validado</param>
        private void Validar(XmlDocument conteudoXML, string rotaArqXML)
        {
            Retorno = 0;
            RetornoString = "";

            bool temXSD = !string.IsNullOrEmpty(TipoArqXml.cArquivoSchema);

            if (File.Exists(TipoArqXml.cArquivoSchema))
            {
                XmlReader xmlReader = null;

                try
                {
                    EncryptAssinatura(rotaArqXML);    //danasa: 12/2013

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;

                    XmlSchemaSet schemas = new XmlSchemaSet();
                    settings.Schemas = schemas;

                    /* Se dentro do XSD houver referência a outros XSDs externos, pode ser necessário ter certas permissões para localizá-lo.
                     * Usa um "Resolver" com as credencias-padrão para obter esse recurso externo. */
                    XmlUrlResolver resolver = new XmlUrlResolver();
                    resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    /* Informa à configuração de leitura do XML que deve usar o "Resolver" criado acima e que a validação deve respeitar
                     * o esquema informado no início. */
                    settings.XmlResolver = resolver;

                    if (TipoArqXml.TargetNameSpace != string.Empty)
                        schemas.Add(TipoArqXml.TargetNameSpace, TipoArqXml.cArquivoSchema);
                    else
                        schemas.Add(NFeStrConstants.NAME_SPACE_NFE, TipoArqXml.cArquivoSchema);

                    settings.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);

                    xmlReader = XmlReader.Create(new StringReader(conteudoXML.OuterXml), settings);

                    cErro = "";
                    try
                    {
                        while (xmlReader.Read()) { }
                    }
                    catch (Exception ex)
                    {
                        cErro = ex.Message;
                    }

                    xmlReader.Close();
                }
                catch (Exception ex)
                {
                    if (xmlReader != null)
                        xmlReader.Close();

                    cErro = ex.Message + "\r\n";
                }

                if (cErro != "")
                {
                    Retorno = 1;
                    RetornoString = "Início da validação...\r\n\r\n";
                    RetornoString += "Arquivo XML: " + rotaArqXML + "\r\n";
                    RetornoString += "Arquivo SCHEMA: " + TipoArqXml.cArquivoSchema + "\r\n\r\n";
                    RetornoString += cErro;
                    RetornoString += "\r\n...Final da validação";
                }
            }
            else if (temXSD)
            {
                Retorno = 3;
                RetornoString = "Arquivo XSD (schema) não foi encontrado em " + TipoArqXml.cArquivoSchema;
            }
        }

        private void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            cErro += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";
        }

        #region ValidarArqXML()

        /// <summary>
        /// Valida o arquivo XML
        /// </summary>
        /// <param name="arquivo">Nome do arquivo XML a ser validado</param>
        /// <returns>
        /// Se retornar uma string em branco, significa que o XML foi
        /// validado com sucesso, ou seja, não tem nenhum erro. Se o retorno
        /// tiver algo, algum erro ocorreu na validação.
        /// </returns>
        public string ValidarArqXML(string arquivo)
        {
            string cRetorna = "";

            if (TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
            {
                Validar(arquivo);
                if (Retorno != 0)
                {
                    cRetorna = "XML INCONSISTENTE!\r\n\r\n" + RetornoString;
                }
            }
            else
            {
                cRetorna = "XML INCONSISTENTE!\r\n\r\n" + TipoArqXml.cRetornoTipoArq;
            }

            return cRetorna;
        }

        #endregion ValidarArqXML()

        #region ValidarArqXML()

        /// <summary>
        /// Valida o arquivo XML
        /// </summary>
        /// <param name="conteudoXML">Conteudo do XML a ser validado</param>
        /// <param name="arquivo">Nome do arquivo XML que será validado</param>
        /// <returns>
        /// Se retornar uma string em branco, significa que o XML foi
        /// validado com sucesso, ou seja, não tem nenhum erro. Se o retorno
        /// tiver algo, algum erro ocorreu na validação.
        /// </returns>
        public string ValidarArqXML(XmlDocument conteudoXML, string arquivo)
        {
            string cRetorna = "";

            if (TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
            {
                Validar(conteudoXML, arquivo);

                if (Retorno != 0)
                {
                    cRetorna = "XML INCONSISTENTE!\r\n\r\n" + RetornoString;
                }
            }
            else
            {
                cRetorna = "XML INCONSISTENTE!\r\n\r\n" + TipoArqXml.cRetornoTipoArq;
            }

            return cRetorna;
        }

        #endregion ValidarArqXML()

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
            int emp = Empresas.FindEmpresaByThread();

            Boolean Assinou = true;

            //Assinar o XML se tiver tag para assinar
            AssinaturaDigital oAD = new AssinaturaDigital();

            try
            {
                if (TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
                {
                    EncryptAssinatura(Arquivo);

                    if (TipoArqXml.TagAssinatura == "eSocial" || TipoArqXml.TagAssinatura == "Reinf")
                    {
                        oAD.Assinar(Arquivo, emp, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, AlgorithmType.Sha256);
                    }
                    else
                    {
                        oAD.Assinar(Arquivo, emp, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);
                    }

                    

                    Assinou = true;
                }
            }
            catch (Exception ex)
            {
                Assinou = false;
                try
                {
                    GravarXMLRetornoValidacao(Arquivo, "2", "Ocorreu um erro ao assinar o XML: " + ex.Message);
                    new Auxiliar().MoveArqErro(Arquivo);
                }
                catch
                {
                    //Se deu algum erro na hora de gravar o retorno do erro para o ERP, infelizmente não posso fazer nada.
                    //Isso pode acontecer se falhar rede, hd, problema de permissão de pastas, etc... Wandrey 23/03/2010
                }
            }

            if (Assinou)
            {
                #region Adicionar a tag do qrCode na NFCe

                if (Arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!String.IsNullOrEmpty(Empresas.Configuracoes[emp].IdentificadorCSC))
                    {
                        XmlDocument conteudoXML = new XmlDocument();
                        conteudoXML.Load(Arquivo);

                        QRCode qrCode = new QRCode(conteudoXML);

                        string url;
                        string versao = string.Empty;
                        if (((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()] != null)
                            versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()].Value;
                        else if (((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.FirstChild.Name)[0]).Attributes[TpcnResources.versao.ToString()] != null)
                            versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.FirstChild.Name)[0]).Attributes[TpcnResources.versao.ToString()].Value;

                        if (versao == "4.00")
                        {
                            url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH_400 : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe_400;
                        }
                        else
                        {
                            url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe;
                        }

                        string linkUFManual = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe;

                        qrCode.GerarLinkConsulta(url, Empresas.Configuracoes[emp].IdentificadorCSC, Empresas.Configuracoes[emp].TokenCSC, linkUFManual);

                        StreamWriter sw = File.CreateText(Arquivo);
                        sw.Write(conteudoXML.OuterXml);
                        sw.Close();
                    }
                }

                #endregion Adicionar a tag do qrCode na NFCe

                // Validar o Arquivo XML
                if (TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
                {
                    try
                    {
                        Validar(Arquivo);
                        if (Retorno != 0)
                        {
                            GravarXMLRetornoValidacao(Arquivo, "3", "Ocorreu um erro ao validar o XML: " + RetornoString);
                            new Auxiliar().MoveArqErro(Arquivo);
                        }
                        else
                        {
                            if (!Directory.Exists(Empresas.Configuracoes[emp].PastaValidado))
                            {
                                Directory.CreateDirectory(Empresas.Configuracoes[emp].PastaValidado);
                            }

                            string ArquivoNovo = Empresas.Configuracoes[emp].PastaValidado + "\\" + Path.GetFileName(Arquivo);

                            Functions.Move(Arquivo, ArquivoNovo);

                            this.GravarXMLRetornoValidacao(Arquivo, "1", "XML assinado e validado com sucesso.");
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            this.GravarXMLRetornoValidacao(Arquivo, "4", "Ocorreu um erro ao validar o XML: " + ex.Message);
                            new Auxiliar().MoveArqErro(Arquivo);
                        }
                        catch
                        {
                            //Se deu algum erro na hora de gravar o retorno do erro para o ERP, infelizmente não posso fazer nada.
                            //Isso pode acontecer se falhar rede, hd, problema de permissão de pastas, etc... Wandrey 23/03/2010
                        }
                    }
                }
                else
                {
                    if (TipoArqXml.nRetornoTipoArq == -1)
                    {
                        ///
                        /// OPS!!! Arquivo de NFS-e enviado p/ a pasta de validação, mas não existe definicao de schemas p/ sua validacao
                        ///
                        this.GravarXMLRetornoValidacao(Arquivo, "1", "XML não validado contra o schema da prefeitura. XML: " + TipoArqXml.cRetornoTipoArq);
                        new Auxiliar().MoveArqErro(Arquivo);
                    }
                    else
                    {
                        try
                        {
                            this.GravarXMLRetornoValidacao(Arquivo, "5", "Ocorreu um erro ao validar o XML: " + TipoArqXml.cRetornoTipoArq);
                            new Auxiliar().MoveArqErro(Arquivo);
                        }
                        catch
                        {
                            //Se deu algum erro na hora de gravar o retorno do erro para o ERP, infelizmente não posso fazer nada.
                            //Isso pode acontecer se falhar rede, hd, problema de permissão de pastas, etc... Wandrey 23/03/2010
                        }
                    }
                }
            }
        }

        #endregion ValidarAssinarXML()

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
            int emp = Empresas.FindEmpresaByThread();

            //Definir o nome do arquivo de retorno
            string ArquivoRetorno = Functions.ExtrairNomeArq(Arquivo, ".xml") + "-ret.xml";

            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                new XElement("Validacao",
                new XElement(TpcnResources.cStat.ToString(), cStat),
                new XElement(TpcnResources.xMotivo.ToString(), xMotivo)));
            xml.Save(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + ArquivoRetorno);
        }

        #endregion GravarXMLRetornoValidacao()
    }
}
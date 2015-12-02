using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Components.QRCode;

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
        #endregion

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
                if (arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML) ||
                    arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML))
                {
                    bool found = false;
                    bool bSave = false;
                    string sh1 = "";
                    XmlDocument doc = new XmlDocument();
                    doc.Load(arquivoXML);

                    if (arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML))
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
                    else if (arquivoXML.EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML) &&
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
        #endregion

        /// <summary>
        /// Método responsável por validar a estrutura do XML de acordo com o schema passado por parâmetro
        /// </summary>
        /// <param name="cRotaArqXML">XML a ser validado</param>
        /// <param name="cRotaArqSchema">Schema a ser utilizado na validação</param>
        /// <param name="nsSchema">Namespace contendo a URL do schema</param>
        private void Validar(string cRotaArqXML)
        {
            bool lArqXML = File.Exists(cRotaArqXML);
            //var caminhoDoSchema = this.PastaSchema + "\\" + TipoArqXml.cArquivoSchema;
            //if (NFe.Components.Propriedade.TipoExecucao == TipoExecucao.teAll)
            //string caminhoDoSchema = Propriedade.PastaExecutavel + "\\" + TipoArqXml.cArquivoSchema;
            bool lArqXSD = File.Exists(TipoArqXml.cArquivoSchema);
            bool temXSD = !string.IsNullOrEmpty(TipoArqXml.cArquivoSchema);

            Retorno = 0;
            RetornoString = "";

            if (lArqXML && lArqXSD)
            {
                XmlReader xmlReader = null;

                try
                {
                    this.EncryptAssinatura(cRotaArqXML);    //danasa: 12/2013

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

                    xmlReader = XmlReader.Create(cRotaArqXML, settings);

                    this.cErro = "";
                    try
                    {
                        while (xmlReader.Read()) { }
                    }
                    catch (Exception ex)
                    {
                        this.cErro = ex.Message;
                    }

                    xmlReader.Close();
                }
                catch (Exception ex)
                {
                    if (xmlReader != null)
                        xmlReader.Close();

                    cErro = ex.Message + "\r\n";
                }

                this.Retorno = 0;
                this.RetornoString = "";
                if (cErro != "")
                {
                    this.Retorno = 1;
                    this.RetornoString = "Início da validação...\r\n\r\n";
                    this.RetornoString += "Arquivo XML: " + cRotaArqXML + "\r\n";
                    this.RetornoString += "Arquivo SCHEMA: " + TipoArqXml.cArquivoSchema + "\r\n\r\n";
                    this.RetornoString += this.cErro;
                    this.RetornoString += "\r\n...Final da validação";
                }
            }
            else
            {
                if (lArqXML == false)
                {
                    this.Retorno = 2;
                    this.RetornoString = "Arquivo XML não foi encontrato";
                }
                else if (lArqXSD == false && temXSD)
                {
                    this.Retorno = 3;
                    this.RetornoString = "Arquivo XSD (schema) não foi encontrado em " + TipoArqXml.cArquivoSchema;
                }
            }
        }

        private void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            this.cErro += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";
        }

        #region ValidarArqXML()
        /// <summary>
        /// Valida o arquivo XML 
        /// </summary>
        /// <param name="arquivo">Nome do arquivo XML a ser validado</param>
        /// <param name="cUFCod">Código da UF/Municipio</param>
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
            int emp = Empresas.FindEmpresaByThread();

            Boolean Assinou = true;

            //Assinar o XML se tiver tag para assinar
            AssinaturaDigital oAD = new AssinaturaDigital();

            try
            {
                if (TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
                {
                    this.EncryptAssinatura(Arquivo);    //danasa: 12/2013

                    oAD.Assinar(Arquivo, emp, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);

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
                        QRCode qrCode = new QRCode(Empresas.Configuracoes[emp].IdentificadorCSC, Empresas.Configuracoes[emp].TokenCSC, Arquivo);

                        if (qrCode.CalcularLink())
                        {
                            string url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe;

                            qrCode.GerarLinkConsulta(url);
                            qrCode.AddLinkQRCode();
                        }
                    }
                }
                #endregion

                // Validar o Arquivo XML
                if (TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
                {
                    try
                    {
                        Validar(Arquivo);
                        if (Retorno != 0)
                        {
                            this.GravarXMLRetornoValidacao(Arquivo, "3", "Ocorreu um erro ao validar o XML: " + RetornoString);
                            new Auxiliar().MoveArqErro(Arquivo);
                        }
                        else
                        {
                            if (!Directory.Exists(Empresas.Configuracoes[emp].PastaValidado))
                            {
                                Directory.CreateDirectory(Empresas.Configuracoes[emp].PastaValidado);
                            }

                            string ArquivoNovo = Empresas.Configuracoes[emp].PastaValidado + "\\" + Path.GetFileName(Arquivo);// Functions.ExtrairNomeArq(Arquivo, ".xml") + ".xml";

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
            int emp = Empresas.FindEmpresaByThread();

            //Definir o nome do arquivo de retorno
            string ArquivoRetorno = Functions.ExtrairNomeArq(Arquivo, ".xml") + "-ret.xml";

            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                    new XElement("Validacao",
                                        new XElement(NFe.Components.TpcnResources.cStat.ToString(), cStat),
                                        new XElement(NFe.Components.TpcnResources.xMotivo.ToString(), xMotivo)));
            xml.Save(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + ArquivoRetorno);
        }
        #endregion
    }
}
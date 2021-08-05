using NFe.Certificado;
using NFe.Components;
using NFe.Components.QRCode;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Unimake.Business.DFe.Xml.CTe;

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

        /// <summary>
        /// Construtor somente para gravar um XML de retorno de erro de validação, não use este construtor para fins de validação pois vai faltar conteúdo para outro fim.
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="cStat"></param>
        /// <param name="xMotivo"></param>
        public ValidarXML(string arquivo, string xMotivo)
        {
            GravarXMLRetornoValidacao(arquivo, "5", xMotivo);
            new Auxiliar().MoveArqErro(arquivo);
        }

        #endregion Construtores

        public TipoArquivoXML TipoArqXml = null;

        public int Retorno { get; private set; }
        public string RetornoString { get; private set; }

        /// <summary>
        /// Pasta dos schemas para validação do XML
        /// </summary>
        private readonly string PastaSchema = Propriedade.PastaSchemas;

        private string cErro;

        #region EncryptAssinatura()

        /// <summary>
        /// Encriptar a tag Assinatura quando for município de Blumenau - SC
        /// </summary>
        /// 
        public bool EncryptAssinatura(string arquivoXML)
        {
            if(TipoArqXml.cArquivoSchema.Contains("DSF\\SJCSP"))
                return false;

            if(TipoArqXml.cArquivoSchema.Contains("PAULISTANA") ||
                TipoArqXml.cArquivoSchema.Contains("BLUMENAU") ||
                TipoArqXml.cArquivoSchema.Contains("DSF"))
            {
                if(arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                    arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                {
                    var found = false;
                    var bSave = false;
                    var sh1 = "";
                    var doc = new XmlDocument();
                    doc.Load(arquivoXML);

                    if(arquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                    {
                        const string Assinatura = "Assinatura";

                        var rpsList = doc.GetElementsByTagName("RPS");
                        foreach(XmlNode rpsNode in rpsList)
                        {
                            var rpsElement = (XmlElement)rpsNode;

                            if(rpsElement.GetElementsByTagName(Assinatura).Count != 0)
                            {
                                found = true;
                                //Encryptar a tag Assinatura
                                var len = TipoArqXml.cArquivoSchema.Contains("DSF") ? 94 : 86;
                                if(rpsElement.GetElementsByTagName(Assinatura)[0].InnerText.Length == len)    //jah assinado?
                                {
                                    bSave = true;
                                    if(TipoArqXml.cArquivoSchema.Contains("DSF"))
                                    {
                                        sh1 = Criptografia.GetSHA1HashData(rpsElement.GetElementsByTagName(Assinatura)[0].InnerText);
                                    }
                                    else
                                    {
                                        if(Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado == null)
                                        {
                                            throw new Exceptions.ExceptionCertificadoDigital(ErroPadrao.CertificadoNaoEncontrado);
                                        }

                                        sh1 = Criptografia.SignWithRSASHA1(Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado,
                                                rpsElement.GetElementsByTagName(Assinatura)[0].InnerText);
                                    }
                                    rpsElement.GetElementsByTagName(Assinatura)[0].InnerText = sh1;
                                }
                            }
                        }
                        if(!found)
                        {
                            throw new Exception("Não foi possivel encontrar a tag <RPS><" + Assinatura + ">");
                        }
                    }
                    else if(arquivoXML.EndsWith(NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML, StringComparison.InvariantCultureIgnoreCase) &&
                            !TipoArqXml.cArquivoSchema.Contains("DSF"))
                    {
                        const string AssinaturaCancelamento = "AssinaturaCancelamento";

                        var detalheList = doc.GetElementsByTagName("Detalhe");
                        foreach(XmlNode detalheNode in detalheList)
                        {
                            var detalheElement = (XmlElement)detalheNode;

                            if(detalheElement.GetElementsByTagName(AssinaturaCancelamento).Count != 0)
                            {
                                found = true;
                                if(detalheElement.GetElementsByTagName(AssinaturaCancelamento)[0].InnerText.Length == 20)
                                {
                                    if(Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado == null)
                                    {
                                        throw new Exceptions.ExceptionCertificadoDigital(ErroPadrao.CertificadoNaoEncontrado);
                                    }

                                    bSave = true;
                                    //Encryptar a tag Assinatura
                                    sh1 = Criptografia.SignWithRSASHA1(Empresas.Configuracoes[Empresas.FindEmpresaByThread()].X509Certificado,
                                                    detalheElement.GetElementsByTagName(AssinaturaCancelamento)[0].InnerText);

                                    detalheElement.GetElementsByTagName(AssinaturaCancelamento)[0].InnerText = sh1;
                                }
                            }
                        }
                        if(!found)
                        {
                            throw new Exception("Não foi possivel encontrar a tag <Detalhe><" + AssinaturaCancelamento + ">");
                        }
                    }
                    //Salvar o XML com as alterações efetuadas
                    if(bSave)
                    {
                        doc.Save(arquivoXML);
                        return true;
                    }
                }
            }
            return false;
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
            var lArqXML = File.Exists(rotaArqXML);

            if(File.Exists(rotaArqXML))
            {
                var doc = new XmlDocument();
                try
                {
                    doc.Load(rotaArqXML);
                }
                catch
                {
                    doc.LoadXml(System.IO.File.ReadAllText(rotaArqXML, System.Text.Encoding.UTF8));
                }
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

            var temXSD = !string.IsNullOrEmpty(TipoArqXml.cArquivoSchema);

            if(File.Exists(TipoArqXml.cArquivoSchema))
            {
                XmlReader xmlReader = null;

                try
                {
                    ValidarInformacaoContingencia(conteudoXML);

                    EncryptAssinatura(rotaArqXML);    //danasa: 12/2013

                    var settings = new XmlReaderSettings
                    {
                        ValidationType = ValidationType.Schema
                    };

                    var schemas = new XmlSchemaSet();
                    settings.Schemas = schemas;

                    /* Se dentro do XSD houver referência a outros XSDs externos, pode ser necessário ter certas permissões para localizá-lo.
                     * Usa um "Resolver" com as credencias-padrão para obter esse recurso externo. */
                    var resolver = new XmlUrlResolver
                    {
                        Credentials = System.Net.CredentialCache.DefaultCredentials
                    };
                    /* Informa à configuração de leitura do XML que deve usar o "Resolver" criado acima e que a validação deve respeitar
                     * o esquema informado no início. */
                    settings.XmlResolver = resolver;

                    if(TipoArqXml.TargetNameSpace != string.Empty)
                    {
                        schemas.Add(TipoArqXml.TargetNameSpace, TipoArqXml.cArquivoSchema);
                    }
                    else
                    {
                        schemas.Add(NFeStrConstants.NAME_SPACE_NFE, TipoArqXml.cArquivoSchema);
                    }

                    settings.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);

                    xmlReader = XmlReader.Create(new StringReader(conteudoXML.OuterXml), settings);

                    cErro = "";
                    try
                    {
                        while(xmlReader.Read()) { }
                    }
                    catch(Exception ex)
                    {
                        cErro = ex.Message;
                    }

                    xmlReader.Close();
                }
                catch(Exception ex)
                {
                    if(xmlReader != null)
                    {
                        xmlReader.Close();
                    }

                    cErro = ex.Message + "\r\n";
                }

                if(cErro != "")
                {
                    Retorno = 1;
                    RetornoString = "Início da validação...\r\n\r\n";
                    RetornoString += "Arquivo XML: " + rotaArqXML + "\r\n";
                    RetornoString += "Arquivo SCHEMA: " + TipoArqXml.cArquivoSchema + "\r\n\r\n";
                    RetornoString += cErro;
                    RetornoString += "\r\n...Final da validação";
                }
            }
            else if(temXSD)
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
            var cRetorna = "";

            if(TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
            {
                Validar(arquivo);
                if(Retorno != 0)
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
            var cRetorna = "";

            if(TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
            {
                Validar(conteudoXML, arquivo);

                if(Retorno != 0)
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
            var emp = Empresas.FindEmpresaByThread();

            var Assinou = true;

            //Assinar o XML se tiver tag para assinar
            var oAD = new AssinaturaDigital();

            XmlDocument conteudoXML = null;

            try
            {
                conteudoXML = new XmlDocument();
                conteudoXML.Load(Arquivo);

                var respTecnico = new RespTecnico(Empresas.Configuracoes[emp].RespTecCNPJ,
                    Empresas.Configuracoes[emp].RespTecXContato,
                    Empresas.Configuracoes[emp].RespTecEmail,
                    Empresas.Configuracoes[emp].RespTecTelefone,
                    Empresas.Configuracoes[emp].RespTecIdCSRT,
                    Empresas.Configuracoes[emp].RespTecCSRT);

                bool salvaXML = respTecnico.AdicionarResponsavelTecnico(conteudoXML);
                if(salvaXML)
                    conteudoXML.Save(Arquivo);

                if(TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
                {
                    if(EncryptAssinatura(Arquivo))
                        conteudoXML.Load(Arquivo);

                    if(TipoArqXml.TargetNameSpace.Contains("envioLoteEventos") && TipoArqXml.TargetNameSpace.Contains("reinf")) //Lote de eventos do EFDReinf
                    {
                        oAD.AssinarLoteEFDReinf(Arquivo, emp);
                    }
                    else if(TipoArqXml.TargetNameSpace.Contains("lote/eventos") && TipoArqXml.TargetNameSpace.Contains("esocial")) //Lote de eventos do eSocial
                    {
                        oAD.AssinarLoteESocial(Arquivo, emp);
                    }
                    else if(TipoArqXml.TagAssinatura == "eSocial")
                    {
                        oAD.Assinar(Arquivo, emp, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, AlgorithmType.Sha256, false);
                    }
                    else if(TipoArqXml.TagAssinatura == "Reinf")
                    {
                        oAD.Assinar(Arquivo, emp, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, AlgorithmType.Sha256);
                    }
                    else if(TipoArqXml.TagAssinatura == "MDFe")
                    {
                        var xmlMDFe = new Unimake.Business.DFe.Xml.MDFe.EnviMDFe
                        {
                            IdLote = "000000000000001",
                            Versao = "3.00",
                            MDFe = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<Unimake.Business.DFe.Xml.MDFe.MDFe>(conteudoXML)
                        };

                        var configMDFe = new Unimake.Business.DFe.Servicos.Configuracao
                        {
                            TipoDFe = Unimake.Business.DFe.Servicos.TipoDFe.MDFe,
                            CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                        };

                        var autorizacao = new Unimake.Business.DFe.Servicos.MDFe.Autorizacao(xmlMDFe, configMDFe);

                        conteudoXML.LoadXml(autorizacao.ConteudoXMLAssinado.GetElementsByTagName("MDFe")[0].OuterXml);

                        var sw = File.CreateText(Arquivo);
                        sw.Write(conteudoXML.OuterXml);
                        sw.Close();
                    }
                    else if(TipoArqXml.TagAssinatura == "CTe")
                    {
                        var xmlCTe = new Unimake.Business.DFe.Xml.CTe.EnviCTe
                        {
                            IdLote = "000000000000001",
                            Versao = "3.00",
                        };
                        xmlCTe.CTe = new List<CTe>();
                        xmlCTe.CTe.Add(Unimake.Business.DFe.Utility.XMLUtility.Deserializar<Unimake.Business.DFe.Xml.CTe.CTe>(conteudoXML));

                        var configCTe = new Unimake.Business.DFe.Servicos.Configuracao
                        {
                            TipoDFe = Unimake.Business.DFe.Servicos.TipoDFe.CTe,
                            CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                        };

                        var autorizacao = new Unimake.Business.DFe.Servicos.CTe.Autorizacao(xmlCTe, configCTe);

                        conteudoXML.LoadXml(autorizacao.ConteudoXMLAssinado.GetElementsByTagName("CTe")[0].OuterXml);

                        var sw = File.CreateText(Arquivo);
                        sw.Write(conteudoXML.OuterXml);
                        sw.Close();
                    }
                    else
                    {
                        oAD.Assinar(conteudoXML, Arquivo, emp, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);
                    }

                    Assinou = true;
                }
            }
            catch(Exception ex)
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

            if(Assinou)
            {
                #region Adicionar a tag do qrCode na NFCe

                if(Arquivo.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                {
                    if(!string.IsNullOrEmpty(Empresas.Configuracoes[emp].IdentificadorCSC))
                    {
                        conteudoXML.Load(Arquivo);

                        var qrCode = new QRCodeNFCe(conteudoXML);

                        string url;
                        var versao = string.Empty;
                        if(((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()] != null)
                        {
                            versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()].Value;
                        }
                        else if(((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.FirstChild.Name)[0]).Attributes[TpcnResources.versao.ToString()] != null)
                        {
                            versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.FirstChild.Name)[0]).Attributes[TpcnResources.versao.ToString()].Value;
                        }

                        if(versao == "4.00")
                        {
                            url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH_400 : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe_400;
                        }
                        else
                        {
                            url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe;
                        }

                        var linkUFManual = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeMH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeM;

                        qrCode.GerarLinkConsulta(url, Empresas.Configuracoes[emp].IdentificadorCSC, Empresas.Configuracoes[emp].TokenCSC, linkUFManual);

                        var sw = File.CreateText(Arquivo);
                        sw.Write(conteudoXML.OuterXml);
                        sw.Close();
                    }
                }
                #endregion

                // Validar o Arquivo XML
                if(TipoArqXml.nRetornoTipoArq >= 1 && TipoArqXml.nRetornoTipoArq <= SchemaXML.MaxID)
                {
                    try
                    {
                        Validar(conteudoXML, Arquivo);
                        if(Retorno != 0)
                        {
                            GravarXMLRetornoValidacao(Arquivo, "3", "Ocorreu um erro ao validar o XML: " + RetornoString);
                            new Auxiliar().MoveArqErro(Arquivo);
                        }
                        else
                        {
                            if(!Directory.Exists(Empresas.Configuracoes[emp].PastaValidado))
                            {
                                Directory.CreateDirectory(Empresas.Configuracoes[emp].PastaValidado);
                            }

                            var ArquivoNovo = Empresas.Configuracoes[emp].PastaValidado + "\\" + Path.GetFileName(Arquivo);

                            Functions.Move(Arquivo, ArquivoNovo);

                            GravarXMLRetornoValidacao(Arquivo, "1", "XML assinado e validado com sucesso.");
                        }
                    }
                    catch(Exception ex)
                    {
                        try
                        {
                            GravarXMLRetornoValidacao(Arquivo, "4", "Ocorreu um erro ao validar o XML: " + ex.Message);
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
                    if(TipoArqXml.nRetornoTipoArq == -1)
                    {
                        ///
                        /// OPS!!! Arquivo de NFS-e enviado p/ a pasta de validação, mas não existe definicao de schemas p/ sua validacao
                        ///
                        GravarXMLRetornoValidacao(Arquivo, "1", "XML não validado contra o schema da prefeitura. XML: " + TipoArqXml.cRetornoTipoArq);
                        new Auxiliar().MoveArqErro(Arquivo);
                    }
                    else
                    {
                        try
                        {
                            GravarXMLRetornoValidacao(Arquivo, "5", "Ocorreu um erro ao validar o XML: " + TipoArqXml.cRetornoTipoArq);
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
        /// <param name="arquivo">Nome do arquivo XML validado</param>
        /// <param name="PastaXMLRetorno">Pasta de retorno para ser gravado o XML</param>
        /// <param name="cStat">Status da validação</param>
        /// <param name="xMotivo">Status descritivo da validação</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>28/05/2009</date>
        private void GravarXMLRetornoValidacao(string arquivo, string cStat, string xMotivo)
        {
            var emp = Empresas.FindEmpresaByThread();

            //Definir o nome do arquivo de retorno
            var arquivoRetorno = Functions.ExtrairNomeArq(arquivo, ".xml") + "-ret.xml";

            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                new XElement("Validacao",
                new XElement(TpcnResources.cStat.ToString(), cStat),
                new XElement(TpcnResources.xMotivo.ToString(), xMotivo)));
            xml.Save(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + arquivoRetorno);
        }

        #endregion GravarXMLRetornoValidacao()

        public void ValidarInformacaoContingencia(XmlDocument conteudoXML)
        {
            var tipoServico = conteudoXML.DocumentElement.Name;

            if(!string.IsNullOrEmpty(tipoServico))
            {
                if(tipoServico.Equals("NFe") || tipoServico.Equals("CTe"))
                {
                    var tipoEmissao = conteudoXML.GetElementsByTagName("tpEmis")[0]?.InnerText;

                    if(!string.IsNullOrEmpty(tipoEmissao))
                    {
                        var tpEmissao = (TipoEmissao)Convert.ToInt32(tipoEmissao);

                        switch(tpEmissao)
                        {
                            case TipoEmissao.teFS:
                            case TipoEmissao.teFSDA:
                            case TipoEmissao.teOffLine:
                                var dhCont = conteudoXML.GetElementsByTagName("dhCont")[0]?.InnerText;
                                var xJust = conteudoXML.GetElementsByTagName("xJust")[0]?.InnerText;

                                if(string.IsNullOrEmpty(dhCont) || string.IsNullOrEmpty(xJust))
                                {
                                    throw new Exception("XML em contingência e não foi informado a data, hora e justificativa da entrada em contingência, tags <dhCont> e <xJust>.");
                                }

                                break;
                        }
                    }
                }
            }
        }
    }
}
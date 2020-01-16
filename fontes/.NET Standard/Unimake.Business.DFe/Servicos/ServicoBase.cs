using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.ConfigurationManager;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml;

namespace Unimake.Business.DFe.Servicos
{
    [ComVisible(true)]
    public abstract class ServicoBase
    {
        #region Private Methods

        /// <summary>
        /// Definir o nome da tag que contem as propriedades de acordo com o serviço que está sendo executado
        /// </summary>
        /// <returns>Nome da tag</returns>
        private string DefinirNomeTag() => GetType().Name;

        private string GetConfigFile(XmlElement elementArquivos)
        {
            var configTipo = Configuracoes.TipoDFe.ToString();
            var arqConfig = elementArquivos.GetElementsByTagName("ArqConfig")[0].InnerText;

            if(arqConfig.Contains(configTipo + "/"))
            {
                return Path.Combine(CurrentConfig.PastaArqConfig, arqConfig);
            }

            return Path.Combine(CurrentConfig.PastaArqConfig, configTipo, arqConfig);
        }

        /// <summary>
        /// Ler as configurações do XML
        /// </summary>
        /// <param name="doc">Documento XML</param>
        private void LerConfig(XmlDocument doc)
        {
            LerConfigPadrao();

            var listServicos = doc.GetElementsByTagName("Servicos");
            foreach(var nodeServicos in listServicos)
            {
                var elementServicos = (XmlElement)nodeServicos;

                if(elementServicos.GetAttribute("ID") == Configuracoes.TipoDFe.ToString())
                {
                    var nomeTagServico = DefinirNomeTag();

                    var listPropriedades = elementServicos.GetElementsByTagName(nomeTagServico);

                    foreach(var nodePropridades in listPropriedades)
                    {
                        var elementPropriedades = (XmlElement)nodePropridades;
                        if(elementPropriedades.GetAttribute("versao") == Configuracoes.SchemaVersao)
                        {
                            if(XMLUtility.TagExist(elementPropriedades, "Descricao"))
                            {
                                Configuracoes.Descricao = XMLUtility.TagRead(elementPropriedades, "Descricao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebActionHomologacao"))
                            {
                                Configuracoes.WebActionHomologacao = XMLUtility.TagRead(elementPropriedades, "WebActionHomologacao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebActionProducao"))
                            {
                                Configuracoes.WebActionProducao = XMLUtility.TagRead(elementPropriedades, "WebActionProducao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebEnderecoHomologacao"))
                            {
                                Configuracoes.WebEnderecoHomologacao = XMLUtility.TagRead(elementPropriedades, "WebEnderecoHomologacao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebEnderecoProducao"))
                            {
                                Configuracoes.WebEnderecoProducao = XMLUtility.TagRead(elementPropriedades, "WebEnderecoProducao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebContentType"))
                            {
                                Configuracoes.WebContentType = XMLUtility.TagRead(elementPropriedades, "WebContentType");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebSoapString"))
                            {
                                Configuracoes.WebSoapString = XMLUtility.TagRead(elementPropriedades, "WebSoapString");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebSoapVersion"))
                            {
                                Configuracoes.WebSoapVersion = XMLUtility.TagRead(elementPropriedades, "WebSoapVersion");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "WebTagRetorno"))
                            {
                                Configuracoes.WebTagRetorno = XMLUtility.TagRead(elementPropriedades, "WebTagRetorno");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "TargetNS"))
                            {
                                Configuracoes.TargetNS = XMLUtility.TagRead(elementPropriedades, "TargetNS");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "SchemaVersao"))
                            {
                                Configuracoes.SchemaVersao = XMLUtility.TagRead(elementPropriedades, "SchemaVersao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "SchemaArquivo"))
                            {
                                Configuracoes.SchemaArquivo = XMLUtility.TagRead(elementPropriedades, "SchemaArquivo").Replace("{0}", Configuracoes.SchemaVersao);
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "TagAssinatura"))
                            {
                                Configuracoes.TagAssinatura = XMLUtility.TagRead(elementPropriedades, "TagAssinatura");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "TagAtributoID"))
                            {
                                Configuracoes.TagAtributoID = XMLUtility.TagRead(elementPropriedades, "TagAtributoID");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "TagLoteAssinatura"))
                            {
                                Configuracoes.TagLoteAssinatura = XMLUtility.TagRead(elementPropriedades, "TagLoteAssinatura");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "TagLoteAtributoID"))
                            {
                                Configuracoes.TagLoteAtributoID = XMLUtility.TagRead(elementPropriedades, "TagLoteAtributoID");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "UrlQrCodeHomologacao"))
                            {
                                Configuracoes.UrlQrCodeHomologacao = XMLUtility.TagRead(elementPropriedades, "UrlQrCodeHomologacao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "UrlQrCodeProducao"))
                            {
                                Configuracoes.UrlQrCodeProducao = XMLUtility.TagRead(elementPropriedades, "UrlQrCodeProducao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "UrlChaveHomologacao"))
                            {
                                Configuracoes.UrlChaveHomologacao = XMLUtility.TagRead(elementPropriedades, "UrlChaveHomologacao");
                            }

                            if(XMLUtility.TagExist(elementPropriedades, "UrlChaveProducao"))
                            {
                                Configuracoes.UrlChaveProducao = XMLUtility.TagRead(elementPropriedades, "UrlChaveProducao");
                            }

                            //Verificar se existem schemas específicos de validação
                            if(XMLUtility.TagExist(elementPropriedades, "SchemasEspecificos"))
                            {
                                var listSchemasEspecificios = elementPropriedades.GetElementsByTagName("SchemasEspecificos");

                                foreach(var nodeSchemasEspecificos in listSchemasEspecificios)
                                {
                                    var elemenSchemasEspecificos = (XmlElement)nodeSchemasEspecificos;

                                    var listTipo = elemenSchemasEspecificos.GetElementsByTagName("Tipo");

                                    foreach(var nodeTipo in listTipo)
                                    {
                                        var elementTipo = (XmlElement)nodeTipo;
                                        var idSchemaEspecifico = elementTipo.GetElementsByTagName("ID")[0].InnerText;

                                        Configuracoes.SchemasEspecificos[idSchemaEspecifico] = new SchemaEspecifico
                                        {
                                            Id = idSchemaEspecifico,
                                            SchemaArquivo = elementTipo.GetElementsByTagName("SchemaArquivo")[0].InnerText.Replace("{0}", Configuracoes.SchemaVersao),
                                            SchemaArquivoEspecifico = elementTipo.GetElementsByTagName("SchemaArquivoEspecifico")[0].InnerText.Replace("{0}", Configuracoes.SchemaVersao)
                                        };
                                    }
                                }
                            }

                            SubstituirValorSoapString();
                        }
                    }

                    break;
                }
            }
        }

        private void LerConfigPadrao()
        {
            var arqConfig = Path.Combine(CurrentConfig.PastaArqConfig, Configuracoes.TipoDFe.ToString(), CurrentConfig.ArquivoConfigPadrao);
            if(!File.Exists(arqConfig))
            {
                throw new System.Exception("Não foi localizado o arquivo de configuração padrão do serviço de " + Configuracoes.TipoDFe.ToString() + ".\r\n\r\n" + arqConfig);
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(arqConfig);

            var listConfigPadrao = xmlDoc.GetElementsByTagName("ConfigPadrao");

            foreach(var nodeConfigPadrao in listConfigPadrao)
            {
                var elementConfigPadrao = (XmlElement)nodeConfigPadrao;

                var listVersao = xmlDoc.GetElementsByTagName("Versao");

                foreach(var nodeVersao in listVersao)
                {
                    var elementVersao = (XmlElement)nodeVersao;

                    if(elementVersao.GetAttribute("ID") == Configuracoes.SchemaVersao)
                    {
                        if(XMLUtility.TagExist(elementVersao, "WebContentType"))
                        {
                            Configuracoes.WebContentType = XMLUtility.TagRead(elementVersao, "WebContentType");
                        }

                        if(XMLUtility.TagExist(elementVersao, "WebSoapString"))
                        {
                            Configuracoes.WebSoapString = XMLUtility.TagRead(elementVersao, "WebSoapString");
                        }

                        if(XMLUtility.TagExist(elementVersao, "WebTagRetorno"))
                        {
                            Configuracoes.WebTagRetorno = XMLUtility.TagRead(elementVersao, "WebTagRetorno");
                        }

                        if(XMLUtility.TagExist(elementVersao, "WebSoapVersion"))
                        {
                            Configuracoes.WebSoapVersion = XMLUtility.TagRead(elementVersao, "WebSoapVersion");
                        }

                        if(XMLUtility.TagExist(elementVersao, "SchemaVersao"))
                        {
                            Configuracoes.SchemaVersao = XMLUtility.TagRead(elementVersao, "SchemaVersao");
                        }

                        if(XMLUtility.TagExist(elementVersao, "TargetNS"))
                        {
                            Configuracoes.TargetNS = XMLUtility.TagRead(elementVersao, "TargetNS");
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Efetua a leitura do XML que contem configurações específicas de cada webservice e atribui o conteúdo nas propriedades do objeto "Configuracoes"
        /// </summary>
        private void LerXmlConfigEspecifico(string xmlConfigEspecifico)
        {
            var doc = new XmlDocument();
            doc.Load(xmlConfigEspecifico);

            #region Leitura do XML herdado, quando tem herança.

            if(doc.GetElementsByTagName("Heranca")[0] != null)
            {
                var arqConfigHeranca =
                    Path.Combine(CurrentConfig.PastaArqConfig,
                    Configuracoes.TipoDFe.ToString(),
                    doc.GetElementsByTagName("Heranca")[0].InnerText);

                doc.Load(arqConfigHeranca);
                LerConfig(doc);

                doc.Load(xmlConfigEspecifico);
            }

            #endregion Leitura do XML herdado, quando tem herança.

            LerConfig(doc);
        }

        /// <summary>
        /// Substituir alguns valores do da configuração do SoapString (Configuracao.WebSoapString)
        /// </summary>
        private void SubstituirValorSoapString()
        {
            Configuracoes.WebSoapString = Configuracoes.WebSoapString.Replace("{ActionWeb}", (Configuracoes.TipoAmbiente == TipoAmbiente.Homologacao ? Configuracoes.WebActionHomologacao : Configuracoes.WebActionProducao));
            Configuracoes.WebSoapString = Configuracoes.WebSoapString.Replace("{cUF}", Configuracoes.CodigoUF.ToString());
            Configuracoes.WebSoapString = Configuracoes.WebSoapString.Replace("{versaoDados}", Configuracoes.SchemaVersao);
        }

        #endregion Private Methods

        #region Protected Properties

        protected XmlDocument ConteudoXML { get; set; }

        #endregion Protected Properties

        #region Protected Methods

        /// <summary>
        /// Defini o valor das propriedades do objeto "Configuracoes"
        /// </summary>
        protected abstract void DefinirConfiguracao();

        /// <summary>
        /// Método para validar o schema do XML
        /// </summary>
        protected abstract void XmlValidar();

        #endregion Protected Methods

        #region Protected Internal Methods

        /// <summary>
        /// Inicializa configurações, parmâtros e propriedades para execução do serviço.
        /// </summary>
        [ComVisible(false)]
        protected internal void Inicializar()
        {
            if(!Configuracoes.Definida)
            {
                DefinirConfiguracao();
                LerXmlConfigGeral();
            }
        }

        /// <summary>
        /// Efetua a leitura do XML que contem configurações gerais e atribui o conteúdo nas propriedades do objeto "Configuracoes"
        /// </summary>
        protected internal void LerXmlConfigGeral()
        {
            var doc = new XmlDocument();
            doc.Load(CurrentConfig.ArquivoConfigGeral);

            var listConfiguracoes = doc.GetElementsByTagName("Configuracoes");

            foreach(XmlNode nodeConfiguracoes in listConfiguracoes)
            {
                var elementConfiguracoes = (XmlElement)nodeConfiguracoes;
                var listArquivos = elementConfiguracoes.GetElementsByTagName("Arquivo");

                foreach(var nodeArquivos in listArquivos)
                {
                    var elementArquivos = (XmlElement)nodeArquivos;

                    if(elementArquivos.GetAttribute("ID") != Configuracoes.CodigoUF.ToString())
                    {
                        continue;
                    }

                    Configuracoes.Nome = elementArquivos.GetElementsByTagName("Nome")[0].InnerText;
                    Configuracoes.NomeUF = elementArquivos.GetElementsByTagName("UF")[0].InnerText;
                    LerXmlConfigEspecifico(GetConfigFile(elementArquivos));

                    break;
                }
            }
        }

        #endregion Protected Internal Methods

        #region Public Properties

        public Configuracao Configuracoes { get; set; }

        public string RetornoWSString { get; set; }

        public XmlDocument RetornoWSXML { get; set; }

        #endregion Public Properties

        #region Public Constructors

        static ServicoBase() => AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.AssemblyResolve;

        public ServicoBase()
        {
        }

        public ServicoBase(XmlDocument conteudoXML, Configuracao configuracao)
        {
            Configuracoes = configuracao;
            ConteudoXML = conteudoXML;

            Inicializar();

            System.Diagnostics.Trace.WriteLine(ConteudoXML?.InnerXml, "Unimake.DFe");
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executar o serviço para consumir o webservice
        /// </summary>
        public virtual void Executar()
        {
            var soap = new WSSoap
            {
                EnderecoWeb = (Configuracoes.TipoAmbiente == TipoAmbiente.Producao ? Configuracoes.WebEnderecoProducao : Configuracoes.WebEnderecoHomologacao),
                ActionWeb = (Configuracoes.TipoAmbiente == TipoAmbiente.Producao ? Configuracoes.WebActionProducao : Configuracoes.WebActionHomologacao),
                TagRetorno = Configuracoes.WebTagRetorno,
                VersaoSoap = Configuracoes.WebSoapVersion,
                SoapString = Configuracoes.WebSoapString,
                ContentType = Configuracoes.WebContentType
            };

            var consumirWS = new ConsumirWS();
            consumirWS.ExecutarServico(ConteudoXML, soap, Configuracoes.CertificadoDigital);

            RetornoWSString = consumirWS.RetornoServicoString;
            RetornoWSXML = consumirWS.RetornoServicoXML;
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML no HD</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gravado no HD</param>
        /// <param name="conteudoXML">String contendo o conteúdo do XML a ser gravado no HD</param>
        public abstract void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML);

        [ComVisible(true)]
        public void Inicializar(Configuracao configuracoes)
        {
            Configuracoes = configuracoes ?? throw new System.ArgumentNullException(nameof(configuracoes));
            Inicializar();
        }

        public void SetXML(XmlDocument xml) => ConteudoXML = xml;

        #endregion Public Methods
    }
}
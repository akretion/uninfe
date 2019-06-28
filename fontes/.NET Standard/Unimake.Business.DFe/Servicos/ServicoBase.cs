using System.Xml;

namespace Unimake.Business.DFe.Servicos
{
    public abstract class ServicoBase
    {
        #region Construtores

        public ServicoBase(XmlDocument conteudoXML, Configuracao configuracao)
        {
            Configuracoes = configuracao;
            ConteudoXML = conteudoXML;

            Inicializar();
        }

        #endregion

        #region Propriedades

        protected XmlDocument ConteudoXML;
        protected Configuracao Configuracoes;
        public string RetornoWSString;
        public XmlDocument RetornoWSXML;

        #endregion

        #region Constantes

        private const string PastaArqConfig = @"Servicos\Nfe\Config\";
        private const string ArquivoConfigGeral = PastaArqConfig + "Config.xml";

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa configurações, parmâtros e propriedades para execução do serviço.
        /// </summary>
        private void Inicializar()
        {
            if (!Configuracoes.Definida)
            {
                DefinirConfiguracao();

                LerXmlConfigGeral();
            }
        }

        /// <summary>
        /// Defini o valor das propriedades do objeto "Configuracoes"
        /// </summary>
        protected abstract void DefinirConfiguracao();

        /// <summary>
        /// Efetua a leitura do XML que contem configurações gerais e atribui o conteúdo nas propriedades do objeto "Configuracoes"
        /// </summary>
        private void LerXmlConfigGeral()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ArquivoConfigGeral);

            XmlNodeList listConfiguracoes = doc.GetElementsByTagName("Configuracoes");

            foreach (XmlNode nodeConfiguracoes in listConfiguracoes)
            {
                XmlElement elementConfiguracoes = (XmlElement)nodeConfiguracoes;
                XmlNodeList listArquivos = elementConfiguracoes.GetElementsByTagName("Arquivo");

                foreach (object nodeArquivos in listArquivos)
                {
                    XmlElement elementArquivos = (XmlElement)nodeArquivos;

                    if (elementArquivos.GetAttribute("ID") == Configuracoes.CodigoUF.ToString())
                    {
                        Configuracoes.Nome = elementArquivos.GetElementsByTagName("Nome")[0].InnerText;
                        Configuracoes.NomeUF = elementArquivos.GetElementsByTagName("UF")[0].InnerText;

                        LerXmlConfigEspecifico(PastaArqConfig + elementArquivos.GetElementsByTagName("ArqConfig")[0].InnerText);
                    }
                }
            }
        }

        /// <summary>
        /// Efetua a leitura do XML que contem configurações específicas de cada webservice e atribui o conteúdo nas propriedades do objeto "Configuracoes"
        /// </summary>
        private void LerXmlConfigEspecifico(string xmlConfigEspecifico)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlConfigEspecifico);

            XmlNodeList listServicos = doc.GetElementsByTagName("Servicos");
            foreach (object nodeServicos in listServicos)
            {
                XmlElement elementServicos = (XmlElement)nodeServicos;

                if (elementServicos.GetAttribute("ID") == Configuracoes.TipoDFe.ToString())
                {
                    string nomeTagServico = DefinirNomeTag();

                    XmlNodeList listPropriedades = elementServicos.GetElementsByTagName(DefinirNomeTag());

                    foreach (object nodePropridades in listPropriedades)
                    {
                        XmlElement elementPropriedades = (XmlElement)nodePropridades;
                        if (elementPropriedades.GetAttribute("versao") == Configuracoes.SchemaVersao)
                        {
                            Configuracoes.Descricao = elementPropriedades.GetElementsByTagName("Descricao")[0].InnerText;
                            Configuracoes.WebActionHomologacao = elementPropriedades.GetElementsByTagName("WebActionHomologacao")[0].InnerText;
                            Configuracoes.WebActionProducao = elementPropriedades.GetElementsByTagName("WebActionProducao")[0].InnerText;
                            Configuracoes.WebEnderecoHomologacao = elementPropriedades.GetElementsByTagName("WebEnderecoHomologacao")[0].InnerText;
                            Configuracoes.WebEnderecoProducao = elementPropriedades.GetElementsByTagName("WebEnderecoProducao")[0].InnerText;
                            Configuracoes.WebContentType = elementPropriedades.GetElementsByTagName("WebContentType")[0].InnerText;
                            Configuracoes.WebSoapString = elementPropriedades.GetElementsByTagName("WebSoapString")[0].InnerText;
                            Configuracoes.WebSoapVersion = elementPropriedades.GetElementsByTagName("WebSoapVersion")[0].InnerText;
                            Configuracoes.WebTagRetorno = elementPropriedades.GetElementsByTagName("WebTagRetorno")[0].InnerText;
                            Configuracoes.TargetNS = elementPropriedades.GetElementsByTagName("TargetNS")[0].InnerText;
                            Configuracoes.SchemaVersao = elementPropriedades.GetElementsByTagName("SchemaVersao")[0].InnerText;
                            Configuracoes.SchemaArquivo = elementPropriedades.GetElementsByTagName("SchemaArquivo")[0].InnerText.Replace("{0}", Configuracoes.SchemaVersao);
                            Configuracoes.TagAssinatura = elementPropriedades.GetElementsByTagName("TagAssinatura")[0].InnerText;
                            Configuracoes.TagAtributoID = elementPropriedades.GetElementsByTagName("TagAtributoID")[0].InnerText;
                            Configuracoes.TagLoteAssinatura = elementPropriedades.GetElementsByTagName("TagLoteAssinatura")[0].InnerText;
                            Configuracoes.TagLoteAtributoID = elementPropriedades.GetElementsByTagName("TagLoteAtributoID")[0].InnerText;
                        }
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Definir o nome da tag que contem as propriedades de acordo com o serviço que está sendo executado
        /// </summary>
        /// <returns>Nome da tag</returns>
        private string DefinirNomeTag()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Executar o serviço para consumir o webservice
        /// </summary>
        public abstract void Executar();

        /// <summary>
        /// Método para validar o schema do XML
        /// </summary>
        protected abstract void XmlValidar();

        #endregion
    }
}

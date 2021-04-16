using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Description;
using System.Security.Authentication;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.Components
{
    public class WebServiceProxy
    {
#if _fw35

        #region Criar protocolo de comunicação TLS11 e TLS12 para .NET 3.5

        private const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        private const SslProtocols _Tls11 = (SslProtocols)0x00000300;
        private const SecurityProtocolType Tls11 = (SecurityProtocolType)_Tls11;
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        #endregion Criar protocolo de comunicação TLS11 e TLS12 para .NET 3.5

#endif

        #region Propriedades

        /// <summary>
        /// Descrição do serviço (WSDL)
        /// </summary>
        private ServiceDescription serviceDescription { get; set; }

        /// <summary>
        /// Código assembly do serviço
        /// </summary>
        private Assembly serviceAssemby { get; set; }

        /// <summary>
        /// Certificado digital a ser utilizado no consumo dos serviços
        /// </summary>
        private X509Certificate2 oCertificado { get; set; }

        #region Proxy

        /// <summary>
        /// Utiliza servidor proxy?
        /// </summary>
        public bool UtilizaServidorProxy { get; set; }

        /// <summary>
        /// Endereço do servidor de proxy
        /// </summary>
        public string ProxyServidor { get; set; }

        /// <summary>
        /// Usuário para autenticação no servidor de proxy
        /// </summary>
        public string ProxyUsuario { get; set; }

        /// <summary>
        /// Senha do usuário para autenticação no servidor de proxy
        /// </summary>
        public string ProxySenha { get; set; }

        /// <summary>
        /// Porta de comunicação do servidor proxy
        /// </summary>
        public int ProxyPorta { get; set; }

        #endregion Proxy

        /// <summary>
        /// Arquivo WSDL
        /// </summary>
        public string ArquivoWSDL { get; set; }

        private PadroesNFSe PadraoNFSe { get; set; }

        private Servicos servico;
        private bool taHomologacao;
        private int cMunicipio;

        private string _NomeClasseWS;

        public string NomeClasseWS
        {
            get
            {
                switch (PadraoNFSe)
                {
                    #region DUETO

                    case PadroesNFSe.DUETO:
                        switch (servico)
                        {
                            case Servicos.NFSeConsultarLoteRps:
                            case Servicos.NFSeConsultar:
                            case Servicos.NFSeConsultarPorRps:
                            case Servicos.NFSeConsultarSituacaoLoteRps:
                                if (cMunicipio == 4302808)
                                    return "basic_INFSEConsultas";
                                else
                                    return "BasicHttpBinding_INFSEConsultas";

                            case Servicos.NFSeCancelar:
                            case Servicos.NFSeRecepcionarLoteRps:
                                if (cMunicipio == 4302808)
                                    return "basic_INFSEGeracao";
                                else
                                    return "BasicHttpBinding_INFSEGeracao";
                            default:
                                return _NomeClasseWS;
                        }

                    #endregion DUETO

                    #region ISSONLINE4R (4R Sistemas)

                    case PadroesNFSe.ISSONLINE4R:
                        switch (servico)
                        {
                            case Servicos.NFSeConsultarPorRps:
                                return (taHomologacao ? "hConsultarNfsePorRps" : "ConsultarNfsePorRps");

                            case Servicos.NFSeCancelar:
                                return (taHomologacao ? "hCancelarNfse" : "CancelarNfse");

                            case Servicos.NFSeRecepcionarLoteRps:
                                return (taHomologacao ? "hRecepcionarLoteRpsSincrono" : "RecepcionarLoteRpsSincrono");

                            default:
                                return _NomeClasseWS;
                        }

                    #endregion ISSONLINE4R (4R Sistemas)

                    #region SIMPLISS

                    case PadroesNFSe.SIMPLISS:
                        return _NomeClasseWS = "NfseService";

                    #endregion SIMPLISS

                    #region PRONIM

                    case PadroesNFSe.PRONIN:
                        switch (servico)
                        {
                            case Servicos.NFSeCancelar:
                                if (cMunicipio == 4109401 ||
                                    cMunicipio == 3131703 ||
                                    cMunicipio == 4303004 ||
                                    cMunicipio == 4322509 ||
                                    cMunicipio == 3556602 ||
                                    cMunicipio == 3512803 ||
                                    cMunicipio == 4323002 ||
                                    cMunicipio == 3505807 ||
                                    cMunicipio == 3530300 ||
                                    cMunicipio == 4308904 ||
                                    cMunicipio == 4118501 ||
                                    cMunicipio == 3554300 ||
                                    cMunicipio == 3542404 ||
                                    cMunicipio == 5005707 ||
                                    cMunicipio == 4314423 ||
                                    cMunicipio == 3511102 ||
                                    cMunicipio == 3535804 ||
                                    cMunicipio == 4306932 ||
                                    cMunicipio == 4322400 ||
                                    cMunicipio == 4302808 ||
                                    cMunicipio == 3501301 ||
                                    cMunicipio == 4300109 ||
                                    cMunicipio == 4124053 ||
                                    cMunicipio == 4101408 ||
                                    cMunicipio == 3550407 ||
                                    cMunicipio == 4310207 ||
                                    cMunicipio == 1502400)
                                    return "BasicHttpBinding_INFSEGeracao";
                                else
                                    return "basic_INFSEGeracao";
                            case Servicos.NFSeSubstituirNfse:
                                if (cMunicipio == 4323002)
                                    return "BasicHttpBinding_INFSEGeracao";
                                else
                                    return "basic_INFSEGeracao";

                            case Servicos.NFSeRecepcionarLoteRps:
                                if (cMunicipio == 4109401 ||
                                    cMunicipio == 3131703 ||
                                    cMunicipio == 4303004 ||
                                    cMunicipio == 4322509 ||
                                    cMunicipio == 3512803 ||
                                    cMunicipio == 4308904 ||
                                    cMunicipio == 4118501 ||
                                    cMunicipio == 3554300 ||
                                    cMunicipio == 3542404 ||
                                    cMunicipio == 5005707 ||
                                    cMunicipio == 4314423 ||
                                    cMunicipio == 3511102 ||
                                    cMunicipio == 3535804 || 
                                    cMunicipio == 4306932 ||
                                    cMunicipio == 4322400 ||
                                    cMunicipio == 4302808 ||
                                    cMunicipio == 3501301 ||
                                    cMunicipio == 4300109 ||
                                    cMunicipio == 4124053 ||
                                    cMunicipio == 4101408 ||
                                    cMunicipio == 3550407 ||
                                    cMunicipio == 4310207 ||
                                    cMunicipio == 1502400)
                                    return "BasicHttpBinding_INFSEGeracao";
                                else
                                    return "basic_INFSEGeracao";

                            default:
                                if (cMunicipio == 4109401 ||
                                    cMunicipio == 3131703 ||
                                    cMunicipio == 4303004 ||
                                    cMunicipio == 4322509 ||
                                    cMunicipio == 3556602 ||
                                    cMunicipio == 4308904 ||
                                    cMunicipio == 5005707 ||
                                    cMunicipio == 4314423 ||
                                    cMunicipio == 3511102 ||
                                    cMunicipio == 3535804 ||
                                    cMunicipio == 4306932 ||
                                    cMunicipio == 4322400 ||
                                    cMunicipio == 4302808 ||
                                    cMunicipio == 3501301 ||
                                    cMunicipio == 4300109 ||
                                    cMunicipio == 4124053 ||
                                    cMunicipio == 4101408 ||
                                    cMunicipio == 3550407 ||
                                    cMunicipio == 4310207 ||
                                    cMunicipio == 1502400)

                                    return "BasicHttpBinding_INFSEConsultas";
                                else
                                    return "basic_INFSEConsultas";
                        }

                    #endregion PRONIM

                    #region E-GOVERNE

                    case PadroesNFSe.EGOVERNE:
                        return "WSNFSeV1001";

                    #endregion E-GOVERNE

                    #region BSIT-BR

                    case PadroesNFSe.BSITBR:
                        return "nfseWS";

                    #endregion BSIT-BR

                    #region FINTEL

                    case PadroesNFSe.FINTEL:
                        return "WebService";

                    #endregion FINTEL

                    default:
                        return _NomeClasseWS;
                }
            }
            protected set { _NomeClasseWS = value; }
        }

        public string[] NomeMetodoWS { get; protected set; }

        /// <summary>
        /// Lista utilizada para armazenar os webservices
        /// </summary>
        public static List<webServices> webServicesList { get; private set; }

        #endregion Propriedades

        #region Construtores

        public WebServiceProxy(int cUF, string arquivoWSDL, X509Certificate2 Certificado, PadroesNFSe padraoNFSe, bool taHomologacao, Servicos servico, int tpEmis, int cMunicipio)
        {
            ArquivoWSDL = arquivoWSDL;
            PadraoNFSe = padraoNFSe;
            this.servico = servico;
            this.taHomologacao = taHomologacao;
            this.cMunicipio = cMunicipio;

            //Definir o certificado digital que será utilizado na conexão com os serviços
            oCertificado = Certificado;

            #region Obter a descrição do serviço (WSDL)

            //Problema identificado com a Prefeitura de Porto Alegre - RS  Renan - 09/02/2015
            //Esta propriedade "Expect100Continue" por default é definida como "true" ou seja, o cliente esperará obter uma resposta 100-Continue do servidor para indicar que o cliente deve
            //enviar os dados a ser lançadas. Esse mecanismo permite que os clientes evitem enviar grandes quantidades de dados através da rede quando o servidor, com base em cabeçalhos de solicitação,
            //pretende descartar a solicitação.
            //Já esta propriedade marcada como "false", quando a solicitação inicial é enviada para o servidor, inclui os dados. Se, após ler os cabeçalhos de solicitação,
            //o servidor requer autenticação e deve enviar uma resposta 401, o cliente deve enviar novamente os dados com os cabeçalhos apropriadas de autenticação.
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = DefinirProtocoloSeguranca(cUF, taHomologacao, tpEmis, padraoNFSe, servico);
            serviceDescription = ServiceDescription.Read(arquivoWSDL);

            #endregion Obter a descrição do serviço (WSDL)

            #region Descobrir o nome da classe e dos métodos

            NomeClasseWS = null;
            NomeMetodoWS = null;
            if (serviceDescription.Services != null && serviceDescription.Services.Count > 0)
            {
                NomeClasseWS = ((Service)serviceDescription.Services[0]).Name.Replace(" ", "").Replace(".", "");
            }

            if (serviceDescription.Bindings != null)
            {
                foreach (var xx in serviceDescription.Bindings)
                {
                    if (((Binding)xx).Operations != null)
                    {
                        if (((Binding)xx).Operations.Count > 0)
                        {
                            NomeMetodoWS = new string[((Binding)xx).Operations.Count];
                            for (int n = 0; n < ((Binding)xx).Operations.Count; ++n)
                            {
                                NomeMetodoWS[n] = ((Binding)xx).Operations[n].Name;
                            }
                        }
                    }
                }
            }

            #endregion Descobrir o nome da classe e dos métodos

            //Gerar e compilar a classe
            GerarClasse();
        }

        public WebServiceProxy(X509Certificate2 Certificado)
        {
            oCertificado = Certificado;
        }

        #endregion Construtores

        #region Métodos públicos

        #region ReturnArray()

        /// <summary>
        /// Método que verifica se o tipo de retornjo de uma operação/método é array ou não
        /// </summary>
        /// <param name="Instance">Instancia do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <returns>true se o tipo de retorno do método passado por parâmetro for um array</returns>
        /*public bool ReturnArray(object Instance, string methodName)
        {
            Type tipoInstance = Instance.GetType();

            return tipoInstance.GetMethod(methodName).ReturnType.IsSubclassOf(typeof(Array));
        }*/

        #endregion ReturnArray()

        #region Invoke()

        /// <summary>
        /// Invocar o método da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Objeto com o conteúdo dos parâmetros do método</param>
        /// <returns>Objeto - Um objeto somente, podendo ser primário ou complexo</returns>
        public object Invoke(object Instance, string methodName, object[] parameters)
        {
            try
            {
                //Relacionar o certificado digital que será utilizado no serviço que será consumido do webservice
                RelacCertificado(Instance);

                Type tipoInstance = Instance.GetType();

                object result = tipoInstance.GetMethod(methodName).Invoke(Instance, parameters);

                return result;
            }
            catch (Exception ex)
            {
                string msgErro = $"Erro ao invocar o método '{methodName}'.\r\nWSDL: {ArquivoWSDL} {Environment.NewLine}{ex.Message}";

                var inner = ex.InnerException;

                while (inner != null)
                {
                    msgErro += $"{Environment.NewLine}{inner.Message}";
                    inner = inner.InnerException;
                }

                throw new Exception(msgErro);
            }
        }

        #endregion Invoke()

        #region InvokeXML()

        /// <summary>
        /// Invocar o método da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Objeto com o conteúdo dos parâmetros do método</param>
        /// <returns>Um objeto do tipo XML</returns>
        public XmlNode InvokeXML(object Instance, string methodName, object[] parameters)
        {
            //Invocar método do serviço
            return (XmlNode)this.Invoke(Instance, methodName, parameters);
        }

        #endregion InvokeXML()

        #region InvokeElement()

        /// <summary>
        /// Invocar o método da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Objeto com o conteúdo dos parâmetros do método</param>
        /// <returns>Um objeto do tipo XML</returns>
        public XmlNode InvokeElement(object Instance, string methodName, object[] parameters)
        {
            //Invocar método do serviço
            return (XmlNode)this.Invoke(Instance, methodName, parameters);
        }

        #endregion InvokeElement()

        #region InvokeXML()

        /// <summary>
        /// Invocar o método da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Objeto com o conteúdo dos parâmetros do método</param>
        /// <returns>Um objeto do tipo string</returns>
        public string InvokeStr(object Instance, string methodName, object[] parameters)
        {
            //Invocar método do serviço
            return (string)Invoke(Instance, methodName, parameters);
        }

        #endregion InvokeXML()

        #region SetProp()

        /// <summary>
        /// Alterar valor das propriedades da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <param name="novoValor">Novo valor para ser gravado na propriedade</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 09/02/2010
        /// </remarks>
        public void SetProp(object instance, string propertyName, object novoValor)
        {
            Type tipoInstance = instance.GetType();
            PropertyInfo property = tipoInstance.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            property?.SetValue(instance, novoValor, null);
        }
        public void SetProp2(object instance, string propertyName, object novoValor)
        {
            Type tipoInstance = instance.GetType();
            PropertyInfo property = tipoInstance.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            property.SetValue(instance, novoValor, null);
        }

        #endregion SetProp()

        #region GetProp()

        /// <summary>
        /// Alterar valor das propriedades da classe
        /// </summary>
        /// <param name="instance">Instância do objeto</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 09/02/2010
        /// </remarks>
        public object GetProp(object instance, string propertyName)
        {
            Type tipoInstance = instance.GetType();
            PropertyInfo property = tipoInstance.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            return property.GetValue(instance, null);
        }

        #endregion GetProp()

        #region CertificateValidation

        /// <summary>
        /// Responsável por retornar uma confirmação verdadeira para a proriedade ServerCertificateValidationCallback
        /// da classe ServicePointManager para confirmar a solicitação SSL automaticamente.
        /// </summary>
        /// <returns>Retornará sempre true</returns>
        public bool CertificateValidation(object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErros)
        {
            return true;
        }

        #endregion CertificateValidation

        #region CriarObjeto()

        /// <summary>
        /// Criar objeto das classes do serviço
        /// </summary>
        /// <param name="NomeClasse">Nome da classe que é para ser instanciado um novo objeto</param>
        /// <returns>Retorna o objeto</returns>
        public object CriarObjeto(string NomeClasse)
        {
            if (string.IsNullOrEmpty(NomeClasse) || this.serviceAssemby.GetType(NomeClasse) == null)
                throw new Exception("Nome da classe '" + NomeClasse + "' no webservice não pode ser processada\r\nWSDL: " + this.ArquivoWSDL);

            return Activator.CreateInstance(this.serviceAssemby.GetType(NomeClasse));
        }

        #endregion CriarObjeto()

        #endregion Métodos públicos

        #region DefinirProtocoloSeguranca()

        /// <summary>
        /// Definir o protocolo de segurança a ser utilizado na comunicação com os WebServices
        /// </summary>
        /// <param name="cUF"></param>
        /// <param name="taHomologacao"></param>
        /// <param name="tpEmis"></param>
        /// <param name="padraoNFSe"></param>
        /// <param name="servico"></param>
        /// <returns>Protocolo de segurança a ser utilizado</returns>
        public static SecurityProtocolType DefinirProtocoloSeguranca(int cUF, bool taHomologacao, int tpEmis, PadroesNFSe padraoNFSe, Servicos servico)
        {
#if _fw35
            SecurityProtocolType securityProtocolType = Tls11 | Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
#else
            SecurityProtocolType securityProtocolType = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
#endif
            //if (cUF.ToString().Length >= 7) //Somente para muncípios, não pode fazer para Estados
            //{
            //    string cUFs = "2910800 / "; //Feira de Santana

            //    if (cUFs.Contains(cUF.ToString()))
            //    {
            //        securityProtocolType = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            //    }
            //}

            return securityProtocolType;
        }

        /// <summary>
        /// Definir o protocolo de segurança a ser utilizado na comunicação com os WebServices
        /// </summary>
        /// <param name="cUF"></param>
        /// <param name="tpAmb"></param>
        /// <param name="tpEmis"></param>
        /// <param name="servico"></param>
        /// <returns>Protocolo de segurança a ser utilizado</returns>
        public static SecurityProtocolType DefinirProtocoloSeguranca(int cUF, int tpAmb, int tpEmis, Servicos servico)
        {
            return DefinirProtocoloSeguranca(cUF, (tpAmb == (int)TipoAmbiente.taHomologacao), tpEmis, PadroesNFSe.NaoIdentificado, servico);
        }

        /// <summary>
        /// Definir o protocolo de segurança a ser utilizado na comunicação com os WebServices
        /// </summary>
        /// <param name="cUF"></param>
        /// <param name="tpAmb"></param>
        /// <param name="tpEmis"></param>
        /// <param name="padraoNFSe"></param>
        /// <param name="servico"></param>
        /// <returns>Protocolo de segurança a ser utilizado</returns>
        public static SecurityProtocolType DefinirProtocoloSeguranca(int cUF, int tpAmb, int tpEmis, PadroesNFSe padraoNFSe, Servicos servico)
        {
            return DefinirProtocoloSeguranca(cUF, (tpAmb == (int)TipoAmbiente.taHomologacao), tpEmis, padraoNFSe, servico);
        }

        #endregion DefinirProtocoloSeguranca()

        #region Métodos privados

        #region GerarClasse()

        /// <summary>
        /// Gerar o source code do serviço
        /// </summary>
        private void GerarClasse()
        {
            #region Gerar o código da classe

            StringWriter writer = new StringWriter(CultureInfo.CurrentCulture);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            provider.GenerateCodeFromNamespace(GerarGrafo(), writer, null);

            #endregion Gerar o código da classe


            string codigoClasse = writer.ToString();

            #region Compilar o código da classe

            CompilerResults results = provider.CompileAssemblyFromSource(ParametroCompilacao(), codigoClasse);
            serviceAssemby = results.CompiledAssembly;

            #endregion Compilar o código da classe
        }

        #endregion GerarClasse()

        #region ParametroCompilacao

        /// <summary>
        /// Montar os parâmetros para a compilação da classe
        /// </summary>
        /// <returns>Retorna os parâmetros</returns>
        private CompilerParameters ParametroCompilacao()
        {
            CompilerParameters parameters = new CompilerParameters(new string[] { "System.dll", "System.Xml.dll", "System.Web.Services.dll", "System.Data.dll" });
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.TreatWarningsAsErrors = false;
            parameters.WarningLevel = 4;

            return parameters;
        }

        #endregion ParametroCompilacao

        #region GerarGrafo()

        /// <summary>
        /// Gerar a estrutura e o grafo da classe
        /// </summary>
        private CodeNamespace GerarGrafo()
        {
            #region Gerar a estrutura da classe do serviço

            //Gerar a estrutura da classe do serviço
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.AddServiceDescription(this.serviceDescription, string.Empty, string.Empty);

            //Definir o nome do protocolo a ser utilizado
            //Não posso definir, tenho que deixar por conta do WSDL definir, ou pode dar erro em alguns estados
            //importer.ProtocolName = "Soap12";
            if (PadraoNFSe == PadroesNFSe.THEMA)
                importer.ProtocolName = "Soap";

            //Tipos deste serviço devem ser gerados como propriedades e não como simples campos
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties;

            #endregion Gerar a estrutura da classe do serviço

            #region Se a NFSe for padrão DUETO/WEBISS/SALVADOR_BA/PRONIN preciso importar os schemas do WSDL

            switch (PadraoNFSe)
            {
                case PadroesNFSe.NOTAINTELIGENTE:
                case PadroesNFSe.SMARAPD:
                case PadroesNFSe.DUETO:
                case PadroesNFSe.WEBISS:
                case PadroesNFSe.WEBISS_202:
                case PadroesNFSe.SALVADOR_BA:
                case PadroesNFSe.GIF:
                case PadroesNFSe.PRONIN:
                case PadroesNFSe.INTERSOL:
                case PadroesNFSe.D2TI:

                    //Tive que utilizar a WebClient para que a OpenRead funcionasse, não foi possível fazer funcionar com a SecureWebClient. Tem que analisar melhor. Wandrey e Renan 10/09/2013
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(ArquivoWSDL);

                    //Esta sim tem que ser com a SecureWebClient pq tem que ter o certificado. Wandrey 10/09/2013
                    SecureWebClient client2 = new SecureWebClient(oCertificado);

                    // Add any imported files
                    foreach (System.Xml.Schema.XmlSchema wsdlSchema in serviceDescription.Types.Schemas)
                    {
                        foreach (System.Xml.Schema.XmlSchemaObject externalSchema in wsdlSchema.Includes)
                        {
                            if (externalSchema is System.Xml.Schema.XmlSchemaImport)
                            {
                                Uri baseUri = new Uri(ArquivoWSDL);
                                Uri schemaUri = new Uri(baseUri, ((System.Xml.Schema.XmlSchemaExternal)externalSchema).SchemaLocation);
                                stream = client2.OpenRead(schemaUri);
                                System.Xml.Schema.XmlSchema schema = System.Xml.Schema.XmlSchema.Read(stream, null);
                                importer.Schemas.Add(schema);
                            }
                        }
                    }
                    break;
            }

            #endregion Se a NFSe for padrão DUETO/WEBISS/SALVADOR_BA/PRONIN preciso importar os schemas do WSDL

            #region Gerar o o grafo da classe para depois gerar o código

            CodeNamespace @namespace = new CodeNamespace();
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(@namespace);
            ServiceDescriptionImportWarnings warmings = importer.Import(@namespace, unit);

            #endregion Gerar o o grafo da classe para depois gerar o código

            return @namespace;
        }

        #endregion GerarGrafo()

        #region RelacCertificado

        /// <summary>
        /// Relacionar o certificado digital com o serviço que será consumido do webservice
        /// </summary>
        /// <param name="instance">Objeto do serviço que será consumido</param>
        private void RelacCertificado(object instance)
        {
            if (oCertificado != null)
            {
                var client = instance as System.Web.Services.Protocols.SoapHttpClientProtocol;
                client?.ClientCertificates.Add(oCertificado);
            }
        }

        #endregion RelacCertificado

        #endregion Métodos privados

        #region Objeto da BETHA Sistemas para acessar os WebServices da NFSe

        public IBetha Betha;

        #endregion Objeto da BETHA Sistemas para acessar os WebServices da NFSe

        #region CarregaWebServicesList()

        /// <summary>
        /// Carrega a lista de webservices definidos no arquivo WebService.XML
        /// </summary>
        public static void CarregaWebServicesList()
        {
            if (webServicesList == null)
            {
                webServicesList = new List<webServices>();
                Propriedade.Municipios = null;
                Propriedade.Municipios = new List<Municipio>();

                XmlDocument doc = new XmlDocument();

                /// danasa 1-2012
                if (File.Exists(Propriedade.NomeArqXMLMunicipios))
                {
                    int contaTentativa = 0;
                    while (contaTentativa <= 2)
                    {
                        try
                        {
                            doc.Load(Propriedade.NomeArqXMLMunicipios);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Functions.WriteLog("Ocorreu um erro na tentativa de carregamento do arquivo " + Propriedade.NomeArqXMLMunicipios + ".\r\n\r\n" +
                                "Erro:\r\n\r\n" + ex.Message, true, true, "");

                            if (contaTentativa++ == 2) break;

                            //Forçar recriar o arquivo
                            File.Delete(Propriedade.NomeArqXMLMunicipios);
                            WebServiceNFSe.SalvarXMLMunicipios();
                        }
                    }

                    XmlNodeList estadoList = doc.GetElementsByTagName(NFeStrConstants.Registro);
                    foreach (XmlNode registroNode in estadoList)
                    {
                        XmlElement registroElemento = (XmlElement)registroNode;
                        if (registroElemento.Attributes.Count > 0)
                        {
                            int IDmunicipio = Convert.ToInt32("0" + Functions.OnlyNumbers(registroElemento.Attributes[TpcnResources.ID.ToString()].Value));
                            string Nome = registroElemento.Attributes[NFeStrConstants.Nome].Value;
                            string Padrao = registroElemento.Attributes[NFeStrConstants.Padrao].Value;
                            string UF = Functions.CodigoParaUF(Convert.ToInt32(IDmunicipio.ToString().Substring(0, 2)));

                            PadroesNFSe pdr = WebServiceNFSe.GetPadraoFromString(Padrao);

                            string urlsH = WebServiceNFSe.WebServicesHomologacao(ref pdr, IDmunicipio);
                            string urlsP = WebServiceNFSe.WebServicesProducao(ref pdr, IDmunicipio);

                            if (!string.IsNullOrEmpty(urlsH) || !string.IsNullOrEmpty(urlsP))
                            {
                                var ci = (from i in webServicesList where i.ID == IDmunicipio select i).FirstOrDefault();
                                if (ci == null)
                                {
                                    webServices wsItem = new webServices(IDmunicipio, Nome, UF);

                                    PreencheURLw(wsItem.LocalHomologacao,
                                                 NFeStrConstants.LocalHomologacao,
                                                 urlsH,
                                                 "",
                                                 "NFse\\");
                                    PreencheURLw(wsItem.LocalProducao,
                                                 NFeStrConstants.LocalProducao,
                                                 urlsP,
                                                 "",
                                                 "NFse\\");
                                    webServicesList.Add(wsItem);
                                }
                            }

                            ///
                            /// adiciona na lista que será usada na manutencao
                            ///
                            Propriedade.Municipios.Add(new Municipio(IDmunicipio, UF, Nome, pdr));
                        }
                    }
                }

                /// danasa 1-2012

                bool salvaXmlLocal = false;
                LoadArqXMLWebService(Propriedade.NomeArqXMLWebService_NFe, "NFe\\");
                salvaXmlLocal = LoadArqXMLWebService(Propriedade.NomeArqXMLWebService_NFSe, "NFse\\");

                if (salvaXmlLocal)
                {
                    WebServiceNFSe.SalvarXMLMunicipios(null, null, 0, null, false);
                }
            }
        }

        private static bool LoadArqXMLWebService(string filenameWS, string subfolder)
        {
            bool salvaXmlLocal = false;

            if (File.Exists(filenameWS))
            {
                XmlDocument doc = new XmlDocument();

                try
                {
                    doc.Load(filenameWS);
                }
                catch (Exception ex)
                {
                    Functions.WriteLog("Ocorreu um erro na tentativa de carregamento do arquivo " + filenameWS + ".\r\n" +
                        "Acesse novamente o sistema para que se recupere automaticamente do erro.\r\n\r\n" +
                        "Erro:\r\n\r\n" + ex.Message, true, true, "");

                    if (File.Exists(Propriedade.XMLVersaoWSDLXSD))
                        File.Delete(Propriedade.XMLVersaoWSDLXSD);

                    Environment.Exit(0);
                }

                XmlNodeList estadoList = doc.GetElementsByTagName(NFeStrConstants.Estado);
                foreach (XmlNode estadoNode in estadoList)
                {
                    XmlElement estadoElemento = (XmlElement)estadoNode;
                    if (estadoElemento.Attributes.Count > 0)
                    {
                        if (estadoElemento.Attributes[TpcnResources.UF.ToString()].Value != "XX")
                        {
                            int ID = Convert.ToInt32("0" + Functions.OnlyNumbers(estadoElemento.Attributes[TpcnResources.ID.ToString()].Value));
                            if (ID == 0)
                                continue;
                            string Nome = estadoElemento.Attributes[NFeStrConstants.Nome].Value;
                            string UF = estadoElemento.Attributes[TpcnResources.UF.ToString()].Value;

                            /// danasa 1-2012
                            ///
                            /// verifica se o ID já está na lista
                            /// isto previne que no xml de configuracao tenha duplicidade e evita derrubar o programa
                            ///
                            var ci = (from i in webServicesList where i.ID == ID select i).FirstOrDefault();
                            if (ci == null)
                            {
                                webServices wsItem = new webServices(ID, Nome, UF);
                                XmlNodeList urlList;

                                urlList = estadoElemento.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalHomologacao);
                                if (urlList.Count > 0)
                                    PreencheURLw(wsItem.LocalHomologacao,
                                                 NFe.Components.NFeStrConstants.LocalHomologacao,
                                                 urlList.Item(0).OuterXml,
                                                 UF,
                                                 subfolder);

                                urlList = estadoElemento.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalProducao);
                                if (urlList.Count > 0)
                                    PreencheURLw(wsItem.LocalProducao,
                                                 NFe.Components.NFeStrConstants.LocalProducao,
                                                 urlList.Item(0).OuterXml,
                                                 UF,
                                                 subfolder);

                                webServicesList.Add(wsItem);
                            }

                            // danasa 1-2012
                            if (estadoElemento.HasAttribute(NFeStrConstants.Padrao))
                            {
                                try
                                {
                                    string padrao = estadoElemento.Attributes[NFeStrConstants.Padrao].Value;
                                    if (!padrao.Equals(PadroesNFSe.NaoIdentificado.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        var cc = (from i in Propriedade.Municipios
                                                  where i.CodigoMunicipio == ID
                                                  select i).FirstOrDefault();
                                        if (cc == null)
                                        {
                                            Propriedade.Municipios.Add(new Municipio(ID, UF, Nome, WebServiceNFSe.GetPadraoFromString(padrao)));
                                            salvaXmlLocal = true;
                                        }
                                        else
                                        {
                                            if (!cc.PadraoStr.Equals(padrao) || !cc.UF.Equals(UF) || !cc.Nome.Equals(Nome, StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                cc.Padrao = WebServiceNFSe.GetPadraoFromString(padrao);
                                                cc.Nome = Nome;
                                                cc.UF = UF;
                                                salvaXmlLocal = true;
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }

                            // danasa 1-2012
                        }
                    }
                }
            }
            else
            {
                Functions.WriteLog("Ocorreu um erro na tentativa de carregamento do arquivo " + filenameWS + ".\r\n" +
                    "Acesse novamente o sistema para que se recupere automaticamente do erro.", true, true, "");

                if (File.Exists(Propriedade.XMLVersaoWSDLXSD))
                    File.Delete(Propriedade.XMLVersaoWSDLXSD);

                Environment.Exit(0);
            }

            return salvaXmlLocal;
        }

        #endregion CarregaWebServicesList()

        #region reloadWebServicesList()

        /// <summary>
        /// Recarrega a lista de webservices
        /// usado pelo projeto da NFes quando da manutencao
        /// </summary>
        public static void reloadWebServicesList()
        {
            webServicesList = null;
            CarregaWebServicesList();
        }

        #endregion reloadWebServicesList()

        #region PreencheURLw

        private static void PreencheURLw(URLws wsItem, string tagName, string urls, string uf, string subfolder)
        {
            if (urls == "")
                return;

            string AppPath = Propriedade.PastaExecutavel + "\\" + subfolder;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(urls);
            XmlNodeList urlList = doc.ChildNodes;
            if (urlList == null)
                return;

            for (int i = 0; i < urlList.Count; ++i)
            {
                for (int j = 0; j < urlList[i].ChildNodes.Count; ++j)
                {
                    string appPath = "";
                    PropertyInfo ClassProperty = wsItem.GetType().GetProperty(urlList[i].ChildNodes[j].Name);
                    if (ClassProperty != null)
                    {
                        appPath = AppPath + urlList[i].ChildNodes[j].InnerText;

                        if (!string.IsNullOrEmpty(urlList[i].ChildNodes[j].InnerText))
                        {
                            if (urlList[i].ChildNodes[j].InnerText.ToLower().EndsWith("asmx?wsdl"))
                            {
                                appPath = urlList[i].ChildNodes[j].InnerText;
                            }
                            else
                            {
                                if (!File.Exists(appPath))
                                {
                                    appPath = "";
                                }
                            }
                        }
                        else
                            appPath = "";

                        ClassProperty.SetValue(wsItem, appPath, null);
                    }

                    if (appPath == "" && !string.IsNullOrEmpty(urlList[i].ChildNodes[j].InnerText))
                    {
                        string msg = "";
                        Console.WriteLine(msg = "wsItem <" + urlList[i].ChildNodes[j].InnerText + "> nao encontrada na classe URLws em <" + urlList[i].ChildNodes[j].Name + ">");
                        Functions.WriteLog(msg, false, true, "");
                    }
                }
            }
        }

        #endregion PreencheURLw
    }

    public class webServices
    {
        public int ID { get; private set; }

        public string Nome { get; private set; }

        public string UF { get; private set; }

        public URLws LocalHomologacao { get; private set; }

        public URLws LocalProducao { get; private set; }

        public webServices(int id, string nome, string uf)
        {
            LocalHomologacao = new URLws();
            LocalProducao = new URLws();
            ID = id;
            Nome = nome;
            UF = uf;
        }
    }

    [ToolboxItem(false)]
    internal class SecureWebClient : WebClient
    {
        private readonly X509Certificate2 Certificado;

        public SecureWebClient(X509Certificate2 certificado)
        {
            Certificado = certificado;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.ClientCertificates.Add(Certificado);
            return request;
        }
    }

    public class URLws
    {
        public URLws()
        {
            ///
            /// NFS-e
            CancelarNfse =
            ConsultarLoteRps =
            ConsultarNfse =
            ConsultarNfsePorRps =
            ConsultarSituacaoLoteRps =
            ConsultarURLNfse =
            ConsultarNFSePNG =
            ConsultarNFSePDF =
            InutilizarNFSe =
            RecepcionarLoteRps =
            ConsultaSequenciaLoteNotaRPS =
            ConsultarStatusNFse =

            ///
            /// CFS-e
            RecepcionarLoteCfse =
            CancelarCfse =
            ConsultarLoteCfse =
            ConsultarLoteCfse =
            ConfigurarTerminalCfse =
            EnviarInformeManutencaoCfse =
            InformeTrasmissaoSemMovimentoCfse =
            ConsultarDadosCadastroCfse =

            ///
            /// NF-e
            NFeRecepcaoEvento =
            NFeConsulta =
            NFeConsultaCadastro =
            NFeDownload =
            NFeInutilizacao =
            NFeManifDest =
            NFeStatusServico =
            NFeAutorizacao =
            NFeRetAutorizacao =

            ///
            /// MDF-e
            MDFeRecepcao =
            MDFeRecepcaoSinc =
            MDFeRetRecepcao =
            MDFeConsulta =
            MDFeStatusServico =
            MDFeRecepcaoEvento =
            MDFeNaoEncerrado =

            ///
            /// DF-e
            DFeRecepcao =

            ///
            /// CT-e
            CTeRecepcao =
            CTeRetRecepcao =
            CTeInutilizacao =
            CTeConsulta =
            CTeStatusServico =
            CTeRecepcaoEvento =
            CTeConsultaCadastro =
            CTeDistribuicaoDFe =
            CteRecepcaoOS =

            ///
            /// LMC
            LMCAutorizacao = string.Empty;
        }

        #region Propriedades referente as tags do webservice.xml

        // ******** ATENÇÃO *******
        // os nomes das propriedades tem que ser iguais as tags no WebService.xml
        // ******** ATENÇÃO *******

        #region NFe

        // public string NFeRecepcao { get; set; }
        //public string NFeRetRecepcao { get; set; }
        public string NFeInutilizacao { get; set; }

        public string NFeConsulta { get; set; }

        public string NFeStatusServico { get; set; }

        public string NFeConsultaCadastro { get; set; }

        public string NFeAutorizacao { get; set; }

        public string NFeRetAutorizacao { get; set; }

        /// <summary>
        /// Recepção de eventos da NFe
        /// </summary>
        public string NFeRecepcaoEvento { get; set; }

        public string NFeDownload { get; set; }

        public string NFeManifDest { get; set; }

        /// <summary>
        /// Distribuição de DFe de interesses de autores (NFe)
        /// </summary>
        public string DFeRecepcao { get; set; }

        #endregion NFe

        #region NFS-e

        /// <summary>
        /// Enviar Lote RPS NFS-e
        /// </summary>
        public string RecepcionarLoteRps { get; set; }

        /// <summary>
        /// Consultar Situação do lote RPS NFS-e
        /// </summary>
        public string ConsultarSituacaoLoteRps { get; set; }

        /// <summary>
        /// Consultar NFS-e por RPS
        /// </summary>
        public string ConsultarNfsePorRps { get; set; }

        /// <summary>
        /// Consultar NFS-e por NFS-e
        /// </summary>
        public string ConsultarNfse { get; set; }

        /// <summary>
        /// Consultar lote RPS
        /// </summary>
        public string ConsultarLoteRps { get; set; }

        /// <summary>
        /// Cancelar NFS-e
        /// </summary>
        public string CancelarNfse { get; set; }

        /// <summary>
        /// Consulta URL de Visualização da NFSe
        /// </summary>
        public string ConsultarURLNfse { get; set; }

        /// <summary>
        /// Consulta a imagem em PNG da Nota
        /// </summary>
        public string ConsultarNFSePNG { get; set; }

        /// <summary>
        /// Consulta a imagem em PDF da Nota
        /// </summary>
        public string ConsultarNFSePDF { get; set; }

        /// <summary>
        /// Inutilização da NFSe
        /// </summary>
        public string InutilizarNFSe { get; set; }

        /// <summary>
        /// Obter o XML da NFSe
        /// </summary>
        public string ObterNotaFiscal { get; set; }

        /// <summary>
        /// Consulta Sequencia Lote Nota RPS
        /// </summary>
        public string ConsultaSequenciaLoteNotaRPS { get; set; }

        /// <summary>
        /// Substituir Nfse
        /// </summary>
        public string SubstituirNfse { get; set; }

        /// <summary>
        /// Consultar as NFS-e que foram recebidas
        /// </summary>
        public string ConsultaNFSeRecebidas { get; set; }

        /// <summary>
        /// Consultar status Nfse
        /// </summary>
        public string ConsultarStatusNFse { get; set; }

        /// <summary>
        /// Consultar as NFS-e tomados
        /// </summary>
        public string ConsultaNFSeTomados { get; set; }

        #endregion NFS-e

        #region CFS-e

        /// <summary>
        /// Enviar lote CFS-e
        /// </summary>
        public string RecepcionarLoteCfse { get; set; }

        /// <summary>
        /// Cancelar CFS-e
        /// </summary>
        public string CancelarCfse { get; set; }

        /// <summary>
        /// Consultar Lote CFS-e
        /// </summary>
        public string ConsultarLoteCfse { get; set; }

        /// <summary>
        /// Consultar Lote CFS-e
        /// </summary>
        public string ConsultarCfse { get; set; }

        /// <summary>
        /// Configurar/ativar terminal CFS-e
        /// </summary>
        public string ConfigurarTerminalCfse { get; set; }

        /// <summary>
        /// Enviar informe manutenção terminal CFS-e
        /// </summary>
        public string EnviarInformeManutencaoCfse { get; set; }

        /// <summary>
        /// Enviar informe de transmissão sem movimento da CFS-e
        /// </summary>
        public string InformeTrasmissaoSemMovimentoCfse { get; set; }

        /// <summary>
        /// Enviar o XML para consultar os dados do cadastro de terminal CFS-e
        /// </summary>
        public string ConsultarDadosCadastroCfse { get; set; }

        #endregion CFS-e

        #region MDF-e

        /// <summary>
        /// Recepção do MDFe Assíncrono
        /// </summary>
        public string MDFeRecepcao { get; set; }

        /// <summary>
        /// Recepção do MDFe Síncrono
        /// </summary>
        public string MDFeRecepcaoSinc { get; set; }
        /// <summary>
        /// Consulta Recibo do lote de MDFe enviado
        /// </summary>
        public string MDFeRetRecepcao { get; set; }

        /// <summary>
        /// Consulta situação do MDFe
        /// </summary>
        public string MDFeConsulta { get; set; }

        /// <summary>
        /// Consulta status do serviço de MDFe
        /// </summary>
        public string MDFeStatusServico { get; set; }

        /// <summary>
        /// Recepcao dos eventos do MDF-e
        /// </summary>
        public string MDFeRecepcaoEvento { get; set; }

        /// <summary>
        /// Consulta dos MDFe´s não encerrados
        /// </summary>
        public string MDFeNaoEncerrado { get; set; }

        /// <summary>
        /// Recepcao dos LMC´s
        /// </summary>
        public string LMCAutorizacao { get; set; }

        #endregion MDF-e

        #region CTe

        /// <summary>
        /// Recepção do CTe
        /// </summary>
        public string CTeRecepcao { get; set; }

        /// <summary>
        /// Consulta recibo do lote de CTe enviado
        /// </summary>
        public string CTeRetRecepcao { get; set; }

        /// <summary>
        /// Inutilização numeração do CTe
        /// </summary>
        public string CTeInutilizacao { get; set; }

        /// <summary>
        /// Consulta Situação do CTe
        /// </summary>
        public string CTeConsulta { get; set; }

        /// <summary>
        /// Consulta Status Serviço do CTe
        /// </summary>
        public string CTeStatusServico { get; set; }

        /// <summary>
        /// Consulta cadastro do contribuinte do CTe
        /// </summary>
        public string CTeConsultaCadastro { get; set; }

        /// <summary>
        /// Recepção de eventos do CTe
        /// </summary>
        public string CTeRecepcaoEvento { get; set; }

        /// <summary>
        /// Distribuição de DFe de interesses de autores (CTe)
        /// </summary>
        public string CTeDistribuicaoDFe { get; set; }

        /// <summary>
        /// Recepção do CTe modelo 67
        /// </summary>
        public string CteRecepcaoOS { get; set; }

        #endregion CTe

        #region EFDReinf

        /// <summary>
        /// Recepção do lote de eventos do EFDReinf
        /// </summary>
        public string RecepcaoLoteReinf { get; set; }

        /// <summary>
        /// Consulta do lote de eventos do EFDReinf
        /// </summary>
        public string ConsultarLoteReinf { get; set; }

        /// <summary>
        /// Consultas do EFDReinf: totalizações e recibo de entrega
        /// </summary>
        public string ConsultasReinf { get; set; }

        #endregion EFDReinf

        #region eSocial

        /// <summary>
        /// Recepção do lote de eventos do eSocial
        /// </summary>
        public string RecepcaoLoteeSocial { get; set; }

        /// <summary>
        /// Consulta do lote de eventos do eSocial
        /// </summary>
        public string ConsultarLoteeSocial { get; set; }

        /// <summary>
        /// Consulta dos identificadores dos eventos do eSocial: Empregador, Tabela e Trabalhador
        /// </summary>
        public string ConsultarIdentificadoresEventoseSocial { get; set; }

        /// <summary>
        /// Download dos eventos por Id e Número do recibo
        /// </summary>
        public string DownloadEventoseSocial { get; set; }

        #endregion eSocial

        #endregion Propriedades referente as tags do webservice.xml
    }
}
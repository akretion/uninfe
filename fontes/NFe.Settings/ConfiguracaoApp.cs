using NFe.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Unimake.Business.DFe.Security;

namespace NFe.Settings
{
    #region Classe ConfiguracaoApp

    public class ArquivoItem
    {
        public string Arquivo;
        public DateTime Data;
        public bool Manual;
        public long Size;
    }

    /// <summary>
    /// Classe responsável por realizar algumas tarefas na parte de configurações da aplicação.
    /// Arquivo de configurações: UniNfeConfig.xml
    /// </summary>
    public class ConfiguracaoApp
    {
        #region NfeConfiguracoes

        /// <summary>
        /// Enumerador com as tags do xml nfe_Configuracoes
        /// </summary>
        private enum NfeConfiguracoes
        {
            Proxy = 0,
            ProxyServidor,
            ProxyUsuario,
            ProxySenha,
            ProxyPorta,
            SenhaConfig,
            ChecarConexaoInternet,
            GravarLogOperacaoRealizada,
            DetectarProxyAuto,
            ConfirmaSaida
        }

        #endregion NfeConfiguracoes

        #region Propriedades

        #region ChecarConexaoInternet

        public static bool ChecarConexaoInternet { get; set; }

        #endregion ChecarConexaoInternet

        #region GravarLogOperacoesRealizadas

        public static bool GravarLogOperacoesRealizadas { get; set; }

        #endregion GravarLogOperacoesRealizadas

        #region Propriedades para controle de servidor proxy

        public static bool Proxy { get; set; }
        public static bool DetectarConfiguracaoProxyAuto { get; set; }
        public static string ProxyServidor { get; set; }
        public static string ProxyUsuario { get; set; }
        public static string ProxySenha { get; set; }
        public static int ProxyPorta { get; set; }

        #endregion Propriedades para controle de servidor proxy

        #region Propriedades para tela de sobre

        public static string NomeEmpresa { get; set; }
        public static string Site { get; set; }
        public static string SiteProduto { get; set; }
        public static string Email { get; set; }

        #endregion Propriedades para tela de sobre

        #region SenhaConfig

        public static string SenhaConfig { get; set; }

        #endregion SenhaConfig

        #region Prorpiedades utilizadas no inicio do sistema

        public static Stopwatch ExecutionTime { get; set; }
        public static bool ConfirmaSaida { get; set; }

        #endregion Prorpiedades utilizadas no inicio do sistema

        #endregion Propriedades

        #region Métodos gerais

        public static bool ExtractResourceToDisk(System.Reflection.Assembly ass, string s, string fileoutput)
        {
            var extraido = false;
            using(var FileReader = new StreamReader(ass.GetManifestResourceStream(s)))
            {
                if(!Directory.Exists(Path.GetDirectoryName(fileoutput)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileoutput));
                }

                using(var FileWriter = new StreamWriter(fileoutput))
                {
                    FileWriter.Write(FileReader.ReadToEnd());
                    FileWriter.Close();
                    extraido = true;
                }
            }
            return extraido;
        }

        #region Extrae os arquivos necessarios a executacao

        internal class loadResources
        {
            public string cErros { get; private set; }

            #region load()

            /// <summary>
            /// Exporta os WSDLs e Schemas da DLL para as pastas do UniNFe
            /// </summary>
            public void load()
            {
                if(Empresas.Configuracoes.Count == 0)
                {
                    ///
                    /// OPS!!! nenhuma empresa ainda cadastrada, então gravamos o log na pasta de log do uninfe
                    ConfiguracaoApp.GravarLogOperacoesRealizadas = true;
                }

                if(AtualizarWSDL())
                {
                    Propriedade.Estados = null;

                    try
                    {
                        var ass = Assembly.LoadFile(Propriedade.PastaExecutavel + "\\NFe.Components.Wsdl.dll");
                        var x = ass.GetManifestResourceNames();
                        if(x.GetLength(0) > 0)
                        {
                            string fileoutput = null;
                            var okFiles = new List<string>();

                            var afiles = (from d in x
                                          where d.StartsWith("NFe.Components.Wsdl.NF")
                                          select d);

                            foreach(var s in afiles)
                            {
                                fileoutput = s.Replace("NFe.Components.Wsdl.", Propriedade.PastaExecutavel + "\\");
                                if(fileoutput == null)
                                {
                                    continue;
                                }

                                if(fileoutput.ToLower().EndsWith(".xsd"))
                                {
                                    /// Ex: NFe.Components.Wsdl.NFe.NFe.xmldsig-core-schema_v1.01.xsd
                                    ///
                                    /// pesquisa pelo nome do XSD
                                    var plast = fileoutput.ToLower().LastIndexOf("_v");
                                    if(plast == -1)
                                    {
                                        plast = fileoutput.IndexOf(".xsd") - 1;
                                    }

                                    while(fileoutput[plast] != '.' && plast >= 0)
                                    {
                                        --plast;
                                    }

                                    var fn = fileoutput.Substring(plast + 1);
                                    fileoutput = fileoutput.Substring(0, plast).Replace(".", "\\") + "\\" + fn;
                                }
                                else
                                {
                                    fileoutput = (fileoutput.Substring(0, fileoutput.LastIndexOf('.')) + "####" +
                                                    fileoutput.Substring(fileoutput.LastIndexOf('.') + 1)).Replace(".", "\\").Replace("####", ".");
                                }

                                ExtractResourceToDisk(ass, s, fileoutput);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        var xMotivo = "Não foi possível atualizar pacotes de Schemas/WSDLs.";

                        Auxiliar.WriteLog(cErros = xMotivo + Environment.NewLine + ex.Message, false);

                        if(Empresas.Configuracoes.Count > 0)
                        {
                            var emp = Empresas.FindEmpresaByThread();
                            var oAux = new Auxiliar();
                            oAux.GravarArqErroERP(Empresas.Configuracoes[emp].CNPJ + ".err", cErros);
                        }
                    }
                    finally
                    {
                        WebServiceNFSe.SalvarXMLMunicipios();
                    }
                }
            }

            #region AtualizarWSDL()

            /// <summary>
            /// Verifica se é para atualizar os recursos contidos no NFe.Components.Wsdl.dll
            /// </summary>
            /// <returns>true=atualiza, false=não atualiza</returns>
            private bool AtualizarWSDL()
            {
                var retorna = false;

                try
                {
                    var fi = new FileInfo(Propriedade.PastaExecutavel + "\\NFe.Components.Wsdl.dll");

                    try
                    {
                        if(!File.Exists(Propriedade.XMLVersaoWSDLXSD))
                        {
                            retorna = true;
                        }
                        else
                        {
                            var doc = new XmlDocument();
                            doc.Load(Propriedade.XMLVersaoWSDLXSD);
                            if(doc.GetElementsByTagName("dVersao")[0] == null)
                            {
                                retorna = true;
                            }
                            else
                            {
                                var versao = doc.GetElementsByTagName("dVersao")[0].InnerText;

                                if(!fi.LastWriteTimeUtc.ToString().Equals(versao))
                                {
                                    retorna = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                        retorna = true;
                    }

                    if(retorna)
                    {
                        XmlWriter xtw = null; // criar instância para xmltextwriter.

                        try
                        {
                            var settings = new XmlWriterSettings();
                            var c = new UTF8Encoding(false);

                            settings.Encoding = c;
                            settings.Indent = true;
                            settings.IndentChars = "";
                            settings.NewLineOnAttributes = false;
                            settings.OmitXmlDeclaration = false;

                            xtw = XmlWriter.Create(Propriedade.XMLVersaoWSDLXSD, settings); //atribuir arquivo, caminho e codificação
                            xtw.WriteStartDocument(); //comaçar a escrever o documento
                            xtw.WriteStartElement("VersaoWSDLXSD"); //Criar elemento raiz
                            xtw.WriteElementString("dVersao", fi.LastWriteTimeUtc.ToString());
                            xtw.WriteEndElement(); //encerrar tag DocumentosNFe
                            xtw.Flush();
                        }
                        catch(Exception ex)
                        {
                            retorna = true;

                            throw (ex);
                        }
                        finally
                        {
                            if(xtw != null)
                            {
                                if(xtw.WriteState != WriteState.Closed)
                                {
                                    xtw.Close(); //Fechar o arquivo e salvar
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    retorna = true;
                    Auxiliar.WriteLog("Ocorreu um erro na hora de verificar a versão dos WSDL/XSD. Erro: " + ex.Message, true);
                }

                return retorna;
            }

            #endregion AtualizarWSDL()

            #endregion load()
        }

        #endregion Extrae os arquivos necessarios a executacao

        #region StartVersoes

        public static void StartVersoes()
        {
            new loadResources().load();

            ConfiguracaoApp.CarregarDados();
            //ConfiguracaoApp.DownloadArquivoURLConsultaDFe();

            if(!Propriedade.ServicoRodando || Propriedade.ExecutandoPeloUniNFe)
            {
                ConfiguracaoApp.CarregarDadosSobre();
            }

            try
            {
                SchemaXML.CriarListaIDXML();
            }
            catch(Exception ex)
            {
                ///
                /// essa mensagem nunca será exibida ao usuário, porque se ela for exibida, você terá que ajustar
                ///
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        #endregion StartVersoes

        private static string LocalFile => Application.StartupPath + "\\sefaz.inc";

        #region URLs do Estados p/ ConsultaDFe

        public static EstadoURLConsultaDFe CarregarURLConsultaDFe(string uf)
        {
            var estado = new EstadoURLConsultaDFe();

            if(File.Exists(LocalFile))
            {
                var inifile = new IniFile(LocalFile);

                estado.UF = uf;
                estado.UrlCTeQrCodeP = inifile.Read("CTeQrCodeP", uf);
                estado.UrlCTeQrCodeH = inifile.Read("CTeQrCodeH", uf);
                estado.UrlNFCe = inifile.Read("NFC-e", uf);
                estado.UrlNFCeH = inifile.Read("NFC-e(h)", uf);
                estado.UrlNFCeM = inifile.Read("NFC-e_ConsultaManual", uf);
                estado.UrlNFCeMH = inifile.Read("NFC-e_ConsultaManual(h)", uf);
                estado.UrlNFCe_400 = inifile.Read("NFC-e_400", uf);
                estado.UrlNFCeH_400 = inifile.Read("NFC-e(h)_400", uf);

                if(string.IsNullOrEmpty(estado.UrlNFCeM))
                {
                    estado.UrlNFCeM = estado.UrlNFCe;
                }

                if(string.IsNullOrEmpty(estado.UrlNFCeMH))
                {
                    estado.UrlNFCeMH = estado.UrlNFCeH;
                }

                if(string.IsNullOrEmpty(estado.UrlNFCe_400))
                {
                    estado.UrlNFCe_400 = estado.UrlNFCe;
                }

                if(string.IsNullOrEmpty(estado.UrlNFCeH_400))
                {
                    estado.UrlNFCeH_400 = estado.UrlNFCeH;
                }
            }
            else
            {
                throw new Exception("O arquivo SEFAZ.INC não foi localizado, por favor reinstale o UniNFe.");
            }

            return estado;
        }

        public static void DownloadArquivoURLConsultaDFe()
        {
            var URL = "http://www.unimake.com.br/pub/downloads/sefaz.inc";
            var URL2 = "http://74.222.1.252/download/sefaz.inc";
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            Stream strResponse = null;
            Stream strLocal = null;
            var result = true;

            if(File.Exists(LocalFile))
            {
                var dateFile = File.GetLastWriteTime(LocalFile).Date;

                var lastUpdate = (DateTime.Now.Date - dateFile).TotalDays;

                if(lastUpdate < 30)
                {
                    return;
                }
            }

            using(var Client = new WebClient())
            {
                try
                {
                    for(var i = 0; i <= 1; i++)
                    {
                        try
                        {
                            // Criar um pedido do arquivo que será baixado
                            webRequest = (HttpWebRequest)WebRequest.Create(URL);

                            // Definir dados da conexao do proxy
                            if(ConfiguracaoApp.Proxy)
                            {
                                webRequest.Proxy = NFe.Components.Proxy.DefinirProxy(ConfiguracaoApp.ProxyServidor, ConfiguracaoApp.ProxyUsuario, ConfiguracaoApp.ProxySenha, ConfiguracaoApp.ProxyPorta, ConfiguracaoApp.DetectarConfiguracaoProxyAuto);
                            }

                            webRequest.Timeout = 10000; //10 segundos, se não conseguir, vai abortar para não atrabalhar o sistema. Wandrey

                            // Atribuir autenticação padrão para a recuperação do arquivo
                            webRequest.Credentials = CredentialCache.DefaultCredentials;

                            // Obter a resposta do servidor
                            webResponse = (HttpWebResponse)webRequest.GetResponse();
                            break;
                        }
                        catch
                        {
                            if(i == 0)
                            {
                                URL = URL2;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    // Perguntar ao servidor o tamanho do arquivo que será baixado
                    var fileSize = webResponse.ContentLength;

                    // Abrir a URL para download
                    strResponse = Client.OpenRead(URL);

                    // Criar um novo arquivo a partir do fluxo de dados que será salvo na local disk
                    var arqSefazIncTemp = LocalFile.Replace(".inc", ".tmp");
                    File.Delete(arqSefazIncTemp);
                    strLocal = new FileStream(arqSefazIncTemp, FileMode.Create, FileAccess.Write, FileShare.Read);

                    // Ele irá armazenar o número atual de bytes recuperados do servidor
                    var bytesSize = 0;

                    // Um buffer para armazenar e gravar os dados recuperados do servidor
                    var downBuffer = new byte[2048];

                    // Loop através do buffer - Até que o buffer esteja vazio
                    while((bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        // Gravar os dados do buffer no disco rigido
                        strLocal.Write(downBuffer, 0, bytesSize);
                    }

                    if(File.Exists(arqSefazIncTemp))
                    {
                        strLocal.Close();
                        if(new FileInfo(arqSefazIncTemp).Length > 0)
                        {
                            File.Copy(arqSefazIncTemp, LocalFile, true);
                        }

                        File.Delete(arqSefazIncTemp);
                    }
                }
                catch(IOException ex)
                {
                    Auxiliar.WriteLog(ex.Message, true);
                    result = false;
                }
                catch(WebException ex)
                {
                    Auxiliar.WriteLog(ex.Message, true);
                    result = false;
                }
                catch(Exception ex)
                {
                    Auxiliar.WriteLog(ex.Message, true);
                    result = false;
                }
                finally
                {
                    // Encerrar as streams
                    if(strResponse != null)
                    {
                        strResponse.Close();
                    }

                    if(strLocal != null)
                    {
                        strLocal.Close();
                    }

                    webRequest.Abort();

                    if(webResponse != null)
                    {
                        webResponse.Close();
                    }
                }

                if(result)
                {
                    if(Empresas.Configuracoes != null)
                    {
                        foreach(var empresa in Empresas.Configuracoes)
                        {
                            var uf = Empresas.GetUF(empresa.UnidadeFederativaCodigo);
                            if(uf != null)
                            {
                                empresa.URLConsultaDFe = ConfiguracaoApp.CarregarURLConsultaDFe(uf);
                            }
                        }
                    }
                }
            }
        }

        #endregion URLs do Estados p/ ConsultaDFe

        #region CarregarDados()

        /// <summary>
        /// Carrega as configurações realizadas na Aplicação gravadas no XML UniNfeConfig.xml para
        /// propriedades, para facilitar a leitura das informações necessárias para as transações da NF-e.
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// </remarks>
        public static void CarregarDados()
        {
            var vArquivoConfig = Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig;
            XmlDocument doc = null;
            if(File.Exists(vArquivoConfig))
            {
                try
                {
                    doc = new XmlDocument();
                    doc.Load(vArquivoConfig);

                    XmlNodeList configList = null;

                    configList = doc.GetElementsByTagName(NFeStrConstants.nfe_configuracoes);

                    foreach(XmlNode nodeConfig in configList)
                    {
                        var elementConfig = (XmlElement)nodeConfig;

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.DetectarProxyAuto.ToString())[0] != null)
                        {
                            ConfiguracaoApp.DetectarConfiguracaoProxyAuto = Convert.ToBoolean(elementConfig[NfeConfiguracoes.DetectarProxyAuto.ToString()].InnerText);
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.Proxy.ToString())[0] != null)
                        {
                            ConfiguracaoApp.Proxy = Convert.ToBoolean(elementConfig[NfeConfiguracoes.Proxy.ToString()].InnerText);
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxyServidor.ToString())[0] != null)
                        {
                            ConfiguracaoApp.ProxyServidor = elementConfig[NfeConfiguracoes.ProxyServidor.ToString()].InnerText.Trim();
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxyUsuario.ToString())[0] != null)
                        {
                            ConfiguracaoApp.ProxyUsuario = elementConfig[NfeConfiguracoes.ProxyUsuario.ToString()].InnerText.Trim();
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxySenha.ToString())[0] != null)
                        {
                            ConfiguracaoApp.ProxySenha = Criptografia.descriptografaSenha(elementConfig[NfeConfiguracoes.ProxySenha.ToString()].InnerText.Trim());
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxyPorta.ToString())[0] != null)
                        {
                            ConfiguracaoApp.ProxyPorta = Convert.ToInt32(elementConfig[NfeConfiguracoes.ProxyPorta.ToString()].InnerText.Trim());
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.SenhaConfig.ToString())[0] != null)
                        {
                            ConfiguracaoApp.SenhaConfig = elementConfig[NfeConfiguracoes.SenhaConfig.ToString()].InnerText.Trim();
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.ChecarConexaoInternet.ToString())[0] != null)
                        {
                            ConfiguracaoApp.ChecarConexaoInternet = Convert.ToBoolean(elementConfig[NfeConfiguracoes.ChecarConexaoInternet.ToString()].InnerText);
                        }
                        else
                        {
                            ConfiguracaoApp.ChecarConexaoInternet = true;
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString())[0] != null)
                        {
                            ConfiguracaoApp.GravarLogOperacoesRealizadas = Convert.ToBoolean(elementConfig[NfeConfiguracoes.GravarLogOperacaoRealizada.ToString()].InnerText);
                        }

                        if(elementConfig.GetElementsByTagName(NfeConfiguracoes.ConfirmaSaida.ToString())[0] != null)
                        {
                            ConfiguracaoApp.ConfirmaSaida = Convert.ToBoolean(elementConfig[NfeConfiguracoes.ConfirmaSaida.ToString()].InnerText);
                        }
                        else
                        {
                            ConfiguracaoApp.ConfirmaSaida = true;
                        }
                    }
                }
                catch(Exception ex)
                {
                    ///
                    /// danasa 8-2009
                    /// como reportar ao usuario que houve erro de leitura do arquivo de configuracao?
                    /// tem um usuário que postou um erro de leitura deste arquivo e não sabia como resolver.
                    ///
                    ///
                    /// danasa 8-2009
                    ///
                    if(!Propriedade.ServicoRodando || Propriedade.ExecutandoPeloUniNFe)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                finally
                {
                    if(doc != null)
                    {
                        doc = null;
                    }
                }
            }
            else
            {
                ChecarConexaoInternet = true;
            }
            //Carregar a lista de webservices disponíveis
            try
            {
                WebServiceProxy.CarregaWebServicesList();
            }
            catch(Exception ex)
            {
                Auxiliar.WriteLog(ex.Message, false);
            }
        }

        #endregion CarregarDados()

        #region CarregarDadosSobre()

        /// <summary>
        /// Carrega informações da tela de sobre
        /// </summary>
        /// <remarks>
        /// Autor: Leandro Souza
        /// </remarks>
        public static void CarregarDadosSobre()
        {
            var vArquivoConfig = Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfigSobre;

            //ConfiguracaoApp.NomeEmpresa = "Unimake Software";
            //ConfiguracaoApp.Site = "www.unimake.com.br";
            //ConfiguracaoApp.SiteProduto = ConfiguracaoApp.Site + "/uninfe";
            //ConfiguracaoApp.Email = "nfe@unimake.com.br";

            if(File.Exists(vArquivoConfig))
            {
                XmlTextReader oLerXml = null;
                try
                {
                    //Carregar os dados do arquivo XML de configurações da Aplicação
                    oLerXml = new XmlTextReader(vArquivoConfig);

                    while(oLerXml.Read())
                    {
                        if(oLerXml.NodeType == XmlNodeType.Element)
                        {
                            if(oLerXml.Name == NFeStrConstants.nfe_configuracoes)
                            {
                                while(oLerXml.Read())
                                {
                                    if(oLerXml.NodeType == XmlNodeType.Element)
                                    {
                                        if(oLerXml.Name == "NomeEmpresa") { oLerXml.Read(); ConfiguracaoApp.NomeEmpresa = oLerXml.Value; }
                                        else if(oLerXml.Name == "Site") { oLerXml.Read(); ConfiguracaoApp.Site = oLerXml.Value.Trim(); }
                                        else if(oLerXml.Name == "SiteProduto") { oLerXml.Read(); ConfiguracaoApp.SiteProduto = oLerXml.Value.Trim(); }
                                        else if(oLerXml.Name == "Email") { oLerXml.Read(); ConfiguracaoApp.Email = oLerXml.Value.Trim(); }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if(oLerXml != null)
                    {
                        oLerXml.Close();
                    }
                }
            }
        }

        #endregion CarregarDadosSobre()

        #region DefinirWS()

        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <returns>Retorna o objeto do WebService</returns>
        public static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int cMunicipio) => DefinirWS(servico, emp, cUF, tpAmb, 1, PadroesNFSe.NaoIdentificado, string.Empty, string.Empty, cMunicipio);

        #endregion DefinirWS()

        #region DefinirWS()

        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <param name="tpEmis">Tipo de emissão do documento fiscal</param>
        /// <param name="mod">Modelo do documento fiscal (55=NFe, 65=NFCe, etc...)</param>
        /// <returns>Retorna o objeto do WebService</returns>
        public static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int tpEmis, int cMunicipio) => DefinirWS(servico, emp, cUF, tpAmb, tpEmis, PadroesNFSe.NaoIdentificado, string.Empty, string.Empty, cMunicipio);

        #endregion DefinirWS()

        #region DefinirWS()

        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <param name="tpEmis">Tipo de emissão do documento fiscal</param>
        /// <param name="versao">Versão do XML</param>
        /// <returns>Retorna o objeto do WebService</returns>
        public static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int tpEmis, string versao, int cMunicipio) => DefinirWS(servico, emp, cUF, tpAmb, tpEmis, PadroesNFSe.NaoIdentificado, versao, string.Empty, cMunicipio);

        #endregion DefinirWS()

        #region DefinirWS()

        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <param name="tpEmis">Tipo de emissão do documento fiscal</param>
        /// <param name="versao">Versão do XML</param>
        /// <param name="mod">Modelo do documento fiscal (55=NFe, 65=NFCe, etc...)</param>
        /// <returns>Retorna o objeto do WebService</returns>
        public static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int tpEmis, string versao, string mod, int cMunicipio) => DefinirWS(servico, emp, cUF, tpAmb, tpEmis, PadroesNFSe.NaoIdentificado, versao, mod, cMunicipio);

        #endregion DefinirWS()

        #region DefinirWS()

        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <param name="tpEmis">Tipo de emissão do documento fiscal</param>
        /// <param name="padraoNFSe">Padrão da NFSe</param>
        /// <returns>Retorna o objeto do WebService</returns>
        public static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int tpEmis, PadroesNFSe padraoNFSe, int cMunicipio) => DefinirWS(servico, emp, cUF, tpAmb, tpEmis, padraoNFSe, string.Empty, string.Empty, cMunicipio);

        #endregion DefinirWS()

        #region DefinirWS()

        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <param name="tpEmis">Tipo de emissão do XML</param>
        /// <param name="padraoNFSe">Padrão da NFSe</param>
        /// <param name="versao">Versão do XML</param>
        /// <param name="mod">Modelo do documento fiscal (55=NFe, 65=NFCe, etc...)</param>
        /// <returns>Retorna o objeto do WebService</returns>
        private static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int tpEmis, PadroesNFSe padraoNFSe, string versao, string mod, int cMunicipio)
        {
            WebServiceProxy wsProxy = null;
            var key = servico + " " + cUF + " " + tpAmb + " " + tpEmis + (!string.IsNullOrEmpty(versao) ? " " + versao : "") + (!string.IsNullOrEmpty(mod) ? " " + mod : "");

            lock(Smf.WebProxy)
            {
                if(Empresas.Configuracoes[emp].WSProxy.ContainsKey(key))
                {
                    ServicePointManager.SecurityProtocol = WebServiceProxy.DefinirProtocoloSeguranca(cUF, tpAmb, tpEmis, padraoNFSe, servico);

                    wsProxy = Empresas.Configuracoes[emp].WSProxy[key];
                }
                else
                {
                    Thread.Sleep(1000); //1 segundo

                    //Definir a URI para conexão com o Webservice
                    var Url = ConfiguracaoApp.DefLocalWSDL(cUF, tpAmb, tpEmis, versao, servico, mod);

                    wsProxy = new WebServiceProxy(cUF,
                                                  Url,
                                                  Empresas.Configuracoes[emp].X509Certificado,
                                                  padraoNFSe,
                                                  (tpAmb == (int)TipoAmbiente.taHomologacao),
                                                  servico,
                                                  tpEmis,
                                                  cMunicipio);

                    Empresas.Configuracoes[emp].WSProxy.Add(key, wsProxy);
                }
            }
            return wsProxy;
        }

        #endregion DefinirWS()

        #region DefLocalWSDL()

        /// <summary>
        /// Definir o local do WSDL do webservice
        /// </summary>
        /// <param name="CodigoUF">Código da UF que é para pesquisar a URL do WSDL</param>
        /// <param name="tipoAmbiente">Tipo de ambiente da NFe</param>
        /// <param name="tipoEmissao">Tipo de Emissao da NFe</param>
        /// <param name="servico">Serviço da NFe que está sendo executado</param>
        /// <param name="versao">Versão do XML</param>
        /// <param name="ehNFCe">Se é NFCe (Nota Fiscal de Consumidor Eletrônica)</param>
        /// <returns>Retorna a URL</returns>
        private static string DefLocalWSDL(int CodigoUF, int tipoAmbiente, int tipoEmissao, string versao, Servicos servico, string modelo)
        {
            var ehNFCe = (modelo == "65");
            var WSDL = string.Empty;

            switch((TipoEmissao)tipoEmissao)
            {
                case TipoEmissao.teSVCRS:
                    CodigoUF = 902;
                    break;

                case TipoEmissao.teSVCSP:
                    CodigoUF = 903;
                    break;

                case TipoEmissao.teSVCAN:
                    CodigoUF = 904;
                    break;

                case TipoEmissao.teEPEC:
                    switch(servico)
                    {
                        case Servicos.EventoEPEC:
                            //Se for NFCe em São Paulo, tem um ambiente para EPEC exclusivo.
                            CodigoUF = (CodigoUF == 35 && ehNFCe ? 906 : 905);
                            break;

                        case Servicos.CTeRecepcaoEvento:
                            CodigoUF = (CodigoUF == 43 ? 902 : 903);
                            break;
                    }
                    break;

                default:
                    break;
            }
            var ufNome = CodigoUF.ToString();  //danasa 20-9-2010

            #region varre a lista de webservices baseado no codigo da UF

            if(WebServiceProxy.webServicesList.Count == 0)
            {
                throw new Exception("Lista de webservices não foi processada verifique se o arquivo 'WebService.xml' existe na pasta WSDL do UniNFe");
            }

            var alist = (from p in WebServiceProxy.webServicesList where p.ID == CodigoUF select p);

            foreach(var list in alist)
            {
                if(list.ID == CodigoUF)
                {
                    switch(servico)
                    {
                        #region NFe

                        case Servicos.ConsultaCadastroContribuinte:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeConsultaCadastro : list.LocalProducao.NFeConsultaCadastro);
                            break;

                        case Servicos.NFeEnviarLote:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeAutorizacao : list.LocalProducao.NFeAutorizacao);
                            break;

                        case Servicos.NFeInutilizarNumeros:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeInutilizacao : list.LocalProducao.NFeInutilizacao);
                            break;

                        case Servicos.NFePedidoConsultaSituacao:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeConsulta : list.LocalProducao.NFeConsulta);
                            break;

                        case Servicos.NFeConsultaStatusServico:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeStatusServico : list.LocalProducao.NFeStatusServico);
                            break;

                        case Servicos.NFePedidoSituacaoLote:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRetAutorizacao : list.LocalProducao.NFeRetAutorizacao);
                            break;

                        case Servicos.EventoCCe:
                        case Servicos.EventoCancelamento:
                        case Servicos.EventoEPEC:
                        case Servicos.EventoRecepcao:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRecepcaoEvento : list.LocalProducao.NFeRecepcaoEvento);
                            break;

                        case Servicos.EventoManifestacaoDest:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeManifDest : list.LocalProducao.NFeManifDest);
                            break;

                        case Servicos.DFeEnviar:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.DFeRecepcao : list.LocalProducao.DFeRecepcao);
                            break;

                        #endregion NFe

                        #region MDF-e

                        case Servicos.MDFeConsultaStatusServico:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeStatusServico : list.LocalProducao.MDFeStatusServico);
                            break;

                        case Servicos.MDFeEnviarLote:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeRecepcao : list.LocalProducao.MDFeRecepcao);
                            break;

                        case Servicos.MDFeEnviarLoteSinc:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeRecepcaoSinc : list.LocalProducao.MDFeRecepcaoSinc);
                            break;

                        case Servicos.MDFePedidoSituacaoLote:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeRetRecepcao : list.LocalProducao.MDFeRetRecepcao);
                            break;

                        case Servicos.MDFePedidoConsultaSituacao:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeConsulta : list.LocalProducao.MDFeConsulta);
                            break;

                        case Servicos.MDFeRecepcaoEvento:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeRecepcaoEvento : list.LocalProducao.MDFeRecepcaoEvento);
                            break;

                        case Servicos.MDFeConsultaNaoEncerrado:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeNaoEncerrado : list.LocalProducao.MDFeNaoEncerrado);
                            break;

                        #endregion MDF-e

                        #region CT-e

                        case Servicos.CTeConsultaStatusServico:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeStatusServico : list.LocalProducao.CTeStatusServico);
                            break;

                        case Servicos.CTeEnviarLote:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeRecepcao : list.LocalProducao.CTeRecepcao);
                            break;

                        case Servicos.CTePedidoSituacaoLote:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeRetRecepcao : list.LocalProducao.CTeRetRecepcao);
                            break;

                        case Servicos.CTePedidoConsultaSituacao:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeConsulta : list.LocalProducao.CTeConsulta);
                            break;

                        case Servicos.CTeInutilizarNumeros:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeInutilizacao : list.LocalProducao.CTeInutilizacao);
                            break;

                        case Servicos.CTeRecepcaoEvento:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeRecepcaoEvento : list.LocalProducao.CTeRecepcaoEvento);
                            break;

                        case Servicos.CTeDistribuicaoDFe:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeDistribuicaoDFe : list.LocalProducao.CTeDistribuicaoDFe);
                            break;

                        case Servicos.CteRecepcaoOS:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CteRecepcaoOS : list.LocalProducao.CteRecepcaoOS);
                            break;

                        #endregion CT-e

                        #region NFS-e

                        case Servicos.NFSeRecepcionarLoteRps:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.RecepcionarLoteRps : list.LocalProducao.RecepcionarLoteRps);
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarSituacaoLoteRps : list.LocalProducao.ConsultarSituacaoLoteRps);
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarNfsePorRps : list.LocalProducao.ConsultarNfsePorRps);
                            break;

                        case Servicos.NFSeConsultar:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarNfse : list.LocalProducao.ConsultarNfse);
                            break;

                        case Servicos.NFSeConsultarLoteRps:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarLoteRps : list.LocalProducao.ConsultarLoteRps);
                            break;

                        case Servicos.NFSeCancelar:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CancelarNfse : list.LocalProducao.CancelarNfse);
                            break;

                        case Servicos.NFSeConsultarURL:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarURLNfse : list.LocalProducao.ConsultarURLNfse);
                            break;

                        case Servicos.NFSeConsultarURLSerie:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarURLNfse : list.LocalProducao.ConsultarURLNfse);
                            break;

                        case Servicos.NFSeConsultarNFSePNG:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarNFSePNG : list.LocalProducao.ConsultarNFSePNG);
                            break;

                        case Servicos.NFSeInutilizarNFSe:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.InutilizarNFSe : list.LocalProducao.InutilizarNFSe);
                            break;

                        case Servicos.NFSeConsultarNFSePDF:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarNFSePDF : list.LocalProducao.ConsultarNFSePDF);
                            break;

                        case Servicos.NFSeObterNotaFiscal:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ObterNotaFiscal : list.LocalProducao.ObterNotaFiscal);
                            break;

                        case Servicos.NFSeConsultaSequenciaLoteNotaRPS:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultaSequenciaLoteNotaRPS : list.LocalProducao.ConsultaSequenciaLoteNotaRPS);
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.SubstituirNfse : list.LocalProducao.SubstituirNfse);
                            break;

                        case Servicos.NFSeConsultarNFSeTomados:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultaNFSeTomados : list.LocalProducao.ConsultaNFSeTomados);
                            break;

                        case Servicos.NFSeConsultarStatusNota:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarStatusNFse : list.LocalProducao.ConsultarStatusNFse);
                            break;

                        #endregion NFS-e

                        #region CFS-e

                        case Servicos.RecepcionarLoteCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.RecepcionarLoteCfse : list.LocalProducao.RecepcionarLoteCfse);
                            break;

                        case Servicos.CancelarCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.CancelarCfse : list.LocalProducao.CancelarCfse);
                            break;

                        case Servicos.ConsultarLoteCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarLoteCfse : list.LocalProducao.ConsultarLoteCfse);
                            break;

                        case Servicos.ConsultarCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarCfse : list.LocalProducao.ConsultarCfse);
                            break;

                        case Servicos.ConfigurarTerminalCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConfigurarTerminalCfse : list.LocalProducao.ConfigurarTerminalCfse);
                            break;

                        case Servicos.EnviarInformeManutencaoCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.EnviarInformeManutencaoCfse : list.LocalProducao.EnviarInformeManutencaoCfse);
                            break;

                        case Servicos.InformeTrasmissaoSemMovimentoCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.InformeTrasmissaoSemMovimentoCfse : list.LocalProducao.InformeTrasmissaoSemMovimentoCfse);
                            break;

                        case Servicos.ConsultarDadosCadastroCfse:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarDadosCadastroCfse : list.LocalProducao.ConsultarDadosCadastroCfse);
                            break;

                        #endregion CFS-e

                        #region LMC

                        case Servicos.LMCAutorizacao:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.LMCAutorizacao : list.LocalProducao.LMCAutorizacao);
                            break;

                        #endregion LMC

                        #region EFDReinf

                        case Servicos.RecepcaoLoteReinf:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.RecepcaoLoteReinf : list.LocalProducao.RecepcaoLoteReinf);
                            break;

                        case Servicos.ConsultasReinf:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultasReinf : list.LocalProducao.ConsultasReinf);
                            break;

                        #endregion EFDReinf

                        #region eSocial

                        case Servicos.RecepcaoLoteeSocial:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.RecepcaoLoteeSocial : list.LocalProducao.RecepcaoLoteeSocial);
                            break;

                        case Servicos.ConsultarLoteeSocial:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarLoteeSocial : list.LocalProducao.ConsultarLoteeSocial);
                            break;

                        case Servicos.ConsultarIdentificadoresEventoseSocial:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarIdentificadoresEventoseSocial : list.LocalProducao.ConsultarIdentificadoresEventoseSocial);
                            break;

                        case Servicos.DownloadEventoseSocial:
                            WSDL = (tipoAmbiente == (int)TipoAmbiente.taHomologacao ? list.LocalHomologacao.DownloadEventoseSocial : list.LocalProducao.DownloadEventoseSocial);
                            break;

                            #endregion eSocial
                    }
                    if(tipoEmissao == (int)TipoEmissao.teEPEC)
                    {
                        ufNome = "EPEC";
                    }
                    else
                    {
                        ufNome = "de " + list.Nome;
                    }

                    break;
                }
            }

            #endregion varre a lista de webservices baseado no codigo da UF

            //Se for NFCe tenho que ver se o estado não tem WSDL específico, ou seja, diferente da NFe
            //Wandrey 17/04/2014
            if(ehNFCe && !string.IsNullOrEmpty(WSDL) && File.Exists(WSDL))
            {
                var temp = Path.Combine(Path.GetDirectoryName(WSDL), Path.GetFileNameWithoutExtension(WSDL) + "_C" + Path.GetExtension(WSDL));
                if(File.Exists(temp))
                {
                    WSDL = temp;
                }
            }

            if(!string.IsNullOrEmpty(versao) && !string.IsNullOrEmpty(WSDL) && File.Exists(WSDL))
            {
                var temp = string.Empty;

                if(versao.Equals("4.00"))
                {
                    temp = Path.Combine(Path.GetDirectoryName(WSDL), Path.GetFileNameWithoutExtension(WSDL) + "_400" + Path.GetExtension(WSDL));
                }
                else if(versao.Equals("2.01") || versao.Equals("2.00"))
                {
                    temp = Path.Combine(Path.GetDirectoryName(WSDL), Path.GetFileNameWithoutExtension(WSDL) + "_200" + Path.GetExtension(WSDL));
                }

                if(!string.IsNullOrEmpty(temp) && File.Exists(temp))
                {
                    WSDL = temp;
                }
            }

            Console.WriteLine("wsdl: " + WSDL);

            if(string.IsNullOrEmpty(WSDL) || !File.Exists(WSDL))
            {
                if(!File.Exists(WSDL) && !string.IsNullOrEmpty(WSDL))
                {
                    throw new Exception("O arquivo \"" + WSDL + "\" não existe. Aconselhamos a reinstalação do UniNFe.");
                }

                var Ambiente = string.Empty;
                switch((NFe.Components.TipoAmbiente)tipoAmbiente)
                {
                    case NFe.Components.TipoAmbiente.taProducao:
                        Ambiente = "produção";
                        break;

                    case NFe.Components.TipoAmbiente.taHomologacao:
                        Ambiente = "homologação";
                        break;
                }
                var errorStr = "O Estado " + ufNome + " ainda não dispõe do serviço {0} para o ambiente de " + Ambiente + ".";

                switch(servico)
                {
                    case Servicos.DFeEnviar:
                        throw new Exception(string.Format(errorStr, "de envio de eventos da DFe"));

                    case Servicos.EventoCCe:
                    case Servicos.EventoCancelamento:
                    case Servicos.EventoRecepcao:
                    case Servicos.EventoManifestacaoDest:
                    case Servicos.EventoEPEC:
                        throw new Exception(string.Format(errorStr, "de envio de eventos da NFe"));

                    case Servicos.MDFeConsultaStatusServico:
                    case Servicos.MDFePedidoConsultaSituacao:
                    case Servicos.MDFePedidoSituacaoLote:
                    case Servicos.MDFeEnviarLote:
                    case Servicos.MDFeEnviarLoteSinc:
                    case Servicos.MDFeRecepcaoEvento:
                    case Servicos.MDFeConsultaNaoEncerrado:
                        throw new Exception(string.Format(errorStr, "do MDF-e"));

                    case Servicos.CTeConsultaStatusServico:
                    case Servicos.CTePedidoConsultaSituacao:
                    case Servicos.CTePedidoSituacaoLote:
                    case Servicos.CTeEnviarLote:
                    case Servicos.CTeRecepcaoEvento:
                    case Servicos.CTeInutilizarNumeros:
                        throw new Exception(string.Format(errorStr, "do CT-e"));

                    default:
                        switch(servico)
                        {
                            case Servicos.NFSeCancelar:
                            case Servicos.NFSeConsultarURL:
                            case Servicos.NFSeConsultarURLSerie:
                            case Servicos.NFSeConsultarLoteRps:
                            case Servicos.NFSeConsultar:
                            case Servicos.NFSeConsultarPorRps:
                            case Servicos.NFSeConsultarSituacaoLoteRps:
                            case Servicos.NFSeRecepcionarLoteRps:
                            case Servicos.NFSeConsultarStatusNota:
                                throw new Exception(string.Format(errorStr, "da NFS-e"));
                        }
                        throw new Exception(string.Format(errorStr, "da NF-e"));
                }
            }

            return WSDL;
        }

        #endregion DefLocalWSDL()

        #region GravarConfig()

        /// <summary>
        /// Método responsável por gravar as configurações da Aplicação no arquivo "UniNfeConfig.xml"
        /// </summary>
        ////public void GravarConfig(bool gravaArqEmpresa, bool validaCertificado)  //<<<<<<danasa 1-5-2011
        ////{
        ////    ValidarConfig(validaCertificado, null);
        ////    GravarConfigGeral();
        ////    foreach (Empresa empresa in Empresas.Configuracoes)
        ////    {
        ////        empresa.SalvarConfiguracao(false, false);
        ////    }
        ////    if (gravaArqEmpresa)        //<<<<<<danasa 1-5-2011
        ////    {                           //<<<<<<danasa 1-5-2011
        ////        GravarArqEmpresas();    //<<<<<<danasa 1-5-2011
        ////    }                           //<<<<<<danasa 1-5-2011
        ////}

        #endregion GravarConfig()

        #region Gravar XML com as empresas cadastradas

        public void GravarArqEmpresas()
        {
            // Não vou mais excluir para evitar problema, vou deixar o arquivo ser regravado em branco, na sequencia. Wandrey 18/01/2021
            //if (Empresas.Configuracoes.Count == 0 && File.Exists(Propriedade.NomeArqEmpresas))
            //{
            //    try
            //    {
            //        File.Delete(Propriedade.NomeArqEmpresas);
            //    }
            //    catch { }
            //    return;
            //}

            var empresasele = new XElement("Empresa");
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));

            foreach(var empresa in Empresas.Configuracoes)
            {
                empresasele.Add(new XElement(NFe.Components.NFeStrConstants.Registro,
                                new XAttribute(NFe.Components.TpcnResources.CNPJ.ToString(), empresa.CNPJ),
                                new XAttribute(NFe.Components.NFeStrConstants.Servico, ((int)empresa.Servico).ToString()),
                                new XElement(NFe.Components.NFeStrConstants.Nome, empresa.Nome.Trim())));
            }
            xml.Add(empresasele);
            xml.Save(Propriedade.NomeArqEmpresas);

            if(new FileInfo(Propriedade.NomeArqEmpresas).Length > 0)
            {
                try
                {
                    var nomeArq2 = Propriedade.PastaExecutavel + "\\UniNfeEmpresa2.xml";
                    var nomeArq3 = Propriedade.PastaExecutavel + "\\UniNfeEmpresa3.xml";

                    if(!Empresas.AbrirArqEmpresa(nomeArq2))
                    {
                        if(!Empresas.AbrirArqEmpresa(nomeArq3))
                        {
                            if(!Empresas.AbrirArqEmpresa())
                            {
                                throw new Exception("Falha ao tentar abrir o arquivo de empresas (UniNFeEmpresa.xml).");
                            }
                        }
                    }

                    File.Copy(Propriedade.NomeArqEmpresas, nomeArq2, true);
                    File.Copy(Propriedade.NomeArqEmpresas, nomeArq3, true);
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion Gravar XML com as empresas cadastradas

        #region GravarConfigGeral()

        /// <summary>
        /// Gravar as configurações gerais
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        public void GravarConfigGeral(bool configuracaoPorArquivo = false)
        {
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var elementos = new XElement(NFeStrConstants.nfe_configuracoes);
            elementos.Add(new XElement(NfeConfiguracoes.DetectarProxyAuto.ToString(), ConfiguracaoApp.DetectarConfiguracaoProxyAuto.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.Proxy.ToString(), ConfiguracaoApp.Proxy.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.ProxyServidor.ToString(), ConfiguracaoApp.ProxyServidor));
            elementos.Add(new XElement(NfeConfiguracoes.ProxyUsuario.ToString(), ConfiguracaoApp.ProxyUsuario));
            elementos.Add(new XElement(NfeConfiguracoes.ProxyPorta.ToString(), ConfiguracaoApp.ProxyPorta.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.ProxySenha.ToString(), Criptografia.criptografaSenha(ConfiguracaoApp.ProxySenha)));
            elementos.Add(new XElement(NfeConfiguracoes.ChecarConexaoInternet.ToString(), ConfiguracaoApp.ChecarConexaoInternet.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString(), ConfiguracaoApp.GravarLogOperacoesRealizadas.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.ConfirmaSaida.ToString(), ConfiguracaoApp.ConfirmaSaida.ToString()));
            if(!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig))
            {

                if(!configuracaoPorArquivo)
                {
                    ConfiguracaoApp.SenhaConfig = Functions.GerarMD5(ConfiguracaoApp.SenhaConfig);
                }

                elementos.Add(new XElement(NfeConfiguracoes.SenhaConfig.ToString(), ConfiguracaoApp.SenhaConfig));
            }
            xml.Add(elementos);
            xml.Save(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig);
        }

        #endregion GravarConfigGeral()

        #region ValidarConfig()

        internal class xValid
        {
            public bool valid;
            public string folder;
            public string msg1;
            public string msg2;

            public xValid(string _folder, string _msg1, string _msg2, bool _valid)
            {
                valid = _valid;
                msg1 = _msg1;
                msg2 = _msg2 + " - '" + (!string.IsNullOrEmpty(_folder) ? "VAZIO" : _folder) + "'";
                folder = _folder;
            }
        }

        private Dictionary<string, int> _folders;

        private string AddEmpresaNaLista(string folder)
        {
            try
            {
                if(!string.IsNullOrEmpty(folder))
                {
                    _folders.Add(folder.ToLower(), 0);
                }

                return "";
            }
            catch
            {
                return "Não é permitido a utilização de pastas idênticas na mesma ou entre empresas.\r\nPasta utilizada: \r\n" + folder;
            }
        }

        /// <summary>
        /// Verifica se algumas das informações das configurações tem algum problema ou falha
        /// </summary>
        /// <param name="validarCertificado">Se valida se tem certificado informado ou não nas configurações</param>
        public void ValidarConfig(bool validarCertificado, Empresa empresaValidada)
        {
            var erro = string.Empty;
            var validou = true;

            _folders = new Dictionary<string, int>();

            foreach(var emp in Empresas.Configuracoes)
            {
                #region Remover End Slash

                emp.RemoveEndSlash();

                #endregion Remover End Slash

                #region Verificar a duplicação de nome de pastas que não pode existir

                if((erro = AddEmpresaNaLista(emp.PastaXmlEnvio)) == "")
                {
                    if((erro = AddEmpresaNaLista(emp.PastaXmlRetorno)) == "")
                    {
                        if((erro = AddEmpresaNaLista(emp.PastaXmlErro)) == "")
                        {
                            if((erro = AddEmpresaNaLista(emp.PastaValidar)) == "")
                            {
                                if(emp.Servico != TipoAplicativo.Nfse)
                                {
                                    if((erro = AddEmpresaNaLista(emp.PastaXmlEnviado)) == "")
                                    {
                                        if((erro = AddEmpresaNaLista(emp.PastaXmlEmLote)) == "")
                                        {
                                            if((erro = AddEmpresaNaLista(emp.PastaBackup)) == "")
                                            {
                                                erro = AddEmpresaNaLista(emp.PastaDownloadNFeDest);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //if ((emp.Servico == TipoAplicativo.Nfe ||
                //    emp.Servico == TipoAplicativo.NFCe ||
                //    emp.Servico == TipoAplicativo.Todos)
                //    && emp.UnidadeFederativaCodigo == 35 && emp.IndSinc == true)
                //{
                //    erro += "\r\nEstado de São Paulo não dispõe do serviço síncrono para emissão de NFe.".TrimStart(new char[] { '\r', '\n' });
                //}

                if(erro != "")
                {
                    erro += "\r\nNa empresa: " + emp.Nome + "\r\n" + emp.CNPJ;
                    validou = false;
                    break;
                }

                #endregion Verificar a duplicação de nome de pastas que não pode existir
            }

            if(validou)
            {
                var empFrom = 0;
                var empTo = Empresas.Configuracoes.Count;

                if(empresaValidada != null)
                {
                    ///
                    /// quando alterada uma configuracao pelo visual, valida apenas a empresa sendo alterada
                    ///
                    empFrom = empTo = Empresas.FindConfEmpresaIndex(empresaValidada.CNPJ, empresaValidada.Servico);
                    if(empFrom == -1)
                    {
                        throw new Exception("Não foi possivel encontrar a empresa para validação");
                    }

                    ++empTo;

                    if(empresaValidada.Servico == TipoAplicativo.NFCe)
                    {
                        if(!string.IsNullOrEmpty(empresaValidada.IdentificadorCSC) && string.IsNullOrEmpty(empresaValidada.TokenCSC))
                        {
                            throw new Exception("É obrigatório informar o IDToken quando informado o CSC.");
                        }
                        else if(string.IsNullOrEmpty(empresaValidada.IdentificadorCSC) && !string.IsNullOrEmpty(empresaValidada.TokenCSC))
                        {
                            throw new Exception("É obrigatório informar o CSC quando informado o IDToken.");
                        }
                    }
                }

                for(var i = empFrom; i < empTo; i++)
                {
                    var empresa = Empresas.Configuracoes[i];

                    var xNomeCNPJ = "\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;

                    #region Verificar se tem alguma pasta em branco

                    var _xValids = new List<xValid>();

                    switch(empresa.Servico)
                    {
                        case TipoAplicativo.Nfse:
                            _xValids.Add(new xValid(empresa.PastaXmlEnvio, "Informe a pasta de envio dos arquivos XML.", "A pasta de envio dos arquivos XML informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlRetorno, "Informe a pasta de envio dos arquivos XML.", "A pasta de retorno dos arquivos XML informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlErro, "Informe a pasta para arquivamento temporário dos arquivos XML que apresentaram erros.", "A pasta para arquivamento temporário dos arquivos XML com erro informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaValidar, "Informe a pasta onde será gravado os arquivos XML somente para ser validado pela aplicação.", "A pasta para validação de XML´s informada não existe.", true));
                            break;

                        case TipoAplicativo.EFDReinf:
                        case TipoAplicativo.eSocial:
                        case TipoAplicativo.EFDReinfeSocial:
                            _xValids.Add(new xValid(empresa.PastaXmlEnvio, "Informe a pasta de envio dos arquivos XML.", "A pasta de envio dos arquivos XML informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlRetorno, "Informe a pasta de envio dos arquivos XML.", "A pasta de retorno dos arquivos XML informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlErro, "Informe a pasta para arquivamento temporário dos arquivos XML que apresentaram erros.", "A pasta para arquivamento temporário dos arquivos XML com erro informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaValidar, "Informe a pasta onde será gravado os arquivos XML somente para ser validado pela aplicação.", "A pasta para validação de XML´s informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlEnviado, "Informe a pasta para arquivamento dos arquivos XML enviados.", "A pasta para arquivamento dos arquivos XML enviados informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaBackup, "", "A pasta para backup dos XML enviados informada não existe.", false));
                            break;

                        case TipoAplicativo.Nfe:
                        case TipoAplicativo.Cte:
                        case TipoAplicativo.MDFe:
                        case TipoAplicativo.NFCe:
                        case TipoAplicativo.SAT:
                        case TipoAplicativo.Todos:
                        case TipoAplicativo.Nulo:
                        default:
                            _xValids.Add(new xValid(empresa.PastaXmlEnvio, "Informe a pasta de envio dos arquivos XML.", "A pasta de envio dos arquivos XML informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlRetorno, "Informe a pasta de envio dos arquivos XML.", "A pasta de retorno dos arquivos XML informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlErro, "Informe a pasta para arquivamento temporário dos arquivos XML que apresentaram erros.", "A pasta para arquivamento temporário dos arquivos XML com erro informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaValidar, "Informe a pasta onde será gravado os arquivos XML somente para ser validado pela aplicação.", "A pasta para validação de XML´s informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlEmLote, "Informe a pasta de envio dos arquivos XML em lote.", "A pasta de envio das notas fiscais eletrônicas em lote informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaXmlEnviado, "Informe a pasta para arquivamento dos arquivos XML enviados.", "A pasta para arquivamento dos arquivos XML enviados informada não existe.", true));
                            _xValids.Add(new xValid(empresa.PastaBackup, "", "A pasta para backup dos XML enviados informada não existe.", false));
                            _xValids.Add(new xValid(empresa.PastaDownloadNFeDest, "", "A pasta para arquivamento das NFe de destinatários informada não existe.", false));
                            _xValids.Add(new xValid(empresa.PastaDanfeMon, "", "A pasta informada para gravação do XML da NFe para o DANFeMon não existe.", false));
                            _xValids.Add(new xValid(empresa.PastaExeUniDanfe, "", "A pasta do executável do UniDANFe informada não existe.", false));
                            _xValids.Add(new xValid(empresa.PastaConfigUniDanfe, "", "A pasta do arquivo de configurações do UniDANFe informada não existe.", false));
                            break;
                    }

                    foreach(var val in _xValids)
                    {
                        if(val.valid && string.IsNullOrEmpty(val.folder))
                        {
                            erro = val.msg1 + xNomeCNPJ;
                            validou = false;
                            break;
                        }
                        else
                            if(!string.IsNullOrEmpty(val.folder))
                        {
                            if(!Directory.Exists(val.folder))
                            {
                                if(empresa.CriaPastasAutomaticamente)
                                {
                                    Directory.CreateDirectory(val.folder);
                                }
                                else
                                {
                                    erro = val.msg2 + xNomeCNPJ;
                                    validou = false;
                                    break;
                                }
                            }
                        }
                    }

#if f
                    List<string> diretorios = new List<string>();
                    List<string> mensagens = new List<string>();

                    diretorios.Add(empresa.PastaXmlEnvio); mensagens.Add("Informe a pasta de envio dos arquivos XML.");
                    diretorios.Add(empresa.PastaXmlRetorno); mensagens.Add("Informe a pasta de retorno dos arquivos XML.");
                    diretorios.Add(empresa.PastaXmlErro); mensagens.Add("Informe a pasta para arquivamento temporário dos arquivos XML que apresentaram erros.");
                    diretorios.Add(empresa.PastaValidar); mensagens.Add("Informe a pasta onde será gravado os arquivos XML somente para ser validado pela Aplicação.");
                    if (empresa.Servico != TipoAplicativo.Nfse)
                    {
                        diretorios.Add(empresa.PastaXmlEmLote); mensagens.Add("Informe a pasta de envio dos arquivos XML em lote.");
                        diretorios.Add(empresa.PastaXmlEnviado); mensagens.Add("Informe a pasta para arquivamento dos arquivos XML enviados.");
                    }

                    for (int b = 0; b < diretorios.Count; b++)
                    {
                        if (diretorios[b].Equals(string.Empty))
                        {
                            erro = mensagens[b] + xNomeCNPJ;
                            validou = false;
                            break;
                        }
                    }
#endif
                    ///
                    /// informacoes do FTP
                    /// danasa 7/7/2011
                    if(empresa.FTPIsAlive && validou)
                    {
                        if(empresa.Servico != TipoAplicativo.Nfse)
                        {
                            if(string.IsNullOrEmpty(empresa.FTPPastaAutorizados))
                            {
                                erro = "Informe a pasta do FTP de destino dos autorizados" + xNomeCNPJ;
                                validou = false;
                            }
                        }
                        else
                            if(string.IsNullOrEmpty(empresa.FTPPastaRetornos))
                        {
                            erro = "Informe a pasta do FTP de destino dos retornos" + xNomeCNPJ;
                            validou = false;
                        }
                    }

                    #endregion Verificar se tem alguma pasta em branco

                    #region Verificar se o certificado foi informado

                    if(validarCertificado && empresa.UsaCertificado && validou)
                    {
                        if(empresa.CertificadoInstalado && empresa.CertificadoDigitalThumbPrint.Equals(string.Empty))
                        {
                            erro = "Selecione o certificado digital a ser utilizado na autenticação dos serviços." + xNomeCNPJ;
                            validou = false;
                        }
                        if(!empresa.CertificadoInstalado && validou)
                        {
                            if(empresa.CertificadoArquivo.Equals(string.Empty))
                            {
                                erro = "Informe o local de armazenamento do certificado digital a ser utilizado na autenticação dos serviços." + xNomeCNPJ;
                                validou = false;
                            }
                            else if(!File.Exists(empresa.CertificadoArquivo))
                            {
                                erro = "Arquivo do certificado digital a ser utilizado na autenticação dos serviços não foi encontrado." + xNomeCNPJ;
                                validou = false;
                            }
                            else if(empresa.CertificadoSenha.Equals(string.Empty))
                            {
                                erro = "Informe a senha do certificado digital a ser utilizado na autenticação dos serviços." + xNomeCNPJ;
                                validou = false;
                            }
                            else
                            {
                                try
                                {
                                    using(var fs = new FileStream(empresa.CertificadoArquivo, FileMode.Open, FileAccess.Read))
                                    {
                                        var buffer = new byte[fs.Length];
                                        fs.Read(buffer, 0, buffer.Length);
                                        empresa.X509Certificado = new X509Certificate2(buffer, empresa.CertificadoSenha);
                                    }
                                }
                                catch(System.Security.Cryptography.CryptographicException ex)
                                {
                                    erro = ex.Message + "." + xNomeCNPJ;
                                    validou = false;
                                }
                                catch(Exception ex)
                                {
                                    erro = ex.Message + "." + xNomeCNPJ;
                                    validou = false;
                                }
                            }
                        }
                    }

                    #endregion Verificar se o certificado foi informado

                    #region Verificar se as pastas informadas existem

                    if(validou)
                    {
                        //Fazer um pequeno ajuste na pasta de configuração do unidanfe antes de verificar sua existência
                        if(empresa.PastaConfigUniDanfe.Trim() != string.Empty)
                        {
                            if(!string.IsNullOrEmpty(empresa.PastaConfigUniDanfe))
                            {
                                while(empresa.PastaConfigUniDanfe.Substring(empresa.PastaConfigUniDanfe.Length - 6, 6).ToLower() == @"\dados" &&
                                    !string.IsNullOrEmpty(empresa.PastaConfigUniDanfe))
                                {
                                    empresa.PastaConfigUniDanfe = empresa.PastaConfigUniDanfe.Substring(0, empresa.PastaConfigUniDanfe.Length - 6);
                                }
                            }
                            empresa.PastaConfigUniDanfe = empresa.PastaConfigUniDanfe.Replace("\r\n", "").Trim();
                            //empresa.PastaConfigUniDanfe = empresa.PastaConfigUniDanfe;
                        }

                        if(empresa.PastaXmlEnvio.ToLower().EndsWith("geral"))
                        {
                            erro = "Pasta de envio não pode terminar com a subpasta 'geral'." + xNomeCNPJ;
                            validou = false;
                        }
                        else if(empresa.PastaXmlEmLote.ToLower().EndsWith("geral"))
                        {
                            erro = "Pasta de envio em lote não pode terminar com a subpasta 'geral'." + xNomeCNPJ;
                            validou = false;
                        }
                        else if(empresa.PastaValidar.ToLower().EndsWith("geral"))
                        {
                            erro = "Pasta de validação não pode terminar com a subpasta 'geral'." + xNomeCNPJ;
                            validou = false;
                        }
                        else if(empresa.PastaXmlEnvio.ToLower().EndsWith("temp"))
                        {
                            erro = "Pasta de envio não pode terminar com a subpasta 'temp'." + xNomeCNPJ;
                            validou = false;
                        }
                        else if(empresa.PastaXmlEmLote.ToLower().EndsWith("temp"))
                        {
                            erro = "Pasta de envio em lote não pode terminar com a subpasta 'temp'." + xNomeCNPJ;
                            validou = false;
                        }
                        else if(empresa.PastaValidar.ToLower().EndsWith("temp"))
                        {
                            erro = "Pasta de validação não pode terminar com a subpasta 'temp'." + xNomeCNPJ;
                            validou = false;
                        }
                        else if(empresa.PastaXmlErro.ToLower().EndsWith("temp"))
                        {
                            erro = "Pasta de XML's com erro na tentativa de envio não pode terminar com a subpasta 'temp'." + xNomeCNPJ;
                            validou = false;
                        }

#if f

                        if (validou)
                        {
                            diretorios.Clear(); mensagens.Clear();
                            diretorios.Add(empresa.PastaXmlEnvio.Trim()); mensagens.Add("A pasta de envio dos arquivos XML informada não existe.");
                            diretorios.Add(empresa.PastaXmlRetorno.Trim()); mensagens.Add("A pasta de retorno dos arquivos XML informada não existe.");
                            diretorios.Add(empresa.PastaXmlErro.Trim()); mensagens.Add("A pasta para arquivamento temporário dos arquivos XML com erro informada não existe.");
                            diretorios.Add(empresa.PastaValidar.Trim()); mensagens.Add("A pasta para validação de XML´s informada não existe.");
                            if (empresa.Servico != TipoAplicativo.Nfse)
                            {
                                diretorios.Add(empresa.PastaXmlEnviado.Trim()); mensagens.Add("A pasta para arquivamento dos arquivos XML enviados informada não existe.");
                                diretorios.Add(empresa.PastaBackup.Trim()); mensagens.Add("A pasta para backup dos XML enviados informada não existe.");
                                diretorios.Add(empresa.PastaDownloadNFeDest.Trim()); mensagens.Add("A pasta para arquivamento das NFe de destinatários informada não existe.");
                                diretorios.Add(empresa.PastaXmlEmLote.Trim()); mensagens.Add("A pasta de envio das notas fiscais eletrônicas em lote informada não existe.");
                                diretorios.Add(empresa.PastaDanfeMon.Trim()); mensagens.Add("A pasta informada para gravação do XML da NFe para o DANFeMon não existe.");
                                diretorios.Add(empresa.PastaExeUniDanfe.Trim()); mensagens.Add("A pasta do executável do UniDANFe informada não existe.");
                                diretorios.Add(empresa.PastaConfigUniDanfe.Trim()); mensagens.Add("A pasta do arquivo de configurações do UniDANFe informada não existe.");
                            }

                            for (int b = 0; b < diretorios.Count; b++)
                            {
                                if (!string.IsNullOrEmpty(diretorios[b]))
                                {
                                    if (!Directory.Exists(diretorios[b]))
                                    {
                                        if (empresa.CriaPastasAutomaticamente)
                                        {
                                            Directory.CreateDirectory(diretorios[b]);
                                        }
                                        else
                                        {
                                            erro = mensagens[b].Trim() + xNomeCNPJ;
                                            validou = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
#endif

                        #region Criar pasta Temp dentro da pasta de envio, envio em lote e validar

                        //Criar pasta Temp dentro da pasta de envio, envio em Lote e Validar. Wandrey 03/08/2011
                        if(validou)
                        {
                            if(Directory.Exists(empresa.PastaXmlEnvio.Trim()))
                            {
                                if(!Directory.Exists(empresa.PastaXmlEnvio.Trim() + "\\Temp"))
                                {
                                    Directory.CreateDirectory(empresa.PastaXmlEnvio.Trim() + "\\Temp");
                                }
                            }

                            if(Directory.Exists(empresa.PastaXmlEmLote.Trim()))
                            {
                                if(!Directory.Exists(empresa.PastaXmlEmLote.Trim() + "\\Temp"))
                                {
                                    Directory.CreateDirectory(empresa.PastaXmlEmLote.Trim() + "\\Temp");
                                }
                            }

                            if(Directory.Exists(empresa.PastaValidar.Trim()))
                            {
                                if(!Directory.Exists(empresa.PastaValidar.Trim() + "\\Temp"))
                                {
                                    Directory.CreateDirectory(empresa.PastaValidar.Trim() + "\\Temp");
                                }
                            }
                        }

                        #endregion Criar pasta Temp dentro da pasta de envio, envio em lote e validar
                    }

                    #endregion Verificar se as pastas informadas existem

                    #region Verificar se as pastas configuradas do unidanfe estão corretas

                    if(empresa.Servico != TipoAplicativo.Nfse && validou)
                    {
                        if(empresa.PastaExeUniDanfe.Trim() != string.Empty)
                        {
                            if(!File.Exists(empresa.PastaExeUniDanfe + "\\unidanfe.exe"))
                            {
                                erro = "O executável do UniDANFe não foi localizado na pasta informada." + xNomeCNPJ;
                                validou = false;
                            }
                        }

                        if(validou && empresa.PastaConfigUniDanfe.Trim() != string.Empty)
                        {
                            //Verificar a existência o arquivo de configuração
                            if(!File.Exists(empresa.PastaConfigUniDanfe + "\\dados\\ConfigBD.tps"))
                            {
                                erro = "O arquivo de configuração do UniDANFe não foi localizado na pasta informada." + xNomeCNPJ;
                                validou = false;
                            }
                        }
                    }

                    #endregion Verificar se as pastas configuradas do unidanfe estão corretas

                    #region Verificar se o IDToken informado é menor que 6 caracteres

                    if(!string.IsNullOrEmpty(empresa.TokenCSC) && empresa.TokenCSC.Length < 6)
                    {
                        erro = "O IDToken deve ter 6 caracteres." + xNomeCNPJ;
                        validou = false;
                    }

                    #endregion Verificar se o IDToken informado é menor que 6 caracteres
                }
            }

            #region Ticket: #110

            /* Validar se já existe uma instancia utilizando estes diretórios
             * Marcelo
             * 03/06/2013
             */
            if(validou)
            {
                //Se encontrar algum arquivo de lock nos diretórios, não permitir que seja executado
                try
                {
                    Empresas.CanRun(empresaValidada);
                }
                catch(NFe.Components.Exceptions.AppJaExecutando ex)
                {
                    erro = ex.Message;
                }

                validou = string.IsNullOrEmpty(erro);
            }

            #endregion Ticket: #110

            if(!validou)
            {
                throw new Exception(erro);
            }
        }

        #endregion ValidarConfig()

        #region ReconfigurarUniNFe()

        /// <summary>
        /// Método responsável por reconfigurar automaticamente o UniNFe a partir de um XML com as
        /// informações necessárias.
        /// O Método grava um arquivo na pasta de retorno do UniNFe com a informação se foi bem
        /// sucedida a reconfiguração ou não.
        /// </summary>
        /// <param name="cArquivoXml">Nome e pasta do arquivo de configurações gerado pelo ERP para atualização das configurações do uninfe</param>
        public void ReconfigurarUniNFe(string cArquivoXml)
        {
            var emp = Empresas.FindEmpresaByThread();

            var cStat = "";
            var xMotivo = "";
            var lErro = false;
            var lEncontrouTag = false;

            try
            {
                if(Path.GetExtension(cArquivoXml).ToLower() != ".txt" && ExcluirEmpresa(cArquivoXml))
                {
                    cStat = "1";
                    xMotivo = "Empresa excluída com sucesso";
                    lErro = false;

                    ConfiguracaoApp.CarregarDados();
                    ConfiguracaoApp.CarregarDadosSobre();
                    Empresas.CarregaConfiguracao();
                }
                else
                {
                    emp = CadastrarEmpresa(cArquivoXml, emp);

                    ///
                    /// danasa - 12/2019
                    /// Salva o serviço porque se o usuario informar novamente a tag->Servico 
                    /// e esta tag for diferente da tag->Servico da chave, evitamos erro
                    /// 
                    var currServico = Empresas.Configuracoes[emp].Servico;

                    if(Path.GetExtension(cArquivoXml).ToLower() == ".txt")
                    {
                        #region Formato TXT

                        var cLinhas = Functions.LerArquivo(cArquivoXml);

                        lEncontrouTag = Functions.PopulateClasse(Empresas.Configuracoes[emp], cLinhas);

                        foreach(var texto in cLinhas)
                        {
                            var dados = texto.Split('|');
                            var nElementos = dados.GetLength(0);
                            if(nElementos <= 1)
                            {
                                continue;
                            }

                            switch(dados[0].ToLower())
                            {
                                case "proxy": //Se a tag <Proxy> existir ele pega o novo conteúdo
                                    ConfiguracaoApp.Proxy = (nElementos == 2 ? Convert.ToBoolean(dados[1].Trim()) : false);
                                    lEncontrouTag = true;
                                    break;

                                case "proxyservidor": //Se a tag <ProxyServidor> existir ele pega o novo conteúdo
                                    ConfiguracaoApp.ProxyServidor = (nElementos == 2 ? dados[1].Trim() : "");
                                    lEncontrouTag = true;
                                    break;

                                case "proxyusuario": //Se a tag <ProxyUsuario> existir ele pega o novo conteúdo
                                    ConfiguracaoApp.ProxyUsuario = (nElementos == 2 ? dados[1].Trim() : "");
                                    lEncontrouTag = true;
                                    break;

                                case "proxysenha": //Se a tag <ProxySenha> existir ele pega o novo conteúdo
                                    ConfiguracaoApp.ProxySenha = (nElementos == 2 ? dados[1].Trim() : "");
                                    lEncontrouTag = true;
                                    break;

                                case "proxyporta": //Se a tag <ProxyPorta> existir ele pega o novo conteúdo
                                    ConfiguracaoApp.ProxyPorta = (nElementos == 2 ? Convert.ToInt32("0" + dados[1].Trim()) : 0);
                                    lEncontrouTag = true;
                                    break;

                                case "checarconexaointernet": //Se a tag <ChecarConexaoInternet> existir ele pega o novo conteúdo
                                    ConfiguracaoApp.ChecarConexaoInternet = (nElementos == 2 ? Convert.ToBoolean(dados[1].Trim()) : true);
                                    lEncontrouTag = true;
                                    break;

                                case "gravarlogoperacaorealizada":
                                    ConfiguracaoApp.GravarLogOperacoesRealizadas = (nElementos == 2 ? Convert.ToBoolean(dados[1].Trim()) : true);
                                    lEncontrouTag = true;
                                    break;

                                case "senhaconfig": //Se a tag <senhaconfig> existir ele pega o novo conteúdo
                                    ConfiguracaoApp.SenhaConfig = Functions.GerarMD5((nElementos == 2 ? dados[1].Trim() : ""));
                                    lEncontrouTag = true;
                                    break;
                            }
                        }

                        #endregion Formato TXT
                    }
                    else
                    {
                        #region Formato XML

                        var doc = new XmlDocument();
                        try
                        {
                            doc.Load(cArquivoXml);
                        }
                        catch
                        {
                            doc.LoadXml(System.IO.File.ReadAllText(cArquivoXml, System.Text.Encoding.UTF8));
                        }

                        var ConfUniNfeList = doc.GetElementsByTagName("altConfUniNFe");

                        foreach(XmlNode ConfUniNfeNode in ConfUniNfeList)
                        {
                            var ConfUniNfeElemento = (XmlElement)ConfUniNfeNode;
                            lEncontrouTag = Functions.PopulateClasse(Empresas.Configuracoes[emp], ConfUniNfeElemento);

                            //Se a tag <Proxy> existir ele pega o novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.Proxy.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.Proxy = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.Proxy.ToString())[0].InnerText);
                                lEncontrouTag = true;
                            }
                            //Se a tag <ProxyServidor> existir ele pega o novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyServidor.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.ProxyServidor = ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyServidor.ToString())[0].InnerText;
                                lEncontrouTag = true;
                            }
                            //Se a tag <ProxyUsuario> existir ele pega o novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyUsuario.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.ProxyUsuario = ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyUsuario.ToString())[0].InnerText;
                                lEncontrouTag = true;
                            }
                            //Se a tag <ProxySenha> existir ele pega o novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxySenha.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.ProxySenha = ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxySenha.ToString())[0].InnerText;
                                lEncontrouTag = true;
                            }
                            //Se a tag <ProxyPorta> existir ele pega o novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyPorta.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.ProxyPorta = Convert.ToInt32("0" + ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyPorta.ToString())[0].InnerText);
                                lEncontrouTag = true;
                            }
                            //Se a tag <ChecarConexaoInternet> existir ele pega o novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ChecarConexaoInternet.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.ChecarConexaoInternet = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ChecarConexaoInternet.ToString())[0].InnerText);
                                lEncontrouTag = true;
                            }
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.GravarLogOperacoesRealizadas = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString())[0].InnerText);
                                lEncontrouTag = true;
                            }
                            //Se a tag <SenhaConfig> existir ele pega no novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.SenhaConfig.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.SenhaConfig = Functions.GerarMD5(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.SenhaConfig.ToString())[0].InnerText);
                                lEncontrouTag = true;
                            }
                            //Se a tag <ConfirmaSaida> existir ele pega novo conteúdo
                            if(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ConfirmaSaida.ToString()).Count != 0)
                            {
                                ConfiguracaoApp.ConfirmaSaida = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ConfirmaSaida.ToString())[0].InnerText);
                                lEncontrouTag = true;
                            }
                        }

                        #endregion Formato XML
                    }
                    Empresas.Configuracoes[emp].Servico = currServico;

                    if(lEncontrouTag)
                    {
                        if(ConfiguracaoApp.Proxy &&
                            (ConfiguracaoApp.ProxyPorta == 0 ||
                            string.IsNullOrEmpty(ConfiguracaoApp.ProxyServidor) ||
                            string.IsNullOrEmpty(ConfiguracaoApp.ProxyUsuario) ||
                            string.IsNullOrEmpty(ConfiguracaoApp.ProxySenha)))
                        {
                            throw new Exception(NFeStrConstants.proxyError);
                        }
                        Empresas.Configuracoes[emp].RemoveEndSlash();
                        Empresas.CriarPasta(false);

                        //Se o certificado digital for o instalado no windows, vamos tentar buscar ele no repositório para ver se existe.
                        if(Empresas.Configuracoes[emp].CertificadoInstalado)
                        {
                            var oX509Cert = new X509Certificate2();
                            var store = new X509Store("MY", StoreLocation.CurrentUser);
                            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                            var collection = store.Certificates;
                            var collection1 = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                            var collection2 = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

                            //Primeiro tento encontrar pelo thumbprint
                            var collection3 = collection2.Find(X509FindType.FindByThumbprint, Empresas.Configuracoes[emp].CertificadoDigitalThumbPrint, false);
                            if(collection3.Count <= 0)
                            {
                                //Se não encontrou pelo thumbprint tento pelo SerialNumber pegando o mesmo thumbprint que veio no arquivo de configurações para ver se não encontro.
                                collection3 = collection2.Find(X509FindType.FindBySerialNumber, Empresas.Configuracoes[emp].CertificadoDigitalThumbPrint, false);

                                if(collection3.Count <= 0)
                                {
                                    throw new Exception("Certificado digital informado não foi localizado no repositório do windows.");
                                }

                                Empresas.Configuracoes[emp].CertificadoDigitalThumbPrint = collection3[0].Thumbprint;
                            }
                        }

                        ///
                        /// salva a configuracao da empresa
                        ///

                        Empresas.Configuracoes[emp].SalvarConfiguracao(false, true);

                        /// salva o arquivo da lista de empresas
                        GravarArqEmpresas();

                        /// salva as configuracoes gerais
                        GravarConfigGeral(true);

                        cStat = "1";
                        xMotivo = "Configuração do " + Propriedade.NomeAplicacao + " alterada com sucesso";
                        lErro = false;
                    }
                    else
                    {
                        cStat = "2";
                        xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + Propriedade.NomeAplicacao + ": Nenhuma tag padrão de configuração foi localizada no XML";
                        lErro = true;
                    }
                }
            }
            catch(Exception ex)
            {
                cStat = "2";
                xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + Propriedade.NomeAplicacao + ": " + ex.Message;
                lErro = true;
            }

            try
            {
                //Gravar o XML de retorno com a informação do sucesso ou não na reconfiguração
                var arqInfo = new FileInfo(cArquivoXml);
                var pastaRetorno = string.Empty;
                if(arqInfo.DirectoryName.ToLower().Trim() == Propriedade.PastaGeralTemporaria.ToLower().Trim())
                {
                    pastaRetorno = Propriedade.PastaGeralRetorno;
                }
                else
                {
                    pastaRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno;
                    ///
                    /// se der erro na atualizacao e se for solicitada a alteracao da pasta de retorno,
                    /// verificamos se ainda assim ela existe
                    ///
                    /// Nao existindo, gravamos o retorno na pasta de retorno do UniNFe
                    ///
                    if(!Directory.Exists(pastaRetorno) && lErro)
                    {
                        pastaRetorno = Propriedade.PastaGeralRetorno;
                    }
                }

                string nomeArqRetorno;
                var EXT = Propriedade.Extensao(Propriedade.TipoEnvio.AltCon);
                if(Path.GetExtension(cArquivoXml).ToLower() == ".txt")
                {
                    nomeArqRetorno = Functions.ExtrairNomeArq(cArquivoXml, EXT.EnvioTXT) + EXT.RetornoTXT;
                }
                else
                {
                    nomeArqRetorno = Functions.ExtrairNomeArq(cArquivoXml, EXT.EnvioXML) + EXT.RetornoXML;
                }

                var cArqRetorno = pastaRetorno + "\\" + nomeArqRetorno;

                try
                {
                    var oArqRetorno = new FileInfo(cArqRetorno);
                    if(oArqRetorno.Exists == true)
                    {
                        oArqRetorno.Delete();
                    }

                    if(Path.GetExtension(cArquivoXml).ToLower() == ".txt")
                    {
                        File.WriteAllText(cArqRetorno, "cStat|" + cStat + "\r\nxMotivo|" + xMotivo + "\r\n");
                    }
                    else
                    {
                        var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                                new XElement("retAltConfUniNFe",
                                                    new XElement(NFe.Components.TpcnResources.cStat.ToString(), cStat),
                                                    new XElement(NFe.Components.TpcnResources.xMotivo.ToString(), xMotivo),
                                                    new XElement("CertificadoDigitalThumbPrint", Empresas.Configuracoes[emp].CertificadoDigitalThumbPrint)));
                        xml.Save(cArqRetorno);
                    }
                }
                catch(Exception ex)
                {
                    //Ocorreu erro na hora de gerar o arquivo de erro para o ERP
                    ///
                    /// danasa 8-2009
                    ///
                    var oAux = new Auxiliar();
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(cArqRetorno) + ".err", xMotivo + Environment.NewLine + ex.Message);
                }

                try
                {
                    //Deletar o arquivo de configurações automáticas gerado pelo ERP
                    Functions.DeletarArquivo(cArquivoXml);
                }
                catch
                {
                    //Não vou fazer nada, so trato a exceção para evitar fechar o aplicativo. Wandrey 09/03/2010
                }
            }
            finally
            {
                //Se deu algum erro tenho que voltar as configurações como eram antes, ou seja
                //exatamente como estavam gravadas no XML de configuração
                if(lErro)
                {
                    ConfiguracaoApp.CarregarDados();
                    ConfiguracaoApp.CarregarDadosSobre();
                    Empresas.CarregaConfiguracao();

                    #region Ticket: #110

                    Empresas.CreateLockFile(true);

                    #endregion Ticket: #110
                }
            }
        }

        #endregion ReconfigurarUniNFe()

        #region ExcluirEmpresa

        /// <summary>
        /// Excluir a empresa e suas configurações
        /// </summary>
        /// <param name="arquivoXml">Nome e pasta do arquivo de configurações gerado pelo ERP para atualização das configurações do uninfe</param>
        /// <returns>
        /// S - Excluido com sucesso.
        /// E - Erro durante a exclusão.
        /// N - Não teve solicitação de exclusão no XML.
        /// </returns>
        private bool ExcluirEmpresa(string arquivoXml)
        {
            var retorna = false;

            try
            {
                var doc2 = new XmlDocument();
                doc2.Load(arquivoXml);

                var altConfUniNFeList = doc2.GetElementsByTagName("altConfUniNFe");
                if(altConfUniNFeList.Count > 0)
                {
                    var altConfUniNFeElement = (XmlElement)altConfUniNFeList[0];

                    if(altConfUniNFeElement.GetElementsByTagName("DadosEmpresa").Count > 0)
                    {
                        var dadosEmpresaElement = (XmlElement)altConfUniNFeElement.GetElementsByTagName("DadosEmpresa")[0];
                        if(dadosEmpresaElement.GetElementsByTagName("ExcluirEmpresa").Count > 0)
                        {
                            var excluir = Convert.ToBoolean(dadosEmpresaElement.GetElementsByTagName("ExcluirEmpresa")[0].InnerText);
                            if(excluir)
                            {
                                try
                                {
                                    var cnpj = dadosEmpresaElement.GetAttribute("CNPJ");
                                    var servico = dadosEmpresaElement.GetAttribute("Servico");
                                    var _empresa = Empresas.FindConfEmpresa(cnpj, EnumHelper.StringToEnum<TipoAplicativo>(servico));
                                    if(_empresa != null)
                                    {
                                        Empresas.Configuracoes.Remove(_empresa);
                                        new ConfiguracaoApp().GravarArqEmpresas();

                                        retorna = true;
                                    }
                                    else
                                    {
                                        throw new Exception("Empresa não localizada (" + cnpj + "). Exclusão não foi efetuada.");
                                    }
                                }
                                catch
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return retorna;
        }

        #endregion ExcluirEmpresa

        #region RemoveEndSlash

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveEndSlash(string value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                value = new DirectoryInfo(value).FullName;
                value = value.TrimEnd('\\');
            }
            else
            {
                value = string.Empty;
            }

            return value.Replace("\r\n", "").Trim();
        }

        #endregion RemoveEndSlash

        #region CadastrarEmpresa()

        private int CadastrarEmpresa(string arqXML, int emp)
        {
            var cnpj = "";
            var nomeEmp = "";
            var servico = "";
            var temEmpresa = false;

            if(Path.GetExtension(arqXML).ToLower() == ".xml")
            {
                var doc = new XmlDocument();
                doc.Load(arqXML);

                var dadosEmpresa = (XmlElement)doc.GetElementsByTagName("DadosEmpresa")[0];

                if(dadosEmpresa != null)
                {
                    #region Nome da empresa

                    if(dadosEmpresa.GetElementsByTagName("Nome")[0] != null)
                    {
                        nomeEmp = dadosEmpresa.GetElementsByTagName("Nome")[0].InnerText;
                        temEmpresa = true;
                    }
                    else if(dadosEmpresa.GetElementsByTagName("nome")[0] != null)
                    {
                        nomeEmp = dadosEmpresa.GetElementsByTagName("nome")[0].InnerText;
                        temEmpresa = true;
                    }

                    #endregion Nome da empresa

                    #region CNPJ

                    if(!string.IsNullOrEmpty(dadosEmpresa.GetAttribute(NFe.Components.TpcnResources.CNPJ.ToString())))
                    {
                        cnpj = dadosEmpresa.GetAttribute(NFe.Components.TpcnResources.CNPJ.ToString());
                        temEmpresa = true;
                    }
                    else if(!string.IsNullOrEmpty(dadosEmpresa.GetAttribute("cnpj")))
                    {
                        cnpj = dadosEmpresa.GetAttribute("cnpj");
                        temEmpresa = true;
                    }
                    else if(!string.IsNullOrEmpty(dadosEmpresa.GetAttribute("Cnpj")))
                    {
                        cnpj = dadosEmpresa.GetAttribute("Cnpj");
                        temEmpresa = true;
                    }

                    #endregion CNPJ

                    #region Servico

                    if(!string.IsNullOrEmpty(dadosEmpresa.GetAttribute("Servico")))
                    {
                        servico = dadosEmpresa.GetAttribute("Servico");
                        temEmpresa = true;
                    }
                    else if(!string.IsNullOrEmpty(dadosEmpresa.GetAttribute("servico")))
                    {
                        servico = dadosEmpresa.GetAttribute("servico");
                        temEmpresa = true;
                    }

                    #endregion Servico
                }
            }
            else
            {
                var cLinhas = Functions.LerArquivo(arqXML);

                foreach(var texto in cLinhas)
                {
                    var dados = texto.Split('|');
                    var nElementos = dados.GetLength(0);
                    if(nElementos <= 1)
                    {
                        continue;
                    }

                    switch(dados[0].ToLower())
                    {
                        case "nome":
                            nomeEmp = dados[1].Trim();
                            temEmpresa = true;
                            break;

                        case "cnpj":
                            cnpj = dados[1].Trim();
                            temEmpresa = true;
                            break;

                        case "servico":
                            servico = dados[1].Trim();
                            temEmpresa = true;
                            break;
                    }
                }
            }

            if(temEmpresa)
            {
                if(string.IsNullOrEmpty(cnpj) || string.IsNullOrEmpty(nomeEmp) || string.IsNullOrEmpty(servico))
                {
                    throw new Exception("Não foi possível localizar os dados da empresa no arquivo de configuração. (CNPJ/Nome ou Tipo de Serviço)");
                }

                if(char.IsLetter(servico, 0))
                {
                    var lista = NFe.Components.EnumHelper.ToStrings(typeof(TipoAplicativo));
                    if(!lista.Contains(servico))
                    {
                        throw new Exception(string.Format("Serviço deve ser ({0}, {1}, {2}, {3}, {4} ou {5})",
                            NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Nfe),
                            NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Cte),
                            NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Nfse),
                            NFe.Components.EnumHelper.GetDescription(TipoAplicativo.MDFe),
                            NFe.Components.EnumHelper.GetDescription(TipoAplicativo.NFCe),
                            NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Todos)));
                    }

                    ///
                    /// veio como 'NFe, NFCe, CTe, MDFe ou NFSe
                    /// converte para numero correspondente
                    servico = ((int)NFe.Components.EnumHelper.StringToEnum<TipoAplicativo>(servico)).ToString();
                }
                else
                {
                    if(!("0,1,2,3,4,10").Contains(servico))
                    {
                        throw new Exception(string.Format("Serviço deve ser ({0} p/{1}, {2} p/{3}, {4} p/{5}, {6} p/{7}, {8} p/{9} ou {10} p/{11})",
                            (int)TipoAplicativo.Nfe, NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Nfe),
                            (int)TipoAplicativo.Cte, NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Cte),
                            (int)TipoAplicativo.Nfse, NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Nfse),
                            (int)TipoAplicativo.MDFe, NFe.Components.EnumHelper.GetDescription(TipoAplicativo.MDFe),
                            (int)TipoAplicativo.NFCe, NFe.Components.EnumHelper.GetDescription(TipoAplicativo.NFCe),
                            (int)TipoAplicativo.Todos, NFe.Components.EnumHelper.GetDescription(TipoAplicativo.Todos)));
                    }
                }
                if(Empresas.FindConfEmpresa(cnpj.Trim(), (TipoAplicativo)Convert.ToInt16(servico)) == null)
                {
                    var empresa = new Empresa
                    {
                        CNPJ = cnpj,
                        Nome = nomeEmp,
                        Servico = (TipoAplicativo)Convert.ToInt16(servico)
                    };
                    Empresas.Configuracoes.Add(empresa);

                    //GravarArqEmpresas();  //tirado daqui pq ele somente grava quando a empresa for gravada com sucesso
                }

                return Empresas.FindConfEmpresaIndex(cnpj, (TipoAplicativo)Convert.ToInt16(servico));
            }
            else
            {
                return emp;
            }
        }

        #endregion CadastrarEmpresa()

        #region CertificadosInstalados()

        public void CertificadosInstalados(string arquivo)
        {
            var lConsultar = false;
            var lErro = false;
            var arqRet = Functions.ExtrairNomeArq(arquivo, Propriedade.Extensao(Propriedade.TipoEnvio.ConsCertificado).EnvioXML) +
                                  Propriedade.Extensao(Propriedade.TipoEnvio.ConsCertificado).RetornoXML;
            var tmp_arqRet = Path.Combine(Propriedade.PastaGeralTemporaria, arqRet);
            var cStat = "";
            var xMotivo = "";

            try
            {
                var doc = new XmlDocument();
                doc.Load(arquivo);

                foreach(XmlElement item in doc.DocumentElement)
                {
                    lConsultar = doc.DocumentElement.GetElementsByTagName("xServ")[0].InnerText.Equals("CONS-CERTIFICADO", StringComparison.InvariantCultureIgnoreCase);
                }

                if(lConsultar)
                {
                    var store = new X509Store("MY", StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var collection = store.Certificates;
                    var collection1 = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                    var collection2 = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

                    #region Cria XML de retorno

                    if(File.Exists(tmp_arqRet))
                    {
                        File.Delete(tmp_arqRet);
                    }

                    var RetCertificados = new XmlDocument();

                    XmlNode raiz = RetCertificados.CreateElement("Certificados");
                    RetCertificados.AppendChild(raiz);

                    RetCertificados.Save(tmp_arqRet);

                    #endregion Cria XML de retorno

                    #region Monta XML de Retorno com dados do Certificados Instalados

                    for(var i = 0; i < collection2.Count; i++)
                    {
                        #region layout retorno

                        /*layout de retorno - Renan Borges
                        <Certificados>
                        <ThumbPrint ID="999...">
                        <Subject>XX...</Subject>
                        <ValidadeInicial>dd/dd/dddd</ValidadeInicial>
                        <ValidadeFinal>dd/dd/dddd</ValidadeFinal>
                        <A3>true</A3>
                        </Certificados>
                        */

                        #endregion layout retorno

                        var _X509Cert = collection2[i];

                        var docGerar = new XmlDocument();
                        docGerar.Load(tmp_arqRet);

                        XmlNode Registro = docGerar.CreateElement("ThumbPrint");
                        var IdThumbPrint = docGerar.CreateAttribute(NFe.Components.TpcnResources.ID.ToString());
                        IdThumbPrint.Value = _X509Cert.Thumbprint.ToString();
                        Registro.Attributes.Append(IdThumbPrint);

                        XmlNode Subject = docGerar.CreateElement("Subject");
                        XmlNode ValidadeInicial = docGerar.CreateElement("ValidadeInicial");
                        XmlNode ValidadeFinal = docGerar.CreateElement("ValidadeFinal");
                        XmlNode A3 = docGerar.CreateElement("A3");
                        XmlNode SerialNumber = docGerar.CreateElement("SerialNumber");

                        Subject.InnerText = _X509Cert.Subject.ToString();
                        ValidadeInicial.InnerText = _X509Cert.NotBefore.ToShortDateString();
                        ValidadeFinal.InnerText = _X509Cert.NotAfter.ToShortDateString();
                        A3.InnerText = _X509Cert.IsA3().ToString().ToLower();
                        SerialNumber.InnerText = _X509Cert.SerialNumber;

                        docGerar.SelectSingleNode("Certificados").AppendChild(Registro);
                        Registro.AppendChild(Subject);
                        Registro.AppendChild(ValidadeInicial);
                        Registro.AppendChild(ValidadeFinal);
                        Registro.AppendChild(A3);
                        Registro.AppendChild(SerialNumber);

                        docGerar.Save(tmp_arqRet);
                    }

                    #endregion Monta XML de Retorno com dados do Certificados Instalados
                }
            }
            catch
            {
                cStat = "2";
                xMotivo = "Nao foi possivel fazer a consulta de Certificados Instalados na estação " + Propriedade.NomeAplicacao;
                lErro = true;
                File.Delete(tmp_arqRet);
            }
            finally
            {
                var cArqRetorno = Propriedade.PastaGeralRetorno + "\\" + arqRet;

                #region XML de Retorno para ERP

                try
                {
                    var oArqRetorno = new FileInfo(cArqRetorno);
                    if(oArqRetorno.Exists == true)
                    {
                        oArqRetorno.Delete();
                    }

                    if(!lConsultar && !lErro)
                    {
                        cStat = "2";
                        xMotivo = "Nao foi possivel fazer a consulta de Certificados Instalados na estação (xServ não identificado) no " + Propriedade.NomeAplicacao;
                    }

                    if(lErro || !lConsultar)
                    {
                        File.Delete(tmp_arqRet);

                        var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                                new XElement("retCadConfUniNFe",
                                                    new XElement(NFe.Components.TpcnResources.cStat.ToString(), cStat),
                                                    new XElement(NFe.Components.TpcnResources.xMotivo.ToString(), xMotivo)));
                        xml.Save(cArqRetorno);
                    }
                    else
                    {
                        if(File.Exists(cArqRetorno))
                        {
                            File.Delete(cArqRetorno);
                        }

                        if(File.Exists(arquivo))
                        {
                            File.Delete(arquivo);
                        }

                        File.Move(tmp_arqRet, Propriedade.PastaGeralRetorno + "\\" + arqRet);
                    }
                }
                catch(Exception ex)
                {
                    //Ocorreu erro na hora de gerar o arquivo de erro para o ERP
                    var oAux = new Auxiliar();
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(cArqRetorno) + ".err", xMotivo + Environment.NewLine + ex.Message);
                }

                #endregion XML de Retorno para ERP
            }
        }

        #endregion CertificadosInstalados()

        #region ForceUpdateWSDL()

        public static bool ForceUpdateWSDL(bool pergunta, ref string cerros)
        {
            if(!pergunta)
            {
                var c = new loadResources();
                c.load();
                cerros = c.cErros;
                return true;
            }

            var msg = "Após confirmada esta função o UniNFe irá sobrepor todos os WSDLs e Schemas com as versões originais da Versão do UniNFe, sobrepondo assim possíveis arquivos que tenham sido atualizados manualmente.\r\n\r\nTem certeza que deseja continuar? ";

            if(MessageBox.Show(msg, "ATENÇÂO! - Atualização dos WSDLs e SCHEMAS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Functions.DeletarArquivo(Propriedade.XMLVersaoWSDLXSD);

                new loadResources().load();

                MessageBox.Show("WSDLs e Schemas atualizados com sucesso.", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }

        #endregion ForceUpdateWSDL()

        #endregion Métodos gerais
    }

    #endregion Classe ConfiguracaoApp
}
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using NFe.Components;
using NFSe.Components;

namespace NFe.Settings
{
    #region Classe ConfiguracaoApp
    /// <summary>
    /// Classe responsável por realizar algumas tarefas na parte de configurações da aplicação.
    /// Arquivo de configurações: UniNfeConfig.xml
    /// </summary>
    public class ConfiguracaoApp
    {
        internal class ArquivoItem
        {
            public string Arquivo;
            public DateTime Data;
            public bool Manual;
        }

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
            GravarLogOperacaoRealizada
        }
        #endregion

        #region Propriedades

        #region ChecarConexaoInternet
        public static bool ChecarConexaoInternet { get; set; }
        #endregion

        #region GravarLogOperacoesRealizadas
        public static bool GravarLogOperacoesRealizadas { get; set; }
        #endregion

        #region Propriedades das versões dos XML´s

        #region NFe
        //public static string VersaoXMLStatusServico { get; set; }
        //public static string VersaoXMLNFe { get; set; }
        //public static string VersaoXMLPedRec { get; set; }
        //public static string VersaoXMLCanc { get; set; }
        //public static string VersaoXMLInut { get; set; }
        //public static string VersaoXMLPedSit { get; set; }
        //public static string VersaoXMLConsCad { get; set; }
        //public static string VersaoXMLCabecMsg { get; set; }
        //public static string VersaoXMLEnvDPEC { get; set; }
        //public static string VersaoXMLConsDPEC { get; set; }
        //public static string VersaoXMLEnvDownload { get; set; }
        //public static string VersaoXMLEnvConsultaNFeDest { get; set; }
        //public static string VersaoXMLEvento { get; set; }
        #endregion

        #region MDFe
        //public static string VersaoXMLMDFeCabecMsg { get; set; }
        //public static string VersaoXMLMDFeStatusServico { get; set; }
        //public static string VersaoXMLMDFe { get; set; }
        //public static string VersaoXMLMDFePedRec { get; set; }
        //public static string VersaoXMLMDFePedSit { get; set; }
        //public static string VersaoXMLMDFeEvento { get; set; }
        #endregion

        #region CTe
        //public static string VersaoXMLCTeCabecMsg { get; set; }
        //public static string VersaoXMLCTeStatusServico { get; set; }
        //public static string VersaoXMLCTe { get; set; }
        //public static string VersaoXMLCTePedRec { get; set; }
        //public static string VersaoXMLCTePedSit { get; set; }
        //public static string VersaoXMLCTeInut { get; set; }
        //public static string VersaoXMLCTeEvento { get; set; }
        #endregion

        #endregion

        #region Propriedades para controle de servidor proxy
        public static bool Proxy { get; set; }
        public static string ProxyServidor { get; set; }
        public static string ProxyUsuario { get; set; }
        public static string ProxySenha { get; set; }
        public static int ProxyPorta { get; set; }
        #endregion

        #region Propriedades para tela de sobre
        public static string NomeEmpresa { get; set; }
        public static string Site { get; set; }
        public static string SiteProduto { get; set; }
        public static string Email { get; set; }
        #endregion

        #region SenhaConfig
        private static bool mSenhaConfigAlterada = false;
        private static string mSenhaConfig;
        public static string SenhaConfig
        {
            get
            {
                return mSenhaConfig;
            }
            set
            {
                if (value != mSenhaConfig)
                {
                    mSenhaConfigAlterada = true;
                    mSenhaConfig = value;
                }
                else
                    mSenhaConfigAlterada = false;
            }
        }
        #endregion

        #region Prorpiedades utilizadas no inicio do sistema
        public static bool AtualizaWSDL { get; set; }
        #endregion

        #endregion

        #region Métodos gerais

        #region Extrae os arquivos necessarios a executacao
        internal class loadResources
        {
            private static string XMLVersoesWSDL = Propriedade.PastaExecutavel + "\\VersoesWSDLs.xml";

            #region load()
            /// <summary>
            /// Exporta os WSDLs e Schemas da DLL para as pastas do UniNFe
            /// </summary>
            public void load()
            {
                List<ArquivoItem> ListArqsAtualizar = new List<ArquivoItem>();
                UpdateWSDL(ListArqsAtualizar);
                bool gravaLista = false;

                try
                {
                    System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom(Propriedade.PastaExecutavel + "\\NFe.Components.Wsdl.dll");
                    string[] x = ass.GetManifestResourceNames();
                    if (x.GetLength(0) > 0)
                    {
                        var afiles = (from d in x
                                      where NFe.Components.Propriedade.TipoAplicativo == TipoAplicativo.Nfse ? d.StartsWith("NFe.Components.Wsdl.NFse.") : d.StartsWith("NFe.Components.Wsdl.NFe.")
                                      select d);

                        foreach (string s in afiles)
                        {
                            string fileoutput = null;
                            switch (NFe.Components.Propriedade.TipoAplicativo)
                            {
                                case TipoAplicativo.Nfe:
                                    fileoutput = s.Replace("NFe.Components.Wsdl.NFe.", NFe.Components.Propriedade.PastaExecutavel + "\\");
                                    break;
                                case TipoAplicativo.Nfse:
                                    fileoutput = s.Replace("NFe.Components.Wsdl.NFse.", NFe.Components.Propriedade.PastaExecutavel + "\\");
                                    break;
                            }

                            if (fileoutput == null)
                                continue;

                            if (fileoutput.ToLower().EndsWith(".xsd"))
                            {
                                /// Ex: NFe.Components.Wsdl.NFe.NFe.xmldsig-core-schema_v1.01.xsd
                                ///
                                /// pesquisa pelo nome do XSD
                                int plast = fileoutput.ToLower().LastIndexOf("_v");
                                if (plast == -1)
                                    plast = fileoutput.IndexOf(".xsd") - 1;

                                while (fileoutput[plast] != '.' && plast >= 0)
                                    --plast;

                                string fn = fileoutput.Substring(plast + 1);
                                fileoutput = fileoutput.Substring(0, plast).Replace(".", "\\") + "\\" + fn;
                            }
                            else
                                fileoutput = (fileoutput.Substring(0, fileoutput.LastIndexOf('.')) + "#" + fileoutput.Substring(fileoutput.LastIndexOf('.') + 1)).Replace(".", "\\").Replace("#", ".");


                            FileInfo fi = new FileInfo(fileoutput);
                            ArquivoItem item = new ArquivoItem();
                            item = null;

                            if (fi.Exists)  //danasa 9-2013
                            {
                                if (ListArqsAtualizar.Count > 0)
                                {
                                    item = ListArqsAtualizar.FirstOrDefault(f => f.Arquivo == fi.Name);
                                }
                            }
                            // A comparação é feita (fi.LastWriteTime != item.Data)
                            // Pois intende-se que se a data do arquivo que esta na pasta do UniNFe for superior a data
                            // de quando foi feita a ultima atualizacao do UniNfe, significa que ele foi atualizado manualmente e não devemos
                            // sobrepor o WSDL ou SCHEMA do Usuario - Renan 26/03/2013
                            if (item == null || (item != null && fi.LastWriteTime.ToString("dd/MM/yyyy") != item.Data.ToString("dd/MM/yyyy")))
                            {
                                if (item == null || (item != null && !item.Manual))
                                {
                                    using (StreamReader FileReader = new StreamReader(ass.GetManifestResourceStream(s)))
                                    {
                                        if (!Directory.Exists(Path.GetDirectoryName(fileoutput)))
                                            Directory.CreateDirectory(Path.GetDirectoryName(fileoutput));

                                        using (StreamWriter FileWriter = new StreamWriter(fileoutput))
                                        {
                                            FileWriter.Write(FileReader.ReadToEnd());
                                            FileWriter.Close();
                                            gravaLista = true;
                                        }
                                    }
                                }
                            }
                            else if (item != null)
                            {
                                item.Manual = true;
                            }
                        }
                    }
                    if (gravaLista)
                        GravarVersoesWSDLs(ListArqsAtualizar);
                }
                catch (Exception ex)
                {
                    int emp = Empresas.FindEmpresaByThread();
                    string xMotivo = "Não foi possível atualizar pacotes de Schemas/WSDLs.";

                    Auxiliar.WriteLog(ex.Message);
                    Auxiliar oAux = new Auxiliar();
                    oAux.GravarArqErroERP(Empresas.Configuracoes[emp].CNPJ + ".err", xMotivo + Environment.NewLine + ex.Message);
                }
                finally
                {
                    if (NFe.Components.Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                        WebServiceNFSe.Start();
                }
            }

            #endregion

            #region UpdateWSDL()
            /// <summary>
            /// Metodo responsavel em definir se as WSDLS e Schemas serão atualizados e quais serão atualizados pois o usuario pode ter atualizado manualmente.
            /// <author>
            /// Renan Borges - 25/06/2013 
            /// </author>
            /// </summary>
            /// <param name="ListArquivosVerificar">Lista a ser montada para ser comparada no momento da Atualizacao dos WSDLs</param>
            public void UpdateWSDL(List<ArquivoItem> ListArquivosVerificar)
            {
                if (File.Exists(XMLVersoesWSDL))
                {
                    LerXmlWSDLs(ListArquivosVerificar);
                }
                else
                {
                    //Não faz nada só vai atualizar e no final gravar o XML
                }
            }
            #endregion

            #region GravarVersoesWSDLs()
            /// <summary>
            /// Metodo responsavel em Ler os Arquivos disponiveis nas pastas e Gravas as Informações no XML que contem as informacoes sobre o mesmo
            /// </summary>
            /// <author>
            /// Renan Borges - 25/06/2013 
            /// </author>
            /// <param name="ListaAnterior"></param>
            public void GravarVersoesWSDLs(List<ArquivoItem> ListaAnterior)
            {
                string pastaExecutavel = Propriedade.PastaExecutavel;
                string pastaWSDLProducao = pastaExecutavel + "\\WSDL\\Producao\\";
                string pastaWSDLHomologacao = pastaExecutavel + "\\WSDL\\Homologacao\\";
                string pastaXSD = pastaExecutavel + "\\schemas\\";

                string[] ArquivosWSDLProducao = Directory.GetFiles(pastaWSDLProducao, "*.wsdl", SearchOption.AllDirectories);
                string[] ArquivosWSDLHomologacao = Directory.GetFiles(pastaWSDLHomologacao, "*.wsdl", SearchOption.AllDirectories);
                string[] ArquivosXSD = Directory.GetFiles(pastaXSD, "*.xsd", SearchOption.AllDirectories);

                List<ArquivoItem> ArquivosXML = new List<ArquivoItem>();

                PreparaDadosWSDLs(ArquivosWSDLProducao, ArquivosXML);
                PreparaDadosWSDLs(ArquivosWSDLHomologacao, ArquivosXML);
                PreparaDadosWSDLs(ArquivosXSD, ArquivosXML);

                foreach (ArquivoItem item in ArquivosXML)
                {
                    ArquivoItem itemAntigo = ListaAnterior.FirstOrDefault(a => a.Arquivo == item.Arquivo);
                    item.Manual = itemAntigo == null ? false : itemAntigo.Manual;
                }

                if (File.Exists(XMLVersoesWSDL))
                {
                    File.Delete(XMLVersoesWSDL);
                }

                EscreverXmlWSDLs(ArquivosXML);

            }
            #endregion

            #region PreparaDadosWSDLs()
            /// <summary>
            /// Metodo responsavel em pegar o Array contendo o caminho dos arquivos encontrado nos diretorios e Montar uma 
            /// Lista Contendo os Dados dos Arquivos que serao gravados no VersoesWSDLs.xml
            /// </summary>
            /// <author>
            /// Renan Borges - 25/06/2013 
            /// </author>
            /// <param name="arquivos">Array com o Caminho dos Arquivos encontrados nos diretorios</param>
            /// <param name="ListArquivos">A Lista onde as informações serão armazenadas para serem gravadas posteriormente</param>
            private void PreparaDadosWSDLs(string[] arquivos, List<ArquivoItem> ListArquivos)
            {

                foreach (string arquivo in arquivos)
                {
                    FileInfo infArquivo = new FileInfo(arquivo);

                    DateTime dataModif = infArquivo.LastWriteTime;
                    string nomearquivo = infArquivo.Name;

                    ListArquivos.Add(new ArquivoItem
                    {
                        Arquivo = nomearquivo,
                        Data = dataModif,
                        Manual = false

                    });
                }


            }
            #endregion

            #region EscreverXmlWSDLs()
            /// <summary>
            /// Metodo responsavel em escrever o XML VersoesWSDLs.xml na pasta do executavel
            /// </summary>
            /// <author>
            /// Renan Borges - 25/06/2013
            /// </author>
            /// <param name="ListArquivosGerar">Lista com os informacoes a serem gravados pertinentes aos arquivos</param>
            private void EscreverXmlWSDLs(List<ArquivoItem> ListArquivosGerar)
            {

                if (ListArquivosGerar.Count > 0)
                {

                    XmlTextWriter arqXML = new XmlTextWriter(XMLVersoesWSDL, Encoding.UTF8);
                    arqXML.WriteStartDocument();

                    arqXML.Formatting = Formatting.Indented;
                    arqXML.WriteStartElement("arquivos");

                    foreach (ArquivoItem item in ListArquivosGerar)
                    {
                        arqXML.WriteStartElement("wsdl");

                        arqXML.WriteElementString("arquivo", item.Arquivo);
                        arqXML.WriteElementString("data", item.Data.ToShortDateString());
                        arqXML.WriteElementString("manual", item.Manual.ToString());

                        arqXML.WriteEndElement();
                    }

                    arqXML.WriteFullEndElement();

                    arqXML.Close();
                }

            }
            #endregion

            #region LerXmlWSDLs()
            /// <summary>
            /// Metodo responsavel em ler as informacoes dos XML WSDLsVersoes e preparar a lista dos arquivos
            /// </summary>
            /// <author>
            /// Renan Borges - 25/06/2013
            /// </author>
            /// <param name="ListArqInstalados">Lista onde vai ser adcionada os arquivos do encontrados no XML</param>
            private void LerXmlWSDLs(List<ArquivoItem> ListArqInstalados)
            {
                XmlDocument oXmlWSDLs = new XmlDocument();
                oXmlWSDLs.Load(XMLVersoesWSDL);
                XmlNodeList xnListXml = oXmlWSDLs.GetElementsByTagName("wsdl");

                foreach (XmlNode item in xnListXml)
                {
                    string _arquivo = item["arquivo"].InnerText;
                    string _data = item["data"].InnerText;
                    string _manual = item["manual"].InnerText;

                    ListArqInstalados.Add(new ArquivoItem
                    {
                        Arquivo = _arquivo,
                        Data = Convert.ToDateTime(_data),
                        Manual = Convert.ToBoolean(_manual)
                    });
                }
            }
            #endregion

        }

        public static void loadResouces()
        {
            new loadResources().load();
        }
        #endregion

        #region StartVersoes
        public static void StartVersoes()
        {
            //if (ConfiguracaoApp.AtualizaWSDL)
            new loadResources().load();

            ConfiguracaoApp.CarregarDados();

            if (!Propriedade.ServicoRodando || Propriedade.ExecutandoPeloUniNFe)
                ConfiguracaoApp.CarregarDadosSobre();

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Nfe:
                    #region NF-e
                    //ConfiguracaoApp.VersaoXMLCanc = "2.00";
                    //ConfiguracaoApp.VersaoXMLConsCad = "2.00";
                    //ConfiguracaoApp.VersaoXMLInut = "2.00";
                    //ConfiguracaoApp.VersaoXMLNFe = "2.00";
                    //ConfiguracaoApp.VersaoXMLPedRec = "2.00";
                    //ConfiguracaoApp.VersaoXMLPedSit = "2.01";
                    //ConfiguracaoApp.VersaoXMLStatusServico = "2.00";
                    //ConfiguracaoApp.VersaoXMLCabecMsg = "2.00";
                    //ConfiguracaoApp.VersaoXMLEnvDPEC = "1.01";
                    //ConfiguracaoApp.VersaoXMLConsDPEC = "1.01";
                    //ConfiguracaoApp.VersaoXMLEvento = "1.00";
                    //ConfiguracaoApp.VersaoXMLEnvDownload = "1.00";
                    //ConfiguracaoApp.VersaoXMLEnvConsultaNFeDest = "1.01";
                    #endregion

                    #region MDF-e
                    //ConfiguracaoApp.VersaoXMLMDFeCabecMsg = "1.00";
                    //ConfiguracaoApp.VersaoXMLMDFeStatusServico = "1.00";
                    //ConfiguracaoApp.VersaoXMLMDFe = "1.00";
                    //ConfiguracaoApp.VersaoXMLMDFePedRec = "1.00";
                    //ConfiguracaoApp.VersaoXMLMDFePedSit = "1.00";
                    //ConfiguracaoApp.VersaoXMLMDFeEvento = "1.00";
                    #endregion

                    #region CT-e
                    //ConfiguracaoApp.VersaoXMLCTeCabecMsg = "2.00";
                    //ConfiguracaoApp.VersaoXMLCTeStatusServico = "2.00";
                    //ConfiguracaoApp.VersaoXMLCTe = "2.00";
                    //ConfiguracaoApp.VersaoXMLCTePedRec = "2.00";
                    //ConfiguracaoApp.VersaoXMLCTePedSit = "2.00";
                    //ConfiguracaoApp.VersaoXMLCTeInut = "2.00";
                    //ConfiguracaoApp.VersaoXMLCTeEvento = "2.00";
                    #endregion

                    Propriedade.nsURI = "http://www.portalfiscal.inf.br/nfe";
                    SchemaXML.CriarListaIDXML();
                    break;

                case TipoAplicativo.Nfse:
                    //ConfiguracaoApp.VersaoXMLCanc = "1.03";
                    //ConfiguracaoApp.VersaoXMLConsCad = "1.03";
                    //ConfiguracaoApp.VersaoXMLInut = "1.03";
                    //ConfiguracaoApp.VersaoXMLNFe = "1.03";
                    //ConfiguracaoApp.VersaoXMLPedRec = "1.03";
                    //ConfiguracaoApp.VersaoXMLPedSit = "1.03";
                    //ConfiguracaoApp.VersaoXMLStatusServico = "1.03";
                    //ConfiguracaoApp.VersaoXMLCabecMsg = "1.03";
                    Propriedade.nsURI = "http://www.abrasf.org.br/";
                    SchemaXMLNFSe.CriarListaIDXML();
                    break;
            }
        }
        #endregion

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
            string vArquivoConfig = Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig;
            XmlDocument doc = null;
            if (File.Exists(vArquivoConfig))
            {
                try
                {
                    doc = new XmlDocument();
                    doc.Load(vArquivoConfig);

                    XmlNodeList configList = null;

                    configList = doc.GetElementsByTagName(NFeStrConstants.nfe_configuracoes);

                    foreach (XmlNode nodeConfig in configList)
                    {
                        XmlElement elementConfig = (XmlElement)nodeConfig;

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.Proxy.ToString())[0] != null)
                            ConfiguracaoApp.Proxy = Convert.ToBoolean(elementConfig[NfeConfiguracoes.Proxy.ToString()].InnerText);

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxyServidor.ToString())[0] != null)
                            ConfiguracaoApp.ProxyServidor = elementConfig[NfeConfiguracoes.ProxyServidor.ToString()].InnerText.Trim();

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxyUsuario.ToString())[0] != null)
                            ConfiguracaoApp.ProxyUsuario = elementConfig[NfeConfiguracoes.ProxyUsuario.ToString()].InnerText.Trim();

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxySenha.ToString())[0] != null)
                            ConfiguracaoApp.ProxySenha = elementConfig[NfeConfiguracoes.ProxySenha.ToString()].InnerText.Trim();

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.ProxyPorta.ToString())[0] != null)
                            ConfiguracaoApp.ProxyPorta = Convert.ToInt32(elementConfig[NfeConfiguracoes.ProxyPorta.ToString()].InnerText.Trim());

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.SenhaConfig.ToString())[0] != null)
                            ConfiguracaoApp.SenhaConfig = elementConfig[NfeConfiguracoes.SenhaConfig.ToString()].InnerText.Trim();

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.ChecarConexaoInternet.ToString())[0] != null)
                            ConfiguracaoApp.ChecarConexaoInternet = Convert.ToBoolean(elementConfig[NfeConfiguracoes.ChecarConexaoInternet.ToString()].InnerText);
                        else
                            ConfiguracaoApp.ChecarConexaoInternet = true;

                        if (elementConfig.GetElementsByTagName(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString())[0] != null)
                            ConfiguracaoApp.GravarLogOperacoesRealizadas = Convert.ToBoolean(elementConfig[NfeConfiguracoes.GravarLogOperacaoRealizada.ToString()].InnerText);

                    }
                }
                catch (Exception ex)
                {
                    ///
                    /// danasa 8-2009
                    /// como reportar ao usuario que houve erro de leitura do arquivo de configuracao?
                    /// tem um usuário que postou um erro de leitura deste arquivo e não sabia como resolver.
                    /// 
                    ///
                    /// danasa 8-2009
                    /// 
                    if (!Propriedade.ServicoRodando || Propriedade.ExecutandoPeloUniNFe)
                        MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (doc != null)
                        doc = null;

                }
            }
            else
            {
                ChecarConexaoInternet = true;
            }
            //Carregar a lista de webservices disponíveis
            try
            {
                if (WebServiceProxy.CarregaWebServicesList())
                    ///
                    /// danasa 9-2013
                    /// força a atualizacao dos wsdl's pois pode ser que tenha sido criado um novo padrao
                    ConfiguracaoApp.AtualizaWSDL = true;
            }
            catch (Exception ex)
            {
                Auxiliar.WriteLog(ex.Message);
            }
        }
        #endregion

        #region CarregarDadosSobre()
        /// <summary>
        /// Carrega informações da tela de sobre
        /// </summary>
        /// <remarks>
        /// Autor: Leandro Souza
        /// </remarks>
        public static void CarregarDadosSobre()
        {
            string vArquivoConfig = Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfigSobre;

            //ConfiguracaoApp.NomeEmpresa = "Unimake Software";
            //ConfiguracaoApp.Site = "www.unimake.com.br";
            //ConfiguracaoApp.SiteProduto = ConfiguracaoApp.Site + "/uninfe";
            //ConfiguracaoApp.Email = "nfe@unimake.com.br";

            if (File.Exists(vArquivoConfig))
            {
                XmlTextReader oLerXml = null;
                try
                {
                    //Carregar os dados do arquivo XML de configurações da Aplicação
                    oLerXml = new XmlTextReader(vArquivoConfig);

                    while (oLerXml.Read())
                    {
                        if (oLerXml.NodeType == XmlNodeType.Element)
                        {
                            if (oLerXml.Name == NFeStrConstants.nfe_configuracoes)
                            {
                                while (oLerXml.Read())
                                {
                                    if (oLerXml.NodeType == XmlNodeType.Element)
                                    {
                                        if (oLerXml.Name == "NomeEmpresa") { oLerXml.Read(); ConfiguracaoApp.NomeEmpresa = oLerXml.Value; }
                                        else if (oLerXml.Name == "Site") { oLerXml.Read(); ConfiguracaoApp.Site = oLerXml.Value.Trim(); }
                                        else if (oLerXml.Name == "SiteProduto") { oLerXml.Read(); ConfiguracaoApp.SiteProduto = oLerXml.Value.Trim(); }
                                        else if (oLerXml.Name == "Email") { oLerXml.Read(); ConfiguracaoApp.Email = oLerXml.Value.Trim(); }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (oLerXml != null)
                        oLerXml.Close();
                }
            }
        }
        #endregion

        #region DefinirWS()
        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <param name="tpEmis">Tipo de emissão do XML</param>
        /// <param name="versao">Versão do XML</param>
        /// <returns>Retorna o objeto do serviço</returns>
        public static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int tpEmis, string versao)
        {
            return DefinirWS(servico, emp, cUF, tpAmb, tpEmis, PadroesNFSe.NaoIdentificado, versao);
        }
        #endregion

        #region DefinirWS()
        /// <summary>
        /// Definir o webservice que será utilizado para o envio do XML
        /// </summary>
        /// <param name="servico">Serviço que será executado</param>
        /// <param name="emp">Index da empresa que será executado o serviço</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="tpAmb">Código do ambiente que será acessado</param>
        /// <param name="tpEmis">Tipo de emissão do XML</param>
        /// <param name="versao">Versão do XML</param>
        /// <param name="padraoNFSe">Padrão da NFS-e</param>
        /// <param name="ehNFCe">Se é NFC-e (Nota fiscal de consumidor eletrônica)</param>
        /// <returns>Retorna o objeto do serviço</returns>
        public static WebServiceProxy DefinirWS(Servicos servico, int emp, int cUF, int tpAmb, int tpEmis, PadroesNFSe padraoNFSe, string versao)
        {
            WebServiceProxy wsProxy = null;
            string key = servico + " " + cUF + " " + tpAmb + " " + tpEmis + (!string.IsNullOrEmpty(versao) ? " " + versao : "");

            while (true)
            {
                lock (Smf.WebProxy)
                {
                    if (Empresas.Configuracoes[emp].WSProxy.ContainsKey(key))
                    {
                        wsProxy = Empresas.Configuracoes[emp].WSProxy[key];
                    }
                    else
                    {
                        //Definir se é uma configurações específica para NFC-e
                        bool ehNFCe = Empresas.Configuracoes[emp].Servico == TipoAplicativo.NFCe;

                        //Definir a URI para conexão com o Webservice
                        string Url = ConfiguracaoApp.DefLocalWSDL(cUF, tpAmb, tpEmis, versao, servico, ehNFCe);

                        wsProxy = new WebServiceProxy(Url, 
                                                      Empresas.Configuracoes[emp].X509Certificado, 
                                                      padraoNFSe,
                                                      (tpAmb == (int)NFe.Components.TipoAmbiente.taHomologacao),
                                                      //cUF,
                                                      //versao,
                                                      servico);

                        Empresas.Configuracoes[emp].WSProxy.Add(key, wsProxy);
                    }

                    break;
                }
            }

            return wsProxy;
        }
        #endregion

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
        private static string DefLocalWSDL(int CodigoUF, int tipoAmbiente, int tipoEmissao, string versao, Servicos servico, bool ehNFCe)
        {
            string WSDL = string.Empty;
            switch ((NFe.Components.TipoEmissao)tipoEmissao)
            {
                case NFe.Components.TipoEmissao.teSVCRS:
                    CodigoUF = 902;
                    break;

                case NFe.Components.TipoEmissao.teSVCSP:
                    CodigoUF = 903;
                    break;

                case NFe.Components.TipoEmissao.teSVCAN:
                    CodigoUF = 904;
                    break;

                case NFe.Components.TipoEmissao.teSCAN:
                    CodigoUF = 900;
                    break;

                case NFe.Components.TipoEmissao.teDPEC:
                    if (servico == Servicos.ConsultarDPEC || servico == Servicos.EnviarDPEC)//danasa 21/10/2010
                        CodigoUF = 901;
                    break;

                default:
                    break;
            }
            string ufNome = CodigoUF.ToString();  //danasa 20-9-2010            

            #region varre a lista de webservices baseado no codigo da UF

            if (WebServiceProxy.webServicesList.Count == 0)
                throw new Exception("Lista de webservices não foi processada verifique se o arquivo 'WebService.xml' existe na pasta WSDL do UniNFe");

            var alist = (from p in WebServiceProxy.webServicesList where p.ID == CodigoUF select p);

            foreach (webServices list in alist)//WebServiceProxy.webServicesList)
            {
                if (list.ID == CodigoUF)
                {
                    switch (servico)
                    {
                        #region NFe
                        case Servicos.ConsultaCadastroContribuinte:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeConsultaCadastro : list.LocalProducao.NFeConsultaCadastro);
                            break;

                        case Servicos.EnviarLoteNfe2:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeAutorizacao : list.LocalProducao.NFeAutorizacao);
                            break;

                        case Servicos.EnviarLoteNfe:
                        case Servicos.EnviarDPEC:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRecepcao : list.LocalProducao.NFeRecepcao);
                            break;

                        case Servicos.InutilizarNumerosNFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeInutilizacao : list.LocalProducao.NFeInutilizacao);
                            break;

                        case Servicos.PedidoConsultaSituacaoNFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeConsulta : list.LocalProducao.NFeConsulta);
                            break;

                        case Servicos.ConsultarDPEC:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeConsulta : list.LocalProducao.NFeConsulta);
                            break;

                        case Servicos.ConsultaStatusServicoNFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeStatusServico : list.LocalProducao.NFeStatusServico);
                            break;

                        case Servicos.PedidoSituacaoLoteNFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRetRecepcao : list.LocalProducao.NFeRetRecepcao);
                            break;

                        case Servicos.PedidoSituacaoLoteNFe2:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRetAutorizacao : list.LocalProducao.NFeRetAutorizacao);
                            break;

                        case Servicos.EnviarCCe:
                        case Servicos.EnviarEventoCancelamento:
                        case Servicos.RecepcaoEvento:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRecepcaoEvento : list.LocalProducao.NFeRecepcaoEvento);
                            break;

                        case Servicos.ConsultaNFDest:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeConsultaNFeDest : list.LocalProducao.NFeConsultaNFeDest);
                            break;

                        case Servicos.DownloadNFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeDownload : list.LocalProducao.NFeDownload);
                            break;

                        case Servicos.EnviarManifDest:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeManifDest : list.LocalProducao.NFeManifDest);
                            break;

                        case Servicos.RegistroDeSaida:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRegistroDeSaida : list.LocalProducao.NFeRegistroDeSaida);
                            break;

                        case Servicos.RegistroDeSaidaCancelamento:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.NFeRegistroDeSaidaCancelamento : list.LocalProducao.NFeRegistroDeSaidaCancelamento);
                            break;

                        #endregion

                        #region MDF-e
                        case Servicos.ConsultaStatusServicoMDFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeStatusServico : list.LocalProducao.MDFeStatusServico);
                            break;
                        case Servicos.EnviarLoteMDFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeRecepcao : list.LocalProducao.MDFeRecepcao);
                            break;
                        case Servicos.PedidoSituacaoLoteMDFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeRetRecepcao : list.LocalProducao.MDFeRetRecepcao);
                            break;
                        case Servicos.PedidoConsultaSituacaoMDFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeConsulta : list.LocalProducao.MDFeConsulta);
                            break;
                        case Servicos.RecepcaoEventoMDFe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.MDFeRecepcaoEvento : list.LocalProducao.MDFeRecepcaoEvento);
                            break;
                        #endregion

                        #region CT-e
                        case Servicos.ConsultaStatusServicoCTe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeStatusServico : list.LocalProducao.CTeStatusServico);
                            break;
                        case Servicos.EnviarLoteCTe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeRecepcao : list.LocalProducao.CTeRecepcao);
                            break;
                        case Servicos.PedidoSituacaoLoteCTe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeRetRecepcao : list.LocalProducao.CTeRetRecepcao);
                            break;
                        case Servicos.PedidoConsultaSituacaoCTe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeConsulta : list.LocalProducao.CTeConsulta);
                            break;
                        case Servicos.InutilizarNumerosCTe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeInutilizacao : list.LocalProducao.CTeInutilizacao);
                            break;
                        case Servicos.RecepcaoEventoCTe:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.CTeRecepcaoEvento : list.LocalProducao.CTeRecepcaoEvento);
                            break;
                        #endregion

                        #region NFS-e
                        case Servicos.RecepcionarLoteRps:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.RecepcionarLoteRps : list.LocalProducao.RecepcionarLoteRps);
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarSituacaoLoteRps : list.LocalProducao.ConsultarSituacaoLoteRps);
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarNfsePorRps : list.LocalProducao.ConsultarNfsePorRps);
                            break;
                        case Servicos.ConsultarNfse:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarNfse : list.LocalProducao.ConsultarNfse);
                            break;
                        case Servicos.ConsultarLoteRps:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarLoteRps : list.LocalProducao.ConsultarLoteRps);
                            break;
                        case Servicos.CancelarNfse:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.CancelarNfse : list.LocalProducao.CancelarNfse);
                            break;
                        case Servicos.ConsultarURLNfse:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarURLNfse : list.LocalProducao.ConsultarURLNfse);
                            break;
                        case Servicos.ConsultarURLNfseSerie:
                            WSDL = (tipoAmbiente == (int)NFe.Components.TipoAmbiente.taHomologacao ? list.LocalHomologacao.ConsultarURLNfse : list.LocalProducao.ConsultarURLNfse);
                            break;
                        #endregion
                    }
                    if (tipoEmissao == (int)NFe.Components.TipoEmissao.teDPEC)
                        ufNome = "DPEC";
                    else
                        ufNome = "de " + list.Nome;

                    break;
                }
            }
            #endregion

            //Se for NFCe tenho que ver se o estado não tem WSDL específico, ou seja, diferente da NFe
            //Wandrey 17/04/2014
            if (ehNFCe && !string.IsNullOrEmpty(WSDL) && File.Exists(WSDL))
            {
                string temp = Path.Combine(Path.GetDirectoryName(WSDL), Path.GetFileNameWithoutExtension(WSDL) + "_C" + Path.GetExtension(WSDL));
                if (File.Exists(temp))
                    WSDL = temp;
            }

            ///
            /// danasa: 4-2014
            /// Compatibilidade com a versao 2.00
            /// 
            if (!string.IsNullOrEmpty(versao) && (versao.Equals("2.01") || versao.Equals("2.00")) && !string.IsNullOrEmpty(WSDL) && File.Exists(WSDL))
            {
                string temp = Path.Combine(Path.GetDirectoryName(WSDL), Path.GetFileNameWithoutExtension(WSDL) + "_200" + Path.GetExtension(WSDL));
                if (File.Exists(temp))
                    WSDL = temp;
            }

            if (string.IsNullOrEmpty(WSDL) || !File.Exists(WSDL))
            {
                if (!File.Exists(WSDL) && !string.IsNullOrEmpty(WSDL))
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Nfe:
                            throw new Exception("O arquivo \"" + WSDL + "\" não existe. Aconselhamos a reinstalação do UniNFe.");
                        case TipoAplicativo.Nfse:
                            throw new Exception("O arquivo \"" + WSDL + "\" não existe. Aconselhamos a reinstalação do UniNFSe.");
                    }

                string Ambiente = string.Empty;
                switch ((NFe.Components.TipoAmbiente)tipoAmbiente)
                {
                    case NFe.Components.TipoAmbiente.taProducao:
                        Ambiente = "produção";
                        break;

                    case NFe.Components.TipoAmbiente.taHomologacao:
                        Ambiente = "homologação";
                        break;
                }
                string errorStr = "O Estado " + ufNome + " ainda não dispõe do serviço {0} para o ambiente de " + Ambiente + ".";

                switch (servico)
                {
                    case Servicos.EnviarCCe:
                    case Servicos.EnviarEventoCancelamento:
                    case Servicos.RecepcaoEvento:
                    case Servicos.EnviarManifDest:
                        throw new Exception(string.Format(errorStr, "de envio de eventos da NFe"));

                    case Servicos.ConsultaNFDest:
                        throw new Exception(string.Format(errorStr, "de envio de consulta a NFe do destinatário"));

                    case Servicos.DownloadNFe:
                        throw new Exception(string.Format(errorStr, "de envio de download de NFe do destinatário"));

                    case Servicos.ConsultaStatusServicoMDFe:
                    case Servicos.PedidoConsultaSituacaoMDFe:
                    case Servicos.PedidoSituacaoLoteMDFe:
                    case Servicos.EnviarLoteMDFe:
                    case Servicos.RecepcaoEventoMDFe:
                        throw new Exception(string.Format(errorStr, "do MDF-e"));

                    case Servicos.ConsultaStatusServicoCTe:
                    case Servicos.PedidoConsultaSituacaoCTe:
                    case Servicos.PedidoSituacaoLoteCTe:
                    case Servicos.EnviarLoteCTe:
                    case Servicos.RecepcaoEventoCTe:
                    case Servicos.InutilizarNumerosCTe:
                        throw new Exception(string.Format(errorStr, "do CT-e"));

                    default:
                        switch (servico)
                        {
                            case Servicos.CancelarNfse:
                            case Servicos.ConsultarURLNfse:
                            case Servicos.ConsultarURLNfseSerie:
                            case Servicos.ConsultarLoteRps:
                            case Servicos.ConsultarNfse:
                            case Servicos.ConsultarNfsePorRps:
                            case Servicos.ConsultarSituacaoLoteRps:
                            case Servicos.RecepcionarLoteRps:
                                throw new Exception(string.Format(errorStr, "da NFS-e"));
                        }
                        throw new Exception(string.Format(errorStr, "da NF-e"));
                }
            }

            return WSDL;
        }
        #endregion

        #region GravarConfig()
        /// <summary>
        /// Método responsável por gravar as configurações da Aplicação no arquivo "UniNfeConfig.xml"
        /// </summary>
        /// <returns>Retorna true se conseguiu gravar corretamente as configurações ou false se não conseguiu</returns>
        public void GravarConfig(bool gravaArqEmpresa, bool validaCertificado)  //<<<<<<danasa 1-5-2011
        {
            ValidarConfig(validaCertificado);
            GravarConfigGeral();
            GravarConfigEmpresa();
            if (gravaArqEmpresa)        //<<<<<<danasa 1-5-2011
            {                           //<<<<<<danasa 1-5-2011
                GravarArqEmpresas();    //<<<<<<danasa 1-5-2011
            }                           //<<<<<<danasa 1-5-2011
        }
        #endregion

        #region Gravar XML com as empresas cadastradas

        public void GravarArqEmpresas()
        {
            if (Empresas.Configuracoes.Count == 0 && File.Exists(Propriedade.NomeArqEmpresas))
            {
                try
                {
                    File.Delete(Propriedade.NomeArqEmpresas);
                }
                catch { }
                return;
            }
            XElement empresasele = new XElement("Empresa");
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));

#if false
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(true);

            //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            //Agora vamos criar um XML Writer
            XmlWriter oXmlGravar = XmlWriter.Create(Propriedade.NomeArqEmpresas, oSettings);

            //Agora vamos gravar os dados
            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("Empresa");
#endif
            foreach (Empresa empresa in Empresas.Configuracoes)
            {
                empresasele.Add(new XElement(NFe.Components.NFeStrConstants.Registro,
                                new XAttribute(NFe.Components.NFeStrConstants.CNPJ, empresa.CNPJ),
                                new XAttribute(NFe.Components.NFeStrConstants.Servico, ((int)empresa.Servico).ToString()),
                                new XElement(NFe.Components.NFeStrConstants.Nome, empresa.Nome.Trim())));

#if false
                //Abrir a tag <Registro>
                oXmlGravar.WriteStartElement(NFe.Components.NFeStrConstants.Registro);

                //Criar o atributo CNPJ dentro da tag Registro
                oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.CNPJ);
                oXmlGravar.WriteString(empresa.CNPJ);

                //Criar o atributo Servico dentro da tag Registro
                oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.Servico);
                oXmlGravar.WriteString(((int)empresa.Servico).ToString());

                //Encerrar o atributo CNPJ
                oXmlGravar.WriteEndAttribute(); // Encerrar o atributo CNPJ

                //Criar a tag <Nome> com seu conteúdo </Nome>
                oXmlGravar.WriteElementString(NFe.Components.NFeStrConstants.Nome, empresa.Nome.Trim());

                //Encerrar a tag </Registro>
                oXmlGravar.WriteEndElement();
#endif
            }
#if false
            oXmlGravar.WriteEndElement(); //Encerrar o elemento Empresa
            oXmlGravar.WriteEndDocument();
            oXmlGravar.Flush();
            oXmlGravar.Close();
#endif
            xml.Add(empresasele);
            xml.Save(Propriedade.NomeArqEmpresas);
        }
        #endregion

        #region GravarConfigEmpresa()
        /// <summary>
        /// Gravar as configurações das empresas
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        private void GravarConfigEmpresa()
        {
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            foreach (Empresa empresa in Empresas.Configuracoes)
            {
                XmlWriter oXmlGravar = null;

                try
                {
                    //Criar pasta das configurações das empresas
                    string pasta = Propriedade.PastaExecutavel + "\\" + empresa.CNPJ.Trim();
                    switch (empresa.Servico)
                    {
                        case TipoAplicativo.Nfe:
                            break;
                        default:
                            pasta += "\\" + empresa.Servico.ToString().ToLower();
                            break;
                    }
                    if (!Directory.Exists(pasta))
                        Directory.CreateDirectory(pasta);

                    //Agora vamos criar um XML Writer
                    oXmlGravar = XmlWriter.Create(pasta + "\\" + Propriedade.NomeArqConfig, oSettings);

                    //Agora vamos gravar os dados
                    oXmlGravar.WriteStartDocument();
                    oXmlGravar.WriteStartElement(NFeStrConstants.nfe_configuracoes);
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEnvio, empresa.PastaXmlEnvio);
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlRetorno, empresa.PastaXmlRetorno);
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlErro, empresa.PastaXmlErro);
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaValidar, empresa.PastaValidar);
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEnviado, empresa.PastaXmlEnviado);
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaBackup, empresa.PastaBackup);
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEmLote, empresa.PastaXmlEmLote);
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaDownloadNFeDest, empresa.PastaDownloadNFeDest);
                    }
                    oXmlGravar.WriteElementString(NFeStrConstants.UnidadeFederativaCodigo, empresa.UnidadeFederativaCodigo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.AmbienteCodigo, empresa.AmbienteCodigo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.CertificadoDigital, empresa.Certificado);
                    oXmlGravar.WriteElementString(NFeStrConstants.CertificadoInstalado, empresa.CertificadoInstalado.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.CertificadoArquivo, empresa.CertificadoArquivo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.CertificadoSenha, Criptografia.criptografaSenha(empresa.CertificadoSenha));
                    oXmlGravar.WriteElementString(NFeStrConstants.CertificadoPIN, Criptografia.criptografaSenha(empresa.CertificadoPIN));
                    oXmlGravar.WriteElementString(NFeStrConstants.tpEmis, empresa.tpEmis.ToString());
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarRetornoTXTNFe, empresa.GravarRetornoTXTNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosDeTerceiros, empresa.GravarEventosDeTerceiros.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosNaPastaEnviadosNFe, empresa.GravarEventosNaPastaEnviadosNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe, empresa.GravarEventosCancelamentoNaPastaEnviadosNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.DiretorioSalvarComo, empresa.DiretorioSalvarComo.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.IndSinc, empresa.IndSinc.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.CompactarNfe, empresa.CompactarNfe.ToString());
                    }

                    oXmlGravar.WriteElementString(NFeStrConstants.DiasLimpeza, empresa.DiasLimpeza.ToString());
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        oXmlGravar.WriteElementString(NFeStrConstants.ConfiguracaoDanfe, empresa.ConfiguracaoDanfe);
                        oXmlGravar.WriteElementString(NFeStrConstants.ConfiguracaoCCe, empresa.ConfiguracaoCCe);
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaExeUniDanfe, empresa.PastaExeUniDanfe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaConfigUniDanfe, empresa.PastaConfigUniDanfe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaDanfeMon, empresa.PastaDanfeMon.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.XMLDanfeMonNFe, empresa.XMLDanfeMonNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.XMLDanfeMonProcNFe, empresa.XMLDanfeMonProcNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.XMLDanfeMonDenegadaNFe, empresa.XMLDanfeMonDenegadaNFe.ToString());
                        oXmlGravar.WriteElementString("EmailDanfe", empresa.EmailDanfe);
                        oXmlGravar.WriteElementString("AdicionaEmailDanfe", empresa.AdicionaEmailDanfe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.TempoConsulta, empresa.TempoConsulta.ToString());
                    }
                    ///
                    /// informacoes do FTP
                    /// danasa 7/7/2011
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPAtivo, empresa.FTPAtivo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPNomeDoUsuario, empresa.FTPNomeDoUsuario.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPNomeDoServidor, empresa.FTPNomeDoServidor.ToString());
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        oXmlGravar.WriteElementString(NFeStrConstants.FTPPastaAutorizados, empresa.FTPPastaAutorizados.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.FTPGravaXMLPastaUnica, empresa.FTPGravaXMLPastaUnica.ToString());
                    }
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPPastaRetornos, empresa.FTPPastaRetornos.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPPorta, empresa.FTPPorta.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPSenha, empresa.FTPSenha.ToString());

                    if (empresa.CertificadoInstalado)
                    {
                        //oXmlGravar.WriteElementString(NFeStrConstants.CertificadoDigitalThumbPrint, empresa.X509Certificado.Thumbprint);
                        oXmlGravar.WriteElementString(NFeStrConstants.CertificadoDigitalThumbPrint, empresa.CertificadoDigitalThumbPrint);
                    }

                    oXmlGravar.WriteElementString(NFeStrConstants.UsuarioWS, (empresa.UsuarioWS == null ? "" : empresa.UsuarioWS.ToString()));
                    oXmlGravar.WriteElementString(NFeStrConstants.SenhaWS, (empresa.SenhaWS == null ? "" : empresa.SenhaWS.ToString()));

                    oXmlGravar.WriteEndElement(); //nfe_configuracoes
                    oXmlGravar.WriteEndDocument();
                    oXmlGravar.Flush();
                }
                finally
                {
                    if (oXmlGravar != null)
                        oXmlGravar.Close();
                }
            }
        }
        #endregion

        #region GravarConfigGeral()
        /// <summary>
        /// Gravar as configurações gerais
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        public void GravarConfigGeral()
        {
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement elementos = new XElement(NFeStrConstants.nfe_configuracoes);
            elementos.Add(new XElement(NfeConfiguracoes.Proxy.ToString(), ConfiguracaoApp.Proxy.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.ProxyServidor.ToString(), ConfiguracaoApp.ProxyServidor));
            elementos.Add(new XElement(NfeConfiguracoes.ProxyUsuario.ToString(), ConfiguracaoApp.ProxyUsuario));
            elementos.Add(new XElement(NfeConfiguracoes.ProxyPorta.ToString(), ConfiguracaoApp.ProxyPorta.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.ChecarConexaoInternet.ToString(), ConfiguracaoApp.ChecarConexaoInternet.ToString()));
            elementos.Add(new XElement(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString(), ConfiguracaoApp.GravarLogOperacoesRealizadas.ToString()));
            if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig))
            {
                if (ConfiguracaoApp.mSenhaConfigAlterada)
                {
                    ConfiguracaoApp.SenhaConfig = Functions.GerarMD5(ConfiguracaoApp.SenhaConfig);
                }

                elementos.Add(new XElement(NfeConfiguracoes.SenhaConfig.ToString(), ConfiguracaoApp.SenhaConfig));
                ConfiguracaoApp.mSenhaConfigAlterada = false;
            }
            xml.Add(elementos);
            xml.Save(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig);

#if false
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            XmlWriter oXmlGravar = null;

            try
            {
                //Agora vamos criar um XML Writer
                oXmlGravar = XmlWriter.Create(Propriedade.PastaExecutavel + "\\" + Propriedade.NomeArqConfig, oSettings);

                //Agora vamos gravar os dados
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement(NFeStrConstants.nfe_configuracoes);
                oXmlGravar.WriteElementString(NfeConfiguracoes.Proxy.ToString(), ConfiguracaoApp.Proxy.ToString());
                oXmlGravar.WriteElementString(NfeConfiguracoes.ProxyServidor.ToString(), ConfiguracaoApp.ProxyServidor);
                oXmlGravar.WriteElementString(NfeConfiguracoes.ProxyUsuario.ToString(), ConfiguracaoApp.ProxyUsuario);
                oXmlGravar.WriteElementString(NfeConfiguracoes.ProxySenha.ToString(), ConfiguracaoApp.ProxySenha);
                oXmlGravar.WriteElementString(NfeConfiguracoes.ProxyPorta.ToString(), ConfiguracaoApp.ProxyPorta.ToString());
                oXmlGravar.WriteElementString(NfeConfiguracoes.ChecarConexaoInternet.ToString(), ConfiguracaoApp.ChecarConexaoInternet.ToString());
                oXmlGravar.WriteElementString(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString(), ConfiguracaoApp.GravarLogOperacoesRealizadas.ToString());
                if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig))
                {
                    if (ConfiguracaoApp.mSenhaConfigAlterada)
                    {
                        ConfiguracaoApp.SenhaConfig = Functions.GerarMD5(ConfiguracaoApp.SenhaConfig);
                    }

                    oXmlGravar.WriteElementString(NfeConfiguracoes.SenhaConfig.ToString(), ConfiguracaoApp.SenhaConfig);
                    ConfiguracaoApp.mSenhaConfigAlterada = false;
                }

                oXmlGravar.WriteEndElement(); //nfe_configuracoes
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
            }
            finally
            {
                if (oXmlGravar != null)
                    oXmlGravar.Close();
            }
#endif
        }
        #endregion

        #region ValidarConfig()

        static void AddEmpresaNaListaDeComparacao(List<FolderCompare> fc, int i, Empresa empresa)
        {
            fc.Add(new FolderCompare(i, empresa.PastaXmlEnvio));
            fc.Add(new FolderCompare(i + 1, empresa.PastaXmlRetorno));
            fc.Add(new FolderCompare(i + 2, empresa.PastaXmlErro));
            fc.Add(new FolderCompare(i + 3, empresa.PastaValidar));
            if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
            {
                fc.Add(new FolderCompare(i + 4, empresa.PastaXmlEnviado));
                fc.Add(new FolderCompare(i + 5, empresa.PastaXmlEmLote));
                fc.Add(new FolderCompare(i + 6, empresa.PastaBackup));
                fc.Add(new FolderCompare(i + 7, empresa.PastaDownloadNFeDest));
            }
        }
        /// <summary>
        /// Verifica se algumas das informações das configurações tem algum problema ou falha
        /// </summary>
        /// <param name="validarCertificado">Se valida se tem certificado informado ou não nas configurações</param>
        private void ValidarConfig(bool validarCertificado)
        {
            string erro = string.Empty;
            bool validou = true;

            #region Remover End Slash
            for (int i = 0; i < Empresas.Configuracoes.Count; i++)
            {
                Empresas.Configuracoes[i].RemoveEndSlash();
            }
            #endregion

            #region Verificar a duplicação de nome de pastas que não pode existir
            List<FolderCompare> fc = new List<FolderCompare>();

            for (int i = 0; i < Empresas.Configuracoes.Count; i++)
            {
                AddEmpresaNaListaDeComparacao(fc, i, Empresas.Configuracoes[i]);
            }

            foreach (FolderCompare fc1 in fc)
            {
                if (string.IsNullOrEmpty(fc1.folder))
                    continue;

                foreach (FolderCompare fc2 in fc)
                {
                    if (fc1.id != fc2.id)
                    {
                        if (fc1.folder.ToLower().Equals(fc2.folder.ToLower()))
                        {
                            erro = "Não é permitido a utilização de pasta idênticas na mesma ou entre empresas..";
                            validou = false;
                            break;
                        }
                    }
                }
                if (!validou)
                    break;
            }
            #endregion

            if (validou)
            {
                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    Empresa empresa = Empresas.Configuracoes[i];
                    List<string> diretorios = new List<string>();
                    List<string> mensagens = new List<string>();

                    #region Verificar se tem alguma pasta em branco
                    diretorios.Add(empresa.PastaXmlEnvio); mensagens.Add("Informe a pasta de envio dos arquivos XML.");
                    diretorios.Add(empresa.PastaXmlRetorno); mensagens.Add("Informe a pasta de retorno dos arquivos XML.");
                    diretorios.Add(empresa.PastaXmlErro); mensagens.Add("Informe a pasta para arquivamento temporário dos arquivos XML que apresentaram erros.");
                    diretorios.Add(empresa.PastaValidar); mensagens.Add("Informe a pasta onde será gravado os arquivos XML somente para ser validado pela Aplicação.");
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        diretorios.Add(empresa.PastaXmlEmLote); mensagens.Add("Informe a pasta de envio dos arquivos XML em lote.");
                        diretorios.Add(empresa.PastaXmlEnviado); mensagens.Add("Informe a pasta para arquivamento dos arquivos XML enviados.");
                        //diretorios.Add(empresa.PastaBackup); mensagens.Add("Informe a pasta para o Backup dos arquivos XML enviados.");
                        //diretorios.Add(empresa.PastaDownloadNFeDest); mensagens.Add("Informe a pasta onde será gravado os arquivos XML de download de destinatário.");
                    }

                    for (int b = 0; b < diretorios.Count; b++)
                    {
                        if (diretorios[b].Equals(string.Empty))
                        {
                            erro = mensagens[b].Trim() + "\r\n" + Empresas.Configuracoes[i].Nome + "\r\n" + Empresas.Configuracoes[i].CNPJ;
                            validou = false;
                            break;
                        }
                    }
                    ///
                    /// informacoes do FTP
                    /// danasa 7/7/2011
                    if (empresa.FTPIsAlive)
                    {
                        if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                        {
                            if (string.IsNullOrEmpty(empresa.FTPPastaAutorizados))
                            {
                                erro = "Informe a pasta do FTP de destino dos autorizados\r\n" + Empresas.Configuracoes[i].Nome + "\r\n" + Empresas.Configuracoes[i].CNPJ;
                                validou = false;
                            }
                        }

                        if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                            if (string.IsNullOrEmpty(empresa.FTPPastaRetornos))
                            {
                                erro = "Informe a pasta do FTP de destino dos retornos\r\n" + Empresas.Configuracoes[i].Nome + "\r\n" + Empresas.Configuracoes[i].CNPJ;
                                validou = false;
                            }
                    }
                    #endregion

                    #region Verificar se o certificado foi informado
                    if (validarCertificado)
                    {
                        if (validou)
                        {
                            if (empresa.CertificadoInstalado && empresa.CertificadoDigitalThumbPrint.Equals(string.Empty))
                            {
                                erro = "Selecione o certificado digital a ser utilizado na autenticação dos serviços.\r\n" + Empresas.Configuracoes[i].Nome + "\r\n" + Empresas.Configuracoes[i].CNPJ;
                                validou = false;
                            }
                            if (!empresa.CertificadoInstalado)
                            {
                                if (empresa.CertificadoArquivo.Equals(string.Empty))
                                {
                                    erro = "Informe o local de armazenamento do certificado digital a ser utilizado na autenticação dos serviços.\r\n" + Empresas.Configuracoes[i].Nome + "\r\n" + Empresas.Configuracoes[i].CNPJ;
                                    validou = false;
                                }
                                else if (!File.Exists(empresa.CertificadoArquivo))
                                {
                                    erro = "Arquivo do certificado digital a ser utilizado na autenticação dos serviços não foi encontrado.\r\n" + Empresas.Configuracoes[i].Nome + "\r\n" + Empresas.Configuracoes[i].CNPJ;
                                    validou = false;
                                }
                                else if (empresa.CertificadoSenha.Equals(string.Empty))
                                {
                                    erro = "Informe a senha do certificado digital a ser utilizado na autenticação dos serviços.\r\n" + Empresas.Configuracoes[i].Nome + "\r\n" + Empresas.Configuracoes[i].CNPJ;
                                    validou = false;
                                }
                                else
                                {
                                    try
                                    {
                                        using (FileStream fs = new FileStream(empresa.CertificadoArquivo, FileMode.Open))
                                        {
                                            byte[] buffer = new byte[fs.Length];
                                            fs.Read(buffer, 0, buffer.Length);
                                            empresa.X509Certificado = new X509Certificate2(buffer, empresa.CertificadoSenha);
                                        }
                                    }
                                    catch (System.Security.Cryptography.CryptographicException ex)
                                    {
                                        erro = ex.Message + ".\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                        validou = false;
                                    }
                                    catch (Exception ex)
                                    {
                                        erro = ex.Message + ".\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                        validou = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Verificar se as pastas informadas existem
                    if (validou)
                    {
                        //Fazer um pequeno ajuste na pasta de configuração do unidanfe antes de verificar sua existência
                        if (empresa.PastaConfigUniDanfe.Trim() != string.Empty)
                        {
                            if (!string.IsNullOrEmpty(empresa.PastaConfigUniDanfe))
                            {
                                while (Empresas.Configuracoes[i].PastaConfigUniDanfe.Substring(Empresas.Configuracoes[i].PastaConfigUniDanfe.Length - 6, 6).ToLower() == @"\dados" && !string.IsNullOrEmpty(Empresas.Configuracoes[i].PastaConfigUniDanfe))
                                    Empresas.Configuracoes[i].PastaConfigUniDanfe = Empresas.Configuracoes[i].PastaConfigUniDanfe.Substring(0, Empresas.Configuracoes[i].PastaConfigUniDanfe.Length - 6);
                            }
                            Empresas.Configuracoes[i].PastaConfigUniDanfe = Empresas.Configuracoes[i].PastaConfigUniDanfe.Replace("\r\n", "").Trim();

                            empresa.PastaConfigUniDanfe = Empresas.Configuracoes[i].PastaConfigUniDanfe;
                        }

                        if (empresa.PastaXmlEnvio.ToLower().EndsWith("temp"))
                        {
                            erro = "Pasta de envio não pode terminar com a subpasta 'temp'.\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                            validou = false;
                        }
                        else
                            if (empresa.PastaXmlEmLote.ToLower().EndsWith("temp"))
                            {
                                erro = "Pasta de envio em lote não pode terminar com a subpasta 'temp'.\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                validou = false;
                            }
                            else
                                if (empresa.PastaValidar.ToLower().EndsWith("temp"))
                                {
                                    erro = "Pasta de validação não pode terminar com a subpasta 'temp'.\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                    validou = false;
                                }
                                else
                                    if (empresa.PastaXmlErro.ToLower().EndsWith("temp"))
                                    {
                                        erro = "Pasta de XML's com erro na tentativa de envio não pode terminar com a subpasta 'temp'.\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                        validou = false;
                                    }

                        if (validou)
                        {
                            diretorios.Clear(); mensagens.Clear();
                            diretorios.Add(empresa.PastaXmlEnvio.Trim()); mensagens.Add("A pasta de envio dos arquivos XML informada não existe.");
                            diretorios.Add(empresa.PastaXmlRetorno.Trim()); mensagens.Add("A pasta de retorno dos arquivos XML informada não existe.");
                            diretorios.Add(empresa.PastaXmlErro.Trim()); mensagens.Add("A pasta para arquivamento temporário dos arquivos XML com erro informada não existe.");
                            diretorios.Add(empresa.PastaValidar.Trim()); mensagens.Add("A pasta para validação de XML´s informada não existe.");
                            if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
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
                                            erro = mensagens[b].Trim() + "\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                            validou = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        #region Criar pasta Temp dentro da pasta de envio, envio em lote e validar
                        //Criar pasta Temp dentro da pasta de envio, envio em Lote e Validar. Wandrey 03/08/2011
                        if (validou)
                        {
                            if (Directory.Exists(empresa.PastaXmlEnvio.Trim()))
                            {
                                if (!Directory.Exists(empresa.PastaXmlEnvio.Trim() + "\\Temp"))
                                {
                                    Directory.CreateDirectory(empresa.PastaXmlEnvio.Trim() + "\\Temp");
                                }
                            }

                            if (Directory.Exists(empresa.PastaXmlEmLote.Trim()) && Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                            {
                                if (!Directory.Exists(empresa.PastaXmlEmLote.Trim() + "\\Temp"))
                                {
                                    Directory.CreateDirectory(empresa.PastaXmlEmLote.Trim() + "\\Temp");
                                }
                            }

                            if (Directory.Exists(empresa.PastaValidar.Trim()))
                            {
                                if (!Directory.Exists(empresa.PastaValidar.Trim() + "\\Temp"))
                                {
                                    Directory.CreateDirectory(empresa.PastaValidar.Trim() + "\\Temp");
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Verificar se as pastas configuradas do unidanfe estão corretas
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        if (validou && empresa.PastaExeUniDanfe.Trim() != string.Empty)
                        {
                            if (!File.Exists(empresa.PastaExeUniDanfe + "\\unidanfe.exe"))
                            {
                                erro = "O executável do UniDANFe não foi localizado na pasta informada.\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                validou = false;
                            }
                        }

                        if (validou && empresa.PastaConfigUniDanfe.Trim() != string.Empty)
                        {
                            //Verificar a existência o arquivo de configuração
                            if (!File.Exists(empresa.PastaConfigUniDanfe + "\\dados\\config.tps"))
                            {
                                erro = "O arquivo de configuração do UniDANFe não foi localizado na pasta informada.\r\n" + empresa.Nome + "\r\n" + empresa.CNPJ;
                                validou = false;
                            }
                        }
                    }
                    #endregion
                }
            }

            #region Ticket: #110
            /* Validar se já existe uma instancia utilizando estes diretórios
             * Marcelo
             * 03/06/2013
             */
            if (validou)
            {
                //Se encontrar algum arquivo de lock nos diretórios, não permtir que seja executado
                erro = Empresas.CanRun(false);
                validou = String.IsNullOrEmpty(erro);
            }
            #endregion


            if (!validou)
            {
                throw new Exception(erro);
            }
        }
        #endregion

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
            int emp = Empresas.FindEmpresaByThread();

            string cStat = "";
            string xMotivo = "";
            bool lErro = false;
            bool lEncontrouTag = false;

            try
            {
                ///
                /// inclui o processo de inclusao de empresa pelo 'txt'
                emp = CadastrarEmpresa(cArquivoXml);

                if (Path.GetExtension(cArquivoXml).ToLower() == ".txt")
                {
                    #region Formato TXT

                    List<string> cLinhas = Functions.LerArquivo(cArquivoXml);

                    lEncontrouTag = Functions.PopulateClasse(Empresas.Configuracoes[emp], cLinhas);

                    foreach (string texto in cLinhas)
                    {
                        string[] dados = texto.Split('|');
                        int nElementos = dados.GetLength(0);
                        if (nElementos <= 1)
                            continue;

                        switch (dados[0].ToLower())
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
                                ConfiguracaoApp.SenhaConfig = (nElementos == 2 ? dados[1].Trim() : "");
                                ConfiguracaoApp.mSenhaConfigAlterada = false;
                                lEncontrouTag = true;
                                break;

                            #region --nao usar
#if false
                            #region Usuário e Senha WS
                            case "usuariows":
                                Empresas.Configuracoes[emp].UsuarioWS = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "senhaws":
                                Empresas.Configuracoes[emp].SenhaWS = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            #endregion

                            case "pastaxmlenvio":
                                Empresas.Configuracoes[emp].PastaXmlEnvio = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastaxmlemlote":
                                Empresas.Configuracoes[emp].PastaXmlEmLote = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastaxmlretorno":
                                Empresas.Configuracoes[emp].PastaXmlRetorno = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastaxmlenviado": //Se a tag <PastaXmlEnviado> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaXmlEnviado = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastaxmlerro":    //Se a tag <PastaXmlErro> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaXmlErro = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastabackup": //Se a tag <PastaBackup> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaBackup = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastadownloadnfedest": //Se a tag <PastaDownloadNFeDest> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaDownloadNFeDest = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastavalidar":    //Se a tag <PastaValidar> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaValidar = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "unidadefederativacodigo": //Se a tag <UnidadeFederativaCodigo> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].UnidadeFederativaCodigo = (nElementos == 2 ? Convert.ToInt32("0" + dados[1].Trim()) : 0);
                                lEncontrouTag = true;
                                break;
                            case "ambientecodigo":  //Se a tag <AmbienteCodigo> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].AmbienteCodigo = (nElementos == 2 ? Convert.ToInt32("0" + dados[1].Trim()) : 1);
                                lEncontrouTag = true;
                                break;
                            case "tpemis":  //Se a tag <tpEmis> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].tpEmis = (nElementos == 2 ? Convert.ToInt32("0" + dados[1].Trim()) : 0);
                                lEncontrouTag = true;
                                break;
                            case "gravarretornotxtnfe": //Se a tag <GravarRetornoTXTNFe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].GravarRetornoTXTNFe = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "gravareventosdeterceiros":
                                Empresas.Configuracoes[emp].GravarEventosDeTerceiros = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "gravareventosnapastaenviadosnfe":
                                Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "gravareventoscancelamentonapastaenviadosnfe":
                                Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "indsinc":
                                Empresas.Configuracoes[emp].IndSinc = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "diretoriosalvarcomo": //Se a tag <DiretorioSalvarComo> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].DiretorioSalvarComo = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "diaslimpeza": //Se a tag <DiasLimpeza> existir ele pega o novo conteúdo
                                Empresas.Configuracoes[emp].DiasLimpeza = (nElementos == 2 ? Convert.ToInt32("0" + dados[1].Trim()) : 0);
                                lEncontrouTag = true;
                                break;
                            case "configuracaodanfe": //Se a tag <ConfiguracaoDanfe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].ConfiguracaoDanfe = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "configuracaocce": //Se a tag <ConfiguracaoCCe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].ConfiguracaoCCe = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "pastaexeunidanfe": //Se a tag <PastaExeUniDanfe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaExeUniDanfe = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastaconfigunidanfe": //Se a tag <PastaConfigUniDanfe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaConfigUniDanfe = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "pastadanfemon": //Se a tag <PastaDanfeMon> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].PastaDanfeMon = (nElementos == 2 ? ConfiguracaoApp.RemoveEndSlash(dados[1].Trim()) : "");
                                lEncontrouTag = true;
                                break;
                            case "xmldanfemonnfe": //Se a tag <XMLDanfeMonNFe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].XMLDanfeMonNFe = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "xmldanfemonprocnfe": //Se a tag <XMLDanfeMonProcNFe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].XMLDanfeMonProcNFe = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "xmldanfemondenegadanfe": //Se a tag <XMLDanfeMonProcNFe> existir ele pega no novo conteúdo
                                Empresas.Configuracoes[emp].XMLDanfeMonDenegadaNFe = (nElementos == 2 ? dados[1].Trim() == "True" : false);
                                lEncontrouTag = true;
                                break;
                            case "tempoconsulta": //Se a tag <TempoConsulta> existir ele pega o novo conteúdo
                                Empresas.Configuracoes[emp].TempoConsulta = (nElementos == 2 ? Convert.ToInt32("0" + dados[1].Trim()) : 0);
                                lEncontrouTag = true;
                                break;
                            ///
                            /// informacoes do FTP
                            /// danasa 7/7/2011
                            #region -- FTP
                            case "ftpativo":
                                Empresas.Configuracoes[emp].FTPAtivo = (nElementos == 2 ? Convert.ToBoolean(dados[1].Trim()) : false);
                                lEncontrouTag = true;
                                break;
                            case "ftpgravaxmlpastaunica":
                                Empresas.Configuracoes[emp].FTPGravaXMLPastaUnica = (nElementos == 2 ? Convert.ToBoolean(dados[1].Trim()) : false);
                                lEncontrouTag = true;
                                break;
                            case "ftpsenha":
                                Empresas.Configuracoes[emp].FTPSenha = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "ftppastaautorizados":
                                Empresas.Configuracoes[emp].FTPPastaAutorizados = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "ftppastaretornos":
                                Empresas.Configuracoes[emp].FTPPastaRetornos = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "ftpnomedousuario":
                                Empresas.Configuracoes[emp].FTPNomeDoUsuario = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "ftpnomedoservidor":
                                Empresas.Configuracoes[emp].FTPNomeDoServidor = (nElementos == 2 ? dados[1].Trim() : "");
                                lEncontrouTag = true;
                                break;
                            case "ftpporta":
                                Empresas.Configuracoes[emp].FTPPorta = (nElementos == 2 ? Convert.ToInt32("0" + dados[1].Trim()) : 21);
                                lEncontrouTag = true;
                                break;
                            #endregion

#endif
                            #endregion
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Formato XML
                    XmlDocument doc = new XmlDocument();
                    doc.Load(cArquivoXml);

                    XmlNodeList ConfUniNfeList = doc.GetElementsByTagName("altConfUniNFe");

                    foreach (XmlNode ConfUniNfeNode in ConfUniNfeList)
                    {
                        XmlElement ConfUniNfeElemento = (XmlElement)ConfUniNfeNode;
                        lEncontrouTag = Functions.PopulateClasse(Empresas.Configuracoes[emp], ConfUniNfeElemento);

                        //Se a tag <Proxy> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.Proxy.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.Proxy = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.Proxy.ToString())[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <ProxyServidor> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyServidor.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.ProxyServidor = ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyServidor.ToString())[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <ProxyUsuario> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyUsuario.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.ProxyUsuario = ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyUsuario.ToString())[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <ProxySenha> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxySenha.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.ProxySenha = ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxySenha.ToString())[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <ProxyPorta> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyPorta.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.ProxyPorta = Convert.ToInt32("0" + ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ProxyPorta.ToString())[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <ChecarConexaoInternet> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ChecarConexaoInternet.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.ChecarConexaoInternet = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.ChecarConexaoInternet.ToString())[0].InnerText);
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.GravarLogOperacoesRealizadas = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.GravarLogOperacaoRealizada.ToString())[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <SenhaConfig> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.SenhaConfig.ToString()).Count != 0)
                        {
                            ConfiguracaoApp.SenhaConfig = ConfUniNfeElemento.GetElementsByTagName(NfeConfiguracoes.SenhaConfig.ToString())[0].InnerText;
                            ConfiguracaoApp.mSenhaConfigAlterada = false;
                            lEncontrouTag = true;
                        }

                        #region --nao usar
#if false
                        #region Usuário e Senha WS
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.UsuarioWS).Count != 0)
                        {
                            Empresas.Configuracoes[emp].UsuarioWS = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.UsuarioWS)[0].InnerText.Trim();
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.SenhaWS).Count != 0)
                        {
                            Empresas.Configuracoes[emp].SenhaWS = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.SenhaWS)[0].InnerText.Trim();
                            lEncontrouTag = true;
                        }
                        #endregion



                        //Se a tag <PastaXmlEnvio> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlEnvio).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaXmlEnvio = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlEnvio)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaXmlEmLote> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlEmLote).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaXmlEmLote = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlEmLote)[0].InnerText, true);
                            lEncontrouTag = true;
                        }

                        //Se a tag <PastaXmlEnviado> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlEnviado).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaXmlEnviado = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlEnviado)[0].InnerText, true);
                            lEncontrouTag = true;
                        }

                        //Se a tag <PastaBackup> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaBackup).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaBackup = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaBackup)[0].InnerText, true);
                            lEncontrouTag = true;
                        }

                        //Se a tag <PastaXmlRetorno> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlRetorno).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaXmlRetorno = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlRetorno)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaXmlErro> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlErro).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaXmlErro = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaXmlErro)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <UnidadeFederativaCodigo> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.UnidadeFederativaCodigo).Count != 0)
                        {
                            Empresas.Configuracoes[emp].UnidadeFederativaCodigo = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.UnidadeFederativaCodigo)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <AmbienteCodigo> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.AmbienteCodigo).Count != 0)
                        {
                            Empresas.Configuracoes[emp].AmbienteCodigo = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.AmbienteCodigo)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <tpEmis> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.tpEmis).Count != 0)
                        {
                            Empresas.Configuracoes[emp].tpEmis = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.tpEmis)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaValidar> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaValidar).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaValidar = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaValidar)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaDownloadNFeDest> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaDownloadNFeDest).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaDownloadNFeDest = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaDownloadNFeDest)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaValidar> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarRetornoTXTNFe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].GravarRetornoTXTNFe = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarRetornoTXTNFe)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarEventosDeTerceiros).Count > 0)
                        {
                            Empresas.Configuracoes[emp].GravarEventosDeTerceiros = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarEventosDeTerceiros)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarEventosNaPastaEnviadosNFe).Count > 0)
                        {
                            Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarEventosNaPastaEnviadosNFe)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe).Count > 0)
                        {
                            Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.IndSinc).Count > 0)
                        {
                            Empresas.Configuracoes[emp].IndSinc = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.IndSinc)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <DiretorioSalvarComo> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.DiretorioSalvarComo).Count != 0)
                        {
                            Empresas.Configuracoes[emp].DiretorioSalvarComo = Convert.ToString(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.DiretorioSalvarComo)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <DiasLimpeza> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.DiasLimpeza).Count != 0)
                        {
                            Empresas.Configuracoes[emp].DiasLimpeza = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.DiasLimpeza)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaExeUniDanfe> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaExeUniDanfe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaExeUniDanfe = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaExeUniDanfe)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <ConfiguracaoDanfe> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.ConfiguracaoDanfe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].ConfiguracaoDanfe = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.ConfiguracaoDanfe)[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <ConfiguracaoCCe> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.ConfiguracaoCCe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].ConfiguracaoCCe = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.ConfiguracaoCCe)[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaConfigUniDanfe> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaConfigUniDanfe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaConfigUniDanfe = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaConfigUniDanfe)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <PastaDanfeMon> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaDanfeMon).Count != 0)
                        {
                            Empresas.Configuracoes[emp].PastaDanfeMon = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.PastaDanfeMon)[0].InnerText, true);
                            lEncontrouTag = true;
                        }
                        //Se a tag <XMLDanfeMonNFe> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.XMLDanfeMonNFe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].XMLDanfeMonNFe = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.XMLDanfeMonNFe)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <XMLDanfeMonProcNFe> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.XMLDanfeMonProcNFe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].XMLDanfeMonProcNFe = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.XMLDanfeMonProcNFe)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <XMLDanfeMonDenegadaNFe> existir ele pega no novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.XMLDanfeMonDenegadaNFe).Count != 0)
                        {
                            Empresas.Configuracoes[emp].XMLDanfeMonDenegadaNFe = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.XMLDanfeMonDenegadaNFe)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        //Se a tag <TempoConsulta> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.TempoConsulta).Count != 0)
                        {
                            Empresas.Configuracoes[emp].TempoConsulta = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.TempoConsulta)[0].InnerText);
                            lEncontrouTag = true;
                        }

                        #region -- FTP
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPAtivo).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPAtivo = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPAtivo)[0].InnerText);
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPGravaXMLPastaUnica).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPGravaXMLPastaUnica = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPGravaXMLPastaUnica)[0].InnerText.Trim());
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPSenha).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPSenha = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPSenha)[0].InnerText.Trim();
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPPastaAutorizados).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPPastaAutorizados = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPPastaAutorizados)[0].InnerText.Trim();
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPPastaRetornos).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPPastaRetornos = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPPastaRetornos)[0].InnerText.Trim();
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPNomeDoUsuario).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPNomeDoUsuario = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPNomeDoUsuario)[0].InnerText.Trim();
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPNomeDoServidor).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPNomeDoServidor = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPNomeDoServidor)[0].InnerText.Trim();
                            lEncontrouTag = true;
                        }
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPPorta).Count != 0)
                        {
                            Empresas.Configuracoes[emp].FTPPorta = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.FTPPorta)[0].InnerText.Trim());
                            lEncontrouTag = true;
                        }
                        #endregion

                        #region --Certificado Digital

                        //Se a tag <CertificadoInstalado> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoInstalado).Count != 0)
                        {
                            Empresas.Configuracoes[emp].CertificadoInstalado = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoInstalado)[0].InnerText);
                            lEncontrouTag = true;
                        }

                        //Se a tag <CertificadoArquivo> existir ele pega o novo conteúdo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoArquivo).Count != 0)
                        {
                            Empresas.Configuracoes[emp].CertificadoArquivo = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoArquivo)[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <CertificadoSenha> existir ele pega o novo conteudo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoSenha).Count != 0)
                        {
                            Empresas.Configuracoes[emp].CertificadoSenha = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoSenha)[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <certificado> existir ele pega o novo conteudo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoDigital).Count != 0)
                        {
                            Empresas.Configuracoes[emp].Certificado = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoDigital)[0].InnerText;
                            lEncontrouTag = true;
                        }
                        //Se a tag <CertificadoDigitalThumbPrint> existir ele pega o novo conteudo
                        if (ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoDigitalThumbPrint).Count != 0)
                        {
                            Empresas.Configuracoes[emp].CertificadoDigitalThumbPrint = ConfUniNfeElemento.GetElementsByTagName(NFeStrConstants.CertificadoDigitalThumbPrint)[0].InnerText;
                            lEncontrouTag = true;
                        }

                        #endregion

#endif
                        #endregion
                    }
                    #endregion
                }
                if (lEncontrouTag)
                {
                    if (ConfiguracaoApp.Proxy &&
                        (ConfiguracaoApp.ProxyPorta == 0 ||
                        string.IsNullOrEmpty(ConfiguracaoApp.ProxyServidor) ||
                        string.IsNullOrEmpty(ConfiguracaoApp.ProxyUsuario) ||
                        string.IsNullOrEmpty(ConfiguracaoApp.ProxySenha)))
                    {
                        throw new Exception(NFeStrConstants.proxyError);
                    }
                    Empresas.Configuracoes[emp].RemoveEndSlash();
                }
            }
            catch (Exception ex)
            {
                cStat = "2";
                xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + Propriedade.NomeAplicacao + ": " + ex.Message;
                lErro = true;
            }

            if (lEncontrouTag)
            {
                if (!lErro)
                {
                    try
                    {
                        Empresas.CriarPasta();

                        ///
                        /// salva a configuracao da empresa
                        /// 
                        bool versaoNova = true;
                        if (File.Exists(Empresas.Configuracoes[emp].NomeArquivoConfig))
                        {
                            versaoNova = File.ReadAllText(Empresas.Configuracoes[emp].NomeArquivoConfig).Contains("<Empresa xmlns:xsi");
                        }

                        if (versaoNova)
                        {
                            //Na reconfiguração enviada pelo ERP, não vou validar o certificado, vou deixar gravar mesmo que o certificado esteja com problema. Wandrey 05/10/2012
                            Empresas.Configuracoes[emp].SalvarConfiguracao(false);

                            ///
                            /// salva o arquivo da lista de empresas
                            this.GravarArqEmpresas();

                            ///
                            /// salva as configuracoes gerais
                            this.GravarConfigGeral();
                        }
                        else
                            //Na reconfiguração enviada pelo ERP, não vou validar o certificado, vou deixar gravar mesmo que o certificado esteja com problema. Wandrey 05/10/2012
                            this.GravarConfig(true, false);

                        cStat = "1";
                        xMotivo = "Configuracao do " + Propriedade.NomeAplicacao + " alterada com sucesso";
                        lErro = false;
                    }
                    catch (Exception ex)
                    {
                        cStat = "2";
                        xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + Propriedade.NomeAplicacao + ": " + ex.Message;
                        lErro = true;
                    }
                }
            }
            else
            {
                cStat = "2";
                xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + Propriedade.NomeAplicacao + ": Nenhuma tag padrão de configuração foi localizada no XML";
                lErro = true;
            }

            //Se deu algum erro tenho que voltar as configurações como eram antes, ou seja
            //exatamente como estavam gravadas no XML de configuração
            if (lErro)
            {
                ConfiguracaoApp.CarregarDados();
                ConfiguracaoApp.CarregarDadosSobre();
                Empresas.CarregaConfiguracao();

                #region Ticket: #110
                Empresas.CreateLockFile(true);
                #endregion
            }

            //Gravar o XML de retorno com a informação do sucesso ou não na reconfiguração
            FileInfo arqInfo = new FileInfo(cArquivoXml);
            string pastaRetorno = string.Empty;
            if (arqInfo.DirectoryName.ToLower().Trim() == Propriedade.PastaGeralTemporaria.ToLower().Trim())
            {
                pastaRetorno = Propriedade.PastaGeralRetorno;
            }
            else
            {
                pastaRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno;
            }

            string nomeArqRetorno;
            if (Path.GetExtension(cArquivoXml).ToLower() == ".txt")
                nomeArqRetorno = Functions.ExtrairNomeArq(cArquivoXml, Propriedade.ExtEnvio.AltCon_TXT) + "-ret-alt-con.txt";
            else
                nomeArqRetorno = Functions.ExtrairNomeArq(cArquivoXml, Propriedade.ExtEnvio.AltCon_XML) + "-ret-alt-con.xml";

            string cArqRetorno = pastaRetorno + "\\" + nomeArqRetorno;

            try
            {
                FileInfo oArqRetorno = new FileInfo(cArqRetorno);
                if (oArqRetorno.Exists == true)
                {
                    oArqRetorno.Delete();
                }

                if (Path.GetExtension(cArquivoXml).ToLower() == ".txt")
                {
                    File.WriteAllText(cArqRetorno, "cStat|" + cStat + "\r\nxMotivo|" + xMotivo + "\r\n", Encoding.Default);
                }
                else
                {
                    var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                            new XElement("retAltConfUniNFe",
                                                new XElement("cStat", cStat),
                                                new XElement("xMotivo", xMotivo)));
                    xml.Save(cArqRetorno);
#if false
                    XmlWriterSettings oSettings = new XmlWriterSettings();
                    UTF8Encoding c = new UTF8Encoding(true);

                    oSettings.Encoding = c;
                    oSettings.Indent = true;
                    oSettings.IndentChars = "";
                    oSettings.NewLineOnAttributes = false;
                    oSettings.OmitXmlDeclaration = false;

                    XmlWriter oXmlGravar = XmlWriter.Create(cArqRetorno, oSettings);

                    oXmlGravar.WriteStartDocument();
                    oXmlGravar.WriteStartElement("retAltConfUniNFe");
                    oXmlGravar.WriteElementString("cStat", cStat);
                    oXmlGravar.WriteElementString("xMotivo", xMotivo);
                    oXmlGravar.WriteEndElement(); //retAltConfUniNFe
                    oXmlGravar.WriteEndDocument();
                    oXmlGravar.Flush();
                    oXmlGravar.Close();
#endif
                }
            }
            catch (Exception ex)
            {
                //Ocorreu erro na hora de gerar o arquivo de erro para o ERP
                ///
                /// danasa 8-2009
                /// 
                Auxiliar oAux = new Auxiliar();
                oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(cArqRetorno) + ".err", xMotivo + Environment.NewLine + ex.Message);
            }

            try
            {
                //Deletar o arquivo de configurações automáticas gerado pelo ERP
                FileInfo oArqReconf = new FileInfo(cArquivoXml);
                oArqReconf.Delete();
            }
            catch
            {
                //Não vou fazer nada, so trato a exceção para evitar fechar o aplicativo. Wandrey 09/03/2010
            }
        }
        #endregion

        #region RemoveEndSlash
        public static string RemoveEndSlash(string value)
        {
            return RemoveEndSlash(value, false);
        }
        #endregion

        #region RemoveEndSlash
        /// <summary>
        /// danasa 8-2009
        /// </summary>
        /// <param name="value"></param>
        /// <param name="acertarNomeCurto"></param>
        /// <returns></returns>
        public static string RemoveEndSlash(string value, bool acertarNomeCurto)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (acertarNomeCurto)
                {
                    DirectoryInfo dir = new DirectoryInfo(value);
                    value = dir.FullName;
                }
                value = value.TrimEnd('\\');
                //while (value.Substring(value.Length - 1, 1) == @"\" && !string.IsNullOrEmpty(value))
                    //value = value.Substring(0, value.Length - 1);
            }
            else
            {
                value = string.Empty;
            }

            return value.Replace("\r\n", "").Trim();
        }
        #endregion

        #region CadastrarEmpresa()
        private int CadastrarEmpresa(string arqXML)
        {
            string cnpj = "";
            string nomeEmp = "";
            string servico = "";

            if (Path.GetExtension(arqXML).ToLower() == ".xml")
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arqXML);

                const string DadosEmpresa = "DadosEmpresa";

                if (doc.DocumentElement.SelectNodes(DadosEmpresa).Count > 0)
                {
                    foreach (XmlElement item in doc.DocumentElement)
                    {
                        cnpj = doc.DocumentElement.SelectNodes(DadosEmpresa)[0].Attributes[NFe.Components.NFeStrConstants.CNPJ].Value;
                        if (doc.DocumentElement.SelectNodes(DadosEmpresa)[0].Attributes[NFe.Components.NFeStrConstants.Servico] != null)
                            servico = doc.DocumentElement.SelectNodes(DadosEmpresa)[0].Attributes[NFe.Components.NFeStrConstants.Servico].Value;

                        if (item.GetElementsByTagName(NFe.Components.NFeStrConstants.Nome).Count != 0)
                        {
                            nomeEmp = item.GetElementsByTagName(NFe.Components.NFeStrConstants.Nome)[0].InnerText;
                        }
                    }
                }
            }
            else
            {
                List<string> cLinhas = Functions.LerArquivo(arqXML);

                foreach (string texto in cLinhas)
                {
                    string[] dados = texto.Split('|');
                    int nElementos = dados.GetLength(0);
                    if (nElementos <= 1)
                        continue;

                    switch (dados[0].ToLower())
                    {
                        case "nome":
                            nomeEmp = dados[1];
                            break;
                        case "cnpj":
                            cnpj = dados[1];
                            break;
                        case "servico":
                            servico = dados[1];
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(cnpj) || string.IsNullOrEmpty(nomeEmp) || string.IsNullOrEmpty(servico))
            {
                throw new Exception("Não foi possível localizar os dados da empresa no arquivo de configuração. (CNPJ/Nome ou Tipo de Serviço)");
            }

            if (Char.IsLetter(servico, 0))
                ///
                /// veio como 'NFe, NFCe, CTe, MDFe ou NFSe
                /// converte para numero correspondente
                servico = ((int)NFe.Components.EnumHelper.StringToEnum<TipoAplicativo>(servico)).ToString();

            if (Empresas.FindConfEmpresa(cnpj.Trim(), (TipoAplicativo)Convert.ToInt16(servico)) == null)
            {
                Empresa empresa = new Empresa();
                empresa.CNPJ = cnpj;
                empresa.Nome = nomeEmp;
                empresa.Servico = (TipoAplicativo)Convert.ToInt16(servico);
                Empresas.Configuracoes.Add(empresa);

                //GravarArqEmpresas();  //tirado daqui pq ele somente grava quando a empresa for gravada com sucesso
            }
            return Empresas.FindConfEmpresaIndex(cnpj, (TipoAplicativo)Convert.ToInt16(servico));
        }
        #endregion

        #region CertificadosInstalados()
        public void CertificadosInstalados(string arquivo)
        {
            bool lConsultar = false;
            bool lErro = false;
            string arqRet = "uninfe-ret-cons-certificado.xml";
            string tmp_arqRet = Path.Combine(Propriedade.PastaGeralTemporaria, arqRet);
            string cStat = "";
            string xMotivo = "";

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arquivo);

                foreach (XmlElement item in doc.DocumentElement)
                {
                    lConsultar = doc.DocumentElement.GetElementsByTagName("xServ")[0].InnerText.Equals("CONS-CERTIFICADO", StringComparison.InvariantCultureIgnoreCase);
                }

                if (lConsultar)
                {
                    X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                    X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                    X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

                    #region Cria XML de retorno
                    if (File.Exists(tmp_arqRet))
                        File.Delete(tmp_arqRet);

                    XmlDocument RetCertificados = new XmlDocument();

                    XmlNode raiz = RetCertificados.CreateElement("Certificados");
                    RetCertificados.AppendChild(raiz);

                    RetCertificados.Save(tmp_arqRet);

                    #endregion

                    #region Monta XML de Retorno com dados do Certificados Instalados
                    for (int i = 0; i < collection2.Count; i++)
                    {
                        #region layout retorno
                        /*layout de retorno - Renan Borges
                        <Certificados> 
                        <ThumbPrint ID="999..."> 
                        <Subject>XX...</Subject> 
                        <ValidadeInicial>dd/dd/dddd</ValidadeInicial> 
                        <ValidadeFinal>dd/dd/dddd</ValidadeFinal> 
                        </Certificados>
                        */
                        #endregion

                        XmlDocument docGerar = new XmlDocument();
                        docGerar.Load(tmp_arqRet);

                        XmlNode Registro = docGerar.CreateElement("ThumbPrint");
                        XmlAttribute IdThumbPrint = docGerar.CreateAttribute(NFe.Components.NFeStrConstants.ID);
                        IdThumbPrint.Value = collection2[i].Thumbprint.ToString();
                        Registro.Attributes.Append(IdThumbPrint);

                        XmlNode Subject = docGerar.CreateElement("Subject");
                        XmlNode ValidadeInicial = docGerar.CreateElement("ValidadeInicial");
                        XmlNode ValidadeFinal = docGerar.CreateElement("ValidadeFinal");

                        Subject.InnerText = collection2[i].Subject.ToString();
                        ValidadeInicial.InnerText = collection2[i].NotBefore.ToShortDateString();
                        ValidadeFinal.InnerText = collection2[i].NotAfter.ToShortDateString();

                        docGerar.SelectSingleNode("Certificados").AppendChild(Registro);
                        Registro.AppendChild(Subject);
                        Registro.AppendChild(ValidadeInicial);
                        Registro.AppendChild(ValidadeFinal);

                        docGerar.Save(tmp_arqRet);

                    }
                    #endregion
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
                string cArqRetorno = Propriedade.PastaGeralRetorno + "\\" + arqRet;

                #region XML de Retorno para ERP
                try
                {
                    FileInfo oArqRetorno = new FileInfo(cArqRetorno);
                    if (oArqRetorno.Exists == true)
                    {
                        oArqRetorno.Delete();
                    }

                    if (!lConsultar && !lErro)
                    {
                        cStat = "2";
                        xMotivo = "Nao foi possivel fazer a consulta de Certificados Instalados na estação (xServ não identificado) no " + Propriedade.NomeAplicacao;
                    }

                    if (lErro || !lConsultar)
                    {
                        File.Delete(tmp_arqRet);

                        var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                                new XElement("retCadConfUniNFe",
                                                    new XElement("cStat", cStat),
                                                    new XElement("xMotivo", xMotivo)));
                        xml.Save(cArqRetorno);

#if false
                        XmlWriterSettings oSettings = new XmlWriterSettings();
                        UTF8Encoding c = new UTF8Encoding(true);

                        oSettings.Encoding = c;
                        oSettings.Indent = true;
                        oSettings.IndentChars = "";
                        oSettings.NewLineOnAttributes = false;
                        oSettings.OmitXmlDeclaration = false;

                        XmlWriter oXmlGravar = XmlWriter.Create(cArqRetorno, oSettings);

                        oXmlGravar.WriteStartDocument();
                        oXmlGravar.WriteStartElement("retCadConfUniNFe");
                        oXmlGravar.WriteElementString("cStat", cStat);
                        oXmlGravar.WriteElementString("xMotivo", xMotivo);
                        oXmlGravar.WriteEndElement(); //retAltConfUniNFe
                        oXmlGravar.WriteEndDocument();
                        oXmlGravar.Flush();
                        oXmlGravar.Close();
#endif
                    }
                    else
                    {
                        if (File.Exists(cArqRetorno))
                            File.Delete(cArqRetorno);

                        if (File.Exists(arquivo))
                            File.Delete(arquivo);

                        File.Move(tmp_arqRet, Propriedade.PastaGeralRetorno + "\\" + arqRet);
                    }
                }
                catch (Exception ex)
                {
                    //Ocorreu erro na hora de gerar o arquivo de erro para o ERP
                    Auxiliar oAux = new Auxiliar();
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(cArqRetorno) + ".err", xMotivo + Environment.NewLine + ex.Message);
                }
                #endregion
            }
        }
        #endregion

        #region ForceUpdateWSDL()
        public static bool ForceUpdateWSDL(bool pergunta = true)
        {
            if (!pergunta)
            {
                new loadResources().load();
                return true;
            }
            string XMLVersoesWSDL = Propriedade.PastaExecutavel + "\\VersoesWSDLs.xml";
            string msg = "Após confirmada esta função o UniNFe irá sobrepor todos os WSDLs e Schemas com as versões originais da Versão do UniNFe, sobrepondo assim possíveis arquivos que tenham sido atualizados manualmente.\r\n\r\nTem certeza que deseja continuar? ";

            if (MessageBox.Show(msg, "ATENÇÂO! - Atualização dos WSDLs e SCHEMAS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (File.Exists(XMLVersoesWSDL))
                {
                    File.Delete(XMLVersoesWSDL);
                }

                new loadResources().load();

                MessageBox.Show("WSDLs e Schemas atualizados com sucesso.", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            return true;
        }

        #endregion

        #endregion
    }

    #endregion
}


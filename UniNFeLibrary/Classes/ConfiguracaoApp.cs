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
using System.Security.Cryptography.X509Certificates;

namespace UniNFeLibrary
{
    #region Classe ConfiguracaoApp
    /// <summary>
    /// Classe responsável por realizar algumas tarefas na parte de configurações da aplicação.
    /// Arquivo de configurações: UniNfeConfig.xml
    /// </summary>
    public class ConfiguracaoApp
    {
        #region Propriedades
        public static string vPastaXMLEnvio { get; set; }
        public static string vPastaXMLRetorno { get; set; }
        public static string vPastaXMLEnviado { get; set; }
        public static string vPastaXMLErro { get; set; }
        public static int UFCod { get; set; }
        public static int tpAmb { get; set; }
        public static int tpEmis { get; set; }
        public static string vCertificado { get; set; }
        public static X509Certificate2 oCertificado { get; set; }
        public static string cNomeEmpresa { get; set; }
        public static string cPastaBackup { get; set; }
        public static string cPastaXMLEmLote { get; set; }
        /// <summary>
        /// Pasta onde é gravado os XML´s da NFE somente para validação
        /// </summary>
        public static string PastaValidar { get; set; }
        /// <summary>
        /// Gravar o retorno da NFe também em TXT
        /// </summary>
        public static bool GravarRetornoTXTNFe { get; set; }
        /// <summary>
        /// Recebe uma mensagem de erro caso venha a ocorrer na execução do método "GravarConfig()"
        /// </summary>
        public static string cErroGravarConfig { get; private set; }
        public static string VersaoXMLStatusServico { get; set; }
        public static string VersaoXMLNFe { get; set; }
        public static string VersaoXMLPedRec { get; set; }
        public static string VersaoXMLCanc { get; set; }
        public static string VersaoXMLInut { get; set; }
        public static string VersaoXMLPedSit { get; set; }
        public static string VersaoXMLConsCad { get; set; }
        public static string VersaoXMLCabecMsg { get; set; }
        public static string NomePastaXMLAssinado { get; private set; }
        /// <summary>
        /// Namespace URI associado (Endereço http dos schemas de XML)
        /// </summary>
        public static string nsURI { get; set; }
        #endregion

        #region Métodos gerais

        #region CarregarDados()
        /// <summary>
        /// Carrega as configurações realizadas na Aplicação gravadas no XML UniNfeConfig.xml para
        /// propriedades, para facilitar a leitura das informações necessárias para as transações da NF-e.
        /// </summary>
        /// <example>
        /// ConfigUniNFe oConfig = new ConfigUniNFe();
        /// oConfig.CarregarDados();
        /// oNfe.oCertificado = oConfig.oCertificado;
        /// oNfe.vUF = oConfig.vUnidadeFederativaCodigo;
        /// oNfe.vAmbiente = oConfig.vAmbienteCodigo;
        /// oNfe.vPastaXMLEnvio = oConfig.vPastaXMLEnvio;
        /// oNfe.vPastaXMLRetorno = oConfig.vPastaXMLRetorno;
        /// oNfe.vPastaXMLEnviado = oConfig.vPastaXMLEnviado;
        /// oNfe.vPastaXMLErro = oConfig.vPastaXMLErro;
        /// </example>
        public static void CarregarDados()
        {
            string vArquivoConfig = InfoApp.PastaExecutavel() + "\\UniNfeConfig.xml";
            ConfiguracaoApp.vPastaXMLEnvio = string.Empty;
            ConfiguracaoApp.cPastaXMLEmLote = string.Empty;
            ConfiguracaoApp.vPastaXMLRetorno = string.Empty;
            ConfiguracaoApp.vPastaXMLEnviado = string.Empty;
            ConfiguracaoApp.vPastaXMLErro = string.Empty;
            ConfiguracaoApp.UFCod = 0;
            ConfiguracaoApp.tpAmb = TipoAmbiente.taHomologacao;// 2;
            ConfiguracaoApp.tpEmis = TipoEmissao.teNormal;// 1;
            ConfiguracaoApp.vCertificado = string.Empty;
            ConfiguracaoApp.cNomeEmpresa = string.Empty;
            ConfiguracaoApp.cPastaBackup = string.Empty;
            ConfiguracaoApp.PastaValidar = string.Empty;
            ConfiguracaoApp.GravarRetornoTXTNFe = false;
            ConfiguracaoApp.NomePastaXMLAssinado = "\\Assinado";

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
                            if (oLerXml.Name == "nfe_configuracoes")
                            {
                                while (oLerXml.Read())
                                {
                                    if (oLerXml.NodeType == XmlNodeType.Element)
                                    {
                                        if (oLerXml.Name == "PastaXmlEnvio") { oLerXml.Read(); ConfiguracaoApp.vPastaXMLEnvio = ConfiguracaoApp.RemoveEndSlash(oLerXml.Value); }
                                        else if (oLerXml.Name == "PastaXmlRetorno") { oLerXml.Read(); ConfiguracaoApp.vPastaXMLRetorno = ConfiguracaoApp.RemoveEndSlash(oLerXml.Value); }
                                        else if (oLerXml.Name == "PastaXmlEnviado") { oLerXml.Read(); ConfiguracaoApp.vPastaXMLEnviado = ConfiguracaoApp.RemoveEndSlash(oLerXml.Value); }
                                        else if (oLerXml.Name == "PastaXmlErro") { oLerXml.Read(); ConfiguracaoApp.vPastaXMLErro = ConfiguracaoApp.RemoveEndSlash(oLerXml.Value); }
                                        else if (oLerXml.Name == "UnidadeFederativaCodigo") { oLerXml.Read(); ConfiguracaoApp.UFCod = Convert.ToInt32(oLerXml.Value); }
                                        else if (oLerXml.Name == "AmbienteCodigo") { oLerXml.Read(); ConfiguracaoApp.tpAmb = Convert.ToInt32(oLerXml.Value); }
                                        else if (oLerXml.Name == "CertificadoDigital") { oLerXml.Read(); ConfiguracaoApp.vCertificado = oLerXml.Value; }
                                        else if (oLerXml.Name == "tpEmis") { oLerXml.Read(); ConfiguracaoApp.tpEmis = Convert.ToInt32(oLerXml.Value); }
                                        else if (oLerXml.Name == "NomeEmpresa") { oLerXml.Read(); ConfiguracaoApp.cNomeEmpresa = oLerXml.Value; }
                                        else if (oLerXml.Name == "PastaBackup") { oLerXml.Read(); ConfiguracaoApp.cPastaBackup = ConfiguracaoApp.RemoveEndSlash(oLerXml.Value); }
                                        else if (oLerXml.Name == "PastaXmlEmLote") { oLerXml.Read(); ConfiguracaoApp.cPastaXMLEmLote = ConfiguracaoApp.RemoveEndSlash(oLerXml.Value); }
                                        else if (oLerXml.Name == "PastaValidar") { oLerXml.Read(); ConfiguracaoApp.PastaValidar = ConfiguracaoApp.RemoveEndSlash(oLerXml.Value); }
                                        else if (oLerXml.Name == "GravarRetornoTXTNFe") { oLerXml.Read(); ConfiguracaoApp.GravarRetornoTXTNFe = Convert.ToBoolean(oLerXml.Value); }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    //Ajustar o certificado digital de String para o tipo X509Certificate2
                    X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                    X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, ConfiguracaoApp.vCertificado, false);

                    if (collection1.Count == 0)
                    {
                    }
                    else
                    {
                        ConfiguracaoApp.oCertificado = collection1[0];
                    }

                    //Criar subpasta Assinado na pasta de envio individual de nfe
                    if (ConfiguracaoApp.vPastaXMLEnvio.Trim() != string.Empty)
                    {
                        if (!Directory.Exists(ConfiguracaoApp.vPastaXMLEnvio + InfoApp.NomePastaXMLAssinado))
                        {
                            System.IO.Directory.CreateDirectory(ConfiguracaoApp.vPastaXMLEnvio + InfoApp.NomePastaXMLAssinado);
                        }
                    }

                    //Criar subpasta Assinado na pasta de envio em lote de nfe
                    if (ConfiguracaoApp.cPastaXMLEmLote.Trim() != string.Empty)
                    {
                        if (!Directory.Exists(ConfiguracaoApp.cPastaXMLEmLote + InfoApp.NomePastaXMLAssinado))
                        {
                            System.IO.Directory.CreateDirectory(ConfiguracaoApp.cPastaXMLEmLote + InfoApp.NomePastaXMLAssinado);
                        }
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
                    if (Directory.Exists(ConfiguracaoApp.vPastaXMLRetorno))
                    {
                        string cArqErro = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                                            Path.GetFileNameWithoutExtension(vArquivoConfig) + ".err";
                        Auxiliar oAux = new Auxiliar();
                        oAux.GravarArqErroERP(cArqErro, ex.Message);
                    }
                    else
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

        #region GravarConfig()
        /// <summary>
        /// Método responsável por gravar as configurações da Aplicação no arquivo "UniNfeConfig.xml"
        /// </summary>
        /// <returns>Retorna true se conseguiu gravar corretamente as configurações ou false se não conseguiu</returns>
        public bool GravarConfig()
        {
            bool lValidou = this.ValidarConfig();
            if (lValidou == true)
            {
                try
                {
                    XmlWriterSettings oSettings = new XmlWriterSettings();
                    UTF8Encoding c = new UTF8Encoding(false);

                    //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
                    oSettings.Encoding = c;
                    oSettings.Indent = true;
                    oSettings.IndentChars = "";
                    oSettings.NewLineOnAttributes = false;
                    oSettings.OmitXmlDeclaration = false;

                    //Agora vamos criar um XML Writer
                    XmlWriter oXmlGravar = XmlWriter.Create(InfoApp.PastaExecutavel() + "\\UniNfeConfig.xml", oSettings);

                    //Agora vamos gravar os dados
                    oXmlGravar.WriteStartDocument();
                    oXmlGravar.WriteStartElement("nfe_configuracoes");
                    oXmlGravar.WriteElementString("PastaXmlEnvio", ConfiguracaoApp.vPastaXMLEnvio);
                    oXmlGravar.WriteElementString("PastaXmlRetorno", ConfiguracaoApp.vPastaXMLRetorno);
                    oXmlGravar.WriteElementString("PastaXmlEnviado", ConfiguracaoApp.vPastaXMLEnviado);
                    oXmlGravar.WriteElementString("PastaXmlErro", ConfiguracaoApp.vPastaXMLErro);
                    oXmlGravar.WriteElementString("UnidadeFederativaCodigo", ConfiguracaoApp.UFCod.ToString());
                    oXmlGravar.WriteElementString("AmbienteCodigo", ConfiguracaoApp.tpAmb.ToString());
                    oXmlGravar.WriteElementString("CertificadoDigital", ConfiguracaoApp.vCertificado);
                    oXmlGravar.WriteElementString("tpEmis", ConfiguracaoApp.tpEmis.ToString());
                    oXmlGravar.WriteElementString("NomeEmpresa", ConfiguracaoApp.cNomeEmpresa);
                    oXmlGravar.WriteElementString("PastaBackup", ConfiguracaoApp.cPastaBackup);
                    oXmlGravar.WriteElementString("PastaXmlEmLote", ConfiguracaoApp.cPastaXMLEmLote);
                    oXmlGravar.WriteElementString("PastaValidar", ConfiguracaoApp.PastaValidar);
                    oXmlGravar.WriteElementString("GravarRetornoTXTNFe", ConfiguracaoApp.GravarRetornoTXTNFe.ToString());
                    oXmlGravar.WriteEndElement(); //nfe_configuracoes
                    oXmlGravar.WriteEndDocument();
                    oXmlGravar.Flush();
                    oXmlGravar.Close();
                }
                catch (Exception ex)
                {
                    ConfiguracaoApp.cErroGravarConfig = ex.Message;
                }
            }

            return (lValidou);
        }
        #endregion

        #region ValidarConfig()
        /// <summary>
        /// Verifica se algumas das informações das configurações tem algum problema ou falha
        /// </summary>
        /// <returns>
        /// true - nenhum problema/falha
        /// false - encontrou algum problema
        /// </returns>
        private bool ValidarConfig()
        {
            bool lValidou = true;

            //Verifica se o nome da empresa ou alguma pasta de configuração está em branco
            if (ConfiguracaoApp.cNomeEmpresa == string.Empty)
            {
                ConfiguracaoApp.cErroGravarConfig = "Informe o nome da empresa.";
                lValidou = false;
            }
            else if (ConfiguracaoApp.vPastaXMLEnviado == string.Empty)
            {
                ConfiguracaoApp.cErroGravarConfig = "Informe a pasta para arquivamento dos arquivos XML enviados.";
                lValidou = false;
            }
            else if (ConfiguracaoApp.vPastaXMLEnvio == string.Empty)
            {
                ConfiguracaoApp.cErroGravarConfig = "Informe a pasta de envio dos arquivos XML.";
                lValidou = false;
            }
            else if (ConfiguracaoApp.vPastaXMLRetorno == string.Empty)
            {
                ConfiguracaoApp.cErroGravarConfig = "Informe a pasta de retorno dos arquivos XML.";
                lValidou = false;
            }
            else if (ConfiguracaoApp.vPastaXMLErro == string.Empty)
            {
                ConfiguracaoApp.cErroGravarConfig = "Informe a pasta para arquivamento temporário dos arquivos XML que apresentaram erros.";
                lValidou = false;
            }
            else if (ConfiguracaoApp.cPastaBackup == string.Empty)
            {
                ConfiguracaoApp.cErroGravarConfig = "Informe a pasta para o Backup dos XML enviados.";
                lValidou = false;
            }
            else if (ConfiguracaoApp.PastaValidar == string.Empty)
            {
                ConfiguracaoApp.cErroGravarConfig = "Informe a pasta onde será gravado os XML somente para ser validado pela Aplicação.";
                lValidou = false;
            }

            //Verificar se as pastas existem
            if (lValidou)
            {
                DirectoryInfo oDirEnvio = new DirectoryInfo(ConfiguracaoApp.vPastaXMLEnvio.Trim());
                DirectoryInfo oDirRetorno = new DirectoryInfo(ConfiguracaoApp.vPastaXMLRetorno.Trim());
                DirectoryInfo oDirEnviado = new DirectoryInfo(ConfiguracaoApp.vPastaXMLEnviado.Trim());
                DirectoryInfo oDirErro = new DirectoryInfo(ConfiguracaoApp.vPastaXMLErro.Trim());
                DirectoryInfo oDirBackup = new DirectoryInfo(ConfiguracaoApp.cPastaBackup.Trim());

                if (ConfiguracaoApp.vCertificado == string.Empty)
                {
                    ConfiguracaoApp.cErroGravarConfig = "Selecione o certificado digital a ser utilizado na autenticação dos serviços da nota fiscal eletrônica.";
                    lValidou = false;
                }
                else if (!oDirEnvio.Exists)
                {
                    ConfiguracaoApp.cErroGravarConfig = "A pasta de envio dos arquivos XML informada não existe.";
                    lValidou = false;
                }
                else if (!oDirRetorno.Exists)
                {
                    ConfiguracaoApp.cErroGravarConfig = "A pasta de retorno dos arquivos XML informada não existe.";
                    lValidou = false;
                }
                else if (!oDirEnviado.Exists)
                {
                    ConfiguracaoApp.cErroGravarConfig = "A pasta para arquivamento dos arquivos XML enviados informada não existe.";
                    lValidou = false;
                }
                else if (!oDirErro.Exists)
                {
                    ConfiguracaoApp.cErroGravarConfig = "A pasta para arquivamento temporário dos arquivos XML com erro informada não existe.";
                    lValidou = false;
                }
                else if (!oDirBackup.Exists)
                {
                    ConfiguracaoApp.cErroGravarConfig = "A pasta para backup dos XML enviados informada não existe.";
                    lValidou = false;
                }
                else if (ConfiguracaoApp.PastaValidar.Trim() != string.Empty)
                {
                    DirectoryInfo oDirValidar = new DirectoryInfo(ConfiguracaoApp.PastaValidar.Trim());

                    if (!oDirValidar.Exists)
                    {
                        ConfiguracaoApp.cErroGravarConfig = "A pasta para validação de XML´s informada não existe.";
                        lValidou = false;
                    }
                }
                else if (ConfiguracaoApp.cPastaXMLEmLote.Trim() != string.Empty)
                {
                    DirectoryInfo oDirLote = new DirectoryInfo(ConfiguracaoApp.cPastaXMLEmLote.Trim());

                    if (!oDirLote.Exists)
                    {
                        ConfiguracaoApp.cErroGravarConfig = "A pasta de envio das notas fiscais eletrônicas em lote informada não existe.";
                        lValidou = false;
                    }
                }
            }

            ///
            /// danasa 8-2009
            /// Wandrey verifique se ok - nos meus testes, foram...
            /// simples, mas funcionou
            /// 
            if (lValidou)
            {
                ///
                /// tirado o slash do final de cada pasta, 
                /// pois o usuário pode informar a mesma pasta, mas colocando um slash no final
                /// 
                /// C:\unimake\uninfe\envio
                /// C:\unimake\uninfe\envio\    <<<<<<<<<<<<<<<<<<< erro >>>>>>>>>>>>>>>>>
                /// unimake\uninfe\envio        <<<<<<<<<<<<<<<<<<< como proceder? >>>>>>>>>>>>>>>>>>
                /// 
                List<folderCompare> fc = new List<folderCompare>();
                fc.Add(new folderCompare(0, ConfiguracaoApp.RemoveEndSlash(ConfiguracaoApp.vPastaXMLEnviado)));
                fc.Add(new folderCompare(1, ConfiguracaoApp.RemoveEndSlash(ConfiguracaoApp.vPastaXMLEnvio)));
                fc.Add(new folderCompare(2, ConfiguracaoApp.RemoveEndSlash(ConfiguracaoApp.vPastaXMLErro)));
                fc.Add(new folderCompare(3, ConfiguracaoApp.RemoveEndSlash(ConfiguracaoApp.vPastaXMLRetorno)));
                fc.Add(new folderCompare(4, ConfiguracaoApp.RemoveEndSlash(ConfiguracaoApp.cPastaBackup)));
                fc.Add(new folderCompare(5, ConfiguracaoApp.RemoveEndSlash(ConfiguracaoApp.cPastaXMLEmLote)));
                fc.Add(new folderCompare(6, ConfiguracaoApp.RemoveEndSlash(ConfiguracaoApp.PastaValidar)));
                foreach (folderCompare fc1 in fc)
                {
                    if (string.IsNullOrEmpty(fc1.folder))
                        continue;

                    foreach (folderCompare fc2 in fc)
                    {
                        if (fc1.id != fc2.id)
                        {
                            if (fc1.folder.ToLower().Equals(fc2.folder.ToLower()))
                            {
                                ConfiguracaoApp.cErroGravarConfig = "Pasta idêntica não é possível.";
                                lValidou = false;
                                break;
                            }
                        }
                    }
                    if (!lValidou)
                        break;
                }
            }
            return lValidou;
        }
        #endregion

        #region ReconfigurarUniNFe()
        /// <summary>
        /// Método responsável por reconfigurar automaticamente o UniNFe a partir de um XML com as 
        /// informações necessárias.
        /// O Método grava um arquivo na pasta de retorno do UniNFe com a informação se foi bem 
        /// sucedida a reconfiguração ou não.
        /// </summary>
        /// <param name="cArquivoXml">Nome e pasta do arquivo de configurações gerado pelo ERP para atualização
        /// das configurações do uninfe</param>        
        /// 
        public void ReconfigurarUniNFe(string cArquivoXml)
        {
            string cStat = "";
            string xMotivo = "";
            bool lErro = false;
            bool lEncontrouTag = false;

            //Recarrega as configurações atuais
            ConfiguracaoApp.CarregarDados();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXml);

                XmlNodeList ConfUniNfeList = doc.GetElementsByTagName("altConfUniNFe");

                foreach (XmlNode ConfUniNfeNode in ConfUniNfeList)
                {
                    XmlElement ConfUniNfeElemento = (XmlElement)ConfUniNfeNode;

                    //Se a tag <PastaXmlEnvio> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnvio").Count != 0)
                    {
                        ConfiguracaoApp.vPastaXMLEnvio = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnvio")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaXmlEmLote> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlEmLote").Count != 0)
                    {
                        ConfiguracaoApp.cPastaXMLEmLote = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName("PastaXmlEmLote")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaXmlRetorno> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlRetorno").Count != 0)
                    {
                        ConfiguracaoApp.vPastaXMLRetorno = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName("PastaXmlRetorno")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaXmlEnviado> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnviado").Count != 0)
                    {
                        ConfiguracaoApp.vPastaXMLEnviado = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName("PastaXmlEnviado")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaXmlErro> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaXmlErro").Count != 0)
                    {
                        ConfiguracaoApp.vPastaXMLErro = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName("PastaXmlErro")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <UnidadeFederativaCodigo> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("UnidadeFederativaCodigo").Count != 0)
                    {
                        ConfiguracaoApp.UFCod = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName("UnidadeFederativaCodigo")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <AmbienteCodigo> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("AmbienteCodigo").Count != 0)
                    {
                        ConfiguracaoApp.tpAmb = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName("AmbienteCodigo")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <tpEmis> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        ConfiguracaoApp.tpEmis = Convert.ToInt32(ConfUniNfeElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <NomeEmpresa> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("NomeEmpresa").Count != 0)
                    {
                        ConfiguracaoApp.cNomeEmpresa = ConfUniNfeElemento.GetElementsByTagName("NomeEmpresa")[0].InnerText;
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaBackup> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaBackup").Count != 0)
                    {
                        ConfiguracaoApp.cPastaBackup = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName("PastaBackup")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaValidar> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("PastaValidar").Count != 0)
                    {
                        ConfiguracaoApp.PastaValidar = ConfiguracaoApp.RemoveEndSlash(ConfUniNfeElemento.GetElementsByTagName("PastaValidar")[0].InnerText);
                        lEncontrouTag = true;
                    }
                    //Se a tag <PastaValidar> existir ele pega no novo conteúdo
                    if (ConfUniNfeElemento.GetElementsByTagName("GravarRetornoTXTNFe").Count != 0)
                    {
                        ConfiguracaoApp.GravarRetornoTXTNFe = Convert.ToBoolean(ConfUniNfeElemento.GetElementsByTagName("GravarRetornoTXTNFe")[0].InnerText);
                        lEncontrouTag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                cStat = "2";
                xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + InfoApp.NomeAplicacao() + ": " + ex.Message;
                lErro = true;
            }

            if (lEncontrouTag == true)
            {
                if (lErro == false)
                {
                    if (this.ValidarConfig() == false || this.GravarConfig() == false)
                    {
                        cStat = "2";
                        xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + InfoApp.NomeAplicacao() + ": " + ConfiguracaoApp.cErroGravarConfig;
                        lErro = true;
                    }
                    else
                    {
                        cStat = "1";
                        xMotivo = "Configuracao do " + InfoApp.NomeAplicacao() + " alterada com sucesso";
                        lErro = false;
                    }
                }
            }
            else
            {
                cStat = "2";
                xMotivo = "Ocorreu uma falha ao tentar alterar a configuracao do " + InfoApp.NomeAplicacao() + ": Nenhuma tag padrão de configuração foi localizada no XML";
                lErro = true;
            }

            //Se deu algum erro tenho que voltar as configurações como eram antes, ou seja
            //exatamente como estavam gravadas no XML de configuração
            if (lErro == true)
            {
                ConfiguracaoApp.CarregarDados();
            }

            //Gravar o XML de retorno com a informação do sucesso ou não na reconfiguração
            string cArqRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\uninfe-ret-alt-con.xml";
            try
            {
                FileInfo oArqRetorno = new FileInfo(cArqRetorno);
                if (oArqRetorno.Exists == true)
                {
                    oArqRetorno.Delete();
                }

                XmlWriterSettings oSettings = new XmlWriterSettings();
                UTF8Encoding c = new UTF8Encoding(false);

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
            }
            catch (Exception ex)
            {
                //Ocorreu erro na hora de gerar o arquivo de erro para o ERP
                ///
                /// danasa 8-2009
                /// 
                string cArqErro = ConfiguracaoApp.vPastaXMLRetorno + "\\" +
                                    Path.GetFileNameWithoutExtension(cArqRetorno) + ".err";
                Auxiliar oAux = new Auxiliar();
                oAux.GravarArqErroERP(cArqErro, xMotivo + Environment.NewLine + ex.Message);
            }
            //Deletar o arquivo de configurações automáticas gerado pelo ERP
            FileInfo oArqReconf = new FileInfo(cArquivoXml);
            oArqReconf.Delete();
        }
        #endregion

        #endregion

        /// <summary>
        /// danasa 8-2009
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string RemoveEndSlash(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                while (value.Substring(value.Length - 1, 1) == @"\" && !string.IsNullOrEmpty(value))
                    value = value.Substring(0, value.Length - 1);
            }
            return value;
        }
    }
    #endregion

    /// <summary>
    /// danasa 8-2009
    /// classe interna para comparar as pastas informadas
    /// </summary>
    internal class folderCompare
    {
        public Int32 id { get; set; }
        public string folder { get; set; }

        public folderCompare(Int32 _id, string _folder)
        {
            this.id = _id;
            this.folder = _folder;
        }
    }
}


﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;
using System.Threading;
using NFe.Components;

namespace NFe.Settings
{
    /// <summary>
    /// Classe contém os dados da empresa e suas configurações
    /// </summary>
    /// <remarks>
    /// Autor: Wandrey Mundin Ferreira
    /// Data: 28/07/2010
    /// </remarks>
    public class Empresa
    {
        #region Propriedades

        #region Propriedades das pastas configuradas para utilização pelo UniNFe
        /// <summary>
        /// Pasta onde deve ser gravado os XML´s a serem enviados
        /// </summary>
        public string PastaEnvio { get; set; }
        /// <summary>
        /// Pasta onde será gravado os XML´s de retorno para o ERP
        /// </summary>
        public string PastaRetorno { get; set; }
        /// <summary>
        /// Pasta onde será gravado os XML´s enviados
        /// </summary>
        public string PastaEnviado { get; set; }
        /// <summary>
        /// Pasta onde será gravado os XML´s que apresentaram algum tipo de erro em sua estrutura
        /// </summary>
        public string PastaErro { get; set; }
        /// <summary>
        /// Pasta onde será gravado como forma de backup os XML´s enviados
        /// </summary>
        public string PastaBackup { get; set; }
        /// <summary>
        /// Pasta onde deve ser gravado os XML´s de notas fiscais a serem enviadas em lote
        /// </summary>
        public string PastaEnvioEmLote { get; set; }
        /// <summary>
        /// Pasta onde é gravado os XML´s da NFE somente para validação
        /// </summary>
        public string PastaValidar { get; set; }
        /// <summary>
        /// Pasta para onde será gravado os XML´s de NFe para o DANFEMon fazer a impressão do DANFe
        /// </summary>
        public string PastaDanfeMon { get; set; }

        public string FTPPastaAutorizados { get; set; }
        public string FTPPastaRetornos { get; set; }
        public string FTPNomeDoServidor { get; set; }
        public string FTPNomeDoUsuario { get; set; }
        public string FTPSenha { get; set; }
        public Int32 FTPPorta { get; set; }
        public bool FTPAtivo { get; set; }
        public bool FTPGravaXMLPastaUnica { get; set; }
        public bool FTPIsAlive
        {
            get
            {
                return this.FTPAtivo &&
                    !string.IsNullOrEmpty(this.FTPNomeDoServidor) &&
                    !string.IsNullOrEmpty(this.FTPNomeDoUsuario) &&
                    !string.IsNullOrEmpty(this.FTPSenha);
            }
        }
        #endregion

        #region Propriedades diversas
        /// <summary>
        /// CNPJ da Empresa
        /// </summary>
        public string CNPJ { get; set; }
        /// <summary>
        /// Nome da Empresa
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Código da unidade Federativa da Empresa
        /// </summary>
        public int UFCod { get; set; }
        /// <summary>
        /// Ambiente a ser utilizado para a emissão da nota fiscal eletrônica
        /// </summary>
        public int tpAmb { get; set; }
        /// <summary>
        /// Tipo de emissão a ser utilizado para a emissão da nota fiscal eletrônica
        /// </summary>
        public int tpEmis { get; set; }
        /// <summary>
        /// Define a utilização do certficado instalado no windows ou através de arquivo
        /// </summary>
        public bool CertificadoInstalado { get; set; }
        /// <summary>
        /// Quando utilizar o certificado através de arquivo será necessário informar o local de armazenamento do certificado digital
        /// </summary>
        public string CertificadoArquivo { get; set; }
        /// <summary>
        /// Quando utilizar o certificado através de arquivo será necessário informar a senha do certificado
        /// </summary>
        public string CertificadoSenha { get; set; }
        /// <summary>
        /// Certificado digital - Subject
        /// </summary>
        public string Certificado { get; set; }
        /// <summary>
        /// Certificado digital - ThumbPrint
        /// </summary>
        public string CertificadoThumbPrint { get; set; }
        /// <summary>
        /// Certificado digital
        /// </summary>
        public X509Certificate2 X509Certificado { get; set; }
        /// <summary>
        /// Gravar o retorno da NFe também em TXT
        /// </summary>
        public bool GravarRetornoTXTNFe { get; set; }
        /// <summary>
        /// dias em que se deve manter os arquivos nas pastas retorno e temporario
        /// <para>coloque 0 para infinito</para>
        /// </summary>
        /// <by>http://desenvolvedores.net/marcelo</by>
        public int DiasLimpeza { get; set; }
        /// <summary>
        /// Tempo para execução da consulta do recibo após o envio do lote
        /// </summary>
        public int TempoConsulta { get; set; }

        /// <summary>
        /// Caminho das pastas com erro no caminho dos diretorios
        /// </summary>
        public static string ErroCaminhoDiretorio { get; set; }

        /// <summary>
        /// Propriedade para exibição  de mensagem de erro referente ao erro no caminho das pastas informadas
        /// </summary>
        public static bool ExisteErroDiretorio { get; set; }

        #endregion

        #region Propriedades da parte das configurações por empresa

        private List<Thread> threads;

        /// <summary>
        /// Nome da pasta onde é gravado as configurações e informações da Empresa
        /// </summary>
        public string PastaEmpresa { get; set; }
        /// <summary>
        /// Nome do arquivo XML das configurações da empresa
        /// </summary>
        public string NomeArquivoConfig { get; set; }

        public bool CriaPastasAutomaticamente { get; set; }
        #endregion

        #region Propriedades para controle da impressão do DANFE
        /// <summary>
        /// Pasta do executável do UniDanfe
        /// </summary>
        public string PastaExeUniDanfe { get; set; }
        /// <summary>
        /// Pasta do arquivo de configurações do UniDanfe (Tem que ser sem o \dados)
        /// </summary>
        public string PastaConfigUniDanfe { get; set; }
        /// <summary>
        /// Copiar o XML da NFe (-nfe.xml) para a pasta do danfemon? 
        /// </summary>
        public bool XMLDanfeMonNFe { get; set; }
        /// <summary>
        /// Copiar o XML de Distribuição da NFe (-procNfe.xml) para a pasta do danfemon?
        /// </summary>
        public bool XMLDanfeMonProcNFe { get; set; }
        /// <summary>
        /// Copiar o XML de denegacao da NFe (-procNfe.xml) para a pasta do danfemon?
        /// </summary>
        public bool XMLDanfeMonDenegadaNFe { get; set; }//danasa 11-4-2012
        #endregion

        #region Propriedade para controle do nome da pasta a serem salvos os XML´s enviados
        private DiretorioSalvarComo mDiretorioSalvarComo = "";
        /// <summary>
        /// Define como devem ser salvos os diretórios dentro do Uninfe.
        /// <para>por enqto apenas usa a data e os valores possíveis para definir são:</para>
        /// <para>    A para ANO</para>
        /// <para>    M para MES</para>
        /// <para>    D para DIA</para>
        /// <para>    pode se passar como desejar</para>
        /// <para>    Ex:</para>
        /// <para>        AMD   para criar a pasta como ..\Enviados\Autorizados\ANOMESDIA\nfe.xml</para>
        /// <para>        AM    para criar a pasta como ..\Enviados\Autorizados\ANOMES\nfe.xml</para>
        /// <para>        AD    para criar a pasta como ..\Enviados\Autorizados\ANODIA\nfe.xml</para>
        /// <para>        A\M\D para criar a pasta como ..\Enviados\Autorizados\ANO\MES\DIA\nfe.xml</para>
        /// <para>        podem ser criadas outras combinações, ficando a critério do usuário</para>
        /// </summary>
        /// <by>http://desenvolvedores.net/marcelo</by>
        public DiretorioSalvarComo DiretorioSalvarComo
        {
            get
            {
                if (string.IsNullOrEmpty(mDiretorioSalvarComo))
                    mDiretorioSalvarComo = "AM";//padrão

                return mDiretorioSalvarComo;
            }

            set { mDiretorioSalvarComo = value; }
        }
        #endregion

        #endregion

        #region Coleções
        /// <summary>
        /// Configurações por empresa
        /// </summary>
        public static List<Empresa> Configuracoes = new List<Empresa>();
        /// <summary>
        /// Objetos dos serviços da NFe
        /// </summary>
        public Dictionary<string, WebServiceProxy> WSProxy = new Dictionary<string, WebServiceProxy>();
        /// <summary>
        /// Lista das threads que estão sendo executadas e o Index da empresa da thread
        /// </summary>
        //public static Dictionary<Thread, int> threads = new Dictionary<Thread, int>();
        #endregion

        /// <summary>
        /// Empresa
        /// danasa 20-9-2010
        /// </summary>
        public Empresa()
        {
            this.CriaPastasAutomaticamente = false;
            this.DiasLimpeza = 0;
            this.TempoConsulta = 2;
            this.FTPAtivo = false;
            this.FTPPorta = 21;
            this.threads = new List<Thread>();
        }

        ~Empresa()
        {
            foreach (Thread thr in threads)
                if (thr.IsAlive)
                    thr.Abort();
        }

        #region Envia arquivos para o FTP

        /// <summary>
        /// Copia o arquivo para o FTP
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderName"></param>
        public void SendFileToFTP(string fileName, string folderName)
        {
            //verifique se o arquivo existe e se o FTP da empresa está configurado e ativo
            if (File.Exists(fileName) && this.FTPIsAlive)
            {
                Thread t = new Thread(new ThreadStart(delegate()
                {
                    string arqDestino = Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName));
                    //Copia o arquivo para a pasta temp
                    FileInfo oArquivo = new FileInfo(fileName);
                    oArquivo.CopyTo(arqDestino, true);

                    FTP ftp = new FTP(this.FTPNomeDoServidor, this.FTPPorta, this.FTPNomeDoUsuario, this.FTPSenha);
                    try
                    {
                        //conecta ao ftp
                        ftp.Connect();
                        //pega a pasta corrente no ftp
                        string vCorrente = ftp.GetWorkingDirectory();
                        //tenta mudar para a pasta de destino
                        if (!ftp.changeDir(folderName))
                            //como nao foi possivel mudar de pasta, a cria
                            ftp.makeDir(folderName);
                        //volta para a pasta corrente já que na "makeDir" a pasta se torna ativa na ultima pasta criada
                        ftp.ChangeDir(vCorrente);
                        //transfere o arquivo da pasta temp
                        ftp.OpenUpload(arqDestino, folderName + "/" + Path.GetFileName(fileName), false);
                        while (ftp.DoUpload() > 0)
                        {
                            //Thread.Sleep(1);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        if (ftp.IsConnected)
                            ftp.Disconnect();

                        //exclui o arquivo transferido da pasta temporaria
                        Functions.DeletarArquivo(arqDestino);
                    }
                    doneThread_FTP(Thread.CurrentThread);
                }));
                this.threads.Add(t);
                t.IsBackground = true;
                //t.Name = name;
                t.Start();
                t.Join();
            }
        }

        private void doneThread_FTP(Thread thread)
        {
            if (this.threads.Contains(thread))
                this.threads.Remove(thread);
        }

        #endregion

        #region CarregaConfiguracao()
        /// <summary>
        /// Carregar as configurações de todas as empresas na coleção "Configuracoes" 
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 29/07/2010
        /// </remarks>
        public static void CarregaConfiguracao()
        {
            Empresa.Configuracoes.Clear();

            if (File.Exists(Propriedade.NomeArqEmpresa))
            {
                FileStream arqXml = null;

                try
                {
                    arqXml = new FileStream(Propriedade.NomeArqEmpresa, FileMode.Open, FileAccess.Read, FileShare.Read); //Abrir um arquivo XML usando FileStream

                    var xml = new XmlDocument();
                    xml.Load(arqXml);

                    var empresaList = xml.GetElementsByTagName("Empresa");

                    foreach (XmlNode empresaNode in empresaList)
                    {
                        var empresaElemento = (XmlElement)empresaNode;

                        var registroList = xml.GetElementsByTagName("Registro");

                        for (int i = 0; i < registroList.Count; i++)
                        {
                            Empresa empresa = new Empresa();

                            var registroNode = registroList[i];
                            var registroElemento = (XmlElement)registroNode;

                            empresa.CNPJ = registroElemento.GetAttribute("CNPJ").Trim();
                            empresa.Nome = registroElemento.GetElementsByTagName("Nome")[0].InnerText.Trim();
                            empresa.PastaEmpresa = Propriedade.PastaExecutavel + "\\" + empresa.CNPJ.Trim();
                            empresa.NomeArquivoConfig = empresa.PastaEmpresa + "\\" + Propriedade.NomeArqConfig;

                            try
                            {
                                BuscaConfiguracao(empresa);
                            }
                            catch (Exception ex)
                            {
                                ///
                                /// nao acessar o metodo Auxiliar.GravarArqErroERP(string Arquivo, string Erro) já que nela tem a pesquisa da empresa
                                /// com base em "int emp = new FindEmpresaThread(Thread.CurrentThread).Index;" e neste ponto ainda não foi criada
                                /// as thread's
                                string cArqErro;
                                if (string.IsNullOrEmpty(empresa.PastaRetorno))
                                    cArqErro = Path.Combine(Propriedade.PastaExecutavel, string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddThhmmss")));
                                else
                                    cArqErro = Path.Combine(empresa.PastaRetorno, string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddThhmmss")));

                                try
                                {
                                    //Grava arquivo de ERRO para o ERP
                                    File.WriteAllText(cArqErro, ex.Message, Encoding.Default);
                                }
                                catch { }
                            }
                            ///
                            /// mesmo com erro, adicionar a lista para que o usuário possa altera-la
                            ChecaCaminhoDiretorio(empresa);
                            Configuracoes.Add(empresa);
                        }
                    }

                    arqXml.Close();
                    arqXml = null;



                    if (!ExisteErroDiretorio)
                        Empresa.CriarPasta();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (arqXml != null)
                        arqXml.Close();
                }
            }
        }
        #endregion

        #region BuscaConfiguracao()
        /// <summary>
        /// Busca as configurações da empresa dentro de sua pasta gravadas em um XML chamado UniNfeConfig.Xml
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 28/07/2010
        /// </remarks>
        private static void BuscaConfiguracao(Empresa empresa)
        {
            #region Criar diretório das configurações e dados da empresa
            if (!Directory.Exists(empresa.PastaEmpresa))
            {
                Directory.CreateDirectory(empresa.PastaEmpresa);
            }
            #endregion

            #region Limpar conteúdo dos atributos de configurações da empresa
            empresa.PastaEnvio = string.Empty;
            empresa.PastaRetorno = string.Empty;
            empresa.PastaEnviado = string.Empty;
            empresa.PastaErro = string.Empty;
            empresa.PastaBackup = string.Empty;
            empresa.PastaEnvioEmLote = string.Empty;
            empresa.PastaValidar = string.Empty;
            empresa.PastaDanfeMon = string.Empty;
            empresa.PastaExeUniDanfe = string.Empty;
            empresa.PastaConfigUniDanfe = string.Empty;
            empresa.Certificado = string.Empty;
            empresa.CertificadoThumbPrint = string.Empty;
            empresa.X509Certificado = null;
            empresa.FTPAtivo = false;
            empresa.FTPSenha =
                empresa.FTPNomeDoServidor =
                empresa.FTPNomeDoUsuario =
                empresa.FTPPastaRetornos =
                empresa.FTPPastaAutorizados = string.Empty;

            empresa.UFCod = 0;
            empresa.DiasLimpeza = 0;
            empresa.TempoConsulta = 2;

            empresa.tpAmb = Propriedade.TipoAmbiente.taHomologacao; //2
            empresa.tpEmis = Propriedade.TipoEmissao.teNormal; //1

            empresa.GravarRetornoTXTNFe =
                empresa.XMLDanfeMonNFe =
                empresa.XMLDanfeMonProcNFe =
                empresa.XMLDanfeMonDenegadaNFe = false;
            empresa.DiretorioSalvarComo = "AM";

            empresa.CertificadoInstalado = true;
            empresa.CertificadoArquivo = string.Empty;
            empresa.CertificadoSenha = string.Empty;
            #endregion

            #region Carregar as configurações do XML UniNFeConfig da Empresa
            FileStream arqXml = null;

            if (File.Exists(empresa.NomeArquivoConfig))
            {
                try
                {
                    arqXml = new FileStream(empresa.NomeArquivoConfig, FileMode.Open, FileAccess.Read, FileShare.Read); //Abrir um arquivo XML usando FileStrem
                    var xml = new XmlDocument();
                    xml.Load(arqXml);

                    var configList = xml.GetElementsByTagName("nfe_configuracoes");
                    foreach (XmlNode configNode in configList)
                    {
                        var configElemento = (XmlElement)configNode;

                        if (configElemento.GetElementsByTagName("PastaXmlEnvio")[0] != null)
                            empresa.PastaEnvio = configElemento.GetElementsByTagName("PastaXmlEnvio")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("PastaXmlRetorno")[0] != null)
                            empresa.PastaRetorno = configElemento.GetElementsByTagName("PastaXmlRetorno")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("PastaXmlEnviado")[0] != null)
                            empresa.PastaEnviado = configElemento.GetElementsByTagName("PastaXmlEnviado")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("PastaXmlErro")[0] != null)
                            empresa.PastaErro = configElemento.GetElementsByTagName("PastaXmlErro")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("UnidadeFederativaCodigo")[0] != null)
                            empresa.UFCod = Convert.ToInt32(configElemento.GetElementsByTagName("UnidadeFederativaCodigo")[0].InnerText);
                        if (configElemento.GetElementsByTagName("AmbienteCodigo")[0] != null)
                            empresa.tpAmb = Convert.ToInt32(configElemento.GetElementsByTagName("AmbienteCodigo")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("CertificadoDigital")[0] != null)
                            empresa.Certificado = configElemento.GetElementsByTagName("CertificadoDigital")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("tpEmis")[0] != null)
                            empresa.tpEmis = Convert.ToInt32(configElemento.GetElementsByTagName("tpEmis")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("PastaBackup")[0] != null)
                            empresa.PastaBackup = configElemento.GetElementsByTagName("PastaBackup")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("PastaXmlEmLote")[0] != null)
                            empresa.PastaEnvioEmLote = configElemento.GetElementsByTagName("PastaXmlEmLote")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("PastaValidar")[0] != null)
                            empresa.PastaValidar = configElemento.GetElementsByTagName("PastaValidar")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("GravarRetornoTXTNFe")[0] != null)
                            empresa.GravarRetornoTXTNFe = Convert.ToBoolean(configElemento.GetElementsByTagName("GravarRetornoTXTNFe")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("DiretorioSalvarComo")[0] != null)
                            empresa.DiretorioSalvarComo = configElemento.GetElementsByTagName("DiretorioSalvarComo")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("DiasLimpeza")[0] != null)
                            empresa.DiasLimpeza = Convert.ToInt32(configElemento.GetElementsByTagName("DiasLimpeza")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("PastaExeUniDanfe")[0] != null)
                            empresa.PastaExeUniDanfe = configElemento.GetElementsByTagName("PastaExeUniDanfe")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("PastaConfigUniDanfe")[0] != null)
                            empresa.PastaConfigUniDanfe = configElemento.GetElementsByTagName("PastaConfigUniDanfe")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("PastaDanfeMon")[0] != null)
                            empresa.PastaDanfeMon = configElemento.GetElementsByTagName("PastaDanfeMon")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("XMLDanfeMonNFe")[0] != null)
                            empresa.XMLDanfeMonNFe = Convert.ToBoolean(configElemento.GetElementsByTagName("XMLDanfeMonNFe")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("XMLDanfeMonProcNFe")[0] != null)
                            empresa.XMLDanfeMonProcNFe = Convert.ToBoolean(configElemento.GetElementsByTagName("XMLDanfeMonProcNFe")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("XMLDanfeMonDenegadaNFe")[0] != null)
                            empresa.XMLDanfeMonDenegadaNFe = Convert.ToBoolean(configElemento.GetElementsByTagName("XMLDanfeMonDenegadaNFe")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("TempoConsulta")[0] != null)
                            empresa.TempoConsulta = Convert.ToInt32(configElemento.GetElementsByTagName("TempoConsulta")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("FTPAtivo")[0] != null)
                            empresa.FTPAtivo = Convert.ToBoolean(configElemento.GetElementsByTagName("FTPAtivo")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("FTPGravaXMLPastaUnica")[0] != null)
                            empresa.FTPGravaXMLPastaUnica = Convert.ToBoolean(configElemento.GetElementsByTagName("FTPGravaXMLPastaUnica")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("FTPSenha")[0] != null)
                            empresa.FTPSenha = configElemento.GetElementsByTagName("FTPSenha")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("FTPPastaAutorizados")[0] != null)
                            empresa.FTPPastaAutorizados = configElemento.GetElementsByTagName("FTPPastaAutorizados")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("FTPPastaRetornos")[0] != null)
                            empresa.FTPPastaRetornos = configElemento.GetElementsByTagName("FTPPastaRetornos")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("FTPNomeDoUsuario")[0] != null)
                            empresa.FTPNomeDoUsuario = configElemento.GetElementsByTagName("FTPNomeDoUsuario")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("FTPNomeDoServidor")[0] != null)
                            empresa.FTPNomeDoServidor = configElemento.GetElementsByTagName("FTPNomeDoServidor")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("FTPPorta")[0] != null)
                            empresa.FTPPorta = Convert.ToInt32(configElemento.GetElementsByTagName("FTPPorta")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("CertificadoDigitalThumbPrint")[0] != null)
                            empresa.CertificadoThumbPrint = configElemento.GetElementsByTagName("CertificadoDigitalThumbPrint")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("CertificadoInstalado")[0] != null)
                            empresa.CertificadoInstalado = Convert.ToBoolean(configElemento.GetElementsByTagName("CertificadoInstalado")[0].InnerText.Trim());
                        if (configElemento.GetElementsByTagName("CertificadoArquivo")[0] != null)
                            empresa.CertificadoArquivo = configElemento.GetElementsByTagName("CertificadoArquivo")[0].InnerText.Trim();
                        if (configElemento.GetElementsByTagName("CertificadoSenha")[0] != null)
                            empresa.CertificadoSenha = Criptografia.descriptografaSenha(configElemento.GetElementsByTagName("CertificadoSenha")[0].InnerText.Trim());
                    }

                    empresa.X509Certificado = null;
                    if (empresa.CertificadoInstalado)
                    {
                        //Ajustar o certificado digital de String para o tipo X509Certificate2
                        X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                        store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                        X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                        X509Certificate2Collection collection1 = null;
                        if (empresa.CertificadoThumbPrint != string.Empty)
                            collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, empresa.CertificadoThumbPrint, false);
                        else
                            collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, empresa.Certificado, false);


                        for (int i = 0; i < collection1.Count; i++)
                        {
                            //Verificar a validade do certificado
                            if (DateTime.Compare(DateTime.Now, collection1[i].NotAfter) == -1)
                            {
                                empresa.X509Certificado = collection1[i];
                                break;
                            }
                        }

                        //Se não encontrou nenhum certificado com validade correta, vou pegar o primeiro certificado, porem vai travar na hora de tentar enviar a nota fiscal, por conta da validade. Wandrey 06/04/2011
                        if (empresa.X509Certificado == null)
                            empresa.X509Certificado = collection1[0];
                    }
                    else
                    {
                        //Leandro Souza - http://leonelfraga.com/neomatrixtech/?p=486
                        //Utilizar o certificado sem instalação
                        using (FileStream fs = new FileStream(empresa.CertificadoArquivo, FileMode.Open))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            empresa.X509Certificado = new X509Certificate2(buffer, empresa.CertificadoSenha);
                        }
                    }
                }
                catch (Exception ex)
                {
                    empresa.Certificado = string.Empty;
                    empresa.CertificadoThumbPrint = string.Empty;
                    throw new Exception("Ocorreu um erro ao efetuar a leitura das configurações da empresa " + empresa.Nome.Trim() + ". Por favor entre na tela de configurações desta empresa e reconfigure.\r\n\r\nErro: " + ex.Message);
                }
                finally
                {
                    if (arqXml != null)
                        arqXml.Close();
                }
            }
            #endregion
        }
        #endregion

        #region FindConfEmpresa()
        /// <summary>
        /// Procurar o cnpj na coleção das empresas
        /// </summary>
        /// <param name="cnpj">CNPJ a ser pesquisado</param>
        /// <returns>objeto empresa localizado, null se nada for localizado</returns>
        public static Empresa FindConfEmpresa(string cnpj)
        {
            Empresa retorna = null;
            foreach (Empresa empresa in Empresa.Configuracoes)
            {
                if (empresa.CNPJ.Equals(cnpj))
                {
                    retorna = empresa;
                    break;
                }
            }

            return retorna;
        }
        #endregion

        #region FindConfEmpresaIndex()
        /// <summary>
        /// Procurar o cnpj na coleção das empresas
        /// </summary>
        /// <param name="cnpj">CNPJ a ser pesquisado</param>
        /// <returns>Retorna o index do objeto localizado ou null se nada for localizado</returns>
        public static int FindConfEmpresaIndex(string cnpj)
        {
            int retorna = -1;

            for (int i = 0; i < Empresa.Configuracoes.Count; i++)
            {
                Empresa empresa = Empresa.Configuracoes[i];

                if (empresa.CNPJ.Equals(cnpj))
                {
                    retorna = i;
                    break;
                }
            }

            return retorna;
        }
        #endregion

        #region Valid()
        /// <summary>
        /// Retorna se o indice da coleção que foi pesquisado é valido ou não
        /// </summary>
        /// <param name="index">Indice a ser validado</param>
        /// <returns>Retorna true or false</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        public static bool Valid(int index)
        {
            bool retorna = true;
            if (index.Equals(-1))
                retorna = false;

            return retorna;
        }
        #endregion

        #region Valid()
        /// <summary>
        /// Retorna se o objeto da coleção que foi pesquisado é valido ou não
        /// </summary>
        /// <param name="empresa">Objeto da empresa</param>
        /// <returns>Retorna true or false</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        public static bool Valid(Empresa empresa)
        {
            bool retorna = true;
            if (empresa.Equals(null))
                retorna = false;

            return retorna;
        }
        #endregion

        #region ChecaCaminhoDiretorio
        /// <summary>
        /// Método para checagem dos caminhos e sua existencia ou não no pc
        /// </summary>
        /// <param name="empresa">Empresa a ser validado os caminhos das pastas</param>
        private static void ChecaCaminhoDiretorio(Empresa empresa)
        {
            FileStream arqXml = null;

            if (File.Exists(empresa.NomeArquivoConfig))
            {
                try
                {
                    arqXml = new FileStream(empresa.NomeArquivoConfig, FileMode.Open, FileAccess.Read, FileShare.Read); //Abrir um arquivo XML usando FileStrem
                    var xml = new XmlDocument();
                    xml.Load(arqXml);
                    var configList = xml.GetElementsByTagName("nfe_configuracoes");
                    foreach (XmlNode configNode in configList)
                    {
                        var configElemento = (XmlElement)configNode;

                        if (!Directory.Exists(configElemento.GetElementsByTagName("PastaXmlEnvio")[0].InnerText.Trim())
                            && configElemento.GetElementsByTagName("PastaXmlEnvio")[0].InnerText.Trim() != null)
                        {
                            Empresa.ExisteErroDiretorio = true;
                            ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + configElemento.GetElementsByTagName("PastaXmlEnvio")[0].InnerText.Trim() + "\r\n";
                        }
                        if (!Directory.Exists(configElemento.GetElementsByTagName("PastaXmlRetorno")[0].InnerText.Trim()) &&
                            configElemento.GetElementsByTagName("PastaXmlRetorno")[0].InnerText.Trim() != null)
                        {
                            Empresa.ExisteErroDiretorio = true;
                            ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + configElemento.GetElementsByTagName("PastaXmlRetorno")[0].InnerText.Trim() + "\r\n";
                        }

                        if (!Directory.Exists(configElemento.GetElementsByTagName("PastaXmlEnviado")[0].InnerText.Trim())
                            && configElemento.GetElementsByTagName("PastaXmlEnviado")[0].InnerText.Trim() != null)
                        {
                            Empresa.ExisteErroDiretorio = true;
                            ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + configElemento.GetElementsByTagName("PastaXmlEnviado")[0].InnerText.Trim() + "\r\n";
                        }

                        if (!Directory.Exists(configElemento.GetElementsByTagName("PastaXmlErro")[0].InnerText.Trim())
                            && configElemento.GetElementsByTagName("PastaXmlErro")[0].InnerText.Trim() != null)
                        {
                            Empresa.ExisteErroDiretorio = true;
                            ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + configElemento.GetElementsByTagName("PastaXmlErro")[0].InnerText.Trim() + "\r\n";
                        }

                        if (!Directory.Exists(configElemento.GetElementsByTagName("PastaBackup")[0].InnerText.Trim()) &&
                            configElemento.GetElementsByTagName("PastaBackup")[0].InnerText.Trim() != null)
                        {
                            Empresa.ExisteErroDiretorio = true;
                            ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + configElemento.GetElementsByTagName("PastaBackup")[0].InnerText.Trim() + "\r\n";
                        }

                        if (!Directory.Exists(configElemento.GetElementsByTagName("PastaXmlEmLote")[0].InnerText.Trim()) &&
                            !string.IsNullOrEmpty(configElemento.GetElementsByTagName("PastaXmlEmLote")[0].InnerText.Trim()))
                        {
                            Empresa.ExisteErroDiretorio = true;
                            ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + configElemento.GetElementsByTagName("PastaXmlEmLote")[0].InnerText.Trim() + "\r\n";
                        }

                        if (!Directory.Exists(configElemento.GetElementsByTagName("PastaValidar")[0].InnerText.Trim())
                            && configElemento.GetElementsByTagName("PastaValidar")[0].InnerText.Trim() != null)
                        {
                            Empresa.ExisteErroDiretorio = true;
                            ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + configElemento.GetElementsByTagName("PastaValidar")[0].InnerText.Trim() + "\r\n";
                        }
                    }
                }
                catch 
                {
                }
                finally
                {
                    if (arqXml != null)
                        arqXml.Close();
                }
            }
        }
        #endregion

        #region CriarPasta()
        /// <summary>
        /// Criar as pastas para todas as empresas cadastradas e configuradas no sistema se as mesmas não existirem
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/09/2009</date>
        private static void CriarPasta()
        {
            try
            {
                foreach (Empresa empresa in Empresa.Configuracoes)
                {
                    //Criar pasta de logs. Wandrey 03/08/2011
                    if (!Directory.Exists(Propriedade.PastaLog))
                    {
                        Directory.CreateDirectory(Propriedade.PastaLog);
                    }

                    //Criar pasta de envio
                    if (!string.IsNullOrEmpty(empresa.PastaEnvio))
                    {
                        if (!Directory.Exists(empresa.PastaEnvio))
                        {
                            Directory.CreateDirectory(empresa.PastaEnvio);
                        }

                        //Criar a pasta Temp dentro da pasta de envio. Wandrey 03/08/2011
                        if (!Directory.Exists(empresa.PastaEnvio.Trim() + "\\Temp"))
                        {
                            Directory.CreateDirectory(empresa.PastaEnvio.Trim() + "\\Temp");
                        }
                    }

                    //Criar pasta de Envio em Lote
                    if (!string.IsNullOrEmpty(empresa.PastaEnvioEmLote))
                    {
                        if (!Directory.Exists(empresa.PastaEnvioEmLote))
                        {
                            Directory.CreateDirectory(empresa.PastaEnvioEmLote);
                        }

                        //Criar a pasta Temp dentro da pasta de envio em lote. Wandrey 05/10/2011
                        if (!Directory.Exists(empresa.PastaEnvioEmLote.Trim() + "\\Temp"))
                        {
                            Directory.CreateDirectory(empresa.PastaEnvioEmLote.Trim() + "\\Temp");
                        }
                    }

                    //Criar pasta de Retorno
                    if (!string.IsNullOrEmpty(empresa.PastaRetorno))
                    {
                        if (!Directory.Exists(empresa.PastaRetorno))
                        {
                            Directory.CreateDirectory(empresa.PastaRetorno);
                        }
                    }

                    //Criar pasta Enviado
                    if (!string.IsNullOrEmpty(empresa.PastaEnviado))
                    {
                        if (!Directory.Exists(empresa.PastaEnviado))
                        {
                            Directory.CreateDirectory(empresa.PastaEnviado);
                        }
                    }

                    //Criar pasta de XML´s com erro
                    if (!string.IsNullOrEmpty(empresa.PastaErro))
                    {
                        if (!Directory.Exists(empresa.PastaErro))
                        {
                            Directory.CreateDirectory(empresa.PastaErro);
                        }
                    }

                    //Criar pasta de Backup
                    if (!string.IsNullOrEmpty(empresa.PastaBackup))
                    {
                        if (!Directory.Exists(empresa.PastaBackup))
                        {
                            Directory.CreateDirectory(empresa.PastaBackup);
                        }
                    }

                    //Criar pasta para somente validação de XML´s
                    if (!string.IsNullOrEmpty(empresa.PastaValidar))
                    {
                        if (!Directory.Exists(empresa.PastaValidar))
                        {
                            Directory.CreateDirectory(empresa.PastaValidar);
                        }

                        //Criar a pasta Temp dentro da pasta de envio em lote. Wandrey 05/10/2011
                        if (!Directory.Exists(empresa.PastaValidar.Trim() + "\\Temp"))
                        {
                            Directory.CreateDirectory(empresa.PastaValidar.Trim() + "\\Temp");
                        }
                    }

                    //Criar subpasta Assinado na pasta de envio individual de nfe
                    if (!string.IsNullOrEmpty(empresa.PastaEnvio))
                    {
                        if (!Directory.Exists(empresa.PastaEnvio + Propriedade.NomePastaXMLAssinado))
                        {
                            System.IO.Directory.CreateDirectory(empresa.PastaEnvio + Propriedade.NomePastaXMLAssinado);
                        }
                    }

                    //Criar subpasta Assinado na pasta de envio em lote de nfe
                    if (!string.IsNullOrEmpty(empresa.PastaEnvioEmLote))
                    {
                        if (!Directory.Exists(empresa.PastaEnvioEmLote + Propriedade.NomePastaXMLAssinado))
                        {
                            System.IO.Directory.CreateDirectory(empresa.PastaEnvioEmLote + Propriedade.NomePastaXMLAssinado);
                        }
                    }

                    //Criar pasta para monitoramento do DANFEMon e impressão do DANFE
                    if (!string.IsNullOrEmpty(empresa.PastaDanfeMon))
                    {
                        if (!Directory.Exists(empresa.PastaDanfeMon))
                        {
                            System.IO.Directory.CreateDirectory(empresa.PastaDanfeMon);
                        }
                    }
                }

                Empresa.CriarSubPastaEnviado();
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region CriarSubPastaEnviado()
        /// <summary>
        /// Criar as subpastas (Autorizados/Denegados/EmProcessamento) dentro da pasta dos XML´s enviados para todas as empresas cadastradas e configuradas
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Date: 20/04/2010
        /// </remarks>
        private static void CriarSubPastaEnviado()
        {
            try
            {
                for (int i = 0; i < Empresa.Configuracoes.Count; i++)
                {
                    Empresa.CriarSubPastaEnviado(i);
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region CriarSubPastaEnviado()
        /// <summary>
        /// Criar as subpastas (Autorizados/Denegados/EmProcessamento) dentro da pasta dos XML´s enviados para a empresa passada por parâmetro
        /// </summary>
        /// <param name="indexEmpresa">Index da Empresa a ser pesquisado na coleção de configurações das empresas cadastradas</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Date: 20/04/2010
        /// </remarks>
        public static void CriarSubPastaEnviado(int indexEmpresa)
        {
            Empresa empresa = Empresa.Configuracoes[indexEmpresa];

            try
            {
                if (!string.IsNullOrEmpty(empresa.PastaEnviado))
                {
                    //Criar a pasta EmProcessamento
                    if (!Directory.Exists(empresa.PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString()))
                    {
                        System.IO.Directory.CreateDirectory(empresa.PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString());
                    }

                    //Criar a Pasta Autorizado
                    if (!Directory.Exists(empresa.PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString()))
                    {
                        System.IO.Directory.CreateDirectory(empresa.PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString());
                    }

                    //Criar a Pasta Denegado
                    if (!Directory.Exists(empresa.PastaEnviado + "\\" + PastaEnviados.Denegados.ToString()))
                    {
                        System.IO.Directory.CreateDirectory(empresa.PastaEnviado + "\\" + PastaEnviados.Denegados.ToString());
                    }
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }

    public class FindEmpresaThread
    {
        public int Index = -1;

        public FindEmpresaThread(Thread currentThread)
        {
            //Index = Auxiliar.threads[currentThread];
            Index = Convert.ToInt32(currentThread.Name);
        }
    }
}
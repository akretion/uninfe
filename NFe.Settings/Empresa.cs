using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;
using System.Threading;
using System.Linq;
using System.Xml.Serialization;

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
    /// 
    //[System.Xml.Serialization.XmlRoot("nfe_configuracoes")]
    [Serializable]
    public class Empresa
    {
        #region Propriedades

        #region Propriedades das pastas configuradas para utilização pelo UniNFe
        #region Ticket: #110
        /* 
         * Marcelo
         * 03/06/2013
         */
        /// <summary>
        /// Pasta base onde todas as outras estão configuradas. 
        /// <para>Diretório Root</para>
        /// </summary>
        /// <remarks>Esta propriedade tem como base a pasta envio</remarks>
        public string PastaBase
        {
            get
            {
                string result = "";

                if (!String.IsNullOrEmpty(PastaXmlEnvio))
                {
                    string[] dirs = PastaXmlEnvio.Split('\\');

                    for (int i = 0; i < dirs.Length - 1; i++)
                    {
                        result += dirs[i] + "\\";
                    }
                    result = result.Substring(0, result.Length - 1);
                }
                return result;
            }
        }
        #endregion

        /// <summary>
        /// Pasta onde deve ser gravado os XML´s a serem enviados
        /// </summary>
        public string PastaXmlEnvio { get; set; }
        /// <summary>
        /// Pasta onde será gravado os XML´s de retorno para o ERP
        /// </summary>
        public string PastaXmlRetorno { get; set; }
        /// <summary>
        /// Pasta onde será gravado os XML´s enviados
        /// </summary>
        public string PastaXmlEnviado { get; set; }
        /// <summary>
        /// Pasta onde será gravado os XML´s que apresentaram algum tipo de erro em sua estrutura
        /// </summary>
        public string PastaXmlErro { get; set; }
        /// <summary>
        /// Pasta onde será gravado como forma de backup os XML´s enviados
        /// </summary>
        public string PastaBackup { get; set; }
        /// <summary>
        /// Pasta onde deve ser gravado os XML´s de notas fiscais a serem enviadas em lote
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string PastaXmlEmLote { get; set; }
        /// <summary>
        /// Pasta onde é gravado os XML´s da NFE somente para validação
        /// </summary>
        public string PastaValidar { get; set; }
        /// <summary>
        /// Pasta para onde será gravado os XML´s de NFe para o DANFEMon fazer a impressão do DANFe
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string PastaDanfeMon { get; set; }
        /// <summary>
        /// Pasta para onde será gravado os XML´s de download das NFe de destinatarios
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string PastaDownloadNFeDest { get; set; }

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
        public int UnidadeFederativaCodigo { get; set; }
        /// <summary>
        /// Ambiente a ser utilizado para a emissão da nota fiscal eletrônica
        /// </summary>
        public int AmbienteCodigo { get; set; }
        /// <summary>
        /// Tipo de emissão a ser utilizado para a emissão da nota fiscal eletrônica
        /// </summary>
        public int tpEmis { get; set; }
        /// <summary>
        /// Define a utilização do certficado instalado no windows ou através de arquivo
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)]
        public bool CertificadoInstalado { get; set; }
        /// <summary>
        /// Quando utilizar o certificado através de arquivo será necessário informar o local de armazenamento do certificado digital
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)]
        public string CertificadoArquivo { get; set; }
        /// <summary>
        /// Quando utilizar o certificado através de arquivo será necessário informar a senha do certificado
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)]
        public string CertificadoSenha { get; set; }
        /// <summary>
        /// Utilizado para certificados A3
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)]
        public string CertificadoPIN { get; set; }
        /// <summary>
        /// Certificado digital - Subject
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)]
        public string Certificado { get; set; }
        /// <summary>
        /// Certificado digital - ThumbPrint
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)]
        public string CertificadoDigitalThumbPrint { get; set; }
        /// <summary>
        /// Certificado digital
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)] 
        public X509Certificate2 X509Certificado { get; set; }
        /// <summary>
        /// Gravar o retorno da NFe também em TXT
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
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
        /// Usuário de acesso ao webservice (Utilizado pelo UniNFS-e para algumas prefeituras)
        /// </summary>
        public string UsuarioWS { get; set; }
        /// <summary>
        /// Senha de acesso ao webservice (Utilizado pelo UniNFS-e para algumas prefeituras)
        /// </summary>
        public string SenhaWS { get; set; }
        /// <summary>
        /// Serviço que está sendo monitorado na configuração da empresa
        /// </summary>
        public TipoAplicativo Servico { get; set; }
        #endregion

        #region Propriedades da parte das configurações por empresa

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private List<Thread> threads;

        /// <summary>
        /// Nome da pasta onde é gravado as configurações e informações da Empresa
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string PastaEmpresa 
        { 
            get
            {
                return Propriedade.PastaExecutavel + "\\" + 
                    this.CNPJ + 
                    (this.Servico == TipoAplicativo.Nfe ? "" : "\\" + this.Servico.ToString().ToLower());
            }
        }
        /// <summary>
        /// Nome do arquivo XML das configurações da empresa
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string NomeArquivoConfig 
        { 
            get 
            { 
                return Path.Combine(this.PastaEmpresa, Propriedade.NomeArqConfig);
            } 
        }

        public bool CriaPastasAutomaticamente { get; set; }

        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool GravarEventosNaPastaEnviadosNFe { get; set; }
        
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool GravarEventosCancelamentoNaPastaEnviadosNFe { get; set; }
        
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool GravarEventosDeTerceiros { get; set; }
        
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool CompactarNfe { get; set; }

        /// <summary>
        /// Enviar NFe utilizando o processo síncrono (true or false)
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool IndSinc { get; set; }
        #endregion

        #region Propriedades para controle da impressão do DANFE
        /// <summary>
        /// Pasta do executável do UniDanfe
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string PastaExeUniDanfe { get; set; }
        /// <summary>
        /// Pasta do arquivo de configurações do UniDanfe (Tem que ser sem o \dados)
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string PastaConfigUniDanfe { get; set; }
        /// <summary>
        /// Nome da configuracao da empresa no UniDANFE
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string ConfiguracaoDanfe { get; set; }
        /// <summary>
        /// Nome da configuracao da empresa no UniDANFE de CCe
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string ConfiguracaoCCe { get; set; }
        /// <summary>
        /// Copiar o XML da NFe (-nfe.xml) para a pasta do danfemon? 
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool XMLDanfeMonNFe { get; set; }
        /// <summary>
        /// Copiar o XML de Distribuição da NFe (-procNfe.xml) para a pasta do danfemon?
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool XMLDanfeMonProcNFe { get; set; }
        /// <summary>
        /// Copiar o XML de denegacao da NFe (-procNfe.xml) para a pasta do danfemon?
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool XMLDanfeMonDenegadaNFe { get; set; }
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string EmailDanfe { get; set; }
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool AdicionaEmailDanfe { get; set; }
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
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nulo)] 
        public string diretorioSalvarComo
        {
            get { return DiretorioSalvarComo.ToString(); }
            set { DiretorioSalvarComo = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
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

        #region Propriedades para controle de FTP (posicionado aqui só para quando for gravar as informacoes a tag FTP seja a ultima da lista)
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

        #endregion

        #region Coleções
        /// <summary>
        /// Objetos dos serviços da NFe
        /// </summary>
        /// 
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public Dictionary<string, WebServiceProxy> WSProxy = new Dictionary<string, WebServiceProxy>();
        #endregion

        /// <summary>
        /// Empresa
        /// danasa 20-9-2010
        /// </summary>
        public Empresa()
        {
            LimparPropriedades(this);

            this.threads = new List<Thread>();
        }

        ~Empresa()
        {
            foreach (Thread thr in threads)
                if (thr.IsAlive)
                    thr.Abort();
        }

        #region CarregaConfiguracao()
        /// <summary>
        /// Carregar as configurações de todas as empresas na coleção "Configuracoes" 
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 29/07/2010
        /// </remarks>
        /// 
#if false
        public static void __CarregaConfiguracao()
        {
            Empresas.Configuracoes.Clear();

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

                        var registroList = xml.GetElementsByTagName(NFe.Components.NFeStrConstants.Registro);

                        for (int i = 0; i < registroList.Count; i++)
                        {
                            Empresa empresa = new Empresa();

                            var registroNode = registroList[i];
                            var registroElemento = (XmlElement)registroNode;

                            empresa.CNPJ = registroElemento.GetAttribute(NFe.Components.NFeStrConstants.CNPJ).Trim();
                            empresa.Nome = registroElemento.GetElementsByTagName(NFe.Components.NFeStrConstants.Nome)[0].InnerText.Trim();

                            empresa.Servico = Propriedade.TipoAplicativo;// TipoAplicativo.Nfe;
                            if (registroElemento.GetAttribute(NFe.Components.NFeStrConstants.Servico) != "")
                                empresa.Servico = (TipoAplicativo)Convert.ToInt16(registroElemento.GetAttribute(NFe.Components.NFeStrConstants.Servico).Trim());

                            #region Definir a pasta das configurações da empresa
                            empresa.PastaEmpresa = Propriedade.PastaExecutavel + "\\" + empresa.CNPJ.Trim();

                            switch (empresa.Servico)
                            {
                                case TipoAplicativo.Nfe:
                                    break;

                                default:
                                    empresa.PastaEmpresa += "\\" + empresa.Servico.ToString().ToLower();
                                    break;
                            }
                            #endregion

                            empresa.NomeArquivoConfig = empresa.PastaEmpresa + "\\" + Propriedade.NomeArqConfig;

                            try
                            {
                                BuscaConfiguracao(empresa);
                            }
                            catch (Exception ex)
                            {
                                ///
                                /// nao acessar o metodo Auxiliar.GravarArqErroERP(string Arquivo, string Erro) já que nela tem a pesquisa da empresa
                                /// com base em "int emp = Empresas.FindEmpresaByThread();" e neste ponto ainda não foi criada
                                /// as thread's
                                string cArqErro;
                                if (string.IsNullOrEmpty(empresa.PastaXmlRetorno))
                                    cArqErro = Path.Combine(Propriedade.PastaExecutavel, string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddTHHmmss")));
                                else
                                    cArqErro = Path.Combine(empresa.PastaXmlRetorno, string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddTHHmmss")));

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
                            Empresas.Configuracoes.Add(empresa);
                        }
                    }

                    arqXml.Close();
                    arqXml = null;
                }
                finally
                {
                    if (arqXml != null)
                        arqXml.Close();
                }
            }

            if (!ExisteErroDiretorio)
                Empresa.CriarPasta();
        }
#endif
        #endregion

        #region BuscaConfiguracao()
        public void BuscaConfiguracao()
        {
            #region Criar diretório das configurações e dados da empresa
            if (!Directory.Exists(this.PastaEmpresa))
            {
                Directory.CreateDirectory(this.PastaEmpresa);
            }
            #endregion

            #region Limpar conteúdo dos atributos de configurações da empresa
            LimparPropriedades(this);
            #endregion

            #region Carregar as configurações do XML UniNFeConfig da Empresa

            if (File.Exists(this.NomeArquivoConfig))
            {
                try
                {
                    ObjectXMLSerializer objObjectXMLSerializer = new ObjectXMLSerializer();
                    ///
                    /// verifica se precisa de conversao para que a Deserializacao funcione
                    string temp = File.ReadAllText(this.NomeArquivoConfig, Encoding.UTF8);
                    if (temp.Contains("<nfe_configuracoes>"))
                    {
                        File.WriteAllText(this.NomeArquivoConfig + ".old", temp);

                        //this.BuscaConfiguracao(this);
                        //objObjectXMLSerializer.Save(this, this.NomeArquivoConfig);

                        temp = temp.Replace("<nfe_configuracoes>", "<Empresa xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                        temp = temp.Replace("</nfe_configuracoes>", "</Empresa>");
                        temp = temp.Replace(">False<", ">false<").Replace(">True<", ">true<");
                        File.WriteAllText(this.NomeArquivoConfig, temp);
                    }
                    Empresa t = new Empresa();
                    t = (Empresa)objObjectXMLSerializer.Load(typeof(Empresa), this.NomeArquivoConfig);
                    t.Nome = this.Nome;
                    t.CNPJ = this.CNPJ;
                    t.Servico = this.Servico;
                    t.CopyObjectTo(this);

                    this.CriarPastasDaEmpresa();

                    this.CertificadoPIN = Criptografia.descriptografaSenha(this.CertificadoPIN);

                    if (!this.CertificadoInstalado && !string.IsNullOrEmpty(this.CertificadoSenha))
                        this.CertificadoSenha = Criptografia.descriptografaSenha(this.CertificadoSenha);

                    this.X509Certificado = this.BuscaConfiguracaoCertificado();
                }
                catch (Exception ex)
                {
                    //Não vou mais fazer isso pois estava gerando problemas com Certificados A3 - Renan 18/06/2013
                    //empresa.Certificado = string.Empty;
                    //empresa.CertificadoThumbPrint = string.Empty;
                    throw new Exception("Ocorreu um erro ao efetuar a leitura das configurações da empresa " + this.Nome.Trim() + ". Por favor entre na tela de configurações desta empresa e reconfigure.\r\n\r\nErro: " + ex.Message);
                }
            }
            #endregion
        }

        /// <summary>
        /// Busca as configurações da empresa dentro de sua pasta gravadas em um XML chamado UniNfeConfig.Xml
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 28/07/2010
        /// </remarks>
#if false
        private void BuscaConfiguracao(Empresa empresa)
        {
            #region Criar diretório das configurações e dados da empresa
            if (!Directory.Exists(empresa.PastaEmpresa))
            {
                Directory.CreateDirectory(empresa.PastaEmpresa);
            }
            #endregion

            #region Limpar conteúdo dos atributos de configurações da empresa
            LimparPropriedades(empresa);
            #endregion

            #region Carregar as configurações do XML UniNFeConfig da Empresa
            FileStream arqXml = null;

            if (File.Exists(empresa.NomeArquivoConfig))
            {
                try
                {
                    //ObjectXMLSerializer objObjectXMLSerializer = new ObjectXMLSerializer();
                    //objObjectXMLSerializer.Load(empresa, empresa.NomeArquivoConfig);

                    arqXml = new FileStream(empresa.NomeArquivoConfig, FileMode.Open, FileAccess.Read, FileShare.Read); //Abrir um arquivo XML usando FileStrem
                    var xml = new XmlDocument();
                    xml.Load(arqXml);

                    var configList = xml.GetElementsByTagName(NFeStrConstants.nfe_configuracoes);
                    foreach (XmlNode configNode in configList)
                    {
                        var configElemento = (XmlElement)configNode;

                        empresa.UnidadeFederativaCodigo = Convert.ToInt32(Functions.LerTag(configElemento, NFeStrConstants.UnidadeFederativaCodigo, false));
                        empresa.AmbienteCodigo = Convert.ToInt32(Functions.LerTag(configElemento, NFeStrConstants.AmbienteCodigo, false));
                        empresa.tpEmis = Convert.ToInt32(Functions.LerTag(configElemento, NFeStrConstants.tpEmis, false));
                        empresa.GravarRetornoTXTNFe = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.GravarRetornoTXTNFe, "False"));
                        empresa.GravarEventosNaPastaEnviadosNFe = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.GravarEventosNaPastaEnviadosNFe, "False"));
                        empresa.GravarEventosCancelamentoNaPastaEnviadosNFe = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe, "False"));
                        empresa.GravarEventosDeTerceiros = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.GravarEventosDeTerceiros, "False"));
                        empresa.CompactarNfe = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.CompactarNfe, "False"));
                        empresa.IndSinc = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.IndSinc, "False"));
                        empresa.DiretorioSalvarComo = Functions.LerTag(configElemento, NFeStrConstants.DiretorioSalvarComo, "AM");
                        empresa.DiasLimpeza = Convert.ToInt32("0" + Functions.LerTag(configElemento, NFeStrConstants.DiasLimpeza, "0"));
                        empresa.TempoConsulta = Convert.ToInt32("0" + Functions.LerTag(configElemento, NFeStrConstants.TempoConsulta, "0"));

                        empresa.PastaXmlEnvio = Functions.LerTag(configElemento, NFeStrConstants.PastaXmlEnvio, false);
                        empresa.PastaXmlRetorno = Functions.LerTag(configElemento, NFeStrConstants.PastaXmlRetorno, false);
                        empresa.PastaXmlErro = Functions.LerTag(configElemento, NFeStrConstants.PastaXmlErro, false);
                        empresa.PastaValidar = Functions.LerTag(configElemento, NFeStrConstants.PastaValidar, false);
                        empresa.PastaXmlEnviado = Functions.LerTag(configElemento, NFeStrConstants.PastaXmlEnviado, false);
                        empresa.PastaBackup = Functions.LerTag(configElemento, NFeStrConstants.PastaBackup, false);
                        empresa.PastaXmlEmLote = Functions.LerTag(configElemento, NFeStrConstants.PastaXmlEmLote, false);
                        empresa.PastaDownloadNFeDest = Functions.LerTag(configElemento, NFeStrConstants.PastaDownloadNFeDest, false);

                        empresa.ConfiguracaoDanfe = Functions.LerTag(configElemento, NFeStrConstants.ConfiguracaoDanfe, false);
                        empresa.ConfiguracaoCCe = Functions.LerTag(configElemento, NFeStrConstants.ConfiguracaoCCe, false);
                        empresa.PastaExeUniDanfe = Functions.LerTag(configElemento, NFeStrConstants.PastaExeUniDanfe, false);
                        empresa.PastaConfigUniDanfe = Functions.LerTag(configElemento, NFeStrConstants.PastaConfigUniDanfe, false);
                        empresa.PastaDanfeMon = Functions.LerTag(configElemento, NFeStrConstants.PastaDanfeMon, false);
                        empresa.XMLDanfeMonNFe = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.XMLDanfeMonNFe, "False"));
                        empresa.XMLDanfeMonProcNFe = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.XMLDanfeMonProcNFe, "False"));
                        empresa.XMLDanfeMonDenegadaNFe = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.XMLDanfeMonDenegadaNFe, "False"));

                        empresa.FTPAtivo = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.FTPAtivo, "False"));
                        empresa.FTPGravaXMLPastaUnica = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.FTPGravaXMLPastaUnica, "False"));
                        empresa.FTPSenha = Functions.LerTag(configElemento, NFeStrConstants.FTPSenha, false);
                        empresa.FTPPastaAutorizados = Functions.LerTag(configElemento, NFeStrConstants.FTPPastaAutorizados, false);
                        empresa.FTPPastaRetornos = Functions.LerTag(configElemento, NFeStrConstants.FTPPastaRetornos, false);
                        empresa.FTPNomeDoUsuario = Functions.LerTag(configElemento, NFeStrConstants.FTPNomeDoUsuario, false);
                        empresa.FTPNomeDoServidor = Functions.LerTag(configElemento, NFeStrConstants.FTPNomeDoServidor, false);
                        empresa.FTPPorta = Convert.ToInt32("0" + Functions.LerTag(configElemento, NFeStrConstants.FTPPorta, false));

                        empresa.Certificado = Functions.LerTag(configElemento, NFeStrConstants.CertificadoDigital, false);
                        empresa.CertificadoArquivo = Functions.LerTag(configElemento, NFeStrConstants.CertificadoArquivo, false);
                        empresa.CertificadoThumbPrint = Functions.LerTag(configElemento, NFeStrConstants.CertificadoDigitalThumbPrint, false);
                        empresa.CertificadoInstalado = Convert.ToBoolean(Functions.LerTag(configElemento, NFeStrConstants.CertificadoInstalado, (!string.IsNullOrEmpty(empresa.CertificadoThumbPrint) || !string.IsNullOrEmpty(empresa.Certificado)).ToString()));
                        empresa.CertificadoPIN = Criptografia.descriptografaSenha(Functions.LerTag(configElemento, NFeStrConstants.CertificadoPIN, ""));

                        if (!empresa.CertificadoInstalado)
                            if (configElemento.GetElementsByTagName(NFeStrConstants.CertificadoSenha)[0] != null)
                                if (!string.IsNullOrEmpty(configElemento.GetElementsByTagName(NFeStrConstants.CertificadoSenha)[0].InnerText.Trim()))
                                    empresa.CertificadoSenha = Criptografia.descriptografaSenha(configElemento.GetElementsByTagName(NFeStrConstants.CertificadoSenha)[0].InnerText.Trim());

                        empresa.UsuarioWS = Functions.LerTag(configElemento, NFeStrConstants.UsuarioWS, false);
                        empresa.SenhaWS = Functions.LerTag(configElemento, NFeStrConstants.SenhaWS, false);
                    }
                    empresa.X509Certificado = empresa.BuscaConfiguracaoCertificado();
                }
                catch (Exception ex)
                {
                    //Não vou mais fazer isso pois estava gerando problemas com Certificados A3 - Renan 18/06/2013
                    //empresa.Certificado = string.Empty;
                    //empresa.CertificadoThumbPrint = string.Empty;
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
#endif
        #endregion

        #region BuscaConfiguracaoCertificado
        public X509Certificate2 BuscaConfiguracaoCertificado()
        {
            X509Certificate2 x509Cert = null;

            //Certificado instalado no windows
            if (this.CertificadoInstalado)
            {
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = null;
                if (!string.IsNullOrEmpty(this.CertificadoDigitalThumbPrint))
                    collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, this.CertificadoDigitalThumbPrint, false);
                else
                    collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, this.Certificado, false);

                for (int i = 0; i < collection1.Count; i++)
                {
                    //Verificar a validade do certificado
                    if (DateTime.Compare(DateTime.Now, collection1[i].NotAfter) == -1)
                    {
                        x509Cert = collection1[i];
                        break;
                    }
                }

                //Se não encontrou nenhum certificado com validade correta, vou pegar o primeiro certificado, porem vai travar na hora de tentar enviar a nota fiscal, por conta da validade. Wandrey 06/04/2011
                if (x509Cert == null && collection1.Count > 0)
                    x509Cert = collection1[0];
            }
            else //Certificado está sendo acessado direto do arquivo .PFX
            {
                if (string.IsNullOrEmpty(this.CertificadoArquivo))
                    throw new Exception("Nome do arquivo referente ao certificado digital não foi informado nas configurações do UniNFe.");
                else if (!string.IsNullOrEmpty(this.CertificadoArquivo) && !File.Exists(this.CertificadoArquivo))
                    throw new Exception(string.Format("Certificado digital \"{0}\" não encontrado.", this.CertificadoArquivo));

                using (FileStream fs = new FileStream(this.CertificadoArquivo, FileMode.Open))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    x509Cert = new X509Certificate2(buffer, this.CertificadoSenha);
                }
            }
            return x509Cert;
        }
        #endregion

        #region ChecaCaminhoDiretorio
        /// <summary>
        /// Método para checagem dos caminhos e sua existencia ou não no pc
        /// </summary>
        /// <param name="empresa">Empresa a ser validado os caminhos das pastas</param>
        public void ChecaCaminhoDiretorio()
        {
            FileStream arqXml = null;

            if (File.Exists(this.NomeArquivoConfig))
            {
                try
                {
                    arqXml = new FileStream(this.NomeArquivoConfig, FileMode.Open, FileAccess.Read, FileShare.Read); //Abrir um arquivo XML usando FileStrem
                    var xml = new XmlDocument();
                    xml.Load(arqXml);
                    var configList = xml.GetElementsByTagName("Empresa");//NFeStrConstants.nfe_configuracoes);
                    foreach (XmlNode configNode in configList)
                    {
                        var configElemento = (XmlElement)configNode;

                        Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaXmlEnvio, "Pasta onde serão gravados os arquivos XML´s a serem enviados individualmente para os WebServices", true);
                        Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaXmlRetorno, "Pasta onde serão gravados os arquivos XML´s de retorno dos WebServices", true);
                        Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaXmlErro, "Pasta para arquivamento temporário dos XML´s que apresentaram erro na tentativa do envio", true);
                        Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaValidar, "Pasta onde serão gravados os arquivos XML´s a serem somente validados", true);
                        if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                        {
                            Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaXmlEnviado, "Pasta onde serão gravados os arquivos XML´s enviados", true);
                            Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaXmlEmLote, "Pasta onde serão gravados os arquivos XML´s de NF-e a serem enviadas em lote para os WebServices", false);
                            Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaBackup, "Pasta para Backup dos XML´s enviados", false);
                            Empresas.verificaPasta(this, configElemento, NFeStrConstants.PastaDownloadNFeDest, "Pasta onde serão gravados os arquivos XML´s de download de NFe de destinatários e eventos de terceiros", false);
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

        #region CriarSubPastaEnviado()
        /// <summary>
        /// Criar as subpastas (Autorizados/Denegados/EmProcessamento) dentro da pasta dos XML´s enviados para a empresa passada por parâmetro
        /// </summary>
        /// <param name="indexEmpresa">Index da Empresa a ser pesquisado na coleção de configurações das empresas cadastradas</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Date: 20/04/2010
        /// </remarks>
        public void CriarSubPastaEnviado()
        {
            if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
            {
                if (!string.IsNullOrEmpty(this.PastaXmlEnviado))
                {
                    //Criar a pasta EmProcessamento
                    if (!Directory.Exists(this.PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString()))
                    {
                        System.IO.Directory.CreateDirectory(this.PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString());
                    }

                    //Criar a Pasta Autorizado
                    if (!Directory.Exists(this.PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString()))
                    {
                        System.IO.Directory.CreateDirectory(this.PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString());
                    }

                    //Criar a Pasta Denegado
                    if (!Directory.Exists(this.PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString()))
                    {
                        System.IO.Directory.CreateDirectory(this.PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString());
                    }
                }
            }
        }
        #endregion

        #region CriarPastasDaEmpresa
        public void CriarPastasDaEmpresa()
        {
            if (!Directory.Exists(this.PastaEmpresa))
                Directory.CreateDirectory(this.PastaEmpresa);

            //Criar pasta de envio
            if (!string.IsNullOrEmpty(PastaXmlEnvio))
            {
                if (!Directory.Exists(PastaXmlEnvio))
                {
                    Directory.CreateDirectory(PastaXmlEnvio);
                }

                //Criar a pasta Temp dentro da pasta de envio. Wandrey 03/08/2011
                if (!Directory.Exists(PastaXmlEnvio.Trim() + "\\Temp"))
                {
                    Directory.CreateDirectory(PastaXmlEnvio.Trim() + "\\Temp");
                }
            }

            if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
            {
                //Criar subpasta Assinado na pasta de envio individual de nfe
                if (!Directory.Exists(PastaXmlEnvio + Propriedade.NomePastaXMLAssinado))
                {
                    System.IO.Directory.CreateDirectory(PastaXmlEnvio + Propriedade.NomePastaXMLAssinado);
                }
                //Criar pasta de Envio em Lote
                if (!string.IsNullOrEmpty(PastaXmlEmLote))
                {
                    if (!Directory.Exists(PastaXmlEmLote))
                    {
                        Directory.CreateDirectory(PastaXmlEmLote);
                    }

                    //Criar a pasta Temp dentro da pasta de envio em lote. Wandrey 05/10/2011
                    if (!Directory.Exists(PastaXmlEmLote.Trim() + "\\Temp"))
                    {
                        Directory.CreateDirectory(PastaXmlEmLote.Trim() + "\\Temp");
                    }
                    if (!Directory.Exists(PastaXmlEmLote + Propriedade.NomePastaXMLAssinado))
                    {
                        System.IO.Directory.CreateDirectory(PastaXmlEmLote + Propriedade.NomePastaXMLAssinado);
                    }
                }

                //Criar pasta Enviado
                if (!string.IsNullOrEmpty(PastaXmlEnviado))
                {
                    if (!Directory.Exists(PastaXmlEnviado))
                    {
                        Directory.CreateDirectory(PastaXmlEnviado);
                    }
                }
                //Criar pasta de Backup
                if (!string.IsNullOrEmpty(PastaBackup))
                {
                    if (!Directory.Exists(PastaBackup))
                    {
                        Directory.CreateDirectory(PastaBackup);
                    }
                }

                //Criar pasta para monitoramento do DANFEMon e impressão do DANFE
                if (!string.IsNullOrEmpty(PastaDanfeMon))
                {
                    if (!Directory.Exists(PastaDanfeMon))
                    {
                        System.IO.Directory.CreateDirectory(PastaDanfeMon);
                    }
                }

                //Criar pasta para gravar as nfe de destinatarios
                if (!string.IsNullOrEmpty(PastaDownloadNFeDest))
                {
                    if (!Directory.Exists(PastaDownloadNFeDest))
                    {
                        System.IO.Directory.CreateDirectory(PastaDownloadNFeDest);
                    }
                }
            }

            //Criar pasta de Retorno
            if (!string.IsNullOrEmpty(PastaXmlRetorno))
            {
                if (!Directory.Exists(PastaXmlRetorno))
                {
                    Directory.CreateDirectory(PastaXmlRetorno);
                }
            }

            //Criar pasta de XML´s com erro
            if (!string.IsNullOrEmpty(PastaXmlErro))
            {
                if (!Directory.Exists(PastaXmlErro))
                {
                    Directory.CreateDirectory(PastaXmlErro);
                }
            }

            //Criar pasta para somente validação de XML´s
            if (!string.IsNullOrEmpty(PastaValidar))
            {
                if (!Directory.Exists(PastaValidar))
                {
                    Directory.CreateDirectory(PastaValidar);
                }

                //Criar a pasta Temp dentro da pasta de envio em lote. Wandrey 05/10/2011
                if (!Directory.Exists(PastaValidar.Trim() + "\\Temp"))
                {
                    Directory.CreateDirectory(PastaValidar.Trim() + "\\Temp");
                }
            }
            this.CriarSubPastaEnviado();
        }
        #endregion

        #region Ticket: #110
        /* Validação do arquivo de lock
         * Marcelo
         * 03/06/2013
         */
        /// <summary>
        /// Exclui o arquivo de lock associado a esta empresa/ instancia
        /// </summary>
        public void DeleteLockFile()
        {
            string file = String.Format("{0}\\{1}-{2}.lock", PastaBase, Propriedade.NomeAplicacao, Environment.MachineName);
            FileInfo fi = new FileInfo(file);

            if (fi.Exists)
                fi.Delete();
        }

        #region ExcluiPastas
        public void ExcluiPastas()
        {
            if (Directory.Exists(this.PastaEmpresa))
            {
                ///
                /// vazia?
                /// 
                bool vazia = true;
                foreach(var vfile in Directory.GetFiles(this.PastaEmpresa, "*.*", SearchOption.AllDirectories))
                {
                    if (vfile.EndsWith(Propriedade.NomeArqXmlFluxoNfe)) continue;
                    if (vfile.EndsWith(Propriedade.NomeArqConfig)) continue;
                    if (vfile.EndsWith(Propriedade.NomeArqXmlLoteBkp1)) continue;
                    if (vfile.EndsWith(Propriedade.NomeArqXmlLoteBkp2)) continue;
                    if (vfile.EndsWith(Propriedade.NomeArqXmlLoteBkp3)) continue;
                    if (vfile.EndsWith(".lock")) continue;

                    vazia = false; 
                    break; 
                }
                if (vazia)
                {
                    DeltreeMain.BeginDeleteTree(this.PastaBackup, true);
                    DeltreeMain.BeginDeleteTree(this.PastaDownloadNFeDest, true);
                    DeltreeMain.BeginDeleteTree(this.PastaValidar, true);
                    DeltreeMain.BeginDeleteTree(this.PastaXmlEmLote, true);
                    DeltreeMain.BeginDeleteTree(this.PastaXmlEnviado, true);
                    DeltreeMain.BeginDeleteTree(this.PastaXmlEnvio, true);
                    DeltreeMain.BeginDeleteTree(this.PastaXmlErro, true);
                    DeltreeMain.BeginDeleteTree(this.PastaXmlRetorno, true);
                    DeltreeMain.BeginDeleteTree(this.PastaEmpresa, true);
                }
            }
        }
        #endregion

        #endregion

        #region Limpar conteúdo dos atributos de configurações da empresa
        private void LimparPropriedades(Empresa empresa)
        {
            empresa.PastaXmlEnvio =
                empresa.PastaXmlRetorno =
                empresa.PastaXmlEnviado =
                empresa.PastaXmlErro =
                empresa.PastaBackup =
                empresa.PastaXmlEmLote =
                empresa.PastaValidar =
                empresa.PastaDanfeMon =
                empresa.PastaExeUniDanfe =
                empresa.ConfiguracaoDanfe =
                empresa.ConfiguracaoCCe =
                empresa.PastaConfigUniDanfe =
                empresa.PastaDownloadNFeDest =
                empresa.EmailDanfe = string.Empty;

            empresa.X509Certificado = null;
            empresa.CertificadoInstalado = true;
            empresa.CertificadoArquivo =
                empresa.CertificadoDigitalThumbPrint =
                empresa.CertificadoSenha =
                empresa.CertificadoPIN =
                empresa.Certificado = string.Empty;

            empresa.FTPAtivo = false;
            empresa.FTPPorta = 21;
            empresa.FTPSenha =
                empresa.FTPNomeDoServidor =
                empresa.FTPNomeDoUsuario =
                empresa.FTPPastaRetornos =
                empresa.FTPPastaAutorizados = string.Empty;

            empresa.UnidadeFederativaCodigo = 0;
            empresa.DiasLimpeza = 0;
            empresa.TempoConsulta = 2;

            empresa.CriaPastasAutomaticamente = false;

            empresa.UsuarioWS = string.Empty;
            empresa.SenhaWS = string.Empty;

            empresa.AmbienteCodigo = (int)NFe.Components.TipoAmbiente.taHomologacao; //2
            empresa.tpEmis = (int)NFe.Components.TipoEmissao.teNormal; //1
            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
                empresa.UnidadeFederativaCodigo = 41;

            empresa.GravarRetornoTXTNFe =
                empresa.GravarEventosNaPastaEnviadosNFe =
                empresa.GravarEventosCancelamentoNaPastaEnviadosNFe =
                empresa.GravarEventosDeTerceiros =
                empresa.XMLDanfeMonNFe =
                empresa.XMLDanfeMonProcNFe =
                empresa.XMLDanfeMonDenegadaNFe =
                empresa.IndSinc = false;
            empresa.AdicionaEmailDanfe = true;
            empresa.DiretorioSalvarComo = "AM";
        }
        #endregion

        /// <summary>
        /// RemoveEndSlash
        /// </summary>
        public void RemoveEndSlash()
        {
            PastaXmlEnvio = ConfiguracaoApp.RemoveEndSlash(PastaXmlEnvio);
            PastaXmlErro = ConfiguracaoApp.RemoveEndSlash(PastaXmlErro);
            PastaXmlRetorno = ConfiguracaoApp.RemoveEndSlash(PastaXmlRetorno);
            PastaValidar = ConfiguracaoApp.RemoveEndSlash(PastaValidar);
            if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
            {
                PastaXmlEnviado = ConfiguracaoApp.RemoveEndSlash(PastaXmlEnviado);
                PastaBackup = ConfiguracaoApp.RemoveEndSlash(PastaBackup);
                PastaXmlEmLote = ConfiguracaoApp.RemoveEndSlash(PastaXmlEmLote);
                PastaDownloadNFeDest = ConfiguracaoApp.RemoveEndSlash(PastaDownloadNFeDest);
                PastaDanfeMon = ConfiguracaoApp.RemoveEndSlash(PastaDanfeMon);
                PastaExeUniDanfe = ConfiguracaoApp.RemoveEndSlash(PastaExeUniDanfe);
                PastaConfigUniDanfe = ConfiguracaoApp.RemoveEndSlash(PastaConfigUniDanfe);
            }
        }

        #region SalvarConfiguracao()
        public void SalvarConfiguracao(bool validaCertificado)
        {
            ValidarConfig(validaCertificado);

            if (!Directory.Exists(this.PastaEmpresa))
                Directory.CreateDirectory(this.PastaEmpresa);

            ObjectXMLSerializer objObjectXMLSerializer = new ObjectXMLSerializer();
            objObjectXMLSerializer.Save(this, this.NomeArquivoConfig);

            this.CriarPastasDaEmpresa();

            if (Empresas.FindConfEmpresaIndex(this.CNPJ, this.Servico) == -1)
                Empresas.Configuracoes.Add(this);
            else
                Empresas.FindConfEmpresa(this.CNPJ, this.Servico).Nome = this.Nome;
        }
        #endregion

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
                        Auxiliar.WriteLog("Ocorreu um erro ao tentar conectar no FTP: " + ex.Message);
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

        void _AddEmpresaNaListaDeComparacao(Dictionary<string, int> fc, Empresa empresa)
        {
            int i = 0;
            fc.Add(empresa.PastaXmlEnvio.ToLower(), i);
            if (!string.IsNullOrEmpty(empresa.PastaXmlRetorno))
                fc.Add(empresa.PastaXmlRetorno.ToLower(), ++i);
            if (!string.IsNullOrEmpty(empresa.PastaXmlErro))
                fc.Add(empresa.PastaXmlErro.ToLower(), ++i);
            if (!string.IsNullOrEmpty(empresa.PastaValidar))
                fc.Add(empresa.PastaValidar.ToLower(), ++i);
            if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
            {
                if (!string.IsNullOrEmpty(empresa.PastaXmlEnviado))
                    fc.Add(empresa.PastaXmlEnviado.ToLower(), ++i);
                if (!string.IsNullOrEmpty(empresa.PastaXmlEmLote))
                    fc.Add(empresa.PastaXmlEmLote.ToLower(), ++i);
                if (!string.IsNullOrEmpty(empresa.PastaBackup))
                    fc.Add(empresa.PastaBackup.ToLower(), ++i);
                if (!string.IsNullOrEmpty(empresa.PastaDownloadNFeDest))
                    fc.Add(empresa.PastaDownloadNFeDest.ToLower(), ++i);
            }
        }

        internal class _validacao
        {
            public string FolderName;
            public string ValueError;   //se branco nao é obrigatorio
            public string FolderError;
            public _validacao(string key, string value, string folder="")
            {
                FolderName = key;
                ValueError = value;
                FolderError = folder;
            }
        }
        /// <summary>
        /// ValidarConfig
        /// </summary>
        private void ValidarConfig(bool validaCertificado)
        {
            new ConfiguracaoApp().ValidarConfig(validaCertificado);
        }
    }
}
using NFe.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

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

                if (!string.IsNullOrEmpty(PastaXmlEnvio))
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

        public string PastaContingencia => Path.Combine(PastaXmlEnvio, "Contingencia");
        public string PastaValidado => Path.Combine(PastaValidar, "Validado");

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
        /// UsaCertificado
        /// Define se a empresa necessita de um certificado digital para assinatura/transmissao
        /// </summary>
        public bool UsaCertificado { get; set; }
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
        /// Utilizado para certificados A3
        /// </summary>
        public string CertificadoPIN { get; set; }
        /// <summary>
        /// Utiliza
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CertificadoPINCarregado = false;
        /// <summary>
        /// Certificado digital - Subject
        /// </summary>
        public string Certificado { get; set; }
        /// <summary>
        /// Certificado digital - ThumbPrint
        /// </summary>
        public string CertificadoDigitalThumbPrint { get; set; }
        /// <summary>
        /// Certificado digital
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        [AttributeTipoAplicacao(TipoAplicativo.Nulo)]
        public X509Certificate2 X509Certificado { get; set; }
        /// <summary>
        /// Gravar o retorno da NFe também em TXT
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.Nfe)]
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
        /// <summary>
        /// Se verdadediro valida o DigestValue da NFe/NFCe/MDFe/CTe que estão na pasta em processamento com o DigestValeu do pedido de situação
        /// </summary>
        public bool CompararDigestValueDFeRetornadoSEFAZ { get; set; }
        /// <summary>
        /// Pode ser: CPF, CEI, CAEPF ou CNPJ
        /// </summary>
        public string Documento { get; set; }
        /// <summary>
        /// Esta propriedade se true, vai processar um arquivo de cada vez.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CriarFilaProcesamento { get; set; }

        /// <summary>
        /// Se verdadeiro, só irá salvar o xml de distribuição na pasta de autorizados
        /// </summary>
        public bool SalvarSomenteXMLDistribuicao { get; set; }
        #endregion

        #region Propriedades da parte das configurações por empresa

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private List<Thread> threads;

        /// <summary>
        /// Nome da pasta onde é gravado as configurações e informações da Empresa
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string PastaEmpresa => Propriedade.PastaExecutavel + "\\" + CNPJ +
                    (Servico == TipoAplicativo.Nfe ||
                     Servico == TipoAplicativo.Todos ? "" : "\\" + Servico.ToString().ToLower());
        /// <summary>
        /// Nome do arquivo XML das configurações da empresa
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string NomeArquivoConfig => Path.Combine(PastaEmpresa, Propriedade.NomeArqConfig);

        public bool CriaPastasAutomaticamente { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool GravarEventosNaPastaEnviadosNFe { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool GravarEventosCancelamentoNaPastaEnviadosNFe { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool GravarEventosDeTerceiros { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.Nfe | TipoAplicativo.Todos)]
        public bool ArqNSU { get; set; }

        /// <summary>
        /// Enviar NFe utilizando o processo síncrono (true or false)
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public bool IndSinc { get; set; }

        /// <summary>
        /// Enviar NFe utilizando o processo síncrono (true or false)
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.MDFe)]
        public bool IndSincMDFe { get; set; }

        /// <summary>
        /// URLs de Serviços da NFe, NFCe, CTe
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public EstadoURLConsultaDFe URLConsultaDFe { get; set; }
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
        /// <summary>
        /// Codigo de identificacao do CSC
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string IdentificadorCSC { get; set; }
        /// <summary>
        /// Codigo CSC/Token
        /// </summary>
        [NFe.Components.AttributeTipoAplicacao(TipoAplicativo.Nfe)]
        public string TokenCSC { get; set; }

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
            get => DiretorioSalvarComo.ToString();
            set => DiretorioSalvarComo = value;
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public DiretorioSalvarComo DiretorioSalvarComo
        {
            get
            {
                if (string.IsNullOrEmpty(mDiretorioSalvarComo))
                {
                    mDiretorioSalvarComo = "AM";//padrão
                }

                return mDiretorioSalvarComo;
            }
            set => mDiretorioSalvarComo = value;
        }
        #endregion

        #region Propriedades para controle de FTP (posicionado aqui só para quando for gravar as informacoes a tag FTP seja a ultima da lista)
        public string FTPPastaAutorizados { get; set; }
        public string FTPPastaRetornos { get; set; }
        public string FTPNomeDoServidor { get; set; }
        public string FTPNomeDoUsuario { get; set; }
        public string FTPSenha { get; set; }
        public int FTPPorta { get; set; }
        public bool FTPAtivo { get; set; }
        public bool FTPPassivo { get; set; }
        public bool FTPGravaXMLPastaUnica { get; set; }
        public bool FTPIsAlive => FTPAtivo &&
                    !string.IsNullOrEmpty(FTPNomeDoServidor) &&
                    !string.IsNullOrEmpty(FTPNomeDoUsuario) &&
                    !string.IsNullOrEmpty(FTPSenha);
        #endregion

        #region Propriedades de configuração do equipamento SAT
        /// <summary>
        /// Maraca do equipamento SAT
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public string MarcaSAT { get; set; }

        /// <summary>
        /// Código de ativação do equipamento SAT
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public string CodigoAtivacaoSAT { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public bool UtilizaConversaoCFe { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public string CNPJSoftwareHouse { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public string SignACSAT { get; set; }

        private string numeroCaixa;

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public string NumeroCaixa
        {
            get
            {
                if (string.IsNullOrEmpty(numeroCaixa))
                {
                    numeroCaixa = "001";
                }

                return numeroCaixa;
            }
            set => numeroCaixa = value;
        }

        private RegTribISSQN regTribISSQNSAT;

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public RegTribISSQN RegTribISSQNSAT
        {
            get
            {
                if (regTribISSQNSAT == 0)
                {
                    regTribISSQNSAT = RegTribISSQN.MicroEmpresaMunicipal;
                }

                return regTribISSQNSAT;
            }
            set => regTribISSQNSAT = value;
        }

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public IndRatISSQN IndRatISSQNSAT { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public string VersaoLayoutSAT { get; set; }

        [AttributeTipoAplicacao(TipoAplicativo.SAT)]
        public string TipoConversao { get; set; }

        /// <summary>
        /// Código da aplicação que está cadastrada para acessar os serviços REST do município de Florianópolis-SC
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.Nfse)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string ClientID { get; set; }

        /// <summary>
        /// Código secreto da aplicação que está cadastrada para acessar os serviços REST do município de Florianópolis-SC
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.Nfse)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Token utilizado pela para acessar os serviços REST do município de Florianópolis-SC
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.Nfse)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string TokenNFse { get; set; }

        /// <summary>
        /// Data e hora em que o token de acesso da nfse irá exiprar
        /// </summary>
        [AttributeTipoAplicacao(TipoAplicativo.Nfse)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public DateTime TokenNFSeExpire { get; set; }

        /// <summary>
        /// CNPJ do responsável técnico
        /// </summary>
        public string RespTecCNPJ { get; set; }

        /// <summary>
        /// Nome do contato do responsável técnico
        /// </summary>
        public string RespTecXContato { get; set; }

        /// <summary>
        /// E-mail do responsável técnico
        /// </summary>
        public string RespTecEmail { get; set; }

        /// <summary>
        /// Telefone do responsável técnico
        /// </summary>
        public string RespTecTelefone { get; set; }

        /// <summary>
        /// Identificador do CSRT do responsável técnico
        /// </summary>
        public string RespTecIdCSRT { get; set; }

        /// <summary>
        /// Número do CSRT do responsável técnico
        /// </summary>
        public string RespTecCSRT { get; set; }

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

            threads = new List<Thread>();
        }

        ~Empresa()
        {
            foreach (Thread thr in threads)
            {
                if (thr.IsAlive)
                {
                    thr.Abort();
                }
            }
        }

        #region BuscaConfiguracao()
        public string BuscaConfiguracao(ref int tipoerro)
        {
            tipoerro = 0;

            if (!Directory.Exists(PastaEmpresa))
            {
                Directory.CreateDirectory(PastaEmpresa);
            }

            LimparPropriedades(this);

            if (File.Exists(NomeArquivoConfig))
            {
                try
                {
                    ObjectXMLSerializer objObjectXMLSerializer = new ObjectXMLSerializer();
                    ///
                    /// verifica se precisa de conversao para que a Deserializacao funcione
                    string temp = File.ReadAllText(NomeArquivoConfig, Encoding.UTF8);

                    if (temp.Contains("<nfe_configuracoes>") || temp.Contains("<CertificadoDigital>"))
                    {
                        File.WriteAllText(NomeArquivoConfig + ".old", temp);

                        //this.BuscaConfiguracao(this);
                        //objObjectXMLSerializer.Save(this, this.NomeArquivoConfig);

                        temp = temp.Replace("<nfe_configuracoes>", "<Empresa xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                        temp = temp.Replace("</nfe_configuracoes>", "</Empresa>");
                        temp = temp.Replace(">False<", ">false<").Replace(">True<", ">true<");
                        if (!temp.Contains("<diretorioSalvarComo>"))
                        {
                            temp = temp.Replace("<DiretorioSalvarComo>", "<diretorioSalvarComo>").Replace("</DiretorioSalvarComo>", "</diretorioSalvarComo>");
                        }
                        temp = temp.Replace("<CertificadoDigital>", "<Certificado>").Replace("</CertificadoDigital>", "</Certificado>");
                        File.WriteAllText(NomeArquivoConfig, temp);
                    }
                    Empresa t = new Empresa();
                    t = (Empresa)objObjectXMLSerializer.Load(typeof(Empresa), NomeArquivoConfig);

                    if (!temp.Contains("<UsaCertificado>"))
                    {
                        t.UsaCertificado = true;
                    }

                    if (!temp.Contains("<FTPPassivo>"))
                    {
                        t.FTPPassivo = false;
                    }

                    if (t.UsaCertificado)
                    {
                        if (!t.CertificadoInstalado && !string.IsNullOrEmpty(t.CertificadoSenha))
                        {
                            t.CertificadoSenha = Criptografia.descriptografaSenha(t.CertificadoSenha);
                        }
                    }

                    t.CertificadoPIN = Criptografia.descriptografaSenha(t.CertificadoPIN);
                    t.FTPNomeDoServidor = Criptografia.descriptografaSenha(t.FTPNomeDoServidor);
                    t.FTPNomeDoUsuario = Criptografia.descriptografaSenha(t.FTPNomeDoUsuario);
                    t.FTPSenha = Criptografia.descriptografaSenha(t.FTPSenha);

                    t.Nome = Nome;
                    t.CNPJ = CNPJ;
                    t.Servico = Servico;
                    if (t.Servico != TipoAplicativo.Nfse)
                    {
                        t.UsaCertificado = true;
                    }

                    t.CopyObjectTo(this);

                    if (t.UnidadeFederativaCodigo == 35)
                    {
                        t.IndSinc = false;
                    }

#if _fw46
                    if (t.UnidadeFederativaCodigo.Equals(4205407))
                    {
                        var result = t.RecuperarConfiguracaoNFSeSoftplan(t.CNPJ);
                        this.ClientID = result.ClientID;
                        this.ClientSecret= result.ClientSecret;
                        this.TokenNFse = result.TokenNFse;
                        this.TokenNFSeExpire = result.TokenNFSeExpire;
                    }
#endif

                    CriarPastasDaEmpresa();

                    X509Certificado = BuscaConfiguracaoCertificado();
                }
                catch (Exception ex)
                {
                    //Não vou mais fazer isso pois estava gerando problemas com Certificados A3 - Renan 18/06/2013
                    //empresa.Certificado = string.Empty;
                    //empresa.CertificadoThumbPrint = string.Empty;
                    tipoerro = 2;
                    return "Ocorreu um erro ao efetuar a leitura das configurações da empresa " +
                        CNPJ + "=" + Nome.Trim() + ". Por favor entre na tela de configurações desta empresa e reconfigure.\r\n\r\nErro: " + ex.Message;
                }
                return null;
            }
            else
            {
                tipoerro = 1;
                return "Arquivo [" + NomeArquivoConfig + "] de configuração da empresa [" + CNPJ + "=" + Nome.Trim() + "] não encontrado";
            }
        }
        #endregion

        #region BuscaConfiguracaoCertificado
        public X509Certificate2 BuscaConfiguracaoCertificado()
        {
            X509Certificate2 x509Cert = null;

            if (UsaCertificado)
            {
                //Certificado instalado no windows
                if (CertificadoInstalado)
                {
                    X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection collection = store.Certificates;
                    X509Certificate2Collection collection1 = null;
                    if (!string.IsNullOrEmpty(CertificadoDigitalThumbPrint))
                    {
                        collection1 = collection.Find(X509FindType.FindByThumbprint, CertificadoDigitalThumbPrint, false);
                    }
                    else
                    {
                        collection1 = collection.Find(X509FindType.FindBySubjectDistinguishedName, Certificado, false);
                    }

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
                    {
                        x509Cert = collection1[0];
                    }
                }
                else //Certificado está sendo acessado direto do arquivo .PFX
                {
                    if (string.IsNullOrEmpty(CertificadoArquivo))
                    {
                        throw new Exception("Nome do arquivo referente ao certificado digital não foi informado nas configurações do UniNFe.");
                    }
                    else if (!string.IsNullOrEmpty(CertificadoArquivo) && !File.Exists(CertificadoArquivo))
                    {
                        throw new Exception(string.Format("Certificado digital \"{0}\" não encontrado.", CertificadoArquivo));
                    }

                    using (FileStream fs = new FileStream(CertificadoArquivo, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        x509Cert = new X509Certificate2(buffer, CertificadoSenha);
                    }
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

            if (File.Exists(NomeArquivoConfig))
            {
                try
                {
                    arqXml = new FileStream(NomeArquivoConfig, FileMode.Open, FileAccess.Read, FileShare.Read); //Abrir um arquivo XML usando FileStrem
                    XmlDocument xml = new XmlDocument();
                    xml.Load(arqXml);
                    XmlNodeList configList = xml.GetElementsByTagName("Empresa");
                    foreach (XmlNode configNode in configList)
                    {
                        XmlElement configElemento = (XmlElement)configNode;

                        Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaXmlEnvio, "Pasta onde serão gravados os arquivos XML´s a serem enviados individualmente para os WebServices", true);
                        Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaXmlRetorno, "Pasta onde serão gravados os arquivos XML´s de retorno dos WebServices", true);
                        Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaXmlErro, "Pasta para arquivamento temporário dos XML´s que apresentaram erro na tentativa do envio", true);
                        Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaValidar, "Pasta onde serão gravados os arquivos XML´s a serem somente validados", true);
                        if (Servico != TipoAplicativo.Nfse)
                        {
                            Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaXmlEnviado, "Pasta onde serão gravados os arquivos XML´s enviados", true);
                            Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaXmlEmLote, "Pasta onde serão gravados os arquivos XML´s de NF-e a serem enviadas em lote para os WebServices", false);
                            Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaBackup, "Pasta para Backup dos XML´s enviados", false);
                            Empresas.VerificaPasta(this, configElemento, NFeStrConstants.PastaDownloadNFeDest, "Pasta onde serão gravados os arquivos XML´s de download de NFe de destinatários e eventos de terceiros", false);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (arqXml != null)
                    {
                        arqXml.Close();
                    }
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
            if (Servico != TipoAplicativo.Nfse)
            {
                if (!string.IsNullOrEmpty(PastaXmlEnviado))
                {
                    //Criar a pasta EmProcessamento
                    if (!Directory.Exists(PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString()))
                    {
                        Directory.CreateDirectory(PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString());
                    }

                    //Criar a Pasta Autorizado
                    if (!Directory.Exists(PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString()))
                    {
                        Directory.CreateDirectory(PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString());
                    }

                    //Criar a Pasta Denegado
                    if (!Directory.Exists(PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString()))
                    {
                        Directory.CreateDirectory(PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString());
                    }
                }
            }
        }
        #endregion

        #region CriarPastasDaEmpresa
        public void CriarPastasDaEmpresa()
        {
            if (!Directory.Exists(PastaEmpresa))
            {
                Directory.CreateDirectory(PastaEmpresa);
            }

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

                if (!Directory.Exists(PastaContingencia) && Servico != TipoAplicativo.Nfse)
                {
                    Directory.CreateDirectory(PastaContingencia);
                }
            }

            if (Servico != TipoAplicativo.Nfse)
            {
                if (!string.IsNullOrEmpty(PastaXmlEnvio))
                {
                    //Criar subpasta Assinado na pasta de envio individual de nfe
                    if (!Directory.Exists(PastaXmlEnvio + Propriedade.NomePastaXMLAssinado))
                    {
                        System.IO.Directory.CreateDirectory(PastaXmlEnvio + Propriedade.NomePastaXMLAssinado);
                    }
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

            CriarSubPastaEnviado();
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
            string file = string.Format("{0}\\{1}-{2}.lock", PastaBase, Propriedade.NomeAplicacao, Environment.MachineName);
            FileInfo fi = new FileInfo(file);

            if (fi.Exists)
            {
                fi.Delete();
            }
        }

        #region ExcluiPastas
        public void ExcluiPastas()
        {
            /*
            //Não vou mais excluir nenhuma pasta, usuário que exclua manualmente, pois aconteceu de usuários excluirem a empresa para cadastrar 
            //novamente e perdeu todos os XMLs da empresa. Wandrey 07/06/2016
            if (Directory.Exists(this.PastaEmpresa))
            {
                ///
                /// vazia?
                /// 
                bool vazia = true;
                foreach (var vfile in Directory.GetFiles(this.PastaEmpresa, "*.*", SearchOption.AllDirectories))
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
            */
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
            empresa.UsaCertificado = true;
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
            PastaXmlEnviado = Servico == TipoAplicativo.Nfse ? "" : ConfiguracaoApp.RemoveEndSlash(PastaXmlEnviado);
            PastaBackup = Servico == TipoAplicativo.Nfse ? "" : ConfiguracaoApp.RemoveEndSlash(PastaBackup);
            PastaXmlEmLote = Servico == TipoAplicativo.Nfse ? "" : ConfiguracaoApp.RemoveEndSlash(PastaXmlEmLote);
            PastaDownloadNFeDest = Servico == TipoAplicativo.Nfse ? "" : ConfiguracaoApp.RemoveEndSlash(PastaDownloadNFeDest);
            PastaDanfeMon = Servico == TipoAplicativo.Nfse ? "" : ConfiguracaoApp.RemoveEndSlash(PastaDanfeMon);
            PastaExeUniDanfe = Servico == TipoAplicativo.Nfse ? "" : ConfiguracaoApp.RemoveEndSlash(PastaExeUniDanfe);
            PastaConfigUniDanfe = Servico == TipoAplicativo.Nfse ? "" : ConfiguracaoApp.RemoveEndSlash(PastaConfigUniDanfe);
        }

        #region SalvarConfiguracao()
        public void SalvarConfiguracao(bool validaCertificado, bool validarConfig)
        {
            bool empresaNova = false;
            try
            {
                if (Empresas.FindConfEmpresaIndex(CNPJ, Servico) == -1)
                {
                    empresaNova = true;
                    Empresas.Configuracoes.Add(this);
                }
                else
                {
                    int emp = Empresas.FindConfEmpresaIndex(CNPJ, Servico);
                    this.CopyObjectTo(Empresas.Configuracoes[emp]);
                }


                //Criptografar a senha do certificado digital para gravar no XML. Wandrey 23/09/2014
                if (validarConfig)
                {
                    new ConfiguracaoApp().ValidarConfig(validaCertificado, this);
                }

                if (!Directory.Exists(PastaEmpresa))
                {
                    Directory.CreateDirectory(PastaEmpresa);
                }

                CriarPastasDaEmpresa();

                Empresa dados = new Empresa();
                this.CopyObjectTo(dados);
                if (dados.UsaCertificado)
                {
                    dados.CertificadoSenha = Criptografia.criptografaSenha(dados.CertificadoSenha);
                }

                if (clsX509Certificate2Extension.IsA3(dados.X509Certificado))
                {
                    dados.CertificadoPIN = Criptografia.criptografaSenha(dados.CertificadoPIN);
                }
                else
                {
                    dados.CertificadoPIN = string.Empty;
                }
                dados.FTPNomeDoServidor = Criptografia.criptografaSenha(dados.FTPNomeDoServidor);
                dados.FTPNomeDoUsuario = Criptografia.criptografaSenha(dados.FTPNomeDoUsuario);
                dados.FTPSenha = Criptografia.criptografaSenha(dados.FTPSenha);

                ObjectXMLSerializer objObjectXMLSerializer = new ObjectXMLSerializer();
                objObjectXMLSerializer.Save(dados, dados.NomeArquivoConfig);

                Empresas.FindConfEmpresa(CNPJ, Servico).Nome = Nome;
            }
            catch (Exception ex)
            {
                if (empresaNova)
                {
                    Empresas.Configuracoes.Remove(this);
                }

                throw ex;
            }
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
            if (File.Exists(fileName) && FTPIsAlive)
            {
                Thread t = new Thread(new ThreadStart(delegate ()
                {
                    string arqDestino = Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName));
                    //Copia o arquivo para a pasta temp
                    FileInfo oArquivo = new FileInfo(fileName);
                    oArquivo.CopyTo(arqDestino, true);

                    FTP ftp = new FTP(FTPNomeDoServidor, FTPPorta, FTPNomeDoUsuario, FTPSenha);
                    try
                    {
                        //conecta ao ftp
                        ftp.Connect();
                        if (!ftp.IsConnected)
                        {
                            throw new Exception("FTP '" + FTPNomeDoServidor + "' não conectado");
                        }

                        ftp.PassiveMode = FTPPassivo;

                        //pega a pasta corrente no ftp
                        string vCorrente = ftp.GetWorkingDirectory();

                        //tenta mudar para a pasta de destino
                        if (!ftp.changeDir(folderName))
                        {
                            //como nao foi possivel mudar de pasta, a cria
                            ftp.makeDir(folderName);
                            if (!ftp.changeDir(folderName))
                            {
                                throw new Exception("Pasta '" + folderName + "' criada, mas não foi possível acessá-la");
                            }
                        }
                        //volta para a pasta corrente já que na "makeDir" a pasta se torna ativa na ultima pasta criada
                        ftp.ChangeDir(vCorrente);

                        //transfere o arquivo da pasta temp
                        string remote_filename = folderName + "/" + Path.GetFileName(fileName);
                        ftp.UploadFile(arqDestino, remote_filename, false);

                        if (ftp.GetFileSize(remote_filename) == 0)
                        {
                            throw new Exception("Arquivo '" + remote_filename + "' não encontrado no FTP");
                        }

                        Auxiliar.WriteLog("Arquivo '" + fileName + "' enviado ao FTP com o nome '" + remote_filename + "' com sucesso.", false);
                    }
                    catch (Exception ex)
                    {
                        Auxiliar.WriteLog("Ocorreu um erro ao tentar conectar no FTP: " + ex.Message, false);
                        ///
                        /// gravado o log de ftp aqui, pq o 'chamador' nao o trataria
                        /// 
                        new Auxiliar().GravarArqErroERP(Path.ChangeExtension(fileName, ".ftp"), ex.Message);
                    }
                    finally
                    {
                        if (ftp.IsConnected)
                        {
                            ftp.Disconnect();
                        }

                        //exclui o arquivo transferido da pasta temporaria
                        Functions.DeletarArquivo(arqDestino);
                    }
                    doneThread_FTP(Thread.CurrentThread);
                }));

                threads.Add(t);
                t.IsBackground = true;
                //t.Name = name;
                t.Start();
                //Não retornar o Join pois causa travamento no envio de arquivos quando com certificado A3: Wandrey/André-28/03/2018
                //t.Join();

            }
        }

#if _fw46
        public void SalvarConfiguracoesNFSeSoftplan(string usuario, string senha, string clientID, string clientSecret, string token, DateTime tokenExpire, string cnpj)
        {
            try
            {
                cnpj = Functions.OnlyNumbers(cnpj, ".-/").ToString();

                if (File.Exists($@"{Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), cnpj, "ConfiguracaoNFSeSoftplan.xml")}"))
                {
                    File.Delete($@"{Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), cnpj, "ConfiguracaoNFSeSoftplan.xml")}");
                }

                XmlDocument xmlConfiguracao = new XmlDocument();
                XmlDeclaration declaration = xmlConfiguracao.CreateXmlDeclaration("1.0", "UTF-8", "");
                xmlConfiguracao.AppendChild(declaration);

                XmlElement configuracao = xmlConfiguracao.CreateElement("Configuracao");
                xmlConfiguracao.AppendChild(configuracao);

                XmlNode usuarioConfiguracao = xmlConfiguracao.CreateNode(XmlNodeType.Element, "Usuario", "");
                XmlNode senhaConfiguracao = xmlConfiguracao.CreateNode(XmlNodeType.Element, "Senha", "");
                XmlNode clientIDConfiguracao = xmlConfiguracao.CreateNode(XmlNodeType.Element, "ClientID", "");
                XmlNode clientSecretConfiguracao = xmlConfiguracao.CreateNode(XmlNodeType.Element, "ClientSecret", "");
                XmlNode tokenConfiguracao = xmlConfiguracao.CreateNode(XmlNodeType.Element, "TokenNFSe", "");
                XmlNode tokenExpireConfiguracao = xmlConfiguracao.CreateNode(XmlNodeType.Element, "TokenNFSeExpire", "");

                usuarioConfiguracao.InnerText = usuario;
                senhaConfiguracao.InnerText = Criptografia.criptografaSenha(senha);
                clientIDConfiguracao.InnerText = clientID;
                clientSecretConfiguracao.InnerText = clientSecret;
                tokenConfiguracao.InnerText = token;
                tokenExpireConfiguracao.InnerText = tokenExpire.ToString();

                configuracao.AppendChild(usuarioConfiguracao);
                configuracao.AppendChild(senhaConfiguracao);
                configuracao.AppendChild(clientIDConfiguracao);
                configuracao.AppendChild(clientSecretConfiguracao);
                configuracao.AppendChild(tokenConfiguracao);
                configuracao.AppendChild(tokenExpireConfiguracao);

                xmlConfiguracao.Save($@"{Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), cnpj, "ConfiguracaoNFSeSoftplan.xml")}");
            }
            catch
            {
                throw;
            }
        }

        public Empresa RecuperarConfiguracaoNFSeSoftplan(string cnpj)
        {
            Empresa result = new Empresa();

            try
            {
                cnpj = Functions.OnlyNumbers(cnpj, ".-/").ToString();

                if (File.Exists($@"{Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), cnpj, "ConfiguracaoNFSeSoftplan.xml")}"))
                {
                    XmlDocument xmlConfiguracao = new XmlDocument();
                    xmlConfiguracao.Load($@"{Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), cnpj, "ConfiguracaoNFSeSoftplan.xml")}");

                    result.ClientID = xmlConfiguracao.GetElementsByTagName("ClientID")[0].InnerText;
                    result.ClientSecret = xmlConfiguracao.GetElementsByTagName("ClientSecret")[0].InnerText;
                    result.TokenNFse = xmlConfiguracao.GetElementsByTagName("TokenNFSe")[0].InnerText;
                    result.TokenNFSeExpire = Convert.ToDateTime(xmlConfiguracao.GetElementsByTagName("TokenNFSeExpire")[0].InnerText);
                    result.UsuarioWS = xmlConfiguracao.GetElementsByTagName("Usuario")[0].InnerText;
                    result.SenhaWS = Criptografia.descriptografaSenha(xmlConfiguracao.GetElementsByTagName("Senha")[0].InnerText);
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
#endif

        private void doneThread_FTP(Thread thread)
        {
            if (threads.Contains(thread))
            {
                threads.Remove(thread);
            }
        }

#endregion
    }
}
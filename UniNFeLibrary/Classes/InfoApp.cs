using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace UniNFeLibrary
{
    /// <summary>
    /// Classe possui métodos que retoram informações sobre o Aplicativo
    /// </summary>
    public class InfoApp
    {
        #region Constantes
        /// <summary>
        /// Nome para a pasta dos XML assinados
        /// </summary>
        public const string NomePastaXMLAssinado = "\\Assinado";
        public const string NomeArqERRUniNFe = "UniNFeErro_{0}.err";
        #endregion

        #region Propriedades Estaticas
        /// <summary>
        /// Sempre na execução do aplicativo (EXE) já defina este campo estático ou a classe não conseguirá pegar
        /// as informações do executável, tais como: Versão, Nome da aplicação, etc.
        /// Defina sempre com o conteúdo: Assembly.GetExecutingAssembly
        /// </summary>
        public static Assembly oAssemblyEXE;
        #endregion

        /// <summary>
        /// Retorna a versão do aplicativo 
        /// </summary>
        /// <param name="oAssembly">Passar sempre: Assembly.GetExecutingAssembly() pois ele vai pegar o Assembly do EXE ou DLL de onde está sendo chamado o método</param>
        /// <returns>string contendo a versão do Aplicativo</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        public static string Versao()
        {
            //Montar a versão do programa
            string versao;

            foreach (Attribute attr in Attribute.GetCustomAttributes(oAssemblyEXE))
            {
                if (attr.GetType() == typeof(AssemblyVersionAttribute))
                {
                    versao = ((AssemblyVersionAttribute)attr).Version;
                    break;
                }
            }
            string delimStr = ",=";
            char[] delimiter = delimStr.ToCharArray();
            string[] strAssembly = oAssemblyEXE.ToString().Split(delimiter);
            versao = strAssembly[2];

            return versao;
        }

        /// <summary>
        /// Retorna o nome do aplicativo 
        /// </summary>
        /// <param name="oAssembly">Passar sempre: Assembly.GetExecutingAssembly() pois ele vai pegar o Assembly do EXE ou DLL de onde está sendo chamado o método</param>
        /// <returns>string contendo o nome do Aplicativo</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>31/07/2009</date>
        public static string NomeAplicacao()
        {
            //Montar o nome da aplicação
            string Produto = string.Empty;

            foreach (Attribute attr in Attribute.GetCustomAttributes(oAssemblyEXE))
            {
                if (attr.GetType() == typeof(AssemblyProductAttribute))
                {
                    Produto = ((AssemblyProductAttribute)attr).Product;
                    break;
                }
            }

            return Produto;
        }

        /// <summary>
        /// Retorna a pasta do executável
        /// </summary>
        /// <returns>Retorna a pasta onde está o executável</returns>
        public static string PastaExecutavel()
        {
            return System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        }

        /// <summary>
        /// Retorna a pasta dos schemas para validar os XML´s
        /// </summary>
        /// <returns></returns>
        public static string PastaSchemas()
        {
            return PastaExecutavel() + "\\schemas";
        }

        /// <summary>
        /// Retorna o XML para salvar os parametros das telas
        /// </summary>
        public static string NomeArqXMLParams()
        {
            return PastaExecutavel() + "\\UniNFeParams.xml";
        }

        /// <summary>
        /// Grava XML com algumas informações do aplicativo, 
        /// dentre elas os dados do certificado digital configurado nos parâmetros, 
        /// versão, última modificação, etc.
        /// </summary>
        /// <param name="sArquivo">Pasta e nome do arquivo XML a ser gravado com as informações</param>
        /// <param name="oAssembly">Passar sempre: Assembly.GetExecutingAssembly() pois ele vai pegar o Assembly do EXE ou DLL de onde está sendo chamado o método</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        public void GravarXMLInformacoes(string sArquivo)
        {
            string cStat = "1";
            string xMotivo = "Consulta efetuada com sucesso";

            //Ler os dados do certificado digital
            string sSubject = "";
            string sValIni = "";
            string sValFin = "";

            CertificadoDigital oDigCert = new CertificadoDigital();

            oDigCert.PrepInfCertificado(ConfiguracaoApp.oCertificado);

            if (oDigCert.lLocalizouCertificado == true)
            {
                sSubject = oDigCert.sSubject;
                sValIni = oDigCert.dValidadeInicial.ToString();
                sValFin = oDigCert.dValidadeFinal.ToString();
            }
            else
            {
                cStat = "2";
                xMotivo = "Certificado digital não foi localizado";
            }

            //Gravar o XML com as informações do aplicativo
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
                XmlWriter oXmlGravar = XmlWriter.Create(sArquivo, oSettings);

                //Abrir o XML
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("retConsInf");
                oXmlGravar.WriteElementString("cStat", cStat);
                oXmlGravar.WriteElementString("xMotivo", xMotivo);

                //Dados do certificado digital
                oXmlGravar.WriteStartElement("DadosCertificado");
                oXmlGravar.WriteElementString("sSubject", sSubject);
                oXmlGravar.WriteElementString("dValIni", sValIni);
                oXmlGravar.WriteElementString("dValFin", sValFin);
                oXmlGravar.WriteEndElement(); //DadosCertificado                

                //Dados gerais do Aplicativo
                oXmlGravar.WriteStartElement("DadosUniNfe");
                oXmlGravar.WriteElementString("versao", InfoApp.Versao());
                oXmlGravar.WriteElementString("dUltModif", File.GetLastWriteTimeUtc(Application.ExecutablePath).ToString("dd/MM/yyyy hh:mm:ss"));
                oXmlGravar.WriteEndElement(); //DadosUniNfe

                //Dados das configurações do aplicativo
                oXmlGravar.WriteStartElement("nfe_configuracoes");
                oXmlGravar.WriteElementString("PastaBackup", ConfiguracaoApp.cPastaBackup);
                oXmlGravar.WriteEndElement(); //nfe_configuracoes

                //Finalizar o XML
                oXmlGravar.WriteEndElement(); //retConsInf
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();
            }
            catch (Exception ex)
            {
                ///
                /// danasa 8-2009
                /// 
                Auxiliar oAux = new Auxiliar();
                oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(sArquivo) + ".err", ex.Message);
            }
        }

        /// <summary>
        /// Verifica se a aplicação já está executando ou não
        /// </summary>
        /// <param name="oOneMutex">Objeto do tipo Mutex para ter como retorno para conseguir encerrar o mutex na finalização do aplicativo</param>
        /// <returns>True=Aplicação está executando</returns>
        /// <date>31/07/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static Boolean AppExecutando(ref System.Threading.Mutex oOneMutex)
        {
            ConfiguracaoApp.CarregarDados();

            // Pegar o nome da empresa do certificado para utilizar na verificação do Mutex
            // Wandrey 27/11/2008
            String nomeEmpresaCertificado = InfoApp.NomeAplicacao().ToUpper();
            String nomePastaEnvio = "";
            String nomePastaEnvioDemo = "";
            if (ConfiguracaoApp.oCertificado != null)
            {
                nomeEmpresaCertificado = ConfiguracaoApp.oCertificado.Subject;
                int nPosI = nomeEmpresaCertificado.ToUpper().IndexOf("CN=");
                if (nPosI >= 0)
                {
                    int nPosF = nomeEmpresaCertificado.ToUpper().IndexOf(",", nPosI);
                    if (nPosF <= 0)
                    {
                        nPosF = nomeEmpresaCertificado.Length;
                    }
                    nomeEmpresaCertificado = nomeEmpresaCertificado.Substring(nPosI + 3, nPosF - 3).Trim().ToUpper();
                }
                else
                {
                    //Não achou o "CN=" na string do nome do certificado, 
                    //então não será possível pegar o nome da empresa com cnpj
                    nomeEmpresaCertificado = InfoApp.NomeAplicacao().ToUpper();
                }

                //Acrescentar a pasta de envio ao Mutex, se for diferente ele vai permitir a execução do uninfe
                if (ConfiguracaoApp.vPastaXMLEnvio != "")
                {
                    nomePastaEnvio = ConfiguracaoApp.vPastaXMLEnvio;

                    //Tirar a unidade e os dois pontos do nome da pasta
                    int iPos = nomePastaEnvio.IndexOf(':') + 1;
                    if (iPos >= 0)
                    {
                        nomePastaEnvio = nomePastaEnvio.Substring(iPos, nomePastaEnvio.Length - iPos);
                    }
                    nomePastaEnvioDemo = nomePastaEnvio;

                    //tirar as barras de separação de pastas e subpastas
                    nomePastaEnvio = nomePastaEnvio.Replace("\\", "").Replace("/", "").ToUpper();
                }
            }

            // Verificar se o aplicativo já está rodando. Se estiver vai emitir um aviso e abortar
            // Pois só pode executar o aplicativo uma única vez por certificado/CNPJ.
            // Wandrey 27/11/2008
            System.Threading.Mutex oneMutex = null;
            String nomeMutex = nomeEmpresaCertificado.Trim() + nomePastaEnvio.Trim();

            try
            {
                oneMutex = System.Threading.Mutex.OpenExisting(nomeMutex);
            }
            catch (System.Threading.WaitHandleCannotBeOpenedException)
            {

            }

            if (oneMutex == null)
            {
                oneMutex = new System.Threading.Mutex(false, nomeMutex);
                oOneMutex = oneMutex;

                return false;
            }
            else
            {
                MessageBox.Show("Somente uma instância do " + InfoApp.NomeAplicacao() + " pode ser executada com o seguintes dados configurados:\r\n\r\n" +
                                "Certificado: " + nomeEmpresaCertificado + "\r\n\r\n" +
                                "Pasta Envio: " + nomePastaEnvioDemo + "\r\n\r\n" +
                                "Já tem uma instância com estes dados em execução.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                oneMutex.Close();

                return true;
            }
        }
    }
}

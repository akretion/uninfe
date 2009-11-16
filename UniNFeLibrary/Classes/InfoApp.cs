using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Threading;

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
                if (Path.GetExtension(sArquivo).ToLower() == ".txt")
                {
                    StringBuilder aTXT = new StringBuilder();
                    aTXT.AppendLine("cStat|" + cStat);
                    aTXT.AppendLine("xMotivo|" + xMotivo);
                    //Dados do certificado digital
                    aTXT.AppendLine("sSubject|" + sSubject);
                    aTXT.AppendLine("dValIni|" + sValIni);
                    aTXT.AppendLine("dValFin|" + sValFin);
                    //Dados gerais do Aplicativo
                    aTXT.AppendLine("versao|" + InfoApp.Versao());
                    aTXT.AppendLine("dUltModif|" + File.GetLastWriteTimeUtc(Application.ExecutablePath).ToString("dd/MM/yyyy hh:mm:ss"));
                    aTXT.AppendLine("PastaExecutavel|" + InfoApp.PastaExecutavel());
                    aTXT.AppendLine("NomeComputador|" + Environment.MachineName);
                    //Dados das configurações do aplicativo
                    aTXT.AppendLine("PastaBackup|" + (string.IsNullOrEmpty(ConfiguracaoApp.cPastaBackup) ? "" : ConfiguracaoApp.cPastaBackup));
                    aTXT.AppendLine("PastaXmlEmLote|" + (string.IsNullOrEmpty(ConfiguracaoApp.cPastaXMLEmLote) ? "" : ConfiguracaoApp.cPastaXMLEmLote));
                    aTXT.AppendLine("PastaXmlAssinado|" + (string.IsNullOrEmpty(ConfiguracaoApp.NomePastaXMLAssinado) ? "" : ConfiguracaoApp.NomePastaXMLAssinado));
                    aTXT.AppendLine("PastaValidar|" + (string.IsNullOrEmpty(ConfiguracaoApp.PastaValidar) ? "" : ConfiguracaoApp.PastaValidar));
                    aTXT.AppendLine("PastaXmlEnviado|" + (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLEnviado) ? "" : ConfiguracaoApp.vPastaXMLEnviado));
                    aTXT.AppendLine("PastaXmlEnvio|" + (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLEnvio) ? "" : ConfiguracaoApp.vPastaXMLEnvio));
                    aTXT.AppendLine("PastaXmlErro|" + (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLErro) ? "" : ConfiguracaoApp.vPastaXMLErro));
                    aTXT.AppendLine("PastaXmlRetorno|" + (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLRetorno) ? "" : ConfiguracaoApp.vPastaXMLRetorno));
                    aTXT.AppendLine("DiasParaLimpeza|" + ConfiguracaoApp.DiasLimpeza.ToString());
                    aTXT.AppendLine("DiretorioSalvarComo|" + ConfiguracaoApp.DiretorioSalvarComo.ToString());
                    aTXT.AppendLine("GravarRetornoTXTNFe|" + ConfiguracaoApp.GravarRetornoTXTNFe.ToString());
                    aTXT.AppendLine("AmbienteCodigo|" + ConfiguracaoApp.tpAmb.ToString());
                    aTXT.AppendLine("tpEmis|" + ConfiguracaoApp.tpEmis.ToString());
                    aTXT.AppendLine("UnidadeFederativaCodigo|" + ConfiguracaoApp.UFCod.ToString());

                    File.WriteAllText(sArquivo, aTXT.ToString(), Encoding.Default);
                }
                else
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
                    oXmlGravar.WriteElementString("PastaExecutavel", InfoApp.PastaExecutavel());
                    oXmlGravar.WriteElementString("NomeComputador", Environment.MachineName);
                    oXmlGravar.WriteEndElement(); //DadosUniNfe

                    //Dados das configurações do aplicativo
                    oXmlGravar.WriteStartElement("nfe_configuracoes");
                    oXmlGravar.WriteElementString("PastaBackup", (string.IsNullOrEmpty(ConfiguracaoApp.cPastaBackup) ? "" : ConfiguracaoApp.cPastaBackup));
                    oXmlGravar.WriteElementString("PastaXmlEmLote", (string.IsNullOrEmpty(ConfiguracaoApp.cPastaXMLEmLote) ? "" : ConfiguracaoApp.cPastaXMLEmLote));
                    oXmlGravar.WriteElementString("PastaXmlAssinado", (string.IsNullOrEmpty(ConfiguracaoApp.NomePastaXMLAssinado) ? "" : ConfiguracaoApp.NomePastaXMLAssinado));
                    oXmlGravar.WriteElementString("PastaValidar", (string.IsNullOrEmpty(ConfiguracaoApp.PastaValidar) ? "" : ConfiguracaoApp.PastaValidar));
                    oXmlGravar.WriteElementString("PastaXmlEnviado", (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLEnviado) ? "" : ConfiguracaoApp.vPastaXMLEnviado));
                    oXmlGravar.WriteElementString("PastaXmlEnvio", (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLEnvio) ? "" : ConfiguracaoApp.vPastaXMLEnvio));
                    oXmlGravar.WriteElementString("PastaXmlErro", (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLErro) ? "" : ConfiguracaoApp.vPastaXMLErro));
                    oXmlGravar.WriteElementString("PastaXmlRetorno", (string.IsNullOrEmpty(ConfiguracaoApp.vPastaXMLRetorno) ? "" : ConfiguracaoApp.vPastaXMLRetorno));
                    oXmlGravar.WriteElementString("DiasParaLimpeza", ConfiguracaoApp.DiasLimpeza.ToString());
                    oXmlGravar.WriteElementString("DiretorioSalvarComo", ConfiguracaoApp.DiretorioSalvarComo.ToString());
                    oXmlGravar.WriteElementString("GravarRetornoTXTNFe", ConfiguracaoApp.GravarRetornoTXTNFe.ToString());
                    oXmlGravar.WriteElementString("AmbienteCodigo", ConfiguracaoApp.tpAmb.ToString());
                    oXmlGravar.WriteElementString("tpEmis", ConfiguracaoApp.tpEmis.ToString());
                    oXmlGravar.WriteElementString("UnidadeFederativaCodigo", ConfiguracaoApp.UFCod.ToString());
                    oXmlGravar.WriteEndElement(); //nfe_configuracoes

                    //Finalizar o XML
                    oXmlGravar.WriteEndElement(); //retConsInf
                    oXmlGravar.WriteEndDocument();
                    oXmlGravar.Flush();
                    oXmlGravar.Close();
                }
            }
            catch (Exception ex)
            {
                ///
                /// danasa 8-2009
                /// 
                Auxiliar oAux = new Auxiliar();
                oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(sArquivo) + ".err", (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
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

            bool Executando = false;
            if (oneMutex == null)
            {
                oneMutex = new System.Threading.Mutex(false, nomeMutex);
                oOneMutex = oneMutex;
                Executando = false;
            }
            else
            {
                oneMutex.Close();
                Executando = true;
            }

            //Alem de verificar no Mutex, vamos fazer uma segunda verificação se passar pelo Mutax para 
            //ter certeza que não está rodando, pois o mutax falha quando é executado por terminal server, quando
            //é executado em duas máquinas diferentes. A opção seguinte dá a certeza.
            bool OcorreuErro = false;
            if (!Executando)
            {
                try
                {
                    Executando = AppExecutandoAux();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu uma falha ao tentar verificar se uma instância do " + InfoApp.NomeAplicacao() + " já está sendo executada.\r\n\r\n" +
                        "Erro ocorrido:\r\n" +
                        ex.Message + "\r\n\r\n" +
                        "O aplicativo não será inicializado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    OcorreuErro = true;
                }
            }

            if (Executando)
            {
                MessageBox.Show("Somente uma instância do " + InfoApp.NomeAplicacao() + " pode ser executada com o seguintes dados configurados:\r\n\r\n" +
                                "Certificado: " + nomeEmpresaCertificado + "\r\n\r\n" +
                                "Pasta Envio: " + nomePastaEnvioDemo + "\r\n\r\n" +
                                "Já tem uma instância com estes dados em execução.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (OcorreuErro)
                Executando = true;

            return Executando;
        }

        /// <summary>
        /// Uma segunda forma de verificar se o UniNFe já está rodando é gerar o XML de consulta de informações
        /// Se tiver retorno é pq já está sendo executando. Isso evita falhas quando é Terminal Server, quando está rodando o EXE em uma 
        /// pasta diferente o EXE que está sendo executado neste exato momento mas está apontando para a mesma pasta de envio.
        /// </summary>
        /// <returns>Se está executando ou não</returns>
        /// <date>16/09/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        private static bool AppExecutandoAux()
        {
            bool Executando = false;

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
                string ArquivoConsulta = ConfiguracaoApp.vPastaXMLEnvio + "\\UniNfeRodando" + ExtXml.ConsInf;
                string ArquivoRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\UniNfeRodando-ret-cons-inf.xml";
                string ArquivoRetornoErr = ConfiguracaoApp.vPastaXMLRetorno + "\\UniNfeRodando-ret-cons-inf.err";

                if (File.Exists(ArquivoConsulta) && !Auxiliar.FileInUse(ArquivoConsulta))
                    File.Delete(ArquivoConsulta);

                if (File.Exists(ArquivoRetorno) && !Auxiliar.FileInUse(ArquivoRetorno))
                    File.Delete(ArquivoRetorno);

                //Agora vamos criar um XML Writer
                oXmlGravar = XmlWriter.Create(ArquivoConsulta, oSettings);

                //Abrir o XML
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("ConsInf");
                oXmlGravar.WriteElementString("xServ", "CONS-INF");
                oXmlGravar.WriteEndElement(); //ConsInf
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();

                int Contador = 0;
                while (Contador < 10)
                {
                    if (File.Exists(ArquivoRetorno) || File.Exists(ArquivoRetornoErr))
                    {
                        if (File.Exists(ArquivoRetorno))
                            File.Delete(ArquivoRetorno);

                        if (File.Exists(ArquivoRetornoErr))
                            File.Delete(ArquivoRetornoErr);

                        Executando = true;
                        break;
                    }

                    Thread.Sleep(1000);

                    Contador++;
                }

                if (!Executando)
                {
                    if (!Auxiliar.FileInUse(ArquivoConsulta))
                        File.Delete(ArquivoConsulta);
                }
            }
            catch (Exception ex)
            {
                if (oXmlGravar != null)
                {
                    oXmlGravar.Close();
                }

                Executando = false;

                throw (ex);
            }

            return Executando;
        }
    }
}

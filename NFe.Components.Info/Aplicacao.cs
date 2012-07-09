using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFe.Settings;
using NFe.Certificado;
using System.Threading;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace NFe.Components.Info
{
    public class Aplicacao
    {
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            string cStat = "1";
            string xMotivo = "Consulta efetuada com sucesso";

            //Ler os dados do certificado digital
            string sSubject = "";
            string sValIni = "";
            string sValFin = "";

            CertificadoDigital oDigCert = new CertificadoDigital();

            oDigCert.PrepInfCertificado(Empresa.Configuracoes[emp].X509Certificado);

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

            //danasa 22/7/2011
            //pega a data da ultima modificacao do 'uninfe.exe' diretamente porque pode ser que esteja sendo executado o servico
            //então, precisamos dos dados do uninfe.exe e não do servico
            string dtUltModif = File.GetLastWriteTimeUtc(Path.Combine(Propriedade.PastaExecutavel, "uninfe.exe")).ToString("dd/MM/yyyy hh:mm:ss");

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
                    aTXT.AppendLine("versao|" + Propriedade.Versao);
                    aTXT.AppendLine("dUltModif|" + dtUltModif);
                    aTXT.AppendLine("PastaExecutavel|" + Propriedade.PastaExecutavel);
                    aTXT.AppendLine("NomeComputador|" + Environment.MachineName);
                    //danasa 22/7/2011
                    aTXT.AppendLine("ExecutandoPeloServico|" + Propriedade.ServicoRodando.ToString());
                    //Dados das configurações do aplicativo
                    aTXT.AppendLine("PastaBackup|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaBackup) ? "" : Empresa.Configuracoes[emp].PastaBackup));
                    aTXT.AppendLine("PastaXmlEmLote|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvioEmLote) ? "" : Empresa.Configuracoes[emp].PastaEnvioEmLote));
                    aTXT.AppendLine("PastaXmlAssinado|" + (string.IsNullOrEmpty(Propriedade.NomePastaXMLAssinado) ? "" : Propriedade.NomePastaXMLAssinado));
                    aTXT.AppendLine("PastaValidar|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaValidar) ? "" : Empresa.Configuracoes[emp].PastaValidar));
                    aTXT.AppendLine("PastaXmlEnviado|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnviado) ? "" : Empresa.Configuracoes[emp].PastaEnviado));
                    aTXT.AppendLine("PastaXmlEnvio|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvio) ? "" : Empresa.Configuracoes[emp].PastaEnvio));
                    aTXT.AppendLine("PastaXmlErro|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaErro) ? "" : Empresa.Configuracoes[emp].PastaErro));
                    aTXT.AppendLine("PastaXmlRetorno|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaRetorno) ? "" : Empresa.Configuracoes[emp].PastaRetorno));
                    aTXT.AppendLine("DiasParaLimpeza|" + Empresa.Configuracoes[emp].DiasLimpeza.ToString());
                    aTXT.AppendLine("DiretorioSalvarComo|" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString());
                    aTXT.AppendLine("GravarRetornoTXTNFe|" + Empresa.Configuracoes[emp].GravarRetornoTXTNFe.ToString());
                    aTXT.AppendLine("AmbienteCodigo|" + Empresa.Configuracoes[emp].tpAmb.ToString());
                    aTXT.AppendLine("tpEmis|" + Empresa.Configuracoes[emp].tpEmis.ToString());
                    aTXT.AppendLine("UnidadeFederativaCodigo|" + Empresa.Configuracoes[emp].UFCod.ToString());
                    aTXT.AppendLine("TempoConsulta|" + Empresa.Configuracoes[emp].TempoConsulta.ToString());

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
                    oXmlGravar.WriteElementString("versao", Propriedade.Versao);
                    oXmlGravar.WriteElementString("dUltModif", dtUltModif);
                    oXmlGravar.WriteElementString("PastaExecutavel", Propriedade.PastaExecutavel);
                    oXmlGravar.WriteElementString("NomeComputador", Environment.MachineName);
                    //danasa 22/7/2011
                    oXmlGravar.WriteElementString("ExecutandoPeloServico", Propriedade.ServicoRodando.ToString());
                    oXmlGravar.WriteEndElement(); //DadosUniNfe

                    //Dados das configurações do aplicativo
                    oXmlGravar.WriteStartElement("nfe_configuracoes");
                    oXmlGravar.WriteElementString("PastaBackup", (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaBackup) ? "" : Empresa.Configuracoes[emp].PastaBackup));
                    oXmlGravar.WriteElementString("PastaXmlEmLote", (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvioEmLote) ? "" : Empresa.Configuracoes[emp].PastaEnvioEmLote));
                    oXmlGravar.WriteElementString("PastaXmlAssinado", (string.IsNullOrEmpty(Propriedade.NomePastaXMLAssinado) ? "" : Propriedade.NomePastaXMLAssinado));
                    oXmlGravar.WriteElementString("PastaValidar", (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaValidar) ? "" : Empresa.Configuracoes[emp].PastaValidar));
                    oXmlGravar.WriteElementString("PastaXmlEnviado", (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnviado) ? "" : Empresa.Configuracoes[emp].PastaEnviado));
                    oXmlGravar.WriteElementString("PastaXmlEnvio", (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvio) ? "" : Empresa.Configuracoes[emp].PastaEnvio));
                    oXmlGravar.WriteElementString("PastaXmlErro", (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaErro) ? "" : Empresa.Configuracoes[emp].PastaErro));
                    oXmlGravar.WriteElementString("PastaXmlRetorno", (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaRetorno) ? "" : Empresa.Configuracoes[emp].PastaRetorno));
                    oXmlGravar.WriteElementString("DiasParaLimpeza", Empresa.Configuracoes[emp].DiasLimpeza.ToString());
                    oXmlGravar.WriteElementString("DiretorioSalvarComo", Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString());
                    oXmlGravar.WriteElementString("GravarRetornoTXTNFe", Empresa.Configuracoes[emp].GravarRetornoTXTNFe.ToString());
                    oXmlGravar.WriteElementString("AmbienteCodigo", Empresa.Configuracoes[emp].tpAmb.ToString());
                    oXmlGravar.WriteElementString("tpEmis", Empresa.Configuracoes[emp].tpEmis.ToString());
                    oXmlGravar.WriteElementString("UnidadeFederativaCodigo", Empresa.Configuracoes[emp].UFCod.ToString());
                    oXmlGravar.WriteElementString("TempoConsulta", Empresa.Configuracoes[emp].TempoConsulta.ToString());
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
                oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(sArquivo) + ".err", ex.Message);
            }
        }

        #region AppExecutando()

        /// <summary>
        /// Verifica se a aplicação já está executando ou não
        /// </summary>
        /// <param name="oOneMutex">Objeto do tipo Mutex para ter como retorno para conseguir encerrar o mutex na finalização do aplicativo</param>
        /// <returns>True=Aplicação está executando</returns>
        /// <date>31/07/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static Boolean AppExecutando(bool chamadaPeloUniNFe, ref System.Threading.Mutex oOneMutex)
        {
            Propriedade.ExecutandoPeloUniNFe = chamadaPeloUniNFe;

            bool executando = false;
            String nomePastaEnvio = "";
            String nomePastaEnvioDemo = "";

            try
            {
                Empresa.CarregaConfiguracao();

                //danasa 22/7/2011
                //se chamadaPeloUniNFe=false, é porque está sendo executado pelo servico
                //se chamadaPeloUniNFe=true, é porque está sendo executado pelo 'uninfe.exe'
                if (chamadaPeloUniNFe)
                {
                    Empresa oEmpresa = null;

                    if (Empresa.Configuracoes.Count > 0)
                    {
                        oEmpresa = Empresa.Configuracoes[0];

                        //Pegar a pasta de envio, se já tiver algum UniNFe configurado para uma determinada pasta de envio os demais não podem
                        if (oEmpresa.PastaEnvio != "")
                        {
                            nomePastaEnvio = oEmpresa.PastaEnvio;

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
                    // Pois só pode executar o aplicativo uma única vez para cada pasta de envio.
                    // Wandrey 27/11/2008
                    System.Threading.Mutex oneMutex = null;
                    String nomeMutex = Propriedade.NomeAplicacao.ToUpper() + nomePastaEnvio.Trim();

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
                        executando = false;
                    }
                    else
                    {
                        oneMutex.Close();
                        executando = true;
                    }
                }
            }
            catch
            {
                //Não preciso retornar nada, somente evito fechar o aplicativo
                //Esta exceção pode ocorrer quando não existe nenhuma empresa cadastrada
                //ainda, ou seja, é a primeira vez que estamos entrando no aplicativo
            }

            if (executando && chamadaPeloUniNFe)//danasa 22/7/2011
            {
                MessageBox.Show("Somente uma instância do " + Propriedade.NomeAplicacao + " pode ser executada com a seguinte pasta de envio configurada: \r\n\r\n" +
                                "Pasta Envio: " + nomePastaEnvioDemo + "\r\n\r\n" +
                                "Já existe uma instância em execução.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return executando;
        }
        #endregion

        #region AppExecutando()
        /// <summary>
        /// Verifica se a aplicação já está executando ou não
        /// </summary>
        /// <returns>True=Aplicação está executando</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 21/07/2011
        /// </remarks>
        public static Boolean AppExecutando()
        {
            Empresa.CarregaConfiguracao();

            bool executando = false;

            string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (System.Diagnostics.Process.GetProcessesByName(procName).Length > 1)
            {
                executando = true;
            }

            if (executando)
                MessageBox.Show("Somente uma instância do " + Propriedade.NomeAplicacao + " pode ser executada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Empresa.ExisteErroDiretorio)
                System.Windows.Forms.MessageBox.Show("Ocorreu um erro ao efetuar a leitura das configurações da empresa. " +
                                "Por favor entre na tela de configurações da(s) empresa(s) listada(s) abaixo na aba \"Pastas\" e reconfigure-as.\r\n\r\n" + Empresa.ErroCaminhoDiretorio, "Atenção", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            return executando;
        }
        #endregion
    }
}

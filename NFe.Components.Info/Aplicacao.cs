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
        /// Grava XML com algumas informações do aplicativo, dentre elas os dados do certificado digital configurado nos parâmetros, versão, última modificação, etc.
        /// </summary>
        /// <param name="sArquivo">Pasta e nome do arquivo XML a ser gravado com as informações</param>
        public void GravarXMLInformacoes(string sArquivo)
        {
            int emp = Empresas.FindEmpresaByThread();

            string cStat = "1";
            string xMotivo = "Consulta efetuada com sucesso";

            //Ler os dados do certificado digital
            string sSubject = "";
            string sValIni = "";
            string sValFin = "";

            CertificadoDigital cert = new CertificadoDigital();

            if (cert.PrepInfCertificado(Empresas.Configuracoes[emp]))
            {
                sSubject = cert.sSubject;
                sValIni = cert.dValidadeInicial.ToString();
                sValFin = cert.dValidadeFinal.ToString();
            }
            else
            {
                cStat = "2";
                xMotivo = "Certificado digital não foi localizado";
            }

            //danasa 22/7/2011
            //pega a data da ultima modificacao do 'uninfe.exe' diretamente porque pode ser que esteja sendo executado o servico
            //então, precisamos dos dados do uninfe.exe e não do servico
            string dtUltModif;
            URLws item;
            string tipo = "";

            dtUltModif = File.GetLastWriteTime(Propriedade.NomeAplicacao + ".exe").ToString("dd/MM/yyyy - HH:mm:ss");

            //Gravar o XML com as informações do aplicativo
            try
            {
                bool isXml = false;
                object oXmlGravar;

                if (Path.GetExtension(sArquivo).ToLower() == ".txt")
                {
                    oXmlGravar = new System.IO.StringWriter();
                }
                else
                {
                    isXml = true;

                    XmlWriterSettings oSettings = new XmlWriterSettings();
                    UTF8Encoding c = new UTF8Encoding(true);

                    //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
                    oSettings.Encoding = c;
                    oSettings.Indent = true;
                    oSettings.IndentChars = "";
                    oSettings.NewLineOnAttributes = false;
                    oSettings.OmitXmlDeclaration = false;

                    //Agora vamos criar um XML Writer
                    oXmlGravar = XmlWriter.Create(sArquivo, oSettings);
                }
                //Abrir o XML
                if (isXml)
                {
                    ((XmlWriter)oXmlGravar).WriteStartDocument();
                    ((XmlWriter)oXmlGravar).WriteStartElement("retConsInf");
                }
                Functions.GravaTxtXml(oXmlGravar, "cStat", cStat);
                Functions.GravaTxtXml(oXmlGravar, "xMotivo", xMotivo);

                //Dados do certificado digital
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("DadosCertificado");
                Functions.GravaTxtXml(oXmlGravar, "sSubject", sSubject);
                Functions.GravaTxtXml(oXmlGravar, "dValIni", sValIni);
                Functions.GravaTxtXml(oXmlGravar, "dValFin", sValFin);
                if (isXml) ((XmlWriter)oXmlGravar).WriteEndElement(); //DadosCertificado                

                //Dados gerais do Aplicativo
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("DadosUniNfe");
                Functions.GravaTxtXml(oXmlGravar, "versao", Propriedade.Versao);
                Functions.GravaTxtXml(oXmlGravar, "dUltModif", dtUltModif);
                Functions.GravaTxtXml(oXmlGravar, "PastaExecutavel", Propriedade.PastaExecutavel);
                Functions.GravaTxtXml(oXmlGravar, "NomeComputador", Environment.MachineName);
                //danasa 22/7/2011
                Functions.GravaTxtXml(oXmlGravar, "ExecutandoPeloServico", Propriedade.ServicoRodando.ToString());
                if (isXml) ((XmlWriter)oXmlGravar).WriteEndElement(); //DadosUniNfe

                //Dados das configurações do aplicativo
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement(NFeStrConstants.nfe_configuracoes);

                bool hasFTP = false;
                foreach (var pT in Empresas.Configuracoes[emp].GetType().GetProperties())
                {
                    if (pT.CanWrite)
                    {
                        bool ok = true;
                        var t0 = pT.GetCustomAttributes(typeof(AttributeTipoAplicacao), false);
                        if (t0 != null && t0.Length > 0)
                        {
                            ok = false;
                            ///
                            /// grava apenas a propriedade que está definida para ser usada pelo ambiente
                            foreach (var t1 in t0)
                            {
                                if (Propriedade.TipoAplicativo == ((AttributeTipoAplicacao)t1).Aplicacao)
                                {
                                    ok = true;
                                    break;
                                }
                            }
                        }

                        if (ok)
                        {
                            if (isXml)
                            {
                                if (!hasFTP && pT.Name.StartsWith("FTP"))
                                {
                                    ((XmlWriter)oXmlGravar).WriteStartElement("FTP");
                                    hasFTP = true;
                                }
                                else
                                    if (hasFTP && !pT.Name.StartsWith("FTP"))
                                    {
                                        ((XmlWriter)oXmlGravar).WriteEndElement();
                                        hasFTP = false;
                                    }
                            }
                            object v = pT.GetValue(Empresas.Configuracoes[emp], null);
                            NFe.Components.Functions.GravaTxtXml(oXmlGravar, pT.Name, v == null ? "" : v.ToString());
                        }
                    }
                }
                if (hasFTP && isXml) ((XmlWriter)oXmlGravar).WriteEndElement();

                ///
                /// o ERP poderá verificar se determinado servico está definido no UniNFe
                /// 
                foreach (webServices list in WebServiceProxy.webServicesList)
                {
                    if (list.ID == Empresas.Configuracoes[emp].UnidadeFederativaCodigo ||
                        list.UF == "DPEC" ||
                        list.UF == "SCAN")
                    {
                        if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement(list.UF);
                        if (Empresas.Configuracoes[emp].AmbienteCodigo == 2)
                        {
                            item = list.LocalHomologacao;
                            if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("Homologacao");
                            else tipo = list.UF + ".Homologacao.";
                        }
                        else
                        {
                            item = list.LocalProducao;
                            if (isXml)((XmlWriter)oXmlGravar).WriteStartElement("Producao");
                            else tipo = list.UF + ".Producao.";
                        }
                        switch (Propriedade.TipoAplicativo)
                        {
                            case TipoAplicativo.Nfse:
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.CancelarNfse.ToString(), (!string.IsNullOrEmpty(item.CancelarNfse)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.ConsultarLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarLoteRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.ConsultarNfse.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfse)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.ConsultarNfsePorRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfsePorRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.ConsultarSituacaoLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarSituacaoLoteRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.RecepcionarLoteRps.ToString(), (!string.IsNullOrEmpty(item.RecepcionarLoteRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.ConsultarURLNfse.ToString(), (!string.IsNullOrEmpty(item.ConsultarURLNfse)).ToString());
                                break;

                            case TipoAplicativo.Nfe:
                                Functions.GravaTxtXml(oXmlGravar, tipo + "NFeConsulta", (!string.IsNullOrEmpty(item.NFeConsulta)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRecepcao", (!string.IsNullOrEmpty(item.NFeRecepcao)).ToString());
                                if (list.UF != "DPEC")
                                {
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRecepcaoEvento", (!string.IsNullOrEmpty(item.NFeRecepcaoEvento)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeConsultaCadastro", (!string.IsNullOrEmpty(item.NFeConsultaCadastro)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeConsultaNFeDest", (!string.IsNullOrEmpty(item.NFeConsultaNFeDest)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeDownload", (!string.IsNullOrEmpty(item.NFeDownload)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeInutilizacao", (!string.IsNullOrEmpty(item.NFeInutilizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeManifDest", (!string.IsNullOrEmpty(item.NFeManifDest)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeStatusServico", (!string.IsNullOrEmpty(item.NFeStatusServico)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRegistroDeSaida", (!string.IsNullOrEmpty(item.NFeRegistroDeSaida)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRegistroDeSaidaCancelamento", (!string.IsNullOrEmpty(item.NFeRegistroDeSaidaCancelamento)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeAutorizacao", (!string.IsNullOrEmpty(item.NFeAutorizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRetAutorizacao", (!string.IsNullOrEmpty(item.NFeRetAutorizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRecepcao", (!string.IsNullOrEmpty(item.MDFeRecepcao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRetRecepcao", (!string.IsNullOrEmpty(item.MDFeRetRecepcao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeConsulta", (!string.IsNullOrEmpty(item.MDFeConsulta)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeStatusServico", (!string.IsNullOrEmpty(item.MDFeStatusServico)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRecepcaoEvento", (!string.IsNullOrEmpty(item.MDFeRecepcaoEvento)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeRecepcaoEvento", (!string.IsNullOrEmpty(item.CTeRecepcaoEvento)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeConsultaCadastro", (!string.IsNullOrEmpty(item.CTeConsultaCadastro)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeInutilizacao", (!string.IsNullOrEmpty(item.CTeInutilizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeStatusServico", (!string.IsNullOrEmpty(item.CTeStatusServico)).ToString());
                                }
                                break;
                        }
                        if (isXml)
                        {
                            ((XmlWriter)oXmlGravar).WriteEndElement();   //Ambiente
                            ((XmlWriter)oXmlGravar).WriteEndElement();   //list.UF
                        }
                    }
                }
                //Finalizar o XML
                if (isXml)
                {
                    ((XmlWriter)oXmlGravar).WriteEndElement(); //nfe_configuracoes
                    ((XmlWriter)oXmlGravar).WriteEndElement(); //retConsInf
                    ((XmlWriter)oXmlGravar).WriteEndDocument();
                    ((XmlWriter)oXmlGravar).Flush();
                    ((XmlWriter)oXmlGravar).Close();
                }
                else
                {
                    ((StringWriter)oXmlGravar).Flush();
                    File.WriteAllText(sArquivo, ((StringWriter)oXmlGravar).GetStringBuilder().ToString(), Encoding.Default);
                    ((StringWriter)oXmlGravar).Close();
                }
            }
            catch (Exception ex)
            {
                Functions.DeletarArquivo(sArquivo);
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
                Empresas.CarregaConfiguracao();

                #region Ticket: #110
                /*
                 * Marcelo
                 * 03/06/2013
                 */
                string podeExecutar = Empresas.CanRun();
                if (!String.IsNullOrEmpty(podeExecutar))
                    return true;

                // Se puder executar a aplicação aqui será recriado todos os arquivos de .lock, 
                // pois podem ter sofridos alterações de configurações nas pastas
                Empresas.CreateLockFile();
                #endregion

                //danasa 22/7/2011
                //se chamadaPeloUniNFe=false, é porque está sendo executado pelo servico
                //se chamadaPeloUniNFe=true, é porque está sendo executado pelo 'uninfe.exe'
                if (chamadaPeloUniNFe)
                {
                    Empresa oEmpresa = null;

                    if (Empresas.Configuracoes.Count > 0)
                    {
                        oEmpresa = Empresas.Configuracoes[0];

                        //Pegar a pasta de envio, se já tiver algum UniNFe configurado para uma determinada pasta de envio os demais não podem
                        if (oEmpresa.PastaXmlEnvio != "")
                        {
                            nomePastaEnvio = oEmpresa.PastaXmlEnvio;

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
        public static Boolean AppExecutando(bool silencioso, bool novoSistema=false)
        {
            Empresas.CarregaConfiguracao();

            #region Ticket: #110
            /*
             * Marcelo
             * 03/06/2013
             */
            string podeExecutar = Empresas.CanRun(!novoSistema);
            if (!String.IsNullOrEmpty(podeExecutar))
                return true;

            // Se puder executar a aplicação aqui será recriado todos os arquivos de .lock, 
            // pois podem ter sofridos alterações de configurações nas pastas
            Empresas.CreateLockFile();
            #endregion

            bool executando = false;

            string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (System.Diagnostics.Process.GetProcessesByName(procName).Length > 1)
            {
                executando = true;
            }

            if (!silencioso && !novoSistema)
            {
                if (executando)
                    MessageBox.Show("Somente uma instância do " + Propriedade.NomeAplicacao + " pode ser executada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (Empresas.ExisteErroDiretorio)
                    System.Windows.Forms.MessageBox.Show("Ocorreu um erro ao efetuar a leitura das configurações da empresa. " +
                                    "Por favor entre na tela de configurações da(s) empresa(s) listada(s) abaixo na aba \"Pastas\" e reconfigure-as.\r\n\r\n" + Empresas.ErroCaminhoDiretorio, "Atenção", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return executando;
        }
        #endregion
    }
}

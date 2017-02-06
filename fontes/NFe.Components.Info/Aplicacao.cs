using System;
using System.Text;
using NFe.Settings;
using NFe.Certificado;
using System.IO;
using System.Xml;

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
                if (!Empresas.Configuracoes[emp].UsaCertificado)
                    xMotivo = "Empresa sem certificado digital informado e/ou não necessário";
                else
                {
                    cStat = "2";
                    xMotivo = "Certificado digital não foi localizado";
                }
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
                Functions.GravaTxtXml(oXmlGravar, NFe.Components.TpcnResources.cStat.ToString(), cStat);
                Functions.GravaTxtXml(oXmlGravar, NFe.Components.TpcnResources.xMotivo.ToString(), xMotivo);

                //Dados do certificado digital
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("DadosCertificado");
                Functions.GravaTxtXml(oXmlGravar, "sSubject", sSubject);
                Functions.GravaTxtXml(oXmlGravar, "dValIni", sValIni);
                Functions.GravaTxtXml(oXmlGravar, "dValFin", sValFin);
                if (isXml) ((XmlWriter)oXmlGravar).WriteEndElement(); //DadosCertificado                

                //Dados gerais do Aplicativo
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("DadosUniNfe");
                Functions.GravaTxtXml(oXmlGravar, NFe.Components.TpcnResources.versao.ToString(), Propriedade.Versao);
                Functions.GravaTxtXml(oXmlGravar, "dUltModif", dtUltModif);
                Functions.GravaTxtXml(oXmlGravar, "PastaExecutavel", Propriedade.PastaExecutavel);
                Functions.GravaTxtXml(oXmlGravar, "NomeComputador", Environment.MachineName);
                //danasa 22/7/2011
                Functions.GravaTxtXml(oXmlGravar, "ExecutandoPeloServico", Propriedade.ServicoRodando.ToString());
                Functions.GravaTxtXml(oXmlGravar, "ConexaoInternet", Functions.IsConnectedToInternet().ToString());

                if (isXml) ((XmlWriter)oXmlGravar).WriteEndElement(); //DadosUniNfe

                //Dados das configurações do aplicativo
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement(NFeStrConstants.nfe_configuracoes);
                //Functions.GravaTxtXml(oXmlGravar, NFe.Components.NFeStrConstants.DiretorioSalvarComo, Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString());

                bool hasFTP = false;
                foreach (var pT in Empresas.Configuracoes[emp].GetType().GetProperties())
                {
                    if (pT.CanWrite)
                    {
                        if (pT.Name.Equals("diretorioSalvarComo")) continue;

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
                if (hasFTP && isXml) ((XmlWriter)oXmlGravar).WriteEndElement();

                ///
                /// o ERP poderá verificar se determinado servico está definido no UniNFe
                /// 
                foreach (webServices list in WebServiceProxy.webServicesList)
                {
                    if (list.ID == Empresas.Configuracoes[emp].UnidadeFederativaCodigo)
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
                            if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("Producao");
                            else tipo = list.UF + ".Producao.";
                        }
                        switch (Empresas.Configuracoes[emp].Servico)
                        {
                            case TipoAplicativo.Nfse:
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.NFSeCancelar.ToString(), (!string.IsNullOrEmpty(item.CancelarNfse)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.NFSeConsultarLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarLoteRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.NFSeConsultar.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfse)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.NFSeConsultarPorRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfsePorRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.NFSeConsultarSituacaoLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarSituacaoLoteRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.NFSeRecepcionarLoteRps.ToString(), (!string.IsNullOrEmpty(item.RecepcionarLoteRps)).ToString());
                                Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.NFSeConsultarURL.ToString(), (!string.IsNullOrEmpty(item.ConsultarURLNfse)).ToString());
                                break;

                            default:
                                if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.NFCe ||
                                    Empresas.Configuracoes[emp].Servico == TipoAplicativo.Nfe ||
                                    Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                {
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeConsulta", (!string.IsNullOrEmpty(item.NFeConsulta)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRecepcao", (!string.IsNullOrEmpty(item.NFeRecepcao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRecepcaoEvento", (!string.IsNullOrEmpty(item.NFeRecepcaoEvento)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeConsultaCadastro", (!string.IsNullOrEmpty(item.NFeConsultaCadastro)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFeDownload.ToString(), (!string.IsNullOrEmpty(item.NFeDownload)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeInutilizacao", (!string.IsNullOrEmpty(item.NFeInutilizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeManifDest", (!string.IsNullOrEmpty(item.NFeManifDest)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeStatusServico", (!string.IsNullOrEmpty(item.NFeStatusServico)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeAutorizacao", (!string.IsNullOrEmpty(item.NFeAutorizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRetAutorizacao", (!string.IsNullOrEmpty(item.NFeRetAutorizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "DFeRecepcao", (!string.IsNullOrEmpty(item.DFeRecepcao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.LMCAutorizacao.ToString(), (!string.IsNullOrEmpty(item.LMCAutorizacao)).ToString());
                                }
                                if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.MDFe ||
                                    Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                {
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRecepcao", (!string.IsNullOrEmpty(item.MDFeRecepcao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRetRecepcao", (!string.IsNullOrEmpty(item.MDFeRetRecepcao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeConsulta", (!string.IsNullOrEmpty(item.MDFeConsulta)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeStatusServico", (!string.IsNullOrEmpty(item.MDFeStatusServico)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.MDFeRecepcaoEvento.ToString(), (!string.IsNullOrEmpty(item.MDFeRecepcaoEvento)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.MDFeConsultaNaoEncerrado.ToString(), (!string.IsNullOrEmpty(item.MDFeNaoEncerrado)).ToString());
                                }
                                if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.Cte ||
                                    Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                {
                                    Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.CTeRecepcaoEvento.ToString(), (!string.IsNullOrEmpty(item.CTeRecepcaoEvento)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeConsultaCadastro", (!string.IsNullOrEmpty(item.CTeConsultaCadastro)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeInutilizacao", (!string.IsNullOrEmpty(item.CTeInutilizacao)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeStatusServico", (!string.IsNullOrEmpty(item.CTeStatusServico)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + "CTeDistribuicaoDFe", (!string.IsNullOrEmpty(item.CTeDistribuicaoDFe)).ToString());
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
                    File.WriteAllText(sArquivo, ((StringWriter)oXmlGravar).GetStringBuilder().ToString());
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
        public static Boolean AppExecutando(ref System.Threading.Mutex oOneMutex)
        {
            Propriedade.ExecutandoPeloUniNFe = false; //Executado pelo UniNfeServico

            try
            {
                Empresas.CarregaConfiguracao();

                Empresas.CanRun();

                // Se puder executar a aplicação aqui será recriado todos os arquivos de .lock, 
                // pois podem ter sofridos alterações de configurações nas pastas
                Empresas.CreateLockFile();
            }
            catch (NFe.Components.Exceptions.AppJaExecutando ex)
            {
                Auxiliar.WriteLog(ex.Message, false);

                return true;
            }
            catch (NFe.Components.Exceptions.ProblemaExecucaoUniNFe ex)
            {
                Auxiliar.WriteLog(ex.Message, false);
            }
            catch
            {
                //Não preciso retornar nada, somente evito fechar o aplicativo
                //Esta exceção pode ocorrer quando não existe nenhuma empresa cadastrada
                //ainda, ou seja, é a primeira vez que estamos entrando no aplicativo
            }

            return false;
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
            bool executando = false;

            Empresas.CarregaConfiguracao();

            try
            {
                Empresas.CanRun();

                // Se puder executar a aplicação aqui será recriado todos os arquivos de .lock, 
                // pois podem ter sofridos alterações de configurações nas pastas
                Empresas.CreateLockFile();

                string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                if (System.Diagnostics.Process.GetProcessesByName(procName).Length > 1)
                {
                    executando = true;
                }
            }
            catch (NFe.Components.Exceptions.AppJaExecutando ex)
            {
                Empresas.ExisteErroDiretorio = true;
                Empresas.ErroCaminhoDiretorio = ex.Message;
                executando = true;
            }
            catch
            {
            }

            return executando;
        }
        #endregion
    }
}

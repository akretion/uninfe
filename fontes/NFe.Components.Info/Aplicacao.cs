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
        public void GravarXMLInformacoes(string sArquivo, bool somenteConfigGeral)
        {
            int emp = Empresas.FindEmpresaByThread();

            string cStat = "1";
            string xMotivo = "Consulta efetuada com sucesso";

            //Ler os dados do certificado digital
            string sSubject = "";
            string sValIni = "";
            string sValFin = "";

            if (!somenteConfigGeral)
            {
                CertificadoDigital cert = new CertificadoDigital();

                if (Empresas.Configuracoes[emp].UsaCertificado)
                {
                    if (cert.PrepInfCertificado(Empresas.Configuracoes[emp]))
                    {
                        sSubject = cert.sSubject;
                        sValIni = cert.dValidadeInicial.ToString("dd/MM/yyyy HH:mm:ss");
                        sValFin = cert.dValidadeFinal.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        //if (!Empresas.Configuracoes[emp].UsaCertificado)
                        //    xMotivo = "Empresa sem certificado digital informado e/ou não necessário";
                        //else
                        {
                            cStat = "2";
                            xMotivo = "Certificado digital não foi localizado";
                        }
                    }
                }
                else
                    xMotivo = "Empresa sem certificado digital informado e/ou não necessário";
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

                if (!somenteConfigGeral)
                {
                    if (Empresas.Configuracoes[emp].UsaCertificado)
                    {
                        //Dados do certificado digital
                        if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("DadosCertificado");
                        Functions.GravaTxtXml(oXmlGravar, "sSubject", sSubject);
                        Functions.GravaTxtXml(oXmlGravar, "dValIni", sValIni);
                        Functions.GravaTxtXml(oXmlGravar, "dValFin", sValFin);
                        if (isXml) ((XmlWriter)oXmlGravar).WriteEndElement(); //DadosCertificado
                    }
                }

                //Dados gerais do Aplicativo
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement("DadosUniNfe");
                Functions.GravaTxtXml(oXmlGravar, NFe.Components.TpcnResources.versao.ToString(), Propriedade.Versao);
                Functions.GravaTxtXml(oXmlGravar, "dUltModif", dtUltModif);
                Functions.GravaTxtXml(oXmlGravar, "PastaExecutavel", Propriedade.PastaExecutavel);
                Functions.GravaTxtXml(oXmlGravar, "NomeComputador", Environment.MachineName);
                Functions.GravaTxtXml(oXmlGravar, "UsuarioComputador", Environment.UserName);
                //danasa 22/7/2011
                Functions.GravaTxtXml(oXmlGravar, "ExecutandoPeloServico", Propriedade.ServicoRodando.ToString());
                Functions.GravaTxtXml(oXmlGravar, "ConexaoInternet", Functions.IsConnectedToInternet().ToString());

                if (isXml) ((XmlWriter)oXmlGravar).WriteEndElement(); //DadosUniNfe

                //Dados das configurações do aplicativo
                if (isXml) ((XmlWriter)oXmlGravar).WriteStartElement(NFeStrConstants.nfe_configuracoes);
                //Functions.GravaTxtXml(oXmlGravar, NFe.Components.NFeStrConstants.DiretorioSalvarComo, Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString());

                if (!somenteConfigGeral)
                {
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
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeCancelar.ToString(), (!string.IsNullOrEmpty(item.CancelarNfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultar.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultarLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarLoteRps)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultarPorRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfsePorRps)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultarNFSePDF.ToString(), (!string.IsNullOrEmpty(item.ConsultarNFSePDF)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultarNFSePNG.ToString(), (!string.IsNullOrEmpty(item.ConsultarNFSePNG)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultarURL.ToString(), (!string.IsNullOrEmpty(item.ConsultarURLNfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultarSituacaoLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarSituacaoLoteRps)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeRecepcionarLoteRps.ToString(), (!string.IsNullOrEmpty(item.RecepcionarLoteRps)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeInutilizarNFSe.ToString(), (!string.IsNullOrEmpty(item.InutilizarNFSe)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeObterNotaFiscal.ToString(), (!string.IsNullOrEmpty(item.ObterNotaFiscal)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeConsultaSequenciaLoteNotaRPS.ToString(), (!string.IsNullOrEmpty(item.ConsultaSequenciaLoteNotaRPS)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.NFSeSubstituirNfse.ToString(), (!string.IsNullOrEmpty(item.SubstituirNfse)).ToString());
                                    break;

                                default:
                                    if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.NFCe ||
                                        Empresas.Configuracoes[emp].Servico == TipoAplicativo.Nfe ||
                                        Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                    {
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeConsulta", (!string.IsNullOrEmpty(item.NFeConsulta)).ToString());
                                        //Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRecepcao", (!string.IsNullOrEmpty(item.NFeRecepcao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRecepcaoEvento", (!string.IsNullOrEmpty(item.NFeRecepcaoEvento)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeConsultaCadastro", (!string.IsNullOrEmpty(item.NFeConsultaCadastro)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeInutilizacao", (!string.IsNullOrEmpty(item.NFeInutilizacao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeManifDest", (!string.IsNullOrEmpty(item.NFeManifDest)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeStatusServico", (!string.IsNullOrEmpty(item.NFeStatusServico)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeAutorizacao", (!string.IsNullOrEmpty(item.NFeAutorizacao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "NFeRetAutorizacao", (!string.IsNullOrEmpty(item.NFeRetAutorizacao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "DFeRecepcao", (!string.IsNullOrEmpty(item.DFeRecepcao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.LMCAutorizacao.ToString(), (!string.IsNullOrEmpty(item.LMCAutorizacao)).ToString());
                                    }
                                    if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.MDFe ||
                                        Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                    {
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRecepcao", (!string.IsNullOrEmpty(item.MDFeRecepcao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRecepcaoSinc", (!string.IsNullOrEmpty(item.MDFeRecepcaoSinc)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeRetRecepcao", (!string.IsNullOrEmpty(item.MDFeRetRecepcao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeConsulta", (!string.IsNullOrEmpty(item.MDFeConsulta)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "MDFeStatusServico", (!string.IsNullOrEmpty(item.MDFeStatusServico)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.MDFeRecepcaoEvento.ToString(), (!string.IsNullOrEmpty(item.MDFeRecepcaoEvento)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.MDFeConsultaNaoEncerrado.ToString(), (!string.IsNullOrEmpty(item.MDFeNaoEncerrado)).ToString());
                                    }
                                    if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.Cte ||
                                        Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                    {
                                        Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.CTeRecepcaoEvento.ToString(), (!string.IsNullOrEmpty(item.CTeRecepcaoEvento)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "CTeConsultaCadastro", (!string.IsNullOrEmpty(item.CTeConsultaCadastro)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "CTeInutilizacao", (!string.IsNullOrEmpty(item.CTeInutilizacao)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "CTeStatusServico", (!string.IsNullOrEmpty(item.CTeStatusServico)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + "CTeDistribuicaoDFe", (!string.IsNullOrEmpty(item.CTeDistribuicaoDFe)).ToString());
                                    }
                                    if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.EFDReinf ||
                                        Empresas.Configuracoes[emp].Servico == TipoAplicativo.EFDReinfeSocial ||
                                        Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                    {
                                        Functions.GravaTxtXml(oXmlGravar, tipo + NFe.Components.Servicos.RecepcaoLoteReinf.ToString(), (!string.IsNullOrEmpty(item.RecepcaoLoteReinf)).ToString());
                                    }
                                    if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.eSocial ||
                                        Empresas.Configuracoes[emp].Servico == TipoAplicativo.Todos)
                                    {
                                        Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.ConsultarLoteeSocial.ToString(), (!string.IsNullOrEmpty(item.ConsultarLoteeSocial)).ToString());
                                        Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.RecepcaoLoteeSocial.ToString(), (!string.IsNullOrEmpty(item.RecepcaoLoteeSocial)).ToString());
                                    }
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.CancelarCfse.ToString(), (!string.IsNullOrEmpty(item.CancelarCfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.ConfigurarTerminalCfse.ToString(), (!string.IsNullOrEmpty(item.ConfigurarTerminalCfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.ConsultarCfse.ToString(), (!string.IsNullOrEmpty(item.ConsultarCfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.ConsultarDadosCadastroCfse.ToString(), (!string.IsNullOrEmpty(item.ConsultarDadosCadastroCfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.ConsultarLoteCfse.ToString(), (!string.IsNullOrEmpty(item.ConsultarLoteCfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.EnviarInformeManutencaoCfse.ToString(), (!string.IsNullOrEmpty(item.EnviarInformeManutencaoCfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.InformeTrasmissaoSemMovimentoCfse.ToString(), (!string.IsNullOrEmpty(item.InformeTrasmissaoSemMovimentoCfse)).ToString());
                                    Functions.GravaTxtXml(oXmlGravar, tipo + Servicos.RecepcionarLoteCfse.ToString(), (!string.IsNullOrEmpty(item.RecepcionarLoteCfse)).ToString());

                                    break;
                            }

                            if (isXml)
                            {
                                ((XmlWriter)oXmlGravar).WriteEndElement();   //Ambiente
                                ((XmlWriter)oXmlGravar).WriteEndElement();   //list.UF
                            }
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
        /// <returns>True=Aplicação está executando</returns>
        public static Boolean UniNFeSevicoAppExecutando()
        {
            Propriedade.ExecutandoPeloUniNFe = false; //Executado pelo UniNfeServico

            try
            {
                Empresas.CarregaConfiguracao();

                Empresas.CanRun(null);

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

        #endregion AppExecutando()

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
                Empresas.CanRun(null);

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

        #endregion AppExecutando()
    }
}
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
            string dtUltModif;
            URLws item;
            string tipo;

            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
                dtUltModif = File.GetLastWriteTimeUtc(Path.Combine(Propriedade.PastaExecutavel, "uninfe.exe")).ToString("dd/MM/yyyy hh:mm:ss");
            else
            if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                dtUltModif = File.GetLastWriteTimeUtc(Path.Combine(Propriedade.PastaExecutavel, "unincte.exe")).ToString("dd/MM/yyyy hh:mm:ss");
            else
                dtUltModif = File.GetLastWriteTimeUtc(Path.Combine(Propriedade.PastaExecutavel, "uninfes.exe")).ToString("dd/MM/yyyy hh:mm:ss");

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
                    aTXT.AppendLine(NFeStrConstants.PastaBackup + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaBackup) ? "" : Empresa.Configuracoes[emp].PastaBackup));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlEmLote + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvioEmLote) ? "" : Empresa.Configuracoes[emp].PastaEnvioEmLote));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlAssinado + "|" + (string.IsNullOrEmpty(Propriedade.NomePastaXMLAssinado) ? "" : Propriedade.NomePastaXMLAssinado));
                    aTXT.AppendLine(NFeStrConstants.PastaValidar + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaValidar) ? "" : Empresa.Configuracoes[emp].PastaValidar));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlEnviado + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnviado) ? "" : Empresa.Configuracoes[emp].PastaEnviado));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlEnvio + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvio) ? "" : Empresa.Configuracoes[emp].PastaEnvio));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlErro + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaErro) ? "" : Empresa.Configuracoes[emp].PastaErro));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlRetorno + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaRetorno) ? "" : Empresa.Configuracoes[emp].PastaRetorno));
                    aTXT.AppendLine(NFeStrConstants.PastaDownloadNFeDest + "|" + (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaDownloadNFeDest) ? "" : Empresa.Configuracoes[emp].PastaDownloadNFeDest));
                    aTXT.AppendLine(NFeStrConstants.DiasLimpeza + "|" + Empresa.Configuracoes[emp].DiasLimpeza.ToString());
                    aTXT.AppendLine(NFeStrConstants.DiretorioSalvarComo + "|" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString());
                    aTXT.AppendLine(NFeStrConstants.GravarRetornoTXTNFe + "|" + Empresa.Configuracoes[emp].GravarRetornoTXTNFe.ToString());
                    aTXT.AppendLine(NFeStrConstants.GravarEventosDeTerceiros + "|" + Empresa.Configuracoes[emp].GravarEventosDeTerceiros.ToString());
                    aTXT.AppendLine(NFeStrConstants.GravarEventosNaPastaEnviadosNFe + "|" + Empresa.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe.ToString());
                    aTXT.AppendLine(NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe + "|" + Empresa.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe.ToString());
                    aTXT.AppendLine(NFeStrConstants.AmbienteCodigo + "|" + Empresa.Configuracoes[emp].tpAmb.ToString());
                    aTXT.AppendLine(NFeStrConstants.tpEmis + "|" + Empresa.Configuracoes[emp].tpEmis.ToString());
                    aTXT.AppendLine(NFeStrConstants.UnidadeFederativaCodigo + "|" + Empresa.Configuracoes[emp].UFCod.ToString());
                    aTXT.AppendLine(NFeStrConstants.TempoConsulta + "|" + Empresa.Configuracoes[emp].TempoConsulta.ToString());
                    ///
                    /// dados do FTP
                    /// 
                    aTXT.AppendLine(NFeStrConstants.FTPAtivo + "|" + Empresa.Configuracoes[emp].FTPAtivo.ToString());
                    aTXT.AppendLine(NFeStrConstants.FTPGravaXMLPastaUnica + "|" + Empresa.Configuracoes[emp].FTPGravaXMLPastaUnica.ToString());
                    aTXT.AppendLine(NFeStrConstants.FTPPastaAutorizados + "|" + Empresa.Configuracoes[emp].FTPPastaAutorizados);
                    aTXT.AppendLine(NFeStrConstants.FTPPastaRetornos + "|" + Empresa.Configuracoes[emp].FTPPastaRetornos);
                    aTXT.AppendLine(NFeStrConstants.FTPNomeDoUsuario + "|" + Empresa.Configuracoes[emp].FTPNomeDoUsuario);
                    aTXT.AppendLine(NFeStrConstants.FTPNomeDoServidor + "|" + Empresa.Configuracoes[emp].FTPNomeDoServidor);
                    aTXT.AppendLine(NFeStrConstants.FTPPorta + "|" + Empresa.Configuracoes[emp].FTPPorta.ToString());
                    ///
                    /// o ERP poderá verificar se determinado servico está definido no UniNFe
                    /// 
                    foreach (webServices list in WebServiceProxy.webServicesList)
                    {
                        if (list.ID == Empresa.Configuracoes[emp].UFCod || 
                            list.UF == "DPEC" ||
                            list.UF == "SCAN")
                        {
                            if (Empresa.Configuracoes[emp].tpAmb == 2)
                            {
                                tipo = list.UF + ".Homologacao.";
                                item = list.LocalHomologacao;
                            }
                            else
                            {
                                tipo = list.UF + ".Producao.";
                                item = list.LocalProducao;
                            }

                            switch (Propriedade.TipoAplicativo)
                            {
                                case TipoAplicativo.Nfse:
                                    aTXT.AppendLine(tipo + "CancelarNFse|" +            (!string.IsNullOrEmpty(item.CancelarNfse)).ToString());
                                    aTXT.AppendLine(tipo + "ConsultarLoteRps|" +        (!string.IsNullOrEmpty(item.ConsultarLoteRps)).ToString());
                                    aTXT.AppendLine(tipo + "ConsultarNfse|" +           (!string.IsNullOrEmpty(item.ConsultarNfse)).ToString());
                                    aTXT.AppendLine(tipo + "ConsultarNfsePorRps|" +     (!string.IsNullOrEmpty(item.ConsultarNfsePorRps)).ToString());
                                    aTXT.AppendLine(tipo + "ConsultarSituacaoLoteRps|" +(!string.IsNullOrEmpty(item.ConsultarSituacaoLoteRps)).ToString());
                                    aTXT.AppendLine(tipo + "RecepcionarLoteRps|" +      (!string.IsNullOrEmpty(item.RecepcionarLoteRps)).ToString());
                                    break;

                                case TipoAplicativo.Nfe:
                                    if (string.IsNullOrEmpty(item.NFeCancelamentoEvento)) item.NFeCancelamentoEvento = item.NFeCCe;
                                    if (string.IsNullOrEmpty(item.NFeManifestacao)) item.NFeManifestacao = item.NFeCCe;

                                    aTXT.AppendLine(tipo + "NFeRecepcao|" + (!string.IsNullOrEmpty(item.NFeRecepcao)).ToString());
                                    aTXT.AppendLine(tipo + "NFeConsulta|" + (!string.IsNullOrEmpty(item.NFeConsulta)).ToString());
                                    if (list.UF != "DPEC")
                                    {
                                        aTXT.AppendLine(tipo + "NFeCancelamento|" + (!string.IsNullOrEmpty(item.NFeCancelamento)).ToString());
                                        aTXT.AppendLine(tipo + "NFeCancelamentoEvento|" + (!string.IsNullOrEmpty(item.NFeCancelamentoEvento)).ToString());
                                        aTXT.AppendLine(tipo + "NFeCCe|" + (!string.IsNullOrEmpty(item.NFeCCe)).ToString());
                                        aTXT.AppendLine(tipo + "NFeConsulta1|" + (!string.IsNullOrEmpty(item.NFeConsulta1)).ToString());
                                        aTXT.AppendLine(tipo + "NFeConsultaCadastro|" + (!string.IsNullOrEmpty(item.NFeConsultaCadastro)).ToString());
                                        aTXT.AppendLine(tipo + "NFeConsultaNFeDest|" + (!string.IsNullOrEmpty(item.NFeConsultaNFeDest)).ToString());
                                        aTXT.AppendLine(tipo + "NFeDownload|" + (!string.IsNullOrEmpty(item.NFeDownload)).ToString());
                                        aTXT.AppendLine(tipo + "NFeInutilizacao|" + (!string.IsNullOrEmpty(item.NFeInutilizacao)).ToString());
                                        aTXT.AppendLine(tipo + "NFeManifestacao|" + (!string.IsNullOrEmpty(item.NFeManifestacao)).ToString());
                                        aTXT.AppendLine(tipo + "NFeStatusServico|" + (!string.IsNullOrEmpty(item.NFeStatusServico)).ToString());
                                        aTXT.AppendLine(tipo + "NFeRegistroDeSaida|" + (!string.IsNullOrEmpty(item.NFeRegistroDeSaida)).ToString());
                                        aTXT.AppendLine(tipo + "NFeRegistroDeSaidaCancelamento|" + (!string.IsNullOrEmpty(item.NFeRegistroDeSaidaCancelamento)).ToString());
                                    }
                                    break;
                            }
                        }
                    }
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
                    oXmlGravar.WriteStartElement(NFeStrConstants.nfe_configuracoes);
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaBackup, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaBackup) ? "" : Empresa.Configuracoes[emp].PastaBackup));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEmLote, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvioEmLote) ? "" : Empresa.Configuracoes[emp].PastaEnvioEmLote));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlAssinado, (string.IsNullOrEmpty(Propriedade.NomePastaXMLAssinado) ? "" : Propriedade.NomePastaXMLAssinado));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaValidar, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaValidar) ? "" : Empresa.Configuracoes[emp].PastaValidar));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEnviado, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnviado) ? "" : Empresa.Configuracoes[emp].PastaEnviado));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEnvio, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaEnvio) ? "" : Empresa.Configuracoes[emp].PastaEnvio));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlErro, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaErro) ? "" : Empresa.Configuracoes[emp].PastaErro));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlRetorno, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaRetorno) ? "" : Empresa.Configuracoes[emp].PastaRetorno));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaDownloadNFeDest, (string.IsNullOrEmpty(Empresa.Configuracoes[emp].PastaDownloadNFeDest) ? "" : Empresa.Configuracoes[emp].PastaDownloadNFeDest));
                    oXmlGravar.WriteElementString(NFeStrConstants.DiasLimpeza, Empresa.Configuracoes[emp].DiasLimpeza.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.DiretorioSalvarComo, Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.GravarRetornoTXTNFe, Empresa.Configuracoes[emp].GravarRetornoTXTNFe.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosDeTerceiros, Empresa.Configuracoes[emp].GravarEventosDeTerceiros.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosNaPastaEnviadosNFe, Empresa.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe, Empresa.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.AmbienteCodigo, Empresa.Configuracoes[emp].tpAmb.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.tpEmis, Empresa.Configuracoes[emp].tpEmis.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.UnidadeFederativaCodigo, Empresa.Configuracoes[emp].UFCod.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.TempoConsulta, Empresa.Configuracoes[emp].TempoConsulta.ToString());
                    ///
                    /// dados do FTP
                    /// 
                    oXmlGravar.WriteStartElement("FTP");
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPAtivo, Empresa.Configuracoes[emp].FTPAtivo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPGravaXMLPastaUnica, Empresa.Configuracoes[emp].FTPGravaXMLPastaUnica.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPPastaAutorizados, Empresa.Configuracoes[emp].FTPPastaAutorizados);
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPPastaRetornos, Empresa.Configuracoes[emp].FTPPastaRetornos);
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPNomeDoUsuario, Empresa.Configuracoes[emp].FTPNomeDoUsuario);
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPNomeDoServidor, Empresa.Configuracoes[emp].FTPNomeDoServidor);
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPPorta, Empresa.Configuracoes[emp].FTPPorta.ToString());
                    oXmlGravar.WriteEndElement();
                    ///
                    /// o ERP poderá verificar se determinado servico está definido no UniNFe
                    /// 
                    foreach (webServices list in WebServiceProxy.webServicesList)
                    {
                        if (list.ID == Empresa.Configuracoes[emp].UFCod ||
                            list.UF == "DPEC" ||
                            list.UF == "SCAN")
                        {
                            oXmlGravar.WriteStartElement(list.UF);
                            if (Empresa.Configuracoes[emp].tpAmb == 2)
                            {
                                item = list.LocalHomologacao;
                                oXmlGravar.WriteStartElement("Homologacao");
                            }
                            else
                            {
                                item = list.LocalProducao;
                                oXmlGravar.WriteStartElement("Producao");
                            }
                            switch (Propriedade.TipoAplicativo)
                            {
                                case TipoAplicativo.Nfse:
                                    oXmlGravar.WriteElementString("CancelarNFse",           (!string.IsNullOrEmpty(item.CancelarNfse)).ToString());
                                    oXmlGravar.WriteElementString("ConsultarLoteRps",       (!string.IsNullOrEmpty(item.ConsultarLoteRps)).ToString());
                                    oXmlGravar.WriteElementString("ConsultarNfse",          (!string.IsNullOrEmpty(item.ConsultarNfse)).ToString());
                                    oXmlGravar.WriteElementString("ConsultarNfsePorRps",    (!string.IsNullOrEmpty(item.ConsultarNfsePorRps)).ToString());
                                    oXmlGravar.WriteElementString("ConsultarSituacaoLoteRps", (!string.IsNullOrEmpty(item.ConsultarSituacaoLoteRps)).ToString());
                                    oXmlGravar.WriteElementString("RecepcionarLoteRps",     (!string.IsNullOrEmpty(item.RecepcionarLoteRps)).ToString());
                                    break;

                                case TipoAplicativo.Nfe:
                                    if (string.IsNullOrEmpty(item.NFeCancelamentoEvento)) item.NFeCancelamentoEvento = item.NFeCCe;
                                    if (string.IsNullOrEmpty(item.NFeManifestacao)) item.NFeManifestacao = item.NFeCCe;

                                    oXmlGravar.WriteElementString("NFeConsulta", (!string.IsNullOrEmpty(item.NFeConsulta)).ToString());
                                    oXmlGravar.WriteElementString("NFeRecepcao", (!string.IsNullOrEmpty(item.NFeRecepcao)).ToString());
                                    if (list.UF != "DPEC")
                                    {
                                        oXmlGravar.WriteElementString("NFeCancelamento", (!string.IsNullOrEmpty(item.NFeCancelamento)).ToString());
                                        oXmlGravar.WriteElementString("NFeCancelamentoEvento", (!string.IsNullOrEmpty(item.NFeCancelamentoEvento)).ToString());
                                        oXmlGravar.WriteElementString("NFeCCe", (!string.IsNullOrEmpty(item.NFeCCe)).ToString());
                                        oXmlGravar.WriteElementString("NFeConsulta1", (!string.IsNullOrEmpty(item.NFeConsulta1)).ToString());
                                        oXmlGravar.WriteElementString("NFeConsultaCadastro", (!string.IsNullOrEmpty(item.NFeConsultaCadastro)).ToString());
                                        oXmlGravar.WriteElementString("NFeConsultaNFeDest", (!string.IsNullOrEmpty(item.NFeConsultaNFeDest)).ToString());
                                        oXmlGravar.WriteElementString("NFeDownload", (!string.IsNullOrEmpty(item.NFeDownload)).ToString());
                                        oXmlGravar.WriteElementString("NFeInutilizacao", (!string.IsNullOrEmpty(item.NFeInutilizacao)).ToString());
                                        oXmlGravar.WriteElementString("NFeManifestacao", (!string.IsNullOrEmpty(item.NFeManifestacao)).ToString());
                                        oXmlGravar.WriteElementString("NFeStatusServico", (!string.IsNullOrEmpty(item.NFeStatusServico)).ToString());
                                        oXmlGravar.WriteElementString("NFeRegistroDeSaida", (!string.IsNullOrEmpty(item.NFeRegistroDeSaida)).ToString());
                                        oXmlGravar.WriteElementString("NFeRegistroDeSaidaCancelamento", (!string.IsNullOrEmpty(item.NFeRegistroDeSaidaCancelamento)).ToString());
                                    }
                                    break;
                            }
                            oXmlGravar.WriteEndElement();   //Ambiente
                            oXmlGravar.WriteEndElement();   //list.UF
                        }
                    }
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

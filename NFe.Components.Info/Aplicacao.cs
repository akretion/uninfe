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
        /// 
        public void xGravarXMLInformacoes(string sArquivo)
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
                    UTF8Encoding c = new UTF8Encoding(false);

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
                    File.WriteAllText(sArquivo, ((StringWriter)oXmlGravar).GetStringBuilder().ToString(), Encoding.UTF8);
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
            string tipo;

            dtUltModif = File.GetLastWriteTime(Propriedade.NomeAplicacao + ".exe").ToString("dd/MM/yyyy - HH:mm:ss");

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
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        aTXT.AppendLine(NFeStrConstants.PastaBackup + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaBackup) ? "" : Empresas.Configuracoes[emp].PastaBackup));
                        aTXT.AppendLine(NFeStrConstants.PastaXmlEmLote + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlEmLote) ? "" : Empresas.Configuracoes[emp].PastaXmlEmLote));
                        aTXT.AppendLine(NFeStrConstants.PastaXmlEnviado + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlEnviado) ? "" : Empresas.Configuracoes[emp].PastaXmlEnviado));
                    }
                    aTXT.AppendLine(NFeStrConstants.PastaXmlAssinado + "|" + (string.IsNullOrEmpty(Propriedade.NomePastaXMLAssinado) ? "" : Propriedade.NomePastaXMLAssinado));
                    aTXT.AppendLine(NFeStrConstants.PastaValidar + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaValidar) ? "" : Empresas.Configuracoes[emp].PastaValidar));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlEnvio + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlEnvio) ? "" : Empresas.Configuracoes[emp].PastaXmlEnvio));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlErro + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlErro) ? "" : Empresas.Configuracoes[emp].PastaXmlErro));
                    aTXT.AppendLine(NFeStrConstants.PastaXmlRetorno + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlRetorno) ? "" : Empresas.Configuracoes[emp].PastaXmlRetorno));
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        aTXT.AppendLine(NFeStrConstants.PastaDownloadNFeDest + "|" + (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaDownloadNFeDest) ? "" : Empresas.Configuracoes[emp].PastaDownloadNFeDest));
                        aTXT.AppendLine(NFeStrConstants.DiretorioSalvarComo + "|" + Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString());
                        aTXT.AppendLine(NFeStrConstants.GravarRetornoTXTNFe + "|" + Empresas.Configuracoes[emp].GravarRetornoTXTNFe.ToString());
                        aTXT.AppendLine(NFeStrConstants.GravarEventosDeTerceiros + "|" + Empresas.Configuracoes[emp].GravarEventosDeTerceiros.ToString());
                        aTXT.AppendLine(NFeStrConstants.GravarEventosNaPastaEnviadosNFe + "|" + Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe.ToString());
                        aTXT.AppendLine(NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe + "|" + Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe.ToString());
                        aTXT.AppendLine(NFeStrConstants.IndSinc + "|" + Empresas.Configuracoes[emp].IndSinc.ToString());
                        aTXT.AppendLine(NFeStrConstants.CompactarNfe + "|" + Empresas.Configuracoes[emp].CompactarNfe.ToString());

                        aTXT.AppendLine(NFeStrConstants.ConfiguracaoDanfe + "|" + Empresas.Configuracoes[emp].ConfiguracaoDanfe.ToString());
                        aTXT.AppendLine(NFeStrConstants.ConfiguracaoCCe + "|" + Empresas.Configuracoes[emp].ConfiguracaoCCe.ToString());
                        aTXT.AppendLine(NFeStrConstants.PastaConfigUniDanfe + "|" + Empresas.Configuracoes[emp].PastaConfigUniDanfe.ToString());
                        aTXT.AppendLine(NFeStrConstants.PastaExeUniDanfe + "|" + Empresas.Configuracoes[emp].PastaExeUniDanfe.ToString());
                        aTXT.AppendLine(NFeStrConstants.PastaDanfeMon + "|" + Empresas.Configuracoes[emp].PastaDanfeMon.ToString());
                    }
                    aTXT.AppendLine(NFeStrConstants.DiasLimpeza + "|" + Empresas.Configuracoes[emp].DiasLimpeza.ToString());
                    aTXT.AppendLine(NFeStrConstants.AmbienteCodigo + "|" + Empresas.Configuracoes[emp].AmbienteCodigo.ToString());
                    aTXT.AppendLine(NFeStrConstants.tpEmis + "|" + Empresas.Configuracoes[emp].tpEmis.ToString());
                    aTXT.AppendLine(NFeStrConstants.UnidadeFederativaCodigo + "|" + Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString());
                    aTXT.AppendLine(NFeStrConstants.TempoConsulta + "|" + Empresas.Configuracoes[emp].TempoConsulta.ToString());
                    ///
                    /// dados do FTP
                    /// 
                    aTXT.AppendLine(NFeStrConstants.FTPAtivo + "|" + Empresas.Configuracoes[emp].FTPAtivo.ToString());
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        aTXT.AppendLine(NFeStrConstants.FTPGravaXMLPastaUnica + "|" + Empresas.Configuracoes[emp].FTPGravaXMLPastaUnica.ToString());
                        aTXT.AppendLine(NFeStrConstants.FTPPastaAutorizados + "|" + Empresas.Configuracoes[emp].FTPPastaAutorizados);
                    }
                    aTXT.AppendLine(NFeStrConstants.FTPPastaRetornos + "|" + Empresas.Configuracoes[emp].FTPPastaRetornos);
                    aTXT.AppendLine(NFeStrConstants.FTPNomeDoUsuario + "|" + Empresas.Configuracoes[emp].FTPNomeDoUsuario);
                    aTXT.AppendLine(NFeStrConstants.FTPNomeDoServidor + "|" + Empresas.Configuracoes[emp].FTPNomeDoServidor);
                    aTXT.AppendLine(NFeStrConstants.FTPPorta + "|" + Empresas.Configuracoes[emp].FTPPorta.ToString());
                    aTXT.AppendLine(NFeStrConstants.UsuarioWS + "|" + Empresas.Configuracoes[emp].UsuarioWS.ToString());
                    aTXT.AppendLine(NFeStrConstants.SenhaWS + "|" + Empresas.Configuracoes[emp].SenhaWS.ToString());
                    ///
                    /// o ERP poderá verificar se determinado servico está definido no UniNFe
                    /// 
                    foreach (webServices list in WebServiceProxy.webServicesList)
                    {
                        if (list.ID == Empresas.Configuracoes[emp].UnidadeFederativaCodigo ||
                            list.UF == "DPEC" ||
                            list.UF == "SCAN")
                        {
                            if (Empresas.Configuracoes[emp].AmbienteCodigo == 2)
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
                                    aTXT.AppendLine(tipo + NFe.Components.Servicos.CancelarNfse.ToString() + "|" + (!string.IsNullOrEmpty(item.CancelarNfse)).ToString());
                                    aTXT.AppendLine(tipo + NFe.Components.Servicos.ConsultarLoteRps.ToString() + "|" + (!string.IsNullOrEmpty(item.ConsultarLoteRps)).ToString());
                                    aTXT.AppendLine(tipo + NFe.Components.Servicos.ConsultarNfse.ToString() + "|" + (!string.IsNullOrEmpty(item.ConsultarNfse)).ToString());
                                    aTXT.AppendLine(tipo + NFe.Components.Servicos.ConsultarNfsePorRps.ToString() + "|" + (!string.IsNullOrEmpty(item.ConsultarNfsePorRps)).ToString());
                                    aTXT.AppendLine(tipo + NFe.Components.Servicos.ConsultarSituacaoLoteRps.ToString() +"|" + (!string.IsNullOrEmpty(item.ConsultarSituacaoLoteRps)).ToString());
                                    aTXT.AppendLine(tipo + NFe.Components.Servicos.RecepcionarLoteRps.ToString() + "|" + (!string.IsNullOrEmpty(item.RecepcionarLoteRps)).ToString());
                                    aTXT.AppendLine(tipo + NFe.Components.Servicos.ConsultarURLNfse.ToString() + "|" + (!string.IsNullOrEmpty(item.ConsultarURLNfse)).ToString());
                                    break;

                                case TipoAplicativo.Nfe:
                                    aTXT.AppendLine(tipo + "NFeRecepcao|" + (!string.IsNullOrEmpty(item.NFeRecepcao)).ToString());
                                    aTXT.AppendLine(tipo + "NFeConsulta|" + (!string.IsNullOrEmpty(item.NFeConsulta)).ToString());
                                    if (list.UF != "DPEC")
                                    {
                                        aTXT.AppendLine(tipo + "NFeRecepcaoEvento|" + (!string.IsNullOrEmpty(item.NFeRecepcaoEvento)).ToString());
                                        aTXT.AppendLine(tipo + "NFeConsultaCadastro|" + (!string.IsNullOrEmpty(item.NFeConsultaCadastro)).ToString());
                                        aTXT.AppendLine(tipo + "NFeConsultaNFeDest|" + (!string.IsNullOrEmpty(item.NFeConsultaNFeDest)).ToString());
                                        aTXT.AppendLine(tipo + "NFeDownload|" + (!string.IsNullOrEmpty(item.NFeDownload)).ToString());
                                        aTXT.AppendLine(tipo + "NFeInutilizacao|" + (!string.IsNullOrEmpty(item.NFeInutilizacao)).ToString());
                                        aTXT.AppendLine(tipo + "NFeManifDest|" + (!string.IsNullOrEmpty(item.NFeManifDest)).ToString());
                                        aTXT.AppendLine(tipo + "NFeStatusServico|" + (!string.IsNullOrEmpty(item.NFeStatusServico)).ToString());
                                        aTXT.AppendLine(tipo + "NFeRegistroDeSaida|" + (!string.IsNullOrEmpty(item.NFeRegistroDeSaida)).ToString());
                                        aTXT.AppendLine(tipo + "NFeRegistroDeSaidaCancelamento|" + (!string.IsNullOrEmpty(item.NFeRegistroDeSaidaCancelamento)).ToString());
                                        aTXT.AppendLine(tipo + "NFeAutorizacao|" + (!string.IsNullOrEmpty(item.NFeAutorizacao)).ToString());
                                        aTXT.AppendLine(tipo + "NFeRetAutorizacao|" + (!string.IsNullOrEmpty(item.NFeRetAutorizacao)).ToString());
                                        aTXT.AppendLine(tipo + "MDFeRecepcao|" + (!string.IsNullOrEmpty(item.MDFeRecepcao)).ToString());
                                        aTXT.AppendLine(tipo + "MDFeRetRecepcao|" + (!string.IsNullOrEmpty(item.MDFeRetRecepcao)).ToString());
                                        aTXT.AppendLine(tipo + "MDFeConsulta|" + (!string.IsNullOrEmpty(item.MDFeConsulta)).ToString());
                                        aTXT.AppendLine(tipo + "MDFeStatusServico|" + (!string.IsNullOrEmpty(item.MDFeStatusServico)).ToString());
                                        aTXT.AppendLine(tipo + "MDFeRecepcaoEvento|" + (!string.IsNullOrEmpty(item.MDFeRecepcao)).ToString());
                                        aTXT.AppendLine(tipo + "CTeRecepcaoEvento|" + (!string.IsNullOrEmpty(item.CTeRecepcaoEvento)).ToString());
                                        aTXT.AppendLine(tipo + "CTeConsultaCadastro|" + (!string.IsNullOrEmpty(item.CTeConsultaCadastro)).ToString());
                                        aTXT.AppendLine(tipo + "CTeInutilizacao|" + (!string.IsNullOrEmpty(item.CTeInutilizacao)).ToString());
                                        aTXT.AppendLine(tipo + "CTeStatusServico|" + (!string.IsNullOrEmpty(item.CTeStatusServico)).ToString());
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
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaBackup, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaBackup) ? "" : Empresas.Configuracoes[emp].PastaBackup));
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEmLote, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlEmLote) ? "" : Empresas.Configuracoes[emp].PastaXmlEmLote));
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEnviado, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlEnviado) ? "" : Empresas.Configuracoes[emp].PastaXmlEnviado));
                    }
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlAssinado, (string.IsNullOrEmpty(Propriedade.NomePastaXMLAssinado) ? "" : Propriedade.NomePastaXMLAssinado));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaValidar, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaValidar) ? "" : Empresas.Configuracoes[emp].PastaValidar));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlEnvio, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlEnvio) ? "" : Empresas.Configuracoes[emp].PastaXmlEnvio));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlErro, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlErro) ? "" : Empresas.Configuracoes[emp].PastaXmlErro));
                    oXmlGravar.WriteElementString(NFeStrConstants.PastaXmlRetorno, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlRetorno) ? "" : Empresas.Configuracoes[emp].PastaXmlRetorno));
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaDownloadNFeDest, (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaDownloadNFeDest) ? "" : Empresas.Configuracoes[emp].PastaDownloadNFeDest));
                        oXmlGravar.WriteElementString(NFeStrConstants.DiretorioSalvarComo, Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarRetornoTXTNFe, Empresas.Configuracoes[emp].GravarRetornoTXTNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosDeTerceiros, Empresas.Configuracoes[emp].GravarEventosDeTerceiros.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosNaPastaEnviadosNFe, Empresas.Configuracoes[emp].GravarEventosNaPastaEnviadosNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.GravarEventosCancelamentoNaPastaEnviadosNFe, Empresas.Configuracoes[emp].GravarEventosCancelamentoNaPastaEnviadosNFe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.IndSinc, Empresas.Configuracoes[emp].IndSinc.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.CompactarNfe, Empresas.Configuracoes[emp].CompactarNfe.ToString());

                        oXmlGravar.WriteElementString(NFeStrConstants.ConfiguracaoDanfe, Empresas.Configuracoes[emp].ConfiguracaoDanfe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.ConfiguracaoCCe, Empresas.Configuracoes[emp].ConfiguracaoCCe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaConfigUniDanfe, Empresas.Configuracoes[emp].PastaConfigUniDanfe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaExeUniDanfe, Empresas.Configuracoes[emp].PastaExeUniDanfe.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.PastaDanfeMon, Empresas.Configuracoes[emp].PastaDanfeMon.ToString());
                    }
                    oXmlGravar.WriteElementString(NFeStrConstants.DiasLimpeza, Empresas.Configuracoes[emp].DiasLimpeza.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.AmbienteCodigo, Empresas.Configuracoes[emp].AmbienteCodigo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.tpEmis, Empresas.Configuracoes[emp].tpEmis.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.UnidadeFederativaCodigo, Empresas.Configuracoes[emp].UnidadeFederativaCodigo.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.TempoConsulta, Empresas.Configuracoes[emp].TempoConsulta.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.UsuarioWS, Empresas.Configuracoes[emp].UsuarioWS.ToString());
                    oXmlGravar.WriteElementString(NFeStrConstants.SenhaWS, Empresas.Configuracoes[emp].SenhaWS.ToString());
                    ///
                    /// dados do FTP
                    /// 
                    oXmlGravar.WriteStartElement("FTP");
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPAtivo, Empresas.Configuracoes[emp].FTPAtivo.ToString());
                    if (Propriedade.TipoAplicativo != TipoAplicativo.Nfse)
                    {
                        oXmlGravar.WriteElementString(NFeStrConstants.FTPGravaXMLPastaUnica, Empresas.Configuracoes[emp].FTPGravaXMLPastaUnica.ToString());
                        oXmlGravar.WriteElementString(NFeStrConstants.FTPPastaAutorizados, Empresas.Configuracoes[emp].FTPPastaAutorizados);
                    }
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPPastaRetornos, Empresas.Configuracoes[emp].FTPPastaRetornos);
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPNomeDoUsuario, Empresas.Configuracoes[emp].FTPNomeDoUsuario);
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPNomeDoServidor, Empresas.Configuracoes[emp].FTPNomeDoServidor);
                    oXmlGravar.WriteElementString(NFeStrConstants.FTPPorta, Empresas.Configuracoes[emp].FTPPorta.ToString());
                    oXmlGravar.WriteEndElement();
                    ///
                    /// o ERP poderá verificar se determinado servico está definido no UniNFe
                    /// 
                    foreach (webServices list in WebServiceProxy.webServicesList)
                    {
                        if (list.ID == Empresas.Configuracoes[emp].UnidadeFederativaCodigo ||
                            list.UF == "DPEC" ||
                            list.UF == "SCAN")
                        {
                            oXmlGravar.WriteStartElement(list.UF);
                            if (Empresas.Configuracoes[emp].AmbienteCodigo == 2)
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
                                    oXmlGravar.WriteElementString(NFe.Components.Servicos.CancelarNfse.ToString(), (!string.IsNullOrEmpty(item.CancelarNfse)).ToString());
                                    oXmlGravar.WriteElementString(NFe.Components.Servicos.ConsultarLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarLoteRps)).ToString());
                                    oXmlGravar.WriteElementString(NFe.Components.Servicos.ConsultarNfse.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfse)).ToString());
                                    oXmlGravar.WriteElementString(NFe.Components.Servicos.ConsultarNfsePorRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarNfsePorRps)).ToString());
                                    oXmlGravar.WriteElementString(NFe.Components.Servicos.ConsultarSituacaoLoteRps.ToString(), (!string.IsNullOrEmpty(item.ConsultarSituacaoLoteRps)).ToString());
                                    oXmlGravar.WriteElementString(NFe.Components.Servicos.RecepcionarLoteRps.ToString(), (!string.IsNullOrEmpty(item.RecepcionarLoteRps)).ToString());
                                    oXmlGravar.WriteElementString(NFe.Components.Servicos.ConsultarURLNfse.ToString(), (!string.IsNullOrEmpty(item.ConsultarURLNfse)).ToString());
                                    break;

                                case TipoAplicativo.Nfe:
                                    oXmlGravar.WriteElementString("NFeConsulta", (!string.IsNullOrEmpty(item.NFeConsulta)).ToString());
                                    oXmlGravar.WriteElementString("NFeRecepcao", (!string.IsNullOrEmpty(item.NFeRecepcao)).ToString());
                                    if (list.UF != "DPEC")
                                    {
                                        oXmlGravar.WriteElementString("NFeRecepcaoEvento", (!string.IsNullOrEmpty(item.NFeRecepcaoEvento)).ToString());
                                        oXmlGravar.WriteElementString("NFeConsultaCadastro", (!string.IsNullOrEmpty(item.NFeConsultaCadastro)).ToString());
                                        oXmlGravar.WriteElementString("NFeConsultaNFeDest", (!string.IsNullOrEmpty(item.NFeConsultaNFeDest)).ToString());
                                        oXmlGravar.WriteElementString("NFeDownload", (!string.IsNullOrEmpty(item.NFeDownload)).ToString());
                                        oXmlGravar.WriteElementString("NFeInutilizacao", (!string.IsNullOrEmpty(item.NFeInutilizacao)).ToString());
                                        oXmlGravar.WriteElementString("NFeManifDest", (!string.IsNullOrEmpty(item.NFeManifDest)).ToString());
                                        oXmlGravar.WriteElementString("NFeStatusServico", (!string.IsNullOrEmpty(item.NFeStatusServico)).ToString());
                                        oXmlGravar.WriteElementString("NFeRegistroDeSaida", (!string.IsNullOrEmpty(item.NFeRegistroDeSaida)).ToString());
                                        oXmlGravar.WriteElementString("NFeRegistroDeSaidaCancelamento", (!string.IsNullOrEmpty(item.NFeRegistroDeSaidaCancelamento)).ToString());
                                        oXmlGravar.WriteElementString("NFeAutorizacao", (!string.IsNullOrEmpty(item.NFeAutorizacao)).ToString());
                                        oXmlGravar.WriteElementString("NFeRetAutorizacao", (!string.IsNullOrEmpty(item.NFeRetAutorizacao)).ToString());
                                        oXmlGravar.WriteElementString("MDFeRecepcao", (!string.IsNullOrEmpty(item.MDFeRecepcao)).ToString());
                                        oXmlGravar.WriteElementString("MDFeRetRecepcao", (!string.IsNullOrEmpty(item.MDFeRetRecepcao)).ToString());
                                        oXmlGravar.WriteElementString("MDFeConsulta", (!string.IsNullOrEmpty(item.MDFeConsulta)).ToString());
                                        oXmlGravar.WriteElementString("MDFeStatusServico", (!string.IsNullOrEmpty(item.MDFeStatusServico)).ToString());
                                        oXmlGravar.WriteElementString("MDFeRecepcaoEvento", (!string.IsNullOrEmpty(item.MDFeRecepcaoEvento)).ToString());
                                        oXmlGravar.WriteElementString("CTeRecepcaoEvento", (!string.IsNullOrEmpty(item.CTeRecepcaoEvento)).ToString());
                                        oXmlGravar.WriteElementString("CTeConsultaCadastro", (!string.IsNullOrEmpty(item.CTeConsultaCadastro)).ToString());
                                        oXmlGravar.WriteElementString("CTeInutilizacao", (!string.IsNullOrEmpty(item.CTeInutilizacao)).ToString());
                                        oXmlGravar.WriteElementString("CTeStatusServico", (!string.IsNullOrEmpty(item.CTeStatusServico)).ToString());
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
        public static Boolean AppExecutando(bool silencioso)
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

            bool executando = false;

            string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (System.Diagnostics.Process.GetProcessesByName(procName).Length > 1)
            {
                executando = true;
            }

            if (!silencioso)
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

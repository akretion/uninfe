using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;

namespace NFe.Service
{
    public class TaskDanfeReport : TaskAbst
    {
        public override void Execute()
        {
            Servico = Servicos.DanfeRelatorio;

            int emp = Empresas.FindEmpresaByThread();

            DateTime datai = DateTime.MinValue, dataf = DateTime.MaxValue;
            bool imprimir = false;
            string exportarPasta = "Enviados";
            string fm = "";

            try
            {
                if (this.vXmlNfeDadosMsgEhXML)
                {
#if modelo_xml
<?xml version="1.0" encoding="utf-8"?>
<dados>
    <DataInicial>2014-1-1</DataInicial>
    <DataFinal>2014-12-1</DataFinal>
    <Imprimir>true</Imprimir>
    <ExportarPasta>Enviar | Enviados | Erros</ExportarPasta>
</dados>
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, 
                                Path.GetFileName(NomeArquivoXML.Replace(Propriedade.ExtEnvio.EnvDanfeReport_XML, Propriedade.ExtRetorno.RetDanfeReport_XML).Replace(".xml", ".err"))));

                    XmlDocument doc = new XmlDocument();
                    doc.Load(NomeArquivoXML);
                    foreach (XmlNode node in doc.GetElementsByTagName("dados"))
                    {
                        XmlElement elementConfig = (XmlElement)node;

                        exportarPasta = Functions.LerTag(elementConfig, "ExportarPasta", "Enviados");
                        datai = Convert.ToDateTime(Functions.LerTag(elementConfig, "DataInicial", "2001-1-1"));
                        dataf = Convert.ToDateTime(Functions.LerTag(elementConfig, "DataFinal", "2999-1-1"));
                        imprimir = Convert.ToBoolean(Functions.LerTag(elementConfig, "Imprimir", "true"));
                    }
                    fm = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.EnvDanfeReport_XML) + Propriedade.ExtRetorno.RetDanfeReport_XML;
                }
                else
                {
#if modelo_txt
DataInicial|2014-1-1
DataFinal|2014-1-1
Imprimir|true | false
ExportarPasta|Enviar | Enviados | Erros
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, 
                                Path.GetFileName(NomeArquivoXML.Replace(Propriedade.ExtEnvio.EnvDanfeReport_TXT, Propriedade.ExtRetorno.RetDanfeReport_TXT).Replace(".txt", ".err"))));

                    List<string> cLinhas = Functions.LerArquivo(NomeArquivoXML);
                    foreach (string cTexto in cLinhas)
                    {
                        string[] dados = cTexto.Split('|');
                        if (dados.GetLength(0) == 1) continue;

                        switch (dados[0].ToLower())
                        {
                            case "exportarpasta":
                                exportarPasta = dados[1].Trim();
                                break;
                            case "datainicial":
                                datai = Convert.ToDateTime(dados[1].Trim());
                                break;
                            case "datafinal":
                                dataf = Convert.ToDateTime(dados[1].Trim());
                                break;
                            case "imprimir":
                                imprimir = Convert.ToBoolean(dados[1].Trim());
                                break;
                        }
                    }
                    fm = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.EnvDanfeReport_TXT) + Propriedade.ExtRetorno.RetDanfeReport_TXT;
                }
                if (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaExeUniDanfe))
                {
                    throw new Exception("Pasta contendo o UniDANFE não definida para a empresa: " + Empresas.Configuracoes[emp].Nome);
                }

                TFunctions.ExecutaUniDanfe_ReportEmail(emp, datai, dataf, imprimir, exportarPasta, fm);
            }
            catch (Exception ex)
            {
                try
                {
                    string ExtRet = (this.vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvDanfeReport_XML : Propriedade.ExtEnvio.EnvDanfeReport_TXT);
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.RetDanfeReport_XML.Replace(".xml", ".err"), ex);
                }
                catch
                {
                }
            }
            finally
            {
                try
                {
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                }
            }
        }
    }

    public class TaskDanfe : TaskAbst
    {
        public override void Execute()
        {
            Servico = Servicos.ImpressaoNFe;

            int emp = Empresas.FindEmpresaByThread();
            string aFilename = "";
            string aAnexos = "";
            string aPrinter = "";
            string aEmail = "";
            int aCopias = -1;

            try
            {
                if (this.vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
#if modelo_xml
<?xml version="1.0" encoding="utf-8"?>
<dados>
    <FileName>c:\enviados\201401\0000000000-nfeProc.xml</FileName>
    <Anexos>c:\temp\anexo1.txt;c:\temp\anexo2.txt;c:\temp\anexo3.txt</Anexos>
    <Copias>2</Copias>
    <Impressora></Impressora>
    <Email></Email>
</dados>
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Path.GetFileName(NomeArquivoXML.Replace(Propriedade.ExtEnvio.EnvImpressaoDanfe_XML, Propriedade.ExtRetorno.RetImpressaoDanfe_XML).Replace(".xml", ".err"))));

                    XmlDocument doc = new XmlDocument();
                    doc.Load(NomeArquivoXML);
                    foreach (XmlNode node in doc.GetElementsByTagName("dados"))
                    {
                        XmlElement elementConfig = (XmlElement)node;

                        aFilename = Functions.LerTag(elementConfig, "FileName", "");
                        aAnexos   = Functions.LerTag(elementConfig, "Anexos", "");
                        aPrinter  = Functions.LerTag(elementConfig, "Impressora", "");
                        aEmail    = Functions.LerTag(elementConfig, "Email", "");
                        aCopias   = Convert.ToInt32("0" + Functions.LerTag(elementConfig, "Anexos", "-1"));
                    }
                }
                else
                {
#if modelo_txt
FileName|c:\enviados\201401\0000000000-nfeProc.xml | 0000000000-mdfeProc.xml | 0000000000-cteProc.xml | 0000000000-nfeEventoNFe.xml | 0000000000-nfeEventoMDFe.xml | 0000000000-nfeEventoCTe.xml
Anexos|c:\temp\anexo1.txt;c:\temp\anexo2.txt;c:\temp\anexo3.txt
Copias|1
Impressora|
Email|
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Path.GetFileName(NomeArquivoXML.Replace(Propriedade.ExtEnvio.EnvImpressaoDanfe_TXT, Propriedade.ExtRetorno.RetImpressaoDanfe_TXT).Replace(".txt", ".err"))));

                    List<string> cLinhas = Functions.LerArquivo(NomeArquivoXML);
                    foreach (string cTexto in cLinhas)
                    {
                        string[] dados = cTexto.Split('|');
                        if (dados.GetLength(0) == 1) continue;

                        switch (dados[0].ToLower())
                        {
                            case "filename":
                                aFilename = dados[1].Trim();
                                break;
                            case "anexos":
                                aAnexos = dados[1].Trim();
                                break;
                            case "impressora":
                                aPrinter = dados[1].Trim();
                                break;
                            case "copias":
                                aCopias = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "email":
                                aEmail = dados[1].Trim();
                                break;
                        }
                    }
                }
                if (Path.GetDirectoryName(aFilename).ToLower().StartsWith((Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString()).ToLower()) ||
                    Path.GetDirectoryName(aFilename).ToLower().StartsWith((Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString()).ToLower()))
                {
                    if (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaExeUniDanfe))
                    {
                        throw new Exception("Pasta contendo o UniDANFE não definida para a empresa: " + Empresas.Configuracoes[emp].Nome);
                    }
                    TFunctions.ExecutaUniDanfe(aFilename, DateTime.Today, Empresas.Configuracoes[emp], aAnexos, aPrinter, aCopias, aEmail);
                }
                else
                    throw new Exception("Arquivo '" + aFilename + "' deve estar na pasta de 'Autorizados/Denegados' da empresa: " + Empresas.Configuracoes[emp].Nome);
            }
            catch (Exception ex)
            {
                try
                {
                    string ExtRet = (this.vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvImpressaoDanfe_XML : Propriedade.ExtEnvio.EnvImpressaoDanfe_TXT);
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.RetImpressaoDanfe_XML.Replace(".xml",".err"), ex);
                }
                catch
                {
                }
            }
            finally
            {
                try
                {
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                }
            }
        }
    }
}

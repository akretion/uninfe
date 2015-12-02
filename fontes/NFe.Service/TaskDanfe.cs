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
            Servico = Servicos.DANFERelatorio;

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
                                Path.GetFileName(NomeArquivoXML.Replace(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).RetornoXML).Replace(".xml", ".err"))));

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
                    fm = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).RetornoXML;
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
                                Path.GetFileName(NomeArquivoXML.Replace(Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioTXT, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).RetornoTXT).Replace(".txt", ".err"))));

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
                    fm = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioTXT) + 
                                Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).RetornoTXT;
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
                    string ExtEnvio = (this.vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioXML : 
                                                                   Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).EnvioTXT);
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtEnvio, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDanfeReport).RetornoXML.Replace(".xml", ".err"), ex);
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

    public class TaskDanfeContingencia : TaskAbst
    {
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                if (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaExeUniDanfe))
                    throw new Exception("Pasta contendo o UniDANFE não definida para a empresa: " + Empresas.Configuracoes[emp].Nome);

                TFunctions.ExecutaUniDanfe(NomeArquivoXML, DateTime.Today, Empresas.Configuracoes[emp]);
            }
            catch (Exception ex)
            {
                try
                {
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, Propriedade.ExtRetorno.Nfe_ERR, ex);
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
            Servico = Servicos.DANFEImpressao;

            int emp = Empresas.FindEmpresaByThread();
            string aFilename = "";

            try
            {
                Dictionary<string, string> args = new Dictionary<string, string>();

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
    <pp></pp>
    <PastaPDF></PastaPDF>
    <np></np>
    <NomePDF></NomePDF>
    <plq></plq>
    <Auxiliar></Auxiliar>
    <Opcoes></Opcoes>
</dados>
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                            Path.GetFileName(NomeArquivoXML.Replace(Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).RetornoXML).Replace(".xml", ".err"))));

                    XmlDocument doc = new XmlDocument();
                    doc.Load(NomeArquivoXML);
                    foreach (XmlNode node in doc.GetElementsByTagName("dados"))
                    {
                        XmlElement elementConfig = (XmlElement)node;

                        aFilename = Functions.LerTag(elementConfig, "FileName", "");
                        args.Add("anexos", Functions.LerTag(elementConfig, "Anexos", ""));
                        args.Add("impressora", Functions.LerTag(elementConfig, "Impressora", ""));
                        args.Add("email", Functions.LerTag(elementConfig, "Email", ""));
                        args.Add("pp", Functions.LerTag(elementConfig, "pp", Functions.LerTag(elementConfig, "PastaPDF", "")));
                        args.Add("np", Functions.LerTag(elementConfig, "np", Functions.LerTag(elementConfig, "NomePDF", "")));
                        args.Add("plq", Functions.LerTag(elementConfig, "plq", ""));
                        args.Add("auxiliar", Functions.LerTag(elementConfig, "Auxiliar", ""));
                        args.Add("copias", Functions.LerTag(elementConfig, "Copias", "-1"));
                        args.Add("opcoes", Functions.LerTag(elementConfig, "Opcoes", ""));
                    }
                    args.Add("xml", "1");
                }
                else
                {
#if modelo_txt
FileName|c:\enviados\201401\0000000000-nfeProc.xml | 0000000000-mdfeProc.xml | 0000000000-cteProc.xml | 0000000000-nfeEventoNFe.xml | 0000000000-nfeEventoMDFe.xml | 0000000000-nfeEventoCTe.xml
Anexos|c:\temp\anexo1.txt;c:\temp\anexo2.txt;c:\temp\anexo3.txt
Copias|1
Impressora|
Email|
pp|
pastapdf|
np|
nomepdf|
plq|
auxiliar|
opcoes|
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, 
                                            Path.GetFileName(NomeArquivoXML.Replace(Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).EnvioTXT, Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).RetornoTXT).Replace(".txt", ".err"))));

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
                            case "impressora":
                            case "plq":
                            case "copias":
                            case "email":
                            case "pp":
                            case "pastapdf":
                            case "np":
                            case "nomepdf":
                            case "auxiliar":
                            case "opcoes":
                                if (dados[0].ToLower().Equals("pastapdf"))
                                    args.Add("pp", dados[1].Trim());
                                else if (dados[0].ToLower().Equals("nomepdf"))
                                    args.Add("np", dados[1].Trim());
                                else args.Add(dados[0].ToLower(), dados[1].Trim());
                                break;
                        }
                        args.Add("xml", "0");
                    }
                }
                if (Path.GetDirectoryName(aFilename).ToLower().StartsWith((Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString()).ToLower()) ||
                    Path.GetDirectoryName(aFilename).ToLower().StartsWith((Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString()).ToLower()))
                {
                    if (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaExeUniDanfe))
                    {
                        throw new Exception("Pasta contendo o UniDANFE não definida para a empresa: " + Empresas.Configuracoes[emp].Nome);
                    }
                    TFunctions.ExecutaUniDanfe(aFilename, DateTime.Today, Empresas.Configuracoes[emp], args);
                }
                else
                    throw new Exception("Arquivo '" + aFilename + "' deve estar na pasta de 'Autorizados/Denegados' da empresa: " + Empresas.Configuracoes[emp].Nome);
            }
            catch (Exception ex)
            {
                try
                {
                    string ExtRet = (this.vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).EnvioXML : Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).EnvioTXT);
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.Extensao(Propriedade.TipoEnvio.EnvImpressaoDanfe).EnvioXML.Replace(".xml", ".err"), ex);
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

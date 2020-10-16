using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    public class TaskDFeRecepcao : TaskAbst
    {
        private string ExtEnvioDFe { get; set; }
        private string ExtEnvioDFeTXT { get; set; }
        private string ExtRetornoDFe { get; set; }
        private string ExtRetEnvDFe_ERR { get; set; }

        public TaskDFeRecepcao(string arquivo)
        {
            Servico = Servicos.DFeEnviar;
            NomeArquivoXML = arquivo;
            if (vXmlNfeDadosMsgEhXML)
            {
                ConteudoXML.PreserveWhitespace = false;
                ConteudoXML.Load(arquivo);
            }
        }

        public override void Execute()
        {
            switch (Servico)
            {
                case Servicos.DFeEnviar:
                    ExtEnvioDFe = Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioXML;
                    ExtEnvioDFeTXT = Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioTXT;
                    ExtRetornoDFe = Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).RetornoXML;
                    ExtRetEnvDFe_ERR = Propriedade.ExtRetorno.retEnvDFe_ERR;
                    break;

                case Servicos.CTeDistribuicaoDFe:
                    ExtEnvioDFe = Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFeCTe).EnvioXML;
                    ExtEnvioDFeTXT = Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFeCTe).EnvioTXT;
                    ExtRetornoDFe = Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFeCTe).RetornoXML;
                    ExtRetEnvDFe_ERR = Propriedade.ExtRetorno.retEnvDFeCTe_ERR;
                    break;
            }

            int emp = Empresas.FindEmpresaByThread();
            distDFeInt _distDFeInt = new distDFeInt();

            try
            {
                if (!this.vXmlNfeDadosMsgEhXML)
                {
                    ///versao|1.00
                    ///tpAmb|1
                    ///cUFAutor|35
                    ///CNPJ|
                    /// ou
                    ///CPF|
                    ///ultNSU|123456789012345
                    /// ou
                    ///NSU|123456789012345
                    List<string> cLinhas = Functions.LerArquivo(NomeArquivoXML);
                    Functions.PopulateClasse(_distDFeInt, cLinhas);

                    string f = Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml";

                    if (NomeArquivoXML.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        f = Path.Combine(Empresas.Configuracoes[emp].PastaValidar, f);
                    }
                    // Gerar o XML de envio de DFe a partir do TXT gerado pelo ERP
                    oGerarXML.RecepcaoDFe(f, _distDFeInt);
                }
                else
                {
                    XmlNodeList consdistDFeIntList = ConteudoXML.GetElementsByTagName("distDFeInt");

                    foreach (XmlNode consdistDFeIntNode in consdistDFeIntList)
                    {
                        XmlElement consdistDFeIntElemento = (XmlElement)consdistDFeIntNode;
                        Functions.PopulateClasse(_distDFeInt, consdistDFeIntElemento);
                    }

                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico,
                        emp,
                        991,
                        _distDFeInt.tpAmb, 0);

                    System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(991, _distDFeInt.tpAmb, 1, Servico);

                    object oConsNFDestEvento = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                    new AssinaturaDigital().CarregarPIN(emp, NomeArquivoXML, Servico);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy,
                                        oConsNFDestEvento,
                                        wsProxy.NomeMetodoWS[0],
                                        null,
                                        this,
                                        ExtEnvioDFe,
                                        ExtRetornoDFe,
                                        true,
                                        securityProtocolType);

                    LeRetornoDFe(emp, ConteudoXML);
                }
            }
            catch (Exception ex)
            {
                WriteLogError(ex);
            }
            finally
            {
                try
                {
                    //Deletar o arquivo de solicitação do serviço
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                }
            }
        }

        private void LeRetornoDFe(int emp, XmlDocument doc)
        {
            try
            {
                ///
                /// pega o nome base dos arquivos a serem gravados
                ///
                string fileRetorno2 = Functions.ExtrairNomeArq(NomeArquivoXML, ExtEnvioDFe);
                ///
                /// pega o nome do arquivo de retorno
                ///
                string fileRetorno = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                  fileRetorno2 + ExtRetornoDFe);

                //File.Copy(@"C:\Users\wandrey\Downloads\10432020000195-dist-dfecte.xml", fileRetorno, true);

                if (!File.Exists(fileRetorno))
                {
                    return;
                }
                ///
                /// cria a pasta para comportar as notas e eventos retornados já descompactados
                ///
                string folderTerceiros = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, "dfe");
                if (!Directory.Exists(folderTerceiros))
                    Directory.CreateDirectory(folderTerceiros);

                ///
                /// exclui todos os arquivos que foram envolvidos no retorno
                ///
                foreach (var item in Directory.GetFiles(folderTerceiros, fileRetorno2 + "-*.xml", SearchOption.TopDirectoryOnly))
                    if (!Functions.FileInUse(item))
                        File.Delete(item);

                doc.Load(fileRetorno);
                XmlNodeList envEventoList = doc.GetElementsByTagName("retDistDFeInt");
                foreach (XmlNode ret1Node in envEventoList)
                {
                    XmlElement ret1Elemento = (XmlElement)ret1Node;

                    XmlNodeList ret1List = ret1Elemento.GetElementsByTagName("loteDistDFeInt");
                    foreach (XmlNode ret in ret1List)
                    {
                        ExtraiDFe(ret, "docZip", folderTerceiros, fileRetorno2, emp, fileRetorno);
                    }
                }
            }
            catch (Exception ex)
            {
                Auxiliar.WriteLog("LeRetornoNFe: " + ex.Message, false);
                ///
                /// Wandrey.
                /// Foi tudo processado mas houve algum erro na descompactacao dos retornos
                /// Se gravar o arquivo com extensao .err, o ERP pode ignorar o XML de retorno, que está correto
                ///
                //WriteLogError(ex);
            }
        }

        private void ExtraiDFe(XmlNode ret, string tagNameDoc, string folderTerceiros, string fileRetorno2, int emp, string fileRetorno)
        {
            for (int n = 0; n < ret.ChildNodes.Count; ++n)
            {
                if (ret.ChildNodes[n].Name.Equals(tagNameDoc))
                {
                    string FileToFtp = "";
                    string NSU = ret.ChildNodes[n].Attributes[TpcnResources.NSU.ToString()].Value;

                    ///
                    /// descompacta o conteudo
                    ///
                    string xmlRes = TFunctions.Decompress(ret.ChildNodes[n].InnerText);

                    XmlDocument docXML = new XmlDocument();
                    docXML.Load(Functions.StringXmlToStreamUTF8(xmlRes));

                    if (string.IsNullOrEmpty(xmlRes))
                    {
                        Auxiliar.WriteLog("LeRetornoNFe: Não foi possivel descompactar o conteudo da NSU: " + NSU, false);
                    }
                    else
                    {
                        #region NFe

                        if (ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("resEvento"))
                        {
                            FileToFtp = Path.Combine(folderTerceiros, fileRetorno2 + "-" + NSU + Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).RetornoXML);
                        }
                        else if (ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procEventoNFe"))
                        {
                            string chNFe = Functions.LerTag(((XmlElement)((XmlElement)docXML.GetElementsByTagName("evento")[0]).GetElementsByTagName("infEvento")[0]), "chNFe", false);
                            string tpEvento = Functions.LerTag(((XmlElement)((XmlElement)docXML.GetElementsByTagName("evento")[0]).GetElementsByTagName("infEvento")[0]), "tpEvento", false);
                            string nSeqEvento = Functions.LerTag(((XmlElement)((XmlElement)docXML.GetElementsByTagName("evento")[0]).GetElementsByTagName("infEvento")[0]), "nSeqEvento", false);

                            if (Empresas.Configuracoes[emp].ArqNSU)
                                FileToFtp = Path.Combine(folderTerceiros, fileRetorno2 + "-" + NSU + Propriedade.ExtRetorno.ProcEventoNFe);
                            else
                                FileToFtp = Path.Combine(folderTerceiros, chNFe + "_" + tpEvento + "_" + nSeqEvento.PadLeft(2, '0') + Propriedade.ExtRetorno.ProcEventoNFe);
                        }
                        else if (ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procNFe"))
                        {
                            string chave = ((XmlElement)docXML.GetElementsByTagName("infNFe")[0]).GetAttribute("Id").Substring(3, 44);

                            if (Empresas.Configuracoes[emp].ArqNSU)
                                FileToFtp = Path.Combine(folderTerceiros, fileRetorno2 + "-" + NSU + Propriedade.ExtRetorno.ProcNFe);
                            else
                                FileToFtp = Path.Combine(folderTerceiros, chave + Propriedade.ExtRetorno.ProcNFe);
                        }
                        else if (ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("resNFe"))
                        {
                            FileToFtp = Path.Combine(folderTerceiros, fileRetorno2 + "-" + NSU + Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML);
                        }

                        #endregion NFe

                        #region CTe

                        else if (ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procEventoCTe"))
                        {
                            string chCTe = Functions.LerTag(((XmlElement)((XmlElement)docXML.GetElementsByTagName("eventoCTe")[0]).GetElementsByTagName("infEvento")[0]), "chCTe", false);
                            string tpEvento = Functions.LerTag(((XmlElement)((XmlElement)docXML.GetElementsByTagName("eventoCTe")[0]).GetElementsByTagName("infEvento")[0]), "tpEvento", false);
                            string nSeqEvento = Functions.LerTag(((XmlElement)((XmlElement)docXML.GetElementsByTagName("eventoCTe")[0]).GetElementsByTagName("infEvento")[0]), "nSeqEvento", false);

                            if (Empresas.Configuracoes[emp].ArqNSU)
                                FileToFtp = Path.Combine(folderTerceiros, fileRetorno2 + "-" + NSU + Propriedade.ExtRetorno.ProcEventoCTe);
                            else
                                FileToFtp = Path.Combine(folderTerceiros, chCTe + "_" + tpEvento + "_" + nSeqEvento.PadLeft(2, '0') + Propriedade.ExtRetorno.ProcEventoCTe);
                        }
                        else if (ret.ChildNodes[n].Attributes["schema"].InnerText.StartsWith("procCTe"))
                        {
                            string chave = ((XmlElement)docXML.GetElementsByTagName("infCte")[0]).GetAttribute("Id").Substring(3, 44);

                            if (Empresas.Configuracoes[emp].ArqNSU)
                                FileToFtp = Path.Combine(folderTerceiros, fileRetorno2 + "-" + NSU + Propriedade.ExtRetorno.ProcCTe);
                            else
                                FileToFtp = Path.Combine(folderTerceiros, chave + Propriedade.ExtRetorno.ProcCTe);
                        }

                        #endregion CTe

                        else
                            Auxiliar.WriteLog("LerRetornoDFe:  Nao foi possivel ler o schema", false);

                        if (FileToFtp != "")
                        {
                            if (!File.Exists(FileToFtp))
                                File.WriteAllText(FileToFtp, xmlRes);

                            string vFolder = Empresas.Configuracoes[emp].FTPPastaRetornos;
                            if (!string.IsNullOrEmpty(vFolder))
                            {
                                try
                                {
                                    Empresas.Configuracoes[emp].SendFileToFTP(FileToFtp, vFolder);
                                }
                                catch (Exception ex)
                                {
                                    ///
                                    /// grava um arquivo de erro com extensao "FTP" para diferenciar dos arquivos de erro
                                    oAux.GravarArqErroERP(Path.ChangeExtension(fileRetorno, ".ftp"), ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteLogError(Exception ex)
        {
            string extRet;

            if (vXmlNfeDadosMsgEhXML)
                extRet = ExtEnvioDFe;
            else
                extRet = ExtEnvioDFeTXT;

            try
            {
                //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                TFunctions.GravarArqErroServico(NomeArquivoXML, extRet, ExtRetEnvDFe_ERR, ex);
            }
            catch
            {
                //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                //Wandrey 09/03/2010
            }
        }
    }
}
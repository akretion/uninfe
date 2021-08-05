using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    /// <summary>
    /// Classe responsável por montar lote de vários MDFes
    /// </summary>
    public class TaskMDFeMontarLoteVarias: TaskAbst
    {
        public TaskMDFeMontarLoteVarias()
        {
            Servico = Servicos.MDFeMontarLoteVarios;
            oGerarXML.Servico = Servico;
        }

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();
            var arquivosNFe = new List<string>();

            //Aguardar a assinatura de todos os arquivos da pasta de lotes
            arquivosNFe = oAux.ArquivosPasta(Empresas.Configuracoes[emp].PastaXmlEmLote, "*" + Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML);
            if(arquivosNFe.Count == 0)
            {
                var notas = new List<ArquivoXMLDFe>();
                FileStream fsArquivo = null;
                var fluxoNfe = new FluxoNfe();

                try
                {
                    try
                    {
                        var doc = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = new FileStream(NomeArquivoXML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); //Abrir um arquivo XML usando FileStream
                        doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        var versaoXml = string.Empty;
                        var modeloDFe = string.Empty;

                        var documentoList = doc.GetElementsByTagName("MontarLoteMDFe"); //Pesquisar o elemento Documento no arquivo XML
                        foreach(XmlNode documentoNode in documentoList)
                        {
                            var documentoElemento = (XmlElement)documentoNode;

                            var QtdeArquivo = documentoElemento.GetElementsByTagName("ArquivoMDFe").Count;

                            for(var d = 0; d < QtdeArquivo; d++)
                            {
                                var arquivoNFe = Empresas.Configuracoes[emp].PastaXmlEmLote + "\\temp\\" + documentoElemento.GetElementsByTagName("ArquivoMDFe")[d].InnerText;

                                if(File.Exists(arquivoNFe))
                                {
                                    var conteudoXMLMDFe = new XmlDocument();
                                    conteudoXMLMDFe.Load(arquivoNFe);

                                    var oDadosNfe = LerXMLNFe(conteudoXMLMDFe);

                                    if(string.IsNullOrEmpty(versaoXml))
                                    {
                                        versaoXml = oDadosNfe.versao;
                                    }

                                    if(string.IsNullOrWhiteSpace(modeloDFe))
                                    {
                                        modeloDFe = oDadosNfe.mod;
                                    }

                                    if(!fluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                                    {
                                        notas.Add(new ArquivoXMLDFe() { NomeArquivoXML = arquivoNFe, ConteudoXML = conteudoXMLMDFe });
                                    }
                                    else
                                    {
                                        throw new Exception("Arquivo: " + arquivoNFe + " já está no fluxo de envio e não será incluido em novo lote.");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Arquivo: " + arquivoNFe + " não existe e não será incluido no lote!");
                                }
                            }
                        }

                        fsArquivo.Close();

                        var xmlLote = LoteNfe(notas, versaoXml, modeloDFe);
                        var mdfeRecepcao = new TaskMDFeRecepcao(xmlLote);
                        mdfeRecepcao.Execute();
                    }
                    catch
                    {
                        if(fsArquivo != null)
                        {
                            fsArquivo.Close();
                        }
                    }

                    //Deletar o arquivo de solicitão de montagem do lote de NFe
                    var oArquivo = new FileInfo(NomeArquivoXML);
                    oArquivo.Delete();
                }
                catch(Exception ex)
                {
                    try
                    {
                        TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.MontarLote).EnvioXML, Propriedade.ExtRetorno.MontarLote_ERR, ex);
                    }
                    catch
                    {
                        //Se deu algum erro na hora de gravar o arquivo de erro de retorno para o ERP, infelizmente não poderemos fazer nada
                        //pois deve estar ocorrendo alguma falha de rede, hd, permissão de acesso a pasta ou arquivos, etc. Wandrey 22/03/2010
                        //TODO: Não poderia gravar algum LOG para análise? Erro de rede normalmente é erro de IO
                    }
                }
            }
        }
    }
}
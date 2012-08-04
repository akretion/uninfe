using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    /// <summary>
    /// Executar as tarefas pertinentes a assinatura e montagem do lote de várias notas fiscais eletrônicas
    /// </summary>
    /// <param name="nfe">Objeto da classe ServicoNFe</param>
    /// <param name="arquivo">Arquivo a ser tratado</param>

    public class TaskMontarLoteVariasNFe : TaskAbst
    {
        public override void Execute()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            List<string> arquivosNFe = new List<string>();

            //Aguardar a assinatura de todos os arquivos da pasta de lotes
            arquivosNFe = oAux.ArquivosPasta(Empresa.Configuracoes[emp].PastaEnvioEmLote, "*" + Propriedade.ExtEnvio.Nfe);
            if (arquivosNFe.Count == 0) // && !Auxiliar.FileInUse(arquivo))
            {
                List<string> notas = new List<string>();
                FileStream fsArquivo = null;
                FluxoNfe fluxoNfe = new FluxoNfe();

                try
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                        fsArquivo = new FileStream(this.NomeArquivoXML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); //Abrir um arquivo XML usando FileStream
                        doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                        XmlNodeList documentoList = doc.GetElementsByTagName("MontarLoteNFe"); //Pesquisar o elemento Documento no arquivo XML
                        foreach (XmlNode documentoNode in documentoList)
                        {
                            XmlElement documentoElemento = (XmlElement)documentoNode;

                            int QtdeArquivo = documentoElemento.GetElementsByTagName("ArquivoNFe").Count;

                            for (int d = 0; d < QtdeArquivo; d++)
                            {
                                string arquivoNFe = Empresa.Configuracoes[emp].PastaEnvioEmLote + Propriedade.NomePastaXMLAssinado + "\\" + documentoElemento.GetElementsByTagName("ArquivoNFe")[d].InnerText;

                                if (File.Exists(arquivoNFe))
                                {

                                    try
                                    {
                                        DadosNFeClass oDadosNfe = this.LerXMLNFe(arquivoNFe);
                                        if (!fluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                                        {
                                            notas.Add(arquivoNFe);
                                        }
                                        else
                                        {
                                            throw new Exception("Arquivo: " + arquivoNFe + " já está no fluxo de envio e não será incluido em novo lote.");

                                            //File.Delete(arquivoNFe);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw (ex);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Arquivo: " + arquivoNFe + " não existe e não será incluido no lote!");
                                }
                            }
                        }

                        fsArquivo.Close(); //Fecha o arquivo XML

                        try
                        {
                            this.LoteNfe(notas);
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (fsArquivo != null)
                        {
                            fsArquivo.Close();
                        }

                        throw (ex);
                    }

                    //Deletar o arquivo de solicitão de montagem do lote de NFe
                    try
                    {
                        FileInfo oArquivo = new FileInfo(this.NomeArquivoXML);
                        oArquivo.Delete();
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        TFunctions.GravarArqErroServico(this.NomeArquivoXML, Propriedade.ExtEnvio.MontarLote, "-montar-lote.err", ex);
                    }
                    catch
                    {
                        //Se deu algum erro na hora de gravar o arquivo de erro de retorno para o ERP, infelizmente não poderemos fazer nada
                        //pois deve estar ocorrendo alguma falha de rede, hd, permissão de acesso a pasta ou arquivos, etc. Wandrey 22/03/2010
                    }
                }
            }
        }

        ///
        /// esta classe será acessada para chamar o metodo LoteNfe pela classe Processar.MontarLoteVariasNFe
        /// 
    }
}

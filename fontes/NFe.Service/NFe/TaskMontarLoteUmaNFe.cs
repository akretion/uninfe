using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service
{
    /// <summary>
    /// Executar as tarefas pertinentes a assinatura e montagem do lote de uma única nota fiscal eletrônica
    /// </summary>
    public class TaskNFeMontarLoteUmaNFe: TaskAbst
    {
        public TaskNFeMontarLoteUmaNFe(string arquivo)
        {
            Servico = Servicos.NFeMontarLoteUma;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public override void Execute()
        {
            try
            {
                var emp = Empresas.FindEmpresaByThread();

                var oDadosNfe = LerXMLNFe(ConteudoXML);

                AssinarValidarXMLNFe(ConteudoXML);

                //Montar lote de nfe
                var oFluxoNfe = new FluxoNfe();

                var cError = "";
                try
                {
                    if(!oFluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                    {
                        var xmlLote = LoteNfe(ConteudoXML, NomeArquivoXML, oDadosNfe.versao, oDadosNfe.mod);

                        var nfeRecepcao = new TaskNFeRecepcao(xmlLote);
                        nfeRecepcao.Execute();
                    }
                }
                catch(IOException ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                catch(Exception ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }

                if(!string.IsNullOrEmpty(cError))
                {
                    try
                    {
                        // grava o arquivo de erro
                        oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".err", cError);
                        // move o arquivo para a pasta de erro
                        oAux.MoveArqErro(NomeArquivoXML);
                    }
                    catch
                    {
                        // A principio não vou fazer nada Wandrey 24/04/2011
                    }
                }
            }
            catch { }
        }
    }
}
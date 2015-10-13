using System;
using System.Collections.Generic;
using System.Text;
//using UniNFeLibrary;
//using UniNFeLibrary.Enums;
using System.Threading;
using System.IO;

namespace UniNFeServico
{
#if x
    class ExecutaThread : UniNFeLibrary.ExecutaThread
    {
        /// <summary>
        /// Executa as thread´s dos serviços da NFe
        /// </summary>
        /// <param name="empresa">Empresa que deve ser executado o serviço</param>
        /// <param name="fullPath">Caminho e arquivo que deve ser tratado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 22/04/2011
        /// </remarks>
        protected override void AbrirThread(int empresa, string fullPath)
        {
            while (true)
            {
                lock (Auxiliar.SmfThread)
                {
                    Servicos tipoServico = Auxiliar.DefinirTipoServico(empresa, fullPath);

                    if (tipoServico == Servicos.Nulo)
                        break;

                    #region Executar a thread do serviço para processar o arquivo
                    if (ArquivoEmProcessamento.Exists(fullPath, StatusArquivoEmProcessamento.Aguardando) >= 0)
                    {
                        try
                        {
                            if (!File.Exists(fullPath))
                                break;

                            if (Auxiliar.FileInUse(fullPath))
                                continue;

                            if (File.ReadAllBytes(fullPath).Length <= 0)
                                continue;

                            ArquivoEmProcessamento.ChangeStatus(fullPath, StatusArquivoEmProcessamento.Processando);

                            ServicoUniNFe srv = new ServicoUniNFe();
                            Thread t = new Thread(new ParameterizedThreadStart(srv.ProcessaArquivo));
                            t.Name = empresa.ToString();
                            t.Start(new ParametroThread(tipoServico, fullPath));
                            break;
                        }
                        catch
                        {
                            ArquivoEmProcessamento.ChangeStatus(fullPath, StatusArquivoEmProcessamento.Aguardando);
                        }
                    }
                    #endregion
                }

                Thread.Sleep(2);
            }
        }
    }
#endif
}

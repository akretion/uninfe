using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFe.Settings;
using NFe.Service;
using NFe.Components;

namespace NFe.Threadings
{
    public class ThreadControlEvents
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public ThreadControlEvents()
        {
            ThreadItem.OnEnded += new ThreadItem.ThreadEndedHandler(ThreadItem_OnEnded);
            ThreadItem.OnReleased += new ThreadItem.ThreadReleasedHandler(ThreadItem_OnReleased);
            ThreadItem.OnStarted += new ThreadItem.ThreadStartHandler(ThreadItem_OnStarted);
        }

        #region ThreadItem_OnStarted()
        /// <summary>
        /// Evento executado quando a thread vai iniciar
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnStarted(ThreadItem item)
        {
            //danasa 12/8/2011
            //mudei de posicao e inclui o FullName
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " iniciou o processamento");

            //Servicos tipoServico = Auxiliar.DefinirTipoServico(item.Empresa, item.FileInfo.FullName);
            new Processar().ProcessaArquivo(item.Empresa, item.FileInfo.FullName);//, tipoServico);
        }
        #endregion

        #region ThreadItem_OnReleased()
        /// <summary>
        /// Evento executado quando o item é removido da fila de execução
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnReleased(ThreadItem item)
        {
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " foi descarregado da lista de processamento");

            //Se estiver reconfigurando o UniNFe, tem que reiniciar as threads
            if (item.FileInfo.FullName.IndexOf(Propriedade.ExtEnvio.AltCon_XML) >= 0 || item.FileInfo.FullName.IndexOf(Propriedade.ExtEnvio.AltCon_TXT) >= 0)
            {
                ThreadService.Stop();
                ThreadService.Start();
            }
        }
        #endregion

        #region ThreadItem_OnEnded()
        /// <summary>
        /// Evento é executado quando a thread vai finalizar
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadItem_OnEnded(ThreadItem item)
        {
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " finalizou o processamento");
        }
        #endregion
    }
}

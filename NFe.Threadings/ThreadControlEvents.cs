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
            ThreadControl.OnEnded += new ThreadControl.ThreadEndedHandler(ThreadControl_OnEnded);
            ThreadControl.OnReleased += new ThreadControl.ThreadReleasedHandler(ThreadControl_OnReleased);
            ThreadControl.OnStarted += new ThreadControl.ThreadStartHandler(ThreadControl_OnStarted);
        }

        #region ThreadControl_OnStarted()
        /// <summary>
        /// Evento executado quando a thread vai iniciar
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadControl_OnStarted(ThreadItem item)
        {
            //danasa 12/8/2011
            //mudei de posicao e inclui o FullName
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " iniciou o processamento");

            //Servicos tipoServico = Auxiliar.DefinirTipoServico(item.Empresa, item.FileInfo.FullName);
            new Processar().ProcessaArquivo(item.Empresa, item.FileInfo.FullName);//, tipoServico);
        }
        #endregion

        #region ThreadControl_OnReleased()
        /// <summary>
        /// Evento executado quando o item é removido da fila de execução
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadControl_OnReleased(ThreadItem item)
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

        #region ThreadControl_OnEnded()
        /// <summary>
        /// Evento é executado quando a thread vai finalizar
        /// </summary>
        /// <param name="item"></param>
        protected void ThreadControl_OnEnded(ThreadItem item)
        {
            Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " finalizou o processamento");
        }
        #endregion
    }
}

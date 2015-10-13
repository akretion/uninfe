using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Service
{
    public class TaskMDFeAssinarValidar : TaskAbst
    {
        public TaskMDFeAssinarValidar()
        {
            Servico = Components.Servicos.MDFeAssinarValidarEnvioEmLote;
        }

        public override void Execute()
        {
        }
    }
}

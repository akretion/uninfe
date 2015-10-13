using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Service
{
    public class TaskCTeAssinarValidar : TaskAbst
    {
        public TaskCTeAssinarValidar()
        {
            Servico = Components.Servicos.CTeAssinarValidarEnvioEmLote;
        }

        public override void Execute()
        {
        }
    }
}

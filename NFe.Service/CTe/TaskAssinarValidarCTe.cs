using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Service
{
    public class TaskAssinarValidarCTe : TaskAbst
    {
        public TaskAssinarValidarCTe()
        {
            Servico = Components.Servicos.AssinarValidarCTeEnvioEmLote;
        }

        public override void Execute()
        {
        }
    }
}

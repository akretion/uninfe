using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Service
{    
    public class TaskNFeAssinarValidar : TaskAbst
    {
        public TaskNFeAssinarValidar()
        {
            Servico = Components.Servicos.NFeAssinarValidarEnvioEmLote;
        }

        public override void Execute()
        {
        }
    }
}

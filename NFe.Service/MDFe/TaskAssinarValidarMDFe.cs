using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Service
{
    public class TaskAssinarValidarMDFe : TaskAbst
    {
        public TaskAssinarValidarMDFe()
        {
            Servico = Components.Servicos.AssinarValidarMDFeEnvioEmLote;
        }

        public override void Execute()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Service
{    
    public class TaskAssinarValidarNFe : TaskAbst
    {
        public TaskAssinarValidarNFe()
        {
            Servico = Components.Servicos.AssinarValidarNFeEnvioEmLote;
        }

        public override void Execute()
        {
        }
    }
}

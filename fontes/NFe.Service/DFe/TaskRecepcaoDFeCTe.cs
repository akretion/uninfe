using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    public class TaskDFeRecepcaoCTe : TaskDFeRecepcao
    {
        public TaskDFeRecepcaoCTe(string arquivo)
            : base(arquivo)
        {
            Servico = Servicos.CTeDistribuicaoDFe;
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}
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
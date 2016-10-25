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
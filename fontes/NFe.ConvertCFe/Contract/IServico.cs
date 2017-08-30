namespace NFe.SAT.Contract
{
    internal interface IServico
    {
        string ArquivoXML { get; set; }

        void Enviar();

        string SaveResponse();
    }
}
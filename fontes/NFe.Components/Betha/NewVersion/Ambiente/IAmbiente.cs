using NFe.Components.Abstract;

namespace NFe.Components.Betha.NewVersion.Ambiente
{
    public interface IAmbiente : IEmiteNFSe
    {
        string NameSpaces { get; }

        void CancelarNfse(string file);
        void ConsultarLoteRps(string file);
        void ConsultarNfse(string file);
        void ConsultarNfsePorRps(string file);
        void ConsultarSituacaoLoteRps(string file);
        void EmiteNF(string file);
        void EmiteNFSincrono(string file);
    }
}
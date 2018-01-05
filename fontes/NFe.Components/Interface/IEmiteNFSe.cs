using System.Net;

namespace NFe.Components
{
    public interface IEmiteNFSe
    {
        void EmiteNF(string file);
        string EmiteNF(string file, bool cancelamento);
        void CancelarNfse(string file);
        void ConsultarLoteRps(string file);
        void ConsultarSituacaoLoteRps(string file);
        void ConsultarNfse(string file);
        void ConsultarNfsePorRps(string file);
        void GerarRetorno(string file, string result, string extEnvio, string extRetorno);

        object WSGeracao { get; }
        object WSConsultas { get; }
        string Usuario { get; set; }
        string Senha { get; set; }
        int Cidade { get; set; }
        IWebProxy Proxy { get; set; }
    }
}
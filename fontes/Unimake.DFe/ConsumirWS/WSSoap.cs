namespace Unimake.DFe
{
    public class WSSoap
    {
        private string _EnderecoWeb = "";
        public string EnderecoWeb { get => _EnderecoWeb; set => _EnderecoWeb = value; }

        private string _ActionWeb = "";
        public string ActionWeb  { get => _ActionWeb; set => _ActionWeb = value; }

        private string _VersaoSoap = "soap12";
        public string VersaoSoap { get => _VersaoSoap; set => _VersaoSoap = value; }

        private string _TagRetorno = "nfeResultMsg";
        public string TagRetorno  { get => _TagRetorno; set => _TagRetorno = value; }
    }
}

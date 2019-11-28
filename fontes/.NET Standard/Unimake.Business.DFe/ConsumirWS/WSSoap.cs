namespace Unimake.Business.DFe
{
    public class WSSoap
    {
        #region Private Fields

        private string _ActionWeb;
        private string _ContentType;
        private string _EnderecoWeb;
        private string _SoapString;
        private string _TagRetorno;
        private string _VersaoSoap;

        #endregion Private Fields

        #region Public Properties

        public string ActionWeb
        {
            get => _ActionWeb;
            set => _ActionWeb = value;
        }

        public string ContentType
        {
            get => string.IsNullOrWhiteSpace(_ContentType) ? (_ContentType = "application/soap+xml; charset=utf-8;") : _ContentType;
            set => _ContentType = value;
        }

        public string EnderecoWeb
        {
            get => _EnderecoWeb;
            set => _EnderecoWeb = value;
        }

        public string SoapString
        {
            get => _SoapString;
            set => _SoapString = value;
        }

        public string TagRetorno
        {
            get => string.IsNullOrWhiteSpace(_TagRetorno) ? (_TagRetorno = "nfeResultMsg") : _TagRetorno;
            set => _TagRetorno = value;
        }

        public string VersaoSoap
        {
            get => string.IsNullOrWhiteSpace(_VersaoSoap) ? (_VersaoSoap = "soap12") : _VersaoSoap;
            set => _VersaoSoap = value;
        }

        #endregion Public Properties
    }
}
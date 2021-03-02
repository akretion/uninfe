using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.GeisWeb
{
    public abstract class GeisWebBase: EmiteNFSeBase
    {
        #region locais/ protegidos

        private int CodigoMun = 0;
        private string ProxyUser = "";
        private string ProxyPass = "";
        private string ProxyServer = "";
        private X509Certificate2 Certificado;
        private EmiteNFSeBase geisWebService;

        protected EmiteNFSeBase GeisWebService
        {
            get
            {
                if(geisWebService == null)
                {
                    if(tpAmb == TipoAmbiente.taHomologacao)
                    {
                        geisWebService = new ItatingaSP.h.GeisWebH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                    }
                    else
                    {
                        switch(CodigoMun)
                        {
                            case 3523503: //Itatinga - SP
                                geisWebService = new ItatingaSP.p.GeisWebP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    }
                }

                return geisWebService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public GeisWebBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
            Certificado = certificado;
        }

        #endregion Construtores

        #region Métodos

        public override void EmiteNF(string file) => GeisWebService.EmiteNF(file);

        public override void CancelarNfse(string file) => GeisWebService.CancelarNfse(file);

        public override void ConsultarLoteRps(string file) => GeisWebService.ConsultarLoteRps(file);

        public override void ConsultarSituacaoLoteRps(string file) => GeisWebService.ConsultarSituacaoLoteRps(file);

        public override void ConsultarNfse(string file) => GeisWebService.ConsultarNfse(file);

        public override void ConsultarNfsePorRps(string file) => GeisWebService.ConsultarNfsePorRps(file);

        #endregion Métodos
    }
}
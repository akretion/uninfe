using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Elotech
{
    public abstract class ElotechBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        private readonly int CodigoMun = 0;
        private readonly string UsuarioProxy = "";
        private readonly string SenhaProxy = "";
        private readonly string DomainProxy = "";
        private readonly X509Certificate Certificado = null;
        private EmiteNFSeBase elotechService;

        protected EmiteNFSeBase ElotechService
        {
            get
            {
                if (elotechService == null)
                {
                    switch (CodigoMun)
                    {
                        case 4113502: //Loanda-PR
                            elotechService = new LoandaPR(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                            break;

                        case 4110706: //Irati-PR
                            elotechService = new IratiPR(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                            break;

                        case 4114807: //Marialva-PR
                            elotechService = new MarialvaPR(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                            break;

                        default:
                            throw new Exceptions.ServicoInexistenteException();
                    }
                }

                return elotechService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public ElotechBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            UsuarioProxy = usuarioProxy;
            SenhaProxy = senhaProxy;
            DomainProxy = domainProxy;
            Certificado = certificado;
        }

        #endregion Construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            ElotechService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            ElotechService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            ElotechService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ElotechService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            ElotechService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ElotechService.ConsultarNfsePorRps(file);
        }

        #endregion Métodos
    }
}
using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Pronin
{
    public abstract class ProninBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        private int CodigoMun = 0;
        private string UsuarioProxy = "";
        private string SenhaProxy = "";
        private string DomainProxy = "";
        private X509Certificate Certificado = null;
        private EmiteNFSeBase proninService;

        protected EmiteNFSeBase ProninService
        {
            get
            {
                if (proninService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 4109401: //Guarapuava-PR
                                proninService = new GuarapuavaPR.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3131703: //Itabira-MG
                                proninService = new ItabiraMG.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;
                            case 4303004: //Cachoeira do Sul-RS
                                proninService = new CachoeiraSulRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4322509: //Vacari-RS
                                proninService = new VacariRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 4109401: //Guarapuava-PR
                                proninService = new GuarapuavaPR.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3131703: //Itabira-MG
                                proninService = new ItabiraMG.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4303004: //Cachoeira do Sul-RS
                                proninService = new CachoeiraSulRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4322509: //Vacari-RS
                                proninService = new VacariRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return proninService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public ProninBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
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
            ProninService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            ProninService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            ProninService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ProninService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            ProninService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ProninService.ConsultarNfsePorRps(file);
        }

        #endregion Métodos
    }
}